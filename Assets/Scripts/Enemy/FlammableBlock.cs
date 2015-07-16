using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class FlammableBlock : MonoBehaviour
{
	public int maxHealthPoint = 150;
	public GameObject burnEffect;
	public GameObject explosionPrefab;

	private AudioSource burnAudio;
	private ParticleSystem hitParticles;
	private int healthPoint;
	private bool destructible;
	private bool aboutToExplode;


	void Awake ()
	{
		Initialize ();

		NotificationCentre.AddObserver (this, "OnBlockInitialize");
		NotificationCentre.AddObserver (this, "OnBlockDestructible");
		NotificationCentre.AddObserver (this, "OnBlockIndestructible");
	}

	void Initialize ()
	{
		healthPoint = maxHealthPoint;
		burnAudio = GetComponent<AudioSource> ();
		hitParticles = GetComponentInChildren<ParticleSystem> ();
		destructible = false;
		aboutToExplode = false;
		
		if (burnEffect)
			burnEffect.SetActive (false);
	}

	public void TakeHit (int damage, Vector3 hitPoint)
	{
		hitParticles.transform.position = hitPoint;
		hitParticles.Play();

		TakeHit (damage);
	}
	public void TakeHit (int damage)
	{
		burnAudio.PlayOneShot (burnAudio.clip);

		if (destructible)
		{
			healthPoint -= damage;

			if (healthPoint <= 0 && ! aboutToExplode)
			{
				// Explosion
				StartCoroutine (ExplodeInSeconds (1.5f));
				aboutToExplode = true;
			}
			else if (healthPoint <= (maxHealthPoint / 2) && burnEffect)
			{
				burnEffect.SetActive (true);
			}
		}
	}

	void OnBlockDestructible ()
	{
		destructible = true;
	}

	void OnBlockIndestructible ()
	{
		destructible = false;
		Initialize ();
	}

	void OnBlockInitialize ()
	{
		Initialize ();
	}

	IEnumerator ExplodeInSeconds (float seconds)
	{
		yield return new WaitForSeconds (seconds);

		if (explosionPrefab)
		{
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		}
		gameObject.SetActive (false);
	}
}
