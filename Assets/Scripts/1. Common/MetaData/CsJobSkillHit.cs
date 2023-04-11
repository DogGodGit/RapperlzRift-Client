using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsJobSkillHit
{
	int m_nJobId;                   // 직업ID
	int m_nSkillId;                 // 스킬ID
	int m_nHitId;                   // 히트ID
	float m_flDamageFactor;         // 피해비중
	int m_nAcquireLak;              // 획득라크 
	List<CsJobSkillHitAbnormalState> m_listCsJobSkillHitAbnormalState;

	//---------------------------------------------------------------------------------------------------
	public int JobId
	{
		get { return m_nJobId; }
	}

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

	public int AcquireLak
	{
		get { return m_nAcquireLak; }
	}

	public List<CsJobSkillHitAbnormalState> JobSkillHitAbnormalStateList
	{
		get { return m_listCsJobSkillHitAbnormalState; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillHit(WPDJobSkillHit jobSkillHit)
	{
		m_nJobId = jobSkillHit.jobId;
		m_nSkillId = jobSkillHit.skillId;
		m_nHitId = jobSkillHit.hitId;
		m_flDamageFactor = jobSkillHit.damageFactor;
		m_nAcquireLak = jobSkillHit.acquireLak;
		m_listCsJobSkillHitAbnormalState = new List<CsJobSkillHitAbnormalState>();
	}
}
