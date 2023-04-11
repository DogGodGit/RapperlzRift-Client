using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookExplorationStepAttr
{
	int m_nStepNo;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
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
	public CsIllustratedBookExplorationStepAttr(WPDIllustratedBookExplorationStepAttr illustratedBookExplorationStepAttr)
	{
		m_nStepNo = illustratedBookExplorationStepAttr.stepNo;
		m_csAttr = CsGameData.Instance.GetAttr(illustratedBookExplorationStepAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(illustratedBookExplorationStepAttr.attrValueId);
	}
}
