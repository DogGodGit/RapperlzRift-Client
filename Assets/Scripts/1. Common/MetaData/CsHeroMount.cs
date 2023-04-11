using System.Collections.Generic;
using UnityEngine;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsHeroMount
{
	CsMount m_csMount;
	CsMountLevel m_csMountLevel;
	int m_nSatiety;
	int m_nBattlePower;
	int m_nAwakeningLevel;
	CsMountAwakeningLevelMaster m_csMountAwakeningLevelMaster;
	int m_nAwakeningExp;
	int m_nPotionAttrCount;

	//---------------------------------------------------------------------------------------------------
	public CsMount Mount
	{
		get { return m_csMount; }
	}

	public int Level
	{
		get { return m_csMountLevel.MountLevelMaster.Level; }
		set
		{
			m_csMountLevel = m_csMount.GetMountLevel(value);
			UpdateBattlePower();
		}
	}

	public CsMountLevel MountLevel
	{
		get { return m_csMountLevel; }
	}

	public int Satiety
	{
		get { return m_nSatiety; }
		set { m_nSatiety = value; }
	}

	public int BattlePower
	{
		get { return m_nBattlePower; }
	}

	public string PrefabName
	{
		get
		{
			CsMountQuality csMountQuality = m_csMount.GetMountQuality(m_csMountLevel.MountLevelMaster.MountQualityMaster.Quality);

			if (csMountQuality != null)
				return csMountQuality.PrefabName;
			else
				return null;
		}
	}

	public CsMountAwakeningLevelMaster MountAwakeningLevelMaster
	{
		get { return m_csMountAwakeningLevelMaster; }
	}

	public int AwakeningLevel
	{
		get { return m_nAwakeningLevel; }
		set
		{
			m_nAwakeningLevel = value;
			m_csMountAwakeningLevelMaster = CsGameData.Instance.GetMountAwakeningLevelMaster(m_nAwakeningLevel);
			UpdateBattlePower();
		}
	}

	public int AwakeningExp
	{
		get { return m_nAwakeningExp; }
		set { m_nAwakeningExp = value; }
	}

	public int PotionAttrCount
	{
		get { return m_nPotionAttrCount; }
		set
		{
			m_nPotionAttrCount = value;
			UpdateBattlePower();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMount(PDHeroMount heroMount)
	{
		m_csMount = CsGameData.Instance.GetMount(heroMount.mountId);
		m_csMountLevel = m_csMount.GetMountLevel(heroMount.level);
		m_nSatiety = heroMount.satiety;

		m_nAwakeningLevel = heroMount.awakeningLevel;
		m_csMountAwakeningLevelMaster = CsGameData.Instance.GetMountAwakeningLevelMaster(heroMount.awakeningLevel);
		m_nAwakeningExp = heroMount.awakeningExp;
		m_nPotionAttrCount = heroMount.potionAttrCount;

		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateBattlePower()
	{
		float flFactor = 1.0f;

		if (CsGameData.Instance.MyHeroInfo.EquippedMountId != m_csMount.MountId)
			flFactor = m_csMountAwakeningLevelMaster.UnequippedAttrFactor;

		m_nBattlePower = Mathf.FloorToInt(m_csMountLevel.MaxHp * CsGameData.Instance.GetAttr(EnAttr.MaxHp).BattlePowerFactor * flFactor)
						+ Mathf.FloorToInt(m_csMountLevel.PhysicalOffense * CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense).BattlePowerFactor * flFactor)
						+ Mathf.FloorToInt(m_csMountLevel.MagicalOffense * CsGameData.Instance.GetAttr(EnAttr.MagicalOffense).BattlePowerFactor * flFactor)
						+ Mathf.FloorToInt(m_csMountLevel.PhysicalDefense * CsGameData.Instance.GetAttr(EnAttr.PhysicalDefense).BattlePowerFactor * flFactor)
						+ Mathf.FloorToInt(m_csMountLevel.MagicalDefense * CsGameData.Instance.GetAttr(EnAttr.MagicalDefense).BattlePowerFactor * flFactor);

		List<CsMountPotionAttrCount> list = CsGameData.Instance.GetMountPotionAttrCountList(m_nPotionAttrCount);

		for (int i = 0; i < list.Count; i++)
		{
			m_nBattlePower += Mathf.FloorToInt(list[i].AttrValue.Value * CsGameData.Instance.GetAttr(list[i].Attr.EnAttr).BattlePowerFactor * flFactor);
		}
	}

	

}
