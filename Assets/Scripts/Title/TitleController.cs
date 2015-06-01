using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.ImageEffects;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

//  Attach BGM clip to the audio source.
[RequireComponent (typeof(AudioSource))]
public class TitleController : MonoBehaviour
{

	private AudioSource asrc;
	private AsyncOperation async;

	void Awake ()
	{
		//
		//  Force developers to fix bugs.
		//
		ExceptionHandler.SetupExceptionHandling();

		asrc = GetComponent<AudioSource> ();
	}


	IEnumerator Start ()
	{
		// Play BGM.
		asrc.PlayDelayed (1);

		// Load the Play Room scene asyncronously.
		NotificationCentre.PostNotification (this, "OnLoading");
		async = Application.LoadLevelAsync("Play Room");
		async.allowSceneActivation = false;

		//
		//  Weird LoadLevelAsync behavior: progress will stop at 0.9
		// until allowSceneActivation is set to true, which in turn
		// activates the new scene and loads the remaining 0.1.
		//
		while (async.progress < 0.89f) { yield return null; }

		NotificationCentre.PostNotification (this, "OnFinishLoading");
	}
	

	// Send "NewGame" Message to TitleController to call this.
	IEnumerator NewGame ()
	{
		NotificationCentre.PostNotification (this, "OnNewGame");

		yield return new WaitForSeconds (7);

		asrc.Stop ();

		yield return new WaitForSeconds (2);

		PlayerPrefs.SetString("Previous Scene", "Title Scene");
		async.allowSceneActivation = true;
	}


	// Send "Quit" Message to TC to quit application.
	IEnumerator Quit ()
	{
		NotificationCentre.PostNotification (this, "OnQuit");

		yield return new WaitForSeconds (1);

		// Fade out screen.
		NotificationCentre.PostNotification (this, "OnFadeOut");

		// Lower BGM volume.
		StartCoroutine (FadeBGM ());

		yield return new WaitForSeconds (1.1f);

#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
	}


	IEnumerator FadeBGM ()
	{
		float a = asrc.volume;
		bool done = false;
		
		for (float t = 0; ! done;)
		{
			asrc.volume = Mathf.Lerp (a, 0, t);
			yield return null;
			
			done = t >= 1;
			t += Time.deltaTime;
		}
	}
}
