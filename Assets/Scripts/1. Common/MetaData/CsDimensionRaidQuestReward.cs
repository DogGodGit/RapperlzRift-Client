using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsDimensionRaidQuestReward
{
	int m_nLevel;
	CsExpReward m_csExpReward;
	CsExploitPointReward m_csExploitPointReward;
	CsItemReward m_csItemRewardId;

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

	public CsItemReward ItemRewardId
	{
		get { return m_csItemRewardId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDimensionRaidQuestReward(WPDDimensionRaidQuestReward dimensionRaidQuestReward)
	{
		m_nLevel = dimensionRaidQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(dimensionRaidQuestReward.expRewardId);
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(dimensionRaidQuestReward.exploitPointRewardId);
		m_csItemRewardId = CsGameData.Instance.GetItemReward(dimensionRaidQuestReward.itemRewardId);
	}
}
