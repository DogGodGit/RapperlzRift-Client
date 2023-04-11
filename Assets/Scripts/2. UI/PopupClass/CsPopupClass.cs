using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupClass : CsPopupSub
{
    [SerializeField] GameObject m_goToggleClass;
    GameObject m_goAccAttrItem;

    Transform m_trItemRewardList;
    Transform m_trClassList;
    Transform m_trClassAttr;
    Transform m_trClassInfo;
    Transform m_trImageBackground;

    Slider m_sliderGuageClassUpExp;

    Image m_imageClassIcon;
    Image m_imageClassEmblem;

    Text m_textClass;
    Text m_textDailyExploitValue;
    Text m_textRewardGoldValue;
    Text m_textClassName;
    Text m_textReceiveComplete;
    Text m_textMaxClass;

    Button m_buttonReceive;
    Button m_buttonClassUp;
    Button m_buttonGetExploit;
    Button m_buttonPopupClose;
    Button m_buttonClosePopupAccAttr;

    int m_nSelectRank = 0;

    bool m_bFirst = true;
    bool m_bPopupFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventRankAcquire += OnEventRankAcquire;
        CsGameEventUIToUI.Instance.EventRankRewardReceive += OnEventRankRewardReceive;

        // 공적치 획득 이벤트
        CsSecretLetterQuestManager.Instance.EventHeroSecretLetterQuestCompleted += OnEventHeroSecretLetterQuestCompleted;
        CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxQuestCompleted += OnEventHeroMysteryBoxQuestCompleted;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete += OnEventTrueHeroQuestComplete;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // Start
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        // Destroy
        CsGameEventUIToUI.Instance.EventRankAcquire -= OnEventRankAcquire;
        CsGameEventUIToUI.Instance.EventRankRewardReceive -= OnEventRankRewardReceive;

        // 공적치 획득 이벤트
        CsSecretLetterQuestManager.Instance.EventHeroSecretLetterQuestCompleted -= OnEventHeroSecretLetterQuestCompleted;
        CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxQuestCompleted -= OnEventHeroMysteryBoxQuestCompleted;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete -= OnEventTrueHeroQuestComplete;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        m_nSelectRank = csRank.RankNo;
    }

    #region Event

    // 계급 승진
    //---------------------------------------------------------------------------------------------------
    void OnEventRankAcquire()
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        m_nSelectRank = csRank.RankNo;

        UpdateGuageClassUpExp(csRank);
        UpdateItemRewardList(csRank);
        UpdateRewardGoldValue(csRank);
        UpdateButtonReceive(csRank);
        UpdateToggleClass();
        UpdateImageClassIcon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRankRewardReceive()
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        UpdateButtonReceive(csRank);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroSecretLetterQuestCompleted(System.Guid guid)
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        UpdateGuageClassUpExp(csRank);
        UpdateDailyExploitCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroMysteryBoxQuestCompleted(System.Guid guid)
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        UpdateGuageClassUpExp(csRank);
        UpdateDailyExploitCount();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
		UpdateGuageClassUpExp(csRank);
		UpdateDailyExploitCount();
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonReceive()
    {
        CsCommandEventManager.Instance.SendRankRewardReceive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonClassUp()
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        CsCommandEventManager.Instance.SendRankAcquire(csRank.RankNo + 1);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonGetExploit()
    {
        if (m_bPopupFirst)
        {
            InitializePopup();
            m_bPopupFirst = false;
        }

        m_buttonPopupClose.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonPopupClose()
    {
        m_buttonPopupClose.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleClass(CsRank csRank, bool bIsOn)
    {
        if (bIsOn)
        {
            m_nSelectRank = csRank.RankNo;

            UpdateImageClassEmblem(csRank);
            UpdateTextClassName(csRank);
            UpdateClassAttr(csRank);
            UpdateButtonClassUp(csRank);
            UpdateClassInfo(csRank);
            UpdateRankSkill(csRank);

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonShortCut(int nIndex)
    {
        switch (nIndex)
        {
            case 0:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.DimensionRaid);
                break;
            case 1:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SecretLetter);
                break;
            case 2:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MysteryBox);
                break;
        }

        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupAccAttr()
    {
        OpenPopupAccAttr();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupAccAttr()
    {
        if (m_goAccAttrItem == null)
        {
            StartCoroutine(LoadAccAttrItem());
        }
        else
        {
            DisplayPopupAccAttr();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadAccAttrItem()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupClass/AccAttrItem");
        yield return resourceRequest;

        m_goAccAttrItem = (GameObject)resourceRequest.asset; 
        DisplayPopupAccAttr();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPopupAccAttr()
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);

        Debug.Log("DisplayPopupAccAttr");
        if (csRank == null)
        {
            return;
        }
        else
        {
            Transform trRankAccAttrContent = m_buttonClosePopupAccAttr.transform.Find("ImageBackground/Scroll View/Viewport/Content");
            Transform trAccAttrItem = null;

            for (int i = 0; i < trRankAccAttrContent.childCount; i++)
            {
                trAccAttrItem = trRankAccAttrContent.GetChild(i);
                trAccAttrItem.gameObject.SetActive(false);
            }

            Text textAttrName = null;
            Text textAttrValue = null;

            for (int i = 0; i < csRank.RankAttrList.Count; i++)
            {
                trAccAttrItem = trRankAccAttrContent.Find("AccAttrItem" + i);

                if (trAccAttrItem == null)
                {
                    trAccAttrItem = Instantiate(m_goAccAttrItem, trRankAccAttrContent).transform;
                    trAccAttrItem.name = "AccAttrItem" + i;
                }
                else
                {
                    trAccAttrItem.gameObject.SetActive(true);
                }

                textAttrName = trAccAttrItem.Find("TextAttrName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrName);
                textAttrName.text = csRank.RankAttrList[i].Attr.Name;

                textAttrValue = trAccAttrItem.Find("TextAttrValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);
                textAttrValue.text = csRank.RankAttrList[i].AttrValueInfo.Value.ToString("#,##0");
            }

            m_buttonClosePopupAccAttr.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupAccAttr()
    {
        m_buttonClosePopupAccAttr.gameObject.SetActive(false);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);

        if (csRank == null)
        {
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            return;
        }

        m_nSelectRank = csRank.RankNo;

        m_imageClassIcon = transform.Find("ImageIconClass").GetComponent<Image>();
        m_imageClassIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/ico_class_" + csRank.RankNo);

        m_textClass = transform.Find("TextClass").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textClass);
        m_textClass.text = string.Format(CsConfiguration.Instance.GetString("A27_TXT_01001"), csRank.ColorCode, csRank.Name);

        m_sliderGuageClassUpExp = transform.Find("GuageClassUpExp").GetComponent<Slider>();
        UpdateGuageClassUpExp(csRank);

        Text textDailyExploit = transform.Find("TextDailyExploit").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDailyExploit);
        textDailyExploit.text = CsConfiguration.Instance.GetString("A27_TXT_00007");

        m_textDailyExploitValue = transform.Find("TextDailyExploitValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDailyExploitValue);
        m_textDailyExploitValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.DailyExploitPoint, CsGameData.Instance.MyHeroInfo.VipLevel.DailyMaxExploitPoint);

        m_textRewardGoldValue = transform.Find("ImageRewardGold/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRewardGoldValue);
        m_textRewardGoldValue.text = csRank.GoldReward.Value.ToString("#,##0");

        m_trItemRewardList = transform.Find("ItemRewardList");
        UpdateItemRewardList(csRank);
        UpdateRewardGoldValue(csRank);

        m_imageClassEmblem = transform.Find("ImageClassEmblem").GetComponent<Image>();
        UpdateImageClassEmblem(csRank);

        m_textClassName = transform.Find("TextClassName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textClassName);
        UpdateTextClassName(csRank);

        m_trClassList = transform.Find("ClassList/Viewport/Content");

        m_textReceiveComplete = transform.Find("TextReceiveComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textReceiveComplete);
        m_textReceiveComplete.text = CsConfiguration.Instance.GetString("A27_TXT_02001");

        m_buttonReceive = transform.Find("ButtonReceive").GetComponent<Button>();
        m_buttonReceive.onClick.RemoveAllListeners();
        m_buttonReceive.onClick.AddListener(OnClickButtonReceive);
        m_buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonReceive = m_buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceive);
        textButtonReceive.text = CsConfiguration.Instance.GetString("A27_BTN_00002");

        UpdateButtonReceive(csRank);

        m_trClassAttr = transform.Find("ClassAttr");
        UpdateClassAttr(csRank);
        UpdateRankSkill(csRank);

        m_buttonClassUp = transform.Find("ButtonClassUp").GetComponent<Button>();
        m_buttonClassUp.onClick.RemoveAllListeners();
        m_buttonClassUp.onClick.AddListener(() => OnClickButtonClassUp());
        m_buttonClassUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonClassUp = m_buttonClassUp.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonClassUp);
        textButtonClassUp.text = CsConfiguration.Instance.GetString("A27_BTN_00003");

        UpdateButtonClassUp(csRank);

        m_trClassInfo = transform.Find("ClassInfo");

        m_textMaxClass = transform.Find("TextMaxClass").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMaxClass);
        m_textMaxClass.text = CsConfiguration.Instance.GetString("A27_TXT_00005");

        UpdateClassInfo(csRank);

        Button buttonOpenPopupAccAttr = m_trClassAttr.Find("ButtonOpenPopupAccAttr").GetComponent<Button>();
        buttonOpenPopupAccAttr.onClick.RemoveAllListeners();
        buttonOpenPopupAccAttr.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonOpenPopupAccAttr.onClick.AddListener(() => OnClickOpenPopupAccAttr());

        m_buttonGetExploit = transform.Find("ButtonGetExploit").GetComponent<Button>();
        m_buttonGetExploit.onClick.RemoveAllListeners();
        m_buttonGetExploit.onClick.AddListener(OnClickButtonGetExploit);
        m_buttonGetExploit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        //A27_BTN_00001 / 획득가기

        m_buttonPopupClose = transform.Find("ButtonPopupClose").GetComponent<Button>();
        m_buttonPopupClose.onClick.RemoveAllListeners();
        m_buttonPopupClose.onClick.AddListener(OnClickButtonPopupClose);
        m_buttonPopupClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trImageBackground = m_buttonPopupClose.transform.Find("ImageBackground");

        m_buttonClosePopupAccAttr = transform.Find("ButtonClosePopupAccAttr").GetComponent<Button>();
        m_buttonClosePopupAccAttr.onClick.RemoveAllListeners();
        m_buttonClosePopupAccAttr.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonClosePopupAccAttr.onClick.AddListener(() => OnClickClosePopupAccAttr());

        Text textButtonOpenPopupAccAttr = m_trClassAttr.Find("ButtonOpenPopupAccAttr/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonOpenPopupAccAttr);
        textButtonOpenPopupAccAttr.text = CsConfiguration.Instance.GetString("PUBLIC_ACC_ATTRV");

        Text textPopupAccAttrName = m_buttonClosePopupAccAttr.transform.Find("ImageBackground/TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupAccAttrName);
        textPopupAccAttrName.text = CsConfiguration.Instance.GetString("PUBLIC_ACC_ATTR");

        for (int i = 0; i < CsGameData.Instance.RankList.Count; i++)
        {
            csRank = CsGameData.Instance.RankList[i];
            CreateToggleClass(csRank);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateToggleClass(CsRank csRank)
    {
        Transform trToggleClass = Instantiate(m_goToggleClass, m_trClassList).transform;
        trToggleClass.name = "ToggleClass" + csRank.RankNo;

        Text textClass = trToggleClass.Find("TextClass").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClass);
        textClass.text = string.Format(CsConfiguration.Instance.GetString("A27_TXT_01001"), csRank.ColorCode, csRank.Name);

        Transform trImageDim = trToggleClass.Find("ImageDim");

        if (csRank.RankNo <= m_nSelectRank)
        {
            trImageDim.gameObject.SetActive(false);
        }
        else
        {
            trImageDim.gameObject.SetActive(true);
        }

        Toggle toggleClass = trToggleClass.GetComponent<Toggle>();
        toggleClass.group = m_trClassList.GetComponent<ToggleGroup>();
        toggleClass.onValueChanged.RemoveAllListeners();

        if (csRank.RankNo == m_nSelectRank)
        {
            toggleClass.isOn = true;
        }

        toggleClass.onValueChanged.AddListener((isOn) => OnValueChangedToggleClass(csRank, isOn));
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopup()
    {
        Text textName = m_trImageBackground.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsConfiguration.Instance.GetString("A27_BTN_00004");
        //A27_NAME_00001

        Text textGoods = m_trImageBackground.Find("ImageGoods/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGoods);
        textGoods.text = CsConfiguration.Instance.GetString("A27_TXT_00010");

        Text textDescription = m_trImageBackground.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString("A27_TXT_00011");

        Transform trShortCutList = m_trImageBackground.Find("ShortCutList");

        for (int i = 0; i < trShortCutList.childCount; i++)
        {
            int nIndex = i;
            Button buttonShortCut = trShortCutList.GetChild(i).GetComponent<Button>();
            buttonShortCut.onClick.RemoveAllListeners();
            buttonShortCut.onClick.AddListener(() => OnClickButtonShortCut(nIndex));
            buttonShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
            {
                buttonShortCut.interactable = false;
            }
            else if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
            {
                buttonShortCut.interactable = true;
            }
            else
            {
                buttonShortCut.interactable = false;
            }

            Text textButtonShortCut = buttonShortCut.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonShortCut);

            switch (nIndex)
            {
                case 0:
                    textButtonShortCut.text = CsConfiguration.Instance.GetString("A27_TXT_00012");
                    break;
                case 1:
                    textButtonShortCut.text = CsConfiguration.Instance.GetString("A27_TXT_00013");
                    break;
                case 2:
                    textButtonShortCut.text = CsConfiguration.Instance.GetString("A27_TXT_00014");
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageClassIcon()
    {
        CsRank csRank = CsGameData.Instance.GetRank(CsGameData.Instance.MyHeroInfo.RankNo);
        m_imageClassIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/ico_class_" + CsGameData.Instance.MyHeroInfo.RankNo);
        m_textClass.text = string.Format(CsConfiguration.Instance.GetString("A27_TXT_01001"), csRank.ColorCode, csRank.Name);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageClassEmblem(CsRank csRank)
    {
        m_imageClassEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/ico_class_" + csRank.RankNo);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextClassName(CsRank csRank)
    {
        m_textClassName.text = string.Format(CsConfiguration.Instance.GetString("A27_TXT_01001"), csRank.ColorCode, csRank.Name);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuageClassUpExp(CsRank csRank)
    {
        CsRank csRankNext = CsGameData.Instance.GetRank(csRank.RankNo + 1);

        Text textRankUpExploitValue = transform.Find("ImageRankUpExploit/TextRankupExploitValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankUpExploitValue);

        if (csRankNext == null)
        {
            // 최대 계급 표시
            m_sliderGuageClassUpExp.maxValue = csRank.RequiredExploitPoint;
            m_sliderGuageClassUpExp.value = CsGameData.Instance.MyHeroInfo.ExploitPoint;

            textRankUpExploitValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), csRank.RequiredExploitPoint, csRank.RequiredExploitPoint);
        }
        else
        {
            m_sliderGuageClassUpExp.maxValue = csRankNext.RequiredExploitPoint;
            m_sliderGuageClassUpExp.value = CsGameData.Instance.MyHeroInfo.ExploitPoint;

            textRankUpExploitValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.ExploitPoint, csRankNext.RequiredExploitPoint);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateItemRewardList(CsRank csRank)
    {
        Transform trItemSlot = null;

        for (int i = 0; i < csRank.RankRewardList.Count; i++)
        {
            CsItemReward csItemReward = csRank.RankRewardList[i].ItemReward;
            trItemSlot = m_trItemRewardList.Find("ItemSlot" + i);

            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Small, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonReceive(CsRank csRank)
    {
        // 보상 받은 날짜와 현재 날짜, 보상 계급과 현재 계급이 같을 경우 이미 보상을 받음
        if ((System.DateTime.Compare(CsGameData.Instance.MyHeroInfo.RankRewardReceivedDate.Date, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0) && CsGameData.Instance.MyHeroInfo.RankRewardReceivedRankNo == CsGameData.Instance.MyHeroInfo.RankNo)
        {
            m_buttonReceive.gameObject.SetActive(false);
            m_textReceiveComplete.gameObject.SetActive(true);
        }
        else
        {
            m_textReceiveComplete.gameObject.SetActive(false);
            m_buttonReceive.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleClass()
    {
        for (int i = 0; i < CsGameData.Instance.RankList.Count; i++)
        {
            CsRank csRank = CsGameData.Instance.RankList[i];

            Transform trToggleClass = m_trClassList.Find("ToggleClass" + csRank.RankNo);
            Transform trImageDim = trToggleClass.Find("ImageDim");

            if (csRank.RankNo <= m_nSelectRank)
            {
                trImageDim.gameObject.SetActive(false);
            }
            else
            {
                trImageDim.gameObject.SetActive(true);
            }

            Toggle toggleClass = trToggleClass.GetComponent<Toggle>();

            if (csRank.RankNo == m_nSelectRank)
            {
                toggleClass.isOn = true;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankSkill(CsRank csRank)
    {
        Text textRankActiveSkill = m_trClassAttr.Find("TextRankActiveSkill").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankActiveSkill);

        Text textRankPassiveSkill = m_trClassAttr.Find("TextRankPassiveSkill").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankPassiveSkill);

        CsRankActiveSkill csRankActiveSkill = CsGameData.Instance.RankActiveSkillList.Find(a => a.RequiredRankNo == csRank.RankNo);

        if (csRankActiveSkill == null)
        {
            textRankActiveSkill.gameObject.SetActive(false);
        }
        else
        {
            textRankActiveSkill.text = string.Format(CsConfiguration.Instance.GetString("A27_TXT_00015"), csRankActiveSkill.Name);
            textRankActiveSkill.gameObject.SetActive(true);
        }

        CsRankPassiveSkill csRankPassiveSkill = CsGameData.Instance.RankPassiveSkillList.Find(a => a.RequiredRankNo == csRank.RankNo);

        if (csRankPassiveSkill == null)
        {
            textRankPassiveSkill.gameObject.SetActive(false);
        }
        else
        {
            textRankPassiveSkill.text = string.Format(CsConfiguration.Instance.GetString("A27_TXT_00015"), csRankPassiveSkill.Name);
            textRankPassiveSkill.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateClassAttr(CsRank csRank)
    {
        // 초기화
        for (int i = 0; i < m_trClassAttr.childCount; i++)
        {
            Transform trOptionAttr = m_trClassAttr.Find("OptionAttr" + i);

            if (trOptionAttr != null)
            {
                trOptionAttr.gameObject.SetActive(false);
            }
        }

        Dictionary<int, int> dicRankAttr = new Dictionary<int, int>();

        for (int i = 0; i < csRank.RankAttrList.Count; i++)
        {
            dicRankAttr.Add(csRank.RankAttrList[i].Attr.AttrId, csRank.RankAttrList[i].AttrValueInfo.Value);
        }

        if (csRank.RankNo == 1)
        {

        }
        else
        {
            CsRank csRankPrev = CsGameData.Instance.GetRank(csRank.RankNo - 1);

            if (csRankPrev == null)
            {
                return;
            }
            else
            {
                for (int i = 0; i < csRankPrev.RankAttrList.Count; i++)
                {
                    if (dicRankAttr.ContainsKey(csRankPrev.RankAttrList[i].Attr.AttrId))
                    {
                        dicRankAttr[csRankPrev.RankAttrList[i].Attr.AttrId] -= csRankPrev.RankAttrList[i].AttrValueInfo.Value;

                        if (dicRankAttr[csRankPrev.RankAttrList[i].Attr.AttrId] <= 0)
                        {
                            dicRankAttr.Remove(csRankPrev.RankAttrList[i].Attr.AttrId);
                        }
                    }
                    else
                    {
                        dicRankAttr.Add(csRankPrev.RankAttrList[i].Attr.AttrId, csRankPrev.RankAttrList[i].AttrValueInfo.Value);
                    }
                }
            }
        }

        int nCount = 0;

        foreach (KeyValuePair<int, int> item in dicRankAttr)
        {
            Transform trOptionAttr = m_trClassAttr.Find("OptionAttr" + nCount);
            trOptionAttr.gameObject.SetActive(true);

            Text textAttrName = trOptionAttr.Find("TextAttrName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = CsGameData.Instance.GetAttr(item.Key).Name;

            Text textAttrValue = trOptionAttr.Find("TextAttrValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);

            float attrValue = (float)dicRankAttr[item.Key];

            switch (item.Key)
            {
                case 6:
                    attrValue = attrValue / (attrValue + 6000) * 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 7:
                    attrValue = attrValue / (attrValue + 6000) * 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 8:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 9:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 10:
                    attrValue = attrValue / (attrValue + 3000) * 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 11:
                    attrValue = attrValue / (attrValue + 3000) * 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 20:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 21:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 22:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 23:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 25:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 26:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 27:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                case 28:
                    attrValue /= 100.0f;
                    textAttrValue.text = attrValue.ToString("0.#");
                    break;
                default:
                    textAttrValue.text = attrValue.ToString("#,##0");
                    break;
            }

            nCount++;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonClassUp(CsRank csRank)
    {
        if (CsGameData.Instance.MyHeroInfo.RankNo < CsGameData.Instance.RankList.Count)
        {
            if (m_nSelectRank == CsGameData.Instance.MyHeroInfo.RankNo)
            {
                CsRank csRankNext = CsGameData.Instance.GetRank(csRank.RankNo + 1);

                if (CsGameData.Instance.MyHeroInfo.ExploitPoint < csRankNext.RequiredExploitPoint)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonClassUp, false);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonClassUp, true);
                }

                m_buttonClassUp.gameObject.SetActive(true);
            }
            else
            {
                m_buttonClassUp.gameObject.SetActive(false);
            }
        }
        else
        {
            m_buttonClassUp.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateClassInfo(CsRank csRank)
    {
        Transform m_trImageLock = m_trClassInfo.Find("ImageLock");

        Text textClassInfo = m_trClassInfo.Find("TextClassInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClassInfo);

        // 잠금 계급
        if (m_nSelectRank > CsGameData.Instance.MyHeroInfo.RankNo)
        {
            m_trImageLock.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/ico_lock_on");
            m_trImageLock.gameObject.SetActive(true);
            textClassInfo.text = CsConfiguration.Instance.GetString("A27_TXT_00009");
        }
        // 잠금 해제된 계급
        else if (m_nSelectRank < CsGameData.Instance.MyHeroInfo.RankNo)
        {
            m_trImageLock.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/ico_lock_off");
            m_trImageLock.gameObject.SetActive(true);
            textClassInfo.text = CsConfiguration.Instance.GetString("A27_TXT_00004");
        }
        // 현재 계급
        else
        {
            m_trImageLock.gameObject.SetActive(false);
            textClassInfo.text = CsConfiguration.Instance.GetString("A27_TXT_00008");
        }

        if (m_nSelectRank == CsGameData.Instance.RankList.Count)
        {
            m_textMaxClass.gameObject.SetActive(true);
        }
        else
        {
            m_textMaxClass.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDailyExploitCount()
    {
        m_textDailyExploitValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.DailyExploitPoint, CsGameData.Instance.MyHeroInfo.VipLevel.DailyMaxExploitPoint);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRewardGoldValue(CsRank csRank)
    {
        if (csRank == null)
        {
            return;
        }
        else
        {
            m_textRewardGoldValue.text = csRank.GoldReward.Value.ToString("#,##0");
        }
    }
}