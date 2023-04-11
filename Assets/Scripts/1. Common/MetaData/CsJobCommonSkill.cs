using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-06-05)
//---------------------------------------------------------------------------------------------------

public class CsJobCommonSkill
{
	int m_nSkillId;
	int m_nOpenRequiredMainQuestNo;
	string m_strName;
	string m_strDescription;
	int m_nType;                    // 1:캐릭터종속, 2:캐릭터독립
	int m_nFormType;                // 1:연계, 2:일반, 3:버프
	bool m_bIsRequireTarget;
	float m_flCastRange;
	float m_flHitRange;
	float m_flCoolTime;
	int m_nHitAreaType;				// 1:원(반지름, 각도)  2:사각형(가로, 세로)
	float m_flHitAreaValue1;
	float m_flHitAreaValue2;
	int m_nHitAreaOffsetType;		// 1:옵셋 기준   2:시전거리 기준
	float m_flHitAreaOffset;
	int m_nSlotIndex;
	int m_nClientSkillIndex;
	int m_nMentalStrengthDamage;    // 테이밍몬스터정신력데미지

	// UI 사용변수
	float m_flRemainCoolTime;
	bool m_bIsClicked;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int OpenRequiredMainQuestNo
	{
		get { return m_nOpenRequiredMainQuestNo; }
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

	public int FormType
	{
		get { return m_nFormType; }
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

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public int ClientSkillIndex
	{
		get { return m_nClientSkillIndex; }
	}

	public int MentalStrengthDamage
	{
		get { return m_nMentalStrengthDamage; }
	}

	//---------------------------------------------------------------------------------------------------

	public bool IsClicked
	{
		get { return m_bIsClicked; }
		set { m_bIsClicked = value; }
	}

	public float RemainCoolTime
	{
		get { return m_flRemainCoolTime; }
		set
		{
			m_flRemainCoolTime = value;
			if (m_flRemainCoolTime < 0)
				m_flRemainCoolTime = 0;
		}
	}

	public float CoolTimeProgress
	{
		get { return m_flRemainCoolTime / m_flCoolTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobCommonSkill(WPDJobCommonSkill jobCommonSkill)
	{
		m_nSkillId = jobCommonSkill.skillId;
		m_nOpenRequiredMainQuestNo = jobCommonSkill.openRequiredMainQuestNo;
		m_strName = CsConfiguration.Instance.GetString(jobCommonSkill.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(jobCommonSkill.descriptionKey);
		m_nType = jobCommonSkill.type;
		m_nFormType = jobCommonSkill.formType;
		m_bIsRequireTarget = jobCommonSkill.isRequireTarget;
		m_flCastRange = jobCommonSkill.castRange;
		m_flHitRange = jobCommonSkill.hitRange;
		m_flCoolTime = jobCommonSkill.coolTime;
		m_nHitAreaType = jobCommonSkill.hitAreaType;
		m_flHitAreaValue1 = jobCommonSkill.hitAreaValue1;
		m_flHitAreaValue2 = jobCommonSkill.hitAreaValue2;
		m_nHitAreaOffsetType = jobCommonSkill.hitAreaOffsetType;
		m_flHitAreaOffset = jobCommonSkill.hitAreaOffset;
		m_nSlotIndex = jobCommonSkill.slotIndex;
		m_nClientSkillIndex = jobCommonSkill.clientSkillIndex;
		m_nMentalStrengthDamage = jobCommonSkill.mentalStrengthDamage;

		m_bIsClicked = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartUseSkill()
	{
		m_flRemainCoolTime = m_flCoolTime;
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset()
	{
		m_flRemainCoolTime = 0;
		m_bIsClicked = false;
	}
}
