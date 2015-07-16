using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(Camera))]
public class CameraMovement : MonoBehaviour
{

	public GameObject target;
	public Vector3 offset;
	public float smoothness;
	public bool follow;


	void FixedUpdate ()
	{
		if (follow && target)
		{
			Vector3 targetCamPos = target.transform.position + offset;

			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothness * Time.deltaTime);

			//Debug.Log ("Pos:" + transform.position + ", TargetPos:" + targetCamPos + ", Offset:" + offset);
		}
	}

	public void SetCameraFollow (bool follow)
	{
		this.follow = follow;
	}

	public void SetCameraTarget (GameObject newTarget)
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
