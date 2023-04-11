using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-14)
//---------------------------------------------------------------------------------------------------

public class CsHonorPointReward
{
	long m_lHonorPointRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long HonorPointRewardId
	{
		get { return m_lHonorPointRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHonorPointReward(WPDHonorPointReward honorPointReward)
	{
		m_lHonorPointRewardId = honorPointReward.honorPointRewardId;
		m_nValue = honorPointReward.value;
	}
}
