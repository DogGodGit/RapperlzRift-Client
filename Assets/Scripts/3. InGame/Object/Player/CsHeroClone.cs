using ClientCommon;
using SimpleDebugLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CsHeroClone : CsHero, IHeroObjectInfo
{
	bool m_bDungeonStart = false;
	int m_nSamePosMoveCount = 0;

	CsSkills m_csSkills = new CsSkills();
	CsHitSkillCast m_csHitSkillCast = null;
	Transform m_trTargetEnemy = null;
	
	public override Guid HeroId { get { return m_csHeroBase == null ? Guid.Empty : m_csHeroBase.HeroId; } }
	public override CsJob Job { get { return m_csHeroBase == null ? null : m_csHeroBase.Job; } }
	public override int NationId { get { return m_csHeroBase == null ? 0 : m_csHeroBase.Nation.NationId; } }
	public override int Rank { get { return m_csHeroBase == null ? 0 : m_csHeroBase.RankNo; } }
	public override string Name { get { return m_csHeroBase == null ? null : m_csHeroBase.Name; } }
	public override int Level { get { return m_csHeroBase == null ? 0 : m_csHeroBase.Level; } set { m_csHeroBase.Level = value; OnStateChanged(); } }
	public override int MaxHp { get { return m_csHeroBase == null ? 0 : m_csHeroBase.MaxHp; } set { m_csHeroBase.MaxHp = value; OnStateChanged(); } }
	public override int Hp { get { return m_csHeroBase == null ? 0 : m_csHeroBase.Hp; } set { m_csHeroBase.Hp = value; OnStateChanged(); } }

	//---------------------------------------------------------------------------------------------------
	class CsSkills
	{
		static int[] s_anAutoSkillIndex = { 3, 1, 2, 0 };		
		//---------------------------------------------------------------------------------------------------
		class CsSkillWithCoolTime
		{
			CsJobSkill m_csJobSkill;
			float m_flTime;

			public CsSkillWithCoolTime(CsJobSkill csJobSkill)
			{
				m_csJobSkill = csJobSkill;
				m_flTime = 0;
			}

			public CsJobSkill JobSkill { get { return m_csJobSkill; } }
			public float TimeLast { get { return m_flTime; } }

			public bool UseSkill()
			{
				if (Time.time < m_flTime)
				{
					return false;
				}
				m_flTime = Time.time + m_csJobSkill.CoolTime;
				return true;
			}
		}

		public void Add(CsJobSkill csJobSkill)
		{
			m_list.Add(new CsSkillWithCoolTime(csJobSkill));
		}

		List<CsSkillWithCoolTime> m_list = new List<CsSkillWithCoolTime>();

		public CsJobSkill GetUsableSkill()
		{
			int nCount = s_anAutoSkillIndex.Length;

			for (int i = 0; i < nCount; i++)
			{
				int nIndex = s_anAutoSkillIndex[i];

				if (nIndex < m_list.Count)
				{
					CsSkillWithCoolTime cs = m_list[nIndex];
					if (cs.UseSkill())
					{
						return cs.JobSkill;
					}
				}
			}

			return null;
		}
	}

	public static PDHero TempMakePDHero(Vector3 vt)
	{
		PDHero c = new PDHero();
		CsMyHeroInfo m = CsGameData.Instance.MyHeroInfo;

		int nJobId = m.Job.ParentJobId == 0 ? m.Job.JobId : m.Job.ParentJobId;
		c.jobId = nJobId;
		c.name = m.Name;
		c.id = m.HeroId;
		c.level = m.Level;
		c.vipLevel = m.VipLevel.VipLevel;
		c.maxHP = m.MaxHp;
		c.rankNo = m.RankNo;
		c.nationId = m.Nation.NationId;
		c.rotationY = 0;
		c.position = CsRplzSession.Translate(vt);
		c.equippedWingId = m.EquippedWingId;
		c.guildId = System.Guid.Empty;
		c.guildMemberGrade = 0;
		c.guildName = "";

		return c;
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHero pDHero, Guid guidPlaceInstanceId)
	{
		m_csHeroBase = new CsHeroBase(pDHero.id, pDHero.name, pDHero.nationId, pDHero.jobId, pDHero.level, pDHero.hp, pDHero.maxHP, pDHero.rankNo);
		
		for (int i = 0; i < CsGameData.Instance.JobSkillList.Count; i++)
		{
			if (CsGameData.Instance.JobSkillList[i].JobId == pDHero.jobId)
			{
				m_csSkills.Add(CsGameData.Instance.JobSkillList[i]);
			}
		}

		m_guidPlaceInstanceId = guidPlaceInstanceId;
		VipLevel = pDHero.vipLevel;
		m_flRotationY = pDHero.rotationY;

		transform.name = Name;
		transform.position = m_vtMovePos = CsRplzSession.Translate(pDHero.position);
		ChangeEulerAngles(m_flRotationY);

		gameObject.layer = LayerMask.NameToLayer("Hero");
		transform.tag = "Hero";

		m_csHeroCustomData = new CsHeroCustomData(pDHero);
		m_csEquipment.LowChangEquipments(m_csHeroCustomData);

		// 날개
		m_csEquipment.CreateWing(m_csHeroCustomData, m_trPivotHUD);

		CreateShadow();
		NavMeshSetting();
		m_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
		m_navMeshAgent.avoidancePriority = 30;

		CsGameData.Instance.ListHeroObjectInfo.Add(this);

		int nNoblesseId = 0;
		CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationInstance(pDHero.nationId).GetNationNoblesseInstanceByHeroId(pDHero.id);
		if (csNationNoblesseInstance != null)
		{
			nNoblesseId = csNationNoblesseInstance.NoblesseId;
		}

		m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateHeroHUD(m_csHeroBase,
																 pDHero.guildName,
																 pDHero.guildMemberGrade,
																 pDHero.pickedSecretLetterGrade,
																 pDHero.pickedMysteryBoxGrade,
																 pDHero.isDistorting,
																 false,
																 0,
																 nNoblesseId,
																 pDHero.displayTitleId);

		m_animator.SetFloat(s_nAnimatorHash_move, 1);
		m_animator.SetInteger(s_nAnimatorHash_job, 0); // 0 기본, 1 전직1, 2 전직2
		StartCoroutine(SendMoveState()); // 서버에 플레이어 좌표 정보 전달 시작.

		m_audioSource.volume = 0.2f;
		m_audioSourceParent.volume = 0.2f;

		Debug.Log("#####     InitOtherHero     #####     strName = " + Name + " // Id = " + HeroId);
	}

	#region 01 MonoBehavier

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_audioSource.volume = 0.2f;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		base.OnDestroy();
		CsGameEventToUI.Instance.OnEventSelectHeroInfoStop();

		m_csSkills = null;
		m_csHeroBase = null;
		m_csHitSkillCast = null;
		m_trTargetEnemy = null;

		CsGameData.Instance.ListHeroObjectInfo.Remove(this);
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_bDungeonStart == false) return;

		UpdateAutoPlay();		

		if (IsStateKindOfMove(m_enState))
		{
			switch (m_enState)
			{
			case EnState.MoveForSkill:
				if (m_trTargetEnemy == null || m_csSkillStatus.JobSkill == null)
				{
					ChangeState(EnState.Idle);
					break;
				}

				if (Vector3.Distance(m_vtMovePos, m_trTargetEnemy.position) > 0.1f) // 타겟 위치 갱신.
				{
					SetDestination(m_trTargetEnemy.position, GetAttackCastRange(m_trTargetEnemy, m_csSkillStatus.JobSkill.CastRange));
				}

				if (UpdateMove(m_trTargetEnemy.position, GetAttackCastRange(m_trTargetEnemy, m_csSkillStatus.JobSkill.CastRange)))
				{
					m_csSkillStatus.SetStatusToPlay();
					Attack();
				}
				break;

			case EnState.MoveToPos:
				if (!CheckHasPath(EnState.MoveToPos)) break;

				if (UpdateMove(m_vtMovePos, m_flMoveStopRange))
				{
					ChangeState(EnState.Idle);
				}
				break;
			}
		}

		m_vtPrevPos = transform.position;
	}

	#endregion 01 MonoBehavier

	bool IsMovable() { return !Dead && !m_bStun && !m_bBlockade; }
	bool IsAttackable() { return !Dead && !m_bStun; }
	//---------------------------------------------------------------------------------------------------
	public override bool ChangeState(EnState enNewState)
	{
		if (m_csSkillStatus.IsStatusPlayAnim() && enNewState != EnState.Attack)
		{
			if (enNewState == EnState.Dead) return base.ChangeState(enNewState);
			if (m_csSkillStatus.IsStatusPlay()) return false;
			if (m_csSkillStatus.Chained) // 연속기 입력 상태에서 이동관련 입력.
			{
				SetAnimStatus(EnAnimStatus.Idle);
			}
//			m_csSkillStatus.SetNextSkill(null);
			return false;
		}

		if (m_enState != enNewState)
		{
			switch (m_enState)
			{
			case EnState.Idle:
				break;
			case EnState.MoveToPos:
				ResetPathOfNavMeshAgent();
				break;
			case EnState.MoveForSkill:
				ResetPathOfNavMeshAgent();
				StopSkill();
				break;
			}

			bool bRet = base.ChangeState(enNewState);

			return bRet;
		}

		return base.ChangeState(enNewState);
	}

	//---------------------------------------------------------------------------------------------------
	void SendSkillCastEvent(Vector3 vtSkillMovePos)
	{
		if (m_csSkillStatus.JobSkill == null) return;

		CEBSkillCastEventBody csEvt = new CEBSkillCastEventBody();
		csEvt.chainSkillId = m_csSkillStatus.GetChainSkillId();
		csEvt.heroId = HeroId;
		csEvt.heroTargetPosition = CsRplzSession.Translate(vtSkillMovePos); // 스킬 이동 위치.
		csEvt.heroTargetRotationY = transform.eulerAngles.y;
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		csEvt.skillId = m_csSkillStatus.JobSkill.SkillId;
		csEvt.skillTargetPosition = CsRplzSession.Translate(m_vtTargetPos); // 타겟 대상 위치.

		if (m_csSkillStatus.JobSkill.HeroHitType != 0) // 영웅적중타입(0:없음(영웅은 안맞음) 1:단일, 2:다중)
		{
			csEvt.targetHeroId = CsGameData.Instance.MyHeroInfo.HeroId;
		}
		CsRplzSession.Instance.Send(ClientEventName.SkillCast, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendSkillHitEvent(PDHitTarget[] apDHitTarget, int nHitId)
	{
		CEBSkillHitEventBody csEvt = new CEBSkillHitEventBody();
		csEvt.chainSkillId = m_csSkillStatus.GetChainSkillId();
		csEvt.heroId = HeroId;
		csEvt.hitId = nHitId;
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		csEvt.skillId = m_csSkillStatus.JobSkill.SkillId;

		csEvt.targets = apDHitTarget;
		CsRplzSession.Instance.Send(ClientEventName.SkillHit, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHitApprove(PDHitResult HitResult, Transform trTarget) // 공격할 대상에대한 정보 저장.
	{
		//Debug.Log("1. ##############         CsHeroClone       NetEventHitApprove      trTarget = " + trTarget + " // HitResult = " + HitResult);
		CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(Job.JobId, HitResult.skillId);
		if (csJobSkill != null && csJobSkill.TypeEnum == EnJobSkillType.AreaOfEffect)
		{
			CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult, Time.time, null, false);
			AttackHitEffect(trTarget);
			return;
		}

		if (m_csSkillStatus.JobSkill == null) return;
		m_csHitSkillCast.HitInfoNodeList.Add(new CsHitInfoNode(trTarget, HitResult, Time.time));
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAbnormalStateEffectHit(int nHp, long[] alRemovedAbnormalStateEffects, long lAbnormalStateEffectInstanceId, int nDamage, int nHpDamage, Transform trAttacker)
	{
		if (!HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects))
		{
			ChangeTransformationState(EnTransformationState.None);
			Dead = true;
			Invoke(DelayDead, 0.2f);
			m_capsuleCollider.enabled = false;
			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnStateChanged()
	{
		if (CsIngameData.Instance.TargetTransform == transform) // 선택된 플레이어 정보 전달.
		{
			CsGameEventToUI.Instance.OnEventSelectInfoUpdate();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroDead(Transform trKiller, PDHitResult pDHitResult)
	{
		Debug.Log("CsHEroClone.NetEventTargetHeroDead");
		SetDamageText(pDHitResult, false);
		Dead = true;

		m_capsuleCollider.enabled = false;
		CsIngameData.Instance.IngameManagement.TartgetReset();
		Invoke(DelayDead, 2f);
	}


	#region 04 FindTarget

	//---------------------------------------------------------------------------------------------------
	// 서버에 전달할 유효한 타겟 리스트 생성.
	//---------------------------------------------------------------------------------------------------
	void FindAttackTarget(int nHitId)
	{
		int nLayerMask = (1 << LayerMask.NameToLayer("Player"));

		EnHitAreaType enHitAreaType = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaTypeEnum : m_csSkillStatus.JobSkill.HitAreaTypeEnum; // 연계스킬 or 일반 스킬.
		float flHitAreaValue1 = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaValue1 : m_csSkillStatus.JobSkill.HitAreaValue1; // 연계스킬 or 일반 스킬.
		float flHitAreaValue2 = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaValue2 : m_csSkillStatus.JobSkill.HitAreaValue2; // 연계스킬 or 일반 스킬.

		List<PDHitTarget> listPDHitTarget = new List<PDHitTarget>();
		Collider[] acollider = new Collider[0];
		Vector3 vtCenter;

		switch (enHitAreaType) // 적중범위유형(1:원(반지름,각도), 2:사각형(가로, 세로))
		{
		case EnHitAreaType.Circle: // 부채꼴 포함.
			EnHitAreaOffsetType enHitAreaOffsetType = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaOffsetTypeEnum : m_csSkillStatus.JobSkill.HitAreaOffsetTypeEnum;
			float flOffest = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaOffset : m_csSkillStatus.JobSkill.HitAreaOffset; // 연계스킬 or 일반 스킬.

			vtCenter = (enHitAreaOffsetType == EnHitAreaOffsetType.Person) ? transform.position : m_vtTargetPos;
			vtCenter = vtCenter + (transform.forward * flOffest);
			acollider = Physics.OverlapSphere(vtCenter, flHitAreaValue1, nLayerMask);				
			break;

		case EnHitAreaType.Rectangle:
			vtCenter = transform.position + (transform.forward * flHitAreaValue2 / 2);
			Vector3 vtHalfExtents = new Vector3(flHitAreaValue1 / 2, 2, flHitAreaValue2 / 2);
			Quaternion quaternion = Quaternion.LookRotation(transform.forward);
			acollider = Physics.OverlapBox(vtCenter, vtHalfExtents, quaternion, nLayerMask);
			break;
		}

		for (int i = 0; i < acollider.Length; i++)
		{
			Transform trTarget = acollider[i].transform;
			if (enHitAreaType == EnHitAreaType.Circle)
			{
				Vector3 vtDir = (trTarget.position - transform.position).normalized;
				vtDir.y = 0;
				if (Vector3.Angle(transform.forward, vtDir) > flHitAreaValue2 / 2) continue; // 써클형 범위에서 벗어나면 다음타겟 확인.
			}

			if (trTarget.CompareTag("Player"))
			{
				PDHeroHitTarget p = new PDHeroHitTarget();
				p.heroId = m_csMyHeroInfo.HeroId;
				listPDHitTarget.Add(p);
			}
		}

		PDHitTarget[] apDHitTarget = new PDHitTarget[listPDHitTarget.Count];
		for (int i = 0; i < listPDHitTarget.Count; i++)
		{
			apDHitTarget[i] = listPDHitTarget[i];
		}

		SendSkillHitEvent(apDHitTarget, nHitId);
	}

	#endregion 04 FindTarget

	#region 05 Move

	Vector3 m_vtLastSendPos;
	float m_flLastSendAngle;
	//---------------------------------------------------------------------------------------------------
	void MyHeroMoveStart()
	{
		if (m_bMoving) return;

		m_bMoving = true;
		SendMoveStartEvent(false, false);
	}

	//---------------------------------------------------------------------------------------------------
	bool MyHeroMove()
	{
		if (!m_bMoving || (transform.position == m_vtLastSendPos && transform.eulerAngles.y == m_flLastSendAngle))
		{
			return false;
		}

		m_vtLastSendPos = transform.position;
		m_flLastSendAngle = transform.eulerAngles.y;

		m_flRotationY = m_flLastSendAngle % 360;
		if (m_flRotationY < 0)
		{
			m_flRotationY += 360;
		}

		SendMoveEvent(m_vtLastSendPos);
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	void MyHeroMoveEnd()
	{
		if (!m_bMoving) return;
		MyHeroMove();
		SendMoveEndEvent();
		m_bMoving = false;
	}

	//---------------------------------------------------------------------------------------------------
	bool UpdateMove(Vector3 vtPos, float flStopDistance)
	{
		if (!IsMovable()) return false;
		if (m_csSkillStatus.IsStatusPlayAnim()) return false;

		if (m_vtPrevPos == transform.position)
		{
			m_nSamePosMoveCount++;
			if (m_nSamePosMoveCount > 30)
			{
				m_nSamePosMoveCount = 0;

				ResetPathOfNavMeshAgent(); // 네비 범위 밖의 Path 갖고 있으면 초기화가 안되서 강제 초기화.
				ChangeState(EnState.Idle);

				//Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
				dd.d(vtPos, transform.position);
				Debug.Log(GetDistanceFormTarget(vtPos));
				Debug.Log(flStopDistance);
				return false;
			}
		}
		else
		{
			m_nSamePosMoveCount = 0;
		}

		if (IsTargetInDistance(vtPos, flStopDistance))
		{
			if (m_enState != EnState.MoveForSkill || NavMeshDirect(vtPos))
			{
				return true;
			}
		}
		return false;
	}

	bool m_bMoving;
	//---------------------------------------------------------------------------------------------------
	protected IEnumerator SendMoveState() 	// 클라이언트 이벤트 - Move // 플레이어 이동.
	{
		while (true)
		{
			bool bFirstOk = false;
			while (m_bMoving)
			{
				if (bFirstOk)
				{
					yield return new WaitForSeconds(0.125f);
					MyHeroMove();
				}
				else
				{
					yield return new WaitForSeconds(0.08f);
					bFirstOk = MyHeroMove();
				}
			}
			
			yield return null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void SetDestination(Vector3 vtPos, float flStopRange)
	{
		base.SetDestination(vtPos, flStopRange);
		MyHeroMoveStart();
	}

	//---------------------------------------------------------------------------------------------------
	void ResetPathOfNavMeshAgent()
	{
		if (m_navMeshAgent.isActiveAndEnabled)
		{
			m_navMeshAgent.ResetPath();
		}
		MyHeroMoveEnd();
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveToPos(Vector3 vtMovePos, float flStopRange)
	{
		Move(EnState.MoveToPos, vtMovePos, flStopRange);
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveForSkill(CsJobSkill csJobSkill, EnSkillStarter enSkillStarter)
	{
		m_csSkillStatus.Init(csJobSkill, false, enSkillStarter);
		Move(EnState.MoveForSkill, m_trTargetEnemy.position, GetAttackCastRange(m_trTargetEnemy, m_csSkillStatus.JobSkill.CastRange), true);
	}

	//---------------------------------------------------------------------------------------------------
	bool CheckHasPath(EnState enState)
	{
		if (m_bMoveHasPath == false)
		{
			if (m_navMeshAgent.hasPath)
			{
				m_bMoveHasPath = true;
				ChangeState(enState);
				return true;
			}
			else
			{
				return false;
			}
		}
		return true;
	}

	#endregion 05 Move

	#region 06 Anim

	//---------------------------------------------------------------------------------------------------
	void OnAnimAttackTargetCheck()
	{
		if (m_csSkillStatus.Chained)
		{
			if (m_trTargetEnemy == null || !IsTargetInDistance(m_trTargetEnemy.position, GetAttackCastRange(m_trTargetEnemy, m_csSkillStatus.JobSkill.CastRange)))
			{
				SetAnimStatus(EnAnimStatus.Idle);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAttackEnd()
	{
		//dd.d("OnAttckEnd()");
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
		m_navMeshAgent.updateRotation = true;
		ResetPathOfNavMeshAgent();
		m_csSkillStatus.Reset();

		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnAnimSkillMove(int nMoveCount) // 스킬 동작중 이동.
	{
		if (nMoveCount == 1) // Asura skill 5번스킬 이중 이동시 사용.
		{
			SetSkillCastingFixedMoveTypeData(m_trTargetEnemy, true);
		}

		transform.LookAt(m_vtTargetPos);
		m_navMeshAgent.speed = m_flSkillMoveSpeed;
		m_navMeshAgent.updateRotation = false;
		m_navMeshAgent.SetDestination(m_vtSkillMovePos);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimTargetSelect(int nHitId = 1)
	{
		if (m_csSkillStatus.IsStatusAnim())
		{
			FindAttackTarget(nHitId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId)
	{
		if (m_csSkillStatus.JobSkill == null) return;
		DamgaeSound(m_csSkillStatus.JobSkill.SkillId);

		if (m_csHitSkillCast == null) return;

		for (int i = 0; i < m_csHitSkillCast.HitInfoNodeList.Count; i++)
		{
			CsHitInfoNode csHitInfoNode = m_csHitSkillCast.HitInfoNodeList[i];
			if (csHitInfoNode.HitResult != null && csHitInfoNode.Target != null && csHitInfoNode.HitResult.hitId == nHitId)
			{
				if (csHitInfoNode.HitResult.hp == 0)
				{
					Debug.Log("OnAnimApplyDamage : HitResult.hp : " + csHitInfoNode.HitResult.hp + " // Hero Dead Hit >>>>>>>   ");
				}
				CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, false);
				AttackHitEffect(csHitInfoNode.Target);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRun()
	{
		m_navMeshAgent.updateRotation = true;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadStart()
	{
		m_csHitSkillCast = null;
		m_csSkillStatus.Reset();
		Invoke(DeadEnd, 1f);
	}

	//---------------------------------------------------------------------------------------------------
	void DeadEnd()
	{
		CsDungeonManager.Instance.DungeonClear();
	}

	protected virtual void DamgaeSound(int nSkill) { }

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		//dd.d("OnEventAnimStartIdle()");
		m_csSkillStatus.Display();
		if (m_csSkillStatus.IsStatusAnim())
		{
			OnAttackEnd();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		//dd.d("OnEventAnimStartAttack");
		if (Dead || m_csSkillStatus.JobSkill == null) return;

		ResetPathOfNavMeshAgent();

		m_vtTargetPos = CsGameData.Instance.MyHeroTransform.position;
		transform.LookAt(CsGameData.Instance.MyHeroTransform);

		m_vtSkillMovePos = transform.position;
		m_csSkillStatus.OnSkillAnimStarted();
		SetSkillCastingFixedMoveTypeData(m_trTargetEnemy);
        SendSkillCastEvent(m_vtSkillMovePos);

		if (m_csSkillStatus.JobSkill.FormTypeEnum == EnFormType.Buff) return; // 버프스킬.
		if (m_csSkillStatus.JobSkill.TypeEnum == EnJobSkillType.AreaOfEffect) return; // 장판스킬.

		m_csHitSkillCast = null;
		m_csHitSkillCast = new CsHitSkillCast(m_csSkillStatus.JobSkill.SkillId, m_csSkillStatus.GetChainSkillId());
		//Debug.Log("1. OnEventAnimStartAttack	SkillId : " + m_csSkillStatus.JobSkill.SkillId + " // ChainSkillId : " + m_csSkillStatus.GetChainSkillId());
	}

	#endregion 06 Anim

	#region 09 Skill

	//---------------------------------------------------------------------------------------------------
	bool PlayNextSkill()
	{
		if (!IsAttackable()) return false; // 사망 or 스턴 리턴.

		if (m_csSkillStatus.IsNextSkill)
		{
			PlaySkill(m_csSkillStatus.NextJobSkill, EnSkillStarter.Clone);
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void ChainSkillAbility(CsJobSkill csJobSkill)
	{
		if (!IsAttackable() || m_trTargetEnemy == null || csJobSkill == null) return;

		if (m_csSkillStatus.IsChainable(csJobSkill.SkillId))
		{
			if (IsTargetInDistance(m_trTargetEnemy.position, GetAttackCastRange(m_trTargetEnemy, m_csSkillStatus.JobSkill.CastRange)))
			{
				m_csSkillStatus.Chained = true;
				//CsGameEventToUI.Instance.OnEventUseAutoSkill(m_csSkillStatus.JobSkill.SkillId);  // 자동스킬 사용 UI에 전달.
				Attack();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayBattle()
	{
		if (m_csSkillStatus.IsChainSkill())  // 연계스킬.
		{
			if (m_csSkillStatus.IsStatusAnim() && !m_csSkillStatus.Chained)
			{
				ChainSkillAbility(m_csSkills.GetUsableSkill());
			}
		}
		else // 일반스킬.
		{
			if (m_enState == EnState.Idle && !m_csSkillStatus.IsStatusPlayAnim())
			{
//				m_csSkillStatus.SetNextSkill(null);
				PlaySkill(m_csSkills.GetUsableSkill(), EnSkillStarter.Net);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsSkillUsable(CsHeroSkill csHeroSkill)
	{
		if (csHeroSkill.RemainCoolTime == 0)
		{
			if (CsMainQuestManager.Instance.MainQuest == null)
			{
				return true;
			}

			return csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void PlaySkill(CsJobSkill csJobSkill, EnSkillStarter enSkillStarter)
	{
		if (!IsAttackable() || m_trTargetEnemy == null || csJobSkill == null)
		{
//			m_csSkillStatus.SetNextSkill(null);
			ChangeState(EnState.Idle);
			dd.d("PlaySkill 1", IsAttackable(), m_trTargetEnemy != null, csJobSkill != null);
			return;
		}

		if (csJobSkill.FormType == 3 || (IsTargetInDistance(m_trTargetEnemy.position, GetAttackCastRange(m_trTargetEnemy, csJobSkill.CastRange)) && NavMeshDirect(m_trTargetEnemy.position)))
		{
			m_csSkillStatus.Init(csJobSkill, true, enSkillStarter);
			Attack();
			return;
		}

		MoveForSkill(csJobSkill, enSkillStarter);
	}

	#endregion 09 Skill

	#region 11 Setting

	//---------------------------------------------------------------------------------------------------
	public override void AttackByHit(PDHitResult HitResult, float flHitTime, PDHitResult AddHitResult, bool bMyHero, bool bKnockback)
	{
		SetDamageText(HitResult);
		ChangedAbnormalStateEffectDamageAbsorbShields(HitResult.changedAbnormalStateEffectDamageAbsorbShields);
		if (AddHitResult != null)
		{
			SetDamageText(AddHitResult);
		}

		if (Dead) return;

		if (m_flHpChangeTime <= flHitTime)
		{
			m_flHpChangeTime = flHitTime;
			Hp = HitResult.hp;
			if (AddHitResult != null)
			{
				Hp = HitResult.hp;
			}
		}

		if (Hp == 0)
		{
			Dead = true;
			DelayDead();
			m_capsuleCollider.enabled = false;

			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void DamageByAbnormal(bool bSkillCancel)
	{
		m_vtSkillMovePos = transform.position;
		ChangeState(EnState.Idle);

		if (bSkillCancel)
		{
			OnAttackEnd();
			ChangeState(EnState.Damage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void HitEffect(Transform trAttacker)
	{
		if (CsIngameData.Instance.Effect != 0)
		{
			if (!trAttacker.CompareTag(CsMonster.c_strTag)) return;
			CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, transform.rotation, "Monster_Hit", 0.5f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool NavMeshDirect(Vector3 vtPos)
	{
		return (NavMeshCornerCount(vtPos) == 2 || IsTargetInDistance(vtPos, 1f));
	}

	//---------------------------------------------------------------------------------------------------
	public int NavMeshCornerCount(Vector3 vtPos)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		if (m_navMeshAgent.CalculatePath(vtPos, navMeshPath))
		{
			return navMeshPath.corners.Length;
		}
		return 0;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartBattle()
	{
		transform.tag = "EnemyHero";
		m_bDungeonStart = true;
	}

	#endregion 11 Setting

	//---------------------------------------------------------------------------------------------------
	protected void UpdateAutoPlay()
	{
		if (State == CsHero.EnState.Idle || (State == CsHero.EnState.Attack && SkillStatus.IsCurrentChainable()))
		{
			if (m_bDungeonStart == false) return;
			m_trTargetEnemy = CsGameData.Instance.MyHeroTransform;
			if (m_trTargetEnemy == null) return;

			if (!IsTargetInDistance(m_trTargetEnemy.position, 50))
			{
				return;
			}

			if (CsIngameData.Instance.IngameManagement.GetCsMoveUnit(m_trTargetEnemy).Dead) return;

			PlayBattle();
		}
	}

	//---------------------------------------------------------------------------------------------------
	Guid IHeroObjectInfo.GetHeroId()
	{
		return HeroId;
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroBase IHeroObjectInfo.GetHeroBase()
	{
		return m_csHeroBase;
	}

	//---------------------------------------------------------------------------------------------------
	Transform IHeroObjectInfo.GetTransform()
	{
		return transform;
	}
} 
