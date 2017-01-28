using UnityEngine;
using System.Collections;

public class GameData
{
	public readonly static GameData Instance = new GameData();

	private int score = 0;

	public void InitializeScore()
	{
		score = 0;
	}

	public void AddScore( int add = 1 )
	{
		score += add;
	}

	public int GetScore()
	{
		return score;
	}

}