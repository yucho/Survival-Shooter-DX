using UnityEngine;
using System.Collections;

public class EnemyHitBox : MonoBehaviour
{
	public int strength = 20;
	public float knockBack = 10f; // better be less.
	
	private HealthPoint playerHP;
	private bool isActive;

	void Awake ()
	{
		playerHP = GameObject.FindWithTag ("HealthPoint").GetComponent<HealthPoint> ();

		isActive = false;
	}

	public void ToggleHitBox (bool toggle)
	{
		/**
		 *  This prevents hitting multiple times per swing.
		 */
		isActive = toggle;
	}

	/**
	 *  Note: It's important to add kinematic rigidbody to the hit box child
	 *        in order to separate this trigger from the parents'.
	 */
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player" && isActive)
		{
			Vector3 direction = (other.transform.position - transform.position).normalized;
			KnockBackPlayer (other.gameObject, direction);

			Vector3 fromDirection = -direction;
			playerHP.TakeDamage (strength, fromDirection);

			isActive = false;
		}
	}

	void KnockBackPlayer (GameObject player, Vector3 direction)
	{
		direction.y = 0.5f;

		Rigidbody body = player.GetComponent<Rigidbody> ();

		if (body)
		{
			/**
			 *  Add velocity with cut-off threshold.
			 */
			body.velocity = Mathf.Min (knockBack, 10f) * direction;
		}
	}
}
