using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-05)
//---------------------------------------------------------------------------------------------------

public class CsGuildSupplySupportQuest
{
	string m_strName;
	string m_strDescription;
	int m_nLimitTime;
	CsNpcInfo m_csNpcStart;
	int m_nCartId;
	CsNpcInfo m_csNpcCompletion;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	CsGuildBuildingPointReward m_csGuildBuildingPointReward;
	CsGuildFundReward m_csGuildFundReward;
	float m_flCompletionRewardableRadius;
	CsGuildContributionPointReward m_csGuildContributionPointRewardCompletion;

	List<CsGuildSupplySupportQuestReward> m_listCsGuildSupplySupportQuestReward;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public CsNpcInfo StartNpc
	{
		get { return m_csNpcStart; }
	}

	public int CartId
	{
		get { return m_nCartId; }
	}

	public CsNpcInfo CompletionNpc
	{
		get { return m_csNpcCompletion; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public CsGuildBuildingPointReward GuildBuildingPointReward
	{
		get { return m_csGuildBuildingPointReward; }
	}

	public CsGuildFundReward GuildFundReward
	{
		get { return m_csGuildFundReward; }
	}

	public float CompletionRewardableRadius
	{
		get { return m_flCompletionRewardableRadius; }
	}

	public CsGuildContributionPointReward CompletionGuildContributionPointReward
	{
		get { return m_csGuildContributionPointRewardCompletion; }
	}

	public List<CsGuildSupplySupportQuestReward> GuildSupplySupportQuestRewardList
	{
		get { return m_listCsGuildSupplySupportQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSupplySupportQuest(WPDGuildSupplySupportQuest guildSupplySupportQuest)
	{
		m_strName = CsConfiguration.Instance.GetString(guildSupplySupportQuest.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(guildSupplySupportQuest.descriptionKey);
		m_nLimitTime = guildSupplySupportQuest.limitTime;
		m_csNpcStart = CsGameData.Instance.GetNpcInfo(guildSupplySupportQuest.startNpcId);
		m_nCartId = guildSupplySupportQuest.cartId;
		m_csNpcCompletion = CsGameData.Instance.GetNpcInfo(guildSupplySupportQuest.completionNpcId);
		m_strStartDialogue = CsConfiguration.Instance.GetString(guildSupplySupportQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(guildSupplySupportQuest.completionDialogueKey);
		m_csGuildBuildingPointReward = CsGameData.Instance.GetGuildBuildingPointReward(guildSupplySupportQuest.guildBuildingPointRewardId);
		m_csGuildFundReward = CsGameData.Instance.GetGuildFundReward(guildSupplySupportQuest.guildFundRewardId);
		m_flCompletionRewardableRadius = guildSupplySupportQuest.completionRewardableRadius;
		m_csGuildContributionPointRewardCompletion = CsGameData.Instance.GetGuildContributionPointReward(guildSupplySupportQuest.completionGuildContributionPointRewardId);

		m_listCsGuildSupplySupportQuestReward = new List<CsGuildSupplySupportQuestReward>();
	}

    //---------------------------------------------------------------------------------------------------
    public CsGuildSupplySupportQuestReward GetGuildSupplySupportQuestReward(int nLevel)
    {
        for (int i = 0; i < m_listCsGuildSupplySupportQuestReward.Count; ++i)
        {
            if (m_listCsGuildSupplySupportQuestReward[i].Level == nLevel)
                return m_listCsGuildSupplySupportQuestReward[i];
        }

        return null;
    }

}
