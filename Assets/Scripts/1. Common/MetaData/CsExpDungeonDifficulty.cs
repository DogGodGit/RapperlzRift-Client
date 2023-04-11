using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsExpDungeonDifficulty
{
	int m_nDifficulty;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	CsExpReward m_csExpReward;

	List<CsExpDungeonDifficultyWave> m_listCsExpDungeonDifficultyWave;

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

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public List<CsExpDungeonDifficultyWave> ExpDungeonDifficultyWaveList
	{
		get { return m_listCsExpDungeonDifficultyWave; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsExpDungeonDifficulty(WPDExpDungeonDifficulty expDungeonDifficulty)
	{
		m_nDifficulty = expDungeonDifficulty.difficulty;
		m_nRequiredConditionType = expDungeonDifficulty.requiredConditionType;
		m_nRequiredMainQuestNo = expDungeonDifficulty.requiredMainQuestNo;
		m_nRequiredHeroLevel = expDungeonDifficulty.requiredHeroLevel;
		m_csExpReward = CsGameData.Instance.GetExpReward(expDungeonDifficulty.expRewardId);

		m_listCsExpDungeonDifficultyWave = new List<CsExpDungeonDifficultyWave>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsExpDungeonDifficultyWave GetExpDungeonDifficultyWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsExpDungeonDifficultyWave.Count; i++)
		{
			if (m_listCsExpDungeonDifficultyWave[i].WaveNo == nWaveNo)
				return m_listCsExpDungeonDifficultyWave[i];
		}

		return null;
	}
}
