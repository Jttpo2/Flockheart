using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour
{

	public RandomMover randMover;
	public Cannon cannon;

	private readonly KeyCode spawnKey = KeyCode.Space;
	private readonly KeyCode fireKey = KeyCode.F;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (Input.GetKeyUp (spawnKey)) {
			if (randMover) {
				randMover.spawn ();
			}
		}
		if (Input.GetKeyDown (fireKey)) {
			if (cannon) {
				cannon.shoot ();
			}
		}

	}
}
