using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureAdditionalAttrValue
{
	CsAttr m_csAttr;
	int m_nInjectionLevel;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public int InjectionLevel
	{
		get { return m_nInjectionLevel; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureAdditionalAttrValue(WPDCreatureAdditionalAttrValue creatureAdditionalAttrValue)
	{
		m_csAttr = CsGameData.Instance.GetAttr(creatureAdditionalAttrValue.attrId);
		m_nInjectionLevel = creatureAdditionalAttrValue.injectionLevel;
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(creatureAdditionalAttrValue.attrValueId);
	}
}
