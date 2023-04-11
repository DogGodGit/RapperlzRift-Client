using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsConstellationCycle
{
	int m_nConstellationId;
	int m_nStep;
	int m_nCycle;

	List<CsConstellationCycleBuff> m_listCsConstellationCycleBuff;
	List<CsConstellationEntry> m_listCsConstellationEntry;

	//---------------------------------------------------------------------------------------------------
	public int ConstellationId
	{
		get { return m_nConstellationId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int Cycle
	{
		get { return m_nCycle; }
	}

	public List<CsConstellationCycleBuff> ConstellationCycleBuffList
	{
		get { return m_listCsConstellationCycleBuff; }
	}

	public List<CsConstellationEntry> ConstellationEntryList
	{
		get { return m_listCsConstellationEntry; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationCycle(WPDConstellationCycle constellationCycle)
	{
		m_nConstellationId = constellationCycle.constellationId;
		m_nStep = constellationCycle.step;
		m_nCycle = constellationCycle.cycle;

		m_listCsConstellationCycleBuff = new List<CsConstellationCycleBuff>();
		m_listCsConstellationEntry = new List<CsConstellationEntry>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationEntry GetConstellationEntry(int nEntryNo)
	{
		for (int i = 0; i < m_listCsConstellationEntry.Count; i++)
		{
			if (m_listCsConstellationEntry[i].EntryNo == nEntryNo)
				return m_listCsConstellationEntry[i];
		}

		return null;
	}
}
