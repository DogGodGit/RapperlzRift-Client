using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientCommon;

public class CsWareHouse : CsPopupSub {

    [SerializeField] GameObject m_goItemSlot;
    [SerializeField] GameObject m_goWarehouseSlot;

    Transform m_trBack;
    Transform m_trContent;
    Transform m_trPopupList;
    Transform m_trItemInfo;
    Transform m_trPopupExtend;
    
    GameObject m_goPopupItemInfo;
    GameObject m_goExtendPopup;

    CsPopupItemInfo m_csPopupItemInfo;

    EnSubMenu m_enSubMenu;

    Text m_textWareHouseSlot;

    int m_nLoadCompleteSlotCount;
    int m_nstandardPosition = 0;
    int m_nStartDiaWarehouseIndex;
    int m_nExtendNeedDia;
    int m_nExtendSlotCount;
    int m_nSelectSlotIndex = -1;
    int m_nSelectWarehouseListIndex = -1;
    int m_nLastSelectComposeItem = -1;
    int m_nMaxWarehouseCount;

    bool m_bIsLoad = false;
    bool m_bIsFirst = true;
    bool m_bProcessingButton = false;

    bool m_bPrefabLoad = false;

    List<int> m_listPotionSlot = new List<int>();
    List<int> m_listReturnScrollSlot = new List<int>();

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        CsGameEventUIToUI.Instance.EventWarehouseDeposit += OnEventWarehouseDeposit;
        CsGameEventUIToUI.Instance.EventWarehouseWithdraw += OnEventWarehouseWithdraw;
        CsGameEventUIToUI.Instance.EventWarehouseSlotExtend += OnEventWarehouseSlotExtend;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (CsUIData.Instance.HpPotionRemainingCoolTime - Time.deltaTime > 0)
        {
            for (int i = 0; i < m_listPotionSlot.Count; i++)
            {
                Transform trWarehouseSlot = m_trContent.Find("WarehouseSlot" + m_listPotionSlot[i]);
                Debug.Log(m_listPotionSlot[i]);
                if (trWarehouseSlot != null)
                {
                    Transform trItemSlot = trWarehouseSlot.Find("ItemSlot");

                    if (trItemSlot != null)
                    {
                        Image imageCoolTime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                        imageCoolTime.fillAmount = (CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup) / CsUIData.Instance.HpPotionCoolTime;
                    }
                }
            }
        }

        if (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup > 0)
        {
            for (int i = 0; i < m_listReturnScrollSlot.Count; i++)
            {
                Transform trWarehouseSlot = m_trContent.Find("WarehouseSlot" + m_listReturnScrollSlot[i]);

                if (trWarehouseSlot != null)
                {
                    Transform trItemSlot = trWarehouseSlot.Find("ItemSlot");

                    if (trItemSlot != null)
                    {
                        Image imageCoolTime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                        imageCoolTime.fillAmount = (CsUIData.Instance.ReturnScrollRemainingCoolTime - Time.realtimeSinceStartup) / CsUIData.Instance.ReturnScrollCoolTime;
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bIsFirst)
        {
            m_bIsFirst = false;
            return;
        }
        m_trBack.gameObject.SetActive(true);
        UpdateWarehouseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_trItemInfo != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Left);
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventWarehouseDeposit -= OnEventWarehouseDeposit;
        CsGameEventUIToUI.Instance.EventWarehouseWithdraw -= OnEventWarehouseWithdraw;
        CsGameEventUIToUI.Instance.EventWarehouseSlotExtend -= OnEventWarehouseSlotExtend;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventWarehouseDeposit()
    {
        UpdateWarehouseCountText();
        UpdateWarehouseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarehouseWithdraw()
    {
        UpdateWarehouseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarehouseSlotExtend()
    {
        UpdateWarehouseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingBaitUse()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestScrollUse()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;

        if (enPopupItemInfoPositionType == EnPopupItemInfoPositionType.Left)
        {
            if (m_trBack != null)
            {
                m_trBack.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWarehouseSlot(int nSlotIndex)
    {
        if (m_bProcessingButton)
        {
            return;
        }

        m_bProcessingButton = true;
        m_nSelectWarehouseListIndex = nSlotIndex;

        //int nInventoryLevelMaxCount = CsGameData.Instance.JobLevelMasterList[CsGameData.Instance.JobLevelMasterList.Count - 1].InventorySlotAccCount;

        //인벤토리가 눌렸을때
        if (nSlotIndex < CsGameData.Instance.MyHeroInfo.WarehouseSlotList.Count)
        {
            
            //아이템정보창 보여주기
            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo());
            }
            else
            {
                OpenPopupItemInfo();
            }
        }
        else if (nSlotIndex < m_nMaxWarehouseCount)
        {
            //빈창
            Debug.Log("빈 슬롯입니다");
        }
        else
        {
            m_nStartDiaWarehouseIndex = m_nMaxWarehouseCount;
            m_nExtendSlotCount = nSlotIndex - m_nMaxWarehouseCount + 1;
            m_nExtendNeedDia = 0;
            

            for (int i = 0; i < m_nExtendSlotCount; i++)
            {
                CsWarehouseSlotExtendRecipe csWarehouseSlotExtendRecipe = CsGameData.Instance.GetWarehouseSlotExtendRecipe(CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount + 1 + i);
                //Debug.Log("dia : " + csWarehouseSlotExtendRecipe.Dia + ", Count : " + csWarehouseSlotExtendRecipe.SlotCount);
                if (csWarehouseSlotExtendRecipe != null)
                {
                    m_nExtendNeedDia += csWarehouseSlotExtendRecipe.Dia;
                }
            }

            //다이아개수체크
            if (CsGameData.Instance.MyHeroInfo.Dia >= m_nExtendNeedDia)
            {
                //팝업오픈
                CheckDiaSlot(true);
                OpenExtendPopup();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, string.Format(CsConfiguration.Instance.GetString("A02_TXT_02005"), m_nExtendNeedDia));
            }
        }

        m_bProcessingButton = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExtendSubmit()
    {
        CheckDiaSlot(false);

        if (m_nExtendNeedDia <= CsGameData.Instance.MyHeroInfo.Dia)
        {
            CsCommandEventManager.Instance.SendWarehouseSlotExtend(m_nExtendSlotCount);
        }
        else
        {
            Debug.Log("다이아가 부족합니다");
        }

        ClosePopupExtend();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExtendCancel()
    {
        CheckDiaSlot(false);
        ClosePopupExtend();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedWarehouse()
    {
        RectTransform rectTransform = m_trContent.GetComponent<RectTransform>();

        for (int i = 0; i < m_trContent.childCount; i++)
        {
            m_trContent.GetChild(i).gameObject.SetActive(true);
        }

        if (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount % 5 == 0)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5 * 105) + 5);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5 * 105) + 110);
        }
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_nMaxWarehouseCount = CsGameConfig.Instance.FreeWarehouseSlotCount + CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount;   // 창고 최대 저장 슬롯수 (기본제공 15개 + 다이아로 구매한 창고 슬롯 수)
        m_trBack = transform.Find("ImageBack");
        m_textWareHouseSlot = m_trBack.Find("TextWareHouseSlot").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWareHouseSlot);
        m_textWareHouseSlot.text = String.Format(CsConfiguration.Instance.GetString("A113_TXT_01001"), m_nMaxWarehouseCount - CsGameData.Instance.MyHeroInfo.WarehouseSlotList.Count, m_nMaxWarehouseCount);

        Text textWareHouseInfo = transform.Find("ImageWareHouseInfo").GetComponentInChildren<Text>();
        CsUIData.Instance.SetFont(textWareHouseInfo);
        textWareHouseInfo.text = CsConfiguration.Instance.GetString("A113_TXT_00001");

        CreateBaseWarehouse();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWarehouseCountText()
    {
        m_nMaxWarehouseCount = CsGameConfig.Instance.FreeWarehouseSlotCount + CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount;
        int nCurrentCount = m_nMaxWarehouseCount - CsGameData.Instance.MyHeroInfo.WarehouseSlotList.Count;
        if (nCurrentCount == 0)
        {
            m_textWareHouseSlot.text = string.Format(CsConfiguration.Instance.GetString("A113_TXT_01003"), nCurrentCount);
        }
        else
        {
            m_textWareHouseSlot.text = string.Format(CsConfiguration.Instance.GetString("A113_TXT_01001"), nCurrentCount);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csitem = null, bool bOwned = false)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        if (csitem == null)
        {
            OpenPopupItemInfo();
        }
        else
        {
            OpenPopupItemInfo(csitem, bOwned);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo()
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        
        bool bIsWarehouse = m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.Warehouse;              // 인벤토리를 사용하고 있는 곳이 창고인지 확인

        EnItemLocationType enItemLocationType = bIsWarehouse ? EnItemLocationType.Warehouse : EnItemLocationType.None;

        int nWarehouseObjectIndex = CsGameData.Instance.MyHeroInfo.WarehouseSlotList[m_nSelectWarehouseListIndex].Index;

        switch (CsGameData.Instance.MyHeroInfo.WarehouseSlotList[m_nSelectWarehouseListIndex].EnType)
        {
            case EnWarehouseObjectType.MainGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, CsGameData.Instance.MyHeroInfo.WarehouseSlotList[m_nSelectWarehouseListIndex].WarehouseObjectMainGear.HeroMainGear, false, false, enItemLocationType, nWarehouseObjectIndex);
                break;

            case EnWarehouseObjectType.MountGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, CsGameData.Instance.MyHeroInfo.WarehouseSlotList[m_nSelectWarehouseListIndex].WarehouseObjectMountGear.HeroMountGear, false, false, enItemLocationType, nWarehouseObjectIndex);
                break;

            case EnWarehouseObjectType.SubGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, CsGameData.Instance.MyHeroInfo.WarehouseSlotList[m_nSelectWarehouseListIndex].WarehouseObjectSubGear.HeroSubGear, false, enItemLocationType, nWarehouseObjectIndex);
                break;

            case EnWarehouseObjectType.Item:
                CsWarehouseObjectItem csWarehouseObjectItem = CsGameData.Instance.MyHeroInfo.WarehouseSlotList[m_nSelectWarehouseListIndex].WarehouseObjectItem;
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, csWarehouseObjectItem.Item, csWarehouseObjectItem.Count, csWarehouseObjectItem.Owned, m_nSelectWarehouseListIndex, false, true, enItemLocationType, nWarehouseObjectIndex);
                break;
        }

        m_trBack.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csitem, bool bOwned)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, bOwned, m_nSelectWarehouseListIndex);
    }

    //---------------------------------------------------------------------------------------------------
    public void QuickOpenPopupItemInfo(int nSelectInventoryListIndex)
    {
        m_nSelectWarehouseListIndex = nSelectInventoryListIndex;
        StartCoroutine(LoadPopupItemInfo());
    }

    //---------------------------------------------------------------------------------------------------
    void CreateBaseWarehouse()
    {
        m_nLoadCompleteSlotCount = 0;
        int BaseLoadCount = CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount;           // 기획의도 : 창고의 최대치를 인벤토리의 최대치와 같게.

        m_listPotionSlot.Clear();
        m_listReturnScrollSlot.Clear();

        if (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount < BaseLoadCount)
        {
            BaseLoadCount = CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount;
        }

        m_trContent = m_trBack.Find("Scroll View/Viewport/Content");

        RectTransform rectTransform = m_trContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5 * 105) + 5);

        for (int i = 0; i < BaseLoadCount; i++)
        {
            Transform trWarehouseSlot = m_trContent.Find("WarehouseSlot" + i);
            int nSlotNum = i;

            if (trWarehouseSlot == null)
            {
                CreateWarehouseSlot(nSlotNum);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //스크롤바가 내려가면 로드되지 않은 인벤토리를 로드한다.
    void OnValueChangedWarehouseScrollbar(Scrollbar scrollbar)
    {
        if (!m_bIsLoad)
        {
            m_bIsLoad = true;

            if (m_nLoadCompleteSlotCount < CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount)
            {
                int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, (CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount / 5), (1 - scrollbar.value))); // 최소 최대값 확인 필요.

                if (nUpdateLine > m_nstandardPosition)
                {
                    int nStartCount = m_nLoadCompleteSlotCount;
                    int nEndCount = (nUpdateLine + 5) * 5;

                    if (nEndCount >= CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount)
                    {
                        nEndCount = CsGameData.Instance.MyHeroInfo.InventorySlotMaxCount;
                    }

                    for (int i = nStartCount; i < nEndCount; i++)
                    {
                        int nSlotNum = i;
                        CreateWarehouseSlot(nSlotNum);
                    }

                    m_nstandardPosition = nUpdateLine;
                }
            }

            m_bIsLoad = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //슬롯생성함수
    void CreateWarehouseSlot(int nSlotIndex)
    {
        Transform trSlot = m_trContent.Find("WarehouseSlot" + nSlotIndex);

        if (trSlot == null)
        {
            GameObject goWarehouseSlot = Instantiate(m_goWarehouseSlot, m_trContent);
            goWarehouseSlot.name = "WarehouseSlot" + nSlotIndex;
            trSlot = goWarehouseSlot.transform;
            m_nLoadCompleteSlotCount++;

            Button buttonWarehouseSlot = trSlot.GetComponent<Button>();
            buttonWarehouseSlot.onClick.RemoveAllListeners();
            buttonWarehouseSlot.onClick.AddListener(() => OnClickWarehouseSlot(nSlotIndex));
            buttonWarehouseSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        UpdateWarehouseSlot(nSlotIndex);
    }

    //---------------------------------------------------------------------------------------------------
    //슬롯 전체 업데이트 함수
    void UpdateWarehouseAll()
    {
        m_listPotionSlot.Clear();
        m_listReturnScrollSlot.Clear();

        for (int i = 0; i < m_nLoadCompleteSlotCount; i++)
        {
            UpdateWarehouseSlot(i);
        }

        OnValueChangedWarehouse();
        UpdateWarehouseCountText();
    }

    //---------------------------------------------------------------------------------------------------
    //슬롯업데이트 함수
    void UpdateWarehouseSlot(int nSlotIndex)
    {
        Transform trWarehouseSlot = m_trContent.Find("WarehouseSlot" + nSlotIndex);

        if (trWarehouseSlot == null)
        {
            return;
        }

        Transform TrCheck = trWarehouseSlot.Find("ImageCheck");

        if (TrCheck.gameObject.activeSelf)
        {
            TrCheck.gameObject.SetActive(false);
        }

        if (CsGameData.Instance.MyHeroInfo.WarehouseSlotList.Count > nSlotIndex)
        {
            //아이템이 있는 슬롯
            Transform trItemSlot = trWarehouseSlot.Find("ItemSlot");

            if (trItemSlot == null)
            {
                GameObject goItemSlot = Instantiate(m_goItemSlot, trWarehouseSlot);
                goItemSlot.name = "ItemSlot";
                trItemSlot = goItemSlot.transform;

                goItemSlot.GetComponent<Button>().enabled = false;
            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            switch (CsGameData.Instance.MyHeroInfo.WarehouseSlotList[nSlotIndex].EnType)
            {
                case EnWarehouseObjectType.MainGear:
                    CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.WarehouseSlotList[nSlotIndex].WarehouseObjectMainGear.HeroMainGear;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGear);
                    break;

                case EnWarehouseObjectType.MountGear:
                    CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.WarehouseSlotList[nSlotIndex].WarehouseObjectMountGear.HeroMountGear;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMountGear);
                    break;

                case EnWarehouseObjectType.SubGear:
                    CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.WarehouseSlotList[nSlotIndex].WarehouseObjectSubGear.HeroSubGear;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroSubGear);
                    break;

                case EnWarehouseObjectType.Item:
                    CsWarehouseObjectItem csWarehouseObjectItem = CsGameData.Instance.MyHeroInfo.WarehouseSlotList[nSlotIndex].WarehouseObjectItem;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csWarehouseObjectItem.Item, csWarehouseObjectItem.Owned, csWarehouseObjectItem.Count);

                    switch (CsGameData.Instance.MyHeroInfo.WarehouseSlotList[nSlotIndex].WarehouseObjectItem.Item.ItemType.EnItemType)
                    {
                        case EnItemType.HpPotion:
                            m_listPotionSlot.Add(nSlotIndex);
                            break;
                        case EnItemType.ReturnScroll:
                            m_listReturnScrollSlot.Add(nSlotIndex);
                            break;
                    }
                    break;
            }

            //선택슬롯표시
            Image imageCooltime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
            imageCooltime.fillAmount = 0;
            
        }
        else
        {
            //아이템이 없거나 잠긴슬롯
            Transform trCheck = trWarehouseSlot.Find("ImageCheck");
            Transform trLock = trWarehouseSlot.Find("ImageLock");

            trCheck.gameObject.SetActive(false);

            Text textRequiredLevel = trWarehouseSlot.Find("TextLevel").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRequiredLevel);

            if (nSlotIndex < CsGameConfig.Instance.FreeWarehouseSlotCount + CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount)
            {
                //빈슬롯
                trLock.gameObject.SetActive(false);
                textRequiredLevel.text = "";

                Transform trItemSlot = trWarehouseSlot.Find("ItemSlot");

                if (trItemSlot != null)
                {
                    trItemSlot.gameObject.SetActive(false);
                }
            }
            else
            {
                trLock.gameObject.SetActive(true);
                textRequiredLevel.text = "";
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenExtendPopup()
    {
        if (m_goExtendPopup == null)
        {
            StartCoroutine(LoadExtendPopupCoroutine());
        }
        else
        {
            CreatePopupExtend();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadExtendPopupCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/PopupInventoryExtend");
        yield return resourceRequest;
        m_goExtendPopup = (GameObject)resourceRequest.asset;

        CreatePopupExtend();
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupExtend()
    {
        GameObject goExtendPopup = Instantiate(m_goExtendPopup, m_trPopupList);
        goExtendPopup.name = "PopupWarehouseExtend";

        m_trPopupExtend = goExtendPopup.transform;

        Transform trBack = goExtendPopup.transform.Find("ImageBackground");

        Text textMessage = trBack.Find("TextMessage").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMessage);
        textMessage.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01001"), m_nExtendNeedDia, m_nExtendSlotCount);

        Text textWarning = trBack.Find("TextWarning").GetComponent<Text>();
        CsUIData.Instance.SetFont(textWarning);
        textWarning.text = CsConfiguration.Instance.GetString("A02_TXT_00010");

        Transform trButtonList = trBack.Find("Buttons");

        Button buttonOk = trButtonList.Find("Button1").GetComponent<Button>();
        buttonOk.onClick.RemoveAllListeners();
        buttonOk.onClick.AddListener(OnClickExtendSubmit);
        buttonOk.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textOk = buttonOk.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOk);
        textOk.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");


        Button buttonNo = trButtonList.Find("Button2").GetComponent<Button>();
        buttonNo.onClick.RemoveAllListeners();
        buttonNo.onClick.AddListener(OnClickExtendCancel);
        buttonNo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonNo.gameObject.SetActive(true);


        Text textNo = buttonNo.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNo);
        textNo.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupExtend()
    {
        if (m_trPopupExtend != null)
        {
            Destroy(m_trPopupExtend.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckDiaSlot(bool bIsCheck)
    {
        for (int i = m_nStartDiaWarehouseIndex; i <= m_nSelectWarehouseListIndex; i++)
        {
            Transform trWarehouseSlot = m_trContent.Find("WarehouseSlot" + i);

            Transform trCheck = trWarehouseSlot.Find("ImageCheck");
            trCheck.gameObject.SetActive(bIsCheck);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWarehouseSlotByWarehouseIndex(int nWarehouseIndex, bool bSelect)
    {
        int nListIndex = CsGameData.Instance.MyHeroInfo.WarehouseSlotList.FindIndex(a => a.Index == nWarehouseIndex);

        UpdateWarehouseSlotSelected(nListIndex, bSelect);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWarehouseSlotSelected(int nWarehouseListIndex, bool bSelect)
    {
        Transform trWarehouseSlot = m_trContent.Find("WarehouseSlot" + nWarehouseListIndex);
        if (trWarehouseSlot != null)
        {
            Transform trItemSlot = trWarehouseSlot.Find("ItemSlot");
            Transform trCheck = trItemSlot.Find("ImageCheck");

            Button buttonWarehouseSlot = trWarehouseSlot.GetComponent<Button>();

            if (bSelect)
            {
                //선택이되면
                trCheck.gameObject.SetActive(true);
                buttonWarehouseSlot.interactable = false;
            }
            else
            {
                //
                trCheck.gameObject.SetActive(false);
                buttonWarehouseSlot.interactable = true;
            }
        }
    }
}
