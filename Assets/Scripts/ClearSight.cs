using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSight : MonoBehaviour
{
	public float DistanceToPlayer = 5.0f;
	public Material TransparentMaterial = null;
	public float FadeInTimeout = 0.6f;
	public float FadeOutTimeout = 0.2f;
	public float TargetTransparency = 0.3f;

	// Layer mask to only hit desired objects
	public LayerMask layerMask;

	private void Update ()
	{
		RaycastHit[] hits; // you can also use CapsuleCastAll() 
		hits = Physics.RaycastAll (transform.position, transform.forward, DistanceToPlayer, layerMask);
		foreach (RaycastHit hit in hits) {
			Debug.DrawLine (transform.position, hit.point, Color.green);
			Renderer R = hit.collider.GetComponent<Renderer> ();
			if (R == null) {
				continue;
			}
			// no renderer attached? go to next hit 
			// TODO: maybe implement here a check for GOs that should not be affected like the player
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
}