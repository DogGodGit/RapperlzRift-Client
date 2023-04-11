using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsChargeEventMission
{
	int m_nEventId;
	int m_nMissionNo;
	int m_nRequiredUnOwnDia;

	List<CsChargeEventMissionReward> m_listCsChargeEventMissionReward;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int MissionNo
	{
		get { return m_nMissionNo; }
	}

	public int RequiredUnOwnDia
	{
		get { return m_nRequiredUnOwnDia; }
	}

	public List<CsChargeEventMissionReward> ChargeEventMissionRewardList
	{
		get { return m_listCsChargeEventMissionReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsChargeEventMission(WPDChargeEventMission chargeEventMission)
	{
		m_nEventId = chargeEventMission.eventId;
		m_nMissionNo = chargeEventMission.missionNo;
		m_nRequiredUnOwnDia = chargeEventMission.requiredUnOwnDia;

		m_listCsChargeEventMissionReward = new List<CsChargeEventMissionReward>();
	}
}
