using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-22)
//---------------------------------------------------------------------------------------------------

public enum EnReviveLocationType
{
    Continent = 1,
    MainQuestDungeon = 2,
    StoryDungeon = 3,
    ExpDungeon = 4,
    GoldDungeon = 5,
    UndergroundMaze = 6,
    AncientRelic = 7,
    SoulConveter = 8,
    EliteDungeon = 9,
    GuildTerritory = 10, 
	RuinsReclaim = 11,
    InfiniteWar = 12, 
	FearAltar = 13,
    WarMemory = 14, 
	BiographyQuestDungeon = 15,
    DragonNest = 16, 
    TradeShip = 17, 
    AnkouTomb = 18, 
}

public class CsPopupRevive : MonoBehaviour
{
    Button m_buttonSaftyRevive;
    Button m_buttonImmediateRevive;
    Button m_buttonAbandon;

    Text m_textWatingTime;
    Text m_textSaftyReviveMove;

    float m_flTime = 0;
    float m_flRevivalWatingTime;
    float m_flDungeonRevivalWatingTime = 0;

    string m_strWatingTime;

    EnReviveLocationType m_enReviveLocationType = EnReviveLocationType.Continent;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventImmediateRevive += OnEventImmediateRevive;
        CsGameEventUIToUI.Instance.EventContinentSaftyRevive += OnEventContinentSaftyRevive;

        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonSaftyRevive += OnEventMainQuestDungeonSaftyRevive;

        CsDungeonManager.Instance.EventStoryDungeonRevive += OnEventStoryDungeonRevive;
        CsDungeonManager.Instance.EventGoldDungeonRevive += OnEventGoldDungeonRevive;
        CsDungeonManager.Instance.EventExpDungeonRevive += OnEventExpDungeonRevive;
        CsDungeonManager.Instance.EventAncientRelicRevive += OnEventAncientRelicRevive;
        CsDungeonManager.Instance.EventSoulCoveterRevive += OnEventSoulCoveterRevive;

        CsDungeonManager.Instance.EventAncientRelicBanished += OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventArtifactRoomBanished += OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventExpDungeonBanished += OnEventExpDungeonBanished;
        CsDungeonManager.Instance.EventGoldDungeonBanished += OnEventGoldDungeonBanished;
        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;
        CsDungeonManager.Instance.EventSoulCoveterBanished += OnEventSoulCoveterBanished;
        CsDungeonManager.Instance.EventEliteDungeonRevive += OnEventEliteDungeonRevive;
        CsDungeonManager.Instance.EventUndergroundMazeRevive += OnEventUndergroundMazeRevive;
		CsDungeonManager.Instance.EventRuinsReclaimRevive += OnEventRuinsReclaimRevive;
		CsDungeonManager.Instance.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;
        CsDungeonManager.Instance.EventInfiniteWarRevive += OnEventInfiniteWarRevive;
        CsDungeonManager.Instance.EventInfiniteWarBanished += OnEventInfiniteWarBanished;
		CsDungeonManager.Instance.EventFearAltarRevive += OnEventFearAltarRevive;
        CsDungeonManager.Instance.EventFearAltarBanished += OnEventFearAltarBanished;
        CsDungeonManager.Instance.EventWarMemoryRevive += OnEventWarMemoryRevive;
        CsDungeonManager.Instance.EventWarMemoryBanished += OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventDragonNestRevive += OnEventDragonNestRevive;
        CsDungeonManager.Instance.EventDragonNestBanished += OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventTradeShipRevive += OnEventTradeShipRevive;
        CsDungeonManager.Instance.EventTradeShipBanished += OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventAnkouTombRevive += OnEventAnkouTombRevive;
        CsDungeonManager.Instance.EventAnkouTombBanished += OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAncientRelicClear += OnEventAncientRelicClear;
        CsDungeonManager.Instance.EventAncientRelicFail += OnEventAncientRelicFail;
        CsDungeonManager.Instance.EventExpDungeonClear += OnEventExpDungeonClear;
        CsDungeonManager.Instance.EventExpDungeonWaveTimeout += OnEventExpDungeonWaveTimeout;
        CsDungeonManager.Instance.EventGoldDungeonClear += OnEventGoldDungeonClear;
        CsDungeonManager.Instance.EventGoldDungeonFail += OnEventGoldDungeonFail;
        CsDungeonManager.Instance.EventStoryDungeonClear += OnEventStoryDungeonClear;
        CsDungeonManager.Instance.EventStoryDungeonFail += OnEventStoryDungeonFail;
        CsDungeonManager.Instance.EventSoulCoveterClear += OnEventSoulCoveterClear;
        CsDungeonManager.Instance.EventSoulCoveterFail += OnEventSoulCoveterFail;
        CsDungeonManager.Instance.EventEliteDungeonClear += OnEventEliteDungeonClear;
        CsDungeonManager.Instance.EventEliteDungeonFail += OnEventEliteDungeonFail;
        CsDungeonManager.Instance.EventRuinsReclaimClear += OnEventRuinsReclaimClear;
        CsDungeonManager.Instance.EventRuinsReclaimFail += OnEventRuinsReclaimFail;
        CsDungeonManager.Instance.EventInfiniteWarClear += OnEventInfiniteWarClear;
        CsDungeonManager.Instance.EventFearAltarClear += OnEventFearAltarClear;
        CsDungeonManager.Instance.EventFearAltarFail += OnEventFearAltarFail;
        CsDungeonManager.Instance.EventWarMemoryClear += OnEventWarMemoryClear;
        CsDungeonManager.Instance.EventWarMemoryFail += OnEventWarMemoryFail;
        CsDungeonManager.Instance.EventDragonNestClear += OnEventDragonNestClear;
        CsDungeonManager.Instance.EventDragonNestFail += OnEventDragonNestFail;
        CsDungeonManager.Instance.EventTradeShipClear += OnEventTradeShipClear;
        CsDungeonManager.Instance.EventTradeShipFail += OnEventTradeShipFail;
        CsDungeonManager.Instance.EventAnkouTombClear += OnEventAnkouTombClear;
        CsDungeonManager.Instance.EventAnkouTombFail += OnEventAnkouTombFail;
        
        // 국가전
        CsNationWarManager.Instance.EventNationWarRevive += OnEventNationWarRevive;

        // 길드 영지 부활
        CsGuildManager.Instance.EventGuildTerritoryEnterForGuildTerritoryRevival += OnEventGuildTerritoryEnterForGuildTerritoryRevival;

        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        // 1초마다 실행.
        if (m_flTime + 1f < Time.time)
        {
            if (m_enReviveLocationType == EnReviveLocationType.Continent || m_enReviveLocationType == EnReviveLocationType.UndergroundMaze || m_enReviveLocationType == EnReviveLocationType.GuildTerritory)
            {
                if (m_textWatingTime != null)
                {
                    m_textWatingTime.text = string.Format(m_strWatingTime, (int)(m_flRevivalWatingTime - Time.realtimeSinceStartup));

                    if (m_flRevivalWatingTime > -1)
                    {
                        if (m_flRevivalWatingTime - Time.realtimeSinceStartup <= 0)
                        {
                            switch (m_enReviveLocationType)
                            {
                                case EnReviveLocationType.Continent:
                                    CsCommandEventManager.Instance.SendContinentSaftyRevive();
                                    break;
                                case EnReviveLocationType.UndergroundMaze:
                                    CsDungeonManager.Instance.SendUndergroundMazeRevive();
                                    break;
                                case EnReviveLocationType.GuildTerritory:
                                    CsGuildManager.Instance.SendGuildTerritoryRevive();
                                    break;
                            }

                            m_flRevivalWatingTime = -1;
                        }
                    }
                }
            }
            else
            {
                if (m_textSaftyReviveMove != null)
                {
                    if (m_flDungeonRevivalWatingTime - Time.realtimeSinceStartup > 0)
                    {
                        m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), (m_flDungeonRevivalWatingTime - Time.realtimeSinceStartup).ToString("#0"));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(m_textSaftyReviveMove.text))
                        {
                            m_textSaftyReviveMove.text = string.Empty;
                        }

                        if (m_buttonSaftyRevive.interactable == false)
                        {
                            m_buttonSaftyRevive.interactable = true;
                        }
                    }
                }
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventImmediateRevive -= OnEventImmediateRevive;
        CsGameEventUIToUI.Instance.EventContinentSaftyRevive -= OnEventContinentSaftyRevive;

        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonSaftyRevive -= OnEventMainQuestDungeonSaftyRevive;

        CsDungeonManager.Instance.EventStoryDungeonRevive -= OnEventStoryDungeonRevive;
        CsDungeonManager.Instance.EventGoldDungeonRevive -= OnEventGoldDungeonRevive;
        CsDungeonManager.Instance.EventExpDungeonRevive -= OnEventExpDungeonRevive;
        CsDungeonManager.Instance.EventAncientRelicRevive -= OnEventAncientRelicRevive;
        CsDungeonManager.Instance.EventSoulCoveterRevive -= OnEventSoulCoveterRevive;

        CsDungeonManager.Instance.EventAncientRelicBanished -= OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventArtifactRoomBanished -= OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventExpDungeonBanished -= OnEventExpDungeonBanished;
        CsDungeonManager.Instance.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;
        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;
        CsDungeonManager.Instance.EventSoulCoveterBanished -= OnEventSoulCoveterBanished;
        CsDungeonManager.Instance.EventEliteDungeonRevive -= OnEventEliteDungeonRevive;
        CsDungeonManager.Instance.EventUndergroundMazeRevive -= OnEventUndergroundMazeRevive;
		CsDungeonManager.Instance.EventRuinsReclaimRevive -= OnEventRuinsReclaimRevive;
        CsDungeonManager.Instance.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;
        CsDungeonManager.Instance.EventInfiniteWarRevive -= OnEventInfiniteWarRevive;
        CsDungeonManager.Instance.EventInfiniteWarBanished -= OnEventInfiniteWarBanished;
		CsDungeonManager.Instance.EventFearAltarRevive -= OnEventFearAltarRevive;
        CsDungeonManager.Instance.EventFearAltarBanished -= OnEventFearAltarBanished;
        CsDungeonManager.Instance.EventWarMemoryRevive -= OnEventWarMemoryRevive;
        CsDungeonManager.Instance.EventWarMemoryBanished -= OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventDragonNestRevive -= OnEventDragonNestRevive;
        CsDungeonManager.Instance.EventDragonNestBanished -= OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventTradeShipRevive -= OnEventTradeShipRevive;
        CsDungeonManager.Instance.EventTradeShipBanished -= OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventAnkouTombRevive -= OnEventAnkouTombRevive;
        CsDungeonManager.Instance.EventAnkouTombBanished -= OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAncientRelicClear -= OnEventAncientRelicClear;
        CsDungeonManager.Instance.EventAncientRelicFail -= OnEventAncientRelicFail;
        CsDungeonManager.Instance.EventExpDungeonClear -= OnEventExpDungeonClear;
        CsDungeonManager.Instance.EventExpDungeonWaveTimeout -= OnEventExpDungeonWaveTimeout;
        CsDungeonManager.Instance.EventGoldDungeonClear -= OnEventGoldDungeonClear;
        CsDungeonManager.Instance.EventGoldDungeonFail -= OnEventGoldDungeonFail;
        CsDungeonManager.Instance.EventStoryDungeonClear -= OnEventStoryDungeonClear;
        CsDungeonManager.Instance.EventStoryDungeonFail -= OnEventStoryDungeonFail;
        CsDungeonManager.Instance.EventSoulCoveterClear -= OnEventSoulCoveterClear;
        CsDungeonManager.Instance.EventSoulCoveterFail -= OnEventSoulCoveterFail;
        CsDungeonManager.Instance.EventEliteDungeonClear -= OnEventEliteDungeonClear;
        CsDungeonManager.Instance.EventEliteDungeonFail -= OnEventEliteDungeonFail;
        CsDungeonManager.Instance.EventRuinsReclaimClear -= OnEventRuinsReclaimClear;
        CsDungeonManager.Instance.EventRuinsReclaimFail -= OnEventRuinsReclaimFail;
        CsDungeonManager.Instance.EventInfiniteWarClear -= OnEventInfiniteWarClear;
        CsDungeonManager.Instance.EventFearAltarClear -= OnEventFearAltarClear;
        CsDungeonManager.Instance.EventFearAltarFail -= OnEventFearAltarFail;
        CsDungeonManager.Instance.EventWarMemoryClear -= OnEventWarMemoryClear;
        CsDungeonManager.Instance.EventWarMemoryFail -= OnEventWarMemoryFail;
        CsDungeonManager.Instance.EventDragonNestClear -= OnEventDragonNestClear;
        CsDungeonManager.Instance.EventDragonNestFail -= OnEventDragonNestFail;
        CsDungeonManager.Instance.EventTradeShipClear -= OnEventTradeShipClear;
        CsDungeonManager.Instance.EventTradeShipFail -= OnEventTradeShipFail;
        CsDungeonManager.Instance.EventAnkouTombClear -= OnEventAnkouTombClear;
        CsDungeonManager.Instance.EventAnkouTombFail -= OnEventAnkouTombFail;

        // 국가전
        CsNationWarManager.Instance.EventNationWarRevive -= OnEventNationWarRevive;

        // 길드 영지 부활
        CsGuildManager.Instance.EventGuildTerritoryEnterForGuildTerritoryRevival -= OnEventGuildTerritoryEnterForGuildTerritoryRevival;

        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;
    }

    #region Event

	// 유적 탈환
	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRevive(ClientCommon.PDVector3 pdVector3, float flRotationY)
	{
		ClosePopup();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nPrevContinentId)
	{
		ClosePopup();
	}

    // 무한 대전
    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBanished(int nPrevContinentId)
    {
        ClosePopup();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarRevive()
	{
		ClosePopup();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nPreviousContinentId)
	{
		ClosePopup();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryRevive(ClientCommon.PDVector3 position, float flRotationY)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryBanished(int nPreviousContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestRevive()
    {
        ClosePopup(); 
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestBanished(int nPreviousContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipBanished(int nPreviousContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombBanished(int nPreviousContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicClear()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonClear(bool bLevelUp, long lExp)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonWaveTimeout()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonClear(long lRewardGold)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonClear(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterClear(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonClear()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRuinsReclaimClear(ClientCommon.PDItemBooty[] aPDItemBooty, ClientCommon.PDItemBooty randomBooty,
        System.Guid monsterTerminatorHeroId, string monsterTerminatorHeroName, ClientCommon.PDItemBooty monsterTerminatorBooty,
        System.Guid ultimateAttackKingHeroId, string ultimateAttackKingHeroName, ClientCommon.PDItemBooty ultimateAttackKingBooty,
        System.Guid partyVolunteerHeroId, string partyVolunteerHeroName, ClientCommon.PDItemBooty partyVolunteerBooty)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRuinsReclaimFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarClear(ClientCommon.PDInfiniteWarRanking[] aPDInfiniteWarRanking, ClientCommon.PDItemBooty[] aPDItemBooty, ClientCommon.PDItemBooty[] aPDItemBootyRanking)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFearAltarClear(long lAcquiredExp, ClientCommon.PDFearAltarHero[] aPDFearAltarHero, bool bLevelUp)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFearAltarFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryClear(ClientCommon.PDWarMemoryRanking[] aPDWarMemoryRanking, ClientCommon.PDItemBooty[] aPDItemBooty, long lAcquiredExp, bool bLevelUp)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestClear(ClientCommon.PDSimpleHero[] arrPDSimpleHero)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestFail()
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipClear(bool bLevelUp, long lAcquiredExp, ClientCommon.PDItemBooty aPDItemBooty)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipFail(bool bLevelUp, long lAcquiredExp)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombClear(bool bLevelUp, long lAcquiredExp, ClientCommon.PDItemBooty aPDItemBooty)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombFail(bool bLevelUp, long lAcquiredExp)
    {
        ClosePopup();
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //지하 미로
    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeRevive()
    {
        ClosePopup();
    }

    // 정예 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonRevive()
    {
        ClosePopup();
    }

    //영혼을 탐하는 자
    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterBanished(int nContinentId)
    {
        ClosePopup();
    }

    //고대인의 유적
    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicBanished(int nContinentId)
    {
        ClosePopup();
    }

    //고대 유물의 방
    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomBanished(int nContinentId)
    {
        ClosePopup();
    }

    //경험치던전
    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonBanished(int nContinentId)
    {
        ClosePopup();
    }

    //골드던전
    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonBanished(int nContinentId)
    {
        ClosePopup();
    }

    //스토리던전
    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonSaftyRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventImmediateRevive()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentSaftyRevive(int nContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarRevive(int nContinentId)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryEnterForGuildTerritoryRevival(System.Guid guidPlaceInstanceId, ClientCommon.PDHero[] arrHeroes, ClientCommon.PDMonsterInstance[] arrMonsters, ClientCommon.PDVector3 vecPosition, float flRotationY)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bChaegeScene)
    {
        ClosePopup();
    }

    #endregion Event

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickSaftyRevive()
    {
        SaftyRevive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickImmediateRevive()
    {
		int nMaxPaidRevivalcount = CsGameData.Instance.PaidImmediateRevivalList.Max(paidImmediateRevival => paidImmediateRevival.RevivalCount);
        
		int nPaidCount = CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount + 1;
		
		if (nPaidCount > nMaxPaidRevivalcount)
        {
			nPaidCount = nMaxPaidRevivalcount;
        }

		int m_nFreeImmediateRevivalAdditionalDailyCount = 0;
		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);

		if (csArtifact != null)
			m_nFreeImmediateRevivalAdditionalDailyCount = csArtifact.FreeImmediateRevivalAdditionalDailyCount;

		if (CsGameConfig.Instance.FreeImmediateRevivalDailyCount + m_nFreeImmediateRevivalAdditionalDailyCount > CsGameData.Instance.MyHeroInfo.FreeImmediateRevivalDailyCount)
        {
            m_flRevivalWatingTime = -1;
            CsCommandEventManager.Instance.SendImmediateRevive();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("A35_TXT_03002"),
                CsGameData.Instance.GetPaidImmdiateRevival(nPaidCount).RequiredDia),
                CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickPaidImmediateRevive,
                CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickPaidImmediateReviveCancel, true);
        }
    }


    //---------------------------------------------------------------------------------------------------
    void OnClickDungeonSaftyRevive()
    {
        if (m_flDungeonRevivalWatingTime - Time.realtimeSinceStartup <= 0)
        {
            switch (m_enReviveLocationType)
            {
                case EnReviveLocationType.MainQuestDungeon:
                    CsMainQuestDungeonManager.Instance.SendMainQuestDungeonSaftyRevive();
                    break;

                case EnReviveLocationType.StoryDungeon:
                    CsDungeonManager.Instance.SendStoryDungeonRevive();
                    break;

                case EnReviveLocationType.ExpDungeon:
                    CsDungeonManager.Instance.SendExpDungeonRevive();
                    break;

                case EnReviveLocationType.GoldDungeon:
                    CsDungeonManager.Instance.SendGoldDungeonRevive();
                    break;

                case EnReviveLocationType.AncientRelic:
                    CsDungeonManager.Instance.SendAncientRelicRevive();
                    break;

                case EnReviveLocationType.SoulConveter:
                    CsDungeonManager.Instance.SendSoulCoveterRevive();
                    break;

                case EnReviveLocationType.EliteDungeon:
                    CsDungeonManager.Instance.SendEliteDungeonRevive();
                    break;

				case EnReviveLocationType.RuinsReclaim:
					CsDungeonManager.Instance.SendRuinsReclaimRevive();
					break;

                case EnReviveLocationType.InfiniteWar:
                    CsDungeonManager.Instance.SendInfiniteWarRevive();
                    break;

				case EnReviveLocationType.FearAltar:
					CsDungeonManager.Instance.FearAltarRevive();
					break;

                case EnReviveLocationType.WarMemory:
                    CsDungeonManager.Instance.SendWarMemoryRevive();
                    break;

				case EnReviveLocationType.BiographyQuestDungeon:
					CsDungeonManager.Instance.SendBiographyQuestDungeonRevive();
					break;

                case EnReviveLocationType.DragonNest:
                    CsDungeonManager.Instance.SendDragonNestRevive();
                    break;

                case EnReviveLocationType.TradeShip:
                    CsDungeonManager.Instance.SendTradeShipRevive();
                    break;

                case EnReviveLocationType.AnkouTomb:
                    CsDungeonManager.Instance.SendAnkouTombRevive();
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDungeonExit()
    {
        switch (CsUIData.Instance.DungeonInNow)
        {
            case EnDungeon.MainQuestDungeon:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonQuestDungeon,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.StoryDungeon:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonStoryDungeon,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.ExpDungeon:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonExpDungeon,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.GoldDungeon:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonGoldDungeon,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.UndergroundMaze:
                CsDungeonManager.Instance.SendUndergroundMazeExit();
                break;

            case EnDungeon.ArtifactRoom:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonArtifactRoom,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;
            case EnDungeon.AncientRelic:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonAncientRelic,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.FieldOfHonor:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonFieldOfHonor,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.SoulCoveter:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonSoulConveter,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;
            case EnDungeon.EliteDungeon:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonEliteDungeon,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

			case EnDungeon.RuinsReclaim:
				CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
												  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonRuinsReclaim,
												  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
				break;

            case EnDungeon.InfiniteWar:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonInfiniteWar,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

			case EnDungeon.FearAltar:
				CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
												  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonFearAltar,
												  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
				break;

            case EnDungeon.WarMemory:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonWarMemory,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

			case EnDungeon.BiographyDungeon:
				CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
												  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonBiographyQuestDungeon,
												  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
				break;

            case EnDungeon.DragonNest:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonDragonNest,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.TradeShip:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonTradeShip,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

            case EnDungeon.AnkouTomb:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonAnkouTomb,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonQuestDungeon()
    {
        CsMainQuestDungeonManager.Instance.SendMainQuestDungeonAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonStoryDungeon()
    {
        CsDungeonManager.Instance.SendStoryDungeonAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonExpDungeon()
    {
        CsDungeonManager.Instance.SendExpDungeonAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonGoldDungeon()
    {
        CsDungeonManager.Instance.SendGoldDungeonAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonAncientRelic()
    {
        CsDungeonManager.Instance.SendAncientRelicAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonArtifactRoom()
    {
        CsDungeonManager.Instance.SendArtifactRoomAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonFieldOfHonor()
    {
        CsDungeonManager.Instance.SendFieldOfHonorAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonSoulConveter()
    {
        CsDungeonManager.Instance.SendSoulCoveterAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonEliteDungeon()
    {
        CsDungeonManager.Instance.SendEliteDungeonAbandon();
        ClosePopup();
    }

	//---------------------------------------------------------------------------------------------------
	void AbandonRuinsReclaim()
	{
		CsDungeonManager.Instance.SendRuinsReclaimAbandon();
		ClosePopup();
	}

    //---------------------------------------------------------------------------------------------------
    void AbandonInfiniteWar()
    {
        CsDungeonManager.Instance.SendInfiniteWarAbandon();
        ClosePopup();
    }

	//---------------------------------------------------------------------------------------------------
	void AbandonFearAltar()
	{
		CsDungeonManager.Instance.SendFearAltarAbandon();
		ClosePopup();
	}

    //---------------------------------------------------------------------------------------------------
    void AbandonWarMemory()
    {
        CsDungeonManager.Instance.SendWarMemoryAbandon();
        ClosePopup();
    }

	//---------------------------------------------------------------------------------------------------
	void AbandonBiographyQuestDungeon()
	{
		CsDungeonManager.Instance.SendBiographyQuestDungeonAbandon();
		ClosePopup();
	}

    //---------------------------------------------------------------------------------------------------
    void AbandonDragonNest()
    {
        CsDungeonManager.Instance.SendDragonNestAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonTradeShip()
    {
        CsDungeonManager.Instance.SendTradeShipAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonAnkouTomb()
    {
        CsDungeonManager.Instance.SendAnkouTombAbandon();
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarRevive()
    {
        if (CsNationWarManager.Instance.MyNationWarDeclaration != null)
        {
            if (CsNationWarManager.Instance.MyNationWarDeclaration.Status == EnNationWarDeclaration.Current)
            {
                if (CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    if (CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        SaftyRevive();
                    }
                    else
                    {
                        CsNationWarManager.Instance.SendNationWarRevive();
                    }
                }
                else
                {
                    SaftyRevive();
                }
            }
            else
            {
                SaftyRevive();
            }
        }
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    public void DisplayRevive(EnReviveLocationType enReviveLocationType, string strName)
    {
        Debug.Log("## DisplayRevive ## : " + enReviveLocationType);
        m_enReviveLocationType = enReviveLocationType;
        DisplayName(strName);

        switch (enReviveLocationType)
        {
            case EnReviveLocationType.Continent:
                InitializeContinent();
                m_flRevivalWatingTime = CsGameConfig.Instance.AutoSaftyRevivalWatingTime + Time.realtimeSinceStartup;
                break;

            case EnReviveLocationType.MainQuestDungeon:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsMainQuestDungeonManager.Instance.MainQuestDungeon.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsMainQuestDungeonManager.Instance.MainQuestDungeon.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsMainQuestDungeonManager.Instance.MainQuestDungeon.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.StoryDungeon:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.StoryDungeon.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.StoryDungeon.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.StoryDungeon.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.ExpDungeon:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.ExpDungeon.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.ExpDungeon.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.ExpDungeon.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.GoldDungeon:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.GoldDungeon.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.GoldDungeon.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.GoldDungeon.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.UndergroundMaze:
                InitializeContinent();
                m_flRevivalWatingTime = CsGameConfig.Instance.AutoSaftyRevivalWatingTime + Time.realtimeSinceStartup;
                break;

            case EnReviveLocationType.AncientRelic:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.AncientRelic.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.AncientRelic.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.AncientRelic.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.SoulConveter:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.SoulCoveter.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.SoulCoveter.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.SoulCoveter.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.EliteDungeon:
                InitializeDungeonRevive();
                m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.EliteDungeon.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.EliteDungeon.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.EliteDungeon.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.GuildTerritory:
                InitializeContinent();
                m_flRevivalWatingTime = CsGameConfig.Instance.AutoSaftyRevivalWatingTime + Time.realtimeSinceStartup;
                break;

			case EnReviveLocationType.RuinsReclaim:
				InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.RuinsReclaim.SafeRevivalWaitingTime.ToString("#0"));
				m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.RuinsReclaim.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.RuinsReclaim.SafeRevivalWaitingTime);
				break;

            case EnReviveLocationType.InfiniteWar:
                InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.InfiniteWar.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.InfiniteWar.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.InfiniteWar.SafeRevivalWaitingTime);
                break;

			case EnReviveLocationType.FearAltar:
				InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.FearAltar.SafeRevivalWaitingTime.ToString("#0"));
				m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.FearAltar.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.FearAltar.SafeRevivalWaitingTime);
				break;

            case EnReviveLocationType.WarMemory:
                InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.WarMemory.SafeRevivalWaitingTime.ToString("#0"));
				m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.WarMemory.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.WarMemory.SafeRevivalWaitingTime);
                break;

			case EnReviveLocationType.BiographyQuestDungeon:
				InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.BiographyQuestDungeon.SafeRevivalWaitingTime.ToString("#0"));
				m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.BiographyQuestDungeon.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.BiographyQuestDungeon.SafeRevivalWaitingTime);
				break;

            case EnReviveLocationType.DragonNest:
                InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.DragonNest.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.DragonNest.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.DragonNest.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.TradeShip:
                InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.TradeShip.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.TradeShip.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.TradeShip.SafeRevivalWaitingTime);
                break;

            case EnReviveLocationType.AnkouTomb:
                InitializeDungeonRevive();
				m_textSaftyReviveMove.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00007"), CsDungeonManager.Instance.AnkouTomb.SafeRevivalWaitingTime.ToString("#0"));
                m_flDungeonRevivalWatingTime = CsDungeonManager.Instance.AnkouTomb.SafeRevivalWaitingTime + Time.realtimeSinceStartup - GetSafeRevivalWaitingDecTime(CsDungeonManager.Instance.AnkouTomb.SafeRevivalWaitingTime);
                break;
        }
    }

	int GetSafeRevivalWaitingDecTime(int nSafeRevivalWaitingTime)
	{
		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);

		if (csArtifact != null)
		{
			return (int)(nSafeRevivalWaitingTime * (csArtifact.SafeRevivalWaitingDecRate / 10000.0f));
		}
		else
		{
			return 0;
		}
	}

	#region 대륙부활

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
    {
        Transform trReviveList = transform.Find("ImageBackGround/ReviveList");

        //즉시 부활
        m_buttonImmediateRevive = trReviveList.Find("ButtonImmediateRevive").GetComponent<Button>();
        m_buttonImmediateRevive.onClick.RemoveAllListeners();
        m_buttonImmediateRevive.onClick.AddListener(OnClickImmediateRevive);
        m_buttonImmediateRevive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textImmediateRevive = m_buttonImmediateRevive.transform.Find("TextName").GetComponent<Text>();
        Text textImmediateReviveCount = m_buttonImmediateRevive.transform.Find("TextCount").GetComponent<Text>();
        textImmediateRevive.text = CsConfiguration.Instance.GetString("A35_TXT_00004");
        CsUIData.Instance.SetFont(textImmediateReviveCount);
        CsUIData.Instance.SetFont(textImmediateRevive);

		int m_nFreeImmediateRevivalAdditionalDailyCount = 0;
		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);

		if (csArtifact != null)
			m_nFreeImmediateRevivalAdditionalDailyCount = csArtifact.FreeImmediateRevivalAdditionalDailyCount;

		int nMaxFreeRevive = CsGameConfig.Instance.FreeImmediateRevivalDailyCount + m_nFreeImmediateRevivalAdditionalDailyCount;
        int nHeroFreeRevive = CsGameData.Instance.MyHeroInfo.FreeImmediateRevivalDailyCount;

        if (nMaxFreeRevive > nHeroFreeRevive)
        {
            textImmediateReviveCount.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00006"), nMaxFreeRevive - nHeroFreeRevive, nMaxFreeRevive);
            textImmediateReviveCount.gameObject.SetActive(true);
        }
        else
        {
            textImmediateReviveCount.gameObject.SetActive(false);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void InitializeContinent()
    {
        Transform trReviveList = transform.Find("ImageBackGround/ReviveList");

        //자동 부활 시간
        m_strWatingTime = CsConfiguration.Instance.GetString("A35_TXT_00002");
        m_textWatingTime = transform.Find("ImageBackGround/TextWatingTime").GetComponent<Text>();
        m_textWatingTime.text = string.Format(m_strWatingTime, (int)m_flRevivalWatingTime);
        CsUIData.Instance.SetFont(m_textWatingTime);

        //안전 부활
        m_buttonSaftyRevive = trReviveList.Find("ButtonSaftyRevive").GetComponent<Button>();
        m_buttonSaftyRevive.onClick.RemoveAllListeners();
        m_buttonSaftyRevive.onClick.AddListener(OnClickSaftyRevive);
        m_buttonSaftyRevive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textSaftyRevive = m_buttonSaftyRevive.transform.Find("TextName").GetComponent<Text>();
        m_textSaftyReviveMove = m_buttonSaftyRevive.transform.Find("TextMove").GetComponent<Text>();
        textSaftyRevive.text = CsConfiguration.Instance.GetString("A35_TXT_00003");
        m_textSaftyReviveMove.text = CsConfiguration.Instance.GetString("A35_TXT_00005");
        CsUIData.Instance.SetFont(textSaftyRevive);
        CsUIData.Instance.SetFont(m_textSaftyReviveMove);

        Button buttonContryRevive = trReviveList.Find("ButtonContryRevive").GetComponent<Button>();
        buttonContryRevive.onClick.RemoveAllListeners();

        CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        Text textContryReviveName = buttonContryRevive.transform.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContryReviveName);
        textContryReviveName.text = CsConfiguration.Instance.GetString("A70_BTN_00007");

        Text textContryReviveMove = buttonContryRevive.transform.Find("TextMove").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContryReviveMove);
        textContryReviveMove.text = CsConfiguration.Instance.GetString("A70_BTN_00008");

        if (csNationWarDeclaration != null)
        {
            Debug.Log("csNationWarDeclaration : " + csNationWarDeclaration.Status);
        }

        if (csContinent != null && csNationWarDeclaration != null && CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
                csContinent.IsNationWarTarget && csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
        {
            buttonContryRevive.onClick.AddListener(OnClickNationWarRevive);
            buttonContryRevive.gameObject.SetActive(true);
        }
        else
        {
            buttonContryRevive.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPaidImmediateRevive()
    {
		int nMaxPaidRevivalcount = CsGameData.Instance.PaidImmediateRevivalList.Max(paidImmediateRevival => paidImmediateRevival.RevivalCount);

        int nPaidCount = CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount + 1;

		if (nPaidCount > nMaxPaidRevivalcount)
        {
			nPaidCount = nMaxPaidRevivalcount;
        }

        if (CsGameData.Instance.MyHeroInfo.Dia >= CsGameData.Instance.GetPaidImmdiateRevival(nPaidCount).RequiredDia)
        {
            m_flRevivalWatingTime = -1;
            CsCommandEventManager.Instance.SendImmediateRevive();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A35_TXT_03005"),
               CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickDiacharge,
               CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickDiachargeCancel, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPaidImmediateReviveCancel()
    {

    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDiacharge()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, "다이아 상점 준비중입니다.");
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDiachargeCancel()
    {
        int nPaidCount = CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount + 1;
        
        if (nPaidCount > 30)
        {
            nPaidCount = 30;
        }

        CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("A35_TXT_03002"),
              CsGameData.Instance.GetPaidImmdiateRevival(nPaidCount).RequiredDia),
              CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickPaidImmediateRevive,
              CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickPaidImmediateReviveCancel, true);
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopup()
    {
        Transform trRestReward = transform.parent.Find("PopupRestReward");

        if (trRestReward == null)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            trRestReward.gameObject.SetActive(true);
        }

        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayName(string strName)
    {
        Text textDead = transform.Find("ImageBackGround/TextDead").GetComponent<Text>();

        if (textDead != null)
        {
            CsUIData.Instance.SetFont(textDead);

            if (string.IsNullOrEmpty(strName))
            {
                textDead.text = CsConfiguration.Instance.GetString("A35_TXT_00008");
            }
            else
            {
                textDead.text = string.Format(CsConfiguration.Instance.GetString("A35_TXT_00001"), strName);
            }
        }
    }

    #endregion 대륙부활

    #region 메인퀘스트 던전 부활

    //---------------------------------------------------------------------------------------------------
    void InitializeDungeonRevive()
    {
        Transform trReviveList = transform.Find("ImageBackGround/ReviveList");

        Transform trButtonContryRevive = trReviveList.Find("ButtonContryRevive");
        trButtonContryRevive.gameObject.SetActive(false);

        //자동 부활 시간
        m_textWatingTime = transform.Find("ImageBackGround/TextWatingTime").GetComponent<Text>();
        m_textWatingTime.text = "";
        CsUIData.Instance.SetFont(m_textWatingTime);

        //안전 부활
        m_buttonSaftyRevive = trReviveList.Find("ButtonSaftyRevive").GetComponent<Button>();
        m_buttonSaftyRevive.onClick.RemoveAllListeners();
        m_buttonSaftyRevive.onClick.AddListener(OnClickDungeonSaftyRevive);
        m_buttonSaftyRevive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonSaftyRevive.interactable = false;

        Text textSaftyRevive = m_buttonSaftyRevive.transform.Find("TextName").GetComponent<Text>();
        m_textSaftyReviveMove = m_buttonSaftyRevive.transform.Find("TextMove").GetComponent<Text>();
        textSaftyRevive.text = CsConfiguration.Instance.GetString("A35_TXT_00003");

        m_buttonAbandon = trReviveList.Find("ButtonAbandon").GetComponent<Button>();
        m_buttonAbandon.onClick.RemoveAllListeners();
        m_buttonAbandon.onClick.AddListener(OnClickDungeonExit);
        m_buttonAbandon.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonAbandon.gameObject.SetActive(true);

        Text textAbandon = m_buttonAbandon.transform.Find("TextName").GetComponent<Text>();
        Text textAbandonMove = m_buttonAbandon.transform.Find("TextMove").GetComponent<Text>();

        textAbandon.text = CsConfiguration.Instance.GetString("A35_TXT_00009");
        textAbandonMove.text = CsConfiguration.Instance.GetString("A35_TXT_00010");

        CsUIData.Instance.SetFont(textSaftyRevive);
        CsUIData.Instance.SetFont(m_textSaftyReviveMove);
        CsUIData.Instance.SetFont(textAbandon);
        CsUIData.Instance.SetFont(textAbandonMove);
    }

    #endregion 메인퀘스트 던전 부활

    //---------------------------------------------------------------------------------------------------
    void SaftyRevive()
    {
        m_flRevivalWatingTime = -1;

        switch (m_enReviveLocationType)
        {
            case EnReviveLocationType.Continent:
                CsCommandEventManager.Instance.SendContinentSaftyRevive();
                break;

            case EnReviveLocationType.UndergroundMaze:
                CsDungeonManager.Instance.SendUndergroundMazeRevive();
                break;

            case EnReviveLocationType.GuildTerritory:
                CsGuildManager.Instance.SendGuildTerritoryRevive();
                break;
        }
    }
}