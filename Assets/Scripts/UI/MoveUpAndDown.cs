using UnityEngine;
using System.Collections;

public class MoveUpAndDown : MonoBehaviour
{

	IEnumerator Start ()
	{
		Vector3 newPos;// = transform.localPosition;
		int flip = 10;

		while (true)
		{
			flip = -flip;
			newPos = ((RectTransform) transform).localPosition;
			newPos.y -= flip;
			((RectTransform) transform).localPosition = newPos;

			yield return StartCoroutine (CoroutineUtilities.WaitForRealTime (0.25f));
		}
	}
}
