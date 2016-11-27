﻿using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{

	private float minVelocity = 5.0f;
	private float maxVelocity = 10.0f;
	private float maxSteeringForce = 5.0f;
	private float randomness = 1.0f;
	private int flockSize = 10;

	public GameObject prefab;
	public GameObject commander;

	private Vector3 flockCenter;
	private Vector3 flockVelocity;

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

	public Vector3 getFlockVelocity ()
	{
		return flockVelocity;
	}
}