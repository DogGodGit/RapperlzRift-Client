using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsAccountConsumeEvent
{
	int m_nEventId;
	int m_nAccDia;
	List<int> m_listRewardedMission;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int AccDia
	{
		get { return m_nAccDia; }
		set { m_nAccDia = value; }
	}

	public List<int> RewardedMissionList
	{
		get { return m_listRewardedMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccountConsumeEvent(PDAccountConsumeEvent accountConsumeEvent)
	{
		m_nEventId = accountConsumeEvent.eventId;
		m_nAccDia = accountConsumeEvent.accDia;
		m_listRewardedMission = new List<int>(accountConsumeEvent.rewardedMissions);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsRewardedMission(int nMissionNo)
	{
		return m_listRewardedMission.Contains(nMissionNo);
	}
}
