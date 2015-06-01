using UnityEngine;
using System.Collections;

//
//  Attach this script to an empty object in Play Room scene. It listens to
// the PlayRoomController for event notifications and manipulates player object.
//
public class PlayRoomPlayerController : MonoBehaviour
{

	private GameObject player;
	//private MeshRenderer [] meshes;
	//private SkinnedMeshRenderer [] skins;


	void Awake ()
	{
		//NotificationCentre.AddObserver (this, "OnEventEnter");
		//NotificationCentre.AddObserver (this, "OnEventExit");
		NotificationCentre.AddObserver (this, "OnIntroEvent");

		GetPlayer ();
	}


	void GetPlayer ()
	{
		player = GameObject.FindWithTag ("Player");
		
		if (player)
		{
			//meshes = player.GetComponentsInChildren<MeshRenderer> ();
			//skins = player.GetComponentsInChildren<SkinnedMeshRenderer> ();
		}
	}


	void OnIntroEvent ()
	{
		if (player)
			player.SetActive (false);
	}
}
