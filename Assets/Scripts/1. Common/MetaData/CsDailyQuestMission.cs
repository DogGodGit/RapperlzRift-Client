using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsDailyQuestMission
{
	int m_nMissionId;
	string m_strTitle;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strTargetDescription;
	int m_nRequiredHeroLevel;
	CsDailyQuestGrade m_csDailyQuestGrade;
	int m_nType;                // 1:몬스터처치,2:상호작용
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosotion;
	float m_flTargetRadius;
	CsMonsterInfo m_csMonsterTarget;
	CsContinentObject m_csContinentObjectTarget;
	int m_nTargetCount;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
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

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsDailyQuestGrade DailyQuestGrade
	{
		get { return m_csDailyQuestGrade; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetPosotion
	{
		get { return m_vtTargetPosotion; }
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
	public CsDailyQuestMission(WPDDailyQuestMission dailyQuestMission)
	{
		m_nMissionId = dailyQuestMission.missionId;
		m_strTitle = CsConfiguration.Instance.GetString(dailyQuestMission.titleKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(dailyQuestMission.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(dailyQuestMission.targetContentKey);
		m_strTargetDescription = CsConfiguration.Instance.GetString(dailyQuestMission.targetDescriptionKey);
		m_nRequiredHeroLevel = dailyQuestMission.requiredHeroLevel;
		m_csDailyQuestGrade = CsGameData.Instance.GetDailyQuestGrade(dailyQuestMission.grade);
		m_nType = dailyQuestMission.type;
		m_csContinentTarget = CsGameData.Instance.GetContinent(dailyQuestMission.targetContinentId);
		m_vtTargetPosotion = new Vector3(dailyQuestMission.targetXPosotion, dailyQuestMission.targetYPosition, dailyQuestMission.targetZPosition);
		m_flTargetRadius = dailyQuestMission.targetRadius;
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(dailyQuestMission.targetMonsterId);
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(dailyQuestMission.targetContinentObjectId);
		m_nTargetCount = dailyQuestMission.targetCount;
	}
}
