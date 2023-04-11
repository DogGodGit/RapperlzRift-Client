using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-06)
//---------------------------------------------------------------------------------------------------

public class CsCreatureFarmQuestMission
{
	int m_nMissionNo;
	int m_nTargetType;
	/*
		* 목표타입
		1 : 이동

		2 : 상호작용
		- 목표대륙오브젝트ID

		3 : 몬스터 처치
		- 몬스터배치
		- 목표자동완료시간 
	*/
	string m_strTargetTitle;
	string m_strTargetContent;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	CsContinentObject m_csContinentObjectTarget;
	int m_nTargetAutoCompletionTime;
	int m_nTargetCount;

	//---------------------------------------------------------------------------------------------------
	public int MissionNo
	{
		get { return m_nMissionNo; }
	}
	
	public int TargetType
	{
		get { return m_nTargetType; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public CsContinent ContinentTarget
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetPosition
	{
		get { return m_vtTargetPosition; }
	}
	
	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public CsContinentObject ContinentObjectTarget
	{
		get { return m_csContinentObjectTarget; }
	}

	public int TargetAutoCompletionTime
	{
		get { return m_nTargetAutoCompletionTime; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureFarmQuestMission(WPDCreatureFarmQuestMission creatureFarmQuestMission)
	{
		m_nMissionNo = creatureFarmQuestMission.missionNo;
		m_nTargetType = creatureFarmQuestMission.targetType;
		m_strTargetTitle = CsConfiguration.Instance.GetString(creatureFarmQuestMission.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(creatureFarmQuestMission.targetContentKey);
		m_csContinentTarget = CsGameData.Instance.GetContinent(creatureFarmQuestMission.targetContinentId);
		m_vtTargetPosition = new Vector3(creatureFarmQuestMission.targetXPosition, creatureFarmQuestMission.targetYPosition, creatureFarmQuestMission.targetZPosition);
		m_flTargetRadius = creatureFarmQuestMission.targetRadius;
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(creatureFarmQuestMission.targetContinentObjectId);
		m_nTargetAutoCompletionTime = creatureFarmQuestMission.targetAutoCompletionTime;
		m_nTargetCount = creatureFarmQuestMission.targetCount;
	}
}
