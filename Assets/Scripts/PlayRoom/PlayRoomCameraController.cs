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





	void SetCamPosRot (Vector3 pos, Vector3 euler)
	{
		cam.transform.position = pos;
		cam.transform.rotation = Quaternion.Euler (euler);
	}
}

/*   Self Memo

Good default cemra offset in Play Room scene is pos(0, 5, -7) and rot(40, 0, 0).

*/
