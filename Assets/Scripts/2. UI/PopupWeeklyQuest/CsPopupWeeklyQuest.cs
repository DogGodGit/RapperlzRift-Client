using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public enum EnWeeklyQuestType
{ 
    Move = 1, 
    Monster = 2, 
    Collect = 3, 
}

public class CsPopupWeeklyQuest : CsPopupSub 
{
    [SerializeField] GameObject m_goItemSlot;
    Transform m_trImageRewardBack;

    Text m_textWeeklyQuestRoundProgressCount;
    Text m_textWeeklyQuestRound;
    Text m_textWeeklyQuestTargetCount;
    Text m_textRewardExp;
    Text m_textRewardGold;

    Button m_buttonCompleteAll;
    Button m_buttonRefresh;
    Button m_buttonAccept;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;

        CsWeeklyQuestManager.Instance.EventWeeklyQuestCreated += OnEventWeeklyQuestCreated;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundAccept += OnEventWeeklyQuestRoundAccept;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted += OnEventWeeklyQuestRoundCompleted;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete += OnEventWeeklyQuestRoundImmediatlyComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated += OnEventWeeklyQuestRoundProgressCountUpdated;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundRefresh += OnEventWeeklyQuestRoundRefresh;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestTenRoundImmediatlyComplete += OnEventWeeklyQuestTenRoundImmediatlyComplete;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // Start
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        // Destroy
        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;

        CsWeeklyQuestManager.Instance.EventWeeklyQuestCreated -= OnEventWeeklyQuestCreated;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundAccept -= OnEventWeeklyQuestRoundAccept;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted -= OnEventWeeklyQuestRoundCompleted;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete -= OnEventWeeklyQuestRoundImmediatlyComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete -= OnEventWeeklyQuestRoundMoveMissionComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated -= OnEventWeeklyQuestRoundProgressCountUpdated;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundRefresh -= OnEventWeeklyQuestRoundRefresh;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestTenRoundImmediatlyComplete -= OnEventWeeklyQuestTenRoundImmediatlyComplete;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventDateChanged()
    {
        // 월요일만 변경
        if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Monday)
        {
            UpdateWeeklyQuest();
            UpdateButtonCompleteAll();
            UpdateButtonRefresh();
            UpdateButtonAccept();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버 이벤트 : 주간 퀘스트 생성
    void OnEventWeeklyQuestCreated()
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 라운드 미션 수락
    void OnEventWeeklyQuestRoundAccept()
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 라운드 미션 완료
    void OnEventWeeklyQuestRoundCompleted(bool bLevelUp, long lAcquiredExp)
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 라운드 미션 즉시 완료
    void OnEventWeeklyQuestRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 라운드 미션 이동 퀘스트 완료
    void OnEventWeeklyQuestRoundMoveMissionComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 서버 이벤트 : 라운드 미션 진행 상황 업데이트
    void OnEventWeeklyQuestRoundProgressCountUpdated()
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 라운드 미션 갱신
    void OnEventWeeklyQuestRoundRefresh()
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    // 라운드 퀘스트 전체 완료
    void OnEventWeeklyQuestTenRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    #endregion EventHandler
    
    #region Event
    //---------------------------------------------------------------------------------------------------
    void OnClickWeeklyQuestCompleteAll()
    {
        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsGameData.Instance.WeeklyQuest.TenRoundCompletionRequiredVipLevel)
        {
            // VIP 레벨 부족
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A104_TXT_01004"));
        }
        else
        {
            int nRemainingRoundCount = 0;

            if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo % 10 == 0)
            {
                nRemainingRoundCount = 1;
            }
            else
            {
                nRemainingRoundCount = 10 - (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo % 10) + 1;
            }

            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.WeeklyQuest.RoundImmediateCompletionRequiredItem.ItemId) < nRemainingRoundCount)
            {
                // 즉시 완료 아이템 부족
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A104_TXT_01003"));
            }
            else
            {
                CsWeeklyQuestManager.Instance.SendWeeklyQuestTenRoundImmediatlyComplete();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWeeklyQuestRefresh()
    {
        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
        {
            // 퀘스트 즉시 완료
            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.WeeklyQuest.RoundImmediateCompletionRequiredItem.ItemId) == 0)
            {
                // 즉시 완료 아이템 부족
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A104_TXT_01003"));
            }
            else
            {
                // 즉시 완료
                CsWeeklyQuestManager.Instance.SendWeeklyQuestRoundImmediatlyComplete();
            }
        }
        else
        {
            // 퀘스트 갱신
            CsWeeklyQuestManager.Instance.SendWeeklyQuestRoundRefresh();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWeeklyQuestAccept()
    {
        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
        {
            // 경로 표시
            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.WeeklyQuest);
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        }
        else
        {
            // 퀘스트 수령
            CsWeeklyQuestManager.Instance.SendWeeklyQuestRoundAccept();
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trImage = transform.Find("Image");

        Text textWeeklyQuestRoundProgress = trImage.Find("TextProgress").GetComponent<Text>();
        CsUIData.Instance.SetFont(textWeeklyQuestRoundProgress);
        textWeeklyQuestRoundProgress.text = CsConfiguration.Instance.GetString("A104_TXT_00001");

        m_textWeeklyQuestRoundProgressCount = trImage.Find("TextProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWeeklyQuestRoundProgressCount);

        Text textDescription = trImage.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = string.Format(CsConfiguration.Instance.GetString("A104_TXT_01002"), CsGameData.Instance.WeeklyQuest.TenRoundCompletionRewardFactor * 100);

        Text textTenRountReward = transform.Find("TextTenRoundReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTenRountReward);
        textTenRountReward.text = CsConfiguration.Instance.GetString("A104_TXT_00002");

        m_trImageRewardBack = transform.Find("ImageRewardBack");

        Transform trImageBackground = transform.Find("ImageBackground");

        m_textWeeklyQuestRound = trImageBackground.Find("TextRound").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWeeklyQuestRound);

        m_textWeeklyQuestTargetCount = trImageBackground.Find("Image/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWeeklyQuestTargetCount);

        m_textRewardExp = trImageBackground.Find("ImageExp/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRewardExp);

        m_textRewardGold = trImageBackground.Find("ImageGold/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRewardGold);

        Text textReward = trImageBackground.Find("TextReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReward);
        textReward.text = CsConfiguration.Instance.GetString("A104_TXT_00003");

        m_buttonCompleteAll = transform.Find("ButtonCompleteAll").GetComponent<Button>();
        m_buttonCompleteAll.onClick.RemoveAllListeners();
        m_buttonCompleteAll.onClick.AddListener(OnClickWeeklyQuestCompleteAll);

        Image imageItemIcon = m_buttonCompleteAll.transform.Find("Image").GetComponent<Image>();
        imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsGameData.Instance.WeeklyQuest.RoundImmediateCompletionRequiredItem.Image);

        m_buttonRefresh = transform.Find("ButtonRefresh").GetComponent<Button>();
        m_buttonRefresh.onClick.RemoveAllListeners();
        m_buttonRefresh.onClick.AddListener(OnClickWeeklyQuestRefresh);

        m_buttonAccept = transform.Find("ButtonAccept").GetComponent<Button>();
        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickWeeklyQuestAccept);

        UpdateTenRoundReward();
        UpdateWeeklyQuest();
        UpdateButtonCompleteAll();
        UpdateButtonRefresh();
        UpdateButtonAccept();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTenRoundReward()
    {
        for (int i = 0; i < CsGameData.Instance.WeeklyQuest.WeeklyQuestTenRoundRewardList.Count; i++)
        {
            Transform trRewardItemSlot = m_trImageRewardBack.Find("RewardItemSlot" + i);

            if (trRewardItemSlot == null)
            {
                trRewardItemSlot = Instantiate(m_goItemSlot, m_trImageRewardBack).transform;
                trRewardItemSlot.name = "RewardItemSlot" + i;
            }

            CsItemReward csItemReward = CsGameData.Instance.WeeklyQuest.WeeklyQuestTenRoundRewardList[i].ItemReward;

            if (csItemReward == null)
            {
                continue;
            }
            else
            {
                CsUIData.Instance.DisplayItemSlot(trRewardItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWeeklyQuest()
    {
        m_textWeeklyQuestRoundProgressCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo - 1, CsGameData.Instance.WeeklyQuest.RoundCount);
        m_textWeeklyQuestRound.text = string.Format(CsConfiguration.Instance.GetString("A104_TXT_01001"), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo);

        CsWeeklyQuestMission csWeeklyQuestMission = CsGameData.Instance.WeeklyQuest.GetWeeklyQuestMission(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundMissionId);

        if (csWeeklyQuestMission == null)
        {
            return;
        }
        else
        {
            switch ((EnWeeklyQuestType)csWeeklyQuestMission.Type)
            {
                case EnWeeklyQuestType.Move:
                    m_textWeeklyQuestTargetCount.text = CsConfiguration.Instance.GetString(csWeeklyQuestMission.TargetContent);
                    break;

                case EnWeeklyQuestType.Monster:
                    m_textWeeklyQuestTargetCount.text = string.Format(CsConfiguration.Instance.GetString(csWeeklyQuestMission.TargetContent), csWeeklyQuestMission.TargetMonster.Name, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount, csWeeklyQuestMission.TargetCount);
                    break;

                case EnWeeklyQuestType.Collect:
                    m_textWeeklyQuestTargetCount.text = string.Format(CsConfiguration.Instance.GetString(csWeeklyQuestMission.TargetContent), csWeeklyQuestMission.TargetContinentObject.Name, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount, csWeeklyQuestMission.TargetCount);
                    break;
            }

            CsWeeklyQuestRoundReward csWeeklyQuestRoundReward = CsGameData.Instance.WeeklyQuest.GetWeeklyQuestRoundReward(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo, CsGameData.Instance.MyHeroInfo.Level);

            if (csWeeklyQuestRoundReward == null)
            {
                return;
            }
            else
            {
                m_textRewardExp.text = csWeeklyQuestRoundReward.ExpReward.Value.ToString("#,##0");
                m_textRewardGold.text = csWeeklyQuestRoundReward.GoldReward.Value.ToString("#,##0");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonCompleteAll()
    {
        int nRequiredItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.WeeklyQuest.RoundImmediateCompletionRequiredItem.ItemId);
        int nRemainingRoundCount = 0;

        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo % 10 == 0)
        {
            nRemainingRoundCount = 1;
        }
        else
        {
            nRemainingRoundCount = 10 - (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo % 10) + 1;
        }

        Text textCount = m_buttonCompleteAll.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);
        textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRequiredItemCount, nRemainingRoundCount);

        Text textCompleteAll = m_buttonCompleteAll.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCompleteAll);
        textCompleteAll.text = CsConfiguration.Instance.GetString("A104_BTN_00001");

        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo <= CsGameData.Instance.WeeklyQuest.RoundCount)
        {
            m_buttonCompleteAll.interactable = true;
        }
        else
        {
            m_buttonCompleteAll.interactable = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonRefresh()
    {
        Text textValue = m_buttonRefresh.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValue);

        Text textButton = m_buttonRefresh.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton);

        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
        {
            m_buttonRefresh.transform.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsGameData.Instance.WeeklyQuest.RoundImmediateCompletionRequiredItem.Image);

            textValue.text = "1";
            textButton.text = CsConfiguration.Instance.GetString("A104_BTN_00005");
        }
        else
        {
            m_buttonRefresh.transform.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods03");

            textValue.text = CsGameData.Instance.WeeklyQuest.RoundRefreshRequiredGold.ToString("#,##0");
            textButton.text = CsConfiguration.Instance.GetString("A104_BTN_00002");
        }

        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo <= CsGameData.Instance.WeeklyQuest.RoundCount)
        {
            m_buttonRefresh.interactable = true;
        }
        else
        {
            m_buttonRefresh.interactable = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonAccept()
    {
        Text textButton = m_buttonAccept.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton);

        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo <= CsGameData.Instance.WeeklyQuest.RoundCount)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonAccept, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonAccept, false);
        }

        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
        {
            textButton.text = CsConfiguration.Instance.GetString("A104_BTN_00004");
        }
        else
        {
            textButton.text = CsConfiguration.Instance.GetString("A104_BTN_00003");
        }
    }
}
