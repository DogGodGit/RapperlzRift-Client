using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsWingPart
{
	int m_nPartId;
	CsAttr m_csAttr;

	//---------------------------------------------------------------------------------------------------
	public int PartId
	{
		get { return m_nPartId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingPart(WPDWingPart wingPart)
	{
		m_nPartId = wingPart.partId;
		m_csAttr = CsGameData.Instance.GetAttr(wingPart.attrId);
	}
}
