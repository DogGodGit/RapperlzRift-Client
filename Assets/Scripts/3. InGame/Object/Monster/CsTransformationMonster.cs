using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnTransformationMonsterState { Idle = 0, Walk = 1, Run = 2, Attack = 3, Dead = 7 , Interact = 10 }

public class CsTransformationMonster : CsMoveUnit
{
	enum EnTransformationMonsterStatus { Idle = 0, Walk, Run, Skill01, Skill02, Skill03, Skill04, Dead, Interact = 10 }
	static int s_nAnimatorHash_status = Animator.StringToHash("status");

	CsMonsterInfo m_csMonsterInfo;
	CsMonsterSkill m_csMonsterSkill;
	Transform m_trMyHero;
	Transform m_trMyTransform;

	AudioSource m_audioSource;
	AudioClip m_asAttack01;
	AudioClip m_asAttack02;
	AudioClip m_asRunRight;
	AudioClip m_asRunLeft;
	AudioClip m_acHitSound;

	Vector3 m_vtTargetPos;
	float m_flBattleMoveSpeed;
	bool m_bMyHeroTransfomationMonster = false;
	Guid m_guidPlaceInstanceId;

	CsMonsterSkill m_csDefaultMonsterSkill = null;
	List<CsMonsterSkill> m_listMonsterSkill = new List<CsMonsterSkill>();
	EnTransformationMonsterState m_enTransformationMonsterState = EnTransformationMonsterState.Idle;
	
	public CsMonsterInfo MonsterInfo { get { return m_csMonsterInfo; } }
	public CsMonsterSkill DefaultMonsterSkill { get { return m_csDefaultMonsterSkill; } }
	public Vector3 TargetPos { get { return m_vtTargetPos; } set { m_vtTargetPos = value; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(CsMonsterInfo csMonsterInfo, Transform trHero, Guid guidPlaceInstanceId)
	{
		Debug.Log("####################                       CsTransformationMonster.Init()                       ##################################");
		m_trMyTransform = transform;
		m_animator = m_trMyTransform.GetComponent<Animator>();
		m_audioSource = m_trMyTransform.GetComponent<AudioSource>();

		m_csMonsterInfo = csMonsterInfo;
		m_guidPlaceInstanceId = guidPlaceInstanceId;
		m_flBattleMoveSpeed = csMonsterInfo.BattleMoveSpeed;

		m_trMyHero = trHero;
		gameObject.layer = m_trMyHero.gameObject.layer;
		m_trMyTransform.tag = m_trMyHero.tag;
		m_trMyTransform.localScale = new Vector3(csMonsterInfo.Scale, csMonsterInfo.Scale, csMonsterInfo.Scale);
		m_trMyTransform.GetChild(1).gameObject.layer = m_trMyHero.gameObject.layer;
		Height = 1.5f;

		for (int i = 0; i < csMonsterInfo.MonsterOwnSkillList.Count; i++)
		{
			CsMonsterSkill csMonsterSkill = CsGameData.Instance.GetMonsterSkill(csMonsterInfo.MonsterOwnSkillList[i].SkillId);
			if (csMonsterSkill != null)
			{
				m_listMonsterSkill.Add(csMonsterSkill);
				if (m_csDefaultMonsterSkill == null)
				{
					m_csDefaultMonsterSkill = csMonsterSkill;
				}
			}
		}

		CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

		Debug.Log("acs.Length = " + acs.Length);
		if (acs.Length > 0)
		{
			for (int i = 0; i < acs.Length; i++)
			{
				if ((EnTransformationMonsterStatus)acs[i].Key == EnTransformationMonsterStatus.Idle)
				{
					Debug.Log("OnEventAnimStartIdle");
					acs[i].EventStateEnter += OnEventAnimStartIdle;
				}
				else
				{
					Debug.Log("OnEventAnimStartAttack");
					acs[i].EventStateEnter += OnEventAnimStartAttack;
				}
			}
		}

		m_bMyHeroTransfomationMonster = CsGameData.Instance.MyHeroTransform == trHero ? true : false;
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_enTransformationMonsterState == EnTransformationMonsterState.Walk)
		{
			if (CsGameData.Instance.JoysticWalk == false)
			{
				ChangeState(EnTransformationMonsterState.Run);
			}
		}
		else if (m_enTransformationMonsterState == EnTransformationMonsterState.Run)
		{
			if (CsGameData.Instance.JoysticWalk)
			{
				ChangeState(EnTransformationMonsterState.Walk);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_animator != null)
		{
			CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

			if (acs.Length > 0)
			{
				for (int i = 0; i < acs.Length; i++)
				{
					if ((EnTransformationMonsterStatus)acs[i].Key == EnTransformationMonsterStatus.Idle)
					{
						acs[i].EventStateEnter -= OnEventAnimStartIdle;
					}
					else
					{
						acs[i].EventStateEnter -= OnEventAnimStartAttack;
					}
				}
			}
		}

		m_csMonsterInfo = null;
		m_csMonsterSkill = null;
		m_trMyHero = null;
		m_trMyTransform = null;
		m_csDefaultMonsterSkill = null;
		m_listMonsterSkill.Clear();

		m_audioSource = null;
		m_asAttack01 = null;
		m_asAttack02 = null;
		m_asRunRight = null;
		m_asRunLeft = null;
		m_acHitSound = null;

		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnTransformationMonsterState enNewTransformationMonsterState)
	{
		if (m_enTransformationMonsterState == EnTransformationMonsterState.Interact)
		{
			OnStateEndOfInteraction();
		}

		if (enNewTransformationMonsterState == EnTransformationMonsterState.Idle)
		{
			SetAnimStatus(EnTransformationMonsterStatus.Idle);
		}
		else if (enNewTransformationMonsterState == EnTransformationMonsterState.Walk || enNewTransformationMonsterState == EnTransformationMonsterState.Run)
		{
			if (CsGameData.Instance.JoysticWalk)
			{
				SetAnimStatus(EnTransformationMonsterStatus.Walk);
			}
			else
			{
				SetAnimStatus(EnTransformationMonsterStatus.Run);
			}
		}
		else if (enNewTransformationMonsterState == EnTransformationMonsterState.Attack)
		{
			if (m_csMonsterSkill == null) return;
			for (int i = 0; i < m_listMonsterSkill.Count; i++)
			{
				if (m_listMonsterSkill[i].SkillId == m_csMonsterSkill.SkillId)
				{
					if (i == 0)
					{
						SetAnimStatus(EnTransformationMonsterStatus.Skill01);
						break;
					}
					else
					{
						SetAnimStatus(EnTransformationMonsterStatus.Skill02);
						break;
					}
				}
			}
		}
		else if (enNewTransformationMonsterState == EnTransformationMonsterState.Interact)
		{
			SetAnimStatus(EnTransformationMonsterStatus.Interact);
		}
		else if (enNewTransformationMonsterState == EnTransformationMonsterState.Dead)
		{
			SetAnimStatus(EnTransformationMonsterStatus.Dead);
		}

		m_enTransformationMonsterState = enNewTransformationMonsterState;
	}

	//---------------------------------------------------------------------------------------------------
	void OnStateEndOfInteraction()
	{
		if (m_trMyHero == CsGameData.Instance.MyHeroTransform)
		{
			CsIngameData.Instance.IngameManagement.StateEndOfInteraction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void TransformationMonsterSkillCast(CsMonsterSkill csMonsterSkill)
	{
		m_csMonsterSkill = csMonsterSkill;
		ChangeState(EnTransformationMonsterState.Attack);
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnTransformationMonsterStatus enTransformationMonsterStatus)
	{
		if (enTransformationMonsterStatus == (EnTransformationMonsterStatus)m_animator.GetInteger(s_nAnimatorHash_status)) return;
		m_animator.SetInteger(s_nAnimatorHash_status, (int)enTransformationMonsterStatus);
	}

	EnTransformationMonsterStatus GetAnimStatus() { return (EnTransformationMonsterStatus)m_animator.GetInteger(s_nAnimatorHash_status); }
	bool IsAnimStatusDead() { return EnTransformationMonsterStatus.Dead == (EnTransformationMonsterStatus)m_animator.GetInteger(s_nAnimatorHash_status); }


	//---------------------------------------------------------------------------------------------------
	void TransformationMonsterSkillCast(Guid guidPlaceInstanceId, int nSkillId, Vector3 vtTargetPos)
	{
		if (CsIngameData.Instance.IngameManagement.IsContinent())
		{
			MainQuestTransformationMonsterSkillCast(m_guidPlaceInstanceId, m_csMonsterSkill.SkillId, vtTargetPos);
		}
		else
		{
			WarMemoryTransformationMonsterSkillCast(m_guidPlaceInstanceId, m_csMonsterSkill.SkillId, vtTargetPos);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void TransformationMonsterSkillHit(Guid guidPlaceInstanceId, int nSkillId, int nHitId, PDHitTarget[] apHitTarget)
	{
		if (CsIngameData.Instance.IngameManagement.IsContinent())
		{
			MainQuestTransformationMonsterSkillHit(m_guidPlaceInstanceId, m_csMonsterSkill.SkillId, nHitId, apHitTarget);
		}
		else
		{
			WarMemoryTransformationMonsterSkillHit(m_guidPlaceInstanceId, m_csMonsterSkill.SkillId, nHitId, apHitTarget);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void MainQuestTransformationMonsterSkillCast(Guid guidPlaceInstanceId, int nSkillId, Vector3 vtTargetPos)
	{
		CEBMainQuestTransformationMonsterSkillCastEventBody csEvt = new CEBMainQuestTransformationMonsterSkillCastEventBody();
		csEvt.placeInstanceId = guidPlaceInstanceId;
		csEvt.skillId = nSkillId;
		csEvt.skillTargetPosition = CsRplzSession.Translate(vtTargetPos);
		CsRplzSession.Instance.Send(ClientEventName.MainQuestTransformationMonsterSkillCast, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void MainQuestTransformationMonsterSkillHit(Guid guidPlaceInstanceId, int nSkillId, int nHitId, PDHitTarget[] aHitTarget)
	{
		CEBMainQuestTransformationMonsterSkillHitEventBody csEvt = new CEBMainQuestTransformationMonsterSkillHitEventBody();
		csEvt.placeInstanceId = guidPlaceInstanceId;
		csEvt.skillId = nSkillId;
		csEvt.hitId = nHitId;
		csEvt.targets = aHitTarget;

		CsRplzSession.Instance.Send(ClientEventName.MainQuestTransformationMonsterSkillHit, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void WarMemoryTransformationMonsterSkillCast(Guid guidPlaceInstanceId, int nSkillId, Vector3 vtTargetPos)
	{
		CEBWarMemoryTransformationMonsterSkillCastEventBody csEvt = new CEBWarMemoryTransformationMonsterSkillCastEventBody();
		csEvt.placeInstanceId = guidPlaceInstanceId;
		csEvt.skillId = nSkillId;
		csEvt.skillTargetPosition = CsRplzSession.Translate(vtTargetPos);

		CsRplzSession.Instance.Send(ClientEventName.WarMemoryTransformationMonsterSkillCast, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void WarMemoryTransformationMonsterSkillHit(Guid guidPlaceInstanceId, int nSkillId, int nHitId, PDHitTarget[] aHitTarget)
	{
		CEBWarMemoryTransformationMonsterSkillHitEventBody csEvt = new CEBWarMemoryTransformationMonsterSkillHitEventBody();
		csEvt.placeInstanceId = guidPlaceInstanceId;
		csEvt.skillId = nSkillId;
		csEvt.hitId = nHitId;
		csEvt.targets = aHitTarget;

		CsRplzSession.Instance.Send(ClientEventName.WarMemoryTransformationMonsterSkillHit, csEvt);
	}

	#region Anim

	bool m_bAttack = false;
	public bool IsAttack { get { return m_bAttack; } }
	//---------------------------------------------------------------------------------------------------
	void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		if (m_bMyHeroTransfomationMonster)
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
		if (m_bMyHeroTransfomationMonster)
		{
			m_bAttack = true;
			if (nKey == (int)EnTransformationMonsterStatus.Skill01)
			{
				m_csMonsterSkill = m_listMonsterSkill[0];
				//m_audioSource.PlayOneShot(m_asAttack01);
			}
			else if (nKey == (int)EnTransformationMonsterStatus.Skill02)
			{
				m_csMonsterSkill = m_listMonsterSkill[1];
				//m_audioSource.PlayOneShot(m_asAttack02);
			}

			CsIngameData.Instance.IngameManagement.TrasfomationAttackStart();
			Vector3 vtTatrget = CsIngameData.Instance.TargetTransform == null ? transform.position : CsIngameData.Instance.TargetTransform.position;

			TransformationMonsterSkillCast(m_guidPlaceInstanceId, m_csMonsterSkill.SkillId, vtTatrget);
			StartCoroutine(DelaySendSelectMonster());
		}
		ChangeState(EnTransformationMonsterState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId)
	{
		if (m_bMyHeroTransfomationMonster)
		{
			if (CsIngameData.Instance.EffectSound && m_audioSource != null)
			{
				m_audioSource.PlayOneShot(m_acHitSound);
			}

			if (m_csMonsterSkill == null) return;
			//CsIngameData.Instance.InGameCamera.DoShake(4, false);
			if (m_csMonsterSkill.SkillId == m_listMonsterSkill[0].SkillId)
			{
				//CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Gaia_Skill_01_03", 1.5f);
			}
			else
			{
				//CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position + transform.forward * 5, "Witch_Attack_03_02", 1.0f);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadEnd()
	{
		if (m_bMyHeroTransfomationMonster)
		{
			StartCoroutine(CsDissolveHelper.LinearDissolve(m_trMyTransform, 0f, 1f, 2f));
			Invoke(SendMonsterDead, 3f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound()
	{
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelaySendSelectMonster()
	{
		yield return new WaitForSeconds(0.4f);
		FindAttackTarget(1);
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

					Vector3 vtDir = (trTarget.position - transform.position).normalized;
					vtDir.y = 0;
					if (Vector3.Angle(transform.forward, vtDir) > m_csMonsterSkill.HitAreaValue2 / 2) continue; // 써클형 범위에서 벗어나면 다음타겟 확인.

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
		TransformationMonsterSkillHit(m_guidPlaceInstanceId, m_csMonsterSkill.SkillId, nHitId, apDHitTarget);
	}

	//---------------------------------------------------------------------------------------------------
	void SendMonsterDead()
	{
		GameObject.Destroy(this);
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
