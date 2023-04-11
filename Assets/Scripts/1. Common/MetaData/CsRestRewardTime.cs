using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-20)
//---------------------------------------------------------------------------------------------------

public class CsRestRewardTime
{
	int m_nRestTime;
	long m_lRequiredGold;
	int m_nRequiredDia;

	//---------------------------------------------------------------------------------------------------
	public int RestTime
	{
		get { return m_nRestTime; }
	}

	public long RequiredGold
	{
		get { return m_lRequiredGold; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRestRewardTime(WPDRestRewardTime restRewardTime)
	{
		m_nRestTime = restRewardTime.restTime;
		m_lRequiredGold = restRewardTime.requiredGold;
		m_nRequiredDia = restRewardTime.requiredDia;
	}
}
