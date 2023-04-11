using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsGoldReward
{
	long m_lGoldRewardId;
	long m_lValue;

	//---------------------------------------------------------------------------------------------------
	public long GoldRewardId
	{
		get { return m_lGoldRewardId; }
	}

	public long Value
	{
		get { return m_lValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldReward(WPDGoldReward goldReward)
	{
		m_lGoldRewardId = goldReward.goldRewardId;
		m_lValue = goldReward.value;
	}
}
