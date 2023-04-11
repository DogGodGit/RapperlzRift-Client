using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsLocationArea
{
	int m_nLocationId;
	int m_nAreaNo;
	string m_strName;
	bool m_bIsMinimapDisplay;
	int m_nMinimapX;
	int m_nMinimapY;
	string m_strMinimapTextColorCode;

	//---------------------------------------------------------------------------------------------------
	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public int AreaNo
	{
		get { return m_nAreaNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public bool IsMinimapDisplay
	{
		get { return m_bIsMinimapDisplay; }
	}

	public int MinimapX
	{
		get { return m_nMinimapX; }
	}

	public int MinimapY
	{
		get { return m_nMinimapY; }
	}

	public string MinimapTextColorCode
	{
		get { return m_strMinimapTextColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLocationArea(WPDLocationArea locationArea)
	{
		m_nLocationId = locationArea.locationId;
		m_nAreaNo = locationArea.areaNo;
		m_strName = CsConfiguration.Instance.GetString(locationArea.nameKey);
		m_bIsMinimapDisplay = locationArea.isMinimapDisplay;
		m_nMinimapX = locationArea.minimapX;
		m_nMinimapY = locationArea.minimapY;
		m_strMinimapTextColorCode = locationArea.minimapTextColorCode;
	}
}
