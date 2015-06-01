using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using System.Collections;

[CustomEditor (typeof(MenuButton))]
public class MenuButtonEditor : ButtonEditor
{
	SerializedProperty m_MenuGroupProperty;
	SerializedProperty m_MessageProperty;
	SerializedProperty m_ConfirmProperty;

	protected override void OnEnable ()
	{
		base.OnEnable ();

		m_MenuGroupProperty = serializedObject.FindProperty("menuGroup");
		m_MessageProperty = serializedObject.FindProperty("message");
		m_ConfirmProperty = serializedObject.FindProperty("confirm");
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.Separator ();

		serializedObject.Update ();
		EditorGUILayout.PropertyField (m_MenuGroupProperty);
		EditorGUILayout.PropertyField (m_MessageProperty);
		EditorGUILayout.PropertyField (m_ConfirmProperty);
		serializedObject.ApplyModifiedProperties ();
	}
}
