using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-22)
//---------------------------------------------------------------------------------------------------

public class CsOsirisRoomDifficulty
{
	int m_nDifficulty;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;

	List<CsOsirisRoomDifficultyWave> m_listCsOsirisRoomDifficultyWave;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public List<CsOsirisRoomDifficultyWave> OsirisRoomDifficultyWaveList
	{
		get { return m_listCsOsirisRoomDifficultyWave; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOsirisRoomDifficulty(WPDOsirisRoomDifficulty osirisRoomDifficulty)
	{
		m_nDifficulty = osirisRoomDifficulty.difficulty;
		m_nRequiredConditionType = osirisRoomDifficulty.requiredConditionType;
		m_nRequiredMainQuestNo = osirisRoomDifficulty.requiredMainQuestNo;
		m_nRequiredHeroLevel = osirisRoomDifficulty.requiredHeroLevel;

		m_listCsOsirisRoomDifficultyWave = new List<CsOsirisRoomDifficultyWave>();
	}

	public CsOsirisRoomDifficultyWave GetOsirisRoomDifficultyWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsOsirisRoomDifficultyWave.Count; i++)
		{
			if (m_listCsOsirisRoomDifficultyWave[i].WaveNo == nWaveNo)
				return m_listCsOsirisRoomDifficultyWave[i];
		}
		return null;
	}
}
