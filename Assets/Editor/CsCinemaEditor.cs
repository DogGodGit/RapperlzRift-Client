using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CsCinemaAnimTrigger))]
public class CsCinemaEditor : Editor 
{
	CsCinemaAnimTrigger m_csCinemaAnimTrigger;

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		m_csCinemaAnimTrigger = target as CsCinemaAnimTrigger;
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("Anim");
		string[] astrAnimNames = new string[] { "Idle", "Walk", "Run", "Atack", "Ride", "Interaction" };
		int[] anAnimStatus = new int[] { 0, 1, 2, 3, 20, 11};
		m_csCinemaAnimTrigger.m_nAnimStatus = EditorGUILayout.IntPopup(m_csCinemaAnimTrigger.m_nAnimStatus, astrAnimNames, anAnimStatus);
		
		EditorGUILayout.EndHorizontal();
	}
}
