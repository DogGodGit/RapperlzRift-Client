using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGearType
{
	int m_nType;
	string m_strName;
	int m_nSlotIndex;
	CsMountGearSlot m_csMountGearSlot;

	//---------------------------------------------------------------------------------------------------
	public int Type
	{
		get { return m_nType; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public CsMountGearSlot MountGearSlot
	{
		get { return m_csMountGearSlot; }
		set { m_csMountGearSlot = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearType(WPDMountGearType mountGearType)
	{
		m_nType = mountGearType.type;
		m_strName = CsConfiguration.Instance.GetString(mountGearType.nameKey);
		m_nSlotIndex = mountGearType.slotIndex;
	}
}
