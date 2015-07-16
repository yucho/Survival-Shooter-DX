using UnityEngine;
using System.Collections;

public class ToiletRoomController : MonoBehaviour
{
	
	void Awake ()
	{
		//
		// Force developers to fix bugs.
		//
		ExceptionHandler.SetupExceptionHandling ();
		
		NotificationCentre.AddObserver (this, "OnGameOver");
	}
	
	
	void Start ()
	{
		//
		//  Avoid notifying on Awake () because observers might register late ().
		//
		NotificationCentre.PostNotification (this, "PauseAllow");
		
		MissionManager.UpdateMission ("Explore  the  area.");
		
		switch (PlayerPrefs.GetString (PlayerPrefsKeys.PREV_SCENE))
		{
		case "Play Room" :
			NotificationCentre.PostNotification (this, "OnFadeIn");
			NotificationCentre.PostNotification (this, "OnBGMFadeIn");
			break;
		default :
			NotificationCentre.PostNotification (this, "OnFadeIn");
			NotificationCentre.PostNotification (this, "OnBGMFadeIn");
			break;
		}
	}
	
	
	void ResumeFromCheckpoint ()
	{
		NotificationCentre.PostNotification (this, "PauseAllow");
		
		switch (PlayerPrefs.GetString (PlayerPrefsKeys.CHECKPOINT))
		{
		default :
			NotificationCentre.PostNotification (this, "OnFadeIn");
			NotificationCentre.PostNotification (this, "OnBGMFadeIn");
			NotificationCentre.PostNotification (this, "OnEventExit");
			NotificationCentre.PostNotification (this, "OnResumeFromCheckpoint");
			break;
		}
	}
	
	
	IEnumerator OnGameOver ()
	{
		// Game Over is an event.
		NotificationCentre.PostNotification (this, "OnEventEnter");
		
		// Wait for animations and effects to finish.
		yield return new WaitForSeconds (2);
		
		NotificationCentre.PostNotification (this, "OnFadeOut");
		NotificationCentre.PostNotification (this, "OnBGMFadeOut");
		
		yield return new WaitForSeconds (2);
		
		ResumeFromCheckpoint ();
	}
}
