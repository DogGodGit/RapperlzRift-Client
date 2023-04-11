using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeDisplay
{
	int m_nCostumeId;
	int m_nJobId;
	string m_strHairPrefabName;
	string m_strFacePrefabName;

	//---------------------------------------------------------------------------------------------------
	public int CostumeId
	{
		get { return m_nCostumeId; }
	}

	public int JobId
	{
		get { return m_nJobId; }
	}

	public string HairPrefabName
	{
		get { return m_strHairPrefabName; }
	}

	public string FacePrefabName
	{
		get { return m_strFacePrefabName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeDisplay(WPDCostumeDisplay costumeDisplay)
	{
		m_nCostumeId = costumeDisplay.costumeId;
		m_nJobId = costumeDisplay.jobId;
		m_strHairPrefabName = costumeDisplay.hairPrefabName;
		m_strFacePrefabName = costumeDisplay.facePrefabName;
	}
}
