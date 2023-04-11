using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsDragonNestObstacle
{
	int m_nObstacleId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	float m_flScale;

	//---------------------------------------------------------------------------------------------------
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
	public CsDragonNestObstacle(WPDDragonNestObstacle dragonNestObstacle)
	{
		m_nObstacleId = dragonNestObstacle.obstacleId;
		m_flXPosition = dragonNestObstacle.xPosition;
		m_flYPosition = dragonNestObstacle.yPosition;
		m_flZPosition = dragonNestObstacle.zPosition;
		m_flYRotation = dragonNestObstacle.yRotation;
		m_flScale = dragonNestObstacle.scale;
	}
}
