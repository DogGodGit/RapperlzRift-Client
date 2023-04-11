using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsHeroTitle
{
	CsTitle m_csTitle;
	float m_flRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public CsTitle Title
	{
		get { return m_csTitle; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTitle(PDHeroTitle heroTitle)
	{
		m_csTitle = CsGameData.Instance.GetTitle(heroTitle.titleId);

        if (heroTitle.remainingTime > 0)
            m_flRemainingTime = Time.realtimeSinceStartup + heroTitle.remainingTime;
        else
            m_flRemainingTime = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTitle(int nTitleId, float flRemainingTime)
	{
		m_csTitle = CsGameData.Instance.GetTitle(nTitleId);

		if (flRemainingTime > 0)
			m_flRemainingTime = Time.realtimeSinceStartup + flRemainingTime;
        else
            m_flRemainingTime = 0;
    }
}
