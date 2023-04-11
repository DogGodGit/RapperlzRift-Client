using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsAccountChargeEvent
{
	int m_nEventId;
	int m_nAccUnOwnDia;
	List<int> m_listRewardedMission;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int AccUnOwnDia
	{
		get { return m_nAccUnOwnDia; }
		set { m_nAccUnOwnDia = value; }
	}

	public List<int> RewardedMissionList
	{
		get { return m_listRewardedMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccountChargeEvent(PDAccountChargeEvent accountChargeEvent)
	{
		m_nEventId = accountChargeEvent.eventId;
		m_nAccUnOwnDia = accountChargeEvent.accUnOwnDia;
		m_listRewardedMission = new List<int>(accountChargeEvent.rewardedMissions);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsRewardedMission(int nMissionNo)
	{
		return m_listRewardedMission.Contains(nMissionNo);
	}
}
