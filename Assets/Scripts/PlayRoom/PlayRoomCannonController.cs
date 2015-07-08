using UnityEngine;
using System.Collections;

public class PlayRoomCannonController : MonoBehaviour
{
	public GameObject cannonBarrel;
	public Transform barrelEnd;
	public float coolDown = 3f;

	private bool cannonActive = false;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "ActivateCannon");
		NotificationCentre.AddObserver (this, "DeactivateCannon");
	}


	void ActivateCannon ()
	{
		InvokeRepeating ("Fire", 0, coolDown);
		cannonActive = true;
	}

	void DeactivateCannon ()
	{
		CancelInvoke ("Fire");
		cannonActive = false;
	}

	public bool IsCannonActive ()
	{
		return cannonActive;
	}


	void Fire ()
	{
		// fire repeatedly.
		GameObject player = GameObject.FindWithTag ("Player");
		GameObject cannon = GameObject.FindWithTag ("Cannon");

		if (player && cannon)
		{
			BallisticProjectile cannonball = cannon.GetComponent<BallisticProjectile> ();
			cannonball.Fire(barrelEnd.position, player.transform.position, 2f);
			NotificationCentre.PostNotification (this, "OnCannonFire");
		}
	}
}
