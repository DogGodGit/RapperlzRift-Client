using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-13)
//---------------------------------------------------------------------------------------------------

public class CsSupplySupportQuestOrder
{
	int m_nOrderId;
	CsItem m_csItemOrder;
	CsGoldReward m_csGoldRewardFailRefund;

	//---------------------------------------------------------------------------------------------------
	public int OrderId
	{
		get { return m_nOrderId; }
	}

	public CsItem OrderItem
	{
		get { return m_csItemOrder; }
	}

	public CsGoldReward FailRefundGoldReward
	{
		get { return m_csGoldRewardFailRefund; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuestOrder(WPDSupplySupportQuestOrder supplySupportQuestOrder)
	{
		m_nOrderId = supplySupportQuestOrder.orderId;
		m_csItemOrder = CsGameData.Instance.GetItem(supplySupportQuestOrder.orderItemId);
		m_csGoldRewardFailRefund = CsGameData.Instance.GetGoldReward(supplySupportQuestOrder.failRefundGoldRewardId);
	}
}
