using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeCollectionAttr
{
	int m_nCostumeCollectionId;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int CostumeCollectionId
	{
		get { return m_nCostumeCollectionId; }
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
	public CsCostumeCollectionAttr(WPDCostumeCollectionAttr costumeCollectionAttr)
	{
		m_nCostumeCollectionId = costumeCollectionAttr.costumeCollectionId;
		m_csAttr = CsGameData.Instance.GetAttr(costumeCollectionAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(costumeCollectionAttr.attrValueId);
	}
}
