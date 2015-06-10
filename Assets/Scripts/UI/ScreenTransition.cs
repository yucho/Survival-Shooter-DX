using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//
//  Rect transform of this game object should cover the entire screen.
//
[RequireComponent (typeof (Image))]
public class ScreenTransition : MonoBehaviour
{

	private Image img;
	private IEnumerator lastCoroutine;

	void Awake ()
	{
		img = GetComponent<Image> ();
		lastCoroutine = null;

		// Register on Awake so you won't miss any early notification.
		NotificationCentre.AddObserver (this, "OnNewGame");
		NotificationCentre.AddObserver (this, "OnIntroEvent");
		NotificationCentre.AddObserver (this, "OnFadeIn");
		NotificationCentre.AddObserver (this, "OnFadeOut");
	}


	// "New Game" option selected in Title Scene.
	IEnumerator OnNewGame ()
	{
		yield return new WaitForSeconds (3f);
		StopCoroutine (lastCoroutine);
		lastCoroutine = Fade (Color.clear, Color.white, 4f);
		StartCoroutine (lastCoroutine);
	}


	// Transition into Play Room scene from Title Scene.
	IEnumerator OnIntroEvent ()
	{
		StopCoroutine (lastCoroutine);
		lastCoroutine = Fade (Color.white, Color.black, 0.5f);
		yield return StartCoroutine (lastCoroutine);
		lastCoroutine = Fade (Color.black, Color.clear);
		StartCoroutine (lastCoroutine);
	}


	void OnFadeIn ()
	{
		StopCoroutine (lastCoroutine);
		lastCoroutine = Fade (Color.black, Color.clear);
		StartCoroutine (lastCoroutine);
	}


	void OnFadeOut ()
	{
		StopCoroutine (lastCoroutine);
		lastCoroutine = Fade (Color.clear, Color.black);
		StartCoroutine (lastCoroutine);
	}


	IEnumerator Fade (Color a, Color b, float time = 1)
	{
		// Sanity check: no more than 10 seconds.
		time = Mathf.Min(time, 10);

		bool done = false;

		for (float t = 0; ! done;)
		{
			img.color = Color.Lerp (a, b, t / time);
			yield return null;

			done = t >= time;
			t += Time.deltaTime;
		}
	}


	public new void StopCoroutine (IEnumerator routine)
	{
		if (routine != null)
			base.StopCoroutine (routine);
	}
}
