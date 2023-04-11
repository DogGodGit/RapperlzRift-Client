using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsNationNoblesseAttr
{
	int m_nNoblesseId;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int NoblesseId
	{
		get { return m_nNoblesseId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInfo
	{
		get { return m_csAttrValueInfo; }
	}

	public long BattlePower
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesseAttr(WPDNationNoblesseAttr nationNoblesseAttr)
	{
		m_nNoblesseId = nationNoblesseAttr.noblesseId;
		m_csAttr = CsGameData.Instance.GetAttr(nationNoblesseAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(nationNoblesseAttr.attrValueId);
	}
}
