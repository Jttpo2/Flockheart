using UnityEngine;
using System.Collections;

public class KeyboardFlier : MonoBehaviour
{
	private Rigidbody body;

	private float initialVelocity = 2.0f;
	public float topSpeed = 10.0f;

	private float accelerationStep = 1.0f;
	private float noseMovementPerUpdate = 0.4f;
	private float rollPerUpdate = 0.4f;

	float liftBooster = 100.0f;

	// Use this for initialization
	void Start ()
	{
		body = this.GetComponent<Rigidbody> ();
		body.velocity = body.transform.forward * initialVelocity;
	}

	void FixedUpdate ()
	{
		if (Input.GetKey (KeyCode.A)) {
			accelerate ();
		} else if (Input.GetKey (KeyCode.Z)) {
			decelerate ();
		} 

		if (Input.GetKey (KeyCode.UpArrow)) {
			noseDown ();
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			noseUp ();
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			rollLeft ();
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			rollRight ();
		}

		// Lift, to make it follow it's nose
		Vector3 lift = Vector3.Project (body.velocity, transform.up);
		body.AddForce (transform.forward * lift.magnitude * liftBooster);

		// Set angular drag relative to speed
		body.drag = 0.001f * body.velocity.magnitude;
		body.angularDrag = 0.1f * body.velocity.magnitude;

		// Limit speed
		body.velocity = Vector3.ClampMagnitude (body.velocity, topSpeed); // Limit velocity

	}


	// Not working
	private void accelerate ()
	{
		body.AddRelativeForce (body.transform.forward * accelerationStep);
	}

	// Not working
	private void decelerate ()
	{
		body.AddRelativeForce (-body.transform.forward * accelerationStep);
	}

	private void noseUp ()
	{
		angleNose (-noseMovementPerUpdate);
	}

	private void noseDown ()
	{
		angleNose (noseMovementPerUpdate);
	}

	private void angleNose (float amount)
	{
		body.AddTorque (transform.right * amount);
	}


	private void rollLeft ()
	{
		roll (rollPerUpdate);
	}

	private void rollRight ()
	{
		roll (-rollPerUpdate);
	}

	private void roll (float amount)
	{
		body.AddTorque (transform.forward * amount);
	}
}
