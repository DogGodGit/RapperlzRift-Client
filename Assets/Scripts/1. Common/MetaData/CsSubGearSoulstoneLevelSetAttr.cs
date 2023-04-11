using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsSubGearSoulstoneLevelSetAttr
{
	int m_nSetNo;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int SetNo
	{
		get { return m_nSetNo; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInfo
	{
		get { return m_csAttrValueInfo; }
	}

	public int BattlePower
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearSoulstoneLevelSetAttr(WPDSubGearSoulstoneLevelSetAttr subGearSoulstoneLevelSetAttr)
	{
		m_nSetNo = subGearSoulstoneLevelSetAttr.setNo;
		m_csAttr = CsGameData.Instance.GetAttr(subGearSoulstoneLevelSetAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(subGearSoulstoneLevelSetAttr.attrValueId);
	}
}
