using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureAdditionalAttr
{
	CsAttr m_csAttr;
	List<CsCreatureAdditionalAttrValue> m_listCsCreatureAdditionalAttrValue;

	//---------------------------------------------------------------------------------------------------
	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public List<CsCreatureAdditionalAttrValue> CreatureAdditionalAttrValueList
	{
		get { return m_listCsCreatureAdditionalAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureAdditionalAttr(WPDCreatureAdditionalAttr creatureAdditionalAttr)
	{
		m_csAttr = CsGameData.Instance.GetAttr(creatureAdditionalAttr.attrId);

		m_listCsCreatureAdditionalAttrValue = new List<CsCreatureAdditionalAttrValue>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureAdditionalAttrValue GetCreatureAdditionalAttrValue(int nInjectionLevel)
	{
		for (int i = 0; i < m_listCsCreatureAdditionalAttrValue.Count; i++)
		{
			if (m_listCsCreatureAdditionalAttrValue[i].InjectionLevel == nInjectionLevel)
				return m_listCsCreatureAdditionalAttrValue[i];
		}

		return null;
	}
}
