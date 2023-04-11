using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum EnHelper
{
	SkillLevelUp					= 1, // 스킬 레벨업
    MainGearEnchant					= 2, // 장비 강화
	MainGearEnchantLevelSet			= 3, // 강화 세트
    SubGearLevelUp					= 4, // 보조장비 제련
    SubGearSoulStone				= 5, // 보조장비 세공
	SubGearSoulStoneSet				= 6, // 보조장비 보석 세트	
    SubGearRune						= 7, // 보조장비 각인
    MountLevelUp					= 8, // 탈 것 육성
    MountGearPickBox				= 9, // 탈 것 장비 제작
    WingEnchant						= 10,// 날개 강화
	WingEquipment					= 11,// 날개 장착
    RankReward						= 12,// 계급 보상
    RankLevelUp						= 13,// 계급 승급
    MailReceive						= 14,// 우편 수령
    ServerLevelRankingReward		= 15,// 레벨 랭킹 보상
    GuildDonate						= 16,// 길드 기부
    GuildApply						= 17,// 길드 가입 신청
    GuildDailyReward				= 18,// 길드 일일 보상
    GuildAltarReward				= 19,// 길드 제단 보상
    GuildHunting					= 20,// 길드 현상금 보상
    GuildFoodWarehouse				= 21,// 길드 군량 보상
    NationDonate					= 22,// 국가 기부
    AccomplishmentReward			= 23,// 업적 보상
    CreatureCardCollectionActive	= 24,// 카드 활성화
    TodayMissionReward				= 25,// 오늘의 미션 보상
    SeriesMissionReward				= 26,// 연속 미션 보상
    AccessReward					= 27,// 접속 보상
    AttendReward					= 28,// 출석 보상
    LevelUpReward					= 29,// 레벨업 보상
	LimitReward						= 30,// 한정 보상
	WeekendReward					= 31,// 주말 보상
    AchievementReward				= 32,// 활약도 보상
	Retrieval						= 33,// 회수
	OrdealQuest						= 34,// 시련퀘스트
	Biography						= 35,// 전기
	FriendBlessing					= 36,// 친구축복
    LuckyShop                       = 37,// 행운 상점
    PotionAttr                      = 38,// 속성 물약
    MountAwakening                  = 39,// 탈것 각성
    MountPotionAttr                 = 40,// 탈것 속성 물약
	CreatureTraining				= 41,// 크리쳐육성
	CreatureInjection				= 42,// 크리쳐주입
    CostumeEnchant                  = 43,// 코스튬강화
}

public class CsPopupHelper : MonoBehaviour 
{
    GameObject m_goButtonShortcut;

    Transform m_trCanvas2;
    Transform m_trImageBackground;
    Transform m_trContent;

    Button m_buttonClose;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopupDestroy();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        PopupDestroy();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonClose()
    {
        PopupDestroy();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHelperShortcut(EnHelper enHelper)
    {
        switch (enHelper)
        {
			case EnHelper.SkillLevelUp:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Skill, EnSubMenu.Skill);
                break;

            case EnHelper.MainGearEnchant:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.MainGear, EnSubMenu.MainGearEnchant);
                break;

			case EnHelper.MainGearEnchantLevelSet:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.CharacterInfo);
				break;

            case EnHelper.SubGearLevelUp:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearLevelUp);
                break;

            case EnHelper.SubGearSoulStone:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearSoulstone);
                break;

			case EnHelper.SubGearSoulStoneSet:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.CharacterInfo);
				break;

            case EnHelper.SubGearRune:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearRune);
                break;

            case EnHelper.MountLevelUp:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountLevelUp);
                break;

            case EnHelper.MountGearPickBox:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountGearPickBoxMake);
                break;

            case EnHelper.WingEnchant:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Wing, EnSubMenu.WingEnchant);
                break;

			case EnHelper.WingEquipment:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Wing, EnSubMenu.WingEquipment);
				break;

            case EnHelper.RankReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Class, EnSubMenu.Class);
                break;

            case EnHelper.RankLevelUp:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Class, EnSubMenu.Class);
                break;

            case EnHelper.MailReceive:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mail, EnSubMenu.Mail);
                break;

            case EnHelper.ServerLevelRankingReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Ranking, EnSubMenu.RankingIndividual);
                break;

            case EnHelper.GuildDonate:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnHelper.GuildApply:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnHelper.GuildDailyReward:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnHelper.GuildAltarReward:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnHelper.GuildHunting:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnHelper.GuildFoodWarehouse:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnHelper.NationDonate:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Nation, EnSubMenu.NationInfo);
                break;

            case EnHelper.AccomplishmentReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Achievement, EnSubMenu.Accomplishment);
                break;

            case EnHelper.CreatureCardCollectionActive:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Collection, EnSubMenu.CardCollection);
                break;

            case EnHelper.TodayMissionReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.TodayMission);
                break;

            case EnHelper.SeriesMissionReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.SeriesMission);
                break;

            case EnHelper.AccessReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.AccessReward);
                break;

            case EnHelper.AttendReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.AttendReward);
                break;

            case EnHelper.LevelUpReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.LevelUpReward);
                break;

			case EnHelper.LimitReward:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.LimitationGift);
				break;

			case EnHelper.WeekendReward:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.WeekendReward);
				break;

            case EnHelper.AchievementReward:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.TodayTask, EnSubMenu.TodayTaskExp);
                break;

			case EnHelper.Retrieval:
				CsGameEventUIToUI.Instance.OnEventSinglePopupOpen(EnMenuId.Retrieval);
				break;

			case EnHelper.OrdealQuest:
				CsGameEventUIToUI.Instance.OnEventOpenPopupOrdealQuest();
				break;

			case EnHelper.Biography:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Collection, EnSubMenu.Biography);
				break;

			case EnHelper.FriendBlessing:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Friend, EnSubMenu.FriendBlessing);
				break;

            case EnHelper.LuckyShop:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.LuckyShop, EnSubMenu.LuckyShop);
                break;

            case EnHelper.PotionAttr:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.CharacterInfo);
                CsGameEventUIToUI.Instance.OnEventOpenPopupHeroPotionAttr();
                break;

            case EnHelper.MountAwakening:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountAwakening);
                break;

			case EnHelper.CreatureTraining:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Creature, EnSubMenu.CreatureTraining);
				break;

			case EnHelper.CreatureInjection:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Creature, EnSubMenu.CreatureInjection);
				break;
        }

        if (enHelper == EnHelper.ServerLevelRankingReward)
        {
            StartCoroutine(LoadServerLevelRankingCategory());
        }
        else if (enHelper == EnHelper.MountPotionAttr)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountLevelUp);
            StartCoroutine(LoadPopupMountPotionAttr());
        }
        else if (enHelper == EnHelper.CostumeEnchant)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.Costume);
            StartCoroutine(LoadPopupCostumeEncahnt());
        }
        else
        {
            PopupDestroy();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trImageBackground = transform.Find("ImageBackground");
        m_trContent = m_trImageBackground.Find("Scroll View/Viewport/Content");

        m_buttonClose = m_trImageBackground.Find("ButtonClose").GetComponent<Button>();
        m_buttonClose.onClick.RemoveAllListeners();
        m_buttonClose.onClick.AddListener(OnClickButtonClose);
        m_buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        InitializeButtonShortcut();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeButtonShortcut()
    {
        if (m_goButtonShortcut == null)
        {
            StartCoroutine(LoadButtonShortcut());
        }
        else
        {
            CheckButtonShortcut();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadButtonShortcut()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/ButtonShortcut");
        yield return resourceRequest;
        m_goButtonShortcut = (GameObject)resourceRequest.asset;

        CheckButtonShortcut();
    }

    //---------------------------------------------------------------------------------------------------
    void CheckButtonShortcut()
    {
        foreach (EnHelper enumItem in System.Enum.GetValues(typeof(EnHelper)))
        {
            switch (enumItem)
            {
				case EnHelper.SkillLevelUp:
                    if (CsGameData.Instance.MyHeroInfo.CheckNoticeSkill())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.MainGearEnchant:
                    if (CsGameData.Instance.MyHeroInfo.CheckNoticeMainGearEnchant())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

				case EnHelper.MainGearEnchantLevelSet:
					if (CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

                case EnHelper.SubGearLevelUp:
                    if (CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearLevelUp())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.SubGearSoulStone:
                    if (CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearSoulstone())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

				case EnHelper.SubGearSoulStoneSet:
					if (CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

                case EnHelper.SubGearRune:
                    if (CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearRune())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.MountLevelUp:
                    if (CsGameData.Instance.MyHeroInfo.CheckMountLevelUp())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.MountGearPickBox:
                    for (int i = 0; i < CsGameData.Instance.MountGearPickBoxRecipeList.Count; i++)
                    {
                        CsMountGearPickBoxRecipe csMountGearPickBoxRecipe = CsGameData.Instance.MountGearPickBoxRecipeList[i];

                        if (CsGameData.Instance.MyHeroInfo.Level >= csMountGearPickBoxRecipe.RequiredHeroLevel
                        && CsGameData.Instance.MyHeroInfo.Gold >= csMountGearPickBoxRecipe.Gold
                        && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem1.ItemId) >= csMountGearPickBoxRecipe.MaterialItem1Count
                        && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem2.ItemId) >= csMountGearPickBoxRecipe.MaterialItem2Count
                        && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem3.ItemId) >= csMountGearPickBoxRecipe.MaterialItem3Count
                        && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem4.ItemId) >= csMountGearPickBoxRecipe.MaterialItem4Count)
                        {
                            CreateButtonShortcut(enumItem);
                            break;
                        }
                    }
                    break;

                case EnHelper.WingEnchant:
					if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.WingEnchant))
					{
						if (CsGameData.Instance.MyHeroInfo.CheckWingEnchant())
						{
							CreateButtonShortcut(enumItem);
						}
					}
                    
                    break;

				case EnHelper.WingEquipment:
					if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.WingEquip))
					{
						if (CsGameData.Instance.MyHeroInfo.CheckWingInstall())
						{
							CreateButtonShortcut(enumItem);
						}
					}

					break;

                case EnHelper.RankReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.Rank))
                    {
                        // 보상조건
                        if ((CsGameData.Instance.MyHeroInfo.RankRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0) && (CsGameData.Instance.MyHeroInfo.RankRewardReceivedRankNo == CsGameData.Instance.MyHeroInfo.RankNo))
                        {
                            Debug.Log("EnHelper.RankReward");
                        }
                        else
                        {
                            CreateButtonShortcut(enumItem);
                        }
                    }
                    break;

                case EnHelper.RankLevelUp:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.Rank))
                    {
                        // 승급조건
                        if (CsGameData.Instance.MyHeroInfo.RankNo < CsGameData.Instance.RankList[CsGameData.Instance.RankList.Count - 1].RankNo)
                        {
                            CsRank csRankNext = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo + 1);

                            if (csRankNext != null)
                            {
                                if (CsGameData.Instance.MyHeroInfo.ExploitPoint >= csRankNext.RequiredExploitPoint)
                                {
                                    CreateButtonShortcut(enumItem);
                                }
                            }
                        }
                    }
                    break;

                case EnHelper.MailReceive:
                    if (CsGameData.Instance.MyHeroInfo.CheckNoticeMail())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.ServerLevelRankingReward:
                    if (CsGameData.Instance.MyHeroInfo.CheckRanking())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.GuildDonate:
                    if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.DailyGuildDonationCount < CsGameData.Instance.MyHeroInfo.VipLevel.GuildDonationMaxCount)
                    {
                        for (int i = 0; i < CsGameData.Instance.GuildDonationEntryList.Count; ++i)
                        {
                            CsGuildDonationEntry csGuildDonationEntry = CsGameData.Instance.GetGuildDonationEntry(i + 1);

                            if (csGuildDonationEntry.MoneyType == 1)
                            {
                                if (CsGameData.Instance.MyHeroInfo.Gold >= csGuildDonationEntry.MoneyAmount)
                                {
                                    CreateButtonShortcut(enumItem);
                                    break;
                                }
                            }
                            else
                            {
                                if (CsGameData.Instance.MyHeroInfo.Dia >= csGuildDonationEntry.MoneyAmount)
                                {
                                    CreateButtonShortcut(enumItem);
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case EnHelper.GuildApply:
                    if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.Guild.ApplicationCount > 0 && CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).ApplicationAcceptanceEnabled)
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.GuildDailyReward:
                    if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.GuildDailyRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.GuildAltarReward:
                    if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.Guild.MoralPoint >= CsGameData.Instance.GuildAltar.DailyGuildMaxMoralPoint
                        && CsGuildManager.Instance.GuildAltarRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.GuildHunting:
                    if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.Guild.DailyHuntingDonationCount >= CsGameConfig.Instance.GuildHuntingDonationMaxCount
                        && CsGuildManager.Instance.GuildHuntingDonationRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.GuildFoodWarehouse:
                    if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.Guild.FoodWarehouseCollectionId != System.Guid.Empty
                        && CsGuildManager.Instance.Guild.FoodWarehouseCollectionId != CsGuildManager.Instance.ReceivedGuildFoodWarehouseCollectionId)
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.NationDonate:
                    if (CsGameData.Instance.MyHeroInfo.CheckNation())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.AccomplishmentReward:
                    if (CsAccomplishmentManager.Instance.CheckAccomplishmentNotice())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.CreatureCardCollectionActive:
                    if (CsCreatureCardManager.Instance.CheckCreatureCardCollictionNotice())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.TodayMissionReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.TodayMission))
                    {
                        // 오늘의 미션 보상 중 1개 이상의 보상을 획득할 수 있거나
                        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroTodayMissionList.Count; i++)
                        {
                            if (!CsGameData.Instance.MyHeroInfo.HeroTodayMissionList[i].RewardReceived && CsGameData.Instance.MyHeroInfo.HeroTodayMissionList[i].TodayMission.TargetCount <= CsGameData.Instance.MyHeroInfo.HeroTodayMissionList[i].ProgressCount)
                            {
                                CreateButtonShortcut(enumItem);
                                break;
                            }
                        }
                    }
                    break;

                case EnHelper.SeriesMissionReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SeriesMisson))
                    {
                        // 연속 미션 보상 중 1개 이상의 보상을 획득할 수 있거나
                        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList.Count; i++)
                        {
                            if (CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList[i].SeriesMissionStep.TargetCount <= CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList[i].ProgressCount)
                            {
                                CreateButtonShortcut(enumItem);
                                break;
                            }
                        }
                    }
                    break;

                case EnHelper.AccessReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AccessReward))
                    {
                        // 접속보상
                        List<CsAccessRewardEntry> listAccessRewardEntry = CsGameData.Instance.AccessRewardEntryList.FindAll(a => a.AccessTime <= CsGameData.Instance.MyHeroInfo.DailyAccessTime);

                        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList.Count; i++)
                        {
                            listAccessRewardEntry.RemoveAll(a => a.EntryId == CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList[i]);
                        }

                        if (listAccessRewardEntry.Count > 0)
                        {
                            CreateButtonShortcut(enumItem);
                        }
                    }
                    break;

                case EnHelper.AttendReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AttendReward))
                    {
                        // 출석 보상 중 1개 이상의 보상을 획득할 수 있거나
                        if (CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
                        {
                            CreateButtonShortcut(enumItem);
                        }
                    }
                    break;

                case EnHelper.LevelUpReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.LevelUpReward))
                    {
                        // 레벨업 보상 중 1개 이상의 보상을 획득할 수 있거나
                        if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.LevelUpRewardEntryList[0].Level)
                        {
                            List<CsLevelUpRewardEntry> list = CsGameData.Instance.LevelUpRewardEntryList.FindAll(a => a.Level <= CsGameData.Instance.MyHeroInfo.Level);

                            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList.Count; i++)
                            {
                                list.RemoveAll(a => a.EntryId == CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList[i]);
                            }

                            if (list.Count > 0)
                            {
                                CreateButtonShortcut(enumItem);
                            }
                        }
                    }
                    break;

				case EnHelper.LimitReward:
					if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.LimitationGift))
					{
						int nMyHeroDayOfWeek = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek;
						int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

						if (0 <= CsGameData.Instance.LimitationGift.LimitationGiftRewardDayOfWeekList.FindIndex(a => a.DayOfWeek == nMyHeroDayOfWeek))
						{
							for (int i = 0; i < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Count; i++)
							{
								CsLimitationGiftRewardSchedule csLimitationGiftRewardSchedule = CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i];

								if (CsGameData.Instance.MyHeroInfo.RewardedLimitationGiftScheduleIdList.FindIndex(a => a == csLimitationGiftRewardSchedule.ScheduleId) < 0 &&
									csLimitationGiftRewardSchedule.StartTime <= nSecond && nSecond < csLimitationGiftRewardSchedule.EndTime)
								{
									CreateButtonShortcut(enumItem);
									break;
								}
							}
						}
					}
					break;

				case EnHelper.WeekendReward:
					if (CsGameData.Instance.WeekendReward.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
					{
						if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Monday)
						{
							if ((CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 != -1 || CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 != -1 ||
								CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 != -1) && !CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Rewarded)
							{
								CreateButtonShortcut(enumItem);
							}
						}
						else if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Friday && CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 == -1)
						{
							CreateButtonShortcut(enumItem);
						}
						else if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Saturday && CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 == -1)
						{
							CreateButtonShortcut(enumItem);
						}
						else if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Sunday && CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 == -1)
						{
							CreateButtonShortcut(enumItem);
						}
					}

					break;

                case EnHelper.AchievementReward:
                    if (CsGameData.Instance.MyHeroInfo.CheckTodayTask())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

				case EnHelper.Retrieval:
					if (CsGameData.Instance.MyHeroInfo.CheckRetrieval())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

				case EnHelper.OrdealQuest:

					if (CsOrdealQuestManager.Instance.CsHeroOrdealQuest != null &&
						!CsOrdealQuestManager.Instance.CsHeroOrdealQuest.Completed &&
						CsGameData.Instance.MyHeroInfo.CheckOrdealQuest())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

                case EnHelper.PotionAttr:

                    if (CsGameData.Instance.MyHeroInfo.CheckPotionAttr())
                    {
                        CreateButtonShortcut(enumItem);
                    }

                    break;

				case EnHelper.Biography:
					if (CsBiographyManager.Instance.CheckBiographyNotices())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

				case EnHelper.FriendBlessing:
					if (CsBlessingQuestManager.Instance.CheckProspectQuest())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

                case EnHelper.LuckyShop:
                    if (CsLuckyShopManager.Instance.CheckNoticeLuckyShop())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.MountAwakening:
                    if (CsGameData.Instance.MyHeroInfo.CheckMountAwakeningLevelUp())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

                case EnHelper.MountPotionAttr:
                    if (CsGameData.Instance.MyHeroInfo.CheckMountPotionAttr())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;

				case EnHelper.CreatureTraining:
					if (CsCreatureManager.Instance.CheckCreatureRear() ||
						CsCreatureManager.Instance.CheckCreatureVaritaion() ||
						CsCreatureManager.Instance.CheckCreatureSwitch())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

				case EnHelper.CreatureInjection:
					if (CsCreatureManager.Instance.CheckCreatureInjection())
					{
						CreateButtonShortcut(enumItem);
					}
					break;

                case EnHelper.CostumeEnchant:
                    if (CsCostumeManager.Instance.CheckCostumeEnchant())
                    {
                        CreateButtonShortcut(enumItem);
                    }
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateButtonShortcut(EnHelper enHelper)
    {
        Transform trButtonShortcut = Instantiate(m_goButtonShortcut, m_trContent).transform;
        trButtonShortcut.name = "ButtonShortcut" + (int)enHelper;
        
        Image imageIcon = trButtonShortcut.Find("ImageIcon").GetComponent<Image>();
        
        Text textShortcut = trButtonShortcut.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textShortcut);

        Button buttonShortcut = trButtonShortcut.GetComponent<Button>();
        buttonShortcut.onClick.RemoveAllListeners();
        buttonShortcut.onClick.AddListener(() => OnClickHelperShortcut(enHelper));
        buttonShortcut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        switch (enHelper)
        {
			case EnHelper.SkillLevelUp:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_03");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00001");
                break;
            case EnHelper.MainGearEnchant:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_11");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00002");
                break;
			case EnHelper.MainGearEnchantLevelSet:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_28");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00028");
				break;
            case EnHelper.SubGearLevelUp:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_12");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00003");
                break;
            case EnHelper.SubGearSoulStone:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_12");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00004");
                break;
			case EnHelper.SubGearSoulStoneSet:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_29");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00030");
                break;
            case EnHelper.SubGearRune:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_12");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00005");
                break;
            case EnHelper.MountLevelUp:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_15");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00006");
                break;
            case EnHelper.MountGearPickBox:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_15");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00007");
                break;
            case EnHelper.WingEnchant:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_13");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00008");
				break;
			case EnHelper.WingEquipment:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_13");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00033");
                break;
            case EnHelper.RankReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_05");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00009");
                break;
            case EnHelper.RankLevelUp:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_05");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00010");
                break;
            case EnHelper.MailReceive:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_22");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00011");
                break;
            case EnHelper.ServerLevelRankingReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_09");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00012");
                break;
            case EnHelper.GuildDonate:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_07");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00013");
                break;
            case EnHelper.GuildApply:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_07");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00014");
                break;
            case EnHelper.GuildDailyReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_07");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00015");
                break;
            case EnHelper.GuildAltarReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_07");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00016");
                break;
            case EnHelper.GuildHunting:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_07");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00017");
                break;
            case EnHelper.GuildFoodWarehouse:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_07");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00018");
                break;
            case EnHelper.NationDonate:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_06");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00019");
                break;
            case EnHelper.AccomplishmentReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_04");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00020");
                break;
            case EnHelper.CreatureCardCollectionActive:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_14");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00021");
                break;
            case EnHelper.TodayMissionReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00022");
                break;
            case EnHelper.SeriesMissionReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00023");
                break;
            case EnHelper.AccessReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00024");
                break;
            case EnHelper.AttendReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00025");
                break;
            case EnHelper.LevelUpReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00026");
                break;
			case EnHelper.LimitReward:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00031");
				break;
			case EnHelper.WeekendReward:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_26");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00032");
				break;
            case EnHelper.AchievementReward:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_24");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00027");
                break;
			case EnHelper.Retrieval:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_23");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00029");
				break;
			case EnHelper.OrdealQuest:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_30");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00034");
				break;
			case EnHelper.Biography:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_33");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00035");
				break;
			case EnHelper.FriendBlessing:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_34");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00036");
				break;
            case EnHelper.LuckyShop:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_31");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00037");
                break;
            case EnHelper.PotionAttr:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_01");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00040");
                break;
            case EnHelper.MountAwakening:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_15");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00041");
                break;
            case EnHelper.MountPotionAttr:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_15");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00042");
                break;
			case EnHelper.CreatureTraining: // %% Helper UI 아이콘 변경
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_15");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00038");
				break;
			case EnHelper.CreatureInjection:
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_15");
				textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00039");
				break;
            case EnHelper.CostumeEnchant:
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_menu_all_01");
                textShortcut.text = CsConfiguration.Instance.GetString("A92_TXT_00043");
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadServerLevelRankingCategory()
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupRanking") != null);
        Transform trPopupRanking = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupRanking");

        CsPopupRanking csPopupRanking = trPopupRanking.GetComponent<CsPopupRanking>();
        csPopupRanking.ChangeToggleLevelRanking();

        PopupDestroy();
        StopCoroutine(LoadServerLevelRankingCategory());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupMountPotionAttr()
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/MountInfo") != null);
        CsGameEventUIToUI.Instance.OnEventOpenPopupMountAttrPotion(CsGameData.Instance.MyHeroInfo.EquippedMountId);

        PopupDestroy();
        StopCoroutine(LoadPopupMountPotionAttr());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCostumeEncahnt()
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupCostume") != null);
        CsGameEventUIToUI.Instance.OnEventOpenPopupCostumeEnchant();

        PopupDestroy();
        StopCoroutine(LoadPopupCostumeEncahnt());
    }

    //---------------------------------------------------------------------------------------------------
    void PopupDestroy()
    {
        Destroy(gameObject);
    }
}