using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-08)
//---------------------------------------------------------------------------------------------------

public class CsMysteryBoxQuest
{
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strDescription;
	int m_nRequiredHeroLevel;
	CsNpcInfo m_csNpcInfoQuest;
	CsNpcInfo m_csNpcInfoTarget;
	int m_nLimitCount;
	int m_nInteractionDuration;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	string m_strCompletionText;

	List<CsMysteryBoxQuestReward> m_listCsMysteryBoxQuestReward;

	//---------------------------------------------------------------------------------------------------
	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsNpcInfo QuestNpcInfo
	{
		get { return m_csNpcInfoQuest; }
	}

	public CsNpcInfo TargetNpcInfo
	{
		get { return m_csNpcInfoTarget; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public int InteractionDuration
	{
		get { return m_nInteractionDuration; }
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

	public List<CsMysteryBoxQuestReward> MysteryBoxQuestRewardList
	{
		get { return m_listCsMysteryBoxQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMysteryBoxQuest(WPDMysteryBoxQuest mysteryBoxQuest)
	{
		m_strTargetTitle = CsConfiguration.Instance.GetString(mysteryBoxQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(mysteryBoxQuest.targetContentKey);
		m_strDescription = CsConfiguration.Instance.GetString(mysteryBoxQuest.descriptionKey);
		m_nRequiredHeroLevel = mysteryBoxQuest.requiredHeroLevel;
		m_csNpcInfoQuest = CsGameData.Instance.GetNpcInfo(mysteryBoxQuest.questNpcId);
		m_csNpcInfoTarget = CsGameData.Instance.GetNpcInfo(mysteryBoxQuest.targetNpcId);
		m_nLimitCount = mysteryBoxQuest.limitCount;
		m_nInteractionDuration = mysteryBoxQuest.interactionDuration;
		m_strStartDialogue = CsConfiguration.Instance.GetString(mysteryBoxQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(mysteryBoxQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(mysteryBoxQuest.completionTextKey);

		m_listCsMysteryBoxQuestReward = new List<CsMysteryBoxQuestReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsMysteryBoxQuestReward GetMysteryBoxQuestReward(int nGrade, int nLevel)
	{
		for (int i = 0; i < m_listCsMysteryBoxQuestReward.Count; i++)
		{
			if (m_listCsMysteryBoxQuestReward[i].MysteryBoxGrade.Grade == nGrade && m_listCsMysteryBoxQuestReward[i].Level == nLevel)
				return m_listCsMysteryBoxQuestReward[i];
		}

		return null;
	}
}
