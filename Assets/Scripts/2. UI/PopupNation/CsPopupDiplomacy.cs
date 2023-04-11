using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupDiplomacy : CsPopupSub
{
    [SerializeField] GameObject m_goImageArrow;
    [SerializeField] GameObject m_goNationWarDate;
    [SerializeField] GameObject m_goImageNationWarItem;

    Transform m_trNationWarFrame;
    Transform m_trImageNationWarRest;
    Transform m_trImageNationWar;
    Transform m_trContent;
    Transform m_trPopupDeclaration;

    Text m_textNoNationWar;
    Text m_textNoNationWarNotice;
    Text m_textNoNationWarHistory;

    int m_nSelectNationId = 0;
    int m_nSystemNationWar = 2;

    bool m_bFirstOpenPopupNationDeclaration = true;

    enum EnNationWarCartegoryType
    {
        NationWarNotice = 0,
        NationWarHistory = 1,
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();

        CsNationWarManager.Instance.EventMyNationWarDeclaration += OnEventMyNationWarDeclaration;
        CsNationWarManager.Instance.EventNationWarDeclaration += OnEventNationWarDeclaration;
        CsNationWarManager.Instance.EventNationWarHistory += OnEventNationWarHistory;

        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccepted += OnEventNationAllianceApplicationAccepted;
        CsNationAllianceManager.Instance.EventNationAllianceBroken += OnEventNationAllianceBroken;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded += OnEventNationAllianceConcluded;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsNationWarManager.Instance.EventMyNationWarDeclaration -= OnEventMyNationWarDeclaration;
        CsNationWarManager.Instance.EventNationWarDeclaration -= OnEventNationWarDeclaration;
        CsNationWarManager.Instance.EventNationWarHistory -= OnEventNationWarHistory;

        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccepted += OnEventNationAllianceApplicationAccepted;
        CsNationAllianceManager.Instance.EventNationAllianceBroken += OnEventNationAllianceBroken;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded += OnEventNationAllianceConcluded;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventMyNationWarDeclaration()
    {
        m_trPopupDeclaration.gameObject.SetActive(false);
        UpdateImageNationWar(m_trImageNationWar);
        UpdateNationWarDeclaration();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarDeclaration(CsNationWarDeclaration csNationWarDeclaration)
    {
        UpdateImageNationWar(m_trImageNationWar);
        UpdatePopupNationWarDeclaration();
        UpdateNationWarDeclaration();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarHistory(ClientCommon.PDNationWarHistory[] nationWarHistory)
    {
        if (nationWarHistory.Length > 0)
        {
            m_textNoNationWarHistory.gameObject.SetActive(false);
            m_trContent.gameObject.SetActive(true);
            UpdateNationWarHistory(nationWarHistory);
        }
        else
        {
            m_trContent.gameObject.SetActive(false);
            m_textNoNationWarHistory.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccepted()
    {
        UpdateImageNationWar(m_trImageNationWar);
        UpdatePopupNationWarDeclaration();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceBroken()
    {
        UpdateImageNationWar(m_trImageNationWar);
        UpdatePopupNationWarDeclaration();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceConcluded(CsNationAlliance csNationAlliance)
    {
        UpdateImageNationWar(m_trImageNationWar);
        UpdatePopupNationWarDeclaration();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleCartegory(Toggle toggleCartegory, EnNationWarCartegoryType enNationWarCartegoryType)
    {
        Text text = toggleCartegory.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(text);

        if (toggleCartegory.isOn)
        {
            // 서버 오픈일 날짜 비교
            System.TimeSpan tsServerOpenTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date - CsGameData.Instance.MyHeroInfo.ServerOpenDate.Date;

            // 국가전 정비 중
            if (tsServerOpenTime.TotalDays < (CsGameData.Instance.NationWar.DeclarationAvailableServerOpenDayCount - m_nSystemNationWar))
            {
                m_textNoNationWar.gameObject.SetActive(true);
            }
            else
            {
                switch (enNationWarCartegoryType)
                {
                    case EnNationWarCartegoryType.NationWarNotice:
                        m_textNoNationWarHistory.gameObject.SetActive(false);
                        UpdateNationWarDeclaration();
                        break;
                    case EnNationWarCartegoryType.NationWarHistory:
                        m_textNoNationWarNotice.gameObject.SetActive(false);
                        CsNationWarManager.Instance.SendNationWarHistory();
                        break;
                }
            }

            text.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupNationWarDeclaration()
    {
        System.TimeSpan tsCurrentTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime - CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date;

        if (CsGameData.Instance.NationWar.DeclarationStartTime <= tsCurrentTime.TotalSeconds && tsCurrentTime.TotalSeconds <= CsGameData.Instance.NationWar.DeclarationEndTime)
        {
            if (m_bFirstOpenPopupNationDeclaration)
            {
                InitializePopupDeclaration();
                m_bFirstOpenPopupNationDeclaration = false;
            }

            //선언가능
            m_nSelectNationId = 0;
            UpdatePopupNationWarDeclaration();
            m_trPopupDeclaration.gameObject.SetActive(true);
        }
        else
        {
            //선언 불가능 시간 토스트
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A61_TXT_02004"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupNationWarDeclaration()
    {
        m_trPopupDeclaration.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleNation(bool bIson, int nNationId)
    {
        if (bIson)
        {
            m_nSelectNationId = nNationId;
            UpdatePopupNationWarDeclaration();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarDeclaration()
    {
        // 이미 선언
        if (GetIsDeclaration(CsGameData.Instance.MyHeroInfo.Nation.NationId))
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A61_TXT_02003"));
        }
        else
        {
            CsNationWarManager.Instance.SendNationWarDeclaration(m_nSelectNationId);
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        UpdateWarCalendar();

        m_trNationWarFrame = transform.Find("NationWarFrame");

        m_trPopupDeclaration = transform.Find("PopupDeclaration");

        // 국가 선포
        m_trImageNationWar = m_trNationWarFrame.Find("ImageNationWar");
        UpdateImageNationWar(m_trImageNationWar);

        Toggle toggleNationWarNotice = m_trNationWarFrame.Find("ToggleNationWarNotice").GetComponent<Toggle>();
        toggleNationWarNotice.onValueChanged.RemoveAllListeners();
        toggleNationWarNotice.onValueChanged.AddListener((bIson) => OnValueChangedToggleCartegory(toggleNationWarNotice, EnNationWarCartegoryType.NationWarNotice));

        Text textNationWarNotice = toggleNationWarNotice.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationWarNotice);
        textNationWarNotice.text = CsConfiguration.Instance.GetString("A61_BTN_00004");

        Toggle toggleNationWarHistory = m_trNationWarFrame.Find("ToggleNationWarHistory").GetComponent<Toggle>();
        toggleNationWarHistory.onValueChanged.RemoveAllListeners();
        toggleNationWarHistory.onValueChanged.AddListener((bIson) => OnValueChangedToggleCartegory(toggleNationWarHistory, EnNationWarCartegoryType.NationWarHistory));

        Text textNationWarHistory = toggleNationWarHistory.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationWarHistory);
        textNationWarHistory.text = CsConfiguration.Instance.GetString("A61_BTN_00005");

        // 
        Transform trNationWarInfo = m_trNationWarFrame.Find("NationWarInfo");
        m_trContent = trNationWarInfo.Find("Scroll View/Viewport/Content");

        Button buttonNationWarDeclaration = trNationWarInfo.Find("ButtonNationWarDeclaration").GetComponent<Button>();
        buttonNationWarDeclaration.onClick.RemoveAllListeners();

        m_textNoNationWar = m_trNationWarFrame.Find("TextNoNationWar").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoNationWar);

        m_textNoNationWarNotice = m_trNationWarFrame.Find("TextNoNationWarNotice").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoNationWarNotice);
        m_textNoNationWarNotice.text = CsConfiguration.Instance.GetString("A61_TXT_00021");

        m_textNoNationWarHistory = m_trNationWarFrame.Find("TextNoNationWarHistory").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoNationWarHistory);
        m_textNoNationWarHistory.text = CsConfiguration.Instance.GetString("A61_TXT_00022");

        m_trImageNationWarRest = transform.Find("ImageNationWarRest");

        Text textNationWarRest = m_trImageNationWarRest.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationWarRest);
        textNationWarRest.text = CsConfiguration.Instance.GetString("A61_TXT_00023");


        // 서버 오픈일 날짜 비교
        System.TimeSpan tsServerOpenTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date - CsGameData.Instance.MyHeroInfo.ServerOpenDate.Date;

        if (tsServerOpenTime.TotalDays < CsGameData.Instance.NationWar.DeclarationAvailableServerOpenDayCount)
        {
            if (tsServerOpenTime.TotalDays < CsGameData.Instance.NationWar.DeclarationAvailableServerOpenDayCount - m_nSystemNationWar)
            {
                m_textNoNationWar.text = string.Format(CsConfiguration.Instance.GetString("A61_TXT_01001"), CsGameData.Instance.NationWar.DeclarationAvailableServerOpenDayCount - tsServerOpenTime.TotalDays - m_nSystemNationWar);
                m_textNoNationWar.gameObject.SetActive(true);
            }
            // 자동 국가전 시작
            else
            {
                UpdateNationWarDeclaration();
            }
        }
        // 국가전 시작
        else
        {
            CsNationWarAvailableDayOfWeek csNationWarAvailableDayOfWeek = CsGameData.Instance.NationWar.NationWarAvailableDayOfWeekList.Find(a => a.DayOfWeek == (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek);

            // 일요일
            if (csNationWarAvailableDayOfWeek == null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                m_trImageNationWarRest.gameObject.SetActive(true);
            }
            else
            {
                UpdateNationWarDeclaration();

                //
                CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(CsGameData.Instance.MyHeroInfo.HeroId);

                if (csNationNoblesseInstance != null && csNationNoblesseInstance.NationNoblesse.NationWarDeclarationEnabled)
                {
                    Text textNationWarDeclaration = buttonNationWarDeclaration.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNationWarDeclaration);
                    textNationWarDeclaration.text = CsConfiguration.Instance.GetString("A61_BTN_00006");

                    buttonNationWarDeclaration.onClick.AddListener(OnClickOpenPopupNationWarDeclaration);

                    buttonNationWarDeclaration.gameObject.SetActive(true);
                }
                else
                {
                    buttonNationWarDeclaration.gameObject.SetActive(false);
                }

                m_trImageNationWarRest.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupDeclaration()
    {
        Transform trImageBackground = m_trPopupDeclaration.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A61_TXT_00024");

        Transform trImageNationFund = trImageBackground.Find("ImageNationFund");

        Text textNationFund = trImageNationFund.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationFund);
        textNationFund.text = CsConfiguration.Instance.GetString("A61_TXT_00025");

        Text textNationFundValue = trImageNationFund.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationFundValue);
        textNationFundValue.text = CsGameData.Instance.MyHeroInfo.NationFund.ToString("#,##0");

        Button buttonExit = trImageBackground.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(OnClickClosePopupNationWarDeclaration);

        Transform trImageNationWar = trImageBackground.Find("ImageNationWar");
        UpdateImageNationWar(trImageNationWar);

        Transform trImageDeclaration = trImageBackground.Find("ImageDeclaration");
        Transform trDeclarationInfo = trImageDeclaration.Find("DeclarationInfo");

        Text textRequiredNationFund = trDeclarationInfo.Find("RequiredNationFund/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRequiredNationFund);
        textRequiredNationFund.text = CsConfiguration.Instance.GetString("A61_TXT_00026");

        Text textRequiredNationFundValue = trDeclarationInfo.Find("RequiredNationFund/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRequiredNationFundValue);
        textRequiredNationFundValue.text = CsGameData.Instance.NationWar.DeclarationRequiredNationFund.ToString("#,##0");

        Text textDeclaration = trDeclarationInfo.Find("DeclarationCount/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDeclaration);
        textDeclaration.text = CsConfiguration.Instance.GetString("A61_TXT_00027");

        Text textDeclarationValue = trDeclarationInfo.Find("DeclarationCount/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDeclarationValue);
        int nRemainingCount = CsGameData.Instance.NationWar.WeeklyDeclarationMaxCount - CsNationWarManager.Instance.WeeklyNationWarDeclarationCount;
        textDeclarationValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingCount, CsGameData.Instance.NationWar.WeeklyDeclarationMaxCount);

        Text textDeclarationCount = trDeclarationInfo.Find("DeclarationCount/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDeclarationCount);
        textDeclarationCount.text = CsConfiguration.Instance.GetString("A61_TXT_00031");

        Button buttonDeclaration = trDeclarationInfo.Find("ButtonDeclaration").GetComponent<Button>();
        buttonDeclaration.onClick.RemoveAllListeners();
        buttonDeclaration.onClick.AddListener(OnClickNationWarDeclaration);

        Text textButtonDeclaration = buttonDeclaration.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonDeclaration);
        textButtonDeclaration.text = CsConfiguration.Instance.GetString("A61_BTN_00006");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWarCalendar()
    {
        Transform trWarCalendar = transform.Find("WarCalendar");
        Transform trToggleWeek = null;
        Transform trImageIcon = null;

        Text textWeek = null;
        Text textWar = null;

        Toggle toggleWeek = null;

        for (int i = 0; i < trWarCalendar.childCount; i++)
        {
            trToggleWeek = trWarCalendar.Find("Toggle" + i);

            if (trToggleWeek == null)
            {
                continue;
            }
            else
            {
                int nIndex = i;

                textWeek = trToggleWeek.Find("TextWeek").GetComponent<Text>();
                CsUIData.Instance.SetFont(textWeek);

                textWar = trToggleWeek.Find("TextWar").GetComponent<Text>();
                CsUIData.Instance.SetFont(textWar);

                trImageIcon = trToggleWeek.Find("ImageIcon");

                switch ((System.DayOfWeek)i)
                {
                    case System.DayOfWeek.Friday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00036");
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00040");
                        break;

                    case System.DayOfWeek.Monday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00032");
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00040");
                        break;

                    case System.DayOfWeek.Saturday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00037");
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00040");
                        break;

                    case System.DayOfWeek.Sunday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00038");
                        // 휴전
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00039");
                        break;

                    case System.DayOfWeek.Thursday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00035");
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00040");
                        break;

                    case System.DayOfWeek.Tuesday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00033");
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00040");
                        break;

                    case System.DayOfWeek.Wednesday:
                        textWeek.text = CsConfiguration.Instance.GetString("A61_TXT_00034");
                        textWar.text = CsConfiguration.Instance.GetString("A61_TXT_00040");
                        break;
                }

                toggleWeek = trToggleWeek.GetComponent<Toggle>();
                toggleWeek.onValueChanged.RemoveAllListeners();

                if ((int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == i)
                {
                    toggleWeek.isOn = true;
                    trImageIcon.gameObject.SetActive(true);

                    textWeek.color = new Color32(246, 231, 180, 255);
                    textWar.color = new Color32(246, 231, 180, 255);
                }
                else
                {
                    toggleWeek.isOn = false;
                    trImageIcon.gameObject.SetActive(false);

                    textWeek.color = CsUIData.Instance.ColorGray;
                    textWar.color = CsUIData.Instance.ColorGray;
                }

                toggleWeek.onValueChanged.AddListener((ison) => OnValueChangedWarCalendar(ison, nIndex));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedWarCalendar(bool bIson, int nIndex)
    {
        if (bIson)
        {
            if ((System.DayOfWeek)nIndex == System.DayOfWeek.Sunday)
            {
                m_trNationWarFrame.gameObject.SetActive(false);
                m_trImageNationWarRest.gameObject.SetActive(true);
            }
            else
            {
                m_trImageNationWarRest.gameObject.SetActive(false);
                m_trNationWarFrame.gameObject.SetActive(true);
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageNationWar(Transform trImageNationWar)
    {
        Transform trNation = null;

        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            trNation = trImageNationWar.Find("ToggleNation" + CsGameData.Instance.NationList[i].NationId);

            if (trNation == null)
            {
                continue;
            }
            else
            {
                CsNation csNation = CsGameData.Instance.NationList[i];

                Text textNation = trNation.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNation);
                textNation.text = csNation.Name;

                Image imageIcon = trNation.Find("Image").GetComponent<Image>();

                // 본국 아이콘 표시
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == csNation.NationId)
                {
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNation/ico_world_home");
                    imageIcon.gameObject.SetActive(true);
                }
                else if (csNation.NationId == CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId))
                {
                    // 동맹 표시
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNation/ico_world_alliance");
                    imageIcon.gameObject.SetActive(true);
                }
                else
                {
                    imageIcon.gameObject.SetActive(false);
                }

                // 본국은 클릭을 못함
                Toggle toggleNation = trNation.GetComponent<Toggle>();
                if (csNation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                {
                    toggleNation.interactable = false;
                }

                toggleNation.onValueChanged.RemoveAllListeners();
                toggleNation.onValueChanged.AddListener((bIson) => OnValueChangedToggleNation(bIson, csNation.NationId));
            }
        }

        // 화살표 제작
        if (CsNationWarManager.Instance.NationWarDeclarationList.Count > 0)
        {
            for (int i = 0; i < CsNationWarManager.Instance.NationWarDeclarationList.Count; i++)
            {
                int nNationIdOffense = CsNationWarManager.Instance.NationWarDeclarationList[i].NationId;
                int nNationIdDefense = CsNationWarManager.Instance.NationWarDeclarationList[i].TargetNationId;

                Transform trImageArrow = trImageNationWar.Find("ImageArrow" + i);

                if (trImageArrow == null)
                {
                    trImageArrow = Instantiate(m_goImageArrow, trImageNationWar).transform;
                    trImageArrow.name = "ImageArrow" + i;
                }

                Transform trNationOffense = m_trImageNationWar.Find("ToggleNation" + nNationIdOffense);
                Transform trNationDefense = m_trImageNationWar.Find("ToggleNation" + nNationIdDefense);

                RectTransform rectTransformImageArrow = trImageArrow.GetComponent<RectTransform>();
                rectTransformImageArrow.localPosition = (trNationDefense.GetComponent<RectTransform>().localPosition + trNationOffense.GetComponent<RectTransform>().localPosition) * 0.5f;

                // 화살표 사이즈 조정
                if (Mathf.Abs(nNationIdOffense - nNationIdDefense) == 5)
                {
                    rectTransformImageArrow.sizeDelta = new Vector2(100, 50);
                }
                else if (Mathf.Abs(nNationIdOffense - nNationIdDefense) == 1)
                {
                    if ((nNationIdOffense == 3 && nNationIdDefense == 4) || (nNationIdOffense == 4 && nNationIdDefense == 3))
                    {
                        rectTransformImageArrow.sizeDelta = new Vector2(100, 50);
                    }
                    else
                    {
                        rectTransformImageArrow.sizeDelta = new Vector2(70, 50);

                        if (rectTransformImageArrow.localPosition.x > 0)
                        {
                            rectTransformImageArrow.localPosition = new Vector3(rectTransformImageArrow.localPosition.x + 15, rectTransformImageArrow.localPosition.y, rectTransformImageArrow.localPosition.z);
                        }
                        else
                        {
                            rectTransformImageArrow.localPosition = new Vector3(rectTransformImageArrow.localPosition.x - 15, rectTransformImageArrow.localPosition.y, rectTransformImageArrow.localPosition.z);
                        }

                        if (rectTransformImageArrow.localPosition.y > 0)
                        {
                            rectTransformImageArrow.localPosition = new Vector3(rectTransformImageArrow.localPosition.x, rectTransformImageArrow.localPosition.y + 15, rectTransformImageArrow.localPosition.z);
                        }
                        else
                        {
                            rectTransformImageArrow.localPosition = new Vector3(rectTransformImageArrow.localPosition.x, rectTransformImageArrow.localPosition.y - 15, rectTransformImageArrow.localPosition.z);
                        }
                    }
                }
                else if (Mathf.Abs(nNationIdOffense - nNationIdDefense) == 2 || Mathf.Abs(nNationIdOffense - nNationIdDefense) == 4)
                {
                    rectTransformImageArrow.sizeDelta = new Vector2(200, 50);
                }
                else if (Mathf.Abs(nNationIdOffense - nNationIdDefense) == 3)
                {
                    rectTransformImageArrow.sizeDelta = new Vector2(260, 50);
                }

                float fAngle = Vector3.Angle(Vector3.left, trNationDefense.position - trNationOffense.position);

                if (trNationDefense.position.y >= trNationOffense.position.y)
                {
                    rectTransformImageArrow.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.PI * Mathf.Rad2Deg - fAngle));
                }
                else
                {
                    rectTransformImageArrow.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.PI * Mathf.Rad2Deg + fAngle));
                }
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarDeclaration()
    {
        m_trContent.gameObject.SetActive(true);

        Transform trNationWarDate = null;
        // 초기화
        for (int i = 0; i < m_trContent.childCount; i++)
        {
            trNationWarDate = m_trContent.GetChild(i);
            trNationWarDate.gameObject.SetActive(false);

            for (int j = 0; j < trNationWarDate.childCount; j++)
            {
                trNationWarDate.GetChild(j).gameObject.SetActive(false);
            }
        }

        if (CsNationWarManager.Instance.NationWarDeclarationList.Count > 0)
        {
            m_textNoNationWarNotice.gameObject.SetActive(false);

            for (int i = 0; i < CsNationWarManager.Instance.NationWarDeclarationList.Count; i++)
            {
                trNationWarDate = m_trContent.Find("NationWarDate" + i);

                if (trNationWarDate == null)
                {
                    trNationWarDate = Instantiate(m_goNationWarDate, m_trContent).transform;
                    trNationWarDate.name = "NationWarDate" + i;
                }

                Text textNationWarDate = trNationWarDate.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNationWarDate);
                System.DateTimeOffset dtOffset = CsNationWarManager.Instance.NationWarDeclarationList[i].Time.Date.AddSeconds(CsGameData.Instance.NationWar.StartTime);
                textNationWarDate.text = dtOffset.ToString("yyyy/MM/dd HH:mm");

                Transform trImageNationWarItem = trNationWarDate.Find("ImageNationWarItem");

                if (trImageNationWarItem == null)
                {
                    trImageNationWarItem = Instantiate(m_goImageNationWarItem, trNationWarDate).transform;
                    trImageNationWarItem.name = "ImageNationWarItem";
                }

                Text textNationOffense = trImageNationWarItem.Find("TextNationOffense").GetComponent<Text>();
                Text textNationDefense = trImageNationWarItem.Find("TextNationDefense").GetComponent<Text>();

                CsUIData.Instance.SetFont(textNationOffense);
                CsUIData.Instance.SetFont(textNationDefense);

                textNationOffense.text = CsGameData.Instance.GetNation(CsNationWarManager.Instance.NationWarDeclarationList[i].NationId).Name;
                textNationDefense.text = CsGameData.Instance.GetNation(CsNationWarManager.Instance.NationWarDeclarationList[i].TargetNationId).Name;

                trNationWarDate.gameObject.SetActive(true);
                trNationWarDate.Find("Text").gameObject.SetActive(true);
                trImageNationWarItem.gameObject.SetActive(true);
            }
        }
        else
        {
            m_textNoNationWarNotice.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarHistory(ClientCommon.PDNationWarHistory[] arrNationWarHistory)
    {
        // 초기화
        for (int i = 0; i < m_trContent.childCount; i++)
        {
            Transform trNationWarDate = m_trContent.GetChild(i);
            trNationWarDate.gameObject.SetActive(false);

            for (int j = 0; j < trNationWarDate.childCount; j++)
            {
                trNationWarDate.GetChild(j).gameObject.SetActive(false);
            }
        }

        List<ClientCommon.PDNationWarHistory> listNationWarHistory = new List<ClientCommon.PDNationWarHistory>(arrNationWarHistory);
        List<System.DateTime> listDtTime = new List<System.DateTime>();

        for (int i = 0; i < arrNationWarHistory.Length; i++)
        {
            System.DateTime dtTime = listDtTime.Find(a => a.Date.CompareTo(arrNationWarHistory[i].date.Date) == 0);

            // 같은 날짜가 없음
            if (dtTime == default(System.DateTime))
            {
                listDtTime.Add(arrNationWarHistory[i].date);
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < listDtTime.Count; i++)
        {
            // 날짜 표시
            Transform trNationWarDate = m_trContent.Find("NationWarDate" + i);

            if (trNationWarDate == null)
            {
                trNationWarDate = Instantiate(m_goNationWarDate, m_trContent).transform;
                trNationWarDate.name = "NationWarDate" + i;
            }

            Text textNationWarDate = trNationWarDate.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationWarDate);
            textNationWarDate.text = listDtTime[i].ToString("yyyy/MM/dd");
            textNationWarDate.gameObject.SetActive(true);

            // 날짜 안의 국가전 승패 표시
            List<ClientCommon.PDNationWarHistory> list = new List<ClientCommon.PDNationWarHistory>();
            list = listNationWarHistory.FindAll(a => a.date.Date.CompareTo(listDtTime[i].Date) == 0);

            for (int j = 0; j < list.Count; j++)
            {
                CsNation csNationOffense = CsGameData.Instance.GetNation(list[j].offenseNationId);
                CsNation csNationDefense = CsGameData.Instance.GetNation(list[j].defenseNationId);

                if (csNationOffense == null || csNationDefense == null)
                {
                    continue;
                }
                else
                {
                    // 승리 국가 패배 국가 표시
                    Transform trImageNationWarItem = trNationWarDate.Find("ImageNationWarItem" + j);

                    if (trImageNationWarItem == null)
                    {
                        trImageNationWarItem = Instantiate(m_goImageNationWarItem, trNationWarDate).transform;
                        trImageNationWarItem.name = "ImageNationWarItem" + j;
                    }

                    Image imageOffense = trImageNationWarItem.Find("ImageNationOffense").GetComponent<Image>();
                    Image imageDefense = trImageNationWarItem.Find("ImageNationDefense").GetComponent<Image>();

                    Text textNationOffense = trImageNationWarItem.Find("TextNationOffense").GetComponent<Text>();
                    Text textNationDefense = trImageNationWarItem.Find("TextNationDefense").GetComponent<Text>();

                    CsUIData.Instance.SetFont(textNationOffense);
                    CsUIData.Instance.SetFont(textNationDefense);

                    textNationOffense.text = csNationOffense.Name;
                    textNationDefense.text = csNationDefense.Name;

                    if (csNationOffense.NationId == list[j].winNationId)
                    {
                        imageOffense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNation/ico_world_win");
                    }
                    else
                    {
                        imageOffense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNation/ico_world_lose");
                    }

                    if (csNationDefense.NationId == list[j].winNationId)
                    {
                        imageDefense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNation/ico_world_win");
                    }
                    else
                    {
                        imageDefense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNation/ico_world_lose");
                    }

                    imageOffense.gameObject.SetActive(true);
                    imageDefense.gameObject.SetActive(true);

                    trImageNationWarItem.gameObject.SetActive(true);
                }
            }

            trNationWarDate.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupNationWarDeclaration()
    {
        Transform trImageBackground = m_trPopupDeclaration.Find("ImageBackground");

        Transform trImageNationWar = trImageBackground.Find("ImageNationWar");
        UpdateImageNationWar(trImageNationWar);

        Transform trImageDeclaration = trImageBackground.Find("ImageDeclaration");

        Text textNoSelectNation = trImageBackground.Find("TextNoSelectNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoSelectNation);
        textNoSelectNation.text = CsConfiguration.Instance.GetString("A61_TXT_00030");

        if (m_nSelectNationId == 0)
        {
            for (int i = 0; i < trImageNationWar.childCount; i++)
            {
                Toggle toggleNationWar = trImageNationWar.GetChild(i).GetComponent<Toggle>();

                if (toggleNationWar == null)
                {
                    continue;
                }
                else
                {
                    if (toggleNationWar.isOn)
                    {
                        toggleNationWar.isOn = false;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            trImageDeclaration.gameObject.SetActive(false);
            textNoSelectNation.gameObject.SetActive(true);
        }
        else
        {
            Image imageNation = trImageDeclaration.Find("ImageNation").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + m_nSelectNationId);

            Text textNationName = trImageDeclaration.Find("ImageNationName/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationName);
            textNationName.text = CsGameData.Instance.GetNation(m_nSelectNationId).Name;

            Transform trDeclarationInfo = trImageDeclaration.Find("DeclarationInfo");

            Text textDesc = trImageDeclaration.Find("TextDesc").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDesc);
            //

            // 선언 되어 있음
            if (GetIsDeclaration(m_nSelectNationId))
            {
                trDeclarationInfo.gameObject.SetActive(false);
                textDesc.gameObject.SetActive(true);
                textDesc.text = CsConfiguration.Instance.GetString("A61_TXT_00029");
            }
            else
            {
                textDesc.gameObject.SetActive(false);
                trDeclarationInfo.gameObject.SetActive(true);

                Button buttonDeclaration = trDeclarationInfo.Find("ButtonDeclaration").GetComponent<Button>();

                Text textRequiredNationFund = trDeclarationInfo.Find("RequiredNationFund/TextValue").GetComponent<Text>();
                Text textDeclarationCount = trDeclarationInfo.Find("DeclarationCount/TextValue").GetComponent<Text>();

                CsUIData.Instance.SetFont(textRequiredNationFund);
                CsUIData.Instance.SetFont(textDeclarationCount);

                int nRemainigDeclarationCount = CsGameData.Instance.NationWar.WeeklyDeclarationMaxCount - CsNationWarManager.Instance.WeeklyNationWarDeclarationCount;
                textDeclarationCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainigDeclarationCount, CsGameData.Instance.NationWar.WeeklyDeclarationMaxCount);

                bool bInteractable = true;

                if (CsGameData.Instance.NationWar.DeclarationRequiredNationFund <= CsGameData.Instance.MyHeroInfo.NationFund)
                {
                    textRequiredNationFund.color = CsUIData.Instance.ColorWhite;
                }
                else
                {
                    textRequiredNationFund.color = CsUIData.Instance.ColorRed;
                    bInteractable = false;
                }

                if (0 < nRemainigDeclarationCount)
                {
                    textDeclarationCount.color = CsUIData.Instance.ColorWhite;
                }
                else
                {
                    textDeclarationCount.color = CsUIData.Instance.ColorRed;
                    bInteractable = false;
                }

                // 동맹 확인
                if (m_nSelectNationId == CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId))
                {
                    bInteractable = false;
                }
                else
                {

                }

                CsUIData.Instance.DisplayButtonInteractable(buttonDeclaration, bInteractable);
            }

            textNoSelectNation.gameObject.SetActive(false);
            trImageDeclaration.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool GetIsDeclaration(int nNationId)
    {
        for (int i = 0; i < CsNationWarManager.Instance.NationWarDeclarationList.Count; i++)
        {
            if (CsNationWarManager.Instance.NationWarDeclarationList[i].NationId == nNationId || CsNationWarManager.Instance.NationWarDeclarationList[i].TargetNationId == nNationId)
            {
                return true;
            }
            else
            {
                continue;
            }
        }

        return false;
    }
}