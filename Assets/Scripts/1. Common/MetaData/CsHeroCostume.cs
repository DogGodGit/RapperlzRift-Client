using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroCostume
{
	CsCostume m_csCostume;
	int m_nEffectId;
	CsCostumeEffect m_csCostumeEffect;
	float m_flRemainingTime;
	int m_nEnchantLevel;
	int m_nLuckyValue;

	//---------------------------------------------------------------------------------------------------
	public int HeroCostumeId
	{
		get { return m_csCostume.CostumeId; }
	}

	public CsCostume Costume
	{
		get { return m_csCostume; }
	}

	public int EffectId
	{
		get { return m_nEffectId; }
		set
		{
			m_nEffectId = value;
			m_csCostumeEffect = CsGameData.Instance.GetCostumeEffect(m_nEffectId);
		}
	}

	public CsCostumeEffect CostumeEffect
	{
		get { return m_csCostumeEffect; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
	}

	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
		set { m_nEnchantLevel = value; }
	}

	public int LuckyValue
	{
		get { return m_nLuckyValue; }
		set { m_nLuckyValue = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCostume(PDHeroCostume heroCostume)
	{
		m_csCostume = CsGameData.Instance.GetCostume(heroCostume.costumeId);
		m_nEffectId = heroCostume.costumeEffectId;
		m_csCostumeEffect = CsGameData.Instance.GetCostumeEffect(heroCostume.costumeEffectId);
		m_flRemainingTime = heroCostume.remainingTime + Time.realtimeSinceStartup;
		m_nEnchantLevel = heroCostume.enchantLevel;
		m_nLuckyValue = heroCostume.luckyValue;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCostume(int costumeId, float remainingTime)
	{
		m_csCostume = CsGameData.Instance.GetCostume(costumeId);
		m_nEffectId = 0;
		m_csCostumeEffect = null;
		m_flRemainingTime = remainingTime + Time.realtimeSinceStartup;
		m_nEnchantLevel = 0;
		m_nLuckyValue = 0;
	}
}
