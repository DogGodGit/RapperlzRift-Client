using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-18)
//---------------------------------------------------------------------------------------------------

public class CsPanelHUD : MonoBehaviour
{
    [SerializeField] GameObject m_goHeroHUD;
    [SerializeField] GameObject m_goNpcHUD;
    [SerializeField] GameObject m_goMonsterHUD;
    [SerializeField] GameObject m_goCartHUD;

    Transform m_trMyHero;
    Transform m_trHero;
    Transform m_trNpc;
    Transform m_trMonster;
    Transform m_trCart;

    //float m_flTime = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trMyHero = transform.Find("MyHero");
        m_trHero = transform.Find("Hero");
        m_trNpc = transform.Find("Npc");
        m_trMonster = transform.Find("Monster");
        m_trCart = transform.Find("Cart");

		CsGameEventUIToUI.Instance.EventHeroObjectGot += OnEventHeroObjectGot;

        CsGameEventUIToUI.Instance.EventDeleteAllHUD += OnEventDeleteAllHUD;
        CsGameEventUIToUI.Instance.EventChattingMessageReceived += OnEventChattingMessageReceived;

        CsGameEventToUI.Instance.EventCreateHeroHUD += OnEventCreateHeroHUD;
        CsGameEventToUI.Instance.EventDeleteHeroHUD += OnEventDeleteHeroHUD;

        CsGameEventToUI.Instance.EventCreateNpcHUD += OnEventCreateNpcHUD;
        CsGameEventToUI.Instance.EventDeleteNpcHUD += OnEventDeleteNpcHUD;

        CsGameEventToUI.Instance.EventCreateMonsterHUD += OnEventCreateMonsterHUD;
        CsGameEventToUI.Instance.EventDeleteMonsterHUD += OnEventDeleteMonsterHUD;
		CsDungeonManager.Instance.EventCreateMonsterHUD += OnEventCreateDungeonMonsterHUD;
        CsDungeonManager.Instance.EventDeleteMonsterHUD += OnEventDeleteDungeonMonsterHUD;
		
        CsRplzSession.Instance.EventEvtMonsterHit += OnEventEvtMonsterHit;

        CsGameEventToUI.Instance.EventCreateCartHUD += OnEventCreateCartHUD;
        CsGameEventToUI.Instance.EventDeleteCartHUD += OnEventDeleteCartHUD;

        CsGameEventToUI.Instance.EventCreateGuildNpcHUD += OnEventCreateGuildNpcHUD;
        CsGameEventToUI.Instance.EventDeleteGuildNpcHUD += OnEventDeleteGuildNpcHUD;

        CsGameEventToUI.Instance.EventCreateNationWarNpcHUD += OnEventCreateNationWarNpcHUD;
        CsGameEventToUI.Instance.EventDeleteNationWarNpcHUD += OnEventDeleteNationWarNpcHUD;

        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCompleted += OnEventSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestComplete += OnEventSecretLetterQuestComplete;
        CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickCompleted += OnEventHeroSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventHeroSecretLetterQuestCompleted += OnEventHeroSecretLetterQuestCompleted;

        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCompleted += OnEventMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestComplete += OnEventMysteryBoxQuestComplete;
        CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickCompleted += OnEventHeroMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxQuestCompleted += OnEventHeroMysteryBoxQuestCompleted;

        CsDungeonManager.Instance.EventCreateUndergroundMazeNpcHUD += OnEventCreateUndergroundMazeNpcHUD;

        // 차원 왜곡
        CsGameEventUIToUI.Instance.EventDistortionScrollUse += OnEventDistortionScrollUse;
        CsGameEventUIToUI.Instance.EventDistortionCanceled += OnEventDistortionCanceled;

        // 차원 왜곡 서버 이벤트
        CsRplzSession.Instance.EventEvtHeroDistortionStarted += OnEventEvtHeroDistortionStarted;
        CsRplzSession.Instance.EventEvtHeroDistortionFinished += OnEventEvtHeroDistortionFinished;
        CsRplzSession.Instance.EventEvtHeroDistortionCanceled += OnEventEvtHeroDistortionCanceled;

        //자신 랭크 상승
        CsGameEventUIToUI.Instance.EventRankAcquire += OnEventRankAcquire;

        // 영웅국가관직변경
        //CsRplzSession.Instance.EventEvtNationNoblesseAppointment += OnEventEvtNationNoblesseAppointment;
        //CsGameEventUIToUI.Instance.EventNationNoblesseDismissed += OnEventNationNoblesseDismissed;

        //칭호
        CsTitleManager.Instance.EventTitleLifetimeEnded += OnEventTitleLifetimeEnded;
        CsTitleManager.Instance.EventDisplayTitleSet += OnEventDisplayTitleSet;

        CsGameEventToUI.Instance.EventClearDirectionFinish += OnEventClearDirectionFinish;

        CsDungeonManager.Instance.EventStoryDungeonExit += OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;
        
        CsDungeonManager.Instance.EventOsirisRoomExit += OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomBanished += OnEventOsirisRoomBanished;

        //퀘스트 마크 체크
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
        CsMainQuestManager.Instance.EventExecuteDataUpdated += OnEventExecuteDataUpdated;
        CsMainQuestManager.Instance.EventAccepted += OnEventAccepted;
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestAccept += OnEventSecretLetterQuestAccept;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestAccept += OnEventMysteryBoxQuestAccept;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestAccept += OnEventDimensionRaidQuestAccept;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestComplete += OnEventDimensionRaidQuestComplete;
        CsThreatOfFarmQuestManager.Instance.EventQuestAccepted += OnEventQuestAccepted;
        CsThreatOfFarmQuestManager.Instance.EventQuestComplete += OnEventQuestComplete;
        CsThreatOfFarmQuestManager.Instance.EventMissionComplete += OnEventMissionComplete;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestAccept += OnEventHolyWarQuestAccept;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestComplete += OnEventHolyWarQuestComplete;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept += OnEventSupplySupportQuestAccept;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail += OnEventSupplySupportQuestFail;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestCartChange += OnEventSupplySupportQuestCartChange;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAccept += OnEventTrueHeroQuestAccept;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted += OnEventTrueHeroQuestStepCompleted;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete += OnEventTrueHeroQuestComplete;
		CsSubQuestManager.Instance.EventSubQuestAccept += OnEventSubQuestAccept;
		CsSubQuestManager.Instance.EventSubQuestAbandon += OnEventSubQuestAbandon;
		CsSubQuestManager.Instance.EventSubQuestProgressCountsUpdated += OnEventSubQuestProgressCountsUpdated;
		CsSubQuestManager.Instance.EventSubQuestComplete += OnEventSubQuestComplete;
		CsBiographyManager.Instance.EventBiographyStart += OnEventBiographyStart;
		CsBiographyManager.Instance.EventBiographyQuestAccept += OnEventBiographyQuestAccept;
		CsBiographyManager.Instance.EventBiographyQuestProgressCountsUpdated += OnEventBiographyQuestProgressCountsUpdated;
		CsBiographyManager.Instance.EventBiographyQuestComplete += OnEventBiographyQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestAccept += OnEventCreatureFarmQuestAccept;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete += OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted += OnEventCreatureFarmQuestMissionCompleted;
        CsJobChangeManager.Instance.EventJobChangeQuestAccept += OnEventJobChangeQuestAccept;
        CsJobChangeManager.Instance.EventJobChangeQuestComplete += OnEventJobChangeQuestComplete;
        CsJobChangeManager.Instance.EventJobChangeQuestFailed += OnEventJobChangeQuestFailed;
        CsJobChangeManager.Instance.EventJobChangeQuestProgressCountUpdated += OnEventJobChangeQuestProgressCountUpdated;
    }

    //----------------------------------------------------------------------------------------------------
    void LateUpdate()
    {
        if (CsGameData.Instance.MyHeroTransform == null) return;
        if (CsIngameData.Instance.InGameCamera == null) return;

        transform.position = CsIngameData.Instance.InGameCamera.transform.position;
        transform.LookAt(2 * CsGameData.Instance.MyHeroTransform.position - CsIngameData.Instance.InGameCamera.transform.position);
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
		CsGameEventUIToUI.Instance.EventHeroObjectGot -= OnEventHeroObjectGot;

        CsGameEventUIToUI.Instance.EventDeleteAllHUD -= OnEventDeleteAllHUD;
        CsGameEventUIToUI.Instance.EventChattingMessageReceived -= OnEventChattingMessageReceived;

        CsGameEventToUI.Instance.EventCreateHeroHUD -= OnEventCreateHeroHUD;
        CsGameEventToUI.Instance.EventDeleteHeroHUD -= OnEventDeleteHeroHUD;

        CsGameEventToUI.Instance.EventCreateNpcHUD -= OnEventCreateNpcHUD;
        CsGameEventToUI.Instance.EventDeleteNpcHUD -= OnEventDeleteNpcHUD;

        CsGameEventToUI.Instance.EventCreateMonsterHUD -= OnEventCreateMonsterHUD;
        CsGameEventToUI.Instance.EventDeleteMonsterHUD -= OnEventDeleteMonsterHUD;
		CsDungeonManager.Instance.EventCreateMonsterHUD -= OnEventCreateDungeonMonsterHUD;
		CsDungeonManager.Instance.EventDeleteMonsterHUD -= OnEventDeleteDungeonMonsterHUD;

		CsRplzSession.Instance.EventEvtMonsterHit -= OnEventEvtMonsterHit;

        CsGameEventToUI.Instance.EventCreateCartHUD -= OnEventCreateCartHUD;
        CsGameEventToUI.Instance.EventDeleteCartHUD -= OnEventDeleteCartHUD;

        CsGameEventToUI.Instance.EventCreateGuildNpcHUD -= OnEventCreateGuildNpcHUD;
        CsGameEventToUI.Instance.EventDeleteGuildNpcHUD -= OnEventDeleteGuildNpcHUD;

        CsGameEventToUI.Instance.EventCreateNationWarNpcHUD -= OnEventCreateNationWarNpcHUD;
        CsGameEventToUI.Instance.EventDeleteNationWarNpcHUD -= OnEventDeleteNationWarNpcHUD;

        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCompleted -= OnEventSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestComplete -= OnEventSecretLetterQuestComplete;
        CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickCompleted -= OnEventHeroSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventHeroSecretLetterQuestCompleted -= OnEventHeroSecretLetterQuestCompleted;

        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCompleted -= OnEventMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestComplete -= OnEventMysteryBoxQuestComplete;
        CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickCompleted -= OnEventHeroMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxQuestCompleted -= OnEventHeroMysteryBoxQuestCompleted;

        CsDungeonManager.Instance.EventCreateUndergroundMazeNpcHUD -= OnEventCreateUndergroundMazeNpcHUD;

        // 차원 왜곡
        CsGameEventUIToUI.Instance.EventDistortionScrollUse -= OnEventDistortionScrollUse;
        CsGameEventUIToUI.Instance.EventDistortionCanceled -= OnEventDistortionCanceled;

        // 차원 왜곡 서버 이벤트
        CsRplzSession.Instance.EventEvtHeroDistortionStarted -= OnEventEvtHeroDistortionStarted;
        CsRplzSession.Instance.EventEvtHeroDistortionFinished -= OnEventEvtHeroDistortionFinished;
        CsRplzSession.Instance.EventEvtHeroDistortionCanceled -= OnEventEvtHeroDistortionCanceled;

        //자신 랭크 상승
        CsGameEventUIToUI.Instance.EventRankAcquire -= OnEventRankAcquire;

        // 영웅국가관직변경
        //CsRplzSession.Instance.EventEvtNationNoblesseAppointment -= OnEventEvtNationNoblesseAppointment;
        //CsGameEventUIToUI.Instance.EventNationNoblesseDismissed -= OnEventNationNoblesseDismissed;

        //칭호
        CsTitleManager.Instance.EventTitleLifetimeEnded -= OnEventTitleLifetimeEnded;
        CsTitleManager.Instance.EventDisplayTitleSet -= OnEventDisplayTitleSet;

        CsGameEventToUI.Instance.EventClearDirectionFinish -= OnEventClearDirectionFinish;

        CsDungeonManager.Instance.EventStoryDungeonExit -= OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;

        CsDungeonManager.Instance.EventOsirisRoomExit -= OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomBanished -= OnEventOsirisRoomBanished;

        //퀘스트 마크 체크
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;
        CsMainQuestManager.Instance.EventExecuteDataUpdated -= OnEventExecuteDataUpdated;
        CsMainQuestManager.Instance.EventAccepted -= OnEventAccepted;
        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestAccept -= OnEventSecretLetterQuestAccept;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestAccept -= OnEventMysteryBoxQuestAccept;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestAccept -= OnEventDimensionRaidQuestAccept;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestComplete -= OnEventDimensionRaidQuestComplete;
        CsThreatOfFarmQuestManager.Instance.EventQuestAccepted -= OnEventQuestAccepted;
        CsThreatOfFarmQuestManager.Instance.EventQuestComplete -= OnEventQuestComplete;
        CsThreatOfFarmQuestManager.Instance.EventMissionComplete -= OnEventMissionComplete;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestAccept -= OnEventHolyWarQuestAccept;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestComplete -= OnEventHolyWarQuestComplete;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept -= OnEventSupplySupportQuestAccept;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail -= OnEventSupplySupportQuestFail;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestCartChange -= OnEventSupplySupportQuestCartChange;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAccept -= OnEventTrueHeroQuestAccept;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted -= OnEventTrueHeroQuestStepCompleted;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete -= OnEventTrueHeroQuestComplete;
		CsSubQuestManager.Instance.EventSubQuestAccept -= OnEventSubQuestAccept;
		CsSubQuestManager.Instance.EventSubQuestAbandon -= OnEventSubQuestAbandon;
		CsSubQuestManager.Instance.EventSubQuestProgressCountsUpdated -= OnEventSubQuestProgressCountsUpdated;
		CsSubQuestManager.Instance.EventSubQuestComplete -= OnEventSubQuestComplete;
		CsBiographyManager.Instance.EventBiographyStart -= OnEventBiographyStart;
		CsBiographyManager.Instance.EventBiographyQuestAccept -= OnEventBiographyQuestAccept;
		CsBiographyManager.Instance.EventBiographyQuestProgressCountsUpdated -= OnEventBiographyQuestProgressCountsUpdated;
		CsBiographyManager.Instance.EventBiographyQuestComplete -= OnEventBiographyQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestAccept -= OnEventCreatureFarmQuestAccept;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete -= OnEventCreatureFarmQuestComplete;
        CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted -= OnEventCreatureFarmQuestMissionCompleted;
        CsJobChangeManager.Instance.EventJobChangeQuestAccept -= OnEventJobChangeQuestAccept;
        CsJobChangeManager.Instance.EventJobChangeQuestComplete -= OnEventJobChangeQuestComplete;
        CsJobChangeManager.Instance.EventJobChangeQuestFailed -= OnEventJobChangeQuestFailed;
        CsJobChangeManager.Instance.EventJobChangeQuestProgressCountUpdated -= OnEventJobChangeQuestProgressCountUpdated;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestAccept()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxQuestAccept()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestAccept()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExecuteDataUpdated(int nProgressCount)
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestAccepted()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestComplete()
    {
        QuestMarkQuestAllCheck();
    }

    void OnEventMissionComplete(bool bLevelUp, long lAcquiredExp)
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestAccept()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        QuestMarkQuestAllCheck();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestAccept(PDSupplySupportQuestCartInstance pDSupplySupportQuestCartInstance)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestComplete(bool bLevelUp, long lExp, long lGold, int nExploitPoint)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestFail()
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestCartChange(int nOldCartGrade, int nNewCartGrade)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestAccept()
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepCompleted()
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestAccept(int nSubQuestId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestAbandon(int nSubQuestId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestProgressCountsUpdated(int nSubQuestId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestComplete(bool bLevelUp, long lAcquireExp, int nSubQuestId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyStart(CsBiography csBiography)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestAccept(int nBiographyId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestProgressCountsUpdated(int nBiographyId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestComplete(bool bLevelUp, long lAcquiredExp, int nBiographyId)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestAccept()
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		QuestMarkQuestAllCheck();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionCompleted(bool bLevelUp, long lAcquiredExp)
	{
		QuestMarkQuestAllCheck();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestAccept()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestComplete()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestFailed()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestProgressCountUpdated()
    {
        QuestMarkQuestAllCheck();
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventDistortionScrollUse()
    {
        DisplayDistortion(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDistortionCanceled()
    {
        DisplayDistortion(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroDistortionStarted(ClientCommon.SEBHeroDistortionStartedEventBody eventBody)
    {
        DisplayDistortion(true, eventBody.heroId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroDistortionFinished(ClientCommon.SEBHeroDistortionFinishedEventBody eventBody)
    {
        DisplayDistortion(true, eventBody.heroId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroDistortionCanceled(ClientCommon.SEBHeroDistortionCanceledEventBody eventBody)
    {
        DisplayDistortion(true, eventBody.heroId);
    }

    void OnEventRankAcquire()
    {
        Debug.Log(">>>OnEventRankAcquire<<< : " + CsGameData.Instance.MyHeroInfo.RankNo);
        DisplayRank(CsGameData.Instance.MyHeroInfo.RankNo);
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateUndergroundMazeNpcHUD(int nNpcId)
    {
        return CreateUndergroundMazeNpcHUD(nNpcId);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroObjectGot(List<CsHeroObject> listCsHeroObject)
	{
		foreach (var csHeroObject in listCsHeroObject)
		{
			if (csHeroObject.HeroObjectType == EnHeroObjectType.Item)
			{
				CsHeroItem csHeroItem = (CsHeroItem)csHeroObject;

				if (1701 <= csHeroItem.Item.ItemId && csHeroItem.Item.ItemId <= 1705)
				{
					QuestMarkQuestAllCheck();
				}
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteAllHUD()
    {
        DestroyAllHUD();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventChattingMessageReceived(CsChattingMessage csChattingMessage)
    {
        if (PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyChattingBubble) == 1)
        {
            DisplayChattingMessage(csChattingMessage.Sender.HeroId, csChattingMessage.ChattingMessage);
        }
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateHeroHUD(CsHeroBase csHeroBase, string strGuildName, int nGuildMemberGrade, int nPickedSecretLetterGrade, int nPickedMysteryBoxGrade, bool bDistorting, bool bSafeMode, EnNationWarPlayerState enNationWarPlayerState, int nNoblesseId, int nTitleId)
    {
        return CreateHeroHUD(csHeroBase, strGuildName, nGuildMemberGrade, nPickedSecretLetterGrade, nPickedMysteryBoxGrade, bDistorting, bSafeMode, enNationWarPlayerState, nNoblesseId, nTitleId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteHeroHUD(Guid guidHeroId)
    {
        DeleteHeroHUD(guidHeroId);
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateNpcHUD(int nNpcId)
    {
        return CreateNpcHUD(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteNpcHUD(int nNpcId)
    {
        DeleteNpcHUD(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateMonsterHUD(long lInstanceId, CsMonsterInfo csMonsterInfo, string strHeroName, bool bExclusive, int nHp)
    {
        return CreateMonsterHUD(lInstanceId, csMonsterInfo, strHeroName, bExclusive, nHp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteMonsterHUD(long lInstanceId)
    {
        DeleteMonsterHUD(lInstanceId);
    }

    //---------------------------------------------------------------------------------------------------
	RectTransform OnEventCreateDungeonMonsterHUD(EnDungeonPlay enDungeonPlay, long lInstanceId, CsMonsterInfo csMonsterInfo, bool bIsBossMonster, int nHalidomId)
    {
		return CreateDungeonMonsterHUD(enDungeonPlay, lInstanceId, csMonsterInfo, bIsBossMonster, nHalidomId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteDungeonMonsterHUD(long lInstanceId)
    {
        DeleteMonsterHUD(lInstanceId);
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventEvtMonsterHit(ClientCommon.SEBMonsterHitEventBody eventBody)
    {
        Transform trMonsterHUD = m_trMonster.Find(eventBody.monsterInstanceId.ToString());

        if (trMonsterHUD != null)
        {
            Slider sliderMonsterHp = trMonsterHUD.Find("SliderMonsterHp").GetComponent<Slider>();
            sliderMonsterHp.value = eventBody.hitResult.hp;
        }
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateCartHUD(long lInstanceId)
    {
        ICartObjectInfo cartObjectInfo = CsGameData.Instance.ListCartObjectInfo.Find(a => a.GetInstanceId() == lInstanceId);

        return CreateCartHUD(lInstanceId, cartObjectInfo.GetCartObject());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteCartHUD(long lInstanceId)
    {
        DeleteCartHUD(lInstanceId);
    }
    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateGuildNpcHUD(int nNpcId)
    {
        return CreateGuildNpcHUD(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteGuildNpcHUD(int nNpcId)
    {
        DeleteGuildNpcHUD(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform OnEventCreateNationWarNpcHUD(int nNpcId)
    {
        return CreateNationWarNpcHUD(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteNationWarNpcHUD(int nNpcId)
    {
        DeleteNationWarNpcHUD(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickCompleted()
    {
        DisplayHeroQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        DisplayHeroQuest();
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickCompleted()
    {
        DisplayHeroQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        DisplayHeroQuest();
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroSecretLetterPickCompleted(Guid guid, int nGrade)
    {
        DisplayHeroQuest(guid, true, nGrade);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroSecretLetterQuestCompleted(Guid guid)
    {
        DisplayHeroQuest(guid, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroMysteryBoxPickCompleted(Guid guid, int nGrade)
    {
        DisplayHeroQuest(guid, false, nGrade);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroMysteryBoxQuestCompleted(Guid guid)
    {
        DisplayHeroQuest(guid, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroNationNoblesseChanged(Guid guidHeroId, int nNoblesseId)
    {
        DisplayHeroNationNoblesse(guidHeroId, nNoblesseId);
    }

    ////---------------------------------------------------------------------------------------------------
    //void OnEventEvtNationNoblesseAppointment(ClientCommon.SEBNationNoblesseAppointmentEventBody eventBody)
    //{
    //    if (CsGameData.Instance.MyHeroInfo.HeroId == eventBody.heroId)
    //    {
    //        DisplayHeroNationNoblesse(CsGameData.Instance.MyHeroInfo.HeroId, eventBody.noblesseId);
    //    }
    //}

    ////---------------------------------------------------------------------------------------------------
    //void OnEventNationNoblesseDismissed()
    //{
    //    CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(CsGameData.Instance.MyHeroInfo.HeroId);

    //    if (csNationNoblesseInstance == null)
    //    {
    //        DisplayHeroNationNoblesse(CsGameData.Instance.MyHeroInfo.HeroId, 0);
    //    }
    //}

    //---------------------------------------------------------------------------------------------------
    void OnEventTitleLifetimeEnded(int nTitleId)
    {
        DisplayTitle();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDisplayTitleSet()
    {
        DisplayTitle();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClearDirectionFinish(EnDungeonPlay enDungeonPlay)
    {
        m_trMyHero.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonExit(int nPreviousContinentId)
    {
        m_trMyHero.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nPreviousContinentId)
    {
        m_trMyHero.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomExit(int nPreviousContinentId)
    {
        m_trMyHero.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomBanished(int nPreviousContinentId)
    {
        m_trMyHero.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
    {
        QuestMarkQuestAllCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        QuestMarkQuestAllCheck();
        DisplayRank(CsGameData.Instance.MyHeroInfo.RankNo);
    }
    #endregion Event

    #region Hero HUD

    //---------------------------------------------------------------------------------------------------
    RectTransform CreateHeroHUD(CsHeroBase csHeroBase, string strGuildName, int nGuildMemberGrade, int nPickedSecretLetterGrade, int nPickedMysteryBoxGrade, bool isDistorting, bool isSafeMode, EnNationWarPlayerState enNationWarPlayerState, int nNoblesseId, int nTitleId)
    {
        Transform trHeroHUD = null;

        if (csHeroBase.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            trHeroHUD = m_trMyHero.Find(csHeroBase.HeroId.ToString());
        }
        else
        {
            trHeroHUD = m_trHero.Find(csHeroBase.HeroId.ToString());
        }

        if (trHeroHUD == null)
        {
            GameObject goHeroHUD = Instantiate<GameObject>(m_goHeroHUD);
            goHeroHUD.name = csHeroBase.HeroId.ToString();

            if (csHeroBase.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                goHeroHUD.transform.SetParent(m_trMyHero);
                DisplayHeroQuest();
            }
            else
            {
                goHeroHUD.transform.SetParent(m_trHero);
                DisplayHeroQuest(csHeroBase.HeroId, true, nPickedSecretLetterGrade);
                DisplayHeroQuest(csHeroBase.HeroId, false, nPickedMysteryBoxGrade);
            }

            goHeroHUD.transform.position = new Vector3(0, 0, 0);
            goHeroHUD.transform.localScale = new Vector3(1, 1, 1);
            goHeroHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trHeroHUD = goHeroHUD.transform;

            Transform trName = trHeroHUD.Find("Name");

            Text textName = trName.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);

            Text textGuild = trHeroHUD.Find("TextGuild").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGuild);

            Text textTitle = trHeroHUD.Find("Title/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTitle);

            Text textMessageBubble = trHeroHUD.Find("MessageBubble/ImageText/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textMessageBubble);
        }

        UpdateHeroHUD(csHeroBase, strGuildName, nGuildMemberGrade, isDistorting, isSafeMode, enNationWarPlayerState, nNoblesseId, nTitleId);

        return trHeroHUD.GetComponent<RectTransform>();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroHUD(CsHeroBase csHeroBase, string strGuildName, int nGuildMemberGrade, bool isDistorting, bool isSafeMode, EnNationWarPlayerState enNationWarPlayerState, int nNoblesseId, int nTitleId)
    {
        Transform trHeroHUD = null;

        if (csHeroBase.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            trHeroHUD = m_trMyHero.Find(csHeroBase.HeroId.ToString());

            if (CsGameData.Instance.MyHeroInfo.RemainingDistortionTime - Time.realtimeSinceStartup > 0)
            {
                isDistorting = true;
            }
            else
            {
                isDistorting = false;
            }
        }
        else
        {
            trHeroHUD = m_trHero.Find(csHeroBase.HeroId.ToString());
        }

        if (trHeroHUD != null)
        {
            Transform trName = trHeroHUD.Find("Name");
            Transform trWorldClass = trHeroHUD.Find("WorldClass");

            if (nNoblesseId != 0)
            {
                Image imageWorldClass = trWorldClass.Find("ImageWorldClass").GetComponent<Image>();
                imageWorldClass.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_class_" + nNoblesseId);

                Text textWorldClass = imageWorldClass.transform.Find("Text").GetComponent<Text>();
                textWorldClass.text = CsGameData.Instance.GetNationNoblesse(nNoblesseId).Name;
                CsUIData.Instance.SetFont(textWorldClass);

                trWorldClass.gameObject.SetActive(true);
            }
            else
            {
                trWorldClass.gameObject.SetActive(false);
            }

            // 국가
            Image imageNation = trName.Find("ImageNation").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_nation" + csHeroBase.Nation.NationId);

            // 이름
            Text textName = trName.Find("Text").GetComponent<Text>();
            textName.text = csHeroBase.Name;

            if (csHeroBase.Nation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                textName.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                // 차원 동맹 표시
                if (CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId) == csHeroBase.Nation.NationId)
                {
                    textName.color = CsUIData.Instance.ColorWhite;
                }
                else
                {
                    textName.color = new Color32(187, 32, 27, 255);
                }
            }

            //계급
            Image imageClass = trName.Find("ImageClass").GetComponent<Image>();

            if (csHeroBase.RankNo == 0)
            {
                imageClass.gameObject.SetActive(false);
            }
            else
            {
                imageClass.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_class_" + csHeroBase.RankNo.ToString());
                imageClass.gameObject.SetActive(true);
            }

            // 왜곡
            Transform trDistortion = trName.Find("ImageDistortion");
            trDistortion.gameObject.SetActive(isDistorting);
			if (isDistorting == false)
			{
				// 안전모드
				trDistortion.gameObject.SetActive(isSafeMode);
			}

			Image imageWar = trName.Find("ImageWar").GetComponent<Image>();
            imageWar.gameObject.SetActive(false);

            CsNationWarDeclaration csNationWarDeclaration = null;
            int nMyAllianceNationId = CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId);
            CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
            
            if (CsGameConfig.Instance.PvpMinHeroLevel <= csHeroBase.Level)
            {
                csNationWarDeclaration = CsNationWarManager.Instance.MyNationWarDeclaration;

                if (csNationWarDeclaration == null)
                {

                }
                else
                {
                    if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
                    {
                        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == csNationWarDeclaration.TargetNationId && csContinent != null && csContinent.IsNationWarTarget)
                        {
                            if (csNationWarDeclaration.NationId == csHeroBase.Nation.NationId)
                            {
                                imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_attack");
                                imageWar.gameObject.SetActive(true);
                            }
                            else if (csNationWarDeclaration.TargetNationId == csHeroBase.Nation.NationId)
                            {
                                imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_defence");
                                imageWar.gameObject.SetActive(true);
                            }
                            else
                            {
                                if (nMyAllianceNationId == csHeroBase.Nation.NationId)
                                {
                                    // 동맹국 표시
                                    imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_alliance");
                                    imageWar.gameObject.SetActive(true);
                                }
                                else
                                {
                                    // 적 국가 동맹국 HUD 표시
                                    if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                                    {
                                        if (CsNationAllianceManager.Instance.GetNationAllianceId(csNationWarDeclaration.TargetNationId) == csHeroBase.Nation.NationId)
                                        {
                                            imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_defence");
                                            imageWar.gameObject.SetActive(true);
                                        }
                                    }
                                    else
                                    {
                                        if (CsNationAllianceManager.Instance.GetNationAllianceId(csNationWarDeclaration.NationId) == csHeroBase.Nation.NationId)
                                        {
                                            imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_attack");
                                            imageWar.gameObject.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        imageWar.gameObject.SetActive(false);
                    }
                }

                csNationWarDeclaration = CsNationWarManager.Instance.GetNationWarDeclaration(nMyAllianceNationId);

                if (csNationWarDeclaration == null)
                {
                    
                }
                else
                {
                    if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
                    {
                        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == csNationWarDeclaration.TargetNationId && csContinent != null && csContinent.IsNationWarTarget)
                        {
                            if (csHeroBase.Nation.NationId == nMyAllianceNationId)
                            {
                                // 동맹국 표시
                                imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_alliance");
                                imageWar.gameObject.SetActive(true);
                            }
                            else
                            {
                                if (csNationWarDeclaration.NationId == nMyAllianceNationId)
                                {
                                    // 동맹국이 공격측
                                    if (csHeroBase.Nation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                                    {
                                        imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_attack");
                                        imageWar.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        if (csHeroBase.Nation.NationId == csNationWarDeclaration.TargetNationId || csHeroBase.Nation.NationId == CsNationAllianceManager.Instance.GetNationAllianceId(csNationWarDeclaration.TargetNationId))
                                        {
                                            imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_defence");
                                            imageWar.gameObject.SetActive(true);
                                        }
                                    }
                                }
                                else
                                {
                                    // 동맹국이 수비측
                                    if (csHeroBase.Nation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                                    {
                                        imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_defence");
                                        imageWar.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        if (csHeroBase.Nation.NationId == csNationWarDeclaration.NationId || csHeroBase.Nation.NationId == CsNationAllianceManager.Instance.GetNationAllianceId(csNationWarDeclaration.NationId))
                                        {
                                            imageWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_war_attack");
                                            imageWar.gameObject.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        imageWar.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                imageWar.gameObject.SetActive(false);
            }
            
            // 길드
            Text textGuild = trHeroHUD.Find("TextGuild").GetComponent<Text>();

            if (nGuildMemberGrade != 0)
            {
                textGuild.gameObject.SetActive(true);

                if (CsGameData.Instance.GetGuildMemberGrade(nGuildMemberGrade).MemberGrade != 4)
                {
                    textGuild.text = string.Format(CsConfiguration.Instance.GetString("A58_TXT_01003"), strGuildName, CsGameData.Instance.GetGuildMemberGrade(nGuildMemberGrade).Name);
                }
                else
                {
                    textGuild.text = strGuildName;
                }
            }
            else
            {
                textGuild.gameObject.SetActive(false);
            }

            // 칭호
            Image imageTitle = trHeroHUD.Find("Title").GetComponent<Image>();
            Text textTitle = imageTitle.transform.Find("Text").GetComponent<Text>();
            CsTitle csTitle = CsGameData.Instance.GetTitle(nTitleId);

            if (csTitle != null)
            {
                textTitle.text = string.Format("<color={0}>{1}</color>", csTitle.TitleGrade.ColorCode, csTitle.Name);

                if (csTitle.TitleGrade.Grade > 3)
                {
                    imageTitle.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + csTitle.BackgroundImageName);
                    imageTitle.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    imageTitle.color = new Color(1, 1, 1, 0);
                }

                imageTitle.transform.gameObject.SetActive(true);
            }
            else
            {
                imageTitle.transform.gameObject.SetActive(false);
            }

            if (csHeroBase.HeroId != CsGameData.Instance.MyHeroInfo.HeroId)
            {
                switch (CsUIData.Instance.DungeonInNow)
                {
                    case EnDungeon.None:
                        break;

                    case EnDungeon.UndergroundMaze: 
                        break;

                    case EnDungeon.AncientRelic:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.SoulCoveter:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.WisdomTemple:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.RuinsReclaim:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.FearAltar:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.InfiniteWar:
                        textName.color = new Color32(187, 32, 27, 255);
                        trWorldClass.gameObject.SetActive(false);
                        imageNation.gameObject.SetActive(false);
                        imageClass.gameObject.SetActive(false);
                        trDistortion.gameObject.SetActive(false);
                        imageWar.gameObject.SetActive(false);
                        textGuild.gameObject.SetActive(false);
                        imageTitle.gameObject.SetActive(false);
                        break;

                    case EnDungeon.WarMemory:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.DragonNest:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.TradeShip:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;

                    case EnDungeon.AnkouTomb:
                        textName.color = CsUIData.Instance.ColorWhite;
                        break;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DeleteHeroHUD(Guid guidHeroId)
    {
        if (this)
        {
            Transform trHUD = null;

            if (guidHeroId == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                trHUD = m_trMyHero.Find(guidHeroId.ToString());
            }
            else
            {
                trHUD = m_trHero.Find(guidHeroId.ToString());
            }

            if (trHUD != null)
            {
                Destroy(trHUD.gameObject);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //자신 왜곡
    void DisplayDistortion(bool bIson)
    {
        Transform trHeroHUD = m_trMyHero.Find(CsGameData.Instance.MyHeroInfo.HeroId.ToString());

        if (trHeroHUD != null)
        {
            trHeroHUD.Find("Name/ImageDistortion").gameObject.SetActive(bIson);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //타유저 왜곡
    void DisplayDistortion(bool bIson, Guid guid)
    {
        Transform trHeroHUD = m_trHero.Find(guid.ToString());

        if (trHeroHUD != null)
        {
            trHeroHUD.Find("Name/ImageDistortion").gameObject.SetActive(bIson);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayRank(int nRank)
    {
        Transform trHeroHUD = m_trMyHero.Find(CsGameData.Instance.MyHeroInfo.HeroId.ToString());

        if (trHeroHUD != null)
        {
            Image imageClass = trHeroHUD.Find("Name/ImageClass").GetComponent<Image>();
            if (nRank == 0)
            {
                imageClass.gameObject.SetActive(false);
            }
            else
            {
                imageClass.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_class_" + nRank.ToString());
                imageClass.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTitle()
    {
        Transform trHeroHUD = m_trMyHero.Find(CsGameData.Instance.MyHeroInfo.HeroId.ToString());

        if (trHeroHUD != null)
        {
            Image imageTitle = trHeroHUD.Find("Title").GetComponent<Image>();
            Text textTitle = imageTitle.transform.Find("Text").GetComponent<Text>();
            CsTitle csTitle = CsGameData.Instance.GetTitle(CsTitleManager.Instance.DisplayTitleId);

            if (csTitle != null)
            {
                textTitle.text = string.Format("<color={0}>{1}</color>", csTitle.TitleGrade.ColorCode, csTitle.Name);

                if (csTitle.TitleGrade.Grade > 3)
                {
                    imageTitle.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_hud_title_" + csTitle.TitleGrade.Grade);
                    imageTitle.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    imageTitle.color = new Color(1, 1, 1, 0);
                }

                imageTitle.transform.gameObject.SetActive(true);
            }
            else
            {
                imageTitle.transform.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //자신 퀘스트
    void DisplayHeroQuest()
    {
        Transform trHeroHUD = m_trMyHero.Find(CsGameData.Instance.MyHeroInfo.HeroId.ToString());
        Transform trQuest = trHeroHUD.Find("Quest");
        trQuest.gameObject.SetActive(true);

        Image imageSecretLetter = trQuest.Find("ImageSecretLetter").GetComponent<Image>();
        Image imageMysteryBox = trQuest.Find("ImageMysteryBox").GetComponent<Image>();

        if (CsSecretLetterQuestManager.Instance.PickedLetterGrade != 0 && CsMysteryBoxQuestManager.Instance.PickedBoxGrade != 0)
        {
            imageSecretLetter.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_scroll0" + CsSecretLetterQuestManager.Instance.PickedLetterGrade);
            imageMysteryBox.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_box0" + CsMysteryBoxQuestManager.Instance.PickedBoxGrade);
            imageSecretLetter.gameObject.SetActive(true);
            imageMysteryBox.gameObject.SetActive(true);
        }
        else if (CsSecretLetterQuestManager.Instance.PickedLetterGrade != 0)
        {
            imageSecretLetter.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_scroll0" + CsSecretLetterQuestManager.Instance.PickedLetterGrade);
            imageSecretLetter.gameObject.SetActive(true);
            imageMysteryBox.gameObject.SetActive(false);
        }
        else if (CsMysteryBoxQuestManager.Instance.PickedBoxGrade != 0)
        {
            imageMysteryBox.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_box0" + CsMysteryBoxQuestManager.Instance.PickedBoxGrade);
            imageSecretLetter.gameObject.SetActive(false);
            imageMysteryBox.gameObject.SetActive(true);
        }
        else
        {
            trQuest.gameObject.SetActive(false);
            imageMysteryBox.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //타유저 퀘스트
    void DisplayHeroQuest(Guid guid, bool bSecretLetter, int nGrade = 0)
    {
        Transform trHeroHUD = m_trHero.Find(guid.ToString());
        if (trHeroHUD == null)
            return;
        Transform trQuest = trHeroHUD.Find("Quest");

        Image imageSecretLetter = trQuest.Find("ImageSecretLetter").GetComponent<Image>();
        Image imageMysteryBox = trQuest.Find("ImageMysteryBox").GetComponent<Image>();

        if (bSecretLetter)
        {
            if (nGrade == 0)
            {
                imageSecretLetter.gameObject.SetActive(false);
            }
            else
            {
                imageSecretLetter.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_scroll0" + nGrade);
                imageSecretLetter.gameObject.SetActive(true);
            }
        }
        else
        {
            if (nGrade == 0)
            {
                imageMysteryBox.gameObject.SetActive(false);
            }
            else
            {
                imageMysteryBox.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_box0" + nGrade);
                imageMysteryBox.gameObject.SetActive(true);
            }
        }

        if (imageSecretLetter.gameObject.activeSelf || imageMysteryBox.gameObject.activeSelf)
        {
            trQuest.gameObject.SetActive(true);
        }
        else
        {
            trQuest.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayHeroNationNoblesse(Guid guidHeroId, int nNoblessedId)
    {
        Transform trHeroHUD = null;

        if (CsGameData.Instance.MyHeroInfo.HeroId == guidHeroId)
        {
            trHeroHUD = m_trMyHero.Find(CsGameData.Instance.MyHeroInfo.HeroId.ToString());
        }
        else
        {
            trHeroHUD = m_trHero.Find(guidHeroId.ToString());
        }

        if (trHeroHUD != null)
        {
            Transform trWorldClass = trHeroHUD.Find("WorldClass");

            if (nNoblessedId != 0)
            {
                CsNationNoblesse csNationNoblesse = CsGameData.Instance.GetNationNoblesse(nNoblessedId);

                // 영웅국가 관직을 표시한다.

                Image imageWorldClass = trWorldClass.Find("ImageWorldClass").GetComponent<Image>();
                imageWorldClass.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_world_class_" + nNoblessedId);

                Text textWorldClass = imageWorldClass.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textWorldClass);
                textWorldClass.text = csNationNoblesse.Name;

                trWorldClass.gameObject.SetActive(true);
            }
            else
            {
                // 영웅국가 관직을 제거한다.
                trWorldClass.gameObject.SetActive(false);
            }
        }
    }

    #endregion Hero HUD

    #region Npc HUD

    //---------------------------------------------------------------------------------------------------
    void QuestMarkQuestAllCheck()
    {
        for (int i = 0; i < m_trNpc.childCount; ++i)
        {
            QuestMarkQuestCheck(m_trNpc.GetChild(i));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void QuestMarkQuestCheck(Transform trNpcHUD)
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam != CsGameData.Instance.MyHeroInfo.Nation.NationId) return;

        int nNpcId = int.Parse(trNpcHUD.name);
        Transform trQuestMark = trNpcHUD.Find("QuestMark");
        CsNpcInfo csNpcInfo;

        //메인퀘스트 마크
        Transform trMarkMainAccpet = trQuestMark.Find("RFX_npcExclamationMark1");
        Transform trMarkMainComplete = trQuestMark.Find("RFX_npcQuestionMark1");
        //서브퀘스트 마크
        Transform trMarkSubAccpet = trQuestMark.Find("RFX_npcExclamationMark2");
        Transform trMarkSubComplete = trQuestMark.Find("RFX_npcQuestionMark2");

        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;

        if (csMainQuest != null)
        {
            //레벨제한 체크
            if (csMainQuest.RequiredHeroLevel > CsGameData.Instance.MyHeroInfo.Level)
            {
                trMarkMainAccpet.gameObject.SetActive(false);
                trMarkMainComplete.gameObject.SetActive(false);
            }
            //퀘스트 수락전
            else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None)
            {
                csNpcInfo = CsMainQuestManager.Instance.MainQuest.StartNpc;

                if (csNpcInfo != null && csNpcInfo.NpcId == nNpcId)
                {
                    trMarkMainComplete.gameObject.SetActive(false);
                    trMarkMainAccpet.gameObject.SetActive(true);
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    trMarkMainAccpet.gameObject.SetActive(false);
                    trMarkMainComplete.gameObject.SetActive(false);
                }
            }
            //퀘스트 완료전
            else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed)
            {
                csNpcInfo = CsMainQuestManager.Instance.MainQuest.CompletionNpc;

                if (csNpcInfo != null && csNpcInfo.NpcId == nNpcId)
                {
                    trMarkMainAccpet.gameObject.SetActive(false);
                    trMarkMainComplete.gameObject.SetActive(true);
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    trMarkMainAccpet.gameObject.SetActive(false);
                    trMarkMainComplete.gameObject.SetActive(false);
                }
            }
            else
            {
                trMarkMainAccpet.gameObject.SetActive(false);
                trMarkMainComplete.gameObject.SetActive(false);
            }
        }
        else
        {
            trMarkMainAccpet.gameObject.SetActive(false);
            trMarkMainComplete.gameObject.SetActive(false);
        }

        //밀서 NPC 확인
        csNpcInfo = CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo;

        if (csNpcInfo.NpcId == nNpcId)
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsSecretLetterQuestManager.Instance.SecretLetterQuest.RequiredHeroLevel)
            {
                csNpcInfo = CsGameData.Instance.SecretLetterQuest.QuestNpcInfo;

                //퀘스트 수락 전 && 밀서 카운트 체크
                if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.None
                     && CsSecretLetterQuestManager.Instance.DailySecretLetterQuestStartCount < CsGameData.Instance.SecretLetterQuest.LimitCount)
                {
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(true);
                    return;
                }
                else if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed
                      || CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Executed)
                {
                    trMarkSubAccpet.gameObject.SetActive(false);
                    trMarkSubComplete.gameObject.SetActive(true);
                    return;
                }
            }
        }

        //의문의 박스 NPC 확인
        csNpcInfo = CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo;

        if (csNpcInfo.NpcId == nNpcId)
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.RequiredHeroLevel)
            {
                if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.None
                    && CsMysteryBoxQuestManager.Instance.DailyMysteryBoxQuestStartCount < CsGameData.Instance.MysteryBoxQuest.LimitCount)
                {
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(true);
                    return;
                }
                else if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Completed
                      || CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Executed)
                {
                    trMarkSubAccpet.gameObject.SetActive(false);
                    trMarkSubComplete.gameObject.SetActive(true);
                    return;
                }
            }
        }

        //차원의 습격 NPC 확인
        csNpcInfo = CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo;

        if (csNpcInfo.NpcId == nNpcId)
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.RequiredHeroLevel)
            {

                if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.None
                     && CsDimensionRaidQuestManager.Instance.DailyDimensionRaidQuestStartCount < CsGameData.Instance.DimensionRaidQuest.LimitCount)
                {
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(true);
                    return;
                }
                else if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
                {
                    trMarkSubAccpet.gameObject.SetActive(false);
                    trMarkSubComplete.gameObject.SetActive(true);
                    return;
                }
            }
        }

		// 보급지원
		csNpcInfo = CsSupplySupportQuestManager.Instance.SupplySupportQuest.StartNpc;

		if (csNpcInfo.NpcId == nNpcId &&
			CsGameData.Instance.MyHeroInfo.Level >= CsSupplySupportQuestManager.Instance.SupplySupportQuest.RequiredHeroLevel &&
			CsSupplySupportQuestManager.Instance.DailySupplySupportQuestCount < CsSupplySupportQuestManager.Instance.SupplySupportQuest.LimitCount &&
			CsSupplySupportQuestManager.Instance.QuestState == EnSupplySupportState.None)
		{
			for (int i = 0; i < CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList.Count; i++)
			{
				int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList[i].OrderItem.ItemId);

				if (nItemCount > 0)
				{
					trMarkSubComplete.gameObject.SetActive(false);
					trMarkSubAccpet.gameObject.SetActive(true);
					return;
				}
			}
		}

		csNpcInfo = CsSupplySupportQuestManager.Instance.SupplySupportQuest.CompletionNpc;

		if (csNpcInfo.NpcId == nNpcId &&
			CsSupplySupportQuestManager.Instance.QuestState == EnSupplySupportState.Executed)
		{
			trMarkSubComplete.gameObject.SetActive(true);
			trMarkSubAccpet.gameObject.SetActive(false);

			return;
		}
		
		if (CsSupplySupportQuestManager.Instance.QuestState == EnSupplySupportState.Accepted)
		{
			foreach (var csSupplySupportQuestWayPoint in CsSupplySupportQuestManager.Instance.CsSupplySupportQuestWayPointList)
			{
				if (csSupplySupportQuestWayPoint.CartChangeNpc.NpcId == nNpcId)
				{
					trMarkSubComplete.gameObject.SetActive(true);
					trMarkSubAccpet.gameObject.SetActive(false);

					return;
				}
			}
		}

        //농장의 위협 NPC 확인
        csNpcInfo = CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc;

        if (csNpcInfo.NpcId == nNpcId)
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.ThreatOfFarmQuest.RequiredHeroLevel)
            {
                if (CsThreatOfFarmQuestManager.Instance.QuestState == EnQuestState.None)
                {
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(true);
                    return;
                }
                else if (CsThreatOfFarmQuestManager.Instance.QuestState == EnQuestState.Executed
                    && CsThreatOfFarmQuestManager.Instance.ProgressCount >= CsThreatOfFarmQuestManager.Instance.Quest.LimitCount)
                {
                    trMarkSubAccpet.gameObject.SetActive(false);
                    trMarkSubComplete.gameObject.SetActive(true);
                    return;
                }
            }
        }

		//진정한 영웅
		csNpcInfo = CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc;

		if (csNpcInfo.NpcId == nNpcId)
		{
			if (CsGameData.Instance.MyHeroInfo.Level >= CsTrueHeroQuestManager.Instance.TrueHeroQuest.RequiredHeroLevel)
			{
				//퀘스트 수락 전
				if (CsTrueHeroQuestManager.Instance.TrueHeroQuestState == EnTrueHeroQuestState.None)
				{
					trMarkSubComplete.gameObject.SetActive(false);
					trMarkSubAccpet.gameObject.SetActive(true);
					return;
				}
				else if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Executed)
				{
					trMarkSubAccpet.gameObject.SetActive(false);
					trMarkSubComplete.gameObject.SetActive(true);
					return;
				}
			}
		}

        //위대한 성전 NPC 확인
        csNpcInfo = CsHolyWarQuestManager.Instance.HolyWarQuest.QuestNpcInfo;

        if (csNpcInfo.NpcId == nNpcId)
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsHolyWarQuestManager.Instance.HolyWarQuest.RequiredHeroLevel
            && CsHolyWarQuestManager.Instance.CheckAvailability())
            {

                if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.None)
                {
                    trMarkSubComplete.gameObject.SetActive(false);
                    trMarkSubAccpet.gameObject.SetActive(true);
                    return;
                }
                else if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
                {
                    trMarkSubAccpet.gameObject.SetActive(false);
                    trMarkSubComplete.gameObject.SetActive(true);
                    return;
                }
            }
        }

		// 서브 퀘스트
		foreach (var csSubQuest in CsGameData.Instance.SubQuestList)
		{
			if (csSubQuest.RequiredConditionType != 1 ||
				CsMainQuestManager.Instance.MainQuest == null ||
					csSubQuest.RequiredConditionValue >= CsMainQuestManager.Instance.MainQuest.MainQuestNo)				continue;

			CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(csSubQuest.QuestId);

			if (csHeroSubQuest == null)
			{
				if (csSubQuest.StartNpc != null &&
					csSubQuest.StartNpc.NpcId == nNpcId)
				{
					trMarkSubComplete.gameObject.SetActive(false);
					trMarkSubAccpet.gameObject.SetActive(true);
					return;
				}
			}
			else
			{
				if (csSubQuest.StartNpc != null &&
					csSubQuest.StartNpc.NpcId == nNpcId &&
					csHeroSubQuest.EnStatus == EnSubQuestStatus.Abandon)
				{
					trMarkSubComplete.gameObject.SetActive(false);
					trMarkSubAccpet.gameObject.SetActive(true);
					return;
				}
				else if (csSubQuest.CompletionNpc != null &&
						csSubQuest.CompletionNpc.NpcId == nNpcId &&
						csHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
				{
					trMarkSubAccpet.gameObject.SetActive(false);
					trMarkSubComplete.gameObject.SetActive(true);
					return;
				}
			}
			
		}
		
		// 전기 퀘스트
		foreach (var csHeroBiography in CsBiographyManager.Instance.HeroBiographyList)
		{
			if (csHeroBiography.Completed)
				continue;

			if (csHeroBiography.HeroBiograhyQuest == null)
			{
				CsBiographyQuest csBiographyQuest = csHeroBiography.Biography.GetBiographyQuest(1);

				// 수락 가능
				if (csBiographyQuest != null &&
					csBiographyQuest.StartNpc != null &&
					csBiographyQuest.StartNpc.NpcId == nNpcId)
				{
					trMarkSubAccpet.gameObject.SetActive(true);
					trMarkSubComplete.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				CsBiographyQuest nextQuest = csHeroBiography.Biography.GetBiographyQuest(csHeroBiography.HeroBiograhyQuest.QuestNo + 1);

				if (csHeroBiography.HeroBiograhyQuest.Completed)
				{
					// 수락 가능
					if (nextQuest != null &&
						nextQuest.StartNpc != null &&
						nextQuest.StartNpc.NpcId == nNpcId)
					{
						trMarkSubAccpet.gameObject.SetActive(true);
						trMarkSubComplete.gameObject.SetActive(false);
						return;
					}
				}
				else if (csHeroBiography.HeroBiograhyQuest.Excuted)
				{
					// 완료 가능
					if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.CompletionNpc != null &&
						csHeroBiography.HeroBiograhyQuest.BiographyQuest.CompletionNpc.NpcId == nNpcId)
					{
						trMarkSubAccpet.gameObject.SetActive(false);
						trMarkSubComplete.gameObject.SetActive(true);
						return;
					}
				}
				else
				{
					// npc 대화 타입인 경우
					if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.Type == 4 &&
						csHeroBiography.HeroBiograhyQuest.BiographyQuest.TargetNpc != null &&
						csHeroBiography.HeroBiograhyQuest.BiographyQuest.TargetNpc.NpcId == nNpcId)
					{
						trMarkSubAccpet.gameObject.SetActive(true);
						trMarkSubComplete.gameObject.SetActive(false);
						return;
					}
				}
			}
		}

		// 크리쳐 농장
		if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuest.StartNpc != null &&
			CsCreatureFarmQuestManager.Instance.CreatureFarmQuest.StartNpc.NpcId == nNpcId &&
			CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.None)
		{
			trMarkSubAccpet.gameObject.SetActive(true);
			trMarkSubComplete.gameObject.SetActive(false);
			return;
		}

		if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuest.CompletionNpc != null &&
			CsCreatureFarmQuestManager.Instance.CreatureFarmQuest.CompletionNpc.NpcId == nNpcId &&
			CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
		{
			trMarkSubAccpet.gameObject.SetActive(false);
			trMarkSubComplete.gameObject.SetActive(true);
			return;
		}

		if (CsGameConfig.Instance.JobChangeRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;
            
            if (csHeroJobChangeQuest == null)
            {
                if (nNpcId == CsGameData.Instance.JobChangeQuestList.First().QuestNpc.NpcId)
                {
                    trMarkSubAccpet.gameObject.SetActive(true);
                    trMarkSubComplete.gameObject.SetActive(false);
                    return;
                }
            }
            else
            {
                CsJobChangeQuest csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);

                if (csJobChangeQuest == null)
                {
                    return;
                }
                else
                {
                    if (CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo).QuestNpc.NpcId == nNpcId)
                    {
                        if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo)
                        {
                            switch ((EnJobChangeQuestStaus)csHeroJobChangeQuest.Status)
                            {
                                case EnJobChangeQuestStaus.Accepted:

                                    if (csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
                                    {
                                        trMarkSubAccpet.gameObject.SetActive(false);
                                        trMarkSubComplete.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        trMarkSubAccpet.gameObject.SetActive(false);
                                        trMarkSubComplete.gameObject.SetActive(true);
                                    }

                                    return;

                                case EnJobChangeQuestStaus.Completed:

                                    trMarkSubAccpet.gameObject.SetActive(false);
                                    trMarkSubComplete.gameObject.SetActive(false);

                                    return;

                                case EnJobChangeQuestStaus.Failed:

                                    trMarkSubAccpet.gameObject.SetActive(true);
                                    trMarkSubComplete.gameObject.SetActive(false);

                                    return;
                            }
                        }
                        else
                        {
                            switch ((EnJobChangeQuestStaus)csHeroJobChangeQuest.Status)
                            {
                                case EnJobChangeQuestStaus.Accepted:

                                    if (csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
                                    {
                                        trMarkSubAccpet.gameObject.SetActive(false);
                                        trMarkSubComplete.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        trMarkSubAccpet.gameObject.SetActive(false);
                                        trMarkSubComplete.gameObject.SetActive(true);
                                    }

                                    return;

                                case EnJobChangeQuestStaus.Completed:

                                    trMarkSubAccpet.gameObject.SetActive(true);
                                    trMarkSubComplete.gameObject.SetActive(false);

                                    return;

                                case EnJobChangeQuestStaus.Failed:
                                    

                                    return;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

		trMarkSubAccpet.gameObject.SetActive(false);
		trMarkSubComplete.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform CreateNpcHUD(int nNpcId)
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(nNpcId);

        Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

        if (trNpcHUD == null)
        {
            GameObject goHUD = Instantiate<GameObject>(m_goNpcHUD, m_trNpc);
            goHUD.name = nNpcId.ToString();
            goHUD.transform.position = new Vector3(0, 0, 0);
            goHUD.transform.localScale = new Vector3(1, 1, 1);
            goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trNpcHUD = goHUD.transform;

            Text textName = trNpcHUD.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csNpcInfo.Name;

            Text textNickname = trNpcHUD.Find("TextNickname").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNickname);
            textNickname.text = csNpcInfo.Nick;

            if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                textName.color = CsUIData.Instance.ColorWhite;
                textNickname.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textName.color = new Color32(187, 32, 27, 255);
                textNickname.color = new Color32(187, 32, 27, 255);
            }
        }

        QuestMarkQuestCheck(trNpcHUD);

        return trNpcHUD.GetComponent<RectTransform>();
    }

    RectTransform CreateUndergroundMazeNpcHUD(int nNpcId)
    {
        CsUndergroundMazeNpc csUndergroundMazeNpc = CsDungeonManager.Instance.UndergroundMazeFloor.GetUndergroundMazeNpc(nNpcId);

        Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

        if (trNpcHUD == null)
        {
            GameObject goHUD = Instantiate<GameObject>(m_goNpcHUD, m_trNpc);
            goHUD.name = nNpcId.ToString();
            goHUD.transform.position = new Vector3(0, 0, 0);
            goHUD.transform.localScale = new Vector3(1, 1, 1);
            goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trNpcHUD = goHUD.transform;

            Text textName = trNpcHUD.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csUndergroundMazeNpc.Name;

            Text textNickname = trNpcHUD.Find("TextNickname").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNickname);
            //textNickname.text = csUndergroundMazeNpc.Nick;
        }

        return trNpcHUD.GetComponent<RectTransform>();
    }

    void DeleteNpcHUD(int nNpcId)
    {
        if (this)
        {
            Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

            if (trNpcHUD != null)
            {
                Destroy(trNpcHUD.gameObject);
            }
        }
    }

	#endregion Npc HUD

	#region Monster HUD

	//---------------------------------------------------------------------------------------------------
	RectTransform CreateMonsterHUD(long lInstanceId, CsMonsterInfo csMonsterInfo, string strHeroName, bool bExclusive, int nHp)
	{
		Transform trMonsterHUD = m_trMonster.Find(lInstanceId.ToString());

		if (trMonsterHUD == null)
		{
			GameObject goHUD = Instantiate<GameObject>(m_goMonsterHUD, m_trMonster);
			goHUD.name = lInstanceId.ToString();
			goHUD.transform.position = new Vector3(0, 0, 0);
			goHUD.transform.localScale = new Vector3(1, 1, 1);
			goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

			trMonsterHUD = goHUD.transform;
			Transform trTextMonsterName = trMonsterHUD.Find("TextMonsterName");
			Transform trTextHeroName = trMonsterHUD.Find("TextHeroName");

			Text textMonsterName = trTextMonsterName.GetComponent<Text>();
			Text textHeroName = trTextHeroName.GetComponent<Text>();
			CsUIData.Instance.SetFont(textMonsterName);
			CsUIData.Instance.SetFont(textHeroName);

			if (bExclusive)
			{
				textMonsterName.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MON_EXC1"), csMonsterInfo.Name);
				textHeroName.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MON_EXC2"), strHeroName);
			}
			else
			{
				trTextMonsterName.gameObject.SetActive(false);
				trTextHeroName.gameObject.SetActive(false);
				trMonsterHUD.GetComponent<CsDungeonMonsterHUD>().Init(nHp);
			}

            Slider sliderMonsterHp = trMonsterHUD.Find("SliderMonsterHp").GetComponent<Slider>();
            sliderMonsterHp.maxValue = csMonsterInfo.MaxHp;
            sliderMonsterHp.value = nHp;
		}

		return trMonsterHUD.GetComponent<RectTransform>();
	}

    //---------------------------------------------------------------------------------------------------
	RectTransform CreateDungeonMonsterHUD(EnDungeonPlay enDungeonPlay, long lInstanceId, CsMonsterInfo csMonsterInfo, bool bIsBossMonster, int nHalidomId)
    {
        Transform trMonsterHUD = m_trMonster.Find(lInstanceId.ToString());

        if (trMonsterHUD == null)
        {
            GameObject goHUD = Instantiate<GameObject>(m_goMonsterHUD, m_trMonster);
            goHUD.name = lInstanceId.ToString();
            goHUD.transform.position = new Vector3(0, 0, 0);
            goHUD.transform.localScale = new Vector3(1, 1, 1);
            goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trMonsterHUD = goHUD.transform;

			Transform trTextMonsterName = trMonsterHUD.Find("TextMonsterName");
			Transform trTextHeroName = trMonsterHUD.Find("TextHeroName");

			Text textMonsterName = trTextMonsterName.GetComponent<Text>();
			CsUIData.Instance.SetFont(textMonsterName);

			Text textHeroName = trTextHeroName.GetComponent<Text>();
			CsUIData.Instance.SetFont(textHeroName);

			switch (enDungeonPlay)
			{
				case EnDungeonPlay.Story:
					trTextMonsterName.gameObject.SetActive(false);
					trTextHeroName.gameObject.SetActive(false);
					break;

				case EnDungeonPlay.WisdomTemple:
					trTextMonsterName.gameObject.SetActive(true);
					textMonsterName.text = csMonsterInfo.Name;

					trTextHeroName.gameObject.SetActive(false);
					break;

				case EnDungeonPlay.FearAltar:
					trTextMonsterName.gameObject.SetActive(true);
					trTextHeroName.gameObject.SetActive(!bIsBossMonster);
					
					if (bIsBossMonster)
					{
						textMonsterName.text = csMonsterInfo.Name;
						textMonsterName.color = new Color32(254, 148, 0, 255);
					}
					else
					{
						if (nHalidomId != 0)
						{
							CsFearAltarHalidom csFearAltarHalidom = CsGameData.Instance.FearAltar.GetFearAltarHalidom(nHalidomId);

							if (csFearAltarHalidom != null)
							{
								string strStar = CsConfiguration.Instance.GetString("PUBLIC_STAR");

								StringBuilder sb = new StringBuilder();

								for (int i = 0; i < csFearAltarHalidom.FearAltarHalidomLevel.HalidomLevel; i++)
								{
									sb.Append(strStar);
								}

								textMonsterName.text = sb.ToString();
							}
						}

						textMonsterName.color = new Color32(35, 216, 128, 255);
						
						textHeroName.text = csMonsterInfo.Name;
						textHeroName.color = new Color32(35, 216, 128, 255);
					}
					break;
			}
            
            Slider sliderMonsterHp = trMonsterHUD.Find("SliderMonsterHp").GetComponent<Slider>();
            sliderMonsterHp.maxValue = csMonsterInfo.MaxHp;
            sliderMonsterHp.value = csMonsterInfo.MaxHp;
        }

        return trMonsterHUD.GetComponent<RectTransform>();
    }

    //---------------------------------------------------------------------------------------------------
    void DeleteMonsterHUD(long lInstanceId)
    {
        if (this)
        {
            Transform trMonsterHUD = m_trMonster.Find(lInstanceId.ToString());

            if (trMonsterHUD != null)
            {
                Destroy(trMonsterHUD.gameObject);
            }
        }
    }

    #endregion Monster HUD

    #region Cart HUD

    RectTransform CreateCartHUD(long lInstanceId, CsCartObject csCartObject)
    {
        Transform trCartHUD = m_trCart.Find(lInstanceId.ToString());

        if (trCartHUD == null)
        {
            GameObject goHUD = Instantiate<GameObject>(m_goCartHUD, m_trCart);
            goHUD.name = lInstanceId.ToString();
            goHUD.transform.position = new Vector3(0, 0, 0);
            goHUD.transform.localScale = new Vector3(1, 1, 1);
            goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trCartHUD = goHUD.transform;

            Text textCartName = trCartHUD.Find("TextCartName").GetComponent<Text>();
            textCartName.text = string.Format(CsGameData.Instance.GetCart(csCartObject.Cart.CartId).Name, csCartObject.OwnerName, csCartObject.Cart.CartGrade.Name);
            CsUIData.Instance.SetFont(textCartName);
        }

        return trCartHUD.GetComponent<RectTransform>();
    }

    void DeleteCartHUD(long lInstanceId)
    {
        if (this)
        {
            Transform trCartHUD = m_trCart.Find(lInstanceId.ToString());

            if (trCartHUD != null)
            {
                Destroy(trCartHUD.gameObject);
            }
        }
    }

    #endregion Cart HUD

    #region GuildNpc HUD

    RectTransform CreateGuildNpcHUD(int nNpcId)
    {
        CsGuildTerritoryNpc csGuildTerritoryNpc = CsGameData.Instance.GuildTerritory.GetGuildTerritoryNpc(nNpcId);

        Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

        if (trNpcHUD == null)
        {
            GameObject goHUD = Instantiate<GameObject>(m_goNpcHUD, m_trNpc);
            goHUD.name = nNpcId.ToString();
            goHUD.transform.position = new Vector3(0, 0, 0);
            goHUD.transform.localScale = new Vector3(1, 1, 1);
            goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trNpcHUD = goHUD.transform;

            Text textName = trNpcHUD.Find("TextName").GetComponent<Text>();
            textName.text = csGuildTerritoryNpc.Name;
            CsUIData.Instance.SetFont(textName);

            trNpcHUD.Find("TextNickname").gameObject.SetActive(false);
        }

		QuestMarkQuestCheck(trNpcHUD);

        return trNpcHUD.GetComponent<RectTransform>();
    }

    void DeleteGuildNpcHUD(int nNpcId)
    {
        if (this)
        {
            Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

            if (trNpcHUD != null)
            {
                Destroy(trNpcHUD.gameObject);
            }
        }
    }

    #endregion GuildNpc HUD

    #region NationWarNpc HUD

    RectTransform CreateNationWarNpcHUD(int nNpcId)
    {
        CsNationWarNpc csNationWarNpc = CsGameData.Instance.NationWar.GetNationWarNpc(nNpcId);
        Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

        if (trNpcHUD == null)
        {
            GameObject goHUD = Instantiate<GameObject>(m_goNpcHUD, m_trNpc);
            goHUD.name = nNpcId.ToString();
            goHUD.transform.position = new Vector3(0, 0, 0);
            goHUD.transform.localScale = new Vector3(1, 1, 1);
            goHUD.transform.localEulerAngles = new Vector3(0, 0, 0);

            trNpcHUD = goHUD.transform;

            Text textName = trNpcHUD.Find("TextName").GetComponent<Text>();
            textName.text = csNationWarNpc.Name;
            CsUIData.Instance.SetFont(textName);

            trNpcHUD.Find("TextNickname").gameObject.SetActive(false);
        }

        return trNpcHUD.GetComponent<RectTransform>();
    }

    void DeleteNationWarNpcHUD(int nNpcId)
    {
        if (this)
        {
            Transform trNpcHUD = m_trNpc.Find(nNpcId.ToString());

            if (trNpcHUD != null)
            {
                Destroy(trNpcHUD.gameObject);
            }
        }
    }

    #endregion NationWarNpc HUD

    //---------------------------------------------------------------------------------------------------
    void DestroyAllHUD()
    {
        int nChildCount = transform.childCount;

        for (int i = 0; i < nChildCount; i++)
        {
            Transform trChild = transform.GetChild(i);

            for (int j = 0; j < trChild.childCount; j++)
            {
                Destroy(trChild.GetChild(j).gameObject);
            }
        }
    }

    void DisplayChattingMessage(Guid guidSenderId, string strMessage)
    {
        Transform trHUD = null;

        if (guidSenderId == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            trHUD = m_trMyHero.Find(guidSenderId.ToString());
        }
        else
        {
            trHUD = m_trHero.Find(guidSenderId.ToString());
        }

        if (trHUD != null)
        {
            Transform trMessageBubble = trHUD.Find("MessageBubble");
            trMessageBubble.gameObject.SetActive(false);
            trMessageBubble.Find("ImageText/Text").GetComponent<Text>().text = strMessage;
            trMessageBubble.gameObject.SetActive(true);
        }

    }
}
