using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsHeroConstellationStep
{
	int m_nStep;
	int m_nCycle;
	int m_nEntryNo;
	int m_nFailPoint;
	bool m_bActivated;

	CsConstellation m_csConstellation;
	CsConstellationStep m_csConstellationStep;
	CsConstellationCycle m_csConstellationCycle;
	CsConstellationEntry m_csConstellationEntry;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public int Cycle
	{
		get { return m_nCycle; }
		set
		{
			m_nCycle = value;
			m_csConstellationCycle = m_csConstellationStep.GetConstellationCycle(m_nCycle);
		}
	}

	public int EntryNo
	{
		get { return m_nEntryNo; }
		set
		{
			m_nEntryNo = value;
			m_csConstellationEntry = m_csConstellationCycle.GetConstellationEntry(m_nEntryNo);
		}
	}

	public int FailPoint
	{
		get { return m_nFailPoint; }
		set { m_nFailPoint = value; }
	}

	public bool Activated
	{
		get { return m_bActivated; }
		set { m_bActivated = value; }
	}

	public CsConstellation Constellation
	{
		get { return m_csConstellation; }
	}

	public CsConstellationStep ConstellationStep
	{
		get { return m_csConstellationStep; }
	}

	public CsConstellationCycle ConstellationCycle
	{
		get { return m_csConstellationCycle; }
	}

	public CsConstellationEntry ConstellationEntry
	{
		get { return m_csConstellationEntry; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroConstellationStep(int nConstellationId, PDHeroConstellationStep heroConstellationStep)
	{
		m_nStep = heroConstellationStep.step;
		m_nCycle = heroConstellationStep.cycle;
		m_nEntryNo = heroConstellationStep.entryNo;
		m_nFailPoint = heroConstellationStep.failPoint;
		m_bActivated = heroConstellationStep.activated;

		m_csConstellation = CsGameData.Instance.GetConstellation(nConstellationId);
		m_csConstellationStep = m_csConstellation.GetConstellationStep(m_nStep);
		m_csConstellationCycle = m_csConstellationStep.GetConstellationCycle(m_nCycle);
		m_csConstellationEntry = m_csConstellationCycle.GetConstellationEntry(m_nEntryNo);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroConstellationStep(int nConstellationId, int nStep)
	{
		m_nStep = nStep;
		m_nCycle = 1;
		m_nEntryNo = 1;
		m_nFailPoint = 0;
		m_bActivated = false;

		m_csConstellation = CsGameData.Instance.GetConstellation(nConstellationId);
		m_csConstellationStep = m_csConstellation.GetConstellationStep(m_nStep);
		m_csConstellationCycle = m_csConstellationStep.GetConstellationCycle(m_nCycle);
		m_csConstellationEntry = m_csConstellationCycle.GetConstellationEntry(m_nEntryNo);
	}
}
