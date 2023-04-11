using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsWingStepPart
{
	int m_nStep;
	CsWingPart m_csWingPart;
	CsAttrValueInfo m_csAttrValueInfoIncrease;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public CsWingPart WingPart
	{
		get { return m_csWingPart; }
	}

	public CsAttrValueInfo IncreaseAttrValueInfo
	{
		get { return m_csAttrValueInfoIncrease; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingStepPart(WPDWingStepPart wingStepPart)
	{
		m_nStep = wingStepPart.step;
		m_csWingPart = CsGameData.Instance.GetWingPart(wingStepPart.partId);
		m_csAttrValueInfoIncrease = CsGameData.Instance.GetAttrValueInfo(wingStepPart.increaseAttrValueId);
	}
}
