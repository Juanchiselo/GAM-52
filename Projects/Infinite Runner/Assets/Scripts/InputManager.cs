using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	private static InputManager instance = null;
	private PlayerController player = null;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		// Get and locally store a reference to the player.
		player = GameManager.GetInstance ().GetPlayer ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// If the player has died, get a reference of it again.
		if (player == null)
			player = GameManager.GetInstance ().GetPlayer ();


		// Check for pressed keys and make the player do
		// the action it needs to do based on the pressed key.
		if (Input.GetKeyDown (KeyCode.Space))
			player.Jump ();
		
		if (Input.GetKeyDown (KeyCode.DownArrow))
			player.Slide ();
		
		if (Input.GetKeyDown (KeyCode.A))
			player.ChangeColor ("Red");

		if (Input.GetKeyDown (KeyCode.S))
			player.ChangeColor ("Blue");
	}

	// Returns an instance of the InputManager.
	public static InputManager GetInstance()
	{
		return instance;
	}
}