using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroBiography
{
	int m_nBiographyId;
	bool m_bCompleted;
	CsHeroBiographyQuest m_csHeroBiograhyQuest;
	CsBiography m_csBiography;

	//---------------------------------------------------------------------------------------------------
	public int BiographyId
	{
		get { return m_nBiographyId; }
	}

	public bool Completed
	{
		get { return m_bCompleted; }
		set { m_bCompleted = value; }
	}
	public CsHeroBiographyQuest HeroBiograhyQuest
	{
		get { return m_csHeroBiograhyQuest; }
		set { m_csHeroBiograhyQuest = value; }
	}

	public CsBiography Biography
	{
		get { return m_csBiography; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBiography(PDHeroBiography heroBiography)
	{
		m_csBiography = CsGameData.Instance.GetBiography(heroBiography.biographyId);

		m_nBiographyId = heroBiography.biographyId;
		m_bCompleted = heroBiography.completed;
		m_csHeroBiograhyQuest = heroBiography.quest == null ? null : new CsHeroBiographyQuest(m_csBiography, heroBiography.quest);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBiography(int nBigraphyId)
	{
		m_csBiography = CsGameData.Instance.GetBiography(nBigraphyId);

		m_nBiographyId = nBigraphyId;
		m_bCompleted = false;
		m_csHeroBiograhyQuest = null;
	}
}
