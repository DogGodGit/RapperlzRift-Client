using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestThreatOfFarm : CsPlayThemeQuest
{
	CsState m_csState;	
	bool m_bPlayDelayedBySkill;
	CsThreatOfFarmQuestManager m_csThreatOfFarmQuestManager;
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
		base.Init(csPlayer);

		m_csThreatOfFarmQuestManager = CsThreatOfFarmQuestManager.Instance;
		m_csThreatOfFarmQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csThreatOfFarmQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csThreatOfFarmQuestManager.EventQuestAccepted += OnEventQuestAccepted;
		m_csThreatOfFarmQuestManager.EventMissionAbandoned += OnEventMissionAbandoned;
		m_csThreatOfFarmQuestManager.EventQuestComplete += OnEventQuestComplete;

		m_csThreatOfFarmQuestManager.EventMissionAccepted += OnEventMissionAccepted;
		m_csThreatOfFarmQuestManager.EventMissionComplete += OnEventMissionComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventAttackEnd += OnEventAttackEnd;

		SyncState();
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csThreatOfFarmQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csThreatOfFarmQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csThreatOfFarmQuestManager.EventQuestAccepted -= OnEventQuestAccepted;
		m_csThreatOfFarmQuestManager.EventMissionAbandoned -= OnEventMissionAbandoned;
		m_csThreatOfFarmQuestManager.EventQuestComplete -= OnEventQuestComplete;

		m_csThreatOfFarmQuestManager.EventMissionAccepted -= OnEventMissionAccepted;
		m_csThreatOfFarmQuestManager.EventMissionComplete -= OnEventMissionComplete;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventAttackEnd -= OnEventAttackEnd;

		m_csThreatOfFarmQuestManager = null;
		m_csState = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		m_timer.Init(0.2f);
		SyncState();
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsThreatOfFarmQuestManager.Instance.IsComplete)
		{
			Player.SetAutoPlay(null, true);
			return;
		}

		m_csState.Update();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			if (bNotify)
			{
				CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
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

	//---------------------------------------------------------------------------------------------------
	public void OnEventAttackEnd()
	{
		if (m_bPlayDelayedBySkill)
		{
			m_bPlayDelayedBySkill = false;
			PlayContinue();
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
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventQuestAccepted()
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMissionAbandoned()
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMissionAccepted()
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMissionComplete(bool bLevelUp, long lExp)
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventQuestComplete()
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
		if (Player.SkillStatus.IsStatusPlayAnim())
		{
			m_bPlayDelayedBySkill = true;
			return;
		}
		m_csState.Play();
	}

	//---------------------------------------------------------------------------------------------------
	void SyncState()
	{
		switch (CsThreatOfFarmQuestManager.Instance.QuestState)
		{
		case EnQuestState.None: 
			SetState(new CsStateAccept());
			break;

		case EnQuestState.Accepted:
			if (CsThreatOfFarmQuestManager.Instance.Mission == null)
			{
				SetState(new CsStateAccept());
			}
			else
			{
				SetState(new CsStateExecute());
			}
			break;

		default:
			SetState(new CsStateComplete());
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
		if (m_csThreatOfFarmQuestManager == null) return;

		Player.StartCoroutine(DelayStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csThreatOfFarmQuestManager.Auto)
		{
			if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				m_csThreatOfFarmQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsPlayThemeQuestThreatOfFarm.RestartAutoPlay()");
				m_csState.SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsState
	{
		protected CsThreatOfFarmQuestManager m_csFarmQuestManager;
		protected CsMyPlayer m_csPlayer;
		protected CsMyHeroInfo m_csMyHeroInfo;
		protected CsMyPlayer Player { get { return m_csPlayer; } }

		public virtual void Init(CsMyPlayer csPlayer)
		{
			m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
			m_csFarmQuestManager = CsThreatOfFarmQuestManager.Instance;
			m_csPlayer = csPlayer;
		}

		public virtual void Uninit()
		{
			m_csMyHeroInfo = null;
			m_csFarmQuestManager = null;
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
				if (MovePlayer(m_csFarmQuestManager.Quest.QuestNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						if (m_csFarmQuestManager.AcceptReadyOK())
						{
							NpcDialog(m_csFarmQuestManager.Quest.QuestNpc);
						}
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csFarmQuestManager.Quest.QuestNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csFarmQuestManager.Quest.QuestNpc.Position);
				Player.SetWayPoint(m_csFarmQuestManager.Quest.QuestNpc.Position, EnWayPointType.Npc, m_csFarmQuestManager.Quest.QuestNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csFarmQuestManager.Quest.QuestNpc.ContinentId);
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
						if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == m_csFarmQuestManager.Quest.QuestNpc.NpcId)
						{
							if (Player.IsTargetInDistance(m_csFarmQuestManager.Quest.QuestNpc.Position, m_csFarmQuestManager.Quest.QuestNpc.InteractionMaxRange))
							{
								if (m_csFarmQuestManager.AcceptReadyOK())
								{
									NpcDialog(m_csFarmQuestManager.Quest.QuestNpc);
								}
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
		enum EnStatus { MoveToPos, MoveToMonster, Battle }
		EnStatus m_enStatus;

		CsThreatOfFarmQuestMission m_csMission;

		public override void Init(CsMyPlayer csPlayer)
		{
			base.Init(csPlayer);
			m_csMission = m_csFarmQuestManager.Mission;
		}

		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (m_csMission != null)
				{
					if (MovePlayer(m_csMission.TargetContinent.ContinentId, m_csMission.TargetPosition, m_csMission.TargetRadius, false))
					{
						Player.ChangeState(CsHero.EnState.Idle);
					}
					m_enStatus = EnStatus.Battle;
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMission.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMission.TargetPosition);
				Player.SetWayPoint(m_csMission.TargetPosition, EnWayPointType.Battle, m_csMission.TargetRadius);
			}
			else
			{				
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMission.TargetContinent.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void Update()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (m_enStatus == EnStatus.Battle && Player.State != CsHero.EnState.MoveToPos)
				{
					CsTopqMonster csMon = m_csFarmQuestManager.Monster;
					if (csMon != null)
					{
						if (MovePlayer(m_csMission.TargetContinent.ContinentId, m_csMission.TargetPosition, m_csMission.TargetRadius, false))
						{
							if (!CsMonster.IsMonsterWithInstanceId(csMon.InstanceId, Player.TargetEnemy))
							{
								Transform tr = CsMyPlayer.FindTarget(csMon.Pos, csMon.Pos, CsMonster.c_strLayer, CsMonster.c_strTag, 5, -1, csMon.InstanceId);
								Player.SelectTargetEnemy(tr, true);
							}
							else
							{
								Player.PlayBattle();
							}
						}
					}
				}
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
				if (MovePlayer(m_csFarmQuestManager.Quest.QuestNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						NpcDialog(m_csFarmQuestManager.Quest.QuestNpc);
						m_csFarmQuestManager.CompleteReadyOK();
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csFarmQuestManager.Quest.QuestNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csFarmQuestManager.Quest.QuestNpc.Position);
				Player.SetWayPoint(m_csFarmQuestManager.Quest.QuestNpc.Position, EnWayPointType.Npc, m_csFarmQuestManager.Quest.QuestNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csFarmQuestManager.Quest.QuestNpc.ContinentId);
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
						if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == m_csFarmQuestManager.Quest.QuestNpc.NpcId)
						{
							if (Player.IsTargetInDistance(m_csFarmQuestManager.Quest.QuestNpc.Position, m_csFarmQuestManager.Quest.QuestNpc.InteractionMaxRange))
							{
								NpcDialog(m_csFarmQuestManager.Quest.QuestNpc);
								m_csFarmQuestManager.CompleteReadyOK();
							}
						}
					}
				}
			}
		}
	}
}
