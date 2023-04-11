using SimpleDebugLog;
using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestSub : CsPlayThemeQuest
{
	bool m_bArrival = false;
	CsSubQuestManager m_csSubQuestManager;
	Coroutine m_corReStartCoroutine;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("CsPlayThemeQuestSub.ArrivalMoveToPos() ");
			SubQuestPlay();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csSubQuestManager = CsSubQuestManager.Instance;
		m_csSubQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csSubQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csSubQuestManager.EventSubQuestProgressCountsUpdated += OnEventSubQuestProgressCountsUpdated;
		m_csSubQuestManager.EventSubQuestComplete += OnEventSubQuestComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		ReStartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csSubQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csSubQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csSubQuestManager.EventSubQuestProgressCountsUpdated -= OnEventSubQuestProgressCountsUpdated;
		m_csSubQuestManager.EventSubQuestComplete -= OnEventSubQuestComplete;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

		m_csSubQuestManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		Debug.Log("CsPlayThemeQuestSub.StartAutoPlay1");
		base.StartAutoPlay();
		//m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{

	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Player.IsQuestDialog = false;

			if (bNotify)
			{
				m_csSubQuestManager.StopAutoPlay(this);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Player.StartCoroutine(CorutineStateChangedToIdle());
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
					Debug.Log("CsPlayThemeQuestSub.CorutineStateChangedToIdle 3      m_bArrival = " + m_bArrival);
					SubQuestPlay();
				}
			}
		}

		m_bArrival = false;
	}
	#endregion Override

	#region Event.WeeklyQuestManager
	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (IsThisAutoPlaying() == false)
		{
			Player.SetAutoPlay(this, true);
		}

		SubQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, int nSubQuestId)
	{
		if (objCaller == this) return;
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestProgressCountsUpdated(int nQuestId)
	{
		if (IsThisAutoPlaying())
		{
			if (m_csSubQuestManager.AutoHeroSubQuest == null) return;
			if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest == null) return;
			if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.Type == 1) return; // 상호작용.

			SubQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestComplete(bool bLevelUp, long lAcquiredExp, int nQuestId)
	{
		if (m_csSubQuestManager.AutoHeroSubQuest == null) return;
		if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest == null) return;

		if (IsThisAutoPlaying())
		{
			if (m_csSubQuestManager.AutoHeroSubQuest == null || m_csSubQuestManager.AutoHeroSubQuest.SubQuest.QuestId == nQuestId)
			{
				Player.SetAutoPlay(null, false);
			}
		}
	}

	#endregion Event.WeeklyQuestManager

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget && Player.Target != null)
		{
			if (Player.Target.CompareTag("Npc"))
			{
				CheckNpcDialog(Player.Target);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog(Transform trNpc)
	{
		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
		{
			int nNpcId = (int)CsIngameData.Instance.IngameManagement.GetNpcId(trNpc);

			CsNpcInfo m_csNpcInfo = m_csSubQuestManager.GetSubQuestNpc(nNpcId);
			if (m_csNpcInfo != null)
			{
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					if (CsMainQuestManager.Instance.IsMainQuestNpc(m_csNpcInfo.NpcId)) return;

					if (m_csSubQuestManager.IsSubQuestNpc(nNpcId))
					{
						NpcDialog(m_csNpcInfo);
					}
				}
			}
		}
	}

	#endregion Event.Player

	//---------------------------------------------------------------------------------------------------
	void ReStartAutoPlay()
	{
		if (m_csSubQuestManager.Auto)
		{
			m_corReStartCoroutine = Player.StartCoroutine(DelayReStart());
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayReStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		Debug.Log("CsPlayThemeQuestSub.RestartAutoPlay()       CsPlayThemeQuestSub.Auto = " + m_csSubQuestManager.Auto);
		SubQuestReStart();
	}

	//---------------------------------------------------------------------------------------------------
	void SubQuestReStart()
	{
		m_corReStartCoroutine = null;

		if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
		{
			m_csSubQuestManager.StopAutoPlay(this);
			return;
		}

		if (IsThisAutoPlaying()) return;

		Player.SetAutoPlay(this, true);
		SubQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath(Vector3 vtTargetPos, EnWayPointType enWayPointType, float flRadius)
	{
		Player.DisplayPath.SetPath(vtTargetPos);
		Player.SetWayPoint(vtTargetPos, enWayPointType, flRadius);
	}

	//---------------------------------------------------------------------------------------------------
	void SubQuestPlay()
	{
		if (m_csSubQuestManager.AutoHeroSubQuest == null) return;
		if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest == null) return;

		Vector3 vtTargetPos = Vector3.zero;
		EnWayPointType enWayPointType = EnWayPointType.Npc;
		float flRadius = 3f;

		if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.TargetCount <= m_csSubQuestManager.AutoHeroSubQuest.ProgressCount)    // 미션 완료.
		{
			if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.CompletionNpc != null)
			{
				if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.CompletionNpc.ContinentId == m_csMyHeroInfo.LocationId)
				{
					vtTargetPos = m_csSubQuestManager.AutoHeroSubQuest.SubQuest.CompletionNpc.Position;
					flRadius = m_csSubQuestManager.AutoHeroSubQuest.SubQuest.CompletionNpc.InteractionMaxRange;
				}
				else
				{
					enWayPointType = EnWayPointType.Move;
					vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csSubQuestManager.AutoHeroSubQuest.SubQuest.CompletionNpc.ContinentId);
				}
			}
		}
		else
		{
			if (m_csSubQuestManager.AutoHeroSubQuest.EnStatus == EnSubQuestStatus.Abandon)
			{
				if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.StartNpc != null)
				{
					if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.StartNpc.ContinentId == m_csMyHeroInfo.LocationId)
					{
						vtTargetPos = m_csSubQuestManager.AutoHeroSubQuest.SubQuest.StartNpc.Position;
						flRadius = m_csSubQuestManager.AutoHeroSubQuest.SubQuest.StartNpc.InteractionMaxRange;
					}
					else
					{
						enWayPointType = EnWayPointType.Move;
						vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csSubQuestManager.AutoHeroSubQuest.SubQuest.StartNpc.ContinentId);
					}
				}
			}
			else if (m_csSubQuestManager.AutoHeroSubQuest.EnStatus == EnSubQuestStatus.Acception || m_csSubQuestManager.AutoHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
			{
				if (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
				{
					vtTargetPos = m_csSubQuestManager.AutoHeroSubQuest.SubQuest.TargetPosition;
					flRadius = m_csSubQuestManager.AutoHeroSubQuest.SubQuest.TargetRadius;

					switch (m_csSubQuestManager.AutoHeroSubQuest.SubQuest.Type)
					{
						case 1: // 상호작용.
							enWayPointType = EnWayPointType.Interaction;
							break;
						case 2: // 몬스터 처치.
							enWayPointType = EnWayPointType.Battle;
							break;
						case 3: // 몬스터 처치후 수집.
							enWayPointType = EnWayPointType.Battle;
							break;
						case 4: // 컨텐츠플레이
							enWayPointType = EnWayPointType.Npc;
							break;
					}
				}
				else
				{
					enWayPointType = EnWayPointType.Move;
					vtTargetPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, m_csSubQuestManager.AutoHeroSubQuest.SubQuest.TargetContinent.ContinentId);
				}
			}
		}

		SetDisplayPath(vtTargetPos, enWayPointType, flRadius);
	}
}
