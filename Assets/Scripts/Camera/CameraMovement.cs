using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(Camera))]
public class CameraMovement : MonoBehaviour
{

	public Transform target;
	public float smoothness;

	private bool follow;
	private Vector3 offset = Vector3.zero;
	

	void Start ()
	{
		if (target)
			offset = transform.position - target.position;

		follow = true;
	}


	void FixedUpdate ()
	{
		if (follow && target)
		{
			Vector3 targetCamPos = target.position + offset;

			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothness);
		}
	}

	public void SetCameraFollow (bool follow)
	{
		this.follow = follow;
	}

	public void SetCameraTarget (Transform newTarget)
	{
		target = newTarget;
	}

	public void SetCameraOffset (Vector3 offset)
	{
		this.offset = offset;
	}

	public void CameraLookAt (Vector3 position)
	{
		transform.LookAt (position);
	}

	public void CameraLookAt (Transform position)
	{
		transform.LookAt (position.position);
	}

	public void SetCameraAngle (Quaternion angle)
	{
		transform.rotation = angle;
	}
}
