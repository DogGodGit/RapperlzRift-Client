using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-24)
//---------------------------------------------------------------------------------------------------

public class CsAccomplishmentLevelAttr
{
	int m_nAccomplishmentLevel;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int AccomplishmentLevel
	{
		get { return m_nAccomplishmentLevel; }
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
	public CsAccomplishmentLevelAttr(WPDAccomplishmentLevelAttr accomplishmentLevelAttr)
	{

	}
}
