using SimpleDebugLog;
using System.Collections;
using UnityEngine;


public class CsPlayThemeQuestSecretLetter : CsPlayThemeQuest
{
	CsNpcInfo m_csNpcInfo;
	CsSecretLetterQuestManager m_csSecretLetterQuestManager;
	bool m_bArrival = false;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			dd.d("1. CsPlayThemeQuestSecretLetter.ArrivalMoveToPos");
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
		m_csSecretLetterQuestManager = CsSecretLetterQuestManager.Instance;
		m_csSecretLetterQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csSecretLetterQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csSecretLetterQuestManager.EventUpdateState += OnEventUpdateState;
		m_csSecretLetterQuestManager.EventMyHeroSecretLetterPickStart += OnEventMyHeroSecretLetterPickStart;

		m_csSecretLetterQuestManager.EventSecretLetterPickStart += OnEventSecretLetterPickStart;
		m_csSecretLetterQuestManager.EventSecretLetterPickCanceled += OnEventSecretLetterPickCanceled;
		m_csSecretLetterQuestManager.EventSecretLetterPickCompleted += OnEventSecretLetterPickCompleted;

		m_csSecretLetterQuestManager.EventSecretLetterQuestComplete += OnEventSecretLetterQuestComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventStateEndOfIdle += OnEventStateEndOfIdle;

		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csSecretLetterQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csSecretLetterQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csSecretLetterQuestManager.EventUpdateState -= OnEventUpdateState;
		m_csSecretLetterQuestManager.EventMyHeroSecretLetterPickStart -= OnEventMyHeroSecretLetterPickStart;

		m_csSecretLetterQuestManager.EventSecretLetterPickStart -= OnEventSecretLetterPickStart;
		m_csSecretLetterQuestManager.EventSecretLetterPickCanceled -= OnEventSecretLetterPickCanceled;
		m_csSecretLetterQuestManager.EventSecretLetterPickCompleted -= OnEventSecretLetterPickCompleted;

		m_csSecretLetterQuestManager.EventSecretLetterQuestComplete -= OnEventSecretLetterQuestComplete;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventStateEndOfIdle -= OnEventStateEndOfIdle;
		m_csSecretLetterQuestManager = null;
		m_csNpcInfo = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		//m_timer.Init(0.5f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		SecretLetterPlay();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Player.IsQuestDialog = false;
			Debug.Log("CsPlayThemeQuestSecretLetter.StopAutoPlay()     bNotify = " + bNotify);
			if (bNotify)
			{
				m_csSecretLetterQuestManager.StopAutoPlay(this);
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
					Debug.Log("CorutineStateChangedToIdle");
					SecretLetterPlay();
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
			m_csSecretLetterQuestManager.StopAutoPlay(this);
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
			Player.ChangeState(CsHero.EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroSecretLetterPickStart()
	{
		Player.ResetBattleMode();
		if (Player.SkillStatus.IsStatusPlayAnim()) return;

		Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
		m_csNpcInfo = m_csSecretLetterQuestManager.SecretLetterQuest.TargetNpcInfo;
		NpcDialog(m_csNpcInfo);
		Player.ChangeState(CsHero.EnState.Interaction);
		m_csSecretLetterQuestManager.SendSecretLetterPickStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSecretLetterPickStart()
	{
		if (m_csPlayer.State != CsHero.EnState.Interaction)
		{
			Player.ChangeState(CsHero.EnState.Idle);
			if (m_csSecretLetterQuestManager.SecretLetterPick)
			{
				m_csSecretLetterQuestManager.SendSecretLetterPickCancel();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSecretLetterPickCanceled()
	{
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSecretLetterPickCompleted()
	{
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSecretLetterQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
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

		if (m_csSecretLetterQuestManager.SecretLetterState != EnSecretLetterState.Accepted) // <수락전 or 완료>. (본인 국가로 이동)
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
			if (m_csSecretLetterQuestManager.TargetNationId != m_csMyHeroInfo.InitEntranceLocationParam) // 퀘스트 목적 국가가 아닐때 >> 국가이동 Npc에게 이동.
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
					EnWayPointType enWayPointType = m_csSecretLetterQuestManager.SecretLetterState == EnSecretLetterState.Accepted ? EnWayPointType.Interaction : EnWayPointType.Npc;
					Player.DisplayPath.SetPath(m_csNpcInfo.Position);
					Player.SetWayPoint(m_csNpcInfo.Position, enWayPointType, m_csNpcInfo.InteractionMaxRange);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csSecretLetterQuestManager.TargetNationId, m_csNpcInfo.ContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SecretLetterPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csSecretLetterQuestManager.SecretLetterPick) return; // 뽑기중.
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
				m_csSecretLetterQuestManager.NationTransmissionReadyOK();
				return;
			}
		}

		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 같은 대륙.
		{
			if (m_csSecretLetterQuestManager.SecretLetterState != EnSecretLetterState.Accepted)
			{
				m_csNpcInfo = m_csSecretLetterQuestManager.SecretLetterQuest.QuestNpcInfo;
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					if (m_csSecretLetterQuestManager.AcceptReadyOK())
					{
						NpcDialog(m_csNpcInfo);
					}
				}
			}
		}
		else // 다른 대륙.
		{
			if (m_csSecretLetterQuestManager.SecretLetterState == EnSecretLetterState.Accepted || m_csSecretLetterQuestManager.SecretLetterState == EnSecretLetterState.Executed)
			{
				m_csNpcInfo = m_csSecretLetterQuestManager.SecretLetterQuest.TargetNpcInfo;
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
					NpcDialog(m_csNpcInfo);
					m_csSecretLetterQuestManager.MissionReadyOK();
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo GetNpcInfo()
	{
		if (m_csSecretLetterQuestManager.SecretLetterState == EnSecretLetterState.Executed) // 미션 수락후 완료 전.
		{
			if (!m_csSecretLetterQuestManager.LowPickComplete)
			{
				if (m_csSecretLetterQuestManager.TargetNationId != m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >> 국가이동 Npc에게 이동.
				{
					return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
				}
				else
				{
					return CsGameData.Instance.SecretLetterQuest.TargetNpcInfo;
				}
			}
		}

		if (m_csSecretLetterQuestManager.SecretLetterState != EnSecretLetterState.Accepted) // <수락전 or 완료>. (본인 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return m_csSecretLetterQuestManager.SecretLetterQuest.QuestNpcInfo;
			}
		}
		else  // <미션중>.  (퀘스트 목적 국가로 이동)
		{
			if (m_csSecretLetterQuestManager.TargetNationId != m_csMyHeroInfo.InitEntranceLocationParam) // 퀘스트 목적 국가가 아닐때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return m_csSecretLetterQuestManager.SecretLetterQuest.TargetNpcInfo;
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
		if (m_csSecretLetterQuestManager.Auto)
		{
			if (Player.Dead || m_csSecretLetterQuestManager == null || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				m_csSecretLetterQuestManager.StopAutoPlay(this);
			}
			else
			{
				SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}
}
