using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

	public Rigidbody projectile;
	public Transform target;

	float muzzleVelocity = 2000.0f;

	private Transform barrel;

	private Vector3 aimHigh = new Vector3 (0, 10, 0);
	//	private Vector3 aimHigh = Vector3.zero;
	private Vector3 upToForward = new Vector3 (90, 0, 0);

	// Use this for initialization
	void Start ()
	{
		barrel = transform.Find ("Barrel");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target) {
			aimAt (target);
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

		Vector3 toTarget = barrel.up.normalized * muzzleVelocity * projectile.mass;
//		Debug.DrawRay (transform.position, toTarget, Color.red);

		projectile.AddForce (toTarget);
	}

	void load (Rigidbody projectile)
	{
		if (!projectile) {
			return;
		}

		// Reset projectile motion
		projectile.velocity = Vector3.zero;
		projectile.angularVelocity = Vector3.zero;
		// Move the projectile into the cannon
		projectile.position = transform.position;
		alignWithBarrel (projectile);
	}

	void aimAt (Transform target)
	{
		if (!barrel) {
			return;
		}
		barrel.LookAt (target.position + aimHigh);
		barrel.Rotate (upToForward);
	}

	void alignWithBarrel (Rigidbody projectile)
	{
		projectile.rotation = barrel.rotation;
	}
}
