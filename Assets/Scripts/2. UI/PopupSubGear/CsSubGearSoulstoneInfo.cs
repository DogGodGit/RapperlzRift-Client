using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsSubGearSoulstoneInfo : CsPopupSub
{
    [SerializeField] GameObject m_goToggleSoulstone;

    GameObject m_goPopupSetInfo;

    Transform m_trPopupList;

    Transform m_trSubGearSlot;
    Transform m_trSocketList;

    Transform m_trSoulstoneListContent;
    Transform m_trNomaterial;
    Transform m_trFillSocket;
    Transform m_trSetInfo;

    Text m_textLock;
    Text m_textEmpty;

    Button m_buttonSoulstoneLevelUp;
    Button m_buttonSoulstoneMount;
    Button m_buttonSoulstoneUnMount;
    Button m_buttonSetInfo;

    int m_nSocketIndex;
    int m_nItemType;
    int m_nItemId;
    bool m_bNoSubGear = false;
    bool m_bMountCheck = false;
    bool m_bLevelUpCheck = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventSubGearSelected += OnEventSubGearSelected;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate += OnEventSubGearSoulstoneLevelSetActivate;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketMount += OnEventSoulstoneSocketMount;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bNoSubGear)
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        for (int i = 0; i < 6; ++i)
        {
            UpdateSoulstoneEffect(i);
        }

        if (m_trSetInfo != null)
        {
            OnEventClosePopupSetInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventSubGearSelected -= OnEventSubGearSelected;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate -= OnEventSubGearSoulstoneLevelSetActivate;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketMount -= OnEventSoulstoneSocketMount;
    }

    //---------------------------------------------------------------------------------------------------

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickSoulstoneLevelUp()
    {
        CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId).GetHeroSubGearSoulstoneSocket(m_nSocketIndex);
        CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(csHeroSubGearSoulstoneSocket.Item.ItemId);

        if (CsGameData.Instance.MyHeroInfo.Gold >= csItemCompositionRecipe.Gold
            && CsGameData.Instance.MyHeroInfo.GetItemCount(csItemCompositionRecipe.MaterialItemId) >= csItemCompositionRecipe.MaterialItemCount - 1)
        {
            m_bLevelUpCheck = true;
            CsCommandEventManager.Instance.SendMountedSoulstoneCompose(CsUIData.Instance.SubGearId, csHeroSubGearSoulstoneSocket.Index);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSoulstoneUnMount()
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);
        CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = csHeroSubGear.GetHeroSubGearSoulstoneSocket(m_nSocketIndex);

        //해당 아이템이 인벤토리에 없는 경우
        if (CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(csHeroSubGearSoulstoneSocket.Item.ItemId) == null)
        {
            //인벤토리 슬롯 자리가 없는 경우
            if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A10_TXT_02001"));
                return;
            }
        }

        if (csHeroSubGearSoulstoneSocket != null)
        {
            m_bMountCheck = true;
            CsCommandEventManager.Instance.SendSoulstoneSocketUnmount(csHeroSubGear.SubGear.SubGearId, m_nSocketIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSoulstoneMount()
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);
        //CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = csHeroSubGear.GetHeroSubGearSoulstoneSocket(m_nSocketIndex);

        CsSubGearSoulstoneSocket csSubGearSoulstoneSocket = csHeroSubGear.SubGear.GetSubGearSoulstoneSocket(m_nSocketIndex);

        if (csHeroSubGear.SubGearLevel.SubGearGrade.Grade >= csSubGearSoulstoneSocket.RequiredSubGearGrade.Grade)
        {
            m_bMountCheck = true;
            CsCommandEventManager.Instance.SendSoulstoneSocketMount(csHeroSubGear.SubGear.SubGearId, m_nSocketIndex, m_nItemId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSoulstone(bool bIsOn, int nItemId)
    {
        if (bIsOn)
        {
            //여러번 호출할 경우
            if (m_nItemId == nItemId)
            {
                return;
            }
            else
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
                m_nItemId = nItemId;
                DisplayFrameBottom();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSoulstoneSocket(bool bIsOn, int nSocketIndex)
    {
        if (bIsOn)
        {
            if (m_nSocketIndex == nSocketIndex)
            {
                return;
            }
            else
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
                m_nSocketIndex = nSocketIndex;
                m_nItemType = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId).SubGear.SubGearSoulstoneSocketList[nSocketIndex].ItemType;
                DisplaySoulstoneList();
                DisplayFrameBottom();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSetInfo()
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

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearSelected(int nSubGearId)
    {
        //장착으로 인한 호출인지 확인
        if (m_bMountCheck)
        {
            m_bMountCheck = false;
            SetInfoNotice();
        }
        //레벨업으로 인한 호출인지 확인
        else if (m_bLevelUpCheck)
        {
            m_bLevelUpCheck = false;
            SetInfoNotice();
        }
        else
        {
            m_nSocketIndex = CheckSocketIndex();
        }
        //선택된 소켓번호에 대한 아이템 타입 저장
        m_nItemType = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId).SubGear.SubGearSoulstoneSocketList[m_nSocketIndex].ItemType;
        DisplayUpdate(CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId));
        DisplaySoulstoneList();
        DisplayFrameBottom();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearSoulstoneLevelSetActivate()
    {
        SetInfoNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulstoneSocketMount(int nSubGearId)
    {
        UpdateSoulstoneEffect(m_nSocketIndex, true);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        m_trSocketList = transform.Find("ImageSocketBack/SocketList");
        m_trSoulstoneListContent = transform.Find("SoulstoneList/ImageBackground/Scroll View/Viewport/Content");
        m_trNomaterial = transform.Find("SoulstoneList/ImageBackground/Nomaterial");
        m_trSubGearSlot = transform.Find("ImageSocketBack/ItemSlot");
        m_trFillSocket = transform.Find("BotFrame/FillSocket");

        m_buttonSetInfo = transform.Find("ButtonSetInfo").GetComponent<Button>();
        m_buttonSetInfo.onClick.RemoveAllListeners();
        m_buttonSetInfo.onClick.AddListener(OnClickSetInfo);
        m_buttonSetInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        SetInfoNotice();

        Transform trButtonList = m_trFillSocket.Find("ButtonList");

        //소울스톤 레벨업
        m_buttonSoulstoneLevelUp = trButtonList.Find("ButtonLevelUp").GetComponent<Button>();
        m_buttonSoulstoneLevelUp.onClick.RemoveAllListeners();
        m_buttonSoulstoneLevelUp.onClick.AddListener(OnClickSoulstoneLevelUp);
        m_buttonSoulstoneLevelUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonSoulstoneLevelUp = m_buttonSoulstoneLevelUp.transform.Find("Text").GetComponent<Text>();
        textButtonSoulstoneLevelUp.text = CsConfiguration.Instance.GetString("A10_BTN_00001");
        CsUIData.Instance.SetFont(textButtonSoulstoneLevelUp);

        //소울스톤 장착
        m_buttonSoulstoneMount = trButtonList.Find("ButtonMount").GetComponent<Button>();
        m_buttonSoulstoneMount.onClick.RemoveAllListeners();
        m_buttonSoulstoneMount.onClick.AddListener(OnClickSoulstoneMount);
        m_buttonSoulstoneMount.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonSoulstoneMount = m_buttonSoulstoneMount.transform.Find("Text").GetComponent<Text>();
        textButtonSoulstoneMount.text = CsConfiguration.Instance.GetString("A10_BTN_00003");
        CsUIData.Instance.SetFont(textButtonSoulstoneMount);

        //소울스톤 해제
        m_buttonSoulstoneUnMount = trButtonList.Find("ButtonUnMount").GetComponent<Button>();
        m_buttonSoulstoneUnMount.onClick.RemoveAllListeners();
        m_buttonSoulstoneUnMount.onClick.AddListener(OnClickSoulstoneUnMount);
        m_buttonSoulstoneUnMount.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonSoulstoneUnMount = m_buttonSoulstoneUnMount.transform.Find("Text").GetComponent<Text>();
        textButtonSoulstoneUnMount.text = CsConfiguration.Instance.GetString("A10_BTN_00002");
        CsUIData.Instance.SetFont(textButtonSoulstoneUnMount);

        //자물쇠
        m_textLock = transform.Find("BotFrame/TextLock").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLock);

        //소켓 비어있는
        m_textEmpty = transform.Find("BotFrame/TextEmpty").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEmpty);

        //소울 스톤 없을때
        Text textNoStone = m_trNomaterial.Find("TextNoStone").GetComponent<Text>();
        textNoStone.text = CsConfiguration.Instance.GetString("A10_TXT_00002");
        CsUIData.Instance.SetFont(textNoStone);

        Button buttonShortCut0 = m_trNomaterial.Find("ShortCutList/ButtonShortCut0").GetComponent<Button>();
        buttonShortCut0.onClick.RemoveAllListeners();
        //buttonShortCut0.onClick.AddListener();
        buttonShortCut0.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textShortCut0 = buttonShortCut0.transform.Find("Text").GetComponent<Text>();
        textShortCut0.text = CsConfiguration.Instance.GetString("A10_BTN_00004");
        CsUIData.Instance.SetFont(textShortCut0);

        Button buttonShortCut1 = m_trNomaterial.Find("ShortCutList/ButtonShortCut1").GetComponent<Button>();
        buttonShortCut1.onClick.RemoveAllListeners();
        //buttonShortCut0.onClick.AddListener();
        buttonShortCut1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textShortCut1 = buttonShortCut1.transform.Find("Text").GetComponent<Text>();
        textShortCut1.text = CsConfiguration.Instance.GetString("A10_BTN_00005");
        CsUIData.Instance.SetFont(textShortCut1);

        Button buttonShortCut2 = m_trNomaterial.Find("ShortCutList/ButtonShortCut2").GetComponent<Button>();
        buttonShortCut2.onClick.RemoveAllListeners();
        //buttonShortCut0.onClick.AddListener();
        buttonShortCut2.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textShortCut2 = buttonShortCut2.transform.Find("Text").GetComponent<Text>();
        textShortCut2.text = CsConfiguration.Instance.GetString("A10_BTN_00006");
        CsUIData.Instance.SetFont(textShortCut2);

        FirstSubGearCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void FirstSubGearCheck()
    {
        bool bSubGearCheck = false;
        List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;

        for (int i = 0; i < listHeroSubGear.Count; ++i)
        {
            //장착 여부 확인
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }
            if (CsUIData.Instance.SubGearId == listHeroSubGear[i].SubGear.SubGearId)
            {
                m_nSocketIndex = CheckSocketIndex();
                //0번 소켓의 보석 타입
                m_nItemType = listHeroSubGear[i].SubGear.SubGearSoulstoneSocketList[m_nSocketIndex].ItemType;

                DisplayUpdate(listHeroSubGear[i]);
                DisplaySoulstoneList();
                DisplayFrameBottom();
                bSubGearCheck = true;
                break;
            }
        }

        //장착한 보조장비가 화면표시 안함
        if (!bSubGearCheck)
        {
            m_bNoSubGear = true;
            gameObject.SetActive(false);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySoulstoneList()
    {
        List<CsInventorySlot> listInventory = CsGameData.Instance.MyHeroInfo.InventorySlotList;

        int nCount = m_trSoulstoneListContent.childCount;

        for (int i = 0; i < nCount; ++i)
        {
            Transform trChild = m_trSoulstoneListContent.GetChild(i);
            trChild.gameObject.SetActive(false);
            Toggle toggleChild = trChild.GetComponent<Toggle>();
            toggleChild.onValueChanged.RemoveAllListeners();
            toggleChild.isOn = false;
        }

        m_nItemId = 0;

        List<int> listNumber = new List<int>();

        for (int i = 0; i < listInventory.Count; ++i)
        {
            //인벤토리의 아이템 타입이 아이템이고, 소켓 아이템 타입과 같은 아이템들만 가져옴
            if (listInventory[i].EnType == EnInventoryObjectType.Item)
            {
                if (listInventory[i].InventoryObjectItem.Item.ItemType.ItemType == m_nItemType)
                {
                    int iItemId = listInventory[i].InventoryObjectItem.Item.ItemId;
                    listNumber.Add(iItemId);
                    Transform trSoulston = CreateSoulstone(listInventory[i].InventoryObjectItem);
                    Toggle toggle = trSoulston.GetComponent<Toggle>();
                    toggle.onValueChanged.AddListener((ison) => OnValueChangedSoulstone(ison, iItemId));
                    trSoulston.gameObject.SetActive(true);
                }
            }
        }

        //아이템 id에 따른 내림차순 정렬
        listNumber.Sort();
        listNumber.Reverse();

        for (int i = 0; i < listNumber.Count; ++i)
        {
            m_trSoulstoneListContent.Find(listNumber[i].ToString()).SetSiblingIndex(i);
        }

        //소울스톤이 없을시 활성화
        m_trNomaterial.gameObject.SetActive(listNumber.Count == 0 ? true : false);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayUpdate(CsHeroSubGear csHeroSubGear)
    {
        //보조 장비 이미지 셋팅
        CsUIData.Instance.DisplayItemSlot(m_trSubGearSlot, csHeroSubGear);

        for (int i = 0; i < csHeroSubGear.SubGear.SubGearSoulstoneSocketList.Count; ++i)
        {
            int nSocketIndex = csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].SocketIndex;
            Transform trToggleSocket = m_trSocketList.Find("ToggleSocket" + nSocketIndex);
            //소켓 이미지 변경
            Image imageBack = trToggleSocket.Find("Background").GetComponent<Image>();
            imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSubGear/frm_socket" + (csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].ItemType));

            Image ImageIcon = trToggleSocket.Find("ImageIcon").GetComponent<Image>();
            ImageIcon.gameObject.SetActive(false);

            Toggle toggle = trToggleSocket.GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();

            //자물쇠
            if (csHeroSubGear.SubGearLevel.SubGearGrade.Grade >= csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].RequiredSubGearGrade.Grade)
            {
                trToggleSocket.Find("ImageLock").gameObject.SetActive(false);
            }
            else
            {
                trToggleSocket.Find("ImageLock").gameObject.SetActive(true);
            }

            if (nSocketIndex == m_nSocketIndex)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedSoulstoneSocket(ison, nSocketIndex));
        }

        for (int i = 0; i < csHeroSubGear.SoulstoneSocketList.Count; ++i)
        {
            Transform trToggleSocket = m_trSocketList.Find("ToggleSocket" + csHeroSubGear.SoulstoneSocketList[i].Index);

            Image ImageIcon = trToggleSocket.Find("ImageIcon").GetComponent<Image>();
            ImageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csHeroSubGear.SoulstoneSocketList[i].Item.ItemId);
            ImageIcon.gameObject.SetActive(true);
        }
    }

    //객체 생성
    //---------------------------------------------------------------------------------------------------
    Transform CreateSoulstone(CsInventoryObjectItem csInventoryObjectItem)
    {
        int nItemId = csInventoryObjectItem.Item.ItemId;
        Transform trSoulstone = m_trSoulstoneListContent.Find(nItemId.ToString());

        //없는 경우 생성
        if (trSoulstone == null)
        {
            trSoulstone = Instantiate(m_goToggleSoulstone, m_trSoulstoneListContent).transform;
            trSoulstone.name = nItemId.ToString();

            //토글 연결
            Toggle toggle = trSoulstone.GetComponent<Toggle>();
            toggle.group = m_trSoulstoneListContent.GetComponent<ToggleGroup>();
        }

        //아이템 이미지 셋팅
        Transform trItemSlot = trSoulstone.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csInventoryObjectItem.Item, csInventoryObjectItem.Owned, CsGameData.Instance.MyHeroInfo.GetItemCount(csInventoryObjectItem.Item.ItemId));

        //아이템 이름
        Text textName = trSoulstone.Find("TextName").GetComponent<Text>();
        textName.text = csInventoryObjectItem.Item.Name;
        CsUIData.Instance.SetFont(textName);

        //속성 이름
        Text textAttr = trSoulstone.Find("TextAttr").GetComponent<Text>();
        textAttr.text = CsGameData.Instance.GetAttr(csInventoryObjectItem.Item.Value1).Name;
        CsUIData.Instance.SetFont(textAttr);

        //속성 값
        Text textValue = textAttr.transform.Find("TextValue").GetComponent<Text>();
        textValue.text = CsGameData.Instance.GetAttrValueInfo(csInventoryObjectItem.Item.LongValue1).Value.ToString("#,##0");
        CsUIData.Instance.SetFont(textValue);

        return trSoulstone;
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayFrameBottom()
    {
        //초기화
        m_trFillSocket.gameObject.SetActive(false);
        m_textLock.gameObject.SetActive(false);
        m_textEmpty.gameObject.SetActive(false);

        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        //해당 소켓을 가져옴
        CsSubGearSoulstoneSocket csSubGearSoulstoneSocket = csHeroSubGear.SubGear.GetSubGearSoulstoneSocket(m_nSocketIndex);

        //해당 소켓이 뚫려있는지 확인
        if (csHeroSubGear.SubGearLevel.SubGearGrade.Grade < csSubGearSoulstoneSocket.RequiredSubGearGrade.Grade)
        {
            m_textLock.text = string.Format(CsConfiguration.Instance.GetString("A10_TXT_01002"), csHeroSubGear.SubGear.SubGearSoulstoneSocketList[m_nSocketIndex].RequiredSubGearGrade.Name);
            m_textLock.gameObject.SetActive(true);
            return;
        }

        CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId).GetHeroSubGearSoulstoneSocket(m_nSocketIndex);
        Transform trItemSlot = m_trFillSocket.Find("ItemSlot");

        Text textName = m_trFillSocket.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        Text textSocketAttr = m_trFillSocket.Find("TextSocketAttr").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSocketAttr);

        Text textValue = textSocketAttr.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValue);

        Text textGold = m_buttonSoulstoneLevelUp.transform.Find("TextGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGold);

        //장착된 보석이 있는 경우
        if (csHeroSubGearSoulstoneSocket != null)
        {
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroSubGearSoulstoneSocket.Item, false, 0);
            textName.text = csHeroSubGearSoulstoneSocket.Item.Name;
            textSocketAttr.text = CsGameData.Instance.GetAttr(csHeroSubGearSoulstoneSocket.Item.Value1).Name;
            textValue.text = CsGameData.Instance.GetAttrValueInfo(csHeroSubGearSoulstoneSocket.Item.LongValue1).Value.ToString("#,##0");
            m_buttonSoulstoneMount.gameObject.SetActive(false);
            m_buttonSoulstoneUnMount.gameObject.SetActive(true);

            CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(csHeroSubGearSoulstoneSocket.Item.ItemId);

            if (csItemCompositionRecipe == null)
            {
                m_buttonSoulstoneLevelUp.gameObject.SetActive(false);
            }
            else
            {
                m_buttonSoulstoneLevelUp.gameObject.SetActive(true);

                textGold.text = csItemCompositionRecipe.Gold.ToString("##,###");

                if (CsGameData.Instance.MyHeroInfo.Gold >= csItemCompositionRecipe.Gold
                    && CsGameData.Instance.MyHeroInfo.GetItemCount(csItemCompositionRecipe.MaterialItemId) >= csItemCompositionRecipe.MaterialItemCount - 1)
                {
                    m_buttonSoulstoneLevelUp.transform.Find("ImageIcon").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textGold.color = CsUIData.Instance.ColorWhite;
                    m_buttonSoulstoneLevelUp.transform.Find("Text").GetComponent<Text>().color = CsUIData.Instance.ColorWhite;
                    m_buttonSoulstoneLevelUp.transform.Find("ImageIcon").GetComponent<Image>().color = CsUIData.Instance.ColorButtonOn;
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSoulstoneLevelUp, true);
                }
                else if (CsGameData.Instance.MyHeroInfo.Gold < csItemCompositionRecipe.Gold)
                {
                    m_buttonSoulstoneLevelUp.transform.Find("ImageIcon").GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
                    textGold.color = CsUIData.Instance.ColorRed;
                    m_buttonSoulstoneLevelUp.transform.Find("Text").GetComponent<Text>().color = CsUIData.Instance.ColorGray;
                    m_buttonSoulstoneLevelUp.transform.Find("ImageIcon").GetComponent<Image>().color = CsUIData.Instance.ColorButtonOff;
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSoulstoneLevelUp, false);
                }
                else
                {
                    m_buttonSoulstoneLevelUp.transform.Find("ImageIcon").GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
                    textGold.color = CsUIData.Instance.ColorGray;
                    m_buttonSoulstoneLevelUp.transform.Find("Text").GetComponent<Text>().color = CsUIData.Instance.ColorGray;
                    m_buttonSoulstoneLevelUp.transform.Find("ImageIcon").GetComponent<Image>().color = CsUIData.Instance.ColorButtonOff;
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonSoulstoneLevelUp, false);
                }
            }

            m_trFillSocket.gameObject.SetActive(true);
        }
        //소울 스톤 리스트 클릭 시
        else if (m_nItemId != 0)
        {
            CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(m_nItemId);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSoulstoneLevelUp, false);

            if (csItemCompositionRecipe == null)
            {
                m_buttonSoulstoneLevelUp.gameObject.SetActive(false);
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.Gold < csItemCompositionRecipe.Gold)
                {
                    textGold.color = CsUIData.Instance.ColorRed;
                }
                else
                {
                    textGold.color = CsUIData.Instance.ColorWhite;
                }

                textGold.text = csItemCompositionRecipe.Gold.ToString("#,##0");

                m_buttonSoulstoneLevelUp.gameObject.SetActive(true);
            }

            CsItem csItem = CsGameData.Instance.GetItem(m_nItemId);
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, false, 0);

            textName.text = csItem.Name;
            textSocketAttr.text = CsGameData.Instance.GetAttr(csItem.Value1).Name;
            textValue.text = CsGameData.Instance.GetAttrValueInfo(csItem.LongValue1).Value.ToString("#,##0");

            //임시
            m_buttonSoulstoneLevelUp.gameObject.SetActive(false);
            m_buttonSoulstoneMount.gameObject.SetActive(true);
            m_buttonSoulstoneUnMount.gameObject.SetActive(false);
            m_trFillSocket.gameObject.SetActive(true);
        }
        else
        {
            //장착 가능한 보석 표시
            m_textEmpty.text = string.Format(CsConfiguration.Instance.GetString("A10_TXT_01001"), CsGameData.Instance.GetItemType(csHeroSubGear.SubGear.SubGearSoulstoneSocketList[m_nSocketIndex].ItemType).SubCategory.Name);
            m_textEmpty.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetInfoNotice()
    {
        m_buttonSetInfo.transform.Find("ImageNotice").gameObject.SetActive(CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet());
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
        m_trSetInfo = goPopupSetInfo.transform;

        CsPopupSetInfo csPopupSetInfo = m_trSetInfo.GetComponent<CsPopupSetInfo>();
        csPopupSetInfo.EventClosePopupSetInfo += OnEventClosePopupSetInfo;
        csPopupSetInfo.DisplayType(EnPopupSetInfoType.SubGear);
        csPopupSetInfo.SetPosition(EnPopupSetInfoPosition.SubGear);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupSetInfo()
    {
        CsPopupSetInfo csPopupSetInfo = m_trSetInfo.GetComponent<CsPopupSetInfo>();
        csPopupSetInfo.EventClosePopupSetInfo -= OnEventClosePopupSetInfo;

        Destroy(m_trSetInfo.gameObject);
        m_trSetInfo = null;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSoulstoneEffect(int nSockIndex, bool bIson = false)
    {
        Transform trEffect = m_trSocketList.Find("ToggleSocket" + nSockIndex + "/SubGear_Soulstone_Equip");

        if (trEffect != null)
        {
            if (bIson)
            {
                trEffect.gameObject.SetActive(false);
                trEffect.gameObject.SetActive(true);
            }
            else
            {
                trEffect.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    int CheckSocketIndex()
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        for (int i = 0; i < csHeroSubGear.SubGear.SubGearSoulstoneSocketList.Count; i++)
        {
            CsSubGearSoulstoneSocket csSubGearSoulstoneSocket = csHeroSubGear.SubGear.GetSubGearSoulstoneSocket(i);

            if (csHeroSubGear.SubGearLevel.SubGearGrade.Grade >= csSubGearSoulstoneSocket.RequiredSubGearGrade.Grade)
            {
                CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId).GetHeroSubGearSoulstoneSocket(i);
                if (csHeroSubGearSoulstoneSocket == null)
                {
                    return i;
                }
            }
        }
        return 0;
    }
}
