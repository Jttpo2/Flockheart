﻿using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

	private Transform bodyTransform;
	private Rigidbody body;

	private Vector3 pos;
	private Vector3 vel;
	private Vector3 acc;

	private Vector3 initialVelocity = new Vector3 (10.0f, 0.0f, 0.0f);

	public float topSpeed = 10;


	// Use this for initialization
	void Start ()
	{
		bodyTransform = this.transform;
		body = this.GetComponent <Rigidbody> ();

//		pos = body.position;
//		vel = initialVelocity;
//		acc = Vector3.zero;

		body.velocity = initialVelocity;
		acc = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//		vel += acc;
//		vel = Vector3.ClampMagnitude (vel, topSpeed); // Limit velocity
//		pos += vel;

		body.AddForce (acc);
		body.velocity = Vector3.ClampMagnitude (body.velocity, topSpeed); // Limit velocity




//		bodyTransform.position = pos;
//		body.GetComponent <Rigidbody> ().AddForce (acc);
	}


}
