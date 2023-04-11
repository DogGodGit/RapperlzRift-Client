using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarNpc
{
	int m_nNpcId;
	string m_strName;
	string m_strNick;
	string m_strDialogue;
	CsContinent m_csContinent;
	Vector3 m_vtPosition;
	float m_flYRotation;
	float m_flInteractionMaxRange;
	string m_strPrefabName;
	float m_flScale;
	int m_nHeight;
	float m_flRadius;

	List<CsNationWarTransmissionExit> m_listCsNationWarTransmissionExit;

	//---------------------------------------------------------------------------------------------------
	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Nick
	{
		get { return m_strNick; }
	}

	public string Dialogue
	{
		get { return m_strDialogue; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
	}

	public Vector3 Position
	{
		get { return m_vtPosition; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public float InteractionMaxRange
	{
		get { return m_flInteractionMaxRange; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public float Scale
	{
		get { return m_flScale; }
	}

	public int Height
	{
		get { return m_nHeight; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public List<CsNationWarTransmissionExit> NationWarTransmissionExitList
	{
		get { return m_listCsNationWarTransmissionExit; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarNpc(WPDNationWarNpc nationWarNpc)
	{
		m_nNpcId = nationWarNpc.npcId;
		m_strName = CsConfiguration.Instance.GetString(nationWarNpc.nameKey);
		m_strNick = CsConfiguration.Instance.GetString(nationWarNpc.nickKey);
		m_strDialogue = CsConfiguration.Instance.GetString(nationWarNpc.dialogueKey);
		m_csContinent = CsGameData.Instance.GetContinent(nationWarNpc.continentId);
		m_vtPosition = new Vector3(nationWarNpc.xPosition, nationWarNpc.yPosition, nationWarNpc.zPosition);
		m_flYRotation= nationWarNpc.yRotation;
		m_flInteractionMaxRange = nationWarNpc.interactionMaxRange;
		m_strPrefabName = nationWarNpc.prefabName;
		m_flScale = nationWarNpc.scale;
		m_nHeight = nationWarNpc.height;
		m_flRadius = nationWarNpc.radius;

		m_listCsNationWarTransmissionExit = new List<CsNationWarTransmissionExit>();
	}
}
