using UnityEngine;
using System.Collections;

public class MovementTrackPlayer : MonoBehaviour
{
	private Transform player;
    private NavMeshAgent nav;
	private bool withinReach;
	private Animator animator;

    void Awake ()
    {
		/**
		 *  Won't throw error even if it doesn't exist.
		 */
        player = GameObject.FindWithTag ("Player").transform;
        
		/**
		 *  Will throw error if don't exist.
		 */
        nav = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
    }

    void Update ()
    {
		/**
		 *  Follows the player until it reaches melee range.
		 */
		if (player)
		{
			if (! withinReach)
			{
	            nav.SetDestination (player.position);
				animator.SetBool ("IsWalking", true);
			}
			else
			{
				animator.SetBool ("IsWalking", false);
			}
		}
    }

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			withinReach = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			withinReach = false;
		}
	}
}
