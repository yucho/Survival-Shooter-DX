using UnityEngine;
using System.Collections;

//
//  Requires Rigidbody. If you want to animate walk, attach Animator
// and set "IsWalking" bool for walking animation. To make Player rotate
// along mouse pointer, place "Floor" layer underneath this object.
//
[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

	public float speed = 6;


	private Rigidbody rigidBody;
	private Animator animator;
	private int floorMask;
	private float camRayLength = 100f;


	private bool canMove = true;
	private bool facePointer = true;

	//  Set to false to make this script do nothing.
	public bool CanMove (bool canMove)
	{
		return this.canMove = canMove;
	}

	//  Set to false to make player face travel direction.
	public bool FacePointer (bool facePointer)
	{
		return this.facePointer = facePointer;
	}


	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		animator = GetComponent <Animator>();
		rigidBody = GetComponent <Rigidbody>();

		NotificationCentre.AddObserver (this, "OnEventEnter");
		NotificationCentre.AddObserver (this, "OnEventExit");
	}

	void OnEventEnter () { CanMove (false); }

	void OnEventExit () {  CanMove (true);  }

    
	void FixedUpdate ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		if (canMove)
		{
			Move (h, v);
			Turn (h, v);
			AnimateWalk (h, v);
		}
		else
		{
			AnimateWalk (0, 0);
		}
	}


	void Move (float h, float v)
	{
		Vector3 movement = new Vector3 (h, 0f, v) * speed * Time.deltaTime;

		rigidBody.MovePosition (transform.position + movement);
	}


	void Turn (float h, float v)
	{
		if (facePointer)
		{
			FacePointer ();
		}
		else
		{
			FaceForward (h, v);
		}

	}

	void FaceForward (float h, float v)
	{
		bool walking = h != 0f || v != 0f;

		if (walking)
		{
			Quaternion newRotation = Quaternion.LookRotation (new Vector3 (h, 0, v));
			rigidBody.MoveRotation (newRotation);
		}
	}

	void FacePointer ()
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

		if (animator)
			animator.SetBool ("IsWalking", walking);
	}
}
