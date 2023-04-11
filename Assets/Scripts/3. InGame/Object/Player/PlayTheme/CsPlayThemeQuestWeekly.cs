using SimpleDebugLog;
using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestWeekly : CsPlayThemeQuest
{
	bool m_bArrival = false;
	CsWeeklyQuestManager m_csWeeklyQuestManager;
	Coroutine m_corReStartCoroutine;
	
	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("CsPlayThemeQuestWeekly.ArrivalMoveToPos() ");
			WeeklyQuestPlay();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csWeeklyQuestManager = CsWeeklyQuestManager.Instance;
		m_csWeeklyQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csWeeklyQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csWeeklyQuestManager.EventWeeklyQuestRoundRefresh += OnEventWeeklyQuestRoundRefresh;
		m_csWeeklyQuestManager.EventWeeklyQuestRoundImmediatlyComplete += OnEventWeeklyQuestRoundImmediatlyComplete;
		m_csWeeklyQuestManager.EventWeeklyQuestRoundCompleted += OnEventWeeklyQuestRoundCompleted;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		ReStartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csWeeklyQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csWeeklyQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csWeeklyQuestManager.EventWeeklyQuestRoundRefresh -= OnEventWeeklyQuestRoundRefresh;
		m_csWeeklyQuestManager.EventWeeklyQuestRoundImmediatlyComplete -= OnEventWeeklyQuestRoundImmediatlyComplete;
		m_csWeeklyQuestManager.EventWeeklyQuestRoundCompleted -= OnEventWeeklyQuestRoundCompleted;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

		m_csWeeklyQuestManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		Debug.Log("CsPlayThemeQuestWeekly.StartAutoPlay1");
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
				Debug.Log("CsPlayThemeQuestWeekly.StopAutoPlay");
				m_csWeeklyQuestManager.StopAutoPlay(this);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("CsPlayThemeQuestWeekly   >>>>  StartCoroutine(StateChangedToIdle())");
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
					Debug.Log("CsPlayThemeQuestWeekly.CorutineStateChangedToIdle 3      m_bArrival = " + m_bArrival);
					WeeklyQuestPlay();
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

		WeeklyQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (objCaller == this) return;
		if (IsThisAutoPlaying())
		{
			Player.DisplayPath.ResetPath();
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundRefresh()
	{
		if (IsThisAutoPlaying())
		{
			WeeklyQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
	{
		if (IsThisAutoPlaying())
		{
			Player.DisplayPath.ResetPath();
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundCompleted(bool bLevelUp, long lAcquiredExp)
	{
		if (IsThisAutoPlaying())
		{
			Player.DisplayPath.ResetPath();
			Player.SetAutoPlay(null, false);
		}
	}

	#endregion Event.WeeklyQuestManager
	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{

		}
	}
	
	#endregion Event.Player

	//---------------------------------------------------------------------------------------------------
	void ReStartAutoPlay()
	{
		if (m_csWeeklyQuestManager.Auto)
		{
			m_corReStartCoroutine = Player.StartCoroutine(DelayReStart());
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayReStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		Debug.Log("CsPlayThemeQuestDaily.RestartAutoPlay()       m_csDailyQuestManager.Auto = " + m_csWeeklyQuestManager.Auto);
		WeeklyQuestReStart();
	}

	//---------------------------------------------------------------------------------------------------
	void WeeklyQuestReStart()
	{
		m_corReStartCoroutine = null;

		if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
		{
			m_csWeeklyQuestManager.StopAutoPlay(this);
			return;
		}

		if (IsThisAutoPlaying()) return;
		
		Player.SetAutoPlay(this, true);
		WeeklyQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath(int nContinentId, Vector3 vtTargetPos, float flTargetRadius, EnWayPointType enWayPointType)
	{
		if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
		{
			Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinentId);
			Player.SetWayPoint(vtPos, EnWayPointType.Move, 3f);
			Player.DisplayPath.SetPath(vtPos);
		}
		else
		{
			if (nContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.SetWayPoint(vtTargetPos, enWayPointType, flTargetRadius);
				Player.DisplayPath.SetPath(vtTargetPos);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, nContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 3f);
				Player.DisplayPath.SetPath(vtPos);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void WeeklyQuestPlay()
	{
		if (m_csWeeklyQuestManager.HeroWeeklyQuest == null) return;
		if (m_csWeeklyQuestManager.HeroWeeklyQuest.WeeklyQuestMission == null) return;

		CsWeeklyQuestMission csWeeklyQuestMission = m_csWeeklyQuestManager.HeroWeeklyQuest.WeeklyQuestMission;

		if (m_csWeeklyQuestManager.HeroWeeklyQuest.IsRoundAccepted)
		{
			EnWayPointType enWayPointType = EnWayPointType.None;
			switch (csWeeklyQuestMission.Type)
			{
				case 1: // 이동.
					enWayPointType = EnWayPointType.Move;
					break;
				case 2: // 몬스터 처치.
					enWayPointType = EnWayPointType.Battle;
					break;
				case 3: // 상호작용.
					enWayPointType = EnWayPointType.Interaction;
					break;
			}
			SetDisplayPath(csWeeklyQuestMission.TargetContinent.ContinentId, csWeeklyQuestMission.TargetPosition, csWeeklyQuestMission.TargetRadius, enWayPointType);
		}
	}
}
