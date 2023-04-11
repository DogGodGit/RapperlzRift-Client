using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureBaseAttrValue
{
	int m_nCreatureId;
	CsCreatureBaseAttr m_csCreatureBaseAttr;
	int m_nMinAttrValue;
	int m_nMaxAttrValue;
	int m_nIncAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int CreatureId
	{
		get { return m_nCreatureId; }
	}

	public CsCreatureBaseAttr CreatureBaseAttr
	{
		get { return m_csCreatureBaseAttr; }
	}

	public int MinAttrValue
	{
		get { return m_nMinAttrValue; }
	}

	public int MaxAttrValue
	{
		get { return m_nMaxAttrValue; }
	}

	public int IncAttrValue
	{
		get { return m_nIncAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureBaseAttrValue(WPDCreatureBaseAttrValue creatureBaseAttrValue)
	{
		m_nCreatureId = creatureBaseAttrValue.creatureId;
		m_csCreatureBaseAttr = CsGameData.Instance.GetCreatureBaseAttr(creatureBaseAttrValue.attrId);
		m_nMinAttrValue = creatureBaseAttrValue.minAttrValue;
		m_nMaxAttrValue = creatureBaseAttrValue.maxAttrValue;
		m_nIncAttrValue = creatureBaseAttrValue.incAttrValue;
	}
}
