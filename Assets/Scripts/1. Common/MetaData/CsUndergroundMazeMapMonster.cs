using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazeMapMonster
{
	int m_nFloor;
	CsMonsterInfo m_csMonster;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;

	//---------------------------------------------------------------------------------------------------
	public int Floor
	{
		get { return m_nFloor; }
	}

	public CsMonsterInfo Monster
	{
		get { return m_csMonster; }
	}

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeMapMonster(WPDUndergroundMazeMapMonster undergroundMazeMapMonster)
	{
		m_nFloor = undergroundMazeMapMonster.floor;
		m_csMonster = CsGameData.Instance.GetMonsterInfo(undergroundMazeMapMonster.monsterId);
		m_flXPosition = undergroundMazeMapMonster.xPosition;
		m_flYPosition = undergroundMazeMapMonster.yPosition;
		m_flZPosition = undergroundMazeMapMonster.zPosition;
	}
}
