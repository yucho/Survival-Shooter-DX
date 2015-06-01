using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class HowToPlay : MonoBehaviour
{

	private CanvasGroup cg;
	private IEnumerator coroutine;

	void Awake ()
	{
		cg = GetComponent<CanvasGroup> ();
		cg.alpha = 0;

		coroutine = null;

		NotificationCentre.AddObserver (this, "DisplayHowToPlay");
		NotificationCentre.AddObserver (this, "HideHowToPlay");
		NotificationCentre.AddObserver (this, "Continue");
	}


	IEnumerator DisplayHowToPlay ()
	{
		if (coroutine != null)
			StopCoroutine (coroutine);

		coroutine = AlphaLerp (0, 1, 0.5f);
		yield return StartCoroutine (coroutine);

		NotificationCentre.PostNotification (this, "PressEnterToContinue");
	}


	void Continue ()
	{
		StartCoroutine (HideHowToPlay ());
	}

	IEnumerator HideHowToPlay ()
	{
		if (coroutine != null)
			StopCoroutine (coroutine);

		yield return 1;
		coroutine = AlphaLerp (1, 0, 0.5f);
		yield return StartCoroutine (coroutine);
	}


	IEnumerator AlphaLerp (float a, float b, float t)
	{
		for (float timer = 0; true; timer += Time.deltaTime)
		{
			cg.alpha = Mathf.Lerp (a, b, timer / t);
			
			if (timer >= t)
				break;
			
			yield return null;
		}
	}
}
