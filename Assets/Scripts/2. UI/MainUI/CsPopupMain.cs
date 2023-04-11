using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-22)
//---------------------------------------------------------------------------------------------------

public class CsPopupMain : CsUpdateableMonoBehaviour, IPopupMain
{
    Transform m_trMainPopupSubMenu;
    Transform m_trBackground;
    Transform m_trToggleList;

    Text m_textGold;
    Text m_textOwnDia;
    Text m_textUnOwnDia;
    Text m_textSoulPowder;
    Text m_textPopupName;

    Image m_imageWarehouseLock;

    CsMainMenu m_csMainMenu;
    EnSubMenu m_enSubMenu;

    CsSubMenu m_csSubMenu;

    EnMainMenu m_enPreMainMenu = 0;
    EnSubMenu m_enPreSubMenu = 0;

    float m_flTime = 0;

    bool m_bLoadSubMenu = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trMainPopupSubMenu = GameObject.Find("Canvas2/MainPopupSubMenu").transform;
        m_trBackground = transform.Find("ImageBackGround");

        m_textPopupName = m_trBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPopupName);

        m_trToggleList = m_trBackground.Find("ToggleList");

        CsSubMenu csSubMenu = CsGameData.Instance.GetMainMenu((int)EnMainMenu.Character).GetSubMenu((int)EnSubMenu.Warehouse);
        var tr_WarehouseLock = m_trToggleList.Find("Toggle" + (csSubMenu.SortNo - 1) + "/ImageLock");
        if (tr_WarehouseLock != null)
            m_imageWarehouseLock = tr_WarehouseLock.GetComponent<Image>();          // 창고 잠금 이미지

		CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
		CsGameEventUIToUI.Instance.EventSubGearLevelUp += OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally += OnventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventMountedSoulstoneCompose += OnEventMountedSoulstoneCompose;

        CsGameEventUIToUI.Instance.EventSkillLevelUp += OnEventSkillLevelUp;
        CsGameEventUIToUI.Instance.EventSkillLevelUpTotally += OnEventSkillLevelUpTotally;

        CsGameEventUIToUI.Instance.EventSimpleShopBuy += OnEventSimpleShopBuy;
        CsGameEventUIToUI.Instance.EventSimpleShopSell += OnEventSimpleShopSell;

        CsGameEventUIToUI.Instance.EventInventorySlotExtend += OnEventInventorySlotExtend;

        CsGameEventUIToUI.Instance.EventItemCompose += OnEventItemCompose;
        CsGameEventUIToUI.Instance.EventItemComposeTotally += OnEventItemComposeTotally;

        CsGameEventUIToUI.Instance.EventRestRewardReceiveGold += OnEventRestRewardReceiveGold;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveDia += OnEventRestRewardReceiveDia;

        CsGameEventUIToUI.Instance.EventImmediateRevive += OnEventImmediateRevive;

        CsGameEventUIToUI.Instance.EventReturnScrollUseStart += OnEventReturnScrollUseStart;

        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyDisband += OnEventPartyDisband;
        CsGameEventUIToUI.Instance.EventPartyDisbanded += OnEventPartyDisbanded;
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted += OnEventPartyApplicationAccepted;
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept += OnEventPartyInvitationAccept;
        CsGameEventUIToUI.Instance.EventPartyBanished += OnEventPartyBanished;
        CsGameEventUIToUI.Instance.EventPartyExit += OnEventPartyExit;

        CsMainQuestManager.Instance.EventCompleted += OnEventMainQuestCompleted;

        CsGameEventUIToUI.Instance.EventGearShare += OnEventGearShare;

        CsGameEventUIToUI.Instance.EventMountGearPickBoxMake += OnEventMountGearPickBoxMake;
        CsGameEventUIToUI.Instance.EventMountGearPickBoxMakeTotally += OnEventMountGearPickBoxMakeTotally;

        CsGameEventUIToUI.Instance.EventStartQuestAutoPlay += OnEventStartQuestAutoPlay;

        CsGameEventUIToUI.Instance.EventMiniMapSelected += OnEventMiniMapSelected;

        CsGameEventUIToUI.Instance.EventRankRewardReceive += OnEventRankRewardReceive;

        CsGameEventUIToUI.Instance.EventOpenOneToOneChat += OnEventOpenOneToOneChat;

        CsGameEventUIToUI.Instance.EventNationDonate += OnEventNationDonate;

        CsGameEventUIToUI.Instance.EventStaminaBuy += OnEventStaminaBuy;

        CsGameEventUIToUI.Instance.EventGoldItemUse += OnEventGoldItemUse;
        CsGameEventUIToUI.Instance.EventOwnDiaItemUse += OnEventOwnDiaItemUse;

		CsGameEventUIToUI.Instance.EventRankActiveSkillLevelUp += OnEventRankActiveSkillLevelUp;
		CsGameEventUIToUI.Instance.EventRankPassiveSkillLevelUp += OnOnEventRankPassiveSkillLevelUp;

		CsGameEventUIToUI.Instance.EventOpen7DayEventProductBuy += OnEventOpen7DayEventProductBuy;

		CsGameEventUIToUI.Instance.EventRetrieveGold += OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll += OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia += OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll += OnEventRetrieveDiaAll;

		// 할일위탁
		CsGameEventUIToUI.Instance.EventTaskConsignmentImmediatelyComplete += OnEventTaskConsignmentImmediatelyComplete;

		// 주말보상
		CsGameEventUIToUI.Instance.EventWeekendRewardReceive += OnEventWeekendRewardReceive;

		// 창고슬롯확장
		CsGameEventUIToUI.Instance.EventWarehouseSlotExtend += OnEventWarehouseSlotExtend;

		// 다이아상점
		CsGameEventUIToUI.Instance.EventDiaShopProductBuy += OnEventDiaShopProductBuy;

		//던전보상
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted += OnEventMainQuestDungeonStepCompleted;

		//골드던전
		CsDungeonManager.Instance.EventGoldDungeonStepCompleted += OnEventGoldDungeonStepCompleted;
        CsDungeonManager.Instance.EventGoldDungeonClear += OnEventGoldDungeonClear;
        CsDungeonManager.Instance.EventGoldDungeonSweep += OnEventGoldDungeonSweep;

		CsDungeonManager.Instance.EventRuinsReclaimRewardObjectInteractionFinished += OnEventRuinsReclaimRewardObjectInteractionFinished;

        // 길드기부
        CsGuildManager.Instance.EventGuildDonate += OnEventGuildDonate;
        CsGuildManager.Instance.EventGuildAltarDonate += OnEventGuildAltarDonate;
		CsGuildManager.Instance.EventGuildBlessingBuffStart += OnEventGuildBlessingBuffStart;

		// 세리우 보급지원
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;

        // 크리처카드상점유료갱신
        CsCreatureCardManager.Instance.EventCreatureCardShopPaidRefresh += OnEventCreatureCardShopPaidRefresh;
        CsCreatureCardManager.Instance.EventCreatureCardShopFixedProductBuy += OnEventCreatureCardShopFixedProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardShopRandomProductBuy += OnEventCreatureCardShopRandomProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardDisassemble += OnEventCreatureCardDisassemble;
        CsCreatureCardManager.Instance.EventCreatureCardDisassembleAll += OnEventCreatureCardDisassembleAll;
        CsCreatureCardManager.Instance.EventCreatureCardCompose += OnEventCreatureCardCompose;

        // 용맹의 증명 유료 갱신
        CsDungeonManager.Instance.EventProofOfValorRefresh += OnEventProofOfValorRefresh;

		// 일일퀘스트
		CsDailyQuestManager.Instance.EventDailyQuestRefresh += OnEventDailyQuestRefresh;
		CsDailyQuestManager.Instance.EventDailyQuestMissionImmediatlyComplete += OnEventDailyQuestMissionImmediatlyComplete;

		// 위클리 퀘스트
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundRefresh += OnEventWeeklyQuestRoundRefresh;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted += OnEventWeeklyQuestRoundCompleted;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete += OnEventWeeklyQuestRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestTenRoundImmediatlyComplete += OnEventWeeklyQuestTenRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;

		// 서브퀘스트
		CsSubQuestManager.Instance.EventSubQuestComplete += OnEventSubQuestComplete;

		// 행운상점
		CsLuckyShopManager.Instance.EventItemLuckyShopFreePick += OnEventItemLuckyShopFreePick;
		CsLuckyShopManager.Instance.EventItemLuckyShop1TimePick += OnEventItemLuckyShop1TimePick;
		CsLuckyShopManager.Instance.EventItemLuckyShop5TimePick += OnEventItemLuckyShop5TimePick;
		CsLuckyShopManager.Instance.EventCreatureCardLuckyShopFreePick += OnEventCreatureCardLuckyShopFreePick;
		CsLuckyShopManager.Instance.EventCreatureCardLuckyShop1TimePick += OnEventCreatureCardLuckyShop1TimePick;
		CsLuckyShopManager.Instance.EventCreatureCardLuckyShop5TimePick += OnEventCreatureCardLuckyShop5TimePick;

		// 축복
		CsBlessingQuestManager.Instance.EventBlessingQuestBlessingSend += OnEventBlessingQuestBlessingSend;
		CsBlessingQuestManager.Instance.EventBlessingRewardReceive += OnEventBlessingRewardReceive;

		// 선물
		CsPresentManager.Instance.EventPresentSend += OnEventPresentSend;

		// 캐쉬
		CsCashManager.Instance.EventCashProductPurchaseComplete += OnEventCashProductPurchaseComplete;

		// 크리쳐
		CsCreatureManager.Instance.EventCreatureInject += OnEventCreatureInject;

		// 별자리
		CsConstellationManager.Instance.EventConstellationStepOpen += OnEventConstellationStepOpen;
		CsConstellationManager.Instance.EventConstellationEntryActivate += OnEventConstellationEntryActivate;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
    {
        Button buttonHelp = m_textPopupName.transform.Find("ButtonHelp").GetComponent<Button>();
        buttonHelp.onClick.RemoveAllListeners();
        buttonHelp.onClick.AddListener(OnClickHelp);
        buttonHelp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textGold = m_trBackground.Find("TextGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGold);
        m_textGold.text = "0";

        m_textOwnDia = m_trBackground.Find("TextOwnDia").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textOwnDia);
        m_textOwnDia.text = "0";

        m_textUnOwnDia = m_trBackground.Find("TextUnOwnDia").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textUnOwnDia);
        m_textUnOwnDia.text = "0";

        m_textSoulPowder = m_trBackground.Find("TextSoulPowder").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textSoulPowder);
        m_textSoulPowder.text = "0";

        Button buttonUnOwnDia = m_textUnOwnDia.transform.Find("ButtonShop").GetComponent<Button>();
        buttonUnOwnDia.onClick.RemoveAllListeners();
        buttonUnOwnDia.onClick.AddListener(OnClickUnownDia);
        buttonUnOwnDia.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonClosePopup = m_trBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClosePopup.onClick.RemoveAllListeners();
        buttonClosePopup.onClick.AddListener(OnClickClosePopup);
        buttonClosePopup.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        UpdateResources();
        MenuGoodsCheck();
        CheckWarehouse();
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickClosePopup();
        }

        // 1초마다 실행.
        if (m_flTime + 1f < Time.time)
        {
            switch ((EnMainMenu)m_csMainMenu.MenuId)
            {
                case EnMainMenu.Character:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.Costume:
                                SetNoticeSubMenu(i, CsCostumeManager.Instance.CheckCostumeEnchant());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Mail:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.Mail:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNoticeMail());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Skill:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.Skill:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNoticeSkill());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Class:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.Class:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckRankReward());
                                break;
							case EnSubMenu.ClassSkill:
								SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckRankSkillLevelUp());
								break;
                        }
                    }
                    break;

                case EnMainMenu.MainGear:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.MainGearEnchant:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNoticeMainGearEnchant());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.SubGear:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.SubGearLevelUp:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearLevelUp());
                                break;

                            case EnSubMenu.SubGearSoulstone:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearSoulstone());
                                break;
                            case EnSubMenu.SubGearRune:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearRune());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Mount:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.MountLevelUp:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckMountLevelUp());
                                break;

                            case EnSubMenu.MountAwakening:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckMountAwakeningLevelUp());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Wing:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.WingEnchant:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckWingEnchant());
                                break;

                            case EnSubMenu.WingEquipment:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckWingInstall());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Ranking:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.RankingIndividual:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckRanking());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Guild:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.GuildMember:
                                SetNoticeSubMenu(i, CsGuildManager.Instance.CheckGuild());
                                break;

                            case EnSubMenu.GuildEvent:
                                SetNoticeSubMenu(i, CsGuildManager.Instance.CheckWeeklyObjectiveSettingEnabled());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Nation:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.NationInfo:
                                SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckNation());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Achievement:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.Accomplishment:
                                SetNoticeSubMenu(i, CsAccomplishmentManager.Instance.CheckAccomplishmentNotice());
                                break;
                        }
                    }
                    break;

                case EnMainMenu.Collection:
                    for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
                    {
                        switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
                        {
                            case EnSubMenu.CardCollection:
                                SetNoticeSubMenu(i, CsCreatureCardManager.Instance.CheckCreatureCardCollictionNotice());
                                break;

							case EnSubMenu.Biography:
								SetNoticeSubMenu(i, CsBiographyManager.Instance.CheckBiographyNotices());
								break;
                        }
                    }
                    break;

				case EnMainMenu.Vip:
					for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
					{
						switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
						{
							case EnSubMenu.VipInfo:
								SetNoticeSubMenu(i, CsGameData.Instance.MyHeroInfo.CheckVipRewardsReceivable());
								break;
						}
					}
					break;

				case EnMainMenu.Friend:
					for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
					{
						switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
						{
							case EnSubMenu.FriendBlessing:
								SetNoticeSubMenu(i, CsBlessingQuestManager.Instance.CheckProspectQuest());
								break;
						}
					}
					break;
            }


            m_flTime = Time.time;
        }
    }
    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        Debug.Log("CsPopupMain::::::OnFinalize");
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventSubGearLevelUp -= OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally -= OnventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventMountedSoulstoneCompose -= OnEventMountedSoulstoneCompose;

        CsGameEventUIToUI.Instance.EventSkillLevelUp -= OnEventSkillLevelUp;
        CsGameEventUIToUI.Instance.EventSkillLevelUpTotally -= OnEventSkillLevelUpTotally;

        CsGameEventUIToUI.Instance.EventSimpleShopBuy -= OnEventSimpleShopBuy;
        CsGameEventUIToUI.Instance.EventSimpleShopSell -= OnEventSimpleShopSell;

        CsGameEventUIToUI.Instance.EventInventorySlotExtend -= OnEventInventorySlotExtend;

        CsGameEventUIToUI.Instance.EventItemCompose -= OnEventItemCompose;
        CsGameEventUIToUI.Instance.EventItemComposeTotally -= OnEventItemComposeTotally;

        CsGameEventUIToUI.Instance.EventRestRewardReceiveGold -= OnEventRestRewardReceiveGold;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveDia -= OnEventRestRewardReceiveDia;

        CsGameEventUIToUI.Instance.EventImmediateRevive -= OnEventImmediateRevive;

        CsGameEventUIToUI.Instance.EventReturnScrollUseStart -= OnEventReturnScrollUseStart;

        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyDisband -= OnEventPartyDisband;
        CsGameEventUIToUI.Instance.EventPartyDisbanded -= OnEventPartyDisbanded;
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted -= OnEventPartyApplicationAccepted;
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept -= OnEventPartyInvitationAccept;
        CsGameEventUIToUI.Instance.EventPartyBanished -= OnEventPartyBanished;
        CsGameEventUIToUI.Instance.EventPartyExit -= OnEventPartyExit;

        CsMainQuestManager.Instance.EventCompleted -= OnEventMainQuestCompleted;

        CsGameEventUIToUI.Instance.EventGearShare -= OnEventGearShare;

        CsGameEventUIToUI.Instance.EventMountGearPickBoxMake -= OnEventMountGearPickBoxMake;
        CsGameEventUIToUI.Instance.EventMountGearPickBoxMakeTotally -= OnEventMountGearPickBoxMakeTotally;

        CsGameEventUIToUI.Instance.EventStartQuestAutoPlay -= OnEventStartQuestAutoPlay;

        CsGameEventUIToUI.Instance.EventMiniMapSelected -= OnEventMiniMapSelected;

        CsGameEventUIToUI.Instance.EventRankRewardReceive -= OnEventRankRewardReceive;

        CsGameEventUIToUI.Instance.EventOpenOneToOneChat -= OnEventOpenOneToOneChat;

        CsGameEventUIToUI.Instance.EventNationDonate -= OnEventNationDonate;

        CsGameEventUIToUI.Instance.EventStaminaBuy -= OnEventStaminaBuy;

        CsGameEventUIToUI.Instance.EventGoldItemUse -= OnEventGoldItemUse;
        CsGameEventUIToUI.Instance.EventOwnDiaItemUse -= OnEventOwnDiaItemUse;

		CsGameEventUIToUI.Instance.EventRankActiveSkillLevelUp -= OnEventRankActiveSkillLevelUp;
		CsGameEventUIToUI.Instance.EventRankPassiveSkillLevelUp -= OnOnEventRankPassiveSkillLevelUp;

		CsGameEventUIToUI.Instance.EventOpen7DayEventProductBuy -= OnEventOpen7DayEventProductBuy;

		CsGameEventUIToUI.Instance.EventRetrieveGold -= OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll -= OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia -= OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll -= OnEventRetrieveDiaAll;

		// 할일위탁
		CsGameEventUIToUI.Instance.EventTaskConsignmentImmediatelyComplete -= OnEventTaskConsignmentImmediatelyComplete;

		// 주말보상
		CsGameEventUIToUI.Instance.EventWeekendRewardReceive -= OnEventWeekendRewardReceive;

		// 창고슬롯확장
		CsGameEventUIToUI.Instance.EventWarehouseSlotExtend -= OnEventWarehouseSlotExtend;

		// 다이아상점
		CsGameEventUIToUI.Instance.EventDiaShopProductBuy -= OnEventDiaShopProductBuy;

		//던전보상
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted -= OnEventMainQuestDungeonStepCompleted;

        //골드던전
        CsDungeonManager.Instance.EventGoldDungeonStepCompleted -= OnEventGoldDungeonStepCompleted;
        CsDungeonManager.Instance.EventGoldDungeonClear -= OnEventGoldDungeonClear;
        CsDungeonManager.Instance.EventGoldDungeonSweep -= OnEventGoldDungeonSweep;

		CsDungeonManager.Instance.EventRuinsReclaimRewardObjectInteractionFinished -= OnEventRuinsReclaimRewardObjectInteractionFinished;

        // 길드기부
        CsGuildManager.Instance.EventGuildDonate -= OnEventGuildDonate;
        CsGuildManager.Instance.EventGuildAltarDonate -= OnEventGuildAltarDonate;
		CsGuildManager.Instance.EventGuildBlessingBuffStart -= OnEventGuildBlessingBuffStart;

		// 세리우 보급지원
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;

        // 크리처카드상점유료갱신
        CsCreatureCardManager.Instance.EventCreatureCardShopPaidRefresh -= OnEventCreatureCardShopPaidRefresh;
        CsCreatureCardManager.Instance.EventCreatureCardShopFixedProductBuy -= OnEventCreatureCardShopFixedProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardShopRandomProductBuy -= OnEventCreatureCardShopRandomProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardDisassemble -= OnEventCreatureCardDisassemble;
        CsCreatureCardManager.Instance.EventCreatureCardDisassembleAll -= OnEventCreatureCardDisassembleAll;
        CsCreatureCardManager.Instance.EventCreatureCardCompose -= OnEventCreatureCardCompose;

        //용맹의 증명 유료 갱신
        CsDungeonManager.Instance.EventProofOfValorRefresh -= OnEventProofOfValorRefresh;

		// 일일퀘스트갱신
		CsDailyQuestManager.Instance.EventDailyQuestRefresh -= OnEventDailyQuestRefresh;
		CsDailyQuestManager.Instance.EventDailyQuestMissionImmediatlyComplete -= OnEventDailyQuestMissionImmediatlyComplete;

		// 위클리 퀘스트
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundRefresh -= OnEventWeeklyQuestRoundRefresh;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted -= OnEventWeeklyQuestRoundCompleted;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete -= OnEventWeeklyQuestRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestTenRoundImmediatlyComplete -= OnEventWeeklyQuestTenRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete -= OnEventWeeklyQuestRoundMoveMissionComplete;

		// 서브퀘스트
		CsSubQuestManager.Instance.EventSubQuestComplete -= OnEventSubQuestComplete;

		// 행운상점
		CsLuckyShopManager.Instance.EventItemLuckyShopFreePick -= OnEventItemLuckyShopFreePick;
		CsLuckyShopManager.Instance.EventItemLuckyShop1TimePick -= OnEventItemLuckyShop1TimePick;
		CsLuckyShopManager.Instance.EventItemLuckyShop5TimePick -= OnEventItemLuckyShop5TimePick;
		CsLuckyShopManager.Instance.EventCreatureCardLuckyShopFreePick -= OnEventCreatureCardLuckyShopFreePick;
		CsLuckyShopManager.Instance.EventCreatureCardLuckyShop1TimePick -= OnEventCreatureCardLuckyShop1TimePick;
		CsLuckyShopManager.Instance.EventCreatureCardLuckyShop5TimePick -= OnEventCreatureCardLuckyShop5TimePick;

		// 축복
		CsBlessingQuestManager.Instance.EventBlessingQuestBlessingSend -= OnEventBlessingQuestBlessingSend;
		CsBlessingQuestManager.Instance.EventBlessingRewardReceive -= OnEventBlessingRewardReceive;

		// 선물
		CsPresentManager.Instance.EventPresentSend -= OnEventPresentSend;

		// 캐쉬
		CsCashManager.Instance.EventCashProductPurchaseComplete -= OnEventCashProductPurchaseComplete;

		// 크리쳐
		CsCreatureManager.Instance.EventCreatureInject -= OnEventCreatureInject;

		// 별자리
		CsConstellationManager.Instance.EventConstellationStepOpen -= OnEventConstellationStepOpen;
		CsConstellationManager.Instance.EventConstellationEntryActivate -= OnEventConstellationEntryActivate;
	}

    #region EventHandler

	//---------------------------------------------------------------------------------------------------
	void OnEventCloseAllPopup()
	{
		OnClickClosePopup();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopPaidRefresh()
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopFixedProductBuy()
    {
        UpdateSoulPowder();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopRandomProductBuy()
    {
        UpdateSoulPowder();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardDisassemble(int nAcquiredSoulPowder)
    {
        UpdateSoulPowder();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardDisassembleAll(int nAcquiredSoulPowder)
    {
        UpdateSoulPowder();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardCompose()
    {
        UpdateSoulPowder();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorRefresh()
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventDailyQuestRefresh()
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDailyQuestMissionImmediatlyComplete()
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundRefresh()
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundCompleted(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestTenRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundMoveMissionComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp, long lGold, int nAcquiredExploitPoint)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDonate()
    {
        UpdateGold();
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDonate()
    {
        UpdateGold();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildBlessingBuffStart()
	{
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGoldDungeonStepCompleted(long lGold)
    {
        UpdateGold();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), lGold));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonClear(long lGold)
    {
        UpdateGold();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), lGold));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonSweep(long lGold)
    {
        UpdateGold();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), lGold));
    }

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRewardObjectInteractionFinished(ClientCommon.PDItemBooty booty, long lInstanceId)
	{
		UpdateGold();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonStepCompleted(bool bLevelUp, long lRewardGold, long lExp)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartQuestAutoPlay(EnQuestCategoryType enQuestCartegoryType, int nIndex)
    {
        OnClickClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGearShare(CsHeroObject csHeroObject)
    {
        OnClickClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseStart()
    {
        OnClickClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInventorySlotExtend()
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopBuy()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopSell()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUp(int nSubGearId)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnventSubGearLevelUpTotally(int nSubGearId)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountedSoulstoneCompose(int nSubGearId, int nSocketIndex)
    {
        UpdateGold();
    }


    //---------------------------------------------------------------------------------------------------
    void OnEventSkillLevelUp(int nSkillId)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSkillLevelUpTotally(int nSkillId)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemCompose()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemComposeTotally()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveGold(bool bLevelUp, long lAcquiredExp)
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveDia(bool bLevelUp, long lAcquiredExp)
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventImmediateRevive()
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMiniMapSelected(int nLocationId)
    {
        SetSubMenuTap(EnSubMenu.MinimapArea);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRankRewardReceive()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenOneToOneChat(Guid guid)
    {
        if (m_enPreMainMenu != 0 || m_enPreSubMenu != 0)
        {
            m_enPreMainMenu = 0;
            m_enPreSubMenu = 0;
        }

        OnClickClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationDonate(int nAcquiredExploitPoint)
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStaminaBuy()
    {
        UpdateOwnDia();
        UpdateUnOwnDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldItemUse()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOwnDiaItemUse()
    {
        UpdateOwnDia();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventRankActiveSkillLevelUp()
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnOnEventRankPassiveSkillLevelUp()
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpen7DayEventProductBuy()
	{
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGold(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGoldAll(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDia(bool bLevelUp, long lAcquiredExp)
	{
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDiaAll(bool bLevelUp, long lAcquiredExp)
	{
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTaskConsignmentImmediatelyComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeekendRewardReceive()
	{
		UpdateOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarehouseSlotExtend()
	{
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDiaShopProductBuy()
	{
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickHelp()
    {
        //도움말창 오픈
		CsGameEventUIToUI.Instance.OnEventOpenPopupHelp((EnMainMenu)m_csMainMenu.MenuId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickUnownDia()
    {
        //상점이동
		CsGameEventUIToUI.Instance.OnEventOpenPopupCharging();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopup()
    {
        //이전 메뉴로 돌아올 경우
        if (m_enPreMainMenu != 0 && m_enPreSubMenu != 0)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(m_enPreMainMenu, m_enPreSubMenu);

            if (m_enPreMainMenu == EnMainMenu.Dungeon && m_enPreSubMenu == EnSubMenu.IndividualDungeon)
            {
                StartCoroutine(FieldOfHonorShortCut());
            }

            m_enPreMainMenu = 0;
            m_enPreSubMenu = 0;
            return;
        }

        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            CsSubMenu csSubMenu = m_csMainMenu.SubMenuList[i];

            for (int j = 0; j < csSubMenu.Layout; j++)
            {
                Transform trSubMenu = m_trMainPopupSubMenu.Find("SubMenu" + (j + 1));
                string strPrefabName = csSubMenu.Prefabs[j];

                if (strPrefabName.LastIndexOf('/') > -1)
                {
                    strPrefabName = strPrefabName.Substring(strPrefabName.LastIndexOf("/") + 1);
                }

                Transform trPrefab = trSubMenu.Find(strPrefabName);

                if (trPrefab != null)
                    Destroy(trPrefab.gameObject);
            }
        }

        CsGameEventUIToUI.Instance.OnEventPopupClose();

        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSubmenuTab(Toggle toggleSubmenuTab, int nToggleIndex)
    {
        DisplayTab(toggleSubmenuTab, nToggleIndex);

        if (toggleSubmenuTab.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티 생성
    void OnEventPartyCreate()
    {
        UpdatePartySubMenuTap();
        SetSubMenuTap(EnSubMenu.MyParty);
    }

    //---------------------------------------------------------------------------------------------------
    //파티 해산
    void OnEventPartyDisband()
    {
        SetSubMenuTap(EnSubMenu.SurroundingHero);
        UpdatePartySubMenuTap();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisbanded()
    {
        SetSubMenuTap(EnSubMenu.SurroundingHero);
        UpdatePartySubMenuTap();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationAccepted()
    {
        UpdatePartySubMenuTap();
        SetSubMenuTap(EnSubMenu.MyParty);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationAccept()
    {
        UpdatePartySubMenuTap();
        SetSubMenuTap(EnSubMenu.MyParty);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyBanished()
    {
        SetSubMenuTap(EnSubMenu.SurroundingHero);
        UpdatePartySubMenuTap();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyExit()
    {
        SetSubMenuTap(EnSubMenu.SurroundingHero);
        UpdatePartySubMenuTap();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearPickBoxMake()
    {
        UpdateGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearPickBoxMakeTotally()
    {
        UpdateGold();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestComplete(bool bLevelUp, long lAcquireExp, int nSubQuestId)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventItemLuckyShopFreePick(ClientCommon.PDItemLuckyShopPickResult PDItemLuckyShopPickResult)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventItemLuckyShop1TimePick(ClientCommon.PDItemLuckyShopPickResult PDItemLuckyShopPickResult)
	{
		UpdateGold();
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventItemLuckyShop5TimePick(ClientCommon.PDItemLuckyShopPickResult[] arrPDItemLuckyShopPickResult)
	{
		UpdateGold();
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardLuckyShopFreePick(CsHeroCreatureCard csHeroCreatureCard)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardLuckyShop1TimePick(CsHeroCreatureCard csHeroCreatureCard)
	{
		UpdateGold();
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardLuckyShop5TimePick(List<CsHeroCreatureCard> listCsHeroCreatureCard)
	{
		UpdateGold();
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingQuestBlessingSend(long lQuestId)
	{
		UpdateGold();
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingRewardReceive(long lInstanceId)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPresentSend(int nPresentId)
	{
		UpdateOwnDia();
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCashProductPurchaseComplete()
	{
		UpdateUnOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureInject(bool bCritical, bool bLevelUp)
	{
		UpdateGold();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConstellationStepOpen(int nStepNo)
	{
		UpdateUnOwnDia();
		UpdateOwnDia();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConstellationEntryActivate(bool bSuccess)
	{
		UpdateGold();
	}

	#endregion EventHandler

	#region Implement Interface

	//---------------------------------------------------------------------------------------------------
	public CsSubMenu GetCurrentSubMenu()
    {
        return m_csSubMenu;
    }

    #endregion Implement Interface

    //---------------------------------------------------------------------------------------------------
    public void DisplayMenu(CsMainMenu csMainMenu, EnSubMenu enSubMenuID)
    {
        m_csMainMenu = csMainMenu;
        m_enSubMenu = enSubMenuID;
        m_textPopupName.text = m_csMainMenu.Name;
        
        if ((EnMainMenu)m_csMainMenu.MenuId == EnMainMenu.LuckyShop)
        {
            DisplayToggleList(false);
        }
        else
        {
            DisplayToggleList(true);
        }

        DisplaySubmenuTab();
    }

    //---------------------------------------------------------------------------------------------------
    //다른 유저 정보 메뉴
    public void DisplayMenu(CsMainMenu csMainMenu, EnSubMenu enSubMenuID, CsHeroInfo csHeroInfo)
    {
        if (m_csMainMenu != null && m_enPreMainMenu == 0 && m_enPreSubMenu == 0)
        {
            m_enPreMainMenu = (EnMainMenu)m_csMainMenu.MenuId;
            m_enPreSubMenu = m_csSubMenu.EnSubMenu;
        }

        m_csMainMenu = csMainMenu;
        m_enSubMenu = enSubMenuID;
        m_textPopupName.text = m_csMainMenu.Name;

        DisplaySubmenuTab();
        StartCoroutine(LoadViewOtherUsers(csHeroInfo));
    }

    //---------------------------------------------------------------------------------------------------
    //크리쳐카드에서만 골드 대신 영혼의 가루를 표시해준다.
    void MenuGoodsCheck()
    {
        switch ((EnMainMenu)m_csMainMenu.MenuId)
        {
            case EnMainMenu.Collection:
                m_textGold.gameObject.SetActive(false);
                m_textSoulPowder.gameObject.SetActive(true);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateResources()
    {
        UpdateGold();
        UpdateOwnDia();
        UpdateUnOwnDia();
        UpdateSoulPowder();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGold()
    {
        m_textGold.text = CsGameData.Instance.MyHeroInfo.Gold.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateOwnDia()
    {
        m_textOwnDia.text = CsGameData.Instance.MyHeroInfo.OwnDia.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateUnOwnDia()
    {
        m_textUnOwnDia.text = CsGameData.Instance.MyHeroInfo.UnOwnDia.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSoulPowder()
    {
        m_textSoulPowder.text = CsGameData.Instance.MyHeroInfo.SoulPowder.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySubmenuTab()
    {
        bool bSelected = false;
        m_trToggleList.GetComponent<ToggleGroup>().allowSwitchOff = false;

        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            int nToggleIndex = i;

            Toggle toggle = m_trToggleList.Find("Toggle" + nToggleIndex).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.gameObject.SetActive(true);

            //탭이름
            Text textSubmenuTab = toggle.transform.Find("Label").GetComponent<Text>();
            CsUIData.Instance.SetFont(textSubmenuTab);
            textSubmenuTab.text = m_csMainMenu.SubMenuList[i].Name;

            //디폴트설정
            if (m_enSubMenu == EnSubMenu.Default)
            {
                if (m_csMainMenu.SubMenuList[i].IsDefault)
                {
                    bSelected = toggle.isOn = true;
                    DisplayTab(toggle, nToggleIndex);
                }
            }
            else
            {
                if (m_csMainMenu.SubMenuList[i].SubMenuId == (int)m_enSubMenu)
                {
                    bSelected = toggle.isOn = true;
                    DisplayTab(toggle, nToggleIndex);
                }
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedSubmenuTab(toggle, nToggleIndex));

            //알림
            Transform trNotice = toggle.transform.Find("ImageNotice");
            trNotice.gameObject.SetActive(false);

            if (m_csMainMenu.SubMenuList[i].ContentId > 0)
            {
                if (CsUIData.Instance.MenuContentOpen(m_csMainMenu.SubMenuList[i].ContentId))
                {
                    toggle.gameObject.SetActive(true);
                }
                else
                {
                    toggle.gameObject.SetActive(false);
                }
            }

            switch ((EnMainMenu)m_csMainMenu.MenuId)
            {
                case EnMainMenu.Party:

                    switch ((EnSubMenu)m_csMainMenu.SubMenuList[i].SubMenuId)
                    {
                        case EnSubMenu.MyParty:

                            if (CsGameData.Instance.MyHeroInfo.Party == null)
                            {
                                toggle.gameObject.SetActive(false);
                            }
                            else
                            {
                                toggle.gameObject.SetActive(true);
                            }
                            break;

                        case EnSubMenu.PartyMatching:
                            if (CsUIData.Instance.DungeonInNow == EnDungeon.AncientRelic
                                || CsUIData.Instance.DungeonInNow == EnDungeon.SoulCoveter)
                            {
                                toggle.gameObject.SetActive(true);
                            }
                            else
                            {
                                toggle.gameObject.SetActive(false);
                            }
                            break;
                    }
                    break;
            }
        }

        // 디플트값이 없을 경우, 첫번째 탭을 선택한다.
        if (!bSelected)
        {
            Toggle toggle = m_trToggleList.Find("Toggle0").GetComponent<Toggle>();
            toggle.isOn = true;
            bSelected = true;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void LoadSubMenu(string strPath, Transform trSubMenu)
    {
        if (!string.IsNullOrEmpty(strPath))
        {
            string strSubName = strPath;

            // Rsources 에서 찾을 경우.
            if (strPath.LastIndexOf("/") > -1)
            {
                strSubName = strPath.Substring(strPath.LastIndexOf("/") + 1);
            }

            Transform trSubMenuPrefab = trSubMenu.Find(strSubName);

            if (trSubMenuPrefab == null)
            {
				//m_bLoadSubMenu = false;
                //ToggleInteractable(m_bLoadSubMenu);
                StartCoroutine(LoadSubMenuCoroutine(strPath, trSubMenu, strSubName));
            }
            else
            {
				trSubMenuPrefab.gameObject.SetActive(true);
                ToggleInteractable(true);
                CheckWarehouse();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSubMenuCoroutine(string strPath, Transform trSubMenu, string strSubMenuName)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>(strPath);
        yield return resourceRequest;
        GameObject goSubMenu = Instantiate((GameObject)resourceRequest.asset, trSubMenu);
        CsPopupSub csPopupSub = goSubMenu.GetComponent<CsPopupSub>();
        csPopupSub.PopupMain = this;
        goSubMenu.name = strSubMenuName;
        ToggleInteractable(true);
        CheckWarehouse();
        //m_bLoadSubMenu = true;
        //ToggleInteractable(m_bLoadSubMenu);
    }

    //---------------------------------------------------------------------------------------------------
    void SetNoticeSubMenu(int nIndex, bool bOnOff)
    {
        Transform trNotice = m_trToggleList.Find("Toggle" + nIndex + "/ImageNotice");
        trNotice.gameObject.SetActive(bOnOff);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePartySubMenuTap()
    {
        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            if (m_csMainMenu.MenuId == (int)EnMainMenu.Party)
            {
                Toggle toggle = m_trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

                switch ((EnSubMenu)m_csMainMenu.SubMenuList[i].SubMenuId)
                {
                    case EnSubMenu.MyParty:

                        if (CsGameData.Instance.MyHeroInfo.Party == null)
                        {
                            toggle.gameObject.SetActive(false);
                        }
                        else
                        {
                            toggle.gameObject.SetActive(true);
                        }
                        break;

                    case EnSubMenu.PartyMatching:
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.AncientRelic || CsUIData.Instance.DungeonInNow == EnDungeon.SoulCoveter)
                        {
                            toggle.gameObject.SetActive(true);
                        }
                        else
                        {
                            toggle.gameObject.SetActive(false);
                        }
                        break;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetSubMenuTap(EnSubMenu enSubMenu)
    {
        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            if (m_csMainMenu.SubMenuList[i].EnSubMenu == enSubMenu)
            {
                int nToggleIndex = i;
                Toggle toggle = m_trToggleList.Find("Toggle" + i).GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.isOn = true;
                DisplayTab(toggle, nToggleIndex);
                toggle.onValueChanged.AddListener((ison) => OnValueChangedSubmenuTab(toggle, nToggleIndex));
                break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SubMenuClear()
    {
        m_trToggleList.GetComponent<ToggleGroup>().allowSwitchOff = true;

        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            Toggle toggle = m_trToggleList.Find("Toggle" + i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.isOn = false;
            toggle.gameObject.SetActive(false);

            CsSubMenu csSubMenu = m_csMainMenu.SubMenuList[i];

            for (int j = 0; j < csSubMenu.Layout; j++)
            {
                Transform trSubMenu = m_trMainPopupSubMenu.Find("SubMenu" + (j + 1));
                string strPrefabName = csSubMenu.Prefabs[j];

                if (strPrefabName.LastIndexOf('/') > -1)
                {
                    strPrefabName = strPrefabName.Substring(strPrefabName.LastIndexOf("/") + 1);
                }

                Transform trPrefab = trSubMenu.Find(strPrefabName);

                if (trPrefab != null)
                    DestroyImmediate(trPrefab.gameObject);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTab(Toggle toggleSubmenuTab, int nToggleIndex)
    {
        m_csSubMenu = m_csMainMenu.SubMenuList[nToggleIndex];

        Transform[] atrSubMenus = new Transform[3];

        for (int i = 0; i < atrSubMenus.Length; i++)
        {
            atrSubMenus[i] = m_trMainPopupSubMenu.Find("SubMenu" + (i + 1));
        }

        Text textSubmenu = toggleSubmenuTab.transform.Find("Label").GetComponent<Text>();

        if (toggleSubmenuTab.isOn)
        {
            textSubmenu.color = CsUIData.Instance.ColorButtonOn;

            ToggleInteractable(false);

            for (int i = 0; i < m_csSubMenu.Layout; i++)
            {
                LoadSubMenu(m_csSubMenu.Prefabs[i], atrSubMenus[i]);
            }

            CheckWarehouse();
        }
        else
        {
            textSubmenu.color = CsUIData.Instance.ColorButtonOff;

            //기존에 켜진걸 끈다.
            for (int i = 0; i < m_trMainPopupSubMenu.childCount; i++)
            {
                Transform trSubMenu = m_trMainPopupSubMenu.GetChild(i);

                for (int j = 0; j < trSubMenu.childCount; j++)
                {
                    trSubMenu.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadViewOtherUsers(CsHeroInfo csHeroInfo)
    {
        yield return new WaitUntil(() => m_trMainPopupSubMenu.Find("SubMenu1/ViewOtherUsers") != null);

        Transform trViewOtherUsers = m_trMainPopupSubMenu.Find("SubMenu1/ViewOtherUsers");

        if (trViewOtherUsers != null)
        {
            trViewOtherUsers.GetComponent<CsPopupViewOtherUsers>().DisplaySetting(csHeroInfo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FieldOfHonorShortCut()
    {
        yield return new WaitUntil(() => m_trMainPopupSubMenu.Find("SubMenu1/PopupDungeonCategory") != null);

        Transform trCategory = m_trMainPopupSubMenu.Find("SubMenu1/PopupDungeonCategory");

        if (trCategory != null)
        {
            CsDungeonCartegory csDungeonCartegory = trCategory.GetComponent<CsDungeonCartegory>();
            csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.FieldOfHonor);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ToggleInteractable(bool bInteractable)
    {
        for (int i = 0; i < m_trToggleList.childCount; i++)
        {
            m_trToggleList.GetChild(i).GetComponent<Toggle>().interactable = bInteractable;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 창고 개방 여부 확인
    void CheckWarehouse()
    {
        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel > 0 || m_csMainMenu.MenuId != (int)EnMenuId.Inventory)
        {
            if (m_imageWarehouseLock != null) m_imageWarehouseLock.enabled = false;

            CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)EnMainMenu.Character);

            if (csMainMenu != null)
            {
                for (int i = 0; i < csMainMenu.SubMenuList.Count; i++)
                {
                    if (csMainMenu.SubMenuList[i].EnSubMenu == EnSubMenu.Warehouse)
                    {
                        m_trToggleList.Find("Toggle" + csMainMenu.SubMenuList[i].SortNo).GetComponent<Toggle>().interactable = true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        else
        {
            if (m_imageWarehouseLock != null) m_imageWarehouseLock.enabled = true;

            for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
            {
                if (m_csMainMenu.SubMenuList[i].EnSubMenu == EnSubMenu.Warehouse)
                {
                    m_trToggleList.Find("Toggle" + (m_csMainMenu.SubMenuList[i].SortNo - 1)).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayToggleList(bool bIson)
    {
        m_trToggleList.gameObject.SetActive(bIson);
    }
}
