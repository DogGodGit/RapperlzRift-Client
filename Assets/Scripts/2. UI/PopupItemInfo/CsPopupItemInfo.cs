using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-11-30)
//---------------------------------------------------------------------------------------------------

public enum EnPopupItemInfoPositionType
{
    Left = 0,
    Right = 1,
    Center = 2
}

public enum EnMainGearShortCutType
{
    Enchant = 0,
    Inherit = 1,
    Share = 2
}

public enum EnMountGearShortCutType
{
    Enchant = 0,
    Share = 1,
}

public enum EnSubGearShortCutType
{
    Refine = 0,
    Sokcet = 1,
    Engrave = 2,
    Share = 3,
}

public enum EnItemShortCutType
{
    Refine = 0,
}

public enum EnWingShortCutType
{
    Enchant = 0,
    Appearance = 1,
}

public enum EnItemLocationType  // 팝업창을 띄우는 곳 지정 '그외/인벤토리/창고'
{
    None = 0,
    Inventory = 1,
    Warehouse = 2,
}

public class CsPopupItemInfo : MonoBehaviour
{
    // 아이템정보창 종료
    public event Delegate<EnPopupItemInfoPositionType> EventClosePopupItemInfo;  // 아이템 정보창 종료

    GameObject m_goSetItemName;
    GameObject m_goShortCutButton;
    GameObject m_goBaseOption;
    GameObject m_goOption;
    GameObject m_goSoulStone;
    GameObject m_goSoulStoneInfo;
    GameObject m_goRune;
    GameObject m_goRuneInfo;
    GameObject m_goPopupSelectUseCount;

    Transform m_trCanvas2;
    Transform m_trImageBack;
    Transform m_trItemSlot;
    Transform m_trContentMainGear;
    Transform m_trContentSubGear;
    Transform m_trContentItem;
    Transform m_trContentCard;
    Transform m_trContentCostume;
    Transform m_trContentCostumeEffect;
    Transform m_trBaseInfoMainGear;
    Transform m_trBaseInfoSubGear;
    Transform m_trBaseInfoItem;
    Transform m_trBaseInfoCard;
    Transform m_trBaseInfoCostume;
    Transform m_trBaseInfoCostumeEffect;
    Transform m_trMainGearShortCutList;
    Transform m_trMountGearShortCoutList;
    Transform m_trSubGearShortCutList;
    Transform m_trItemShortCutList;
    Transform m_trPopupList;
    Transform m_trPopupSelectUseCount;

    //날개
    Transform m_trBaseInfoWing;
    Transform m_trWingShortCutList;
    Transform m_trContentWing;

    //아이템 사용
    Transform m_trButtonBack;

    RectTransform m_rtScrollView;

    Text m_textItemName;
    Text m_textUse;
    //Text m_textItemShortCutName;
    Text m_textMultyUse;
    Text m_textUseCountValue;
    Text m_textWithdraw;
    Text m_textDeposit;
    Text m_textCostumeTimer;

    Button m_buttonUse;
    Button m_buttonMultyUse;
    Button m_buttonSelectUseCount;
    Button m_buttonDeposit;
    Button m_buttonWithdraw;

    Slider m_slider;

    Image m_imageCoolTime;

    EnInventoryObjectType m_enInventoryObjectTypeNow;
    EnPopupItemInfoPositionType m_enPopupItemInfoPositionType;
    EnItemLocationType m_enItemLocationType = EnItemLocationType.None;

    CsHeroMainGear m_csHeroMainGearSelect;
    CsHeroSubGear m_csHeroSubGearSelect;
    CsHeroMountGear m_csHeroMountGear;
    CsItem m_csItemSelect;
    CsHeroCostume m_csHeroCostume;

    bool m_bMainGearIsEquiped;
    bool m_bMountGearIsEquiped;
    bool m_bButtonOn = true;

    int m_nInventoryListIndex;
    int m_nMaxUseCount;
    int m_nSelectUseCount;
    int m_nSelectObjectIndex;

    //Color32 m_colorOn = new Color32(255, 255, 255, 255);
    //Color32 m_colorOff = new Color32(133, 141, 148, 255);

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitialIzeUI();

        CsGameEventUIToUI.Instance.EventMainGearEquip += OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip += OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip += OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip += OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventMountGearEquip += OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip += OnEventMountGearUnequip;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_csItemSelect != null)
        {
            if (m_csItemSelect.ItemType.EnItemType == EnItemType.HpPotion)
            {
                if (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup > 0)
                {
                    m_imageCoolTime.fillAmount = (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup) / CsUIData.Instance.HpPotionCoolTime;
                }
            }
            else if (m_csItemSelect.ItemType.EnItemType == EnItemType.ReturnScroll)
            {
                if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup > 0)
                {
                    m_imageCoolTime.fillAmount = (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup) / CsUIData.Instance.ReturnScrollCoolTime;
                }
            }
        }

        if (m_csHeroCostume != null && m_textCostumeTimer != null)
        {
            TimeSpan tsRemainingTimer = TimeSpan.FromSeconds(m_csHeroCostume.RemainingTime - Time.realtimeSinceStartup);

            if (86400 <= tsRemainingTimer.TotalSeconds)
            {
                // 일, 시
                m_textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00007"), tsRemainingTimer.Days, tsRemainingTimer.Hours);
            }
            else if (3600 <= tsRemainingTimer.TotalSeconds)
            {
                // 시, 분
                m_textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00008"), tsRemainingTimer.Hours, tsRemainingTimer.Minutes);
            }
            else
            {
                // 분, 초
                m_textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00009"), tsRemainingTimer.Minutes, tsRemainingTimer.Seconds);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventMainGearEquip -= OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip -= OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip -= OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip -= OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventMountGearEquip -= OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip -= OnEventMountGearUnequip;

        OnClickClosePopupSelectUseCount();
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearEquip(Guid guid)
    {
        ClosePopupItemInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearUnequip(Guid guid)
    {
        ClosePopupItemInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEquip(Guid guidHeroGearId)
    {
        ClosePopupItemInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearUnequip(Guid guidHeroGearId)
    {
        ClosePopupItemInfo();

        if (CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroGearId).MainGear.MainGearType.EnMainGearType == EnMainGearType.Armor)
        {
            Debug.Log("## ReleaseArmor ##");
            CsUIData.Instance.PlayUISound(EnUISoundType.ReleaseArmor);
        }
        else
        {
            Debug.Log("## ReleaseWeapon ##");
            CsUIData.Instance.PlayUISound(EnUISoundType.ReleaseWeapon);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearEquip(int nSubGearId)
    {
        ClosePopupItemInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearUnequip(int nSubGearId)
    {
        ClosePopupItemInfo();

        if (nSubGearId == (int)EnSubGearType.Necklace || nSubGearId == (int)EnSubGearType.Ring)
        {
            Debug.Log("## ReleaseSubGear ##");
            CsUIData.Instance.PlayUISound(EnUISoundType.ReleaseSubGear);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemUse()
    {
        switch (m_enInventoryObjectTypeNow)
        {
            case EnInventoryObjectType.MainGear:

                if (m_bMainGearIsEquiped)
                {
                    //해제
                    CsCommandEventManager.Instance.SendMainGearUnequip(m_csHeroMainGearSelect.Id);
                }
                else
                {
                    //장착
                    int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

                    if (m_csHeroMainGearSelect.MainGear.Job == null || m_csHeroMainGearSelect.MainGear.Job.JobId == nJobId)
                    {
                        if (m_csHeroMainGearSelect.MainGear.MainGearTier.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                        {
                            CsCommandEventManager.Instance.SendMainGearEquip(m_csHeroMainGearSelect.Id);
                        }
                        else
                        {
                            Debug.Log("레벨이 부족합니다.");
                        }
                    }
                    else
                    {
                        Debug.Log("직업이 다릅니다.");
                    }
                }

                break;

            case EnInventoryObjectType.MountGear:
                if (m_bMountGearIsEquiped)
                {
                    //해제
                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        CsCommandEventManager.Instance.SendMountGearUnequip(m_csHeroMountGear.Id);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_ERROR_01701"));
                    }
                }
                else
                {
                    //장착
                    if (m_csHeroMountGear.MountGear.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        CsCommandEventManager.Instance.SendMountGearEquip(m_csHeroMountGear.Id);
                    }
                    else
                    {
                        Debug.Log("레벨이 부족합니다");
                    }
                }

                break;

            case EnInventoryObjectType.SubGear:
                if (m_csHeroSubGearSelect.Equipped)
                {
                    //해제
                    CsCommandEventManager.Instance.SendSubGearUnequip(m_csHeroSubGearSelect.SubGear.SubGearId);
                }
                else
                {
                    //장착
                    CsCommandEventManager.Instance.SendSubGearEquip(m_csHeroSubGearSelect.SubGear.SubGearId);
                }

                break;

            case EnInventoryObjectType.Item:
                if (m_csItemSelect.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= m_csItemSelect.RequiredMaxHeroLevel)
                {
					CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nInventoryListIndex];

                    switch (m_csItemSelect.ItemType.EnItemType)
                    {
                        case EnItemType.HpPotion:
                            if (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup <= 0 && CsGameData.Instance.MyHeroInfo.Hp < CsGameData.Instance.MyHeroInfo.MaxHp)
                            {
                                if (csInventorySlot != null)
                                {
                                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                    ClosePopupItemInfo();
                                }
                            }

                            break;

                        case EnItemType.ReturnScroll:
                            if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup <= 0 && CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup <= 0)
                            {
                                if (csInventorySlot != null)
                                {
                                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                    ClosePopupItemInfo();
                                }
                            }

                            break;

                        case EnItemType.MainGearBox:
                        case EnItemType.PickBox:
						
                            if (CsGameData.Instance.MyHeroInfo.InventorySlotCount > CsGameData.Instance.MyHeroInfo.InventorySlotList.Count)
                            {
                                CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, 1);
                                ClosePopupItemInfo();
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                            }

                            break;

                        case EnItemType.ExpPotion:
                            if (CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount < CsGameData.Instance.MyHeroInfo.VipLevel.ExpPotionUseMaxCount)
                            {
                                CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, 1);
                                ClosePopupItemInfo();
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_ERROR_01602"));
                            }

                            break;

                        case EnItemType.ExpScroll:
                            if (CsGameData.Instance.MyHeroInfo.ExpScrollDailyUseCount < CsGameData.Instance.MyHeroInfo.VipLevel.ExpScrollUseMaxCount)
                            {
                                CsItem csItem = CsGameData.Instance.GetItem(CsGameData.Instance.MyHeroInfo.ExpScrollItemId);

                                if (csItem == null)
                                {
                                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                }
                                else
                                {
                                    if (csItem.ItemId == csInventorySlot.InventoryObjectItem.Item.ItemId)
                                    {
                                        CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                    }
                                    else
                                    {
                                        if (CsGameData.Instance.MyHeroInfo.ExpScrollRemainingTime - Time.realtimeSinceStartup > 0)
                                        {
                                            string strMessage = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01007"), csItem.Name, csInventorySlot.InventoryObjectItem.Item.Name);
                                            CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index),
                                                CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                                        }
                                        else
                                        {
                                            CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                        }
                                    }
                                }

                                ClosePopupItemInfo();
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_ERROR_01602"));
                            }
                            break;

                        case EnItemType.FishingBait:
                            if (CsFishingQuestManager.Instance.BaitItemId == 0)
                            {
                                if (CsFishingQuestManager.Instance.FishingQuestDailyStartCount < CsGameData.Instance.FishingQuest.LimitCount)
                                {
                                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                }
                                else
                                {
                                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A46_TXT_02002"));
                                }
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                                CsGameEventUIToUI.Instance.OnEventAutoMoveFishingZone();
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A46_TXT_02001"));
                            }
                            break;

                        case EnItemType.BountyHunter:
                            if (CsBountyHunterQuestManager.Instance.BountyHunterQuest == null)
                            {
                                if (CsGameConfig.Instance.BountyHunterQuestMaxCount > CsBountyHunterQuestManager.Instance.BountyHunterQuestDailyStartCount)
                                {
                                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                }
                                else
                                {
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A45_TXT_02002"));
                                }
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A45_TXT_02001"));
                            }
                            break;

                        case EnItemType.DistortionScroll:
                            if (CsGameData.Instance.MyHeroInfo.DistortionScrollDailyUseCount < CsGameData.Instance.MyHeroInfo.VipLevel.DistortionScrollUseMaxCount)
                            {
                                //전투상태 및 이동 검사
                                if (CsUIData.Instance.DungeonInNow == EnDungeon.FieldOfHonor)
                                {
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A31_TXT_02005"));
                                }
                                else
                                {
                                    CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.NationWarDeclarationList.Find(a => a.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId || a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId);

                                    if (csNationWarDeclaration != null && csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
                                    {
                                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A58_TXT_02020"));
                                    }
                                    else
                                    {
                                        CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                        ClosePopupItemInfo();
                                    }
                                }
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A55_TXT_02002"));
                            }
                            break;

                        case EnItemType.GuildCall:
                            if (CsGuildManager.Instance.Guild == null)
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A58_TXT_02019"));
                            }
                            else if (CsGuildManager.Instance.MyGuildMemberGrade.GuildCallEnabled)
                            {
                                CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
                                CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.NationWarDeclarationList.Find(a => a.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId || a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId);

                                if (csContinent == null)
                                {
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A70_TXT_02007"));
                                }
                                else if (csNationWarDeclaration != null && csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
                                {
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A58_TXT_02020"));
                                }
                                else
                                {
                                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                                }
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A58_TXT_02021"));
                            }

                            ClosePopupItemInfo();
                            break;

                        case EnItemType.NationCall:
                            CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(CsGameData.Instance.MyHeroInfo.HeroId);

                            if (csNationNoblesseInstance != null && csNationNoblesseInstance.NationNoblesse.NationCallEnabled)
                            {
                                CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);

                                if (csContinent == null)
                                {
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A70_TXT_02007"));
                                }
                                else if (CsNationWarManager.Instance.MyNationWarDeclaration != null && CsNationWarManager.Instance.MyNationWarDeclaration.Status != EnNationWarDeclaration.Current)
                                {
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A70_TXT_02008"));
                                }
                                else
                                {
                                    CsCommandEventManager.Instance.SendNationCall(csInventorySlot.Index);
                                }
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A70_TXT_02009"));
                            }

                            ClosePopupItemInfo();
                            break;

                        case EnItemType.Title:
                            if (CsTitleManager.Instance.GetHeroTitle(csInventorySlot.InventoryObjectItem.Item.Value1) == null)
                            {
                                CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A83_ERROR_00102"));
                            }

                            ClosePopupItemInfo();
                            break;

                        case EnItemType.MountPotionAttr:
                            if (0 < CsGameData.Instance.MyHeroInfo.HeroMountList.Count)
                            {
                                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountLevelUp);
                                CsGameEventUIToUI.Instance.OnEventOpenPopupMountAttrPotion(CsGameData.Instance.MyHeroInfo.EquippedMountId);

                                ClosePopupItemInfo();
                            }
                            else
                            {
                                // 탈 것이 없습니다.
                                ClosePopupItemInfo();
                            }

                            break;

                        case EnItemType.MountAwakening:
                            if (0 < CsGameData.Instance.MyHeroInfo.HeroMountList.Count)
                            {
                                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountAwakening);
                                ClosePopupItemInfo();
                            }
                            else
                            {
                                ClosePopupItemInfo();
                            }

                            break;

                        case EnItemType.CostumeEffect:

                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.Costume);
                            CsGameEventUIToUI.Instance.OnEventOpenPopupCostumeEffect();

                            break;

                        case EnItemType.PotionAttr:

                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.CharacterInfo);
                            CsGameEventUIToUI.Instance.OnEventOpenPopupHeroPotionAttr();

                            break;

                        default:
                            CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                            ClosePopupItemInfo();
                            break;
                    }
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A55_TXT_02001"));
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemMultyUse()
    {
        switch (m_enInventoryObjectTypeNow)
        {
            case EnInventoryObjectType.MainGear:
            case EnInventoryObjectType.MountGear:
            case EnInventoryObjectType.SubGear:
                break;

            case EnInventoryObjectType.Item:
                CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nInventoryListIndex];

                if (csInventorySlot.InventoryObjectItem.Count == 1)
                {
                    OnClickItemUse();
                }
                else
                {
                    switch (m_csItemSelect.ItemType.EnItemType)
                    {
                        case EnItemType.MainGearBox:
                        case EnItemType.PickBox:

                            if (CsGameData.Instance.MyHeroInfo.InventorySlotCount > CsGameData.Instance.MyHeroInfo.InventorySlotList.Count)
                            {
                                m_nMaxUseCount = csInventorySlot.InventoryObjectItem.Count;
                                OpenPopupSelectUseCount();
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                            }

                            break;

                        case EnItemType.ExpPotion:

                            if (CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount < CsGameData.Instance.MyHeroInfo.VipLevel.ExpPotionUseMaxCount)
                            {
                                m_nMaxUseCount = csInventorySlot.InventoryObjectItem.Count;
                                OpenPopupSelectUseCount();
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_ERROR_01602"));
                            }

                            break;

                        case EnItemType.Gold:
                        case EnItemType.OwnDia:
                        case EnItemType.HonorPoint:
                        case EnItemType.ExploitPoint:
                            m_nMaxUseCount = csInventorySlot.InventoryObjectItem.Count;
                            OpenPopupSelectUseCount();
                            break;
                    }
                }
                break;
        }
    }

    //--------------------------------------------------------------------------------------------------- 
    void OnClickDepositItem()
    {
        if((CsGameConfig.Instance.FreeWarehouseSlotCount + CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount) > CsGameData.Instance.MyHeroInfo.WarehouseSlotList.Count)
        {
            int[] anDepositIndex = new int[1];
            anDepositIndex[0] = m_nSelectObjectIndex;
            CsCommandEventManager.Instance.SendWarehouseDeposit(anDepositIndex);
        }
        else
        {
            // 창고에 빈자리가 더이상 없을 경우
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A113_TXT_02002"));
        }        
        ClosePopupItemInfo();
    }

    //--------------------------------------------------------------------------------------------------- 
    void OnClickWithdrawItem()
    {
        if (CsGameData.Instance.MyHeroInfo.InventorySlotCount > CsGameData.Instance.MyHeroInfo.InventorySlotList.Count)
        {
            CsCommandEventManager.Instance.SendWarehouseWithdraw(m_nSelectObjectIndex);
        }
        else
        {
            // 가방에 더이상 자리가 없을 경우
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A113_TXT_02001"));
        }
        ClosePopupItemInfo();
    }

    //--------------------------------------------------------------------------------------------------- 
    void OnClickClosePopup()
    {
        ClosePopupItemInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMainGearShortCut(int nButtonIndex)
    {
        switch ((EnMainGearShortCutType)nButtonIndex)
        {
            case EnMainGearShortCutType.Enchant:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.MainGear, EnSubMenu.MainGearEnchant);
                break;
            case EnMainGearShortCutType.Inherit:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.MainGear, EnSubMenu.MainGearRefine);
                break;
            case EnMainGearShortCutType.Share:
                CsGameEventUIToUI.Instance.OnEventGearShare(m_csHeroMainGearSelect);
                ClosePopupItemInfo();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMountGearShortCut(int nButtonIndex)
    {
        switch ((EnMountGearShortCutType)nButtonIndex)
        {
            case EnMountGearShortCutType.Enchant:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountGearRefine);
                break;
            case EnMountGearShortCutType.Share:
                CsGameEventUIToUI.Instance.OnEventGearShare(m_csHeroMountGear);
                ClosePopupItemInfo();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSubGearShortCut(int nButtonIndex)
    {
        switch ((EnSubGearShortCutType)nButtonIndex)
        {
            case EnSubGearShortCutType.Refine:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearLevelUp);
                break;
            case EnSubGearShortCutType.Sokcet:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearSoulstone);
                break;
            case EnSubGearShortCutType.Engrave:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearRune);
                break;
            case EnSubGearShortCutType.Share:
                CsGameEventUIToUI.Instance.OnEventGearShare(m_csHeroSubGearSelect);
                ClosePopupItemInfo();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemShortCut(int nButtonIndex)
    {
        switch ((EnItemShortCutType)nButtonIndex)
        {
            case EnItemShortCutType.Refine:
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingShortCut(int nButtonIndex)
    {
        switch ((EnWingShortCutType)nButtonIndex)
        {
            case EnWingShortCutType.Enchant:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Wing, EnSubMenu.WingEnchant);
                break;
            case EnWingShortCutType.Appearance:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Wing, EnSubMenu.WingAppearance);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSoulStoneInfo(Toggle toggleSoulStone)
    {
        Image imageArrow = toggleSoulStone.transform.Find("Background").GetComponent<Image>();
        Text textToggle = toggleSoulStone.transform.Find("Label").GetComponent<Text>();

        Transform trSoulStone = m_trContentSubGear.Find("SoulStone");
        Transform trSoulStoneList = trSoulStone.Find("SoulStoneList");
        Transform trSoulStoneInfoList = trSoulStone.Find("SoulStoneInfoList");

        if (toggleSoulStone.isOn)
        {
            //펼친상태
            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_arrow01_up");
            textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00023");

            trSoulStoneList.gameObject.SetActive(false);
            trSoulStoneInfoList.gameObject.SetActive(true);
        }
        else
        {
            //접힌상태
            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_arrow01_down");
            textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00022");

            trSoulStoneList.gameObject.SetActive(true);
            trSoulStoneInfoList.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedRuneInfo(Toggle toggleRune)
    {
        Image imageArrow = toggleRune.transform.Find("Background").GetComponent<Image>();
        Text textToggle = toggleRune.transform.Find("Label").GetComponent<Text>();

        Transform trRune = m_trContentSubGear.Find("Rune");
        Transform trRuneList = trRune.Find("RuneList");
        Transform trRuneInfoList = trRune.Find("RuneInfoList");

        if (toggleRune.isOn)
        {
            //펼친상태
            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_arrow01_up");
            textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00023");

            trRuneList.gameObject.SetActive(false);
            trRuneInfoList.gameObject.SetActive(true);
        }
        else
        {
            //접힌상태
            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_arrow01_down");
            textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00022");

            trRuneList.gameObject.SetActive(true);
            trRuneInfoList.gameObject.SetActive(false);
        }
    }

    #endregion EventHandler

    #region Event

    //----------------------------------------------------------------------------------------------------
    // 아이템 정보창 종료
    public void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        if (EventClosePopupItemInfo != null)
        {
            EventClosePopupItemInfo(enPopupItemInfoPositionType);
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitialIzeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = m_trCanvas2.Find("PopupList");

        m_trImageBack = transform.Find("ImageBackground");
        m_trItemSlot = m_trImageBack.Find("ItemSlot");

        m_rtScrollView = m_trImageBack.Find("Scroll View").GetComponent<RectTransform>();
        Transform trContent = m_trImageBack.Find("Scroll View/Viewport/Content");
        m_trContentMainGear = trContent.Find("MainGear");
        m_trContentSubGear = trContent.Find("SubGear");
        m_trContentItem = trContent.Find("Item");
        m_trContentWing = trContent.Find("Wing");
        m_trContentCard = trContent.Find("Card");
        m_trContentCostume = trContent.Find("Costume");
        m_trContentCostumeEffect = trContent.Find("CostumeEffect");

        m_imageCoolTime = m_trItemSlot.Find("ImageCooltime").GetComponent<Image>();
        m_imageCoolTime.fillAmount = 0;

        m_textItemName = m_trImageBack.Find("TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textItemName);

        Button buttonClose = m_trImageBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopup);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trBaseInfo = m_trImageBack.Find("BaseInfo");

        //메인기어 기본정보
        m_trBaseInfoMainGear = trBaseInfo.Find("MainGear");

        //서브기어 기본정보
        m_trBaseInfoSubGear = trBaseInfo.Find("SubGear");

        //아이템 기본정보
        m_trBaseInfoItem = trBaseInfo.Find("Item");

        //날개 기본정보
        m_trBaseInfoWing = trBaseInfo.Find("Wing");

        //카드 기본정보
        m_trBaseInfoCard = trBaseInfo.Find("Card");

        // 코스튬 기본정보
        m_trBaseInfoCostume = trBaseInfo.Find("Costume");

        // 코스튬 효과 기본정보
        m_trBaseInfoCostumeEffect = trBaseInfo.Find("CostumeEffect");

        //버튼리스트 이름
        //m_textItemShortCutName = m_trImageBack.Find("TextListName").GetComponent<Text>();
        //CsUIData.Instance.SetFont(m_textItemShortCutName);
        //m_textItemShortCutName.text = CsConfiguration.Instance.GetString("A02_TXT_00011");

        //숏컷버튼리스트 세팅
        Transform trShortCutList = m_trImageBack.Find("LeftButtonList");

        //메인기어 숏컷
        m_trMainGearShortCutList = trShortCutList.Find("MainGear");

        //탈것장비 숏컷
        m_trMountGearShortCoutList = trShortCutList.Find("MountGear");

        //서브기어 숏컷
        m_trSubGearShortCutList = trShortCutList.Find("SubGear");

        //아이템 숏컷
        m_trItemShortCutList = trShortCutList.Find("Item");

        //날개
        m_trWingShortCutList = trShortCutList.Find("Wing");

        Text textItemShortCutName = m_trContentItem.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemShortCutName);
        textItemShortCutName.text = CsConfiguration.Instance.GetString("A02_TXT_00018");

        //아이템 사용 장착 해제 버튼
        m_trButtonBack = m_trImageBack.Find("ImageButtonBack");
        Transform trUseButtonList = m_trButtonBack.Find("UseButtonList");

        m_buttonUse = trUseButtonList.Find("ButtonUse").GetComponent<Button>();
        m_buttonUse.onClick.RemoveAllListeners();
        m_buttonUse.onClick.AddListener(OnClickItemUse);
        m_buttonUse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textUse = m_buttonUse.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textUse);

        m_buttonMultyUse = trUseButtonList.Find("ButtonMultyUse").GetComponent<Button>();
        m_buttonMultyUse.onClick.RemoveAllListeners();
        m_buttonMultyUse.onClick.AddListener(OnClickItemMultyUse);
        m_buttonMultyUse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textMultyUse = m_buttonMultyUse.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMultyUse);

        m_buttonDeposit = trUseButtonList.Find("ButtonDeposit").GetComponent<Button>();
        m_buttonDeposit.onClick.RemoveAllListeners();
        m_buttonDeposit.onClick.AddListener(OnClickDepositItem);
        m_buttonDeposit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textDeposit = m_buttonDeposit.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDeposit);
        m_textDeposit.text = CsConfiguration.Instance.GetString("A113_BTN_00006");

        m_buttonWithdraw = trUseButtonList.Find("ButtonWithdraw").GetComponent<Button>();
        m_buttonWithdraw.onClick.RemoveAllListeners();
        m_buttonWithdraw.onClick.AddListener(OnClickWithdrawItem);
        m_buttonWithdraw.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textWithdraw = m_buttonWithdraw.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWithdraw);
        m_textWithdraw.text = CsConfiguration.Instance.GetString("A113_BTN_00007");
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupItemInfo()
    {
        OnClickClosePopupSelectUseCount();
        OnEventClosePopupItemInfo(m_enPopupItemInfoPositionType);
    }

    #region Display

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsHeroMainGear csHeroMainGear, bool bMainGearIsEquiped, bool bButtonOn = true, EnItemLocationType enItemLocationType = EnItemLocationType.None, int nSelectObjectIndex = 0)
    {
        m_bButtonOn = bButtonOn;
        m_enItemLocationType = enItemLocationType;
        m_nSelectObjectIndex = nSelectObjectIndex;
        DisplayPosition(enPopupItemInfoPositionType);
        m_bMainGearIsEquiped = bMainGearIsEquiped;
        m_csHeroMainGearSelect = csHeroMainGear;
        UpdateHeroMainGear(csHeroMainGear);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsMainGear csMainGear, bool bButtonOn = true)
    {
        m_bButtonOn = bButtonOn;
        DisplayPosition(enPopupItemInfoPositionType);
        UpdateMainGear(csMainGear);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsHeroSubGear csHeroSubGear, bool bButtonOn = true, EnItemLocationType enItemLocationType = EnItemLocationType.None, int nSelectObjectIndex = 0)
    {
        m_bButtonOn = bButtonOn;
        m_enItemLocationType = enItemLocationType;
        m_nSelectObjectIndex = nSelectObjectIndex;
        DisplayPosition(enPopupItemInfoPositionType);
        m_csHeroSubGearSelect = csHeroSubGear;
        UpdateHeroSubGear(csHeroSubGear);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsSubGear csSubGear, bool bButtonOn = true)
    {
        m_bButtonOn = bButtonOn;
        DisplayPosition(enPopupItemInfoPositionType);
        UpdateSubGear(csSubGear);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsItem csItem, int nCount, bool bOwned, int nInventorytListIndex, bool bButtonOn = true, bool bVisibleusingRecommendationEnabled = true, EnItemLocationType enItemLocationType = EnItemLocationType.None, int nSelectObjectIndex = 0)
    {
        m_bButtonOn = bButtonOn;
        m_enItemLocationType = enItemLocationType;
        m_nSelectObjectIndex = nSelectObjectIndex;
        DisplayPosition(enPopupItemInfoPositionType);
        m_csItemSelect = csItem;
        m_nInventoryListIndex = nInventorytListIndex;
        UpdateItem(csItem, nCount, bOwned, bVisibleusingRecommendationEnabled);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsHeroMountGear csHeroMountGear, bool bMountGearIsEquiped, bool bButtonOn = true, EnItemLocationType enItemLocationType = EnItemLocationType.None, int nSelectObjectIndex = 0)
    {
        m_bButtonOn = bButtonOn;
        m_enItemLocationType = enItemLocationType;
        m_nSelectObjectIndex = nSelectObjectIndex;
        DisplayPosition(enPopupItemInfoPositionType);
        m_csHeroMountGear = csHeroMountGear;
        m_bMountGearIsEquiped = bMountGearIsEquiped;
        UpdateHeroMountGear(csHeroMountGear);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, int nWingId, int nWingLevel, int nWingStep, List<CsWing> listWing, List<CsHeroWingPart> listWingPart, bool bButtonOn = true)
    {
        m_bButtonOn = bButtonOn;
        DisplayPosition(enPopupItemInfoPositionType);
        UpdateWing(nWingId, nWingLevel, nWingStep, listWing, listWingPart);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsHeroCostume csHeroCostume, bool bButtonOn = true)
    {
        m_bButtonOn = bButtonOn;
        DisplayPosition(enPopupItemInfoPositionType);
        UpdateCostume(csHeroCostume);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupItemInfoPositionType enPopupItemInfoPositionType, CsCreatureCard csCreatureCard, bool bButtonOn = false)
    {
        m_bButtonOn = bButtonOn;
        DisplayPosition(enPopupItemInfoPositionType);
        UpdateCreatureCard(csCreatureCard);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPosition(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        RectTransform rtfImageBack = m_trImageBack.GetComponent<RectTransform>();
        Transform trImageDim = transform.Find("ImageDim");
        m_enPopupItemInfoPositionType = enPopupItemInfoPositionType;

        switch (enPopupItemInfoPositionType)
        {
            case EnPopupItemInfoPositionType.Left:
                rtfImageBack.anchoredPosition = new Vector2(-315, -60);
                trImageDim.gameObject.SetActive(false);
                this.name = "PopupItemInfoLeft";
                break;

            case EnPopupItemInfoPositionType.Right:
                rtfImageBack.anchoredPosition = new Vector2(315, -60);
                trImageDim.gameObject.SetActive(false);
                this.name = "PopupItemInfoRight";
                break;

            case EnPopupItemInfoPositionType.Center:
                rtfImageBack.anchoredPosition = new Vector2(0, 0);
                trImageDim.gameObject.SetActive(true);
                this.name = "PopupItemInfoCenter";
                break;
        }
    }

    #endregion Display

    #region Main Gear

    //---------------------------------------------------------------------------------------------------
    //Hero Main Gear
    void UpdateHeroMainGear(CsHeroMainGear csHeroMainGear)
    {
        UpdateMainGear(csHeroMainGear.MainGear, csHeroMainGear.EnchantLevel, csHeroMainGear.BattlePower, csHeroMainGear.Owned);

        Transform trState = m_trBaseInfoMainGear.Find("State");

        Text textGearBattlePower = trState.Find("TextBattlePower").GetComponent<Text>();
        textGearBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), csHeroMainGear.BattlePower);

        //추가옵션

        Transform trAddOption = m_trContentMainGear.Find("AddOption");
        trAddOption.gameObject.SetActive(true);

        Text textAddOptionName = trAddOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAddOptionName);
        textAddOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00013");
        Transform trAddOptionList = m_trContentMainGear.Find("AddOption/AddOptionList");

        for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; i++)
        {
            Transform trAttr = trAddOptionList.Find("Option" + i);
            Text textOptionName;
            Text textValue;

            if (m_goOption == null)
            {
                m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goOption, trAddOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textOptionName = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textValue = trAttr.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);
                textOptionName = trAttr.GetComponent<Text>();
                textValue = trAttr.Find("TextValue").GetComponent<Text>();
            }

            textOptionName.text = string.Format("<color={0}>{1}</color>", csHeroMainGear.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode, csHeroMainGear.OptionAttrList[i].Attr.Name);
            textValue.text = string.Format("<color={0}>{1}</color>", csHeroMainGear.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode, csHeroMainGear.OptionAttrList[i].Value.ToString("#,###"));
        }

        for (int i = 0; i < trAddOptionList.childCount - csHeroMainGear.OptionAttrList.Count; i++)
        {
            Transform trAttr = trAddOptionList.Find("Option" + (i + csHeroMainGear.OptionAttrList.Count));
            trAttr.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //Main Gear
    void UpdateMainGear(CsMainGear csMainGear, int nEnchantLevel = 0, int nBattlePower = 0, bool bOwned = false)
    {
        m_enInventoryObjectTypeNow = EnInventoryObjectType.MainGear;
        m_textItemName.text = string.Format("<color={0}>{1}</color>", csMainGear.MainGearGrade.ColorCode, csMainGear.Name);

        CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csMainGear, nEnchantLevel, nBattlePower, bOwned, EnItemSlotSize.Large);

        //메인기어 기본정보
        m_trBaseInfoMainGear.gameObject.SetActive(true);
        m_trBaseInfoSubGear.gameObject.SetActive(false);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoWing.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(false);
        m_trBaseInfoCostume.gameObject.SetActive(false);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        Transform trEnchantIconList = m_trBaseInfoMainGear.Find("EnchantIconList");
        trEnchantIconList.gameObject.SetActive(true);
        CsUIData.Instance.UpdateEnchantLevelIcon(trEnchantIconList, nEnchantLevel);

        Transform trState = m_trBaseInfoMainGear.Find("State");

        Text textGearGrade = trState.Find("TextGrade").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearGrade);
        textGearGrade.text = csMainGear.MainGearGrade.Name;

        Text textGearQuality = trState.Find("TextQuality").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearQuality);
        textGearQuality.text = csMainGear.MainGearQuality.Name;

        Text textGearJob = trState.Find("TextJob").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearJob);
        string strJob;

        if (csMainGear.Job == null)
        {
            strJob = CsConfiguration.Instance.GetString("JOB_NAME_0");
        }
        else
        {
            strJob = csMainGear.Job.Name;
        }

        textGearJob.text = string.Format(CsConfiguration.Instance.GetString("INPUT_JOB"), strJob);

        Text textGearLevel = trState.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearLevel);
        textGearLevel.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01002"), csMainGear.MainGearTier.RequiredHeroLevel);

        Text textGearBattlePower = trState.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearBattlePower);
        textGearBattlePower.text = "";

        //숏컷 세팅
        m_trMainGearShortCutList.gameObject.SetActive(m_bButtonOn);
        //m_textItemShortCutName.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            for (int i = 0; i < m_trMainGearShortCutList.childCount; i++)
            {
                int nButtonIndex = i;

                Button buttonGearShortCut = m_trMainGearShortCutList.Find("Button" + i).GetComponent<Button>();

                buttonGearShortCut.onClick.RemoveAllListeners();
                buttonGearShortCut.onClick.AddListener(() => OnClickMainGearShortCut(nButtonIndex));
                buttonGearShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textMainGearShortCut = buttonGearShortCut.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textMainGearShortCut);
                switch ((EnMainGearShortCutType)nButtonIndex)
                {
                    case EnMainGearShortCutType.Enchant:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MainGearEnchant))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textMainGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00028");
                        break;

                    case EnMainGearShortCutType.Inherit:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MainGearRefine))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textMainGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00029");
                        break;

                    case EnMainGearShortCutType.Share:

                        textMainGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00030");
                        break;
                }
            }
        }

        //숏컷버튼리스트
        m_trMountGearShortCoutList.gameObject.SetActive(false);
        m_trSubGearShortCutList.gameObject.SetActive(false);
        m_trItemShortCutList.gameObject.SetActive(false);
        m_trWingShortCutList.gameObject.SetActive(false);

        //옵션
        m_trContentMainGear.gameObject.SetActive(true);
        m_trContentSubGear.gameObject.SetActive(false);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(false);
        m_trContentCard.gameObject.SetActive(false);
        m_trContentCostume.gameObject.SetActive(false);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        //기본옵션
        Transform trBaseOption = m_trContentMainGear.Find("BaseOption");

        Text textBaseOptionName = trBaseOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBaseOptionName);
        textBaseOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00012");

        Transform trBaseOptionList = m_trContentMainGear.Find("BaseOption/BaseOptionList");

        for (int i = 0; i < csMainGear.MainGearBaseAttrList.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + i);
            Text textOptionName;
            Text textOptionValue;

            if (m_goBaseOption == null)
            {
                m_goBaseOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextBaseOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goBaseOption, trBaseOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textOptionName = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);

                textOptionName = trAttr.GetComponent<Text>();
                textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
            }

            textOptionName.text = csMainGear.MainGearBaseAttrList[i].Attr.Name;

            CsMainGearBaseAttrEnchantLevel CsMainGearBaseAttrEnchantLevel = csMainGear.MainGearBaseAttrList[i].GetMainGearBaseAttrEnchantLevel(nEnchantLevel);
            CsMainGearBaseAttrEnchantLevel CsMainGearBaseAttrEnchantLevelZero = csMainGear.MainGearBaseAttrList[i].GetMainGearBaseAttrEnchantLevel(0);

            int nNowValue = 0;
            int nZeroValue = 0;

            if (CsMainGearBaseAttrEnchantLevel != null && CsMainGearBaseAttrEnchantLevelZero != null)
            {
                nNowValue = CsMainGearBaseAttrEnchantLevel.Value;
                nZeroValue = CsMainGearBaseAttrEnchantLevelZero.Value;
            }

            if (nEnchantLevel == 0)
            {
                textOptionValue.text = nNowValue.ToString("#,##0");
            }
            else
            {
                textOptionValue.text = string.Format(CsConfiguration.Instance.GetString(""), nZeroValue.ToString("#,###"), (nNowValue - nZeroValue).ToString("#,###"));
            }
        }

        for (int i = 0; i < trBaseOptionList.childCount - csMainGear.MainGearBaseAttrList.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + (i + csMainGear.MainGearBaseAttrList.Count));
            trAttr.gameObject.SetActive(false);
        }

        //추가옵션
        Transform trAddOption = m_trContentMainGear.Find("AddOption");
        trAddOption.gameObject.SetActive(false);

        //세트옵션
        Transform trSetOption = m_trContentMainGear.Find("SetOption");

        CsMainGearSet csMainGearSet = CsGameData.Instance.GetMainGearSet(csMainGear.MainGearTier.Tier, csMainGear.MainGearGrade.Grade, csMainGear.MainGearQuality.Quality);

        if (csMainGearSet == null)
        {
            trSetOption.gameObject.SetActive(false);
        }
        else
        {
            trSetOption.gameObject.SetActive(true);

            Text textSetOptionName = trSetOption.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textSetOptionName);
            textSetOptionName.text = csMainGearSet.Name;

            Transform trSetOptionList = trSetOption.Find("SetOptionList");

            for (int i = 0; i < csMainGearSet.MainGearSetAttrList.Count; i++)
            {
                Transform trAttr = trSetOptionList.Find("Option" + i);
                Text textOptionName;
                Text textOptionValue;

                if (m_goBaseOption == null)
                {
                    m_goBaseOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextBaseOptionName");
                }

                if (trAttr == null)
                {
                    GameObject goOption = Instantiate(m_goBaseOption, trSetOptionList);
                    goOption.name = "Option" + i;
                    trAttr = goOption.transform;

                    textOptionName = trAttr.GetComponent<Text>();
                    CsUIData.Instance.SetFont(textOptionName);

                    textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textOptionValue);
                }
                else
                {
                    trAttr.gameObject.SetActive(true);

                    textOptionName = trAttr.GetComponent<Text>();
                    textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
                }

                textOptionName.text = csMainGearSet.MainGearSetAttrList[i].Attr.Name;
                textOptionValue.text = csMainGearSet.MainGearSetAttrList[i].AttrValueInfo.Value.ToString("#,##0");

                if (m_bMainGearIsEquiped)
                {
                    bool bSet = true;

                    for (int j = 0; j < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; j++)
                    {
                        CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[j];
                        if (csHeroMainGear != null &&
                            (csHeroMainGear.MainGear.MainGearTier.Tier != csMainGear.MainGearTier.Tier ||
                            csHeroMainGear.MainGear.MainGearGrade.Grade != csMainGear.MainGearGrade.Grade ||
                            csHeroMainGear.MainGear.MainGearQuality.Quality != csMainGear.MainGearQuality.Quality))
                        {
                            bSet = false;
                        }
                    }

                    if (bSet)
                    {
                        textOptionName.color = CsUIData.Instance.ColorMellow;
                        textOptionValue.color = CsUIData.Instance.ColorMellow;
                    }
                    else
                    {
                        textOptionName.color = CsUIData.Instance.ColorGray;
                        textOptionValue.color = CsUIData.Instance.ColorGray;
                    }
                }
                else
                {
                    textOptionName.color = CsUIData.Instance.ColorGray;
                    textOptionValue.color = CsUIData.Instance.ColorGray;
                }
            }

            for (int i = 0; i < trSetOptionList.childCount - csMainGearSet.MainGearSetAttrList.Count; i++)
            {
                Transform trAttr = trSetOptionList.Find("Option" + (i + csMainGearSet.MainGearSetAttrList.Count));

                if (trAttr != null)
                {
                    trAttr.gameObject.SetActive(false);
                }
            }
        }

        //버튼
        m_buttonUse.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

            if (m_csHeroMainGearSelect.MainGear.Job == null || m_csHeroMainGearSelect.MainGear.Job.JobId == nJobId)
            {
                if (m_csHeroMainGearSelect.MainGear.MainGearTier.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, true);
                }
                else
                {
                    //레벨부족
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, false);
                }
            }
            else
            {
                //직업다름
                CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, false);
            }


            if (m_bMainGearIsEquiped)
            {
                m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00035");
            }
            else
            {
                m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00031");
            }
        }

        m_buttonMultyUse.gameObject.SetActive(false);

        ButtonSwitching();        
    }

    #endregion Main Gear

    #region Mount Gear

    //---------------------------------------------------------------------------------------------------
    //Hero Mount Gear
    void UpdateHeroMountGear(CsHeroMountGear csHeroMountGear)
    {
        UpdateMountGear(csHeroMountGear.MountGear, csHeroMountGear.Owned);

        Transform trState = m_trBaseInfoMainGear.Find("State");

        Text textGearBattlePower = trState.Find("TextBattlePower").GetComponent<Text>();
        textGearBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), csHeroMountGear.BattlePower);

        //추가옵션

        Transform trAddOption = m_trContentMainGear.Find("AddOption");
        trAddOption.gameObject.SetActive(true);

        Text textAddOptionName = trAddOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAddOptionName);
        textAddOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00013");
        Transform trAddOptionList = m_trContentMainGear.Find("AddOption/AddOptionList");

        for (int i = 0; i < csHeroMountGear.HeroMountGearOptionAttrList.Count; i++)
        {
            Transform trAttr = trAddOptionList.Find("Option" + i);
            Text textOptionName;
            Text textValue;

            if (m_goOption == null)
            {
                m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goOption, trAddOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textOptionName = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textValue = trAttr.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);
                textOptionName = trAttr.GetComponent<Text>();
                textValue = trAttr.Find("TextValue").GetComponent<Text>();
            }

            textOptionName.text = string.Format("<color={0}>{1}</color>", csHeroMountGear.HeroMountGearOptionAttrList[i].MountGearOptionAttrGrade.ColorCode, csHeroMountGear.HeroMountGearOptionAttrList[i].Attr.Name);
            textValue.text = string.Format("<color={0}>{1}</color>", csHeroMountGear.HeroMountGearOptionAttrList[i].MountGearOptionAttrGrade.ColorCode, csHeroMountGear.HeroMountGearOptionAttrList[i].AttrValueInfo.Value.ToString("#,###"));
        }

        for (int i = 0; i < trAddOptionList.childCount - csHeroMountGear.HeroMountGearOptionAttrList.Count; i++)
        {
            Transform trAttr = trAddOptionList.Find("Option" + (i + csHeroMountGear.HeroMountGearOptionAttrList.Count));
            trAttr.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //Mount Gear
    void UpdateMountGear(CsMountGear csMountGear, bool bOwned)
    {
        m_enInventoryObjectTypeNow = EnInventoryObjectType.MountGear;

        m_textItemName.text = string.Format("<color={0}>{1}</color>", csMountGear.MountGearGrade.ColorCode, csMountGear.Name);

        CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csMountGear, bOwned, EnItemSlotSize.Large);

        //메인기어 기본정보
        m_trBaseInfoMainGear.gameObject.SetActive(true);
        m_trBaseInfoSubGear.gameObject.SetActive(false);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoWing.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(false);
        m_trBaseInfoCostume.gameObject.SetActive(false);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        Transform trEnchantIconList = m_trBaseInfoMainGear.Find("EnchantIconList");
        trEnchantIconList.gameObject.SetActive(false);

        Transform trState = m_trBaseInfoMainGear.Find("State");

        Text textGearGrade = trState.Find("TextGrade").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearGrade);
        textGearGrade.text = csMountGear.Name;

        Text textGearQuality = trState.Find("TextQuality").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearQuality);
        textGearQuality.text = csMountGear.MountGearQuality.Name;

        Text textGearJob = trState.Find("TextJob").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearJob);
        textGearJob.text = "";

        Text textGearLevel = trState.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearLevel);
        textGearLevel.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01002"), csMountGear.RequiredHeroLevel);

        Text textGearBattlePower = trState.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearBattlePower);
        textGearBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), csMountGear.BattlePowerValue);

        //숏컷 세팅
        m_trMountGearShortCoutList.gameObject.SetActive(m_bButtonOn);
        //m_textItemShortCutName.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            for (int i = 0; i < m_trMountGearShortCoutList.childCount; i++)
            {
                int nButtonIndex = i;

                Button buttonGearShortCut = m_trMountGearShortCoutList.Find("Button" + i).GetComponent<Button>();
                buttonGearShortCut.onClick.RemoveAllListeners();
                buttonGearShortCut.onClick.AddListener(() => OnClickMountGearShortCut(nButtonIndex));
                buttonGearShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textMountGearShortCut = buttonGearShortCut.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textMountGearShortCut);

                switch ((EnMountGearShortCutType)nButtonIndex)
                {
                    case EnMountGearShortCutType.Enchant:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MountLevelUp))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textMountGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00028");
                        break;

                    case EnMountGearShortCutType.Share:
                        textMountGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00030");
                        break;
                }
            }
        }

        //숏컷버튼리스트
        m_trMainGearShortCutList.gameObject.SetActive(false);
        m_trSubGearShortCutList.gameObject.SetActive(false);
        m_trItemShortCutList.gameObject.SetActive(false);
        m_trWingShortCutList.gameObject.SetActive(false);

        //옵션
        m_trContentMainGear.gameObject.SetActive(true);
        m_trContentSubGear.gameObject.SetActive(false);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(false);
        m_trContentCard.gameObject.SetActive(false);
        m_trContentCostume.gameObject.SetActive(false);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        //기본옵션
        Transform trBaseOption = m_trContentMainGear.Find("BaseOption");

        Text textBaseOptionName = trBaseOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBaseOptionName);
        textBaseOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00012");

        Transform trBaseOptionList = m_trContentMainGear.Find("BaseOption/BaseOptionList");

        int nCount = 0;

        if (csMountGear.MaxHp > 0)
        {
            CreateMountGearBaseOption(trBaseOptionList, csMountGear, CsConfiguration.Instance.GetString("ATTR_NAME_1"), csMountGear.MaxHp.ToString("#,###"));
            nCount++;
        }

        if (csMountGear.PhysicalOffense > 0)
        {
            CreateMountGearBaseOption(trBaseOptionList, csMountGear, CsConfiguration.Instance.GetString("ATTR_NAME_2"), csMountGear.PhysicalOffense.ToString("#,###"));
            nCount++;
        }

        if (csMountGear.MagicalOffenseAttr > 0)
        {
            CreateMountGearBaseOption(trBaseOptionList, csMountGear, CsConfiguration.Instance.GetString("ATTR_NAME_3"), csMountGear.MagicalOffenseAttr.ToString("#,###"));
            nCount++;
        }

        if (csMountGear.PhysicalDefense > 0)
        {
            CreateMountGearBaseOption(trBaseOptionList, csMountGear, CsConfiguration.Instance.GetString("ATTR_NAME_4"), csMountGear.PhysicalDefense.ToString("#,###"));
            nCount++;
        }

        if (csMountGear.MagicalDefense > 0)
        {
            CreateMountGearBaseOption(trBaseOptionList, csMountGear, CsConfiguration.Instance.GetString("ATTR_NAME_5"), csMountGear.MagicalDefense.ToString("#,###"));
            nCount++;
        }

        for (int i = 0; i < trBaseOptionList.childCount - nCount; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + (nCount));
            trAttr.gameObject.SetActive(false);
        }

        //추가옵션
        Transform trAddOption = m_trContentMainGear.Find("AddOption");
        trAddOption.gameObject.SetActive(false);

        //세트옵션
        Transform trSetOption = m_trContentMainGear.Find("SetOption");
        trSetOption.gameObject.SetActive(false);

        //버튼
        m_buttonUse.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            if (csMountGear.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, true);
            }
            else
            {
                //레벨부족
                CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, false);
            }

            if (m_bMountGearIsEquiped)
            {
                m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00035");
            }
            else
            {
                m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00031");
            }
        }

        m_buttonMultyUse.gameObject.SetActive(false);

        ButtonSwitching();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateMountGearBaseOption(Transform trBaseOptionList, CsMountGear csMountGear, string strName, string strValue)
    {
        if (m_goBaseOption == null)
        {
            m_goBaseOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextBaseOptionName");
        }

        GameObject goOption = Instantiate(m_goBaseOption, trBaseOptionList);
        goOption.name = "OptionMaxHp";
        Transform trAttr = goOption.transform;

        Text textOptionName = trAttr.GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionName);
        textOptionName.text = strName;

        Text textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionValue);
        textOptionValue.text = strValue;
    }

    #endregion Mount Gear

    #region Sub Gear

    //---------------------------------------------------------------------------------------------------
    //Hero Sub Gear
    void UpdateHeroSubGear(CsHeroSubGear csHeroSubGear)
    {
        UpdateSubGear(csHeroSubGear.SubGear, csHeroSubGear.SubGearLevel.SubGearGrade.Grade, csHeroSubGear.SubGearLevel.Level, csHeroSubGear.Quality);

        Text textAssistantGearGrade = m_trBaseInfoSubGear.Find("TextGrade").GetComponent<Text>();
        textAssistantGearGrade.text = csHeroSubGear.SubGearLevel.SubGearGrade.Name;

        Text textAssistantGearPower = m_trBaseInfoSubGear.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAssistantGearPower);
        textAssistantGearPower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), csHeroSubGear.BattlePower);

        //기본옵션
        Transform trSubGearBaseOption = m_trContentSubGear.Find("BaseOption");

        Text textSubGearBaseOptionName = trSubGearBaseOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSubGearBaseOptionName);
        textSubGearBaseOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00012");

        Transform trBaseOptionList = trSubGearBaseOption.Find("BaseOptionList");

        for (int i = 0; i < csHeroSubGear.SubGear.SubGearAttrList.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + i);
            Text textAttr;
            Text textAttrValue;

            if (m_goOption == null)
            {
                m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goOption, trBaseOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textAttr = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttr);

                textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);

                textAttr = trAttr.GetComponent<Text>();
                textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
            }

            textAttr.text = csHeroSubGear.AttrValueList[i].Attr.Name;
            textAttrValue.text = csHeroSubGear.AttrValueList[i].Value.ToString("#,##0");

        }

        for (int i = 0; i < trBaseOptionList.childCount - csHeroSubGear.SubGear.SubGearAttrList.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + (i + csHeroSubGear.SubGear.SubGearAttrList.Count));

            if (trAttr != null)
            {
                trAttr.gameObject.SetActive(false);
            }
        }

        //소울스톤 리스트

        Transform trSubGearSoulStone = m_trContentSubGear.Find("SoulStone");
        Transform trSubGearSoulStoneList = trSubGearSoulStone.Find("SoulStoneList");

        for (int i = 0; i < csHeroSubGear.SoulstoneSocketList.Count; i++)
        {
            Transform trSoulStone = trSubGearSoulStoneList.Find("SoulStone" + csHeroSubGear.SoulstoneSocketList[i].Index);
            Image imageIcon = trSoulStone.Find("ImageIcon").GetComponent<Image>();
            imageIcon.gameObject.SetActive(true);
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csHeroSubGear.SoulstoneSocketList[i].Item.ItemId);

            //소울스톤 정보 리스트
            Transform trSubGearSoulStoneInfo = m_trContentSubGear.Find("SoulStone/SoulStoneInfoList");

            //소울스톤 상세정보 리스트 세팅
            Transform trSoulStoneInfo = trSubGearSoulStoneInfo.Find("SoulStoneInfo" + csHeroSubGear.SoulstoneSocketList[i].Index);

            Image imageInfoIcon = trSoulStoneInfo.Find("ImageIcon").GetComponent<Image>();
            imageInfoIcon.gameObject.SetActive(true);
            imageInfoIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csHeroSubGear.SoulstoneSocketList[i].Item.ItemId);

            Text textName = trSoulStoneInfo.Find("TextName").GetComponent<Text>();
            textName.text = csHeroSubGear.SoulstoneSocketList[i].Item.Name;

            Text textOptionName = trSoulStoneInfo.Find("TextOptionName").GetComponent<Text>();
            CsAttr csAttr = CsGameData.Instance.GetAttr(csHeroSubGear.SoulstoneSocketList[i].Item.Value1);

            if (csAttr != null)
            {
                textOptionName.text = csAttr.Name;
            }
            else
            {
                textOptionName.text = "";
            }

            Text textOptionValue = trSoulStoneInfo.Find("TextOptionValue").GetComponent<Text>();
            CsAttrValueInfo csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(csHeroSubGear.SoulstoneSocketList[i].Item.LongValue1);

            if (csAttrValueInfo != null)
            {
                textOptionValue.text = csAttrValueInfo.Value.ToString("#,##0");
            }
            else
            {
                textOptionValue.text = "";
            }
        }

        //룬
        Transform trSubGearRune = m_trContentSubGear.Find("Rune");
        Transform trSubGearRuneList = trSubGearRune.Find("RuneList");

        for (int i = 0; i < csHeroSubGear.RuneSocketList.Count; i++)
        {
            //룬 간략 리스트 세팅
            Transform trRune = trSubGearRuneList.Find("Rune" + csHeroSubGear.RuneSocketList[i].Index);
            Image imageIcon = trRune.Find("ImageIcon").GetComponent<Image>();
            imageIcon.gameObject.SetActive(true);
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csHeroSubGear.RuneSocketList[i].Item.ItemId);

            //룬 정보 리스트
            Transform trSubGearRuneInfoList = trSubGearRune.Find("RuneInfoList");
            Transform trSubGearRuneInfo = trSubGearRuneInfoList.Find("RuneInfo" + csHeroSubGear.RuneSocketList[i].Index);

            //룬 상세정보 리스트 세팅
            Image imageInfoIcon = trSubGearRuneInfo.Find("ImageIcon").GetComponent<Image>();
            imageInfoIcon.gameObject.SetActive(true);
            imageInfoIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csHeroSubGear.RuneSocketList[i].Item.ItemId);

            Text textName = trSubGearRuneInfo.Find("TextName").GetComponent<Text>();
            textName.text = csHeroSubGear.RuneSocketList[i].Item.Name;

            Text textOptionName = trSubGearRuneInfo.Find("TextOptionName").GetComponent<Text>();
            CsAttr csAttr = CsGameData.Instance.GetAttr(csHeroSubGear.RuneSocketList[i].Item.Value1);

            if (csAttr != null)
            {
                textOptionName.text = csAttr.Name;
            }
            else
            {
                textOptionName.text = "";
            }

            Text textOptionValue = trSubGearRuneInfo.Find("TextOptionValue").GetComponent<Text>();
            CsAttrValueInfo csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(csHeroSubGear.RuneSocketList[i].Item.LongValue1);

            if (csAttrValueInfo != null)
            {
                textOptionValue.text = csAttrValueInfo.Value.ToString("#,##0");
            }
            else
            {
                textOptionValue.text = "";
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //Sub Gear
    void UpdateSubGear(CsSubGear csSubGear, int nGrade = 1, int nLevel = 1, int nQuality = 0)
    {
        m_enInventoryObjectTypeNow = EnInventoryObjectType.SubGear;

        m_textItemName.text = csSubGear.Name;

        CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csSubGear, nGrade, nLevel, EnItemSlotSize.Large);

        //서브기어 기본정보
        m_trBaseInfoMainGear.gameObject.SetActive(false);
        m_trBaseInfoSubGear.gameObject.SetActive(true);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoWing.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(false);
        m_trBaseInfoCostume.gameObject.SetActive(false);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        Text textAssistantGearLevel = m_trBaseInfoSubGear.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAssistantGearLevel);
        textAssistantGearLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), nLevel);

        Text textAssistantGearGrade = m_trBaseInfoSubGear.Find("TextGrade").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAssistantGearGrade);
        textAssistantGearGrade.text = "";

        Text textAssistantGearQuality = m_trBaseInfoSubGear.Find("TextQuality").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAssistantGearQuality);

        if (nQuality > 0)
        {
            textAssistantGearQuality.text = string.Format(CsConfiguration.Instance.GetString("INPUT_QUALITYLEVEL"), nQuality);
        }
        else if (nQuality == 0)
        {
            textAssistantGearQuality.text = "";
        }

        Text textAssistantGearPower = m_trBaseInfoSubGear.Find("TextBattlePower").GetComponent<Text>();
        textAssistantGearPower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), CsConfiguration.Instance.GetString("A02_TXT_00025"));

        m_trSubGearShortCutList.gameObject.SetActive(m_bButtonOn);
        //m_textItemShortCutName.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            //숏컷 세팅
            for (int i = 0; i < m_trSubGearShortCutList.childCount; i++)
            {
                int nButtonIndex = i;

                Button buttonGearShortCut = m_trSubGearShortCutList.Find("Button" + i).GetComponent<Button>();
                buttonGearShortCut.onClick.RemoveAllListeners();
                buttonGearShortCut.onClick.AddListener(() => OnClickSubGearShortCut(nButtonIndex));
                buttonGearShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textSubGearShortCut = buttonGearShortCut.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textSubGearShortCut);

                switch ((EnSubGearShortCutType)nButtonIndex)
                {
                    case EnSubGearShortCutType.Refine:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearLevelUp))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textSubGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00032");
                        break;
                    case EnSubGearShortCutType.Sokcet:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearSoulstone))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textSubGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00033");
                        break;
                    case EnSubGearShortCutType.Engrave:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearRune))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textSubGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00034");
                        break;
                    case EnSubGearShortCutType.Share:
                        textSubGearShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00030");
                        break;
                }
            }
        }

        //숏컷버튼리스트
        m_trMainGearShortCutList.gameObject.SetActive(false);
        m_trMountGearShortCoutList.gameObject.SetActive(false);
        m_trItemShortCutList.gameObject.SetActive(false);
        m_trWingShortCutList.gameObject.SetActive(false);

        //옵션
        m_trContentMainGear.gameObject.SetActive(false);
        m_trContentSubGear.gameObject.SetActive(true);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(false);
        m_trContentCard.gameObject.SetActive(false);
        m_trContentCostume.gameObject.SetActive(false);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        //기본옵션
        Transform trSubGearBaseOption = m_trContentSubGear.Find("BaseOption");

        Text textSubGearBaseOptionName = trSubGearBaseOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSubGearBaseOptionName);
        textSubGearBaseOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00012");

        Transform trBaseOptionList = trSubGearBaseOption.Find("BaseOptionList");

        for (int i = 0; i < csSubGear.SubGearAttrList.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + i);
            Text textAttr;
            Text textAttrValue;

            if (m_goOption == null)
            {
                m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goOption, trBaseOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textAttr = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttr);

                textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);

                textAttr = trAttr.GetComponent<Text>();
                textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
            }

            textAttr.text = csSubGear.SubGearAttrList[i].Attr.Name;
            textAttrValue.text = csSubGear.SubGearAttrList[i].GetSubGearAttrValue(nLevel, nQuality).Value.ToString("#,##0");

        }

        for (int i = 0; i < trBaseOptionList.childCount - csSubGear.SubGearAttrList.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + (i + csSubGear.SubGearAttrList.Count));

            if (trAttr != null)
            {
                trAttr.gameObject.SetActive(false);
            }
        }

        //소울스톤 리스트

        Transform trSubGearSoulStone = m_trContentSubGear.Find("SoulStone");

        Text textSubGearSoulStoneName = trSubGearSoulStone.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSubGearSoulStoneName);
        textSubGearSoulStoneName.text = CsConfiguration.Instance.GetString("A02_TXT_00015");

        Toggle toggleSoulStone = textSubGearSoulStoneName.transform.Find("ToggleSoulStone").GetComponent<Toggle>();
        toggleSoulStone.onValueChanged.RemoveAllListeners();
        toggleSoulStone.onValueChanged.AddListener((ison) => OnValueChangedSoulStoneInfo(toggleSoulStone));
        toggleSoulStone.isOn = false;
        toggleSoulStone.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textSoulStoneToggle = toggleSoulStone.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSoulStoneToggle);


        Transform trSubGearSoulStoneList = trSubGearSoulStone.Find("SoulStoneList");


        for (int i = 0; i < csSubGear.SubGearSoulstoneSocketList.Count; i++)
        {
            int nSlotIndex = csSubGear.SubGearSoulstoneSocketList[i].SocketIndex;

            //소울스톤 간략 리스트 세팅
            Transform trSoulStone = trSubGearSoulStoneList.Find("SoulStone" + nSlotIndex);

            if (m_goSoulStone == null)
            {
                m_goSoulStone = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/SoulStone");
            }

            if (trSoulStone == null)
            {
                GameObject goSoulStone = Instantiate(m_goSoulStone, trSubGearSoulStoneList);
                goSoulStone.name = "SoulStone" + nSlotIndex;
                trSoulStone = goSoulStone.transform;
            }
            else
            {
                trSoulStone.gameObject.SetActive(true);
            }

            Image imageBack = trSoulStone.Find("ImageBackground").GetComponent<Image>();
            imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_mini_socket" + csSubGear.SubGearSoulstoneSocketList[i].ItemType);

            Image imageIcon = trSoulStone.Find("ImageIcon").GetComponent<Image>();
            imageIcon.gameObject.SetActive(false);

            Transform trLock = trSoulStone.Find("ImageLock");

            if (csSubGear.SubGearSoulstoneSocketList[i].RequiredSubGearGrade.Grade <= nGrade)
            {
                trLock.gameObject.SetActive(false);
            }
            else
            {
                trLock.gameObject.SetActive(true);
            }

            //소울스톤 정보 리스트
            Transform trSubGearSoulStoneInfo = m_trContentSubGear.Find("SoulStone/SoulStoneInfoList");

            //소울스톤 상세정보 리스트 세팅
            Transform trSoulStoneInfo = trSubGearSoulStoneInfo.Find("SoulStoneInfo" + nSlotIndex);
            Text textName;
            Text textOptionName;
            Text textOptionValue;
            Text textNO;

            if (m_goSoulStoneInfo == null)
            {
                m_goSoulStoneInfo = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/SoulStoneInfo");
            }

            if (trSoulStoneInfo == null)
            {
                GameObject goSoulStoneInfo = Instantiate(m_goSoulStoneInfo, trSubGearSoulStoneInfo);
                goSoulStoneInfo.name = "SoulStoneInfo" + nSlotIndex;
                trSoulStoneInfo = goSoulStoneInfo.transform;

                textName = trSoulStoneInfo.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);

                textOptionName = trSoulStoneInfo.Find("TextOptionName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textOptionValue = trSoulStoneInfo.Find("TextOptionValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionValue);

                textNO = trSoulStoneInfo.Find("TextNO").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNO);
            }
            else
            {
                trSoulStoneInfo.gameObject.SetActive(true);

                textName = trSoulStoneInfo.Find("TextName").GetComponent<Text>();
                textOptionName = trSoulStoneInfo.Find("TextOptionName").GetComponent<Text>();
                textOptionValue = trSoulStoneInfo.Find("TextOptionValue").GetComponent<Text>();
                textNO = trSoulStoneInfo.Find("TextNO").GetComponent<Text>();
            }

            Image imageInfoBack = trSoulStoneInfo.Find("ImageBackground").GetComponent<Image>();
            imageInfoBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_mini_socket" + csSubGear.SubGearSoulstoneSocketList[i].ItemType);

            Image imageInfoIcon = trSoulStoneInfo.Find("ImageIcon").GetComponent<Image>();
            imageInfoIcon.gameObject.SetActive(false);

            Transform trInfoLock = trSoulStoneInfo.Find("ImageLock");

            if (csSubGear.SubGearSoulstoneSocketList[i].RequiredSubGearGrade.Grade <= nGrade)
            {
                //추후 착용 미착용 구분해야함
                trInfoLock.gameObject.SetActive(false);

                textName.text = "";
                textOptionName.text = "";
                textOptionValue.text = "";
                textNO.text = "";
            }
            else
            {
                //오픈되지않음
                trInfoLock.gameObject.SetActive(true);

                textName.text = "";
                textOptionName.text = "";
                textOptionValue.text = "";
                textNO.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01003"), csSubGear.SubGearSoulstoneSocketList[i].RequiredSubGearGrade.Name);
            }
        }


        //룬
        Transform trSubGearRune = m_trContentSubGear.Find("Rune");

        Text textSubGearRuneName = trSubGearRune.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSubGearRuneName);
        textSubGearRuneName.text = CsConfiguration.Instance.GetString("A02_TXT_00017");

        Toggle toggleRune = textSubGearRuneName.transform.Find("ToggleSoulStone").GetComponent<Toggle>();
        toggleRune.onValueChanged.RemoveAllListeners();
        toggleRune.onValueChanged.AddListener((ison) => OnValueChangedRuneInfo(toggleRune));
        toggleRune.isOn = false;
        toggleRune.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textRuneToggle = toggleRune.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRuneToggle);

        //룬 리스트
        Transform trSubGearRuneList = trSubGearRune.Find("RuneList");

        for (int i = 0; i < csSubGear.SubGearRuneSocketList.Count; i++)
        {
            int nSlotIndex = csSubGear.SubGearRuneSocketList[i].SocketIndex;

            //룬 간략 리스트 세팅
            Transform trRune = trSubGearRuneList.Find("Rune" + nSlotIndex);

            if (m_goRune == null)
            {
                m_goRune = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/Rune");
            }

            if (trRune == null)
            {
                GameObject goRune = Instantiate(m_goRune, trSubGearRuneList);
                goRune.name = "Rune" + nSlotIndex;
                trRune = goRune.transform;
            }
            else
            {
                trRune.gameObject.SetActive(true);
            }

            Image imageBack = trRune.Find("ImageBackground").GetComponent<Image>();
            imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + csSubGear.SubGearRuneSocketList[i].MiniBackgroundImageName);

            Image imageIcon = trRune.Find("ImageIcon").GetComponent<Image>();
            imageIcon.gameObject.SetActive(false);
            Transform trLock = trRune.Find("ImageLock");

            if (csSubGear.SubGearRuneSocketList[i].RequiredSubGearLevel <= nLevel)
            {
                trLock.gameObject.SetActive(false);
            }
            else
            {
                trLock.gameObject.SetActive(true);
            }

            //룬 정보 리스트
            Transform trSubGearRuneInfoList = trSubGearRune.Find("RuneInfoList");
            trSubGearRuneInfoList.gameObject.SetActive(false);

            Transform trSubGearRuneInfo = trSubGearRuneInfoList.Find("RuneInfo" + nSlotIndex);

            //룬 상세정보 리스트 세팅
            Text textName;
            Text textOptionName;
            Text textOptionValue;
            Text textNO;

            if (m_goRuneInfo == null)
            {
                m_goRuneInfo = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/RuneInfo");
            }

            if (trSubGearRuneInfo == null)
            {
                GameObject goRuneInfo = Instantiate(m_goRuneInfo, trSubGearRuneInfoList);
                goRuneInfo.name = "RuneInfo" + nSlotIndex;
                trSubGearRuneInfo = goRuneInfo.transform;

                textName = trSubGearRuneInfo.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);

                textOptionName = trSubGearRuneInfo.Find("TextOptionName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textOptionValue = trSubGearRuneInfo.Find("TextOptionValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionValue);

                textNO = trSubGearRuneInfo.Find("TextNO").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNO);
            }
            else
            {
                trSubGearRuneInfo.gameObject.SetActive(true);

                textName = trSubGearRuneInfo.Find("TextName").GetComponent<Text>();
                textOptionName = trSubGearRuneInfo.Find("TextOptionName").GetComponent<Text>();
                textOptionValue = trSubGearRuneInfo.Find("TextOptionValue").GetComponent<Text>();
                textNO = trSubGearRuneInfo.Find("TextNO").GetComponent<Text>();
            }
            Image imageInfoBack = trSubGearRuneInfo.Find("ImageBackground").GetComponent<Image>();
            imageInfoBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + csSubGear.SubGearRuneSocketList[i].MiniBackgroundImageName);

            Image imageInfoIcon = trSubGearRuneInfo.Find("ImageIcon").GetComponent<Image>();
            imageInfoIcon.gameObject.SetActive(false);

            Transform trInfoLock = trSubGearRuneInfo.Find("ImageLock");

            if (csSubGear.SubGearRuneSocketList[i].RequiredSubGearLevel <= nLevel)
            {
                //추후 착용 미착용 구분해야함
                trInfoLock.gameObject.SetActive(false);

                textName.text = "";
                textOptionName.text = "";
                textOptionValue.text = "";
                textNO.text = "";
            }
            else
            {
                //오픈되지않음
                trInfoLock.gameObject.SetActive(true);

                textName.text = "";
                textOptionName.text = "";
                textOptionValue.text = "";
                textNO.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01004"), csSubGear.SubGearRuneSocketList[i].RequiredSubGearLevel);
            }
        }

        //버튼
        m_buttonUse.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, true);

            if (m_csHeroSubGearSelect.Equipped)
            {
                m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00035");
            }
            else
            {
                m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00031");
            }
        }

        m_buttonMultyUse.gameObject.SetActive(false);

        ButtonSwitching();
    }

    #endregion Sub Gear

    #region Item

    //---------------------------------------------------------------------------------------------------
    //아이템
    void UpdateItem(CsItem csItem, int nCount, bool bOwned, bool bVisibleUsingRecommendationEnabled)
    {
        m_enInventoryObjectTypeNow = EnInventoryObjectType.Item;

        m_textItemName.text = string.Format("<color={0}>{1}</color>", csItem.ItemGrade.ColorCode, csItem.Name);

        CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csItem, bOwned, nCount, csItem.UsingRecommendationEnabled, EnItemSlotSize.Large, bVisibleUsingRecommendationEnabled);

        //아이템 기본정보
        m_trBaseInfoMainGear.gameObject.SetActive(false);
        m_trBaseInfoSubGear.gameObject.SetActive(false);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoWing.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(false);
        m_trBaseInfoCostume.gameObject.SetActive(false);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        //옵션
        m_trContentMainGear.gameObject.SetActive(false);
        m_trContentSubGear.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(false);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentCard.gameObject.SetActive(false);
        m_trContentCostume.gameObject.SetActive(false);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        m_trItemShortCutList.gameObject.SetActive(false);

        if (m_bButtonOn)
        {
            //숏컷세팅
            for (int i = 0; i < m_trItemShortCutList.childCount; i++)
            {
                int nButtonIndex = i;

                Button buttonGearShortCut = m_trItemShortCutList.Find("Button" + i).GetComponent<Button>();
                buttonGearShortCut.onClick.RemoveAllListeners();
                buttonGearShortCut.onClick.AddListener(() => OnClickItemShortCut(nButtonIndex));
                buttonGearShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textItemShortCut = buttonGearShortCut.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textItemShortCut);

                switch ((EnItemShortCutType)nButtonIndex)
                {
                    case EnItemShortCutType.Refine:
                        textItemShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00032");
                        break;
                }
            }
        }

        //숏컷버튼리스트 차후 버튼 키고 끄는것에 따라가게 변경
        m_trMainGearShortCutList.gameObject.SetActive(false);
        m_trSubGearShortCutList.gameObject.SetActive(false);
        m_trMountGearShortCoutList.gameObject.SetActive(false);
        m_trWingShortCutList.gameObject.SetActive(false);

        if (csItem.ItemType.EnItemType == EnItemType.Costume)
        {
            Text textItemType = m_trBaseInfoCostume.Find("TextType").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemType);
            textItemType.text = csItem.ItemType.MainCategory.Name;

            Text textPeriodLimitDayType = m_trBaseInfoCostume.Find("TextPeriodLimitDayType").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPeriodLimitDayType);

            Text textRequiredLevel = m_trBaseInfoCostume.Find("TextRequiredLevel").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRequiredLevel);

            Text textPeriodLimitDay = m_trBaseInfoCostume.Find("TextPeriodLimitDay").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPeriodLimitDay);

            CsCostume csCostume = CsGameData.Instance.GetCostume(csItem.Value1);

            if (csCostume == null)
            {
                textPeriodLimitDayType.text = "";
                textRequiredLevel.text = "";
                textPeriodLimitDay.text = "";
            }
            else
            {
                if (csCostume.PeriodLimitDay == 0)
                {
                    // 영구
                    textPeriodLimitDayType.text = CsConfiguration.Instance.GetString("");
                    textPeriodLimitDay.gameObject.SetActive(false);
                }
                else
                {
                    // 기간제
                    textPeriodLimitDayType.text = CsConfiguration.Instance.GetString("");

                    textPeriodLimitDay.text = string.Format(CsConfiguration.Instance.GetString(""), csCostume.PeriodLimitDay);
                    textPeriodLimitDay.gameObject.SetActive(true);
                }

                textRequiredLevel.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_REQ_LEVEL"), csCostume.RequiredHeroLevel);
            }

            m_trBaseInfoCostume.gameObject.SetActive(true);

            Transform trBaseOption = m_trContentCostume.Find("BaseOption");

            Text textBaseOption = trBaseOption.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBaseOption);
            textBaseOption.text = CsConfiguration.Instance.GetString("A151_TXT_00033");

            Transform trBaseOptionList = trBaseOption.Find("BaseOptionList");

            Transform trAttr = null;
            Text textAttr = null;
            Text textAttrValue = null;

            if (m_goOption == null)
            {
                m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
            }

            List<CsCostumeEnchantLevelAttr> listCsCostumeEnchantLevelAttr = new List<CsCostumeEnchantLevelAttr>(CsGameData.Instance.CostumeEnchantLevelAttrList).FindAll(a => a.EnchantLevel == 0 && a.CostumeId == csCostume.CostumeId);

            for (int i = 0; i < listCsCostumeEnchantLevelAttr.Count; i++)
            {
                trAttr = trBaseOptionList.Find("Option" + i);

                if (trAttr == null)
                {
                    GameObject goOption = Instantiate(m_goOption, trBaseOptionList);
                    goOption.name = "Option" + i;
                    trAttr = goOption.transform;

                    textAttr = trAttr.GetComponent<Text>();
                    CsUIData.Instance.SetFont(textAttr);

                    textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textAttrValue);
                }
                else
                {
                    trAttr.gameObject.SetActive(true);

                    textAttr = trAttr.GetComponent<Text>();
                    textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
                }

                textAttr.text = listCsCostumeEnchantLevelAttr[i].Attr.Name;
                textAttrValue.text = listCsCostumeEnchantLevelAttr[i].AttrValue.Value.ToString("#,##0");
            }

            Text textBasicInform = m_trContentCostume.Find("TextBasicInform").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBasicInform);

            if (csCostume == null)
            {
                textBasicInform.text = "";
            }
            else
            {
                textBasicInform.text = csItem.Description;
            }

            m_trContentCostume.gameObject.SetActive(true);
        }
        else if (csItem.ItemType.EnItemType == EnItemType.CostumeEffect)
        {
            Text textItemType = m_trBaseInfoCostumeEffect.Find("TextType").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemType);
            textItemType.text = csItem.ItemType.MainCategory.Name;

            Text textRequiredLevel = m_trBaseInfoCostumeEffect.Find("TextRequiredLevel").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRequiredLevel);

            if (csItem.RequiredMinHeroLevel == 0)
            {
                textRequiredLevel.gameObject.SetActive(false);
            }
            else
            {
                textRequiredLevel.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_REQ_LEVEL"), csItem.RequiredMinHeroLevel);
                textRequiredLevel.gameObject.SetActive(true);
            }

            Text textDescription = m_trBaseInfoCostumeEffect.Find("TextDescription").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDescription);
            textDescription.text = csItem.Description;

            m_trBaseInfoCostumeEffect.gameObject.SetActive(true);

            Text textEffectInform = m_trContentCostumeEffect.Find("TextEffectInform").GetComponent<Text>();
            CsUIData.Instance.SetFont(textEffectInform);

            textEffectInform.text = csItem.Description;

            m_trContentCostumeEffect.gameObject.SetActive(true);
        }
        else
        {
            Text textItemType = m_trBaseInfoItem.Find("TextType").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemType);
            textItemType.text = csItem.ItemType.MainCategory.Name;

            Text textItemUsage = m_trBaseInfoItem.Find("TextUsage").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemUsage);
            textItemUsage.text = csItem.ItemType.SubCategory.Name;

            Transform trDescription = m_trBaseInfoItem.Find("Description");

            Text textRequireLevel = trDescription.Find("TextRequireLevel").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRequireLevel);

            if (csItem.RequiredMinHeroLevel == 0)
            {
                textRequireLevel.gameObject.SetActive(false);
            }
            else
            {
                textRequireLevel.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_REQ_LEVEL"), csItem.RequiredMinHeroLevel);
                textRequireLevel.gameObject.SetActive(true);
            }

            Text textItemDesc = trDescription.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemDesc);

            switch (csItem.ItemType.EnItemType)
            {
                case EnItemType.ExpPotion:
                    textItemDesc.text = csItem.Description + "\n" + string.Format(CsConfiguration.Instance.GetString("A02_TXT_00031"), (CsGameData.Instance.MyHeroInfo.VipLevel.ExpPotionUseMaxCount - CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount));
                    break;

                case EnItemType.ExpScroll:
                    textItemDesc.text = csItem.Description + "\n" + string.Format(CsConfiguration.Instance.GetString("A02_TXT_00031"), (CsGameData.Instance.MyHeroInfo.VipLevel.ExpScrollUseMaxCount - CsGameData.Instance.MyHeroInfo.ExpScrollDailyUseCount));
                    break;

                case EnItemType.DistortionScroll:
                    textItemDesc.text = csItem.Description + "\n" + string.Format(CsConfiguration.Instance.GetString("A02_TXT_00031"), (CsGameData.Instance.MyHeroInfo.VipLevel.DistortionScrollUseMaxCount - CsGameData.Instance.MyHeroInfo.DistortionScrollDailyUseCount));
                    break;

                case EnItemType.FishingBait:
                    textItemDesc.text = csItem.Description + "\n" + string.Format(CsConfiguration.Instance.GetString("A02_TXT_00031"), (CsGameData.Instance.FishingQuest.LimitCount - CsFishingQuestManager.Instance.FishingQuestDailyStartCount));
                    break;

                case EnItemType.BountyHunter:
                    textItemDesc.text = csItem.Description + "\n" + string.Format(CsConfiguration.Instance.GetString("A02_TXT_00031"), (CsGameConfig.Instance.BountyHunterQuestMaxCount - CsBountyHunterQuestManager.Instance.BountyHunterQuestDailyStartCount));
                    break;

                case EnItemType.Title:
                    CsTitle csTitle = CsGameData.Instance.GetTitle(csItem.Value1);

                    if (csTitle == null)
                    {
                        textItemDesc.text = "";
                    }
                    else
                    {
                        textItemDesc.text = string.Format(CsConfiguration.Instance.GetString(csItem.Description), csTitle.TitleActiveAttrList[0].Attr.Name, csTitle.TitleActiveAttrList[0].AttrValue.Value.ToString("#,##0"),
                                                                                                                  csTitle.TitleActiveAttrList[1].Attr.Name, csTitle.TitleActiveAttrList[1].AttrValue.Value.ToString("#,##0"),
                                                                                                                  csTitle.TitlePassiveAttrList[0].Attr.Name, csTitle.TitlePassiveAttrList[0].AttrValue.Value.ToString("#,##0"),
                                                                                                                  csTitle.TitlePassiveAttrList[1].Attr.Name, csTitle.TitlePassiveAttrList[1].AttrValue.Value.ToString("#,##0"));
                    }

                    break;

				case EnItemType.StarEssense:
					textItemDesc.text = csItem.Description + "\n" + string.Format(CsConfiguration.Instance.GetString("A02_TXT_00031"), (csItem.Value2 - CsConstellationManager.Instance.DailyStarEssenseItemUseCount));
					break;

                default:
                    textItemDesc.text = csItem.Description;
                    break;
            }

            m_trBaseInfoItem.gameObject.SetActive(true);

            //아이템 리스트
            Text textContentItemDesc = m_trContentItem.Find("TextDesc").GetComponent<Text>();
            CsUIData.Instance.SetFont(textContentItemDesc);
            textContentItemDesc.gameObject.SetActive(false);

            //획득가기 리스트
            Transform trShortCutList = m_trContentItem.Find("ShortCutList");
            //차후 버튼 키고 끄는것에 따라가게 변경
            trShortCutList.gameObject.SetActive(false);

            Transform trItemShortCutName = m_trContentItem.Find("TextName");
            trItemShortCutName.gameObject.SetActive(false);
            //데이터 없음

            m_trContentItem.gameObject.SetActive(true);
        }

        m_textMultyUse.text = CsConfiguration.Instance.GetString("A02_BTN_00038");

        switch (csItem.UsingType)
        {
            case EnUsingType.NotAvailable:
                m_buttonUse.gameObject.SetActive(false);
                m_buttonMultyUse.gameObject.SetActive(false);
                m_trButtonBack.gameObject.SetActive(false);
                break;

            case EnUsingType.OnlyOne:
                m_buttonMultyUse.gameObject.SetActive(false);
                m_buttonUse.gameObject.SetActive(m_bButtonOn);
                m_trButtonBack.gameObject.SetActive(true);

                if (m_bButtonOn)
                {
                    if (csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && csItem.RequiredMaxHeroLevel >= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, true);
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, false);
                    }
                }

                break;

            case EnUsingType.Multiple:
                m_buttonMultyUse.gameObject.SetActive(m_bButtonOn);
                m_buttonUse.gameObject.SetActive(m_bButtonOn);
                m_trButtonBack.gameObject.SetActive(true);

                if (m_bButtonOn)
                {
                    if (csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && csItem.RequiredMaxHeroLevel >= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, true);
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonMultyUse, true);
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonUse, false);
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonMultyUse, false);
                    }
                }

                break;
        }

        m_textUse.text = CsConfiguration.Instance.GetString("A02_BTN_00036");

        ButtonSwitching();
    }

	#endregion Item

    #region Costume

    void UpdateCostume(CsHeroCostume csHeroCostume)
    {
        m_csHeroCostume = csHeroCostume;

        m_trButtonBack.gameObject.SetActive(false);

        m_textItemName.text = csHeroCostume.Costume.Name;

        CsItem csItem = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume && a.Value1 == csHeroCostume.Costume.CostumeId);

        if (csItem == null)
        {
            m_trItemSlot.gameObject.SetActive(false);
        }
        else
        {
            CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csItem, true, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
        }

        m_trBaseInfoWing.gameObject.SetActive(false);
        m_trBaseInfoMainGear.gameObject.SetActive(false);
        m_trBaseInfoSubGear.gameObject.SetActive(false);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(false);
        m_trBaseInfoCostume.gameObject.SetActive(true);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        Text textItemType = m_trBaseInfoCostume.Find("TextType").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemType);
        textItemType.text = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume).ItemType.MainCategory.Name;

        Text textPeriodLimitDayType = m_trBaseInfoCostume.Find("TextPeriodLimitDayType").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPeriodLimitDayType);

        m_textCostumeTimer = m_trBaseInfoCostume.Find("TextPeriodLimitDay").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCostumeTimer);

        if (csHeroCostume.Costume.PeriodLimitDay == 0)
        {
            // 영구
            textPeriodLimitDayType.text = CsConfiguration.Instance.GetString("A151_TXT_00006");

            m_textCostumeTimer.gameObject.SetActive(false);
        }
        else
        {
            // 기간제
            textPeriodLimitDayType.text = CsConfiguration.Instance.GetString("A151_TXT_00015");

            TimeSpan tsRemainingTimer = TimeSpan.FromSeconds(csHeroCostume.RemainingTime - Time.realtimeSinceStartup);

            if (86400 <= tsRemainingTimer.TotalSeconds)
            {
                // 일, 시
                m_textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00007"), tsRemainingTimer.Days, tsRemainingTimer.Hours);
            }
            else if (3600 <= tsRemainingTimer.TotalSeconds)
            {
                // 시, 분
                m_textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00008"), tsRemainingTimer.Hours, tsRemainingTimer.Minutes);
            }
            else
            {
                // 분, 초
                m_textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00009"), tsRemainingTimer.Minutes, tsRemainingTimer.Seconds);
            }

            m_textCostumeTimer.gameObject.SetActive(true);
        }

        Text textRequiredLevel = m_trBaseInfoCostume.Find("TextRequiredLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRequiredLevel);
        textRequiredLevel.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_REQ_LEVEL"), csHeroCostume.Costume.RequiredHeroLevel);

        m_trMainGearShortCutList.gameObject.SetActive(false);
        m_trMountGearShortCoutList.gameObject.SetActive(false);
        m_trSubGearShortCutList.gameObject.SetActive(false);
        m_trItemShortCutList.gameObject.SetActive(false);
        m_trWingShortCutList.gameObject.SetActive(false);

        m_trContentMainGear.gameObject.SetActive(false);
        m_trContentSubGear.gameObject.SetActive(false);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(false);
        m_trContentCard.gameObject.SetActive(false);
        m_trContentCostume.gameObject.SetActive(true);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        Text textBasicInform = m_trContentCostume.Find("TextBasicInform").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBasicInform);

        Transform trBaseOption = m_trContentCostume.Find("BaseOption"); 
        trBaseOption.gameObject.SetActive(true);

        Text textBaseOption = trBaseOption.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBaseOption);
        textBaseOption.text = CsConfiguration.Instance.GetString("A151_TXT_00033");

        Transform trBaseOptionList = trBaseOption.Find("BaseOptionList");

        Transform trAttr = null;
        Text textAttr = null;
        Text textAttrValue = null;

        if (m_goOption == null)
        {
            m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
        }

        List<CsCostumeEnchantLevelAttr> listCsCostumeEnchantLevelAttr = new List<CsCostumeEnchantLevelAttr>(CsGameData.Instance.CostumeEnchantLevelAttrList).FindAll(a => a.EnchantLevel == csHeroCostume.EnchantLevel && a.CostumeId == csHeroCostume.Costume.CostumeId);

        for (int i = 0; i < listCsCostumeEnchantLevelAttr.Count; i++)
        {
            trAttr = trBaseOptionList.Find("Option" + i);

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goOption, trBaseOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textAttr = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttr);

                textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);

                textAttr = trAttr.GetComponent<Text>();
                textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
            }

            textAttr.text = listCsCostumeEnchantLevelAttr[i].Attr.Name;
            textAttrValue.text = listCsCostumeEnchantLevelAttr[i].AttrValue.Value.ToString("#,##0");
        }

        textBasicInform.text = csHeroCostume.Costume.Description;

        m_buttonUse.gameObject.SetActive(false);
        m_buttonMultyUse.gameObject.SetActive(false);
    }

    #endregion Costume

    #region Wing

    //---------------------------------------------------------------------------------------------------
    void UpdateWing(int nWingId, int nWingLevel, int nWingStep, List<CsWing> listWing, List<CsHeroWingPart> listWingPart)
    {
        m_trButtonBack.gameObject.SetActive(false);

        CsWing csWingEquipped = CsGameData.Instance.GetWing(nWingId);
        m_textItemName.text = csWingEquipped.Name;

        for (int i = 0; i < m_trItemSlot.childCount; i++)
        {
            m_trItemSlot.GetChild(i).gameObject.SetActive(false);
        }

        //아이템아이콘
        Image imageItemIcon = m_trItemSlot.Find("ImageIcon").GetComponent<Image>();
        imageItemIcon.gameObject.SetActive(true);
        imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/wing_" + nWingId);

        //랭크테두리
        Image imageFrameRank = m_trItemSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.gameObject.SetActive(true);
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank01");

        //메인기어 기본정보
        m_trBaseInfoWing.gameObject.SetActive(true);
        m_trBaseInfoMainGear.gameObject.SetActive(false);
        m_trBaseInfoSubGear.gameObject.SetActive(false);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(false);
        m_trBaseInfoCostume.gameObject.SetActive(false);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        Text textGearGrade = m_trBaseInfoWing.Find("TextGrade").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearGrade);
        CsWingStep csWingStep = CsGameData.Instance.GetWingStep(nWingStep);
        textGearGrade.text = string.Format(CsConfiguration.Instance.GetString("A22_TXT_01003"), csWingStep.ColorCode, nWingLevel, csWingStep.Name);

        Text textGearBattlePower = m_trBaseInfoWing.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearBattlePower);

        int nBattlePower = 0;

        for (int i = 0; i < listWingPart.Count; i++)
        {
            nBattlePower += listWingPart[i].GetBattlePower();
        }

        for (int i = 0; i < listWing.Count; i++)
        {
            CsWing csWing = listWing[i];
            nBattlePower += csWing.BattlePower;
        }

        textGearBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), nBattlePower.ToString("#,###"));

        //숏컷 세팅
        m_trWingShortCutList.gameObject.SetActive(m_bButtonOn);
        //m_textItemShortCutName.gameObject.SetActive(m_bButtonOn);

        if (m_bButtonOn)
        {
            for (int i = 0; i < m_trWingShortCutList.childCount; i++)
            {
                int nButtonIndex = i;

                Button buttonGearShortCut = m_trWingShortCutList.Find("Button" + i).GetComponent<Button>();
                buttonGearShortCut.onClick.RemoveAllListeners();
                buttonGearShortCut.onClick.AddListener(() => OnClickWingShortCut(nButtonIndex));
                buttonGearShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textWingShortCut = buttonGearShortCut.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textWingShortCut);

                switch ((EnWingShortCutType)nButtonIndex)
                {
                    case EnWingShortCutType.Enchant:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.WingEnchant))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }

                        textWingShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00040");
                        break;

                    case EnWingShortCutType.Appearance:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.WingEquip))
                        {
                            buttonGearShortCut.gameObject.SetActive(true);
                        }
                        else
                        {
                            buttonGearShortCut.gameObject.SetActive(false);
                        }
                        textWingShortCut.text = CsConfiguration.Instance.GetString("A02_BTN_00041");
                        break;
                }
            }
        }

        //숏컷버튼리스트.
        m_trMainGearShortCutList.gameObject.SetActive(false);
        m_trMountGearShortCoutList.gameObject.SetActive(false);
        m_trSubGearShortCutList.gameObject.SetActive(false);
        m_trItemShortCutList.gameObject.SetActive(false);


        //옵션
        m_trContentMainGear.gameObject.SetActive(false);
        m_trContentSubGear.gameObject.SetActive(false);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(true);
        m_trContentCard.gameObject.SetActive(false);
        m_trContentCostume.gameObject.SetActive(false);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        //강화속성
        Transform trBaseOption = m_trContentWing.Find("BaseOption");

        Text textBaseOptionName = trBaseOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBaseOptionName);
        textBaseOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00029");

        Transform trBaseOptionList = m_trContentWing.Find("BaseOption/BaseOptionList");

        for (int i = 0; i < listWingPart.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + i);
            Text textOptionName;
            Text textOptionValue;

            if (m_goBaseOption == null)
            {
                m_goBaseOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextBaseOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goBaseOption, trBaseOptionList);
                goOption.name = "Option" + i;
                trAttr = goOption.transform;

                textOptionName = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);

                textOptionName = trAttr.GetComponent<Text>();
                textOptionValue = textOptionName.transform.Find("TextValue").GetComponent<Text>();
            }

            CsWingPart csWingPart = CsGameData.Instance.GetWingPart(listWingPart[i].PartId);
            textOptionName.text = csWingPart.Attr.Name;

            int nValue = 0;

            for (int j = 0; j < listWingPart[i].HeroWingEnchantList.Count; j++)
            {
                CsWingStep csWingStepAll = CsGameData.Instance.GetWingStep(listWingPart[i].HeroWingEnchantList[j].Step);
                CsWingStepPart csWingStepPart = csWingStepAll.GetWingStepPart(listWingPart[i].PartId);
                nValue += listWingPart[i].HeroWingEnchantList[j].EnchantCount * csWingStepPart.IncreaseAttrValueInfo.Value;
            }

            textOptionValue.text = nValue.ToString("#,##0");
        }

        for (int i = 0; i < trBaseOptionList.childCount - listWingPart.Count; i++)
        {
            Transform trAttr = trBaseOptionList.Find("Option" + (i + listWingPart.Count));
            trAttr.gameObject.SetActive(false);
        }

        // 날개속성

        Transform trAddOption = m_trContentWing.Find("AddOption");

        Text textAddOptionName = trAddOption.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAddOptionName);
        textAddOptionName.text = CsConfiguration.Instance.GetString("A02_TXT_00013");
        Transform trAddOptionList = m_trContentWing.Find("AddOption/AddOptionList");

        Dictionary<int, int> dicWingAttr = new Dictionary<int, int>();
        dicWingAttr.Clear();

        for (int i = 0; i < listWing.Count; i++)
        {
            CsWing csWing = listWing[i];

            if (dicWingAttr.ContainsKey(csWing.Attr.AttrId))
            {
                dicWingAttr[csWing.Attr.AttrId] += csWing.AttrValueInfo.Value;
            }
            else
            {
                dicWingAttr.Add(csWing.Attr.AttrId, csWing.AttrValueInfo.Value);
            }
        }

        int nAttrCount = 0;

        foreach (KeyValuePair<int, int> kv in dicWingAttr)
        {
            Transform trAttr = trAddOptionList.Find("Option" + nAttrCount);

            Text textOptionName;
            Text textValue;

            if (m_goOption == null)
            {
                m_goOption = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/TextOptionName");
            }

            if (trAttr == null)
            {
                GameObject goOption = Instantiate(m_goOption, trAddOptionList);
                goOption.name = "Option" + nAttrCount;
                trAttr = goOption.transform;

                textOptionName = trAttr.GetComponent<Text>();
                CsUIData.Instance.SetFont(textOptionName);

                textValue = trAttr.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textValue);
            }
            else
            {
                trAttr.gameObject.SetActive(true);
                textOptionName = trAttr.GetComponent<Text>();
                textValue = trAttr.Find("TextValue").GetComponent<Text>();
            }

            CsAttr csAttr = CsGameData.Instance.GetAttr(kv.Key);

            textOptionName.text = csAttr.Name;
            textValue.text = kv.Value.ToString("#,##0");

            nAttrCount++;
        }

        for (int i = 0; i < trAddOptionList.childCount - dicWingAttr.Count; i++)
        {
            Transform trAttr = trAddOptionList.Find("Option" + (i + dicWingAttr.Count));
            trAttr.gameObject.SetActive(false);
        }

        //버튼
        m_buttonUse.gameObject.SetActive(false);
        m_buttonMultyUse.gameObject.SetActive(false);
    }

    #endregion Wing

    #region CreatureCard

    //---------------------------------------------------------------------------------------------------
    void UpdateCreatureCard(CsCreatureCard csCreatureCard)
    {
        m_rtScrollView.sizeDelta = new Vector2(m_rtScrollView.sizeDelta.x, 316f);
        m_trButtonBack.gameObject.SetActive(false);

        m_textItemName.text = string.Format("<color={0}>{1}</color>", csCreatureCard.CreatureCardGrade.ColorCode, csCreatureCard.Name);

        CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csCreatureCard, false, EnItemSlotSize.Large);

        //카드 기본정보
        m_trBaseInfoMainGear.gameObject.SetActive(false);
        m_trBaseInfoSubGear.gameObject.SetActive(false);
        m_trBaseInfoItem.gameObject.SetActive(false);
        m_trBaseInfoWing.gameObject.SetActive(false);
        m_trBaseInfoCard.gameObject.SetActive(true);
        m_trBaseInfoCostume.gameObject.SetActive(false);
        m_trBaseInfoCostumeEffect.gameObject.SetActive(false);

        Text textItemCount = m_trBaseInfoCard.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemCount);
        CsHeroCreatureCard csHeroCreatureCard = CsCreatureCardManager.Instance.GetHeroCreatureCard(csCreatureCard.CreatureCardId);

        if (csHeroCreatureCard != null)
        {
            textItemCount.text = string.Format(CsConfiguration.Instance.GetString("A89_TXT_01001"), csHeroCreatureCard.Count.ToString("#,##0"));
        }
        else
        {
            textItemCount.text = string.Format(CsConfiguration.Instance.GetString("A89_TXT_01001"), 0);
        }

        Text textItemType = m_trBaseInfoCard.Find("TextType").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemType);
        textItemType.text = csCreatureCard.CreatureCardCategory.Name;

        Text textItemCollection = m_trBaseInfoCard.Find("TextCollection").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemCollection);

        GameObject goChollection = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupItemInfo/CardCollection");
        List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCreatureCard(csCreatureCard.CreatureCardId);
        int nCount = 0;

        for (int i = 0; i < listEntry.Count; i++)
        {
            Text textColleciton = Instantiate(goChollection, m_trContentCard).GetComponent<Text>();
            textColleciton.text = listEntry[i].CreatureCardCollection.Name;
            CsUIData.Instance.SetFont(textColleciton);

            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listEntry[i].CreatureCardCollection.CollectionId))
            {
                textColleciton.transform.Find("Image").gameObject.SetActive(true);
                nCount++;
            }
        }

        textItemCollection.text = string.Format(CsConfiguration.Instance.GetString("A89_TXT_01002"), nCount, listEntry.Count);
        m_trItemShortCutList.gameObject.SetActive(false);

        //숏컷버튼리스트 차후 버튼 키고 끄는것에 따라가게 변경
        m_trMainGearShortCutList.gameObject.SetActive(false);
        m_trSubGearShortCutList.gameObject.SetActive(false);
        m_trMountGearShortCoutList.gameObject.SetActive(false);
        m_trWingShortCutList.gameObject.SetActive(false);

        //옵션
        m_trContentMainGear.gameObject.SetActive(false);
        m_trContentSubGear.gameObject.SetActive(false);
        m_trContentWing.gameObject.SetActive(false);
        m_trContentItem.gameObject.SetActive(false);
        m_trContentCard.gameObject.SetActive(true);
        m_trContentCostume.gameObject.SetActive(false);
        m_trContentCostumeEffect.gameObject.SetActive(false);

        //아이템 리스트
        Text textContentItemDesc = m_trContentItem.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContentItemDesc);
        textContentItemDesc.gameObject.SetActive(false);

        //획득가기 리스트
        Transform trShortCutList = m_trContentItem.Find("ShortCutList");
        //차후 버튼 키고 끄는것에 따라가게 변경
        trShortCutList.gameObject.SetActive(false);

        Transform trItemShortCutName = m_trContentItem.Find("TextName");
        trItemShortCutName.gameObject.SetActive(false);
    }


    #endregion CreatureCard

    #region 다중 아이템사용 카운트 설정 팝업

    //---------------------------------------------------------------------------------------------------
    //아이템 다중사용 개수
    void OpenPopupSelectUseCount()
    {
        if (m_goPopupSelectUseCount == null)
        {
            StartCoroutine(LoadPopupSelectUseCountCoroutine());
        }
        else
        {
            InitializePopupSelectUseCount();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSelectUseCountCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupSelectUseCount");
        yield return resourceRequest;
        m_goPopupSelectUseCount = (GameObject)resourceRequest.asset;

        InitializePopupSelectUseCount();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupSelectUseCount()
    {
        GameObject go_PopupSelectUseCount = Instantiate(m_goPopupSelectUseCount, m_trPopupList);
        go_PopupSelectUseCount.name = "PopupSelectUseCount";

        m_trPopupSelectUseCount = go_PopupSelectUseCount.transform;

        Transform trBack = m_trPopupSelectUseCount.Find("ImageBackground");

        Transform trItemSlot = trBack.Find("ItemSlot");
        CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nInventoryListIndex];
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventorySlot.InventoryObjectItem);

        Text textPopupName = trBack.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A02_BTN_00038");

        Text textItemName = trBack.Find("TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemName);
        textItemName.text = m_csItemSelect.Name;

        Text textUseCount = trBack.Find("TextUseCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textUseCount);
        textUseCount.text = CsConfiguration.Instance.GetString("A02_TXT_00025");

        m_textUseCountValue = trBack.Find("TextUseCountValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textUseCountValue);
        m_textUseCountValue.text = m_nMaxUseCount.ToString("#,##0");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopupSelectUseCount);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonPlus = trBack.Find("ButtonPlus").GetComponent<Button>();
        buttonPlus.onClick.RemoveAllListeners();
        buttonPlus.onClick.AddListener(OnClickPlusCount);
        buttonPlus.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonMinus = trBack.Find("ButtonMinus").GetComponent<Button>();
        buttonMinus.onClick.RemoveAllListeners();
        buttonMinus.onClick.AddListener(OnClickMinusCount);
        buttonMinus.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonSelectUseCount = trBack.Find("ButtonMultyUse").GetComponent<Button>();
        m_buttonSelectUseCount.onClick.RemoveAllListeners();
        m_buttonSelectUseCount.onClick.AddListener(OnClickItemMultyUseSelectCount);
        m_buttonSelectUseCount.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textSelectUseCount = m_buttonSelectUseCount.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSelectUseCount);
        textSelectUseCount.text = CsConfiguration.Instance.GetString("A02_BTN_00038");

        m_slider = trBack.Find("Slider").GetComponent<Slider>();
        m_slider.maxValue = m_nMaxUseCount - 1;
        m_slider.value = m_slider.maxValue;
        m_slider.onValueChanged.RemoveAllListeners();
        m_slider.onValueChanged.AddListener((ison) => OnValueChangedSelectCount());

        m_nSelectUseCount = (int)m_slider.value + 1;
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSelectCount()
    {
        m_nSelectUseCount = (int)m_slider.value + 1;
        m_textUseCountValue.text = m_nSelectUseCount.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPlusCount()
    {
        if (m_slider.value < m_nMaxUseCount - 1)
        {
            m_slider.value++;
            m_nSelectUseCount = (int)m_slider.value + 1;
            m_textUseCountValue.text = m_nSelectUseCount.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMinusCount()
    {
        if (m_slider.value > 0)
        {
            m_slider.value--;
            m_nSelectUseCount = (int)m_slider.value + 1;
            m_textUseCountValue.text = m_nSelectUseCount.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupSelectUseCount()
    {
        m_nMaxUseCount = 0;
        m_nSelectUseCount = 0;

        if (m_trPopupSelectUseCount != null)
        {
            Destroy(m_trPopupSelectUseCount.gameObject);
        }

        m_trPopupSelectUseCount = null;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemMultyUseSelectCount()
    {
        Debug.Log("UseCount : " + m_nSelectUseCount);

        if (m_nSelectUseCount > 0 && m_nSelectUseCount <= m_nMaxUseCount)
        {
            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nInventoryListIndex];

            switch (m_csItemSelect.ItemType.EnItemType)
            {
                case EnItemType.MainGearBox:
                case EnItemType.PickBox:
				    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, m_nSelectUseCount);
                    ClosePopupItemInfo();
                    break;

                case EnItemType.ExpPotion:
                    if (CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount < CsGameData.Instance.MyHeroInfo.VipLevel.ExpPotionUseMaxCount)
                    {
                        CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, m_nSelectUseCount);
                        ClosePopupItemInfo();
                    }
                    break;

                case EnItemType.Gold:
                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, m_nSelectUseCount);
                    ClosePopupItemInfo();
                    break;

                case EnItemType.OwnDia:
                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, m_nSelectUseCount);
                    ClosePopupItemInfo();
                    break;

                case EnItemType.HonorPoint:
                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, m_nSelectUseCount);
                    ClosePopupItemInfo();
                    break;

                case EnItemType.ExploitPoint:
                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index, m_nSelectUseCount);
                    ClosePopupItemInfo();
                    break;
            }
        }
    }

    #endregion 다중 아이템사용 카운트 설정 팝업

    void ButtonSwitching()
    {
        switch (m_enItemLocationType)
        {
            case EnItemLocationType.None:
                //m_trButtonBack.gameObject.SetActive(false);
                m_buttonDeposit.gameObject.SetActive(false);
                m_buttonWithdraw.gameObject.SetActive(false);
                break;
            case EnItemLocationType.Inventory:
                m_trButtonBack.gameObject.SetActive(true);
                m_buttonDeposit.gameObject.SetActive(true);
                m_buttonWithdraw.gameObject.SetActive(false);
                break;
            case EnItemLocationType.Warehouse:
                m_trButtonBack.gameObject.SetActive(true);
                m_buttonDeposit.gameObject.SetActive(false);
                m_buttonWithdraw.gameObject.SetActive(true);
                break;
        }
    }
}
