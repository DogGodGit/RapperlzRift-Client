using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsPotionAttr
{
	int m_nPotionAttrId;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInc;
	CsItem m_csItemRequired;

	//---------------------------------------------------------------------------------------------------
	public int PotionAttrId
	{
		get { return m_nPotionAttrId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInc
	{
		get { return m_csAttrValueInc; }
	}

	public CsItem ItemRequired
	{
		get { return m_csItemRequired; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsPotionAttr(WPDPotionAttr potionAttr)
	{
		m_nPotionAttrId = potionAttr.potionAttrId;
		m_csAttr = CsGameData.Instance.GetAttr(potionAttr.attrId);
		m_csAttrValueInc = CsGameData.Instance.GetAttrValueInfo(potionAttr.incAttrValueId);
		m_csItemRequired = CsGameData.Instance.GetItem(potionAttr.requiredItemId);
	}
}
