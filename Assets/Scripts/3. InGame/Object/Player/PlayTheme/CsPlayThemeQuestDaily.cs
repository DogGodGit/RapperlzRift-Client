using SimpleDebugLog;
using System;
using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestDaily : CsPlayThemeQuest
{
	bool m_bArrival = false;
	Guid m_guidAutoQuestId;

	CsDailyQuestManager m_csDailyQuestManager;
	CsHeroDailyQuest m_csHeroDailyQuest;

	Coroutine m_corReStartCoroutine;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("CsPlayThemeQuestDaily.ArrivalMoveToPos() ");
			DailyQuestPlay();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDailyQuestManager = CsDailyQuestManager.Instance;
		m_csDailyQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csDailyQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csDailyQuestManager.EventDailyQuestMissionImmediatlyComplete += OnEventDailyQuestMissionImmediatlyComplete;
		m_csDailyQuestManager.EventDailyQuestAbandon += OnEventDailyQuestAbandon;
		m_csDailyQuestManager.EventDailyQuestComplete += OnEventDailyQuestComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		ReStartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDailyQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csDailyQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csDailyQuestManager.EventDailyQuestMissionImmediatlyComplete -= OnEventDailyQuestMissionImmediatlyComplete;
		m_csDailyQuestManager.EventDailyQuestAbandon -= OnEventDailyQuestAbandon;
		m_csDailyQuestManager.EventDailyQuestComplete -= OnEventDailyQuestComplete;
		
		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

		m_csDailyQuestManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		Debug.Log("CsPlayThemeQuestDaily.StartAutoPlay1");
		base.StartAutoPlay();
		//m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	//protected override void UpdateAutoPlay()
	//{
	//}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Player.IsQuestDialog = false;

			if (bNotify)
			{
				Debug.Log("CsPlayThemeQuestDaily.StopAutoPlay");
				m_csDailyQuestManager.StopAutoPlay(this, m_guidAutoQuestId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("CsPlayThemeQuestDaily   >>>>  StartCoroutine(StateChangedToIdle())");
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
				Debug.Log("CsPlayThemeQuestDaily.CorutineStateChangedToIdle 2  IsThisAutoPlaying() = " + IsThisAutoPlaying());
				if (IsThisAutoPlaying())
				{
					Debug.Log("CsPlayThemeQuestDaily.CorutineStateChangedToIdle 3      m_bArrival = " + m_bArrival);
					DailyQuestPlay();
				}
			}
		}

		m_bArrival = false;
	}

	#endregion Override


	#region Event.DailyQuestManager
	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay(Guid guidAutoQuestId)
	{
		dd.d("CsPlayThemeQuestDaily.OnEventStopAutoPlay          guidAutoQuestId = " + guidAutoQuestId);
		if (IsThisAutoPlaying() == false)
		{
			Player.SetAutoPlay(this, true);
		}

		m_guidAutoQuestId = guidAutoQuestId;
		DailyQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, Guid guidAutoQuestId)
	{
		dd.d("CsPlayThemeQuestDaily.OnEventStopAutoPlay          guidAutoQuestId = "+ guidAutoQuestId);

		if (objCaller == this) return;
		if (IsThisAutoPlaying())
		{
			if (m_guidAutoQuestId == guidAutoQuestId)
			{
				Player.SetAutoPlay(null, false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDailyQuestMissionImmediatlyComplete()
	{
		if (IsThisAutoPlaying())
		{
			if (m_guidAutoQuestId == m_csDailyQuestManager.QuestId)
			{
				dd.d("CsPlayThemeQuestDaily.OnEventDailyQuestMissionImmediatlyComplete          m_guidAutoQuestId = " + m_guidAutoQuestId);
				Player.SetAutoPlay(null, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDailyQuestAbandon(int nSlotIndex)
	{
		if (IsThisAutoPlaying())
		{
			if (m_guidAutoQuestId == m_csDailyQuestManager.QuestId)
			{
				Player.SetAutoPlay(null, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDailyQuestComplete(bool bLevelUp, long lExp, int nSlotIndex)
	{
		if (IsThisAutoPlaying())
		{
			if (m_guidAutoQuestId == m_csDailyQuestManager.QuestId)
			{
				Player.SetAutoPlay(null, true);
			}
		}
	}

	#endregion Event.DailyQuestManager

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
		if (m_csDailyQuestManager.Auto)
		{
			m_corReStartCoroutine = Player.StartCoroutine(DelayReStart());
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayReStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		Debug.Log("CsPlayThemeQuestDaily.RestartAutoPlay()       m_csDailyQuestManager.Auto = " + m_csDailyQuestManager.Auto);
		DaliyQuestReStart();
	}

	//---------------------------------------------------------------------------------------------------
	void DaliyQuestReStart()
	{
		m_corReStartCoroutine = null;

		if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
		{
			m_csDailyQuestManager.StopAutoPlay(this, m_csDailyQuestManager.AutoQuestId);
			return;
		}

		if (IsThisAutoPlaying()) return;
		dd.d("DaliyQuestReStart()");
		m_guidAutoQuestId = m_csDailyQuestManager.AutoQuestId;
		Player.SetAutoPlay(this, true);
		DailyQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (m_csHeroDailyQuest == null) return;

		CsDailyQuestMission csDailyQuestMission = m_csHeroDailyQuest.DailyQuestMission;
		if (csDailyQuestMission != null)
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, csDailyQuestMission.TargetContinent.ContinentId);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				Player.DisplayPath.SetPath(vtPos);
			}
			else
			{
				dd.d("SetDisplayPath()     ", csDailyQuestMission.TargetContinent.ContinentId, m_csMyHeroInfo.LocationId);
				if (csDailyQuestMission.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
				{
					EnWayPointType enWayPointType = csDailyQuestMission.Type == 1 ? EnWayPointType.Battle : EnWayPointType.Interaction;
					Player.SetWayPoint(csDailyQuestMission.TargetPosotion, enWayPointType, csDailyQuestMission.TargetRadius);
					Player.DisplayPath.SetPath(csDailyQuestMission.TargetPosotion);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, csDailyQuestMission.TargetContinent.ContinentId);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
					Player.DisplayPath.SetPath(vtPos);					
				}
			}
		}
		else
		{
			Debug.Log("###############              SetDisplayPath                csDailyQuestMission = "+ csDailyQuestMission+"  //  수행할 퀘스트가 없음 문제있음");
		}
	}

	//---------------------------------------------------------------------------------------------------
	void DailyQuestPlay()
	{
		m_csHeroDailyQuest = m_csDailyQuestManager.GetHeroDailyQuest(m_guidAutoQuestId);

		if (m_csHeroDailyQuest != null)
		{
			if (m_csHeroDailyQuest.IsAccepted)
			{
				if (m_csHeroDailyQuest.Completed) return;															// 퀘스트 완료 상태.
				if (m_csHeroDailyQuest.MissionImmediateCompleted) return;											// 미션 즉시완료 상태.
				if (m_csHeroDailyQuest.ProgressCount >= m_csHeroDailyQuest.DailyQuestMission.TargetCount) return;   // 미션 완료 상태.
				
				SetDisplayPath();
			}
		}
	}
}
