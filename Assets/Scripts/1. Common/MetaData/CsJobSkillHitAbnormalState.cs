using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsJobSkillHitAbnormalState
{
	int m_nJobId;
	int m_nSkillId;
	int m_nHitId;
	int m_nAbnormalStateId;

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

	public int AbnormalStateId
	{
		get { return m_nAbnormalStateId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillHitAbnormalState(WPDJobSkillHitAbnormalState jobSkillHitAbnormalState)
	{
		m_nJobId = jobSkillHitAbnormalState.jobId;
		m_nSkillId = jobSkillHitAbnormalState.skillId;
		m_nHitId = jobSkillHitAbnormalState.hitId;
		m_nAbnormalStateId = jobSkillHitAbnormalState.abnormalStateId;
	}
}
