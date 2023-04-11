using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsHeroConstellation
{
	int m_nId;
	CsConstellation m_csConstellation;
	List<CsHeroConstellationStep> m_listCsHeroConstellationStep;

	//---------------------------------------------------------------------------------------------------
	public int Id
	{
		get { return m_nId; }
	}

	public List<CsHeroConstellationStep> HeroConstellationStepList
	{
		get { return m_listCsHeroConstellationStep; }
	}

	public CsConstellation Constellation
	{
		get { return m_csConstellation; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroConstellation(PDHeroConstellation heroConstellation)
	{
		m_nId = heroConstellation.id;
		m_csConstellation = CsGameData.Instance.GetConstellation(m_nId);

		m_listCsHeroConstellationStep = new List<CsHeroConstellationStep>();

		for (int i = 0; i < heroConstellation.steps.Length; i++)
		{
			m_listCsHeroConstellationStep.Add(new CsHeroConstellationStep(m_nId, heroConstellation.steps[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroConstellationStep GetHeroConstellationStep(int nStep)
	{
		for (int i = 0; i < m_listCsHeroConstellationStep.Count; i++)
		{
			if (m_listCsHeroConstellationStep[i].Step == nStep)
				return m_listCsHeroConstellationStep[i];
		}

		return null;
	}
}
