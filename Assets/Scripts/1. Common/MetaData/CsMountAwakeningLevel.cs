using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsMountAwakeningLevel
{
	int m_nMountId;
	CsMountAwakeningLevelMaster m_csMountAwakeningLevelMaster;
	int m_nNextLevelUpRequiredAwakeningExp;

	//---------------------------------------------------------------------------------------------------
	public int MountId
	{
		get { return m_nMountId; }
	}

	public CsMountAwakeningLevelMaster MountAwakeningLevelMaster
	{
		get { return m_csMountAwakeningLevelMaster; }
	}

	public int NextLevelUpRequiredAwakeningExp
	{
		get { return m_nNextLevelUpRequiredAwakeningExp; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountAwakeningLevel(WPDMountAwakeningLevel mountAwakeningLevel)
	{
		m_nMountId = mountAwakeningLevel.mountId;
		m_csMountAwakeningLevelMaster = CsGameData.Instance.GetMountAwakeningLevelMaster(mountAwakeningLevel.awakeningLevel);
		m_nNextLevelUpRequiredAwakeningExp = mountAwakeningLevel.nextLevelUpRequiredAwakeningExp;
	}
}
