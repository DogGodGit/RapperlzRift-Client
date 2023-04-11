using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsGoldDungeonDifficulty
{
	int m_nDifficulty;
	int m_nRequiredHeroLevel;
	CsGoldReward m_csGoldReward;

	List<CsGoldDungeonStep> m_listCsGoldDungeonStep;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public List<CsGoldDungeonStep> GoldDungeonStepList
	{
		get { return m_listCsGoldDungeonStep; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldDungeonDifficulty(WPDGoldDungeonDifficulty goldDungeonDifficulty)
	{
		m_nDifficulty = goldDungeonDifficulty.difficulty;
		m_nRequiredHeroLevel = goldDungeonDifficulty.requiredHeroLevel;
		m_csGoldReward = CsGameData.Instance.GetGoldReward(goldDungeonDifficulty.goldRewardId);

		m_listCsGoldDungeonStep = new List<CsGoldDungeonStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldDungeonStep GetGoldDungeonStep(int nStep)
	{
		for (int i = 0; i < m_listCsGoldDungeonStep.Count; i++)
		{
			if (m_listCsGoldDungeonStep[i].Step == nStep)
				return m_listCsGoldDungeonStep[i];
		}

		return null;
	}
}
