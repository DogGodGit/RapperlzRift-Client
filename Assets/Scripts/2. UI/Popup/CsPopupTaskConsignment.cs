using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnTaskConsignment
{
    UndergroundMaze = 1,    // 지하미로
    aaa = 2,                // 크리쳐 농장 n/a
    GuildHunting = 3,       // 길드 현상금
    GuildMission = 4,       // 길드 미션
    bbb = 5,                // 프로즌 전쟁 n/a
    ccc = 6,                // 전쟁신의 사명
}

public class CsPopupTaskConsignment : MonoBehaviour 
{
    GameObject m_goTaskConsignmentItem;
    GameObject m_goPopupCalculator;

    Transform m_trImageBackground;
    Transform m_trContent;
    Transform m_trCalculator;
    Transform m_trPopupList;
    Transform m_trPopupUseExpItem;

    Text m_textGoldValue;
    Text m_textTaskConsignmentValue;

    CsPopupCalculator m_csPopupCalculator;

    const int m_cnReduceGold = 5000;

    float m_flTime = 0.0f;
    
    //---------------------------------------------------------------------------------------------------
	void Awake ()
    {
        CsGameEventUIToUI.Instance.EventTaskConsignmentStart += OnEventTaskConsignmentStart;
        CsGameEventUIToUI.Instance.EventTaskConsignmentImmediatelyComplete += OnEventTaskConsignmentImmediatelyComplete;
        CsGameEventUIToUI.Instance.EventTaskConsignmentComplete += OnEventTaskConsignmentComplete;

        InitializeUI();
	}

    //---------------------------------------------------------------------------------------------------
	void Update () 
    {
        if (m_flTime + 1.0f < Time.time)
        {
            DisplayTaskConsignmentItems();
            m_flTime = Time.time;
        }
	}

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventTaskConsignmentStart -= OnEventTaskConsignmentStart;
        CsGameEventUIToUI.Instance.EventTaskConsignmentImmediatelyComplete -= OnEventTaskConsignmentImmediatelyComplete;
        CsGameEventUIToUI.Instance.EventTaskConsignmentComplete -= OnEventTaskConsignmentComplete;
    }

    #region Event

    // 위탁 시작
    //---------------------------------------------------------------------------------------------------
    void OnEventTaskConsignmentStart()
    {
        UpdateGoodsValue();
        DisplayTaskConsignmentItems();
    }

    // 위탁 즉시 완료
    //---------------------------------------------------------------------------------------------------
    void OnEventTaskConsignmentImmediatelyComplete(bool bLevelUp, long lExpAcq)
    {
        UpdateGoodsValue();
        DisplayTaskConsignmentItems();
    }

    // 위탁 완료
    //---------------------------------------------------------------------------------------------------
    void OnEventTaskConsignmentComplete(bool bLevelUp, long lExpAcq)
    {
        UpdateGoodsValue();
        DisplayTaskConsignmentItems();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTaskConsignment(int nTaskConsignmentId, int nUseExpItemId = 0)
    {
        // 즉시 완료 or 위탁 시작
        CsHeroTaskConsignment csHeroTaskConsignment = CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignment(nTaskConsignmentId);

        if (csHeroTaskConsignment == null)
        {
            CsTaskConsignment csTaskConsignment = CsGameData.Instance.GetTaskConsignment(nTaskConsignmentId);

            if (CsGameData.Instance.MyHeroInfo.GetItemCount(csTaskConsignment.RequiredItem.ItemId) < csTaskConsignment.RequiredItemCount)
            {
                // 위탁령 구매 팝업
                // 준비중
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
            }
            else
            {
                if ((EnTaskConsignment)nTaskConsignmentId == EnTaskConsignment.UndergroundMaze)
                {
                    CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A109_TXT_00007"),
                        CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OpenPopupUseExpScroll,
                        CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), () => CsCommandEventManager.Instance.SendTaskConsignmentStart(nTaskConsignmentId, nUseExpItemId), true);
                }
                else
                {
                    // 위탁 시작
                    CsCommandEventManager.Instance.SendTaskConsignmentStart(nTaskConsignmentId, nUseExpItemId);
                }
            }
        }
        else
        {
            // 즉시 완료
            CsTaskConsignment csTaskConsignment = CsGameData.Instance.GetTaskConsignment(csHeroTaskConsignment.ConsignmentId);

            if (csTaskConsignment == null)
            {
                return; 
            }
            else
            {
                System.TimeSpan tsTaskConsignmentRemainingTime = System.TimeSpan.FromSeconds(csHeroTaskConsignment.RemainingTime - Time.realtimeSinceStartup);  // 남은 시간
                int nImmediateGoldValue = 0;

                // 경험치 스크롤에 따른 골드 표시 여부
                if ((EnTaskConsignment)csTaskConsignment.ConsignmentId == EnTaskConsignment.UndergroundMaze && csHeroTaskConsignment.UsedExpItemId != 0)
                {
                    nImmediateGoldValue = csTaskConsignment.ImmediateCompletionRequiredGold;
                }
                else
                {
                    nImmediateGoldValue = csTaskConsignment.ImmediateCompletionRequiredGold - ((csTaskConsignment.CompletionRequiredTime - (int)tsTaskConsignmentRemainingTime.TotalSeconds) / csTaskConsignment.ImmediateCompletionRequiredGoldReduceInterval * m_cnReduceGold);
                }
                
                if (CsGameData.Instance.MyHeroInfo.Gold < nImmediateGoldValue)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A109_TXT_00012"));
                }
                else
                {
                    if (AvailableReward(csHeroTaskConsignment))
                    {
                        CsCommandEventManager.Instance.SendTaskConsignmentImmediatelyComplete(csHeroTaskConsignment.InstanceId);
                    }
                    else
                    {
                        //실패 토스트 출력
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A109_TXT_00013"));
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReward(CsHeroTaskConsignment csHeroTaskConsignment)
    {
        if (AvailableReward(csHeroTaskConsignment))
        {
            CsCommandEventManager.Instance.SendTaskConsignmentComplete(csHeroTaskConsignment.InstanceId);
        }
        else
        {
            //실패 토스트 출력
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A109_TXT_00013"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildApply()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExpScrollUse(int nItemId)
    {
        CsCommandEventManager.Instance.SendTaskConsignmentStart((int)EnTaskConsignment.UndergroundMaze, nItemId);
        m_trPopupUseExpItem.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        m_trPopupUseExpItem.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        PopupClose();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsGameConfig.Instance.TaskConsignmentRequiredVipLevel)
        {
            PopupClose();
        }
        else
        {
            Transform trCanvas2 = GameObject.Find("Canvas2").transform;
            m_trPopupList = trCanvas2.Find("PopupList");

            m_trImageBackground = transform.Find("ImageBackground");

            Text textPopupName = m_trImageBackground.Find("TextPopupName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPopupName);
            textPopupName.text = CsConfiguration.Instance.GetString("A109_TXT_00001");

            Button buttonClose = m_trImageBackground.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(OnClickClose);
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            m_textGoldValue = m_trImageBackground.Find("ImageGold/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textGoldValue);

            Image imageTaskConsignmentRequiredItemIcon = m_trImageBackground.Find("ImageConsignment/Image").GetComponent<Image>();

            if (CsGameData.Instance.TaskConsignmentList[0] == null)
            {
                PopupClose();
            }
            else
            {
                imageTaskConsignmentRequiredItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsGameData.Instance.TaskConsignmentList[0].RequiredItem.Image);
            }

            m_textTaskConsignmentValue = m_trImageBackground.Find("ImageConsignment/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textTaskConsignmentValue);

            UpdateGoodsValue();

            m_trContent = m_trImageBackground.Find("Scroll View/Viewport/Content");

            DisplayTaskConsignmentItems();

            m_trPopupUseExpItem = transform.Find("PopupUseExpScroll");
            m_trPopupUseExpItem.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGoodsValue()
    {
        long lGoldValue = CsGameData.Instance.MyHeroInfo.Gold;
        CsUIData.Instance.SetFont(m_textGoldValue);
        m_textGoldValue.text = lGoldValue.ToString("#,##0");

        int nItemId = 0;

        if (CsGameData.Instance.TaskConsignmentList[0] == null)
        {
            return;
        }
        else
        {
            nItemId = CsGameData.Instance.TaskConsignmentList[0].RequiredItem.ItemId;
        }

        int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId);
        CsUIData.Instance.SetFont(m_textTaskConsignmentValue);
        m_textTaskConsignmentValue.text = nItemCount.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTaskConsignmentItems()
    {
        if (m_goTaskConsignmentItem == null)
        {
            StartCoroutine(LoadTaskConsignmentItem());
        }
        else
        {
            UpdateTaskConsignments();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadTaskConsignmentItem()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupTaskConsignment/TaskConsignmentItem");
        yield return resourceRequest;

        m_goTaskConsignmentItem = (GameObject)resourceRequest.asset;

        UpdateTaskConsignments();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTaskConsignments()
    {
        Transform trTaskConsignmentItem = null;

        for (int i = 0; i < CsGameData.Instance.TaskConsignmentList.Count; i++)
        {
            CsTaskConsignment csTaskConsignment = CsGameData.Instance.TaskConsignmentList[i];

            if (csTaskConsignment == null)
            {
                continue;
            }
            else
            {
                trTaskConsignmentItem = m_trContent.Find("TaskConsignmentItem" + i);

                if (trTaskConsignmentItem == null)
                {
                    trTaskConsignmentItem = Instantiate(m_goTaskConsignmentItem, m_trContent).transform;
                    trTaskConsignmentItem.name = "TaskConsignmentItem" + i;
                }

                // 아이콘 이미지
                Image imageTaskIcon = trTaskConsignmentItem.Find("Image").GetComponent<Image>();
                imageTaskIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + csTaskConsignment.TodayTask.TaskId);

                Transform trTaskConsignmentInfo = trTaskConsignmentItem.Find("TaskConsignmentInfo");

                Text textTaskConsignmentName = trTaskConsignmentInfo.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTaskConsignmentName);
                textTaskConsignmentName.text = csTaskConsignment.TodayTask.Name;

                Text textProgressCount = trTaskConsignmentInfo.Find("TextProgressCount").GetComponent<Text>();
                CsUIData.Instance.SetFont(textProgressCount);

                Transform trTaskConsignmentImmediateInfo = trTaskConsignmentInfo.Find("TaskConsignImmediateInfo");

                Text textRemainingTimeMinute = trTaskConsignmentImmediateInfo.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRemainingTimeMinute);

                Text textImmediateGoldValue = trTaskConsignmentImmediateInfo.Find("ImageGold/TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textImmediateGoldValue);

                Slider sliderTimer = trTaskConsignmentInfo.Find("Slider").GetComponent<Slider>();
                sliderTimer.maxValue = CsGameData.Instance.TaskConsignmentList[i].CompletionRequiredTime;

                Text textSliderTimer = sliderTimer.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textSliderTimer);

                Text textTaskConsignmentState = trTaskConsignmentItem.Find("TextState").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTaskConsignmentState);

                Button buttonGuildApply = trTaskConsignmentItem.Find("ButtonGuildApply").GetComponent<Button>();
                buttonGuildApply.onClick.RemoveAllListeners();
                buttonGuildApply.onClick.AddListener(OnClickGuildApply);
                buttonGuildApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textGuildApply = buttonGuildApply.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textGuildApply);
                textGuildApply.text = CsConfiguration.Instance.GetString("A50_BTN_00002");

                Button buttonTaskConsignment = trTaskConsignmentItem.Find("ButtonTaskConsignment").GetComponent<Button>();
                buttonTaskConsignment.onClick.RemoveAllListeners();
                buttonTaskConsignment.onClick.AddListener(() => OnClickTaskConsignment(csTaskConsignment.ConsignmentId));
                buttonTaskConsignment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Image imageGoods = buttonTaskConsignment.transform.Find("Image").GetComponent<Image>();

                Text textValue = buttonTaskConsignment.transform.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textValue);

                Text textTaskConsignment = buttonTaskConsignment.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTaskConsignment);

                Button buttonReward = trTaskConsignmentItem.Find("ButtonReward").GetComponent<Button>();

                Text textReward = buttonReward.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textReward);
                textReward.text = CsConfiguration.Instance.GetString("");

                CsHeroTodayTask csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(csTaskConsignment.TodayTask.TaskId);

                switch ((EnTaskConsignment)csTaskConsignment.ConsignmentId)
                {
                    case EnTaskConsignment.UndergroundMaze:

                        System.TimeSpan tsRemainingTime;

                        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount(csTaskConsignment.ConsignmentId) == null)
                        {
                            tsRemainingTime = System.TimeSpan.FromSeconds(CsGameData.Instance.UndergroundMaze.LimitTime - CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime);
                        }
                        else
                        {
                            tsRemainingTime = System.TimeSpan.FromSeconds(0.0f);
                        }

                        textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01008"), tsRemainingTime.Hours.ToString("0#"), tsRemainingTime.Minutes.ToString("0#"), tsRemainingTime.Seconds.ToString("0#"));

                        break;

                    case EnTaskConsignment.aaa:
                        break;

                    case EnTaskConsignment.GuildHunting:

                        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignment(csTaskConsignment.ConsignmentId) == null)
                        {
                            if (csHeroTodayTask == null)
                            {
                                textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A109_TXT_00006"), 0, csTaskConsignment.TodayTask.MaxCount);
                            }
                            else
                            {
                                textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A109_TXT_00006"), csHeroTodayTask.ProgressCount, csTaskConsignment.TodayTask.MaxCount);
                            }
                        }
                        else
                        {
                            textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A109_TXT_00006"), csTaskConsignment.TodayTask.MaxCount, csTaskConsignment.TodayTask.MaxCount);
                        }

                        break;

                    case EnTaskConsignment.GuildMission:

                        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignment(csTaskConsignment.ConsignmentId) == null)
                        {
                            if (csHeroTodayTask == null)
                            {
                                textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A109_TXT_00006"), 0, csTaskConsignment.TodayTask.MaxCount);
                            }
                            else
                            {
                                textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A109_TXT_00006"), csHeroTodayTask.ProgressCount, csTaskConsignment.TodayTask.MaxCount);
                            }
                        }
                        else
                        {
                            textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A109_TXT_00006"), csTaskConsignment.TodayTask.MaxCount, csTaskConsignment.TodayTask.MaxCount);
                        }

                        break;

                    case EnTaskConsignment.bbb:
                        break;

                    case EnTaskConsignment.ccc:
                        break;
                }

                // 현재 실행 중인 위탁인지
                CsHeroTaskConsignment csHeroTaskConsignment = CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignment(csTaskConsignment.ConsignmentId);

                // 현재 실행중이 아닌 위탁
                if (csHeroTaskConsignment == null)
                {
                    CsHeroTaskConsignmentStartCount csHeroTaskConsignmentStartCount = CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount(csTaskConsignment.ConsignmentId);

                    // 오늘 한번이라도 위탁을 했는지
                    if (csHeroTaskConsignmentStartCount == null)
                    {
                        // 위탁을 아직 안함
                        trTaskConsignmentImmediateInfo.gameObject.SetActive(false);
                        textProgressCount.gameObject.SetActive(true);

                        textTaskConsignmentState.gameObject.SetActive(false);
                        buttonReward.gameObject.SetActive(false);

                        System.TimeSpan timeSpanRemainingTime = System.TimeSpan.FromSeconds(csTaskConsignment.CompletionRequiredTime);
                        textSliderTimer.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME_2"), timeSpanRemainingTime.Hours.ToString("0#"), timeSpanRemainingTime.Minutes.ToString("0#"), timeSpanRemainingTime.Seconds.ToString("0#"));

                        switch ((EnTaskConsignment)csTaskConsignment.ConsignmentId)
                        {
                            case EnTaskConsignment.UndergroundMaze:
                                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.UndergroundMaze.RequiredHeroLevel)
                                {
                                    trTaskConsignmentItem.gameObject.SetActive(false);
                                }
                                else
                                {
                                    buttonGuildApply.gameObject.SetActive(false);

                                    if (CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime == 0.0f)
                                    {
                                        // 위탁 가능
                                        AvailableTaskConsignment(csTaskConsignment, trTaskConsignmentItem);
                                    }
                                    else
                                    {
                                        // 시간 부족
                                        NotAvailableTaskConsignment(csTaskConsignment, trTaskConsignmentItem);
                                    }

                                    trTaskConsignmentItem.gameObject.SetActive(true);
                                }
                                break;

                            case EnTaskConsignment.aaa:

                                break;

                            case EnTaskConsignment.GuildHunting:
                                if (CsGuildManager.Instance.Guild == null)
                                {
                                    buttonTaskConsignment.gameObject.SetActive(false);
                                    buttonGuildApply.gameObject.SetActive(true);
                                }
                                else
                                {
                                    buttonGuildApply.gameObject.SetActive(false);

                                    if (CsGuildManager.Instance.DailyGuildHuntingQuestStartCount == 0 && System.DateTime.Compare(CsGuildManager.Instance.GuildHuntingQuestStartCountDate, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
                                    {
                                        // 위탁 가능
                                        AvailableTaskConsignment(csTaskConsignment, trTaskConsignmentItem);
                                    }
                                    else
                                    {
                                        NotAvailableTaskConsignment(csTaskConsignment, trTaskConsignmentItem);
                                    }
                                }
                                break;

                            case EnTaskConsignment.GuildMission:
                                if (CsGuildManager.Instance.Guild == null)
                                {
                                    buttonTaskConsignment.gameObject.SetActive(false);
                                    buttonGuildApply.gameObject.SetActive(true);
                                }
                                else
                                {
                                    buttonGuildApply.gameObject.SetActive(false);

                                    if (CsGuildManager.Instance.MissionCompletedCount == 0 && !CsGuildManager.Instance.MissionQuest)
                                    {
                                        // 위탁 가능
                                        AvailableTaskConsignment(csTaskConsignment, trTaskConsignmentItem);
                                    }
                                    else
                                    {
                                        NotAvailableTaskConsignment(csTaskConsignment, trTaskConsignmentItem);
                                    }
                                }
                                break;

                            case EnTaskConsignment.bbb:

                                break;

                            case EnTaskConsignment.ccc:

                                break;
                        }
                    }
                    else
                    {
                        // 이미 함...
                        textProgressCount.gameObject.SetActive(true);
                        trTaskConsignmentImmediateInfo.gameObject.SetActive(false);

                        sliderTimer.value = 0;
                        textSliderTimer.gameObject.SetActive(false);
                        textSliderTimer.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME_2"), "00", "00", "00");

                        textTaskConsignmentState.text = CsConfiguration.Instance.GetString("A109_TXT_00005");
                        textTaskConsignmentState.color = CsUIData.Instance.ColorGreen;

                        textTaskConsignmentState.gameObject.SetActive(true);
                        buttonTaskConsignment.gameObject.SetActive(false);
                        buttonReward.gameObject.SetActive(false);
                        buttonGuildApply.gameObject.SetActive(false);
                    }
                }
                else
                {
                    buttonGuildApply.gameObject.SetActive(false);
                    
                    // 현재 실행 중인 위탁
                    System.TimeSpan tsTaskConsignmentRemainingTime = System.TimeSpan.FromSeconds(csHeroTaskConsignment.RemainingTime - Time.realtimeSinceStartup);  // 남은 시간

                    // 남은 시간이 없음
                    if (tsTaskConsignmentRemainingTime.TotalSeconds <= 0.0f)
                    {
                        textRemainingTimeMinute.text = "0";

                        textImmediateGoldValue.text = "0";

                        textProgressCount.gameObject.SetActive(false);
                        trTaskConsignmentImmediateInfo.gameObject.SetActive(true);

                        sliderTimer.value = sliderTimer.maxValue;
                        textSliderTimer.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME_2"), "00", "00", "00");

                        buttonReward.onClick.RemoveAllListeners();
                        buttonReward.onClick.AddListener(() => OnClickReward(csHeroTaskConsignment));
                        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                        textReward.text = CsConfiguration.Instance.GetString("A109_TXT_00003");

                        textTaskConsignmentState.gameObject.SetActive(false);
                        buttonTaskConsignment.gameObject.SetActive(false);
                        buttonReward.gameObject.SetActive(true);
                    }
                    // 남은 시간이 있음
                    else
                    {
                        textRemainingTimeMinute.text = (tsTaskConsignmentRemainingTime.Minutes + 1).ToString();

                        int nImmediateGoldValue = 0;

                        // 지하미로 경험치 스크롤 사용 여부에 따른 골드 감소 여부
                        if ((EnTaskConsignment)csTaskConsignment.ConsignmentId == EnTaskConsignment.UndergroundMaze && csHeroTaskConsignment.UsedExpItemId != 0)
                        {
                            nImmediateGoldValue = csTaskConsignment.ImmediateCompletionRequiredGold;
                        }
                        else
                        {
                            nImmediateGoldValue = csTaskConsignment.ImmediateCompletionRequiredGold - ((csTaskConsignment.CompletionRequiredTime - (int)tsTaskConsignmentRemainingTime.TotalSeconds) / csTaskConsignment.ImmediateCompletionRequiredGoldReduceInterval * m_cnReduceGold);
                        }
                        
                        textImmediateGoldValue.text = nImmediateGoldValue.ToString("#,##0");

                        textProgressCount.gameObject.SetActive(false);
                        trTaskConsignmentImmediateInfo.gameObject.SetActive(true);

                        sliderTimer.value = csTaskConsignment.CompletionRequiredTime - (float)tsTaskConsignmentRemainingTime.TotalSeconds;
                        textSliderTimer.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME_2"), tsTaskConsignmentRemainingTime.Hours.ToString("0#"), tsTaskConsignmentRemainingTime.Minutes.ToString("0#"), tsTaskConsignmentRemainingTime.Seconds.ToString("0#"));

                        textValue.text = nImmediateGoldValue.ToString("#,##0");
                        textTaskConsignment.text = CsConfiguration.Instance.GetString("A109_TXT_00005");

                        imageGoods.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods03");

                        textTaskConsignmentState.gameObject.SetActive(false);
                        buttonTaskConsignment.gameObject.SetActive(true);
                        buttonReward.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AvailableTaskConsignment(CsTaskConsignment csTaskConsignment, Transform trTaskConsignmentItem)
    {
        Text textTaskConsignmentState = trTaskConsignmentItem.Find("TextState").GetComponent<Text>();

        Button buttonTaskConsignment = trTaskConsignmentItem.Find("ButtonTaskConsignment").GetComponent<Button>();
        Text textValue = buttonTaskConsignment.transform.Find("TextValue").GetComponent<Text>();
        Text textTaskConsignment = buttonTaskConsignment.transform.Find("Text").GetComponent<Text>();

        Image imageRequiredItemIcon = buttonTaskConsignment.transform.Find("Image").GetComponent<Image>();
        imageRequiredItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csTaskConsignment.RequiredItem.Image);

        // 위탁 가능
        textValue.text = csTaskConsignment.RequiredItemCount.ToString("#,##0");
        textTaskConsignment.text = CsConfiguration.Instance.GetString("A109_TXT_00004");

        textTaskConsignmentState.gameObject.SetActive(false);
        buttonTaskConsignment.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void NotAvailableTaskConsignment(CsTaskConsignment csTaskConsignment, Transform trTaskConsignmentItem)
    {
        Button buttonTaskConsignment = trTaskConsignmentItem.Find("ButtonTaskConsignment").GetComponent<Button>();

        Text textTaskConsignmentState = trTaskConsignmentItem.Find("TextState").GetComponent<Text>();

        textTaskConsignmentState.color = CsUIData.Instance.ColorRed;

        buttonTaskConsignment.gameObject.SetActive(false);
        textTaskConsignmentState.gameObject.SetActive(true);

        switch ((EnTaskConsignment)csTaskConsignment.ConsignmentId)
        {
            case EnTaskConsignment.UndergroundMaze:
                textTaskConsignmentState.text = CsConfiguration.Instance.GetString("A109_TXT_00002");
                break;

            case EnTaskConsignment.aaa:
                break;

            case EnTaskConsignment.GuildHunting:
                textTaskConsignmentState.text = CsConfiguration.Instance.GetString("A109_TXT_00014");
                break;

            case EnTaskConsignment.GuildMission:
                textTaskConsignmentState.text = CsConfiguration.Instance.GetString("A109_TXT_00014");
                break;

            case EnTaskConsignment.bbb:
                break;

            case EnTaskConsignment.ccc:
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupUseExpScroll()
    {
        InitializePopupUseExpScroll();
        m_trPopupUseExpItem.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupUseExpScroll()
    {
        Button buttonPopupClose = m_trPopupUseExpItem.Find("ButtonClose").GetComponent<Button>();
        buttonPopupClose.onClick.RemoveAllListeners();
        buttonPopupClose.onClick.AddListener(OnClickPopupClose);
        buttonPopupClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textPopupName = buttonPopupClose.transform.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A109_TXT_00008");

        Transform trImageExpItemList = buttonPopupClose.transform.Find("ImageExpItemList");

        Transform trExpItemSlot = null;
        List<CsItem> listExpScroll = CsGameData.Instance.ItemList.FindAll(a => a.ItemType.EnItemType == EnItemType.ExpScroll);
        
        for (int i = 0; i < listExpScroll.Count; i++)
        {
            CsItem csItem = listExpScroll[i];
            trExpItemSlot = trImageExpItemList.Find("ImageExpItemSlot" + i);

            if (trExpItemSlot == null)
            {
                continue;
            }
            else
            {
                int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);
                Transform trItemSlot = trExpItemSlot.Find("ItemSlot");

                CsUIData.Instance.DisplayItemSlot(trItemSlot, listExpScroll[i], false, nItemCount, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                Text textName = trExpItemSlot.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);
                textName.text = csItem.Name;

                Button buttonUse = trExpItemSlot.Find("Button").GetComponent<Button>();
                buttonUse.onClick.RemoveAllListeners();
                buttonUse.onClick.AddListener(() => OnClickExpScrollUse(csItem.ItemId));
                buttonUse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textUse = buttonUse.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textUse);
                textUse.text = CsConfiguration.Instance.GetString("A109_TXT_00010");

                if (0 < nItemCount)
                {
                    textName.color = CsUIData.Instance.ColorWhite;
                    CsUIData.Instance.DisplayButtonInteractable(buttonUse, true);
                }
                else
                {
                    textName.color = CsUIData.Instance.ColorRed;
                    CsUIData.Instance.DisplayButtonInteractable(buttonUse, false);
                }
            }
        }

        Text textDescription = buttonPopupClose.transform.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString("A109_TXT_00009");

        Text textClose = buttonPopupClose.transform.Find("TextClose").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClose);
        textClose.text = CsConfiguration.Instance.GetString("A109_TXT_00011");
    }

    //---------------------------------------------------------------------------------------------------
    bool AvailableReward(CsHeroTaskConsignment csHeroTaskConsignment)
    {
        int nCount = 0;
        bool bReward = true;

        CsTaskConsignment csTaskConsignment = CsGameData.Instance.GetTaskConsignment(csHeroTaskConsignment.ConsignmentId);

        if (csTaskConsignment == null)
        {
            return false;
        }
        else
        {
            List<CsTaskConsignmentAvailableReward> listCsTaskConsignmentAvailableReward = csTaskConsignment.TaskConsignmentAvailableRewardList.FindAll(a => a.ConsignmentId == csTaskConsignment.ConsignmentId);

            for (int i = 0; i < listCsTaskConsignmentAvailableReward.Count; i++)
            {
                CsTaskConsignmentAvailableReward csTaskConsignmentAvailableReward = listCsTaskConsignmentAvailableReward[i];

                if (1 <= CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(csTaskConsignmentAvailableReward.Item.ItemId, false))
                {
                    continue;
                }
                else
                {
                    if ((CsGameData.Instance.MyHeroInfo.InventorySlotList.Count + nCount) < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        nCount++;
                    }
                    else
                    {
                        bReward = false;
                        break;
                    }
                }
            }
        }

        return bReward;
    }

    //---------------------------------------------------------------------------------------------------
    void PopupClose()
    {
        Destroy(gameObject);
    }
}
