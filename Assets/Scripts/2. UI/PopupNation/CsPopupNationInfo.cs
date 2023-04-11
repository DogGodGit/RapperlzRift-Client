using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationInfo : CsPopupSub
{
    GameObject m_goSearchHero;

    Transform m_trNationNoblessList;
    Transform m_trNationNotice;
    Transform m_trNationInfo;
    Transform m_trPopupDonation;
    Transform m_trPopupAppoint;
    Transform m_trSearchHeroContent;
    Transform m_trRedDot;

    bool m_bFirstPopupDonation = true;
    bool m_bFirstPopupAppoint = true;
    bool m_bIsLoad = false;
    bool m_bIsDonation = true;

    Text m_textNationFundValue;
    Text m_textPopupDonationNationFundValue;

    string m_strSearchHero;
    System.Guid m_guidSelectHeroId;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        StartCoroutine(LoadSearchHero());

        CsGameEventUIToUI.Instance.EventHeroSearch += OnEventHeroSearch;

        CsGameEventUIToUI.Instance.EventNationDonate += OnEventNationDonate;

        CsGameEventUIToUI.Instance.EventNationNoblesseAppointed += OnEventNationNoblesseAppointed;
        CsGameEventUIToUI.Instance.EventNationNoblesseDismissed += OnEventNationNoblesseDismissed;

        CsGameEventUIToUI.Instance.EventNationNoblesseAppoint += OnEventNationNoblesseAppoint;
        CsGameEventUIToUI.Instance.EventNationNoblesseDismiss += OnEventNationNoblesseDismiss;

        CsGameEventUIToUI.Instance.EventNationFundChanged += OnEventNationFundChanged;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventHeroSearch -= OnEventHeroSearch;

        CsGameEventUIToUI.Instance.EventNationDonate -= OnEventNationDonate;

        CsGameEventUIToUI.Instance.EventNationNoblesseAppointed -= OnEventNationNoblesseAppointed;
        CsGameEventUIToUI.Instance.EventNationNoblesseDismissed -= OnEventNationNoblesseDismissed;

        CsGameEventUIToUI.Instance.EventNationNoblesseAppoint -= OnEventNationNoblesseAppoint;
        CsGameEventUIToUI.Instance.EventNationNoblesseDismiss -= OnEventNationNoblesseDismiss;

        CsGameEventUIToUI.Instance.EventNationFundChanged -= OnEventNationFundChanged;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroSearch(List<CsSearchHero> listSearchHero)
    {
        CreateSearchHero(listSearchHero);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationDonate(int nEntryId)
    {
        UpdatePopupDonation();
        UpdateNationFund();
        m_bIsDonation = true;

        if (CsGameData.Instance.MyHeroInfo.CheckNation())
        {
            m_trRedDot.gameObject.SetActive(true);
        }
        else
        {
            m_trRedDot.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationNoblesseAppoint()
    {
        m_trPopupAppoint.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationNoblesseDismiss()
    {

    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationNoblesseAppointed(int nNoblesseId, string strHeroName)
    {
        UpdateNationNoblessList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationNoblesseDismissed()
    {
        UpdateNationNoblessList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationFundChanged()
    {
        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.NationInfo)
        {
            UpdateNationFund();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupDonate()
    {
        if (m_bFirstPopupDonation)
        {
            InitializePopupDonation();
            m_bFirstPopupDonation = false;
        }

        m_trPopupDonation.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupDonate()
    {
        m_trPopupDonation.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonDonate(int nEntryId)
    {
        CsNationDonationEntry csNationDonationEntry = CsGameData.Instance.NationDonationEntryList.Find(a => a.EntryId == nEntryId);

        if (csNationDonationEntry == null)
        {
            return;
        }
        else
        {
            switch (csNationDonationEntry.MoneyType)
            {
                case 1:
                    if (CsGameData.Instance.MyHeroInfo.Gold < csNationDonationEntry.MoneyAmount)
                    {
                        // 토스트
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A61_TXT_02002"));
                    }
                    else
                    {
                        if (m_bIsDonation)
                        {
                            m_bIsDonation = false;
                            CsCommandEventManager.Instance.SendNationDonate(nEntryId);
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;

                case 2:
                    if (CsGameData.Instance.MyHeroInfo.Dia < csNationDonationEntry.MoneyAmount)
                    {
                        // 준비중입니다.
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
                    }
                    else
                    {
                        if (m_bIsDonation)
                        {
                            m_bIsDonation = false;
                            CsCommandEventManager.Instance.SendNationDonate(nEntryId);
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupAppoint(CsNationNoblesseInstance csNationNoblesseInstance)
    {
        // 공석
        if (csNationNoblesseInstance.HeroId == System.Guid.Empty)
        {
            int nNoblessId = csNationNoblesseInstance.NoblesseId;

            if (m_bFirstPopupAppoint)
            {
                InitializePopupAppoint(nNoblessId);
                m_bFirstPopupAppoint = false;
            }

            UpdatePopupAppoint(nNoblessId);
            m_trPopupAppoint.gameObject.SetActive(true);
        }
        // 공석이 아님
        else
        {
            CsGameEventUIToUI.Instance.OnEventOpenNationNoblesseReference(csNationNoblesseInstance);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupAppoint()
    {
        m_trPopupAppoint.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSearchHeroName(string strSearchHeroName)
    {
        m_strSearchHero = strSearchHeroName;

        if (strSearchHeroName.Length < 2)
        {
            // 두글자 이하
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A61_TXT_00018"));
        }
        else
        {
            CsCommandEventManager.Instance.SendHeroSearch(strSearchHeroName);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSelectSearchHero(bool bIson, System.Guid guidSelectHeroId)
    {
        if (bIson)
        {
            m_guidSelectHeroId = guidSelectHeroId;
            UpdateButtonAppoint();
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAppoint(int nNoblesseId)
    {
        CsNationNoblesseInstance csNationNoblessInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstance(nNoblesseId);

        if (csNationNoblessInstance == null)
        {
            return;
        }
        else
        {
            if (csNationNoblessInstance.AppointmentDate.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A61_TXT_00019"));
            }
            else
            {
                CsNationNoblesseInstance csSelectHeroNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(m_guidSelectHeroId);

                if (csSelectHeroNationNoblesseInstance == null)
                {
                    // 관직 없음 -> 임명
                    CsCommandEventManager.Instance.SendNationNoblesseAppoint(nNoblesseId, m_guidSelectHeroId);
                }
                else
                {
                    // 에러 토스트
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A61_TXT_02001"));
                }
            }
        }
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trNationNoblessList = transform.Find("NationNoblessList");
        m_trNationNotice = transform.Find("NationNotice");
        m_trNationInfo = transform.Find("NationInfo");

        m_trPopupDonation = transform.Find("PopupNationDonation");
        m_trPopupAppoint = transform.Find("PopupNoblessAppoint");

        // 국가 관직
        UpdateNationNoblessList();

        // 국가 공지
        Text textNationNotice = m_trNationNotice.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationNotice);
        textNationNotice.text = CsConfiguration.Instance.GetString("A61_TXT_00001");

        //Transform trNoticeList = m_trNationNotice.Find("ImageBackground");

        // 국가 정보
        Image imageNationIcon = m_trNationInfo.Find("ImageNationIcon").GetComponent<Image>();
        imageNationIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + CsGameData.Instance.MyHeroInfo.Nation.NationId);

        Transform trNationInfoList = m_trNationInfo.Find("NationInfoList");

        Text textNationName = trNationInfoList.Find("TextNationName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationName);
        textNationName.text = CsGameData.Instance.MyHeroInfo.Nation.Name;

        Text textNationAlliance = trNationInfoList.Find("NationAlliance/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationAlliance);
        textNationAlliance.text = CsConfiguration.Instance.GetString("A61_TXT_00002");

        Text textNationAllianceValue = trNationInfoList.Find("NationAlliance/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationAllianceValue);

        Text textNationPower = trNationInfoList.Find("NationPower/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationPower);
        textNationPower.text = CsConfiguration.Instance.GetString("A61_TXT_00003");

        Text textNationPowerValue = trNationInfoList.Find("NationPower/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationPowerValue);

        Text textNationFund = trNationInfoList.Find("NationFund/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationFund);
        textNationFund.text = CsConfiguration.Instance.GetString("A61_TXT_00004");

        m_textNationFundValue = trNationInfoList.Find("NationFund/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNationFundValue);
        m_textNationFundValue.text = CsGameData.Instance.MyHeroInfo.NationFund.ToString("#,##0");

        Button buttonDonation = m_trNationInfo.Find("ButtonDonate").GetComponent<Button>();
        buttonDonation.onClick.RemoveAllListeners();
        buttonDonation.onClick.AddListener(OnClickOpenPopupDonate);
        buttonDonation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Debug.Log("CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.NationDonation) : " + CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.NationDonation));
        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.NationDonation))
        {
            // 헌납 열림
            buttonDonation.gameObject.SetActive(true);
        }
        else
        {
            // 닫힘
            buttonDonation.gameObject.SetActive(false);
        }

        m_trRedDot = buttonDonation.transform.Find("Image");

        if (CsGameData.Instance.MyHeroInfo.CheckNation())
        {
            m_trRedDot.gameObject.SetActive(true);
        }
        else
        {
            m_trRedDot.gameObject.SetActive(false);
        }

        Text textButtonDonation = buttonDonation.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonDonation);
        textButtonDonation.text = CsConfiguration.Instance.GetString("A61_BTN_00001");

        m_guidSelectHeroId = System.Guid.Empty;
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupDonation()
    {
        Transform trImageBackground = m_trPopupDonation.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A61_TXT_00005");

        Text textOwnGold = trImageBackground.Find("ImageGold/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOwnGold);

        Text textOwnDia = trImageBackground.Find("ImageDia/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOwnDia);

        Button buttonExit = trImageBackground.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(OnClickClosePopupDonate);
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textTodayDonate = trImageBackground.Find("TextTodayDonation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTodayDonate);
        textTodayDonate.text = CsConfiguration.Instance.GetString("A61_TXT_00006");

        Text textTodayDonationCount = textTodayDonate.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTodayDonationCount);

        Text textPopupDonateNationFund = trImageBackground.Find("TextNationFund").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupDonateNationFund);
        textPopupDonateNationFund.text = CsConfiguration.Instance.GetString("A61_TXT_00007");

        m_textPopupDonationNationFundValue = textPopupDonateNationFund.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPopupDonationNationFundValue);

        Transform trDonateList = trImageBackground.Find("DonationList");
        Transform trDonate = null;

        for (int i = 0; i < CsGameData.Instance.NationDonationEntryList.Count; i++)
        {
            trDonate = trDonateList.Find("Donation" + i);

            if (trDonate == null)
            {
                continue;
            }
            else
            {
                Text textName = trDonate.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);
                textName.text = CsGameData.Instance.NationDonationEntryList[i].Name;

                Transform trExploit = trDonate.Find("Exploit");
                Text textExploit = trExploit.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textExploit);
                textExploit.text = CsConfiguration.Instance.GetString("A61_TXT_00011");

                Text textExploitValue = trExploit.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textExploitValue);
                textExploitValue.text = CsGameData.Instance.NationDonationEntryList[i].ExploitPointReward.Value.ToString("#,##0");

                Transform trNationFund = trDonate.Find("NationFund");
                Text textNationFund = trNationFund.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNationFund);
                textNationFund.text = CsConfiguration.Instance.GetString("A61_TXT_00012");

                Text textNationFundValue = trNationFund.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNationFundValue);
                textNationFundValue.text = CsGameData.Instance.NationDonationEntryList[i].NationFundReward.Value.ToString("#,##0");

                int nEntryId = CsGameData.Instance.NationDonationEntryList[i].EntryId;
                Button buttonDonate = trDonate.Find("Button").GetComponent<Button>();
                buttonDonate.onClick.RemoveAllListeners();
                buttonDonate.onClick.AddListener(() => OnClickButtonDonate(nEntryId));
                buttonDonate.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textDonateValue = buttonDonate.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDonateValue);
                textDonateValue.text = CsGameData.Instance.NationDonationEntryList[i].MoneyAmount.ToString("#,##0");
            }
        }

        Text textExploitPoint = trImageBackground.Find("ImageExploit/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExploitPoint);

        Text textTodayExploitPoint = trImageBackground.Find("ImageExploit/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTodayExploitPoint);

        UpdatePopupDonation();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupAppoint(int nNoblessId)
    {
        Transform trImageBackground = m_trPopupAppoint.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = string.Format(CsConfiguration.Instance.GetString("A61_TXT_00014"), CsGameData.Instance.GetNationNoblesse(nNoblessId).Name);

        Button buttonExit = trImageBackground.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(OnClickClosePopupAppoint);
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        InputField inputFieldHeroName = trImageBackground.Find("InputFieldHeroName").GetComponent<InputField>();

        Text textPlaceHolder = inputFieldHeroName.transform.Find("Placeholder").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPlaceHolder);
        textPlaceHolder.text = CsConfiguration.Instance.GetString("A01_TXT_00014");

        Button buttonSearch = trImageBackground.Find("ButtonSearch").GetComponent<Button>();
        buttonSearch.onClick.RemoveAllListeners();
        buttonSearch.onClick.AddListener(() => OnClickSearchHeroName(inputFieldHeroName.text));
        buttonSearch.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trHeroList = trImageBackground.Find("HeroList");

        Text textHeroList = trHeroList.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textHeroList);
        textHeroList.text = CsConfiguration.Instance.GetString("A61_TXT_00015");

        m_trSearchHeroContent = trHeroList.Find("Scroll View/Viewport/Content");

        Button buttonAppoint = trImageBackground.Find("ButtonAppoint").GetComponent<Button>();
        buttonAppoint.onClick.RemoveAllListeners();

        UpdateButtonAppoint();

        Text textButtonAppoint = buttonAppoint.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAppoint);
        textButtonAppoint.text = CsConfiguration.Instance.GetString("A61_BTN_00002");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationNoblessList()
    {
        Transform trButtonNationNobless = null;
        CsNationNoblesse csMyHeroNationNoblesse = null;
        CsNationNoblesseInstance csMyHeroNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.NationNoblesseInstanceList.Find(a => a.HeroId == CsGameData.Instance.MyHeroInfo.HeroId);

        if (csMyHeroNationNoblesseInstance == null)
        {
            csMyHeroNationNoblesse = null;
        }
        else
        {
            csMyHeroNationNoblesse = CsGameData.Instance.GetNationNoblesse(csMyHeroNationNoblesseInstance.NoblesseId);
        }

        // 국가 관직
        for (int i = 0; i < CsGameData.Instance.NationNoblesseList.Count; i++)
        {
            trButtonNationNobless = m_trNationNoblessList.Find("ButtonNationNobless" + i);

            if (trButtonNationNobless == null)
            {
                continue;
            }
            else
            {
                int nNoblessId = CsGameData.Instance.NationNoblesseList[i].NoblesseId;
                CsNationNoblesse csNationNoblesse = CsGameData.Instance.GetNationNoblesse(nNoblessId);

                Text textNoblessName = trButtonNationNobless.Find("TextNoblessName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNoblessName);
                textNoblessName.text = csNationNoblesse.Name;

                Text textHeroName = trButtonNationNobless.Find("TextHeroName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textHeroName);

                Button buttonNationNobless = trButtonNationNobless.GetComponent<Button>();
                buttonNationNobless.onClick.RemoveAllListeners();

                CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.NationNoblesseInstanceList.Find(a => a.NoblesseId == CsGameData.Instance.NationNoblesseList[i].NoblesseId);

                if (csNationNoblesseInstance.HeroId == System.Guid.Empty)
                {
                    textHeroName.text = CsConfiguration.Instance.GetString("A61_TXT_00016");
                    textHeroName.color = CsUIData.Instance.ColorGray;
                }
                else
                {
                    textHeroName.text = csNationNoblesseInstance.HeroName;
                    textHeroName.color = CsUIData.Instance.ColorWhite;
                }

                // 관직 자체가 없음
                if (csMyHeroNationNoblesse == null)
                {
                    buttonNationNobless.transition = Selectable.Transition.None;
                    buttonNationNobless.interactable = false;
                }
                else
                {
                    CsNationNoblesseAppointmentAuthority csNationNoblesseAppointmentAuthority = csMyHeroNationNoblesse.NationNoblesseAppointmentAuthorityList.Find(a => a.NoblesseId == csMyHeroNationNoblesse.NoblesseId && a.TargetNoblesseId == nNoblessId);

                    // 관직 권한이 없음
                    if (csNationNoblesseAppointmentAuthority == null)
                    {
                        buttonNationNobless.transition = Selectable.Transition.None;
                        buttonNationNobless.interactable = false;
                    }
                    else
                    {
                        // 권한이 있음
                        buttonNationNobless.transition = Selectable.Transition.ColorTint;
                        buttonNationNobless.interactable = true;
                        buttonNationNobless.onClick.AddListener(() => OnClickOpenPopupAppoint(csNationNoblesseInstance));
                        buttonNationNobless.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationFund()
    {
        m_textPopupDonationNationFundValue.text = CsGameData.Instance.MyHeroInfo.NationFund.ToString("#,##0");
        m_textNationFundValue.text = CsGameData.Instance.MyHeroInfo.NationFund.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupDonation()
    {
        Transform trImageBackground = m_trPopupDonation.Find("ImageBackground");

        Text textOwnGold = trImageBackground.Find("ImageGold/Text").GetComponent<Text>();
        textOwnGold.text = CsGameData.Instance.MyHeroInfo.Gold.ToString("#,##0");

        Text textOwnDia = trImageBackground.Find("ImageDia/Text").GetComponent<Text>();
        textOwnDia.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");

        Text textTodayDonationCount = trImageBackground.Find("TextTodayDonation/TextValue").GetComponent<Text>();
        int nRemainingDonationCount = CsGameData.Instance.MyHeroInfo.VipLevel.NationDonationMaxCount - CsGameData.Instance.MyHeroInfo.DailyNationDonationCount;
        textTodayDonationCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.DailyNationDonationCount, CsGameData.Instance.MyHeroInfo.VipLevel.NationDonationMaxCount);

        m_textPopupDonationNationFundValue.text = CsGameData.Instance.MyHeroInfo.NationFund.ToString("#,##0");

        Transform trDonateList = trImageBackground.Find("DonationList");
        Transform trDonate = null;

        for (int i = 0; i < CsGameData.Instance.NationDonationEntryList.Count; i++)
        {
            trDonate = trDonateList.Find("Donation" + i);

            if (trDonate == null)
            {
                continue;
            }
            else
            {
                Button buttonDonate = trDonate.Find("Button").GetComponent<Button>();

                if (nRemainingDonationCount <= 0)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonDonate, false);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonDonate, true);
                }
            }
        }

        Text textExploitPoint = trImageBackground.Find("ImageExploit/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExploitPoint);
        textExploitPoint.text = CsGameData.Instance.MyHeroInfo.ExploitPoint.ToString("#,##0");

        Text textTodayExploitPoint = trImageBackground.Find("ImageExploit/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTodayExploitPoint);
        textTodayExploitPoint.text = string.Format(CsConfiguration.Instance.GetString("A61_TXT_00013"), CsGameData.Instance.MyHeroInfo.DailyExploitPoint, CsGameData.Instance.MyHeroInfo.VipLevel.DailyMaxExploitPoint);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupAppoint(int nNoblessId)
    {
        Transform trImageBackground = m_trPopupAppoint.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        textPopupName.text = string.Format(CsConfiguration.Instance.GetString("A61_TXT_00014"), CsGameData.Instance.GetNationNoblesse(nNoblessId).Name);

        InputField inputFieldHeroName = trImageBackground.Find("InputFieldHeroName").GetComponent<InputField>();
        inputFieldHeroName.text = string.Empty;

        Button buttonAppoint = trImageBackground.Find("ButtonAppoint").GetComponent<Button>();
        buttonAppoint.onClick.RemoveAllListeners();
        buttonAppoint.onClick.AddListener(() => OnClickAppoint(nNoblessId));
        buttonAppoint.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        UpdateSearchHero();
        UpdateButtonAppoint();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSearchHero()
    {
        for (int i = 0; i < m_trSearchHeroContent.childCount; i++)
        {
            m_trSearchHeroContent.GetChild(i).gameObject.SetActive(false);

            if (m_trSearchHeroContent.GetChild(i).GetComponent<Toggle>().isOn)
            {
                m_trSearchHeroContent.GetChild(i).GetComponent<Toggle>().isOn = false;
            }
        }

        m_guidSelectHeroId = System.Guid.Empty;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonAppoint()
    {
        Transform trImageBackground = m_trPopupAppoint.Find("ImageBackground");
        Button buttonAppoint = trImageBackground.Find("ButtonAppoint").GetComponent<Button>();

        if (m_guidSelectHeroId == System.Guid.Empty)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonAppoint, false);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonAppoint, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSearchHero(List<CsSearchHero> listSearchHero)
    {
        if (!m_bIsLoad)
        {
            return;
        }
        else
        {
            Transform trHeroList = m_trPopupAppoint.Find("ImageBackground/HeroList");
            Text textHeroList = trHeroList.Find("Text").GetComponent<Text>();

            for (int i = 0; i < m_trSearchHeroContent.childCount; i++)
            {
                m_trSearchHeroContent.GetChild(i).gameObject.SetActive(false);
            }

            if (listSearchHero.Count == 0)
            {
                textHeroList.text = string.Format(CsConfiguration.Instance.GetString("A61_TXT_00017"), m_strSearchHero);
                textHeroList.gameObject.SetActive(true);

                m_trSearchHeroContent.gameObject.SetActive(false);
            }
            else
            {
                textHeroList.gameObject.SetActive(false);

                Transform trSearchHero = null;

                for (int i = 0; i < listSearchHero.Count; i++)
                {
                    trSearchHero = m_trSearchHeroContent.Find("ToggleSearchHero" + i);

                    if (trSearchHero == null)
                    {
                        trSearchHero = Instantiate(m_goSearchHero, m_trSearchHeroContent).transform;
                        trSearchHero.name = "ToggleSearchHero" + i;
                    }

                    System.Guid guidHeroId = listSearchHero[i].HeroId;

                    Toggle toggleSearchHero = trSearchHero.GetComponent<Toggle>();
                    toggleSearchHero.group = m_trSearchHeroContent.GetComponent<ToggleGroup>();
                    toggleSearchHero.onValueChanged.RemoveAllListeners();
                    toggleSearchHero.onValueChanged.AddListener((ison) => OnValueChangedSelectSearchHero(ison, guidHeroId));

                    Text textHeroName = trSearchHero.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textHeroName);
                    textHeroName.text = listSearchHero[i].Name;

                    trSearchHero.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSearchHero()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNation/ToggleSearchHero");
        yield return resourceRequest;
        m_goSearchHero = (GameObject)resourceRequest.asset;
        m_bIsLoad = true;
    }
}