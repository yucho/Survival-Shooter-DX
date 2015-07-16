using UnityEngine;
using System.Collections;

public class PlayerSFXManager : MonoBehaviour
{
	public AudioClip 	hurtClip;
	public float 		hurtVolume 		= 0.8f;
	public AudioClip 	healClip;
	public float 		healVolume	 	= 0.8f;
	public AudioClip 	heartBeatClip;
	public float 		heartBeatVolume = 0.8f;

	private AudioSource audioSource;
	private AudioSource hurtSource;
	private AudioSource heartSource;

	void Awake ()
	{
		Initialize ();
	}

	void Initialize ()
	{
		audioSource = gameObject.AddComponent <AudioSource> ();
		audioSource.loop = false;
		audioSource.playOnAwake = false;

		hurtSource = gameObject.AddComponent <AudioSource> ();
		hurtSource.clip = hurtClip;
		hurtSource.loop = false;
		hurtSource.volume = hurtVolume;
		hurtSource.playOnAwake = false;

		heartSource = gameObject.AddComponent <AudioSource> ();
		heartSource.clip = heartBeatClip;
		heartSource.loop = true;
		heartSource.volume = heartBeatVolume;
		heartSource.pitch = 1.5f;
		heartSource.playOnAwake = false;
	}

	public void PlayerHurtVoice (float delay = 0f)
	{
		hurtSource.PlayDelayed (delay);
	}

	public void PlayerHealSound ()
	{
		audioSource.PlayOneShot (healClip, healVolume);
	}

	public void PlayHeartBeat (bool play)
	{
		if (play && ! heartSource.isPlaying)
		{
			heartSource.Play ();
		}
		else if (! play)
		{
			heartSource.Stop ();
		}
	}
}
