using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EnGuildContentType
{
    GuildMission = 1,
    GuildHunting,
    GuildAltar,
    GuildFoodWarehouse,
    GuildFarmQuest,
    GuildHuntingDonate,
    GuildSupply,
}

public class CsPopupGuildEvent : CsPopupSub
{
    [SerializeField] GameObject m_goGuildEventItem;
    [SerializeField] GameObject m_goGuildCompletedMember;

    Transform m_trEventContent;
    Transform m_trCompletedContent;
    Transform m_trDailyReward;
    Transform m_trWeeklyReward;

    Transform m_trPopupCompleted;
    Transform m_trPopupRewardTarget;
    Transform m_trPopupDailyReward;

    Transform m_trImageNotice;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGuildManager.Instance.EventGuildWeeklyObjectiveSetEvent += OnEventGuildWeeklyObjectiveSetEvent;
        CsGuildManager.Instance.EventGuildWeeklyObjectiveRewardReceive += OnEventGuildWeeklyObjectiveRewardReceive;
        CsGuildManager.Instance.EventGuildWeeklyObjectiveCompletionMemberCountUpdated += OnEventGuildWeeklyObjectiveCompletionMemberCountUpdated;

        CsGuildManager.Instance.EventGuildDailyObjectiveCompletionMemberList += OnEventGuildDailyObjectiveCompletionMemberList;
        CsGuildManager.Instance.EventGuildDailyObjectiveSet += OnEventGuildDailyObjectiveSet;
        CsGuildManager.Instance.EventGuildDailyObjectiveRewardReceive += OnEventGuildDailyObjectiveRewardReceive;
        CsGuildManager.Instance.EventGuildDailyObjectiveCompletionMemberCountUpdated += OnEventGuildDailyObjectiveCompletionMemberCountUpdated;

        CsGuildManager.Instance.EventGuildMissionComplete += OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail += OnEventGuildSupplySupportQuestFail;
        CsGuildManager.Instance.EventGuildSupplySupportQuestCompleted += OnEventGuildSupplySupportQuestCompleted;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGuildManager.Instance.EventGuildWeeklyObjectiveSetEvent -= OnEventGuildWeeklyObjectiveSetEvent;
        CsGuildManager.Instance.EventGuildWeeklyObjectiveRewardReceive -= OnEventGuildWeeklyObjectiveRewardReceive;
        CsGuildManager.Instance.EventGuildWeeklyObjectiveCompletionMemberCountUpdated -= OnEventGuildWeeklyObjectiveCompletionMemberCountUpdated;

        CsGuildManager.Instance.EventGuildDailyObjectiveCompletionMemberList -= OnEventGuildDailyObjectiveCompletionMemberList;
        CsGuildManager.Instance.EventGuildDailyObjectiveSet -= OnEventGuildDailyObjectiveSet;
        CsGuildManager.Instance.EventGuildDailyObjectiveRewardReceive -= OnEventGuildDailyObjectiveRewardReceive;
        CsGuildManager.Instance.EventGuildDailyObjectiveCompletionMemberCountUpdated -= OnEventGuildDailyObjectiveCompletionMemberCountUpdated;

        CsGuildManager.Instance.EventGuildMissionComplete -= OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail -= OnEventGuildSupplySupportQuestFail;
        CsGuildManager.Instance.EventGuildSupplySupportQuestCompleted -= OnEventGuildSupplySupportQuestCompleted;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildWeeklyObjectiveSetEvent()
    {
        if (m_trPopupRewardTarget.gameObject.activeSelf)
        {
            m_trPopupRewardTarget.gameObject.SetActive(false);
        }

        m_trImageNotice.gameObject.SetActive(CsGuildManager.Instance.CheckWeeklyObjectiveSettingEnabled());

        DisplayWeeklyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildWeeklyObjectiveRewardReceive()
    {
        UpdateWeeklyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildWeeklyObjectiveCompletionMemberCountUpdated()
    {
        UpdateWeeklyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDailyObjectiveCompletionMemberList(List<CsGuildDailyObjectiveCompletionMember> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            CreateCompletedMember(list[i]);
        }

        m_trPopupCompleted.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDailyObjectiveSet()
    {
        Transform trDailyMission = transform.Find("PanelLeft/DailyMission");
        Text textMissionName = trDailyMission.Find("TextMission").GetComponent<Text>();
        textMissionName.text = CsGameData.Instance.GetGuildContent(CsGuildManager.Instance.Guild.DailyObjectiveContentId).Name;

        UpdateDailyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDailyObjectiveRewardReceive()
    {
        UpdateDailyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDailyObjectiveCompletionMemberCountUpdated()
    {
        UpdateDailyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionComplete(bool bLevelUp, long lAcquredExp)
    {
        UpdateGuildEvent(CsGameData.Instance.GetGuildContent((int)EnGuildContentType.GuildMission));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestFail()
    {
        UpdateGuildEvent(CsGameData.Instance.GetGuildContent((int)EnGuildContentType.GuildSupply));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestCompleted(bool bLevelUp, long lAcquredExp)
    {
        UpdateGuildEvent(CsGameData.Instance.GetGuildContent((int)EnGuildContentType.GuildSupply));
    }

    #endregion Event

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildNotice()
    {
        if (CsGuildManager.Instance.GuildDailyObjectiveNoticeRemainingCoolTime < Time.realtimeSinceStartup)
        {
            CsGuildManager.Instance.SendGuildDailyObjectiveNotice();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A58_TXT_02022"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCompletedMember()
    {
        CsGuildManager.Instance.SendGuildDailyObjectiveCompletionMemberList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReward(int nRewardNo)
    {
        int nRewarded = CsGuildManager.Instance.GuildDailyObjectiveRewardReceivedNo;
        int nCount = CsGuildManager.Instance.Guild.DailyObjectiveCompletionMemberCount;
        int nMaxCount = CsGameData.Instance.GetGuildDailyObjectiveReward(3).CompletionMemberCount;

        //Transform trRewardList = m_trDailyReward.Find("RewardButtonList");
        CsGuildDailyObjectiveReward csGuildDailyObjectiveReward = CsGameData.Instance.GetGuildDailyObjectiveReward(nRewardNo);

        //보상을 안받았을때 
        if (nRewardNo > nRewarded)
        {
            //보상 조건 만족
            if (nCount >= csGuildDailyObjectiveReward.CompletionMemberCount && nRewardNo - 1 == nRewarded)
            {
                CsGuildManager.Instance.SendGuildDailyObjectiveRewardReceive();
            }
            else
            {
                DisplayDailyRewardItem(csGuildDailyObjectiveReward.ItemReward1, csGuildDailyObjectiveReward.ItemReward2, csGuildDailyObjectiveReward.ItemReward3);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("이미 받은 보상입니다."));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWeeklyReward()
    {
        int nCount = CsGuildManager.Instance.Guild.WeeklyObjectiveCompletionMemberCount;
        int nMaxCount = CsGameData.Instance.GetGuildWeeklyObjective(CsGuildManager.Instance.Guild.WeeklyObjectiveId).CompletionMemberCount;

        CsGuildWeeklyObjective csGuildWeeklyObjective = CsGameData.Instance.GetGuildWeeklyObjective(CsGuildManager.Instance.Guild.WeeklyObjectiveId);

        if (CsGuildManager.Instance.GuildWeeklyObjectiveRewardReceivedDate.Date.CompareTo(CsGuildManager.Instance.Guild.WeeklyObjectiveDate) == 0)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("이미 받은 보상입니다."));
        }
        else
        {
            if (nCount < nMaxCount)
            {
                DisplayDailyRewardItem(csGuildWeeklyObjective.ItemReward1, csGuildWeeklyObjective.ItemReward2, csGuildWeeklyObjective.ItemReward3);
            }
            else
            {
                CsGuildManager.Instance.SendGuildWeeklyObjectiveRewardReceive();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRewardTargetPopupOpen()
    {
        if (CsGuildManager.Instance.MyGuildMemberGrade.WeeklyObjectiveSettingEnabled)
        {
            m_trPopupRewardTarget.gameObject.SetActive(true);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("권한이 없습니다."));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose(Transform trPopup)
    {
        trPopup.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTargetSelectAlert(int nObjectiveId)
    {
        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A60_TXT_00010"),
              CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGuildManager.Instance.SendGuildWeeklyObjectiveSet(nObjectiveId),
              CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);

    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildEvent(int nGuildContentId)
    {
		Debug.Log((EnGuildContentType)nGuildContentId);
        if (CsGameData.Instance.MyHeroInfo.LocationId != 201)
        {
            switch ((EnGuildContentType)nGuildContentId)
            {
                case EnGuildContentType.GuildMission:
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildMission);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

                case EnGuildContentType.GuildSupply:
                    if (CsGuildManager.Instance.MyGuildMemberGrade.GuildSupplySupportQuestEnabled)
                    {
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildSupplySupport);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("권한 없음"));
                    }
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

                case EnGuildContentType.GuildHunting:
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildHunting);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;
                case EnGuildContentType.GuildHuntingDonate:
                    CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(CsGuildManager.Instance.GuildHuntingQuest.QuestNpcId);

                    if (csNpcInfo != null)
                    {
                        CsUIData.Instance.AutoStateType = EnAutoStateType.Move;

						CsGameEventToIngame.Instance.OnEventNpcAutoMove(CsGameData.Instance.MyHeroInfo.Nation.NationId, csNpcInfo);
                        CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Move);
                    }

                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

                case EnGuildContentType.GuildAltar:
                case EnGuildContentType.GuildFarmQuest:
                case EnGuildContentType.GuildFoodWarehouse:
                    CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                             CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                             CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                    break;

            }
        }
        else
        {
			switch ((EnGuildContentType)nGuildContentId)
            {
				case EnGuildContentType.GuildMission:
				case EnGuildContentType.GuildSupply:
				case EnGuildContentType.GuildHunting:
				case EnGuildContentType.GuildHuntingDonate:
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_03001"));
                    break;

				case EnGuildContentType.GuildAltar:
                    if (CsGuildManager.Instance.GuildPlayAutoState == EnGuildPlayState.Altar && CsGuildManager.Instance.Auto)
                        return;
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildAlter);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

				case EnGuildContentType.GuildFarmQuest:
                    if (CsGuildManager.Instance.GuildPlayAutoState == EnGuildPlayState.FarmQuest && CsGuildManager.Instance.Auto)
                        return;
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildFarm);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;

				case EnGuildContentType.GuildFoodWarehouse:
                    if (CsGuildManager.Instance.GuildPlayAutoState == EnGuildPlayState.FoodWareHouse && CsGuildManager.Instance.Auto)
                        return;

                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildFoodWareHouse);
                    CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                    break;
            }
        }
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trEventContent = transform.Find("PanelRight/Scroll View/Viewport/Content");
        m_trPopupRewardTarget = transform.Find("PopupRewardTarget");
        m_trPopupDailyReward = transform.Find("PopupDailyReward");

        // max 카운트 초기화
        for (int i = 0; i < CsGameData.Instance.TodayTaskList.Count; i++)
        {
            CsGameData.Instance.TodayTaskList[i].Init();
        }

        List<CsGuildContent> listGuildContent = CsGameData.Instance.GuildContentList;

        for (int i = 0; i < listGuildContent.Count; ++i)
        {
            CreateGuildEvent(listGuildContent[i]);
        }

        //금일 목표
        Transform trDailyMission = transform.Find("PanelLeft/DailyMission");

        Text textMission = trDailyMission.Find("Text").GetComponent<Text>();
        textMission.text = CsConfiguration.Instance.GetString("A60_TXT_00001");
        CsUIData.Instance.SetFont(textMission);

        Text textMissionName = trDailyMission.Find("TextMission").GetComponent<Text>();
        textMissionName.text = CsGameData.Instance.GetGuildContent(CsGuildManager.Instance.Guild.DailyObjectiveContentId).Name;
        CsUIData.Instance.SetFont(textMissionName);

        Button buttonNotice = trDailyMission.Find("ButtonNotice").GetComponent<Button>();
        buttonNotice.onClick.RemoveAllListeners();
        buttonNotice.onClick.AddListener(OnClickGuildNotice);
        buttonNotice.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNotcie = buttonNotice.transform.Find("Text").GetComponent<Text>();
        textNotcie.text = CsConfiguration.Instance.GetString("A60_BTN_00001");
        CsUIData.Instance.SetFont(textNotcie);

        Button buttonProgress = trDailyMission.Find("ButtonProgress").GetComponent<Button>();
        buttonProgress.onClick.RemoveAllListeners();
        buttonProgress.onClick.AddListener(OnClickCompletedMember);
        buttonProgress.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textProgress = buttonProgress.transform.Find("Text").GetComponent<Text>();
        textProgress.text = CsConfiguration.Instance.GetString("A60_BTN_00002");
        CsUIData.Instance.SetFont(textProgress);

        //매일 목표 보상
        m_trDailyReward = transform.Find("PanelLeft/DailyReward");

        Text textDailyReward = m_trDailyReward.Find("Text").GetComponent<Text>();
        textDailyReward.text = CsConfiguration.Instance.GetString("A60_TXT_00002");
        CsUIData.Instance.SetFont(textDailyReward);

        Transform trRewardList = m_trDailyReward.Find("RewardButtonList");
        List<CsGuildDailyObjectiveReward> listReward = CsGameData.Instance.GuildDailyObjectiveRewardList;

        for (int i = 0; i < listReward.Count; ++i)
        {
            int nRewardNo = listReward[i].RewardNo;
            Button buttonReward = trRewardList.Find("ButtonReward" + nRewardNo).GetComponent<Button>();
            buttonReward.onClick.RemoveAllListeners();
            buttonReward.onClick.AddListener(() => OnClickReward(nRewardNo));
            buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textReward = buttonReward.transform.Find("Text").GetComponent<Text>();
            textReward.text = listReward[i].CompletionMemberCount.ToString("#,##0");
            CsUIData.Instance.SetFont(textReward);
        }

        UpdateDailyReward();

        //매주 목표 보상

        m_trWeeklyReward = transform.Find("PanelLeft/WeeklyReward");

        Text textWeekly = m_trWeeklyReward.Find("Text").GetComponent<Text>();
        textWeekly.text = CsConfiguration.Instance.GetString("A60_TXT_00003");
        CsUIData.Instance.SetFont(textWeekly);

        if (CsGuildManager.Instance.Guild.WeeklyObjectiveId == 0 && CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Monday)
        {
            DisplayWeeklyRewardTarget();
        }
        else
        {
            DisplayWeeklyReward();
        }

        //일일퀘 완료 멤버
        m_trPopupCompleted = transform.Find("PopupCompletedMember");

        Button buttonPopupCompletedClose = m_trPopupCompleted.Find("ButtonExit").GetComponent<Button>();
        buttonPopupCompletedClose.onClick.RemoveAllListeners();
        buttonPopupCompletedClose.onClick.AddListener(() => OnClickPopupClose(m_trPopupCompleted));
        buttonPopupCompletedClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCompletedName = m_trPopupCompleted.Find("ImageBackground/Text").GetComponent<Text>();
        textCompletedName.text = CsConfiguration.Instance.GetString("A60_BTN_00002");
        CsUIData.Instance.SetFont(textCompletedName);

        m_trCompletedContent = m_trPopupCompleted.Find("ImageBackground/Scroll View/Viewport/Content");

        m_trImageNotice = m_trWeeklyReward.Find("ButtonRewardTarget/ImageNotice");
        m_trImageNotice.gameObject.SetActive(CsGuildManager.Instance.CheckWeeklyObjectiveSettingEnabled());
    }

    //---------------------------------------------------------------------------------------------------
    void CreateGuildEvent(CsGuildContent csGuildContent)
    {
        int nContentId = csGuildContent.GuildContentId;

        Transform trEvent = Instantiate(m_goGuildEventItem, m_trEventContent).transform;
        trEvent.name = nContentId.ToString();

        Image imageIcon = trEvent.Find("Image").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/guildtask_" + csGuildContent.GuildContentId);

        Text textEventName = trEvent.Find("TextEventName").GetComponent<Text>();
        textEventName.text = csGuildContent.Name;
        CsUIData.Instance.SetFont(textEventName);

        Text textDescription = trEvent.Find("TextDescription").GetComponent<Text>();
        textDescription.text = csGuildContent.RewardText;
        CsUIData.Instance.SetFont(textDescription);

        Text textProgressCount = trEvent.Find("TextProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProgressCount);

        Button buttonMove = trEvent.Find("ButtonMove").GetComponent<Button>();
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(() => OnClickGuildEvent(nContentId));
        buttonMove.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textMove = buttonMove.transform.Find("Text").GetComponent<Text>();
        textMove.text = CsConfiguration.Instance.GetString("A60_BTN_00003");
        CsUIData.Instance.SetFont(textMove);

        Text textComplete = trEvent.Find("TextComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(textComplete);

        UpdateGuildEvent(csGuildContent);

    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildEvent(CsGuildContent csGuildContent)
    {
        bool bGuildSupplyFail = false;
        int nProgressCount = 0;
        int nProgressMaxCount = 0;
        CsHeroTodayTask csHeroTodayTask = null;

        Transform trEvent = m_trEventContent.Find(csGuildContent.GuildContentId.ToString());
        if (trEvent == null) return;

        Button buttonMove = trEvent.Find("ButtonMove").GetComponent<Button>();
        Text textComplete = trEvent.Find("TextComplete").GetComponent<Text>();

        switch ((EnGuildContentType)csGuildContent.GuildContentId)
        {
            case EnGuildContentType.GuildHuntingDonate:
                if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Day.CompareTo(CsGuildManager.Instance.GuildHuntingDonationDate.Day) == 0)
                {
                    nProgressCount = 1;
                }

                nProgressMaxCount = 1;
                break;
            case EnGuildContentType.GuildSupply:
                nProgressMaxCount = CsGameData.Instance.GetTodayTask(csGuildContent.TaskId).MaxCount;
                csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(csGuildContent.TaskId);

                if (csHeroTodayTask == null)
                {
                    nProgressCount = 0;

                    //길드물자지원 실패
                    if (csGuildContent.GuildContentId == (int)EnGuildContentType.GuildSupply
                        && CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.None
                        && CsGuildManager.Instance.Guild.DailyGuildSupplySupportQuestStartCount == 1)
                    {
                        nProgressCount = 1;
                        bGuildSupplyFail = true;
                    }
                }
                else
                {
                    nProgressCount = csHeroTodayTask.ProgressCount;
                }

                break;
            case EnGuildContentType.GuildHunting:
                nProgressCount = CsGuildManager.Instance.DailyGuildHuntingQuestStartCount;
                nProgressMaxCount = CsGuildManager.Instance.GuildHuntingQuest.LimitCount;
                break;

            case EnGuildContentType.GuildMission:
            case EnGuildContentType.GuildAltar:
            case EnGuildContentType.GuildFoodWarehouse:
            case EnGuildContentType.GuildFarmQuest:
                nProgressMaxCount = CsGameData.Instance.GetTodayTask(csGuildContent.TaskId).MaxCount;
                csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(csGuildContent.TaskId);

                if (csHeroTodayTask == null)
                {
                    nProgressCount = 0;
                }
                else
                {
                    nProgressCount = csHeroTodayTask.ProgressCount;
                }

                break;
        }

        Text textProgressCount = trEvent.Find("TextProgressCount").GetComponent<Text>();
        textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), nProgressCount, nProgressMaxCount);

        if (nProgressCount < nProgressMaxCount)
        {
            textComplete.gameObject.SetActive(false);

            if((EnGuildContentType)csGuildContent.GuildContentId == EnGuildContentType.GuildSupply
                && !CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).GuildSupplySupportQuestEnabled)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonMove, false);
            }

            buttonMove.gameObject.SetActive(true);
        }
        else
        {
            //길드 물자지원 실패
            if (bGuildSupplyFail)
            {
                textComplete.text = CsConfiguration.Instance.GetString("A60_TXT_00012");
                textComplete.color = CsUIData.Instance.ColorRed;
            }
            else
            {
                textComplete.text = CsConfiguration.Instance.GetString("A60_TXT_00011");
                textComplete.color = CsUIData.Instance.ColorGreen;
            }

            textComplete.gameObject.SetActive(true);
            buttonMove.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDailyReward()
    {
        int nRewarded = CsGuildManager.Instance.GuildDailyObjectiveRewardReceivedNo;

        int nCount = CsGuildManager.Instance.Guild.DailyObjectiveCompletionMemberCount;
        int nMaxCount = CsGameData.Instance.GetGuildDailyObjectiveReward(3).CompletionMemberCount;

        Slider sliderCount = m_trDailyReward.Find("Slider").GetComponent<Slider>();
        sliderCount.value = (float)nCount / nMaxCount;

        Transform trRewardList = m_trDailyReward.Find("RewardButtonList");
        List<CsGuildDailyObjectiveReward> listReward = CsGameData.Instance.GuildDailyObjectiveRewardList;

        for (int i = 0; i < listReward.Count; ++i)
        {
            int nRewardNo = listReward[i].RewardNo;
            Image imageReward = trRewardList.Find("ButtonReward" + nRewardNo).GetComponent<Image>();

            //보상을 안받았을때 
            if (nRewardNo > nRewarded)
            {
                //보상 조건 만족
                if (nCount >= listReward[i].CompletionMemberCount && nRewardNo - 1 == nRewarded)
                {
                    imageReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift02");
                }
                else
                {
                    imageReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                }
            }
            else
            {
                imageReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayWeeklyRewardTarget()
    {
        m_trWeeklyReward.Find("Slider").gameObject.SetActive(false);
        m_trWeeklyReward.Find("ButtonReward").gameObject.SetActive(false);

        Text textTime = m_trWeeklyReward.Find("TextTime").GetComponent<Text>();
        textTime.text = CsConfiguration.Instance.GetString("A60_TXT_00004");
        CsUIData.Instance.SetFont(textTime);
        textTime.gameObject.SetActive(true);

        Button buttonTarget = m_trWeeklyReward.Find("ButtonRewardTarget").GetComponent<Button>();
        buttonTarget.onClick.RemoveAllListeners();
        buttonTarget.onClick.AddListener(OnClickRewardTargetPopupOpen);
        buttonTarget.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonTarget.gameObject.SetActive(true);

        Text textTarget = buttonTarget.transform.Find("Text").GetComponent<Text>();
        textTarget.text = CsConfiguration.Instance.GetString("A60_BTN_00004");
        CsUIData.Instance.SetFont(textTarget);


        Text textPopupName = m_trPopupRewardTarget.Find("ImageBackground/Text").GetComponent<Text>();
        textPopupName.text = CsConfiguration.Instance.GetString("A60_TXT_00009");
        CsUIData.Instance.SetFont(textPopupName);

        Button buttonExit = m_trPopupRewardTarget.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(() => OnClickPopupClose(m_trPopupRewardTarget));
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        List<CsGuildWeeklyObjective> listWeekly = CsGameData.Instance.GuildWeeklyObjectiveList;

        for (int i = 0; i < listWeekly.Count; ++i)
        {
            Transform trReward = m_trPopupRewardTarget.Find("ImageBackground/RewardTarget" + listWeekly[i].ObjectiveId);

            if (trReward != null)
            {
                int nObjectiveId = listWeekly[i].ObjectiveId;

                Text textName = trReward.Find("TextName").GetComponent<Text>();
                textName.text = listWeekly[i].Name;
                CsUIData.Instance.SetFont(textName);

                Text textNumber = trReward.Find("TextNumber").GetComponent<Text>();
                textNumber.text = listWeekly[i].Description;
                CsUIData.Instance.SetFont(textNumber);

                Button buttonSelect = trReward.Find("ButtonSelect").GetComponent<Button>();
                buttonSelect.onClick.RemoveAllListeners();
                buttonSelect.onClick.AddListener(() => OnClickTargetSelectAlert(nObjectiveId));
                buttonSelect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textSelect = buttonSelect.transform.Find("Text").GetComponent<Text>();
                textSelect.text = CsConfiguration.Instance.GetString("A60_BTN_00006");
                CsUIData.Instance.SetFont(textSelect);

                DisplayItemReward(listWeekly[i].ItemReward1, trReward.Find("ItemSlot1"));
                DisplayItemReward(listWeekly[i].ItemReward2, trReward.Find("ItemSlot2"));
                DisplayItemReward(listWeekly[i].ItemReward3, trReward.Find("ItemSlot3"));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayWeeklyReward()
    {
        m_trWeeklyReward.Find("TextTime").gameObject.SetActive(false);
        m_trWeeklyReward.Find("ButtonRewardTarget").gameObject.SetActive(false);
        m_trWeeklyReward.Find("Slider").gameObject.SetActive(true);

        Button buttonReward = m_trWeeklyReward.Find("ButtonReward").GetComponent<Button>();
        buttonReward.onClick.RemoveAllListeners();
        buttonReward.onClick.AddListener(OnClickWeeklyReward);
        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonReward.gameObject.SetActive(true);

        Text textReward = buttonReward.transform.Find("Text").GetComponent<Text>();
        textReward.text = CsGameData.Instance.GetGuildWeeklyObjective(CsGuildManager.Instance.Guild.WeeklyObjectiveId).CompletionMemberCount.ToString("#,##0");
        CsUIData.Instance.SetFont(textReward);

        UpdateWeeklyReward();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWeeklyReward()
    {
        Image imageIcon = m_trWeeklyReward.Find("ButtonReward").GetComponent<Image>();

        int nCount = CsGuildManager.Instance.Guild.WeeklyObjectiveCompletionMemberCount;
        int nMaxCount = CsGameData.Instance.GetGuildWeeklyObjective(CsGuildManager.Instance.Guild.WeeklyObjectiveId).CompletionMemberCount;

        //보상 받음
        if (CsGuildManager.Instance.GuildWeeklyObjectiveRewardReceivedDate.Date.CompareTo(CsGuildManager.Instance.Guild.WeeklyObjectiveDate) == 0)
        {
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
        }
        else
        {
            if (nCount < nMaxCount)
            {
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
            }
            else
            {
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift02");
            }
        }

        Slider slider = m_trWeeklyReward.Find("Slider").GetComponent<Slider>();
        slider.value = (float)nCount / nMaxCount;
    }

    //---------------------------------------------------------------------------------------------------
    void CreateCompletedMember(CsGuildDailyObjectiveCompletionMember csMember)
    {
        Transform trMember = m_trCompletedContent.Find(csMember.Id.ToString());

        if (trMember == null)
        {
            trMember = Instantiate(m_goGuildCompletedMember, m_trCompletedContent).transform;
            trMember.name = csMember.Id.ToString();

            Text textName = trMember.Find("TextName").GetComponent<Text>();
            textName.text = csMember.Name;
            CsUIData.Instance.SetFont(textName);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayItemReward(CsItemReward csItemReward, Transform trItemSlot)
    {
        if (csItemReward != null)
        {
            Image imageIcon = trItemSlot.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);

            Image imageFrameRank = trItemSlot.Find("ImageFrameRank").GetComponent<Image>();
            imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csItemReward.Item.Grade.ToString("00"));

            Text textCount = trItemSlot.Find("TextCount").GetComponent<Text>();
            textCount.text = csItemReward.ItemCount.ToString("#,##0");
            CsUIData.Instance.SetFont(textCount);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayDailyRewardItem(CsItemReward csItemReward1, CsItemReward csItemReward2, CsItemReward csItemReward3)
    {
        Button buttonExit = m_trPopupDailyReward.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(() => OnClickPopupClose(m_trPopupDailyReward));
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trBack = m_trPopupDailyReward.Find("ImageBackground");
        Transform trItemList = trBack.Find("ItemRewardList");

        Text textName = trBack.Find("Text").GetComponent<Text>();
        textName.text = CsConfiguration.Instance.GetString("A60_TXT_00007");
        CsUIData.Instance.SetFont(textName);

        DisplayItemReward(csItemReward1, trItemList.Find("RewardItem1/ItemSlot"));
        Text textName1 = trItemList.Find("RewardItem1/TextName").GetComponent<Text>();
        textName1.text = csItemReward1.Item.Name;
        CsUIData.Instance.SetFont(textName1);

        DisplayItemReward(csItemReward2, trItemList.Find("RewardItem2/ItemSlot"));
        Text textName2 = trItemList.Find("RewardItem2/TextName").GetComponent<Text>();
        textName2.text = csItemReward2.Item.Name;
        CsUIData.Instance.SetFont(textName2);

        DisplayItemReward(csItemReward3, trItemList.Find("RewardItem3/ItemSlot"));
        Text textName3 = trItemList.Find("RewardItem3/TextName").GetComponent<Text>();
        textName3.text = csItemReward3.Item.Name;
        CsUIData.Instance.SetFont(textName3);

        m_trPopupDailyReward.gameObject.SetActive(true);
    }
}
