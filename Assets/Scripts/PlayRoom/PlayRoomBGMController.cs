using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class PlayRoomBGMController : MonoBehaviour
{

	public AudioClip bGM;
	public float bGMVolume;

	public AudioClip battleBGM;
	public float battleBGMVolume;

	public AudioClip howToPlay;
	public float howToPlayVolume;


	private AudioSource src;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnFadeIn");
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "DisplayHowToPlay");

		src = GetComponent<AudioSource> ();
	}
	

	void OnFadeIn ()
	{
		if (bGM)
		{
			src.volume = bGMVolume;
			src.loop = true;
			src.PlayDelayed (1);
		}
	}

	IEnumerator OnIntroEvent ()
	{
		yield return new WaitForSeconds (0.5f);
		OnFadeIn ();
	}

	IEnumerator DisplayHowToPlay ()
	{
		yield return new WaitForSeconds (0.2f);
		src.PlayOneShot (howToPlay, howToPlayVolume);
	}
}
