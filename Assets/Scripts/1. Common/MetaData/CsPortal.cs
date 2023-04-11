using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsPortal
{
	int m_nPortalId;                        // 포탈ID
	string m_strName;                       // 이름키
	int m_nContinentId;                     // 대륙ID
	Vector3 m_vtPosition;					// 좌표
	float m_flRadius;                       // 반지름
	Vector3 m_vtExitPosition;               // 출구좌표
	float m_flExitRadius;                   // 출구반지름
	int m_nExitYRotationType;               // 출구방향타입(1:고정,2:랜덤)
	float m_flExitYRotation;                // 출구방향
	int m_nLinkedPortalId;                  // 연결된포탈ID

	//---------------------------------------------------------------------------------------------------
	public int PortalId
	{
		get { return m_nPortalId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int ContinentId
	{
		get { return m_nContinentId; }
	}

	public Vector3 Position
	{
		get { return m_vtPosition; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public Vector3 ExitPosition
	{
		get { return m_vtExitPosition; }
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
	public CsPortal(WPDPortal portal)
	{
		m_nPortalId = portal.portalId;
		m_strName = CsConfiguration.Instance.GetString(portal.nameKey);
		m_nContinentId = portal.continentId;
		m_vtPosition = new Vector3((float)portal.xPosition, (float)portal.yPosition, (float)portal.zPosition);
		m_flRadius = portal.radius;
		m_vtExitPosition = new Vector3((float)portal.exitXPosition, (float)portal.exitYPosition, (float)portal.exitZPosition);
		m_flExitRadius = portal.exitRadius;
		m_nExitYRotationType = portal.exitYRotationType;
		m_flExitYRotation = portal.exitYRotation;
		m_nLinkedPortalId = portal.linkedPortalId;
	}
}
