using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsGuildHuntingQuest
{
	int m_nLimitCount;
	int m_nQuestNpcId;
	string m_strCompletionDialogue;
	CsItemReward m_csItemReward;

	List<CsGuildHuntingQuestObjective> m_listCsGuildHuntingQuestObjective;

	//---------------------------------------------------------------------------------------------------
	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public int QuestNpcId
	{
		get { return m_nQuestNpcId; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	public List<CsGuildHuntingQuestObjective> GuildHuntingQuestObjectiveList
	{
		get { return m_listCsGuildHuntingQuestObjective; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildHuntingQuest(WPDGuildHuntingQuest guildHuntingQuest)
	{
		m_nLimitCount = guildHuntingQuest.limitCount;
		m_nQuestNpcId = guildHuntingQuest.questNpcId;
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(guildHuntingQuest.completionDialogueKey);
		m_csItemReward = CsGameData.Instance.GetItemReward(guildHuntingQuest.itemRewardId);

		m_listCsGuildHuntingQuestObjective = new List<CsGuildHuntingQuestObjective>();
	}
}
