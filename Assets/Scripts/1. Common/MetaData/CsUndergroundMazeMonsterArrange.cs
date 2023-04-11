using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazeMonsterArrange
{
	int m_nFloor;
	int m_nArrangeNo;
	CsMonsterArrange m_csMonsterArrange;
	int m_nMonsterCount;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;

	//---------------------------------------------------------------------------------------------------
	public int Floor
	{
		get { return m_nFloor; }
	}

	public int ArrangeNo
	{
		get { return m_nArrangeNo; }
	}

	public CsMonsterArrange MonsterArrange
	{
		get { return m_csMonsterArrange; }
	}

	public int MonsterCount
	{
		get { return m_nMonsterCount; }
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

	public float Radius
	{
		get { return m_flRadius; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeMonsterArrange(WPDUndergroundMazeMonsterArrange undergroundMazeMonsterArrange)
	{
		m_nFloor = undergroundMazeMonsterArrange.floor;
		m_nArrangeNo = undergroundMazeMonsterArrange.arrangeNo;
		m_csMonsterArrange = CsGameData.Instance.GetMonsterArrange(undergroundMazeMonsterArrange.monsterArrangeId);
		m_nMonsterCount = undergroundMazeMonsterArrange.monsterCount;
		m_flXPosition = undergroundMazeMonsterArrange.xPosition;
		m_flYPosition = undergroundMazeMonsterArrange.yPosition;
		m_flZPosition = undergroundMazeMonsterArrange.zPosition;
		m_flRadius = undergroundMazeMonsterArrange.radius;
	}
}
