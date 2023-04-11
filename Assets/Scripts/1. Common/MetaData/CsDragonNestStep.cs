using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsDragonNestStep
{
	int m_nStepNo;
	int m_nType; // 1 이동, 2모든몬스터처치
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nStartDelayTime;
	bool m_bTargetAreaDisplayed;
	float m_flTargetXPosition;
	float m_flTargetYPosition;
	float m_flTargetZPosition;
	float m_flTargetRadius;
	int m_nRemoveObstacleId;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	List<CsDragonNestStepReward> m_listCsDragonNestStepReward;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public bool TargetAreaDisplayed
	{
		get { return m_bTargetAreaDisplayed; }
	}

	public float TargetXPosition
	{
		get { return m_flTargetXPosition; }
	}

	public float TargetYPosition
	{
		get { return m_flTargetYPosition; }
	}

	public float TargetZPosition
	{
		get { return m_flTargetZPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public int RemoveObstacleId
	{
		get { return m_nRemoveObstacleId; }
	}

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public string GuideTitle
	{
		get { return m_strGuideTitle; }
	}

	public string GuideContent
	{
		get { return m_strGuideContent; }
	}

	public List<CsDragonNestStepReward> DragonNestStepRewardList
	{
		get { return m_listCsDragonNestStepReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDragonNestStep(WPDDragonNestStep dragonNestStep)
	{
		m_nStepNo = dragonNestStep.stepNo;
		m_nType = dragonNestStep.type;
		m_strTargetTitle = CsConfiguration.Instance.GetString(dragonNestStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(dragonNestStep.targetContentKey);
		m_nStartDelayTime = dragonNestStep.startDelayTime;
		m_bTargetAreaDisplayed = dragonNestStep.targetAreaDisplayed;
		m_flTargetXPosition = dragonNestStep.targetXPosition;
		m_flTargetYPosition = dragonNestStep.targetYPosition;
		m_flTargetZPosition = dragonNestStep.targetZPosition;
		m_flTargetRadius = dragonNestStep.targetRadius;
		m_nRemoveObstacleId = dragonNestStep.removeObstacleId;
		m_strGuideImageName = dragonNestStep.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(dragonNestStep.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(dragonNestStep.guideContentKey);

		m_listCsDragonNestStepReward = new List<CsDragonNestStepReward>();
	}
}
