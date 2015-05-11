using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
	public List<Sprite> sprites;
	public float fadeWait = 1f;
	public float fadeSpeed = 5f;

	private Image img;
	private CanvasScaler canvas;
	private RectTransform rect;
	private bool fadeAway;

	void Awake ()
	{
		Initialize ();
	}

	void Initialize ()
	{
		img = GetComponent<Image> ();
		canvas = GetComponentInParent<CanvasScaler> ();
		rect = GetComponent<RectTransform> ();

		fadeAway = false;
	}

	void Update ()
	{
		if (fadeAway)
		{
			img.color = Color.Lerp(img.color, Color.clear, fadeSpeed * Time.deltaTime);
		}
	}

	public void Display (Color color, Vector3 position)
	{
		Initialize ();

		int count = Random.Range (0, sprites.Count);
		img.sprite = sprites[count];
		img.color = color;

		ConvertToRect (position);
		RandomizeRotScale ();

		this.gameObject.SetActive (true);
	}

	void ConvertToRect (Vector3 position)
	{
		/**
		 *  Sanitize input.
		 */
		Vector3 input = position.normalized;

		/**
		 *  Calculate where on screen to display.
		 */
		Vector2 resolution = canvas.referenceResolution / 2;
		Vector3 displayPosition = new Vector3 (input.x * resolution.x, input.z * resolution.y, 0f);

		rect.localPosition = displayPosition;
	}

	void RandomizeRotScale ()
	{
		rect.localScale *= Random.Range (1f, 1.5f);
		rect.Rotate (new Vector3 (0f, 0f, Random.Range (0f, 360f)));
	}
	
	public void FadeAway ()
	{
		StartCoroutine (WaitForFade ());
	}

	IEnumerator WaitForFade ()
	{
		yield return new WaitForSeconds (fadeWait);

		fadeAway = true;

		yield return new WaitForSeconds (Mathf.Min(1 / fadeSpeed + 1, 10f));

		Destroy (this.gameObject);
	}
}
