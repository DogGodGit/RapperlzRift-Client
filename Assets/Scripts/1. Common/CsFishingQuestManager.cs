using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using SimpleDebugLog;

public class CsFishingQuestManager
{
    //낚시 최고 등급 아이템 ID 기본값
    const int m_nItemIdDefault = 1405;

    bool m_bWaitResponse = false;
	bool m_bFishing = false;
	int m_nFinshingBoxNo = 0;

	int m_nBaitItemId;
	int m_nCastingCount;

	DateTime m_dtDateFishingQuestStartCount;
	int m_nFishingQuestDailyStartCount;
	List<CsFishingArea> m_listFishingZones = new List<CsFishingArea>();

	//---------------------------------------------------------------------------------------------------
	public static CsFishingQuestManager Instance
	{
		get { return CsSingleton<CsFishingQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<CsNpcInfo> EventFishingNpcAutoMove;
	public event Delegate EventArrivalNpcDialog;
	public event Delegate EventFishingStart;
	public event Delegate<Transform> EventMyHeroFishingStart;
	// 당사자
	public event Delegate<bool, long, bool> EventFishingCastingCompleted;
	public event Delegate EventFishingCanceled;
	public event Delegate EventMyHeroFishingCanceled;

	// 당사자외
	public event Delegate<Guid> EventHeroFishingStarted;
	public event Delegate<Guid> EventHeroFishingCompleted;
	public event Delegate<Guid> EventHeroFishingCanceled;

	public event Delegate<bool, int> EventFishingZone;

	//---------------------------------------------------------------------------------------------------
	public bool Fishing
	{
		get { return m_bFishing; }
	}

	public int FinshingBoxNo
	{
		get { return m_nFinshingBoxNo; }
	}

	public int BaitItemId
	{
		get { return m_nBaitItemId; }
	}

	public int CastingCount
	{
		get { return m_nCastingCount; }
	}

	public DateTime FishingQuestStartCountDate
	{
		get { return m_dtDateFishingQuestStartCount; }
		set { m_dtDateFishingQuestStartCount = value; }
	}

	public int FishingQuestDailyStartCount
	{
		get { return m_nFishingQuestDailyStartCount; }
		set { m_nFishingQuestDailyStartCount = value; }
	}

	public List<CsFishingArea> FishingZones
	{
		get { return m_listFishingZones; }
		set { m_listFishingZones = value; }
	}
	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroFishingQuest heroFishingQuest, int nFishingQuestDailyStartCount, DateTime dtDate)
	{
		UnInit();
        SetQuestInfo(heroFishingQuest, nFishingQuestDailyStartCount, dtDate);

		CsRplzSession.Instance.EventResFishingStart += OnEventResFishingStart;

		CsRplzSession.Instance.EventEvtFishingCastingCompleted += OnEventEvtFishingCastingCompleted;
		CsRplzSession.Instance.EventEvtFishingCanceled += OnEventEvtFishingCanceled;
		CsRplzSession.Instance.EventEvtHeroFishingStarted += OnEventEvtHeroFishingStarted;
		CsRplzSession.Instance.EventEvtHeroFishingCompleted += OnEventEvtHeroFishingCompleted;
		CsRplzSession.Instance.EventEvtHeroFishingCanceled += OnEventEvtHeroFishingCanceled;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		CsRplzSession.Instance.EventResFishingStart -= OnEventResFishingStart;

		CsRplzSession.Instance.EventEvtFishingCastingCompleted -= OnEventEvtFishingCastingCompleted;
		CsRplzSession.Instance.EventEvtFishingCanceled -= OnEventEvtFishingCanceled;
		CsRplzSession.Instance.EventEvtHeroFishingStarted -= OnEventEvtHeroFishingStarted;
		CsRplzSession.Instance.EventEvtHeroFishingCompleted -= OnEventEvtHeroFishingCompleted;
		CsRplzSession.Instance.EventEvtHeroFishingCanceled -= OnEventEvtHeroFishingCanceled;

		m_bWaitResponse = false;
		m_bFishing = false;
		m_nFinshingBoxNo = 0;
		m_nBaitItemId = 0;
		m_nCastingCount = 0;
		m_dtDateFishingQuestStartCount = DateTime.Now;
		m_nFishingQuestDailyStartCount = 0;
		m_listFishingZones.Clear();
	}

	//---------------------------------------------------------------------------------------------------
	public void SetQuestInfo(PDHeroFishingQuest heroFishingQuest, int nFishingQuestDailyStartCount, DateTime dtDate)
	{
		if (heroFishingQuest == null)
		{
			m_nBaitItemId = 0;
			m_nCastingCount = 0;
        }
		else
		{
			m_nBaitItemId = heroFishingQuest.baitItemId;
            m_nCastingCount = heroFishingQuest.castingCount;
        }
        m_dtDateFishingQuestStartCount = dtDate;
        m_nFishingQuestDailyStartCount = nFishingQuestDailyStartCount;
    }

	//---------------------------------------------------------------------------------------------------
	public void FishingNpcAutoMove()
	{
		Debug.Log("CsFishingQuestManager.FishingNpcAutoMove()");
		if (EventFishingNpcAutoMove != null)
		{
			EventFishingNpcAutoMove(CsGameData.Instance.FishingQuest.NpcInfo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ArrivalNpcDialog()
	{
		Debug.Log("CsFishingQuestManager.ArrivalNpcDialog()");
		if (EventArrivalNpcDialog != null)
		{
            EventArrivalNpcDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void FishingZone(bool bEnter, int nSpotId, int nBoxNo)
	{
		Debug.Log("CsFishingQuestManager.FishingZone()  "+ bEnter+" // "+ m_nFinshingBoxNo +" >> "+ nBoxNo);
		if (bEnter)
		{
			m_nFinshingBoxNo = nBoxNo;
		}
		else
		{
			m_nFinshingBoxNo = 0;
		}
		
        if (EventFishingZone != null)
		{
			EventFishingZone(bEnter, nSpotId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void FishingCancel()
	{
		Debug.Log("#########              CsFishingQuestManager.FishingCancel()                #############");
		SendFishingCancel();
	}

	//---------------------------------------------------------------------------------------------------
	public void HeroFishingStart()
	{
		CsFishingArea csFishingArea = m_listFishingZones.Find(a => a.FishingBoxNo == m_nFinshingBoxNo);
        
		if (csFishingArea == null)
		{
			if (EventMyHeroFishingStart != null)
			{
				EventMyHeroFishingStart(null);
			}
		}
		else
        {
			if (EventMyHeroFishingStart != null)
            {
				EventMyHeroFishingStart(csFishingArea.transform);
			}
		}
	}

	#region public.Event

	//---------------------------------------------------------------------------------------------------
	void SendFishingCancel()
	{
		m_bFishing = false;
		CEBFishingCancelEventBody csEvt = new CEBFishingCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.FishingCancel, csEvt);

		if (EventMyHeroFishingCanceled != null)
		{
			EventMyHeroFishingCanceled();
		}
	}

	#endregion public.Event

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendFishingStart()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FishingStartCommandBody cmdBody = new FishingStartCommandBody();
			cmdBody.spotId = 1;
			CsRplzSession.Instance.Send(ClientCommandName.FishingStart, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResFishingStart(int nReturnCode, FishingStartResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResFishingStart");
		if (nReturnCode == 0)
		{
			m_bFishing = true;
			if (EventFishingStart != null)
			{
				EventFishingStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A46_ERROR_00201"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 탈것 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A46_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 다른 행동중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A46_ERROR_00204"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFishingCastingCompleted(SEBFishingCastingCompletedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		m_nCastingCount = eventBody.castingCount;
		long lAcquiredExp = eventBody.acquiredExp;
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;

		if (m_nCastingCount > CsGameData.Instance.FishingQuest.CastingCount - 1)
		{
			m_nBaitItemId = 0;
		}

		if (EventFishingCastingCompleted != null)
		{
			bool bLevelUp = (nOldLevel == eventBody.level) ? false : true;
            bool bBaitEnable = (m_nCastingCount < CsGameData.Instance.FishingQuest.CastingCount) ? true : false;
            m_bFishing = bBaitEnable;
            EventFishingCastingCompleted(bLevelUp, lAcquiredExp, bBaitEnable);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFishingCanceled(SEBFishingCanceledEventBody eventBody)
	{
		m_bFishing = false;
		if (EventFishingCanceled != null)
		{
			EventFishingCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroFishingStarted(SEBHeroFishingStartedEventBody eventBody) // 당사자외
	{
		Debug.Log("OnEventEvtHeroFishingStarted");
		if (EventHeroFishingStarted != null)
		{
			EventHeroFishingStarted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroFishingCompleted(SEBHeroFishingCompletedEventBody eventBody) // 당사자외
	{
		if (EventHeroFishingCompleted != null)
		{
			EventHeroFishingCompleted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroFishingCanceled(SEBHeroFishingCanceledEventBody eventBody) // 당사자외
	{
		if (EventHeroFishingCanceled != null)
		{
			EventHeroFishingCanceled(eventBody.heroId);
		}
	}

    #endregion Protocol.Event

}
