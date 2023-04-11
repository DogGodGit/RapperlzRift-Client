using System;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsJobLevelMaster : IComparable
{
	int m_nLevel;
	long m_lNextLevelUpExp;
	int m_nInventorySlotAccCount;
	int m_nInventorySlotCount;
	CsExpReward m_csExpRewardRestMax;
	int m_nPotionAttrMaxCount;
	int m_nTradition;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public long NextLevelUpExp
	{
		get { return m_lNextLevelUpExp; }
	}

	public int InventorySlotAccCount
	{
		get { return m_nInventorySlotAccCount; }
	}

	public int InventorySlotCount
	{
		get { return m_nInventorySlotCount; }
	}

	public int PotionAttrMaxCount
	{
		get { return m_nPotionAttrMaxCount; }
	}

	public long ExpReward
	{
		get { return Mathf.FloorToInt(m_csExpRewardRestMax.Value * ((CsGameData.Instance.MyHeroInfo.RestTime * 1.0f) / CsGameData.Instance.RestRewardTimeList[CsGameData.Instance.RestRewardTimeList.Count - 1].RestTime)); }
	}

	public long ExpRewardByGold
	{
		get { return Mathf.FloorToInt(ExpReward * (CsGameConfig.Instance.RestRewardGoldReceiveExpPercentage * 0.01f)); }
	}

	public long ExpRewardByDia
	{
		get { return Mathf.FloorToInt(ExpReward * (CsGameConfig.Instance.RestRewardDiaReceiveExpPercentage * 0.01f)); }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobLevelMaster(WPDJobLevelMaster jobLevelMaster)
	{
		m_nLevel = jobLevelMaster.level;
		m_lNextLevelUpExp = jobLevelMaster.nextLevelUpExp;
		m_nInventorySlotAccCount = jobLevelMaster.inventorySlotAccCount;
		m_nInventorySlotCount = jobLevelMaster.inventorySlotAccCount;
		m_nPotionAttrMaxCount = jobLevelMaster.potionAttrMaxCount;
		m_csExpRewardRestMax = CsGameData.Instance.GetExpReward(jobLevelMaster.restMaxExpRewardId);
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nLevel.CompareTo(((CsJobLevelMaster)obj).Level);
	}
	#endregion Interface(IComparable) implement
}
