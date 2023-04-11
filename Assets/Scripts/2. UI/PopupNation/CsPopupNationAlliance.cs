using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationAlliance : CsPopupSub
{
    Transform m_trNationList;
    Transform m_trButtonList;
    Transform m_trPanelModal;

    Button m_buttonCancel;
    Button m_buttonApply;
    Button m_buttonBreak;

    int m_nSelectNationId = 0;

    bool m_bFirstPanelModal = true;

    float m_flTime = 0f;
    float m_flNationAllianceRemaningTime = 0f;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept += OnEventNationAllianceApplicationAccept;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccepted += OnEventNationAllianceApplicationAccepted;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationCancel += OnEventNationAllianceApplicationCancel;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationCanceled += OnEventNationAllianceApplicationCanceled;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationReject += OnEventNationAllianceApplicationReject;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationRejected += OnEventNationAllianceApplicationRejected;
        CsNationAllianceManager.Instance.EventNationAllianceApplied += OnEventNationAllianceApplied;
        CsNationAllianceManager.Instance.EventNationAllianceApply += OnEventNationAllianceApply;
        CsNationAllianceManager.Instance.EventNationAllianceBreak += OnEventNationAllianceBreak;
        CsNationAllianceManager.Instance.EventNationAllianceBroken += OnEventNationAllianceBroken;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded += OnEventNationAllianceConcluded;

        CsGameEventUIToUI.Instance.EventDailyServerNationPowerRankingUpdated += OnEventDailyServerNationPowerRankingUpdated;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept -= OnEventNationAllianceApplicationAccept;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccepted -= OnEventNationAllianceApplicationAccepted;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationCancel -= OnEventNationAllianceApplicationCancel;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationCanceled -= OnEventNationAllianceApplicationCanceled;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationReject -= OnEventNationAllianceApplicationReject;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationRejected -= OnEventNationAllianceApplicationRejected;
        CsNationAllianceManager.Instance.EventNationAllianceApplied -= OnEventNationAllianceApplied;
        CsNationAllianceManager.Instance.EventNationAllianceApply -= OnEventNationAllianceApply;
        CsNationAllianceManager.Instance.EventNationAllianceBreak -= OnEventNationAllianceBreak;
        CsNationAllianceManager.Instance.EventNationAllianceBroken -= OnEventNationAllianceBroken;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded -= OnEventNationAllianceConcluded;

        CsGameEventUIToUI.Instance.EventDailyServerNationPowerRankingUpdated -= OnEventDailyServerNationPowerRankingUpdated;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + 1.0f < Time.time)
        {
            // 국가 동맹 시간 체크
            UpdateButton();
            UpdatePanelModal();

            m_flTime = m_flTime + 1.0f;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccept(CsNationAlliance csNationAlliance)
    {
        // 국가 동맹 신청 수락
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationAlliance(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccepted()
    {
        // 국가 동맹 신청 수락
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationAlliance(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationCancel()
    {
        // 국가 동맹 신청 제의 취소
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationFund(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationCanceled()
    {
        // 국가 동맹 신청 제의 취소
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationFund(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationReject()
    {
        // 국가 동맹 신청 거절
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationFund(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationRejected()
    {
        // 국가 동맹 거절
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationFund(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplied()
    {
        // 국가 동맹 신청
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationFund(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApply()
    {
        // 국가 동맹 신청
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationFund(csNation);
        }

        UpdateImageSuggest();
        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceBreak()
    {
        // 국가 동맹 파기
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateNationAlliance(csNation);
        }

        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceBroken()
    {
        // 국가 동맹 파기
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateNationAlliance(csNation);
        }

        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceConcluded(CsNationAlliance csNationAlliance)
    {
        // 
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateImageNotice(csNation);
            UpdateNationAlliance(csNation);
        }

        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDailyServerNationPowerRankingUpdated()
    {
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            UpdateNationPowerValue(csNation);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleNation(bool bIson, CsNation csNation)
    {
        if (bIson)
        {
            m_nSelectNationId = csNation.NationId;
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            if (m_trNationList.GetComponent<ToggleGroup>().AnyTogglesOn() == false)
            {
                m_nSelectNationId = 0;
            }
            else
            {
                return;
            }
        }

        UpdateButton();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAliianceApply()
    {
        string strMessage = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00010"), CsGameData.Instance.GetNation(m_nSelectNationId).Name, CsGameConfig.Instance.NationAllianceRequiredFund);

        CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsNationAllianceManager.Instance.SendNationAllianceApply(m_nSelectNationId),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAllianceBreak()
    {
        CsNationAlliance csNationAlliance = CsNationAllianceManager.Instance.GetNationAlliance(m_nSelectNationId);

        if (csNationAlliance == null)
        {
            return;
        }
        else
        {
            m_flNationAllianceRemaningTime = csNationAlliance.AllianceRenounceAvailableRemainingTime;
            TimeSpan tsNationallianceRemainingTime = TimeSpan.FromSeconds(m_flNationAllianceRemaningTime - Time.realtimeSinceStartup);

            if (tsNationallianceRemainingTime.TotalSeconds > 0)
            {
                if (m_bFirstPanelModal)
                {
                    InitializePanelModal();
                }
                else
                {
                    m_trPanelModal.gameObject.SetActive(true);
                }
            }
            else
            {
                string strMessage = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00013"), CsGameData.Instance.GetNation(m_nSelectNationId).Name);

                CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsNationAllianceManager.Instance.SendNationAllianceBreak(csNationAlliance.Id),
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAllianceCancel()
    {
        CsNationAllianceApplication csNationAllianceApplication = CsNationAllianceManager.Instance.NationAllianceApplicationList.Find(a => a.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId && a.TargetNationId == m_nSelectNationId);

        if (csNationAllianceApplication == null)
        {
            return;
        }
        else
        {
            string strMessage = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00011"), CsGameData.Instance.GetNation(csNationAllianceApplication.TargetNationId).Name);
            CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsNationAllianceManager.Instance.SendNationAllianceApplicationCancel(csNationAllianceApplication.Id),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationAllianceAccept(CsNation csNation)
    {
        CsNationAllianceApplication csNationAllianceApplication = CsNationAllianceManager.Instance.NationAllianceApplicationList.Find(a => a.NationId == csNation.NationId && a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId);

        if (csNationAllianceApplication == null)
        {
            return;
        }
        else
        {
            string strMessage = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00015"), CsGameData.Instance.GetNation(csNationAllianceApplication.NationId).Name, CsGameConfig.Instance.NationAllianceRequiredFund);
            CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsNationAllianceManager.Instance.SendNationAllianceApplicationAccept(csNationAllianceApplication.Id),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), () => CsNationAllianceManager.Instance.SendNationAllianceApplicationReject(csNationAllianceApplication.Id), true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAlert()
    {
        if (m_trPanelModal == null)
        {
            return;
        }
        else
        {
            m_trPanelModal.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trNationList = transform.Find("NationList");

        InitializeToggleNation();

        Text textDescription = transform.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00005"), CsGameConfig.Instance.NationAllianceRequiredFund, TimeSpan.FromSeconds(CsGameConfig.Instance.NationAllianceUnavailableStartTime).Hours, TimeSpan.FromSeconds(CsGameConfig.Instance.NationAllianceUnavailableEndTime).Hours);

        m_buttonCancel = transform.Find("ButtonCancel").GetComponent<Button>();
        m_buttonCancel.onClick.RemoveAllListeners();
        m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonCancel.onClick.AddListener(OnClickAllianceCancel);

        Text textButtonCancel = m_buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonCancel);
        textButtonCancel.text = CsConfiguration.Instance.GetString("A156_TXT_00006");

        m_buttonApply = transform.Find("ButtonApply").GetComponent<Button>();
        m_buttonApply.onClick.RemoveAllListeners();
        m_buttonApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonApply.onClick.AddListener(OnClickAliianceApply);

        Text textButtonApply = m_buttonApply.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonApply);
        textButtonApply.text = CsConfiguration.Instance.GetString("A156_TXT_00007");

        m_buttonBreak = transform.Find("ButtonBreak").GetComponent<Button>();
        m_buttonBreak.onClick.RemoveAllListeners();
        m_buttonBreak.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonBreak.onClick.AddListener(OnClickAllianceBreak);

        Text textButtonBreak = m_buttonBreak.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonBreak);
        textButtonBreak.text = CsConfiguration.Instance.GetString("A156_TXT_00008");

        UpdateButton();

        m_trPanelModal = transform.Find("PanelModal");
    }
    
    //---------------------------------------------------------------------------------------------------
    void InitializePanelModal()
    {
        m_bFirstPanelModal = false;

        Transform trImageBackground = m_trPanelModal.Find("ImageBackground");

        TimeSpan tsRemainingTime = TimeSpan.FromSeconds(m_flNationAllianceRemaningTime - Time.realtimeSinceStartup);

        Text textMessage = trImageBackground.Find("TextMessage").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMessage);
        textMessage.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsRemainingTime.Hours.ToString("00"), tsRemainingTime.Minutes.ToString("00"));

        Button buttonAlert = trImageBackground.Find("ButtonAlert").GetComponent<Button>();
        buttonAlert.onClick.RemoveAllListeners();
        buttonAlert.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonAlert.onClick.AddListener(OnClickAlert);

        Text textButtonAlert = buttonAlert.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAlert);
        textButtonAlert.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");

        m_trPanelModal.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePanelModal()
    {
        if (0f < m_flNationAllianceRemaningTime - Time.realtimeSinceStartup && m_trPanelModal.gameObject.activeSelf)
        {
            TimeSpan tsRemainingTime = TimeSpan.FromSeconds(m_flNationAllianceRemaningTime - Time.realtimeSinceStartup);

            Text textMessage = m_trPanelModal.Find("ImageBackground/TextMessage").GetComponent<Text>();
            textMessage.text = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00014"), tsRemainingTime.Hours.ToString("00"), tsRemainingTime.Minutes.ToString("00"), tsRemainingTime.Seconds.ToString("00"));
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeToggleNation()
    {
        Transform trToggleNation = null;

        for (int i = 0; i < m_trNationList.childCount; i++)
        {
            if (i < CsGameData.Instance.NationList.Count)
            {
                CsNation csNation = CsGameData.Instance.NationList[i];

                trToggleNation = m_trNationList.Find("ToggleNation" + csNation.NationId);

                if (trToggleNation == null)
                {
                    continue;
                }
                else
                {
                    Toggle toggleNation = trToggleNation.GetComponent<Toggle>();
                    toggleNation.onValueChanged.RemoveAllListeners();
                    toggleNation.isOn = false;

                    CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(CsGameData.Instance.MyHeroInfo.HeroId);
                    
                    if (csNationNoblesseInstance != null && csNationNoblesseInstance.NationNoblesse.NationAllianceEnabled)
                    {
                        if (CsGameData.Instance.MyHeroInfo.Nation.NationId == csNation.NationId)
                        {
                            toggleNation.interactable = false;
                        }
                        else
                        {
                            toggleNation.interactable = true;
                        }
                    }
                    else
                    {
                        toggleNation.interactable = false;
                    }

                    toggleNation.onValueChanged.AddListener((ison) => OnValueChangedToggleNation(ison, csNation));

                    Text textNationName = trToggleNation.Find("TextNationName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationName);
                    textNationName.text = csNation.Name;

                    Text textNationAlliance = trToggleNation.Find("TextAlliance").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationAlliance);
                    UpdateNationAlliance(csNation);

                    Text textNationFund = trToggleNation.Find("TextNationFund").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationFund);
                    textNationFund.text = CsConfiguration.Instance.GetString("A156_TXT_00003");

                    Text textNationFundValue = trToggleNation.Find("TextNationFundValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationFundValue);

                    CsNationInstance csNationInstance = CsGameData.Instance.MyHeroInfo.NationInstanceList.Find(a => a.NationId == csNation.NationId);

                    if (csNationInstance == null)
                    {
                        textNationFundValue.text = "";
                    }
                    else
                    {
                        textNationFundValue.text = csNationInstance.Fund.ToString("#,##0");
                    }

                    Text textNationPower = trToggleNation.Find("TextNationPower").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationPower);
                    textNationPower.text = CsConfiguration.Instance.GetString("A156_TXT_00004");

                    Text textNationPowerValue = trToggleNation.Find("TextNationPowerValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationPowerValue);
                    UpdateNationPowerValue(csNation);

                    Transform trImageMyNation = trToggleNation.Find("ImageMyNation");

                    if (csNation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        trImageMyNation.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageMyNation.gameObject.SetActive(false);
                    }

                    Text textMyNation = trImageMyNation.Find("TextMyNation").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textMyNation);
                    textMyNation.text = CsConfiguration.Instance.GetString("A156_TXT_00016");

                    UpdateImageNotice(csNation);

                    Text textSuggest = trToggleNation.Find("ImageSuggest/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textSuggest);
                    textSuggest.text = CsConfiguration.Instance.GetString("A156_TXT_00009");

                    Button buttonAllianceApply = trToggleNation.Find("ButtonAllianceAccept").GetComponent<Button>();
                    buttonAllianceApply.onClick.RemoveAllListeners();
                    buttonAllianceApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    buttonAllianceApply.onClick.AddListener(() => OnClickNationAllianceAccept(csNation));
                }
            }
            else
            {
                continue;
            }
        }

        UpdateImageSuggest();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationFund(CsNation csNation)
    {
        Transform trToggleNation = m_trNationList.Find("ToggleNation" + csNation.NationId);

        if (trToggleNation == null)
        {
            return;
        }
        else
        {
            Text textNationFundValue = trToggleNation.Find("TextNationFundValue").GetComponent<Text>();

            CsNationInstance csNationInstance = CsGameData.Instance.MyHeroInfo.NationInstanceList.Find(a => a.NationId == csNation.NationId);

            if (csNationInstance == null)
            {
                textNationFundValue.text = "";
            }
            else
            {
                textNationFundValue.text = csNationInstance.Fund.ToString("#,##0");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 동맹 차원 표시
    void UpdateNationAlliance(CsNation csNation)
    {
        Transform trToggleNation = m_trNationList.Find("ToggleNation" + csNation.NationId);

        if (trToggleNation == null)
        {
            return;
        }
        else
        {
            Text textNationAlliance = trToggleNation.Find("TextAlliance").GetComponent<Text>();
            CsNation csNationAlliacne = CsGameData.Instance.GetNation(CsNationAllianceManager.Instance.GetNationAllianceId(csNation.NationId));

            if (csNationAlliacne == null)
            {
                // 동맹 없음
                textNationAlliance.text = CsConfiguration.Instance.GetString("A156_TXT_00002");
            }
            else
            {
                // 동맹국 차원
                textNationAlliance.text = string.Format(CsConfiguration.Instance.GetString("A156_TXT_00001"), csNationAlliacne.Name);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국력
    void UpdateNationPowerValue(CsNation csNation)
    {
        Transform trToggleNation = m_trNationList.Find("ToggleNation" + csNation.NationId);

        if (trToggleNation == null)
        {
            return;
        }
        else
        {
            Text textNationPowerValue = trToggleNation.Find("TextNationPowerValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationPowerValue);

            if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList == null)
            {
                textNationPowerValue.text = CsGameConfig.Instance.NationBasePower.ToString("#,##0");
            }
            else
            {
                CsNationPowerRanking csNationPowerRanking = CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Find(a => a.NationId == csNation.NationId);

                if (csNationPowerRanking == null)
                {
                    textNationPowerValue.text = CsGameConfig.Instance.NationBasePower.ToString("#,##0");
                }
                else
                {
                    if (csNationPowerRanking.Ranking == 0)
                    {
                        textNationPowerValue.text = CsGameConfig.Instance.NationBasePower.ToString("#,##0");
                    }
                    else
                    {
                        textNationPowerValue.text = csNationPowerRanking.NationPower.ToString("#,##0");
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 동맹 신청 레드닷 표시
    void UpdateImageNotice(CsNation csNation)
    {
        Transform trToggleNation = m_trNationList.Find("ToggleNation" + csNation.NationId);

        if (trToggleNation == null)
        {
            return;
        }
        else
        {
            Transform trImageNotice = trToggleNation.Find("ImageNotice");

            CsNationAllianceApplication csNationAllianceApplication = CsNationAllianceManager.Instance.NationAllianceApplicationList.Find(a => a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId && a.NationId == csNation.NationId);

            Toggle toggleNation = trToggleNation.GetComponent<Toggle>();
            Button buttonAllianceApply = trToggleNation.Find("ButtonAllianceAccept").GetComponent<Button>();

            CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(CsGameData.Instance.MyHeroInfo.HeroId);

            if (csNationAllianceApplication == null)
            {
                trImageNotice.gameObject.SetActive(false);

                if (csNationNoblesseInstance != null && csNationNoblesseInstance.NationNoblesse.NationAllianceEnabled)
                {
                    if (csNation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        toggleNation.interactable = false;
                        buttonAllianceApply.enabled = false;
                    }
                    else
                    {
                        toggleNation.interactable = true;
                        buttonAllianceApply.enabled = false;
                    }
                }
                else
                {
                    toggleNation.interactable = false;
                    buttonAllianceApply.enabled = false;
                }
            }
            else
            {
                trImageNotice.gameObject.SetActive(true);

                if (csNationNoblesseInstance != null && csNationNoblesseInstance.NationNoblesse.NationAllianceEnabled)
                {
                    if (csNation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        toggleNation.interactable = false;
                        buttonAllianceApply.enabled = false;
                    }
                    else
                    {
                        toggleNation.interactable = false;
                        buttonAllianceApply.enabled = true;
                    }
                }
                else
                {
                    toggleNation.interactable = false;
                    buttonAllianceApply.interactable = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageSuggest()
    {
        Transform trToggleNation = null;

        List<int> listMyNationAllianceApplication = CsNationAllianceManager.Instance.GetNationIdAllianceApplicationList(CsGameData.Instance.MyHeroInfo.Nation.NationId);

        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];
            trToggleNation = m_trNationList.Find("ToggleNation" + csNation.NationId);

            Transform trImageSuggest = trToggleNation.Find("ImageSuggest");

            if (listMyNationAllianceApplication.FindIndex(a => a == csNation.NationId) < 0)
            {
                trImageSuggest.gameObject.SetActive(false);
            }
            else
            {
                trImageSuggest.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButton()
    {
        if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Count == 0)
        {
            m_buttonApply.gameObject.SetActive(false);
            m_buttonCancel.gameObject.SetActive(false);
            m_buttonBreak.gameObject.SetActive(false);
        }
        else if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Find(a => a.Ranking == 0) != null)
        {
            m_buttonApply.gameObject.SetActive(false);
            m_buttonCancel.gameObject.SetActive(false);
            m_buttonBreak.gameObject.SetActive(false);
        }
        else
        {

            CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetMyNationNoblesseInstance(CsGameData.Instance.MyHeroInfo.HeroId);

            // 권한 없음
            if (csNationNoblesseInstance == null)
            {
                m_buttonApply.gameObject.SetActive(false);
                m_buttonCancel.gameObject.SetActive(false);
                m_buttonBreak.gameObject.SetActive(false);
            }
            else
            {
                int nCurrentTime = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                if (csNationNoblesseInstance.NationNoblesse.NationAllianceEnabled && (nCurrentTime <= CsGameConfig.Instance.NationAllianceUnavailableStartTime || CsGameConfig.Instance.NationAllianceUnavailableEndTime <= nCurrentTime) && m_nSelectNationId != 0)
                {
                    int nServerOpenDay = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.Subtract(CsGameData.Instance.MyHeroInfo.ServerOpenDate.Date).TotalDays;

                    // 시스템 국가전일때 막아줌
                    if (nServerOpenDay < CsGameData.Instance.NationWar.DeclarationAvailableServerOpenDayCount)
                    {
                        m_buttonApply.gameObject.SetActive(false);
                        m_buttonCancel.gameObject.SetActive(false);
                        m_buttonBreak.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (CsGameData.Instance.MyHeroInfo.NationPowerRankingList == null)
                        {
                            m_buttonApply.gameObject.SetActive(false);
                            m_buttonCancel.gameObject.SetActive(false);
                            m_buttonBreak.gameObject.SetActive(false);
                        }
                        else
                        {
                            CsNationPowerRanking csNationPowerRanking = CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Find(a => a.Ranking == 1);

                            // 국력 1위 막아줌
                            if (csNationPowerRanking != null && csNationPowerRanking.NationId == m_nSelectNationId)
                            {
                                m_buttonApply.gameObject.SetActive(false);
                                m_buttonCancel.gameObject.SetActive(false);
                                m_buttonBreak.gameObject.SetActive(false);
                            }
                            else
                            {
                                if (CsNationAllianceManager.Instance.GetNationAlliance(CsGameData.Instance.MyHeroInfo.Nation.NationId) == null)
                                {
                                    // 내 국가 동맹 없음
                                    List<int> listNationAllianceApplication = CsNationAllianceManager.Instance.GetNationIdAllianceApplicationList(CsGameData.Instance.MyHeroInfo.Nation.NationId);
                                    List<int> listNationAllianceApplicationSelectNation = CsNationAllianceManager.Instance.GetNationIdAllianceApplicationList(m_nSelectNationId);

                                    if (listNationAllianceApplication.FindIndex(a => a == m_nSelectNationId) < 0)
                                    {
                                        // 동맹 제의 안함
                                        if (CsGameConfig.Instance.NationAllianceRequiredFund <= CsGameData.Instance.MyHeroInfo.NationFund)
                                        {
                                            CsUIData.Instance.DisplayButtonInteractable(m_buttonApply, true);
                                        }
                                        else
                                        {
                                            CsUIData.Instance.DisplayButtonInteractable(m_buttonApply, false);
                                        }

                                        m_buttonApply.gameObject.SetActive(true);
                                        m_buttonCancel.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        // 동맹 제의 함
                                        m_buttonApply.gameObject.SetActive(false);
                                        m_buttonCancel.gameObject.SetActive(true);
                                    }

                                    m_buttonBreak.gameObject.SetActive(false);
                                }
                                else
                                {
                                    // 내 국가 동맹 있음
                                    m_buttonApply.gameObject.SetActive(false);
                                    m_buttonCancel.gameObject.SetActive(false);

                                    if (CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId) == m_nSelectNationId)
                                    {
                                        m_buttonBreak.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        m_buttonBreak.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    m_buttonApply.gameObject.SetActive(false);
                    m_buttonCancel.gameObject.SetActive(false);
                    m_buttonBreak.gameObject.SetActive(false);
                }
            }
        }
    }
}