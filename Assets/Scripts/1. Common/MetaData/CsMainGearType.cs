using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

/*
1 : 검
2 : 창
3 : 법장
4 : 궁
5 : 갑옷
*/

public enum EnMainGearType
{
	Sword = 1,
	Spear = 2,
	Wand = 3,
	Bow = 4,
	Armor = 5,
}


public class CsMainGearType
{
	int m_nMainGearType;						// 메인장비타입

	CsMainGearCategory m_csMainGearCategory;	// 
	string m_strName;							// 

	//---------------------------------------------------------------------------------------------------
	public int MainGearType
	{
		get { return m_nMainGearType; }
	}

	public EnMainGearType EnMainGearType
	{
		get { return (EnMainGearType)m_nMainGearType; }
	}

	public int SlotIndex
	{
		get { return m_csMainGearCategory.SlotIndex; }
	}

	public int EquippedIndex
	{
		get
		{
			if (m_csMainGearCategory.SlotIndex == (int)EnMainGearSlotIndex.Weapon)
			{
				return (int)EnMainGearIndex.Weapon;
			}
			else
			{
				return (int)EnMainGearIndex.Armor;
			}
		}
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsMainGearCategory MainGearCategory
	{
		get { return m_csMainGearCategory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearType(WPDMainGearType mainGearType)
	{
		m_nMainGearType = mainGearType.mainGearType;
		m_csMainGearCategory = CsGameData.Instance.GetMainGearCategory(mainGearType.categoryId);
		m_strName = CsConfiguration.Instance.GetString(mainGearType.nameKey);


	}
}
