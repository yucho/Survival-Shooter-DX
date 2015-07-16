using UnityEngine;
using System.Collections;

//
//  Attach this script to an empty object in Play Room scene. It listens to
// the PlayRoomController for event notifications and manipulates camera object.
// Attach CameraMovement script to the camera object and tag it "MainCamera".
//
public class PlayRoomCameraController : MonoBehaviour
{

	private GameObject player;
	private GameObject pe;

	private CameraMovement cam;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnIntroEventExit");
		NotificationCentre.AddObserver (this, "OnPickUpGun");
		NotificationCentre.AddObserver (this, "OnHoldingGunHigh");
		NotificationCentre.AddObserver (this, "OnPlayerActivate");
		NotificationCentre.AddObserver (this, "OnEnemyAppear");
		NotificationCentre.AddObserver (this, "OnBattleBegin");
		NotificationCentre.AddObserver (this, "OnZoomInCannon");
		NotificationCentre.AddObserver (this, "OnCameraNormalize");
		NotificationCentre.AddObserver (this, "OnUpdatePlayer");

		player = GameObject.FindWithTag ("Player");
		pe = GameObject.FindWithTag ("PlayerEvent");

		cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement> ();
		if (cam)
		{
			cam.gameObject.SetActive (true);
			cam.SetCameraOffset (new Vector3 (0, 5, -7));
			cam.SetCameraTarget (player);
			cam.SetCameraFollow (true);
			SetCamPosRot(new Vector3 (0, 6, -7), new Vector3(40,0,0));
		}
	}


	// Descend from sky.
	IEnumerator OnIntroEvent ()
	{
		if (pe && cam)
		{
			cam.SetCameraFollow (false);

			SetCamPosRot (new Vector3 (-8, 34, -4), new Vector3 (40, 0, 0));

			yield return StartCoroutine (DescendFromSky ());

			SetCamPosRot (pe.transform.position + new Vector3 (0, 3, -4), new Vector3 (30, 0, 0));

			yield return new WaitForSeconds (1);

			NotificationCentre.PostNotification (this, "OnIntroEventCameraDone");
		}
	}
	
	IEnumerator DescendFromSky ()
	{
		float timer = 0;

		while (cam)
		{
			cam.transform.position = Vector3.Lerp (new Vector3 (-8, 34, -4),
			                                       new Vector3 (-8, 20, -4), timer / 10);
			if (timer >= 7)
				break;

			timer += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator OnIntroEventExit ()
	{
		if (pe && cam)
		{
			cam.SetCameraOffset (new Vector3 (0, 5, -7));
			cam.SetCameraTarget (pe);
			cam.SetCameraFollow (true);

			float timer = 0;
			Quaternion a = cam.transform.rotation;
			while (true)
			{
				cam.transform.rotation = Quaternion.Lerp (a, Quaternion.Euler (new Vector3 (40, 0, 0)), timer / 2);

				if (timer > 2)
					break;

				timer += Time.deltaTime;
				yield return null;
			}

			NotificationCentre.PostNotification (this, "DisplayHowToPlay");
			MissionManager.UpdateMission ("Search  the  room  for  something  useful.");
		}
	}


	IEnumerator OnPickUpGun ()
	{
		if (pe && cam)
		{
			cam.SetCameraFollow (false);
			SetCamPosRot (new Vector3 (22,2,-2), new Vector3 (60,0,0));

			yield return new WaitForSeconds(3);

			StartCoroutine (MovLocRot (new Vector3 (0,1,-1), Vector3.zero, 1));
			yield return null;
		}
	}


	IEnumerator OnHoldingGunHigh ()
	{
		if (pe && cam)
			StartCoroutine (MovLocRot (new Vector3 (-0.5f,0.5f,0.5f), Vector3.zero, 0.2f));

		yield return null;
	}


	void OnPlayerActivate ()
	{
		SetCamPosRot (new Vector3 (21.5f,0.5f,-3), new Vector3 (-5,0,0));
	}


	void OnEnemyAppear ()
	{
		cam.gameObject.SetActive (false);
	}


	void OnBattleBegin ()
	{
		OnCameraNormalize ();
		StartCoroutine (ClickToShoot ());
	}

	void OnCameraNormalize ()
	{
		if (cam && player)
		{
			SetCamPosRot(new Vector3 (0, 6, -7), new Vector3(40,0,0));
			
			cam.gameObject.SetActive (true);
			cam.SetCameraOffset (new Vector3 (0, 5, -7));
			cam.SetCameraTarget (player);
			cam.SetCameraFollow (true);
		}
	}


	IEnumerator ClickToShoot ()
	{
		Hashtable table = new Hashtable {
			{ "TextAnchor", "UpperCenter" },
			{ "Text", "Click  to  shoot  enemies!!" }
		};
		NotificationCentre.PostNotification (this, "DisplayText", table);

		yield return new WaitForSeconds(3);

		NotificationCentre.PostNotification (this, "HideText");
	}


	IEnumerator OnZoomInCannon ()
	{
		if (cam)
		{
			cam.SetCameraFollow (false);
			CustomUtilities.SetPosRot (cam.gameObject, new Vector3(0,1,-3), Vector3.zero);
		}

		NotificationCentre.PostNotification (this, "OnFadeIn");
		NotificationCentre.PostNotification (this, "OnBGMFadeIn");
		
		yield return new WaitForSeconds (3);

		// Final Pos (3,16,24) Rot (25,30,0)
		if (cam)
		{
			yield return StartCoroutine (CustomUtilities.MovLocRot(cam.gameObject, new Vector3(3,15,27), new Vector3(25,30,0), 2));
		}

		yield return new WaitForSeconds (1);

		NotificationCentre.PostNotification (this, "OnZombunnyLaugh");

		yield return new WaitForSeconds (0.5f);

		NotificationCentre.PostNotification (this, "OnIgniteCannon");

		yield return new WaitForSeconds (0.5f);

		if (cam)
		{
			// Final Pos (3,16,24) Rot (35,45,0)
			yield return StartCoroutine (CustomUtilities.MovLocRot(cam.gameObject, Vector3.zero, new Vector3(10,15,0), 0.7f));
			// Final Pos (4.2,16.8,22) Rot (35,5,0)
			yield return StartCoroutine (CustomUtilities.MovLocRot(cam.gameObject, new Vector3(1.2f,0.8f,-2), new Vector3(0,-40,0), 0.8f));
		}

		NotificationCentre.PostNotification (this, "OnFadeOut");

		yield return new WaitForSeconds (1);

		OnCameraNormalize ();
		NotificationCentre.PostNotification (this, "OnEventExit");
		NotificationCentre.PostNotification (this, "OnEnemyRushOverExit");
		NotificationCentre.PostNotification (this, "OnNaturalSpawn");
		NotificationCentre.PostNotification (this, "OnFadeIn");
		MissionManager.UpdateMission ("This  room  is  a  nightmare!  Find  the  way  out.");

		yield return new WaitForSeconds (3);

		NotificationCentre.PostNotification (this, "ActivateCannon");
	}


	void OnUpdatePlayer ()
	{
		player = GameObject.FindWithTag ("Player");

		// Glide camera in.
		if (player && cam)
		{
			cam.SetCameraOffset (new Vector3 (0, 5, -7));
			cam.SetCameraTarget (player);
			cam.SetCameraFollow (true);

			//OnCameraNormalize ();

			CustomUtilities.SetPosRot (cam.gameObject, player.transform.position + new Vector3(-2,6,-6), new Vector3(40,0,0));
		}
	}


	void SetCamPosRot (Vector3 pos, Vector3 euler)
	{
		cam.transform.position = pos;
		cam.transform.rotation = Quaternion.Euler (euler);
	}

	// Pass offset parameters as they get added to current loc & rot.
	IEnumerator MovLocRot (Vector3 loc, Vector3 angles, float duration)
	{
		Vector3 locA = cam.transform.position;
		Vector3 locB = locA + loc;
		Quaternion rotA = cam.transform.rotation;
		Quaternion rotB = rotA * Quaternion.Euler (angles);

		float timer = 0;
		while (true)
		{
			cam.transform.position = Vector3.Lerp (locA, locB, timer / duration);
			cam.transform.rotation = Quaternion.Lerp (rotA, rotB, timer / duration);

			if (timer >= duration)
				break;
			
			timer += Time.deltaTime;
			yield return null;
		}
	}
}

/*   Self Memo

Good default cemra offset in Play Room scene is pos(0, 5, -7) and rot(40, 0, 0).

*/
