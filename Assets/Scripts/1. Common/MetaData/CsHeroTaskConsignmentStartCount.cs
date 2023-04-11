using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroTaskConsignmentStartCount
{
	int m_nConsignmentId;
	int m_nStartCount;

	//---------------------------------------------------------------------------------------------------
	public int ConsignmentId
	{
		get { return m_nConsignmentId; }
	}

	public int StartCount
	{
		get { return m_nStartCount; }
		set { m_nStartCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTaskConsignmentStartCount(PDHeroTaskConsignmentStartCount heroTaskConsignmentStartCount)
	{
		m_nConsignmentId = heroTaskConsignmentStartCount.consignmentId;
		m_nStartCount = heroTaskConsignmentStartCount.startCount;
	}
}
