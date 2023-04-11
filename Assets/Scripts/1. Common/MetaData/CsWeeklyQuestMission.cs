using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyQuestMission
{
	int m_nMissionId;
	int m_nMinHeroLevel;
	int m_nMaxHeroLevel;
	string m_strTitle;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strTargetDescription;
	int m_nType;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	CsMonsterInfo m_csMonsterTarget;
	CsContinentObject m_csContinentObjectTarget;
	int m_nTargetCount;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public int MinHeroLevel
	{
		get { return m_nMinHeroLevel; }
	}

	public int MaxHeroLevel
	{
		get { return m_nMaxHeroLevel; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string TargetDescription
	{
		get { return m_strTargetDescription; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsContinent TargetContinent
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

	public CsMonsterInfo TargetMonster
	{
		get { return m_csMonsterTarget; }
	}

	public CsContinentObject TargetContinentObject
	{
		get { return m_csContinentObjectTarget; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuestMission(WPDWeeklyQuestMission weeklyQuestMission)
	{
		m_nMissionId = weeklyQuestMission.missionId;
		m_nMinHeroLevel = weeklyQuestMission.minHeroLevel;
		m_nMaxHeroLevel = weeklyQuestMission.maxHeroLevel;
		m_strTitle = CsConfiguration.Instance.GetString(weeklyQuestMission.titleKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(weeklyQuestMission.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(weeklyQuestMission.targetContentKey);
		m_strTargetDescription = CsConfiguration.Instance.GetString(weeklyQuestMission.targetDescriptionKey);
		m_nType = weeklyQuestMission.type;
		m_csContinentTarget = CsGameData.Instance.GetContinent(weeklyQuestMission.targetContinentId);
		m_vtTargetPosition = new Vector3(weeklyQuestMission.targetXPosition, weeklyQuestMission.targetYPosition, weeklyQuestMission.targetZPosition);
		m_flTargetRadius = weeklyQuestMission.targetRadius;
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(weeklyQuestMission.targetMonsterId);
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(weeklyQuestMission.targetContinentObjectId);
		m_nTargetCount = weeklyQuestMission.targetCount;
	}
}
