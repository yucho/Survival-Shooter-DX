using UnityEngine;
using System.Collections;

public class PlayRoomController : MonoBehaviour
{

	void Awake ()
	{
		//
		// Force developers to fix bugs.
		//
		ExceptionHandler.SetupExceptionHandling ();
	}


	void Start ()
	{
		// Deactivate intro event for debug purpose:
		PlayerPrefs.SetString(PlayerPrefsKeys.PREV_SCENE, "None");
		NotificationCentre.PostNotification (this, "ActivateCannon"); // Debug the cannon.

		//
		//  Avoid notifying on Awake () because observers might register on Start ().
		//

		NotificationCentre.PostNotification (this, "PauseAllow");

		MissionManager.UpdateMission ("None");

		switch (PlayerPrefs.GetString (PlayerPrefsKeys.PREV_SCENE))
		{
			case "Title Scene" :
				NotificationCentre.PostNotification (this, "OnEventEnter");
				NotificationCentre.PostNotification (this, "OnBGMFadeIn");
				NotificationCentre.PostNotification (this, "OnIntroEvent");
				break;
			default :
				NotificationCentre.PostNotification (this, "OnFadeIn");
				NotificationCentre.PostNotification (this, "OnBGMFadeIn");
				break;
		}
	}
}
