using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsArtifactLevelUpMaterial
{
	int m_nTier;
	int m_nGrade;
	int m_nExp;

	//---------------------------------------------------------------------------------------------------
	public int Tier
	{
		get { return m_nTier; }
	}

	public int Grade
	{
		get { return m_nGrade; }
	}

	public int Exp
	{
		get { return m_nExp; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactLevelUpMaterial(WPDArtifactLevelUpMaterial artifactLevelUpMaterial)
	{
		m_nTier = artifactLevelUpMaterial.tier;
		m_nGrade = artifactLevelUpMaterial.grade;
		m_nExp = artifactLevelUpMaterial.exp;
	}
}
