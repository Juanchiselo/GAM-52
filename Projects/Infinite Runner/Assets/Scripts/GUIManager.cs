using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GUIManager : MonoBehaviour 
{
	public static GUIManager Instance = null;

	// Labels
	[Header("Text Labels")]
	public Text lblPlayerName = null;
	public Text lblDistance = null;
	public Text lblScore = null;
	public Text lblCounter = null;
	public Text lblFinalScore = null;
	public Text lblNewHighScore = null;
	public Text[] lblHighScoreNames;
	public Text[] lblHighScores;
	public Text[] lblInstructions;

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
	public Button btnQuitPAM = null;
	public Button btnQuitIGM = null;
	public Button btnA = null;
	public Button btnS = null;
	public Button btnNextButton = null;

	void Awake()
	{
		if (Instance != null && Instance != this) 
		{
			Destroy (gameObject);
		}
		Instance = this;

		DontDestroyOnLoad (gameObject);
	}

	void OnLevelWasLoaded(int index)
	{
		switch (index) 
		{
		case 1:
			txtPlayerName = GameObject.Find("Player Name Input").GetComponent<InputField>();
			txtPlayerName.gameObject.SetActive(false);
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
			btnQuitPAM = null;
			btnQuitIGM = null;
			lblHighScoreNames = null;
			lblHighScores = null;

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
		case 2:
			lblPlayerName = null;
			lblDistance = null;
			lblScore = null;
			lblCounter = null;
			lblFinalScore = null;
			lblNewHighScore = null;
			inGameMenu = null;
			playAgainMenu = null;
			btnResumeGame = null;
			btnPlayAgain = null;
			btnQuitPAM = null;
			btnQuitIGM = null;
			txtPlayerName = null;
			btnStartGame = GameObject.Find("Start Button").GetComponent<Button>();
			btnStartGame.onClick.AddListener(
				delegate
				{
					OnButtonPress(btnStartGame);
				}
			);
			btnStartGame.gameObject.SetActive(false);
			btnQuitGame = null;
			btnNextButton = GameObject.Find("Next Button").GetComponent<Button>();
			btnNextButton.onClick.AddListener(
				delegate
				{
					OnButtonPress(btnNextButton);
				}
			);
			lblInstructions = new Text[4];
			for(int i = 0; i < lblInstructions.Length; i++)
			{
				lblInstructions[i] = GameObject.Find("Instructions 0" + 
				                                     (i + 1).ToString()).GetComponent<Text>();
				lblInstructions[i].gameObject.SetActive(false);
			}

			lblInstructions[0].gameObject.SetActive(true);
			break;
		case 3:
			txtPlayerName = null;
			lblPlayerName = GameObject.Find("Player Name").GetComponent<Text>();
			lblPlayerName.text = "Name:  " + GameManager.Instance.GetPlayerName();
			lblDistance = GameObject.Find("Distance").GetComponent<Text>();
			lblScore = GameObject.Find("Score").GetComponent<Text>();
			lblCounter = GameObject.Find("Counter").GetComponent<Text>();
			lblCounter.gameObject.SetActive(false);
			btnA = GameObject.Find ("A").GetComponent<Button>();
			btnA.GetComponent<Image>().color = Color.red;
			btnS = GameObject.Find("S").GetComponent<Button>();
			btnS.GetComponent<Image>().color = Color.blue;
			inGameMenu = GameObject.Find("In-Game Menu").GetComponent<Canvas>();
			btnResumeGame = inGameMenu.transform.Find("Resume Game Button").GetComponent<Button>();
			btnQuitIGM = inGameMenu.transform.Find("Quit Button").GetComponent<Button>();
			btnMainMenu = null;

			// Add the function to the button's onclick.
			btnResumeGame.onClick.AddListener(
				delegate
				{
					OnButtonPress(btnResumeGame);
				}
			);

			btnQuitIGM.onClick.AddListener(
				delegate 	
				{
					OnButtonPress(btnQuitIGM);
				}
			);

			playAgainMenu = GameObject.Find("Play Again Menu").GetComponent<Canvas>();
			lblNewHighScore = playAgainMenu.transform.Find("New High Score").GetComponent<Text>();
			lblNewHighScore.gameObject.SetActive(false);
			lblFinalScore = playAgainMenu.transform.Find("Final Score").GetComponent<Text>();
			btnPlayAgain = playAgainMenu.transform.Find("Play Again Button").GetComponent<Button>();
			btnQuitPAM = playAgainMenu.transform.Find("Quit Button").GetComponent<Button>();
			// Add the function to the button's onclick.
			btnPlayAgain.onClick.AddListener(
				delegate
				{
				OnButtonPress(btnPlayAgain);
			}
			);
			
			btnQuitPAM.onClick.AddListener(
				delegate 	
				{
				OnButtonPress(btnQuitPAM);
			}
			);

			playAgainMenu.gameObject.SetActive(false);

			btnStartGame = null;
			btnQuitGame = null;

			inGameMenu.gameObject.SetActive(false);
			break;
		case 4:
			lblHighScoreNames = new Text[5];
			lblHighScores = new Text[5];
			btnMainMenu = GameObject.Find("Main Menu Button").GetComponent<Button>();
			
			btnMainMenu.onClick.AddListener(
				delegate 	
				{
				OnButtonPress(btnMainMenu);
			}
			);

			for(int i = 0; i < lblHighScoreNames.Length; i++)
			{
				lblHighScoreNames[i] = GameObject.Find("High Score " + (i + 1) 
				                                   + " - Name").GetComponent<Text>();

				lblHighScores[i] = GameObject.Find("High Score " + (i + 1)
				                                   + " - Score").GetComponent<Text>();
			}


			UpdateHighScores();

			lblPlayerName = null;
			lblDistance = null;
			lblScore = null;
			lblCounter = null;
			lblFinalScore = null;
			lblNewHighScore = null;
			inGameMenu = null;
			playAgainMenu = null;
			btnResumeGame = null;
			btnPlayAgain = null;
			btnQuitPAM = null;
			btnQuitIGM = null;
			txtPlayerName = null;
			btnStartGame = null;
			btnQuitGame = null;
			break;
		default:
			Debug.Log ("ERROR: That is not a valid level index.");
			break;
		}
	}


	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

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

			if(Application.isEditor)
				UnityEditor.EditorApplication.isPlaying = false;
		}

		if (button.name == "Resume Game Button") 
		{
			inGameMenu.gameObject.SetActive(false);
			StartCoroutine (ResumeGame ());
		}

		if (button.name == "Quit Button") 
		{
			if(GameManager.Instance.GetHighScores().Count != 0)
				Application.LoadLevel("High Scores");
			else
				Application.LoadLevel("Main Menu");
		}

		if (button.name == "Main Menu Button") 
		{
			Application.LoadLevel("Main Menu");
		}

		if (button.name == "Play Again Button") 
		{
			GameManager.Instance.Reset();
		}

		if (button.name == "Start Button") 
		{
			Application.LoadLevel("Level 1");
		}

		if (button.name == "Next Button")
		{
			int previousIndex = 0;

			for(int i = 0; i < lblInstructions.Length; i++)
			{
				if(lblInstructions[i].IsActive())
				{
					lblInstructions[i].gameObject.SetActive(false);
					previousIndex = i;
				}
			}

			if(previousIndex + 1 != lblInstructions.Length - 1)
			{
				lblInstructions[previousIndex + 1].gameObject.SetActive(true);
			}
			else
			{
				lblInstructions[previousIndex + 1].gameObject.SetActive(true);
				btnNextButton.gameObject.SetActive(false);
				btnStartGame.gameObject.SetActive(true);
			}
		}
	}

	public void OnInputEnter(InputField inputField)
	{
		// First Name only.
		if (Regex.IsMatch (inputField.text, "^[A-Z]?[a-z]{2,14}$")) 
		{
			GameManager.Instance.SetPlayerName (inputField.text);
			if(GameManager.Instance.doLoadTutorial)
				Application.LoadLevel ("Tutorial");
			else
				Application.LoadLevel ("Level 1");
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

	public void ShowNewHighScore()
	{
		lblNewHighScore.gameObject.SetActive (true);
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

	public void UpdateHighScores()
	{
		int numberOfScores = GameManager.Instance.GetHighScores ().Count;

		for (int i = 0; i < numberOfScores; i++) 
		{
			lblHighScoreNames[i].text = (i + 1).ToString() + ". "
				+ GameManager.Instance.GetHighScores()[i].GetName();

			lblHighScores[i].text = 
				GameManager.Instance.GetHighScores()[i].GetScore().ToString();
		}

		for (int i = numberOfScores; i < lblHighScoreNames.Length; i++) 
		{
			lblHighScoreNames[i].gameObject.SetActive(false);
			lblHighScores[i].gameObject.SetActive(false);
		}


	}
}
