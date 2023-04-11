using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarRevivalPoint
{
	int m_nRevivalPointId;
	string m_strName;
	CsContinent m_csContinent;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nYRotationType;
	float m_flYRotation;
	int m_nPriority;

	//---------------------------------------------------------------------------------------------------
	public int RevivalPointId
	{
		get { return m_nRevivalPointId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
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

	public int Priority
	{
		get { return m_nPriority; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarRevivalPoint(WPDNationWarRevivalPoint nationWarRevivalPoint)
	{
		m_nRevivalPointId = nationWarRevivalPoint.revivalPointId;
		m_strName = CsConfiguration.Instance.GetString(nationWarRevivalPoint.nameKey);
		m_csContinent = CsGameData.Instance.GetContinent(nationWarRevivalPoint.continentId);
		m_flXPosition = nationWarRevivalPoint.xPosition;
		m_flYPosition = nationWarRevivalPoint.yPosition;
		m_flZPosition = nationWarRevivalPoint.zPosition;
		m_flRadius = nationWarRevivalPoint.radius;
		m_nYRotationType = nationWarRevivalPoint.yRotationType;
		m_flYRotation = nationWarRevivalPoint.yRotation;
		m_nPriority = nationWarRevivalPoint.priority;
	}
}
