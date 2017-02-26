using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour
{

	private float initialVelocity = 0.0f;
	private float maxVelocity = 10.0f;

	private float xScale;
	private float yScale;
	private float zScale;


	private Rigidbody body;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody> ();
		body.velocity = body.transform.forward * initialVelocity;

		xScale = 10.0f;
		yScale = xScale;
		zScale = xScale;
	}
	
	// Update is called once per frame
	void Update ()
	{
		body.position = calcRandVector ();
//		body.AddForce (calcRandVector ());
//		body.velocity = Vector3.ClampMagnitude (body.velocity, maxVelocity);
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
}
