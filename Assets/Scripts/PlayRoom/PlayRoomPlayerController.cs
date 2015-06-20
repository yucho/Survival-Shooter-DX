using UnityEngine;
using System.Collections;

//
//  Attach this script to an empty object in Play Room scene. It listens to
// the PlayRoomController for event notifications and manipulates player object.
//
public class PlayRoomPlayerController : MonoBehaviour
{

	private GameObject player;
	private PlayerShooting gun;


	void Awake ()
	{
		//NotificationCentre.AddObserver (this, "OnEventEnter");
		//NotificationCentre.AddObserver (this, "OnEventExit");
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnPlayerActivate");
		NotificationCentre.AddObserver (this, "OnBattleBegin");

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
			gun.enabled = false;
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
		if (gun) { gun.enabled = true; }
	}


	void SetPlayerPosRot (Vector3 pos, Vector3 euler)
	{
		player.transform.position = pos;
		player.transform.rotation = Quaternion.Euler (euler);
	}
}
