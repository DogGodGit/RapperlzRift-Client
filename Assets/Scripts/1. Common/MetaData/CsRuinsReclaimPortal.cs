using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimPortal
{
	int m_nPortalId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	float m_flExitXPosition;
	float m_flExitYPosition;
	float m_flExitZPosition;
	float m_flExitRadius;
	int m_nExitYRotationType;
	float m_flExitYRotation;

	//---------------------------------------------------------------------------------------------------
	public int PortalId
	{
		get { return m_nPortalId; }
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

	public float ExitXPosition
	{
		get { return m_flExitXPosition; }
	}

	public float ExitYPosition
	{
		get { return m_flExitYPosition; }
	}

	public float ExitZPosition
	{
		get { return m_flExitZPosition; }
	}

	public float ExitRadius
	{
		get { return m_flExitRadius; }
	}

	public int ExitYRotationType
	{
		get { return m_nExitYRotationType; }
	}

	public float ExitYRotation
	{
		get { return m_flExitYRotation; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimPortal(WPDRuinsReclaimPortal ruinsReclaimPortal)
	{
		m_nPortalId = ruinsReclaimPortal.portalId;
		m_flXPosition = ruinsReclaimPortal.xPosition;
		m_flYPosition = ruinsReclaimPortal.yPosition;
		m_flZPosition = ruinsReclaimPortal.zPosition;
		m_flRadius = ruinsReclaimPortal.radius;
		m_flExitXPosition = ruinsReclaimPortal.exitXPosition;
		m_flExitYPosition = ruinsReclaimPortal.exitYPosition;
		m_flExitZPosition = ruinsReclaimPortal.exitZPosition;
		m_flExitRadius = ruinsReclaimPortal.exitRadius;
		m_nExitYRotationType = ruinsReclaimPortal.exitYRotationType;
		m_flExitYRotation = ruinsReclaimPortal.exitYRotation;
	}
}
