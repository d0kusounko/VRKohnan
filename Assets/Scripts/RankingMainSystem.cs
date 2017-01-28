using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingMainSystem : MonoBehaviour
{
	public GameObject scoreBoard			= null;
	public GameObject rankingBorad			= null;
	public GameObject objNameInputSystem    = null;

	// URIスキーマ.
	public string	uriScheme	= "http://";
    // URIホスト.
	public string	uriHost		= "160.16.211.175";
	//! アプリケーションパス.
	public string	uriAppPath	= "VRKohnan/index.php";

    // リクエストタイムアウト時間.
    public float	reqTimeOutSec = 20.0f;
    private float	reqTime = 0.0f;

    // ランキング取得数.
    public uint		getRankingNum	= 10;

    // HTTP通信ステータス.
	private enum HttpState
	{
		None,
		Busy,
		Success,
		Error,
		Timeout,
	};

	// HTTP通信オブジェクト.
	private WWW www	= null;

	// ダミー通信待ち時間.
	public float dummyHttpWaitTime		= 1.0f;
	private bool dummytimeCountEnable	= false;
	private float dummytimeCount		= 0.0f;
 
	private enum State
	{
		None,
		NameInput,
		WaitDummy,
		RankingRegister,
		RankingGet,
		RankingView,
	};
	private State state;

	// Use this for initialization
	void Start ()
	{
		SetScoreBoardEnable( true );
		SetRankingBoardEnable( false );

		state	= State.NameInput;
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch( state )
		{
			case State.NameInput:
				{
					// 名前决定待ち.
					if( objNameInputSystem.GetComponent<NameInputSystem>().IsDecide() )
					{
						// TODO: 决定SE.

						// ランキング登録データ作成.
						byte[] post_data = null;
						{
							byte[]	data_score	= System.BitConverter.GetBytes( GameData.Instance.GetScore() );
							string	name_str	= objNameInputSystem.GetComponent<NameInputSystem>().GetName();
							byte[]	data_name	= System.Text.Encoding.ASCII.GetBytes( name_str );

							List<byte> post_data_list  = new List<byte>( data_score.Length + data_name.Length );
							post_data_list.AddRange( data_score );
							post_data_list.AddRange( data_name );

							post_data = post_data_list.ToArray();
						}

						// ランキング登録リクエスト開始.
						HttpReqestPost( "register_ranking", post_data );

						// ロードマーク表示.
						objNameInputSystem.transform.FindChild( "LoadingMark" ).GetComponent< LoadingMark >().SetEnable( true ); 

						StartDummyTime();

						state = State.RankingRegister;
					}
					break;
				}

			case State.RankingRegister:
			{
				HttpState result	= HttpReceiveResponse();
				if( result != HttpState.Busy )
				{
					if( result == HttpState.Success )
					{
						byte[] resData = GetHttpResponseBinaryData();
						// ランキング登録完了チェック.
						bool registered	= false;
						if( resData.Length > 1 )
						{
							if( resData[1] == 1 )
							{
								registered	= true;
							}
						}

						if( registered == false )
						{
							// TODO: 失敗処理.
						}
					}
					else
					{
						// TODO: 失敗処理.
					}

					// ランキング取得データ作成.
					byte[] post_data = null;
					{
						byte[]	data_get_num	= System.BitConverter.GetBytes( getRankingNum );

						List<byte> post_data_list  = new List<byte>( data_get_num.Length );
						post_data_list.AddRange( data_get_num );

						post_data = post_data_list.ToArray();
					}
					// ランキング取得リクエスト開始.
					HttpReqestPost( "get_ranking", post_data );

					state	= State.WaitDummy;
				}
				break;
			}

			case State.WaitDummy:
			{
				// ダミー待ち時間.
				if( IsDummyTimeEnd() )
				{
					// ランキング取得へ.
					state = State.RankingGet;
				}
				break;
			}

			case State.RankingGet:
			{
				HttpState result	= HttpReceiveResponse();
				if( result != HttpState.Busy )
				{
					if( result == HttpState.Success )
					{
						byte[] resData = GetHttpResponseBinaryData();

						// ランキングデータから表示する文字列作成.
						string ranking_str	= "";
						if( resData.Length > 0 )
						{
							int byte_idx		= 0;
							int ranking_numeric	= 1;

							// ランキング取得個数分処理.
							uint ranking_length = System.BitConverter.ToUInt32( resData, byte_idx );
							byte_idx += 4;
							for( uint i = 0; i < ranking_length; ++i )
							{
								int	score	= System.BitConverter.ToInt32( resData, byte_idx );
								byte_idx += 4;

								string name = "";
								for( int name_i = 0; name_i < 3; ++name_i )
								{
									byte[] name_one = new byte[] { resData[ byte_idx ] };
									byte_idx += 1;
									name += System.Text.Encoding.ASCII.GetString( name_one );
								}

								ranking_str	+= ranking_numeric.ToString() + ". " + name + ": " + score.ToString() + "\n";
								++ranking_numeric;
							}
						}

						ranking_str	+= "Press [Space] key to restart.";

						SetRankingBoardEnable( true );
						SetTextRankingBoard( ranking_str );
						}
					else
					{
						// TODO: 失敗処理.
					}

					// 名前入力UI無効.
					objNameInputSystem.SetActive( false );

					state	= State.RankingView;
				}
				break;
			}

			case State.RankingView:
			{
				// スペースキーでゲームリスタート.
				if( Input.GetKey( KeyCode.Space ) )
				{
					SceneManager.LoadScene( "Main" );
				}
				break;
			}
		}

		UpdateDummyTime();
	}

	private void SetScoreBoardEnable( bool enable )
	{
		if( scoreBoard )
		{
			ScoreBoard text = scoreBoard.GetComponent<ScoreBoard>();
			if( text )
			{
				text.SetVisible( enable );
			}
		}
	}

	private void SetRankingBoardEnable( bool enable )
	{
		if( rankingBorad )
		{
			rankingBorad.SetActive( enable );
		}
	}

	private void SetTextRankingBoard( string set_text )
	{
		if( rankingBorad )
		{
			Text text = rankingBorad.transform.FindChild( "Text" ).GetComponent< Text >();
			if( text )
			{
				text.text	= set_text;
			}
		}
	}

	private void HttpReqestPost( string api, byte[] data )
	{
        WWWForm form = new WWWForm();
        form.AddField( "c", "Game" );
        form.AddField( "act", api );
        form.AddField( "data", System.Text.Encoding.ASCII.GetString( data ) );
        // form.AddBinaryData( "data", data );

        string url 	= uriScheme + uriHost + "/" + uriAppPath;
		www = new WWW( url, form );

		// タイムアウトカウント初期化.
		reqTime = reqTimeOutSec;
	}

    // HTTPレスポンス受信.
	private HttpState HttpReceiveResponse()
	{
		if( www == null )
		{
			return HttpState.None;
		}

        if( www.error != null )
        {
        	// エラー.
            Debug.Log( "Http NG: " + www.error );
            return HttpState.Error;
        }
        else if( www.isDone )
        {
			// サーバからのレスポンスを表示.
			Debug.Log( "Http OK Resp Text: " + www.text );
			Debug.Log( "Http OK Resp Bin: " + System.BitConverter.ToString( www.bytes ).Replace( "-", string.Empty ) );
			return HttpState.Success;
        }

		reqTime -= Time.deltaTime;
        if( reqTime < 0.0f )
        {
        	// タイムアウト.
            Debug.Log( "Http Timeout" );
        	return HttpState.Timeout;
        }

        return HttpState.Busy;
	}

	// HTTPレスポンスバイナリデータ取得.
	private byte[] GetHttpResponseBinaryData()
	{
        if( www.isDone )
        {
        	return www.bytes;
        }
        return null;
	}

	// ダミータイムカウント開始.
	void StartDummyTime()
	{
		dummytimeCountEnable	= true;
		dummytimeCount			= 0.0f;
	}

	// ダミータイム更新.
	void UpdateDummyTime()
	{
		if( !dummytimeCountEnable )
		{
			return;
		}

		if( dummytimeCount < dummyHttpWaitTime )
		{
			dummytimeCount	+= Time.deltaTime;
		}
	}

	// ダミータイム終了判定.
	bool IsDummyTimeEnd()
	{
		return ( dummytimeCount >= dummyHttpWaitTime );
	}
}
