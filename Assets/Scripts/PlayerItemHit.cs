using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHit : MonoBehaviour
{
	public GameObject effectItemGet;
	public GameObject effectVodafonExplosion;

	private GameMainSystem gameMainSystem;
	private AudioSource seItemGet;
	private AudioSource seVodafonExplosion;

	// Use this for initialization
	void Start ()
	{
		// ゲームメイン処理取得.
		GameObject gameMainSystemObj = GameObject.Find( "GameMainSystem" );
		if( gameMainSystemObj )
		{
			gameMainSystem = gameMainSystemObj.GetComponent<GameMainSystem>();
		}

		// SE設定.
		AudioSource[] audioSources = GetComponents<AudioSource>();
		seItemGet = audioSources[ 0 ];
		seVodafonExplosion = audioSources[ 1 ];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter( Collision collision )
	{
		// 衝突チェックはゲーム中のみ.
		if( !gameMainSystem.IsGamePlaying() )
		{
			return;
		}

		// コーナンロゴと衝突.
		if( collision.gameObject.CompareTag( "ItemKohnan" ) )
		{
			// 衝突可能な時のみ.
			ItemKohnan itemKohnan = collision.gameObject.GetComponent<ItemKohnan>();
			if( itemKohnan.IsHitEnable() )
			{
				// スコア加算.
				GameData.Instance.AddScore();

				// SE再生.
				seItemGet.Play();

				// エフェクト再生.
				GameObject effect = Instantiate( effectItemGet, collision.transform.position, collision.transform.rotation ) as GameObject;
				Destroy( effect, 1.0f );

				// コーナンロゴの削除.
				Destroy( collision.gameObject );
			}
		}
		// ボーダフォンロゴと衝突.
		else if( collision.gameObject.CompareTag( "ItemVodafon" ) )
		{
			// 衝突可能な時のみ.
			ItemVodafon itemVodafon = collision.gameObject.GetComponent<ItemVodafon>();
			if( itemVodafon.IsHitEnable() )
			{
				// SE再生.
				seVodafonExplosion.Play();

				// エフェクト再生.
				GameObject effect = Instantiate( effectVodafonExplosion, collision.transform.position, collision.transform.rotation ) as GameObject;
				Destroy( effect, 10.0f );

				// ボーダフォンロゴの削除.
				Destroy( collision.gameObject );
			}

		}
	}


}
