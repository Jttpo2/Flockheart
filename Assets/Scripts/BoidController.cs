using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{

	public GameObject spawnPointObject;
	private Vector3 spawnPoint;

	private float minVelocity = 0.0f;
	private float maxVelocity = 30.0f;
	private float maxSteeringForce = 10.0f;
	private float randomness = 1.0f;
	private int flockSize = 180;
	private float slowDownDistance = 10.0f;
	private float fleeingDistance = 20.0f;
	private float desiredSeparation = 5.0f;

	public GameObject prefab;
	public GameObject commander;

	private Vector3 flockCenter;
	//	private Vector3 flockVelocity;

	private GameObject[] boids;

	private float originBoxX;
	private float originBoxY;
	private float originBoxZ;

	void Start ()
	{
		populateBoids ();
	}

	void Update ()
	{
		calcFlockCenter ();
	}

	void populateBoids ()
	{
		spawnPoint = spawnPointObject.transform.position;
		spawnPoint.y += 10;


		originBoxX = 40;
		originBoxY = 40;
		originBoxZ = 40;

		boids = new GameObject[flockSize];
		for (int i = 0; i < flockSize; i++) {
			Vector3 position = new Vector3 (
				                   Random.value * originBoxX,
				                   Random.value * originBoxY,
				                   Random.value * originBoxZ);
			position += spawnPoint;
			addBoid (position, i);
		}
	}

	void addBoid (Vector3 position, int arrayIndex)
	{
		GameObject boid = Instantiate (prefab, this.transform.position, this.transform.rotation) as GameObject;
		boid.transform.parent = this.transform;
		boid.transform.localPosition = position;
		boid.GetComponent <Boid> ().setController (this.gameObject);
		boids [arrayIndex] = boid;
	}

	private void calcFlockCenter ()
	{
		flockCenter = Vector3.zero;
		//		Vector3 theVelocity = Vector3.zero;

		foreach (GameObject boid in boids) {
			flockCenter += boid.transform.localPosition;
			//			theVelocity += boid.GetComponent <Rigidbody> ().velocity;
		}

		flockCenter /= flockSize;
		//		flockVelocity = theVelocity / flockSize;
	}

	public float getMinVelocity ()
	{
		return minVelocity;
	}

	public float getMaxVelocity ()
	{
		return maxVelocity;
	}

	public float getMaxSteeringForce ()
	{
		return maxSteeringForce;
	}

	public float getRandomness ()
	{
		return randomness;
	}

	public Vector3 getFlockCenter ()
	{
		return flockCenter;
	}

	//	public Vector3 getFlockVelocity ()
	//	{
	//		return flockVelocity;
	//	}

	public float getSlowdownDistance ()
	{
		return slowDownDistance;
	}

	public float getFleeingDistance ()
	{
		return fleeingDistance;
	}

	public float getDesiredSeparation ()
	{
		return desiredSeparation;
	}

	public GameObject[] getFlock ()
	{
		return boids;
	}
}
