using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class CsOtherPlayer : CsHero, IHeroObjectInfo
{
	public class CsNetSkill
	{
		int m_nSkillId;
		int m_nChainSkillId;
		float m_flRotationY;
		Vector3 m_vtHeroPos;
		Vector3 m_vtTargetPos;
		EnSkillType m_enSkillType;

		public int SkillId { get { return m_nSkillId; } }
		public int ChainSkillId { get { return m_nChainSkillId; } }
		public float RotationY { get { return m_flRotationY; } }
		public Vector3 HeroPos { get { return m_vtHeroPos; } }
		public Vector3 TargetPos { get { return m_vtTargetPos; } }
		public EnSkillType SkillType { get { return m_enSkillType; } }

		public CsNetSkill(int nSkillId, int nChainId, Vector3 vtHeroPos, Vector3 vtTargetPos, float flRotationY, EnSkillType enSkillType = EnSkillType.Hero)
		{
			m_nSkillId = nSkillId;
			m_nChainSkillId = nChainId;
			m_flRotationY = flRotationY;
			m_vtHeroPos = vtHeroPos;
			m_vtTargetPos = vtTargetPos;
			m_enSkillType = enSkillType;
		}
	}

	class CsHeroHitSkillCast
	{
		CsNetSkill m_csNetSkill = null;
		List<CsHitInfoNode> m_listcsHitInfoNode = new List<CsHitInfoNode>();

		public CsNetSkill NetSkill { get { return m_csNetSkill; } }
		public List<CsHitInfoNode> HitInfoNode { get { return m_listcsHitInfoNode; } }

		public CsHeroHitSkillCast(CsNetSkill csNetSkill = null)
		{
			m_csNetSkill = csNetSkill;
		}
	}

	const int MAX_BEGINATTACKCOUNT = 3;

	PDHero m_pDHero;

	int m_nEquippedArtifactNo;
	int m_nPickedSecretLetterGrade;
	int m_nPickedMysteryBoxGrade;
	bool m_bFishing = false;

	string m_strGuildName;
	int m_nGuildMemberGrade;
	int m_nNoblesseId = 0;
	int m_nDisplayTitleId;

	Vector3 m_vtInteractionPos = Vector3.zero;
	bool m_bAfterInteraction = false;

	EnNationWarPlayerState m_enNationWarPlayerState = EnNationWarPlayerState.None;

	bool m_bOverAttackSkillCast = false;
	List<CsHeroHitSkillCast> m_listHeroAttackInfo = new List<CsHeroHitSkillCast>();

	public override Guid HeroId { get { return m_csHeroBase == null ? Guid.Empty : m_csHeroBase.HeroId; } }
	public override CsJob Job { get { return m_csHeroBase == null ? null : m_csHeroBase.Job; } }
	public override int NationId { get { return m_csHeroBase == null ? 0 : m_csHeroBase.Nation.NationId; } }
	public override int Rank { get { return m_csHeroBase == null ? 0 : m_csHeroBase.RankNo; } }
	public override string Name { get { return m_csHeroBase == null ? null : m_csHeroBase.Name; } }

	public override int Level { get { return m_csHeroBase == null ? 0 : m_csHeroBase.Level; } set { m_csHeroBase.Level = value; OnStateChanged(); } }
	public override int MaxHp { get { return m_csHeroBase == null ? 0 : m_csHeroBase.MaxHp; } set { m_csHeroBase.MaxHp = value; OnStateChanged(); } }
	public override int Hp { get { return m_csHeroBase == null ? 0 : m_csHeroBase.Hp; } set { m_csHeroBase.Hp = value; OnStateChanged(); } }

	public EnNationWarPlayerState NationWarPlayerState { get { return m_enNationWarPlayerState; } set { m_enNationWarPlayerState = value; } }
	//---------------------------------------------------------------------------------------------------
	public void Init(PDHero pDHero, bool bFirst, bool bRevivalEnter, float flRevivalInvincibilityRemainingTime)
	{
		m_pDHero = pDHero;
		m_csHeroBase = new CsHeroBase(pDHero);

		CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationInstance(pDHero.nationId).GetNationNoblesseInstanceByHeroId(pDHero.id);
		if (csNationNoblesseInstance != null)
		{
			m_nNoblesseId = csNationNoblesseInstance.NoblesseId;
		}
		else
		{
			m_nNoblesseId = 0;
		}

		VipLevel = pDHero.vipLevel;
		m_nDisplayTitleId = pDHero.displayTitleId;

		m_nGuildMemberGrade = pDHero.guildMemberGrade;
		m_strGuildName = pDHero.guildName;

		m_nCreatureId = pDHero.participatedCreatureId;
		m_bRidingMount = pDHero.isRiding;
		m_nMountId = pDHero.mountId;
		m_nMountLevel = pDHero.mountLevel;

		m_flRotationY = pDHero.rotationY;
		transform.name = Name;
		transform.position = CsRplzSession.Translate(pDHero.position);
		ChangeEulerAngles(m_flRotationY);

		m_bSafeMode = pDHero.isSafeMode;
		m_bDistortion = pDHero.isDistorting;

		m_bWalk = pDHero.isWalking;
		m_bFishing = pDHero.isFishing;
		m_bAcceleration = pDHero.accelerationMode;

		m_nEquippedArtifactNo = pDHero.equippedArtifactNo;
		m_nPickedSecretLetterGrade = pDHero.pickedSecretLetterGrade;
		m_nPickedMysteryBoxGrade = pDHero.pickedMysteryBoxGrade;

		m_csHeroCustomData = new CsHeroCustomData(pDHero);
		m_bView = false;

		SetHeroTagSetting();
		ViewHero(true);	

		SetHeroViewSetting(false);
		ChangeBattleMode(pDHero.isBattleMode);
		NavMeshSetting();

		if (Hp == 0) // OterHero 죽어있는 상태.
		{
			Debug.Log(" 플레이어 이미 죽어있는 상태 처리. ");
			Dead = true;
			ChangeState(EnState.Dead);
			m_capsuleCollider.enabled = false;
		}
		else
		{
			if (bRevivalEnter) // 부활 입장.
			{
				HeroReviveEffect();
				StartCoroutine(RevivalInvincibility(flRevivalInvincibilityRemainingTime));
			}
			else
			{
				if (bFirst) // 최초 입장.
				{
					EnterEffectStart();
				}
			}

			if (m_bRidingMount)
			{
				ChangeTransformationState(EnTransformationState.Mount);
			}
			else if (m_bFishing)
			{
				ChangeState(EnState.Fishing);
			}
			else if (pDHero.transformationMonsterId != 0)	// 몬스터 변신 여부.
			{
				CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(pDHero.transformationMonsterId);
				ChangeTransformationState(CsHero.EnTransformationState.Monster, false, csMonsterInfo);
			}
		}

		if (pDHero.abnormalStateEffects != null) // 상태이상 관련 처리.
		{
			for (int i = 0; i < pDHero.abnormalStateEffects.Length; i++)  // 타 유저 상태이상 관련.
			{
				NetEventHeroAbnormalStateEffectStart(pDHero.abnormalStateEffects[i].instanceId,
													 pDHero.abnormalStateEffects[i].abnormalStateId,
													 pDHero.abnormalStateEffects[i].sourceJobId,
													 pDHero.abnormalStateEffects[i].level,
													 pDHero.abnormalStateEffects[i].damageAbsorbShieldRemainingAbsorbAmount,
													 pDHero.abnormalStateEffects[i].remainingTime,
													 pDHero.abnormalStateEffects[i].damageAbsorbShieldRemainingAbsorbAmount,
													 null);
			}
		}

		CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetNationWarDeclaration(m_csHeroBase.Nation.NationId);
		if (csNationWarDeclaration != null)
		{
			if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
			{
				if (csNationWarDeclaration.NationId == m_csHeroBase.Nation.NationId)
				{
					m_enNationWarPlayerState = EnNationWarPlayerState.Offense;
				}
				else if (csNationWarDeclaration.TargetNationId == m_csHeroBase.Nation.NationId)
				{
					m_enNationWarPlayerState = EnNationWarPlayerState.Defense;
				}
			}
		}

		CreateHeroHUD();

		CsCreature csCreature = CsGameData.Instance.GetCreature(pDHero.participatedCreatureId);
		if (csCreature != null)
		{
			StartCoroutine(CreateCreature(csCreature));
		}

		m_animator.SetFloat(s_nAnimatorHash_move, 1);
		m_animator.SetInteger(s_nAnimatorHash_job, 0); // 0 기본, 1 전직1, 2 전직2
		CsGameData.Instance.ListHeroObjectInfo.Add(this);

		m_flMoveStopRange = 1f;
		m_vtMovePos = transform.position;

		if (Dead == false)
		{
			ChangeState(EnState.Idle);
		}

		Debug.Log("#####     InitOtherHero     #####     strName = " + Name + " // Id = " + HeroId);
	}

	#region 01 MonoBehavier

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_audioSource.volume = 0.2f;
		m_audioSourceParent.volume = 0.2f;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		Debug.Log("CsOtherPlayer.OnDestroy()   m_rtfHUD= " + m_rtfHUD);
		if (CsIngameData.Instance.TargetTransform == transform)
		{
			CsGameEventToUI.Instance.OnEventSelectHeroInfoStop();
		}

		base.OnDestroy();

		m_listHeroAttackInfo.Clear();
		m_listHeroAttackInfo = null;

		m_pDHero = null;
		m_csHeroBase = null;
		CsGameData.Instance.ListHeroObjectInfo.Remove(this);
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (CsGameData.Instance.MyHeroTransform == null) return;

		if (Job == null) return;

		if (m_enState == EnState.Dead) return;

		if (m_enState == EnState.Idle)
		{
			IdleState();
		}
		else if (m_enState == EnState.MoveToPos)
		{
			MoveToPosState();
		}
		else
		{
			if (m_enState != EnState.Interaction && m_enState != EnState.ReturnScroll && m_enState != EnState.Fishing && m_enState != EnState.ItemSteal)
			{
				if (m_csSkillStatus.IsStatusPlayAnim() == false)
				{
					if (m_enState != EnState.Idle)
					{
						ChangeState(EnState.Idle);
					}
				}
			}
		}

		m_vtPrevPos = transform.position;
	}


	//---------------------------------------------------------------------------------------------------
	public override bool ChangeState(EnState enNewState)
	{
		if (m_enState != enNewState)
		{
			switch (m_enState)
			{
				case EnState.Fishing:
					m_csEquipment.RemoveFishStaff();
					break;
			}

			switch (enNewState)
			{
				case EnState.Idle:

					if (m_bFishing)
					{
						ChangeState(EnState.Fishing);
					}
					break;
			}
		}

		return base.ChangeState(enNewState);
	}

	//---------------------------------------------------------------------------------------------------
	void IdleState()
	{
		if (IsTargetInDistance(m_vtMovePos, m_flMoveStopRange) == false)
		{
			m_nMoveStopWaitCount++;
			if (m_nMoveStopWaitCount > 3)
			{
				m_nMoveStopWaitCount = 0;

				if (m_navMeshAgent.hasPath == false)
				{
					MoveToPos(m_vtMovePos, c_flStopRange);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void MoveToPosState()
	{
		if (IsTargetInDistance(m_vtMovePos, m_flMoveStopRange))
		{
			if (FindSkillCast().NetSkill != null)
			{
				PlayNextSkill();
			}
			else
			{
				m_nMoveStopWaitCount++;
				if (m_nMoveStopWaitCount > 5)
				{
					ChangeState(EnState.Idle);
					m_nMoveStopWaitCount = 0;
				}
			}
		}
		else
		{
			if (m_vtPrevPos == transform.position) // 추가 입력 없이 제자리에 있는경우.
			{
				m_nMoveStopWaitCount++;
				if (m_nMoveStopWaitCount > 30)
				{
					ChangeState(EnState.Idle);
					m_navMeshAgent.ResetPath();
					m_nMoveStopWaitCount = 0;
				}
			}
			else
			{
				ChangeHeroMoveSpeed(GetHeroMoveSpeed());
			}
		}
	}

	#endregion 01 MonoBehavier

	//---------------------------------------------------------------------------------------------------
	public override void OnStateChanged()
	{
		if (CsIngameData.Instance.TargetTransform == transform) // 선택된 플레이어 정보 전달.
		{
			CsGameEventToUI.Instance.OnEventSelectInfoUpdate();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void DelayDead()
	{
		Debug.Log("DelayDead    >>>>>    Dead = " + Dead);
		if (Dead == false)
		{
			m_flHpChangeTime = Time.time;
			Hp = 0;
			Dead = true;
			m_capsuleCollider.enabled = false;

			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}

			if (m_listHeroAttackInfo.Count != 0)
			{
				Debug.Log("DelayDead    >>>>>    Dead           //      m_listAttackInfo  = " + m_listHeroAttackInfo.Count);

				for (int i = 0; i < m_listHeroAttackInfo.Count; i++)
				{
					CsHeroHitSkillCast FindSkillCast = m_listHeroAttackInfo[i];
					for (int j = 0; j < FindSkillCast.HitInfoNode.Count; j++)
					{
						CsHitInfoNode csHitInfoNode = FindSkillCast.HitInfoNode[j];
						if (csHitInfoNode.HitResult != null && csHitInfoNode.Target != null)
						{
							CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, false);
						}
					}
				}
			}
			base.DelayDead();
		}
	}

	#region 02 NetEvent .

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMove(Vector3 vtPos, float flRotationY) // OrherHero  이동 수신.
	{
		m_nMoveStopWaitCount = 0;
		m_flRotationY = transform.eulerAngles.y;
		m_vtMovePos = vtPos;

		if (m_csSkillStatus.IsStatusPlayAnimWithNoMove()) return; // 이동 불가능한 스킬사용 중.
		if (m_enState == EnState.Idle && IsTargetInDistance(vtPos, 0.4f)) return; // 첫이동 이동 값이 작은 경우 지연처리(일반 0.66 정도 거리 발생).

		if (transform.position == vtPos && m_flRotationY != flRotationY)
		{
			m_flRotationY = flRotationY;
			ChangeEulerAngles(flRotationY);
			return;
		}

		MoveToPos(m_vtMovePos, c_flStopRange);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartMove(Vector3 vtPos, float flRotationY)
	{
		if (m_csCartObject == null)
		{
			Debug.Log("NetEventCartMove          m_csCartObject == null         문제 있음 확인 필요.");
			return;
		}

		m_nMoveStopWaitCount = 0;
		m_flRotationY = flRotationY;
		m_vtMovePos = vtPos;

		if (m_enState == EnState.Idle && IsTargetInDistance(vtPos, 0.4f)) return; // 첫이동 시작시 이동 값이 작은 경우 지연처리(일반 0.66 정도 거리 발생).
		if (m_listHeroAttackInfo.Count > 0) return;

		MoveToPos(m_vtMovePos, c_flStopRange);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroSkillCast(int nSkillId, int nChainSkillId, Vector3 vtHeroPos, Vector3 vtTargetPos)  // 스킬 케스트 수신.
	{
		CsNetSkill csNetSkill = new CsNetSkill(nSkillId, nChainSkillId, vtHeroPos, vtTargetPos, m_flRotationY, EnSkillType.Hero);
		AddHeroHitSkillCast(csNetSkill); // 공격 캐스트 추가
		PlayNextSkill();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroJobCommonSkillCast(int nSkillId, Vector3 vtTargetPos)
	{
		Debug.Log("NetEventHeroJobCommonSkillCast");
		LookAtPosition(vtTargetPos);
		ChangeState(EnState.Idle);

		CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.GetJobCommonSkill(nSkillId);
		if (csJobCommonSkill.SkillId == 2) // 모션있는 공격 스킬.
		{
			CsNetSkill csNetSkill = new CsNetSkill(nSkillId, 0, transform.position, vtTargetPos, m_flRotationY, EnSkillType.Common);
			AddHeroHitSkillCast(csNetSkill); // 공격 캐스트 추가
			PlayNextSkill();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroRankActiveSkillCast(int nSkillId, Vector3 vtTargetPos)
	{
		Debug.Log("NetEventHeroRankActiveSkillCast");
		LookAtPosition(vtTargetPos);
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTransformationMonsterSkillCast(int nSkillId, Vector3 vtTargetPos)
	{
		Debug.Log("NetEventTransformationMonsterSkillCast nSkillId = "+ nSkillId);
		if (m_csTransformationMonster != null)
		{
			m_nTransformationMonsterSkillId = nSkillId;
			LookAtPosition(vtTargetPos);
			CsMonsterSkill csMonsterSkill = CsGameData.Instance.MonsterSkillList.Find(a => a.SkillId == nSkillId);
			m_csTransformationMonster.TransformationMonsterSkillCast(csMonsterSkill);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTargetHeroDead(Transform trTarget)
	{
		for (int i = 0; i < m_listHeroAttackInfo.Count; i++) // 기존 저장중인 대상 관련 정보 삭제.
		{
			CsHeroHitSkillCast FindSkillCast = m_listHeroAttackInfo[i];
			
			for (int j=0; j<FindSkillCast.HitInfoNode.Count; j++)
			{
				if (FindSkillCast.HitInfoNode[j].Target == trTarget)
				{
					CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, FindSkillCast.HitInfoNode[j].HitResult, Time.time, FindSkillCast.HitInfoNode[j].AddHitResult, false);
				}
			}
			FindSkillCast.HitInfoNode.RemoveAll(a => a.Target == trTarget);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHitApprove(PDHitResult HitResult, Transform trTarget) // 피격된 몬스터 정보 저장용 >> 실제 피격 타이밍에 전달.
	{
		CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(Job.JobId, HitResult.skillId);

		if (csJobSkill == null) return;

		if (m_listHeroAttackInfo.Count == 0)
		{
			if (csJobSkill.TypeEnum == EnJobSkillType.AreaOfEffect) // 장판스킬.
			{				
				CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult,Time.time, null, false);
			}
			return;
		}

		if (m_bOverAttackSkillCast) // 누적 케스트 정보 3개 이상시.
		{
			List<CsHitInfoNode> listCsHitInfoNode = m_listHeroAttackInfo[m_listHeroAttackInfo.Count - 1].HitInfoNode;
			int nCount = listCsHitInfoNode.Count; // 마지막 공격에 공격 결과 저장.
			if (nCount > 0)
			{
				listCsHitInfoNode[nCount - 1].AddHitResult = HitResult;
			}
			else
			{
				Debug.Log("CsOtherPlayer.NetEventHitApprove()          HitInfoNode.Count == 0    문제있음.");
			}
			return;
		}
		else
		{
			CsHeroHitSkillCast csHeroHitSkillCast = m_listHeroAttackInfo.Find(a => (a.NetSkill.SkillId == HitResult.skillId) && (a.NetSkill.ChainSkillId == HitResult.chainSkillId));

			if (csJobSkill.TypeEnum == EnJobSkillType.AreaOfEffect) // 장판 첫타는 Hit 있음.
			{
				CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult, Time.time, null, false);
				if (csHeroHitSkillCast != null)
				{
					m_listHeroAttackInfo.Remove(csHeroHitSkillCast);
				}
			}
			else
			{
				if (csHeroHitSkillCast != null)
				{
					csHeroHitSkillCast.HitInfoNode.Add(new CsHitInfoNode(trTarget, HitResult, Time.time));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventPortalEnter(Vector3 vtPos, float flRotationY)
	{
		Debug.Log("NetEventRuinsReclaimPortalEnter() vtPos = " + vtPos);
		StartCoroutine(HeroMoveDirecting(vtPos, flRotationY));
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroRevived(int nHp, Vector3 vtPos, float flRotationY, float flRevivalInvincibilityRemainingTime)
	{
		Debug.Log("NetHeroRevived    부활!!!");
		m_flHpChangeTime = Time.time;
		Hp = nHp;
		Dead = false;
		m_bInvincibility = true;
		m_capsuleCollider.enabled = true;

		if (CsIngameData.Instance.EffectEnum == EnOptionEffect.All && flRevivalInvincibilityRemainingTime != 0)
		{
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.User, transform, transform.position, "Abnormal_9", flRevivalInvincibilityRemainingTime);
		}

		StartCoroutine(HeroMoveDirecting(vtPos, flRotationY));
		HeroReviveEffect();
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator HeroMoveDirecting(Vector3 vtPos, float flRotationY)
	{
		m_navMeshAgent.enabled = false;
		transform.position = vtPos;
		ChangeEulerAngles(flRotationY);
		yield return new WaitForSeconds(0.2f);
		m_navMeshAgent.enabled = true;
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventInteractionStart(Vector3 vtTargetPos, float flInteractionMaxRange) // 상호작용 모션 시작
	{
		if (IsTargetInDistance(vtTargetPos, flInteractionMaxRange)) // 정비 범위 내일때는 스킬 발동.
		{
			LookAtPosition(vtTargetPos);
			ChangeState(EnState.Interaction);
		}
		else
		{
			m_vtMovePos = m_vtInteractionPos = vtTargetPos;
			m_bAfterInteraction = true;
			m_nMoveStopWaitCount = 0;
			MoveToPos(m_vtMovePos, flInteractionMaxRange);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventInteractionStart() // 상호작용 모션 시작
	{
		Debug.Log("NetEventHeroContinentObjectInteractionCancel()      서버로부터 Other 상호작용 취소 이벤트 전달.");
		ChangeState(EnState.Interaction);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventInteractionCancel() // 상호작용 모션 취소
	{
		Debug.Log("NetEventHeroContinentObjectInteractionCancel()");
		ChangeState(EnState.Idle);
		m_bAfterInteraction = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventInteractionFinished() // 상호작용 모션 종료
	{
		Debug.Log("NetEventHeroContinentObjectInteractionFinished()");
		ChangeState(EnState.Idle);
		m_bAfterInteraction = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventRuinsReclaimInteractionFinished() // 유적탈환 변신취소 상호작용 모션 종료
	{
		Debug.Log("NetEventRuinsReclaimInteractionFinished()");
		ChangeTransformationState(CsHero.EnTransformationState.None);
		m_bAfterInteraction = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetWarMemoryTransformationObjectInteractionFinished(CsMonsterInfo csMonsterInfo) // 전쟁의기억 변식오브젝트 상호작용 종료 변신 시작.
	{
		Debug.Log("NetWarMemoryTransformationObjectInteractionFinished()");
		ChangeState(EnState.Idle);
		m_bAfterInteraction = false;
		ChangeTransformationState(EnTransformationState.Monster, false, csMonsterInfo);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventWarMemoryMonsterTransformationFinished(int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("NetEventWarMemoryMonsterTransformationFinished()");
		ChangeTransformationState(CsHero.EnTransformationState.None);

		m_flHpChangeTime = Time.time;
		m_nMaxHp = nMaxHp;
		m_nHp = nHp;
		RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAbnormalStateEffectHit(int nHp, long[] alRemovedAbnormalStateEffects, long lAbnormalStateEffectInstanceId, int nDamage, int nHpDamage, Transform trAttacker)
	{
		// lAbnormalStateEffectInstanceId 에 대한 추가적인 처리 위해 nAbnormalStateId에 대한 정보도 함께 수신 필요.
		if (!HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects))
		{
			Invoke(DelayDead, 0.2f);
			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMainGearEquip(Guid guidGearId, int nGearId, int nEnchantLevel) // 장비장착 ,교체, 해제.
	{
		CsMainGear csMainGear = CsGameData.Instance.GetMainGear(nGearId);

		if (csMainGear == null) return;

		if (csMainGear.MainGearType.MainGearCategory.EnMainGearCategory == EnMainGearCategory.Weapon)
		{
			m_csHeroCustomData.SetWeapon(guidGearId, nGearId, nEnchantLevel);
		}
		else if (csMainGear.MainGearType.MainGearCategory.EnMainGearCategory == EnMainGearCategory.Armor)
		{
			m_csHeroCustomData.SetArmor(guidGearId, nGearId, nEnchantLevel);
		}

		SetHeroViewSetting(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMainGearUnequip(Guid guidGearId) // 장비 해제.
	{
		if (m_csHeroCustomData.Weapon == guidGearId)
		{
			m_csHeroCustomData.SetWeapon(Guid.Empty, 0, 0);
		}
		else if (m_csHeroCustomData.Armor == guidGearId)
		{
			m_csHeroCustomData.SetArmor(Guid.Empty, 0, 0);
		}
		SetHeroViewSetting(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCostumeEquip(int nCostumeId, int nCostumeEffectId)
	{
		m_csHeroCustomData.SetCostum(nCostumeId, nCostumeEffectId);
		SetHeroViewSetting(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCostumeEffectApply(int nCostumeEffectId)
	{
		m_csHeroCustomData.SetCostum(m_csHeroCustomData.CostumeId, nCostumeEffectId);
		SetHeroViewSetting(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCostumeUnequip()
	{
		m_csHeroCustomData.SetCostum(0, 0);
		SetHeroViewSetting(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroEquippedArtifactChanged(int nEquippedArtifactNo)
	{
		m_pDHero.equippedArtifactNo = nEquippedArtifactNo;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroReturnScrollCastStart() // 귀환주문서 사용모션 시작.
	{
		ChangeState(EnState.ReturnScroll);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroReturnScrollCastEnd() // 귀환주문서 사용모션 종료.
	{
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void NetEventHeroLevelUp(int nLevel, int nMaxHp, int nHp) // 레벨업.
	{
		m_flHpChangeTime = Time.time;
		Level = nLevel;
		MaxHp = nMaxHp;
		Hp = nHp;

		if (CsIngameData.Instance.EffectEnum == EnOptionEffect.All)
		{
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "LevelUP", 3f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMountGetOn(int nMountId, int nMountLevel)
	{
		Debug.Log("OtherHero.NetEventHeroMountGetOn()   nMountId = " + nMountId + " // nMountLevel = " + nMountLevel);
		m_nMountId = nMountId;
		m_nMountLevel = nMountLevel;
		ChangeTransformationState(EnTransformationState.Mount);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMountGetOff()
	{
		Debug.Log("OtherHero.NetEventHeroMountGetOff()");
		IsRidingMount = false;
		ChangeTransformationState(EnTransformationState.None);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMountLevelUp(int nMountId, int nMountLevel)
	{
		Debug.Log("OtherHero.NetEventHeroMountLevelUp()");
		m_nMountId = nMountId;
		m_nMountLevel = nMountLevel;
		if (IsRidingMount)
		{
			ChangeTransformationState(EnTransformationState.Mount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMysteryBoxPickStart()
	{
		Debug.Log("NetEventMysteryBoxPickStart");
		ChangeState(EnState.Interaction);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMysteryBoxPickCancel()
	{
		Debug.Log("NetEventMysteryBoxPickCancel");
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMysteryBoxPick(int nGrade)
	{
		Debug.Log("NetEventMysteryBoxPick     nGrade = " + nGrade);		
		m_nPickedMysteryBoxGrade = nGrade;
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSecretLetterPickStart()
	{
		Debug.Log("NetEventSecretLetterPickStart");
		ChangeState(EnState.Interaction);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSecretLetterPickCancel()
	{
		Debug.Log("NetEventSecretLetterPickCancel");		
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSecretLetterPick(int nGrade)
	{
		Debug.Log("NetEventSecretLetterPick     nGrade = "+ nGrade);
		m_nPickedSecretLetterGrade = nGrade;
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDimensionRaidInteractionStart()
	{
		Debug.Log("NetEventDimensionRaidInteractionStart");
		ChangeState(EnState.Interaction);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDimensionRaidInteractionCancel()
	{
		Debug.Log("NetEventDimensionRaidInteractionCancel");
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventFishingStart(Transform trTarget = null)
	{
		Debug.Log("NetEventFishingStart()  trTarget = " + trTarget);
		if (trTarget != null)
		{
			transform.LookAt(trTarget);
		}

		m_bFishing = true;
		ChangeState(EnState.Fishing);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventFishingStop()
	{
		m_bFishing = false;
		Debug.Log("NetEventFishingStop()");
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventEvtHeroHpRestored(int nHp)
	{
		m_flHpChangeTime = Time.time;
		Hp = nHp;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventEvtHeroBattleMode(bool bBattleMode)
	{
		if (bBattleMode)
		{
			m_animator.SetFloat(s_nAnimatorHash_battle, 1); // 1 전투
		}
		else
		{
			m_animator.SetFloat(s_nAnimatorHash_battle, 0); // 0 비전투
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroDead(PDHitResult pDHitResult)
	{
		Debug.Log("OtherPlayer.NetEventHeroDead()");
		Invoke(DelayDead, 2f);
		if (CsIngameData.Instance.TargetTransform == transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}
	}

    //---------------------------------------------------------------------------------------------------
    public void NetHeroGuildInfoUpdated(Guid guidGuildId, string strGuildName, int nGuildMemberGrade)
    {
		Debug.Log("OtherPlayer.NetHeroGuildInfoUpdated    strGuildName = " + strGuildName + " // nGuildMemberGrade = " + nGuildMemberGrade);
		m_strGuildName = strGuildName;
		m_nGuildMemberGrade = nGuildMemberGrade;
		CreateHeroHUD();
    }

    //---------------------------------------------------------------------------------------------------
    public void NetEventNationWarChangeState()
    {
		Debug.Log("OtherPlayer.NetEventNationWarChangeState");
		CreateHeroHUD();
    }

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroDisplayTitleChange(int nTitleId)
	{
		Debug.Log("OtherPlayer.NetEventHeroDisplayTitleChange     nTitleId = " + nTitleId);
		m_nDisplayTitleId = nTitleId;
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetTrapHit(int nHp, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects, bool bSlowMoveSpeed, int nTrapPenaltyMoveSpeed, PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{
		Debug.Log("##########           OtherPlayer.NetTrapHit            ############");
		ChangedAbnormalStateEffectDamageAbsorbShields(apDAbnormalStateEffectDamageAbsorbShield);

		if (!HeroTrapHit(nHp, nDamage, nHpDamage, alRemovedAbnormalStateEffects, bSlowMoveSpeed, nTrapPenaltyMoveSpeed))
		{
			Invoke(DelayDead, 0.2f);
			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetStartTrap()
	{
		m_flTrapMoveSpeed = (float)CsDungeonManager.Instance.AncientRelic.TrapPenaltyMoveSpeed / 10000;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetTrapEffectFinished()
	{
		Debug.Log("##########           OtherPlayer.NetTrapEffectFinished                    m_flOffestMoveSpeed  " + m_flTrapMoveSpeed);
		m_flTrapMoveSpeed = 0f;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAccelerationStarted()
	{
		Debug.Log("OtherPlayer.NetEventHeroAccelerationStarted");
		m_bWalk = false;
		m_bAcceleration = true;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAccelerationEnded()
	{
		Debug.Log("OtherPlayer.NetEventHeroAccelerationEnded");
		m_bAcceleration = false;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	protected override bool HeroAbnormalEffectHit(int nHp, Transform trAttacker, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects)
	{
		if (trAttacker == CsGameData.Instance.MyHeroTransform)
		{
			if (CsIngameData.Instance.InGameCamera.SleepMode == false)
			{
				Vector3 vtPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
				CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nDamage, Camera.main.WorldToScreenPoint(vtPos), false);  // 데미지 Text 처리.
			}
		}

		m_flHpChangeTime = Time.time;
		Hp = nHp;
		return base.HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	protected override bool HeroTrapHit(int nHp, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects, bool bSlowMoveSpeed, int nTrapPenaltyMoveSpeed)
	{
		m_flHpChangeTime = Time.time;
		Hp = nHp;
		return base.HeroTrapHit(nHp, nDamage, nHpDamage, alRemovedAbnormalStateEffects, bSlowMoveSpeed, nTrapPenaltyMoveSpeed);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroDisplayRank(int nRankNo) // 계급 갱신 => Test 필요
	{
		Debug.Log("OtherPlayer.NetEventHeroDisplayRank()        nRankNo = " + nRankNo);
		m_csHeroBase.RankNo = nRankNo;
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetGroggyMonsterItemStealStart()
	{
		Debug.Log("OtherPlayer.NetGroggyMonsterItemStealStart");
		if (m_enState != EnState.ItemSteal)
		{
			ChangeState(EnState.ItemSteal);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetGroggyMonsterItemStealFinished()
	{
		Debug.Log("OtherPlayer.NetGroggyMonsterItemStealFinished");
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDistortionStart()
	{
		Debug.Log("OtherPlayer.NetEventHeroDistortionStart        m_bDistortion = " + m_bDistortion);
		m_bDistortion = true;
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDistortionFinish()
	{
		Debug.Log("OtherPlayer.NetEventDistortionFinish          m_bDistortion  = " + m_bDistortion);
		m_bDistortion = false;
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSafeModeStarted()
	{
		Debug.Log("OtherPlayer.NetEventSafeModeStarted");
		m_bSafeMode = true;
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSafeModeEnded()
	{
		Debug.Log("OtherPlayer.NetEventSafeModeEnded");
		m_bSafeMode = false;
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMoveModeChanged(bool bWalk)
	{
		m_bWalk = bWalk;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventNationAllianceConcluded()
	{
		Debug.Log("OtherPlayer.NetEventNationAllianceConcluded");
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventNationAllianceBroken()
	{
		Debug.Log("OtherPlayer.NetEventNationAllianceBroken");
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	public void NetEventHeroJobChanged(int nJobId)
	{
		Debug.Log("OtherPlayer.NetEventHeroJobChanged : " + nJobId);
		m_csHeroBase.JobId = nJobId;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroTeamBattlefieldSetting()
	{
		Debug.Log("##### NetEventHeroTeamBattlefieldSetting() ######");
		SetHeroTagSetting();
		CreateHeroHUD();
	}

	#endregion 02 NetEvent.

	#region  03.Event.

	//---------------------------------------------------------------------------------------------------
	public void OnEventHideOptionChange()
	{
		SetHeroViewSetting(false);
	}

	//---------------------------------------------------------------------------------------------------
	public void SelectHero(bool bSelect)
	{
		Debug.Log("OtherPlayer.SelectHero   m_bView = " + m_bView + " // bSelect = " + bSelect);
		if (bSelect)
		{
			ViewHero(false);
		}
		else
		{
			SetHeroViewSetting(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeViewHero(bool bView)
	{
		if (m_bView != bView)
		{
			Debug.Log("OtherPlayer.ViewChange    bView = " + bView);
			if (bView)
			{
				SetHeroViewSetting(false);
			}
			else
			{
				HideHero();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetHeroTagSetting()
	{
		Debug.Log("OtherPlayer.SetHeroTagSetting         LocationId = " + m_csMyHeroInfo.LocationId);

		gameObject.layer = LayerMask.NameToLayer("Hero");
		transform.tag = "Hero";                             // 기본 아군 처리.

		if (m_bSafeMode || m_bDistortion) // 안전 모드 , 주문서 상태일경우 무조건 아군처럼 처리.
		{
			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}
		}
		else
		{
			if (CsIngameData.Instance.IngameManagement.IsContinent() || m_csMyHeroInfo.LocationId == CsGameData.Instance.UndergroundMaze.LocationId)    // 대륙 or 지하미로.
			{
				if (CsNationWarManager.Instance.IsEnemyNation(NationId))  // 적국유저(동맹도 아닌경우)
				{
					transform.tag = "EnemyHero";
				}
			}
			else
			{
				if (m_csMyHeroInfo.LocationId == CsGameData.Instance.FieldOfHonor.LocationId || m_csMyHeroInfo.LocationId == CsGameData.Instance.InfiniteWar.Location.LocationId || m_csMyHeroInfo.LocationId == CsGameData.Instance.TeamBattlefield.Location.LocationId)
				{
					transform.tag = "EnemyHero";                // PvP 던전 적국처리.
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetHeroViewSetting(bool bChangEquipment)
	{
		if (CsIngameData.Instance.TargetTransform == transform)  // 선택상태에서는 Hide 안시킴.
		{
			ViewHero(bChangEquipment);
		}
		else
		{
			if (CsIngameData.Instance.IngameManagement.IsContinent() == false)  // 던전일때.
			{
				switch (CsDungeonManager.Instance.DungeonPlay)
				{
				case EnDungeonPlay.FieldOfHonor:
				case EnDungeonPlay.InfiniteWar:
					ViewHero(bChangEquipment);
					break;

				case EnDungeonPlay.FearAltar:
				case EnDungeonPlay.AncientRelic:
				case EnDungeonPlay.SoulCoveter:
				case EnDungeonPlay.RuinsReclaim:
				case EnDungeonPlay.WarMemory:
				case EnDungeonPlay.TradeShip:
				case EnDungeonPlay.AnkouTomb:
				default:
					if (CsIngameData.Instance.UserViewFilter == EnUserViewFilter.Other)
					{
						if (bChangEquipment)
						{
							ViewHero(bChangEquipment);
						}

						HideHero();
					}
					else
					{
						ViewHero(bChangEquipment);
					}
					break;
				}
			}
			else
			{
				if (CsNationWarManager.Instance.IsEnemyNation(NationId))				// 동맹아닌 적국인 경우.
				{
					if (CsIngameData.Instance.UserViewFilter == EnUserViewFilter.Other)	// 아군만 표시 일때는 적군 장비 교체 후 숨김 처리.
					{
						if (bChangEquipment)
						{
							ViewHero(bChangEquipment);
						}

						HideHero();
					}
					else
					{
						ViewHero(bChangEquipment);
					}
				}
				else																	// 아군인 경우.
				{
					if (CsIngameData.Instance.UserViewFilter == EnUserViewFilter.Enemy)	// 적군만 표시 일때는 아군 장비 교체 후 숨김 처리.
					{
						if (bChangEquipment)
						{
							ViewHero(bChangEquipment);
						}

						HideHero();
					}
					else
					{
						ViewHero(bChangEquipment);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ViewHero(bool bChangEquipment)
	{
		Debug.Log("OtherPlayer.OnEventSetOptionHideOtherCharacter     타 플레이어 보기         >>>>      ViewHero()   m_bView = " + m_bView);
		m_bView = true;

		if (bChangEquipment)
		{
			m_csEquipment.CreateArtifact(m_csHeroCustomData, m_nEquippedArtifactNo);
			m_csEquipment.CreateWing(m_csHeroCustomData, m_trPivotHUD);
			m_csEquipment.LowChangEquipments(m_csHeroCustomData);
		}
		else
		{
			m_csEquipment.ViewEquipments();
		}

		if (IsTransformationStateMount())
		{
			MountGetOn();
		}
		else if (IsTransformationStateCart())
		{
			CartGetOn();
		}
		else if (IsTransformationStateMonster() && m_csTransformationMonster != null)
		{
			TransformationMonsterGetOn(m_csTransformationMonster.MonsterInfo);
		}

		CreateShadow();
	}

	//---------------------------------------------------------------------------------------------------
	void HideHero()
	{
		if (CsIngameData.Instance.TargetTransform == transform || m_bView == false) return; // 선택상태에서는 Hide 안시킴.

		Debug.Log("OtherPlayer.OnEventSetOptionHideOtherCharacter     타 플레이어 숨기기    m_bView = " + m_bView);
		m_bView = false;

		if (IsTransformationStateMount() && m_csMountObject != null)
		{
			m_csMountObject.gameObject.SetActive(false);
		}
		else if (IsTransformationStateCart() && m_csCartObject != null)
		{
			m_csCartObject.RidindCart.gameObject.SetActive(false);
		}
		else if (IsTransformationStateMonster() && m_csTransformationMonster != null)
		{
			m_csTransformationMonster.gameObject.SetActive(false);
		}

		m_csEquipment.HideEquipments();
		Destroy(m_goShadow);
		m_goShadow = null;
	}

	#endregion 03. Event.

	#region 05 Move
	//---------------------------------------------------------------------------------------------------
	bool UpdateMove(Vector3 vtPos, float flStopDistance)
	{
		if (m_csSkillStatus.IsStatusPlayAnim())
			return false;

		if (IsTargetInDistance(vtPos, flStopDistance))
		{
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void MoveToPos(Vector3 vtMovePos, float flStopRange)
	{
		m_nMoveStopWaitCount = 0;
		Move(EnState.MoveToPos, vtMovePos, flStopRange, true);
	}

	#endregion 05 Move

	#region 06 Anim

	void OnAnimTargetSelect(int nHitId) { }

	//---------------------------------------------------------------------------------------------------
	void OnAnimAttackTargetCheck()	{	}

	//---------------------------------------------------------------------------------------------------
	void OnAttackEnd()
	{
		if (m_csSkillStatus != null)
		{
			if (m_listHeroAttackInfo.Count > 0)
			{
				if (m_csSkillStatus.JobSkill != null)
				{
					CsHeroHitSkillCast csHitSkillCast = m_listHeroAttackInfo.Find(a => a.NetSkill.SkillId == (m_csSkillStatus.JobSkill.SkillId)); // 이전 스킬 케스트 정보 찾기.

					if (csHitSkillCast != null)
					{
						m_listHeroAttackInfo.Remove(csHitSkillCast);
					}
				}
			}
		}

		m_bOverAttackSkillCast = false;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
		m_navMeshAgent.updateRotation = true;
		m_navMeshAgent.ResetPath();
		m_csSkillStatus.Reset();

		if (!PlayNextSkill())
		{
			if (Hp == 0 || Dead) // 죽음상태여야 하는데 죽지 않아서 죽음 처리.
			{
				DelayDead();
			}
			else
			{
				ChangeState(EnState.Idle);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnAnimSkillMove(int nMoveCount) // 스킬 동작중 이동.
	{
		transform.LookAt(m_vtTargetPos);
		ChangeHeroMoveSpeed(m_flSkillMoveSpeed);
		m_navMeshAgent.updateRotation = false;
		m_navMeshAgent.SetDestination(m_vtSkillMovePos);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId)
	{
		if (m_csSkillStatus.JobSkill == null)
		{
			Debug.Log("OtherPlayer.OnAnimApplyDamage	m_csSkillStatus.JobSkill = null	");
			return;
		} 

		CsHeroHitSkillCast csHeroHitSkillCast = null;

		for (int i = 0; i < m_listHeroAttackInfo.Count; i++)
		{
			if (m_listHeroAttackInfo[i] != null)
			{
				if (m_listHeroAttackInfo[i].NetSkill.SkillId == m_csSkillStatus.JobSkill.SkillId)
				{
					if (m_listHeroAttackInfo[i].NetSkill.ChainSkillId == m_csSkillStatus.GetChainSkillId())
					{
						csHeroHitSkillCast = m_listHeroAttackInfo[i];
						break;
					}
				}
			}
		}

		if (csHeroHitSkillCast == null)
		{
			Debug.Log("OtherPlayer.OnAnimApplyDamage	m_listHeroAttackInfo = null	");
			return;
		}

		for (int i = 0; i < csHeroHitSkillCast.HitInfoNode.Count; i++)
		{
			CsHitInfoNode csHitInfoNode = csHeroHitSkillCast.HitInfoNode[i];

			if (csHitInfoNode != null && csHitInfoNode.HitResult != null && csHitInfoNode.Target != null && csHitInfoNode.HitResult.hitId == nHitId)
			{
				CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, false);
			}
		}
	}  
	
	//---------------------------------------------------------------------------------------------------
	void OnAnimMentalSkill()
	{
		CsHeroHitSkillCast csHeroHitSkillCast = FindSkillCast();

		if (CsIngameData.Instance.Effect != 0 )
		{
			if (csHeroHitSkillCast.NetSkill != null && csHeroHitSkillCast.NetSkill.SkillType == EnSkillType.Common)
			{
				CsEffectPoolManager.Instance.PlayEffect(false, transform, csHeroHitSkillCast.NetSkill.TargetPos, "MetalSkill", 0.8f);
			}
		}
		m_listHeroAttackInfo.Remove(csHeroHitSkillCast);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRun()
	{
		m_navMeshAgent.updateRotation = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimFishingStart()
	{
		if (m_bView)
		{
			int nJobId = Job.ParentJobId == 0 ? Job.JobId : Job.ParentJobId;
			m_csEquipment.CreateFishStaff(nJobId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadStart()
	{
		m_csSkillStatus.Reset();
		m_listHeroAttackInfo.Clear();
	} 

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		if (m_csSkillStatus.IsStatusAnim())
		{
			OnAttackEnd();
		}
		else if (m_bAfterInteraction) // 대기 모션 시작시. 상호작용 대기중이면 상호작용 모션 시작.
		{
			LookAtPosition(m_vtInteractionPos);
			ChangeState(EnState.Interaction);
			m_bAfterInteraction = false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		if (m_csSkillStatus.GetChainSkillId() > 1) // 연계스킬 2 or 3 진행중.
		{
			if (m_listHeroAttackInfo.Count > 0)
			{
				CsHeroHitSkillCast csHitSkillCast = m_listHeroAttackInfo.Find(a => a.NetSkill.ChainSkillId == (m_csSkillStatus.GetChainSkillId() - 1)); // 이전 연계스킬 케스트 정보 찾기.

				if (csHitSkillCast != null)
				{
					m_listHeroAttackInfo.Remove(csHitSkillCast);
				}
			}
			m_bOverAttackSkillCast = false;
		}

		m_navMeshAgent.updateRotation = false;
		m_csSkillStatus.ChangeStatusAnim();
		SetSkillCastingFixedMoveTypeData(null);
	}

	#endregion 06 Anim

	#region 11 Transformation
	//---------------------------------------------------------------------------------------------------
	protected override void MountGetOn()
	{
		base.MountGetOn();
		if (m_bView == false)
		{
			SetHeroViewSetting(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void MountGetOff(bool bSend)
	{
		base.MountGetOff(bSend);
		SetHeroViewSetting(false);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void CartGetOn()
	{
		if (m_rtfHUD != null)
		{
			m_rtfHUD = null;
			CsGameEventToUI.Instance.OnEventDeleteHeroHUD(HeroId);
		}

		base.CartGetOn();

		if (m_bView == false)
		{
			SetHeroViewSetting(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void CartGetOff(bool bSend)
	{
		base.CartGetOff(bSend);
		CreateHeroHUD();
		SetHeroViewSetting(false);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void TransformationMonsterGetOff()
	{
		base.TransformationMonsterGetOff();
		SetHeroViewSetting(false);
	}

	#endregion 11 Transformation

	#region 09 Skill
	//---------------------------------------------------------------------------------------------------
	bool PlayNextSkill()
	{
		CsNetSkill csNetSkill = FindSkillCast().NetSkill;

		if (csNetSkill == null) return false;

		if (IsTargetInDistance(m_vtMovePos, c_flStopRange))
		{
			m_navMeshAgent.ResetPath();
		}
		else
		{
			m_navMeshAgent.SetDestination(m_vtMovePos);
		}

		if (csNetSkill.SkillType == EnSkillType.Hero)
		{
			CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(Job.JobId, csNetSkill.SkillId);

			m_vtMovePos = m_vtSkillMovePos = csNetSkill.HeroPos;

			if (csNetSkill.ChainSkillId == 2 || csNetSkill.ChainSkillId == 3) // 연계스킬.
			{
				SetAttack(csJobSkill);
			}
			else
			{
				if (m_csSkillStatus.IsStatusAnim()) return false;

				if (csJobSkill.CastingMoveTypeEnum == EnCastingMoveType.Manual) // 이동가능 스킬 or 
				{
					SetAttack(csJobSkill);
					return true;
				}
				else
				{
					SetAttack(csJobSkill);
					return true;
				}
			}
		}
		else if (csNetSkill.SkillType == EnSkillType.Common)
		{
			CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.GetJobCommonSkill(csNetSkill.SkillId);
			if (csJobCommonSkill != null)
			{
				SetAttack(csJobCommonSkill);
			}
		}

		return false;
	}

	//----------------------------------------------------------------------------------------------------
	void SetAttack(CsJobSkill csJobSkill)
	{
		m_vtTargetPos = FindSkillCast().NetSkill.TargetPos;
		transform.LookAt(m_vtTargetPos);
		m_csSkillStatus.Init(csJobSkill, true, EnSkillStarter.Net, FindSkillCast().NetSkill.ChainSkillId - 1);
		Attack();
	}

	//----------------------------------------------------------------------------------------------------
	void SetAttack(CsJobCommonSkill csJobCommonSkill)
	{
		m_vtTargetPos = FindSkillCast().NetSkill.TargetPos;
		transform.LookAt(m_vtTargetPos);
		m_csSkillStatus.Init(csJobCommonSkill, true);
		Attack();
	}

	#endregion 09 Skill

	#region 10 Setting
	
	//---------------------------------------------------------------------------------------------------
	void CreateHeroHUD()
	{
		m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateHeroHUD(m_csHeroBase,
																 m_strGuildName,
																 m_nGuildMemberGrade,
																 m_nPickedSecretLetterGrade,
																 m_nPickedMysteryBoxGrade,
																 m_bDistortion,
																 m_bSafeMode,
																 m_enNationWarPlayerState,
																 m_nNoblesseId,
																 m_nDisplayTitleId);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void DamageByAbnormal(bool bSkillCancel)
	{
		Debug.Log("OtherPlayer.DamageByAbnormal     >>>    bSkillCancel  = " + bSkillCancel);
		m_vtSkillMovePos = transform.position;
		ChangeState(EnState.Idle);
		if (bSkillCancel)
		{
			OnAttackEnd();
			ChangeState(EnState.Damage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void AttackByHit(PDHitResult HitResult, float flHitTime, PDHitResult AddHitResult, bool bMyHero, bool bKnockback)
	{
		if (CsGameData.Instance.MyHeroTransform == CsIngameData.Instance.IngameManagement.GetAttacker(HitResult.attacker))  // 피격자가 MyHero 일때.
		{
			SetDamageText(HitResult);
			if (AddHitResult != null)
			{
				SetDamageText(AddHitResult);
			}
		}

		ChangedAbnormalStateEffectDamageAbsorbShields(HitResult.changedAbnormalStateEffectDamageAbsorbShields);
		if (Dead) return;

		if (m_flHpChangeTime <= flHitTime)
		{
			m_flHpChangeTime = flHitTime;
			Hp = HitResult.hp;

			if (AddHitResult != null)
			{
				if (Hp > AddHitResult.hp)
				{
					Hp = AddHitResult.hp;
				}
			}
		}

		if (Hp == 0)
		{
			DelayDead();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void AddHeroHitSkillCast(CsNetSkill csNetSkill)
	{
		if (m_listHeroAttackInfo.Count >= MAX_BEGINATTACKCOUNT) // 3개 이상인 경우 Over 처리
		{
			m_bOverAttackSkillCast = true;
			return;
		}

		m_listHeroAttackInfo.Add(new CsHeroHitSkillCast(csNetSkill));
	}

	//---------------------------------------------------------------------------------------------------
	int GetAttackInfoUserTargetCount(Transform trTrans)
	{
		int nlistCount = 0;

		for (int i = 0; i < m_listHeroAttackInfo.Count; i++)
		{
			for (int j = 0; j < m_listHeroAttackInfo[i].HitInfoNode.Count; j++)
			{
				if (m_listHeroAttackInfo[i].HitInfoNode[j].Target == trTrans)
				{
					nlistCount++;
				}
			}
		}

		return nlistCount;
	}

	//---------------------------------------------------------------------------------------------------
	//가장 최근에 받은 스킬캐스트를 반환 == 다음의 실행할 공격. (연계캐스트 여부 확인). 
	CsHeroHitSkillCast FindSkillCast()
	{
		if (m_listHeroAttackInfo.Count <= 0)
		{
			return new CsHeroHitSkillCast();
		}
		else
		{
			return m_listHeroAttackInfo[m_listHeroAttackInfo.Count-1];
		}
	}

	#endregion 10 Setting

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
