using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-06)
//---------------------------------------------------------------------------------------------------

public class CsCreatureFarmQuest
{
	string m_strName;
	int m_nRequiredHeroLevel;
	CsNpcInfo m_csNpcStart;
	CsNpcInfo m_csNpcCompletion;
	int m_nLimitCount;
	string m_strStartDialogue;
	string m_strCompletionDialogue;

	List<CsCreatureFarmQuestExpReward> m_listCsCreatureFarmQuestExpReward;
	List<CsCreatureFarmQuestItemReward> m_listCsCreatureFarmQuestItemReward;
	List<CsCreatureFarmQuestMission> m_listCsCreatureFarmQuestMission;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsNpcInfo StartNpc
	{
		get { return m_csNpcStart; }
	}

	public CsNpcInfo CompletionNpc
	{
		get { return m_csNpcCompletion; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public List<CsCreatureFarmQuestExpReward> CreatureFarmQuestExpRewardList
	{
		get { return m_listCsCreatureFarmQuestExpReward; }
	}

	public List<CsCreatureFarmQuestItemReward> CreatureFarmQuestItemRewardList
	{
		get { return m_listCsCreatureFarmQuestItemReward; }
	}

	public List<CsCreatureFarmQuestMission> CreatureFarmQuestMissionList
	{
		get { return m_listCsCreatureFarmQuestMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureFarmQuest(WPDCreatureFarmQuest creatureFarmQuest)
	{
		m_strName = CsConfiguration.Instance.GetString(creatureFarmQuest.nameKey);
		m_nRequiredHeroLevel = creatureFarmQuest.requiredHeroLevel;
		m_csNpcStart = CsGameData.Instance.GetNpcInfo(creatureFarmQuest.startNpcId);
		m_csNpcCompletion = CsGameData.Instance.GetNpcInfo(creatureFarmQuest.completionNpcId);
		m_nLimitCount = creatureFarmQuest.limitCount;
		m_strStartDialogue = CsConfiguration.Instance.GetString(creatureFarmQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(creatureFarmQuest.completionDialogueKey);

		m_listCsCreatureFarmQuestExpReward = new List<CsCreatureFarmQuestExpReward>();
		m_listCsCreatureFarmQuestItemReward = new List<CsCreatureFarmQuestItemReward>();
		m_listCsCreatureFarmQuestMission = new List<CsCreatureFarmQuestMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureFarmQuestExpReward GetCreatureFarmQuestExpReward(int nLevel)
	{
		for (int i = 0; i < m_listCsCreatureFarmQuestExpReward.Count; i++)
		{
			if (m_listCsCreatureFarmQuestExpReward[i].Level == nLevel)
				return m_listCsCreatureFarmQuestExpReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureFarmQuestItemReward GetCreatureFarmQuestItemReward(int nRewardNo)
	{
		for (int i = 0; i < m_listCsCreatureFarmQuestItemReward.Count; i++)
		{
			if (m_listCsCreatureFarmQuestItemReward[i].RewardNo == nRewardNo)
				return m_listCsCreatureFarmQuestItemReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureFarmQuestMission GetCreatureFarmQuestMission(int nMissionNo)
	{
		for (int i = 0; i < m_listCsCreatureFarmQuestMission.Count; i++)
		{
			if (m_listCsCreatureFarmQuestMission[i].MissionNo == nMissionNo)
				return m_listCsCreatureFarmQuestMission[i];
		}

		return null;
	}
}
