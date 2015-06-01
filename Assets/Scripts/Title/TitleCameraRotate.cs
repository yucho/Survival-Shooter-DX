using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
public class TitleCameraRotate : MonoBehaviour
{
	void Update ()
	{
		transform.Rotate(Vector3.up * 5f * Time.deltaTime, Space.World);
	}
}
