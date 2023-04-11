using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarPaidTransmission
{
	int m_nTransmissionCount;
	int m_nRequiredDia;

	//---------------------------------------------------------------------------------------------------
	public int TransmissionCount
	{
		get { return m_nTransmissionCount; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarPaidTransmission(WPDNationWarPaidTransmission nationWarPaidTransmission)
	{
		m_nTransmissionCount = nationWarPaidTransmission.transmissionCount;
		m_nRequiredDia = nationWarPaidTransmission.requiredDia;
	}
}
