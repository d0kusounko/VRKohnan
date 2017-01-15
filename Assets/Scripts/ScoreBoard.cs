using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
	private Text scoreText;
	private bool isVisible  = false;
	private GameMainSystem gameMainSystem;

	// Use this for initialization
	void Start()
	{
		// ゲームメイン処理取得.
		GameObject gameMainSystemObj = GameObject.Find( "GameMainSystem" );
		if( gameMainSystemObj )
		{
			gameMainSystem = gameMainSystemObj.GetComponent<GameMainSystem>();
		}

		scoreText = transform.FindChild( "Text" ).gameObject.GetComponent<Text>();
		scoreText.text = "";
	}

	public void SetVisible( bool visible )
	{
		isVisible = visible;
		ApplyText();
	}

	// Update is called once per frame
	void Update()
	{
		ApplyText();
	}

	private void ApplyText()
	{
		if( isVisible )
		{
			// スコア表示.
			scoreText.text = "Score: " + gameMainSystem.GetScore().ToString( "D" );
		}
		else
		{
			scoreText.text = "";
		}
	}
}
