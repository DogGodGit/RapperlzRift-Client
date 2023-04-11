using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsMainGearBaseAttrEnchantLevel
{
	int m_nMainGearId;          // 메인장비ID
	int m_nAttrId;              // 속성ID
	int m_nEnchantLevel;        // 강화레벨
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int MainGearId
	{
		get { return m_nMainGearId; }
	}

	public int AttrId
	{
		get { return m_nAttrId; }
	}

	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
	}

	public int Value
	{
		get { return m_csAttrValueInfo.Value; }
	}

	public long AttrValueId
	{
		get { return m_csAttrValueInfo.AttrValueId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearBaseAttrEnchantLevel(WPDMainGearBaseAttrEnchantLevel mainGearBaseAttrEnchantLevel)
	{
		m_nMainGearId = mainGearBaseAttrEnchantLevel.mainGearId;
		m_nAttrId = mainGearBaseAttrEnchantLevel.attrId;
		m_nEnchantLevel = mainGearBaseAttrEnchantLevel.enchantLevel;
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(mainGearBaseAttrEnchantLevel.attrValueId);
	}

}
