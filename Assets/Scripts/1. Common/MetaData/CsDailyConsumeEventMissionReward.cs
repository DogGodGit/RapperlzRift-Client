using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsDailyConsumeEventMissionReward
{
	int m_nMissionNo;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
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
	public CsDailyConsumeEventMissionReward(WPDDailyConsumeEventMissionReward dailyConsumeEventMissionReward)
	{
		m_nMissionNo = dailyConsumeEventMissionReward.missionNo;
		m_nRewardNo = dailyConsumeEventMissionReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(dailyConsumeEventMissionReward.itemRewardId);
	}
		
}
