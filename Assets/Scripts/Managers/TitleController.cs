using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleController : MonoBehaviour
{
	public GameObject cam;
	public MovingUI loadIcon;
	public MovingUI fadeImage;
	public MovingUI UI;

	private AsyncOperation async;
	private bool quitting = false;

	void Awake ()
	{
		/**
		 *  Force developers to fix bugs.
		 */
		ExceptionHandler.SetupExceptionHandling();
	}

	IEnumerator Start ()
	{
		FadeIn ();
		async = Application.LoadLevelAsync("Play Room");
		async.allowSceneActivation = false;

		yield return async;
	}

	void Update ()
	{
		if (async.progress > 0.89f)
		{
			loadIcon.FadeAndDestroy ();
		}
	}

	void FadeIn ()
	{
		//fadeImage.FadeAndDestroy ();
		GameObject.FindWithTag("BGM").GetComponent<AudioSource> ().PlayDelayed (1f);
	}

	public void ReleaseLoad ()
	{
		StartCoroutine (ZoomInPlayer ());
		//UI.SwayAndDestroy ();
		//async.allowSceneActivation = true;
	}

	IEnumerator ZoomInPlayer ()
	{
		Vector3 target = cam.transform.position + new Vector3 (2.5f, 2, 1);
		Quaternion rot = cam.transform.rotation * Quaternion.Euler (40, 0, 0);

		for (float t = 0f; t < 3f; t += Time.deltaTime)
		{
			cam.transform.position = Vector3.Lerp (cam.transform.position, target, Time.deltaTime); //x+2, y+1, z+1
			cam.transform.rotation = Quaternion.Lerp (cam.transform.rotation, rot, Time.deltaTime);	
			yield return t;
		}

		cam.GetComponentInChildren<MotionBlur> ().enabled = true;
		Image img = fadeImage.GetComponent<Image> ();
		img.enabled = true;
		img.color = new Color (1, 1, 1, 0);
		AudioSource bgm = GameObject.FindWithTag("BGM").GetComponent<AudioSource> ();
		AudioReverbFilter rev = cam.GetComponentInChildren<AudioReverbFilter> ();
		rev.enabled = true;
		target = target + new Vector3 (0.3f, -2, 1.5f);
		rot = rot * Quaternion.Euler (20, 30, 0);

		for (float t = 0f; t < 6f; t += Time.deltaTime)
		{
			cam.transform.position = Vector3.Lerp (cam.transform.position, target, 0.2f * Time.deltaTime);
			cam.transform.rotation = Quaternion.Lerp (cam.transform.rotation, rot, Time.deltaTime);
			img.color = Color.Lerp (img.color, Color.white, Time.deltaTime);
			rev.decayTime = Mathf.MoveTowards (rev.decayTime, 20f, 2f * Time.deltaTime);

			if (t > 4f)
				bgm.Stop ();

			yield return t;
		}

		PlayerPrefs.SetString("Previous Scene", "Title Scene");
		async.allowSceneActivation = true;
	}

	public void Quit (float time = 0f)
	{
		if (! quitting)
		{
			quitting = true;
			StartCoroutine (QuitApplication (time));
		}
	}

	IEnumerator QuitApplication (float time)
	{
		yield return new WaitForSeconds (time);

#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
	}
}
