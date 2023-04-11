using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsPaidImmediateRevival
{
	int m_nRevivalCount;
	int m_nRequiredDia;

	//---------------------------------------------------------------------------------------------------
	public int RevivalCount
	{
		get { return m_nRevivalCount; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsPaidImmediateRevival(WPDPaidImmediateRevival paidImmediateRevival)
	{
		m_nRevivalCount = paidImmediateRevival.revivalCount;
		m_nRequiredDia = paidImmediateRevival.requiredDia;
	}
}
