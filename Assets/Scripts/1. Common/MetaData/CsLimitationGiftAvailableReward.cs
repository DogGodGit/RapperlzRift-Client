using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsLimitationGiftAvailableReward : IComparable
{
	int m_nScheduleId;
	int m_nRewardNo;
	CsItem m_csItem;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int ScheduleId
	{
		get { return m_nScheduleId; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLimitationGiftAvailableReward(WPDLimitationGiftAvailableReward limitationGiftAvailableReward)
	{
		m_nScheduleId = limitationGiftAvailableReward.scheduleId;
		m_nRewardNo = limitationGiftAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(limitationGiftAvailableReward.itemId);
		m_nSortNo = limitationGiftAvailableReward.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsLimitationGiftAvailableReward)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
