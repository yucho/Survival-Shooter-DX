using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
	public List<GameObject> enemies;
	public float spawnTime = 20f; // one per time.

	private bool isPlayerDistant;
	
	void Awake ()
	{
		isPlayerDistant = true;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			isPlayerDistant = false;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			isPlayerDistant = true;
		}
	}

	public void SetActive (bool active)
	{
		if (active)
		{
			InvokeRepeating ("Spawn", spawnTime / 2, spawnTime);
		}
		else
		{
			CancelInvoke ("Spawn");
		}
	}

	public void Spawn ()
	{
		int index = Random.Range (0, enemies.Count);

		if (isPlayerDistant)
		{
			Instantiate (enemies [index], transform.position, transform.rotation);
		}
	}
}
