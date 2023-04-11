using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-24)
//---------------------------------------------------------------------------------------------------

public enum EnDisassembleInputType
{
    High = 0,
    Magic = 1,
    Rare = 2,
    Legend = 3,
}

public enum EnOpenedInventoryPopup
{
    None = 0,
    SimpleShopSell = 1,
    Compose = 2,
    DeCompose = 3,
}

public enum EnInventoryTab
{
    All = 0,
    Gear = 1,
    Item = 2
}

public class CsInventory : CsPopupSub
{
    [SerializeField] GameObject m_goItemSlot;
    [SerializeField] GameObject m_goInventorySlot;
    Transform m_trBack;
    Transform m_trContent;
    Transform m_trPopupList;
    Transform m_trItemInfo;
    Transform m_trCalculator;
    Transform m_trInventoryShop;
    Transform m_trPopupExtend;
    Transform m_trPopupDisassemble;
    Transform m_trCompose;
    Transform m_trBotFrame;
    Transform m_trBotDeposit;

    GameObject m_goCompose;
    GameObject m_goShop;
    GameObject m_goShopProduct;
    GameObject m_goPopupItemInfo;
    GameObject m_goPopupCalculator;
    GameObject m_goExtendPopup;
    GameObject m_goDisassemble;

    CsPopupCalculator m_csPopupCalculator;
    CsSimpleShopProduct m_csSimpleShopProductSeleced;
    CsPopupItemInfo m_csPopupItemInfo;
    CsItemCompositionRecipe m_csItemCompositionRecipe;

    EnInventoryTab m_enInventoryCategoryNow = EnInventoryTab.All;
    EnDisassembleInputType m_enDisassembleInputTypeNow = EnDisassembleInputType.Legend;
    EnOpenedInventoryPopup m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.None;
    EnSubMenu m_enSubMenu;

    Text m_textGetGoldValue;
    Text m_textInventoryCount;
    Text m_textMaterialItemName;
    Text m_textResultItemName;
    Text m_textComposeGold;
    Text m_textAllComposeGold;

    Button m_buttonSell;
    Button m_buttonDisassemble;
    Button m_buttonCompose;
    Button m_buttonAllCompose;
    Button m_buttonSelectMode;
    Button m_buttonShop;
    Button m_buttonCancel;
    Button m_buttonDeposit;

    int m_nLoadCompliteSlotCount;
    int m_nstandardPosition = 0;
    int m_nStartDiaInventoryIndex;
    int m_nExtendNeedDia;
    int m_nExtendSlotCount;
    int m_nSelectSlotIndex = -1;
    int m_nSelectInventoryListIndex = -1;
    int m_nLastSelectComposeItem = -1;

    bool m_bIsLoad = false;
    bool m_bFirst = true;
    bool m_bProcessingButton = false;
    bool m_bSelectMode = false;
    bool m_bPrefabLoad = false;

    int?[] m_arraySellItem = new int?[10];
    int?[] m_arrayDisassemble = new int?[10];

    List<CsDisassembleResultItem> m_listCsDisassembleResultItem = new List<CsDisassembleResultItem>();
    List<int> m_listPotionSlot = new List<int>();
    List<int> m_listReturnScrollSlot = new List<int>();
    List<int> m_listSelectItemIndex = new List<int>();
    //List<int> m_listExpScrollSlot = new List<int>();

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        CsGameEventUIToUI.Instance.EventMainGearEquip += OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip += OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip += OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip += OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventSimpleShopBuy += OnEventSimpleShopBuy;
        CsGameEventUIToUI.Instance.EventSimpleShopSell += OnEventSimpleShopSell;

        CsGameEventUIToUI.Instance.EventInventorySlotExtend += OnEventInventorySlotExtend;

        CsGameEventUIToUI.Instance.EventMainGearDisassemble += OnEventMainGearDisassemble;

        CsGameEventUIToUI.Instance.EventItemCompose += OnEventItemCompose;
        CsGameEventUIToUI.Instance.EventItemComposeTotally += OnEventItemComposeTotally;

        CsGameEventUIToUI.Instance.EventHpPotionUse += OnEventHpPotionUse;
        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished += OnEventReturnScrollUseFinished;

        CsGameEventUIToUI.Instance.EventPickBoxUse += OnEventPickBoxUse;
        CsGameEventUIToUI.Instance.EventMainGearBoxUse += OnEventMainGearBoxUse;

        CsGameEventUIToUI.Instance.EventDropObjectLooted += OnEventDropObjectLooted;

        CsGameEventUIToUI.Instance.EventExpPotionUse += OnEventExpPotionUse;

        CsGameEventUIToUI.Instance.EventExpAcquisition += OnEventExpAcquisition;

        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;

        CsGameEventUIToUI.Instance.EventMountGearEquip += OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip += OnEventMountGearUnequip;

        CsGameEventUIToUI.Instance.EventExpScrollUse += OnEventExpScrollUse;

        //던전보상
        CsDungeonManager.Instance.EventStoryDungeonClear += OnEventStoryDungeonClear;
		CsDungeonManager.Instance.EventRuinsReclaimStepCompleted += OnEventRuinsReclaimStepCompleted;
		CsDungeonManager.Instance.EventRuinsReclaimRewardObjectInteractionFinished += OnEventRuinsReclaimRewardObjectInteractionFinished;
		
		CsGameEventUIToUI.Instance.EventFishingBaitUse += OnEventFishingBaitUse;
        CsGameEventUIToUI.Instance.EventBountyHunterQuestScrollUse += OnEventBountyHunterQuestScrollUse;

        CsGameEventUIToUI.Instance.EventDistortionScrollUse += OnEventDistortionScrollUse;
        CsGuildManager.Instance.EventGuildCall += OnEventGuildCall;

        CsGameEventUIToUI.Instance.EventNationCall += OnEventNationCall;
        CsTitleManager.Instance.EventTitleItemUse += OnEventTitleItemUse;

        CsIllustratedBookManager.Instance.EventIllustratedBookUse += OnEventIllustratedBookUse;
        CsIllustratedBookManager.Instance.EventSceneryQuestCompleted += OnEventSceneryQuestCompleted;

        CsGameEventUIToUI.Instance.EventGoldItemUse += OnEventGoldItemUse;
        CsGameEventUIToUI.Instance.EventOwnDiaItemUse += OnEventOwnDiaItemUse;
        CsGameEventUIToUI.Instance.EventHonorPointItemUse += OnEventHonorPointItemUse;
        CsGameEventUIToUI.Instance.EventExploitPointItemUse += OnEventExploitPointItemUse;
        CsGameEventUIToUI.Instance.EventWarehouseDeposit += OnEventWarehouseDeposit;
        CsGameEventUIToUI.Instance.EventWarehouseWithdraw += OnEventWarehouseWithdraw;

		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted += OnEventTrueHeroQuestStepCompleted;
		CsBiographyManager.Instance.EventBiographyStart += OnEventBiographyStart;

        CsCostumeManager.Instance.EventCostumeItemUse += OnEventCostumeItemUse;
        CsCostumeManager.Instance.EventCostumeEffectApply += OnEventCostumeEffectApply;

		CsCreatureManager.Instance.EventCreatureEggUse += OnEventCreatureEggUse;
        CsGameEventUIToUI.Instance.EventWingItemUse += OnEventWingItemUse;

		CsConstellationManager.Instance.EventStarEssenseItemUse += OnEventStarEssenseItemUse;
		CsConstellationManager.Instance.EventPremiumStarEssenseItemUse += OnEventPremiumStarEssenseItemUse;

        CsGameEventUIToUI.Instance.EventMountItemUse += OnEventMountItemUse;

		CsGameEventUIToUI.Instance.EventSpiritStoneItemUse += OnEventSpiritStoneItemUse;

        CsAccomplishmentManager.Instance.EventAccomplishmentPointItemUse += OnEventAccomplishmentPointItemUse;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (CsUIData.Instance.HpPotionRemainingCoolTime - Time.deltaTime > 0)
        {
            for (int i = 0; i < m_listPotionSlot.Count; i++)
            {
                Transform trInventorySlot = m_trContent.Find("InventorySlot" + m_listPotionSlot[i]);

                if (trInventorySlot != null)
                {
                    Transform trItemSlot = trInventorySlot.Find("ItemSlot");

                    if (trItemSlot != null)
                    {
                        Image imageCoolTime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                        imageCoolTime.fillAmount = (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup) / CsUIData.Instance.HpPotionCoolTime;
                    }
                }
            }
        }

        if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup > 0)
        {
            for (int i = 0; i < m_listReturnScrollSlot.Count; i++)
            {
                Transform trInventorySlot = m_trContent.Find("InventorySlot" + m_listReturnScrollSlot[i]);

                if (trInventorySlot != null)
                {
                    Transform trItemSlot = trInventorySlot.Find("ItemSlot");

                    if (trItemSlot != null)
                    {
                        Image imageCoolTime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                        imageCoolTime.fillAmount = (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup) / CsUIData.Instance.ReturnScrollCoolTime;
                    }
                }
            }
        }

        ChangeBotFrame();
    }

    //---------------------------------------------------------------------------------------------------
    void ChangeBotFrame()
    {
		bool bIsWarehouse = m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.Warehouse;              // 인벤토리를 사용하고 있는 곳이 창고인지 확인

		if (bIsWarehouse)
		{
			m_trBotFrame.gameObject.SetActive(false);
			m_trBotDeposit.gameObject.SetActive(true);
		}
		else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.Inventory)
		{
			m_trBotFrame.gameObject.SetActive(true);
			m_trBotDeposit.gameObject.SetActive(false);
			if (m_bSelectMode)
			{
				m_bSelectMode = false;
				InitializeSlot();
				InitializeDepositButton(m_bSelectMode);
			}
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        m_trBack.gameObject.SetActive(true);
        OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        OnClickCloseSimpleShop();
        OnClickCloseDisassemble();
        OnClickCloseCompose();

        if (m_trItemInfo != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Right);
        }
        if (m_trCalculator != null)
        {
            OnEventCloseCalculator();
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        //if (m_trItemInfoRight != null)
        //{
        //    m_trItemInfoRight.gameObject.SetActive(false);
        //}

        CsGameEventUIToUI.Instance.EventMainGearEquip -= OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip -= OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip -= OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip -= OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventSimpleShopBuy -= OnEventSimpleShopBuy;
        CsGameEventUIToUI.Instance.EventSimpleShopSell -= OnEventSimpleShopSell;

        CsGameEventUIToUI.Instance.EventInventorySlotExtend -= OnEventInventorySlotExtend;

        CsGameEventUIToUI.Instance.EventMainGearDisassemble -= OnEventMainGearDisassemble;

        CsGameEventUIToUI.Instance.EventItemCompose -= OnEventItemCompose;
        CsGameEventUIToUI.Instance.EventItemComposeTotally -= OnEventItemComposeTotally;

        CsGameEventUIToUI.Instance.EventHpPotionUse -= OnEventHpPotionUse;
        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished -= OnEventReturnScrollUseFinished;

        CsGameEventUIToUI.Instance.EventPickBoxUse -= OnEventPickBoxUse;
        CsGameEventUIToUI.Instance.EventMainGearBoxUse -= OnEventMainGearBoxUse;

        CsGameEventUIToUI.Instance.EventDropObjectLooted -= OnEventDropObjectLooted;

        CsGameEventUIToUI.Instance.EventExpPotionUse -= OnEventExpPotionUse;

        CsGameEventUIToUI.Instance.EventExpAcquisition -= OnEventExpAcquisition;

        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;

        CsGameEventUIToUI.Instance.EventMountGearEquip -= OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip -= OnEventMountGearUnequip;

        CsGameEventUIToUI.Instance.EventExpScrollUse -= OnEventExpScrollUse;

        //던전보상
        CsDungeonManager.Instance.EventStoryDungeonClear -= OnEventStoryDungeonClear;
        CsDungeonManager.Instance.EventRuinsReclaimStepCompleted -= OnEventRuinsReclaimStepCompleted;
        CsDungeonManager.Instance.EventRuinsReclaimRewardObjectInteractionFinished -= OnEventRuinsReclaimRewardObjectInteractionFinished;

        CsGameEventUIToUI.Instance.EventFishingBaitUse -= OnEventFishingBaitUse;
        CsGameEventUIToUI.Instance.EventBountyHunterQuestScrollUse -= OnEventBountyHunterQuestScrollUse;

        CsGameEventUIToUI.Instance.EventDistortionScrollUse -= OnEventDistortionScrollUse;
        CsGuildManager.Instance.EventGuildCall -= OnEventGuildCall;

        CsGameEventUIToUI.Instance.EventNationCall -= OnEventNationCall;
        CsTitleManager.Instance.EventTitleItemUse -= OnEventTitleItemUse;

        CsIllustratedBookManager.Instance.EventIllustratedBookUse -= OnEventIllustratedBookUse;
        CsIllustratedBookManager.Instance.EventSceneryQuestCompleted -= OnEventSceneryQuestCompleted;

        CsGameEventUIToUI.Instance.EventGoldItemUse -= OnEventGoldItemUse;
        CsGameEventUIToUI.Instance.EventOwnDiaItemUse -= OnEventOwnDiaItemUse;
        CsGameEventUIToUI.Instance.EventHonorPointItemUse -= OnEventHonorPointItemUse;
        CsGameEventUIToUI.Instance.EventExploitPointItemUse -= OnEventExploitPointItemUse;
        CsGameEventUIToUI.Instance.EventWarehouseDeposit -= OnEventWarehouseDeposit;
        CsGameEventUIToUI.Instance.EventWarehouseWithdraw -= OnEventWarehouseWithdraw;

        CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted -= OnEventTrueHeroQuestStepCompleted;
		CsBiographyManager.Instance.EventBiographyStart -= OnEventBiographyStart;

        CsCostumeManager.Instance.EventCostumeItemUse -= OnEventCostumeItemUse;
        CsCostumeManager.Instance.EventCostumeEffectApply -= OnEventCostumeEffectApply;

        CsCreatureManager.Instance.EventCreatureEggUse -= OnEventCreatureEggUse;
        CsGameEventUIToUI.Instance.EventWingItemUse -= OnEventWingItemUse;

		CsConstellationManager.Instance.EventStarEssenseItemUse -= OnEventStarEssenseItemUse;
		CsConstellationManager.Instance.EventPremiumStarEssenseItemUse -= OnEventPremiumStarEssenseItemUse;

        CsGameEventUIToUI.Instance.EventMountItemUse -= OnEventMountItemUse;

		CsGameEventUIToUI.Instance.EventSpiritStoneItemUse -= OnEventSpiritStoneItemUse;

        CsAccomplishmentManager.Instance.EventAccomplishmentPointItemUse -= OnEventAccomplishmentPointItemUse;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventAccomplishmentPointItemUse()
    {
        UpdateInventoryAll();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventSpiritStoneItemUse(int nDifference)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_SPIRITSOUL"), nDifference));

		UpdateInventoryAll();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventMountItemUse()
    {
        UpdateInventoryAll();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventStarEssenseItemUse(int nDifference)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_STARTOAST"), nDifference));

		UpdateInventoryAll();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPremiumStarEssenseItemUse(int nDifference)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_STARTOAST"), nDifference));

		UpdateInventoryAll();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventWingItemUse()
    {
        UpdateInventoryAll();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureEggUse()
	{
		UpdateInventoryAll();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEffectApply(int nEffectId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeItemUse()
    {
        UpdateInventoryAll();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyStart(CsBiography csBiography)
	{
		UpdateInventoryAll();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepCompleted()
	{
		UpdateInventoryAll();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventExploitPointItemUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHonorPointItemUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOwnDiaItemUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldItemUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTitleItemUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventIllustratedBookUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneryQuestCompleted(PDItemBooty pDItemBooty, int nQuestId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationCall()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildCall()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDistortionScrollUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingBaitUse()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestScrollUse()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

   //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonClear(PDItemBooty[] pDItemBooty)
    {
        UpdateInventoryAll();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepCompleted(PDItemBooty[] aPDItemBooty)
	{
		UpdateInventoryAll();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRewardObjectInteractionFinished(PDItemBooty booty, long lInstanceId)
	{
		UpdateInventoryAll();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventExpScrollUse()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearEquip(Guid guid)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearUnequip(Guid guid)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpAcquisition(long lExp, bool bLevelUp)
    {
        if (bLevelUp)
        {
            UpdateInventoryAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        if (bLevelUp)
        {
            UpdateInventoryAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpPotionUse(bool bLevelUp, long lAcquiredExp)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDropObjectLooted(List<CsDropObject> listLooted, List<CsDropObject> listNotLooted)
    {
        if (listLooted.Count > 0)
        {
            UpdateInventoryAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPickBoxUse(bool bIsFull)
    {
        if (bIsFull)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_TXT_02006"));
        }
        else
        {
            UpdateInventoryAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearBoxUse(bool bIsFull)
    {
        if (bIsFull)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_TXT_02006"));
        }
        else
        {
            UpdateInventoryAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHpPotionUse(int nRecoveryHp)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseFinished(int nContinentId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemCompose()
    {
        int nListIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindIndex(a => a.Index == m_nSelectSlotIndex);

        if (m_nSelectInventoryListIndex != nListIndex)
        {
            UpdateInventorySlotSelected(m_nSelectInventoryListIndex, false);
            m_nSelectInventoryListIndex = nListIndex;
        }

        UpdateInventoryAll();
        UpdateComposeDisplay();

        UpdateInventorySlotSelected(m_nSelectInventoryListIndex, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemComposeTotally()
    {
        int nListIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindIndex(a => a.Index == m_nSelectSlotIndex);

        if (m_nSelectInventoryListIndex != nListIndex)
        {
            UpdateInventorySlotSelected(m_nSelectInventoryListIndex, false);
            m_nSelectInventoryListIndex = nListIndex;
        }

        UpdateInventoryAll();
        UpdateComposeDisplay();

        UpdateInventorySlotSelected(m_nSelectInventoryListIndex, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearDisassemble()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInventorySlotExtend()
    {
        UpdateInventoryCountText();
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopBuy()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopSell()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), m_textGetGoldValue.text));
        UpdateSellDisplay();
        UpdateInventoryAll();
        OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEquip(Guid guidHeroGearId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearUnequip(Guid guidHeroGearId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearEquip(int nSubGearId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearUnequip(int nSubGearId)
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarehouseWithdraw()
    {
        UpdateInventoryAll();        
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarehouseDeposit()
    {
        UpdateInventoryAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCompose()
    {
        OpenCompose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDisassemble()
    {
        OpenDisassemble();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickShop()
    {
        OpenShop();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSelectMode()
    {
        InitializeSlot();
        m_bSelectMode = true;
        InitializeDepositButton(m_bSelectMode);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDeposit()
    {
        if (m_listSelectItemIndex.Count == 0)
        {
            return;
        }
        int nWarehouseTotalCount = CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount + CsGameConfig.Instance.FreeWarehouseSlotCount;  // 현재 오픈한 창고의 총 슬롯 수(다이아로 오픈한 슬롯 수 + 기본 제공 슬롯 수)
        int nWarehouseEmptyCount = nWarehouseTotalCount - CsGameData.Instance.MyHeroInfo.WarehouseSlotList.Count;   // 현재 창고에 남은 슬롯 수(총 슬롯 수에서 사용하고 있는 슬롯 수 빼기)

        if (nWarehouseEmptyCount > 0)
        {
            // 창고에 아이템 맡기기.
            int[] anDepositList = new int[m_listSelectItemIndex.Count];
            for (int i = 0; i < m_listSelectItemIndex.Count; i++)
            {
                anDepositList[i] = m_listSelectItemIndex[i];
            }
            CsCommandEventManager.Instance.SendWarehouseDeposit(anDepositList);
        }
        else
        {
            // 창고에 빈 슬롯이 없을 경우 토스트메시지
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A113_TXT_02001"));
        }
        InitializeSlot();
        m_bSelectMode = false;
        InitializeDepositButton(m_bSelectMode);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickModeCancel()
    {
        InitializeSlot();

        m_bSelectMode = false;
        InitializeDepositButton(m_bSelectMode);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeSlot()
    {
        if (m_listSelectItemIndex.Count > 0)
        {
            for (int i = 0; i < m_listSelectItemIndex.Count; i++)
            {
                // 취소버튼 누를 경우, 저장해놨던 인덱스 번호 모두 삭제.
                UpdateInventorySlotByInventoryIndex(m_listSelectItemIndex[i], false);
            }
        }
        m_listSelectItemIndex.Clear();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeDepositButton(bool bSelectMode)
    {
        m_buttonSelectMode.gameObject.SetActive(!bSelectMode);
        m_buttonDeposit.gameObject.SetActive(bSelectMode);
        m_buttonCancel.gameObject.SetActive(bSelectMode);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;

        if (enPopupItemInfoPositionType == EnPopupItemInfoPositionType.Right)
        {
            if (m_trBack != null)
            {
                m_trBack.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickInventorySlot(int nSlotIndex)
    {
        if (m_bProcessingButton)
        {
            return;
        }

        m_bProcessingButton = true;
        int nOldSelectInventoryListIndex = m_nSelectInventoryListIndex;
        m_nSelectInventoryListIndex = nSlotIndex;

        int nInventoryLevelMaxCount = CsGameData.Instance.JobLevelMasterList[CsGameData.Instance.JobLevelMasterList.Count - 1].InventorySlotAccCount;

        bool bIsFull;

        //인벤토리가 눌렸을때
        if (nSlotIndex < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count)
        {
            switch (m_enOpenedInventoryPopupNow)
            {
                case EnOpenedInventoryPopup.None:

                    if (m_bSelectMode)
                    {
						// 선택모드일땐 체크박스 설정 및 아이템 인덱스 저장.
						Transform trCheck = m_trContent.Find("InventorySlot" + nSlotIndex + "/ItemSlot/ImageCheck");

						if (trCheck.gameObject.activeSelf)
						{
							m_listSelectItemIndex.Remove(CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index);
							UpdateInventorySlotByInventoryIndex(CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index, false, true);
						}
						else
						{
							m_listSelectItemIndex.Add(CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index);
							UpdateInventorySlotByInventoryIndex(CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index, true, true);
						}
                    }
                    else
                    {
                        //아이템정보창 보여주기
                        if (m_goPopupItemInfo == null)
                        {
                            StartCoroutine(LoadPopupItemInfo());
                        }
                        else
                        {
                            OpenPopupItemInfo();
                        }
                    }
                    break;

                case EnOpenedInventoryPopup.SimpleShopSell:

                    bIsFull = true;

                    for (int i = 0; i < m_arraySellItem.Length; i++)
                    {
                        if (m_arraySellItem[i] == null)
                        {
                            m_arraySellItem[i] = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index;
                            UpdateInventorySlotByInventoryIndex(CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index, true);
                            UpdateSellSlot(i);
                            UpdateSellDisplay();
                            bIsFull = false;
                            break;
                        }
                    }

                    if (bIsFull)
                    {
                        //토스트
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02001"));
                    }

                    break;

                case EnOpenedInventoryPopup.DeCompose:

                    bIsFull = true;

                    for (int i = 0; i < m_arrayDisassemble.Length; i++)
                    {
                        if (m_arrayDisassemble[i] == null)
                        {
                            m_arrayDisassemble[i] = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index;
                            UpdateInventorySlotByInventoryIndex(CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].Index, true);
                            UpdateDisassembleSlot(i);
                            UpdateResultSlot(i, true);
                            bIsFull = false;
                            break;
                        }
                    }

                    if (bIsFull)
                    {
                        //토스트
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02002"));
                    }

                    break;
                case EnOpenedInventoryPopup.Compose:

                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].EnType == EnInventoryObjectType.Item)
                    {
                        int nItemId = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectItem.Item.ItemId;

                        m_csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(nItemId);

                        if (m_csItemCompositionRecipe.MaterialItemCount <= CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId))
                        {
                            //합성가능한 개수를 가지고있으면
                            if (m_nLastSelectComposeItem == -1)
                            {
                                //선택된 아이템이 없음
                                UpdateInventorySlotSelected(nSlotIndex, true);
                            }
                            else
                            {
                                UpdateInventorySlotSelected(m_nLastSelectComposeItem, false);
                                UpdateInventorySlotSelected(nSlotIndex, true);
                            }

                            m_nLastSelectComposeItem = nSlotIndex;

                            UpdateComposeDisplay();
                        }
                        else
                        {
                            m_nSelectInventoryListIndex = nOldSelectInventoryListIndex;
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A02_TXT_02003"));
                        }
                    }

                    break;
            }
        }
        else if (nSlotIndex < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
        {
            //빈창
            Debug.Log("빈 슬롯입니다");
        }
        else if (nInventoryLevelMaxCount > nSlotIndex - CsGameData.Instance.MyHeroInfo.PaidInventorySlotCount)
        {
            //레벨에 따른 오픈 슬롯
            Debug.Log("레벨을 올리면 오픈됩니다.");
        }
        else
        {
            m_nStartDiaInventoryIndex = nInventoryLevelMaxCount + CsGameData.Instance.MyHeroInfo.PaidInventorySlotCount;
            m_nExtendSlotCount = nSlotIndex - m_nStartDiaInventoryIndex + 1;
            m_nExtendNeedDia = 0;

            for (int i = 0; i < m_nExtendSlotCount; i++)
            {
                CsInventorySlotExtendRecipe csInventorySlotExtendRecipe = CsGameData.Instance.GetInventorySlotExtendRecipe(CsGameData.Instance.MyHeroInfo.PaidInventorySlotCount + 1 + i);

                if (csInventorySlotExtendRecipe != null)
                {
                    m_nExtendNeedDia += csInventorySlotExtendRecipe.Dia;
                }
            }

            //다이아개수체크
            if (CsGameData.Instance.MyHeroInfo.Dia >= m_nExtendNeedDia)
            {
                //팝업오픈
                CheckDiaSlot(true);
                OpenExtendPopup();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, string.Format(CsConfiguration.Instance.GetString("A02_TXT_02005"), m_nExtendNeedDia));
            }
        }

        m_bProcessingButton = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExtendOk()
    {
        CheckDiaSlot(false);

        if (m_nExtendNeedDia <= CsGameData.Instance.MyHeroInfo.Dia)
        {
            CsCommandEventManager.Instance.SendInventorySlotExtend(m_nExtendSlotCount);
        }
        else
        {
            Debug.Log("다이아가 부족합니다");
        }

        ClosePopupExtend();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExtendCancel()
    {
        CheckDiaSlot(false);
        ClosePopupExtend();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedInventoryCategory(EnInventoryTab enInventoryTab)
    {
        RectTransform rectTransform = m_trContent.GetComponent<RectTransform>();

        int nCount;

        switch (enInventoryTab)
        {
            case EnInventoryTab.All:

                m_enInventoryCategoryNow = EnInventoryTab.All;

                switch (m_enOpenedInventoryPopupNow)
                {
                    case EnOpenedInventoryPopup.None:

                        for (int i = 0; i < m_trContent.childCount; i++)
                        {
                            m_trContent.GetChild(i).gameObject.SetActive(true);

                            //Transform trInventorySlot = m_trContent.Find("InventorySlot" + i);
                            //trInventorySlot.gameObject.SetActive(true);
                        }

                        if (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount % 5 == 0)
                        {
                            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5 * 105) + 5);
                        }
                        else
                        {
                            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5 * 105) + 110);
                        }

                        break;

                    case EnOpenedInventoryPopup.SimpleShopSell:
                    case EnOpenedInventoryPopup.DeCompose:
                    case EnOpenedInventoryPopup.Compose:

                        nCount = 0;

                        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount; i++)
                        {
                            Transform trInventorySlot = m_trContent.Find("InventorySlot" + i);

                            if (trInventorySlot == null)
                            {
                                CreateInventorySlot(i);
                            }
                            else
                            {
                                UpdateInventorySlot(i);
                            }

                            trInventorySlot = m_trContent.Find("InventorySlot" + i);

                            if (trInventorySlot.gameObject.activeSelf)
                            {
                                nCount++;
                            }
                        }

                        if (nCount % 5 == 0)
                        {
                            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nCount / 5 * 105) + 5);
                        }
                        else
                        {
                            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nCount / 5 * 105) + 110);
                        }

                        break;
                }

                break;

            case EnInventoryTab.Gear:
                m_enInventoryCategoryNow = EnInventoryTab.Gear;

                int nGearCount = 0;

                for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
                {
                    Transform trInventorySlot = m_trContent.Find("InventorySlot" + i);

                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.MainGear
                        || CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.SubGear
                        || CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.MountGear)
                    {
                        if (trInventorySlot == null)
                        {
                            CreateInventorySlot(i);
                            trInventorySlot = m_trContent.Find("InventorySlot" + i);
                        }

                        switch (m_enOpenedInventoryPopupNow)
                        {
                            case EnOpenedInventoryPopup.None:
                                trInventorySlot.gameObject.SetActive(true);
                                nGearCount++;
                                break;

                            case EnOpenedInventoryPopup.SimpleShopSell:

                                if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.MainGear
                                    || CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.MountGear)
                                {
                                    trInventorySlot.gameObject.SetActive(true);
                                    nGearCount++;
                                }
                                else
                                {
                                    trInventorySlot.gameObject.SetActive(false);
                                }

                                break;

                            case EnOpenedInventoryPopup.DeCompose:

                                if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.MainGear)
                                {
                                    trInventorySlot.gameObject.SetActive(true);
                                    nGearCount++;
                                }
                                else
                                {
                                    trInventorySlot.gameObject.SetActive(false);
                                }

                                break;

                            case EnOpenedInventoryPopup.Compose:

                                trInventorySlot.gameObject.SetActive(false);
                                break;
                        }
                    }
                    else
                    {
                        if (trInventorySlot != null)
                        {
                            trInventorySlot.gameObject.SetActive(false);
                        }
                    }
                }

                if (nGearCount % 5 == 0)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nGearCount / 5 * 105) + 5);
                }
                else
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nGearCount / 5 * 105) + 110);
                }

                for (int i = 0; i < m_trContent.childCount - CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
                {
                    Transform trInventorySlot = m_trContent.Find("InventorySlot" + (i + CsGameData.Instance.MyHeroInfo.InventorySlotList.Count));

                    if (trInventorySlot != null)
                    {
                        trInventorySlot.gameObject.SetActive(false);
                    }
                }

                break;

            case EnInventoryTab.Item:
                m_enInventoryCategoryNow = EnInventoryTab.Item;

                int nItemCount = 0;

                for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
                {
                    Transform trInventorySlot = m_trContent.Find("InventorySlot" + i);

                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.Item)
                    {
                        if (trInventorySlot == null)
                        {
                            CreateInventorySlot(i);
                            trInventorySlot = m_trContent.Find("InventorySlot" + i);
                        }

                        switch (m_enOpenedInventoryPopupNow)
                        {
                            case EnOpenedInventoryPopup.None:
                                trInventorySlot.gameObject.SetActive(true);
                                nItemCount++;
                                break;

                            case EnOpenedInventoryPopup.SimpleShopSell:

                                if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].InventoryObjectItem.Item.Saleable)
                                {
                                    trInventorySlot.gameObject.SetActive(true);
                                    nItemCount++;
                                }
                                else
                                {
                                    trInventorySlot.gameObject.SetActive(false);
                                }

                                break;

                            case EnOpenedInventoryPopup.DeCompose:
                                trInventorySlot.gameObject.SetActive(false);
                                break;

                            case EnOpenedInventoryPopup.Compose:

                                if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].InventoryObjectItem.Item.IsComposable)
                                {
                                    trInventorySlot.gameObject.SetActive(true);
                                    nItemCount++;
                                }
                                else
                                {
                                    trInventorySlot.gameObject.SetActive(false);
                                }

                                break;
                        }
                    }
                    else
                    {
                        if (trInventorySlot != null)
                        {
                            trInventorySlot.gameObject.SetActive(false);
                        }
                    }
                }

                if (nItemCount % 5 == 0)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nItemCount / 5 * 105) + 5);
                }
                else
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (nItemCount / 5 * 105) + 110);
                }



                for (int i = 0; i < m_trContent.childCount - CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
                {
                    Transform trInventorySlot = m_trContent.Find("InventorySlot" + (i + CsGameData.Instance.MyHeroInfo.InventorySlotList.Count));

                    if (trInventorySlot != null)
                    {
                        trInventorySlot.gameObject.SetActive(false);
                    }
                }

                break;
        }

        Toggle toggleInventoryTab = m_trBack.Find("ToggleList/Toggle" + (int)enInventoryTab).GetComponent<Toggle>();
        Text textLabel = toggleInventoryTab.transform.Find("Label").GetComponent<Text>();

        if (toggleInventoryTab.isOn)
        {
            textLabel.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textLabel.color = CsUIData.Instance.ColorGray;
        }
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trBack = transform.Find("ImageBack");
        m_trBotFrame = m_trBack.Find("BotFrame");
        m_trBotDeposit = m_trBack.Find("BotDeposit");

        m_textInventoryCount = m_trBotFrame.Find("TextInventoryCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textInventoryCount);
        UpdateInventoryCountText();

        //합성버튼
        Button buttonCompose = m_trBotFrame.Find("ButtonCompose").GetComponent<Button>();
        buttonCompose.onClick.RemoveAllListeners();
        buttonCompose.onClick.AddListener(OnClickCompose);
        buttonCompose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCompose = buttonCompose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCompose);
        textCompose.text = CsConfiguration.Instance.GetString("A02_BTN_00001");

        //분해버튼
        Button buttonDisassemble = m_trBotFrame.Find("ButtonDisassemble").GetComponent<Button>();
        buttonDisassemble.onClick.RemoveAllListeners();
        buttonDisassemble.onClick.AddListener(OnClickDisassemble);
        buttonDisassemble.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textDisassemble = buttonDisassemble.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDisassemble);
        textDisassemble.text = CsConfiguration.Instance.GetString("A02_BTN_00002");

        //상점버튼
        m_buttonShop = m_trBotFrame.Find("ButtonShop").GetComponent<Button>();
        m_buttonShop.onClick.RemoveAllListeners();
        m_buttonShop.onClick.AddListener(OnClickShop);
        m_buttonShop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textShop = m_buttonShop.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textShop);
        textShop.text = CsConfiguration.Instance.GetString("A02_BTN_00003");

        m_buttonSelectMode = m_trBotDeposit.Find("ButtonSelectMode").GetComponent<Button>();
        m_buttonSelectMode.onClick.RemoveAllListeners();
        m_buttonSelectMode.onClick.AddListener(OnClickSelectMode);
        m_buttonSelectMode.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textSelectMode = m_buttonSelectMode.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSelectMode);
        textSelectMode.text = CsConfiguration.Instance.GetString("A113_BTN_00005");

        m_buttonDeposit = m_trBotDeposit.Find("ButtonDeposit").GetComponent<Button>();
        m_buttonDeposit.onClick.RemoveAllListeners();
        m_buttonDeposit.onClick.AddListener(OnClickDeposit);
        m_buttonDeposit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textDeposit = m_buttonDeposit.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDeposit);
        textDeposit.text = CsConfiguration.Instance.GetString("A113_BTN_00011");
        m_buttonDeposit.gameObject.SetActive(false);

        m_buttonCancel = m_trBotDeposit.Find("ButtonCancel").GetComponent<Button>();
        m_buttonCancel.onClick.RemoveAllListeners();
        m_buttonCancel.onClick.AddListener(OnClickModeCancel);
        m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCancel = m_buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCancel);
        textCancel.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
        m_buttonCancel.gameObject.SetActive(false);

        DisplayInventoryTabList();
        CreateBaseInventory();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInventoryCountText()
    {
        m_textInventoryCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.InventorySlotList.Count, CsGameData.Instance.MyHeroInfo.InventorySlotCount);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csitem = null, bool bOwned = false)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        if (csitem == null)
        {
            OpenPopupItemInfo();
        }
        else
        {
            OpenPopupItemInfo(csitem, bOwned);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo()
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        bool bIsWarehouse = m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.Warehouse;              // 인벤토리를 사용하고 있는 곳이 창고인지 확인
        EnItemLocationType enItemLocationType = bIsWarehouse ? EnItemLocationType.Inventory : EnItemLocationType.None;
        int nWarehouseObjectIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].Index;
        // 창고에서 아이템 팝업창을 띄울 경우, 맡기고 찾기 외 버튼은 모두 숨겨달라고 했기 때문에 
        // 창고 탭에서는 bButtonOn 파라미터를 모두 false로 될 수 있게 설정

		switch (CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].EnType)
		{
			case EnInventoryObjectType.MainGear:
				Debug.Log("main inventory" + CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectMainGear.HeroMainGear);
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Right, CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectMainGear.HeroMainGear, false, !bIsWarehouse, enItemLocationType, nWarehouseObjectIndex);
				break;

			case EnInventoryObjectType.MountGear:
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Right, CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectMountGear.HeroMountGear, false, !bIsWarehouse, enItemLocationType, nWarehouseObjectIndex);
				break;

			case EnInventoryObjectType.SubGear:
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Right, CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectSubGear.HeroSubGear, !bIsWarehouse, enItemLocationType, nWarehouseObjectIndex);
				break;

			case EnInventoryObjectType.Item:
				CsInventoryObjectItem csInventoryObjectItem = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectItem;
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Right, csInventoryObjectItem.Item, csInventoryObjectItem.Count, csInventoryObjectItem.Owned, m_nSelectInventoryListIndex, !bIsWarehouse, true, enItemLocationType, nWarehouseObjectIndex);
				break;
		}

		m_trBack.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csitem, bool bOwned)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, bOwned, m_nSelectInventoryListIndex);
    }

    //---------------------------------------------------------------------------------------------------
    public void QuickOpenPopupItemInfo(int nSelectInventoryListIndex)
    {
        m_nSelectInventoryListIndex = nSelectInventoryListIndex;
        StartCoroutine(LoadPopupItemInfo());
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayInventoryTabList()
    {
        Transform trToggleList = m_trBack.Find("ToggleList");

        for (int i = 0; i < trToggleList.childCount; i++)
        {
            int nInventoryToggleIndex = i;

            Toggle toggle = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();

            Text textToggle = toggle.transform.Find("Label").GetComponent<Text>();
            CsUIData.Instance.SetFont(textToggle);
            
            if (i == 0)
            {
                toggle.isOn = true;
                textToggle.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textToggle.color = CsUIData.Instance.ColorGray;
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedInventoryCategory((EnInventoryTab)nInventoryToggleIndex));
            toggle.onValueChanged.AddListener((ison) => { if (ison) { CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup); } });

            switch (i)
            {
                case 0:
                    textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00019");
                    break;
                case 1:
                    textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00020");
                    break;
                case 2:
                    textToggle.text = CsConfiguration.Instance.GetString("A02_TXT_00021");
                    break;
                case 3:
                    toggle.gameObject.SetActive(false);
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateBaseInventory()
    {
        m_nLoadCompliteSlotCount = 0;
        int BaseLoadCount = 30;

        m_listPotionSlot.Clear();
        m_listReturnScrollSlot.Clear();

        if (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount < BaseLoadCount)
        {
            BaseLoadCount = CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount;
        }

        m_trContent = m_trBack.Find("Scroll View/Viewport/Content");

        RectTransform rectTransform = m_trContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5 * 105) + 5);

        for (int i = 0; i < BaseLoadCount; i++)
        {
            Transform trInventorySlot = m_trContent.Find("InventorySlot" + i);
            int nSlotNum = i;

            if (trInventorySlot == null)
            {
                CreateInventorySlot(nSlotNum);
            }
        }

        Scrollbar scrollbar = m_trBack.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedInventoryScrollbar(scrollbar));
    }


    //---------------------------------------------------------------------------------------------------
    //스크롤바가 내려가면 로드되지 않은 인벤토리를 로드한다.
    void OnValueChangedInventoryScrollbar(Scrollbar scrollbar)
    {
        if (m_enInventoryCategoryNow == EnInventoryTab.All)
        {
            if (!m_bIsLoad)
            {
                m_bIsLoad = true;

                if (m_nLoadCompliteSlotCount < CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount)
                {
                    int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5), (1 - scrollbar.value))); // 최소 최대값 확인 필요.

                    if (nUpdateLine > m_nstandardPosition)
                    {
                        int nStartCount = m_nLoadCompliteSlotCount;
                        int nEndCount = (nUpdateLine + 5) * 5;

                        if (nEndCount >= CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount)
                        {
                            nEndCount = CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount;
                        }

                        for (int i = nStartCount; i < nEndCount; i++)
                        {
                            int nSlotNum = i;
                            CreateInventorySlot(nSlotNum);
                        }

                        m_nstandardPosition = nUpdateLine;
                    }
                }

                m_bIsLoad = false;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //슬롯생성함수
    void CreateInventorySlot(int nSlotIndex)
    {
        Transform trSlot = m_trContent.Find("InventorySlot" + nSlotIndex);

        if (trSlot == null)
        {
            GameObject goInventorySlot = Instantiate(m_goInventorySlot, m_trContent);
            goInventorySlot.name = "InventorySlot" + nSlotIndex;
            trSlot = goInventorySlot.transform;
            m_nLoadCompliteSlotCount++;

            Button buttonInventorySlot = trSlot.GetComponent<Button>();
            buttonInventorySlot.onClick.RemoveAllListeners();
            buttonInventorySlot.onClick.AddListener(() => OnClickInventorySlot(nSlotIndex));
            buttonInventorySlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        UpdateInventorySlot(nSlotIndex);
    }


    //---------------------------------------------------------------------------------------------------
    //슬롯 전체 업데이트 함수
    void UpdateInventoryAll()
    {
        m_listPotionSlot.Clear();
        m_listReturnScrollSlot.Clear();

        for (int i = 0; i < m_nLoadCompliteSlotCount; i++)
        {
            UpdateInventorySlot(i);
        }

        OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
        UpdateInventoryCountText();
    }


    //---------------------------------------------------------------------------------------------------
    //슬롯업데이트 함수
    void UpdateInventorySlot(int nSlotIndex)
    {
        Transform trInventorySlot = m_trContent.Find("InventorySlot" + nSlotIndex);

        if (trInventorySlot == null)
        {
            return;
        }

        Transform TrCheck = trInventorySlot.Find("ImageCheck");

        if (TrCheck.gameObject.activeSelf)
        {
            TrCheck.gameObject.SetActive(false);
        }

        //Button buttonInventorySlot = trInventorySlot.GetComponent<Button>();

        if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count > nSlotIndex)
        {
            //아이템이 있는 슬롯
            Transform trItemSlot = trInventorySlot.Find("ItemSlot");

            if (trItemSlot == null)
            {
                GameObject goItemSlot = Instantiate(m_goItemSlot, trInventorySlot);
                goItemSlot.name = "ItemSlot";
                trItemSlot = goItemSlot.transform;

                goItemSlot.GetComponent<Button>().enabled = false;
            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            switch (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].EnType)
            {
                case EnInventoryObjectType.MainGear:
                    CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectMainGear.HeroMainGear;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGear);
                    break;

                case EnInventoryObjectType.MountGear:
                    CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectMountGear.HeroMountGear;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMountGear);
                    break;

                case EnInventoryObjectType.SubGear:
                    CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectSubGear.HeroSubGear;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroSubGear);
                    break;

                case EnInventoryObjectType.Item:
                    CsInventoryObjectItem csInventoryObjectItem = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectItem;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventoryObjectItem);

                    switch (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectItem.Item.ItemType.EnItemType)
                    {
                        case EnItemType.HpPotion:
                            m_listPotionSlot.Add(nSlotIndex);
                            break;
                        case EnItemType.ReturnScroll:
                            m_listReturnScrollSlot.Add(nSlotIndex);
                            break;
                    }
                    break;
            }

            //선택슬롯표시
            Image imageCooltime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
            imageCooltime.fillAmount = 0;

            //if (buttonInventorySlot.interactable == false)
            //{
            //    imageCooltime.fillAmount = 1;
            //}
            //else
            //{
            //    imageCooltime.fillAmount = 0;
            //}

            switch (m_enOpenedInventoryPopupNow)
            {
                case EnOpenedInventoryPopup.None:
                    break;
                case EnOpenedInventoryPopup.SimpleShopSell:
                    switch (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].EnType)
                    {
                        case EnInventoryObjectType.MainGear:
                            trInventorySlot.gameObject.SetActive(true);
                            break;

                        case EnInventoryObjectType.MountGear:
                            trInventorySlot.gameObject.SetActive(true);
                            break;

                        case EnInventoryObjectType.SubGear:
                            trInventorySlot.gameObject.SetActive(false);
                            break;

                        case EnInventoryObjectType.Item:
                            CsInventoryObjectItem csInventoryObjectItem = CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectItem;

                            if (csInventoryObjectItem.Item.Saleable)
                            {
                                trInventorySlot.gameObject.SetActive(true);
                            }
                            else
                            {
                                trInventorySlot.gameObject.SetActive(false);
                            }

                            break;
                    }

                    break;

                case EnOpenedInventoryPopup.DeCompose:

                    switch (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].EnType)
                    {
                        case EnInventoryObjectType.MainGear:
                            trInventorySlot.gameObject.SetActive(true);
                            break;

                        case EnInventoryObjectType.MountGear:
                            trInventorySlot.gameObject.SetActive(false);
                            break;

                        case EnInventoryObjectType.SubGear:
                            trInventorySlot.gameObject.SetActive(false);
                            break;

                        case EnInventoryObjectType.Item:
                            trInventorySlot.gameObject.SetActive(false);
                            break;
                    }

                    break;

                case EnOpenedInventoryPopup.Compose:

                    switch (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].EnType)
                    {
                        case EnInventoryObjectType.MainGear:
                            trInventorySlot.gameObject.SetActive(false);
                            break;

                        case EnInventoryObjectType.MountGear:
                            trInventorySlot.gameObject.SetActive(false);
                            break;

                        case EnInventoryObjectType.SubGear:
                            trInventorySlot.gameObject.SetActive(false);
                            break;

                        case EnInventoryObjectType.Item:

                            if (CsGameData.Instance.MyHeroInfo.InventorySlotList[nSlotIndex].InventoryObjectItem.Item.IsComposable)
                            {
                                trInventorySlot.gameObject.SetActive(true);
                            }
                            else
                            {
                                trInventorySlot.gameObject.SetActive(false);
                            }
                            break;
                    }

                    break;
            }
        }
        else
        {
            //아이템이 없거나 잠긴슬롯

            Transform trCheck = trInventorySlot.Find("ImageCheck");
            Transform trLock = trInventorySlot.Find("ImageLock");

            trCheck.gameObject.SetActive(false);

            Text textRequiredLevel = trInventorySlot.Find("TextLevel").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRequiredLevel);

            if (nSlotIndex < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
            {
                //빈슬롯
                trLock.gameObject.SetActive(false);
                textRequiredLevel.text = "";

                Transform trItemSlot = trInventorySlot.Find("ItemSlot");

                if (trItemSlot != null)
                {
                    trItemSlot.gameObject.SetActive(false);
                }
            }
            else if (CsGameData.Instance.JobLevelMasterList[CsGameData.Instance.JobLevelMasterList.Count - 1].InventorySlotAccCount > nSlotIndex - CsGameData.Instance.MyHeroInfo.PaidInventorySlotCount)
            {
                for (int i = 0; i < CsGameData.Instance.JobLevelMasterList.Count; i++)
                {
                    if (CsGameData.Instance.JobLevelMasterList[i].InventorySlotAccCount >= nSlotIndex - CsGameData.Instance.MyHeroInfo.PaidInventorySlotCount + 1)
                    {
                        textRequiredLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TXT_LEVEL"), CsGameData.Instance.JobLevelMasterList[i].Level);
                        trLock.gameObject.SetActive(false);
                        break;
                    }
                }
            }
            else
            {
                trLock.gameObject.SetActive(true);
                textRequiredLevel.text = "";
            }

            switch (m_enOpenedInventoryPopupNow)
            {
                case EnOpenedInventoryPopup.None:
                    break;
                case EnOpenedInventoryPopup.SimpleShopSell:
                case EnOpenedInventoryPopup.DeCompose:
                case EnOpenedInventoryPopup.Compose:
                    trInventorySlot.gameObject.SetActive(false);
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenExtendPopup()
    {
        if (m_goExtendPopup == null)
        {
            StartCoroutine(LoadExtendPopupCoroutine());
        }
        else
        {
            CreatePopupExtend();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadExtendPopupCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/PopupInventoryExtend");
        yield return resourceRequest;
        m_goExtendPopup = (GameObject)resourceRequest.asset;

        CreatePopupExtend();
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupExtend()
    {
        GameObject goExtendPopup = Instantiate(m_goExtendPopup, m_trPopupList);
        goExtendPopup.name = "PopupInventoryExtend";

        m_trPopupExtend = goExtendPopup.transform;

        Transform trBack = goExtendPopup.transform.Find("ImageBackground");

        Text textMessage = trBack.Find("TextMessage").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMessage);
        textMessage.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01001"), m_nExtendNeedDia, m_nExtendSlotCount);

        Text textWarning = trBack.Find("TextWarning").GetComponent<Text>();
        CsUIData.Instance.SetFont(textWarning);
        textWarning.text = CsConfiguration.Instance.GetString("A02_TXT_00010");

        Transform trButtonList = trBack.Find("Buttons");

        Button buttonOk = trButtonList.Find("Button1").GetComponent<Button>();
        buttonOk.onClick.RemoveAllListeners();
        buttonOk.onClick.AddListener(OnClickExtendOk);
        buttonOk.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textOk = buttonOk.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOk);
        textOk.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");


        Button buttonNo = trButtonList.Find("Button2").GetComponent<Button>();
        buttonNo.onClick.RemoveAllListeners();
        buttonNo.onClick.AddListener(OnClickExtendCancel);
        buttonNo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonNo.gameObject.SetActive(true);


        Text textNo = buttonNo.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNo);
        textNo.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupExtend()
    {
        if (m_trPopupExtend != null)
        {
            Destroy(m_trPopupExtend.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckDiaSlot(bool bIsCheck)
    {
        for (int i = m_nStartDiaInventoryIndex; i <= m_nSelectInventoryListIndex; i++)
        {
            Transform trInventorySlot = m_trContent.Find("InventorySlot" + i);

            Transform trCheck = trInventorySlot.Find("ImageCheck");
            trCheck.gameObject.SetActive(bIsCheck);
        }
    }

    #region 상점

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventBuyItem(int nCount)
    {
        CsCommandEventManager.Instance.SendSimpleShopBuy(m_csSimpleShopProductSeleced.ProductId, nCount);
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
    void OnValueChangedSimpleShopTab(Toggle toggle, bool bIsBuy)
    {
        Text textToggleTab = toggle.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleTab);

        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            textToggleTab.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textToggleTab.color = CsUIData.Instance.ColorGray;
        }

        if (bIsBuy)
        {
            UpdateShopBuy(toggle);
        }
        else
        {
            UpdateShopSell(toggle);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseSimpleShop()
    {
        Transform trInventoryShop = m_trPopupList.Find("InventoryShop");

        if (trInventoryShop != null)
        {
            ResetSimpleShopSellList();
            m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.None;
            OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
            CsGameEventUIToUI.Instance.OnEventVisibleSection(true);

            Destroy(trInventoryShop.gameObject);
            m_bPrefabLoad = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSimpleShopBuy(int nProductIndex)
    {

        m_csSimpleShopProductSeleced = CsGameData.Instance.SimpleShopProductList[nProductIndex];

        //구매 팝업 오픈.
        if (m_goPopupCalculator == null)
        {
            StartCoroutine(LoadPopupSkillBookShopCoroutine());
        }
        else
        {
            OpenPopupBuyItem();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSimpleShopSellItem(int nSellSlotIndex)
    {
        UpdateInventorySlotByInventoryIndex((int)m_arraySellItem[nSellSlotIndex], false);
        m_arraySellItem[nSellSlotIndex] = null;
        UpdateSellSlot(nSellSlotIndex);
        UpdateSellDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSimpleShopSell()
    {
        List<int> listTmep = new List<int>();

        listTmep.Clear();

        for (int i = 0; i < m_arraySellItem.Length; i++)
        {
            if (m_arraySellItem[i] != null)
            {
                listTmep.Add((int)m_arraySellItem[i]);
            }
        }

        if (listTmep.Count > 0)
        {
            ResetSimpleShopSellList();
            CsCommandEventManager.Instance.SendSimpleShopSell(listTmep.ToArray());
        }
        else
        {
            Debug.Log("선택된 아이템이 없음");
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void OpenShop()
    {
        if (m_bPrefabLoad) return;

        m_bPrefabLoad = true;

        if (m_goShop == null)
        {
            StartCoroutine(LoadShopCoroutine());
        }
        else
        {
            InitializeShop();
        }

        CsGameEventUIToUI.Instance.OnEventVisibleSection(false);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadShopCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/Shop");
        yield return resourceRequest;
        m_goShop = (GameObject)resourceRequest.asset;

        InitializeShop();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeShop()
    {
        m_trInventoryShop = m_trPopupList.Find("InventoryShop");

        if (m_trInventoryShop == null)
        {
            GameObject goInventoryShop = Instantiate(m_goShop, m_trPopupList);
            goInventoryShop.name = "InventoryShop";
            m_trInventoryShop = goInventoryShop.transform;

            Transform trBack = m_trInventoryShop.Find("ImageBackground");

            Transform trToggleList = trBack.Find("ToggleList");

            Toggle toggleBuy = trToggleList.Find("ToggleBuy").GetComponent<Toggle>();
            toggleBuy.onValueChanged.RemoveAllListeners();
            toggleBuy.isOn = true;
            toggleBuy.onValueChanged.AddListener((ison) => OnValueChangedSimpleShopTab(toggleBuy, true));
            UpdateShopBuy(toggleBuy);

            Text textToggleBuy = toggleBuy.transform.Find("Label").GetComponent<Text>();
            CsUIData.Instance.SetFont(textToggleBuy);
            textToggleBuy.text = CsConfiguration.Instance.GetString("A02_BTN_00010");
            textToggleBuy.color = CsUIData.Instance.ColorWhite;

            Toggle toggleSell = trToggleList.Find("ToggleSell").GetComponent<Toggle>();
            toggleSell.onValueChanged.RemoveAllListeners();
            toggleSell.onValueChanged.AddListener((ison) => OnValueChangedSimpleShopTab(toggleSell, false));

            Text textToggleSell = toggleSell.transform.Find("Label").GetComponent<Text>();
            CsUIData.Instance.SetFont(textToggleSell);
            textToggleSell.text = CsConfiguration.Instance.GetString("A02_BTN_00011");
            textToggleSell.color = CsUIData.Instance.ColorGray;

            Button buttonClose = trBack.Find("BotFrame/ButtonPopupClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(OnClickCloseSimpleShop);
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textClose = buttonClose.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textClose);
            textClose.text = CsConfiguration.Instance.GetString("A02_BTN_00013");
        }
        else
        {
            m_trInventoryShop.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateShopBuy(Toggle toggle)
    {
        Transform trBack = m_trInventoryShop.Find("ImageBackground");
        Transform trBuy = trBack.Find("Buy");

        if (toggle.isOn)
        {
            trBuy.gameObject.SetActive(true);

            Transform trContent = trBuy.Find("Scroll View/Viewport/Content");

            if (m_goShopProduct == null)
            {
                m_goShopProduct = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharacter/ShopItem");
            }

            for (int i = 0; i < CsGameData.Instance.SimpleShopProductList.Count; i++)
            {
                int nProductIndex = i;
                Transform trProduct = trContent.Find("Product" + nProductIndex);

                Text textItemName;
                Text textPrice;

                if (trProduct == null)
                {
                    GameObject goProduct = Instantiate(m_goShopProduct, trContent);
                    goProduct.name = "Product" + nProductIndex;
                    trProduct = goProduct.transform;

                    textItemName = trProduct.Find("TextItemName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textItemName);

                    textPrice = trProduct.Find("TextPrice").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textPrice);

                    //버튼세팅
                    Button buttonBuy = trProduct.Find("Button").GetComponent<Button>();
                    buttonBuy.onClick.RemoveAllListeners();
                    buttonBuy.onClick.AddListener(() => OnClickSimpleShopBuy(nProductIndex));
                    buttonBuy.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                    Text textButton = buttonBuy.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textButton);
                    textButton.text = CsConfiguration.Instance.GetString("A02_BTN_00012");
                }
                else
                {
                    trProduct.gameObject.SetActive(true);
                    textItemName = trProduct.Find("TextItemName").GetComponent<Text>();
                    textPrice = trProduct.Find("TextPrice").GetComponent<Text>();
                }

                //아이템슬롯세팅
                Transform trItemSlot = trProduct.Find("ItemSlot");
                CsUIData.Instance.DisplayItemSlot(trItemSlot, CsGameData.Instance.SimpleShopProductList[i].Item, CsGameData.Instance.SimpleShopProductList[i].ItemOwned, 0);

                //아이템이름
                textItemName.text = CsGameData.Instance.SimpleShopProductList[i].Item.Name;

                //아이템가격
                textPrice.text = CsGameData.Instance.SimpleShopProductList[i].SaleGold.ToString("#,##0");

                //아이템가격타입 이미지
                //보류
            }

            for (int i = 0; i < trContent.childCount - CsGameData.Instance.SimpleShopProductList.Count; i++)
            {
                Transform trProduct = trContent.Find("Product" + (i + CsGameData.Instance.SimpleShopProductList.Count));
                trProduct.gameObject.SetActive(false);
            }
        }
        else
        {
            trBuy.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateShopSell(Toggle toggle)
    {
        Transform trBack = m_trInventoryShop.Find("ImageBackground");
        Transform trSell = trBack.Find("Sell");

        if (toggle.isOn)
        {
            Text textSellGuide = trSell.Find("TextSellGuide").GetComponent<Text>();
            CsUIData.Instance.SetFont(textSellGuide);
            textSellGuide.text = CsConfiguration.Instance.GetString("");

            Transform trImageGold = trSell.Find("ImageGetGold");

            Text textGetGold = trImageGold.Find("TextGold").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGetGold);
            textGetGold.text = CsConfiguration.Instance.GetString("A02_TXT_00007");

            m_textGetGoldValue = trImageGold.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textGetGoldValue);
            m_textGetGoldValue.text = "0";

            m_buttonSell = trSell.Find("Button").GetComponent<Button>();
            m_buttonSell.onClick.RemoveAllListeners();
            m_buttonSell.onClick.AddListener(OnClickSimpleShopSell);
            m_buttonSell.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textButton = m_buttonSell.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButton);
            textButton.text = CsConfiguration.Instance.GetString("A02_BTN_00014");

            CsUIData.Instance.DisplayButtonInteractable(m_buttonSell, false);

            trSell.gameObject.SetActive(true);
            m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.SimpleShopSell;
            OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
        }
        else
        {
            m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.None;
            OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
            trSell.gameObject.SetActive(false);
            ResetSimpleShopSellList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSkillBookShopCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        OpenPopupBuyItem();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupBuyItem()
    {
        m_trCalculator = m_trPopupList.Find("PopupCalculator");

        if (m_trCalculator == null)
        {
            GameObject goPopupBuyCount = Instantiate(m_goPopupCalculator, m_trPopupList);
            goPopupBuyCount.name = "PopupCalculator";
            m_trCalculator = goPopupBuyCount.transform;
        }
        else
        {
            m_trCalculator.gameObject.SetActive(false);
        }

        m_csPopupCalculator = m_trCalculator.GetComponent<CsPopupCalculator>();
        m_csPopupCalculator.EventBuyItem += OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;
        m_csPopupCalculator.DisplayItem(m_csSimpleShopProductSeleced.Item, m_csSimpleShopProductSeleced.ItemOwned, m_csSimpleShopProductSeleced.SaleGold, EnResourceType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSellSlot(int nSellSlotIndex)
    {
        m_trInventoryShop = m_trPopupList.Find("InventoryShop");
        Transform trSellSlot = m_trInventoryShop.Find("ImageBackground/Sell/SellItemList/ImageItemSlot" + nSellSlotIndex);
        Transform trItemSlot = trSellSlot.Find("ItemSlot");

        if (m_arraySellItem[nSellSlotIndex] == null)
        {
            if (trItemSlot != null)
            {
                trItemSlot.gameObject.SetActive(false);
            }
        }
        else
        {
            if (trItemSlot == null)
            {
                GameObject goItemslot = Instantiate(m_goItemSlot, trSellSlot);
                goItemslot.name = "ItemSlot";
                trItemSlot = goItemslot.transform;

                Button buttonItemList = trItemSlot.GetComponent<Button>();
                buttonItemList.onClick.RemoveAllListeners();
                buttonItemList.onClick.AddListener(() => OnClickSimpleShopSellItem(nSellSlotIndex));
                buttonItemList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot((int)m_arraySellItem[nSellSlotIndex]);

            if (csInventorySlot.EnType == EnInventoryObjectType.MainGear)
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventorySlot.InventoryObjectMainGear.HeroMainGear);
            }
            else if (csInventorySlot.EnType == EnInventoryObjectType.MountGear)
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventorySlot.InventoryObjectMountGear.HeroMountGear);
            }
            else if (csInventorySlot.EnType == EnInventoryObjectType.Item)
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventorySlot.InventoryObjectItem);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSellDisplay()
    {
        int nSellGold = 0;

        for (int i = 0; i < m_arraySellItem.Length; i++)
        {
            if (m_arraySellItem[i] != null)
            {
                CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot((int)m_arraySellItem[i]);

                if (csInventorySlot.EnType == EnInventoryObjectType.MainGear)
                {
                    nSellGold += csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.SaleGold;
                }
                else if (csInventorySlot.EnType == EnInventoryObjectType.MountGear)
                {
                    nSellGold += csInventorySlot.InventoryObjectMountGear.HeroMountGear.MountGear.SaleGold;
                }
                else if (csInventorySlot.EnType == EnInventoryObjectType.Item)
                {
                    nSellGold += csInventorySlot.InventoryObjectItem.Item.SaleGold * csInventorySlot.InventoryObjectItem.Count;
                }
            }
        }

        m_textGetGoldValue.text = nSellGold.ToString("#,##0");

        if (nSellGold <= 0)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSell, false);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSell, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResetSimpleShopSellList()
    {
        for (int i = 0; i < m_arraySellItem.Length; i++)
        {
            if (m_arraySellItem[i] != null)
            {
                UpdateInventorySlotByInventoryIndex((int)m_arraySellItem[i], false);
                m_arraySellItem[i] = null;
                UpdateSellSlot(i);
            }
        }
    }

    #endregion 상점

    #region 분해


    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickDisassembleItem(int nDisassembleSlotIndex)
    {
        UpdateResultSlot(nDisassembleSlotIndex, false);
        UpdateInventorySlotByInventoryIndex((int)m_arrayDisassemble[nDisassembleSlotIndex], false);
        m_arrayDisassemble[nDisassembleSlotIndex] = null;
        UpdateDisassembleSlot(nDisassembleSlotIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMaterial(int nIndex)
    {
        //아이템정보창 보여주기
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(m_listCsDisassembleResultItem[nIndex].Item, m_listCsDisassembleResultItem[nIndex].Owned));
        }
        else
        {
            OpenPopupItemInfo(m_listCsDisassembleResultItem[nIndex].Item, m_listCsDisassembleResultItem[nIndex].Owned);
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickSoul()
	{
		if (CsArtifactManager.Instance.ArtifactNo == 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A157_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Soul, EnSubMenu.Soul);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickDisassembleAll()
    {
        List<Guid> listTmep = new List<Guid>();

        listTmep.Clear();

        for (int i = 0; i < m_arrayDisassemble.Length; i++)
        {
            if (m_arrayDisassemble[i] != null)
            {
                CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot((int)m_arrayDisassemble[i]);

                if (csInventorySlot.EnType == EnInventoryObjectType.MainGear)
                {
                    listTmep.Add(csInventorySlot.InventoryObjectMainGear.HeroMainGear.Id);
                }
            }
        }

        if (listTmep.Count > 0)
        {
            ResetDisassembleDisplay();
            CsCommandEventManager.Instance.SendMainGearDisassemble(listTmep.ToArray());
        }
        else
        {
            Debug.Log("선택된 아이템이 없음");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickInputAll()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.MainGear)
            {
                CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.InventorySlotList[i].InventoryObjectMainGear.HeroMainGear;

                if (csHeroMainGear.MainGear.MainGearGrade.Grade <= (int)m_enDisassembleInputTypeNow + 2)
                {
                    bool bIsFull = true;
                    bool bIsCheck = false;
                    
                    for (int j = 0; j < m_arrayDisassemble.Length; j++)
                    {
                        if (m_arrayDisassemble[j] == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (m_arrayDisassemble[j] == CsGameData.Instance.MyHeroInfo.InventorySlotList[i].Index)
                            {
                                bIsCheck = true;
                                break;
                            }
                        }
                    }

                    for (int j = 0; j < m_arrayDisassemble.Length; j++)
                    {
                        if (m_arrayDisassemble[j] == null)
                        {
                            bIsFull = false;

                            if (!bIsCheck)
                            {
                                m_arrayDisassemble[j] = CsGameData.Instance.MyHeroInfo.InventorySlotList[i].Index;
                                UpdateInventorySlotByInventoryIndex(CsGameData.Instance.MyHeroInfo.InventorySlotList[i].Index, true);
                                UpdateDisassembleSlot(j);
                                UpdateResultSlot(j, true);
                                break;
                            }
                        }
                    }

                    if (bIsFull)
                    {
                        break;
                        //CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02002"));
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseDisassemble()
    {
        if (m_trPopupDisassemble != null)
        {
            ResetDisassembleDisplay();
            m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.None;
            CsGameEventUIToUI.Instance.OnEventVisibleSection(true);
            OnValueChangedInventoryCategory(m_enInventoryCategoryNow);

            Destroy(m_trPopupDisassemble.gameObject);
            m_trPopupDisassemble = null;
            m_bPrefabLoad = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDropdown(Dropdown dropdown)
    {
        m_enDisassembleInputTypeNow = (EnDisassembleInputType)dropdown.value;

        if (dropdown.IsActive())
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void OpenDisassemble()
    {
        if (m_bPrefabLoad) return;

        m_bPrefabLoad = true;

        if (m_goDisassemble == null)
        {
            StartCoroutine(LoadDecomposeCoroutine());
        }
        else
        {
            InitializeDisassemble();
        }

        CsGameEventUIToUI.Instance.OnEventVisibleSection(false);
        m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.DeCompose;
        OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDecomposeCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/Disassemble");
        yield return resourceRequest;
        m_goDisassemble = (GameObject)resourceRequest.asset;

        InitializeDisassemble();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeDisassemble()
    {
        m_listCsDisassembleResultItem.Clear();

        GameObject goDisassemble = Instantiate(m_goDisassemble, m_trPopupList);
        goDisassemble.name = "PopupDisassemble";
        m_trPopupDisassemble = goDisassemble.transform;
        m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.DeCompose;
        OnValueChangedInventoryCategory(m_enInventoryCategoryNow);

        Transform trBack = m_trPopupDisassemble.Find("Background");

        Text textPopupName = trBack.Find("TextDisplayName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A02_NAME_00002");

		Button buttonSoul = trBack.Find("ButtonSoul").GetComponent<Button>();
		buttonSoul.onClick.RemoveAllListeners();
		buttonSoul.onClick.AddListener(OnClickSoul);
		buttonSoul.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		if (CsArtifactManager.Instance.ArtifactNo == 0)
		{
			CsUIData.Instance.DisplayButtonInteractable(buttonSoul, false);
		}
		else
		{
			CsUIData.Instance.DisplayButtonInteractable(buttonSoul, true);
		}

		Text textSoul = buttonSoul.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSoul);
		textSoul.text = CsConfiguration.Instance.GetString("MMENU_NAME_30");

        m_buttonDisassemble = trBack.Find("ButtonDeCompose").GetComponent<Button>();
        m_buttonDisassemble.onClick.RemoveAllListeners();
        m_buttonDisassemble.onClick.AddListener(OnClickDisassembleAll);
        m_buttonDisassemble.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        CsUIData.Instance.DisplayButtonInteractable(m_buttonDisassemble, false);

        Text textDisassemble = m_buttonDisassemble.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDisassemble);
        textDisassemble.text = CsConfiguration.Instance.GetString("A02_BTN_00007");

        Transform trBotFrame = trBack.Find("BotFrame");

        Button buttonInputAll = trBotFrame.Find("ButtonAllInput").GetComponent<Button>();
        buttonInputAll.onClick.RemoveAllListeners();
        buttonInputAll.onClick.AddListener(OnClickInputAll);
        buttonInputAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textInputAll = buttonInputAll.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInputAll);
        textInputAll.text = CsConfiguration.Instance.GetString("A02_BTN_00008");

        Button buttonClose = trBotFrame.Find("ButtonPopupClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickCloseDisassemble);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textClose = buttonClose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClose);
        textClose.text = CsConfiguration.Instance.GetString("A02_BTN_00009");

        Dropdown dropdown = trBotFrame.Find("Dropdown").GetComponent<Dropdown>();

        for (int i = 0; i < dropdown.options.Count; i++)
        {
            dropdown.options[i].text = CsConfiguration.Instance.GetString("A02_TXT_0000" + (3 + i));
        }

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.value = (int)EnDisassembleInputType.Legend;
        dropdown.onValueChanged.AddListener((ison) => OnValueChangedDropdown(dropdown));

        Text textDropdown = dropdown.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDropdown);

        Text textItem = dropdown.transform.Find("Template/Viewport/Content/Item/Item Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItem);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDisassembleSlot(int nDisassembleSlotIndex)
    {
        Transform trDisassembleList = m_trPopupDisassemble.Find("Background/DeComposeList");
        Transform trDisassembleSlot = trDisassembleList.Find("ImageSlot" + nDisassembleSlotIndex);
        Transform trItemSlot = trDisassembleSlot.Find("ItemSlot");

        if (m_arrayDisassemble[nDisassembleSlotIndex] == null)
        {
            if (trItemSlot != null)
            {
                trItemSlot.gameObject.SetActive(false);
            }
        }
        else
        {
            if (trItemSlot == null)
            {
                GameObject goItemslot = Instantiate(m_goItemSlot, trDisassembleSlot);
                goItemslot.name = "ItemSlot";
                trItemSlot = goItemslot.transform;

                Button buttonItemList = trItemSlot.GetComponent<Button>();
                buttonItemList.onClick.RemoveAllListeners();
                buttonItemList.onClick.AddListener(() => OnClickDisassembleItem(nDisassembleSlotIndex));
                buttonItemList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot((int)m_arrayDisassemble[nDisassembleSlotIndex]);

            if (csInventorySlot.EnType == EnInventoryObjectType.MainGear)
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventorySlot.InventoryObjectMainGear.HeroMainGear);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateResultList()
    {
        m_listCsDisassembleResultItem.Clear();

        for (int i = 0; i < m_arrayDisassemble.Length; i++)
        {
            if (m_arrayDisassemble[i] != null)
            {
                UpdateResultSlot(i, true);
            }
        }

        UpdateResultItemSlotDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateResultSlot(int nDisassembleIndex, bool bIsSelect)
    {
        CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot((int)m_arrayDisassemble[nDisassembleIndex]);

        if (csInventorySlot.EnType == EnInventoryObjectType.MainGear)
        {
            List<CsMainGearDisassembleAvailableResultEntry> listTemp = CsGameData.Instance.GetMainGearDisassembleAvailableResultEntryList(csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearTier.Tier, csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearGrade.Grade);

            for (int i = 0; i < listTemp.Count; i++)
            {
                CsDisassembleResultItem csDisassembleResultItem = m_listCsDisassembleResultItem.Find(a => a.Item == listTemp[i].Item && a.Owned == listTemp[i].ItemOwned);

                if (csDisassembleResultItem != null)
                {
                    if (bIsSelect)
                    {
                        csDisassembleResultItem.Count += listTemp[i].ItemCount;
                    }
                    else
                    {
                        csDisassembleResultItem.Count -= listTemp[i].ItemCount;

                        if (csDisassembleResultItem.Count <= 0)
                        {
                            m_listCsDisassembleResultItem.Remove(csDisassembleResultItem);
                        }
                    }
                }
                else
                {
                    if (bIsSelect)
                    {
                        m_listCsDisassembleResultItem.Add(new CsDisassembleResultItem(listTemp[i].Item, listTemp[i].ItemCount, listTemp[i].ItemOwned));
                    }
                }
            }
        }

        UpdateResultItemSlotDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateResultItemSlotDisplay()
    {
        Transform trContent = m_trPopupDisassemble.Find("Background/Scroll View/Viewport/Content");

        if (m_listCsDisassembleResultItem.Count > 0)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonDisassemble, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonDisassemble, false);
        }

        for (int i = 0; i < m_listCsDisassembleResultItem.Count; i++)
        {
            int nMaterialIndex = i;

            if (m_listCsDisassembleResultItem[i] != null)
            {
                Transform trResultSlot = trContent.Find("ResultSlot" + i);

                if (i > 4)
                {
                    trResultSlot = trContent.Find("ResultSlot" + i);

                    if (trResultSlot == null)
                    {
                        GameObject goResultSlot = Instantiate(CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharacter/DisassembleResultSlot"), trContent);
                        goResultSlot.name = "ResultSlot" + i;
                        trResultSlot = goResultSlot.transform;
                    }
                    else
                    {
                        trResultSlot.gameObject.SetActive(true);
                    }
                }

                Transform trItemSlot = trResultSlot.Find("ItemSlot");

                if (trItemSlot == null)
                {
                    GameObject goItemSlot = Instantiate(m_goItemSlot, trResultSlot);
                    goItemSlot.name = "ItemSlot";
                    trItemSlot = goItemSlot.transform;
                }
                else
                {
                    trItemSlot.gameObject.SetActive(true);
                }

                Button buttonMaterialItem = trItemSlot.GetComponent<Button>();
                buttonMaterialItem.onClick.RemoveAllListeners();
                buttonMaterialItem.onClick.AddListener(() => OnClickMaterial(nMaterialIndex));
                buttonMaterialItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                CsUIData.Instance.DisplayItemSlot(trItemSlot, m_listCsDisassembleResultItem[i].Item, m_listCsDisassembleResultItem[i].Owned, 0);
            }
        }

        for (int i = 0; i < trContent.childCount - m_listCsDisassembleResultItem.Count; i++)
        {
            Transform trResultSlot = trContent.Find("ResultSlot" + i);

            if (i + m_listCsDisassembleResultItem.Count < 5)
            {
                Transform trItemSlot = trResultSlot.Find("ItemSlot");

                if (trItemSlot != null)
                {
                    trItemSlot.gameObject.SetActive(false);
                }
            }
            else
            {
                trResultSlot.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResetDisassembleDisplay()
    {
        for (int i = 0; i < m_arrayDisassemble.Length; i++)
        {
            if (m_arrayDisassemble[i] != null)
            {
                UpdateInventorySlotByInventoryIndex((int)m_arrayDisassemble[i], false);
                m_arrayDisassemble[i] = null;
                UpdateDisassembleSlot(i);
            }
        }

        m_listCsDisassembleResultItem.Clear();
        UpdateResultItemSlotDisplay();
    }

    #endregion 분해



    #region 합성


    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickComposeMaterial()
    {
        string strGoldValue = m_textComposeGold.text.Replace(",", "");

        if (CsGameData.Instance.MyHeroInfo.Gold < int.Parse(strGoldValue))
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02007"));
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].EnType == EnInventoryObjectType.Item)
            {
                CsInventoryObjectItem csInventoryObjectItem = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectItem;

                if (csInventoryObjectItem.Item.IsComposable)
                {
                    m_nSelectSlotIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].Index;
                    CsCommandEventManager.Instance.SendItemCompose(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Owned);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAllComposeMaterial()
    {
        string strGoldValue = m_textAllComposeGold.text.Replace(",", "");

        if (CsGameData.Instance.MyHeroInfo.Gold < int.Parse(strGoldValue))
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02007"));
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].EnType == EnInventoryObjectType.Item)
            {
                CsInventoryObjectItem csInventoryObjectItem = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectItem;

                if (csInventoryObjectItem.Item.IsComposable)
                {
                    m_nSelectSlotIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].Index;
                    CsCommandEventManager.Instance.SendItemComposeTotally(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Owned);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseCompose()
    {
        if (m_trCompose != null)
        {
            if (m_nSelectInventoryListIndex > -1)
            {
                Debug.Log("m_nSelectInventoryListIndex : " + m_nSelectInventoryListIndex);
                UpdateInventorySlotSelected(m_nSelectInventoryListIndex, false);
            }

            m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.None;
            CsGameEventUIToUI.Instance.OnEventVisibleSection(true);
            OnValueChangedInventoryCategory(m_enInventoryCategoryNow);

            Destroy(m_trCompose.gameObject);
            m_trCompose = null;
            m_bPrefabLoad = false;
        }
    }

    #endregion EventHandler


    //---------------------------------------------------------------------------------------------------
    void OpenCompose()
    {
        if (m_bPrefabLoad) return;

        m_bPrefabLoad = true;

        if (m_goCompose == null)
        {
            StartCoroutine(LoadComposeCoroutine());
        }
        else
        {
            InitializeCompose();
        }

        CsGameEventUIToUI.Instance.OnEventVisibleSection(false);
        m_enOpenedInventoryPopupNow = EnOpenedInventoryPopup.Compose;
        OnValueChangedInventoryCategory(m_enInventoryCategoryNow);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadComposeCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/ComposeMaterial");
        yield return resourceRequest;
        m_goCompose = (GameObject)resourceRequest.asset;

        InitializeCompose();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeCompose()
    {
        m_csItemCompositionRecipe = null;
        m_nSelectInventoryListIndex = -1;

        GameObject goPopupCompose = Instantiate(m_goCompose, m_trPopupList);
        goPopupCompose.name = "PopupCompose";
        m_trCompose = goPopupCompose.transform;

        Transform trBack = m_trCompose.Find("Background");

        Text textPopupName = trBack.Find("TextDisplayName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A02_NAME_00001");

        Transform trComposeGold = trBack.Find("ComposeGold");

        Text textComposeGold = trComposeGold.Find("TextCompose").GetComponent<Text>();
        CsUIData.Instance.SetFont(textComposeGold);
        textComposeGold.text = CsConfiguration.Instance.GetString("A02_TXT_00001");

        m_textComposeGold = trComposeGold.Find("TextGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textComposeGold);

        Transform trAllComposeGold = trBack.Find("AllComposeGold");

        Text textAllComposeGold = trAllComposeGold.Find("TextCompose").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAllComposeGold);
        textAllComposeGold.text = CsConfiguration.Instance.GetString("A02_TXT_00002");

        m_textAllComposeGold = trAllComposeGold.Find("TextGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textComposeGold);

        m_buttonCompose = trBack.Find("ButtonCompose").GetComponent<Button>();
        m_buttonCompose.onClick.RemoveAllListeners();
        m_buttonCompose.onClick.AddListener(OnClickComposeMaterial);
        m_buttonCompose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCompose = m_buttonCompose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCompose);
        textCompose.text = CsConfiguration.Instance.GetString("A02_BTN_00004");

        m_buttonAllCompose = trBack.Find("ButtonAllCompose").GetComponent<Button>();
        m_buttonAllCompose.onClick.RemoveAllListeners();
        m_buttonAllCompose.onClick.AddListener(OnClickAllComposeMaterial);
        m_buttonAllCompose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAllCompose = m_buttonAllCompose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAllCompose);
        textAllCompose.text = CsConfiguration.Instance.GetString("A02_BTN_00005");

        Button buttonClose = trBack.Find("BotFrame/ButtonPopupClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickCloseCompose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textClose = buttonClose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClose);
        textClose.text = CsConfiguration.Instance.GetString("A02_BTN_00006");

        m_textMaterialItemName = trBack.Find("TextMaterialName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMaterialItemName);

        m_textResultItemName = trBack.Find("TextResultItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textResultItemName);

        UpdateComposeDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateComposeDisplay()
    {
        Transform trMaterialList = m_trCompose.Find("Background/MaterialList");

        Transform trMaterialOwn = trMaterialList.Find("ImageMaterialOwned");
        Transform trItemSlotOwn = trMaterialOwn.Find("ItemSlot");

        Transform trMaterialUnOwn = trMaterialList.Find("ImageMaterialUnOwned");
        Transform trItemSlotUnOwn = trMaterialUnOwn.Find("ItemSlot");

        Transform trResultSlot = m_trCompose.Find("Background/ImageResult");
        Transform trItemSlotResult = trResultSlot.Find("ItemSlot");

        RectTransform rtfArrow = m_trCompose.Find("Background/ImageArrow").GetComponent<RectTransform>();

        if (m_nSelectInventoryListIndex == -1)
        {
            trMaterialOwn.gameObject.SetActive(true);

            if (trItemSlotOwn != null)
            {
                trItemSlotOwn.gameObject.SetActive(false);
            }

            trMaterialUnOwn.gameObject.SetActive(false);

            if (trItemSlotResult != null)
            {
                trItemSlotResult.gameObject.SetActive(false);
            }

            m_textMaterialItemName.text = "";
            m_textResultItemName.text = "";
            m_textComposeGold.text = "0";
            m_textAllComposeGold.text = "0";

            CsUIData.Instance.DisplayButtonInteractable(m_buttonCompose, false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonAllCompose, false);
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].EnType == EnInventoryObjectType.Item)
            {
                CsInventoryObjectItem csInventoryObjectItem = CsGameData.Instance.MyHeroInfo.InventorySlotList[m_nSelectInventoryListIndex].InventoryObjectItem;
                int nTotalItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId);
                int nNowItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Owned);

                //선택된애가 개수가 충분하면
                if (m_csItemCompositionRecipe.MaterialItemCount <= nNowItemCount)
                {
                    trMaterialOwn.gameObject.SetActive(csInventoryObjectItem.Owned);
                    trMaterialUnOwn.gameObject.SetActive(!csInventoryObjectItem.Owned);

                    if (csInventoryObjectItem.Owned)
                    {
                        //귀속세팅
                        if (trItemSlotOwn == null)
                        {
                            GameObject goItemslot = Instantiate(m_goItemSlot, trMaterialOwn);
                            goItemslot.name = "ItemSlot";
                            trItemSlotOwn = goItemslot.transform;
                        }
                        else
                        {
                            trItemSlotOwn.gameObject.SetActive(true);
                        }

                        CsUIData.Instance.DisplayItemSlot(trItemSlotOwn, csInventoryObjectItem.Item, csInventoryObjectItem.Owned, CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Owned));
                    }
                    else
                    {
                        //비귀속세팅
                        if (trItemSlotUnOwn == null)
                        {
                            GameObject goItemslot = Instantiate(m_goItemSlot, trMaterialUnOwn);
                            goItemslot.name = "ItemSlot";
                            trItemSlotUnOwn = goItemslot.transform;
                        }
                        else
                        {
                            trItemSlotUnOwn.gameObject.SetActive(true);
                        }

                        CsUIData.Instance.DisplayItemSlot(trItemSlotOwn, csInventoryObjectItem.Item, csInventoryObjectItem.Owned, CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Owned));
                    }

                    //결과 아이템 세팅
                    if (trItemSlotResult == null)
                    {
                        GameObject goItemslot = Instantiate(m_goItemSlot, trResultSlot);
                        goItemslot.name = "ItemSlot";
                        trItemSlotResult = goItemslot.transform;
                    }
                    else
                    {
                        trItemSlotResult.gameObject.SetActive(true);
                    }

                    int nResoultItemCount = nNowItemCount / m_csItemCompositionRecipe.MaterialItemCount;
                    CsUIData.Instance.DisplayItemSlot(trItemSlotResult, m_csItemCompositionRecipe.Item, csInventoryObjectItem.Owned, nResoultItemCount);

                    m_textMaterialItemName.text = csInventoryObjectItem.Item.Name;
                    m_textResultItemName.text = m_csItemCompositionRecipe.Item.Name;
                    m_textComposeGold.text = m_csItemCompositionRecipe.Gold.ToString("#,##0");
                    m_textAllComposeGold.text = (nResoultItemCount * m_csItemCompositionRecipe.Gold).ToString("#,##0");

                    CsUIData.Instance.DisplayButtonInteractable(m_buttonCompose, true);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonAllCompose, true);

                    rtfArrow.anchoredPosition = new Vector2(0, rtfArrow.anchoredPosition.y);
                }
                else if (m_csItemCompositionRecipe.MaterialItemCount <= nTotalItemCount)
                {
                    trMaterialOwn.gameObject.SetActive(true);
                    trMaterialUnOwn.gameObject.SetActive(true);

                    if (trItemSlotOwn == null)
                    {
                        GameObject goItemslot = Instantiate(m_goItemSlot, trMaterialOwn);
                        goItemslot.name = "ItemSlot";
                        trItemSlotOwn = goItemslot.transform;
                    }
                    else
                    {
                        trItemSlotOwn.gameObject.SetActive(true);
                    }

                    if (trItemSlotUnOwn == null)
                    {
                        GameObject goItemslot = Instantiate(m_goItemSlot, trMaterialUnOwn);
                        goItemslot.name = "ItemSlot";
                        trItemSlotUnOwn = goItemslot.transform;
                    }
                    else
                    {
                        trItemSlotUnOwn.gameObject.SetActive(true);
                    }

                    //귀속세팅
                    CsUIData.Instance.DisplayItemSlot(trItemSlotOwn, csInventoryObjectItem.Item, csInventoryObjectItem.Owned, CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Owned));
                    //비귀속세팅
                    CsUIData.Instance.DisplayItemSlot(trItemSlotUnOwn, csInventoryObjectItem.Item, !csInventoryObjectItem.Owned, CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId, !csInventoryObjectItem.Owned));

                    //결과 아이템 세팅
                    if (trItemSlotResult == null)
                    {
                        GameObject goItemslot = Instantiate(m_goItemSlot, trResultSlot);
                        goItemslot.name = "ItemSlot";
                        trItemSlotResult = goItemslot.transform;
                    }
                    else
                    {
                        trItemSlotResult.gameObject.SetActive(true);
                    }

                    CsUIData.Instance.DisplayItemSlot(trItemSlotResult, m_csItemCompositionRecipe.Item, true, 1);

                    m_textMaterialItemName.text = csInventoryObjectItem.Item.Name;
                    m_textResultItemName.text = m_csItemCompositionRecipe.Item.Name;
                    m_textComposeGold.text = m_csItemCompositionRecipe.Gold.ToString("#,##0");
                    m_textAllComposeGold.text = m_csItemCompositionRecipe.Gold.ToString("#,##0");

                    CsUIData.Instance.DisplayButtonInteractable(m_buttonCompose, true);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonAllCompose, false);

                    rtfArrow.anchoredPosition = new Vector2(50, rtfArrow.anchoredPosition.y);
                }
                else
                {
                    trMaterialOwn.gameObject.SetActive(true);

                    if (trItemSlotOwn != null)
                    {
                        trItemSlotOwn.gameObject.SetActive(false);
                    }

                    trMaterialUnOwn.gameObject.SetActive(false);

                    if (trItemSlotResult != null)
                    {
                        trItemSlotResult.gameObject.SetActive(false);
                    }

                    //선택해제
                    UpdateInventorySlotSelected(m_nSelectInventoryListIndex, false);
                    m_nSelectInventoryListIndex = -1;

                    m_textMaterialItemName.text = "";
                    m_textResultItemName.text = "";
                    m_textComposeGold.text = "0";
                    m_textAllComposeGold.text = "0";

                    CsUIData.Instance.DisplayButtonInteractable(m_buttonCompose, false);
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonAllCompose, false);

                    m_csItemCompositionRecipe = null;
                }
            }
        }
    }

    #endregion 합성

    //---------------------------------------------------------------------------------------------------
    void UpdateInventorySlotByInventoryIndex(int nInventoryIndex, bool bSelect, bool bSelectMode = false)
    {
        int nListIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindIndex(a => a.Index == nInventoryIndex);

        UpdateInventorySlotSelected(nListIndex, bSelect, bSelectMode);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInventorySlotSelected(int nInventoryListIndex, bool bSelect, bool bSelectMode = false)
    {
        Transform trInventorySlot = m_trContent.Find("InventorySlot" + nInventoryListIndex);
        if (trInventorySlot != null)
        {
            Transform trItemSlot = trInventorySlot.Find("ItemSlot");
            Transform trCheck = trItemSlot.Find("ImageCheck");

            Button buttonInventorySlot = trInventorySlot.GetComponent<Button>();

            if (bSelect)
            {
                trCheck.gameObject.SetActive(true);

				if (!bSelectMode)
					buttonInventorySlot.interactable = false;
            }
            else
            {
                trCheck.gameObject.SetActive(false);

				if (!bSelectMode)
					buttonInventorySlot.interactable = true;
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	public void OpenComposeToImprovement()
	{
		OpenCompose();
	}
}
