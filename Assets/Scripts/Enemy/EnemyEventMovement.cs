using UnityEngine;
using System.Collections;

// Attach this to EnemyEvent object.
public class EnemyEventMovement : MonoBehaviour
{

	private Animator m_animator;
	private Rigidbody m_rigidbody;
	private NavMeshAgent m_navmeshagent;

	private GameObject m_target;

	private bool m_followTarget = false;
	private bool m_isPaused = false;

	private Vector3 m_oldVelocity = Vector3.zero;
	private Vector3 m_oldAngularVelocity = Vector3.zero;


	public GameObject Target
	{
		get {return m_target; }
		set { m_target = value; }
	}

	public bool followTarget
	{
		get { return m_followTarget; }
		set { m_followTarget = value; }
	}


	void Awake ()
	{
		m_animator = GetComponent<Animator> ();
		m_rigidbody = GetComponent<Rigidbody> ();
		m_navmeshagent = GetComponent<NavMeshAgent> ();

		m_target = gameObject;
	}


	void Update ()
	{
		if (m_followTarget && !m_isPaused)
			Resume ();
		else
			m_navmeshagent.Stop ();

		if (m_animator)
			AnimateWalk ();
	}

	
	public void Pause ()
	{
		m_isPaused = true;

		if (m_animator)
		{
			m_animator.enabled = false;
		}

		if (m_rigidbody)
		{
			m_oldVelocity = m_rigidbody.velocity;
			m_oldAngularVelocity = m_rigidbody.angularVelocity;

			//m_rigidbody.velocity = Vector3.zero;
			//m_rigidbody.angularVelocity = Vector3.zero;
			m_rigidbody.Sleep ();
		}
	}


	public void Resume ()
	{
		if (m_navmeshagent)
		{
			m_navmeshagent.Resume ();
			m_navmeshagent.SetDestination (m_target.transform.position);
		}

		if (m_animator)
		{
			m_animator.enabled = true;
		}

		if (m_rigidbody && m_isPaused)
		{
			m_isPaused = false;

			m_rigidbody.velocity = m_oldVelocity;
			m_rigidbody.angularVelocity = m_oldAngularVelocity;
		}
	}


	void AnimateWalk ()
	{
		if (m_navmeshagent && m_animator)
		{
			float dist = m_navmeshagent.remainingDistance;

			if (m_navmeshagent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
				m_animator.SetBool ("IsWalking", false);
			else
				m_animator.SetBool ("IsWalking", true);
		}
	}
}
