using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public enum EnDailyQuestType
{
    Monster = 1, 
    Collect = 2, 
}

public class CsPopupDailyQuest : CsPopupSub 
{
    Transform m_trQuestList;

    Button m_buttonRefreshMaxGrade;
    Button m_buttonRefresh;

    Text m_textVip;
    Text m_textVipGuage;
    Text m_textVipDiscription;
    Text m_textDailyQuestAcceptCount;

    Slider m_sliderVipGuage;

    float m_flTime = 0.0f;

    IEnumerator m_iEnumeratorRefreshMaxGrade;
    
    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;

        CsDailyQuestManager.Instance.EventDailyQuestAbandon += OnEventDailyQuestAbandon;
        CsDailyQuestManager.Instance.EventDailyQuestAccept += OnEventDailyQuestAccept;
        CsDailyQuestManager.Instance.EventDailyQuestComplete += OnEventDailyQuestComplete;
        CsDailyQuestManager.Instance.EventDailyQuestMissionImmediatlyComplete += OnEventDailyQuestMissionImmediatlyComplete;
        CsDailyQuestManager.Instance.EventDailyQuestRefresh += OnEventDailyQuestRefresh;
        CsDailyQuestManager.Instance.EventHeroDailyQuestCreated += OnEventHeroDailyQuestCreated;
        CsDailyQuestManager.Instance.EventHeroDailyQuestProgressCountUpdated += OnEventHeroDailyQuestProgressCountUpdated;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // Start
    }

    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            UpdateDailyQuestTimer();
            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        // Destroy
        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;

        CsDailyQuestManager.Instance.EventDailyQuestAbandon -= OnEventDailyQuestAbandon;
        CsDailyQuestManager.Instance.EventDailyQuestAccept -= OnEventDailyQuestAccept;
        CsDailyQuestManager.Instance.EventDailyQuestComplete -= OnEventDailyQuestComplete;
        CsDailyQuestManager.Instance.EventDailyQuestMissionImmediatlyComplete -= OnEventDailyQuestMissionImmediatlyComplete;
        CsDailyQuestManager.Instance.EventDailyQuestRefresh -= OnEventDailyQuestRefresh;
        CsDailyQuestManager.Instance.EventHeroDailyQuestCreated -= OnEventHeroDailyQuestCreated;
        CsDailyQuestManager.Instance.EventHeroDailyQuestProgressCountUpdated -= OnEventHeroDailyQuestProgressCountUpdated;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    // 날짜 변경
    void OnEventDateChanged()
    {
        UpdateDailyQuestAcceptCount();
        UpdateButtonRefresh();
    }

    //---------------------------------------------------------------------------------------------------
    // 퀘스트 포기
    void OnEventDailyQuestAbandon(int nSlotIndex)
    {
        UpdateDailyQuest();
        UpdateButtonRefresh();
        UpdateButtonRefreshMaxGrade();
    }

    //---------------------------------------------------------------------------------------------------
    // 퀘스트 수락
    void OnEventDailyQuestAccept()
    {
        UpdateDailyQuest();
        UpdateButtonRefresh();
        UpdateButtonRefreshMaxGrade();
        UpdateDailyQuestAcceptCount();

        if (m_iEnumeratorRefreshMaxGrade != null)
        {
            StopCoroutine(m_iEnumeratorRefreshMaxGrade);
            m_iEnumeratorRefreshMaxGrade = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 퀘스트 완료
    void OnEventDailyQuestComplete(bool bLevelUp, long lAcquiredExp, int nSlotIndex)
    {
        UpdateDailyQuest();
        UpdateButtonRefresh();
        UpdateButtonRefreshMaxGrade();
        UpdateVipGuage();
    }

    //---------------------------------------------------------------------------------------------------
    // 퀘스트 즉시 완료
    void OnEventDailyQuestMissionImmediatlyComplete()
    {
        UpdateDailyQuest();
        UpdateButtonRefresh();
        UpdateButtonRefreshMaxGrade();
    }

    //---------------------------------------------------------------------------------------------------
    // 퀘스트 갱신
    void OnEventDailyQuestRefresh()
    {
        UpdateDailyQuest();
        UpdateButtonRefresh();
    }

    //---------------------------------------------------------------------------------------------------
    // 서버 이벤트 : 일일 퀘스트 생성
    void OnEventHeroDailyQuestCreated()
    {
        UpdateDailyQuest();
    }

    //---------------------------------------------------------------------------------------------------
    // 서버 이벤트 : 일일 퀘스트 progress count 변경
    void OnEventHeroDailyQuestProgressCountUpdated()
    {
        UpdateDailyQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSearchVipinfo()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Vip, EnSubMenu.VipInfo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRefreshMaxGrade()
    {
        if (m_iEnumeratorRefreshMaxGrade != null)
        {
            StopCoroutine(m_iEnumeratorRefreshMaxGrade);
            m_iEnumeratorRefreshMaxGrade = null;
        }

        m_iEnumeratorRefreshMaxGrade = RefreshMaxGrade();
        StartCoroutine(m_iEnumeratorRefreshMaxGrade);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRefresh()
    {
        // 무료 갱신
        if (CsDailyQuestManager.Instance.DailyQuestFreeRefreshCount < CsGameData.Instance.DailyQuest.FreeRefreshCount)
        {
            CheckMaxGrade();
        }
        else
        {
            // 유료 갱신
            if (CsGameData.Instance.DailyQuest.RefreshRequiredGold <= CsGameData.Instance.MyHeroInfo.Gold)
            {
                CheckMaxGrade();
            }
            else
            {
                // 돈 부족
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString(""));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDailyQuest(CsHeroDailyQuest csHeroDailyQuest)
    {
        if (csHeroDailyQuest.IsAccepted)
        {
            if (csHeroDailyQuest.Completed)
            {
                List<CsItemReward> listCsItemReward = new List<CsItemReward>();
                listCsItemReward.Add(csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.ItemReward);

                if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listCsItemReward))
                {
                    CsDailyQuestManager.Instance.SendDailyQuestComplete(csHeroDailyQuest.Id);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A12_TXT_02008"));
                }
            }
            else
            {
                System.TimeSpan tsAutoCompletionRemainingTime = System.TimeSpan.FromSeconds(csHeroDailyQuest.AutoCompletionRemainingTime - Time.realtimeSinceStartup);
                int nGoldValue = csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.ImmediateCompletionRequiredGold * tsAutoCompletionRemainingTime.Minutes / csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AutoCompletionRequiredTime;
                
                if (nGoldValue <= CsGameData.Instance.MyHeroInfo.Gold)
                {
                    CsDailyQuestManager.Instance.SendDailyQuestMissionImmediatlyComplete(csHeroDailyQuest.Id);
                }
                else
                {
                    // 돈 부족
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A101_TXT_03001"));
                }
            }
        }
        else
        {
            if (CsDailyQuestManager.Instance.DailyQuestAcceptionCount < CsGameData.Instance.DailyQuest.PlayCount)
            {
                CsDailyQuestManager.Instance.SendDailyQuestAccept(csHeroDailyQuest.Id);
            }
            else
            {
                // 일일 퀘스트 모두 수행
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A101_TXT_03002"));
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trVipInfo = transform.Find("ImageTopBackground/VipInfo");

        m_textVip = trVipInfo.Find("TextVip").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textVip);

        m_sliderVipGuage = trVipInfo.Find("SliderVipGuage").GetComponent<Slider>();

        m_textVipGuage = m_sliderVipGuage.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textVipGuage);

        m_textVipDiscription = trVipInfo.Find("ImageDia/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textVipDiscription);

        UpdateVipGuage();

        Button buttonSearch = transform.Find("ImageTopBackground/ButtonSearch").GetComponent<Button>();
        buttonSearch.onClick.RemoveAllListeners();
        buttonSearch.onClick.AddListener(OnClickSearchVipinfo);

        Text textSearch = buttonSearch.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSearch);
        textSearch.text = CsConfiguration.Instance.GetString("A101_BTN_00001");

        m_textDailyQuestAcceptCount = transform.Find("TextDailyQuestAcceptCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDailyQuestAcceptCount);

        UpdateDailyQuestAcceptCount();

        m_buttonRefreshMaxGrade = transform.Find("ButtonRefreshMaxGrade").GetComponent<Button>();
        m_buttonRefreshMaxGrade.onClick.RemoveAllListeners();
        m_buttonRefreshMaxGrade.onClick.AddListener(() => OnClickRefreshMaxGrade());

        Text textRefreshMaxGrade = m_buttonRefreshMaxGrade.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRefreshMaxGrade);
        textRefreshMaxGrade.text = CsConfiguration.Instance.GetString("A101_BTN_00002");

        UpdateButtonRefreshMaxGrade();

        m_buttonRefresh = transform.Find("ButtonRefresh").GetComponent<Button>();
        m_buttonRefresh.onClick.RemoveAllListeners();
        m_buttonRefresh.onClick.AddListener(OnClickRefresh);

        UpdateButtonRefresh();

        m_trQuestList = transform.Find("QuestList");
        UpdateDailyQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDailyQuest()
    {
        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[i];

            if (csHeroDailyQuest == null)
            {
                continue;
            }
            else
            {
                Transform trImageBackground = m_trQuestList.Find("ImageBackground" + i);

                Image imageRank = trImageBackground.Find("ImageRank").GetComponent<Image>();
                imageRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_achievement_rank0" + csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.Grade);

                Text textTitle = trImageBackground.Find("TextTitle").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTitle);
                textTitle.text = csHeroDailyQuest.DailyQuestMission.Title;

                Text textTarget = trImageBackground.Find("ImageTarget/Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTarget);

                if (csHeroDailyQuest.DailyQuestMission.Type == 1)
                {
                    textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                }
                else if (csHeroDailyQuest.DailyQuestMission.Type == 2)
                {
                    textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                }
                
                Text textReward = trImageBackground.Find("TextReward").GetComponent<Text>();
                CsUIData.Instance.SetFont(textReward);
                textReward.text = CsConfiguration.Instance.GetString("A101_BTN_00005");

                Transform trRewardList = trImageBackground.Find("RewardList");

                Transform trItemSlot01 = trRewardList.Find("ItemSlot01");
                Transform trItemSlot02 = trRewardList.Find("ItemSlot02");

                CsItemReward csAvailableItemReward1 = csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AvailableItemReward1;
                CsUIData.Instance.DisplayItemSlot(trItemSlot01, csAvailableItemReward1.Item, csAvailableItemReward1.ItemOwned, csAvailableItemReward1.ItemCount, csAvailableItemReward1.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                CsItemReward csAvailableItemReward2 = csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AvailableItemReward2;
                CsUIData.Instance.DisplayItemSlot(trItemSlot02, csAvailableItemReward2.Item, csAvailableItemReward2.ItemOwned, csAvailableItemReward2.ItemCount, csAvailableItemReward2.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                Transform trImageTimerback = trImageBackground.Find("ImageTimerback");

                Text textTimer = trImageTimerback.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTimer);
                System.TimeSpan tsAutoCompletionRemainingTime = new System.TimeSpan();

                Button buttonDailyQuest = trImageBackground.Find("Button").GetComponent<Button>();
                buttonDailyQuest.onClick.RemoveAllListeners();
                buttonDailyQuest.onClick.AddListener(() => OnClickDailyQuest(csHeroDailyQuest));

                Image imageButtonDailyQuest = buttonDailyQuest.transform.GetComponent<Image>();

                Text textButton = buttonDailyQuest.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButton);

                Transform trImmediateCompleteFrame = buttonDailyQuest.transform.Find("ImmediateCompleteFrame");

                if (csHeroDailyQuest.IsAccepted)
                {
                    // 진행 중인 일일 퀘스트 완료
                    if (csHeroDailyQuest.Completed)
                    {
                        //
                        textTarget.text = CsConfiguration.Instance.GetString("A101_TXT_00001");

                        // 타이머
                        trImageTimerback.gameObject.SetActive(false);
                        
                        // 버튼 이미지
                        imageButtonDailyQuest.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_02");

                        // 버튼 텍스트
                        trImmediateCompleteFrame.gameObject.SetActive(false);
                        textButton.text = CsConfiguration.Instance.GetString("A101_BTN_00008");
                        textButton.gameObject.SetActive(true);
                    }
                    // 일일 퀘스트 진행중
                    else
                    {
                        // 타이머
                        trImageTimerback.gameObject.SetActive(true);
                        tsAutoCompletionRemainingTime = System.TimeSpan.FromSeconds(csHeroDailyQuest.AutoCompletionRemainingTime - Time.realtimeSinceStartup);
                        textTimer.text = string.Format(CsConfiguration.Instance.GetString("A101_TXT_00003"), (tsAutoCompletionRemainingTime.Minutes + 1).ToString("#0"));
                        textTimer.color = new Color32(35, 216, 128, 255);

                        // 버튼 이미지
                        imageButtonDailyQuest.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_01");

                        // 버튼 텍스트
                        Text textImmediateComplete = trImmediateCompleteFrame.Find("Text").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textImmediateComplete);
                        textImmediateComplete.text = CsConfiguration.Instance.GetString("A101_BTN_00007");

                        Text textGoldValue = trImmediateCompleteFrame.Find("TextValue").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textGoldValue);
                        int nGoldValue = csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.ImmediateCompletionRequiredGold * (tsAutoCompletionRemainingTime.Minutes + 1) / csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AutoCompletionRequiredTime;
                        textGoldValue.text = nGoldValue.ToString("#,##0");

                        textButton.gameObject.SetActive(false);
                        trImmediateCompleteFrame.gameObject.SetActive(true);
                    }
                }
                // 수락도 받지 않음
                else
                {
                    // 타이머
                    trImageTimerback.gameObject.SetActive(true);
                    tsAutoCompletionRemainingTime = System.TimeSpan.FromMinutes(csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AutoCompletionRequiredTime);
                    textTimer.text = string.Format(CsConfiguration.Instance.GetString("A101_TXT_00002"), tsAutoCompletionRemainingTime.Minutes.ToString("#0"));
                    textTimer.color = CsUIData.Instance.ColorWhite;

                    imageButtonDailyQuest.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_03");

                    trImmediateCompleteFrame.gameObject.SetActive(false);
                    textButton.text = CsConfiguration.Instance.GetString("A101_BTN_00006");
                    textButton.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDailyQuestTimer()
    {
        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[i];

            Transform trImageBackground = m_trQuestList.Find("ImageBackground" + i);
            Transform trImageTimerback = trImageBackground.Find("ImageTimerback");

            Text textGoldValue = trImageBackground.Find("Button/ImmediateCompleteFrame/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGoldValue);

            if (csHeroDailyQuest == null)
            {
                continue;
            }
            else
            {
                if (csHeroDailyQuest.IsAccepted)
                {
                    // 완료된 퀘스트 시간 표시 X
                    if (csHeroDailyQuest.Completed)
                    {
                        UpdateDailyQuest();
                    }
                    // 진행중인 퀘스트
                    else
                    {
                        Text textTimer = trImageTimerback.Find("Text").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textTimer);

                        System.TimeSpan tsAutoCompletionRemainingTime = System.TimeSpan.FromSeconds(csHeroDailyQuest.AutoCompletionRemainingTime - Time.realtimeSinceStartup);

                        if (tsAutoCompletionRemainingTime.Minutes < 0)
                        {
                            textTimer.text = "";
                            textGoldValue.text = "";
                        }
                        else
                        {
                            textTimer.text = string.Format(CsConfiguration.Instance.GetString("A101_TXT_00003"), (tsAutoCompletionRemainingTime.Minutes + 1).ToString("#0"));

                            int nGoldValue = csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.ImmediateCompletionRequiredGold * (tsAutoCompletionRemainingTime.Minutes + 1) / csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AutoCompletionRequiredTime;
                            textGoldValue.text = nGoldValue.ToString("#,##0");
                        }
                    }
                }
                // 수락 받지 않은 퀘스트
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDailyQuestAcceptCount()
    {
        m_textDailyQuestAcceptCount.text = string.Format(CsConfiguration.Instance.GetString("A101_TXT_01001"), CsGameData.Instance.DailyQuest.PlayCount - CsDailyQuestManager.Instance.DailyQuestAcceptionCount, CsGameData.Instance.DailyQuest.PlayCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateVipGuage()
    {
        int nHeroVipLevel = CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel;
        int nHeroVipPoint = CsGameData.Instance.MyHeroInfo.VipPoint;
        int nHeroVipNextPoint = 0;

        CsVipLevel csVipLevelNext = CsGameData.Instance.GetVipLevel(nHeroVipLevel + 1);

        if (csVipLevelNext == null)
        {
            nHeroVipNextPoint = CsGameData.Instance.GetVipLevel(nHeroVipLevel).RequiredAccVipPoint;
        }
        else
        {
            nHeroVipNextPoint = CsGameData.Instance.GetVipLevel(nHeroVipLevel + 1).RequiredAccVipPoint;
        }

        if (nHeroVipNextPoint < nHeroVipPoint)
        {
            nHeroVipPoint = nHeroVipNextPoint;
        }

        m_textVip.text = string.Format(CsConfiguration.Instance.GetString("A54_TXT_01002"), nHeroVipLevel);

        m_sliderVipGuage.value = (float)nHeroVipPoint / (float)nHeroVipNextPoint;
        m_textVipGuage.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroVipPoint, nHeroVipNextPoint);

        m_textVipDiscription.text = string.Format(CsConfiguration.Instance.GetString("A54_TXT_01003"), (nHeroVipNextPoint - nHeroVipPoint), (nHeroVipLevel + 1));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonRefreshMaxGrade()
    {
        if (CsDailyQuestManager.Instance.DailyQuestAcceptionCount < CsGameData.Instance.DailyQuest.PlayCount)
        {
            // 수락 받을 수 있는 상태
            bool bAllQuestAccept = true;

            for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
            {
                CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[i];

                if (csHeroDailyQuest == null)
                {
                    return;
                }
                else
                {
                    if (!csHeroDailyQuest.IsAccepted)
                    {
                        bAllQuestAccept = false;
                        break;
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonRefreshMaxGrade, !bAllQuestAccept);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonRefreshMaxGrade, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonRefresh()
    {
        Transform trRefreshFree = m_buttonRefresh.transform.Find("RefreshFreeFrame");
        Transform trRefreshCharge = m_buttonRefresh.transform.Find("RefreshChargeFrame");

        int nRemainingRefreshFreeCount = CsGameData.Instance.DailyQuest.FreeRefreshCount - CsDailyQuestManager.Instance.DailyQuestFreeRefreshCount;
        
        // 무료 갱신
        if (0 < nRemainingRefreshFreeCount)
        {
            Text textRefreshFreeCount = trRefreshFree.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRefreshFreeCount);
            textRefreshFreeCount.text = string.Format(CsConfiguration.Instance.GetString("A101_BTN_00003"), nRemainingRefreshFreeCount, CsGameData.Instance.DailyQuest.FreeRefreshCount);

            trRefreshCharge.gameObject.SetActive(false);
            trRefreshFree.gameObject.SetActive(true);
        }
        // 유료 갱신
        else
        {
            if (trRefreshCharge.gameObject.activeSelf)
            {

            }
            else
            {
                Text textGoldValue = trRefreshCharge.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textGoldValue);
                textGoldValue.text = CsGameData.Instance.DailyQuest.RefreshRequiredGold.ToString("#,##0");

                Text textRefreshCharge = trRefreshCharge.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRefreshCharge);
                textRefreshCharge.text = CsConfiguration.Instance.GetString("A101_BTN_00004");

                trRefreshFree.gameObject.SetActive(false);
                trRefreshCharge.gameObject.SetActive(true);
            }
        }

        if (CsDailyQuestManager.Instance.DailyQuestAcceptionCount < CsGameData.Instance.DailyQuest.PlayCount)
        {
            bool bAllQuestAccept = true;

            for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
            {
                CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[i];

                if (csHeroDailyQuest == null)
                {
                    return;
                }
                else
                {
                    if (!csHeroDailyQuest.IsAccepted)
                    {
                        bAllQuestAccept = false;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            m_buttonRefresh.interactable = !bAllQuestAccept;
        }
        else
        {
            m_buttonRefresh.interactable = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckMaxGrade()
    {
        // 수락 받지 않고 최고 등급인 경우
        CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList.Find(a => a.IsAccepted == false && a.DailyQuestMission.DailyQuestGrade.Grade == CsGameData.Instance.DailyQuestGradeList.Count);

        if (csHeroDailyQuest == null)
        {
            CsDailyQuestManager.Instance.SendDailyQuestRefresh();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A101_TXT_00004"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsDailyQuestManager.Instance.SendDailyQuestRefresh,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator RefreshMaxGrade()
    {
        // 수락 받지 않고 최고 등급인 경우
        CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList.Find(a => a.IsAccepted == false && a.DailyQuestMission.DailyQuestGrade.Grade == CsGameData.Instance.DailyQuestGradeList.Count);

        if (csHeroDailyQuest != null)
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A101_TXT_00004"),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsDailyQuestManager.Instance.SendDailyQuestRefresh,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
        else
        {
            while (csHeroDailyQuest == null)
            {
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList.Find(a => a.IsAccepted == false && a.DailyQuestMission.DailyQuestGrade.Grade == CsGameData.Instance.DailyQuestGradeList.Count);

                int nRemainingRefreshFreeCount = CsGameData.Instance.DailyQuest.FreeRefreshCount - CsDailyQuestManager.Instance.DailyQuestFreeRefreshCount;
				Debug.Log("## nRemainingRefreshFreeCount : " + nRemainingRefreshFreeCount);
                if (nRemainingRefreshFreeCount <= 0 && CsGameData.Instance.MyHeroInfo.Gold < CsGameData.Instance.DailyQuest.RefreshRequiredGold)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A101_TXT_03001"));
                    break;
                }
                else
                {
                    if (csHeroDailyQuest != null)
                    {
                        break;
                    }
                    else
                    {
                        CsDailyQuestManager.Instance.SendDailyQuestRefresh();
                    }   
                }

                yield return new WaitForSeconds(1.0f);
            }
        }

        if (m_iEnumeratorRefreshMaxGrade != null)
        {
            StopCoroutine(m_iEnumeratorRefreshMaxGrade);
            m_iEnumeratorRefreshMaxGrade = null;
        }
    }
}