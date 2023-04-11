using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsPopupDungeonInfo : CsPopupSub
{
    Transform m_trItemInfo;
    Transform m_trBack;
    Transform m_trDifficultyContent;
    Transform m_trRewardList;
    Transform m_trRewardContent;
    Transform m_trExpReward;

    Text m_textDungeonName;
    Text m_textOpenLevel;
    Text m_textRecomandCp;
    Text m_textChallengeTime;
    Text m_textStamina;
    Text m_textCount;
    Text m_textStaminaCount;
    Text m_textEnterMember;
    Text m_textPurchaseStaminaCount;
    Text m_textSweep;
    Text m_textDungeonDesc;
    Text m_textEnterDungeon;

    Button m_buttonPointRecord;
    Button m_buttonClosePointRecord;
    Button m_buttonSweep;
    Button m_buttonStamina;
    Button m_buttonEnterDungeon;
    Button m_buttonTip;

    Toggle m_toggleTeam;

    GameObject m_goToggleDifficulty;
    GameObject m_goPopupItemInfo;
    GameObject m_goRewardItem;
    GameObject m_goMatchingSelect;

    CsSubMenu m_csSubMenu;
    CsPopupItemInfo m_csPopupItemInfo;

    bool m_bIsFirst = true;

    int m_nDungeonIndex;
    int m_nDiffiCulty;
    int m_nNpcId;
    int m_nFloor;

    float m_flTime = 0;

    TimeSpan m_timeSpan;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trBack = transform.Find("ImageBackground");
        m_trBack.gameObject.SetActive(false);

        //소탕
        CsDungeonManager.Instance.EventStoryDungeonSweep += OnEventStoryDungeonSweep;
        CsDungeonManager.Instance.EventExpDungeonSweep += OnEventExpDungeonSweep;
        CsDungeonManager.Instance.EventGoldDungeonSweep += OnEventGoldDungeonSweep;
		CsDungeonManager.Instance.EventWisdomTempleSweep += OnEventWisdomTempleSweep;

        //고대인의 유적
        CsDungeonManager.Instance.EventAncientRelicMatchingCancel += OnEventAncientRelicMatchingCancel;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomBanished += OnEventAncientRelicMatchingRoomBanished;
        CsDungeonManager.Instance.EventAncientRelicMatchingStart += OnEventAncientRelicMatchingStart;
		CsDungeonManager.Instance.EventAncientRelicMatchingStatusChanged += OnEventAncientRelicMatchingStatusChanged;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimMatchingCancel += OnEventRuinsReclaimMatchingCancel;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomBanished += OnEventRuinsReclaimMatchingRoomBanished;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStart += OnEventRuinsReclaimMatchingStart;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStatusChanged += OnEventRuinsReclaimMatchingStatusChanged;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomPartyEnter += OnEventRuinsReclaimMatchingRoomPartyEnter;

        // 무한 대전
        CsDungeonManager.Instance.EventInfiniteWarMatchingCancel += OnEventInfiniteWarMatchingCancel;
        CsDungeonManager.Instance.EventInfiniteWarMatchingRoomBanished += OnEventInfiniteWarMatchingRoomBanished;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStart += OnEventInfiniteWarMatchingStart;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStatusChanged += OnEventInfiniteWarMatchingStatusChanged;
		
		// 공포의 제단
		CsDungeonManager.Instance.EventFearAltarMatchingStart += OnEventFearAltarMatchingStart;
		CsDungeonManager.Instance.EventFearAltarMatchingCancel += OnEventFearAltarMatchingCancel;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomPartyEnter += OnEventFearAltarMatchingRoomPartyEnter;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomBanished += OnEventFearAltarMatchingRoomBanished;
		CsDungeonManager.Instance.EventFearAltarMatchingStatusChanged += OnEventFearAltarMatchingStatusChanged;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryMatchingStart += OnEventWarMemoryMatchingStart;
        CsDungeonManager.Instance.EventWarMemoryMatchingCancel += OnEventWarMemoryMatchingCancel;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomPartyEnter += OnEventWarMemoryMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomBanished += OnEventWarMemoryMatchingRoomBanished;
        CsDungeonManager.Instance.EventWarMemoryMatchingStatusChanged += OnEventWarMemoryMatchingStatusChanged;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestMatchingStart += OnEventDragonNestMatchingStart;
        CsDungeonManager.Instance.EventDragonNestMatchingCancel += OnEventDragonNestMatchingCancel;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomPartyEnter += OnEventDragonNestMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomBanished += OnEventDragonNestMatchingRoomBanished;
        CsDungeonManager.Instance.EventDragonNestMatchingStatusChanged += OnEventDragonNestMatchingStatusChanged;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombMatchingStart += OnEventAnkouTombMatchingStart;
        CsDungeonManager.Instance.EventAnkouTombMatchingCancel += OnEventAnkouTombMatchingCancel;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomPartyEnter += OnEventAnkouTombMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomBanished += OnEventAnkouTombMatchingRoomBanished;
        CsDungeonManager.Instance.EventAnkouTombMatchingStatusChanged += OnEventAnkouTombMatchingStatusChanged;
        CsDungeonManager.Instance.EventAnkouTombServerBestRecordUpdated += OnEventAnkouTombServerBestRecordUpdated;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipMatchingStart += OnEventTradeShipMatchingStart;
        CsDungeonManager.Instance.EventTradeShipMatchingCancel += OnEventTradeShipMatchingCancel;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomPartyEnter += OnEventTradeShipMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomBanished += OnEventTradeShipMatchingRoomBanished;
        CsDungeonManager.Instance.EventTradeShipMatchingStatusChanged += OnEventTradeShipMatchingStatusChanged;
        CsDungeonManager.Instance.EventTradeShipServerBestRecordUpdated += OnEventTradeShipServerBestRecordUpdated;

		// 팀 전장
		CsDungeonManager.Instance.EventTeamBattlefieldInfo += OnEventTeamBattlefieldInfo;
		CsDungeonManager.Instance.EventTeamBattlefieldInfoFail += OnEventTeamBattlefieldInfoFail;

		CsGameEventUIToUI.Instance.EventStaminaBuy += OnEventStaminaBuy;
        CsGameEventUIToUI.Instance.EventStaminaScheduleRecovery += OnEventStaminaScheduleRecovery;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_textStaminaCount != null)
            {
                if (CsGameData.Instance.MyHeroInfo.Stamina >= CsGameConfig.Instance.MaxStamina)
                {
                    UpdateStaminaCount(CsConfiguration.Instance.GetString("A13_TITLE_00003"));
                }
                else
                {
                    if ((CsGameData.Instance.MyHeroInfo.StaminaAutoRecoveryRemainingTime) > 0)
                    {
                        m_timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.MyHeroInfo.StaminaAutoRecoveryRemainingTime);
                        UpdateStaminaCount(string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00004"), m_timeSpan.Minutes.ToString("00"), m_timeSpan.Seconds.ToString("00")));
                    }
                    else
                    {
                        UpdateStaminaCount(string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00004"), "00", "00"));
                    }
                }
            }

			if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon &&
				(EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.RuinsReclaim &&
				m_buttonEnterDungeon != null)
			{
				bool bEnterable = false;

				if (CsGameData.Instance.RuinsReclaim.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
				{
					// 입장 횟수 & 재료 체크
					if (CsGameData.Instance.RuinsReclaim.FreePlayCount < CsGameData.Instance.RuinsReclaim.FreeEnterCount ||
						CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.RuinsReclaim.EnterRequiredItem.ItemId) > 0)
					{
						// 입장 시간 체크
						int sSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

						foreach (var schedule in CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList)
						{
							if (schedule.StartTime <= sSeconds && sSeconds < schedule.EndTime)
							{
								bEnterable = true;
								break;
							}
						}
					}
				}

				CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
			}
            else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && (EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.InfiniteWar && m_buttonEnterDungeon != null)
            {
                bool bEnterable = false;

                if (CsGameData.Instance.InfiniteWar.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    // 입장 횟수 & 재료 체크
                    if (CsGameData.Instance.InfiniteWar.DailyPlayCount < CsGameData.Instance.InfiniteWar.EnterCount &&
                        CsGameData.Instance.InfiniteWar.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
                    {
                        // 입장 시간 체크
                        int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                        for (int i = 0; i < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
                        {
                            if (CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].StartTime <= nSecond && nSecond < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                            {
                                bEnterable = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
            }
            else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && (EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.WarMemory && m_buttonEnterDungeon != null)
            {
                bool bEnterable = false;
                CsWarMemory csWarMemory = CsGameData.Instance.WarMemory;

                if (csWarMemory.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    if (csWarMemory.FreePlayCount < csWarMemory.FreeEnterCount || 0 < CsGameData.Instance.MyHeroInfo.GetItemCount(csWarMemory.EnterRequiredItemId))
                    {
                        int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                        for (int i = 0; i < csWarMemory.WarMemoryScheduleList.Count; i++)
                        {
                            if (csWarMemory.WarMemoryScheduleList[i].StartTime <= nSecond && nSecond < csWarMemory.WarMemoryScheduleList[i].EndTime)
                            {
                                bEnterable = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
            }
            else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && (EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.TradeShip && m_buttonEnterDungeon != null)
            {
                bool bEnterable = false;

                CsTradeShip csTradeShip = CsGameData.Instance.TradeShip;

                EnRequiredConditionType enRequiredConditionType = (EnRequiredConditionType)csTradeShip.RequiredConditionType;

                if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                {
                    if (csTradeShip.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        bEnterable = true;
                    }
                    else
                    {
                        bEnterable = false;
                    }
                }
                else
                {
                    if (CsMainQuestManager.Instance.MainQuest != null && csTradeShip.RequiredMainQuestNo <= CsMainQuestManager.Instance.MainQuest.MainQuestNo)
                    {
                        bEnterable = true;
                    }
                    else
                    {
                        bEnterable = false;
                    }
                }

                if (bEnterable)
                {
                    bEnterable = false;

                    if (csTradeShip.PlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount && csTradeShip.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
                    {
                        CsTradeShipSchedule csTradeShipSchedule = null;

                        int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                        for (int i = 0; i < csTradeShip.TradeShipScheduleList.Count; i++)
                        {
                            csTradeShipSchedule = csTradeShip.TradeShipScheduleList[i];

                            if (csTradeShipSchedule.StartTime <= nSecond && nSecond < csTradeShipSchedule.EndTime)
                            {
                                bEnterable = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
            }
            else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && (EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.AnkouTomb && m_buttonEnterDungeon != null)
            {
                bool bEnterable = false;

                CsAnkouTomb csAnkouTomb = CsGameData.Instance.AnkouTomb;

                EnRequiredConditionType enRequiredConditionType = (EnRequiredConditionType)csAnkouTomb.RequiredConditionType;

                if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                {
                    if (csAnkouTomb.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        bEnterable = true;
                    }
                    else
                    {
                        bEnterable = false;
                    }
                }
                else
                {
                    if (CsMainQuestManager.Instance.MainQuest != null && csAnkouTomb.RequiredMainQuestNo <= CsMainQuestManager.Instance.MainQuest.MainQuestNo)
                    {
                        bEnterable = true;
                    }
                    else
                    {
                        bEnterable = false;
                    }
                }

                if (bEnterable)
                {
                    bEnterable = false;

                    if (csAnkouTomb.PlayCount < csAnkouTomb.EnterCount && csAnkouTomb.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
                    {
                        CsAnkouTombSchedule csAnkouTombSchedule = null;

                        int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                        for (int i = 0; i < csAnkouTomb.AnkouTombScheduleList.Count; i++)
                        {
                            csAnkouTombSchedule = csAnkouTomb.AnkouTombScheduleList[i];

                            if (csAnkouTombSchedule.StartTime <= nSecond && nSecond < csAnkouTombSchedule.EndTime)
                            {
                                bEnterable = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        //소탕
        CsDungeonManager.Instance.EventStoryDungeonSweep -= OnEventStoryDungeonSweep;
        CsDungeonManager.Instance.EventExpDungeonSweep -= OnEventExpDungeonSweep;
        CsDungeonManager.Instance.EventGoldDungeonSweep -= OnEventGoldDungeonSweep;
		CsDungeonManager.Instance.EventWisdomTempleSweep -= OnEventWisdomTempleSweep;

        //고대인의 유적
        CsDungeonManager.Instance.EventAncientRelicMatchingCancel -= OnEventAncientRelicMatchingCancel;
        CsDungeonManager.Instance.EventAncientRelicMatchingRoomBanished -= OnEventAncientRelicMatchingRoomBanished;
        CsDungeonManager.Instance.EventAncientRelicMatchingStart -= OnEventAncientRelicMatchingStart;
		CsDungeonManager.Instance.EventAncientRelicMatchingStatusChanged -= OnEventAncientRelicMatchingStatusChanged;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimMatchingCancel -= OnEventRuinsReclaimMatchingCancel;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomBanished -= OnEventRuinsReclaimMatchingRoomBanished;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStart -= OnEventRuinsReclaimMatchingStart;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingStatusChanged -= OnEventRuinsReclaimMatchingStatusChanged;
		CsDungeonManager.Instance.EventRuinsReclaimMatchingRoomPartyEnter -= OnEventRuinsReclaimMatchingRoomPartyEnter;

        // 무한 대전
        CsDungeonManager.Instance.EventInfiniteWarMatchingCancel -= OnEventInfiniteWarMatchingCancel;
        CsDungeonManager.Instance.EventInfiniteWarMatchingRoomBanished -= OnEventInfiniteWarMatchingRoomBanished;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStart -= OnEventInfiniteWarMatchingStart;
        CsDungeonManager.Instance.EventInfiniteWarMatchingStatusChanged -= OnEventInfiniteWarMatchingStatusChanged;

		// 공포의 제단
		CsDungeonManager.Instance.EventFearAltarMatchingStart -= OnEventFearAltarMatchingStart;
		CsDungeonManager.Instance.EventFearAltarMatchingCancel -= OnEventFearAltarMatchingCancel;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomPartyEnter -= OnEventFearAltarMatchingRoomPartyEnter;
		CsDungeonManager.Instance.EventFearAltarMatchingRoomBanished -= OnEventFearAltarMatchingRoomBanished;
		CsDungeonManager.Instance.EventFearAltarMatchingStatusChanged -= OnEventFearAltarMatchingStatusChanged;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryMatchingStart -= OnEventWarMemoryMatchingStart;
        CsDungeonManager.Instance.EventWarMemoryMatchingCancel -= OnEventWarMemoryMatchingCancel;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomPartyEnter -= OnEventWarMemoryMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventWarMemoryMatchingRoomBanished -= OnEventWarMemoryMatchingRoomBanished;
        CsDungeonManager.Instance.EventWarMemoryMatchingStatusChanged -= OnEventWarMemoryMatchingStatusChanged;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestMatchingStart -= OnEventDragonNestMatchingStart;
        CsDungeonManager.Instance.EventDragonNestMatchingCancel -= OnEventDragonNestMatchingCancel;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomPartyEnter -= OnEventDragonNestMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventDragonNestMatchingRoomBanished -= OnEventDragonNestMatchingRoomBanished;
        CsDungeonManager.Instance.EventDragonNestMatchingStatusChanged -= OnEventDragonNestMatchingStatusChanged;

        // 임시 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombMatchingStart -= OnEventAnkouTombMatchingStart;
        CsDungeonManager.Instance.EventAnkouTombMatchingCancel -= OnEventAnkouTombMatchingCancel;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomPartyEnter -= OnEventAnkouTombMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventAnkouTombMatchingRoomBanished -= OnEventAnkouTombMatchingRoomBanished;
        CsDungeonManager.Instance.EventAnkouTombMatchingStatusChanged -= OnEventAnkouTombMatchingStatusChanged;
        CsDungeonManager.Instance.EventAnkouTombServerBestRecordUpdated -= OnEventAnkouTombServerBestRecordUpdated;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipMatchingStart -= OnEventTradeShipMatchingStart;
        CsDungeonManager.Instance.EventTradeShipMatchingCancel -= OnEventTradeShipMatchingCancel;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomPartyEnter -= OnEventTradeShipMatchingRoomPartyEnter;
        CsDungeonManager.Instance.EventTradeShipMatchingRoomBanished -= OnEventTradeShipMatchingRoomBanished;
        CsDungeonManager.Instance.EventTradeShipMatchingStatusChanged -= OnEventTradeShipMatchingStatusChanged;
        CsDungeonManager.Instance.EventTradeShipServerBestRecordUpdated -= OnEventTradeShipServerBestRecordUpdated;

		// 팀 전장
		CsDungeonManager.Instance.EventTeamBattlefieldInfo -= OnEventTeamBattlefieldInfo;
		CsDungeonManager.Instance.EventTeamBattlefieldInfoFail -= OnEventTeamBattlefieldInfoFail;
		
        CsGameEventUIToUI.Instance.EventStaminaBuy -= OnEventStaminaBuy;
        CsGameEventUIToUI.Instance.EventStaminaScheduleRecovery -= OnEventStaminaScheduleRecovery;
    }

    #region EventHandler

    #region Stamina
    //---------------------------------------------------------------------------------------------------
    void OnEventStaminaScheduleRecovery()
    {
        UpdateStaminaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStaminaBuy()
    {
        UpdateStaminaCount();
        UpdatePurchaseStaminaCount();
    }
    #endregion Stamina

    #region AncientRelic

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingCancel()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
        {
            if (m_nDungeonIndex == (int)EnPartyDungeonType.AncientRelic)
            {
                UpdateAncientRelicDungeon();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingRoomBanished()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
        {
            if (m_nDungeonIndex == (int)EnPartyDungeonType.AncientRelic)
            {
                UpdateAncientRelicDungeon();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicMatchingStart()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
        {
            if (m_nDungeonIndex == (int)EnPartyDungeonType.AncientRelic)
            {
                UpdateAncientRelicDungeon();
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventAncientRelicMatchingStatusChanged()
	{

	}

    #endregion AncientRelic

    #region RuinsReclaim
    //---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingCancel()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
		{
			if (m_nDungeonIndex == (int)EnTimeLimitDungeonType.RuinsReclaim)
			{
				UpdateRuinsReclaimDungeon();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingRoomBanished()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
		{
			if (m_nDungeonIndex == (int)EnTimeLimitDungeonType.RuinsReclaim)
			{
				UpdateRuinsReclaimDungeon();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingStart()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
		{
			if (m_nDungeonIndex == (int)EnTimeLimitDungeonType.RuinsReclaim)
			{
				UpdateRuinsReclaimDungeon();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingStatusChanged()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
		{
			if (m_nDungeonIndex == (int)EnTimeLimitDungeonType.RuinsReclaim)
			{
				UpdateRuinsReclaimDungeon();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMatchingRoomPartyEnter()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
		{
			if (m_nDungeonIndex == (int)EnTimeLimitDungeonType.RuinsReclaim)
			{
				UpdateRuinsReclaimDungeon();
			}
		}
	}

    #endregion RuinsReclaim

    #region InfiniteWar

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingCancel()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.InfiniteWar)
        {
            UpdateInfiniteWarDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingRoomBanished()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.InfiniteWar)
        {
            UpdateInfiniteWarDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingStart()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.InfiniteWar)
        {
            UpdateInfiniteWarDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMatchingStatusChanged()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.InfiniteWar)
        {
            UpdateInfiniteWarDungeon();
        }
        else
        {
            return;
        }
    }

    #endregion InfiniteWar

	#region FearAltar

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingCancel()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
		{
			if (m_nDungeonIndex == (int)EnPartyDungeonType.FearAltar)
			{
				UpdateFearAltar();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingRoomBanished()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
		{
			if (m_nDungeonIndex == (int)EnPartyDungeonType.FearAltar)
			{
				UpdateFearAltar();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingStart()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
		{
			if (m_nDungeonIndex == (int)EnPartyDungeonType.FearAltar)
			{
				UpdateFearAltar();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingStatusChanged()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
		{
			if (m_nDungeonIndex == (int)EnPartyDungeonType.FearAltar)
			{
				UpdateFearAltar();
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
	void OnEventFearAltarMatchingRoomPartyEnter()
	{
		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
		{
			if (m_nDungeonIndex == (int)EnPartyDungeonType.FearAltar)
			{
				UpdateFearAltar();
			}
		}
	}

    #endregion FearAltar

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingStart()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.WarMemory)
        {
            UpdateWarMemoryDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingCancel()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.WarMemory)
        {
            UpdateWarMemoryDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingRoomPartyEnter()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.WarMemory)
        {
            UpdateWarMemoryDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingRoomBanished()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.WarMemory)
        {
            UpdateWarMemoryDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMatchingStatusChanged()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.WarMemory)
        {
            UpdateWarMemoryDungeon();
        }
        else
        {
            return;
        }
    }

    #endregion WarMemory

    #region DragonNest

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingStart()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon && m_nDungeonIndex == (int)EnPartyDungeonType.DragonNest)
        {
            UpdateDragonNestDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingCancel()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon && m_nDungeonIndex == (int)EnPartyDungeonType.DragonNest)
        {
            UpdateDragonNestDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingRoomPartyEnter()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon && m_nDungeonIndex == (int)EnPartyDungeonType.DragonNest)
        {
            UpdateDragonNestDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingRoomBanished()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon && m_nDungeonIndex == (int)EnPartyDungeonType.DragonNest)
        {
            UpdateDragonNestDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestMatchingStatusChanged()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon && m_nDungeonIndex == (int)EnPartyDungeonType.DragonNest)
        {
            UpdateDragonNestDungeon();
        }
        else
        {
            return;
        }
    }

    #endregion DragonNest

    #region AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingStart()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.AnkouTomb)
        {
            UpdateAnkouTombDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingCancel()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.AnkouTomb)
        {
            UpdateAnkouTombDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingRoomPartyEnter(int nDifficulty)
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.AnkouTomb)
        {
            UpdateAnkouTombDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingRoomBanished()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.AnkouTomb)
        {
            UpdateAnkouTombDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombMatchingStatusChanged()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.AnkouTomb)
        {
            UpdateAnkouTombDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombServerBestRecordUpdated()
    {
        UpdatePointRecord(EnTimeLimitDungeonType.AnkouTomb);
    }

    #endregion AnkouTomb

    #region TradeShip

    void OnEventTradeShipMatchingStart()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.TradeShip)
        {
            UpdateTradeShipDungeon();
        }
        else
        {
            return;
        }
    }

    void OnEventTradeShipMatchingCancel()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.TradeShip)
        {
            UpdateTradeShipDungeon();
        }
        else
        {
            return;
        }
    }

    void OnEventTradeShipMatchingRoomPartyEnter()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.TradeShip)
        {
            UpdateTradeShipDungeon();
        }
        else
        {
            return;
        }
    }

    void OnEventTradeShipMatchingRoomBanished()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.TradeShip)
        {
            UpdateTradeShipDungeon();
        }
        else
        {
            return;
        }
    }

    void OnEventTradeShipMatchingStatusChanged()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon && m_nDungeonIndex == (int)EnTimeLimitDungeonType.TradeShip)
        {
            UpdateTradeShipDungeon();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipServerBestRecordUpdated()
    {
        UpdatePointRecord(EnTimeLimitDungeonType.TradeShip);
    }

    #endregion TradeShip

	#region TeamBattlefield
	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldInfo(bool bIsEnterableTime, bool bIsOpened, bool bIsFinished, int nMemberCount)
	{
		m_nDiffiCulty = 1;
		m_buttonStamina.gameObject.SetActive(false);
		UpdateTeamBattlefieldDungeon(bIsEnterableTime, bIsOpened, bIsFinished, nMemberCount);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldInfoFail()
	{
		m_trBack.gameObject.SetActive(false);
		CsGameEventUIToUI.Instance.OnEventGoBackDungeonCartegoryList();
	}
	//---------------------------------------------------------------------------------------------------
	#endregion TeamBattlefield

	//---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonSweep(bool bLevelUp, long lExp)
    {
        UpdateDungeonInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonSweep(long lGold)
    {
        UpdateDungeonInfo();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleSweep(bool bLevelUp, long lAcquiredExp, PDItemBooty pdItemBooty)
	{
		UpdateDungeonInfo();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonSweep()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A17_TXT_00011"));
        UpdateDungeonInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGoBackDungeonList()
    {
        m_trBack.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventGoBackDungeonCartegoryList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSweep()
    {
        string strAlert;

        switch (m_csSubMenu.EnSubMenu)
        {
            case EnSubMenu.StoryDungeon:

                if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
                {
                    //무료소탕
                    strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03004"), CsGameData.Instance.StoryDungeonList[m_nDungeonIndex].RequiredStamina, CsGameConfig.Instance.DungeonFreeSweepDailyCount - CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount);
                    CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), StoryDungeonSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                }
                else
                {
                    //소탕령 검사.
                    if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.DungeonSweepItemId) > 0)
                    {
                        //소탕가능
                        strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03006"), CsGameData.Instance.StoryDungeonList[m_nDungeonIndex].RequiredStamina);
                        CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), StoryDungeonSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("DUN_GOTO_SHOP"),
                            CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop),
                            CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                    }
                }
                break;

			case EnSubMenu.PartyDungeon:
				
				switch ((EnPartyDungeonType)m_nDungeonIndex)
				{
					case EnPartyDungeonType.FearAltar:
						// 성물 도감 팝업
						CsGameEventUIToUI.Instance.OnEventOpenPopupHalidomCollection();
						break;
				}
				break;

            case EnSubMenu.IndividualDungeon:

                switch ((EnIndividualDungeonType)m_nDungeonIndex)
                {
                    case EnIndividualDungeonType.Exp:

                        if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
                        {
                            //무료소탕
                            strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03004"), CsGameData.Instance.ExpDungeon.RequiredStamina, CsGameConfig.Instance.DungeonFreeSweepDailyCount - CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount);
                            CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), ExpDungeonSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                        }
                        else
                        {
                            //소탕령 검사.
                            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.DungeonSweepItemId) > 0)
                            {
                                //소탕가능
                                strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03006"), CsGameData.Instance.ExpDungeon.RequiredStamina);
                                CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), ExpDungeonSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("DUN_GOTO_SHOP"),
                                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop),
                                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                            }
                        }

                        break;

                    case EnIndividualDungeonType.Gold:

                        if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
                        {
                            //무료소탕
                            strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03004"), CsGameData.Instance.GoldDungeon.RequiredStamina, CsGameConfig.Instance.DungeonFreeSweepDailyCount - CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount);
                            CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), GoldDungeonSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                        }
                        else
                        {
                            //소탕령 검사.
                            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.DungeonSweepItemId) > 0)
                            {
                                //소탕가능
                                strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03006"), CsGameData.Instance.GoldDungeon.RequiredStamina);
                                CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), GoldDungeonSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("DUN_GOTO_SHOP"),
                                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop),
                                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                            }
                        }

                        break;

					case EnIndividualDungeonType.WisdomTemple:

						if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
						{
							//무료소탕
							strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03004"), CsGameData.Instance.WisdomTemple.RequiredStamina, CsGameConfig.Instance.DungeonFreeSweepDailyCount - CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount);
							CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), WisdomTempleSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
						}
						else
						{
							//소탕령 검사.
							if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.DungeonSweepItemId) > 0)
							{
								//소탕가능
								strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03006"), CsGameData.Instance.WisdomTemple.RequiredStamina);
								CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), WisdomTempleSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
							}
							else
                            {
                                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("DUN_GOTO_SHOP"),
                                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop),
                                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
							}
						}

						break;
                }
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnterDungeon()
    {
        switch ((EnSubMenu)m_csSubMenu.SubMenuId)
        {
            case EnSubMenu.StoryDungeon:
                CsStoryDungeon csStoryDungeon = CsGameData.Instance.StoryDungeonList[m_nDungeonIndex];
                CsDungeonManager.Instance.SendContinentExitForStoryDungeonEnter(csStoryDungeon.DungeonNo, m_nDiffiCulty);
                break;

            case EnSubMenu.IndividualDungeon:

                switch ((EnIndividualDungeonType)m_nDungeonIndex)
                {
                    case EnIndividualDungeonType.Exp:
                        CsDungeonManager.Instance.SendContinentExitForExpDungeonEnter(m_nDiffiCulty);
                        break;

                    case EnIndividualDungeonType.Gold:
                        CsDungeonManager.Instance.SendContinentExitForGoldDungeonEnter(m_nDiffiCulty);
                        break;

					case EnIndividualDungeonType.WisdomTemple:
						CsDungeonManager.Instance.SendContinentExitForWisdomTempleEnter();
						break;

                    case EnIndividualDungeonType.OsirisRoom:
                        CsDungeonManager.Instance.SendContinentExitForOsirisRoomEnter(m_nDiffiCulty);
                        break;
                }

                break;

            case EnSubMenu.PartyDungeon:

                switch ((EnPartyDungeonType)m_nDungeonIndex)
                {
					case EnPartyDungeonType.FearAltar:

						if (CsDungeonManager.Instance.FearAltarMatchingState == EnDungeonMatchingState.None)
						{
							if (CsGameData.Instance.MyHeroInfo.Party != null)
							{
								if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
								{
									//파티장이면
									if (m_goMatchingSelect == null)
									{
										StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnPartyDungeonType.FearAltar)));
									}
									else
									{
										OpenPopupMatchingSelect(EnPartyDungeonType.FearAltar);
									}
								}
								else
								{
									CsDungeonManager.Instance.SendFearAltarMatchingStart(false);
								}
							}
							else
							{
								CsDungeonManager.Instance.SendFearAltarMatchingStart(false);
							}
						}
						else
						{
							CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
						}

						break;

                    case EnPartyDungeonType.AncientRelic:

                        if (CsDungeonManager.Instance.AncientRelicState == EnDungeonMatchingState.None)
                        {
                            if (CsGameData.Instance.MyHeroInfo.Party != null)
                            {
                                if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                                {
                                    //파티장이면
                                    if (m_goMatchingSelect == null)
                                    {
                                        StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnPartyDungeonType.AncientRelic)));
                                    }
                                    else
                                    {
                                        OpenPopupMatchingSelect(EnPartyDungeonType.AncientRelic);
                                    }
                                }
                                else
                                {
                                    CsDungeonManager.Instance.SendAncientRelicMatchingStart(false);
                                }
                            }
                            else
                            {
                                CsDungeonManager.Instance.SendAncientRelicMatchingStart(false);
                            }
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
                        }

                        break;

                    case EnPartyDungeonType.SoulCoveter:

                        if (CsDungeonManager.Instance.SoulCoveterMatchingState == EnDungeonMatchingState.None)
                        {
                            if (CsGameData.Instance.MyHeroInfo.Party != null)
                            {
                                if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                                {
                                    //파티장이면
                                    if (m_goMatchingSelect == null)
                                    {
                                        StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnPartyDungeonType.SoulCoveter)));
                                    }
                                    else
                                    {
                                        OpenPopupMatchingSelect(EnPartyDungeonType.SoulCoveter);
                                    }
                                }
                                else
                                {
                                    CsDungeonManager.Instance.SendSoulCoveterMatchingStart(false, m_nDiffiCulty);
                                }
                            }
                            else
                            {
                                CsDungeonManager.Instance.SendSoulCoveterMatchingStart(false, m_nDiffiCulty);
                            }
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
                        }

                        break;

                    case EnPartyDungeonType.DragonNest:

                        if (CsDungeonManager.Instance.DragonNestMatchingState == EnDungeonMatchingState.None)
                        {
                            if (CsGameData.Instance.MyHeroInfo.Party != null)
                            {
                                if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                                {
                                    if (m_goMatchingSelect == null)
                                    {
                                        StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnPartyDungeonType.DragonNest)));
                                    }
                                    else
                                    {
                                        OpenPopupMatchingSelect(EnPartyDungeonType.DragonNest);
                                    }
                                }
                                else
                                {
                                    CsDungeonManager.Instance.SendDragonNestMatchingStart(false);
                                }
                            }
                            else
                            {
                                CsDungeonManager.Instance.SendDragonNestMatchingStart(false);
                            }
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
                        }

                        break;
                }
                break;

            case EnSubMenu.TimeLimitDungeon:

                switch ((EnTimeLimitDungeonType)m_nDungeonIndex)
                {
                    case EnTimeLimitDungeonType.UndergroundMaze:
                        CsDungeonManager.Instance.SendContinentExitForUndergroundMazeEnter(m_nFloor);
                        break;

					case EnTimeLimitDungeonType.RuinsReclaim:
						CsRuinsReclaim csRuinsReclaim = CsDungeonManager.Instance.RuinsReclaim;
						
						if (csRuinsReclaim.FreePlayCount < csRuinsReclaim.FreeEnterCount)
						{
							OnClickRuinsReclaimEnter();
						}
						else if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsDungeonManager.Instance.RuinsReclaim.EnterRequiredItem.ItemId) > 0)
						{
							// 확인 창
							CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("PUBLIC_TICKET_USE"), CsDungeonManager.Instance.RuinsReclaim.EnterRequiredItem.Name),
								CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickRuinsReclaimEnter,
								CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
						}
						else
						{
							// 토스트
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_TICKET_ERROR"), CsDungeonManager.Instance.RuinsReclaim.EnterRequiredItem.Name));
						}

						break;

                    case EnTimeLimitDungeonType.InfiniteWar:

                        if (CsDungeonManager.Instance.InfiniteWarMatchingState == EnDungeonMatchingState.None)
                        {
                            CheckInfiniteWarEnter();
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
                        }

                        break;

                    case EnTimeLimitDungeonType.WarMemory:
                        CsWarMemory csWarMemory = CsDungeonManager.Instance.WarMemory;

                        if (csWarMemory.FreePlayCount < csWarMemory.FreeEnterCount)
                        {
                            OnClickWarMemoryEnter();
                        }
                        else if (0 < CsGameData.Instance.MyHeroInfo.GetItemCount(csWarMemory.EnterRequiredItemId))
                        {
                            // 확인 창
                            CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("PUBLIC_TICKET_USE"), CsGameData.Instance.GetItem(csWarMemory.EnterRequiredItemId).Name),
                                CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickWarMemoryEnter,
                                CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                        }
                        else
                        {
                            // 토스트
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_TICKET_ERROR"), CsGameData.Instance.GetItem(csWarMemory.EnterRequiredItemId).Name));
                        }

                        break;

                    case EnTimeLimitDungeonType.TradeShip:
                        CsTradeShip csTradeShip = CsDungeonManager.Instance.TradeShip;

                        if (csTradeShip.PlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount)
                        {
                            OnClickTradeShipEnter();
                        }
                        else
                        {

                        }
                        break;

                    case EnTimeLimitDungeonType.AnkouTomb:

                        CsAnkouTomb csAnkouTomb = CsDungeonManager.Instance.AnkouTomb;

                        if (csAnkouTomb.PlayCount < csAnkouTomb.EnterCount)
                        {
                            OnClickAnkouTombEnter();
                        }
                        else
                        {

                        }

                        break;
                }
                break;

			case EnSubMenu.EventDungeon:

				switch ((EnEventDungeonType)m_nDungeonIndex)
				{
					case EnEventDungeonType.TeamBattlefield:

						CsDungeonManager.Instance.SendContinentExitForTeamBattlefieldEnter();

						break;
				}

				break;
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickRuinsReclaimEnter()
	{
		if (CsDungeonManager.Instance.RuinsReclaimMatchingState == EnDungeonMatchingState.None)
		{
			if (CsGameData.Instance.MyHeroInfo.Party != null)
			{
				if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
				{
					//파티장이면
					if (m_goMatchingSelect == null)
					{
						StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnTimeLimitDungeonType.RuinsReclaim)));
					}
					else
					{
						OpenPopupMatchingSelect(EnTimeLimitDungeonType.RuinsReclaim);
					}
				}
				else
				{
					CheckRuinsReclaimEnter(false);
				}
			}
			else
			{
				CheckRuinsReclaimEnter(false);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickWarMemoryEnter()
    {
        if (CsDungeonManager.Instance.WarMemoryMatchingState == EnDungeonMatchingState.None)
        {
            if (CsGameData.Instance.MyHeroInfo.Party != null)
            {
                if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                {
                    if (m_goMatchingSelect == null)
                    {
                        StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnTimeLimitDungeonType.WarMemory)));
                    }
                    else
                    {
                        OpenPopupMatchingSelect(EnTimeLimitDungeonType.WarMemory);
                    }
                }
                else
                {
                    CheckWarMemoryEnter(false);
                }
            }
            else
            {
                CheckWarMemoryEnter(false);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDragonNestEnter()
    {
        if (CsDungeonManager.Instance.DragonNestMatchingState == EnDungeonMatchingState.None)
        {
            if (CsGameData.Instance.MyHeroInfo.Party != null)
            {
                if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                {
                    if (m_goMatchingSelect == null)
                    {
                        StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnPartyDungeonType.DragonNest)));
                    }
                    else
                    {
                        OpenPopupMatchingSelect(EnPartyDungeonType.DragonNest);
                    }
                }
                else
                {
                    CheckDragonNestEnter(false);
                }
            }
            else
            {
                CheckDragonNestEnter(false);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTradeShipEnter()
    {
        CsTradeShipDifficulty csTradeShipDifficulty = CsGameData.Instance.TradeShip.TradeShipDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

        if (csTradeShipDifficulty == null)
        {
            return;
        }
        else
        {
            if (CsDungeonManager.Instance.TradeShipMatchingState == EnDungeonMatchingState.None)
            {
                if (CsGameData.Instance.MyHeroInfo.Party != null)
                {
                    if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                    {
                        if (m_goMatchingSelect == null)
                        {
                            StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnTimeLimitDungeonType.TradeShip)));
                        }
                        else
                        {
                            OpenPopupMatchingSelect(EnTimeLimitDungeonType.TradeShip);
                        }
                    }
                    else
                    {
                        CheckTradeShipEnter(false, csTradeShipDifficulty.Difficulty);
                    }
                }
                else
                {
                    CheckTradeShipEnter(false, csTradeShipDifficulty.Difficulty);
                }
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAnkouTombEnter()
    {
        CsAnkouTombDifficulty csAnkouTombDifficulty = CsGameData.Instance.AnkouTomb.AnkouTombDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

        if (csAnkouTombDifficulty == null)
        {
            return;
        }
        else
        {
            if (CsDungeonManager.Instance.AnkouTombMatchingState == EnDungeonMatchingState.None)
            {
                if (CsGameData.Instance.MyHeroInfo.Party != null)
                {
                    if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                    {
                        if (m_goMatchingSelect == null)
                        {
                            StartCoroutine(LoadPopupMatchingSelect(() => OpenPopupMatchingSelect(EnTimeLimitDungeonType.AnkouTomb)));
                        }
                        else
                        {
                            OpenPopupMatchingSelect(EnTimeLimitDungeonType.AnkouTomb);
                        }
                    }
                    else
                    {
                        CheckAnkouTompEnter(false, csAnkouTombDifficulty.Difficulty);
                    }
                }
                else
                {
                    CheckAnkouTompEnter(false, csAnkouTombDifficulty.Difficulty);
                }
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventOpenPopupMatching();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenStaminaToolTip()
    {
        m_buttonTip.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseStaminaToolTip()
    {
        m_buttonTip.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickChargeStamina()
    {
        int nStaminBuyCount = CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount + 1;
        CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);

        if (csStaminaBuyCount == null)
        {
            nStaminBuyCount = CsGameData.Instance.StaminaBuyCountList.Count;
            csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);
        }

        //스테미너 충전 확인창
        string strDes = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03001"), csStaminaBuyCount.RequiredDia, csStaminaBuyCount.Stamina);

        CsGameEventUIToUI.Instance.OnEventConfirm(strDes, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickChargeStaminaOK(nStaminBuyCount), CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickChargeStaminaOK(int nStaminaBuyCount)
    {
        CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminaBuyCount);

        if (csStaminaBuyCount.RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia)
        {
            CsCommandEventManager.Instance.SendStaminaBuy();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDifficulty(Toggle toggle, int nDifficulty)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nDiffiCulty = nDifficulty;
            UpdateDungeonInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedFloor(Toggle toggle, int nFloor)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nFloor = nFloor;
            UpdateDungeonInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDungeonRewardInfo(CsItem csItem)
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(() => OpenPopupItemInfo(csItem)));
        }
        else
        {
            OpenPopupItemInfo(csItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedPartyDungeonTeamInvitation(bool bIson)
    {
        int nAutoAccept = 0;

        if (bIson)
        {
            nAutoAccept = 1;
        }
        else
        {
            nAutoAccept = 0;
        }

        CsUIData.Instance.PlayUISound(EnUISoundType.Toggle);
        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept, nAutoAccept);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPointRecord()
    {
        if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
        {
            if ((EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.TradeShip)
            {
                UpdatePointRecord(EnTimeLimitDungeonType.TradeShip);
            }
            else if ((EnTimeLimitDungeonType)m_nDungeonIndex == EnTimeLimitDungeonType.AnkouTomb)
            {
                UpdatePointRecord(EnTimeLimitDungeonType.AnkouTomb);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePointRecord()
    {
        m_buttonClosePointRecord.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePointRecord(EnTimeLimitDungeonType enEventDungeonType)
    {
        if (m_nDiffiCulty == 0)
            return;
        
        Transform trImageBackground = m_buttonClosePointRecord.transform.Find("ImageBackground");

        Text textServerPointHeroName = trImageBackground.Find("TextServerName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textServerPointHeroName);

        Text textServerPointValue = trImageBackground.Find("TextServerPointValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textServerPointValue);

        Text textMyName = trImageBackground.Find("TextMyName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyName);

        Text textMyPointValue = trImageBackground.Find("TextMyPointValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyPointValue);

        switch (enEventDungeonType)
        {
            case EnTimeLimitDungeonType.TradeShip:
                Debug.Log("m_nDifficulty : " + m_nDiffiCulty);
                CsHeroTradeShipBestRecord csHeroTradeShipBestRecordServer = CsGameData.Instance.TradeShip.GetServerHeroTradeShipBestRecord(m_nDiffiCulty);

                if (csHeroTradeShipBestRecordServer == null)
                {
                    textServerPointHeroName.text = "";
                    textServerPointValue.text = "";
                }
                else
                {
                    textServerPointHeroName.text = csHeroTradeShipBestRecordServer.HeroName;
                    textServerPointValue.text = csHeroTradeShipBestRecordServer.Point.ToString("#,##0");
                }

                CsHeroTradeShipBestRecord csHeroTradeShipBestRecordMy = CsGameData.Instance.TradeShip.GetMyHeroTradeShipBestRecord(m_nDiffiCulty);

                if (csHeroTradeShipBestRecordMy == null)
                {
                    textMyName.text = "";
                    textMyPointValue.text = "";
                }
                else
                {
                    textMyName.text = csHeroTradeShipBestRecordMy.HeroName;
                    textMyPointValue.text = csHeroTradeShipBestRecordMy.Point.ToString("#,##0");
                }

                break;

            case EnTimeLimitDungeonType.AnkouTomb:

                CsHeroAnkouTombBestRecord csHeroAnkouTombBestRecordServer = CsGameData.Instance.AnkouTomb.GetServerHeroAnkouTombBestRecord(m_nDiffiCulty);

                if (csHeroAnkouTombBestRecordServer == null)
                {
                    textServerPointHeroName.text = "";
                    textServerPointValue.text = "";
                }
                else
                {
                    textServerPointHeroName.text = csHeroAnkouTombBestRecordServer.HeroName;
                    textServerPointValue.text = csHeroAnkouTombBestRecordServer.Point.ToString("#,##0");
                }

                CsHeroAnkouTombBestRecord csHeroAnkouTombBestRecordMy = CsGameData.Instance.AnkouTomb.GetMyHeroAnkouTombBestRecord(m_nDiffiCulty);

                if (csHeroAnkouTombBestRecordMy == null)
                {
                    textMyName.text = "";
                    textMyPointValue.text = "";
                }
                else
                {
                    textMyName.text = csHeroAnkouTombBestRecordMy.HeroName;
                    textMyPointValue.text = csHeroAnkouTombBestRecordMy.Point.ToString("#,##0");
                }

                break;
        }

        m_buttonClosePointRecord.gameObject.SetActive(true);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    public void DisplaySelectDungeonCartegory(int nDungeonIndex, CsSubMenu csSubMenu)
    {
        m_csSubMenu = csSubMenu;
        m_nDungeonIndex = nDungeonIndex;

        if (m_trBack != null)
        {
            m_trBack.gameObject.SetActive(true);
        }

        if (m_bIsFirst)
        {
            InitializeUI();
            m_bIsFirst = false;
        }

        switch ((EnSubMenu)m_csSubMenu.SubMenuId)
        {
            case EnSubMenu.StoryDungeon:
                Debug.Log("m_nDungeonIndex : " + m_nDungeonIndex);
                CsStoryDungeon csStoryDungeon = CsGameData.Instance.StoryDungeonList[m_nDungeonIndex];

                if (csStoryDungeon.ClearMaxDifficulty == 0)
                {
                    m_nDiffiCulty = 1;
                }
                else
                {
                    if (csStoryDungeon.GetStoryDungeonDifficulty(csStoryDungeon.ClearMaxDifficulty + 1) == null)
                    {
                        m_nDiffiCulty = csStoryDungeon.ClearMaxDifficulty;
                    }
                    else
                    {
                        m_nDiffiCulty = csStoryDungeon.ClearMaxDifficulty + 1;
                    }
                }

                m_buttonStamina.gameObject.SetActive(true);
                UpdateStoryDungeon();
                break;

            case EnSubMenu.IndividualDungeon:

                m_buttonStamina.gameObject.SetActive(true);

                switch ((EnIndividualDungeonType)m_nDungeonIndex)
                {
                    case EnIndividualDungeonType.Exp:

                        CsExpDungeon csExpDungeon = CsGameData.Instance.ExpDungeon;

                        for (int i = 0; i < csExpDungeon.ExpDungeonDifficultyList.Count; i++)
                        {
                            if (csExpDungeon.ExpDungeonDifficultyList[i].RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                            {
                                m_nDiffiCulty = csExpDungeon.ExpDungeonDifficultyList[i].Difficulty;
                            }
                        }

                        UpdateExpDungeon();
                        break;

                    case EnIndividualDungeonType.Gold:

                        CsGoldDungeon csGoldDungeon = CsGameData.Instance.GoldDungeon;

                        for (int i = 0; i < csGoldDungeon.GoldDungeonDifficultyList.Count; i++)
                        {
                            if (csGoldDungeon.GoldDungeonDifficultyList[i].RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                            {
                                m_nDiffiCulty = csGoldDungeon.GoldDungeonDifficultyList[i].Difficulty;
                            }
                        }

                        UpdateGoldDungeon();
                        break;

					case EnIndividualDungeonType.WisdomTemple:
						UpdateWisdomTemple();
						break;

                    case EnIndividualDungeonType.OsirisRoom:

                        CsOsirisRoom csOsirisRoom = CsDungeonManager.Instance.OsirisRoom;

                        for (int i = csOsirisRoom.OsirisRoomDifficultyList.Count - 1; i >= 0; i--)
                        {
                            if (csOsirisRoom.OsirisRoomDifficultyList[i].RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                            {
                                m_nDiffiCulty = csOsirisRoom.OsirisRoomDifficultyList[i].Difficulty;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        UpdateOsirisRoom();
                        break;
                }
                break;

            case EnSubMenu.PartyDungeon:

                m_buttonStamina.gameObject.SetActive(true);

                switch ((EnPartyDungeonType)m_nDungeonIndex)
                {
					case EnPartyDungeonType.FearAltar:
						UpdateFearAltar();
						break;

                    case EnPartyDungeonType.AncientRelic:
                        UpdateAncientRelicDungeon();
                        break;

                    case EnPartyDungeonType.SoulCoveter:
                        m_nDiffiCulty = 1;
                        UpdateSoulConveterDungeon();
                        break;

                    case EnPartyDungeonType.DragonNest:
                        UpdateDragonNestDungeon();
                        break;
                }
                break;

            case EnSubMenu.TimeLimitDungeon:

                switch ((EnTimeLimitDungeonType)m_nDungeonIndex)
                {
                    case EnTimeLimitDungeonType.UndergroundMaze:
                        m_buttonStamina.gameObject.SetActive(false);
                        UpdateUndergroundMazeDungeon();
                        break;

					case EnTimeLimitDungeonType.RuinsReclaim:
						m_buttonStamina.gameObject.SetActive(true);
						UpdateRuinsReclaimDungeon();
						break;

                    case EnTimeLimitDungeonType.InfiniteWar:
                        m_buttonStamina.gameObject.SetActive(true);
                        UpdateInfiniteWarDungeon();
                        break;

                    case EnTimeLimitDungeonType.WarMemory:
                        m_buttonStamina.gameObject.SetActive(true);
                        UpdateWarMemoryDungeon();
                        break;

                    case EnTimeLimitDungeonType.TradeShip:
                        m_buttonStamina.gameObject.SetActive(true);
                        UpdateTradeShipDungeon();
                        break;

                    case EnTimeLimitDungeonType.AnkouTomb:
                        m_buttonStamina.gameObject.SetActive(true);
                        UpdateAnkouTombDungeon();
                        break;
                }

                break;

			case EnSubMenu.EventDungeon:

                switch ((EnEventDungeonType)m_nDungeonIndex)
				{
					case EnEventDungeonType.TeamBattlefield:
						CsDungeonManager.Instance.SendTeamBattlefieldInfo();
						break;
				}

				break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Button buttonBackDungeonList = m_trBack.Find("ButtonBackDungeonList").GetComponent<Button>();
        buttonBackDungeonList.onClick.RemoveAllListeners();
        buttonBackDungeonList.onClick.AddListener(OnClickGoBackDungeonList);
        buttonBackDungeonList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textBackDungeonList = buttonBackDungeonList.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBackDungeonList);
        textBackDungeonList.text = CsConfiguration.Instance.GetString("A17_BTN_00003");

        m_trDifficultyContent = m_trBack.Find("Scroll View/Viewport/Content");

        Transform trDungeonInfo = m_trBack.Find("DungeonInfo");

        m_textDungeonName = trDungeonInfo.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDungeonName);

        m_textDungeonDesc = trDungeonInfo.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDungeonDesc);

        Text textDungeonInfo = trDungeonInfo.Find("TextDungeonInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDungeonInfo);
        textDungeonInfo.text = CsConfiguration.Instance.GetString("A17_TXT_00001");

        Text textClearReward = trDungeonInfo.Find("RewardList/TextClearReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClearReward);
        textClearReward.text = CsConfiguration.Instance.GetString("A17_TXT_00006");

        Transform trInfoList = trDungeonInfo.Find("InfoList");

        Text textOpenLevel = trInfoList.Find("InfoLevel/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOpenLevel);
        textOpenLevel.text = CsConfiguration.Instance.GetString("A17_TXT_00002");

        m_textOpenLevel = trInfoList.Find("InfoLevel/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textOpenLevel);

        Text textRecomandCp = trInfoList.Find("InfoBattlePower/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRecomandCp);
        textRecomandCp.text = CsConfiguration.Instance.GetString("A17_TXT_00003");

        m_textRecomandCp = trInfoList.Find("InfoBattlePower/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRecomandCp);

        Text textChallengeTime = trInfoList.Find("InfoTime/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textChallengeTime);
        textChallengeTime.text = CsConfiguration.Instance.GetString("A17_TXT_00004");

        m_textChallengeTime = trInfoList.Find("InfoTime/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textChallengeTime);

        Text textStamina = trInfoList.Find("InfoStamina/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textStamina);
        textStamina.text = CsConfiguration.Instance.GetString("A17_TXT_00005");

        m_textStamina = trInfoList.Find("InfoStamina/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textStamina);

        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");

        m_textCount = trInfoList.Find("InfoCount/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCount);

        Text textEnterMember = trInfoList.Find("InfoEnterMember/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnterMember);
        textEnterMember.text = CsConfiguration.Instance.GetString("PUBLIC_DUN_RECC");

        m_textEnterMember = trInfoList.Find("InfoEnterMember/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEnterMember);

        m_trRewardList = trDungeonInfo.Find("RewardList");
        m_trRewardContent = m_trRewardList.Find("Viewport/Content");

        m_trRewardContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        m_buttonPointRecord = trDungeonInfo.Find("ButtonPointRecord").GetComponent<Button>();
        m_buttonPointRecord.onClick.RemoveAllListeners();
        m_buttonPointRecord.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonPointRecord.onClick.AddListener(OnClickPointRecord);

        Text textButtonPointRecord = m_buttonPointRecord.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonPointRecord);
        textButtonPointRecord.text = CsConfiguration.Instance.GetString("PUBLIC_DUNPOINT_1");

        m_buttonClosePointRecord = m_buttonPointRecord.transform.Find("ButtonClose").GetComponent<Button>();
        m_buttonClosePointRecord.onClick.RemoveAllListeners();
        m_buttonClosePointRecord.onClick.AddListener(OnClickClosePointRecord);
        m_buttonClosePointRecord.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textServerPointRecord = m_buttonClosePointRecord.transform.Find("ImageBackground/ImageServerPoint/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textServerPointRecord);
        textServerPointRecord.text = CsConfiguration.Instance.GetString("PUBLIC_DUNPOINT_2");

        Text textMyPointRecord = m_buttonClosePointRecord.transform.Find("ImageBackground/ImageMyPoint/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyPointRecord);
        textMyPointRecord.text = CsConfiguration.Instance.GetString("PUBLIC_DUNPOINT_3");

        m_buttonSweep = trDungeonInfo.Find("ButtonSweep").GetComponent<Button>();
        m_buttonSweep.onClick.RemoveAllListeners();
        m_buttonSweep.onClick.AddListener(OnClickSweep);
        m_buttonSweep.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textSweep = m_buttonSweep.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textSweep);
        m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00002");

        m_buttonEnterDungeon = trDungeonInfo.Find("ButtonChallenge").GetComponent<Button>();
        m_buttonEnterDungeon.onClick.RemoveAllListeners();
        m_buttonEnterDungeon.onClick.AddListener(OnClickEnterDungeon);
        m_buttonEnterDungeon.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textEnterDungeon = m_buttonEnterDungeon.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEnterDungeon);

        m_buttonStamina = m_trBack.Find("ButtonStamina").GetComponent<Button>();
        m_buttonStamina.onClick.RemoveAllListeners();
        m_buttonStamina.onClick.AddListener(OnClickOpenStaminaToolTip);
        m_buttonStamina.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textStaminaCount = m_buttonStamina.transform.Find("TextStaminaCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textStaminaCount);

        Button buttonChargeStamina = m_buttonStamina.transform.Find("ButtonChargeStamina").GetComponent<Button>();
        buttonChargeStamina.onClick.RemoveAllListeners();
        buttonChargeStamina.onClick.AddListener(OnClickChargeStamina);
        buttonChargeStamina.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonTip = m_buttonStamina.transform.Find("ButtonClose").GetComponent<Button>();
        m_buttonTip.onClick.RemoveAllListeners();
        m_buttonTip.onClick.AddListener(OnClickCloseStaminaToolTip);
        m_buttonTip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trTipBack = m_buttonTip.transform.Find("ImageBack");

        Text textPurchaseStaminaInfo = trTipBack.Find("PurchaseStaminaCount/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPurchaseStaminaInfo);
        textPurchaseStaminaInfo.text = CsConfiguration.Instance.GetString("A13_TXT_04004");

        m_textPurchaseStaminaCount = trTipBack.Find("PurchaseStaminaCount/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPurchaseStaminaCount);

        for (int i = 0; i < CsGameData.Instance.StaminaRecoveryScheduleList.Count; i++)
        {
            Text textStaminaRecoveryGuide = trTipBack.Find("TextStaminaRecovery" + i).GetComponent<Text>();
            CsUIData.Instance.SetFont(textStaminaRecoveryGuide);
            TimeSpan timespanRecoveryTime = TimeSpan.FromSeconds(CsGameData.Instance.StaminaRecoveryScheduleList[i].RecoveryTime);
            textStaminaRecoveryGuide.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_04006"), timespanRecoveryTime.Hours.ToString("00"), CsGameData.Instance.StaminaRecoveryScheduleList[i].RecoveryStamina);
        }

        m_toggleTeam = trDungeonInfo.Find("ToggleTeam").GetComponent<Toggle>();
        m_toggleTeam.onValueChanged.RemoveAllListeners();

        Text textToggleTeam = m_toggleTeam.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleTeam);
        textToggleTeam.text = CsConfiguration.Instance.GetString("A144_TXT_00005");

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyPartyDungeonTeamInvitation))
        {
            int nAutoAccept = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyPartyDungeonTeamInvitation);

            if (nAutoAccept == 1)
            {
                m_toggleTeam.isOn = true;
                textToggleTeam.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                m_toggleTeam.isOn = false;
                textToggleTeam.color = CsUIData.Instance.ColorGray;
            }
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyPartyDungeonTeamInvitation, 0);

            m_toggleTeam.isOn = false;
            textToggleTeam.color = CsUIData.Instance.ColorGray;
        }

        m_toggleTeam.onValueChanged.AddListener((ison) => OnValueChangedPartyDungeonTeamInvitation(ison));

        UpdateStaminaCount();
        UpdatePurchaseStaminaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateStaminaCount(string strTime = "00:00")
    {
        //현재 스테미너
        m_textStaminaCount.text = string.Format(CsConfiguration.Instance.GetString("A17_TXT_00010"), CsGameData.Instance.MyHeroInfo.Stamina, CsGameConfig.Instance.MaxStamina, strTime);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePurchaseStaminaCount()
    {
        //스테미너 구매 횟수
        m_textPurchaseStaminaCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_04005"), CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount, CsGameData.Instance.MyHeroInfo.VipLevel.StaminaBuyMaxCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDungeonInfo()
    {
        if (m_bIsFirst)
        {
            InitializeUI();
            m_bIsFirst = false;
        }

        switch ((EnSubMenu)m_csSubMenu.SubMenuId)
        {
            case EnSubMenu.StoryDungeon:
                m_buttonStamina.gameObject.SetActive(true);
                UpdateStoryDungeon();
                break;

            case EnSubMenu.IndividualDungeon:
                m_buttonStamina.gameObject.SetActive(true);

                switch ((EnIndividualDungeonType)m_nDungeonIndex)
                {
                    case EnIndividualDungeonType.Exp:
                        UpdateExpDungeon();
                        break;

                    case EnIndividualDungeonType.Gold:
                        UpdateGoldDungeon();
                        break;
					case EnIndividualDungeonType.WisdomTemple:
						UpdateWisdomTemple();
						break;
                }
                break;

            case EnSubMenu.PartyDungeon:
                switch ((EnPartyDungeonType)m_nDungeonIndex)
                {
                    case EnPartyDungeonType.AncientRelic:
                        UpdateAncientRelicDungeon();
                        break;

                    case EnPartyDungeonType.SoulCoveter:
                        UpdateSoulConveterDungeon();
                        break;

                    case EnPartyDungeonType.DragonNest:
                        UpdateDragonNestDungeon();
                        break;
                }
                break;

            case EnSubMenu.TimeLimitDungeon:
                switch ((EnTimeLimitDungeonType)m_nDungeonIndex)
                {
                    case EnTimeLimitDungeonType.UndergroundMaze:
                        m_buttonStamina.gameObject.SetActive(false);
                        UpdateUndergroundMazeDungeon();
                        break;

                    case EnTimeLimitDungeonType.WarMemory:
                        m_buttonStamina.gameObject.SetActive(false);
                        UpdateWarMemoryDungeon();
                        break;

                    case EnTimeLimitDungeonType.TradeShip:
                        m_buttonStamina.gameObject.SetActive(false);
                        UpdateTradeShipDungeon();
                        break;

                    case EnTimeLimitDungeonType.AnkouTomb:
                        m_buttonStamina.gameObject.SetActive(false);
                        UpdateAnkouTombDungeon();
                        break;
                }
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExpDungeon()
    {
        CsExpDungeon csExpDungeon = CsGameData.Instance.ExpDungeon;

        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungoenNum + "_" + m_nDungeonIndex);

        Transform trImagePartyDungeon = m_trBack.Find("ImagePartyDungeon");
        trImagePartyDungeon.gameObject.SetActive(false);

        m_textDungeonName.text = csExpDungeon.Name;
        m_textDungeonDesc.text = csExpDungeon.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(false);

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csExpDungeon.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.ExpDungeonEnterCount - csExpDungeon.ExpDungeonDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.ExpDungeonEnterCount);

        for (int i = 0; i < csExpDungeon.ExpDungeonDifficultyList.Count; i++)
        {
            int nToggleIndex = i;
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + nToggleIndex);

            if (trToggle == null)
            {
                if (m_goToggleDifficulty == null)
                {
                    m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                }

                GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trDifficultyContent);
                goToggle.name = "ToggleDifficulty" + nToggleIndex;
                trToggle = goToggle.transform;
            }
            else
            {
                trToggle.gameObject.SetActive(true);
            }

            Toggle toggleDifficulty = trToggle.GetComponent<Toggle>();
            toggleDifficulty.onValueChanged.RemoveAllListeners();

            if (csExpDungeon.ExpDungeonDifficultyList[i].Difficulty == m_nDiffiCulty)
            {
                toggleDifficulty.isOn = true;
            }
            else
            {
                toggleDifficulty.isOn = false;
            }

            toggleDifficulty.group = m_trDifficultyContent.GetComponent<ToggleGroup>();
            toggleDifficulty.onValueChanged.AddListener((ison) => OnValueChangedDifficulty(toggleDifficulty, csExpDungeon.ExpDungeonDifficultyList[nToggleIndex].Difficulty));

            Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDifficulty);
            textDifficulty.text = string.Format(CsConfiguration.Instance.GetString("INPUT_DUN_LEVEL"), csExpDungeon.Name, csExpDungeon.ExpDungeonDifficultyList[i].RequiredHeroLevel);

            Transform trComplete = trToggle.Find("ImageComplete");
            Transform trLock = trToggle.Find("ImageLock");

            Text textLock = trLock.Find("TextUnLockInfo").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);
            textLock.text = "";

            bool bIsClear = false;

            for (int j = 0; j < csExpDungeon.ExpDungeonClearedDifficultyList.Count; j++)
            {
                if (csExpDungeon.ExpDungeonDifficultyList[i].Difficulty == csExpDungeon.ExpDungeonClearedDifficultyList[j])
                {
                    bIsClear = true;
                    break;
                }
            }

            if (CsGameData.Instance.MyHeroInfo.Level >= csExpDungeon.ExpDungeonDifficultyList[i].RequiredHeroLevel)
            {
                if (bIsClear)
                {
                    trComplete.gameObject.SetActive(true);
                    trLock.gameObject.SetActive(false);
                    toggleDifficulty.interactable = true;
                }
                else
                {
                    trComplete.gameObject.SetActive(false);
                    trLock.gameObject.SetActive(false);
                    toggleDifficulty.interactable = true;
                }
            }
            else
            {
                trComplete.gameObject.SetActive(false);
                trLock.gameObject.SetActive(true);
                textLock.text = string.Format(CsConfiguration.Instance.GetString("A32_BTN_00003"), csExpDungeon.ExpDungeonDifficultyList[i].RequiredHeroLevel);
                toggleDifficulty.interactable = false;
            }
        }

        for (int i = 0; i < m_trDifficultyContent.childCount - csExpDungeon.ExpDungeonDifficultyList.Count; i++)
        {
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + (i + csExpDungeon.ExpDungeonDifficultyList.Count));
            trToggle.gameObject.SetActive(false);
        }

        // 보상
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trExp = m_trRewardContent.Find("Exp");
        trExp.gameObject.SetActive(true);

        Text textExp = trExp.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExp);
        textExp.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

        CsExpDungeonDifficulty csExpDungeonDifficulty = csExpDungeon.GetExpDungeonDifficulty(m_nDiffiCulty);

        Text textExpValue = trExp.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExpValue);
        textExpValue.text = csExpDungeonDifficulty.ExpReward.Value.ToString("#,##0");

        Transform trVip = m_trRewardContent.Find("Vip");
        trVip.gameObject.SetActive(true);

        Text textVip = trVip.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textVip);
        textVip.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_VIPLV"), CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel);

        Text textVipValue = trVip.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textVipValue);
        textVipValue.text = "";

        //소탕조건

        bool bIsClearSelectLevel = false;

        for (int i = 0; i < csExpDungeon.ExpDungeonClearedDifficultyList.Count; i++)
        {
            if (csExpDungeon.ExpDungeonClearedDifficultyList[i] == m_nDiffiCulty)
            {
                bIsClearSelectLevel = true;
                break;
            }
        }

        m_buttonSweep.gameObject.SetActive(bIsClearSelectLevel);
        m_buttonPointRecord.gameObject.SetActive(false);

        if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
        {
            m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00001");
        }
        else
        {
            m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00002");
        }

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)

        if (CsGameData.Instance.MyHeroInfo.Level >= csExpDungeonDifficulty.RequiredHeroLevel)
        {
            //레벨조건만족
            if (CsGameData.Instance.MyHeroInfo.Stamina >= csExpDungeon.RequiredStamina)
            {
                //스테미너 충분
                if (csExpDungeon.ExpDungeonDailyPlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.ExpDungeonEnterCount)
                {
                    //입장횟수 충분
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, true);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
        }

        m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGoldDungeon()
    {
        CsGoldDungeon csGoldDungeon = CsGameData.Instance.GoldDungeon;

        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungoenNum + "_" + m_nDungeonIndex);

        Transform trImagePartyDungeon = m_trBack.Find("ImagePartyDungeon");
        trImagePartyDungeon.gameObject.SetActive(false);

        m_textDungeonName.text = csGoldDungeon.Name;
        m_textDungeonDesc.text = csGoldDungeon.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(false);

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csGoldDungeon.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.GoldDungeonEnterCount - csGoldDungeon.GoldDungeonDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.GoldDungeonEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        for (int i = 0; i < csGoldDungeon.GoldDungeonDifficultyList.Count; i++)
        {
            int nToggleIndex = i;
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + nToggleIndex);

            if (trToggle == null)
            {
                if (m_goToggleDifficulty == null)
                {
                    m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                }

                GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trDifficultyContent);
                goToggle.name = "ToggleDifficulty" + nToggleIndex;
                trToggle = goToggle.transform;
            }
            else
            {
                trToggle.gameObject.SetActive(true);
            }

            Toggle toggleDifficulty = trToggle.GetComponent<Toggle>();
            toggleDifficulty.onValueChanged.RemoveAllListeners();

            if (csGoldDungeon.GoldDungeonDifficultyList[i].Difficulty == m_nDiffiCulty)
            {
                toggleDifficulty.isOn = true;
            }
            else
            {
                toggleDifficulty.isOn = false;
            }

            toggleDifficulty.group = m_trDifficultyContent.GetComponent<ToggleGroup>();
            toggleDifficulty.onValueChanged.AddListener((ison) => OnValueChangedDifficulty(toggleDifficulty, csGoldDungeon.GoldDungeonDifficultyList[nToggleIndex].Difficulty));

            Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDifficulty);
            textDifficulty.text = string.Format(CsConfiguration.Instance.GetString("INPUT_DUN_LEVEL"), csGoldDungeon.Name, csGoldDungeon.GoldDungeonDifficultyList[i].RequiredHeroLevel);

            Transform trComplete = trToggle.Find("ImageComplete");
            Transform trLock = trToggle.Find("ImageLock");

            Text textLock = trLock.Find("TextUnLockInfo").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);
            textLock.text = "";

            bool bIsClear = false;

            for (int j = 0; j < csGoldDungeon.GoldDungeonClearedDifficultyList.Count; j++)
            {
                if (csGoldDungeon.GoldDungeonDifficultyList[i].Difficulty == csGoldDungeon.GoldDungeonClearedDifficultyList[j])
                {
                    bIsClear = true;
                    break;
                }
            }

            if (CsGameData.Instance.MyHeroInfo.Level >= csGoldDungeon.GoldDungeonDifficultyList[i].RequiredHeroLevel)
            {
                if (bIsClear)
                {
                    trComplete.gameObject.SetActive(true);
                    trLock.gameObject.SetActive(false);
                    toggleDifficulty.interactable = true;
                }
                else
                {
                    trComplete.gameObject.SetActive(false);
                    trLock.gameObject.SetActive(false);
                    toggleDifficulty.interactable = true;
                }
            }
            else
            {
                trComplete.gameObject.SetActive(false);
                trLock.gameObject.SetActive(true);
                textLock.text = string.Format(CsConfiguration.Instance.GetString("A32_BTN_00003"), csGoldDungeon.GoldDungeonDifficultyList[i].RequiredHeroLevel);
                toggleDifficulty.interactable = false;
            }
        }

        for (int i = 0; i < m_trDifficultyContent.childCount - csGoldDungeon.GoldDungeonDifficultyList.Count; i++)
        {
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + (i + csGoldDungeon.GoldDungeonDifficultyList.Count));
            trToggle.gameObject.SetActive(false);
        }

        // 보상
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trGold = m_trRewardContent.Find("Gold");
        trGold.gameObject.SetActive(true);

        Text textGold = trGold.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGold);
        textGold.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_RUPEE");

        CsGoldDungeonDifficulty csGoldDungeonDifficulty = csGoldDungeon.GetGoldDungeonDifficulty(m_nDiffiCulty);
        CsGoldReward csGoldReward = CsGameData.Instance.GetGoldReward(csGoldDungeonDifficulty.GoldReward.GoldRewardId);

        Text textGoldValue = trGold.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGoldValue);
        textGoldValue.text = csGoldReward.Value.ToString("#,##0");

        //소탕조건

        bool bIsClearSelectLevel = false;

        for (int i = 0; i < csGoldDungeon.GoldDungeonClearedDifficultyList.Count; i++)
        {
            if (csGoldDungeon.GoldDungeonClearedDifficultyList[i] == m_nDiffiCulty)
            {
                bIsClearSelectLevel = true;
                break;
            }
        }

        m_buttonSweep.gameObject.SetActive(bIsClearSelectLevel);
        m_buttonPointRecord.gameObject.SetActive(false);

        if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
        {
            m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00001");
        }
        else
        {
            m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00002");
        }

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)               

        if (CsGameData.Instance.MyHeroInfo.Level >= csGoldDungeonDifficulty.RequiredHeroLevel)
        {
            //레벨조건만족
            if (CsGameData.Instance.MyHeroInfo.Stamina >= csGoldDungeon.RequiredStamina)
            {
                //스테미너 충분
                if (csGoldDungeon.GoldDungeonDailyPlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.GoldDungeonEnterCount)
                {
                    //입장횟수 충분
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, true);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
        }

        m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateOsirisRoom()
    {
        CsOsirisRoom csOsirisRoom = CsDungeonManager.Instance.OsirisRoom;
        
        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungoenNum + "_" + m_nDungeonIndex);

        Transform trImagePartyDungeon = m_trBack.Find("ImagePartyDungeon");
        trImagePartyDungeon.gameObject.SetActive(false);

        m_textDungeonName.text = csOsirisRoom.Name;
        m_textDungeonDesc.text = csOsirisRoom.Description;

        m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(false);

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csOsirisRoom.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.OsirisRoomEnterCount - csOsirisRoom.DailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.OsirisRoomEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        for (int i = 0; i < csOsirisRoom.OsirisRoomDifficultyList.Count; i++)
        {
            int nToggleIndex = i;
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + nToggleIndex);

            if (trToggle == null)
            {
                if (m_goToggleDifficulty == null)
                {
                    m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                }

                GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trDifficultyContent);
                goToggle.name = "ToggleDifficulty" + nToggleIndex;
                trToggle = goToggle.transform;
            }
            else
            {
                trToggle.gameObject.SetActive(true);
            }

            Toggle toggleDifficulty = trToggle.GetComponent<Toggle>();
            toggleDifficulty.onValueChanged.RemoveAllListeners();

            if (csOsirisRoom.OsirisRoomDifficultyList[i].Difficulty == m_nDiffiCulty)
            {
                toggleDifficulty.isOn = true;
            }
            else
            {
                toggleDifficulty.isOn = false;
            }

            toggleDifficulty.group = m_trDifficultyContent.GetComponent<ToggleGroup>();
            toggleDifficulty.onValueChanged.AddListener((ison) => OnValueChangedDifficulty(toggleDifficulty, csOsirisRoom.OsirisRoomDifficultyList[nToggleIndex].Difficulty));

            Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDifficulty);
            textDifficulty.text = string.Format(CsConfiguration.Instance.GetString("INPUT_DUN_LEVEL"), csOsirisRoom.Name, csOsirisRoom.OsirisRoomDifficultyList[i].RequiredHeroLevel);

            Transform trComplete = trToggle.Find("ImageComplete");
            trComplete.gameObject.SetActive(false);

            Transform trLock = trToggle.Find("ImageLock");

            Text textLock = trLock.Find("TextUnLockInfo").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);
            textLock.text = "";

            if (csOsirisRoom.OsirisRoomDifficultyList[i].RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                trLock.gameObject.SetActive(false);
                toggleDifficulty.interactable = true;
            }
            else
            {
                trLock.gameObject.SetActive(true);
                textLock.text = string.Format(CsConfiguration.Instance.GetString("A32_BTN_00003"), csOsirisRoom.OsirisRoomDifficultyList[i].RequiredHeroLevel);
                toggleDifficulty.interactable = false;
            }
        }

        for (int i = 0; i < m_trDifficultyContent.childCount - csOsirisRoom.OsirisRoomDifficultyList.Count; i++)
        {
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + (i + csOsirisRoom.OsirisRoomDifficultyList.Count));
            trToggle.gameObject.SetActive(false);
        }

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        // 보상
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trSlot = null; 

        for (int i = 0; i < csOsirisRoom.OsirisRoomAvailableRewardList.Count; i++)
        {
            trSlot = m_trRewardContent.Find("Reward" + i);

            if (trSlot == null)
            {
                trSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trSlot.name = "Reward" + i;
            }
            else
            {
                trSlot.gameObject.SetActive(true);
            }

            CsItem csItem = csOsirisRoom.OsirisRoomAvailableRewardList[i].Item;

            Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);

            Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csItem.Name;

            Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csItem));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        CsOsirisRoomDifficulty csOsirisRoomDifficulty = csOsirisRoom.GetOsirisRoomDifficulty(m_nDiffiCulty);

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)
        if (csOsirisRoomDifficulty.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            //레벨조건만족
            if (csOsirisRoom.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
            {
                //스테미너 충분
                if (csOsirisRoom.DailyPlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.OsirisRoomEnterCount)
                {
                    //입장횟수 충분
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
        }

        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateWisdomTemple()
	{
		CsWisdomTemple csWisdomTemple = CsGameData.Instance.WisdomTemple;
		
		int DungeonNum = m_csSubMenu.SubMenuId - 1100;
		Image imageBack = m_trBack.GetComponent<Image>();
		imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungeonNum + "_" + m_nDungeonIndex);

		Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
		ImagePartyDungeon.gameObject.SetActive(true);
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + DungeonNum + "_" + m_nDungeonIndex);

		m_textDungeonName.text = csWisdomTemple.Name;
		m_textDungeonDesc.text = csWisdomTemple.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

		Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

		Transform trInfoLevel = trInfoList.Find("InfoLevel");
		trInfoLevel.gameObject.SetActive(true);
		m_textOpenLevel.text = csWisdomTemple.RequiredHeroLevel.ToString();

		Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
		trInfoBattlePower.gameObject.SetActive(false);

		//Text textRecomandCp = trInfoBattlePower.Find("TextInfo").GetComponent<Text>();
		//textRecomandCp.text = CsConfiguration.Instance.GetString("A40_TXT_00006");

		//m_textRecomandCp.text = CsConfiguration.Instance.GetString("A40_TXT_00007");

		Transform trInfoTime = trInfoList.Find("InfoTime");
		trInfoTime.gameObject.SetActive(false);

		Transform trInfoStamina = trInfoList.Find("InfoStamina");
		trInfoStamina.gameObject.SetActive(true);
		m_textStamina.text = csWisdomTemple.RequiredStamina.ToString("#,##0");

		Transform trInfoCount = trInfoList.Find("InfoCount");
		trInfoCount.gameObject.SetActive(true);
		Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
		textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
		m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), Math.Max(0, 1 - CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount), 1);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

		for (int i = 0; i < m_trDifficultyContent.childCount; i++)
		{
			m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
		}

		if (m_goRewardItem == null)
		{
			m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
		}

		// 보상 
		m_trRewardList.gameObject.SetActive(true);

		for (int i = 0; i < m_trRewardContent.childCount; i++)
		{
			m_trRewardContent.GetChild(i).gameObject.SetActive(false);
		}

		Transform trSlot = m_trRewardContent.Find("Reward0");

		if (trSlot == null)
		{
			trSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
			trSlot.gameObject.name = "Reward0";
		}
		else
		{
			trSlot.gameObject.SetActive(true);
		}

		Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
		imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csWisdomTemple.AvailableRewardItem.Image);

		Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textItemName);
		textItemName.text = csWisdomTemple.AvailableRewardItem.Name;

		Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
		CsUIData.Instance.SetFont(textItemCount);
		textItemCount.text = "";


		Button buttonReward = trSlot.GetComponent<Button>();
		buttonReward.onClick.RemoveAllListeners();
		buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csWisdomTemple.AvailableRewardItem));
		buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		//소탕조건
		m_buttonSweep.gameObject.SetActive(CsGameData.Instance.WisdomTemple.WisdomTempleCleared);
        m_buttonPointRecord.gameObject.SetActive(false);

		if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
		{
			m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00001");
		}
		else
		{
			m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00002");
		}

		//버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)

		m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");

		if (csWisdomTemple.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
		{
			if (CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount < 1)
			{
				if (csWisdomTemple.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
				{
					CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, true);
					CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
				}
				else
				{
					CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
					CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
				}
			}
			else
			{
				CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
				CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
			}
		}
		else
		{
			CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
			CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateStoryDungeon()
    {
        CsStoryDungeon csStoryDungeon = CsGameData.Instance.StoryDungeonList[m_nDungeonIndex];

        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungoenNum + "_" + m_nDungeonIndex);

        Transform trImagePartyDungeon = m_trBack.Find("ImagePartyDungeon");
        trImagePartyDungeon.gameObject.SetActive(false);

        m_textDungeonName.text = csStoryDungeon.Name;
        m_textDungeonDesc.text = "";

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        m_textOpenLevel.text = string.Format(CsConfiguration.Instance.GetString("A17_TXT_00008"), csStoryDungeon.RequiredHeroMinLevel, csStoryDungeon.RequiredHeroMaxLevel);


        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(true);

        Text textRecomandCp = trInfoBattlePower.Find("TextInfo").GetComponent<Text>();
        textRecomandCp.text = CsConfiguration.Instance.GetString("A17_TXT_00003");

        CsStoryDungeonDifficulty csStoryDungeonDifficulty = csStoryDungeon.GetStoryDungeonDifficulty(m_nDiffiCulty);
        m_textRecomandCp.text = csStoryDungeonDifficulty.RecommendBattlePower.ToString("#,##0");

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(true);
        m_textChallengeTime.text = string.Format(CsConfiguration.Instance.GetString("A17_TXT_00009"), csStoryDungeon.LimitTime.ToString("#0"));

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csStoryDungeon.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), csStoryDungeon.EnterCount - csStoryDungeon.PlayCount, csStoryDungeon.EnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        for (int i = 0; i < csStoryDungeon.StoryDungeonDifficultyList.Count; i++)
        {
            int nToggleIndex = i;
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + nToggleIndex);

            if (trToggle == null)
            {
                if (m_goToggleDifficulty == null)
                {
                    m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                }

                GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trDifficultyContent);
                goToggle.name = "ToggleDifficulty" + nToggleIndex;
                trToggle = goToggle.transform;
            }
            else
            {
                trToggle.gameObject.SetActive(true);
            }

            Toggle toggleDifficulty = trToggle.GetComponent<Toggle>();
            toggleDifficulty.onValueChanged.RemoveAllListeners();

            if (csStoryDungeon.StoryDungeonDifficultyList[i].Difficulty == m_nDiffiCulty)
            {
                toggleDifficulty.isOn = true;
            }
            else
            {
                toggleDifficulty.isOn = false;
            }

            toggleDifficulty.group = m_trDifficultyContent.GetComponent<ToggleGroup>();
            toggleDifficulty.onValueChanged.AddListener((ison) => OnValueChangedDifficulty(toggleDifficulty, csStoryDungeon.StoryDungeonDifficultyList[nToggleIndex].Difficulty));

            Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDifficulty);
            textDifficulty.text = csStoryDungeon.StoryDungeonDifficultyList[i].Name;

            Transform trComplete = trToggle.Find("ImageComplete");
            Transform trLock = trToggle.Find("ImageLock");

            Text textLock = trLock.Find("TextUnLockInfo").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);
            textLock.text = "";

            if (csStoryDungeon.ClearMaxDifficulty >= csStoryDungeon.StoryDungeonDifficultyList[i].Difficulty)
            {
                trComplete.gameObject.SetActive(true);
                trLock.gameObject.SetActive(false);
                toggleDifficulty.interactable = true;
            }
            else if ((csStoryDungeon.ClearMaxDifficulty + 1) == csStoryDungeon.StoryDungeonDifficultyList[i].Difficulty)
            {
                trComplete.gameObject.SetActive(false);
                trLock.gameObject.SetActive(false);
                toggleDifficulty.interactable = true;
            }
            else
            {
                trComplete.gameObject.SetActive(false);
                trLock.gameObject.SetActive(true);

                CsStoryDungeonDifficulty csStoryDungeonDifficultyLock = csStoryDungeon.GetStoryDungeonDifficulty(csStoryDungeon.StoryDungeonDifficultyList[i].Difficulty - 1);

                textLock.text = string.Format(CsConfiguration.Instance.GetString("A17_BTN_00008"), csStoryDungeonDifficultyLock.Name);
                toggleDifficulty.interactable = false;
            }
        }

        for (int i = 0; i < m_trDifficultyContent.childCount - csStoryDungeon.StoryDungeonDifficultyList.Count; i++)
        {
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + (i + csStoryDungeon.StoryDungeonDifficultyList.Count));
            trToggle.gameObject.SetActive(false);
        }

        // 보상 
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        for (int i = 0; i < csStoryDungeonDifficulty.StoryDungeonAvailableRewardList.Count; i++)
        {
            int nRewardIndex = i;
            Transform trSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trSlot == null)
            {
                trSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trSlot.gameObject.name = "Reward" + i;
            }
            else
            {
                trSlot.gameObject.SetActive(true);
            }

            Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csStoryDungeonDifficulty.StoryDungeonAvailableRewardList[nRewardIndex].Item.Image);

            Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csStoryDungeonDifficulty.StoryDungeonAvailableRewardList[nRewardIndex].Item.Name;

            Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csStoryDungeonDifficulty.StoryDungeonAvailableRewardList[nRewardIndex].Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        //소탕조건
        if (csStoryDungeon.StoryDungeonDifficultyList.Count > csStoryDungeon.ClearMaxDifficulty)
        {
            m_buttonSweep.gameObject.SetActive(false);
        }
        else
        {
            m_buttonSweep.gameObject.SetActive(true);

            if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
            {
                m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00001");
            }
            else
            {
                m_textSweep.text = CsConfiguration.Instance.GetString("A13_BTN_00002");
            }
        }

        m_buttonPointRecord.gameObject.SetActive(false);

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)

        if (CsGameData.Instance.MyHeroInfo.Level >= csStoryDungeon.RequiredHeroMinLevel && CsGameData.Instance.MyHeroInfo.Level <= csStoryDungeon.RequiredHeroMaxLevel)
        {
            //레벨조건만족
            if (CsGameData.Instance.MyHeroInfo.Stamina >= csStoryDungeon.RequiredStamina)
            {
                //스테미너 충분
                if (csStoryDungeon.PlayCount < csStoryDungeon.EnterCount)
                {
                    //입장횟수 충분
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, true);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
        }

        m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateUndergroundMazeDungeon()
    {
        if (m_nFloor < 1)
        {
            m_nFloor = 1;
        }

        CsUndergroundMaze csUndergroundMaze = CsGameData.Instance.UndergroundMaze;

        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungoenNum + "_" + m_nDungeonIndex);

        Transform trImagePartyDungeon = m_trBack.Find("ImagePartyDungeon");
        trImagePartyDungeon.gameObject.SetActive(false);

        m_textDungeonName.text = csUndergroundMaze.Name;
        m_textDungeonDesc.text = csUndergroundMaze.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        CsUndergroundMazeEntrance csUndergroundMazeEntrance = csUndergroundMaze.GetUndergroundMazeEntrance(m_nFloor);
        m_textOpenLevel.text = csUndergroundMazeEntrance.RequiredHeroLevel.ToString("#,##0");

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(false);

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A43_TXT_00001");

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        TimeSpan timeSpan;

        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.UndergroundMaze) == null)
        {
            timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.UndergroundMaze.LimitTime - CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime);
        }
        else
        {
            timeSpan = TimeSpan.FromSeconds(0.0f);
        }

        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A43_TXT_01001"), (timeSpan.Minutes + (timeSpan.Hours * 60)).ToString("00"), timeSpan.Seconds.ToString("00"));

        for (int i = 0; i < csUndergroundMaze.UndergroundMazeEntranceList.Count; i++)
        {
            int nToggleIndex = i;
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + nToggleIndex);

            if (trToggle == null)
            {
                if (m_goToggleDifficulty == null)
                {
                    m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                }

                GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trDifficultyContent);
                goToggle.name = "ToggleDifficulty" + nToggleIndex;
                trToggle = goToggle.transform;
            }
            else
            {
                trToggle.gameObject.SetActive(true);
            }

            Toggle toggleDifficulty = trToggle.GetComponent<Toggle>();
            toggleDifficulty.onValueChanged.RemoveAllListeners();

            if (csUndergroundMaze.UndergroundMazeEntranceList[i].Floor == m_nFloor)
            {
                toggleDifficulty.isOn = true;
            }
            else
            {
                toggleDifficulty.isOn = false;
            }

            toggleDifficulty.group = m_trDifficultyContent.GetComponent<ToggleGroup>();
            toggleDifficulty.onValueChanged.AddListener((ison) => OnValueChangedFloor(toggleDifficulty, csUndergroundMaze.UndergroundMazeEntranceList[nToggleIndex].Floor));

            Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDifficulty);
            textDifficulty.text = string.Format(CsConfiguration.Instance.GetString("A43_BTN_00004"), csUndergroundMaze.UndergroundMazeEntranceList[i].Floor);

            Transform trComplete = trToggle.Find("ImageComplete");
            trComplete.gameObject.SetActive(false);

            Transform trLock = trToggle.Find("ImageLock");

            Text textLock = trLock.Find("TextUnLockInfo").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);

            if (csUndergroundMaze.UndergroundMazeEntranceList[i].RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                trLock.gameObject.SetActive(false);
                toggleDifficulty.interactable = true;
            }
            else
            {
                trLock.gameObject.SetActive(true);
                toggleDifficulty.interactable = false;
                textLock.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DUN_UNLOCK"), csUndergroundMaze.UndergroundMazeEntranceList[i].RequiredHeroLevel);
            }
        }

        for (int i = 0; i < m_trDifficultyContent.childCount - csUndergroundMaze.UndergroundMazeEntranceList.Count; i++)
        {
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + (i + csUndergroundMaze.UndergroundMazeEntranceList.Count));
            trToggle.gameObject.SetActive(false);
        }

        m_trRewardList.gameObject.SetActive(false);
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)

        if (CsGameData.Instance.MyHeroInfo.Level >= csUndergroundMazeEntrance.RequiredHeroLevel)
        {
            if (CsGameData.Instance.UndergroundMaze.LimitTime - CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime <= 0.0f || CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.UndergroundMaze) != null)
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
            }
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
        }

        m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAncientRelicDungeon()
    {
        CsAncientRelic csAncientRelic = CsGameData.Instance.AncientRelic;

        int DungeonNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungeonNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.gameObject.SetActive(true);
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + DungeonNum + "_" + m_nDungeonIndex);

        m_textDungeonName.text = csAncientRelic.Name;
        m_textDungeonDesc.text = csAncientRelic.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        m_textOpenLevel.text = csAncientRelic.RequiredHeroLevel.ToString();

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(true);

        Text textRecomandCp = trInfoBattlePower.Find("TextInfo").GetComponent<Text>();
        textRecomandCp.text = CsConfiguration.Instance.GetString("A40_TXT_00006");

        m_textRecomandCp.text = CsConfiguration.Instance.GetString("A40_TXT_00007");

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csAncientRelic.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.AncientRelicEnterCount - csAncientRelic.AncientRelicDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.AncientRelicEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        for (int i = 0; i < m_trDifficultyContent.childCount; i++)
        {
            m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
        }

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        // 보상 
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < csAncientRelic.AncientRelicAvailableRewardList.Count; i++)
        {
            int nRewardIndex = i;
            Transform trSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trSlot == null)
            {
                trSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trSlot.gameObject.name = "Reward" + i;
            }
            else
            {
                trSlot.gameObject.SetActive(true);
            }

            Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
            Debug.Log(csAncientRelic.AncientRelicAvailableRewardList[nRewardIndex].Item);
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csAncientRelic.AncientRelicAvailableRewardList[nRewardIndex].Item.Image);

            Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csAncientRelic.AncientRelicAvailableRewardList[i].Item.Name;

            Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csAncientRelic.AncientRelicAvailableRewardList[nRewardIndex].Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        //소탕조건
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)

        if (CsDungeonManager.Instance.AncientRelicState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            if (csAncientRelic.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
            {
                if (csAncientRelic.AncientRelicDailyPlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.AncientRelicEnterCount)
                {
                    if (csAncientRelic.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                    }
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
        }
        else
        {
            //매칭중
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSoulConveterDungeon()
    {
        CsSoulCoveter csSoulCoveter = CsGameData.Instance.SoulCoveter;

        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungoenNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.gameObject.SetActive(false);

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);
        //

        for (int i = 0; i < csSoulCoveter.SoulCoveterDifficultyList.Count; i++)
        {
            int nToggleIndex = i;
            Transform trToggle = m_trDifficultyContent.Find("ToggleDifficulty" + nToggleIndex);

            if (trToggle == null)
            {
                if (m_goToggleDifficulty == null)
                {
                    m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                }

                GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trDifficultyContent);
                goToggle.name = "ToggleDifficulty" + nToggleIndex;
                trToggle = goToggle.transform;
            }
            else
            {
                trToggle.gameObject.SetActive(true);
            }

            Toggle toggleDifficulty = trToggle.GetComponent<Toggle>();
            toggleDifficulty.onValueChanged.RemoveAllListeners();

            if (csSoulCoveter.SoulCoveterDifficultyList[i].Difficulty == m_nDiffiCulty)
            {
                toggleDifficulty.isOn = true;
            }
            else
            {
                toggleDifficulty.isOn = false;
            }

            toggleDifficulty.group = m_trDifficultyContent.GetComponent<ToggleGroup>();
            toggleDifficulty.onValueChanged.AddListener((ison) => OnValueChangedDifficulty(toggleDifficulty, csSoulCoveter.SoulCoveterDifficultyList[nToggleIndex].Difficulty));

            Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDifficulty);
            textDifficulty.text = csSoulCoveter.SoulCoveterDifficultyList[i].Name;

            Transform trComplete = trToggle.Find("ImageComplete");
            Transform trLock = trToggle.Find("ImageLock");

            Text textLock = trLock.Find("TextUnLockInfo").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);
            textLock.text = "";

            trComplete.gameObject.SetActive(false);
            trLock.gameObject.SetActive(false);
            toggleDifficulty.interactable = true;
        }


        for (int i = 0; i < m_trDifficultyContent.childCount - csSoulCoveter.SoulCoveterDifficultyList.Count; i++)
        {
            m_trDifficultyContent.GetChild((i + csSoulCoveter.SoulCoveterDifficultyList.Count)).gameObject.SetActive(false);
        }
        //

        m_textDungeonName.text = csSoulCoveter.Name;
        m_textDungeonDesc.text = csSoulCoveter.Description;

        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        m_textOpenLevel.text = csSoulCoveter.RequiredHeroLevel.ToString();

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(true);

        Text textRecomandCp = trInfoBattlePower.Find("TextInfo").GetComponent<Text>();
        textRecomandCp.text = CsConfiguration.Instance.GetString("A40_TXT_00006");

        m_textRecomandCp.text = CsConfiguration.Instance.GetString("A74_TXT_00001");

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csSoulCoveter.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
        textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.SoulCoveterWeeklyEnterCount - csSoulCoveter.SoulCoveterWeeklyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.SoulCoveterWeeklyEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        // 보상 
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < csSoulCoveter.SoulCoveterAvailableRewardList.Count; i++)
        {
            int nRewardIndex = i;
            Transform trSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trSlot == null)
            {
                trSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trSlot.gameObject.name = "Reward" + i;
            }
            else
            {
                trSlot.gameObject.SetActive(true);
            }

            Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csSoulCoveter.SoulCoveterAvailableRewardList[nRewardIndex].Item.Image);

            Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csSoulCoveter.SoulCoveterAvailableRewardList[i].Item.Name;

            Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csSoulCoveter.SoulCoveterAvailableRewardList[nRewardIndex].Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        //소탕조건
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        //버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)

        if (CsDungeonManager.Instance.SoulCoveterMatchingState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            if (csSoulCoveter.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
            {
                if (csSoulCoveter.SoulCoveterWeeklyPlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.SoulCoveterWeeklyEnterCount)
                {
                    if (csSoulCoveter.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                    }
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
        }
        else
        {
            //매칭중
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateFearAltar()
	{
		CsFearAltar csFearAltar = CsGameData.Instance.FearAltar;

		int DungeonNum = m_csSubMenu.SubMenuId - 1100;
		Image imageBack = m_trBack.GetComponent<Image>();
		imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungeonNum + "_" + m_nDungeonIndex);

		Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
		ImagePartyDungeon.gameObject.SetActive(true);
		ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + DungeonNum + "_" + m_nDungeonIndex);

		m_textDungeonName.text = csFearAltar.Name;
		m_textDungeonDesc.text = csFearAltar.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);

		Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

		Transform trInfoLevel = trInfoList.Find("InfoLevel");
		trInfoLevel.gameObject.SetActive(true);
		m_textOpenLevel.text = csFearAltar.RequiredHeroLevel.ToString();

		Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
		trInfoBattlePower.gameObject.SetActive(false);

		Transform trInfoTime = trInfoList.Find("InfoTime");
		trInfoTime.gameObject.SetActive(false);

		Transform trInfoStamina = trInfoList.Find("InfoStamina");
		trInfoStamina.gameObject.SetActive(true);
		m_textStamina.text = csFearAltar.RequiredStamina.ToString("#,##0");

		Transform trInfoCount = trInfoList.Find("InfoCount");
		trInfoCount.gameObject.SetActive(true);
		Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
		textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
		m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.FearAltarEnterCount - CsGameData.Instance.FearAltar.DailyFearAltarPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.FearAltarEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

		for (int i = 0; i < m_trDifficultyContent.childCount; i++)
		{
			m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
		}

		if (m_goRewardItem == null)
		{
			m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
		}

		// 보상 
		m_trRewardList.gameObject.SetActive(true);

		for (int i = 0; i < m_trRewardContent.childCount; i++)
		{
			m_trRewardContent.GetChild(i).gameObject.SetActive(false);
		}

		Transform trExp = m_trRewardContent.Find("Exp");
		trExp.gameObject.SetActive(true);

		Text textExp = trExp.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textExp);
		textExp.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");

		Text textExpValue = trExp.Find("TextCount").GetComponent<Text>();
		CsUIData.Instance.SetFont(textExpValue);

		CsFearAltarReward csFearAltarReward = CsGameData.Instance.FearAltar.GetFearAltarReward();

		if (csFearAltarReward != null)
		{
			textExpValue.text = csFearAltarReward.ExpReward.Value.ToString("#,##0");
		}

		// 도감
		m_buttonSweep.gameObject.SetActive(true);
		m_textSweep.text = CsConfiguration.Instance.GetString("A116_BTN_00001");

        m_buttonPointRecord.gameObject.SetActive(false);
		
		//버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)
		if (CsDungeonManager.Instance.FearAltarMatchingState == EnDungeonMatchingState.None)
		{
			m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

			if (csFearAltar.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
			{
				if (csFearAltar.DailyFearAltarPlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.FearAltarEnterCount)
				{
					if (csFearAltar.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
					{
						CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
					}
					else
					{
						CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
					}
				}
				else
				{
					CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
				}
			}
			else
			{
				CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
			}
		}
		else
		{
			//매칭중
			m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateRuinsReclaimDungeon()
	{
		CsRuinsReclaim csRuinsReclaim = CsGameData.Instance.RuinsReclaim;

		int DungeonNum = m_csSubMenu.SubMenuId - 1100;
		Image imageBack = m_trBack.GetComponent<Image>();
		imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + DungeonNum + "_" + m_nDungeonIndex);

		Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
		ImagePartyDungeon.gameObject.SetActive(true);
		ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + DungeonNum + "_" + m_nDungeonIndex);

		m_textDungeonName.text = csRuinsReclaim.Name;
		m_textDungeonDesc.text = csRuinsReclaim.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(true);

		Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

		for (int i = 0; i < trContent.childCount; i++)
		{
			trContent.GetChild(i).gameObject.SetActive(false);
		}

		GameObject goTimeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/TimeItem");

		int nChildIndex = 0;

		foreach (var schedule in csRuinsReclaim.RuinsReclaimOpenScheduleList)
		{
			Transform trItem;

			if (nChildIndex < trContent.childCount)
			{
				trItem = trContent.GetChild(nChildIndex);
				trItem.gameObject.SetActive(true);
			}
			else
			{
				trItem = Instantiate(goTimeItem, trContent).transform;
			}

			trItem.name = "TimeItem" + nChildIndex;

			DateTime dtStartTime = new DateTime().AddSeconds(schedule.StartTime);
			DateTime dtEndTime = new DateTime().AddSeconds(schedule.EndTime);

			Text textTime = trItem.Find("Text").GetComponent<Text>();
			CsUIData.Instance.SetFont(textTime);
			textTime.text = string.Format(CsConfiguration.Instance.GetString("A110_TXT_00002"),
				dtStartTime.Hour.ToString("00"), dtStartTime.Minute.ToString("00"), dtEndTime.Hour.ToString("00"), dtEndTime.Minute.ToString("00"));

			nChildIndex++;
		}

		Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

		Transform trInfoLevel = trInfoList.Find("InfoLevel");
		trInfoLevel.gameObject.SetActive(true);
		m_textOpenLevel.text = csRuinsReclaim.RequiredHeroLevel.ToString();

		Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
		trInfoBattlePower.gameObject.SetActive(false);

		Transform trInfoTime = trInfoList.Find("InfoTime");
		trInfoTime.gameObject.SetActive(false);

		Transform trInfoStamina = trInfoList.Find("InfoStamina");
		trInfoStamina.gameObject.SetActive(false);
		
		Transform trInfoCount = trInfoList.Find("InfoCount");
		trInfoCount.gameObject.SetActive(true);
		Text textCount = trInfoList.Find("InfoCount/TextInfo").GetComponent<Text>();
		textCount.text = CsConfiguration.Instance.GetString("A17_TXT_00007");
		m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), Math.Max(0, 1 - csRuinsReclaim.FreePlayCount), 1);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(true);

        Image imageRequiredItemIcon = trInfoRequiredItem.Find("ImageIcon").GetComponent<Image>();
        imageRequiredItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csRuinsReclaim.EnterRequiredItem.Image);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Text textInfoRequiredItem = trInfoRequiredItem.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfoRequiredItem);
        textInfoRequiredItem.text = csRuinsReclaim.EnterRequiredItem.Name;

        Text textValueRequiredItem = trInfoRequiredItem.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValueRequiredItem);
        textValueRequiredItem.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.GetItemCount(csRuinsReclaim.EnterRequiredItem.ItemId), 1);

        m_toggleTeam.gameObject.SetActive(false);

		for (int i = 0; i < m_trDifficultyContent.childCount; i++)
		{
			m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
		}

		if (m_goRewardItem == null)
		{
			m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
		}

		// 보상 
		m_trRewardList.gameObject.SetActive(true);

		for (int i = 0; i < m_trRewardContent.childCount; i++)
		{
			m_trRewardContent.GetChild(i).gameObject.SetActive(false);
		}

		for (int i = 0; i < csRuinsReclaim.RuinsReclaimAvailableRewardList.Count; i++)
		{
			int nRewardIndex = i;
			Transform trSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

			if (trSlot == null)
			{
				trSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
				trSlot.gameObject.name = "Reward" + i;
			}
			else
			{
				trSlot.gameObject.SetActive(true);
			}

			Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
			imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csRuinsReclaim.RuinsReclaimAvailableRewardList[nRewardIndex].Item.Image);

			Text textItemName = trSlot.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textItemName);
			textItemName.text = csRuinsReclaim.RuinsReclaimAvailableRewardList[i].Item.Name;

			Text textItemCount = trSlot.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textItemCount);
			textItemCount.text = "";

			Button buttonReward = trSlot.GetComponent<Button>();
			buttonReward.onClick.RemoveAllListeners();
			buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csRuinsReclaim.RuinsReclaimAvailableRewardList[nRewardIndex].Item));
			buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		}

		//소탕조건
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

		//버튼 조건 만들기. 입장조건(레벨,스테미너,입장횟수)
		if (CsDungeonManager.Instance.RuinsReclaimMatchingState == EnDungeonMatchingState.None)
		{
			m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

			bool bEnterable = false;
			
			if (csRuinsReclaim.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
			{
				// 입장 횟수 & 재료 체크
				if (csRuinsReclaim.FreePlayCount < csRuinsReclaim.FreeEnterCount ||
					CsGameData.Instance.MyHeroInfo.GetItemCount(CsDungeonManager.Instance.RuinsReclaim.EnterRequiredItem.ItemId) > 0)
				{
					// 입장 시간 체크
					int sSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

					foreach (var schedule in CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList)
					{
						if (schedule.StartTime <= sSeconds && sSeconds < schedule.EndTime)
						{
							bEnterable = true;
							break;
						}
					}
				}
			}
			
			CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
		}
		else
		{
			//매칭중
			m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
		}
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateInfiniteWarDungeon()
    {
        CsInfiniteWar csInfiniteWar = CsGameData.Instance.InfiniteWar;

        int nDungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + nDungoenNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + nDungoenNum + "_" + m_nDungeonIndex);
        ImagePartyDungeon.gameObject.SetActive(true);

        m_textDungeonName.text = csInfiniteWar.Name;
        m_textDungeonDesc.text = csInfiniteWar.Description;

        m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(true);

        Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        GameObject goTimeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/TimeItem");

        Transform trItem = null;

        for (int i = 0; i < csInfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
        {
            if (i < trContent.childCount)
            {
                trItem = trContent.GetChild(i);
                trItem.gameObject.SetActive(true);
            }
            else
            {
                trItem = Instantiate(goTimeItem, trContent).transform;
            }

            trItem.name = "TimeItem" + i;

            CsInfiniteWarOpenSchedule csInfiniteWarOpenSchedule = csInfiniteWar.InfiniteWarOpenScheduleList[i];

            DateTime dtStartTime = new DateTime().AddSeconds(csInfiniteWarOpenSchedule.StartTime);
            DateTime dtEndTime = new DateTime().AddSeconds(csInfiniteWarOpenSchedule.EndTime);

            Text textTime = trItem.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTime);
            textTime.text = string.Format(CsConfiguration.Instance.GetString("A110_TXT_00002"), dtStartTime.Hour.ToString("00"), dtStartTime.Minute.ToString("00"), dtEndTime.Hour.ToString("00"), dtEndTime.Minute.ToString("00"));
        }

        for (int i = 0; i < m_trDifficultyContent.childCount; i++)
        {
            m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
        }

        // 던전 정보
        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        m_textOpenLevel.text = csInfiniteWar.RequiredHeroLevel.ToString();

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(true);
        m_textStamina.text = csInfiniteWar.RequiredStamina.ToString("#,##0");

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), Math.Max(0, csInfiniteWar.EnterCount - csInfiniteWar.DailyPlayCount), csInfiniteWar.EnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        m_toggleTeam.gameObject.SetActive(false);

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        // 보상 
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trItemRewardSlot = null;
        for (int i = 0; i < csInfiniteWar.InfiniteWarAvailableRewardList.Count; i++)
        {
            int nRewardIndex = i;
            trItemRewardSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trItemRewardSlot == null)
            {
                trItemRewardSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trItemRewardSlot.name = "Reward" + i;
            }
            else
            {
                trItemRewardSlot.gameObject.SetActive(true);
            }

            CsInfiniteWarAvailableReward csInfiniteWarAvailableReward = csInfiniteWar.InfiniteWarAvailableRewardList[nRewardIndex];

            Image imageIcon = trItemRewardSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csInfiniteWarAvailableReward.Item.Image);

            Text textItemName = trItemRewardSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csInfiniteWarAvailableReward.Item.Name;

            Text textItemCount = trItemRewardSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trItemRewardSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csInfiniteWarAvailableReward.Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        // 소탕 불가
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        if (CsDungeonManager.Instance.InfiniteWarMatchingState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            bool bEnterable = false;
            
            if (csInfiniteWar.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                if (csInfiniteWar.DailyPlayCount < csInfiniteWar.EnterCount ||
                    csInfiniteWar.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
                {
                    int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                    for (int i = 0; i < csInfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
                    {
                        if (csInfiniteWar.InfiniteWarOpenScheduleList[i].StartTime <= nSeconds && nSeconds < csInfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                        {
                            bEnterable = true;
                            break;
                        }
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
        }
        else
        {
            //매칭중
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }
    
    //---------------------------------------------------------------------------------------------------
    void UpdateWarMemoryDungeon()
    {
        CsWarMemory csWarMemory = CsGameData.Instance.WarMemory;

        int nDungeonNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + nDungeonNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + nDungeonNum + "_" + m_nDungeonIndex);
        ImagePartyDungeon.gameObject.SetActive(true);

        m_textDungeonName.text = csWarMemory.Name;
        m_textDungeonDesc.text = csWarMemory.Description;

        m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(true);
        Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        GameObject goTimeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/TimeItem");
        Transform trItem = null;

        for (int i = 0; i < csWarMemory.WarMemoryScheduleList.Count; i++)
        {
            if (i < trContent.childCount)
            {
                trItem = trContent.GetChild(i);
                trItem.gameObject.SetActive(true);
            }
            else
            {
                trItem = Instantiate(goTimeItem, trContent).transform;
            }

            trItem.name = "TimeItem" + i;

            CsWarMemorySchedule csWarMemorySchedule = csWarMemory.WarMemoryScheduleList[i];

            DateTime dtStartTime = new DateTime().AddSeconds(csWarMemorySchedule.StartTime);
            DateTime dtEndTime = new DateTime().AddSeconds(csWarMemorySchedule.EndTime);

            Text textTime = trItem.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTime);
            textTime.text = string.Format(CsConfiguration.Instance.GetString("A110_TXT_00002"), dtStartTime.Hour.ToString("00"), dtStartTime.Minute.ToString("00"), dtEndTime.Hour.ToString("00"), dtEndTime.Minute.ToString("00"));
        }

        for (int i = 0; i < m_trDifficultyContent.childCount; i++)
        {
            m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
        }

        // 던전 정보
        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        m_textOpenLevel.text = csWarMemory.RequiredHeroLevel.ToString();

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(false);

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), Math.Max(0, csWarMemory.FreeEnterCount - csWarMemory.FreePlayCount), csWarMemory.FreeEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(false);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(true);

        CsItem csItem = CsGameData.Instance.GetItem(csWarMemory.EnterRequiredItemId);

        Image imageRequiredItemIcon = trInfoRequiredItem.Find("ImageIcon").GetComponent<Image>();
        imageRequiredItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);

        Text textInfoRequiredItem = trInfoRequiredItem.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfoRequiredItem);
        textInfoRequiredItem.text = csItem.Name;

        Text textValueRequiredItem = trInfoRequiredItem.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValueRequiredItem);
        textValueRequiredItem.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId), 1);

        m_toggleTeam.gameObject.SetActive(false);

        // 던전 정보
        
        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        // 보상 
        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trItemRewardSlot = null;
        for (int i = 0; i < csWarMemory.WarMemoryAvailableRewardList.Count; i++)
        {
            int nRewardIndex = i;
            trItemRewardSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trItemRewardSlot == null)
            {
                trItemRewardSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trItemRewardSlot.name = "Reward" + i;
            }
            else
            {
                trItemRewardSlot.gameObject.SetActive(true);
            }

            CsWarMemoryAvailableReward csWarMemoryAvailableReward = csWarMemory.WarMemoryAvailableRewardList[nRewardIndex];

            Image imageIcon = trItemRewardSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csWarMemoryAvailableReward.Item.Image);

            Text textItemName = trItemRewardSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csWarMemoryAvailableReward.Item.Name;

            Text textItemCount = trItemRewardSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trItemRewardSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csWarMemoryAvailableReward.Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        // 소탕 불가
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        if (CsDungeonManager.Instance.WarMemoryMatchingState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            bool bEnterable = false;

            if (csWarMemory.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                if (csWarMemory.FreePlayCount < csWarMemory.FreeEnterCount || 0 < CsGameData.Instance.MyHeroInfo.GetItemCount(csWarMemory.EnterRequiredItemId))
                {
                    int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                    for (int i = 0; i < csWarMemory.WarMemoryScheduleList.Count; i++)
                    {
                        if (csWarMemory.WarMemoryScheduleList[i].StartTime <= nSeconds && nSeconds < csWarMemory.WarMemoryScheduleList[i].EndTime)
                        {
                            bEnterable = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
        }
        else
        {
            //매칭중
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDragonNestDungeon()
    {
        CsDragonNest csDragonNest = CsGameData.Instance.DragonNest;

        int nDungeonNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + nDungeonNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + nDungeonNum + "_" + m_nDungeonIndex);
        ImagePartyDungeon.gameObject.SetActive(true);

        m_textDungeonName.text = csDragonNest.Name;
        m_textDungeonDesc.text = csDragonNest.Description;

        m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(false);
        Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < m_trDifficultyContent.childCount; i++)
        {
            m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
        }

        // 던전 정보
        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);
        m_textOpenLevel.text = csDragonNest.RequiredHeroLevel.ToString();

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(false);

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(false);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(true);

        m_textEnterMember.text = string.Format(CsConfiguration.Instance.GetString("A144_TXT_00015"), csDragonNest.EnterMinMemberCount, csDragonNest.EnterMaxMemberCount);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(true);

        Image imageRequiredItemIcon = trInfoRequiredItem.Find("ImageIcon").GetComponent<Image>();

        Text textInfoRequiredItem = trInfoRequiredItem.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfoRequiredItem);

        Text textValueRequiredItem = trInfoRequiredItem.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValueRequiredItem);

        m_toggleTeam.gameObject.SetActive(true);

        if (csDragonNest.EnterRequiredItem == null)
        {
            imageRequiredItemIcon.sprite = null;
            textInfoRequiredItem.text = "";
            textValueRequiredItem.text = "";
        }
        else
        {
            imageRequiredItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csDragonNest.EnterRequiredItem.Image);
            textInfoRequiredItem.text = csDragonNest.EnterRequiredItem.Name;
            textValueRequiredItem.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.GetItemCount(csDragonNest.EnterRequiredItem.ItemId), 1);
        }

        // 던전 정보

        // 보상 
        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trItemRewardSlot = null;

        for (int i = 0; i < csDragonNest.DragonNestAvailableRewardList.Count; i++)
        {
            int nRewardIndex = i;
            trItemRewardSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trItemRewardSlot == null)
            {
                trItemRewardSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trItemRewardSlot.name = "Reward" + i;
            }
            else
            {
                trItemRewardSlot.gameObject.SetActive(true);
            }

            CsDragonNestAvailableReward csDragonNestAvailableReward = csDragonNest.DragonNestAvailableRewardList[nRewardIndex];

            Image imageIcon = trItemRewardSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csDragonNestAvailableReward.Item.Image);

            Text textItemName = trItemRewardSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csDragonNestAvailableReward.Item.Name;

            Text textItemCount = trItemRewardSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trItemRewardSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csDragonNestAvailableReward.Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        // 소탕 불가
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(false);

        if (CsDungeonManager.Instance.DragonNestMatchingState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            bool bEnterable = false;

            if (csDragonNest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
                0 < CsGameData.Instance.MyHeroInfo.GetItemCount(csDragonNest.EnterRequiredItem.ItemId))
            {
                bEnterable = true;
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
        }
        else
        {
            //매칭중
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTradeShipDungeon()
    {
        CsTradeShip csTradeShip = CsDungeonManager.Instance.TradeShip;

        int nDungeonNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + nDungeonNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + nDungeonNum + "_" + m_nDungeonIndex);
        ImagePartyDungeon.gameObject.SetActive(true);

        m_textDungeonName.text = csTradeShip.Name;
        m_textDungeonDesc.text = csTradeShip.Description;

        m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(true);
        Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        GameObject goTimeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/TimeItem");
        Transform trItem = trContent.Find("TimeItem");

        if (trItem == null)
        {
            trItem = Instantiate(goTimeItem, trContent).transform;
            trItem.name = "TimeItem";
        }
        else
        {
            trItem.gameObject.SetActive(true);
        }

        LayoutElement layoutElement = trItem.GetComponent<LayoutElement>();
        layoutElement.preferredWidth = 210f;

        Text textTime = trItem.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTime);
        textTime.text = CsConfiguration.Instance.GetString("A162_TXT_00004");

        for (int i = 0; i < m_trDifficultyContent.childCount; i++)
        {
            m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
        }

        // 던전 정보
        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);

        CsTradeShipDifficulty csTradeShipDifficulty = csTradeShip.TradeShipDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

        if (csTradeShipDifficulty == null)
        {
            m_textOpenLevel.text = csTradeShip.RequiredHeroLevel.ToString();
            m_nDiffiCulty = 0;
        }
        else
        {
            m_textOpenLevel.text = csTradeShipDifficulty.MinHeroLevel.ToString();
            m_nDiffiCulty = csTradeShipDifficulty.Difficulty;
        }

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(false);

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), Math.Max(0, CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount - csTradeShip.PlayCount), CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(true);

        m_textEnterMember.text = string.Format(CsConfiguration.Instance.GetString("A162_TXT_00005"), csTradeShip.EnterMaxMemberCount);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        // 보상 

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trItemRewardSlot = null;

        List<CsTradeShipAvailableReward> listCsTradeShipAvailableReward = new List<CsTradeShipAvailableReward>(csTradeShipDifficulty.TradeShipAvailableRewardList).FindAll(a => a.Difficulty == csTradeShipDifficulty.Difficulty);

        for (int i = 0; i < listCsTradeShipAvailableReward.Count; i++)
        {
            int nRewardIndex = i;
            trItemRewardSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trItemRewardSlot == null)
            {
                trItemRewardSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trItemRewardSlot.name = "Reward" + i;
            }
            else
            {
                trItemRewardSlot.gameObject.SetActive(true);
            }

            CsTradeShipAvailableReward csTradeShipAvailableReward = listCsTradeShipAvailableReward[nRewardIndex];

            Image imageIcon = trItemRewardSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csTradeShipAvailableReward.Item.Image);

            Text textItemName = trItemRewardSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csTradeShipAvailableReward.Item.Name;

            Text textItemCount = trItemRewardSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trItemRewardSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csTradeShipAvailableReward.Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        // 소탕 불가
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(true);

        if (CsDungeonManager.Instance.TradeShipMatchingState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            bool bEnterable = false;

            if (csTradeShip.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                if (csTradeShip.PlayCount < CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount && csTradeShip.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
                {
                    int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                    for (int i = 0; i < csTradeShip.TradeShipScheduleList.Count; i++)
                    {
                        if (csTradeShip.TradeShipScheduleList[i].StartTime <= nSeconds && nSeconds < csTradeShip.TradeShipScheduleList[i].EndTime)
                        {
                            bEnterable = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
        }
        else
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAnkouTombDungeon()
    {
        CsAnkouTomb csAnkouTomb = CsDungeonManager.Instance.AnkouTomb;

        int nDungeonNum = m_csSubMenu.SubMenuId - 1100;
        Image imageBack = m_trBack.GetComponent<Image>();
        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + nDungeonNum + "_" + m_nDungeonIndex);

        Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
        ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + nDungeonNum + "_" + m_nDungeonIndex);
        ImagePartyDungeon.gameObject.SetActive(true);

        m_textDungeonName.text = csAnkouTomb.Name;
        m_textDungeonDesc.text = csAnkouTomb.Description;

        m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(true);
        Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        GameObject goTimeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/TimeItem");
        Transform trItem = trContent.Find("TimeItem");

        if (trItem == null)
        {
            trItem = Instantiate(goTimeItem, trContent).transform;
            trItem.name = "TimeItem";
        }
        else
        {
            trItem.gameObject.SetActive(true);
        }

        LayoutElement layoutElement = trItem.GetComponent<LayoutElement>();
        layoutElement.preferredWidth = 210f;

        Text textTime = trItem.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTime);
        textTime.text = CsConfiguration.Instance.GetString("A162_TXT_00002");

        for (int i = 0; i < m_trDifficultyContent.childCount; i++)
        {
            m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
        }

        // 던전 정보
        Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

        Transform trInfoLevel = trInfoList.Find("InfoLevel");
        trInfoLevel.gameObject.SetActive(true);

        CsAnkouTombDifficulty csAnkouTombDifficulty = csAnkouTomb.AnkouTombDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

        if (csAnkouTombDifficulty == null)
        {
            m_nDiffiCulty = 0;
        }
        else
        {
            m_textOpenLevel.text = csAnkouTombDifficulty.MinHeroLevel.ToString();
            m_nDiffiCulty = csAnkouTombDifficulty.Difficulty;
        }

        Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
        trInfoBattlePower.gameObject.SetActive(false);

        Transform trInfoTime = trInfoList.Find("InfoTime");
        trInfoTime.gameObject.SetActive(false);

        Transform trInfoStamina = trInfoList.Find("InfoStamina");
        trInfoStamina.gameObject.SetActive(false);

        Transform trInfoCount = trInfoList.Find("InfoCount");
        trInfoCount.gameObject.SetActive(true);
        m_textCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00002"), Math.Max(0, csAnkouTomb.EnterCount - csAnkouTomb.PlayCount), csAnkouTomb.EnterCount);

        Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
        trInfoEnterMember.gameObject.SetActive(true);

        m_textEnterMember.text = string.Format(CsConfiguration.Instance.GetString("A162_TXT_00005"), csAnkouTomb.EnterMaxMemberCount);

        Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
        trInfoRequiredItem.gameObject.SetActive(false);

        // 보상 

        if (m_goRewardItem == null)
        {
            m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
        }

        m_trRewardList.gameObject.SetActive(true);

        for (int i = 0; i < m_trRewardContent.childCount; i++)
        {
            m_trRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trItemRewardSlot = null;

        List<CsAnkouTombAvailableReward> listCsAnkouTombAvailableReward = new List<CsAnkouTombAvailableReward>(csAnkouTombDifficulty.AnkouTombAvailableRewardList).FindAll(a => a.Difficulty == csAnkouTombDifficulty.Difficulty);

        for (int i = 0; i < listCsAnkouTombAvailableReward.Count; i++)
        {
            int nRewardIndex = i;
            trItemRewardSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

            if (trItemRewardSlot == null)
            {
                trItemRewardSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
                trItemRewardSlot.name = "Reward" + i;
            }
            else
            {
                trItemRewardSlot.gameObject.SetActive(true);
            }

            CsAnkouTombAvailableReward csAnkouTombAvailableReward = listCsAnkouTombAvailableReward[nRewardIndex];

            Image imageIcon = trItemRewardSlot.Find("Image").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csAnkouTombAvailableReward.Item.Image);

            Text textItemName = trItemRewardSlot.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csAnkouTombAvailableReward.Item.Name;

            Text textItemCount = trItemRewardSlot.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemCount);
            textItemCount.text = "";

            Button buttonReward = trItemRewardSlot.GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csAnkouTombAvailableReward.Item));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        // 소탕 불가
        m_buttonSweep.gameObject.SetActive(false);
        m_buttonPointRecord.gameObject.SetActive(true);

        if (CsDungeonManager.Instance.AnkouTombMatchingState == EnDungeonMatchingState.None)
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

            bool bEnterable = false;

            if (csAnkouTomb.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                if (csAnkouTomb.PlayCount < csAnkouTomb.EnterCount && csAnkouTomb.RequiredStamina <= CsGameData.Instance.MyHeroInfo.Stamina)
                {
                    int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                    for (int i = 0; i < csAnkouTomb.AnkouTombScheduleList.Count; i++)
                    {
                        if (csAnkouTomb.AnkouTombScheduleList[i].StartTime <= nSeconds && nSeconds < csAnkouTomb.AnkouTombScheduleList[i].EndTime)
                        {
                            bEnterable = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
        }
        else
        {
            m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00002");
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateTeamBattlefieldDungeon(bool bIsEnterableTime, bool bIsOpened, bool bIsFinished, int nMemberCount)
	{
		CsTeamBattlefield csTeamBattlefield = CsGameData.Instance.TeamBattlefield;

		int nDungeonNum = m_csSubMenu.SubMenuId - 1100;
		Image imageBack = m_trBack.GetComponent<Image>();
		imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/bg_dungeon_" + nDungeonNum + "_" + m_nDungeonIndex);

		Image ImagePartyDungeon = m_trBack.Find("ImagePartyDungeon").GetComponent<Image>();
		ImagePartyDungeon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/dungeon_card_" + nDungeonNum + "_" + m_nDungeonIndex);
		ImagePartyDungeon.gameObject.SetActive(true);

		m_textDungeonName.text = csTeamBattlefield.Name;
		m_textDungeonDesc.text = csTeamBattlefield.Description;

		m_trBack.Find("DungeonInfo/FrameTime").gameObject.SetActive(true);
		Transform trContent = m_trBack.Find("DungeonInfo/FrameTime/Scroll View/Viewport/Content");

		for (int i = 0; i < trContent.childCount; i++)
		{
			trContent.GetChild(i).gameObject.SetActive(false);
		}

		GameObject goTimeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/TimeItem");
		Transform trItem = trContent.Find("TimeItem");

		if (trItem == null)
		{
			trItem = Instantiate(goTimeItem, trContent).transform;
			trItem.name = "TimeItem";
		}
		else
		{
			trItem.gameObject.SetActive(true);
		}

		LayoutElement layoutElement = trItem.GetComponent<LayoutElement>();
		layoutElement.preferredWidth = 210f;

		Text textTime = trItem.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTime);
		//textTime.text = CsConfiguration.Instance.GetString("A162_TXT_00002");
		// %% 클텍 추가, 요일 추가
		TimeSpan timeSpan = TimeSpan.FromSeconds(csTeamBattlefield.OpenTime);
		textTime.text = string.Format(/*CsConfiguration.Instance.GetString("")*/"{0}:{1}", timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"));

		for (int i = 0; i < m_trDifficultyContent.childCount; i++)
		{
			m_trDifficultyContent.GetChild(i).gameObject.SetActive(false);
		}

		// 던전 정보
		Transform trInfoList = m_trBack.Find("DungeonInfo/InfoList");

		Transform trInfoLevel = trInfoList.Find("InfoLevel");
		trInfoLevel.gameObject.SetActive(false);
		
		Transform trInfoBattlePower = trInfoList.Find("InfoBattlePower");
		trInfoBattlePower.gameObject.SetActive(true);
		m_textRecomandCp.text = csTeamBattlefield.RecommendBattlePower.ToString("#,##0");

		Transform trInfoTime = trInfoList.Find("InfoTime");
		trInfoTime.gameObject.SetActive(true);
		m_textChallengeTime.text = string.Format(CsConfiguration.Instance.GetString("A17_TXT_00009"), csTeamBattlefield.LimitTime.ToString("#0"));

		Transform trInfoStamina = trInfoList.Find("InfoStamina");
		trInfoStamina.gameObject.SetActive(false);

		Transform trInfoCount = trInfoList.Find("InfoCount");
		trInfoCount.gameObject.SetActive(false);
		
		Transform trInfoEnterMember = trInfoList.Find("InfoEnterMember");
		trInfoEnterMember.gameObject.SetActive(true);

		m_textEnterMember.text = string.Format(CsConfiguration.Instance.GetString("A162_TXT_00005"), csTeamBattlefield.EnterMaxMemberCount);

		Transform trInfoRequiredItem = trInfoList.Find("InfoRequiredItem");
		trInfoRequiredItem.gameObject.SetActive(false);

		// 보상 
		if (m_goRewardItem == null)
		{
			m_goRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonRewardItem");
		}

		m_trRewardList.gameObject.SetActive(true);

		for (int i = 0; i < m_trRewardContent.childCount; i++)
		{
			m_trRewardContent.GetChild(i).gameObject.SetActive(false);
		}

		Transform trItemRewardSlot = null;

		for (int i = 0; i < csTeamBattlefield.TeamBattlefieldAvailableRewardList.Count; i++)
		{
			int nRewardIndex = i;
			trItemRewardSlot = m_trRewardContent.Find("Reward" + nRewardIndex);

			if (trItemRewardSlot == null)
			{
				trItemRewardSlot = Instantiate(m_goRewardItem, m_trRewardContent).transform;
				trItemRewardSlot.name = "Reward" + i;
			}
			else
			{
				trItemRewardSlot.gameObject.SetActive(true);
			}

			CsTeamBattlefieldAvailableReward csTeamBattlefieldAvailableReward = csTeamBattlefield.TeamBattlefieldAvailableRewardList[nRewardIndex];

			Image imageIcon = trItemRewardSlot.Find("Image").GetComponent<Image>();
			imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csTeamBattlefieldAvailableReward.Item.Image);

			Text textItemName = trItemRewardSlot.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textItemName);
			textItemName.text = csTeamBattlefieldAvailableReward.Item.Name;

			Text textItemCount = trItemRewardSlot.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textItemCount);
			textItemCount.text = "";

			Button buttonReward = trItemRewardSlot.GetComponent<Button>();
			buttonReward.onClick.RemoveAllListeners();
			buttonReward.onClick.AddListener(() => OnClickDungeonRewardInfo(csTeamBattlefieldAvailableReward.Item));
			buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		}

		// 소탕 불가
		m_buttonSweep.gameObject.SetActive(false);
		m_buttonPointRecord.gameObject.SetActive(true);

		//%% 클텍 변경
		m_textEnterDungeon.text = CsConfiguration.Instance.GetString("A40_BTN_00001");

		bool bEnterable = bIsEnterableTime && bIsOpened && !bIsFinished && nMemberCount < csTeamBattlefield.EnterMaxMemberCount &&
							csTeamBattlefield.TeamBattlefieldOpenDayOfWeekList.Find(dayOfWeek => dayOfWeek.DayOfWeek == (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek) == null;

		CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, bEnterable);
	}

    //---------------------------------------------------------------------------------------------------
    void StoryDungeonSweep()
    {
        CsDungeonManager.Instance.SendStoryDungeonSweep(CsGameData.Instance.StoryDungeonList[m_nDungeonIndex].DungeonNo, m_nDiffiCulty);
    }

    //---------------------------------------------------------------------------------------------------
    void GoldDungeonSweep()
    {
        CsDungeonManager.Instance.SendGoldDungeonSweep(m_nDiffiCulty);
    }

    //---------------------------------------------------------------------------------------------------
    void ExpDungeonSweep()
    {
        CsDungeonManager.Instance.SendExpDungeonSweep(m_nDiffiCulty);
    }

	//---------------------------------------------------------------------------------------------------
	void WisdomTempleSweep()
	{
		CsDungeonManager.Instance.SendWisdomTempleSweep();
	}

    //---------------------------------------------------------------------------------------------------
    void DungeonSweepBuyDia()
    {

    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csItem)
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        Transform trPopupList = trCanvas2.Find("PopupList");

        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, 1, false, -1, false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupMatchingSelect(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/PartyDungeonModal");
        yield return resourceRequest;
        m_goMatchingSelect = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupMatchingSelect(EnPartyDungeonType enPartyDungeonType)
    {
        Transform trPopup = GameObject.Find("Canvas2").transform.Find("PopupList");
        Transform trPopupSelect = trPopup.Find("PopupMatchingSelect");

        if (trPopupSelect == null)
        {
            GameObject goPopupSelect = Instantiate(m_goMatchingSelect, trPopup);
            goPopupSelect.name = "PopupMatchingSelect";
            trPopupSelect = goPopupSelect.transform;
        }

        Transform trModal = trPopupSelect.Find("CommonModal");

        Button buttonClose = trModal.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickCloseMatchingSelect);

        Text textMessage = trModal.Find("TextMessage").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMessage);
        textMessage.text = CsConfiguration.Instance.GetString("A36_TXT_03007");

        Button buttonSolo = trModal.Find("Buttons/Button2").GetComponent<Button>();
        buttonSolo.onClick.RemoveAllListeners();
        buttonSolo.onClick.AddListener(() => OnClickSoloMatching(enPartyDungeonType));

        Text textSolo = buttonSolo.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSolo);
        textSolo.text = CsConfiguration.Instance.GetString("A36_BTN_00011");

        Button buttonParty = trModal.Find("Buttons/Button1").GetComponent<Button>();
        buttonParty.onClick.RemoveAllListeners();
        buttonParty.onClick.AddListener(() => OnClickPartyMatching(enPartyDungeonType));

        Text textParty = buttonParty.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textParty);
        textParty.text = CsConfiguration.Instance.GetString("A36_BTN_00010");
    }

	//---------------------------------------------------------------------------------------------------
	void OpenPopupMatchingSelect(EnTimeLimitDungeonType eEventDungeonType)
	{
		Transform trPopup = GameObject.Find("Canvas2").transform.Find("PopupList");
		Transform trPopupSelect = trPopup.Find("PopupMatchingSelect");

		if (trPopupSelect == null)
		{
			GameObject goPopupSelect = Instantiate(m_goMatchingSelect, trPopup);
			goPopupSelect.name = "PopupMatchingSelect";
			trPopupSelect = goPopupSelect.transform;
		}

		Transform trModal = trPopupSelect.Find("CommonModal");

		Button buttonClose = trModal.Find("ButtonClose").GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClickCloseMatchingSelect);

		Text textMessage = trModal.Find("TextMessage").GetComponent<Text>();
		CsUIData.Instance.SetFont(textMessage);
		textMessage.text = CsConfiguration.Instance.GetString("A36_TXT_03007");

		Button buttonSolo = trModal.Find("Buttons/Button2").GetComponent<Button>();
		buttonSolo.onClick.RemoveAllListeners();
		buttonSolo.onClick.AddListener(() => OnClickSoloMatching(eEventDungeonType));

		Text textSolo = buttonSolo.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSolo);
		textSolo.text = CsConfiguration.Instance.GetString("A36_BTN_00011");

		Button buttonParty = trModal.Find("Buttons/Button1").GetComponent<Button>();
		buttonParty.onClick.RemoveAllListeners();
		buttonParty.onClick.AddListener(() => OnClickPartyMatching(eEventDungeonType));

		Text textParty = buttonParty.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textParty);
		textParty.text = CsConfiguration.Instance.GetString("A36_BTN_00010");
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseMatchingSelect()
    {
        Transform trPopup = GameObject.Find("Canvas2").transform.Find("PopupList");
        Transform trPopupSelect = trPopup.Find("PopupMatchingSelect");

        if (trPopupSelect != null)
        {
            Destroy(trPopupSelect.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyMatching(EnPartyDungeonType enPartyDungeonType)
    {
        switch (enPartyDungeonType)
        {
			case EnPartyDungeonType.FearAltar:
				CsDungeonManager.Instance.SendFearAltarMatchingStart(true);
				break;

            case EnPartyDungeonType.AncientRelic:
                CsDungeonManager.Instance.SendAncientRelicMatchingStart(true);
                break;

            case EnPartyDungeonType.SoulCoveter:
                CsDungeonManager.Instance.SendSoulCoveterMatchingStart(true, m_nDiffiCulty);
                break;

            case EnPartyDungeonType.DragonNest:
                CsDungeonManager.Instance.SendDragonNestMatchingStart(true);
                break;
        }

        OnClickCloseMatchingSelect();
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickSoloMatching(EnPartyDungeonType enPartyDungeonType)
	{
		switch (enPartyDungeonType)
		{
			case EnPartyDungeonType.FearAltar:
				CsDungeonManager.Instance.SendFearAltarMatchingStart(false);

				break;
			case EnPartyDungeonType.AncientRelic:
				CsDungeonManager.Instance.SendAncientRelicMatchingStart(false);
				break;

			case EnPartyDungeonType.SoulCoveter:
				CsDungeonManager.Instance.SendSoulCoveterMatchingStart(false, m_nDiffiCulty);
				break;

            case EnPartyDungeonType.DragonNest:
                CsDungeonManager.Instance.SendDragonNestMatchingStart(false);
                break;
		}

		OnClickCloseMatchingSelect();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickPartyMatching(EnTimeLimitDungeonType enEventDungeonType)
	{
		switch (enEventDungeonType)
		{
			case EnTimeLimitDungeonType.RuinsReclaim:
				CheckRuinsReclaimEnter(true);
				break;

            case EnTimeLimitDungeonType.InfiniteWar:
                CheckInfiniteWarEnter();
                break;

            case EnTimeLimitDungeonType.WarMemory:
                CheckWarMemoryEnter(true);
                break;

            case EnTimeLimitDungeonType.AnkouTomb:

                CsAnkouTombDifficulty csAnkouTombDifficulty = CsGameData.Instance.AnkouTomb.AnkouTombDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

                if (csAnkouTombDifficulty == null)
                {
                    return;
                }
                else
                {
                    CheckAnkouTompEnter(true, csAnkouTombDifficulty.Difficulty);
                }

                break;

            case EnTimeLimitDungeonType.TradeShip:

                CsTradeShipDifficulty csTradeShipDifficulty = CsGameData.Instance.TradeShip.TradeShipDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

                if (csTradeShipDifficulty == null)
                {
                    return;
                }
                else
                {
                    CheckTradeShipEnter(true, csTradeShipDifficulty.Difficulty);
                }

                break;
		}

		OnClickCloseMatchingSelect();
	}

    //---------------------------------------------------------------------------------------------------
	void OnClickSoloMatching(EnTimeLimitDungeonType enEventDungeonType)
    {
		switch (enEventDungeonType)
        {
			case EnTimeLimitDungeonType.RuinsReclaim:
				CheckRuinsReclaimEnter(false);
                break;
                
            case EnTimeLimitDungeonType.InfiniteWar:
                CheckInfiniteWarEnter();
                break;

            case EnTimeLimitDungeonType.WarMemory:
                CheckWarMemoryEnter(false);
                break;

            case EnTimeLimitDungeonType.AnkouTomb:

                CsAnkouTombDifficulty csAnkouTombDifficulty = CsGameData.Instance.AnkouTomb.AnkouTombDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

                if (csAnkouTombDifficulty == null)
                {
                    return;
                }
                else
                {
                    CheckAnkouTompEnter(false, csAnkouTombDifficulty.Difficulty);
                }

                break;

            case EnTimeLimitDungeonType.TradeShip:

                CsTradeShipDifficulty csTradeShipDifficulty = CsGameData.Instance.TradeShip.TradeShipDifficultyList.Find(a => a.MinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= a.MaxHeroLevel);

                if (csTradeShipDifficulty == null)
                {
                    return;
                }
                else
                {
                    CheckTradeShipEnter(false, csTradeShipDifficulty.Difficulty);
                }

                break;
        }

        OnClickCloseMatchingSelect();
    }

	//---------------------------------------------------------------------------------------------------
	void CheckRuinsReclaimEnter(bool bParty)
	{
		// 입장 횟수 & 재료 체크
		if (CsGameData.Instance.RuinsReclaim.FreePlayCount >= CsGameData.Instance.RuinsReclaim.FreeEnterCount &&
			CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.RuinsReclaim.EnterRequiredItem.ItemId) <= 0)
			return;
		
		// 입장 시간 체크
		int sSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;
		
		foreach (var schedule in CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList)
		{
			if (schedule.StartTime <= sSeconds && sSeconds < schedule.EndTime)
			{
				CsDungeonManager.Instance.SendRuinsReclaimMatchingStart(bParty);
				break;
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void CheckInfiniteWarEnter()
    {
        CsInfiniteWar csInfiniteWar = CsGameData.Instance.InfiniteWar;

        if (csInfiniteWar.EnterCount <= csInfiniteWar.DailyPlayCount && CsGameData.Instance.MyHeroInfo.Stamina < csInfiniteWar.RequiredStamina)
        {
            return;
        }
        else
        {
            int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

            for (int i = 0; i < csInfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
            {
                if (csInfiniteWar.InfiniteWarOpenScheduleList[i].StartTime <= nSeconds && nSeconds < csInfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                {
                    CsDungeonManager.Instance.SendInfiniteWarMatchingStart();
                    return;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckWarMemoryEnter(bool bPartyEnter)
    {
        if (CsGameData.Instance.WarMemory.FreePlayCount >= CsGameData.Instance.WarMemory.FreeEnterCount && CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.WarMemory.EnterRequiredItemId) <= 0)
        {
            return;
        }
        else
        {
            int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

            foreach (var schedule in CsGameData.Instance.WarMemory.WarMemoryScheduleList)
            {
                if (schedule.StartTime <= nSeconds && nSeconds < schedule.EndTime)
                {
                    CsDungeonManager.Instance.SendWarMemoryMatchingStart(bPartyEnter);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckDragonNestEnter(bool bPartyEnter)
    {
        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.DragonNest.RequiredHeroLevel && CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.DragonNest.EnterRequiredItem.ItemId) <= 0)
        {
            return;
        }
        else
        {
            CsDungeonManager.Instance.SendDragonNestMatchingStart(bPartyEnter);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckTradeShipEnter(bool bPartyEnter, int nDifficulty)
    {
        if (CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount <= CsGameData.Instance.TradeShip.PlayCount || CsGameData.Instance.MyHeroInfo.Stamina < CsGameData.Instance.TradeShip.RequiredStamina)
        {
            return;
        }
        else
        {
            int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

            CsTradeShipSchedule csTradeShipSchedule = null;

            for (int i = 0; i < CsGameData.Instance.TradeShip.TradeShipScheduleList.Count; i++)
            {
                csTradeShipSchedule = CsGameData.Instance.TradeShip.TradeShipScheduleList[i];

                if (csTradeShipSchedule.StartTime <= nSeconds && nSeconds < csTradeShipSchedule.EndTime)
                {
                    CsDungeonManager.Instance.SendTradeShipMatchingStart(bPartyEnter, nDifficulty);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckAnkouTompEnter(bool bPartyEnter, int nDifficulty)
    {
        if (CsGameData.Instance.AnkouTomb.EnterCount <= CsGameData.Instance.AnkouTomb.PlayCount || CsGameData.Instance.MyHeroInfo.Stamina < CsGameData.Instance.AnkouTomb.RequiredStamina)
        {
            return;
        }
        else
        {
            int nSeconds = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

            CsAnkouTombSchedule csAnkouTombSchedule = null;

            for (int i = 0; i < CsGameData.Instance.AnkouTomb.AnkouTombScheduleList.Count; i++)
            {
                csAnkouTombSchedule = CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[i];

                if (csAnkouTombSchedule.StartTime <= nSeconds && nSeconds < csAnkouTombSchedule.EndTime)
                {
                    CsDungeonManager.Instance.SendAnkouTombMatchingStart(bPartyEnter, nDifficulty);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}