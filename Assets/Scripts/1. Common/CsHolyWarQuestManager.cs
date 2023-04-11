using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using System;


public enum EnHolyWarQuestState { None, Accepted, Completed };
public class CsHolyWarQuestManager : MonoBehaviour
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;

	int m_nKillCount = 0;
	float m_flRemainingTime = 0f;

	DateTime m_dtDateHolyWarQuestStartCount;
	int[] m_anHolyWarQuestDailyStartCount;
	EnHolyWarQuestState m_enHolyWarQuestState = EnHolyWarQuestState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsHolyWarQuestManager Instance
	{
		get { return CsSingleton<CsHolyWarQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ig, ui
	public event Delegate EventUpdateState;
	public event Delegate EventNpctDialog;
	public event Delegate EventNationTransmission;

	public event Delegate EventHolyWarQuestAccept;
	public event Delegate<bool, long, int> EventHolyWarQuestComplete;
	public event Delegate EventHolyWarQuestUpdated;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public EnHolyWarQuestState HolyWarQuestState
	{
		get { return m_enHolyWarQuestState; }
	}

	public int KillCount
	{
		get { return m_nKillCount; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime - Time.realtimeSinceStartup; }
		set { m_flRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public DateTime HolyWarQuestStartCountDate
	{
		get { return m_dtDateHolyWarQuestStartCount; }
		set { m_dtDateHolyWarQuestStartCount = value; }
	}

	public int[] HolyWarQuestDailyStartCount
	{
		get { return m_anHolyWarQuestDailyStartCount; }
		set { m_anHolyWarQuestDailyStartCount = value; }
	}

	public CsHolyWarQuest HolyWarQuest
	{
		get { return CsGameData.Instance.HolyWarQuest; }
		set { CsGameData.Instance.HolyWarQuest = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroHolyWarQuest heroHolyWarQuest, int[] anDailyHolyWarQuestStartSchedules, DateTime dtDate)
	{
		UnInit();

		SetQuestInfo(heroHolyWarQuest, anDailyHolyWarQuestStartSchedules, dtDate);

		// Command
		CsRplzSession.Instance.EventResHolyWarQuestAccept += OnEventResHolyWarQuestAccept;
		CsRplzSession.Instance.EventResHolyWarQuestComplete += OnEventResHolyWarQuestComplete;

		// Event
		CsRplzSession.Instance.EventEvtHolyWarQuestUpdated += OnEventEvtHolyWarQuestUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResHolyWarQuestAccept -= OnEventResHolyWarQuestAccept;
		CsRplzSession.Instance.EventResHolyWarQuestComplete -= OnEventResHolyWarQuestComplete;

		// Event
		CsRplzSession.Instance.EventEvtHolyWarQuestUpdated -= OnEventEvtHolyWarQuestUpdated;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_anHolyWarQuestDailyStartCount = null;
		m_enHolyWarQuestState = EnHolyWarQuestState.None;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetQuestInfo(PDHeroHolyWarQuest heroHolyWarQuest, int[] adailyHolyWarQuestStartSchedules, DateTime dtDate)
	{
		if (heroHolyWarQuest == null)
		{
			m_enHolyWarQuestState = EnHolyWarQuestState.None;
			m_nKillCount = 0;
			m_flRemainingTime = 0;
		}
		else
		{
			m_nKillCount = heroHolyWarQuest.killCount;
			m_flRemainingTime = heroHolyWarQuest.remainingTime + Time.realtimeSinceStartup;

			if (m_flRemainingTime - Time.realtimeSinceStartup <= 0) // 남은 시간이 없으면 완료.
			{
				m_enHolyWarQuestState = EnHolyWarQuestState.Completed;
			}
			else
			{
				m_enHolyWarQuestState = EnHolyWarQuestState.Accepted;
			}
		}

		m_anHolyWarQuestDailyStartCount = adailyHolyWarQuestStartSchedules;
		m_dtDateHolyWarQuestStartCount = dtDate;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateHolyWarQuestState(bool bReset = false)
	{
		Debug.Log("UpdateHolyWarQuestState               ");
		if (bReset)
		{
			m_enHolyWarQuestState = EnHolyWarQuestState.None;
			m_nKillCount = 0;
			m_flRemainingTime = 0;
		}
		else
		{
			if (m_flRemainingTime - Time.realtimeSinceStartup <= 0) // 남은 시간이 없으면 완료.
			{
				m_enHolyWarQuestState = EnHolyWarQuestState.Completed;
			}
			else
			{
				m_enHolyWarQuestState = EnHolyWarQuestState.Accepted;
			}
		}

		if (EventUpdateState != null)
		{
			EventUpdateState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;

		if (!m_bAuto)
		{
			m_bAuto = true;

			if (EventStartAutoPlay != null)
			{
				EventStartAutoPlay();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		if (m_bAuto == true)
		{
			m_bAuto = false;

			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NpctDialog()
	{
		if (m_bWaitResponse) return;

		if (EventNpctDialog != null)
		{
			EventNpctDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NationTransmissionReadyOK()
	{
		if (m_bWaitResponse) return;

		if (EventNationTransmission != null)
		{
			EventNationTransmission();
		}
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendHolyWarQuestAccept() // 위대한성전퀘스트수락
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			HolyWarQuestAcceptCommandBody cmdBody = new HolyWarQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.HolyWarQuestAccept, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResHolyWarQuestAccept(int nReturnCode, HolyWarQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nKillCount = resBody.quest.killCount;
			m_flRemainingTime = resBody.quest.remainingTime + Time.realtimeSinceStartup;
			m_dtDateHolyWarQuestStartCount = resBody.date;
			UpdateHolyWarQuestState();

			if (EventHolyWarQuestAccept != null)
			{
				EventHolyWarQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A53_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 현재 퀘스트시간이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A53_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 퀘스트를 수락했습니다..
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A53_ERROR_00103"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendHolyWarQuestComplete() // 위대한성전퀘스트완료
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			HolyWarQuestCompleteCommandBody cmdBody = new HolyWarQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.HolyWarQuestComplete, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResHolyWarQuestComplete(int nReturnCode, HolyWarQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			UpdateHolyWarQuestState(true);
			long lAcquiredExp = resBody.acquiredExp; // 획득한 경험치
			int nAcquiredExploitPoint = resBody.acquiredExploitPoint; // 획득한 공적포인트

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.ExploitPoint = resBody.exploitPoint;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.ExploitPointDate = resBody.date; // 어디에 적용해야 하는 값인지 확인 필요.
			CsGameData.Instance.MyHeroInfo.DailyExploitPoint = resBody.dailyExploitPoint;

			if (EventHolyWarQuestComplete != null)
			{
				bool bLevelUp = (nOldLevel == resBody.level) ? false : true;
				EventHolyWarQuestComplete(bLevelUp, lAcquiredExp, nAcquiredExploitPoint);
			}
		}
		else if (nReturnCode == 101)
		{
			// 제한시간이 경과되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A53_ERROR_00201"));
		}
		else
		{
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHolyWarQuestUpdated(SEBHolyWarQuestUpdatedEventBody eventBody)
	{
		Debug.Log("OnEventEvtHolyWarQuestUpdated");
		m_nKillCount = eventBody.killCount;
		if (EventHolyWarQuestUpdated != null)
		{
			EventHolyWarQuestUpdated();
		}
	}

	#endregion Protocol.Event

	//---------------------------------------------------------------------------------------------------
	public bool CheckAvailability()
	{
		if (m_enHolyWarQuestState == EnHolyWarQuestState.None)	// 퀘스트 수락전.
		{
			for (int i = 0; i < HolyWarQuest.HolyWarQuestScheduleList.Count; i++)
			{
				System.TimeSpan tsCurrentTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime - CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date;

				if (HolyWarQuest.HolyWarQuestScheduleList[i].StartTime < tsCurrentTime.TotalSeconds &&
					HolyWarQuest.HolyWarQuestScheduleList[i].EndTime > tsCurrentTime.TotalSeconds)
				{
					for (int j = 0; j < m_anHolyWarQuestDailyStartCount.Length; j++)
					{
						if (HolyWarQuest.HolyWarQuestScheduleList[i].ScheduleId == m_anHolyWarQuestDailyStartCount[j])
						{
							return false;
						}
					}
					return true;
				}
			}
		}
		else
		{
			if (m_enHolyWarQuestState == EnHolyWarQuestState.Completed) return true;	// 퀘스트 미션완료.
		}
		return false;
	}
}
