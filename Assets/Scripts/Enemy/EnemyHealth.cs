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

		enemyAudio.clip = hurtClip;
        enemyAudio.Play ();

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

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartShrinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isShrinking = true;

        Destroy (gameObject, 2f);
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
