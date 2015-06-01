using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// Attach this script to a canvas on which PauseManager enables on pause.
[RequireComponent (typeof(Canvas))]
public class PauseMenu : MonoBehaviour
{

	public Font menuFont;
	public int menuFontSize = 26;

	void Awake ()
	{
		NotificationCentre.AddObserver (this, "OnPauseEnter");
		NotificationCentre.AddObserver (this, "OnPauseExit");
	}

	
	void Start ()
	{
		GenerateUI ();
	}


	void OnPauseEnter ()
	{
	}


	void OnPauseExit ()
	{
	}

	void GenerateUI ()
	{
		GameObject background = GenerateBackground ();

		GameObject layout = GeneratePanel (background.transform);
		VerticalLayoutGroup lGroup = layout.AddComponent<VerticalLayoutGroup> ();
		lGroup.childForceExpandWidth = false;
		lGroup.childForceExpandHeight = false;
		lGroup.spacing = 15;
		lGroup.childAlignment = TextAnchor.LowerLeft;

		/*GameObject menu1 = */GenerateMenu (layout.transform, "Resume", 115, 30);
		/*GameObject menu2 = */GenerateMenu (layout.transform, "Return To Title", 220, 30);
	}




	GameObject GenerateBackground ()
	{ return GenerateBackground (transform); }

	GameObject GenerateBackground (Transform parent)
	{
		GameObject background = GenerateGameObject (parent);

		RectTransform rect = background.AddComponent<RectTransform> ();
		SetRectTransform (rect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Vector2(0.5f,0.5f));

		Image img = background.AddComponent<Image> ();
		img.color = Color.black;

		return background;
	}


	GameObject GeneratePanel ()
	{ return GeneratePanel (transform); }

	GameObject GeneratePanel (Transform parent)
	{ return GeneratePanel(transform, new Vector2(0.2f,0.2f), new Vector2(0.5f,0.8f), Vector2.zero, Vector2.zero); }

	GameObject GeneratePanel (Transform parent, Vector2 aMin, Vector2 aMax, Vector2 oMin, Vector2 oMax)
	{
		GameObject panel = GenerateGameObject (parent);

		RectTransform rect = panel.AddComponent<RectTransform> ();
		SetRectTransform (rect, aMin, aMax, oMin, oMax, new Vector2(0.5f,0.5f));

		return panel;
	}


	GameObject GenerateMenu (string entry, int width, int height)
	{ return GenerateMenu (transform, entry, width, height); }

	GameObject GenerateMenu (Transform parent, string entry, int width, int height)
	{
		//  Button object, a child of layout object. It contains a layout too.
		GameObject menu = GenerateGameObject (parent);
		SetRectTransform (menu.AddComponent<RectTransform> (), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Vector2(0.5f,0.5f));

		Button button = menu.AddComponent<Button> ();
		SetButton (button, 3, new Color(1,1,1,0.25f), Color.white, Color.white, new Color(1,1,1,0.25f));

		Image highlight = menu.AddComponent<Image> ();
		highlight.color = new Color(1, 1, 1, 0.125f);


		//  Text object, a child of button object.
		GameObject menuText = GenerateGameObject (menu.transform);
		SetRectTransform (menuText.AddComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Vector2(0.5f,0.5f));

		Text text = menuText.AddComponent<Text> ();
		text.text = entry;
		text.color = new Color(1,1,1,0.5f);
		if(menuFont)
			text.font = menuFont;
		else
			text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		text.fontSize = menuFontSize;
		text.alignment = TextAnchor.LowerLeft;
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.verticalOverflow = VerticalWrapMode.Overflow;

		HorizontalLayoutGroup layout = menu.AddComponent<HorizontalLayoutGroup> ();
		layout.childForceExpandWidth = false;
		layout.childForceExpandHeight = false;
		layout.childAlignment = TextAnchor.MiddleLeft;
		layout.padding = new RectOffset(10, 0, 0, 0);

		SetLayoutElement(menu.AddComponent<LayoutElement> (), width, height, 0, -1);
		SetLayoutElement(menuText.AddComponent<LayoutElement> (), width, height, 1, -1);

		return menu;
	}





	GameObject GenerateGameObject (Transform parent)
	{
		GameObject obj = new GameObject ();
		Instantiate (obj);
		obj.transform.SetParent (parent);

		return obj;
	}

	void SetRectTransform (RectTransform rect, Vector2 aMin, Vector2 aMax, Vector2 oMin, Vector2 oMax, Vector2 pivot)
	{
		rect.transform.position = Vector3.zero;
		rect.transform.rotation = Quaternion.identity;
		rect.transform.localScale = Vector3.one;

		rect.anchorMin = aMin;
		rect.anchorMax = aMax;
		rect.offsetMin = oMin;
		rect.offsetMax = oMax;
		rect.pivot = pivot;
	}

	void SetButton (Button button, float multiplier, Color normal, Color pressed, Color highlight, Color disabled)
	{
		ColorBlock colors = button.colors;
		colors.colorMultiplier = multiplier;
		colors.normalColor = normal;
		colors.pressedColor = pressed;
		colors.highlightedColor = highlight;
		colors.disabledColor = disabled;
	}

	void SetLayoutElement (LayoutElement element, int pWidth, int pHeight, float fWidth, float fHeight)
	{
		element.preferredWidth = pWidth;
		element.preferredHeight = pHeight;
		element.flexibleWidth = fWidth;
		element.flexibleHeight = fHeight;
	}
}
