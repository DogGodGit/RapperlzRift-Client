using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsWingEnchant : CsPopupSub
{
    Transform m_trWingAttrPartList;
    Transform m_trWingEnchantMaterial;

    Button m_buttonEnchant;
    Button m_buttonEnchantTotally;

    Image m_imageWingExp;

    Text m_textWingName;
    Text m_textWingExpCount;
    Text m_textBattlePowerValue;
    Text m_textMaxLevel;

    int m_nPartId = 1;
    bool m_bFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventWingEnchant += OnEventWingEnchant;
        CsGameEventUIToUI.Instance.EventWingEnchantTotally += OnEventWingEnchantTotally;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventWingEnchant -= OnEventWingEnchant;
        CsGameEventUIToUI.Instance.EventWingEnchantTotally -= OnEventWingEnchantTotally;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        CsWing csWing = CsGameData.Instance.GetWing(CsGameData.Instance.MyHeroInfo.EquippedWingId);
        LoadWingModel(csWing);

        for (int i = 0; i < m_trWingAttrPartList.childCount; i++)
        {
            Toggle toggleWingPart = m_trWingAttrPartList.GetChild(i).GetComponent<Toggle>();

            if (toggleWingPart == null)
            {
                continue;
            }
            else
            {
                if (i == 0)
                {
                    toggleWingPart.isOn = true;
                }
                else
                {
                    if (toggleWingPart.isOn)
                    {
                        toggleWingPart.isOn = false;
                    }
                }
            }
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEnchant()
    {
        UpdateWingInfo();
        UpdateWingAttrPatrList();
        UpdateWingEnchantMaterial();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEnchantTotally()
    {
        UpdateWingInfo();
        UpdateWingAttrPatrList();
        UpdateWingEnchantMaterial();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedWingPart(bool bIson, int partId)
    {
        if (bIson)
        {
            m_nPartId = partId;
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingEnchant()
    {
        // partId 의 누적 강화치가 limitEnchant 이상
        // partId
        CsHeroWingPart csHeroWingPart = CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nPartId);

        if (csHeroWingPart == null)
        {
            return;
        }
        else
        {
            CsWingStep csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep);

            if (csWingStep == null)
            {
                return;
            }
            else
            {
                CsWingStepLevel csWingStepLevel = csWingStep.WingStepLevelList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.WingLevel);

                if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WingEnchantItemId) >= csWingStep.EnchantMaterialItemCount)
                {
                    int nEnchantCount = 0;

                    for (int i = 0; i < CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nPartId).HeroWingEnchantList.Count; i++)
                    {
                        nEnchantCount += CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nPartId).HeroWingEnchantList[i].EnchantCount;
                    }

                    if (nEnchantCount < csWingStepLevel.AccEnchantLimitCount)
                    {
                        CsCommandEventManager.Instance.SendWingEnchant(m_nPartId);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A22_TXT_02001"));
                    }
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A22_TXT_02002"));
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingEnchantTotally()
    {
        // partId
        CsHeroWingPart csHeroWingPart = CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nPartId);

        if (csHeroWingPart == null)
        {
            return;
        }
        else
        {
            CsWingStep csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep);

            if (csWingStep == null)
            {
                return;
            }
            else
            {
                CsWingStepLevel csWingStepLevel = csWingStep.WingStepLevelList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.WingLevel);

                if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WingEnchantItemId) >= csWingStep.EnchantMaterialItemCount)
                {
                    int nEnchantCount = 0;

                    for (int i = 0; i < CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nPartId).HeroWingEnchantList.Count; i++)
                    {
                        nEnchantCount += CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nPartId).HeroWingEnchantList[i].EnchantCount;
                    }

                    if (nEnchantCount < csWingStepLevel.AccEnchantLimitCount)
                    {
                        CsCommandEventManager.Instance.SendWingEnchantTotally(m_nPartId);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A22_TXT_02001"));
                    }
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A22_TXT_02002"));
                }
            }
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CsWingStep csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep);

        if (csWingStep == null)
        {
            return;
        }

        CsWingStepLevel csWingStepLevel = csWingStep.WingStepLevelList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.WingLevel);

        Transform trImageFrameName = transform.Find("ImageFrameName");

        m_textWingName = trImageFrameName.Find("TextWingName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWingName);
        m_textWingName.text = string.Format(CsConfiguration.Instance.GetString("A22_TXT_01003"), csWingStep.ColorCode, CsGameData.Instance.MyHeroInfo.WingLevel, csWingStep.Name);

        Transform trImageWingExpBack = transform.Find("ImageGuageCircleBack");
        m_imageWingExp = trImageWingExpBack.Find("ImageWingExp").GetComponent<Image>();
        m_imageWingExp.fillAmount = CsGameData.Instance.MyHeroInfo.WingExp / (float)csWingStepLevel.NextLevelUpRequiredExp;

        m_textWingExpCount = transform.Find("TextWingExpCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWingExpCount);
        m_textWingExpCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.WingExp, csWingStepLevel.NextLevelUpRequiredExp);

        Text textBattlePower = transform.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePower);
        textBattlePower.text = CsConfiguration.Instance.GetString("A22_TXT_00001");

        m_textBattlePowerValue = transform.Find("TextBattlePowerValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBattlePowerValue);

        m_trWingAttrPartList = transform.Find("WingAttrPartList");

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingPartList.Count; i++)
        {
            Transform trWingAttrPart = m_trWingAttrPartList.Find("ToggleWingAttr" + i);

            Text textAttrName = trWingAttrPart.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = CsGameData.Instance.WingPartList[i].Attr.Name;

            Text textAttrValue = trWingAttrPart.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);

            int nPartId = CsGameData.Instance.WingPartList[i].PartId;

            Toggle toggleWingAttrPart = trWingAttrPart.GetComponent<Toggle>();
            toggleWingAttrPart.group = m_trWingAttrPartList.GetComponent<ToggleGroup>();
            toggleWingAttrPart.onValueChanged.RemoveAllListeners();

            if (i == 0)
            {
                toggleWingAttrPart.isOn = true;
            }

            toggleWingAttrPart.onValueChanged.AddListener((ison) => OnValueChangedWingPart(ison, nPartId));
        }

        UpdateWingAttrPatrList();

        m_trWingEnchantMaterial = transform.Find("ImageFrameMaterial");

        Text textInfo = m_trWingEnchantMaterial.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfo);
        textInfo.text = CsConfiguration.Instance.GetString("A22_TXT_00002");

        m_buttonEnchant = transform.Find("ButtonEnchant").GetComponent<Button>();
        m_buttonEnchant.onClick.RemoveAllListeners();
        m_buttonEnchant.onClick.AddListener(() => OnClickWingEnchant());
        m_buttonEnchant.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonEnchant = m_buttonEnchant.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEnchant);
        textButtonEnchant.text = CsConfiguration.Instance.GetString("A22_BTN_00001");

        m_buttonEnchantTotally = transform.Find("ButtonEnchantAll").GetComponent<Button>();
        m_buttonEnchantTotally.onClick.RemoveAllListeners();
        m_buttonEnchantTotally.onClick.AddListener(() => OnClickWingEnchantTotally());
        m_buttonEnchantTotally.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonEnchantAll = m_buttonEnchantTotally.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEnchantAll);
        textButtonEnchantAll.text = CsConfiguration.Instance.GetString("A22_BTN_00002");

        m_textMaxLevel = transform.Find("TextMaxLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMaxLevel);
        m_textMaxLevel.text = CsConfiguration.Instance.GetString("A22_TXT_00003");

        UpdateWingEnchantMaterial();

        CsWing csWing = CsGameData.Instance.GetWing(CsGameData.Instance.MyHeroInfo.EquippedWingId);
        LoadWingModel(csWing);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingInfo()
    {
        // 스텝 레벨 다시 계산
        CsWingStep csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep);

        if (csWingStep == null)
        {
            return;
        }
        else
        {
            CsWingStepLevel csWingStepLevel = csWingStep.WingStepLevelList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.WingLevel);

            m_textWingName.text = string.Format(CsConfiguration.Instance.GetString("A22_TXT_01003"), csWingStep.ColorCode, CsGameData.Instance.MyHeroInfo.WingLevel, csWingStep.Name);
            m_imageWingExp.fillAmount = CsGameData.Instance.MyHeroInfo.WingExp / (float)csWingStepLevel.NextLevelUpRequiredExp;
            m_textWingExpCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.WingExp, csWingStepLevel.NextLevelUpRequiredExp);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingAttrPatrList()
    {
        int nBattlePowerValue = 0;

        // 누적 속성 전투력 합
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingList.Count; i++)
        {
            CsWing csWing = CsGameData.Instance.MyHeroInfo.HeroWingList[i].Wing;
            nBattlePowerValue += csWing.BattlePower;
        }

        // 강화 속성 전투력 합
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingPartList.Count; i++)
        {
            Transform trWingAttrPart = m_trWingAttrPartList.Find("ToggleWingAttr" + i);

            Text textAttrValue = trWingAttrPart.Find("TextValue").GetComponent<Text>();

            int nEnchantCount = 0;
            int nAttrValue = 0;

            CsWingStep csWingStep = null;

            for (int j = 0; j < CsGameData.Instance.MyHeroInfo.HeroWingPartList[i].HeroWingEnchantList.Count; j++)
            {
                csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.HeroWingPartList[i].HeroWingEnchantList[j].Step);

                if (csWingStep == null)
                {

                }
                else
                {
                    CsWingStepPart csWingStepPart = csWingStep.GetWingStepPart(CsGameData.Instance.MyHeroInfo.HeroWingPartList[i].PartId);

                    if (csWingStepPart == null)
                    {

                    }
                    else
                    {
                        CsHeroWingEnchant csHeroWingEnchant = CsGameData.Instance.MyHeroInfo.HeroWingPartList[i].HeroWingEnchantList[j];

                        nEnchantCount += csHeroWingEnchant.EnchantCount;
                        nAttrValue += csHeroWingEnchant.EnchantCount * csWingStepPart.IncreaseAttrValueInfo.Value;
                    }
                }
            }

            csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep);

            if (csWingStep == null)
            {

            }
            else
            {
                textAttrValue.text = string.Format(CsConfiguration.Instance.GetString("A22_TXT_01002"), nAttrValue.ToString("#,##0"), csWingStep.WingStepPartList[i].IncreaseAttrValueInfo.Value.ToString("#,##0"));
            }

            CsWingStepLevel csWingStepLevel = null;
            // 날개 강화 만렙
            if (CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep + 1) == null
                && CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep).WingStepLevelList.Count == CsGameData.Instance.MyHeroInfo.WingLevel)
            {
                csWingStepLevel = csWingStep.WingStepLevelList.Find(a => a.Level == (CsGameData.Instance.MyHeroInfo.WingLevel - 1));
            }
            else
            {
                csWingStepLevel = csWingStep.WingStepLevelList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.WingLevel);
            }

            if (csWingStepLevel == null)
            {
                //
            }
            else
            {
                Slider sliderUpgradeGuage = trWingAttrPart.Find("UpgradeGuage").GetComponent<Slider>();
                sliderUpgradeGuage.maxValue = csWingStepLevel.AccEnchantLimitCount;

                Text textUpgradeCount = sliderUpgradeGuage.transform.Find("TextCount").GetComponent<Text>();
                CsUIData.Instance.SetFont(textUpgradeCount);

                if (CsGameData.Instance.MyHeroInfo.HeroWingPartList.Count == 0)
                {
                    textUpgradeCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), 0, sliderUpgradeGuage.maxValue.ToString("#,##0"));
                    sliderUpgradeGuage.value = 0;
                }
                else
                {
                    // 누적 강화 횟수
                    textUpgradeCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nEnchantCount.ToString("#,##0"), sliderUpgradeGuage.maxValue.ToString("#,##0"));
                    sliderUpgradeGuage.value = nEnchantCount;
                }
            }

            nBattlePowerValue += CsGameData.Instance.MyHeroInfo.HeroWingPartList[i].GetBattlePower();
        }

        m_textBattlePowerValue.text = nBattlePowerValue.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingEnchantMaterial()
    {
        // 날개 강화 만렙
        if (CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep + 1) == null
            && CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep).WingStepLevelList.Count == CsGameData.Instance.MyHeroInfo.WingLevel)
        {
            m_textWingExpCount.gameObject.SetActive(false);
            m_trWingEnchantMaterial.gameObject.SetActive(false);
            m_buttonEnchant.gameObject.SetActive(false);
            m_buttonEnchantTotally.gameObject.SetActive(false);

            m_textMaxLevel.gameObject.SetActive(true);
        }
        else
        {
            // 스텝 레벨 다시 계산
            CsWingStep csWingStep = CsGameData.Instance.GetWingStep(CsGameData.Instance.MyHeroInfo.WingStep);

            if (csWingStep == null)
            {
                return;
            }
            else
            {
                m_textMaxLevel.gameObject.SetActive(false);

                Text textCount = m_trWingEnchantMaterial.Find("TextCount").GetComponent<Text>();
                textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WingEnchantItemId), csWingStep.EnchantMaterialItemCount);

                if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WingEnchantItemId) < csWingStep.EnchantMaterialItemCount)
                {
                    textCount.color = CsUIData.Instance.ColorRed;
                }
                else
                {
                    textCount.color = CsUIData.Instance.ColorWhite;
                }

                m_textWingExpCount.gameObject.SetActive(true);
                m_trWingEnchantMaterial.gameObject.SetActive(true);
                m_buttonEnchant.gameObject.SetActive(true);
                m_buttonEnchantTotally.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void LoadWingModel(CsWing csWing)
    {
        Transform tr3DWing = transform.Find("3DWing");

        for (int i = 0; i < tr3DWing.childCount; i++)
        {
            tr3DWing.GetChild(i).gameObject.SetActive(false);
        }

        tr3DWing.Find("UIChar_Camera_Wing").gameObject.SetActive(true);

        if (csWing == null)
        {
            return;
        }
        else
        {
            Transform trWingModel = tr3DWing.Find(csWing.PrefabName);

            if (trWingModel == null)
            {
                StartCoroutine(LoadWingModelCoroutine(csWing));
            }
            else
            {
                trWingModel.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadWingModelCoroutine(CsWing csWing)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/WingObject/" + csWing.PrefabName);
        yield return resourceRequest;
        GameObject goWing = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DWing"));

        int nLayer = LayerMask.NameToLayer("UIChar");
        Transform[] atrWing = goWing.GetComponentsInChildren<Transform>();

        for (int i = 0; i < atrWing.Length; ++i)
        {
            atrWing[i].gameObject.layer = nLayer;
        }

        goWing.name = csWing.PrefabName;

        goWing.transform.localPosition = new Vector3(0, -30, 500);
        goWing.transform.eulerAngles = new Vector3(0, 90.0f, -90.0f);
        goWing.transform.localScale = new Vector3(250, 250, 250);

        goWing.gameObject.SetActive(true);
    }
}