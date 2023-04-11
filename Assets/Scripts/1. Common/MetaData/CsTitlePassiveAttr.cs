using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsTitlePassiveAttr
{
	int m_nTitleId;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int TitleId
	{
		get { return m_nTitleId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	public long BattlePower
	{
		get { return m_csAttr.BattlePowerFactor * m_csAttrValue.Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitlePassiveAttr(WPDTitlePassiveAttr titlePassiveAttr)
	{
		m_nTitleId = titlePassiveAttr.titleId;
		m_csAttr = CsGameData.Instance.GetAttr(titlePassiveAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(titlePassiveAttr.attrValueId);
	}
}
