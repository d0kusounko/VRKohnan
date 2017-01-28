using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
	private Text scoreText;
	private bool isVisible  = false;

	// Use this for initialization
	void Start()
	{
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
			scoreText.text = "Score: " + GameData.Instance.GetScore().ToString( "D" );
		}
		else
		{
			scoreText.text = "";
		}
	}
}
