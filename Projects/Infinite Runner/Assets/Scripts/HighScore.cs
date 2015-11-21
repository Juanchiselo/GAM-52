using UnityEngine;
using System.Collections;

public class HighScore
{
	private string name = null;
	private int score = 0;

	public HighScore(string name, int score)
	{
		this.name = name;
		this.score = score;
	}

	public void SetName(string name)
	{
		this.name = name;
	}

	public string GetName()
	{
		return name;
	}

	public void SetScore(int score)
	{
		this.score = score;
	}

	public int GetScore()
	{
		return score;
	}
}
