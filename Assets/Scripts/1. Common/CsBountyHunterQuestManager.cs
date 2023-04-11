using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using SimpleDebugLog;

public class CsBountyHunterQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;

	CsBountyHunterQuest m_csBountyHunterQuest;
	int m_nItemGrade;
	int m_nProgressCount;

	DateTime m_dtDateBountyHunterQuestStartCount;
	int m_nBountyHunterQuestDailyStartCount;

	//---------------------------------------------------------------------------------------------------
	public static CsBountyHunterQuestManager Instance
	{
		get { return CsSingleton<CsBountyHunterQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ig, ui

	public event Delegate<bool, long> EventBountyHunterQuestComplete;
	public event Delegate EventBountyHunterQuestAbandon;
    public event Delegate EventBountyHunterQuestUpdated;

	//---------------------------------------------------------------------------------------------------
	public CsBountyHunterQuest BountyHunterQuest
	{
		get { return m_csBountyHunterQuest; }
	}

	public bool Auto
	{
		get { return m_bAuto; }
	}

	public int ItemGrade
	{
		get { return m_nItemGrade; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
	}

	public DateTime BountyHunterQuestStartCountDate
	{
		get { return m_dtDateBountyHunterQuestStartCount; }
		set { m_dtDateBountyHunterQuestStartCount = value; }
	}

	public int BountyHunterQuestDailyStartCount
	{
		get { return m_nBountyHunterQuestDailyStartCount; }
		set { m_nBountyHunterQuestDailyStartCount = value; }
	}
	
	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroBountyHunterQuest heroBountyHunterQuest, int nBountyHunterDailyStartCount, DateTime dtDate)
	{
		UnInit();

		SetQuestInfo(heroBountyHunterQuest, nBountyHunterDailyStartCount, dtDate);

		// Command
		CsRplzSession.Instance.EventResBountyHunterQuestComplete += OnEventResBountyHunterQuestComplete;
		CsRplzSession.Instance.EventResBountyHunterQuestAbandon += OnEventResBountyHunterQuestAbandon;

		// Event
		CsRplzSession.Instance.EventEvtBountyHunterQuestUpdated += OnEventEvtBountyHunterQuestUpdated;

	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResBountyHunterQuestComplete -= OnEventResBountyHunterQuestComplete;
		CsRplzSession.Instance.EventResBountyHunterQuestAbandon -= OnEventResBountyHunterQuestAbandon;

		// Event
		CsRplzSession.Instance.EventEvtBountyHunterQuestUpdated -= OnEventEvtBountyHunterQuestUpdated;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_csBountyHunterQuest = null;
		m_nItemGrade = 0;
		m_nProgressCount = 0;
		m_dtDateBountyHunterQuestStartCount = DateTime.Now;
		m_nBountyHunterQuestDailyStartCount = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetQuestInfo(PDHeroBountyHunterQuest heroBountyHunterQuest, int nBountyHunterDailyStartCount, DateTime dtDate)
	{
		if (heroBountyHunterQuest == null)
		{
			m_csBountyHunterQuest = null;
			m_nItemGrade = 0;
			m_nProgressCount = 0;
		}
		else
		{
			m_csBountyHunterQuest = CsGameData.Instance.GetBountyHunterQuest(heroBountyHunterQuest.questId);
			m_nItemGrade = heroBountyHunterQuest.itemGrade;
			m_nProgressCount = heroBountyHunterQuest.progressCount;
		}

		m_nBountyHunterQuestDailyStartCount = nBountyHunterDailyStartCount;
		m_dtDateBountyHunterQuestStartCount = dtDate;
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetBountyHunter()
	{
		Debug.Log("ResetBountyHunter()");
		m_csBountyHunterQuest = null;
		m_nItemGrade = 0;
		m_nProgressCount = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;
		if (m_csBountyHunterQuest == null) return; // 더이상 진행할 퀘스트가 없을때.

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
		dd.d("CsBountyHunterQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
		if (m_bAuto == true)
		{
			m_bAuto = false;

			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller);
			}
		}
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendBountyHunterQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			BountyHunterQuestCompleteCommandBody cmdBody = new BountyHunterQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BountyHunterQuestComplete, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResBountyHunterQuestComplete(int nReturnCode, BountyHunterQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			if (EventBountyHunterQuestComplete != null)
			{
				bool bLevelUp = (nOldLevel == resBody.level) ? false : true;
				EventBountyHunterQuestComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendBountyHunterQuestAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			BountyHunterQuestAbandonCommandBody cmdBody = new BountyHunterQuestAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BountyHunterQuestAbandon, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResBountyHunterQuestAbandon(int nReturnCode, BountyHunterQuestAbandonResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			SetQuestInfo(null, m_nBountyHunterQuestDailyStartCount, m_dtDateBountyHunterQuestStartCount);

			if (EventBountyHunterQuestAbandon != null)
			{
				EventBountyHunterQuestAbandon();
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtBountyHunterQuestUpdated(SEBBountyHunterQuestUpdatedEventBody eventBody)
	{
		m_nProgressCount = eventBody.progressCount;
        if (EventBountyHunterQuestUpdated != null)
        {
            EventBountyHunterQuestUpdated();
        }
	}

	#endregion Protocol.Event
}
