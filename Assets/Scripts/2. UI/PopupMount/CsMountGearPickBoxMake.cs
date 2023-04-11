using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-17)
//---------------------------------------------------------------------------------------------------

public class CsMountGearPickBoxMake : CsPopupSub
{

    [SerializeField] GameObject m_goToggleMountPickBoxMake;

    Transform m_trMakePanel;
    Transform m_trMountGearPickBox;
    Transform m_trMakeListContent;
    Transform[] m_atrRecipeMaterial = new Transform[4];

    ScrollRect m_scrollMakeList;

    Text m_textGold;
    Text m_textNoMakeList;
    Text m_textMakePossibleList;

    Button m_buttonMake;
    Button m_buttonMakeTotally;
    bool m_bMakePossibleList = false;
    bool m_bFirst = true;

    int m_nMountGearPickBoxItemId = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMountGearPickBoxMake += OnEventMountGearPickBoxMake;
        CsGameEventUIToUI.Instance.EventMountGearPickBoxMakeTotally += OnEventMountGearPickBoxMakeTotally;
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
        CsGameEventUIToUI.Instance.EventMountGearPickBoxMake -= OnEventMountGearPickBoxMake;
        CsGameEventUIToUI.Instance.EventMountGearPickBoxMakeTotally -= OnEventMountGearPickBoxMakeTotally;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickMountGearMake()
    {
        //해당 아이템이 인벤토리에 없는 경우
        if (CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(m_nMountGearPickBoxItemId) == null)
        {
            //인벤토리 슬롯 자리가 없는 경우
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A21_ERROR_00106"));
                return;
            }
        }

        if (CheckRecip(CsGameData.Instance.GetMountGearPickBoxRecipe(m_nMountGearPickBoxItemId)))
        {
            CsCommandEventManager.Instance.SendMountGearPickBoxMake(m_nMountGearPickBoxItemId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMountGearMakeTotally()
    {
        //해당 아이템이 인벤토리에 없는 경우
        if (CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(m_nMountGearPickBoxItemId) == null)
        {
            //인벤토리 슬롯 자리가 없는 경우
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A21_ERROR_00106"));
                return;
            }
        }

        if (CheckRecip(CsGameData.Instance.GetMountGearPickBoxRecipe(m_nMountGearPickBoxItemId)))
        {
            CsCommandEventManager.Instance.SendMountGearPickBoxMakeTotally(m_nMountGearPickBoxItemId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMakePossibleList(bool bIson)
    {
        m_bFirst = true;
        m_bMakePossibleList = bIson;
        DisplayList();
        DisplayMountGearPickBox();
        m_textMakePossibleList.color = bIson ? CsUIData.Instance.ColorWhite : CsUIData.Instance.ColorGray;
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMountGearPickBox(bool bIson, int nItemId)
    {
        if (bIson && m_nMountGearPickBoxItemId != nItemId)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nMountGearPickBoxItemId = nItemId;
            DisplayMountGearPickBox();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //아이템 정보
    void OnClickItemInfo(CsItem csItem)
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
    void OnEventMountGearPickBoxMake()
    {
        if (m_bMakePossibleList)
        {
            if (!CheckRecip(CsGameData.Instance.GetMountGearPickBoxRecipe(m_nMountGearPickBoxItemId)))
            {
                m_bFirst = true;
            }
        }

        DisplayList();
        DisplayMountGearPickBox();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A21_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearPickBoxMakeTotally()
    {
        if (m_bMakePossibleList)
        {
            if (!CheckRecip(CsGameData.Instance.GetMountGearPickBoxRecipe(m_nMountGearPickBoxItemId)))
            {
                m_bFirst = true;
            }
        }

        DisplayList();
        DisplayMountGearPickBox();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A21_TXT_02002"));
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        m_trMakePanel = transform.Find("MakePanel");
        m_trMountGearPickBox = m_trMakePanel.Find("ImageBackground/ItemSlotMountGearPickBox");

        Text textOneMake = m_trMakePanel.Find("ImageGoldBackground/TextOneMake").GetComponent<Text>();
        textOneMake.text = CsConfiguration.Instance.GetString("A21_TXT_00002");
        CsUIData.Instance.SetFont(textOneMake);

        m_textGold = m_trMakePanel.Find("ImageGoldBackground/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGold);

        m_buttonMake = m_trMakePanel.Find("ButtonMake").GetComponent<Button>();
        m_buttonMake.onClick.RemoveAllListeners();
        m_buttonMake.onClick.AddListener(OnClickMountGearMake);
        m_buttonMake.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMake = m_buttonMake.transform.Find("Text").GetComponent<Text>();
        textButtonMake.text = CsConfiguration.Instance.GetString("A21_BTN_00001");
        CsUIData.Instance.SetFont(textButtonMake);

        m_buttonMakeTotally = m_trMakePanel.Find("ButtonMakeTotally").GetComponent<Button>();
        m_buttonMakeTotally.onClick.RemoveAllListeners();
        m_buttonMakeTotally.onClick.AddListener(OnClickMountGearMakeTotally);
        m_buttonMakeTotally.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMakeTotally = m_buttonMakeTotally.transform.Find("Text").GetComponent<Text>();
        textButtonMakeTotally.text = CsConfiguration.Instance.GetString("A21_BTN_00002");
        CsUIData.Instance.SetFont(textButtonMakeTotally);

        m_trMakeListContent = transform.Find("MakeListPanel/ImageBackground/Scroll View/Viewport/Content");

        Toggle toggleMakePossibleList = transform.Find("MakeListPanel/ToggleMakePossibleList").GetComponent<Toggle>();
        toggleMakePossibleList.onValueChanged.RemoveAllListeners();
        toggleMakePossibleList.onValueChanged.AddListener(OnValueChangedMakePossibleList);
        toggleMakePossibleList.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        m_textMakePossibleList = toggleMakePossibleList.transform.Find("Text").GetComponent<Text>();
        m_textMakePossibleList.text = CsConfiguration.Instance.GetString("A21_TXT_00001");
        CsUIData.Instance.SetFont(m_textMakePossibleList);

        m_textNoMakeList = transform.Find("MakeListPanel/TextNoMakeList").GetComponent<Text>();
        m_textNoMakeList.text = CsConfiguration.Instance.GetString("A21_TXT_00003");
        CsUIData.Instance.SetFont(m_textNoMakeList);

        for (int i = 0; i < 4; ++i)
        {
            m_atrRecipeMaterial[i] = m_trMakePanel.Find("ImageBackground/ItemSlotMaterial" + i);
        }

        DisplayList();
        DisplayMountGearPickBox();

    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMountGearPickBox()
    {
        if (m_nMountGearPickBoxItemId == 0)
        {
            if (m_trMakePanel.gameObject.activeSelf)
                m_trMakePanel.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (!m_trMakePanel.gameObject.activeSelf)
                m_trMakePanel.gameObject.SetActive(true);
        }

        CsMountGearPickBoxRecipe csMountGearPickBoxRecipe = CsGameData.Instance.GetMountGearPickBoxRecipe(m_nMountGearPickBoxItemId);

        CsUIData.Instance.DisplayItemSlot(m_trMountGearPickBox, csMountGearPickBoxRecipe.Item, csMountGearPickBoxRecipe.Owned, 0, csMountGearPickBoxRecipe.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
        Button buttonItemInfo = m_trMountGearPickBox.GetComponent<Button>();
        buttonItemInfo.onClick.RemoveAllListeners();
        buttonItemInfo.onClick.AddListener(() => OnClickItemInfo(csMountGearPickBoxRecipe.Item));
        buttonItemInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trMountGearPickBox.Find("Item/ImageNotice").gameObject.SetActive(false);

        MaterialSetting(m_atrRecipeMaterial[0], csMountGearPickBoxRecipe.MaterialItem1, csMountGearPickBoxRecipe.MaterialItem1Count);
        MaterialSetting(m_atrRecipeMaterial[1], csMountGearPickBoxRecipe.MaterialItem2, csMountGearPickBoxRecipe.MaterialItem2Count);
        MaterialSetting(m_atrRecipeMaterial[2], csMountGearPickBoxRecipe.MaterialItem3, csMountGearPickBoxRecipe.MaterialItem3Count);
        MaterialSetting(m_atrRecipeMaterial[3], csMountGearPickBoxRecipe.MaterialItem4, csMountGearPickBoxRecipe.MaterialItem4Count);

        m_textGold.color = (CsGameData.Instance.MyHeroInfo.Gold >= csMountGearPickBoxRecipe.Gold) ? CsUIData.Instance.ColorWhite : CsUIData.Instance.ColorRed;
        m_textGold.text = csMountGearPickBoxRecipe.Gold.ToString("#,##0");

        //조건에 따른 버튼 변환
        if (CheckRecip(csMountGearPickBoxRecipe))
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonMake, true);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonMakeTotally, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonMake, false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonMakeTotally, false);
        }
    }

    void DisplayList()
    {
        List<CsMountGearPickBoxRecipe> listRecipe = CsGameData.Instance.MountGearPickBoxRecipeList;

        if (m_bFirst)
            listRecipe.Sort(SortToRecipe);

        for (int i = 0; i < m_trMakeListContent.childCount; ++i)
        {
            m_trMakeListContent.GetChild(i).gameObject.SetActive(false);
            Toggle togglePickBoxRecipe = m_trMakeListContent.GetChild(i).GetComponent<Toggle>();
            togglePickBoxRecipe.onValueChanged.RemoveAllListeners();
            togglePickBoxRecipe.isOn = false;
        }


        if (m_bMakePossibleList)
        {
            bool bMake = false;

            for (int i = 0; i < listRecipe.Count; ++i)
            {
                if (CheckRecip(listRecipe[i]))
                {
                    bMake = true;
                    CreatePickBoxRecipe(listRecipe[i]);
                }
            }

            if (bMake)
            {
                m_textNoMakeList.gameObject.SetActive(false);
            }
            else
            {
                m_nMountGearPickBoxItemId = 0;
                m_textNoMakeList.gameObject.SetActive(true);
            }
        }
        else
        {
            m_textNoMakeList.gameObject.SetActive(false);

            for (int i = 0; i < listRecipe.Count; ++i)
            {
                CreatePickBoxRecipe(listRecipe[i]);
            }
        }

    }

    //---------------------------------------------------------------------------------------------------
    void CreatePickBoxRecipe(CsMountGearPickBoxRecipe csMountGearPickBoxRecipe)
    {
        bool bMake = CsGameData.Instance.MyHeroInfo.Level >= csMountGearPickBoxRecipe.RequiredHeroLevel;

        Transform trPickBoxRecipe = m_trMakeListContent.Find(csMountGearPickBoxRecipe.Item.ItemId.ToString());

        if (trPickBoxRecipe == null)
        {
            trPickBoxRecipe = Instantiate(m_goToggleMountPickBoxMake, m_trMakeListContent).transform;
            trPickBoxRecipe.name = csMountGearPickBoxRecipe.Item.ItemId.ToString();
        }

        Toggle togglePickBoxRecipe = trPickBoxRecipe.GetComponent<Toggle>();
        togglePickBoxRecipe.group = m_trMakeListContent.GetComponent<ToggleGroup>();
        int nItemid = csMountGearPickBoxRecipe.Item.ItemId;

        if (m_bFirst)
        {
            m_nMountGearPickBoxItemId = nItemid;
            togglePickBoxRecipe.isOn = true;
            m_bFirst = false;
        }
        else
        {
            if (m_nMountGearPickBoxItemId == nItemid)
                togglePickBoxRecipe.isOn = true;
        }

        togglePickBoxRecipe.onValueChanged.AddListener((ison) => { OnValueChangedMountGearPickBox(ison, nItemid); });

        Transform trItemSlot = trPickBoxRecipe.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csMountGearPickBoxRecipe.Item, csMountGearPickBoxRecipe.Owned, 0, csMountGearPickBoxRecipe.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

        Text textLevel = trPickBoxRecipe.Find("TextLevel").GetComponent<Text>();
        textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csMountGearPickBoxRecipe.RequiredHeroLevel);
        CsUIData.Instance.SetFont(textLevel);

        Text textName = trPickBoxRecipe.Find("TextName").GetComponent<Text>();
        textName.text = string.Format("<color={0}>{1}</color>", csMountGearPickBoxRecipe.Item.ItemGrade.ColorCode, csMountGearPickBoxRecipe.Item.Name);
        CsUIData.Instance.SetFont(textName);

        Transform trNoMake = trPickBoxRecipe.Find("ImageNoMake");
        trNoMake.gameObject.SetActive(!bMake);

        //제작가능하고 레시피도 충족할경우 레드닷 표시
        if (bMake)
        {
            if (CheckRecip(csMountGearPickBoxRecipe))
            {
                trItemSlot.Find("Item/ImageNotice").gameObject.SetActive(true);
                trPickBoxRecipe.gameObject.SetActive(true);
            }
            else
            {
                trItemSlot.Find("Item/ImageNotice").gameObject.SetActive(false);
                trPickBoxRecipe.gameObject.SetActive(!m_bMakePossibleList);
            }
        }
        else
        {
            trItemSlot.Find("Item/ImageNotice").gameObject.SetActive(false);
            trPickBoxRecipe.gameObject.SetActive(!m_bMakePossibleList);
        }
    }

    //---------------------------------------------------------------------------------------------------
    int SortToRecipe(CsMountGearPickBoxRecipe A, CsMountGearPickBoxRecipe B)
    {
        //레벨 제한 확인
        bool bA = CsGameData.Instance.MyHeroInfo.Level >= A.RequiredHeroLevel ? true : false;
        bool bB = CsGameData.Instance.MyHeroInfo.Level >= B.RequiredHeroLevel ? true : false;
        //두개다 제작 가능한 레벨이면
        if (bA && bB)
        {
            //내림차순으로 정렬
            if (A.RequiredHeroLevel < B.RequiredHeroLevel) return 1;
            else if (A.RequiredHeroLevel > B.RequiredHeroLevel) return -1;
            return 0;
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
            if (A.RequiredHeroLevel < B.RequiredHeroLevel) return 1;
            else if (A.RequiredHeroLevel > B.RequiredHeroLevel) return -1;
            return 0;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void MaterialSetting(Transform trSlot, CsItem csItem, int nCount)
    {
        CsUIData.Instance.DisplayItemSlot(trSlot, CsGameData.Instance.GetItem(csItem.ItemId), false, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

        Button buttonItemInfo = trSlot.GetComponent<Button>();
        buttonItemInfo.onClick.RemoveAllListeners();
        buttonItemInfo.onClick.AddListener(() => OnClickItemInfo(csItem));
        buttonItemInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        int nHeroItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);

        Text textItemCount = trSlot.Find("Item/TextCount").GetComponent<Text>();
        textItemCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroItemCount, nCount);
        CsUIData.Instance.SetFont(textItemCount);

        Text textName = trSlot.Find("TextName").GetComponent<Text>();
        textName.text = csItem.Name;
        CsUIData.Instance.SetFont(textName);

        Transform trDim = trSlot.Find("ImageCooltime");

        if (nHeroItemCount >= nCount)
        {
            textItemCount.color = CsUIData.Instance.ColorWhite;
            trDim.gameObject.SetActive(false);
        }
        else
        {
            textItemCount.color = CsUIData.Instance.ColorRed;
            trDim.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //제작 조건 체크
    bool CheckRecip(CsMountGearPickBoxRecipe csMountGearPickBoxRecipe)
    {
        if (CsGameData.Instance.MyHeroInfo.Level >= csMountGearPickBoxRecipe.RequiredHeroLevel
            && CsGameData.Instance.MyHeroInfo.Gold >= csMountGearPickBoxRecipe.Gold
            && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem1.ItemId) >= csMountGearPickBoxRecipe.MaterialItem1Count
            && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem2.ItemId) >= csMountGearPickBoxRecipe.MaterialItem2Count
            && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem3.ItemId) >= csMountGearPickBoxRecipe.MaterialItem3Count
            && CsGameData.Instance.MyHeroInfo.GetItemCount(csMountGearPickBoxRecipe.MaterialItem4.ItemId) >= csMountGearPickBoxRecipe.MaterialItem4Count)
        {
            return true;
        }
        else
        {
            return false;
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
