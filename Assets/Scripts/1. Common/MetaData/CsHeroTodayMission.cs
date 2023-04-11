using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsHeroTodayMission
{
	CsTodayMission m_csTodayMission;
	int m_nProgressCount;
	bool m_bRewardReceived;
	DateTime m_dtDate;

	//---------------------------------------------------------------------------------------------------
	public CsTodayMission TodayMission
	{
		get { return m_csTodayMission; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	public bool RewardReceived
	{
		get { return m_bRewardReceived; }
		set { m_bRewardReceived = value; }
	}

	public DateTime Date
	{
		get { return m_dtDate; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTodayMission(PDHeroTodayMission heroTodayMission, DateTime dtDate)
	{
		m_csTodayMission = CsGameData.Instance.GetTodayMission(heroTodayMission.missionId);
		m_nProgressCount = heroTodayMission.progressCount;
		m_bRewardReceived = heroTodayMission.rewardReceived;
		m_dtDate = dtDate;
	}
}
