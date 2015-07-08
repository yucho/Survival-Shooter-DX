using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionManager : MonoBehaviour
{

	public Text m_Text;

	private static string key = PlayerPrefsKeys.MISSION;

	
	public static MissionManager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new GameObject("MissionManager").AddComponent<MissionManager> ();
			}
			
			return instance;
		}
	}
	
	private static MissionManager instance;
	
	
	void Awake ()
	{
		if (this.gameObject)
			instance = this;
	}

	void OnApplicationQuit ()
	{
		instance = null;
	}

	public static void UpdateMission (string mission)
	{
		PlayerPrefs.SetString (key, mission);
		UpdateMission ();
	}

	public static void UpdateMission ()
	{
		if (Instance.m_Text)
		{
			Instance.m_Text.text = PlayerPrefs.GetString (key);
		}
	}
}
