using ClientCommon;
using SimpleDebugLog;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//---------------------------------------------------------------------------------------------------
// 작성 : 이민우 (2018-01-11) ㅋㅋ
//---------------------------------------------------------------------------------------------------
public class CsMainQuestDungeonManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bMainQuestDungeon = false;
	
	CsMainQuestDungeonStep m_csMainQuestDungeonStep;
	bool m_bMainQuestDungeonClear = false;

	//---------------------------------------------------------------------------------------------------
	public static CsMainQuestDungeonManager Instance
	{
		get { return CsSingleton<CsMainQuestDungeonManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
    public event Delegate EventContinentExitForMainQuestDungeonReEnter;		// 메인퀘스던전 재입장.

    public event Delegate EventMainQuestDungeonStartAutoPlay; // ig
    public event Delegate<object> EventMainQuestDungeonStopAutoPlay; // ui, ig
	public event Delegate<UnityAction> EventContinentExit;

    public event Delegate<bool> EventContinentExitForMainQuestDungeonEnter;
	public event Delegate<PDVector3 ,float , Guid > EventMainQuestDungeonEnter;
	public event Delegate<int, bool> EventMainQuestDungeonAbandon;
	public event Delegate<int, bool> EventMainQuestDungeonExit;
	public event Delegate EventMainQuestDungeonSaftyRevive;

	public event Delegate<PDMainQuestDungeonMonsterInstance[]> EventMainQuestDungeonStepStart;
    public event Delegate<bool, long, long> EventMainQuestDungeonStepCompleted;
	public event Delegate EventMainQuestDungeonFail;
	public event Delegate EventMainQuestDungeonClear;
	public event Delegate<int, bool> EventMainQuestDungeonBanished;
	public event Delegate<long, PDMainQuestDungeonSummonMonsterInstance[]> EventMainQuestDungeonMonsterSummon;

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeon MainQuestDungeon
	{
		get { return CsMainQuestManager.Instance.MainQuest.MainQuestDungeonTarget; }
	}

	public CsMainQuestDungeonStep MainQuestDungeonStep
	{
		get { return m_csMainQuestDungeonStep; }
	}

	public bool MainQuestDungeonClear
	{
		get { return m_bMainQuestDungeonClear; }
	}

	public bool Auto
	{
		get { return m_bAuto; }
	}

	public bool IsMainQuestDungeon
	{
		get { return m_bMainQuestDungeon; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeonManager()
	{
		UnInit();
		// Command
		CsRplzSession.Instance.EventResContinentExitForMainQuestDungeonEnter += OnEventResContinentExitForMainQuestDungeonEnter;
		CsRplzSession.Instance.EventResMainQuestDungeonEnter += OnEventResMainQuestDungeonEnter;
		CsRplzSession.Instance.EventResMainQuestDungeonAbandon += OnEventResMainQuestDungeonAbandon;
		CsRplzSession.Instance.EventResMainQuestDungeonExit += OnEventResMainQuestDungeonExit;
		CsRplzSession.Instance.EventResMainQuestDungeonSaftyRevive += OnEventResMainQuestDungeonSaftyRevive;

		// Event
		CsRplzSession.Instance.EventEvtMainQuestDungeonStepStart += OnEventEvtMainQuestDungeonStepStart;
		CsRplzSession.Instance.EventEvtMainQuestDungeonStepCompleted += OnEventEvtMainQuestDungeonStepCompleted;
		CsRplzSession.Instance.EventEvtMainQuestDungeonFail += OnEventEvtMainQuestDungeonFail;
		CsRplzSession.Instance.EventEvtMainQuestDungeonClear += OnEventEvtMainQuestDungeonClear;
		CsRplzSession.Instance.EventEvtMainQuestDungeonBanished += OnEventEvtMainQuestDungeonBanished;
		CsRplzSession.Instance.EventEvtMainQuestDungeonMonsterSummon += OnEventEvtMainQuestDungeonMonsterSummon;
	}

	public void UnInit()
	{
		m_bWaitResponse = false;
		m_bAuto = false;
		m_bMainQuestDungeon = false;

		m_csMainQuestDungeonStep = null;
		m_bMainQuestDungeonClear = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void StartAutoPlay()
    {
		Debug.Log("CsMainQuestDungeonManager.StartAutoPlay");
        if (m_bWaitResponse)
        {
            return;
        }

        if (m_bAuto == false)
        {
            m_bAuto = true;

            Debug.Log("EventStartAutoPlay: " + EventMainQuestDungeonStartAutoPlay);
            if (EventMainQuestDungeonStartAutoPlay != null)
            {
                EventMainQuestDungeonStartAutoPlay();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void StopAutoPlay(object objCaller)
    {
		dd.d("CsMainQuestDungeonManager.StopAutoPlay", m_bWaitResponse, m_bAuto);

        if (m_bAuto == true)
        {
            m_bAuto = false;
            if (EventMainQuestDungeonStopAutoPlay != null)
            {
                EventMainQuestDungeonStopAutoPlay(objCaller);
            }
        }
    }

	#region Public Event
	//---------------------------------------------------------------------------------------------------
	public void ContinentExit()
	{
		if (EventContinentExit != null)
		{
			EventContinentExit(SendContinentExitForMainQuestDungeonEnter);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventContinentExitForMainQuestDungeonReEnter()
	{
		if (EventContinentExitForMainQuestDungeonReEnter != null)
		{
			EventContinentExitForMainQuestDungeonReEnter();
		}
	}

	#endregion Public Event

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전입장을위한대륙퇴장
	public void SendContinentExitForMainQuestDungeonEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentExitForMainQuestDungeonEnterCommandBody cmdBody = new ContinentExitForMainQuestDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForMainQuestDungeonEnter, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResContinentExitForMainQuestDungeonEnter(int nReturnCode, ContinentExitForMainQuestDungeonEnterResponseBody responseBody)
	{
		Debug.Log("##########          OnEventResContinentExitForMainQuestDungeonEnter()     nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
        {
			m_bMainQuestDungeonClear = false; // 초기화.
			CsDungeonManager.Instance.DungeonPlay = EnDungeonPlay.MainQuest;

			bool bChangeScene = SceneManager.GetActiveScene().name == MainQuestDungeon.SceneName ? false : true;

			if (EventContinentExitForMainQuestDungeonEnter != null)
			{				
				EventContinentExitForMainQuestDungeonEnter(bChangeScene);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 입장할 수 없는 위치입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00301"));
			}
			else if (nReturnCode == 102)
			{
				// 죽은 상태입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00302"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			CsGameEventToUI.Instance.OnEventFade(false);
		}

	
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	//메인퀘스트던전입장
	public void SendMainQuestDungeonEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			MainQuestDungeonEnterCommandBody cmdBody = new MainQuestDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MainQuestDungeonEnter, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMainQuestDungeonEnter(int nReturnCode, MainQuestDungeonEnterResponseBody responseBody)
	{
		Debug.Log("##########          OnEventResMainQuestDungeonEnter()     nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
        {
			//responseBody.date  처리 필요.
			m_bMainQuestDungeon = true;
			CsGameData.Instance.MyHeroInfo.LocationId = MainQuestDungeon.LocationId; // 위치 정보 갱신.
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount; // 일일유료즉시부활횟수
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventMainQuestDungeonEnter != null)
			{
				EventMainQuestDungeonEnter(responseBody.position, responseBody.rotationY, responseBody.placeInstanceId);
			}
		}
        else if (nReturnCode == 101)
        {
            // 골드가 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00401"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	//메인퀘스트던전포기
	public void SendMainQuestDungeonAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			MainQuestDungeonAbandonCommandBody cmdBody = new MainQuestDungeonAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MainQuestDungeonAbandon, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMainQuestDungeonAbandon(int nReturnCode, MainQuestDungeonAbandonResponseBody responseBody)
	{
		Debug.Log("##########          OnEventResMainQuestDungeonAbandon()     nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
        {
			bool bChangeScene = CsGameData.Instance.GetContinent(responseBody.previousContinentId).SceneName == MainQuestDungeon.SceneName ? false : true;

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventMainQuestDungeonAbandon != null)
			{
				EventMainQuestDungeonAbandon(responseBody.previousContinentId, bChangeScene);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00502"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	//메인퀘스트던전퇴장
	public void SendMainQuestDungeonExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			MainQuestDungeonExitCommandBody cmdBody = new MainQuestDungeonExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MainQuestDungeonExit, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMainQuestDungeonExit(int nReturnCode, MainQuestDungeonExitResponseBody responseBody)
	{
		Debug.Log("##########          OnEventResMainQuestDungeonExit()     nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
        {
			bool bChangeScene = CsGameData.Instance.GetContinent(responseBody.previousContinentId).SceneName == MainQuestDungeon.SceneName ? false : true;

			m_bMainQuestDungeon = false;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventMainQuestDungeonExit != null)
			{
				EventMainQuestDungeonExit(responseBody.previousContinentId, bChangeScene);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00601"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	//메인퀘스트던전안전부활
	public void SendMainQuestDungeonSaftyRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			MainQuestDungeonSaftyReviveCommandBody cmdBody = new MainQuestDungeonSaftyReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MainQuestDungeonSaftyRevive, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMainQuestDungeonSaftyRevive(int nReturnCode, MainQuestDungeonSaftyReviveResponseBody responseBody)
	{
		Debug.Log("##########          OnEventResMainQuestDungeonSaftyRevive()     nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date; // 유료즉시부활시간.
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount; // 일일유료즉시부활횟수.

			if (EventMainQuestDungeonSaftyRevive != null)
			{
				EventMainQuestDungeonSaftyRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태가 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 부활대기시간이 경과하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00702"));
		}
		else if (nReturnCode == 103)
		{
			// 던전상태가 유효하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00703"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전단계시작
	void OnEventEvtMainQuestDungeonStepStart(SEBMainQuestDungeonStepStartEventBody eventBody)
	{
		Debug.Log("##########          OnEventEvtMainQuestDungeonStepStart()     eventBody.stepNo = " + eventBody.stepNo);
		m_csMainQuestDungeonStep = MainQuestDungeon.GetMainQuestDungeonStep(eventBody.stepNo); // 최초 스텝 입력
		if (EventMainQuestDungeonStepStart != null)
		{
			EventMainQuestDungeonStepStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전단계완료
	void OnEventEvtMainQuestDungeonStepCompleted(SEBMainQuestDungeonStepCompletedEventBody eventBody)
	{
		Debug.Log("##########          OnEventEvtMainQuestDungeonStepCompleted()     ");

        long lTempGold = CsGameData.Instance.MyHeroInfo.Gold;
        long lRewardGold = 0;

		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;

		// 최대골드
		CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
        CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
        bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

        if ((eventBody.gold - lTempGold) > 0)
        {
            lRewardGold = eventBody.gold - lTempGold;
        }

        if (EventMainQuestDungeonStepCompleted != null)
        {
            EventMainQuestDungeonStepCompleted(bLevelUp, lRewardGold, eventBody.acquiredExp);
        }
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전실패
	void OnEventEvtMainQuestDungeonFail(SEBMainQuestDungeonFailEventBody eventBody)
	{
		Debug.Log("##########          OnEventEvtMainQuestDungeonFail()     ");
		if (EventMainQuestDungeonFail != null)
		{
			EventMainQuestDungeonFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전클리어
	void OnEventEvtMainQuestDungeonClear(SEBMainQuestDungeonClearEventBody eventBody)
	{
		Debug.Log("##########          OnEventEvtMainQuestDungeonClear()     ");
		m_bMainQuestDungeonClear = true;
		if (EventMainQuestDungeonClear != null)
		{
			EventMainQuestDungeonClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전강퇴
	void OnEventEvtMainQuestDungeonBanished(SEBMainQuestDungeonBanishedEventBody eventBody)
    {
		Debug.Log("##########          OnEventEvtMainQuestDungeonBanished()     ");
		bool bChangeScene = CsGameData.Instance.GetContinent(eventBody.previousContinentId).SceneName == MainQuestDungeon.SceneName ? false : true;

		m_bMainQuestDungeon = false;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
        CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventMainQuestDungeonBanished != null)
		{
            EventMainQuestDungeonBanished(eventBody.previousContinentId, bChangeScene);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전몬스터소환
	void OnEventEvtMainQuestDungeonMonsterSummon(SEBMainQuestDungeonMonsterSummonEventBody eventBody)
	{
		Debug.Log("##########          OnEventEvtMainQuestDungeonMonsterSummon()     ");

		if (EventMainQuestDungeonMonsterSummon != null)
		{
			EventMainQuestDungeonMonsterSummon(eventBody.summonerInstanceId, eventBody.summonMonsterInsts);
		}
	}


	#endregion Protocol.Event
}
