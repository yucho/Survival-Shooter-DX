using UnityEngine;
using System.Collections;

// Attach this to detonator prefab and assign a collider.
public class DetonatorHitBox : MonoBehaviour
{
	public int damage;
	public float life;

	private float timer;

	void Start ()
	{
		timer = 0;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (timer >= life)
		{
			Collider hitBox = GetComponent<Collider> ();
			if (hitBox)
			{
				hitBox.enabled = false;
			}
			this.enabled = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			GameObject HPBar = GameObject.FindWithTag("HealthPoint");
			HealthPoint HP = null;

			if (HPBar)
			{
				HP = HPBar.GetComponent<HealthPoint> ();
			}
			if (HP)
			{
				Vector3 direction = (transform.position - other.gameObject.transform.position).normalized;
				HP.TakeDamage (damage, direction);
			}
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			EnemyHealth HP = other.gameObject.GetComponent<EnemyHealth> ();
			if (HP)
			{
				HP.TakeDamage (damage * 2, Vector3.zero);
			}
		}

		// If it's flammable block.
		FlammableBlock block = other.gameObject.GetComponent<FlammableBlock> ();
		if (block)
		{
			block.TakeHit (damage);
		}
	}
}
