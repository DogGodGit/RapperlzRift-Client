using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazeNpcTransmissionEntry
{
	int m_nNpcId;
	int m_nFloor;

	//---------------------------------------------------------------------------------------------------
	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public int Floor
	{
		get { return m_nFloor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeNpcTransmissionEntry(WPDUndergroundMazeNpcTransmissionEntry undergroundMazeNpcTransmissionEntry)
	{
		m_nNpcId = undergroundMazeNpcTransmissionEntry.npcId;
		m_nFloor = undergroundMazeNpcTransmissionEntry.floor;
	}
}
