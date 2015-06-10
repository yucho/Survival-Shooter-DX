using UnityEngine;
using System.Collections;

//
//  Attach this script to an empty object in Play Room scene. It listens to
// the PlayRoomController for event notifications and manipulates playerevent object.
//
public class PlayRoomPlayerEventController : MonoBehaviour
{

	private GameObject pe;
	//private MeshRenderer [] meshes;
	//private SkinnedMeshRenderer [] skins;
	
	
	void Awake ()
	{
		//NotificationCentre.AddObserver (this, "OnEventEnter");
		//NotificationCentre.AddObserver (this, "OnEventExit");
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnIntroEventCameraDone");
		NotificationCentre.AddObserver (this, "OnPickUpGun");
		NotificationCentre.AddObserver (this, "OnPlayerActivate");
		
		GetPlayer ();
	}
	
	
	void GetPlayer ()
	{
		pe = GameObject.FindWithTag ("PlayerEvent");
		
		if (pe)
		{
			//meshes = pe.GetComponentsInChildren<MeshRenderer> ();
			//skins = pe.GetComponentsInChildren<SkinnedMeshRenderer> ();
		}
	}
	

	// Make PE sleep in front of the dollhouse.
	void OnIntroEvent ()
	{
		if (pe)
		{
			pe.SetActive (true);

			SetPEPosRot (new Vector3 (-20, 0, 6), new Vector3(0, 180, 0));

			Animator a = pe.GetComponent<Animator> ();
			if (a)
			{
				a.SetTrigger ("Sleep");
			}
		}
	}

	IEnumerator OnIntroEventCameraDone ()
	{
		Animator a = pe.GetComponent<Animator> ();
		if (a)
		{
			a.SetTrigger ("Wake");
		

			yield return new WaitForSeconds (7);

			// Player can't turn along the mouse pointer.
			PlayerMovement pm = pe.GetComponent<PlayerMovement> ();
			if (pm)
				pm.FacePointer (false);

			NotificationCentre.PostNotification (this, "OnIntroEventExit");

			//a.SetTrigger ("Standby"); // Wake automatically transitions to Idle
		}
	}

	IEnumerator OnPickUpGun ()
	{
		if (pe)
		{
			SetPEPosRot(new Vector3 (21.5f,0,-1), new Vector3 (0,90,0));

			Animator a = pe.GetComponent<Animator> ();
			if (a)
			{
				a.SetTrigger ("Standby");
				yield return new WaitForSeconds (2);
				a.SetTrigger ("Crouch");
				yield return new WaitForSeconds (3);

				a.SetTrigger ("Raise");
				NotificationCentre.PostNotification (this, "OnHoldingGunHigh");

				Quaternion rotA = pe.transform.rotation;
				Quaternion rotB = Quaternion.Euler (0,180,0);
				float timer = 0;
				while (true)
				{
					pe.transform.rotation = Quaternion.Lerp (rotA, rotB, timer / 0.4f);
					
					if (timer >= 0.4f)
						break;
					
					timer += Time.deltaTime;
					yield return null;
				}


			}
		}

		yield return null;
	}


	void OnPlayerActivate ()
	{
		pe.SetActive (false);
	}




	void SetPEPosRot (Vector3 pos, Vector3 euler)
	{
		pe.transform.position = pos;
		pe.transform.rotation = Quaternion.Euler (euler);
	}
}
