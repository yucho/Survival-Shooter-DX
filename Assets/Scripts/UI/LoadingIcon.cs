using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
public class LoadingIcon : MonoBehaviour
{

	public float rotation = 180;

	private Image icon;

	void Awake ()
	{
		icon = GetComponent<Image> ();

		NotificationCentre.AddObserver (this, "OnLoading");
		NotificationCentre.AddObserver (this, "OnFinishLoading");
	}
	

	void Update ()
	{
		// Rotate the icon image.
		transform.Rotate (Vector3.forward, -rotation * Time.deltaTime);
	}


	void OnLoading ()
	{
		icon.enabled = true;
	}


	IEnumerator OnFinishLoading ()
	{
		yield return StartCoroutine (Fade (Color.white, Color.clear));
		icon.enabled = false;
	}


	IEnumerator Fade (Color a, Color b, float time = 0.3f)
	{
		bool done = false;
		
		for (float t = 0; ! done;)
		{
			icon.color = Color.Lerp (a, b, t / time);
			yield return null;
			
			done = t >= time;
			t += Time.deltaTime;
		}
	}
}
