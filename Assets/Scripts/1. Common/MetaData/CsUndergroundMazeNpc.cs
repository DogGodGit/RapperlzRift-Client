using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazeNpc
{
	int m_nNpcId;
	int m_nFloor;
	string m_strName;
	string m_strDialogue;
	Vector3 m_vtPosition;
	float m_flYRotation;
	float m_flInteractionMaxRange;
	string m_strPrefabName;
	float m_flScale;
	int m_nHeight;
	float m_flRadius;

	List<CsUndergroundMazeNpcTransmissionEntry> m_listCsUndergroundMazeNpcTransmissionEntry;

	//---------------------------------------------------------------------------------------------------
	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public int Floor
	{
		get { return m_nFloor; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Dialogue
	{
		get { return m_strDialogue; }
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

	public List<CsUndergroundMazeNpcTransmissionEntry> UndergroundMazeNpcTransmissionEntryList
	{
		get { return m_listCsUndergroundMazeNpcTransmissionEntry; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeNpc(WPDUndergroundMazeNpc undergroundMazeNpc)
	{
		m_nNpcId = undergroundMazeNpc.npcId;
		m_nFloor = undergroundMazeNpc.floor;
		m_strName = CsConfiguration.Instance.GetString(undergroundMazeNpc.nameKey);
		m_strDialogue = CsConfiguration.Instance.GetString(undergroundMazeNpc.dialogueKey);
		m_vtPosition = new Vector3(undergroundMazeNpc.xPosition, undergroundMazeNpc.yPosition, undergroundMazeNpc.zPosition);
		m_flYRotation = undergroundMazeNpc.yRotation;
		m_flInteractionMaxRange = undergroundMazeNpc.interactionMaxRange;
		m_strPrefabName = undergroundMazeNpc.prefabName;
		m_flScale = undergroundMazeNpc.scale;
		m_nHeight = undergroundMazeNpc.height;
		m_flRadius = undergroundMazeNpc.radius;

		m_listCsUndergroundMazeNpcTransmissionEntry = new List<CsUndergroundMazeNpcTransmissionEntry>();
	}
}
