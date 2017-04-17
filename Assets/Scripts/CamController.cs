using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{

	public List<Camera> cameras;

	private int currentCamIndex;

	void Start ()
	{
		foreach (Camera cam in cameras) {
			cam.enabled = false;
		}
		currentCamIndex = 0;
		getCurrentCam ().enabled = true;
	}

	public void nextCam ()
	{
//		if (cameras.Count <= 1) {
//			return;
//		}
		Camera previousCam = getCurrentCam ();
		currentCamIndex++;
		if (currentCamIndex == cameras.Count) {
			currentCamIndex = 0;
		}
		Camera currentCam = getCurrentCam ();
		previousCam.enabled = !previousCam.enabled;
		currentCam.enabled = !currentCam.enabled;
	}

	public Camera getCurrentCam ()
	{
		return cameras [currentCamIndex];
	}

	public void zoomIn ()
	{
		getCurrentCam ().GetComponent <FollowCamera> ().zoomIn ();
	}

	public void zoomOut ()
	{
		getCurrentCam ().GetComponent <FollowCamera> ().zoomOut ();
	}
}
