using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class TitleCanvasGroup : MonoBehaviour
{

	private CanvasGroup cg;
	
	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnNewGame");
		NotificationCentre.AddObserver (this, "OnQuit");

		cg = GetComponent<CanvasGroup> ();
	}
	
	void OnNewGame ()
	{
		StartCoroutine (FadeAway ());
	}

	void OnQuit ()
	{
		StartCoroutine (FadeAway ());
	}


	IEnumerator FadeAway ()
	{
		bool done = false;
		
		for (float t = 0; ! done;)
		{
			cg.alpha = Mathf.Lerp (1, 0, t);
			yield return null;
			
			done = t >= 1;
			t += Time.deltaTime;
		}
	}
}
