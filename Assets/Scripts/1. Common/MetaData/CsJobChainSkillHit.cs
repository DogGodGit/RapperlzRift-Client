using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsJobChainSkillHit
{
	int m_nJobId;                   // 직업ID
	int m_nSkillId;                 // 스킬ID
	int m_nChainSkillId;            // 연계스킬ID
	int m_nHitId;                   // 히트ID
	float m_flDamageFactor;         // 피해비중
	int m_nAcquireLak;              // 획득라크 

	//---------------------------------------------------------------------------------------------------
	public int JobId
	{
		get { return m_nJobId; }
	}

	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int ChainSkillId
	{
		get { return m_nChainSkillId; }
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

	//---------------------------------------------------------------------------------------------------
	public CsJobChainSkillHit(WPDJobChainSkillHit jobChainSkillHit)
	{
		m_nJobId = jobChainSkillHit.jobId;
		m_nSkillId = jobChainSkillHit.skillId;
		m_nChainSkillId = jobChainSkillHit.chainSkillId;
		m_nHitId = jobChainSkillHit.hitId;
		m_flDamageFactor = jobChainSkillHit.damageFactor;
		m_nAcquireLak = jobChainSkillHit.acquireLak;
	}
}
