using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-10)
//---------------------------------------------------------------------------------------------------

public class CsSoulCoveterDifficultyWave
{
	int m_nDifficulty;
	int m_nWaveNo;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nTargetArrangeNo;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int TargetArrangeNo
	{
		get { return m_nTargetArrangeNo; }
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
	public CsSoulCoveterDifficultyWave(WPDSoulCoveterDifficultyWave soulCoveterDifficultyWave)
	{
		m_nDifficulty = soulCoveterDifficultyWave.difficulty;
		m_nWaveNo = soulCoveterDifficultyWave.waveNo;
		m_strTargetTitle = CsConfiguration.Instance.GetString(soulCoveterDifficultyWave.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(soulCoveterDifficultyWave.targetContentKey);
		m_nTargetArrangeNo = soulCoveterDifficultyWave.targetArrangeNo;
		m_strGuideImageName = soulCoveterDifficultyWave.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(soulCoveterDifficultyWave.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(soulCoveterDifficultyWave.guideContentKey);
	}
}
