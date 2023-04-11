using UnityEngine;

public class CsPortalArea : CsBaseArea
{
	enum EnPortalType { Continent = 0, UndergroundMaze = 1, RuinsReclaim = 2 }

	CsPortal m_csPortal;
	CsUndergroundMazePortal m_csUndergroundMazePortal;
	CsRuinsReclaimPortal m_csRuinsReclaimPortal;

	bool m_bSendPortalEnter = false;
	EnPortalType m_enPortalType = EnPortalType.Continent;
	CsSimpleTimer m_csSimpleTimer = new CsSimpleTimer();

	//---------------------------------------------------------------------------------------------------
	public void Init(CsPortal csPortal)
	{
		m_enPortalType = EnPortalType.Continent;
		m_csPortal = csPortal;
		transform.name = m_csPortal.PortalId.ToString();
		transform.position = m_csPortal.Position;

		SphereCollider sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.radius = m_csPortal.Radius - 0.5f;
		sphereCollider.enabled = true;
		sphereCollider.isTrigger = true;
		m_csSimpleTimer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(CsUndergroundMazePortal csUndergroundMazePortal)
	{
		m_enPortalType = EnPortalType.UndergroundMaze;
		m_csUndergroundMazePortal = csUndergroundMazePortal;
		transform.name = csUndergroundMazePortal.PortalId.ToString();
		transform.position = new Vector3(csUndergroundMazePortal.XPosition, csUndergroundMazePortal.YPosition, csUndergroundMazePortal.ZPosition);
		transform.eulerAngles = new Vector3(0f, csUndergroundMazePortal.YRotation, 0f);

		SphereCollider sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.radius = csUndergroundMazePortal.Radius - 0.5f;
		sphereCollider.enabled = true;
		sphereCollider.isTrigger = true;
		m_csSimpleTimer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(CsRuinsReclaimPortal csRuinsReclaimPortal)
	{
		m_enPortalType = EnPortalType.RuinsReclaim;
		m_csRuinsReclaimPortal = csRuinsReclaimPortal;
		transform.name = m_csRuinsReclaimPortal.PortalId.ToString();
		transform.position = new Vector3(m_csRuinsReclaimPortal.XPosition, m_csRuinsReclaimPortal.YPosition, m_csRuinsReclaimPortal.ZPosition);

		SphereCollider sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.radius = m_csRuinsReclaimPortal.Radius - 0.5f;
		sphereCollider.enabled = true;
		sphereCollider.isTrigger = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		m_csPortal = null;
		m_csUndergroundMazePortal = null;
		m_csRuinsReclaimPortal = null;
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		m_bSendPortalEnter = true;
		EnterPortal();
	}

	//---------------------------------------------------------------------------------------------------
	public override void StayAction()
	{
		if (m_csSimpleTimer.CheckSetTimer())
		{
			if (m_bSendPortalEnter == false)
			{
				m_bSendPortalEnter = true;
				EnterPortal();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void EnterPortal()
	{
		int nPortalId = 0;
		switch (m_enPortalType)
		{
		case EnPortalType.Continent:
			nPortalId = m_csPortal.PortalId;
			if (CsCartManager.Instance.IsMyHeroRidingCart) // 카트 탑승중.
			{
				CsCartManager.Instance.CartPortalEnter(m_csPortal.PortalId);
			}
			else
			{
				CsGameEventToUI.Instance.OnEventPortalEnter(m_csPortal.PortalId);
			}
			break;
		case EnPortalType.UndergroundMaze:
			nPortalId = m_csUndergroundMazePortal.PortalId;
			CsDungeonManager.Instance.UndergroundMazePortalEnter(m_csUndergroundMazePortal.PortalId);
			break;
		case EnPortalType.RuinsReclaim:
			nPortalId = m_csRuinsReclaimPortal.PortalId;
			CsDungeonManager.Instance.RuinsReclaimPortalEnter(m_csRuinsReclaimPortal.PortalId);
			break;
		}
		CsIngameData.Instance.IngameManagement.PortalAreaEnter(nPortalId);
	}
}
