using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-22)
//---------------------------------------------------------------------------------------------------

public class CsOsirisRoomDifficultyWave
{
	int m_nDifficulty;
	int m_nWaveNo;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOsirisRoomDifficultyWave(WPDOsirisRoomDifficultyWave osirisRoomDifficultyWave)
	{
		m_nDifficulty = osirisRoomDifficultyWave.difficulty;
		m_nWaveNo = osirisRoomDifficultyWave.waveNo;
	}
}
