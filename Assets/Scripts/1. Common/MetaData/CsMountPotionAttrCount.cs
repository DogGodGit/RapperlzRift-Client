using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsMountPotionAttrCount
{
	int m_nCount;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int Count
	{
		get { return m_nCount; }
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
	public CsMountPotionAttrCount(WPDMountPotionAttrCount mountPotionAttrCount)
	{
		m_nCount = mountPotionAttrCount.count;
		m_csAttr = CsGameData.Instance.GetAttr(mountPotionAttrCount.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(mountPotionAttrCount.attrValueId);
	}
}
