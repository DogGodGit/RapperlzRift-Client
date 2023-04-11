using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsConsumeEventMission
{
	int m_nEventId;
	int m_nMissionNo;
	int m_nRequiredDia;

	List<CsConsumeEventMissionReward> m_listCsConsumeEventMissionReward;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int MissionNo
	{
		get { return m_nMissionNo; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	public List<CsConsumeEventMissionReward> ConsumeEventMissionRewardList
	{
		get { return m_listCsConsumeEventMissionReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConsumeEventMission(WPDConsumeEventMission consumeEventMission)
	{
		m_nEventId = consumeEventMission.eventId;
		m_nMissionNo = consumeEventMission.missionNo;
		m_nRequiredDia = consumeEventMission.requiredDia;

		m_listCsConsumeEventMissionReward = new List<CsConsumeEventMissionReward>();
	}
}
