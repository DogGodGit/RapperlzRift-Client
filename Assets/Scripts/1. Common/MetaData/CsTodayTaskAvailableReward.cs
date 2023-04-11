using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsTodayTaskAvailableReward
{
	int m_nTaskId;
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int TaskId
	{
		get { return m_nTaskId; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTodayTaskAvailableReward(WPDTodayTaskAvailableReward todayTaskAvailableReward)
	{
		m_nTaskId = todayTaskAvailableReward.taskId;
		m_nRewardNo = todayTaskAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(todayTaskAvailableReward.itemId);
	}
}
