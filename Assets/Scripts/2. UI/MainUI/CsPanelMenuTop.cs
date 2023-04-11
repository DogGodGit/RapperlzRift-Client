using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ClientCommon;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-21)
//---------------------------------------------------------------------------------------------------

public class CsPanelMenuTop : MonoBehaviour
{
    Transform m_trPopup;
	Transform m_trPopupList;
    Transform m_trLeftMenuList;
	Transform m_trButtonOpenGift;
	Transform m_trButtonRookieGift;
    Transform m_trPopupMoneyBuff;
	RectTransform m_rtrLeftMenuList;

    GameObject m_goPopupMainMenu;
    GameObject m_goPopupPreview;
    GameObject m_goPopupHelper;
	GameObject m_goPopupBless;
    GameObject m_goPopupMoneyBuff;

    Toggle m_toggleLeftMenu;
    Button m_buttonExit;

    Button m_buttonNationWar;
    Button m_buttonPreview;
    Button m_buttonHelper;
	Button m_buttonBless;
    Button m_buttonMoneyBuff;

    Text m_textTime;

	Text m_textOpenGiftRemainingTime;
	Text m_textRookieGiftRemainingTime;

    TimeSpan m_timeSpan;

    CsPanelAttainment m_csPanelAttainment;

    float m_flTime = 0;
    float m_flLimitTime;
    float m_flRemainingNationWarTime;
    float m_flMoneyBuffLifeTime;

	Vector2 m_vt2LeftMenuPosition;
	int m_nOpenGiftLastDay;

    List<CsMenu> m_listCsMenuTop = new List<CsMenu>();
    List<CsMenu> m_listCsMenuLeft = new List<CsMenu>();

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
		m_nOpenGiftLastDay = CsGameData.Instance.OpenGiftRewardList.Max(gift => gift.Day);

        InitializeUI();

        CsGameEventToUI.Instance.EventPrevContinentEnter += OnEventPrevContinentEnter;
        //CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;
        //CsGameEventUIToUI.Instance.EventLoadingSliderComplete += OnEventLoadingSliderComplete;

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter += OnEventMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepStart += OnEventMainQuestDungeonStepStart;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonClear += OnEventMainQuestDungeonClear;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonFail += OnEventMainQuestDungeonFail;

        //스토리던전
        CsDungeonManager.Instance.EventStoryDungeonEnter += OnEventStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonStepStart += OnEventStoryDungeonStepStart;

        //경험치던전
        CsDungeonManager.Instance.EventExpDungeonEnter += OnEventExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonWaveStart += OnEventExpDungeonWaveStart;

        //골드던전
        CsDungeonManager.Instance.EventGoldDungeonEnter += OnEventGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonStepStart += OnEventGoldDungeonStepStart;

        //지하미로
        CsDungeonManager.Instance.EventUndergroundMazeEnter += OnEventUndergroundMazeEnter;
        CsDungeonManager.Instance.EventUndergroundMazePortalExit += OnEventUndergroundMazePortalExit;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForTransmission += OnEventUndergroundMazeEnterForTransmission;

        //고대 유물의 방
        CsDungeonManager.Instance.EventContinentExitForArtifactRoomEnter += OnEventContinentExitForArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomStart += OnEventArtifactRoomStart;

        //고대인의 유적
        CsDungeonManager.Instance.EventAncientRelicMatchingCancel += OnEventAncientRelicMatchingCancel;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomBanished += OnEventAncientRelicMatchingRoomBanished;
        CsDungeonManager.Instance.EventAncientRelicMatchingStart += OnEventAncientRelicMatchingStart;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomPartyEnter += OnEventAncientRelicMatchingRoomPartyEnter;

        CsDungeonManager.Instance.EventAncientRelicEnter += OnEventAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicStepStart += OnEventAncientRelicStepStart;

        //영혼을 탐하는 자
        CsDungeonManager.Instance.EventSoulCoveterMatchingCancel += OnEventSoulCoveterMatchingCancel;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomBanished += OnEventSoulCoveterMatchingRoomBanished;
        CsDungeonManager.Instance.EventSoulCoveterMatchingStart += OnEventSoulCoveterMatchingStart;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomPartyEnter += OnEventSoulCoveterMatchingRoomPartyEnter;

        CsDungeonManager.Instance.EventSoulCoveterEnter += OnEventSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterWaveStart += OnEventSoulCoveterWaveStart;

        //용맹의 증명
        CsDungeonManager.Instance.EventProofOfValorEnter += OnEventProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorStart += OnEventProofOfValorStart;

		// 지혜의 신전
		CsDungeonManager.Instance.EventWisdomTempleEnter += OnEventWisdomTempleEnter;
		CsDungeonManager.Instance.EventWisdomTempleStepStart += OnEventWisdomTempleStepStart;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimMatchingCancel += OnEventRuinsReclaimMatchingCancel;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomBanished += OnEventRuinsReclaimMatchingRoomBanished;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStart += OnEventRuinsReclaimMatchingStart;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomPartyEnter += OnEventRuinsReclaimMatchingRoomPartyEnter;

		CsDungeonManager.Instance.EventRuinsReclaimEnter += OnEventRuinsReclaimEnter;
		CsDungeonManager.Instance.EventRuinsReclaimStepStart += OnEventRuinsReclaimStepStart;

        // 무한 대전
        CsDungeonManager.Instance.EventInfiniteWarMatchingCancel += OnEventInfiniteWarMatchingCancel;
        CsDungeonManager.Instance.EventInfiniteWarMatchingRoomBanished += OnEventInfiniteWarMatchingRoomBanished;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStart += OnEventInfiniteWarMatchingStart;

        CsDungeonManager.Instance.EventInfiniteWarEnter += OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarStart += OnEventInfiniteWarStart;

		// 공포의 제단
		CsDungeonManager.Instance.EventFearAltarMatchingStart += OnEventFearAltarMatchingStart;
		CsDungeonManager.Instance.EventFearAltarMatchingCancel += OnEventFearAltarMatchingCancel;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomPartyEnter += OnEventFearAltarMatchingRoomPartyEnter;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomBanished += OnEventFearAltarMatchingRoomBanished;
		
		CsDungeonManager.Instance.EventFearAltarEnter += OnEventFearAltarEnter;
		CsDungeonManager.Instance.EventFearAltarWaveStart += OnEventFearAltarWaveStart;
        
        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryMatchingStart += OnEventWarMemoryMatchingStart;
        CsDungeonManager.Instance.EventWarMemoryMatchingCancel += OnEventWarMemoryMatchingCancel;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomPartyEnter += OnEventWarMemoryMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomBanished += OnEventWarMemoryMatchingRoomBanished;

        CsDungeonManager.Instance.EventWarMemoryEnter += OnEventWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryWaveStart += OnEventWarMemoryWaveStart;

        // 오시리스 방
        CsDungeonManager.Instance.EventOsirisRoomEnter += OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomExit += OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomAbandon += OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished += OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomWaveStart += OnEventOsirisRoomWaveStart;
        CsDungeonManager.Instance.EventOsirisRoomMoneyBuffActivate += OnEventOsirisRoomMoneyBuffActivate;
        CsDungeonManager.Instance.EventOsirisRoomMoneyBuffCancel += OnEventOsirisRoomMoneyBuffCancel;
        CsDungeonManager.Instance.EventOsirisRoomMoneyBuffFinished += OnEventOsirisRoomMoneyBuffFinished;
        CsDungeonManager.Instance.EventDungeonClear += OnEventDungeonClear;
        CsDungeonManager.Instance.EventOsirisRoomClear += OnEventOsirisRoomClear;
        CsDungeonManager.Instance.EventOsirisRoomFail += OnEventOsirisRoomFail;

		// 전기퀘스트 던전
		CsDungeonManager.Instance.EventBiographyQuestDungeonEnter += OnEventBiographyQuestDungeonEnter;
		CsDungeonManager.Instance.EventBiographyQuestDungeonWaveStart += OnEventBiographyQuestDungeonWaveStart;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestEnter += OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestStepStart += OnEventDragonNestStepStart;

        CsDungeonManager.Instance.EventDragonNestMatchingCancel += OnEventDragonNestMatchingCancel;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomBanished += OnEventDragonNestMatchingRoomBanished;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomPartyEnter += OnEventDragonNestMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventDragonNestMatchingStart += OnEventDragonNestMatchingStart;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipMatchingStart += OnEventTradeShipMatchingStart;
        CsDungeonManager.Instance.EventTradeShipMatchingCancel += OnEventTradeShipMatchingCancel;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomPartyEnter += OnEventTradeShipMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomBanished += OnEventTradeShipMatchingRoomBanished;

        CsDungeonManager.Instance.EventTradeShipEnter += OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipExit += OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipAbandon += OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished += OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipMoneyBuffActivate += OnEventTradeShipMoneyBuffActivate;
        CsDungeonManager.Instance.EventTradeShipMoneyBuffCancel += OnEventTradeShipMoneyBuffCancel;
        CsDungeonManager.Instance.EventTradeShipMoneyBuffFinished += OnEventTradeShipMoneyBuffFinished;
        CsDungeonManager.Instance.EventTradeShipClear += OnEventTradeShipClear;
        CsDungeonManager.Instance.EventTradeShipFail += OnEventTradeShipFail;
        CsDungeonManager.Instance.EventTradeShipStepStart += OnEventTradeShipStepStart;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombMatchingStart += OnEventAnkouTombMatchingStart;
        CsDungeonManager.Instance.EventAnkouTombMatchingCancel += OnEventAnkouTombMatchingCancel;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomPartyEnter += OnEventAnkouTombMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomBanished += OnEventAnkouTombMatchingRoomBanished;

        CsDungeonManager.Instance.EventAnkouTombEnter += OnEventAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombExit += OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombAbandon += OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished += OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombMoneyBuffActivate += OnEventAnkouTombMoneyBuffActivate;
        CsDungeonManager.Instance.EventAnkouTombMoneyBuffCancel += OnEventAnkouTombMoneyBuffCancel;
        CsDungeonManager.Instance.EventAnkouTombMoneyBuffFinished += OnEventAnkouTombMoneyBuffFinished;
        CsDungeonManager.Instance.EventAnkouTombClear += OnEventAnkouTombClear;
        CsDungeonManager.Instance.EventAnkouTombFail += OnEventAnkouTombFail;
        CsDungeonManager.Instance.EventAnkouTombWaveStart += OnEventAnkouTombWaveStart;

		// 팀 전장
		CsDungeonManager.Instance.EventTeamBattlefieldEnter += OnEventTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldExit += OnEventTeamBattlefieldExit;
		CsDungeonManager.Instance.EventTeamBattlefieldAbandon += OnEventTeamBattlefieldAbandon;
		CsDungeonManager.Instance.EventTeamBattlefieldBanished += OnEventTeamBattlefieldBanished;
		CsDungeonManager.Instance.EventTeamBattlefieldPlayWaitStart += OnEventTeamBattlefieldPlayWaitStart;
		
        //검투 대회
        CsDungeonManager.Instance.EventFieldOfHonorChallenge += OnEventFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorStart += OnEventFieldOfHonorStart;

        // 정예 던전
        CsDungeonManager.Instance.EventEliteDungeonEnter += OnEventEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonStart += OnEventEliteDungeonStart;

        //도달 보상
        CsGameEventUIToUI.Instance.EventAttainmentRewardReceive += OnEventAttainmentRewardReceive;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;

        //길드 영지
        CsGuildManager.Instance.EventGuildTerritoryEnter += OnEventGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryExit += OnEventGuildTerritoryExit;

        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;

        // 메뉴버튼업데이트
        CsGameEventUIToUI.Instance.EventMainButtonUpdate += OnEventMainButtonUpdate;
        CsGameEventUIToUI.Instance.EventMenuOpenButtonNoticeUpdate += OnEventMenuOpenButtonNoticeUpdate;
		CsGameEventUIToUI.Instance.EventLeftTopMenuOpenButtonNoticeUpdate += OnEventLeftTopMenuOpenButtonNoticeUpdate;

        // 국가전
        CsNationWarManager.Instance.EventNationWarStart += OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarFinished += OnEventNationWarFinished;

        //재접속
        CsGameEventUIToUI.Instance.EventHeroLogin += OnEventHeroLogin;

		// 선물
		CsGameEventUIToUI.Instance.EventOpenGiftReceive += OnEventOpenGiftReceive;
		CsGameEventUIToUI.Instance.EventRookieGiftReceive += OnEventRookieGiftReceive;

		// 축복, 축복퀘스트
		CsBlessingQuestManager.Instance.EventBlessingReceived += OnEventBlessingReceived;
		CsBlessingQuestManager.Instance.EventBlessingQuestStarted += OnEventBlessingQuestStarted;

		CsBlessingQuestManager.Instance.EventBlessingQuestBlessingSend += OnEventBlessingQuestBlessingSend;
		CsBlessingQuestManager.Instance.EventBlessingQuestDeleteAll += OnEventBlessingQuestDeleteAll;
		CsBlessingQuestManager.Instance.EventBlessingRewardReceive += OnEventBlessingRewardReceive;
		CsBlessingQuestManager.Instance.EventBlessingDeleteAll += OnEventBlessingDeleteAll;

        // 증정
        CsPresentManager.Instance.EventPresentReceived += OnEventPresentReceived;
        CsGameEventUIToUI.Instance.EventCheckPresentButtonDisplay += OnEventCheckPresentButtonDisplay;
    }

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationId == CsGameData.Instance.UndergroundMaze.LocationId)
        {
            //미로입장
            DungeonEnter();
            CsUIData.Instance.DungeonInNow = EnDungeon.UndergroundMaze;

            m_flLimitTime = CsDungeonManager.Instance.UndergroundMaze.LimitTime - CsDungeonManager.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime + Time.realtimeSinceStartup;
        }
        else if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationId == 201)
        {
            GuildTerritoryEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if ((m_flLimitTime - Time.realtimeSinceStartup) > 0)
            {
                m_timeSpan = TimeSpan.FromSeconds(m_flLimitTime - Time.realtimeSinceStartup);

                UpdateTimer();
            }

            if ((m_flRemainingNationWarTime - Time.realtimeSinceStartup) > 0)
            {
                m_timeSpan = TimeSpan.FromSeconds(m_flRemainingNationWarTime - Time.realtimeSinceStartup);

                UpdateButtonNationWar();
            }

			// 오픈 선물 시간 업데이트
			if (m_trButtonOpenGift != null && m_trButtonRookieGift.gameObject.activeSelf)
			{
				int nTotalDays = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.Subtract(CsGameData.Instance.MyHeroInfo.RegDate).TotalDays + 1;
				bool bShowTime = nTotalDays < m_nOpenGiftLastDay;

				m_textOpenGiftRemainingTime.gameObject.SetActive(bShowTime);

				if (bShowTime)
				{
					TimeSpan timeSpanRemainingTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.AddDays(1.0).Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime);

					if (timeSpanRemainingTime.TotalSeconds <= 0)
					{
						timeSpanRemainingTime = TimeSpan.FromSeconds(0);
					}

					m_textOpenGiftRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("A99_TXT_01001"), timeSpanRemainingTime.Hours.ToString("00"), timeSpanRemainingTime.Minutes.ToString("00"), timeSpanRemainingTime.Seconds.ToString("00"));
				}
			}

			// 신규 선물 시간 업데이트
			if (m_trButtonRookieGift != null && m_trButtonRookieGift.gameObject.activeSelf)
			{
				int nRemainingTime = (int)CsGameData.Instance.MyHeroInfo.RookieGiftRemainingTime;
				bool bShowTime = nRemainingTime > 0;

				TimeSpan timeSpan = TimeSpan.FromSeconds(nRemainingTime);

				m_textRookieGiftRemainingTime.gameObject.SetActive(bShowTime);

				if (bShowTime)
				{
					m_textRookieGiftRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("A100_TXT_01001"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
				}
			}

            if (m_bActivateMoneyBuff)
            {
                UpdateMoneyBuffLifeTime();
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventToUI.Instance.EventPrevContinentEnter -= OnEventPrevContinentEnter;
        //CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;
        //CsGameEventUIToUI.Instance.EventLoadingSliderComplete -= OnEventLoadingSliderComplete;

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter -= OnEventMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepStart -= OnEventMainQuestDungeonStepStart;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonClear -= OnEventMainQuestDungeonClear;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonFail -= OnEventMainQuestDungeonFail;

        //스토리던전
        CsDungeonManager.Instance.EventStoryDungeonEnter -= OnEventStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonStepStart -= OnEventStoryDungeonStepStart;

        //경험치던전
        CsDungeonManager.Instance.EventExpDungeonEnter -= OnEventExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonWaveStart -= OnEventExpDungeonWaveStart;

        //골드던전
        CsDungeonManager.Instance.EventGoldDungeonEnter -= OnEventGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonStepStart -= OnEventGoldDungeonStepStart;

        //지하미로
        CsDungeonManager.Instance.EventUndergroundMazeEnter -= OnEventUndergroundMazeEnter;
        CsDungeonManager.Instance.EventUndergroundMazePortalExit -= OnEventUndergroundMazePortalExit;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForTransmission -= OnEventUndergroundMazeEnterForTransmission;

        //고대 유물의 방
        CsDungeonManager.Instance.EventContinentExitForArtifactRoomEnter -= OnEventContinentExitForArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomStart -= OnEventArtifactRoomStart;

        //고대인의 유적
        CsDungeonManager.Instance.EventAncientRelicMatchingCancel -= OnEventAncientRelicMatchingCancel;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomBanished -= OnEventAncientRelicMatchingRoomBanished;
        CsDungeonManager.Instance.EventAncientRelicMatchingStart -= OnEventAncientRelicMatchingStart;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomPartyEnter -= OnEventAncientRelicMatchingRoomPartyEnter;

        CsDungeonManager.Instance.EventAncientRelicEnter -= OnEventAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicStepStart -= OnEventAncientRelicStepStart;

        //영혼을 탐하는 자
        CsDungeonManager.Instance.EventSoulCoveterMatchingCancel -= OnEventSoulCoveterMatchingCancel;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomBanished -= OnEventSoulCoveterMatchingRoomBanished;
        CsDungeonManager.Instance.EventSoulCoveterMatchingStart -= OnEventSoulCoveterMatchingStart;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomPartyEnter -= OnEventSoulCoveterMatchingRoomPartyEnter;

        CsDungeonManager.Instance.EventSoulCoveterEnter -= OnEventSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterWaveStart -= OnEventSoulCoveterWaveStart;

        //용맹의 증명
        CsDungeonManager.Instance.EventProofOfValorEnter -= OnEventProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorStart -= OnEventProofOfValorStart;

        // 지혜의 신전
        CsDungeonManager.Instance.EventWisdomTempleEnter -= OnEventWisdomTempleEnter;
        CsDungeonManager.Instance.EventWisdomTempleStepStart -= OnEventWisdomTempleStepStart;

        // 유적 탈환
        CsDungeonManager.Instance.EventRuinsReclaimMatchingCancel -= OnEventRuinsReclaimMatchingCancel;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomBanished -= OnEventRuinsReclaimMatchingRoomBanished;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingStart -= OnEventRuinsReclaimMatchingStart;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomPartyEnter -= OnEventRuinsReclaimMatchingRoomPartyEnter;

        CsDungeonManager.Instance.EventRuinsReclaimEnter -= OnEventRuinsReclaimEnter;
        CsDungeonManager.Instance.EventRuinsReclaimStepStart -= OnEventRuinsReclaimStepStart;

        // 무한 대전
        CsDungeonManager.Instance.EventInfiniteWarMatchingCancel -= OnEventInfiniteWarMatchingCancel;
        CsDungeonManager.Instance.EventInfiniteWarMatchingRoomBanished -= OnEventInfiniteWarMatchingRoomBanished;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStart -= OnEventInfiniteWarMatchingStart;

        CsDungeonManager.Instance.EventInfiniteWarEnter -= OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarStart -= OnEventInfiniteWarStart;

        // 공포의 제단
        CsDungeonManager.Instance.EventFearAltarMatchingStart -= OnEventFearAltarMatchingStart;
        CsDungeonManager.Instance.EventFearAltarMatchingCancel -= OnEventFearAltarMatchingCancel;
        CsDungeonManager.Instance.EventFearAltarMatchingRoomPartyEnter -= OnEventFearAltarMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventFearAltarMatchingRoomBanished -= OnEventFearAltarMatchingRoomBanished;

        CsDungeonManager.Instance.EventFearAltarEnter -= OnEventFearAltarEnter;
        CsDungeonManager.Instance.EventFearAltarWaveStart -= OnEventFearAltarWaveStart;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryMatchingStart -= OnEventWarMemoryMatchingStart;
        CsDungeonManager.Instance.EventWarMemoryMatchingCancel -= OnEventWarMemoryMatchingCancel;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomPartyEnter -= OnEventWarMemoryMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomBanished -= OnEventWarMemoryMatchingRoomBanished;

        CsDungeonManager.Instance.EventWarMemoryEnter -= OnEventWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryWaveStart -= OnEventWarMemoryWaveStart;

        // 오시리스 방
        CsDungeonManager.Instance.EventOsirisRoomEnter -= OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomExit -= OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomAbandon -= OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished -= OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomWaveStart -= OnEventOsirisRoomWaveStart;
        CsDungeonManager.Instance.EventOsirisRoomMoneyBuffActivate -= OnEventOsirisRoomMoneyBuffActivate;
        CsDungeonManager.Instance.EventOsirisRoomMoneyBuffCancel -= OnEventOsirisRoomMoneyBuffCancel;
        CsDungeonManager.Instance.EventOsirisRoomMoneyBuffFinished -= OnEventOsirisRoomMoneyBuffFinished;
        CsDungeonManager.Instance.EventDungeonClear -= OnEventDungeonClear;
        CsDungeonManager.Instance.EventOsirisRoomClear -= OnEventOsirisRoomClear;
        CsDungeonManager.Instance.EventOsirisRoomFail -= OnEventOsirisRoomFail;

        // 전기퀘스트 던전
        CsDungeonManager.Instance.EventBiographyQuestDungeonEnter -= OnEventBiographyQuestDungeonEnter;
        CsDungeonManager.Instance.EventBiographyQuestDungeonWaveStart -= OnEventBiographyQuestDungeonWaveStart;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestEnter -= OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestStepStart -= OnEventDragonNestStepStart;

        CsDungeonManager.Instance.EventDragonNestMatchingCancel -= OnEventDragonNestMatchingCancel;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomBanished -= OnEventDragonNestMatchingRoomBanished;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomPartyEnter -= OnEventDragonNestMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventDragonNestMatchingStart -= OnEventDragonNestMatchingStart;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipMatchingStart -= OnEventTradeShipMatchingStart;
        CsDungeonManager.Instance.EventTradeShipMatchingCancel -= OnEventTradeShipMatchingCancel;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomPartyEnter -= OnEventTradeShipMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomBanished -= OnEventTradeShipMatchingRoomBanished;

        CsDungeonManager.Instance.EventTradeShipEnter -= OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipExit -= OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipAbandon -= OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished -= OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipMoneyBuffActivate -= OnEventTradeShipMoneyBuffActivate;
        CsDungeonManager.Instance.EventTradeShipMoneyBuffCancel -= OnEventTradeShipMoneyBuffCancel;
        CsDungeonManager.Instance.EventTradeShipMoneyBuffFinished -= OnEventTradeShipMoneyBuffFinished;
        CsDungeonManager.Instance.EventTradeShipClear -= OnEventTradeShipClear;
        CsDungeonManager.Instance.EventTradeShipFail -= OnEventTradeShipFail;
        CsDungeonManager.Instance.EventTradeShipStepStart -= OnEventTradeShipStepStart;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombMatchingStart -= OnEventAnkouTombMatchingStart;
        CsDungeonManager.Instance.EventAnkouTombMatchingCancel -= OnEventAnkouTombMatchingCancel;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomPartyEnter -= OnEventAnkouTombMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomBanished -= OnEventAnkouTombMatchingRoomBanished;

        CsDungeonManager.Instance.EventAnkouTombEnter -= OnEventAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombExit -= OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombAbandon -= OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished -= OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombMoneyBuffActivate -= OnEventAnkouTombMoneyBuffActivate;
        CsDungeonManager.Instance.EventAnkouTombMoneyBuffCancel -= OnEventAnkouTombMoneyBuffCancel;
        CsDungeonManager.Instance.EventAnkouTombMoneyBuffFinished -= OnEventAnkouTombMoneyBuffFinished;
        CsDungeonManager.Instance.EventAnkouTombClear -= OnEventAnkouTombClear;
        CsDungeonManager.Instance.EventAnkouTombFail -= OnEventAnkouTombFail;
        CsDungeonManager.Instance.EventAnkouTombWaveStart -= OnEventAnkouTombWaveStart;

		// 팀 전장
		CsDungeonManager.Instance.EventTeamBattlefieldEnter -= OnEventTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldExit -= OnEventTeamBattlefieldExit;
		CsDungeonManager.Instance.EventTeamBattlefieldAbandon -= OnEventTeamBattlefieldAbandon;
		CsDungeonManager.Instance.EventTeamBattlefieldBanished -= OnEventTeamBattlefieldBanished;
		CsDungeonManager.Instance.EventTeamBattlefieldPlayWaitStart -= OnEventTeamBattlefieldPlayWaitStart;
		
        //검투 대회
        CsDungeonManager.Instance.EventFieldOfHonorChallenge -= OnEventFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorStart -= OnEventFieldOfHonorStart;

        // 정예 던전
        CsDungeonManager.Instance.EventEliteDungeonEnter -= OnEventEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonStart -= OnEventEliteDungeonStart;

        //도달 보상
        CsGameEventUIToUI.Instance.EventAttainmentRewardReceive -= OnEventAttainmentRewardReceive;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;

        //길드 영지
        CsGuildManager.Instance.EventGuildTerritoryEnter -= OnEventGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryExit -= OnEventGuildTerritoryExit;

        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;

        // 메뉴버튼업데이트
        CsGameEventUIToUI.Instance.EventMainButtonUpdate -= OnEventMainButtonUpdate;
        CsGameEventUIToUI.Instance.EventMenuOpenButtonNoticeUpdate -= OnEventMenuOpenButtonNoticeUpdate;
        CsGameEventUIToUI.Instance.EventLeftTopMenuOpenButtonNoticeUpdate -= OnEventLeftTopMenuOpenButtonNoticeUpdate;

        // 국가전
        CsNationWarManager.Instance.EventNationWarStart -= OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarFinished -= OnEventNationWarFinished;

        //재접속
        CsGameEventUIToUI.Instance.EventHeroLogin -= OnEventHeroLogin;

        // 선물
        CsGameEventUIToUI.Instance.EventOpenGiftReceive -= OnEventOpenGiftReceive;
        CsGameEventUIToUI.Instance.EventRookieGiftReceive -= OnEventRookieGiftReceive;

        // 축복, 축복퀘스트
        CsBlessingQuestManager.Instance.EventBlessingReceived -= OnEventBlessingReceived;
        CsBlessingQuestManager.Instance.EventBlessingQuestStarted -= OnEventBlessingQuestStarted;

        CsBlessingQuestManager.Instance.EventBlessingQuestBlessingSend -= OnEventBlessingQuestBlessingSend;
        CsBlessingQuestManager.Instance.EventBlessingQuestDeleteAll -= OnEventBlessingQuestDeleteAll;
        CsBlessingQuestManager.Instance.EventBlessingRewardReceive -= OnEventBlessingRewardReceive;
        CsBlessingQuestManager.Instance.EventBlessingDeleteAll -= OnEventBlessingDeleteAll;

        // 증정
        CsPresentManager.Instance.EventPresentReceived -= OnEventPresentReceived;
        CsGameEventUIToUI.Instance.EventCheckPresentButtonDisplay -= OnEventCheckPresentButtonDisplay;
    }

    #region EventHandler
    //---------------------------------------------------------------------------------------------------
    void OnEventMenuOpenButtonNoticeUpdate(bool bIsOn)
    {
        transform.Find("MenuList/ButtonMenuTop0/ImageNotice").gameObject.SetActive(bIsOn);
        transform.Find("ButtonList/ButtonMenu/ImageNotice").gameObject.SetActive(bIsOn);
        transform.Find("GuildButtonList/ButtonMenu/ImageNotice").gameObject.SetActive(bIsOn);

        UpdateHelperButton(bIsOn);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventLeftTopMenuOpenButtonNoticeUpdate(bool bIsOn)
    {
        transform.Find("LeftMenuList/ToggleLeftMenu/ImageNotice").gameObject.SetActive(bIsOn);
    }

    //---------------------------------------------------------------------------------------------------
    ////지역 토스트
    //void OnEventLoadingSliderComplete()
    //{
    //    string strName;

    //    switch (CsUIData.Instance.DungeonInNow)
    //    {
    //        case EnDungeon.None:

    //            CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);

    //            if (csContinent != null)
    //            {
    //                CsNation csNation = CsGameData.Instance.GetNation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam);

    //                if (csNation != null)
    //                {
    //                    if (CsGameData.Instance.MyHeroInfo.Nation == csNation)
    //                    {
    //                        CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaContinent, csContinent.Name, csNation.Name);
    //                    }
    //                    else
    //                    {
    //                        CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaContinent, csContinent.Name, csNation.Name, false);
    //                    }
    //                }
    //                else
    //                {
    //                    if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == 0)
    //                    {
    //                        CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaContinent, csContinent.Name, "");
    //                    }
    //                }
    //            }

    //            break;
    //        case EnDungeon.MainQuestDungeon:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsMainQuestDungeonManager.Instance.MainQuestDungeon.Name, CsMainQuestDungeonManager.Instance.MainQuestDungeon.Description);
    //            break;
    //        case EnDungeon.StoryDungeon:
    //            CsStoryDungeonDifficulty csStoryDungeonDifficulty = CsDungeonManager.Instance.StoryDungeon.GetStoryDungeonDifficulty(CsDungeonManager.Instance.StoryDungeonStep.Difficulty);
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsDungeonManager.Instance.StoryDungeon.Name, csStoryDungeonDifficulty.Name);
    //            break;
    //        case EnDungeon.ExpDungeon:
    //            CsExpDungeonDifficulty csExpDungeonDifficulty = CsDungeonManager.Instance.ExpDungeon.GetExpDungeonDifficulty(CsDungeonManager.Instance.ExpDungeonDifficultyWave.Difficulty);
    //            strName = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_DUNLV"), CsDungeonManager.Instance.ExpDungeon.Name, csExpDungeonDifficulty.RequiredHeroLevel);
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A32_TXT_00007"), strName);
    //            break;
    //        case EnDungeon.GoldDungeon:
    //            strName = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_DUNLV"), CsDungeonManager.Instance.GoldDungeon.Name, CsDungeonManager.Instance.GoldDungeonDifficulty.RequiredHeroLevel);
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A39_TXT_00004"), strName);
    //            break;
    //        case EnDungeon.UndergroundMaze:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A43_TXT_00003"), CsDungeonManager.Instance.UndergroundMazeFloor.Name);
    //            break;
    //        case EnDungeon.ArtifactRoom:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A47_TXT_03001"), CsDungeonManager.Instance.ArtifactRoom.Name);
    //            break;
    //        case EnDungeon.AncientRelic:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A40_TXT_00009"), CsDungeonManager.Instance.AncientRelic.Name);
    //            break;
    //        case EnDungeon.FieldOfHonor:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A31_TXT_00013"), CsDungeonManager.Instance.FieldOfHonor.Name);
    //            break;
    //        case EnDungeon.SoulCoveter:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A74_TXT_00002"), CsDungeonManager.Instance.SoulCoveter.Name);
    //            break;
    //        case EnDungeon.EliteDungeon:
    //            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A87_TXT_00001"), CsDungeonManager.Instance.EliteDungeon.Name);
    //            break;
    //    }
    //}

    // 정예 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonEnter(Guid guid, PDVector3 pdVector3, float flRotationY, PDEliteDungeonMonsterInstance[] pDEliteDungeonMonsterInstance)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.EliteDungeon;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonStart()
    {
        m_flLimitTime = CsDungeonManager.Instance.EliteDungeon.LimitTime + Time.realtimeSinceStartup;
    }

    //검투 대회
    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorChallenge(Guid guid, PDVector3 pdVector3, float flRotationY, PDHero pdHeroTarget)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.FieldOfHonor;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorStart()
    {
        m_flLimitTime = CsDungeonManager.Instance.FieldOfHonor.LimitTime + Time.realtimeSinceStartup;
    }

    // 영혼을 탐하는 자
    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.SoulCoveter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.SoulCoveter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.SoulCoveter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingRoomPartyEnter()
    {
        UpdateMatchingButton(EnDungeon.SoulCoveter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterEnter(Guid guid, PDVector3 pDVector3, float flRotate, PDHero[] pDHero, PDMonsterInstance[] aPDMonsterInstance)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.SoulCoveter;
        UpdateMatchingButton(EnDungeon.SoulCoveter);

		// 시작 후 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
		{
			m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterWaveStart(PDSoulCoveterMonsterInstance[] pDSoulCoveterMonsterInstance)
    {
        if (CsDungeonManager.Instance.SoulCoveterDifficultyWave.WaveNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.AncientRelic.LimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorEnter(Guid guid, PDVector3 pDVector3, float flRotate, PDMonsterInstance[] pDMonsterInstance)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.ProofOfValor;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorStart()
    {
        m_flLimitTime = CsDungeonManager.Instance.ProofOfValor.LimitTime + Time.realtimeSinceStartup;
    }

	// 지혜의 신전
	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleEnter(Guid guid, PDVector3 pDVector3, float flRotate)
	{
		DungeonEnter();
		CsUIData.Instance.DungeonInNow = EnDungeon.WisdomTemple;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepStart(PDWisdomTempleMonsterInstance[] aPDWisdomTempleMonsterInstance, PDWisdomTempleColorMatchingObjectInstance[] aPDWisdomTempleColorMatchingObjectInstance, int nQuizNo)
	{
		if (CsDungeonManager.Instance.WisdomTempleStep.StepNo == 1)
		{
			m_flLimitTime = CsDungeonManager.Instance.WisdomTemple.LimitTime + Time.realtimeSinceStartup;
		}
	}

	// 유적 탈환
	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingCancel()
	{
		UpdateMatchingButton(EnDungeon.RuinsReclaim);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingRoomBanished()
	{
		UpdateMatchingButton(EnDungeon.RuinsReclaim);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingStart()
	{
		UpdateMatchingButton(EnDungeon.RuinsReclaim);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingRoomPartyEnter()
	{
		UpdateMatchingButton(EnDungeon.RuinsReclaim);
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventRuinsReclaimEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotate, ClientCommon.PDHero[] aPDHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance, ClientCommon.PDRuinsReclaimRewardObjectInstance[] aPDRuinReclaimRewardObjectInstance, ClientCommon.PDRuinsReclaimMonsterTransformationCancelObjectInstance[] aPDRuinsReclaimMonsterTransformationCancelObjectInstance, Guid[] aGuidMonsterTransformationHero)
	{
		DungeonEnter();
		CsUIData.Instance.DungeonInNow = EnDungeon.RuinsReclaim;
		UpdateMatchingButton(EnDungeon.RuinsReclaim);

		// 시작 후 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
		{
			m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepStart(PDRuinsReclaimRewardObjectInstance[] aPDRuinsReclaimRewardObjectInstance)
	{
		if (CsDungeonManager.Instance.RuinsReclaimStep.StepNo == 1)
		{
			m_flLimitTime = CsDungeonManager.Instance.RuinsReclaim.LimitTime + Time.realtimeSinceStartup;
		}
	}

    #region InfiniteWar

    // 무한 대전
    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.InfiniteWar);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.InfiniteWar);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingRoomPartyEnter()
    {
        UpdateMatchingButton(EnDungeon.InfiniteWar);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.InfiniteWar);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pDHeroes, ClientCommon.PDMonsterInstance[] pDMonsterInstance, ClientCommon.PDInfiniteWarBuffBoxInstance[] pDInfiniteWarBuffBoxInstance)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.InfiniteWar;
        UpdateMatchingButton(EnDungeon.InfiniteWar);

        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0f)
        {
            if (CsDungeonManager.Instance.MultiDungeonRemainingLimitTime < CsGameData.Instance.InfiniteWar.LimitTime)
            {
                m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
            }
            else
            {
                m_flLimitTime = CsGameData.Instance.InfiniteWar.LimitTime + Time.realtimeSinceStartup;
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarStart()
    {
        m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
    }

    #endregion InfiniteWar

	#region FearAltar

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingCancel()
	{
		UpdateMatchingButton(EnDungeon.FearAltar);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingRoomBanished()
	{
		UpdateMatchingButton(EnDungeon.FearAltar);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingStart()
	{
		UpdateMatchingButton(EnDungeon.FearAltar);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingRoomPartyEnter()
	{
		UpdateMatchingButton(EnDungeon.FearAltar);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotate, ClientCommon.PDHero[] aPDHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance)
	{
		DungeonEnter();
		CsUIData.Instance.DungeonInNow = EnDungeon.FearAltar;
		UpdateMatchingButton(EnDungeon.FearAltar);

		// 시작 후 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
		{
			m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarWaveStart(PDFearAltarMonsterInstance[] aPDFearAltarMonsterInstance, PDFearAltarHalidomMonsterInstance pdFearAltarHalidomMonsterInstance)
	{
		if (CsDungeonManager.Instance.FearAltarStageWave.WaveNo == 1)
		{
			m_flLimitTime = CsDungeonManager.Instance.FearAltar.LimitTime + Time.realtimeSinceStartup;
		}
	}

	#endregion FearAltar

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.WarMemory);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.WarMemory);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingRoomPartyEnter()
    {
        UpdateMatchingButton(EnDungeon.WarMemory);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.WarMemory);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryEnter(Guid placeInstanceId, PDVector3 position, float rotationY, PDHero[] aPDHero, PDMonsterInstance[] aPDMonsterInstance, PDWarMemoryTransformationObjectInstance[] aPDWarMemoryTransformationObjectInstance)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.WarMemory;
        UpdateMatchingButton(EnDungeon.WarMemory);

        // 시작 후 입장했을 경우
        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryWaveStart(PDWarMemoryMonsterInstance[] aPDWarMemoryMonsterInstance, PDWarMemoryTransformationObjectInstance[] aPDWarMemoryTransformationObjectInstance)
    {
        if (CsDungeonManager.Instance.WarMemoryWave.WaveNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.WarMemory.LimitTime + Time.realtimeSinceStartup;
        }
    }

    #endregion WarMemory

    #region OsirisRoom

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY)
    {
        CsUIData.Instance.DungeonInNow = EnDungeon.OsirisRoom;
        DisplayPreviewButton();
        DungeonEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomExit(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomAbandon(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomBanished(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomWaveStart()
    {
        if (CsDungeonManager.Instance.OsirisRoomDifficultyWave != null && CsDungeonManager.Instance.OsirisRoomDifficultyWave.WaveNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.OsirisRoom.LimitTime + Time.realtimeSinceStartup;
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomMoneyBuffActivate()
    {
        m_bActivateMoneyBuff = true;

        m_flMoneyBuffLifeTime = m_csMoneyBuff.Lifetime + Time.realtimeSinceStartup;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomMoneyBuffCancel()
    {
        m_bActivateMoneyBuff = false;

        m_flMoneyBuffLifeTime = 0f;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomMoneyBuffFinished()
    {
        m_bActivateMoneyBuff = false;

        m_flMoneyBuffLifeTime = 0f;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDungeonClear()
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomClear()
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomFail()
    {
        PopupMoneyBuffClose();
    }

    #endregion OsirisRoom

    //고대인의 유적
    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicEnter(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance, Guid[] arrGuidTrapEffectHeroe, int[] arrRemovedObstacleId)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.AncientRelic;
        UpdateMatchingButton(EnDungeon.AncientRelic);

		// 시작 후 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
		{
			m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicStepStart(int nRemoveObstacleId, Vector3 v3Position, float flTargetRadius)
    {
        if (CsDungeonManager.Instance.AncientRelicStep.Step == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.AncientRelic.LimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.AncientRelic);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.AncientRelic);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.AncientRelic);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingRoomPartyEnter()
    {
        UpdateMatchingButton(EnDungeon.AncientRelic);
    }

    //고대 유물의 방
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForArtifactRoomEnter()
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.ArtifactRoom;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomStart(PDArtifactRoomMonsterInstance[] pDArtifactRoomMonsterInstance)
    {
        m_flLimitTime = CsDungeonManager.Instance.ArtifactRoom.LimitTime + Time.realtimeSinceStartup;
    }

    //지하미로
    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeEnter(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.UndergroundMaze;

        m_flLimitTime = CsDungeonManager.Instance.UndergroundMaze.LimitTime - CsDungeonManager.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazePortalExit(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        //CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A43_TXT_00003"), CsDungeonManager.Instance.UndergroundMazeFloor.Name);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeEnterForTransmission(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        //CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A43_TXT_00003"), CsDungeonManager.Instance.UndergroundMazeFloor.Name);
    }

    //골드던전
    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonEnter(PDVector3 pDVector3, float flRotationY, System.Guid guidPlaceInstanceId)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.GoldDungeon;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonStepStart(PDGoldDungeonMonsterInstance[] pPDGoldDungeonMonsterInstance)
    {
        if (CsDungeonManager.Instance.GoldDungeonStep.Step == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.GoldDungeon.LimitTime + Time.realtimeSinceStartup;
        }
    }

    //경험치 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonEnter(PDVector3 pDVector3, float flRotationY, System.Guid guidPlaceInstanceId)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.ExpDungeon;
    }

    //스토리 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonEnter(PDVector3 pDVector3, float flRotationY, System.Guid guidPlaceInstanceId)
    {
        DungeonEnter();
        m_buttonExit.interactable = true;
        CsUIData.Instance.DungeonInNow = EnDungeon.StoryDungeon;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonStepStart(PDStoryDungeonMonsterInstance[] pDStoryDungeonMonsterInstance)
    {
        if (CsDungeonManager.Instance.StoryDungeonStep.Step == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.StoryDungeon.LimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonWaveStart(PDExpDungeonMonsterInstance[] pDExpDungeonMonsterInstance, PDExpDungeonLakChargeMonsterInstance pDExpDungeonLakChargeMonsterInstance)
    {
        if (CsDungeonManager.Instance.ExpDungeonDifficultyWave.WaveNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.ExpDungeon.LimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPrevContinentEnter()
    {
        DungeonExit();
        CsDungeonManager.Instance.DungeonPlay = EnDungeonPlay.None;
        CsUIData.Instance.DungeonInNow = EnDungeon.None;
    }

    //메인퀘 던전

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonExit(int nContinentId, bool bChangeScene)
    {
        DungeonExit();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonClear()
    {
        //나가기 버튼 막기
        m_buttonExit.interactable = false;
        //나가기 팝업 끄기
        CsGameEventUIToUI.Instance.OnEventCommonModalClose();

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A13_TXT_02003"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonFail()
    {
        //나가기 버튼 막기
        m_buttonExit.interactable = false;
        //나가기 팝업 끄기
        CsGameEventUIToUI.Instance.OnEventCommonModalClose();

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A13_TXT_02004"));
    }


    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonStepStart(PDMainQuestDungeonMonsterInstance[] apDMainQuestDungeonMonsterInstance)
    {
        if (CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.Step == 1)
        {
            m_flLimitTime = CsMainQuestDungeonManager.Instance.MainQuestDungeon.LimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonEnter(PDVector3 pDVector3, float flRotationY, System.Guid guidPlaceInstanceId)
    {
        DungeonEnter();
        m_buttonExit.interactable = true;
        CsUIData.Instance.DungeonInNow = EnDungeon.MainQuestDungeon;
    }

	// 전기퀘스트 던전
	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonEnter(Guid guidPlaceInstanceId, PDVector3 vtPosition, float flRotationY)
	{
		CsUIData.Instance.DungeonInNow = EnDungeon.BiographyDungeon;
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonWaveStart(PDBiographyQuestDungeonMonsterInstance[] aPDBiographyQuestDungeonMonsterInstance)
	{
		if (CsDungeonManager.Instance.BiographyQuestDungeonWave != null && CsDungeonManager.Instance.BiographyQuestDungeonWave.WaveNo == 1)
		{
			m_flLimitTime = CsDungeonManager.Instance.BiographyQuestDungeon.LimitTime + Time.realtimeSinceStartup;
		}
	}

    #region DragonNest

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.DragonNest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.DragonNest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingRoomPartyEnter()
    {
        UpdateMatchingButton(EnDungeon.DragonNest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.DragonNest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHero, PDMonsterInstance[] aMonsterInstance, Guid[] aTrapHeros)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.DragonNest;
        UpdateMatchingButton(EnDungeon.DragonNest);

        // 시작 후 입장했을 경우
        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestStepStart(PDDragonNestMonsterInstance[] aMonsterInstance)
    {
        if (CsDungeonManager.Instance.DragonNestStep.StepNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    #endregion DragonNest

    #region TradeShip

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.TradeShip);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.TradeShip);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingRoomPartyEnter()
    {
        UpdateMatchingButton(EnDungeon.TradeShip);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.TradeShip);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.TradeShip;
        UpdateMatchingButton(EnDungeon.TradeShip);

        // 시작 후 입장했을 경우
        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipExit(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipAbandon(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipBanished(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMoneyBuffActivate()
    {
        m_bActivateMoneyBuff = true;

        m_flMoneyBuffLifeTime = m_csMoneyBuff.Lifetime + Time.realtimeSinceStartup;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMoneyBuffCancel()
    {
        m_bActivateMoneyBuff = false;

        m_flMoneyBuffLifeTime = 0f;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMoneyBuffFinished()
    {
        m_bActivateMoneyBuff = false;

        m_flMoneyBuffLifeTime = 0f;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipClear(bool bLevelUp, long lAcquiredExp, PDItemBooty aPDItemBooty)
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipFail(bool bLevelUp, long lAcquiredExp)
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipStepStart(PDTradeShipMonsterInstance[] apMonInst, PDTradeShipAdditionalMonsterInstance[] apAddMonInst, PDTradeShipObjectInstance[] apObjInst)
    {
        if (CsDungeonManager.Instance.TradeShipStep != null && CsDungeonManager.Instance.TradeShipStep.StepNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    #endregion TradeShip

    #region AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingStart()
    {
        UpdateMatchingButton(EnDungeon.AnkouTomb);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingCancel()
    {
        UpdateMatchingButton(EnDungeon.AnkouTomb);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingRoomPartyEnter(int nDifficulty)
    {
        UpdateMatchingButton(EnDungeon.AnkouTomb);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingRoomBanished()
    {
        UpdateMatchingButton(EnDungeon.AnkouTomb);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        DungeonEnter();
        CsUIData.Instance.DungeonInNow = EnDungeon.AnkouTomb;
        UpdateMatchingButton(EnDungeon.AnkouTomb);

        // 시작 후 입장했을 경우
        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombExit(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombAbandon(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombBanished(int nPreviousContinentId)
    {
        DisplayPreviewButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMoneyBuffActivate()
    {
        m_bActivateMoneyBuff = true;

        m_flMoneyBuffLifeTime = m_csMoneyBuff.Lifetime + Time.realtimeSinceStartup;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMoneyBuffCancel()
    {
        m_bActivateMoneyBuff = false;

        m_flMoneyBuffLifeTime = 0f;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMoneyBuffFinished()
    {
        m_bActivateMoneyBuff = false;

        m_flMoneyBuffLifeTime = 0f;

        UpdateMoneyBuffLifeTime();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombClear(bool bLevelUp, long lAcquiredExp, PDItemBooty aPDItemBooty)
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombFail(bool bLevelUp, long lAcquiredExp)
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombWaveStart(PDAnkouTombMonsterInstance[] arrAnkouTombMonsterInstance, int nWaveNo)
    {
        if (nWaveNo == 1)
        {
            m_flLimitTime = CsDungeonManager.Instance.MultiDungeonRemainingLimitTime + Time.realtimeSinceStartup;
        }
    }

    #endregion AnkouTomb

	#region 팀 전장
	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldEnter(Guid guidPlaceInstanceId, PDVector3 position, float flRotationY, PDHero[] aPDHeroes)
	{
		DungeonEnter();
		CsUIData.Instance.DungeonInNow = EnDungeon.TeamBattlefield;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldExit(int nPrevContinentId)
	{
		DisplayPreviewButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldAbandon(bool bLevelUp, long lAcquiredExp, int nPrevContinentId)
	{
		DisplayPreviewButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldBanished(int nPrevContinentId)
	{
		DisplayPreviewButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldPlayWaitStart(PDTeamBattlefieldMember[] aPDTeamBattlefieldMember)
	{
		m_flLimitTime = CsGameData.Instance.TeamBattlefield.StartDelayTime + Time.realtimeSinceStartup;
	}

	//---------------------------------------------------------------------------------------------------
	#endregion 팀 전장

    //---------------------------------------------------------------------------------------------------
    //도달 보상
    void OnEventAttainmentRewardReceive()
    {
        UpdateAttaniment();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        UpdateOpenMenu();
		UpdateOpenLeftMenu();
        UpdateAttaniment();
        DisplayNationWarButton();
        DisplayHelperButton();
        DisplayPreviewButton();
        UpdatePopupPreview();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryEnter(Guid guidPlaceInstanceId, PDHero[] pDHeroes, PDMonsterInstance[] pDMonsters, PDVector3 pDPosition, float flRotationY)
    {
        GuildTerritoryEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryExit(int nPreviousContinentId)
    {
        GuildTerritoryExit();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csOldMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        UpdateOpenMenu();
        UpdateAttaniment();
        DisplayPreviewButton(); 
        UpdatePopupPreview();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenMainMenu()
    {
        if (m_goPopupMainMenu == null)
        {
            StartCoroutine(LoadMainMenu());
        }
        else
        {
            OpenPopupMainMenu();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedLeftMenu(bool bIson)
    {
        //임시 위치
        if (bIson)
        {
            m_toggleLeftMenu.transform.Find("ImageOpen").gameObject.SetActive(false);
			m_rtrLeftMenuList.pivot = new Vector2(0, 1);
            m_rtrLeftMenuList.anchoredPosition = new Vector2(0f, -80f);
        }
        else
        {
            m_toggleLeftMenu.transform.Find("ImageOpen").gameObject.SetActive(true);
			m_rtrLeftMenuList.pivot = new Vector2(1, 1);
			m_rtrLeftMenuList.anchoredPosition = m_vt2LeftMenuPosition;
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

            case EnDungeon.ProofOfValor:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonProofOfValor,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                break;

			case EnDungeon.WisdomTemple:
				CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
												 CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonWisdomTemple,
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

            case EnDungeon.OsirisRoom:
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A13_TXT_03003"),
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), AbandonOsirisRoom,
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

			case EnDungeon.TeamBattlefield:

				// %% 퇴장 처리
				switch (CsDungeonManager.Instance.BattlefieldState)
				{
					case EnBattlefieldState.Playing:
						break;
					default:
						break;
				}

				break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildTerritoryExit()
    {
        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00003"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), ActionGuildTerritoryExit,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupMatching()
    {
        CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMenuTop(int nMenuId)
    {
        switch ((EnMenuId)nMenuId)
        {
            case EnMenuId.Mail:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mail, EnSubMenu.Mail);
                break;

            case EnMenuId.Inventory:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.Inventory);
                break;

            case EnMenuId.TodayTask:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.TodayTask, EnSubMenu.Default);
                break;

            case EnMenuId.Dungeon:

                if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                {
                    if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                    }
                }
                break;

            case EnMenuId.Attaniment:
                m_csPanelAttainment.OpenPanelAttainment();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMenuLeft(int nMenuId)
    {
        switch ((EnMenuId)nMenuId)
        {
            case EnMenuId.Vip:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Vip, EnSubMenu.VipInfo);
                break;
			case EnMenuId.OpenGift:
			case EnMenuId.RookieGift:
				CsGameEventUIToUI.Instance.OnEventSinglePopupOpen((EnMenuId)nMenuId);
				break;
            case EnMenuId.Open7Day:
                CsGameEventUIToUI.Instance.OnEventSinglePopupOpen((EnMenuId)nMenuId);
                break;
			case EnMenuId.ChargingEvent:
				CsGameEventUIToUI.Instance.OnEventSinglePopupOpen(EnMenuId.ChargingEvent);
				break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenInventory()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.Inventory);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildAuto(EnAutoStateType enAutoState)
    {
        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(enAutoState);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainButtonUpdate(int nGroupNo, int nMenuId, bool bVisible)
    {
        if (nGroupNo == (int)EnMenuGroup.MainMenu1)
        {
            UpdateMenuNotice(nMenuId, bVisible);
        }
		else if (nGroupNo == (int)EnMenuGroup.MainMenu6)
		{
			UpdateLeftTopMenuNotice(nMenuId, bVisible);
		}
    }

    //---------------------------------------------------------------------------------------------------
    // 국가전 시작
    void OnEventNationWarStart(Guid guidNationWarDeclarationId)
    {
        // MainUI 국가전 버튼 표시
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        // 자신의 국가가 국가전이 있을 경우
        if (csNationWarDeclaration == null)
        {
            return;
        }
        else
        {
            CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);

            // pvp 레벨보다 높고, 대륙에 있을 경우 생성
            if (CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
                csContinent != null)
            {
                TimeSpan tsNationWarRemainingTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.AddSeconds(CsGameData.Instance.NationWar.EndTime) - CsGameData.Instance.MyHeroInfo.CurrentDateTime;
                m_flRemainingNationWarTime = (float)tsNationWarRemainingTime.TotalSeconds + Time.realtimeSinceStartup;
                m_buttonNationWar.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarFinished(Guid guidNationWarDeclarationId, int nWinNationId)
    {
        Transform trTextButtonNationWar = m_buttonNationWar.transform.Find("Text");
        trTextButtonNationWar.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroLogin()
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationId == CsGameData.Instance.UndergroundMaze.LocationId)
        {
            //미로입장
            DungeonEnter();
            CsUIData.Instance.DungeonInNow = EnDungeon.UndergroundMaze;

            m_flLimitTime = CsDungeonManager.Instance.UndergroundMaze.LimitTime - CsDungeonManager.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime + Time.realtimeSinceStartup;
        }
        else if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationId == 201)
        {
            GuildTerritoryEnter();
        }
        else
        {
            DungeonExit();
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenGiftReceive(int nDay)
	{
		UpdateOpenLeftMenu();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRookieGiftReceive()
	{
		UpdateOpenLeftMenu();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingReceived()
	{
		UpdateBlessButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingQuestStarted()
	{
		UpdateBlessButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingQuestBlessingSend(long lQuestId)
	{
		UpdateBlessButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingQuestDeleteAll()
	{
		UpdateBlessButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingRewardReceive(long lInstanceId)
	{
		UpdateBlessButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingDeleteAll()
	{
		UpdateBlessButton();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventPresentReceived()
    {
        UpdateBlessButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCheckPresentButtonDisplay()
    {
        UpdateBlessButton();
    }
	
    //---------------------------------------------------------------------------------------------------
    void OnClickNationWar()
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        if (csNationWarDeclaration == null)
        {
            return;
        }
        else
        {
            if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
            {
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.NationWar, EnSubMenu.NationWarInfo);
            }
            else
            {
                // 랭킹 팝업 오픈
                CsGameEventUIToUI.Instance.OnEventOpenPopupNationWarResult();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHelper()
    {
        OpenPopupHelper();
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickBless()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		OpenPopupBless();
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickPreview()
    {
        OpenPopupPreview();
    }

    #endregion EventHandler

    GameObject m_goButton;
    GameObject m_goLeftButton;

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas = GameObject.Find("Canvas").transform;
        m_trPopup = trCanvas.Find("Popup");

		Transform trCanvas2 = GameObject.Find("Canvas2").transform;
		m_trPopupList = trCanvas2.Find("PopupList");

        m_csPanelAttainment = trCanvas.Find("PanelAttainment").GetComponent<CsPanelAttainment>();

        //버튼 프리팹 로드
        if (m_goButton == null)
        {
            m_goButton = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/ButtonMenuTop");
        }

        //왼쪽버튼 프리팹 로드
        if (m_goLeftButton == null)
        {
            m_goLeftButton = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/LeftButton");
        }

        //그룹 1
        Transform trMenuList = transform.Find("MenuList");

        m_listCsMenuTop.Clear();
        m_listCsMenuTop = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 1);

        for (int i = 0; i < m_listCsMenuTop.Count; i++)
        {
            int nIndex = i;
            Transform trButton = trMenuList.Find("ButtonMenuTop" + m_listCsMenuTop[i].MenuId);

            if (trButton == null)
            {
                GameObject goButton = Instantiate(m_goButton, trMenuList);
                goButton.name = "ButtonMenuTop" + m_listCsMenuTop[i].MenuId;
                trButton = goButton.transform;
                trButton.SetAsFirstSibling();
            }

            Button buttonMenuTop = trButton.GetComponent<Button>();
            buttonMenuTop.onClick.RemoveAllListeners();
            buttonMenuTop.onClick.AddListener(() => OnClickMenuTop(m_listCsMenuTop[nIndex].MenuId));
            buttonMenuTop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image image = trButton.Find("ImageIcon").GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuTop[i].ImageName);
        }

		UpdateOpenMenu();

        Button buttonMainMenu = trMenuList.Find("ButtonMenuTop0").GetComponent<Button>();
        buttonMainMenu.onClick.RemoveAllListeners();
        buttonMainMenu.onClick.AddListener(OnClickOpenMainMenu);
        buttonMainMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonMatching = transform.Find("ButtonMatching").GetComponent<Button>();
        buttonMatching.onClick.RemoveAllListeners();
        buttonMatching.onClick.AddListener(OnClickOpenPopupMatching);
        buttonMatching.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        UpdateMatchingButton(CsUIData.Instance.DungeonInNow);

        m_trLeftMenuList = transform.Find("LeftMenuList");
		m_rtrLeftMenuList = m_trLeftMenuList.GetComponent<RectTransform>();
		m_vt2LeftMenuPosition = new Vector2(40f, m_rtrLeftMenuList.anchoredPosition.y);

        m_listCsMenuLeft.Clear();
        m_listCsMenuLeft = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 6);

        for (int i = 0; i < m_listCsMenuLeft.Count; i++)
        {
            int nIndex = i;
            Transform trButton = m_trLeftMenuList.Find("LeftButton" + m_listCsMenuLeft[i].MenuId);

            if (trButton == null)
            {
                trButton = Instantiate(m_goLeftButton, m_trLeftMenuList).transform;
                trButton.name = "LeftButton" + m_listCsMenuLeft[i].MenuId;
            }

            Button buttonLeft = trButton.GetComponent<Button>();
            buttonLeft.onClick.RemoveAllListeners();
            buttonLeft.onClick.AddListener(() => OnClickMenuLeft(m_listCsMenuLeft[nIndex].MenuId));
            buttonLeft.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image image = trButton.GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuLeft[i].ImageName);

            Text textName = trButton.Find("Text").GetComponent<Text>();
            textName.text = m_listCsMenuLeft[i].Name;
            CsUIData.Instance.SetFont(textName);

            trButton.SetSiblingIndex(m_listCsMenuLeft[i].SortNo);
        }

		UpdateOpenLeftMenu();

        m_toggleLeftMenu = m_trLeftMenuList.Find("ToggleLeftMenu").GetComponent<Toggle>();
        m_toggleLeftMenu.transform.SetAsLastSibling();
        m_toggleLeftMenu.onValueChanged.RemoveAllListeners();
        m_toggleLeftMenu.onValueChanged.AddListener(OnValueChangedLeftMenu);
        m_toggleLeftMenu.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        //던전 메뉴
        Transform trButtonList = transform.Find("ButtonList");

        m_buttonExit = trButtonList.Find("ButtonExit").GetComponent<Button>();
        m_buttonExit.onClick.RemoveAllListeners();
        m_buttonExit.onClick.AddListener(OnClickDungeonExit);
        m_buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonDungeonInventory = trButtonList.Find("ButtonInventory").GetComponent<Button>();
        buttonDungeonInventory.onClick.RemoveAllListeners();
        buttonDungeonInventory.onClick.AddListener(OnClickOpenInventory);
        buttonDungeonInventory.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonMenu = trButtonList.Find("ButtonMenu").GetComponent<Button>();
        buttonMenu.onClick.RemoveAllListeners();
        buttonMenu.onClick.AddListener(OnClickOpenMainMenu);
        buttonMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //던전 타이머
        Transform trTimer = transform.Find("Timer");

        m_textTime = trTimer.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTime);
        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), "00", "00");

		// 오픈 선물
		m_trButtonOpenGift = m_trLeftMenuList.Find("LeftButton" + (int)EnMenuId.OpenGift);

		if (m_trButtonOpenGift != null)
		{
			m_textOpenGiftRemainingTime = m_trButtonOpenGift.Find("Text").GetComponent<Text>();
			CsUIData.Instance.SetFont(m_textOpenGiftRemainingTime);
			m_textOpenGiftRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("A99_TXT_01001"), "00", "00", "00");
		}

		// 신규 선물
		m_trButtonRookieGift = m_trLeftMenuList.Find("LeftButton" + (int)EnMenuId.RookieGift);

		if (m_trButtonRookieGift != null)
		{
			m_textRookieGiftRemainingTime = m_trButtonRookieGift.Find("Text").GetComponent<Text>();
			CsUIData.Instance.SetFont(m_textRookieGiftRemainingTime);
			m_textRookieGiftRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("A100_TXT_01001"), "00", "00");
		}

        //길드 메뉴
        Transform trGuildButtonList = transform.Find("GuildButtonList");

        Button buttonGuildExit = trGuildButtonList.Find("ButtonExit").GetComponent<Button>();
        buttonGuildExit.onClick.RemoveAllListeners();
        buttonGuildExit.onClick.AddListener(OnClickGuildTerritoryExit);
        buttonGuildExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonGuildFarm = trGuildButtonList.Find("ButtonGuildFarm").GetComponent<Button>();
        buttonGuildFarm.onClick.RemoveAllListeners();
        buttonGuildFarm.onClick.AddListener(() => OnClickGuildAuto(EnAutoStateType.GuildFarm));
        buttonGuildFarm.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonGuildFoddWarehouse = trGuildButtonList.Find("ButtonGuildFoodWarehouse").GetComponent<Button>();
        buttonGuildFoddWarehouse.onClick.RemoveAllListeners();
        buttonGuildFoddWarehouse.onClick.AddListener(() => OnClickGuildAuto(EnAutoStateType.GuildFoodWareHouse));
        buttonGuildFoddWarehouse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonGuildFishing = trGuildButtonList.Find("ButtonGuildFishing").GetComponent<Button>();
        buttonGuildFishing.onClick.RemoveAllListeners();
        buttonGuildFishing.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonGuildFishing.onClick.AddListener(() => OnClickGuildAuto(EnAutoStateType.GuildFishing));

        Button buttonGuildAltar = trGuildButtonList.Find("ButtonGuildAltar").GetComponent<Button>();
        buttonGuildAltar.onClick.RemoveAllListeners();
        buttonGuildAltar.onClick.AddListener(() => OnClickGuildAuto(EnAutoStateType.GuildAlter));
        buttonGuildAltar.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonGuildMenu = trGuildButtonList.Find("ButtonMenu").GetComponent<Button>();
        buttonGuildMenu.onClick.RemoveAllListeners();
        buttonGuildMenu.onClick.AddListener(OnClickOpenMainMenu);
        buttonGuildMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonNationWar = transform.Find("ButtonNationWar").GetComponent<Button>();
        m_buttonNationWar.onClick.RemoveAllListeners();
        m_buttonNationWar.onClick.AddListener(OnClickNationWar);
        m_buttonNationWar.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        DisplayNationWarButton();

        m_buttonHelper = transform.Find("ButtonHelper").GetComponent<Button>();
        m_buttonHelper.onClick.RemoveAllListeners();
        m_buttonHelper.onClick.AddListener(OnClickHelper);
        m_buttonHelper.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        DisplayHelperButton();

		m_buttonBless = transform.Find("ButtonBless").GetComponent<Button>();
		m_buttonBless.onClick.RemoveAllListeners();
		m_buttonBless.onClick.AddListener(OnClickBless);
		UpdateBlessButton();

        m_buttonPreview = transform.Find("ButtonPreview").GetComponent<Button>();
        m_buttonPreview.onClick.RemoveAllListeners();
        m_buttonPreview.onClick.AddListener(OnClickPreview);
        m_buttonPreview.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textPreview = m_buttonPreview.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPreview);
        textPreview.text = CsConfiguration.Instance.GetString("A94_TXT_00003");

        m_buttonMoneyBuff = transform.Find("ButtonMoneyBuff").GetComponent<Button>();
        m_buttonMoneyBuff.onClick.RemoveAllListeners();
        m_buttonMoneyBuff.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonMoneyBuff.onClick.AddListener(OnClickOpenPopupMoneyBuff);

        DisplayPreviewButton();
        UpdateAttaniment();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttaniment()
    {
        Transform trMenuList = transform.Find("MenuList");
        Transform trButtonAttaniment = trMenuList.Find("ButtonMenuTop" + (int)EnMenuId.Attaniment);

        CsAttainmentEntry csAttainmentEntry = CsGameData.Instance.GetAttainmentEntry(CsGameData.Instance.MyHeroInfo.RewardedAttainmentEntryNo + 1);
        trButtonAttaniment.gameObject.SetActive(csAttainmentEntry == null ? false : true);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadMainMenu()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupMainMenu/MainMenu");
        yield return resourceRequest;
        m_goPopupMainMenu = (GameObject)resourceRequest.asset;

        OpenPopupMainMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupMainMenu()
    {
        Transform trMainMenu = m_trPopup.Find("PopupMainMenu");

        if (trMainMenu == null)
        {
            GameObject goMainMenu = Instantiate(m_goPopupMainMenu, m_trPopup);
            goMainMenu.name = "PopupMainMenu";
            trMainMenu = goMainMenu.transform;
        }
        else
        {
            trMainMenu.gameObject.SetActive(true);
        }

        trMainMenu.GetComponent<CsUiAnimation>().StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMenuList(bool bIsOn)
    {
        Transform trMenuList = transform.Find("MenuList");

        if (trMenuList != null)
        {
            trMenuList.gameObject.SetActive(bIsOn);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayLeftMenuList(bool bIson)
    {
        m_trLeftMenuList.gameObject.SetActive(bIson);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPanelDungeon(bool bIsEnter)
    {
        DisplayDungeonMenuList(bIsEnter);
        DisplayTimer(bIsEnter);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayDungeonMenuList(bool bIsOn)
    {
        Transform trMenuList = transform.Find("ButtonList");

        if (trMenuList != null)
        {
            trMenuList.gameObject.SetActive(bIsOn);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayGuildMenuList(bool bIsOn)
    {
        Transform trMenuList = transform.Find("GuildButtonList");

        if (trMenuList != null)
        {
            trMenuList.gameObject.SetActive(bIsOn);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTimer(bool bIsOn)
    {
        Transform trTimer = transform.Find("Timer");

        if (trTimer != null)
        {
            trTimer.gameObject.SetActive(bIsOn);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTimer()
    {
        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), m_timeSpan.Minutes.ToString("00"), m_timeSpan.Seconds.ToString("00"));
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayButtonNationWar(bool bIsOn)
    {
        // 국가전이 없을 경우
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        if (csNationWarDeclaration == null)
        {
            m_buttonNationWar.gameObject.SetActive(false);
            return;
        }
        else
        {
            switch (csNationWarDeclaration.Status)
            {
                // 국전 선포 중
                case EnNationWarDeclaration.Before:
                    m_buttonNationWar.gameObject.SetActive(false);

                    break;
                // 국전 시작
                case EnNationWarDeclaration.Current:
                    m_buttonNationWar.gameObject.SetActive(bIsOn);

                    break;
                // 국전 종료
                case EnNationWarDeclaration.End:
                    if (CsNationWarManager.Instance.NationWarJoined)
                    {
                        if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.CompareTo(DateTime.Now.Date.AddSeconds(CsGameData.Instance.NationWar.ResultDisplayEndTime)) <= 0 &&
                        0 <= CsGameData.Instance.MyHeroInfo.CurrentDateTime.CompareTo(DateTime.Now.Date.AddSeconds(CsGameData.Instance.NationWar.StartTime)))
                        {
                            m_buttonNationWar.gameObject.SetActive(bIsOn);
                        }
                        else
                        {
                            m_buttonNationWar.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        m_buttonNationWar.gameObject.SetActive(false);
                    }

                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonNationWar()
    {
        // 국가전이 없을 경우
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        if (csNationWarDeclaration == null)
        {
            return;
        }
        // 국가전이 있을 경우
        else
        {
            Text textButtonNationWar = m_buttonNationWar.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonNationWar);

            // 국가전 중일 경우 남은 시간 표시
            if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
            {
                textButtonNationWar.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), m_timeSpan.Minutes.ToString("0#"), m_timeSpan.Seconds.ToString("0#"));
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonQuestDungeon()
    {
        CsMainQuestDungeonManager.Instance.SendMainQuestDungeonAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonStoryDungeon()
    {
        CsDungeonManager.Instance.SendStoryDungeonAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonExpDungeon()
    {
        CsDungeonManager.Instance.SendExpDungeonAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonGoldDungeon()
    {
        CsDungeonManager.Instance.SendGoldDungeonAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonAncientRelic()
    {
        CsDungeonManager.Instance.SendAncientRelicAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonArtifactRoom()
    {
        CsDungeonManager.Instance.SendArtifactRoomAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonFieldOfHonor()
    {
        CsDungeonManager.Instance.SendFieldOfHonorAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonSoulConveter()
    {
        CsDungeonManager.Instance.SendSoulCoveterAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonEliteDungeon()
    {
        CsDungeonManager.Instance.SendEliteDungeonAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonProofOfValor()
    {
        CsDungeonManager.Instance.SendProofOfValorAbandon();
    }

	//---------------------------------------------------------------------------------------------------
	void AbandonWisdomTemple()
	{
		CsDungeonManager.Instance.SendWisdomTempleAbandon();
	}

	//---------------------------------------------------------------------------------------------------
	void AbandonRuinsReclaim()
	{
		CsDungeonManager.Instance.SendRuinsReclaimAbandon();
	}

    //---------------------------------------------------------------------------------------------------
    void AbandonInfiniteWar()
    {
        CsDungeonManager.Instance.SendInfiniteWarAbandon();
    }

	//---------------------------------------------------------------------------------------------------
	void AbandonFearAltar()
	{
		CsDungeonManager.Instance.SendFearAltarAbandon();
	}

    //---------------------------------------------------------------------------------------------------
    void AbandonWarMemory()
    {
        CsDungeonManager.Instance.SendWarMemoryAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonOsirisRoom()
    {
        CsDungeonManager.Instance.SendOsirisRoomAbandon();
    }

	//---------------------------------------------------------------------------------------------------
	void AbandonBiographyQuestDungeon()
	{
		CsDungeonManager.Instance.SendBiographyQuestDungeonAbandon();
	}

    //---------------------------------------------------------------------------------------------------
    void AbandonDragonNest()
    {
        CsDungeonManager.Instance.SendDragonNestAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonTradeShip()
    {
        CsDungeonManager.Instance.SendTradeShipAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void AbandonAnkouTomb()
    {
        Debug.Log("@@ AbandonAnkouTomb @@");
        CsDungeonManager.Instance.SendAnkouTombAbandon();
    }

    //---------------------------------------------------------------------------------------------------
    void ActionGuildTerritoryExit()
    {
        CsGuildManager.Instance.SendGuildTerritoryExit();
    }

    //---------------------------------------------------------------------------------------------------
    void DungeonEnter()
    {
        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), "00", "00");
        DisplayMenuList(false);
        DisplayLeftMenuList(false);
        DisplayPanelDungeon(true);
        DisplayButtonNationWar(false);
        DisplayButtonMoneyBuff(true);
    }

    //---------------------------------------------------------------------------------------------------
    void DungeonExit()
    {
        m_flLimitTime = 0;
        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), "00", "00");
        DisplayMenuList(true);
        DisplayLeftMenuList(true);
        DisplayPanelDungeon(false);
        DisplayButtonNationWar(true);
        DisplayButtonMoneyBuff(false);
    }

    //---------------------------------------------------------------------------------------------------
    void GuildTerritoryEnter()
    {
        DisplayMenuList(false);
        DisplayLeftMenuList(false);
        DisplayGuildMenuList(true);
        DisplayButtonNationWar(false);
    }

    //---------------------------------------------------------------------------------------------------
    void GuildTerritoryExit()
    {
        DisplayMenuList(true);
        DisplayLeftMenuList(false);
        DisplayGuildMenuList(false);
        DisplayButtonNationWar(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMatchingButton(EnDungeon enDungeon)
    {
        switch (enDungeon)
        {
            case EnDungeon.AncientRelic:

                if (CsDungeonManager.Instance.AncientRelicState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;

            case EnDungeon.SoulCoveter:

                if (CsDungeonManager.Instance.SoulCoveterMatchingState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;

			case EnDungeon.RuinsReclaim:

				if (CsDungeonManager.Instance.RuinsReclaimMatchingState == EnDungeonMatchingState.None)
				{
					transform.Find("ButtonMatching").gameObject.SetActive(false);
				}
				else
				{
					transform.Find("ButtonMatching").gameObject.SetActive(true);
				}

				break;

            case EnDungeon.InfiniteWar:

                if (CsDungeonManager.Instance.InfiniteWarMatchingState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;

			case EnDungeon.FearAltar:

				if (CsDungeonManager.Instance.FearAltarMatchingState == EnDungeonMatchingState.None)
				{
					transform.Find("ButtonMatching").gameObject.SetActive(false);
				}
				else
				{
					transform.Find("ButtonMatching").gameObject.SetActive(true);
				}

				break;

            case EnDungeon.WarMemory:

                if (CsDungeonManager.Instance.WarMemoryMatchingState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;

            case EnDungeon.DragonNest:

                if (CsDungeonManager.Instance.DragonNestMatchingState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;

            case EnDungeon.TradeShip:
                if (CsDungeonManager.Instance.TradeShipMatchingState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;

            case EnDungeon.AnkouTomb:
                if (CsDungeonManager.Instance.AnkouTombMatchingState == EnDungeonMatchingState.None)
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(false);
                }
                else
                {
                    transform.Find("ButtonMatching").gameObject.SetActive(true);
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateOpenMenu()
    {
        Transform trMenuList = transform.Find("MenuList");

        for (int i = 0; i < m_listCsMenuTop.Count; i++)
        {
            Transform trButton = trMenuList.Find("ButtonMenuTop" + m_listCsMenuTop[i].MenuId);
            trButton.gameObject.SetActive(CsUIData.Instance.MenuOpen(m_listCsMenuTop[i]));
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateOpenLeftMenu()
	{
		Transform trLeftMenuList = transform.Find("LeftMenuList");

		for (int i = 0; i < m_listCsMenuLeft.Count; i++)
		{
			Transform trButton = trLeftMenuList.Find("LeftButton" + m_listCsMenuLeft[i].MenuId);
			trButton.gameObject.SetActive(CheckShowLeftTopButton(m_listCsMenuLeft[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateMenuNotice(int nMenuId, bool bIsOn)
    {
        Transform trButton = transform.Find("MenuList/ButtonMenuTop" + nMenuId);

        if (trButton != null)
        {
            Transform trNotice = trButton.Find("ImageNotice");
            trNotice.gameObject.SetActive(bIsOn);
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateLeftTopMenuNotice(int nMenuId, bool bIsOn)
	{
		Transform trButton = transform.Find("LeftMenuList/LeftButton" + nMenuId);

		if (trButton != null)
		{
			Transform trNotice = trButton.Find("ImageNotice");
			trNotice.gameObject.SetActive(bIsOn);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void DisplayNationWarButton()
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        if (csNationWarDeclaration != null)
        {
            switch (csNationWarDeclaration.Status)
            {
                case EnNationWarDeclaration.Current:
                    TimeSpan tsNationWarRemainingTime = DateTime.Now.Date.AddSeconds(CsGameData.Instance.NationWar.EndTime) - CsGameData.Instance.MyHeroInfo.CurrentDateTime;
                    m_flRemainingNationWarTime = (float)tsNationWarRemainingTime.TotalSeconds + Time.realtimeSinceStartup;
                    break;
                case EnNationWarDeclaration.End:
                    m_buttonNationWar.transform.Find("Text").gameObject.SetActive(false);
                    break;
            }

            CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);

            if (csContinent == null || CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.PvpMinHeroLevel)
            {
                m_buttonNationWar.gameObject.SetActive(false);
            }
            else
            {
                DisplayButtonNationWar(true);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	bool CheckShowLeftTopButton(CsMenu csMenu)
	{
		bool bIsOpen = CsUIData.Instance.MenuOpen(csMenu);

		switch ((EnMenuId)csMenu.MenuId)
		{
			case EnMenuId.OpenGift:
				bIsOpen &= CsGameData.Instance.MyHeroInfo.ReceivedOpenGiftRewardList.Count < m_nOpenGiftLastDay;
				break;
			case EnMenuId.RookieGift:
				bIsOpen &= 0 < CsGameData.Instance.MyHeroInfo.RookieGiftNo && 
					CsGameData.Instance.MyHeroInfo.RookieGiftNo <= CsGameData.Instance.RookieGiftList.Max(gift => gift.GiftNo);
				break;
			default:
				break;
		}

		return bIsOpen;
	}

    #region 도우미
    //---------------------------------------------------------------------------------------------------
    void DisplayHelperButton()
    {
        if (CsGameConfig.Instance.GuideActivationRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            m_buttonHelper.gameObject.SetActive(true);
        }
        else
        {
            m_buttonHelper.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHelperButton(bool bIson)
    {
        Transform trImageNotice = m_buttonHelper.transform.Find("ImageNotice");
        trImageNotice.gameObject.SetActive(bIson);

        m_buttonHelper.interactable = bIson;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupHelper()
    {
        if (m_goPopupHelper == null)
        {
            StartCoroutine(LoadPopupHelper());
        }
        else
        {
            CreatePopupHelper();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupHelper()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupHelper");
        yield return resourceRequest;
        m_goPopupHelper = (GameObject)resourceRequest.asset;

        CreatePopupHelper();
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupHelper()
    {
        Transform trPopupHelper = m_trPopup.Find("PopupHelper");

        if (trPopupHelper == null)
        {
            trPopupHelper = Instantiate(m_goPopupHelper, m_trPopup).transform;
            trPopupHelper.name = "PopupHelper";
        }
    }
    #endregion 도우미

	#region 축복
	//---------------------------------------------------------------------------------------------------
	void UpdateBlessButton()
	{
        if (CsBlessingQuestManager.Instance.HeroBlessingQuestList.Count > 0 ||
			CsBlessingQuestManager.Instance.HeroBlessingReceivedList.Count > 0 ||
            CsPresentManager.Instance.CsHeroReceivedPresentList.Count > 0)
		{
			m_buttonBless.gameObject.SetActive(true);
		}
		else
		{
			m_buttonBless.gameObject.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupBless()
	{
		if (m_goPopupBless == null)
		{
			StartCoroutine(LoadPopupBless());
		}
		else
		{
			CreatePopupBless();
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupBless()
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupBless");
		yield return resourceRequest;
		m_goPopupBless = (GameObject)resourceRequest.asset;

		CreatePopupBless();
	}

	//---------------------------------------------------------------------------------------------------
	void CreatePopupBless()
	{
		Transform trPopupHelper = m_trPopupList.Find("PopupBless");

		if (trPopupHelper == null)
		{
			trPopupHelper = Instantiate(m_goPopupBless, m_trPopupList).transform;
			trPopupHelper.name = "PopupBless";
		}
	}
	#endregion 축복

	#region 컨텐츠 개방 버튼
	//---------------------------------------------------------------------------------------------------
    void DisplayPreviewButton()
    {
        if (CsUIData.Instance.DungeonInNow == EnDungeon.OsirisRoom ||
            CsUIData.Instance.DungeonInNow == EnDungeon.TradeShip ||
            CsUIData.Instance.DungeonInNow == EnDungeon.AnkouTomb)
        {
            m_buttonPreview.gameObject.SetActive(false);
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.MenuContentOpenPreviewRequiredHeroLevel)
            {
                m_buttonPreview.gameObject.SetActive(false);
            }
            else
            {
                CsMenuContentOpenPreview csMenuContentOpenPreview = GetCurrentMenuContentOpenPreview();
                CsMenuContentOpenPreview prevOpenPreview = GetLastOpenedMenuContentOpenPreview();

                int nDisplayStartMainQuestNo = prevOpenPreview == null ? 0 : prevOpenPreview.MenuContent.RequiredMainQuestNo + 1;

                if (csMenuContentOpenPreview == null)
                {
                    m_buttonPreview.gameObject.SetActive(false);
                }
                else
                {
                    m_buttonPreview.gameObject.SetActive(true);

                    Image imageIcon = m_buttonPreview.transform.Find("ImageIcon").GetComponent<Image>();
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + csMenuContentOpenPreview.MenuContent.ImageName);

                    int nCurrent = CsMainQuestManager.Instance.MainQuest.MainQuestNo - nDisplayStartMainQuestNo;
                    int nLast = (csMenuContentOpenPreview.MenuContent.RequiredMainQuestNo + 1) - nDisplayStartMainQuestNo;

                    if (nCurrent > nLast)
                    {
                        nCurrent = nLast;
                    }

                    Transform trSlider = m_buttonPreview.transform.Find("Slider");
                    Slider slider = trSlider.GetComponent<Slider>();
                    slider.minValue = 0;
                    slider.maxValue = nLast;
                    slider.value = nCurrent;

                    CsUIData.Instance.SetText(trSlider.Find("TextProgress"), string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nCurrent, nLast), false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupPreview()
    {
        if (m_goPopupPreview == null)
        {
            StartCoroutine(LoadPopupPreview());
        }
        else
        {
            CreatePopupPreview();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupPreview()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupContentOpenPreview");
        yield return resourceRequest;
        m_goPopupPreview = (GameObject)resourceRequest.asset;

        CreatePopupPreview();
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupPreview()
    {
        Transform trPopupPreview = m_trPopup.Find("PopupPreview");

        if (trPopupPreview == null)
        {
            trPopupPreview = Instantiate(m_goPopupPreview, m_trPopup).transform;
            trPopupPreview.name = "PopupPreview";
        }

        Button buttonClose = trPopupPreview.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopupPreview);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        UpdatePopupPreview();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupPreview()
    {
        Transform trPopupPreview = m_trPopup.Find("PopupPreview");

        if (trPopupPreview == null)
        {
            return;
        }
        else
        {
            CsMenuContentOpenPreview csMenuContentOpenPreview = GetCurrentMenuContentOpenPreview();

            if (csMenuContentOpenPreview == null)
            {
                ClosePopupPreview();
            }
            else
            {
                Transform trImageBackground = trPopupPreview.Find("ImageBackground");

                Text textContentName = trImageBackground.Find("TextContentName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textContentName);
                textContentName.text = csMenuContentOpenPreview.MenuContent.Name;

                Text textDescription = trImageBackground.Find("TextDescription").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDescription);
                textDescription.text = csMenuContentOpenPreview.MenuContent.Description;

                Transform trRequireOpenMenu = trImageBackground.Find("RequireOpenMenu");

                Text textRequireLevel = trRequireOpenMenu.Find("TextRequireLevel").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRequireLevel);

                if (csMenuContentOpenPreview.MenuContent.RequiredHeroLevel == 0)
                {
                    textRequireLevel.gameObject.SetActive(false);
                }
                else
                {
                    textRequireLevel.text = string.Format(CsConfiguration.Instance.GetString("A94_TXT_00001"), csMenuContentOpenPreview.MenuContent.RequiredHeroLevel);
                    textRequireLevel.gameObject.SetActive(true);
                }

                Text textRequireMainQuest = trRequireOpenMenu.Find("TextRequireMainQuest").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRequireMainQuest);

                if (csMenuContentOpenPreview.MenuContent.RequiredMainQuestNo == 0)
                {
                    textRequireMainQuest.gameObject.SetActive(false);
                }
                else
                {
                    CsMainQuest csMainQuest = CsGameData.Instance.GetMainQuest(csMenuContentOpenPreview.MenuContent.RequiredMainQuestNo);

                    if (csMainQuest == null)
                    {
                        textRequireMainQuest.gameObject.SetActive(false);
                    }
                    else
                    {
                        textRequireMainQuest.text = string.Format(CsConfiguration.Instance.GetString("A94_TXT_00002"), csMainQuest.Title);
                        textRequireMainQuest.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupPreview()
    {
        ClosePopupPreview();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupPreview()
    {
        Transform trPopupPreview = m_trPopup.Find("PopupPreview");

        if (trPopupPreview == null)
        {
            return;
        }
        else
        {
            Destroy(trPopupPreview.gameObject);
            trPopupPreview = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    CsMenuContentOpenPreview GetCurrentMenuContentOpenPreview()
    {
        CsMenuContentOpenPreview csMenuContentOpenPreview = null;

        for (int i = 0; i < CsGameData.Instance.MenuContentOpenPreviewList.Count; i++)
        {
            if (CsUIData.Instance.MenuContentOpen(CsGameData.Instance.MenuContentOpenPreviewList[i].MenuContent))
            {
                continue;
            }
            else
            {
                csMenuContentOpenPreview = CsGameData.Instance.MenuContentOpenPreviewList[i];
                break;
            }
        }

        return csMenuContentOpenPreview;
    }

	//---------------------------------------------------------------------------------------------------
	CsMenuContentOpenPreview GetLastOpenedMenuContentOpenPreview()
	{
		CsMenuContentOpenPreview csMenuContentOpenPreview = null;

		for (int i = 0; i < CsGameData.Instance.MenuContentOpenPreviewList.Count; i++)
		{
			if (CsUIData.Instance.MenuContentOpen(CsGameData.Instance.MenuContentOpenPreviewList[i].MenuContent))
			{
				if (i + 1 < CsGameData.Instance.MenuContentOpenPreviewList.Count &&
					!CsUIData.Instance.MenuContentOpen(CsGameData.Instance.MenuContentOpenPreviewList[i + 1].MenuContent))
				{
					csMenuContentOpenPreview = CsGameData.Instance.MenuContentOpenPreviewList[i];
					break;
				}
			}
		}

		return csMenuContentOpenPreview;
	}

    #endregion 컨텐츠 개방 버튼

    #region 재화 버프

    enum EnMoneyBuffType
    {
        Gold = 1, 
        Dia = 2, 
    }

    CsMoneyBuff m_csMoneyBuff = null;
    bool m_bActivateMoneyBuff = false;

    #region OsirisRoom.Event
    
    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupMoneyBuff()
    {
        OpenPopupMoneyBuff();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMoneyBuffClose()
    {
        PopupMoneyBuffClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMoneyBuff(bool bIson, CsMoneyBuff csMoneyBuff)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (csMoneyBuff == null)
            {
                return;
            }
            else
            {
                m_csMoneyBuff = csMoneyBuff;
                UpdatePopupMoneyBuff();
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMoneyBuffApply()
    {
        switch ((EnMoneyBuffType)m_csMoneyBuff.MoneyType)
        {
            case EnMoneyBuffType.Gold:
                if (CsGameData.Instance.MyHeroInfo.Gold < m_csMoneyBuff.MoneyAmount)
                {
                    // 골드 부족
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_GOLDERROR"));
                }
                else
                {
                    CheckPrevBuffActivate();
                }
                break;

            case EnMoneyBuffType.Dia:
                if (CsGameData.Instance.MyHeroInfo.Dia < m_csMoneyBuff.MoneyAmount)
                {
                    // 다이아 부족
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
                }
                else
                {
                    CheckPrevBuffActivate();
                }
                break;
        }

        PopupMoneyBuffClose();
    }

    #endregion OsirisRoom.Event

    //---------------------------------------------------------------------------------------------------
    void OpenPopupMoneyBuff()
    {
        if (m_goPopupMoneyBuff == null)
        {
            StartCoroutine(LoadPopupMoneyBuff());
        }
        else
        {
            InitializePopupMoneyBuff();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupMoneyBuff()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/PopupDungeonBuff");
        yield return resourceRequest;

        m_goPopupMoneyBuff = (GameObject)resourceRequest.asset;

        InitializePopupMoneyBuff();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupMoneyBuff()
    {
        if (m_trPopupMoneyBuff == null)
        {
            m_trPopupMoneyBuff = Instantiate(m_goPopupMoneyBuff, m_trPopup).transform;
            m_trPopupMoneyBuff.name = "PopupDungeonBuff";
        }
        else
        {
            m_trPopupMoneyBuff.gameObject.SetActive(true);
        }

        Transform trImageBackground = m_trPopupMoneyBuff.Find("ImageBackground");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickMoneyBuffClose);

        Text textTitle = trImageBackground.Find("TextTitle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTitle);
        textTitle.text = CsConfiguration.Instance.GetString("A124_TXT_00003");

        Transform trBuffList = trImageBackground.Find("BuffList");

        for (int i = 0; i < trBuffList.childCount; i++)
        {
            trBuffList.GetChild(i).gameObject.SetActive(true);

            if (i + 1 <= CsGameData.Instance.MoneyBuffList.Count)
            {
                CsMoneyBuff csMoneyBuff = CsGameData.Instance.MoneyBuffList[i];

                Toggle toggleMoneyBuff = trBuffList.GetChild(i).GetComponent<Toggle>();
                toggleMoneyBuff.onValueChanged.RemoveAllListeners();

                if (i == 0)
                {
                    toggleMoneyBuff.isOn = true;
                    m_csMoneyBuff = csMoneyBuff;
                }
                else
                {
                    toggleMoneyBuff.isOn = false;
                }

                Text textMoneyType = toggleMoneyBuff.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textMoneyType);

                if (csMoneyBuff.MoneyType == (int)EnMoneyBuffType.Gold)
                {
                    textMoneyType.text = CsConfiguration.Instance.GetString("A124_TXT_00004");
                }
                else
                {
                    textMoneyType.text = CsConfiguration.Instance.GetString("A124_TXT_00005");
                }

                toggleMoneyBuff.onValueChanged.AddListener((ison) => OnValueChangedMoneyBuff(ison, csMoneyBuff));

                trBuffList.GetChild(i).gameObject.SetActive(true);
            }
        }

        Button buttonApply = trImageBackground.Find("ButtonApply").GetComponent<Button>();
        buttonApply.onClick.RemoveAllListeners();
        buttonApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonApply.onClick.AddListener(OnClickMoneyBuffApply);

        Text textButtonApply = buttonApply.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonApply);
        textButtonApply.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");

        UpdatePopupMoneyBuff();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupMoneyBuff()
    {
        if (m_trPopupMoneyBuff == null || m_csMoneyBuff == null)
        {
            return;
        }
        else
        {
            Transform trImageBackground = m_trPopupMoneyBuff.Find("ImageBackground");

            Text textDescription = trImageBackground.Find("TextDetail").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDescription);

            List<CsMoneyBuffAttr> listMoneyBuffIndex = new List<CsMoneyBuffAttr>();

            for (int i = 0; i < m_csMoneyBuff.MoneyBuffAttrList.Count; i++)
            {
                listMoneyBuffIndex.Add(m_csMoneyBuff.MoneyBuffAttrList[i]);
            }

            CsMoneyBuffAttr csMoneyBuffAttr = null;

            if (CsGameData.Instance.MyHeroInfo.Job.OffenseType == EnOffenseType.Physical)
            {
                csMoneyBuffAttr = m_csMoneyBuff.MoneyBuffAttrList.Find(a => a.Attr.EnAttr == EnAttr.MagicalOffense);
            }
            else
            {
                csMoneyBuffAttr = m_csMoneyBuff.MoneyBuffAttrList.Find(a => a.Attr.EnAttr == EnAttr.PhysicalOffense);
            }


            if (csMoneyBuffAttr != null)
            {
                listMoneyBuffIndex.Remove(csMoneyBuffAttr);
            }

            textDescription.text = string.Format(CsConfiguration.Instance.GetString(m_csMoneyBuff.Description), m_csMoneyBuff.MoneyAmount, m_csMoneyBuff.Lifetime,
                                                                                    listMoneyBuffIndex[0].Attr.Name, (listMoneyBuffIndex[0].AttrFactor - 1) * 100,
                                                                                    listMoneyBuffIndex[1].Attr.Name, (listMoneyBuffIndex[1].AttrFactor - 1) * 100,
                                                                                    listMoneyBuffIndex[2].Attr.Name, (listMoneyBuffIndex[2].AttrFactor - 1) * 100);

            listMoneyBuffIndex.Clear();
            listMoneyBuffIndex = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckPrevBuffActivate()
    {
        // 버프 사용 중
        if (m_bActivateMoneyBuff)
        {
            if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.OsirisRoom)
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A124_TXT_00006"),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsDungeonManager.Instance.SendOsirisRoomMoneyBuffActivate(m_csMoneyBuff.BuffId),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
            else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.TradeShip)
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A124_TXT_00006"),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsDungeonManager.Instance.SendTradeShipMoneyBuffActivate(m_csMoneyBuff.BuffId),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
            else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.AnkouTomb)
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A124_TXT_00006"),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsDungeonManager.Instance.SendAnkouTombMoneyBuffActivate(m_csMoneyBuff.BuffId),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
        }
        else
        {
            if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.OsirisRoom)
            {
                CsDungeonManager.Instance.SendOsirisRoomMoneyBuffActivate(m_csMoneyBuff.BuffId);
            }
            else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.TradeShip)
            {
                CsDungeonManager.Instance.SendTradeShipMoneyBuffActivate(m_csMoneyBuff.BuffId);
            }
            else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.AnkouTomb)
            {
                CsDungeonManager.Instance.SendAnkouTombMoneyBuffActivate(m_csMoneyBuff.BuffId);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMoneyBuffLifeTime()
    {
        Text textMoneyBuffLifeTime = m_buttonMoneyBuff.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMoneyBuffLifeTime);

        if (m_bActivateMoneyBuff)
        {
            TimeSpan tsMoneyBuffLifeTime = TimeSpan.FromSeconds(m_flMoneyBuffLifeTime - Time.realtimeSinceStartup);
            textMoneyBuffLifeTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsMoneyBuffLifeTime.Minutes.ToString("0#"), tsMoneyBuffLifeTime.Seconds.ToString("0#"));
        }
        else
        {
            textMoneyBuffLifeTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), "00", "00");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayButtonMoneyBuff(bool bIson)
    {
        if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.OsirisRoom ||
            CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.TradeShip ||
            CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.AnkouTomb)
        {
            m_buttonMoneyBuff.gameObject.SetActive(bIson);
        }
        else
        {
            m_buttonMoneyBuff.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void PopupMoneyBuffClose()
    {
        if (m_trPopupMoneyBuff == null)
        {
            return;
        }
        else
        {
            Destroy(m_trPopupMoneyBuff.gameObject);
            m_trPopupMoneyBuff = null;
        }
    }

    #endregion 재화 버프
}