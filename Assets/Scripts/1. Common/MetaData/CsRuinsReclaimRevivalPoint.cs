using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimRevivalPoint
{
	int m_nRevivalPointId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nYRotationType;
	float m_flYRotation;

	//---------------------------------------------------------------------------------------------------
	public int RevivalPointId
	{
		get { return m_nRevivalPointId; }
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
	public CsRuinsReclaimRevivalPoint(WPDRuinsReclaimRevivalPoint ruinsReclaimRevivalPoint)
	{
		m_nRevivalPointId = ruinsReclaimRevivalPoint.revivalPointId;
		m_flXPosition = ruinsReclaimRevivalPoint.xPosition;
		m_flYPosition = ruinsReclaimRevivalPoint.yPosition;
		m_flZPosition = ruinsReclaimRevivalPoint.zPosition;
		m_flRadius = ruinsReclaimRevivalPoint.radius;
		m_nYRotationType = ruinsReclaimRevivalPoint.yRotationType;
		m_flYRotation = ruinsReclaimRevivalPoint.yRotation;
	}
}
