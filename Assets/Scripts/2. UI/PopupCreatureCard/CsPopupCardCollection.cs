using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsPopupCardCollection : CsPopupSub
{
    Transform m_trCategories;
    Transform m_trCollection;
    Transform m_trCategoriesContent;
    Transform m_trCollectionContent;
    Transform m_trCumulativeAttr;
    Transform m_trCumulativeContent;

    GameObject m_goCreatureCardCollection = null;
    GameObject m_goCumulativeItem = null;

	Text m_textFameCount;

    bool m_bFirst = true;
    bool m_bIsLoad = false;

    int m_nCategoryId = 0;
    int m_nLoadItemCount = 0;
    int m_nStandardPosition = 0;

    List<CsCreatureCardCollection> m_listCsCreatureCardCollection = new List<CsCreatureCardCollection>();

    Dictionary<int, int> m_dicAttr = new Dictionary<int, int>();

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsCreatureCardManager.Instance.EventCreatureCardCollectionActivate += OnEventCreatureCardCollectionActivate;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsCreatureCardManager.Instance.EventCreatureCardCollectionActivate -= OnEventCreatureCardCollectionActivate;
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
            OnClickCategoriesView();
        }
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCollectionScrollBar(Scrollbar scrollbar)
    {
        if (!m_bIsLoad)
        {
            m_bIsLoad = true;

            if (m_nLoadItemCount < m_listCsCreatureCardCollection.Count)
            {
                int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, m_listCsCreatureCardCollection.Count, (1 - scrollbar.value)));

                if (nUpdateLine > m_nStandardPosition)
                {
                    int nStartCount = m_nLoadItemCount;
                    int nEndCount = nUpdateLine + 5;

                    if (nEndCount >= m_listCsCreatureCardCollection.Count)
                    {
                        nEndCount = m_listCsCreatureCardCollection.Count;
                    }

                    for (int i = nStartCount; i < nEndCount; i++)
                    {
                        CreateCollection(m_listCsCreatureCardCollection[i]);
                    }

                    m_nStandardPosition = nUpdateLine;
                }
            }

            m_bIsLoad = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCollectionCategory(int nCategoryId)
    {
        m_nCategoryId = nCategoryId;
        DisplayCollectionView();
        m_trCollection.Find("List/Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCategoriesView()
    {
        if (!m_trCategories.gameObject.activeSelf)
            m_trCategories.gameObject.SetActive(true);

        if (m_trCollection.gameObject.activeSelf)
            m_trCollection.gameObject.SetActive(false);

        CheckCollictionNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickActvieCollection(int nCollectionId)
    {
        CsCreatureCardManager.Instance.SendCreatureCardCollectionActivate(nCollectionId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCumulativeActive(bool bIson)
    {
        if (bIson)
        {
            for (int i = 0; i < m_dicAttr.Count; ++i)
            {
                m_dicAttr[m_dicAttr.Keys.ToList()[i]] = 0;
            }

            DisplayCumlativeAttr();
        }

        m_trCumulativeAttr.gameObject.SetActive(bIson);
    }

    #endregion Event Handler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardCollectionActivate(int nCollectionId)
    {
        //컬렉션 업데이트
        //Transform trCollection = m_trCollectionContent.Find(nCollectionId.ToString());

        //if (trCollection != null)
        //{
        //    Button buttonActive = trCollection.Find("ButtonActive").GetComponent<Button>();
        //    buttonActive.onClick.RemoveAllListeners();
        //    buttonActive.onClick.AddListener(() => OnClickActvieCollection(nCollectionId));
        //    buttonActive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //    if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(nCollectionId))
        //    {
        //        buttonActive.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        buttonActive.gameObject.SetActive(true);
        //    }

        //    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A24_TXT_02001"));
        //}

        //카테고리 업데이트
        //int nCategoryId = CsGameData.Instance.GetCreatureCardCollection(nCollectionId).CreatureCardCollectionCategory.CategoryId;

        DisplayCollectionView();
        List<CsCreatureCardCollection> listCollection = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == m_nCategoryId);
        int nCount = 0;
        int nMaxCount = listCollection.Count;

        for (int i = 0; i < listCollection.Count; i++)
        {
            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listCollection[i].CollectionId))
            {
                nCount++;
            }
        }

        Transform trCategory = m_trCategoriesContent.Find(m_nCategoryId.ToString());

        if (trCategory != null)
        {
            Text textCount = trCategory.Find("TextCount").GetComponent<Text>();
            textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nCount, nMaxCount);
        }

        Transform trView = m_trCollection.Find("View");

        if (trView != null)
        {
            Slider slider = trView.Find("Slider").GetComponent<Slider>();
            slider.value = (float)nCount / nMaxCount;

            Text textSlider = slider.transform.Find("Text").GetComponent<Text>();
            textSlider.text = string.Format(CsConfiguration.Instance.GetString("A24_TXT_01001"), nCount, nMaxCount);
        }

		m_textFameCount.text = CsCreatureCardManager.Instance.CreatureCardCollectionFamePoint.ToString();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCategories = transform.Find("PanelCategories");
        m_trCollection = transform.Find("PanelCollection");
        m_trCategoriesContent = m_trCategories.Find("Scroll View/Viewport/Content");
        m_trCollectionContent = m_trCollection.Find("List/Scroll View/Viewport/Content");
        m_trCumulativeAttr = m_trCollection.Find("View/CumulativeAttr");
        m_trCumulativeContent = m_trCumulativeAttr.Find("ImageBackground/Scroll View/Viewport/Content");

		CsUIData.Instance.SetText(m_trCollection.Find("View/ImageFame/TextFame"), CsConfiguration.Instance.GetString("A16_TXT_00023"), true);

		m_textFameCount = m_trCollection.Find("View/ImageFame/TextCount").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textFameCount);
		m_textFameCount.text = CsCreatureCardManager.Instance.CreatureCardCollectionFamePoint.ToString();

        Button buttonCumulative = m_trCumulativeAttr.GetComponent<Button>();
        buttonCumulative.onClick.RemoveAllListeners();
        buttonCumulative.onClick.AddListener(() => OnClickCumulativeActive(false));
        buttonCumulative.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        List<CsCreatureCardCollectionCategory> listCategory = CsGameData.Instance.CreatureCardCollectionCategoryList;
        List<int> listCollection = CsCreatureCardManager.Instance.ActivatedCreatureCardCollectionList;

        GameObject goCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CreatureCardCollectionCategory");

        for (int i = 0; i < listCategory.Count; ++i)
        {
            int nCategoryId = listCategory[i].CategoryId;
            List<CsCreatureCardCollection> list = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == nCategoryId);

            int nCount = 0;
            int nMaxCount = list.Count;

            Transform trCategory = Instantiate(goCategory, m_trCategoriesContent).transform;
            trCategory.name = nCategoryId.ToString();

            Button buttonCategory = trCategory.GetComponent<Button>();
            buttonCategory.onClick.RemoveAllListeners();
            buttonCategory.onClick.AddListener(() => OnClickCollectionCategory(nCategoryId));
            buttonCategory.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image imageCard = trCategory.Find("ImageCard").GetComponent<Image>();
            imageCard.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/frm_card_creature_" + nCategoryId);

            Text textName = trCategory.Find("TextName").GetComponent<Text>();
            textName.text = listCategory[i].Name;
            CsUIData.Instance.SetFont(textName);

            Text textCount = trCategory.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);

            for (int j = 0; j < list.Count; j++)
            {
                if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(list[j].CollectionId))
                {
                    nCount++;
                }
            }

            textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nCount, nMaxCount);
        }

        CheckCollictionNotice();
        InitializeCollection();
    }

    //---------------------------------------------------------------------------------------------------
    void CheckCollictionNotice()
    {
        List<CsCreatureCardCollectionCategory> listCategory = CsGameData.Instance.CreatureCardCollectionCategoryList;
        List<int> listCollection = CsCreatureCardManager.Instance.ActivatedCreatureCardCollectionList;

        for (int i = 0; i < listCategory.Count; ++i)
        {
            int nCategoryId = listCategory[i].CategoryId;
            bool bActiveCheck = false;

            //해당카테고리의 컬렉션을 모두 가져옴
            List<CsCreatureCardCollection> list = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == nCategoryId);

            for (int j = 0; j < list.Count; ++j)
            {
                bool bActiveCollection = false;

                //만약 활성화된 컬렉션이라면 continue;
                for (int k = 0; k < listCollection.Count; ++k)
                {
                    if (listCollection[k] == list[j].CollectionId)
                    {
                        bActiveCollection = true;
                        break;
                    }
                }

                if (bActiveCollection) continue;

                //해당 컬렉션의 Entry을 가져와 보유중인 카드와 비교한다.
                List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCollection(list[j].CollectionId);

                for (int k = 0; k < listEntry.Count; ++k)
                {
                    if (CsCreatureCardManager.Instance.GetHeroCreatureCard(listEntry[k].CreatureCard.CreatureCardId) != null)
                    {
                        bActiveCheck = true;
                        continue;
                    }
                    else
                    {
                        bActiveCheck = false;
                        break;
                    }
                }

                if (bActiveCheck) break;
            }

            Transform trCategory = m_trCategoriesContent.Find(nCategoryId.ToString());

            if (trCategory != null)
            {
                trCategory.Find("ImageNotice").gameObject.SetActive(bActiveCheck);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeCollection()
    {
        Transform trView = m_trCollection.Find("View");

        Button buttonCategoriesView = trView.Find("ButtonCategoriesView").GetComponent<Button>();
        buttonCategoriesView.onClick.RemoveAllListeners();
        buttonCategoriesView.onClick.AddListener(OnClickCategoriesView);
        buttonCategoriesView.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonView = buttonCategoriesView.transform.Find("Text").GetComponent<Text>();
        textButtonView.text = CsConfiguration.Instance.GetString("A24_TXT_00001");
        CsUIData.Instance.SetFont(textButtonView);

        Text textCategoryName = trView.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCategoryName);

        Text textSlider = trView.Find("Slider/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSlider);

        Button buttonAttrInfo = trView.Find("ButtonAttrInfo").GetComponent<Button>();
        buttonAttrInfo.onClick.RemoveAllListeners();
        buttonAttrInfo.onClick.AddListener(() => OnClickCumulativeActive(true));
        buttonAttrInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCumulativeAttr = m_trCumulativeAttr.Find("ImageBackground/TextCumulativeAttr").GetComponent<Text>();
        textCumulativeAttr.text = CsConfiguration.Instance.GetString("A24_TXT_00003");
        CsUIData.Instance.SetFont(textCumulativeAttr);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCollectionView()
    {
        if (m_trCategories.gameObject.activeSelf)
            m_trCategories.gameObject.SetActive(false);

        if (!m_trCollection.gameObject.activeSelf)
            m_trCollection.gameObject.SetActive(true);

        for (int i = 0; i < m_trCollectionContent.childCount; ++i)
        {
            m_trCollectionContent.GetChild(i).gameObject.SetActive(false);
        }

        Transform trView = m_trCollection.Find("View");

        trView.Find("ImageCard").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/ico_card_creature_" + m_nCategoryId);
        trView.Find("TextName").GetComponent<Text>().text = CsGameData.Instance.GetCreatureCardCategory(m_nCategoryId).Name;

        //List<CsCreatureCardCollection> listSort = new List<CsCreatureCardCollection>();
        m_listCsCreatureCardCollection.Clear();
        List<int> listCollection = CsCreatureCardManager.Instance.ActivatedCreatureCardCollectionList;
        List<CsCreatureCardCollection> list = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == m_nCategoryId);
        //조합가능 
        List<CsCreatureCardCollection> list1 = list.FindAll(a => CheckCollectionActive(a.CollectionId));
        list1.Sort(SortToCollection);
        m_listCsCreatureCardCollection.AddRange(list1.ToArray());

        //조합 불가능
        List<CsCreatureCardCollection> list2 = list.FindAll(a => !CheckCollectionActive(a.CollectionId) && !CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(a.CollectionId));
        list2.Sort(SortToCollection);
        m_listCsCreatureCardCollection.AddRange(list2.ToArray());

        //활성화된것
        List<CsCreatureCardCollection> list3 = list.FindAll(a => CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(a.CollectionId));
        list3.Sort(SortToCollection);
        m_listCsCreatureCardCollection.AddRange(list3.ToArray());

        int nCount = 0;
        int nMaxCount = m_listCsCreatureCardCollection.Count;

        for (int i = 0; i < listCollection.Count; ++i)
        {
            if (m_listCsCreatureCardCollection.Find(a => a.CollectionId == listCollection[i]) != null)
            {
                nCount++;
            }
        }

        Slider slider = trView.Find("Slider").GetComponent<Slider>();
        slider.value = (float)nCount / nMaxCount;

        Text textSlider = slider.transform.Find("Text").GetComponent<Text>();
        textSlider.text = string.Format(CsConfiguration.Instance.GetString("A24_TXT_01001"), nCount, nMaxCount);

        m_nStandardPosition = 0;
        m_nLoadItemCount = 0;

        int nItemSize = 150;
        int nBaseCount = 0;

        if (m_listCsCreatureCardCollection.Count < 10)
        {
            nBaseCount = m_listCsCreatureCardCollection.Count;
        }
        else
        {
            nBaseCount = 10;
        }

        RectTransform rectTransform = m_trCollectionContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * m_listCsCreatureCardCollection.Count);

        for (int i = 0; i < nBaseCount; ++i)
        {
            CreateCollection(m_listCsCreatureCardCollection[i]);
        }

        Scrollbar scrollbar = m_trCollection.Find("List/Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.RemoveAllListeners();
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedCollectionScrollBar(scrollbar));
    }

    //---------------------------------------------------------------------------------------------------
    void CreateCollection(CsCreatureCardCollection csCreatureCardCollection)
    {
        int nCollectionId = csCreatureCardCollection.CollectionId;
        //활성화된 컬렉션 유무
        bool bActiveCheck = CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(nCollectionId);
        bool bCardCheck = true;

        Transform trCollection = CheckCollection();
        trCollection.name = nCollectionId.ToString();

        trCollection.Find("TextName").GetComponent<Text>().text = string.Format("<color={0}>{1}</color>", csCreatureCardCollection.CreatureCardCollectionGrade.ColorCode, csCreatureCardCollection.Name);

        Transform trCardList = trCollection.Find("CardList");
        Transform trAttrList = trCollection.Find("AttrList");
        List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCollection(csCreatureCardCollection.CollectionId);
        listEntry.Sort(SortToCard);

        for (int i = 0; i < listEntry.Count; ++i)
        {
            Transform trItem = trCardList.Find("ItemSlot" + i);

            if (bActiveCheck)
            {
                CsUIData.Instance.DisplayItemSlot(trItem, listEntry[i].CreatureCard, false);
            }
            else if (CsCreatureCardManager.Instance.GetHeroCreatureCard(listEntry[i].CreatureCard.CreatureCardId) != null)
            {
                CsUIData.Instance.DisplayItemSlot(trItem, listEntry[i].CreatureCard, false);
            }
            else
            {
                CsUIData.Instance.DisplayItemSlot(trItem, listEntry[i].CreatureCard, true);
                bCardCheck = false;
            }

            trItem.gameObject.SetActive(true);
        }

        List<CsCreatureCardCollectionAttr> listAttr = CsGameData.Instance.GetCreatureCardCollection(csCreatureCardCollection.CollectionId).CreatureCardCollectionAttrList;

        for (int i = 0; i < listAttr.Count; ++i)
        {
            Transform trAttr = trAttrList.Find("TextAttr" + i);
            trAttr.GetComponent<Text>().text = listAttr[i].Attr.Name;
            trAttr.gameObject.SetActive(true);

            if ((listAttr[i].Attr.AttrId >= 6 && listAttr[i].Attr.AttrId < 12)
                || (listAttr[i].Attr.AttrId >= 20 && listAttr[i].Attr.AttrId < 29))
            {
                trAttr.Find("TextValue").GetComponent<Text>().text = (listAttr[i].AttrValue.Value / 100f).ToString("###.#");
            }
            else
            {
                trAttr.Find("TextValue").GetComponent<Text>().text = listAttr[i].AttrValue.Value.ToString("###,0");
            }
        }

        Button buttonActive = trCollection.Find("ButtonActive").GetComponent<Button>();

        if (bActiveCheck)
        {
            buttonActive.gameObject.SetActive(false);
        }
        else
        {
            buttonActive.gameObject.SetActive(true);
            CsUIData.Instance.DisplayButtonInteractable(buttonActive, bCardCheck);
        }

        buttonActive.onClick.RemoveAllListeners();
        buttonActive.onClick.AddListener(() => OnClickActvieCollection(nCollectionId));
        buttonActive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_nLoadItemCount++;
        trCollection.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCumlativeAttr()
    {
        for (int i = 0; i < m_trCumulativeContent.childCount; ++i)
        {
            m_trCumulativeContent.GetChild(i).Find("TextValue").GetComponent<Text>().text = "0";
            m_trCumulativeContent.GetChild(i).gameObject.SetActive(false);
        }

        List<int> listCollection = CsCreatureCardManager.Instance.ActivatedCreatureCardCollectionList;

        for (int i = 0; i < listCollection.Count; ++i)
        {
            List<CsCreatureCardCollectionAttr> listAttr = CsGameData.Instance.GetCreatureCardCollection(listCollection[i]).CreatureCardCollectionAttrList;

            for (int j = 0; j < listAttr.Count; ++j)
            {
                CreateCumlativeAttr(listAttr[j]);
            }
        }
    }


    //---------------------------------------------------------------------------------------------------
    void CreateCumlativeAttr(CsCreatureCardCollectionAttr csCreatureCardCollectionAttr)
    {
        if (m_goCumulativeItem == null)
        {
            m_goCumulativeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CollectionCumulativeItem");
        }

        Transform trCumulativeItem = m_trCumulativeContent.Find(csCreatureCardCollectionAttr.Attr.AttrId.ToString());

        if (trCumulativeItem == null)
        {
            m_dicAttr.Add(csCreatureCardCollectionAttr.Attr.AttrId, csCreatureCardCollectionAttr.AttrValue.Value);
            trCumulativeItem = Instantiate(m_goCumulativeItem, m_trCumulativeContent).transform;
            trCumulativeItem.name = csCreatureCardCollectionAttr.Attr.AttrId.ToString();

            Text textName = trCumulativeItem.Find("TextName").GetComponent<Text>();
            textName.text = csCreatureCardCollectionAttr.Attr.Name;
            CsUIData.Instance.SetFont(textName);

            Text textValue = trCumulativeItem.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textValue);

            if ((csCreatureCardCollectionAttr.Attr.AttrId >= 6 && csCreatureCardCollectionAttr.Attr.AttrId < 12)
                || (csCreatureCardCollectionAttr.Attr.AttrId >= 20 && csCreatureCardCollectionAttr.Attr.AttrId < 29))
            {
                textValue.text = (m_dicAttr[csCreatureCardCollectionAttr.Attr.AttrId] / 100f).ToString("###.#");
            }
            else
            {
                textValue.text = m_dicAttr[csCreatureCardCollectionAttr.Attr.AttrId].ToString("###,0");
            }
        }
        else
        {
            m_dicAttr[csCreatureCardCollectionAttr.Attr.AttrId] += csCreatureCardCollectionAttr.AttrValue.Value;
            Text textValue = trCumulativeItem.Find("TextValue").GetComponent<Text>();

            if ((csCreatureCardCollectionAttr.Attr.AttrId >= 6 && csCreatureCardCollectionAttr.Attr.AttrId < 12)
               || (csCreatureCardCollectionAttr.Attr.AttrId >= 20 && csCreatureCardCollectionAttr.Attr.AttrId < 29))
            {
                textValue.text = (m_dicAttr[csCreatureCardCollectionAttr.Attr.AttrId] / 100f).ToString("###.#");
            }
            else
            {
                textValue.text = m_dicAttr[csCreatureCardCollectionAttr.Attr.AttrId].ToString("###,0");
            }
        }

        trCumulativeItem.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    Transform CheckCollection()
    {
        if (m_goCreatureCardCollection == null)
        {
            m_goCreatureCardCollection = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CreatureCardCollection");
        }

        Transform trCreatureCardCollection;

        for (int i = 0; i < m_trCollectionContent.childCount; ++i)
        {
            if (!m_trCollectionContent.GetChild(i).gameObject.activeSelf)
            {
                trCreatureCardCollection = m_trCollectionContent.GetChild(i);

                for (int j = 0; j < 4; ++j)
                {
                    trCreatureCardCollection.Find("CardList/ItemSlot" + j).gameObject.SetActive(false);
                    trCreatureCardCollection.Find("AttrList/TextAttr" + j).gameObject.SetActive(false);
                }

                return m_trCollectionContent.GetChild(i);
            }
        }

        trCreatureCardCollection = Instantiate(m_goCreatureCardCollection, m_trCollectionContent).transform;

        trCreatureCardCollection.Find("TextCompleted").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A24_BTN_00001");
        trCreatureCardCollection.Find("ButtonActive/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A24_BTN_00001");

        foreach (Text item in trCreatureCardCollection.GetComponentsInChildren<Text>(true))
        {
            CsUIData.Instance.SetFont(item);
        }
        trCreatureCardCollection.SetAsLastSibling();
        return trCreatureCardCollection;
    }

    bool CheckCollectionActive(int nCollectionId)
    {
        //해당 컬렉션의 Entry을 가져와 보유중인 카드와 비교한다.
        List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCollection(nCollectionId);

        if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(nCollectionId)) return false;

        for (int k = 0; k < listEntry.Count; ++k)
        {
            if (CsCreatureCardManager.Instance.GetHeroCreatureCard(listEntry[k].CreatureCard.CreatureCardId) == null)
            {
                return false;
            }
        }
        return true;
    }

    int SortToCollection(CsCreatureCardCollection A, CsCreatureCardCollection B)
    {

        if (A.CreatureCardCollectionGrade.Grade > B.CreatureCardCollectionGrade.Grade) return 1;
        else if (A.CreatureCardCollectionGrade.Grade < B.CreatureCardCollectionGrade.Grade) return -1;
        else
        {
            if (A.CollectionId > B.CollectionId) return 1;
            else if (A.CollectionId < B.CollectionId) return -1;
            else return 0;
        }

        //bool bA = CheckCollectionActive(A.CollectionId);
        //bool bB = CheckCollectionActive(B.CollectionId);

        //bA = CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(A.CollectionId);
        //bB = CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(A.CollectionId);

        //if (bA && bB)
        //{
        //    return 0;
        //    //if (A.CreatureCardCollectionGrade.Grade > B.CreatureCardCollectionGrade.Grade) return 1;
        //    //else if (A.CreatureCardCollectionGrade.Grade < B.CreatureCardCollectionGrade.Grade) return -1;
        //    //else
        //    //{
        //    //    if (A.CollectionId > B.CollectionId) return 1;
        //    //    else if (A.CollectionId < B.CollectionId) return -1;
        //    //    else return 0;
        //    //}
        //}
        //else if (bA)
        //{
        //    return 1;
        //}
        //else if (bB)
        //{
        //    return 1;
        //}
        //else
        //{
        //    return 0;
        //    //bA = CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(A.CollectionId);
        //    //bB = CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(A.CollectionId);

        //    //if (bA && bB)
        //    //{
        //    //    if (A.CreatureCardCollectionGrade.Grade > B.CreatureCardCollectionGrade.Grade) return 1;
        //    //    else if (A.CreatureCardCollectionGrade.Grade < B.CreatureCardCollectionGrade.Grade) return -1;
        //    //    else
        //    //    {
        //    //        if (A.CollectionId > B.CollectionId) return 1;
        //    //        else if (A.CollectionId < B.CollectionId) return -1;
        //    //        else return 0;
        //    //    }
        //    //}
        //    //else if (bA)
        //    //{
        //    //    return 1;
        //    //}
        //    //else if (bB)
        //    //{
        //    //    return 1;
        //    //}
        //    //else
        //    //    return 0;
        //}
    }


    //---------------------------------------------------------------------------------------------------
    int SortToCard(CsCreatureCardCollectionEntry A, CsCreatureCardCollectionEntry B)
    {
        if (A.CreatureCard.CreatureCardId > B.CreatureCard.CreatureCardId) return 1;
        else if (A.CreatureCard.CreatureCardId < B.CreatureCard.CreatureCardId) return -1;
        return 0;
    }
}

