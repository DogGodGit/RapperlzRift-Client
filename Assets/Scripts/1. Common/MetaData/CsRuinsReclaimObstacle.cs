using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimObstacle
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
	public CsRuinsReclaimObstacle(WPDRuinsReclaimObstacle ruinsReclaimObstacle)
	{
		m_nObstacleId = ruinsReclaimObstacle.obstacleId;
		m_flXPosition = ruinsReclaimObstacle.xPosition;
		m_flYPosition = ruinsReclaimObstacle.yPosition;
		m_flZPosition = ruinsReclaimObstacle.zPosition;
		m_flYRotation = ruinsReclaimObstacle.yRotation;
		m_flScale = ruinsReclaimObstacle.scale;
	}
}
