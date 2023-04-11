using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-03-12)
//---------------------------------------------------------------------------------------------------

public enum EnGuildReward
{
    GuildToday = 0,
    FoodWarehouse = 1,
    GuildAltar = 2,
    GuildHunter = 3,
}

public class CsPopupGuildMember : CsPopupSub
{
    const int c_nGuildRewardCount = 4;

    [SerializeField] GameObject m_goGuildMemberItem;
    [SerializeField] GameObject m_goGuildApplyItem;
    [SerializeField] GameObject m_goGuildRewardItem;

    Transform m_trPopupGuildNotice;
    Transform m_trPopupGuildApply;
    Transform m_trPopupGuildDonate;
    Transform m_trContent;
    Transform m_trApplyContent;
    Transform m_trPopupGuildReward;

    Transform m_trImageRewardNotice;
    Transform m_trImageDonateNotice;

    Text m_textGuildNoticeScript;
    InputField m_inputFieldNotice;

    Button m_buttonApply;
    Button m_buttonNotice;

    bool m_bIsLoad = false;
    bool m_bFirst = true;

    int m_nLoadMemberItemCount = 0;
    int m_nStandardPosition = 0;
    float m_flTime = 0f;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGuildManager.Instance.EventGuildApplicationAccept += OnEventGuildApplicationAccept;
        CsGuildManager.Instance.EventGuildApplicationRefuse += OnEventGuildApplicationRefuse;
        CsGuildManager.Instance.EventGuildApplicationList += OnEventGuildApplicationList;
        CsGuildManager.Instance.EventGuildExit += OnEventGuildExit;
        CsGuildManager.Instance.EventGuildMemberBanish += OnEventGuildMemberBanish;
        CsGuildManager.Instance.EventGuildBanished += OnEventGuildBanished;
        CsGuildManager.Instance.EventGuildNoticeSet += OnEventGuildNoticeSet;
        CsGuildManager.Instance.EventGuildAppoint += OnEventGuildAppoint;
        CsGuildManager.Instance.EventGuildAppointed += OnEventGuildAppointed;
        CsGuildManager.Instance.EventGuildMasterTransfer += OnEventGuildMasterTransfer;
        CsGuildManager.Instance.EventGuildMasterTransferred += OnEventGuildMasterTransferred;
        CsGuildManager.Instance.EventGuildMemberEnter += OnEventGuildMemberEnter;
        CsGuildManager.Instance.EventGuildMemberExit += OnEventGuildMemberExit;
        CsGuildManager.Instance.EventGuildMemberTabInfo += OnEventGuildMemberTabInfo;
        CsGuildManager.Instance.EventGuildFundChanged += OnEventGuildFundChanged;
        CsGuildManager.Instance.EventGuildBuildingLevelUp += OnEventGuildBuildingLevelUp;
        CsGuildManager.Instance.EventGuildBuildingLevelUpEvent += OnEventGuildBuildingLevelUpEvent;
        CsGuildManager.Instance.EventGuildNoticeChanged += OnEventGuildNoticeChanged;
        CsGuildManager.Instance.EventGuildSkillLevelUp += OnEventGuildSkillLevelUp;
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter += OnEventContinentExitForGuildTerritoryEnter;

        CsGuildManager.Instance.EventGuildDonate += OnEventGuildDonate;

        CsDungeonManager.Instance.EventGoldDungeonStepCompleted += OnEventGoldDungeonStepCompleted;
        CsMainQuestManager.Instance.EventCompleted += OnEventMainQuestCompleted;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted += OnEventMainQuestDungeonStepCompleted;

        //보상
        CsGuildManager.Instance.EventGuildDailyRewardReceive += OnEventGuildDailyRewardReceive;
        CsGuildManager.Instance.EventGuildAltarRewardReceive += OnEventGuildAltarRewardReceive;
        CsGuildManager.Instance.EventGuildFoodWarehouseRewardReceive += OnEventGuildFoodWarehouseRewardReceive;
		CsGuildManager.Instance.EventGuildHuntingDonationRewardReceive += OnEventGuildHuntingDonationRewardReceive;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGuildManager.Instance.EventGuildApplicationAccept -= OnEventGuildApplicationAccept;
        CsGuildManager.Instance.EventGuildApplicationRefuse -= OnEventGuildApplicationRefuse;
        CsGuildManager.Instance.EventGuildApplicationList -= OnEventGuildApplicationList;
        CsGuildManager.Instance.EventGuildExit -= OnEventGuildExit;
        CsGuildManager.Instance.EventGuildMemberBanish -= OnEventGuildMemberBanish;
        CsGuildManager.Instance.EventGuildBanished -= OnEventGuildBanished;
        CsGuildManager.Instance.EventGuildNoticeSet -= OnEventGuildNoticeSet;
        CsGuildManager.Instance.EventGuildAppoint -= OnEventGuildAppoint;
        CsGuildManager.Instance.EventGuildAppointed -= OnEventGuildAppointed;
        CsGuildManager.Instance.EventGuildMasterTransfer -= OnEventGuildMasterTransfer;
        CsGuildManager.Instance.EventGuildMasterTransferred -= OnEventGuildMasterTransferred;
        CsGuildManager.Instance.EventGuildMemberEnter -= OnEventGuildMemberEnter;
        CsGuildManager.Instance.EventGuildMemberExit -= OnEventGuildMemberExit;
        CsGuildManager.Instance.EventGuildMemberTabInfo -= OnEventGuildMemberTabInfo;
        CsGuildManager.Instance.EventGuildFundChanged -= OnEventGuildFundChanged;
        CsGuildManager.Instance.EventGuildBuildingLevelUp -= OnEventGuildBuildingLevelUp;
        CsGuildManager.Instance.EventGuildBuildingLevelUpEvent -= OnEventGuildBuildingLevelUpEvent;
        CsGuildManager.Instance.EventGuildNoticeChanged -= OnEventGuildNoticeChanged;
        CsGuildManager.Instance.EventGuildSkillLevelUp -= OnEventGuildSkillLevelUp;
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter -= OnEventContinentExitForGuildTerritoryEnter;

        CsGuildManager.Instance.EventGuildDonate -= OnEventGuildDonate;

        CsDungeonManager.Instance.EventGoldDungeonStepCompleted -= OnEventGoldDungeonStepCompleted;
        CsMainQuestManager.Instance.EventCompleted -= OnEventMainQuestCompleted;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted -= OnEventMainQuestDungeonStepCompleted;

        //보상
        CsGuildManager.Instance.EventGuildDailyRewardReceive -= OnEventGuildDailyRewardReceive;
        CsGuildManager.Instance.EventGuildAltarRewardReceive -= OnEventGuildAltarRewardReceive;
        CsGuildManager.Instance.EventGuildFoodWarehouseRewardReceive -= OnEventGuildFoodWarehouseRewardReceive;
		CsGuildManager.Instance.EventGuildHuntingDonationRewardReceive -= OnEventGuildHuntingDonationRewardReceive;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        // 1초마다 실행.
        if (m_flTime + 1f < Time.time)
        {
            UpdateGuildApply();

            m_flTime = Time.time;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationList()
    {
        m_trPopupGuildApply.gameObject.SetActive(true);
        DisplayApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationAccept()
    {
        DisplayApply();
        CsGuildManager.Instance.SendGuildMemberTabInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationRefuse()
    {
        DisplayApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMemberBanish()
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBanished(int nContinentId)
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildExit(int nContinentId)
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildNoticeSet()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02012"));
        m_textGuildNoticeScript.text = CsGuildManager.Instance.Notice;
        m_inputFieldNotice.text = "";
        OnClickPopupNotice(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildNoticeChanged()
    {
        m_textGuildNoticeScript.text = CsGuildManager.Instance.Notice;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSkillLevelUp()
    {
        DisplayGuildInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMasterTransfer()
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
        UpdateGuildApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMasterTransferred(Guid guidTransfererId, string strTransFererName, Guid guidTransfereeId, string strTransfereeName)
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId != guidTransfererId)
        {
            CsGuildManager.Instance.SendGuildMemberTabInfo();
            UpdateGuildApply();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAppoint()
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
        UpdateGuildApply();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAppointed(Guid guidAppointerId, string strAppointerName, int nAppointerGrade, Guid guidAppointeeId, string strAppointeeName, int nApoointeeGrade)
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId != guidAppointerId)
        {
            CsGuildManager.Instance.SendGuildMemberTabInfo();
            UpdateGuildApply();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMemberEnter(Guid guidHeroid, string strName)
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMemberExit(Guid guidHeroId, string strHeroName, bool bBanished)
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDonate()
    {
        DisplayGuildInfo();
        UpdateDonate();
        UpdateDonateNotcie();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDailyRewardReceive()
    {
        UpdateGuildRewardButtonReceive((int)EnGuildReward.GuildToday);
        UpdateGuildRewardNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarRewardReceive()
    {
        UpdateGuildRewardButtonReceive((int)EnGuildReward.GuildAltar);
        UpdateGuildRewardNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFundChanged()
    {
        DisplayGuildInfo();
        UpdateDonate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBuildingLevelUp()
    {
        DisplayGuildInfo();
        UpdateDonate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBuildingLevelUpEvent()
    {
        DisplayGuildInfo();
        UpdateDonate();
    }
    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonStepCompleted(long lGold)
    {
        UpdateDonate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestCompleted(CsMainQuest cs, bool b, long lAcquiredExp)
    {
        UpdateDonate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonStepCompleted(bool bLevelUp, long lRewardGold, long lExp)
    {
        UpdateDonate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMemberTabInfo()
    {
        DisplayGuildInfo();
        InitializeMember();
    }

    // 길드 군량 보상
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFoodWarehouseRewardReceive()
    {
        UpdateGuildRewardButtonReceive((int)EnGuildReward.FoodWarehouse);
        UpdateGuildRewardNotice();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildHuntingDonationRewardReceive()
	{
		UpdateGuildRewardButtonReceive((int)EnGuildReward.GuildHunter);
		UpdateGuildRewardNotice();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForGuildTerritoryEnter(string sSceneName)
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsGuildManager.Instance.SendGuildMemberTabInfo();
    }

    #endregion Event

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupGuildReward()
    {
        m_trPopupGuildReward.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupGuildReward()
    {
        m_trPopupGuildReward.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildRewardReceive(int nIndex)
    {
        switch ((EnGuildReward)nIndex)
        {
            case EnGuildReward.GuildToday:
                CsGuildManager.Instance.SendGuildDailyRewardReceive();
                break;

            case EnGuildReward.GuildAltar:
                CsGuildManager.Instance.SendGuildAltarRewardReceive();
                break;

            case EnGuildReward.FoodWarehouse:
                CsGuildManager.Instance.SendGuildFoodWarehouseRewardReceive();
                break;

            case EnGuildReward.GuildHunter:
                CsGuildManager.Instance.SendGuildHuntingDonationRewardReceive();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupExit()
    {
        //길드장일 경우 탈퇴 못함
        if (CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade == 1)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02010"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A58_TXT_00018"),
                   CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickExit,
                   CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExit()
    {
        CsGuildManager.Instance.SendGuildExit();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupDonate(bool bActive)
    {
        m_trPopupGuildDonate.gameObject.SetActive(bActive);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDonate(int nEntryId)
    {
        if (CsGuildManager.Instance.DailyGuildDonationCount < CsGameData.Instance.MyHeroInfo.VipLevel.GuildDonationMaxCount)
        {
            CsGuildManager.Instance.SendGuildDonate(nEntryId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupApply(bool bActive)
    {
        if (bActive && CsGameData.Instance.GetGuildMemberGrade( CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).ApplicationAcceptanceEnabled)
        {
            CsGuildManager.Instance.SendGuildApplicationList();
        }
        else
        {
            m_trPopupGuildApply.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAccept(Guid guid)
    {
        CsGuildManager.Instance.SendGuildApplicationAccept(guid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRefuse(Guid guid)
    {
        CsGuildManager.Instance.SendGuildApplicationRefuse(guid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupNotice(bool bActive)
    {
        if (bActive)
        {
            if (CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade != 1)
                return;
        }

        m_trPopupGuildNotice.gameObject.SetActive(bActive);
        m_inputFieldNotice.text = "";
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNoticeSet()
    {
        CsGuildManager.Instance.SendGuildNoticeSet(m_inputFieldNotice.text);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildManorEnter()
    {
        if (CsUIData.Instance.DungeonInNow != EnDungeon.None)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02017"));
        }
        else if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02018"));
        }
        else
        {
            CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReference(CsGuildMember csGuildMember)
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId != csGuildMember.Id)
            CsGameEventUIToUI.Instance.OnEventOpenGuildMemberReference(csGuildMember);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildMemberScrollbar(Scrollbar scrollbar)
    {
        if (!m_bIsLoad)
        {
            m_bIsLoad = true;

            List<CsGuildMember> listMember = CsGuildManager.Instance.GuildMemberList;

            if (m_nLoadMemberItemCount < listMember.Count)
            {
                int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, listMember.Count, (1 - scrollbar.value)));

                if (nUpdateLine >= m_nStandardPosition)
                {
                    int nStartCount = m_nLoadMemberItemCount;
                    int nEndCount = nUpdateLine + 10;

                    if (nEndCount >= listMember.Count)
                    {
                        nEndCount = listMember.Count;
                    }

                    for (int i = nStartCount; i < nEndCount; i++)
                    {
                        CreateMember(listMember[i], i);
                    }

                    m_nStandardPosition = nUpdateLine;
                }
            }

            m_bIsLoad = false;
        }
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");

        Image imageGuildLevel = transform.Find("ImageGuildLevel").GetComponent<Image>();
        imageGuildLevel.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/ico_guild_lv_" + CsGuildManager.Instance.Level);

        Transform trGuildInfoList = transform.Find("GuildInfoList");

        Text textGuildName = trGuildInfoList.Find("TextGuildName").GetComponent<Text>();
        textGuildName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), CsGuildManager.Instance.Level, CsGuildManager.Instance.GuildName);
        CsUIData.Instance.SetFont(textGuildName);

        Text textMemberCount = trGuildInfoList.Find("GuildMemberCount/Text").GetComponent<Text>();
        textMemberCount.text = CsConfiguration.Instance.GetString("A58_TXT_00004");
        CsUIData.Instance.SetFont(textMemberCount);

        Text textMemberCountValue = trGuildInfoList.Find("GuildMemberCount/TextValue").GetComponent<Text>();
        textMemberCountValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGuildManager.Instance.GuildMemberList.Count,
            CsGameData.Instance.GetGuildLevel(CsGuildManager.Instance.Level).MaxMemberCount);
        CsUIData.Instance.SetFont(textMemberCountValue);

        Text textGuildConstructionPoint = trGuildInfoList.Find("GuildConstructionPoint/Text").GetComponent<Text>();
        textGuildConstructionPoint.text = CsConfiguration.Instance.GetString("A58_TXT_00005");
        CsUIData.Instance.SetFont(textGuildConstructionPoint);

        Text textGuildConstructionPointValue = trGuildInfoList.Find("GuildConstructionPoint/TextValue").GetComponent<Text>();
        textGuildConstructionPointValue.text = CsGuildManager.Instance.BuildingPoint.ToString("#,##0");
        CsUIData.Instance.SetFont(textGuildConstructionPointValue);

        Text textGuildFunds = trGuildInfoList.Find("GuildFunds/Text").GetComponent<Text>();
        textGuildFunds.text = CsConfiguration.Instance.GetString("A58_TXT_00006");
        CsUIData.Instance.SetFont(textGuildFunds);

        Text textGuildFundsValue = trGuildInfoList.Find("GuildFunds/TextValue").GetComponent<Text>();
        textGuildFundsValue.text = CsGuildManager.Instance.Fund.ToString("#,##0");
        CsUIData.Instance.SetFont(textGuildFundsValue);

        Text textGuildContributionPoint = trGuildInfoList.Find("GuildContributionPoint/Text").GetComponent<Text>();
        textGuildContributionPoint.text = CsConfiguration.Instance.GetString("A58_TXT_00007");
        CsUIData.Instance.SetFont(textGuildContributionPoint);

        Text textGuildContributionPointValue = trGuildInfoList.Find("GuildContributionPoint/TextValue").GetComponent<Text>();
        textGuildContributionPointValue.text = CsGuildManager.Instance.GuildContributionPoint.ToString("#,##0");
        CsUIData.Instance.SetFont(textGuildContributionPointValue);

        Button buttonExit = transform.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(OnClickPopupExit);
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textExit = buttonExit.transform.Find("Text").GetComponent<Text>();
        textExit.text = CsConfiguration.Instance.GetString("A58_BTN_00004");
        CsUIData.Instance.SetFont(textExit);

        Button buttonDedicate = transform.Find("ButtonDedicate").GetComponent<Button>();
        buttonDedicate.onClick.RemoveAllListeners();
        buttonDedicate.onClick.AddListener(() => OnClickPopupDonate(true));
        buttonDedicate.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textDedicate = buttonDedicate.transform.Find("Text").GetComponent<Text>();
        textDedicate.text = CsConfiguration.Instance.GetString("A58_BTN_00005");
        CsUIData.Instance.SetFont(textDedicate);

        m_buttonApply = transform.Find("ButtonApply").GetComponent<Button>();
        m_buttonApply.onClick.RemoveAllListeners();
        m_buttonApply.onClick.AddListener(() => OnClickPopupApply(true));
        m_buttonApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //길드신청
        m_trPopupGuildApply = transform.Find("PopupGuildApply");
        Button buttonApplyClose = m_trPopupGuildApply.Find("ImageBackground/ButtonClose").GetComponent<Button>();
        buttonApplyClose.onClick.RemoveAllListeners();
        buttonApplyClose.onClick.AddListener(() => OnClickPopupApply(false));
        buttonApplyClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildApplyName = m_trPopupGuildApply.Find("ImageBackground/Text").GetComponent<Text>();
        textGuildApplyName.text = CsConfiguration.Instance.GetString("A58_TXT_00017");
        CsUIData.Instance.SetFont(textGuildApplyName);
        m_trApplyContent = m_trPopupGuildApply.Find("ImageBackground/Scroll View/Viewport/Content");

        //길드공지
        m_trPopupGuildNotice = transform.Find("PopupGuildNotice");
        Button buttonNoticeClose = m_trPopupGuildNotice.Find("ImageBackground/ButtonClose").GetComponent<Button>();
        buttonNoticeClose.onClick.RemoveAllListeners();
        buttonNoticeClose.onClick.AddListener(() => OnClickPopupNotice(false));
        buttonNoticeClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNoticeName = m_trPopupGuildNotice.Find("ImageBackground/Text").GetComponent<Text>();
        textNoticeName.text = CsConfiguration.Instance.GetString("A58_TXT_00019");
        CsUIData.Instance.SetFont(textNoticeName);

        Text textNoticePlaceholder = m_trPopupGuildNotice.Find("ImageBackground/GuildNoticeItem/InputField/Placeholder").GetComponent<Text>();
        textNoticePlaceholder.text = CsConfiguration.Instance.GetString("A58_TXT_00020");
        CsUIData.Instance.SetFont(textNoticePlaceholder);

        m_inputFieldNotice = m_trPopupGuildNotice.Find("ImageBackground/GuildNoticeItem/InputField").GetComponent<InputField>();
        m_inputFieldNotice.text = "";
        m_inputFieldNotice.characterLimit = CsGameConfig.Instance.GuildNoticeMaxLength;
        CsUIData.Instance.SetFont(m_inputFieldNotice.textComponent);

        Text textInputCount = m_trPopupGuildNotice.Find("ImageBackground/GuildNoticeItem/TextInputCount").GetComponent<Text>();
        textInputCount.text = string.Format(CsConfiguration.Instance.GetString("A58_TXT_01005"), CsGameConfig.Instance.GuildNoticeMaxLength);
        CsUIData.Instance.SetFont(textInputCount);

        Button buttonNoticeSet = m_trPopupGuildNotice.Find("ImageBackground/GuildNoticeItem/ButtonNoticeSet").GetComponent<Button>();
        buttonNoticeSet.onClick.RemoveAllListeners();
        buttonNoticeSet.onClick.AddListener(OnClickNoticeSet);
        buttonNoticeSet.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNoticeSet = buttonNoticeSet.transform.Find("Text").GetComponent<Text>();
        textNoticeSet.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
        CsUIData.Instance.SetFont(textNoticeSet);

        Transform trNotice = transform.Find("GuildNotice");
        Text textGuildNotice = trNotice.Find("TextNotice").GetComponent<Text>();
        textGuildNotice.text = CsConfiguration.Instance.GetString("A58_TXT_00011");
        CsUIData.Instance.SetFont(textGuildNotice);

        m_textGuildNoticeScript = trNotice.Find("TextScript").GetComponent<Text>();
        m_textGuildNoticeScript.text = CsGuildManager.Instance.Notice;
        CsUIData.Instance.SetFont(m_textGuildNoticeScript);

        m_buttonNotice = trNotice.Find("ButtonNotice").GetComponent<Button>();
        m_buttonNotice.onClick.RemoveAllListeners();
        m_buttonNotice.onClick.AddListener(() => OnClickPopupNotice(true));
        m_buttonNotice.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //영지입장
        Button buttonGuildManorEnter = transform.Find("ButtonGuildManorEnter").GetComponent<Button>();
        buttonGuildManorEnter.onClick.RemoveAllListeners();
        buttonGuildManorEnter.onClick.AddListener(OnClickGuildManorEnter);
        buttonGuildManorEnter.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGuildManorEnter = buttonGuildManorEnter.transform.Find("Text").GetComponent<Text>();
        textGuildManorEnter.text = CsConfiguration.Instance.GetString("A58_BTN_00007");
        CsUIData.Instance.SetFont(textGuildManorEnter);

        //길드멤버셋팅
        CsGuildManager.Instance.GuildMemberList.Sort(SortToMember);

        InitializeMember();
        DisplayDonate();
        m_bFirst = false;

        //길드 보상
        Button buttonGuildReward = transform.Find("ButtonGuildReward").GetComponent<Button>();
        buttonGuildReward.onClick.RemoveAllListeners();
        buttonGuildReward.onClick.AddListener(OnClickOpenPopupGuildReward);
        buttonGuildReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonGuildReward = buttonGuildReward.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonGuildReward);
        textButtonGuildReward.text = CsConfiguration.Instance.GetString("A58_BTN_00010");

        m_trPopupGuildReward = transform.Find("PopupGuildReward");
        InitializePopupGuildReward();

        m_trImageRewardNotice = buttonGuildReward.transform.Find("ImageNotice");
        m_trImageDonateNotice = buttonDedicate.transform.Find("ImageNotice");

        UpdateGuildRewardNotice();
        UpdateDonateNotcie();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeMember()
    {
        for (int i = 0; i < m_trContent.childCount; ++i)
        {
            m_trContent.GetChild(i).gameObject.SetActive(false);
        }

        int nItemSizeY = 95;
        int nBaseLoadCount = 10;

        List<CsGuildMember> listMember = CsGuildManager.Instance.GuildMemberList;
        listMember.Sort(SortToMember);

        RectTransform rectTransform = m_trContent.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSizeY * listMember.Count);

        Scrollbar scrollbar = transform.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.RemoveAllListeners();
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedGuildMemberScrollbar(scrollbar));

        if (m_bFirst)
        {
            if (listMember.Count < nBaseLoadCount)
            {
                nBaseLoadCount = listMember.Count;
            }
            for (int i = 0; i < nBaseLoadCount; i++)
            {
                CreateMember(listMember[i], i);
            }
        }
        else
        {
            m_nLoadMemberItemCount = 0;
            m_nStandardPosition = 0;
            OnValueChangedGuildMemberScrollbar(scrollbar);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayGuildInfo()
    {
        Transform trGuildInfoList = transform.Find("GuildInfoList");

        Image imageGuildLevel = transform.Find("ImageGuildLevel").GetComponent<Image>();
        imageGuildLevel.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/ico_guild_lv_" + CsGuildManager.Instance.Level);

        Text textMemberCountValue = trGuildInfoList.Find("GuildMemberCount/TextValue").GetComponent<Text>();
        textMemberCountValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGuildManager.Instance.GuildMemberList.Count,
            CsGameData.Instance.GetGuildLevel(CsGuildManager.Instance.Level).MaxMemberCount);

        Text textGuildConstructionPointValue = trGuildInfoList.Find("GuildConstructionPoint/TextValue").GetComponent<Text>();
        textGuildConstructionPointValue.text = CsGuildManager.Instance.BuildingPoint.ToString("#,##0");

        Text textGuildFundsValue = trGuildInfoList.Find("GuildFunds/TextValue").GetComponent<Text>();
        textGuildFundsValue.text = CsGuildManager.Instance.Fund.ToString("#,##0");

        Text textGuildContributionPointValue = trGuildInfoList.Find("GuildContributionPoint/TextValue").GetComponent<Text>();
        textGuildContributionPointValue.text = CsGuildManager.Instance.GuildContributionPoint.ToString("#,##0");

        Transform trMember = m_trContent.Find(CsGameData.Instance.MyHeroInfo.HeroId.ToString());

        if (trMember != null)
        {
            Text textContributionPoint = trMember.Find("TextContributionPoint").GetComponent<Text>();
            textContributionPoint.text = CsGuildManager.Instance.TotalGuildContributionPoint.ToString("#,##0");
            CsUIData.Instance.SetFont(textContributionPoint);
        }
    }

    //---------------------------------------------------------------------------------------------------
    int SortToMember(CsGuildMember A, CsGuildMember B)
    {
        //로그인 여부
        if (!A.IsLoggedIn && B.IsLoggedIn)
            return 1;
        else if (A.IsLoggedIn && !B.IsLoggedIn)
            return -1;
        else
        {
            //관직 오름차
            if (A.GuildMemberGrade.MemberGrade > B.GuildMemberGrade.MemberGrade)
                return 1;
            else if (A.GuildMemberGrade.MemberGrade < B.GuildMemberGrade.MemberGrade)
                return -1;
            else
            {
                //레벨 내림차
                if (A.Level < B.Level)
                    return 1;
                else if (A.Level > B.Level)
                    return -1;
                else
                {
                    //이름 오름차
                    if (A.Name.CompareTo(B.Name) > 0) return 1;
                    else if (A.Name.CompareTo(B.Name) < 0) return -1;
                    else
                        return 0;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateMember(CsGuildMember csGuildMember, int nIndex)
    {
        Transform trMember = m_trContent.Find(csGuildMember.Id.ToString());

        if (trMember == null)
        {
            trMember = Instantiate(m_goGuildMemberItem, m_trContent).transform;
            trMember.name = csGuildMember.Id.ToString();
        }

        Button buttonGuildReference = trMember.GetComponent<Button>();
        buttonGuildReference.onClick.RemoveAllListeners();
        buttonGuildReference.onClick.AddListener(() => OnClickReference(csGuildMember));
        buttonGuildReference.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Image imageMemberJob = trMember.Find("ImageJob").GetComponent<Image>();
        imageMemberJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csGuildMember.Job.JobId);

        Text textHeroLevel = trMember.Find("TextHeroLevel").GetComponent<Text>();
        textHeroLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csGuildMember.Level);
        CsUIData.Instance.SetFont(textHeroLevel);

        Text textHeroName = trMember.Find("TextHeroName").GetComponent<Text>();
        textHeroName.text = csGuildMember.Name;
        CsUIData.Instance.SetFont(textHeroName);

        Text textGuildGrade = trMember.Find("TextGuildGrade").GetComponent<Text>();
        textGuildGrade.text = CsConfiguration.Instance.GetString("A58_TXT_00008");
        CsUIData.Instance.SetFont(textGuildGrade);

        Text textGradeValue = trMember.Find("TextGradeValue").GetComponent<Text>();
        textGradeValue.text = csGuildMember.GuildMemberGrade.Name;
        CsUIData.Instance.SetFont(textGradeValue);

        Text textGuildContribution = trMember.Find("TextGuildContribution").GetComponent<Text>();
        textGuildContribution.text = CsConfiguration.Instance.GetString("A58_TXT_00009");
        CsUIData.Instance.SetFont(textGuildContribution);

        Text textContributionPoint = trMember.Find("TextContributionPoint").GetComponent<Text>();
        textContributionPoint.text = csGuildMember.TotalContributionPoint.ToString("#,##0");
        CsUIData.Instance.SetFont(textContributionPoint);

        Text textAccess = trMember.Find("TextAccess").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAccess);

        if (csGuildMember.IsLoggedIn)
        {
            textAccess.text = CsConfiguration.Instance.GetString("A58_TXT_00010");
            textAccess.color = CsUIData.Instance.ColorGreen;
        }
        else
        {
            int nTime = (int)(csGuildMember.LogoutElapsedTime / 60f);
            textAccess.color = CsUIData.Instance.ColorGray;
            if (nTime < 60)
            {
                //분
                textAccess.text = string.Format(CsConfiguration.Instance.GetString("A58_TXT_01004"), nTime);
            }
            else if (nTime >= 60 && nTime < 1440)
            {
                //시
                nTime /= 60;
                textAccess.text = string.Format(CsConfiguration.Instance.GetString("A58_TXT_01001"), nTime);
            }
            else
            {
                //일
                nTime /= 1440;
                textAccess.text = string.Format(CsConfiguration.Instance.GetString("A58_TXT_01002"), nTime);
            }
        }

        m_nLoadMemberItemCount++;
        trMember.Find("ImageOnline").gameObject.SetActive(csGuildMember.IsLoggedIn);
        trMember.Find("ImageOffline").gameObject.SetActive(!csGuildMember.IsLoggedIn);
        trMember.SetSiblingIndex(nIndex);
        trMember.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayApply()
    {
        for (int i = 0; i < m_trApplyContent.childCount; ++i)
        {
            m_trApplyContent.GetChild(i).gameObject.SetActive(false);
        }

        List<CsGuildApplication> listApplication = CsGuildManager.Instance.GuildApplicationList;

        for (int i = 0; i < listApplication.Count; i++)
        {
            CreateApply(listApplication[i]);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void DisplayDonate()
    {
        m_trPopupGuildDonate = transform.Find("PopupGuildDonate");
        Transform trBackground = m_trPopupGuildDonate.Find("ImageBackground");

        Text textPopupName = trBackground.Find("TextPopupName").GetComponent<Text>();
        textPopupName.text = CsConfiguration.Instance.GetString("A58_TXT_00012");
        CsUIData.Instance.SetFont(textPopupName);

        Text textDonate = trBackground.Find("TextDonate").GetComponent<Text>();
        textDonate.text = CsConfiguration.Instance.GetString("A58_TXT_00013");
        CsUIData.Instance.SetFont(textDonate);

        Text textGuildGold = trBackground.Find("TextGuildGold").GetComponent<Text>();
        textGuildGold.text = CsConfiguration.Instance.GetString("A58_TXT_00014");
        CsUIData.Instance.SetFont(textGuildGold);

        Button buttonDedicateClose = trBackground.Find("ButtonClose").GetComponent<Button>();
        buttonDedicateClose.onClick.RemoveAllListeners();
        buttonDedicateClose.onClick.AddListener(() => OnClickPopupDonate(false));
        buttonDedicateClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trDonateList = trBackground.Find("DonateList");

        for (int i = 0; i < 3; ++i)
        {
            CsGuildDonationEntry csGuildDonationEntry = CsGameData.Instance.GetGuildDonationEntry(i + 1);
            int nEntryId = csGuildDonationEntry.EntryId;
            Transform trDonateItem = trDonateList.Find("DonateItem" + i + "/ImageBackground");

            Text textDonateName = trDonateItem.Find("Text").GetComponent<Text>();
            textDonateName.text = csGuildDonationEntry.Name;
            CsUIData.Instance.SetFont(textDonateName);

            Text textExploit = trDonateItem.Find("Exploit/Text").GetComponent<Text>();
            textExploit.text = CsConfiguration.Instance.GetString("A58_TXT_00015");
            CsUIData.Instance.SetFont(textExploit);

            Text textExploitValue = trDonateItem.Find("Exploit/TextValue").GetComponent<Text>();
            textExploitValue.text = csGuildDonationEntry.GuildContributionPointReward.Value.ToString("#,##0");
            CsUIData.Instance.SetFont(textExploitValue);

            Text textGold = trDonateItem.Find("Gold/Text").GetComponent<Text>();
            textGold.text = CsConfiguration.Instance.GetString("A58_TXT_00016");
            CsUIData.Instance.SetFont(textGold);

            Text textGoldValue = trDonateItem.Find("Gold/TextValue").GetComponent<Text>();
            textGoldValue.text = csGuildDonationEntry.GuildFundReward.Value.ToString("#,##0");
            CsUIData.Instance.SetFont(textGoldValue);

            Button buttonDonate = trDonateItem.Find("ButtonDonate").GetComponent<Button>();
            buttonDonate.onClick.RemoveAllListeners();
            buttonDonate.onClick.AddListener(() => OnClickDonate(nEntryId));
            buttonDonate.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image imageIcon = buttonDonate.transform.Find("ImageIcon").GetComponent<Image>();

            if (csGuildDonationEntry.MoneyType == 1)
            {
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods03");
            }
            else
            {
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods02");
            }

            Text textDonateValue = buttonDonate.transform.Find("Text").GetComponent<Text>();
            textDonateValue.text = csGuildDonationEntry.MoneyAmount.ToString("#,##0");
            CsUIData.Instance.SetFont(textDonateValue);
        }

        UpdateDonate();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDonate()
    {
        Transform trBackground = m_trPopupGuildDonate.Find("ImageBackground");

        Text textGoldValue = trBackground.Find("ImageBackgroundGold/TextValue").GetComponent<Text>();
        textGoldValue.text = CsGameData.Instance.MyHeroInfo.Gold.ToString("#,##0");
        CsUIData.Instance.SetFont(textGoldValue);

        Text textDiaValue = trBackground.Find("ImageBackgroundDia/TextValue").GetComponent<Text>();
        textDiaValue.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");
        CsUIData.Instance.SetFont(textDiaValue);

        Text textTodayCount = trBackground.Find("TextTodayCount").GetComponent<Text>();
        textTodayCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGuildManager.Instance.DailyGuildDonationCount, CsGameData.Instance.MyHeroInfo.VipLevel.GuildDonationMaxCount);
        CsUIData.Instance.SetFont(textTodayCount);

        Text textGuildGoldValue = trBackground.Find("TextGoldValue").GetComponent<Text>();
        textGuildGoldValue.text = CsGuildManager.Instance.Fund.ToString("#,##0");
        CsUIData.Instance.SetFont(textGuildGoldValue);

        for (int i = 0; i < 3; ++i)
        {
            CsGuildDonationEntry csGuildDonationEntry = CsGameData.Instance.GetGuildDonationEntry(i + 1);
            Transform trDonateItem = trBackground.Find("DonateList/DonateItem" + i + "/ImageBackground");
            Button buttonDonate = trDonateItem.Find("ButtonDonate").GetComponent<Button>();
            if (CsGuildManager.Instance.DailyGuildDonationCount >= CsGameData.Instance.MyHeroInfo.VipLevel.GuildDonationMaxCount)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonDonate, false);
                buttonDonate.transform.Find("ImageIcon").GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
            }
            else if (csGuildDonationEntry.MoneyType == 1)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonDonate, CsGameData.Instance.MyHeroInfo.Gold >= csGuildDonationEntry.MoneyAmount ? true : false);

                buttonDonate.transform.Find("ImageIcon").GetComponent<Image>().color = CsGameData.Instance.MyHeroInfo.Gold >= csGuildDonationEntry.MoneyAmount ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.7f);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonDonate, CsGameData.Instance.MyHeroInfo.Dia >= csGuildDonationEntry.MoneyAmount ? true : false);
                buttonDonate.transform.Find("ImageIcon").GetComponent<Image>().color = CsGameData.Instance.MyHeroInfo.Gold >= csGuildDonationEntry.MoneyAmount ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.7f);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateApply(CsGuildApplication csGuildApplication)
    {
        Transform trApplay = m_trApplyContent.Find(csGuildApplication.Id.ToString());

        if (trApplay == null)
        {
            trApplay = Instantiate(m_goGuildApplyItem, m_trApplyContent).transform;
            trApplay.name = csGuildApplication.Id.ToString();
        }

        Image imageJob = trApplay.Find("ImageJob").GetComponent<Image>();
        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csGuildApplication.HeroJob.JobId);

        Text TextHeroInfo = trApplay.Find("TextHeroInfo").GetComponent<Text>();
        TextHeroInfo.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), csGuildApplication.HeroLevel, csGuildApplication.HeroName);
        CsUIData.Instance.SetFont(TextHeroInfo);

        Text TextHeroBattlePower = trApplay.Find("TextBattlePower").GetComponent<Text>();
        TextHeroBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), csGuildApplication.HeroBattlePower.ToString("#,###"));
        CsUIData.Instance.SetFont(TextHeroBattlePower);

        Button buttonAccept = trApplay.Find("ButtonAccept").GetComponent<Button>();
        buttonAccept.onClick.RemoveAllListeners();
        buttonAccept.onClick.AddListener(() => OnClickAccept(csGuildApplication.Id));
        buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAccept = buttonAccept.transform.Find("Text").GetComponent<Text>();
        textAccept.text = CsConfiguration.Instance.GetString("A58_BTN_00008");
        CsUIData.Instance.SetFont(textAccept);

        Button buttonRefuse = trApplay.Find("ButtonRefuse").GetComponent<Button>();
        buttonRefuse.onClick.RemoveAllListeners();
        buttonRefuse.onClick.AddListener(() => OnClickRefuse(csGuildApplication.Id));
        buttonRefuse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textRefuse = buttonRefuse.transform.Find("Text").GetComponent<Text>();
        textRefuse.text = CsConfiguration.Instance.GetString("A58_BTN_00009");
        CsUIData.Instance.SetFont(textRefuse);

        trApplay.gameObject.SetActive(true);
    }

    // 길드 보상 팝업 창
    //---------------------------------------------------------------------------------------------------
    void InitializePopupGuildReward()
    {
        Transform trImageBackground = m_trPopupGuildReward.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A58_TXT_00021");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopupGuildReward);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trContent = trImageBackground.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < c_nGuildRewardCount; i++)
        {
            Transform trGuildReward = trContent.Find("GuildRewardItem" + i);

            if (trGuildReward == null)
            {
                trGuildReward = Instantiate(m_goGuildRewardItem, trContent).transform;
                trGuildReward.name = "GuildRewardItem" + i;
            }

            Text textGuildReward = trGuildReward.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGuildReward);

            Text textGuildRewardDesc = trGuildReward.Find("TextDescription").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGuildRewardDesc);

            Transform trItemSlot = trGuildReward.Find("ItemSlot");

            CsItemReward csItemReward;

            int nIndex = i;

            // 보상 아이템 업데이트
            switch ((EnGuildReward)nIndex)
            {
                case EnGuildReward.GuildToday:
                    textGuildReward.text = CsConfiguration.Instance.GetString("A58_TXT_00022");
                    textGuildRewardDesc.text = CsConfiguration.Instance.GetString("A58_TXT_00023");
                    csItemReward = CsGameData.Instance.GetGuildLevel(CsGuildManager.Instance.Level).DailyItemReward;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    break;

                case EnGuildReward.GuildAltar:
                    textGuildReward.text = CsConfiguration.Instance.GetString("A58_TXT_00024");
                    textGuildRewardDesc.text = CsConfiguration.Instance.GetString("A58_TXT_00025");
                    csItemReward = CsGameData.Instance.GetGuildLevel(CsGuildManager.Instance.Level).AltarItemReward;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    break;

                case EnGuildReward.FoodWarehouse:
                    textGuildReward.text = CsConfiguration.Instance.GetString("A58_TXT_00026");
                    textGuildRewardDesc.text = CsConfiguration.Instance.GetString("A58_TXT_00027");
                    csItemReward = CsGameData.Instance.GuildFoodWarehouse.ItemRewardFullLevel;
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    break;

                case EnGuildReward.GuildHunter:
                    textGuildReward.text = CsConfiguration.Instance.GetString("A58_TXT_00028");
                    textGuildRewardDesc.text = CsConfiguration.Instance.GetString("A58_TXT_00029");
                    csItemReward = CsGameData.Instance.GetItemReward(CsGameConfig.Instance.GuildHuntingDonationCompletionItemRewardId);
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    break;
            }

            Button buttonReceive = trGuildReward.Find("ButtonReceive").GetComponent<Button>();
            buttonReceive.onClick.RemoveAllListeners();
            buttonReceive.onClick.AddListener(() => OnClickGuildRewardReceive(nIndex));
            buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            UpdateGuildRewardButtonReceive(nIndex);

            Text textButtonReceive = buttonReceive.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonReceive);
            textButtonReceive.text = CsConfiguration.Instance.GetString("A58_BTN_00011");

            Text textReceiveComplete = trGuildReward.Find("TextReceiveComplete").GetComponent<Text>();
            CsUIData.Instance.SetFont(textReceiveComplete);
            textReceiveComplete.text = CsConfiguration.Instance.GetString("A58_TXT_00030");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildRewardButtonReceive(int nIndex)
    {
        Transform trImageBackground = m_trPopupGuildReward.Find("ImageBackground");
        Transform trContent = trImageBackground.Find("Scroll View/Viewport/Content");

        Transform trGuildReward = trContent.Find("GuildRewardItem" + nIndex);
        Transform trReceiveComplete = trGuildReward.Find("TextReceiveComplete");


        if (trGuildReward == null)
        {
            return;
        }
        else
        {
            Button buttonReceive = trGuildReward.Find("ButtonReceive").GetComponent<Button>();

            switch ((EnGuildReward)nIndex)
            {
                case EnGuildReward.GuildToday:
                    if (CsGuildManager.Instance.GuildDailyRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
                    {
                        trReceiveComplete.gameObject.SetActive(true);
                        buttonReceive.gameObject.SetActive(false);
                    }
                    else
                    {
                        trReceiveComplete.gameObject.SetActive(false);
                        buttonReceive.gameObject.SetActive(true);
                    }
                    break;

                case EnGuildReward.FoodWarehouse:
                    if (CsGuildManager.Instance.Guild.FoodWarehouseCollectionId == System.Guid.Empty)
                    {
                        trReceiveComplete.gameObject.SetActive(false);
                        buttonReceive.gameObject.SetActive(true);
                        CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);
                    }
                    else
                    {
                        if (CsGuildManager.Instance.Guild.FoodWarehouseCollectionId == CsGuildManager.Instance.ReceivedGuildFoodWarehouseCollectionId)
                        {
                            trReceiveComplete.gameObject.SetActive(true);
                            buttonReceive.gameObject.SetActive(false);
                        }
                        else
                        {
                            CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
                        }
                    }
                    break;

                case EnGuildReward.GuildAltar:
                    if (CsGuildManager.Instance.GuildMoralPoint >= CsGameData.Instance.GuildAltar.DailyGuildMaxMoralPoint)
                    {
                        if (CsGuildManager.Instance.GuildAltarRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
                        {
                            trReceiveComplete.gameObject.SetActive(true);
                            buttonReceive.gameObject.SetActive(false);
                        }
                        else
                        {
                            trReceiveComplete.gameObject.SetActive(false);
                            buttonReceive.gameObject.SetActive(true);
                            CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
                        }
                    }
                    else
                    {
                        trReceiveComplete.gameObject.SetActive(false);
                        buttonReceive.gameObject.SetActive(true);
                        CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);
                    }
                    break;

                case EnGuildReward.GuildHunter:
                    if (CsGuildManager.Instance.Guild.DailyHuntingDonationCount >= CsGameConfig.Instance.GuildHuntingDonationMaxCount)
                    {
                        if (CsGuildManager.Instance.GuildHuntingDonationRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
                        {
                            trReceiveComplete.gameObject.SetActive(true);
                            buttonReceive.gameObject.SetActive(false);
                        }
                        else
                        {
                            trReceiveComplete.gameObject.SetActive(false);
                            buttonReceive.gameObject.SetActive(true);
                            CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
                        }
                    }
                    else
                    {
                        trReceiveComplete.gameObject.SetActive(false);
                        buttonReceive.gameObject.SetActive(true);
                        CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);
                    }
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildRewardNotice()
    {
        bool bCheck = false;

        if (CsGuildManager.Instance.GuildDailyRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
        {
            bCheck = true;
        }
        else if (CsGuildManager.Instance.Guild.MoralPoint >= CsGameData.Instance.GuildAltar.DailyGuildMaxMoralPoint
            && CsGuildManager.Instance.GuildAltarRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
        {
            bCheck = true;
        }
        else if (CsGuildManager.Instance.Guild.FoodWarehouseCollectionId != System.Guid.Empty
             && CsGuildManager.Instance.Guild.FoodWarehouseCollectionId != CsGuildManager.Instance.ReceivedGuildFoodWarehouseCollectionId)
        {
            bCheck = true;
        }

        else if (CsGuildManager.Instance.Guild.DailyHuntingDonationCount >= CsGameConfig.Instance.GuildHuntingDonationMaxCount
            && CsGuildManager.Instance.GuildHuntingDonationRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
        {
            bCheck = true;
        }

        m_trImageRewardNotice.gameObject.SetActive(bCheck);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDonateNotcie()
    {
        bool bCheck = false;

        if (CsGuildManager.Instance.DailyGuildDonationCount < CsGameData.Instance.MyHeroInfo.VipLevel.GuildDonationMaxCount)
        {
            for (int i = 0; i < CsGameData.Instance.GuildDonationEntryList.Count; ++i)
            {
                CsGuildDonationEntry csGuildDonationEntry = CsGameData.Instance.GetGuildDonationEntry(i + 1);

                if (csGuildDonationEntry.MoneyType == 1)
                {
                    if (CsGameData.Instance.MyHeroInfo.Gold >= csGuildDonationEntry.MoneyAmount)
                    {
                        bCheck = true;
                        break;
                    }
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.Dia >= csGuildDonationEntry.MoneyAmount)
                    {
                        bCheck = true;
                        break;
                    }
                }
            }
        }

        m_trImageDonateNotice.gameObject.SetActive(bCheck);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildApply()
    {
        if (CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).ApplicationAcceptanceEnabled)
        {
            if (!m_buttonApply.gameObject.activeSelf)
            {
                m_buttonApply.gameObject.SetActive(true);
            }

            if (CsGuildManager.Instance.Guild.ApplicationCount > 0)
            {
                m_buttonApply.transform.Find("ImageNotice").gameObject.SetActive(true);

            }
            else
            {
                m_buttonApply.transform.Find("ImageNotice").gameObject.SetActive(false);

            }
        }
        else
        {
            if (m_buttonApply.gameObject.activeSelf)
            {
                m_buttonApply.gameObject.SetActive(false);
            }
        }
    }
}
