using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsBlessingTargetLevel
{
	int m_nTargetLevelId;
	int m_nTargetHeroLevel;
	int m_nProspectQuestObjectiveLevel;
	int m_nProspectQuestObjectiveLimitTime;

	List<CsProspectQuestOwnerReward> m_listCsProspectQuestOwnerReward;
	List<CsProspectQuestTargetReward> m_listCsProspectQuestTargetReward;

	//---------------------------------------------------------------------------------------------------
	public int TargetLevelId
	{
		get { return m_nTargetLevelId; }
	}

	public int TargetHeroLevel
	{
		get { return m_nTargetHeroLevel; }
	}

	public int ProspectQuestObjectiveLevel
	{
		get { return m_nProspectQuestObjectiveLevel; }
	}

	public int ProspectQuestObjectiveLimitTime
	{
		get { return m_nProspectQuestObjectiveLimitTime; }
	}

	public List<CsProspectQuestOwnerReward> ProspectQuestOwnerRewardList
	{
		get { return m_listCsProspectQuestOwnerReward; }
	}

	public List<CsProspectQuestTargetReward> ProspectQuestTargetRewardList
	{
		get { return m_listCsProspectQuestTargetReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBlessingTargetLevel(WPDBlessingTargetLevel blessingTargetLevel)
	{
		m_nTargetLevelId = blessingTargetLevel.targetLevelId;
		m_nTargetHeroLevel = blessingTargetLevel.targetHeroLevel;
		m_nProspectQuestObjectiveLevel = blessingTargetLevel.prospectQuestObjectiveLevel;
		m_nProspectQuestObjectiveLimitTime = blessingTargetLevel.prospectQuestObjectiveLimitTime;

		m_listCsProspectQuestOwnerReward = new List<CsProspectQuestOwnerReward>();
		m_listCsProspectQuestTargetReward = new List<CsProspectQuestTargetReward>();
	}
}
