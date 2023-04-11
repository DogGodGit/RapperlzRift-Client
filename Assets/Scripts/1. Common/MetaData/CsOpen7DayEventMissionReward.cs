using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-16)
//---------------------------------------------------------------------------------------------------

public class CsOpen7DayEventMissionReward
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
	public CsOpen7DayEventMissionReward(WPDOpen7DayEventMissionReward open7DayEventMissionReward)
	{
		m_nMissionId = open7DayEventMissionReward.missionId;
		m_nRewardNo = open7DayEventMissionReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(open7DayEventMissionReward.itemRewardId);
	}
}
