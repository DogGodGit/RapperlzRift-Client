using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroWingPart
{
	int m_nPartId;
	List<CsHeroWingEnchant> m_listCsHeroWingEnchant;

	//---------------------------------------------------------------------------------------------------
	public int PartId
	{
		get { return m_nPartId; }
	}

	public List<CsHeroWingEnchant> HeroWingEnchantList
	{
		get { return m_listCsHeroWingEnchant; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWingPart(PDHeroWingPart heroWingPart)
	{
		m_nPartId = heroWingPart.partId;

		m_listCsHeroWingEnchant = new List<CsHeroWingEnchant>();

		for (int i = 0; i < heroWingPart.enchants.Length; i++)
		{
			m_listCsHeroWingEnchant.Add(new CsHeroWingEnchant(heroWingPart.enchants[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWingEnchant GetHeroWingEnchant(int nStep, int nLevel)
	{
		for (int i = 0; i < m_listCsHeroWingEnchant.Count; i++)
		{
			if (m_listCsHeroWingEnchant[i].Step == nStep && m_listCsHeroWingEnchant[i].Level == nLevel)
			{
				return m_listCsHeroWingEnchant[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateHeroWingEnchant(PDHeroWingEnchant heroWingEnchant)
	{
		CsHeroWingEnchant csHeroWingEnchant = GetHeroWingEnchant(heroWingEnchant.step, heroWingEnchant.level);

		if (csHeroWingEnchant != null)
		{
			csHeroWingEnchant.EnchantCount = heroWingEnchant.enchantCount;
		}
		else
		{
			m_listCsHeroWingEnchant.Add(new CsHeroWingEnchant(heroWingEnchant));
		}
	}

	public int GetBattlePower()
	{
		int nBattlePower = 0;

		for (int i = 0; i < m_listCsHeroWingEnchant.Count; i++)
		{
			CsWingStep csWingStep = CsGameData.Instance.GetWingStep(m_listCsHeroWingEnchant[i].Step);
			CsWingStepPart csWingStepPart = csWingStep.GetWingStepPart(m_nPartId);

			nBattlePower += m_listCsHeroWingEnchant[i].EnchantCount * csWingStepPart.IncreaseAttrValueInfo.Value * csWingStepPart.WingPart.Attr.BattlePowerFactor;
		}

		return nBattlePower;
	}

}
