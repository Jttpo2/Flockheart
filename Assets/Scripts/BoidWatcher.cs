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
			rotateAroundFlock (mouseDelta.x * rotationSpeedX);

			// Update offset
			offset = targetPos - transform.position;

			prevMousePos = Input.mousePosition;
		}
			
		transform.LookAt (targetPos);
	}

	void rotateAroundFlock(float degrees) {
		transform.RotateAround (targetPos, Vector3.up, degrees);
	}
}