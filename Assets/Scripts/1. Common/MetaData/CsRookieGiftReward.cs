using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsRookieGiftReward
{
	int m_nGiftNo;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int GiftNo
	{
		get { return m_nGiftNo; }
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
	public CsRookieGiftReward(WPDRookieGiftReward rookieGiftReward)
	{
		m_nGiftNo = rookieGiftReward.giftNo;
		m_nRewardNo = rookieGiftReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(rookieGiftReward.itemRewardId);
	}

}
