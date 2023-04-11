using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsLimitationGiftRewardSchedule
{
	int m_nScheduleId;
	int m_nStartTime;
	int m_nEndTime;

	List<CsLimitationGiftAvailableReward> m_listCsLimitationGiftAvailableReward;

	//---------------------------------------------------------------------------------------------------
	public int ScheduleId
	{
		get { return m_nScheduleId; }
	}

	public int StartTime
	{
		get { return m_nStartTime; }
	}

	public int EndTime
	{
		get { return m_nEndTime; }
	}

	public List<CsLimitationGiftAvailableReward> LimitationGiftAvailableRewardList
	{
		get { return m_listCsLimitationGiftAvailableReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLimitationGiftRewardSchedule(WPDLimitationGiftRewardSchedule limitationGiftRewardSchedule)
	{
		m_nScheduleId = limitationGiftRewardSchedule.scheduleId;
		m_nStartTime = limitationGiftRewardSchedule.startTime;
		m_nEndTime = limitationGiftRewardSchedule.endTime;

		m_listCsLimitationGiftAvailableReward = new List<CsLimitationGiftAvailableReward>();
	}
}
