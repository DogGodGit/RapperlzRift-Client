using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeAttr
{
	int m_nCostumeId;
	CsAttr m_csAttr;

	//---------------------------------------------------------------------------------------------------
	public int CostumeId
	{
		get { return m_nCostumeId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeAttr(WPDCostumeAttr costumeAttr)
	{
		m_nCostumeId = costumeAttr.costumeId;
		m_csAttr = CsGameData.Instance.GetAttr(costumeAttr.attrId);
	}
}
