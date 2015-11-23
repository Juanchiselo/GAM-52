using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
//	public List<GameState> states = new List<GameState>();
//	public StateMachineEngine stateMachine = null;

	#region Events
	public delegate void OnIncreaseSpeedHandler ();
	public event OnIncreaseSpeedHandler onIncreaseSpeed;
	public delegate void OnReset();
	public event OnReset onReset;
	#endregion

	public static GameManager Instance { get; private set; }

	public GameObject obstacleInstantiator = null;
	public Obstacle obstaclePrefab = null;
	private int score = 0;
	private int distance = 0;
	private int lives = 3;
	private int previousPosition = 0;
	public GameObject startPoint = null;
	private float speed = 5.0f;

	public static List<Obstacle> obstacles = new List<Obstacle>();
	public static List<HighScore> highScores = new List<HighScore> ();
	public PlayerController player = null;
	public PlayerController playerPrefab = null;

	public float obstacleFrequency = 1.0f;
	public float changeColorFrequency = 2.0f;

	public string playerName = null;
	
	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy (gameObject);
		
		Instance = this;
		
		DontDestroyOnLoad (gameObject);
	}

	void OnLevelWasLoaded(int index)
	{
		switch (index) 
		{
		case 0:
			break;
		case 1:
			obstacleInstantiator = GameObject.Find("Obstacle Instantiator");
			//obstaclePrefab = GameObject.Find("Color Cube Obstacle").GetComponent<Obstacle>();
			obstaclePrefab = Resources.Load("Prefabs/Color Cube Obstacle", typeof(Obstacle)) as Obstacle;
			startPoint = GameObject.Find("Start Point");
			//playerPrefab = GameObject.Find("AnimatedPlayer").GetComponent<PlayerController>();
			playerPrefab = Resources.Load("Prefabs/AnimatedPlayer", typeof(PlayerController)) as PlayerController;
			player = instantiatePlayer ();
			player.onPlayerDeath += this.PlayerDeath;
			this.onIncreaseSpeed += this.IncreaseDifficulty;
			//guiManager = GUIManager.GetInstance ();
			StartCoroutine (instantiateObstacle (obstacleFrequency));
			StartCoroutine (ChangeObstacleColor (changeColorFrequency));
			break;
		default:
			Debug.Log ("ERROR: That is not a valid level index.");
			break;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player) 
		{
			CalculateDistance ();
			CalculateScore ();
			DisplayData ();
		}
	}

	public float GetSpeed()
	{
		return speed;
	}

	public string GetPlayerName()
	{
		return playerName;
	}

	public void SetPlayerName(string playerName)
	{
		this.playerName = playerName;
	}

	public PlayerController GetPlayer()
	{
		return player;
	}

	public List<Obstacle> GetObstaclesList()
	{
		return obstacles;
	}

	public void DisplayData()
	{
		GUIManager.Instance.UpdateDistance (distance.ToString());
		GUIManager.Instance.UpdateScore (score.ToString());
	}

	private void CalculateDistance()
	{
		int currentPosition = (int)player.transform.position.x;

		int deltaDistance = currentPosition - previousPosition;

		if(deltaDistance >= 1)
			distance += deltaDistance;

		previousPosition = currentPosition;
	}

	private void CalculateScore()
	{
		score = distance * 3;


//		foreach (Obstacle obstacle in obstacles) 
//		{
//			if (obstacle.transform.position.x
//				< player.transform.position.x) 
//			{
//				obstacle.isPassed = true;
//
//				if(obstacle.isPassed == true)
//				{
//					Debug.Log ("Obstacle #" + obstacles.IndexOf(obstacle)
//					           + " has been passed!");
//					score++;
//				}
//
//			}
//		}
	}

	private PlayerController instantiatePlayer()
	{
		player = Instantiate (playerPrefab, startPoint.transform.position,
		                      startPoint.transform.rotation) as PlayerController;
		return player;
	}



	private IEnumerator instantiateObstacle(float frequency)
	{
		yield return new WaitForSeconds(frequency);
		Obstacle obstacleInstance;
		obstacleInstance = Instantiate (obstaclePrefab, obstacleInstantiator.transform.position, 
		             obstacleInstantiator.transform.rotation) as Obstacle;

		StartCoroutine (instantiateObstacle (frequency));
	}

	private IEnumerator ChangeObstacleColor(float frequency)
	{
		yield return new WaitForSeconds (frequency);

		foreach (Obstacle obstacle in obstacles) 
		{
			obstacle.ChangeColor();
		}

		onIncreaseSpeed ();

		StartCoroutine (ChangeObstacleColor (changeColorFrequency));
	}

	private void PlayerDeath()
	{
		PauseGame ();
		GUIManager.Instance.UpdateFinalScore (score.ToString ());
		SetHighScore ();
		GUIManager.Instance.ShowPlayAgainMenu ();
	}

	public void Reset()
	{
		// Reset variables.
		distance = 0;
		score = 0;
		speed = 5;

		GUIManager.Instance.playAgainMenu.gameObject.SetActive (false);
		ResumeGame ();

		// Instantiate a new player.
		player = instantiatePlayer ();

		//Subscribe it to the new player.
		player.onPlayerDeath += this.PlayerDeath;

		onReset ();

		// Destroy all obstacles.
		foreach (Obstacle obstacle in obstacles) 
			Destroy (obstacle.gameObject);

		// Delete obstacles from list.
		obstacles.Clear ();



		// If the obstacle is out of camera and behind
		// the player, destroy it.
		foreach (Obstacle obstacle in obstacles) 
		{
			if (!obstacle.GetComponent<Renderer> ().isVisible
					&& (obstacle.transform.position.x
					< player.transform.position.x)) 
			{
					obstacles.Remove (obstacle);
					Destroy (obstacle.gameObject);
			}
		}
	}

	private void IncreaseDifficulty()
	{
		speed += 1;
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
	}

	public void SetHighScore()
	{
		// Add the player to the highScores list.
		if (highScores.Count == 0)
			highScores.Add (new HighScore (playerName, score));
		else 
		{
			int insertIndex = 0;

			foreach (HighScore highScore in highScores) 
			{
				if(score >= highScore.GetScore())
				{
					insertIndex = highScores.IndexOf(highScore);
					break;
				}
				else if(score < highScore.GetScore())
					insertIndex = highScores.IndexOf(highScore) + 1;
			}

			// Insert score
			highScores.Insert(insertIndex, new HighScore(playerName, score));

			// If there are more than 5 high scores, then remove
			// the last high score from the list.
			if(highScores.Count == 6)
				highScores.Remove(highScores[5]);
		}

		int position = 1;

		foreach (HighScore highScore in highScores)
		{
			print ("#" + position + " - " + highScore.GetName ()
				+ " _ " + highScore.GetScore ());

			position++;
		}
	}





}

//public enum GameState()
//{
//	NONE,
//	RUNNING,
//	PAUSE
//};