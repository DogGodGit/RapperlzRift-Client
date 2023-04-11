using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsDropObjectMainGear : CsDropObject
{
	CsMainGear m_csMainGear;
	bool m_bOwned;
	int m_nEnchantLevel;

	//---------------------------------------------------------------------------------------------------
	public CsMainGear MainGear
	{
		get { return m_csMainGear; }
	}

	public bool Owned
	{
		get { return m_bOwned; }
	}

	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDropObjectMainGear(PDMainGearDropObject mainGearDropObject)
		: base(mainGearDropObject.type)
	{
		m_csMainGear = CsGameData.Instance.GetMainGear(mainGearDropObject.id);
		m_bOwned = mainGearDropObject.owned;
		m_nEnchantLevel = mainGearDropObject.enchantLevel;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDropObjectMainGear(int nType, int nMainGearId, bool bOwned, int nEnchantLevel)
		: base(nType)
	{
		m_csMainGear = CsGameData.Instance.GetMainGear(nMainGearId);
		m_bOwned = bOwned;
		m_nEnchantLevel = nEnchantLevel;
	}
}
