using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShipAvailableReward
{
	int m_nDifficulty;
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipAvailableReward(WPDTradeShipAvailableReward tradeShipAvailableReward)
	{
		m_nDifficulty = tradeShipAvailableReward.difficulty;
		m_nRewardNo = tradeShipAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(tradeShipAvailableReward.itemId);
	}
}
