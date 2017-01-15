using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
	public Text countDownText;
	public int countToSelect  = 5;
	private int count;
	private float timeLeft;
	private bool isExecute	= false;
	private AudioSource countDownSE;
	private AudioSource countEndSE;

	// Use this for initialization
	void Start ()
	{
		countDownText.text = "";
		AudioSource[] audioSources = GetComponents<AudioSource>();
		countDownSE = audioSources[ 0 ];
		countEndSE = audioSources[ 1 ];
	}

	public void Execute()
	{
		isExecute = true;
		count = countToSelect;
		timeLeft = 1.0f;
		ApplyText();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( !isExecute )
		{
			return;
		}

		if( !IsEnd() )
		{
			timeLeft -= Time.deltaTime;
			// 1秒毎の処理.
			if( timeLeft <= 0.0f )
			{
				timeLeft = 1.0f;
				--count;

				// SE.
				if( !IsEnd() )
				{
					// カウントダウン中.
					countDownSE.Play();
				}
				else
				{
					// カウントダウン終了.
					countEndSE.Play();
				}
			}
		}
		ApplyText();
	}

	void ApplyText()
	{
		if( IsEnd() )
		{
			countDownText.text = "";
		}
		else
		{
			countDownText.text = count.ToString( "D" );
		}
	}

	public bool IsEnd()
	{
		return ( count <= 0 );
	}
}
