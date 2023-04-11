using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupCostumeEffect : CsUpdateableMonoBehaviour
{
    GameObject m_goToggleCostumeItem;
    GameObject m_goCostumeEffectItem;

    Transform m_trCostumeContent;
    Transform m_trCostumeEffectContent;

    Text m_textNoHeroCostume;

    CsHeroCostume m_csHeroCostume = null;

    float m_flTime = 0f;

    public event Delegate EventClosePopupCostumeEffect;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsCostumeManager.Instance.EventCostumeEffectApply += OnEventCostumeEffectApply;
        CsCostumeManager.Instance.EventCostumePeriodExpired += OnEventCostumePeriodExpired;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsCostumeManager.Instance.EventCostumeEffectApply -= OnEventCostumeEffectApply;
        CsCostumeManager.Instance.EventCostumePeriodExpired -= OnEventCostumePeriodExpired;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1f <= Time.time)
        {
            DisplayCostumes();
            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCostumeEffect()
    {
        if (EventClosePopupCostumeEffect != null)
        {
            EventClosePopupCostumeEffect();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        OnEventClosePopupCostumeEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEffectApply(int nEffectId)
    {
        DisplayCostumes();
        DisplayCostumeEffects();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumePeriodExpired()
    {
        DisplayCostumes();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        OnEventClosePopupCostumeEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleCostume(bool bIson, CsHeroCostume csHeroCostume)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_csHeroCostume = csHeroCostume;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeEffectItemUse(CsCostumeEffect csCostumeEffect)
    {
        if (CsGameData.Instance.MyHeroInfo.GetItemCount(csCostumeEffect.RequiredItem.ItemId) == 0)
        {
            // 구매
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
            OnEventClosePopupCostumeEffect();
        }
        else
        {
            if (m_csHeroCostume == null)
            {
                return;
            }
            else
            {
                // 사용
                string strMessage = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00020"), m_csHeroCostume.Costume.Name, csCostumeEffect.Name);

                CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsCostumeManager.Instance.SendCostumeEffectApply(m_csHeroCostume.HeroCostumeId, csCostumeEffect.CostumeEffectId),
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A151_TXT_00010");

        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClose);

        Text textClose = buttonClose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClose);
        textClose.text = CsConfiguration.Instance.GetString("A151_TXT_00025");

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

        m_trCostumeEffectContent = transform.Find("CostumeEffectItemList/Viewport/Content");

        DisplayCostumeEffects();
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

        listCsHeroCostume.Clear();
        listCsHeroCostume = null;
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

            Text textCostumeTimer = trCostumeInfo.Find("CostumeTime/TextTimer").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCostumeTimer);

            if (csHeroCostume.Costume.PeriodLimitDay == 0)
            {
                // 영구
                textCostumeTimer.text = CsConfiguration.Instance.GetString("A151_TXT_00006");
            }
            else
            {
                // 남은 시간
                System.TimeSpan tsRemainingCostumeTime = System.TimeSpan.FromSeconds(csHeroCostume.RemainingTime - Time.realtimeSinceStartup);

                if (86400 <= tsRemainingCostumeTime.TotalSeconds)
                {
                    // 일, 시
                    textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00007"), tsRemainingCostumeTime.Days, tsRemainingCostumeTime.Hours);
                }
                else if (3600 <= tsRemainingCostumeTime.Hours)
                {
                    // 시, 분
                    textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00008"), tsRemainingCostumeTime.Hours, tsRemainingCostumeTime.Minutes);
                }
                else
                {
                    // 분, 초
                    textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00009"), tsRemainingCostumeTime.Minutes, tsRemainingCostumeTime.Seconds);
                }
            }

            Transform trCostumeLevelList = trCostumeInfo.Find("CostumeLevelList");
            trCostumeLevelList.gameObject.SetActive(false);

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
    void DisplayCostumeEffects()
    {
        if (m_goCostumeEffectItem == null)
        {
            StartCoroutine(LoadCostumeEffectItem());
        }
        else
        {
            UpdateCostumeEffect();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCostumeEffectItem()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/CostumeEffectItem");
        yield return resourceRequest;

        m_goCostumeEffectItem = (GameObject)resourceRequest.asset;
        UpdateCostumeEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostumeEffect()
    {
        Transform trCostumeEffectItem = null;

        for (int i = 0; i < CsGameData.Instance.CostumeEffectList.Count; i++)
        {
            CsCostumeEffect csCostumeEffect = CsGameData.Instance.CostumeEffectList[i];

            trCostumeEffectItem = m_trCostumeEffectContent.Find("CostumeEffectItem" + i);

            if (trCostumeEffectItem == null)
            {
                trCostumeEffectItem = Instantiate(m_goCostumeEffectItem, m_trCostumeEffectContent).transform;
                trCostumeEffectItem.name = "CostumeEffectItem" + i;
            }
            else
            {
                trCostumeEffectItem.gameObject.SetActive(true);
            }

            Image imageItemIcon = trCostumeEffectItem.Find("ImageItemSlot/ImageIcon").GetComponent<Image>();
            imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csCostumeEffect.RequiredItem.Image);

            Text textName = trCostumeEffectItem.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);

            CsItem csItemCostumeEffect = csCostumeEffect.RequiredItem;

            if (csItemCostumeEffect == null)
            {
                textName.text = "";
            }
            else
            {
                string strName = string.Format("<color={0}>{1}</color>", csItemCostumeEffect.ItemGrade.ColorCode, csCostumeEffect.Name);
                textName.text = strName;
            }

            Button buttonItemUse = trCostumeEffectItem.Find("ButtonItemUse").GetComponent<Button>();
            buttonItemUse.onClick.RemoveAllListeners();
            buttonItemUse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textButtonItemUse = buttonItemUse.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonItemUse);

            if (CsGameData.Instance.MyHeroInfo.GetItemCount(csCostumeEffect.RequiredItem.ItemId) == 0)
            {
                // 구매
                buttonItemUse.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_03");
                textButtonItemUse.text = CsConfiguration.Instance.GetString("A151_TXT_00018");
            }
            else
            {
                // 사용
                buttonItemUse.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_01");
                textButtonItemUse.text = CsConfiguration.Instance.GetString("A151_TXT_00019 ");
            }

            buttonItemUse.onClick.AddListener(() => OnClickCostumeEffectItemUse(csCostumeEffect));
        }
    }
}
