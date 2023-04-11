using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsRookieGift
{
	int m_nGiftNo;
	int m_nWaitingTime;

	List<CsRookieGiftReward> m_listCsRookieGiftReward;

	//---------------------------------------------------------------------------------------------------
	public int GiftNo
	{
		get { return m_nGiftNo; }
	}

	public int WaitingTime
	{
		get { return m_nWaitingTime; }
	}

	public List<CsRookieGiftReward> RookieGiftRewardList
	{
		get { return m_listCsRookieGiftReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRookieGift(WPDRookieGift rookieGift)
	{
		m_nGiftNo = rookieGift.giftNo;
		m_nWaitingTime = rookieGift.waitingTime;

		m_listCsRookieGiftReward = new List<CsRookieGiftReward>();
	}

}
