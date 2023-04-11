using SimpleDebugLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsPlayThemeQuestTrueHero : CsPlayThemeQuest
{
	CsTrueHeroQuestManager m_csTrueHeroQuestManager;
	bool m_bArrival = false;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			dd.d("1. CsplayThemeQuestTrueHero.ArrivalMoveToPos");
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
		m_csTrueHeroQuestManager = CsTrueHeroQuestManager.Instance;

		m_csTrueHeroQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csTrueHeroQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csTrueHeroQuestManager.EventTrueHeroQuestAccept += OnEventTrueHeroQuestAccept;
		m_csTrueHeroQuestManager.EventMyHeroTrueHeroQuestInteractionStart += OnEventMyHeroTrueHeroQuestStepInteractionStart;
		m_csTrueHeroQuestManager.EventTrueHeroQuestStepCompleted += OnEventTrueHeroQuestStepCompleted;
		m_csTrueHeroQuestManager.EventTrueHeroQuestComplete += OnEventTrueHeroQuestComplete;

		m_csTrueHeroQuestManager.EventTrueHeroQuestStepInteractionStart += OnEventTrueHeroQuestStepInteractionStart;
		m_csTrueHeroQuestManager.EventTrueHeroQuestStepInteractionCanceled += OnEventTrueHeroQuestStepInteractionCanceled;
		m_csTrueHeroQuestManager.EventTrueHeroQuestStepInteractionFinished += OnEventTrueHeroQuestStepInteractionFinished;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csTrueHeroQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csTrueHeroQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csTrueHeroQuestManager.EventTrueHeroQuestAccept -= OnEventTrueHeroQuestAccept;
		m_csTrueHeroQuestManager.EventMyHeroTrueHeroQuestInteractionStart -= OnEventMyHeroTrueHeroQuestStepInteractionStart;
		m_csTrueHeroQuestManager.EventTrueHeroQuestStepCompleted -= OnEventTrueHeroQuestStepCompleted;
		m_csTrueHeroQuestManager.EventTrueHeroQuestComplete -= OnEventTrueHeroQuestComplete;

		m_csTrueHeroQuestManager.EventTrueHeroQuestStepInteractionStart -= OnEventTrueHeroQuestStepInteractionStart;
		m_csTrueHeroQuestManager.EventTrueHeroQuestStepInteractionCanceled -= OnEventTrueHeroQuestStepInteractionCanceled;
		m_csTrueHeroQuestManager.EventTrueHeroQuestStepInteractionFinished -= OnEventTrueHeroQuestStepInteractionFinished;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		m_csTrueHeroQuestManager = null;
		base.Uninit();
	}

	#endregion Override

	#region Event.TrueHeroQuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			m_csTrueHeroQuestManager.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsplayThemeQuestTrueHero.OnEventStartAutoPlay()         TrueHeroQuestState = " + m_csTrueHeroQuestManager.TrueHeroQuestState);
		
		Player.SetAutoPlay(this, true);
		TrueHeroQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsplayThemeQuestTrueHero.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateState()
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsplayThemeQuestTrueHero.OnEventUpdateState()");
			TrueHeroQuestPlay();
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
	void OnEventTrueHeroQuestAccept()
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsplayThemeQuestTrueHero.OnEventTrueHeroQuestAccept()");
			TrueHeroQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroTrueHeroQuestStepInteractionStart()
	{
		Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
		Player.ResetBattleMode();

		if (Player.SkillStatus.IsStatusPlayAnim()) return;
		if (m_csTrueHeroQuestManager.Interacted) return;	// 이미 상호작용 진행했음.

		Debug.Log("2. CsplayThemeQuestTrueHero()OnEventMyHeroTrueHeroQuestStepInteractionStart");
		Player.ChangeState(CsHero.EnState.Interaction);
		m_csTrueHeroQuestManager.InteractionStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepCompleted()
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsplayThemeQuestTrueHero.OnEventTrueHeroQuestStepCompleted()");
			TrueHeroQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsplayThemeQuestTrueHero.OnEventTrueHeroQuestComplete()");
			Player.SetAutoPlay(null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepInteractionStart()
	{
		if (Player.State != CsHero.EnState.Interaction)
		{
			dd.d("CsplayThemeQuestTrueHero     >>    InteractionCancel ");
			Player.ChangeState(CsHero.EnState.Idle);
			CsIngameData.Instance.IngameManagement.StateEndOfInteraction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepInteractionCanceled()
	{
		Debug.Log("CsplayThemeQuestTrueHero.OnEventTrueHeroQuestStepInteractionCanceled()");
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepInteractionFinished()
	{
		Debug.Log("CsplayThemeQuestTrueHero.OnEventTrueHeroQuestStepInteractionFinished()");
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			CheckNpcDialog();
		}
	}

	#endregion Event
	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog()
	{
		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
		{
			if (m_csTrueHeroQuestManager.TrueHeroQuestState == EnTrueHeroQuestState.None || m_csTrueHeroQuestManager.TrueHeroQuestState == EnTrueHeroQuestState.Executed)
			{
				if (Player.IsTargetInDistance(m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc.Position, m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc.InteractionMaxRange))
				{
					if (m_csTrueHeroQuestManager.NpcDialogReadyOK())
					{
						NpcDialog(m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath(Vector3 vtTargetPos, EnWayPointType enWayPointType, float flRadius)
	{
		Player.DisplayPath.SetPath(vtTargetPos);
		Player.SetWayPoint(vtTargetPos, enWayPointType, flRadius);
	}

	//---------------------------------------------------------------------------------------------------
	void TrueHeroQuestPlay()
	{
		if (m_csTrueHeroQuestManager.IsCompleted)
		{
			Player.SetAutoPlay(null, true);
			return;
		}

		Vector3 vtTargetPos = Vector3.zero;
		EnWayPointType enWayPointType = EnWayPointType.Npc;
		float flRadius = 3f;

		if (m_csTrueHeroQuestManager.IsAccepted) // 수락 후
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam)  // 자기 국가일때는 적국으로 이동.
			{
				CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
				if (csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
				{
					vtTargetPos = csNpcInfo.Position;
					flRadius = csNpcInfo.InteractionMaxRange;
				}
				else
				{
					vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, csNpcInfo.ContinentId);
					enWayPointType = EnWayPointType.Move;
				}
			}
			else // 적국일때 미션 지역으로 이동.
			{
				if (m_csTrueHeroQuestManager.TrueHeroQuestStep.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
				{
					vtTargetPos = m_csTrueHeroQuestManager.TrueHeroQuestStep.TargetObjectPosition;
					enWayPointType = EnWayPointType.Interaction;
					flRadius = 30f; // 가져올 값이 없음.
				}
				else
				{
					vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csTrueHeroQuestManager.TrueHeroQuestStep.TargetContinent.ContinentId);
					enWayPointType = EnWayPointType.Move;
				}
			}
		}
		else if (m_csTrueHeroQuestManager.IsNone || m_csTrueHeroQuestManager.IsExecuted) // <수락전, 미션완료 후>. (본인 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >>  완료 Npc에게 이동.
			{
				if (m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc.ContinentId == m_csMyHeroInfo.LocationId)
				{
					vtTargetPos = m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc.Position;
					flRadius = m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc.InteractionMaxRange;
				}
				else
				{
					vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csTrueHeroQuestManager.TrueHeroQuest.QuestNpc.ContinentId);
					enWayPointType = EnWayPointType.Move;
				}
			}
			else
			{
				CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
				if (csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
				{
					vtTargetPos = csNpcInfo.Position;
				}
				else
				{
					vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, csNpcInfo.ContinentId);
					enWayPointType = EnWayPointType.Move;
				}
				flRadius = csNpcInfo.InteractionMaxRange;
			}
		}

		SetDisplayPath(vtTargetPos, enWayPointType, flRadius);
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
		if (m_csTrueHeroQuestManager.Auto)
		{
			if (Player.Dead || m_csTrueHeroQuestManager == null || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				Debug.Log("CsplayThemeQuestTrueHero.RestartAutoPlay()   >>  Stop");
				m_csTrueHeroQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsplayThemeQuestTrueHero.RestartAutoPlay()  >>  Satrt");
				Player.SetAutoPlay(this, true);
				TrueHeroQuestPlay();
			}
		}
	}
}
