using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-07-02)
//---------------------------------------------------------------------------------------------------

public class CsPopupNpcShop : MonoBehaviour 
{
    GameObject m_goToggleCategory;
    GameObject m_goButtonSaleItem;
    GameObject m_goPopupItemInfo;

    Transform m_trShopCategoryList;
    Transform m_trContent;
    Transform m_trPopupItemInfo;

    Image m_imageGoods;
    Text m_textGoods;

    CsNpcShop m_csNpcShop;

    int m_nSelectShopCategoryId;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventNpcShopProductBuy += OnEventNpcShopProductBuy;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventNpcShopProductBuy -= OnEventNpcShopProductBuy;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNpcShopProductBuy()
    {
        CsNpcShopCategory csNpcShopCategory = m_csNpcShop.NpcShopCategoryList.Find(a => a.CategoryId == m_nSelectShopCategoryId);

        if (csNpcShopCategory == null)
        {
            return;
        }
        else
        {
            UpdateGoods(csNpcShopCategory);
            UpdateButtonSaleItem(csNpcShopCategory);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedShopCategory(bool bIson, CsNpcShopCategory csNpcShopCategory)
    {
        if (bIson)
        {
            m_nSelectShopCategoryId = csNpcShopCategory.CategoryId;
            UpdateGoods(csNpcShopCategory);
            UpdateButtonSaleItem(csNpcShopCategory);

            m_trContent.localPosition = Vector3.zero;
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSaleItem(CsNpcShopProduct csNpcShopProduct)
    {
        CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("A96_TXT_00002"), csNpcShopProduct.Item.Name),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickNpcShopProductBuy(csNpcShopProduct),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemSlot(CsItem csItem)
    {
        if (csItem == null)
        {
            return;
        }
        else
        {
            OpenPopupItemInfo(csItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNpcShopProductBuy(CsNpcShopProduct csNpcShopProduct)
    {
        CsNpcShopCategory csNpcShopCategory = m_csNpcShop.NpcShopCategoryList.Find(a => a.CategoryId == m_nSelectShopCategoryId);

        if (csNpcShopCategory == null)
        {
            return;
        }
        else
        {
            int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csNpcShopCategory.RequiredItem.ItemId);

            if (nCount < csNpcShopProduct.RequiredItemCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A96_TXT_00003"), csNpcShopCategory.RequiredItem.Name));
            }
            else
            {
                CsCommandEventManager.Instance.SendNpcShopProductBuy(csNpcShopProduct.ProductId);
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    public void DisplayPopupNpcShop(int nNpcId)
    {
        InitializeUI(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI(int nNpcId)
    {
        m_csNpcShop = CsGameData.Instance.GetNpcShopByNpcId(nNpcId);

        if (m_csNpcShop == null)
        {
            PopupClose();
        }
        else
        {
            Transform trImageBackground = transform.Find("ImageBackground");

            Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPopupName);
            textPopupName.text = m_csNpcShop.Name;

            Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(OnClickPopupClose);
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Transform trImageGoodsback = trImageBackground.Find("ImageGoodsback");

            m_imageGoods = trImageGoodsback.Find("Image").GetComponent<Image>();

            m_textGoods = trImageGoodsback.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textGoods);

            m_trShopCategoryList = trImageBackground.Find("ShopCategoryList");
            m_trContent = trImageBackground.Find("Scroll View/Viewport/Content");

            StartCoroutine(LoadToggleShopCategory());
        }
    }

    //---------------------------------------------------------------------------------------------------
    void PopupClose()
    {
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadToggleShopCategory()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNpcShop/ToggleShopCategory");
        yield return resourceRequest;
        m_goToggleCategory = (GameObject)resourceRequest.asset;

        InitializeShopCategory();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeShopCategory()
    {
        for (int i = 0; i < m_csNpcShop.NpcShopCategoryList.Count; i++)
        {
            CsNpcShopCategory csNpcShopCategory = m_csNpcShop.NpcShopCategoryList[i];

            if (csNpcShopCategory == null)
            {
                continue;
            }
            else
            {
                Transform trToggleShopCategory = m_trShopCategoryList.Find("ToggleShopCategory" + i);

                if (trToggleShopCategory == null)
                {
                    trToggleShopCategory = Instantiate(m_goToggleCategory, m_trShopCategoryList).transform;
                    trToggleShopCategory.name = "ToggleShopCategory" + i;
                }

                Text textShopCategory = trToggleShopCategory.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textShopCategory);
                textShopCategory.text = csNpcShopCategory.Name;

                Toggle toggleShopCategory = trToggleShopCategory.GetComponent<Toggle>();
                toggleShopCategory.onValueChanged.RemoveAllListeners();

                if (i == 0)
                {
                    toggleShopCategory.isOn = true;
                    m_nSelectShopCategoryId = csNpcShopCategory.CategoryId;
                }
                else
                {
                    toggleShopCategory.isOn = false;
                }

                toggleShopCategory.onValueChanged.AddListener((bIson) => OnValueChangedShopCategory(bIson, csNpcShopCategory));
                toggleShopCategory.group = m_trShopCategoryList.GetComponent<ToggleGroup>();
            }
        }

        InitializeSaleItem();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeSaleItem()
    {
        CsNpcShopCategory csNpcShopCategory = m_csNpcShop.NpcShopCategoryList.Find(a => a.CategoryId == m_nSelectShopCategoryId);

        if (csNpcShopCategory == null)
        {
            PopupClose();
        }
        else
        {
            UpdateGoods(csNpcShopCategory);
            StartCoroutine(LoadButtonSaleItem(csNpcShopCategory));
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadButtonSaleItem(CsNpcShopCategory csNpcShopCategory)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNpcShop/ButtonSaleItem");
        yield return resourceRequest;
        m_goButtonSaleItem = (GameObject)resourceRequest.asset;

        UpdateButtonSaleItem(csNpcShopCategory);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonSaleItem(CsNpcShopCategory csNpcShopCategory)
    {
        for (int i = 0; i < m_trContent.childCount; i++)
        {
            m_trContent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < csNpcShopCategory.NpcShopProductList.Count; i++)
        {
            CsNpcShopProduct csNpcShopProduct = csNpcShopCategory.NpcShopProductList[i];

            Transform trButtonSaleItem = m_trContent.Find("ButtonSaleItem" + i);

            if (trButtonSaleItem == null)
            {
                trButtonSaleItem = Instantiate(m_goButtonSaleItem, m_trContent).transform;
                trButtonSaleItem.name = "ButtonSaleItem" + i;
            }

            Button buttonSaleItem = trButtonSaleItem.GetComponent<Button>();
            buttonSaleItem.onClick.RemoveAllListeners();
            buttonSaleItem.onClick.AddListener(() => OnClickSaleItem(csNpcShopProduct));
            buttonSaleItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Transform trItemSlot = trButtonSaleItem.Find("ItemSlot");
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csNpcShopProduct.Item, csNpcShopProduct.ItemOwned, 0, csNpcShopProduct.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Button buttonItemSlot = trItemSlot.GetComponent<Button>();
            buttonItemSlot.onClick.RemoveAllListeners();
            buttonItemSlot.onClick.AddListener(() => OnClickItemSlot(csNpcShopProduct.Item));
            buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textItemName = trButtonSaleItem.Find("TextItemName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csNpcShopProduct.Item.Name;

            Image imageGoods = trButtonSaleItem.Find("ImageGoods").GetComponent<Image>();
            imageGoods.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csNpcShopCategory.RequiredItem.Image);

            Text textPrice = trButtonSaleItem.Find("TextPrice").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPrice);
            textPrice.text = csNpcShopProduct.RequiredItemCount.ToString("#,##0");

            Text textCount = trButtonSaleItem.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);

            Transform trImageLock = trButtonSaleItem.Find("ImageLock");

            Text textLock = trImageLock.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);
            textLock.text = CsConfiguration.Instance.GetString("A96_TXT_00001");

            if (csNpcShopProduct.LimitCount == 0)
            {
                buttonSaleItem.interactable = true;
                textCount.gameObject.SetActive(false);
                trImageLock.gameObject.SetActive(false);
            }
            else
            {
                int nRemainingBuyCount = -1;
                CsHeroNpcShopProduct csHeroNpcShopProduct = CsGameData.Instance.MyHeroInfo.GetHeroNpcShopProduct(csNpcShopProduct.ProductId);

                if (csHeroNpcShopProduct == null)
                {
                    nRemainingBuyCount = csNpcShopProduct.LimitCount;
                }
                else
                {
                    nRemainingBuyCount = csNpcShopProduct.LimitCount - csHeroNpcShopProduct.BuyCount;
                }

                textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingBuyCount, csNpcShopProduct.LimitCount);

                // 매진
                if (nRemainingBuyCount == 0)
                {
                    buttonSaleItem.interactable = false;
                    trImageLock.gameObject.SetActive(true);
                }
                else
                {
                    buttonSaleItem.interactable = true;
                    trImageLock.gameObject.SetActive(false);
                }
            }

            trButtonSaleItem.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGoods(CsNpcShopCategory csNpcShopCategory)
    {
        m_imageGoods.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csNpcShopCategory.RequiredItem.Image);
        m_textGoods.text = CsGameData.Instance.MyHeroInfo.GetItemCount(csNpcShopCategory.RequiredItem.ItemId).ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csItem)
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(csItem));
        }
        else
        {
            UpdatePopupItemInfo(csItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csItem)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        UpdatePopupItemInfo(csItem);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupItemInfo(CsItem csItem)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, transform);
        m_trPopupItemInfo = goPopupItemInfo.transform;

        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, 0, false, -1, false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;

        Destroy(m_trPopupItemInfo.gameObject);
        m_trPopupItemInfo = null;
    }
}