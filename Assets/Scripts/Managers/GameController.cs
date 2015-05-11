using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	private EnemyManager enemies;

	void Awake ()
	{
		/**
		 *  Force developers to fix bugs.
		 */
		ExceptionHandler.SetupExceptionHandling();

		enemies = GameObject.FindWithTag("SpawnPoints").GetComponent<EnemyManager> ();
	}

	void Start ()
	{
		//.Temporary
		//enemies.SetActive (true);
	}
}
