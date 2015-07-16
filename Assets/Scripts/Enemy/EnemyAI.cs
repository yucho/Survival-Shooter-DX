using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float 		coolDown 	= 0.5f;
	public AudioClip	attackClip;
	public float		attackVolume = 0.8f;

	private Transform 		player;
	private Rigidbody		rigidBody;
	private NavMeshAgent 	nav;
	private Animator 		animator;
	private EnemyHitBox		hitBox;
	private EnemyHealth		myHealth;
	private AudioSource		audioSrc;

	private float			timer;
	private bool 			targetInRange;
	private bool			attackEnd;
	private bool 			isDead;
	private bool			isGameOver;


	void Awake ()
	{
		/**
		 *  Won't throw error even if it doesn't exist.
		 */
		player = GameObject.FindWithTag ("Player").transform;
		
		/**
		 *  Will throw error if they don't exist.
		 */
		nav = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		hitBox = GetComponentInChildren<EnemyHitBox> ();
		myHealth = GetComponent<EnemyHealth> ();

		timer = 0f;
		attackEnd = false;
		isDead = false;
		isGameOver = false;

		audioSrc = gameObject.AddComponent<AudioSource> ();
		audioSrc.playOnAwake = false;
		audioSrc.loop = false;

		// Stay idle when player die.
		NotificationCentre.AddObserver (this, "OnPlayerDeath");

		// Reassign player object when new one is created.
		NotificationCentre.AddObserver (this, "OnUpdatePlayer");
	}
	
	void Update ()
	{
		timer += Time.deltaTime;

		/**
		 *  Follows the player until it reaches melee range, then attack.
		 */
		if (player)
		{
			if (isDead)
			{
				return;
			}
			else if (myHealth.currentHealth <= 0)
			{
				/**
				 *  Stop the navigation and toggle isDead.
				 */
				nav.Stop ();
				isDead = true;
				return;
			}
			else if (attackEnd)
			{
				animator.SetBool ("IsAttacking", false);
				timer = 0f;
				attackEnd = false;
				return;
			}
			else if (IsAttacking ())
			{
				return;
			}

			if (! targetInRange && ! isGameOver)
			{
				nav.Resume ();
				nav.SetDestination (player.position);
				animator.SetBool ("IsWalking", true);
			}
			else if (targetInRange && ! isGameOver)
			{
				nav.Stop ();
				animator.SetBool ("IsWalking", false);

				if (timer >= coolDown)
				{
					Attack ();
				}

				FacePlayer ();
			}
			else
			{
				nav.Stop();
				animator.SetBool ("IsWalking", false);
			}
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			targetInRange = true;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			targetInRange = false;
		}
	}

	void FacePlayer()
	{
		Quaternion rotNew = Quaternion.LookRotation(player.position - transform.position);
		rigidBody.MoveRotation(rotNew);
	}

	void Attack ()
	{
		if (player == null)
		{
			return;
		}

		animator.SetBool ("IsAttacking", true);
		hitBox.ToggleHitBox (true);
	}

	void AttackEnd ()
	{
		attackEnd = true;
		timer = 0f;
	}

	bool IsAttacking ()
	{
		return animator.GetBool("IsAttacking");
	}

	void PlayAttackSound ()
	{
		audioSrc.clip = attackClip;
		audioSrc.volume = attackVolume;
		audioSrc.Play ();
	}

	void OnPlayerDeath ()
	{
		isGameOver = true;
	}

	void OnUpdatePlayer ()
	{
		player = GameObject.FindWithTag ("Player").transform;
	}
}
