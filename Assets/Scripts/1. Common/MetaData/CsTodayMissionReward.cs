using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsTodayMissionReward
{
	int m_nMissionId;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
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
	public CsTodayMissionReward(WPDTodayMissionReward todayMissionReward)
	{
		m_nMissionId = todayMissionReward.missionId;
		m_nRewardNo = todayMissionReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(todayMissionReward.itemRewardId);
	}
}
