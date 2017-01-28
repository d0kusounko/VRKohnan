using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMainSystem : MonoBehaviour
{
	public GameObject scoreBoard = null;
	public GameObject timeBoard = null;
	public GameObject endBoard = null;
	public int gameTimeToSelect = 60;
	public int gameEndWaitTimeToSelect = 3;

	private CountDownTimer countDownTimer = null;
	private LookMove lookMove = null;
	private float gameTime			= 0;
	private float gameEndWaitTime	= 0;
	private AudioSource bgm;
	private AudioSource seGameEnd;

	private enum State
	{
		CountDownStart,
		CountDown,
		Game,
		GameEnd,
	};
	private State   state;

	// Use this for initialization
	void Start()
	{
		GameObject countDownTimerObj = GameObject.Find( "CountDownTimer" );
		if( countDownTimerObj )
		{
			countDownTimer	= countDownTimerObj.GetComponent<CountDownTimer>();
		}

		GameObject lookMoveObj = GameObject.Find( "MeMyselfEye" );
		if( lookMoveObj )
		{
			lookMove = lookMoveObj.GetComponent<LookMove>();
		}

		// サウンド設定.
		AudioSource[] audioSources = GetComponents<AudioSource>();
		bgm			= audioSources[ 0 ];
		seGameEnd	= audioSources[ 1 ];

		GameData.Instance.InitializeScore();
		state = State.CountDownStart;
	}

	// Update is called once per frame
	void Update()
	{
		switch( state )
		{
			case State.CountDownStart:
			{
				// ゲーム開始のカウントダウン開始.
				countDownTimer.Execute();
				state = State.CountDown;
				break;
			}

			case State.CountDown:
			{
				if( countDownTimer.IsEnd() )
				{
					lookMove.Execute();
					bgm.Play();
					ScoreBoardEnable();
					TimeBoardEnable();
					state = State.Game;
					gameTime = gameTimeToSelect;
				}
				break;
			}
				
			case State.Game:
			{
				// ゲームカウント.
				gameTime -= Time.deltaTime;
				if( gameTime < 0.0 )
				{
					// ゲーム終了.
					gameTime = 0;
					gameEndWaitTime = gameEndWaitTimeToSelect;
					EndBoardEnable();
					seGameEnd.Play();
					state = State.GameEnd;
				}
				break;
			}

			case State.GameEnd:
			{
				// 終了画面表示待ち.
				gameEndWaitTime -= Time.deltaTime;
				if( gameEndWaitTime < 0.0 )
				{
					// ランキング登録へ.
					SceneManager.LoadScene( "Ranking" );
				}
				break;
			}
		}

		// ゲームリセット.
		if( Input.GetKey( KeyCode.Space ) )
		{
			SceneManager.LoadScene( "Main" );
		}
	}

	public bool IsGamePlaying()
	{
		return state == State.Game;
	}

	public float GetGameTime()
	{
		return gameTime;
	}

	private void ScoreBoardEnable()
	{
		if( scoreBoard )
		{
			ScoreBoard text = scoreBoard.GetComponent<ScoreBoard>();
			if( text )
			{
				text.SetVisible( true );
			}
		}
	}

	private void TimeBoardEnable()
	{
		if( timeBoard )
		{
			TimeBoard text = timeBoard.GetComponent<TimeBoard>();
			if( text )
			{
				text.SetVisible( true );
			}
		}
	}

	private void EndBoardEnable()
	{
		if( endBoard )
		{
			EndBoard text = endBoard.GetComponent<EndBoard>();
			if( text )
			{
				text.SetVisible( true );
			}
		}
	}
}
