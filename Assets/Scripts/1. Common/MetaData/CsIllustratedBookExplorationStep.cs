using System.Collections.Generic;
using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookExplorationStep
{
	int m_nStepNo;
	string m_strName;
	string m_strDescription;
	int m_nActivationExplorationPoint;
	CsGoldReward m_csGoldReward;

	List<CsIllustratedBookExplorationStepAttr> m_listCsIllustratedBookExplorationStepAttr;
	List<CsIllustratedBookExplorationStepReward> m_listCsIllustratedBookExplorationStepReward;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int ActivationExplorationPoint
	{
		get { return m_nActivationExplorationPoint; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public List<CsIllustratedBookExplorationStepAttr> IllustratedBookExplorationStepAttrList
	{
		get { return m_listCsIllustratedBookExplorationStepAttr; }
	}

	public List<CsIllustratedBookExplorationStepReward> IllustratedBookExplorationStepRewardList
	{
		get { return m_listCsIllustratedBookExplorationStepReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookExplorationStep(WPDIllustratedBookExplorationStep illustratedBookExplorationStep)
	{
		m_nStepNo = illustratedBookExplorationStep.stepNo;
		m_strName = CsConfiguration.Instance.GetString(illustratedBookExplorationStep.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(illustratedBookExplorationStep.descriptionKey);
		m_nActivationExplorationPoint = illustratedBookExplorationStep.activationExplorationPoint;
		m_csGoldReward = CsGameData.Instance.GetGoldReward(illustratedBookExplorationStep.goldRewardId);

		m_listCsIllustratedBookExplorationStepAttr = new List<CsIllustratedBookExplorationStepAttr>();
		m_listCsIllustratedBookExplorationStepReward = new List<CsIllustratedBookExplorationStepReward>();
	}
}
