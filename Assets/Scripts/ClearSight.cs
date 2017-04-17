using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Modified: http://answers.unity3d.com/questions/44815/make-object-transparent-when-between-camera-and-pl.html
public class ClearSight : MonoBehaviour
{
	public Material TransparentMaterial = null;
	public float FadeInTimeout = 0.6f;
	public float FadeOutTimeout = 0.2f;
	public float TargetTransparency = 0.3f;

	// The objects to keep in view (fade everyting between them and the camera - that is in the correct layer mask)
	public List<Transform> clearObjectsList;

	// Layer mask to only hit desired objects
	public LayerMask layerMask;

	private void Update ()
	{
		if (!GetComponent <Camera> ().enabled) {
			return;
		} 
		foreach (Transform viewObject in clearObjectsList) {
			RaycastHit lineHit = new RaycastHit ();
			if (Physics.Linecast (transform.position, viewObject.position, out lineHit, layerMask)) {
//				Debug.DrawLine (transform.position, lineHit.point, Color.cyan);
				Renderer R = lineHit.collider.GetComponent<Renderer> ();
				if (R == null) {
					continue;
				}
					
				makeTransparent (R);
			}
		}
	}

	private void makeTransparent (Renderer R)
	{
		AutoTransparent AT = R.GetComponent<AutoTransparent> ();
		if (AT == null) { // if no script is attached, attach one
			AT = R.gameObject.AddComponent<AutoTransparent> ();
			AT.TransparentMaterial = TransparentMaterial;
			AT.FadeInTimeout = FadeInTimeout;
			AT.FadeOutTimeout = FadeOutTimeout;
			AT.TargetTransparency = TargetTransparency;
		}
		AT.BeTransparent (); // get called every frame to reset the falloff
	}
}