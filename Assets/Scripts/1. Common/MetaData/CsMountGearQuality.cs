using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGearQuality
{
	int m_nQuality;
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public int Quality
	{
		get { return m_nQuality; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearQuality(WPDMountGearQuality mountGearQuality)
	{
		m_nQuality = mountGearQuality.quality;
		m_strName = CsConfiguration.Instance.GetString(mountGearQuality.nameKey);
	}
}
