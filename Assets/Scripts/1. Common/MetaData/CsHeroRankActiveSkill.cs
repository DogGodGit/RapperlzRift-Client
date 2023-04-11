using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsHeroRankActiveSkill
{
	int m_nSkillId;
	int m_nLevel;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
		set { m_nLevel = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRankActiveSkill(PDHeroRankActiveSkill heroRankActiveSkill)
	{
		m_nSkillId = heroRankActiveSkill.skillId;
		m_nLevel = heroRankActiveSkill.level;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRankActiveSkill(int nSkillId, int nLevel)
	{
		m_nSkillId = nSkillId;
		m_nLevel = nLevel;
	}
}
