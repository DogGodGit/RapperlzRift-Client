using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-10)
//---------------------------------------------------------------------------------------------------

public class CsSoulCoveterDifficulty
{
	int m_nDifficulty;
	string m_strName;

	List<CsSoulCoveterDifficultyWave> m_listCsSoulCoveterDifficultyWave;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsSoulCoveterDifficultyWave> SoulCoveterDifficultyWaveList
	{
		get { return m_listCsSoulCoveterDifficultyWave; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSoulCoveterDifficulty(WPDSoulCoveterDifficulty soulCoveterDifficulty)
	{
		m_nDifficulty = soulCoveterDifficulty.difficulty;
		m_strName = CsConfiguration.Instance.GetString(soulCoveterDifficulty.nameKey);

		m_listCsSoulCoveterDifficultyWave = new List<CsSoulCoveterDifficultyWave>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSoulCoveterDifficultyWave GetSoulCoveterDifficultyWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsSoulCoveterDifficultyWave.Count; i++)
		{
			if (m_listCsSoulCoveterDifficultyWave[i].WaveNo == nWaveNo)
			{
				return m_listCsSoulCoveterDifficultyWave[i];
			}
		}

		return null;
	}
}
