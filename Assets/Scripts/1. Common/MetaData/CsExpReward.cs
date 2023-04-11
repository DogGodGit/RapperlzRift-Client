using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsExpReward
{
	long m_lExpRewardId;
	long m_lValue;

	//---------------------------------------------------------------------------------------------------
	public long ExpRewardId
	{
		get { return m_lExpRewardId; }
	}

	public long Value
	{
		get { return m_lValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsExpReward(WPDExpReward expReward)
	{
		m_lExpRewardId = expReward.expRewardId;
		m_lValue = expReward.value;
	}
}
