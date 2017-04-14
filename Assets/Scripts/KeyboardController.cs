using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour
{

	public RandomMover randMover;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (Input.GetKeyUp (KeyCode.Space)) {
			randMover.spawn ();
		}

	}
}
