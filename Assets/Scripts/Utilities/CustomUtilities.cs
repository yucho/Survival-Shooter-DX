using UnityEngine;
using System.Collections;

public static class CustomUtilities
{
	
	public static IEnumerator MovLocRot (GameObject obj, Vector3 loc, Vector3 angles, float duration)
	{
		Vector3 locA = obj.transform.position;
		Vector3 locB = locA + loc;
		Quaternion rotA = obj.transform.rotation;
		Quaternion rotB = rotA * Quaternion.Euler (angles);
		
		float timer = 0;
		while (true)
		{
			obj.transform.position = Vector3.Lerp (locA, locB, timer / duration);
			obj.transform.rotation = Quaternion.Lerp (rotA, rotB, timer / duration);
			
			if (timer >= duration)
				break;
			
			timer += Time.deltaTime;
			yield return null;
		}
	}
}
