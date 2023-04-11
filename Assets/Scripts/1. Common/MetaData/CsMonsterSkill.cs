using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsMonsterSkill
{
	int m_nSkillId;
	int m_nBaseDamageType;
	int m_nType;
	int m_nElementalId;
	int m_nFormType;
	bool m_bIsRequiredTarget;
	float m_flCastRange;
	float m_flHitRange;
	float m_flCoolTime;
	int m_nHitAreaType;
	float m_flHitAreaValue1;
	float m_flHitAreaValue2;
	int m_nHitAreaOffsetType;
	float m_flHitAreaOffset;
	float m_flSsStartDelay;
	float m_flSsDuration;
	int m_nAutoPriorityGroup;
	int m_nAutoWeight;
	string m_strSound;
	float m_flSoundVolume;

	List<CsMonsterSkillHit> m_listCsMonsterSkillHit;
	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int BaseDamageType
	{
		get { return m_nBaseDamageType; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int ElementalId
	{
		get { return m_nElementalId; }
	}

	public int FormType
	{
		get { return m_nFormType; }
	}

	public bool IsRequiredTarget
	{
		get { return m_bIsRequiredTarget; }
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

	public int HitAreaType
	{
		get { return m_nHitAreaType; }
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

	public float HitAreaOffset
	{
		get { return m_flHitAreaOffset; }
	}

	public float SsStartDelay
	{
		get { return m_flSsStartDelay; }
	}

	public float SsDuration
	{
		get { return m_flSsDuration; }
	}

	public int AutoPriorityGroup
	{
		get { return m_nAutoPriorityGroup; }
	}

	public int AutoWeight
	{
		get { return m_nAutoWeight; }
	}

	public string Sound
	{
		get { return m_strSound; }
	}

	public float SoundVolume
	{
		get { return m_flSoundVolume; }
	}

	public List<CsMonsterSkillHit> MonsterSkillHitList
	{
		get { return m_listCsMonsterSkillHit; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterSkill(WPDMonsterSkill monsterSkill)
	{
		m_nSkillId = monsterSkill.skillId;
		m_nBaseDamageType = monsterSkill.baseDamageType;
		m_nType = monsterSkill.type;
		m_nElementalId = monsterSkill.elementalId;
		m_nFormType = monsterSkill.formType;
		m_bIsRequiredTarget = monsterSkill.isRequiredTarget;
		m_flCastRange = monsterSkill.castRange;
		m_flHitRange = monsterSkill.hitRange;
		m_flCoolTime = monsterSkill.coolTime;
		m_nHitAreaType = monsterSkill.hitAreaType;
		m_flHitAreaValue1 = monsterSkill.hitAreaValue1;
		m_flHitAreaValue2 = monsterSkill.hitAreaValue2;
		m_nHitAreaOffsetType = monsterSkill.hitAreaOffsetType;
		m_flHitAreaOffset = monsterSkill.hitAreaOffset;
		m_flSsStartDelay = monsterSkill.ssStartDelay;
		m_flSsDuration = monsterSkill.ssDuration;
		m_nAutoPriorityGroup = monsterSkill.autoPriorityGroup;
		m_nAutoWeight = monsterSkill.autoWeight;
		m_strSound = monsterSkill.sound;
		m_flSoundVolume = monsterSkill.soundVolume;

		m_listCsMonsterSkillHit = new List<CsMonsterSkillHit>();
	}
}
