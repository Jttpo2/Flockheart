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
	private float slowDownDistance;
	private float fleeingDistance;
	private float desiredSeparation;

	private float desiredCohesion = 100.0f;

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
//				seek (commander.transform.position);

				// Go towards target and slow down if too close
				arrive (commander.transform.position);

				// Flee from antagonist
//				flee (commander.transform.position);

				// Separate from other close by boids
				separate (boidController.getFlock ());

				// Cohere to far away boids
				cohere (boidController.getFlock ());

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

	// Steer toward target
	private void seek (Vector3 targetPosition)
	{
		Vector3 desired = targetPosition - body.transform.position;
		desired.Normalize ();
		desired *= maxVel;

		Vector3 steeringVector = desired - body.velocity;
		steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		body.AddForce (steeringVector);
	}

	// Move away from antagonist
	private void flee (Vector3 antagonist)
	{
		Vector3 desired = (antagonist * -1) - body.transform.position;
		float distance = desired.magnitude;

		desired.Normalize ();
		if (distance < fleeingDistance) {
			float m = map (distance, 0, fleeingDistance, maxVel, 0);
			desired *= m;

		} else {
			// Don't affect course when antagonist is outside fleeing distance
		}

		Vector3 steeringVector = desired - body.velocity;
		steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		body.AddForce (steeringVector);
	}

	// Go towards target and slow down if too close
	private void arrive (Vector3 target)
	{
		Vector3 desired = target - body.transform.position;
		float distance = desired.magnitude;

		desired.Normalize ();

		if (distance < slowDownDistance) {
			float m = map (distance, 0, slowDownDistance, 0, maxVel); // Map to max velocity
			desired *= m;
		} else {
			// Continue at max velocity
			desired *= maxVel;
		}

		Vector3 steeringVector = desired - body.velocity;
		steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		body.AddForce (steeringVector);
	}

	private void separate (GameObject[] flock)
	{
		Vector3 sum = Vector3.zero;
		int closeBoids = 0; // Counting the amount of boids within separation distance 

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);


			if (d > 0 && d < desiredSeparation) {
				Vector3 diff = body.position - boid.transform.position;
				diff.Normalize ();// Should we normalize here?

				diff /= d; // Separating less with larger distance
				sum += diff;
				closeBoids++;
			}
		}
		if (closeBoids > 0) { // Don't divide with 0
			sum /= closeBoids;
			sum.Normalize ();
			sum *= maxVel;
			Vector3 steeringVector = sum - body.velocity;
			steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
			body.AddForce (steeringVector);
		}
	}

	private void cohere (GameObject[] flock)
	{
		Vector3 sum = Vector3.zero;
		int farBoids = 0; // Counting the amount of boids within separation distance 

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);

			if (d > desiredCohesion) {
				Vector3 diff = body.position + boid.transform.position;
				diff.Normalize ();// Should we normalize here?

				diff *= d; // Cohere more with larger distance
				sum += diff;
				farBoids++;
			}
		}
		if (farBoids > 0) { // Don't divide with 0
			sum /= farBoids;
			sum.Normalize ();
			sum *= maxVel;
			Vector3 steeringVector = sum - body.velocity;
			steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
			body.AddForce (steeringVector);
		}
	}



	// Map between number ranges
	public static float map (float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
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
		slowDownDistance = boidController.getSlowdownDistance ();
		fleeingDistance = boidController.getFleeingDistance ();
		desiredSeparation = boidController.getDesiredSeparation ();

		commander = boidController.commander;
		isInitiated = true;
	}
}
