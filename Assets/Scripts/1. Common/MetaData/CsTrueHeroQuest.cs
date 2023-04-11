using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsTrueHeroQuest
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nRequiredVipLevel;
	int m_nRequiredHeroLevel;
	CsNpcInfo m_csNpcQuest;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	string m_strCompletionText;
	string m_strTargetObjectPrefabName;
	float m_flTargetObjectInteractionDuration;
	float m_flTargetObjectInteractionMaxRange;
	float m_flTargetObjectScale;
	int m_nTargetObjectHeight;
	float m_flTargetObjectRadius;
	string m_strTargetObjectInteractionText;
	string m_strChattingMessage;

	List<CsTrueHeroQuestStep> m_listCsTrueHeroQuestStep;
	List<CsTrueHeroQuestReward> m_listCsTrueHeroQuestReward;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}
	
	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int RequiredVipLevel
	{
		get { return m_nRequiredVipLevel; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsNpcInfo QuestNpc
	{
		get { return m_csNpcQuest; }
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

	public string TargetObjectPrefabName
	{
		get { return m_strTargetObjectPrefabName; }
	}

	public float TargetObjectInteractionDuration
	{
		get { return m_flTargetObjectInteractionDuration; }
	}

	public float TargetObjectInteractionMaxRange
	{
		get { return m_flTargetObjectInteractionMaxRange; }
	}

	public float TargetObjectScale
	{
		get { return m_flTargetObjectScale; }
	}

	public int TargetObjectHeight
	{
		get { return m_nTargetObjectHeight; }
	}

	public float TargetObjectRadius
	{
		get { return m_flTargetObjectRadius; }
	}

	public string TargetObjectInteractionText
	{
		get { return m_strTargetObjectInteractionText; }
	}

	public string ChattingMessage
	{
		get { return m_strChattingMessage; }
	}

	public List<CsTrueHeroQuestStep> TrueHeroQuestStepList
	{
		get { return m_listCsTrueHeroQuestStep; }
	}

	public List<CsTrueHeroQuestReward> TrueHeroQuestRewardList
	{
		get { return m_listCsTrueHeroQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTrueHeroQuest(WPDTrueHeroQuest trueHeroQuest)
	{
		m_strName = CsConfiguration.Instance.GetString(trueHeroQuest.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(trueHeroQuest.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(trueHeroQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(trueHeroQuest.targetContentKey);
		m_nRequiredVipLevel = trueHeroQuest.requiredVipLevel;
		m_nRequiredHeroLevel = trueHeroQuest.requiredHeroLevel;
		m_csNpcQuest = CsGameData.Instance.GetNpcInfo(trueHeroQuest.questNpcId);
		m_strStartDialogue = CsConfiguration.Instance.GetString(trueHeroQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(trueHeroQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(trueHeroQuest.completionTextKey);
		m_strTargetObjectPrefabName = trueHeroQuest.targetObjectPrefabName;
		m_flTargetObjectInteractionDuration = trueHeroQuest.targetObjectInteractionDuration;
		m_flTargetObjectInteractionMaxRange = trueHeroQuest.targetObjectInteractionMaxRange;
		m_flTargetObjectScale = trueHeroQuest.targetObjectScale;
		m_nTargetObjectHeight = trueHeroQuest.targetObjectHeight;
		m_flTargetObjectRadius = trueHeroQuest.targetObjectRadius;
		m_strTargetObjectInteractionText = CsConfiguration.Instance.GetString(trueHeroQuest.targetObjectInteractionTextKey);
		m_strChattingMessage = CsConfiguration.Instance.GetString(trueHeroQuest.chattingMessageKey);

		m_listCsTrueHeroQuestStep = new List<CsTrueHeroQuestStep>();
		m_listCsTrueHeroQuestReward = new List<CsTrueHeroQuestReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsTrueHeroQuestStep GetTrueHeroStep(int nStepNo, int nVipLevel)
	{
		CsVipLevel csVipLevel = CsGameData.Instance.GetVipLevel(nVipLevel);

		if (csVipLevel == null)
		{
			return null;
		}

		if (csVipLevel.TrueHeroQuestStepNo < nStepNo)
		{
			return null;
		}

		for (int i = 0; i < m_listCsTrueHeroQuestStep.Count; i++)
		{
			if (m_listCsTrueHeroQuestStep[i].StepNo == nStepNo)
			{
				return m_listCsTrueHeroQuestStep[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTrueHeroQuestReward GetTrueHeroReward(int nLevel)
	{
		for (int i = 0; i < m_listCsTrueHeroQuestReward.Count; i++)
		{
			if (m_listCsTrueHeroQuestReward[i].Level == nLevel)
			{
				return m_listCsTrueHeroQuestReward[i];
			}
		}

		return null;
	}
}
