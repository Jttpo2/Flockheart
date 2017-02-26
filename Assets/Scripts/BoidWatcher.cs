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
			targetPos = boidController.GetComponent <BoidController> ().getFlockCenter ();
			transform.position = targetPos - offset;

			transform.LookAt (targetPos);
		}
	}
}