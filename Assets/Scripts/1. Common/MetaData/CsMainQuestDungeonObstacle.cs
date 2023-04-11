using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-04)
//---------------------------------------------------------------------------------------------------

public class CsMainQuestDungeonObstacle
{
	int m_nDungeonId;
	int m_nObstacleId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	float m_flScale;

	//---------------------------------------------------------------------------------------------------
	public int DungeonId
	{
		get { return m_nDungeonId; }
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
	public CsMainQuestDungeonObstacle(WPDMainQuestDungeonObstacle mainQuestDungeonObstacle)
	{
		m_nDungeonId = mainQuestDungeonObstacle.dungeonId;
		m_nObstacleId = mainQuestDungeonObstacle.obstacleId;
		m_flXPosition = mainQuestDungeonObstacle.xPosition;
		m_flYPosition = mainQuestDungeonObstacle.yPosition;
		m_flZPosition = mainQuestDungeonObstacle.zPosition;
		m_flYRotation = mainQuestDungeonObstacle.yRotation;
		m_flScale = mainQuestDungeonObstacle.scale;
	}

}
