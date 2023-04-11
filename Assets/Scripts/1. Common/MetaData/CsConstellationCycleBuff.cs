using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsConstellationCycleBuff
{
	int m_nConstellationId;
	int m_nStep;
	int m_nCycle;
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

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationCycleBuff(WPDConstellationCycleBuff constellationCycleBuff)
	{
		m_nConstellationId = constellationCycleBuff.constellationId;
		m_nStep = constellationCycleBuff.step;
		m_nCycle = constellationCycleBuff.cycle;
		m_csAttr = CsGameData.Instance.GetAttr(constellationCycleBuff.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(constellationCycleBuff.attrValueId);
	}
}
