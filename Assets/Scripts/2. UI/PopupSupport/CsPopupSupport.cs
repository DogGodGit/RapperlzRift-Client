using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CsPopupSupport : CsUpdateableMonoBehaviour, IPopupMain
{
    GameObject m_goToggleSubMenu;
    GameObject m_goPopupItemInfo;

    Transform m_trMainPopupSubMenu;
    Transform m_trBackground;
    Transform m_trToggleList;
    Transform m_trPopupList;
    Transform m_trPopupItemInfo;

    CsMainMenu m_csMainMenu;
    CsSubMenu m_csSubMenu;
    EnSubMenu m_enSubMenu;

    Text m_textPopupName;

    float m_flTime = 0.0f;
    bool m_bLoadSubMenu = false;

    void Awake()
    {
        m_trMainPopupSubMenu = transform.Find("PopupSubMenu");
        m_trBackground = transform.Find("ImageBackground");

        m_textPopupName = m_trBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPopupName);

        m_trToggleList = m_trBackground.Find("ToggleList");

        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        // 레벨업 상황
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;

        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;

        CsGameEventUIToUI.Instance.EventLevelUpRewardReceive += OnEventLevelUpRewardReceive;
        CsGameEventUIToUI.Instance.EventDailyAccessTimeRewardReceive += OnEventDailyAccessTimeRewardReceive;
        CsGameEventUIToUI.Instance.EventAttendRewardReceive += OnEventAttendRewardReceive;

        CsGameEventUIToUI.Instance.EventTodayMissionRewardReceive += OnEventTodayMissionRewardReceive;
        CsGameEventUIToUI.Instance.EventTodayMissionListChanged += OnEventTodayMissionListChanged;
        CsGameEventUIToUI.Instance.EventTodayMissionUpdated += OnEventTodayMissionUpdated;

        CsGameEventUIToUI.Instance.EventSeriesMissionRewardReceive += OnEventSeriesMissionRewardReceive;
        CsGameEventUIToUI.Instance.EventSeriesMissionUpdated += OnEventSeriesMissionUpdated;

        CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;

        CsGameEventUIToUI.Instance.EventLimitationGiftRewardReceive += OnEventLimitationGiftRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // 버튼 초기화
        Button buttonClosePopup = m_trBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClosePopup.onClick.RemoveAllListeners();
        buttonClosePopup.onClick.AddListener(OnClickClosePopup);
        buttonClosePopup.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }

        if (m_flTime + 1.0f < Time.time)
        {
            if (m_bLoadSubMenu)
            {
                if (m_csSubMenu.EnSubMenu == EnSubMenu.AccessReward)
                {
                    UpdateTextAccessTime();
                }
                else if (m_csSubMenu.EnSubMenu == EnSubMenu.TodayMission)
                {
                    UpdateTextRemainingTimer();
                }

                UpdateNotice();

                m_flTime = Time.time;
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        // Destroy
        // 레벨업 상황
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;

        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;

        CsGameEventUIToUI.Instance.EventLevelUpRewardReceive -= OnEventLevelUpRewardReceive;
        CsGameEventUIToUI.Instance.EventDailyAccessTimeRewardReceive -= OnEventDailyAccessTimeRewardReceive;
        CsGameEventUIToUI.Instance.EventAttendRewardReceive -= OnEventAttendRewardReceive;

        CsGameEventUIToUI.Instance.EventTodayMissionRewardReceive -= OnEventTodayMissionRewardReceive;
        CsGameEventUIToUI.Instance.EventTodayMissionListChanged -= OnEventTodayMissionListChanged;
        CsGameEventUIToUI.Instance.EventTodayMissionUpdated -= OnEventTodayMissionUpdated;

        CsGameEventUIToUI.Instance.EventSeriesMissionRewardReceive -= OnEventSeriesMissionRewardReceive;
        CsGameEventUIToUI.Instance.EventSeriesMissionUpdated -= OnEventSeriesMissionUpdated;

        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;

        CsGameEventUIToUI.Instance.EventLimitationGiftRewardReceive -= OnEventLimitationGiftRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDateChanged()
    {
        UpdateSubMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventLimitationGiftRewardReceive()
    {
        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopup()
    {
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSubmenuTab(Toggle toggleSubmenuTab, int nToggleIndex)
    {
        m_csSubMenu = m_csMainMenu.SubMenuList[nToggleIndex];

        if (toggleSubmenuTab.isOn)
        {
            LoadSubMenu(m_csSubMenu.Prefabs[0], m_trMainPopupSubMenu);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            //기존에 켜진걸 끈다.
            for (int i = 0; i < m_trMainPopupSubMenu.childCount; i++)
            {
                m_trMainPopupSubMenu.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemSlot(CsItemReward csItemReward)
    {
        StartCoroutine(LoadPopupItemInfo(csItemReward));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        Destroy(gameObject);

        if (m_trPopupItemInfo)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Center);
        }
    }

    #region Implement Interface

    //---------------------------------------------------------------------------------------------------
    public CsSubMenu GetCurrentSubMenu()
    {
        return m_csSubMenu;
    }

    #endregion Implement Interface

    //---------------------------------------------------------------------------------------------------
    public void DisplayMenu(CsMainMenu csMainMenu, EnSubMenu enSubMenuID)
    {
        m_csMainMenu = csMainMenu;
        m_enSubMenu = enSubMenuID;

        m_textPopupName.text = m_csMainMenu.Name;

        StartCoroutine(LoadResourceToggleSubMenu());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadResourceToggleSubMenu()
    {
        ResourceRequest resourceRequestToggle = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupSupport/ToggleSupportContent");
        yield return resourceRequestToggle;
        m_goToggleSubMenu = (GameObject)resourceRequestToggle.asset;

        DisplaySubmenuTab();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySubmenuTab()
    {
        bool bSelected = false;

        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            int nToggleIndex = i;

            Toggle toggle = Instantiate(m_goToggleSubMenu, m_trToggleList).GetComponent<Toggle>();
            toggle.name = "Toggle" + m_csMainMenu.SubMenuList[nToggleIndex].SubMenuId;
            toggle.group = m_trToggleList.GetComponent<ToggleGroup>();
            toggle.onValueChanged.RemoveAllListeners();

            switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
            {
                case EnSubMenu.TodayMission:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.TodayMission))
                    {
                        toggle.gameObject.SetActive(true);
                    }
                    else
                    {
                        toggle.gameObject.SetActive(false);
                    }
                    break;

                case EnSubMenu.SeriesMission:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SeriesMisson))
                    {
                        toggle.gameObject.SetActive(true);
                    }
                    else
                    {
                        toggle.gameObject.SetActive(false);
                    }
                    break;

                case EnSubMenu.AccessReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AccessReward))
                    {
                        toggle.gameObject.SetActive(true);
                    }
                    else
                    {
                        toggle.gameObject.SetActive(false);
                    }
                    break;

                case EnSubMenu.AttendReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AttendReward))
                    {
                        toggle.gameObject.SetActive(true);
                    }
                    else
                    {
                        toggle.gameObject.SetActive(false);
                    }
                    break;

                case EnSubMenu.LevelUpReward:
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.LevelUpReward))
                    {
                        toggle.gameObject.SetActive(true);
                    }
                    else
                    {
                        toggle.gameObject.SetActive(false);
                    }
                    break;

                case EnSubMenu.LimitationGift:
                    
                    break;
            }

            //디폴트설정
            if (m_csMainMenu.SubMenuList[i].EnSubMenu == EnSubMenu.Default)
            {
                if (m_csMainMenu.SubMenuList[i].IsDefault)
                {
                    bSelected = toggle.isOn = true;
                    CreateSubMenu(nToggleIndex);
                }
                else
                {
                    toggle.isOn = false;
                }
            }
            else
            {
                if (m_csMainMenu.SubMenuList[i].EnSubMenu == m_enSubMenu)
                {
                    bSelected = toggle.isOn = true;
                    CreateSubMenu(nToggleIndex);
                }
                else
                {
                    toggle.isOn = false;
                }
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedSubmenuTab(toggle, nToggleIndex));

            //탭이름
            Text textSubmenuTab = toggle.transform.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textSubmenuTab);
            textSubmenuTab.text = m_csMainMenu.SubMenuList[i].Name;
        }

        // 디플트값이 없을 경우, 첫번째 탭을 선택한다.
        if (!bSelected)
        {
            Toggle toggle = m_trToggleList.Find("Toggle0").GetComponent<Toggle>();
            toggle.isOn = true;
            bSelected = true;
        }

        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSubMenu(int nToggleIndex)
    {
        m_csSubMenu = m_csMainMenu.SubMenuList[nToggleIndex];
        LoadSubMenu(m_csSubMenu.Prefabs[0], m_trMainPopupSubMenu);
    }

    //---------------------------------------------------------------------------------------------------
    void LoadSubMenu(string strPath, Transform trSubMenu)
    {
        if (!string.IsNullOrEmpty(strPath))
        {
            string strSubName = strPath;

            if (strPath.LastIndexOf("/") > -1)
            {
                strSubName = strPath.Substring(strPath.LastIndexOf("/") + 1);
            }

            Transform trSubMenuPrefab = trSubMenu.Find(strSubName);

            if (trSubMenuPrefab == null)
            {
                m_bLoadSubMenu = false;
                ToggleInteractable(m_bLoadSubMenu);
                StartCoroutine(LoadSubMenuCoroutine(strPath, trSubMenu, strSubName));
            }
            else
            {
                trSubMenuPrefab.gameObject.SetActive(true);
                UpdateSubMenu();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSubMenuCoroutine(string strPath, Transform trSubMenu, string strSubMenuName)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>(strPath);
        yield return resourceRequest;
        GameObject goSubMenu = Instantiate((GameObject)resourceRequest.asset, trSubMenu);
        goSubMenu.name = strSubMenuName;

        m_bLoadSubMenu = true;
        ToggleInteractable(m_bLoadSubMenu);

        Toggle toggle = null;
        
        toggle =m_trToggleList.Find("Toggle" + (int)EnSubMenu.LimitationGift).GetComponent<Toggle>();

        if (toggle == null)
        {

        }
        else
        {
            int nDayOfWeek = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek;

            if (CsGameData.Instance.LimitationGift.LimitationGiftRewardDayOfWeekList.FindIndex(a => a.DayOfWeek == nDayOfWeek) < 0)
            {
                toggle.interactable = false;
                toggle.transform.Find("TextName").GetComponent<Text>().color = new Color32(133, 141, 148, 255);
            }
            else
            {
                toggle.interactable = true;
                toggle.transform.Find("TextName").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
        }

        toggle = m_trToggleList.Find("Toggle" + (int)EnSubMenu.WeekendReward).GetComponent<Toggle>();

        if (toggle == null)
        {

        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Monday || 
                CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Friday ||
                CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Saturday ||
                CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                toggle.interactable = true;
                toggle.transform.Find("TextName").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                toggle.interactable = false;
                toggle.transform.Find("TextName").GetComponent<Text>().color = new Color32(133, 141, 148, 255);
            }
        }

        // 서브 메뉴 초기화
        switch (m_csSubMenu.EnSubMenu)
        {
            case EnSubMenu.TodayMission:
                InitializeTodayMission(goSubMenu.transform);
                break;

            case EnSubMenu.SeriesMission:
                InitializeSeriesMission(goSubMenu.transform);
                break;

            case EnSubMenu.AccessReward:
                InitializeAccessReward(goSubMenu.transform);
                break;

            case EnSubMenu.AttendReward:
                InitializeAttendReward(goSubMenu.transform);
                break;

            case EnSubMenu.LevelUpReward:
                InitializeLevelUpReward(goSubMenu.transform);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItemReward csItemReward)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csItemReward);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItemReward csItemReward)
    {
        m_trPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList).transform;

        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItemReward.Item, 0, csItemReward.ItemOwned, -1, false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trPopupItemInfo.gameObject);
        m_trPopupItemInfo = null;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSubMenu()
    {
        // 서브 메뉴 목록이 바뀔때마다 업데이트
        if (m_csSubMenu.EnSubMenu == EnSubMenu.LevelUpReward)
        {
            for (int i = 0; i < CsGameData.Instance.LevelUpRewardEntryList.Count; i++)
            {
                UpdateLevelUpRewardItem(i);
            }
        }
        else if (m_csSubMenu.EnSubMenu == EnSubMenu.AccessReward)
        {
            UpdateTextAccessTime();
        }
        else if (m_csSubMenu.EnSubMenu == EnSubMenu.AttendReward)
        {
            // 받은 날짜가 달라지면 출석 보상 일차 업데이트
            if (DateTime.Compare(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date, CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate.Date) == 1)
            {
                int nMaxDay = CsGameData.Instance.DailyAttendRewardEntryList[CsGameData.Instance.DailyAttendRewardEntryList.Count - 1].Day;

                if (CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay == nMaxDay)
                {
                    CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay = 0;
                }
            }

            for (int i = 0; i < CsGameData.Instance.DailyAttendRewardEntryList.Count; i++)
            {
                UpdateAttendRewardItem(i);
            }

            UpdateReceiveButton();
        }
        else if (m_csSubMenu.EnSubMenu == EnSubMenu.TodayMission)
        {
            UpdateTextRemainingTimer();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNotice()
    {
        for (int i = 0; i < m_trToggleList.childCount; i++)
        {
            Transform trToggleSubMenu = m_trToggleList.Find("Toggle" + m_csMainMenu.SubMenuList[i].SubMenuId);
            Transform trImageNotice = trToggleSubMenu.Find("ImageNotice");

            switch (m_csMainMenu.SubMenuList[i].EnSubMenu)
            {
                case EnSubMenu.TodayMission:

                    if (CheckNoticeTodayMission())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;

                case EnSubMenu.SeriesMission:

                    if (CheckNoticeSeriesMission())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;

                case EnSubMenu.AccessReward:

                    if (CheckNoticeAccessReward())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;

                case EnSubMenu.AttendReward:

                    if (CheckNoticeAttendReward())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;

                case EnSubMenu.LevelUpReward:

                    if (CheckNoticeLevelUpReward())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;

                case EnSubMenu.LimitationGift:

                    if (CheckNoticeLimitationGift())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;

                case EnSubMenu.WeekendReward:

                    if (CheckNoticeWeekendReward())
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }

                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ToggleInteractable(bool bInteractable)
    {
        for (int i = 0; i < m_trToggleList.childCount; i++)
        {
            m_trToggleList.GetChild(i).GetComponent<Toggle>().interactable = bInteractable;
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNoticeLimitationGift()
    {
        int nMyHeroDayOfWeek = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek;

        if (CsGameData.Instance.LimitationGift.LimitationGiftRewardDayOfWeekList.FindIndex(a => a.DayOfWeek == nMyHeroDayOfWeek) < 0)
        {
            // 요일 아님
            return false;
        }
        else
        {
            int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

            for (int i = 0; i < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Count; i++)
            {
                if (CsGameData.Instance.MyHeroInfo.RewardedLimitationGiftScheduleIdList.FindIndex(a => a == CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i].ScheduleId) < 0)
                {
                    if (CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i].StartTime <= nSecond && 
                        nSecond < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i].EndTime)
                    {

                        return true;
                    }
                }
            }

            return false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNoticeWeekendReward()
    {
        if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Monday)
        {
            if ((CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 != -1 ||
                 CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 != -1 ||
                 CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 != -1) &&
                !CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Rewarded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Friday)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Saturday)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Sunday)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}