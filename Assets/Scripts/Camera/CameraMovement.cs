using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	public Transform target;
	public float smoothness;

	private bool follow;
	private Vector3 offset;

	void Start () {
		offset = transform.position - target.position;
		follow = true;
	}

	void FixedUpdate ()
	{
		if (follow)
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
}
