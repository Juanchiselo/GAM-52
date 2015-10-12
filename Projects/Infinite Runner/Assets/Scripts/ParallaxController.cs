using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxController : MonoBehaviour 
{
	public List<GameObject> parallaxBackgrounds = new List<GameObject>();

	public float baseSpeed = 1.0f;
	public float scaleSpeed = 0.1f;
	public float currentSpeed = 0.0f;
	public Vector2 offset = Vector2.zero;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentSpeed = baseSpeed;

		foreach (GameObject go in parallaxBackgrounds) 
		{
			offset = go.GetComponent<Renderer>().material.mainTextureOffset;
			go.GetComponent<Renderer>().material.mainTextureOffset =
				new Vector2(offset.x + Time.deltaTime * currentSpeed, 0.0f);
			currentSpeed += scaleSpeed;
		}
	}
}
