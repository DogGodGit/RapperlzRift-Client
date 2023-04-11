using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeonObstacle
{
	int m_nDungeonNo;
	int m_nObstacleId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	float m_flScale;

	//---------------------------------------------------------------------------------------------------
	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public int ObstacleId
	{
		get { return m_nObstacleId; }
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

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public float Scale
	{
		get { return m_flScale; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeonObstacle(WPDStoryDungeonObstacle storyDungeonObstacle)
	{
		m_nDungeonNo = storyDungeonObstacle.dungeonNo;
		m_nObstacleId = storyDungeonObstacle.obstacleId;
		m_flXPosition = storyDungeonObstacle.xPosition;
		m_flYPosition = storyDungeonObstacle.yPosition;
		m_flZPosition = storyDungeonObstacle.zPosition;
		m_flYRotation = storyDungeonObstacle.yRotation;
		m_flScale = storyDungeonObstacle.scale;
	}
}
