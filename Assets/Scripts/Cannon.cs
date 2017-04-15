using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

	public Rigidbody projectile;
	public Transform target;

	float muzzleVelocity = 2000.0f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
		toTarget += new Vector3 (0, 100, 0); 
		projectile.AddForce (toTarget);
	}

	void load (Rigidbody projectile)
	{
		if (!projectile) {
			return;
		}

		// Move the projectile into the cannon
		projectile.position = transform.position;
		// Reset projectile motion
		projectile.velocity = Vector3.zero;
	}


}
