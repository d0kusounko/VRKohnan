using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBoard : MonoBehaviour
{
	private Text timeText;
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

		timeText = transform.FindChild( "Text" ).gameObject.GetComponent<Text>();
		timeText.text = "";
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
			// ゲームタイム表示.
			timeText.text = "Time: " + gameMainSystem.GetGameTime().ToString( "F0" );
		}
		else
		{
			timeText.text = "";
		}
	}
}
