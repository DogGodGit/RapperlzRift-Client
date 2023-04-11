using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-28)
//---------------------------------------------------------------------------------------------------

public enum EnBattleMode
{
    None = 1,
    Auto,
    Manual,
}

public class CsPanelConsumItemMenu : MonoBehaviour
{
    Transform m_trpotionBuyText;
    //Transform m_trScrollBuyText;
    Transform m_trPopupList;
    Transform m_trDistortion;
    Transform m_trButtonList;
    Transform m_trPopupQuickMenu;
    Transform m_trImageDungeonEnterShortCut;

    GameObject m_goPopupCalculator;
    GameObject m_goPopupQuickMenu;

    Button m_buttonpotion;
    Button m_buttonQuickMenu;
    //Button m_buttonReturnScroll;
    Button m_buttonChangeTarget;

    Toggle m_toggleRide;
    Button m_buttonAutoBattle;

    Text m_textpotionCount;
    Text m_textpotionCooltime;
    //Text m_textReturnScrollCount;
    //Text m_textReturnScrollCooltime;
    Text m_textDistortionTime;
    Text m_textGuideTime;

    Image m_imagepotion;
    //Image m_imageReturnScroll;

    int m_nPotionCount = 0;
    //int m_nReturnScrollCount = 0;

    float m_flTime = 0;
	float m_flTimePotion = 0;

    bool m_bPotionOpen = false;
    //bool m_bReturnScrollOpen = false;

    CsPopupCalculator m_csPopupCalculator;
    EnItemType m_EnItemTypeBuy;

    EnBattleMode m_enBattleMode = EnBattleMode.None;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventHpPotionUse += OnEventHpPotionUse;
        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished += OnEventReturnScrollUseFinished;

        CsGameEventUIToUI.Instance.EventSimpleShopBuy += OnEventSimpleShopBuy;
        CsGameEventUIToUI.Instance.EventSimpleShopSell += OnEventSimpleShopSell;

        CsGameEventUIToUI.Instance.EventDropObjectLooted += OnEventDropObjectLooted;
        CsGameEventToUI.Instance.EventChangeAutoBattleMode += OnEventChangeAutoBattleMode;

        CsGameEventUIToUI.Instance.EventLevelUpRewardReceive += OnEventLevelUpRewardReceive;
        CsGameEventUIToUI.Instance.EventDailyAccessTimeRewardReceive += OnEventDailyAccessTimeRewardReceive;
        CsGameEventUIToUI.Instance.EventAttendRewardReceive += OnEventAttendRewardReceive;
        CsGameEventUIToUI.Instance.EventMailReceive += OnEventMailReceive;
        CsGameEventUIToUI.Instance.EventMailReceiveAll += OnEventMailReceiveAll;

		// 메인퀘스트 카트 타입, 물자지원, 길드 물자지원 퀘스트 시 탑승 버튼 제거 & 탑승 토글 끄기
		// 완료 시 리셋
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
        CsMainQuestManager.Instance.EventAccepted += OnEventAccepted;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept += OnEventSupplySupportQuestAccept;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;

		CsGuildManager.Instance.EventGuildSupplySupportQuestAccept += OnEventGuildSupplySupportQuestAccept;
		CsGuildManager.Instance.EventGuildSupplySupportQuestComplete += OnEventGuildSupplySupportQuestComplete;
		
        CsGameEventToUI.Instance.EventMountGetOff += OnEventMountGetOff;

        CsGameEventUIToUI.Instance.EventDisplayMountToggleIsOn += OnEventDisplayMountToggleIsOn;

        //던전 나가거나 들어갈때 탈것 버튼 감추기.
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter += OnEventMainQuestDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonEnter += OnEventStoryDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonEnter += OnEventExpDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonEnter += OnEventGoldDungeonEnter;
        CsDungeonManager.Instance.EventArtifactRoomEnter += OnEventArtifactRoomEnter;
        CsDungeonManager.Instance.EventAncientRelicEnter += OnEventAncientRelicEnter;
        CsDungeonManager.Instance.EventFieldOfHonorChallenge += OnEventFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventProofOfValorEnter += OnEventProofOfValorEnter;
		CsDungeonManager.Instance.EventWisdomTempleEnter += OnEventWisdomTempleEnter;
		CsDungeonManager.Instance.EventRuinsReclaimEnter += OnEventRuinsReclaimEnter;
        CsDungeonManager.Instance.EventInfiniteWarEnter += OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventWarMemoryEnter += OnEventWarMemoryEnter;
        CsDungeonManager.Instance.EventOsirisRoomEnter += OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventDragonNestEnter += OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventTradeShipEnter += OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventAnkouTombEnter += OnEventAnkouTombEnter;

        CsGameEventToUI.Instance.EventPrevContinentEnter += OnEventPrevContinentEnter;
        CsGameEventUIToUI.Instance.EventMountEquip += OnEventMountEquip;

        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete += OnEventBountyHunterQuestComplete;

        CsGameEventUIToUI.Instance.EventDistortionCanceled += OnEventDistortionCanceled;
        CsGameEventUIToUI.Instance.EventDistortionScrollUse += OnEventDistortionScrollUse;

        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;

        CsCartManager.Instance.EventMyHeroCartGetOff += OnEventMyHeroCartGetOff;
        CsCartManager.Instance.EventMyHeroCartGetOn += OnEventMyHeroCartGetOn;

        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationStart += OnEventRuinsReclaimMonsterTransformationStart;
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationFinished += OnEventRuinsReclaimMonsterTransformationFinished;
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		CsDungeonManager.Instance.EventRuinsReclaimWaveCompleted += OnEventRuinsReclaimWaveCompleted;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;
		CsDungeonManager.Instance.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;
		CsDungeonManager.Instance.EventRuinsReclaimExit += OnEventRuinsReclaimExit;

        CsGameEventUIToUI.Instance.EventLimitationGiftRewardReceive += OnEventLimitationGiftRewardReceive;

		CsBiographyManager.Instance.EventBiographyComplete += OnEventBiographyComplete;

        // 임시 물약 아이템 사용
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRankingRewardReceive += OnEventNationWeeklyPresentPopularityPointRankingRewardReceive;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRankingRewardReceive += OnEventNationWeeklyPresentContributionPointRankingRewardReceive;
        CsGameEventUIToUI.Instance.EventWingMemoryPieceInstall += OnEventWingMemoryPieceInstall;
        CsGameEventUIToUI.Instance.EventDiaShopProductBuy += OnEventDiaShopProductBuy;

        CsGameEventUIToUI.Instance.EventChangeAutoBattleMode += OnEventChangeAutoBattleMode;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        //포션 버튼
        if (m_nPotionCount > 0)
        {
            if (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup > 0)
            {
                UpdatePotionCoolTime();
            }
            else
            {
                CsUIData.Instance.HpPotionRemainingCoolTime = 0;
                m_textpotionCooltime.text = "";
            }
        }
        else
        {
            m_imagepotion.fillAmount = 1;
        }

        /*
        //귀환 스크롤 버튼
        if (m_nReturnScrollCount > 0)
        {
            if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup > 0)
            {
                UpdateReturnScrollCoolTime();
            }
            else
            {
                CsUIData.Instance.ReturnScrollRemainingCoolTime = 0;
                m_textReturnScrollCooltime.text = "";
            }
        }
        else
        {
            m_imageReturnScroll.fillAmount = 1;
        }

        if (CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup > 0)
        {
            UpdateReturnScrollCastTime();
        }
        else
        {
            CsUIData.Instance.ReturnScrollRemainingCastTime = 0;
        }
        */
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_textDistortionTime != null && CsGameData.Instance.MyHeroInfo.RemainingDistortionTime - Time.realtimeSinceStartup > 0)
            {
                UpdateDistortion();
            }
            else
            {
                if (m_trDistortion != null && m_trDistortion.gameObject.activeSelf)
                {
                    m_trDistortion.gameObject.SetActive(false);
                }
            }

            UpdateTextGuideTime();
            DisplayDungeonEnterShortCut();
			
            m_flTime = Time.time;
        }

		if (m_flTimePotion + 5.0f < Time.time)
		{
			UpdatePotionCount();

			m_flTimePotion = Time.time;
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventHpPotionUse -= OnEventHpPotionUse;
        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished -= OnEventReturnScrollUseFinished;

        CsGameEventUIToUI.Instance.EventSimpleShopBuy -= OnEventSimpleShopBuy;
        CsGameEventUIToUI.Instance.EventSimpleShopSell -= OnEventSimpleShopSell;

        CsGameEventUIToUI.Instance.EventDropObjectLooted -= OnEventDropObjectLooted;
        CsGameEventToUI.Instance.EventChangeAutoBattleMode -= OnEventChangeAutoBattleMode;

        CsGameEventUIToUI.Instance.EventLevelUpRewardReceive -= OnEventLevelUpRewardReceive;
        CsGameEventUIToUI.Instance.EventDailyAccessTimeRewardReceive -= OnEventDailyAccessTimeRewardReceive;
        CsGameEventUIToUI.Instance.EventAttendRewardReceive -= OnEventAttendRewardReceive;
        CsGameEventUIToUI.Instance.EventMailReceive -= OnEventMailReceive;
        CsGameEventUIToUI.Instance.EventMailReceiveAll -= OnEventMailReceiveAll;

        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
        CsMainQuestManager.Instance.EventAccepted -= OnEventAccepted;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept -= OnEventSupplySupportQuestAccept;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;

        CsGameEventToUI.Instance.EventMountGetOff -= OnEventMountGetOff;

        CsGameEventUIToUI.Instance.EventDisplayMountToggleIsOn -= OnEventDisplayMountToggleIsOn;

        //던전 나가거나 들어갈때 탈것 버튼 감추기.
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter -= OnEventMainQuestDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonEnter -= OnEventStoryDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonEnter -= OnEventExpDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonEnter -= OnEventGoldDungeonEnter;
        CsDungeonManager.Instance.EventArtifactRoomEnter -= OnEventArtifactRoomEnter;
        CsDungeonManager.Instance.EventAncientRelicEnter -= OnEventAncientRelicEnter;
        CsDungeonManager.Instance.EventFieldOfHonorChallenge -= OnEventFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventProofOfValorEnter -= OnEventProofOfValorEnter;
		CsDungeonManager.Instance.EventWisdomTempleEnter -= OnEventWisdomTempleEnter;
        CsDungeonManager.Instance.EventRuinsReclaimEnter -= OnEventRuinsReclaimEnter;
        CsDungeonManager.Instance.EventInfiniteWarEnter -= OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventWarMemoryEnter -= OnEventWarMemoryEnter;
        CsDungeonManager.Instance.EventOsirisRoomEnter -= OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventDragonNestEnter -= OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventTradeShipEnter -= OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventAnkouTombEnter -= OnEventAnkouTombEnter;

        CsGameEventToUI.Instance.EventPrevContinentEnter -= OnEventPrevContinentEnter;
        CsGameEventUIToUI.Instance.EventMountEquip -= OnEventMountEquip;

        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete -= OnEventBountyHunterQuestComplete;

        CsGameEventUIToUI.Instance.EventDistortionCanceled -= OnEventDistortionCanceled;
        CsGameEventUIToUI.Instance.EventDistortionScrollUse -= OnEventDistortionScrollUse;

        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;

        CsCartManager.Instance.EventMyHeroCartGetOff -= OnEventMyHeroCartGetOff;
        CsCartManager.Instance.EventMyHeroCartGetOn -= OnEventMyHeroCartGetOn;

        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationStart -= OnEventRuinsReclaimMonsterTransformationStart;
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationFinished -= OnEventRuinsReclaimMonsterTransformationFinished;
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished -= OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		CsDungeonManager.Instance.EventRuinsReclaimWaveCompleted -= OnEventRuinsReclaimWaveCompleted;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;
		CsDungeonManager.Instance.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;
		CsDungeonManager.Instance.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;

        CsGameEventUIToUI.Instance.EventLimitationGiftRewardReceive -= OnEventLimitationGiftRewardReceive;

		CsBiographyManager.Instance.EventBiographyComplete -= OnEventBiographyComplete;

        // 임시 물약 아이템 사용
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRankingRewardReceive -= OnEventNationWeeklyPresentPopularityPointRankingRewardReceive;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRankingRewardReceive -= OnEventNationWeeklyPresentContributionPointRankingRewardReceive;
        CsGameEventUIToUI.Instance.EventWingMemoryPieceInstall -= OnEventWingMemoryPieceInstall;
        CsGameEventUIToUI.Instance.EventDiaShopProductBuy -= OnEventDiaShopProductBuy;

        CsGameEventUIToUI.Instance.EventChangeAutoBattleMode -= OnEventChangeAutoBattleMode;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventChangeAutoBattleMode(EnBattleMode enBattleMode)
    {
        m_enBattleMode = enBattleMode;
        SetAutoChanged(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        //UpdateContentButtonOpen(EnMenuContentId.ReturnScroll);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bSceneLoad)
    {
        //if (CsUIData.Instance.DungeonInNow != EnDungeon.None && CsUIData.Instance.DungeonInNow != EnDungeon.StoryDungeon)
        //{
        //    m_enBattleMode = EnBattleMode.Auto;
        //    SetAutoChanged();
        //}
        UpdatePotionCount();

        m_enBattleMode = EnBattleMode.None;
        SetAutoChanged();

        DisplayMainQuestCart();
        DisplaySupplySupportCart();
		DisplayGuidSupplySupportCart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroCartGetOn(ClientCommon.PDCartInstance pDCartInstance)
    {
        DisplayMainQuestCart();
        DisplaySupplySupportCart();
		DisplayGuidSupplySupportCart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroCartGetOff()
    {
        DisplayMainQuestCart();
        DisplaySupplySupportCart();
		DisplayGuidSupplySupportCart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDistortionCanceled()
    {
        m_trDistortion.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDistortionScrollUse()
    {
        UpdateDistortion();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonEnter(ClientCommon.PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonEnter(ClientCommon.PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonEnter(ClientCommon.PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonEnter(ClientCommon.PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomEnter(Guid guidPlaceInstanceId, ClientCommon.PDVector3 pDVector3, float flRotationY)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicEnter(Guid guidPlaceInstanceId, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pdHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance, Guid[] arrGuidTrapEffectHeroe, int[] arrRemovedObstacleId)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorChallenge(Guid guidPlaceInstanceId, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero pdHero)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotate, ClientCommon.PDMonsterInstance[] pDMonsterInstance)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotate)
	{
		DisplayRideToggle(false);
		DisplayRideToggleIsOn(false);
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventRuinsReclaimEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotate, ClientCommon.PDHero[] aPDHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance, ClientCommon.PDRuinsReclaimRewardObjectInstance[] aPDRuinReclaimRewardObjectInstance, ClientCommon.PDRuinsReclaimMonsterTransformationCancelObjectInstance[] aPDRuinsReclaimMonsterTransformationCancelObjectInstance, Guid[] aGuidMonsterTransformationHero)
	{
		DisplayRideToggle(false);
		DisplayRideToggleIsOn(false);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pDHeroes, ClientCommon.PDMonsterInstance[] pDMonsterInstance, ClientCommon.PDInfiniteWarBuffBoxInstance[] pDInfiniteWarBuffBoxInstance)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryEnter(Guid placeInstanceId, ClientCommon.PDVector3 position, float rotationY, ClientCommon.PDHero[] aPDHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance, ClientCommon.PDWarMemoryTransformationObjectInstance[] aPDWarMemoryTransformationObjectInstance)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomEnter(Guid placeInstanceId, ClientCommon.PDVector3 position, float flRotationY)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestEnter(Guid guidPlaceInstanceId, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] aHero, ClientCommon.PDMonsterInstance[] aMonsterInstance, Guid[] aTrapHeros)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipEnter(Guid guidPlaceInstanceId, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pdHero, ClientCommon.PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombEnter(Guid guidPlaceInstanceId, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pdHero, ClientCommon.PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        DisplayRideToggle(false);
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPrevContinentEnter()
    {
        DisplayRideToggle(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountEquip()
    {
        if (CsUIData.Instance.DungeonInNow == EnDungeon.None || CsUIData.Instance.DungeonInNow == EnDungeon.UndergroundMaze)
        {
            DisplayRideToggle(true);
        }
        else
        {
            DisplayRideToggle(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDisplayMountToggleIsOn(bool bIsOn)
    {
        DisplayRideToggleIsOn(bIsOn);
    }
    //---------------------------------------------------------------------------------------------------
    void OnEventMountGetOff()
    {
        DisplayRideToggleIsOn(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventLevelUpRewardReceive()
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDailyAccessTimeRewardReceive()
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAttendRewardReceive()
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceive(Guid guidMail)
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceiveAll(Guid[] guidMails)
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csOldMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
        ResetCart();
        UpdateContentButtonOpen(EnMenuContentId.HpPotion);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
    {
        DisplayMainQuestCart(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestAccept(ClientCommon.PDSupplySupportQuestCartInstance pDSupplySupportQuestCartInstance)
    {
		DisplaySupplySupportCart(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp, long lGold, int nAcquiredExploitPoint)
    {
        ResetCart();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildSupplySupportQuestAccept(ClientCommon.PDGuildSupplySupportQuestCartInstance pdGuildSupplySupportQuestCartInstance)
	{
		DisplayGuidSupplySupportCart(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		ResetCart();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventDropObjectLooted(List<CsDropObject> listLooted, List<CsDropObject> listNotLooted)
    {
        if (listLooted.Count > 0)
        {
            UpdatePotionCount();
            //UpdateReturnScrollCount();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopBuy()
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopSell()
    {
        UpdatePotionCount();
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHpPotionUse(int nRecoveryHp)
    {
        UpdatePotionCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseFinished(int nContinentId)
    {
        //UpdateReturnScrollCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        if (CsBountyHunterQuestManager.Instance.Auto)
        {
            m_enBattleMode = EnBattleMode.Auto;
            SetAutoChanged();
        }
    }

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationStart(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		DisplayAutoBattleButton(false);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationFinished()
	{
		DisplayAutoBattleButton(true);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(long lInstanceId)
	{
		DisplayAutoBattleButton(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimWaveCompleted()
	{
		DisplayAutoBattleButton(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nContinentId)
	{
		DisplayAutoBattleButton(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nContinentId)
	{
		DisplayAutoBattleButton(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nContinentId)
	{
		DisplayAutoBattleButton(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyComplete(int nBiographyId)
	{
		UpdatePotionCount();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventLimitationGiftRewardReceive()
    {
        UpdatePotionCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentPopularityPointRankingRewardReceive()
    {
        UpdatePotionCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentContributionPointRankingRewardReceive()
    {
        UpdatePotionCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWingMemoryPieceInstall()
    {
        UpdatePotionCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDiaShopProductBuy()
    {
        UpdatePotionCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPotion()
    {
        if (m_nPotionCount > 0)
        {
            CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.HpPotionId);

            if (csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= csItem.RequiredMaxHeroLevel)
            {
                if (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup <= 0 && CsGameData.Instance.MyHeroInfo.Hp < CsGameData.Instance.MyHeroInfo.MaxHp)
                {
                    CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(CsUIData.Instance.HpPotionId);

                    if (csInventorySlot != null)
                    {
                        CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                    }
                }
                else if(CsGameData.Instance.MyHeroInfo.Hp >= CsGameData.Instance.MyHeroInfo.MaxHp)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_HPFULL"));
                }
            }
        }
        else
        {
            //구매창호출
            CsSimpleShopProduct csSimpleShopProduct = CsGameData.Instance.SimpleShopProductList.Find(a => a.Item.ItemId == CsUIData.Instance.HpPotionId);

            if (csSimpleShopProduct != null)
            {
                m_EnItemTypeBuy = EnItemType.HpPotion;
                OpenShop(csSimpleShopProduct);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQuickMenu()
    {
        OpenPopupQuickMenu();
    }
    
    /*
    //---------------------------------------------------------------------------------------------------
    void OnClickReturnScroll()
    {
        if (m_nReturnScrollCount > 0)
        {
            if (CsUIData.Instance.DungeonInNow != EnDungeon.None && CsUIData.Instance.DungeonInNow != EnDungeon.UndergroundMaze)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A55_TXT_02005"));
                return;
            }

            CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.ReturnScrollId);

            if (csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= csItem.RequiredMaxHeroLevel)
            {
                if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup <= 0 && CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup <= 0)
                {
                    CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(CsUIData.Instance.ReturnScrollId);

                    if (csInventorySlot != null)
                    {
                        CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                    }
                }
            }
        }
        else
        {
            CsSimpleShopProduct csSimpleShopProduct = CsGameData.Instance.SimpleShopProductList.Find(a => a.Item.ItemId == CsUIData.Instance.ReturnScrollId);

            if (csSimpleShopProduct != null)
            {
                m_EnItemTypeBuy = EnItemType.ReturnScroll;
                OpenShop(csSimpleShopProduct);
            }
        }
    }
    */

    //---------------------------------------------------------------------------------------------------
    void OnClickChangeTarget()
    {
        CsGameEventToIngame.Instance.OnEventTab();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickChangedAutoBattle()
    {
        if(m_enBattleMode == EnBattleMode.Manual)
        {
            m_enBattleMode = EnBattleMode.None;
        }
        else
        {
            m_enBattleMode++;
        }

        SetAutoChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedRide(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsCommandEventManager.Instance.SendMountGetOn();
        }
        else
        {
            CsGameEventToIngame.Instance.OnEventMountGetOff();
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

        m_trButtonList = transform.Find("ButtonList");

        m_buttonpotion = m_trButtonList.Find("ButtonPotion").GetComponent<Button>();
        m_buttonpotion.onClick.RemoveAllListeners();
        m_buttonpotion.onClick.AddListener(OnClickPotion);
        m_buttonpotion.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textpotionCount = m_buttonpotion.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textpotionCount);

        m_textpotionCooltime = m_buttonpotion.transform.Find("TextCooltime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textpotionCooltime);

        m_trpotionBuyText = m_buttonpotion.transform.Find("TextBuy");
        Text textBuypotion = m_trpotionBuyText.GetComponent<Text>();
        CsUIData.Instance.SetFont(textBuypotion);
        textBuypotion.text = CsConfiguration.Instance.GetString("A02_BTN_00010");

        m_imagepotion = m_buttonpotion.transform.Find("ImageCooltime").GetComponent<Image>();

        m_buttonQuickMenu = m_trButtonList.Find("ButtonQuickMenu").GetComponent<Button>();
        m_buttonQuickMenu.onClick.RemoveAllListeners();
        m_buttonQuickMenu.onClick.AddListener(OnClickQuickMenu);
        m_buttonQuickMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        /*
        m_buttonReturnScroll = m_trButtonList.Find("ButtonReturnScroll").GetComponent<Button>();
        m_buttonReturnScroll.onClick.RemoveAllListeners();
        m_buttonReturnScroll.onClick.AddListener(OnClickReturnScroll);
        m_buttonReturnScroll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textReturnScrollCount = m_buttonReturnScroll.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textReturnScrollCount);

        m_textReturnScrollCooltime = m_buttonReturnScroll.transform.Find("TextCooltime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textReturnScrollCooltime);

        m_trScrollBuyText = m_buttonReturnScroll.transform.Find("TextBuy");
        Text textBuyScroll = m_trScrollBuyText.GetComponent<Text>();
        CsUIData.Instance.SetFont(textBuyScroll);
        textBuyScroll.text = CsConfiguration.Instance.GetString("A02_BTN_00010");

        m_imageReturnScroll = m_buttonReturnScroll.transform.Find("ImageCooltime").GetComponent<Image>();
        */

        m_buttonChangeTarget = m_trButtonList.Find("ButtonCahngeTarget").GetComponent<Button>();
        m_buttonChangeTarget.onClick.RemoveAllListeners();
        m_buttonChangeTarget.onClick.AddListener(OnClickChangeTarget);
        m_buttonChangeTarget.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonAutoBattle = transform.Find("ButtonBattle").GetComponent<Button>();
        m_buttonAutoBattle.onClick.RemoveAllListeners();
        m_buttonAutoBattle.onClick.AddListener(OnClickChangedAutoBattle);
        m_buttonAutoBattle.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonAuto = m_buttonAutoBattle.transform.Find("Text").GetComponent<Text>();
        textButtonAuto.text = CsConfiguration.Instance.GetString("A04_BTN_00004");
        CsUIData.Instance.SetFont(textButtonAuto);

        m_toggleRide = transform.Find("ToggleRide").GetComponent<Toggle>();
        m_toggleRide.onValueChanged.RemoveAllListeners();
        m_toggleRide.onValueChanged.AddListener((ison) => OnValueChangedRide(m_toggleRide));
        m_toggleRide.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        m_trDistortion = transform.Find("PopupDistortion");

        m_textDistortionTime = m_trDistortion.Find("TextTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDistortionTime);

        if (CsGameData.Instance.MyHeroInfo.RemainingDistortionTime - Time.realtimeSinceStartup > 0)
        {
            UpdateDistortion();
        }
        else
        {
            m_trDistortion.gameObject.SetActive(false);
        }

        CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.HpPotionId);
        CsUIData.Instance.HpPotionCoolTime = csItem.Value1;

        csItem = CsGameData.Instance.GetItem(CsUIData.Instance.ReturnScrollId);
        CsUIData.Instance.ReturnScrollCoolTime = csItem.Value1;

        Button buttonCloseDistortion = m_trDistortion.Find("ButtonClose").GetComponent<Button>();
        buttonCloseDistortion.onClick.RemoveAllListeners();
        buttonCloseDistortion.onClick.AddListener(OnClickCloseDistortion);
        buttonCloseDistortion.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        DisplayRideToggleIsOn(CsGameData.Instance.MyHeroInfo.IsRiding);
        DisplayRideToggle(true);
        UpdateContentButtonOpen(EnMenuContentId.HpPotion);
        //UpdateContentButtonOpen(EnMenuContentId.ReturnScroll);

        m_textGuideTime = transform.Find("TextGuideTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGuideTime);

        m_trImageDungeonEnterShortCut = transform.Find("ImageDungeonEnterShortCut");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDistortion()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.MyHeroInfo.RemainingDistortionTime - Time.realtimeSinceStartup);
        m_textDistortionTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), timeSpan.Minutes.ToString("0#"), timeSpan.Seconds.ToString("0#"));

        if (!m_trDistortion.gameObject.activeSelf)
        {
            m_trDistortion.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseDistortion()
    {
        CsCommandEventManager.Instance.SendDistortionCancel();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayRideToggle(bool bIsOn)
    {
        if (bIsOn && CsGameData.Instance.MyHeroInfo.EquippedMountId != 0)
        {
            m_toggleRide.gameObject.SetActive(true);
        }
        else
        {
            m_toggleRide.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayRideToggleIsOn(bool bIsOn)
    {
        m_toggleRide = transform.Find("ToggleRide").GetComponent<Toggle>();
        m_toggleRide.onValueChanged.RemoveAllListeners();
        m_toggleRide.isOn = bIsOn;
        m_toggleRide.onValueChanged.AddListener((ison) => OnValueChangedRide(m_toggleRide));
        m_toggleRide.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePotionCount()
    {
        m_nPotionCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsUIData.Instance.HpPotionId);
        if (m_bPotionOpen)
        {
            if (m_nPotionCount > 0)
            {
                m_textpotionCount.text = m_nPotionCount.ToString("#,##0");
                m_imagepotion.fillAmount = 0;
                m_trpotionBuyText.gameObject.SetActive(false);
            }
            else
            {
                m_textpotionCount.text = "";
                m_imagepotion.fillAmount = 1;
                m_trpotionBuyText.gameObject.SetActive(true);
            }
        }
        else
        {
            m_textpotionCount.text = "";
            m_imagepotion.fillAmount = 0;
            m_trpotionBuyText.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateContentButtonOpen(EnMenuContentId enMenuContentId)
    {
        Transform trPotionLock = m_buttonpotion.transform.Find("ImageLock");

        m_buttonpotion.interactable = m_bPotionOpen = CsUIData.Instance.MenuContentOpen((int)enMenuContentId);
        trPotionLock.gameObject.SetActive(!m_bPotionOpen);
        UpdatePotionCount();

        /*
        Transform trReturnScrollLock = m_buttonReturnScroll.transform.Find("ImageLock");

        switch (enMenuContentId)
        {
            case EnMenuContentId.HpPotion:
                m_buttonpotion.interactable = m_bPotionOpen = CsUIData.Instance.MenuContentOpen((int)enMenuContentId);
                trPotionLock.gameObject.SetActive(!m_bPotionOpen);
                UpdatePotionCount();
                break;

            case EnMenuContentId.ReturnScroll:
                m_buttonReturnScroll.interactable = m_bReturnScrollOpen = CsUIData.Instance.MenuContentOpen((int)enMenuContentId);
                trReturnScrollLock.gameObject.SetActive(!m_bReturnScrollOpen);
                //UpdateReturnScrollCount();
                break;
        }
        */
    }

    /*
    //---------------------------------------------------------------------------------------------------
    void UpdateReturnScrollCount()
    {
        if (m_bReturnScrollOpen)
        {
            m_nReturnScrollCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsUIData.Instance.ReturnScrollId);

            if (m_nReturnScrollCount > 0)
            {
                m_textReturnScrollCount.text = m_nReturnScrollCount.ToString("#,##0");
                m_imageReturnScroll.fillAmount = 0;
                m_trScrollBuyText.gameObject.SetActive(false);
            }
            else
            {
                m_textReturnScrollCount.text = "";
                m_imageReturnScroll.fillAmount = 1;
                m_trScrollBuyText.gameObject.SetActive(true);
            }
        }
        else
        {
            m_textReturnScrollCount.text = "";
            m_imageReturnScroll.fillAmount = 0;
            m_trScrollBuyText.gameObject.SetActive(false);
        }
    }
    */

    //---------------------------------------------------------------------------------------------------
    void UpdatePotionCoolTime()
    {
        float flRemainingTime = CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup;

        if (flRemainingTime < 0)
            flRemainingTime = 0;

        Image imagepotion = m_buttonpotion.transform.Find("ImageCooltime").GetComponent<Image>();
        imagepotion.fillAmount = flRemainingTime / CsUIData.Instance.HpPotionCoolTime;
        m_textpotionCooltime.text = flRemainingTime.ToString("#0");
    }

    /*
    //---------------------------------------------------------------------------------------------------
    void UpdateReturnScrollCoolTime()
    {
        float flRemainingTime = CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup;

        if (flRemainingTime < 0)
            flRemainingTime = 0;

        Image imageReturnScroll = m_buttonReturnScroll.transform.Find("ImageCooltime").GetComponent<Image>();
        imageReturnScroll.fillAmount = flRemainingTime / CsUIData.Instance.ReturnScrollCoolTime;
        m_textReturnScrollCooltime.text = flRemainingTime.ToString("#0");
    }
    */

    //---------------------------------------------------------------------------------------------------
    void UpdateReturnScrollCastTime()
    {
        float flRemainingTime = CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup;

        if (flRemainingTime < 0)
            flRemainingTime = 0;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenShop(CsSimpleShopProduct csSimpleShopProduct)
    {
        if (m_goPopupCalculator == null)
        {
            StartCoroutine(LoadPopupCalculatorCoroutine(() => OpenPopupBuyItem(csSimpleShopProduct)));
        }
        else
        {
            OpenPopupBuyItem(csSimpleShopProduct);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCalculatorCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupBuyItem(CsSimpleShopProduct csSimpleShopProduct)
    {
        Transform trPopup = m_trPopupList.Find("PopupCalculator");

        if (trPopup == null)
        {
            GameObject goPopupBuyCount = Instantiate(m_goPopupCalculator, m_trPopupList);
            goPopupBuyCount.name = "PopupCalculator";
            trPopup = goPopupBuyCount.transform;
        }
        else
        {
            trPopup.gameObject.SetActive(false);
        }

        m_csPopupCalculator = trPopup.GetComponent<CsPopupCalculator>();
        m_csPopupCalculator.EventBuyItem += OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;
        m_csPopupCalculator.DisplayItem(csSimpleShopProduct.Item, csSimpleShopProduct.ItemOwned, csSimpleShopProduct.SaleGold, EnResourceType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBuyItem(int nCount)
    {
        List<CsSimpleShopProduct> listcsSimpleShopProduct;

        switch (m_EnItemTypeBuy)
        {
            case EnItemType.HpPotion:
                listcsSimpleShopProduct = CsGameData.Instance.GetSimpleShopProductList(CsUIData.Instance.HpPotionId);

                if (listcsSimpleShopProduct != null)
                {
                    CsCommandEventManager.Instance.SendSimpleShopBuy(listcsSimpleShopProduct[0].ProductId, nCount);
                }

                break;

            case EnItemType.ReturnScroll:
                listcsSimpleShopProduct = CsGameData.Instance.GetSimpleShopProductList(CsUIData.Instance.ReturnScrollId);

                if (listcsSimpleShopProduct != null)
                {
                    CsCommandEventManager.Instance.SendSimpleShopBuy(listcsSimpleShopProduct[0].ProductId, nCount);
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseCalculator()
    {
        m_csPopupCalculator.EventBuyItem -= OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator -= OnEventCloseCalculator;
        m_csPopupCalculator = null;

        Transform trPopupCalculator = m_trPopupList.Find("PopupCalculator");

        if (trPopupCalculator != null)
        {
            Destroy(trPopupCalculator.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMainQuestCart(bool bQuestAccepted = false)
    {
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;

        if (csMainQuest != null)
        {
            if (csMainQuest.MainQuestType == EnMainQuestType.Cart && CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Accepted)
            {
                if (bQuestAccepted)
                {
					DisplayRideToggleIsOn(false);
                    DisplayRideToggle(false);
                    m_trButtonList.gameObject.SetActive(false);
                }
                else
                {
                    DisplayRideToggle(!CsCartManager.Instance.IsMyHeroRidingCart);
                    m_trButtonList.gameObject.SetActive(!CsCartManager.Instance.IsMyHeroRidingCart);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySupplySupportCart(bool bQuestAccepted = false)
    {
        EnSupplySupportState enSupplySupportState = CsSupplySupportQuestManager.Instance.QuestState;

        if (enSupplySupportState == EnSupplySupportState.Accepted)
        {
            if (bQuestAccepted)
            {
				DisplayRideToggleIsOn(false);
                DisplayRideToggle(false);
                m_trButtonList.gameObject.SetActive(false);
            }
            else
            {
                DisplayRideToggle(!CsCartManager.Instance.IsMyHeroRidingCart);
                m_trButtonList.gameObject.SetActive(!CsCartManager.Instance.IsMyHeroRidingCart);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void DisplayGuidSupplySupportCart(bool bQuestAccepted = false)
	{
		EnSupplySupportState enSupplySupportState = CsSupplySupportQuestManager.Instance.QuestState;

		if (CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.Accepted)
		{
			if (bQuestAccepted)
			{
				DisplayRideToggleIsOn(false);
				DisplayRideToggle(false);
				m_trButtonList.gameObject.SetActive(false);
			}
			else
			{
				DisplayRideToggle(!CsCartManager.Instance.IsMyHeroRidingCart);
				m_trButtonList.gameObject.SetActive(!CsCartManager.Instance.IsMyHeroRidingCart);
			}
		}
	}


    //---------------------------------------------------------------------------------------------------
    void ResetCart()
    {
        DisplayRideToggle(true);
        m_trButtonList.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextGuideTime()
    {
        TimeSpan timeSpanCurrentTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime - CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date;

        if (CsGameData.Instance.DimensionInfiltrationEvent.StartTime <= timeSpanCurrentTime.TotalSeconds && timeSpanCurrentTime.TotalSeconds <= CsGameData.Instance.DimensionInfiltrationEvent.EndTime)
        {
            m_textGuideTime.text = CsConfiguration.Instance.GetString("A57_TXT_00001");
            
            if (m_textGuideTime.gameObject.activeSelf)
            {
                return;
            }
            else
            {
                m_textGuideTime.gameObject.SetActive(true);
            }
        }
        else if (CsGameData.Instance.BattlefieldSupportEvent.StartTime <= timeSpanCurrentTime.TotalSeconds && timeSpanCurrentTime.TotalSeconds <= CsGameData.Instance.BattlefieldSupportEvent.EndTime)
        {
            m_textGuideTime.text = CsConfiguration.Instance.GetString("A57_TXT_00002");
            
            if (m_textGuideTime.gameObject.activeSelf)
            {
                return;
            }
            else
            {
                m_textGuideTime.gameObject.SetActive(true);
            }
        }
        else
        {
            if (m_textGuideTime.gameObject.activeSelf)
            {
                m_textGuideTime.gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetAutoChanged(bool bEventCall = true)
    {
        switch (m_enBattleMode)
        {
            case EnBattleMode.None:
                m_buttonAutoBattle.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A04_BTN_00004");
                break;
            case EnBattleMode.Auto:
                m_buttonAutoBattle.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A04_BTN_00003");
                break;
            case EnBattleMode.Manual:
                m_buttonAutoBattle.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A04_BTN_00005");
                break;
        }

        if (bEventCall)
        {
            CsGameEventToIngame.Instance.OnEventAutoBattleStart(m_enBattleMode);
            CsGameEventUIToUI.Instance.OnEventChangeAutoBattleMode(m_enBattleMode);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupQuickMenu()
    {
        if (m_goPopupQuickMenu == null)
        {
            StartCoroutine(LoadPopupQuickMenu());
        }
        else
        {
            Instantiate(m_goPopupQuickMenu, m_trPopupList);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupQuickMenu()
    { 
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupQuickMenu");
        yield return resourceRequest;
        m_goPopupQuickMenu = (GameObject)resourceRequest.asset;

        Instantiate(m_goPopupQuickMenu, m_trPopupList);
    }

	//---------------------------------------------------------------------------------------------------
	void DisplayAutoBattleButton(bool bVisible)
	{
		m_buttonAutoBattle.gameObject.SetActive(bVisible);
	}

    //---------------------------------------------------------------------------------------------------
    void DisplayDungeonEnterShortCut()
    {
        CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
        
        // 자국의 대륙에 있고 살아 있으면
        if (csContinent != null && CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam && !CsIngameData.Instance.MyHeroDead)
        {
            bool bDungeonOpen = false;

            int nCurrentSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;
            int nRemainingTime = 0;
            
            CsTradeShipSchedule csTradeShipSchedule = null;
            
            if (CsDungeonManager.Instance.TradeShip.PlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount)
            {
                switch (CsDungeonManager.Instance.TradeShip.RequiredConditionType)
                {
                    case 1:
                        if (CsDungeonManager.Instance.TradeShip.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                        {
                            bDungeonOpen = true;
                        }
                        else
                        {
                            bDungeonOpen = false;
                        }
                        break;

                    case 2:
                        if (CsMainQuestManager.Instance.MainQuest != null && CsDungeonManager.Instance.TradeShip.RequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
                        {
                            bDungeonOpen = true;
                        }
                        else
                        {
                            bDungeonOpen = false;
                        }
                        break;
                }

                if (bDungeonOpen)
                {
                    for (int i = 0; i < CsGameData.Instance.TradeShip.TradeShipScheduleList.Count; i++)
                    {
                        if (CsGameData.Instance.TradeShip.TradeShipScheduleList[i].StartTime < nCurrentSecond && nCurrentSecond < CsGameData.Instance.TradeShip.TradeShipScheduleList[i].EndTime)
                        {
                            csTradeShipSchedule = CsGameData.Instance.TradeShip.TradeShipScheduleList[i];
                            nRemainingTime = csTradeShipSchedule.EndTime - nCurrentSecond;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    nRemainingTime = 0;
                    csTradeShipSchedule = null;
                }
            }
            else
            {
                nRemainingTime = 0;
                csTradeShipSchedule = null;
            }

            CsAnkouTombSchedule csAnkouTombSchedule = null;

            if (CsDungeonManager.Instance.AnkouTomb.PlayCount < CsDungeonManager.Instance.AnkouTomb.EnterCount)
            {
                switch (CsDungeonManager.Instance.AnkouTomb.RequiredConditionType)
                {
                    case 1:
                        if (CsDungeonManager.Instance.AnkouTomb.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                        {
                            bDungeonOpen = true;
                        }
                        else
                        {
                            bDungeonOpen = false;
                        }
                        break;

                    case 2:
                        if (CsMainQuestManager.Instance.MainQuest != null && CsDungeonManager.Instance.AnkouTomb.RequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
                        {
                            bDungeonOpen = true;
                        }
                        else
                        {
                            bDungeonOpen = false;
                        }
                        break;
                }

                if (bDungeonOpen)
                {
                    for (int i = 0; i < CsGameData.Instance.AnkouTomb.AnkouTombScheduleList.Count; i++)
                    {
                        if (CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[i].StartTime < nCurrentSecond && nCurrentSecond < CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[i].EndTime)
                        {
                            csAnkouTombSchedule = CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[i];
                            nRemainingTime = csAnkouTombSchedule.EndTime - nCurrentSecond;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    nRemainingTime = 0;
                    csAnkouTombSchedule = null;
                }
            }
            else
            {
                nRemainingTime = 0;
                csAnkouTombSchedule = null;
            }

            if (nRemainingTime == 0)
            {
                if (m_trImageDungeonEnterShortCut.gameObject.activeSelf)
                {
                    m_trImageDungeonEnterShortCut.gameObject.SetActive(false);
                }
                else
                {
                    return;
                }
            }
            else
            {
				Text textDungeonName = m_trImageDungeonEnterShortCut.Find("RemainDungeon/TextDungeonName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textDungeonName);

                Text textRemainingTime = m_trImageDungeonEnterShortCut.Find("RemainDungeon/TextRemainingTime").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRemainingTime);

                TimeSpan tsRemainingTime = TimeSpan.FromSeconds(nRemainingTime);

				Button buttonDungeonEnter = m_trImageDungeonEnterShortCut.GetComponent<Button>();
				//buttonDungeonEnter.onClick.RemoveAllListeners();
				//buttonDungeonEnter.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                bool bDisplay = false;

                if (csTradeShipSchedule == null)
                {

                }
                else
                {
                    if (CsDungeonManager.Instance.TradeShipMatchingState == EnDungeonMatchingState.None)
                    {
                        bDisplay = true;
						textDungeonName.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUNINTIME1"), CsGameData.Instance.TradeShip.Name);
						textRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUNINTIME2"), tsRemainingTime.Minutes.ToString("00"), tsRemainingTime.Seconds.ToString("00"));

                        buttonDungeonEnter.onClick.RemoveAllListeners();
                        buttonDungeonEnter.onClick.AddListener(() => OnClickOpenPopupDungeon(EnDungeonPlay.TradeShip));
                        buttonDungeonEnter.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    }
                    else
                    {
                        bDisplay = false;
                    }
                }

                if (csAnkouTombSchedule == null)
                {

                }
                else
                {
                    if (CsDungeonManager.Instance.AnkouTombMatchingState == EnDungeonMatchingState.None)
                    {
                        bDisplay = true;
						textDungeonName.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUNINTIME1"), CsGameData.Instance.AnkouTomb.Name);
						textRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUNINTIME2"), tsRemainingTime.Minutes.ToString("00"), tsRemainingTime.Seconds.ToString("00"));

                        buttonDungeonEnter.onClick.RemoveAllListeners();
                        buttonDungeonEnter.onClick.AddListener(() => OnClickOpenPopupDungeon(EnDungeonPlay.AnkouTomb));
                        buttonDungeonEnter.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    }
                    else
                    {
                        bDisplay = false;
                    }
                }

                if (bDisplay)
                {
                    if (!m_trImageDungeonEnterShortCut.gameObject.activeSelf)
                    {
                        m_trImageDungeonEnterShortCut.gameObject.SetActive(true);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (m_trImageDungeonEnterShortCut.gameObject.activeSelf)
                    {
                        m_trImageDungeonEnterShortCut.gameObject.SetActive(false);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        else
        {
            if (m_trImageDungeonEnterShortCut.gameObject.activeSelf)
            {
                m_trImageDungeonEnterShortCut.gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupDungeon(EnDungeonPlay enDungeonPlay)
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(LoadDungeonSubMenu(enDungeonPlay));
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDungeonSubMenu(EnDungeonPlay enDungeonPlay)
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        yield return new WaitUntil(() => trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory") != null);

        Transform trDungeonSubMenu = trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory");
        CsDungeonCartegory csDungeonCartegory = trDungeonSubMenu.GetComponent<CsDungeonCartegory>();

        switch (enDungeonPlay)
        {
            case EnDungeonPlay.AnkouTomb:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.AnkouTomb);
                break;

            case EnDungeonPlay.TradeShip:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.TradeShip);
                break;
        }
    }
}