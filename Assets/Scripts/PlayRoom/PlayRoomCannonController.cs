using UnityEngine;
using System.Collections;

public class PlayRoomCannonController : MonoBehaviour
{
	public GameObject cannonBarrel;
	public GameObject ignition;
	public Transform barrelEnd;
	public float coolDown = 3f;

	private bool cannonActive = false;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "ActivateCannon");
		NotificationCentre.AddObserver (this, "DeactivateCannon");
		NotificationCentre.AddObserver (this, "OnEventEnter");
		NotificationCentre.AddObserver (this, "OnIgniteCannon");

		OnPutOutCannon ();
	}


	void ActivateCannon ()
	{
		CancelInvoke ("Fire");
		InvokeRepeating ("Fire", 0, coolDown);
		cannonActive = true;
		OnIgniteCannon ();
	}

	void DeactivateCannon ()
	{
		CancelInvoke ("Fire");
		cannonActive = false;
		OnPutOutCannon ();
	}

	void OnEventEnter ()
	{
		DeactivateCannon ();
	}

	public bool IsCannonActive ()
	{
		return cannonActive;
	}

	void OnIgniteCannon ()
	{
		if (ignition)
			ignition.SetActive (true);
	}

	void OnPutOutCannon ()
	{
		if (ignition)
			ignition.SetActive (false);
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
