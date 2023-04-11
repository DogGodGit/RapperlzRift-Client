using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildFarmQuest
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strTargetCompletion;
	int m_nStartTime;
	int m_nEndTime;
	int m_nLimitCount;
	CsGuildTerritoryNpc m_csGuildTerritoryNpcQuest;
	CsGuildTerritoryNpc m_csGuildTerritoryNpcTarget;
	int m_nInteractionDuration;
	string m_strInteractionText;
	CsItemReward m_csItemRewardCompletion;
	CsGuildContributionPointReward m_csGuildContributionPointRewardCompletion;
	CsGuildBuildingPointReward m_csGuildBuildingPointRewardCompletion;
	string m_strQuestStartDialogue;
	string m_strQuestCompletionDialogue;

	List<CsGuildFarmQuestReward> m_listCsGuildFarmQuestReward;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int StartTime
	{
		get { return m_nStartTime; }
	}

	public int EndTime
	{
		get { return m_nEndTime; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public CsGuildTerritoryNpc QuestGuildTerritoryNpc
	{
		get { return m_csGuildTerritoryNpcQuest; }
	}

	public CsGuildTerritoryNpc TargetGuildTerritoryNpc
	{
		get { return m_csGuildTerritoryNpcTarget; }
	}

	public int InteractionDuration
	{
		get { return m_nInteractionDuration; }
	}

	public string InteractionText
	{
		get { return m_strInteractionText; }
	}

	public CsItemReward CompletionItemReward
	{
		get { return m_csItemRewardCompletion; }
	}

	public string QuestStartDialogue
	{
		get { return m_strQuestStartDialogue; }
	}

	public string QuestCompletionDialogue
	{
		get { return m_strQuestCompletionDialogue; }
	}

	public List<CsGuildFarmQuestReward> GuildFarmQuestRewardList
	{
		get { return m_listCsGuildFarmQuestReward; }
	}

    public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

    public string TargetContent
	{
		get { return m_strTargetContent; }
	}

    public string TargetCompletion
	{
		get { return m_strTargetCompletion; }
	}

    public CsGuildContributionPointReward CompletionGuildContributionPointReward
	{
		get { return m_csGuildContributionPointRewardCompletion; }
	}

    public CsGuildBuildingPointReward CompletionGuildBuildingPointReward
	{
		get { return m_csGuildBuildingPointRewardCompletion; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildFarmQuest(WPDGuildFarmQuest guildFarmQuest)
	{
		m_strName = CsConfiguration.Instance.GetString(guildFarmQuest.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(guildFarmQuest.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(guildFarmQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(guildFarmQuest.targetContentKey);
		m_strTargetCompletion = CsConfiguration.Instance.GetString(guildFarmQuest.targetCompletionKey);
		m_nStartTime = guildFarmQuest.startTime;
		m_nEndTime = guildFarmQuest.endTime;
		m_nLimitCount = guildFarmQuest.limitCount;
		m_csGuildTerritoryNpcQuest = CsGameData.Instance.GuildTerritory.GetGuildTerritoryNpc(guildFarmQuest.questGuildTerritoryNpcId);
		m_csGuildTerritoryNpcTarget = CsGameData.Instance.GuildTerritory.GetGuildTerritoryNpc(guildFarmQuest.targetGuildTerritoryNpcId);
		m_nInteractionDuration = guildFarmQuest.interactionDuration;
		m_strInteractionText = CsConfiguration.Instance.GetString(guildFarmQuest.interactionTextKey);
		m_csItemRewardCompletion = CsGameData.Instance.GetItemReward(guildFarmQuest.completionItemRewardId);
		m_csGuildContributionPointRewardCompletion = CsGameData.Instance.GetGuildContributionPointReward(guildFarmQuest.completionGuildContributionPointRewardId);
		m_csGuildBuildingPointRewardCompletion = CsGameData.Instance.GetGuildBuildingPointReward(guildFarmQuest.completionGuildBuildingPointRewardId);
		m_strQuestStartDialogue = CsConfiguration.Instance.GetString(guildFarmQuest.questStartDialogueKey);
		m_strQuestCompletionDialogue = CsConfiguration.Instance.GetString(guildFarmQuest.questCompletionDialogueKey);

		m_listCsGuildFarmQuestReward = new List<CsGuildFarmQuestReward>();
	}
}
