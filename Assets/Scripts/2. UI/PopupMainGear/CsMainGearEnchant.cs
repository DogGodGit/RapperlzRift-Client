using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsMainGearEnchant : CsPopupSub
{
    GameObject m_goPopupSetInfo;
    GameObject m_goPopupItemInfo;

    Transform m_trPopupList;
    Transform m_trSlotMainGear;
    Transform m_trPlus;
    Transform m_trSlotEnchantMaterial;
    Transform m_trSlotProtect;
    Transform m_trEnchatList;
    Transform[] m_atrAttr = new Transform[3];
    Transform m_trProtectDesc;
    Transform m_trPopupProtect;
    Transform m_trPopupItemInfo;
    Transform m_trPopupSetInfo;
    Transform m_trParticleMainGearEnchantResult;

    Toggle m_toggleProtect;

    Button m_buttonEnchant;
    Button m_buttonShop;
    Button m_buttonSetEquipment;

    Text m_textProtect;
    Text m_textEnchantDailyCount;
    Text m_textSuccessivity;
    Text m_textMaxLevel;

    bool m_bFirst = true;
    bool m_bOpenPopupFirst = true;
    bool m_bEnchant = false;
    bool m_bProtectUse = false;
    bool m_bSavePopupProtect = false;

    Guid m_guidPrevMainGearId;
    CsHeroMainGear m_csHeroMainGear;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMainGearSelected += OnEventMainGearSelected;
        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate += OnEventMainGearEnchantLevelSetActivate;
        CsGameEventUIToUI.Instance.EventMainGearEnchant += OnEventMainGearEnchant;
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
        CsGameEventUIToUI.Instance.EventMainGearSelected -= OnEventMainGearSelected;
        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate -= OnEventMainGearEnchantLevelSetActivate;
        CsGameEventUIToUI.Instance.EventMainGearEnchant -= OnEventMainGearEnchant;
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
                if (m_trParticleMainGearEnchantResult.gameObject.activeSelf)
                {
                    m_trParticleMainGearEnchantResult.gameObject.SetActive(false);
                }

                DisplayUpdate(m_csHeroMainGear);
            }
        }
    }

    void OnDisable()
    {
        if (m_trPopupSetInfo)
        {
            OnEventClosePopupSetInfo();
        }
        if (m_trPopupItemInfo)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Center);
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearSelected(Guid guidHeroMainGearId)
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.MainGearEnchant)
        {
            if (guidHeroMainGearId == Guid.Empty)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                // 다른 장비 선택
                if (m_guidPrevMainGearId != guidHeroMainGearId)
                {
                    m_guidPrevMainGearId = guidHeroMainGearId;
                    m_toggleProtect.isOn = false;
                }

                m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroMainGearId);
                DisplayUpdate(m_csHeroMainGear);

                transform.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEnchantLevelSetActivate()
    {
        UpdateMainGearEnchantLevelSet(CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEnchant(bool bSuccess, Guid guidMainGearId)
    {
        if (bSuccess)
        {
            if (m_trParticleMainGearEnchantResult.gameObject.activeSelf)
            {
                m_trParticleMainGearEnchantResult.gameObject.SetActive(false);
            }

            m_trParticleMainGearEnchantResult.gameObject.SetActive(true);
        }
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickMainGearEnchant()
    {
        CsItem csItemProtect = m_csHeroMainGear.MainGearEnchantLevel.MainGearEnchantStep.NextEnchantPenaltyPreventItem;
        int nProtectCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemProtect.ItemId);

        if (m_trPopupProtect.parent.gameObject.activeSelf)
        {
            CsCommandEventManager.Instance.SendMainGearEnchant(m_csHeroMainGear.Id, m_bProtectUse);

            // 키가 없을 때
            if (m_bSavePopupProtect)
            {
                PlayerPrefs.SetString(CsGameData.Instance.MyHeroInfo.HeroId.ToString(), "SavePopupProtect");
            }

            m_trPopupProtect.parent.gameObject.SetActive(false);
        }
        else
        {
            // 행운석 사용 가능 여부 / 행운석 사용 여부 / 행운석 재료량 / 키 값이 없을 때
            if (m_csHeroMainGear.MainGearEnchantLevel.PenaltyPreventEnabled && !m_bProtectUse
                && nProtectCount < 1 && !PlayerPrefs.HasKey(CsGameData.Instance.MyHeroInfo.HeroId.ToString()))
            {
                if (m_bOpenPopupFirst)
                {
                    InitiallizePopupProtect();
                    m_bOpenPopupFirst = false;
                }

                Text textBuyProtect = m_trPopupProtect.Find("TextBuyProtect").GetComponent<Text>();
                CsUIData.Instance.SetFont(textBuyProtect);
                textBuyProtect.text = string.Format(CsConfiguration.Instance.GetString("A06_TXT_01003"), 30, csItemProtect.Name);

                m_trPopupProtect.parent.gameObject.SetActive(true);
            }
            else
            {
                CsCommandEventManager.Instance.SendMainGearEnchant(m_csHeroMainGear.Id, m_bProtectUse);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickShop()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSetEquipment()
    {
        if (m_goPopupSetInfo == null)
        {
            StartCoroutine(LoadPopupSetInfo());
        }
        else
        {
            OpenPopupSetInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedProtectUse(bool bIson)
    {
        m_bProtectUse = bIson;
        // 행운석 사용 여부 색 변화

        if (bIson)
        {
            m_textProtect.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            m_textProtect.color = CsUIData.Instance.ColorGray;
        }

        UpdateSlotEnchantMaterial(m_csHeroMainGear);
        UpdateSlotProtect(m_csHeroMainGear);
        UpdateButton(m_csHeroMainGear);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemInfo(CsItem csItem)
    {
        StartCoroutine(LoadPopupItemInfo(csItem));
    }

    // 행운석 구매여부 토글
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSavePopupProtect(bool bIsOn)
    {
        m_bSavePopupProtect = bIsOn;

        Transform trOptionSetting = m_trPopupProtect.Find("OptionSetting");
        //Toggle toggleSave = trOptionSetting.Find("ToggleOptionSetting").GetComponent<Toggle>();

        Text texttrOptionSetting = trOptionSetting.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(texttrOptionSetting);

        if (bIsOn)
        {
            texttrOptionSetting.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            texttrOptionSetting.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBuyProtect()
    {
        //CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
		//CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
		Destroy(this.gameObject);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");
        // 강화 버튼 초기화
        m_buttonEnchant = transform.Find("ButtonEnchant").GetComponent<Button>();
        m_buttonEnchant.onClick.RemoveAllListeners();
        m_buttonEnchant.onClick.AddListener(() => OnClickMainGearEnchant());
        m_buttonEnchant.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        // 강화 버튼 텍스트 초기화
        Text textEnchant = m_buttonEnchant.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnchant);
        textEnchant.text = CsConfiguration.Instance.GetString("A06_BTN_00001");

        // 상점 버튼 초기화
        m_buttonShop = transform.Find("ButtonShop").GetComponent<Button>();
        m_buttonShop.onClick.RemoveAllListeners();
        m_buttonShop.onClick.AddListener(() => OnClickShop());
        m_buttonShop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        // 상점 버튼 텍스트 초기화
        Text textShop = m_buttonShop.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textShop);
        textShop.text = CsConfiguration.Instance.GetString("A06_BTN_00002");

        // 강화 세트 버튼 초기화
        m_buttonSetEquipment = transform.Find("ButtonSetEquipment").GetComponent<Button>();
        m_buttonSetEquipment.onClick.RemoveAllListeners();
        m_buttonSetEquipment.onClick.AddListener(() => OnClickSetEquipment());
        m_buttonSetEquipment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        // 강화 세트 레벨 텍스트 초기화
        Text textSetEquipment = m_buttonSetEquipment.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSetEquipment);
        //textSetEquipment.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), );

        // 강화 성공 확률 텍스트 초기화
        m_textSuccessivity = transform.Find("TextSuccessivity").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textSuccessivity);

        // 강화 가능 횟수 텍스트 초기화
        m_textEnchantDailyCount = transform.Find("TextEnchantCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEnchantDailyCount);
        int nEnchantDailyCount = CsGameData.Instance.MyHeroInfo.MainGearEnchantDailyCount;
        m_textEnchantDailyCount.text = string.Format(CsConfiguration.Instance.GetString("A06_TXT_01002"), nEnchantDailyCount, 20);

        // 최대 레벨 도달 텍스트 초기화
        m_textMaxLevel = transform.Find("TextMaxLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMaxLevel);
        m_textMaxLevel.text = CsConfiguration.Instance.GetString("A06_TXT_00006");

        // 아이템 슬롯 찾기
        Transform trItemGrid1 = transform.Find("ItemGrid");
        Transform trItemGrid2 = trItemGrid1.Find("ItemGridEnchant");
        m_trSlotMainGear = trItemGrid2.Find("ItemSlotMain");
        m_trPlus = trItemGrid2.Find("ImagePlus");
        m_trSlotEnchantMaterial = trItemGrid2.Find("ItemSlotMaterial");
        m_trSlotProtect = trItemGrid1.Find("ItemSlotProtect");

        // 강화 정도
        m_trEnchatList = transform.Find("EnchantList");

        // 아이템 속성
        Transform trAttrList = transform.Find("AttrList");
        for (int i = 0; i < 3; i++)
        {
            m_atrAttr[i] = trAttrList.Find("BattlePower" + i);
            m_atrAttr[i].gameObject.SetActive(false);
        }

        // 행운석 사용 여부
        m_trProtectDesc = transform.Find("ProtectDescription");

        m_toggleProtect = m_trProtectDesc.Find("ToggleProtect").GetComponent<Toggle>();
        m_toggleProtect.isOn = false;
        m_toggleProtect.onValueChanged.AddListener((ison) => OnValueChangedProtectUse(ison));
        m_toggleProtect.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        // 행운석 사용 여부토글 텍스트
        m_textProtect = m_trProtectDesc.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textProtect);
        m_textProtect.text = CsConfiguration.Instance.GetString("A06_TXT_00003");
        m_textProtect.color = CsUIData.Instance.ColorGray;

        // 행운석 구매 여부 팝업
        m_trPopupProtect = transform.Find("PopupProtect/ImageBackground");
        m_trPopupProtect.parent.gameObject.SetActive(false);

        m_trParticleMainGearEnchantResult = m_trSlotMainGear.Find("MainGear_Enchant_Result");

        FirstMainGearCheck();

        if (CsGameData.Instance.MyHeroInfo.CheckNoticeMainGearEnchant() && CsConfiguration.Instance.GetTutorialKey(EnTutorialType.MainGearEnchant))
        {
            CsGameEventUIToUI.Instance.OnEventReferenceTutorial(EnTutorialType.MainGearEnchant);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitiallizePopupProtect()
    {
        // 팝업 버튼 초기화
        Button buttonForcedEnchant = m_trPopupProtect.Find("ButtonForcedEnchant").GetComponent<Button>();
        buttonForcedEnchant.onClick.RemoveAllListeners();
        buttonForcedEnchant.onClick.AddListener(() => OnClickMainGearEnchant());
        buttonForcedEnchant.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textForcedEnchant = buttonForcedEnchant.transform.Find("TextForcedEnchant").GetComponent<Text>();
        CsUIData.Instance.SetFont(textForcedEnchant);
        textForcedEnchant.text = CsConfiguration.Instance.GetString("A06_BTN_00003");

        Button buttonBuyProtect = m_trPopupProtect.Find("ButtonBuy").GetComponent<Button>();
        buttonBuyProtect.onClick.RemoveAllListeners();
        buttonBuyProtect.onClick.AddListener(() => OnClickBuyProtect());
        buttonBuyProtect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textBuyProtect = buttonBuyProtect.transform.Find("TextBuy").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBuyProtect);
        textBuyProtect.text = CsConfiguration.Instance.GetString("A06_BTN_00004");

        // 팝업 텍스트 초기화
        Text textDesc = m_trPopupProtect.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDesc);
        textDesc.text = CsConfiguration.Instance.GetString("A06_TXT_00004");

        Transform trOptionSetting = m_trPopupProtect.Find("OptionSetting");

        Toggle toggleOptionSetting = trOptionSetting.Find("ToggleOptionSetting").GetComponent<Toggle>();
        toggleOptionSetting.isOn = false;
        toggleOptionSetting.onValueChanged.AddListener((ison) => OnValueChangedSavePopupProtect(ison));
        toggleOptionSetting.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textOptionSetting = trOptionSetting.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionSetting);
        textOptionSetting.text = CsConfiguration.Instance.GetString("A06_TXT_00005");
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

        UpdateSlotEnchantMaterial(csHeroMainGear);
        UpdateSlotProtect(csHeroMainGear);

        // 장비 강화정도 표시
        CsUIData.Instance.UpdateEnchantLevelIcon(m_trEnchatList, csHeroMainGear.EnchantLevel);

        UpdateAttr(csHeroMainGear);
        UpdateTextSuccessivity(csHeroMainGear);
        UpdateTextEnchantDailyCount(csHeroMainGear);
        UpdateButton(csHeroMainGear);

        UpdateMainGearEnchantLevelSet(CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSlotMainGear(CsHeroMainGear csHeroMainGear)
    {
        // 아이템 슬롯 업데이트
        CsUIData.Instance.DisplayItemSlot(m_trSlotMainGear, csHeroMainGear, EnItemSlotSize.Large);

        // 아이템 이름 텍스트 업데이트
        Text textName = m_trSlotMainGear.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csHeroMainGear.MainGear.Name;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSlotEnchantMaterial(CsHeroMainGear csHeroMainGear)
    {
        if (CsGameData.Instance.GetMainGearEnchantLevel(csHeroMainGear.EnchantLevel + 1) == null)
        {
            m_trPlus.gameObject.SetActive(false);
            m_trSlotEnchantMaterial.gameObject.SetActive(false);
        }
        else
        {
            m_trPlus.gameObject.SetActive(true);
            // 아이템 강화 재료 표시
            CsMainGearEnchantLevel csMainGearEnchantLevel = csHeroMainGear.MainGearEnchantLevel;

            CsItem csItemEnchantMaterial = csMainGearEnchantLevel.MainGearEnchantStep.NextEnchantMaterialItem;
            int nEnchantMaterialCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemEnchantMaterial.ItemId);

            string strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nEnchantMaterialCount, 1);

            CsUIData.Instance.DisplayItemSlot(m_trSlotEnchantMaterial, csItemEnchantMaterial, false, nEnchantMaterialCount, csItemEnchantMaterial.UsingRecommendationEnabled, EnItemSlotSize.Large);

            Text textName = m_trSlotEnchantMaterial.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csItemEnchantMaterial.Name;

            Text textCount = m_trSlotEnchantMaterial.Find("Item/TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);
            textCount.text = strItemCount;

            Image imageDim = m_trSlotEnchantMaterial.Find("ImageCooltime").GetComponent<Image>();

            if (1 <= nEnchantMaterialCount)
            {
                m_bEnchant = true;

                textCount.color = CsUIData.Instance.ColorWhite;

                imageDim.gameObject.SetActive(false);
            }
            else
            {
                m_bEnchant = false;

                textCount.color = CsUIData.Instance.ColorRed;

                imageDim.gameObject.SetActive(true);
                imageDim.fillAmount = 1f;
            }

            m_trSlotEnchantMaterial.gameObject.SetActive(true);

            Button buttonEnchantMaterial = m_trSlotEnchantMaterial.GetComponent<Button>();
            buttonEnchantMaterial.onClick.RemoveAllListeners();
            buttonEnchantMaterial.onClick.AddListener(() => OnClickItemInfo(csItemEnchantMaterial));
            buttonEnchantMaterial.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSlotProtect(CsHeroMainGear csHeroMainGear)
    {
        if (CsGameData.Instance.GetMainGearEnchantLevel(csHeroMainGear.EnchantLevel + 1) == null)
        {
            m_trSlotProtect.gameObject.SetActive(false);
            m_trProtectDesc.gameObject.SetActive(false);
        }
        else
        {
            // 아이템 강화 재료 표시
            CsMainGearEnchantLevel csMainGearEnchantLevel = csHeroMainGear.MainGearEnchantLevel;

            // 행운석 사용가능 여부
            if (csMainGearEnchantLevel.PenaltyPreventEnabled)
            {
                // 행운석 슬롯 활성화
                CsItem csItemProtect = csMainGearEnchantLevel.MainGearEnchantStep.NextEnchantPenaltyPreventItem;

                int nProtectCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemProtect.ItemId);
                string strItemCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nProtectCount, 1);

                CsUIData.Instance.DisplayItemSlot(m_trSlotProtect, csItemProtect, false, nProtectCount, csItemProtect.UsingRecommendationEnabled, EnItemSlotSize.Large);

                Text textName = m_trSlotProtect.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);
                textName.text = csItemProtect.Name;
                textName.color = CsUIData.Instance.ColorWhite;

                Text textCount = m_trSlotProtect.Find("Item/TextCount").GetComponent<Text>();
                CsUIData.Instance.SetFont(textCount);
                textCount.text = strItemCount;

                Image imageDim = m_trSlotProtect.Find("ImageCooltime").GetComponent<Image>();

                if (1 <= nProtectCount)
                {
                    textCount.color = CsUIData.Instance.ColorWhite;

                    imageDim.gameObject.SetActive(false);
                }
                else
                {
                    textCount.color = CsUIData.Instance.ColorRed;

                    imageDim.gameObject.SetActive(true);
                    imageDim.fillAmount = 1f;
                }

                // 행운석 사용 여부(행운석 사용시)
                if (m_bProtectUse)
                {
                    textName.color = CsUIData.Instance.ColorWhite;

                    if (1 > nProtectCount)
                    {
                        m_bEnchant = false;
                    }
                }
                else
                {
                    // 이름과 수량 텍스트를 회색으로
                    textName.color = CsUIData.Instance.ColorGray;

                    // 행운석 보유량 조건이 충족할 때
                    if (1 <= nProtectCount)
                    {
                        textCount.color = CsUIData.Instance.ColorGray;

                        // 배경 비활성화
                        imageDim.gameObject.SetActive(true);
                        imageDim.fillAmount = 1f;
                    }
                }

                m_trSlotProtect.gameObject.SetActive(true);
                m_trProtectDesc.gameObject.SetActive(true);

                Button buttonProtect = m_trSlotProtect.GetComponent<Button>();
                buttonProtect.onClick.RemoveAllListeners();
                buttonProtect.onClick.AddListener(() => OnClickItemInfo(csItemProtect));
                buttonProtect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            //행운석 사용 가능 여부
            else
            {
                m_trSlotProtect.gameObject.SetActive(false);
                m_trProtectDesc.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttr(CsHeroMainGear csHeroMainGear)
    {
        if (CsGameData.Instance.GetMainGearEnchantLevel(csHeroMainGear.EnchantLevel + 1) == null)
        {
            for (int i = 0; i < 3; i++)
            {
                m_atrAttr[i].gameObject.SetActive(false);
            }

            m_textMaxLevel.gameObject.SetActive(true);
        }
        else
        {
            // 장비 강화 옵션 초기화
            for (int i = 0; i < 3; i++)
            {
                m_atrAttr[i].gameObject.SetActive(false);
            }

            // 장비 강화 옵션
            List<CsAttrValue> listcsAttrValue = csHeroMainGear.BaseAttrValueList;
            for (int i = 0; i < listcsAttrValue.Count; i++)
            {
                m_atrAttr[i].gameObject.SetActive(true);

                CsMainGearBaseAttr csMainGearBaseAttr = csHeroMainGear.MainGear.GetMainGearBaseAttr(listcsAttrValue[i].Attr.AttrId);

                // 현재 속성값
                int nValue = csHeroMainGear.BaseAttrValueList[i].Value;
                // 현재 강화 레벨의 추가 속성값
                int nNextValue = csMainGearBaseAttr.GetMainGearBaseAttrEnchantLevel(csHeroMainGear.EnchantLevel + 1).Value;

                Text textAttrName = m_atrAttr[i].Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrName);
                textAttrName.text = csMainGearBaseAttr.Attr.Name;

                Text textValue = m_atrAttr[i].Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textValue);
                textValue.text = nValue.ToString("#,##0");

                Text textChangeValue = m_atrAttr[i].Find("TextChangeValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textChangeValue);
                textChangeValue.text = (nNextValue - nValue).ToString("#,##0");
            }

            m_textMaxLevel.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextSuccessivity(CsHeroMainGear csHeroMainGear)
    {
        if (CsGameData.Instance.GetMainGearEnchantLevel(csHeroMainGear.EnchantLevel + 1) == null)
        {
            m_textSuccessivity.gameObject.SetActive(false);
        }
        else
        {
            // 성공 확률 텍스트
            int nSuccessivity = csHeroMainGear.MainGearEnchantLevel.NextSuccessRatePercentage;
            m_textSuccessivity.text = string.Format(CsConfiguration.Instance.GetString("A06_TXT_01001"), nSuccessivity);

            m_textSuccessivity.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextEnchantDailyCount(CsHeroMainGear csHeroMainGear)
    {
        if (CsGameData.Instance.GetMainGearEnchantLevel(csHeroMainGear.EnchantLevel + 1) == null)
        {
            m_textEnchantDailyCount.gameObject.SetActive(false);
        }
        else
        {
            int nRemainingEnchantCount = CsGameData.Instance.MyHeroInfo.VipLevel.MainGearEnchantMaxCount - CsGameData.Instance.MyHeroInfo.MainGearEnchantDailyCount;

            if (nRemainingEnchantCount <= 0)
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnchant, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnchant, true);
            }

            m_textEnchantDailyCount.text = string.Format(CsConfiguration.Instance.GetString("A06_TXT_01002"), nRemainingEnchantCount, CsGameData.Instance.MyHeroInfo.VipLevel.MainGearEnchantMaxCount);
            m_textEnchantDailyCount.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButton(CsHeroMainGear csHeroMainGear)
    {
        if (CsGameData.Instance.GetMainGearEnchantLevel(csHeroMainGear.EnchantLevel + 1) == null)
        {
            // 강화 / 상점 버튼 비활성화
            m_buttonEnchant.gameObject.SetActive(false);
            m_buttonShop.gameObject.SetActive(false);
        }
        else
        {
            // 강화 / 상점 버튼 표시
            if (m_bEnchant == true)
            {
                m_buttonShop.gameObject.SetActive(false);
                m_buttonEnchant.gameObject.SetActive(true);
            }
            else
            {
                m_buttonEnchant.gameObject.SetActive(false);
                m_buttonShop.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearEnchantLevelSet(bool bIson)
    {
        Transform trImageNotice = m_buttonSetEquipment.transform.Find("ImageNotice");
        trImageNotice.gameObject.SetActive(bIson);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSetInfo()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupSetInfo/PopupSetInfo");
        yield return resourceRequest;
        m_goPopupSetInfo = (GameObject)resourceRequest.asset;

        OpenPopupSetInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSetInfo()
    {
        GameObject goPopupSetInfo = Instantiate(m_goPopupSetInfo, m_trPopupList);
        m_trPopupSetInfo = goPopupSetInfo.transform;

        CsPopupSetInfo csPopupSetInfo = goPopupSetInfo.GetComponent<CsPopupSetInfo>();
        csPopupSetInfo.EventClosePopupSetInfo += OnEventClosePopupSetInfo;
        csPopupSetInfo.DisplayType(EnPopupSetInfoType.MainGear);
        csPopupSetInfo.SetPosition(EnPopupSetInfoPosition.MainGear);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupSetInfo()
    {
        if (m_trPopupSetInfo == null)
        {
            return;
        }
        else
        {
            CsPopupSetInfo csPopupSetInfo = m_trPopupSetInfo.GetComponent<CsPopupSetInfo>();
            csPopupSetInfo.EventClosePopupSetInfo -= OnEventClosePopupSetInfo;

            Destroy(m_trPopupSetInfo.gameObject);
            m_trPopupSetInfo = null;
        }
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