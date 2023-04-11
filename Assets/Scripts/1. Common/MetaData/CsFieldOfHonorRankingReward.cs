using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsFieldOfHonorRankingReward
{
	int m_nHighRanking;
	int m_nLowRanking;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int HighRanking
	{
		get { return m_nHighRanking; }
	}

	public int LowRanking
	{
		get { return m_nLowRanking; }
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
	public CsFieldOfHonorRankingReward(WPDFieldOfHonorRankingReward fieldOfHonorRankingReward)
	{
		m_nHighRanking = fieldOfHonorRankingReward.highRanking;
		m_nLowRanking = fieldOfHonorRankingReward.lowRanking;
		m_nRewardNo = fieldOfHonorRankingReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(fieldOfHonorRankingReward.itemRewardId);
	}
}
