using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
	public Splatter splatter;
	public Image flash;
	public Image heartBeatFlash;

	private Color flashColor;
	private bool flashing;
	private bool beating;

	void Awake ()
	{
		flashColor = Color.clear;
		flash.color = flashColor;
		heartBeatFlash.color = flashColor;

		flashing = false;
		beating = false;
	}

	void Update ()
	{
		if (flashing)
		{
			flash.color = flashColor;
			flashing = false;
		}
		else
		{
			FadeAwayFlash ();
		}


		if (beating)
		{
			HeartBeatFlash ();
		}
		else
		{
			FadeAwayHeartBeatFlash ();
		}
	}
	
	public void Flash (Color color)
	{
		flashColor = color;
		flashing = true;
	}

	public void HurtFlash ()
	{
		flashColor = new Color (1, 0, 0, 0.5f);
		flashing = true;
	}

	void FadeAwayFlash ()
	{
		flash.color = Color.Lerp(flash.color, Color.clear, 5f * Time.deltaTime);
	}

	public void DisplaySplatter (Color color, Vector3 position)
	{
		Splatter newSplatter =  Instantiate (splatter) as Splatter;

		newSplatter.transform.SetParent(transform, false);
		newSplatter.Display (color, position);
		newSplatter.FadeAway ();
	}

	public void ToggleHeartBeatFlash (bool toggle)
	{
		beating = toggle;
	}

	void HeartBeatFlash ()
	{
		heartBeatFlash.color = new Color(1f, 0f, 0f, Mathf.PingPong(Time.time, 0.4f) + 0.1f);
	}

	void FadeAwayHeartBeatFlash ()
	{
		heartBeatFlash.color = Color.Lerp(heartBeatFlash.color, Color.clear, 5f * Time.deltaTime);
	}
}
