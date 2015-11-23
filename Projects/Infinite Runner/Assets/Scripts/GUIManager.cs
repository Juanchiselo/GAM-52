using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GUIManager : MonoBehaviour 
{
	public static GUIManager Instance { get; private set; }

	// Labels
	[Header("Text Labels")]
	public Text lblPlayerName = null;
	public Text lblDistance = null;
	public Text lblScore = null;
	public Text lblCounter = null;
	public Text lblFinalScore = null;

	// Input Fields
	[Header("Input Fields")]
	public InputField txtPlayerName = null;

	// Canvases
	[Header("Canvases")]
	public Canvas inGameMenu = null;
	public Canvas playAgainMenu = null;

	// Buttons
	[Header("Buttons")]
	public Button btnStartGame = null;
	public Button btnQuitGame = null;
	public Button btnResumeGame = null;
	public Button btnMainMenu = null;
	public Button btnPlayAgain = null;
	public Button btnMainMenu2 = null;
	
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
			print("You are in the main menu");

			// Finds the Player Name input field.
			txtPlayerName = GameObject.Find("Player Name Input").GetComponent<InputField>();
			btnStartGame = GameObject.Find("Start Game Button").GetComponent<Button>();
			btnQuitGame = GameObject.Find("Quit Game Button").GetComponent<Button>();
			lblPlayerName = null;
			lblDistance = null;
			lblScore = null;
			lblCounter = null;
			lblFinalScore = null;
			inGameMenu = null;
			playAgainMenu = null;
			btnResumeGame = null;
			btnMainMenu = null;
			btnPlayAgain = null;
			btnMainMenu2 = null;

			// Add the function to the button's onclick.
			btnStartGame.onClick.AddListener(
				delegate
				{
				OnButtonPress(btnStartGame);
			}
			);
			
			btnQuitGame.onClick.AddListener(
				delegate 	
				{
				OnButtonPress(btnQuitGame);
			}
			);

			txtPlayerName.onEndEdit.AddListener(
				delegate
				{
				OnInputEnter(txtPlayerName);
			}
			);

			break;
		case 1:
			txtPlayerName = null;
			lblPlayerName = GameObject.Find("Player Name").GetComponent<Text>();
			lblPlayerName.text = "Name:  " + GameManager.Instance.GetPlayerName();
			lblDistance = GameObject.Find("Distance").GetComponent<Text>();
			lblScore = GameObject.Find("Score").GetComponent<Text>();
			lblCounter = GameObject.Find("Counter").GetComponent<Text>();
			lblCounter.gameObject.SetActive(false);
			inGameMenu = GameObject.Find("In-Game Menu").GetComponent<Canvas>();
			btnResumeGame = inGameMenu.transform.Find("Resume Game Button").GetComponent<Button>();
			btnMainMenu = inGameMenu.transform.Find("Quit Game Button").GetComponent<Button>();

			// Add the function to the button's onclick.
			btnResumeGame.onClick.AddListener(
				delegate
				{
					OnButtonPress(btnResumeGame);
				}
			);

			btnMainMenu.onClick.AddListener(
				delegate 	
				{
					OnButtonPress(btnMainMenu);
				}
			);

			playAgainMenu = GameObject.Find("Play Again Menu").GetComponent<Canvas>();
			lblFinalScore = playAgainMenu.transform.Find("Final Score").GetComponent<Text>();
			btnPlayAgain = playAgainMenu.transform.Find("Play Again Button").GetComponent<Button>();
			btnMainMenu2 = playAgainMenu.transform.Find("Quit Game Button").GetComponent<Button>();
			// Add the function to the button's onclick.
			btnPlayAgain.onClick.AddListener(
				delegate
				{
				OnButtonPress(btnPlayAgain);
			}
			);
			
			btnMainMenu2.onClick.AddListener(
				delegate 	
				{
				OnButtonPress(btnMainMenu2);
			}
			);

			playAgainMenu.gameObject.SetActive(false);

			btnStartGame = null;
			btnQuitGame = null;

			inGameMenu.gameObject.SetActive(false);
			break;
		default:
			Debug.Log ("ERROR: That is not a valid level index.");
			break;
		}
	}


	// Use this for initialization
	void Start () 
	{
		GameObject.Find ("Player Name Input").gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

//	// Returns an instance of the GUIManager.
//	public static GUIManager GetInstance()
//	{
//		return instance;
//	}

	// Updates the Distance text label.
	public void UpdateDistance(string distance)
	{
		lblDistance.text = "Distance:  " + distance;
	}

	// Updates the Score text label.
	public void UpdateScore(string score)
	{
		lblScore.text = "Score:  " + score;
	}

	// Updates the Final Score text label.
	public void UpdateFinalScore(string score)
	{
		lblFinalScore.text = "Your score: " + score;
	}

	// Recognizes what button has been pressed
	// and performs its action.
	public void OnButtonPress(Button button)
	{
		if (button.name == "Start Game Button") 
		{
			if(!txtPlayerName.gameObject.activeSelf)
				txtPlayerName.gameObject.SetActive (true);

			txtPlayerName.ActivateInputField();
		}

		if (button.name == "Quit Game Button") 
		{
			if(Application.isPlaying)
				Application.Quit();

//			if(Application.isEditor)
//				UnityEditor.EditorApplication.isPlaying = false;
		}

		if (button.name == "Resume Game Button") 
		{
			inGameMenu.gameObject.SetActive(false);
			StartCoroutine (ResumeGame ());
		}

		if (button.name == "Main Menu Button") 
		{
			Application.LoadLevel(0);
		}

		if (button.name == "Play Again Button") 
		{
			GameManager.Instance.Reset();
		}
	}

	public void OnInputEnter(InputField inputField)
	{
		// First Name and Last Name But Needs to fix Out of Screen issue.
		//if (Regex.IsMatch (inputField.text, "^[A-Z]?[a-z]{2,14}|([A-Z]?[a-z]{2-14})?$")) 

		// First Name only.
		if (Regex.IsMatch (inputField.text, "^[A-Z]?[a-z]{2,14}$")) 
		{
			GameManager.Instance.SetPlayerName (inputField.text);
			Application.LoadLevel (1);
		} 
		else 
		{
			inputField.text = "";
			inputField.placeholder.GetComponent<Text>().text = "Invalid Name";
			inputField.ActivateInputField();
		}
	}

	// Shows the In-Game Menu by making its canvas active.
	public void ShowInGameMenu()
	{
		inGameMenu.gameObject.SetActive(true);
	}

	// Shows the Play Again Menu by making its canvas active.
	public void ShowPlayAgainMenu()
	{
		playAgainMenu.gameObject.SetActive (true);
	}

	// Resumes the game when the "Resume Game" button
	// has been pressed. It also displays a countdown
	// before resuming the game.
	public IEnumerator ResumeGame()
	{
		// The time when the "Resume Game" button was pressed.
		int pauseTime = (int) Time.realtimeSinceStartup;

		// The time when the game should unpause.
		int pauseEndTime = (int) pauseTime + 4;

		// Activate the Counter text label.
		if(!lblCounter.gameObject.activeSelf)
			lblCounter.gameObject.SetActive (true);

		// Update the Counter text label.
		while (Time.realtimeSinceStartup < pauseEndTime) 
		{
			switch(pauseEndTime - (int) Time.realtimeSinceStartup)
			{
			case 1:
				lblCounter.text = "1";
				break;
			case 2:
				lblCounter.text = "2";
				break;
			case 3:
				lblCounter.text = "3";
				break;
			}

			yield return null;
		}

		// Hide the Counter text label by making it inactive.
		lblCounter.gameObject.SetActive (false);

		// Reset the Counter text label.
		lblCounter.text = "3";

		// Tell the GameManager to resume the game.
		GameManager.Instance.ResumeGame();
	}
}
