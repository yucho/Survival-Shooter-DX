using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed;

	private bool canMove = true;
	private Vector3 movement;
	private Rigidbody rigidBody;
	private Animator animator;
	private int floorMask;
	private float camRayLength = 100f;

	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		animator = GetComponent <Animator>();
		rigidBody = GetComponent <Rigidbody>();
	}
    
	void FixedUpdate ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if (canMove){
			Move (h, v);
			Turn ();
			AnimateWalk (h, v);
		}
		else
		{
			AnimateWalk (0, 0);
		}
	}

	void Move (float h, float v)
	{
		movement = new Vector3 (h, 0f, v) * speed * Time.deltaTime;

		rigidBody.MovePosition (transform.position + movement);
	}

	void Turn ()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			rigidBody.MoveRotation (newRotation);
		}
	}

	void AnimateWalk (float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		animator.SetBool ("IsWalking", walking);
	}

	public void CanMove (bool canMove)
	{
		this.canMove = canMove;
	}
}
