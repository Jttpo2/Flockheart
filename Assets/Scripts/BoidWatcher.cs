using UnityEngine;
using System.Collections;

public class BoidWatcher : MonoBehaviour
{
	public Transform boidController;

	private Vector3 targetPos;

	private Vector3 offset;
	private Vector3 mouseOrigin;
	private Vector3 prevMousePos;
	private bool isPanning;

	private float zoomSpeed = 0.01f;

	private float rotationSpeedX = 0.2f;

	void Start ()
	{
		prevMousePos = Vector3.zero;

		if (boidController) {
			targetPos = boidController.GetComponent <BoidController> ().getFlockCenter ();
			offset = targetPos - transform.position;
		}

	}

	void LateUpdate ()
	{
		if (boidController) {
			targetPos = boidController.GetComponent <BoidController> ().getFlockCenter ();
			transform.position = targetPos - offset;
		}

		if (Input.GetMouseButtonDown(0)) {
			prevMousePos = Input.mousePosition;
			isPanning = true;
		} 
		if (Input.GetMouseButtonUp(0)) {
			isPanning = false;
		}
		if (isPanning) {
			Vector3 mouseDelta = Input.mousePosition - prevMousePos;
//			newPos.z *= 0; // Don't change z axis
//			newPos.x *= zoomSpeed;
//			newPos.y *= zoomSpeed;
//			offset -= newPos;

//			float deltaX = mouseDelta.x;
			rotateAroundFlock (mouseDelta.x * rotationSpeedX);

			offset = targetPos - transform.position;

			prevMousePos = Input.mousePosition;
		}

	
		transform.LookAt (targetPos);
	}

	void rotateAroundFlock(float degrees) {
		transform.RotateAround (targetPos, Vector3.up, degrees);
	}

//	void zoomIn() {
//		offset *= 1.0f + zoomSpeed;
//	}
//
//	void zoomOut() {
//		offset *= 1.0f - zoomSpeed;
//	}
}