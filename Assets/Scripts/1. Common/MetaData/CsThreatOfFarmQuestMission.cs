using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-01)
//---------------------------------------------------------------------------------------------------

public class CsThreatOfFarmQuestMission
{
	int m_nMissionId;
	string m_strTargetPositionName;
	CsContinent m_csContinentTarget;
	float m_flTargetXPosition;
	float m_flTargetYPosition;
	float m_flTargetZPosition;
	float m_flTargetRadius;
	float m_flMonsterSpawnXPosition; // for server
	float m_flMonsterSpawnYPosition; // for server
	float m_flMonsterSpawnZPosition; // for server
	int m_nMonsterYRotationType; // for server
	float m_flMonsterYRotation; // for server

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public string TargetPositionName
	{
		get { return m_strTargetPositionName; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

//	public float TargetXPosition
//	{
//		get { return m_flTargetXPosition; }
//	}
//
//	public float TargetYPosition
//	{
//		get { return m_flTargetYPosition; }
//	}
//
//	public float TargetZPosition
//	{
//		get { return m_flTargetZPosition; }
//	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

//	public float MonsterSpawnXPosition
//	{
//		get { return m_flMonsterSpawnXPosition; }
//	}
//
//	public float MonsterSpawnYPosition
//	{
//		get { return m_flMonsterSpawnYPosition; }
//	}
//
//	public float MonsterSpawnZPosition
//	{
//		get { return m_flMonsterSpawnZPosition; }
//	}
//
	public int MonsterYRotationType
	{
		get { return m_nMonsterYRotationType; }
	}

	public float MonsterYRotation
	{
		get { return m_flMonsterYRotation; }
	}

	public Vector3 TargetPosition
	{
		get { return new Vector3(m_flTargetXPosition, m_flTargetYPosition, m_flTargetZPosition); }
	}

	public Vector3 MonsterSpawnPosition
	{
		get { return new Vector3(m_flMonsterSpawnXPosition, m_flMonsterSpawnYPosition, m_flMonsterSpawnZPosition); }
	}

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuestMission(WPDTreatOfFarmQuestMission threatOfFarmQuestMission)
	{
		m_nMissionId = threatOfFarmQuestMission.missionId;
		m_strTargetPositionName = CsConfiguration.Instance.GetString(threatOfFarmQuestMission.targetPositionNameKey);
		m_csContinentTarget = CsGameData.Instance.GetContinent(threatOfFarmQuestMission.targetContinentId);
		m_flTargetXPosition = threatOfFarmQuestMission.targetXPosition;
		m_flTargetYPosition = threatOfFarmQuestMission.targetYPosition;
		m_flTargetZPosition = threatOfFarmQuestMission.targetZPosition;
		m_flTargetRadius = threatOfFarmQuestMission.targetRadius;
		m_flMonsterSpawnXPosition = threatOfFarmQuestMission.monsterSpawnXPosition;
		m_flMonsterSpawnYPosition = threatOfFarmQuestMission.monsterSpawnYPosition;
		m_flMonsterSpawnZPosition = threatOfFarmQuestMission.monsterSpawnZPosition;
		m_nMonsterYRotationType = threatOfFarmQuestMission.monsterYRotationType;
		m_flMonsterYRotation = threatOfFarmQuestMission.monsterYRotation;
	}
}
