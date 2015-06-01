using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;

// Must be attached to child object of MenuManager.
[Serializable]
public class MenuButton : Button
{
	public string menuGroup;
	public string message = "";
	public bool confirm = false;

	private MenuManager mm;

	new void Awake ()
	{
		//mm = GetComponentInParent<MenuManager> ();
		mm = FindObjectOfType<MenuManager> ();

		if (mm)
		{
			mm.Enable += OnEnableInteractable;
			mm.Disable += OnDisableInteractable;
			mm.DisableAllExcept += OnDisableAllExcept;
		}
	}

	public void OnClick ()
	{
		OnSubmit ();
	}

	public void OnSubmit ()
	{
		mm.ExecuteMessage(this, message, confirm);
	}

	void OnEnableInteractable (MenuEventArgs e)
	{
		if (e.MenuGroup == menuGroup)
			this.interactable = true;
	}

	void OnDisableInteractable (MenuEventArgs e)
	{
		if (e.MenuGroup == menuGroup || e.MenuGroup == "All")
			this.interactable = false;
	}

	void OnDisableAllExcept (MenuEventArgs e)
	{
		if (e.MenuGroup != menuGroup)
			this.interactable = false;
	}	
}
