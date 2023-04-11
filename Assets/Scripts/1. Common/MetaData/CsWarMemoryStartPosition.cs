using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryStartPosition
{
	int m_nPositionId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nYRotationType;
	float m_flYRotation;

	//---------------------------------------------------------------------------------------------------
	public int PositionId
	{
		get { return m_nPositionId; }
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

	public int YRotationType
	{
		get { return m_nYRotationType; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryStartPosition(WPDWarMemoryStartPosition warMemoryStartPosition)
	{
		m_nPositionId = warMemoryStartPosition.positionId;
		m_flXPosition = warMemoryStartPosition.xPosition;
		m_flYPosition = warMemoryStartPosition.yPosition; 
		m_flZPosition = warMemoryStartPosition.zPosition;
		m_flRadius = warMemoryStartPosition.radius;
		m_nYRotationType = warMemoryStartPosition.yRotationType;
		m_flYRotation = warMemoryStartPosition.yRotation;
	}
}
