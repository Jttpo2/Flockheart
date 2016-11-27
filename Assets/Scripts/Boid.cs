using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
	//	private GameObject controllerGameObject;
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

				// Point the transform in the direction of it's velocity
				transform.LookAt (transform.position - body.velocity);

//				seek (commander.GetComponent <Rigidbody> ().transform.position);

//				body.velocity += calcVelocity () * Time.deltaTime;

				// Clamp velocity
//				if (body.velocity.magnitude > maxVel) {
//					body.velocity = Vector3.ClampMagnitude (body.velocity, maxVel);
//				} else if (body.velocity.magnitude < minVel) {
//					body.velocity = body.velocity.normalized * minVel;
//				}


			}
			float waitTime = Random.Range (0.005f, 0.01f);
			yield return new WaitForSeconds (waitTime);

		}
	}

	private void seek (Vector3 targetPosition)
	{
		
		Vector3 desired = targetPosition - body.transform.position;
		desired.Normalize ();
		desired *= maxVel;

		Vector3 steeringVector = desired - body.velocity;
		steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		body.AddForce (steeringVector);


	}





















	//	private Vector3 calcVelocity ()
	//	{
	//		Vector3 randomize = new Vector3 ((Random.value * 2) * -1, (Random.value * 2) * -1, (Random.value * 2) * -1);
	//
	//		randomize.Normalize ();
	//		boidController = controllerGameObject.GetComponent <BoidController> ();
	//		Vector3 flockCenter = boidController.flockCenter;
	//		Vector3 flockVelocity = boidController.flockVelocity;
	//		Vector3 follow = commander.transform.localPosition;
	//
	//		// Head  towards flock center?
	//		flockCenter = flockCenter - transform.localPosition;
	//		flockVelocity = flockVelocity - GetComponent <Rigidbody> ().velocity;
	//		follow = follow - transform.localPosition;
	//
	//		return (flockCenter + flockVelocity + follow * 2 + randomize * randomness);
	//	}

	void Update ()
	{
	
	}

	public void setController (GameObject controller)
	{
//		this.controllerGameObject = controller;
		boidController = controller.GetComponent <BoidController> ();
		minVel = boidController.getMinVelocity ();
		maxVel = boidController.getMaxVelocity ();
		maxSteeringForce = boidController.getMaxSteeringForce ();
		randomness = boidController.getRandomness ();
		commander = boidController.commander;
		isInitiated = true;
	}
}
