using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingUI : MonoBehaviour 
{
	public float motion = 10f;
	public float rotation = 5f;

	private Vector2 initOfsMin, initOfsMax, targetOfsMin, targetOfsMax;
	private Vector3 initRot, targetRot;
	private RectTransform rect;

	void Start ()
	{
		rect = GetComponent<RectTransform> ();
		initOfsMin = rect.offsetMin;
		initOfsMax = rect.offsetMax;
		initRot = transform.rotation.eulerAngles;

		Invoke("SetNewOfs", 0f);
		Invoke("SetNewRot", 0f);
	}

	void Update ()
	{
		rect.offsetMin = Vector2.Lerp (rect.offsetMin, targetOfsMin, 0.5f * Time.deltaTime);
		rect.offsetMax = Vector2.Lerp (rect.offsetMax, targetOfsMax, 0.5f * Time.deltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), 0.3f * Time.deltaTime);
	}

	void SetNewOfs ()
	{
		motion *= -1;

		Vector2 offset = new Vector2 (motion, motion);
		targetOfsMin = initOfsMin + offset;
		targetOfsMax = initOfsMax + offset;

		Invoke("SetNewOfs", Random.Range(2f, 3f));
	}

	void SetNewRot ()
	{
		rotation *= -1;

		targetRot = initRot;
		targetRot.z += rotation;

		Invoke("SetNewRot", Random.Range(3f, 4f));
	}
}
