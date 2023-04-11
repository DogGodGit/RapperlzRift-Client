using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public enum EnQuestCategoryType
{
    MainQuest = 0,
    JobQuest = 1,
    SubQuest = 2,
    BiographyQuest = 3,
    CommonQuest = 4,
    DailyQuest = 5,
    WeeklyQuest = 6,
    GuildQuest = 7,
    NationQuest = 8,
}

public enum EnQuestType
{
    MainQuest = 0,          // 메인 퀘스트
    // 일상 
    ThreatOfFarm,           // 농장의 위협
    BountyHunter,           // 현상금 사냥
    // 일일 퀘스트
    DailyQuest01, 
    DailyQuest02, 
    DailyQuest03, 
    // 주간 퀘스트
    WeeklyQuest,            // 주간 퀘스트
    // 길드
    GuildAlter,             // 길드 제단
    GuildFarm,              // 길드 농장
    GuildMission,           // 길드 미션
    GuildAlterDefence,      // 길드 제단 수호
    GuildSupplySupport,     // 길드 물자지원
    GuildHunting,           // 길드 헌팅
    // 국가
    SecretLetter,           // 밀서
    MysteryBox,             // 의문의 상자
    DimensionRaid,          // 차원의 습격
    HolyWar,                // 위대한 성전
    SupplySupport,          // 세리우 보급지원
    // 던전
    Dungeon,                // 던전 퀘스트
	
	TrueHero,				// 진정한 영웅
	SubQuest,				// 서브 퀘스트
	Biography,				// 전기 퀘스트
	CreatureFarm,			// 크리처 농장
	JobChange,				// 전직
}

public class CsQuestList : CsPopupSub
{
    Transform m_trContent;

    GameObject m_goCartegory;
    GameObject m_goToggleQuest;

    Sprite m_spriteArrowUp;
    Sprite m_spriteArrowDown;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        // 일상
        CsThreatOfFarmQuestManager.Instance.EventMissionAbandoned += OnEventMissionAbandoned;
        CsThreatOfFarmQuestManager.Instance.EventMissionFail += OnEventMissionFail;

        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestAbandon += OnEventBountyHunterQuestAbandon;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete += OnEventBountyHunterQuestComplete;

        // 일일
        CsDailyQuestManager.Instance.EventDailyQuestAbandon += OnEventDailyQuestAbandon;

        // 주간
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated += OnEventWeeklyQuestRoundProgressCountUpdated;

        // 길드
        CsGuildManager.Instance.EventGuildMissionComplete += OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildMissionAbandon += OnEventGuildMissionAbandon;
        CsGuildManager.Instance.EventGuildMissionFailed += OnEventGuildMissionFailed;
        CsGuildManager.Instance.EventUpdateMissionState += OnEventUpdateMissionState;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail += OnEventGuildSupplySupportQuestFail;
        CsGuildManager.Instance.EventGuildHuntingQuestAbandon += OnEventGuildHuntingQuestAbandon;

        // 국가
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail += OnEventSupplySupportQuestFail;

        // 서브퀘스트
        CsSubQuestManager.Instance.EventSubQuestAbandon += OnEventSubQuestAbandon;

        // 전직 퀘스트
        CsJobChangeManager.Instance.EventJobChangeQuestProgressCountUpdated += OnEventJobChangeQuestProgressCountUpdated;
        CsJobChangeManager.Instance.EventJobChangeQuestFailed += OnEventJobChangeQuestFailed;
    }

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		UpdateDefaultSelect();
	}

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        // 일상
        CsThreatOfFarmQuestManager.Instance.EventMissionAbandoned -= OnEventMissionAbandoned;
        CsThreatOfFarmQuestManager.Instance.EventMissionFail -= OnEventMissionFail;
        
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestAbandon -= OnEventBountyHunterQuestAbandon;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete -= OnEventBountyHunterQuestComplete;

        // 일일
        CsDailyQuestManager.Instance.EventDailyQuestAbandon -= OnEventDailyQuestAbandon;

        // 주간
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated += OnEventWeeklyQuestRoundProgressCountUpdated;

        // 길드
        CsGuildManager.Instance.EventGuildMissionComplete -= OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildMissionAbandon -= OnEventGuildMissionAbandon;
        CsGuildManager.Instance.EventGuildMissionFailed -= OnEventGuildMissionFailed;
        CsGuildManager.Instance.EventUpdateMissionState -= OnEventUpdateMissionState;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail -= OnEventGuildSupplySupportQuestFail;
        CsGuildManager.Instance.EventGuildHuntingQuestAbandon -= OnEventGuildHuntingQuestAbandon;

        // 국가
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail -= OnEventSupplySupportQuestFail;

		// 서브퀘스트
		CsSubQuestManager.Instance.EventSubQuestAbandon -= OnEventSubQuestAbandon;

        // 전직 퀘스트
        CsJobChangeManager.Instance.EventJobChangeQuestProgressCountUpdated -= OnEventJobChangeQuestProgressCountUpdated;
        CsJobChangeManager.Instance.EventJobChangeQuestFailed -= OnEventJobChangeQuestFailed;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	void OnvalueChangedDisplayPanel(Toggle toggle, EnQuestType enQuestType, EnQuestCategoryType enQuestCartegoryType, int nParam)
    {
        Transform trCartegory = m_trContent.Find("Cartogory" + (int)enQuestCartegoryType);
        Toggle toggleAllSelect = trCartegory.Find("ImageBack/ToggleAllSelect").GetComponent<Toggle>();

        if (toggle.isOn)
        {
            if (toggleAllSelect.isOn == false)
            {
                DisplayChangedSelectAllToggle(enQuestCartegoryType, true);
            }
        }
        else
        {
            Transform trCategoryQuestList = trCartegory.Find("QuestList");
            bool bAllOff = true;

            for (int i = 0; i < trCategoryQuestList.childCount; i++)
            {
                if (trCategoryQuestList.GetChild(i).Find("ToggleSwitch").GetComponent<Toggle>().isOn)
                {
                    bAllOff = false;
                    break;
                }
                else
                {
                    continue;
                }
            }

            if (bAllOff)
            {
                DisplayChangedSelectAllToggle(enQuestCartegoryType, false);
            }
        }

		CsGameEventUIToUI.Instance.OnEventDisplayQuestPanel(enQuestType, toggle.isOn, nParam);
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	void OnValueChangedQuestSelect(Toggle toggle, EnQuestType enQuestType, int nParam)
    {
        if (toggle.isOn)
        {
			CsGameEventUIToUI.Instance.OnEventSelectQuest(enQuestType, nParam, false);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //카테고리 전체선택
    void OnValueChangedAllSelect(Toggle toggle, EnQuestCategoryType enQuestCartegoryType)
    {
        Transform trCategoryQuestList = m_trContent.Find("Cartogory" + (int)enQuestCartegoryType + "/QuestList");

        Toggle toggleDisplay;

        for (int i = 0; i < trCategoryQuestList.childCount; i++)
        {
            toggleDisplay = trCategoryQuestList.GetChild(i).Find("ToggleSwitch").GetComponent<Toggle>();

            if (toggle.isOn)
            {
                toggleDisplay.isOn = true;
            }
            else
            {
                toggleDisplay.isOn = false;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //카테고리 열기/닫기
    void OnValueChangedCartegoryOpen(Toggle toggle)
    {
        Transform trList = toggle.transform.parent.parent.Find("QuestList");
        Image imageArrow = toggle.transform.Find("Background").GetComponent<Image>();

        if (toggle.isOn)
        {
            if (m_spriteArrowUp == null)
            {
                m_spriteArrowUp = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_arrow01_up");
            }

            imageArrow.sprite = m_spriteArrowUp;
            trList.gameObject.SetActive(true);
        }
        else
        {
            if (m_spriteArrowDown == null)
            {
                m_spriteArrowDown = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_arrow01_down");
            }

            imageArrow.sprite = m_spriteArrowDown;

            for (int i = 0; i < trList.childCount; i++)
            {
                if (trList.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    UpdateDefaultSelect();
                    break;
                }
                else
                {
                    continue;
                }
            }

            trList.gameObject.SetActive(false);
        }
    }

    #endregion Event

    #region EventHandler

    #region ThreatOfFarmQuest

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionAbandoned()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.ThreatOfFarm, EnQuestCategoryType.CommonQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionFail()
    {
        UpdateQuestCartegory(EnQuestType.ThreatOfFarm, EnQuestCategoryType.CommonQuest);
    }

    #endregion ThreatOfFarmQuest

    #region BountyHunter

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestAbandon()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.BountyHunter, EnQuestCategoryType.CommonQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.BountyHunter, EnQuestCategoryType.CommonQuest);
    }

    #endregion BountyHunter

    #region DailyQuest

    //---------------------------------------------------------------------------------------------------
    void OnEventDailyQuestAbandon(int nSlotIndex)
    {
        UpdateDefaultSelect();

        EnQuestType enQuestType = (EnQuestType)(nSlotIndex + (int)EnQuestType.DailyQuest01);
        UpdateQuestCartegory(enQuestType, EnQuestCategoryType.DailyQuest);
    }

    #endregion DailyQuest

    #region WeeklyQuest

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundMoveMissionComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.WeeklyQuest, EnQuestCategoryType.WeeklyQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundProgressCountUpdated()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.WeeklyQuest, EnQuestCategoryType.WeeklyQuest);
    }

    #endregion WeeklyQuest

    #region Guild

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionComplete(bool bLevelUp, long lAcquredExp)
    {
        UpdateToggleQuestName(EnQuestCategoryType.GuildQuest, EnQuestType.GuildMission, string.Format(CsGuildManager.Instance.GuildMission.TargetTitle, CsGuildManager.Instance.MissionCompletedCount, CsGuildManager.Instance.GuildMissionQuest.LimitCount));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionAbandon()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.GuildMission, EnQuestCategoryType.GuildQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionFailed()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.GuildMission, EnQuestCategoryType.GuildQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateMissionState()
    {
        if (CsGuildManager.Instance.GuildMission != null)
        {
            UpdateToggleQuestName(EnQuestCategoryType.GuildQuest, EnQuestType.GuildMission, string.Format(CsGuildManager.Instance.GuildMission.TargetTitle, CsGuildManager.Instance.MissionCompletedCount, CsGuildManager.Instance.GuildMissionQuest.LimitCount));
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestFail()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.GuildSupplySupport, EnQuestCategoryType.GuildQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingQuestAbandon()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.GuildHunting, EnQuestCategoryType.GuildQuest);
    }

    #endregion Guild

    #region SupplySupport

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestFail()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.SupplySupport, EnQuestCategoryType.NationQuest);
    }

    #endregion SupplySupport

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestAbandon(int nSubQuestId)
	{
		UpdateDefaultSelect();
		UpdateQuestCartegory(EnQuestType.SubQuest, EnQuestCategoryType.SubQuest, nSubQuestId);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestProgressCountUpdated()
    {
        UpdateQuestCartegory(EnQuestType.JobChange, EnQuestCategoryType.JobQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestFailed()
    {
        UpdateDefaultSelect();
        UpdateQuestCartegory(EnQuestType.JobChange, EnQuestCategoryType.JobQuest);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");

        m_goCartegory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupQuestList/QuestCartegory");
        m_goToggleQuest = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupQuestList/ToggleQuest");

        // 메인 퀘스트
        if (CsMainQuestManager.Instance.MainQuest != null)
        {
            CreateCartegory(EnQuestCategoryType.MainQuest);
        }

        // 일상 퀘스트
        if (CsThreatOfFarmQuestManager.Instance.Mission != null)
        {
            CreateCartegory(EnQuestCategoryType.CommonQuest);                          // 농장의 위협
        }
        else if (CsBountyHunterQuestManager.Instance.BountyHunterQuest != null)
        {
            CreateCartegory(EnQuestCategoryType.CommonQuest);                          // 현상금 사냥꾼
        }
		else if (CsTrueHeroQuestManager.Instance.TrueHeroQuestState != EnTrueHeroQuestState.None)
		{
			CreateCartegory(EnQuestCategoryType.CommonQuest);
		}
		else if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState != EnCreatureFarmQuestState.None &&
			CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState != EnCreatureFarmQuestState.Completed)
		{
			CreateCartegory(EnQuestCategoryType.CommonQuest);
		}
		
		// 일일 퀘스트
        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            if (CsDailyQuestManager.Instance.HeroDailyQuestList[i].IsAccepted)
            {
                CreateCartegory(EnQuestCategoryType.DailyQuest);
                break;
            }
            else
            {
                continue;
            }
        }

        // 주간 퀘스트
        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest != null &&
			CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
        {
            CreateCartegory(EnQuestCategoryType.WeeklyQuest);
        }

        // 길드 퀘스트
        if (CsGuildManager.Instance.GuildMission != null)
        {
            CreateCartegory(EnQuestCategoryType.GuildQuest);                           // 길드 미션
        }
        else if (CsGuildManager.Instance.GuildSupplySupportQuestPlay != null)
        {
            CreateCartegory(EnQuestCategoryType.GuildQuest);                           // 길드 물자 지원
        }
        else if (CsGuildManager.Instance.GuildHuntingQuestObjective != null)
        {
            CreateCartegory(EnQuestCategoryType.GuildQuest);                           // 길드 현팅 퀘스트
        }
        else if (CsGuildManager.Instance.IsGuildDefense)
        {
            CreateCartegory(EnQuestCategoryType.GuildQuest);                           // 길드 제단 수호
        }
        else if (CsGuildManager.Instance.HeroGuildFarmQuest != null)
        {
            CreateCartegory(EnQuestCategoryType.GuildQuest);                           // 길드 농장
        }


        // 국가 퀘스트
        if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.None)
        {
            CreateCartegory(EnQuestCategoryType.NationQuest);                          // 밀서
        }
        else if (CsMysteryBoxQuestManager.Instance.MysteryBoxState != EnMysteryBoxState.None)
        {
            CreateCartegory(EnQuestCategoryType.NationQuest);                          // 의문의 상자
        }
        else if (CsDimensionRaidQuestManager.Instance.DimensionRaidState != EnDimensionRaidState.None)
        {
            CreateCartegory(EnQuestCategoryType.NationQuest);                          // 차원의 습격
        }
        else if (CsHolyWarQuestManager.Instance.HolyWarQuestState != EnHolyWarQuestState.None)
        {
            CreateCartegory(EnQuestCategoryType.NationQuest);                          // 위대한 성전
        }
        else if (CsSupplySupportQuestManager.Instance.QuestState != EnSupplySupportState.None)
        {
            CreateCartegory(EnQuestCategoryType.NationQuest);                          // 보급 지원
        }

		for (int i = 0; i < CsSubQuestManager.Instance.HeroSubQuestList.Count; i++)
		{
			if (CsSubQuestManager.Instance.HeroSubQuestList[i].EnStatus != EnSubQuestStatus.Abandon)
			{
				CreateCartegory(EnQuestCategoryType.SubQuest);
				break;
			}
		}

		for (int i = 0; i < CsBiographyManager.Instance.HeroBiographyList.Count; i++)
		{
			CsBiography csBiography = CsBiographyManager.Instance.HeroBiographyList[i].Biography;
			CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.HeroBiographyList[i].HeroBiograhyQuest;

			if (csHeroBiographyQuest != null &&
				(!csHeroBiographyQuest.Completed || csBiography.GetBiographyQuest(csHeroBiographyQuest.QuestNo + 1) != null))
			{
				CreateCartegory(EnQuestCategoryType.BiographyQuest);
				break;
			}
		}

        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
        {

        }
        else
        {
            CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

            if (csHeroJobChangeQuest == null)
            {
                // 퀘스트 수락 가능
                CreateCartegory(EnQuestCategoryType.JobQuest);
            }
            else
            {
                if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
                {
                    // 퀘스트 완료
                }
                else
                {
                    // 퀘스트 진행 중
                    CreateCartegory(EnQuestCategoryType.JobQuest);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateCartegory(EnQuestCategoryType enQuestCartegoryType)
    {
        GameObject goCartegory = Instantiate(m_goCartegory, m_trContent);
        goCartegory.name = "Cartogory" + (int)enQuestCartegoryType;

        Transform trMainQuestCartegory = goCartegory.transform;
        Transform trBack = trMainQuestCartegory.Find("ImageBack");

        Text textCartegoryName = trBack.Find("TextCartegoryName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCartegoryName);

        Toggle toggleAllSelect = trBack.Find("ToggleAllSelect").GetComponent<Toggle>();
        toggleAllSelect.onValueChanged.RemoveAllListeners();
        toggleAllSelect.isOn = true;
        toggleAllSelect.onValueChanged.AddListener((ison) => OnValueChangedAllSelect(toggleAllSelect, enQuestCartegoryType));
        toggleAllSelect.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Toggle toggleCartegoryOpen = trBack.Find("ToggleCartegoryOpen").GetComponent<Toggle>();
        toggleCartegoryOpen.onValueChanged.RemoveAllListeners();
        toggleCartegoryOpen.onValueChanged.AddListener((ison) => OnValueChangedCartegoryOpen(toggleCartegoryOpen));
        toggleCartegoryOpen.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Transform trQuestList = trMainQuestCartegory.Find("QuestList");

        switch (enQuestCartegoryType)
        {
            case EnQuestCategoryType.MainQuest:
                textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00001");
                toggleAllSelect.gameObject.SetActive(false);
                toggleCartegoryOpen.gameObject.SetActive(false);

                //퀘스트 토글 만들기.
                if (CsMainQuestManager.Instance.MainQuest != null)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.MainQuest, enQuestCartegoryType);
                }

                break;

            case EnQuestCategoryType.CommonQuest:
                textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00005");

                // 일상 퀘스트 토글 만들기
                if (CsThreatOfFarmQuestManager.Instance.Mission != null)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.ThreatOfFarm, enQuestCartegoryType);             // 농장의 위협
                }
                if (CsBountyHunterQuestManager.Instance.BountyHunterQuest != null)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.BountyHunter, enQuestCartegoryType);             // 현상금 사냥
                }
				if (CsTrueHeroQuestManager.Instance.TrueHeroQuestState != EnTrueHeroQuestState.None)
				{
					CreateQuestToggle(trQuestList, EnQuestType.TrueHero, enQuestCartegoryType);
				}
				if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState != EnCreatureFarmQuestState.None &&
					CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState != EnCreatureFarmQuestState.Completed)
				{
					CreateQuestToggle(trQuestList, EnQuestType.CreatureFarm, enQuestCartegoryType);
				}

                break;

            case EnQuestCategoryType.DailyQuest:
                textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00006");

                int nQuestType = (int)EnQuestType.DailyQuest01;

                for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
                {
                    CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[i];

                    if (csHeroDailyQuest.IsAccepted)
                    {
                        CreateQuestToggle(trQuestList, (EnQuestType)nQuestType, enQuestCartegoryType);
                    }

                    nQuestType++;
                }

                break;

            case EnQuestCategoryType.WeeklyQuest:
                textCartegoryName.text = CsConfiguration.Instance.GetString(CsGameData.Instance.WeeklyQuest.Title);

                if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.WeeklyQuest, enQuestCartegoryType);
                }

                break;

            case EnQuestCategoryType.GuildQuest:
                textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00008");

                if (CsGuildManager.Instance.GuildMissionState == EnGuildMissionState.Accepted)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.GuildMission, enQuestCartegoryType);             // 길드 미션
                }
                if (CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.Accepted)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.GuildSupplySupport, enQuestCartegoryType);       // 길드 보급 지원
                }
                if (CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Accepted || CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Executed)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.GuildHunting, enQuestCartegoryType);             // 길드 현상금 사냥
                }
                if (CsGuildManager.Instance.IsGuildDefense)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.GuildAlterDefence, enQuestCartegoryType);
                }
                if (CsGuildManager.Instance.GuildFarmQuestState != EnGuildFarmQuestState.None)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.GuildFarm, enQuestCartegoryType);
                }

                break;

            case EnQuestCategoryType.NationQuest:
                textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00009");

                // 국가 퀘스트 토글 만들기
                if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.None)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.SecretLetter, enQuestCartegoryType);             // 밀서
                }
                if (CsMysteryBoxQuestManager.Instance.MysteryBoxState != EnMysteryBoxState.None)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.MysteryBox, enQuestCartegoryType);               // 의문의 상자
                }
                if (CsDimensionRaidQuestManager.Instance.DimensionRaidState != EnDimensionRaidState.None)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.DimensionRaid, enQuestCartegoryType);            // 차원의 습격
                }
                if (CsHolyWarQuestManager.Instance.HolyWarQuestState != EnHolyWarQuestState.None)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.HolyWar, enQuestCartegoryType);                  // 위대한 성전
                }
                if (CsSupplySupportQuestManager.Instance.QuestState != EnSupplySupportState.None)
                {
                    CreateQuestToggle(trQuestList, EnQuestType.SupplySupport, enQuestCartegoryType);            // 물자 지원
                }

                break;

			case EnQuestCategoryType.SubQuest:
				textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00003");

				foreach (CsHeroSubQuest csHeroSubQuest in CsSubQuestManager.Instance.HeroSubQuestList)
				{
					if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Acception ||
						csHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
					{
						CreateQuestToggle(trQuestList, EnQuestType.SubQuest, enQuestCartegoryType, csHeroSubQuest.SubQuest.QuestId);
					}
				}

				break;

			case EnQuestCategoryType.BiographyQuest:
				textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00004");

				foreach (CsHeroBiography csHeroBiography in CsBiographyManager.Instance.HeroBiographyList)
				{
					if (csHeroBiography.HeroBiograhyQuest != null)
					{
						if (!csHeroBiography.HeroBiograhyQuest.Completed ||
							csHeroBiography.Biography.GetBiographyQuest(csHeroBiography.HeroBiograhyQuest.QuestNo + 1) != null)
						{
							CreateQuestToggle(trQuestList, EnQuestType.Biography, enQuestCartegoryType, csHeroBiography.BiographyId);
						}
					}
				}

				break;

            case EnQuestCategoryType.JobQuest:

                textCartegoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00002");

                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
                {

                }
                else
                {
                    CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

                    if (csHeroJobChangeQuest == null)
                    {
                        // 퀘스트 수락 가능
                        CreateQuestToggle(trQuestList, EnQuestType.JobChange, enQuestCartegoryType);
                    }
                    else
                    {
                        if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
                        {
                            // 퀘스트 완료
                        }
                        else
                        {
                            // 퀘스트 진행 중
                            CreateQuestToggle(trQuestList, EnQuestType.JobChange, enQuestCartegoryType);
                        }
                    }
                }

                break;
        }

        UpdateDisplayToggle(enQuestCartegoryType);
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
    void CreateQuestToggle(Transform trQuestList, EnQuestType enQuestType, EnQuestCategoryType enQuestCartegoryType, int nParam = 0)
    {
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

        GameObject goToggleQuest = Instantiate(m_goToggleQuest, trQuestList);
		goToggleQuest.name = "ToggleQuest" + strName;

        Toggle toggleQuest = goToggleQuest.GetComponent<Toggle>();
        toggleQuest.onValueChanged.RemoveAllListeners();

        if (enQuestType == EnQuestType.MainQuest)
        {
            toggleQuest.isOn = true;
        }
        else
        {
            toggleQuest.isOn = false;
        }

		toggleQuest.onValueChanged.AddListener((ison) => OnValueChangedQuestSelect(toggleQuest, enQuestType, nParam));
        toggleQuest.group = m_trContent.GetComponent<ToggleGroup>();

        Toggle toggleDisplay = toggleQuest.transform.Find("ToggleSwitch").GetComponent<Toggle>();
        toggleDisplay.onValueChanged.RemoveAllListeners();

		if (PlayerPrefs.HasKey(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName))
        {
			toggleDisplay.isOn = PlayerPrefs.GetInt(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName) == 1;
        }
        else
        {
            toggleDisplay.isOn = true;
        }

        toggleDisplay.onValueChanged.AddListener((ison) => OnvalueChangedDisplayPanel(toggleDisplay, enQuestType, enQuestCartegoryType, nParam));
        toggleDisplay.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Transform trImageComplete = toggleQuest.transform.Find("ImageComplete");

        Text textQuestName = toggleQuest.transform.Find("TextQuestName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textQuestName);

        int nHeroDailyQuestIndex = (int)enQuestType - (int)EnQuestType.DailyQuest01;
        CsHeroDailyQuest csHeroDailyQuest = null;

        switch (enQuestType)
        {
            case EnQuestType.MainQuest:
                toggleDisplay.gameObject.SetActive(false);

                if (CsMainQuestManager.Instance.MainQuest == null)
                {
                    trImageComplete.gameObject.SetActive(true);
                    textQuestName.text = CsConfiguration.Instance.GetString("");
                }
				else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Completed)
				{
					trImageComplete.gameObject.SetActive(true);
					textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_00002");
				}
                else
                {
                    textQuestName.text = CsMainQuestManager.Instance.MainQuest.Title;

                    if (CsMainQuestManager.Instance.IsExecuted)
                    {
                        trImageComplete.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageComplete.gameObject.SetActive(false);
                    }
                }
                break;

            // 일상
            case EnQuestType.ThreatOfFarm:
                textQuestName.text = CsThreatOfFarmQuestManager.Instance.Quest.Title;

                if (CsThreatOfFarmQuestManager.Instance.QuestState == EnQuestState.Complete)
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                break;

            case EnQuestType.BountyHunter:
                textQuestName.text = CsBountyHunterQuestManager.Instance.BountyHunterQuest.TargetTitle;

                if (CsBountyHunterQuestManager.Instance.ProgressCount >= CsBountyHunterQuestManager.Instance.BountyHunterQuest.TargetCount)
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                break;

			case EnQuestType.TrueHero:
				textQuestName.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.Name;

				if (CsTrueHeroQuestManager.Instance.TrueHeroQuestState == EnTrueHeroQuestState.Completed)
				{
					trImageComplete.gameObject.SetActive(true);
				}
				else
				{
					trImageComplete.gameObject.SetActive(false);
				}
				break;

			case EnQuestType.CreatureFarm:

				int nMissionCount = CsGameData.Instance.CreatureFarmQuest.CreatureFarmQuestMissionList.Count;

				 if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
				 {
					 textQuestName.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01004"), string.Format(CsConfiguration.Instance.GetString("A149_TXT_00001"), nMissionCount, nMissionCount));

					 trImageComplete.gameObject.SetActive(true);
				 }
				 else
				 {
					 textQuestName.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01004"), string.Format(CsConfiguration.Instance.GetString("A149_TXT_00001"), CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.MissionNo, nMissionCount));
					 
					 trImageComplete.gameObject.SetActive(false);
				 }

				break;

            // 일일 퀘스트
            case EnQuestType.DailyQuest01: 
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
                textQuestName.text = csHeroDailyQuest.DailyQuestMission.Title;

                if (csHeroDailyQuest.ProgressCount < csHeroDailyQuest.DailyQuestMission.TargetCount)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                break;

            case EnQuestType.DailyQuest02:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
                textQuestName.text = csHeroDailyQuest.DailyQuestMission.Title;

                if (csHeroDailyQuest.ProgressCount < csHeroDailyQuest.DailyQuestMission.TargetCount)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                break;

            case EnQuestType.DailyQuest03:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
                textQuestName.text = csHeroDailyQuest.DailyQuestMission.Title;

                if (csHeroDailyQuest.ProgressCount < csHeroDailyQuest.DailyQuestMission.TargetCount)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                break;

            // 주간
            case EnQuestType.WeeklyQuest:
                textQuestName.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.Title), (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo - 1), CsGameData.Instance.WeeklyQuest.RoundCount);

                if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount < CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetCount)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                break;

            // 국가
            case EnQuestType.SecretLetter:
                textQuestName.text = CsSecretLetterQuestManager.Instance.SecretLetterQuest.TargetTitle;

                if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Accepted)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                break;

            case EnQuestType.MysteryBox:
                textQuestName.text = CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.TargetTitle;

                if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Accepted)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                break;

            case EnQuestType.DimensionRaid:
                CsDimensionRaidQuestStep csDimensionRaidQuestStep = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestStep(CsDimensionRaidQuestManager.Instance.Step);

                if (csDimensionRaidQuestStep == null)
                {
                    textQuestName.text = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestStep(1).TargetTitle;
                }
                else
                {
                    textQuestName.text = csDimensionRaidQuestStep.TargetTitle;
                }

                if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                break;

            case EnQuestType.HolyWar:
                textQuestName.text = CsHolyWarQuestManager.Instance.HolyWarQuest.TargetTitle;

                if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
                {
                    trImageComplete.gameObject.SetActive(true);
                }
                else
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                break;

            case EnQuestType.SupplySupport:
                textQuestName.text = CsSupplySupportQuestManager.Instance.SupplySupportQuest.TargetTitle;
                trImageComplete.gameObject.SetActive(false);
                break;

            // 길드
            case EnQuestType.GuildMission:
                textQuestName.text = string.Format(CsGuildManager.Instance.GuildMission.TargetTitle, CsGuildManager.Instance.MissionCompletedCount, CsGuildManager.Instance.GuildMissionQuest.LimitCount);
                trImageComplete.gameObject.SetActive(false);
                break;

            case EnQuestType.GuildSupplySupport:
                textQuestName.text = CsGuildManager.Instance.GuildSupplySupportQuest.Name;
                trImageComplete.gameObject.SetActive(false);
                break;

            case EnQuestType.GuildHunting:
                textQuestName.text = CsGuildManager.Instance.GuildHuntingQuestObjective.TargetTitle;

                if (CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Accepted)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else if (CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Competed)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                break;

            case EnQuestType.GuildFarm:
                textQuestName.text = CsGameData.Instance.GuildFarmQuest.Name;

                if (CsGuildManager.Instance.GuildFarmQuestState == EnGuildFarmQuestState.Accepted)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                else if (CsGuildManager.Instance.GuildFarmQuestState == EnGuildFarmQuestState.Competed)
                {
                    trImageComplete.gameObject.SetActive(false);
                }
                break;

            case EnQuestType.GuildAlter:
                break;

            case EnQuestType.GuildAlterDefence:
                textQuestName.text = CsConfiguration.Instance.GetString("A68_TXT_00003");
                trImageComplete.gameObject.SetActive(false);
                break;

			case EnQuestType.SubQuest:
				CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(nParam);

				textQuestName.text = csHeroSubQuest.SubQuest.Title;

				if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Completion)
				{
					trImageComplete.gameObject.SetActive(true);
				}
				else
				{
					trImageComplete.gameObject.SetActive(false);
				}

				break;

			case EnQuestType.Biography:
				CsBiography csBiography = CsGameData.Instance.GetBiography(nParam);
				CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(nParam);
				
				CsBiographyQuest csBiographyQuest = null;

				if (csHeroBiographyQuest != null)
				{
					if (csHeroBiographyQuest.Completed)
					{
						csBiographyQuest = csBiography.GetBiographyQuest(csHeroBiographyQuest.QuestNo + 1);
					}
					else
					{
						csBiographyQuest = csHeroBiographyQuest.BiographyQuest;
					}
					
				}

				if (csBiographyQuest != null)
				{
					textQuestName.text = string.Format(csBiographyQuest.TargetTitle, csBiography.Name, csBiographyQuest.QuestNo);
					trImageComplete.gameObject.SetActive(!csHeroBiographyQuest.Completed && csHeroBiographyQuest.Excuted);
				}
				break;

            case EnQuestType.JobChange:

                textQuestName.text = CsConfiguration.Instance.GetString("A38_BTN_00002");

                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
                {

                }
                else
                {
                    CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

                    if (csHeroJobChangeQuest == null)
                    {
                        // 퀘스트 수락 가능
                    }
                    else
                    {
                        CsJobChangeQuest csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);

                        if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
                        {
                            // 퀘스트 완료
                        }
                        else
                        {
                            switch ((EnJobChangeQuestStaus)csHeroJobChangeQuest.Status)
                            {
                                case EnJobChangeQuestStaus.Accepted:

                                    if (csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
                                    {
                                        trImageComplete.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        trImageComplete.gameObject.SetActive(true);
                                    }

                                    break;

                                case EnJobChangeQuestStaus.Completed:

                                    trImageComplete.gameObject.SetActive(false);

                                    break;

                                case EnJobChangeQuestStaus.Failed:

                                    trImageComplete.gameObject.SetActive(false);

                                    break;
                            }
                        }
                    }
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayChangedSelectAllToggle(EnQuestCategoryType enQuestCartegoryType, bool bIsOn)
    {
        Transform trCategory = m_trContent.Find("Cartogory" + (int)enQuestCartegoryType);
        Toggle toggleAllSelect = trCategory.Find("ImageBack/ToggleAllSelect").GetComponent<Toggle>();

        toggleAllSelect.onValueChanged.RemoveAllListeners();
        toggleAllSelect.isOn = bIsOn;
        toggleAllSelect.onValueChanged.AddListener((ison) => OnValueChangedAllSelect(toggleAllSelect, enQuestCartegoryType));
        toggleAllSelect.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDisplayToggle(EnQuestCategoryType enQuestCartegoryType)
    {
        Transform trCategory = m_trContent.Find("Cartogory" + (int)enQuestCartegoryType);
        //Toggle toggleAllSelect = trCategory.Find("ImageBack/ToggleAllSelect").GetComponent<Toggle>();

        Transform trCategoryQuestList = trCategory.Find("QuestList");
        bool bAllOff = true;

        for (int i = 0; i < trCategoryQuestList.childCount; i++)
        {
            if (trCategoryQuestList.GetChild(i).Find("ToggleSwitch").GetComponent<Toggle>().isOn)
            {
                bAllOff = false;
                break;
            }
            else
            {
                continue;
            }
        }

        if (bAllOff)
        {
            DisplayChangedSelectAllToggle(enQuestCartegoryType, false);
        }
        else
        {
            DisplayChangedSelectAllToggle(enQuestCartegoryType, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateQuestCartegory(EnQuestType enQuestType, EnQuestCategoryType enQuestCartegoryType, int nSubQuestId = 0)
    {
        Transform trCategory = m_trContent.Find("Cartogory" + (int)enQuestCartegoryType);

        if (trCategory == null)
        {
            return;
        }
        else
        {
			string strName = enQuestType == EnQuestType.SubQuest ? enQuestType + nSubQuestId.ToString() : enQuestType.ToString();

            Transform trCategoryQuestList = trCategory.Find("QuestList");
			Transform trCategoryQuest = trCategoryQuestList.Find("ToggleQuest" + strName);

            if (trCategoryQuest == null)
            {
                return;
            }
            else
            {
                trCategoryQuest.gameObject.SetActive(false);

                bool bAllOff = true;

                for (int i = 0; i < trCategoryQuestList.childCount; i++)
                {
                    if (trCategoryQuestList.GetChild(i).gameObject.activeSelf == true)
                    {
                        bAllOff = false;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (bAllOff)
                {
                    trCategory.gameObject.SetActive(false);
                }
                else
                {
                    trCategory.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDefaultSelect()
    {
        Transform trCategoryQuestList = m_trContent.Find("Cartogory" + (int)EnQuestCategoryType.MainQuest + "/QuestList");
        Toggle toggleMainQuest = trCategoryQuestList.Find("ToggleQuest" + EnQuestType.MainQuest.ToString()).GetComponent<Toggle>();

        toggleMainQuest.isOn = true;

		for (int i = 0; i < m_trContent.childCount; i++)
		{
			Transform trQuestList = m_trContent.GetChild(i).Find("QuestList");

			for (int j = 0; j < trQuestList.childCount; j++)
			{
				Toggle toggle = trQuestList.GetChild(j).GetComponent<Toggle>();

				if (toggle != null && toggle.name != toggleMainQuest.name)
				{
					toggle.isOn = false;
				}
			}
		}
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleQuestName(EnQuestCategoryType enQuestCartegoryType, EnQuestType enQuestType, string strQuestName, int nSubQuestId = 0)
    {
		string strName = enQuestType == EnQuestType.SubQuest ? enQuestType + nSubQuestId.ToString() : enQuestType.ToString();

        Transform trCartegory = m_trContent.Find("Cartogory" + (int)enQuestCartegoryType);

        Transform trCartegoryQuestList = trCartegory.Find("QuestList");
		Transform trCartegoryQeust = trCartegoryQuestList.Find("ToggleQuest" + strName);

        Text textQuestName = trCartegoryQeust.Find("TextQuestName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textQuestName);
        textQuestName.text = strQuestName;
    }
}