﻿using UnityEngine;
using System.Collections;

public class Watcher : MonoBehaviour
{
	public Transform watchee;

	private Vector3 targetPos;

	private Vector3 offset;
	private Vector3 mouseOrigin;
	private Vector3 prevMousePos;
	private bool isPanning;

	private float rotationSpeedX = 0.2f;
	private float rotationSpeedY = 0.2f;

	void Start ()
	{
		prevMousePos = Vector3.zero;

		if (watchee) {
			targetPos = watchee.GetComponent <BoidController> ().getFlockCenter ();
			offset = targetPos - transform.position;
		}
	}

	void LateUpdate ()
	{
		if (!watchee) {
			return;
		}
		
		targetPos = watchee.GetComponent <BoidController> ().getFlockCenter ();
		transform.position = targetPos - offset;


		if (Input.GetMouseButtonDown(0)) {
			prevMousePos = Input.mousePosition;
			isPanning = true;
		} 
		if (Input.GetMouseButtonUp(0)) {
			isPanning = false;
		}
		if (isPanning) {
			Vector3 mouseDelta = Input.mousePosition - prevMousePos;
			rotateAroundWatchee (mouseDelta.x * rotationSpeedX);

			panUpAndDown (mouseDelta.y * rotationSpeedY);

			// Update offset
			offset = targetPos - transform.position;

			prevMousePos = Input.mousePosition;
		}
			
		transform.LookAt (targetPos);
	}

	void rotateAroundWatchee(float degrees) {
		transform.RotateAround (targetPos, Vector3.up, degrees);
	}

	void panUpAndDown(float degrees) {
		//	Pan axis is horizontal and perpendicular to sight line
		Vector3 panAxis = Vector3.Cross(offset, Vector3.up);

		Debug.Log (transform.rotation.eulerAngles.x + degrees);
		if (degrees > 0 && transform.rotation.eulerAngles.x + degrees >= 90) {
			Debug.Log ("Over 90");
			return;
		}

		transform.RotateAround (targetPos, panAxis, degrees);
//		transform.rotation.eulerAngles
	}
}