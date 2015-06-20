using UnityEngine;
using System.Collections;

public static class CustomUtilities
{
	
	public static IEnumerator MovLocRot (GameObject obj, Vector3 loc, Vector3 angles, float duration)
	{
		Vector3 locA = obj.transform.position;
		Vector3 locB = locA + loc;
		Quaternion rotA = obj.transform.rotation;
		Quaternion rotB = Quaternion.Euler (rotA.eulerAngles + angles);
		/*
		Debug.Log ("rotA.eulerAngles = " + rotA.eulerAngles);
		Debug.Log ("angles = " + angles);

		Vector3 v = rotA.eulerAngles + angles;

		Debug.Log ("sum = " + v);
		Debug.Log ("rotB = " + rotB);
		Debug.Log ("rotB.eulerAngles = " + rotB.eulerAngles);
		*/
		float timer = 0;
		while (true)
		{
			obj.transform.position = Vector3.Lerp (locA, locB, timer / duration);
			obj.transform.rotation = Quaternion.Slerp (rotA, rotB, timer / duration);
			
			if (timer >= duration)
			{
				break;
			}
			
			timer += Time.deltaTime;
			yield return null;
		}
	}


	public static void SetPosRot (GameObject obj, Vector3 pos, Vector3 angles)
	{
		obj.transform.position = pos;
		obj.transform.rotation = Quaternion.Euler (angles);
	}

	public static void SetPosRotLocal (GameObject obj, Vector3 pos, Vector3 angles)
	{
		obj.transform.localPosition = pos;
		obj.transform.localRotation = Quaternion.Euler (angles);
	}
}
