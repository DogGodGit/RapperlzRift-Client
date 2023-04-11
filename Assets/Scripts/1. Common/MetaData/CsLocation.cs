using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsLocation
{
	int m_nLocationId;              // 위치ID
	float m_flMinimapMagnification; // 미니맵배율
	bool m_bAccelerationEnabled;		

	List<CsLocationArea> m_listCsLocationArea;

	//---------------------------------------------------------------------------------------------------
	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public float MinimapMagnification
	{
		get { return m_flMinimapMagnification; }
	}

	public bool AccelerationEnabled
	{
		get { return m_bAccelerationEnabled; }
	}

	public List<CsLocationArea> LocationAreaList
	{
		get { return m_listCsLocationArea; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLocation(WPDLocation location)
	{
		m_nLocationId = location.locationId;
		m_flMinimapMagnification = location.minimapMagnification;
		m_bAccelerationEnabled = location.accelerationEnabled;

		m_listCsLocationArea = new List<CsLocationArea>();
	}
}
