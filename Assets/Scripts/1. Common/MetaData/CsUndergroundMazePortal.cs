using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazePortal
{
	int m_nPortalId;
	int m_nFloor;
	string m_strName;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	float m_flYRotation;
	float m_flExitXPosition;
	float m_flExitYPosition;
	float m_flExitZPosition;
	float m_flExitRadius;
	int m_nExitYRotationType;
	float m_flExitYRotation;
	int m_nLinkedPortalId;

	//---------------------------------------------------------------------------------------------------
	public int PortalId
	{
		get { return m_nPortalId; }
	}

	public int Floor
	{
		get { return m_nFloor; }
	}

	public string Name
	{
		get { return m_strName; }
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

	public float YRotation
	{
		get { return m_flYRotation; }
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

	public int LinkedPortalId
	{
		get { return m_nLinkedPortalId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazePortal(WPDUndergroundMazePortal undergroundMazePortal)
	{
		m_nPortalId = undergroundMazePortal.portalId;
		m_nFloor = undergroundMazePortal.floor;
		m_strName = CsConfiguration.Instance.GetString(undergroundMazePortal.nameKey);
		m_flXPosition = undergroundMazePortal.xPosition;
		m_flYPosition = undergroundMazePortal.yPosition;
		m_flZPosition = undergroundMazePortal.zPosition;
		m_flRadius = undergroundMazePortal.radius;
		m_flYRotation = undergroundMazePortal.yRotation;
		m_flExitXPosition = undergroundMazePortal.exitXPosition;
		m_flExitYPosition = undergroundMazePortal.exitYPosition;
		m_flExitZPosition = undergroundMazePortal.exitZPosition;
		m_flExitRadius = undergroundMazePortal.exitRadius;
		m_nExitYRotationType = undergroundMazePortal.exitYRotationType;
		m_flExitYRotation = undergroundMazePortal.exitYRotation;
		m_nLinkedPortalId = undergroundMazePortal.linkedPortalId;
	}
}
