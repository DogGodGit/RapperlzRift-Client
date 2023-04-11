using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsConstellationEntryBuff
{
	int m_nConstellationId;
	int m_nStep;
	int m_nCycle;
	int m_nEntryNo;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

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

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationEntryBuff(WPDConstellationEntryBuff constellationEntryBuff)
	{
		m_nConstellationId = constellationEntryBuff.constellationId;
		m_nStep = constellationEntryBuff.step;
		m_nCycle = constellationEntryBuff.cycle;
		m_nEntryNo = constellationEntryBuff.entryNo;
		m_csAttr = CsGameData.Instance.GetAttr(constellationEntryBuff.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(constellationEntryBuff.attrValueId);
	}
}
