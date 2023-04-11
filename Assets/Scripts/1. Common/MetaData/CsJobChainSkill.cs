using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsJobChainSkill
{
	int m_nJobId;                           // 직업ID
	int m_nSkillId;                         // 스킬ID
	int m_nChainSkillId;                    // 연계스킬ID
	int m_nHitAreaType;                     // 적중범위유형(1:원(반지름,각도), 2:사각형(가로, 세로))
	float m_flHitAreaValue1;                // 적중범위값1
	float m_flHitAreaValue2;                // 적중범위값2
	int m_nHitAreaOffsetType;               // 적중범위옵셋타입(1:옵셋 기준, 2:시전거리 기준)
	float m_flHitAreaOffset;                // 적중범위옵셋
	float m_flCastConditionStartTime;       // 시전조건시작시간(초)
	float m_flCastConditionEndTIme;         // 시전조건종료시간(초)

	List<CsJobChainSkillHit> m_listCsJobChainSkillHit;
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

	public int HitAreaType
	{
		get { return m_nHitAreaType; }
	}

	public EnHitAreaType HitAreaTypeEnum
	{
		get { return (EnHitAreaType)m_nHitAreaType; }
	}

	public float HitAreaValue1
	{
		get { return m_flHitAreaValue1; }
	}

	public float HitAreaValue2
	{
		get { return m_flHitAreaValue2; }
	}

	public int HitAreaOffsetType
	{
		get { return m_nHitAreaOffsetType; }
	}

	public EnHitAreaOffsetType HitAreaOffsetTypeEnum
	{
		get { return (EnHitAreaOffsetType)m_nHitAreaOffsetType; }
	}

	public float HitAreaOffset
	{
		get { return m_flHitAreaOffset; }
	}

	public float CastConditionStartTime
	{
		get { return m_flCastConditionStartTime; }
	}

	public float CastConditionEndTime
	{
		get { return m_flCastConditionEndTIme; }
	}

	public List<CsJobChainSkillHit> JobChainSkillHitList
	{
		get { return m_listCsJobChainSkillHit; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobChainSkill(WPDJobChainSkill jobChainSkill)
	{
		m_nJobId = jobChainSkill.jobId;
		m_nSkillId = jobChainSkill.skillId;
		m_nChainSkillId = jobChainSkill.chainSkillId;
		m_nHitAreaType = jobChainSkill.hitAreaType;
		m_flHitAreaValue1 = jobChainSkill.hitAreaValue1;
		m_flHitAreaValue2 = jobChainSkill.hitAreaValue2;
		m_nHitAreaOffsetType = jobChainSkill.hitAreaOffsetType;
		m_flHitAreaOffset = jobChainSkill.hitAreaOffset;
		m_flCastConditionStartTime = jobChainSkill.castConditionStartTime;
		m_flCastConditionEndTIme = jobChainSkill.castConditionEndTIme;

		m_listCsJobChainSkillHit = new List<CsJobChainSkillHit>();
	}
}
