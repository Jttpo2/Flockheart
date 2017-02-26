using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour
{

	private float initialVelocity = 0.0f;
	private float maxVelocity = 100.0f;

	private float xScale;
	private float yScale;
	private float zScale;
	private Vector3 centeringVector;

	private Rigidbody body;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody> ();
		body.velocity = body.transform.forward * initialVelocity;

		xScale = 100.0f;
		yScale = xScale;
		zScale = xScale;
		centeringVector = new Vector3 (xScale / 2, 0, zScale / 2);

		StartCoroutine ("Move");
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	Vector3 calcRandVector ()
	{
		float x = Mathf.PerlinNoise (Time.time * body.transform.transform.position.x, Time.time);
//		float y = Mathf.PerlinNoise (body.transform.transform.position.y * Time.time, body.transform.transform.position.x);
		float y = 0.0f;
//		float z = 0.0f;
		float z = Mathf.PerlinNoise (Time.time * body.transform.transform.position.z, Time.time);
		return new Vector3 (x * xScale, y * yScale, z * zScale);
	}

	IEnumerator Move ()
	{
		while (true) {

			body.position = calcRandVector ();
			body.position -= centeringVector;
			body.position = new Vector3 (body.position.x, body.transform.localScale.y * 10.0f, body.position.z);
//			
			body.velocity = Vector3.zero;
			//		body.AddForce (calcRandVector ());
			//		body.velocity = Vector3.ClampMagnitude (body.velocity, maxVelocity);


			float waitTime = Random.Range (10.0f, 20.0f);
			yield return new WaitForSeconds (waitTime);
		}
	}
}
