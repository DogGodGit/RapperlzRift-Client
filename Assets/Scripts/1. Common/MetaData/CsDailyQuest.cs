using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsDailyQuest
{
	string m_strTitle;
	int m_nPlayCount;
	int m_nRequiredHeroLevel;
	int m_nFreeRefreshCount;
	int m_nRefreshRequiredGold;
	int m_nSlotCount;

	List<CsDailyQuestReward> m_listCsDailyQuestReward;
	List<CsDailyQuestMission> m_listCsDailyQuestMission;

	//---------------------------------------------------------------------------------------------------
	public string Title
	{
		get { return m_strTitle; }
	}

	public int PlayCount
	{
		get { return m_nPlayCount; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int FreeRefreshCount
	{
		get { return m_nFreeRefreshCount; }
	}

	public int RefreshRequiredGold
	{
		get { return m_nRefreshRequiredGold; }
	}

	public int SlotCount
	{
		get { return m_nSlotCount; }
	}

	public List<CsDailyQuestReward> DailyQuestRewardList
	{
		get { return m_listCsDailyQuestReward; }
	}

	public List<CsDailyQuestMission> DailyQuestMissionList
	{
		get { return m_listCsDailyQuestMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyQuest(WPDDailyQuest dailyQuest)
	{
		m_strTitle = CsConfiguration.Instance.GetString(dailyQuest.titleKey);
		m_nPlayCount = dailyQuest.playCount;
		m_nRequiredHeroLevel = dailyQuest.requiredHeroLevel;
		m_nFreeRefreshCount = dailyQuest.freeRefreshCount;
		m_nRefreshRequiredGold = dailyQuest.refreshRequiredGold;
		m_nSlotCount = dailyQuest.slotCount;

		m_listCsDailyQuestReward = new List<CsDailyQuestReward>();
		m_listCsDailyQuestMission = new List<CsDailyQuestMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyQuestReward GetDailyQuestReward(int nLevel)
	{
		for (int i = 0; i < m_listCsDailyQuestReward.Count; i++)
		{
			if (m_listCsDailyQuestReward[i].Level == nLevel)
				return m_listCsDailyQuestReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyQuestMission GetDailyQuestMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsDailyQuestMission.Count; i++)
		{
			if (m_listCsDailyQuestMission[i].MissionId == nMissionId)
				return m_listCsDailyQuestMission[i];
		}

		return null;
	}


}
