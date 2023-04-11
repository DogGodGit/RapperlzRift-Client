using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsHeroSeriesMission
{
	CsSeriesMission m_csSeriesMission;
	int m_nCurrentStep;
	int m_nProgressCount;

	//---------------------------------------------------------------------------------------------------
	public CsSeriesMission SeriesMission
	{
		get { return m_csSeriesMission; }
	}

	public CsSeriesMissionStep SeriesMissionStep
	{
		get { return m_csSeriesMission.GetSeriesMissionStep(m_nCurrentStep); }
	}

	public int CurrentStep
	{
		get { return m_nCurrentStep; }
		set { m_nCurrentStep = value; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSeriesMission(PDHeroSeriesMission heroSeriesMission)
	{
		m_csSeriesMission = CsGameData.Instance.GetSeriesMission(heroSeriesMission.missionId);
		m_nCurrentStep = heroSeriesMission.currentStep;
		m_nProgressCount = heroSeriesMission.progressCount;
	}
}
