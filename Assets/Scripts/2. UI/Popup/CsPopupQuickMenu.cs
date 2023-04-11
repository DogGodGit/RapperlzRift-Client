using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CsPopupQuickMenu : MonoBehaviour
{
    [SerializeField] GameObject m_goItemSlot;
    GameObject m_goPopupCalculator;

    Transform m_trPopupList;
    Transform m_trMenuList;
    Transform m_trContent;
    
    Image m_imageCooltime;
    Text m_textNoItem;
    
    int m_nSelectQuickMenu;

    CsPopupCalculator m_csPopupCalculator;
    EnItemType m_EnItemTypeBuy;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;

        CsGameEventUIToUI.Instance.EventReturnScrollUseStart += OnEventReturnScrollUseStart;
        CsGameEventUIToUI.Instance.EventFishingBaitUse += OnEventFishingBaitUse;
        CsGameEventUIToUI.Instance.EventBountyHunterQuestScrollUse += OnEventBountyHunterQuestScrollUse;
        CsGameEventUIToUI.Instance.EventDistortionScrollUse += OnEventDistortionScrollUse;

        CsGameEventUIToUI.Instance.EventDropObjectLooted += OnEventDropObjectLooted;
        CsGameEventUIToUI.Instance.EventSimpleShopBuy += OnEventSimpleShopBuy;

        InitializeUI();
    }

    void Update()
    {
        if (CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.ReturnScroll) > 0)
        {
            if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup > 0)
            {
                UpdateReturnScrollCoolTime();
            }
            else
            {
                CsUIData.Instance.ReturnScrollRemainingCoolTime = 0;
            }
        }
        else
        {
            if (m_imageCooltime == null)
            {
                return;
            }
            else
            {
                m_imageCooltime.fillAmount = 1;
            }
        }

        if (CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup > 0)
        {
            UpdateReturnScrollCastTime();
        }
        else
        {
            CsUIData.Instance.ReturnScrollRemainingCastTime = 0;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;

        CsGameEventUIToUI.Instance.EventReturnScrollUseStart -= OnEventReturnScrollUseStart;
        CsGameEventUIToUI.Instance.EventFishingBaitUse -= OnEventFishingBaitUse;
        CsGameEventUIToUI.Instance.EventBountyHunterQuestScrollUse -= OnEventBountyHunterQuestScrollUse;
        CsGameEventUIToUI.Instance.EventDistortionScrollUse -= OnEventDistortionScrollUse;

        CsGameEventUIToUI.Instance.EventDropObjectLooted -= OnEventDropObjectLooted;
        CsGameEventUIToUI.Instance.EventSimpleShopBuy -= OnEventSimpleShopBuy;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
		QuickMenuClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseStart()
    {
        QuickMenuClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingBaitUse()
    {
        QuickMenuClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestScrollUse()
    {
        QuickMenuClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDistortionScrollUse()
    {
        QuickMenuClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDropObjectLooted(List<CsDropObject> listLooted, List<CsDropObject> listNotLooted)
    {
        if (listLooted.Count > 0)
        {
            if (m_goItemSlot == null)
            {
                StartCoroutine(LoadItemSlot());
            }
            else
            {
                UpdateItemList();
            }

            for (int i = 0; i < CsGameData.Instance.QuickMenuList.Count; i++)
            {
                UpdateQuickMenu(CsGameData.Instance.QuickMenuList[i].MenuId);
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSimpleShopBuy()
    {
        if (m_goItemSlot == null)
        {
            StartCoroutine(LoadItemSlot());
        }
        else
        {
            UpdateItemList();
        }

        UpdateQuickMenu(m_nSelectQuickMenu);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQuickMenuClose()
    {
        QuickMenuClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemSlot(EnItemType enItemType, CsInventorySlot csInventorySlot)
    {
        switch (enItemType)
        {
            case EnItemType.ReturnScroll:

                if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup <= 0 && CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup <= 0)
                {
                    if (csInventorySlot != null)
                    {
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                        {
                            CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                        }
                        else
                        {
                            QuickMenuClose();
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                break;

            case EnItemType.BountyHunter:

                if (CsBountyHunterQuestManager.Instance.BountyHunterQuest == null)
                {
                    if (CsGameConfig.Instance.BountyHunterQuestMaxCount > CsBountyHunterQuestManager.Instance.BountyHunterQuestDailyStartCount)
                    {
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                        {
                            CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                        }
                        else
                        {
                            QuickMenuClose();
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                        }
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

            case EnItemType.FishingBait:

                if (CsFishingQuestManager.Instance.BaitItemId == 0)
                {
                    if (CsFishingQuestManager.Instance.FishingQuestDailyStartCount < CsGameData.Instance.FishingQuest.LimitCount)
                    {
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                        {
                            CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                        }
                        else
                        {
                            QuickMenuClose();
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                        }
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
                            if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                            {
                                CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                            }
                            else
                            {
                                QuickMenuClose();
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                            }
                        }
                    }
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A55_TXT_02002"));
                }

                break;

            default:
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedQuickMenu(bool bIson, int nIndex)
    {
        if (bIson)
        {
            m_nSelectQuickMenu = nIndex;

            if (m_goItemSlot == null)
            {
                StartCoroutine(LoadItemSlot());
            }
            else
            {
                UpdateItemList();
            }

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickToggleQuickMenu(Toggle toggle)
    {
        if (!toggle.isOn)
        {
            return;
        }
        else
        {
            if (GetTotalItemCount(m_nSelectQuickMenu) == 0)
            {
                CsQuickMenu csQuickMenu = CsGameData.Instance.QuickMenuList.Find(a => a.MenuId == m_nSelectQuickMenu);

                if (csQuickMenu == null)
                {
                    return;
                }
                else
                {
                    switch ((EnItemType)csQuickMenu.ItemType)
                    {
                        case EnItemType.ReturnScroll:
                            CsSimpleShopProduct csSimpleShopProduct = CsGameData.Instance.SimpleShopProductList.Find(a => a.Item.ItemId == CsUIData.Instance.ReturnScrollId);

                            if (csSimpleShopProduct != null)
                            {
                                m_EnItemTypeBuy = EnItemType.ReturnScroll;
                                OpenShop(csSimpleShopProduct);
                            }

                            break;

                        case EnItemType.FishingBait:
                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
                            QuickMenuClose();
                            break;

                        case EnItemType.BountyHunter:
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
                            break;

                        case EnItemType.DistortionScroll:
                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
                            QuickMenuClose();
                            break;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickQuickMenuClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trImageBackground = transform.Find("ImageBackground");

        m_trMenuList = trImageBackground.Find("MenuList");

        for (int i = 0; i < CsGameData.Instance.QuickMenuList.Count; i++)
        {
            Toggle toggleQuickMenu = m_trMenuList.Find("Toggle" + CsGameData.Instance.QuickMenuList[i].MenuId).GetComponent<Toggle>();
            toggleQuickMenu.onValueChanged.RemoveAllListeners();

            if (toggleQuickMenu == null)
            {
                continue;
            }
            else
            {
                if (i == 0)
                {
                    toggleQuickMenu.isOn = true;
                    m_nSelectQuickMenu = CsGameData.Instance.QuickMenuList[i].MenuId;
                }
                else
                {
                    toggleQuickMenu.isOn = false;
                }

                int nIndex = CsGameData.Instance.QuickMenuList[i].MenuId;
                toggleQuickMenu.onValueChanged.AddListener((bIson) => OnValueChangedQuickMenu(bIson, nIndex));

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => OnClickToggleQuickMenu(toggleQuickMenu));

                toggleQuickMenu.GetComponent<EventTrigger>().triggers.Add(entry);

                Text textBuy = toggleQuickMenu.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textBuy);
                textBuy.text = CsConfiguration.Instance.GetString("A02_BTN_00010");
            }

            UpdateQuickMenu(CsGameData.Instance.QuickMenuList[i].MenuId);
        }

        m_trContent = trImageBackground.Find("Scroll View/Viewport/Content");

        m_textNoItem = trImageBackground.Find("TextNoItem").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoItem);
        m_textNoItem.text = CsConfiguration.Instance.GetString("A98_TXT_00001");

        if (m_goItemSlot == null)
        {
            StartCoroutine(LoadItemSlot());
        }
        else
        {
            UpdateItemList();
        }

        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateItemList()
    {
        int nTotalCount = 0;
        List<CsItem> listCsItem = new List<CsItem>();

        CsQuickMenu csQuickMenu = CsGameData.Instance.QuickMenuList.Find(a => a.MenuId == m_nSelectQuickMenu);

        if (csQuickMenu == null)
        {
            return;
        }
        else
        {
            switch ((EnItemType)csQuickMenu.ItemType)
            {
                case EnItemType.ReturnScroll:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.ReturnScroll);
                    break;

                case EnItemType.FishingBait:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.FishingBait);
                    break;

                case EnItemType.BountyHunter:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.BountyHunter);
                    break;

                case EnItemType.DistortionScroll:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.DistortionScroll);
                    break;
            }
        }

        

        for (int i = 0; i < m_trContent.childCount; i++)
        {
            m_trContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trItemSlot = null;
        int nCount = 0;

        for (int i = 0; i < listCsItem.Count; i++)
        {
            int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(listCsItem[i].ItemId);

            if (0 < nItemCount && listCsItem[i].RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level < listCsItem[i].RequiredMaxHeroLevel)
            {
                trItemSlot = m_trContent.Find("ItemSlot" + nCount);

                if (trItemSlot == null)
                {
                    trItemSlot = Instantiate(m_goItemSlot, m_trContent).transform;
                    trItemSlot.name = "ItemSlot" + nCount;
                }

                if (m_imageCooltime == null && listCsItem[i].ItemType.EnItemType == EnItemType.ReturnScroll)
                {
                    m_imageCooltime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                }

                CsUIData.Instance.DisplayItemSlot(trItemSlot, listCsItem[i], false, nItemCount, listCsItem[i].UsingRecommendationEnabled, EnItemSlotSize.Small, false);
                trItemSlot.gameObject.SetActive(true);

                nTotalCount += nItemCount;
                nCount++;

                EnItemType enItemType = listCsItem[i].ItemType.EnItemType;
                CsInventorySlot csInventorySlot  = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(listCsItem[i].ItemId);

                Button buttonItemSlot = trItemSlot.GetComponent<Button>();
                buttonItemSlot.onClick.RemoveAllListeners();
                buttonItemSlot.onClick.AddListener(() => OnClickItemSlot(enItemType, csInventorySlot));
                buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                trItemSlot.GetComponent<LayoutElement>().preferredWidth = 64;
                trItemSlot.GetComponent<LayoutElement>().preferredHeight = 64;
            }
            else
            {
                continue;
            }
        }

        if (nTotalCount == 0)
        {
            m_textNoItem.gameObject.SetActive(true);
        }
        else
        {
            m_textNoItem.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadItemSlot()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("");
        yield return resourceRequest;
        m_goItemSlot = (GameObject)resourceRequest.asset;

        UpdateItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateReturnScrollCoolTime()
    {
        if (m_imageCooltime == null)
            return;

        CsQuickMenu csQuickMenu = CsGameData.Instance.QuickMenuList.Find(a => a.MenuId == m_nSelectQuickMenu);

        if (csQuickMenu == null)
        {
            return;
        }
        else
        {
            if (csQuickMenu.ItemType == (int)EnItemType.ReturnScroll)
            {
                float flRemainingTime = CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup;

                if (flRemainingTime < 0)
                    flRemainingTime = 0;

                m_imageCooltime.fillAmount = flRemainingTime / CsUIData.Instance.ReturnScrollCoolTime;
            }
            else
            {
                m_imageCooltime.fillAmount = 0.0f;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateReturnScrollCastTime()
    {
        float flRemainingTime = CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup;

        if (flRemainingTime < 0)
            flRemainingTime = 0;
    }

    //---------------------------------------------------------------------------------------------------
    int GetTotalItemCount(int nQuickMenu)
    {
		int nTotalCount = 0;
        List<CsItem> listCsItem = new List<CsItem>();

        CsQuickMenu csQuickMenu = CsGameData.Instance.QuickMenuList.Find(a => a.MenuId == nQuickMenu);

        if (csQuickMenu == null)
        {
            return 0;
        }
        else
        {
			switch ((EnItemType)csQuickMenu.ItemType)
            {
                case EnItemType.ReturnScroll:
					listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.ReturnScroll);
                    break;

                case EnItemType.FishingBait:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.FishingBait);
                    break;

                case EnItemType.BountyHunter:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.BountyHunter);
                    break;

                case EnItemType.DistortionScroll:
                    listCsItem = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.DistortionScroll);
                    break;
            }

            for (int i = 0; i < listCsItem.Count; i++)
            {
                int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(listCsItem[i].ItemId);

                if (0 < nItemCount && listCsItem[i].RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level < listCsItem[i].RequiredMaxHeroLevel)
                {
                    nTotalCount += nItemCount;
                }
                else
                {
                    continue;
                }
            }
        }

        return nTotalCount;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateQuickMenu(int nQuickMenu)
    {
        Transform trQuickMenu = m_trMenuList.Find("Toggle" + nQuickMenu);

        Image imageCooltime = trQuickMenu.Find("ImageCooltime").GetComponent<Image>();
        Text textBuy = trQuickMenu.Find("Text").GetComponent<Text>();

        if (GetTotalItemCount(nQuickMenu) == 0)
        {
            imageCooltime.fillAmount = 1.0f;
            textBuy.gameObject.SetActive(true);
        }
        else
        {
            imageCooltime.fillAmount = 0.0f;
            textBuy.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void QuickMenuClose()
    {
        Destroy(transform.gameObject);
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
}