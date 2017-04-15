using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

	public Rigidbody projectile;
	public Transform target;

	float muzzleVelocity = 2000.0f;

	private Transform barrel;

	//	private Vector3 aimHigh = new Vector3 (0, 100, 0);
	private Vector3 aimHigh = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
		barrel = transform.Find ("Barrel");
		pointBarrelAt (target);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target) {
			pointBarrelAt (target);
//			Debug.DrawLine (transform.position, target.position);
		}
	}

	public void shoot ()
	{
		load (projectile);
		fire ();
	}

	void fire ()
	{
		if (!target || !projectile) {
			return;
		}
			
		Vector3 toTarget = target.position - transform.position;
		toTarget.Normalize ();
		toTarget *= muzzleVelocity * projectile.mass;
//		Debug.Log ("Fire: " + toTarget + " pMass: " + projectile.mass + " mVel: " + muzzleVelocity);
		// Aim a bit higher than target
		toTarget += aimHigh;
		projectile.AddForce (toTarget);
	}

	void load (Rigidbody projectile)
	{
		if (!projectile) {
			return;
		}

		// Reset projectile motion
		projectile.velocity = Vector3.zero;
		// Move the projectile into the cannon
		projectile.position = transform.position;
	}

	void pointBarrelAt (Transform target)
	{
		if (!barrel) {
			return;
		}
		barrel.LookAt (target.position + aimHigh);
		barrel.Rotate (new Vector3 (90, 0, 0));
	}
}
