using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsDailyChargeEventMission
{
	int m_nMissionNo;
	int m_nRequiredUnOwnDia;

	List<CsDailyChargeEventMissionReward> m_listCsDailyChargeEventMissionReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionNo
	{
		get { return m_nMissionNo; }
	}

	public int RequiredUnOwnDia
	{
		get { return m_nRequiredUnOwnDia; }
	}

	public List<CsDailyChargeEventMissionReward> DailyChargeEventMissionRewardList
	{
		get { return m_listCsDailyChargeEventMissionReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyChargeEventMission(WPDDailyChargeEventMission dailyChargeEventMission)
	{
		m_nMissionNo = dailyChargeEventMission.missionNo;
		m_nRequiredUnOwnDia = dailyChargeEventMission.requiredUnOwnDia;

		m_listCsDailyChargeEventMissionReward = new List<CsDailyChargeEventMissionReward>();
	}
}
