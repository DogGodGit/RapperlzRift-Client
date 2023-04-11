using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CsHideTriggerMainQuestDungeon : CsBaseArea
{
	List<GameObject> m_listHideObject = new List<GameObject>();

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		//GameObject[] ago = gameObject.GetComponentsInChildren<GameObject>();

		m_listHideObject.Clear();
		m_listHideObject.Add(GameObject.Find("Dun01_Objects/GameObject/Area_Wall/Elkasia_DunIn_01_Ring"));
		m_listHideObject.Add(GameObject.Find("Dun01_Objects/GameObject/Area_Wall/Elkasia_DunIn_01_LHalf"));
		m_listHideObject.Add(GameObject.Find("Dun01_Objects/GameObject/Area_Wall/Elkasia_DunIn_01_Rhalf"));
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		for (int i = 0; i < m_listHideObject.Count; i++)
		{
			m_listHideObject[i].SetActive(false);
		}		
	}

	//---------------------------------------------------------------------------------------------------
	public override void ExitAction()
	{
		for (int i = 0; i < m_listHideObject.Count; i++)
		{
			m_listHideObject[i].SetActive(true);
		}
	}
}
