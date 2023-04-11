using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ClientCommon;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-11)
//---------------------------------------------------------------------------------------------------

public enum EnItemBootyType
{
    Item = 1,
}

public class CsPanelDungeon : MonoBehaviour
{
    Transform m_trPopupDungeonClear;
    Transform m_trPopupMatchingState;
    //
    Transform m_trNewPopupDungeonClear;
	Transform m_trPopupRuinsReclaimClear;

    Transform m_trPopupDungeonStartCount;
    Transform m_trPopup;

	Transform m_trButtonTargetHalidomMonster;	// 공포의 제단 성물 몬스터 타겟 버튼
	Transform m_trButtonShowHalidomCollection;	// 공포의 제단 성물 조회 버튼

	Transform m_trButtons;

    Transform m_trDungeonClear;                 // 앙쿠의 무덤, 무역선 탈환 클리어 팝업

	float m_nHalidomDisplayDuration = 0;		// 공포의 제단 성물 조회 버튼 유지 시간

    GameObject m_goPopupMainMenu;
    GameObject m_goRewardItem;
    GameObject m_goPopupDungeonStartCount;
    GameObject m_goDragonNestHeroInfo;
    //
    [SerializeField] GameObject m_goRewardItemSlot;

    Text m_textRemaining;
    Text m_textPopupName;
    Text m_textState;
    Text m_textTime;
    Text m_textButtonCancel;
	Text m_textProgress;
	Button m_buttonProgress;

	PDItemBooty[] m_pDItemBooty;
    PDItemBooty m_pDItemBootyArtifactRoom;
    PDInfiniteWarRanking[] m_aPDInfiniteWarRanking;
    PDWarMemoryRanking[] m_aPDWarMemoryRanking;
    PDSimpleHero[] m_arrPDHero;

	List<PDItemBooty> m_listWisdomTempleStepReward;
    List<PDItemBooty> m_listDragonNestStepReward;
    List<PDTradeShipObjectInstance> m_listTradeShipObjectInstance;

    float m_flTime = 0;
    float m_flDungeonExitDelayTime;
    float m_flDungeonCelarTime;
    float m_flDungeonStartDelayTime;
    long m_lGoldDungeonRewardGold;
    long m_lExpDungeonRewardExp;
    int m_nGetHonorPoint;
    int m_nHit;
    int m_nDamaged;
    int m_nDifficulty;

    int m_nPreCreatureCardId;
    int m_nPreBossMonsterArrangeId;

    string m_strExitGuide;

    IEnumerator m_IEnumerator;
    EnDungeon m_enDungeonMatching = EnDungeon.None;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventOpenPopupMatching += OnEventOpenPopupMatching;

		CsGameEventToUI.Instance.EventHideMainUI += OnEventHideMainUI;
		CsGameEventUIToUI.Instance.EventVisibleMainUI += OnEventVisibleMainUI;

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonClear += OnEventMainQuestDungeonClear;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonFail += OnEventMainQuestDungeonFail;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;

        //스토리던전
        CsDungeonManager.Instance.EventStoryDungeonStepStart += OnEventStoryDungeonStepStart;
        CsDungeonManager.Instance.EventStoryDungeonClear += OnEventStoryDungeonClear;
        CsDungeonManager.Instance.EventStoryDungeonFail += OnEventStoryDungeonFail;
        CsDungeonManager.Instance.EventStoryDungeonExit += OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;
        CsGameEventToUI.Instance.EventClearDirectionFinish += OnEventClearDirectionFinish;

        //경험치던전
        CsDungeonManager.Instance.EventExpDungeonClear += OnEventExpDungeonClear;
        CsDungeonManager.Instance.EventExpDungeonExit += OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonBanished += OnEventExpDungeonBanished;

        //골드던전
        CsDungeonManager.Instance.EventGoldDungeonClear += OnEventGoldDungeonClear;
        CsDungeonManager.Instance.EventGoldDungeonFail += OnEventGoldDungeonFail;
        CsDungeonManager.Instance.EventGoldDungeonExit += OnEventGoldDungeonExit;
        CsDungeonManager.Instance.EventGoldDungeonBanished += OnEventGoldDungeonBanished;

        //고대 유물의 방
        CsDungeonManager.Instance.EventArtifactRoomClear += OnEventArtifactRoomClear;
        CsDungeonManager.Instance.EventArtifactRoomFail += OnEventArtifactRoomFail;
        CsDungeonManager.Instance.EventArtifactRoomExit += OnEventArtifactRoomExit;
        CsDungeonManager.Instance.EventArtifactRoomBanished += OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomBanishedForNextFloorChallenge += EventArtifactRoomBanishedForNextFloorChallenge;
        CsDungeonManager.Instance.EventArtifactRoomNextFloorChallenge += OnEventArtifactRoomNextFloorChallenge;

        //고대인의 유적
        CsDungeonManager.Instance.EventAncientRelicClear += OnEventAncientRelicClear;
        CsDungeonManager.Instance.EventAncientRelicFail += OnEventAncientRelicFail;
        CsDungeonManager.Instance.EventAncientRelicExit += OnEventAncientRelicExit;
        CsDungeonManager.Instance.EventAncientRelicBanished += OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicStepCompleted += OnEventAncientRelicStepCompleted;

        CsDungeonManager.Instance.EventAncientRelicMatchingCancel += OnEventAncientRelicMatchingCancel;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomBanished += OnEventAncientRelicMatchingRoomBanished;
        CsDungeonManager.Instance.EventAncientRelicMatchingStart += OnEventAncientRelicMatchingStart;
        CsDungeonManager.Instance.EventAncientRelicMatchingStatusChanged += OnEventAncientRelicMatchingStatusChanged;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomPartyEnter += OnEventAncientRelicMatchingRoomPartyEnter;

        //검투 대회
        CsDungeonManager.Instance.EventFieldOfHonorClear += OnEventFieldOfHonorClear;
        CsDungeonManager.Instance.EventFieldOfHonorFail += OnEventFieldOfHonorFail;
        CsDungeonManager.Instance.EventFieldOfHonorBanished += OnEventFieldOfHonorBanished;
        CsDungeonManager.Instance.EventFieldOfHonorExit += OnEventFieldOfHonorExit;

        // 영혼을 탐하는 자
        CsDungeonManager.Instance.EventSoulCoveterClear += OnEventSoulCoveterClear;
        CsDungeonManager.Instance.EventSoulCoveterFail += OnEventSoulCoveterFail;
        CsDungeonManager.Instance.EventSoulCoveterExit += OnEventSoulCoveterExit;
        CsDungeonManager.Instance.EventSoulCoveterBanished += OnEventSoulCoveterBanished;

        CsDungeonManager.Instance.EventSoulCoveterMatchingCancel += OnEventSoulCoveterMatchingCancel;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomBanished += OnEventSoulCoveterMatchingRoomBanished;
        CsDungeonManager.Instance.EventSoulCoveterMatchingStart += OnEventSoulCoveterMatchingStart;
        CsDungeonManager.Instance.EventSoulCoveterMatchingStatusChanged += OnEventSoulCoveterMatchingStatusChanged;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomPartyEnter += OnEventSoulCoveterMatchingRoomPartyEnter;

        //용맹의 증명
        CsDungeonManager.Instance.EventProofOfValorEnter += OnEventProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorClear += OnEventProofOfValorClear;
        CsDungeonManager.Instance.EventProofOfValorFail += OnEventProofOfValorFail;
        CsDungeonManager.Instance.EventProofOfValorExit += OnEventProofOfValorExit;
        CsDungeonManager.Instance.EventProofOfValorBanished += OnEventProofOfValorBanished;

		// 지혜의 신전
		CsDungeonManager.Instance.EventWisdomTempleExit += OnEventWisdomTempleExit;
		CsDungeonManager.Instance.EventWisdomTempleClear += OnEventWisdomTempleClear;
		CsDungeonManager.Instance.EventWisdomTempleFail += OnEventWisdomTempleFail;
		CsDungeonManager.Instance.EventWisdomTempleBanished += OnEventWisdomTempleBanished;
		CsDungeonManager.Instance.EventWisdomTempleStepCompleted += OnEventWisdomTempleStepCompleted;
		CsDungeonManager.Instance.EventWisdomTempleAbandon += OnEventWisdomTempleAbandon;

        // 정예 던전
        CsDungeonManager.Instance.EventEliteDungeonClear += OnEventEliteDungeonClear;
        CsDungeonManager.Instance.EventEliteDungeonFail += OnEventEliteDungeonFail;
        CsDungeonManager.Instance.EventEliteDungeonAbandon += OnEventEliteDungeonAbandon;
        CsDungeonManager.Instance.EventEliteDungeonBanished += OnEventEliteDungeonBanished;
        CsDungeonManager.Instance.EventEliteDungeonExit += OnEventEliteDungeonExit;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimClear += OnEventRuinsReclaimClear;
		CsDungeonManager.Instance.EventRuinsReclaimFail += OnEventRuinsReclaimFail;
		CsDungeonManager.Instance.EventRuinsReclaimExit += OnEventRuinsReclaimExit;
		CsDungeonManager.Instance.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;

		CsDungeonManager.Instance.EventRuinsReclaimMatchingCancel += OnEventRuinsReclaimMatchingCancel;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomBanished += OnEventRuinsReclaimMatchingRoomBanished;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStart += OnEventRuinsReclaimMatchingStart;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStatusChanged += OnEventRuinsReclaimMatchingStatusChanged;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomPartyEnter += OnEventRuinsReclaimMatchingRoomPartyEnter;

        // 무한 대전
        CsDungeonManager.Instance.EventInfiniteWarEnter += OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarClear += OnEventInfiniteWarClear;
        CsDungeonManager.Instance.EventInfiniteWarBanished += OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarExit += OnEventInfiniteWarExit;

        CsDungeonManager.Instance.EventInfiniteWarMatchingCancel += OnEventInfiniteWarMatchingCancel;
        CsDungeonManager.Instance.EventInfiniteWarMatchingRoomBanished += OnEventInfiniteWarMatchingRoomBanished;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStart += OnEventInfiniteWarMatchingStart;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStatusChanged += OnEventInfiniteWarMatchingStatusChanged;

		// 공포의 제단
		CsDungeonManager.Instance.EventFearAltarClear += OnEventFearAltarClear;
		CsDungeonManager.Instance.EventFearAltarFail += OnEventFearAltarFail;
		CsDungeonManager.Instance.EventFearAltarExit += OnEventFearAltarExit;
		CsDungeonManager.Instance.EventFearAltarAbandon += OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished += OnEventFearAltarBanished;

		CsDungeonManager.Instance.EventFearAltarMatchingCancel += OnEventFearAltarMatchingCancel;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomBanished += OnEventFearAltarMatchingRoomBanished;
		CsDungeonManager.Instance.EventFearAltarMatchingStart += OnEventFearAltarMatchingStart;
		CsDungeonManager.Instance.EventFearAltarMatchingStatusChanged += OnEventFearAltarMatchingStatusChanged;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomPartyEnter += OnEventFearAltarMatchingRoomPartyEnter;

		CsGameEventUIToUI.Instance.EventFearAltarHalidomAcquisition += OnEventFearAltarHalidomAcquisition;			// 성물 획득 처리
		CsDungeonManager.Instance.EventFearAltarWaveStart += OnEventFearAltarWaveStart;								// 성물 몬스터 젠 처리
		CsDungeonManager.Instance.EventFearAltarHalidomMonsterKill += OnEventFearAltarHalidomMonsterKill;			// 성물 몬스터 킬 처리
		CsDungeonManager.Instance.EventFearAltarHalidomMonsterKillFail += OnEventFearAltarHalidomMonsterKillFail;	// 성물 몬스터 킬 실패 처리

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryClear += OnEventWarMemoryClear;
        CsDungeonManager.Instance.EventWarMemoryFail += OnEventWarMemoryFail;
        CsDungeonManager.Instance.EventWarMemoryExit += OnEventWarMemoryExit;
        CsDungeonManager.Instance.EventWarMemoryAbandon += OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished += OnEventWarMemoryBanished;

        CsDungeonManager.Instance.EventWarMemoryMatchingStart += OnEventWarMemoryMatchingStart;
        CsDungeonManager.Instance.EventWarMemoryMatchingCancel += OnEventWarMemoryMatchingCancel;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomPartyEnter += OnEventWarMemoryMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomBanished += OnEventWarMemoryMatchingRoomBanished;
        CsDungeonManager.Instance.EventWarMemoryMatchingStatusChanged += OnEventWarMemoryMatchingStatusChanged;

        // 오시리스 룸
        CsDungeonManager.Instance.EventOsirisRoomEnter += OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomClear += OnEventOsirisRoomClear;
        CsDungeonManager.Instance.EventOsirisRoomFail += OnEventOsirisRoomFail;
        CsDungeonManager.Instance.EventOsirisRoomExit += OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomAbandon += OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished += OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomWaveStart += OnEventOsirisRoomWaveStart;

		// 전기퀘스트 던전
		CsDungeonManager.Instance.EventBiographyQuestDungeonClear += OnEventBiographyQuestDungeonClear;
		CsDungeonManager.Instance.EventBiographyQuestDungeonFail += OnEventBiographyQuestDungeonFail;
		CsDungeonManager.Instance.EventBiographyQuestDungeonEnter += OnEventBiographyQuestDungeonEnter;
		CsDungeonManager.Instance.EventBiographyQuestDungeonExit += OnEventBiographyQuestDungeonExit;
		CsDungeonManager.Instance.EventBiographyQuestDungeonAbandon += OnEventBiographyQuestDungeonAbandon;
		CsDungeonManager.Instance.EventBiographyQuestDungeonBanished += OnEventBiographyQuestDungeonBanished;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestMatchingStart += OnEventDragonNestMatchingStart;
        CsDungeonManager.Instance.EventDragonNestMatchingCancel += OnEventDragonNestMatchingCancel;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomPartyEnter += OnEventDragonNestMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomBanished += OnEventDragonNestMatchingRoomBanished;
        CsDungeonManager.Instance.EventDragonNestMatchingStatusChanged += OnEventDragonNestMatchingStatusChanged;

        CsDungeonManager.Instance.EventDragonNestEnter += OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestClear += OnEventDragonNestClear;
        CsDungeonManager.Instance.EventDragonNestFail += OnEventDragonNestFail;
        CsDungeonManager.Instance.EventDragonNestAbandon += OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished += OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventDragonNestExit += OnEventDragonNestExit;
        CsDungeonManager.Instance.EventDragonNestStepCompleted += OnEventDragonNestStepCompleted;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipMatchingStart += OnEventTradeShipMatchingStart;
        CsDungeonManager.Instance.EventTradeShipMatchingCancel += OnEventTradeShipMatchingCancel;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomPartyEnter += OnEventTradeShipMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomBanished += OnEventTradeShipMatchingRoomBanished;
        CsDungeonManager.Instance.EventTradeShipMatchingStatusChanged += OnEventTradeShipMatchingStatusChanged;

        CsDungeonManager.Instance.EventTradeShipEnter += OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipClear += OnEventTradeShipClear;
        CsDungeonManager.Instance.EventTradeShipFail += OnEventTradeShipFail;
        CsDungeonManager.Instance.EventTradeShipAbandon += OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished += OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipExit += OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipStepStart += OnEventTradeShipStepStart;
        CsDungeonManager.Instance.EventTradeShipAdditionalRewardExpReceive += OnEventTradeShipAdditionalRewardExpReceive;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombMatchingStart += OnEventAnkouTombMatchingStart;
        CsDungeonManager.Instance.EventAnkouTombMatchingCancel += OnEventAnkouTombMatchingCancel;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomPartyEnter += OnEventAnkouTombMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomBanished += OnEventAnkouTombMatchingRoomBanished;
        CsDungeonManager.Instance.EventAnkouTombMatchingStatusChanged += OnEventAnkouTombMatchingStatusChanged;

        CsDungeonManager.Instance.EventAnkouTombEnter += OnEventAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombClear += OnEventAnkouTombClear;
        CsDungeonManager.Instance.EventAnkouTombFail += OnEventAnkouTombFail;
        CsDungeonManager.Instance.EventAnkouTombAbandon += OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished += OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombExit += OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombWaveStart += OnEventAnkouTombWaveStart;
        CsDungeonManager.Instance.EventAnkouTombAdditionalRewardExpReceive += OnEventAnkouTombAdditionalRewardExpReceive;

        // 스토리 던전 입은 피해 받은 피해
        CsRplzSession.Instance.EventEvtHeroHit += OnEventEvtHeroHit;
        CsRplzSession.Instance.EventEvtMonsterHit += OnEventEvtMonsterHit;
        CsGameEventToUI.Instance.EventBossAppear += OnEventBossAppear;

        // 던전이 끝나고 모든 몬스터가 죽은 후 이벤트
        CsDungeonManager.Instance.EventDungeonClear += OnEventDungeonClear;
		
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_flDungeonExitDelayTime - Time.realtimeSinceStartup > 0)
            {
				if (m_textRemaining != null && ((m_trPopupDungeonClear != null && m_trPopupDungeonClear.gameObject.activeSelf) || 
												(m_trNewPopupDungeonClear != null && m_trNewPopupDungeonClear.gameObject.activeSelf) ||
												(m_trPopupRuinsReclaimClear != null && m_trPopupRuinsReclaimClear.gameObject.activeSelf)))
                {
					m_textRemaining.text = string.Format(m_strExitGuide, ((int)(m_flDungeonExitDelayTime - Time.realtimeSinceStartup)).ToString("#0"));
                }
            }

            if (m_textTime != null)
			{
				if ((CsDungeonManager.Instance.AncientRelicMatchingRemainingTime - Time.realtimeSinceStartup) > 0)
				{
					switch (CsDungeonManager.Instance.AncientRelicState)
					{
					    case EnDungeonMatchingState.MatchReady:
					        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.AncientRelicMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
					        break;

					    case EnDungeonMatchingState.MatchComplete:
					        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.AncientRelicMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
					        break;
					}
				}
				else if (CsDungeonManager.Instance.SoulCoveterMatchingRemainingTime - Time.realtimeSinceStartup > 0)
				{
					switch (CsDungeonManager.Instance.SoulCoveterMatchingState)
					{
					    case EnDungeonMatchingState.MatchReady:
					        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.SoulCoveterMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
					        break;

					    case EnDungeonMatchingState.MatchComplete:
					        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.SoulCoveterMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
					        break;
					}
				}
				else if ((CsDungeonManager.Instance.RuinsReclaimMatchingRemainingTime - Time.realtimeSinceStartup) > 0)
				{
					switch (CsDungeonManager.Instance.RuinsReclaimMatchingState)
					{
						case EnDungeonMatchingState.MatchReady:
							m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.RuinsReclaimMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
							break;

						case EnDungeonMatchingState.MatchComplete:
							m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.RuinsReclaimMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
							break;
					}
				}
				else if (CsDungeonManager.Instance.FearAltarMatchingRemainingTime - Time.realtimeSinceStartup > 0)
				{
					switch (CsDungeonManager.Instance.FearAltarMatchingState)
					{
						case EnDungeonMatchingState.MatchReady:
							m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.FearAltarMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
							break;

						case EnDungeonMatchingState.MatchComplete:
							m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.FearAltarMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
							break;
					}
				}
                else if (CsDungeonManager.Instance.DragonNestMatchingRemainingTime - Time.realtimeSinceStartup > 0)
                {
                    switch (CsDungeonManager.Instance.DragonNestMatchingState)
                    {
                        case EnDungeonMatchingState.MatchReady:
                            m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.DragonNestMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                            break;

                        case EnDungeonMatchingState.MatchComplete:
                            m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.DragonNestMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                            break;
                    }
                }
                else if (CsDungeonManager.Instance.TradeShipMatchingRemainingTime - Time.realtimeSinceStartup > 0)
                {
                    switch (CsDungeonManager.Instance.TradeShipMatchingState)
                    {
                        case EnDungeonMatchingState.MatchReady:
                            m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.TradeShipMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                            break;
                        case EnDungeonMatchingState.MatchComplete:
                            m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.TradeShipMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                            break;
                    }
                }
                else if (CsDungeonManager.Instance.AnkouTombMatchingRemainingTime - Time.realtimeSinceStartup > 0)
                {
                    switch (CsDungeonManager.Instance.AnkouTombMatchingState)
                    {
                        case EnDungeonMatchingState.MatchReady:
                            m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.AnkouTombMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                            break;

                        case EnDungeonMatchingState.MatchComplete:
                            m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.AnkouTombMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                            break;
                    }
                }
            }

            if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.InfiniteWar)
            {
                CreateDungeonStartCount(m_flDungeonStartDelayTime - Time.realtimeSinceStartup);
            }
            else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.OsirisRoom)
            {
                CreateDungeonStartCount(m_flDungeonStartDelayTime - Time.realtimeSinceStartup);
            }
            else
            {
                m_trPopupDungeonStartCount = null;
            }
            

			// 성물 유지시간이 지난 경우
			if (m_trButtonShowHalidomCollection.gameObject.activeSelf &&
				m_nHalidomDisplayDuration - Time.realtimeSinceStartup <= 0)
			{
				m_trButtonShowHalidomCollection.gameObject.SetActive(false);
			}

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventOpenPopupMatching -= OnEventOpenPopupMatching;

        CsGameEventToUI.Instance.EventHideMainUI -= OnEventHideMainUI;
        CsGameEventUIToUI.Instance.EventVisibleMainUI -= OnEventVisibleMainUI;

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonClear -= OnEventMainQuestDungeonClear;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonFail -= OnEventMainQuestDungeonFail;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;

        //스토리던전
        CsDungeonManager.Instance.EventStoryDungeonStepStart -= OnEventStoryDungeonStepStart;
        CsDungeonManager.Instance.EventStoryDungeonClear -= OnEventStoryDungeonClear;
        CsDungeonManager.Instance.EventStoryDungeonFail -= OnEventStoryDungeonFail;
        CsDungeonManager.Instance.EventStoryDungeonExit -= OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;
        CsGameEventToUI.Instance.EventClearDirectionFinish -= OnEventClearDirectionFinish;

        //경험치던전
        CsDungeonManager.Instance.EventExpDungeonClear -= OnEventExpDungeonClear;
        CsDungeonManager.Instance.EventExpDungeonExit -= OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonBanished -= OnEventExpDungeonBanished;

        //골드던전
        CsDungeonManager.Instance.EventGoldDungeonClear -= OnEventGoldDungeonClear;
        CsDungeonManager.Instance.EventGoldDungeonFail -= OnEventGoldDungeonFail;
        CsDungeonManager.Instance.EventGoldDungeonExit -= OnEventGoldDungeonExit;
        CsDungeonManager.Instance.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;

        //고대 유물의 방
        CsDungeonManager.Instance.EventArtifactRoomClear -= OnEventArtifactRoomClear;
        CsDungeonManager.Instance.EventArtifactRoomFail -= OnEventArtifactRoomFail;
        CsDungeonManager.Instance.EventArtifactRoomExit -= OnEventArtifactRoomExit;
        CsDungeonManager.Instance.EventArtifactRoomBanished -= OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomBanishedForNextFloorChallenge -= EventArtifactRoomBanishedForNextFloorChallenge;
        CsDungeonManager.Instance.EventArtifactRoomNextFloorChallenge -= OnEventArtifactRoomNextFloorChallenge;

        //고대인의 유적
        CsDungeonManager.Instance.EventAncientRelicClear -= OnEventAncientRelicClear;
        CsDungeonManager.Instance.EventAncientRelicFail -= OnEventAncientRelicFail;
        CsDungeonManager.Instance.EventAncientRelicExit -= OnEventAncientRelicExit;
        CsDungeonManager.Instance.EventAncientRelicBanished -= OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicStepCompleted -= OnEventAncientRelicStepCompleted;

        CsDungeonManager.Instance.EventAncientRelicMatchingCancel -= OnEventAncientRelicMatchingCancel;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomBanished -= OnEventAncientRelicMatchingRoomBanished;
        CsDungeonManager.Instance.EventAncientRelicMatchingStart -= OnEventAncientRelicMatchingStart;
        CsDungeonManager.Instance.EventAncientRelicMatchingStatusChanged -= OnEventAncientRelicMatchingStatusChanged;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomPartyEnter -= OnEventAncientRelicMatchingRoomPartyEnter;

        //검투 대회
        CsDungeonManager.Instance.EventFieldOfHonorClear -= OnEventFieldOfHonorClear;
        CsDungeonManager.Instance.EventFieldOfHonorFail -= OnEventFieldOfHonorFail;
        CsDungeonManager.Instance.EventFieldOfHonorBanished -= OnEventFieldOfHonorBanished;
        CsDungeonManager.Instance.EventFieldOfHonorExit -= OnEventFieldOfHonorExit;

        // 영혼을 탐하는 자
        CsDungeonManager.Instance.EventSoulCoveterClear -= OnEventSoulCoveterClear;
        CsDungeonManager.Instance.EventSoulCoveterFail -= OnEventSoulCoveterFail;
        CsDungeonManager.Instance.EventSoulCoveterExit -= OnEventSoulCoveterExit;
        CsDungeonManager.Instance.EventSoulCoveterBanished -= OnEventSoulCoveterBanished;

        CsDungeonManager.Instance.EventSoulCoveterMatchingCancel -= OnEventSoulCoveterMatchingCancel;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomBanished -= OnEventSoulCoveterMatchingRoomBanished;
        CsDungeonManager.Instance.EventSoulCoveterMatchingStart -= OnEventSoulCoveterMatchingStart;
        CsDungeonManager.Instance.EventSoulCoveterMatchingStatusChanged -= OnEventSoulCoveterMatchingStatusChanged;
        CsDungeonManager.Instance.EventSoulCoveterMatchingRoomPartyEnter -= OnEventSoulCoveterMatchingRoomPartyEnter;

        //용맹의 증명
        CsDungeonManager.Instance.EventProofOfValorEnter -= OnEventProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorClear -= OnEventProofOfValorClear;
        CsDungeonManager.Instance.EventProofOfValorFail -= OnEventProofOfValorFail;
        CsDungeonManager.Instance.EventProofOfValorExit -= OnEventProofOfValorExit;
        CsDungeonManager.Instance.EventProofOfValorBanished -= OnEventProofOfValorBanished;

        // 지혜의 신전
        CsDungeonManager.Instance.EventWisdomTempleExit -= OnEventWisdomTempleExit;
        CsDungeonManager.Instance.EventWisdomTempleClear -= OnEventWisdomTempleClear;
        CsDungeonManager.Instance.EventWisdomTempleFail -= OnEventWisdomTempleFail;
        CsDungeonManager.Instance.EventWisdomTempleBanished -= OnEventWisdomTempleBanished;
        CsDungeonManager.Instance.EventWisdomTempleStepCompleted -= OnEventWisdomTempleStepCompleted;
        CsDungeonManager.Instance.EventWisdomTempleAbandon -= OnEventWisdomTempleAbandon;

        // 정예 던전
        CsDungeonManager.Instance.EventEliteDungeonClear -= OnEventEliteDungeonClear;
        CsDungeonManager.Instance.EventEliteDungeonFail -= OnEventEliteDungeonFail;
        CsDungeonManager.Instance.EventEliteDungeonAbandon -= OnEventEliteDungeonAbandon;
        CsDungeonManager.Instance.EventEliteDungeonBanished -= OnEventEliteDungeonBanished;
        CsDungeonManager.Instance.EventEliteDungeonExit -= OnEventEliteDungeonExit;

        // 유적 탈환
        CsDungeonManager.Instance.EventRuinsReclaimClear -= OnEventRuinsReclaimClear;
        CsDungeonManager.Instance.EventRuinsReclaimFail -= OnEventRuinsReclaimFail;
        CsDungeonManager.Instance.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;
        CsDungeonManager.Instance.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;

        CsDungeonManager.Instance.EventRuinsReclaimMatchingCancel -= OnEventRuinsReclaimMatchingCancel;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomBanished -= OnEventRuinsReclaimMatchingRoomBanished;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingStart -= OnEventRuinsReclaimMatchingStart;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingStatusChanged -= OnEventRuinsReclaimMatchingStatusChanged;
        CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomPartyEnter -= OnEventRuinsReclaimMatchingRoomPartyEnter;

        // 무한 대전
        CsDungeonManager.Instance.EventInfiniteWarEnter -= OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarClear -= OnEventInfiniteWarClear;
        CsDungeonManager.Instance.EventInfiniteWarBanished -= OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarExit -= OnEventInfiniteWarExit;

        CsDungeonManager.Instance.EventInfiniteWarMatchingCancel -= OnEventInfiniteWarMatchingCancel;
        CsDungeonManager.Instance.EventInfiniteWarMatchingRoomBanished -= OnEventInfiniteWarMatchingRoomBanished;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStart -= OnEventInfiniteWarMatchingStart;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStatusChanged -= OnEventInfiniteWarMatchingStatusChanged;

        // 공포의 제단
        CsDungeonManager.Instance.EventFearAltarClear -= OnEventFearAltarClear;
        CsDungeonManager.Instance.EventFearAltarFail -= OnEventFearAltarFail;
        CsDungeonManager.Instance.EventFearAltarExit -= OnEventFearAltarExit;
        CsDungeonManager.Instance.EventFearAltarAbandon -= OnEventFearAltarAbandon;
        CsDungeonManager.Instance.EventFearAltarBanished -= OnEventFearAltarBanished;

        CsDungeonManager.Instance.EventFearAltarMatchingCancel -= OnEventFearAltarMatchingCancel;
        CsDungeonManager.Instance.EventFearAltarMatchingRoomBanished -= OnEventFearAltarMatchingRoomBanished;
        CsDungeonManager.Instance.EventFearAltarMatchingStart -= OnEventFearAltarMatchingStart;
        CsDungeonManager.Instance.EventFearAltarMatchingStatusChanged -= OnEventFearAltarMatchingStatusChanged;
        CsDungeonManager.Instance.EventFearAltarMatchingRoomPartyEnter -= OnEventFearAltarMatchingRoomPartyEnter;

        CsGameEventUIToUI.Instance.EventFearAltarHalidomAcquisition -= OnEventFearAltarHalidomAcquisition;			// 성물 획득 처리
        CsDungeonManager.Instance.EventFearAltarWaveStart -= OnEventFearAltarWaveStart;								// 성물 몬스터 젠 처리
        CsDungeonManager.Instance.EventFearAltarHalidomMonsterKill -= OnEventFearAltarHalidomMonsterKill;			// 성물 몬스터 킬 처리
        CsDungeonManager.Instance.EventFearAltarHalidomMonsterKillFail -= OnEventFearAltarHalidomMonsterKillFail;	// 성물 몬스터 킬 실패 처리

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryClear -= OnEventWarMemoryClear;
        CsDungeonManager.Instance.EventWarMemoryFail -= OnEventWarMemoryFail;
        CsDungeonManager.Instance.EventWarMemoryExit -= OnEventWarMemoryExit;
        CsDungeonManager.Instance.EventWarMemoryAbandon -= OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished -= OnEventWarMemoryBanished;

        CsDungeonManager.Instance.EventWarMemoryMatchingStart -= OnEventWarMemoryMatchingStart;
        CsDungeonManager.Instance.EventWarMemoryMatchingCancel -= OnEventWarMemoryMatchingCancel;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomPartyEnter -= OnEventWarMemoryMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomBanished -= OnEventWarMemoryMatchingRoomBanished;
        CsDungeonManager.Instance.EventWarMemoryMatchingStatusChanged -= OnEventWarMemoryMatchingStatusChanged;

        // 오시리스 룸
        CsDungeonManager.Instance.EventOsirisRoomEnter -= OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomClear -= OnEventOsirisRoomClear;
        CsDungeonManager.Instance.EventOsirisRoomFail -= OnEventOsirisRoomFail;
        CsDungeonManager.Instance.EventOsirisRoomExit -= OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomAbandon -= OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished -= OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomWaveStart -= OnEventOsirisRoomWaveStart;

        // 전기퀘스트 던전
        CsDungeonManager.Instance.EventBiographyQuestDungeonClear -= OnEventBiographyQuestDungeonClear;
        CsDungeonManager.Instance.EventBiographyQuestDungeonFail -= OnEventBiographyQuestDungeonFail;
        CsDungeonManager.Instance.EventBiographyQuestDungeonEnter -= OnEventBiographyQuestDungeonEnter;
        CsDungeonManager.Instance.EventBiographyQuestDungeonExit -= OnEventBiographyQuestDungeonExit;
        CsDungeonManager.Instance.EventBiographyQuestDungeonAbandon -= OnEventBiographyQuestDungeonAbandon;
        CsDungeonManager.Instance.EventBiographyQuestDungeonBanished -= OnEventBiographyQuestDungeonBanished;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestMatchingStart -= OnEventDragonNestMatchingStart;
        CsDungeonManager.Instance.EventDragonNestMatchingCancel -= OnEventDragonNestMatchingCancel;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomPartyEnter -= OnEventDragonNestMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomBanished -= OnEventDragonNestMatchingRoomBanished;
        CsDungeonManager.Instance.EventDragonNestMatchingStatusChanged -= OnEventDragonNestMatchingStatusChanged;

        CsDungeonManager.Instance.EventDragonNestEnter -= OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestClear -= OnEventDragonNestClear;
        CsDungeonManager.Instance.EventDragonNestFail -= OnEventDragonNestFail;
        CsDungeonManager.Instance.EventDragonNestAbandon -= OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished -= OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventDragonNestExit -= OnEventDragonNestExit;
        CsDungeonManager.Instance.EventDragonNestStepCompleted -= OnEventDragonNestStepCompleted;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipMatchingStart -= OnEventTradeShipMatchingStart;
        CsDungeonManager.Instance.EventTradeShipMatchingCancel -= OnEventTradeShipMatchingCancel;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomPartyEnter -= OnEventTradeShipMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomBanished -= OnEventTradeShipMatchingRoomBanished;
        CsDungeonManager.Instance.EventTradeShipMatchingStatusChanged -= OnEventTradeShipMatchingStatusChanged;

        CsDungeonManager.Instance.EventTradeShipEnter -= OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipClear -= OnEventTradeShipClear;
        CsDungeonManager.Instance.EventTradeShipFail -= OnEventTradeShipFail;
        CsDungeonManager.Instance.EventTradeShipAbandon -= OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished -= OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipExit -= OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipStepStart -= OnEventTradeShipStepStart;
        CsDungeonManager.Instance.EventTradeShipAdditionalRewardExpReceive -= OnEventTradeShipAdditionalRewardExpReceive;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombMatchingStart -= OnEventAnkouTombMatchingStart;
        CsDungeonManager.Instance.EventAnkouTombMatchingCancel -= OnEventAnkouTombMatchingCancel;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomPartyEnter -= OnEventAnkouTombMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomBanished -= OnEventAnkouTombMatchingRoomBanished;
        CsDungeonManager.Instance.EventAnkouTombMatchingStatusChanged -= OnEventAnkouTombMatchingStatusChanged;

        CsDungeonManager.Instance.EventAnkouTombEnter -= OnEventAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombClear -= OnEventAnkouTombClear;
        CsDungeonManager.Instance.EventAnkouTombFail -= OnEventAnkouTombFail;
        CsDungeonManager.Instance.EventAnkouTombAbandon -= OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished -= OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombExit -= OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombWaveStart -= OnEventAnkouTombWaveStart;
        CsDungeonManager.Instance.EventAnkouTombAdditionalRewardExpReceive -= OnEventAnkouTombAdditionalRewardExpReceive;

        // 스토리 던전 입은 피해 받은 피해
        CsRplzSession.Instance.EventEvtHeroHit -= OnEventEvtHeroHit;
        CsRplzSession.Instance.EventEvtMonsterHit -= OnEventEvtMonsterHit;
        CsGameEventToUI.Instance.EventBossAppear -= OnEventBossAppear;

        // 던전이 끝나고 모든 몬스터가 죽은 후 이벤트
        CsDungeonManager.Instance.EventDungeonClear -= OnEventDungeonClear;
    }

    #region EventHandler
	//---------------------------------------------------------------------------------------------------
	void OnEventHideMainUI(bool bIsOn)
	{
		m_trButtons.gameObject.SetActive(!bIsOn);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventVisibleMainUI(bool bIsOn)
	{
		m_trButtons.gameObject.SetActive(bIsOn);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupMatching()
    {
        OpenPopupMatching();
    }

    // 정예 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonClear()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.EliteDungeon.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.EliteDungeon.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.Elite);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonAbandon(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonBanished(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonExit(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    // 영혼을 탐하는 자
    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterClear(PDItemBooty[] pDItemBooty)
    {
        m_pDItemBooty = pDItemBooty;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.SoulCoveter.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.SoulCoveter.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.SoulCoveter);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterExit(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterBanished(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingStart()
    {
        m_enDungeonMatching = EnDungeon.SoulCoveter;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterMatchingRoomPartyEnter()
    {
        m_enDungeonMatching = EnDungeon.SoulCoveter;
        OpenPopupMatching();
        UpdateMatchingState();
    }

    //용맹의 증명
    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorEnter(Guid guidPlaceInstanceId, PDVector3 pdVec3Position, float flRotationY, PDMonsterInstance[] arrPdMonsterInstance)
    {
        m_nPreCreatureCardId = CsGameData.Instance.ProofOfValor.CreatureCardId;
        m_nPreBossMonsterArrangeId = CsGameData.Instance.ProofOfValor.BossMonsterArrangeId;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorClear(bool bLevelUp, long lAcquiredExp)
    {
        m_lExpDungeonRewardExp = lAcquiredExp;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.ProofOfValor.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorFail(bool bLevelUp, long lAcquiredExp)
    {
        m_lExpDungeonRewardExp = lAcquiredExp;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.ProofOfValor.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.ProofOfValor);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorExit(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorBanished(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

	// 지혜의 신전
	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleExit(int nPreviousContinentId)
	{
		if (m_listWisdomTempleStepReward != null)
		{
			m_listWisdomTempleStepReward.Clear();
			m_listWisdomTempleStepReward = null;
		}

		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleClear()
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.WisdomTemple.ExitDelayTime + Time.realtimeSinceStartup;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleFail()
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.WisdomTemple.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.WisdomTemple);
		CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBanished(int nPreviousContinentId)
	{
		if (m_listWisdomTempleStepReward != null)
		{
			m_listWisdomTempleStepReward.Clear();
			m_listWisdomTempleStepReward = null;
		}

		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepCompleted (bool bLevelUp, long lAcquiredExp, PDItemBooty pdItemBooty)
	{
		if (m_listWisdomTempleStepReward == null)
		{
			m_listWisdomTempleStepReward = new List<PDItemBooty>();
		}

		PDItemBooty itemBooty = m_listWisdomTempleStepReward.Find(item => item.id == pdItemBooty.id);

		if (itemBooty == null)
		{
			m_listWisdomTempleStepReward.Add(pdItemBooty);
		}
		else
		{
			itemBooty.count += pdItemBooty.count;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleAbandon(int nPreviousContinentId)
	{
		if (m_listWisdomTempleStepReward != null)
		{
			m_listWisdomTempleStepReward.Clear();
			m_listWisdomTempleStepReward = null;
		}
	}

    //검투 대회
    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorClear(bool bLevelUp, long lAcquiredExp, int nHonorPoint)
    {
        m_nGetHonorPoint = nHonorPoint;
        m_lExpDungeonRewardExp = lAcquiredExp;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.FieldOfHonor.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorFail(bool bLevelUp, long lAcquiredExp, int nHonorPoint)
    {
        m_nGetHonorPoint = nHonorPoint;
        m_lExpDungeonRewardExp = lAcquiredExp;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.FieldOfHonor.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.FieldOfHonor);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorBanished(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorExit(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //고대인의 유적
    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicClear()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.AncientRelic.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.AncientRelic.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.AncientRelic);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicExit(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicBanished(int nContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicStepCompleted(PDItemBooty[] arrPDItemBooty)
    {
        if (m_pDItemBooty == null)
        {
            m_pDItemBooty = arrPDItemBooty;
        }
        else
        {
            List<PDItemBooty> listPDItemBootyCombine = new List<PDItemBooty>(m_pDItemBooty);
            listPDItemBootyCombine.AddRange(arrPDItemBooty);

            m_pDItemBooty = listPDItemBootyCombine.ToArray();

            listPDItemBootyCombine.Clear();
            listPDItemBootyCombine = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingStart()
    {
        m_pDItemBooty = null;
        m_enDungeonMatching = EnDungeon.AncientRelic;

        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingRoomPartyEnter()
    {
        m_pDItemBooty = null;
        m_enDungeonMatching = EnDungeon.AncientRelic;

        OpenPopupMatching();
        UpdateMatchingState();
    }

    //고대 유물의 방
    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomClear(PDItemBooty pDItemBooty)
    {
        m_pDItemBootyArtifactRoom = pDItemBooty;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.ArtifactRoom.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomFail()
    {
        Debug.Log("OnEventArtifactRoomFail");
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.ArtifactRoom.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.ArtifactRoom);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomExit(int nPreviousContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomBanished(int nPreviousContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void EventArtifactRoomBanishedForNextFloorChallenge()
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomNextFloorChallenge()
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //골드던전
    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonBanished(int nPreviousContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonExit(int nPreviousContinentId)
    {
        m_trPopupDungeonClear.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonClear(long lRewardGold)
    {
        m_lGoldDungeonRewardGold = lRewardGold;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.GoldDungeon.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.GoldDungeon.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.Gold);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //경험치던전
    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonBanished(int nPreviousContinentId)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonExit(int nPreviousContinentId)
    {
        m_trPopupDungeonClear.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonClear(bool bLevelUp, long lExp)
    {
        m_lExpDungeonRewardExp = lExp;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.ExpDungeon.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //스토리던전

    //---------------------------------------------------------------------------------------------------
    void OnEventClearDirectionFinish(EnDungeonPlay enDungeonPlay)
    {
        OpenNewPopupDungeonClear(true, enDungeonPlay);

        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonExit(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonStepStart(PDStoryDungeonMonsterInstance[] pDStoryDungeonMonsterInstance)
    {
        if (CsDungeonManager.Instance.StoryDungeonStep.Step == 1)
        {
            m_flDungeonCelarTime = Time.realtimeSinceStartup;
            m_nHit = 0;
            m_nDamaged = 0;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonClear(PDItemBooty[] pDItemBooty)
    {
        m_pDItemBooty = pDItemBooty;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.StoryDungeon.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.StoryDungeon.ExitDelayTime + Time.realtimeSinceStartup;
        OpenNewPopupDungeonClear(false, EnDungeonPlay.Story);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //메인퀘스트 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonClear()
    {
        m_flDungeonExitDelayTime = CsMainQuestDungeonManager.Instance.MainQuestDungeon.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(true, EnDungeonPlay.MainQuest);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonFail()
    {
        m_flDungeonExitDelayTime = CsMainQuestDungeonManager.Instance.MainQuestDungeon.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.MainQuest);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonExit(int nContinentId, bool bChangeScene)
    {
        m_trPopupDungeonClear.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonBanished(int nContinentId, bool bChangeScene)
    {
        if (m_trPopupDungeonClear != null)
        {
            m_trPopupDungeonClear.gameObject.SetActive(false);
        }
    }

	// 유적 탈환
	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimClear(PDItemBooty[] aPDItemBooty, PDItemBooty randomBooty,
		Guid monsterTerminatorHeroId, string monsterTerminatorHeroName, PDItemBooty monsterTerminatorBooty,
		Guid ultimateAttackKingHeroId, string ultimateAttackKingHeroName, PDItemBooty ultimateAttackKingBooty,
		Guid partyVolunteerHeroId, string partyVolunteerHeroName, PDItemBooty partyVolunteerBooty)
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.RuinsReclaim.ExitDelayTime + Time.realtimeSinceStartup;

        OpenPopupRuinsReclaimDungeonClear(aPDItemBooty, randomBooty, monsterTerminatorHeroName, ultimateAttackKingHeroName, partyVolunteerHeroName);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimFail()
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.RuinsReclaim.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.RuinsReclaim);
		CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nPreviousContinentId)
	{
		if (m_trPopupRuinsReclaimClear != null)
		{
			Destroy(m_trPopupRuinsReclaimClear.gameObject);
		}

		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
		}

		CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nPreviousContinentId)
	{
		if (m_trPopupRuinsReclaimClear != null)
		{
			Destroy(m_trPopupRuinsReclaimClear.gameObject);
		}

		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
		}

		CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingCancel()
	{
		m_enDungeonMatching = EnDungeon.None;
		ClosePopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingRoomBanished()
	{
		m_enDungeonMatching = EnDungeon.None;
		ClosePopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingStart()
	{
		m_enDungeonMatching = EnDungeon.RuinsReclaim;
		OpenPopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingStatusChanged()
	{
		UpdateMatchingState();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingRoomPartyEnter()
	{
		m_enDungeonMatching = EnDungeon.RuinsReclaim;
		OpenPopupMatching();
	}

    #region InfiniteWar

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pDHeroes, ClientCommon.PDMonsterInstance[] pDMonsterInstance, ClientCommon.PDInfiniteWarBuffBoxInstance[] pDInfiniteWarBuffBoxInstance)
    {
        m_flDungeonStartDelayTime = CsDungeonManager.Instance.MultiDungeonRemainingStartTime + Time.realtimeSinceStartup;
    }
    
    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarClear(PDInfiniteWarRanking[] aPDInfiniteWarRanking, PDItemBooty[] aPDItemBooty, PDItemBooty[] aPDItemBootyRanking)
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.InfiniteWar.ExitDelayTime + Time.realtimeSinceStartup;

        m_pDItemBooty = aPDItemBooty;
        m_pDItemBooty = aPDItemBootyRanking;
        m_aPDInfiniteWarRanking = aPDInfiniteWarRanking;

        OpenNewPopupDungeonClear(true, EnDungeonPlay.InfiniteWar);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBanished(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarExit(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingRoomPartyEnter()
    {
        m_enDungeonMatching = EnDungeon.InfiniteWar;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingStart()
    {
        m_enDungeonMatching = EnDungeon.InfiniteWar;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    #endregion InfiniteWar

	// 공포의 제단
	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarClear(long lAcquiredExp, PDFearAltarHero[] aPDFearAltarHero, bool bLevelUp)
	{
		m_lExpDungeonRewardExp = lAcquiredExp;
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.FearAltar.ExitDelayTime + Time.realtimeSinceStartup;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarFail()
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.FearAltar.ExitDelayTime + Time.realtimeSinceStartup;
        OpenPopupDungeonClear(false, EnDungeonPlay.FearAltar);
		CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarAbandon(int nPreviousContinentId)
	{
		// 성물 조회 버튼 제거
		m_trButtonShowHalidomCollection.gameObject.SetActive(false);

		// 타겟 버튼 제거
		m_trButtonTargetHalidomMonster.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nPreviousContinentId)
	{
		// 성물 조회 버튼 제거
		m_trButtonShowHalidomCollection.gameObject.SetActive(false);

		// 타겟 버튼 제거
		m_trButtonTargetHalidomMonster.gameObject.SetActive(false);

		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarExit(int nPreviousContinentId)
	{
		// 성물 조회 버튼 제거
		m_trButtonShowHalidomCollection.gameObject.SetActive(false);

		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingCancel()
	{
		m_enDungeonMatching = EnDungeon.None;
		ClosePopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingRoomBanished()
	{
		m_enDungeonMatching = EnDungeon.None;
		ClosePopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingStart()
	{
		m_enDungeonMatching = EnDungeon.FearAltar;
		OpenPopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingStatusChanged()
	{
		UpdateMatchingState();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingRoomPartyEnter()
	{
		m_enDungeonMatching = EnDungeon.FearAltar;
		OpenPopupMatching();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarHalidomAcquisition(int nHalidomId)
	{
		var csFearAltarHalidom = CsGameData.Instance.FearAltar.FearAltarHalidomList.Find(halidom => halidom.HalidomId == nHalidomId);

		if (csFearAltarHalidom != null)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A116_TXT_00009"), csFearAltarHalidom.FearAltarHalidomLevel.HalidomLevel.ToString(), csFearAltarHalidom.FearAltarHalidomElemental.Name));

			// 성물 조회 버튼 표시
			Image imageIcon = m_trButtonShowHalidomCollection.Find("ImageIcon").GetComponent<Image>();
			imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupHalidomCollection/" + csFearAltarHalidom.ImageName);

			m_nHalidomDisplayDuration = CsDungeonManager.Instance.FearAltar.HalidomDisplayDuration + Time.realtimeSinceStartup;
			m_trButtonShowHalidomCollection.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarWaveStart(PDFearAltarMonsterInstance[] aPDFearAltarMonsterInstance, PDFearAltarHalidomMonsterInstance pdFearAltarHalidomMonsterInstance)
	{
		// 성물 몬스터 젠
		if (pdFearAltarHalidomMonsterInstance != null)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsDungeonManager.Instance.FearAltar.HalidomMonsterSpawnText, CsDungeonManager.Instance.FearAltar.HalidomMonsterLifetime));

			// 타겟 버튼 표시
			Text textName = m_trButtonTargetHalidomMonster.Find("TextName").GetComponent<Text>();
			
			var csMonsterInfo = CsGameData.Instance.GetMonsterInfo(pdFearAltarHalidomMonsterInstance.monsterId);

			if (csMonsterInfo != null)
			{
				textName.text = csMonsterInfo.Name;
			}

			m_trButtonTargetHalidomMonster.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarHalidomMonsterKill()
	{
		// 타겟 버튼 제거
		m_trButtonTargetHalidomMonster.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarHalidomMonsterKillFail()
	{
		// 타겟 버튼 제거
		m_trButtonTargetHalidomMonster.gameObject.SetActive(false);
	}

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryClear(PDWarMemoryRanking[] aPDWarMemoryRanking, PDItemBooty[] aPDItemBooty, long lAcquiredExp, bool bLevelUp)
    {
        m_aPDWarMemoryRanking = aPDWarMemoryRanking;
        m_pDItemBooty = aPDItemBooty;

        m_lExpDungeonRewardExp = lAcquiredExp;
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.WarMemory.ExitDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.WarMemory.ExitDelayTime + Time.realtimeSinceStartup;

        OpenNewPopupDungeonClear(false, EnDungeonPlay.WarMemory);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryExit(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryAbandon(int nPreviousContinentId)
    {
 
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryBanished(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingStart()
    {
        m_enDungeonMatching = EnDungeon.WarMemory;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingRoomPartyEnter()
    {
        m_enDungeonMatching = EnDungeon.WarMemory;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    #endregion WarMemory

    #region OsirisRoom

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomEnter(Guid placeInstanceId, PDVector3 position, float flRotationY)
    {
        m_flDungeonStartDelayTime = CsDungeonManager.Instance.OsirisRoom.StartDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomClear()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.OsirisRoom.ExitDelayTime + Time.realtimeSinceStartup;
        //OpenNewPopupDungeonClear(true, EnDungeon.OsirisRoom);
        //CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.OsirisRoom.ExitDelayTime + Time.realtimeSinceStartup;

		OpenNewPopupDungeonClear(true, EnDungeonPlay.OsirisRoom);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomExit(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomAbandon(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomBanished(int nPreviousContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomWaveStart()
    {
        if (CsDungeonManager.Instance.OsirisRoomDifficultyWave != null && CsDungeonManager.Instance.OsirisRoomDifficultyWave.WaveNo == 1)
        {
            m_flDungeonCelarTime = Time.realtimeSinceStartup;
            m_nHit = 0;
            m_nDamaged = 0;
        }
    }

    #endregion OsirisRoom

	#region BiographyQuestDungeon

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonClear()
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.BiographyQuestDungeon.ExitDelayTime + Time.realtimeSinceStartup;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonFail()
	{
		m_flDungeonExitDelayTime = CsDungeonManager.Instance.BiographyQuestDungeon.ExitDelayTime + Time.realtimeSinceStartup;

        OpenPopupDungeonClear(false, EnDungeonPlay.Biography);
		CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonEnter(Guid guidPlaceInstanceId, PDVector3 vtPosition, float flRotationY)
	{
		m_flDungeonStartDelayTime = CsDungeonManager.Instance.BiographyQuestDungeon.StartDelayTime + Time.realtimeSinceStartup;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonExit(int nPrevContinentId)
	{
		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonAbandon(int nPrevContinentId)
	{
		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonBanished(int nPrevContinentId)
	{
		if (m_trPopupDungeonClear != null)
		{
			m_trPopupDungeonClear.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------

	#endregion BiographyQuestDungeon

    #region DragonNest

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingStart()
    {
        m_enDungeonMatching = EnDungeon.DragonNest;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingRoomPartyEnter()
    {
        m_enDungeonMatching = EnDungeon.DragonNest;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHero, PDMonsterInstance[] aMonsterInstance, Guid[] aTrapHeros)
    {
        m_flDungeonStartDelayTime = CsDungeonManager.Instance.DragonNest.StartDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestClear(PDSimpleHero[] arrPDSimpleHero)
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.DragonNest.ExitDelayTime + Time.realtimeSinceStartup;
        m_arrPDHero = arrPDSimpleHero;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestFail()
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.DragonNest.ExitDelayTime + Time.realtimeSinceStartup;

        OpenNewPopupDungeonClear(false, EnDungeonPlay.DragonNest);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestAbandon(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestBanished(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestExit(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    void OnEventDragonNestStepCompleted(PDItemBooty[] arrPDItemBooty)
    {
        if (m_listDragonNestStepReward == null)
        {
            m_listDragonNestStepReward = new List<PDItemBooty>();
        }

        for (int i = 0; i < arrPDItemBooty.Length; i++)
        {
            PDItemBooty pdItemBooty = arrPDItemBooty[i];
            PDItemBooty itemBooty = m_listDragonNestStepReward.Find(item => item.id == pdItemBooty.id);

            if (itemBooty == null)
            {
                m_listDragonNestStepReward.Add(pdItemBooty);
            }
            else
            {
                itemBooty.count += pdItemBooty.count;
            }
        }
    }

    #endregion DragonNest

    #region TradeShip

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingStart()
    {
        m_enDungeonMatching = EnDungeon.TradeShip;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingRoomPartyEnter()
    {
        m_enDungeonMatching = EnDungeon.TradeShip;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        m_nDifficulty = nDifficulty;

        m_flDungeonStartDelayTime = CsDungeonManager.Instance.TradeShip.StartDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipClear(bool bLevelUp, long lAcquiredExp, PDItemBooty aPDItemBooty)
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.TradeShip.ExitDelayTime + Time.realtimeSinceStartup;

        m_lExpDungeonRewardExp = lAcquiredExp;

        m_pDItemBooty = null;
        
        List<PDItemBooty> listPDItemBooty = new List<PDItemBooty>();
        listPDItemBooty.Add(aPDItemBooty);

        m_pDItemBooty = listPDItemBooty.ToArray();

        listPDItemBooty.Clear();
        listPDItemBooty = null;

        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipFail(bool bLevelUp, long lAcquiredExp)
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.TradeShip.ExitDelayTime + Time.realtimeSinceStartup;
        m_lExpDungeonRewardExp = lAcquiredExp;

        OpenNewPopupDungeonClear(false, EnDungeonPlay.TradeShip);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipAbandon(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipBanished(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipExit(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipStepStart(PDTradeShipMonsterInstance[] apMonInst, PDTradeShipAdditionalMonsterInstance[] apAddMonInst, PDTradeShipObjectInstance[] apObjInst)
    {
        if (m_listTradeShipObjectInstance == null)
        {
            m_listTradeShipObjectInstance = new List<PDTradeShipObjectInstance>();
        }
        else
        {
            m_listTradeShipObjectInstance.Clear();
        }

        if (apObjInst.Length > 0)
        {
            m_listTradeShipObjectInstance.AddRange(apObjInst);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipAdditionalRewardExpReceive(bool bLevelUp, long lAcquiredExp)
    {
        m_lExpDungeonRewardExp += lAcquiredExp;
        UpdatePopupDungeonClear();
        RewardToggleInteractable(false);
    }

    #endregion TradeShip

    #region AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingStart()
    {
        m_enDungeonMatching = EnDungeon.AnkouTomb;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingCancel()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingRoomPartyEnter(int nDifficulty)
    {
        m_enDungeonMatching = EnDungeon.AnkouTomb;
        OpenPopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingRoomBanished()
    {
        m_enDungeonMatching = EnDungeon.None;
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingStatusChanged()
    {
        UpdateMatchingState();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        m_nDifficulty = nDifficulty;

        m_flDungeonStartDelayTime = CsDungeonManager.Instance.AnkouTomb.StartDelayTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombClear(bool bLevelUp, long lAcquiredExp, PDItemBooty aPDItemBooty)
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.AnkouTomb.ExitDelayTime + Time.realtimeSinceStartup;

        m_lExpDungeonRewardExp = lAcquiredExp;

        m_pDItemBooty = null;

        List<PDItemBooty> listPDItemBooty = new List<PDItemBooty>();
        listPDItemBooty.Add(aPDItemBooty);

        m_pDItemBooty = listPDItemBooty.ToArray();

        listPDItemBooty.Clear();
        listPDItemBooty = null;

        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombFail(bool bLevelUp, long lAcquiredExp)
    {
        m_flDungeonExitDelayTime = CsDungeonManager.Instance.AnkouTomb.ExitDelayTime + Time.realtimeSinceStartup;
        m_lExpDungeonRewardExp = lAcquiredExp;

        OpenNewPopupDungeonClear(false, EnDungeonPlay.AnkouTomb);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombAbandon(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombBanished(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombExit(int nPrevContinentId)
    {
        if (m_trNewPopupDungeonClear != null)
        {
            m_trNewPopupDungeonClear.gameObject.SetActive(false);
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombWaveStart(PDAnkouTombMonsterInstance[] arrAnkouTombMonsterInstance, int nWaveNo)
    {
        if (nWaveNo == 1)
        {
            m_flDungeonCelarTime = Time.realtimeSinceStartup;

            m_nHit = 0;
            m_nDamaged = 0;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombAdditionalRewardExpReceive(bool bLevelUp, long lAcquiredExp)
    {
        m_lExpDungeonRewardExp += lAcquiredExp;
        UpdatePopupDungeonClear();
        RewardToggleInteractable(false);
    }

    #endregion AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseMatchingPopup()
    {
        ClosePopupMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMatchingCancel()
    {
        switch (m_enDungeonMatching)
        {
            case EnDungeon.AncientRelic:
                CsDungeonManager.Instance.SendAncientRelicMatchingCancel();
                break;

            case EnDungeon.SoulCoveter:
                CsDungeonManager.Instance.SendSoulCoveterMatchingCancel();
                break;

			case EnDungeon.RuinsReclaim:
				CsDungeonManager.Instance.SendRuinsReclaimMatchingCancel();
				break;

            case EnDungeon.InfiniteWar:
                CsDungeonManager.Instance.SendInfiniteWarMatchingCancel();
                break;

			case EnDungeon.FearAltar:
				CsDungeonManager.Instance.SendFearAltarMatchingCancel();
				break;

            case EnDungeon.WarMemory:
                CsDungeonManager.Instance.SendWarMemoryMatchingCancel();
                break;

            case EnDungeon.DragonNest:
                CsDungeonManager.Instance.SendDragonNestMatchingCancel();
                break;

            case EnDungeon.TradeShip:

                CsDungeonManager.Instance.SendTradeShipMatchingCancel();

                break;

            case EnDungeon.AnkouTomb:

                CsDungeonManager.Instance.SendAnkouTombMatchingCancel();

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroHit(SEBHeroHitEventBody eventBody)
    {
        m_nDamaged += eventBody.hitResult.hpDamage;
    }

    IEnumerator m_iEnumeratorToastAttackTradeShipObject = null;

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtMonsterHit(SEBMonsterHitEventBody eventBody)
    {
        m_nHit += eventBody.hitResult.hpDamage;
        
        if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.TradeShip)
        {
            PDTradeShipObjectInstance pDTradeShipObjectInstance = m_listTradeShipObjectInstance.Find(a => a.instanceId == eventBody.monsterInstanceId);

            if (pDTradeShipObjectInstance == null)
            {
                return;
            }
            else
            {
                if (m_iEnumeratorToastAttackTradeShipObject == null && eventBody.hitResult.attacker != null && eventBody.hitResult.attacker.type == 1)
                {
                    PDHeroAttacker pdHeroAttacker = eventBody.hitResult.attacker as PDHeroAttacker;

                    if (pdHeroAttacker == null)
                    {
                        return;
                    }
                    else
                    {
                        if (pdHeroAttacker.heroId == CsGameData.Instance.MyHeroInfo.HeroId)
                        {
                            return;
                        }
                        else
                        {
                            string strToastMessage = string.Format("{0} {1} 클텍 필요", pdHeroAttacker.name, CsGameData.Instance.GetMonsterInfo(pDTradeShipObjectInstance.monsterId).Name);

                            m_iEnumeratorToastAttackTradeShipObject = ToastAttackTradeShipObject(strToastMessage);
                            StartCoroutine(m_iEnumeratorToastAttackTradeShipObject);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator ToastAttackTradeShipObject(string strToastMessage)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, strToastMessage);

        yield return new WaitForSeconds(1f);

        m_iEnumeratorToastAttackTradeShipObject = null;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBossAppear(string strBossName, bool bAppear)
    {
        OpenPopupDungeonBossAppear(strBossName);
        transform.Find("PopupBoss").gameObject.SetActive(bAppear);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(!bAppear);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBossAppearSkip()
    {
        transform.Find("PopupBoss").gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        CsGameEventToIngame.Instance.OnEventBossAppearSkip();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDungeonClear()
    {
		// Dungeon Clear Event 중복 처리 방지
		if (transform.Find("PopupDungeonClear").gameObject.activeSelf)
			return;

        switch (CsDungeonManager.Instance.DungeonPlay)
        {
            case EnDungeonPlay.Story:

                break;

            case EnDungeonPlay.Exp:
                OpenPopupDungeonClear(true, EnDungeonPlay.Exp);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.Gold:
                OpenPopupDungeonClear(true, EnDungeonPlay.Gold);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.OsirisRoom:

                break;

            case EnDungeonPlay.UndergroundMaze:
                break;

            case EnDungeonPlay.ArtifactRoom:
                OpenPopupDungeonClear(true, EnDungeonPlay.ArtifactRoom);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.AncientRelic:
                OpenPopupDungeonClear(true, EnDungeonPlay.AncientRelic);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.FieldOfHonor:
                OpenPopupDungeonClear(true, EnDungeonPlay.FieldOfHonor);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.SoulCoveter:
                OpenPopupDungeonClear(true, EnDungeonPlay.SoulCoveter);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.Elite:
                OpenPopupDungeonClear(true, EnDungeonPlay.Elite);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.ProofOfValor:
                OpenPopupDungeonClear(true, EnDungeonPlay.ProofOfValor);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

			case EnDungeonPlay.WisdomTemple:
                OpenPopupDungeonClear(true, EnDungeonPlay.WisdomTemple);
				CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

			case EnDungeonPlay.RuinsReclaim:
				break;

            case EnDungeonPlay.InfiniteWar:
                break;

			case EnDungeonPlay.FearAltar:
                OpenPopupDungeonClear(true, EnDungeonPlay.FearAltar);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
				break;

            case EnDungeonPlay.WarMemory:
                OpenNewPopupDungeonClear(true, EnDungeonPlay.WarMemory);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.Biography:
                OpenPopupDungeonClear(true, EnDungeonPlay.Biography);
		        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.DragonNest:
                OpenNewPopupDungeonClear(true, EnDungeonPlay.DragonNest);
                CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);
                break;

            case EnDungeonPlay.TradeShip:

                break;

            case EnDungeonPlay.AnkouTomb:

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
	void OnClickButtonProgress()
	{
		CsGameEventUIToUI.Instance.OnEventOpenPopupHalidomCollection();
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
	}

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        //매칭팝업
        m_trPopupMatchingState = transform.Find("MatchingState");
        Transform trBack = m_trPopupMatchingState.Find("ImageBackground");

        m_trNewPopupDungeonClear = transform.Find("NewPopupDungeonClear");

        m_textRemaining = m_trNewPopupDungeonClear.Find("TextRemaining").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRemaining);

        m_strExitGuide = CsConfiguration.Instance.GetString("A13_TXT_03010");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickCloseMatchingPopup);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textPopupName = trBack.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPopupName);

        m_textState = trBack.Find("TextState").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textState);

        m_textTime = trBack.Find("TextTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTime);

		Transform trButtonProgress = trBack.Find("ButtonProgress");
		m_buttonProgress = trButtonProgress.GetComponent<Button>();
		m_textProgress = trButtonProgress.Find("TextProgress").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textProgress);

        Button buttonCancel = trBack.Find("ButtonMatcingCahcel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(OnClickMatchingCancel);
        buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textButtonCancel = buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textButtonCancel);

        Transform trCanvas = GameObject.Find("Canvas").transform;
        m_trPopup = trCanvas.Find("Popup");

		m_trButtons = transform.Find("Buttons");

		m_trButtonTargetHalidomMonster = m_trButtons.Find("ButtonTargetHalidomMonster");

		Button buttonTargetHalidomMonster = m_trButtonTargetHalidomMonster.GetComponent<Button>();
		buttonTargetHalidomMonster.onClick.RemoveAllListeners();
		buttonTargetHalidomMonster.onClick.AddListener(OnClickButtonTargetHalidomMonster);

		Text textHargetHalidomMonster = m_trButtonTargetHalidomMonster.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textHargetHalidomMonster);

		m_trButtonShowHalidomCollection = m_trButtons.Find("ButtonShowHalidomCollection");

		Button buttonShowHalidomCollection = m_trButtonShowHalidomCollection.GetComponent<Button>();
		buttonShowHalidomCollection.onClick.RemoveAllListeners();
		buttonShowHalidomCollection.onClick.AddListener(OnClickButtonShowHalidomCollection);

		Text textShowHalidomCollection = m_trButtonShowHalidomCollection.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textShowHalidomCollection);
		textShowHalidomCollection.text = CsConfiguration.Instance.GetString("A116_TXT_00010");	// 성물 조회
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMatchingState()
    {
        Image imageIcon = m_trPopupMatchingState.Find("ImageBackground/ImageIcon").GetComponent<Image>();

		m_buttonProgress.onClick.RemoveAllListeners();
		m_buttonProgress.gameObject.SetActive(m_enDungeonMatching == EnDungeon.FearAltar);

        switch (m_enDungeonMatching)
        {
            case EnDungeon.AncientRelic:
                m_textPopupName.text = CsDungeonManager.Instance.AncientRelic.Name;
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.AncientRelic);

                switch (CsDungeonManager.Instance.AncientRelicState)
                {
                    case EnDungeonMatchingState.None:
                        m_textState.text = "";
                        m_textTime.text = "";
                        break;

                    case EnDungeonMatchingState.Matching:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.AncientRelicMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.AncientRelicMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
                }

                break;

            case EnDungeon.SoulCoveter:
                m_textPopupName.text = CsDungeonManager.Instance.SoulCoveter.Name;
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.SoulCoveter);

                switch (CsDungeonManager.Instance.SoulCoveterMatchingState)
                {
                    case EnDungeonMatchingState.None:
                        m_textState.text = "";
                        m_textTime.text = "";
                        break;

                    case EnDungeonMatchingState.Matching:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.SoulCoveterMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.SoulCoveterMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
                }

				break;

			case EnDungeon.RuinsReclaim:
				m_textPopupName.text = CsDungeonManager.Instance.RuinsReclaim.Name;
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.RuinsReclaim);

				switch (CsDungeonManager.Instance.RuinsReclaimMatchingState)
	            {
                    case EnDungeonMatchingState.None:
                        m_textState.text = "";
                        m_textTime.text = "";
                        break;

					case EnDungeonMatchingState.Matching:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

					case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
						m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.RuinsReclaimMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

					case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
						m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.RuinsReclaimMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
	            }
               
				break;

            case EnDungeon.InfiniteWar:
                m_textPopupName.text = CsDungeonManager.Instance.InfiniteWar.Name;
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.InfiniteWar);

				switch (CsDungeonManager.Instance.InfiniteWarMatchingState)
                {
                    case EnDungeonMatchingState.None:
                        m_textState.text = "";
                        m_textTime.text = "";
                        break;

					case EnDungeonMatchingState.Matching:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

					case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = "";
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

					case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = "";
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
                }

                break;

			case EnDungeon.FearAltar:
				m_textPopupName.text = CsDungeonManager.Instance.FearAltar.Name;
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.FearAltar);

				m_buttonProgress.onClick.AddListener(OnClickButtonProgress);
				
				m_textProgress.text = string.Format(CsConfiguration.Instance.GetString("A116_TXT_00002"), CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomList.Count, CsGameData.Instance.FearAltar.FearAltarHalidomList.Count);

				switch (CsDungeonManager.Instance.FearAltarMatchingState)
				{
					case EnDungeonMatchingState.None:
						m_textState.text = "";
						m_textTime.text = "";
						break;

					case EnDungeonMatchingState.Matching:
						m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
						m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
						m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
						break;

					case EnDungeonMatchingState.MatchReady:
						m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
						m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.FearAltarMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
						m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
						break;

					case EnDungeonMatchingState.MatchComplete:
						m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
						m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.FearAltarMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
						m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
						break;
				}

				break;

            case EnDungeon.WarMemory:

                m_textPopupName.text = CsGameData.Instance.WarMemory.Name;
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.WarMemory);

				switch (CsDungeonManager.Instance.WarMemoryMatchingState)
				{
					case EnDungeonMatchingState.None:
						m_textState.text = "";
						m_textTime.text = "";
						break;

                    case EnDungeonMatchingState.Matching:
						m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
						m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
						m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
						break;

                    case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = "";
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = "";
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
				}

                break;

            case EnDungeon.DragonNest:

                m_textPopupName.text = CsGameData.Instance.DragonNest.Name;
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.DragonNest);

				switch (CsDungeonManager.Instance.DragonNestMatchingState)
				{
					case EnDungeonMatchingState.None:
						m_textState.text = "";
						m_textTime.text = "";
						break;

                    case EnDungeonMatchingState.Matching:
						m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
						m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
						m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
						break;

                    case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.DragonNestMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.DragonNestMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
				}

                break;

            case EnDungeon.TradeShip:
                
                m_textPopupName.text = CsGameData.Instance.TradeShip.Name;
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.TradeShip);

                switch (CsDungeonManager.Instance.TradeShipMatchingState)
                {
                    case EnDungeonMatchingState.None:
                        m_textState.text = "";
                        m_textTime.text = "";
                        break;

                    case EnDungeonMatchingState.Matching:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.TradeShipMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.TradeShipMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
                }

                break;

            case EnDungeon.AnkouTomb:

                m_textPopupName.text = CsGameData.Instance.AnkouTomb.Name;
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + (int)EnTodayTaskType.AnkouTomb);

				switch (CsDungeonManager.Instance.AnkouTombMatchingState)
				{
					case EnDungeonMatchingState.None:
						m_textState.text = "";
						m_textTime.text = "";
						break;

                    case EnDungeonMatchingState.Matching:
						m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
						m_textTime.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_2");
						m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
						break;

                    case EnDungeonMatchingState.MatchReady:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_1");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_3"), (CsDungeonManager.Instance.AnkouTombMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN1");
                        break;

                    case EnDungeonMatchingState.MatchComplete:
                        m_textState.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_4");
                        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_MATCH_5"), (CsDungeonManager.Instance.AnkouTombMatchingRemainingTime - Time.realtimeSinceStartup).ToString("0"));
                        m_textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_MATCH_BTN2");
                        break;
				}

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupMatching()
    {
        m_trPopupMatchingState.gameObject.SetActive(true);
        UpdateMatchingState();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupMatching()
    {
        m_trPopupMatchingState.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupDungeonClear(bool bIsClear, EnDungeonPlay enDungeonPlay)
    {
        m_trPopupDungeonClear = transform.Find("PopupDungeonClear");
		m_trPopupDungeonClear.gameObject.SetActive(true);

        Transform trNormal = m_trPopupDungeonClear.Find("Normal");
        Transform trParty = m_trPopupDungeonClear.Find("Party");
        Transform trBattle = m_trPopupDungeonClear.Find("Battle");
        Transform trElite = m_trPopupDungeonClear.Find("Elite");
        Transform trProofOfValor = m_trPopupDungeonClear.Find("ProofOfValor");
        Transform trRewardList = m_trPopupDungeonClear.Find("RewardList/Viewport/Content");

        Transform trClear;
        Transform trFail;
        Text textResult;

        trNormal.gameObject.SetActive(false);
        trParty.gameObject.SetActive(false);
        trBattle.gameObject.SetActive(false);
        trElite.gameObject.SetActive(false);
        trProofOfValor.gameObject.SetActive(false);

        switch (enDungeonPlay)
        {
            case EnDungeonPlay.MainQuest:
            case EnDungeonPlay.Story:
            case EnDungeonPlay.Gold:
            case EnDungeonPlay.Exp:
            case EnDungeonPlay.ArtifactRoom:
            case EnDungeonPlay.WisdomTemple:
            case EnDungeonPlay.Biography:
                trNormal.gameObject.SetActive(true);

                trClear = trNormal.Find("ImageClear");
                trClear.gameObject.SetActive(bIsClear);

                trFail = trNormal.Find("ImageFail");
                trFail.gameObject.SetActive(!bIsClear);

                textResult = trNormal.Find("TextResult").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResult);

                if (bIsClear)
                {
                    if (enDungeonPlay == EnDungeonPlay.ArtifactRoom)
                    {
                        textResult.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_00011"), CsDungeonManager.Instance.ArtifactRoomFloor.Floor);
                    }
                    else
                    {
                        textResult.text = CsConfiguration.Instance.GetString("A13_TXT_03008");
                    }
                }
                else
                {
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_03011");
                }

                trRewardList.gameObject.SetActive(bIsClear);
                break;

            case EnDungeonPlay.AncientRelic:
            case EnDungeonPlay.SoulCoveter:
            case EnDungeonPlay.FearAltar:
                if (bIsClear)
                {
                    trParty.gameObject.SetActive(true);

                    trClear = trParty.Find("ImageClear");
                    trClear.gameObject.SetActive(bIsClear);

                    trFail = trParty.Find("ImageFail");
                    trFail.gameObject.SetActive(!bIsClear);

                    //매칭맴버 추가

                    Transform trMemberList = trParty.Find("MemberList");

                    for (int i = 0; i < CsGameData.Instance.ListHeroObjectInfo.Count; i++)
                    {
                        Transform trMebmer = trMemberList.Find("Member" + i);
                        trMebmer.gameObject.SetActive(true);

                        CsHeroBase csHeroBase = CsGameData.Instance.ListHeroObjectInfo[i].GetHeroBase();

                        Image imageJob = trMebmer.Find("ImageJob").GetComponent<Image>();
                        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csHeroBase.Job.JobId);

                        Text textLevelName = trMebmer.Find("TextLvName").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textLevelName);
                        textLevelName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), csHeroBase.Level, csHeroBase.Name);
                    }

                    for (int i = 0; i < trMemberList.childCount - CsGameData.Instance.ListHeroObjectInfo.Count; i++)
                    {
                        Transform trMebmer = trMemberList.Find("Member" + (i + CsGameData.Instance.ListHeroObjectInfo.Count));
                        trMebmer.gameObject.SetActive(false);
                    }
                }
                else
                {
                    trNormal.gameObject.SetActive(true);

                    trClear = trNormal.Find("ImageClear");
                    trClear.gameObject.SetActive(bIsClear);

                    trFail = trNormal.Find("ImageFail");
                    trFail.gameObject.SetActive(!bIsClear);

                    textResult = trNormal.Find("TextResult").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textResult);
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_03011");
                }

                trRewardList.gameObject.SetActive(bIsClear);
                break;

            case EnDungeonPlay.FieldOfHonor:
                trBattle.gameObject.SetActive(true);

                trClear = trBattle.Find("ImageClear");
                trClear.gameObject.SetActive(bIsClear);

                trFail = trBattle.Find("ImageFail");
                trFail.gameObject.SetActive(!bIsClear);

                textResult = trBattle.Find("TextResult").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResult);

                Text textRanking = trBattle.Find("TextRanking").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRanking);

                if (bIsClear)
                {
                    textResult.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_03007"), CsDungeonManager.Instance.FieldOfHonorTartgetHero.name);
                    textRanking.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_03008"), CsGameData.Instance.FieldOfHonor.MyRanking);
                }
                else
                {
                    textResult.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_03009"), CsDungeonManager.Instance.FieldOfHonorTartgetHero.name);
                    textRanking.text = "";
                }

                trRewardList.gameObject.SetActive(true);
                break;

            case EnDungeonPlay.Elite:
                if (bIsClear)
                {
                    trElite.gameObject.SetActive(true);

                    trClear = trElite.Find("ImageClear");
                    trClear.gameObject.SetActive(bIsClear);

                    trFail = trElite.Find("ImageFail");
                    trFail.gameObject.SetActive(!bIsClear);

                    int nGrade = CsDungeonManager.Instance.EliteMonster.StarGrade;

                    Transform trGradeList = trElite.Find("GradeList");

                    for (int i = 0; i < nGrade; i++)
                    {
                        trGradeList.GetChild(i).gameObject.SetActive(true);
                    }

                    for (int i = 0; i < trGradeList.childCount - nGrade; i++)
                    {
                        trGradeList.GetChild(i + nGrade).gameObject.SetActive(false);
                    }

                    Text textname = trElite.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textname);
                    textname.text = string.Format(CsDungeonManager.Instance.EliteMonster.EliteMonsterMaster.Name, CsDungeonManager.Instance.EliteMonster.EliteMonsterMaster.Level);

                    Text textClear = trElite.Find("TextClear").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textClear);
                    textClear.text = CsConfiguration.Instance.GetString("A87_TXT_00002");
                }
                else
                {
                    trNormal.gameObject.SetActive(true);

                    trClear = trNormal.Find("ImageClear");
                    trClear.gameObject.SetActive(bIsClear);

                    trFail = trNormal.Find("ImageFail");
                    trFail.gameObject.SetActive(!bIsClear);

                    textResult = trNormal.Find("TextResult").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textResult);
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_03011");
                }

                trRewardList.gameObject.SetActive(false);
                break;

            case EnDungeonPlay.ProofOfValor:
                if (bIsClear)
                {
                    trProofOfValor.gameObject.SetActive(true);

                    trClear = trProofOfValor.Find("ImageClear");
                    trClear.gameObject.SetActive(bIsClear);

                    trFail = trProofOfValor.Find("ImageFail");
                    trFail.gameObject.SetActive(!bIsClear);

                    Image imageGrade = trProofOfValor.Find("ImageGrade").GetComponent<Image>();
					imageGrade.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/frm_dungeon_rank_" + CsGameData.Instance.ProofOfValor.ClearGrade);

                    textResult = trProofOfValor.Find("TextResult").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textResult);
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_02003");
                }
                else
                {
                    trProofOfValor.gameObject.SetActive(true);

                    trClear = trProofOfValor.Find("ImageClear");
                    trClear.gameObject.SetActive(bIsClear);

                    trFail = trProofOfValor.Find("ImageFail");
                    trFail.gameObject.SetActive(!bIsClear);

                    trProofOfValor.Find("ImageGrade").gameObject.SetActive(bIsClear);

                    textResult = trProofOfValor.Find("TextResult").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textResult);
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_03011");
                }

                trRewardList.gameObject.SetActive(true);
                break;

            case EnDungeonPlay.RuinsReclaim:
				if (!bIsClear)
				{
					trNormal.gameObject.SetActive(true);

					trClear = trNormal.Find("ImageClear");
					trClear.gameObject.SetActive(bIsClear);

					trFail = trNormal.Find("ImageFail");
					trFail.gameObject.SetActive(!bIsClear);

					textResult = trNormal.Find("TextResult").GetComponent<Text>();
					CsUIData.Instance.SetFont(textResult);
					textResult.text = CsConfiguration.Instance.GetString("A13_TXT_03011");
				}
				break;
        }

        Text textReward = m_trPopupDungeonClear.Find("TextReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReward);
        textReward.gameObject.SetActive(bIsClear);
        textReward.text = CsConfiguration.Instance.GetString("A13_TXT_03009");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/DungeonClearReward");
        }

        Transform trVip;
        Transform trExp;
        Transform trGold;
        Transform trHonor;
        Transform trSoulPowder;

        if (bIsClear)
        {
            //보상업데이트
            switch (enDungeonPlay)
            {
                case EnDungeonPlay.MainQuest:
                case EnDungeonPlay.Elite:
                
                case EnDungeonPlay.Biography:
				    textReward.gameObject.SetActive(false);
                    break;
                case EnDungeonPlay.AncientRelic:
                case EnDungeonPlay.Story:
                    for (int i = 0; i < m_pDItemBooty.Length; i++)
                    {
                        int nRewardIndex = i;
                        Transform trSlot = trRewardList.Find("Reward" + nRewardIndex);

                        if (trSlot == null)
                        {
                            trSlot = Instantiate(m_goRewardItem, trRewardList).transform;
                            trSlot.gameObject.name = "Reward" + i;
                            trSlot.gameObject.SetActive(true);
                        }
                        else
                        {
                            trSlot.gameObject.SetActive(true);
                        }

                        Image imageIcon = trSlot.Find("Image").GetComponent<Image>();

                        Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textItemName);

                        Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textItemCount);

                        if (m_pDItemBooty[i].type == (int)EnItemBootyType.Item)
                        {
                            CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);

                            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);
                            textItemName.text = csItem.Name;
                            textItemCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), m_pDItemBooty[i].count);
                        }
                    }
                    break;

                case EnDungeonPlay.Exp:
                    trExp = trRewardList.Find("Exp");
                    trExp.gameObject.SetActive(true);

                    Text textExp = trExp.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExp);
                    textExp.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

                    Text textExpValue = trExp.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpValue);
                    textExpValue.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    trVip = trRewardList.Find("Vip");
                    trVip.gameObject.SetActive(true);

                    Text textVip = trVip.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textVip);
                    textVip.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_VIPLV"), CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel);

                    Text textVipValue = trVip.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textVipValue);
                    textVipValue.text = "";
                    break;

                case EnDungeonPlay.Gold:
                    trGold = trRewardList.Find("Gold");
                    trGold.gameObject.SetActive(true);

                    Text textGold = trGold.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textGold);
                    textGold.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_RUPEE");

                    Text textGoldValue = trGold.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textGoldValue);
                    textGoldValue.text = m_lGoldDungeonRewardGold.ToString("#,##0");
                    break;

                case EnDungeonPlay.ArtifactRoom:
                    Transform trArtifactRoomRewardSlot = trRewardList.Find("Reward0");

                    if (trArtifactRoomRewardSlot == null)
                    {
                        trArtifactRoomRewardSlot = Instantiate(m_goRewardItem, trRewardList).transform;
                        trArtifactRoomRewardSlot.gameObject.name = "Reward0";
                        trArtifactRoomRewardSlot.gameObject.SetActive(true);
                    }
                    else
                    {
                        trArtifactRoomRewardSlot.gameObject.SetActive(true);
                    }

                    Image imageArtifactRoomRewardIcon = trArtifactRoomRewardSlot.Find("Image").GetComponent<Image>();

                    Text textArtifactRoomRewardItemName = trArtifactRoomRewardSlot.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textArtifactRoomRewardItemName);

                    Text textArtifactRoomRewardItemCount = trArtifactRoomRewardSlot.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textArtifactRoomRewardItemCount);


                    CsItem csItemArtifactRoomReward = CsGameData.Instance.GetItem(m_pDItemBootyArtifactRoom.id);

                    imageArtifactRoomRewardIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemArtifactRoomReward.Image);
                    textArtifactRoomRewardItemName.text = csItemArtifactRoomReward.Name;
                    textArtifactRoomRewardItemCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), m_pDItemBootyArtifactRoom.count);

                    break;

                case EnDungeonPlay.FieldOfHonor:
                    trExp = trRewardList.Find("Exp");
                    trExp.gameObject.SetActive(true);

                    Text textExpFieldOfHonor = trExp.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpFieldOfHonor);
                    textExpFieldOfHonor.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

                    Text textExpValueFieldOfHonor = trExp.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpValueFieldOfHonor);
                    textExpValueFieldOfHonor.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    trHonor = trRewardList.Find("Honor");
                    trHonor.gameObject.SetActive(true);

                    Text textHonor = trHonor.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textHonor);
                    textHonor.text = CsConfiguration.Instance.GetString("A31_TXT_03010");

                    Text textHonorValue = trHonor.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textHonorValue);
                    textHonorValue.text = m_nGetHonorPoint.ToString("#,##0");

                    break;

                case EnDungeonPlay.SoulCoveter:
                    for (int i = 0; i < m_pDItemBooty.Length; i++)
                    {
                        Transform trSoulCoveterRewardSlot = trRewardList.Find("Reward" + i);

                        if (trSoulCoveterRewardSlot == null)
                        {
                            trSoulCoveterRewardSlot = Instantiate(m_goRewardItem, trRewardList).transform;
                            trSoulCoveterRewardSlot.gameObject.name = "Reward" + i;
                            trSoulCoveterRewardSlot.gameObject.SetActive(true);
                        }
                        else
                        {
                            trSoulCoveterRewardSlot.gameObject.SetActive(true);
                        }

                        Image imageSoulCoveterRewardIcon = trSoulCoveterRewardSlot.Find("Image").GetComponent<Image>();

                        Text textSoulCoveterRewardItemName = trSoulCoveterRewardSlot.Find("TextName").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textSoulCoveterRewardItemName);

                        Text textSoulCoveterRewardItemCount = trSoulCoveterRewardSlot.Find("TextCount").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textSoulCoveterRewardItemCount);

                        CsItem csItemSoulCoveterReward = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);

                        imageSoulCoveterRewardIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemSoulCoveterReward.Image);
                        textSoulCoveterRewardItemName.text = csItemSoulCoveterReward.Name;
                        textSoulCoveterRewardItemCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), m_pDItemBooty[i].count);
                    }

                    break;

                case EnDungeonPlay.ProofOfValor:
                    trExp = trRewardList.Find("Exp");
                    trExp.gameObject.SetActive(true);

                    Text textExpProofOfValor = trExp.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpProofOfValor);
                    textExpProofOfValor.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

                    Text textExpValueProofOfValor = trExp.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpValueProofOfValor);
                    textExpValueProofOfValor.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    trSoulPowder = trRewardList.Find("SoulPowder");
                    trSoulPowder.gameObject.SetActive(true);

                    Text textSoulPowderProofOfValor = trSoulPowder.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textSoulPowderProofOfValor);
                    textSoulPowderProofOfValor.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_SOULP");

                    Text textSoulPowderValueProofOfValor = trSoulPowder.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textSoulPowderValueProofOfValor);
                    CsProofOfValorBossMonsterArrange csProofOfValorBossMonsterArrange = CsDungeonManager.Instance.ProofOfValor.GetProofOfValorBossMonsterArrange(m_nPreBossMonsterArrangeId);
                    textSoulPowderValueProofOfValor.text = (csProofOfValorBossMonsterArrange.RewardSoulPowder + csProofOfValorBossMonsterArrange.SpecialRewardSoulPowder).ToString("#,##0");

                    Transform trProofOfValorRewardSlot = trRewardList.Find("Reward0");

                    if (trProofOfValorRewardSlot == null)
                    {
                        trProofOfValorRewardSlot = Instantiate(m_goRewardItem, trRewardList).transform;
                        trProofOfValorRewardSlot.gameObject.name = "Reward0";
                        trProofOfValorRewardSlot.gameObject.SetActive(true);
                    }
                    else
                    {
                        trProofOfValorRewardSlot.gameObject.SetActive(true);
                    }

                    CsCreatureCard csCreatureCard = CsGameData.Instance.GetCreatureCard(m_nPreCreatureCardId);

                    if (csCreatureCard != null)
                    {
                        Image imageCardRewardIcon = trProofOfValorRewardSlot.Find("Image").GetComponent<Image>();
                        imageCardRewardIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_card_" + csCreatureCard.CreatureCardGrade.Grade);

                        Text textProofOfValorRewardItemName = trProofOfValorRewardSlot.Find("TextName").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textProofOfValorRewardItemName);
                        textProofOfValorRewardItemName.text = csCreatureCard.Name;

                        Text textProofOfValorRewardItemCount = trProofOfValorRewardSlot.Find("TextCount").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textProofOfValorRewardItemCount);
                        textProofOfValorRewardItemCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), 1);
                    }

                    break;

                case EnDungeonPlay.WisdomTemple:
					
					int nIndex = 0;

					foreach (var itemBooty in m_listWisdomTempleStepReward)
					{
						Transform trSlot = trRewardList.Find("Reward" + nIndex);

						if (trSlot == null)
						{
							trSlot = Instantiate(m_goRewardItem, trRewardList).transform;
							trSlot.gameObject.name = "Reward" + nIndex;
							trSlot.gameObject.SetActive(true);
						}
						else
						{
							trSlot.gameObject.SetActive(true);
						}

						Image imageIcon = trSlot.Find("Image").GetComponent<Image>();

						Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
						CsUIData.Instance.SetFont(textItemName);

						Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
						CsUIData.Instance.SetFont(textItemCount);

						if (itemBooty.type == (int)EnItemBootyType.Item)
						{
							CsItem csItem = CsGameData.Instance.GetItem(itemBooty.id);

							imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);
							textItemName.text = csItem.Name;
							textItemCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), itemBooty.count);
						}

						nIndex++;
					}

					break;

                case EnDungeonPlay.FearAltar:
					trExp = trRewardList.Find("Exp");
                    trExp.gameObject.SetActive(true);

                    Text textExpFearAltar = trExp.Find("TextName").GetComponent<Text>();
					CsUIData.Instance.SetFont(textExpFearAltar);
					textExpFearAltar.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

                    Text textExpValueFearAltar = trExp.Find("TextCount").GetComponent<Text>();
					CsUIData.Instance.SetFont(textExpValueFearAltar);
					textExpValueFearAltar.text = m_lExpDungeonRewardExp.ToString("#,##0");
					break;

            }
        }
        else
        {
            switch (enDungeonPlay)
            {
                case EnDungeonPlay.FieldOfHonor:
                    trExp = trRewardList.Find("Exp");
                    trExp.gameObject.SetActive(true);

                    Text textExpFieldOfHonor = trExp.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpFieldOfHonor);
                    textExpFieldOfHonor.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

                    Text textExpValueFieldOfHonor = trExp.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpValueFieldOfHonor);
                    textExpValueFieldOfHonor.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    trHonor = trRewardList.Find("Honor");
                    trHonor.gameObject.SetActive(true);

                    Text textHonor = trHonor.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textHonor);
                    textHonor.text = CsConfiguration.Instance.GetString("A31_TXT_03010");

                    Text textHonorValue = trHonor.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textHonorValue);
                    textHonorValue.text = m_nGetHonorPoint.ToString("#,##0");

                    break;

                case EnDungeonPlay.ProofOfValor:
                    textReward.gameObject.SetActive(true);
                    trExp = trRewardList.Find("Exp");
                    trExp.gameObject.SetActive(true);

                    Text textExpProofOfValor = trExp.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpProofOfValor);
                    textExpProofOfValor.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

                    Text textExpProofOfValorValue = trExp.Find("TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textExpProofOfValorValue);
                    textExpProofOfValorValue.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    break;
            }
        }

        Button buttonExit = m_trPopupDungeonClear.Find("ButtonList/ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(() => OnClickExitDungeon(enDungeonPlay));
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textExit = buttonExit.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExit);
        textExit.text = CsConfiguration.Instance.GetString("A13_BTN_00004");

        Button buttonNext = m_trPopupDungeonClear.Find("ButtonList/ButtonNext").GetComponent<Button>();
        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(OnClickNextFloor);
        buttonNext.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNext = buttonNext.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNext);
        textNext.text = CsConfiguration.Instance.GetString("A47_BTN_00008");

		m_textRemaining = m_trPopupDungeonClear.Find("TextRemaning").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textRemaining);
		
		m_strExitGuide = CsConfiguration.Instance.GetString("A13_TXT_03010");
		m_textRemaining.text = "";

        if (bIsClear)
        {
            if (enDungeonPlay == EnDungeonPlay.ArtifactRoom)
            {
                buttonNext.gameObject.SetActive(true);

                CsArtifactRoomFloor csArtifactRoomFloor = CsDungeonManager.Instance.ArtifactRoom.GetArtifactRoomFloor(CsDungeonManager.Instance.ArtifactRoom.ArtifactRoomCurrentFloor);

                if (csArtifactRoomFloor == null)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonNext, false);
                }
                else
                {
                    if (csArtifactRoomFloor.RequiredHeroLevel > CsGameData.Instance.MyHeroInfo.Level)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(buttonNext, false);
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(buttonNext, true);
                        m_strExitGuide = CsConfiguration.Instance.GetString("A47_TXT_00010");
                    }
                }
            }
            else
            {
                buttonNext.gameObject.SetActive(false);
            }
        }
        else
        {
            buttonNext.gameObject.SetActive(false);
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OpenPopupRuinsReclaimDungeonClear(PDItemBooty[] aPDItemBooty, PDItemBooty randomBooty, string strMonsterTerminator, string strUltimateAttackKing, string strPartyVolunteer)
	{
		GameObject goPopupRuinsReclaimClear = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/PopupRuinsReclaimClear");
		m_trPopupRuinsReclaimClear = Instantiate(goPopupRuinsReclaimClear, transform).transform;

		// 랜덤 보상 -> 카드 선택 후 세팅
		Transform trRewardCardList = m_trPopupRuinsReclaimClear.Find("FrameTop/RewardCardList");

		for (int i = 0; i < trRewardCardList.childCount; i++)
		{
			int nButtonIndex = i;

			Transform trFrameCard = trRewardCardList.GetChild(i);

			trFrameCard.Find("ImageButtonBack").gameObject.SetActive(true);
			trFrameCard.Find("ImageFront").gameObject.SetActive(false);
			trFrameCard.Find("ImageSelected").gameObject.SetActive(false);

			Button button = trFrameCard.Find("ImageButtonBack").GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => OnClickRuinsReclaimRewardCard(nButtonIndex, randomBooty));
			button.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		}

		// 카드 선택 텍스트
		Text textDescription = m_trPopupRuinsReclaimClear.Find("FrameTop/TextDescription").GetComponent<Text>();
		CsUIData.Instance.SetFont(textDescription);
		textDescription.text = CsConfiguration.Instance.GetString("A110_TXT_00004");

		// 특별 보상
		Transform trFrameSpecialReward = m_trPopupRuinsReclaimClear.Find("FrameSpecialReward");

		Text textSpecialReward = trFrameSpecialReward.Find("ImageSpecialReward/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSpecialReward);
		textSpecialReward.text = CsConfiguration.Instance.GetString("A110_TXT_00005");

		// 몬스터
		Transform trImageMonster = trFrameSpecialReward.Find("ImageMonster");

		Text textTitle = trImageMonster.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A110_TXT_00007");

		Text textName = trImageMonster.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = strMonsterTerminator;

		// 공격
		Transform trImageAttacker = trFrameSpecialReward.Find("ImageAttacker");

		textTitle = trImageAttacker.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A110_TXT_00008");

		textName = trImageAttacker.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = strUltimateAttackKing;

		// 서포트
		Transform trImageSupporter = trFrameSpecialReward.Find("ImageSupporter");

		textTitle = trImageSupporter.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A110_TXT_00009");

		textName = trImageSupporter.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = strPartyVolunteer;

		// 고정 보상
		Transform trFrameReward = m_trPopupRuinsReclaimClear.Find("FrameReward");

		Text textReward = trFrameReward.Find("ImageReward/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textReward);
		textReward.text = CsConfiguration.Instance.GetString("A110_TXT_00006");

		Transform trContent = trFrameReward.Find("Scroll View/Viewport/Content");

		for (int i = 0; i < trContent.childCount; i++)
		{
			trContent.GetChild(i).gameObject.SetActive(false);
		}

		int nBootyIndex = 0;

		foreach (var itemBooty in aPDItemBooty)
		{
			Transform trItemBooty;

			if (nBootyIndex < trContent.childCount)
			{
				// 켜기
				trItemBooty = trContent.GetChild(nBootyIndex);
				trItemBooty.gameObject.SetActive(true);
			}
			else
			{
				// 생성
				trItemBooty = Instantiate(m_goRewardItemSlot, trContent).transform;
				trItemBooty.name = "ItemBooty" + nBootyIndex;
			}

			CsUIData.Instance.DisplayItemSlot(trItemBooty, CsGameData.Instance.GetItem(itemBooty.id), itemBooty.owned, itemBooty.count, false);

			nBootyIndex++;
		}

		Button buttonExit = m_trPopupRuinsReclaimClear.Find("ButtonExit").GetComponent<Button>();
		buttonExit.onClick.RemoveAllListeners();
		buttonExit.onClick.AddListener(() => OnClickExitDungeon(EnDungeonPlay.RuinsReclaim));
		buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		buttonExit.interactable = false; // 카드 뒤집은 후 클릭 가능

		Text textExit = buttonExit.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textExit);
		textExit.text = CsConfiguration.Instance.GetString("A13_BTN_00004");
	}

    //---------------------------------------------------------------------------------------------------
    void OpenNewPopupDungeonClear(bool bIsClear, EnDungeonPlay enDungeonPlay)
    {
        m_trNewPopupDungeonClear = transform.Find("NewPopupDungeonClear");

        Transform trImageClear = m_trNewPopupDungeonClear.Find("ImageClear");
        Transform trImageFail = m_trNewPopupDungeonClear.Find("ImageFail");

        Button buttonExit = m_trNewPopupDungeonClear.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();

        Text textExit = buttonExit.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExit);
        textExit.text = CsConfiguration.Instance.GetString("A13_BTN_00004");

        m_textRemaining = m_trNewPopupDungeonClear.Find("TextRemaining").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRemaining);
		m_textRemaining.text = "";

		m_strExitGuide = CsConfiguration.Instance.GetString("A13_TXT_03010");

        if (bIsClear)
        {
            trImageFail.gameObject.SetActive(false);
            trImageClear.gameObject.SetActive(true);
        }
        else
        {
            trImageClear.gameObject.SetActive(false);
            trImageFail.gameObject.SetActive(true);
        }

        Transform trStory = m_trNewPopupDungeonClear.Find("Story");
        Transform trInfiniteWar = m_trNewPopupDungeonClear.Find("InfiniteWar");
        Transform trWarMemory = m_trNewPopupDungeonClear.Find("WarMemory");
        Transform trOsirisRoom = m_trNewPopupDungeonClear.Find("OsirisRoom");
        Transform trDragonNest = m_trNewPopupDungeonClear.Find("DragonNest");
        Transform trTradeShip = m_trNewPopupDungeonClear.Find("TradeShip");
        Transform trAnkouTomb = m_trNewPopupDungeonClear.Find("AnkouTomb");

        trStory.gameObject.SetActive(false);
        trInfiniteWar.gameObject.SetActive(false);
        trWarMemory.gameObject.SetActive(false);
        trOsirisRoom.gameObject.SetActive(false);
        trDragonNest.gameObject.SetActive(false);
        trTradeShip.gameObject.SetActive(false);
        trAnkouTomb.gameObject.SetActive(false);

        Transform trRewardItemList = null;
        Transform trItemSlot = null;
        Transform trPanelResult = null;

        Text textResult = null;
        Text textReward = null;
        Text textFail = null;
        Text textNoReward = null;

        Animation animPopupDungeonClear = m_trNewPopupDungeonClear.GetComponent<Animation>();

        switch (enDungeonPlay)
        {
            case EnDungeonPlay.Story:
                {
                    trPanelResult = trStory.Find("PanelResult");

                    textResult = trPanelResult.Find("Image/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textResult);
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_00009");

                    Text textResultTime = trPanelResult.Find("ResultTime/Text").GetComponent<Text>();
                    Text textResultTimeValue = trPanelResult.Find("ResultTime/TextValue").GetComponent<Text>();
                    Text textResultHit = trPanelResult.Find("ResultHit/Text").GetComponent<Text>();
                    Text textResultHitValue = trPanelResult.Find("ResultHit/TextValue").GetComponent<Text>();
                    Text textResultDamaged = trPanelResult.Find("ResultDamaged/Text").GetComponent<Text>();
                    Text textResultDamagedValue = trPanelResult.Find("ResultDamaged/TextValue").GetComponent<Text>();

                    CsUIData.Instance.SetFont(textResultTime);
                    CsUIData.Instance.SetFont(textResultTimeValue);
                    CsUIData.Instance.SetFont(textResultHit);
                    CsUIData.Instance.SetFont(textResultHitValue);
                    CsUIData.Instance.SetFont(textResultDamaged);
                    CsUIData.Instance.SetFont(textResultDamagedValue);

                    textResultTime.text = CsConfiguration.Instance.GetString("A13_TXT_00010");
                    textResultHit.text = CsConfiguration.Instance.GetString("A13_TXT_00011");
                    textResultDamaged.text = CsConfiguration.Instance.GetString("A13_TXT_00012");

                    m_flDungeonCelarTime = Time.realtimeSinceStartup - m_flDungeonCelarTime;
                    TimeSpan timeSpanClearTime = TimeSpan.FromSeconds(m_flDungeonCelarTime);
                    textResultTimeValue.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), timeSpanClearTime.Minutes.ToString("0#"), timeSpanClearTime.Seconds.ToString("0#"));
                    textResultHitValue.text = m_nHit.ToString("#,##0");
                    textResultDamagedValue.text = m_nDamaged.ToString("#,##0");

                    textFail = trPanelResult.Find("TextFail").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textFail);
                    textFail.text = CsConfiguration.Instance.GetString("A121_TXT_00004");

                    Transform trPanelReward = trStory.Find("PanelReward");

                    textReward = trPanelReward.Find("Image/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textReward);
                    textReward.text = CsConfiguration.Instance.GetString("A13_TXT_00013");

                    trRewardItemList = trPanelReward.Find("Scroll View/Viewport/Content");

                    textNoReward = trPanelReward.Find("TextNoReward").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNoReward);
                    textNoReward.text = CsConfiguration.Instance.GetString("A121_TXT_00005");

                    for (int i = 0; i < trRewardItemList.childCount; i++)
                    {
                        trRewardItemList.GetChild(i).gameObject.SetActive(false);
                    }

                    if (bIsClear)
                    {
                        for (int i = 0; i < trPanelResult.childCount; i++)
                        {
                            if (trPanelResult.GetChild(i).name == "Image")
                            {
                                continue;
                            }
                            else
                            {
                                if (trPanelResult.GetChild(i).name == textFail.name)
                                {
                                    trPanelResult.GetChild(i).gameObject.SetActive(false);
                                }
                                else
                                {
                                    trPanelResult.GetChild(i).gameObject.SetActive(true);
                                }
                            }
                        }

                        textNoReward.gameObject.SetActive(false);

                        for (int i = 0; i < m_pDItemBooty.Length; i++)
                        {
                            trItemSlot = trRewardItemList.Find("ItemSlot" + i);

                            if (trItemSlot == null)
                            {
                                trItemSlot = Instantiate(m_goRewardItemSlot, trRewardItemList).transform;
                                trItemSlot.name = "ItemSlot" + i;
                            }
                            else
                            {
                                trItemSlot.gameObject.SetActive(true);
                            }

                            CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);
                            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_pDItemBooty[i].owned, m_pDItemBooty[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                        }

                        trPanelReward.gameObject.SetActive(true);
                    }
                    else
                    {
                        for (int i = 0; i < trPanelResult.childCount; i++)
                        {
                            if (trPanelResult.GetChild(i).name == "Image")
                            {
                                continue;
                            }
                            else
                            {
                                if (trPanelResult.GetChild(i).name == textFail.name)
                                {
                                    trPanelResult.GetChild(i).gameObject.SetActive(true);
                                }
                                else
                                {
                                    trPanelResult.GetChild(i).gameObject.SetActive(false);
                                }
                            }
                        }

                        textNoReward.gameObject.SetActive(true);
                    }


                    trStory.gameObject.SetActive(true);

                    animPopupDungeonClear.Stop();
                    animPopupDungeonClear.Play();
                }
                break;

            case EnDungeonPlay.InfiniteWar:

                Transform trRankList = trInfiniteWar.Find("RankingList");

                for (int i = 0; i < trRankList.childCount; i++)
                {
                    trRankList.GetChild(i).gameObject.SetActive(false);
                }

                Transform trImageRankBack = null;

                if (m_aPDInfiniteWarRanking == null)
                {

                }
                else
                {
                    int nIndex = 0;

                    for (int i = 0; i < m_aPDInfiniteWarRanking.Length; i++)
                    {
                        trImageRankBack = trRankList.Find("ImageRankBack" + i);

                        if (trImageRankBack == null || m_aPDInfiniteWarRanking[i].rank == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (nIndex < trRankList.childCount)
                            {
                                Text textRank = trImageRankBack.Find("ImageRank/Text").GetComponent<Text>();
                                CsUIData.Instance.SetFont(textRank);
                                textRank.text = m_aPDInfiniteWarRanking[i].rank.ToString();

                                Image imageJobIcon = trImageRankBack.Find("ImageJobIcon").GetComponent<Image>();
                                imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + m_aPDInfiniteWarRanking[i].jobId);

                                Text textName = trImageRankBack.Find("TextName").GetComponent<Text>();
                                CsUIData.Instance.SetFont(textName);
                                textName.text = m_aPDInfiniteWarRanking[i].name;

                                Text textPoint = trImageRankBack.Find("TextPoint").GetComponent<Text>();
                                CsUIData.Instance.SetFont(textPoint);
                                textPoint.text = m_aPDInfiniteWarRanking[i].point.ToString("#,##0");

                                trImageRankBack.gameObject.SetActive(true);

                                nIndex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                Text textMyPoint = trInfiniteWar.Find("ImageMyPoint/TextMyPoint").GetComponent<Text>();
                CsUIData.Instance.SetFont(textMyPoint);
                textMyPoint.text = CsConfiguration.Instance.GetString("A112_TXT_00001");

                Text textMyPointValue = trInfiniteWar.Find("ImageMyPoint/TextMyPointValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textMyPointValue);

                if (CsDungeonManager.Instance.DicInfiniteWarHeroPoint.ContainsKey(CsGameData.Instance.MyHeroInfo.HeroId))
                {
                    textMyPointValue.text = CsDungeonManager.Instance.DicInfiniteWarHeroPoint[CsGameData.Instance.MyHeroInfo.HeroId].Point.ToString("#,##0");
                }
                else
                {
                    textMyPointValue.text = "0";
                }

                Text textRewardItem = trInfiniteWar.Find("ImageRewardItem/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRewardItem);
                textRewardItem.text = CsConfiguration.Instance.GetString("A13_TXT_00013");

                trRewardItemList = trInfiniteWar.Find("ImageRewardItem/Scroll View/Viewport/Content");

                for (int i = 0; i < trRewardItemList.childCount; i++)
                {
                    trRewardItemList.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < m_pDItemBooty.Length; i++)
                {
                    trItemSlot = trRewardItemList.Find("ItemSlot" + i);

                    if (trItemSlot == null)
                    {
                        trItemSlot = Instantiate(m_goRewardItemSlot, trRewardItemList).transform;
                        trItemSlot.name = "ItemSlot" + i;
                    }
                    else
                    {
                        trItemSlot.gameObject.SetActive(true);
                    }

                    CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_pDItemBooty[i].owned, m_pDItemBooty[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                }

                for (int i = 0; i < m_aPDInfiniteWarRanking.Length; i++)
                {
                    if (m_aPDInfiniteWarRanking[i].heroId == CsGameData.Instance.MyHeroInfo.HeroId)
                    {
                        for (int j = 0; j < m_pDItemBooty.Length; j++)
                        {
                            trItemSlot = trInfiniteWar.Find("ItemSlot" + (j + m_pDItemBooty.Length));

                            if (trItemSlot == null)
                            {
                                trItemSlot = Instantiate(m_goRewardItemSlot, trRewardItemList).transform;
                                trItemSlot.name = "ItemSlot" + (j + m_pDItemBooty.Length);
                            }
                            else
                            {
                                trItemSlot.gameObject.SetActive(true);
                            }

                            CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);
                            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_pDItemBooty[i].owned, m_pDItemBooty[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                        }

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }


                trInfiniteWar.gameObject.SetActive(true);
                animPopupDungeonClear.Stop();

                break;

            case EnDungeonPlay.WarMemory:

                trPanelResult = trWarMemory.Find("PanelResult");

                textResult = trPanelResult.Find("Image/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResult);
                textResult.text = CsConfiguration.Instance.GetString("A13_TXT_00009");

                Text textResultPoint = trPanelResult.Find("ResultPoint/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResultPoint);
                textResultPoint.text = CsConfiguration.Instance.GetString("A121_TXT_00003");

                Text textResultPointValue = trPanelResult.Find("ResultPoint/TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResultPointValue);
                textResultPointValue.text = CsDungeonManager.Instance.DicWarMemoryHeroPoint[CsGameData.Instance.MyHeroInfo.HeroId].Point.ToString("#,##0");

                Text textResultRanking = trPanelResult.Find("ResultRanking/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResultRanking);
                textResultRanking.text = CsConfiguration.Instance.GetString("A121_TXT_00002");

                Text textResultRankingValue = trPanelResult.Find("ResultRanking/TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResultRankingValue);

                textFail = trPanelResult.Find("TextFail").GetComponent<Text>();
                CsUIData.Instance.SetFont(textFail);
                textFail.text = CsConfiguration.Instance.GetString("A121_TXT_00004");

                Transform trRightPanel = trWarMemory.Find("RightPanel");

                CsWarMemoryReward csWarMemoryReward = CsGameData.Instance.WarMemory.WarMemoryRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level);

                Text textExpReward = trRightPanel.Find("PanelExpReward/TextExpReward").GetComponent<Text>();
                CsUIData.Instance.SetFont(textExpReward);
                textExpReward.text = CsConfiguration.Instance.GetString("A121_TXT_00011");

                Text textValue = trRightPanel.Find("PanelExpReward/TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textValue);

                if (csWarMemoryReward == null)
                {
                    textValue.text = "";
                }
                else
                {
                    textValue.text = csWarMemoryReward.ExpReward.Value.ToString("#,##0");
                }

                textReward = trRightPanel.Find("PanelReward/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textReward);
                textReward.text = CsConfiguration.Instance.GetString("A13_TXT_00013");

                textNoReward = trRightPanel.Find("PanelReward/TextNoReward").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNoReward);
                textNoReward.text = CsConfiguration.Instance.GetString("A121_TXT_00005");

                trRewardItemList = trRightPanel.Find("PanelReward/Scroll View/Viewport/Content");

                for (int i = 0; i < trRewardItemList.childCount; i++)
                {
                    trRewardItemList.GetChild(i).gameObject.SetActive(false);
                }

                if (bIsClear)
                {
                    int nMyRanking = 0;

                    for (int i = 0; i < m_aPDWarMemoryRanking.Length; i++)
                    {
                        if (m_aPDWarMemoryRanking[i].heroId == CsGameData.Instance.MyHeroInfo.HeroId)
                        {
                            nMyRanking = m_aPDWarMemoryRanking[i].rank;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    textResultRankingValue.text = nMyRanking.ToString("#,##0");

                    for (int i = 0; i < trPanelResult.childCount; i++)
                    {
                        if (trPanelResult.GetChild(i).name == "Image")
                        {
                            continue;
                        }
                        else
                        {
                            if (trPanelResult.GetChild(i).name == textFail.name)
                            {
                                trPanelResult.GetChild(i).gameObject.SetActive(false);
                            }
                            else
                            {
                                trPanelResult.GetChild(i).gameObject.SetActive(true);
                            }
                        }
                    }

                    trRightPanel.Find("PanelExpReward").gameObject.SetActive(true);

                    for (int i = 0; i < m_pDItemBooty.Length; i++)
                    {
                        trItemSlot = trRewardItemList.Find("ItemSlot" + i);

                        if (trItemSlot == null)
                        {
                            trItemSlot = Instantiate(m_goRewardItemSlot, trRewardItemList).transform;
                            trItemSlot.name = "ItemSlot" + i;
                        }
                        else
                        {
                            trItemSlot.gameObject.SetActive(true);
                        }

                        CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);
                        CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_pDItemBooty[i].owned, m_pDItemBooty[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    }

                    textNoReward.gameObject.SetActive(false);
                }
                else
                {
                    for (int i = 0; i < trPanelResult.childCount; i++)
                    {
                        if (trPanelResult.GetChild(i).name == "Image")
                        {
                            continue;
                        }
                        else
                        {
                            if (trPanelResult.GetChild(i).name == textFail.name)
                            {
                                trPanelResult.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                trPanelResult.GetChild(i).gameObject.SetActive(false);
                            }
                        }
                    }

                    trRightPanel.Find("PanelExpReward").gameObject.SetActive(false);

                    textNoReward.gameObject.SetActive(true);
                }


                trWarMemory.gameObject.SetActive(true);
                animPopupDungeonClear.Stop();

                break;

            case EnDungeonPlay.OsirisRoom:
                {
                    trPanelResult = trOsirisRoom.Find("PanelResult");

                    textResult = trPanelResult.Find("Image/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textResult);
                    textResult.text = CsConfiguration.Instance.GetString("A13_TXT_00009");

                    Text textResultTime = trPanelResult.Find("ResultTime/Text").GetComponent<Text>();
                    Text textResultTimeValue = trPanelResult.Find("ResultTime/TextValue").GetComponent<Text>();
                    Text textResultHit = trPanelResult.Find("ResultHit/Text").GetComponent<Text>();
                    Text textResultHitValue = trPanelResult.Find("ResultHit/TextValue").GetComponent<Text>();
                    Text textResultDamaged = trPanelResult.Find("ResultDamaged/Text").GetComponent<Text>();
                    Text textResultDamagedValue = trPanelResult.Find("ResultDamaged/TextValue").GetComponent<Text>();

                    CsUIData.Instance.SetFont(textResultTime);
                    CsUIData.Instance.SetFont(textResultTimeValue);
                    CsUIData.Instance.SetFont(textResultHit);
                    CsUIData.Instance.SetFont(textResultHitValue);
                    CsUIData.Instance.SetFont(textResultDamaged);
                    CsUIData.Instance.SetFont(textResultDamagedValue);

                    textResultTime.text = CsConfiguration.Instance.GetString("A13_TXT_00010");
                    textResultHit.text = CsConfiguration.Instance.GetString("A13_TXT_00011");
                    textResultDamaged.text = CsConfiguration.Instance.GetString("A13_TXT_00012");

                    m_flDungeonCelarTime = Time.realtimeSinceStartup - m_flDungeonCelarTime;
                    TimeSpan timeSpanClearTime = TimeSpan.FromSeconds(m_flDungeonCelarTime);
                    textResultTimeValue.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), timeSpanClearTime.Minutes.ToString("0#"), timeSpanClearTime.Seconds.ToString("0#"));
                    textResultHitValue.text = m_nHit.ToString("#,##0");
                    textResultDamagedValue.text = m_nDamaged.ToString("#,##0");

                    Transform trPanelReward = trOsirisRoom.Find("PanelGoldReward");

                    textReward = trPanelReward.Find("TextGoldReward").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textReward);
                    textReward.text = CsConfiguration.Instance.GetString("A121_TXT_00011");

                    Text textGoldValue = trPanelReward.Find("TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textGoldValue);
                    textGoldValue.text = CsDungeonManager.Instance.OsirisRoomTotalGoldAcquisitionValue.ToString("#,##0");

                    trOsirisRoom.gameObject.SetActive(true);
                    animPopupDungeonClear.Stop();
                }

                break;

            case EnDungeonPlay.DragonNest:
                Transform trHeroList = trDragonNest.Find("HeroList");

                Transform trPanelFail = trDragonNest.Find("PanelFail");

                textResult = trPanelFail.Find("Image/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResult);
                textResult.text = CsConfiguration.Instance.GetString("A13_TXT_00009");

                textFail = trPanelFail.Find("TextFail").GetComponent<Text>();
                CsUIData.Instance.SetFont(textFail);
                textFail.text = CsConfiguration.Instance.GetString("A121_TXT_00004");

                Transform trDragonNestPanelReward = trDragonNest.Find("PanelReward");
                Transform trDragonNestRewardList = trDragonNestPanelReward.Find("Scroll View/Viewport/Content");

                textReward = trDragonNestPanelReward.Find("Image/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textReward);
                textReward.text = CsConfiguration.Instance.GetString("A13_TXT_00013");

                textNoReward = trDragonNestPanelReward.Find("TextNoReward").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNoReward);
                textNoReward.text = CsConfiguration.Instance.GetString("A121_TXT_00005");

                if (m_goDragonNestHeroInfo == null)
                {
                    m_goDragonNestHeroInfo = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ImageHeroBack");
                }

                if (bIsClear)
                {
                    Transform trHeroItem = null;
                    // 클리어
                    for (int i = 0; i < m_arrPDHero.Length; i++)
                    {
                        PDSimpleHero pDSimpleHero = m_arrPDHero[i];

                        if (pDSimpleHero.id == CsGameData.Instance.MyHeroInfo.HeroId)
                        {
                            continue;
                        }
                        else
                        {
                            trHeroItem = trHeroList.Find("HeroItem" + i);

                            if (trHeroItem == null)
                            {
                                trHeroItem = Instantiate(m_goDragonNestHeroInfo, trHeroList).transform;
                                trHeroItem.name = "HeroItem" + i;
                            }
                            else
                            {
                                trHeroItem.gameObject.SetActive(true);
                            }

                            Image imageJobIcon = trHeroItem.Find("ImageJobIcon").GetComponent<Image>();
                            imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + pDSimpleHero.jobId);

                            Text textLevel = trHeroItem.Find("TextLevel").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textLevel);
                            textLevel.text = string.Format(CsConfiguration.Instance.GetString("A144_TXT_01005"), pDSimpleHero.level);

                            Text textName = trHeroItem.Find("TextName").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textName);
                            textName.text = pDSimpleHero.name;

                            Transform trImageFriendIcon = trHeroItem.Find("ImageFriendIcon");

                            Text textFriend = trImageFriendIcon.Find("Text").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textFriend);
                            textFriend.text = CsConfiguration.Instance.GetString("A144_TXT_00010");

                            if (CsFriendManager.Instance.FriendList.Find(a => a.Id == pDSimpleHero.id) != null)
                            {
                                trImageFriendIcon.gameObject.SetActive(true);
                            }
                            else
                            {
                                trImageFriendIcon.gameObject.SetActive(false);
                            }

                            Transform trImageTeam = trHeroItem.Find("ImageTeam");

                            Text textTeam = trImageTeam.Find("Text").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textTeam);
                            textTeam.text = CsConfiguration.Instance.GetString("A144_TXT_00011");

                            Button buttonTeam = trHeroItem.Find("ButtonTeam").GetComponent<Button>();
                            buttonTeam.onClick.RemoveAllListeners();
                            buttonTeam.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                            buttonTeam.onClick.AddListener(() => OnClickTeamInvite(pDSimpleHero.id));

                            Text textButtonTeam = buttonTeam.transform.Find("Text").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textButtonTeam);
                            textButtonTeam.text = CsConfiguration.Instance.GetString("A144_BTN_00003");
                        }
                    }

                    trHeroList.gameObject.SetActive(true);

                    trPanelFail.gameObject.SetActive(false);

                    for (int i = 0; i < m_listDragonNestStepReward.Count; i++)
                    {
                        trItemSlot = trDragonNestRewardList.Find("ItemSlot" + i);

                        if (trItemSlot == null)
                        {
                            trItemSlot = Instantiate(m_goRewardItemSlot, trDragonNestRewardList).transform;
                            trItemSlot.name = "ItemSlot" + i;
                        }
                        else
                        {
                            trItemSlot.gameObject.SetActive(true);
                        }

                        CsItem csItem = CsGameData.Instance.GetItem(m_listDragonNestStepReward[i].id);
                        CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_listDragonNestStepReward[i].owned, m_listDragonNestStepReward[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    }

                    textNoReward.gameObject.SetActive(false);
                }
                else
                {
                    // 실패
                    trHeroList.gameObject.SetActive(false);

                    trPanelFail.gameObject.SetActive(true);

                    for (int i = 0; i < trDragonNestRewardList.childCount; i++)
                    {
                        trDragonNestRewardList.GetChild(i).gameObject.SetActive(false);
                    }

                    textNoReward.gameObject.SetActive(true);
                }

                trDragonNest.gameObject.SetActive(true);
                break;

            case EnDungeonPlay.TradeShip:
                {
                    Transform trImageBackground = trTradeShip.Find("ImageBackground");
                    m_trDungeonClear = trImageBackground;

                    textMyPoint = trImageBackground.Find("TextMyPoint").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textMyPoint);
                    textMyPoint.text = CsConfiguration.Instance.GetString("PUBLIC_DUNPOINT_4");

                    textMyPointValue = trImageBackground.Find("TextMyPointValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textMyPointValue);
                    textMyPointValue.text = CsDungeonManager.Instance.TradeShipPoint.ToString("#,##0");

                    Text textRewardExpValue = trImageBackground.Find("RewardExp/TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardExpValue);
                    textRewardExpValue.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    Text textRewardGoldValue = trImageBackground.Find("RewardGold/TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardExpValue);

                    CsTradeShipDifficulty csTradeShipDifficulty = CsGameData.Instance.TradeShip.TradeShipDifficultyList.Find(a => a.Difficulty == m_nDifficulty);

                    if (csTradeShipDifficulty == null)
                    {
                        textRewardGoldValue.text = "";
                    }
                    else
                    {
                        textRewardGoldValue.text = csTradeShipDifficulty.GoldReward.Value.ToString("#,##0");
                    }

                    textRewardItem = trImageBackground.Find("ImageLineTop/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardItem);
                    textRewardItem.text = CsConfiguration.Instance.GetString("A13_TXT_00013");

                    textNoReward = trImageBackground.Find("TextNoReward").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNoReward);
                    textNoReward.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_5");

                    trRewardItemList = trImageBackground.Find("ItemRewardList");

                    ResetRewardToggle(EnDungeonPlay.TradeShip);
                    RewardToggleInteractable(true);

                    Text textDescription = trImageBackground.Find("Description/TextDescription").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textDescription);
                    textDescription.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_4");

                    if (bIsClear)
                    {
                        if (m_pDItemBooty.Length == 0)
                        {
                            trRewardItemList.gameObject.SetActive(false);
                            textNoReward.gameObject.SetActive(true);
                        }
                        else
                        {
                            trRewardItemList.gameObject.SetActive(true);
                            textNoReward.gameObject.SetActive(false);

                            for (int i = 0; i < m_pDItemBooty.Length; i++)
                            {
                                trItemSlot = trRewardItemList.Find("ItemSlot" + i);

                                if (trItemSlot == null)
                                {
                                    trItemSlot = Instantiate(m_goRewardItemSlot, trRewardItemList).transform;
                                    trItemSlot.name = "ItemSlot" + i;
                                }
                                else
                                {
                                    trItemSlot.gameObject.SetActive(true);
                                }

                                CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);
                                CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_pDItemBooty[i].owned, m_pDItemBooty[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                            }
                        }
                    }
                    else
                    {
                        trRewardItemList.gameObject.SetActive(false);
                        textNoReward.gameObject.SetActive(true);
                    }

                    trTradeShip.gameObject.SetActive(true);
                }
                break;

            case EnDungeonPlay.AnkouTomb:
                {
                    Transform trImageBackground = trAnkouTomb.Find("ImageBackground");
                    m_trDungeonClear = trImageBackground;

                    textMyPoint = trImageBackground.Find("TextMyPoint").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textMyPoint);
                    textMyPoint.text = CsConfiguration.Instance.GetString("PUBLIC_DUNPOINT_4");

                    textMyPointValue = trImageBackground.Find("TextMyPointValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textMyPointValue);
                    textMyPointValue.text = CsDungeonManager.Instance.AnkouTombPoint.ToString("#,##0");

                    Text textRewardExpValue = trImageBackground.Find("RewardExp/TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardExpValue);
                    textRewardExpValue.text = m_lExpDungeonRewardExp.ToString("#,##0");

                    Text textRewardGoldValue = trImageBackground.Find("RewardGold/TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardExpValue);
                    
                    CsAnkouTombDifficulty csAnkouTombDifficulty = CsGameData.Instance.AnkouTomb.AnkouTombDifficultyList.Find(a => a.Difficulty == m_nDifficulty);

                    if (csAnkouTombDifficulty == null)
	                {
                        textRewardGoldValue.text = "";
	                }
                    else
	                {
                        textRewardGoldValue.text = csAnkouTombDifficulty.GoldReward.Value.ToString("#,##0");
	                }

                    textRewardItem = trImageBackground.Find("ImageLineTop/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardItem);
                    textRewardItem.text = CsConfiguration.Instance.GetString("A13_TXT_00013");

                    textNoReward = trImageBackground.Find("TextNoReward").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNoReward);
                    textNoReward.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_5");

                    trRewardItemList = trImageBackground.Find("ItemRewardList");

                    ResetRewardToggle(EnDungeonPlay.AnkouTomb);
                    RewardToggleInteractable(true);

                    Text textDescription = trImageBackground.Find("Description/TextDescription").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textDescription);
                    textDescription.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_4");

                    if (bIsClear)
                    {
                        if (m_pDItemBooty.Length == 0)
                        {
                            trRewardItemList.gameObject.SetActive(false);
                            textNoReward.gameObject.SetActive(true);
                        }
                        else
                        {
                            trRewardItemList.gameObject.SetActive(true);
                            textNoReward.gameObject.SetActive(false);

                            for (int i = 0; i < m_pDItemBooty.Length; i++)
                            {
                                trItemSlot = trRewardItemList.Find("ItemSlot" + i);

                                if (trItemSlot == null)
                                {
                                    trItemSlot = Instantiate(m_goRewardItemSlot, trRewardItemList).transform;
                                    trItemSlot.name = "ItemSlot" + i;
                                }
                                else
                                {
                                    trItemSlot.gameObject.SetActive(true);
                                }

                                CsItem csItem = CsGameData.Instance.GetItem(m_pDItemBooty[i].id);
                                CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, m_pDItemBooty[i].owned, m_pDItemBooty[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                            }
                        }
                    }
                    else
                    {
                        trRewardItemList.gameObject.SetActive(false);
                        textNoReward.gameObject.SetActive(true);
                    }

                    trAnkouTomb.gameObject.SetActive(true);
                }
                break;
        }

        buttonExit.onClick.AddListener(() => OnClickExitDungeon(enDungeonPlay));
        m_trNewPopupDungeonClear.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleReward(bool bIson, int nRewardIndex, EnDungeonPlay enDungeonPlay)
    {
        if (bIson)
        {
            int nUnOwnDia = 0;
            string strMessage = string.Empty;

            switch (nRewardIndex)
            {
                case 1:

                    if (enDungeonPlay == EnDungeonPlay.TradeShip)
                    {
                        nUnOwnDia = CsGameData.Instance.TradeShip.Exp2xRewardRequiredUnOwnDia;
                    }
                    else if (enDungeonPlay == EnDungeonPlay.AnkouTomb)
                    {
                        nUnOwnDia = CsGameData.Instance.AnkouTomb.Exp2xRewardRequiredUnOwnDia;
                    }

                    strMessage = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_6"), nUnOwnDia);
                    strMessage += "\n" + CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_8");

                    if (enDungeonPlay == EnDungeonPlay.TradeShip)
                    {
                        CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickAdditionalRewardExpReceive(enDungeonPlay, nUnOwnDia, nRewardIndex),
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), () => ResetRewardToggle(enDungeonPlay), true);
                    }
                    else if (enDungeonPlay == EnDungeonPlay.AnkouTomb)
                    {
                        CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickAdditionalRewardExpReceive(enDungeonPlay, nUnOwnDia, nRewardIndex),
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), () => ResetRewardToggle(enDungeonPlay), true);
                    }

                    break;

                case 2:

                    if (enDungeonPlay == EnDungeonPlay.TradeShip)
                    {
                        nUnOwnDia = CsGameData.Instance.TradeShip.Exp3xRewardRequiredUnOwnDia;
                    }
                    else if (enDungeonPlay == EnDungeonPlay.AnkouTomb)
                    {
                        nUnOwnDia = CsGameData.Instance.AnkouTomb.Exp3xRewardRequiredUnOwnDia;
                    }

                    strMessage = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_7"), nUnOwnDia);
                    strMessage += "\n" + CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_8");

                    if (enDungeonPlay == EnDungeonPlay.TradeShip)
                    {
                        CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickAdditionalRewardExpReceive(enDungeonPlay, nUnOwnDia, nRewardIndex),
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), () => ResetRewardToggle(enDungeonPlay), true);
                    }
                    else if (enDungeonPlay == EnDungeonPlay.AnkouTomb)
                    {
                        CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickAdditionalRewardExpReceive(enDungeonPlay, nUnOwnDia, nRewardIndex),
                                                              CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), () => ResetRewardToggle(enDungeonPlay), true);
                    }

                    break;
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAdditionalRewardExpReceive(EnDungeonPlay enDungeonPlay, int nUnOwnDia, int nRewardIndex)
    {
        if (CsGameData.Instance.MyHeroInfo.UnOwnDia < nUnOwnDia)
        {
            ResetRewardToggle(enDungeonPlay);
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_9"));
        }
        else
        {
            if (enDungeonPlay == EnDungeonPlay.TradeShip)
            {
                CsDungeonManager.Instance.SendTradeShipAdditionalRewardExpReceive(nRewardIndex);
            }
            else if (enDungeonPlay == EnDungeonPlay.AnkouTomb)
            {
                CsDungeonManager.Instance.SendAnkouTombAdditionalRewardExpReceive(nRewardIndex);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupDungeonClear()
    {
        if (m_trDungeonClear == null)
        {
            return;
        }
        else
        {
            Text textRewardExpValue = m_trDungeonClear.Find("RewardExp/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRewardExpValue);
            textRewardExpValue.text = m_lExpDungeonRewardExp.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResetRewardToggle(EnDungeonPlay enDungeonPlay)
    {
        if (m_trDungeonClear == null)
        {
            return;
        }
        else
        {
            Transform trRewardList = m_trDungeonClear.Find("RewardList");
            Toggle toggleReward = null;
            Text textToggleReward = null;

            for (int i = 0; i < trRewardList.childCount; i++)
            {
                toggleReward = trRewardList.Find("Toggle" + i).GetComponent<Toggle>();
                
                if (toggleReward == null)
                {
                    continue;
                }
                else
                {
                    int nIndex = i;

                    toggleReward.onValueChanged.RemoveAllListeners();
                    toggleReward.group = trRewardList.GetComponent<ToggleGroup>();

                    if (nIndex == 0)
                    {
                        toggleReward.isOn = true;
                    }
                    else
                    {
                        toggleReward.isOn = false;
                    }

                    toggleReward.onValueChanged.AddListener((ison) => OnValueChangedToggleReward(ison, nIndex, enDungeonPlay));

                    textToggleReward = toggleReward.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textToggleReward);

                    if (i == 0)
                    {
                        textToggleReward.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_1");
                    }
                    else if (i == 1)
                    {
                        textToggleReward.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_2");
                    }
                    else
                    {
                        textToggleReward.text = CsConfiguration.Instance.GetString("PUBLIC_DUNCOMPLE_3");
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RewardToggleInteractable(bool bInteractable)
    {
        if (m_trDungeonClear == null)
        {
            return;
        }
        else
        {
            Transform trRewardList = m_trDungeonClear.Find("RewardList");
            Toggle toggleReward = null;

            for (int i = 0; i < trRewardList.childCount; i++)
            {
                toggleReward = trRewardList.GetChild(i).GetComponent<Toggle>();

                if (toggleReward == null)
                {
                    continue;
                }
                else
                {
                    toggleReward.interactable = bInteractable;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTeamInvite(Guid guidHeroId)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNextFloor()
    {
        CsDungeonManager.Instance.SendArtifactRoomNextFloorChallenge();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExitDungeon(EnDungeonPlay enDungeonPlay)
    {
        Debug.Log("#@#@ OnClickExitDungeon : " + enDungeonPlay);
        switch (enDungeonPlay)
        {
            case EnDungeonPlay.MainQuest:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsMainQuestDungeonManager.Instance.SendMainQuestDungeonExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.Story:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendStoryDungeonExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.Exp:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendExpDungeonExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.Gold:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendGoldDungeonExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.OsirisRoom:
                
                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendOsirisRoomExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.ArtifactRoom:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendArtifactRoomExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.AncientRelic:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendAncientRelicExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.FieldOfHonor:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendFieldOfHonorExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.SoulCoveter:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendSoulCoveterExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.Elite:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendEliteDungeonExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.ProofOfValor:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendProofOfValorExit();
                }
                else
                {
                    return;
                }

				break;

            case EnDungeonPlay.WisdomTemple:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendWisdomTempleExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.RuinsReclaim:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendRuinsReclaimExit();
                }
                else
                {
                    return;
                }

				break;

            case EnDungeonPlay.InfiniteWar:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendInfiniteWarExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.FearAltar:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendFearAltarExit();
                }
                else
                {
                    return;
                }

				break;

            case EnDungeonPlay.WarMemory:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendWarMemoryExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.Biography:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendBiographyQuestDungeonExit();
                }
                else
                {
                    return;
                }

				break;

            case EnDungeonPlay.DragonNest:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendDragonNestExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.TradeShip:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendTradeShipExit();
                }
                else
                {
                    return;
                }

                break;

            case EnDungeonPlay.AnkouTomb:

                if (CsDungeonManager.Instance.DungeonPlay == enDungeonPlay)
                {
                    CsDungeonManager.Instance.SendAnkouTombExit();
                }
                else
                {
                    return;
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupDungeonBossAppear(string strBossName)
    {
        Transform trBossAppear = transform.Find("PopupBoss");

        Button buttonSkip = trBossAppear.Find("ButtonSkip").GetComponent<Button>();
        buttonSkip.onClick.RemoveAllListeners();
        buttonSkip.onClick.AddListener(OnClickBossAppearSkip);

        Text textSkip = buttonSkip.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSkip);
        textSkip.text = CsConfiguration.Instance.GetString("A13_TXT_00008");

        Transform trDungeonBoss = trBossAppear.Find("ImageDungeonBoss");

        Text textBossName = trDungeonBoss.Find("TextBossName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBossName);
        textBossName.text = strBossName;
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickRuinsReclaimRewardCard(int nButtonIndex, PDItemBooty randomBooty)
	{
		// 랜덤 보상 -> 카드 선택 후 세팅
		Transform trRewardCardList = m_trPopupRuinsReclaimClear.Find("FrameTop/RewardCardList");

		var enumerator = CsGameData.Instance.RuinsReclaim.RuinsReclaimRandomRewardPoolEntryList.GetEnumerator();

		for (int i = 0; i < trRewardCardList.childCount; i++)
		{
			Transform trFrameCard = trRewardCardList.GetChild(i);
			trFrameCard.Find("ImageButtonBack").gameObject.SetActive(false);

			Transform trCard;
			CsItem csItem;

			if (i == nButtonIndex)
			{
				trCard = trFrameCard.Find("ImageSelected");
				csItem = CsGameData.Instance.GetItem(randomBooty.id);

				CsUIData.Instance.DisplayItemSlot(trCard.Find("ItemSlot"), csItem, randomBooty.owned, randomBooty.count, false);
			}
			else
			{
				trCard = trFrameCard.Find("ImageFront");

				enumerator.MoveNext();
				csItem = enumerator.Current.ItemReward.Item;

				if (csItem.ItemId == randomBooty.id &&
					CsGameData.Instance.RuinsReclaim.RuinsReclaimRandomRewardPoolEntryList[i].ItemReward.ItemCount == randomBooty.count)
				{
					enumerator.MoveNext();
					csItem = enumerator.Current.ItemReward.Item;
				}

				CsUIData.Instance.DisplayItemSlot(trCard.Find("ItemSlot"), csItem, enumerator.Current.ItemReward.ItemOwned, enumerator.Current.ItemReward.ItemCount, false);
			}

			Text textName = trCard.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textName);
			textName.text = csItem.Name;

			trCard.gameObject.SetActive(true);
		}

		Button buttonExit = m_trPopupRuinsReclaimClear.Find("ButtonExit").GetComponent<Button>();
		buttonExit.interactable = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonTargetHalidomMonster()
	{
		CsDungeonManager.Instance.OnEventClickFearAltarTargetHalidomMonsterTargetButton();

		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonShowHalidomCollection()
	{
		m_trButtonShowHalidomCollection.gameObject.SetActive(false);

		CsGameEventUIToUI.Instance.OnEventOpenPopupHalidomCollection(false);

		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
	}

    //---------------------------------------------------------------------------------------------------
    void CreateDungeonStartCount(float flTime)
    {
        if (0.0f < flTime && flTime < 4.0f && m_trPopupDungeonStartCount == null)
        {
            if (m_goPopupDungeonStartCount == null)
            {
                StartCoroutine(LoadDungeonStartCount(flTime));
            }
            else
            {
                OpenPopupDungeonStartCount(flTime);
            }
        }
        else
        {
            m_trPopupDungeonStartCount = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDungeonStartCount(float flTime)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/PopupDungeonStartCount");
        yield return resourceRequest;

        m_goPopupDungeonStartCount = (GameObject)resourceRequest.asset;
        OpenPopupDungeonStartCount(flTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupDungeonStartCount(float flTime)
    {
        if (m_trPopupDungeonStartCount == null)
        {
            m_trPopupDungeonStartCount = Instantiate(m_goPopupDungeonStartCount, m_trPopup).transform;
            m_trPopupDungeonStartCount.name = "PopupDungeonStartCount";
            m_trPopupDungeonStartCount.GetComponent<CsPopupDungeonStartCount>().DisplayStartCount(flTime);
        }
        else
        {
            return;
        }
    }
}