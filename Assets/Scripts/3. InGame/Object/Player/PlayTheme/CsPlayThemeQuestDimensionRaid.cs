using SimpleDebugLog;
using System.Collections;
using UnityEngine;


public class CsPlayThemeQuestDimensionRaid : CsPlayThemeQuest
{
	CsDimensionRaidQuestManager m_csDimensionRaidQuestManager;
	CsNpcInfo m_csNpcInfo;

	bool m_bArrival = false;
	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			dd.d("1. CsPlayThemeQuestDimensionRaid.ArrivalMoveToPos");
			CheckNpcDialog();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDimensionRaidQuestManager = CsDimensionRaidQuestManager.Instance;
		m_csDimensionRaidQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csDimensionRaidQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csDimensionRaidQuestManager.EventUpdateState += OnEventUpdateState;
		m_csDimensionRaidQuestManager.EventInteractionArea += OnEventInteractionArea;
		m_csDimensionRaidQuestManager.EventMyHeroDimensionRaidInteractionStart += OnEventMyHeroDimensionRaidInteractionStart;
		m_csDimensionRaidQuestManager.EventDimensionRaidQuestComplete += OnEventDimensionRaidQuestComplete;

		m_csDimensionRaidQuestManager.EventDimensionRaidInteractionStart += OnEventDimensionRaidInteractionStart;
		m_csDimensionRaidQuestManager.EventDimensionRaidInteractionCompleted += OnEventDimensionRaidInteractionCompleted;
		m_csDimensionRaidQuestManager.EventDimensionRaidInteractionCanceled += OnEventDimensionRaidInteractionCanceled;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDimensionRaidQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csDimensionRaidQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csDimensionRaidQuestManager.EventUpdateState -= OnEventUpdateState;
		m_csDimensionRaidQuestManager.EventInteractionArea -= OnEventInteractionArea;
		m_csDimensionRaidQuestManager.EventMyHeroDimensionRaidInteractionStart -= OnEventMyHeroDimensionRaidInteractionStart;
		m_csDimensionRaidQuestManager.EventDimensionRaidQuestComplete -= OnEventDimensionRaidQuestComplete;

		m_csDimensionRaidQuestManager.EventDimensionRaidInteractionStart -= OnEventDimensionRaidInteractionStart;
		m_csDimensionRaidQuestManager.EventDimensionRaidInteractionCompleted -= OnEventDimensionRaidInteractionCompleted;
		m_csDimensionRaidQuestManager.EventDimensionRaidInteractionCanceled -= OnEventDimensionRaidInteractionCanceled;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		m_csDimensionRaidQuestManager = null;
		m_csNpcInfo = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csPlayer.IsStateIdle)
			{
				DimensionRaidPlay();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestDimensionRaid.StopAutoPlay()     bNotify = " + bNotify);
			if (bNotify)
			{
				m_csDimensionRaidQuestManager.StopAutoPlay(this);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (IsThisAutoPlaying())
			{
				dd.d("CsPlayThemeQuestThreatOfFarm.StateChangedToIdle");
				Player.StartCoroutine(CorutineStateChangedToIdle());
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		yield return new WaitForSeconds(0.2f);
		if (Player.IsStateIdle)
		{
			dd.d("CorutineStateChangedToIdle 2");
			if (IsThisAutoPlaying())
			{
				if (m_bArrival == false)
				{
					dd.d("CorutineStateChangedToIdle 3");
					DimensionRaidPlay();
				}
			}
		}
		m_bArrival = false;
	}

	#endregion Override

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			CheckNpcDialog();
		}
	}

	#endregion Event.Player

	#region Event.DimensionRaidQuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			m_csDimensionRaidQuestManager.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeQuestDimensionRaid.OnEventStartAutoPlay()");
		SetDisplayPath();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeQuestDimensionRaid.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateState()
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestDimensionRaid.OnEventUpdateState()");
			SetDisplayPath();
			Player.ChangeState(CsHero.EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInteractionArea(bool bEnter)
	{
		if (bEnter && IsThisAutoPlaying())
		{
			CheckNpcDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroDimensionRaidInteractionStart()
	{
		Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
		Player.ResetBattleMode();

		if (Player.SkillStatus.IsStatusPlayAnim()) return;

		Debug.Log("OnEventMyHeroDimensionRaidInteractionStart()");
		m_csNpcInfo = m_csDimensionRaidQuestManager.DimensionRaidQuest.GetDimensionRaidQuestStep(m_csDimensionRaidQuestManager.Step).TargetNpcInfo;
		NpcDialog(m_csNpcInfo);
		Player.ChangeState(CsHero.EnState.Interaction);

		m_csDimensionRaidQuestManager.InteractionStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDimensionRaidQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestDimensionRaid.OnEventDimensionRaidQuestComplete()");
			Player.SetAutoPlay(null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDimensionRaidInteractionStart()
	{
		Debug.Log("1. OnEventDimensionRaidInteractionStart()");
		if (Player.State != CsHero.EnState.Interaction)
		{
			dd.d("2. OnEventDimensionRaidInteractionStart     >>    InteractionCancel ");
			Player.ChangeState(CsHero.EnState.Idle);
			CsIngameData.Instance.IngameManagement.StateEndOfInteraction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDimensionRaidInteractionCanceled()
	{
		Debug.Log("CsPlayThemeQuestDimensionRaid.OnEventDimensionRaidInteractionCanceled()");
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDimensionRaidInteractionCompleted()
	{
		Debug.Log("CsPlayThemeQuestDimensionRaid.OnEventDimensionRaidInteractionCompleted()");
		Player.ChangeState(CsHero.EnState.Idle);
	}

	#endregion Event.DimensionRaidQuestManager

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		m_csNpcInfo = GetNpcInfo();

		if (m_csDimensionRaidQuestManager.DimensionRaidState == EnDimensionRaidState.Accepted) // <수락전 or 완료>.  (퀘스트 목적 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
			{
				if (m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
				{
					Player.DisplayPath.SetPath(m_csNpcInfo.Position);
					Player.SetWayPoint(m_csNpcInfo.Position, EnWayPointType.Npc, m_csNpcInfo.InteractionMaxRange);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csNpcInfo.ContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
			else
			{
				if (m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
				{
					Player.DisplayPath.SetPath(m_csNpcInfo.Position);
					Player.SetWayPoint(m_csNpcInfo.Position, EnWayPointType.Npc, m_csNpcInfo.InteractionMaxRange);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
		}
		else  // <미션중>. (본인 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >> 국가이동 Npc에게 이동.
			{
				if (m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
				{
					Player.DisplayPath.SetPath(m_csNpcInfo.Position);
					Player.SetWayPoint(m_csNpcInfo.Position, EnWayPointType.Npc, m_csNpcInfo.InteractionMaxRange);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
			else
			{
				if (m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
				{
					EnWayPointType enWayPointType = m_csDimensionRaidQuestManager.DimensionRaidState == EnDimensionRaidState.Accepted ? EnWayPointType.Interaction : EnWayPointType.Npc;
					Player.DisplayPath.SetPath(m_csNpcInfo.Position);
					Player.SetWayPoint(m_csNpcInfo.Position, enWayPointType, m_csNpcInfo.InteractionMaxRange);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csNpcInfo.ContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog()
	{
		m_csNpcInfo = GetNpcInfo();

		if (IsThisAutoPlaying())
		{
			if (m_csNpcInfo.NpcType == EnNpcType.NationTransmission && Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange)) // 국가이동 Npc앞 도착.
			{
				Debug.Log("CheckNpcDialog  >>  m_csDimensionRaidQuestManager.NationTransmissionReadyOK()  m_csNpcInfo.NpcId = " + m_csNpcInfo.NpcId);
				NpcDialog(m_csNpcInfo);
				m_csDimensionRaidQuestManager.NationTransmissionReadyOK();
				return;
			}
		}

		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
		{
			if (m_csDimensionRaidQuestManager.DimensionRaidState != EnDimensionRaidState.Accepted) // 퀘스트 수락 or 완료 
			{
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					if (m_csDimensionRaidQuestManager.NpctDialog())
					{
						Debug.Log("CheckNpcDialog  >>  m_csDimensionRaidQuestManager.NpctDialog()  m_csNpcInfo.NpcId = " + m_csNpcInfo.NpcId);
						NpcDialog(m_csNpcInfo);
					}
				}
			}
		}
		else // 적국일때.
		{
			if (m_csDimensionRaidQuestManager.DimensionRaidState == EnDimensionRaidState.Accepted) // 퀘스트 미션 진행중.
			{
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					Debug.Log("CheckNpcDialog  >>  m_csDimensionRaidQuestManager.InteractionReadyOK()  m_csNpcInfo.NpcId = " + m_csNpcInfo.NpcId);
					NpcDialog(m_csNpcInfo);
					m_csDimensionRaidQuestManager.InteractionReadyOK();
				}
			}
		}	
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo GetNpcInfo()
	{
		if (m_csDimensionRaidQuestManager.DimensionRaidState == EnDimensionRaidState.Accepted)  // <미션중>. (적 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return m_csDimensionRaidQuestManager.DimensionRaidQuest.GetDimensionRaidQuestStep(m_csDimensionRaidQuestManager.Step).TargetNpcInfo;
			}
		}
		else  // <수락전 or 완료>.  (퀘스트 목적 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return m_csDimensionRaidQuestManager.DimensionRaidQuest.QuestNpcInfo;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void DimensionRaidPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csDimensionRaidQuestManager.Interaction) return; // 상호작용중.

			m_csNpcInfo = GetNpcInfo();

			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csNpcInfo.ContinentId, m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange, true))
			{
				CheckNpcDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RestartAutoPlay() // 재시작시 오토 상태 확인.
	{
		Player.StartCoroutine(DelayStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csDimensionRaidQuestManager.Auto)
		{
			if (Player.Dead || m_csDimensionRaidQuestManager == null || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				Debug.Log("CsPlayThemeQuestDimensionRaid.RestartAutoPlay()   >>  Stop");
				m_csDimensionRaidQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsPlayThemeQuestDimensionRaid.RestartAutoPlay()  >>  Satrt");
				SetDisplayPath();
				DimensionRaidPlay();
				Player.SetAutoPlay(this, true);
			}
		}
	}
}
