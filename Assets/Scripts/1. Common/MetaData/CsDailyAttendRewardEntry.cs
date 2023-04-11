using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsDailyAttendRewardEntry
{
	int m_nDay;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int Day
	{
		get { return m_nDay; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyAttendRewardEntry(WPDDailyAttendRewardEntry dailyAttendRewardEntry)
	{
		m_nDay = dailyAttendRewardEntry.day;
		m_csItemReward = CsGameData.Instance.GetItemReward(dailyAttendRewardEntry.itemRewardId);
	}
}
