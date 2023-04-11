using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestGuildHunting : CsPlayThemeQuest
{
	CsState m_csState;
	CsGuildManager m_csGuildManager;	
	bool m_bArrival = false;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestGuildHunting.ArrivalMoveToPos() ");
			if (m_csGuildManager.Guild != null)
			{
				PlayContinue();
			}
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);

		m_csGuildManager = CsGuildManager.Instance;
		m_csGuildManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csGuildManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csGuildManager.EventUpdateGuildHuntingQuestState += OnEventUpdateGuildHuntingQuestState;
		m_csGuildManager.EventGuildHuntingQuestAccept += OnEventGuildHuntingQuestAccept;
		m_csGuildManager.EventGuildHuntingQuestComplete += OnEventGuildHuntingQuestComplete;
		m_csGuildManager.EventGuildHuntingQuestAbandon += OnEventGuildHuntingQuestAbandon;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		SyncState();
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csGuildManager.EventUpdateGuildHuntingQuestState -= OnEventUpdateGuildHuntingQuestState;
		m_csGuildManager.EventGuildHuntingQuestAccept -= OnEventGuildHuntingQuestAccept;
		m_csGuildManager.EventGuildHuntingQuestComplete -= OnEventGuildHuntingQuestComplete;
		m_csGuildManager.EventGuildHuntingQuestAbandon -= OnEventGuildHuntingQuestAbandon;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

		m_csGuildManager = null;
		SetState(null);
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		Debug.Log("CsPlayThemeQuestGuildHunting.StartAutoPlay1");
		base.StartAutoPlay();
		//m_timer.Init(0.2f);
		SyncState();
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	//protected override void UpdateAutoPlay()
	//{
	//	if (m_csState != null)
	//	{
	//		m_csState.Update();
	//	}
	//}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Player.IsQuestDialog = false;

			if (bNotify)
			{
				m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.Hunting);
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
				Player.StartCoroutine(CorutineStateChangedToIdle());
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		yield return new WaitForSeconds(0.2f);
		if (m_bArrival == false)
		{
			if (Player.IsStateIdle)
			{				
				if (IsThisAutoPlaying())
				{
					Debug.Log("CsPlayThemeQuestGuildHunting.CorutineStateChangedToIdle 3      m_bArrival = " + m_bArrival);
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
		if (bMoveByTouchTarget)
		{
			if (Player.Target == null || m_csState == null) return;
			m_csState.ArrivalMoveByTouchByManual(bMoveByTouchTarget);
		}
	}

	#endregion Event.Player

	#region Event.GuildManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.Hunting)
		{
			Debug.Log("CsPlayThemeQuestGuildHunting.OnEventStartAutoPlay      enGuildPlayAutoState  =  " + enGuildPlayAutoState);
			Player.SetAutoPlay(this, true);
			SyncState();
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.Hunting)
		{
			if (!IsThisAutoPlaying()) return;
			Debug.Log(" CsPlayThemeQuestGuildHunting OnEventStopAutoPlay() ");
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateGuildHuntingQuestState()
	{
		SyncState();
		if (IsThisAutoPlaying())
		{
			Debug.Log(" CsPlayThemeQuestGuildHunting.OnEventUpdateGuildHuntingQuestState()   ");
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildHuntingQuestAccept()
	{
		SyncState();
		if (IsThisAutoPlaying())
		{
			Debug.Log(" CsPlayThemeQuestGuildHunting.OnEventGuildHuntingQuestAccept()   ");
			m_csState.SetDisplayPath();
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildHuntingQuestComplete()
	{
		SyncState();
		if (IsThisAutoPlaying())
		{
			Debug.Log(" CsPlayThemeQuestGuildHunting.OnEventGuildHuntingQuestComplete()   ");
			Player.SetAutoPlay(null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildHuntingQuestAbandon()
	{
		SyncState();
		if (IsThisAutoPlaying())
		{
			Debug.Log(" CsPlayThemeQuestGuildHunting.OnEventGuildHuntingQuestAbandon()   ");
			Player.SetAutoPlay(null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlayContinue()
	{
		Debug.Log("CsPlayThemeQuestGuildHunting.PlayContinue()");
		if (Player.SkillStatus.IsStatusPlayAnim()) return;

		if (m_csState != null)
		{
			m_csState.Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SyncState()
	{
		//Debug.Log("CsPlayThemeQuestGuildHunting.SyncState     m_csGuildManager.GuildHuntingState = " + m_csGuildManager.GuildHuntingState);
		switch (m_csGuildManager.GuildHuntingState)
		{
			case EnGuildHuntingState.None:
				SetState(new CsStateAccept());
				break;
			case EnGuildHuntingState.Accepted: // 미션 수락 완료 전.
				if (m_csGuildManager.GuildHuntingQuestObjective.Type == 1)
				{
					SetState(new CsStateExecuteBattle());
				}
				else if (m_csGuildManager.GuildHuntingQuestObjective.Type == 2)
				{
					SetState(new CsStateExecuteInteraction());
				}
				break;
			case EnGuildHuntingState.Executed: // 미션은 완료 완료 전.
				SetState(new CsStateComplete());
				break;
			case EnGuildHuntingState.Competed: // 모든 길드미션 퀘스트 완료.	
				Player.SetAutoPlay(null, true); // 오토 종료.
				SetState(null);
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
			if (m_csGuildManager.GuildPlayAutoState == EnGuildPlayState.Hunting)
			{
				if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
				{
					m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.Hunting);
				}
				else
				{
					Debug.Log("CsPlayThemeQuestGuildHunting.RestartAutoPlay()");
					m_csState.SetDisplayPath();
					Player.SetAutoPlay(this, true);
				}
			}
		}
	}

	#endregion Event.GuildManger
	//---------------------------------------------------------------------------------------------------
	class CsState
	{
		protected CsMyPlayer m_csPlayer;
		protected CsMyHeroInfo m_csMyHeroInfo;
		protected CsGuildManager m_csGuildManager;
		protected CsGuildHuntingQuestObjective m_csGuildHuntingQuestObjective;
		protected CsMyPlayer Player { get { return m_csPlayer; } }
		protected CsNpcInfo m_csNpcInfo;

		public virtual void Init(CsMyPlayer csPlayer)
		{
			m_csGuildManager = CsGuildManager.Instance;
			m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
			m_csPlayer = csPlayer;
			m_csGuildHuntingQuestObjective = m_csGuildManager.GuildHuntingQuestObjective;
			m_csNpcInfo = CsGameData.Instance.GetNpcInfo(m_csGuildManager.GuildHuntingQuest.QuestNpcId);
		}

		public virtual void Uninit() { }
		public virtual void SetDisplayPath() { }
		public virtual void Play() { }
		public virtual void Update() { }
		public virtual void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget) { }
		public virtual void ResetInteractionData() { }
		public virtual void StateChangedToIdle() { }

		protected void NpcDialog(CsNpcInfo csNpcInfo)
		{
			if (csNpcInfo == null) return;
			if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
			{
				Debug.Log("CsPlayThemeQuestGuildHunting.NpcDialog      NpcId = " + csNpcInfo.NpcId);
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
				if (MovePlayer(csNpcInfo.ContinentId, csNpcInfo.Position, csNpcInfo.InteractionMaxRange, true, EnWayPointType.Npc))
				{
					NpcDialog(csNpcInfo);
					return true;
				}
				return false;
			}
			return true;
		}

		protected bool MovePlayer(int nContinentId, Vector3 vtPosition, float flRange, bool bTargetNpc, EnWayPointType enWayPointType)
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
				vtPosition = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinentId);

				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (Player.IsStateIdle)
					{
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
				if (MovePlayer(m_csNpcInfo))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						Debug.Log("CsPlayThemeQuestGuildHunting.CsStateAccept.Play()");
						NpcDialog(m_csNpcInfo);
						m_csGuildManager.GuildHuntingNpcDialog();
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csNpcInfo.Position, EnWayPointType.Npc, m_csNpcInfo.InteractionMaxRange);
				Player.DisplayPath.SetPath(m_csNpcInfo.Position);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (m_csPlayer.Target.CompareTag("Npc"))
				{
					if (m_csNpcInfo.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
					{
						if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
						{
							if (m_csGuildManager.GuildHuntingNpcDialog())
							{
								NpcDialog(m_csNpcInfo);
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
	class CsStateExecuteBattle : CsState
	{
		public override void Play()
		{
			if (MovePlayer(m_csGuildHuntingQuestObjective.TargetContinent.ContinentId, m_csGuildHuntingQuestObjective.TargetPosition, m_csGuildHuntingQuestObjective.TargetRadius, false, EnWayPointType.Battle))
			{
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildHuntingQuestObjective.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(m_csGuildHuntingQuestObjective.TargetPosition, EnWayPointType.Battle, m_csGuildHuntingQuestObjective.TargetRadius);
				Player.DisplayPath.SetPath(m_csGuildHuntingQuestObjective.TargetPosition);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildHuntingQuestObjective.TargetContinent.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
			}
		}

		public override void Update()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (Player.State == CsHero.EnState.MoveToPos) return;
				if (MovePlayer(m_csGuildHuntingQuestObjective.TargetContinent.ContinentId, m_csGuildHuntingQuestObjective.TargetPosition, m_csGuildHuntingQuestObjective.TargetRadius, false, EnWayPointType.Battle))
				{
					Player.PlayBattle(m_csGuildHuntingQuestObjective.TargetMonster.MonsterId, m_csGuildHuntingQuestObjective.TargetPosition, m_csGuildHuntingQuestObjective.TargetRadius);
				}
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (m_csPlayer.Target.CompareTag("Npc"))
				{
					if (m_csNpcInfo.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
					{
						if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
						{
							if (m_csGuildManager.GuildHuntingNpcDialog())
							{
								Debug.Log("CsPlayThemeQuestGuildHunting   >>>   CsStateAccept.ArrivalMoveByTouchByManual()");
								NpcDialog(m_csNpcInfo);
							}
						}
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteInteraction : CsState
	{
		protected CsInteractionObject m_csInteractionObject;
		bool m_bInteractionComplete;
		Coroutine m_corPlayInteraction = null;

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
			if (MovePlayer(m_csGuildHuntingQuestObjective.TargetContinent.ContinentId, m_csGuildHuntingQuestObjective.TargetPosition, m_csGuildHuntingQuestObjective.TargetRadius, false, EnWayPointType.Interaction))
			{
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					Player.ContinentObjectInteractionStart();
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildHuntingQuestObjective.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csGuildHuntingQuestObjective.TargetPosition);
				Player.SetWayPoint(m_csGuildHuntingQuestObjective.TargetPosition, EnWayPointType.Interaction, m_csGuildHuntingQuestObjective.TargetRadius);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildHuntingQuestObjective.TargetContinent.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
			}
		}
		
		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (m_csPlayer.Target.CompareTag("Npc"))
			{
				if (m_csNpcInfo.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
				{
					if (Player.IsTargetInDistance(m_csNpcInfo.InteractionMaxRange))
					{
						if (m_csGuildManager.GuildHuntingNpcDialog())
						{
							Debug.Log("CsPlayThemeQuestGuildHunting   >>>   CsStateComplete.ArrivalMoveByTouchByManual()");
							NpcDialog(m_csNpcInfo);
						}
					}
				}
			}
			//else if (Player.Target.CompareTag("InteractionObject"))
			//{
			//    if (m_csGuildHuntingQuestObjective == null) return; // 모든 퀘스트 완료.

			//    CsInteractionObject csInteraction = Player.Target.GetComponent<CsInteractionObject>();
			//    CsContinentObject csContinentObject = CsGameData.Instance.GetContinentObject(m_csGuildHuntingQuestObjective.TargetContinentObjectId);

			//    if (csInteraction == null || csInteraction.ObjectId != m_csGuildHuntingQuestObjective.TargetContinentObjectId) return;
			//    if (!Player.IsTargetInDistance(csContinentObject.InteractionMaxRange)) return;

			//    Debug.Log("#####     CsPlayThemeQuestGuildHunting.ArrivalMoveByTouchByManual       Player.State = " + Player.State);
			//    m_csGuildManager.GuildHuntingInteractionStart(csInteraction.InstanceId);
			//}
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
				if (MovePlayer(m_csNpcInfo))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						Debug.Log("CsPlayThemeQuestGuildHunting   >>>>   CsStateComplete.Play()");
						NpcDialog(m_csNpcInfo);
						m_csGuildManager.GuildHuntingNpcDialog();
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csNpcInfo.Position);
				Player.SetWayPoint(m_csNpcInfo.Position, EnWayPointType.Npc, m_csNpcInfo.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4);
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (m_csPlayer.Target.CompareTag("Npc"))
				{
					if (m_csNpcInfo.NpcId == CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target))
					{
						if (Player.IsTargetInDistance(m_csNpcInfo.InteractionMaxRange))
						{
							if (m_csGuildManager.GuildHuntingNpcDialog())
							{
								Debug.Log("CsPlayThemeQuestGuildHunting   >>>   CsStateComplete.ArrivalMoveByTouchByManual()");
								NpcDialog(m_csNpcInfo);
							}
						}
					}
				}
			}
		}
	}
}
