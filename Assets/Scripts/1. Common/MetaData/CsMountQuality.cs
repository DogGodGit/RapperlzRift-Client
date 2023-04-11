using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountQuality
{
	int m_nMountId;
	CsMountQualityMaster m_csMountQualityMaster;
	string m_strPrefabName;
	int m_nPotionAttrMaxCount;

	//---------------------------------------------------------------------------------------------------
	public int MountId
	{
		get { return m_nMountId; }
	}

	public CsMountQualityMaster MountQualityMaster
	{
		get { return m_csMountQualityMaster; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public int PotionAttrMaxCount
	{
		get { return m_nPotionAttrMaxCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountQuality(WPDMountQuality mountQuality)
	{
		m_nMountId = mountQuality.mountId;
		m_csMountQualityMaster = CsGameData.Instance.GetMountQualityMaster(mountQuality.quality);
		m_strPrefabName = mountQuality.prefabName;
		m_nPotionAttrMaxCount = mountQuality.potionAttrMaxCount;
	}
}
