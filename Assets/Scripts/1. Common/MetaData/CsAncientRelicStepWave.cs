using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicStepWave
{
	int m_nStep;
	int m_nWaveNo;
	bool m_bIsGuideDisplay;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	List<CsAncientRelicMonsterSkillCastingGuide> m_listCsAncientRelicMonsterSkillCastingGuide;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public bool IsGuideDisplay
	{
		get { return m_bIsGuideDisplay; }
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

	public List<CsAncientRelicMonsterSkillCastingGuide> AncientRelicMonsterSkillCastingGuideList
	{
		get { return m_listCsAncientRelicMonsterSkillCastingGuide; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicStepWave(WPDAncientRelicStepWave ancientRelicStepWave)
	{
		m_nStep = ancientRelicStepWave.step;
		m_nWaveNo = ancientRelicStepWave.waveNo;
		m_bIsGuideDisplay = ancientRelicStepWave.isGuideDisplay;
		m_strGuideImageName = ancientRelicStepWave.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(ancientRelicStepWave.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(ancientRelicStepWave.guideContentKey);

		m_listCsAncientRelicMonsterSkillCastingGuide = new List<CsAncientRelicMonsterSkillCastingGuide>();
	}
}
