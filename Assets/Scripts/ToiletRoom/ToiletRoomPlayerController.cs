using UnityEngine;
using System.Collections;

public class ToiletRoomPlayerController : MonoBehaviour
{
	private GameObject player;
	private PlayerShooting gun;
	
	
	void Awake ()
	{		
		GetPlayer ();
	}
	
	
	void GetPlayer ()
	{
		player = GameObject.FindWithTag ("Player");
		
		if (player)
		{
			gun = player.GetComponentInChildren<PlayerShooting> ();
		}
	}
}
