using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-18)
//---------------------------------------------------------------------------------------------------

public class CsPopupUserReference : MonoBehaviour
{
    public event Delegate EventCloseUserReference;

    Transform m_trPopupList;

    CsFriend m_csFriend = null;
    CsHeroBase m_csHeroBase = null;
    CsGuildMember m_csGuildMember = null;
    CsNationNoblesseInstance m_csNationNoblesseInstance = null;

    Image m_imageEmblem;
    Text m_textUserName;
    Text m_textUserNumber;

    Button m_buttonReference;
    Button m_buttonFriendAdd;
    Button m_buttonFriendDelete;
    Button m_buttonParty;
    Button m_buttonPresent;
    Button m_buttonOneToOne;
    Button m_buttonBlackList;
    Button m_buttonGuildInvite;
    Button m_buttonGuildMasterTransfer;
    Button m_buttonGuildSubMasterAppoint;
    Button m_buttonGuildLoadAppoint;
    Button m_buttonGuildDismissAppointment;
    Button m_buttonGuildMemberBanish;
    Button m_buttonNationNoblessDismiss;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyInvite += OnEventPartyInvite;
        CsGuildManager.Instance.EventGuildInvite += OnEventGuildInvite;
        CsGuildManager.Instance.EventGuildMasterTransfer += OnEventGuildMasterTransfer;
        CsGuildManager.Instance.EventGuildAppoint += OnEventGuildAppoint;
        CsGuildManager.Instance.EventGuildMemberBanish += OnEventGuildMemberBanish;

        // 친구
        CsFriendManager.Instance.EventFriendApplicationAccepted += OnEventFriendApplicationAccepted;
        CsFriendManager.Instance.EventBlacklistEntryAdd += OnEventBlacklistEntryAdd;
        CsFriendManager.Instance.EventFriendDelete += OnEventFriendDelete;

        // 국가 관직 해임
        CsGameEventUIToUI.Instance.EventNationNoblesseDismiss += OnEventNationNoblesseDismiss;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyInvite -= OnEventPartyInvite;
        CsGuildManager.Instance.EventGuildInvite -= OnEventGuildInvite;
        CsGuildManager.Instance.EventGuildMasterTransfer -= OnEventGuildMasterTransfer;
        CsGuildManager.Instance.EventGuildAppoint -= OnEventGuildAppoint;
        CsGuildManager.Instance.EventGuildMemberBanish -= OnEventGuildMemberBanish;

        // 친구
        CsFriendManager.Instance.EventFriendApplicationAccepted -= OnEventFriendApplicationAccepted;
        CsFriendManager.Instance.EventBlacklistEntryAdd -= OnEventBlacklistEntryAdd;
        CsFriendManager.Instance.EventFriendDelete -= OnEventFriendDelete;

        // 국가 관직 해임
        CsGameEventUIToUI.Instance.EventNationNoblesseDismiss -= OnEventNationNoblesseDismiss;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseUserReference()
    {
        if (EventCloseUserReference != null)
        {
            EventCloseUserReference();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        if (m_csHeroBase != null)
        {
            CsCommandEventManager.Instance.SendPartyInvite(m_csHeroBase.HeroId);
        }
        else if (m_csFriend != null)
        {
            CsCommandEventManager.Instance.SendPartyInvite(m_csFriend.Id);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvite(CsPartyInvitation partyInvitation)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04019"));
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvite()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("길드 초대"));
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMasterTransfer()
    {
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAppoint()
    {
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMemberBanish()
    {
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationNoblesseDismiss()
    {
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(System.Guid guidHeroId, int nJobId)
    {
        if (m_csHeroBase != null)
        {
            UpdateReference(m_csHeroBase);
        }
        else if (m_csFriend != null)
        {
            UpdateReference(m_csFriend);
        }
        else if (m_csGuildMember != null)
        {
            UpdateReference(m_csGuildMember);
        }
        else if (m_csNationNoblesseInstance != null)
        {
            UpdateReference(m_csNationNoblesseInstance);
        }
    }

    #region Friend

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationAccepted()
    {
        UpdateReference(m_csHeroBase);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendDelete()
    {
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBlacklistEntryAdd()
    {
        UpdateReference(m_csHeroBase);
    }

    #endregion Friend

    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOneToOneChat()
    {
        if (m_csHeroBase != null)
        {
            CsGameEventUIToUI.Instance.OnEventOpenOneToOneChat(m_csHeroBase.HeroId);
            OnEventCloseUserReference();
        }
        else if (m_csFriend != null)
        {
            CsGameEventUIToUI.Instance.OnEventOpenOneToOneChat(m_csFriend.Id);
            OnEventCloseUserReference();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickParty()
    {
        if (m_csHeroBase != null)
        {
            if (CsGameData.Instance.MyHeroInfo.Party == null)
            {
                CsCommandEventManager.Instance.SendPartyCreate();
            }
            else
            {
                List<CsPartyInvitation> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyInvitationList;

                for (int i = 0; i < listPartyApplication.Count; ++i)
                {
                    if (listPartyApplication[i].TargetId == m_csHeroBase.HeroId)
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04007"));
                        OnEventCloseUserReference();
                        return;
                    }
                }

                CsCommandEventManager.Instance.SendPartyInvite(m_csHeroBase.HeroId);
            }
        }
        else if (m_csFriend != null)
        {
            if (CsGameData.Instance.MyHeroInfo.Party == null)
            {
                CsCommandEventManager.Instance.SendPartyCreate();
            }
            else
            {
                List<CsPartyInvitation> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyInvitationList;

                for (int i = 0; i < listPartyApplication.Count; ++i)
                {
                    if (listPartyApplication[i].TargetId == m_csFriend.Id)
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04007"));
                        OnEventCloseUserReference();
                        return;
                    }
                }

                CsCommandEventManager.Instance.SendPartyInvite(m_csFriend.Id);
            }
        }
    }

    bool m_bOpenPopupPresent = false;
    
    //---------------------------------------------------------------------------------------------------
    void OnClickPresent()
    {
        if (!m_bOpenPopupPresent)
        {
            m_bOpenPopupPresent = true;
            StartCoroutine(LoadPopupPresent());
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupPresent()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/PopupPresent");
        yield return resourceRequest;

        Transform trPopupPresent = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        trPopupPresent.name = "PopupPresent";

        if (m_csHeroBase != null)
        {
            CsGameEventUIToUI.Instance.OnEventOpenPopupPresent(m_csHeroBase.HeroId);
        }
        else if (m_csFriend != null)
        {
            CsGameEventUIToUI.Instance.OnEventOpenPopupPresent(m_csFriend.Id);
        }

        m_bOpenPopupPresent = false;

        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReference()
    {
        if (m_csHeroBase != null)
        {
            CsCommandEventManager.Instance.SendHeroInfo(m_csHeroBase.HeroId);
        }
        else if (m_csFriend != null)
        {
            CsCommandEventManager.Instance.SendHeroInfo(m_csFriend.Id);
        }
        else if (m_csGuildMember != null)
        {
            CsCommandEventManager.Instance.SendHeroInfo(m_csGuildMember.Id);
        }
        else if (m_csNationNoblesseInstance != null)
        {
            Debug.Log("m_csNationNoblesseInstance HeroId : " + m_csNationNoblesseInstance.HeroId);
            CsCommandEventManager.Instance.SendHeroInfo(m_csNationNoblesseInstance.HeroId);
        }

        OnEventCloseUserReference();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickFriendAdd()
    {
        if (CsFriendManager.Instance.FriendApplicationList.Find(a => a.TargetId == m_csHeroBase.HeroId) != null)
        {
            // 이미 신청
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02006"));
        }
        else if (CsGameConfig.Instance.FriendMaxCount <= CsFriendManager.Instance.FriendList.Count)
        {
            // 친구 Max
        }
        else
        {
            CsFriendManager.Instance.SendFriendApply(m_csHeroBase.HeroId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickFriendDelete()
    {
        List<System.Guid> listDeleteFriendId = new List<System.Guid>();

        if (m_csHeroBase != null)
        {
            listDeleteFriendId.Add(m_csHeroBase.HeroId);
        }
        else if (m_csFriend != null)
        {
            listDeleteFriendId.Add(m_csFriend.Id);
        }

        CsFriendManager.Instance.SendFriendDelete(listDeleteFriendId.ToArray());

        listDeleteFriendId.Clear();
        listDeleteFriendId = null;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildInvite()
    {
        if (m_csHeroBase != null)
        {
            if (m_csHeroBase.Level < CsGameConfig.Instance.GuildRequiredHeroLevel)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00305"));
            }
            else
            {
                CsGuildManager.Instance.SendGuildInvite(m_csHeroBase.HeroId);
            }
        }
        else if (m_csFriend != null)
        {
            if (m_csFriend.Level < CsGameConfig.Instance.GuildRequiredHeroLevel)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00305"));
            }
            else
            {
                CsGuildManager.Instance.SendGuildInvite(m_csFriend.Id);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildMasterTransfer()
    {
        CsGuildManager.Instance.SendGuildMasterTransfer(m_csGuildMember.Id);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildSubMasterAppoint()
    {
        CsGuildManager.Instance.SendGuildAppoint(m_csGuildMember.Id, 2);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildLoadAppoint()
    {
        CsGuildManager.Instance.SendGuildAppoint(m_csGuildMember.Id, 3);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildDismissAppointment()
    {
        CsGuildManager.Instance.SendGuildAppoint(m_csGuildMember.Id, 4);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildMemberBanish()
    {
        if (CsGuildManager.Instance.DailyBanishmentCount >= CsGameConfig.Instance.GuildDailyBanishmentMaxCount)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02011"));
            OnEventCloseUserReference();
        }
        else
        {
            CsGuildManager.Instance.SendGuildMemberBanish(m_csGuildMember.Id);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationNoblessDismiss()
    {
        CsCommandEventManager.Instance.SendNationNoblesseDismiss(m_csNationNoblesseInstance.NoblesseId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnclickBlackList()
    {
        CsFriendManager.Instance.SendBlacklistEntryAdd(m_csHeroBase.HeroId); 
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

        Transform trBack = transform.Find("ImageBackground");

        Text textPopupName = trBack.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A15_NAME_00001");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_imageEmblem = trBack.Find("ImageEmblem").GetComponent<Image>();

        m_textUserName = trBack.Find("TextUserName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textUserName);

        m_textUserNumber = trBack.Find("TextUserNumber").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textUserNumber);

        Transform trButtonList = trBack.Find("ButtonList");

        //정보조회
        m_buttonReference = trButtonList.Find("ButtonReference").GetComponent<Button>();
        m_buttonReference.onClick.RemoveAllListeners();
        m_buttonReference.onClick.AddListener(OnClickReference);
        m_buttonReference.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textReference = m_buttonReference.transform.Find("Text").GetComponent<Text>();
        textReference.text = CsConfiguration.Instance.GetString("A15_BTN_00008");
        CsUIData.Instance.SetFont(textReference);

        //친구추가
        m_buttonFriendAdd = trButtonList.Find("ButtonFriendAdd").GetComponent<Button>();
        m_buttonFriendAdd.onClick.RemoveAllListeners();
        m_buttonFriendAdd.onClick.AddListener(OnClickFriendAdd);
        m_buttonFriendAdd.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        

        Text textFriendAdd = m_buttonFriendAdd.transform.Find("Text").GetComponent<Text>();
        textFriendAdd.text = CsConfiguration.Instance.GetString("A15_BTN_00004");
        CsUIData.Instance.SetFont(textFriendAdd);

        //친구삭제
        m_buttonFriendDelete = trButtonList.Find("ButtonFriendDelete").GetComponent<Button>();
        m_buttonFriendDelete.onClick.RemoveAllListeners();
        m_buttonFriendDelete.onClick.AddListener(OnClickFriendDelete);
        m_buttonFriendDelete.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textFriendDelete = m_buttonFriendDelete.transform.Find("Text").GetComponent<Text>();
        textFriendDelete.text = CsConfiguration.Instance.GetString("A15_BTN_00011");
        CsUIData.Instance.SetFont(textFriendDelete);

        //파티초대
        m_buttonParty = trButtonList.Find("ButtonParty").GetComponent<Button>();
        m_buttonParty.onClick.RemoveAllListeners();
        m_buttonParty.onClick.AddListener(OnClickParty);
        m_buttonParty.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textParty = m_buttonParty.transform.Find("Text").GetComponent<Text>();
        textParty.text = CsConfiguration.Instance.GetString("A15_BTN_00005");
        CsUIData.Instance.SetFont(textParty);

        // 증정
        m_buttonPresent = trButtonList.Find("ButtonPresent").GetComponent<Button>();
        m_buttonPresent.onClick.RemoveAllListeners();
        m_buttonPresent.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonPresent.onClick.AddListener(OnClickPresent);

        Text textPresent = m_buttonPresent.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPresent);
        textPresent.text = CsConfiguration.Instance.GetString("A108_BTN_01001");

        //귓속말
        m_buttonOneToOne = trButtonList.Find("ButtonOneToOne").GetComponent<Button>();
        m_buttonOneToOne.onClick.RemoveAllListeners();
        m_buttonOneToOne.onClick.AddListener(OnClickOneToOneChat);
        m_buttonOneToOne.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textOneToOne = m_buttonOneToOne.transform.Find("Text").GetComponent<Text>();
        textOneToOne.text = CsConfiguration.Instance.GetString("A15_BTN_00003");
        CsUIData.Instance.SetFont(textOneToOne);

        // 블랙리스트
        m_buttonBlackList = trButtonList.Find("ButtonBlackList").GetComponent<Button>();
        m_buttonBlackList.onClick.RemoveAllListeners();
        m_buttonBlackList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonBlackList.onClick.AddListener(OnclickBlackList);

        Text textBlackList = m_buttonBlackList.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBlackList);
        textBlackList.text = CsConfiguration.Instance.GetString("A108_BTN_00041");

        //길드초대
        m_buttonGuildInvite = trButtonList.Find("ButtonGuildInvite").GetComponent<Button>();
        m_buttonGuildInvite.onClick.RemoveAllListeners();
        m_buttonGuildInvite.onClick.AddListener(OnClickGuildInvite);
        m_buttonGuildInvite.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildInvite = m_buttonGuildInvite.transform.Find("Text").GetComponent<Text>();
        textGuildInvite.text = CsConfiguration.Instance.GetString("A15_BTN_00006");
        CsUIData.Instance.SetFont(textGuildInvite);

        //길드장위임
        m_buttonGuildMasterTransfer = trButtonList.Find("ButtonGuildMasterTransfer").GetComponent<Button>();
        m_buttonGuildMasterTransfer.onClick.RemoveAllListeners();
        m_buttonGuildMasterTransfer.onClick.AddListener(OnClickGuildMasterTransfer);
        m_buttonGuildMasterTransfer.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildMasterTransfer = m_buttonGuildMasterTransfer.transform.Find("Text").GetComponent<Text>();
        textGuildMasterTransfer.text = CsConfiguration.Instance.GetString("A15_BTN_00012");
        CsUIData.Instance.SetFont(textGuildMasterTransfer);

        //부길드장임명
        m_buttonGuildSubMasterAppoint = trButtonList.Find("ButtonGuildSubMasterAppoint").GetComponent<Button>();
        m_buttonGuildSubMasterAppoint.onClick.RemoveAllListeners();
        m_buttonGuildSubMasterAppoint.onClick.AddListener(OnClickGuildSubMasterAppoint);
        m_buttonGuildSubMasterAppoint.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildSubMasterAppoint = m_buttonGuildSubMasterAppoint.transform.Find("Text").GetComponent<Text>();
        textGuildSubMasterAppoint.text = CsConfiguration.Instance.GetString("A15_BTN_00013");
        CsUIData.Instance.SetFont(textGuildSubMasterAppoint);

        //로드임명
        m_buttonGuildLoadAppoint = trButtonList.Find("ButtonGuildLoadAppoint").GetComponent<Button>();
        m_buttonGuildLoadAppoint.onClick.RemoveAllListeners();
        m_buttonGuildLoadAppoint.onClick.AddListener(OnClickGuildLoadAppoint);
        m_buttonGuildLoadAppoint.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildLoadAppoint = m_buttonGuildLoadAppoint.transform.Find("Text").GetComponent<Text>();
        textGuildLoadAppoint.text = CsConfiguration.Instance.GetString("A15_BTN_00014");
        CsUIData.Instance.SetFont(textGuildLoadAppoint);

        //임명해제
        m_buttonGuildDismissAppointment = trButtonList.Find("ButtonGuildDismissAppointment").GetComponent<Button>();
        m_buttonGuildDismissAppointment.onClick.RemoveAllListeners();
        m_buttonGuildDismissAppointment.onClick.AddListener(OnClickGuildDismissAppointment);
        m_buttonGuildDismissAppointment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildDismissAppointment = m_buttonGuildDismissAppointment.transform.Find("Text").GetComponent<Text>();
        textGuildDismissAppointment.text = CsConfiguration.Instance.GetString("A15_BTN_00015");
        CsUIData.Instance.SetFont(textGuildDismissAppointment);

        //길드추방
        m_buttonGuildMemberBanish = trButtonList.Find("ButtonGuildMemberBanish").GetComponent<Button>();
        m_buttonGuildMemberBanish.onClick.RemoveAllListeners();
        m_buttonGuildMemberBanish.onClick.AddListener(OnClickGuildMemberBanish);
        m_buttonGuildMemberBanish.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonNationNoblessDismiss = trButtonList.Find("ButtonNationNoblessDismiss").GetComponent<Button>();
        m_buttonNationNoblessDismiss.onClick.RemoveAllListeners();
        m_buttonNationNoblessDismiss.onClick.AddListener(OnClickNationNoblessDismiss);
        m_buttonNationNoblessDismiss.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNationNoblessDismiss = m_buttonNationNoblessDismiss.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationNoblessDismiss);
        textNationNoblessDismiss.text = CsConfiguration.Instance.GetString("A61_BTN_00003");

        Text textGuildMemberBanish = m_buttonGuildMemberBanish.transform.Find("Text").GetComponent<Text>();
        textGuildMemberBanish.text = CsConfiguration.Instance.GetString("A15_BTN_00016");
        CsUIData.Instance.SetFont(textGuildMemberBanish);
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateReference(CsFriend csFriend)
    {
        InitializeUI();

        m_csFriend = csFriend;

        m_imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csFriend.Job.JobId);
        m_textUserName.text = csFriend.Name;
        m_textUserNumber.text = csFriend.Id.ToString();

        m_buttonReference.gameObject.SetActive(true);
        m_buttonOneToOne.gameObject.SetActive(true);

        if (CsGuildManager.Instance.Guild != null)
        {
            if (csFriend.Level >= CsGameConfig.Instance.GuildRequiredHeroLevel && CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).InvitationEnabled)
            {
                m_buttonGuildInvite.gameObject.SetActive(true);
            }
        }

        // 파티버튼 조건.
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                bool bIsMember = CsGameData.Instance.MyHeroInfo.Nation.NationId == csFriend.Nation.NationId ? true : false;

                if (bIsMember)
                {
                    for (int i = 0; i < CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count; i++)
                    {
                        if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList[i].Id == csFriend.Id)
                        {
                            bIsMember = false;
                            break;
                        }
                    }
                }

                m_buttonParty.gameObject.SetActive(bIsMember);
            }
            else
            {
                m_buttonParty.gameObject.SetActive(false);
            }
        }
        else
        {
            if (csFriend.Nation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                m_buttonParty.interactable = true;
            }
            else
            {
                m_buttonParty.interactable = false;
            }

            m_buttonParty.gameObject.SetActive(true);
        }

        m_buttonFriendDelete.gameObject.SetActive(true);

        m_buttonPresent.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateReference(CsHeroBase csHeroBase)
    {
        InitializeUI();
        m_csHeroBase = csHeroBase;

        m_imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csHeroBase.Job.JobId);
        m_textUserName.text = csHeroBase.Name;
        m_textUserNumber.text = "";

        m_buttonReference.gameObject.SetActive(true);
        m_buttonOneToOne.gameObject.SetActive(true);

        if (CsGuildManager.Instance.Guild != null)
        {
            if (csHeroBase.Level >= CsGameConfig.Instance.GuildRequiredHeroLevel && CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).InvitationEnabled)
            {
                m_buttonGuildInvite.gameObject.SetActive(true);
            }
        }

        // 파티버튼 조건.
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                bool bIsMember = CsGameData.Instance.MyHeroInfo.Nation.NationId == csHeroBase.Nation.NationId ? true : false;

                if (bIsMember)
                {
                    for (int i = 0; i < CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count; i++)
                    {
                        if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList[i].Id == csHeroBase.HeroId)
                        {
                            bIsMember = false;
                            break;
                        }
                    }
                }

                m_buttonParty.gameObject.SetActive(bIsMember);
            }
            else
            {
                m_buttonParty.gameObject.SetActive(false);
            }
        }
        else
        {
            if (csHeroBase.Nation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                m_buttonParty.interactable = true;
            }
            else
            {
                m_buttonParty.interactable = false;
            }

            m_buttonParty.gameObject.SetActive(true);
        }

        if (CsFriendManager.Instance.BlacklistEntryList.Find(a => a.HeroId == m_csHeroBase.HeroId) == null)
        {
            if (CsFriendManager.Instance.FriendList.Find(a => a.Id == m_csHeroBase.HeroId) == null)
            {
                m_buttonFriendDelete.gameObject.SetActive(false);
                m_buttonFriendAdd.gameObject.SetActive(true);
            }
            else
            {
                m_buttonFriendAdd.gameObject.SetActive(false);
                m_buttonFriendDelete.gameObject.SetActive(true);
            }

            m_buttonBlackList.gameObject.SetActive(true);
        }
        else
        {
            m_buttonFriendAdd.gameObject.SetActive(false);
            m_buttonFriendDelete.gameObject.SetActive(false);

            m_buttonBlackList.gameObject.SetActive(false);
        }

        m_buttonPresent.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateReference(CsGuildMember csGuildMember)
    {
        InitializeUI();

        m_csGuildMember = csGuildMember;

        m_imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csGuildMember.Job.JobId);
        m_textUserName.text = csGuildMember.Name;
        m_textUserNumber.text = "";

        m_buttonReference.gameObject.SetActive(true);

        switch (CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade)
        {
            //길드장
            case 1:
                if (csGuildMember.GuildMemberGrade.MemberGrade == 2)
                {
                    //길드장위임,로드임명,임명해제,길드추방
                    m_buttonGuildMasterTransfer.gameObject.SetActive(true);
                    m_buttonGuildLoadAppoint.gameObject.SetActive(true);
                    m_buttonGuildDismissAppointment.gameObject.SetActive(true);
                    m_buttonGuildMemberBanish.gameObject.SetActive(true);
                }
                else if (csGuildMember.GuildMemberGrade.MemberGrade == 3)
                {
                    //부길드장위임,임명해제,길드추방
                    m_buttonGuildSubMasterAppoint.gameObject.SetActive(true);
                    m_buttonGuildDismissAppointment.gameObject.SetActive(true);
                    m_buttonGuildMemberBanish.gameObject.SetActive(true);
                }
                else
                {
                    //부길드장위임,로드임명,길드추방
                    m_buttonGuildSubMasterAppoint.gameObject.SetActive(true);
                    m_buttonGuildLoadAppoint.gameObject.SetActive(true);
                    m_buttonGuildMemberBanish.gameObject.SetActive(true);
                }
                break;
            case 2:
                if (csGuildMember.GuildMemberGrade.MemberGrade == 1)
                {

                }
                else if (csGuildMember.GuildMemberGrade.MemberGrade == 3)
                {
                    //임명해제,길드추방
                    m_buttonGuildDismissAppointment.gameObject.SetActive(true);
                    m_buttonGuildMemberBanish.gameObject.SetActive(true);
                }
                else
                {
                    //로드임명,길드추방
                    m_buttonGuildLoadAppoint.gameObject.SetActive(true);
                    m_buttonGuildMemberBanish.gameObject.SetActive(true);
                }
                break;
            case 3:
                if (csGuildMember.GuildMemberGrade.MemberGrade == 4)
                {
                    //길드추방
                    m_buttonGuildMemberBanish.gameObject.SetActive(true);
                }
                break;
            case 4:
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateReference(CsNationNoblesseInstance csTargetNationNoblesseInstance)
    {
        InitializeUI();

        m_csNationNoblesseInstance = csTargetNationNoblesseInstance;

        m_imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csTargetNationNoblesseInstance.JobId);
        m_textUserName.text = csTargetNationNoblesseInstance.HeroName;
        m_textUserNumber.text = "";

        m_buttonReference.gameObject.SetActive(true);
        m_buttonNationNoblessDismiss.gameObject.SetActive(true);

        CsNationNoblesseInstance csMyHeroNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.NationNoblesseInstanceList.Find(a => a.HeroId == CsGameData.Instance.MyHeroInfo.HeroId);

        if (csMyHeroNationNoblesseInstance == null)
        {
            m_buttonNationNoblessDismiss.gameObject.SetActive(false);
        }
        else
        {
            CsNationNoblesse csMyHeroNationNoblesse = CsGameData.Instance.GetNationNoblesse(csMyHeroNationNoblesseInstance.NoblesseId);
            CsNationNoblesseAppointmentAuthority csNationNoblesseAppointmentAuthority = csMyHeroNationNoblesse.NationNoblesseAppointmentAuthorityList.Find(a => a.TargetNoblesseId == csTargetNationNoblesseInstance.NoblesseId);

            if (csNationNoblesseAppointmentAuthority == null)
            {
                m_buttonNationNoblessDismiss.gameObject.SetActive(false);
            }
            else
            {
                m_buttonNationNoblessDismiss.gameObject.SetActive(true);
            }
        }
    }

    #region Present



    #endregion Present
}