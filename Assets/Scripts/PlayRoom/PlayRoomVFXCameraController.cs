using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayRoomVFXCameraController : MonoBehaviour
{

	public GameObject vfxcam;

	public GameObject splashEffect;
	public GameObject splashEffect2;
	public GameObject splashEffect3;

	public GameObject textZombunny;
	public GameObject textZombear;


	GameObject empty;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnEnemyAppear");
		NotificationCentre.AddObserver (this, "OnZombunnyAppear");

		if (vfxcam) { vfxcam.SetActive (false); }
		if (splashEffect) { splashEffect.SetActive (false); }
		if (splashEffect2) { splashEffect2.SetActive (false); }
		if (splashEffect3) { splashEffect3.SetActive (false); }
		if (textZombunny) { textZombunny.SetActive (false); }
		if (textZombear) { textZombear.SetActive (false); }
	}


	IEnumerator OnEnemyAppear ()
	{
		if (vfxcam)
		{
			splashEffect.SetActive (false);;
			vfxcam.SetActive (true);

			CustomUtilities.SetPosRot (vfxcam, new Vector3(22,2,-2), new Vector3(20,-70,0));

			NotificationCentre.PostNotification (this, "OnSFXSwift");

			yield return new WaitForSeconds (0.5f);

			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, new Vector3(-18.5f,0,0), Vector3.zero, 0.7f));
		}
	}


	IEnumerator OnZombunnyAppear ()
	{
		if (vfxcam)
		{
			CustomUtilities.SetPosRot (vfxcam, new Vector3(3.5f,2,-2), new Vector3(20,-70,0));
			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, new Vector3(-1.5f,-1.5f,2), new Vector3(-20,-20,0), 0.5f));

			NotificationCentre.PostNotification (this, "OnSFXVFX1");

			if (splashEffect)
			{
				CustomUtilities.SetPosRotLocal (splashEffect, new Vector3(0.15f,0.55f,8), Vector3.zero);
				splashEffect.SetActive (true);
			}
			
			yield return new WaitForSeconds (0.1f);
			
			if (splashEffect2)
			{
				CustomUtilities.SetPosRotLocal (splashEffect2, new Vector3(-8,-3.5f,8), Vector3.zero);
				splashEffect2.SetActive (true);
			}
			
			yield return new WaitForSeconds (0.1f);
			
			if (splashEffect3)
			{
				CustomUtilities.SetPosRotLocal (splashEffect3, new Vector3(-4,-2,6), Vector3.zero);
				splashEffect3.SetActive (true);
			}

			if (textZombunny)
				textZombunny.SetActive (true);

			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, new Vector3(0,0.2f,-0.1f), Vector3.zero, 4));

			if (splashEffect) { splashEffect.SetActive (false); }
			if (splashEffect2) { splashEffect2.SetActive (false); }
			if (splashEffect3) { splashEffect3.SetActive (false); }
			if (textZombunny) { textZombunny.SetActive (false); }

			StartCoroutine (OnZombearAppear ());
		}
	}


	IEnumerator OnZombearAppear ()
	{
		NotificationCentre.PostNotification (this, "OnZombearAppear");
		NotificationCentre.PostNotification (this, "OnMonsterCry");

		if (vfxcam)
		{
			CustomUtilities.SetPosRot (vfxcam, new Vector3(2,0.5f,0), new Vector3(0,-90,0));
			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, Vector3.zero, new Vector3(0,90,0), 0.3f));
		}

		yield return new WaitForSeconds (0.45f);

		NotificationCentre.PostNotification (this, "OnSFXVFX2");

		if (vfxcam)
		{
			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, new Vector3(3,0.5f,5), Vector3.zero, 0.2f));
		}

		if (splashEffect)
		{
			CustomUtilities.SetPosRotLocal (splashEffect, new Vector3(6,1.5f,7), new Vector3(0,0,60));
			splashEffect.SetActive (true);
		}
		
		yield return new WaitForSeconds (0.1f);
		
		if (splashEffect2)
		{
			CustomUtilities.SetPosRotLocal (splashEffect2, new Vector3(0,-3.5f,8), new Vector3(0,0,90));
			splashEffect2.SetActive (true);
		}
		
		yield return new WaitForSeconds (0.1f);
		
		if (splashEffect3)
		{
			CustomUtilities.SetPosRotLocal (splashEffect3, new Vector3(-1,1,4), new Vector3(0,0,180));
			splashEffect3.SetActive (true);
		}
		
		if (textZombear)
			textZombear.SetActive (true);

		if (vfxcam)
		{
			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, Vector3.zero, new Vector3(-1,-5,0), 4f));
			//yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, new Vector3(-0.2f,0.1f,0), Vector3.zero, 4f));
		}

		if (splashEffect) { splashEffect.SetActive (false); }
		if (splashEffect2) { splashEffect2.SetActive (false); }
		if (splashEffect3) { splashEffect3.SetActive (false); }
		if (textZombear) { textZombear.SetActive (false); }
		
		StartCoroutine (OnPanOut ());
	}


	IEnumerator OnPanOut ()
	{
		NotificationCentre.PostNotification (this, "OnPanOut");

		if (vfxcam)
		{
			//CustomUtilities.SetPosRot (vfxcam, new Vector3(5,1,5), new Vector3(0,-35,0));

			yield return StartCoroutine (CustomUtilities.MovLocRot (vfxcam, Vector3.zero, new Vector3(1,-45,0), 0.5f));

			StartCoroutine (CustomUtilities.MovLocRot (vfxcam, new Vector3(15,0,-8), Vector3.zero, 2));

			//CustomUtilities.SetPosRot (vfxcam, new Vector3(18,1,-3), new Vector3(0,-35,0));
		}

		NotificationCentre.PostNotification (this, "OnBattleBGM");

		yield return new WaitForSeconds(3);

		NotificationCentre.PostNotification (this, "OnFadeOut");

		yield return new WaitForSeconds(1.5f);

		if (vfxcam) { vfxcam.SetActive (false); }

		NotificationCentre.PostNotification (this, "OnFadeIn");
		NotificationCentre.PostNotification (this, "OnEventExit");
		NotificationCentre.PostNotification (this, "OnBattleBegin");

		MissionManager.UpdateMission ("Survive  the  horde.");
	}

}
