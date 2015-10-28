using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour 
{
	private static GUIManager instance = null;
	public Text distance = null;
	public Text score = null;
	public Text lives = null;
	public Text counterLabel = null;
	public Canvas inGameMenu = null;
	private int counter = 3;
	
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
			//DontDestroyOnLoad(this);
		}
		else Destroy(this);
		inGameMenu.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	// Returns an instance of the GUIManager.
	public static GUIManager GetInstance()
	{
		return instance;
	}

	// Updates the Distance label.
	public void UpdateDistance(string distance)
	{
		this.distance.text = "Distance:  " + distance;
	}

	// Updates the Score label.
	public void UpdateScore(string score)
	{
		this.score.text = "Score:  " + score;
	}

	// Updates the Lives label.
	public void UpdateLives(string lives)
	{
		this.lives.text = "Lives:  " + lives;
	}

	public void OnButtonPress(Button button)
	{
		print (button.name);

		if(button.name == "Start Game Button")
			Application.LoadLevel (1);

		if (button.name == "Quit Game Button") 
		{
			UnityEditor.EditorApplication.isPlaying = false;


			//Application.Quit ();
		}

		if (button.name == "Resume Game Button") 
		{
			inGameMenu.gameObject.SetActive(false);
			StartCoroutine (ResumeGame ());
		}
	}

	public void ShowInGameMenu()
	{
		inGameMenu.gameObject.SetActive(true);

	}

	public IEnumerator ResumeGame()
	{
		// The time when the resume button was clicked.
		int pauseTime = (int) Time.realtimeSinceStartup;

		// The time when the game should unpause.
		int pauseEndTime = (int) pauseTime + 4;

		// Activate the Counter Text.
		if(this.counterLabel.gameObject.activeSelf == false)
			this.counterLabel.gameObject.SetActive (true);


		while (Time.realtimeSinceStartup < pauseEndTime) 
		{
			switch(pauseEndTime - (int) Time.realtimeSinceStartup)
			{
			case 1:
				this.counterLabel.text = "1";
				break;
			case 2:
				this.counterLabel.text = "2";
				break;
			case 3:
				this.counterLabel.text = "3";
				break;
			}

			yield return null;
		}

		this.counterLabel.gameObject.SetActive (false);
		counterLabel.text = "3";
		//counter = 3;
		GameManager.GetInstance().ResumeGame();
	}

	public void LoadGameobjects()
	{

	}
}
