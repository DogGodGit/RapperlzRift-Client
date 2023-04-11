using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookAttr
{
	int m_nIllustratedBookId;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;
	CsIllustratedBookAttrGrade m_csIllustratedBookAttrGrade;

	//---------------------------------------------------------------------------------------------------
	public int IllustratedBookId
	{
		get { return m_nIllustratedBookId; }
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

	public CsIllustratedBookAttrGrade IllustratedBookAttrGrade
	{
		get { return m_csIllustratedBookAttrGrade; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookAttr(WPDIllustratedBookAttr illustratedBookAttr)
	{
		m_nIllustratedBookId = illustratedBookAttr.illustratedBookId;
		m_csAttr = CsGameData.Instance.GetAttr(illustratedBookAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(illustratedBookAttr.attrValueId);
		m_csIllustratedBookAttrGrade = CsGameData.Instance.GetIllustratedBookAttrGrade(illustratedBookAttr.grade);
	}
}
