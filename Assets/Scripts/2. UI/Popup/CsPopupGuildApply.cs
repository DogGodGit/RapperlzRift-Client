using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-03-12)
//---------------------------------------------------------------------------------------------------

public class CsPopupGuildApply : MonoBehaviour
{
    public event Delegate EventCloseGuildApply;

    [SerializeField] GameObject m_goGuildApplyItem;

    Transform m_trContent;
    Transform m_trCreateGuildPopup;

    InputField m_inputFieldSearch;

    List<CsSimpleGuild> m_listSimpleGuild;
    List<CsSimpleGuild> m_listTotalSimpleGuild;

    bool m_bApplyTotal = false;
    bool m_bSearch = false;
    string m_strSearchText = "";
    int m_nTotalCount;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGuildManager.Instance.EventGuildList += OnEventGuildList;
        CsGuildManager.Instance.EventGuildCreate += OnEventGuildCreate;
        CsGuildManager.Instance.EventGuildApply += OnEventGuildApply;
        CsGuildManager.Instance.EventGuildApplicationAccepted += OnEventGuildApplicationAccepted;
        CsGuildManager.Instance.EventGuildInvitationAccept += OnEventGuildInvitationAccept;
        InitializeUI();
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
        CsGuildManager.Instance.EventGuildList -= OnEventGuildList;
        CsGuildManager.Instance.EventGuildCreate -= OnEventGuildCreate;
        CsGuildManager.Instance.EventGuildApply -= OnEventGuildApply;
        CsGuildManager.Instance.EventGuildApplicationAccepted -= OnEventGuildApplicationAccepted;
        CsGuildManager.Instance.EventGuildInvitationAccept -= OnEventGuildInvitationAccept;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseGuildApply()
    {
        if (EventCloseGuildApply != null)
        {
            EventCloseGuildApply();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildList(List<CsSimpleGuild> listSimpleGuild)
    {
        m_listSimpleGuild = listSimpleGuild;
        DisplayGuild();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildCreate()
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
        OnEventCloseGuildApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApply()
    {
        if (m_bApplyTotal)
        {
            m_nTotalCount++;

            if (CsGuildManager.Instance.DailyGuildApplicationCount < CsGameConfig.Instance.GuildDailyApplicationMaxCount && m_listTotalSimpleGuild.Count > m_nTotalCount)
            {
                CsGuildManager.Instance.SendGuildApply(m_listTotalSimpleGuild[m_nTotalCount].Id);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02005"));
                m_bApplyTotal = false;
                m_nTotalCount = 0;
                UpdateButtonApply();
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02004"));
            UpdateButtonApply();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationAccepted()
    {
        OnEventCloseGuildApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationAccept()
    {
        OnEventCloseGuildApply();
    }

    #endregion Event

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        OnEventCloseGuildApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickApply(Guid guid)
    {
        if (CsGuildManager.Instance.Guild == null && CsGameData.Instance.MyHeroInfo.Level >= CsGameConfig.Instance.GuildRequiredHeroLevel)
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
    void OnClickApplyAll()
    {
        if (CsGuildManager.Instance.Guild == null && CsGameData.Instance.MyHeroInfo.Level >= CsGameConfig.Instance.GuildRequiredHeroLevel)
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
            else if (!m_bApplyTotal)
            {
                m_bApplyTotal = true;
                m_nTotalCount = 0;

                List<CsHeroGuildApplication> listApplication = CsGuildManager.Instance.HeroGuildApplicationList;

                if (m_bSearch)
                {
                    m_listTotalSimpleGuild = m_listSimpleGuild.FindAll(a => a.MemberCount < CsGameData.Instance.GetGuildLevel(a.Level).MaxMemberCount && a.Name.Contains(m_strSearchText) && listApplication.Find(b => a.Id == b.GuildId) == null);
                }
                else
                {
                    m_listTotalSimpleGuild = m_listSimpleGuild.FindAll(a => a.MemberCount < CsGameData.Instance.GetGuildLevel(a.Level).MaxMemberCount && listApplication.Find(b => a.Id == b.GuildId) == null);
                }

                if (m_listTotalSimpleGuild.Count != 0)
                {
                    CsGuildManager.Instance.SendGuildApply(m_listTotalSimpleGuild[m_nTotalCount].Id);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02006"));
                    m_bApplyTotal = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCreateGuildPopupOpen()
    {
        if (!m_trCreateGuildPopup.gameObject.activeSelf)
            m_trCreateGuildPopup.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCreateGuildPopupClose()
    {
        if (m_trCreateGuildPopup.gameObject.activeSelf)
            m_trCreateGuildPopup.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCreateGuild()
    {
        Text textInput = m_trCreateGuildPopup.Find("ImageBackground/InputField/Text").GetComponent<Text>();

        string strGuildName = textInput.text;
        String str = Regex.Replace(strGuildName, @"[^0-9a-zA-Z가-힣]", "");

        if (string.IsNullOrEmpty(strGuildName))
        {
            //길이 에러
            return;
        }
        else if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsGameConfig.Instance.GuildCreationRequiredVipLevel)
        {
            //Vip 제한
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02013"));
            return;
        }
        else if (CsGameData.Instance.MyHeroInfo.Dia < CsGameConfig.Instance.GuildCreationRequiredDia)
        {
            //Dia제한
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02014"));
            return;
        }
        else if (strGuildName.Length > 8)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02015"), 8));
            return;
        }
        else if (strGuildName.IndexOf("　") > -1 || strGuildName.IndexOf(" ") > -1)  // ㄱ + 한자 중 첫번째 글자 또는 공백.
        {
            // 길드 이름에 공백을 넣을 수 없습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02024"));
            return;
        }
        else if (str != strGuildName)
        {
            //특수문자 찾기
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02023"));
            return;
        }
        //금지문자 찾기
        //else if (ContainsBanWord(CsConfiguration.Instance.NameBanWordList, sNickname))
        //{
        //    OpenPopupCreateNameError(0);
        //    return;
        //}
        else if (CsGuildManager.Instance.GuildRejoinRemainingTime >= Time.realtimeSinceStartup)
        {
            int nRejoinTime = (int)((CsGuildManager.Instance.GuildRejoinRemainingTime - Time.realtimeSinceStartup) / 60);
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02002"), nRejoinTime.ToString()));
            return;
        }
        else
        {
            CsGuildManager.Instance.SendGuildCreate(strGuildName);
        }
    }

    void OnClickGuildSearch()
    {
        DisplayGuild();
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");

        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        textPopupName.text = CsConfiguration.Instance.GetString("MMENU_NAME_19");
        CsUIData.Instance.SetFont(textPopupName);

        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickPopupClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonApplyAll = transform.Find("ButtonApplyAll").GetComponent<Button>();
        buttonApplyAll.onClick.RemoveAllListeners();
        buttonApplyAll.onClick.AddListener(OnClickApplyAll);
        buttonApplyAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textApplyAll = buttonApplyAll.transform.Find("Text").GetComponent<Text>();
        textApplyAll.text = CsConfiguration.Instance.GetString("A58_BTN_00002");
        CsUIData.Instance.SetFont(textApplyAll);

        Button buttonCreateGuildPopupOpen = transform.Find("ButtonCreateGuildPopup").GetComponent<Button>();
        buttonCreateGuildPopupOpen.onClick.RemoveAllListeners();
        buttonCreateGuildPopupOpen.onClick.AddListener(OnClickCreateGuildPopupOpen);
        buttonCreateGuildPopupOpen.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCreateGuild = buttonCreateGuildPopupOpen.transform.Find("Text").GetComponent<Text>();
        textCreateGuild.text = CsConfiguration.Instance.GetString("A58_BTN_00003");
        CsUIData.Instance.SetFont(textCreateGuild);

        m_trCreateGuildPopup = transform.Find("ImageCreateGuild");
        Button buttonCreateClose = m_trCreateGuildPopup.Find("ImageBackground/ButtonCreateClose").GetComponent<Button>();
        buttonCreateClose.onClick.RemoveAllListeners();
        buttonCreateClose.onClick.AddListener(OnClickCreateGuildPopupClose);
        buttonCreateClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textName = m_trCreateGuildPopup.Find("ImageBackground/TextName").GetComponent<Text>();
        textName.text = CsConfiguration.Instance.GetString("A58_BTN_00003");
        CsUIData.Instance.SetFont(textName);

        Text textInput = m_trCreateGuildPopup.Find("ImageBackground/TextInput").GetComponent<Text>();
        textInput.text = CsConfiguration.Instance.GetString("A58_TXT_00031");
        CsUIData.Instance.SetFont(textInput);

        Text textCreatePlaceholder = m_trCreateGuildPopup.Find("ImageBackground/InputField/Placeholder").GetComponent<Text>();
        textCreatePlaceholder.text = CsConfiguration.Instance.GetString("A58_TXT_00003");
        CsUIData.Instance.SetFont(textCreatePlaceholder);

        Text textVip = m_trCreateGuildPopup.Find("ImageBackground/ImageVip/Text").GetComponent<Text>();
        textVip.text = string.Format(CsConfiguration.Instance.GetString("A58_TXT_01011"), CsGameConfig.Instance.GuildCreationRequiredVipLevel);
        CsUIData.Instance.SetFont(textVip);

        Button buttonCreateGuild = m_trCreateGuildPopup.Find("ImageBackground/ButtonCreateGuild").GetComponent<Button>();
        buttonCreateGuild.onClick.RemoveAllListeners();
        buttonCreateGuild.onClick.AddListener(OnClickCreateGuild);
        buttonCreateGuild.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonCreateGuild = buttonCreateGuild.transform.Find("Text").GetComponent<Text>();
        textButtonCreateGuild.text = CsConfiguration.Instance.GetString("A58_BTN_00003");
        CsUIData.Instance.SetFont(textButtonCreateGuild);

        Text textDiaCount = buttonCreateGuild.transform.Find("ImageDia/Text").GetComponent<Text>();
        textDiaCount.text = CsGameConfig.Instance.GuildCreationRequiredDia.ToString();
        CsUIData.Instance.SetFont(textDiaCount);

        m_inputFieldSearch = transform.Find("InputFieldSearch").GetComponent<InputField>();
        CsUIData.Instance.SetFont(m_inputFieldSearch.textComponent);

        Text textSearchPlaceholder = m_inputFieldSearch.transform.Find("Placeholder").GetComponent<Text>();
        textSearchPlaceholder.text = CsConfiguration.Instance.GetString("A58_TXT_00003");
        CsUIData.Instance.SetFont(textSearchPlaceholder);

        Button buttonSearch = m_inputFieldSearch.transform.Find("ButtonSearch").GetComponent<Button>();
        buttonSearch.onClick.RemoveAllListeners();
        buttonSearch.onClick.AddListener(OnClickGuildSearch);
        buttonSearch.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        textVip.color = CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsGameConfig.Instance.GuildCreationRequiredVipLevel ? CsUIData.Instance.ColorRed : CsUIData.Instance.ColorWhite;
        textDiaCount.color = CsGameData.Instance.MyHeroInfo.Dia < CsGameConfig.Instance.GuildCreationRequiredDia ? CsUIData.Instance.ColorGray : CsUIData.Instance.ColorWhite;

        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsGameConfig.Instance.GuildCreationRequiredVipLevel || CsGameData.Instance.MyHeroInfo.Dia < CsGameConfig.Instance.GuildCreationRequiredDia)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonCreateGuild, false);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonCreateGuild, true);
        }


        CsGuildManager.Instance.SendGuildList();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateGuild(CsSimpleGuild csSimpleGuild)
    {
        Transform trGuild = m_trContent.Find(csSimpleGuild.Id.ToString());

        if (trGuild == null)
        {
            trGuild = Instantiate(m_goGuildApplyItem, m_trContent).transform;
            trGuild.name = csSimpleGuild.Id.ToString();
        }

        Image imageGuild = trGuild.Find("Image").GetComponent<Image>();
        imageGuild.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/ico_guild_lv_" + csSimpleGuild.Level);

        Text textGuildName = trGuild.Find("TextGuildName").GetComponent<Text>();
        textGuildName.text = csSimpleGuild.Name;
        CsUIData.Instance.SetFont(textGuildName);

        Text textGuildNotice = trGuild.Find("TextGuildNotice").GetComponent<Text>();
        if (!string.IsNullOrEmpty(csSimpleGuild.Notice) && csSimpleGuild.Notice.Length > 20)
        {
            textGuildNotice.text = csSimpleGuild.Notice.Substring(0, 20) + "....";
        }
        else
        {
            textGuildNotice.text = csSimpleGuild.Notice;
        }

        CsUIData.Instance.SetFont(textGuildNotice);

        Text textGuildMaster = trGuild.Find("TextGuildMaster").GetComponent<Text>();
        textGuildMaster.text = CsConfiguration.Instance.GetString("A58_TXT_00001");
        CsUIData.Instance.SetFont(textGuildMaster);

        Text textGuildMasterName = trGuild.Find("TextMasterName").GetComponent<Text>();
        textGuildMasterName.text = csSimpleGuild.MasterName;
        CsUIData.Instance.SetFont(textGuildMasterName);

        Text textGuildMember = trGuild.Find("TextGuildMember").GetComponent<Text>();
        textGuildMember.text = CsConfiguration.Instance.GetString("A58_TXT_00002");
        CsUIData.Instance.SetFont(textGuildMember);

        Text textGuildMemberCount = trGuild.Find("TextMemberCount").GetComponent<Text>();
        textGuildMemberCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), csSimpleGuild.MemberCount, CsGameData.Instance.GetGuildLevel(csSimpleGuild.Level).MaxMemberCount);
        CsUIData.Instance.SetFont(textGuildMemberCount);

        Button buttonGuildApply = trGuild.Find("ButtonApply").GetComponent<Button>();
        buttonGuildApply.onClick.RemoveAllListeners();
        buttonGuildApply.onClick.AddListener(() => OnClickApply(csSimpleGuild.Id));
        buttonGuildApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonGuildApply = buttonGuildApply.transform.Find("Text").GetComponent<Text>();
        textButtonGuildApply.text = CsConfiguration.Instance.GetString("A58_BTN_00001");
        CsUIData.Instance.SetFont(textButtonGuildApply);

        Text textGuildApply = trGuild.Find("TextApply").GetComponent<Text>();
        textGuildApply.text = CsConfiguration.Instance.GetString("A58_BTN_00001");
        CsUIData.Instance.SetFont(textGuildApply);

        trGuild.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonApply()
    {
        List<CsHeroGuildApplication> listHeroApply = CsGuildManager.Instance.HeroGuildApplicationList;

        for (int i = 0; i < listHeroApply.Count; ++i)
        {
            Transform trGuild = m_trContent.Find(listHeroApply[i].GuildId.ToString());
            if (trGuild != null)
            {
                Transform trGuildApply = trGuild.Find("ButtonApply");
                trGuildApply.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    int SortToGuild(CsSimpleGuild A, CsSimpleGuild B)
    {
        if (A.MemberCount == CsGameData.Instance.GetGuildLevel(A.Level).MaxMemberCount) return 1;
        else if (B.MemberCount == CsGameData.Instance.GetGuildLevel(B.Level).MaxMemberCount) return -1;
        else
        {
            if (A.Level < B.Level) return 1;
            else if (A.Level > B.Level) return -1;
            else
            {
                if (A.MemberCount < B.MemberCount) return 1;
                else if (A.MemberCount > B.MemberCount) return -1;
                else
                {
                    if (A.Name.CompareTo(B.Name) > 0) return 1;
                    else if (A.Name.CompareTo(B.Name) < 0) return -1;
                    else
                        return 0;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayGuild()
    {
        if (m_listSimpleGuild.Count == 0)
        {
            Text textNoGuild = transform.Find("TextNoGuild").GetComponent<Text>();
            textNoGuild.text = CsConfiguration.Instance.GetString("A58_TXT_00019");
            CsUIData.Instance.SetFont(textNoGuild);
            textNoGuild.gameObject.SetActive(true);
        }
        else
        {
            m_listSimpleGuild.Sort(SortToGuild);

            if (string.IsNullOrEmpty(m_inputFieldSearch.text))
            {
                m_bSearch = false;
                m_strSearchText = "";

                for (int i = 0; i < m_listSimpleGuild.Count; ++i)
                {
                    if (i == 30) break;
                    CreateGuild(m_listSimpleGuild[i]);
                }
            }
            else
            {
                List<CsSimpleGuild> listSearchGuild = m_listSimpleGuild.FindAll(a => a.Name.Contains(m_inputFieldSearch.text));

                if (listSearchGuild.Count == 0)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02007"));
                    return;
                }
                else
                {
                    m_bSearch = true;
                    m_strSearchText = m_inputFieldSearch.text;
                    for (int i = 0; i < m_trContent.childCount; ++i)
                    {
                        m_trContent.GetChild(i).gameObject.SetActive(false);
                    }

                    for (int i = 0; i < listSearchGuild.Count; ++i)
                    {
                        CreateGuild(listSearchGuild[i]);
                    }
                }
            }

            UpdateButtonApply();
        }
    }
}
