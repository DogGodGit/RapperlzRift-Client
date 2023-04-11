using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicObstacle
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
	public CsAncientRelicObstacle(WPDAncientRelicObstacle ancientRelicObstacle)
	{
		m_nObstacleId = ancientRelicObstacle.obstacleId;
		m_flXPosition = ancientRelicObstacle.xPosition;
		m_flYPosition = ancientRelicObstacle.yPosition;
		m_flZPosition = ancientRelicObstacle.zPosition;
		m_flYRotation = ancientRelicObstacle.yRotation;
		m_flScale = ancientRelicObstacle.scale;
	}
}
