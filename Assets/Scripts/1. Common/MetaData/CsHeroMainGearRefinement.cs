using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroMainGearRefinement
{
	int m_nTurn;
	List<CsHeroMainGearRefinementAttr> m_listCsHeroMainGearRefinementAttr;

	int m_nBattlePower = 0;

	//---------------------------------------------------------------------------------------------------
	public int Turn
	{
		get { return m_nTurn; }
	}

	public List<CsHeroMainGearRefinementAttr> HeroMainGearRefinementAttrList
	{
		get { return m_listCsHeroMainGearRefinementAttr; }
	}

	public int OptionAttributesBattlePower
	{
		get { return m_nBattlePower; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGearRefinement(PDHeroMainGearRefinement heroMainGearRefinement)
	{
		m_nTurn = heroMainGearRefinement.turn;

		m_listCsHeroMainGearRefinementAttr = new List<CsHeroMainGearRefinementAttr>();

		for (int i = 0; i < heroMainGearRefinement.attrs.Length; i++)
		{
			CsHeroMainGearRefinementAttr csHeroMainGearRefinementAttr = new CsHeroMainGearRefinementAttr(heroMainGearRefinement.attrs[i]);
			m_listCsHeroMainGearRefinementAttr.Add(csHeroMainGearRefinementAttr);

			m_nBattlePower += csHeroMainGearRefinementAttr.BattlePower;
		}
	}
}
