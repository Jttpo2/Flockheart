using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
	private GameObject controllerGameObject;
	private BoidController boidController;
	private bool isInitiated = false;
	private float minVel;
	private float maxVel;
	private float maxSteeringForce;
	private float randomness;
	private GameObject commander;

	private Rigidbody body;

	void Start ()
	{
		body = GetComponent <Rigidbody> ();
		StartCoroutine ("Steering");
	}

	IEnumerator Steering ()
	{
		while (true) {
			if (isInitiated) {
				// Get steering force
//				body.velocity += calcSteeringForce () * Time.deltaTime;

				// Steer toward target
				seek (commander.transform.position);

//				body.velocity += calcVelocity () * Time.deltaTime;

				// Clamp velocity
				if (body.velocity.magnitude > maxVel) {
					body.velocity = Vector3.ClampMagnitude (body.velocity, maxVel);
				} else if (body.velocity.magnitude < minVel) {
					body.velocity = body.velocity.normalized * minVel;
				}


			}
			float waitTime = Random.Range (0.3f, 0.5f);
			yield return new WaitForSeconds (waitTime);

		}
	}

	//	private Vector3 calcSteeringForce ()
	//	{
	//		return desired = commander.transform.position;
	//
	//
	//	}

	private void seek (Vector3 targetPosition)
	{
		
		Vector3 desired = targetPosition - body.transform.position;
		desired.Normalize ();
		desired *= maxVel;

		Vector3 steeringVector = desired - body.velocity;
		steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		body.AddForce (steeringVector); // * Time.deltaTime);
	}

	private Vector3 calcVelocity ()
	{
		Vector3 randomize = new Vector3 ((Random.value * 2) * -1, (Random.value * 2) * -1, (Random.value * 2) * -1);

		randomize.Normalize ();
		boidController = controllerGameObject.GetComponent <BoidController> ();
		Vector3 flockCenter = boidController.flockCenter;
		Vector3 flockVelocity = boidController.flockVelocity;
		Vector3 follow = commander.transform.localPosition;

		// Head  towards flock center?
		flockCenter = flockCenter - transform.localPosition;
		flockVelocity = flockVelocity - GetComponent <Rigidbody> ().velocity;
		follow = follow - transform.localPosition;
			
		return (flockCenter + flockVelocity + follow * 2 + randomize * randomness);
	}

	void Update ()
	{
	
	}

	public void setController (GameObject controller)
	{
		this.controllerGameObject = controller;
		boidController = controller.GetComponent <BoidController> ();
		minVel = boidController.minVelocity;
		maxVel = boidController.maxVelocity;
		maxSteeringForce = boidController.maxSteeringForce;
		randomness = boidController.randomness;
		commander = boidController.commander;
		isInitiated = true;
	}
}
