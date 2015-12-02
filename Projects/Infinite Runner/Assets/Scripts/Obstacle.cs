using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour 
{
	private int randomNumber = 0;
	private bool canChangeColor = true;
	private Color color = Color.gray;
	public bool isPassed = false;


	// Use this for initialization
	void Start () 
	{
		// Adds this obstacle to the list of obstacles.
		GameManager.Instance.GetObstaclesList ().Add (this);

		// Sets the color of the obstacle.
		ChangeColor ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	// Changes the color of the obstacle.
	public void ChangeColor()
	{
		// Checks if the obstacle is allowed to change color
		// before doing it.
		if (canChangeColor) 
		{
			// Generates a random number to randomly choose the color
			// of the obstacle.
			randomNumber = Random.Range (1, 3);
		
			// Sets the color based on the random
			// number obtained.
			switch (randomNumber) 
			{
			case 0:
				color = Color.gray;
				break;
			case 1:
				color = Color.blue;
				break;
			case 2:
				color = Color.red;
				break;
			default:
				Debug.Log ("NOT a valid color!");
				break;
			}
		
			// Sets the color of the obstacle based on
			// the color obtained.
			GetComponent<Renderer> ().material.color = color;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Destroyer") 
		{
			GameManager.Instance.GetObstaclesList().Remove(this);
			Destroy (gameObject);
		}
	}


	// Checks if the player is inside the no change
	// color area. If it is, the obstacle can't
	// change color.
	void OnTriggerStay(Collider other)
	{		
		if (other.gameObject.tag == "Player") 
			canChangeColor = false;
	}

	// When the player exits the no change color
	// area, the obstacle is allowed to change color again.
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			canChangeColor = true;
	}

}
