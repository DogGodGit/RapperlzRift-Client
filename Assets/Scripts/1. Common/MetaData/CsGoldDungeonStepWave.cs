using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsGoldDungeonStepWave
{
	int m_nDifficulty;
	int m_nStep;
	int m_nWaveNo;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nLimitTime;
	int m_nNextWaveIntervalTime;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int Step
	{
		get { return m_nStep; }
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

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public int NextWaveIntervalTime
	{
		get { return m_nNextWaveIntervalTime; }
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
	public CsGoldDungeonStepWave(WPDGoldDungeonStepWave goldDungeonStepWave)
	{
		m_nDifficulty = goldDungeonStepWave.difficulty;
		m_nStep = goldDungeonStepWave.step;
		m_nWaveNo = goldDungeonStepWave.waveNo;
		m_strTargetTitle = CsConfiguration.Instance.GetString(goldDungeonStepWave.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(goldDungeonStepWave.targetContentKey);
		m_nLimitTime = goldDungeonStepWave.limitTime;
		m_nNextWaveIntervalTime = goldDungeonStepWave.nextWaveIntervalTime;
		m_strGuideImageName = goldDungeonStepWave.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(goldDungeonStepWave.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(goldDungeonStepWave.guideContentKey);
	}
}
