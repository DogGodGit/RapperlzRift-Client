using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsPopupCardInventory : CsPopupSub
{
    GameObject m_goCreatureCard;
    GameObject m_goCardInfoCollection;

    Transform m_trPopupList;
    Transform m_trCardContent;
    Transform m_trCardInfo;
    Transform m_trCardInfoCollectionContent;
    ScrollRect m_srCard;

    Toggle m_toggleAll;
    Toggle m_toggleMyCardCheck;

    bool m_bMyCard = true;
    bool m_bFirst = true;

    int m_nCategoryId = 0;
    int m_nCardInfoId = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsCreatureCardManager.Instance.EventCreatureCardDisassemble += OnEventCreatureCardDisassemble;
        CsCreatureCardManager.Instance.EventCreatureCardDisassembleAll += OnEventCreatureCardDisassembleAll;
        CsCreatureCardManager.Instance.EventCreatureCardCompose += OnEventCreatureCardCompose;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsCreatureCardManager.Instance.EventCreatureCardDisassemble -= OnEventCreatureCardDisassemble;
        CsCreatureCardManager.Instance.EventCreatureCardDisassembleAll -= OnEventCreatureCardDisassembleAll;
        CsCreatureCardManager.Instance.EventCreatureCardCompose -= OnEventCreatureCardCompose;
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
            m_toggleAll.isOn = true;
            m_toggleMyCardCheck.isOn = true;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (!m_bFirst)
        {
            if (m_nCategoryId != 0)
            {
                Toggle toggle = transform.Find("PanelCategory/CardCategoryList/" + m_nCategoryId).GetComponent<Toggle>();
                toggle.isOn = false;
            }
            else
            {
                m_toggleAll.isOn = false;
            }
        }
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnValueChengedCategory(bool bIson, int nCategoryId)
    {
        if (bIson)
        {
            m_nCategoryId = nCategoryId;
            DisplayCardList();
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChengedCardCheck(bool bIson)
    {
        m_bMyCard = bIson;
        DisplayCardList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupDisassembleOpen()
    {
        //구매 팝업 오픈.
        if (m_goPopupCalculator == null)
        {
            StartCoroutine(LoadPopupDisassembleCoroutine());
        }
        else
        {
            OpenPopupDisassemble();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDisassembleAll()
    {
        CsCreatureCardManager.Instance.SendCreatureCardDisassembleAll(m_nCategoryId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupCardInfoOpen(CsCreatureCard csCreatureCard)
    {
        DisplayCardInfo(csCreatureCard);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupCardInfoClose()
    {
        m_trCardInfo.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCompose()
    {
        if (CsGameData.Instance.MyHeroInfo.SoulPowder < CsGameData.Instance.GetCreatureCard(m_nCardInfoId).CreatureCardGrade.CompositionSoulPowder)
        {

        }
        else if (!CsGameData.Instance.MyHeroInfo.VipLevel.CreatureCardCompositionEnabled)
        {
            int nVipLevel = 0;

            //가능 레벨
            for (int i = 0; i < CsGameData.Instance.VipLevelList.Count; ++i)
            {
                if (CsGameData.Instance.VipLevelList[i].CreatureCardCompositionEnabled)
                {
                    nVipLevel = CsGameData.Instance.VipLevelList[i].VipLevel;
                    break;
                }
            }

            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A25_TXT_02002"), nVipLevel));
        }
        else
        {
            CsCreatureCardManager.Instance.SendCreatureCardCompose(m_nCardInfoId);
        }
    }

    bool m_bIsLoad = false;
    int m_nCardLoadIndex = 0;
    List<CsCreatureCard> m_listCard;
    int m_nStandardPosition = 0;

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCardScrollbar(Scrollbar scrollbar)
    {
        if (!m_bIsLoad)
        {
            m_bIsLoad = true;

            if (m_nCardLoadIndex < m_listCard.Count)
            {
                int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, m_listCard.Count, (1 - scrollbar.value))) * 4;

                if (nUpdateLine > m_nStandardPosition)
                {
                    int nStartCount = m_nCardLoadIndex;
                    int nEndCount = nUpdateLine;

                    if (nEndCount >= m_listCard.Count)
                    {
                        nEndCount = m_listCard.Count;
                    }

                    for (int i = nStartCount; i < nEndCount; i++)
                    {
                        CreateCard(m_listCard[i]);
                    }

                    m_nStandardPosition = nUpdateLine;
                }
            }

            m_bIsLoad = false;
        }
    }

    #endregion Event Handler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardDisassemble(int nAcquiredSoulPowder)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A25_TXT_02003"), nAcquiredSoulPowder));
        CsCreatureCard csCreatureCard = CsGameData.Instance.GetCreatureCard(m_nCardInfoId);
        UpdateCard(csCreatureCard);
        DisplayCardInfo(csCreatureCard);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardDisassembleAll(int nAcquiredSoulPowder)
    {
        if (nAcquiredSoulPowder == 0)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A25_TXT_02005"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A25_TXT_02004"), nAcquiredSoulPowder));
            DisplayCardList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardCompose()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A25_TXT_02001"));
        CsCreatureCard csCreatureCard = CsGameData.Instance.GetCreatureCard(m_nCardInfoId);
        UpdateCard(csCreatureCard);
        DisplayCardInfo(csCreatureCard);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        m_srCard = transform.Find("PanelCard/Scroll View").GetComponent<ScrollRect>();
        m_trCardContent = m_srCard.transform.Find("Viewport/Content");
        Transform trCardCategoryList = transform.Find("PanelCategory/CardCategoryList");
        GameObject goCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CreatureCardCategory");

        ToggleGroup toggleGroup = trCardCategoryList.GetComponent<ToggleGroup>();
        m_toggleAll = trCardCategoryList.Find("ToggleAll").GetComponent<Toggle>();
        m_toggleAll.group = toggleGroup;
        m_toggleAll.onValueChanged.RemoveAllListeners();
        m_toggleAll.isOn = true;
        m_toggleAll.onValueChanged.AddListener((ison) => OnValueChengedCategory(ison, 0));

        Text textAll = m_toggleAll.transform.Find("Text").GetComponent<Text>();
        textAll.text = CsConfiguration.Instance.GetString("A25_BTN_00001");
        CsUIData.Instance.SetFont(textAll);

        List<CsCreatureCardCategory> listCategory = CsGameData.Instance.CreatureCardCategoryList;

        for (int i = 0; i < listCategory.Count; ++i)
        {
            int nCategoryId = listCategory[i].CategoryId;

            Toggle toggleCategory = Instantiate(goCategory, trCardCategoryList).GetComponent<Toggle>();
            toggleCategory.name = nCategoryId.ToString();
            toggleCategory.group = toggleGroup;
            toggleCategory.onValueChanged.RemoveAllListeners();
            toggleCategory.onValueChanged.AddListener((ison) => OnValueChengedCategory(ison, nCategoryId));

            Text textCategory = toggleCategory.transform.Find("Text").GetComponent<Text>();
            textCategory.text = listCategory[i].Name;
            CsUIData.Instance.SetFont(textCategory);
        }

        DisplayCardList();

        //Bottom
        Transform trBottom = transform.Find("PanelBottom");

        m_toggleMyCardCheck = trBottom.Find("ToggleCardCheck").GetComponent<Toggle>();
        m_toggleMyCardCheck.onValueChanged.RemoveAllListeners();
        m_toggleMyCardCheck.isOn = m_bMyCard;
        m_toggleMyCardCheck.onValueChanged.AddListener(OnValueChengedCardCheck);
        m_toggleMyCardCheck.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textCardCheck = m_toggleMyCardCheck.transform.Find("Text").GetComponent<Text>();
        textCardCheck.text = CsConfiguration.Instance.GetString("A25_TXT_00001");
        CsUIData.Instance.SetFont(textCardCheck);

        Button buttonDisassembleAll = trBottom.Find("ButtonDisassembleAll").GetComponent<Button>();
        buttonDisassembleAll.onClick.RemoveAllListeners();
        buttonDisassembleAll.onClick.AddListener(OnClickDisassembleAll);
        buttonDisassembleAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textDisassembleAll = buttonDisassembleAll.transform.Find("Text").GetComponent<Text>();
        textDisassembleAll.text = CsConfiguration.Instance.GetString("A25_BTN_00002");
        CsUIData.Instance.SetFont(textDisassembleAll);

        InitializeCardInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeCardInfo()
    {
        m_trCardInfo = transform.Find("PopupCardInfo");
        Transform trBack = m_trCardInfo.Find("ImageBackground");
        m_trCardInfoCollectionContent = trBack.Find("Scroll View/Viewport/Content");

        foreach (Text item in trBack.GetComponentsInChildren<Text>(true))
        {
            CsUIData.Instance.SetFont(item);
        }

        trBack.Find("TextPopupName").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_NAME_00001");
        trBack.Find("TextCompose").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_TXT_00002");
        trBack.Find("TextDescription").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_TXT_00003");
        trBack.Find("TextAcquisition").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_TXT_00004");
        trBack.Find("TextCount").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_TXT_00005");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickPopupCardInfoClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonCompose = trBack.Find("ButtonCompose").GetComponent<Button>();
        buttonCompose.onClick.RemoveAllListeners();
        buttonCompose.onClick.AddListener(OnClickCompose);
        buttonCompose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonCompose.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_BTN_00003");

        Button buttonDisassemble = trBack.Find("ButtonDisassemble").GetComponent<Button>();
        buttonDisassemble.onClick.RemoveAllListeners();
        buttonDisassemble.onClick.AddListener(OnClickPopupDisassembleOpen);
        buttonDisassemble.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonDisassemble.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_BTN_00004");
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCardList()
    {
        for (int i = 0; i < m_trCardContent.childCount; ++i)
        {
            m_trCardContent.GetChild(i).gameObject.SetActive(false);
        }

        m_srCard.verticalNormalizedPosition = 1;
        m_trCardContent.localPosition = Vector3.zero;
        m_nCardLoadIndex = 0;
        m_nStandardPosition = 0;

        int nItemSize = 360;
        int nBaseLoadCount = 12;

        if (m_nCategoryId != 0)
        {
            if (m_bMyCard)
            {
                m_listCard = CsGameData.Instance.CreatureCardList.FindAll(a => a.CreatureCardCategory.CategoryId == m_nCategoryId && CsCreatureCardManager.Instance.GetHeroCreatureCard(a.CreatureCardId) != null);
            }
            else
            {
                m_listCard = CsGameData.Instance.CreatureCardList.FindAll(a => a.CreatureCardCategory.CategoryId == m_nCategoryId);
            }
        }
        else
        {
            if (m_bMyCard)
            {
                m_listCard = CsGameData.Instance.CreatureCardList.FindAll(a => CsCreatureCardManager.Instance.GetHeroCreatureCard(a.CreatureCardId) != null);
            }
            else
            {
                m_listCard = CsGameData.Instance.CreatureCardList;
            }
        }

        if (m_listCard.Count < nBaseLoadCount)
            nBaseLoadCount = m_listCard.Count;

        m_listCard.Sort(SortToCard);

        RectTransform rectTransform = m_trCardContent.GetComponent<RectTransform>();

        int nSize = m_listCard.Count / 4;
        nSize += (m_listCard.Count % 4) != 0 ? 1 : 0;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * nSize);

        for (int i = 0; i < nBaseLoadCount; i++)
        {
            CreateCard(m_listCard[i]);
        }

        Scrollbar scrollbar = transform.Find("PanelCard/Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.RemoveAllListeners();
        scrollbar.value = 1;
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedCardScrollbar(scrollbar));
    }

    //---------------------------------------------------------------------------------------------------
    bool CreateCard(CsCreatureCard csCreatureCard)
    {
        Transform trCard = CheckCreatureCard();
        trCard.name = csCreatureCard.CreatureCardId.ToString();
        trCard.SetSiblingIndex(m_nCardLoadIndex++);

        Image imageCard = trCard.Find("ImageCard").GetComponent<Image>();
        imageCard.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Card/card_" + csCreatureCard.CreatureCardId);

        Image imageFrm = trCard.Find("ImageFrm").GetComponent<Image>();
        imageFrm.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/frm_card_rank_" + csCreatureCard.CreatureCardGrade.Grade);

        Button buttonPopup = imageFrm.GetComponent<Button>();
        buttonPopup.onClick.RemoveAllListeners();
        buttonPopup.onClick.AddListener(() => OnClickPopupCardInfoOpen(csCreatureCard));
        buttonPopup.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Image imageIcon = trCard.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/ico_Card_creature_" + csCreatureCard.CreatureCardCategory.CategoryId);

        trCard.Find("TextHp").GetComponent<Text>().text = csCreatureCard.Life.ToString("#,##0");
        trCard.Find("TextAttack").GetComponent<Text>().text = csCreatureCard.Attack.ToString("#,##0");
        trCard.Find("TextName").GetComponent<Text>().text = csCreatureCard.Name;
        trCard.Find("TextDescription").GetComponent<Text>().text = csCreatureCard.Description;

        CsHeroCreatureCard csHeroCreatureCard = CsCreatureCardManager.Instance.GetHeroCreatureCard(csCreatureCard.CreatureCardId);

        if (csHeroCreatureCard != null)
        {
            trCard.Find("TextCount").GetComponent<Text>().text = csHeroCreatureCard.Count.ToString("#,##0");
            trCard.Find("ImageDim").gameObject.SetActive(false);
            trCard.gameObject.SetActive(true);
            return true;
        }
        else
        {
            trCard.Find("TextCount").GetComponent<Text>().text = "0";
            trCard.Find("ImageDim").gameObject.SetActive(true);
            trCard.gameObject.SetActive(!m_bMyCard);
            return !m_bMyCard;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCard(CsCreatureCard csCreatureCard)
    {
        Transform trCard = m_trCardContent.Find(csCreatureCard.CreatureCardId.ToString());

        if (trCard != null)
        {
            CsHeroCreatureCard csHeroCreatureCard = CsCreatureCardManager.Instance.GetHeroCreatureCard(csCreatureCard.CreatureCardId);

            if (csHeroCreatureCard != null)
            {
                trCard.Find("TextCount").GetComponent<Text>().text = csHeroCreatureCard.Count.ToString("#,##0");
                trCard.Find("ImageDim").gameObject.SetActive(false);
                trCard.gameObject.SetActive(true);
            }
            else
            {
                trCard.Find("TextCount").GetComponent<Text>().text = "0";
                trCard.Find("ImageDim").gameObject.SetActive(true);
                trCard.gameObject.SetActive(!m_bMyCard);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    Transform CheckCardInfoCollection()
    {
        if (m_goCardInfoCollection == null)
        {
            m_goCardInfoCollection = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CardInfoCollectionItem");
        }

        for (int i = 0; i < m_trCardInfoCollectionContent.childCount; ++i)
        {
            if (!m_trCardInfoCollectionContent.GetChild(i).gameObject.activeSelf)
            {
                return m_trCardInfoCollectionContent.GetChild(i);
            }
        }

        Transform trCardInfoCollection = Instantiate(m_goCardInfoCollection, m_trCardInfoCollectionContent).transform;

        foreach (Text item in trCardInfoCollection.GetComponentsInChildren<Text>(true))
        {
            CsUIData.Instance.SetFont(item);
        }

        return trCardInfoCollection;
    }

    //---------------------------------------------------------------------------------------------------
    Transform CheckCreatureCard()
    {
        if (m_goCreatureCard == null)
        {
            m_goCreatureCard = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CreatureCardItem");
        }

        for (int i = 0; i < m_trCardContent.childCount; ++i)
        {
            if (!m_trCardContent.GetChild(i).gameObject.activeSelf)
            {
                return m_trCardContent.GetChild(i);
            }
        }

        Transform trCreatureCard = Instantiate(m_goCreatureCard, m_trCardContent).transform;

        foreach (Text item in trCreatureCard.GetComponentsInChildren<Text>(true))
        {
            CsUIData.Instance.SetFont(item);
        }

        return trCreatureCard;
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCardInfo(CsCreatureCard csCreatureCard)
    {
        m_nCardInfoId = csCreatureCard.CreatureCardId;

        for (int i = 0; i < m_trCardInfoCollectionContent.childCount; ++i)
        {
            m_trCardInfoCollectionContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trBack = m_trCardInfo.Find("ImageBackground");
        Transform trCard = trBack.Find("CreatureCardItem");

        //카드 이미지
        Image imageCard = trCard.Find("ImageCard").GetComponent<Image>();
        imageCard.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Card/card_" + csCreatureCard.CreatureCardId);

        Image imageFrm = trCard.Find("ImageFrm").GetComponent<Image>();
        imageFrm.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/frm_card_rank_" + csCreatureCard.CreatureCardGrade.Grade);

        Image imageIcon = trCard.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/ico_Card_creature_" + csCreatureCard.CreatureCardCategory.CategoryId);

        trCard.Find("TextHp").GetComponent<Text>().text = csCreatureCard.Life.ToString("#,##0");
        trCard.Find("TextAttack").GetComponent<Text>().text = csCreatureCard.Attack.ToString("#,##0");
        trCard.Find("TextName").GetComponent<Text>().text = csCreatureCard.Name;
        trCard.Find("TextDescription").GetComponent<Text>().text = csCreatureCard.Description;

        CsHeroCreatureCard csHeroCreatureCard = CsCreatureCardManager.Instance.GetHeroCreatureCard(csCreatureCard.CreatureCardId);

        if (csHeroCreatureCard != null)
        {
            trCard.Find("TextCount").GetComponent<Text>().text = csHeroCreatureCard.Count.ToString("#,##0");
            trBack.Find("TextCountValue").GetComponent<Text>().text = csHeroCreatureCard.Count.ToString("#,##0");
            CsUIData.Instance.DisplayButtonInteractable(trBack.Find("ButtonDisassemble").GetComponent<Button>(), true);

        }
        else
        {
            trCard.Find("TextCount").GetComponent<Text>().text = "0";
            trBack.Find("TextCountValue").GetComponent<Text>().text = "0";
            CsUIData.Instance.DisplayButtonInteractable(trBack.Find("ButtonDisassemble").GetComponent<Button>(), false);
        }

        trBack.Find("TextDescriptionValue").GetComponent<Text>().text = csCreatureCard.Description;
        trBack.Find("TextAcquisitionValue").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A25_TXT_00008");

        if (CsGameData.Instance.MyHeroInfo.SoulPowder >= csCreatureCard.CreatureCardGrade.CompositionSoulPowder)
        {
            CsUIData.Instance.DisplayButtonInteractable(trBack.Find("ButtonCompose").GetComponent<Button>(), true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(trBack.Find("ButtonCompose").GetComponent<Button>(), false);
        }

        //카드 컬렉션

        List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCreatureCard(m_nCardInfoId);
        int nActivatedCount = 0;
        
        for (int i = 0; i < listEntry.Count; ++i)
        {
            Transform trCardCollection = CheckCardInfoCollection();
            trCard.SetSiblingIndex(i);

            Text textCollection = trCardCollection.GetComponent<Text>();
            textCollection.text = listEntry[i].CreatureCardCollection.Name;

            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listEntry[i].CreatureCardCollection.CollectionId))
            {
                textCollection.color = CsUIData.Instance.ColorSkyblue;
                trCardCollection.Find("Image").gameObject.SetActive(true);
                nActivatedCount++;
            }
            else
            {
                textCollection.color = CsUIData.Instance.ColorGray;
                trCardCollection.Find("Image").gameObject.SetActive(false);
            }

            trCardCollection.gameObject.SetActive(true);
        }

        trBack.Find("TextComposeValue").GetComponent<Text>().text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nActivatedCount, listEntry.Count);


        m_trCardInfo.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    int SortToCard(CsCreatureCard A, CsCreatureCard B)
    {
        //카드 등급 오름차
        if (A.CreatureCardGrade.Grade > B.CreatureCardGrade.Grade) return 1;
        else if (A.CreatureCardGrade.Grade < B.CreatureCardGrade.Grade) return -1;
        else
        {
            //카드 계열 오름차
            if (A.CreatureCardCategory.CategoryId > B.CreatureCardCategory.CategoryId) return 1;
            else if (A.CreatureCardCategory.CategoryId < B.CreatureCardCategory.CategoryId) return -1;
            else
            {
                //카드 ID 오름차
                if (A.CreatureCardId > B.CreatureCardId) return 1;
                else if (A.CreatureCardId < B.CreatureCardId) return -1;
                else return 0;
            }
        }
    }

    #region 분해

    GameObject m_goPopupCalculator;
    CsPopupCalculator m_csPopupCalculator;

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupDisassembleCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        OpenPopupDisassemble();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupDisassemble()
    {
        Transform trPopup = m_trPopupList.Find("PopupCalculator");

        CsHeroCreatureCard csHeroCreatureCard = CsCreatureCardManager.Instance.GetHeroCreatureCard(m_nCardInfoId);

        if (csHeroCreatureCard == null) return;

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
        m_csPopupCalculator.EventBuyItem += OnEventDisassemble;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;
        m_csPopupCalculator.DisplayCard(csHeroCreatureCard, EnResourceType.SoulPowder);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseCalculator()
    {
        m_csPopupCalculator.EventBuyItem -= OnEventDisassemble;
        m_csPopupCalculator.EventCloseCalculator -= OnEventCloseCalculator;
        m_csPopupCalculator = null;

        Transform trPopupCalculator = m_trPopupList.Find("PopupCalculator");

        if (trPopupCalculator != null)
        {
            Destroy(trPopupCalculator.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDisassemble(int nCount)
    {
        CsCreatureCardManager.Instance.SendCreatureCardDisassemble(m_nCardInfoId, nCount);
    }

    #endregion 분해
}
