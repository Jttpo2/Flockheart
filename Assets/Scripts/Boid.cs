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

	// Cohesion distance
	private float desiredCohesion = 50.0f;

	// Max distance for sensing other boids
	private float maxSensingDistance = 100.0f;

	private float minRandom = 0.0f;
	private float maxRandom = 10.0f;

	private Rigidbody body;

	void Start ()
	{
		Random.InitState (System.DateTime.Today.Millisecond);

		body = GetComponent <Rigidbody> ();
		StartCoroutine ("Steering");
	}

	IEnumerator Steering ()
	{
		while (true) {
			if (isInitiated) {

				// Steer toward target
				Vector3 seekVector = seek (commander.transform.position);

//				// Go towards target and slow down if too close
				Vector3 arriveVector = arrive (commander.transform.position);
//
//				// Flee from antagonist
				Vector3 fleeVector = flee (commander.transform.position);
//
//				// Separate from other close by boids
				Vector3 separateVector = separate (boidController.getFlock ());
//
//				// Cohere to far away boids
				Vector3 cohereVector = cohere (boidController.getFlock ());
//
//				// Align with the rest of the flock
				Vector3 alignVector = align (boidController.getFlock ());
//
//				// Sprinkle a bit of randomness to simulate free will
				Vector3 randomVector = addRandom ();
//
				seekVector *= 0.01f;
				arriveVector *= 0.0f;
				fleeVector *= 1.0f;
				separateVector *= 1.0f;
				cohereVector *= 0.1f;
				alignVector *= 0.7f;
				randomVector *= 0.1f;

				body.AddForce (seekVector);
				body.AddForce (arriveVector);
				body.AddForce (fleeVector);
				body.AddForce (separateVector);
				body.AddForce (cohereVector);
//				body.AddForce (alignVector);
//				body.AddForce (randomVector);
//
				// Point the transform in the direction of it's velocity
				pointTowardsVelocity ();

				// Clamp velocity
				if (body.velocity.magnitude > maxVel) {
					body.velocity = Vector3.ClampMagnitude (body.velocity, maxVel);
				} else if (body.velocity.magnitude < minVel) {
					body.velocity = body.velocity.normalized * minVel;
				}
			}
			float waitTime = Random.Range (0.005f, 0.05f);
			yield return new WaitForSeconds (waitTime);

		}
	}

	// Steer toward target
	private Vector3 seek (Vector3 targetPosition)
	{
		Vector3 desired = targetPosition - body.transform.position;
		desired.Normalize ();
		desired *= maxVel;

		Vector3 steeringVector = desired - body.velocity;
		steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		return steeringVector;
	}

	// Move away from antagonist
	private Vector3 flee (Vector3 antagonist)
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
		return steeringVector;
	}

	// Go towards target and slow down if too close
	private Vector3 arrive (Vector3 target)
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
		return steeringVector;
	}

	private Vector3 separate (GameObject[] flock)
	{
		Vector3 sum = Vector3.zero;
		int tooCloseBoids = 0; // Counting the amount of boids within separation distance 

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);

			if (d > 0 && d < desiredSeparation) {
				Vector3 diff = body.position - boid.transform.position;
				diff.Normalize ();// Should we normalize here?

				diff /= d; // Separating less with larger distance
				sum += diff;
				tooCloseBoids++;
			}
		}
		Vector3 steeringVector = Vector3.zero;
		if (tooCloseBoids > 0) { // Don't divide with 0
			sum /= tooCloseBoids;
			sum.Normalize ();
			sum *= maxVel;
			steeringVector = sum - body.velocity;
			steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		}
		return steeringVector;
	}

	// Go towards mass of flock
	private Vector3 cohere (GameObject[] flock)
	{
		Vector3 sum = Vector3.zero;
		int nearBoids = 0; // Counting the amount of boids within separation distance 

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);

			if (d > desiredCohesion && d < maxSensingDistance) {
				Vector3 diff = body.position + boid.transform.position;
				diff.Normalize ();// Should we normalize here?

				diff *= d; // Cohere more with larger distance
				sum += diff;
				nearBoids++;
			}
		}
		Vector3 steeringVector = Vector3.zero;
		if (nearBoids > 0) { // Don't divide with 0
			sum /= nearBoids;
			steeringVector = seek (sum);
		}
		return steeringVector;
	}

	Vector3 align (GameObject[] flock)
	{
		int nearBoids = 0;

		Vector3 sum = Vector3.zero;
		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);
			if (d > 0 && d < maxSensingDistance) {
				sum += boid.GetComponent <Rigidbody> ().velocity;
				nearBoids++;
			}

		}
		Vector3 steeringVector = Vector3.zero;
		if (nearBoids > 0) {
			sum /= nearBoids;
			sum.Normalize ();
			sum *= maxVel;
			steeringVector = sum - body.velocity;
			steeringVector = Vector3.ClampMagnitude (steeringVector, maxSteeringForce);
		}

		return steeringVector;
	}

	Vector3 addRandom ()
	{
		Vector3 randomVector = new Vector3 (
			                       Random.Range (minRandom, maxRandom), 
			                       Random.Range (minRandom, maxRandom), 
			                       Random.Range (minRandom, maxRandom));
		return randomVector;
	}

	void pointTowardsVelocity ()
	{
		transform.LookAt (transform.position - body.velocity);
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
