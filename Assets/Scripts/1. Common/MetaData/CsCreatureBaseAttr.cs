using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureBaseAttr
{
	CsAttr m_csAttr;

	//---------------------------------------------------------------------------------------------------
	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureBaseAttr(WPDCreatureBaseAttr creatureBaseAttr)
	{
		m_csAttr = CsGameData.Instance.GetAttr(creatureBaseAttr.attrId);
	}
}
