using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsSecretLetterQuest
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

	List<CsSecretLetterQuestReward> m_listCsSecretLetterQuestReward;

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

	public List<CsSecretLetterQuestReward> SecretLetterQuestRewardList
	{
		get { return m_listCsSecretLetterQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSecretLetterQuest(WPDSecretLetterQuest secretLetterQuest)
	{
		m_strTargetTitle = CsConfiguration.Instance.GetString(secretLetterQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(secretLetterQuest.targetContentKey);
		m_strDescription = CsConfiguration.Instance.GetString(secretLetterQuest.descriptionKey);
		m_nRequiredHeroLevel = secretLetterQuest.requiredHeroLevel;
		m_csNpcInfoQuest = CsGameData.Instance.GetNpcInfo(secretLetterQuest.questNpcId);
		m_csNpcInfoTarget = CsGameData.Instance.GetNpcInfo(secretLetterQuest.targetNpcId);
		m_nLimitCount = secretLetterQuest.limitCount;
		m_nInteractionDuration = secretLetterQuest.interactionDuration;
		m_strStartDialogue = CsConfiguration.Instance.GetString(secretLetterQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(secretLetterQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(secretLetterQuest.completionTextKey);

		m_listCsSecretLetterQuestReward = new List<CsSecretLetterQuestReward>();
	}
}
