using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsVipLevelReward
{
	int m_nVipLevel;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int VipLevel
	{
		get { return m_nVipLevel; }
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
	public CsVipLevelReward(WPDVipLevelReward vipLevelReward)
	{
		m_nVipLevel = vipLevelReward.vipLevel;
		m_nRewardNo = vipLevelReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(vipLevelReward.itemRewardId);
	}
}
