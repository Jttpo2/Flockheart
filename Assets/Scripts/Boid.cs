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
	private float desiredCohesion = 20.0f;

	// Max distance for sensing other boids
	private float maxSensingDistance = 100.0f;

	// Deegrees of perigheral vision, on both sides of velocity center line
	int peripheralVisionDegrees = 45;

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

				// Go towards target and slow down if too close
				Vector3 arriveVector = arrive (commander.transform.position);

				// Flee from antagonist
				Vector3 fleeVector = flee (commander.transform.position);

				// Separate from other close by boids
				Vector3 separateVector = separate (boidController.getFlock ());

				// Cohere to far away boids
				Vector3 cohereVector = cohere (boidController.getFlock ());

				// Align with the rest of the flock
				Vector3 alignVector = align (boidController.getFlock ());

				// Avoid boids blocking the view (Arrange, or stagger, with closest bird in peripherals. Like a v)
				Vector3 viewVector = view (boidController.getFlock ());

				// Sprinkle a bit of randomness to simulate free will
				Vector3 randomVector = addRandom ();

				seekVector *= 0.4f;
				arriveVector *= 0.01f;
				fleeVector *= 0.0f;
				separateVector *= 0.2f;
				cohereVector *= 0.4f;
				alignVector *= 0.02f;
				viewVector *= 0.05f;
				randomVector *= 0.05f;

				body.AddForce (seekVector);
				body.AddForce (arriveVector);
				body.AddForce (fleeVector);
				body.AddForce (separateVector);
				body.AddForce (cohereVector);
				body.AddForce (alignVector);
				body.AddForce (viewVector);
				body.AddForce (randomVector);

				// Point the transform in the direction of it's velocity
				pointTowardsVelocity ();

				// Clamp velocity
				if (body.velocity.magnitude > maxVel) {
					body.velocity = Vector3.ClampMagnitude (body.velocity, maxVel);
				} //else if (body.velocity.magnitude < minVel) {
//					body.velocity = body.velocity.normalized * minVel;
//				}
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

		desired -= body.velocity;
		return Vector3.ClampMagnitude (desired, maxSteeringForce);
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

		desired -= body.velocity;
		return Vector3.ClampMagnitude (desired, maxSteeringForce);
	}

	// Go towards target and slow down if too close
	private Vector3 arrive (Vector3 target)
	{
		Vector3 desired = target - body.transform.position;
		float distance = desired.magnitude;

		desired.Normalize ();

		if (distance < slowDownDistance) {
			desired *= map (distance, 0, slowDownDistance, 0, maxVel); // Map to max velocity
		} else {
			// Continue at max velocity
			desired *= maxVel;
		}

		desired -= body.velocity;
		return Vector3.ClampMagnitude (desired, maxSteeringForce);
	}

	private Vector3 separate (GameObject[] flock)
	{
		Vector3 sum = Vector3.zero;
		int tooCloseBoids = 0; // Counting the amount of boids within separation distance 

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);

			if (d > 0 && d < desiredSeparation) {
				sum += (body.position - boid.transform.position).normalized / d; // Separating less with larger distance
				tooCloseBoids++;
			}
		}

		if (tooCloseBoids > 0) { // Don't divide with 0
			sum /= tooCloseBoids;
			sum.Normalize ();
			sum *= maxVel;
			sum -= body.velocity;
			return Vector3.ClampMagnitude (sum, maxSteeringForce);
		}
		return Vector3.zero;
	}

	// Go towards mass of flock
	private Vector3 cohere (GameObject[] flock)
	{
		int nearBoids = 0; // Counting the amount of boids within separation distance 
		Vector3 sum = Vector3.zero;

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);

			if (d > desiredCohesion && d < maxSensingDistance) {
				sum += boid.transform.position;
				nearBoids++;
			}
		}
	
		if (nearBoids > 0) { // Don't divide with 0
			sum /= nearBoids;
			return seek (sum);
		}
		return Vector3.zero;
	}

	Vector3 align (GameObject[] flock)
	{
		int nearBoids = 0;
		Vector3 sum = Vector3.zero;

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);

			if (d < maxSensingDistance && d > 0) {
				sum += boid.GetComponent <Rigidbody> ().velocity;
				nearBoids++;
			}
		}

		if (nearBoids > 0) {
			sum /= nearBoids;
			sum.Normalize ();
			sum *= maxVel;
			sum -= body.velocity;
			return Vector3.ClampMagnitude (sum, maxSteeringForce);
		}
		return Vector3.zero;
	}

	// Avoid boids blocking the view (Stagger relative to the boids in your peripherals (in front of you))
	Vector3 view (GameObject[] flock)
	{
		int nearBoids = 0;

		Vector3 sum = Vector3.zero;

		foreach (GameObject boid in flock) {
			float d = Vector3.Distance (boid.transform.position, body.position);
			if (d < maxSensingDistance && isInPeripherals (boid) && d > 0) {
				sum += boid.transform.position;
				nearBoids++;
			}	
		}

		if (nearBoids > 0) {
			// Get vector orthogonal to diff and down
			Vector3 diff = sum / nearBoids - body.position;
			Vector3 ortho = Vector3.Cross (diff, Vector3.down);

			// Make sure the ortogonal vector points in the general direction of the boids velocity
			if (Vector3.Dot (body.velocity, ortho) < 0) {
				ortho *= -1;
			}
//			Debug.DrawLine (body.position, sum / nearBoids, Color.cyan);
//			Debug.DrawRay (body.position, diff, Color.magenta);

			ortho.Normalize ();
			ortho *= maxVel;
			ortho -= body.velocity;
			return Vector3.ClampMagnitude (ortho, maxSteeringForce);
		} else {
			return Vector3.zero;
		}

		// Debug peripheral vision
//		if (closestBoid != nu	ll) {
//			Debug.DrawLine (body.position, closestBoid.transform.position);	
//		} else {
//			Debug.DrawRay (body.position, body.velocity, Color.green);
//		}
	}

	bool isInPeripherals (GameObject boid)
	{
		return Vector3.Angle (body.velocity, boid.transform.position - body.position) < peripheralVisionDegrees;
	}

	Vector3 addRandom ()
	{
		return new Vector3 (
			Random.Range (minRandom, maxRandom), 
			Random.Range (minRandom, maxRandom), 
			Random.Range (minRandom, maxRandom));
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

	void Update ()
	{
	
	}

	public void setController (GameObject controller)
	{
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
