using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsTaskConsignmentExpReward
{
	int m_nConsignmentId;
	int m_nLevel;
	CsExpReward m_csExpReward;

	//---------------------------------------------------------------------------------------------------
	public int ConsignmentId
	{
		get { return m_nConsignmentId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTaskConsignmentExpReward(WPDTaskConsignmentExpReward taskConsignmentExpReward)
	{
		m_nConsignmentId = taskConsignmentExpReward.consignmentId;
		m_nLevel = taskConsignmentExpReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(taskConsignmentExpReward.expRewardId);
	}
}
