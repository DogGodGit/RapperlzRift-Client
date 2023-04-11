using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsTrueHeroQuestReward
{
	int m_nLevel;
	CsExpReward m_csExpReward;
	CsExploitPointReward m_csExploitPointReward;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public CsExploitPointReward ExploitPointReward
	{
		get { return m_csExploitPointReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTrueHeroQuestReward(WPDTrueHeroQuestReward trueHeroQuestReward)
	{
		m_nLevel = trueHeroQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(trueHeroQuestReward.expRewardId);
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(trueHeroQuestReward.exploitPointRewardId);
	}
}
