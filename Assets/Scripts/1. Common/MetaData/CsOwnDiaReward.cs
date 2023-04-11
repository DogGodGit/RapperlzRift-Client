using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsOwnDiaReward
{
	long m_lOwnDiaRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long OwnDiaRewardId
	{
		get { return m_lOwnDiaRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOwnDiaReward(WPDOwnDiaReward ownDiaReward)
	{
		m_lOwnDiaRewardId = ownDiaReward.ownDiaRewardId;
		m_nValue = ownDiaReward.value;
	}
}
