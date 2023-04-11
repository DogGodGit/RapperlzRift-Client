using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsBountyHunterQuest
{
	int m_nQuestId;
	string m_strTitle;
	string m_strContent;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strDescription;
	int m_nTargetMonsterMinLevel;
	int m_nTargetCount;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strStartGuideContent;
	string m_strCompletionGuideContent;
			
	//---------------------------------------------------------------------------------------------------
	public int QuestId
	{
		get { return m_nQuestId; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string Content
	{
		get { return m_strContent; }
	}

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

	public int TargetMonsterMinLevel
	{
		get { return m_nTargetMonsterMinLevel; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetPosition
	{
		get { return m_vtTargetPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public string GuideTitle
	{
		get { return m_strGuideTitle; }
	}

	public string StartGuideContent
	{
		get { return m_strStartGuideContent; }
	}

	public string CompletionGuideContent
	{
		get { return m_strCompletionGuideContent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBountyHunterQuest(WPDBountyHunterQuest bountyHunterQuest)
	{
		m_nQuestId = bountyHunterQuest.questId;
		m_strTitle = CsConfiguration.Instance.GetString(bountyHunterQuest.titleKey);
		m_strContent = CsConfiguration.Instance.GetString(bountyHunterQuest.contentKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(bountyHunterQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(bountyHunterQuest.targetContentKey);
		m_strDescription = CsConfiguration.Instance.GetString(bountyHunterQuest.descriptionKey);
		m_nTargetMonsterMinLevel = bountyHunterQuest.targetMonsterMinLevel;
		m_nTargetCount = bountyHunterQuest.targetCount;
		m_csContinentTarget = CsGameData.Instance.GetContinent(bountyHunterQuest.targetContinentId);
		m_vtTargetPosition = new Vector3(bountyHunterQuest.targetXPosition, bountyHunterQuest.targetYPosition, bountyHunterQuest.targetZPosition);
		m_flTargetRadius = bountyHunterQuest.targetRadius;
		m_strGuideImageName = bountyHunterQuest.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(bountyHunterQuest.guideTitleKey);
		m_strStartGuideContent = CsConfiguration.Instance.GetString(bountyHunterQuest.startGuideContentKey);
		m_strCompletionGuideContent = CsConfiguration.Instance.GetString(bountyHunterQuest.completionGuideContentKey);
	}
}
