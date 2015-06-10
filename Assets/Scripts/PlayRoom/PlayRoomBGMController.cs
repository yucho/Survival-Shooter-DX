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

	public AudioClip pickupGun;
	public float pickupGunVolume;


	private AudioSource src;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnBGMFadeIn");
		NotificationCentre.AddObserver (this, "OnBGMFadeOut");
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "DisplayHowToPlay");
		NotificationCentre.AddObserver (this, "OnHoldingGunHigh");

		src = GetComponent<AudioSource> ();
	}
	

	void OnBGMFadeIn ()
	{
		if (bGM)
		{
			src.volume = bGMVolume;
			src.loop = true;
			src.PlayDelayed (1);
		}
	}

	IEnumerator OnBGMFadeOut ()
	{
		if (bGM)
		{
			float initv = src.volume;
			float timer = 0;
			while (true)
			{
				src.volume = Mathf.Lerp (initv, 0, 1);
				
				if (timer >= 1)
					break;
				
				timer += Time.deltaTime;
				yield return null;
			}
			src.Stop ();
		}
	}

	IEnumerator OnIntroEvent ()
	{
		yield return new WaitForSeconds (0.5f);
		OnBGMFadeIn ();
	}

	IEnumerator DisplayHowToPlay ()
	{
		yield return new WaitForSeconds (0.2f);
		src.PlayOneShot (howToPlay, howToPlayVolume);
	}

	IEnumerator OnHoldingGunHigh ()
	{
		yield return new WaitForSeconds (0.4f);
		src.PlayOneShot (pickupGun, pickupGunVolume);
	}
}
