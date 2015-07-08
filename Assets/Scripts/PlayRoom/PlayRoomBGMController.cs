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

	public AudioClip swift;
	public float swiftVolume;

	public AudioClip vFX1;
	public float vFX1Volume;

	public AudioClip vFX2;
	public float vFX2Volume;

	public AudioClip monsterCry;
	public float monsterCryVolume;

	public AudioClip exclamation;
	public float exclamationVolume;

	public AudioClip question;
	public float questionVolume;

	public AudioClip pauseEnter;
	public float pauseEnterVolume;

	public AudioClip pauseExit;
	public float pauseExitVolume;

	public AudioClip crosshairAppear;
	public float crosshairAppearVolume;

	public AudioClip cannonFire;
	public float cannonFireVolume;


	private AudioSource src;
	private AudioSource one;
	private AudioSource pause;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnBGMFadeIn");
		NotificationCentre.AddObserver (this, "OnBGMFadeOut");
		NotificationCentre.AddObserver (this, "DisplayHowToPlay");
		NotificationCentre.AddObserver (this, "OnHoldingGunHigh");
		NotificationCentre.AddObserver (this, "OnBattleBGM");
		NotificationCentre.AddObserver (this, "OnSFXSwift");
		NotificationCentre.AddObserver (this, "OnSFXVFX1");
		NotificationCentre.AddObserver (this, "OnSFXVFX2");
		NotificationCentre.AddObserver (this, "OnMonsterCry");
		NotificationCentre.AddObserver (this, "OnExclamation");
		NotificationCentre.AddObserver (this, "OnQuestion");
		NotificationCentre.AddObserver (this, "OnPauseEnter");
		NotificationCentre.AddObserver (this, "OnPauseExit");
		NotificationCentre.AddObserver (this, "OnCrosshairAppear");
		NotificationCentre.AddObserver (this, "OnCannonFire");

		src = GetComponent<AudioSource> ();
		one = gameObject.AddComponent<AudioSource> () as AudioSource;
		pause = gameObject.AddComponent<AudioSource> () as AudioSource;

		InitializeAudioSource (one);
		InitializeAudioSource (pause);
		pause.ignoreListenerVolume = true;
	}

	void InitializeAudioSource (AudioSource aSrc, bool playOnAwake = false, bool loop = false, float volume = 1)
	{
		//aSrc = gameObject.AddComponent<AudioSource> () as AudioSource;
		aSrc.playOnAwake = playOnAwake;
		aSrc.loop = loop;
		aSrc.volume = volume;
	}
	

	void OnBGMFadeIn ()
	{
		Play (bGM, bGMVolume, 1);
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

	IEnumerator DisplayHowToPlay ()
	{
		yield return new WaitForSeconds (0.2f);
		PlayOneShot (howToPlay, howToPlayVolume);
	}

	IEnumerator OnHoldingGunHigh ()
	{
		yield return new WaitForSeconds (0.4f);
		PlayOneShot (pickupGun, pickupGunVolume);
	}

	void OnBattleBGM ()
	{
		Play (battleBGM, battleBGMVolume);
	}

	void OnSFXSwift ()
	{
		PlayOneShot (swift, swiftVolume);
	}

	void OnSFXVFX1 ()
	{
		PlayOneShot (vFX1,vFX1Volume);
	}

	void OnSFXVFX2 ()
	{
		PlayOneShot (vFX2,vFX2Volume);
	}

	void OnMonsterCry ()
	{
		PlayOneShot (monsterCry, monsterCryVolume);
	}

	void OnExclamation ()
	{
		PlayOneShot (exclamation, exclamationVolume);
	}

	void OnQuestion ()
	{
		PlayOneShot (question, questionVolume);
	}

	void OnPauseEnter ()
	{
		Play (pause, pauseEnter, pauseEnterVolume, loop : false);
	}

	void OnPauseExit ()
	{
		Play (pause, pauseExit, pauseExitVolume, loop: false);
	}

	void OnCrosshairAppear ()
	{
		PlayOneShot (crosshairAppear, crosshairAppearVolume);
	}

	void OnCannonFire ()
	{
		PlayOneShot (cannonFire, cannonFireVolume);
	}

	void Play (AudioClip clip, float volume = 1, float delay = 0, bool loop = true)
	{
		Play (src, clip, volume, delay, loop);
	}

	void Play (AudioSource source, AudioClip clip, float volume = 1, float delay = 0, bool loop = true)
	{
		if (clip)
		{
			source.clip = clip;
			source.volume = volume;
			source.loop = loop;

			if (delay <= 0)
				source.Play ();
			else
				source.PlayDelayed (delay);
		}
	}

	void PlayOneShot (AudioClip clip, float volume = 1)
	{
		if (clip)
		{
			one.PlayOneShot (clip, volume);
		}
	}
}
