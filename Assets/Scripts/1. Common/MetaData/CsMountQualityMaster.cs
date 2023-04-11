using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountQualityMaster
{
	int m_nQuality;
	string m_strName;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public int Quality
	{
		get { return m_nQuality; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountQualityMaster(WPDMountQualityMaster mountQualityMaster)
	{
		m_nQuality = mountQualityMaster.quality;
		m_strName = CsConfiguration.Instance.GetString(mountQualityMaster.nameKey);
		m_strColorCode = mountQualityMaster.colorCode;
	}

}
