using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	private static GameManager instance = null;

	public GameObject obstacleInstantiator = null;
	public Obstacle obstaclePrefab = null;
	public Canvas canvas = null;	
	private int score = 0;
	private int distance = 0;
	private int previousDistance = 0;
	private int previousTime = 0;
	private int currentTime = 0;
	private Text distanceGUI = null;
	public GameObject startPoint = null;
	public CameraController camera = null;
	public float playerSpeed = 5.0f;

	public static List<Obstacle> obstacles = new List<Obstacle>();
	public PlayerController player = null;
	public PlayerController playerPrefab = null;
	
	void Awake()
	{
		instance = this;
		player = instantiatePlayer ();

	}
	
	// Use this for initialization
	void Start () 
	{
		distanceGUI = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player == null) 
		{
			Invoke ("Reset", 5.0f);
			//Reset ();
		}

		currentTime = Mathf.RoundToInt (Time.time);
		
		if (currentTime % 2 == 0 && previousTime != currentTime)
			ChangeObstacleColor ();

		if (distance % 5 == 0 && previousDistance != distance)
			instantiateObstacle ();

		//DisplayData ();
		
		previousTime = currentTime;
		previousDistance = distance;

		CalculateScore ();
		CleanUp ();
	}
	
	public static GameManager GetInstance()
	{
		return instance;
	}

	public void SetScore(int score)
	{
		this.score = score;
	}

	public int GetScore()
	{
		return score;
	}

	public void SetDistance(int distance)
	{
		this.distance = distance;
	}

	public int GetDistance()
	{
		return distance;
	}

	public List<Obstacle> GetObstaclesList()
	{
		return obstacles;
	}

	private void DisplayData()
	{
		//distanceGUI.text = "Distance: " + distance;
	}

	private void OnGUI()
	{
		GUI.Label (new Rect (10, 10, 100, 20), "Score: " + score);
		GUI.Label (new Rect (10, 30, 100, 20), "Distance: " + distance);
	}

	public void CalculateDistance(int currentPosition, int previousPosition)
	{
		int deltaDistance = currentPosition - previousPosition;

		if(deltaDistance >= 1)
		{			
			distance += deltaDistance;
		}
	}

	private void CalculateScore()
	{
		foreach (Obstacle obstacle in obstacles) 
		{
			if (obstacle.transform.position.x
				< player.transform.position.x) 
			{
				obstacle.isPassed = true;

				if(obstacle.isPassed == true)
				{
					Debug.Log ("Obstacle #" + obstacles.IndexOf(obstacle)
					           + " has been passed!");
					score++;
				}

			}
		}
	}

	private PlayerController instantiatePlayer()
	{
		player = Instantiate (playerPrefab, startPoint.transform.position,
		                      startPoint.transform.rotation) as PlayerController;
		return player;
	}

	public PlayerController GetPlayer()
	{
		return player;
	}

	private void instantiateObstacle()
	{
		Obstacle obstacleInstance;
		obstacleInstance = Instantiate (obstaclePrefab, obstacleInstantiator.transform.position, 
		             obstacleInstantiator.transform.rotation) as Obstacle;
	}

	public void ChangeObstacleColor()
	{
		foreach (Obstacle obstacle in obstacles) 
		{
			obstacle.ChangeColor();
		}
	}

	private void Reset()
	{
		//camera.ResetPosition ();
		player = instantiatePlayer ();

		foreach (Obstacle obstacle in obstacles) 
		{
			Destroy (obstacle.gameObject);
		}

		obstacles.Clear ();

		distance = 0;
		score = 0;


	}

	private void CleanUp()
	{
		// If the obstacle is out of camera and behind
		// the player, destroy it.
		foreach (Obstacle obstacle in obstacles) 
		{
			if(!obstacle.GetComponent<Renderer>().isVisible
			   && (obstacle.transform.position.x
			    < player.transform.position.x))
			{
				obstacles.Remove (obstacle);
				Destroy (obstacle.gameObject);
			}
		}
	}



}