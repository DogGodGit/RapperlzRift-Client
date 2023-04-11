using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsPopupCardShop : CsPopupSub
{
    Transform m_trPanelShopList;
    Transform m_trPopupDiaRefresh;

    bool m_bFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsCreatureCardManager.Instance.EventCreatureCardShopFixedProductBuy += OnEventCreatureCardShopFixedProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardShopPaidRefresh += OnEventCreatureCardShopPaidRefresh;
        CsCreatureCardManager.Instance.EventCreatureCardShopRandomProductBuy += OnEventCreatureCardShopRandomProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardShopRefreshed += OnEventCreatureCardShopRefreshed;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsCreatureCardManager.Instance.EventCreatureCardShopFixedProductBuy -= OnEventCreatureCardShopFixedProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardShopPaidRefresh -= OnEventCreatureCardShopPaidRefresh;
        CsCreatureCardManager.Instance.EventCreatureCardShopRandomProductBuy -= OnEventCreatureCardShopRandomProductBuy;
        CsCreatureCardManager.Instance.EventCreatureCardShopRefreshed -= OnEventCreatureCardShopRefreshed;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
        }
        else
        {
            UpdateFixedProduct();
            UpdateRandomProduct();
        }
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickFixedProductBuy(int nProductId)
    {
        CsCreatureCardManager.Instance.SendCreatureCardShopFixedProductBuy(nProductId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRandomProductBuy(int nProductId)
    {
        CsCreatureCardManager.Instance.SendCreatureCardShopRandomProductBuy(nProductId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRefreshPopup()
    {
        m_trPopupDiaRefresh.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRefresh(bool bIson)
    {
        if (bIson)
        {
            int nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.CreatureCardShopPaidRefreshMaxCount;
            int nCount = nMaxCount - CsCreatureCardManager.Instance.DailyCreatureCardShopPaidRefreshCount;

            if (CsGameData.Instance.MyHeroInfo.Dia >= CsGameConfig.Instance.CreatureCardShopPaidRefreshDia && nCount > 0)
            {
                CsCreatureCardManager.Instance.SendCreatureCardShopPaidRefresh();
            }
        }

        m_trPopupDiaRefresh.gameObject.SetActive(false);
    }

    #endregion Event Handler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopFixedProductBuy()
    {
        UpdateFixedProduct();
        UpdateRandomProduct();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A26_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopPaidRefresh()
    {
        UpdateFixedProduct();
        UpdateRandomProduct();
        UpdateBottom();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopRandomProductBuy()
    {
        UpdateFixedProduct();
        UpdateRandomProduct();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A26_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardShopRefreshed()
    {
        UpdateFixedProduct();
        UpdateRandomProduct();
        UpdateBottom();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPanelShopList = transform.Find("PanelShopList");

        UpdateFixedProduct();
        UpdateRandomProduct();
        UpdateBottom();
        InitializePopupDiarefresh();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupDiarefresh()
    {
        m_trPopupDiaRefresh = transform.Find("PopupDiaRefresh");

        Transform trModal = m_trPopupDiaRefresh.Find("Modal");

        Text textMessage = trModal.Find("TextMessage").GetComponent<Text>();
        textMessage.text = CsConfiguration.Instance.GetString("A26_TXT_00004");
        CsUIData.Instance.SetFont(textMessage);

        Text textDia = trModal.Find("TextDia").GetComponent<Text>();
        textDia.text = CsGameConfig.Instance.CreatureCardShopPaidRefreshDia.ToString("#,##0");
        CsUIData.Instance.SetFont(textDia);

        Button buttonNo = trModal.Find("Buttons/ButtonNo").GetComponent<Button>();
        buttonNo.onClick.RemoveAllListeners();
        buttonNo.onClick.AddListener(() => OnClickRefresh(false));
        buttonNo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNo = buttonNo.transform.Find("Text").GetComponent<Text>();
        textNo.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
        CsUIData.Instance.SetFont(textNo);

        Button buttonYes = trModal.Find("Buttons/ButtonYes").GetComponent<Button>();
        buttonYes.onClick.RemoveAllListeners();
        buttonYes.onClick.AddListener(() => OnClickRefresh(true));
        buttonYes.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textYes = buttonYes.transform.Find("Text").GetComponent<Text>();
        textYes.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
        CsUIData.Instance.SetFont(textYes);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateFixedProduct()
    {
        List<CsCreatureCardShopFixedProduct> listFixedProduct = CsGameData.Instance.CreatureCardShopFixedProductList;

        for (int i = 0; i < listFixedProduct.Count; ++i)
        {
            int nProductId = listFixedProduct[i].ProductId;
            Transform trProduct = m_trPanelShopList.Find("FixedProduct" + nProductId);

            Image imageIcon = trProduct.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + listFixedProduct[i].Item.Image);

            Text textName = trProduct.Find("TextName").GetComponent<Text>();
            textName.text = listFixedProduct[i].Item.Name;
            CsUIData.Instance.SetFont(textName);

            Text textSoldOut = trProduct.Find("TextSoldOut").GetComponent<Text>();
            textSoldOut.text = CsConfiguration.Instance.GetString("A26_TXT_00003");
            CsUIData.Instance.SetFont(textSoldOut);

            Button buttonBuy = trProduct.Find("ButtonBuy").GetComponent<Button>();
            buttonBuy.onClick.RemoveAllListeners();
            buttonBuy.onClick.AddListener(() => OnClickFixedProductBuy(nProductId));
            buttonBuy.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textValue = buttonBuy.transform.Find("Text").GetComponent<Text>();
            textValue.text = listFixedProduct[i].SaleSoulPowder.ToString("#,##0");
            CsUIData.Instance.SetFont(textValue);

            //구매한 상태
            if (CsCreatureCardManager.Instance.PurchasedCreatureCardShopFixedProductList.Find(a => a == listFixedProduct[i].ProductId) != 0)
            {
                buttonBuy.gameObject.SetActive(false);
                textSoldOut.gameObject.SetActive(true);
            }
            else if (CsGameData.Instance.MyHeroInfo.SoulPowder < listFixedProduct[i].SaleSoulPowder)
            {
                buttonBuy.gameObject.SetActive(true);
                textSoldOut.gameObject.SetActive(false);
                CsUIData.Instance.DisplayButtonInteractable(buttonBuy, false);
            }
            else
            {
                buttonBuy.gameObject.SetActive(true);
                textSoldOut.gameObject.SetActive(false);
                CsUIData.Instance.DisplayButtonInteractable(buttonBuy, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateBottom()
    {
        int nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.CreatureCardShopPaidRefreshMaxCount;
        int nCount = nMaxCount - CsCreatureCardManager.Instance.DailyCreatureCardShopPaidRefreshCount;

        Transform trPanelBottom = transform.Find("PanelBottom");

        Text textNext = trPanelBottom.Find("ImageTime/TextNext").GetComponent<Text>();
        textNext.text = CsConfiguration.Instance.GetString("A26_TXT_00001");
        CsUIData.Instance.SetFont(textNext);

        Text textTime = trPanelBottom.Find("ImageTime/TextTime").GetComponent<Text>();
        textTime.text = CsCreatureCardManager.Instance.CreatureCardShopPaidRefreshCountDate.ToString("hh:mm:ss");
        CsUIData.Instance.SetFont(textTime);

        Button buttonRefresh = trPanelBottom.Find("ButtonRefresh").GetComponent<Button>();
        buttonRefresh.onClick.RemoveAllListeners();
        buttonRefresh.onClick.AddListener(OnClickRefreshPopup);
        buttonRefresh.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textRefresh = buttonRefresh.transform.Find("Text").GetComponent<Text>();
        textRefresh.text = CsConfiguration.Instance.GetString("A26_BTN_00001");
        CsUIData.Instance.SetFont(textRefresh);

        Text textDia = buttonRefresh.transform.Find("TextDia").GetComponent<Text>();
        textDia.text = CsGameConfig.Instance.CreatureCardShopPaidRefreshDia.ToString("#,##0");
        CsUIData.Instance.SetFont(textDia);

        Image imageDia = buttonRefresh.transform.Find("Image").GetComponent<Image>();

        if (CsGameData.Instance.MyHeroInfo.Dia >= CsGameConfig.Instance.CreatureCardShopPaidRefreshDia
            && CsCreatureCardManager.Instance.DailyCreatureCardShopPaidRefreshCount < CsGameData.Instance.MyHeroInfo.VipLevel.CreatureCardShopPaidRefreshMaxCount)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonRefresh, true);
            textDia.color = CsUIData.Instance.ColorWhite;
            imageDia.color = new Color(1, 1, 1, 1);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonRefresh, false);
            textDia.color = new Color32(133, 141, 148, 255);
            imageDia.color = new Color(1, 1, 1, 0.3f);
        }

        Text textCount = trPanelBottom.Find("TextCount").GetComponent<Text>();
        textCount.text = string.Format(CsConfiguration.Instance.GetString("A26_TXT_01001"), nCount, nMaxCount);
        CsUIData.Instance.SetFont(textCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRandomProduct()
    {
        List<CsHeroCreatureCardShopRandomProduct> listRandomProduct = CsCreatureCardManager.Instance.HeroCreatureCardShopRandomProductList;

        for (int i = 0; i < listRandomProduct.Count; ++i)
        {
            int nProductId = listRandomProduct[i].CreatureCardShopRandomProduct.ProductId;
            Transform trProduct = m_trPanelShopList.Find("RandomProduct" + i);
            CsUIData.Instance.DisplayItemSlot(trProduct.Find("ItemSlot"), listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard, false);

            Text textName = trProduct.Find("TextName").GetComponent<Text>();
            textName.text = string.Format("<color={0}>{1}</color>", listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard.CreatureCardGrade.ColorCode, listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard.Name);
            CsUIData.Instance.SetFont(textName);

            Text textSoldOut = trProduct.Find("TextSoldOut").GetComponent<Text>();
            textSoldOut.text = CsConfiguration.Instance.GetString("A26_TXT_00003");
            CsUIData.Instance.SetFont(textSoldOut);

            Button buttonBuy = trProduct.Find("ButtonBuy").GetComponent<Button>();
            buttonBuy.onClick.RemoveAllListeners();
            buttonBuy.onClick.AddListener(() => OnClickRandomProductBuy(nProductId));
            buttonBuy.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textValue = buttonBuy.transform.Find("Text").GetComponent<Text>();
            textValue.text = listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard.CreatureCardGrade.SaleSoulPowder.ToString("#,##0");
            CsUIData.Instance.SetFont(textValue);

            if (listRandomProduct[i].Purchased)
            {
                buttonBuy.gameObject.SetActive(false);
                textSoldOut.gameObject.SetActive(true);
            }
            else if (CsGameData.Instance.MyHeroInfo.SoulPowder < listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard.CreatureCardGrade.SaleSoulPowder)
            {
                buttonBuy.gameObject.SetActive(true);
                textSoldOut.gameObject.SetActive(false);
                CsUIData.Instance.DisplayButtonInteractable(buttonBuy, false);
            }
            else
            {
                buttonBuy.gameObject.SetActive(true);
                textSoldOut.gameObject.SetActive(false);
                CsUIData.Instance.DisplayButtonInteractable(buttonBuy, true);
            }

            Transform trNeed = trProduct.Find("ImageNeed");
            trNeed.gameObject.SetActive(false);
            Text textNeed = trNeed.Find("Text").GetComponent<Text>();
            textNeed.text = CsConfiguration.Instance.GetString("A26_TXT_00002");
            CsUIData.Instance.SetFont(textNeed);

            List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCreatureCard(listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard.CreatureCardId);

            int nNeedCount = 0;
            int nUesCount = 0;

            for (int j = 0; j < listEntry.Count; ++j)
            {
                if (!CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listEntry[j].CreatureCardCollection.CollectionId))
                {
                    nNeedCount++;
                }
            }

            CsHeroCreatureCard csHeroCreatureCard = CsCreatureCardManager.Instance.GetHeroCreatureCard(listRandomProduct[i].CreatureCardShopRandomProduct.CreatureCard.CreatureCardId);

            if (csHeroCreatureCard != null)
            {
                nUesCount = csHeroCreatureCard.Count;
            }

            if (nNeedCount > nUesCount)
            {
                trNeed.gameObject.SetActive(true);
            }
        }
    }
}
