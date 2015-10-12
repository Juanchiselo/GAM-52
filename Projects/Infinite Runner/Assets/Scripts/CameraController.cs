using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float speed = 1.0f;
	private Vector3 cameraPosition = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		// Get the current Y and Z coordinates.
		cameraPosition.y = transform.position.y;
		cameraPosition.z = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Constantly move the camera to the right.
		cameraPosition.x = transform.position.x + speed;
		transform.position = cameraPosition;

		if (GameManager.GetInstance ().GetPlayer () == null) 
		{
			cameraPosition.x = GameManager.GetInstance().startPoint.transform.position.x;
			transform.position = cameraPosition;
		}
	}

	public void ResetPosition()
	{
		cameraPosition.x = GameManager.GetInstance().startPoint.transform.position.x;
		transform.position = cameraPosition;
	}
}
