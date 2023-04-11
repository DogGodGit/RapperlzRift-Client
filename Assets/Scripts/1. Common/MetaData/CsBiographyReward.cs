using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsBiographyReward
{
	int m_nBiographyId;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int BiographyId
	{
		get { return m_nBiographyId; }
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
	public CsBiographyReward(WPDBiographyReward biographyReward)
	{
		m_nBiographyId = biographyReward.biographyId;
		m_nRewardNo = biographyReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(biographyReward.itemRewardId);
	}
}
