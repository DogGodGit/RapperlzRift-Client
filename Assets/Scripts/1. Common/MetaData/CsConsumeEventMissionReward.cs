using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsConsumeEventMissionReward
{
	int m_nEventId;
	int m_nMissionNo;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int MissionNo
	{
		get { return m_nMissionNo; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConsumeEventMissionReward(WPDConsumeEventMissionReward consumeEventMissionReward)
	{
		m_nEventId = consumeEventMissionReward.eventId;
		m_nMissionNo = consumeEventMissionReward.missionNo;
		m_nRewardNo = consumeEventMissionReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(consumeEventMissionReward.itemRewardId);
	}
}
