using UnityEngine;
using System.Collections;

public class CameraWiggle : MonoBehaviour
{
	public float maxRot = 5f;
	
	void Update ()
	{
		int x = Screen.width; int y = Screen.height;
		Vector2 mouse = Input.mousePosition;
		Vector3 target = new Vector3(-maxRot * 2 * (mouse.y / y), maxRot * 2 * (mouse.x / x));

		/**
		 *  Sanitize target vector
		 */
		target.x = Mathf.Max (target.x, -maxRot * 2) + maxRot;
		target.y = Mathf.Min (target.y, maxRot * 2) - maxRot;

		transform.localRotation = Quaternion.Lerp(transform.localRotation,
		                                          Quaternion.Euler(target), Time.deltaTime);
	}
}
