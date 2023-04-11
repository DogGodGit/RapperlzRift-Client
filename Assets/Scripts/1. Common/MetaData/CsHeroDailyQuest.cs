using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsHeroDailyQuest
{
	Guid m_guidId;
	int m_nSlotIndex;
	CsDailyQuestMission m_csDailyQuestMission;
	bool m_bIsAccepted;
	bool m_bMissionImmediateCompleted;
	int m_nProgressCount;
	float m_flAutoCompletionRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public CsDailyQuestMission DailyQuestMission
	{
		get { return m_csDailyQuestMission; }
	}

	public bool IsAccepted
	{
		get { return m_bIsAccepted; }
		set { m_bIsAccepted = value; }
	}

	public bool MissionImmediateCompleted
	{
		get { return m_bMissionImmediateCompleted; }
		set { m_bMissionImmediateCompleted = value; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	public float AutoCompletionRemainingTime
	{
		get { return m_flAutoCompletionRemainingTime; }
		set { m_flAutoCompletionRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public bool Completed
	{
		get
		{
			if (m_bMissionImmediateCompleted || (m_nProgressCount >= m_csDailyQuestMission.TargetCount) || (m_flAutoCompletionRemainingTime < Time.realtimeSinceStartup))
				return true;
			else
				return false;
		}
	}

    //---------------------------------------------------------------------------------------------------
    public CsHeroDailyQuest()
    {
        m_guidId = Guid.Empty;
        m_nSlotIndex = 0;
        m_csDailyQuestMission = null;
        m_bIsAccepted = false;
        m_bMissionImmediateCompleted = false;
        m_nProgressCount = 0;
        m_flAutoCompletionRemainingTime = 0f;
    }

	//---------------------------------------------------------------------------------------------------
	public CsHeroDailyQuest(PDHeroDailyQuest heroDailyQuest)
	{
		m_guidId = heroDailyQuest.id;
		m_nSlotIndex = heroDailyQuest.slotIndex;
		m_csDailyQuestMission = CsGameData.Instance.DailyQuest.GetDailyQuestMission(heroDailyQuest.missionId);
		m_bIsAccepted = heroDailyQuest.isAccepted;
		m_bMissionImmediateCompleted = heroDailyQuest.missionImmediateCompleted;
		m_nProgressCount = heroDailyQuest.progressCount;
		m_flAutoCompletionRemainingTime = heroDailyQuest.autoCompletionRemainingTime + Time.realtimeSinceStartup;
	}
}
