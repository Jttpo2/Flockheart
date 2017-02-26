using UnityEngine;
using System.Collections;

public class BoidWatcher : MonoBehaviour
{
	public Transform boidController;

	private Vector3 targetPos;

	private Vector3 offset;

	void Start ()
	{
		if (boidController) {
			targetPos = boidController.GetComponent <BoidController> ().getFlockCenter ();
			offset = targetPos - transform.position;
		}

	}

	void LateUpdate ()
	{
		if (boidController) {
//			float desiredAngle = target.transform.eulerAngles.y;
//			Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);

			targetPos = boidController.GetComponent <BoidController> ().getFlockCenter ();
			transform.position = targetPos - offset;

			transform.LookAt (targetPos);

//			Vector3 watchPoint = boidController.GetComponent<BoidController> ().getFlockCenter ();
//			transform.LookAt (watchPoint + boidController.transform.position);
		}
	}
}