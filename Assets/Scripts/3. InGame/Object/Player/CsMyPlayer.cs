using ClientCommon;
using SimpleDebugLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnAutoMode { None = 0, Move, Quest }

public class CsMyPlayer : CsHero
{
	public enum EnPlayerTarget { Normal, Enemy, Auto }
	static int[] s_anAutoSkillIndex = { 3, 1, 2, 4, 0 };

	CsHitSkillCast m_csHitSkillCast = null;
	GameObject m_goSelectMark;
	CsWayPointObject m_csWayPointObject;
	Vector3 m_vtAttackAfterPos;
	Transform m_trKiller = null;

	[SerializeField]
	Transform m_trTarget = null;
	[SerializeField]
	EnPlayerTarget m_enTarget;

	int m_nSamePosMoveCount = 0;
	float m_flAttackRange = 0;
	float m_flAutoMoveStopRange = 0f;
	float m_flAttackAfterStopRange = 0f;

	bool m_bMoveByTouchTarget = false;
	bool m_bQuestDialog = false;

	protected bool m_bOnWater = false;

	protected CsDisplayPlayerPath m_csDisplayPath;

	EnState m_enStateAttackAfter = EnState.Idle;
	EnNationWarPlayerState m_enNationWarPlayerState = EnNationWarPlayerState.None;
	
	CsPlayTheme m_csPlayThemeNone = new CsPlayTheme();
	IAutoPlay m_itrAutoPlay;
	IAutoPlay m_itrAutoBattlePlay;

	public event Delegate<bool> EventArrivalMoveByTouch;
	public event Delegate EventAttackEnd;
	public event Delegate EventStateEndOfIdle;
	public event Delegate EventChangeCartRiding;

	public override bool IsMyHero { get { return true; } }

	public override Guid HeroId { get { return m_csMyHeroInfo.HeroId; } }
	public override CsJob Job { get { return m_csMyHeroInfo.Job; } }
	public override int NationId { get { return m_csMyHeroInfo.Nation.NationId; } }
	public override int Rank { get { return m_csMyHeroInfo.RankNo; } }

	public override string Name { get { return m_csMyHeroInfo.Name; } }
	public override int Level { get { return m_csMyHeroInfo.Level; } set { m_csMyHeroInfo.Level = value; } }
	public override int MaxHp { get { return m_csMyHeroInfo.MaxHp; } set { m_csMyHeroInfo.MaxHp = value; } }
	public override int Hp { get { return m_csMyHeroInfo.Hp; } set { m_csMyHeroInfo.Hp = value; } }
	public override bool IsRidingMount { get { return m_csMyHeroInfo.IsRiding; } set { m_bRidingMount = m_csMyHeroInfo.IsRiding = value; } }
	public override bool Dead { get { return m_bDead; } set { CsIngameData.Instance.MyHeroDead = m_bDead = value; } }
	public override float Height { get { return m_flHeght; } set { m_flHeght = value; CsIngameData.Instance.HeroCenter = (value * 2 / 3); } }
	public Transform Target { get { return m_trTarget; } }
	public Transform TargetEnemy { get { return m_enTarget == EnPlayerTarget.Enemy ? m_trTarget : null; } }
	public bool IsTargetEnemy { get { return (m_enTarget == EnPlayerTarget.Enemy) && (m_trTarget != null); } }
	public bool IsTargetNormal { get { return (m_enTarget == EnPlayerTarget.Normal) && (m_trTarget != null); } }
	public float AutoMoveStopRange { get { return m_flAutoMoveStopRange; } set { m_flAutoMoveStopRange = value; } }
	public bool IsQuestDialog { get { return m_bQuestDialog; } set { m_bQuestDialog = value; } }
	public bool IsAutoPlaying { get { return m_itrAutoPlay != m_csPlayThemeNone; } }
	public IAutoPlay AutoPlay { get { return m_itrAutoPlay; } }
    public EnNationWarPlayerState NationWarPlayerState { get { return m_enNationWarPlayerState; } set { m_enNationWarPlayerState = value; } }
	public CsDisplayPlayerPath DisplayPath { get { return m_csDisplayPath; } set { m_csDisplayPath = value; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(Vector3 vtPos, float flRotationY, Guid guidPlaceInstanceId, bool bFirst)
	{
		m_itrAutoPlay = m_csPlayThemeNone;
		m_itrAutoBattlePlay = m_csPlayThemeNone;

		CsGameEventToIngame.Instance.EventMountGetOn += OnEventMountGetOn;							
		CsGameEventToIngame.Instance.EventMountGetOff += OnEventMountGetOff;						
		CsGameEventToIngame.Instance.EventHeroMountEquipped += OnEventHeroMountEquipped;									// 탈것 장비장착.

		CsGameEventToIngame.Instance.EventMainGearChanged += OnEventMainGearChanged;										// 장비변경.
		CsGameEventToIngame.Instance.EventWingEquip += OnEventWingEquip;													// 날개장착.

		CsGameEventToIngame.Instance.EventStartTutorial += OnStartTutorial;													// UI 튜토리얼시작.
		CsGameEventToIngame.Instance.EventAutoStop += OnEventAutoStop;								
		CsGameEventToIngame.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;	
		
		CsGameEventToIngame.Instance.EventUseSkill += OnEventUseSkill;														// 영웅스킬사용요청.
		CsGameEventToIngame.Instance.EventUseCommonSkill += OnEventUseCommonSkill;											// 공용스킬 사용 요청.
		CsGameEventToIngame.Instance.EventUseRankActiveSkill += OnEventUseRankActiveSkill;									// 계급스킬 사용 요청.
		CsGameEventToIngame.Instance.EventUseTransformationMonsterSkillCast += OnEventUseTransformationMonsterSkillCast;	// 변신몬스터스킬 사용 요청.

		CsGameEventToIngame.Instance.EventIsStateIdle += OnEventIsStateIdle;												// 대기상태확인.
		CsGameEventToIngame.Instance.EventTab += OnEventTab;
		CsGameEventToIngame.Instance.EventHpPotionUse += OnEventHpPotionUse;

		CsGameEventToIngame.Instance.EventReturnScrollUseStart += OnEventReturnScrollUseStart;		
		CsGameEventToIngame.Instance.EventReturnScrollUseCancel += OnEventReturnScrollUseCancel;	
		CsGameEventToIngame.Instance.EventImmediateRevived += OnEventImmediateRevived;

		CsGameEventToIngame.Instance.EventQuestCompltedError += OnEventQuestCompltedError;
		CsGameEventToIngame.Instance.EventGroggyMonsterItemStealStart += OnEventGroggyMonsterItemStealStart;

		CsGameEventToIngame.Instance.EventRequestNpcDialog += OnEventRequestNpcDialog;
		CsGameEventToIngame.Instance.EventDungeonEnter += OnEventDungeonEnter;
		CsGameEventToIngame.Instance.EventCheckQuestAreaInHero += OnEventCheckQuestAreaInHero;

		CsTouchInfo.Instance.EventClickByTouch += OnEventClickByTouch;

		m_guidPlaceInstanceId = guidPlaceInstanceId;
		transform.name = Name;
		transform.position = vtPos;

		m_flRotationY = flRotationY;
		ChangeEulerAngles(flRotationY);

		// 장비 장착.
		m_csHeroCustomData = new CsHeroCustomData(m_csMyHeroInfo);
		m_csEquipment.MidChangeEquipments(m_csHeroCustomData);
		m_csEquipment.CreateArtifact(m_csHeroCustomData, CsArtifactManager.Instance.EquippedArtifactNo);
		m_csEquipment.CreateWing(m_csHeroCustomData, m_trPivotHUD);

		gameObject.layer = LayerMask.NameToLayer("Player"); // 오브젝트 레이어 변경.
		transform.tag = "Player"; // 테그변경.

		// 탈것.
		m_nMountId = m_csMyHeroInfo.EquippedMountId;
		if (m_csMyHeroInfo.GetHeroMount(m_nMountId) != null)
		{
			m_nMountLevel = m_csMyHeroInfo.GetHeroMount(m_nMountId).Level;
		}

		CreateShadow(true);
		NavMeshSetting();
		m_timer.Init(0.2f);

		StartCoroutine(SendMoveState());	// 서버에 플레이어 좌표 정보 전달 시작.
		
		CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();
		if (csNationWarDeclaration != null)
		{
			if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
			{
				if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
				{
					m_enNationWarPlayerState = EnNationWarPlayerState.Offense;
				}
				else if (csNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
				{
					m_enNationWarPlayerState = EnNationWarPlayerState.Defense;
				}
			}
		}

		CreateMyHeroHUD();

		CsIngameData.Instance.IngameManagement.InitPlayThemes();

		if (Hp == 0 || CsIngameData.Instance.MyHeroDead) // 죽어있는 상태.
		{
			DeadSetting(true);
			CsGameEventToUI.Instance.OnEventHeroDead(null); // 죽인대상 UI 전달.
		}
		else
		{
			if (bFirst) // 최초 입장일때.
			{
				if (IsRidingMount)
				{
					SendMountGetOff();
				}

				Invoke(EnterEffectStart, 2f);
				ViewHUD(false);
				CsIngameData.Instance.InGameCamera.FirstEnter = true;
			}
			else if (IsRidingMount)
			{
				ChangeTransformationState(EnTransformationState.Mount);
			}
		}

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsCreatureManager.Instance.ParticipatedCreatureId);
		if (csHeroCreature != null)
		{
			if (csHeroCreature.Creature != null)
			{
				StartCoroutine(CreateCreature(csHeroCreature.Creature));
			}
		}

		m_animator.SetInteger(s_nAnimatorHash_job, 0); // 0 기본, 1 전직1, 2 전직2
		m_animator.SetFloat(s_nAnimatorHash_battle, 0); // 0 비전투, 1 전투	
		CsGameEventToUI.Instance.OnEventMyHeroInfoUpdate();

		m_csDisplayPath = CsDisplayPlayerPath.Create("PlayerPath", transform);
		CsGameEventToUI.Instance.OnEventJoystickReset();
		ResetBattleMode();

		Debug.Log("#####     InitMyHero   bFirst : " + bFirst + " // Name = " + Name + " // HP = " + Hp + " // LocationId = " + m_csMyHeroInfo.LocationId + " //  JobId = " + m_csMyHeroInfo.Job.JobId);		
	}

	#region 01 MonoBehavier

	//---------------------------------------------------------------------------------------------------
	protected override void Awake()
	{
		base.Awake();
		m_csHeroBase = m_csMyHeroInfo;
		m_audioSource.volume = 0.5f;
		m_audioSourceParent.volume = 0.5f;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTargetInDistance(float flDistance)
	{
		if (m_trTarget == null) return false;

		return Vector3.Distance(transform.position, m_trTarget.position) <= flDistance;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		Debug.Log("CsMyPlayer.OnDestroy()  Nation = " + m_csMyHeroInfo.InitEntranceLocationParam + " // Location = " + m_csMyHeroInfo.LocationId);
		m_csSkillStatus.Reset();    // 스킬 초기화.

		if (IsTransformationStateFlight())
		{
			ChangeTransformationState(EnTransformationState.None);
		}

		if (m_itrAutoPlay.IsSavable()) // 설정 유지. // AutoMove일때.
		{
			Debug.Log("CsMyPlayer.OnDestroy()   >>>   AutoMove일때");
			CsIngameData.Instance.AutoPlayKey = m_itrAutoPlay.GetTypeKey();
			CsIngameData.Instance.AutoPlaySub = m_itrAutoPlay.GetTypeSub();
		}
		else  // 설정 초기화.
		{
			if (m_itrAutoPlay.GetType() == EnAutoMode.Move)
			{
				Debug.Log("CsMyPlayer.OnDestroy()    EnAutoMode.Move   >>>   Reset");
				CsIngameData.Instance.AutoPlayKey = 0;
				CsIngameData.Instance.AutoPlaySub = 0;
				SetAutoPlay(null, true);
			}
			else
			{
				if (CsIngameData.Instance.IngameManagement.IsContinent() == false) // 현재 위치가 던전일때.
				{
					SetAutoPlay(null, true, false);
				}
			}
		}

		if (m_enState == EnState.Interaction)
		{
			ChangeState(EnState.Idle);
		}

		CsGameEventToIngame.Instance.EventMountGetOn -= OnEventMountGetOn;
		CsGameEventToIngame.Instance.EventMountGetOff -= OnEventMountGetOff;
		CsGameEventToIngame.Instance.EventHeroMountEquipped -= OnEventHeroMountEquipped;			// 탈것 장비장착.

		CsGameEventToIngame.Instance.EventMainGearChanged -= OnEventMainGearChanged;				// 장비변경.
		CsGameEventToIngame.Instance.EventWingEquip -= OnEventWingEquip;							// 날개장착.

		CsGameEventToIngame.Instance.EventStartTutorial -= OnStartTutorial;							// UI 튜토리얼시작.
		CsGameEventToIngame.Instance.EventAutoStop -= OnEventAutoStop;
		CsGameEventToIngame.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;

		CsGameEventToIngame.Instance.EventUseSkill -= OnEventUseSkill;
		CsGameEventToIngame.Instance.EventUseCommonSkill -= OnEventUseCommonSkill;
		CsGameEventToIngame.Instance.EventUseRankActiveSkill -= OnEventUseRankActiveSkill;
		CsGameEventToIngame.Instance.EventUseTransformationMonsterSkillCast -= OnEventUseTransformationMonsterSkillCast;

		CsGameEventToIngame.Instance.EventIsStateIdle -= OnEventIsStateIdle;						// 대기상태확인.
		CsGameEventToIngame.Instance.EventTab -= OnEventTab;
		CsGameEventToIngame.Instance.EventHpPotionUse -= OnEventHpPotionUse;

		CsGameEventToIngame.Instance.EventReturnScrollUseStart -= OnEventReturnScrollUseStart;
		CsGameEventToIngame.Instance.EventReturnScrollUseCancel -= OnEventReturnScrollUseCancel;
		CsGameEventToIngame.Instance.EventImmediateRevived -= OnEventImmediateRevived;

		CsGameEventToIngame.Instance.EventQuestCompltedError -= OnEventQuestCompltedError;
		CsGameEventToIngame.Instance.EventGroggyMonsterItemStealStart -= OnEventGroggyMonsterItemStealStart;
		CsGameEventToIngame.Instance.EventRequestNpcDialog -= OnEventRequestNpcDialog;
		CsGameEventToIngame.Instance.EventDungeonEnter -= OnEventDungeonEnter;
		CsGameEventToIngame.Instance.EventCheckQuestAreaInHero -= OnEventCheckQuestAreaInHero;

		CsTouchInfo.Instance.EventClickByTouch -= OnEventClickByTouch;

		CsIngameData.Instance.IngameManagement.UninitPlayThemes();
		CsIngameData.Instance.ActiveScene = false;

		if (CsCartManager.Instance.IsMyHeroRidingCart == false) // 탑승중이 아닐때.
		{
			m_csMyHeroInfo.CartInstance = null; // 카트 초기화.
		}
		
		m_csPlayThemeNone = null;
		m_itrAutoPlay = null;
		m_itrAutoBattlePlay = null;
		m_csHitSkillCast = null;

		ResetTarget();
		m_goSelectMark = null;
		m_trTarget = null;
		m_csWayPointObject = null;
		StopCoroutine(SendMoveState());
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (Job == null) return; // 직업 세팅전이나 포탈 입장중.

		if (CsGameData.Instance.JoystickDragging || CsGameData.Instance.JoystickDown)	// 조이스틱 드레그 or 다운 시작.
		{
			if (m_enState != EnState.MoveByJoystic)
			{
				if (IsMovable() && CsIngameData.Instance.Directing == false)
				{
					if (m_csSkillStatus.IsStatusPlay() || m_csSkillStatus.IsStatusPlayAnimWithNoMove()) return;
					if (IsTransformationStateTame() && m_csTameMonster.IsAttack) return;

					if (m_csSkillStatus.IsStatusAnim() && !m_csSkillStatus.IsStatusPlayAnimWithNoMove()) // 이동 가능 스킬시 상태 변화 없이 이동만.
					{
						ChangeHeroMoveSpeed((float)(m_csSkillStatus.JobSkill.CastingMoveValue1 / 100));

						if (UpdateMoveByJoystick(CsGameData.Instance.JoystickAngle))
						{
							ChangeState(EnState.Idle);
						}
					}
					else
					{
						m_nJoyAutoWaitCount = 0;
						ChangeState(EnState.MoveByJoystic);
						UpdateMoveByJoystick(CsGameData.Instance.JoystickAngle);
					}
				}
			}
		}

		if (IsStateKindOfMove(m_enState))
		{
			switch (m_enState)
			{
				case EnState.MoveByJoystic:
					if (UpdateMoveByJoystick(CsGameData.Instance.JoystickAngle))
					{
						ChangeState(EnState.Idle);
					}
					break;
	
				case EnState.MoveByTouch:
					if (CheckHasPath(EnState.MoveByTouch) == false) break;

					if (m_bTargetNpc)	// 터치 대상이 Npc일때.
					{
						m_flMoveStopRange = IsTransformationStateNone() ? m_flNpcStopRange - 1.5f : m_flMoveStopRange;	// 탑승 상태가 아닐때.
					}
					
					if (UpdateMove(m_vtMovePos, m_flMoveStopRange))
					{
						ChangeState(EnState.Idle);

						if (EventArrivalMoveByTouch != null)
						{
							EventArrivalMoveByTouch(m_bMoveByTouchTarget);
						}

						if (m_bQuestDialog == false && m_trTarget != null && m_trTarget.CompareTag("Npc")) // 퀘스트 npc가 아닐때.
						{
							if (m_vtMovePos == m_trTarget.position)
							{
								int nNpcId = CsIngameData.Instance.IngameManagement.GetNpcId(m_trTarget);
								if (CsNationWarManager.Instance.IsNationWarNpc(nNpcId))
								{
									CsNationWarManager.Instance.NationWarNpcDialog();
								}
								else
								{
									CsGameEventToUI.Instance.OnEventArrivalNpcByTouch(nNpcId);
								}

								CsIngameData.Instance.IngameManagement.NpcDialog(nNpcId);
							}
						}
						m_bQuestDialog = false;
					}
					break;

				case EnState.MoveForSkill:
					if (IsTargetEnemy == false)
					{
						ChangeState(EnState.Idle);
						break;
					}

					if (Vector3.Distance(m_vtMovePos, m_trTarget.position) > 0.2f) // 타겟 위치 갱신.
					{
						SetDestination(m_trTarget.position, m_flAttackRange);
					}

					if (UpdateMove(m_trTarget.position, m_flAttackRange))
					{
						if (m_csSkillStatus.IsSkillTypeNone() == false)
						{
							m_csSkillStatus.SetStatusToPlay();

							if (IsTransformationStateNone())
							{
								LookAtPosition(m_trTarget.position);

								if (m_csSkillStatus.IsSkillTypeRank())
								{
									SendRankActiveSkillCast();
									m_csSkillStatus.OnSkillAnimStarted();
									m_csSkillStatus.Reset();
								}
								else
								{
									Attack();
									break;
								}
							}
							else if (IsTransformationStateMonster())
							{
								if (m_csSkillStatus.IsSkillTypeTransformation())
								{
									m_csTransformationMonster.TransformationMonsterSkillCast(m_csSkillStatus.TransformationMonsterSkill);
									m_csSkillStatus.OnSkillAnimStarted();
								}
							}
						}

						ChangeState(EnState.Idle);
					}
					else
					{
						CheckHasPath(EnState.MoveForSkill);
					}
					break;

				case EnState.MoveToPos:
					if (!CheckHasPath(EnState.MoveToPos)) break;

					if (m_bTargetNpc)	// 터치 대상이 Npc일때.
					{
						m_flMoveStopRange = IsTransformationStateNone() ? m_flNpcStopRange - 1.5f : m_flMoveStopRange;	// 탑승 상태가 아닐때는 2.5f.
					}

					if (UpdateMove(m_vtMovePos, m_flMoveStopRange))
					{
						ChangeState(EnState.Idle);
						m_bQuestDialog = false;
						m_itrAutoPlay.ArrivalMoveToPos();
					}
					else
					{
						AutoRidingStart();
					}
					break;
			}
			m_itrAutoPlay.Update(true);			// 자동 퀘스트
			m_itrAutoBattlePlay.Update(true);	// 자동 전투
		}
		else
		{
			m_itrAutoPlay.Update(false);		// 자동 퀘스트
			m_itrAutoBattlePlay.Update(false);	// 자동 전투
		}

		if (m_timer.CheckTimer())
		{
			AutoSelectTarget(20f);		// 11/15 자동 타겟 검색 추가
			OverDistanceResetTarget();	// 20m 거리 멀어지면 초기화.
			m_timer.ResetTimer();
		}

		m_vtPrevPos = transform.position;
	}

	public Vector3 vtTest = new Vector3(0, 0, 0);
	//---------------------------------------------------------------------------------------------------
	protected override void LateUpdate()
	{
		base.LateUpdate();

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//CsIngameData.Instance.IngameManagement.Test(1);

			//m_csEquipment.CreateArtifact(m_csHeroCustomData, 1);
			//transform.rotation = Quaternion.Euler(vtTest);

			//if (m_csCreatureObject == null)
			//{
			//	CsCreature csCreature = CsGameData.Instance.GetCreature(3001);
			//	NetEventCreatureParticipated(csCreature);
			//}
			//else
			//{
			//	NetEventCreatureParticipationCanceled();
			//}
		}
#endif
	}

	//---------------------------------------------------------------------------------------------------
	#endregion 01 MonoBehavier

	bool IsMovable() { return !Dead && !m_bStun && !m_bBlockade; }
	bool IsAttackable() { return !Dead && !m_bStun && !CsIngameData.Instance.Directing && CsIngameData.Instance.IngameManagement.IsDungeonStart(); }

	//---------------------------------------------------------------------------------------------------
	public void SetAutoPlay(IAutoPlay itr, bool bNotify, bool bIdle = true)
	{
		Debug.Log("CsMyPlay.SetAutoPlay()  itr = " + itr + " // bNotify = " + bNotify + " bIdle = " + bIdle+" , "+ CsIngameData.Instance.AutoBattleMode);

		m_bTargetNpc = false;

		if (bIdle && !Dead)
		{
			if (m_enState != EnState.Interaction && m_enState != EnState.ReturnScroll)
			{
				ChangeState(EnState.Idle);
			}
		}

		m_itrAutoPlay.Stop(bNotify);

		if (itr == null)
		{
			m_csDisplayPath.ResetPath();
			CsGameData.Instance.PathCornerByAutoMove = null;
			SetWayPoint(Vector3.zero, EnWayPointType.None, 1);
			m_itrAutoPlay = m_csPlayThemeNone;
		}
		else
		{
			m_itrAutoPlay = itr;
			itr.Start();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SetAutoBattlePlay(IAutoPlay itr)
	{
		Debug.Log("SetAutoBattlePlay    itr = " + itr);
		m_itrAutoBattlePlay = itr;

		if (itr != null)
		{
			m_itrAutoBattlePlay.Start();
		}		
	}

	//---------------------------------------------------------------------------------------------------
	public void SetWayPoint(Vector3 vtPos, EnWayPointType enNewWayPointType, float flRaius)
	{
		if (m_csWayPointObject == null)
		{
			GameObject goWayPoint = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/WayPoint"));
			goWayPoint.name = "WayPoint";
			m_csWayPointObject = goWayPoint.GetComponent<CsWayPointObject>();
		}

		if (m_csWayPointObject != null)
		{
			m_csWayPointObject.SetWayPoint(enNewWayPointType, vtPos, flRaius);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override bool ChangeState(EnState enNewState)
	{
		if (m_csSkillStatus != null && m_csSkillStatus.IsStatusPlayAnim())
		{
			if (enNewState != EnState.Attack)
			{
				if (m_csSkillStatus.IsStatusPlay()) return false;

				if (enNewState == EnState.MoveByTouch)
				{
					m_enStateAttackAfter = enNewState;
				}
				else
				{
					m_enStateAttackAfter = EnState.Idle;
				}

				if (m_csSkillStatus.Chained) // 연속기 입력 상태에서 이동관련 입력.
				{
					SetAnimStatus(EnAnimStatus.Idle);
				}
				m_csSkillStatus.ResetNextSkill();
				return false;
			}
		}

		if (CsIngameData.Instance.InGameCamera != null && CsIngameData.Instance.InGameCamera.FirstEnter)
		{
			CsIngameData.Instance.InGameCamera.FirstEnter = false;
			ViewHUD(true);
		}

		if (m_enState != enNewState)
		{
			switch (m_enState)
			{
			case EnState.Idle:
				OnStateEndOfIdle(enNewState);
				break;
			case EnState.MoveToPos:
			case EnState.MoveByTouch:
			case EnState.MoveByJoystic:
				ResetPathOfNavMeshAgent();
				break;
			case EnState.MoveForSkill:
				ResetPathOfNavMeshAgent();
				if (enNewState != EnState.Attack)
				{
					StopSkill();
				}
				break;
			case EnState.ReturnScroll:
				OnStateEndOfReturnScroll();
				break;
			case EnState.Fishing:
				OnStateEndOfFishing();
				break;
			case EnState.Interaction:
				OnStateEndOfInteraction();
				break;
			case EnState.ItemSteal:
				OnStateEndOfItemSteal();
				break;
			}

			if (enNewState == EnState.MoveByJoystic || enNewState == EnState.MoveByTouch ||
				enNewState == EnState.Interaction || enNewState == EnState.ReturnScroll)
			{
				ResetAccelerate();
				ResetBattleMode();
			}
			else if (enNewState == EnState.Attack)
			{
				if (m_bDistortion)
				{
					SendDistortionCancel();
				}
			}
			else if (enNewState == EnState.Dead)
			{
				MyHeroView(true);
			}
			else if (enNewState == EnState.Idle)
			{
				ResetAccelerate();
			}

			if (IsStateKindOfMove(enNewState) && enNewState != EnState.MoveByJoystic)
			{
				m_flJoysticWalk = 1f;
				m_animator.SetFloat(s_nAnimatorHash_move, m_flJoysticWalk);
			}

			EnState enOld = m_enState;

			bool bRet = base.ChangeState(enNewState);

			if (enNewState == EnState.Idle && IsAutoPlaying)
			{
				m_itrAutoPlay.StateChangedToIdle();
			}

			return bRet;
		}

		return base.ChangeState(enNewState);
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetBattleMode()
	{
		if (CsIngameData.Instance.AutoBattleMode != EnBattleMode.None)
		{
			CsIngameData.Instance.AutoBattleMode = EnBattleMode.None;
			CsGameEventToUI.Instance.OnEventChangeAutoBattleMode(CsIngameData.Instance.AutoBattleMode);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ResetAccelerate()
	{
		if (m_bAcceleration)
		{
			if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.Story)
			{
				CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);
			}
			else
			{
				CsIngameData.Instance.InGameCamera.ChangeDashState(EnDashState.DashEnd);
				m_nDashMotionCount = 0;
				m_flDashTime = 0;
			}
		}

		m_bAcceleration = false;
		m_flTrapMoveSpeed = 0f;
		m_flAccelerationValue = 0f;
		m_animator.SetFloat(s_nAnimatorHash_acceleration, m_flAccelerationValue);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	void OnStateEndOfIdle(EnState enNewState)
	{
		if (EventStateEndOfIdle != null)
		{
			EventStateEndOfIdle();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnChangeCartRiding()
	{
		if (EventChangeCartRiding != null)
		{
			EventChangeCartRiding();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnStateEndOfReturnScroll()
	{
		if (CsIngameData.Instance.ReturnScroll)
		{
			Debug.Log("OnStateEndOfReturnScroll         CsGameEventToUI.Instance.OnEventReturnScrollUseCancel()");
			CsIngameData.Instance.ReturnScroll = false;
			CsGameEventToUI.Instance.OnEventReturnScrollUseCancel(); // 귀환주문서 사용 취소.
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnStateEndOfFishing()
	{
		m_csEquipment.RemoveFishStaff();
		if (CsFishingQuestManager.Instance.Fishing)
		{
			Debug.Log("OnStateEndOfFishing            CsFishingQuestManager.Instance.FishingCancel()");
			CsFishingQuestManager.Instance.FishingCancel();
		}
	}

	bool m_bGroggyMonsterItemSteal = false;
	//---------------------------------------------------------------------------------------------------
	void OnStateEndOfItemSteal()
	{
		if (m_bGroggyMonsterItemSteal)
		{
			Debug.Log("OnStateEndOfItemSteal    CsGameEventToUI.Instance.OnEventGroggyMonsterItemStealCancel()");
			m_bGroggyMonsterItemSteal = false;
			CsGameEventToUI.Instance.OnEventGroggyMonsterItemStealCancel(); // 정신제압 취소.
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnStateEndOfInteraction()
	{
		CsIngameData.Instance.IngameManagement.StateEndOfInteraction();
	}

	#region 02 NetEvent.

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator SendMoveState() 	// 클라이언트 이벤트 - Move // 플레이어 이동.
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);

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
					yield return new WaitForSeconds(0.04f);
					bFirstOk = MyHeroMove();
				}
			}

			if (m_enTransformationState == EnTransformationState.None) // 카트 탑승중이 아닐때.
			{
				if (m_enState != EnState.Interaction && ForceSyncMoveDataWithServer())
				{
					yield return new WaitForSeconds(0.04f);
				}
			}
			
			yield return null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool ForceSyncMoveDataWithServer(bool bStrict = false)
	{
		if (bStrict || (!IsTargetInDistance(m_vtLastSendPos, 0.15f) || Mathf.Abs(transform.eulerAngles.y - m_flLastSendAngle) > 3.0f))
		{
			MyHeroMoveStart(false);
			MyHeroMoveEnd();
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void SendMoveModeChanged()
	{
		//Debug.Log("CsMyPlayer.SendMoveModeChanged()  : "+ CsGameData.Instance.JoystickDragging +" , "+ CsIngameData.Instance.JoysticWalk);
		CEBMoveModeChangedEventBody csEvt = new CEBMoveModeChangedEventBody();
		csEvt.isManualMoving = m_enState == EnState.MoveByJoystic ? true : false;
		csEvt.isWalking = m_bWalk = CsGameData.Instance.JoysticWalk;
		CsRplzSession.Instance.Send(ClientEventName.MoveModeChanged, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendMountGetOff()
	{
		Debug.Log("CsMyPlayer.SendMountGetOff()");
		CsGameEventToUI.Instance.OnEventMountGetOff();
		CEBMountGetOffEventBody csEvt = new CEBMountGetOffEventBody();
		CsRplzSession.Instance.Send(ClientEventName.MountGetOff, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendSkillCastEvent(Vector3 vtSkillMovePos)
	{
		if (m_csSkillStatus.JobSkill == null) return;

		if (m_csSkillStatus.JobSkill.TypeEnum == EnJobSkillType.AreaOfEffect)	// 장판스킬인 경우.
		{
			float flDistance = GetDistanceFormTarget(m_vtTargetPos);
			if (flDistance > m_csSkillStatus.JobSkill.CastRange)	// 장판 케스트 거리보다 큰경우.
			{
				m_vtTargetPos = transform.position + (transform.forward * m_csSkillStatus.JobSkill.CastRange);
			}
		}

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
			if (IsTargetEnemy && (m_trTarget.CompareTag("EnemyHero") || m_trTarget.CompareTag("EnemyCart")))
			{
				csEvt.targetHeroId = CsIngameData.Instance.IngameManagement.GetHeroId(m_trTarget);
			}
		}

		//Debug.Log("1. ##############           SendSkillCastEvent       " + csEvt.skillId + " , " + csEvt.chainSkillId);
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

		//Debug.Log("2. ##############           SendSkillHitEvent       " + csEvt.skillId + " , " + csEvt.chainSkillId + " , " + nHitId);
		CsRplzSession.Instance.Send(ClientEventName.SkillHit, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendJobCommonSkillCast(CsJobCommonSkill csJobCommonSkill)
	{
		if (csJobCommonSkill == null)
		{
			csJobCommonSkill = m_csSkillStatus.JobCommonSkill;
		}

		
		if (csJobCommonSkill != null)
		{
			Debug.Log("CsMyPlayer.SendJobCommonSkillCast()      " + csJobCommonSkill.SkillId);
			CEBJobCommonSkillCastEventBody csEvt = new CEBJobCommonSkillCastEventBody();
			csEvt.placeInstanceId = m_guidPlaceInstanceId;
			
			csEvt.skillId = csJobCommonSkill.SkillId;
			if (m_trTarget != null)
			{
				csEvt.skillTargetPosition = CsRplzSession.Translate(m_trTarget.position);
			}
			else
			{
				csEvt.skillTargetPosition = CsRplzSession.Translate(transform.position);
			}
			CsRplzSession.Instance.Send(ClientEventName.JobCommonSkillCast, csEvt);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendRankActiveSkillCast()
	{
		Debug.Log("CsMyPlayer.SendRankActiveSkillCast()  RankActiveSkill = " + m_csSkillStatus.RankActiveSkill.SkillId);
		CEBRankActiveSkillCastEventBody csEvt = new CEBRankActiveSkillCastEventBody();
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		csEvt.skillId = m_csSkillStatus.RankActiveSkill.SkillId;

		if (m_csSkillStatus.RankActiveSkill.Type == 1 && m_trTarget != null)
		{
			if (m_trTarget.CompareTag(CsMonster.c_strTag))
			{
				PDMonsterHitTarget MonsterHitTarget = new PDMonsterHitTarget();
				MonsterHitTarget.monsterInstanceId = CsIngameData.Instance.IngameManagement.GetInstanceId(m_trTarget);
				csEvt.target = MonsterHitTarget;
			}
			else if (m_trTarget.CompareTag("EnemyHero"))
			{
				PDHeroHitTarget HeroHitTarget = new PDHeroHitTarget();
				HeroHitTarget.heroId = CsIngameData.Instance.IngameManagement.GetHeroId(m_trTarget);
				csEvt.target = HeroHitTarget;
			}
			else if (m_trTarget.CompareTag("EnemyCart"))
			{
				PDCartHitTarget CartHitTarget = new PDCartHitTarget();
				CartHitTarget.cartInstanceId = CsIngameData.Instance.IngameManagement.GetInstanceId(m_trTarget);
				csEvt.target = CartHitTarget;
			}

			csEvt.skillTargetPosition = CsRplzSession.Translate(m_trTarget.position);
		}
		else
		{
			csEvt.skillTargetPosition = CsRplzSession.Translate(transform.position);
		}

		CsRplzSession.Instance.Send(ClientEventName.RankActiveSkillCast, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendJobCommonSkillHit(int nHit = 1)
	{
		if (m_csSkillStatus.IsSkillTypeCommon())
		{
			Debug.Log("CsMyPlayer.SendJobCommonSkillHit()");
			CEBJobCommonSkillHitEventBody csEvt = new CEBJobCommonSkillHitEventBody();
			csEvt.placeInstanceId = m_guidPlaceInstanceId;
			csEvt.skillId = m_csSkillStatus.JobCommonSkill.SkillId;
			csEvt.hitId = nHit;
			csEvt.targetMonsterInstanceId = CsIngameData.Instance.IngameManagement.GetInstanceId(m_trTarget);
			CsRplzSession.Instance.Send(ClientEventName.JobCommonSkillHit, csEvt);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendGroggyMonsterItemStealStart()
	{
		Debug.Log("CsMyPlayer.SendGroggyMonsterItemStealStart()");
		GroggyMonsterItemStealStartCommandBody csEvt = new GroggyMonsterItemStealStartCommandBody();
		csEvt.targetMonsterInstanceId = CsIngameData.Instance.TameMonster.InstanceId;
		CsRplzSession.Instance.Send(ClientCommandName.GroggyMonsterItemStealStart, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	void SendDistortionCancel()
	{
		Debug.Log("CsMyPlayer.SendDistortionCancel()");
		DistortionCancelCommandBody csEvt = new DistortionCancelCommandBody();
		CsRplzSession.Instance.Send(ClientCommandName.DistortionCancel, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHitApprove(PDHitResult HitResult, Transform trTarget) // 공격할 대상에대한 정보 저장.
	{
		//Debug.Log("1. Start          NetEventHitApprove        trTarget = " + trTarget);
		if (m_csHitSkillCast == null || m_csSkillStatus.JobSkill == null || m_csSkillStatus.JobSkill.TypeEnum == EnJobSkillType.AreaOfEffect || m_csSkillStatus.JobSkill.SkillId != HitResult.skillId)
		{
			CsIngameData.Instance.IngameManagement.AttackByHit(trTarget, HitResult, Time.time, null, true);
			AttackHitEffect(trTarget);
			return;
		}

		m_csHitSkillCast.HitInfoNodeList.Add(new CsHitInfoNode(trTarget, HitResult,Time.time));
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAbnormalStateEffectHit(int nHp, long[] alRemovedAbnormalStateEffects, long lAbnormalStateEffectInstanceId, int nDamage, int nHpDamage, Transform trAttacker)
	{
		// lAbnormalStateEffectInstanceId 데이터 필요없어보임 처리내용 없음.
		if (!HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects))
		{
			m_trKiller = trAttacker;
			MyHeroDead();
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventPortalEnter(Vector3 vtPos, float flRotationY)
	{
		Debug.Log("NetEventRuinsReclaimPortalEnter() vtPos = " + vtPos);
		StartCoroutine(HeroMoveDirecting(vtPos, flRotationY));		
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventRevive(float flRevivalInvincibilityRemainingTime = 0) // 플레이어 부활.
	{
		Debug.Log("NetEventRevive     nHp = " + Hp);
		DeadSetting(false);
		m_flHpChangeTime = Time.time;

		if (flRevivalInvincibilityRemainingTime != 0)	// 부활 무적 남은 시간.
		{
			AbnormalSet(9, 0, 0, 0, 0, flRevivalInvincibilityRemainingTime, true);
			//CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.User, transform, transform.position, "Abnormal_9", flRevivalInvincibilityRemainingTime);
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventRevive(Vector3 vtPos, float flRotationY)
	{
		Debug.Log("NetEventRuinsReclaimRevive() vtPos = " + vtPos);
		StartCoroutine(HeroMoveDirecting(vtPos, flRotationY, true));
		DeadSetting(false);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator HeroMoveDirecting(Vector3 vtPos, float flRotationY, bool bRevive = false)
	{
		CsGameEventToUI.Instance.OnEventFade(true);
		CsGameEventToUI.Instance.OnEventJoystickReset();
		CsIngameData.Instance.Directing = true;
		m_navMeshAgent.enabled = false;
		yield return new WaitForSeconds(1f);

		transform.position = vtPos;
		ChangeEulerAngles(flRotationY);
		yield return new WaitForSeconds(0.2f);

		m_navMeshAgent.enabled = true;
		CsGameEventToUI.Instance.OnEventFade(false);
		CsIngameData.Instance.Directing = false;

		if (bRevive)
		{
			CsIngameData.Instance.InGameCamera.ChangeTexture(false);
		}

		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroMountGetOff()
	{
		Debug.Log("MyHero.NetEventHeroMountGetOff()");
		ChangeTransformationState(EnTransformationState.None);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventFishingStart(Transform trTarget = null)
	{
		Debug.Log("NetEventFishingStart()        trTarget = " + trTarget);
		ResetBattleMode();

		if (m_enState != EnState.Fishing)
		{
			ChangeTransformationState(EnTransformationState.None, true);
			ChangeState(EnState.Fishing);

			if (trTarget != null)
			{
				transform.LookAt(trTarget);
				MyHeroMoveStart(false);
				MyHeroMoveEnd();
			}

			CsFishingQuestManager.Instance.SendFishingStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventFishingStop()
	{
		Debug.Log("NetEventFishingStop()");
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroDead(Transform trKiller, PDHitResult pDHitResult)
	{
		Debug.Log("MyPlayer.NetEventHeroHit()");
		m_trKiller = trKiller;
		Invoke(MyHeroDead, 4f);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetPortalEnter(int nPortarlId)
	{
		Debug.Log("MyPlayer.NetPortalEnter()   nPortarlId = "+ nPortarlId);
		ResetBattleMode();
		m_itrAutoPlay.EnterPortal(nPortarlId);
		m_nAutoRidingWaitCount = 0;
		MyHeroMoveEnd();
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public override void NetEventBattleModeEnd() // 비전투상태.
	{
		base.NetEventBattleModeEnd();
		if (IsAutoPlaying && m_enState == EnState.Idle) // 03/08 전투상태 해지시 오토 연결.
		{
			m_itrAutoPlay.StateChangedToIdle();
		}		
	}

	//---------------------------------------------------------------------------------------------------
    public void NetMyGuildHUDUpdate()
    {
		Debug.Log("NetMyGuildHUDUpdate");
        if (CsCartManager.Instance.IsMyHeroRidingCart == true)
        {
            m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateCartHUD(CsGameData.Instance.MyHeroInfo.CartInstance.instanceId);
        }
        else
        {
			CreateMyHeroHUD();
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void NetEventNationWarChangeState() // 국가전 자신의 HUD변경
    {
        Debug.Log("MyPlayer.NetEventNationWarChangeState ");
		CreateMyHeroHUD();
    }

	//---------------------------------------------------------------------------------------------------
	public void NetEventDisplayTitleChange()
	{
		Debug.Log("MyPlayer.NetEventDisplayTitleSet()");
		CreateMyHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetTrapHit(int nHp, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects, bool bSlowMoveSpeed, int nTrapPenaltyMoveSpeed, PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{
		Debug.Log("MyPlayer.NetTrapHit()             nHp = " + nHp + " // bSlowMoveSpeed = " + bSlowMoveSpeed);
		ChangedAbnormalStateEffectDamageAbsorbShields(apDAbnormalStateEffectDamageAbsorbShield);
		if (!HeroTrapHit(nHp, nDamage, nHpDamage, alRemovedAbnormalStateEffects, bSlowMoveSpeed, nTrapPenaltyMoveSpeed))
		{
			MyHeroDead();
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetTrapEffectFinished()
	{
		Debug.Log("##########           NetTrapEffectFinished                    m_flOffestMoveSpeed  " + m_flTrapMoveSpeed);
		m_flTrapMoveSpeed = 0f;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetBuffBoxAcquire(int nBuffBoxId)
	{
		Debug.Log("NetBuffBoxAcquire    nBuffBoxId = " + nBuffBoxId);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetBuffFinish()
	{
		// 버프별 리소스 해제
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTameGetOn() // 몬스터 탑승하기
	{
		Debug.Log("NetEventDominateGetOn    m_csSkillStatus.IsStatusPlayAnim() = " + m_csSkillStatus.IsStatusPlayAnim());
		ResetBattleMode();

		if (IsTransformationStateTame() == false)
		{
			ChangeTransformationState(EnTransformationState.Tame);
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTameGetOff() // 몬스터 탑승 해제.
	{
		Debug.Log("OnEventHeroDominateMonsterOff");
		if (IsTransformationStateTame())
		{
			CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Domination, false);
			ChangeTransformationState(EnTransformationState.None);
			SetAutoPlay(null, true);
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	protected override void TameGetOff()
	{
		base.TameGetOff();
		MyHeroView(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventAccelerationStarted() // 대륙 가속 시작.
	{
		m_bAcceleration = true;

		if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.Story)
		{
			if (m_enState == EnState.MoveByJoystic)
			{
				CsIngameData.Instance.InGameCamera.PostProcessingStart(true, true, 0.5f);
				CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Acceleartion);
			}
		}
		else
		{
			if (IsTransformationStateNone())
			{
				if (CsGameData.Instance.JoysticWalk == false)
				{
					CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.User, transform, transform.position, "Gaia_Skill_01_02", 0.5f);
					CsIngameData.Instance.InGameCamera.PostProcessingStart(false, true);
				}

				CsIngameData.Instance.InGameCamera.ChangeDashState(EnDashState.Dash);
				m_animator.ResetTrigger(s_nAnimatorHash_dashmotion);
				m_flAccelerationValue = 0f;
				m_flDashTime = 0.0f;
			}
		}

		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetGroggyMonsterItemStealStart(bool bSuccess)
	{
		Debug.Log("NetGroggyMonsterItemStealStart    bSuccess = "+ bSuccess);
		ResetBattleMode();
		if (bSuccess)
		{
			m_bGroggyMonsterItemSteal = true;
			if (m_enState != EnState.ItemSteal)
			{
				ChangeState(EnState.ItemSteal);
			}
		}
		else
		{
			if (m_enState != EnState.Dead)
			{
				ChangeState(EnState.Idle);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetGroggyMonsterItemStealFinished()
	{
		Debug.Log("NetGroggyMonsterItemStealFinished");
		m_bGroggyMonsterItemSteal = false;
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDistortionStart()
	{
		Debug.Log("NetEventHeroDistortionStart        m_bDistortion = " + m_bDistortion);
		m_bDistortion = true;
		CreateMyHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventDistortionFinish()
	{
		Debug.Log("NetEventDistortionFinish          m_bDistortion  = " + m_bDistortion);
		m_bDistortion = false;
		CreateMyHeroHUD();
	}

	//---------------------------------------------------------------------------------------------------
	public override void NetEventChangedMaxHp(int nMaxHp, int nHp)
	{
		base.NetEventChangedMaxHp(nMaxHp, nHp);
		m_flHpChangeTime = Time.time;
	}

	//---------------------------------------------------------------------------------------------------
	public override void NetEventChangeHp(int nHp)
	{
		base.NetEventChangeHp(nHp);
		m_flHpChangeTime = Time.time;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSelectMonster(Transform trMonster)
	{
		Debug.Log("NetEventSelectMonster");
		SelectTargetEnemy(trMonster, true);

		if (m_csSkillStatus.IsStatusPlayAnim())
		{
			Vector3 dir = m_trTarget.position - transform.position;
			dir.y = 0f;
			dir.Normalize();
			CsIngameData.Instance.InGameCamera.ChangeCamera(false, true, false, 0, Quaternion.LookRotation(dir).eulerAngles.y * Mathf.Deg2Rad, 0, 0.5f);
		}
		else
		{
			transform.LookAt(trMonster.position);
			CsIngameData.Instance.InGameCamera.ChangeCamera(false, true, false, 0, CsGameData.Instance.MyHeroTransform.rotation.eulerAngles.y * Mathf.Deg2Rad, 0, 0.5f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSafeModeStarted()
	{
		Debug.Log("NetEventSafeModeStarted");
		m_bSafeMode = true;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventSafeModeEnded()
	{
		Debug.Log("NetEventSafeModeEnded");
		m_bSafeMode = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCostumeEquip(int nCostumeId, int nCostumeEffectId)
	{
		m_csHeroCustomData.SetCostum(nCostumeId, nCostumeEffectId);
		m_csEquipment.MidChangeEquipments(m_csHeroCustomData);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCostumeEffectApply(int nCostumeEffectId)
	{
		m_csHeroCustomData.SetCostum(m_csHeroCustomData.CostumeId, nCostumeEffectId);
		m_csEquipment.CreateCostumeEffect(CsGameData.Instance.CostumeEffectList.Find(a => a.CostumeEffectId == nCostumeEffectId));
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCostumeUnequip()
	{
		m_csHeroCustomData.SetCostum(0, 0);
		m_csEquipment.MidChangeEquipments(m_csHeroCustomData);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventArtifactEquip()
	{
		m_csEquipment.CreateArtifact(m_csHeroCustomData, CsArtifactManager.Instance.EquippedArtifactNo);
	}

	#region  ContinentObjectInteraction

	Coroutine m_corPlayInteraction;
	CsInteractionObject m_csInteractionObject = null;
	//---------------------------------------------------------------------------------------------------
	public void ContinentObjectInteractionStart()
	{
		if (m_corPlayInteraction == null)
		{
			m_corPlayInteraction = StartCoroutine(PlayInteraction());
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ContinentObjectInteractionCancel()
	{
		ResetContinentObjectInteraction();
	}

	//---------------------------------------------------------------------------------------------------
	public void ContinentObjectInteractionFinished()
	{
		ResetContinentObjectInteraction();
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator PlayInteraction()
	{
		dd.d("PlayInteraction");
		ResetTarget();
		m_csInteractionObject = null;
		if (IsTransformationStateNone() == false && IsTransformationStateMonster() == false)
		{
			ChangeTransformationState(EnTransformationState.None, true);
		}
		yield return new WaitForSeconds(0.2f);
        dd.d("PlayInteraction", m_enState == EnState.Idle);
        if (m_enState == EnState.Idle)
		{
			Transform trTarget = FindTarget(transform.position, transform.position, "Object", "InteractionObject", 10);

			if (trTarget != null)
			{
				SelectTarget(trTarget, true);
				m_csInteractionObject = trTarget.GetComponent<CsInteractionObject>();

				if (IsTargetInDistance(m_csInteractionObject.ContinentObject.InteractionMaxRange))
				{
					LookAtPosition(m_csInteractionObject.transform.position);
					ChangeState(CsHero.EnState.Interaction);
					ForceSyncMoveDataWithServer();
					CsIngameData.Instance.IngameManagement.Interaction(m_csInteractionObject.InstanceId, m_csInteractionObject.QuestType, m_csInteractionObject.ContinentObject);
				}
			}
			else
			{
				ChangeState(CsHero.EnState.Idle);
			}
		}

		m_corPlayInteraction = null;
	}

	//---------------------------------------------------------------------------------------------------
	void ResetContinentObjectInteraction()
	{
		if (m_corPlayInteraction != null)
		{
			StopCoroutine(m_corPlayInteraction);
			m_corPlayInteraction = null;
		}
		ResetTarget();
		m_csInteractionObject = null;
		ChangeState(EnState.Idle);
	}

	#endregion  ContinentObjectInteraction

	#endregion 02 NetEvent.

	#region  03.Event.

	//---------------------------------------------------------------------------------------------------
	bool OnEventCheckQuestAreaInHero()
	{
		if (m_csWayPointObject != null)
		{
			if (m_csWayPointObject.WayPointType != EnWayPointType.None)
			{
				if (IsTargetInDistance(m_csWayPointObject.WayPointPos, m_csWayPointObject.Radius))
				{
					Debug.Log("OnEventCheckQuestAreaInHero()    >>>  true");
					return true;
				}
			}
		}
		Debug.Log("OnEventCheckQuestAreaInHero()    >>>  false");
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnEventMyHeroLevelUp() // 레벨업.
	{
		m_flHpChangeTime = Time.time;		
		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.User, transform, transform.position, "LevelUP", 2.5f);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventQuestCompltedError()
	{
		ResetBattleMode();
		SetAutoPlay(null, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGroggyMonsterItemStealStart()
	{
		ResetBattleMode();
		if (m_csSkillStatus.IsStatusNo())
		{
			LookAtPosition(CsIngameData.Instance.TameMonster.transform.position);
			ChangeState(EnState.ItemSteal);
			SendGroggyMonsterItemStealStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventClickByTouch(Vector3 vtTouchPos, GameObject goTouchObject, bool bDoubleClick)
	{
		if (IsTransformationStateTame() && m_csTameMonster.IsAttack) return;	// Tame공격중.

		if (IsAutoPlaying && bDoubleClick) // Auto 중 더블클릭시.
		{
			SetAutoPlay(null, true);
			//return;
		}

		m_bTargetNpc = false;
		if (goTouchObject == this.gameObject)
		{
			ResetTarget();
			ChangeState(EnState.Idle);
		}
		else if (goTouchObject.CompareTag("Hero"))
		{
			if (m_trTarget != goTouchObject.transform)
			{
				SelectTarget(goTouchObject.transform, true);
			}
		}
		else if (goTouchObject.CompareTag("EnemyHero") || goTouchObject.CompareTag("EnemyCart") || goTouchObject.CompareTag(CsMonster.c_strTag))
		{
			SelectTarget(goTouchObject.transform, true, EnPlayerTarget.Enemy);
		}
		else if (goTouchObject.CompareTag("Cart"))
		{
			if (IsTransformationStateCart() == false || m_csCartObject == null || m_csCartObject.gameObject != goTouchObject)
			{
				SelectTarget(goTouchObject.transform, true);
				MoveByTouch(goTouchObject.transform.position, 1.5f, true);
			}
		}
		else if (goTouchObject.CompareTag("InteractionObject"))
		{
			SelectTarget(goTouchObject.transform, true);
			MoveByTouch(goTouchObject.transform.position, 1.5f, true);
		}
		else if (goTouchObject.CompareTag("Npc"))
		{
			CsNpcInfo csNpcInfo = CsIngameData.Instance.IngameManagement.GetNpcInfo(goTouchObject.transform);
			if (csNpcInfo != null)
			{
				m_flNpcStopRange = csNpcInfo.InteractionMaxRange;
			}
			else
			{
				m_flNpcStopRange = 4f;  // 길드Npc 및 대륙 Npc가 아닌경우.
			}

			SelectTarget(goTouchObject.transform, true);
			m_bTargetNpc = true;
			MoveByTouch(goTouchObject.transform.position, m_flNpcStopRange, true);
		}
		else
		{
			if (CsGameData.Instance.JoystickDragging) return;
			if (!IsMovable()) return;

			// 터치 이동 보류   2018.05.29
			//if (goTouchObject.CompareTag("NavMesh"))
			//{
			//    if (vtTouchPos == Vector3.zero) return;

			//    NavMeshHit navMeshHit;
			//    NavMesh.Raycast(vtTouchPos, vtTouchPos, out navMeshHit, NavMesh.AllAreas);

			//    if (Vector3.Distance(navMeshHit.position, vtTouchPos) > 0.5f) return; // 체크 위치가 네비위 실제 이동 가능 위치보다 5m 차이가 나면 리턴.

			//    if (!IsTargetInDistance(navMeshHit.position, c_flStopRange))
			//    {
			//        if (m_csSkillStatus.IsStatusPlayAnim() && !m_csSkillStatus.IsStatusAnimWithNoMove()) // 이동 가능 스킬중 터치 입력 >> 이동 상태 변화 없이 이동 동작만.
			//        {
			//            m_navMeshAgent.speed = (float)(m_csSkillStatus.JobSkill.CastingMoveValue1 / 100); // 이동조작 가능 스킬 이동 속도 제어.
			//            m_bMoveHasPath = true;
			//            SetDestination(navMeshHit.position, c_flStopRange);
			//        }
			//        else
			//        {
			//            MoveByTouch(navMeshHit.position, c_flStopRange);
			//        }

			//        CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, navMeshHit.position, "ClickPoint", 0.65f);
			//    }
			//}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRequestNpcDialog()
	{
		Transform trNpc = FindNpc(transform.position);
		if (trNpc != null)
		{
			SelectTarget(trNpc, true);
			m_bTargetNpc = true;
			ChangeState(EnState.Idle);

			if (EventArrivalMoveByTouch != null)
			{
				EventArrivalMoveByTouch(true);
			}

			if (m_bQuestDialog == false && m_trTarget != null && m_trTarget.CompareTag("Npc")) // 퀘스트 npc가 아닐때.
			{
				int nNpcId = CsIngameData.Instance.IngameManagement.GetNpcId(m_trTarget);
				if (CsNationWarManager.Instance.IsNationWarNpc(nNpcId))
				{
					Debug.Log("MoveByTouch   >>>   NationWarNpcDialog()    nNpcId = " + nNpcId);
					CsNationWarManager.Instance.NationWarNpcDialog();
				}
				else
				{
					Debug.Log("MoveByTouch   >>>   OnEventArrivalNpcByTouch     nNpcId = " + nNpcId);
					CsGameEventToUI.Instance.OnEventArrivalNpcByTouch(nNpcId);
				}

				CsIngameData.Instance.IngameManagement.NpcDialog(nNpcId);
			}
			LookAtPosition(trNpc.position);
			m_bQuestDialog = false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonEnter()
	{
		Debug.Log("$$$$$$$$$$ OnEventDungeonEnter $$$$$$$$$$");
		ResetBattleMode();
		SetAutoPlay(null, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainGearChanged(EnMainGearCategory enMainGearCategory, CsHeroMainGear csHeroMainGear) // MainGear 교체 및 해제.
	{
		if (enMainGearCategory == EnMainGearCategory.Weapon)
		{
			if (csHeroMainGear != null)
			{
				m_csHeroCustomData.SetWeapon(csHeroMainGear.Id, csHeroMainGear.MainGear.MainGearId, csHeroMainGear.EnchantLevel);
			}
			else
			{
				m_csHeroCustomData.SetWeapon(Guid.Empty, 0, 0);
			}
		}
		else if (enMainGearCategory == EnMainGearCategory.Armor)
		{
			if (csHeroMainGear != null)
			{
				m_csHeroCustomData.SetArmor(csHeroMainGear.Id, csHeroMainGear.MainGear.MainGearId, csHeroMainGear.EnchantLevel);
			}
			else
			{
				m_csHeroCustomData.SetArmor(Guid.Empty, 0, 0);
			}
		}

		if (IsTransformationStateMount())
		{
			m_trPivotHUD.SetParent(transform); // 윈래 위치로 본 이동.
			m_csMountObject.transform.SetParent(transform.parent);
			m_csEquipment.MidChangeEquipments(m_csHeroCustomData);
			m_csMountObject.transform.SetParent(transform);
			Transform trRidePosition = m_csMountObject.transform.Find("Ride01");
			m_trPivotHUD.SetParent(trRidePosition);
		}
		else if (IsTransformationStateCart())
		{
			m_trPivotHUD.SetParent(transform); // 윈래 위치로 본 이동.
			m_csCartObject.transform.SetParent(transform.parent);
			m_csEquipment.MidChangeEquipments(m_csHeroCustomData);
			m_csCartObject.transform.SetParent(transform);
			Transform trRidePosition = m_csCartObject.RidindCart.Find("Ride01");
			m_trPivotHUD.SetParent(trRidePosition);
		}
		else if (IsTransformationStateNone())
		{
			m_csEquipment.MidChangeEquipments(m_csHeroCustomData);
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventReturnScrollUseStart() // 마을 귀환서 사용.
	{
		Debug.Log("OnEventReturnScrollUseStart()");
		ResetBattleMode();
		SetAutoPlay(null, true);
		
		if (m_csSkillStatus.IsStatusPlayAnim()) return false;
		if (CsIngameData.Instance.ReturnScroll) return false;
		if (CsIngameData.Instance.IngameManagement.IsContinent() == false) return false;	// 던전에서 사용 불가.

		ChangeTransformationState(EnTransformationState.None, true);
		CsIngameData.Instance.ReturnScroll = true;
		ChangeState(EnState.ReturnScroll);
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventReturnScrollUseCancel() // 마을 귀환서 사용 취소.
	{
		CsIngameData.Instance.ReturnScroll = false;
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventImmediateRevived() // 유저 즉시부활.
	{
		m_flTrapMoveSpeed = 0f;
		DeadSetting(false);
		m_flHpChangeTime = Time.time;
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHpPotionUse() // 물약 사용. 
	{
		Debug.Log("OnEventHpPotionUse : " + transform.name + " hp : " + Hp);
		m_flHpChangeTime = Time.time;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTab()
	{
		if (m_csSkillStatus.IsStatusPlay()) return;

		if (!IsTargetEnemy)
		{
			SelectTarget(FindBestTarget(transform.position, transform.position, 50), true, EnPlayerTarget.Enemy);
		}
		else
		{
			FindTabTartget();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnStartTutorial() // 튜토리얼 시작 오토 종료.
	{
		ResetBattleMode();
		//if (IsAutoPlaying && m_itrAutoPlay.GetType() == EnAutoMode.Move)
		//{			
		//	SetAutoPlay(null, true);
		//}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAutoStop(EnAutoStateType enAutoStateType)
	{
		if (enAutoStateType == EnAutoStateType.Move || enAutoStateType == EnAutoStateType.Battle)
		{
			SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMountEquipped(CsHeroMount csHeroMount)
	{
		m_nMountId = csHeroMount.Mount.MountId;
		m_nMountLevel = csHeroMount.Level;
		if (IsTransformationStateMount())
		{
			ChangeTransformationState(EnTransformationState.Mount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMountGetOn()
	{
		ResetBattleMode();
		ChangeTransformationState(EnTransformationState.Mount);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMountGetOff()
	{		
		ChangeTransformationState(EnTransformationState.None, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWingEquip()
	{
		m_csHeroCustomData.WingId = m_csMyHeroInfo.EquippedWingId;
		m_csEquipment.CreateWing(m_csHeroCustomData, m_trPivotHUD);
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventIsStateIdle()
	{
		return IsStateIdle;
	}

	#endregion 03. Event.

	#region 04 FindTarget

	//---------------------------------------------------------------------------------------------------
	public static Transform FindTarget(Vector3 vtPosFind, Vector3 vtPosNear, string strLayer, string strObject, float flRadius, int nResourceId = -1, long lInstanceId = -1)	
	{
		Transform trTarget = null;
		int nLayerMask = 1 << LayerMask.NameToLayer(strLayer);
		Collider[] acol = Physics.OverlapSphere(vtPosFind, flRadius, nLayerMask);
		float flDistanceMin = flRadius * 2;

		for (int i = 0; i < acol.Length; i++)
		{
			Transform tr = acol[i].transform;
			if (tr.CompareTag(strObject))
			{
				if (nResourceId >= 0 || lInstanceId >= 0)
				{
					IInteractionObject itr = tr.GetComponent<IInteractionObject>();
					if (itr == null) continue;
					
					if (nResourceId >= 0 && itr.GetSpecificId() != nResourceId) continue;
					if (lInstanceId >= 0 && itr.GetInstanceId() != lInstanceId) continue;
				}

				float flDistance = Vector3.Distance(tr.position, vtPosNear);

				if (flDistance < flDistanceMin)
				{
					trTarget = tr;
					flDistanceMin = flDistance;
				}
			}
		}
		return trTarget;
	}

	//---------------------------------------------------------------------------------------------------
	public Transform FindNpc(Vector3 vtPos)
	{
		return FindTarget(vtPos, vtPos, "Npc", "Npc", 4f);
	}

	//---------------------------------------------------------------------------------------------------
	void AutoSelectTarget(float flFindRange)
	{
		if (Dead) return;
		if (m_trTarget == null || m_enTarget == EnPlayerTarget.Auto)
		{
			Transform trTargetHero = null;
			Transform trTargetMonster = null;
			float flDistanceMin = flFindRange;

			int nLayerMask = 1 << LayerMask.NameToLayer(CsMonster.c_strLayer);

			if (m_bSafeMode == false)
			{
				nLayerMask = 1 << LayerMask.NameToLayer(CsMonster.c_strLayer) | (1 << LayerMask.NameToLayer("Hero"));
			}

			Collider[] acol = Physics.OverlapSphere(transform.position, flFindRange, nLayerMask);

			for (int i = 0; i < acol.Length; i++)
			{
				Transform tr = acol[i].transform;

				if (IsNavMeshCornerCountIn(tr.position, 8)) // 네브메쉬 코너가 8개 미만일때.
				{
					float flDistance = Vector3.Distance(tr.position, transform.position);

					if (flDistance < flDistanceMin)
					{
						if (m_bSafeMode)
						{
							if (tr.CompareTag(CsMonster.c_strTag))
							{
								trTargetMonster = tr;
							}
						}
						else
						{
							if (tr.CompareTag("EnemyHero") || tr.CompareTag("EnemyCart"))
							{
								trTargetHero = tr;
							}
							else if (tr.CompareTag(CsMonster.c_strTag))
							{
								trTargetMonster = tr;
							}
						}
			
						flDistanceMin = flDistance;
					}
				}
			}

			Transform trAuto = (trTargetHero != null) ? trTargetHero : trTargetMonster;

			if (trAuto == null || trAuto == m_trTarget) return;

			SelectTarget(trAuto, true, EnPlayerTarget.Auto);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public Transform FindBestTarget(Vector3 vtPosFind, Vector3 vtPosNear, float flFindRange)
	{
		Transform trTarget = null;
		if (m_bSafeMode == false)
		{
			trTarget = FindTarget(vtPosFind, vtPosNear, "Hero", "EnemyHero", flFindRange); // 적국 유저 먼저 탐색.
		}
		
		if (trTarget == null)
		{
			trTarget = FindTarget(vtPosFind, vtPosNear, CsMonster.c_strLayer, CsMonster.c_strTag, flFindRange); // 가까운 적이 없는 경우 몬스터 탐색.
		}
		return trTarget;
	}

	//---------------------------------------------------------------------------------------------------
	// 서버에 전달할 유효한 타겟 리스트 생성.
	//---------------------------------------------------------------------------------------------------
	void FindAttackTarget(int nHitId)
	{
		if (m_csSkillStatus.JobSkill == null) return;

		int nLayerMask = (1 << LayerMask.NameToLayer(CsMonster.c_strLayer));

		if (m_bSafeMode == false) // 안전모드가 아닐때.
		{
			if (m_csSkillStatus.JobSkill.HeroHitTypeEnum != EnHeroHitType.None)
			{
				nLayerMask = (1 << LayerMask.NameToLayer(CsMonster.c_strLayer)) | (1 << LayerMask.NameToLayer("Hero"));
			}
		}

		EnHitAreaType enHitAreaType = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaTypeEnum : m_csSkillStatus.JobSkill.HitAreaTypeEnum;	// 연계스킬 or 일반 스킬.
		float flHitAreaValue1 = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaValue1 : m_csSkillStatus.JobSkill.HitAreaValue1;			// 연계스킬 or 일반 스킬.
		float flHitAreaValue2 = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaValue2 : m_csSkillStatus.JobSkill.HitAreaValue2;			// 연계스킬 or 일반 스킬.

		Vector3 vtCenter;

		switch (enHitAreaType) // 적중범위유형(1:원(반지름,각도), 2:사각형(가로, 세로))
		{
			case EnHitAreaType.Circle: // 부채꼴 포함.
				EnHitAreaOffsetType enHitAreaOffsetType = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaOffsetTypeEnum : m_csSkillStatus.JobSkill.HitAreaOffsetTypeEnum;
				float flOffest = m_csSkillStatus.IsChainSkill() ? m_csSkillStatus.GetChainSkill().HitAreaOffset : m_csSkillStatus.JobSkill.HitAreaOffset;           // 연계스킬 or 일반 스킬.
				vtCenter = (enHitAreaOffsetType == EnHitAreaOffsetType.Person) ? transform.position : m_vtTargetPos;
				vtCenter = vtCenter + (transform.forward * flOffest);
				Collider[] acollider1 = Physics.OverlapSphere(vtCenter, flHitAreaValue1, nLayerMask);
				AttackTargetSend(acollider1, enHitAreaType, flHitAreaValue2, nHitId);
				break;

			case EnHitAreaType.Rectangle: // OverlapBox을 모바일빌드에서 사용하면 엡이 죽어버리는 증상이 있음. 
				vtCenter = transform.position + (transform.forward * flHitAreaValue2 / 2);
				Vector3 vtHalfExtents = new Vector3(flHitAreaValue1 / 2, 2, flHitAreaValue2 / 2);
				Quaternion quaternion = Quaternion.LookRotation(transform.forward);
				RaycastHit[] araycastHit = Physics.BoxCastAll(vtCenter, vtHalfExtents, transform.forward, quaternion, 0, nLayerMask);
				AttackTargetSend(araycastHit, enHitAreaType, flHitAreaValue2, nHitId);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void AttackTargetSend(Collider[] acollider, EnHitAreaType enHitAreaType, float flHitAreaValue2, int nHitId)
	{
		List<PDHitTarget> listPDHitTarget = new List<PDHitTarget>();

		for (int i = 0; i < acollider.Length; i++)
		{
			Transform trTarget = acollider[i].transform;
			if (enHitAreaType == EnHitAreaType.Circle)
			{
				Vector3 vtDir = (trTarget.position - transform.position).normalized;
				vtDir.y = 0;

				if (Vector3.Angle(transform.forward, vtDir) > flHitAreaValue2 * 0.5f) // 써클형 범위에서 벗어나면 다음타겟 확인.
				{
					if (GetDistanceFormTarget(trTarget.position) > 0.5f) // 플레이어와 겹쳐있지 않을때.
					{
						continue; 
					}
				}
			}

			if (trTarget.CompareTag(CsMonster.c_strTag))
			{
				listPDHitTarget.Add(new PDMonsterHitTarget(CsIngameData.Instance.IngameManagement.GetInstanceId(trTarget)));
			}
			else if (trTarget.CompareTag("EnemyHero"))
			{
				listPDHitTarget.Add(new PDHeroHitTarget(CsIngameData.Instance.IngameManagement.GetHeroId(trTarget)));
			}
			else if (trTarget.CompareTag("EnemyCart"))
			{
				listPDHitTarget.Add(new PDCartHitTarget(CsIngameData.Instance.IngameManagement.GetInstanceId(trTarget)));
			}
		}

		PDHitTarget[] apDHitTarget = listPDHitTarget.ToArray();
		SendSkillHitEvent(apDHitTarget, nHitId);
	}

	//---------------------------------------------------------------------------------------------------
	void AttackTargetSend(RaycastHit[] araycastHit, EnHitAreaType enHitAreaType, float flHitAreaValue2, int nHitId)
	{
		List<PDHitTarget> listPDHitTarget = new List<PDHitTarget>();

		for (int i = 0; i < araycastHit.Length; i++)
		{
			Transform trTarget = araycastHit[i].transform;
			if (trTarget.CompareTag(CsMonster.c_strTag))
			{
				listPDHitTarget.Add(new PDMonsterHitTarget(CsIngameData.Instance.IngameManagement.GetInstanceId(trTarget)));
			}
			else if (trTarget.CompareTag("EnemyHero"))
			{
				listPDHitTarget.Add(new PDHeroHitTarget(CsIngameData.Instance.IngameManagement.GetHeroId(trTarget)));
			}
			else if (trTarget.CompareTag("EnemyCart"))
			{
				listPDHitTarget.Add(new PDCartHitTarget(CsIngameData.Instance.IngameManagement.GetInstanceId(trTarget)));
			}
		}

		PDHitTarget[] apDHitTarget = listPDHitTarget.ToArray();
		SendSkillHitEvent(apDHitTarget, nHitId);
	}

	//---------------------------------------------------------------------------------------------------
	void FindTabTartget()
	{
		int nLayerMask = (1 << LayerMask.NameToLayer(CsMonster.c_strLayer) | (1 << LayerMask.NameToLayer("Hero")));
		Collider[] acols = Physics.OverlapSphere(transform.position, 50, nLayerMask);

		int nIndex = -1;

		if (IsTargetEnemy)
		{
			for (int i = 0; i < acols.Length; i++)
			{
				Transform tr = acols[i].gameObject.transform;
				if (tr.CompareTag(CsMonster.c_strTag) || tr.CompareTag("EnemyHero") || tr.CompareTag("EnemyCart"))
				{
					if (tr == m_trTarget)
					{
						nIndex = i;
						break;
					}
				}
			}
		}
		nIndex++;

		Transform trNext = FindSelectableMonster(acols, nIndex, acols.Length);

		if (trNext == null)
		{
			trNext = FindSelectableMonster(acols, 0, nIndex);
		}

		SelectTarget(trNext, true, EnPlayerTarget.Enemy);
	}

	//---------------------------------------------------------------------------------------------------
	Transform FindSelectableMonster(Collider[] acols, int nStartIndex, int nCount)
	{
		int nCount2 = Mathf.Min(nCount, acols.Length);

		for (int i = nStartIndex; i < nCount2; i++)
		{
			Transform tr = acols[i].gameObject.transform;
			if (tr.CompareTag(CsMonster.c_strTag) || tr.CompareTag("EnemyHero") || tr.CompareTag("EnemyCart"))
			{
				return tr;
			}
		}
		return null;
	}

	#endregion 04 FindTarget

	#region 05 Move

	Vector3 m_vtLastSendPos;
	
	float m_flDashTime = 0;
	float m_flLastSendAngle = 0;

	int m_nDashMotionCount = 0;

	bool m_bMoving = false;
	bool m_bTargetNpc = false;
	bool m_bAttackRush = false;
	bool m_bJoyAutoMove = false;
	bool m_bJoysticDraggingOld = false;

	//---------------------------------------------------------------------------------------------------
	void MyHeroMoveStart(bool bWalk)
	{
		if (m_bMoving) return;
		if (CsIngameData.Instance.Directing) return; // 연출중.

		m_bMoving = true;
		m_bWalk = bWalk;

		if (m_enTransformationState == EnTransformationState.Cart)
		{
			CsCartManager.Instance.SendCartMoveStartEvent(m_guidPlaceInstanceId);
		}
		else
		{
			bool bManual = m_enState == EnState.MoveByJoystic ? true : false;			
			SendMoveStartEvent(bManual, m_bWalk);
		}
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

		if (m_enTransformationState == EnTransformationState.Cart)
		{
			CsCartManager.Instance.SendCartMoveEvent(m_guidPlaceInstanceId, m_vtLastSendPos, m_flRotationY);
		}
		else
		{
			SendMoveEvent(m_vtLastSendPos);
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	void MyHeroMoveEnd()
	{
		if (!m_bMoving) return;
		//Debug.Log("CsMyPlayer.MyHeroMoveEnd()");
		MyHeroMove();

		if (IsTransformationStateCart())
		{
			CsCartManager.Instance.SendCartMoveEndEvent(m_guidPlaceInstanceId);
		}
		else
		{
			if (m_bAcceleration)
			{
				ResetAccelerate();
			}
			SendMoveEndEvent();
		}

		m_bWalk = false;
		m_bMoving = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void DirectingEnd()
	{
		SendMoveStartEvent(true, false);
		SendMoveEndEvent();
	}

	//---------------------------------------------------------------------------------------------------
	bool UpdateMoveByJoystick(float flAngle)
	{
		if (m_bAcceleration)    // 가속 상태일때.
		{
			if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.Story)    // 스토리 던전은 별도 가속이 있음.
			{
				if (m_flDashTime > 1.5f)
				{
					CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.User, transform, transform.position, "Gaia_Skill_01_02", 0.5f);
					m_flDashTime = 0.0f;

					if (m_nDashMotionCount > 2)
					{
						m_animator.SetTrigger(s_nAnimatorHash_dashmotion);
						m_nDashMotionCount = 0;
					}
					else
					{
						m_nDashMotionCount++;
					}
				}
				m_flDashTime += Time.deltaTime;
			}
		}

		if (CsGameData.Instance.JoystickDragging)
		{
			if (m_bJoyAutoMove || m_nJoyAutoWaitCount != 0)
			{
				m_nJoyAutoWaitCount = 0;
				ResetPathOfNavMeshAgent();
			}
			else if (IsAutoPlaying)
			{
				if (CsIngameData.Instance.AutoBattleMode == EnBattleMode.Auto ||
				   (m_itrAutoPlay.GetType() == EnAutoMode.Move && (EnAutoMovePlay)m_itrAutoPlay.GetTypeSub() == EnAutoMovePlay.Move))
				{
					if (m_bMoving)
					{
						SendMoveModeChanged();
					}
					ResetPathOfNavMeshAgent();
					SetAutoPlay(null, true, false);
				}
			}

			if (!m_bMoving)
			{
				MyHeroMoveStart(CsGameData.Instance.JoysticWalk);
			}
			else
			{
				if (m_bWalk != CsGameData.Instance.JoysticWalk)
				{
					SendMoveModeChanged();
					if (CsGameData.Instance.JoysticWalk && m_bAcceleration)
					{
						ResetAccelerate();
					}
				}				
			}

			if (m_csSkillStatus.IsStatusPlayAnim() == false)    // 스킬 동작중이 아닐때.
			{
				ChangeHeroMoveSpeed(GetHeroMoveSpeed());
			}

			m_navMeshAgent.velocity = AngleRotateDir(flAngle) * m_navMeshAgent.speed;

			if (m_vtPrevPos == transform.position) // 동일 장소에서 3 프레임 이상 대기시 초기화.
			{
				m_nSamePosMoveCount++;
				if (m_nSamePosMoveCount > 3)
				{
					ChangeHeroMoveSpeed(GetHeroMoveSpeed());
				}
			}
			else
			{
				m_nSamePosMoveCount = 0;
			}
		}
		else
		{
			if (CsGameData.Instance.JoystickDown)
			{
				if (JoystickAutoMove())
				{
					return false;
				}
				return true;
			}
			else
			{
				if (m_bJoyAutoMove || m_nJoyAutoWaitCount != 0)
				{
					if (m_bMoving)
					{
						SendMoveModeChanged();
					}
					m_nJoyAutoWaitCount = 0;
					ResetPathOfNavMeshAgent();
				}

				if (m_bMoving && m_bJoysticDraggingOld)
				{
					MyHeroMoveEnd();
					ChangeHeroMoveSpeed(GetHeroMoveSpeed());
				}
			}			
		}

		m_bJoysticDraggingOld = CsGameData.Instance.JoystickDragging;

		return !CsGameData.Instance.JoystickDragging;
	}

	//---------------------------------------------------------------------------------------------------
	bool UpdateMove(Vector3 vtPos, float flStopDistance)
	{
		if (!IsMovable()) return false;
		if (m_csSkillStatus.IsStatusPlayAnim()) return false;

		if (m_vtPrevPos == transform.position)
		{
			m_nSamePosMoveCount++;
			if (m_nSamePosMoveCount > 5)
			{
				GetHeroMoveSpeed();
			}

			if (m_nSamePosMoveCount > 30)
			{
				m_nSamePosMoveCount = 0;
				ResetPathOfNavMeshAgent(); // 네비 범위 밖의 Path 갖고 있으면 초기화가 안되서 강제 초기화.
				ChangeState(EnState.Idle);

				dd.d("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
				dd.d(vtPos, transform.position);
				return false;
			}
		}
		else
		{
			m_nSamePosMoveCount = 0;
			ChangeHeroMoveSpeed(GetHeroMoveSpeed());
		}

		if (m_bAttackRush == false)	// 데쉬가 아닐때.
		{
			if (IsTransformationStateFlight())
			{
				if (IsTargetInDistance(vtPos, flStopDistance + m_navMeshAgent.baseOffset))
				{
					StartCoroutine(DelayChangeTransformation());
					return true;
				}
			}
			else
			{
				if (IsTargetInDistance(vtPos, flStopDistance))
				{
					if (m_enState != EnState.MoveForSkill || NavMeshDirect(vtPos))
					{
						return true;
					}
				}
				else
				{
					if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.Story)
					{
						if (IsTransformationStateNone())
						{
							if (m_enState == EnState.MoveForSkill)
							{
								if (m_csSkillStatus.JobSkill != null && m_csSkillStatus.JobSkill.FormTypeEnum == EnFormType.Chain)
								{
									int nJobId = Job.ParentJobId == 0 ? Job.JobId : Job.ParentJobId;
									if ((EnJob)nJobId == EnJob.Gaia || (EnJob)nJobId == EnJob.Witch)
									{
										if (m_bAcceleration == false)
										{
											float flDistance = GetDistanceFormTarget(vtPos);
											if (flDistance > m_flAttackRange + 1 && flDistance <= 6)
											{
												m_bAttackRush = true;
												CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.AttackRush);
												transform.LookAt(vtPos);
												CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, vtPos - transform.forward, "Dash01", 1.5f);
												Vector3 vtRushPos = transform.position + (transform.forward * (flDistance - m_flAttackRange));
												m_navMeshAgent.enabled = false;
												transform.position = vtRushPos;
												m_navMeshAgent.enabled = true;

												m_csSkillStatus.SetStatusToPlay();
												Attack();

												StartCoroutine(MoveCoroutine());     // 몬스터 위치가 공격 가능 거리 위치보다 멀리 있을때.
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveByDirecting(Vector3 vtMovePos, float flStopRange, bool bRun)
	{
		m_animator.SetBool(s_nAnimatorHash_enterrun, bRun);
		if (bRun)
		{
			Move(EnState.MoveByDirecting, vtMovePos, flStopRange, true);
			ChangeHeroMoveSpeed(6f);
		}
		else
		{
			ChangeHeroMoveSpeed(4f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveToPos(Vector3 vtMovePos, float flStopRange, bool bTargetNpc)
	{
		m_bTargetNpc = bTargetNpc;

		if (m_bTargetNpc)
		{
			m_flNpcStopRange = flStopRange;
		}
		Move(EnState.MoveToPos, vtMovePos, flStopRange);
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveToPosNear(Vector3 vtMovePos, float flStopRange)
	{
		Move(EnState.MoveToPos, vtMovePos, flStopRange, true);
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveByTouch(Vector3 vtMovePos, float flStopRange, bool bMoveByTouchTarget = false)
	{
		if (IsAutoPlaying)
		{
			SetAutoPlay(null, true);
		}

		m_bMoveByTouchTarget = bMoveByTouchTarget;
		if (!Move(EnState.MoveByTouch, vtMovePos, flStopRange, bMoveByTouchTarget))
		{
			m_vtAttackAfterPos = vtMovePos;
			m_flAttackAfterStopRange = flStopRange;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveForSkill(CsJobSkill csJobSkill, EnSkillStarter enSkillStarter)
	{
		m_csSkillStatus.Init(csJobSkill, false, enSkillStarter);
		m_flAttackRange = GetAttackCastRange(m_trTarget, csJobSkill.CastRange);
		Move(EnState.MoveForSkill, m_trTarget.position, m_flAttackRange, false);
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveForSkill(CsJobCommonSkill csJobCommonSkill, EnSkillStarter enSkillStarter)
	{
		m_csSkillStatus.Init(csJobCommonSkill, false);
		m_flAttackRange = GetAttackCastRange(m_trTarget, csJobCommonSkill.CastRange);
		Move(EnState.MoveForSkill, m_trTarget.position, m_flAttackRange, true);
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveForSkill(CsRankActiveSkill csRankActiveSkill, EnSkillStarter enSkillStarter)
	{
		m_csSkillStatus.Init(csRankActiveSkill, false);
		m_flAttackRange = GetAttackCastRange(m_trTarget, csRankActiveSkill.CastRange);
		Move(EnState.MoveForSkill, m_trTarget.position, m_flAttackRange, true);
	}

	//---------------------------------------------------------------------------------------------------
	public void MoveForSkill(CsMonsterSkill csMonsterSkill, EnSkillStarter enSkillStarter)
	{
		m_csSkillStatus.Init(csMonsterSkill, false);
		m_flAttackRange = GetAttackCastRange(m_trTarget, csMonsterSkill.CastRange);
		Move(EnState.MoveForSkill, m_trTarget.position, m_flAttackRange, true);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void SetDestination(Vector3 vtPos, float flStopRange)
	{
		base.SetDestination(vtPos, flStopRange);
		MyHeroMoveStart(false);
	}

	//---------------------------------------------------------------------------------------------------
	void ResetPathOfNavMeshAgent()
	{
		if (m_navMeshAgent.isActiveAndEnabled)
		{
			m_navMeshAgent.ResetPath();
			ChangeHeroMoveSpeed(GetHeroMoveSpeed());
		}

		m_bWalk = false;
		m_bJoyAutoMove = false;
		m_bTargetNpc = false;
		MyHeroMoveEnd();
	}

	//---------------------------------------------------------------------------------------------------
	Vector3 AngleRotateDir(float flAngle)
	{
		float flRotationSpeed = 4f;
		Vector3 vtDir = CsIngameData.Instance.InGameCamera.transform.TransformDirection(CsConfiguration.Instance.GetMoveDirection(flAngle));
		vtDir.y = 0;
		vtDir.Normalize();

		if (vtDir != Vector3.zero)
		{
			Quaternion qtnFrom = transform.rotation;
			Quaternion qtnTo = Quaternion.LookRotation(vtDir);
			transform.rotation = Quaternion.Lerp(qtnFrom, qtnTo, flRotationSpeed * Time.deltaTime);
		}
		return vtDir;
	}

	//---------------------------------------------------------------------------------------------------
	void CheckJoystickMove()
	{
		if (IsTransformationStateMonster())
		{
			if (m_bWalk)
			{
				m_navMeshAgent.speed = (float)m_csTransformationMonster.MonsterInfo.MoveSpeed / 100;
			}
			else
			{
				m_navMeshAgent.speed = (float)m_csTransformationMonster.MonsterInfo.BattleMoveSpeed / 100;
			}
		}
		else
		{
			if (m_bWalk)
			{
				m_navMeshAgent.speed = 2f + (m_flJoysticWalk * 4);

				if (m_flJoysticWalk != 0)
				{
					if (m_flJoysticWalk - 0.1f >= 0)
					{
						m_flJoysticWalk -= 0.1f;
					}
					else
					{
						m_flJoysticWalk = 0f;
					}

					m_flAccelerationValue = 0f;
					m_animator.SetFloat(s_nAnimatorHash_acceleration, m_flAccelerationValue);
					m_animator.SetFloat(s_nAnimatorHash_move, m_flJoysticWalk);
				}
			}
			else
			{
				if (m_flJoysticWalk != 1)
				{
					if (m_flJoysticWalk + 0.1f <= 1)
					{
						m_flJoysticWalk += 0.1f;
					}
					else
					{
						m_flJoysticWalk = 1f;
					}

					m_navMeshAgent.speed = 2f + (m_flJoysticWalk * 4);
					m_animator.SetFloat(s_nAnimatorHash_move, m_flJoysticWalk);
				}
			}
		}		
	}
	int m_nJoyAutoWaitCount = 0;
	//---------------------------------------------------------------------------------------------------
	bool JoystickAutoMove()
	{
		if (IsAutoPlaying && m_enState == EnState.MoveByJoystic)
		{
			if (m_csDisplayPath != null)
			{
				if (m_bJoyAutoMove == false || m_csDisplayPath.MoveTarget != m_vtMovePos)
				{
					if (m_csDisplayPath.MoveTarget != Vector3.zero)
					{
						if (IsTargetInDistance(m_csDisplayPath.MoveTarget, 1f) == false)
						{
							if (m_nJoyAutoWaitCount > 4)
							{
								m_bJoyAutoMove = true;
								m_flJoysticWalk = 1;
								CsGameEventToUI.Instance.OnEventJoystickStartAutoMove();
							}
							else
							{
								m_flJoysticWalk = m_nJoyAutoWaitCount * 0.2f;

								if (m_nJoyAutoWaitCount == 0)
								{
									SetDestination(m_csDisplayPath.MoveTarget, 1f);
								}
							}

							m_animator.SetFloat(s_nAnimatorHash_move, m_flJoysticWalk);
							m_nJoyAutoWaitCount++;
							return true;
						}
					}
				}
				else
				{
					if (IsTargetInDistance(m_vtMovePos, 1f))
					{
						if (m_bJoyAutoMove)
						{
							ResetPathOfNavMeshAgent();
						}
					}
					else
					{
						if (m_bAcceleration)
						{
							ChangeHeroMoveSpeed(GetHeroMoveSpeed());
						}
						return true;
					}
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator MoveCoroutine()
	{
		yield return new WaitForSeconds(0.15f);
		m_bAttackRush = false;
		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayChangeTransformation()
	{
		yield return new WaitForSeconds(0.5f);
		ChangeTransformationState(EnTransformationState.None, true);
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
			if (CsGameData.Instance.JoystickDragging ||
				IsTargetEnemy == false ||
				IsTargetInDistance(m_trTarget.position, GetAttackCastRange(m_trTarget, m_csSkillStatus.JobSkill.CastRange)) == false)
			{
				m_csSkillStatus.Reset();
				m_csSkillStatus.Chained = false;
				ChangeState(EnState.Idle);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAttackEnd()
	{
		m_navMeshAgent.updateRotation = true;
		ResetPathOfNavMeshAgent();
		m_csSkillStatus.Reset();
		m_flAttackRange = 0f;

		if (Hp == 0) // 죽음상태여야 하는데 죽지 않아서 죽음 처리.
		{
			Debug.Log("MyPlayer.OnAttckEnd()      공격 종료시 Dead 처리 과정 추가 ");
			ChangeState(EnState.Idle);
			MyHeroDead();
		}
		else
		{
			if (PlayNextSkill() == false)
			{
				if (m_enStateAttackAfter == EnState.MoveByTouch)
				{
					if (IsMovable())
					{
						MoveByTouch(m_vtAttackAfterPos, m_flAttackAfterStopRange);
					}
				}
				else
				{
					ChangeState(EnState.Idle);
					if (EventAttackEnd != null)
					{
						EventAttackEnd();
					}
				}

				m_enStateAttackAfter = EnState.Idle;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnAnimSkillMove(int nMoveCount) // 스킬 동작중 이동.
	{
		if (IsTargetEnemy)
		{
			transform.LookAt(m_vtTargetPos);
		}

		m_navMeshAgent.updateRotation = false;

		if (m_csSkillStatus.JobSkill != null)
		{
			ChangeHeroMoveSpeed(m_flSkillMoveSpeed);
			if (m_navMeshAgent.isActiveAndEnabled)
			{
				m_navMeshAgent.SetDestination(m_vtSkillMovePos);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimTargetSelect(int nHitId)
	{
		if (nHitId == 0)
		{
			nHitId = 1;
		}
		FindAttackTarget(nHitId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId)
	{
		//Debug.Log(" OnAnimApplyDamage	nHitId : " + nHitId + " // m_csHitSkillCast = " + m_csHitSkillCast);
		if (m_csSkillStatus.JobSkill == null || m_csHitSkillCast == null) return;

		DamgaeSound(m_csSkillStatus.JobSkill.SkillId);
		CsIngameData.Instance.InGameCamera.DoShake(2, !m_csSkillStatus.IsChainSkill());

		bool bKnockback = false;

		int nJobId = Job.ParentJobId == 0 ? Job.JobId : Job.ParentJobId;

		//if ((EnJob)nJobId != EnJob.Deva)
		//{
		//	bKnockback = m_csSkillStatus.JobSkill.SkillId == 2 ? true : false;
		//}

		for (int i = 0; i < m_csHitSkillCast.HitInfoNodeList.Count; i++)
		{
			CsHitInfoNode csHitInfoNode = m_csHitSkillCast.HitInfoNodeList[i];
			if (csHitInfoNode != null)
			{
				if (csHitInfoNode.HitResult != null && csHitInfoNode.Target != null && csHitInfoNode.HitResult.hitId == nHitId)
				{
					CsGameEventToUI.Instance.OnEventHitCombo();	// 콤보 이벤트 전달.
					CsIngameData.Instance.IngameManagement.AttackByHit(csHitInfoNode.Target, csHitInfoNode.HitResult, csHitInfoNode.HitTime, csHitInfoNode.AddHitResult, true, bKnockback);
					AttackHitEffect(csHitInfoNode.Target);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimMentalSkill()
	{
		SendJobCommonSkillHit();
		if (CsIngameData.Instance.Effect != 0 && m_trTarget != null)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, m_trTarget, m_trTarget.position, "MetalSkill", 0.8f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRun()
	{
		m_navMeshAgent.updateRotation = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimFishingStart()
	{
		int nJobId = Job.ParentJobId == 0 ? Job.JobId : Job.ParentJobId;
		m_csEquipment.CreateFishStaff(nJobId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadStart()
	{
		Debug.Log("OnAnimDeadStart          CsIngameData.Instance.MyHeroDead = "+ CsIngameData.Instance.MyHeroDead);		
		m_csHitSkillCast = null;
		m_csSkillStatus.ResetNextSkill();
		m_csSkillStatus.Reset();
		CsGameEventToUI.Instance.OnEventHeroDead(CsIngameData.Instance.IngameManagement.GetName(m_trKiller)); // 죽인대상 UI 전달.
		CsIngameData.Instance.InGameCamera.ChangeTexture(Dead);
		m_trKiller = null;
	}

	protected virtual void DamgaeSound(int nSkill) { }
	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		m_csSkillStatus.Display();
		if (m_csSkillStatus.IsStatusAnim())
		{
			OnAttackEnd();
		}
		else if (m_bDungeonClear)
		{
			m_bDungeonClear = false;
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		//Debug.Log("1. Start    OnEventAnimStartAttack     m_trTarget = " + m_trTarget);
		ResetPathOfNavMeshAgent();

		if (Dead)
		{
			if (m_enState != EnState.Dead)
			{
				ChangeState(EnState.Dead);
			}
		}
		else
		{
			m_vtTargetPos = m_vtSkillMovePos = transform.position;

			if (m_csSkillStatus.IsSkillTypeCommon())		// 공용스킬.
			{
				if (IsTargetEnemy)
				{
					m_vtTargetPos = m_trTarget.position;
				}

				SendJobCommonSkillCast(m_csSkillStatus.JobCommonSkill);
				m_csSkillStatus.OnSkillAnimStarted();
			}
			else if (m_csSkillStatus.IsSkillTypeHero())					 // 영웅스킬인.
			{
				m_csSkillStatus.Display();
				if (IsTargetEnemy)
				{
					m_vtTargetPos = m_trTarget.position;
				}

				m_csSkillStatus.OnSkillAnimStarted();
				SetSkillCastingFixedMoveTypeData(m_trTarget);

				SendSkillCastEvent(m_vtSkillMovePos);

				if (m_csSkillStatus.JobSkill.FormTypeEnum == EnFormType.Buff) return;			// 버프스킬.
				if (m_csSkillStatus.JobSkill.TypeEnum == EnJobSkillType.AreaOfEffect) return;	// 장판스킬.

				m_csHitSkillCast = null;
				m_csHitSkillCast = new CsHitSkillCast(m_csSkillStatus.JobSkill.SkillId, m_csSkillStatus.GetChainSkillId());
			}
		}
	}

	#endregion 06 Anim

	#region 09 Skill

	//---------------------------------------------------------------------------------------------------
	bool OnEventUseSkill(CsHeroSkill csHeroSkill)
	{
		Debug.Log("OnEventUseSkill : "+ IsAttackable());
		if (!IsAttackable()) return false;

		if (m_enTarget != EnPlayerTarget.Enemy)
		{
			if (m_enTarget == EnPlayerTarget.Auto)
			{
				m_enTarget = EnPlayerTarget.Enemy;
			}
			else
			{
				SelectTarget(FindBestTarget(transform.position, transform.position, CsGameConfig.Instance.StandingBattleRange), true, EnPlayerTarget.Enemy);
			}
		}

		if (IsTransformationStateMount() || IsTransformationStateCart()) //  변신 상태일때는 공격 불가 처리.变身时无法攻击。
        {
			ChangeTransformationState(EnTransformationState.None, true);
		}
		else if (IsTransformationStateTame())
		{
			if (m_csSkillStatus.IsStatusReady())
			{
				m_csSkillStatus.Reset();
			}

			if (CheckEnemyHeroAttackAble() == false) return false;

			m_flAttackRange = GetAttackCastRange(m_trTarget, csHeroSkill.JobSkill.CastRange);

			if (IsTargetEnemy == false || IsTargetInDistance(m_trTarget.position, m_flAttackRange)) // 공격 대상이 없거나 공격대상이 거리 내에 있을때.
			{
				// m_csSkillStatus.Init(csHeroSkill.JobSkill, true, EnSkillStarter.UI); // 공격.
				// Attack();
				// 휴머노이드 에니메이션의 방향 좌표 보정을 위해서 이동을 통한 지연 처리. ( 바로 공격하는경우 대상을 바라보지 않음.)

				LookAtPosition(m_trTarget.position);
				MoveForSkill(csHeroSkill.JobSkill, EnSkillStarter.UI);
				return true;
			}
			else
			{
				if (IsMovable())
				{
					LookAtPosition(m_trTarget.position);
					MoveForSkill(csHeroSkill.JobSkill, EnSkillStarter.UI);
					return true;
				}
			}
		}
		else
		{
			if (m_csSkillStatus.IsStatusPlayAnim())
			{
				if (m_csSkillStatus.JobSkill == null) return false;
				if (m_csSkillStatus.IsStatusPlay()) return false;

				if (m_csSkillStatus.IsChainable(csHeroSkill.JobSkill.SkillId) == false) // 스킬중 연계스킬 입력이 아닌 경우(NextSkill에 저장) .
				{
					if (m_csSkillStatus.JobSkill.SkillId == CsGameConfig.Instance.SpecialSkillId && csHeroSkill.JobSkill.SkillId == CsGameConfig.Instance.SpecialSkillId) return false;
					if (CheckEnemyHeroAttackAble() == false) return false;

					m_csSkillStatus.SetNextJobSkill(csHeroSkill.JobSkill);
					m_enStateAttackAfter = EnState.Idle;
				}
				else
				{
					if (CheckEnemyHeroAttackAble() == false) return false;

					if (IsTargetEnemy) // 타겟이 이미 존재하는 경우.
					{
						m_flAttackRange = GetAttackCastRange(m_trTarget, csHeroSkill.JobSkill.CastRange);

						if (!IsTargetInDistance(m_trTarget.position, m_flAttackRange)) return false;
					}

					if (m_csSkillStatus.Chained)
					{
						return false;
					}

					m_csSkillStatus.ResetNextSkill();
					m_csSkillStatus.Chained = true;
					Attack();
				}
				return true;
			}
			else
			{
				if (m_csSkillStatus.IsStatusReady())
				{
					m_csSkillStatus.Reset();
				}

				if (PlayJobSkill(csHeroSkill.JobSkill, EnSkillStarter.UI))
				{
					return true;
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventUseCommonSkill(CsJobCommonSkill csJobCommonSkill)
	{
		if (!IsAttackable()) return false;

		if (m_csSkillStatus.IsStatusPlayAnim())
		{
			m_csSkillStatus.SetNextCommonSkill(csJobCommonSkill);
			m_enStateAttackAfter = EnState.Idle;
			return true;
		}

		if (IsTransformationStateNone() == false)
		{
			if (IsTransformationStateMount() || IsTransformationStateCart()) //  변신 상태일때는 공격 불가 처리.
			{
				ChangeTransformationState(EnTransformationState.None, true);
			}
			else
			{
				return false;
			}
		}

		m_csSkillStatus.ResetNextSkill();

		if (PlayCommonSkill(csJobCommonSkill))
		{
			return true;
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventUseRankActiveSkill(CsRankActiveSkill csRankActiveSkill)
	{
		if (!IsAttackable()) return false;

		if (m_csSkillStatus.IsStatusPlayAnim())
		{
			m_csSkillStatus.SetNextRankActiveSkill(csRankActiveSkill);
			m_enStateAttackAfter = EnState.Idle;
			return true;
		}

		if (IsTransformationStateNone() == false)
		{
			if (IsTransformationStateMount() || IsTransformationStateCart()) //  변신 상태일때는 공격 불가 처리.
			{
				ChangeTransformationState(EnTransformationState.None, true);
			}
			else
			{
				return false;
			}
		}

		m_csSkillStatus.ResetNextSkill();

		if (PlayRankActiveSkill(csRankActiveSkill))
		{
			return true;
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventUseTransformationMonsterSkillCast(CsMonsterSkill csMonsterSkill)
	{
		if (!IsAttackable()) return false;

		if (m_csSkillStatus.IsStatusPlayAnim()) return false;

		if (IsTransformationStateMonster())
		{
			if (PlayTransformationMonsterSkill(csMonsterSkill))
			{
				return true;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool PlayJobSkill(CsJobSkill csJobSkill, EnSkillStarter enSkillStarter)
	{
		if (csJobSkill != null)
		{
			m_flAttackRange = GetAttackCastRange(m_trTarget, csJobSkill.CastRange);

			if (IsTargetEnemy == false)
			{
				if (m_enTarget == EnPlayerTarget.Auto)
				{
					m_enTarget = EnPlayerTarget.Enemy;
				}
				else
				{
					SelectTarget(FindBestTarget(transform.position, transform.position, CsGameConfig.Instance.StandingBattleRange), true, EnPlayerTarget.Enemy);
				}
			}

			if (enSkillStarter == EnSkillStarter.AutoBattle)
			{
				if (IsTargetEnemy)
				{
					CsGameEventToUI.Instance.OnEventUseAutoSkill(csJobSkill.SkillId); // 자동스킬 사용 UI에 전달.

					if (csJobSkill.FormTypeEnum == EnFormType.Buff || csJobSkill.SkillId == CsGameConfig.Instance.SpecialSkillId)
					{
						m_csSkillStatus.Init(csJobSkill, true, enSkillStarter);
						Attack();
						return true;
					}
					else
					{
						if ((IsTargetInDistance(m_trTarget.position, m_flAttackRange) && NavMeshDirect(m_trTarget.position)))
						{
							//m_csSkillStatus.Init(csJobSkill, true, enSkillStarter);
							//Attack();
							// 휴머노이드 에니메이션의 방향 좌표 보정을 위해서 이동을 통한 지연 처리. ( 바로 공격하는경우 대상을 바라보지 않음.)

							LookAtPosition(m_trTarget.position);
							MoveForSkill(csJobSkill, enSkillStarter);
							return true;
						}
						else
						{
							if (IsMovable())
							{
								LookAtPosition(m_trTarget.position);
								MoveForSkill(csJobSkill, enSkillStarter);
								return true;
							}
						}
					}
				}
			}
			else if (enSkillStarter == EnSkillStarter.UI)
			{
				if (csJobSkill.FormTypeEnum == EnFormType.Buff || csJobSkill.IsRequireTarget == false) // 버프스킬이거나 타겟이 필요 없는 스킬인경우.
				{
					m_csSkillStatus.Init(csJobSkill, true, enSkillStarter);
					Attack();
					return true;
				}
				else
				{
					if (IsTargetEnemy)
					{
						if ((IsTargetInDistance(m_trTarget.position, m_flAttackRange) && NavMeshDirect(m_trTarget.position)))
						{
							LookAtPosition(m_trTarget.position);
							//m_csSkillStatus.Init(csJobSkill, true, enSkillStarter);
							//Attack();
							MoveForSkill(csJobSkill, enSkillStarter);
							return true;
						}
						else
						{
							if (IsMovable())
							{
								LookAtPosition(m_trTarget.position);
								MoveForSkill(csJobSkill, enSkillStarter);
								return true;
							}
						}
					}
					else
					{
						m_csSkillStatus.Init(csJobSkill, true, enSkillStarter);
						Attack();
						return true;
					}
				}
			}
		}

		m_csSkillStatus.ResetNextSkill();
		ChangeState(EnState.Idle);
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool PlayCommonSkill(CsJobCommonSkill csJobCommonSkill)
	{
		if (csJobCommonSkill != null)
		{
			Debug.Log("PlayCommonSkill     csJobCommonSkill  " + csJobCommonSkill.SkillId);

			if (csJobCommonSkill.SkillId == 1)  // 버프스킬.
			{
				ChangeState(EnState.Idle);
				StartCoroutine(DelayConfirmUseCommonSkill(csJobCommonSkill));
				return true;
			}
			else if (csJobCommonSkill.SkillId == 2) // 대상스킬.
			{
				if (IsTargetEnemy == false)
				{
					if (m_enTarget == EnPlayerTarget.Auto)
					{
						m_enTarget = EnPlayerTarget.Enemy;
					}
					else
					{
						SelectTarget(FindBestTarget(transform.position, transform.position, CsGameConfig.Instance.StandingBattleRange), true, EnPlayerTarget.Enemy);
					}
				}

				if (IsTargetEnemy)
				{
					m_flAttackRange = GetAttackCastRange(m_trTarget, csJobCommonSkill.CastRange);
					if (IsTargetInDistance(m_trTarget.position, m_flAttackRange)) // 공격대상이 거리 내에 있을때.
					{
						LookAtPosition(m_trTarget.position);
						m_csSkillStatus.Init(csJobCommonSkill, true); // 공격.
						Attack();
						return true;
					}
					else
					{
						if (IsMovable())
						{
							MoveForSkill(csJobCommonSkill, EnSkillStarter.UI);
							return true;
						}
					}
				}
			}
		}

		ChangeState(EnState.Idle);
		m_csSkillStatus.ResetNextSkill();
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool PlayRankActiveSkill(CsRankActiveSkill csRankActiveSkill)
	{
		if (csRankActiveSkill != null)
		{
			Debug.Log("PlayRankActiveSkill          SkillId = " + csRankActiveSkill.SkillId);

			if (csRankActiveSkill.Type == 1)  // 해로운(대상). 
			{
				if (IsTargetEnemy == false)
				{
					if (m_enTarget == EnPlayerTarget.Auto)
					{
						m_enTarget = EnPlayerTarget.Enemy;
					}
					else
					{
						SelectTarget(FindBestTarget(transform.position, transform.position, CsGameConfig.Instance.StandingBattleRange), true, EnPlayerTarget.Enemy);
					}
				}

				if (IsTargetEnemy)
				{
					m_flAttackRange = GetAttackCastRange(m_trTarget, csRankActiveSkill.CastRange);
					if (IsTargetInDistance(m_trTarget.position, m_flAttackRange)) // 공격대상이 거리 내에 있을때.
					{
						ChangeState(EnState.Idle);
						LookAtPosition(m_trTarget.position);
						StartCoroutine(DelayConfirmUseRankActiveSkill(csRankActiveSkill));
						return true;
					}
					else
					{
						if (IsMovable())
						{
							MoveForSkill(csRankActiveSkill, EnSkillStarter.UI);
							return true;
						}
					}
				}
			}
			else
			{
				ChangeState(EnState.Idle);
				StartCoroutine(DelayConfirmUseRankActiveSkill(csRankActiveSkill));
				return true;
			}
		}

		ChangeState(EnState.Idle);
		m_csSkillStatus.ResetNextSkill();
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool PlayTransformationMonsterSkill(CsMonsterSkill csMonsterSkill)
	{
		if (csMonsterSkill == null) return false;
		if (m_csTransformationMonster.IsAttack) return false;

		if (IsTargetEnemy == false)
		{
			if (m_enTarget == EnPlayerTarget.Auto)
			{
				m_enTarget = EnPlayerTarget.Enemy;
			}
			else
			{
				SelectTarget(FindBestTarget(transform.position, transform.position, CsGameConfig.Instance.StandingBattleRange), true, EnPlayerTarget.Enemy);
			}
		}

		if (IsTargetEnemy)
		{
			m_flAttackRange = GetAttackCastRange(m_trTarget, csMonsterSkill.CastRange);
			if (IsTargetInDistance(m_trTarget.position, m_flAttackRange)) // 공격대상이 거리 내에 있을때.
			{
				LookAtPosition(m_trTarget.position);
				m_csTransformationMonster.TransformationMonsterSkillCast(csMonsterSkill);
				return true;
			}
			else
			{
				if (IsMovable())
				{
					MoveForSkill(csMonsterSkill, EnSkillStarter.UI);
					return true;
				}
			}
		}
		else
		{
			m_csTransformationMonster.TransformationMonsterSkillCast(csMonsterSkill);
			return true;
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayConfirmUseCommonSkill(CsJobCommonSkill csJobCommonSkill)
	{
		m_csSkillStatus.Init(csJobCommonSkill, true); // 공격.
		SendJobCommonSkillCast(csJobCommonSkill);
		yield return null;
		m_csSkillStatus.OnSkillAnimStarted();
		ChangeState(EnState.Idle);
		m_csSkillStatus.Reset();
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayConfirmUseRankActiveSkill(CsRankActiveSkill csRankActiveSkill)
	{
		m_csSkillStatus.Init(csRankActiveSkill, true);      // 공격. 시전동작없이 바로 Play 상태.
		SendRankActiveSkillCast();
		yield return new WaitForEndOfFrame();
		m_csSkillStatus.OnSkillAnimStarted();
		ChangeState(EnState.Idle);
		m_csSkillStatus.Reset();
	}

	//---------------------------------------------------------------------------------------------------
	bool PlayNextSkill()
	{
		if (!IsAttackable()) return false;

		if (m_enStateAttackAfter == EnState.Idle)
		{
			if (m_csSkillStatus.IsNextSkill)
			{
				if (m_csSkillStatus.NextJobSkill != null)
				{
					PlayJobSkill(m_csSkillStatus.NextJobSkill, EnSkillStarter.UI);
				}
				else if (m_csSkillStatus.NextJobCommonSkill != null)
				{
					PlayCommonSkill(m_csSkillStatus.NextJobCommonSkill);
				}
				else if (m_csSkillStatus.NextRankActiveSkill != null)
				{
					PlayRankActiveSkill(m_csSkillStatus.NextRankActiveSkill);
				}
				return true;
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool SetTargetInHitRange(float flRange)
	{
		if (IsTargetEnemy) return false;

		SelectTarget(FindBestTarget(transform.position, transform.position, flRange), true, EnPlayerTarget.Enemy);

		return (IsTargetEnemy);
	}

	//---------------------------------------------------------------------------------------------------
	void ChainSkillAbility(CsJobSkill csJobSkill)
	{
		if (!IsAttackable() || !IsTargetEnemy || csJobSkill == null) return;

		if (m_csSkillStatus.IsChainable(csJobSkill.SkillId))
		{
			if (IsTargetInDistance(m_trTarget.position, GetAttackCastRange(m_trTarget, m_csSkillStatus.JobSkill.CastRange)))
			{
				if (CheckEnemyHeroAttackAble() == false) return;

				m_csSkillStatus.Chained = true;
				CsGameEventToUI.Instance.OnEventUseAutoSkill(m_csSkillStatus.JobSkill.SkillId);  // 자동스킬 사용 UI에 전달.
				Attack();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayBattle()
	{
		if (!IsAttackable()) return;

		if (IsTransformationStateMonster())
		{
			if (m_csSkillStatus.IsStatusNo() && m_enState != EnState.MoveForSkill)
			{
				if (m_csTransformationMonster != null)
				{
					PlayTransformationMonsterSkill(m_csTransformationMonster.DefaultMonsterSkill);
				}
			}
		}
		else
		{
			if (m_csSkillStatus.IsNextSkill) return; // 수동 입력스킬이 있으면 추가 입력 제한.

			if (m_csSkillStatus.IsChainSkill())  // 연계스킬.
			{
				if (m_csSkillStatus.IsStatusAnim() && !m_csSkillStatus.Chained)
				{
					ChainSkillAbility(BattleSelectSkill());
				}
			}
			else // 일반스킬.
			{
				if (IsStateIdle && !m_csSkillStatus.IsStatusPlayAnim())
				{
					if (CheckEnemyHeroAttackAble() == false) return;

					m_csSkillStatus.ResetNextSkill();
					PlayJobSkill(BattleSelectSkill(), EnSkillStarter.AutoBattle);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayBattle(int nMonsterId, Vector3 vtPos, float flRadius, long lInstanceId = -1)
	{
		if (m_enTarget == EnPlayerTarget.Auto) // 자동타겟 검색에 의하여 타겟이 지정되어 있을때.
		{
			m_enTarget = EnPlayerTarget.Enemy;
		}

		if (!CsMonster.IsMonster(nMonsterId, TargetEnemy))
		{
			if (!m_csSkillStatus.IsStatusAnim()) // 스킬 모션이 끝나면 타겟 변경.
			{
				Transform tr = FindTarget(vtPos, transform.position, CsMonster.c_strLayer, CsMonster.c_strTag, flRadius, nMonsterId, lInstanceId);
				SelectTarget(tr, true, EnPlayerTarget.Enemy);			
			}
		}
		else
		{
			PlayBattle();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayBattle(long lInstanceId, Vector3 vtPos, float flRadius)
	{
		if (m_enTarget == EnPlayerTarget.Auto) // 자동타겟 검색에 의하여 타겟이 지정되어 있을때.
		{
			m_enTarget = EnPlayerTarget.Enemy;
		}

		if (!CsMonster.IsMonsterWithInstanceId(lInstanceId, TargetEnemy))
		{
			if (!m_csSkillStatus.IsStatusAnim()) // 스킬 모션이 끝나면 타겟 변경.
			{
				Transform tr = FindTarget(vtPos, transform.position, CsMonster.c_strLayer, CsMonster.c_strTag, flRadius, -1, lInstanceId);
				SelectTarget(tr, true, EnPlayerTarget.Enemy);
			}
		}
		else
		{
			PlayBattle();
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool IsSkillUsable(CsHeroSkill csHeroSkill)
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
	CsJobSkill BattleSelectSkill()
	{
		if (CsIngameData.Instance.PlayerAutoMode != EnPlayerAutoMode.Auto)
		{
			if (CsIngameData.Instance.AutoBattleMode == EnBattleMode.None) return null;
		}

		if (CsIngameData.Instance.AutoBattleMode == EnBattleMode.Manual)	// 자동사냥 타입이 수동스킬인 경우.
		{
			CsHeroSkill csHeroSkill = m_csMyHeroInfo.HeroSkillList.Find(a => a.JobSkill.SkillId == 1);
			if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
			{
				return csHeroSkill.JobSkill;
			}
			return null;
		}

		for (int i = 0; i < s_anAutoSkillIndex.Length; i++)
		{
			if (s_anAutoSkillIndex[i] + 1 > m_csMyHeroInfo.HeroSkillList.Count) continue;

			CsHeroSkill csHeroSkill = m_csMyHeroInfo.HeroSkillList[s_anAutoSkillIndex[i]];

			if (IsSkillUsable(csHeroSkill))
			{
				switch (csHeroSkill.JobSkill.SkillId)
				{
				case 1:
						if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
						{
							return csHeroSkill.JobSkill;
						}
						break;
				case 2:
					if (CsIngameData.Instance.AutoSkill2)
					{
						if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
						{
							return csHeroSkill.JobSkill;
						}
					}
					break;
				case 3:
					if (CsIngameData.Instance.AutoSkill3)
					{
						if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
						{
							return csHeroSkill.JobSkill;
						}
					}
					break;
				case 4:
					if (CsIngameData.Instance.AutoSkill4)
					{
						if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
						{
							return csHeroSkill.JobSkill;
						}
					}
					break;
				case 5:
					if (CsIngameData.Instance.AutoSkill5)
					{
						if (m_csMyHeroInfo.Lak == CsGameConfig.Instance.SpecialSkillMaxLak)
						{
							if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
							{
								return csHeroSkill.JobSkill;
							}
						}
					}
					break;						
				}
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	bool CheckEnemyHeroAttackAble()
	{
		if (CsIngameData.Instance.IngameManagement.IsContinent())
		{
			if (m_trTarget != null)
			{
				if (m_trTarget.CompareTag("EnemyHero") || m_trTarget.CompareTag("EnemyCart"))
				{
					if (CsGameConfig.Instance.PvpMinHeroLevel > CsIngameData.Instance.IngameManagement.GetCsMoveUnit(m_trTarget).Level) return false;
					if (CsGameConfig.Instance.PvpMinHeroLevel > Level) return false;

					CsHero csHero = CsIngameData.Instance.IngameManagement.GetHero(m_trTarget);
					if (csHero != null && csHero.Distortion) return false;
				}
			}
		}
		return true;
	}

	#endregion 09 Skill

	#region 10 Transformation

	int m_nAutoRidingWaitCount = 0;
	//---------------------------------------------------------------------------------------------------
	bool AutoRidingStart()
	{
		if (m_csMyHeroInfo.EquippedMountId == 0 || m_bBattleMode || IsTransformationStateNone() == false || CsIngameData.Instance.IngameManagement.IsContinent() == false || m_enState != EnState.MoveToPos)
		{
			m_nAutoRidingWaitCount = 0;
			return false;
		}

		if (m_nAutoRidingWaitCount > 30)
		{
			if (CsGameData.Instance.JoystickDragging || IsTargetInDistance(m_navMeshAgent.destination, 20f) == false) // 조이스틱이동 or 목적지가 20m 이상 멀리 있는경우
			{
				if (IsTransformationStateNone())
				{
					SendMountGetOn(); // 서버에 탈것타기 클라이언트 명령 전달.
					m_nAutoRidingWaitCount = 0;
					return true;
				}
			}

			m_nAutoRidingWaitCount = 0;
			return false;
		}
		m_nAutoRidingWaitCount++;
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void MountGetOn()
	{
		Debug.Log("#####     MyPlayer.MountGetOn()   #####");
		if (CsIngameData.Instance.InGameCamera.FirstEnter)
		{
			CsIngameData.Instance.InGameCamera.FirstEnter = false;
			ViewHUD(true);
		}
		ResetBattleMode();

		base.MountGetOn();

		IsRidingMount = true;
		AudioClip EnterSound = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_boarding_01");
		if (CsIngameData.Instance.EffectSound)
		{
			m_audioSource.PlayOneShot(EnterSound);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void MountGetOff(bool bSend)
	{
		Debug.Log("#####     MyPlayer.MountGetOff()   #####   bSend = "+ bSend);
		if (bSend)
		{
			SendMountGetOff(); // 서버에 탈것내리기 이벤트 전달.
		}
		base.MountGetOff(bSend);
		
		if (IsRidingMount)
		{
			IsRidingMount = false;
		}		
	}

	//---------------------------------------------------------------------------------------------------
	protected override void FlightGetOn()
	{
		Debug.Log("#####     MyPlayer.FlightGetOn()     #####");
		if (CsIngameData.Instance.InGameCamera.FirstEnter)
		{
			CsIngameData.Instance.InGameCamera.FirstEnter = false;
			ViewHUD(true);
		}
		ResetBattleMode();
		base.FlightGetOn();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void FlightGetOff()
	{
		base.FlightGetOff();
		float flOffset = m_navMeshAgent.baseOffset;
		ViewMyHero();
		m_navMeshAgent.baseOffset = flOffset;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void CartGetOn()
	{
		Debug.Log("#####     MyPlayer.CartGetOn()     #####");
		if (CsIngameData.Instance.InGameCamera.FirstEnter)
		{
			CsIngameData.Instance.InGameCamera.FirstEnter = false;
			ViewHUD(true);
		}

		if (m_rtfHUD != null)
		{
			m_rtfHUD = null;
			CsGameEventToUI.Instance.OnEventDeleteHeroHUD(HeroId);
		}

		AudioClip EnterSound = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_boarding_01");
		if (CsIngameData.Instance.EffectSound)
		{
			m_audioSource.PlayOneShot(EnterSound);
		}

		CsCartManager.Instance.IsMyHeroRidingCart = true;
		ResetBattleMode();
		base.CartGetOn();

		m_itrAutoPlay.StateChangedToIdle();
		OnChangeCartRiding();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void CartGetOff(bool bSend)
	{
		Debug.Log("#####     MyPlayer.CartGetOff()   #####  bSend = "+ bSend);
		if (bSend)
		{
			CsCartManager.Instance.SendCartGetOff(); // 서버에 탈것내리기 이벤트 전달.
		}

		CreateMyHeroHUD();
		
		if (m_csCartObject.CartQuestType == EnCartQuestType.Main)
		{
			CsMainQuestManager.Instance.SaveMainQuestCart(m_csMyHeroInfo.LocationId, m_csCartObject.InstanceId, transform.position, transform.eulerAngles.y);
		}
		else if (m_csCartObject.CartQuestType == EnCartQuestType.SupplySupport)
		{
			CsSupplySupportQuestManager.Instance.CartContinentId = m_csMyHeroInfo.LocationId;
			CsSupplySupportQuestManager.Instance.CartInstanceId = m_csCartObject.InstanceId;
			CsSupplySupportQuestManager.Instance.CartPosition = transform.position;
			CsSupplySupportQuestManager.Instance.CartRotationY = transform.eulerAngles.y;
		}
		else if (m_csCartObject.CartQuestType == EnCartQuestType.GuildSupplySupport)
		{
			CsGuildManager.Instance.GuildSupplySupportQuestPlay = new CsGuildSupplySupportQuestPlay(m_csCartObject.InstanceId, 
																									m_csMyHeroInfo.LocationId, 
																									transform.position, 
																									transform.eulerAngles.y,
																									CsGuildManager.Instance.GuildSupplySupportQuestRemainingTime);
		}

		base.CartGetOff(bSend);
		CsCartManager.Instance.IsMyHeroRidingCart = false;
		OnChangeCartRiding();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void TransformationMonsterGetOn(CsMonsterInfo csMonsterInfo)
	{
		ResetBattleMode();
		if (m_csSkillStatus.IsStatusAnim())
		{
			Debug.Log("TransformationMonsterGetOn        OnAttckEnd()");
			m_csSkillStatus.ResetNextSkill();
			OnAttackEnd();
		}

		CsGameEventToUI.Instance.OnEventJoystickReset();
		base.TransformationMonsterGetOn(csMonsterInfo);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void TransformationMonsterGetOff()
	{
		base.TransformationMonsterGetOff();
		ChangeState(EnState.Idle);
		ViewMyHero();
	}

	#endregion 10 Transformation

	#region 11 Setting
	//---------------------------------------------------------------------------------------------------
	void CreateMyHeroHUD()
	{
		Debug.Log("CsMyPlayer.CreateMyHeroHUD()");
		if (m_bDistortion == false)
		{
			m_bDistortion = (m_csMyHeroInfo.RemainingDistortionTime - Time.realtimeSinceStartup) > 0 ? true : false;
		}

		int nNoblesseId = m_csMyHeroInfo.GetMyNationNoblesseInstance(HeroId) == null ? 0 : m_csMyHeroInfo.GetMyNationNoblesseInstance(HeroId).NoblesseId;

		int nMyGuildMemberGrade = 0;
		if (CsGuildManager.Instance.MyGuildMemberGrade != null)
		{
			nMyGuildMemberGrade = CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade;
		}

		m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateHeroHUD(m_csMyHeroInfo,
																 CsGuildManager.Instance.GuildName,
																 nMyGuildMemberGrade,
																 CsSecretLetterQuestManager.Instance.PickedLetterGrade,
																 CsMysteryBoxQuestManager.Instance.PickedBoxGrade,
																 m_bDistortion,
																 false,
																 m_enNationWarPlayerState,
																 nNoblesseId,
																 CsTitleManager.Instance.DisplayTitleId);
	}

	//---------------------------------------------------------------------------------------------------
	public void MyHeroView(bool bView)
	{
		if (m_bView != bView)
		{
			m_bView = bView;
			if (m_bView)
			{
				ViewMyHero();
			}
			else
			{
				HideMyHero();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ViewMyHero()
	{
		Debug.Log("CsMyPlayer.ViewHero        m_rtfHUD = " + m_rtfHUD);
		ViewHUD(true);
		m_csEquipment.ViewEquipments();

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
	}

	//---------------------------------------------------------------------------------------------------
	void HideMyHero()
{	
		Debug.Log("CsMyPlayer.HideHero");

		if (IsTransformationStateMount())
		{
			m_csMountObject.gameObject.SetActive(false);
		}
		else if (IsTransformationStateCart())
		{
			m_csCartObject.RidindCart.gameObject.SetActive(false);
		}

		m_csEquipment.HideEquipments();
		Destroy(m_goShadow);
		m_goShadow = null;
		ViewHUD(false);
	}

	//---------------------------------------------------------------------------------------------------
	void ViewHUD(bool bView)
	{
		if (m_rtfHUD != null)
		{
			m_rtfHUD.gameObject.SetActive(bView);
		}
	}

	bool m_bDungeonClear = false;
	//---------------------------------------------------------------------------------------------------
	public void DungeonClear()
	{
		Debug.Log("DungeonClear()");
		m_bDungeonClear = true;
		SetAnimStatus(EnAnimStatus.Clear);
	}

	//---------------------------------------------------------------------------------------------------
	public override void AttackByHit(PDHitResult HitResult, float flHitTime, PDHitResult AddHitResult, bool bMyHero, bool bKnockback)
	{
		SetDamageText(HitResult, true);
		ChangedAbnormalStateEffectDamageAbsorbShields(HitResult.changedAbnormalStateEffectDamageAbsorbShields);

		if (AddHitResult != null)
		{
			SetDamageText(AddHitResult, true);
		}
		
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

			CsGameEventToUI.Instance.OnEventMyHeroInfoUpdate();
		}

		if (Hp == 0)
		{			
			MyHeroDead();
		}
	} 

	//---------------------------------------------------------------------------------------------------
	protected override bool HeroAbnormalEffectHit(int nHp, Transform trAttacker, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects)
	{
		if (CsIngameData.Instance.InGameCamera.SleepMode == false)
		{
			Vector3 vtPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
			CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nDamage, Camera.main.WorldToScreenPoint(vtPos), true);  // 데미지 Text 처리.
		}

		m_flHpChangeTime = Time.time;
		Hp = nHp;
		CsGameEventToUI.Instance.OnEventMyHeroInfoUpdate();

		return base.HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	protected override bool HeroTrapHit(int nHp, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects, bool bSlowMoveSpeed, int nTrapPenaltyMoveSpeed)
	{
		if (CsIngameData.Instance.InGameCamera.SleepMode == false)
		{
			Vector3 vtPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
			CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nDamage, Camera.main.WorldToScreenPoint(vtPos), true);  // 데미지 Text 처리.
		}

		Hp = nHp;
		m_flHpChangeTime = Time.time;
		CsGameEventToUI.Instance.OnEventMyHeroInfoUpdate();

		return base.HeroTrapHit(nHp, nDamage, nHpDamage, alRemovedAbnormalStateEffects, bSlowMoveSpeed, nTrapPenaltyMoveSpeed);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void DamageByAbnormal(bool bSkillCancel)
	{
		Debug.Log("1. DamageByAbnormal     >>          bSkillCancel = " + bSkillCancel);
		m_vtSkillMovePos = transform.position;
		ChangeState(EnState.Idle);

		if (bSkillCancel)
		{
			m_navMeshAgent.updateRotation = true;
			ResetPathOfNavMeshAgent();
			m_csSkillStatus.Reset();
			m_flAttackRange = 0f;
			m_csSkillStatus.ResetNextSkill();
			ChangeState(EnState.Damage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void MyHeroDead()
	{
		ResetBattleMode();

		if (Dead == false)
		{
			Debug.Log("############                    MyHeroDead()                  ##################");
			m_csSkillStatus.ResetNextSkill();
			m_csSkillStatus.Reset();	

			ChangeTransformationState(EnTransformationState.None, true);
			ResetTarget();
			ResetBattleMode();
			SetAutoPlay(null, true);

			Hp = 0;
			m_flHpChangeTime = Time.time;
			CsGameEventToUI.Instance.OnEventMyHeroInfoUpdate();
			Dead = true;

			Invoke(DelayDead, 0.2f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void DeadSetting(bool bDead, bool bEffect = true)
	{
		Dead = bDead;
		if (CsIngameData.Instance.InGameCamera != null)
		{
			CsIngameData.Instance.InGameCamera.ChangeTexture(Dead);
		}

		m_csSkillStatus.ResetNextSkill();
		m_csSkillStatus.Reset();

		if (bDead)
		{
			ChangeState(EnState.Dead);
		}
		else
		{
			if (bEffect)
			{
				HeroReviveEffect();
			}
			ChangeState(EnState.Idle);
		}
	}

	Vector3 m_vtPortalPos = Vector3.zero;	//인수 수정 필요.
	//---------------------------------------------------------------------------------------------------
	public Vector3 ChaseContinent(int nNation, int nContinentId)
	{
		CsNpcInfo csNationTransmissionNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
		CsNpcInfo csContinentTransmissionNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.ContinentTransmission);
		
		if (m_csMyHeroInfo.InitEntranceLocationParam != nNation) // 현재 위치한 국가와 이동할 국가가 다를때.
		{
			if (m_csMyHeroInfo.LocationId == csNationTransmissionNpcInfo.ContinentId)
			{
				m_bTargetNpc = true;
				return csNationTransmissionNpcInfo.Position;
			}
			else
			{
				if (m_csMyHeroInfo.InitEntranceLocationParam == m_csMyHeroInfo.Nation.NationId)		// 본인 국가일때.
				{
					if (m_csMyHeroInfo.LocationId == csContinentTransmissionNpcInfo.ContinentId)	// 현재 위치가 대륙이동 Npc가 있는 대륙인 경우.
					{
						if (IsTransformationStateCart() == false)									// 카트 탑승중이 아닐때.
						{
							m_bTargetNpc = true;
							return csContinentTransmissionNpcInfo.Position;
						}
					}
				}

				nContinentId = csNationTransmissionNpcInfo.ContinentId;
			}
		}

		List<int> listContinentId = new List<int>();
		listContinentId.Add(nContinentId);

		if (m_csMyHeroInfo.LocationId == 6 && nContinentId == 2)							// 6번 대륙에서 2번 대륙으로 이동시 2개의 포탈중 가까운 포탈로 이동.
		{
			CsPortal csPortal62 = CsGameData.Instance.GetPortal(62);
			CsPortal csPortal64 = CsGameData.Instance.GetPortal(64);

			if (csPortal62 != null && csPortal64 != null)
			{
				if (GetDistanceFormTarget(csPortal62.Position) > GetDistanceFormTarget(csPortal64.Position))
				{
					return csPortal64.Position;
				}
				else
				{
					return csPortal62.Position;
				}
			}
		}
		else if (m_csMyHeroInfo.LocationId == 2)											// 2번 대륙인 경우
		{
			if (m_csMyHeroInfo.InitEntranceLocationParam == m_csMyHeroInfo.Nation.NationId)	// 본인 국가일때.
			{
				if (nContinentId == csNationTransmissionNpcInfo.ContinentId)                // 목표 대륙이 국가이동 Npc가 있는 대륙인 경우.
				{
					if (IsTransformationStateCart() == false)                                   // 카트 탑승중이 아닐때.
					{
						m_bTargetNpc = true;
						return csContinentTransmissionNpcInfo.Position;
					}
				}
			}

			CsPortal csPortal21 = CsGameData.Instance.GetPortal(21);
			CsPortal csPortal22 = CsGameData.Instance.GetPortal(22);

			if (csPortal21 != null && csPortal22 != null)
			{
				if (nContinentId == 6)														// 목표 대륙이 6번인 경우.
				{
					if (GetDistanceFormTarget(csPortal21.Position) > GetDistanceFormTarget(csPortal22.Position))
					{
						return csPortal22.Position;
					}
					else
					{
						return csPortal21.Position;
					}
				}
				else
				{
					if (nContinentId == 1)  // 목표 대륙이 1번인 경우.
					{
						return csPortal21.Position;
					}
					else
					{
						return csPortal22.Position;
					}
				}
			}
		}

		listContinentId = GetDirection(listContinentId);							// 1. 다음 대륙에 존재 여부 확인.
		if (listContinentId != null)
		{
			listContinentId = GetDirection(listContinentId);						// 2. 다음 다음 대륙에 존재 여부 확인.
			if (listContinentId != null)
			{
				listContinentId = GetDirection(listContinentId);					// 3. 다음 다음 다음 대륙에 존재 여부 확인.
				if (listContinentId != null)
				{
					listContinentId = GetDirection(listContinentId);				// 4. 다음 다음 다음 다음 대륙에 존재 여부 확인.
					if (listContinentId != null)
					{
						listContinentId = GetDirection(listContinentId);            // 5. 다음 다음 다음 다음 다음 대륙에 존재 여부 확인.
						if (listContinentId != null)
						{
							listContinentId = GetDirection(listContinentId);        // 6. 다음 다음 다음 다음 다음 다음 대륙에 존재 여부 확인.
						}
					}
				}
			}
		}

		if (listContinentId != null)
		{
			Debug.Log("ChaseContinent     길을 찾을수가 없습니다 추가 길찾기 필요.");
		}

		return m_vtPortalPos;
	}

	//---------------------------------------------------------------------------------------------------
	List<int> GetDirection(List<int> listContinentId)
	{
		List<int> listNextContinentId = new List<int>();

		for (int i = 0; i < listContinentId.Count; i++)
		{
			List<CsPortal> listPortal = CsGameData.Instance.GetPortalList(listContinentId[i]); // 1. 목적 대륙에 존재하는 포탈을 리스트에 저장.
			for (int j = 0; j < listPortal.Count; j++)
			{
				CsPortal csPortal = CsGameData.Instance.GetPortal(listPortal[j].LinkedPortalId); // 2. 목적 포탈에 링크로 연결된 다른 대륙에 위치해 있는 포탈 정보.
				if (csPortal == null) continue;

				if (csPortal.ContinentId == m_csMyHeroInfo.LocationId) // 3. 링크로 연결된 포탈이 위치한 대륙과 현재 내 대륙과 같은지 확인.
				{
					m_vtPortalPos = csPortal.Position;
					//MoveToPos(csPortal.Position, 0.5f, false); // 3-1. 위치가 같으면 저장.
					return null;
				}
				else
				{
					if (!listContinentId.Contains(csPortal.ContinentId) && !listNextContinentId.Contains(csPortal.ContinentId))
					{
						listNextContinentId.Add(csPortal.ContinentId); // 3-2. 위치가 다르면 다음 체크 포탈 리스트에 저장.
					}
				}
			}
		}
		
		return listNextContinentId;
	}

	//---------------------------------------------------------------------------------------------------
	void SelectTarget(Transform tr, bool bSelect, EnPlayerTarget enTarget = EnPlayerTarget.Normal)
	{
		ResetTarget();

		if (tr != null)
		{
			if (tr.CompareTag("Hero") || tr.CompareTag("EnemyHero"))
			{
				CsGameEventToUI.Instance.OnEventSelectHeroInfo(CsIngameData.Instance.IngameManagement.GetHeroId(tr));
				CsIngameData.Instance.IngameManagement.SelectHero(tr, true);

			}
			else if (tr.CompareTag(CsMonster.c_strTag))
			{
				CsGameEventToUI.Instance.OnEventSelectMonsterInfo(CsIngameData.Instance.IngameManagement.GetInstanceId(tr.transform));
			}
			else if (tr.CompareTag("Cart") || tr.CompareTag("EnemyCart"))
			{
				CsGameEventToUI.Instance.OnEventSelectCartInfo(CsIngameData.Instance.IngameManagement.GetInstanceId(tr.transform));
			}
		}
		else
		{
			if (CsIngameData.Instance.TargetTransform != null)
			{
				if (CsIngameData.Instance.TargetTransform.CompareTag("Hero") || CsIngameData.Instance.TargetTransform.CompareTag("EnemyHero"))
				{
					CsIngameData.Instance.IngameManagement.SelectHero(CsIngameData.Instance.TargetTransform, false);
				}
			}
		}

		if (m_goSelectMark == null) // 없을시 재 생성.
		{
			m_goSelectMark = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/TargetRing")) as GameObject;
			m_goSelectMark.SetActive(false);
		}

		if (tr != null && bSelect)
		{
			m_goSelectMark.transform.SetParent(tr);
			m_goSelectMark.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			m_goSelectMark.SetActive(true);
		}
		else
		{
			m_goSelectMark.transform.SetParent(transform);
			m_goSelectMark.SetActive(false);
		}

		m_trTarget = tr;
		CsIngameData.Instance.TargetTransform = tr;
		m_enTarget = enTarget;
	}

	//---------------------------------------------------------------------------------------------------
	public void SelectTarget(Transform tr, bool bSelect)
	{
		SelectTarget(tr, bSelect, EnPlayerTarget.Normal);
	}

	//---------------------------------------------------------------------------------------------------
	public void SelectTargetEnemy(Transform trTarget, bool bSelect)
	{
		SelectTarget(trTarget, bSelect, EnPlayerTarget.Enemy);
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetTarget()
	{
		//Debug.Log("ResetTarget()");
		if (m_goSelectMark != null)
		{
			m_goSelectMark.SetActive(false);
			m_goSelectMark.transform.parent = transform;
		}

		if (m_trTarget != null)
		{
			if (m_trTarget.CompareTag("Hero") || m_trTarget.CompareTag("EnemyHero"))
			{
				CsGameEventToUI.Instance.OnEventSelectHeroInfoStop();
			}
			else if (m_trTarget.CompareTag(CsMonster.c_strTag))
			{
				CsGameEventToUI.Instance.OnEventSelectMonsterInfoStop();
			}
			else if (m_trTarget.CompareTag("Cart") || m_trTarget.CompareTag("EnemyCart"))
			{
				CsGameEventToUI.Instance.OnEventSelectCartInfoStop();
			}
		}

		m_enTarget = EnPlayerTarget.Normal;
		m_trTarget = null;
		CsIngameData.Instance.TargetTransform = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void TargetEnemyDead()
	{
		ResetTarget();
	}

	//---------------------------------------------------------------------------------------------------
	void OverDistanceResetTarget()
	{
		if (m_trTarget == null) return;

		if (m_csSkillStatus.IsStatusNo())
		{
			if (CsIngameData.Instance.IngameManagement.IsContinent())
			{
				if (IsTargetInDistance(m_trTarget.position, 20f)) return;
			}
			else
			{
				if (IsTargetInDistance(m_trTarget.position, 40f)) return;
			}

			ResetTarget();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool ChangeTargetAutoToEnemy()
	{
		if (m_enTarget == EnPlayerTarget.Auto)
		{
			m_enTarget = EnPlayerTarget.Enemy;
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool NavMeshDirect(Vector3 vtPos)
	{
		return (NavMeshCornerCount(vtPos) == 2 || IsTargetInDistance(vtPos, 1f));
	}

	//---------------------------------------------------------------------------------------------------
	bool IsNavMeshCornerCountIn(Vector3 vtPos, int nCount)
	{
		return (NavMeshCornerCount(vtPos) <= nCount);
	}

	//---------------------------------------------------------------------------------------------------
	public int NavMeshCornerCount(Vector3 vtPos)
	{
		if (m_navMeshAgent.isActiveAndEnabled)
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			if (m_navMeshAgent.CalculatePath(vtPos, navMeshPath))
			{
				return navMeshPath.corners.Length;
			}
		}
		return 0;
	}

	#endregion 11 Setting

	//---------------------------------------------------------------------------------------------------
	public void TrasfomationAttackStart()
	{
		ResetPathOfNavMeshAgent();
		if (IsTransformationStateMonster())
		{
			ChangeState(EnState.Attack);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void TrasfomationAttackEnd()
	{
		ResetPathOfNavMeshAgent();
		m_navMeshAgent.updateRotation = true;
		m_csSkillStatus.ResetNextSkill();
		m_csSkillStatus.Reset();
		ChangeState(EnState.Idle);
	}
}
