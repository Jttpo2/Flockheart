using UnityEngine;
using System.Collections;

public class KeyboardFlier : MonoBehaviour
{

	private Rigidbody body;

	private float initialVelocity = 2.0f;
	public float topSpeed = 10.0f;

	private float accelerationStep = 0.5f;
	private Vector3 accelerationVector;
	private float noseMovementPerUpdate = 0.2f;
	private float rollPerUpdate = 0.2f;

	// Use this for initialization
	void Start ()
	{
		body = this.GetComponent<Rigidbody> ();
		body.velocity = body.transform.forward * initialVelocity;
		accelerationVector = new Vector3 (0.0f, accelerationStep, 0.0f);
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


		// Lift
		float liftBooster = 100.0f;
		Vector3 lift = Vector3.Project (body.velocity, transform.forward);
		body.AddForce (transform.up * lift.magnitude * liftBooster);

		// Set angular drag relative to speed
		body.drag = 0.01f * body.velocity.magnitude;
		body.angularDrag = 0.1f * body.velocity.magnitude;

		body.velocity = Vector3.ClampMagnitude (body.velocity, topSpeed); // Limit velocity


	}

	private void accelerate ()
	{
		body.AddRelativeForce (accelerationVector);
		//		body.velocity += body.velocity.normalized * accelerationStep;
	}

	private void decelerate ()
	{
		body.AddRelativeForce (-accelerationVector);
//		body.velocity -= body.velocity.normalized * accelerationStep;
	}

	private void noseUp ()
	{
//		body.AddRelativeForce (new Vector3 (-noseMovementPerUpdate, 0.0f, 0.0f));
//		body.transform.Rotate (new Vector3 (-noseMovementPerUpdate, 0.0f, 0.0f));
		angleNose (-noseMovementPerUpdate);
	}

	private void noseDown ()
	{
//		body.AddRelativeForce (new Vector3 (noseMovementPerUpdate, 0.0f, 0.0f));
//		body.transform.Rotate (new Vector3 (noseMovementPerUpdate, 0.0f, 0.0f));
		angleNose (noseMovementPerUpdate);
	}

	private void angleNose (float amount)
	{
//		body.AddTorque (new Vector3 (0.0f, 0.0f, amount));	
		body.AddTorque (transform.forward * amount);
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
		body.AddTorque (transform.up * amount);
	}
}
