using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarTransmissionExit
{
	int m_nNpcId;
	int m_nExitNo;
	string m_strName;
	CsContinent m_csContinent;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nYRotationType;
	float m_flYRotation;

	//---------------------------------------------------------------------------------------------------
	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public int ExitNo
	{
		get { return m_nExitNo; }
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

	//---------------------------------------------------------------------------------------------------
	public CsNationWarTransmissionExit(WPDNationWarTransmissionExit nationWarTransmissionExit)
	{
		m_nNpcId = nationWarTransmissionExit.npcId;
		m_nExitNo = nationWarTransmissionExit.exitNo;
		m_strName = CsConfiguration.Instance.GetString(nationWarTransmissionExit.nameKey);
		m_csContinent = CsGameData.Instance.GetContinent(nationWarTransmissionExit.continentId);
		m_flXPosition = nationWarTransmissionExit.xPosition;
		m_flYPosition = nationWarTransmissionExit.yPosition;
		m_flZPosition = nationWarTransmissionExit.zPosition;
		m_flRadius = nationWarTransmissionExit.radius;
		m_nYRotationType = nationWarTransmissionExit.yRotationType;
		m_flYRotation = nationWarTransmissionExit.yRotation;
	}
}
