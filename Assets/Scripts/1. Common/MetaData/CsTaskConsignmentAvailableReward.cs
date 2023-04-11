using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsTaskConsignmentAvailableReward
{
	int m_nConsignmentId;
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int ConsignmentId
	{
		get { return m_nConsignmentId; }
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
	public CsTaskConsignmentAvailableReward(WPDTaskConsignmentAvailableReward taskConsignmentAvailableReward)
	{
		m_nConsignmentId = taskConsignmentAvailableReward.consignmentId;
		m_nRewardNo = taskConsignmentAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(taskConsignmentAvailableReward.itemId);
	}
}
