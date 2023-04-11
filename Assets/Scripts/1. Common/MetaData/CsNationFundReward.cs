using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsNationFundReward
{
	long m_lNationFundRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long NationFundRewardId
	{
		get { return m_lNationFundRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationFundReward(WPDNationFundReward nationFundReward)
	{
		m_lNationFundRewardId = nationFundReward.nationFundRewardId;
		m_nValue = nationFundReward.value;
	}
}
