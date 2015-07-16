using UnityEngine;
using System.Collections;

// Object touching "Shootable" layer object will trigger explosion.
public class ExplosiveCannonball : MonoBehaviour
{
	public GameObject explosionPrefab;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnEventEnter");
	}

	void OnEvententer ()
	{
		// Immediately destroy itself on event enter.
		Destroy (this.gameObject);
	}

	void OnCollisionEnter (Collision collision)
	{
		//Debug.Log ("Collision with " + LayerMask.LayerToName(collision.collider.gameObject.layer) + "object");
		if (explosionPrefab && ((1<<collision.collider.gameObject.layer) & LayerMask.GetMask("Shootable", "Enemy", "Player")) != 0)
		{
			LayerMask.GetMask("Shootable", "Enemy", "Player");
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}
}
