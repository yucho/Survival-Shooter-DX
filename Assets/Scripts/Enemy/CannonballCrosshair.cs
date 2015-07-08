using UnityEngine;
using System.Collections;

public class CannonballCrosshair : MonoBehaviour
{
	public float life = 0.5f;

	private float timer;

	void Start ()
	{
		timer = 0;
		NotificationCentre.PostNotification (this, "OnCrosshairAppear");
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (timer >= life)
		{
			Destroy (this.gameObject);
		}
	}
}
