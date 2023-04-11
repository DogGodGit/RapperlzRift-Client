using SimpleDebugLog;
using System.Collections;
using UnityEngine;


public class CsPlayThemeQuestMysteryBox : CsPlayThemeQuest
{
	CsNpcInfo m_csNpcInfo;
	CsMysteryBoxQuestManager m_csMysteryBoxQuestManager;
	bool m_bArrival = false;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			dd.d("1. CsPlayThemeQuestMysteryBox.ArrivalMoveToPos");
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
		m_csMysteryBoxQuestManager = CsMysteryBoxQuestManager.Instance;
		m_csMysteryBoxQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csMysteryBoxQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csMysteryBoxQuestManager.EventUpdateState += OnEventUpdateState;
		m_csMysteryBoxQuestManager.EventMyHeroMysteryBoxPickStart += OnEventMyHeroMysteryBoxPickStart;

		m_csMysteryBoxQuestManager.EventMysteryBoxPickStart += OnEventMysteryBoxPickStart;
		m_csMysteryBoxQuestManager.EventMysteryBoxPickCanceled += OnEventMysteryBoxPickCanceled;
		m_csMysteryBoxQuestManager.EventMysteryBoxPickCompleted += OnEventMysteryBoxPickCompleted;

		m_csMysteryBoxQuestManager.EventMysteryBoxQuestComplete += OnEventMysteryBoxQuestComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventStateEndOfIdle += OnEventStateEndOfIdle;
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csMysteryBoxQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csMysteryBoxQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csMysteryBoxQuestManager.EventUpdateState -= OnEventUpdateState;
		m_csMysteryBoxQuestManager.EventMyHeroMysteryBoxPickStart -= OnEventMyHeroMysteryBoxPickStart;

		m_csMysteryBoxQuestManager.EventMysteryBoxPickStart -= OnEventMysteryBoxPickStart;
		m_csMysteryBoxQuestManager.EventMysteryBoxPickCanceled -= OnEventMysteryBoxPickCanceled;
		m_csMysteryBoxQuestManager.EventMysteryBoxPickCompleted -= OnEventMysteryBoxPickCompleted;

		m_csMysteryBoxQuestManager.EventMysteryBoxQuestComplete -= OnEventMysteryBoxQuestComplete;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventStateEndOfIdle -= OnEventStateEndOfIdle;
		m_csMysteryBoxQuestManager = null;
		m_csNpcInfo = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		m_timer.Init(0.5f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		MysteryBoxPlay();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Player.IsQuestDialog = false;
			if (bNotify)
			{
				m_csMysteryBoxQuestManager.StopAutoPlay(this);
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
			CheckNpcDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStateEndOfIdle()
	{
		if (IsThisAutoPlaying())
		{
			Player.StartCoroutine(CorutineStateChangedToIdle());
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
					Debug.Log("CsPlayThemeQuestMysteryBox.CorutineStateChangedToIdle");
					MysteryBoxPlay();
				}
			}
		}
		m_bArrival = false;
	}

	#endregion Event.Player

	#region Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			m_csMysteryBoxQuestManager.StopAutoPlay(this);
			return;
		}

		SetDisplayPath();
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
	void OnEventUpdateState()
	{
		if (IsThisAutoPlaying())
		{
			SetDisplayPath();
			m_csPlayer.ChangeState(CsHero.EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroMysteryBoxPickStart()
	{
		Player.ResetBattleMode();
		if (Player.SkillStatus.IsStatusPlayAnim()) return;

		Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);

		m_csNpcInfo = m_csMysteryBoxQuestManager.MysteryBoxQuest.TargetNpcInfo;
		NpcDialog(m_csNpcInfo);
		Player.ChangeState(CsHero.EnState.Interaction);
		m_csMysteryBoxQuestManager.SendMysteryBoxPickStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMysteryBoxPickStart()
	{
		if (m_csPlayer.State != CsHero.EnState.Interaction)
		{
			Player.ChangeState(CsHero.EnState.Idle);

			if (CsMysteryBoxQuestManager.Instance.MysteryBoxPick) // 밀서 뽑기중
			{
				CsMysteryBoxQuestManager.Instance.SendMysteryBoxPickCancel();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMysteryBoxPickCanceled()
	{
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMysteryBoxPickCompleted()
	{
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMysteryBoxQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		Player.ChangeState(CsHero.EnState.Idle);

		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.DungeonManager
	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null) return;

		m_csNpcInfo = GetNpcInfo();

		if (m_csMysteryBoxQuestManager.MysteryBoxState != EnMysteryBoxState.Accepted) // <수락전 or 완료>.  (퀘스트 목적 국가로 이동)
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
					EnWayPointType enWayPointType = m_csMysteryBoxQuestManager.MysteryBoxState == EnMysteryBoxState.Accepted ? EnWayPointType.Interaction : EnWayPointType.Npc;
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
	void MysteryBoxPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csMysteryBoxQuestManager.MysteryBoxPick) return; // 뽑기중.
			if (m_csNpcInfo == null)
			{
				m_csNpcInfo = GetNpcInfo();
				return;
			}

			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csNpcInfo.ContinentId, m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange, true))
			{
				CheckNpcDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto && IsThisAutoPlaying())
		{
			m_csNpcInfo = GetNpcInfo();
			if (m_csNpcInfo.NpcType == EnNpcType.NationTransmission && Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange)) // 국가이동 Npc앞 도착.
			{
				NpcDialog(m_csNpcInfo);
				return;
			}
		}

		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam)
		{
			if (m_csMysteryBoxQuestManager.MysteryBoxState != EnMysteryBoxState.Accepted)
			{
				m_csNpcInfo = m_csMysteryBoxQuestManager.MysteryBoxQuest.QuestNpcInfo;
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					if (m_csMysteryBoxQuestManager.AcceptReadyOK())
					{
						NpcDialog(m_csNpcInfo);
					}
				}
			}
		}
		else
		{
			if (m_csMysteryBoxQuestManager.MysteryBoxState == EnMysteryBoxState.Accepted || m_csMysteryBoxQuestManager.MysteryBoxState == EnMysteryBoxState.Executed)
			{
				m_csNpcInfo = m_csMysteryBoxQuestManager.MysteryBoxQuest.TargetNpcInfo;

				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
					if (m_csMysteryBoxQuestManager.MissionReadyOK())
					{
						NpcDialog(m_csNpcInfo);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo GetNpcInfo()
	{
		if (m_csMysteryBoxQuestManager.MysteryBoxState == EnMysteryBoxState.Executed) // 미션 수락후 완료 전.
		{
			if (!m_csMysteryBoxQuestManager.LowPickComplete)
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >> 국가이동 Npc에게 이동.
				{
					return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
				}
				else
				{
					return CsGameData.Instance.MysteryBoxQuest.TargetNpcInfo;
				}
			}
		}

		if (m_csMysteryBoxQuestManager.MysteryBoxState != EnMysteryBoxState.Accepted) // <수락전 or 완료>.  (퀘스트 목적 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return CsGameData.Instance.MysteryBoxQuest.QuestNpcInfo;
			}
		}
		else  // <미션중>. (본인 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return CsGameData.Instance.MysteryBoxQuest.TargetNpcInfo;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RestartAutoPlay()
	{
		Player.StartCoroutine(DelayStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csMysteryBoxQuestManager.Auto)
		{
			if (Player.Dead || m_csMysteryBoxQuestManager == null || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				m_csMysteryBoxQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsPlayThemeQuestMysteryBox.RestartAutoPlay()");
				SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}
}
