using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsFieldBossEventAvailableReward
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
	public CsFieldBossEventAvailableReward(WPDFieldBossEventAvailableReward fieldBossEventAvailableReward)
	{
		m_nRewardNo = fieldBossEventAvailableReward.rewardNo;
        m_csItem = CsGameData.Instance.GetItem(fieldBossEventAvailableReward.itemId);
	}
}
