using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-02)
//---------------------------------------------------------------------------------------------------

public class CsNationNoblesseInstance
{
	CsNationNoblesse m_csNationNoblesse;
	Guid m_guidHeroId;
	int m_nJobId;
	CsJob m_csJob;
	string m_strHeroName;
	DateTime m_dtAppointmentDate;

	//---------------------------------------------------------------------------------------------------
	public int NoblesseId
	{
		get { return m_csNationNoblesse.NoblesseId; }
	}

	public CsNationNoblesse NationNoblesse
	{
		get { return m_csNationNoblesse; }
	}

	public Guid HeroId
	{
		get { return m_guidHeroId; }
		set { m_guidHeroId = value; }
	}

	public string HeroName
	{
		get { return m_strHeroName; }
		set { m_strHeroName = value; }
	}

	public int JobId
	{
		get { return m_nJobId; }
		set
		{
			m_nJobId = value;
			m_csJob = CsGameData.Instance.GetJob(m_nJobId);
		}
	}
	public CsJob Job
	{
		get { return m_csJob; }
	}

	public DateTime AppointmentDate
	{
		get { return m_dtAppointmentDate; }
        set { m_dtAppointmentDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesseInstance(PDNationNoblesseInstance nationNoblesseInstance)
	{
		m_csNationNoblesse = CsGameData.Instance.GetNationNoblesse(nationNoblesseInstance.noblesseId);
		m_guidHeroId = nationNoblesseInstance.heroId;
		JobId = nationNoblesseInstance.heroJobId;
		m_strHeroName = nationNoblesseInstance.heroName;
		m_dtAppointmentDate = nationNoblesseInstance.appointmentDate;
	}
}
