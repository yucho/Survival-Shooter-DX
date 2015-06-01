using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

//
// Attach this to the main camera on Title Scene.
//
[RequireComponent (typeof(Camera))]
public class TitleCameraEffect : MonoBehaviour
{

	public float maxRot = 5f;

	// Wiggle camera along mouse pointer.
	void Update ()
	{
		int x = Screen.width; int y = Screen.height;
		Vector2 mouse = Input.mousePosition;
		Vector3 target = new Vector3(-maxRot * 2 * (mouse.y / y), maxRot * 2 * (mouse.x / x));

		//
		//  Sanitize target vector
		//
		target.x = Mathf.Max (target.x, -maxRot * 2) + maxRot;
		target.y = Mathf.Min (target.y, maxRot * 2) - maxRot;

		transform.localRotation = Quaternion.Lerp(transform.localRotation,
		                                          Quaternion.Euler(target), Time.deltaTime);
	}


	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnNewGame");
	}


	IEnumerator OnNewGame ()
	{
		yield return new WaitForSeconds (3);

		AudioReverbFilter arf = GetComponent<AudioReverbFilter> ();

		if (arf)
			arf.enabled = true;

		MotionBlur mb = GetComponent<MotionBlur> ();

		if (mb)
			mb.enabled = true;

		for (float t = 0; t <= 4; t += Time.deltaTime)
		{
			// Gradually strengthen reverb filter.
			if (arf)
				arf.decayTime = Mathf.Lerp (0, 20, t / 4);

			yield return null;
		}
	}
}
