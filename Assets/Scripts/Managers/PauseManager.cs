using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
	public AudioClip pauseEnter;
	public float pauseEnterVol = 0.8f;
	public AudioClip pauseExit;
	public float pauseExitVol = 0.8f;

	private Canvas canvas;
	private AudioSource audioSrc;
	//private AudioListener listener;

	void Awake ()
	{
		canvas = GetComponent <Canvas> ();
		audioSrc = GetComponent <AudioSource> ();
		audioSrc.ignoreListenerVolume = true;
		//listener = FindObjectOfType <AudioListener> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			canvas.enabled = !canvas.enabled;
			Pause ();
			AudioEffect ();
		}
	}

	public void Pause ()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}

	public void Quit ()
	{
#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
	}

	void AudioEffect ()
	{
		if (canvas.enabled)
		{
			audioSrc.clip = pauseEnter;
			audioSrc.volume = pauseEnterVol;
			AudioListener.volume = 0.2f;
		}
		else
		{
			audioSrc.clip = pauseExit;
			audioSrc.volume = pauseExitVol;
			AudioListener.volume = 1f;
		}
		audioSrc.Play ();
	}
}
