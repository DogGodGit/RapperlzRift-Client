using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupCostumeCollection : CsUpdateableMonoBehaviour
{
    GameObject m_goCostumeCollectionIncAttr;
    GameObject m_goToggleCostumeCollection;
    GameObject m_goCostumeCollectionAttr;

    Transform m_trPopupCostumeCollectionList;
    Transform m_trPopupCostumeCollectionIncAttr;

    Text m_textActivation;
    Text m_textShuffle;

    Button m_buttonActivation;
    Button m_buttonShuffle;

    CsCostumeCollection m_csCostumeCollection = null;

    public event Delegate EventClosePopupCostumeCollection;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;

        CsCostumeManager.Instance.EventCostumePeriodExpired += OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeCollectionActivate += OnEventCostumeCollectionActivate;
        CsCostumeManager.Instance.EventCostumeCollectionShuffle += OnEventCostumeCollectionShuffle;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;

        CsCostumeManager.Instance.EventCostumePeriodExpired -= OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeCollectionActivate -= OnEventCostumeCollectionActivate;
        CsCostumeManager.Instance.EventCostumeCollectionShuffle -= OnEventCostumeCollectionShuffle;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCostumeCollection()
    {
        if (EventClosePopupCostumeCollection != null)
        {
            EventClosePopupCostumeCollection();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        OnEventClosePopupCostumeCollection();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumePeriodExpired()
    {
        UpdateCostumeCollection();
        UpdateButtonActivation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeCollectionActivate()
    {
        UpdateButtonActivation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeCollectionShuffle()
    {
        UpdateCostumeCollection();
        UpdateButtonActivation();
        UpdateButtonShuffle();
        UpdateCostumeCollectionName();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopup()
    {
        OnEventClosePopupCostumeCollection();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHelp()
    {
        InitializePopupCostumeCollectionList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupCostumeCollectionList()
    {
        m_trPopupCostumeCollectionList.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangeCostumeCollection(bool bIson, CsCostumeCollection csCostumeCollection)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_csCostumeCollection = csCostumeCollection;
            LoadCostumeCollectionAttr();
            UpdateCostumeCollectionCostumeList();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeCollectionActivation()
    {
        CsCostumeManager.Instance.SendCostumeCollectionActivate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeCollectionShuffle()
    {
        CsCostumeManager.Instance.SendCostumeCollectionShuffle();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickIncAttr()
    {
        DisplayPopupCostumeCollectionIncAttr();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupCostumeCollectionIncAttr()
    {
        m_trPopupCostumeCollectionIncAttr.gameObject.SetActive(false);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        UpdateCostumeCollectionName();

        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A151_TXT_00027");

        m_trPopupCostumeCollectionIncAttr = transform.Find("PopupIncAttr");
        m_trPopupCostumeCollectionList = transform.Find("PopupCostumeCollectionList");

        Button buttonHelp = transform.Find("ButtonHelp").GetComponent<Button>();
        buttonHelp.onClick.RemoveAllListeners();
        buttonHelp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonHelp.onClick.AddListener(OnClickHelp);

        m_buttonActivation = transform.Find("ButtonActivation").GetComponent<Button>();
        m_buttonActivation.onClick.RemoveAllListeners();
        m_buttonActivation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonActivation.onClick.AddListener(OnClickCostumeCollectionActivation);

        Text textActivation = m_buttonActivation.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textActivation);
        textActivation.text = CsConfiguration.Instance.GetString("A151_TXT_00028");

        m_textActivation = transform.Find("TextActivationMaterial").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textActivation);

        m_buttonShuffle = transform.Find("ButtonShuffle").GetComponent<Button>();
        m_buttonShuffle.onClick.RemoveAllListeners();
        m_buttonShuffle.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonShuffle.onClick.AddListener(OnClickCostumeCollectionShuffle);

        Text textButtonShuffle = m_buttonShuffle.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonShuffle);
        textButtonShuffle.text = CsConfiguration.Instance.GetString("A151_TXT_00031");

        Image imageShuffleItemIcon = transform.Find("ImageGoods/ImageIcon").GetComponent<Image>();
        imageShuffleItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + CsGameConfig.Instance.CostumeCollectionShuffleItemId);

        m_textShuffle = transform.Find("ImageGoods/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textShuffle);

        Button buttonIncAttr = transform.Find("ImageIncAttr/Button").GetComponent<Button>();
        buttonIncAttr.onClick.RemoveAllListeners();
        buttonIncAttr.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonIncAttr.onClick.AddListener(OnClickIncAttr);

        Text textButtonIncAttr = transform.transform.Find("ImageIncAttr/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonIncAttr);
        textButtonIncAttr.text = CsConfiguration.Instance.GetString("A151_TXT_00030");

        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClosePopup);

        Text textButtonClose = buttonClose.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonClose);
        textButtonClose.text = CsConfiguration.Instance.GetString("A151_TXT_00025");

        UpdateCostumeCollection();
        UpdateButtonActivation();
        UpdateButtonShuffle();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostumeCollectionName()
    {
        CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(CsCostumeManager.Instance.CostumeCollectionId);

        Text textCostumeCollectionName = transform.Find("ImageGlow/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCostumeCollectionName);
        textCostumeCollectionName.text = csCostumeCollection.Name;
    }

    //---------------------------------------------------------------------------------------------------
    bool GetCostumeCollectionActivation()
    {
        if (CsCostumeManager.Instance.CostumeCollectionActivated)
        {
            return false;
        }
        else
        {
            CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(CsCostumeManager.Instance.CostumeCollectionId);

            if (csCostumeCollection == null)
            {
                return false;
            }
            else
            {
                bool bCostumeCollectionActivation = true;

                for (int i = 0; i < csCostumeCollection.CostumeCollectionEntryList.Count; i++)
                {
                    if (CsCostumeManager.Instance.HeroCostumeList.Find(a => a.Costume.CostumeId == csCostumeCollection.CostumeCollectionEntryList[i].Costume.CostumeId) == null)
                    {
                        bCostumeCollectionActivation = false;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                return bCostumeCollectionActivation;
            }
        }
    }
    
    //---------------------------------------------------------------------------------------------------
    void UpdateButtonActivation()
    {
        int nMyCostumeCollectionActivationItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CostumeCollectionActivationItemId);
        CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(CsCostumeManager.Instance.CostumeCollectionId);

        if (csCostumeCollection == null)
        {
            return;
        }
        else
        {
            if (GetCostumeCollectionActivation())
            {
                if (nMyCostumeCollectionActivationItemCount < csCostumeCollection.ActivationItemCount)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonActivation, false);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonActivation, true);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonActivation, false);
            }

            m_textActivation.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00029"), nMyCostumeCollectionActivationItemCount, csCostumeCollection.ActivationItemCount);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonShuffle()
    {
        int nMyCostumeCollectionShuffleItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CostumeCollectionShuffleItemId);

        if (nMyCostumeCollectionShuffleItemCount < CsGameConfig.Instance.CostumeCollectionShuffleItemCount)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonShuffle, false);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonShuffle, true);
        }

        m_textShuffle.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nMyCostumeCollectionShuffleItemCount, CsGameConfig.Instance.CostumeCollectionShuffleItemCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostumeCollection()
    {
        CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(CsCostumeManager.Instance.CostumeCollectionId);

        if (csCostumeCollection == null)
        {
            return;
        }
        else
        {
            Transform trCostumeCollection = transform.Find("CostumeCollection");
            Transform trCostumeList = null;

            for (int i = 0; i < trCostumeCollection.childCount; i++)
            {
                trCostumeList = trCostumeCollection.GetChild(i);
                trCostumeList.gameObject.SetActive(false);

                for (int j = 0; j < trCostumeList.childCount; j++)
                {
                    trCostumeList.GetChild(j).gameObject.SetActive(false);
                }
            }

            Transform trCostumeItem = null;

            Transform trItemSlot = null;
            Transform trImageDim = null;
            Text textItemName = null;

            int nLineIndex = 0;
            int nCostumeIndex = 0;

            for (int i = 0; i < csCostumeCollection.CostumeCollectionEntryList.Count; i++)
            {
                nLineIndex = i / 4;
                nCostumeIndex = i % 4;

                trCostumeList = trCostumeCollection.Find("CostumeList" + nLineIndex);

                if (trCostumeList.gameObject.activeSelf)
                {

                }
                else
                {
                    trCostumeList.gameObject.SetActive(true);
                }

                trCostumeItem = trCostumeList.Find("CostumeCollectionItem" + nCostumeIndex);

                trItemSlot = trCostumeItem.Find("ItemSlot");
                CsItem csItem = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume && a.Value1 == csCostumeCollection.CostumeCollectionEntryList[i].Costume.CostumeId);

                if (csItem == null)
                {
                    trItemSlot.gameObject.SetActive(false);
                }
                else
                {
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, true, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    trItemSlot.gameObject.SetActive(true);
                }

                trImageDim = trCostumeItem.Find("ImageDim");

                if (CsCostumeManager.Instance.HeroCostumeList.Find(a => a.Costume.CostumeId == csCostumeCollection.CostumeCollectionEntryList[i].Costume.CostumeId) == null)
                {
                    trImageDim.gameObject.SetActive(true);
                }
                else
                {
                    trImageDim.gameObject.SetActive(false);
                }

                textItemName = trCostumeItem.Find("TextItemName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textItemName);
                textItemName.text = csCostumeCollection.CostumeCollectionEntryList[i].Costume.Name;

                trCostumeItem.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupCostumeCollectionList()
    {
        Transform trImageBackground = m_trPopupCostumeCollectionList.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A151_TXT_00032");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClosePopupCostumeCollectionList);

        UpdatePopupCostumeCollectionList();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupCostumeCollectionList()
    {
        LoadToggleCostumeCollection();
    }

    //---------------------------------------------------------------------------------------------------
    void LoadToggleCostumeCollection()
    {
        if (m_goToggleCostumeCollection == null)
        {
            StartCoroutine(LoadToggleCostumeCollectionCoroutine());
        }
        else
        {
            UpdateToggleCostumeCollection();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadToggleCostumeCollectionCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/ToggleCostumeCollection");
        yield return resourceRequest;

        m_goToggleCostumeCollection = (GameObject)resourceRequest.asset;
        UpdateToggleCostumeCollection();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleCostumeCollection()
    {
        Transform trCostumeCollectionContent = m_trPopupCostumeCollectionList.Find("ImageBackground/ScrollCostumeCollectionList/Viewport/Content");
        Transform trToggleCostumeCollection = null;
        Toggle toggleCostumeCollection = null;

        for (int i = 0; i < CsGameData.Instance.CostumeCollectionList.Count; i++)
        {
            CsCostumeCollection csCostumeCollection = CsGameData.Instance.CostumeCollectionList[i];

            trToggleCostumeCollection = trCostumeCollectionContent.Find("ToggleCostumeCollection" + i);

            if (trToggleCostumeCollection == null)
            {
                trToggleCostumeCollection = Instantiate(m_goToggleCostumeCollection, trCostumeCollectionContent).transform;
                trToggleCostumeCollection.name = "ToggleCostumeCollection" + i;

                toggleCostumeCollection = trToggleCostumeCollection.GetComponent<Toggle>();
                toggleCostumeCollection.onValueChanged.RemoveAllListeners();

                if (i == 0)
                {
                    m_csCostumeCollection = csCostumeCollection;
                    toggleCostumeCollection.isOn = true;
                }
                else
                {
                    toggleCostumeCollection.isOn = false;
                }

                toggleCostumeCollection.onValueChanged.AddListener((ison) => OnValueChangeCostumeCollection(ison, csCostumeCollection));
                toggleCostumeCollection.group = trCostumeCollectionContent.GetComponent<ToggleGroup>();

                Text textCostumeCollection = trToggleCostumeCollection.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textCostumeCollection);
                textCostumeCollection.text = csCostumeCollection.Name;
            }
            else
            {
                trToggleCostumeCollection.gameObject.SetActive(true);
            }
        }

        LoadCostumeCollectionAttr();
        UpdateCostumeCollectionCostumeList();

        m_trPopupCostumeCollectionList.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void LoadCostumeCollectionAttr()
    {
        if (m_goCostumeCollectionAttr == null)
        {
            StartCoroutine(LoadCostumeCollectionAttrCoroutine());
        }
        else
        {
            UpdateCostumeCollectionAttr();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCostumeCollectionAttrCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/CostumeCollectionAttr");
        yield return resourceRequest;

        m_goCostumeCollectionAttr = (GameObject)resourceRequest.asset;
        UpdateCostumeCollectionAttr();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostumeCollectionAttr()
    {
        Transform trCostumeCollectionAttrContent = m_trPopupCostumeCollectionList.Find("ImageBackground/ScrollCostumeCollectionAttrList/Viewport/Content");
        Transform trCostumeCollectionAttr = null;

        for (int i = 0; i < trCostumeCollectionAttrContent.childCount; i++)
        {
            trCostumeCollectionAttr = trCostumeCollectionAttrContent.GetChild(i);
            trCostumeCollectionAttr.gameObject.SetActive(false);
        }

        trCostumeCollectionAttr = null;

        Text textAttrName = null;
        Text textAttrValue = null;

        if (m_csCostumeCollection == null)
        {
            return;
        }
        else
        {
            List<CsCostumeCollectionAttr> listCsCostumeCollectionAttr = new List<CsCostumeCollectionAttr>(m_csCostumeCollection.CostumeCollectionAttrList);

            for (int i = 0; i < listCsCostumeCollectionAttr.Count; i++)
            {
                trCostumeCollectionAttr = trCostumeCollectionAttrContent.Find("CostumeCollectionAttr" + i);

                if (trCostumeCollectionAttr == null)
                {
                    trCostumeCollectionAttr = Instantiate(m_goCostumeCollectionAttr, trCostumeCollectionAttrContent).transform;
                    trCostumeCollectionAttr.name = "CostumeCollectionAttr" + i;
                }
                else
                {
                    trCostumeCollectionAttr.gameObject.SetActive(true);
                }

                textAttrName = trCostumeCollectionAttr.Find("Text").GetComponent<Text>();
                textAttrValue = trCostumeCollectionAttr.Find("TextValue").GetComponent<Text>();
                
                CsUIData.Instance.SetFont(textAttrName);
                CsUIData.Instance.SetFont(textAttrValue);

                textAttrName.text = listCsCostumeCollectionAttr[i].Attr.Name;
                textAttrValue.text = listCsCostumeCollectionAttr[i].AttrValue.Value.ToString("#,##0");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostumeCollectionCostumeList()
    {
        if (m_csCostumeCollection == null)
        {
            return;
        }
        else
        {
            Transform trImageCostumeBack = m_trPopupCostumeCollectionList.Find("ImageBackground/ImageCostumeBack");
            Transform trCostumeList = null;
            Transform trCostumeEntry = null;

            List<CsCostumeCollectionEntry> listCsCostumeCollectionEntry = new List<CsCostumeCollectionEntry>(m_csCostumeCollection.CostumeCollectionEntryList);

            int nLineIndex = 0;
            int nCostumeIndex = 0;

            for (int i = 0; i < trImageCostumeBack.childCount; i++)
            {
                trCostumeList = trImageCostumeBack.Find("CostumeList" + i);

                if (trCostumeList == null)
                {
                    continue;
                }
                else
                {
                    for (int j = 0; j < trCostumeList.childCount; j++)
                    {
                        trCostumeEntry = trCostumeList.GetChild(j);
                        trCostumeEntry.gameObject.SetActive(false);
                    }

                    trCostumeList.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < listCsCostumeCollectionEntry.Count; i++)
            {
                nLineIndex = i / 3;
                nCostumeIndex = i % 3;

                trCostumeList = trImageCostumeBack.Find("CostumeList" + nLineIndex);

                if (trCostumeList == null)
                {
                    continue;
                }
                else
                {
                    trCostumeEntry = trCostumeList.Find("ItemSlot" + nCostumeIndex);

                    if (trCostumeEntry == null)
                    {
                        continue;
                    }
                    else
                    {
                        CsItem csItem = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume && a.Value1 == listCsCostumeCollectionEntry[i].Costume.CostumeId);

                        if (csItem == null)
                        {
                            trCostumeEntry.gameObject.SetActive(false);
                        }
                        else
                        {
                            CsUIData.Instance.DisplayItemSlot(trCostumeEntry, csItem, true, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                            trCostumeEntry.gameObject.SetActive(true);
                        }
                    }

                    if (trCostumeList.gameObject.activeSelf)
                    {

                    }
                    else
                    {
                        trCostumeList.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPopupCostumeCollectionIncAttr()
    {
        if (m_goCostumeCollectionIncAttr == null)
        {
            StartCoroutine(LoadPopupCostumeCollectionIncAttr());
        }
        else
        {
            UpdatePopupCostumeCollectionIncAttr();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCostumeCollectionIncAttr()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/CostumeCollectionIncAttr");
        yield return resourceRequest;

        m_goCostumeCollectionIncAttr = (GameObject)resourceRequest.asset;
        UpdatePopupCostumeCollectionIncAttr();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupCostumeCollectionIncAttr()
    {
        Button buttonClose = m_trPopupCostumeCollectionIncAttr.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClosePopupCostumeCollectionIncAttr);

        Transform trImageBackground = m_trPopupCostumeCollectionIncAttr.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A151_TXT_00030");

        Transform trContent = trImageBackground.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(CsCostumeManager.Instance.CostumeCollectionId);

        if (csCostumeCollection == null)
        {
            m_trPopupCostumeCollectionIncAttr.gameObject.SetActive(false);
        }
        else
        {
            Transform trCostumeCollectionIncAttr = null;

            Text textAttrName = null;
            Text textAttrValue = null;

            for (int i = 0; i < csCostumeCollection.CostumeCollectionAttrList.Count; i++)
            {
                trCostumeCollectionIncAttr = trContent.Find("CostumeCollectionIncAttr" + i);

                if (trCostumeCollectionIncAttr == null)
                {
                    trCostumeCollectionIncAttr = Instantiate(m_goCostumeCollectionIncAttr, trContent).transform;
                    trCostumeCollectionIncAttr.name = "CostumeCollectionIncAttr" + i;
                }
                else
                {
                    trCostumeCollectionIncAttr.gameObject.SetActive(true);
                }

                textAttrName = trCostumeCollectionIncAttr.Find("TextAttrName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrName);
                textAttrName.text = csCostumeCollection.CostumeCollectionAttrList[i].Attr.Name;

                textAttrValue = trCostumeCollectionIncAttr.Find("TextAttrValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);
                textAttrValue.text = csCostumeCollection.CostumeCollectionAttrList[i].AttrValue.Value.ToString("#,##0");
            }
        }

        m_trPopupCostumeCollectionIncAttr.gameObject.SetActive(true);
    }
}
