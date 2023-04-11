using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsDimensionRaidQuest
{
	string m_strContent;
	int m_nRequiredHeroLevel;
	CsNpcInfo m_csNpcInfoQuest;
	int m_nLimitCount;
	int m_nTargetInteractionDuration;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	string m_strCompletionText;

	List<CsDimensionRaidQuestStep> m_listCsDimensionRaidQuestStep;
	List<CsDimensionRaidQuestReward> m_listCsDimensionRaidQuestReward;

	//---------------------------------------------------------------------------------------------------
	public string Content
	{
		get { return m_strContent; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsNpcInfo QuestNpcInfo
	{
		get { return m_csNpcInfoQuest; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public int TargetInteractionDuration
	{
		get { return m_nTargetInteractionDuration; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public string CompletionText
	{
		get { return m_strCompletionText; }
	}

	public List<CsDimensionRaidQuestStep> DimensionRaidQuestStepList
	{
		get { return m_listCsDimensionRaidQuestStep; }
	}

	public List<CsDimensionRaidQuestReward> DimensionRaidQuestRewardList
	{
		get { return m_listCsDimensionRaidQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDimensionRaidQuest(WPDDimensionRaidQuest dimensionRaidQuest)
	{
		m_strContent = CsConfiguration.Instance.GetString(dimensionRaidQuest.contentKey);
		m_nRequiredHeroLevel = dimensionRaidQuest.requiredHeroLevel;
		m_csNpcInfoQuest = CsGameData.Instance.GetNpcInfo(dimensionRaidQuest.questNpcId);
		m_nLimitCount = dimensionRaidQuest.limitCount;
		m_nTargetInteractionDuration = dimensionRaidQuest.targetInteractionDuration;
		m_strStartDialogue = CsConfiguration.Instance.GetString(dimensionRaidQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(dimensionRaidQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(dimensionRaidQuest.completionTextKey);

		m_listCsDimensionRaidQuestStep = new List<CsDimensionRaidQuestStep>();
		m_listCsDimensionRaidQuestReward = new List<CsDimensionRaidQuestReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsDimensionRaidQuestStep GetDimensionRaidQuestStep(int nStep)
	{
		for (int i = 0; i < m_listCsDimensionRaidQuestStep.Count; i++)
		{
			if (m_listCsDimensionRaidQuestStep[i].Step == nStep)
				return m_listCsDimensionRaidQuestStep[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDimensionRaidQuestReward GetDimensionRaidQuestReward(int nLevel)
	{
		for (int i = 0; i < m_listCsDimensionRaidQuestReward.Count; i++)
		{
			if (m_listCsDimensionRaidQuestReward[i].Level == nLevel)
				return m_listCsDimensionRaidQuestReward[i];
		}

		return null;
	}
}
