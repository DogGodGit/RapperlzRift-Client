using SimpleDebugLog;
using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestCreatureFarm : CsPlayThemeQuest
{
	CsState m_csState;	
	CsCreatureFarmQuestManager m_csCreatureFarmQuestManager;
	bool m_bArrival = false;

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			PlayContinue();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		//dd.d("CsPlayThemeQuestCreatureFarm.Init");
		base.Init(csPlayer);

		m_csCreatureFarmQuestManager = CsCreatureFarmQuestManager.Instance;
		m_csCreatureFarmQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csCreatureFarmQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csCreatureFarmQuestManager.EventCreatureFarmQuestAccept += OnEventCreatureFarmQuestAccept;
		m_csCreatureFarmQuestManager.EventCreatureFarmQuestComplete += OnEventCreatureFarmQuestComplete;
		m_csCreatureFarmQuestManager.EventCreatureFarmQuestMissionMoveObjectiveComplete += OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		m_csCreatureFarmQuestManager.EventCreatureFarmQuestMissionCompleted += OnEventCreatureFarmQuestMissionCompleted;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		SyncState();
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{		
		m_csCreatureFarmQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csCreatureFarmQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csCreatureFarmQuestManager.EventCreatureFarmQuestAccept -= OnEventCreatureFarmQuestAccept;
		m_csCreatureFarmQuestManager.EventCreatureFarmQuestComplete -= OnEventCreatureFarmQuestComplete;
		m_csCreatureFarmQuestManager.EventCreatureFarmQuestMissionMoveObjectiveComplete -= OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		m_csCreatureFarmQuestManager.EventCreatureFarmQuestMissionCompleted -= OnEventCreatureFarmQuestMissionCompleted;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		m_csCreatureFarmQuestManager = null;
		m_csState = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		Debug.Log("CsPlayThemeQuestCreatureFarm.StartAutoPlay()  + "+ m_csCreatureFarmQuestManager.CreatureFarmQuestState);
		m_timer.Init(0.2f);
		SyncState();
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		m_csState.Update();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			if (bNotify)
			{
				m_csCreatureFarmQuestManager.StopAutoPlay(this);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (IsThisAutoPlaying())
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (m_csState != null)
				{
					Player.StartCoroutine(CorutineStateChangedToIdle());
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		yield return new WaitForSeconds(0.2f);
		if (Player.IsStateIdle)
		{
			if (IsThisAutoPlaying())
			{
				if (m_bArrival)
				{
					m_bArrival = false;
				}
				else
				{
					m_csState.StateChangedToIdle();
				}
			}
		}
	}

	#endregion Override

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			if (Player.Target == null || m_csState == null) return;

			m_csState.ArrivalMoveByTouchByManual(bMoveByTouchTarget);
		}
	}

	#endregion Event.Player

	#region Event.QuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
			return;
		}

		m_csState.SetDisplayPath();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (IsThisAutoPlaying())
		{
			if (objCaller == this) return;
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestAccept()
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionMoveObjectiveComplete(bool bLevelUp, long lExp)
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionCompleted(bool bLevelUp, long lExp)
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestComplete(bool bLevelUp, long lExp)
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.QuestManager

	//---------------------------------------------------------------------------------------------------
	void PlayContinue()
	{
		m_csState.Play();
	}

	//---------------------------------------------------------------------------------------------------
	void SyncState()
	{
		if (m_csCreatureFarmQuestManager == null) return;
		
		switch (m_csCreatureFarmQuestManager.CreatureFarmQuestState)
		{
		case EnCreatureFarmQuestState.None:
			if (m_csCreatureFarmQuestManager.DailyCreatureFarmQuestAcceptionCount == 0)
			{
				SetState(new CsStateAccept());
			}
			else
			{
				SetState(new CsState());
			}
			break;
		case EnCreatureFarmQuestState.Accepted:
			if (m_csCreatureFarmQuestManager.HeroCreatureFarmQuest.CreatureFarmQuestMission == null)
			{
				SetState(new CsStateComplete());
			}
			else
			{
				if (m_csCreatureFarmQuestManager.HeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType == 1)
				{
					SetState(new CsStateExecuteMove());
				}
				else if (m_csCreatureFarmQuestManager.HeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType == 2)
				{
					SetState(new CsStateExecuteInteraction());
				}
				else if (m_csCreatureFarmQuestManager.HeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType == 3)
				{
					SetState(new CsStateExecuteBattle());
				}
			}
			break;
		case EnCreatureFarmQuestState.Executed:
			SetState(new CsStateComplete());
			break;
		default:
			SetState(new CsState());
			break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetState(CsState csStateNew)
	{
		if (m_csState != null)
		{
			m_csState.Uninit();
		}

		if (csStateNew != null)
		{
			csStateNew.Init(Player);
		}
		m_csState = csStateNew;
	}

	//---------------------------------------------------------------------------------------------------
	void RestartAutoPlay()
	{
		if (m_csCreatureFarmQuestManager == null) return;

		Player.StartCoroutine(DelayStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csCreatureFarmQuestManager.Auto)
		{
			if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				m_csCreatureFarmQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsPlayThemeQuestCreatureFarm.RestartAutoPlay()");
				m_csState.SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsState
	{
		protected CsCreatureFarmQuestManager m_csCreatureFarmQuestManager;
		protected CsMyPlayer m_csPlayer;
		protected CsMyHeroInfo m_csMyHeroInfo;
		protected CsMyPlayer Player { get { return m_csPlayer; } }

		public virtual void Init(CsMyPlayer csPlayer)
		{
			m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
			m_csCreatureFarmQuestManager = CsCreatureFarmQuestManager.Instance;
			m_csPlayer = csPlayer;
		}

		public virtual void Uninit()
		{
			m_csMyHeroInfo = null;
			m_csCreatureFarmQuestManager = null;
			m_csPlayer = null;
		}

		public virtual void Reset(){}
		public virtual void Play(){}
		public virtual void SetDisplayPath(){}
		public virtual void Update(){}
		public virtual void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget){}
		public virtual void ResetInteractionData() {}
		public virtual void StateChangedToIdle() {}

		protected bool MovePlayer(CsNpcInfo csNpcInfo)
		{
			if (csNpcInfo != null)
			{
				if (MovePlayer(csNpcInfo.ContinentId, csNpcInfo.Position, csNpcInfo.InteractionMaxRange, true))
				{
					return true;
				}
				return false;
			}
			return true;
		}

		public bool MovePlayer(int nContinentId, Vector3 vtPosition, float flRange, bool bTargetNpc)
		{
			if (nContinentId == m_csMyHeroInfo.LocationId && m_csMyHeroInfo.InitEntranceLocationParam == m_csMyHeroInfo.Nation.NationId) // 대륙 및 국가 확인.)
			{
				if (Player.IsTargetInDistance(vtPosition, flRange))
				{
					return true;
				}

				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (Player.IsStateIdle)
					{						
						Player.MoveToPos(vtPosition, flRange, bTargetNpc);
					}
				}
			}
			else
			{
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					vtPosition = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinentId);

					if (Player.IsTargetInDistance(vtPosition, 2f) == false)
					{
						Player.MoveToPos(vtPosition, 2f, false);
					}
				}
			}
			return false;
		}

		public void NpcDialog(CsNpcInfo csNpcInfo)
		{
			if (csNpcInfo == null) return;
			if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
			{
				Debug.Log("CsPlayThemeQuest.NpcDialog      NpcId = " + csNpcInfo.NpcId);
				Player.SelectTarget(Player.FindNpc(csNpcInfo.Position), true);
				Player.transform.LookAt(csNpcInfo.Position);
				Player.IsQuestDialog = true;
				CsIngameData.Instance.IngameManagement.NpcDialog(csNpcInfo.NpcId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateAccept : CsState
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						m_csCreatureFarmQuestManager.AcceptReadyOK();
						NpcDialog(m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc);
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.Position);
				Player.SetWayPoint(m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.Position, EnWayPointType.Npc, m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (bMoveByTouchTarget)
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
				{
					if (Player.Target.CompareTag("Npc"))
					{
						if (m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc == null) return;

						if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.NpcId)
						{
							if (Player.IsTargetInDistance(m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.Position, m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc.InteractionMaxRange))
							{
								m_csCreatureFarmQuestManager.AcceptReadyOK();
								NpcDialog(m_csCreatureFarmQuestManager.CreatureFarmQuest.StartNpc);
							}
						}
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecute : CsState
	{
		protected CsCreatureFarmQuestMission m_csMission;

		public override void Init(CsMyPlayer csPlayer)
		{
			base.Init(csPlayer);
			m_csMission = m_csCreatureFarmQuestManager.HeroCreatureFarmQuest.CreatureFarmQuestMission;
			//Debug.Log("CsPlayThemeQuestCreatureFarm     CsStateExecute               MissionNo : " + m_csMission.MissionNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteMove : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csMission.ContinentTarget.ContinentId, m_csMission.TargetPosition, m_csMission.TargetRadius, false))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			Debug.Log("CsStateExecuteMove.SetDisplayPath()");
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMission.ContinentTarget.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMission.TargetPosition);
				Player.SetWayPoint(m_csMission.TargetPosition, EnWayPointType.Move, m_csMission.TargetRadius);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMission.ContinentTarget.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteInteraction : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csMission.ContinentTarget.ContinentId, m_csMission.TargetPosition, m_csMission.TargetRadius, false))
				{
					Player.ContinentObjectInteractionStart();
				}
			}
		}

		public override void SetDisplayPath()
		{
			Debug.Log("CsStateExecuteInteraction.SetDisplayPath()");
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMission.ContinentTarget.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMission.TargetPosition);
				Player.SetWayPoint(m_csMission.TargetPosition, EnWayPointType.Interaction, m_csMission.TargetRadius);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMission.ContinentTarget.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteBattle : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csMission.ContinentTarget.ContinentId, m_csMission.TargetPosition, m_csMission.TargetRadius, false))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			Debug.Log("CsStateExecuteBattle.SetDisplayPath()");
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMission.ContinentTarget.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMission.TargetPosition);
				Player.SetWayPoint(m_csMission.TargetPosition, EnWayPointType.Battle, m_csMission.TargetRadius);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMission.ContinentTarget.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateComplete : CsState
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						m_csCreatureFarmQuestManager.CompleteReadyOK();
						NpcDialog(m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc);
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.Position);
				Player.SetWayPoint(m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.Position, EnWayPointType.Npc, m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (bMoveByTouchTarget)
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
				{
					if (Player.Target.CompareTag("Npc"))
					{
						if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.NpcId)
						{
							if (Player.IsTargetInDistance(m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.Position, m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc.InteractionMaxRange))
							{
								m_csCreatureFarmQuestManager.CompleteReadyOK();
								NpcDialog(m_csCreatureFarmQuestManager.CreatureFarmQuest.CompletionNpc);
							}
						}
					}
				}
			}
		}
	}
}
