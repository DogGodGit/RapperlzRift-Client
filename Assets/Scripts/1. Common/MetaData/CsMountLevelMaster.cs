using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountLevelMaster
{
	int m_nLevel;
	CsMountQualityMaster m_csMountQualityMaster;
	int m_nQualityLevel;
	int m_nNextLevelUpRequiredSatiety;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public CsMountQualityMaster MountQualityMaster
	{
		get { return m_csMountQualityMaster; }
	}

	public int QualityLevel
	{
		get { return m_nQualityLevel; }
	}

	public int NextLevelUpRequiredSatiety
	{
		get { return m_nNextLevelUpRequiredSatiety; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountLevelMaster(WPDMountLevelMaster mountLevelMaster)
	{
		m_nLevel = mountLevelMaster.level;
		m_csMountQualityMaster = CsGameData.Instance.GetMountQualityMaster(mountLevelMaster.quality);
		m_nQualityLevel = mountLevelMaster.qualityLevel;
		m_nNextLevelUpRequiredSatiety = mountLevelMaster.nextLevelUpRequiredSatiety;
	}
}
