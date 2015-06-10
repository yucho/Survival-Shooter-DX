using UnityEngine;
using System.Collections;

//
//  Attach this script to an empty object in Play Room scene. It listens to
// the PlayRoomController for event notifications and manipulates camera object.
// Attach CameraMovement script to the camera object and tag it "MainCamera".
//
public class PlayRoomCameraController : MonoBehaviour
{

	//private GameObject player;
	private GameObject pe;

	private CameraMovement cam;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnIntroEventExit");
		NotificationCentre.AddObserver (this, "OnPickUpGun");
		NotificationCentre.AddObserver (this, "OnHoldingGunHigh");
		NotificationCentre.AddObserver (this, "OnPlayerActivate");

		//player = GameObject.FindWithTag ("Player");
		pe = GameObject.FindWithTag ("PlayerEvent");

		cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement> ();
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
			cam.SetCameraTarget (pe.transform);
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
