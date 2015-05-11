using UnityEngine;
using System.Collections;

public class MovingUI : MonoBehaviour
{
	public float rotate = 0f;
	public CanvasGroup canvas;

	private bool fading = false;
	private float fadeSpeed = 2f;

	void Update ()
	{
		if (rotate > 0f)
		{
			transform.Rotate(Vector3.Lerp (Vector3.zero, Vector3.forward * -rotate, Time.deltaTime));
		}

		if (fading)
		{
			canvas.alpha = Mathf.MoveTowards(canvas.alpha, 0f, fadeSpeed * Time.deltaTime);
		}
	}

	public void FadeAndDestroy (float time = 2f)
	{
		if (fading)
			return;

		fadeSpeed = 2f / time;
		fading = true;

		Invoke ("SelfDestruction", time);
	}


	void SelfDestruction ()
	{
		Destroy (gameObject);
	}

}
