using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour 
{
	public List<CharacterState> states = new List<CharacterState>();
	public StateMachineEngine stateMachine = null;
	public float coolDownTimer = 0.0f;
	
	void Awake()
	{
		stateMachine.Initialize<CharacterState> (this);
		stateMachine.ChangeState (CharacterState.NONE);
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		states = stateMachine.GetStates<CharacterState> ();

		if (stateMachine.GetState<CharacterState> (CharacterState.NONE) == true) 
		{
		}
	}

	public void NONE_Enter()
	{
		print ("Entering NONE State");
	}

	public void NONE_Update()
	{
		print ("NONE is Updating");

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			stateMachine.AddConcurrentState(CharacterState.COOLDOWN);
		}
	}

	public void NONE_Exit()
	{
		print ("Exiting NONE State");
	}

	public void COOLDOWN_Enter()
	{
		print ("Entering COOLDOWN State");
		coolDownTimer = 5.0f;
	}
	
	public void COOLDOWN_Update()
	{
		print ("COOLDOWN is Updating");
		coolDownTimer -= Time.deltaTime;
		if (coolDownTimer <= 0.0f) 
		{
			coolDownTimer = 0.0f;
			stateMachine.RemoveConcurrentState(CharacterState.COOLDOWN);
		}
	}
	
	public void COOLDOWN_Exit()
	{
		print ("Exiting COOLDOWN State");
	}
}

public enum CharacterState
{
	NONE,
	RUNNING,
	COOLDOWN
};