using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsMonsterSkillHit
{
	int m_nSkillId;
	int m_nHitId;
	float m_flDamageFactor;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int HitId
	{
		get { return m_nHitId; }
	}

	public float DamageFactor
	{
		get { return m_flDamageFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterSkillHit(WPDMonsterSkillHit monsterSkillHit)
	{
		m_nSkillId = monsterSkillHit.skillId;
		m_nHitId = monsterSkillHit.hitId;
		m_flDamageFactor = monsterSkillHit.damageFactor;
	}
}
