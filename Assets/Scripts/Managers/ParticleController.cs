using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleController : MonoBehaviour
{
	// Prefabs
	public GameObject question;
	public GameObject exclamation;

	public static ParticleController Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new GameObject("ParticleController").AddComponent<ParticleController> ();
			}

			return instance;
		}
	}

	private static ParticleController instance;


	void Awake ()
	{
		if (this.gameObject)
			instance = this;
	}

	public static void Exclamation (Vector3 position, float y = 0)
	{
		Instance.StartCoroutine (PlaySoundDelayed ("OnExclamation", 0.1f));
		Instance.StartCoroutine (PlayParticle (Instance.exclamation, position, y));
	}

	public static void Question (Vector3 position, float y = 0)
	{
		NotificationCentre.PostNotification (Instance, "OnQuestion");
		Instance.StartCoroutine (PlayParticle (Instance.question, position, y));
	}

	private static IEnumerator PlayParticle (GameObject obj, Vector3 position, float y, float life = 2)
	{
		position.y += y;

		if (obj)
		{
			GameObject ins = Instantiate (obj, position, Quaternion.identity) as GameObject;

			ParticleSystem ps = obj.GetComponent<ParticleSystem> ();
			if (ps)
			{
				ps.Play ();
			}

			yield return new WaitForSeconds (life);

			Destroy (ins);
		}
	}

	private static IEnumerator PlaySoundDelayed (string sound, float delay)
	{
		yield return new WaitForSeconds (delay);
		NotificationCentre.PostNotification (Instance, sound);
	}
}
