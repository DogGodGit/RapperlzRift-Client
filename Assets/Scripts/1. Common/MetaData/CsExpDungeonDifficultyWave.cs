using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsExpDungeonDifficultyWave
{
	int m_nDifficulty;
	int m_nWaveNo;
	int m_nWaveLimitTime;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nLakChargeAmount;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public int WaveLimitTime
	{
		get { return m_nWaveLimitTime; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int LakChargeAmount
	{
		get { return m_nLakChargeAmount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsExpDungeonDifficultyWave(WPDExpDungeonDifficultyWave expDungeonDifficultyWave)
	{
		m_nDifficulty = expDungeonDifficultyWave.difficulty;
		m_nWaveNo = expDungeonDifficultyWave.waveNo;
		m_nWaveLimitTime = expDungeonDifficultyWave.waveLimitTime;
		m_nLakChargeAmount = expDungeonDifficultyWave.lakChargeAmount;
		m_strTargetTitle = CsConfiguration.Instance.GetString(expDungeonDifficultyWave.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(expDungeonDifficultyWave.targetContentKey);
	}
}
