using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGear
{
	int m_nMountGearId;
	int m_nRequiredHeroLevel;
	CsMountGearType m_csMountGearType;
	CsMountGearGrade m_csMountGearGrade;
	CsMountGearQuality m_csMountGearQuality;
	string m_strName;
	int m_nSaleGold;
	CsAttrValueInfo m_csAttrValueMaxHp;
	CsAttrValueInfo m_csAttrValuePhysicalOffense;
	CsAttrValueInfo m_csAttrValueMagicalOffense;
	CsAttrValueInfo m_csAttrValuePhysicalDefense;
	CsAttrValueInfo m_csAttrValueMagicalDefense;

	int m_nBattlePower;
    string m_strImageName;

	//---------------------------------------------------------------------------------------------------
	public int MountGearId
	{
		get { return m_nMountGearId; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsMountGearType MountGearType
	{
		get { return m_csMountGearType; }
	}

	public CsMountGearGrade MountGearGrade
	{
		get { return m_csMountGearGrade; }
	}

	public CsMountGearQuality MountGearQuality
	{
		get { return m_csMountGearQuality; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int SaleGold
	{
		get { return m_nSaleGold; }
	}

	public int MaxHp
	{
		get { return m_csAttrValueMaxHp.Value; }
	}

	public int PhysicalOffense
	{
		get { return m_csAttrValuePhysicalOffense.Value; }
	}

	public int MagicalOffenseAttr
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

	public int BattlePowerValue
	{
		get { return m_nBattlePower; }
	}

    public string ImageName
    {
        get { return m_strImageName; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsMountGear(WPDMountGear mountGear)
	{
		m_nMountGearId = mountGear.mountGearId;
		m_nRequiredHeroLevel = mountGear.requiredHeroLevel;
		m_csMountGearType = CsGameData.Instance.GetMountGearType(mountGear.type);
		m_csMountGearGrade = CsGameData.Instance.GetMountGearGrade(mountGear.grade);
		m_csMountGearQuality = CsGameData.Instance.GetMountGearQuality(mountGear.quality);
		m_strName = CsConfiguration.Instance.GetString(mountGear.nameKey);
		m_nSaleGold = mountGear.saleGold;
		m_csAttrValueMaxHp = CsGameData.Instance.GetAttrValueInfo(mountGear.maxHpAttrValueId);
		m_csAttrValuePhysicalOffense = CsGameData.Instance.GetAttrValueInfo(mountGear.physicalOffenseAttrValueId);
		m_csAttrValueMagicalOffense = CsGameData.Instance.GetAttrValueInfo(mountGear.magicalOffenseAttrValueId);
		m_csAttrValuePhysicalDefense = CsGameData.Instance.GetAttrValueInfo(mountGear.physicalDefenseAttrValueId);
		m_csAttrValueMagicalDefense = CsGameData.Instance.GetAttrValueInfo(mountGear.magicalDefenseAttrValueId);
        m_strImageName = mountGear.imageName;

		m_nBattlePower = m_csAttrValueMaxHp.Value * CsGameData.Instance.GetAttr(EnAttr.MaxHp).BattlePowerFactor
						+ m_csAttrValuePhysicalOffense.Value * CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense).BattlePowerFactor
						+ m_csAttrValueMagicalOffense.Value * CsGameData.Instance.GetAttr(EnAttr.MagicalOffense).BattlePowerFactor
						+ m_csAttrValuePhysicalDefense.Value * CsGameData.Instance.GetAttr(EnAttr.PhysicalDefense).BattlePowerFactor
						+ m_csAttrValueMagicalDefense.Value * CsGameData.Instance.GetAttr(EnAttr.MagicalDefense).BattlePowerFactor;
	}
}
