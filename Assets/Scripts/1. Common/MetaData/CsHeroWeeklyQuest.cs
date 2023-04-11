using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsHeroWeeklyQuest
{
	DateTime m_dtWeekStartDate;
	int m_nRoundNo;
	Guid m_guidRoundId;
	int m_nRoundMissionId;
	CsWeeklyQuestMission m_csWeeklyQuestMission;
	bool m_bIsRoundAccepted;
	int m_nRoundProgressCount;

	//---------------------------------------------------------------------------------------------------
	public DateTime WeekStartDate
	{
		get { return m_dtWeekStartDate; }
	}

	public int RoundNo
	{
		get { return m_nRoundNo; }
		set { m_nRoundNo = value; }
	}

	public Guid RoundId
	{
		get { return m_guidRoundId; }
		set { m_guidRoundId = value; }
	}

	public int RoundMissionId
	{
		get { return m_nRoundMissionId; }
		set
		{
			m_nRoundMissionId = value;
			m_csWeeklyQuestMission = CsGameData.Instance.WeeklyQuest.GetWeeklyQuestMission(m_nRoundMissionId);
		}
	}

	public CsWeeklyQuestMission WeeklyQuestMission
	{
		get { return m_csWeeklyQuestMission; }
	}

	public bool IsRoundAccepted
	{
		get { return m_bIsRoundAccepted; }
		set { m_bIsRoundAccepted = value; }
	}

	public int RoundProgressCount
	{
		get { return m_nRoundProgressCount; }
		set { m_nRoundProgressCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWeeklyQuest(PDHeroWeeklyQuest heroWeeklyQuest)
	{
		m_dtWeekStartDate = heroWeeklyQuest.weekStartDate;
		m_nRoundNo = heroWeeklyQuest.roundNo;
		m_guidRoundId = heroWeeklyQuest.roundId;
		m_nRoundMissionId = heroWeeklyQuest.roundMissionId;
		m_csWeeklyQuestMission = CsGameData.Instance.WeeklyQuest.GetWeeklyQuestMission(heroWeeklyQuest.roundMissionId);
		m_bIsRoundAccepted = heroWeeklyQuest.isRoundAccepted;
		m_nRoundProgressCount = heroWeeklyQuest.roundProgressCount;
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset(int nRoundNo, Guid guildRoundId, int nMissionId)
	{
		m_nRoundNo = nRoundNo;
		m_guidRoundId = guildRoundId;
		m_nRoundMissionId = nMissionId;
		m_csWeeklyQuestMission = CsGameData.Instance.WeeklyQuest.GetWeeklyQuestMission(nMissionId);
		m_bIsRoundAccepted = false;
		m_nRoundProgressCount = 0;
	}
}