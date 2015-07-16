using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//  This prevents the same audio clips to play at the same frame.
// User can add certain clips and assign minimum cooldown time.
public class AudioQueue : MonoBehaviour
{
	private static AudioQueue instance;

	public static AudioQueue Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("AudioQueue").AddComponent<AudioQueue> ();
			}
			
			return instance;
		}
	}
	
	public void OnApplicationQuit ()
	{
		instance = null;
	}
	
	private Dictionary<AudioClip, AudioLog> m_qdata  = new Dictionary<AudioClip, AudioLog> ();



	// Add audio clip for which you want to set cool down time.
	public static void AddData (AudioClip clip, float cooldown)
	{
		if (Instance.m_qdata.ContainsKey (clip))
			Instance.m_qdata[clip].cooldown = cooldown;
		else
			Instance.m_qdata.Add (clip, new AudioLog(cooldown));
	}


	public static void RemoveData (AudioClip clip)
	{
		Instance.m_qdata.Remove (clip);
	}


	// PlayOneShot that won't play the same clips within cooldown timeframe.
	public static void PlayOneShot (AudioSource src, AudioClip clip, float volume=1)
	{
		float elapsedTime = 0;

		if (Instance.m_qdata.ContainsKey (clip))
		{
			elapsedTime = Time.time - Instance.m_qdata[clip].timeLastPlayed;

			if (elapsedTime < Instance.m_qdata[clip].cooldown)
			{
				Instance.m_qdata[clip].qsrc.Enqueue (src);
				Instance.m_qdata[clip].qvolume.Enqueue (volume);
				return;
			}
		}
		src.PlayOneShot (clip, volume);
		Instance.m_qdata[clip].timeLastPlayed = Time.time;
	}


	// If any clip is in queue and cooled down, play it.
	void Update ()
	{
		foreach (KeyValuePair<AudioClip, AudioLog> pair in Instance.m_qdata)
		{
			if (pair.Value.qsrc.Count > 0)
			{
				float elapsedTime = Time.time - pair.Value.timeLastPlayed;
				if (elapsedTime >= pair.Value.cooldown)
				{
					AudioSource src = pair.Value.qsrc.Dequeue ();
					float volume = pair.Value.qvolume.Dequeue ();

					src.PlayOneShot (pair.Key, volume);
					pair.Value.timeLastPlayed = Time.time;
				}
			}
		}
	}
}

public class AudioLog
{
	public Queue<AudioSource> qsrc = new Queue<AudioSource> ();
	public Queue<float> qvolume = new Queue<float> ();
	public float timeLastPlayed = 0;
	public float cooldown;

	public AudioLog (float cooldown)
	{
		this.cooldown = cooldown;
	}
}
