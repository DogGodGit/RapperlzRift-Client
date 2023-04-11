using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsNationNoblesseAppointmentAuthority
{
	int m_nNoblesseId;
	int m_nTargetNoblesseId;

	//---------------------------------------------------------------------------------------------------
	public int NoblesseId
	{
		get { return m_nNoblesseId; }
	}

	public int TargetNoblesseId
	{
		get { return m_nTargetNoblesseId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesseAppointmentAuthority(WPDNationNoblesseAppointmentAuthority nationNoblesseAppointmentAuthority)
	{
		m_nNoblesseId = nationNoblesseAppointmentAuthority.noblesseId;
		m_nTargetNoblesseId = nationNoblesseAppointmentAuthority.targetNoblesseId;
	}
}
