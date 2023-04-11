using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupCostumeEnchant : CsUpdateableMonoBehaviour
{
    GameObject m_goToggleCostumeItem;

    Transform m_trCostumeContent;
    Transform m_trEnchantFrame;
    Transform m_trEnchantLevelList;
    Transform m_trEnchantAttrList;

    Slider m_sliderLuckyPoint;

    Text m_textNoHeroCostume;
    Text m_textMaxLevel;

    CsHeroCostume m_csHeroCostume = null;

    bool m_bLevelupIng = false;

    public event Delegate EventClosePopupCostumeEnchant;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsCostumeManager.Instance.EventCostumePeriodExpired += OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeEnchant += OnEventCostumeEnchant;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsCostumeManager.Instance.EventCostumePeriodExpired -= OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeEnchant -= OnEventCostumeEnchant;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_bLevelupIng)
        {
            m_bLevelupIng = false;
        }
        else
        {
            return;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCostumeEnchant()
    {
        if (EventClosePopupCostumeEnchant != null)
        {
            EventClosePopupCostumeEnchant();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        OnEventClosePopupCostumeEnchant();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumePeriodExpired()
    {
        if (0 < CsCostumeManager.Instance.HeroCostumeList.Count)
        {
            UpdateSelectHeroCostume();

            DisplayCostumes();

            UpdateEnchantFrame();
            UpdateEnchantLevelList();
            UpdateEnchantAttrList();
            UpdateSliderLuckyPoint();
        }
        else
        {
            OnEventClosePopupCostumeEnchant();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEnchant(bool bEnchant)
    {
        UpdateEnchantFrame();
        UpdateEnchantLevelList();
        UpdateEnchantAttrList();
        UpdateSliderLuckyPoint();
        DisplayCostumes();

        if (bEnchant)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A151_TXT_00034"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A151_TXT_00035"));
        }

        if (m_bLevelupIng)
        {
            StartCoroutine(WaitingLevelUp());
        }
        else
        {
            StopCoroutine(WaitingLevelUp());
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleCostume(bool bIson, CsHeroCostume csHeroCostume)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_csHeroCostume = csHeroCostume;

            UpdateEnchantFrame();
            UpdateEnchantLevelList();
            UpdateEnchantAttrList();
            UpdateSliderLuckyPoint();

            if (m_bLevelupIng)
            {
                m_bLevelupIng = false;
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        OnEventClosePopupCostumeEnchant();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnchant()
    {
        SendCostumeEnchant();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnchantAll()
    {
        if (!m_bLevelupIng)
        {
            m_bLevelupIng = true;
            SendCostumeEnchant();
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A151_TXT_00021");

        m_csHeroCostume = null;

        UpdateSelectHeroCostume();

        m_textNoHeroCostume = transform.Find("ImageCostumeListBack/HeroCostumeList/TextNoHeroCostume").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoHeroCostume);
        m_textNoHeroCostume.text = CsConfiguration.Instance.GetString("A151_TXT_00013");

        m_trCostumeContent = transform.Find("ImageCostumeListBack/HeroCostumeList/Viewport/Content");

        if (CsCostumeManager.Instance.HeroCostumeList.Count == 0)
        {
            m_textNoHeroCostume.gameObject.SetActive(true);
        }
        else
        {
            m_textNoHeroCostume.gameObject.SetActive(false);
            DisplayCostumes();
        }

        m_trEnchantFrame = transform.Find("EnchantFrame");

        m_trEnchantLevelList = transform.Find("EnchantLevelList");

        m_trEnchantAttrList = transform.Find("EnchantAttrList");

        m_textMaxLevel = transform.Find("TextMaxLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMaxLevel);
        m_textMaxLevel.text = CsConfiguration.Instance.GetString("A151_TXT_00026");

        Text textLuckyPoint = transform.Find("TextLuckyPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLuckyPoint);
        textLuckyPoint.text = CsConfiguration.Instance.GetString("A151_TXT_00022");

        m_sliderLuckyPoint = textLuckyPoint.transform.Find("SliderLuckyPoint").GetComponent<Slider>();

        UpdateEnchantFrame();
        UpdateEnchantLevelList();
        UpdateEnchantAttrList();
        UpdateSliderLuckyPoint();

        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClose);

        Text textButtonClose = buttonClose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonClose);
        textButtonClose.text = CsConfiguration.Instance.GetString("A151_TXT_00025");

        Button buttonEnchant = transform.Find("ButtonEnchant").GetComponent<Button>();
        buttonEnchant.onClick.RemoveAllListeners();
        buttonEnchant.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonEnchant.onClick.AddListener(OnClickEnchant);

        Text textButtonEnchant = buttonEnchant.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEnchant);
        textButtonEnchant.text = CsConfiguration.Instance.GetString("A151_TXT_00023");

        Button buttonEnchantAll = transform.Find("ButtonEnchantAll").GetComponent<Button>();
        buttonEnchantAll.onClick.RemoveAllListeners();
        buttonEnchantAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonEnchantAll.onClick.AddListener(OnClickEnchantAll);

        Text textButtonEnchantAll = buttonEnchantAll.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEnchantAll);
        textButtonEnchantAll.text = CsConfiguration.Instance.GetString("A151_TXT_00024");
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCostumes()
    {
        if (m_goToggleCostumeItem == null)
        {
            StartCoroutine(LoadToggleCostumeItem());
        }
        else
        {
            UpdateCostume();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadToggleCostumeItem()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/ToggleCostumeItem");
        yield return resourceRequest;

        m_goToggleCostumeItem = (GameObject)resourceRequest.asset;
        UpdateCostume();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostume()
    {
        Transform trToggleCostumeItem = null;

        List<CsHeroCostume> listCsHeroCostume = new List<CsHeroCostume>(CsCostumeManager.Instance.HeroCostumeList).OrderBy(a => a.HeroCostumeId).ToList();

        for (int i = 0; i < listCsHeroCostume.Count; i++)
        {
            CsHeroCostume csHeroCostume = listCsHeroCostume[i];

            trToggleCostumeItem = m_trCostumeContent.Find("ToggleCostumeItem" + csHeroCostume.HeroCostumeId);

            if (trToggleCostumeItem == null)
            {
                trToggleCostumeItem = Instantiate(m_goToggleCostumeItem, m_trCostumeContent).transform;
                trToggleCostumeItem.name = "ToggleCostumeItem" + csHeroCostume.HeroCostumeId;
            }
            else
            {
                trToggleCostumeItem.gameObject.SetActive(true);
            }

            UpdateHeroCostumeItem(csHeroCostume, i);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroCostumeItem(CsHeroCostume csHeroCostume, int nIndex)
    {
        Transform trToggleCostumeItem = m_trCostumeContent.Find("ToggleCostumeItem" + csHeroCostume.HeroCostumeId);

        if (trToggleCostumeItem == null)
        {
            return;
        }
        else
        {
            CsItem csItem = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume && a.Value1 == csHeroCostume.Costume.CostumeId);
            Transform trItemSlot = trToggleCostumeItem.Find("ItemSlot");
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, true, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Transform trCostumeInfo = trToggleCostumeItem.Find("CostumeInfo");

            Text textName = trCostumeInfo.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csHeroCostume.Costume.Name;

            Text textAttr = trCostumeInfo.Find("TextAttr").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttr);
            textAttr.text = CsConfiguration.Instance.GetString("A151_TXT_00003");

            if (csHeroCostume.Costume.CostumeAttrList != null && 0 < csHeroCostume.Costume.CostumeAttrList.Count)
            {
                textAttr.gameObject.SetActive(true);
            }
            else
            {
                textAttr.gameObject.SetActive(false);
            }

            Transform trCostumeTime = trCostumeInfo.Find("CostumeTime");
            trCostumeTime.gameObject.SetActive(false);

            Transform trCostumeLevelList = trCostumeInfo.Find("CostumeLevelList");
            CsUIData.Instance.UpdateEnchantLevelIcon(trCostumeLevelList, csHeroCostume.EnchantLevel);

            // 장착
            Transform trImageEquip = trToggleCostumeItem.Find("ImageEquip");

            Text textEquip = trImageEquip.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textEquip);
            textEquip.text = CsConfiguration.Instance.GetString("A151_TXT_00004");

            Toggle toggleCostume = trToggleCostumeItem.GetComponent<Toggle>();
            toggleCostume.onValueChanged.RemoveAllListeners();

            if (csHeroCostume.HeroCostumeId == m_csHeroCostume.HeroCostumeId)
            {
                toggleCostume.isOn = true;
                
                if (CsCostumeManager.Instance.EquippedHeroCostumeId == csHeroCostume.HeroCostumeId)
                {
                    trImageEquip.gameObject.SetActive(true);
                }
                else
                {
                    trImageEquip.gameObject.SetActive(false);
                }
            }
            else
            {
                toggleCostume.isOn = false;
                trImageEquip.gameObject.SetActive(false);
            }

            toggleCostume.onValueChanged.AddListener((ison) => OnValueChangedToggleCostume(ison, csHeroCostume));
            toggleCostume.group = m_trCostumeContent.GetComponent<ToggleGroup>();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSelectHeroCostume()
    {
        if (0 < CsCostumeManager.Instance.HeroCostumeList.Count)
        {
            CsHeroCostume csHeroCostumeEquipped = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId);

            if (csHeroCostumeEquipped == null)
            {
                List<CsHeroCostume> listCsHeroCostume = new List<CsHeroCostume>(CsCostumeManager.Instance.HeroCostumeList).OrderBy(a => a.HeroCostumeId).ToList();

                m_csHeroCostume = listCsHeroCostume.First();

                listCsHeroCostume.Clear();
                listCsHeroCostume = null;
            }
            else
            {
                m_csHeroCostume = csHeroCostumeEquipped;
            }
        }
        else
        {
            m_csHeroCostume = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEnchantFrame()
    {
        if (m_csHeroCostume == null)
        {
            return;
        }
        else
        {
            CsItem csItemCostume = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume && a.Value1 == m_csHeroCostume.HeroCostumeId);
            CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.CostumeEnchantItemId);

            if (csItem == null)
            {
                return;
            }
            else
            {
                Transform trItemSlotCostume = m_trEnchantFrame.Find("ItemSlotCostume");

                CsUIData.Instance.DisplayItemSlot(trItemSlotCostume, csItemCostume, false, 0, csItemCostume.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                Text textCostumeName = trItemSlotCostume.Find("TextItemName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textCostumeName);
                textCostumeName.text = string.Format("<color={0}>{1}</color>", csItemCostume.ItemGrade.ColorCode, csItemCostume.Name);

                Transform trImageIcon = m_trEnchantFrame.Find("Image");
                Transform trItemSlot = m_trEnchantFrame.Find("ItemSlot");

                if (m_csHeroCostume.EnchantLevel == CsGameData.Instance.CostumeEnchantLevelList.Last().EnchantLevel)
                {
                    // 최고 레벨
                    trImageIcon.gameObject.SetActive(false);
                    trItemSlot.gameObject.SetActive(false);
                }
                else
                {
                    CsCostumeEnchantLevel csCostumeEnchantLevel = CsGameData.Instance.CostumeEnchantLevelList.Find(a => a.EnchantLevel == m_csHeroCostume.EnchantLevel);

                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, true, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                    Text textItemName = trItemSlot.Find("TextItemName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textItemName);
                    textItemName.text = string.Format("<color={0}>{1}</color>", csItem.ItemGrade.ColorCode, csItem.Name);

                    int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);
                    Text textCount = trItemSlot.Find("Item/TextCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textCount);
                    textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nItemCount, csCostumeEnchantLevel.NextLevelRequiredItemCount);

                    trImageIcon.gameObject.SetActive(true);
                    trItemSlot.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEnchantLevelList()
    {
        if (m_csHeroCostume == null)
        {
            return;
        }
        else
        {
            CsUIData.Instance.UpdateEnchantLevelIcon(m_trEnchantLevelList, m_csHeroCostume.EnchantLevel);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEnchantAttrList()
    {
        if (m_csHeroCostume == null)
        {
            return;
        }
        else
        {
            List<CsCostumeEnchantLevelAttr> listCsCostumeEnchantLevelAttr = new List<CsCostumeEnchantLevelAttr>();
            listCsCostumeEnchantLevelAttr = CsGameData.Instance.CostumeEnchantLevelAttrList.FindAll(a => a.CostumeId == m_csHeroCostume.HeroCostumeId).OrderBy(a => a.EnchantLevel).ToList();

            if (m_csHeroCostume.EnchantLevel == listCsCostumeEnchantLevelAttr.Last().EnchantLevel)
            {
                // 최고 레벨
                m_trEnchantAttrList.gameObject.SetActive(false);
                m_textMaxLevel.gameObject.SetActive(true);
            }
            else
            {
                m_textMaxLevel.gameObject.SetActive(false);

                Transform trAttr = null;

                Text textAttrName = null;
                Text textAttrValue = null;
                Text textIncAttrValue = null;

                CsCostumeEnchantLevelAttr csCostumeEnchantLevelAttr = null;

                if (0 < listCsCostumeEnchantLevelAttr.Count)
                {
                    listCsCostumeEnchantLevelAttr = listCsCostumeEnchantLevelAttr.FindAll(a => a.EnchantLevel == m_csHeroCostume.EnchantLevel).OrderBy(a => a.Attr.AttrId).ToList();

                    for (int i = 0; i < m_trEnchantAttrList.childCount; i++)
                    {
                        trAttr = m_trEnchantAttrList.Find("Attr" + i);

                        if (trAttr == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (i < listCsCostumeEnchantLevelAttr.Count)
                            {
                                textAttrName = trAttr.Find("TextAttrName").GetComponent<Text>();
                                CsUIData.Instance.SetFont(textAttrName);
                                textAttrName.text = listCsCostumeEnchantLevelAttr[i].Attr.Name;

                                textAttrValue = trAttr.Find("TextAttrValue").GetComponent<Text>();
                                CsUIData.Instance.SetFont(textAttrValue);
                                textAttrValue.text = listCsCostumeEnchantLevelAttr[i].AttrValue.Value.ToString("#,##0");

                                int nIncAttrValue = 0;

                                csCostumeEnchantLevelAttr = CsGameData.Instance.CostumeEnchantLevelAttrList.Find(a => a.CostumeId == m_csHeroCostume.HeroCostumeId && a.EnchantLevel == m_csHeroCostume.EnchantLevel + 1 && a.Attr.AttrId == listCsCostumeEnchantLevelAttr[i].Attr.AttrId);

                                if (csCostumeEnchantLevelAttr == null)
                                {
                                    nIncAttrValue = 0;
                                }
                                else
                                {
                                    nIncAttrValue = csCostumeEnchantLevelAttr.AttrValue.Value - listCsCostumeEnchantLevelAttr[i].AttrValue.Value;
                                }

                                textIncAttrValue = trAttr.Find("TextIncAttrValue").GetComponent<Text>();
                                CsUIData.Instance.SetFont(textIncAttrValue);
                                textIncAttrValue.text = nIncAttrValue.ToString("#,##0");

                                trAttr.gameObject.SetActive(true);
                            }
                            else
                            {
                                trAttr.gameObject.SetActive(false);
                            }
                        }
                    }

                    m_trEnchantAttrList.gameObject.SetActive(true);
                }
                else
                {
                    m_trEnchantAttrList.gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSliderLuckyPoint()
    {
        if (m_csHeroCostume == null)
        {
            return;
        }
        else
        {
            if (m_csHeroCostume.EnchantLevel == CsGameData.Instance.CostumeEnchantLevelList.Last().EnchantLevel)
            {
                m_sliderLuckyPoint.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                CsCostumeEnchantLevel csCostumeEnchantLevel = CsGameData.Instance.CostumeEnchantLevelList.Find(a => a.EnchantLevel == m_csHeroCostume.EnchantLevel);

                if (csCostumeEnchantLevel == null)
                {
                    m_sliderLuckyPoint.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    m_sliderLuckyPoint.maxValue = csCostumeEnchantLevel.NextLevelMaxLuckyValue;
                    m_sliderLuckyPoint.value = m_csHeroCostume.LuckyValue;

                    Debug.Log("csCostumeEnchantLevel.NextLevelMaxLuckyValue : " + csCostumeEnchantLevel.NextLevelMaxLuckyValue);
                    Debug.Log("m_sliderLuckyPoint.value : " + m_sliderLuckyPoint.value);

                    m_sliderLuckyPoint.transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SendCostumeEnchant()
    {
        CsCostumeEnchantLevel csCostumeEnchantLevel = CsGameData.Instance.CostumeEnchantLevelList.Find(a => a.EnchantLevel == m_csHeroCostume.EnchantLevel);

        if (csCostumeEnchantLevel.EnchantLevel == CsGameData.Instance.CostumeEnchantLevelList.Last().EnchantLevel)
        {
            // 최대 레벨
            m_bLevelupIng = false;
        }
        else
        {
            if (csCostumeEnchantLevel.NextLevelRequiredItemCount <= CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CostumeEnchantItemId))
            {
                if (m_csHeroCostume == null)
                {
                    return;
                }
                else
                {
                    CsCostumeManager.Instance.SendCostumeEnchant(m_csHeroCostume.HeroCostumeId);
                }
            }
            else
            {
                // 아이템 부족
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A22_TXT_02002"));
                m_bLevelupIng = false;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //이벤트 보내기 시간 지연
    IEnumerator WaitingLevelUp()
    {
        yield return new WaitForSeconds(0.1f);

        if (m_bLevelupIng)
        {
            SendCostumeEnchant();
        }
    }
}
