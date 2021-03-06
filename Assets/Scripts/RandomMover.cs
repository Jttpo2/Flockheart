﻿using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour
{
	public GameObject spawnPoint;
	private Vector3 spawnP;

	public GameObject worldPlane;

	public float spawnInterval = 60.0f;
	public float minY = -20.0f;

	private float initialVelocity = 0.0f;
	//	private float maxVelocity = 120.0f;
	private float maxVelocity = 20000.0f;

	private float xScale;
	private float yScale;
	private float zScale;


	private Rigidbody body;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody> ();
		body.velocity = body.transform.forward * initialVelocity;

		xScale = 100.0f;
		yScale = xScale;
		zScale = xScale;

		spawnP = spawnPoint.transform.position;
//		spawnP = new Vector3 (xScale / 2, 0, zScale / 2);

		StartCoroutine ("Move");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isOutOfBounds ()) {
			spawn ();
		}
	}

	Vector3 calcRandVector ()
	{
//		float x = Mathf.PerlinNoise (Time.time * body.transform.transform.position.x, Time.time);
//		float y = Mathf.PerlinNoise (body.transform.transform.position.y * Time.time, body.transform.transform.position.x);
//		float y = 0.0f;
//		float z = 0.0f;
//		float z = Mathf.PerlinNoise (Time.time * body.transform.transform.position.z, Time.time);
//		return new Vector3 (x * xScale, y * yScale, z * zScale);\

		return new Vector3 (
			Random.Range (-50, 50), 10, Random.Range (-50, 50)
		);
	}

	IEnumerator Move ()
	{
		while (true) {

			spawn ();

			yield return new WaitForSeconds (spawnInterval);
		}
	}

	public void spawn ()
	{
		Vector3 randVector = calcRandVector ();
		body.position = spawnP + randVector;

		body.velocity = Vector3.zero;
	}

	bool isOutOfBounds ()
	{
//		Vector3 worldCenterPos = worldPlane.transform.position;


		if (body.position.y < minY) {
			return true;
		}
		return false;
	}
}
