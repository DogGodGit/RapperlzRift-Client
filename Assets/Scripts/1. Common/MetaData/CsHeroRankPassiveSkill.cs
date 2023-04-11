using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsHeroRankPassiveSkill
{
	int m_nSkillId;
	int m_nLevel;
	CsRankPassiveSkill m_csRankPassiveSkill;

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

	public CsRankPassiveSkill RankPassiveSkill
	{
		get { return m_csRankPassiveSkill; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRankPassiveSkill(PDHeroRankPassiveSkill heroRankPassiveSkill)
	{
		m_nSkillId = heroRankPassiveSkill.skillId;
		m_nLevel = heroRankPassiveSkill.level;
		m_csRankPassiveSkill = CsGameData.Instance.GetRankPassiveSkill(m_nSkillId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRankPassiveSkill(int nSkillId, int nLevel)
	{
		m_nSkillId = nSkillId;
		m_nLevel = nLevel;
		m_csRankPassiveSkill = CsGameData.Instance.GetRankPassiveSkill(m_nSkillId);
	}
}
