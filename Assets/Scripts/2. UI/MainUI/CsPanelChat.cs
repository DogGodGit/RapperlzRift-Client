using System;
using System.Collections;
using System.Collections.Generic;
using ClientCommon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-11)
//---------------------------------------------------------------------------------------------------

public class CsPanelChat : MonoBehaviour
{
    const int m_nMyChatMaxSize = 540;
    const int m_nSystemMaxSize = 550;
    const int m_nChatMaxSize = 470;
    const int m_nMiniChatMaxSzie = 410;

    [SerializeField] GameObject m_goMessageItem;
    [SerializeField] GameObject m_goMessageItemMy;
    [SerializeField] GameObject m_goMessageItemSimple;
    [SerializeField] GameObject m_goMessageItemMiniSimple;

    [SerializeField] GameObject m_goChatGroup;
    [SerializeField] GameObject m_goChatMsg;
    [SerializeField] GameObject m_goItemMsg;
    [SerializeField] GameObject m_goMiniChatMsg;
    [SerializeField] GameObject m_goMiniItemMsg;

    [SerializeField] GameObject m_goButtonFriend;
    [SerializeField] GameObject m_goButtonOneToOne;

    GameObject m_goPopupItemInfo;

    //Transform m_trPanelPopup;
    Transform m_trChat;
    Transform m_trScrollViewChat;
    Transform m_trMiniChat;
    Transform m_trFriendList;
    Transform m_trOneToOneChattingList;
    Transform m_trInputChat;
    Transform m_trPopupList;
    Transform m_trWorld;
	Transform m_trSystemWorld;

    Transform m_trChatContent;
    Transform m_trMiniChatContent;
    Transform m_trFriendListContent;
    Transform m_trOneToOneChattingListContent;

    Transform m_trItemInfo;
    Transform m_trChatGroup;

    Button m_buttonSmallFriendList;
    Button m_buttonSpeaker;
    Button m_buttonFriendList;
    Button m_buttonOneToOneList;

    InputField m_inputFieldSend;

    Text m_textNoSend;
    Text m_textNoFriend;
    Text m_textNoOneToOneChatting;

	Image m_imageChat;

    ScrollRect m_scrollRectChat;
    ScrollRect m_scrollRectMiniChat;

    int m_nChannel;
    int m_nChattingMinInterval;
    int m_nPreChat;

    Guid m_guidTargetHeroId = Guid.Empty;

    bool m_bSend = true;
    bool m_bReferenceOneToOne = false;
	bool m_bSendingScheduleNotice = false;
    float m_flTime = 0;

    List<CsChattingMessage> m_listPreChatMessage = new List<CsChattingMessage>();

    CsPopupItemInfo m_csPopupItemInfo;

    PDChattingLink m_pDChattingLink;

    string m_strItemName = string.Empty;
	string m_strSpacing = "                                                                                                                                  ";

    Coroutine m_coroutineWorldChatting;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestTaunted += OnEventTrueHeroQuestTaunted;

        CsGameEventUIToUI.Instance.EventChattingMessageReceived += OnEventChattingMessageReceived;
        CsGameEventUIToUI.Instance.EventChattingMessageSend += OnEventChattingMessageSend;
        CsGameEventUIToUI.Instance.EventGearShare += OnEventGearShare;
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventOpenOneToOneChat += OnEventOpenOneToOneChat;
		CsGameEventUIToUI.Instance.EventSystemMessage += OnEventSystemMessage;
		CsGameEventUIToUI.Instance.EventScheduleNotice += OnEventScheduleNotice;

        CsGuildManager.Instance.EventGuildDailyObjectiveNoticeEvent += OnEventGuildDailyObjectiveNoticeEvent;
        CsGuildManager.Instance.EventGuildSpiritAnnounced += OnEventGuildSpiritAnnounced;

        CsGuildManager.Instance.EventGuildBlessingBuffStarted += OnEventGuildBlessingBuffStarted;
        CsGuildManager.Instance.EventGuildBlessingBuffEnded += OnEventGuildBlessingBuffEnded;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestTaunted -= OnEventTrueHeroQuestTaunted;

        CsGameEventUIToUI.Instance.EventChattingMessageReceived -= OnEventChattingMessageReceived;
        CsGameEventUIToUI.Instance.EventChattingMessageSend -= OnEventChattingMessageSend;
        CsGameEventUIToUI.Instance.EventGearShare -= OnEventGearShare;
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventOpenOneToOneChat -= OnEventOpenOneToOneChat;
		CsGameEventUIToUI.Instance.EventSystemMessage -= OnEventSystemMessage;
		CsGameEventUIToUI.Instance.EventScheduleNotice -= OnEventScheduleNotice;

        CsGuildManager.Instance.EventGuildDailyObjectiveNoticeEvent -= OnEventGuildDailyObjectiveNoticeEvent;
        CsGuildManager.Instance.EventGuildSpiritAnnounced -= OnEventGuildSpiritAnnounced;

        CsGuildManager.Instance.EventGuildBlessingBuffStarted -= OnEventGuildBlessingBuffStarted;
        CsGuildManager.Instance.EventGuildBlessingBuffEnded -= OnEventGuildBlessingBuffEnded;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");
        //m_trPanelPopup = Canvas2.Find("PanelPopup");

        //채팅 재전송 시간
        m_nChattingMinInterval = CsGameConfig.Instance.ChattingMinInterval;

        //채널 기본 값
        m_nChannel = 0;
        //이전 메시지 기본 값
        m_nPreChat = -1;

        m_trChat = transform.Find("PopupChat");
        m_trScrollViewChat = m_trChat.Find("ImageBackGround/ScrollViewChat");
        m_scrollRectChat = m_trScrollViewChat.GetComponent<ScrollRect>();
        m_trChatContent = m_trScrollViewChat.Find("Viewport/Content");

        Button buttonCancel = m_trChat.Find("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(OnClickChatClose);
        buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //세계채팅 아이템
        m_buttonSpeaker = m_trChat.Find("ImageBackGround/ButtonSpeaker").GetComponent<Button>();
        m_buttonSpeaker.onClick.RemoveAllListeners();
        m_buttonSpeaker.onClick.AddListener(OnClickWorldChattingItemInfo);
        m_buttonSpeaker.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textSpeakerCount = m_buttonSpeaker.transform.Find("Text").GetComponent<Text>();
        textSpeakerCount.text = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WorldChattingItemId).ToString("#,##0");
        CsUIData.Instance.SetFont(textSpeakerCount);

        //대화 목록 버튼
        m_buttonOneToOneList = m_trChat.Find("ImageBackGround/ButtonChattingList").GetComponent<Button>();
        m_buttonOneToOneList.onClick.RemoveAllListeners();
        m_buttonOneToOneList.onClick.AddListener(OnClickOneToOneChattingList);
        m_buttonOneToOneList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textChattingList = m_buttonOneToOneList.transform.Find("Text").GetComponent<Text>();
        textChattingList.text = CsConfiguration.Instance.GetString("A34_BTN_00003");
        CsUIData.Instance.SetFont(textChattingList);

        //친구 목록 버튼
        m_buttonFriendList = m_trChat.Find("ImageBackGround/ButtonFriendList").GetComponent<Button>();
        m_buttonFriendList.onClick.RemoveAllListeners();
        m_buttonFriendList.onClick.AddListener(OnClickFriendList);
        m_buttonFriendList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonFriendList = m_buttonFriendList.transform.Find("Text").GetComponent<Text>();
        textButtonFriendList.text = CsConfiguration.Instance.GetString("A34_BTN_00002");
        CsUIData.Instance.SetFont(textButtonFriendList);
        m_buttonSmallFriendList = m_trChat.Find("ImageBackGround/ButtonSmallFriendList").GetComponent<Button>();
        m_buttonSmallFriendList.onClick.RemoveAllListeners();
        m_buttonSmallFriendList.onClick.AddListener(OnClickFriendList);
        m_buttonSmallFriendList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //전체 채널 대화 입력 불가
        m_textNoSend = m_trChat.Find("ImageBackGround/TextNoSend").GetComponent<Text>();
        m_textNoSend.text = CsConfiguration.Instance.GetString("A34_TXT_00001");
        CsUIData.Instance.SetFont(m_textNoSend);

        //미니 채팅
        m_trMiniChat = transform.Find("PopupMiniChat");
        m_scrollRectMiniChat = m_trMiniChat.Find("Scroll View").GetComponent<ScrollRect>();
        m_trMiniChatContent = m_trMiniChat.Find("Scroll View/Viewport/Content");
        m_trWorld = m_trMiniChat.Find("ImageWorld");
		m_trSystemWorld = m_trMiniChat.Find("ImageSystemWorld");

        Button buttonMiniChat = m_trMiniChat.GetComponent<Button>();
        buttonMiniChat.onClick.RemoveAllListeners();
        buttonMiniChat.onClick.AddListener(OnClickChatOpen);
        buttonMiniChat.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		m_imageChat = m_trWorld.Find("Image").GetComponent<Image>();

        //채팅 입력
        m_trInputChat = m_trChat.Find("ImageBackGround/InputChat");

        m_inputFieldSend = m_trInputChat.Find("InputField").GetComponent<InputField>();
        Button buttonSend = m_trInputChat.Find("ButtonSend").GetComponent<Button>();
        buttonSend.onClick.RemoveAllListeners();
        buttonSend.onClick.AddListener(OnClickSendMessage);
        buttonSend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textSendPlaceholder = m_inputFieldSend.transform.Find("Placeholder").GetComponent<Text>();
        textSendPlaceholder.text = CsConfiguration.Instance.GetString("A34_TXT_00002");
        CsUIData.Instance.SetFont(textSendPlaceholder);

        Text textInput = m_inputFieldSend.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInput);

        Button buttonPreMessage = m_trInputChat.Find("ButtonPreChat").GetComponent<Button>();
        buttonPreMessage.onClick.RemoveAllListeners();
        buttonPreMessage.onClick.AddListener(OnClickPreMessage);
        buttonPreMessage.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //상단
        Transform trTop = m_trChat.Find("ImageBackGround/ImageTop");

        Text textName = trTop.Find("TextName").GetComponent<Text>();
        textName.text = CsConfiguration.Instance.GetString("MMENU_NAME_7");
        CsUIData.Instance.SetFont(textName);

        Button buttonExit = trTop.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(OnClickChatClose);
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //말풍선 On / Off
        Text textBubble = trTop.Find("TextBubble").GetComponent<Text>();
        textBubble.text = CsConfiguration.Instance.GetString("A34_TXT_00009");
        CsUIData.Instance.SetFont(textBubble);

        Toggle toggleBubbleOn = textBubble.transform.Find("ToggleBubbleOn").GetComponent<Toggle>();
        toggleBubbleOn.onValueChanged.RemoveAllListeners();
        Text textBubbleOn = toggleBubbleOn.transform.Find("Text").GetComponent<Text>();
        textBubbleOn.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ON");
        CsUIData.Instance.SetFont(textBubbleOn);

        Toggle toggleBubbleOff = textBubble.transform.Find("ToggleBubbleOff").GetComponent<Toggle>();
        toggleBubbleOff.onValueChanged.RemoveAllListeners();
        Text textBubbleOff = toggleBubbleOff.transform.Find("Text").GetComponent<Text>();
        textBubbleOff.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_OFF");
        CsUIData.Instance.SetFont(textBubbleOff);

        //말풍선
        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyChattingBubble))
        {
            int nBubble = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyChattingBubble);

            if (nBubble == 1)
            {
                toggleBubbleOn.isOn = true;
                textBubbleOn.color = CsUIData.Instance.ColorWhite;
                textBubbleOff.color = CsUIData.Instance.ColorGray;
            }
            else
            {
                toggleBubbleOff.isOn = true;
                textBubbleOn.color = CsUIData.Instance.ColorGray;
                textBubbleOff.color = CsUIData.Instance.ColorWhite;
            }
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyChattingBubble, 1);
            toggleBubbleOn.isOn = true;
            textBubbleOn.color = CsUIData.Instance.ColorWhite;
            textBubbleOff.color = CsUIData.Instance.ColorGray;
        }

        toggleBubbleOn.onValueChanged.AddListener((ison) => { OnValueChangedBubble(ison, textBubbleOn, 1); });
        toggleBubbleOff.onValueChanged.AddListener((ison) => { OnValueChangedBubble(ison, textBubbleOff, 0); });

        //친구 리스트
        m_trFriendList = m_trChat.Find("ImageBackGround/TextFriendList");
        m_trFriendListContent = m_trFriendList.Find("Scroll View/Viewport/Content");

        Text textFriendList = m_trFriendList.GetComponent<Text>();
        textFriendList.text = CsConfiguration.Instance.GetString("A34_TXT_00005");
        CsUIData.Instance.SetFont(textFriendList);

        m_textNoFriend = m_trFriendList.Find("TextNoFriend").GetComponent<Text>();
        m_textNoFriend.text = CsConfiguration.Instance.GetString("A34_TXT_00006");
        CsUIData.Instance.SetFont(m_textNoFriend);

        //1:1채팅 리스트 
        m_trOneToOneChattingList = m_trChat.Find("ImageBackGround/TextOneToOneChattingList");
        m_trOneToOneChattingListContent = m_trOneToOneChattingList.Find("Scroll View/Viewport/Content");
        Text textOneToOneList = m_trOneToOneChattingList.GetComponent<Text>();
        textOneToOneList.text = CsConfiguration.Instance.GetString("A34_TXT_00010");
        CsUIData.Instance.SetFont(textOneToOneList);

        m_textNoOneToOneChatting = m_trOneToOneChattingList.Find("TextNoOneToOneChatting").GetComponent<Text>();
        m_textNoOneToOneChatting.text = CsConfiguration.Instance.GetString("A34_TXT_00004");
        CsUIData.Instance.SetFont(m_textNoOneToOneChatting);

        //채널 설정
        Transform trChatChannel = m_trChat.Find("ImageBackGround/ChatChannel");
        for (int i = 0; i <= (int)EnChattingType.OneToOne; i++)
        {
            int nChannel = i;

            Toggle toggleChannel = trChatChannel.Find("Toggle" + i).GetComponent<Toggle>();
            Text textChannel = toggleChannel.transform.Find("Text").GetComponent<Text>();

            toggleChannel.onValueChanged.RemoveAllListeners();
            toggleChannel.onValueChanged.AddListener((ison) => { OnValueChangedChannel(ison, nChannel, textChannel); });
            CsUIData.Instance.SetFont(textChannel);

            if (i == 0)
            {
                textChannel.text = CsConfiguration.Instance.GetString("A34_BTN_00001");
            }
            else
            {
                textChannel.text = CsGameData.Instance.GetChattingType((EnChattingType)i).Name;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        //메시지 대기 시간
        if (!m_bSend)
        {
            if (m_flTime + m_nChattingMinInterval < Time.time)
            {
                m_bSend = true;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AllObjectOff()
    {
        m_trScrollViewChat.gameObject.SetActive(false);
        m_trFriendList.gameObject.SetActive(false);
        m_trOneToOneChattingList.gameObject.SetActive(false);
        m_trInputChat.gameObject.SetActive(false);
        m_textNoSend.gameObject.SetActive(false);
        m_buttonOneToOneList.gameObject.SetActive(false);
        m_buttonFriendList.gameObject.SetActive(false);
        m_buttonSmallFriendList.gameObject.SetActive(false);
        m_buttonSpeaker.gameObject.SetActive(false);
        m_trOneToOneChattingList.gameObject.SetActive(false);
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    //채팅 창 열기
    void OnClickChatOpen()
    {
        if ((EnChattingType)m_nChannel == EnChattingType.OneToOne)
        {
            AllObjectOff();
            m_trOneToOneChattingList.gameObject.SetActive(true);
            m_buttonFriendList.gameObject.SetActive(true);
            DisplayOneToOne();
        }
        else
        {
            DisplayChanneMessage();
        }

        Text textSpeakerCount = m_buttonSpeaker.transform.Find("Text").GetComponent<Text>();
        textSpeakerCount.text = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WorldChattingItemId).ToString("#,##0");
        m_trChat.gameObject.SetActive(true);
        m_trMiniChat.gameObject.SetActive(false);
        m_scrollRectChat.verticalNormalizedPosition = 0;
    }

    //---------------------------------------------------------------------------------------------------
    //채팅 창 닫기
    void OnClickChatClose()
    {
        RemoveAllChatMessage();
        m_trChat.gameObject.SetActive(false);
        m_trMiniChat.gameObject.SetActive(true);
        m_scrollRectMiniChat.verticalNormalizedPosition = 0;
        m_guidTargetHeroId = Guid.Empty;
        m_pDChattingLink = null;
        m_inputFieldSend.text = string.Empty;
        m_bReferenceOneToOne = false;

        if (m_listPreChatMessage.Count != 0)
        {
            m_nPreChat = m_listPreChatMessage.Count - 1;
        }
        else
        {
            m_nPreChat = -1;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //대화 목록 버튼
    void OnClickOneToOneChattingList()
    {
        AllObjectOff();
        m_trOneToOneChattingList.gameObject.SetActive(true);
        m_buttonFriendList.gameObject.SetActive(true);
        RemoveAllChatMessage();
        DisplayOneToOne();
    }

    //---------------------------------------------------------------------------------------------------
    //친구 목록 버튼
    void OnClickFriendList()
    {
        AllObjectOff();
        m_trFriendList.gameObject.SetActive(true);
        m_textNoFriend.gameObject.SetActive(true);
        m_buttonOneToOneList.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    //채널 변경
    void OnValueChangedChannel(bool bIson, int nChannel, Text text)
    {
        if (bIson && (EnChattingType)m_nChannel == EnChattingType.OneToOne || m_nChannel != nChannel)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nChannel = nChannel;
            ChannelSetting();
            text.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //채널 변경시 셋팅 변경
    void ChannelSetting()
    {
        RemoveAllChatMessage();
        AllObjectOff();
        switch (m_nChannel)
        {
            case 0:
                DisplayChanneMessage();
                m_textNoSend.gameObject.SetActive(true);
                m_trScrollViewChat.gameObject.SetActive(true);
                m_guidTargetHeroId = Guid.Empty;
                m_bReferenceOneToOne = false;
                break;
            case 1:
            case 2:
            case 4:
            case 5:
                DisplayChanneMessage();
                m_trInputChat.gameObject.SetActive(true);
                m_trScrollViewChat.gameObject.SetActive(true);
                m_guidTargetHeroId = Guid.Empty;
                m_bReferenceOneToOne = false;
                break;
            case 3:
                DisplayChanneMessage();
                m_buttonSpeaker.gameObject.SetActive(true);
                m_trInputChat.gameObject.SetActive(true);
                m_trScrollViewChat.gameObject.SetActive(true);
                m_guidTargetHeroId = Guid.Empty;
                m_bReferenceOneToOne = false;
                break;
            case 6:
                if (m_bReferenceOneToOne)
                {
                    DisplayChanneMessage();
                    m_trInputChat.gameObject.SetActive(true);
                    m_trScrollViewChat.gameObject.SetActive(true);
                    m_buttonSmallFriendList.gameObject.SetActive(true);
                    m_bReferenceOneToOne = false;
                }
                else
                {
                    m_trOneToOneChattingList.gameObject.SetActive(true);
                    m_buttonFriendList.gameObject.SetActive(true);
                    DisplayOneToOne();
                }
                break;
        }
        m_scrollRectChat.verticalNormalizedPosition = 0;
    }

    //---------------------------------------------------------------------------------------------------
    //말풍선 On/Off
    void OnValueChangedBubble(bool bIson, Text textToggle ,int nValue)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyChattingBubble, nValue);
            textToggle.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textToggle.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //채널에 따른 메시지 전송
    void OnClickSendMessage()
    {
        switch (m_nChannel)
        {
            case 0:
                break;
            case 1:
                SendMessage();
                break;
            case 2:
                //SendMessage();
                break;
            case 3:
                //확성기가 있는 경우
                if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WorldChattingItemId) > 0)
                {
                    SendMessage();
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A34_TXT_02002"));
                }
                break;
            case 4:
                if (CsGameData.Instance.MyHeroInfo.Party != null)
                {
                    SendMessage();
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A34_TXT_02003"));
                }
                break;
            case 5:
                if (CsGuildManager.Instance.Guild != null)
                {
                    SendMessage();
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A34_TXT_02004"));
                }
                break;
            case 6:
                if (m_guidTargetHeroId != Guid.Empty)
                {
                    SendMessage();
                }
                break;

            default:
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //확성기 아이템 정보창
    void OnClickWorldChattingItemInfo()
    {
        CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.WorldChattingItemId);

        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(csItem));
        }
        else
        {
            OpenPopupItemInfo(csItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //보냈던 메시지 다시 불러오기 최대 5개
    void OnClickPreMessage()
    {
        if (m_listPreChatMessage.Count != 0)
        {
            if (m_listPreChatMessage[m_nPreChat].ChattingLink != null)
            {
                m_inputFieldSend.text = string.Empty;
                for (int i = 0; i < m_listPreChatMessage[m_nPreChat].Messages.Length; ++i)
                {
                    m_inputFieldSend.text += m_listPreChatMessage[m_nPreChat].Messages[i];

                    if (i == 0 && m_listPreChatMessage[m_nPreChat].ChattingLink.type == PDChattingLink.kType_MainGear)
                    {
                        PDMainGearChattingLink pDMainGearChattingLink = (PDMainGearChattingLink)m_listPreChatMessage[m_nPreChat].ChattingLink;
                        CsMainGear csMainGear = CsGameData.Instance.GetMainGear(pDMainGearChattingLink.gear.mainGearId);
                        m_inputFieldSend.text += csMainGear.Name;
                    }
                    else if (i == 0 && m_listPreChatMessage[m_nPreChat].ChattingLink.type == PDChattingLink.kType_SubGear)
                    {
                        PDSubGearChattingLink pDSubGearChattingLink = (PDSubGearChattingLink)m_listPreChatMessage[m_nPreChat].ChattingLink;
                        CsSubGear csSubGear = CsGameData.Instance.GetSubGear(pDSubGearChattingLink.gear.subGearId);
                        m_inputFieldSend.text += csSubGear.Name;
                    }
                    else if (i == 0 && m_listPreChatMessage[m_nPreChat].ChattingLink.type == PDChattingLink.kType_MountGear)
                    {
                        PDMountGearChattingLink pDMountGearChattingLink = (PDMountGearChattingLink)m_listPreChatMessage[m_nPreChat].ChattingLink;
                        CsMountGear csMount = CsGameData.Instance.GetMountGear(pDMountGearChattingLink.gear.mountGearId);
                        m_inputFieldSend.text += csMount.Name;
                    }
                }
                m_pDChattingLink = m_listPreChatMessage[m_nPreChat].ChattingLink;
            }
            else
            {
                m_inputFieldSend.text = m_listPreChatMessage[m_nPreChat].ChattingMessage;
                m_pDChattingLink = null;
            }

            if (m_nPreChat > 0)
            {
                m_nPreChat--;
            }
            else
            {
                m_nPreChat = m_listPreChatMessage.Count - 1;
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    //아이템 채팅 링크 팝업창
    void OnClickItemInfo(PDChattingLink pDChattingLink)
    {
        if (pDChattingLink != null)
        {
            if (pDChattingLink.type == PDChattingLink.kType_MainGear)
            {
                PDMainGearChattingLink pDMainGearChattingLink = (PDMainGearChattingLink)pDChattingLink;
                CsHeroMainGear csHeroMainGear = new CsHeroMainGear(pDMainGearChattingLink.gear.id, pDMainGearChattingLink.gear.mainGearId, pDMainGearChattingLink.gear.enchantLevel,
                    pDMainGearChattingLink.gear.owned, pDMainGearChattingLink.gear.optionAttrs);
                if (m_goPopupItemInfo == null)
                {
                    StartCoroutine(LoadPopupItemInfo(csHeroMainGear));
                }
                else
                {
                    OpenPopupItemInfo(csHeroMainGear);
                }
            }
            else if (pDChattingLink.type == PDChattingLink.kType_SubGear)
            {
                PDSubGearChattingLink pDSubGearChattingLink = (PDSubGearChattingLink)pDChattingLink;
                CsHeroSubGear csHeroSubGear = new CsHeroSubGear(pDSubGearChattingLink.gear.subGearId, pDSubGearChattingLink.gear.level, pDSubGearChattingLink.gear.quality,
                    pDSubGearChattingLink.gear.equippedRuneSockets, pDSubGearChattingLink.gear.equippedSoulstoneSockets);
                if (m_goPopupItemInfo == null)
                {
                    StartCoroutine(LoadPopupItemInfo(csHeroSubGear));
                }
                else
                {
                    OpenPopupItemInfo(csHeroSubGear);
                }
            }
            else
            {
                PDMountGearChattingLink pDMountGearChattingLink = (PDMountGearChattingLink)pDChattingLink;
                CsHeroMountGear csHeroMountGear = new CsHeroMountGear(pDMountGearChattingLink.gear);
                if (m_goPopupItemInfo == null)
                {
                    StartCoroutine(LoadPopupItemInfo(csHeroMountGear));
                }
                else
                {
                    OpenPopupItemInfo(csHeroMountGear);
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    //클릭시 1:1 대화로 전환
    void OnClickOneToOneEnter(Guid guidTarget)
    {
        RemoveAllChatMessage();
        AllObjectOff();
        m_trScrollViewChat.gameObject.SetActive(true);
        m_trInputChat.gameObject.SetActive(true);
        m_buttonSmallFriendList.gameObject.SetActive(true);
        m_guidTargetHeroId = guidTarget;
        DisplayChanneMessage();
    }

    //--------------------------------------------------------------------------------------------------
    //다른 유저 조회창 활성화
    void OnClickOpenPopupUserReference(CsHeroBase csHeroBase)
    {
        CsGameEventUIToUI.Instance.OnEventOpenUserReference(csHeroBase);
    }

    //---------------------------------------------------------------------------------------------------
    //길드 미션에 대한 길드 가입 버튼 클릭시
    void OnClickGuildApply(Guid guid)
    {
        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.GuildRequiredHeroLevel)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00305"));
        }
        else if (CsGuildManager.Instance.Guild != null)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00303"));
        }
        else
        {
            if (CsGuildManager.Instance.DailyGuildApplicationCount >= CsGameConfig.Instance.GuildDailyApplicationMaxCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02001"));
            }
            else if (CsGuildManager.Instance.GuildRejoinRemainingTime >= Time.realtimeSinceStartup)
            {
                int nRejoinTime = (int)((CsGuildManager.Instance.GuildRejoinRemainingTime - Time.realtimeSinceStartup) / 60) + 1;
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02002"), nRejoinTime.ToString()));
            }
            else
            {
                CsGuildManager.Instance.SendGuildApply(guid);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildBlessingBuff()
    {
        if (CsGuildManager.Instance.Guild == null)
        {
            // 길드 없음
            return;
        }
        else
        {
            if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.None)
            {
                if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
                {
                    // 길드 영지

                    // 낚시 이동 표시
                    CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Fishing);
                }
                else
                {
                    // 대륙

                    // 길드 영지 이동
                    CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                }
            }
            else
            {
                // 던전
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //길드 알림
    void OnClickGuildContent(int nContentId)
    {
        int nTaskId = CsGameData.Instance.GetGuildContent(nContentId).TaskId;
        int nProgressMaxCount = CsGameData.Instance.GetTodayTask(nTaskId).MaxCount;

        CsHeroTodayTask csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(nTaskId);

        if (csHeroTodayTask != null)
        {
            if (csHeroTodayTask.ProgressCount >= nProgressMaxCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_01005"));
                return;
            }
        }
        else if ((EnTodayTaskType)nTaskId == EnTodayTaskType.GuildHunting)
        {
            nProgressMaxCount = CsGuildManager.Instance.GuildHuntingQuest.LimitCount;

            if (CsGuildManager.Instance.DailyGuildHuntingQuestStartCount >= nProgressMaxCount)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_01005"));
                return;
            }
        }

        if (CsGameData.Instance.MyHeroInfo.LocationId != 201)
        {
            if ((EnTodayTaskType)nTaskId == EnTodayTaskType.GuildMissionQuest)
            {
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildMission);
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            }
            else if ((EnTodayTaskType)nTaskId == EnTodayTaskType.GuildSupply)
            {
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildSupplySupport);
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            }
            else if ((EnTodayTaskType)nTaskId == EnTodayTaskType.GuildHunting)
            {
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildSupplySupport);
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                             CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                             CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
        }
        else
        {
            switch ((EnTodayTaskType)nTaskId)
            {
                case EnTodayTaskType.GuildMissionQuest:
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_03001"));
                    break;

                case EnTodayTaskType.GuildAltar:
                    if (CsGuildManager.Instance.GuildPlayAutoState == EnGuildPlayState.Altar && CsGuildManager.Instance.Auto)
                        return;
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildAlter);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

                case EnTodayTaskType.GuildSupply:
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_03001"));
                    break;

                case EnTodayTaskType.GuildFarmQuest:
                    if (CsGuildManager.Instance.GuildPlayAutoState == EnGuildPlayState.FarmQuest && CsGuildManager.Instance.Auto)
                        return;
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildFarm);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

                case EnTodayTaskType.GuildFoodWarehouse:
                    if (CsGuildManager.Instance.GuildPlayAutoState == EnGuildPlayState.FoodWareHouse && CsGuildManager.Instance.Auto)
                        return;

                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildFoodWareHouse);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;
            }
        }
    }

	//--------------------------------------------------------------------------------------------------
	void OnClickTrueHeroQuestTaunted(int nContinentId, int nNationId, Vector3 vtPosition)
	{
		CsTrueHeroQuestManager.Instance.OnEventTrueHeroQuestAutoMove(nContinentId, nNationId, vtPosition);
		CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.TrueHeroTaunted);
	}

	#endregion Event Handler

    #region Event

    //--------------------------------------------------------------------------------------------------
    //
    void OnEventChattingMessageSend()
    {
        //보낸 메시지가 월드채팅 메시지 일때
        if ((EnChattingType)m_nChannel == EnChattingType.World)
        {
            Text textSpeakerCount = m_buttonSpeaker.transform.Find("Text").GetComponent<Text>();
            textSpeakerCount.text = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.WorldChattingItemId).ToString("#,##0");
        }
    }

    //--------------------------------------------------------------------------------------------------
    void OnEventOpenOneToOneChat(Guid guid)
    {
        m_bReferenceOneToOne = true;
        m_guidTargetHeroId = guid;

        if (!m_trChat.gameObject.activeSelf)
        {
            m_trChat.gameObject.SetActive(true);
            m_trMiniChat.gameObject.SetActive(false);
        }

        if (m_nChannel != (int)EnChattingType.OneToOne)
        {
            Transform trChatChannel = m_trChat.Find("ImageBackGround/ChatChannel");
            Toggle toggleChannel = trChatChannel.Find("Toggle" + (int)EnChattingType.OneToOne).GetComponent<Toggle>();
            toggleChannel.isOn = true;
        }
        else
        {
            ChannelSetting();
        }

    }

    //--------------------------------------------------------------------------------------------------
    //아이템 공유시 서버에 보낼 채팅 링크 생성
    void OnEventGearShare(CsHeroObject csHeroObject)
    {
        if (csHeroObject != null)
        {
            //메인장비 팩킹
            if (csHeroObject.HeroObjectType == EnHeroObjectType.MainGear)
            {
                PDMainGearChattingLink pDMainGearChattingLink = new PDMainGearChattingLink();
                PDFullHeroMainGear pDFullHeroMainGear = new PDFullHeroMainGear();
                CsHeroMainGear csHeroMainGear = (CsHeroMainGear)csHeroObject;
                m_strItemName = csHeroMainGear.MainGear.Name;
                pDFullHeroMainGear.id = csHeroMainGear.Id;
                pDFullHeroMainGear.mainGearId = csHeroMainGear.MainGear.MainGearId;
                pDFullHeroMainGear.enchantLevel = csHeroMainGear.EnchantLevel;
                pDFullHeroMainGear.owned = csHeroMainGear.Owned;
                PDHeroMainGearOptionAttr[] apDHeroMainGearOptionAttr = new PDHeroMainGearOptionAttr[csHeroMainGear.OptionAttrList.Count];

                for (int i = 0; i < csHeroMainGear.OptionAttrList.Count; ++i)
                {
                    PDHeroMainGearOptionAttr pDHeroMainGearOptionAttr = new PDHeroMainGearOptionAttr();
                    pDHeroMainGearOptionAttr.index = csHeroMainGear.OptionAttrList[i].Index;
                    pDHeroMainGearOptionAttr.attrId = csHeroMainGear.OptionAttrList[i].Attr.AttrId;
                    pDHeroMainGearOptionAttr.attrValueId = csHeroMainGear.OptionAttrList[i].AttrValueId;
                    pDHeroMainGearOptionAttr.grade = csHeroMainGear.OptionAttrList[i].MainGearOptionAttrGrade.AttrGrade;
                    apDHeroMainGearOptionAttr[i] = pDHeroMainGearOptionAttr;
                }

                pDFullHeroMainGear.optionAttrs = apDHeroMainGearOptionAttr;
                pDMainGearChattingLink.gear = pDFullHeroMainGear;
                m_inputFieldSend.text = csHeroMainGear.MainGear.Name;
                m_pDChattingLink = pDMainGearChattingLink;
            }
            //보조장비 팩킹
            else if (csHeroObject.HeroObjectType == EnHeroObjectType.SubGear)
            {

                PDSubGearChattingLink pDSubGearChattingLink = new PDSubGearChattingLink();
                PDFullHeroSubGear pDFullHeroSubGear = new PDFullHeroSubGear();
                CsHeroSubGear csHeroSubGear = (CsHeroSubGear)csHeroObject;

                m_strItemName = csHeroSubGear.SubGear.Name;
                pDFullHeroSubGear.subGearId = csHeroSubGear.SubGear.SubGearId;
                pDFullHeroSubGear.level = csHeroSubGear.Level;
                pDFullHeroSubGear.quality = csHeroSubGear.Quality;
                PDHeroSubGearSoulstoneSocket[] apDHeroSubGearSoulstoneSocket = new PDHeroSubGearSoulstoneSocket[csHeroSubGear.SoulstoneSocketList.Count];
                PDHeroSubGearRuneSocket[] apDHeroSubGearRuneSocket = new PDHeroSubGearRuneSocket[csHeroSubGear.RuneSocketList.Count];

                for (int i = 0; i < csHeroSubGear.SoulstoneSocketList.Count; ++i)
                {
                    PDHeroSubGearSoulstoneSocket pDHeroSubGearSoulstoneSocket = new PDHeroSubGearSoulstoneSocket();
                    pDHeroSubGearSoulstoneSocket.index = csHeroSubGear.SoulstoneSocketList[i].Index;
                    pDHeroSubGearSoulstoneSocket.itemId = csHeroSubGear.SoulstoneSocketList[i].Item.ItemId;
                    apDHeroSubGearSoulstoneSocket[i] = pDHeroSubGearSoulstoneSocket;
                }

                for (int i = 0; i < csHeroSubGear.RuneSocketList.Count; ++i)
                {
                    PDHeroSubGearRuneSocket pDHeroSubGearRuneSocket = new PDHeroSubGearRuneSocket();
                    pDHeroSubGearRuneSocket.index = csHeroSubGear.RuneSocketList[i].Index;
                    pDHeroSubGearRuneSocket.itemId = csHeroSubGear.RuneSocketList[i].Item.ItemId;
                    apDHeroSubGearRuneSocket[i] = pDHeroSubGearRuneSocket;
                }

                pDFullHeroSubGear.equippedSoulstoneSockets = apDHeroSubGearSoulstoneSocket;
                pDFullHeroSubGear.equippedRuneSockets = apDHeroSubGearRuneSocket;
                m_inputFieldSend.text = csHeroSubGear.SubGear.Name;
                pDSubGearChattingLink.gear = pDFullHeroSubGear;
                m_pDChattingLink = pDSubGearChattingLink;
            }
            else if (csHeroObject.HeroObjectType == EnHeroObjectType.MountGear)
            {
                PDMountGearChattingLink pDMountGearChattingLink = new PDMountGearChattingLink();
                PDHeroMountGear pDHeroMountGear = new PDHeroMountGear();
                CsHeroMountGear csHeroMountGear = (CsHeroMountGear)csHeroObject;
                m_strItemName = csHeroMountGear.MountGear.Name;
                pDHeroMountGear.id = csHeroMountGear.Id;
                pDHeroMountGear.mountGearId = csHeroMountGear.MountGear.MountGearId;
                pDHeroMountGear.owned = csHeroMountGear.Owned;
                PDHeroMountGearOptionAttr[] apDHeroMountGearOptionAttr = new PDHeroMountGearOptionAttr[CsGameConfig.Instance.MountGearOptionAttrCount];

                for (int i = 0; i < CsGameConfig.Instance.MountGearOptionAttrCount; ++i)
                {
                    PDHeroMountGearOptionAttr pDHeroMountGearOptionAttr = new PDHeroMountGearOptionAttr();
                    pDHeroMountGearOptionAttr.index = csHeroMountGear.HeroMountGearOptionAttrList[i].Index;
                    pDHeroMountGearOptionAttr.grade = csHeroMountGear.HeroMountGearOptionAttrList[i].MountGearOptionAttrGrade.AttrGrade;
                    pDHeroMountGearOptionAttr.attrId = csHeroMountGear.HeroMountGearOptionAttrList[i].Attr.AttrId;
                    pDHeroMountGearOptionAttr.attrValueId = csHeroMountGear.HeroMountGearOptionAttrList[i].AttrValueInfo.AttrValueId;
                    apDHeroMountGearOptionAttr[i] = pDHeroMountGearOptionAttr;
                }

                pDHeroMountGear.optionAttrs = apDHeroMountGearOptionAttr;
                m_inputFieldSend.text = csHeroMountGear.MountGear.Name;
                pDMountGearChattingLink.gear = pDHeroMountGear;
                m_pDChattingLink = pDMountGearChattingLink;
            }

            OnClickChatOpen();
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestTaunted(CsChattingMessage csChattingMessage)
	{
		if (m_trChat.gameObject.activeSelf && csChattingMessage.ChattingType == (EnChattingType)m_nChannel)
        {
            CreateSystemMessage(csChattingMessage, null);
        }
        else if (m_trChat.gameObject.activeSelf && m_nChannel == 0)
        {
            CreateSystemMessage(csChattingMessage, null);
        }

        CreateMiniSystemMessage(csChattingMessage, null);

		if (m_scrollRectChat.verticalNormalizedPosition < 0.01f)
		{
			StartCoroutine(FixVerticalScrollPositionCoroutine(m_scrollRectChat));
		}
		else if (m_scrollRectChat.verticalScrollbar.size >= 0.9f)
		{
			StartCoroutine(FixVerticalScrollPositionCoroutine(m_scrollRectChat));
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventChattingMessageReceived(CsChattingMessage csChattingMessage)
    {
        if (m_trChat.gameObject.activeSelf && csChattingMessage.ChattingType == (EnChattingType)m_nChannel)
        {
            CreateChattingMessage(csChattingMessage);
        }
        else if (m_trChat.gameObject.activeSelf && m_nChannel == 0)
        {
            CreateChattingMessage(csChattingMessage);
        }

        CreateMiniChattingMessage(csChattingMessage);

        if (m_scrollRectChat.verticalNormalizedPosition < 0.01f)
        {
            StartCoroutine(FixVerticalScrollPositionCoroutine(m_scrollRectChat));
        }
        else if (m_scrollRectChat.verticalScrollbar.size >= 0.9f)
        {
            StartCoroutine(FixVerticalScrollPositionCoroutine(m_scrollRectChat));
        }

        if (csChattingMessage.Sender.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            //이전 채팅 내용 저장
            if (m_listPreChatMessage.Count >= CsGameConfig.Instance.ChattingSendHistoryMaxCount)
            {
                m_listPreChatMessage.RemoveAt(0);
            }
            m_listPreChatMessage.Add(csChattingMessage);
            m_nPreChat = m_listPreChatMessage.Count - 1;
        }

        DisplayOneToOne();

    }

	//---------------------------------------------------------------------------------------------------
	void OnEventSystemMessage(CsSystemMessage csSystemMessage)
	{
		string strMessage = GetSystemMessageString(csSystemMessage);

		if (!m_bSendingScheduleNotice)
			CreateFloatingMessage(m_trSystemWorld, strMessage);

		CsChattingMessage csChattingMessage = new CsChattingMessage(strMessage);
		CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventScheduleNotice(string strMessage)
	{
		m_bSendingScheduleNotice = true;
		CreateFloatingMessage(m_trSystemWorld, strMessage);

		CsChattingMessage csChattingMessage = new CsChattingMessage(strMessage);
		CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        OnClickChatClose();

        if (m_goPopupItemInfo != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Center);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //길드 알림
    void OnEventGuildDailyObjectiveNoticeEvent(CsChattingMessage csChattingMessage)
    {
        if (m_trChat.gameObject.activeSelf && csChattingMessage.ChattingType == (EnChattingType)m_nChannel)
        {
            CreateSystemMessage(csChattingMessage, () => OnClickGuildContent(csChattingMessage.ContentId));
        }
        else if (m_trChat.gameObject.activeSelf && m_nChannel == 0)
        {
            CreateSystemMessage(csChattingMessage, () => OnClickGuildContent(csChattingMessage.ContentId));
        }

        CreateMiniSystemMessage(csChattingMessage, () => OnClickGuildContent(csChattingMessage.ContentId));
    }

    //---------------------------------------------------------------------------------------------------
    //길드 미션을 통한 길드 가입
    void OnEventGuildSpiritAnnounced(CsChattingMessage csChattingMessage)
    {
        if (m_trChat.gameObject.activeSelf && csChattingMessage.ChattingType == (EnChattingType)m_nChannel)
        {
            CreateSystemMessage(csChattingMessage, () => OnClickGuildApply(csChattingMessage.GuildId));
        }
        else if (m_trChat.gameObject.activeSelf && m_nChannel == 0)
        {
            CreateSystemMessage(csChattingMessage, () => OnClickGuildApply(csChattingMessage.GuildId));
        }

        CreateMiniSystemMessage(csChattingMessage, () => OnClickGuildApply(csChattingMessage.GuildId));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBlessingBuffStarted(CsChattingMessage csChattingMessage)
    {
        if (m_trChat.gameObject.activeSelf && csChattingMessage.ChattingType == (EnChattingType)m_nChannel)
        {
            CreateSystemMessage(csChattingMessage, OnClickGuildBlessingBuff);
        }
        else if (m_trChat.gameObject.activeSelf && m_nChannel == 0)
        {
            CreateSystemMessage(csChattingMessage, OnClickGuildBlessingBuff);
        }

        CreateMiniSystemMessage(csChattingMessage, OnClickGuildBlessingBuff);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBlessingBuffEnded(CsChattingMessage csChattingMessage)
    {
        if (m_trChat.gameObject.activeSelf && csChattingMessage.ChattingType == (EnChattingType)m_nChannel)
        {
            CreateSystemMessage(csChattingMessage, null);
        }
        else if (m_trChat.gameObject.activeSelf && m_nChannel == 0)
        {
            CreateSystemMessage(csChattingMessage, null);
        }

        CreateMiniSystemMessage(csChattingMessage, null);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsChattingMessage csChattingMessage = CsUIData.Instance.ChattingMessageList.Find(a => a.HeroId == guidHeroId);

        if (csChattingMessage != null)
        {
            UpdateChattingMessage(csChattingMessage);
        }

        CsChattingMessage csChattingMessageOneToOne = CsUIData.Instance.OneToOneList.Find(a => a.HeroId == guidHeroId);

        if (csChattingMessageOneToOne != null)
        {
            UpdateOneToOne(csChattingMessageOneToOne);
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    //채널에 맞는 채팅목록에서 표시해줄 메시지 처리
    void DisplayChanneMessage()
    {
        List<CsChattingMessage> listChattingMessage = CsUIData.Instance.ChattingMessageList;

        if (m_nChannel == 0)
        {
            for (int i = 0; i < listChattingMessage.Count; ++i)
            {
                switch (listChattingMessage[i].NoticeType)
                {
                    case EnNoticeType.None:
                        CreateChattingMessage(listChattingMessage[i]);
                        break;

                    case EnNoticeType.GuildApply:
                        CreateSystemMessage(listChattingMessage[i], () => OnClickGuildApply(listChattingMessage[i].GuildId));
                        break;

                    case EnNoticeType.GuildEvent:
                        CreateSystemMessage(listChattingMessage[i], () => OnClickGuildContent(listChattingMessage[i].ContentId));
                        break;

                    case EnNoticeType.Party:
                        break;

					case EnNoticeType.Taunting:

						if (listChattingMessage[i].HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
						{
							CreateSystemMessage(listChattingMessage[i], null);
						}
						else
						{
							CreateSystemMessage(listChattingMessage[i], () => OnClickTrueHeroQuestTaunted(
								listChattingMessage[i].ContinentId, listChattingMessage[i].NationId, listChattingMessage[i].TrueHeroPosition));
						}
						break;

                    case EnNoticeType.GuildBlessingBuff:

                        if (listChattingMessage[i].IsBlessingBuffRunning)
                        {
                            CreateSystemMessage(listChattingMessage[i], OnClickGuildBlessingBuff);
                        }
                        else
                        {
                            CreateSystemMessage(listChattingMessage[i], null);
                        }

                        break;

					case EnNoticeType.SystemMessage:
						CreateSystemChattingMessage(listChattingMessage[i]);
						break;

				}
            }
        }
        //1:1 대화목록에서 선택한 플레이어와의 대화 메시지
        else if ((EnChattingType)m_nChannel == EnChattingType.OneToOne)
        {
            for (int i = 0; i < listChattingMessage.Count; ++i)
            {
                if (m_guidTargetHeroId != Guid.Empty && listChattingMessage[i].ChattingType == EnChattingType.OneToOne)
                {
                    if (m_guidTargetHeroId == listChattingMessage[i].Sender.HeroId || m_guidTargetHeroId == listChattingMessage[i].Target.HeroId)
                    {
                        CreateChattingMessage(listChattingMessage[i]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < listChattingMessage.Count; ++i)
            {
                if (listChattingMessage[i].ChattingType == (EnChattingType)m_nChannel)
                {
                    switch (listChattingMessage[i].NoticeType)
                    {
                        case EnNoticeType.None:
                            CreateChattingMessage(listChattingMessage[i]);
                            break;

                        case EnNoticeType.GuildApply:
                            CreateSystemMessage(listChattingMessage[i], () => OnClickGuildApply(listChattingMessage[i].GuildId));
                            break;

                        case EnNoticeType.GuildEvent:
                            CreateSystemMessage(listChattingMessage[i], () => OnClickGuildContent(listChattingMessage[i].ContentId));
                            break;

                        case EnNoticeType.Party:
                            break;

						case EnNoticeType.Taunting:

							if (listChattingMessage[i].HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
							{
								CreateSystemMessage(listChattingMessage[i], null);
							}
							else
							{
								CreateSystemMessage(listChattingMessage[i], () =>
									OnClickTrueHeroQuestTaunted(listChattingMessage[i].ContinentId, listChattingMessage[i].NationId, listChattingMessage[i].TrueHeroPosition));
							}

							break;

                        case EnNoticeType.GuildBlessingBuff:

                            if (listChattingMessage[i].IsBlessingBuffRunning)
                            {
                                CreateSystemMessage(listChattingMessage[i], OnClickGuildBlessingBuff);
                            }
                            else
                            {
                                CreateSystemMessage(listChattingMessage[i], null);
                            }

                            break;
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //1:1 대화목록
    void DisplayOneToOne()
    {
        List<CsChattingMessage> listOneToOneMessage = CsUIData.Instance.OneToOneList;

        bool bNoOneToOne = false;

        if ((EnChattingType)m_nChannel == EnChattingType.OneToOne)
        {
            for (int i = 0; i < listOneToOneMessage.Count; ++i)
            {
                CreateOneToOne(listOneToOneMessage[i]);

                if (!bNoOneToOne)
                    bNoOneToOne = true;
            }
        }
        else
        {
            return;
        }

        if (bNoOneToOne)
        {
            m_textNoOneToOneChatting.gameObject.SetActive(false);
        }
        else
        {
            m_textNoOneToOneChatting.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //1:1 대화목록 생성
    void CreateOneToOne(CsChattingMessage csChattingMessage)
    {
        Transform trOneToOne = m_trOneToOneChattingListContent.Find(csChattingMessage.Sender.HeroId.ToString());

        if (trOneToOne == null)
        {
            trOneToOne = Instantiate(m_goButtonOneToOne, m_trOneToOneChattingListContent).transform;
            trOneToOne.name = csChattingMessage.Sender.HeroId.ToString();
        }

        UpdateOneToOne(csChattingMessage);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateOneToOne(CsChattingMessage csChattingMessage)
    {
        Transform trOneToOne = m_trOneToOneChattingListContent.Find(csChattingMessage.Sender.HeroId.ToString());

        if (trOneToOne == null)
        {
            return;
        }
        else
        {
            Button buttonOneToOne = trOneToOne.GetComponent<Button>();
            buttonOneToOne.onClick.RemoveAllListeners();
            buttonOneToOne.onClick.AddListener(() => { OnClickOneToOneEnter(csChattingMessage.Sender.HeroId); });
            buttonOneToOne.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image imageEmblem = trOneToOne.Find("ImageEmblem").GetComponent<Image>();
            imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csChattingMessage.Sender.Job.JobId);

            Text textName = trOneToOne.Find("TextName").GetComponent<Text>();
            textName.text = csChattingMessage.Sender.Name;
            CsUIData.Instance.SetFont(textName);

            Text textChat = trOneToOne.Find("TextChat").GetComponent<Text>();
            if (csChattingMessage.ChattingMessage.Length > 20)
            {
                textChat.text = csChattingMessage.ChattingMessage.Substring(0, 20) + "....";
            }
            else
            {
                textChat.text = csChattingMessage.ChattingMessage;
            }

            CsUIData.Instance.SetFont(textChat);
        }
    }

	//---------------------------------------------------------------------------------------------------
	string GetSystemMessageString(CsSystemMessage csSystemMessage)
	{
		string strMessage = null;

		switch (csSystemMessage.SystemMessageInfo.SystemMessageId)
		{
			case EnSystemMessage.MainGearAcquirement:
				CsSystemMessageMainGearAcquirement csSystemMessageMainGearAcquirement = (CsSystemMessageMainGearAcquirement)csSystemMessage;
				strMessage = string.Format(csSystemMessage.SystemMessageInfo.Message, csSystemMessage.Nation.Name, csSystemMessage.HeroName, csSystemMessageMainGearAcquirement.MainGear.MainGearGrade.ColorCode, csSystemMessageMainGearAcquirement.MainGear.Name);
				break;

			case EnSystemMessage.MainGearEnchantment:
				CsSystemMessageMainGearEnchantment csSystemMessageMaingearEnchantment = (CsSystemMessageMainGearEnchantment)csSystemMessage;
				strMessage = string.Format(csSystemMessage.SystemMessageInfo.Message, csSystemMessage.Nation.Name, csSystemMessage.HeroName, csSystemMessageMaingearEnchantment.MainGear.MainGearGrade.ColorCode, csSystemMessageMaingearEnchantment.MainGear.Name, csSystemMessageMaingearEnchantment.EnchantLevel);
				break;

			case EnSystemMessage.CreatureCardAcquirement:
				CsSystemMessageCreatureCardAcquirement csSystemMessageCreatureCardAcquirement = (CsSystemMessageCreatureCardAcquirement)csSystemMessage;
				strMessage = string.Format(csSystemMessage.SystemMessageInfo.Message, csSystemMessage.Nation.Name, csSystemMessage.HeroName, csSystemMessageCreatureCardAcquirement.CreatureCard.CreatureCardGrade.ColorCode, csSystemMessageCreatureCardAcquirement.CreatureCard.Name);
				break;

			case EnSystemMessage.CreatureAcquirement:
				CsSystemMessageCreatureAcquirement csSystemMessageCreatureAcquirement = (CsSystemMessageCreatureAcquirement)csSystemMessage;
				strMessage = string.Format(csSystemMessage.SystemMessageInfo.Message, csSystemMessage.Nation.Name, csSystemMessage.HeroName, csSystemMessageCreatureAcquirement.Creature.CreatureGrade.ColorCode, csSystemMessageCreatureAcquirement.Creature.CreatureCharacter.Name);
				break;

			case EnSystemMessage.CreatureInjection:
				CsSystemMessageCreatureInjection csSystemMessageCreatureInjection = (CsSystemMessageCreatureInjection)csSystemMessage;
				strMessage = string.Format(csSystemMessage.SystemMessageInfo.Message, csSystemMessage.Nation.Name, csSystemMessage.HeroName, csSystemMessageCreatureInjection.Creature.CreatureGrade.ColorCode, csSystemMessageCreatureInjection.InjectionLevel);
				break;

			case EnSystemMessage.CostumeEnchantment:
				CsSystemMessageCostumeEnchantment csSystemMessageCostumeEnchantment = (CsSystemMessageCostumeEnchantment)csSystemMessage;
				strMessage = string.Format(csSystemMessage.SystemMessageInfo.Message, csSystemMessage.Nation.Name, csSystemMessage.HeroName, csSystemMessageCostumeEnchantment.Costume.Name, csSystemMessageCostumeEnchantment.EnchantLevel);
				break;

			default:
				strMessage = null;
				break;
		}

		return strMessage;
	}

	//---------------------------------------------------------------------------------------------------
	void CreateFloatingMessage(Transform trContent, string strMessage, bool bIsWorldChat = false)
	{
        if (trContent == null) return;
        if (trContent.gameObject.activeSelf)
        {
            StopCoroutine(m_coroutineWorldChatting);
            trContent.gameObject.SetActive(false);
        }

		string strChatContent;
		float flWorldTime;

		if (bIsWorldChat)
		{
			strChatContent = strMessage;
			flWorldTime = CsGameConfig.Instance.WorldChattingDisplayDuration;
		}
		else
		{
			strChatContent = m_strSpacing + strMessage + m_strSpacing;
			flWorldTime = CsGameConfig.Instance.NoticeBoardDisplayDuration;
		}

		Text textWorld = trContent.Find("Scroll View/Viewport/Content/Text").GetComponent<Text>();
		textWorld.text = strChatContent;
		CsUIData.Instance.SetFont(textWorld);

		m_coroutineWorldChatting = StartCoroutine(WorldChatTextScrollPositionCoroutine(trContent, flWorldTime));
	}

    //---------------------------------------------------------------------------------------------------
    void CreateMiniChattingMessage(CsChattingMessage csChattingMessage)
    {
        if (csChattingMessage.ChattingType == EnChattingType.World)
        {
			CreateFloatingMessage(m_trWorld, string.Format(CsConfiguration.Instance.GetString("A34_TXT_01003"), csChattingMessage.ChatType.ColorCode, csChattingMessage.ChatType.Name
			    , csChattingMessage.Sender.Name, csChattingMessage.ChattingMessage), true);
        }
        else
        {
            //미니 메시지
            Transform trSimpleMessage = Instantiate(m_goMessageItemMiniSimple, m_trMiniChatContent).transform;

            if (csChattingMessage.ChattingLink == null)
            {
                Transform trTextArea = trSimpleMessage.Find("TextArea");
                Text textChatMsg = Instantiate(m_goMiniChatMsg, trTextArea).GetComponent<Text>();
                textChatMsg.text = string.Format(CsConfiguration.Instance.GetString("A34_TXT_01003"), csChattingMessage.ChatType.ColorCode, csChattingMessage.ChatType.Name,
                    csChattingMessage.Sender.Name, csChattingMessage.ChattingMessage);
                CsUIData.Instance.SetFont(textChatMsg);
            }
            else
            {
                Transform trTextArea = trSimpleMessage.Find("TextArea");
                ItemChatGroup(csChattingMessage, trTextArea, m_nMiniChatMaxSzie, true);
            }

            //가장 오래된 채팅 삭제
            if (m_trMiniChatContent.childCount > 100)
            {
                DestroyImmediate(m_trMiniChatContent.GetChild(0).gameObject);
            }

            StartCoroutine(FixVerticalScrollPositionCoroutine(m_scrollRectMiniChat));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateMiniSystemMessage(CsChattingMessage csChattingMessage, UnityAction unityAction)
    {
		//미니 메시지
		Transform trSimpleMessage = Instantiate(m_goMessageItemMiniSimple, m_trMiniChatContent).transform;
		Transform trTextArea = trSimpleMessage.Find("TextArea");
		SystemChatGroup(csChattingMessage, trTextArea, m_nMiniChatMaxSzie, true, unityAction);

		//가장 오래된 채팅 삭제
		if (m_trMiniChatContent.childCount > 100)
		{
			DestroyImmediate(m_trMiniChatContent.GetChild(0).gameObject);
		}

		StartCoroutine(FixVerticalScrollPositionCoroutine(m_scrollRectMiniChat));
    }

	//---------------------------------------------------------------------------------------------------
    IEnumerator FixVerticalScrollPositionCoroutine(ScrollRect scrollRect)
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator WorldChatTextScrollPositionCoroutine(Transform trContent, float flWorldTime)
    {
		trContent.gameObject.SetActive(true);

        float flWatingTime = flWorldTime + Time.realtimeSinceStartup;
		ScrollRect scrollRectWorld = trContent.Find("Scroll View").GetComponent<ScrollRect>();
        scrollRectWorld.horizontalNormalizedPosition = 0;

        while (true)
        {
            scrollRectWorld.horizontalNormalizedPosition = 1 - ((flWatingTime - Time.realtimeSinceStartup) / flWorldTime);

            if (flWatingTime - Time.realtimeSinceStartup <= 0)
            {
				trContent.gameObject.SetActive(false);
				m_bSendingScheduleNotice = false;
                yield break;
            }
            yield return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateChattingMessage(CsChattingMessage csChattingMessage)
    {
        //자신이 보낸 메시지
        if (csChattingMessage.Sender.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            Transform trMyMessage = Instantiate(m_goMessageItemMy, m_trChatContent).transform;
            
            Text textInfo = trMyMessage.Find("ImageBackGround/Chat/TextInfo").GetComponent<Text>();
            textInfo.text = string.Format(CsConfiguration.Instance.GetString("A34_TXT_01002"), csChattingMessage.DateTime.Hour.ToString("00"), csChattingMessage.DateTime.Minute.ToString("00"),
                csChattingMessage.Sender.Name, csChattingMessage.ChatType.ColorCode, csChattingMessage.ChatType.Name);
            CsUIData.Instance.SetFont(textInfo);

            //아이템이 없는 경우
            if (csChattingMessage.ChattingLink == null)
            {
                Transform trTextArea = trMyMessage.Find("ImageBackGround/Chat/TextArea");
                Text textChatMsg = Instantiate(m_goChatMsg, trTextArea).GetComponent<Text>();
                textChatMsg.alignment = TextAnchor.UpperRight;
                textChatMsg.text = csChattingMessage.ChattingMessage;
                CsUIData.Instance.SetFont(textChatMsg);
            }
            else
            {
                Transform trTextArea = trMyMessage.Find("ImageBackGround/Chat/TextArea");
                ItemChatGroup(csChattingMessage, trTextArea, m_nMyChatMaxSize);
            }
        }
        //다른 유저가 보낸 메시지
        else
        {
            Transform trMessage = Instantiate(m_goMessageItem, m_trChatContent).transform;
            trMessage.name = "Message" + csChattingMessage.HeroId;

            Image imageEmblem = trMessage.Find("ImageBackGround/Emblem/ImageEmblem").GetComponent<Image>();
            imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csChattingMessage.Sender.Job.JobId);

            Button buttonInfo = imageEmblem.GetComponent<Button>();
            buttonInfo.onClick.RemoveAllListeners();
            buttonInfo.onClick.AddListener(() => { OnClickOpenPopupUserReference(csChattingMessage.Sender); });
            buttonInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image imageNation = imageEmblem.transform.Find("ImageNation").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_nation" + csChattingMessage.Sender.Nation.NationId);

            imageEmblem.transform.Find("ImageEmperor").gameObject.SetActive(csChattingMessage.Sender.NoblesseId == 1);

            Text textInfo = trMessage.Find("ImageBackGround/Chat/TextInfo").GetComponent<Text>();
            textInfo.text = string.Format(CsConfiguration.Instance.GetString("A34_TXT_01001"), csChattingMessage.ChatType.ColorCode, csChattingMessage.ChatType.Name,
                csChattingMessage.Sender.Name, csChattingMessage.DateTime.Hour.ToString("00"), csChattingMessage.DateTime.Minute.ToString("00"));
            //아이템이 없는 경우
            if (csChattingMessage.ChattingLink == null)
            {
                Transform trTextArea = trMessage.Find("ImageBackGround/Chat/TextArea");
                Text textChatMsg = Instantiate(m_goChatMsg, trTextArea).GetComponent<Text>();
                textChatMsg.text = csChattingMessage.ChattingMessage;
                CsUIData.Instance.SetFont(textChatMsg);
            }
            else
            {
                Transform trTextArea = trMessage.Find("ImageBackGround/Chat/TextArea");
                ItemChatGroup(csChattingMessage, trTextArea, m_nChatMaxSize);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void CreateSystemChattingMessage(CsChattingMessage csChattingMessage)
	{
		Transform trMessage = Instantiate(m_goMessageItem, m_trChatContent).transform;
		trMessage.name = "SystemMessage";

		GameObject goEmblem = trMessage.Find("ImageBackGround/Emblem").gameObject;
		goEmblem.SetActive(false);

		VerticalLayoutGroup verticalLayoutGroup = trMessage.Find("ImageBackGround/Chat").GetComponent<VerticalLayoutGroup>();
		verticalLayoutGroup.padding.left = 10;

		Text textInfo = trMessage.Find("ImageBackGround/Chat/TextInfo").GetComponent<Text>();
		textInfo.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_CHAT_SYSTEM"), CsGameData.Instance.MyHeroInfo.CurrentDateTime.Hour.ToString("00"), CsGameData.Instance.MyHeroInfo.CurrentDateTime.Minute.ToString("00"));

		Transform trTextArea = trMessage.Find("ImageBackGround/Chat/TextArea");
		Text textChatMsg = Instantiate(m_goChatMsg, trTextArea).GetComponent<Text>();
		textChatMsg.text = csChattingMessage.ChattingMessage;
		CsUIData.Instance.SetFont(textChatMsg);
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateChattingMessage(CsChattingMessage csChattingMessage)
    {
        if (csChattingMessage.Sender.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            return;
        }
        else
        {
            Transform trMessage = m_trChatContent.Find("Message" + csChattingMessage.HeroId);

            if (trMessage == null)
            {
                return;
            }
            else
            {
                Image imageEmblem = trMessage.Find("ImageBackGround/Emblem/ImageEmblem").GetComponent<Image>();
                imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csChattingMessage.Sender.Job.JobId);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void CreateSystemMessage(CsChattingMessage csChattingMessage, UnityAction unityAction)
    {
        Transform trMyMessage = Instantiate(m_goMessageItemSimple, m_trChatContent).transform;
        Transform trTextArea = trMyMessage.Find("TextArea");
        SystemChatGroup(csChattingMessage, trTextArea, m_nSystemMaxSize, false, unityAction);
    }

    //---------------------------------------------------------------------------------------------------
    void ItemChatGroup(CsChattingMessage csChattingMessage, Transform trParent, int nMaxSize, bool bMiniChat = false)
    {
        m_trChatGroup = CreateChatPrefab(m_goChatGroup, trParent);
        int nSize = nMaxSize;

        if (bMiniChat)
        {
            string strFormat = string.Format(CsConfiguration.Instance.GetString("A34_TXT_01003"), csChattingMessage.ChatType.ColorCode,
                csChattingMessage.ChatType.Name, csChattingMessage.Sender.Name, csChattingMessage.Messages[0]);
            nSize = ChattingReSize(trParent, strFormat, nSize, nMaxSize, bMiniChat);
        }
        else
        {
            nSize = ChattingReSize(trParent, csChattingMessage.Messages[0], nSize, nMaxSize);
        }

        nSize = ChattingReSize(trParent, csChattingMessage.GearName, nSize, nMaxSize, bMiniChat, csChattingMessage.ChattingLink);
        ChattingReSize(trParent, csChattingMessage.Messages[1], nSize, nMaxSize);
    }

    //---------------------------------------------------------------------------------------------------
    void SystemChatGroup(CsChattingMessage csChattingMessage, Transform trParent, int nMaxSize, bool bMiniChat, UnityAction unityAction)
    {
        m_trChatGroup = CreateChatPrefab(m_goChatGroup, trParent);
        int nSize = nMaxSize;
        string strNotice = "";
        switch (csChattingMessage.NoticeType)
        {
            case EnNoticeType.GuildApply:
                strNotice += string.Format(CsConfiguration.Instance.GetString("A71_TXT_01001"), csChattingMessage.GuildName, csChattingMessage.HeroName, CsGameData.Instance.GetContinent(csChattingMessage.ContinentId).Name);
                break;

            case EnNoticeType.GuildEvent:
                strNotice += string.Format(CsConfiguration.Instance.GetString("A60_TXT_01003"), CsGameData.Instance.GetGuildContent(csChattingMessage.ContentId).Name);
                break;

            case EnNoticeType.Party:
                break;

			case EnNoticeType.Taunting:
				strNotice += string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.ChattingMessage, CsGameData.Instance.GetNation(csChattingMessage.NationId).Name, csChattingMessage.HeroName);
				break;

            case EnNoticeType.GuildBlessingBuff:

                if (csChattingMessage.IsBlessingBuffRunning)
                {
                    strNotice += CsConfiguration.Instance.GetString("A131_TXT_00001");
                }
                else
                {
                    strNotice += CsConfiguration.Instance.GetString("A131_TXT_00003");
                }

                break;
        }

        string strMessage = string.Format(CsConfiguration.Instance.GetString("PUBLIC_SYSTEM_MS"), csChattingMessage.ChatType.ColorCode, csChattingMessage.ChatType.Name, strNotice);

        nSize = ChattingReSize(trParent, strMessage, nSize, nMaxSize, bMiniChat);

        switch (csChattingMessage.NoticeType)
        {
            case EnNoticeType.GuildApply:
                ChattingReSize(trParent, CsConfiguration.Instance.GetString("A71_TXT_01002"), nSize, nMaxSize, bMiniChat, () => OnClickGuildApply(csChattingMessage.GuildId));
                break;

            case EnNoticeType.GuildEvent:
                ChattingReSize(trParent, CsConfiguration.Instance.GetString("A60_TXT_01004"), nSize, nMaxSize, bMiniChat, () => OnClickGuildContent(csChattingMessage.ContentId));
                break;

            case EnNoticeType.Party:
                break;

			case EnNoticeType.Taunting:
				if (csChattingMessage.HeroId != CsGameData.Instance.MyHeroInfo.HeroId)
				{
					ChattingReSize(trParent, CsConfiguration.Instance.GetString("A111_TXT_00001"), nSize, nMaxSize, bMiniChat, () =>
					OnClickTrueHeroQuestTaunted(csChattingMessage.ContinentId, CsGameData.Instance.MyHeroInfo.Nation.NationId, csChattingMessage.TrueHeroPosition));
				}

				break;

            case EnNoticeType.GuildBlessingBuff:

                if (csChattingMessage.IsBlessingBuffRunning)
                {
                    ChattingReSize(trParent, CsConfiguration.Instance.GetString("A131_TXT_00002"), nSize, nMaxSize, bMiniChat, OnClickGuildBlessingBuff);
                }
                else
                {
                    //
                }

                break;
        }

    }
    //---------------------------------------------------------------------------------------------------
    Transform CreateChatPrefab(GameObject objUnit, Transform trParent)
    {
        Transform trPrefab = Instantiate(objUnit, trParent).transform;
        trPrefab.name = objUnit.name;
        return trPrefab;
    }

    //---------------------------------------------------------------------------------------------------
    void RemoveAllChatMessage()
    {
        int nCount = m_trChatContent.childCount;

        for (int i = 0; i < nCount; ++i)
        {
            DestroyImmediate(m_trChatContent.GetChild(0).gameObject);
        }

        nCount = m_trOneToOneChattingListContent.childCount;

        for (int i = 0; i < nCount; ++i)
        {
            DestroyImmediate(m_trOneToOneChattingListContent.GetChild(0).gameObject);
        }

    }

    //---------------------------------------------------------------------------------------------------
    int ChattingReSize(Transform trArea, string strChatting, int nSize, int nMaxSize, bool bMiniChat = false, PDChattingLink pDChattingLink = null)
    {
        Text textMsg;
        string strReturn = string.Empty;

        if (pDChattingLink != null)
        {
            if (bMiniChat)
            {
                textMsg = CreateChatPrefab(m_goMiniItemMsg, m_trChatGroup).GetComponent<Text>();
            }
            else
            {
                textMsg = CreateChatPrefab(m_goItemMsg, m_trChatGroup).GetComponent<Text>();
            }

            textMsg.text = strChatting;
            CsUIData.Instance.SetFont(textMsg);

            if (nSize > textMsg.preferredWidth)
            {
                Button buttonItemMsg = textMsg.GetComponent<Button>();
                buttonItemMsg.onClick.RemoveAllListeners();
                buttonItemMsg.onClick.AddListener(() => { OnClickItemInfo(pDChattingLink); });
                buttonItemMsg.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                CsUIData.Instance.SetFont(textMsg);
                return nSize - (int)textMsg.preferredWidth;
            }
            else
            {
                m_trChatGroup = CreateChatPrefab(m_goChatGroup, trArea);
                textMsg.transform.SetParent(m_trChatGroup);
                textMsg.transform.localScale = Vector3.one;
                Button buttonItemMsg = textMsg.GetComponent<Button>();
                buttonItemMsg.onClick.RemoveAllListeners();
                buttonItemMsg.onClick.AddListener(() => { OnClickItemInfo(pDChattingLink); });
                buttonItemMsg.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                CsUIData.Instance.SetFont(textMsg);
                return nMaxSize - (int)textMsg.preferredWidth;
            }
        }
        else
        {
            if (bMiniChat)
            {
                textMsg = CreateChatPrefab(m_goMiniChatMsg, m_trChatGroup).GetComponent<Text>();
            }
            else
            {
                textMsg = CreateChatPrefab(m_goChatMsg, m_trChatGroup).GetComponent<Text>();
            }
            textMsg.text = strChatting;
            CsUIData.Instance.SetFont(textMsg);
        }

        while (textMsg.preferredWidth > nSize)
        {
            strReturn += textMsg.text[textMsg.text.Length - 1];
            textMsg.text = textMsg.text.Substring(0, textMsg.text.Length - 1);
        }

        if (!string.IsNullOrEmpty(strReturn))
        {
            char[] achArray = strReturn.ToCharArray();
            Array.Reverse(achArray);
            strReturn = new string(achArray);
            m_trChatGroup = CreateChatPrefab(m_goChatGroup, trArea);
            nSize = nMaxSize;
            return ChattingReSize(trArea, strReturn, nSize, nMaxSize, bMiniChat);
        }
        else
        {
            return nSize - (int)textMsg.preferredWidth;
        }
    }

    //---------------------------------------------------------------------------------------------------
    int ChattingReSize(Transform trArea, string strChatting, int nSize, int nMaxSize, bool bMiniChat, UnityAction unityAction)
    {
        Text textMsg;
        string strReturn = string.Empty;

        if (unityAction != null)
        {
            if (bMiniChat)
            {
                textMsg = CreateChatPrefab(m_goMiniItemMsg, m_trChatGroup).GetComponent<Text>();
            }
            else
            {
                textMsg = CreateChatPrefab(m_goItemMsg, m_trChatGroup).GetComponent<Text>();
            }

            textMsg.text = strChatting;
            CsUIData.Instance.SetFont(textMsg);

            if (nSize > textMsg.preferredWidth)
            {
                Button buttonItemMsg = textMsg.GetComponent<Button>();
                buttonItemMsg.onClick.RemoveAllListeners();
                buttonItemMsg.onClick.AddListener(unityAction);
                buttonItemMsg.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                CsUIData.Instance.SetFont(textMsg);
                return nSize - (int)textMsg.preferredWidth;
            }
            else
            {
                m_trChatGroup = CreateChatPrefab(m_goChatGroup, trArea);
                textMsg.transform.SetParent(m_trChatGroup);
                textMsg.transform.localScale = Vector3.one;
                Button buttonItemMsg = textMsg.GetComponent<Button>();
                buttonItemMsg.onClick.RemoveAllListeners();
                buttonItemMsg.onClick.AddListener(unityAction);
                buttonItemMsg.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                CsUIData.Instance.SetFont(textMsg);
                return nMaxSize - (int)textMsg.preferredWidth;
            }
        }
        else
        {
            if (bMiniChat)
            {
                textMsg = CreateChatPrefab(m_goMiniChatMsg, m_trChatGroup).GetComponent<Text>();
            }
            else
            {
                textMsg = CreateChatPrefab(m_goChatMsg, m_trChatGroup).GetComponent<Text>();
            }

            textMsg.text = strChatting;
            CsUIData.Instance.SetFont(textMsg);
        }

        while (textMsg.preferredWidth > nSize)
        {
            strReturn += textMsg.text[textMsg.text.Length - 1];
            textMsg.text = textMsg.text.Substring(0, textMsg.text.Length - 1);
        }

        if (!string.IsNullOrEmpty(strReturn))
        {
            char[] achArray = strReturn.ToCharArray();
            Array.Reverse(achArray);
            strReturn = new string(achArray);
            m_trChatGroup = CreateChatPrefab(m_goChatGroup, trArea);
            nSize = nMaxSize;
            return ChattingReSize(trArea, strReturn, nSize, nMaxSize);
        }
        else
        {
            return nSize - (int)textMsg.preferredWidth;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SendMessage()
    {
        if (m_bSend)
        {
            for (int i = 0; i < CsGameData.Instance.BanWordList.Count; ++i)
            {
                if (m_inputFieldSend.text.Contains(CsGameData.Instance.BanWordList[i].Word))
                {
                    m_inputFieldSend.text = string.Empty;
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A34_TXT_02006"));
                    return;
                }
            }

            //입력값 체크
            if (!string.IsNullOrEmpty(m_inputFieldSend.text))
            {
                //길이 초과시 잘라내기
                if (m_inputFieldSend.text.Length > CsGameConfig.Instance.ChattingMaxLength)
                    m_inputFieldSend.text = m_inputFieldSend.text.Substring(0, CsGameConfig.Instance.ChattingMaxLength);

                //아이템 링크가 있을때
                if (m_pDChattingLink != null)
                {
                    if (m_inputFieldSend.text.Contains(m_strItemName))
                    {
                        string[] asMessage = new string[2];
                        int nItemIndex = m_inputFieldSend.text.IndexOf(m_strItemName);

                        //아이템 링크 앞에 문자가 없는 경우
                        if (nItemIndex == 0)
                        {
                            asMessage[0] = string.Empty;
                            asMessage[1] = m_inputFieldSend.text.Substring(m_strItemName.Length);
                        }
                        else
                        {
                            asMessage[0] = m_inputFieldSend.text.Substring(0, nItemIndex);
                            asMessage[1] = m_inputFieldSend.text.Substring(nItemIndex + m_strItemName.Length);
                        }

                        CsCommandEventManager.Instance.SendChattingMessageSend(m_nChannel, asMessage, m_pDChattingLink, m_guidTargetHeroId);
                    }
                    else
                    {
                        //아이템 텍스트가 깨진경우
                        string[] asMessage = { m_inputFieldSend.text };
                        CsCommandEventManager.Instance.SendChattingMessageSend(m_nChannel, asMessage, null, m_guidTargetHeroId);
                    }
                }
                else
                {
                    string[] asMessage = { m_inputFieldSend.text };
                    CsCommandEventManager.Instance.SendChattingMessageSend(m_nChannel, asMessage, null, m_guidTargetHeroId);
                }

                m_pDChattingLink = null;
                m_inputFieldSend.text = string.Empty;
                m_flTime = Time.time;
                m_bSend = false;
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A34_TXT_02005"));
            }
        }
        //전송 대기 시간
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A34_TXT_02001"), m_nChattingMinInterval));
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsHeroObject csHeroObject)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csHeroObject);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csitem)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csitem);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsHeroObject csHeroObject)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        switch (csHeroObject.HeroObjectType)
        {
            case EnHeroObjectType.MainGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, (CsHeroMainGear)csHeroObject, false, false);
                break;
            case EnHeroObjectType.SubGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, (CsHeroSubGear)csHeroObject, false);
                break;
            case EnHeroObjectType.MountGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, (CsHeroMountGear)csHeroObject, false, false);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csitem)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, false, 0, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }
}
