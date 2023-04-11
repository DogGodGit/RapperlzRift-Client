using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-18)
//---------------------------------------------------------------------------------------------------

public class CsMainGearRefine : CsPopupSub
{
    const int c_nMaxAttrCount = 5;

    GameObject m_goPopupItemInfo;

    Transform m_trPopupList;
    Transform m_trSlotMainGear;
    Transform m_trSlotRefinementItem;
    Transform m_trSlotProtectionItem;
    Transform m_trToggleList;
    Transform m_trMainGearRefineInfo;
    Transform m_trChangeAttr;
    Transform m_trPopupMultiRefine;
    Transform m_trPopupItemInfo;

    Button m_buttonRefine;
    Button m_buttonSave;
    Button m_buttonPopupMultiRefine;
    Button m_buttonPopupSave;

    int m_nTurn = 1;

    bool m_bFirst = true;
    bool m_bOpenPopupFirst = true;
    bool m_bRefine = false;
    bool m_bMultiRefine = false;
    bool m_bRefineDaily = false;
    bool m_bSameHeroMainGear = false;

    Guid m_guidPrevMainGearId;
    CsHeroMainGear m_csHeroMainGear;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMainGearSelected += OnEventMainGearSelected;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMainGearSelected -= OnEventMainGearSelected;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }
        else
        {
            if (CsUIData.Instance.MainGearId == Guid.Empty)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                DisplayUpdate(m_csHeroMainGear);
            }
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearSelected(Guid guidHeroMainGearId)
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.MainGearRefine)
        {
            if (guidHeroMainGearId == Guid.Empty)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                if (m_guidPrevMainGearId == guidHeroMainGearId)
                {
                    m_bSameHeroMainGear = true;
                }
                else
                {
                    m_guidPrevMainGearId = guidHeroMainGearId;
                    m_bSameHeroMainGear = false;
                }

                m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroMainGearId);
                DisplayUpdate(m_csHeroMainGear);
                transform.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMainGearRefine()
    {
        // 막을 인덱스 저장
        List<int> listIndices = new List<int>();

        for (int i = 0; i < m_csHeroMainGear.OptionAttrList.Count; i++)
        {
            Toggle toggleAttr = m_trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

            if (toggleAttr.isOn)
            {
                listIndices.Add(m_csHeroMainGear.OptionAttrList[i].Index);
            }
        }

        CsCommandEventManager.Instance.SendMainGearRefine(m_csHeroMainGear.Id, true, listIndices.ToArray());
    }

    // 다중 마법 부여
    //---------------------------------------------------------------------------------------------------
    void OnClickMultiMainGearRefine()
    {
        Transform trToggleList = m_trPopupMultiRefine.Find("ToggleList");
        // 막을 인덱스 저장
        List<int> listIndices = new List<int>();

        for (int i = 0; i < m_csHeroMainGear.OptionAttrList.Count; i++)
        {
            Toggle toggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

            if (toggleAttr.isOn)
            {
                listIndices.Add(m_csHeroMainGear.OptionAttrList[i].Index);
            }
        }

        CsCommandEventManager.Instance.SendMainGearRefine(m_csHeroMainGear.Id, false, listIndices.ToArray());
    }

    // 마법 부여 저장
    //---------------------------------------------------------------------------------------------------
    void OnClickRefinementApply()
    {
        CsCommandEventManager.Instance.SendMainGearRefinementApply(m_csHeroMainGear.Id, m_nTurn);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenMultiMainGearRefine()
    {
        if (m_bOpenPopupFirst)
        {
            InitializePopupMultiRefine();
            m_bOpenPopupFirst = false;
        }

        UpdatePopupMultiRefineOptionAttr(m_csHeroMainGear);
        UpdatePopupMultiRefineChangeAttrs(m_csHeroMainGear);
        UpdatePopupMultiRefineMaterial();
        UpdateTextDailyCount(m_trPopupMultiRefine);

        m_trPopupMultiRefine.parent.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseMultiMainGearRefine()
    {
        if (m_csHeroMainGear.HeroMainGearRefinementList.Count != 0)
        {
            m_nTurn = m_csHeroMainGear.HeroMainGearRefinementList[0].Turn;
        }

        m_trPopupMultiRefine.parent.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedProtectAttribute(Toggle toggleAttr)
    {
        // 잠금 해제 이미지
        Transform trLock = toggleAttr.transform.Find("ImageLock");

        if (toggleAttr.isOn)
        {
            trLock.gameObject.SetActive(false);
        }
        else
        {
            trLock.gameObject.SetActive(true);
        }

        if (m_trPopupMultiRefine.parent.gameObject.activeSelf)
        {
            UpdateHeroMainGearOptionAttrToggleList(m_trPopupMultiRefine);
            UpdatePopupMultiRefineMaterial();
        }
        else
        {
            UpdateHeroMainGearOptionAttrToggleList(transform);
            UpdateSlotRefinementItem();
            UpdateSlotProtectionItem();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMultiRefine(Toggle toggleRefine, int nToggleIndex)
    {
        if (toggleRefine.isOn)
        {
            m_nTurn = m_csHeroMainGear.HeroMainGearRefinementList[nToggleIndex].Turn;
            CsUIData.Instance.DisplayButtonInteractable(m_buttonPopupSave, true);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemInfo(CsItem csItem)
    {
        StartCoroutine(LoadPopupItemInfo(csItem));
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    public void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");
        // 메인기어 / 재료 슬롯
        Transform trItemGrid = transform.Find("ItemGrid");
        m_trSlotMainGear = trItemGrid.Find("ItemSlotMain");
        m_trSlotRefinementItem = trItemGrid.Find("ItemSlotMaterial");
        m_trSlotProtectionItem = trItemGrid.Find("ItemSlotProtect");

        Button buttonRefinementItem = m_trSlotRefinementItem.GetComponent<Button>();
        buttonRefinementItem.onClick.RemoveAllListeners();
        buttonRefinementItem.onClick.AddListener(() => OnClickItemInfo(CsGameData.Instance.GetItem(CsGameConfig.Instance.MainGearRefinementItemId)));
        buttonRefinementItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //토글 리스트
        m_trToggleList = transform.Find("ToggleList");
        InitializeMainGearAttr(transform);
        // 바뀐 아이템 속성
        m_trChangeAttr = transform.Find("ChangeAttr");
        InitializeChangeAttr(m_trChangeAttr);

        m_trMainGearRefineInfo = transform.Find("Scroll View");
        m_trMainGearRefineInfo.gameObject.SetActive(false);

        Text textMainGearRefineInfo = m_trMainGearRefineInfo.Find("Viewport/Content/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMainGearRefineInfo);
        textMainGearRefineInfo.text = CsConfiguration.Instance.GetString("A07_TXT_00001");

        // 버튼 초기화
        m_buttonRefine = transform.Find("ButtonInherit").GetComponent<Button>();
        m_buttonRefine.onClick.RemoveAllListeners();
        m_buttonRefine.onClick.AddListener(() => OnClickMainGearRefine());
        m_buttonRefine.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonInherit = m_buttonRefine.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonInherit);
        textButtonInherit.text = CsConfiguration.Instance.GetString("A07_BTN_00003");

        Button buttonPopupMultiRefine = transform.Find("ButtonInheritMulti").GetComponent<Button>();
        buttonPopupMultiRefine.onClick.RemoveAllListeners();
        buttonPopupMultiRefine.onClick.AddListener(() => OnClickOpenMultiMainGearRefine());
        buttonPopupMultiRefine.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonInheritMulti = buttonPopupMultiRefine.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonInheritMulti);
        textButtonInheritMulti.text = CsConfiguration.Instance.GetString("A07_BTN_00001");

        m_buttonSave = transform.Find("ButtonInheritSave").GetComponent<Button>();
        m_buttonSave.onClick.RemoveAllListeners();
        m_buttonSave.onClick.AddListener(() => OnClickRefinementApply());
        m_buttonSave.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonInheritSave = m_buttonSave.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonInheritSave);
        textButtonInheritSave.text = CsConfiguration.Instance.GetString("A07_BTN_00002");

        m_trPopupMultiRefine = transform.Find("PopupMultiRefine/ImageBackground");
        m_trPopupMultiRefine.parent.gameObject.SetActive(false);

        FirstMainGearCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupMultiRefine()
    {
        // 팝업 이름
        Text textPopupName = m_trPopupMultiRefine.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A07_NAME_00001");

        InitializeMainGearAttr(m_trPopupMultiRefine);

        // 다중 마법 부여 버튼
        m_buttonPopupMultiRefine = m_trPopupMultiRefine.Find("ButtonMultiInherit").GetComponent<Button>();
        m_buttonPopupMultiRefine.onClick.RemoveAllListeners();
        m_buttonPopupMultiRefine.onClick.AddListener(() => OnClickMultiMainGearRefine());
        m_buttonPopupMultiRefine.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textMultiRefine = m_buttonPopupMultiRefine.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMultiRefine);
        textMultiRefine.text = CsConfiguration.Instance.GetString("A07_BTN_00004");

        // 세련 저장 버튼
        m_buttonPopupSave = m_trPopupMultiRefine.Find("ButtonSave").GetComponent<Button>();
        m_buttonPopupSave.onClick.RemoveAllListeners();
        m_buttonPopupSave.onClick.AddListener(() => OnClickRefinementApply());
        m_buttonPopupSave.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textInheritSave = m_buttonPopupSave.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInheritSave);
        textInheritSave.text = CsConfiguration.Instance.GetString("A07_BTN_00005");

        Button buttonClose = m_trPopupMultiRefine.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickCloseMultiMainGearRefine());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        // 다중 마법 부여 속성
        Transform trChangeInheritList = m_trPopupMultiRefine.Find("ChangeInheritList");

        for (int i = 0; i < 3; i++)
        {
            Transform trChangeInherit = trChangeInheritList.Find("ToggleInheritAttr" + i);

            Transform trChangeAttr = trChangeInherit.Find("ChangeAttr");
            InitializeChangeAttr(trChangeAttr);

            Text textNoAttr = trChangeInherit.Find("TextNoAttr").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNoAttr);
            textNoAttr.text = CsConfiguration.Instance.GetString("A07_TXT_00003");

            int nToggleIndex = i;

            Toggle toggleChangeInherit = trChangeInherit.GetComponent<Toggle>();
            toggleChangeInherit.isOn = false;
            toggleChangeInherit.onValueChanged.RemoveAllListeners();
            toggleChangeInherit.onValueChanged.AddListener((ison) => OnValueChangedMultiRefine(toggleChangeInherit, nToggleIndex));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeMainGearAttr(Transform trPopupRefine)
    {
        // 아이템 전투력
        Text textBattlePower = trPopupRefine.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePower);
        textBattlePower.text = CsConfiguration.Instance.GetString("A07_TXT_00002");

        Text textBattlePowerValue = trPopupRefine.Find("TextBattlePowerValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePowerValue);

        // 아이템 속성 잠금
        Transform trToggleList = trPopupRefine.Find("ToggleList");

        // 아이템 속성
        Transform trAttrList = trPopupRefine.Find("AttrList");

        for (int i = 0; i < c_nMaxAttrCount; i++)
        {
            // 아이템 속성 잠금
            Toggle toggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();
            toggleAttr.isOn = false;
            toggleAttr.onValueChanged.AddListener((ison) => OnValueChangedProtectAttribute(toggleAttr));
            toggleAttr.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));
            toggleAttr.gameObject.SetActive(false);

            // 아이템 속성
            Transform trAttr = trAttrList.Find("OptionAttribute" + i);
            trAttr.gameObject.SetActive(false);
        }

        Text textDayCount = trPopupRefine.Find("TextDayCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDayCount);
        textDayCount.text = string.Format(CsConfiguration.Instance.GetString("A07_TXT_01001"), CsGameData.Instance.MyHeroInfo.MainGearRefineDailyCount, 20);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeChangeAttr(Transform trChangeAttr)
    {
        Text textBattlePower = trChangeAttr.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePower);
        textBattlePower.text = CsConfiguration.Instance.GetString("A07_TXT_00002");

        Text textBattlePowerValue = trChangeAttr.Find("TextBattlePowerValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePowerValue);

        Transform trChangeAttrList = trChangeAttr.Find("ChangeAttrList");

        for (int i = 0; i < c_nMaxAttrCount; i++)
        {
            Transform trAttr = trChangeAttrList.Find("OptionAttribute" + i);
            trAttr.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void FirstMainGearCheck()
    {
        if (CsUIData.Instance.MainGearId == Guid.Empty)
        {
            List<CsHeroMainGear> listHeroMainGearEquip = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList;

            for (int i = 0; i < listHeroMainGearEquip.Count; i++)
            {
                if (listHeroMainGearEquip[i] == null)
                    continue;

                CsUIData.Instance.MainGearId = listHeroMainGearEquip[i].Id;

                break;
            }
        }

        m_guidPrevMainGearId = CsUIData.Instance.MainGearId;
        m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(m_guidPrevMainGearId);

        if (m_csHeroMainGear == null)
        {
            transform.gameObject.SetActive(false);
        }
        else
        {
            DisplayUpdate(m_csHeroMainGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayUpdate(CsHeroMainGear csHeroMainGear)
    {
        UpdateSlotMainGear(csHeroMainGear);
        UpdateHeroMainGearOptionAttr(transform, csHeroMainGear);
        UpdateHeroMainGearChangeAttr(csHeroMainGear);

        // 재료 슬롯 업데이트 부여석 / 보호석
        UpdateSlotRefinementItem();
        UpdateSlotProtectionItem();

        UpdateTextDailyCount(transform);

        if (m_trPopupMultiRefine.parent.gameObject.activeSelf)
        {
            UpdatePopupMultiRefineOptionAttr(csHeroMainGear);
            UpdatePopupMultiRefineChangeAttrs(csHeroMainGear);
            UpdatePopupMultiRefineMaterial();

            UpdateTextDailyCount(m_trPopupMultiRefine);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSlotMainGear(CsHeroMainGear csHeroMainGear)
    {
        // 메인 장비 슬롯 업데이트
        CsUIData.Instance.DisplayItemSlot(m_trSlotMainGear, csHeroMainGear, EnItemSlotSize.Large);

        Text textMainGearName = m_trSlotMainGear.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMainGearName);
        textMainGearName.text = csHeroMainGear.MainGear.Name;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSlotRefinementItem()
    {
        // 재료 슬롯 업데이트 부여석
        CsItem csItemRefinementItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.MainGearRefinementItemId);
        int nRefinementItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemRefinementItem.ItemId);

        // 아이템 슬롯 초기화
        CsUIData.Instance.DisplayItemSlot(m_trSlotRefinementItem, csItemRefinementItem, false, nRefinementItemCount, csItemRefinementItem.UsingRecommendationEnabled, EnItemSlotSize.Large);

        string strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRefinementItemCount, 1);

        Text textName = m_trSlotRefinementItem.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csItemRefinementItem.Name;

        Text textCount = m_trSlotRefinementItem.Find("Item/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);
        textCount.text = strItemCount;

        if (1 <= nRefinementItemCount)
        {
            m_bRefine = true;
            textCount.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            m_bRefine = false;
            textCount.color = CsUIData.Instance.ColorRed;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSlotProtectionItem()
    {
        int nAttrLockCount = GetAttrLockCount(m_trToggleList);

        CsItem csItemProtectionItem = null;
        int nProtectionItemCount = 0;

        Image imageDim = m_trSlotProtectionItem.Find("ImageCooltime").GetComponent<Image>();

        Button buttonProtectionItem = m_trSlotProtectionItem.GetComponent<Button>();

        if (nAttrLockCount == 0)
        {
            csItemProtectionItem = CsGameData.Instance.GetMainGearRefinementRecipe(nAttrLockCount + 1).ProtectionItem;
            nProtectionItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemProtectionItem.ItemId);

            CsUIData.Instance.DisplayItemSlot(m_trSlotProtectionItem, csItemProtectionItem, false, nProtectionItemCount, csItemProtectionItem.UsingRecommendationEnabled, EnItemSlotSize.Large);

            imageDim.gameObject.SetActive(true);
            imageDim.fillAmount = 1.0f;

            // 이름과 수량을 회색으로
            string strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nProtectionItemCount, 1);

            Text textName = m_trSlotProtectionItem.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csItemProtectionItem.Name;
            textName.color = CsUIData.Instance.ColorGray;

            Text textCount = m_trSlotProtectionItem.Find("Item/TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);
            textCount.text = strItemCount;
            textCount.color = CsUIData.Instance.ColorGray;

            buttonProtectionItem.onClick.RemoveAllListeners();
            buttonProtectionItem.onClick.AddListener(() => OnClickItemInfo(csItemProtectionItem));
            buttonProtectionItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }
        else
        {
            csItemProtectionItem = CsGameData.Instance.GetMainGearRefinementRecipe(nAttrLockCount).ProtectionItem;
            nProtectionItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemProtectionItem.ItemId);

            CsUIData.Instance.DisplayItemSlot(m_trSlotProtectionItem, csItemProtectionItem, false, nProtectionItemCount, csItemProtectionItem.UsingRecommendationEnabled, EnItemSlotSize.Large);

            string strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nProtectionItemCount, 1);

            Text textName = m_trSlotProtectionItem.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csItemProtectionItem.Name;
            textName.color = CsUIData.Instance.ColorWhite;

            Text textCount = m_trSlotProtectionItem.Find("Item/TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);
            textCount.text = strItemCount;

            if (1 <= nProtectionItemCount)
            {
                textCount.color = CsUIData.Instance.ColorWhite;

                imageDim.gameObject.SetActive(false);
            }
            else
            {
                m_bRefine = false;
                textCount.color = CsUIData.Instance.ColorRed;

                imageDim.fillAmount = 1.0f;
                imageDim.gameObject.SetActive(true);
            }

            buttonProtectionItem.onClick.RemoveAllListeners();
            buttonProtectionItem.onClick.AddListener(() => OnClickItemInfo(csItemProtectionItem));
            buttonProtectionItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        int nRemainingRefinementCount = CsGameData.Instance.MyHeroInfo.VipLevel.MainGearRefinementMaxCount - CsGameData.Instance.MyHeroInfo.MainGearRefineDailyCount;

        if (nRemainingRefinementCount <= 0)
        {
            m_bRefineDaily = false;
        }
        else
        {
            m_bRefineDaily = true;
        }

        m_bRefine = m_bRefine && m_bRefineDaily;

        CsUIData.Instance.DisplayButtonInteractable(m_buttonRefine, m_bRefine);
    }

    // 메인 장비 옵션 변환
    //---------------------------------------------------------------------------------------------------
    void UpdateHeroMainGearOptionAttr(Transform trPopupMainGearRefine, CsHeroMainGear csHeroMainGear)
    {
        // 아이템 속성 초기화
        // 마법 부여 아이템 속성
        Transform trAttrList = trPopupMainGearRefine.Find("AttrList");
        //토글 초기화
        Transform trToggleList = trPopupMainGearRefine.Find("ToggleList");

        // 속성 초기화
        for (int i = 0; i < c_nMaxAttrCount; i++)
        {
            Transform trAttr = trAttrList.Find("OptionAttribute" + i);
            trAttr.gameObject.SetActive(false);

            // 토글 초기화
            Toggle toggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();
            toggleAttr.gameObject.SetActive(false);

            // 다른 아이템이라면 토글 초기화
            if (!m_bSameHeroMainGear)
            {
                if (toggleAttr.isOn)
                {
                    toggleAttr.isOn = false;
                }
            }
        }

        // 메인 장비 속성
        string strColorCode = "";

        for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; i++)
        {
            Transform trAttr = trAttrList.Find("OptionAttribute" + i);
            trAttr.gameObject.SetActive(true);

            Toggle toggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();
            toggleAttr.gameObject.SetActive(true);

            strColorCode = csHeroMainGear.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode;

            Text textAttrName = trAttr.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = string.Format("<color={0}>{1}</color>", strColorCode, csHeroMainGear.OptionAttrList[i].Attr.Name);

            Text textAttrValue = trAttr.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);
            textAttrValue.text = string.Format("<color={0}>{1}</color>", strColorCode, csHeroMainGear.OptionAttrList[i].Value);
        }

        Text textBattlePowewrValue = trPopupMainGearRefine.Find("TextBattlePowerValue").GetComponent<Text>();
        textBattlePowewrValue.text = csHeroMainGear.OptionAttributesBattlePower.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroMainGearOptionAttrToggleList(Transform trMainGearOption)
    {
        CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(CsUIData.Instance.MainGearId);

        Transform trToggleList = trMainGearOption.Find("ToggleList");

        if (GetAttrLockCount(trToggleList) >= 3)
        {
            for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; i++)
            {
                Toggle trToggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

                if (!trToggleAttr.isOn)
                {
                    trToggleAttr.interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; i++)
            {
                Toggle trToggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

                if (!trToggleAttr.interactable)
                {
                    trToggleAttr.interactable = true;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    int GetAttrLockCount(Transform trToggleList)
    {
        int nCount = 0;

        for (int i = 0; i < c_nMaxAttrCount; i++)
        {
            Toggle toggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

            if (toggleAttr.isOn)
            {
                nCount++;
            }
        }

        return nCount;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroMainGearChangeAttr(CsHeroMainGear csHeroMainGear)
    {
        // 초기 / 저장 이후
        if (csHeroMainGear.HeroMainGearRefinementList.Count == 0)
        {
            m_trChangeAttr.gameObject.SetActive(false);
            m_trMainGearRefineInfo.gameObject.SetActive(true);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSave, false);
        }
        // 마법 부여 1개 이상
        else
        {
            m_trChangeAttr.gameObject.SetActive(true);
            m_trMainGearRefineInfo.gameObject.SetActive(false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSave, true);

            UpdateHeroMainGearChangeAttrList(m_trChangeAttr, csHeroMainGear, 0);
            m_nTurn = csHeroMainGear.HeroMainGearRefinementList[0].Turn;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroMainGearChangeAttrList(Transform trChangeAttr, CsHeroMainGear csHeroMainGear, int nTurn)
    {
        // 변하는 속성
        Transform trChangeAttrList = trChangeAttr.Find("ChangeAttrList");

        // 속성 초기화
        for (int i = 0; i < c_nMaxAttrCount; i++)
        {
            Transform trAttr = trChangeAttrList.Find("OptionAttribute" + i);
            trAttr.gameObject.SetActive(false);
        }

        // 변하는 전투력
        List<CsHeroMainGearRefinementAttr> listHeroMainGearRefinementAttr = csHeroMainGear.HeroMainGearRefinementList[nTurn].HeroMainGearRefinementAttrList;
        string strColorCode = "";

        for (int i = 0; i < listHeroMainGearRefinementAttr.Count; i++)
        {
            Transform trAttr = trChangeAttrList.Find("OptionAttribute" + i);
            trAttr.gameObject.SetActive(true);

            strColorCode = listHeroMainGearRefinementAttr[i].Grade.ColorCode;

            Text textAttrName = trAttr.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = string.Format("<color={0}>{1}</color>", strColorCode, listHeroMainGearRefinementAttr[i].Attr.Name);

            Text textAttrValue = trAttr.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);
            textAttrValue.text = string.Format("<color={0}>{1}</color>", strColorCode, listHeroMainGearRefinementAttr[i].Value);
        }

        // 변화되는 전투력 표시
        Image imageArrow = trChangeAttr.Find("ImageArrow").GetComponent<Image>();
        imageArrow.gameObject.SetActive(true);

        if (csHeroMainGear.HeroMainGearRefinementList[nTurn].OptionAttributesBattlePower > csHeroMainGear.OptionAttributesBattlePower)
        {
            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_up");
        }
        else if (csHeroMainGear.HeroMainGearRefinementList[nTurn].OptionAttributesBattlePower < csHeroMainGear.OptionAttributesBattlePower)
        {
            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_down");
        }
        else
        {
            imageArrow.gameObject.SetActive(false);
        }

        Text textBattlePowerValue = trChangeAttr.Find("TextBattlePowerValue").GetComponent<Text>();
        textBattlePowerValue.text = csHeroMainGear.HeroMainGearRefinementList[nTurn].OptionAttributesBattlePower.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupMultiRefineOptionAttr(CsHeroMainGear csHeroMainGear)
    {
        if (m_trPopupMultiRefine.parent.gameObject.activeSelf)
        {
            UpdateHeroMainGearOptionAttr(m_trPopupMultiRefine, csHeroMainGear);
        }
        else
        {
            UpdateHeroMainGearOptionAttr(m_trPopupMultiRefine, csHeroMainGear);

            //토글 초기화
            Transform trToggleList = m_trPopupMultiRefine.Find("ToggleList");

            for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; i++)
            {
                Toggle toggleAttr = trToggleList.Find("Toggle" + i).GetComponent<Toggle>();

                if (toggleAttr.isOn)
                {
                    toggleAttr.isOn = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupMultiRefineChangeAttrs(CsHeroMainGear csHeroMainGear)
    {
        if (m_trPopupMultiRefine.parent.gameObject.activeSelf)
        {
            Transform trChangeInheritList = m_trPopupMultiRefine.Find("ChangeInheritList");

            if (csHeroMainGear.HeroMainGearRefinementList.Count == 3)
            {
                trChangeInheritList.GetComponent<ToggleGroup>().allowSwitchOff = false;
            }
            else
            {
                trChangeInheritList.GetComponent<ToggleGroup>().allowSwitchOff = true;
                CsUIData.Instance.DisplayButtonInteractable(m_buttonPopupSave, false);
            }

            for (int i = 0; i < trChangeInheritList.childCount; i++)
            {
                Transform trChangeInherit = trChangeInheritList.Find("ToggleInheritAttr" + i);
                Transform trTextNoAttr = trChangeInherit.Find("TextNoAttr");
                Transform trChangeAttr = trChangeInherit.Find("ChangeAttr");

                Toggle toggleChangeInherit = trChangeInherit.GetComponent<Toggle>();

                if (csHeroMainGear.HeroMainGearRefinementList.Count == 3)
                {
                    trChangeAttr.gameObject.SetActive(true);
                    trTextNoAttr.gameObject.SetActive(false);

                    toggleChangeInherit.interactable = true;

                    UpdateHeroMainGearChangeAttrList(trChangeAttr, csHeroMainGear, i);
                }
                else
                {
                    trChangeAttr.gameObject.SetActive(false);
                    trTextNoAttr.gameObject.SetActive(true);

                    toggleChangeInherit.interactable = false;

                    if (toggleChangeInherit.isOn)
                    {
                        toggleChangeInherit.isOn = false;
                    }
                }
            }
        }
        else
        {
            Transform trChangeInheritList = m_trPopupMultiRefine.Find("ChangeInheritList");
            trChangeInheritList.GetComponent<ToggleGroup>().allowSwitchOff = true;

            for (int i = 0; i < trChangeInheritList.childCount; i++)
            {
                Transform trChangeInherit = trChangeInheritList.Find("ToggleInheritAttr" + i);
                Transform trTextNoAttr = trChangeInherit.Find("TextNoAttr");
                Transform trChangeAttr = trChangeInherit.Find("ChangeAttr");

                Toggle toggleChangeInherit = trChangeInheritList.Find("ToggleInheritAttr" + i).GetComponent<Toggle>();

                if (csHeroMainGear.HeroMainGearRefinementList.Count == 3)
                {
                    trChangeAttr.gameObject.SetActive(true);
                    trTextNoAttr.gameObject.SetActive(false);

                    toggleChangeInherit.interactable = true;

                    UpdateHeroMainGearChangeAttrList(trChangeAttr, csHeroMainGear, i);
                }
                else
                {
                    trChangeAttr.gameObject.SetActive(false);
                    trTextNoAttr.gameObject.SetActive(true);

                    toggleChangeInherit.interactable = false;

                    if (toggleChangeInherit.isOn)
                    {
                        toggleChangeInherit.isOn = false;
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonPopupSave, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupMultiRefineMaterial()
    {
        CsItem csItemRefinementItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.MainGearRefinementItemId);
        int nRefinementItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemRefinementItem.ItemId);

        string strItemName = string.Format(CsConfiguration.Instance.GetString("INPUT_RECIPE_ITEM"), csItemRefinementItem.Name);
        string strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRefinementItemCount, 3);

        Text textRefinementItem = m_trPopupMultiRefine.Find("ImageFrameMaterial/TextInheritStone").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRefinementItem);
        textRefinementItem.text = strItemName;

        Text textRefinementItemCount = textRefinementItem.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRefinementItemCount);
        textRefinementItemCount.text = strItemCount;

        Image imageRefinementItem = textRefinementItem.transform.Find("Image").GetComponent<Image>();
        imageRefinementItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_12301");

        if (3 <= nRefinementItemCount)
        {
            textRefinementItemCount.color = CsUIData.Instance.ColorWhite;

            m_bMultiRefine = true;
        }
        else
        {
            textRefinementItemCount.color = CsUIData.Instance.ColorRed;

            m_bMultiRefine = false;
        }

        Text textProtectionItem = m_trPopupMultiRefine.Find("ImageFrameMaterial/TextLockScroll").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProtectionItem);

        Text textProtectionItemCount = textProtectionItem.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProtectionItemCount);

        Transform trToggleList = m_trPopupMultiRefine.Find("ToggleList");
        int nLockCount = GetAttrLockCount(trToggleList);

        CsItem csItemProtectionItem = null;
        int nProtectionItemCount = 0;

        Image imageProtectionItem = textProtectionItem.transform.Find("Image").GetComponent<Image>();

        if (nLockCount == 0)
        {
            csItemProtectionItem = CsGameData.Instance.GetMainGearRefinementRecipe(nLockCount + 1).ProtectionItem;
            nProtectionItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemProtectionItem.ItemId);

            imageProtectionItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_12401");

            strItemName = string.Format(CsConfiguration.Instance.GetString("INPUT_RECIPE_ITEM"), csItemProtectionItem.Name);
            strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nProtectionItemCount, 3);

            textProtectionItem.text = strItemName;
            textProtectionItem.color = CsUIData.Instance.ColorGray;

            textProtectionItemCount.text = strItemCount;
            textProtectionItemCount.color = CsUIData.Instance.ColorGray;
        }
        else
        {
            csItemProtectionItem = CsGameData.Instance.GetMainGearRefinementRecipe(nLockCount).ProtectionItem;
            nProtectionItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemProtectionItem.ItemId);

            imageProtectionItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_1240" + nLockCount);

            strItemName = string.Format(CsConfiguration.Instance.GetString("INPUT_RECIPE_ITEM"), csItemProtectionItem.Name);

            textProtectionItem.text = strItemName;
            textProtectionItem.color = CsUIData.Instance.ColorWhite;

            strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nProtectionItemCount, 3);
            textProtectionItemCount.text = strItemCount;

            if (3 <= nProtectionItemCount)
            {
                textProtectionItemCount.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textProtectionItemCount.color = CsUIData.Instance.ColorRed;
                m_bMultiRefine = false;
            }
        }

        int nRemainingRefinementCount = CsGameData.Instance.MyHeroInfo.VipLevel.MainGearRefinementMaxCount - CsGameData.Instance.MyHeroInfo.MainGearRefineDailyCount;

        if (nRemainingRefinementCount <= 2)
        {
            m_bRefineDaily = false;
        }
        else
        {
            m_bRefineDaily = true;
        }

        m_bMultiRefine = m_bMultiRefine && m_bRefineDaily;

        Button buttonMultiRefine = m_trPopupMultiRefine.Find("ButtonMultiInherit").GetComponent<Button>();
        CsUIData.Instance.DisplayButtonInteractable(buttonMultiRefine, m_bMultiRefine);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextDailyCount(Transform trPopupRefine)
    {
        Text textDayCount = trPopupRefine.Find("TextDayCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDayCount);

        int nRemainingRefinementCount = CsGameData.Instance.MyHeroInfo.VipLevel.MainGearRefinementMaxCount - CsGameData.Instance.MyHeroInfo.MainGearRefineDailyCount;
        textDayCount.text = string.Format(CsConfiguration.Instance.GetString("A07_TXT_01001"), nRemainingRefinementCount, CsGameData.Instance.MyHeroInfo.VipLevel.MainGearRefinementMaxCount);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csItem)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csItem);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csItem)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);

        m_trPopupItemInfo = goPopupItemInfo.transform;

        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId), false, -1, false);
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