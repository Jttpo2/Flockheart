using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
	public RandomMover randMover;
	public Cannon cannon;
	public FollowCamera followCam;

	private readonly KeyCode spawnKey = KeyCode.Space;
	private readonly KeyCode fireKey = KeyCode.F;
	private readonly KeyCode zoomInKey = KeyCode.A;
	private readonly KeyCode zoomOutKey = KeyCode.Z;

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
		if (Input.GetKey (zoomInKey)) {
			followCam.zoomIn ();
		} else if (Input.GetKey (zoomOutKey)) {
			followCam.zoomOut ();
		}
	}
}
