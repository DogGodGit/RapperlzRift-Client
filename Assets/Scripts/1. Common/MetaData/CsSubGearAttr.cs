using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsSubGearAttr
{
	int m_nSubGearId;       // 보조장비ID
	CsAttr m_csAttr;        // 속성ID

	List<CsSubGearAttrValue> m_listCsSubGearAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public List<CsSubGearAttrValue> SubGearAttrValueList
	{
		get { return m_listCsSubGearAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearAttr(WPDSubGearAttr subGearAttr)
	{
		m_nSubGearId = subGearAttr.subGearId;
		m_csAttr = CsGameData.Instance.GetAttr(subGearAttr.attrId);

		m_listCsSubGearAttrValue = new List<CsSubGearAttrValue>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearAttrValue GetSubGearAttrValue(int nLevel, int nQuality)
	{
		for (int i = 0; i < m_listCsSubGearAttrValue.Count; i++)
		{
			if (m_listCsSubGearAttrValue[i].Level == nLevel && m_listCsSubGearAttrValue[i].Quality == nQuality)
				return m_listCsSubGearAttrValue[i];
		}

		return null;
	}
}
