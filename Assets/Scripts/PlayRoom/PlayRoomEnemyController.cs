﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayRoomEnemyController : MonoBehaviour
{
	public static int enemyNumber = 0; // current number of enemies in the scene.
	public static int spawnNumber = 0; // use this to count spawns for event, etc.
	
	public EnemyEventMovement zombunny;
	public EnemyEventMovement zombear;

	public GameObject zombunnyPrefab;
	public GameObject zombearPrefab;

    private EnemySpawn [] spawnPoints;
	

    void Awake ()
    {
		spawnPoints = GetComponentsInChildren<EnemySpawn> ();

		NotificationCentre.AddObserver (this, "OnEnemyAppear");
		NotificationCentre.AddObserver (this, "OnZombearAppear");
		NotificationCentre.AddObserver (this, "OnPanOut");
		NotificationCentre.AddObserver (this, "OnBattleBegin");

		if (zombunny) { zombunny.gameObject.SetActive (false); }
		if (zombear) { zombear.gameObject.SetActive (false); }
    }


    public void SetActive (bool active)
    {
		foreach (EnemySpawn spawnPoint in spawnPoints)
		{
			spawnPoint.SetActive (active);
		}
    }


	IEnumerator OnEnemyAppear ()
	{
		if (zombunny)
		{
			CustomUtilities.SetPosRot (zombunny.gameObject, Vector3.zero, Vector3.zero);
			zombunny.gameObject.SetActive (true);
		}

		yield return new WaitForSeconds (1);

		GameObject player = GameObject.FindWithTag ("Player");

		if (player && zombunny)
			zombunny.Target = player;
		//zombunny.followTarget = true;

		if (zombunny)
			yield return StartCoroutine (CustomUtilities.MovLocRot (zombunny.gameObject, Vector3.zero, new Vector3(0,90,0), 0.5f));

		yield return new WaitForSeconds (0.75f);

		StartCoroutine (OnZombunnyAppear ());
	}


	IEnumerator OnZombunnyAppear ()
	{
		NotificationCentre.PostNotification (this, "OnZombunnyAppear");

		yield return new WaitForSeconds(0.3f);

		if(zombunny)
			zombunny.Pause ();
	}


	IEnumerator OnZombearAppear ()
	{
		if(zombunny)
			zombunny.Resume ();

		GameObject player = GameObject.FindWithTag ("Player");

		if (zombear & player)
		{
			zombear.gameObject.SetActive (true);
			zombear.Target = player;

			yield return new WaitForSeconds (0.3f);

			CustomUtilities.SetPosRot (zombear.gameObject, new Vector3(5,0,7), Vector3.zero);
			yield return StartCoroutine (CustomUtilities.MovLocRot (zombear.gameObject, Vector3.zero, new Vector3(0,-160,0), 0.5f));

			zombear.Pause ();
		}

		yield return null;
	}


	IEnumerator OnPanOut ()
	{
		if (zombunny) { zombunny.followTarget = true; zombunny.Resume ();}
		if (zombear) { zombear.followTarget = true; zombear.Resume (); }

		yield return new WaitForSeconds (4.5f);

		if (zombunny) { zombunny.followTarget = false; zombunny.gameObject.SetActive (false);}
		if (zombear) { zombear.followTarget = false; zombear.gameObject.SetActive (false); }
	}


	void OnBattleBegin ()
	{
		spawnNumber = 0;

		if (zombunnyPrefab)
		{
			enemyNumber ++;
			spawnNumber ++;
			Instantiate (zombunnyPrefab, Vector3.zero, Quaternion.identity);
		}

		if (zombearPrefab)
		{
			enemyNumber ++;
			spawnNumber ++;
			Instantiate (zombearPrefab, new Vector3(5,0,7), Quaternion.identity);
		}

		OnEnemyRush ();
	}


	void OnEnemyRush ()
	{
		foreach (EnemySpawn spawnPoint in spawnPoints)
		{
			StartCoroutine(spawnPoint.EnemyRush (20));
		}

		StartCoroutine (OnEnemyRushOver ());
	}


	IEnumerator OnEnemyRushOver ()
	{
		// Wait until all enemies are dead.
		while (enemyNumber > 0)
		{
			yield return null;
		}


	}
}
