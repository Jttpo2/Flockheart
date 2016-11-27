using UnityEngine;
using System.Collections;

public class BoidWatcher : MonoBehaviour
{
	public Transform boidController;

	void LateUpdate ()
	{
		if (boidController) {
			Vector3 watchPoint = boidController.GetComponent<BoidController> ().getFlockCenter ();
			transform.LookAt (watchPoint + boidController.transform.position);
		}
	}
}