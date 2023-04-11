using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-24)
//---------------------------------------------------------------------------------------------------

public class CsAccomplishmentPointReward
{
	long m_lAccomplishmentPointRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long AccomplishmentPointRewardId
	{
		get { return m_lAccomplishmentPointRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishmentPointReward(WPDAccomplishmentPointReward accomplishmentPointReward)
	{

	}
}
