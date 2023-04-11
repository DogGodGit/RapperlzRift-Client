using SimpleDebugLog;
using System;
using System.Collections;
using UnityEngine;

public enum EnMissionType { FindNpc = 1, BattleByMonster = 2, BattleBySummonedMonster = 3, GuildSpirit = 4 }

public class CsPlayThemeQuestGuildMission : CsPlayThemeQuest
{
	CsGuildManager m_csGuildManager;
	CsState m_csState;
	bool m_bPlayDelayedBySkill;
	bool m_bArrival = false;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		//dd.d("CsPlayThemeQuestGuildMission.ArrivalMoveToPos()");
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
		//dd.d("CsPlayThemeQuestGuildMission.Init  ");
		base.Init(csPlayer);
		m_csGuildManager = CsGuildManager.Instance;
		m_csGuildManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csGuildManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csGuildManager.EventUpdateMissionState += OnEventUpdateMissionState;
		m_csGuildManager.EventGuildMissionQuestAccept += OnEventGuildMissionQuestAccept;
		m_csGuildManager.EventGuildMissionAbandon += OnEventGuildMissionAbandon;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventAttackEnd += OnEventAttackEnd;

		SyncState();
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csGuildManager.EventUpdateMissionState -= OnEventUpdateMissionState;
		m_csGuildManager.EventGuildMissionQuestAccept -= OnEventGuildMissionQuestAccept;
		m_csGuildManager.EventGuildMissionAbandon -= OnEventGuildMissionAbandon;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventAttackEnd -= OnEventAttackEnd;

		m_csGuildManager = null;
		SetState(null);
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		//dd.d("StartAutoPlay1");
		base.StartAutoPlay();
		m_timer.Init(0.2f);
		SyncState();
		m_csState.SetDisplayPath();
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (m_csGuildManager.GuildMissionState == EnGuildMissionState.Competed)
		{
			Player.SetAutoPlay(null, true);
			return;
		}

		if (m_csState != null)
		{
			m_csState.Update();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Player.IsQuestDialog = false;
			Debug.Log("CsPlayThemeQuestGuildMission.StopAutoPlay() bNotify = " + bNotify);
			if (bNotify)
			{
				m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.Mission);
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
				if (m_csState != null)
				{
					//dd.d("CsPlayThemeQuestGuildMission.StateChangedToIdle");
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
				if (m_bArrival == false)
				{
					//dd.d("CorutineStateChangedToIdle        m_bArrival = " + m_bArrival);
					m_csState.StateChangedToIdle();
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
		if (!CsIngameData.Instance.IngameManagement.IsContinent()) return; // 대륙이 아닌경우.

		if (Player.Target == null || m_csState == null) return;
		m_csState.ArrivalMoveByTouchByManual(bMoveByTouchTarget);
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventAttackEnd()
	{
		if (m_bPlayDelayedBySkill)
		{
			//dd.d("CsPlayThemeQuestGuildMission.OnEventAttackEnd()");
			m_bPlayDelayedBySkill = false;
			if (IsThisAutoPlaying())
			{
				PlayContinue();
			}
		}
	}

	#endregion Event.Player

	#region Event.QuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.Mission)
		{
			//dd.d("CsPlayThemeQuestGuildMission.OnEventStartAutoPlay   enGuildPlayAutoState = " + enGuildPlayAutoState);
			if (Player.Dead)
			{
				m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.Mission);
				return;
			}

			Player.SetAutoPlay(this, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.Mission)
		{
			if (objCaller == this) return;
			if (IsThisAutoPlaying())
			{
				//dd.d("CsPlayThemeQuestGuildMission.OnEventStopAutoPlay");
				Player.SetAutoPlay(null, false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateMissionState()
	{
		//dd.d("1. CsPlayThemeQuestGuildMission.OnEventUpdateMissionState");
		SyncState();
		if (CsGuildManager.Instance.GuildMissionState == EnGuildMissionState.Competed)
		{
			if (IsThisAutoPlaying())
			{
				//dd.d("2. CsPlayThemeQuestGuildMission.OnEventUpdateMissionState");
				Player.SetAutoPlay(null, false);
			}
		}
		else
		{
			//dd.d("3. CsPlayThemeQuestGuildMission.OnEventUpdateMissionState");
			if (IsThisAutoPlaying())
			{
				m_csState.SetDisplayPath();
				PlayContinue();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildMissionQuestAccept()
	{
		//dd.d("CsPlayThemeQuestGuildMission.OnEventQuestAccepted");
		SyncState();
		if (IsThisAutoPlaying())
		{
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildMissionAbandon()
	{
		//dd.d("CsPlayThemeQuestGuildMission.OnEventGuildMissionAbandon"); 
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.QuestManager

	//---------------------------------------------------------------------------------------------------
	void PlayContinue()
	{
		//Debug.Log("CsPlayThemeQuestGuildMission.PlayContinue()   m_csState = " + m_csState);
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
		//Debug.Log("SyncState     m_csGuildManager.GuildMissionState = " + m_csGuildManager.GuildMissionState);
		switch (m_csGuildManager.GuildMissionState)
		{
			case EnGuildMissionState.None:
				SetState(new CsStateAccept());
				break;
			case EnGuildMissionState.Accepted: // 미션 수락 완료 전.
				if (m_csGuildManager.GuildMission != null)
				{
					//Debug.Log("SyncState     Accepted      m_csGuildManager.GuildMission.Type = " + (EnMissionType)m_csGuildManager.GuildMission.Type);
					switch ((EnMissionType)m_csGuildManager.GuildMission.Type)
					{
						case EnMissionType.FindNpc:
							SetState(new CsStateExecuteNpc());
							break;
						case EnMissionType.BattleByMonster:
							SetState(new CsStateExecuteBattle1());
							break;
						case EnMissionType.BattleBySummonedMonster:
							SetState(new CsStateExecuteBattle2());
							break;
						case EnMissionType.GuildSpirit:
							SetState(new CsStateExecuteGuildSpirit());
							break;
					}
				}
				else
				{
					SetState(new CsStateMissionAccept());
				}
				break;
			case EnGuildMissionState.Competed: // 모든 길드미션 퀘스트 완료.				
				Player.SetAutoPlay(null, true); // 오토 종료.
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
		if (m_csGuildManager == null) return;
		Player.StartCoroutine(DelayStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csGuildManager.Auto)
		{
			if (m_csGuildManager.GuildPlayAutoState == EnGuildPlayState.Mission)
			{
				if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
				{
					m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.Mission);
				}
				else
				{
					Debug.Log("CsPlayThemeQuestGuildMission.RestartAutoPlay()");
					m_csState.SetDisplayPath();
					Player.SetAutoPlay(this, true);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsState
	{
		protected CsMyPlayer m_csPlayer;
		protected CsMyHeroInfo m_csMyHeroInfo;
		protected CsGuildManager m_csGuildManager;
		protected CsMyPlayer Player { get { return m_csPlayer; } }

		public virtual void Init(CsMyPlayer csPlayer)
		{
			m_csGuildManager = CsGuildManager.Instance;
			m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
			m_csPlayer = csPlayer;
		}

		public virtual void Uninit(){}
		public virtual void Reset(){}
		public virtual void Play(){}
		public virtual void SetDisplayPath(){}
		public virtual void Update(){}
		public virtual void StateChangedToIdle(){}
		public virtual void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget){}

		//---------------------------------------------------------------------------------------------------
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

		protected bool MovePlayer(int nContinentId, Vector3 vtPosition, float flRange, bool bTargetNpc)
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
					if (Player.IsStateIdle)
					{
						vtPosition = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinentId);

						if (Player.IsTargetInDistance(vtPosition, 2f) == false)
						{
							Player.MoveToPos(vtPosition, 2f, false);
						}
					}
				}
			}
			return false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateAccept : CsState
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csGuildManager.GuildMissionQuest.StartNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						if (m_csGuildManager.MissionAcceptReadyOK())
						{
							//Debug.Log("CsPlayThemeQuestGuildMission.CsStateAccept.Play()");
							NpcDialog(m_csGuildManager.GuildMissionQuest.StartNpc);
						}
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildManager.GuildMissionQuest.StartNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csGuildManager.GuildMissionQuest.StartNpc.Position, EnWayPointType.Npc, m_csGuildManager.GuildMissionQuest.StartNpc.InteractionMaxRange);
				Player.DisplayPath.SetPath(m_csGuildManager.GuildMissionQuest.StartNpc.Position);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildManager.GuildMissionQuest.StartNpc.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
				Player.DisplayPath.SetPath(vtPos);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (bMoveByTouchTarget)
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
				{
					if (m_csPlayer.Target.CompareTag("Npc"))
					{
						if (m_csGuildManager.GuildMissionQuest.StartNpc.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
						{
							if (Player.IsTargetInDistance(m_csGuildManager.GuildMissionQuest.StartNpc.InteractionMaxRange))
							{
								if (m_csGuildManager.MissionAcceptReadyOK())
								{
									Debug.Log("CsPlayThemeQuestGuildMission.CsStateAccept.ArrivalMoveByTouchByManual()");
									NpcDialog(m_csGuildManager.GuildMissionQuest.StartNpc);
								}
							}
						}
					}
				}
			}
		}

		public override void StateChangedToIdle()
		{
			//dd.d("StateChangedToIdle    CsStateExecuteInteraction####################################### ");
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateMissionAccept : CsState // 퀘스트는 수락 >> 미션은 수락전.
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (m_csGuildManager.GuildMissionState == EnGuildMissionState.Accepted) // 자동전투 미션 완료가 아닐때.
				{
					if (MovePlayer(m_csGuildManager.GuildMissionQuest.StartNpc))
					{
						if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
						{
							//Debug.Log("2 CsPlayThemeQuestGuildMission.CsStateMissionAccept.Play()");
							NpcDialog(m_csGuildManager.GuildMissionQuest.StartNpc);
							m_csGuildManager.MissionAcceptReadyOK();
						}
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildManager.GuildMissionQuest.StartNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csGuildManager.GuildMissionQuest.StartNpc.Position, EnWayPointType.Npc, m_csGuildManager.GuildMissionQuest.StartNpc.InteractionMaxRange);
				Player.DisplayPath.SetPath(m_csGuildManager.GuildMissionQuest.StartNpc.Position);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildManager.GuildMissionQuest.StartNpc.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
				Player.DisplayPath.SetPath(vtPos);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (bMoveByTouchTarget)
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
				{
					if (m_csPlayer.Target.CompareTag("Npc"))
					{
						if (m_csGuildManager.GuildMissionQuest.StartNpc.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
						{
							if (Player.IsTargetInDistance(m_csGuildManager.GuildMissionQuest.StartNpc.InteractionMaxRange))
							{
								Debug.Log("CsPlayThemeQuestGuildMission.CsStateMissionAccept.ArrivalMoveByTouchByManual()");
								NpcDialog(m_csGuildManager.GuildMissionQuest.StartNpc);
								m_csGuildManager.MissionAcceptReadyOK();
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
	class CsStateExecute : CsState
	{
		bool m_bModeBattle;
		protected CsGuildMission m_csGuildMission;
			
		public override void Init(CsMyPlayer csPlayer)
		{
			base.Init(csPlayer);
			m_csGuildMission = m_csGuildManager.GuildMission;
		}
	}

	// 1 : NPC찾기
	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteNpc : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csGuildMission.TargetNpc.ContinentId, m_csGuildMission.TargetNpc.Position, m_csGuildMission.TargetNpc.InteractionMaxRange, true))
				{
					NpcDialog(m_csGuildMission.TargetNpc);
					m_csGuildManager.MissionNpcDialog();
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildMission.TargetNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csGuildMission.TargetNpc.Position, EnWayPointType.Npc, m_csGuildMission.TargetNpc.InteractionMaxRange);
				Player.DisplayPath.SetPath(m_csGuildMission.TargetNpc.Position);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildMission.TargetNpc.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
				Player.DisplayPath.SetPath(vtPos);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (bMoveByTouchTarget)
			{
				if (m_csPlayer.Target.CompareTag("Npc"))
				{
					//Debug.Log("CsPlayThemeQuestGuildMission.CsStateExecuteNpc.ArrivalMoveByTouchByManual()       >>>>       TargetNpc    ");
					if (m_csGuildMission.TargetNpc.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
					{
						if (Player.IsTargetInDistance(m_csGuildMission.TargetNpc.Position, m_csGuildMission.TargetNpc.InteractionMaxRange))
						{
							Debug.Log("CsPlayThemeQuestGuildMission.CsStateMissionAccept.ArrivalMoveByTouchByManual()");
							NpcDialog(m_csGuildMission.TargetNpc);
							m_csGuildManager.MissionNpcDialog();
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

	// 2 : 몬스터처치
	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteBattle1 : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csGuildMission.TargetContinent.ContinentId, m_csGuildMission.TargetPosition, m_csGuildMission.TargetRadius, false))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildMission.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csGuildMission.TargetPosition, EnWayPointType.Battle, m_csGuildMission.TargetRadius);
				Player.DisplayPath.SetPath(m_csGuildMission.TargetPosition);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildMission.TargetContinent.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
				Player.DisplayPath.SetPath(vtPos);
			}
		}

		public override void Update()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (Player.State == CsHero.EnState.MoveToPos) return;
				if (MovePlayer(m_csGuildMission.TargetContinent.ContinentId, m_csGuildMission.TargetPosition, m_csGuildMission.TargetRadius, false))
				{
					Player.PlayBattle(m_csGuildMission.TargetMonster.MonsterId, m_csGuildMission.TargetPosition, m_csGuildMission.TargetMonster.QuestAreaRadius);
				}
			}
		}
	}

	// 3 : 소환몬스터처치
	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteBattle2 : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csGuildManager.GuildMissionMonster.ContinentId, m_csGuildManager.GuildMissionMonster.Position, m_csGuildMission.TargetRadius, false))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildManager.GuildMissionMonster.ContinentId == m_csMyHeroInfo.LocationId)
			{				
				Player.SetWayPoint(m_csGuildManager.GuildMissionMonster.Position, EnWayPointType.Battle, m_csGuildMission.TargetRadius);
				Player.DisplayPath.SetPath(m_csGuildManager.GuildMissionMonster.Position);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildManager.GuildMissionMonster.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
				Player.DisplayPath.SetPath(vtPos);
			}
		}

		public override void Update()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (Player.State == CsHero.EnState.MoveToPos) return;
				if (MovePlayer(m_csGuildManager.GuildMissionMonster.ContinentId, m_csGuildManager.GuildMissionMonster.Position, m_csGuildMission.TargetRadius, false))
				{
					Player.PlayBattle(m_csGuildManager.GuildMissionMonster.InstanceId, m_csGuildManager.GuildMissionMonster.Position, m_csGuildMission.TargetRadius);
				}
			}
		}
	}

	// 4 : 길드정신알리기
	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteGuildSpirit : CsStateExecute
	{
		public override void Init(CsMyPlayer csPlayer)
		{
			base.Init(csPlayer);
		}

		public override void Uninit()
		{
			base.Uninit();
		}

		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csGuildMission.TargetContinent.ContinentId, m_csGuildMission.TargetPosition, m_csGuildMission.TargetRadius, false))
				{
					Player.ContinentObjectInteractionStart();
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildMission.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csGuildMission.TargetPosition, EnWayPointType.Interaction, m_csGuildMission.TargetRadius);
				Player.DisplayPath.SetPath(m_csGuildMission.TargetPosition);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildMission.TargetContinent.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
				Player.DisplayPath.SetPath(vtPos);
			}
		}
		
		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (bMoveByTouchTarget)
			{
				if (m_csGuildManager.GuildMission == null) return;          // 모든 퀘스트 완료.
				if (Player.Target == null) return;
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}
}
