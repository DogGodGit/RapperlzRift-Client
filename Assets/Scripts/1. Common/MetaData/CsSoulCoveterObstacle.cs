using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-10)
//---------------------------------------------------------------------------------------------------

public class CsSoulCoveterObstacle
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
	public CsSoulCoveterObstacle(WPDSoulCoveterObstacle soulCoveterObstacle)
	{
		m_nObstacleId = soulCoveterObstacle.obstacleId;
		m_flXPosition = soulCoveterObstacle.xPosition;
		m_flYPosition = soulCoveterObstacle.yPosition;
		m_flZPosition = soulCoveterObstacle.zPosition;
		m_flYRotation = soulCoveterObstacle.yRotation;
		m_flScale = soulCoveterObstacle.scale;
	}
}
