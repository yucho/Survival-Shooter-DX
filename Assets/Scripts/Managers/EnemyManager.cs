using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    private EnemySpawn [] spawnPoints;

    void Awake ()
    {
		spawnPoints = GetComponentsInChildren<EnemySpawn> ();
    }


    public void SetActive (bool active)
    {
		foreach (EnemySpawn spawnPoint in spawnPoints)
		{
			spawnPoint.SetActive (active);
		}
    }
}
