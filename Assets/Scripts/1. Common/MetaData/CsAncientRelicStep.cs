using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicStep
{
	int m_nStep;
	int m_nType;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nTargetPoint;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	List<CsAncientRelicStepWave> m_listCsAncientRelicStepWave;
	List<CsAncientRelicStepRoute> m_listCsAncientRelicStepRoute;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
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

	public int TargetPoint
	{
		get { return m_nTargetPoint; }
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

	public List<CsAncientRelicStepWave> AncientRelicStepWaveList
	{
		get { return m_listCsAncientRelicStepWave; }
	}

	public List<CsAncientRelicStepRoute> AncientRelicStepRouteList
	{
		get { return m_listCsAncientRelicStepRoute; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicStep(WPDAncientRelicStep ancientRelicStep)
	{
		m_nStep = ancientRelicStep.step;
		m_nType = ancientRelicStep.type;
		m_strTargetTitle = CsConfiguration.Instance.GetString(ancientRelicStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(ancientRelicStep.targetContentKey);
		m_nTargetPoint = ancientRelicStep.targetPoint;
		m_strGuideImageName = ancientRelicStep.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(ancientRelicStep.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(ancientRelicStep.guideContentKey);

		m_listCsAncientRelicStepWave = new List<CsAncientRelicStepWave>();
		m_listCsAncientRelicStepRoute = new List<CsAncientRelicStepRoute>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicStepWave GetAncientRelicStepWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsAncientRelicStepWave.Count; i++)
		{
			if (m_listCsAncientRelicStepWave[i].WaveNo == nWaveNo)
				return m_listCsAncientRelicStepWave[i];
		}

		return null;
	}
}
