using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnRankingIndividualType
{
    ServerBattlePowerRanking = 1,
    NationBattlePowerRanking = 2,
    ServerLevelRanking = 3,
    ServerCreatureCardRanking = 4,
    ServerIllustratedBookRanking = 5,
    ServerPresentPopularityPointRanking = 6,
    NationWeeklyPresentPopularityPointRanking = 7,
    ServerPresentContributionPointRanking = 8,
    NationWeeklyPresentContributionPointRanking = 9,
}

public enum EnRankingNationType
{
    NationPowerRanking = 1, 
    NationExploitPointRanking = 2,
}

public enum EnRankingGuildType
{
    ServerGuildRanking = 1,
    NationGuildRanking = 2,
}

public class CsPopupRanking : CsPopupSub
{
    [SerializeField] GameObject m_goRankingItem;
    [SerializeField] GameObject m_goRankingRewardItem;

    Transform m_trPanelLeft;
    Transform m_trPanelRight;
    Transform m_trPanelMyRanking;
    Transform m_trPanelYesterday;
    Transform m_trPanelRanking;
    Transform m_trRankingList;
    Transform m_trRankingReward;
    Transform m_trImageNotice;

	GameObject m_goNew;

    Button m_buttonReward;
    Button m_buttonRewardCheck;

    // 전투력 랭킹
    CsRanking m_csRanking;
    List<CsRanking> m_listCsRanking = new List<CsRanking>();

    // 길드 랭킹
    CsGuildRanking m_csGuildRanking;
    List<CsGuildRanking> m_listCsGuildRanking = new List<CsGuildRanking>();

    // 카드 랭킹
    CsCreatureCardRanking m_csCreatureCardRanking;
    List<CsCreatureCardRanking> m_listCsCreatureCardRanking = new List<CsCreatureCardRanking>();

    // 도감 랭킹
    CsIllustratedBookRanking m_csIllustratedBookRanking;
    List<CsIllustratedBookRanking> m_listCsIllustratedBookRanking = new List<CsIllustratedBookRanking>();

    // 인기도 랭킹
    CsPresentPopularityPointRanking m_csPresentPopularityPointRanking = null;
    List<CsPresentPopularityPointRanking> m_listCsPresentPopularityPointRanking = null;

    // 공헌도 랭킹
    CsPresentContributionPointRanking m_csPresentContributionPointRanking = null;
    List<CsPresentContributionPointRanking> m_listCsPresentContributionPointRanking = null;

    EnRankingIndividualType m_enRankingIndividualType;
    EnRankingNationType m_enRankingNationType;
    EnRankingGuildType m_enRankingGuildType;

    int m_nSelectJob;

    bool m_bFirst = true;
    bool m_bFirstPopup = true;
    bool m_bIsLoad = false;

    int m_nLoadRankingItemCount = 0;
    int m_nStandardPosition = 0;
    int m_nRankingCount = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        // 랭킹 이벤트
        // 개인
        CsGameEventUIToUI.Instance.EventServerBattlePowerRanking += OnEventServerBattlePowerRanking;
        CsGameEventUIToUI.Instance.EventNationBattlePowerRanking += OnEventNationBattlePowerRanking;
        CsGameEventUIToUI.Instance.EventServerLevelRanking += OnEventServerLevelRanking;
        CsGameEventUIToUI.Instance.EventServerCreatureCardRanking += OnEventServerCreatureCardRanking;
        CsGameEventUIToUI.Instance.EventServerIllustratedBookRanking += OnEventServerIllustratedBookRanking;

        // 국가
        CsGameEventUIToUI.Instance.EventDailyServerNationPowerRankingUpdated += OnEventDailyServerNationPowerRankingUpdated;
        CsGameEventUIToUI.Instance.EventNationExploitPointRanking += OnEventNationExploitPointRanking;

        // 직업
        CsGameEventUIToUI.Instance.EventServerJobBattlePowerRanking += OnEventServerJobBattlePowerRanking;

        // 길드
        CsGuildManager.Instance.EventServerGuildRanking += OnEventServerGuildRanking;
        CsGuildManager.Instance.EventNationGuildRanking += OnEventNationGuildRanking;

        // 랭킹 보상 이벤트
        CsGameEventUIToUI.Instance.EventServerLevelRankingRewardReceive += OnEventServerLevelRankingRewardReceive;

        // 인기도
        CsPresentManager.Instance.EventServerPresentPopularityPointRanking += OnEventServerPresentPopularityPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRanking += OnEventNationWeeklyPresentPopularityPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRankingUpdated += OnEventNationWeeklyPresentPopularityPointRankingUpdated;
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRankingRewardReceive += OnEventNationWeeklyPresentPopularityPointRankingRewardReceive;

        // 공헌도
        CsPresentManager.Instance.EventServerPresentContributionPointRanking += OnEventServerPresentContributionPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRanking += OnEventNationWeeklyPresentContributionPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRankingUpdated += OnEventNationWeeklyPresentContributionPointRankingUpdated;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRankingRewardReceive += OnEventNationWeeklyPresentContributionPointRankingRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // Start
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        // Destroy
        // 개인
        CsGameEventUIToUI.Instance.EventServerBattlePowerRanking -= OnEventServerBattlePowerRanking;
        CsGameEventUIToUI.Instance.EventNationBattlePowerRanking -= OnEventNationBattlePowerRanking;
        CsGameEventUIToUI.Instance.EventServerLevelRanking -= OnEventServerLevelRanking;
        CsGameEventUIToUI.Instance.EventServerCreatureCardRanking -= OnEventServerCreatureCardRanking;
        CsGameEventUIToUI.Instance.EventServerIllustratedBookRanking -= OnEventServerIllustratedBookRanking;

        // 국가
        CsGameEventUIToUI.Instance.EventDailyServerNationPowerRankingUpdated -= OnEventDailyServerNationPowerRankingUpdated;
        CsGameEventUIToUI.Instance.EventNationExploitPointRanking -= OnEventNationExploitPointRanking;

        // 직업
        CsGameEventUIToUI.Instance.EventServerJobBattlePowerRanking -= OnEventServerJobBattlePowerRanking;

        // 길드
        CsGuildManager.Instance.EventServerGuildRanking -= OnEventServerGuildRanking;
        CsGuildManager.Instance.EventNationGuildRanking -= OnEventNationGuildRanking;

        // 랭킹 보상 이벤트
        CsGameEventUIToUI.Instance.EventServerLevelRankingRewardReceive -= OnEventServerLevelRankingRewardReceive;
        
        // 인기도
        CsPresentManager.Instance.EventServerPresentPopularityPointRanking -= OnEventServerPresentPopularityPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRanking -= OnEventNationWeeklyPresentPopularityPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRankingUpdated -= OnEventNationWeeklyPresentPopularityPointRankingUpdated;
        CsPresentManager.Instance.EventNationWeeklyPresentPopularityPointRankingRewardReceive -= OnEventNationWeeklyPresentPopularityPointRankingRewardReceive;

        // 공헌도
        CsPresentManager.Instance.EventServerPresentContributionPointRanking -= OnEventServerPresentContributionPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRanking -= OnEventNationWeeklyPresentContributionPointRanking;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRankingUpdated -= OnEventNationWeeklyPresentContributionPointRankingUpdated;
        CsPresentManager.Instance.EventNationWeeklyPresentContributionPointRankingRewardReceive -= OnEventNationWeeklyPresentContributionPointRankingRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        m_trRankingList.localPosition = Vector3.zero;
        m_nLoadRankingItemCount = 0;
        m_nStandardPosition = 0;

        int nCount = 0;

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            foreach (EnRankingIndividualType enumItem in System.Enum.GetValues(typeof(EnRankingIndividualType)))
            {
                nCount++;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
        {
            foreach (EnRankingNationType enumItem in System.Enum.GetValues(typeof(EnRankingNationType)))
            {
                nCount++;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
        {
            nCount = CsGameData.Instance.JobList.FindAll(a => a.ParentJobId == 0 || a.JobId == a.ParentJobId).Count;
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingGuild)
        {
            foreach (EnRankingGuildType enumItem in System.Enum.GetValues(typeof(EnRankingGuildType)))
            {
                nCount++;
            }
        }

        InitializePanelLeft(nCount);
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventServerBattlePowerRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
    {
        m_csRanking = csRanking;
        m_listCsRanking = listCsRanking;
        m_nRankingCount = m_listCsRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationBattlePowerRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
    {
        m_csRanking = csRanking;
        m_listCsRanking = listCsRanking;
        m_nRankingCount = m_listCsRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventServerLevelRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
    {
        m_csRanking = csRanking;
        m_listCsRanking = listCsRanking;
        m_nRankingCount = m_listCsRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    // 카드 랭킹
    //---------------------------------------------------------------------------------------------------
    void OnEventServerCreatureCardRanking(CsCreatureCardRanking csCreatureCardRanking, List<CsCreatureCardRanking> listCsCreatureCardRanking)
    {
        m_csCreatureCardRanking = csCreatureCardRanking;
        m_listCsCreatureCardRanking = listCsCreatureCardRanking;
        m_nRankingCount = m_listCsCreatureCardRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    // 도감 랭킹
    //---------------------------------------------------------------------------------------------------
    void OnEventServerIllustratedBookRanking(CsIllustratedBookRanking csIllustratedBookRanking, List<CsIllustratedBookRanking> listCsIllustratedBookRanking)
    {
        m_csIllustratedBookRanking = csIllustratedBookRanking;
        m_listCsIllustratedBookRanking = listCsIllustratedBookRanking;
        m_nRankingCount = m_listCsIllustratedBookRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    void OnEventDailyServerNationPowerRankingUpdated()
    {
        UpdateNationPowerRanking();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationExploitPointRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
    {
        m_csRanking = csRanking;
        m_listCsRanking = listCsRanking;
        m_nRankingCount = m_listCsRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventServerJobBattlePowerRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
    {
        m_csRanking = csRanking;
        m_listCsRanking = listCsRanking;
        m_nRankingCount = m_listCsRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventServerGuildRanking(CsGuildRanking csGuildRanking, List<CsGuildRanking> listCsGuildRanking)
    {
        m_csGuildRanking = csGuildRanking;
        m_listCsGuildRanking = listCsGuildRanking;
        m_nRankingCount = m_listCsGuildRanking.Count;

        UpdateImageGuildRankTop();
        UpdatePanelGuildRanking();
        CreateGuildRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationGuildRanking(CsGuildRanking csGuildRanking, List<CsGuildRanking> listCsGuildRanking)
    {
        m_csGuildRanking = csGuildRanking;
        m_listCsGuildRanking = listCsGuildRanking;
        m_nRankingCount = m_listCsGuildRanking.Count;

        UpdateImageGuildRankTop();
        UpdatePanelGuildRanking();
        CreateGuildRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventServerLevelRankingRewardReceive()
    {
        UpdateButtonReward();

		if (m_goNew != null)
			m_goNew.SetActive(false);
    }

    // 인기
    //---------------------------------------------------------------------------------------------------
    void OnEventServerPresentPopularityPointRanking(CsPresentPopularityPointRanking csPresentPopularityPointRanking, List<CsPresentPopularityPointRanking> listCsPresentPopularityPointRanking)
    {
        m_csPresentPopularityPointRanking = csPresentPopularityPointRanking;

        if (m_listCsPresentPopularityPointRanking != null)
        {
            m_listCsPresentPopularityPointRanking.Clear();
            m_listCsPresentPopularityPointRanking = null;
        }

        m_listCsPresentPopularityPointRanking = new List<CsPresentPopularityPointRanking>(listCsPresentPopularityPointRanking);
        m_nRankingCount = m_listCsPresentPopularityPointRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentPopularityPointRanking(CsPresentPopularityPointRanking csPresentPopularityPointRanking, List<CsPresentPopularityPointRanking> listCsPresentPopularityPointRanking)
    {
        m_csPresentPopularityPointRanking = csPresentPopularityPointRanking;

        if (m_listCsPresentPopularityPointRanking != null)
        {
            m_listCsPresentPopularityPointRanking.Clear();
            m_listCsPresentPopularityPointRanking = null;
        }

        m_listCsPresentPopularityPointRanking = new List<CsPresentPopularityPointRanking>(listCsPresentPopularityPointRanking);
        m_nRankingCount = m_listCsPresentPopularityPointRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    // 랭킹 갱신
    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentPopularityPointRankingUpdated()
    {
        CsPresentManager.Instance.SendNationWeeklyPresentPopularityPointRanking();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentPopularityPointRankingRewardReceive()
    {
        UpdateButtonReward();
    }

    // 공헌도
    //---------------------------------------------------------------------------------------------------
    void OnEventServerPresentContributionPointRanking(CsPresentContributionPointRanking csPresentContributionPointRanking, List<CsPresentContributionPointRanking> listCsPresentContributionPointRanking)
    {
        m_csPresentContributionPointRanking = csPresentContributionPointRanking;

        if (m_listCsPresentContributionPointRanking != null)
        {
            m_listCsPresentContributionPointRanking.Clear();
            m_listCsPresentContributionPointRanking = null;
        }

        m_listCsPresentContributionPointRanking = new List<CsPresentContributionPointRanking>(listCsPresentContributionPointRanking);
        m_nRankingCount = m_listCsPresentContributionPointRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentContributionPointRanking(CsPresentContributionPointRanking csPresentContributionPointRanking, List<CsPresentContributionPointRanking> listCsPresentContributionPointRanking)
    {
        m_csPresentContributionPointRanking = csPresentContributionPointRanking;

        if (m_listCsPresentContributionPointRanking != null)
        {
            m_listCsPresentContributionPointRanking.Clear();
            m_listCsPresentContributionPointRanking = null;
        }

        m_listCsPresentContributionPointRanking = new List<CsPresentContributionPointRanking>(listCsPresentContributionPointRanking);
        m_nRankingCount = m_listCsPresentContributionPointRanking.Count;

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }

    // 랭킹 갱신
    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentContributionPointRankingUpdated()
    {
        CsPresentManager.Instance.SendNationWeeklyPresentContributionPointRanking();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWeeklyPresentContributionPointRankingRewardReceive()
    {
        UpdateButtonReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleRankingContent(bool bIsOn, int nIndex)
    {
        if (bIsOn)
        {
            if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
            {
                m_enRankingIndividualType = (EnRankingIndividualType)nIndex;
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
            {
                m_enRankingNationType = (EnRankingNationType)nIndex;
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
            {
                m_nSelectJob = nIndex;
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingGuild)
            {
                m_enRankingGuildType = (EnRankingGuildType)nIndex;
            }

            m_trRankingList.localPosition = Vector3.zero;
            m_nLoadRankingItemCount = 0;
            m_nStandardPosition = 0;
            UpdateRankingList();

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonRewardReceive()
    {
        switch (m_enRankingIndividualType)
        {
            case EnRankingIndividualType.ServerLevelRanking:
                CsCommandEventManager.Instance.SendServerLevelRankingRewardReceive();
                break;

            case EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking:
                CsPresentManager.Instance.SendNationWeeklyPresentPopularityPointRankingRewardReceive();
                break;

            case EnRankingIndividualType.NationWeeklyPresentContributionPointRanking:
                CsPresentManager.Instance.SendNationWeeklyPresentContributionPointRankingRewardReceive();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonRewardCheck()
    {
        if (m_bFirstPopup)
        {
            InitializePopup();
            m_bFirstPopup = false;
        }

        UpdatePopup();
        m_trRankingReward.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        m_trRankingReward.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedRankingScrollbar(Scrollbar scrollbar)
    {
        if (!m_bIsLoad)
        {
            m_bIsLoad = true;

            if (m_nLoadRankingItemCount < m_nRankingCount)
            {
                int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, m_nRankingCount, (1 - scrollbar.value)));

                if (nUpdateLine > m_nStandardPosition)
                {
                    int nStartCount = m_nLoadRankingItemCount;
                    int nEndCount = nUpdateLine + 5;

                    if (nEndCount >= m_nRankingCount)
                    {
                        nEndCount = m_nRankingCount;
                    }

                    for (int i = nStartCount; i < nEndCount; i++)
                    {
                        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
                        {
                            if (m_enRankingIndividualType == EnRankingIndividualType.ServerCreatureCardRanking)
                            {
                                CreateRankingItem(m_listCsCreatureCardRanking[i]);
                            }
                            else if (m_enRankingIndividualType == EnRankingIndividualType.ServerIllustratedBookRanking)
                            {
                                CreateRankingItem(m_listCsIllustratedBookRanking[i]);
                            }
                            else
                            {
                                CreateRankingItem(m_listCsRanking[i]);
                            }
                        }
                        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
                        {
                            CreateRankingItem(m_listCsRanking[i]);
                        }
                        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
                        {
                            CreateRankingItem(m_listCsRanking[i]);
                        }
                        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingGuild)
                        {
                            CreateRankingItem(m_listCsGuildRanking[i]);
                        }
                    }

                    m_nStandardPosition = nUpdateLine;
                }
            }

            m_bIsLoad = false;
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPanelLeft = transform.Find("Scroll View/Viewport/Content");

        int nCount = 0;

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            foreach (EnRankingIndividualType enumItem in System.Enum.GetValues(typeof(EnRankingIndividualType)))
            {
                nCount++;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
        {
            foreach (EnRankingNationType enumItem in System.Enum.GetValues(typeof(EnRankingNationType)))
            {
                nCount++;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
        {
            nCount = CsGameData.Instance.JobList.Count;
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingGuild)
        {
            foreach (EnRankingGuildType enumItem in System.Enum.GetValues(typeof(EnRankingGuildType)))
            {
                nCount++;
            }
        }

        InitializePanelLeft(nCount);

        m_trPanelRight = transform.Find("PanelRight");

        Transform trImageRankTop = m_trPanelRight.Find("ImageRankTop");

        m_trPanelMyRanking = trImageRankTop.Find("PanelMyRanking");
        m_trPanelYesterday = trImageRankTop.Find("PanelYesterday");

        UpdateImageRankTop();

        // 보상 버튼
        m_buttonReward = trImageRankTop.Find("ButtonReward").GetComponent<Button>();
        m_buttonReward.onClick.RemoveAllListeners();
        m_buttonReward.onClick.AddListener(() => OnClickButtonRewardReceive());
        m_buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonRewardCheck = trImageRankTop.Find("ButtonRewardCheck").GetComponent<Button>();
        m_buttonRewardCheck.onClick.RemoveAllListeners();
        m_buttonRewardCheck.onClick.AddListener(() => OnClickButtonRewardCheck());
        m_buttonRewardCheck.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonRewardCheck = m_buttonRewardCheck.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonRewardCheck);
        textButtonRewardCheck.text = CsConfiguration.Instance.GetString("A16_NAME_00001");

        UpdateButtonReward();

        m_trPanelRanking = m_trPanelRight.Find("PanelRanking");

        UpdatePanelRanking();

        m_trRankingReward = transform.Find("RankingReward");

        m_trRankingList = m_trPanelRanking.Find("Scroll View/Viewport/Content");

        CreateRankingItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePanelLeft(int nCount)
    {
        m_enRankingIndividualType = EnRankingIndividualType.ServerBattlePowerRanking;
        m_enRankingNationType = EnRankingNationType.NationPowerRanking;
        m_enRankingGuildType = EnRankingGuildType.ServerGuildRanking;

        m_nSelectJob = (int)EnJob.Gaia;

        Transform trToggleRanking = null;

        for (int i = 0; i < m_trPanelLeft.childCount; i++)
        {
            trToggleRanking = m_trPanelLeft.GetChild(i);
            trToggleRanking.gameObject.SetActive(false);

            Toggle toggleRanking = trToggleRanking.GetComponent<Toggle>();
            toggleRanking.onValueChanged.RemoveAllListeners();

            if (toggleRanking.isOn)
            {
                toggleRanking.isOn = false;
            }
        }

        GameObject goToggleRanking = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupRanking/ToggleRanking");

        for (int i = 0; i < nCount; i++)
        {
            int nIndex = 0;

            trToggleRanking = m_trPanelLeft.Find("ToggleRanking" + i);

            if (trToggleRanking == null)
            {
                trToggleRanking = Instantiate(goToggleRanking, m_trPanelLeft).transform;
                trToggleRanking.name = "ToggleRanking" + i;
            }
            else
            {
                trToggleRanking.gameObject.SetActive(true);
            }

            Toggle toggleRanking = trToggleRanking.GetComponent<Toggle>();

            Text textToggleRanking = toggleRanking.transform.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textToggleRanking);

            if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
            {
                nIndex = i + 1;

                switch ((EnRankingIndividualType)nIndex)
                {
                    case EnRankingIndividualType.ServerBattlePowerRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00009");
                        break;

                    case EnRankingIndividualType.NationBattlePowerRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00010");
                        break;

                    case EnRankingIndividualType.ServerLevelRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00011");

						m_goNew = trToggleRanking.Find("Image").gameObject;
						Debug.Log(CsGameData.Instance.MyHeroInfo.DailyServerLevelRakingNo + ", " + CsGameData.Instance.MyHeroInfo.RewardedDailyServerLevelRankingNo);
						if (CsGameData.Instance.MyHeroInfo.DailyServerLevelRakingNo == CsGameData.Instance.MyHeroInfo.RewardedDailyServerLevelRankingNo)
							m_goNew.SetActive(false);
						else
							m_goNew.SetActive(true);

                        break;

                    case EnRankingIndividualType.ServerCreatureCardRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00021");
                        break;

                    case EnRankingIndividualType.ServerIllustratedBookRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00022");
                        break;

                    case EnRankingIndividualType.ServerPresentPopularityPointRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A108_TXT_06003");
                        break;

                    case EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A108_TXT_06004");
                        break;

                    case EnRankingIndividualType.ServerPresentContributionPointRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A108_TXT_06005");
                        break;

                    case EnRankingIndividualType.NationWeeklyPresentContributionPointRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A108_TXT_06006");
                        break;
                }
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
            {
                nIndex = i + 1;

                switch ((EnRankingNationType)nIndex)
                {
                    case EnRankingNationType.NationPowerRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A108_TXT_06011");
                        break;

                    case EnRankingNationType.NationExploitPointRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00013");
                        break;
                }
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
            {
                nIndex = CsGameData.Instance.JobList[i].JobId;
                textToggleRanking.text = CsGameData.Instance.JobList[i].Name;

				if (m_goNew != null)
					m_goNew.SetActive(false);
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingGuild)
            {
                nIndex = i + 1;
                switch ((EnRankingGuildType)nIndex)
                {
                    case EnRankingGuildType.ServerGuildRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00019");
                        break;
                    case EnRankingGuildType.NationGuildRanking:
                        textToggleRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00020");
                        break;
                }
            }

            if (i == 0)
            {
                UpdateRankingList();
                toggleRanking.isOn = true;
            }

            toggleRanking.onValueChanged.RemoveAllListeners();
            toggleRanking.onValueChanged.AddListener((ison) => OnValueChangedToggleRankingContent(ison, nIndex));
            toggleRanking.group = m_trPanelLeft.GetComponent<ToggleGroup>();

            toggleRanking.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopup()
    {
        Transform trImageBackground = m_trRankingReward.Find("ImageBackGround");
        Transform trPanelTop = trImageBackground.Find("PanelTop");

        Button buttonClose = trPanelTop.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickPopupClose());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textPanelName = trPanelTop.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPanelName);
        textPanelName.text = CsConfiguration.Instance.GetString("A16_NAME_00001");
        
        UpdatePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopup()
    {
        Transform trImageBackground = m_trRankingReward.Find("ImageBackGround");
        Transform trContentRankingReward = trImageBackground.Find("RankingRewardList/Viewport/Content");

        Transform trImageRankingRewardItem = null;

        for (int i = 0; i < trContentRankingReward.childCount; i++)
        {
            trContentRankingReward.GetChild(i).gameObject.SetActive(false);
        }

        switch (m_enRankingIndividualType)
        {
            case EnRankingIndividualType.ServerLevelRanking:

                for (int i = 0; i < CsGameData.Instance.LevelRankingRewardList.Count; i++)
                {
                    CsLevelRankingReward csLevelRankingReward = CsGameData.Instance.LevelRankingRewardList[i];

                    trImageRankingRewardItem = trContentRankingReward.Find("ImageRankingRewardItem" + i);

                    if (trImageRankingRewardItem == null)
                    {
                        trImageRankingRewardItem = Instantiate(m_goRankingRewardItem, trContentRankingReward).transform;
                        trImageRankingRewardItem.name = "ImageRankingRewardItem" + i;
                    }
                    else
                    {
                        trImageRankingRewardItem.gameObject.SetActive(true);
                    }

                    Text textRanking = trImageRankingRewardItem.Find("TextRanking").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRanking);

                    if (csLevelRankingReward.HighRanking == csLevelRankingReward.LowRanking)
                    {
                        textRanking.text = string.Format(CsConfiguration.Instance.GetString("INPUT_RANK"), csLevelRankingReward.LowRanking);
                    }
                    else
                    {
                        textRanking.text = string.Format(CsConfiguration.Instance.GetString("INPUT_RANK_RANGE"), csLevelRankingReward.HighRanking, csLevelRankingReward.LowRanking);
                    }

                    Transform trItemSlotList = trImageRankingRewardItem.Find("ItemSlotList");
                    Transform trItemSlot = null;

                    CsItemReward csItemReward = csLevelRankingReward.ItemReward;

                    for (int j = 0; j < trItemSlotList.childCount; j++)
                    {
                        trItemSlot = trItemSlotList.GetChild(j);

                        if (j == 0)
                        {
                            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                            trItemSlot.gameObject.SetActive(true);
                        }
                        else
                        {
                            trItemSlot.gameObject.SetActive(false);
                        }
                    }
                }

                break;

            case EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking:

                for (int i = 0; i < CsGameData.Instance.WeeklyPresentPopularityPointRankingRewardGroupList.Count; i++)
                {
                    CsWeeklyPresentPopularityPointRankingRewardGroup csWeeklyPresentPopularityPointRankingRewardGroup = CsGameData.Instance.WeeklyPresentPopularityPointRankingRewardGroupList[i];

                    trImageRankingRewardItem = trContentRankingReward.Find("ImageRankingRewardItem" + i);

                    if (trImageRankingRewardItem == null)
                    {
                        trImageRankingRewardItem = Instantiate(m_goRankingRewardItem, trContentRankingReward).transform;
                        trImageRankingRewardItem.name = "ImageRankingRewardItem" + i;
                    }
                    else
                    {
                        trImageRankingRewardItem.gameObject.SetActive(true);
                    }

                    Text textRanking = trImageRankingRewardItem.Find("TextRanking").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRanking);

                    if (csWeeklyPresentPopularityPointRankingRewardGroup.HighRanking == csWeeklyPresentPopularityPointRankingRewardGroup.LowRanking)
                    {
                        textRanking.text = string.Format(CsConfiguration.Instance.GetString("INPUT_RANK"), csWeeklyPresentPopularityPointRankingRewardGroup.LowRanking);
                    }
                    else
                    {
                        textRanking.text = string.Format(CsConfiguration.Instance.GetString("INPUT_RANK_RANGE"), csWeeklyPresentPopularityPointRankingRewardGroup.HighRanking, csWeeklyPresentPopularityPointRankingRewardGroup.LowRanking);
                    }

                    Transform trItemSlotList = trImageRankingRewardItem.Find("ItemSlotList");
                    Transform trItemSlot = null;

                    for (int j = 0; j < trItemSlotList.childCount; j++)
                    {
                        trItemSlot = trItemSlotList.GetChild(j);

                        if (j < csWeeklyPresentPopularityPointRankingRewardGroup.WeeklyPresentPopularityPointRankingRewardList.Count)
                        {
                            CsWeeklyPresentPopularityPointRankingReward csWeeklyPresentPopularityPointRankingReward = csWeeklyPresentPopularityPointRankingRewardGroup.WeeklyPresentPopularityPointRankingRewardList[j];

                            CsItemReward csItemReward = csWeeklyPresentPopularityPointRankingReward.ItemReward;
                            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                            trItemSlot.gameObject.SetActive(true);
                        }
                        else
                        {
                            trItemSlot.gameObject.SetActive(false);
                        }
                    }
                }

                break;

            case EnRankingIndividualType.NationWeeklyPresentContributionPointRanking:

                for (int i = 0; i < CsGameData.Instance.WeeklyPresentContributionPointRankingRewardGroupList.Count; i++)
                {
                    CsWeeklyPresentContributionPointRankingRewardGroup csWeeklyPresentContributionPointRankingRewardGroup = CsGameData.Instance.WeeklyPresentContributionPointRankingRewardGroupList[i];

                    trImageRankingRewardItem = trContentRankingReward.Find("ImageRankingRewardItem" + i);

                    if (trImageRankingRewardItem == null)
                    {
                        trImageRankingRewardItem = Instantiate(m_goRankingRewardItem, trContentRankingReward).transform;
                        trImageRankingRewardItem.name = "ImageRankingRewardItem" + i;
                    }
                    else
                    {
                        trImageRankingRewardItem.gameObject.SetActive(true);
                    }

                    Text textRanking = trImageRankingRewardItem.Find("TextRanking").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRanking);

                    if (csWeeklyPresentContributionPointRankingRewardGroup.HighRanking == csWeeklyPresentContributionPointRankingRewardGroup.LowRanking)
                    {
                        textRanking.text = string.Format(CsConfiguration.Instance.GetString("INPUT_RANK"), csWeeklyPresentContributionPointRankingRewardGroup.LowRanking);
                    }
                    else
                    {
                        textRanking.text = string.Format(CsConfiguration.Instance.GetString("INPUT_RANK_RANGE"), csWeeklyPresentContributionPointRankingRewardGroup.HighRanking, csWeeklyPresentContributionPointRankingRewardGroup.LowRanking);
                    }

                    Transform trItemSlotList = trImageRankingRewardItem.Find("ItemSlotList");
                    Transform trItemSlot = null;

                    for (int j = 0; j < trItemSlotList.childCount; j++)
                    {
                        trItemSlot = trItemSlotList.GetChild(j);

                        if (j < csWeeklyPresentContributionPointRankingRewardGroup.WeeklyPresentContributionPointRankingRewardList.Count)
                        {
                            CsWeeklyPresentContributionPointRankingReward csWeeklyPresentContributionPointRankingReward = csWeeklyPresentContributionPointRankingRewardGroup.WeeklyPresentContributionPointRankingRewardList[j];

                            CsItemReward csItemReward = csWeeklyPresentContributionPointRankingReward.ItemReward;
                            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                            trItemSlot.gameObject.SetActive(true);
                        }
                        else
                        {
                            trItemSlot.gameObject.SetActive(false);
                        }
                    }
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItemList()
    {
        // 랭킹 리스트 초기화
        for (int i = 0; i < m_trRankingList.childCount; i++)
        {
            m_trRankingList.GetChild(i).gameObject.SetActive(false);
        }

        int nItemSize = 88;
        int nBaseLoadCount = 10;

        if (m_nRankingCount < nBaseLoadCount)
        {
            nBaseLoadCount = m_nRankingCount;
        }

        RectTransform rectTransform = m_trRankingList.GetComponent<RectTransform>();

        if (m_nRankingCount < CsGameConfig.Instance.RankingDisplayMaxCount)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * m_nRankingCount);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * CsGameConfig.Instance.RankingDisplayMaxCount);
        }

        for (int i = 0; i < nBaseLoadCount; i++)
        {
            if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
            {
                if (m_enRankingIndividualType == EnRankingIndividualType.ServerCreatureCardRanking)
                {
                    CreateRankingItem(m_listCsCreatureCardRanking[i]);
                }
                else if (m_enRankingIndividualType == EnRankingIndividualType.ServerIllustratedBookRanking)
                {
                    CreateRankingItem(m_listCsIllustratedBookRanking[i]);
                }
                else if (m_enRankingIndividualType == EnRankingIndividualType.ServerPresentPopularityPointRanking || m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking)
                {
                    CreateRankingItem(m_listCsPresentPopularityPointRanking[i]);
                }
                else if (m_enRankingIndividualType == EnRankingIndividualType.ServerPresentContributionPointRanking || m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentContributionPointRanking)
                {
                    CreateRankingItem(m_listCsPresentContributionPointRanking[i]);
                }
                else
                {
                    CreateRankingItem(m_listCsRanking[i]);
                }
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation && m_enRankingNationType == EnRankingNationType.NationPowerRanking)
            {
                if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList == null)
                {
                    CreateRankingItem(CsGameData.Instance.NationList[i]);
                }
                else
                {
                    CreateRankingItem(CsGameData.Instance.MyHeroInfo.NationPowerRankingList[i]);
                }
            }
            else
            {
                CreateRankingItem(m_listCsRanking[i]);
            }
        }

        Scrollbar scrollbar = m_trPanelRanking.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.RemoveAllListeners();
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedRankingScrollbar(scrollbar));
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsRanking csRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csRanking.Ranking.ToString("#,##0");

        if (csRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsRanking csRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);
            textHeroName.text = csRanking.Name;

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
            {
                switch (m_enRankingIndividualType)
                {
                    case EnRankingIndividualType.ServerBattlePowerRanking:
                        imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csRanking.Nation.NationId);
                        textTag.text = csRanking.Nation.Name;
                        textBattlePower.text = csRanking.BattlePower.ToString("#,##0");
                        break;

                    case EnRankingIndividualType.NationBattlePowerRanking:
                        imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csRanking.Job.JobId);
                        textTag.text = csRanking.Job.Name;
                        textBattlePower.text = csRanking.BattlePower.ToString("#,##0");
                        break;

                    case EnRankingIndividualType.ServerLevelRanking:
                        imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csRanking.Nation.NationId);
                        textTag.text = csRanking.Nation.Name;
                        textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csRanking.Level);
                        break;
                }
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
            {
                imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csRanking.Job.JobId);
                textTag.text = csRanking.Job.Name;
                textBattlePower.text = csRanking.ExploitPoint.ToString("#,##0");
            }
            else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
            {
                imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csRanking.Nation.NationId);
                textTag.text = csRanking.Nation.Name;
                textBattlePower.text = csRanking.BattlePower.ToString("#,##0");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsCreatureCardRanking csCreatureCardRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csCreatureCardRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csCreatureCardRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csCreatureCardRanking.Ranking.ToString("#,##0");

        if (csCreatureCardRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csCreatureCardRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csCreatureCardRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csCreatureCardRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsCreatureCardRanking csCreatureCardRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csCreatureCardRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);
            textHeroName.text = csCreatureCardRanking.Name;

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csCreatureCardRanking.Nation.NationId);
            textTag.text = csCreatureCardRanking.Nation.Name;
            textBattlePower.text = csCreatureCardRanking.CollectionFamePoint.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsIllustratedBookRanking csIllustratedBookRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csIllustratedBookRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csIllustratedBookRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csIllustratedBookRanking.Ranking.ToString("#,##0");

        if (csIllustratedBookRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csIllustratedBookRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csIllustratedBookRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csIllustratedBookRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsIllustratedBookRanking csIllustratedBookRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csIllustratedBookRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);
            textHeroName.text = csIllustratedBookRanking.Name;

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csIllustratedBookRanking.Nation.NationId);
            textTag.text = csIllustratedBookRanking.Nation.Name;
            textBattlePower.text = csIllustratedBookRanking.ExplorationPoint.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsPresentPopularityPointRanking csPresentPopularityPointRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csPresentPopularityPointRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csPresentPopularityPointRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csPresentPopularityPointRanking.Ranking.ToString("#,##0");

        if (csPresentPopularityPointRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csPresentPopularityPointRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csPresentPopularityPointRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csPresentPopularityPointRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsPresentPopularityPointRanking csPresentPopularityPointRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csPresentPopularityPointRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);
            textHeroName.text = csPresentPopularityPointRanking.Name;

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csPresentPopularityPointRanking.Nation.NationId);
            textTag.text = csPresentPopularityPointRanking.Nation.Name;
            textBattlePower.text = csPresentPopularityPointRanking.PopularityPoint.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsPresentContributionPointRanking csPresentContributionPointRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csPresentContributionPointRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csPresentContributionPointRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csPresentContributionPointRanking.Ranking.ToString("#,##0");

        if (csPresentContributionPointRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csPresentContributionPointRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csPresentContributionPointRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csPresentContributionPointRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsPresentContributionPointRanking csPresentContributionPointRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csPresentContributionPointRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);
            textHeroName.text = csPresentContributionPointRanking.Name;

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csPresentContributionPointRanking.Nation.NationId);
            textTag.text = csPresentContributionPointRanking.Nation.Name;
            textBattlePower.text = csPresentContributionPointRanking.ContributionPoint.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsNationPowerRanking csNationPowerRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csNationPowerRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csNationPowerRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csNationPowerRanking.Ranking.ToString("#,##0");

        if (csNationPowerRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csNationPowerRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csNationPowerRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csNationPowerRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsNationPowerRanking csNationPowerRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csNationPowerRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            // 왕 이름
            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);

            CsNationInstance csNationInstance = CsGameData.Instance.MyHeroInfo.GetNationInstance(csNationPowerRanking.NationId);

            if (csNationInstance == null)
            {
                textHeroName.text = "";   
            }
            else
            {
                CsNationNoblesseInstance csNationNoblesseInstance = csNationInstance.NationNoblesseInstanceList.Find(a => a.NoblesseId == 1);
                
                if (csNationNoblesseInstance == null)
                {
                    textHeroName.text = "";   
                }
                else
                {
                    if (csNationNoblesseInstance.HeroId == System.Guid.Empty)
                    {
                        textHeroName.text = CsConfiguration.Instance.GetString("A61_TXT_00016");
                    }
                    else
                    {
                        textHeroName.text = csNationNoblesseInstance.HeroName;
                    }
                }
            }

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csNationPowerRanking.NationId);
            
            CsNation csNation = CsGameData.Instance.GetNation(csNationPowerRanking.NationId);

            if (csNation == null)
            {
                textTag.text = "";
            }
            else
            {
                textTag.text = csNation.Name;
            }

            textBattlePower.text = csNationPowerRanking.NationPower.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsNation csNation)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csNation.NationId);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csNation.NationId;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csNation.NationId.ToString("#,##0");

        if (csNation.NationId == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csNation.NationId == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csNation.NationId == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csNation);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsNation csNation)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csNation.NationId);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            imageTag.gameObject.SetActive(true);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHeroName);
            textHeroName.text = "";

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);

            imageTag.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_Nation" + csNation.NationId);
            textTag.text = csNation.Name;

            textBattlePower.text = CsGameConfig.Instance.NationBasePower.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingList()
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            switch (m_enRankingIndividualType)
            {
                case EnRankingIndividualType.ServerBattlePowerRanking:
                    CsCommandEventManager.Instance.SendServerBattlePowerRanking();
                    break;

                case EnRankingIndividualType.NationBattlePowerRanking:
                    CsCommandEventManager.Instance.SendNationBattlePowerRanking();
                    break;

                case EnRankingIndividualType.ServerLevelRanking:
                    CsCommandEventManager.Instance.SendServerLevelRanking();
                    break;

                case EnRankingIndividualType.ServerCreatureCardRanking:
                    CsCommandEventManager.Instance.SendServerCreatureCardRanking();
                    break;

                case EnRankingIndividualType.ServerIllustratedBookRanking:
                    CsCommandEventManager.Instance.SendServerIllustratedBookRanking();
                    break;

                case EnRankingIndividualType.ServerPresentPopularityPointRanking:
                    CsPresentManager.Instance.SendServerPresentPopularityPointRanking();
                    break;

                case EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking:
                    CsPresentManager.Instance.SendNationWeeklyPresentPopularityPointRanking();
                    break;

                case EnRankingIndividualType.ServerPresentContributionPointRanking:
                    CsPresentManager.Instance.SendServerPresentContributionPointRanking();
                    break;

                case EnRankingIndividualType.NationWeeklyPresentContributionPointRanking:
                    CsPresentManager.Instance.SendNationWeeklyPresentContributionPointRanking();
                    break;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
        {
            switch (m_enRankingNationType)
            {
                case EnRankingNationType.NationPowerRanking:
                    UpdateNationPowerRanking();
                    break;

                case EnRankingNationType.NationExploitPointRanking:
                    CsCommandEventManager.Instance.SendNationExploitPointRanking();
                    break;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
        {
            CsCommandEventManager.Instance.SendServerJobBattlePowerRanking(m_nSelectJob);
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingGuild)
        {
            switch (m_enRankingGuildType)
            {
                case EnRankingGuildType.ServerGuildRanking:
                    CsGuildManager.Instance.SendServerGuildRanking();
                    break;

                case EnRankingGuildType.NationGuildRanking:
                    CsGuildManager.Instance.SendNationGuildRanking();
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageRankTop()
    {
        Text textName = m_trPanelMyRanking.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation && m_enRankingNationType == EnRankingNationType.NationPowerRanking)
        {
            textName.text = CsConfiguration.Instance.GetString("A16_TXT_00026");
        }
        else
        {
            textName.text = CsConfiguration.Instance.GetString("A16_TXT_00001");
        }

        Text textMyRank = m_trPanelMyRanking.Find("TextMyRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyRank);

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            // 랭킹 순위
            if (m_enRankingIndividualType == EnRankingIndividualType.ServerCreatureCardRanking)
            {
                if (m_csCreatureCardRanking == null)
                {
                    textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textMyRank.text = m_csCreatureCardRanking.Ranking.ToString("#,##0");
                }
            }
            else if (m_enRankingIndividualType == EnRankingIndividualType.ServerIllustratedBookRanking)
            {
                if (m_csIllustratedBookRanking == null)
                {
                    textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textMyRank.text = m_csIllustratedBookRanking.Ranking.ToString("#,##0");
                }
            }
            else if (m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking || m_enRankingIndividualType == EnRankingIndividualType.ServerPresentPopularityPointRanking)
            {
                if (m_csPresentPopularityPointRanking == null)
                {
                    textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textMyRank.text = m_csPresentPopularityPointRanking.Ranking.ToString("#,##0");
                }
            }
            else if (m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentContributionPointRanking || m_enRankingIndividualType == EnRankingIndividualType.ServerPresentContributionPointRanking)
            {
                if (m_csPresentContributionPointRanking == null)
                {
                    textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textMyRank.text = m_csPresentContributionPointRanking.Ranking.ToString("#,##0");
                }
            }
            else
            {
                // 내 랭킹 없으면 '미진입' 있으면 '랭킹 순위'
                if (m_csRanking == null)
                {
                    textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textMyRank.text = m_csRanking.Ranking.ToString("#,##0");
                }
            }

            Text textPrevRanking = m_trPanelYesterday.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPrevRanking);
            textPrevRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00008");

            // 작일 랭킹 작업
            Text textPrevMyRank = m_trPanelYesterday.Find("TextMyRank").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPrevMyRank);

            // 작일 랭킹
            if (m_enRankingIndividualType == EnRankingIndividualType.ServerLevelRanking)
            {
                if (CsGameData.Instance.MyHeroInfo.DailyServerLevelRanking == 0)
                {
                    textPrevMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textPrevMyRank.text = CsGameData.Instance.MyHeroInfo.DailyServerLevelRanking.ToString("#,##0");
                }

                m_trPanelYesterday.gameObject.SetActive(true);
            }
            else if (m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking)
            {
                if (CsPresentManager.Instance.NationWeeklyPresentPopularityPointRanking == 0)
                {
                    textPrevMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textPrevMyRank.text = CsPresentManager.Instance.NationWeeklyPresentPopularityPointRanking.ToString("#,##0");
                }

                m_trPanelYesterday.gameObject.SetActive(true);
            }
            else if (m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentContributionPointRanking)
            {
                if (CsPresentManager.Instance.NationWeeklyPresentContributionPointRanking == 0)
                {
                    textPrevMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
                }
                else
                {
                    textPrevMyRank.text = CsPresentManager.Instance.NationWeeklyPresentContributionPointRanking.ToString("#,##0");
                }

                m_trPanelYesterday.gameObject.SetActive(true);
            }
            else
            {
                m_trPanelYesterday.gameObject.SetActive(false);
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation && m_enRankingNationType == EnRankingNationType.NationPowerRanking)
        {
            if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList == null)
            {
                textMyRank.text = CsGameData.Instance.MyHeroInfo.Nation.NationId.ToString("#,##0");
            }
            else
            {
                textMyRank.text = CsGameData.Instance.MyHeroInfo.Nation.Name;
            }

            m_trPanelYesterday.gameObject.SetActive(false);
        }
        else
        {
            // 내 랭킹 없으면 '미진입' 있으면 '랭킹 순위'
            if (m_csRanking == null)
            {
                textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
            }
            else
            {
                textMyRank.text = m_csRanking.Ranking.ToString("#,##0");
            }

            m_trPanelYesterday.gameObject.SetActive(false);
        }
    }


    //---------------------------------------------------------------------------------------------------
    void UpdatePanelRanking()
    {
        Text textRanking = m_trPanelRanking.Find("TextRanking").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRanking);
        textRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00003");

        Text textNation = m_trPanelRanking.Find("TextNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNation);

        Text textHeroName = m_trPanelRanking.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textHeroName);

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation && m_enRankingNationType == EnRankingNationType.NationPowerRanking)
        {
            textHeroName.text = CsConfiguration.Instance.GetString("A16_TXT_00028");
        }
        else
        {
            textHeroName.text = CsConfiguration.Instance.GetString("A16_TXT_00005");
        }

        Text textPoint = m_trPanelRanking.Find("TextPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPoint);

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            if (m_enRankingIndividualType == EnRankingIndividualType.NationBattlePowerRanking)
            {
                textNation.text = CsConfiguration.Instance.GetString("A16_TXT_00007");
            }
            else
            {
                textNation.text = CsConfiguration.Instance.GetString("A16_TXT_00004");
            }

            switch (m_enRankingIndividualType)
            {
                //전투력 클텍 추가
                case EnRankingIndividualType.ServerBattlePowerRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00012");
                    break;

                case EnRankingIndividualType.NationBattlePowerRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00012");
                    break;

                case EnRankingIndividualType.ServerLevelRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00006");
                    break;

                case EnRankingIndividualType.ServerCreatureCardRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00023");
                    break;

                case EnRankingIndividualType.ServerIllustratedBookRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00024");
                    break;

                case EnRankingIndividualType.ServerPresentPopularityPointRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A108_TXT_06007");
                    break;

                case EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A108_TXT_06008");
                    break;

                case EnRankingIndividualType.ServerPresentContributionPointRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A108_TXT_06009");
                    break;

                case EnRankingIndividualType.NationWeeklyPresentContributionPointRanking:
                    textPoint.text = CsConfiguration.Instance.GetString("A108_TXT_06010");
                    break;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingNation)
        {
            switch (m_enRankingNationType)
            {
                case EnRankingNationType.NationPowerRanking:
                    textNation.text = CsConfiguration.Instance.GetString("A16_TXT_00027");
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00025");
                    break;

                case EnRankingNationType.NationExploitPointRanking:
                    textNation.text = CsConfiguration.Instance.GetString("A16_TXT_00007");
                    textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00014");
                    break;
            }
        }
        else if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingJob)
        {
            textNation.text = CsConfiguration.Instance.GetString("A16_TXT_00004");
            textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00012");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonReward()
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            if (m_enRankingIndividualType == EnRankingIndividualType.ServerLevelRanking || 
                m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking || 
                m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentContributionPointRanking)
            {
                int nRanking = 0;
                int nRankingNo = 0;
                int nRewardedRankingNo = 0;

                if (m_enRankingIndividualType == EnRankingIndividualType.ServerLevelRanking)
                {
                    nRanking = CsGameData.Instance.MyHeroInfo.DailyServerLevelRanking;
                    nRankingNo = CsGameData.Instance.MyHeroInfo.DailyServerLevelRakingNo;
                    nRewardedRankingNo = CsGameData.Instance.MyHeroInfo.RewardedDailyServerLevelRankingNo;

                    if (nRanking == 0)
                    {
                        m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                        m_buttonReward.interactable = false;
                    }
                    else
                    {
                        m_buttonReward.transition = Selectable.Transition.None;

                        if (nRanking <= CsGameData.Instance.LevelRankingRewardList[CsGameData.Instance.LevelRankingRewardList.Count - 1].LowRanking)
                        {
                            // 이미 받음
                            if (nRankingNo == nRewardedRankingNo)
                            {
                                m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
                                m_buttonReward.interactable = false;
                            }
                            else
                            {
                                m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift02");
                                m_buttonReward.transition = Selectable.Transition.ColorTint;
                                m_buttonReward.interactable = true;
                            }
                        }
                        else
                        {
                            m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                            m_buttonReward.interactable = false;
                        }
                    }
                }
                else if (m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentPopularityPointRanking)
                {
                    nRanking = CsPresentManager.Instance.NationWeeklyPresentPopularityPointRanking;
                    nRankingNo = CsPresentManager.Instance.NationWeeklyPresentPopularityPointRankingNo;
                    nRewardedRankingNo = CsPresentManager.Instance.RewardedNationWeeklyPresentPopularityPointRankingNo;

                    if (nRanking == 0)
                    {
                        m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                        m_buttonReward.interactable = false;
                    }
                    else
                    {
                        m_buttonReward.transition = Selectable.Transition.None;

                        if (nRanking <= CsGameData.Instance.WeeklyPresentPopularityPointRankingRewardGroupList[CsGameData.Instance.WeeklyPresentPopularityPointRankingRewardGroupList.Count - 1].LowRanking)
                        {
                            // 이미 받음
                            if (nRankingNo == nRewardedRankingNo)
                            {
                                m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
                                m_buttonReward.interactable = false;
                            }
                            else
                            {
                                m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift02");
                                m_buttonReward.transition = Selectable.Transition.ColorTint;
                                m_buttonReward.interactable = true;
                            }
                        }
                        else
                        {
                            m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                            m_buttonReward.interactable = false;
                        }
                    }
                }
                else if (m_enRankingIndividualType == EnRankingIndividualType.NationWeeklyPresentContributionPointRanking)
                {
                    nRanking = CsPresentManager.Instance.NationWeeklyPresentContributionPointRanking;
                    nRankingNo = CsPresentManager.Instance.NationWeeklyPresentContributionPointRankingNo;
                    nRewardedRankingNo = CsPresentManager.Instance.RewardedNationWeeklyPresentContributionPointRankingNo;

                    if (nRanking == 0)
                    {
                        m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                        m_buttonReward.interactable = false;
                    }
                    else
                    {
                        m_buttonReward.transition = Selectable.Transition.None;

                        if (nRanking <= CsGameData.Instance.WeeklyPresentContributionPointRankingRewardGroupList[CsGameData.Instance.WeeklyPresentContributionPointRankingRewardGroupList.Count - 1].LowRanking)
                        {
                            // 이미 받음
                            if (nRankingNo == nRewardedRankingNo)
                            {
                                m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
                                m_buttonReward.interactable = false;
                            }
                            else
                            {
                                m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift02");
                                m_buttonReward.transition = Selectable.Transition.ColorTint;
                                m_buttonReward.interactable = true;
                            }
                        }
                        else
                        {
                            m_buttonReward.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
                            m_buttonReward.interactable = false;
                        }
                    }
                }

                m_buttonReward.gameObject.SetActive(true);
                m_buttonRewardCheck.gameObject.SetActive(true);
            }
            else
            {
                m_buttonReward.gameObject.SetActive(false);
                m_buttonRewardCheck.gameObject.SetActive(false);
            }
        }
        else
        {
            m_buttonReward.gameObject.SetActive(false);
            m_buttonRewardCheck.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageGuildRankTop()
    {
        Text textName = m_trPanelMyRanking.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsConfiguration.Instance.GetString("A16_TXT_00018");

        Text textMyRank = m_trPanelMyRanking.Find("TextMyRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyRank);

        // 내 랭킹 없으면 '미진입' 있으면 '랭킹 순위'
        if (m_csGuildRanking == null)
        {
            textMyRank.text = CsConfiguration.Instance.GetString("A16_TXT_00002");
        }
        else
        {
            textMyRank.text = m_csGuildRanking.Ranking.ToString("#,##0");
        }

        m_trPanelYesterday.gameObject.SetActive(false);
        m_buttonReward.gameObject.SetActive(false);
        m_buttonRewardCheck.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePanelGuildRanking()
    {
        Text textRanking = m_trPanelRanking.Find("TextRanking").GetComponent<Text>();
        textRanking.text = CsConfiguration.Instance.GetString("A16_TXT_00003");
        CsUIData.Instance.SetFont(textRanking);

        Text textNation = m_trPanelRanking.Find("TextNation").GetComponent<Text>();
        textNation.text = CsConfiguration.Instance.GetString("A16_TXT_00015");
        CsUIData.Instance.SetFont(textNation);

        Text textHeroName = m_trPanelRanking.Find("TextName").GetComponent<Text>();
        textHeroName.text = CsConfiguration.Instance.GetString("A16_TXT_00016");
        CsUIData.Instance.SetFont(textHeroName);

        Text textPoint = m_trPanelRanking.Find("TextPoint").GetComponent<Text>();
        textPoint.text = CsConfiguration.Instance.GetString("A16_TXT_00017");
        CsUIData.Instance.SetFont(textPoint);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateGuildRankingItemList()
    {
        // 랭킹 리스트 초기화
        for (int i = 0; i < m_trRankingList.childCount; i++)
        {
            m_trRankingList.GetChild(i).gameObject.SetActive(false);
        }

        int nItemSize = 88;
        int nBaseLoadCount = 10;

        if (m_listCsGuildRanking.Count < nBaseLoadCount)
        {
            nBaseLoadCount = m_listCsGuildRanking.Count;
        }

        RectTransform rectTransform = m_trRankingList.GetComponent<RectTransform>();

        if (m_listCsGuildRanking.Count < CsGameConfig.Instance.RankingDisplayMaxCount)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * m_listCsGuildRanking.Count);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, nItemSize * CsGameConfig.Instance.RankingDisplayMaxCount);
        }

        for (int i = 0; i < nBaseLoadCount; i++)
        {
            CreateRankingItem(m_listCsGuildRanking[i]);
        }

        Scrollbar scrollbar = m_trPanelRanking.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.RemoveAllListeners();
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedRankingScrollbar(scrollbar));
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRankingItem(CsGuildRanking csGuildRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csGuildRanking.Ranking);

        if (trRankingSlot == null)
        {
            trRankingSlot = Instantiate(m_goRankingItem, m_trRankingList).transform;
            trRankingSlot.name = "RankingSlot" + csGuildRanking.Ranking;
        }
        else
        {
            trRankingSlot.gameObject.SetActive(true);
        }

        Image imageRank = trRankingSlot.Find("ImageRank").GetComponent<Image>();
        imageRank.gameObject.SetActive(true);

        Text textRank = trRankingSlot.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = csGuildRanking.Ranking.ToString("#,##0");

        if (csGuildRanking.Ranking == 1)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_gold");
            textRank.color = new Color32(254, 148, 0, 255);
        }
        else if (csGuildRanking.Ranking == 2)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_silver");
            textRank.color = new Color32(40, 121, 255, 255);
        }
        else if (csGuildRanking.Ranking == 3)
        {
            imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRanking/frm_wreath_bronze");
            textRank.color = new Color32(206, 170, 139, 255);
        }
        else
        {
            imageRank.gameObject.SetActive(false);
        }

        m_nLoadRankingItemCount++;
        UpdateRankingItem(csGuildRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingItem(CsGuildRanking csGuildRanking)
    {
        Transform trRankingSlot = m_trRankingList.Find("RankingSlot" + csGuildRanking.Ranking);

        if (trRankingSlot == null)
        {
            return;
        }
        else
        {
            Transform trTag = trRankingSlot.Find("Tag");

            Image imageTag = trTag.Find("Image").GetComponent<Image>();
            if (imageTag.gameObject.activeSelf)
                imageTag.gameObject.SetActive(false);

            Text textTag = trTag.Find("TextNationName").GetComponent<Text>();
            textTag.text = csGuildRanking.GuildName;
            CsUIData.Instance.SetFont(textTag);

            Text textHeroName = trRankingSlot.Find("TextHeroName").GetComponent<Text>();
            textHeroName.text = csGuildRanking.GuildMasterName;
            CsUIData.Instance.SetFont(textHeroName);

            Text textBattlePower = trRankingSlot.Find("TextBattlePower").GetComponent<Text>();
            textBattlePower.text = csGuildRanking.Might.ToString("#,##0");
            CsUIData.Instance.SetFont(textBattlePower);

        }
    }

    //---------------------------------------------------------------------------------------------------
    public void ChangeToggleLevelRanking()
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.RankingIndividual)
        {
            int nCount = 0;
            
            foreach (EnRankingIndividualType enumItem in System.Enum.GetValues(typeof(EnRankingIndividualType)))
            {
                if (enumItem == EnRankingIndividualType.ServerLevelRanking)
                {
                    Toggle toggleRanking = m_trPanelLeft.Find("ToggleRanking" + nCount).GetComponent<Toggle>();
                    toggleRanking.isOn = true;
                }

                nCount++;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationPowerRanking()
    {
        if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList == null)
        {
            m_nRankingCount = CsGameData.Instance.NationList.Count;
        }
        else
        {
            m_nRankingCount = CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Count;
        }

        UpdateImageRankTop();
        UpdatePanelRanking();
        UpdateButtonReward();
        CreateRankingItemList();
    }
}