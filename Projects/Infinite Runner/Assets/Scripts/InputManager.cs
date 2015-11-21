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

		// Add this object to the list of observers for various events.
		player.onPlayerDeath += this.SetPlayer;
	}
	
	// Update is called once per frame
	void Update () 
	{
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

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			GameManager.GetInstance().PauseGame();
			GUIManager.GetInstance ().ShowInGameMenu ();
		}
	}

	// Returns an instance of the InputManager.
	public static InputManager GetInstance()
	{
		return instance;
	}

	// Gets and stores a reference to a new player
	// when the player dies.
	private void SetPlayer()
	{
		player = GameManager.GetInstance ().GetPlayer ();

		//Subscribe it to the new player.
		GameManager.GetInstance ().GetPlayer ().onPlayerDeath
			+= this.SetPlayer;
	}
}