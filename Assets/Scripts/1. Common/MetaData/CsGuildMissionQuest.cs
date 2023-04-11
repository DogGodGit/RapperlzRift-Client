using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-27)
//---------------------------------------------------------------------------------------------------

public class CsGuildMissionQuest
{
	string m_strName;
	int m_nLimitCount;
	CsNpcInfo m_csNpcStart;
	CsItemReward m_csItemRewardCompletion;

	List<CsGuildMissionQuestReward> m_listCsGuildMissionQuestReward;
	List<CsGuildMission> m_listCsGuildMission;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}
	
	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public CsNpcInfo StartNpc
	{
		get { return m_csNpcStart; }
	}

	public CsItemReward ItemRewardCompletion
	{
		get { return m_csItemRewardCompletion; }
	}

	public List<CsGuildMissionQuestReward> GuildMissionQuestRewardList
	{
		get { return m_listCsGuildMissionQuestReward; }
	}

	public List<CsGuildMission> GuildMissionList
	{
		get { return m_listCsGuildMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMissionQuest(WPDGuildMissionQuest guildMissionQuest)
	{
		m_strName = CsConfiguration.Instance.GetString(guildMissionQuest.nameKey);
		m_nLimitCount = guildMissionQuest.limitCount;
		m_csNpcStart = CsGameData.Instance.GetNpcInfo(guildMissionQuest.startNpcId);
		m_csItemRewardCompletion = CsGameData.Instance.GetItemReward(guildMissionQuest.completionItemRewardId);

		m_listCsGuildMissionQuestReward = new List<CsGuildMissionQuestReward>();
		m_listCsGuildMission = new List<CsGuildMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMissionQuestReward GetGuildMissionQuestReward(int nLevel)
	{
		for (int i = 0; i < m_listCsGuildMissionQuestReward.Count; i++)
		{
			if (m_listCsGuildMissionQuestReward[i].Level == nLevel)
				return m_listCsGuildMissionQuestReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMission GetGuildMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsGuildMission.Count; i++)
		{
			if (m_listCsGuildMission[i].MissionId == nMissionId)
				return m_listCsGuildMission[i];
		}

		return null;
	}
}
