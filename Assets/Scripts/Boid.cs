using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{

	private GameObject controller;
	private bool isInitiated = false;
	private float minVel;
	private float maxVel;
	private float randomness;
	private GameObject commander;

	void Start ()
	{
		StartCoroutine ("Steering");
	}

	IEnumerator Steering ()
	{
		while (true) {
			if (isInitiated) {
				Rigidbody body = GetComponent <Rigidbody> ();
				body.velocity += calcVelocity () * Time.deltaTime;

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

	private Vector3 calcVelocity ()
	{
		Vector3 randomize = new Vector3 ((Random.value * 2) * -1, (Random.value * 2) * -1, (Random.value * 2) * -1);

		randomize.Normalize ();
		BoidController boidController = controller.GetComponent <BoidController> ();
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
		this.controller = controller;
		BoidController boidController = controller.GetComponent <BoidController> ();
		minVel = boidController.minVelocity;
		maxVel = boidController.maxVelocity;
		randomness = boidController.randomness;
		commander = boidController.commander;
		isInitiated = true;
	}
}
