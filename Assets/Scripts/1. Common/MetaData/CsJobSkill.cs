using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------
public enum EnJobSkillType // 스킬타입(1:캐릭터종속, 2:캐릭터독립)
{
	Character = 1,
	AreaOfEffect = 2,
}

public enum EnHitAreaType // 적중범위유형(1:원(반지름,각도), 2:사각형(가로, 세로))
{
	Circle = 1,
	Rectangle = 2,
}

public enum EnHitAreaOffsetType // 적중범위옵셋타입(1:본인위치 기준, 2:대상위치 기준)
{
	Person = 1,
	Target = 2
}

public enum EnHeroHitType // 영웅적중타입(0:없음(영웅은 안맞음) 1:단일, 2:다중)
{
	None = 0,
	Single = 1,
	Multiple = 2
}

public enum EnFormType // 스킬형태(1:연계, 2:일반, 3:버프)
{
	Chain = 1,
	Normal = 2,
	Buff = 3
}

public enum EnCastingMoveType // 시전중이동타입(0:이동없음, 1:고정위치이동, 2:조작형)
{
	No = 0,
	Fixed = 1,
	Manual = 2
}	

public class CsJobSkill
{
	int m_nJobId;									// 직업ID
	int m_nSkillId;									// 스킬ID
	string m_strName;								// 이름
	string m_strDescription;						// 설명
	int m_nType;									// 스킬타입(1:캐릭터종속, 2:캐릭터독립)
	int m_nFormType;								// 스킬형태(1:연계, 2:일반, 3:버프)
	bool m_bIsRequireTarget;						// 타겟여부
	float m_flCastRange;							// 시전거리
	float m_flHitRange;								// 유효사거리
	float m_flCoolTime;								// 쿨타임(초)
	int m_nHeroHitType;								// 영웅적중타입(0:없음(영웅은 안맞음) 1:단일, 2:다중)
	int m_nHitAreaType;								// 적중범위유형(1:원(반지름,각도), 2:사각형(가로, 세로))
	float m_flHitAreaValue1;						// 적중범위값1
	float m_flHitAreaValue2;						// 적중범위값2
	int m_nHitAreaOffsetType;						// 적중범위옵셋타입(1:옵셋 기준, 2:시전거리 기준)
	float m_flHitAreaOffset;						// 적중범위옵셋
	int m_nSlotIndex;								// 스킬슬롯인덱스
	float m_flSsStartDelay;							// 독립스킬시작딜레이
	float m_flSsDuration;							// 독립스킬지속시간
	int m_nCastingMoveType;							// 시전중이동타입(0:이동없음, 1:고정위치이동, 2:조작형)
	int m_nCastingMoveValue1;						// 시전중이동값1
	int m_nCastingMoveValue2;						// 시전중이동값2
	int m_nAutoPriorityGroup;						// 자동우선순위그룹
	int m_nAutoWeight;								// 자동가중치
	int m_nClientSkillIndex;						// 클라이언트스킬인덱스

	List<CsJobSkillLevel> m_listCsJobSkillLevel;	// 스킬레벨리스트
	List<CsJobSkillHit> m_listCsJobSkillHit;        // 스킬히트리스트
	List<CsJobChainSkill> m_listCsJobChainSkill;	// 체인스킬리스트

	//---------------------------------------------------------------------------------------------------
	public int JobId
	{
		get { return m_nJobId; }
	}

	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public EnJobSkillType TypeEnum
	{
		get { return (EnJobSkillType)m_nType; }
	}

	public int FormType
	{
		get { return m_nFormType; }
	}

	public EnFormType FormTypeEnum
	{
		get { return (EnFormType)m_nFormType; }
	}

	public bool IsRequireTarget
	{
		get { return m_bIsRequireTarget; }
	}

	public float CastRange
	{
		get { return m_flCastRange; }
	}

	public float HitRange
	{
		get { return m_flHitRange; }
	}

	public float CoolTime
	{
		get { return m_flCoolTime; }
	}

	public int HeroHitType
	{
		get { return m_nHeroHitType; }
	}

	public EnHeroHitType HeroHitTypeEnum
	{
		get { return (EnHeroHitType)m_nHeroHitType; }
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

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public float SsStartDelay
	{
		get { return m_flSsStartDelay; }
	}

	public float SsDuration
	{
		get { return m_flSsDuration; }
	}

	public int CastingMoveType
	{
		get { return m_nCastingMoveType; }
	}

	public EnCastingMoveType CastingMoveTypeEnum
	{
		get { return (EnCastingMoveType)m_nCastingMoveType; }
	}

	public int CastingMoveValue1
	{
		get { return m_nCastingMoveValue1; }
	}

	public int CastingMoveValue2
	{
		get { return m_nCastingMoveValue2; }
	}

	public int AutoPriorityGroup
	{
		get { return m_nAutoPriorityGroup; }
	}

	public int AutoWeight
	{
		get { return m_nAutoWeight; }
	}

	public int ClientSkillIndex
	{
		get { return m_nClientSkillIndex; }
	}

	public List<CsJobSkillLevel> JobSkillLevelList
	{
		get { return m_listCsJobSkillLevel; }
	}

	public List<CsJobSkillHit> JobSkillHitList
	{
		get { return m_listCsJobSkillHit; }
	}

	public List<CsJobChainSkill> JobChainSkillList
	{
		get { return m_listCsJobChainSkill; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkill(WPDJobSkill jobSkill)
	{
		m_nJobId = jobSkill.jobId;
		m_nSkillId = jobSkill.skillId;
		m_strName = CsConfiguration.Instance.GetString(jobSkill.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(jobSkill.descriptionKey);
		m_nType = jobSkill.type;
		m_nFormType = jobSkill.formType;
		m_bIsRequireTarget = jobSkill.isRequireTarget;
		m_flCastRange = jobSkill.castRange;
		m_flHitRange = jobSkill.hitRange;
		m_flCoolTime = jobSkill.coolTime;
		m_nHeroHitType = jobSkill.heroHitType;
		m_nHitAreaType = jobSkill.hitAreaType;
		m_flHitAreaValue1 = jobSkill.hitAreaValue1;
		m_flHitAreaValue2 = jobSkill.hitAreaValue2;
		m_nHitAreaOffsetType = jobSkill.hitAreaOffsetType;
		m_flHitAreaOffset = jobSkill.hitAreaOffset;
		m_nSlotIndex = jobSkill.slotIndex;
		m_flSsStartDelay = jobSkill.ssStartDelay;
		m_flSsDuration = jobSkill.ssDuration;
		m_nCastingMoveType = jobSkill.castingMoveType;
		m_nCastingMoveValue1 = jobSkill.castingMoveValue1;
		m_nCastingMoveValue2 = jobSkill.castingMoveValue2;
		m_nAutoPriorityGroup = jobSkill.autoPriorityGroup;
		m_nAutoWeight = jobSkill.autoWeight;
		m_nClientSkillIndex = jobSkill.clientSkillIndex;

		m_listCsJobSkillLevel = new List<CsJobSkillLevel>();
		m_listCsJobSkillHit = new List<CsJobSkillHit>();
		m_listCsJobChainSkill = new List<CsJobChainSkill>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobChainSkill GetJobChainSkill(int nChainSkillId)
	{
		for (int i = 0; i < m_listCsJobChainSkill.Count; i++)
		{
			if (m_listCsJobChainSkill[i].ChainSkillId == nChainSkillId)
				return m_listCsJobChainSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillLevel GetJobSkillLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsJobSkillLevel.Count; i++)
		{
			if (m_listCsJobSkillLevel[i].Level == nLevel)
				return m_listCsJobSkillLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillHit GetJobSkillHit(int nHitId)
	{
		for (int i = 0; i < m_listCsJobSkillHit.Count; i++)
		{
			if (m_listCsJobSkillHit[i].HitId == nHitId)
				return m_listCsJobSkillHit[i];
		}

		return null;
	}
}
