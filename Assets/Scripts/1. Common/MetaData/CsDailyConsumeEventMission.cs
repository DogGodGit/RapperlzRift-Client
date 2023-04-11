using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsDailyConsumeEventMission
{
	int m_nMissionNo;
	int m_nRequiredDia;

	List<CsDailyConsumeEventMissionReward> m_listCsDailyConsumeEventMissionReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionNo
	{
		get { return m_nMissionNo; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	public List<CsDailyConsumeEventMissionReward> DailyConsumeEventMissionRewardList
	{
		get { return m_listCsDailyConsumeEventMissionReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyConsumeEventMission(WPDDailyConsumeEventMission dailyConsumeEventMission)
	{
		m_nMissionNo = dailyConsumeEventMission.missionNo;
		m_nRequiredDia = dailyConsumeEventMission.requiredDia;

		m_listCsDailyConsumeEventMissionReward = new List<CsDailyConsumeEventMissionReward>();
	}
}
