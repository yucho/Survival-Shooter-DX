using UnityEngine;
using System.Collections;

public class PlayRoomFlammableBlockController : MonoBehaviour
{
	public GameObject flammableBlock;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnEnemyRushOver");
		NotificationCentre.AddObserver (this, "OnActivateFlammableBlock");
		NotificationCentre.AddObserver (this, "OnResumeFromCheckpoint");

		if (flammableBlock)
			flammableBlock.SetActive (false);
	}


	void OnIntroEvent ()
	{
		OnActivateFlammableBlock ();
		NotificationCentre.PostNotification (this, "OnBlockIndestructible");
	}


	void OnEnemyRushOver ()
	{
		OnActivateFlammableBlock ();
		NotificationCentre.PostNotification (this, "OnBlockDestructible");
	}


	void OnActivateFlammableBlock ()
	{
		if (flammableBlock)
			flammableBlock.SetActive (true);
	}


	void OnResumeFromCheckpoint ()
	{
		OnActivateFlammableBlock ();
		NotificationCentre.PostNotification (this, "OnBlockInitialize");

		switch (PlayerPrefs.GetString (PlayerPrefsKeys.CHECKPOINT))
		{
		case "PuzzleSolving" :
			NotificationCentre.PostNotification (this, "OnBlockDestructible");
			break;
		case "FirstBattle" :
			NotificationCentre.PostNotification (this, "OnBlockIndestructible");
			break;
		default:
			break;
		}
	}
}
