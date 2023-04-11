using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazeEntrance
{
	int m_nFloor;
	int m_nRequiredHeroLevel;

	//---------------------------------------------------------------------------------------------------
	public int Floor
	{
		get { return m_nFloor; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeEntrance(WPDUndergroundMazeEntrance undergroundMazeEntrance)
	{
		m_nFloor = undergroundMazeEntrance.floor;
		m_nRequiredHeroLevel = undergroundMazeEntrance.requiredHeroLevel;
	}
}
