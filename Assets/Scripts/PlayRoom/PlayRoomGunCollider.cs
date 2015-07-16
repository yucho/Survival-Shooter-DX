using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]
public class PlayRoomGunCollider : MonoBehaviour
{
	private SphereCollider c;

	private bool eventTriggered;


	void Awake ()
	{
		c = GetComponent<SphereCollider> ();
		eventTriggered = false;

		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnHoldingGunHigh");
		NotificationCentre.AddObserver (this, "Continue");

		// Activate only in new game.
		SetActiveGun (false);

	}


	void OnIntroEvent ()
	{
		SetActiveGun (true);
	}

	void OnEventEnter ()
	{
		c.enabled = false;
	}

	void OnEventExit ()
	{
		c.enabled = true;
	}

	void SetActiveGun (bool active)
	{
		SetActiveChildren (active);
		c.enabled = active;
	}

	void SetActiveChildren (bool active)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive (active);
		}
	}


	void OnTriggerStay (Collider other)
	{
		if (!eventTriggered && other.tag == "PlayerEvent")
		{
			OnTriggerEnter (other);
		}
	}


	void OnTriggerEnter (Collider other)
	{
		if (!eventTriggered && other.tag == "PlayerEvent")
		{
			eventTriggered = true;

			StartCoroutine (OnGunFound ());
		}
	}


	IEnumerator OnGunFound ()
	{
		NotificationCentre.PostNotification (this, "OnEventEnter");
		NotificationCentre.PostNotification (this, "OnFadeOut");
		yield return new WaitForSeconds(1);

		NotificationCentre.PostNotification (this, "OnPickUpGun");
		NotificationCentre.PostNotification (this, "OnFadeIn");
		//yield return new WaitForSeconds(1);
	}


	IEnumerator OnHoldingGunHigh ()
	{
		StartCoroutine (RotateGun ());

		Vector3 locA = transform.position;
		Vector3 locB = locA + new Vector3 (-0.5f, 1.5f, 0);
		
		float timer = 0;
		while (true)
		{
			transform.position = Vector3.Lerp (locA, locB, timer / 0.4f);
			
			if (timer >= 0.4f)
				break;
			
			timer += Time.deltaTime;
			yield return null;
		}


		Hashtable table = new Hashtable ();
		table.Add ("Text", "You  found  a  gun!");
		table.Add ("TextAnchor", "UpperCenter");
		table.Add ("Sender", "PlayRoomGunCollider");

		NotificationCentre.PostNotification (this, "DisplayText", table);
		NotificationCentre.PostNotification (this, "PressEnterToContinue", table);
	}


	IEnumerator RotateGun ()
	{
		while (true)
		{
			//Quaternion rot = transform.rotation * Quaternion.Euler (0, 45, 0);
			//transform.rotation = Quaternion.Lerp (transform.rotation, rot, 2 * Time.deltaTime);
			transform.Rotate (30 * Time.deltaTime, 0, 0);
			yield return null;
		}
	}


	IEnumerator Continue (Notification value)
	{
		Hashtable table = value.data;

		if (table.ContainsKey ("Sender"))
		{
			if ((string) table["Sender"] == "PlayRoomGunCollider")
			{
				NotificationCentre.PostNotification (this, "HideText");
				NotificationCentre.PostNotification (this, "OnFadeOut");
				NotificationCentre.PostNotification (this, "OnBGMFadeOut");

				yield return new WaitForSeconds (1);

				NotificationCentre.PostNotification (this, "OnPlayerActivate");
				NotificationCentre.PostNotification (this, "OnFadeIn");

				StopAllCoroutines ();
				gameObject.SetActive (false);
			}
		}
	}
}
