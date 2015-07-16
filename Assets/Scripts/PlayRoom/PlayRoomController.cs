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

		NotificationCentre.AddObserver (this, "OnGameOver");
	}


	void Start ()
	{
		PlayerPrefs.SetString (PlayerPrefsKeys.PREV_SCENE, "None"); // Deactivate intro event for debug.
		NotificationCentre.PostNotification (this, "ActivateCannon"); // Debug the cannon.
		NotificationCentre.PostNotification (this, "OnActivateFlammableBlock"); // Debug the block.
		NotificationCentre.PostNotification (this, "OnBlockDestructible"); // Debug the block.
		//PlayerPrefs.SetString (PlayerPrefsKeys.PREV_SCENE, "Title Scene"); // Simulate New Game.
		//PlayerPrefs.SetString (PlayerPrefsKeys.CHECKPOINT, "FirstBattle");
		PlayerPrefs.SetString (PlayerPrefsKeys.CHECKPOINT, "PuzzleSolving");

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


	IEnumerator ResumeFromCheckpoint ()
	{
		NotificationCentre.PostNotification (this, "PauseAllow");
		
		switch (PlayerPrefs.GetString (PlayerPrefsKeys.CHECKPOINT))
		{
		case "FirstBattle" :
			NotificationCentre.PostNotification (this, "OnFadeIn");
			NotificationCentre.PostNotification (this, "OnBattleBGM");
			NotificationCentre.PostNotification (this, "OnEventExit");
			NotificationCentre.PostNotification (this, "OnResumeFromCheckpoint");
			break;
		case "PuzzleSolving" :
			NotificationCentre.PostNotification (this, "OnFadeIn");
			NotificationCentre.PostNotification (this, "OnBGMFadeIn");
			NotificationCentre.PostNotification (this, "OnEventExit");
			NotificationCentre.PostNotification (this, "OnResumeFromCheckpoint");
			yield return new WaitForSeconds (3);
			NotificationCentre.PostNotification (this, "ActivateCannon");
			break;
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

		StartCoroutine (ResumeFromCheckpoint ());
	}
}
