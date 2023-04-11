using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public enum EnMainGearCategory
{
	Weapon = 1,
	Armor = 2,
}

public enum EnMainGearIndex
{
	Weapon = 0,
	Armor = 1,
}

public enum EnMainGearSlotIndex
{
	Weapon = 4,
	Armor = 9,
}

public class CsMainGearCategory
{
	int m_nCategoryId;
	string m_strName;
	int m_nSlotIndex;

	//---------------------------------------------------------------------------------------------------
	public int CategoryId
	{
		get { return m_nCategoryId; }
	}

	public EnMainGearCategory EnMainGearCategory
	{
		get { return (EnMainGearCategory)m_nCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearCategory(WPDMainGearCategory mainGearCategory)
	{
		m_nCategoryId = mainGearCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(mainGearCategory.nameKey);
		m_nSlotIndex = mainGearCategory.slotIndex;
	}
}
