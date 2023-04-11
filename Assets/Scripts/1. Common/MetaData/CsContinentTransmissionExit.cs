using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsContinentTransmissionExit
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
	public CsContinentTransmissionExit(WPDContinentTransmissionExit continentTransmissionExit)
	{
		m_nNpcId = continentTransmissionExit.npcId;
		m_nExitNo = continentTransmissionExit.exitNo;
		m_strName = CsConfiguration.Instance.GetString(continentTransmissionExit.nameKey);
		m_csContinent = CsGameData.Instance.GetContinent(continentTransmissionExit.continentId);
		m_flXPosition = continentTransmissionExit.xPosition;
		m_flYPosition = continentTransmissionExit.yPosition;
		m_flZPosition = continentTransmissionExit.zPosition;
		m_flRadius = continentTransmissionExit.radius;
		m_nYRotationType = continentTransmissionExit.yRotationType;
		m_flYRotation = continentTransmissionExit.yRotation;
	}
}
