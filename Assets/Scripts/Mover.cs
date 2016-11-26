using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

	//	private Transform bodyTransform;
	private Rigidbody body;

	//	private Vector3 pos;
	//	private Vector3 vel;
	//	private Vector3 acc;

	//	private Vector3 initialVelocity = new Vector3 (10.0f, 0.0f, 0.0f);
	//	private Vector3 initialVelocity = new Vector3 (10.0f, 0.0f, 0.0f);

	//	public float topSpeed = 10;

	//	public float accTowardsMouse;

	// Use this for initialization
	void Start ()
	{
//		bodyTransform = this.transform;
		body = this.GetComponent <Rigidbody> ();

//		body.velocity = initialVelocity;
//		acc = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//		body.AddForce (acc);
//		moveTowardsMouse ();

//		body.velocity = Vector3.ClampMagnitude (body.velocity, topSpeed); // Limit velocity
	}

	//	private void moveTowardsMouse ()
	//	{
	//		Vector3 mousePos = Input.mousePosition;
	//		Vector3 mouseDir = body.position - mousePos;
	//		mouseDir.Normalize ();
	//		mouseDir *= accTowardsMouse;
	//		body.AddForce (mouseDir);
	//	}
}
