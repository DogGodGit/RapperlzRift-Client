using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------
/*
1 : 일반
2 : 대륙전송
3 : 국가전송
4 : 지하미로NPC
*/
public enum EnNpcType
{
	Normal = 1,
	ContinentTransmission = 2,
	NationTransmission = 3,
	UndergroundMaze = 4
}

public class CsNpcInfo
{
	int m_nNpcId;                           // NPCID
	string m_strName;                       // 이름
	string m_strNick;                       // 닉네임
	string m_strDialogue;                   // 대화
	string m_strImageName;                  // 이미지이름
	int m_nType;							// NPC타입
	CsContinent m_csContinent;              // 대륙ID
	Vector3 m_vtPosition;					// 좌표
	float m_flYRotation;                    // 방향
	float m_flInteractionMaxRange;          // 상호작용최대범위	
	string m_strPrefabName;                 // 프리팹이름
	string m_strSoundName;                  // 음성이름
	float m_flScale;                        // 크기
	int m_nHeight;                          // 높이
	float m_flRadius;                       // 반지름

	List<CsContinentTransmissionExit> m_listCsContinentTransmissionExit;

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

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int ContinentId
	{
		get { return m_csContinent.ContinentId; }
	}

	public EnNpcType NpcType
	{
		get { return (EnNpcType)m_nType; }
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

	public string SoundName
	{
		get { return m_strSoundName; }
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

	public List<CsContinentTransmissionExit> ContinentTransmissionExitList
	{
		get { return m_listCsContinentTransmissionExit; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcInfo(WPDNpc npc)
	{
		m_nNpcId = npc.npcId;
		m_strName = CsConfiguration.Instance.GetString(npc.nameKey);
		m_strNick = CsConfiguration.Instance.GetString(npc.nickKey);
		m_strDialogue = CsConfiguration.Instance.GetString(npc.dialogueKey);
		m_strImageName = npc.imageName;
		m_nType = npc.type;
		m_csContinent = CsGameData.Instance.GetContinent(npc.continentId);
		m_vtPosition = new Vector3((float)npc.xPosition, (float)npc.yPosition, (float)npc.zPosition);
		m_flYRotation = npc.yRotation;
		m_flInteractionMaxRange = npc.interactionMaxRange;
		m_strPrefabName = npc.prefabName;
		m_strSoundName = npc.soundName;
		m_flScale = npc.scale;
		m_nHeight = npc.height;
		m_flRadius = npc.radius;

		m_listCsContinentTransmissionExit = new List<CsContinentTransmissionExit>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinentTransmissionExit GetContinentTransmissionExit(int nExitNo)
	{
		for (int i = 0; i < m_listCsContinentTransmissionExit.Count; i++)
		{
			if (m_listCsContinentTransmissionExit[i].ExitNo == nExitNo)
				return m_listCsContinentTransmissionExit[i];
		}

		return null;
	}
}
