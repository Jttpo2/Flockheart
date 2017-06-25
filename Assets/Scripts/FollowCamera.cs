using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
	public Transform watchee;

	private Vector3 targetPos;

	private Vector3 offset;
	private Vector3 mouseOrigin;
	private Vector3 prevMousePos;
	private bool isPanning;

	private float rotationSpeedX = 0.2f;
	private float rotationSpeedY = 0.2f;

	public float standardZoomStep = 0.1f;
	// Currently mimum working value, from manual testing
	private float minZoomDistance = 6.0f;

	void Start ()
	{
		prevMousePos = Vector3.zero;

		if (watchee) {
			targetPos = getTargetPos ();
			offset = targetPos - transform.position;
		}
	}

	void LateUpdate ()
	{
		if (!watchee) {
			return;
		}
		
		targetPos = getTargetPos ();
		transform.position = targetPos - offset;


		if (Input.GetMouseButtonDown (0)) {
			prevMousePos = Input.mousePosition;
			isPanning = true;
		} 
		if (Input.GetMouseButtonUp (0)) {
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

	void rotateAroundWatchee (float degrees)
	{
		transform.RotateAround (targetPos, Vector3.up, degrees);
	}

	void panUpAndDown (float degrees)
	{
		//	Pan axis is horizontal and perpendicular to sight line
		Vector3 panAxis = Vector3.Cross (offset, Vector3.up);

//		Debug.Log (transform.rotation.eulerAngles.x + degrees);
//		if (degrees > 0 && transform.rotation.eulerAngles.x + degrees >= 90) {
//			Debug.Log ("Over 90");
//			return;
//		}

		transform.RotateAround (targetPos, panAxis, degrees);
//		transform.rotation.eulerAngles
	}

	Vector3 getTargetPos ()
	{
		return watchee.position;
	}

	public void zoom (float amount)
	{
		Vector3 desiredOffset = offset + offset.normalized * amount;
		if (desiredOffset.magnitude < minZoomDistance) {
			offset.Normalize ();
			offset *= minZoomDistance;
		} else {
			offset = desiredOffset;
		}
	}

	public void zoomIn ()
	{
		zoom (-standardZoomStep);
	}

	public void zoomOut ()
	{
		zoom (standardZoomStep);
	}

	//	void follow() {
	//		float desiredAngle = target.transform.eulerAngles.y;
	//		Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);
	//		transform.position = target.transform.position - (rotation * offset);
	//
	//		transform.LookAt (target.transform);
	//	}
}