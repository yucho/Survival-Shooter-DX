using UnityEngine;
using System.Collections;

//
//  Attach this script to an empty object in Play Room scene. It listens to
// the PlayRoomController for event notifications and manipulates player object.
//
public class PlayRoomPlayerController : MonoBehaviour
{
	public GameObject playerPrefab;

	private GameObject player;
	private PlayerShooting gun;


	void Awake ()
	{
		//NotificationCentre.AddObserver (this, "OnEventEnter");
		//NotificationCentre.AddObserver (this, "OnEventExit");
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnPlayerActivate");
		NotificationCentre.AddObserver (this, "OnBattleBegin");
		NotificationCentre.AddObserver (this, "OnEnemyRushOver");
		NotificationCentre.AddObserver (this, "OnEnemyRushOverExit");
		NotificationCentre.AddObserver (this, "OnResumeFromCheckpoint");

		GetPlayer ();
	}


	void GetPlayer ()
	{
		player = GameObject.FindWithTag ("Player");
		
		if (player)
		{
			gun = player.GetComponentInChildren<PlayerShooting> ();
		}
	}


	void OnIntroEvent ()
	{
		if (player)
			player.SetActive (false);
	}


	IEnumerator OnPlayerActivate ()
	{
		if (player)
		{
			player.SetActive (true);
			gun.EnableGun (false);
			SetPlayerPosRot(new Vector3 (21.5f,0,-1), new Vector3 (0,180,0));

			yield return new WaitForSeconds (1);

			ParticleController.Exclamation (player.transform.position, 2);

			yield return new WaitForSeconds(0.5f);

			yield return StartCoroutine(CustomUtilities.MovLocRot(player, Vector3.zero, new Vector3(0,90,0), 0.5f));

			yield return new WaitForSeconds (0.3f);

			NotificationCentre.PostNotification (this, "OnEnemyAppear");
		}
	}


	void OnBattleBegin ()
	{
		if (gun) { gun.EnableGun (true); }
	}


	IEnumerator OnEnemyRushOver ()
	{
		if (gun) { gun.EnableGun (false); }
		yield return new WaitForSeconds (1);

		if (player)
		{
			CustomUtilities.SetPosRot(player, Vector3.zero, new Vector3(0,180,0));
		}

		NotificationCentre.PostNotification (this, "OnZoomInCannon");
	}

	void OnEnemyRushOverExit ()
	{
		if (gun) { gun.EnableGun (true); }
	}


	IEnumerator OnResumeFromCheckpoint ()
	{
		if (player)
		{
			// Destroy the old player and instantiate the new.
			Destroy (player);
			player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gun = player.GetComponentInChildren<PlayerShooting> ();

			switch (PlayerPrefs.GetString (PlayerPrefsKeys.CHECKPOINT))
			{
			case "FirstBattle":
				player.SetActive (true);
				gun.enabled = true;
				SetPlayerPosRot(new Vector3 (21.5f,0,-1), new Vector3 (0,180,0));
				break;
			default:
				player.SetActive (true);
				gun.enabled = true;
				SetPlayerPosRot(Vector3.zero, Vector3.zero);
				break;
			}

			// Need to wait 1 frame for gameobject tag search to work.
			yield return null;
			NotificationCentre.PostNotification (this, "OnUpdatePlayer");
		}
	}


	void SetPlayerPosRot (Vector3 pos, Vector3 euler)
	{
		player.transform.position = pos;
		player.transform.rotation = Quaternion.Euler (euler);
	}
}
