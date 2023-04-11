using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsTrueHeroQuestStep
{
	int m_nStepNo;
	string m_strTargetContent;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetObjectPosition;
	int m_nObjectiveWaitingTime;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetObjectPosition
	{
		get { return m_vtTargetObjectPosition; }
	}
	
	public int ObjectiveWaitingTime
	{
		get { return m_nObjectiveWaitingTime; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTrueHeroQuestStep(WPDTrueHeroQuestStep trueHeroQuestStep)
	{
		m_nStepNo = trueHeroQuestStep.stepNo;
		m_strTargetContent = CsConfiguration.Instance.GetString(trueHeroQuestStep.targetContentKey);
		m_csContinentTarget = CsGameData.Instance.GetContinent(trueHeroQuestStep.targetContinentId);
		m_vtTargetObjectPosition = new Vector3(trueHeroQuestStep.targetObjectXPosition, trueHeroQuestStep.targetObjectYPosition, trueHeroQuestStep.targetObjectZPosition);
		m_nObjectiveWaitingTime = trueHeroQuestStep.objectiveWaitingTime;
		m_csItemReward = CsGameData.Instance.GetItemReward(trueHeroQuestStep.itemRewardId);
	}
}
