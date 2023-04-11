using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-22)
//---------------------------------------------------------------------------------------------------

public class CsMoneyBuffAttr
{
	int m_nBuffId;
	CsAttr m_csAttr;
	float m_flAttrFactor;

	//---------------------------------------------------------------------------------------------------
	public int BuffId
	{
		get { return m_nBuffId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public float AttrFactor
	{
		get { return m_flAttrFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMoneyBuffAttr(WPDMoneyBuffAttr moneyBuffAttr)
	{
		m_nBuffId = moneyBuffAttr.buffId;
		m_csAttr = CsGameData.Instance.GetAttr(moneyBuffAttr.attrId);
		m_flAttrFactor = moneyBuffAttr.attrFactor;
	}
}
