﻿using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{

	public GameObject spawnPointObject;
	private Vector3 spawnPoint;

	public int flockSize = 190;

	private float minVelocity = 0.0f;
	private float maxVelocity = 40.0f;
	private float maxSteeringForce = 10.0f;
	private float randomness = 1.0f;
	private float slowDownDistance = 10.0f;
	private float fleeingDistance = 20.0f;
	private float desiredSeparation = 5.0f;

	public GameObject prefab;
	public GameObject commander;

	private Vector3 flockCenter;
	//	private Vector3 flockVelocity;

	private Transform flockCenterTransform;

	private GameObject[] boids;

	private float originBoxX;
	private float originBoxY;
	private float originBoxZ;

	// Framerate calculations
	int m_frameCounter = 0;
	float m_timeCounter = 0.0f;
	float m_lastFramerate = 0.0f;
	public float m_refreshTime = 5.0f;

	void Start ()
	{
		populateBoids ();
		setupFlockCenterTransform ();
	}

	void Update ()
	{
		calcFlockCenter ();
	}

	void LateUpdate ()
	{
//		calcFrameRate ();
//		Boid.operationCounter = 0;
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
			flockCenter += boid.transform.position;
			//			theVelocity += boid.GetComponent <Rigidbody> ().velocity;
		}

		flockCenter /= flockSize;
		flockCenterTransform.position = flockCenter;
		//		flockVelocity = theVelocity / flockSize;
	}

	private void setupFlockCenterTransform ()
	{
		flockCenterTransform = transform.Find ("FlockCenter");
	}

	void calcFrameRate ()
	{
		if (m_timeCounter < m_refreshTime) {
			m_timeCounter += Time.deltaTime;
			m_frameCounter++;
		} else {
			//This code will break if you set your m_refreshTime to 0, which makes no sense.
			m_lastFramerate = (float)m_frameCounter / m_timeCounter;
			m_frameCounter = 0;
			m_timeCounter = 0.0f;

//			Debug.Log (Mathf.Floor (Boid.operationCounter / m_lastFramerate));
		}
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
