using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeEnchantLevelAttr
{
	int m_nCostumeId;
	int m_nEnchantLevel;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int CostumeId
	{
		get { return m_nCostumeId; }
	}

	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
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
	public CsCostumeEnchantLevelAttr(WPDCostumeEnchantLevelAttr costumeEnchantLevelAttr)
	{
		m_nCostumeId = costumeEnchantLevelAttr.costumeId;
		m_nEnchantLevel = costumeEnchantLevelAttr.enchantLevel;
		m_csAttr = CsGameData.Instance.GetAttr(costumeEnchantLevelAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(costumeEnchantLevelAttr.attrValueId);
	}
}
