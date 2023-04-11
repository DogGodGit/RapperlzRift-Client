using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-17)
//---------------------------------------------------------------------------------------------------

public class CsMountGearRefine : CsPopupSub
{

    [SerializeField] GameObject m_goToggleMountGear;

    Transform m_trMountGear;
    Transform m_trMaterial;
    Transform m_trRefinePanel;
    Transform m_trMountGearListContent;

    Text m_textNoGearMount;

    Button m_buttonRefine;

    Guid m_guidSelected = Guid.Empty;
    int m_nAttrSelected = 0;

    bool m_bFirst = true;

    bool m_bAllMountGear = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMountGearEquip += OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip += OnEventMountGearUnequip;
        CsGameEventUIToUI.Instance.EventMountGearRefine += OnEventMountGearRefine;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_trItemInfo != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Center);
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMountGearEquip -= OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip -= OnEventMountGearUnequip;
        CsGameEventUIToUI.Instance.EventMountGearRefine -= OnEventMountGearRefine;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    //재강화 클릭
    void OnClickRefine()
    {
        if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountGearRefinementItemId) > 0
            && CsGameData.Instance.MyHeroInfo.MountGearRefinementDailyCount < CsGameData.Instance.MyHeroInfo.VipLevel.MountGearRefinementMaxCount)
        {
            CsCommandEventManager.Instance.SendMountGearRefine(m_guidSelected, m_nAttrSelected);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //재강화 할 속성 변경
    void OnValueChangedAttr(bool bIson, int nSelect)
    {
        if (bIson && m_nAttrSelected != nSelect)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nAttrSelected = nSelect;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //리스트 선택
    void OnValueChangedMountGear(bool bIson, Guid guid)
    {
        if (bIson && m_guidSelected != guid)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_guidSelected = guid;
            DisplayMountGear();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //모든 장비 표시 토글
    void OnValueChangedMountGearTotally(bool bIson, Text text)
    {

        if (bIson)
            text.color = CsUIData.Instance.ColorWhite;
        else
            text.color = CsUIData.Instance.ColorGray;

        m_bAllMountGear = bIson;
        DisplayMountGearList();
    }

    //---------------------------------------------------------------------------------------------------
    //탈것 재강화 아이템 정보
    void OnClickMountItemInfo(CsItem csItem)
    {
        if (csItem != null)
        {
            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(csItem));
            }
            else
            {
                OpenPopupItemInfo(csItem);
            }
        }
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearRefine(Guid guid)
    {
        m_guidSelected = guid;
        DisplayMountGear();
        UpdateRefineButton();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A20_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    //탈것 장착이 들어오면 셋팅을 초기화 한다.
    void OnEventMountGearEquip(Guid guid)
    {
        m_guidSelected = Guid.Empty;
        m_bFirst = true;
        DisplayMountGearList();
        SettingAttrToggle();
    }

    //---------------------------------------------------------------------------------------------------
    //탈것 해제가 들어오면 셋팅을 초기화 한다.
    void OnEventMountGearUnequip(Guid guid)
    {
        m_guidSelected = Guid.Empty;
        m_bFirst = true;
        DisplayMountGearList();
        SettingAttrToggle();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        m_trRefinePanel = transform.Find("RefinePanel");

        Transform trMountGearListPanel = transform.Find("MountGearListPanel");
        m_trMountGearListContent = trMountGearListPanel.Find("ImageBackground/Scroll View/Viewport/Content");
        Toggle toggleMountGearAll = trMountGearListPanel.Find("ToggleMountGearAll").GetComponent<Toggle>();
        Text textMountGearAll = toggleMountGearAll.transform.Find("TextAll").GetComponent<Text>();
        toggleMountGearAll.onValueChanged.RemoveAllListeners();
        toggleMountGearAll.onValueChanged.AddListener((ison) => OnValueChangedMountGearTotally(ison, textMountGearAll));
        toggleMountGearAll.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));
        textMountGearAll.text = CsConfiguration.Instance.GetString("A20_TXT_00001");
        CsUIData.Instance.SetFont(textMountGearAll);

        m_textNoGearMount = trMountGearListPanel.Find("TextNoGearMount").GetComponent<Text>();
        m_textNoGearMount.text = CsConfiguration.Instance.GetString("A19_TXT_00008");
        CsUIData.Instance.SetFont(m_textNoGearMount);

        Transform trItemGrid = m_trRefinePanel.Find("ItemGrid");

        m_trMountGear = trItemGrid.Find("ItemSlotMountGear");
        m_trMaterial = trItemGrid.Find("ItemSlotMaterial");


        m_buttonRefine = m_trRefinePanel.Find("ButtonRefine").GetComponent<Button>();
        m_buttonRefine.onClick.RemoveAllListeners();
        m_buttonRefine.onClick.AddListener(OnClickRefine);
        m_buttonRefine.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textRefine = m_buttonRefine.transform.Find("Text").GetComponent<Text>();
        textRefine.text = CsConfiguration.Instance.GetString("A20_BTN_00001");
        CsUIData.Instance.SetFont(textRefine);

        Text textBattlePoint = m_trRefinePanel.Find("TextGearBattlePoint").GetComponent<Text>();
        textBattlePoint.text = CsConfiguration.Instance.GetString("A20_TXT_00002");
        CsUIData.Instance.SetFont(textBattlePoint);

        Text textSelect = m_trRefinePanel.Find("TextSelect").GetComponent<Text>();
        textSelect.text = CsConfiguration.Instance.GetString("A20_TXT_00003");
        CsUIData.Instance.SetFont(textSelect);

        CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.MountGearRefinementItemId);
        CsUIData.Instance.DisplayItemSlot(m_trMaterial, csItem, false, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Large, false);
        Button buttonItemInfo = m_trMaterial.GetComponent<Button>();
        buttonItemInfo.onClick.RemoveAllListeners();
        buttonItemInfo.onClick.AddListener(() => OnClickMountItemInfo(csItem));
        buttonItemInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textMaterialName = m_trMaterial.Find("TextName").GetComponent<Text>();
        textMaterialName.text = csItem.Name;
        CsUIData.Instance.SetFont(textMaterialName);

        DisplayMountGearList();
        SettingAttrToggle();
        UpdateRefineButton();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMountGearList()
    {
        for (int i = 0; i < m_trMountGearListContent.childCount; ++i)
        {
            m_trMountGearListContent.GetChild(i).gameObject.SetActive(false);
        }

        List<Guid> listEquipHeroMounGear = CsGameData.Instance.MyHeroInfo.EquippedMountGearList;
        List<CsHeroMountGear> listHeroMountGear = CsGameData.Instance.MyHeroInfo.HeroMountGearList;

        List<CsHeroMountGear> listEquipSort = new List<CsHeroMountGear>();
        List<CsHeroMountGear> listUnEquipSort = new List<CsHeroMountGear>();

        //장착한 장비 
        for (int i = 0; i < listEquipHeroMounGear.Count; ++i)
        {
            CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(listEquipHeroMounGear[i]);
            if (csHeroMountGear != null)
            {
                listEquipSort.Add(csHeroMountGear);
            }
        }

        listEquipSort.Sort(SortToEquip);

        for (int i = 0; i < listEquipSort.Count; ++i)
        {
            if (m_bFirst)
            {
                m_bFirst = false;
                m_guidSelected = listEquipSort[i].Id;
                DisplayMountGear();
            }
            CreateMountGear(listEquipSort[i], i, true);
        }

        if (m_bAllMountGear)
        {

            for (int i = 0; i < listHeroMountGear.Count; ++i)
            {
                CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByHeroMountGearId(listHeroMountGear[i].Id);
                if (csInventorySlot != null)
                {
                    listUnEquipSort.Add(csInventorySlot.InventoryObjectMountGear.HeroMountGear);
                }
            }

            listUnEquipSort.Sort(SortToUnEquip);

            for (int i = 0; i < listUnEquipSort.Count; ++i)
            {
                if (m_bFirst)
                {
                    m_bFirst = false;
                    m_guidSelected = listUnEquipSort[i].Id;
                    DisplayMountGear();
                }
                CreateMountGear(listUnEquipSort[i], (i + listEquipSort.Count));
            }
        }
        else
        {
            for (int i = 0; i < listHeroMountGear.Count; ++i)
            {
                if (listHeroMountGear[i].Id == m_guidSelected)
                {
                    CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByHeroMountGearId(listHeroMountGear[i].Id);
                    if (csInventorySlot != null)
                    {
                        listUnEquipSort.Add(csInventorySlot.InventoryObjectMountGear.HeroMountGear);
                        DisplayMountGear();
                        CreateMountGear(listUnEquipSort[0], (i + listEquipSort.Count));
                    }
                    break;
                }
            }
        }

        bool bMountGearCheck = (listEquipSort.Count == 0 && listUnEquipSort.Count == 0) ? true : false;
        m_textNoGearMount.gameObject.SetActive(bMountGearCheck);
        m_trRefinePanel.gameObject.SetActive(!bMountGearCheck);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateMountGear(CsHeroMountGear csHeroMountGear, int nIndex, bool bEquip = false)
    {
        Guid guid = csHeroMountGear.Id;
        Transform trMountGear = m_trMountGearListContent.Find(guid.ToString());

        if (trMountGear == null)
        {
            trMountGear = Instantiate(m_goToggleMountGear, m_trMountGearListContent).transform;
            trMountGear.name = guid.ToString();
        }

        Toggle toggleMountGear = trMountGear.GetComponent<Toggle>();
        toggleMountGear.group = m_trMountGearListContent.GetComponent<ToggleGroup>();
        toggleMountGear.onValueChanged.RemoveAllListeners();
        toggleMountGear.isOn = m_guidSelected == guid ? true : false;
        toggleMountGear.onValueChanged.AddListener((ison) => { OnValueChangedMountGear(ison, guid); });

        Transform trItemSlot = trMountGear.transform.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMountGear);

        Text textName = trMountGear.transform.Find("TextName").GetComponent<Text>();
        textName.text = csHeroMountGear.MountGear.Name;
        CsUIData.Instance.SetFont(textName);

        Text textEquip = trMountGear.transform.Find("TextEquip").GetComponent<Text>();
        textEquip.text = CsConfiguration.Instance.GetString("A20_TXT_00004");
        CsUIData.Instance.SetFont(textEquip);
        textEquip.gameObject.SetActive(bEquip);
        trMountGear.SetSiblingIndex(nIndex);
        trMountGear.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMountGear()
    {
        CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(m_guidSelected);
        CsUIData.Instance.DisplayItemSlot(m_trMountGear, csHeroMountGear, EnItemSlotSize.Large);
        Text textName = m_trMountGear.Find("TextName").GetComponent<Text>();
        textName.text = csHeroMountGear.MountGear.Name;
        CsUIData.Instance.SetFont(textName);

        Text textBattlePointValue = m_trRefinePanel.Find("TextGearBattlePointValue").GetComponent<Text>();
        textBattlePointValue.text = csHeroMountGear.BattlePower.ToString("#,##0");
        CsUIData.Instance.SetFont(textBattlePointValue);

        Transform trMountGearAttrList = m_trRefinePanel.Find("MountGearAttrList");

        List<CsHeroMountGearOptionAttr> listMountAttr = csHeroMountGear.HeroMountGearOptionAttrList;


        for (int i = 0; i < listMountAttr.Count; ++i)
        {
            Transform trGearAtt = trMountGearAttrList.Find("GearAttr" + i);
            Text textAttrName = trGearAtt.Find("TextAttrName").GetComponent<Text>();
            textAttrName.text = string.Format("<color={0}>{1}</color>", listMountAttr[i].MountGearOptionAttrGrade.ColorCode, listMountAttr[i].Attr.Name);
            CsUIData.Instance.SetFont(textAttrName);

            Text textAttrValue = trGearAtt.Find("TextAttrValue").GetComponent<Text>();
            textAttrValue.text = listMountAttr[i].AttrValueInfo.Value.ToString("#,##0");
            CsUIData.Instance.SetFont(textAttrValue);

            if (m_bFirst)
            {
                int nAttr = i;
                Toggle toggleGearAttr = trGearAtt.GetComponent<Toggle>();
                toggleGearAttr.group = trMountGearAttrList.GetComponent<ToggleGroup>();
                toggleGearAttr.onValueChanged.RemoveAllListeners();
                toggleGearAttr.isOn = i == 0 ? true : false;
                toggleGearAttr.onValueChanged.AddListener((ison) => { OnValueChangedAttr(ison, nAttr); });
            }
        }
        UpdateItemCount();
        UpdateDayCount();
    }

    //---------------------------------------------------------------------------------------------------
    void SettingAttrToggle()
    {
        Transform trMountGearAttrList = m_trRefinePanel.Find("MountGearAttrList");

        for (int i = 0; i < CsGameConfig.Instance.MountGearOptionAttrCount; ++i)
        {
            Transform trGearAtt = trMountGearAttrList.Find("GearAttr" + i);
            int nAttr = i;
            Toggle toggleGearAttr = trGearAtt.GetComponent<Toggle>();
            toggleGearAttr.group = trMountGearAttrList.GetComponent<ToggleGroup>();
            toggleGearAttr.onValueChanged.RemoveAllListeners();
            toggleGearAttr.isOn = i == 0 ? true : false;
            toggleGearAttr.onValueChanged.AddListener((ison) => { OnValueChangedAttr(ison, nAttr); });
        }
    }

    //---------------------------------------------------------------------------------------------------
    //재강화 재료 표시
    void UpdateItemCount()
    {
        Text textCount = m_trMaterial.Find("Item/TextCount").GetComponent<Text>();
        int nHeroItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountGearRefinementItemId);
        textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroItemCount, 1);
        CsUIData.Instance.SetFont(textCount);
        Transform trDim = m_trMaterial.Find("ImageCooltime");

        if (nHeroItemCount > 0)
        {
            textCount.color = CsUIData.Instance.ColorWhite;
            trDim.gameObject.SetActive(false);
        }
        else
        {
            textCount.color = CsUIData.Instance.ColorRed;
            trDim.gameObject.SetActive(true);
        }

        CsUIData.Instance.SetFont(textCount);
    }

    //---------------------------------------------------------------------------------------------------
    //재강화 횟수 표시
    void UpdateDayCount()
    {
        Text textDayCount = m_trRefinePanel.Find("TextDayCount").GetComponent<Text>();
        int nCount = CsGameData.Instance.MyHeroInfo.VipLevel.MountGearRefinementMaxCount - CsGameData.Instance.MyHeroInfo.MountGearRefinementDailyCount;

        textDayCount.text = string.Format(CsConfiguration.Instance.GetString("A20_TXT_01001"), nCount, CsGameData.Instance.MyHeroInfo.VipLevel.MountGearRefinementMaxCount);
        CsUIData.Instance.SetFont(textDayCount);
    }

    //재강화 버튼 활성화
    void UpdateRefineButton()
    {
        if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountGearRefinementItemId) > 0
           && CsGameData.Instance.MyHeroInfo.MountGearRefinementDailyCount < CsGameData.Instance.MyHeroInfo.VipLevel.MountGearRefinementMaxCount)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonRefine, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonRefine, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //장착된 장비는 타입별로 정렬
    int SortToEquip(CsHeroMountGear A, CsHeroMountGear B)
    {
        //타입 오름차
        if (A.MountGear.MountGearType.Type > B.MountGear.MountGearType.Type) return 1;
        else if (A.MountGear.MountGearType.Type < B.MountGear.MountGearType.Type) return -1;
        return 0;
    }

    //---------------------------------------------------------------------------------------------------
    //장착되지 않은 장비는 등급,품질,타입 순으로 정렬
    int SortToUnEquip(CsHeroMountGear A, CsHeroMountGear B)
    {
        //등급 내림차
        if (A.MountGear.MountGearGrade.Grade < B.MountGear.MountGearGrade.Grade) return 1;
        else if (A.MountGear.MountGearGrade.Grade > B.MountGear.MountGearGrade.Grade) return -1;
        else
        {
            //품질 내림차
            if (A.MountGear.MountGearQuality.Quality < B.MountGear.MountGearQuality.Quality) return 1;
            else if (A.MountGear.MountGearQuality.Quality > B.MountGear.MountGearQuality.Quality) return -1;
            else
            {
                //타입 오름차
                if (A.MountGear.MountGearType.Type > B.MountGear.MountGearType.Type) return 1;
                else if (A.MountGear.MountGearType.Type < B.MountGear.MountGearType.Type) return -1;
                return 0;
            }
        }
    }

    #region ItemInfo

    GameObject m_goPopupItemInfo;
    Transform m_trItemInfo;
    Transform m_trPopupList;
    CsPopupItemInfo m_csPopupItemInfo;

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
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId), false, -1, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }

    #endregion ItemInfo
}
