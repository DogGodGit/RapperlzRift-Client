using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShipObstacle
{
	int m_nObstacleId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	float m_flScale;
	int m_nRemoveStepNo;

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

	public int RemoveStepNo
	{
		get { return m_nRemoveStepNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipObstacle(WPDTradeShipObstacle tradeShipObstacle)
	{
		m_nObstacleId = tradeShipObstacle.obstacleId;
		m_flXPosition = tradeShipObstacle.xPosition;
		m_flYPosition = tradeShipObstacle.yPosition;
		m_flZPosition = tradeShipObstacle.zPosition;
		m_flYRotation = tradeShipObstacle.yRotation;
		m_flScale = tradeShipObstacle.scale;
		m_nRemoveStepNo = tradeShipObstacle.removeStepNo;
	}
}
