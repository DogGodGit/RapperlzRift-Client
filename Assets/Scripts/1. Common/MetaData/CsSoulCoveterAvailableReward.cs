using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-10)
//---------------------------------------------------------------------------------------------------

public class CsSoulCoveterAvailableReward
{
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSoulCoveterAvailableReward(WPDSoulCoveterAvailableReward soulCoveterAvailableReward)
	{
		m_nRewardNo = soulCoveterAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(soulCoveterAvailableReward.itemId);
	}
}
