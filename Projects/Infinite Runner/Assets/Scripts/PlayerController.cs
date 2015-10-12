using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private float speed = 0.1f;
	public float jumpHeight = 200.0f;
	private bool isGrounded = true;
	private bool canChangeColor = true;
	private int previousPosition = 0;
	private Vector3 playerPosition = Vector3.zero;
	public GameObject remains = null;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Renderer> ().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () 
	{

		playerPosition.x = transform.position.x + speed;
		playerPosition.y = transform.position.y;

		// Makes the player constantly move to the right.
		transform.position = playerPosition;

		// Lock Rotation
		//transform.rotation = new Vector3 (0f, 0f, 0f);

		CalculateDistance ();
	}

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	// Jump function
	public void Jump()
	{
		if (isGrounded) 
		{
			isGrounded = false;
			
			Vector3 jump = new Vector3 (0.0f, jumpHeight, 0.0f);
			
			GetComponent<Rigidbody> ().AddForce (jump);
		}	
	}
	
	public void Slide()
	{
		if (isGrounded) 
		{
			Debug.Log ("I'm Sliding!");
		}
	}

	public void ChangeColor(string color)
	{
		if (canChangeColor) 
		{
			if (string.Equals (color, "Red")) 
				GetComponent<Renderer> ().material.color = Color.red;

			if (string.Equals (color, "Blue"))
				GetComponent<Renderer> ().material.color = Color.blue;
		}
	}


	// To detect whether the player is grounded.
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Floor")
			isGrounded = true;

		if (collision.gameObject.tag == "Obstacle")
		{
			if(GetComponent<Renderer>().material.color 
			   == collision.gameObject.GetComponent<Renderer>().material.color)
				collision.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			else
			{
				Instantiate(remains, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
	}

//	void OnTriggerStay(Collider other)
//	{
//		if (other.gameObject.tag == "Obstacle") 
//			canChangeColor = false;
//	}
//
//	void OnTriggerExit(Collider other)
//	{
//		if (other.gameObject.tag == "Obstacle")
//			canChangeColor = true;
//	}

	private void CalculateDistance()
	{
		int currentPosition = (int)gameObject.transform.position.x;

		GameManager.GetInstance ().CalculateDistance (
			currentPosition, previousPosition);

		previousPosition = currentPosition;
	}
}
