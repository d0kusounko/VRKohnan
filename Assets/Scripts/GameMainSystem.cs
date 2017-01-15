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

	private CountDownTimer countDownTimer = null;
	private LookMove lookMove = null;
	private int gameScore = 0;
	private float gameTime = 0;
	private AudioSource bgm;

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
		bgm = audioSources[ 0 ];

		gameScore = 0;
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
					EndBoardEnable();
					state = State.GameEnd;
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

	public void AddGameScore( int add = 1 )
	{
		gameScore += add;
	}

	public int GetScore()
	{
		return gameScore;
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
