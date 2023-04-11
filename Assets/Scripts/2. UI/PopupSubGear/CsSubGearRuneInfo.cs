using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-17)
//---------------------------------------------------------------------------------------------------

public class CsSubGearRuneInfo : CsPopupSub
{
    [SerializeField] GameObject m_goToggleRuneItem;

    Transform m_trSubGearSlot;
    Transform m_trRuneSocketList;
    Transform m_trRuneItemListContent;
    Transform m_trNomaterial;
    Transform m_trFillSocket;

    Text m_textLock;
    Text m_textEmpty;

    Button m_buttonMount;
    Button m_buttonUnMount;

    int m_nPrevSubGearId;
    int m_nRuneSocketIndex;
    int m_nItemId;

    bool m_bNoSubGear = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventSubGearSelected += OnEventSubGearSelected;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bNoSubGear)
        {
            gameObject.SetActive(false);
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
        CsGameEventUIToUI.Instance.EventSubGearSelected -= OnEventSubGearSelected;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearSelected(int nSubGearId)
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu != EnSubMenu.SubGearRune)
        {
            m_nRuneSocketIndex = 0;
        }

        // 이전의 서브기어와 같다면
        if (m_nPrevSubGearId == CsUIData.Instance.SubGearId)
        {
            //
        }
        else
        {
            m_nPrevSubGearId = CsUIData.Instance.SubGearId;
            m_nRuneSocketIndex = 0;
        }

        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        UpdateSubGearRuneSocketList(csHeroSubGear);
        UpdateRuneItemList(csHeroSubGear);
        UpdateFrameBottom();
    }

    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickRuneMount()
    {
        // 선택된 룬 소켓에 선택된 룬 아이템을 장착
        CsCommandEventManager.Instance.SendRuneSocketMount(CsUIData.Instance.SubGearId, m_nRuneSocketIndex, m_nItemId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRuneUnMount()
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);
        CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = null;

        for (int i = 0; i < csHeroSubGear.RuneSocketList.Count; i++)
        {
            // 장착한 룬의 소켓 인덱스가 선택한 룬 소켓 인덱스와 같을 때
            if (csHeroSubGear.RuneSocketList[i].Index == m_nRuneSocketIndex)
            {
                csHeroSubGearRuneSocket = csHeroSubGear.RuneSocketList[i];
                break;
            }

            csHeroSubGearRuneSocket = null;
        }

        if (csHeroSubGearRuneSocket == null)
        {

        }
        else
        {
            // 인벤토리 슬롯이 부족할 경우
            if (CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(csHeroSubGearRuneSocket.Item.ItemId) == null)
            {
                if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A10_TXT_02001"));
                    return;
                }
            }

            CsCommandEventManager.Instance.SendRuneSocketUnmount(CsUIData.Instance.SubGearId, m_nRuneSocketIndex, csHeroSubGearRuneSocket.Item.ItemId);
        }
    }

    // 오른쪽 룬 아이템 리스트 이벤트
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedRuneItem(bool bIson, int nItemId)
    {
        if (bIson)
        {
            if (m_nItemId == nItemId)
            {
                return;
            }

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nItemId = nItemId;
            UpdateFrameBottom();
        }
    }

    // 룬 소켓 클릭 이벤트
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedRuneSocket(bool bIson, int nSocketIndex)
    {
        if (bIson)
        {
            if (m_nRuneSocketIndex == nSocketIndex)
            {
                return;
            }

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nRuneSocketIndex = nSocketIndex;
            UpdateRuneItemList(CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId));
            UpdateFrameBottom();
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trImageSocketBack = transform.Find("ImageSocketBack");

        m_trSubGearSlot = trImageSocketBack.Find("ItemSlot");
        m_trRuneSocketList = trImageSocketBack.Find("SocketList");

        Transform trRuneListBackground = transform.Find("RuneList/ImageBackground");

        m_trRuneItemListContent = trRuneListBackground.Find("Scroll View/Viewport/Content");
        m_trNomaterial = trRuneListBackground.Find("Nomaterial");

        Text textNoRune = m_trNomaterial.Find("TextNoRune").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoRune);
        textNoRune.text = CsConfiguration.Instance.GetString("A11_TXT_00001");

        Transform trBotFrame = transform.Find("BotFrame");

        m_trFillSocket = trBotFrame.Find("FillSocket");

        m_buttonMount = m_trFillSocket.Find("ButtonMount").GetComponent<Button>();
        m_buttonMount.onClick.RemoveAllListeners();
        m_buttonMount.onClick.AddListener(OnClickRuneMount);
        m_buttonMount.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMount = m_buttonMount.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonMount);
        textButtonMount.text = CsConfiguration.Instance.GetString("A11_BTN_00001");

        Transform trShortCutList = m_trNomaterial.Find("ShortCutList");

        Button buttonShortCut = trShortCutList.Find("ButtonShortCut").GetComponent<Button>();
        buttonShortCut.onClick.RemoveAllListeners();
        buttonShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        //buttonShortCut.onClick.AddListener();

        Text textButtonShortCut = buttonShortCut.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonShortCut);
        textButtonShortCut.text = CsConfiguration.Instance.GetString("A10_BTN_00005");

        m_buttonUnMount = m_trFillSocket.Find("ButtonUnMount").GetComponent<Button>();
        m_buttonUnMount.onClick.RemoveAllListeners();
        m_buttonUnMount.onClick.AddListener(OnClickRuneUnMount);
        m_buttonUnMount.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonUnMount = m_buttonUnMount.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonUnMount);
        textButtonUnMount.text = CsConfiguration.Instance.GetString("A11_BTN_00002");

        m_textEmpty = trBotFrame.Find("TextEmpty").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEmpty);

        m_textLock = trBotFrame.Find("TextLock").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLock);

        FirstSubGearCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void FirstSubGearCheck()
    {
        bool bSubGearCheck = false;
        List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;

        for (int i = 0; i < listHeroSubGear.Count; i++)
        {
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }
            else
            {
                if (CsUIData.Instance.SubGearId == listHeroSubGear[i].SubGear.SubGearId)
                {
                    m_nPrevSubGearId = listHeroSubGear[i].SubGear.SubGearId;

                    m_nRuneSocketIndex = 0;

                    UpdateSubGearRuneSocketList(listHeroSubGear[i]);
                    UpdateRuneItemList(CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId));
                    UpdateFrameBottom();

                    bSubGearCheck = true;

                    break;
                }
            }
        }

        if (!bSubGearCheck)
        {
            m_bNoSubGear = true;
            gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSubGearRuneSocketList(CsHeroSubGear csHeroSubGear)
    {
        CsUIData.Instance.DisplayItemSlot(m_trSubGearSlot, csHeroSubGear);

        for (int i = 0; i < csHeroSubGear.SubGear.SubGearRuneSocketList.Count; i++)
        {
            int nSocketIndex = csHeroSubGear.SubGear.SubGearRuneSocketList[i].SocketIndex;
            Transform trToggleSocket = m_trRuneSocketList.Find("ToggleSocket" + nSocketIndex);

            Image imageBack = trToggleSocket.Find("Background").GetComponent<Image>();
            imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSubGear/" + csHeroSubGear.SubGear.SubGearRuneSocketList[i].BackgroundImageName);

            Image imageIcon = trToggleSocket.Find("ImageIcon").GetComponent<Image>();
            imageIcon.gameObject.SetActive(false);

            Toggle toggleSocket = trToggleSocket.GetComponent<Toggle>();
            toggleSocket.onValueChanged.RemoveAllListeners();

            Transform trImageLock = trToggleSocket.Find("ImageLock");

            if (csHeroSubGear.SubGear.SubGearRuneSocketList[i].RequiredSubGearLevel <= csHeroSubGear.SubGearLevel.Level)
            {
                trImageLock.gameObject.SetActive(false);
            }
            else
            {
                trImageLock.gameObject.SetActive(true);
            }

            if (nSocketIndex == m_nRuneSocketIndex)
            {
                toggleSocket.isOn = true;
            }
            else
            {
                toggleSocket.isOn = false;
            }

            toggleSocket.onValueChanged.AddListener((ison) => OnValueChangedRuneSocket(ison, nSocketIndex));
        }

        // 장착된 룬 리스트
        for (int i = 0; i < csHeroSubGear.RuneSocketList.Count; i++)
        {
            Transform trToggleSocket = m_trRuneSocketList.Find("ToggleSocket" + csHeroSubGear.RuneSocketList[i].Index);

            Image imageIcon = trToggleSocket.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csHeroSubGear.RuneSocketList[i].Item.ItemId);
            imageIcon.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRuneItemList(CsHeroSubGear csHeroSubGear)
    {
        List<CsInventorySlot> listInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList;
        bool bRuneCheck = false;
        m_nItemId = 0;

        for (int i = 0; i < m_trRuneItemListContent.childCount; i++)
        {
            Transform trRuneItem = m_trRuneItemListContent.GetChild(i);

            Toggle toggleRuneItem = trRuneItem.GetComponent<Toggle>();
            toggleRuneItem.onValueChanged.RemoveAllListeners();
            toggleRuneItem.isOn = false;

            trRuneItem.gameObject.SetActive(false);
        }

        List<int> listItemId = new List<int>();

        for (int i = 0; i < listInventorySlot.Count; i++)
        {
            if (listInventorySlot[i].EnType == EnInventoryObjectType.Item)
            {
                for (int j = 0; j < csHeroSubGear.SubGear.SubGearRuneSocketList[m_nRuneSocketIndex].SubGearRuneSocketAvailableItemTypeList.Count; j++)
                {
                    if (listInventorySlot[i].InventoryObjectItem.Item.ItemType.ItemType == csHeroSubGear.SubGear.SubGearRuneSocketList[m_nRuneSocketIndex].SubGearRuneSocketAvailableItemTypeList[j].ItemType)
                    {
                        int nItemId = listInventorySlot[i].InventoryObjectItem.Item.ItemId;
                        listItemId.Add(nItemId);
                        Transform trRuneItem = CreateRuneItem(listInventorySlot[i].InventoryObjectItem);
                        Toggle toggleRuneItem = trRuneItem.GetComponent<Toggle>();
                        toggleRuneItem.onValueChanged.RemoveAllListeners();
                        toggleRuneItem.isOn = false;
                        toggleRuneItem.onValueChanged.AddListener((ison) => OnValueChangedRuneItem(ison, nItemId));
                        bRuneCheck = true;
                        trRuneItem.gameObject.SetActive(true);
                    }
                }
            }
        }

        listItemId.Sort();
        listItemId.Reverse();

        for (int i = 0; i < listItemId.Count; i++)
        {
            // 아이템 순서 내림차순 정렬
            Transform trRuneItem = m_trRuneItemListContent.Find(m_goToggleRuneItem.name + listItemId[i]);
            trRuneItem.SetSiblingIndex(i);
        }

        if (bRuneCheck)
        {
            m_trNomaterial.gameObject.SetActive(false);
        }
        else
        {
            m_trNomaterial.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    Transform CreateRuneItem(CsInventoryObjectItem csInventoryObjectItem)
    {
        int nItemId = csInventoryObjectItem.Item.ItemId;
        Transform trRuneItem = m_trRuneItemListContent.Find(m_goToggleRuneItem.name + nItemId);

        if (trRuneItem)
        {
            //
        }
        else
        {
            trRuneItem = Instantiate(m_goToggleRuneItem, m_trRuneItemListContent).transform;
            trRuneItem.name = m_goToggleRuneItem.name + nItemId;

            Toggle toggleRuneItem = trRuneItem.GetComponent<Toggle>();
            toggleRuneItem.group = m_trRuneItemListContent.GetComponent<ToggleGroup>();

            Text textName = trRuneItem.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csInventoryObjectItem.Item.Name;

            Text textAttr = trRuneItem.Find("TextAttr").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttr);
            textAttr.text = CsGameData.Instance.GetAttr(csInventoryObjectItem.Item.Value1).Name;

            Text textAttrValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);
            textAttrValue.text = CsGameData.Instance.GetAttrValueInfo(csInventoryObjectItem.Item.LongValue1).Value.ToString("#,##0");
        }

        Transform trItemSlot = trRuneItem.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventoryObjectItem.Item, false, CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId), true, EnItemSlotSize.Medium);

        return trRuneItem;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateFrameBottom()
    {
        m_trFillSocket.gameObject.SetActive(false);
        m_textEmpty.gameObject.SetActive(false);
        m_textLock.gameObject.SetActive(false);

        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);
        CsSubGearRuneSocket csSubGearRuneSocket = csHeroSubGear.SubGear.SubGearRuneSocketList[m_nRuneSocketIndex];

        // 룬 소켓이 안뚫림
        if (csHeroSubGear.SubGearLevel.Level < csSubGearRuneSocket.RequiredSubGearLevel)
        {
            m_textLock.text = string.Format(CsConfiguration.Instance.GetString("A11_TXT_01001"), csSubGearRuneSocket.RequiredSubGearLevel);
            m_textLock.gameObject.SetActive(true);
        }
        // 룬 소켓이 뚫림
        else
        {
            CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = null;

            for (int i = 0; i < csHeroSubGear.RuneSocketList.Count; i++)
            {
                // 장착한 룬의 소켓 인덱스가 선택한 룬 소켓 인덱스와 같을 때
                if (csHeroSubGear.RuneSocketList[i].Index == m_nRuneSocketIndex)
                {
                    csHeroSubGearRuneSocket = csHeroSubGear.RuneSocketList[i];
                    break;
                }

                csHeroSubGearRuneSocket = null;
            }

            Transform trItemSlot = m_trFillSocket.Find("ItemSlot");

            Text textName = m_trFillSocket.Find("TextName").GetComponent<Text>();
            Text textSocketAttr = m_trFillSocket.Find("TextSocketAttr").GetComponent<Text>();
            Text textSocketAttrValue = textSocketAttr.transform.Find("TextValue").GetComponent<Text>();

            // 룬이 장착이 안되 있음
            if (csHeroSubGearRuneSocket == null)
            {
                // 오른쪽 룬 아이템 리스트 선택 안함
                if (m_nItemId == 0)
                {
                    m_trFillSocket.gameObject.SetActive(false);
                    m_textEmpty.text = csHeroSubGear.SubGear.SubGearRuneSocketList[m_nRuneSocketIndex].EnableText;
                    m_textEmpty.gameObject.SetActive(true);
                }
                else
                {
                    m_textEmpty.gameObject.SetActive(false);

                    CsItem csItem = CsGameData.Instance.GetItem(m_nItemId);
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, false, 0, true, EnItemSlotSize.Medium);

                    textName.text = csItem.Name;
                    textSocketAttr.text = CsGameData.Instance.GetAttr(csItem.Value1).Name;
                    textSocketAttrValue.text = CsGameData.Instance.GetAttrValueInfo(csItem.LongValue1).Value.ToString("#,##0");

                    m_buttonMount.gameObject.SetActive(true);
                    m_buttonUnMount.gameObject.SetActive(false);

                    m_trFillSocket.gameObject.SetActive(true);
                }
            }
            // 룬이 장착 되어 있음
            else
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroSubGearRuneSocket.Item, false, 0, true, EnItemSlotSize.Medium);

                textName.text = csHeroSubGearRuneSocket.Item.Name;
                textSocketAttr.text = CsGameData.Instance.GetAttr(csHeroSubGearRuneSocket.Item.Value1).Name;
                textSocketAttrValue.text = CsGameData.Instance.GetAttrValueInfo(csHeroSubGearRuneSocket.Item.LongValue1).Value.ToString("#,##0");

                m_buttonMount.gameObject.SetActive(false);
                m_buttonUnMount.gameObject.SetActive(true);

                m_trFillSocket.gameObject.SetActive(true);
            }
        }
    }
}