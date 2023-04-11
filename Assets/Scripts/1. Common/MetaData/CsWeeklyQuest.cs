using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyQuest
{
	string m_strTitle;
	int m_nRoundCount;
	int m_nRequiredHeroLevel;
	int m_nRoundRefreshRequiredGold;
	CsItem m_csItemRoundImmediateCompletionRequired;
	int m_nTenRoundCompletionRequiredVipLevel;
	float m_flTenRoundCompletionRewardFactor;

	List<CsWeeklyQuestRoundReward> m_listCsWeeklyQuestRoundReward;
	List<CsWeeklyQuestMission> m_listCsWeeklyQuestMission;
	List<CsWeeklyQuestTenRoundReward> m_listCsWeeklyQuestTenRoundReward;

	//---------------------------------------------------------------------------------------------------
	public string Title
	{
		get { return m_strTitle; }
	}

	public int RoundCount
	{
		get { return m_nRoundCount; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RoundRefreshRequiredGold
	{
		get { return m_nRoundRefreshRequiredGold; }
	}

	public CsItem RoundImmediateCompletionRequiredItem
	{
		get { return m_csItemRoundImmediateCompletionRequired; }
	}

	public int TenRoundCompletionRequiredVipLevel
	{
		get { return m_nTenRoundCompletionRequiredVipLevel; }
	}

	public float TenRoundCompletionRewardFactor
	{
		get { return m_flTenRoundCompletionRewardFactor; }
	}

	public List<CsWeeklyQuestRoundReward> WeeklyQuestRoundRewardList
	{
		get { return m_listCsWeeklyQuestRoundReward; }
	}

	public List<CsWeeklyQuestMission> WeeklyQuestMissionList
	{
		get { return m_listCsWeeklyQuestMission; }
	}

	public List<CsWeeklyQuestTenRoundReward> WeeklyQuestTenRoundRewardList
	{
		get { return m_listCsWeeklyQuestTenRoundReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuest(WPDWeeklyQuest weeklyQuest)
	{
		m_strTitle = CsConfiguration.Instance.GetString(weeklyQuest.titleKey);
		m_nRoundCount = weeklyQuest.roundCount;
		m_nRequiredHeroLevel = weeklyQuest.requiredHeroLevel;
		m_nRoundRefreshRequiredGold = weeklyQuest.roundRefreshRequiredGold;
		m_csItemRoundImmediateCompletionRequired = CsGameData.Instance.GetItem(weeklyQuest.roundImmediateCompletionRequiredItemId);
		m_nTenRoundCompletionRequiredVipLevel = weeklyQuest.tenRoundCompletionRequiredVipLevel;
		m_flTenRoundCompletionRewardFactor = weeklyQuest.tenRoundCompletionRewardFactor;

		m_listCsWeeklyQuestRoundReward = new List<CsWeeklyQuestRoundReward>();
		m_listCsWeeklyQuestMission = new List<CsWeeklyQuestMission>();
		m_listCsWeeklyQuestTenRoundReward = new List<CsWeeklyQuestTenRoundReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuestRoundReward GetWeeklyQuestRoundReward(int nRoundNo, int nLevel)
	{
		for (int i = 0; i < m_listCsWeeklyQuestRoundReward.Count; i++)
		{
			if (m_listCsWeeklyQuestRoundReward[i].RoundNo == nRoundNo && m_listCsWeeklyQuestRoundReward[i].Level == nLevel)
				return m_listCsWeeklyQuestRoundReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuestMission GetWeeklyQuestMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsWeeklyQuestMission.Count; i++)
		{
			if (m_listCsWeeklyQuestMission[i].MissionId == nMissionId)
				return m_listCsWeeklyQuestMission[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuestTenRoundReward GetWeeklyQuestTenRoundReward(int nRewardNo)
	{
		for (int i = 0; i < m_listCsWeeklyQuestTenRoundReward.Count; i++)
		{
			if (m_listCsWeeklyQuestTenRoundReward[i].RewardNo == nRewardNo)
				return m_listCsWeeklyQuestTenRoundReward[i];
		}

		return null;
	}
}
