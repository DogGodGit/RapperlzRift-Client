using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationWarInfo : CsPopupSub
{
    Transform m_trImageBgNationWar;
    Transform m_trPopupNationwarTransmission;

    Text m_textNationWarTimer;
    Text m_textFreeNationWarTransmission;

    List<ClientCommon.PDSimpleNationWarMonsterInstance> m_listSimpleNationWarMonsterInstance = new List<ClientCommon.PDSimpleNationWarMonsterInstance>();

    int m_nSelectMonsterArrangeId = 0;

    bool m_bFirst = true;
    bool m_bPopupFirst = true;
    bool m_bConvergingAttack = false;
    //bool m_bRevivalPointOpen = false;

    float m_flTime = 0.0f;
    float m_flRemainingNationWarTime = 0.0f;
    float m_flNationWarCallCoolTime = 0.0f;
    float m_flNationWarConvergingAttackCoolTime = 0.0f;

    // 국가전 몬스터 타입
    enum EnNationWarMonsterType
    {
        NationWarCommander = 1,
        NationWarWizard = 2,
        NationWarAngel = 3,
        NationWarDragon = 4,
        NationWarRock = 5,
    }

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsNationWarManager.Instance.EventNationWarInfo += OnEventNationWarInfo;

        // 국가전 소집
        CsNationWarManager.Instance.EventMyNationWarCall += OnEventMyNationWarCall;
        CsNationWarManager.Instance.EventNationWarCall += OnEventNationWarCall;

        // 국가전 전송
        CsNationWarManager.Instance.EventContinentEnterForNationWarTransmission += OnEventContinentEnterForNationWarTransmission;

        // 집중 공격
        CsNationWarManager.Instance.EventMyNationWarConvergingAttack += OnEventMyNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttack += OnEventNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttackFinished += OnEventNationWarConvergingAttackFinished;

    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        CsNationWarManager.Instance.SendNationWarInfo();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsNationWarManager.Instance.EventNationWarInfo -= OnEventNationWarInfo;

        // 국가전 소집
        CsNationWarManager.Instance.EventMyNationWarCall -= OnEventMyNationWarCall;
        CsNationWarManager.Instance.EventNationWarCall -= OnEventNationWarCall;

        // 국가전 전송
        CsNationWarManager.Instance.EventContinentEnterForNationWarTransmission -= OnEventContinentEnterForNationWarTransmission;

        CsNationWarManager.Instance.EventMyNationWarConvergingAttack -= OnEventMyNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttack -= OnEventNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttackFinished -= OnEventNationWarConvergingAttackFinished;

    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + 1.0f < Time.time)
        {
            UpdateNationWarButton();

            if ((m_flRemainingNationWarTime - Time.realtimeSinceStartup) > 0.0f)
            {
                UpdateNationWarTimer();
            }

            m_flTime = Time.time;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarInfo(ClientCommon.PDSimpleNationWarMonsterInstance[] arrSimpleNationWarMonsterInstance)
    {
        m_listSimpleNationWarMonsterInstance.Clear();
        m_listSimpleNationWarMonsterInstance.AddRange(arrSimpleNationWarMonsterInstance);

        UpdateNationWarMonster();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyNationWarCall()
    {
        m_flNationWarCallCoolTime = CsNationWarManager.Instance.NationWarCallRemainingCoolTime + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarCall(System.Guid guidCallerId, string strCallerName, int nCallerNoblesseId, System.DateTime dtCall)
    {
        m_flNationWarCallCoolTime = CsNationWarManager.Instance.NationWarCallRemainingCoolTime + Time.realtimeSinceStartup;
    }

    void OnEventContinentEnterForNationWarTransmission(ClientCommon.PDContinentEntranceInfo continentEntranceInfo)
    {
        UpdateFreeNationWarTransmission();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyNationWarConvergingAttack()
    {
        m_flNationWarConvergingAttackCoolTime = CsNationWarManager.Instance.NationWarConvergingAttackRemainingCoolTime + Time.realtimeSinceStartup;
        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarConvergingAttack(int nMonsterArrangeId)
    {
        m_flNationWarConvergingAttackCoolTime = CsNationWarManager.Instance.NationWarConvergingAttackRemainingCoolTime + Time.realtimeSinceStartup;
        UpdateNationWarConvergingAttack(nMonsterArrangeId);

        m_bConvergingAttack = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarConvergingAttackFinished()
    {
        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarTransmission(int nArrangeId)
    {
        ClientCommon.PDSimpleNationWarMonsterInstance simpleNationWarMonsterInstance = CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance.Find(a => a.monsterArrangeId == nArrangeId);
        CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(nArrangeId);

        if (csNationWarMonsterArrange != null)
        {
            // 위자드 타입 전송
            if (csNationWarMonsterArrange.Type == (int)EnNationWarMonsterType.NationWarWizard)
            {
                if (simpleNationWarMonsterInstance != null)
                {
                    if (simpleNationWarMonsterInstance.nationId != CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        // 전송 불가
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A70_TXT_02001"));
                        CsNationWarManager.Instance.SendNationWarInfo();
                    }
                    else
                    {
                        // 전송
                        NationWarTransmission(nArrangeId);
                    }
                }
            }
            else
            {
                // 점령
                if (simpleNationWarMonsterInstance == null)
                {
                    // 공격
                    if (CsNationWarManager.Instance.MyNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        // 전송
                        NationWarTransmission(nArrangeId);
                    }
                    else
                    {
                        // 전송 불가
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A70_TXT_02001"));
                        CsNationWarManager.Instance.SendNationWarInfo();
                    }
                }
                else
                {
                    // 수비
                    if (CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        // 전송
                        NationWarTransmission(nArrangeId);
                    }
                    else
                    {
                        // 전송 불가
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A70_TXT_02001"));
                        CsNationWarManager.Instance.SendNationWarInfo();
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMoveToNationWarMonster(int nArrangedId)
    {
        if (m_bConvergingAttack)
        {
            CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId = nArrangedId;
            CsNationWarManager.Instance.SendNationWarConvergingAttack(nArrangedId);

            m_bConvergingAttack = false;
        }
        else
        {
            CsNationWarManager.Instance.MoveToNationWarMonster(nArrangedId);
            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.NationWar);
            CsGameEventUIToUI.Instance.OnEventPopupClose();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarCall()
    {
        CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.MyNationWarDeclaration;

        if (csContinent != null && csNationWarDeclaration != null && CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            if (csContinent.IsNationWarTarget && CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == csNationWarDeclaration.TargetNationId)
            {
                CsNationWarManager.Instance.SendNationWarCall();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A70_TXT_02010"));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarConvergingAttack()
    {
        m_bConvergingAttack = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupCancel()
    {
        m_trPopupNationwarTransmission.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupConfirm()
    {
        int nDailyNationWarPaidTransmissionCount = 0;

        if (CsNationWarManager.Instance.DailyNationWarPaidTransmissionCount < CsGameData.Instance.NationWar.NationWarPaidTransmissionList.Count)
        {
            nDailyNationWarPaidTransmissionCount = CsNationWarManager.Instance.DailyNationWarPaidTransmissionCount;
        }
        else
        {
            nDailyNationWarPaidTransmissionCount = CsGameData.Instance.NationWar.NationWarPaidTransmissionList.Count - 1;
        }

        if (CsGameData.Instance.NationWar.NationWarPaidTransmissionList[nDailyNationWarPaidTransmissionCount].RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia)
        {
            CsNationWarManager.Instance.SendNationWarTransmission(m_nSelectMonsterArrangeId);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_TXT_02006"));
        }

        m_trPopupNationwarTransmission.gameObject.SetActive(false);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPopupNationwarTransmission = transform.Find("PopupPaidNationWarTransmission");
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        // 국가전 타이머 표시
        m_textNationWarTimer = transform.transform.Find("ImageTimer/TextTimer").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNationWarTimer);

        System.TimeSpan tsNationWarRemainingTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.AddSeconds(CsGameData.Instance.NationWar.EndTime).Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime);
        m_flRemainingNationWarTime = (float)tsNationWarRemainingTime.TotalSeconds + Time.realtimeSinceStartup;
        tsNationWarRemainingTime = System.TimeSpan.FromSeconds(m_flRemainingNationWarTime - Time.realtimeSinceStartup);
        m_textNationWarTimer.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsNationWarRemainingTime.Minutes.ToString("0#"), tsNationWarRemainingTime.Seconds.ToString("0#"));

        m_trImageBgNationWar = transform.Find("ImageBgNationWar");

        // 공격 수비 진영 표시
        Image imageNationWar = m_trImageBgNationWar.Find("ImageNationWar").GetComponent<Image>();

        Text textNationWar = imageNationWar.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationWar);

        if (CsGameData.Instance.MyHeroInfo.Nation.NationId == csNationWarDeclaration.NationId)
        {
            imageNationWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_attack");
            textNationWar.text = CsConfiguration.Instance.GetString("A70_TXT_00004");
        }
        else if (CsGameData.Instance.MyHeroInfo.Nation.NationId == csNationWarDeclaration.TargetNationId)
        {
            imageNationWar.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_defence");
            textNationWar.text = CsConfiguration.Instance.GetString("A70_TXT_00005");
        }

        // 전송 횟수
        m_textFreeNationWarTransmission = m_trImageBgNationWar.Find("TextFreeTransmission").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textFreeNationWarTransmission);
        int nRemainingFreeTransmission = CsGameData.Instance.NationWar.FreeTransmissionCount - CsNationWarManager.Instance.DailyNationWarFreeTransmissionCount;
        m_textFreeNationWarTransmission.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_00006"), nRemainingFreeTransmission, CsGameData.Instance.NationWar.FreeTransmissionCount);

        Text textFreeTransmissionDescription = m_textFreeNationWarTransmission.transform.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textFreeTransmissionDescription);
        textFreeTransmissionDescription.text = CsConfiguration.Instance.GetString("A70_TXT_00007");

        // 몬스터 정보 표시
        CsNationWarManager.Instance.SendNationWarInfo();

        // 집중공격 버튼
        m_flNationWarCallCoolTime = CsNationWarManager.Instance.NationWarCallRemainingCoolTime + Time.realtimeSinceStartup;
        m_flNationWarConvergingAttackCoolTime = CsNationWarManager.Instance.NationWarConvergingAttackRemainingCoolTime + Time.realtimeSinceStartup;

        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);

        // 국가 소집 / 집중 공격 버튼
        UpdateNationWarButton();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarTimer()
    {
        System.TimeSpan tsNationWarRemainingTime = System.TimeSpan.FromSeconds(m_flRemainingNationWarTime - Time.realtimeSinceStartup);
        m_textNationWarTimer.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsNationWarRemainingTime.Minutes.ToString("0#"), tsNationWarRemainingTime.Seconds.ToString("0#"));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateFreeNationWarTransmission()
    {
        int nRemainingFreeTransmission = CsGameData.Instance.NationWar.FreeTransmissionCount - CsNationWarManager.Instance.DailyNationWarFreeTransmissionCount;
        m_textFreeNationWarTransmission.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_00006"), nRemainingFreeTransmission, CsGameData.Instance.NationWar.FreeTransmissionCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarMonster()
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        for (int i = 0; i < CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Count; i++)
        {
            CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i];
            Transform trImagePosition = null;

            trImagePosition = m_trImageBgNationWar.Find("ImagePosition" + csNationWarMonsterArrange.ArrangeId);

            if (csNationWarMonsterArrange.Type == (int)EnNationWarMonsterType.NationWarCommander)
            {
                List<ClientCommon.PDSimpleNationWarMonsterInstance> listNationWarMonsterWizard = m_listSimpleNationWarMonsterInstance.FindAll(a => CsGameData.Instance.NationWar.GetNationWarMonsterArrange(a.monsterArrangeId).Type == (int)EnNationWarMonsterType.NationWarWizard && a.nationId == csNationWarDeclaration.NationId);
                int nCount = listNationWarMonsterWizard.Count;

                Text textDamage = trImagePosition.Find("TextDamage").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDamage);
                textDamage.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_01001"), (100 - nCount * 50));
            }
            
            // 몬스터 이름
            Text textMonsterName = trImagePosition.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textMonsterName);

            CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(CsGameData.Instance.GetMonsterArrange(csNationWarMonsterArrange.MonsterArrangeId).MonsterId);
            textMonsterName.text = csMonsterInfo.Name;

            Transform trNationWarMonsterInfo = trImagePosition.Find("NationWarMonsterInfo");

            Image imageNation = trNationWarMonsterInfo.Find("Image").GetComponent<Image>();
            ClientCommon.PDSimpleNationWarMonsterInstance simpleNationWarMonsterInstance = m_listSimpleNationWarMonsterInstance.Find(a => a.monsterArrangeId == csNationWarMonsterArrange.ArrangeId);

            Image imageButton = trImagePosition.Find("ButtonMove").GetComponent<Image>();

            // 전송 버튼
            Transform trButtonTransmission = trImagePosition.Find("ButtonTransmission");

            if (trButtonTransmission != null)
            {
                Button buttonTransmission = trButtonTransmission.GetComponent<Button>();
                buttonTransmission.onClick.RemoveAllListeners();
                buttonTransmission.onClick.AddListener(() => OnClickNationWarTransmission(csNationWarMonsterArrange.ArrangeId));
                buttonTransmission.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textButtonTransmission = trButtonTransmission.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButtonTransmission);
                textButtonTransmission.text = CsConfiguration.Instance.GetString("A70_TXT_00008");
            }

            if (simpleNationWarMonsterInstance != null)
            {
                if (csNationWarMonsterArrange.Type == (int)EnNationWarMonsterType.NationWarRock)
                {
                    // 수비측
                    imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_occupation_on");
                    imageButton.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_position_7_on");
                }
                else
                {
                    // 수비측
                    if (csNationWarDeclaration.TargetNationId == simpleNationWarMonsterInstance.nationId)
                    {
                        imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_mini_world_war_defence");
                    }
                    // 공격측
                    else
                    {
                        imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_mini_world_war_attack");
                    }

                    // 점령한 곳만 전송 버튼 표시
                    if (simpleNationWarMonsterInstance.nationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        trButtonTransmission.gameObject.SetActive(true);
                    }
                    else
                    {
                        trButtonTransmission.gameObject.SetActive(false);
                    }
                }

                // Hp 표시
                Slider sliderMonsterHp = trNationWarMonsterInfo.Find("Slider").GetComponent<Slider>();
                sliderMonsterHp.maxValue = csMonsterInfo.MaxHp;
                sliderMonsterHp.value = simpleNationWarMonsterInstance.monsterHp;
            }
            else
            {
                if (csNationWarMonsterArrange.Type == (int)EnNationWarMonsterType.NationWarRock)
                {
                    // 점령
                    imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_occupation_off");
                    imageButton.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_position_7_off");
                }
                else
                {
                    // 점령
                    imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_mini_world_war_attack");

                    if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        trButtonTransmission.gameObject.SetActive(true);
                    }
                    else
                    {
                        trButtonTransmission.gameObject.SetActive(false);
                    }
                }

                // Hp 표시
                Slider sliderMonsterHp = trNationWarMonsterInfo.Find("Slider").GetComponent<Slider>();
                sliderMonsterHp.maxValue = csMonsterInfo.MaxHp;
                sliderMonsterHp.value = 0.0f;
            }

            // 자동 이동 버튼
            Button buttonMove = trImagePosition.Find("ButtonMove").GetComponent<Button>();
            buttonMove.onClick.RemoveAllListeners();
            buttonMove.onClick.AddListener(() => OnClickMoveToNationWarMonster(csNationWarMonsterArrange.ArrangeId));
            buttonMove.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            // 부활 장소 표기
            CsNationWarRevivalPointActivationCondition csNationWarRevivalPointActivationCondition = csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Find(a => a.ArrangeId == csNationWarMonsterArrange.ArrangeId);

            if (csNationWarRevivalPointActivationCondition != null)
            {
                Transform trTextRevivalPoint = trImagePosition.Find("TextRevivalPoint");

                if (trTextRevivalPoint != null)
                {
                    Text textRevivalPoint = trTextRevivalPoint.GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRevivalPoint);
                    textRevivalPoint.text = csNationWarRevivalPointActivationCondition.RevivalPoint.Name;
                }
            }
        }

        UpdateAllianceRevivalPoint();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarConvergingAttack(int nMonsterArrangeId)
    {
        Image imageConvergingAttack = m_trImageBgNationWar.Find("ImageConvergingAttack").GetComponent<Image>();

        if (nMonsterArrangeId == 0)
        {
            imageConvergingAttack.gameObject.SetActive(false);
        }
        else
        {
            Transform trImagePosition = null;

            for (int i = 0; i < CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Count; i++)
            {
                CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i];
                trImagePosition = m_trImageBgNationWar.Find("ImagePosition" + csNationWarMonsterArrange.ArrangeId);

                if (csNationWarMonsterArrange.ArrangeId == nMonsterArrangeId)
                {
                    imageConvergingAttack.transform.localPosition = new Vector3(trImagePosition.localPosition.x, trImagePosition.localPosition.y + 15, trImagePosition.localPosition.z);
                    break;
                }
            }

            imageConvergingAttack.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarButton()
    {
        Transform trNationButtons = transform.Find("NationButtons");
        CsNationNoblesseInstance csNationNoblesseInstance = CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(CsGameData.Instance.MyHeroInfo.HeroId);

        if (csNationNoblesseInstance == null)
        {
            trNationButtons.gameObject.SetActive(false);
        }
        else
        {
            Button buttonNationCall = trNationButtons.Find("ButtonNationCall").GetComponent<Button>();
            buttonNationCall.onClick.RemoveAllListeners();

            Button buttonNationConvergingAttack = trNationButtons.Find("ButtonNationConvergingAttack").GetComponent<Button>();
            buttonNationConvergingAttack.onClick.RemoveAllListeners();

            Text textButtonNationCall = trNationButtons.transform.Find("TextNationCall").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonNationCall);

            Text textButtonNationConvergingAttack = trNationButtons.transform.Find("TextNationConvergingAttack").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonNationConvergingAttack);

            if (csNationNoblesseInstance.NationNoblesse.NationWarCallEnabled)
            {
                int nRemainingCallCount = CsGameData.Instance.NationWar.NationCallCount - CsNationWarManager.Instance.DailyNationWarCallCount;

                if (nRemainingCallCount > 0)
                {
                    // 개수가 남아 있음
                    if ((m_flNationWarCallCoolTime - Time.realtimeSinceStartup) > 0)
                    {
                        System.TimeSpan tsRemainingCoolTime = System.TimeSpan.FromSeconds(m_flNationWarCallCoolTime - Time.realtimeSinceStartup);
                        // 남은 시간
                        textButtonNationCall.text = string.Format(CsConfiguration.Instance.GetString("A70_BTN_00005"), (int)tsRemainingCoolTime.TotalSeconds);
                        textButtonNationCall.color = new Color32(229, 115, 115, 255);

                        buttonNationCall.interactable = false;
                    }
                    else
                    {
                        textButtonNationCall.text = string.Format(CsConfiguration.Instance.GetString("A70_BTN_00003"), nRemainingCallCount);
                        textButtonNationCall.color = CsUIData.Instance.ColorWhite;

                        buttonNationCall.interactable = true;
                    }
                }
                else
                {
                    textButtonNationCall.text = string.Format(CsConfiguration.Instance.GetString("A70_BTN_00003"), nRemainingCallCount);
                    textButtonNationCall.color = CsUIData.Instance.ColorWhite;

                    buttonNationCall.interactable = false;
                }

                buttonNationCall.gameObject.SetActive(true);
                buttonNationCall.onClick.AddListener(OnClickNationWarCall);
                buttonNationCall.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            else
            {
                buttonNationCall.gameObject.SetActive(false);
            }

            if (csNationNoblesseInstance.NationNoblesse.NationWarConvergingAttackEnabled)
            {
                int nRemainingConvergingAttackCount = CsGameData.Instance.NationWar.ConvergingAttackCount - CsNationWarManager.Instance.DailyNationWarConvergingAttackCount;

                if (nRemainingConvergingAttackCount > 0)
                {
                    // 개수가 남음
                    if ((m_flNationWarConvergingAttackCoolTime - Time.realtimeSinceStartup) > 0)
                    {
                        System.TimeSpan tsRemainingCoolTime = System.TimeSpan.FromSeconds(m_flNationWarConvergingAttackCoolTime - Time.realtimeSinceStartup);
                        // 남은 시간 표시
                        textButtonNationConvergingAttack.text = string.Format(CsConfiguration.Instance.GetString("A70_BTN_00006"), (int)tsRemainingCoolTime.TotalSeconds);
                        textButtonNationConvergingAttack.color = new Color32(229, 115, 115, 255);

                        buttonNationConvergingAttack.interactable = false;
                    }
                    else
                    {
                        textButtonNationConvergingAttack.color = CsUIData.Instance.ColorWhite;
                        textButtonNationConvergingAttack.text = string.Format(CsConfiguration.Instance.GetString("A70_BTN_00004"), nRemainingConvergingAttackCount);

                        buttonNationConvergingAttack.interactable = true;
                    }
                }
                else
                {
                    textButtonNationConvergingAttack.color = CsUIData.Instance.ColorWhite;
                    textButtonNationConvergingAttack.text = string.Format(CsConfiguration.Instance.GetString("A70_BTN_00004"), nRemainingConvergingAttackCount);

                    buttonNationConvergingAttack.interactable = false;
                }

                buttonNationConvergingAttack.gameObject.SetActive(true);
                buttonNationConvergingAttack.onClick.AddListener(OnClickNationWarConvergingAttack);
                buttonNationConvergingAttack.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            else
            {
                buttonNationConvergingAttack.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupPaidTransmission()
    {
        Transform trImageBackground = m_trPopupNationwarTransmission.Find("ImageBackground");

        Text textDescription = trImageBackground.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString("A70_TXT_02005");

        Text textDia = trImageBackground.Find("TextDia").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDia);

        Button buttonCancel = trImageBackground.Find("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(OnClickPopupCancel);
        buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonConfirm = trImageBackground.Find("ButtonConfirm").GetComponent<Button>();
        buttonConfirm.onClick.RemoveAllListeners();
        buttonConfirm.onClick.AddListener(() => OnClickPopupConfirm());
        buttonConfirm.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonCancel = buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonCancel);
        textButtonCancel.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");

        Text textButtonConfirm = buttonConfirm.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonConfirm);
        textButtonConfirm.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupPaidTransmission(int nDailyNationWarPaidTransmission)
    {
        Transform trImageBackground = m_trPopupNationwarTransmission.Find("ImageBackground");

        Text textDia = trImageBackground.Find("TextDia").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDia);
        textDia.text = CsGameData.Instance.NationWar.NationWarPaidTransmissionList[nDailyNationWarPaidTransmission].RequiredDia.ToString("#,##0");
    }

    void NationWarTransmission(int nArrangeId)
    {
        Debug.Log(">>NationWarTransmission<<");
        int nRemainingFreeNationWarTransmission = CsGameData.Instance.NationWar.FreeTransmissionCount - CsNationWarManager.Instance.DailyNationWarFreeTransmissionCount;

        if (0 < nRemainingFreeNationWarTransmission)
        {
            CsNationWarManager.Instance.SendNationWarTransmission(nArrangeId);
        }
        else
        {
            // 무료 전송이 없을 경우 유료 전송으로
            m_nSelectMonsterArrangeId = nArrangeId;

            if (m_bPopupFirst)
            {
                InitializePopupPaidTransmission();
                m_bPopupFirst = false;
            }

            int nDailyNationWarPaidTransmissionCount = 0;

            if (CsNationWarManager.Instance.DailyNationWarPaidTransmissionCount < CsGameData.Instance.NationWar.NationWarPaidTransmissionList.Count)
            {
                nDailyNationWarPaidTransmissionCount = CsNationWarManager.Instance.DailyNationWarPaidTransmissionCount;
            }
            else
            {
                nDailyNationWarPaidTransmissionCount = CsGameData.Instance.NationWar.NationWarPaidTransmissionList.Count - 1;
            }

            UpdatePopupPaidTransmission(nDailyNationWarPaidTransmissionCount);
            m_trPopupNationwarTransmission.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAllianceRevivalPoint()
    {
        Image imagePosition = m_trImageBgNationWar.Find("ImagePosition8").GetComponent<Image>();

        Text textName = imagePosition.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        int nCount = 0;

        for (int i = 0; i < CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance.Count; i++)
        {
            CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance[i].monsterArrangeId);

            if (csNationWarMonsterArrange != null && csNationWarMonsterArrange.Type == (int)EnNationWarMonsterType.NationWarWizard)
            {
                textName.text = csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Find(a => a.ArrangeId == csNationWarMonsterArrange.ArrangeId).RevivalPoint.Name;

                if (CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance[i].nationId == CsNationWarManager.Instance.MyNationWarDeclaration.NationId)
                {
                    nCount++;
                }
            }
        }

        // 공격측이 모두 점령
        if (nCount == 2)
        {
            imagePosition.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_position_8_on");
        }
        else
        {
            imagePosition.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_position_8_off");
        }
    }
}