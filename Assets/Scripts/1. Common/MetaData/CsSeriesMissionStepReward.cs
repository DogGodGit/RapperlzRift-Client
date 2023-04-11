using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsSeriesMissionStepReward
{
	int m_nMissionId;
	int m_nStep;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public int Step
	{
		get { return m_nStep; }
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
	public CsSeriesMissionStepReward(WPDSeriesMissionStepReward seriesMissionStepReward)
	{
		m_nMissionId = seriesMissionStepReward.missionId;
		m_nStep = seriesMissionStepReward.step;
		m_nRewardNo = seriesMissionStepReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(seriesMissionStepReward.itemRewardId);
	}
}
