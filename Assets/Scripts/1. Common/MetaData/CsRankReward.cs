using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsRankReward
{
	int m_nRankNo;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int RankNo
	{
		get { return m_nRankNo; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankReward(WPDRankReward rankReward)
	{
		m_nRankNo = rankReward.rankNo;
		m_nRewardNo = rankReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(rankReward.itemRewardId);
	}
}
