using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CsMonster : CsMoveUnit, IInteractionObject, IMonsterObjectInfo
{
	public enum EnState { Idle, Attack, Chase, Walk, Run, Damage, Dominate, Dead }
	public enum EnAnimStatus { Idle = 0, Walk, Run, Skill01, Skill02, Skill03, Skill04, Dead }
	public enum EnOwnership { None = 0, Controller = 1, Target = 2 }
	public enum EnStoryMonsterType { Normal = 1, Boss, Tame }

	static int s_nAnimatorHash_status = Animator.StringToHash("status");
	static int s_nAnimatorHash_damage = Animator.StringToHash("damage");
	static int s_nAnimatorHash_dead = Animator.StringToHash("dead");
	static int s_nAnimatorHash_rundamage = Animator.StringToHash("rundamage");
	static int s_nAnimatorHash_skip = Animator.StringToHash("skip");

	public const string c_strLayer = "Monster";
	public const string c_strTag = "Monster";

	const float c_flStopDistance = 0.3f;
	const float c_flRotationSpeed = 720f;
	const int MAX_BEGINATTACKCOUNT = 3;

	AudioSource m_audioSource = null;
	protected CsMonsterInfo m_csMonsterInfo = null;
	CapsuleCollider m_capsuleCollider = null;
	RectTransform m_rtfHUD = null;
	protected Transform m_trMyTransform = null;

	protected Vector3 m_vtPrevPos = Vector3.zero;
	Vector3 m_vtSendPrevPos = Vector3.zero;
	Vector3 m_vtMovePos = Vector3.zero;
	Vector3 m_vtNetTartgetPos = Vector3.zero;
	Vector3 m_vtSpawnedPosition = Vector3.zero;

	[SerializeField]
	float m_flAttackDistance = 2f;
	float m_flPatrolTimer = 0f;
	float m_flWalkTimer = 0f;
	float m_flRotationY = 0f;
	float m_flHpChangeTime = 0;

	[SerializeField]
	int m_nMonsterId = 0;
	int m_nMaxMentalStrength = 0;
	int m_nMentalStrength = 0;
	int m_nSkillIndex = 0;
	int m_nSamePosMoveCount = 0;
	int m_nCastSkillId = 0;
	int m_nHalidomId = 0;

	[SerializeField]
	protected bool m_bAppear = false;
	[SerializeField]
	protected bool m_bBoss = false;
	[SerializeField]
	protected bool m_bDungeonClear = false;
	[SerializeField]
	bool m_bAttack = false;
	[SerializeField]
	bool m_bAttackAble = false;
	[SerializeField]
	bool m_bMoveAble = false;
	[SerializeField]
	bool m_bReturnMode = false;

	bool m_bOverAttackSkillCast = false;
	bool m_bExclusive = false;
	
	Guid m_guidExclusiveHeroId;
	protected Guid m_guidOwnerId;
	List<CsHitSkillCast> m_listAttackInfo = new List<CsHitSkillCast>();

	[SerializeField]
	MonsterInstanceType m_enMonsterInstanceType = 0;
	[SerializeField]
	protected EnState m_enState = EnState.Idle;
	[SerializeField]
	protected EnOwnership m_enOwnerType = EnOwnership.None;

	EnOwnership m_enOwnerTypeAfterChange = EnOwnership.None;
	EnStoryMonsterType m_enStoryMonsterType = EnStoryMonsterType.Normal;

	//---------------------------------------------------------------------------------------------------
	public CsMonsterInfo MonsterInfo { get { return m_csMonsterInfo; } }
	public int MonsterId { get { return m_nMonsterId; } }
	public bool Appear { get { return m_bAppear; } set { m_bAppear = value; } }
	public int MentalStrength { get { return m_nMentalStrength; } set { m_nMentalStrength = value; OnStateChanged(); } }

	//---------------------------------------------------------------------------------------------------
	public void Init(CsMonsterInfo csMonsterInfo, long lInstanceId, MonsterInstanceType enMonsterInstanceType, int nMaxHP, int nHp, int nMaxMentalStrength, int nMentalStrength,
					 Vector3 vtSpawnedPosition, float flSpawnedRotationY, Vector3 vtPos, float RotationY, Guid guidOwnerId, EnOwnership enOwnership, bool bExclusive,
					 Guid guidExclusiveHeroId, string strExclusiveHeroName, int nNationId, bool bBoss)
	{
		SetComponent();

		CsDungeonManager.Instance.EventDungeonResult += OnEventDungeonResult;
		m_csMonsterInfo = csMonsterInfo;
		InstanceId = lInstanceId;
		m_nMonsterId = m_csMonsterInfo.MonsterId;
		m_enMonsterInstanceType = enMonsterInstanceType;
		m_nMaxHp = nMaxHP;
		Hp = nHp;
		m_nMaxMentalStrength = nMaxMentalStrength;
		MentalStrength = nMentalStrength;
		Name = m_csMonsterInfo.Name;
		Radius = m_csMonsterInfo.Radius;
		Height = m_csMonsterInfo.Height* m_csMonsterInfo.Scale;
		m_flAttackDistance = m_csMonsterInfo.AttackStopRange;

		m_trMyTransform.name = InstanceId.ToString();
		m_vtSpawnedPosition = vtSpawnedPosition;
		m_trMyTransform.position = m_vtMovePos = vtPos;
		m_flRotationY = RotationY;
		m_bExclusive = bExclusive;
		m_guidExclusiveHeroId = guidExclusiveHeroId;
		m_bBoss = bBoss;

		m_bMoveAble = m_csMonsterInfo.MoveEnabled;
		m_bAttackAble = m_csMonsterInfo.AttackEnabled;

		m_capsuleCollider.center = new Vector3(0f, Height*0.5f, 0f);
		m_capsuleCollider.height = Height;
		m_capsuleCollider.radius = Radius / m_csMonsterInfo.Scale;
		m_capsuleCollider.enabled = true;

		ChangeEulerAngles(m_flRotationY);
		m_trMyTransform.localScale = new Vector3(m_csMonsterInfo.Scale, m_csMonsterInfo.Scale, m_csMonsterInfo.Scale);

		m_guidOwnerId = guidOwnerId;
		m_enOwnerType = (EnOwnership)enOwnership;

		StartCoroutine(CsDissolveHelper.LinearDissolve(m_trMyTransform, 1f, 0f, 1f));
		CreateShadow();

		if (m_navMeshAgent != null)
		{
			NavmeshSetting();
		}

		CsGameData.Instance.ListMonsterObjectInfo.Add(this);

		switch (m_enMonsterInstanceType)
		{
		case MonsterInstanceType.NationWarMonster:
			if (CsGameData.Instance.MyHeroInfo.Nation.NationId == nNationId)
			{
				m_trMyTransform.tag = "Npc";
				m_trMyTransform.gameObject.layer = LayerMask.NameToLayer("Npc");
			}
			StartCoroutine(ActivityCollider(m_bExclusive, m_guidExclusiveHeroId, strExclusiveHeroName));
			break;
		case MonsterInstanceType.WisdomTempleQuizMonster:
			if (m_bBoss == false)
			{
				m_rtfHUD = CsDungeonManager.Instance.OnEventCreateMonsterHUD(InstanceId, m_csMonsterInfo, m_bBoss);
			}
			StartCoroutine(ActivityCollider(m_bExclusive, m_guidExclusiveHeroId, strExclusiveHeroName));
			break;
		case MonsterInstanceType.EliteDungeonMonster:
		case MonsterInstanceType.ProofOfValorBossMonster:
		case MonsterInstanceType.ProofOfValorNormalMonster:
			m_capsuleCollider.enabled = false;
			break;
		case MonsterInstanceType.WisdomTempleBossMonster:
			m_bAppear = true;
			StartCoroutine(ActivityCollider(m_bExclusive, m_guidExclusiveHeroId, strExclusiveHeroName));
			break;
		case MonsterInstanceType.DragonNestMonster:
			StartCoroutine(ActivityCollider(m_bExclusive, m_guidExclusiveHeroId, strExclusiveHeroName));
			if (m_bBoss)
			{
				BossAppearSkip();
			}
			break;
		default:
			StartCoroutine(ActivityCollider(m_bExclusive, m_guidExclusiveHeroId, strExclusiveHeroName));
			break;
		}

		if (m_animator != null)
		{
			CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

			if (acs.Length > 0)
			{
				for (int i = 0; i < acs.Length; i++)
				{
					if ((EnAnimStatus)acs[i].Key == EnAnimStatus.Idle)
					{
						acs[i].EventStateEnter += OnEventAnimStartIdle;
					}
					else
					{
						acs[i].EventStateEnter += OnEventAnimStartAttack;
					}
				}
			}
		}
	}
	
	//---------------------------------------------------------------------------------------------------
	protected virtual void Start()
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void Update()
	{
		if (m_csMonsterInfo != null)
		{
			NormalUpdate();
			m_vtPrevPos = m_trMyTransform.position;
		}
	}

	//----------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		HUDUpdatePos();
	}

	//---------------------------------------------------------------------------------------------------
	void NormalUpdate()
	{
		if (m_bAppear) return;  // 등장 연출중.		

		if (m_timer.CheckSetTimer())
		{
			MoveSendToServer();
		}

		if (m_bReturnMode)		// 복귀모드.
		{
			if (m_enState == EnState.Run)
			{
				if (IsTargetInDistance(m_vtSpawnedPosition, c_flStopDistance))
				{
					m_bReturnMode = false;
					m_capsuleCollider.enabled = true;
					m_flPatrolTimer = Time.time;
					ChangeState(EnState.Idle);
				}
			}
			else if (m_enState == EnState.Idle)
			{
				m_capsuleCollider.enabled = false;
				if (CsIngameData.Instance.TargetTransform == m_trMyTransform)
				{
					CsIngameData.Instance.IngameManagement.TartgetReset();
				}

				SetDestination(m_vtSpawnedPosition, EnState.Run); // 스폰 지역으로 이동.
			}
		}
		else
		{
			switch (m_enOwnerType)
			{
			case EnOwnership.None:
				if (m_enState == EnState.Dead)
				{
					if (GetAnimStatus() != EnAnimStatus.Dead)
					{
						ChangeState(EnState.Dead);
					}
				}
				break;
			case EnOwnership.Controller:
				if (m_enState == EnState.Idle)
				{
					IdleState();
				}
				else if (m_enState == EnState.Walk)
				{
					WalkState();
				}
				else if (m_enState == EnState.Attack || m_enState == EnState.Chase || (m_enState == EnState.Run && !m_bReturnMode))
				{
					ChangeState(EnState.Idle);
				}
				break;
			case EnOwnership.Target:
				if (m_enState == EnState.Chase)
				{
					ChaseState();
				}
				else if (m_enState == EnState.Run)
				{
					RunState();
				}
				else if (m_enState == EnState.Idle || m_enState == EnState.Walk)
				{
					if (IsPlayerOwner())
					{
						if (m_listAttackInfo.Count == 0 && m_enOwnerTypeAfterChange == EnOwnership.Controller)
						{
							m_enOwnerType = m_enOwnerTypeAfterChange;
							m_flPatrolTimer = UnityEngine.Random.Range(0, Time.time); // 바로 패트롤 시작.
							m_enOwnerTypeAfterChange = EnOwnership.None;
						}
						else
						{
							if (m_csMonsterInfo.AttackEnabled)
							{
								ChangeState(EnState.Chase);
							}
						}
					}
				}
				break;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		if (m_animator != null)
		{
			CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

			if (acs.Length > 0)
			{
				for (int i = 0; i < acs.Length; i++)
				{
					if ((EnAnimStatus)acs[i].Key == EnAnimStatus.Idle)
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

		if (CsIngameData.Instance.TargetTransform == m_trMyTransform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (CsGameData.Instance.ListMonsterObjectInfo.Contains(this))
		{
			CsGameData.Instance.ListMonsterObjectInfo.Remove(this);
		}

		if (m_rtfHUD != null)
		{
			if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.None)
			{
				CsGameEventToUI.Instance.OnEventDeleteMonsterHUD(InstanceId);
			}
			else
			{
				CsDungeonManager.Instance.OnEventDeleteMonsterHUD(InstanceId);
			}
		}

		CsDungeonManager.Instance.EventDungeonResult -= OnEventDungeonResult;

		if (Dead == false)
		{
			if (m_enMonsterInstanceType == MonsterInstanceType.FearAltarHalidomMonster)
			{
				CsDungeonManager.Instance.OnEventFearAltarHalidomMonsterKillFail();
			}
			else if (m_enMonsterInstanceType == MonsterInstanceType.OsirisMonster)
			{
				CsDungeonManager.Instance.OnEventOsirisRoomMonsterKillFail();
			}
		}

		if (m_bDungeonClear) // 던전 클리어되었을때.
		{
			if (CsGameData.Instance.ListMonsterObjectInfo.Count == 0)
			{
				//if (m_enMonsterInstanceType == MonsterInstanceType.OsirisMonster || 
				//	m_enMonsterInstanceType == MonsterInstanceType.AnkouTombMonster ||
				//	m_enMonsterInstanceType == MonsterInstanceType.TradeShipAdditionalMonster ||
				//	m_enMonsterInstanceType == MonsterInstanceType.TradeShipMonster ||
				//	m_enMonsterInstanceType == MonsterInstanceType.TradeShipObject)
				//{
				//	CsIngameData.Instance.IngameManagement.StartClearDiretion();
				//}
				//else
				//{
					CsDungeonManager.Instance.DungeonClear();
				//}
			}
		}

		m_rtfHUD = null;
		m_trMyTransform = null;
		m_csMonsterInfo = null;
		
		m_audioSource = null;
		m_capsuleCollider = null;

		m_listAttackInfo.Clear();
		StopAllCoroutines();

		base.OnDestroy();
	}

	//----------------------------------------------------------------------------------------------------
	void HUDUpdatePos()
	{
		if (m_rtfHUD != null)
		{
			if (CsIngameData.Instance.InGameCamera == null) return;

			if (m_rtfHUD.gameObject.activeInHierarchy)
			{
				m_rtfHUD.position = new Vector3(m_trMyTransform.position.x, + m_trMyTransform.position.y + Height, m_trMyTransform.position.z);

				float flDistance = (Vector3.Distance(CsIngameData.Instance.InGameCamera.transform.position, m_trMyTransform.position) - 10) / 20;
				m_rtfHUD.localScale = new Vector3(1 + flDistance, 1 + flDistance, 1 + flDistance);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnStateChanged()
	{
		if (CsIngameData.Instance.TargetTransform == m_trMyTransform) // 선택된 플레이어 정보 전달.
		{
			CsGameEventToUI.Instance.OnEventSelectInfoUpdate();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void ChangeState(EnState enNewState)
	{
		if (m_animator == null) return;

		if (enNewState != EnState.Walk && enNewState != EnState.Run)
		{
			if (m_navMeshAgent != null && m_navMeshAgent.hasPath)
			{
				m_navMeshAgent.ResetPath();
			}
		}

		if (enNewState == EnState.Idle)
		{
			m_flPatrolTimer = Time.time;
			SetAnimStatus(EnAnimStatus.Idle);
		}
		else if (enNewState == EnState.Walk)
		{
			m_flWalkTimer = Time.time;
			m_navMeshAgent.speed = m_csMonsterInfo.MoveSpeed / 100;
			SetAnimStatus(EnAnimStatus.Walk);
		}
		else if (enNewState == EnState.Run)
		{
			m_navMeshAgent.speed = m_csMonsterInfo.BattleMoveSpeed / 100;
			SetAnimStatus(EnAnimStatus.Run);
		}
		else if (enNewState == EnState.Attack)
		{			
			SetAnimStatus((EnAnimStatus)((int)EnAnimStatus.Skill01 + m_nSkillIndex));
		}
		else if (enNewState == EnState.Damage)
		{
			SetAnimStatus(EnAnimStatus.Idle);
			m_animator.SetTrigger(s_nAnimatorHash_damage);
		}
		else if (enNewState == EnState.Dead)
		{
			SetAnimStatus(EnAnimStatus.Dead);
			m_animator.SetTrigger(s_nAnimatorHash_dead);
		}

		m_enState = enNewState;
	}

	//---------------------------------------------------------------------------------------------------
	public override void AttackByHit(PDHitResult HitResult, float flHitTime, PDHitResult AddHitResult, bool bMyHero, bool bKnockback)
	{
		RemoveAbnormalEffect(HitResult.removedAbnormalStateEffects); // 이팩트 제거.

		if (CsGameData.Instance.MyHeroTransform == CsIngameData.Instance.IngameManagement.GetAttacker(HitResult.attacker))  // 피격자가 MyHero 일때.
		{
			SetDamageText(HitResult);

			if (AddHitResult != null)
			{
				SetDamageText(AddHitResult);
			}
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

			if (bMyHero)
			{
				ShowParticleEffect(); // 몬스터 쉐이더로 데미지 히트 처리.-> 이동가능 몬스터에서만 처리하던것 고정형 몬스터에도 쉐이더 적용되게 바꿈. 문제시 수정필요.

				if (m_bBoss == false && m_rtfHUD == null)   // HUD가 없거나 보스몬스터가 아닌경우 HP바용 HUD 생성.
				{
					m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateMonsterHUD(InstanceId, m_csMonsterInfo, "", false, Hp);
				}
			}

			if (Hp == 0)
			{
				StartCoroutine(DelayDead(0));
			}
			else
			{
				if (bMyHero)
				{
					if (m_enMonsterInstanceType == MonsterInstanceType.OsirisMonster) return;

					if (m_listAttackInfo.Count == 0)
					{
						if (m_animator == null || m_bAttack || m_bBoss) return;

						if (m_enState == EnState.Idle || m_enState == EnState.Walk || m_enState == EnState.Run)
						{
							m_animator.SetTrigger(s_nAnimatorHash_rundamage);
						}
					}
				}
			}
		}


	}

	//---------------------------------------------------------------------------------------------------
	protected override void DamageByAbnormal(bool bSkillCancel)
	{
		if (bSkillCancel)
		{
			if (m_animator != null && m_bBoss == false)
			{
				m_animator.SetTrigger(s_nAnimatorHash_rundamage);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnAnimStatus en) { m_animator.SetInteger(s_nAnimatorHash_status, (int)en); }
	protected EnAnimStatus GetAnimStatus() { return (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status); }

	bool IsTagSkill() { return (m_animator.GetCurrentAnimatorStateInfo(0).IsTag("skill")); }
	bool IsIdleState() { return m_enState == EnState.Idle; }
	bool IsStoryBossMonster() { return m_enStoryMonsterType == EnStoryMonsterType.Boss; }
	bool IsStoryTameMonster() { return m_enStoryMonsterType == EnStoryMonsterType.Tame; }
	bool IsStoryDungeonMonster() { return m_enMonsterInstanceType == MonsterInstanceType.StoryDungeonMonster; }
	bool IsExpDungeonLakChargeMonster() { return m_enMonsterInstanceType == MonsterInstanceType.ExpDungeonLakChargeMonster; }

	//---------------------------------------------------------------------------------------------------
	public void BossMotionSetting(EnAnimStatus en)
	{
		SetAnimStatus(en);
	}

	//---------------------------------------------------------------------------------------------------
	public void BossStateSetting(bool bBossAppearance)
	{
		m_bAppear = bBossAppearance;
		if (m_bAppear)
		{
			transform.LookAt(CsGameData.Instance.MyHeroTransform);
		}

		ChangeState(EnState.Idle);
	}

	#region State
	//---------------------------------------------------------------------------------------------------
	protected void IdleState()
	{
		if (m_animator == null) return;

		if (m_bDungeonClear)
		{
			StartCoroutine(DelayDead(0));
			return;
		}

		if (SetAttack() == false)
		{
			if (IsPlayerOwner())
			{
				if (IsMoveAble() && m_bBoss == false)
				{
					if (m_csMonsterInfo.PatrolRange == 0) return;

					if (Time.time - m_flPatrolTimer > m_csMonsterInfo.PatrolRange)
					{
						Vector3 vtMovePos = RandomMovePos(m_vtSpawnedPosition, m_csMonsterInfo.PatrolRange); // 반경

						if (!IsTargetInDistance(vtMovePos, 2f))
						{
							m_flPatrolTimer = Time.time;
							SetDestination(vtMovePos, EnState.Walk);
						}
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void WalkState()
	{
		if (!IsMoveAble()) return;

		if (IsPlayerOwner())
		{
			if (Time.time - m_flWalkTimer > 5)	// 5초이상 워크 상태일경우 초기화.
			{
				m_vtMovePos = transform.position;
				ChangeState(EnState.Idle);
				return;
			}
		}

		if (UpdataeMove(m_vtMovePos, c_flStopDistance))
		{
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void ChaseState()
	{
		if (IsPlayerOwner())
		{
			if (m_csMonsterInfo.AttackEnabled)
			{
				if (!IsTargetInDistance(CsGameData.Instance.MyHeroTransform.position, m_flAttackDistance))
				{
					if (IsMoveAble())
					{
						SetDestination(CsGameData.Instance.MyHeroTransform.position, EnState.Run);
					}
				}
				else
				{
					if (SetAttack())
					{
						LookAtPosition(CsGameData.Instance.MyHeroTransform.position);
						ChangeState(EnState.Idle);
					}
				}
			}
			else
			{
				ChangeState(EnState.Idle);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void RunState()
	{
		if (!IsMoveAble())
		{
			if (!Dead)
			{
				ChangeState(EnState.Idle);
			}
			return;
		}

		if (IsPlayerOwner())
		{
			if (m_vtMovePos != CsGameData.Instance.MyHeroTransform.position && !IsTargetInDistance(CsGameData.Instance.MyHeroTransform.position, m_flAttackDistance))
			{
				if (IsMoveAble())
				{
					SetDestination(CsGameData.Instance.MyHeroTransform.position, EnState.Run);
				}
			}
		}

		if (UpdataeMove(m_vtMovePos, m_flAttackDistance))
		{
			if (m_bReturnMode) // 스폰 위치 복귀.
			{
				m_bReturnMode = false;
				m_capsuleCollider.enabled = true;
				m_flPatrolTimer = Time.time;
				ChangeState(EnState.Idle);
			}
			else
			{
				if (SetAttack() == false)
				{
					m_flPatrolTimer = Time.time;
					ChangeState(EnState.Idle);
				}
			}
		}
	}

	#endregion State

	#region Anim

	void OnAnimRunRightSound() { }
	void OnAnimRunLeftSound() { }
	//---------------------------------------------------------------------------------------------------
	protected void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		if (m_bAttack)
		{
			AttackEnd();
			ChangeState(EnState.Idle);

			if (Hp == 0)
			{
				StartCoroutine(DelayDead(0));
			}
		}
		else if (m_bAppear)
		{
			m_bAppear = false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void AttackEnd()
	{
		m_bAttack = false;
		m_bOverAttackSkillCast = false;
		if (m_listAttackInfo.Count > 0)
		{
			m_listAttackInfo.RemoveAt(0);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		m_bAttack = true;
		OnAnimAttackSound(1);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId)
	{
		if (CsIngameData.Instance.EffectSound && m_audioSource != null)
		{
			AudioClip acHitSound = CsIngameData.Instance.IngameManagement.GetMonsterSound("SFX_Mon_Hit_Public");
			if (acHitSound != null)
			{
				m_audioSource.PlayOneShot(acHitSound);
			}
			else
			{
				Debug.Log("OnAnimApplyDamage    acHitSound == null");
			}
		}

		if (m_listAttackInfo.Count == 0)
		{
			return;
		}

		if (nHitId == 0)
		{
			nHitId = 1;
		}

		bool bMyPlayerTarget = false;
		for (int i = 0; i < m_listAttackInfo.Count; i++)
		{
			CsHitSkillCast FindSkillCast = m_listAttackInfo[i];

			if (FindSkillCast != null)
			{
				for (int j = 0; j < FindSkillCast.HitInfoNodeList.Count; j++)
				{
					CsHitInfoNode csHitInfoNode = FindSkillCast.HitInfoNodeList[j];
					if (csHitInfoNode != null)
					{
						if (csHitInfoNode.HitResult != null && csHitInfoNode.HitResult.hitId == nHitId && csHitInfoNode.Target != null)
						{
							CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, false);
							if (bMyPlayerTarget == false && csHitInfoNode.Target == CsGameData.Instance.MyHeroTransform)
							{
								bMyPlayerTarget = true;
							}
						}
					}
				}
				break;
			}
		}

		if (bMyPlayerTarget )
		{
			Debug.Log("OnAnimApplyDamage   >>>   bMyPlayerTarget   : "+ m_rtfHUD);
			if (m_rtfHUD == null)
			{
				m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateMonsterHUD(InstanceId, m_csMonsterInfo, "", false, Hp);
			}
			else
			{
				CsGameEventToUI.Instance.OnEventMonsterAttackToMyHero(InstanceId);
			}
		}

		if (IsPlayerOwner())
		{
			HitEffect(CsGameData.Instance.MyHeroTransform.position);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void HitEffect(Vector3 vtTarget)
	{
		if (CsIngameData.Instance.Effect != 0)
		{
			if (m_csMonsterInfo.AttackStopRange < 4)	// 근거리 일반 공격.
			{
				float flDistance = Vector3.Distance(vtTarget, m_trMyTransform.position);
				flDistance = flDistance > 1 ? flDistance - 1 : 1;
				Vector3 vtPos = transform.position;
				Vector3 vtDir = transform.forward * flDistance;			
				
				vtDir.y = Height * 0.5f; // 플레이어 허리 높이.
				vtPos = vtPos + vtDir;

				CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, transform, vtPos, Quaternion.identity, "Monster_Hit", 0.5f);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimSkillEffect() // 스킬 Effect.
	{
		if (CsIngameData.Instance.Effect != 0)
		{
			string stEffect = "Monster_" + m_csMonsterInfo.MonsterCharacter.MonsterCharacterId.ToString() + "_Skill0" + (m_nSkillIndex + 1).ToString();
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, m_trMyTransform, m_trMyTransform.position, stEffect, 0.8f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimBulletEffect() // 원거리 스킬 Effect.
	{
		if (CsIngameData.Instance.Effect != 0)
		{
			Vector3 vtTargetPos = CsGameData.Instance.MyHeroTransform.position;
			LookAtPosition(vtTargetPos);

			if (IsPlayerOwner() == false)
			{
				if (CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId) == null) return;

				vtTargetPos = CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId).transform.position;
			}

			// 마두루카 주술사, 금안족 블로, 금안족 플룻, 흑랑 도적단 궁수.
			string stEffect = "Monster_" + m_csMonsterInfo.MonsterCharacter.MonsterCharacterId.ToString() + "_Skill0" + (m_nSkillIndex + 1).ToString();
			string strHitEffect = "Monster_Bullet_Hit";

			if (m_csMonsterInfo.MonsterCharacter.MonsterCharacterId == 3 && m_nSkillIndex == 0)
			{
				strHitEffect = stEffect + "_Hit";
			}

			//bool bSmoke = m_csMonsterInfo.MonsterCharacter.MonsterCharacterId == 67 ? true : false;

			float flOffestHeight = 0;
			if (m_csMonsterInfo.MonsterCharacter.MonsterCharacterId == 3)
			{
				flOffestHeight = -0.35f;
			}
			else if (m_csMonsterInfo.MonsterCharacter.MonsterCharacterId == 67)
			{
				flOffestHeight = 0.7f;
			}

			CsEffectPoolManager.Instance.PlayMoveHitEffect(m_trMyTransform, vtTargetPos, stEffect, strHitEffect, flOffestHeight, true);

			Debug.Log("OnAnimBulletEffect()     " + stEffect);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimAttackSound(int nIndex)
	{
		if (CsIngameData.Instance.EffectSound)
		{
			//if (!IsPlayerOwner() || CsIngameData.Instance.TargetTransform != m_trMyTransform) return; // Owner가 아니거나 타겟이 아닐때.
			//if (m_aaudioClipSkills == null || m_aaudioClipSkills.Length == 0) return; // 사운드가 없을때.

			//m_audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.08f);
			//m_audioSource.volume = m_aflVolume[nIndex - 1];
			//m_audioSource.PlayOneShot(m_aaudioClipSkills[nIndex - 1]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadEnd()
	{
		m_listAttackInfo.Clear();
		StartCoroutine(CsDissolveHelper.LinearDissolve(m_trMyTransform, 0f, 1f, 2f));

		if (m_bDungeonClear) // 던전 클리어되었을때.
		{
			if (IsStoryBossMonster())
			{
				//CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);
				//CsIngameData.Instance.IngameManagement.StartClearDiretion();
			}
			else
			{
				if (CsGameData.Instance.ListMonsterObjectInfo.Count <= 1 || m_enMonsterInstanceType == MonsterInstanceType.AncientRelicBossMonster)
				{
					//if (m_enMonsterInstanceType == MonsterInstanceType.OsirisMonster ||
					//	m_enMonsterInstanceType == MonsterInstanceType.TradeShipAdditionalMonster ||
					//	m_enMonsterInstanceType == MonsterInstanceType.TradeShipMonster ||
					//	m_enMonsterInstanceType == MonsterInstanceType.TradeShipObject)
					//{
					//	CsIngameData.Instance.IngameManagement.StartClearDiretion();
					//}
					//else
					//{
						CsDungeonManager.Instance.DungeonClear();
					//}
				}
			}
		}

		if (CsGameData.Instance.ListMonsterObjectInfo.Contains(this))
		{
			CsGameData.Instance.ListMonsterObjectInfo.Remove(this);
		}

		if (IsStoryBossMonster()) return;
		Invoke(SendMonsterDead, 4f);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound()
	{
		if (CsIngameData.Instance.EffectSound)
		{
			//if (!m_bTarget) return; // 타겟이 아니 였을때.
			//if (m_audioClipDead == null) return; // 사운드가 없을때.

			//m_audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.08f);
			//m_audioSource.volume = m_csMonsterData.MonsterCharacterId.DeadSoundVolume;
			//m_audioSource.PlayOneShot(m_audioClipDead);
		}
	}

	#endregion Anim

	#region Move

	int m_nStopCount = 0;
	float m_flMoveTimer = 0;
	//---------------------------------------------------------------------------------------------------
	protected bool UpdataeMove(Vector3 vtPos, float flStopDistance)
	{
		if (IsTargetInDistance(vtPos, flStopDistance))
		{
			if (IsPlayerOwner()) return true;

			if (m_enState == EnState.Run && IsTargetInDistance(m_vtNetTartgetPos, flStopDistance)) return true;  // 타겟과의 거리가 flStopDistance 보다 멀때.

			if (m_nStopCount == 0)
			{
				m_flMoveTimer = Time.time;
				m_nStopCount = 1;
			}
			else if (Time.time > m_flMoveTimer + 0.3f) // 몬스터 MoveEvent는 0.25초마다 전달하기 때문에 0.3초 체크.
			{
				m_nStopCount = 0;
				return true;
			}
		}
		else
		{
			if (IsPlayerOwner() && m_vtPrevPos == m_trMyTransform.position)
			{
				m_nSamePosMoveCount++;
				if (m_nSamePosMoveCount > 50)
				{
					m_vtMovePos = m_trMyTransform.position;
					m_nSamePosMoveCount = 0;
					m_navMeshAgent.ResetPath();
					ChangeState(EnState.Idle);
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void SetDestination(Vector3 vtPos, EnState enNewState)
	{
		if (m_vtMovePos != vtPos || !m_navMeshAgent.hasPath) // 같은 값 중복 입력 방지.
		{
			m_nSamePosMoveCount = 0;
			m_vtMovePos = vtPos;

			m_navMeshAgent.SetDestination(vtPos);

			if (m_enState != enNewState)
			{
				m_nStopCount = 0;
				ChangeState(enNewState);
			}
		}
	}

	#endregion Move

	#region Net

	//---------------------------------------------------------------------------------------------------
	public void OnEventDungeonResult(bool bSuccess)
	{
		if (bSuccess) // 서버에서 완료시 몬스터를 삭제하기 때문에 완료시 연출위해 임의 처리.
		{
			m_bDungeonClear = true;
			m_guidOwnerId = CsGameData.Instance.MyHeroInfo.HeroId;
			m_enOwnerType = EnOwnership.Controller;
			StartCoroutine(DelayDead(0));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void BossAppearSkip()
	{
		m_animator.SetTrigger(s_nAnimatorHash_skip);
		m_navMeshAgent.ResetPath();
		BossStateSetting(false);
		m_bAppear = false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void MoveSendToServer()
	{
		if (IsPlayerOwner())
		{
			if (m_vtSendPrevPos != m_trMyTransform.position || m_flRotationY != Mathf.Abs(m_trMyTransform.eulerAngles.y))
			{
				SendMonsterMoveEvent();
			}
			m_vtSendPrevPos = m_trMyTransform.position;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendMonsterMoveEvent()
	{
		CEBMonsterMoveEventBody csEvt = new CEBMonsterMoveEventBody();
		csEvt.instanceId = InstanceId;
		csEvt.position = CsRplzSession.Translate(m_trMyTransform.position);
		csEvt.rotationY = m_flRotationY = Mathf.Abs(m_trMyTransform.eulerAngles.y);
		CsRplzSession.Instance.Send(ClientEventName.MonsterMove, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterMove(Vector3 vtPos, float flRotationY)
	{
		//Debug.Log("CsMonster.NetEventMonsterMove()   Id = "+InstanceId);
		if (IsPlayerOwner()) return;
	
		m_flRotationY = flRotationY;
		m_nStopCount = 0;

		if (vtPos != m_vtMovePos)
		{
			if (IsMoveAble())
			{
				if (m_enOwnerType == EnOwnership.Target)
				{
					SetDestination(vtPos, EnState.Run);
				}
				else if (m_enOwnerType == EnOwnership.Controller)
				{
					SetDestination(vtPos, EnState.Walk);
				}
			}
		}
		else
		{
			ChangeEulerAngles(m_flRotationY);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterSkillCast(Vector3 vtTargetPos, int nSkillId)
	{
		m_nCastSkillId = nSkillId;
		m_vtNetTartgetPos = vtTargetPos;

		for (int i = 0; i < m_csMonsterInfo.MonsterOwnSkillList.Count; i++) // 낮은 순번의 스킬이 리스트에 있다는 전제를 기준으로 구성.
		{
			if (m_csMonsterInfo.MonsterOwnSkillList[i].SkillId == m_nCastSkillId)
			{
				m_nSkillIndex = i;
				break;
			}
		}

		AddMonsterHitSkillCast(nSkillId);

		if (!m_bAttack)
		{
			if (IsPlayerOwner())
			{
				LookAtPosition(CsGameData.Instance.MyHeroTransform.position);
			}
			else
			{
				LookAtPosition(vtTargetPos);
			}

			ChangeState(EnState.Attack);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventChangeOwnership(Guid guidOwnerId, EnOwnership NewOwnership)
	{
		if (m_csMonsterInfo == null) return;

		m_guidOwnerId = guidOwnerId;
		EnOwnership enOldOwnerType = m_enOwnerType;
		m_enOwnerType = NewOwnership;

		if (m_bExclusive)
		{
			if (m_guidOwnerId == m_guidExclusiveHeroId)
			{
				m_capsuleCollider.enabled = true;
			}
		}

		if (NewOwnership == EnOwnership.None && Dead == false)
		{
			ChangeState(EnState.Idle);
		}

		m_flAttackDistance = c_flStopDistance;

		if (IsPlayerOwner())
		{
			if (NewOwnership == EnOwnership.Target)
			{
				if (m_csMonsterInfo.AttackEnabled)
				{
					m_flAttackDistance = m_csMonsterInfo.AttackStopRange;
					ChangeState(EnState.Chase);
				}
				else
				{
					ChangeState(EnState.Idle);
				}
			}
			else if (NewOwnership == EnOwnership.Controller)
			{
				if (enOldOwnerType == EnOwnership.Target)
				{
					if (m_listAttackInfo.Count > 0)
					{
						m_enOwnerType = enOldOwnerType;
						m_enOwnerTypeAfterChange = NewOwnership;
						return;
					}
				}

				m_flPatrolTimer = UnityEngine.Random.Range(0, Time.time); // 바로 패트롤 시작.
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterReturnToSpawnedPosition(bool bReturnMode) // 스폰 위치로 이동.
	{
		m_bReturnMode = bReturnMode;
		if (bReturnMode)
		{
			m_capsuleCollider.enabled = false;
			if (CsIngameData.Instance.TargetTransform == m_trMyTransform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}

			SetDestination(m_vtSpawnedPosition, EnState.Run); // 스폰 지역으로 이동.
		}
		else
		{
			m_capsuleCollider.enabled = true;
			m_vtMovePos = m_trMyTransform.position;
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTargetHeroDead(Transform trTarget)
	{
		for (int i = 0; i < m_listAttackInfo.Count; i++)
		{
			CsHitSkillCast FindSkillCast = m_listAttackInfo[i];
			if (FindSkillCast != null)
			{
				FindSkillCast.HitInfoNodeList.RemoveAll(a => a.Target == trTarget);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHitApprove(PDHitResult HitResult, Transform trTarget)
	{
		if (m_listAttackInfo.Count == 0)
		{
			CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult, Time.time, null, false, false);
		}
		else
		{
			if (m_bOverAttackSkillCast) // skill 케스트 정보가 3회 누적.
			{
				m_bOverAttackSkillCast = false;
				List<CsHitInfoNode> listCsHitInfoNode = m_listAttackInfo[m_listAttackInfo.Count - 1].HitInfoNodeList;

				if (listCsHitInfoNode.Count > 0)
				{
					listCsHitInfoNode[listCsHitInfoNode.Count - 1].AddHitResult = HitResult;
				}
				else
				{
					CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult, Time.time, null, false, false);
				}
			}
			else
			{
				CsHitSkillCast csHitSkillCast = m_listAttackInfo.Find(a => a.SkillId == HitResult.skillId);
				if (csHitSkillCast == null) 
				{
					CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult, Time.time, null, false, false);
				}
				else
				{
					csHitSkillCast.HitInfoNodeList.Add(new CsHitInfoNode(trTarget, HitResult, Time.time));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterDead()
	{
		StartCoroutine(DelayDead(2.5f)); // 죽어야 하는데 죽지 않는 경우 방지.
	}

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator DelayDead(float flDelayTime)
	{
		yield return new WaitForSeconds(flDelayTime);
		if (Dead == false)
		{
			Dead = true;
			m_flHpChangeTime = Time.time;
			Hp = 0;
			m_navMeshAgent.enabled = false;
			m_capsuleCollider.isTrigger = false;
			m_capsuleCollider.enabled = false;

			SkinnedMeshRenderer[] renderer = m_trMyTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (renderer != null && renderer.Length != 0)
			{
				CsDissolveHelper.ResetBrilliantly(renderer[0].material);
			}

			if (IsExpDungeonLakChargeMonster())
			{
				CsDungeonManager.Instance.LakMonsterDead();
			}

			if (m_enMonsterInstanceType == MonsterInstanceType.FearAltarHalidomMonster)
			{
				CsDungeonManager.Instance.OnEventFearAltarHalidomMonsterKill();
			}
			else if (m_enMonsterInstanceType == MonsterInstanceType.OsirisMonster)
			{
				CsIngameData.Instance.IngameManagement.CreateGold(transform);
			}

			if (CsIngameData.Instance.TargetTransform == m_trMyTransform)
			{
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}

			if (!m_bAttack && m_listAttackInfo.Count != 0) // 누락 공격 정보 처리.
			{
				for (int i = 0; i < m_listAttackInfo.Count; i++)
				{
					CsHitSkillCast FindSkillCast = m_listAttackInfo[i];
					for (int j = 0; j < FindSkillCast.HitInfoNodeList.Count; j++)
					{
						CsHitInfoNode csHitInfoNode = FindSkillCast.HitInfoNodeList[j];
						if (csHitInfoNode.HitResult != null && csHitInfoNode.Target != null)
						{
							CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, false);
						}
					}
				}
			}

			if (IsStoryBossMonster())
			{
				if (m_enState != EnState.Dead)
				{
					ChangeState(EnState.Dead);
				}
				CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.BossDead);
				CsGameEventToUI.Instance.OnEventHideMainUI(true);
				yield return new WaitForSeconds(3f);
			}
			else
			{
				if (m_csMonsterInfo.MonsterCharacter.PrefabName != "mon_11001_Barricade") // 임시 하드코딩. 0609
				{
					if (m_enState != EnState.Dead)
					{
						if (m_animator != null)
						{
							ChangeState(EnState.Dead);
							yield return new WaitForSeconds(2.5f);
						}
					}
				}
			}

			if (m_csMonsterInfo.MonsterCharacter.PrefabName != "mon_11001_Barricade") // 임시 하드코딩. 0609
			{
				yield return new WaitForSeconds(1.5f);
			}

			SendMonsterDead();
			m_listAttackInfo.Clear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterMentalHit(Guid guTamerId, int nMentalStrength, int nMentalStrengthDamage, long[] alRemovedAbnormalStateEffects)
	{		
		MentalStrength = nMentalStrength;
		if (CsIngameData.Instance.InGameCamera.SleepMode == false)
		{
			float flHeight = Height > 3 ? 3 : Height;
			Vector3 vtPos = new Vector3(m_trMyTransform.position.x, m_trMyTransform.position.y + flHeight, m_trMyTransform.position.z);
			CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nMentalStrengthDamage, Camera.main.WorldToScreenPoint(vtPos), false);  // 데미지 Text 처리.
		}

		RemoveAbnormalEffect(alRemovedAbnormalStateEffects); // 상태이상 이펙스 삭제.

		if (MentalStrength == 0)
		{
			ChangeState(EnState.Idle);
			Dead = true;
			m_capsuleCollider.isTrigger = false;
			m_capsuleCollider.enabled = false;
			m_navMeshAgent.enabled = false;

			CsIngameData.Instance.IngameManagement.TartgetReset();

			if (guTamerId == CsGameData.Instance.MyHeroInfo.HeroId)
			{
				gameObject.AddComponent<CsTameMonster>();
				m_trMyTransform.GetComponent<CsTameMonster>().enabled = true;
				m_trMyTransform.GetComponent<CsMonster>().enabled = false;
				m_trMyTransform.GetComponent<CsTameMonster>().Init(this, guTamerId);
			}

			if (!m_bAttack && m_listAttackInfo.Count != 0) // 누락 공격 정보 처리.
			{
				Debug.Log("DelayDead   m_listAttackInfo  = " + m_listAttackInfo.Count + " // OwnerType = " + m_enOwnerType);
				for (int i = 0; i < m_listAttackInfo.Count; i++)
				{
					CsHitSkillCast FindSkillCast = m_listAttackInfo[i];
					for (int j = 0; j < FindSkillCast.HitInfoNodeList.Count; j++)
					{
						CsHitInfoNode csHitInfoNode = FindSkillCast.HitInfoNodeList[j];
						if (csHitInfoNode.HitResult != null && csHitInfoNode.Target != null)
						{
							CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, false);
						}
					}
				}
			}

			m_listAttackInfo.Clear();

			if (CsGameData.Instance.ListMonsterObjectInfo.Contains(this))
			{
				CsGameData.Instance.ListMonsterObjectInfo.Remove(this);
			}

			return;
		}

		ShowParticleEffect();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterAbnormalStateEffectStart(long lAbnormalStateEffectId, int nAbnormalStateId, int nSourceJobId, int nAbnormalLevel, int nDamageAbsorbShieldRemainingAbsorbAmount, float flRemainTime, long[] alRemovedAbnormalStateEffects)
	{
		RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
		AbnormalSet(nAbnormalStateId, lAbnormalStateEffectId, nSourceJobId, nAbnormalLevel, 0,flRemainTime, true);

		if (IsMoveAble() == false)
		{
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterAbnormalStateEffectHit(int nHp, long[] alRemovedAbnormalStateEffects,
													  long lAbnormalStateEffectInstanceId, int nDamage, int nHpDamage, Guid guidAttackerId)
	{
		Debug.Log("NetEventMonsterAbnormalStateEffectHit      Hp = " + Hp + "  //  nHp = " + nHp + "  //  nHpDamage = " + nHpDamage);

		if (CsIngameData.Instance.InGameCamera.SleepMode == false)
		{
			float flHeight = Height > 3 ? 3 : Height;
			Vector3 vtPos = new Vector3(m_trMyTransform.position.x, m_trMyTransform.position.y + flHeight, m_trMyTransform.position.z);
			CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nDamage, Camera.main.WorldToScreenPoint(vtPos), false);  // 데미지 Text 처리.
		}

		RemoveAbnormalEffect(alRemovedAbnormalStateEffects); // 상태이상 이펙스 삭제.

		if (m_flHpChangeTime <= Time.time)
		{
			m_flHpChangeTime = Time.time;
			Hp = nHp;
		}

		if (nHp == 0)
		{
			StartCoroutine(DelayDead(0));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterAbnormalStateEffectFinished(long lAbnormalStateEffectInstanceId)
	{
		RemoveAbnormalEffect(lAbnormalStateEffectInstanceId); // 상태이상 이펙스 삭제.
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventMonsterSummonSkillCast()
	{
		Debug.Log("NetMonsterSummonSkillCast()    몬스터 소환 스킬 시전 시작.");
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDungeonClear()
	{
		StartCoroutine(DelayDead(0));
	}
	
	//----------------------------------------------------------------------------------------------------
	public void NetEventStartEliteDungeon()
	{
		if (m_guidOwnerId == m_guidExclusiveHeroId)
		{
			m_capsuleCollider.enabled = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void NetEventStartProofOfValor()
	{
		if (m_guidOwnerId == m_guidExclusiveHeroId)
		{
			m_capsuleCollider.enabled = true;
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void NetEventSetStoryDungeonMoneter(int nStoryMonsterType)
	{
		m_enStoryMonsterType = (EnStoryMonsterType)nStoryMonsterType;
		m_trMyTransform.LookAt(CsGameData.Instance.MyHeroTransform);

		if (m_enStoryMonsterType == EnStoryMonsterType.Boss) // 보스 몬스터,
		{
			CsGameEventToUI.Instance.OnEventCreateBossMonster(InstanceId, m_csMonsterInfo);
		}
		else
		{
			m_rtfHUD = CsDungeonManager.Instance.OnEventCreateMonsterHUD(InstanceId, m_csMonsterInfo, false, m_nHalidomId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void NetEventSetFearAltarHalidomMonster(int nHalidomId)
	{
		m_nHalidomId = nHalidomId;
		m_enMonsterInstanceType = MonsterInstanceType.FearAltarHalidomMonster;
		m_rtfHUD = CsDungeonManager.Instance.OnEventCreateMonsterHUD(InstanceId, m_csMonsterInfo, false, m_nHalidomId);
	}

	#endregion Net

	#region Setting

	//---------------------------------------------------------------------------------------------------
	void SetComponent()
	{
		m_trMyTransform = transform;
		m_capsuleCollider = m_trMyTransform.GetComponent<CapsuleCollider>();
		m_audioSource = m_trMyTransform.GetComponent<AudioSource>();
		m_audioSource.volume = 0.3f;

		m_animator = m_trMyTransform.GetComponent<Animator>();
		m_navMeshAgent = m_trMyTransform.GetComponent<NavMeshAgent>();
		m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	bool SetAttack()
	{
		if (m_bAttack) return false;

		if (m_listAttackInfo.Count > 0)
		{
			CsHitSkillCast csHitSkillCast = m_listAttackInfo[0];

			if (csHitSkillCast != null)
			{
				for (int i = 0; i < m_csMonsterInfo.MonsterOwnSkillList.Count; i++) // 낮은 순번의 스킬이 리스트에 있다는 전제를 기준으로 구성.
				{
					if (m_csMonsterInfo.MonsterOwnSkillList[i].SkillId == csHitSkillCast.SkillId)
					{
						m_nSkillIndex = i;
						break;
					}
				}

				ChangeState(EnState.Attack);
				return true;
			}
			else
			{
				Debug.Log("CsMonster.SetAttack()                csHitSkillCast == null     문제있음 확인필요.   (홍은기)");
			}
		}
		return false;
	}
	
	//---------------------------------------------------------------------------------------------------
	IEnumerator ActivityCollider(bool bExclusive, Guid guidExclusiveHeroId, string strExclusiveHeroName)
	{
		if (bExclusive) // 소환 몬스터인경우.
		{
			m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateMonsterHUD(InstanceId, m_csMonsterInfo, strExclusiveHeroName, bExclusive, Hp);

			if (guidExclusiveHeroId == CsGameData.Instance.MyHeroInfo.HeroId)
			{
				m_capsuleCollider.enabled = true;
			}
			else
			{
				m_capsuleCollider.enabled = false;
			}
		}
		else
		{
			m_capsuleCollider.enabled = false;
			yield return new WaitForSeconds(1f);
			m_capsuleCollider.enabled = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool IsMoveAble()
	{
		return (!m_bAttack && !Dead && m_animator != null && !m_bStun  && !m_bBlockade && !m_bSuppression && m_csMonsterInfo.MoveEnabled);
	}

	//---------------------------------------------------------------------------------------------------
	bool IsPlayerOwner()
	{
		if (CsGameData.Instance.MyHeroTransform == null)
		{
			return false;
		}
		return m_guidOwnerId == CsGameData.Instance.MyHeroInfo.HeroId;
	}

	//---------------------------------------------------------------------------------------------------
	void SendMonsterDead()
	{
		CsIngameData.Instance.IngameManagement.MonsterDead(InstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void EscapeMonsterDead()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void NavmeshSetting()
	{
		m_navMeshAgent.enabled = false;
		m_navMeshAgent.angularSpeed = 2000f;
		m_navMeshAgent.acceleration = 100f;
		m_navMeshAgent.stoppingDistance = 0.1f;
		m_navMeshAgent.autoBraking = true;
		m_navMeshAgent.autoRepath = true;
		m_navMeshAgent.radius = 0.3f / m_csMonsterInfo.Scale;
		m_navMeshAgent.height = Height / m_csMonsterInfo.Scale;
		m_navMeshAgent.baseOffset = 0f;
		m_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
		m_navMeshAgent.avoidancePriority = 40;
		m_navMeshAgent.speed = m_csMonsterInfo.MoveSpeed / 100;
		m_navMeshAgent.enabled = true;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SpawnEffect()
	{
		yield return new WaitForSeconds(1f);
		if (CsIngameData.Instance.Effect != 0)
		{
			CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, m_trMyTransform, m_trMyTransform.position, m_trMyTransform.rotation, "Spawn", 1f);
			yield return new WaitForSeconds(1f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ShowParticleEffect()
	{
		SkinnedMeshRenderer[] renderer = m_trMyTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (renderer != null && renderer.Length != 0)
		{
			StartCoroutine(CsDissolveHelper.Brilliantly(renderer[0].material, 100f, 0.2f));
		}
	}

	//---------------------------------------------------------------------------------------------------
	int GetAttackInfoUserTargetCount(Transform trTarget)
	{
		int nlistCount = 0;
		for (int i = 0; i < m_listAttackInfo.Count; i++)
		{
			for (int j = 0; j < m_listAttackInfo[i].HitInfoNodeList.Count; j++)
			{
				if (m_listAttackInfo[i].HitInfoNodeList[j].Target == trTarget)
				{
					nlistCount++;
				}
			}
		}
		return nlistCount;
	}

	//---------------------------------------------------------------------------------------------------
	int GetTragetCount(List<CsHitInfoNode> listHitInfoNode, Transform trTarget)
	{
		int nNodeCount = 0;
		for (int i = 0; i < listHitInfoNode.Count; i++)
		{
			if (listHitInfoNode[i].Target == trTarget)
			{
				nNodeCount++;
			}
		}
		return nNodeCount;
	}

	//---------------------------------------------------------------------------------------------------
	void AddMonsterHitSkillCast(int nSkillId)
	{
		if (m_listAttackInfo.Count >= MAX_BEGINATTACKCOUNT) // 3개 이상인 경우 Over 처리
		{
			m_bOverAttackSkillCast = true;
		}
		else
		{
			m_listAttackInfo.Add(new CsHitSkillCast(nSkillId, 0));
		}
	}

	//----------------------------------------------------------------------------------------------------
	public static bool IsMonster(int nMonsterId, Transform trMonster)
	{
		if (trMonster == null) return false;
		if (trMonster.CompareTag(c_strTag) && trMonster.GetComponent<CsMonster>().MonsterId == nMonsterId)
		{
			return true;
		}
		return false;
	}

	//----------------------------------------------------------------------------------------------------
	public static bool IsMonsterWithInstanceId(long lInstanceId, Transform trMonster)
	{
		if (trMonster == null) return false;
		if (trMonster.CompareTag(c_strTag) && trMonster.GetComponent<CsMonster>().InstanceId == lInstanceId)
		{
			return true;
		}
		return false;
	}

	#endregion Setting

	//---------------------------------------------------------------------------------------------------
	int IInteractionObject.GetSpecificId()
	{
		return m_nMonsterId;
	}

	//---------------------------------------------------------------------------------------------------
	long IInteractionObject.GetInstanceId()
	{
		return InstanceId;
	}

	//---------------------------------------------------------------------------------------------------
	long IMonsterObjectInfo.GetInstanceId()
	{
		return InstanceId;
	}

	//---------------------------------------------------------------------------------------------------
	CsMonsterInfo IMonsterObjectInfo.GetMonsterInfo()
	{
		return m_csMonsterInfo;
	}

	//---------------------------------------------------------------------------------------------------
	Transform IMonsterObjectInfo.GetTransform()
	{
		return m_trMyTransform;
	}

	//---------------------------------------------------------------------------------------------------
	int IMonsterObjectInfo.GetMaxHp()
	{
		return m_nMaxHp;
	}

	//---------------------------------------------------------------------------------------------------
	int IMonsterObjectInfo.GetHp()
	{
		return Hp;
	}

	//---------------------------------------------------------------------------------------------------
	int IMonsterObjectInfo.GetMaxMentalStrength()
	{
		return m_nMaxMentalStrength;
	}

	//---------------------------------------------------------------------------------------------------
	int IMonsterObjectInfo.GetMentalStrength()
	{
		return MentalStrength;
	}
}
