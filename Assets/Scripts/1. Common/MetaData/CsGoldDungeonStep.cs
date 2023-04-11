using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsGoldDungeonStep
{
	int m_nDifficulty;
	int m_nStep;
	CsGoldReward m_csGoldReward;

	List<CsGoldDungeonStepWave> m_listCsGoldDungeonStepWave;
	List<CsGoldDungeonStepMonsterArrange> m_listCsGoldDungeonStepMonsterArrange;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public List<CsGoldDungeonStepWave> GoldDungeonStepWaveList
	{
		get { return m_listCsGoldDungeonStepWave; }
	}

	public List<CsGoldDungeonStepMonsterArrange> GoldDungeonStepMonsterArrangeList
	{
		get { return m_listCsGoldDungeonStepMonsterArrange; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldDungeonStep(WPDGoldDungeonStep goldDungeonStep)
	{
		m_nDifficulty = goldDungeonStep.difficulty;
		m_nStep = goldDungeonStep.step;
		m_csGoldReward = CsGameData.Instance.GetGoldReward(goldDungeonStep.goldRewardId);

		m_listCsGoldDungeonStepWave = new List<CsGoldDungeonStepWave>();
		m_listCsGoldDungeonStepMonsterArrange = new List<CsGoldDungeonStepMonsterArrange>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldDungeonStepWave GetGoldDungeonStepWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsGoldDungeonStepWave.Count; i++)
		{
			if (m_listCsGoldDungeonStepWave[i].WaveNo == nWaveNo)
				return m_listCsGoldDungeonStepWave[i];
		}

		return null;
	}
}
