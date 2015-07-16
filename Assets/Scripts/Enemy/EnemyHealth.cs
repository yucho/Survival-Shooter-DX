using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float shrinkSpeed = 5f;
    public AudioClip deathClip;
	public AudioClip hurtClip;


    private Animator anim;
    private AudioSource enemyAudio;
    private ParticleSystem hitParticles;
    private CapsuleCollider capsuleCollider;
	private SkinnedMeshRenderer meshRenderer;
    private bool isDead;
    private bool isShrinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
		meshRenderer = GetComponentInChildren <SkinnedMeshRenderer> ();

        currentHealth = startingHealth;

		enemyAudio = gameObject.AddComponent<AudioSource> ();
		enemyAudio.loop = false;
		enemyAudio.playOnAwake = false;

		// Listen to annihilation command, which instantly destroys all enemies.
		NotificationCentre.AddObserver (this, "OnDestroyAllEnemies");

		// Initialize AudioQueue
		AudioQueue.AddData (hurtClip, 0.1f);
		AudioQueue.AddData (deathClip, 0.1f);
    }


    void Update ()
    {
        if(isShrinking)
        {
			Shrink ();
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
		{
            return;
		}

		//enemyAudio.clip = hurtClip;
        //enemyAudio.Play ();
		AudioQueue.PlayOneShot (enemyAudio, hurtClip); // call custom method that prevents audio overlap.

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
		else
		{
			BlinkEnemy ();
		}
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Die");

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play ();
		AudioQueue.PlayOneShot (enemyAudio, deathClip); // call custom method that prevents audio overlap.
    }


    public void StartShrinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isShrinking = true;

		NotificationCentre.PostNotification (this, "ReportEnemyDeath");
        Destroy (gameObject, 2f);
    }

	void OnDestroyAllEnemies ()
	{
		NotificationCentre.PostNotification (this, "ReportEnemyDeath");
		Destroy (gameObject);
	}

	void Shrink ()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
	}

	void BlinkEnemy ()
	{
		StopCoroutine ("BlinkRoutine");
		StartCoroutine ("BlinkRoutine");
	}

	IEnumerator BlinkRoutine ()
	{
		for (int i = 0; i < 3; i++)
		{
			meshRenderer.enabled ^= true;
			yield return new WaitForSeconds(0.05f);
		}

		meshRenderer.enabled = true;
	}
}
