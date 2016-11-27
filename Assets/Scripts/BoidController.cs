using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{

	public float minVelocity = 5.0f;
	public float maxVelocity = 12.0f;
	public float randomness = 1.0f;
	public int flockSize = 10;

	public GameObject prefab;
	public GameObject commander;

	public Vector3 flockCenter;
	public Vector3 flockVelocity;

	private GameObject[] boids;

	private float originBoxX = 10;
	private float originBoxY = 10;
	private float originBoxZ = 10;

	void Start ()
	{
		boids = new GameObject[flockSize];
		for (int i = 0; i < flockSize; i++) {
			Vector3 position = new Vector3 (
				                   Random.value * originBoxX,
				                   Random.value * originBoxY,
				                   Random.value * originBoxZ);

			GameObject boid = Instantiate (prefab, this.transform.position, this.transform.rotation) as GameObject;
			boid.transform.parent = this.transform;
			boid.transform.localPosition = position;
			boid.GetComponent <Boid> ().setController (this.gameObject);
			boids [i] = boid;
		}


	}

	void Update ()
	{
		Vector3 theCenter = Vector3.zero;
		Vector3 theVelocity = Vector3.zero;

		foreach (GameObject boid in boids) {
			theCenter += boid.transform.localPosition;
			theVelocity += boid.GetComponent <Rigidbody> ().velocity;
		}

		flockCenter = theCenter / flockSize;
		flockVelocity = theVelocity / flockSize;
	}
}
