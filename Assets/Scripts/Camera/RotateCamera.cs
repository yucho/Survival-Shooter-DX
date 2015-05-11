using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour
{
	void Update ()
	{
		transform.Rotate(Vector3.up * 5f * Time.deltaTime, Space.World);
	}
}
