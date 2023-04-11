using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-04-19)
//---------------------------------------------------------------------------------------------------

public class CsPopupTitle : CsPopupSub
{
    GameObject m_goTitleItem;
    GameObject m_goCumlativeItem;

    Transform m_trPanelTitleOwn;
    Transform m_trPanelTitleInfo;
    Transform m_trPanelTitleList;
    Transform m_trTitleListContent;
    Transform m_trCumulativePassive;

    int m_nSelectCategory = 0;
    int m_nSelectDisplayTitleId;
    int m_nSelectActivationTitleId;

    ToggleGroup m_toggleGroupDIsplay;
    ToggleGroup m_toggleGroupActivation;

    //Prefab 로드용
    List<CsTitle> m_listTitle;
    int m_nTitleItemLoadIndex = 0;
    int m_nStandardPosition = 0;
    bool m_bIsLoad = false;
    bool m_bSoundFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsTitleManager.Instance.EventDisplayTitleSet += OnEventDisplayTitleSet;
        CsTitleManager.Instance.EventActivationTitleSet += OnEventActivationTitleSet;
        CsTitleManager.Instance.EventTitleLifetimeEnded += OnEventTitleLifetimeEnded;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsTitleManager.Instance.EventDisplayTitleSet -= OnEventDisplayTitleSet;
        CsTitleManager.Instance.EventActivationTitleSet -= OnEventActivationTitleSet;
        CsTitleManager.Instance.EventTitleLifetimeEnded -= OnEventTitleLifetimeEnded;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventDisplayTitleSet()
    {
        DisplayTitleInfoDisplayName();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventActivationTitleSet()
    {
        UpdateCumlativePassive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTitleLifetimeEnded(int nTitleId)
    {
        Transform trContent = m_trPanelTitleOwn.Find("Scroll View/Viewport/Content");
        Transform trTitleOwn = trContent.Find(nTitleId.ToString());

        List<CsHeroTitle> listTitle = CsTitleManager.Instance.HeroTitleList;

        if (trTitleOwn != null)
        {
            if (listTitle.Count == 0)
            {
                Text textNoTitel = m_trPanelTitleOwn.Find("TextNoTitle").GetComponent<Text>();
                textNoTitel.text = CsConfiguration.Instance.GetString("A83_TXT_00005");
                CsUIData.Instance.SetFont(textNoTitel);
                textNoTitel.gameObject.SetActive(true);
                DisplayTitleInfo(null);
            }
            else
            {

                for (int i = 0; i < trContent.childCount; ++i)
                {
                    if (listTitle[0].Title.TitleId.ToString() == trContent.GetChild(i).name)
                    {
                        trContent.GetChild(0).GetComponent<Toggle>().isOn = true;

                        if (!m_trPanelTitleOwn.gameObject.activeSelf)
                        {
                            DisplayTitleInfo(listTitle[0]);
                        }
                    }
                }

                DisplayTitleInfoDisplayName();
            }

            UpdateTitleCount();
            UpdateCumlativePassive();
            trTitleOwn.gameObject.SetActive(false);
        }
    }

    #endregion Event

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMainCategory(Toggle toggle, int nCategoryId, List<Toggle> listToggle)
    {
        if (listToggle != null)
        {
            for (int i = 0; i < listToggle.Count; ++i)
            {
                listToggle[i].gameObject.SetActive(toggle.isOn);
            }
        }

        if (toggle.isOn && m_nSelectCategory != nCategoryId && nCategoryId == 0)
        {
            m_nSelectCategory = nCategoryId;
            toggle.group.allowSwitchOff = false;
            m_trPanelTitleInfo.gameObject.SetActive(true);
            m_trPanelTitleOwn.gameObject.SetActive(true);
            m_trPanelTitleList.gameObject.SetActive(false);
        }
        else if (toggle.isOn && m_nSelectCategory != nCategoryId)
        {
            m_nSelectCategory = nCategoryId;
            toggle.group.allowSwitchOff = true;
            if (listToggle != null)
            {
                m_bSoundFirst = true;
                listToggle[0].group.SetAllTogglesOff();
                listToggle[0].isOn = true;
            }
        }

        //화살표
        if (nCategoryId != 0)
        {
            toggle.transform.Find("ImageOn").gameObject.SetActive(!toggle.isOn);
            toggle.transform.Find("ImageOff").gameObject.SetActive(toggle.isOn);
        }

        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nSelectCategory = nCategoryId;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSmallCategory(bool bIson, int nType)
    {
        if (bIson)
        {
            if (m_trPanelTitleInfo.gameObject.activeSelf || m_trPanelTitleOwn.gameObject.activeSelf)
            {
                m_trPanelTitleInfo.gameObject.SetActive(false);
                m_trPanelTitleOwn.gameObject.SetActive(false);
                m_trPanelTitleList.gameObject.SetActive(true);
            }

            if (m_bSoundFirst)
            {
                m_bSoundFirst = false;
            }
            else
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            }
            DisplayTitleList(nType);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDisplayTitle(bool bIson, int nTitleId)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsTitleManager.Instance.SendDisplayTitleSet(nTitleId);
        }
        else
        {
            if (!m_toggleGroupDIsplay.AnyTogglesOn())
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
                CsTitleManager.Instance.SendDisplayTitleSet(0);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedActivationTitle(bool bIson, int nTitleId)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsTitleManager.Instance.SendActivationTitleSet(nTitleId);
        }
        else
        {
            if (!m_toggleGroupActivation.AnyTogglesOn())
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
                CsTitleManager.Instance.SendActivationTitleSet(0);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedTitleInfo(bool bIson, int nTitleId)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsHeroTitle csHeroTitle = CsTitleManager.Instance.GetHeroTitle(nTitleId);
            DisplayTitleInfo(csHeroTitle);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDisplayCumlativePassive(bool bIson)
    {
        m_trCumulativePassive.gameObject.SetActive(bIson);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedTitleScrollbar(Scrollbar scrollbar)
    {
        if (!m_bIsLoad)
        {
            m_bIsLoad = true;

            if (m_nTitleItemLoadIndex < m_listTitle.Count)
            {
                int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, m_listTitle.Count, (1 - scrollbar.value))) * 3;

                if (nUpdateLine > m_nStandardPosition)
                {
                    int nStartCount = m_nTitleItemLoadIndex;
                    int nEndCount = nUpdateLine;

                    if (nEndCount >= m_listTitle.Count)
                    {
                        nEndCount = m_listTitle.Count;
                    }

                    for (int i = nStartCount; i < nEndCount; i++)
                    {
                        CreateTitleItem(m_listTitle[i]);
                    }

                    m_nStandardPosition = nUpdateLine;
                }
            }

            m_bIsLoad = false;
        }
    }

    #endregion Evetn Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_nSelectDisplayTitleId = CsTitleManager.Instance.DisplayTitleId;
        m_nSelectActivationTitleId = CsTitleManager.Instance.ActivationTitleId;
        m_trPanelTitleOwn = transform.Find("PanelTitleOwn");
        m_trPanelTitleInfo = transform.Find("PanelTitleInfo");
        m_trPanelTitleList = transform.Find("PanelTitleList");
        m_trTitleListContent = m_trPanelTitleList.Find("Scroll View/Viewport/Content");
        m_goTitleItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/TitleItem");

        Text textDisplay = m_trPanelTitleInfo.Find("TextDisplay").GetComponent<Text>();
        textDisplay.text = CsConfiguration.Instance.GetString("A83_TXT_00004");
        CsUIData.Instance.SetFont(textDisplay);

        Transform trInfo = m_trPanelTitleInfo.Find("Info");

        foreach (Text text in trInfo.GetComponentsInChildren<Text>())
        {
            CsUIData.Instance.SetFont(text);
        }

        DisplayPanelCategories();
        DisplayTitleOwn();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTitleCount()
    {
        Transform trContent = transform.Find("PanelTitleCategories/Scroll View/Viewport/Content");
        Text textAll = trContent.Find("ToggleAll/Text").GetComponent<Text>();
        textAll.text = string.Format(CsConfiguration.Instance.GetString("A83_BTN_01003"), CsTitleManager.Instance.HeroTitleList.Count, CsGameData.Instance.TitleList.Count);

        List<CsTitleCategory> listCategory = CsGameData.Instance.TitleCategoryList;

        for (int i = 0; i < listCategory.Count; ++i)
        {
            int nCategoryId = listCategory[i].CategoryId;
            int nCategoryCount = CsTitleManager.Instance.HeroTitleList.FindAll(a => a.Title.TitleType.TitleCategory.CategoryId == nCategoryId).Count;
            int nCategoryMaxCount = CsGameData.Instance.TitleList.FindAll(a => a.TitleType.TitleCategory.CategoryId == nCategoryId).Count;
            Transform trMainCategory = trContent.Find("MainCategory" + nCategoryId);

            if (trMainCategory != null)
            {
                Text textMain = trMainCategory.Find("Text").GetComponent<Text>();
                textMain.text = string.Format(listCategory[i].Name, nCategoryCount, nCategoryMaxCount);
            }

            List<CsTitleType> listType = CsGameData.Instance.TitleTypeList.FindAll(a => a.TitleCategory.CategoryId == nCategoryId);

            for (int j = 0; j < listType.Count; ++j)
            {
                int nTypeId = listType[j].Type;
                int nTypeCount = CsTitleManager.Instance.HeroTitleList.FindAll(a => a.Title.TitleType.Type == nTypeId).Count;
                int nTypeMaxCount = CsGameData.Instance.TitleList.FindAll(a => a.TitleType.Type == nTypeId).Count;

                Transform trSmallCategory = trContent.Find("SmallCategory" + nTypeId);
                if (trSmallCategory != null)
                {
                    Text textSmall = trSmallCategory.Find("Text").GetComponent<Text>();
                    textSmall.text = string.Format(listType[j].Name, nTypeCount, nTypeMaxCount);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPanelCategories()
    {
        Transform trContent = transform.Find("PanelTitleCategories/Scroll View/Viewport/Content");
        ToggleGroup toggleGroupContent = trContent.GetComponent<ToggleGroup>();

        GameObject goMainCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleTitleMainCategory");
        GameObject goSmallCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleTitleSmallCategory");

        Transform trAllCategory = trContent.Find("ToggleAll");

        Toggle toggleAll = trAllCategory.GetComponent<Toggle>();
        toggleAll.group = toggleGroupContent;
        toggleAll.onValueChanged.RemoveAllListeners();
        toggleAll.onValueChanged.AddListener((ison) => OnValueChangedMainCategory(toggleAll, 0, null));

        Text textAll = trAllCategory.Find("Text").GetComponent<Text>();
        textAll.text = string.Format(CsConfiguration.Instance.GetString("A83_BTN_01003"), CsTitleManager.Instance.HeroTitleList.Count, CsGameData.Instance.TitleList.Count);
        CsUIData.Instance.SetFont(textAll);

        if (goMainCategory != null && goSmallCategory != null)
        {
            List<CsTitleCategory> listCategory = CsGameData.Instance.TitleCategoryList;

            for (int i = 0; i < listCategory.Count; ++i)
            {
                int nCategoryId = listCategory[i].CategoryId;
                int nCategoryCount = CsTitleManager.Instance.HeroTitleList.FindAll(a => a.Title.TitleType.TitleCategory.CategoryId == nCategoryId).Count;
                int nCategoryMaxCount = CsGameData.Instance.TitleList.FindAll(a => a.TitleType.TitleCategory.CategoryId == nCategoryId).Count;

                Transform trMainCategory = Instantiate(goMainCategory, trContent).transform;
                trMainCategory.name = "MainCategory" + nCategoryId;

                ToggleGroup toggleGroupMain = trMainCategory.GetComponent<ToggleGroup>();
                Toggle toggleMain = trMainCategory.GetComponent<Toggle>();
                toggleMain.group = toggleGroupContent;
                toggleMain.onValueChanged.RemoveAllListeners();

                Text textMain = trMainCategory.Find("Text").GetComponent<Text>();
                textMain.text = string.Format(listCategory[i].Name, nCategoryCount, nCategoryMaxCount);
                CsUIData.Instance.SetFont(textMain);

                List<CsTitleType> listType = CsGameData.Instance.TitleTypeList.FindAll(a => a.TitleCategory.CategoryId == nCategoryId);
                List<Toggle> listToggle = new List<Toggle>();

                for (int j = 0; j < listType.Count; ++j)
                {
                    int nTypeId = listType[j].Type;
                    int nTypeCount = CsTitleManager.Instance.HeroTitleList.FindAll(a => a.Title.TitleType.Type == nTypeId).Count;
                    int nTypeMaxCount = CsGameData.Instance.TitleList.FindAll(a => a.TitleType.Type == nTypeId).Count;

                    Transform trSmallCategory = Instantiate(goSmallCategory, trContent).transform;
                    trSmallCategory.name = "SmallCategory" + nTypeId;
                    Toggle toggleSmall = trSmallCategory.GetComponent<Toggle>();
                    listToggle.Add(toggleSmall);
                    toggleSmall.group = toggleGroupMain;
                    toggleSmall.onValueChanged.RemoveAllListeners();
                    toggleSmall.onValueChanged.AddListener((ison) => OnValueChangedSmallCategory(ison, nTypeId));

                    Text textSmall = trSmallCategory.Find("Text").GetComponent<Text>();
                    textSmall.text = string.Format(listType[j].Name, nTypeCount, nTypeMaxCount); ;
                    CsUIData.Instance.SetFont(textSmall);

                    trSmallCategory.gameObject.SetActive(false);
                }

                toggleMain.onValueChanged.AddListener((ison) => OnValueChangedMainCategory(toggleMain, nCategoryId, listToggle));
            }
        }
    }


    //---------------------------------------------------------------------------------------------------
    void DisplayTitleOwn()
    {
        foreach (Text text in m_trPanelTitleOwn.GetComponentsInChildren<Text>())
        {
            CsUIData.Instance.SetFont(text);
        }

        m_trPanelTitleOwn.Find("TextDisplay").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00001");
        m_trPanelTitleOwn.Find("TextAttr").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00002");
        m_trPanelTitleOwn.Find("TextActiveTitle").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00003");

        GameObject goTitleOwn = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleTitleOwnItem");
        Transform trContent = m_trPanelTitleOwn.Find("Scroll View/Viewport/Content");
        List<CsHeroTitle> listTitle = CsTitleManager.Instance.HeroTitleList;

        if (listTitle.Count == 0)
        {
            Text textNoTitel = m_trPanelTitleOwn.Find("TextNoTitle").GetComponent<Text>();
            textNoTitel.text = CsConfiguration.Instance.GetString("A83_TXT_00005");
            CsUIData.Instance.SetFont(textNoTitel);
            textNoTitel.gameObject.SetActive(true);
            DisplayTitleInfo(null);
            return;
        }

        listTitle.Sort(SortToHeroTitle);
        ToggleGroup toggleGroupTitle = trContent.GetComponent<ToggleGroup>();
        m_toggleGroupDIsplay = m_trPanelTitleOwn.Find("ToggleGroupDisplay").GetComponent<ToggleGroup>();
        m_toggleGroupActivation = m_trPanelTitleOwn.Find("ToggleGroupActivation").GetComponent<ToggleGroup>();

        for (int i = 0; i < listTitle.Count; ++i)
        {
            int nTitleId = listTitle[i].Title.TitleId;
            Transform trTitleOwn = Instantiate(goTitleOwn, trContent).transform;
            trTitleOwn.name = nTitleId.ToString();

            Toggle toggleTitle = trTitleOwn.GetComponent<Toggle>();
            toggleTitle.group = toggleGroupTitle;

            if (i == 0)
            {
                toggleTitle.isOn = true;
                DisplayTitleInfoDisplayName();
                DisplayTitleInfo(listTitle[i]);
            }

            toggleTitle.onValueChanged.RemoveAllListeners();
            toggleTitle.onValueChanged.AddListener((ison) => OnValueChangedTitleInfo(ison, nTitleId));

            Text textName = trTitleOwn.Find("TextTitleName").GetComponent<Text>();
            textName.text = string.Format("<color={0}>{1}</color>", listTitle[i].Title.TitleGrade.ColorCode, listTitle[i].Title.Name);
            CsUIData.Instance.SetFont(textName);

            Toggle toggleDisplay = trTitleOwn.Find("ToggleDisplay").GetComponent<Toggle>();
            if (m_nSelectDisplayTitleId == listTitle[i].Title.TitleId) toggleDisplay.isOn = true;
            toggleDisplay.group = m_toggleGroupDIsplay;
            toggleDisplay.onValueChanged.RemoveAllListeners();
            toggleDisplay.onValueChanged.AddListener((ison) => OnValueChangedDisplayTitle(ison, nTitleId));

            Toggle toggleActivation = trTitleOwn.Find("ToggleActivation").GetComponent<Toggle>();
            if (m_nSelectActivationTitleId == listTitle[i].Title.TitleId) toggleActivation.isOn = true;
            toggleActivation.group = m_toggleGroupActivation;
            toggleActivation.onValueChanged.RemoveAllListeners();
            toggleActivation.onValueChanged.AddListener((ison) => OnValueChangedActivationTitle(ison, nTitleId));

			Transform trTitleLock = trTitleOwn.Find("TitleLock");
			trTitleLock.gameObject.SetActive(false);

			// 장착 가능 레벨에 따라 잠금 프로세스 추가 필요.
			CsUIData.Instance.SetText(trTitleLock.Find("Text"), CsConfiguration.Instance.GetString("A83_TXT_00014"), true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTitleInfoDisplayName()
    {
        CsTitle csTitle = CsGameData.Instance.GetTitle(CsTitleManager.Instance.DisplayTitleId);
        Text textName = m_trPanelTitleInfo.Find("Info/TextTitleName").GetComponent<Text>();

        if (csTitle != null)
        {
            textName.text = string.Format("<color={0}>{1}</color>", csTitle.TitleGrade.ColorCode, csTitle.Name);
        }
        else
        {
            textName.text = string.Empty;
        }

    }

    //---------------------------------------------------------------------------------------------------
    //칭호 정보 표시
    void DisplayTitleInfo(CsHeroTitle csHeroTitle)
    {
        Transform trInfo = m_trPanelTitleInfo.Find("Info");

        if (csHeroTitle == null)
        {
            Text textNoTitle = m_trPanelTitleInfo.Find("TextNoTitle").GetComponent<Text>();
            textNoTitle.text = CsConfiguration.Instance.GetString("A83_TXT_00006");
            CsUIData.Instance.SetFont(textNoTitle);
            textNoTitle.gameObject.SetActive(true);
            trInfo.gameObject.SetActive(false);
        }
        else
        {
            Text textRequiredAccomplishmentLevelValue = trInfo.Find("RequireLv/TextAttr0/TextValue").GetComponent<Text>();

			CsUIData.Instance.SetText(trInfo.Find("RequireLv/TextAttr0"), CsConfiguration.Instance.GetString("A83_TXT_00013"), true);
            CsUIData.Instance.SetText(textRequiredAccomplishmentLevelValue.transform, csHeroTitle.Title.ActivationRequiredAccomplishmentLevel.ToString(), false);			// 장착 가능 레벨

            if (csHeroTitle.Title.ActivationRequiredAccomplishmentLevel <= CsAccomplishmentManager.Instance.AccomplishmentLevel)
            {
                // 장착 가능
                textRequiredAccomplishmentLevelValue.color = new Color32(255, 214, 80, 255);
            }
            else
            {
                // 장착 불가능
                textRequiredAccomplishmentLevelValue.color = new Color32(229, 115, 115, 255);
            }

            trInfo.Find("TextAttr").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00012");
            trInfo.Find("Active/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00008");
            trInfo.Find("Active/TextAttr0").GetComponent<Text>().text = csHeroTitle.Title.TitleActiveAttrList[0].Attr.Name;
            trInfo.Find("Active/TextAttr0/TextValue").GetComponent<Text>().text = csHeroTitle.Title.TitleActiveAttrList[0].AttrValue.Value.ToString("#,##0");
            trInfo.Find("Active/TextAttr1").GetComponent<Text>().text = csHeroTitle.Title.TitleActiveAttrList[1].Attr.Name;
            trInfo.Find("Active/TextAttr1/TextValue").GetComponent<Text>().text = csHeroTitle.Title.TitleActiveAttrList[1].AttrValue.Value.ToString("#,##0");

            trInfo.Find("Passive/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00009");
            trInfo.Find("Passive/TextAttr0").GetComponent<Text>().text = csHeroTitle.Title.TitlePassiveAttrList[0].Attr.Name;
            trInfo.Find("Passive/TextAttr0/TextValue").GetComponent<Text>().text = csHeroTitle.Title.TitlePassiveAttrList[0].AttrValue.Value.ToString("#,##0");
            trInfo.Find("Passive/TextAttr1").GetComponent<Text>().text = csHeroTitle.Title.TitlePassiveAttrList[1].Attr.Name;
            trInfo.Find("Passive/TextAttr1/TextValue").GetComponent<Text>().text = csHeroTitle.Title.TitlePassiveAttrList[1].AttrValue.Value.ToString("#,##0");
            trInfo.Find("TextTime").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00010");

            if (csHeroTitle.RemainingTime == 0)
            {
                trInfo.Find("TextTime/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00011");
            }
            else
            {
                Debug.Log("남은시간 " + (csHeroTitle.RemainingTime - Time.realtimeSinceStartup));
                int nDay = (int)(csHeroTitle.RemainingTime - Time.realtimeSinceStartup) / 86400;
                int nSec = (int)(csHeroTitle.RemainingTime - Time.realtimeSinceStartup) % 86400;
                int nHour = (nSec / 3600) + 1;
                if (nDay == 0)
                {
                    trInfo.Find("TextTime/Text").GetComponent<Text>().text = string.Format(CsConfiguration.Instance.GetString("A83_TXT_01002"), nHour);
                }
                else
                {
                    trInfo.Find("TextTime/Text").GetComponent<Text>().text = string.Format(CsConfiguration.Instance.GetString("A83_TXT_01001"), nDay);
                }
            }

            Button buttonCumlativePassive = trInfo.Find("ButtonCumlativePassive").GetComponent<Button>();
            buttonCumlativePassive.onClick.RemoveAllListeners();
            buttonCumlativePassive.onClick.AddListener(() => OnClickDisplayCumlativePassive(true));
            buttonCumlativePassive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonCumlativePassive.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_BTN_00001");

            DisplayCumlativePassive();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCumlativePassive()
    {
        m_trCumulativePassive = m_trPanelTitleInfo.Find("CumulativePassive");
        m_goCumlativeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/TitleCumulativeItem");

        Button buttonClose = m_trCumulativePassive.GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickDisplayCumlativePassive(false));
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //Transform trContent = m_trCumulativePassive.Find("ImageBackground/Scroll View/Viewport/Content");

        Text textCumulative = m_trCumulativePassive.Find("ImageBackground/TextCumlativePassive").GetComponent<Text>();
        textCumulative.text = CsConfiguration.Instance.GetString("A83_TXT_00007");
        CsUIData.Instance.SetFont(textCumulative);

        UpdateCumlativePassive();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCumlativePassive()
    {
        Transform trContent = m_trCumulativePassive.Find("ImageBackground/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; ++i)
        {
            trContent.GetChild(i).Find("TextValue").GetComponent<Text>().text = "0";
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        List<CsTitlePassiveAttr> listAttr = new List<CsTitlePassiveAttr>();
        List<CsHeroTitle> listTitle = CsTitleManager.Instance.HeroTitleList;

        for (int i = 0; i < listTitle.Count; ++i)
        {
            if (listTitle[i].Title.TitleId == CsTitleManager.Instance.ActivationTitleId)
                continue;

            for (int j = 0; j < listTitle[i].Title.TitlePassiveAttrList.Count; ++j)
            {
                listAttr.Add(listTitle[i].Title.TitlePassiveAttrList[j]);
            }
        }

        for (int i = 0; i < listAttr.Count; ++i)
        {
            Transform trAttr = trContent.Find(listAttr[i].Attr.AttrId.ToString());

            if (trAttr == null)
            {
                trAttr = Instantiate(m_goCumlativeItem, trContent).transform;
                trAttr.name = listAttr[i].Attr.AttrId.ToString();

                Text textName = trAttr.Find("TextName").GetComponent<Text>();
                textName.text = listAttr[i].Attr.Name;
                CsUIData.Instance.SetFont(textName);

                Text textValue = trAttr.Find("TextValue").GetComponent<Text>();
                textValue.text = listAttr[i].AttrValue.Value.ToString();
                CsUIData.Instance.SetFont(textValue);
            }
            else
            {
                Text textValue = trAttr.Find("TextValue").GetComponent<Text>();
                int nTotal = int.Parse(textValue.text) + listAttr[i].AttrValue.Value;
                textValue.text = nTotal.ToString();
            }

            if (!trAttr.gameObject.activeSelf)
                trAttr.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTitleList(int nType)
    {
        //칭호리스트초기화
        for (int i = 0; i < m_trTitleListContent.childCount; ++i)
        {
            m_trTitleListContent.GetChild(i).gameObject.SetActive(false);
        }

        m_trTitleListContent.localPosition = Vector3.zero;
        m_nTitleItemLoadIndex = 0;
        m_nStandardPosition = 0;

        int nItemSize = 346;
        int nBaseLoadCount = 9;

        m_listTitle = CsGameData.Instance.TitleList.FindAll(a => a.TitleType.Type == nType);
        m_listTitle.Sort(SortToTitle);

        if (m_listTitle.Count < nBaseLoadCount)
        {
            nBaseLoadCount = m_listTitle.Count;
        }

        RectTransform rectTransform = m_trTitleListContent.GetComponent<RectTransform>();

        int nSize = m_listTitle.Count / 3;
        nSize += (m_listTitle.Count % 3) != 0 ? 1 : 0;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * nSize);

        for (int i = 0; i < nBaseLoadCount; i++)
        {
            CreateTitleItem(m_listTitle[i]);
        }

        Scrollbar scrollbar = m_trPanelTitleList.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.RemoveAllListeners();
        scrollbar.value = 1;
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedTitleScrollbar(scrollbar));

    }

    //---------------------------------------------------------------------------------------------------
    void CreateTitleItem(CsTitle csTitle)
    {
        Transform trTitle = CheckTitlePrefab();
        trTitle.SetSiblingIndex(m_nTitleItemLoadIndex++);

        if (trTitle != null)
        {
            trTitle.name = csTitle.TitleId.ToString();

            Image imageRankLine = trTitle.Find("ImageRankLine").GetComponent<Image>();
            imageRankLine.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_achievement_rank0" + csTitle.TitleGrade.Grade);

            Image imageRank = trTitle.Find("ImageRank").GetComponent<Image>();

            if (csTitle.TitleGrade.Grade > 0 && csTitle.TitleGrade.Grade <= 5)
            {
                imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupAchievement/ico_achievement_title_" + csTitle.TitleGrade.Grade);
                imageRank.gameObject.SetActive(true);
            }
            else
            {
                imageRank.gameObject.SetActive(false);
            }

            Transform trDim = trTitle.Find("ImageDim");

            if (CsTitleManager.Instance.HeroTitleList.Find(a => a.Title.TitleId == csTitle.TitleId) == null)
            {
                trDim.gameObject.SetActive(true);
            }
            else
            {
                trDim.gameObject.SetActive(false);
            }

            trTitle.Find("TextName").GetComponent<Text>().text = string.Format("<color={0}>{1}</color>", csTitle.TitleGrade.ColorCode, csTitle.Name);
            trTitle.Find("ImageLimit/Text").GetComponent<Text>().text = csTitle.AcquisitionText;

            trTitle.Find("Active/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00008");
            trTitle.Find("Active/TextAttr0").GetComponent<Text>().text = csTitle.TitleActiveAttrList[0].Attr.Name;
            trTitle.Find("Active/TextAttr0/TextValue").GetComponent<Text>().text = csTitle.TitleActiveAttrList[0].AttrValue.Value.ToString("#,##0");
            trTitle.Find("Active/TextAttr1").GetComponent<Text>().text = csTitle.TitleActiveAttrList[1].Attr.Name;
            trTitle.Find("Active/TextAttr1/TextValue").GetComponent<Text>().text = csTitle.TitleActiveAttrList[1].AttrValue.Value.ToString("#,##0");

            trTitle.Find("Passive/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00009");
            trTitle.Find("Passive/TextAttr0").GetComponent<Text>().text = csTitle.TitlePassiveAttrList[0].Attr.Name;
            trTitle.Find("Passive/TextAttr0/TextValue").GetComponent<Text>().text = csTitle.TitlePassiveAttrList[0].AttrValue.Value.ToString("#,##0");
            trTitle.Find("Passive/TextAttr1").GetComponent<Text>().text = csTitle.TitlePassiveAttrList[1].Attr.Name;
            trTitle.Find("Passive/TextAttr1/TextValue").GetComponent<Text>().text = csTitle.TitlePassiveAttrList[1].AttrValue.Value.ToString("#,##0");

            trTitle.Find("TextTime").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00010");

            if (csTitle.Lifetime == 0)
            {
                trTitle.Find("TextTime/Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A83_TXT_00011");
            }
            else
            {
                int nDay = csTitle.Lifetime / 86400;
                int nHour = ((csTitle.Lifetime % 86400) / 3600) + 1;

                if (nDay == 0)
                {
                    trTitle.Find("TextTime/Text").GetComponent<Text>().text = string.Format(CsConfiguration.Instance.GetString("A83_TXT_01002"), nHour);
                }
                else
                {
                    trTitle.Find("TextTime/Text").GetComponent<Text>().text = string.Format(CsConfiguration.Instance.GetString("A83_TXT_01001"), nDay);
                }
            }

            trTitle.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    Transform CheckTitlePrefab()
    {
        for (int i = 0; i < m_trTitleListContent.childCount; ++i)
        {
            if (!m_trTitleListContent.GetChild(i).gameObject.activeSelf)
                return m_trTitleListContent.GetChild(i);
        }

        Transform trTitlteItem = Instantiate(m_goTitleItem, m_trTitleListContent).transform;

        foreach (Text text in trTitlteItem.GetComponentsInChildren<Text>(true))
        {
            CsUIData.Instance.SetFont(text);
        }

        return trTitlteItem;
    }

    //---------------------------------------------------------------------------------------------------
    int SortToHeroTitle(CsHeroTitle A, CsHeroTitle B)
    {
        //등급 내림차
        if (A.Title.TitleGrade.Grade < B.Title.TitleGrade.Grade) return 1;
        else if (A.Title.TitleGrade.Grade > B.Title.TitleGrade.Grade) return -1;
        else
        {
            //ID 오름차
            if (A.Title.TitleId > B.Title.TitleId) return 1;
            else if (A.Title.TitleId < B.Title.TitleId) return -1;
            else return 0;
        }
    }

    //---------------------------------------------------------------------------------------------------
    int SortToTitle(CsTitle A, CsTitle B)
    {
        bool bA = CsTitleManager.Instance.HeroTitleList.Find(a => a.Title.TitleId == A.TitleId) != null ? true : false;
        bool bB = CsTitleManager.Instance.HeroTitleList.Find(b => b.Title.TitleId == B.TitleId) != null ? true : false;

        if (bA && bB)
        {
            //내림차순으로 정렬
            if (A.TitleGrade.Grade < B.TitleGrade.Grade) return 1;
            else if (A.TitleGrade.Grade > B.TitleGrade.Grade) return -1;
            else
            {
                if (A.TitleId > B.TitleId) return 1;
                else if (A.TitleId < B.TitleId) return -1;
                else return 0;
            }
        }
        else if (bA)
        {
            return -1;
        }
        else if (bB)
        {
            return 1;
        }
        else
        {
            //내림차순으로 정렬
            if (A.TitleGrade.Grade > B.TitleGrade.Grade) return 1;
            else if (A.TitleGrade.Grade < B.TitleGrade.Grade) return -1;
            else
            {
                if (A.TitleId < B.TitleId) return 1;
                else if (A.TitleId > B.TitleId) return -1;
                else return 0;
            }
        }
    }
}
