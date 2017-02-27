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


	// For optimization
	private Vector3 seekVector;
	private Vector3 arriveVector;
	private Vector3 fleeVector;
	private Vector3 separateVector;
	private Vector3 cohereVector;
	private Vector3 alignVector;
	private Vector3 viewVector;
	private Vector3 randomVector;

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
				seekVector = seek (commander.transform.position);

				// Go towards target and slow down if too close
				arriveVector = arrive (commander.transform.position);

				// Flee from antagonist
				fleeVector = flee (commander.transform.position);
		
				// Conglomerated flocking behaviour (Separate, Cohere, Align and View)
				flock (boidController.getFlock ());

				// Sprinkle a bit of randomness to simulate free will
				randomVector = addRandom ();

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

	Vector3 separationSum = Vector3.zero;
	Vector3 cohesionSum = Vector3.zero;
	Vector3 alignmentSum = Vector3.zero;
	Vector3 viewSum = Vector3.zero;
	Vector3 ortho = Vector3.zero;

	private Vector3 flock (GameObject[] flock)
	{
		separationSum = Vector3.zero;
		cohesionSum = Vector3.zero;
		alignmentSum = Vector3.zero;
		viewSum = Vector3.zero;

		int tooClose = 0; // Counting the amount of boids within separation distance 
		int nearEnoughToCohere = 0; // Counting the amount of boids within cohesion distance 
		int nearEnoughToAlign = 0;
		int inPeripherals = 0;

		float d = 0;

		foreach (GameObject boid in flock) {
			d = Vector3.Distance (boid.transform.position, body.position);

			if (d > maxSensingDistance || d == 0) {
				continue;
			}
			// Separation - Separate from other close by boids
			if (d < desiredSeparation) {
				separationSum += (body.position - boid.transform.position).normalized / d; // Separating less with larger distance
				tooClose++;
			}

			// Cohesion - Cohere to far away boids
			if (d > desiredCohesion) {
				cohesionSum += boid.transform.position;
				nearEnoughToCohere++;
			}

			// Alignment -  Align with the rest of the flock
			alignmentSum += boid.GetComponent <Rigidbody> ().velocity;
			nearEnoughToAlign++;

			// View - Avoid boids blocking the view (Arrange, or stagger, with closest bird in peripherals. Like a v)
			if (isInPeripherals (boid)) {
				viewSum += boid.transform.position;
				inPeripherals++;
			}	
		}

		// Separation
		if (tooClose > 0) { // Don't divide with 0
			separationSum /= tooClose;
			separationSum.Normalize ();
			separationSum *= maxVel;
			separationSum -= body.velocity;
			separateVector = Vector3.ClampMagnitude (separationSum, maxSteeringForce);
		}

		// Cohesion
		if (nearEnoughToCohere > 0) { // Don't divide with 0
			cohesionSum /= nearEnoughToCohere;
			cohereVector = seek (cohesionSum);
		}

		// Alignment
		if (nearEnoughToAlign > 0) {
			alignmentSum /= nearEnoughToAlign;
			alignmentSum.Normalize ();
			alignmentSum *= maxVel;
			alignmentSum -= body.velocity;
			alignVector = Vector3.ClampMagnitude (alignmentSum, maxSteeringForce);
		}

		// View
		if (inPeripherals > 0) {
			// Get vector orthogonal to diff and down
			viewSum /= inPeripherals;
			ortho = Vector3.Cross (viewSum - body.position, Vector3.down);

			// Make sure the ortogonal vector points in the general direction of the boids velocity
			if (Vector3.Dot (body.velocity, ortho) < 0) {
				ortho *= -1;
			}
		
			ortho.Normalize ();
			ortho *= maxVel;
			ortho -= body.velocity;
			viewVector = Vector3.ClampMagnitude (ortho, maxSteeringForce);
		}
		return Vector3.zero;
	}

	bool isInPeripherals (GameObject boid)
	{
		return Vector3.Angle (body.velocity, boid.transform.position - body.position) < peripheralVisionDegrees;
	}

	Vector3 addRandom ()
	{
		float n = Random.Range (minRandom, maxRandom);
		return new Vector3 (n, n, n);
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
