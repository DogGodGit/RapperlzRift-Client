using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountLevel
{
	int m_nMountId;
	CsMountLevelMaster m_csMountLevelMaster;
	CsAttrValueInfo m_csAttrValueMaxHp;
	CsAttrValueInfo m_csAttrValuePhysicalOffense;
	CsAttrValueInfo m_csAttrValueMagicalOffense;
	CsAttrValueInfo m_csAttrValuePhysicalDefense;
	CsAttrValueInfo m_csAttrValueMagicalDefense;

	//---------------------------------------------------------------------------------------------------
	public int MountId
	{
		get { return m_nMountId; }
	}

	public CsMountLevelMaster MountLevelMaster
	{
		get { return m_csMountLevelMaster; }
	}

	public int MaxHp
	{
		get { return m_csAttrValueMaxHp.Value; }
	}

	public int PhysicalOffense
	{
		get { return m_csAttrValuePhysicalOffense.Value; }
	}

	public int MagicalOffense
	{
		get { return m_csAttrValueMagicalOffense.Value; }
	}

	public int PhysicalDefense
	{
		get { return m_csAttrValuePhysicalDefense.Value; }
	}

	public int MagicalDefense
	{
		get { return m_csAttrValueMagicalDefense.Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountLevel(WPDMountLevel mountLevel)
	{
		m_nMountId = mountLevel.mountId;
		m_csMountLevelMaster = CsGameData.Instance.GetMountLevelMaster(mountLevel.level);
		m_csAttrValueMaxHp = CsGameData.Instance.GetAttrValueInfo(mountLevel.maxHpAttrValueId);
		m_csAttrValuePhysicalOffense = CsGameData.Instance.GetAttrValueInfo(mountLevel.physicalOffenseAttrValueId);
		m_csAttrValueMagicalOffense = CsGameData.Instance.GetAttrValueInfo(mountLevel.magicalOffenseAttrValueId);
		m_csAttrValuePhysicalDefense = CsGameData.Instance.GetAttrValueInfo(mountLevel.physicalDefenseAttrValueId);
		m_csAttrValueMagicalDefense = CsGameData.Instance.GetAttrValueInfo(mountLevel.magicalDefenseAttrValueId);
	}
}
