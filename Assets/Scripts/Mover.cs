using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

	private Transform body;

	private Vector3 pos;
	private Vector3 vel;
	private Vector3 acc;

	private Vector3 initialVelocity = new Vector3 (0.2f, 0.0f, 0.0f);

	public float topSpeed = 10;


	// Use this for initialization
	void Start ()
	{
		body = this.transform;
		pos = body.transform.position;
		vel = initialVelocity;
		acc = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		vel += acc;
		vel = Vector3.ClampMagnitude (vel, topSpeed); // Limit velocity

		pos += vel;

		body.position = pos;
//		body.GetComponent <Rigidbody> ().AddForce (acc);
	}


}
