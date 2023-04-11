using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupDiaShop : CsPopupSub
{
    GameObject m_goProduct;
    GameObject m_goCostumeProduct;
    GameObject m_goPopupCalculator;

    Transform m_trCanvas2;
    Transform m_trToggleList;
    Transform m_trCategoryDiaShopList;
    Transform m_trPopupList;
    Transform m_trPopupCalculator;
    Transform m_tr3DCharacter;

    CsPopupCalculator m_csPopupCalculator;
    IEnumerator m_IEnumeratorLoadPopupCalculator;

    int m_nSelectCategoryId;
    int m_nSelectProductId;
    int m_nSelectCostumeProductType;
    int m_nSelectCostumeId;
    int m_nSelectCostumeEffectId;

    float m_flTime = 0.0f;

    List<CsDiaShopProduct> m_listCategoryDiaShopProduct;

    Camera m_uiCamera;

    enum EnDiaShopCategory
    {
        LimitEdition = 0, 
        Daily = 1, 
        Equipment = 2, 
        Enchant = 3, 
        Special = 4, 
        Costume = 5, 
        Vip = 6, 
    }

    enum EnDiaShopTagType
    {
        Normal = 0, 
        New = 1, 
        Limit = 2, 
        Poppularity = 3, 
        SpecialOffer = 4, 
        BuyCount = 5, 
    }

    enum EnDiaShopMoneyType
    {
        Dia = 1, 
        UnOwnDia = 2, 
        Item = 3, 
    }

    enum EnDiaShopPeriodType
    {
        UnLimited = 0, 
        LimitedTime = 1, 
        LimitedDayOfWeek = 2, 
    }

    enum EnDiaShopCostumeProductType
    {
        Total = 0, 
        Costume = 1, 
        Effect = 2, 
        Mount = 3, 
    }

    enum EnDiaShopBuyLimitType
    {
        Daily = 1, 
        Acc = 2, 
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsGameEventUIToUI.Instance.EventDiaShopProductBuy += OnEventDiaShopProductBuy;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventDiaShopProductBuy -= OnEventDiaShopProductBuy;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + flTime < Time.time)
        {
            if (m_goProduct == null || m_goCostumeProduct == null)
            {
                return;
            }
            else
            {
                UpdateAllCategoryDiaShopProduct();
            }

            m_flTime = Time.time;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventDiaShopProductBuy()
    {
        if (m_goProduct == null || m_goCostumeProduct == null)
        {
            StartCoroutine(LoadDiaShopProduct());
        }
        else
        {
            UpdateAllCategoryDiaShopProduct();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDiaShopCategory(int nDiaShopCategory, bool bIson)
    {
        if (bIson)
        {
            if (nDiaShopCategory == (int)EnDiaShopCategory.Vip)
            {
                CsDiaShopCategory csDiaShopCategory = CsGameData.Instance.GetDiaShopCategory(nDiaShopCategory);

                if (csDiaShopCategory == null)
                {
                    return;
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < csDiaShopCategory.RequiredVipLevel)
                    {
                        // 이전 카테고리로
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A117_TXT_00015"));
                        ResetDisplayCategoryDiaShop();
                    }
                    else
                    {
                        m_nSelectCategoryId = nDiaShopCategory;
                        UpdateCategoryShop(m_nSelectCategoryId);

                        CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
                    }
                }
            }
            else
            {
                m_nSelectCategoryId = nDiaShopCategory;
                UpdateCategoryShop(m_nSelectCategoryId);

                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            }
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDiaShopProductBuy(int nProductId)
    {
        CsDiaShopProduct csDiaShopProduct = CsGameData.Instance.GetDiaShopProduct(nProductId);

        if (csDiaShopProduct == null)
        {
            return;
        }
        else
        {
            m_nSelectProductId = nProductId;

            if (m_IEnumeratorLoadPopupCalculator != null)
            {
                StopCoroutine(m_IEnumeratorLoadPopupCalculator);
                m_IEnumeratorLoadPopupCalculator = null;

                m_IEnumeratorLoadPopupCalculator = LoadPopupBuytItem(csDiaShopProduct);
            }
            else
            {
                m_IEnumeratorLoadPopupCalculator = LoadPopupBuytItem(csDiaShopProduct);
            }

            StartCoroutine(m_IEnumeratorLoadPopupCalculator);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCostumeProductType(int nCostumeProductType, bool bIson)
    {
        if (bIson)
        {
            m_nSelectCostumeProductType = nCostumeProductType;
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            DisplayCostumeShopProduct();
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCostumeProduct(bool bIson, CsDiaShopProduct csDiaShopProduct, ToggleGroup toggleGroup)
    {
        CsItem csItem = csDiaShopProduct.Item;

        if (csItem == null)
        {
            return;
        }
        else
        {
            if (bIson)
            {
                switch (csItem.ItemType.EnItemType)
                {
                    case EnItemType.Mount:

						RemoveMount();
						m_nMountId = csItem.Value1;
						m_nMountLevel = csItem.Value2;

						UpdateCharacterModel(CsGameData.Instance.GetCostume(m_nSelectCostumeId));
						UpdateCharacterModel(CsGameData.Instance.GetCostumeEffect(m_nSelectCostumeEffectId));
						UpdateCharacterModel(m_nMountId, m_nMountLevel);
                        UpdateMountStateCaemraSetting(true);

                        break;
                        
                    case EnItemType.Costume:

                        CsCostume csCostume = CsGameData.Instance.GetCostume(csItem.Value1);

                        if (csCostume == null)
                        {
                            
                        }
                        else
                        {
							RemoveMount();
							m_nSelectCostumeId = csCostume.CostumeId;
                            UpdateCharacterModel(csCostume);
							UpdateCharacterModel(CsGameData.Instance.GetCostumeEffect(m_nSelectCostumeEffectId));
							UpdateCharacterModel(m_nMountId, m_nMountLevel);
						}

                        break;

                    case EnItemType.CostumeEffect:

                        CsCostumeEffect csCostumeEffect = CsGameData.Instance.GetCostumeEffect(csItem.Value1);

                        if (csCostumeEffect == null)
                        {

                        }
                        else
                        {
                            m_nSelectCostumeEffectId = csCostumeEffect.CostumeEffectId;
                            UpdateCharacterModel(csCostumeEffect);
                        }

                        break;
                }
            }
            else
            {
                if (toggleGroup.AnyTogglesOn())
                {
                    return;
                }
                else
                {
                    switch ((EnDiaShopCostumeProductType)csDiaShopProduct.CostumeProductType)
                    {
                        case EnDiaShopCostumeProductType.Costume:

							RemoveMount();
							m_nSelectCostumeId = 0;

                            UpdateCharacterModel(CsGameData.Instance.GetCostume(m_nSelectCostumeId));
							UpdateCharacterModel(CsGameData.Instance.GetCostumeEffect(m_nSelectCostumeEffectId));
							UpdateCharacterModel(m_nMountId, m_nMountLevel);
							break;

                        case EnDiaShopCostumeProductType.Effect:
                            
                            m_nSelectCostumeEffectId = 0;
                            UpdateCharacterModel(CsGameData.Instance.GetCostumeEffect(m_nSelectCostumeEffectId));
                            break;

                        case EnDiaShopCostumeProductType.Mount:

							m_nMountId = 0;
							m_nMountLevel = 0;
							RemoveMount();

							UpdateCharacterModel(CsGameData.Instance.GetCostume(m_nSelectCostumeId));
							UpdateCharacterModel(CsGameData.Instance.GetCostumeEffect(m_nSelectCostumeEffectId));

                            UpdateMountStateCaemraSetting(false);

							break;
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickResetModel()
    {
        UpdateToggleIsOnDiaShopProduct();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;

        Transform trVipInfo = transform.Find("VipInfo");

        Text textVipLevel = trVipInfo.Find("TextVip").GetComponent<Text>();
        CsUIData.Instance.SetFont(textVipLevel);
        textVipLevel.text = string.Format(CsConfiguration.Instance.GetString("A117_TXT_00002"), CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel);

        m_trToggleList = transform.Find("ToggleList");
        Transform trToggle = null;

        trToggle = m_trToggleList.Find("Toggle" + (int)EnDiaShopCategory.LimitEdition);

        if (trToggle == null)
        {

        }
        else
        {
            Text textCategoryName = trToggle.Find("Label").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCategoryName);
            textCategoryName.text = CsConfiguration.Instance.GetString("A117_TXT_00001");

            Toggle toggleCategory = trToggle.GetComponent<Toggle>();
            toggleCategory.onValueChanged.RemoveAllListeners();
            toggleCategory.isOn = true;
            toggleCategory.onValueChanged.AddListener((ison) => OnValueChangedDiaShopCategory((int)EnDiaShopCategory.LimitEdition, ison));
            toggleCategory.interactable = false;
        }

        List<CsDiaShopCategory> listDiaShopCategory = CsGameData.Instance.DiaShopCategoryList.OrderBy(a => a.SortNo).ToList();

        for (int i = 0; i < listDiaShopCategory.Count; i++)
        {
            CsDiaShopCategory csDiaShopCategory = listDiaShopCategory[i];
            trToggle = m_trToggleList.Find("Toggle" + csDiaShopCategory.CategoryId);

            if (trToggle == null || csDiaShopCategory == null)
            {
                continue;
            }
            else
            {
                Text textCategoryName = trToggle.Find("Label").GetComponent<Text>();
                CsUIData.Instance.SetFont(textCategoryName);
                textCategoryName.text = csDiaShopCategory.Name;

                Toggle toggleCategory = trToggle.GetComponent<Toggle>();
                toggleCategory.onValueChanged.RemoveAllListeners();
                toggleCategory.onValueChanged.AddListener((ison) => OnValueChangedDiaShopCategory(csDiaShopCategory.CategoryId, ison));
                toggleCategory.interactable = false;
            }
        }

        m_trCategoryDiaShopList = transform.Find("CategoryDiaShopList");

        for (int i = 0; i < m_trCategoryDiaShopList.childCount; i++)
        {
            m_trCategoryDiaShopList.GetChild(i).gameObject.SetActive(false);

            if (m_trCategoryDiaShopList.GetChild(i).name == "Costume")
            {
                m_tr3DCharacter = m_trCategoryDiaShopList.GetChild(i).Find("3DCharacter");
                m_uiCamera = m_tr3DCharacter.Find("UIChar_Camera").GetComponent<Camera>();
            }
            else
            {
                continue;
            }
        } 
        
        m_nSelectCategoryId = (int)EnDiaShopCategory.LimitEdition;
        StartCoroutine(LoadDiaShopProduct());

        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDiaShopProduct()
    {
        ResourceRequest resourceRequestDiaShopProduct = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDiaStore/DiaShopProduct");
        yield return resourceRequestDiaShopProduct;

        m_goProduct = (GameObject)resourceRequestDiaShopProduct.asset;

        ResourceRequest resourceRequestCostumeProduct = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDiaStore/CostumeProduct");
        yield return resourceRequestCostumeProduct;

        m_goCostumeProduct = (GameObject)resourceRequestCostumeProduct.asset;

        UpdateCategoryShop(m_nSelectCategoryId);

        for (int i = 0; i < m_trToggleList.childCount; i++)
        {
            Toggle toggle = m_trToggleList.GetChild(i).GetComponent<Toggle>();

            if (toggle == null)
            {
                continue;
            }
            else
            {
                toggle.interactable = true;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCategoryShop(int nCategoryId)
    {
        CsDiaShopCategory csDiaShopCategory = CsGameData.Instance.GetDiaShopCategory(nCategoryId);

        Transform trCategoryShop = null;

        for (int i = 0; i < m_trCategoryDiaShopList.childCount; i++)
        {
            m_trCategoryDiaShopList.GetChild(i).gameObject.SetActive(false);
        }

        if (csDiaShopCategory == null && nCategoryId == (int)EnDiaShopCategory.LimitEdition)
        {
            // 리밋 에디션
            trCategoryShop = m_trCategoryDiaShopList.Find("LimitEdition");

            Transform trRecommand = trCategoryShop.Find("Recommand");

            Text textRecommand = trRecommand.Find("RecommandTitle/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRecommand);
            textRecommand.text = CsConfiguration.Instance.GetString("A117_TXT_00017");

            Transform trRecommandItemList = trRecommand.Find("RecommandList");
            Transform trRecommandItem = null;

            List<CsDiaShopProduct> listRecommandDiaShopProduct = CsGameData.Instance.GetDiaShopProductListByLimitEdition().FindAll(a => a.Recommended == true).OrderBy(a => a.LimitEditionSortNo).ToList();
            
            for (int i = 0; i < trRecommandItemList.childCount; i++)
            {
                trRecommandItem = trRecommandItemList.GetChild(i);

                if (i < listRecommandDiaShopProduct.Count)
                {
                    UpdateDiaShopProductTag(trRecommandItem, listRecommandDiaShopProduct[i]);
                    UpdateDiaShopProductBuyCount(trRecommandItem, listRecommandDiaShopProduct[i]);
                    UpdateDiaShopProductMoneyType(trRecommandItem, listRecommandDiaShopProduct[i]);
                    UpdateDiaShopProductSale(trRecommandItem, listRecommandDiaShopProduct[i]);
                    UpdateDiaShopProductRemainTime(trRecommandItem, listRecommandDiaShopProduct[i]);

                    Image imageItemIcon = trRecommandItem.Find("ImageItem").GetComponent<Image>();
                    imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + listRecommandDiaShopProduct[i].Item.Image);

                    Text textItemName = trRecommandItem.Find("ItemDetail/TextItem").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textItemName);
                    textItemName.text = listRecommandDiaShopProduct[i].Item.Name;

                    int nProductId = listRecommandDiaShopProduct[i].ProductId;

                    Button buttonBuyItem = trRecommandItem.Find("ButtonBuy").GetComponent<Button>();
                    buttonBuyItem.onClick.RemoveAllListeners();
                    buttonBuyItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    buttonBuyItem.onClick.AddListener(() => OnClickDiaShopProductBuy(nProductId));

                    Text textRequiredMoneyTypeValue = buttonBuyItem.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRequiredMoneyTypeValue);
                    textRequiredMoneyTypeValue.text = listRecommandDiaShopProduct[i].Price.ToString("#,##0");

                    trRecommandItem.gameObject.SetActive(true);
                }
                else
                {
                    trRecommandItem.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            switch ((EnDiaShopCategory)nCategoryId)
            {
                case EnDiaShopCategory.Daily:

                    trCategoryShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");

                    break;

                case EnDiaShopCategory.Equipment:

                    trCategoryShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");

                    break;

                case EnDiaShopCategory.Enchant:

                    trCategoryShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");

                    break;

                case EnDiaShopCategory.Special:

                    trCategoryShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");

                    break;

                case EnDiaShopCategory.Costume:

                    trCategoryShop = m_trCategoryDiaShopList.Find("Costume");

                    Button buttonReset = trCategoryShop.Find("ButtonReset").GetComponent<Button>();
                    buttonReset.onClick.RemoveAllListeners();
                    buttonReset.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    buttonReset.onClick.AddListener(OnClickResetModel);

                    Transform trToggleList = trCategoryShop.Find("ToggleList");

                    for (int i = 0; i < trToggleList.childCount; i++)
                    {
                        Transform trToggleCostumeProductType = trToggleList.GetChild(i);
                        Toggle toggleCostumeProductType = trToggleCostumeProductType.GetComponent<Toggle>();

                        if (toggleCostumeProductType == null)
                        {
                            continue;
                        }
                        else
                        {
                            Text textCostumeProductType = trToggleCostumeProductType.Find("Label").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textCostumeProductType);

                            switch ((EnDiaShopCostumeProductType)i)
                            {
                                case EnDiaShopCostumeProductType.Total:
                                    textCostumeProductType.text = CsConfiguration.Instance.GetString("A117_TXT_00010");
                                    break;

                                case EnDiaShopCostumeProductType.Costume:
                                    textCostumeProductType.text = CsConfiguration.Instance.GetString("A117_TXT_00011");
                                    break;

                                case EnDiaShopCostumeProductType.Effect:
                                    textCostumeProductType.text = CsConfiguration.Instance.GetString("A117_TXT_00012");
                                    break;

                                case EnDiaShopCostumeProductType.Mount:
                                    textCostumeProductType.text = CsConfiguration.Instance.GetString("A117_TXT_00013");
                                    break;
                            }

                            if (i == 0)
                            {
                                m_nSelectCostumeProductType = i;
                                toggleCostumeProductType.isOn = true;
                            }
                            else
                            {
                                toggleCostumeProductType.isOn = false;
                            }

                            int nCostumeProductType = i;
                            toggleCostumeProductType.onValueChanged.RemoveAllListeners();
                            toggleCostumeProductType.onValueChanged.AddListener((ison) => OnValueChangedCostumeProductType(nCostumeProductType, ison));
                        }
                    }

                    LoadCharacterModel();

                    break;

                case EnDiaShopCategory.Vip:

                    trCategoryShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");

                    break;
            }
        }

        DisplayDiaShopProduct(trCategoryShop);
        trCategoryShop.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCostumeShopProduct()
    {
        Transform trCategoryShop = m_trCategoryDiaShopList.Find("Costume");
        DisplayDiaShopProduct(trCategoryShop);
    }

    //---------------------------------------------------------------------------------------------------
    void ResetDisplayCategoryDiaShop()
    {
        UpdateCategoryShop(m_nSelectCategoryId);

        Toggle toggleCategory = null;

        toggleCategory = m_trToggleList.Find("Toggle" + m_nSelectCategoryId).GetComponent<Toggle>();
        toggleCategory.isOn = true;

        toggleCategory = m_trToggleList.Find("Toggle" + (int)EnDiaShopCategory.Vip).GetComponent<Toggle>();
        toggleCategory.isOn = false;
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayDiaShopProduct(Transform trCategoryDiaShop)
    {
        CsDiaShopCategory csDiaShopCategory = CsGameData.Instance.GetDiaShopCategory(m_nSelectCategoryId);

        Transform trContent = trCategoryDiaShop.Find("Scroll View/Viewport/Content");
        Transform trDiaShopProduct = null;

        if (csDiaShopCategory == null)
        {
            if (CsGameData.Instance.GetDiaShopProductListByLimitEdition() == null)
            {
                return;
            }
            else
            {
                m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductListByLimitEdition().FindAll(a => a.Recommended == false).OrderBy(a => a.LimitEditionSortNo).ToList();
            }
        }
        else if (csDiaShopCategory.CategoryId == (int)EnDiaShopCategory.Costume)
        {
            if (CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId) == null)
            {
                return;
            }
            else
            {
                if (m_nSelectCostumeProductType == (int)EnDiaShopCostumeProductType.Total)
                {
                    m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId).OrderBy(a => a.CategorySortNo).ToList();
                }
                else
                {
                    m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId).FindAll(a => a.CostumeProductType == m_nSelectCostumeProductType).OrderBy(a => a.CategorySortNo).ToList();
                }
            }
        }
        else
        {
            if (CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId) == null)
            {
                return;
            }
            else
            {
                m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId).OrderBy(a => a.CategorySortNo).ToList();
            }
        }

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        Toggle toggleDiaShopProduct = null;
        Transform trToggleList = m_trCategoryDiaShopList.Find("Costume/ToggleList");

        for (int i = 0; i < m_listCategoryDiaShopProduct.Count; i++)
        {
            CsDiaShopProduct csDiaShopProduct = m_listCategoryDiaShopProduct[i];
            trDiaShopProduct = trContent.Find("DiaShopProduct" + csDiaShopProduct.ProductId);

            if (trDiaShopProduct == null)
            {
                if (m_nSelectCategoryId == (int)EnDiaShopCategory.Costume)
                {
                    ToggleGroup toggleGroup = trToggleList.Find("Toggle" + m_listCategoryDiaShopProduct[i].CostumeProductType).GetComponent<ToggleGroup>();

                    trDiaShopProduct = Instantiate(m_goCostumeProduct, trContent).transform;
                    toggleDiaShopProduct = trDiaShopProduct.Find("ToggleCostumeProduct").GetComponent<Toggle>();
                    toggleDiaShopProduct.onValueChanged.RemoveAllListeners();
                    toggleDiaShopProduct.isOn = false;
                    toggleDiaShopProduct.onValueChanged.AddListener((ison) => OnValueChangedCostumeProduct(ison, csDiaShopProduct, toggleGroup));
                    toggleDiaShopProduct.group = toggleGroup;
                }
                else
                {
                    trDiaShopProduct = Instantiate(m_goProduct, trContent).transform;
                }

                trDiaShopProduct.name = "DiaShopProduct" + csDiaShopProduct.ProductId;
            }
            else
            {
                trDiaShopProduct.gameObject.SetActive(true);
            }

            UpdateDiaShopProduct(trDiaShopProduct, csDiaShopProduct);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAllCategoryDiaShopProduct()
    {
        Transform trCategoryDiaShop = null;
        Transform trContent = null;
        Transform trDiaShopProduct = null;

        switch ((EnDiaShopCategory)m_nSelectCategoryId)
        {
            case EnDiaShopCategory.LimitEdition:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("LimitEdition");

                Transform trRecommandItemList = trCategoryDiaShop.Find("Recommand/RecommandList");
                Transform trRecommandItem = null;

                List<CsDiaShopProduct> listRecommandDiaShopProduct = CsGameData.Instance.GetDiaShopProductListByLimitEdition().FindAll(a => a.Recommended == true).OrderBy(a => a.LimitEditionSortNo).ToList();
                
                for (int i = 0; i < trRecommandItemList.childCount; i++)
                {
                    trRecommandItem = trRecommandItemList.GetChild(i);

                    if (i < listRecommandDiaShopProduct.Count)
                    {
                        UpdateDiaShopProductBuyCount(trRecommandItem, listRecommandDiaShopProduct[i]);
                        UpdateDiaShopProductRemainTime(trRecommandItem, listRecommandDiaShopProduct[i]);
                    }
                    else
                    {
                        continue;
                    }
                }

                break;

            case EnDiaShopCategory.Daily:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");
                break;

            case EnDiaShopCategory.Equipment:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");
                break;

            case EnDiaShopCategory.Enchant:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");
                break;

            case EnDiaShopCategory.Special:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");
                break;

            case EnDiaShopCategory.Costume:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("Costume");
                break;

            case EnDiaShopCategory.Vip:
                trCategoryDiaShop = m_trCategoryDiaShopList.Find("CategoryDiaShop");
                break;
        }

        trContent = trCategoryDiaShop.Find("Scroll View/Viewport/Content");

        if (m_nSelectCategoryId == (int)EnDiaShopCategory.LimitEdition)
        {
            if (CsGameData.Instance.GetDiaShopProductListByLimitEdition() == null)
            {
                return;
            }
            else
            {
                m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductListByLimitEdition().FindAll(a => a.Recommended == false).OrderBy(a => a.LimitEditionSortNo).ToList();
            }
        }
        else if (m_nSelectCategoryId == (int)EnDiaShopCategory.Costume)
        {
            if (CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId) == null)
            {
                return;
            }
            else
            {
                if (m_nSelectCostumeProductType == (int)EnDiaShopCostumeProductType.Total)
                {
                    m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId).OrderBy(a => a.CategorySortNo).ToList();
                }
                else
                {
                    m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId).FindAll(a => a.CostumeProductType == m_nSelectCostumeProductType).OrderBy(a => a.CategorySortNo).ToList();
                }
            }
        }
        else
        {
            if (CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId) == null)
            {
                return;
            }
            else
            {
                m_listCategoryDiaShopProduct = CsGameData.Instance.GetDiaShopProductList(m_nSelectCategoryId).OrderBy(a => a.CategorySortNo).ToList();
            }
        }

        Toggle toggleDiaShopProduct = null;
        Transform trToggleList = m_trCategoryDiaShopList.Find("Costume/ToggleList");

        for (int i = 0; i < m_listCategoryDiaShopProduct.Count; i++)
        {
            CsDiaShopProduct csDiaShopProduct = m_listCategoryDiaShopProduct[i];

            trDiaShopProduct = trContent.Find("DiaShopProduct" + csDiaShopProduct.ProductId);

            if (trDiaShopProduct == null)
            {
                if (m_nSelectCategoryId == (int)EnDiaShopCategory.Costume)
                {
                    ToggleGroup toggleGroup = trToggleList.Find("Toggle" + csDiaShopProduct.CostumeProductType).GetComponent<ToggleGroup>();

                    trDiaShopProduct = Instantiate(m_goCostumeProduct, trContent).transform;
                    toggleDiaShopProduct = trDiaShopProduct.Find("ToggleCostumeProduct").GetComponent<Toggle>();
                    toggleDiaShopProduct.onValueChanged.RemoveAllListeners();
                    toggleDiaShopProduct.isOn = false;
                    toggleDiaShopProduct.onValueChanged.AddListener((ison) => OnValueChangedCostumeProduct(ison, csDiaShopProduct, toggleGroup));
                    toggleDiaShopProduct.group = toggleGroup;
                }
                else
                {
                    trDiaShopProduct = Instantiate(m_goProduct, trContent).transform;
                }

                trDiaShopProduct.name = "DiaShopProduct" + csDiaShopProduct.ProductId;
            }
            else
            {
                trDiaShopProduct.gameObject.SetActive(true);
            }

            UpdateDiaShopProductRemainTime(trDiaShopProduct, csDiaShopProduct);
            UpdateDiaShopProductBuyCount(trDiaShopProduct, csDiaShopProduct);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDiaShopProduct(Transform trDiaShopProduct, CsDiaShopProduct csDiaShopProduct)
    {
        UpdateDiaShopProductTag(trDiaShopProduct, csDiaShopProduct);
        UpdateDiaShopProductBuyCount(trDiaShopProduct, csDiaShopProduct);
        UpdateDiaShopProductRemainTime(trDiaShopProduct, csDiaShopProduct);
        UpdateDiaShopProductMoneyType(trDiaShopProduct, csDiaShopProduct);
        UpdateDiaShopProductSale(trDiaShopProduct, csDiaShopProduct);

        Image imageItem = trDiaShopProduct.Find("ImageItem").GetComponent<Image>();
        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csDiaShopProduct.Item.Image);

        Text textItemName = trDiaShopProduct.Find("ItemDetail/TextItem").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemName);
        textItemName.text = csDiaShopProduct.Item.Name;

        Button buttonBuy = trDiaShopProduct.Find("ButtonBuy").GetComponent<Button>();
        buttonBuy.onClick.RemoveAllListeners();
        buttonBuy.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonBuy.onClick.AddListener(() => OnClickDiaShopProductBuy(csDiaShopProduct.ProductId));

        Text textRequiredMoneyTypeValue = buttonBuy.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRequiredMoneyTypeValue);
        textRequiredMoneyTypeValue.text = csDiaShopProduct.Price.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDiaShopProductTag(Transform trDiaShopProduct, CsDiaShopProduct csDiaShopProduct)
    {
        Image imageTag = trDiaShopProduct.Find("ImageTag").GetComponent<Image>();
        imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDiaStore/frm_mark_" + csDiaShopProduct.TagType);

        Text textTag = imageTag.transform.Find("TextTag").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTag);

        switch ((EnDiaShopTagType)csDiaShopProduct.TagType)
        {
            // 기본
            case EnDiaShopTagType.Normal:
                imageTag.gameObject.SetActive(false);
                break;

            // 신품
            case EnDiaShopTagType.New:
                textTag.text = CsConfiguration.Instance.GetString("A117_TXT_00003");
                imageTag.gameObject.SetActive(true);
                break;

            // 한정
            case EnDiaShopTagType.Limit:
                textTag.text = CsConfiguration.Instance.GetString("A117_TXT_00004");
                imageTag.gameObject.SetActive(true);
                break;

            // 인기
            case EnDiaShopTagType.Poppularity:
                textTag.text = CsConfiguration.Instance.GetString("A117_TXT_00005");
                imageTag.gameObject.SetActive(true);
                break;

            // 특가
            case EnDiaShopTagType.SpecialOffer:
                textTag.text = CsConfiguration.Instance.GetString("A117_TXT_00006");
                imageTag.gameObject.SetActive(true);
                break;

            // 구매 제한
            case EnDiaShopTagType.BuyCount:
                textTag.text = CsConfiguration.Instance.GetString("A117_TXT_00007");
                imageTag.gameObject.SetActive(true);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDiaShopProductBuyCount(Transform trDiaShopProduct, CsDiaShopProduct csDiaShopProduct)
    {
        Button buttonBuy = trDiaShopProduct.Find("ButtonBuy").GetComponent<Button>();

        Text textBuyCount = trDiaShopProduct.Find("TextBuyCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBuyCount);

        if (csDiaShopProduct.BuyLimitCount == 0)
        {
            textBuyCount.gameObject.SetActive(false);
            CsUIData.Instance.DisplayButtonInteractable(buttonBuy, true);
        }
        else
        {
            int nBuyCount = 0;
            CsHeroDiaShopProductBuyCount csHeroDiaShopProductBuyCount = null;

            switch ((EnDiaShopBuyLimitType)csDiaShopProduct.BuyLimitType)
            {
                case EnDiaShopBuyLimitType.Daily:

                    csHeroDiaShopProductBuyCount = CsGameData.Instance.MyHeroInfo.HeroDiaShopProductBuyCountList.Find(a => a.ProductId == csDiaShopProduct.ProductId);

                    break;

                case EnDiaShopBuyLimitType.Acc:

                    csHeroDiaShopProductBuyCount = CsGameData.Instance.MyHeroInfo.TotalHeroDiaShopProductBuyCountList.Find(a => a.ProductId == csDiaShopProduct.ProductId);

                    break;
            }

            if (csHeroDiaShopProductBuyCount == null)
            {
                nBuyCount = 0;
            }
            else
            {
                nBuyCount = csHeroDiaShopProductBuyCount.BuyCount;
            }

            textBuyCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), (csDiaShopProduct.BuyLimitCount - nBuyCount), csDiaShopProduct.BuyLimitCount);
            textBuyCount.gameObject.SetActive(true);

            // 구매 횟수를 모두 구입하면 버튼 비활성화
            if (nBuyCount == csDiaShopProduct.BuyLimitCount)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonBuy, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonBuy, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDiaShopProductRemainTime(Transform trDiaShopProduct, CsDiaShopProduct csDiaShopProduct)
    {
        Transform trRemainTime = trDiaShopProduct.Find("ItemDetail/RemainTime");

        Text textRemainTime = trRemainTime.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRemainTime);

        // 기간 한정 상품인지
        int nSecond = 0;
        System.TimeSpan tsRemainTime = new System.TimeSpan();

        switch ((EnDiaShopPeriodType)csDiaShopProduct.PeriodType)
        {
            case EnDiaShopPeriodType.UnLimited:

                trRemainTime.gameObject.SetActive(false);

                break;

            case EnDiaShopPeriodType.LimitedTime:
                
                nSecond = (int)csDiaShopProduct.PeriodEndTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime).TotalSeconds;

                if (nSecond <= 0)
                {
                    trDiaShopProduct.gameObject.SetActive(false);
                }
                else
                {
                    tsRemainTime = System.TimeSpan.FromSeconds(nSecond);
                    int nTotalHours = tsRemainTime.Days * 24 + tsRemainTime.Hours;
                    textRemainTime.text = string.Format(CsConfiguration.Instance.GetString("A117_TXT_00008"), nTotalHours.ToString("000"), tsRemainTime.Minutes.ToString("00"), tsRemainTime.Seconds.ToString("00"));
                    trRemainTime.gameObject.SetActive(true);
                }

                break;

            case EnDiaShopPeriodType.LimitedDayOfWeek:

                if ((int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek != csDiaShopProduct.PeriodDayOfWeek)
                {
                    trDiaShopProduct.gameObject.SetActive(false);
                }
                else
                {
                    nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.AddDays(1).Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime).TotalSeconds;

                    if (nSecond <= 0)
                    {
                        trDiaShopProduct.gameObject.SetActive(false);
                    }
                    else
                    {
                        tsRemainTime = System.TimeSpan.FromSeconds(nSecond);
                        textRemainTime.text = string.Format(CsConfiguration.Instance.GetString("A117_TXT_00008"), tsRemainTime.Hours.ToString("000"), tsRemainTime.Minutes.ToString("00"), tsRemainTime.Seconds.ToString("00"));
                        trRemainTime.gameObject.SetActive(true);
                    }
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDiaShopProductMoneyType(Transform trDiaShopProduct, CsDiaShopProduct csDiaShopProduct)
    {
        Image imageMoneyType = trDiaShopProduct.Find("ButtonBuy/Image").GetComponent<Image>();

        switch ((EnDiaShopMoneyType)csDiaShopProduct.MoneyType)
        {
            case EnDiaShopMoneyType.Dia:
                imageMoneyType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods01");
                break;

            case EnDiaShopMoneyType.UnOwnDia:
                imageMoneyType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods02");
                break;

            case EnDiaShopMoneyType.Item:
                imageMoneyType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csDiaShopProduct.MoneyItem.Image);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDiaShopProductSale(Transform trDiaShopProduct, CsDiaShopProduct csDiaShopProduct)
    {
        Text textItemSale = trDiaShopProduct.Find("ItemDetail/TextSale").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemSale);

        // 특가 상품인지
        if (csDiaShopProduct.OriginalPrice == 0)
        {
            textItemSale.gameObject.SetActive(false);
        }
        else
        {
            textItemSale.text = string.Format(CsConfiguration.Instance.GetString("A117_TXT_00009"), csDiaShopProduct.OriginalPrice.ToString("#,##0"));
            textItemSale.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckAddItemAvailable(CsDiaShopProduct csDiaShopProduct, int nItemCount)
    {
        if (nItemCount <= CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(csDiaShopProduct.Item.ItemId, csDiaShopProduct.ItemOwned))
        {
            return true;
        }
        else
        {
            if ((CsGameData.Instance.MyHeroInfo.InventorySlotList.Count) < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleIsOnDiaShopProduct()
    {
        Transform trContent = transform.Find("CategoryDiaShopList/Costume/Scroll View/Viewport/Content");

        if (trContent == null)
        {
            return;
        }
        else
        {
            Toggle toggleDiaShopProduct = null;

            for (int i = 0; i < trContent.childCount; i++)
            {
                toggleDiaShopProduct = trContent.GetChild(i).GetComponent<Toggle>();

                if (toggleDiaShopProduct == null)
                {
                    continue;
                }
                else
                {
                    if (toggleDiaShopProduct.isOn)
                    {
                        toggleDiaShopProduct.isOn = false;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }

    #region PopupCalculator

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupBuytItem(CsDiaShopProduct csDiaShopProduct)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        OpenPopupBuyItem(csDiaShopProduct);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupBuyItem(CsDiaShopProduct csDiaShopProduct)
    {
        m_trPopupCalculator = m_trPopupList.Find("PopupCalculator");

        if (m_trPopupCalculator == null)
        {
            GameObject goPopupBuyCount = Instantiate(m_goPopupCalculator, m_trPopupList);
            goPopupBuyCount.name = "PopupCalculator";
            m_trPopupCalculator = goPopupBuyCount.transform;
        }
        else
        {
            m_trPopupCalculator.gameObject.SetActive(false);
        }

        m_csPopupCalculator = m_trPopupCalculator.GetComponent<CsPopupCalculator>();
        m_csPopupCalculator.EventBuyItem += OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;

        switch ((EnDiaShopMoneyType)csDiaShopProduct.MoneyType)
        {
            case EnDiaShopMoneyType.Dia:
                m_csPopupCalculator.DisplayItem(csDiaShopProduct.Item, csDiaShopProduct.ItemOwned, csDiaShopProduct.Price, EnResourceType.OwnDia);
                break;

            case EnDiaShopMoneyType.UnOwnDia:
                m_csPopupCalculator.DisplayItem(csDiaShopProduct.Item, csDiaShopProduct.ItemOwned, csDiaShopProduct.Price, EnResourceType.UnOwnDia);
                break;

            case EnDiaShopMoneyType.Item:
                m_csPopupCalculator.DisplayItem(csDiaShopProduct.Item, csDiaShopProduct.ItemOwned, csDiaShopProduct.Price, EnResourceType.Item, csDiaShopProduct.MoneyItem.ItemId);
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
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBuyItem(int nCount)
    {
        if (nCount == 0)
        {
            return;
        }
        else
        {
            CheckBuyDiaShopProduct(m_nSelectProductId, nCount);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckBuyDiaShopProduct(int nProductId, int nCount)
    {
        CsDiaShopProduct csDiaShopProduct = CsGameData.Instance.GetDiaShopProduct(nProductId);

        if (csDiaShopProduct == null)
        {
            return;
        }
        else
        {
            int nBuyCount = 0;
            CsHeroDiaShopProductBuyCount csHeroDiaShopProductBuyCount = null; 

            switch ((EnDiaShopBuyLimitType)csDiaShopProduct.BuyLimitType)
            {
                case EnDiaShopBuyLimitType.Daily:           // 일일
                    csHeroDiaShopProductBuyCount = CsGameData.Instance.MyHeroInfo.HeroDiaShopProductBuyCountList.Find(a => a.ProductId == csDiaShopProduct.ProductId);
                    break;

                case EnDiaShopBuyLimitType.Acc:             // 누적
                    csHeroDiaShopProductBuyCount = CsGameData.Instance.MyHeroInfo.TotalHeroDiaShopProductBuyCountList.Find(a => a.ProductId == csDiaShopProduct.ProductId);
                    break;
            }

            if (csHeroDiaShopProductBuyCount == null)
            {
                nBuyCount = 0;
            }
            else
            {
                nBuyCount = csHeroDiaShopProductBuyCount.BuyCount;
            }

            // 수량 초과
            if (csDiaShopProduct.BuyLimitCount != 0 && (csDiaShopProduct.BuyLimitCount - nBuyCount) < nCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A117_TXT_00014"));
            }
            else
            {
                if (CheckAddItemAvailable(csDiaShopProduct, nCount))
                {
                    switch ((EnDiaShopMoneyType)csDiaShopProduct.MoneyType)
                    {
                        case EnDiaShopMoneyType.Dia:
                            if (CsGameData.Instance.MyHeroInfo.Dia < csDiaShopProduct.Price)
                            {
                                // 재화 부족
                            }
                            else
                            {
                                CsCommandEventManager.Instance.SendDiaShopProductBuy(csDiaShopProduct.ProductId, nCount);
                            }
                            break;

                        case EnDiaShopMoneyType.UnOwnDia:
                            if (CsGameData.Instance.MyHeroInfo.UnOwnDia < csDiaShopProduct.Price)
                            {
                                // 재화 부족
                            }
                            else
                            {
                                CsCommandEventManager.Instance.SendDiaShopProductBuy(csDiaShopProduct.ProductId, nCount);
                            }
                            break;

                        case EnDiaShopMoneyType.Item:
                            if (CsGameData.Instance.MyHeroInfo.GetItemCount(csDiaShopProduct.MoneyItem.ItemId) < csDiaShopProduct.Price)
                            {
                                // 재화 부족
                            }
                            else
                            {
                                CsCommandEventManager.Instance.SendDiaShopProductBuy(csDiaShopProduct.ProductId, nCount);
                            }
                            break;
                    }
                }
                else
                {
                    // 인벤토리가 가득 참
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A117_TXT_00016"));
                }
            }
        }
    }

    #endregion PopupCalculator

    #region 3DCharacterModel

    //---------------------------------------------------------------------------------------------------
    void LoadCharacterModel()		//캐릭터모델 동적로드함수
    {
		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;
		Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

        if (trCharacterModel == null)
        {
            StartCoroutine(LoadCharacterModelCoroutine());
        }
        else
        {
            trCharacterModel.gameObject.SetActive(true);
            UpdateCharacterModel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCharacterModelCoroutine()
    {
		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Common/Character" + nJobId);
        yield return resourceRequest;
        GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, m_tr3DCharacter);

        float flScale = 1 / m_trCanvas2.GetComponent<RectTransform>().localScale.x;

        switch (nJobId)
        {
            case (int)EnJob.Gaia:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Asura:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 185, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Deva:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 175, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Witch:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
        }

        goCharacter.GetComponent<CsUICharcterRotate>().UICamera = m_uiCamera;
        goCharacter.name = "Character" + nJobId;
        goCharacter.gameObject.SetActive(true);

        UpdateCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterModel()
    {
		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

		//Debug.Log("UpdateCharacterModel()");
        Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

		if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
			CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

			csEquipment.MidChangeEquipments(csHeroCustomData, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterModel(CsCostume csCostume)
    {
		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

		Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

		if (trCharacterModel != null)
        {
			CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
            CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

            CsHeroCostume csHeroCostume = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId);
            CsCostumeEffect csCostumeEffect = null;

            if (m_nSelectCostumeId == 0)
            {
                if (csHeroCostume == null)
                {
                    csCostume = null;
                }
                else
                {
					csCostume = csHeroCostume.Costume;
                }
            }

            if (csHeroCostume == null)
            {
                csCostumeEffect = null;
            }
            else
            {
                if (m_nSelectCostumeEffectId == 0)
                {
                    csCostumeEffect = csHeroCostume.CostumeEffect;
                }
                else
                {
                    csCostumeEffect = CsGameData.Instance.GetCostumeEffect(m_nSelectCostumeEffectId);
                }
            }

            if (csCostume == null)
            {
                if (csCostumeEffect == null)
                {
                    csHeroCustomData.SetCostum(0, 0);
                }
                else
                {
                    csHeroCustomData.SetCostum(0, csCostumeEffect.CostumeEffectId);
                }
            }
            else
            {
                if (csCostumeEffect == null)
                {
                    csHeroCustomData.SetCostum(csCostume.CostumeId, 0);
                }
                else
                {
                    csHeroCustomData.SetCostum(csCostume.CostumeId, csCostumeEffect.CostumeEffectId);
                }
            }

			//Debug.Log("UpdateCharacterModel >> CsCostume()  " + csHeroCustomData.CostumeId);
			csEquipment.MidChangeEquipments(csHeroCustomData, false);
        }
        else
        {
			if (csCostume != null)
			{
				//Debug.Log("UpdateCharacterModUpdateCharacterModel(CsCostume()  " + csCostume.CostumeId);
			}
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterModel(CsCostumeEffect csCostumeEffect)
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

        if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
            CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

            CsHeroCostume csHeroCostume = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId);
            CsCostume csCostume = null;

            if (m_nSelectCostumeEffectId == 0)
            {
                if (csHeroCostume == null)
                {
                    csCostumeEffect = null;
                }
                else
                {
                    csCostumeEffect = csHeroCostume.CostumeEffect;
                }
            }

            if (csHeroCostume == null)
            {
                csCostume = null;
            }
            else
            {
                if (m_nSelectCostumeId == 0)
                {
                    csCostume = csHeroCostume.Costume;
                }
                else
                {
                    csCostume = CsGameData.Instance.GetCostume(m_nSelectCostumeId);
                }
            }

            if (csCostume == null)
            {
                if (csCostumeEffect == null)
                {
                    csHeroCustomData.SetCostum(0, 0);
                }
                else
                {
                    csHeroCustomData.SetCostum(0, csCostumeEffect.CostumeEffectId);
                }
            }
            else
            {
                if (csCostumeEffect == null)
                {
                    csHeroCustomData.SetCostum(csCostume.CostumeId, 0);
                }
                else
                {
                    csHeroCustomData.SetCostum(csCostume.CostumeId, csCostumeEffect.CostumeEffectId);
                }
            }

			csEquipment.CreateCostumeEffect(CsGameData.Instance.CostumeEffectList.Find(a => a.CostumeEffectId == csHeroCustomData.CostumeEffectId));
        }
        else
        {
            return;
        }
    }

	GameObject m_goMount = null;
	int m_nMountId = 0;
	int m_nMountLevel = 0;

	//---------------------------------------------------------------------------------------------------
	void UpdateCharacterModel(int nMountId, int nMountLevel)
    {
		//Debug.Log("1. UpdateCharacterModel()  " + nMountId + " , " + nMountLevel);
		RemoveMount();

		if (nMountId == 0) return;

		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

		Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

		if (trCharacterModel != null)
		{
			Transform trPivot = trCharacterModel.Find("Bip001");
			if (trPivot == null)
			{
				trPivot = trCharacterModel.Find("Bip01");
			}

			m_goMount = CreateMount(nMountId, nMountLevel, trCharacterModel);

			if (m_goMount != null)
			{
				trCharacterModel.GetComponent<Animator>().SetBool("Riding", true);
				m_goMount.transform.position = trCharacterModel.position;
				m_goMount.transform.eulerAngles = trCharacterModel.eulerAngles;

				for (int i = 0; i < m_goMount.transform.childCount; i++)
				{
					m_goMount.transform.GetChild(i).gameObject.layer = trCharacterModel.gameObject.layer;
				}

				m_goMount.SetActive(true);
				trPivot.SetParent(m_goMount.transform.Find("Ride01"));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	GameObject CreateMount(int nMountId, int nMountLevel, Transform trParent)
	{
		CsMount csMount = CsGameData.Instance.GetMount(nMountId);

		if (csMount == null) return null;

		CsMountQuality csMountQuality = csMount.GetMountQuality(csMount.GetMountLevel(nMountLevel).MountLevelMaster.MountQualityMaster.Quality);

		if (csMountQuality != null)
		{
			GameObject goMount = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/MountObject/" + csMountQuality.PrefabName), trParent) as GameObject;
			if (goMount != null)
			{
				goMount.name = csMountQuality.PrefabName;
				goMount.SetActive(false);
			}
			return goMount;
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveMount()
	{
		if (m_goMount != null)
		{
			//Debug.Log("1. RemoveMount()");
			int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

			Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

			Transform trPivot = m_goMount.transform.Find("Ride01/Bip001");
			if (trPivot == null)
			{
				trPivot = m_goMount.transform.Find("Ride01/Bip01");
			}

			trPivot.SetParent(trCharacterModel); // 윈래 위치로 본 이동.
			m_goMount.transform.SetParent(transform);

			GameObject.Destroy(m_goMount);
			m_goMount = null;

			trCharacterModel.GetComponent<Animator>().SetBool("Riding", false);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateMountStateCaemraSetting(bool bMount)
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;
        Transform trCharacterModel = m_tr3DCharacter.Find("Character" + nJobId);

        Transform trCamera = m_uiCamera.transform.GetComponent<Transform>();

        if (bMount)
        {
            trCharacterModel.transform.eulerAngles = new Vector3(0, 150, 0);

            trCamera.localPosition = new Vector3(0, 90, -220);
            trCamera.eulerAngles = new Vector3(4.5f, 0, 0);
        }
        else
        {
            switch (nJobId)
            {
                case (int)EnJob.Gaia:
                    trCharacterModel.eulerAngles = new Vector3(0, 180, 0);
                    break;

                case (int)EnJob.Asura:
                    trCharacterModel.eulerAngles = new Vector3(0, 185, 0);
                    break;

                case (int)EnJob.Deva:
                    trCharacterModel.eulerAngles = new Vector3(0, 175, 0);
                    break;

                case (int)EnJob.Witch:
                    trCharacterModel.eulerAngles = new Vector3(0, 180, 0);
                    break;
            }

            trCamera.localPosition = new Vector3(-4.5f, 90, -100);
            trCamera.eulerAngles = new Vector3(9.8f, 0, 0);
        }
    }

	#endregion 3DCharacterModel
}