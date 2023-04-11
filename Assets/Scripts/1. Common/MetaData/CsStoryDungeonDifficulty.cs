using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeonDifficulty
{
	int m_nDungeonNo;
	int m_nDifficulty;
	string m_strName;
	string m_strDescription;
	long m_lRecommendBattlePower;

	List<CsStoryDungeonAvailableReward> m_listCsStoryDungeonAvailableReward;
	List<CsStoryDungeonStep> m_listCsStoryDungeonStep;
	List<CsStoryDungeonTrap> m_listCsStoryDungeonTrap;

	//---------------------------------------------------------------------------------------------------
	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public long RecommendBattlePower
	{
		get { return m_lRecommendBattlePower; }
	}

	public List<CsStoryDungeonAvailableReward> StoryDungeonAvailableRewardList
	{
		get { return m_listCsStoryDungeonAvailableReward; }
	}

	public List<CsStoryDungeonStep> StoryDungeonStepList
	{
		get { return m_listCsStoryDungeonStep; }
	}

	public List<CsStoryDungeonTrap> StoryDungeonTrapList
	{
		get { return m_listCsStoryDungeonTrap; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeonDifficulty(WPDStoryDungeonDifficulty storyDungeonDifficulty)
	{
		m_nDungeonNo = storyDungeonDifficulty.dungeonNo;
		m_nDifficulty = storyDungeonDifficulty.difficulty;
		m_strName = CsConfiguration.Instance.GetString(storyDungeonDifficulty.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(storyDungeonDifficulty.descriptionKey);
		m_lRecommendBattlePower = storyDungeonDifficulty.recommendBattlePower;

		m_listCsStoryDungeonAvailableReward = new List<CsStoryDungeonAvailableReward>();
		m_listCsStoryDungeonStep = new List<CsStoryDungeonStep>();
		m_listCsStoryDungeonTrap = new List<CsStoryDungeonTrap>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeonStep GetStoryDungeonStep(int nStep)
	{
		for (int i = 0; i < m_listCsStoryDungeonStep.Count; i++)
		{
			if (m_listCsStoryDungeonStep[i].Step == nStep)
				return m_listCsStoryDungeonStep[i];
		}

		return null;
	}
}
