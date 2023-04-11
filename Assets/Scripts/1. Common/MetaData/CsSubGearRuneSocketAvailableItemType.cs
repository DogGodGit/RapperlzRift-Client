using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsSubGearRuneSocketAvailableItemType
{
	int m_nSubGearId;
	int m_nSocketIndex;
	int m_nItemType;

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public int SocketIndex
	{
		get { return m_nSocketIndex; }
	}

	public int ItemType
	{
		get { return m_nItemType; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearRuneSocketAvailableItemType(WPDSubGearRuneSocketAvailableItemType subGearRuneSocketAvailableItemType)
	{
		m_nSubGearId = subGearRuneSocketAvailableItemType.subGearId;
		m_nSocketIndex = subGearRuneSocketAvailableItemType.socketIndex;
		m_nItemType = subGearRuneSocketAvailableItemType.itemType;
	}
}
