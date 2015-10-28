using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	#region Events
	public delegate void OnIncreaseSpeedHandler ();
	public event OnIncreaseSpeedHandler onIncreaseSpeed;
	public delegate void OnGameOver();
	public event OnGameOver onGameOver;
	#endregion

	private static GameManager instance = null;
	private GUIManager guiManager = null;

	public GameObject obstacleInstantiator = null;
	public Obstacle obstaclePrefab = null;
	private int score = 0;
	private int distance = 0;
	private int lives = 3;
	private int previousPosition = 0;
	public GameObject startPoint = null;
	private float speed = 5.0f;

	public static List<Obstacle> obstacles = new List<Obstacle>();
	public PlayerController player = null;
	public PlayerController playerPrefab = null;

	public float obstacleFrequency = 1.0f;
	public float changeColorFrequency = 2.0f;
	
	void Awake()
	{
		instance = this;
		player = instantiatePlayer ();
		player.onPlayerDeath += this.Reset;
		this.onIncreaseSpeed += this.IncreaseDifficulty;
		guiManager = GUIManager.GetInstance ();

		// Don't destroy
		DontDestroyOnLoad (this);
	}
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (instantiateObstacle (obstacleFrequency));
		StartCoroutine (ChangeObstacleColor (changeColorFrequency));
	}
	
	// Update is called once per frame
	void Update () 
	{
		CalculateDistance ();
		CalculateScore ();
		DisplayData ();
	}
	
	public static GameManager GetInstance()
	{
		return instance;
	}

	public float GetSpeed()
	{
		return speed;
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
		guiManager.UpdateDistance (distance.ToString());
		guiManager.UpdateScore (score.ToString());
		guiManager.UpdateLives (lives.ToString ());
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
		//StartCoroutine (Reset ());
	}

	private void Reset()
	{
		//yield return new WaitForSeconds (5);

		// Instantiate a new player.
		player = instantiatePlayer ();

		//Subscribe it to the new player.
		player.onPlayerDeath += this.Reset;

		// Destroy all obstacles.
		foreach (Obstacle obstacle in obstacles) 
			Destroy (obstacle.gameObject);

		// Delete obstacles from list.
		obstacles.Clear ();

		// Reset variables.
		distance = 0;
		score = 0;
		speed = 5;

		if(lives >= 0)
			lives--;
		else
			Debug.Log ("GAME OVER");

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

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
	}





}