using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DisplayTextCenter : MonoBehaviour 
{

	public Canvas canvas;
	public Text text;


	private void Awake ()
	{
		NotificationCentre.AddObserver (this, "DisplayText");
		NotificationCentre.AddObserver (this, "HideText");
	}


	// Call this to display text on screen. Specify options with hash values.
	internal void DisplayText (Notification value)
	{
		if (value != null)
		{
			Hashtable data = value.data;

			if (!data.ContainsKey ("TextAnchor"))
			{
				data["TextAnchor"] = "UpperCenter";
			}


			string alignment = (string) data ["TextAnchor"];

			switch (alignment)
			{
			default:
				text.alignment = TextAnchor.UpperCenter;
				break;
			case "UpperCenter":
				text.alignment = TextAnchor.UpperCenter;
				break;
			}


			if (!data.ContainsKey ("Text"))
			{
				data["Text"] = "No Text Input.";
			}

			text.text = (string) data["Text"];
		}

		if (canvas)
			canvas.enabled = true;

		if (text)
			text.enabled = true;
	}


	void HideText ()
	{
		if (canvas)
			canvas.enabled = false;

		if (text)
			text.enabled = false;
	}
}
