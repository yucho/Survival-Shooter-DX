using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour
{
	public float loop = 2f;
	public float range = 0.5f;

	private ParticleSystem ps;

	void Start ()
	{
		ps = GetComponent<ParticleSystem> ();

		Invoke("Play", Random.Range(loop - range, loop + range));
	}

	void Play ()
	{
		ps.Play ();
		Invoke("Play", Random.Range(loop - range, loop + range));
	}

}
