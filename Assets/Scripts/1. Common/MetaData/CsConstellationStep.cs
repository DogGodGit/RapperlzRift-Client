using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsConstellationStep
{
	int m_nConstellationId;
	int m_nStep;
	int m_nRequiredDia;

	List<CsConstellationCycle> m_listCsConstellationCycle;

	//---------------------------------------------------------------------------------------------------
	public int ConstellationId
	{
		get { return m_nConstellationId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	public List<CsConstellationCycle> ConstellationCycleList
	{
		get { return m_listCsConstellationCycle; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationStep(WPDConstellationStep constellationStep)
	{
		m_nConstellationId = constellationStep.constellationId;
		m_nStep = constellationStep.step;
		m_nRequiredDia = constellationStep.requiredDia;

		m_listCsConstellationCycle = new List<CsConstellationCycle>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationCycle GetConstellationCycle(int nCycle)
	{
		for (int i = 0; i < m_listCsConstellationCycle.Count; i++)
		{
			if (m_listCsConstellationCycle[i].Cycle == nCycle)
				return m_listCsConstellationCycle[i];
		}

		return null;
	}
}
