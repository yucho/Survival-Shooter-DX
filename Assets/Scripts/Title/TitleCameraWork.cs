using UnityEngine;
using System.Collections;

public class TitleCameraWork : MonoBehaviour
{
	
	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnNewGame");
	}


	IEnumerator OnNewGame ()
	{
		yield return StartCoroutine (MovLocRot (new Vector3 (2.5f, 2, 1), new Vector3 (40, 0, 0), 3));
		StartCoroutine (MovLocRot (new Vector3 (0.3f, -1.5f, 1.5f), new Vector3 (50, 40, 0), 4));
	}


	// Pass offset parameters as they get added to current loc & rot.
	IEnumerator MovLocRot (Vector3 loc, Vector3 angles, float duration)
	{
		Vector3 locA = transform.position;
		Vector3 locB = locA + loc;
		Quaternion rotA = transform.rotation;
		Quaternion rotB = rotA * Quaternion.Euler (angles);

		for (float t = 0; t <= duration; t += Time.deltaTime)
		{
			transform.position = Vector3.Lerp (locA, locB, t / duration);
			transform.rotation = Quaternion.Lerp (rotA, rotB, t / duration);

			yield return null;
		}
	}
}
