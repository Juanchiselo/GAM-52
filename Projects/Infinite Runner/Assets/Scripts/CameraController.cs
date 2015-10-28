using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	private float speed = 5.0f;
	private Vector3 cameraPosition = Vector3.zero;
	private Vector3 resetCameraPosition = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		// Get the current Y and Z coordinates.
		cameraPosition.y = transform.position.y;
		cameraPosition.z = transform.position.z;
		resetCameraPosition = transform.position;

		// Add it to the list of observers for various events.
		GameManager.GetInstance ().GetPlayer ().onPlayerDeath
			+= this.Reset;
		GameManager.GetInstance ().onIncreaseSpeed
			+= this.SetSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Constantly move the camera to the right.
		cameraPosition.x = transform.position.x + speed * Time.deltaTime;
		transform.position = cameraPosition;
	}

	public void Reset()
	{
		//Subscribe it to the new player.
		GameManager.GetInstance ().GetPlayer ().onPlayerDeath
			+= this.Reset;

		// Reset the position of the camera.
		transform.position = resetCameraPosition;

		// Reset the speed of the camera.
		speed = GameManager.GetInstance ().GetSpeed ();
	}

	public void SetSpeed()
	{
		speed = GameManager.GetInstance ().GetSpeed();
	}
}
