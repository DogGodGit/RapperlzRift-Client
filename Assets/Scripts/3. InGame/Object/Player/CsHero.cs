using ClientCommon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class CsHero : CsMoveUnit
{
	public enum EnState { Idle = 0, MoveByJoystic, MoveByTouch, MoveForSkill, MoveToPos, Attack, Damage, Interaction, ReturnScroll, Dead, Fishing, MoveByDirecting, ItemSteal }
	public enum EnSkillStarter { UI, AutoBattle, Net, Clone }
	public enum EnSkillType { None, Hero, Common, Rank , Transformation }
	public enum EnAnimStatus { Idle = 0, Walk, Run, Skill01_01, Skill01_02, Skill01_03, Skill02, Skill03, Skill04, Skill05, Interact, Dead, Riding, Fishing, Taming, CommonSkill, Clear }
	public enum EnTransformationState { None = 0, Mount, Cart, Tame, Flight, Monster } // 기본상태, 탈것탑승, 운송, 조종, 비행

	public const float c_flStopRange = 0.3f;
	public const float c_flAttackRotationSpeed = 720.0f;

	protected static int s_nAnimatorHash_status = Animator.StringToHash("status");
	protected static int s_nAnimatorHash_damage = Animator.StringToHash("damage");
	protected static int s_nAnimatorHash_dead = Animator.StringToHash("dead");

	protected static int s_nAnimatorHash_battle = Animator.StringToHash("battle");
	protected static int s_nAnimatorHash_changebattle = Animator.StringToHash("changebattle");
	protected static int s_nAnimatorHash_job = Animator.StringToHash("job");

	protected static int s_nAnimatorHash_move = Animator.StringToHash("move");
	protected static int s_nAnimatorHash_enterrun = Animator.StringToHash("enterrun");
	protected static int s_nAnimatorHash_acceleration = Animator.StringToHash("acceleration");
	protected static int s_nAnimatorHash_dashmotion = Animator.StringToHash("dashmotion");
	protected static int s_nAnimatorHash_flight = Animator.StringToHash("flight");
	protected static int s_nAnimatorHash_ridingstatus = Animator.StringToHash("ridingstatus");

	protected CapsuleCollider m_capsuleCollider;
	protected AudioSource m_audioSource;
	protected AudioSource m_audioSourceParent;
	protected CsEquipment m_csEquipment;

	protected Vector3 m_vtPrevPos;
	protected Vector3 m_vtMovePos;
	protected Vector3 m_vtSkillMovePos;
	protected Vector3 m_vtTargetPos;

	protected Guid m_guidPlaceInstanceId;
	protected CsMyHeroInfo m_csMyHeroInfo = null;
	protected CsHeroBase m_csHeroBase = null;
	protected CsHeroCustomData m_csHeroCustomData = null;

	protected int m_nCreatureId;
	protected CsCreatureObject m_csCreatureObject;

	protected CsMount m_csMount = null;
	protected CsMountObject m_csMountObject = null;
	protected int m_nMountId;
	protected int m_nMountLevel;
	protected bool m_bRidingMount;
	protected bool m_bDistortion = false;
	protected bool m_bWalk = false;
	protected float m_flJoysticWalk = 0;

	protected GameObject m_goWing = null;
	protected CsCartObject m_csCartObject = null;
	protected bool m_bCartAccelerate = false;       // 가속상태.

	//protected int m_nDamageAbsorbShieldRemainingAbsorbAmount = 0;
	protected float m_flMoveStopRange = 0.3f;
	protected float m_flNpcStopRange = 0f;
	protected float m_flRotationY;
	protected float m_flSkillMoveSpeed = 0f;
	protected float m_flAccelerationValue = 0f;

	[SerializeField]
	protected bool m_bView = true;
	[SerializeField]
	protected bool m_bSafeMode = false;
	[SerializeField]
	protected float m_flTrapMoveSpeed = 0f;
	[SerializeField]
	protected bool m_bAcceleration = false;

	protected RectTransform m_rtfHUD = null;
	protected Transform m_trPivotHUD;
	protected float m_flHeroHeightOffset;

	protected int m_nMoveStopWaitCount = 0;
	protected bool m_bMoveHasPath;
	protected bool m_bInvincibility = false;        // 무적상태.  (상태만있고 관련 이팩트 없음)
	protected bool m_bBattleMode = false;
	protected float m_flHpChangeTime;               // HP증가된 시간.

	public EnState m_enState = EnState.Idle;
	public EnTransformationState m_enTransformationState = EnTransformationState.None;

	//---------------------------------------------------------------------------------------------------
	public Guid PlaceInstanceId { get { return m_guidPlaceInstanceId; } set { m_guidPlaceInstanceId = value; } }
	public CsCartObject CartObject { get { return m_csCartObject; } set { m_csCartObject = value; } }
	public bool BattleMode { get { return m_bBattleMode; } set { m_bBattleMode = value; } }
	public virtual bool IsRidingMount { get { return m_bRidingMount; } set { m_bRidingMount = value; } }
	public bool Distortion { get { return m_bDistortion; } set { m_bDistortion = value; } }
	public bool View { get { return m_bView; } set { m_bView = value; } }

	public EnState State { get { return m_enState; } set { m_enState = value; } }
	public EnTransformationState TransformationState { get { return m_enTransformationState; } }
	public float MoveStopRange { get { return m_flMoveStopRange; } set { m_flMoveStopRange = value; } }
	public Vector3 MovePos { get { return m_vtMovePos; } }
	public bool IsStateIdle { get { return m_enState == EnState.Idle; } }
	public bool IsStateAttack { get { return m_enState == EnState.Attack; } }
	public NavMeshAgent MyHeroNavMeshAgent { get { return m_navMeshAgent; } set { m_navMeshAgent = value; } }
	public Transform HeroPivot { get { return m_trPivotHUD; } }

	public virtual bool IsMyHero { get { return false; } }
	public virtual Guid HeroId { get { return m_csHeroBase.HeroId; } }
	public virtual CsJob Job { get { return m_csHeroBase.Job; } }
	public virtual int NationId { get { return m_csHeroBase.Nation.NationId; } }
	public virtual int Rank { get { return m_csHeroBase.RankNo; } }
	public Vector3 TargetPos { get { return m_vtTargetPos; } }

	//---------------------------------------------------------------------------------------------------
	public class CsSkillStatus
	{
		protected enum EnStatus { No, Ready, Play, Anim }

		CsJobSkill m_csJobSkill, m_csNextJobSkill;
		CsJobCommonSkill m_csJobCommonSkill, m_csNextJobCommonSkill;
		CsRankActiveSkill m_csRankActiveSkill, m_csNextRankActiveSkill;
		CsMonsterSkill m_csMonsterSkill, m_csNextMonsterSkill;
		int m_nChainSkillIndex;
		bool m_bChained;

		EnSkillStarter m_enSkillStarter;
		EnStatus m_enStatus = EnStatus.No;
		EnSkillType m_enSkillType = EnSkillType.None;

		public void Display()
		{
			//Debug.Log("CsSkillStatus.m_enSkillType : " m_enSkillType);
			//Debug.Log("CsSkillStatus.m_enStatus : " + m_enStatus);
			//Debug.Log("CsSkillStatus.m_nChainSkillIndex : " + m_nChainSkillIndex);
			//Debug.Log("CsSkillStatus.m_bChained : " + m_bChained);
			//Debug.Log("CsSkillStatus.m_enSkillStarter : " + m_enSkillStarter);
		}

		public void Init(CsJobSkill csJobSkill, bool bPlay, EnSkillStarter enSkillStarter, int nChainSkill = 0) // 영웅스킬
		{
			Reset();
			ClearNextSkill();
			m_enStatus = (bPlay == true) ? EnStatus.Play : EnStatus.Ready;

			m_csJobSkill = csJobSkill;
			m_nChainSkillIndex = nChainSkill;

			if (nChainSkill > 0)
			{
				if (enSkillStarter != EnSkillStarter.Net || nChainSkill >= m_csJobSkill.JobChainSkillList.Count)
				{
					m_nChainSkillIndex = 0;
				}
			}

			m_enSkillStarter = enSkillStarter;
			m_enSkillType = EnSkillType.Hero;
			Display();
		}

		public void Init(CsJobCommonSkill csJobCommonSkill, bool bPlay)	// 공용스킬
		{
			Reset();
			ClearNextSkill();
			m_enStatus = (bPlay == true) ? EnStatus.Play : EnStatus.Ready;

			m_csJobCommonSkill = csJobCommonSkill;
			m_nChainSkillIndex = 0;

			m_enSkillStarter = EnSkillStarter.UI;
			m_enSkillType = EnSkillType.Common;
			Display();
		}

		public void Init(CsRankActiveSkill csRankActiveSkill, bool bPlay)	// 직업스킬
		{
			Reset();
			ClearNextSkill();
			m_enStatus = (bPlay == true) ? EnStatus.Play : EnStatus.Ready;

			m_csRankActiveSkill = csRankActiveSkill;
			m_nChainSkillIndex = 0;

			m_enSkillStarter = EnSkillStarter.UI;
			m_enSkillType = EnSkillType.Rank;
			Display();
		}

		public void Init(CsMonsterSkill csMonsterSkill, bool bPlay)
		{
			Reset();
			ClearNextSkill();
			m_enStatus = (bPlay == true) ? EnStatus.Play : EnStatus.Ready;

			m_csMonsterSkill = csMonsterSkill;
			m_nChainSkillIndex = 0;

			m_enSkillStarter = EnSkillStarter.UI;
			m_enSkillType = EnSkillType.Transformation;
			Display();
		}

		public void Reset()
		{
			//Debug.Log("CsSkillStatus.Reset()");
			if (m_bChained || m_enStatus == EnStatus.Ready)
			{
				switch (m_enSkillType)
				{
				case EnSkillType.Hero:
					ConfirmUseSkill(false, m_csJobSkill.SkillId);
					break;
				case EnSkillType.Common:
					ConfirmUseCommonSkill(false, m_csJobCommonSkill.SkillId);
					break;
				case EnSkillType.Rank:
					ConfirmUseRankActiveSkill(false, m_csRankActiveSkill.SkillId);
					break;
				case EnSkillType.Transformation:
					ConfirmUseTransformationSkill(false, m_csMonsterSkill.SkillId);
					break;
				}
			}

			m_csJobSkill = null;
			m_csJobCommonSkill = null;
			m_csRankActiveSkill = null;
			m_csMonsterSkill = null;

			m_nChainSkillIndex = 0;
			m_enStatus = EnStatus.No;
			m_enSkillStarter = EnSkillStarter.UI;
			m_enSkillType = EnSkillType.None;
			m_bChained = false;
		}

		public void SetNextJobSkill(CsJobSkill csJobSkill)
		{
			m_bChained = false;
			ResetNextSkill();
			m_csNextJobSkill = csJobSkill;
		}

		public void SetNextCommonSkill(CsJobCommonSkill csJobCommonSkill)
		{
			m_bChained = false;
			ResetNextSkill();
			m_csNextJobCommonSkill = csJobCommonSkill;
		}

		public void SetNextRankActiveSkill(CsRankActiveSkill csRankActiveSkill)
		{
			m_bChained = false;
			ResetNextSkill();
			m_csNextRankActiveSkill = csRankActiveSkill;
		}

		public void SetNextTransformationSkill(CsMonsterSkill csMonsterSkill)
		{
			m_bChained = false;
			ResetNextSkill();
			m_csNextMonsterSkill = csMonsterSkill;
		}

		public void ResetNextSkill()
		{
			if (m_csNextJobSkill != null)
			{
				ConfirmUseSkill(false, m_csNextJobSkill.SkillId);
				m_csNextJobSkill = null;
			}

			if (m_csNextJobCommonSkill != null)
			{
				ConfirmUseCommonSkill(false, m_csNextJobCommonSkill.SkillId);
				m_csNextJobCommonSkill = null;
			}

			if (m_csNextRankActiveSkill != null)
			{
				ConfirmUseRankActiveSkill(false, m_csNextRankActiveSkill.SkillId);
				m_csNextRankActiveSkill = null;
			}

			if (m_csNextMonsterSkill != null)
			{
				ConfirmUseTransformationSkill(false, m_csNextMonsterSkill.SkillId);
				m_csNextMonsterSkill = null;
			}
		}

		void ClearNextSkill()
		{
			m_csNextJobSkill = null;
			m_csNextJobCommonSkill = null;
			m_csNextRankActiveSkill = null;
			m_csNextMonsterSkill = null;
		}

		public void OnSkillAnimStarted()
		{
			if (m_bChained) // 연계스킬중일때는 상태가 Anim 이여야함.
			{
				if (m_enStatus != EnStatus.Anim)
				{
					Debug.Log("OnSkillAnimStarted   확인용");
					return;
				}
			}
			else            // 연계스킬 외의 상태는 Play 상태로 시작 호출이 되어야 함.
			{
				if (m_enStatus != EnStatus.Play)
				{
					Debug.Log("OnSkillAnimStarted   확인용");
					return;
				}
			}

			m_enStatus = EnStatus.Anim;

			if (m_enSkillStarter == EnSkillStarter.UI || m_enSkillStarter == EnSkillStarter.AutoBattle) // 오토 중에도 연속기 사용.
			{
				switch (m_enSkillType)
				{
				case EnSkillType.Hero:
					ConfirmUseSkill(true, m_csJobSkill.SkillId);
					if (m_bChained)
					{
						MoveNextChainSkill();
					}
					break;
				case EnSkillType.Common:
					ConfirmUseCommonSkill(true, m_csJobCommonSkill.SkillId);
					break;
				case EnSkillType.Rank:
					ConfirmUseRankActiveSkill(true, m_csRankActiveSkill.SkillId);
					break;
				case EnSkillType.Transformation:
					ConfirmUseTransformationSkill(true, m_csMonsterSkill.SkillId);
					break;
				}
			}
		}

		void ConfirmUseSkill(bool bReturn, int nSkillId)
		{
			CsGameEventToUI.Instance.OnEventConfirmUseSkill(bReturn, nSkillId);
		}

		void ConfirmUseCommonSkill(bool bReturn, int nSkillId)
		{
			CsGameEventToUI.Instance.OnEventConfirmUseCommonSkill(bReturn, nSkillId);
		}

		void ConfirmUseRankActiveSkill(bool bReturn, int nSkillId)
		{
			CsGameEventToUI.Instance.OnEventConfirmUseRankActiveSkill(bReturn, nSkillId);
		}

		void ConfirmUseTransformationSkill(bool bReturn, int nSkillId)
		{
			CsGameEventToUI.Instance.OnEventConfirmUseTransformationSkill(bReturn, nSkillId);
		}

		public CsJobSkill JobSkill
		{
			get { return m_csJobSkill; }
			set { m_csJobSkill = value; }
		}

		public bool IsNextSkill
		{
			get { return (m_csNextJobSkill != null || m_csNextJobCommonSkill != null || m_csNextRankActiveSkill != null); }
		}

		public CsJobCommonSkill JobCommonSkill
		{
			get { return m_csJobCommonSkill; }
		}

		public CsRankActiveSkill RankActiveSkill
		{
			get { return m_csRankActiveSkill; }
		}

		public CsMonsterSkill TransformationMonsterSkill
		{
			get { return m_csMonsterSkill; }
		}

		public CsJobSkill NextJobSkill
		{
			get { return m_csNextJobSkill; }
		}

		public CsJobCommonSkill NextJobCommonSkill
		{
			get { return m_csNextJobCommonSkill; }
		}

		public CsRankActiveSkill NextRankActiveSkill
		{
			get { return m_csNextRankActiveSkill; }
		}

		public CsMonsterSkill NextTransformationMonsterSkill
		{
			get { return m_csNextMonsterSkill; }
		}

		public bool Chained
		{
			get { return m_bChained; }
			set { m_bChained = value; }
		}

		public int ChainSkillIndex
		{
			get { return m_nChainSkillIndex; }
		}

		public void MoveNextChainSkill()
		{
			m_nChainSkillIndex++;
			m_bChained = false;
		}

		public bool IsChainable(int nJobSkillId)
		{
			return (IsStatusPlayAnim() && IsChainSkill() && m_csJobSkill.SkillId == nJobSkillId
					&& m_bChained == false && m_nChainSkillIndex + 1 < m_csJobSkill.JobChainSkillList.Count);
		}

		public bool IsCurrentChainable()
		{
			return (IsStatusPlayAnim() && IsChainSkill()
					&& m_bChained == false && m_nChainSkillIndex + 1 < m_csJobSkill.JobChainSkillList.Count);
		}

		public void SetStatusToPlay()
		{
			if (m_enStatus != EnStatus.Ready) return;
			m_enStatus = EnStatus.Play;
		}

		public void SetStatusToNo()
		{
			if (m_enStatus != EnStatus.Play) return;
			m_enStatus = EnStatus.No;
		}

		public bool IsStatusNo()
		{
			return (m_enStatus == EnStatus.No);
		}

		public bool IsStatusReady()
		{
			return (m_enStatus == EnStatus.Ready);
		}

		public bool IsStatusPlay()
		{
			return (m_enStatus == EnStatus.Play);
		}

		public bool IsStatusAnim()
		{
			return (m_enStatus == EnStatus.Anim);
		}

		public bool IsStatusPlayAnim()
		{
			return (m_enStatus >= EnStatus.Play);
		}

		public bool IsSkillTypeNone()
		{
			return m_enSkillType == EnSkillType.None;
		}

		public bool IsSkillTypeHero()
		{
			return m_enSkillType == EnSkillType.Hero;
		}

		public bool IsSkillTypeCommon()
		{
			return m_enSkillType == EnSkillType.Common;
		}

		public bool IsSkillTypeRank()
		{
			return m_enSkillType == EnSkillType.Rank;
		}

		public bool IsSkillTypeTransformation()
		{
			return m_enSkillType == EnSkillType.Transformation;
		}

		public EnSkillStarter SkillStarter()
		{
			return m_enSkillStarter;
		}

		public bool ChangeStatusAnim()
		{
			m_enStatus = EnStatus.Anim;
			return (m_enStatus == EnStatus.Anim);
		}

		public bool IsCastingMoveTypeManual()
		{
			return (m_enSkillType == EnSkillType.Hero && m_csJobSkill != null && m_csJobSkill.CastingMoveTypeEnum == EnCastingMoveType.Manual);
		}

		public bool IsStatusAnimCastingMoveTypeFixed()
		{
			return (m_enSkillType == EnSkillType.Hero && m_csJobSkill != null && IsStatusAnim() && m_csJobSkill.CastingMoveTypeEnum == EnCastingMoveType.Fixed);
		}

		public bool IsStatusPlayAnimWithNoMove()
		{
			return (IsStatusPlayAnim() && !IsCastingMoveTypeManual());
		}

		public bool IsStatusAnimWithNoMove()
		{
			return (IsStatusAnim() && !IsCastingMoveTypeManual());
		}

		public int GetChainSkillId()
		{
			if (m_enSkillType == EnSkillType.Hero && m_csJobSkill != null && IsChainSkill())
			{
				return m_csJobSkill.JobChainSkillList[m_nChainSkillIndex].ChainSkillId;
			}
			return 0;
		}

		public CsJobChainSkill GetChainSkill()
		{
			if (m_enSkillType == EnSkillType.Hero && m_csJobSkill != null && IsChainSkill())
			{
				return m_csJobSkill.JobChainSkillList[m_nChainSkillIndex];
			}
			return null;
		}

		public bool IsChainSkill() // FormType  스킬형태(1:연계, 2:일반, 3:버프)
		{
			return (m_enSkillType == EnSkillType.Hero && m_csJobSkill != null && m_csJobSkill.FormTypeEnum == EnFormType.Chain);
		}

		public bool IsBuffSkill() // FormType  스킬형태(1:연계, 2:일반, 3:버프)
		{
			return (m_enSkillType == EnSkillType.Hero && m_csJobSkill != null && m_csJobSkill.FormTypeEnum == EnFormType.Buff);
		}

		public int GetSkillNo()
		{
			if (m_csJobSkill == null) return 1;
			return m_csJobSkill.SkillId % 100;
		}

		public int GetSkillIndex()
		{
			return GetSkillNo() - 1;
		}
	}

	protected CsSkillStatus m_csSkillStatus = new CsSkillStatus();
	public CsSkillStatus SkillStatus { get { return m_csSkillStatus; } set { m_csSkillStatus = value; } }

	//---------------------------------------------------------------------------------------------------
	protected virtual void Awake()
	{
		m_trPivotHUD = FindHudPivot(); // 본 정보.
		m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
		m_navMeshAgent = transform.GetComponent<NavMeshAgent>();
		m_animator = transform.GetComponent<Animator>();
		m_capsuleCollider = transform.GetComponent<CapsuleCollider>();
		m_csEquipment = transform.GetComponent<CsEquipment>();

		Height = transform.Find("pos_top").localPosition.y;
		m_flHeroHeightOffset = Height - m_trPivotHUD.localPosition.y;
		m_audioSource = transform.GetComponent<AudioSource>();
		m_audioSourceParent = transform.parent.GetComponent<AudioSource>();
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void Start()
	{
		//dd.d("CsHero.Start", IsMyHero);
		if (m_trPivotHUD == null)
		{
			Debug.Log("##########################    m_trPivotHUD == null       gameObject.name = " + gameObject.name);
		}

		CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

		foreach (CsAnimStateBehaviour cs in acs)
		{
			if ((EnAnimStatus)cs.Key == EnAnimStatus.Idle)
			{
				cs.EventStateEnter += OnEventAnimStartIdle;
			}
			else
			{
				cs.EventStateEnter += OnEventAnimStartAttack;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void LateUpdate()
	{
		HUDUpdatePos();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

		foreach (CsAnimStateBehaviour cs in acs)
		{
			if ((EnAnimStatus)cs.Key == EnAnimStatus.Idle)
			{
				cs.EventStateEnter -= OnEventAnimStartIdle;
			}
			else
			{
				cs.EventStateEnter -= OnEventAnimStartAttack;
			}
		}

		if (m_rtfHUD != null)
		{
			CsGameEventToUI.Instance.OnEventDeleteHeroHUD(HeroId);
			m_rtfHUD = null;
		}

		m_capsuleCollider = null;
		m_audioSource = null;

		m_trPivotHUD = null;
		m_csMyHeroInfo = null;
		m_csEquipment = null;
		m_csSkillStatus = null;

		RemoveCreature();
		m_csMount = null;
		m_csMountObject = null;
		m_goWing = null;
		m_csCartObject = null;
		m_csFlightObject = null;
		m_csTransformationMonster = null;

		
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	protected void ChangeBattleMode(bool bBattleMode)
	{
		m_bBattleMode = bBattleMode;
		if (IsStateIdle)
		{
			m_animator.SetTrigger(s_nAnimatorHash_changebattle);
		}

		if (m_bBattleMode)
		{
			m_animator.SetFloat(s_nAnimatorHash_battle, 1);

			if (IsTransformationStateMount() || IsTransformationStateCart())
			{
				ChangeTransformationState(EnTransformationState.None, true);
			}
		}
		else
		{
			m_animator.SetFloat(s_nAnimatorHash_battle, 0);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void HUDUpdatePos()
	{
		if (CsIngameData.Instance.InGameCamera == null) return;
		if (m_rtfHUD == null || m_trPivotHUD == null) return;

		if (CsIngameData.Instance.InGameCamera.IsAcceleartionCameraPlay() ||
			CsIngameData.Instance.InGameCamera.IsActionCameraPlay() ||
			CsIngameData.Instance.InGameCamera.IsClearCameraPlay() ||
			CsIngameData.Instance.InGameCamera.FirstEnter ||
			CsIngameData.Instance.ActiveScene == false)
		{
			if (m_rtfHUD.gameObject.activeInHierarchy)
			{
				m_rtfHUD.gameObject.SetActive(false);
			}
		}
		else
		{
			if (m_rtfHUD.gameObject.activeInHierarchy == false)
			{
				m_rtfHUD.gameObject.SetActive(true);
			}

			Vector3 vtPos;
			if (Height + 0.2f > m_trPivotHUD.localPosition.y + m_flHeroHeightOffset)
			{
				vtPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
			}
			else
			{
				vtPos = new Vector3(transform.position.x, m_trPivotHUD.position.y + 1.3f, transform.position.z);
			}
			m_rtfHUD.position = vtPos;

			float flDistance = (Vector3.Distance(CsIngameData.Instance.InGameCamera.transform.position, transform.position) - 10) / 25;
			m_rtfHUD.localScale = new Vector3(0.6f + flDistance, 0.6f + flDistance, 0.6f + flDistance);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SetAnimStatus(EnAnimStatus enAnimStatus)
	{
		if (CheckTransformationStatus(enAnimStatus)) return;

		if (enAnimStatus == (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status)) return;

		m_animator.SetInteger(s_nAnimatorHash_status, (int)enAnimStatus);

		if (enAnimStatus != (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status))
		{
			Debug.Log("SetAnimStatus2 " + ((EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status)).ToString());
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected EnAnimStatus GetAnimStatus() { return (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status); }
	protected bool IsAnimStatus(EnAnimStatus en) { return en == (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status); }
	protected bool IsStatusKindOfAttack(EnAnimStatus en) { return en >= EnAnimStatus.Skill01_01 && en <= EnAnimStatus.Skill05; }
	protected bool IsStateKindOfMove() { return m_enState > EnState.Idle && m_enState < EnState.Attack; }
	protected bool IsStateKindOfMove(EnState en) { return en > EnState.Idle && en < EnState.Attack; }
	public bool IsStateNoJoyOfMove() { return m_enState > EnState.MoveByJoystic && m_enState < EnState.Attack; }

	//---------------------------------------------------------------------------------------------------
	protected string GetAnimStateName(bool bCurrent)
	{
		string[] astr = { "Skill01_01", "Skill01_02", "Skill01_03", "Idle", "Walk", "Run", "Skill02", "Skill03", "Skill04", "Skill05", "Skill06" };
		AnimatorStateInfo asi = bCurrent ? m_animator.GetCurrentAnimatorStateInfo(0) : m_animator.GetNextAnimatorStateInfo(0);
		for (int i = 0; i < astr.Length; i++)
		{
			if (asi.IsName(astr[i]))
			{
				return astr[i];
			}
		}

		return "Unknown";
	}

	//---------------------------------------------------------------------------------------------------
	protected EnAnimStatus GetSkillAnimStatus()
	{
		if (m_csSkillStatus.IsSkillTypeCommon())
		{
			return (EnAnimStatus)((int)EnAnimStatus.CommonSkill);
		}

		if (m_csSkillStatus.Chained)
		{
			return (EnAnimStatus)((int)EnAnimStatus.Skill01_01 + m_csSkillStatus.ChainSkillIndex + 1);
		}

		if (m_csSkillStatus.IsChainSkill())
		{
			return (EnAnimStatus)((int)EnAnimStatus.Skill01_01 + m_csSkillStatus.ChainSkillIndex);
		}

		return (EnAnimStatus)((int)(EnAnimStatus.Skill02) + m_csSkillStatus.GetSkillIndex() - 1);
	}

	//---------------------------------------------------------------------------------------------------
	public virtual bool ChangeState(EnState enNewState)
	{
		if (enNewState == EnState.MoveByDirecting)
		{
			SetAnimStatus(EnAnimStatus.Walk);
		}
		else if (enNewState == EnState.MoveToPos)
		{
			SetAnimStatus(m_bMoveHasPath ? EnAnimStatus.Run : EnAnimStatus.Idle);
		}
		else if (enNewState == EnState.MoveByTouch || enNewState == EnState.MoveForSkill)
		{
			SetAnimStatus(m_bMoveHasPath ? EnAnimStatus.Run : EnAnimStatus.Idle);
		}
		else if (enNewState == EnState.Idle) // 1. 기본 상태.
		{
			SetAnimStatus(EnAnimStatus.Idle);
		}
		else if (enNewState == EnState.MoveByJoystic)
		{
			SetAnimStatus(EnAnimStatus.Run);
		}
		else if (enNewState == EnState.Interaction || enNewState == EnState.ReturnScroll || enNewState == EnState.ItemSteal)
		{
			SetAnimStatus(EnAnimStatus.Interact);
		}
		else if (enNewState == EnState.Fishing)
		{
			SetAnimStatus(EnAnimStatus.Fishing);
		}
		else if (enNewState == EnState.Damage) // 4. 데미지 (스턴 및 넉백시 사용).
		{
			SetAnimStatus(EnAnimStatus.Idle);
			m_animator.SetTrigger(s_nAnimatorHash_damage);
		}
		else if (enNewState == EnState.Dead) // 5. 사망.
		{
			SetAnimStatus(EnAnimStatus.Dead);
			m_animator.SetTrigger(s_nAnimatorHash_dead);
		}
		else if (enNewState == EnState.Attack) // 6. 공격.
		{
			SetAnimStatus(GetSkillAnimStatus());
		}

		m_enState = enNewState;
		return true;
	}

	// 탑승, 운송, 조종 관련.
	#region Transformation

	//---------------------------------------------------------------------------------------------------
	public bool IsTransformationStateNone()
	{
		return (m_enTransformationState == EnTransformationState.None);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTransformationStateMount()
	{
		return (m_enTransformationState == EnTransformationState.Mount);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTransformationStateCart()
	{
		return (m_enTransformationState == EnTransformationState.Cart);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTransformationStateTame()
	{
		return (m_enTransformationState == EnTransformationState.Tame);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTransformationStateFlight()
	{
		return (m_enTransformationState == EnTransformationState.Flight);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTransformationStateMonster()
	{
		return (m_enTransformationState == EnTransformationState.Monster);
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeTransformationState(EnTransformationState enNewTransformationState, bool bSend = false, CsMonsterInfo csMonsterInfo = null)
	{
		if (m_enTransformationState == enNewTransformationState) return;

		Debug.Log("ChangeTransformationState  enNewTransformationState = "+ enNewTransformationState+" , "+ bSend);
		if (IsTransformationStateMount()) // 탈것 탑승.
		{
			MountGetOff(bSend);
		}
		else if (IsTransformationStateCart())
		{
			CartGetOff(bSend);
		}
		else if (IsTransformationStateTame())
		{
			TameGetOff();
		}
		else if (IsTransformationStateFlight())
		{
			FlightGetOff(); // 비행 연출있음.
		}
		else if (IsTransformationStateMonster())
		{
			TransformationMonsterGetOff();
		}

		if (enNewTransformationState == EnTransformationState.None)
		{
			ChangeState(EnState.Idle);
		}
		else if (enNewTransformationState == EnTransformationState.Mount)
		{
			MountGetOn();
		}
		else if (enNewTransformationState == EnTransformationState.Cart)
		{
			CartGetOn();
		}
		else if (enNewTransformationState == EnTransformationState.Tame)
		{
			TameGetOn();
		}
		else if (enNewTransformationState == EnTransformationState.Flight)
		{
			FlightGetOn();
		}
		else if (enNewTransformationState == EnTransformationState.Monster)
		{
			TransformationMonsterGetOn(csMonsterInfo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool CheckTransformationStatus(EnAnimStatus enAnimStatus)
	{
		if (IsTransformationStateMount()) // 탈것 탑승중 기본 or 이동 상태가 아닌 경우.
		{
			if (enAnimStatus == EnAnimStatus.Idle || enAnimStatus == EnAnimStatus.Walk || enAnimStatus == EnAnimStatus.Run) // 기본 상태 or 이동
			{
				SetTransformationanimStatus(enAnimStatus);
				return true;
			}

			if (enAnimStatus != EnAnimStatus.Riding) // 변경 요청 Status가 라이딩이 아니라면 초기화.
			{
				Debug.Log("#####     CheckTransformationStatus     초기화 되어라     ");
				ChangeTransformationState(EnTransformationState.None, true);
			}
		}
		else if (IsTransformationStateCart())
		{
			if (enAnimStatus == EnAnimStatus.Idle || enAnimStatus == EnAnimStatus.Walk || enAnimStatus == EnAnimStatus.Run) // 기본 상태 or 이동
			{
				SetTransformationanimStatus(enAnimStatus);
				return true;
			}
		}
		else if (IsTransformationStateTame())
		{
			if (enAnimStatus == EnAnimStatus.Idle || enAnimStatus == EnAnimStatus.Walk || enAnimStatus == EnAnimStatus.Run) // 기본 상태 or 이동
			{
				SetTransformationanimStatus(enAnimStatus);
				return true;
			}
		}
		else if (IsTransformationStateFlight())
		{
			if (enAnimStatus == EnAnimStatus.Idle || enAnimStatus == EnAnimStatus.Walk || enAnimStatus == EnAnimStatus.Run) // 기본 상태 or 이동
			{
				SetTransformationanimStatus(enAnimStatus);
				return true;
			}
		}
		else if (IsTransformationStateMonster())
		{
			if (enAnimStatus == EnAnimStatus.Idle || enAnimStatus == EnAnimStatus.Walk || enAnimStatus == EnAnimStatus.Run || enAnimStatus == EnAnimStatus.Interact || IsStatusKindOfAttack(enAnimStatus))
			{
				SetTransformationanimStatus(enAnimStatus);
				return true;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void SetTransformationanimStatus(EnAnimStatus enAnimStatus)
	{
		if (IsTransformationStateMount()) // 탑승.
		{
			if (m_csMountObject == null) return;
			m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)enAnimStatus);
			m_csMountObject.ChangeState((EnMountState)enAnimStatus);
		}
		else if (IsTransformationStateCart()) // 운송.
		{
			if (m_csCartObject == null) return;
			if (enAnimStatus == EnAnimStatus.Idle)
			{
				m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)EnCartState.Idle);
				m_csCartObject.ChangeState(EnCartState.Idle);
			}
			else if (enAnimStatus == EnAnimStatus.Run) // 0,1,2
			{
				if (m_bCartAccelerate)
				{
					m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)EnCartState.Acceleration);
					m_csCartObject.ChangeState(EnCartState.Acceleration);
				}
				else
				{
					m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)EnCartState.Move);
					m_csCartObject.ChangeState(EnCartState.Move);
				}
			}
		}
		else if (IsTransformationStateTame()) // 조종
		{
			if (m_csTameMonster == null) return;
			m_csTameMonster.ChangeState((EnTameState)enAnimStatus);
		}
		else if (IsTransformationStateFlight())
		{
			if (m_csFlightObject == null) return;
			m_csFlightObject.ChangeState((EnFlightState)enAnimStatus);
		}
		else if (IsTransformationStateMonster())
		{
			if (m_csTransformationMonster == null) return;
			m_csTransformationMonster.ChangeState((EnTransformationMonsterState)enAnimStatus);
		}
	}

	#region Cart

	//---------------------------------------------------------------------------------------------------
	protected virtual void CartGetOn()
	{
		Debug.Log("1. Hero.CartGetOn()  m_corTransformation = " + m_corTransformation);

		if (m_corTransformation != null)
		{
			m_enNextTransformationState = EnTransformationState.Cart;
			return;
		}

		if (m_csCartObject == null) return;

		SetAnimStatus(EnAnimStatus.Riding);
		m_enTransformationState = EnTransformationState.Cart;

		m_bCartAccelerate = false;

		if (Height < 2.5f) // HUD 높이 수정.
		{
			Height = Height + 1f;
		}

		m_capsuleCollider.enabled = false;

		m_csCartObject.ChangeRiding(true);
		SetTransformationanimStatus(EnAnimStatus.Idle);

		m_csCartObject.transform.position = transform.position;
		m_csCartObject.transform.eulerAngles = transform.eulerAngles;
		m_csCartObject.transform.SetParent(transform);

		Transform trRidePosition = m_csCartObject.RidindCart.Find("Ride01");
		m_trPivotHUD.SetParent(trRidePosition);
		ChangeState(EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void CartGetOff(bool bSend)
	{
		Debug.Log("Hero.CartGetOff()  m_corTransformation = " + m_corTransformation);
		if (m_corTransformation != null) return;

		m_bCartAccelerate = false;
		ChangeState(EnState.Idle);

		if (Height > 2.5f) // HUD 높이 수정.
		{
			Height = Height - 1f;
		}

		m_capsuleCollider.enabled = true;
		m_enTransformationState = EnTransformationState.None;

		m_trPivotHUD.SetParent(transform); // 윈래 위치로 본 이동.
		m_csCartObject.transform.SetParent(transform.parent);
		ChangeState(EnState.Idle);

		if (m_csCartObject != null)
		{
			m_csCartObject.ChangeRiding(false);
		}
	}

	#endregion Cart

	#region Mount

	//---------------------------------------------------------------------------------------------------
	void CreateMount(int nMountId, int nMountLevel)
	{
		Debug.Log("#####     CreateMount     nMountId = " + nMountId + " // nMountLevel = " + nMountLevel);
		m_csMount = CsGameData.Instance.GetMount(nMountId);

		if (m_csMount == null) return;

		CsMountQuality csMountQuality = m_csMount.GetMountQuality(m_csMount.GetMountLevel(nMountLevel).MountLevelMaster.MountQualityMaster.Quality);

		if (csMountQuality == null) return;
		if (csMountQuality.PrefabName != null)
		{
			if (m_csMountObject != null) // 이전 탈것 정보 삭제.
			{
				if (m_csMountObject.name == csMountQuality.PrefabName) return;

				GameObject.Destroy(m_csMountObject.gameObject);
				m_csMountObject = null;
			}

			GameObject goMount = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/MountObject/" + csMountQuality.PrefabName), transform) as GameObject;
			goMount.AddComponent<CsMountObject>();

			goMount.name = csMountQuality.PrefabName;
			goMount.SetActive(false);

			m_csMountObject = goMount.GetComponent<CsMountObject>();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void MountGetOn()
	{
		Debug.Log("#####     MountGetOn()        m_corTransformation = " + m_corTransformation);
		if (m_corTransformation != null)
		{
			m_enNextTransformationState = EnTransformationState.Mount;
			return;
		}

		EnAnimStatus OldenAnimStatus = GetAnimStatus();
		SetAnimStatus(EnAnimStatus.Riding);

		CreateMount(m_nMountId, m_nMountLevel);

		if (m_csMount == null || m_csMountObject == null)
		{
			ChangeState(EnState.Idle);

			return;
		}

		if (Height < 2.5f) // HUD 높이 수정.
		{
			Height = Height + 1f;
		}

		m_enTransformationState = EnTransformationState.Mount;
		m_csMountObject.gameObject.SetActive(true);
		SetTransformationanimStatus(OldenAnimStatus);

		int nJobId = m_csMyHeroInfo.Job.ParentJobId == 0 ? m_csMyHeroInfo.Job.JobId : m_csMyHeroInfo.Job.ParentJobId;
		m_csMountObject.Init(m_csMount, transform, (EnJob)nJobId);
		Transform trRidePosition = m_csMountObject.transform.Find("Ride01");
		m_trPivotHUD.SetParent(trRidePosition);
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void MountGetOff(bool bSend)
	{
		Debug.Log("MountGetOff()   m_corTransformation = " + m_corTransformation);
		if (m_corTransformation != null) return;

		if (Height > 2.5f) // HUD 높이 수정.
		{
			Height = Height - 1f;
		}

		m_enTransformationState = EnTransformationState.None;
		m_trPivotHUD.SetParent(transform); // 윈래 위치로 본 이동.

		if (IsStateKindOfMove(m_enState))
		{
			SetAnimStatus(EnAnimStatus.Run);
		}
		else
		{
			SetAnimStatus(EnAnimStatus.Idle);
		}

		if (m_csMountObject != null)
		{
			GameObject.Destroy(m_csMountObject.gameObject);
			m_csMountObject = null;
		}
	}

	#endregion Mount

	#region Tame

	protected CsTameMonster m_csTameMonster;

	//---------------------------------------------------------------------------------------------------
	protected void TameGetOn()
	{
		Debug.Log("TameGetOn() m_corTransformation = " + m_corTransformation);
		if (m_corTransformation != null)
		{
			m_enNextTransformationState = EnTransformationState.Tame;
			return;
		}

		m_csTameMonster = CsIngameData.Instance.TameMonster;
		m_csTameMonster.gameObject.SetActive(true);
		m_csTameMonster.PlaceInstanceId = m_guidPlaceInstanceId;
		m_csTameMonster.StartTaming();

		transform.position = m_csTameMonster.transform.position + (m_csTameMonster.transform.forward * 6f); // 전방 6미터 앞에 설정함.
		transform.LookAt(m_csTameMonster.transform);
		SetAnimStatus(EnAnimStatus.Taming);

		m_enTransformationState = EnTransformationState.Tame;
		m_corTransformation = StartCoroutine(Tame());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator Tame()
	{
		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Tayming01", 3f);
		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Domination, true);
		yield return new WaitForSeconds(2.5f);

		m_animator.SetTrigger("taming");
		yield return new WaitForSeconds(0.2f);

		m_animator.SetInteger(s_nAnimatorHash_status, (int)EnAnimStatus.Idle);
		yield return new WaitForSeconds(0.3f);

		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Tayming02", 2f);
		m_csTameMonster.TameMonsterView(false);
		yield return new WaitForSeconds(1f);

		SetTransformationanimStatus(EnAnimStatus.Idle);
		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Tayming03", 2f);
		yield return new WaitForSeconds(0.1f);

		m_csTameMonster.transform.position = transform.position;
		m_csTameMonster.transform.rotation = transform.rotation;
		m_csTameMonster.TameMonsterView(true);

		if (Height < 2.5f) // HUD 높이 수정.
		{
			Height = Height + 3f;
		}

		m_bView = false;
		m_csEquipment.HideEquipments();
		Destroy(m_goShadow);
		m_goShadow = null;

		m_csTameMonster.transform.SetParent(transform);
		m_trPivotHUD.SetParent(m_csTameMonster.transform);
		yield return new WaitForSeconds(1f);

		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal, true);
		CsGameEventToUI.Instance.OnEventHideMainUI(false);
		CsGameEventToUI.Instance.OnEventTameButton(true);
		m_corTransformation = null;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void TameGetOff()
	{
		Debug.Log("TameGetOff   m_corTransformation = " + m_corTransformation);
		if (m_corTransformation == null)
		{
			if (Height > 2.5f) // HUD 높이 수정.
			{
				Height = Height - 3f;
			}
			m_trPivotHUD.SetParent(transform);

			m_trPivotHUD.SetParent(transform);                              // 윈래 위치로 본 이동.
			m_csTameMonster.transform.SetParent(transform.parent);

			m_enTransformationState = EnTransformationState.None;
			m_csTameMonster.TameEnd();                                      // 쓰러지는 모션 수행.	

			if (Dead)
			{
				ChangeState(EnState.Dead);
			}
			else
			{
				transform.position = transform.position + transform.forward;
				ChangeState(EnState.Idle);
			}

			CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);

			if (m_enNextTransformationState != EnTransformationState.None)
			{
				ChangeTransformationState(m_enNextTransformationState);
				m_enNextTransformationState = EnTransformationState.None;
			}
		}
	}

	#endregion Tame

	#region Flight

	CsFlightObject m_csFlightObject;
	protected bool m_bFlightAccelerate = false;       // 가속상태.
	float m_flOrgNavOffset = 0;
	Coroutine m_corTransformation;
	EnTransformationState m_enNextTransformationState = EnTransformationState.None;
	//int m_nNavAreaMask;

	// 연출 테스트
	public bool FlightAccelerate { get { return m_bFlightAccelerate; } set { m_bFlightAccelerate = value; } }

	//---------------------------------------------------------------------------------------------------
	protected virtual void FlightGetOn()
	{
		Debug.Log("FlightGetOn()    m_corTransformation = " + m_corTransformation);
		//m_nNavAreaMask = m_navMeshAgent.areaMask;
		//m_navMeshAgent.areaMask = 1;

		if (m_corTransformation != null)
		{
			m_enNextTransformationState = EnTransformationState.Flight;
			return;
		}

		if (m_csFlightObject == null)
		{
			GameObject goFlight = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/MountObject/Flight_Eagle"), transform) as GameObject;
			goFlight.AddComponent<CsFlightObject>();
			goFlight.name = "FlightEagle";
			goFlight.SetActive(false);
			m_csFlightObject = goFlight.GetComponent<CsFlightObject>();
		}

		m_flOrgNavOffset = m_navMeshAgent.baseOffset;
		m_corTransformation = StartCoroutine(FlightStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator FlightStart()
	{
		bool bDirecting = false;
		if (CsIngameData.Instance.Directing)
		{
			bDirecting = true;
		}
		else
		{
			CsIngameData.Instance.Directing = true;
		}

		CsGameEventToUI.Instance.OnEventJoystickReset();
		ChangeState(EnState.Idle);
		Vector3 vtPos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, vtPos, "Tayming03", 2f);
		m_navMeshAgent.updateRotation = true;
		m_navMeshAgent.angularSpeed = 70f;

		yield return new WaitForSeconds(0.1f);
		m_enTransformationState = EnTransformationState.Flight;

		m_csEquipment.HideEquipments();
		Destroy(m_goShadow);
		m_goShadow = null;
		m_csFlightObject.Init(transform, Height);

		yield return new WaitForSeconds(0.2f);
		m_csFlightObject.gameObject.SetActive(true);
		Transform trRidePosition = m_csFlightObject.transform.Find("Ride01");
		m_trPivotHUD.SetParent(trRidePosition);

		if (Height < 2.5f) // HUD 높이 수정.
		{
			Height = Height + 2.5f;
		}

		if (bDirecting == false)
		{
			CsIngameData.Instance.Directing = false;
		}
		m_corTransformation = null;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void FlightGetOff()
	{
		Debug.Log("FlightGetOff   m_corTransformation = " + m_corTransformation);		
		if (m_corTransformation == null)
		{
			//m_navMeshAgent.areaMask = m_nNavAreaMask;
			if (CsIngameData.Instance.InGameCamera != null)
			{
				Vector3 vtPos = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
				CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, vtPos, "Tayming03", 2f);
				if (Height > 2.5f) // HUD 높이 수정.
				{
					Height = Height - 2.5f;
				}
			}

			m_trPivotHUD.SetParent(transform);                            // 윈래 위치로 본 이동.

			if (m_csFlightObject != null)
			{
				m_csFlightObject.transform.SetParent(transform.parent);
				m_csFlightObject.gameObject.SetActive(false);
			}

			if (CsIngameData.Instance.InGameCamera != null)
			{
				m_corTransformation = StartCoroutine(FlightEnd());
			}
			else
			{
				m_navMeshAgent.baseOffset = 0f;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator FlightEnd()
	{
		bool bDirecting = false;
		if (CsIngameData.Instance.Directing)
		{
			bDirecting = true;
		}
		else
		{
			CsIngameData.Instance.Directing = true;
		}

		CsGameEventToUI.Instance.OnEventJoystickReset();

		bool bLanding = true;
		m_animator.SetBool(s_nAnimatorHash_flight, bLanding);

		float flValue = m_navMeshAgent.baseOffset * Time.deltaTime;
		if (m_navMeshAgent.baseOffset > 12f)
		{
			flValue = flValue * 0.7f;
			Debug.Log("1. flValue = " + flValue + " // baseOffset = " + m_navMeshAgent.baseOffset);
		}
		else
		{
			flValue = flValue * 1.2f;
			Debug.Log("2. flValue = " + flValue + " // baseOffset = " + m_navMeshAgent.baseOffset);
		}
		
		while (m_flOrgNavOffset != m_navMeshAgent.baseOffset)
		{
			if (m_navMeshAgent.baseOffset - flValue > m_flOrgNavOffset)
			{
				if (m_navMeshAgent.baseOffset < 3.8f)
				{
					if (bLanding)
					{
						bLanding = false;
						m_animator.SetBool(s_nAnimatorHash_flight, bLanding);
					}
				}

				m_navMeshAgent.baseOffset -= flValue;
			}
			else
			{
				m_navMeshAgent.baseOffset = m_flOrgNavOffset;
				break;
			}
			yield return new WaitForEndOfFrame();
		}

		m_navMeshAgent.angularSpeed = 720f;
		m_animator.SetBool(s_nAnimatorHash_flight, false);

		yield return new WaitForSeconds(0.1f);
		CsIngameData.Instance.InGameCamera.DoShake(1, false);

		yield return new WaitForSeconds(0.4f);
		m_enTransformationState = EnTransformationState.None;
		ChangeState(EnState.Idle);

		yield return new WaitForSeconds(1f);
		if (bDirecting == false)
		{
			CsIngameData.Instance.Directing = false;
		}
		m_corTransformation = null;

		if (m_enNextTransformationState != EnTransformationState.None)
		{
			ChangeTransformationState(m_enNextTransformationState);
			m_enNextTransformationState = EnTransformationState.None;
		}
	}

	#endregion Flight

	#region TransformationMonster

	protected CsTransformationMonster m_csTransformationMonster;
	protected int m_nTransformationMonsterSkillId = 0;
	//---------------------------------------------------------------------------------------------------
	protected virtual void TransformationMonsterGetOn(CsMonsterInfo csMonsterInfo)
	{
		Debug.Log("TransformationMonsterGetOn()            m_corTransformation = " + m_corTransformation + " // csMonsterInfo = "+ csMonsterInfo);

		if (m_corTransformation != null)
		{
			m_enNextTransformationState = EnTransformationState.Monster;
			return;
		}

		if (csMonsterInfo == null) return;

		ChangeState(EnState.Idle);
		m_corTransformation = StartCoroutine(TransformationMonster(csMonsterInfo));
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator TransformationMonster(CsMonsterInfo csMonsterInfo)
	{
		m_enTransformationState = EnTransformationState.Monster;
		ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/MonsterObject/" + csMonsterInfo.MonsterCharacter.PrefabName);
		yield return req;
		GameObject goTransformationMonster = Instantiate(req.asset, transform.parent) as GameObject;

		yield return goTransformationMonster;
		goTransformationMonster.name = csMonsterInfo.MonsterCharacter.PrefabName;
		goTransformationMonster.AddComponent<CsTransformationMonster>();

		m_csTransformationMonster = goTransformationMonster.GetComponent<CsTransformationMonster>();
		m_csTransformationMonster.Init(csMonsterInfo, transform, m_guidPlaceInstanceId);

		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Tayming03", 2f);

		yield return new WaitForSeconds(0.1f);
		m_csTransformationMonster.transform.position = transform.position;
		m_csTransformationMonster.transform.rotation = transform.rotation;

		m_csEquipment.HideEquipments();
		Destroy(m_goShadow);
		m_goShadow = null;

		m_csTransformationMonster.transform.SetParent(transform);
		m_trPivotHUD.SetParent(m_csTransformationMonster.transform);

		m_enState = EnState.Idle;
		SetTransformationanimStatus(EnAnimStatus.Idle);
		m_corTransformation = null;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void TransformationMonsterGetOff()
	{
		Debug.Log("TransformationMonsterGetOff   m_corTransformation = " + m_corTransformation);

		CsGameEventToUI.Instance.OnEventJoystickReset();
		ChangeState(EnState.Idle);

		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "Tayming03", 2f);
		if (m_corTransformation == null)
		{
			m_trPivotHUD.SetParent(transform);
			m_csTransformationMonster.transform.SetParent(transform.parent);
			m_enTransformationState = EnTransformationState.None;

			if (Dead)
			{
				ChangeState(EnState.Dead);
			}
			else
			{
				transform.position = transform.position + transform.forward;
				ChangeState(EnState.Idle);
			}

			Destroy(m_csTransformationMonster.gameObject);
			m_csTransformationMonster = null;

			if (m_enNextTransformationState != EnTransformationState.None)
			{
				ChangeTransformationState(m_enNextTransformationState);
				m_enNextTransformationState = EnTransformationState.None;
			}
		}
	}

	#endregion TransformationMonster

	#endregion Transformation

	//---------------------------------------------------------------------------------------------------
	protected virtual Transform FindHudPivot()
	{
		Transform trPivot = transform.Find("Bip001");
		if (trPivot == null)
		{
			return transform.Find("Bip01");
		}
		return trPivot;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void AttackHitEffect(Transform trTarget)
	{
		if (CsIngameData.Instance.Effect != 0 && trTarget != null)
		{
			Vector3 vtPos = trTarget.position;
			vtPos = new Vector3(vtPos.x + UnityEngine.Random.Range(-0.5f, 0.5f), vtPos.y + (Height * 0.5f), vtPos.z+ UnityEngine.Random.Range(0f, 0.3f)) - (transform.forward / 2);

			switch ((EnJob)Job.ParentJobId)   // 정리 끝나면 직업별로 상속 처리.
			{
				case EnJob.Gaia:
					CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, trTarget, vtPos, trTarget.rotation, "Gaia_Hit", 0.5f);
					break;
				case EnJob.Asura:
					CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, trTarget, vtPos, trTarget.rotation, "Asura_Hit", 0.5f);
					break;
				case EnJob.Deva:
					CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, trTarget, vtPos, trTarget.rotation, "Deva_Hit", 0.5f);
					break;
				case EnJob.Witch:
					CsEffectPoolManager.Instance.PlayHitEffect(CsEffectPoolManager.EnEffectOwner.None, trTarget, vtPos, trTarget.rotation, "Witch_Hit", 0.5f);
					break;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	//protected void CheckDamageAbsorbShield(long lAbnormalStateEffectInstanceId, int nDamageAbsorbShieldRemainingAbsorbAmount)  // 피해흡수쉴드남은흡수량 처리.
	//{
	//    if (m_dicAbnormalEffect.ContainsKey(lAbnormalStateEffectInstanceId))  // 1. 상태 이상 아이디가 현재 적용 중인경우,
	//    {
	//        if (nDamageAbsorbShieldRemainingAbsorbAmount <= 0) // 2. 피해흡수쉴드 남은 흡수량이 0이하인 경우.
	//        {
	//            RemoveAbnormalEffect(lAbnormalStateEffectInstanceId); // 3. 상태이상 Effect 제거.
	//        }
	//    }
	//} // 패킷데이터 변경으로 인한 주석처리

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator RevivalInvincibility(float flRemainingTime) // 유저 부활 후 무적상태.
	{
		Debug.Log(" 부활 후 무적상태 시작    무적상태 표시 관련 처리 필요.  이팩트 효과 없으면 삭제 가능.   flRemainingTime = " + flRemainingTime);
		while (m_bInvincibility)
		{
			flRemainingTime -= Time.deltaTime;

			if (flRemainingTime <= 0 || m_enState != EnState.Idle)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}

		Debug.Log(" 부활 후 무적상태 종료.     flRemainingTime = " + flRemainingTime);
		m_bInvincibility = false;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual bool HeroTrapHit(int nHp, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects, bool bSlowMoveSpeed, int nTrapPenaltyMoveSpeed)
	{
		if (alRemovedAbnormalStateEffects != null)
		{
			for (int i = 0; i < alRemovedAbnormalStateEffects.Length; i++) // 상태이상 이팩트 삭제.
			{
				RemoveAbnormalEffect(alRemovedAbnormalStateEffects[i]);
			}
		}

		if (bSlowMoveSpeed)
		{
			m_flTrapMoveSpeed = (float)nTrapPenaltyMoveSpeed / 10000;
			Debug.Log("HeroTrapHit      SlowMoveSpeed               >>>>               m_flTrapMoveSpeed = " + m_flTrapMoveSpeed);
		}

		if (nHp == 0 && !Dead)
		{
			return false;
		}

		return true;
	}

	#region NetEvent.

	//---------------------------------------------------------------------------------------------------
	protected void SendMoveStartEvent(bool bManualMoving, bool bWalk)
	{
		//dd.d("!!!!!!!!!!!!!!!!!!!!!!!  SendMoveStartEvent  @@@@@@@@@@@@@@@@@@@@@@@");
		CEBMoveStartEventBody csEvt = new CEBMoveStartEventBody();
		csEvt.heroId = HeroId;
		csEvt.isManualMoving = bManualMoving;
		csEvt.isWalking = bWalk;
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		CsRplzSession.Instance.Send(ClientEventName.MoveStart, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendMoveEvent(Vector3 vtLastSendPos)
	{
		CEBMoveEventBody csEvt = new CEBMoveEventBody();
		csEvt.heroId = HeroId;
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		csEvt.position = CsRplzSession.Translate(vtLastSendPos);
		csEvt.rotationY = m_flRotationY;
		CsRplzSession.Instance.Send(ClientEventName.Move, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendMoveEndEvent()
	{
		//dd.d("!!!!!!!!!!!!!!!!!!!!!!!  SendMoveEndEvent  @@@@@@@@@@@@@@@@@@@@@@@");
		CEBMoveEndEventBody csEvt = new CEBMoveEndEventBody();
		csEvt.heroId = HeroId;
		csEvt.placeInstanceId = m_guidPlaceInstanceId;
		CsRplzSession.Instance.Send(ClientEventName.MoveEnd, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendMountGetOn()
	{
		Debug.Log("CsMyPlayer.SendMountGetOn()");
		MountGetOnCommandBody csEvt = new MountGetOnCommandBody();
		CsRplzSession.Instance.Send(ClientCommandName.MountGetOn, csEvt);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAbnormalStateEffectStart(long lAbnormalStateEffectId, int nAbnormalStateId, int nSourceJobId, int nAbnormalLevel, int nDamageAbsorbShieldRemainingAbsorbAmount, float flRemainTime, int nDamageAbsorbShieldRemain, long[] alRemovedAbnormalEffects)
	{
		RemoveAbnormalEffect(alRemovedAbnormalEffects);
		AbnormalSet(nAbnormalStateId, lAbnormalStateEffectId, nSourceJobId, nAbnormalLevel, nDamageAbsorbShieldRemain, flRemainTime, true);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventHeroAbnormalStateEffectFinished(long lAbnormalStateEffectInstanceId) // 상태이상 종료.
	{
		//Debug.Log("######         NetEventHeroAbnormalStateEffectFinished        lAbnormalStateEffectInstanceId   : " + lAbnormalStateEffectInstanceId);
		RemoveAbnormalEffect(lAbnormalStateEffectInstanceId);
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventBattleModeStart() // 전투상태.
	{
		ChangeBattleMode(true);
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void NetEventBattleModeEnd() // 비전투상태.
	{
		//Debug.Log("NetEventBattleModeEnd()");
		ChangeBattleMode(false);
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void NetEventChangedMaxHp(int nMaxHp, int nHp)
	{
		Debug.Log("NetEventEvtMaxHpChanged()");
		MaxHp = nMaxHp;
		Hp = nHp;		
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void NetEventChangeHp(int nHp)
	{
		Debug.Log("NetEventChangeHp() nHp = " + nHp);
		Hp = nHp;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventFishingZoneEnter()
	{
		Debug.Log("NetEventFishingZoneEnter()");
		ChangeTransformationState(EnTransformationState.None, true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartGetOn(CsCartObject csCartObject) // EventMyHeroCartGetOn,  EventCartGetOn
	{
		Debug.Log("NetEventCartGetOn()   csCartObject = " + csCartObject);
		m_csCartObject = csCartObject;
		ChangeTransformationState(EnTransformationState.Cart);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartChange(CsCartObject csCartObject)
	{
		Debug.Log("NetEventCartChange()");
		m_csCartObject = csCartObject;
		ChangeTransformationState(EnTransformationState.Cart);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartGetOff() // EventMyHeroCartGetOff, EventCartGetOff
	{
		Debug.Log("NetEventCartGetOff()");
		ChangeTransformationState(EnTransformationState.None);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartAccelerate(bool bSuccess) // EventCartAccelerate, EventCartHighSpeedStart	// 당사자 + 당사자 외 공용.
	{
		Debug.Log("NetEventCartAccelerate() bSuccess = " + bSuccess);
		if (bSuccess)
		{
			m_bCartAccelerate = true;

			if (m_csCartObject.CartState != EnCartState.Idle)
			{
				m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)EnCartState.Acceleration);
				m_csCartObject.ChangeState(EnCartState.Acceleration);
			}
		}
		else
		{
			m_bCartAccelerate = false;

			if (m_csCartObject.CartState != EnCartState.Idle)
			{
				m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)EnCartState.Move);
				m_csCartObject.ChangeState(EnCartState.Move);
			}
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartHighSpeedEnd() // EventMyCartHighSpeedEnd, EventCartHighSpeedEnd
	{
		Debug.Log("NetEventCartHighSpeedEnd()");
		m_bCartAccelerate = false;

		if (m_csCartObject.CartState != EnCartState.Idle)
		{
			m_animator.SetInteger(s_nAnimatorHash_ridingstatus, (int)EnCartState.Move);
			m_csCartObject.ChangeState(EnCartState.Move);
		}
		ChangeHeroMoveSpeed(GetHeroMoveSpeed());
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTransformationMonster(int nTransformationMonsterId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		if (nTransformationMonsterId != 0)
		{
			Debug.Log("NetEventTransformationMonster()    nTransformationMonsterId = " + nTransformationMonsterId);
			CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(nTransformationMonsterId);
			ChangeTransformationState(CsHero.EnTransformationState.Monster, false, csMonsterInfo);
		}

		MaxHp = nMaxHp;
		Hp = nHp;
		RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventTransformationMonsterFinish(int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("NetEventTransformationMonsterFinish()    ");
		MaxHp = nMaxHp;
		Hp = nHp;
		RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
		ChangeTransformationState(CsHero.EnTransformationState.None, false);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCreatureParticipated(CsCreature csCreature)
	{
		if (csCreature != null)
		{
			Debug.Log("NetEventCreatureParticipated");
			StartCoroutine(CreateCreature(csCreature));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCreatureParticipationCanceled()
	{
		Debug.Log("NetEventCreatureParticipationCanceled");
		RemoveCreature();
	}

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator CreateCreature(CsCreature csCreature)
	{
		ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/MonsterObject/" + csCreature.CreatureCharacter.PrefabName);
		yield return req;

		GameObject goCreature = Instantiate(req.asset, transform.parent) as GameObject;
		yield return goCreature;

		goCreature.AddComponent<NavMeshAgent>();
		goCreature.AddComponent<CsCreatureObject>();
		m_csCreatureObject = goCreature.GetComponent<CsCreatureObject>();
		m_csCreatureObject.Init(csCreature, transform);
	}

	//---------------------------------------------------------------------------------------------------
	protected void RemoveCreature()
	{
		if (m_csCreatureObject != null)
		{
			Debug.Log("NetEventCreatureParticipationCanceled");
			Destroy(m_csCreatureObject.gameObject);
			m_csCreatureObject = null;
		}
	}

	#endregion NetEvent.

	#region Event.

	//---------------------------------------------------------------------------------------------------
	protected abstract void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey);
	protected abstract void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey);

	#endregion Event.

	#region Anim
	//---------------------------------------------------------------------------------------------------
	void OnAnimDamageEnd()
	{
		ChangeState(EnState.Idle);
	}

	// Test 중
	//---------------------------------------------------------------------------------------------------
	protected virtual void OnEventAnimCameraActionStart() { }
	protected virtual void OnEventAnimCameraActionEnd() { }

	#endregion Anim

	//---------------------------------------------------------------------------------------------------
	public void NavMeshSetting()
	{
		m_navMeshAgent.enabled = false;
		m_navMeshAgent.height = 2f;
		m_navMeshAgent.angularSpeed = 720f;
		m_navMeshAgent.acceleration = 100f;
		m_navMeshAgent.stoppingDistance = 0.1f;
		m_navMeshAgent.autoBraking = true;
		m_navMeshAgent.autoRepath = true;
		m_navMeshAgent.avoidancePriority = 50;
		m_navMeshAgent.autoTraverseOffMeshLink = false;
		m_navMeshAgent.autoRepath = false;
		m_navMeshAgent.speed = GetHeroMoveSpeed();
		m_navMeshAgent.enabled = true;
	}

	//---------------------------------------------------------------------------------------------------
	public void EnterEffectStart() // 최초 입장시 효과.
	{
		if (CsIngameData.Instance.EffectEnum != EnOptionEffect.No)
		{
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "HeroEnter", 3.5f);
		}

		if (CsIngameData.Instance.EffectSound)
		{
			AudioClip EnterSound = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_continent_enter");
			m_audioSource.PlayOneShot(EnterSound);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void HeroReviveEffect() // 부활 입장시 효과.
	{
		if (CsIngameData.Instance.EffectEnum != EnOptionEffect.No)
		{
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, transform, transform.position, "HeroRevive", 3.5f);
		}

		if (CsIngameData.Instance.EffectSound)
		{
			AudioClip ReviveSound = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_hero_resurrection");
			m_audioSource.PlayOneShot(ReviveSound);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void DelayDead()
	{
		if (IsTransformationStateNone() == false)
		{
			ChangeTransformationState(EnTransformationState.None, IsMyHero);
		}
		if (m_enState != EnState.Dead)
		{
			ChangeState(EnState.Dead);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected float GetHeroMoveSpeed()
	{
		if (Job == null) return 0;

		float flMoveSpeed = Job.MoveSpeed / 100;

		if (IsTransformationStateTame())
		{
			flMoveSpeed = Job.MoveSpeed / 100;
		}
		else if (IsTransformationStateMount())
		{
			if (m_csMount != null)
			{
				flMoveSpeed = m_csMount.MoveSpeed / 100;
			}
		}
		else if (IsTransformationStateCart())
		{
			if (m_bCartAccelerate) // 가속중.
			{
				flMoveSpeed = CsGameConfig.Instance.CartHighSpeed / 100;
			}
			else
			{
				flMoveSpeed = CsGameConfig.Instance.CartNormalSpeed / 100;
			}
		}
		else if (IsTransformationStateFlight())
		{
			if (m_bFlightAccelerate)
			{
				flMoveSpeed = 13f;
			}
			else
			{
				flMoveSpeed = 8f;
			}
		}
		else if (IsTransformationStateMonster())
		{
			if (m_csTransformationMonster != null)
			{
				if (m_bWalk)
				{
					flMoveSpeed = m_csTransformationMonster.MonsterInfo.MoveSpeed / 100;
				}
				else
				{
					flMoveSpeed = m_csTransformationMonster.MonsterInfo.BattleMoveSpeed / 100;
				}
				
			}
		}
		else
		{
			flMoveSpeed = GetAccelerationState();
		}
		
		return flMoveSpeed + (flMoveSpeed * (m_flWindWalkMoveSpeed + m_flTrapMoveSpeed));
	}

	//---------------------------------------------------------------------------------------------------
	protected float GetAccelerationState()
	{
		if (m_bAcceleration)   // 가속상태일때.
		{
			if (m_flAccelerationValue != 1f)
			{
				m_flAccelerationValue += 0.1f;
				if (m_flAccelerationValue > 1f)
				{
					m_flAccelerationValue = 1f;
				}
				m_animator.SetFloat(s_nAnimatorHash_acceleration, m_flAccelerationValue);
			}

			return CsGameConfig.Instance.AccelerationMoveSpeed / 100f;
		}
		else
		{
			if (m_flAccelerationValue != 0f)
			{
				if (m_flAccelerationValue > 0)
				{
					m_flAccelerationValue -= 0.1f;
				}
				else
				{
					m_flAccelerationValue = 0f;
				}

				m_animator.SetFloat(s_nAnimatorHash_acceleration, m_flAccelerationValue);
			}

			if (m_bWalk)
			{
				if (m_flJoysticWalk != 0)
				{
					m_flJoysticWalk = 0;
					m_animator.SetFloat(s_nAnimatorHash_move, m_flJoysticWalk);
				}
				
				return Job.WalkMoveSpeed / 100f;
			}
			else
			{
				if (m_flJoysticWalk != 1)
				{
					m_flJoysticWalk = 1;
					m_animator.SetFloat(s_nAnimatorHash_move, m_flJoysticWalk);
				}

				return Job.MoveSpeed / 100f;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void SetDestination(Vector3 vtPos, float flStopRange)
	{
		if (m_navMeshAgent.isActiveAndEnabled)
		{
			m_navMeshAgent.SetDestination(vtPos);
			m_vtMovePos = vtPos;
			m_flMoveStopRange = flStopRange;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected bool Move(EnState en, Vector3 vtMovePos, float flStopRange, bool bMoveToPosHasPath = false)
	{
		m_bMoveHasPath = bMoveToPosHasPath;

		if (!ChangeState(en))
		{
			return false;
		}
		
		SetDestination(vtMovePos, flStopRange);
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	protected float SkillMoveSpeed(Vector3 vtDir, float flTime)
	{
		if (flTime == 0) return 0;

		float flSpeed = GetDistanceFormTarget(vtDir) / flTime;
		if (flSpeed < 15f)
		{
			flSpeed = 15f;
		}
		return flSpeed;
	}

	//---------------------------------------------------------------------------------------------------
	protected void SetSkillCastingFixedMoveTypeData(Transform trTargetEnemy, bool bSecondMove = false)
	{
		if (m_csSkillStatus.JobSkill == null) return;

		if (m_bStun || m_bBlockade) return;

		m_vtSkillMovePos = transform.position;

		switch (m_csSkillStatus.JobSkill.CastingMoveTypeEnum)
		{
			case EnCastingMoveType.Manual: // 조작가능.
				ChangeHeroMoveSpeed(((float)m_csSkillStatus.JobSkill.CastingMoveValue1) / 100); // 조작중 이동속도 보정.
				break;

			case EnCastingMoveType.Fixed:  // 고정.
				float flDistance = (float)m_csSkillStatus.JobSkill.CastingMoveValue1 / 100;
				float flTime = (float)m_csSkillStatus.JobSkill.CastingMoveValue2 / 100;

				if (bSecondMove) // 아수라 이중이동시.
				{
					flDistance = -0.6f;
					flTime = 3f;
				}

				if (flDistance == 0) // 타겟을 기준으로 스킬 시전.
				{
					if (trTargetEnemy != null)
					{
						Vector3 vtDir = transform.position - trTargetEnemy.position;
						vtDir.y = 0;
						vtDir.Normalize();

						if (IsTargetInDistance(m_vtTargetPos, GetAttackCastRange(trTargetEnemy, m_csSkillStatus.JobSkill.CastRange))) // 스킬거리 + 몬스터 둘레 안에 타겟이 위치할때.
						{
							float flTargetRadius = CsIngameData.Instance.IngameManagement.GetCsMoveUnit(trTargetEnemy).Radius;
							if (IsTargetInDistance(m_vtTargetPos, flTargetRadius + 1f) == false) // 몬스터 둘레 안에 위치해있지 않을때.
							{
								m_vtSkillMovePos = m_vtTargetPos + (vtDir * (flTargetRadius + 1f));
							}
						}
						else
						{
							m_vtSkillMovePos = transform.position - (vtDir * m_csSkillStatus.JobSkill.CastRange);
						}
					}
					else
					{
						m_vtSkillMovePos = transform.position + transform.forward;
					}
				}
				else  // 본인 기준 방향 거리로 시전.
				{
					m_vtSkillMovePos = transform.position + (transform.forward * flDistance); // 방향으로 flDistance 만큼 이동.
				}

				if (m_navMeshAgent.isActiveAndEnabled)
				{
					NavMeshHit navMeshHit;
					m_navMeshAgent.Raycast(m_vtSkillMovePos, out navMeshHit);
					m_vtSkillMovePos = navMeshHit.position;
				}
				
				m_flSkillMoveSpeed = SkillMoveSpeed(m_vtSkillMovePos, flTime);				
				ChangeHeroMoveSpeed(m_flSkillMoveSpeed);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected bool StopSkill()
	{
		if (m_csSkillStatus.IsStatusReady())
		{
			m_csSkillStatus.Reset();
			m_csSkillStatus.ResetNextSkill();
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void Attack()
	{
		if (ChangeState(EnState.Attack))
		{
			m_csSkillStatus.ResetNextSkill();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected float GetAttackCastRange(Transform trTarget, float flJobSkillCastRange)
	{
		if (trTarget == null || trTarget.CompareTag(CsMonster.c_strTag) == false)
		{
			return flJobSkillCastRange;
		}
		return (CsIngameData.Instance.IngameManagement.GetCsMoveUnit(trTarget).Radius + flJobSkillCastRange);
	}
}
