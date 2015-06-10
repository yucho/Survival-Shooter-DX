using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//  Attach this to any game object in the scene to enable pause.
public class PauseManager : MonoBehaviour
{

	//  Canvas to enable on pause.
	public Canvas pressEnterCanvas;

	private bool pauseAllowed;
	private bool paused;
	private bool pressEnterToContinue;

	private Notification continueNotification;


	void Awake ()
	{
		NotificationCentre.AddObserver (this, "PauseAllow");
		NotificationCentre.AddObserver (this, "PauseDisallow");
		NotificationCentre.AddObserver (this, "Pause");
		NotificationCentre.AddObserver (this, "PressEnterToContinue");

		pauseAllowed = false;
		paused = false;
		pressEnterToContinue = false;
	}

	
	void Update ()
	{
		if (pressEnterToContinue && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)))
		{
			PressEnterToContinue (false);
			NotificationCentre.PostNotification (this, "Continue", continueNotification.data);
		}
		else if (pauseAllowed && Input.GetKeyDown(KeyCode.Escape))
		{
			Pause ();
		}
	}

	public void PauseAllow () { pauseAllowed = true; }
	public void PauseDisallow () { pauseAllowed = false; }


	public void Pause ()
	{
		paused = !paused;

		Time.timeScale = paused ? 0 : 1;

		if (paused)
			NotificationCentre.PostNotification (this, "OnPauseEnter");
		else
			NotificationCentre.PostNotification (this, "OnPauseExit");

		//  Dim all sounds. Set AudioSource.ignoreListenerVolume to ignore this.
		DimAudio ();
	}


	// Call this to pause the game and display Press Enter canvas.
	// Make sure to post Hashtable data to distinguish yourself from other callers. 
	internal void PressEnterToContinue (Notification value)
	{
		PressEnterToContinue (true);

		continueNotification = value;
	}


	public void PressEnterToContinue (bool enable)
	{
		pressEnterToContinue = enable;

		//Time.timeScale = enable ? 0 : 1;

		if (pressEnterCanvas)
			pressEnterCanvas.enabled = enable;
	}
	

	void DimAudio ()
	{
		if (Time.timeScale == 0)
			AudioListener.volume = 0.2f;
		else
			AudioListener.volume = 1;
	}
}
