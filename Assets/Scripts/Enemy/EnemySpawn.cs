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

	public IEnumerator EnemyRush (int number)
	{
		// Spawn number of enemies in random interval
		float interval = Random.Range (2f, 10f);

		while (true)
		{
			yield return new WaitForSeconds (interval);

			if (PlayRoomEnemyController.spawnNumber >= number)
				break;

			bool success = Spawn ();
			if (success)
			{
				PlayRoomEnemyController.spawnNumber ++;

				// Spawn succeeded, so make interval long.
				interval = Random.Range (2f, 10f);
			}
			else
			{
				// Spawn failed, so make interval short.
				interval = Random.Range (0.5f, 2f);
			}
		}
	}

	public bool Spawn ()
	{
		int index = Random.Range (0, enemies.Count);

		if (isPlayerDistant && PlayRoomEnemyController.enemyNumber < SSGlobals.MAX_NUM_ENEMY)
		{
			Instantiate (enemies [index], transform.position, transform.rotation);
			PlayRoomEnemyController.enemyNumber ++;

			return true;
		}

		return false;
	}
}
