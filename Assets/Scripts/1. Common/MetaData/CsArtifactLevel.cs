using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsArtifactLevel
{
	int m_nArtifactNo;
	int m_nLevel;
	int m_nNextLevelUpRequiredExp;

	List<CsArtifactLevelAttr> m_listCsArtifactLevelAttr;

	//---------------------------------------------------------------------------------------------------
	public int ArtifactNo
	{
		get { return m_nArtifactNo; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int NextLevelUpRequiredExp
	{
		get { return m_nNextLevelUpRequiredExp; }
	}

	public List<CsArtifactLevelAttr> ArtifactLevelAttrList
	{
		get { return m_listCsArtifactLevelAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactLevel(WPDArtifactLevel artifactLevel)
	{
		m_nArtifactNo = artifactLevel.artifactNo;
		m_nLevel = artifactLevel.level;
		m_nNextLevelUpRequiredExp = artifactLevel.nextLevelUpRequiredExp;

		m_listCsArtifactLevelAttr = new List<CsArtifactLevelAttr>();
	}
}
