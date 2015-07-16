using UnityEngine;
using System.Collections;

public class ToiletRoomCameraController : MonoBehaviour
{
	
	private GameObject player;
	
	private CameraMovement cam;
	
	void Awake ()
	{
		player = GameObject.FindWithTag ("Player");
		cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement> ();

		CustomUtilities.SetPosRot(cam.gameObject, new Vector3 (0, 6, -47), new Vector3(20,0,0));

		CameraDefault ();
	}

	void CameraDefault ()
	{
		if (player && cam)
		{
			Debug.Log ("called");
			cam.gameObject.SetActive (true);
			cam.SetCameraOffset (new Vector3 (0, 3, -6));
			cam.SetCameraTarget (player);
			cam.SetCameraFollow (true);
		}
	}

}

/*   Memo

Good default cemra offset in Toilet Room scene is pos(0, 3, -6) and rot(20, 0, 0).

*/
