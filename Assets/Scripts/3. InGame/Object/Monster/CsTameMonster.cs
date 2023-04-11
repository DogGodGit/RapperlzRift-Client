using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnTameState { Idle = 0, Walk = 1, Run = 2, Attack = 3, Dead = 7, Groggy = 8}

public class CsTameMonster : CsMoveUnit
{
	enum EnTameStatus { Idle = 0, Walk, Run, Skill01, Skill02, Skill03, Skill04, Dead, Groggy}	
	static int s_nAnimatorHash_status = Animator.StringToHash("status");
	static int s_nAnimatorHash_dominate = Animator.StringToHash("dominate");
	static int s_nAnimatorHash_groggy = Animator.StringToHash("groggy");

	CsMonsterSkill m_csMonsterSkill;
	CapsuleCollider m_capsuleCollider;
	Transform m_trMyHero;
	Transform m_trMyTransform;
	RectTransform m_rtfTame = null;

	AudioSource m_audioSource;
	AudioClip m_asAttack01;
	AudioClip m_asAttack02;
	AudioClip m_asRunRight;
	AudioClip m_asRunLeft;
	AudioClip m_acHitSound;

	Vector3 m_vtTargetPos;
	Guid m_guidPlaceInstanceId;
	Guid m_guTamerId;

	Vector3 vtStartTamePos;
	float m_flStartTameRotationY;
	float m_flBattleMoveSpeed;
	float m_flTameRadius;

	List<CsMonsterSkill> m_listMonsterSkill = new List<CsMonsterSkill>();

	public Vector3 TargetPos { get { return m_vtTargetPos; } set { m_vtTargetPos = value; } }
	public float BattleMoveSpeed { get { return m_flBattleMoveSpeed; } }
	public float TameRadius { get { return m_flTameRadius; } }
	public Guid PlaceInstanceId { get { return m_guidPlaceInstanceId; } set { m_guidPlaceInstanceId = value; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(CsMonster csMonster, Guid guTamerId)
	{
		Debug.Log("####################                       CsDominateObject.Init()                       ##################################");
		CsIngameData.Instance.TameMonster = this;
		InstanceId = csMonster.InstanceId;
		m_flBattleMoveSpeed = csMonster.MonsterInfo.BattleMoveSpeed;
		
		m_trMyHero = CsGameData.Instance.MyHeroTransform;
		gameObject.layer = m_trMyHero.gameObject.layer;
		transform.tag = m_trMyHero.tag;
		transform.localScale = new Vector3(csMonster.MonsterInfo.Scale, csMonster.MonsterInfo.Scale, csMonster.MonsterInfo.Scale);
		m_animator.SetBool(s_nAnimatorHash_dominate, true);
		m_animator.SetTrigger(s_nAnimatorHash_groggy);
		m_capsuleCollider.radius = csMonster.MonsterInfo.StealRadius - 0.5f;
		m_flTameRadius = csMonster.MonsterInfo.StealRadius;
		m_guTamerId = guTamerId;

		m_capsuleCollider.enabled = false;
		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		m_audioSource = transform.GetComponent<AudioSource>();
		m_asAttack01 = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Monster/SFX_Mon_ImusChieftain_Attack01"); 
		m_asAttack02 = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Monster/SFX_Mon_ImusChieftain_Attack02");
		m_asRunRight = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Monster/SFX_Mon_ImusChieftain_Run_Right");
		m_asRunLeft = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Monster/SFX_Mon_ImusChieftain_Run_Left");
		m_acHitSound = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Monster/SFX_Mon_Hit_Public");

		Height = 2.5f;
		ChangeState(EnTameState.Groggy);

		for (int i = 0; i < csMonster.MonsterInfo.MonsterOwnSkillList.Count; i++)
		{
			CsMonsterSkill csMonsterSkill = CsGameData.Instance.GetMonsterSkill(csMonster.MonsterInfo.MonsterOwnSkillList[i].SkillId);
			if (csMonsterSkill != null)
			{
				m_listMonsterSkill.Add(csMonsterSkill);
			}
		}

		CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

		if (acs.Length > 0)
		{
			for (int i = 0; i < acs.Length; i++)
			{
				if ((EnTameStatus)acs[i].Key == EnTameStatus.Idle)
				{
					acs[i].EventStateEnter += OnEventAnimStartIdle;
				}
				else
				{
					acs[i].EventStateEnter += OnEventAnimStartAttack;
				}
			}
		}

		m_rtfTame = CsGameEventToUI.Instance.OnEventCreateTameMonster();				// Tame UI 생성.
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_capsuleCollider = transform.GetComponent<CapsuleCollider>();
		m_animator = transform.GetComponent<Animator>();
		m_trMyTransform = transform;
	}

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		
	}

	//----------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		TameHUDUpdatePos();
	}

	//----------------------------------------------------------------------------------------------------
	void TameHUDUpdatePos()
	{
		if (m_rtfTame == null) return;
		if (CsIngameData.Instance.InGameCamera == null) return;

		if (m_rtfTame.gameObject.activeInHierarchy)
		{
			if (CsIngameData.Instance.InGameCamera.SleepMode == false)
			{
				Vector2 vt2Pos = Camera.main.WorldToScreenPoint(new Vector3(m_trMyTransform.position.x, +m_trMyTransform.position.y, m_trMyTransform.position.z));
				vt2Pos = vt2Pos / CsGameConfig.Instance.ScaleFactor;
				m_rtfTame.anchoredPosition = vt2Pos / CsGameConfig.Instance.ScaleFactor;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		CsGameEventToUI.Instance.OnEventDeleteTameMonster();
		CsDungeonManager.Instance.StoryDungeonTameable(false);
		CsGameEventToIngame.Instance.EventTameMonsterUseSkill -= OnEventTameMonsterUseSkill;
		m_animator = null;
		m_trMyHero = null;
		m_capsuleCollider = null;
		m_listMonsterSkill = null;
		m_csMonsterSkill = null;
		m_rtfTame = null;
		CsIngameData.Instance.TameMonster = null;

		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider col)
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				if (m_guTamerId == CsGameData.Instance.MyHeroInfo.HeroId)
				{
					CsDungeonManager.Instance.StoryDungeonTameable(true);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerExit(Collider col)
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				if (m_guTamerId == CsGameData.Instance.MyHeroInfo.HeroId)
				{
					CsDungeonManager.Instance.StoryDungeonTameable(false);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnTameState enNewTameState)
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			//Debug.Log("##################                           ChangeState     enNewTameState = " + enNewTameState);
			if (enNewTameState == EnTameState.Idle)
			{
				SetAnimStatus(EnTameStatus.Idle);
			}
			else if (enNewTameState == EnTameState.Walk)
			{
				SetAnimStatus(EnTameStatus.Walk);
			}
			else if (enNewTameState == EnTameState.Run)
			{
				SetAnimStatus(EnTameStatus.Walk);
			}
			else if (enNewTameState == EnTameState.Attack)
			{
				SetAnimStatus(EnTameStatus.Skill01);
			}
			else if (enNewTameState == EnTameState.Groggy)
			{
				SetAnimStatus(EnTameStatus.Groggy);
			}
			else if (enNewTameState == EnTameState.Dead)
			{
				SetAnimStatus(EnTameStatus.Dead);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnTameStatus enDominateStatus)
	{
		//Debug.Log("##################                           SetAnimStatus                enDominateStatus = " + enDominateStatus);
		if (enDominateStatus == (EnTameStatus)m_animator.GetInteger(s_nAnimatorHash_status)) return;
		m_animator.SetInteger(s_nAnimatorHash_status, (int)enDominateStatus);
	}

	EnTameStatus GetAnimStatus() { return (EnTameStatus)m_animator.GetInteger(s_nAnimatorHash_status); }
	bool IsAnimStatusDead() { return EnTameStatus.Dead == (EnTameStatus)m_animator.GetInteger(s_nAnimatorHash_status); }

	//---------------------------------------------------------------------------------------------------
	void SendTamingMonsterSkillCast(int nSkillId)
	{
		//Debug.Log("SendTamingMonsterSkillCast               nSkillId = " + nSkillId);
		CEBTamingMonsterSkillCastEventBody csEvt = new CEBTamingMonsterSkillCastEventBody();
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		csEvt.skillId = m_csMonsterSkill.SkillId;
		csEvt.skillTargetPosition = CsRplzSession.Translate(m_vtTargetPos);
		CsRplzSession.Instance.Send(ClientEventName.TamingMonsterSkillCast, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendTamingMonsterSkillHit(PDHitTarget[] apDHitTarget, int nHitId)
	{
		//Debug.Log("SendTamingMonsterSkillHit    SkillId =  " + m_csMonsterSkill.SkillId + " //  apDHitTarget =  " + apDHitTarget.Length);
		CEBTamingMonsterSkillHitEventBody csEvt = new CEBTamingMonsterSkillHitEventBody();
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		csEvt.skillId = m_csMonsterSkill.SkillId;
		csEvt.hitId = nHitId;
		csEvt.targets = apDHitTarget;
		CsRplzSession.Instance.Send(ClientEventName.TamingMonsterSkillHit, csEvt);
	}

	#region Anim

	bool m_bAttack = false;
	public bool IsAttack { get { return m_bAttack; } }
	//---------------------------------------------------------------------------------------------------
	void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			if (m_bAttack)
			{
				m_bAttack = false;
				CsIngameData.Instance.IngameManagement.TrasfomationAttackEnd();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			m_bAttack = true;
			CsIngameData.Instance.IngameManagement.TrasfomationAttackStart();			
			if (nKey == (int)EnTameStatus.Skill01)
			{
				m_csMonsterSkill = m_listMonsterSkill[0];
			}
			else if (nKey == (int)EnTameStatus.Skill02)
			{
				m_csMonsterSkill = m_listMonsterSkill[1];
			}

			SendTamingMonsterSkillCast(m_csMonsterSkill.SkillId);
			StartCoroutine(DelaySendSelectMonster());

			if (nKey ==3)
			{
				m_audioSource.PlayOneShot(m_asAttack01);
			}
			else if (nKey == 4)
			{
				m_audioSource.PlayOneShot(m_asAttack02);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelaySendSelectMonster()
	{
		yield return new WaitForSeconds(0.4f);
		FindAttackTarget(1);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId)
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			if (CsIngameData.Instance.EffectSound && m_audioSource != null)
			{
				m_audioSource.PlayOneShot(m_acHitSound);
			}

			if (m_csMonsterSkill == null) return;
			CsIngameData.Instance.InGameCamera.DoShake(4, false);
			if (m_csMonsterSkill.SkillId == m_listMonsterSkill[0].SkillId)
			{
				CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Gaia_Skill_01_03", 1.5f);
			}
			else
			{
				CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position + transform.forward * 5, "Witch_Attack_03_02", 1.0f);
			}

		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadEnd()
	{
		if (CsIngameData.Instance.TameMonster == this)
		{
			StartCoroutine(CsDissolveHelper.LinearDissolve(transform, 0f, 1f, 2f));
			Invoke(SendMonsterDead, 3f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound()
	{
	}

	//---------------------------------------------------------------------------------------------------
	void SendMonsterDead()
	{
		GameObject.Destroy(this);
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventTameMonsterUseSkill()
	{
		//Debug.Log("OnEventTameMonsterUseSkill()");
		if (m_bAttack) return false; // 공격중.
		m_csMonsterSkill = m_listMonsterSkill[0];

		if (CsIngameData.Instance.TargetTransform == null)
		{
			m_vtTargetPos = CsGameData.Instance.MyHeroTransform.position;
			return true;
		}
		else
		{
			m_vtTargetPos = CsIngameData.Instance.TargetTransform.position;
			if (IsTargetInDistance(m_vtTargetPos, m_csMonsterSkill.CastRange))
			{
				return true; // 공격.
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void FindAttackTarget(int nHitId)
	{
		int nLayerMask = (1 << LayerMask.NameToLayer(CsMonster.c_strLayer));

		List<PDHitTarget> listPDHitTarget = new List<PDHitTarget>();
		Vector3 vtCenter;

		switch ((EnHitAreaType)m_csMonsterSkill.HitAreaType) // 적중범위유형(1:원(반지름,각도), 2:사각형(가로, 세로))
		{
			case EnHitAreaType.Circle: // 부채꼴 포함.
				vtCenter = ((EnHitAreaOffsetType)m_csMonsterSkill.HitAreaOffsetType == EnHitAreaOffsetType.Person) ? transform.position : m_vtTargetPos;
				vtCenter = vtCenter + (transform.forward * m_csMonsterSkill.HitAreaOffset);
				Collider[] acollider = Physics.OverlapSphere(vtCenter, m_csMonsterSkill.HitAreaValue1, nLayerMask);

				for (int i = 0; i < acollider.Length; i++)
				{
					Transform trTarget = acollider[i].transform;
					if ((EnHitAreaType)m_csMonsterSkill.HitAreaType == EnHitAreaType.Circle)
					{
						Vector3 vtDir = (trTarget.position - transform.position).normalized;
						vtDir.y = 0;

						if (Vector3.Angle(transform.forward, vtDir) > m_csMonsterSkill.HitAreaValue2 / 2) continue; // 써클형 범위에서 벗어나면 다음타겟 확인.
					}

					if (trTarget.CompareTag(CsMonster.c_strTag))
					{
						listPDHitTarget.Add(new PDMonsterHitTarget(CsIngameData.Instance.IngameManagement.GetInstanceId(trTarget)));
					}
				}
				break;

			case EnHitAreaType.Rectangle:
				vtCenter = transform.position + (transform.forward * m_csMonsterSkill.HitAreaValue2 / 2);
				Vector3 vtHalfExtents = new Vector3(m_csMonsterSkill.HitAreaValue1 / 2, 2, m_csMonsterSkill.HitAreaValue2 / 2);
				Quaternion quaternion = Quaternion.LookRotation(transform.forward);
				RaycastHit[] araycastHit = Physics.BoxCastAll(vtCenter, vtHalfExtents, transform.forward, quaternion, 0, nLayerMask);

				for (int i = 0; i < araycastHit.Length; i++)
				{
					Transform trTarget = araycastHit[i].transform;
					if (trTarget.CompareTag(CsMonster.c_strTag))
					{
						listPDHitTarget.Add(new PDMonsterHitTarget(CsIngameData.Instance.IngameManagement.GetInstanceId(trTarget)));
					}
				}
				break;
		}

		PDHitTarget[] apDHitTarget = listPDHitTarget.ToArray();
		SendTamingMonsterSkillHit(apDHitTarget, nHitId);
	}

	//---------------------------------------------------------------------------------------------------
	public float GetCastRange()
	{
		if (m_listMonsterSkill == null) return 0;
		
		return m_listMonsterSkill[0].CastRange;
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayBattle(Transform trTarget)
	{
		if (m_bAttack) return;
		Debug.Log("PlayBattle    trTarget =  " + trTarget);
		if (trTarget == null)
		{
			m_vtTargetPos = CsGameData.Instance.MyHeroTransform.position;
		}
		else
		{
			m_vtTargetPos = trTarget.position;
		}

		m_csMonsterSkill = m_listMonsterSkill[0];
		ChangeState(EnTameState.Attack);
	}

	//---------------------------------------------------------------------------------------------------
	public void SetTame(Vector3 vtPos, float flRotationY)
	{
		vtStartTamePos = vtPos;
		m_flStartTameRotationY = -flRotationY;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartTaming()
	{
		Debug.Log("StartTaming     m_flStartTameRotationY = " + m_flStartTameRotationY);
		GameObject.Destroy(transform.GetComponent<CsMonster>());
		transform.GetChild(1).gameObject.layer = CsGameData.Instance.MyHeroTransform.gameObject.layer;
		transform.position = vtStartTamePos;		
		ChangeEulerAngles(m_flStartTameRotationY);
		m_capsuleCollider.enabled = false;
		CsGameEventToUI.Instance.OnEventDeleteTameMonster();
	}

	//---------------------------------------------------------------------------------------------------
	public void TameEnd()
	{
		CsGameEventToUI.Instance.OnEventTameButton(false);
		m_animator.SetBool(s_nAnimatorHash_dominate, false);
		m_goTameEffect.SetActive(false);
		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Tayming03", 1f);
		Destroy(gameObject);
	}

	GameObject m_goTameEffect = null;
	//---------------------------------------------------------------------------------------------------
	public void TameMonsterView(bool bView)
	{
		if (bView)
		{
			transform.Find("mon_EimusChief").gameObject.SetActive(bView);
			StartCoroutine(CsDissolveHelper.LinearDissolve(transform, 0.5f, 0f, 1.5f));
			m_goTameEffect = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Effect/RFX_tayming_head"), transform) as GameObject;
			m_goTameEffect.SetActive(true);
		}
		else
		{
			transform.Find("mon_EimusChief").gameObject.SetActive(bView);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRunRightSound()
	{
		m_audioSource.PlayOneShot(m_asRunRight);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRunLeftSound()
	{
		m_audioSource.PlayOneShot(m_asRunLeft);
	}

	#endregion Anim
}

