using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicMonsterSkillCastingGuide
{
	int m_nStep;
	int m_nWaveNo;
	int m_nArrangeNo;
	int m_nMonsterSkillId;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public int ArrangeNo
	{
		get { return m_nArrangeNo; }
	}

	public int MonsterSkillId
	{
		get { return m_nMonsterSkillId; }
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

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicMonsterSkillCastingGuide(WPDAncientRelicMonsterSkillCastingGuide aAncientRelicMonsterSkillCastingGuide)
	{
		m_nStep = aAncientRelicMonsterSkillCastingGuide.step;
		m_nWaveNo = aAncientRelicMonsterSkillCastingGuide.waveNo;
		m_nArrangeNo = aAncientRelicMonsterSkillCastingGuide.arrangeNo;
		m_nMonsterSkillId = aAncientRelicMonsterSkillCastingGuide.monsterSkillId;
		m_strGuideImageName = aAncientRelicMonsterSkillCastingGuide.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(aAncientRelicMonsterSkillCastingGuide.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(aAncientRelicMonsterSkillCastingGuide.guideContentKey);
	}
}
