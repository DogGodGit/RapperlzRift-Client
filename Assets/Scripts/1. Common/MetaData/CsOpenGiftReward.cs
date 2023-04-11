using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsOpenGiftReward
{
	int m_nDay;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int Day
	{
		get { return m_nDay; }
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
	public CsOpenGiftReward(WPDOpenGiftReward openGiftReward)
	{
		m_nDay = openGiftReward.day;
		m_nRewardNo = openGiftReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(openGiftReward.itemRewardId);
	}
}
