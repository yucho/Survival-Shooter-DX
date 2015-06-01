using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

//  Attach this to menu canvas.
// Behavior: When menu canvas is enabled the first time, default selectable will be selected.
//          For second time, it resumes from the last closed state except if it was confirmation
//          window, then it will select previously selected selectable.

public delegate void EnableInteractableHandler (MenuEventArgs e);
public delegate void DisableInteractableHandler (MenuEventArgs e);
public delegate void DisableAllInteractableExceptHandler (MenuEventArgs e);


public class MenuManager : MonoBehaviour
{
	public event EnableInteractableHandler Enable;
	public event DisableInteractableHandler Disable;
	public event DisableAllInteractableExceptHandler DisableAllExcept;
	
	public List<MenuGroup> menuGroup;

	private MenuEventArgs me;
	private MenuButton caller;
	private string message;


	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnPauseEnter");
		NotificationCentre.AddObserver (this, "OnPauseExit");
	}


	void Start ()
	{
	}


	public void ExecuteMessage (MenuButton caller, string msg = "", bool confirm = false)
	{
		if (confirm)
		{
			this.caller = caller;
			this.message = msg;
			DisableAllElseButEnableThis ("Confirmation");
			EnableCanvas ("Confirmation");
			FindMenuButtonOnTopLeft ().Select ();
		}
		else
		{
			Invoke (msg, 0);
		}
	}


	private void DisableAllElseButEnableThis (string enableGroup)
	{
		DisableAllExcept (new MenuEventArgs (enableGroup));
		Enable (new MenuEventArgs (enableGroup));
	}


	private void EnableCanvas (string groupName)
	{
		ToggleCanvas (groupName, true);
	}

	private void DisableCanvas (string groupName)
	{
		ToggleCanvas (groupName, false);
	}

	private void ToggleCanvas (string groupName, bool enable)
	{
		MenuGroup menu = menuGroup.Find(x => x.GroupName == groupName);
		
		if (menu != null)
		{
			menu.MenuCanvas.enabled = enable;
		}
	}

	private void DisableAllCanvas ()
	{
		foreach (MenuGroup menu in menuGroup)
		{
			if (menu != null)
			{
				menu.MenuCanvas.enabled = false;
			}
		}
	}


	private void SelectCaller ()
	{
		DisableAllElseButEnableThis (caller.menuGroup);
		DisableCanvas ("Confirmation");

		caller.Select ();
	}

	
	void OnPauseEnter ()
	{
		Enable (new MenuEventArgs ("Main"));
		EnableCanvas ("Main");

		FindMenuButtonOnTopLeft ().Select ();
	}

	// Make sure to get out of confirmation window.
	void OnPauseExit ()
	{
		Disable (MenuEventArgs.All);
		DisableAllCanvas ();
	}


	private MenuButton FindMenuButtonOnTopLeft ()
	{
		List<Selectable> s_list = Selectable.allSelectables;

		float maxScore = Mathf.Infinity;
		MenuButton bestPick = null;

		for (int i = 0; i < s_list.Count; ++i)
		{
			Selectable sel = s_list[i];
			
			if (sel == null || sel.GetType () != typeof (MenuButton))
				continue;
			
			if (!sel.IsInteractable() || sel.navigation.mode == Navigation.Mode.None)
				continue;
			
			var selRect = sel.transform as RectTransform;
			Vector3 selCenter = selRect != null ? (Vector3)selRect.rect.center : Vector3.zero;
			Vector3 myVector = sel.transform.TransformPoint(selCenter) - new Vector3(0, Screen.height);

			float score = myVector.magnitude;

			if (score < maxScore)
			{
				maxScore = score;
				bestPick = (MenuButton) sel;
			}
		}
		return bestPick;
	}
	

	//
	// Following are executable messages:
	//
	
	public void Yes ()
	{
		SelectCaller ();
		Invoke (message, 0);
	}
	
	
	public void No ()
	{
		SelectCaller ();
	}


	public void Resume ()
	{
		NotificationCentre.PostNotification (this, "Pause");
	}

	public void ReturnToTitle ()
	{
		// Temporary.
		//FindObjectOfType<PauseManager> ().Pause();
		Resume ();

		// No more pause because it will screw up the title.
		//NotificationCentre.PostNotification (this, "PauseDisallow");

		Application.LoadLevel ("Title Scene");
	}


	[Serializable]
	public class MenuGroup
	{
		public Canvas MenuCanvas = null;
		public string GroupName  = "";
	}
}


public class MenuEventArgs : EventArgs
{
	public static readonly MenuEventArgs All = new MenuEventArgs ("All");
	public MenuEventArgs(string MenuGroup) { this.MenuGroup = MenuGroup; }
	public string MenuGroup { get; set; }
}
