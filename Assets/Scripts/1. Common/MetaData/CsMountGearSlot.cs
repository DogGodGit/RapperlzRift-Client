using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGearSlot
{
	int m_nSlotIndex;
	int m_nOpenHeroLevel;

	//---------------------------------------------------------------------------------------------------
	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public int OpenHeroLevel
	{
		get { return m_nOpenHeroLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearSlot(WPDMountGearSlot mountGearSlot)
	{
		m_nSlotIndex = mountGearSlot.slotIndex;
		m_nOpenHeroLevel = mountGearSlot.openHeroLevel;
	}
}
