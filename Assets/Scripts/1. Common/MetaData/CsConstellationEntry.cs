using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsConstellationEntry
{
	int m_nConstellationId;
	int m_nStep;
	int m_nCycle;
	int m_nEntryNo;
	int m_nRequiredStarEssense;
	long m_lRequiredGold;
	int m_nSuccessRate;

	List<CsConstellationEntryBuff> m_listCsConstellationEntryBuff;

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

	public int EntryNo
	{
		get { return m_nEntryNo; }
	}

	public int RequiredStarEssense
	{
		get { return m_nRequiredStarEssense; }
	}

	public long RequiredGold
	{
		get { return m_lRequiredGold; }
	}

	public List<CsConstellationEntryBuff> ConstellationEntryBuffList
	{
		get { return m_listCsConstellationEntryBuff; }
	}

	public int SuccessRate
	{
		get { return m_nSuccessRate; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationEntry(WPDConstellationEntry constellationEntry)
	{
		m_nConstellationId = constellationEntry.constellationId;
		m_nStep = constellationEntry.step;
		m_nCycle = constellationEntry.cycle;
		m_nEntryNo = constellationEntry.entryNo;
		m_nRequiredStarEssense = constellationEntry.requiredStarEssense;
		m_lRequiredGold = constellationEntry.requiredGold;
		m_nSuccessRate = constellationEntry.successRate;

		m_listCsConstellationEntryBuff = new List<CsConstellationEntryBuff>();
	}
}
