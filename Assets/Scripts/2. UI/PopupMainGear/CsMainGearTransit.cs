using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-26)
//---------------------------------------------------------------------------------------------------

public class CsMainGearTransit : CsPopupSub
{
    const int c_nMaxAttrCount = 5;

    [SerializeField] GameObject m_goTransitMainGear;

    Transform m_trSlotMainGear;
    Transform m_trSlotMaterial;
    Transform m_trMainGearImageLevel;
    Transform m_trMainGearImageAttr;
    Transform m_trMaterialImageLevel;
    Transform m_trMaterialImageAttr;
    Transform m_trPopupSelectTransitMainGear;
    Transform m_trPopupContent;

    Toggle m_toggleEnchantLevel;
    Toggle m_toggleOptionAttr;

    Button m_buttonTransition;

    bool m_bEnchantTransit = false;
    bool m_bAttrTransit = false;
    bool m_bFirst = true;
    bool m_bFirstPopupOpen = true;

    Guid m_guidPrevMainGearId;
    CsHeroMainGear m_csHeroMainGear;
    CsHeroMainGear m_csHeroMainGearMaterial;

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
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.MainGearTransit)
        {
            if (guidHeroMainGearId == Guid.Empty)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                // 다른 메인 장비를 선택했을 때
                if (m_guidPrevMainGearId != guidHeroMainGearId)
                {
                    m_guidPrevMainGearId = guidHeroMainGearId;
                    m_csHeroMainGearMaterial = null;

                    m_toggleEnchantLevel.isOn = false;
                    m_toggleOptionAttr.isOn = false;
                }

                m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroMainGearId);
                DisplayUpdate(m_csHeroMainGear);
                transform.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedEnchantLevel(bool bIson)
    {
        if (bIson)
        {
            UpdateToggleEnchantLevel();
        }
        else
        {
            m_bEnchantTransit = false;
        }

        Transform trMainGearImageCheck = m_trMainGearImageLevel.Find("ImageCheck");
        trMainGearImageCheck.gameObject.SetActive(m_bEnchantTransit);

        Transform trMaterialImageCheck = m_trMaterialImageLevel.Find("ImageCheck");
        trMaterialImageCheck.gameObject.SetActive(m_bEnchantTransit);

        UpdateButtonTransition();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedOptionAttr(bool bIson)
    {
        if (bIson)
        {
            UpdateToggleOptionAttr();
        }
        else
        {
            m_bAttrTransit = false;
        }

        Transform trMainGearImageCheck = m_trMainGearImageAttr.Find("ImageCheck");
        trMainGearImageCheck.gameObject.SetActive(m_bAttrTransit);

        Transform trMaterialImageCheck = m_trMaterialImageAttr.Find("ImageCheck");
        trMaterialImageCheck.gameObject.SetActive(m_bAttrTransit);

        UpdateButtonTransition();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenTargetMainGear()
    {
        if (m_bFirstPopupOpen)
        {
            InitializePopupUI();

            m_bFirstPopupOpen = false;
        }

        m_trPopupContent.localPosition = Vector3.zero;

        List<CsHeroMainGear> listHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearList;

        bool bPopupOpen = false;

        for (int i = 0; i < listHeroMainGear.Count; i++)
        {
            Transform trTransitMainGear = m_trPopupContent.Find(listHeroMainGear[i].Id.ToString());

            if (trTransitMainGear == null)
            {
                trTransitMainGear = CreateTransitMainGear(listHeroMainGear[i]);
            }

            // 같은 장비 지움
            if (m_csHeroMainGear.Id == listHeroMainGear[i].Id)
            {
                trTransitMainGear.gameObject.SetActive(false);
            }
            else
            {
                // 카테고리(1. 무기 / 2. 갑옷)가 같은 아이템 / 강화 레벨이 다르거나 추가 속성 수가 같을 때 표시
                if (m_csHeroMainGear.MainGear.MainGearType.MainGearCategory.CategoryId == listHeroMainGear[i].MainGear.MainGearType.MainGearCategory.CategoryId
                    && (m_csHeroMainGear.EnchantLevel != listHeroMainGear[i].EnchantLevel || m_csHeroMainGear.OptionAttrList.Count == listHeroMainGear[i].OptionAttrList.Count))
                {
                    trTransitMainGear.gameObject.SetActive(true);

                    UpdatePopupSelectTransitMainGear(listHeroMainGear[i], trTransitMainGear);

                    bPopupOpen = true;
                }
                else
                {
                    trTransitMainGear.gameObject.SetActive(false);
                }
            }
        }

        if (bPopupOpen)
        {
            m_trPopupSelectTransitMainGear.parent.gameObject.SetActive(true);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A08_TXT_02005"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSelectTargetMainGear(CsHeroMainGear csHeroMainGearMaterial)
    {
        // 다른 장비를 선택한다면 토글 초기화
        if (m_csHeroMainGearMaterial != csHeroMainGearMaterial)
        {
            m_toggleEnchantLevel.isOn = false;
            m_toggleOptionAttr.isOn = false;
        }

        m_csHeroMainGearMaterial = csHeroMainGearMaterial;

        UpdateSlotMaterialGear();
        UpdateMaterialGearImageLevel();
        UpdateMaterialGearImageAttr();

        UpdateMainGearOptionBattlePower(m_csHeroMainGear);

        m_trPopupSelectTransitMainGear.parent.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseTargetMainGear()
    {
        m_trPopupSelectTransitMainGear.parent.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMainGearTransit()
    {
        CsCommandEventManager.Instance.SendMainGearTransit(m_csHeroMainGear.Id, m_csHeroMainGearMaterial.Id, m_bEnchantTransit, m_bAttrTransit);
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    public void InitializeUI()
    {
        Transform trItemGrid = transform.Find("ItemGrid");
        m_trSlotMainGear = trItemGrid.Find("ItemSlotMain");
        m_trSlotMaterial = trItemGrid.Find("ItemSlotMaterial");

        Button buttonSlotMaterial = m_trSlotMaterial.GetComponent<Button>();
        buttonSlotMaterial.onClick.RemoveAllListeners();
        buttonSlotMaterial.onClick.AddListener(() => OnClickOpenTargetMainGear());
        buttonSlotMaterial.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trItemSlot = m_trSlotMaterial.Find("ItemSlot");

        buttonSlotMaterial = trItemSlot.GetComponent<Button>();
        buttonSlotMaterial.onClick.RemoveAllListeners();
        buttonSlotMaterial.onClick.AddListener(() => OnClickOpenTargetMainGear());
        buttonSlotMaterial.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trMainGear = transform.Find("MainGear");
        m_trMainGearImageLevel = trMainGear.Find("ImageLevel");
        m_trMainGearImageAttr = trMainGear.Find("ImageAttr");

        Transform trMaterialGear = transform.Find("MaterialGear");
        m_trMaterialImageLevel = trMaterialGear.Find("ImageLevel");
        m_trMaterialImageAttr = trMaterialGear.Find("ImageAttr");

        // 팝업창
        m_trPopupSelectTransitMainGear = transform.Find("PopupSelectTransitMainGear/ImageBackground");
        m_trPopupSelectTransitMainGear.parent.gameObject.SetActive(false);

        // 강화 토글
        m_toggleEnchantLevel = transform.Find("ToggleEnchantLevel").GetComponent<Toggle>();
        m_toggleEnchantLevel.isOn = false;
        m_toggleEnchantLevel.onValueChanged.RemoveAllListeners();
        m_toggleEnchantLevel.onValueChanged.AddListener((ison) => OnValueChangedEnchantLevel(ison));
        m_toggleEnchantLevel.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle)); ;

        // 추가 속성 토글
        m_toggleOptionAttr = transform.Find("ToggleAttr").GetComponent<Toggle>();
        m_toggleOptionAttr.isOn = false;
        m_toggleOptionAttr.onValueChanged.RemoveAllListeners();
        m_toggleOptionAttr.onValueChanged.AddListener((ison) => OnValueChangedOptionAttr(ison));
        m_toggleOptionAttr.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        // 도움말 텍스트 초기화
        Text textHelp = transform.Find("TextHelp").GetComponent<Text>();
        CsUIData.Instance.SetFont(textHelp);
        textHelp.text = CsConfiguration.Instance.GetString("A08_TXT_00004");

        // 전이 버튼 초기화
        m_buttonTransition = transform.Find("ButtonTransition").GetComponent<Button>();
        m_buttonTransition.onClick.RemoveAllListeners();
        m_buttonTransition.onClick.AddListener(() => OnClickMainGearTransit());
        m_buttonTransition.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonTransition = m_buttonTransition.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonTransition);
        textButtonTransition.text = CsConfiguration.Instance.GetString("A08_BTN_00001");

        FirstMainGearCheck();
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
                {
                    continue;
                }

                CsUIData.Instance.MainGearId = listHeroMainGearEquip[i].Id;
                break;
            }
        }

        m_guidPrevMainGearId = CsUIData.Instance.MainGearId;
        m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(CsUIData.Instance.MainGearId);

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
    void InitializePopupUI()
    {
        Text textPopupName = m_trPopupSelectTransitMainGear.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A08_NAME_00001");

        Button buttonClose = m_trPopupSelectTransitMainGear.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickCloseTargetMainGear());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupContent = m_trPopupSelectTransitMainGear.Find("Scroll View/Viewport/Content");
    }

    //---------------------------------------------------------------------------------------------------
    Transform CreateTransitMainGear(CsHeroMainGear csHeroMainGearTransit)
    {
        Transform trTransitMainGear = Instantiate(m_goTransitMainGear, m_trPopupContent).transform;
        trTransitMainGear.name = csHeroMainGearTransit.Id.ToString();

        Text textGearName = trTransitMainGear.Find("TextGearName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGearName);
        textGearName.text = csHeroMainGearTransit.MainGear.Name;

        Transform trItemSlot = trTransitMainGear.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGearTransit, EnItemSlotSize.Medium);

        Transform trEnchantLevel = trTransitMainGear.Find("EnchantLevel");
        CsUIData.Instance.UpdateEnchantLevelIcon(trEnchantLevel, csHeroMainGearTransit.EnchantLevel);

        Button buttonSelectGear = trTransitMainGear.Find("ButtonSelectGear").GetComponent<Button>();
        buttonSelectGear.onClick.RemoveAllListeners();
        buttonSelectGear.onClick.AddListener(() => OnClickSelectTargetMainGear(csHeroMainGearTransit));
        buttonSelectGear.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonSelectGear = buttonSelectGear.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonSelectGear);
        textButtonSelectGear.text = CsConfiguration.Instance.GetString("A08_BTN_00002");

        Text textDiscordAttr = trTransitMainGear.Find("TextDiscordAttr").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDiscordAttr);
        textDiscordAttr.text = CsConfiguration.Instance.GetString("A08_TXT_00005");

        Transform trOptionBattlePower = trTransitMainGear.Find("OptionBattlePower");

        Text textBattlePower = trOptionBattlePower.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePower);
        textBattlePower.text = CsConfiguration.Instance.GetString("A08_TXT_00002");

        Text textBattlePowerValue = trOptionBattlePower.Find("TextBattlePowerValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePowerValue);

        return trTransitMainGear;
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayUpdate(CsHeroMainGear csHeroMainGear)
    {
        UpdateSlotMainGear(csHeroMainGear);
        UpdateMainGearImageLevel(csHeroMainGear);
        UpdateMainGearImageAttr(csHeroMainGear);
        UpdateMainGearOptionBattlePower(csHeroMainGear);

        UpdateSlotMaterialGear();
        UpdateMaterialGearImageLevel();
        UpdateMaterialGearImageAttr();

        UpdateButtonTransition();
    }

    // 메인 장비 슬롯
    //---------------------------------------------------------------------------------------------------
    void UpdateSlotMainGear(CsHeroMainGear csHeroMainGear)
    {
        // 메인 장비 슬롯 업데이트
        CsUIData.Instance.DisplayItemSlot(m_trSlotMainGear, csHeroMainGear, EnItemSlotSize.Large);

        Text textMainGearName = m_trSlotMainGear.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMainGearName);
        textMainGearName.text = csHeroMainGear.MainGear.Name;
    }

    // 메인 장비 강화 정도
    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearImageLevel(CsHeroMainGear csHeroMainGear)
    {
        Transform trEnchantLevel = m_trMainGearImageLevel.Find("EnchantLevel");
        CsUIData.Instance.UpdateEnchantLevelIcon(trEnchantLevel, csHeroMainGear.EnchantLevel);
    }

    // 메인 장비 추가 속성
    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearImageAttr(CsHeroMainGear csHeroMainGear)
    {
        Transform trAttr = m_trMainGearImageAttr.Find("Attr");
        Transform trAttrList = trAttr.Find("AttrList");

        // 추가 속성 초기화
        for (int i = 0; i < c_nMaxAttrCount; i++)
        {
            Transform trOptionAttribute = trAttrList.Find("OptionAttribute" + i);
            trOptionAttribute.gameObject.SetActive(false);
        }

        for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; i++)
        {
            Transform trOptionAttribute = trAttrList.Find("OptionAttribute" + i);
            trOptionAttribute.gameObject.SetActive(true);

            Text textAttrName = trOptionAttribute.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = string.Format("<color={0}>{1}</color>", csHeroMainGear.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode, csHeroMainGear.OptionAttrList[i].Attr.Name);

            Text textAttrValue = trOptionAttribute.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);
            textAttrValue.text = string.Format("<color={0}>{1}</color>", csHeroMainGear.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode, csHeroMainGear.OptionAttrList[i].Value);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearOptionBattlePower(CsHeroMainGear csHeroMainGear)
    {
        Transform trAttr = m_trMainGearImageAttr.Find("Attr");

        // 전투력 텍스트 초기화
        Text textBattlePower = trAttr.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePower);
        textBattlePower.text = CsConfiguration.Instance.GetString("A08_TXT_00002");

        // 추가 옵션 전투력
        Text textBattlePowerValue = trAttr.Find("TextBattlePowerValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePowerValue);

        Image imageArrow = trAttr.Find("ImageArrow").GetComponent<Image>();

        if (m_csHeroMainGearMaterial == null)
        {
            textBattlePower.gameObject.SetActive(false);
            textBattlePowerValue.gameObject.SetActive(false);
            imageArrow.gameObject.SetActive(false);
        }
        else
        {
            if (csHeroMainGear.OptionAttrList.Count == m_csHeroMainGearMaterial.OptionAttrList.Count)
            {
                textBattlePowerValue.text = Mathf.Abs(m_csHeroMainGearMaterial.OptionAttributesBattlePower - csHeroMainGear.OptionAttributesBattlePower).ToString("#,##0");
                imageArrow.gameObject.SetActive(true);

                if (csHeroMainGear.OptionAttributesBattlePower > m_csHeroMainGearMaterial.OptionAttributesBattlePower)
                {
                    imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_up");
                }
                else if (csHeroMainGear.OptionAttributesBattlePower < m_csHeroMainGearMaterial.OptionAttributesBattlePower)
                {
                    imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_down");
                }
                else
                {
                    imageArrow.gameObject.SetActive(false);
                }

                textBattlePower.gameObject.SetActive(true);
                textBattlePowerValue.gameObject.SetActive(true);
            }
            else
            {
                textBattlePower.gameObject.SetActive(false);
                textBattlePowerValue.gameObject.SetActive(false);
                imageArrow.gameObject.SetActive(false);
            }
        }
    }

    // 강화 토글
    //---------------------------------------------------------------------------------------------------
    void UpdateToggleEnchantLevel()
    {
        if (m_csHeroMainGearMaterial == null)
        {
            m_bEnchantTransit = false;

            m_toggleEnchantLevel.onValueChanged.RemoveAllListeners();
            m_toggleEnchantLevel.isOn = false;
            m_toggleEnchantLevel.onValueChanged.AddListener((ison) => OnValueChangedEnchantLevel(ison));
            m_toggleEnchantLevel.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A08_TXT_02004"));
        }
        else
        {
            CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(CsUIData.Instance.MainGearId);

            if (m_csHeroMainGearMaterial.EnchantLevel == csHeroMainGear.EnchantLevel)
            {
                m_bEnchantTransit = false;

                m_toggleEnchantLevel.onValueChanged.RemoveAllListeners();
                m_toggleEnchantLevel.isOn = false;
                m_toggleEnchantLevel.onValueChanged.AddListener((ison) => OnValueChangedEnchantLevel(ison));
                m_toggleEnchantLevel.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A08_TXT_02002"));
            }
            else
            {
                m_bEnchantTransit = true;
            }
        }
    }

    // 추가 속성 토글
    //---------------------------------------------------------------------------------------------------
    void UpdateToggleOptionAttr()
    {
        if (m_csHeroMainGearMaterial == null)
        {
            m_bAttrTransit = false;

            m_toggleOptionAttr.onValueChanged.RemoveAllListeners();
            m_toggleOptionAttr.isOn = false;
            m_toggleOptionAttr.onValueChanged.AddListener((ison) => OnValueChangedOptionAttr(ison));
            m_toggleOptionAttr.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A08_TXT_02004"));
        }
        else
        {
            CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(CsUIData.Instance.MainGearId);

            if (m_csHeroMainGearMaterial.OptionAttrList.Count == csHeroMainGear.OptionAttrList.Count)
            {
                m_bAttrTransit = true;
            }
            else
            {
                m_bAttrTransit = false;

                m_toggleOptionAttr.onValueChanged.RemoveAllListeners();
                m_toggleOptionAttr.isOn = false;
                m_toggleOptionAttr.onValueChanged.AddListener((ison) => OnValueChangedOptionAttr(ison));
                m_toggleOptionAttr.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A08_TXT_02003"));
            }
        }
    }

    // 부 장비 슬롯
    //---------------------------------------------------------------------------------------------------
    void UpdateSlotMaterialGear()
    {
        Transform trImage = m_trSlotMaterial.Find("Image");

        Text textName = m_trSlotMaterial.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        Transform trItemSlot = m_trSlotMaterial.Find("ItemSlot");

        if (m_csHeroMainGearMaterial == null)
        {
            trImage.gameObject.SetActive(true);
            trItemSlot.gameObject.SetActive(false);
            textName.color = CsUIData.Instance.ColorGray;
            textName.text = CsConfiguration.Instance.GetString("A08_TXT_00001");
        }
        else
        {
            trImage.gameObject.SetActive(false);
            trItemSlot.gameObject.SetActive(true);
            CsUIData.Instance.DisplayItemSlot(trItemSlot, m_csHeroMainGearMaterial, EnItemSlotSize.Medium);
            textName.color = CsUIData.Instance.ColorWhite;
            textName.text = m_csHeroMainGearMaterial.MainGear.Name;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMaterialGearImageLevel()
    {
        Transform trEnchantLevel = m_trMaterialImageLevel.Find("EnchantLevel");

        if (m_csHeroMainGearMaterial == null)
        {
            trEnchantLevel.gameObject.SetActive(false);
        }
        else
        {
            CsUIData.Instance.UpdateEnchantLevelIcon(trEnchantLevel, m_csHeroMainGearMaterial.EnchantLevel);
            trEnchantLevel.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMaterialGearImageAttr()
    {
        Transform trAttr = m_trMaterialImageAttr.Find("Attr");

        Text textNoMaterial = m_trMaterialImageAttr.Find("TextNoMaterial").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoMaterial);
        textNoMaterial.text = CsConfiguration.Instance.GetString("A08_TXT_00003");

        if (m_csHeroMainGearMaterial == null)
        {
            trAttr.gameObject.SetActive(false);
            textNoMaterial.gameObject.SetActive(true);
        }
        else
        {
            Transform trAttrList = trAttr.Find("AttrList");

            // 추가 속성 초기화
            for (int i = 0; i < c_nMaxAttrCount; i++)
            {
                Transform trOptionAttribute = trAttrList.Find("OptionAttribute" + i);
                trOptionAttribute.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_csHeroMainGearMaterial.OptionAttrList.Count; i++)
            {
                Transform trOptionAttribute = trAttrList.Find("OptionAttribute" + i);
                trOptionAttribute.gameObject.SetActive(true);

                Text textAttrName = trOptionAttribute.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrName);
                textAttrName.text = string.Format("<color={0}>{1}</color>", m_csHeroMainGearMaterial.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode, m_csHeroMainGearMaterial.OptionAttrList[i].Attr.Name);

                Text textAttrValue = trOptionAttribute.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);
                textAttrValue.text = string.Format("<color={0}>{1}</color>", m_csHeroMainGearMaterial.OptionAttrList[i].MainGearOptionAttrGrade.ColorCode, m_csHeroMainGearMaterial.OptionAttrList[i].Value);
            }

            trAttr.gameObject.SetActive(true);
            textNoMaterial.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonTransition()
    {
        if (m_bEnchantTransit || m_bAttrTransit)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonTransition, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonTransition, false);
        }
    }

    // 부 장비 선택창 업데이트
    //---------------------------------------------------------------------------------------------------
    void UpdatePopupSelectTransitMainGear(CsHeroMainGear csHeroMainGearTransit, Transform trTransitMainGear)
    {
        // 아이템 슬롯 변경
        Transform trItemSlot = trTransitMainGear.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGearTransit, EnItemSlotSize.Medium);

        // 아이템 강화 레벨 변경
        Transform trEnchantLevel = trTransitMainGear.Find("EnchantLevel");
        CsUIData.Instance.UpdateEnchantLevelIcon(trEnchantLevel, csHeroMainGearTransit.EnchantLevel);

        Transform trDiscordAttr = trTransitMainGear.Find("TextDiscordAttr");
        Transform trOptionBattlePower = trTransitMainGear.Find("OptionBattlePower");

        // 추가 속성 개수 일치
        if (m_csHeroMainGear.OptionAttrList.Count == csHeroMainGearTransit.OptionAttrList.Count)
        {
            trOptionBattlePower.gameObject.SetActive(true);
            trDiscordAttr.gameObject.SetActive(false);

            Image imageArrow = trOptionBattlePower.Find("Image").GetComponent<Image>();
            imageArrow.gameObject.SetActive(true);

            Text textBattlePowerValue = trOptionBattlePower.Find("TextBattlePowerValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePowerValue);
            textBattlePowerValue.text = Mathf.Abs(csHeroMainGearTransit.OptionAttributesBattlePower - m_csHeroMainGear.OptionAttributesBattlePower).ToString("#,##0");

            if (m_csHeroMainGear.OptionAttributesBattlePower > csHeroMainGearTransit.OptionAttributesBattlePower)
            {
                imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_up");
            }
            else if (m_csHeroMainGear.OptionAttributesBattlePower < csHeroMainGearTransit.OptionAttributesBattlePower)
            {
                imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_down");
            }
            else
            {
                imageArrow.gameObject.SetActive(false);
            }
        }
        // 추가 속성 개수 불일치
        else
        {
            trOptionBattlePower.gameObject.SetActive(false);
            trDiscordAttr.gameObject.SetActive(true);
        }
    }
}