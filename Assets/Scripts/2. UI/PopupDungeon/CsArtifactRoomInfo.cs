using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-02-08)
//---------------------------------------------------------------------------------------------------

public class CsArtifactRoomInfo : MonoBehaviour
{
    Transform m_trBack;
    Transform m_trFloorContent;
    Transform m_trAutoList;
    Transform m_trDungeonInfo;
    Transform m_trItemInfo;

    Text m_textMaxFloor;
    Text m_textCurrentFloor;
    Text m_textOpenLevel;
    Text m_textNowCp;
    Text m_textRecomandCp;
    Text m_textStaminaCount;
    Text m_textPurchaseStaminaCount;
    Text m_textProgress;
    Text m_textRemaning;
    Text m_textAccelDia;
    Text m_textResetCount;
    Text m_textRewardCount;
    Text m_textAutoItemName;
    Text m_textButtonStop;

    GameObject m_goToggleDifficulty;
    GameObject m_goPopupItemInfo;

    Button m_buttonReset;
    Button m_buttonAuto;
    Button m_buttonEnterDungeon;
    Button m_buttonStamina;
    Button m_buttonTip;
    Button m_buttonAcceleration;

    TimeSpan m_timeSpan;

    CsArtifactRoomFloor m_csArtifactRoomFloor = null;
    CsPopupItemInfo m_csPopupItemInfo;

    float m_flTime = 0;

    bool m_bIsFirst = true;
    bool m_bAutoStop = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        //Response
        CsDungeonManager.Instance.EventArtifactRoomInit += OnEventArtifactRoomInit;
        CsDungeonManager.Instance.EventArtifactRoomSweep += OnEventArtifactRoomSweep;
        CsDungeonManager.Instance.EventArtifactRoomSweepStop += OnEventArtifactRoomSweepStop;
        CsDungeonManager.Instance.EventArtifactRoomSweepComplete += OnEventArtifactRoomSweepComplete;
        CsDungeonManager.Instance.EventArtifactRoomSweepAccelerate += OnEventArtifactRoomSweepAccelerate;

        //Event
        CsDungeonManager.Instance.EventArtifactRoomSweepNextFloorStart += OnEventArtifactRoomSweepNextFloorStart;
        CsDungeonManager.Instance.EventArtifactRoomSweepCompleted += OnEventArtifactRoomSweepCompleted;

        CsGameEventUIToUI.Instance.EventStaminaBuy += OnEventStaminaBuy;
        CsGameEventUIToUI.Instance.EventStaminaScheduleRecovery += OnEventStaminaScheduleRecovery;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_textStaminaCount != null)
            {
                if (CsGameData.Instance.MyHeroInfo.Stamina >= CsGameConfig.Instance.MaxStamina)
                {
                    UpdateStaminaCount(CsConfiguration.Instance.GetString("A13_TITLE_00003"));
                }
                else
                {
                    if (((CsGameData.Instance.MyHeroInfo.StaminaAutoRecoveryRemainingTime) > 0.0f))
                    {
                        m_timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.MyHeroInfo.StaminaAutoRecoveryRemainingTime);
                        UpdateStaminaCount(string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00004"), m_timeSpan.Minutes.ToString("00"), m_timeSpan.Seconds.ToString("00")));
                    }
                    else
                    {
                        UpdateStaminaCount(string.Format(CsConfiguration.Instance.GetString("A13_TITLE_00004"), "00", "00"));
                    }
                }
            }

            if (m_textRemaning != null && CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime - Time.realtimeSinceStartup > 0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime - Time.realtimeSinceStartup);
                m_textRemaning.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00005"), timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bIsFirst)
        {
            InitializeUI();
            m_bIsFirst = false;
        }

        if (CsDungeonManager.Instance.ArtifactRoom.ArtifactRoomBestFloor == CsDungeonManager.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor && (CsDungeonManager.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime - Time.realtimeSinceStartup) <= 0.0f)
        {
            m_bAutoStop = false;
        }
        else
        {
            m_bAutoStop = true;
        }

        UpdateArtifactRoomInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        //Response
        CsDungeonManager.Instance.EventArtifactRoomInit -= OnEventArtifactRoomInit;
        CsDungeonManager.Instance.EventArtifactRoomSweep -= OnEventArtifactRoomSweep;
        CsDungeonManager.Instance.EventArtifactRoomSweepStop -= OnEventArtifactRoomSweepStop;
        CsDungeonManager.Instance.EventArtifactRoomSweepComplete -= OnEventArtifactRoomSweepComplete;
        CsDungeonManager.Instance.EventArtifactRoomSweepAccelerate -= OnEventArtifactRoomSweepAccelerate;

        //Event
        CsDungeonManager.Instance.EventArtifactRoomSweepNextFloorStart -= OnEventArtifactRoomSweepNextFloorStart;
        CsDungeonManager.Instance.EventArtifactRoomSweepCompleted -= OnEventArtifactRoomSweepCompleted;

        CsGameEventUIToUI.Instance.EventStaminaBuy -= OnEventStaminaBuy;
        CsGameEventUIToUI.Instance.EventStaminaScheduleRecovery -= OnEventStaminaScheduleRecovery;
    }

    #region EventHandler
    //---------------------------------------------------------------------------------------------------
    void OnEventStaminaBuy()
    {
        UpdateStaminaCount();
        UpdatePurchaseStaminaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStaminaScheduleRecovery()
    {
        UpdateStaminaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomSweepCompleted()
    {
        m_bAutoStop = false;
        UpdateArtifactRoomInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomSweepNextFloorStart()
    {
        UpdateArtifactRoomInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomInit()
    {
        UpdateArtifactRoomInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomSweep()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime - Time.realtimeSinceStartup);
        m_textRemaning.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00005"), timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
        m_bAutoStop = true;

        UpdateArtifactRoomInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomSweepStop(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        UpdateArtifactRoomInfo();

        if (pDItemBooty.Length > 0)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A47_TXT_00012"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomSweepComplete(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        UpdateArtifactRoomInfo();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A47_TXT_00012"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomSweepAccelerate(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        UpdateArtifactRoomInfo();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A47_TXT_00012"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenItemInfo()
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(OpenPopupItemInfo));
        }
        else
        {
            OpenPopupItemInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickStartAutoClear()
    {
        if (CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor <= CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor)
        {
            CsDungeonManager.Instance.SendArtifactRoomSweep();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAutoStop()
    {
        if (m_bAutoStop)
        {
            CsDungeonManager.Instance.SendArtifactRoomSweepStop();
        }
        else
        {
            CsDungeonManager.Instance.SendArtifactRoomSweepComplete();
        }

        //if (CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime - Time.realtimeSinceStartup > 0)
        //{
        //    CsDungeonManager.Instance.SendArtifactRoomSweepStop();
        //}
        //else
        //{
        //    CsDungeonManager.Instance.SendArtifactRoomSweepComplete();
        //}
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAcceleration()
    {
        CsDungeonManager.Instance.SendArtifactRoomSweepAccelerate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickResetFloor()
    {
        if (CsGameData.Instance.ArtifactRoom.ArtifactRoomDailyInitCount < CsGameData.Instance.MyHeroInfo.VipLevel.ArtifactRoomInitMaxCount && CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor != 1)
        {
            CsDungeonManager.Instance.SendArtifactRoomInit();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnterDungeon()
    {
        if (m_csArtifactRoomFloor != null && m_csArtifactRoomFloor.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            CsDungeonManager.Instance.SendContinentExitForArtifactRoomEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGoBackDungeonList()
    {
        transform.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventGoBackDungeonCartegoryList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenStaminaToolTip()
    {
        m_buttonTip.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseStaminaToolTip()
    {
        m_buttonTip.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickChargeStamina()
    {
        int nStaminBuyCount = CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount + 1;
        CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);

        if (csStaminaBuyCount == null)
        {
            nStaminBuyCount = CsGameData.Instance.StaminaBuyCountList.Count;
            csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);
        }

        //스테미너 충전 확인창
        string strDes = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03001"), csStaminaBuyCount.RequiredDia, csStaminaBuyCount.Stamina);

        CsGameEventUIToUI.Instance.OnEventConfirm(strDes, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickChargeStaminaOK(nStaminBuyCount), CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickChargeStaminaOK(int nStaminaBuyCount)
    {
        CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminaBuyCount);

        if (csStaminaBuyCount.RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia)
        {
            CsCommandEventManager.Instance.SendStaminaBuy();
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trBack = transform.Find("ImageBackground");

        Button buttonBackDungeonList = m_trBack.Find("ButtonBackDungeonList").GetComponent<Button>();
        buttonBackDungeonList.onClick.RemoveAllListeners();
        buttonBackDungeonList.onClick.AddListener(OnClickGoBackDungeonList);
        buttonBackDungeonList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textBackDungeonList = buttonBackDungeonList.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBackDungeonList);
        textBackDungeonList.text = CsConfiguration.Instance.GetString("A17_BTN_00003");

        m_trFloorContent = m_trBack.Find("Scroll View/Viewport/Content");

        Text textDungeonName = m_trBack.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDungeonName);
        textDungeonName.text = CsGameData.Instance.ArtifactRoom.Name;

        m_textMaxFloor = m_trBack.Find("TextMaxFloor").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMaxFloor);

        m_textCurrentFloor = m_trBack.Find("TextCurrentFloor").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCurrentFloor);

        m_trDungeonInfo = m_trBack.Find("DungeonInfo");

        Transform trInfo = m_trDungeonInfo.Find("Info");

        Text textDungeonInfo = trInfo.Find("TextDungeonInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDungeonInfo);
        textDungeonInfo.text = CsConfiguration.Instance.GetString("A17_TXT_00001");

        Text textClearReward = trInfo.Find("TextClearReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClearReward);
        textClearReward.text = CsConfiguration.Instance.GetString("A17_TXT_00006");

        Transform trInfoList = trInfo.Find("InfoList");

        Text textOpenLevel = trInfoList.Find("InfoLevel/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOpenLevel);
        textOpenLevel.text = CsConfiguration.Instance.GetString("PUBLIC_DUN_LV");

        m_textOpenLevel = trInfoList.Find("InfoLevel/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textOpenLevel);

        Text textNowCp = trInfoList.Find("InfoBattlePower/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNowCp);
        textNowCp.text = CsConfiguration.Instance.GetString("A47_TXT_00001");

        m_textNowCp = trInfoList.Find("InfoBattlePower/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNowCp);

        Text textRecomandCp = trInfoList.Find("InfoRecommendBattlePower/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRecomandCp);
        textRecomandCp.text = CsConfiguration.Instance.GetString("A47_TXT_00002");

        m_textRecomandCp = trInfoList.Find("InfoRecommendBattlePower/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRecomandCp);

        Button buttonReward = trInfo.Find("ButtonRewardItem").GetComponent<Button>();
        buttonReward.onClick.RemoveAllListeners();
        buttonReward.onClick.AddListener(OnClickOpenItemInfo);
        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Image imageReward = buttonReward.transform.Find("Image").GetComponent<Image>();
        imageReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[0].ItemReward.Item.ItemId);

        Text textName = buttonReward.transform.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[0].ItemReward.Item.Name;

        m_textRewardCount = buttonReward.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRewardCount);

        m_buttonReset = m_trDungeonInfo.Find("ButtonReset").GetComponent<Button>();
        m_buttonReset.onClick.RemoveAllListeners();
        m_buttonReset.onClick.AddListener(OnClickResetFloor);
        m_buttonReset.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textReset = m_buttonReset.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReset);
        textReset.text = CsConfiguration.Instance.GetString("A47_BTN_00002");

        m_buttonAuto = m_trDungeonInfo.Find("ButtonAuto").GetComponent<Button>();
        m_buttonAuto.onClick.RemoveAllListeners();
        m_buttonAuto.onClick.AddListener(OnClickStartAutoClear);
        m_buttonAuto.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAuto = m_buttonAuto.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAuto);
        textAuto.text = CsConfiguration.Instance.GetString("A47_BTN_00003");

        m_buttonEnterDungeon = m_trDungeonInfo.Find("ButtonChallenge").GetComponent<Button>();
        m_buttonEnterDungeon.onClick.RemoveAllListeners();
        m_buttonEnterDungeon.onClick.AddListener(OnClickEnterDungeon);
        m_buttonEnterDungeon.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textEnterDungeon = m_buttonEnterDungeon.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnterDungeon);
        textEnterDungeon.text = CsConfiguration.Instance.GetString("A47_BTN_00004");

        m_textResetCount = m_trDungeonInfo.Find("TextResetCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textResetCount);

        //오토
        m_trAutoList = m_trBack.Find("AutoState");

        Text textProgress = m_trAutoList.Find("TextProgress").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProgress);
        textProgress.text = CsConfiguration.Instance.GetString("A47_TXT_00005");

        m_textProgress = m_trAutoList.Find("TextProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textProgress);

        Text textRemaning = m_trAutoList.Find("TextRemaning").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRemaning);
        textRemaning.text = CsConfiguration.Instance.GetString("A47_TXT_00006");

        m_textRemaning = m_trAutoList.Find("TextRemaningTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRemaning);

        Text textAutoReward = m_trAutoList.Find("TextReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAutoReward);
        textAutoReward.text = CsConfiguration.Instance.GetString("A47_TXT_00008");

        Transform trAutoReward = m_trAutoList.Find("ButtonReward");

        Button buttonAutoReward = trAutoReward.GetComponent<Button>();
        buttonAutoReward.onClick.RemoveAllListeners();
        buttonAutoReward.onClick.AddListener(OnClickOpenItemInfo);
        buttonAutoReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Image imageAutoReward = trAutoReward.Find("ImageIcon").GetComponent<Image>();
        imageAutoReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[0].ItemReward.Item.ItemId);

        m_textAutoItemName = trAutoReward.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAutoItemName);

        Button buttonAutoStop = m_trAutoList.Find("ButtonStop").GetComponent<Button>();
        buttonAutoStop.onClick.RemoveAllListeners();
        buttonAutoStop.onClick.AddListener(OnClickAutoStop);
        buttonAutoStop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textButtonStop = buttonAutoStop.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textButtonStop);

        m_buttonAcceleration = m_trAutoList.Find("ButtonAcceleration").GetComponent<Button>();
        m_buttonAcceleration.onClick.RemoveAllListeners();
        m_buttonAcceleration.onClick.AddListener(OnClickAcceleration);
        m_buttonAcceleration.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAcceleration = m_buttonAcceleration.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAcceleration);
        textAcceleration.text = CsConfiguration.Instance.GetString("A47_BTN_00006");

        m_textAccelDia = m_buttonAcceleration.transform.Find("TextDia").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAccelDia);

        //스테미너
        m_buttonStamina = m_trBack.Find("ButtonStamina").GetComponent<Button>();
        m_buttonStamina.onClick.RemoveAllListeners();
        m_buttonStamina.onClick.AddListener(OnClickOpenStaminaToolTip);
        m_buttonStamina.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textStaminaCount = m_buttonStamina.transform.Find("TextStaminaCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textStaminaCount);

        Button buttonChargeStamina = m_buttonStamina.transform.Find("ButtonChargeStamina").GetComponent<Button>();
        buttonChargeStamina.onClick.RemoveAllListeners();
        buttonChargeStamina.onClick.AddListener(OnClickChargeStamina);
        buttonChargeStamina.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonTip = m_buttonStamina.transform.Find("ButtonClose").GetComponent<Button>();
        m_buttonTip.onClick.RemoveAllListeners();
        m_buttonTip.onClick.AddListener(OnClickCloseStaminaToolTip);
        m_buttonTip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trTipBack = m_buttonTip.transform.Find("ImageBack");

        Text textPurchaseStaminaInfo = trTipBack.Find("PurchaseStaminaCount/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPurchaseStaminaInfo);
        textPurchaseStaminaInfo.text = CsConfiguration.Instance.GetString("A13_TXT_04004");

        m_textPurchaseStaminaCount = trTipBack.Find("PurchaseStaminaCount/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPurchaseStaminaCount);

        for (int i = 0; i < CsGameData.Instance.StaminaRecoveryScheduleList.Count; i++)
        {
            Text textStaminaRecoveryGuide = trTipBack.Find("TextStaminaRecovery" + i).GetComponent<Text>();
            CsUIData.Instance.SetFont(textStaminaRecoveryGuide);

			TimeSpan timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.StaminaRecoveryScheduleList[i].RecoveryTime);

			textStaminaRecoveryGuide.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_04006"), timeSpan.Hours.ToString("00"), CsGameData.Instance.StaminaRecoveryScheduleList[i].RecoveryStamina);
        }

        UpdateStaminaCount();
        UpdatePurchaseStaminaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateArtifactRoomInfo()
    {
        m_trBack.gameObject.SetActive(true);
        m_textMaxFloor.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_01001"), CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor);

        if (CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor > 0)
        {
            //소탕중
            m_trDungeonInfo.gameObject.SetActive(false);
            m_trAutoList.gameObject.SetActive(true);

            m_textCurrentFloor.text = CsConfiguration.Instance.GetString("A47_TXT_00004");

            int nRewarCount = 0;
            int nDiaCount = 0;

            //층
            for (int i = 0; i < CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList.Count; i++)
            {
                int nToggleIndex = i;
                Transform trToggle = m_trFloorContent.Find("Floor" + nToggleIndex);

                if (trToggle == null)
                {
                    if (m_goToggleDifficulty == null)
                    {
                        m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                    }

                    GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trFloorContent);
                    goToggle.name = "Floor" + nToggleIndex;
                    trToggle = goToggle.transform;
                }
                else
                {
                    trToggle.gameObject.SetActive(true);
                }

                Toggle toggleFloor = trToggle.GetComponent<Toggle>();
                toggleFloor.onValueChanged.RemoveAllListeners();
                toggleFloor.interactable = false;

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor == CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor)
                {
                    toggleFloor.isOn = true;
                }
                else
                {
                    toggleFloor.isOn = false;
                }

                Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDifficulty);
                textDifficulty.text = CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Name;

                Transform trComplete = trToggle.Find("ImageComplete");

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor > CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor)
                {
                    trComplete.gameObject.SetActive(true);
                }
                else
                {
                    trComplete.gameObject.SetActive(false);
                }

                Transform trLock = trToggle.Find("ImageLock");
                trLock.gameObject.SetActive(false);

                //보상 갯수
                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor >= CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor && CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor < CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor)
                {
                    nRewarCount += CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].ItemReward.ItemCount;
                }
                else if (CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor == CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor)
                {
                    if (CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime - Time.realtimeSinceStartup <= 0)
                    {
                        nRewarCount += CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].ItemReward.ItemCount;
                    }
                }

                //다이아 소모량
                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor >= CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor && CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor <= CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor)
                {
                    nDiaCount += CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].SweepDia;
                }
            }

            for (int i = 0; i < m_trFloorContent.childCount - CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList.Count; i++)
            {
                Transform trToggle = m_trFloorContent.Find("Floor" + (i + CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList.Count));
                if (trToggle != null)
                {
                    trToggle.gameObject.SetActive(false);
                }
            }

            m_textAutoItemName.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_01005"), CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[0].ItemReward.Item.Name, nRewarCount);
            m_textAccelDia.text = nDiaCount.ToString("#,##0");

            if (m_bAutoStop)
            {
                m_textButtonStop.text = CsConfiguration.Instance.GetString("A47_BTN_00005");
                CsUIData.Instance.DisplayButtonInteractable(m_buttonAcceleration, true);
                m_textAccelDia.color = CsUIData.Instance.ColorButtonOn;
                m_textProgress.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_01003"), CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor - 1, CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor);
            }
            else
            {
                m_textButtonStop.text = CsConfiguration.Instance.GetString("A47_BTN_00007");
                CsUIData.Instance.DisplayButtonInteractable(m_buttonAcceleration, false);
                m_textAccelDia.color = CsUIData.Instance.ColorButtonOff;
                m_textProgress.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_01003"), CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor, CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor);
            }
        }
        else
        {
            for (int i = 0; i < CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList.Count; i++)
            {
                int nToggleIndex = i;
                Transform trToggle = m_trFloorContent.Find("Floor" + nToggleIndex);

                if (trToggle == null)
                {
                    if (m_goToggleDifficulty == null)
                    {
                        m_goToggleDifficulty = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ToggleDungeonStep");
                    }

                    GameObject goToggle = Instantiate(m_goToggleDifficulty, m_trFloorContent);
                    goToggle.name = "Floor" + nToggleIndex;
                    trToggle = goToggle.transform;
                }
                else
                {
                    trToggle.gameObject.SetActive(true);
                }

                Toggle toggleFloor = trToggle.GetComponent<Toggle>();
                toggleFloor.onValueChanged.RemoveAllListeners();
                toggleFloor.interactable = false;

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor == CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor)
                {
                    toggleFloor.isOn = true;
                }
                else
                {
                    toggleFloor.isOn = false;
                }

                Text textDifficulty = trToggle.Find("TextStep").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDifficulty);
                textDifficulty.text = CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Name;

                Transform trComplete = trToggle.Find("ImageComplete");

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor > CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor)
                {
                    trComplete.gameObject.SetActive(true);
                }
                else
                {
                    trComplete.gameObject.SetActive(false);
                }

                Transform trLock = trToggle.Find("ImageLock");
                trLock.gameObject.SetActive(false);

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i].Floor == CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor)
                {
                    m_csArtifactRoomFloor = CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[i];
                }
            }

            m_trAutoList.gameObject.SetActive(false);
            m_trDungeonInfo.gameObject.SetActive(true);

            if (m_csArtifactRoomFloor == null)
            {
                //모든층 클리어                
                m_trDungeonInfo.Find("Info").gameObject.SetActive(false);
                m_textCurrentFloor.text = CsConfiguration.Instance.GetString("A47_TXT_00009");

                m_csArtifactRoomFloor = CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList.Count - 1];

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomDailyInitCount < CsGameData.Instance.MyHeroInfo.VipLevel.ArtifactRoomInitMaxCount)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonReset, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonReset, false);
                }

                CsUIData.Instance.DisplayButtonInteractable(m_buttonAuto, false);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            }
            else
            {
                m_trDungeonInfo.Find("Info").gameObject.SetActive(true);
                m_textCurrentFloor.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_01002"), CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor);

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomDailyInitCount < CsGameData.Instance.MyHeroInfo.VipLevel.ArtifactRoomInitMaxCount)
                {
                    if (CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor > 1)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonReset, true);
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonReset, false);
                    }
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonReset, false);
                }

                if (CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor <= CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonAuto, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonAuto, false);
                }

                if (m_csArtifactRoomFloor.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
                }
            }

            m_textOpenLevel.text = m_csArtifactRoomFloor.RequiredHeroLevel.ToString();

            if (m_csArtifactRoomFloor.RequiredHeroLevel > CsGameData.Instance.MyHeroInfo.Level)
            {
                m_textOpenLevel.color = new Color32(229, 115, 115, 255);
            }
            else
            {
                m_textOpenLevel.color = new Color32(255, 214, 80, 255);
            }

            m_textNowCp.text = CsGameData.Instance.MyHeroInfo.BattlePower.ToString("#,##0");
            m_textRecomandCp.text = m_csArtifactRoomFloor.RecommendBattlePower.ToString("#,##0");
            m_textResetCount.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_00003"), CsGameData.Instance.MyHeroInfo.VipLevel.ArtifactRoomInitMaxCount - CsGameData.Instance.ArtifactRoom.ArtifactRoomDailyInitCount);

            m_textRewardCount.text = m_csArtifactRoomFloor.ItemReward.ItemCount.ToString("#,##0");
        }

        RectTransform rectTransform = m_trFloorContent.GetComponent<RectTransform>();
        int nMaxY = (116 * (CsDungeonManager.Instance.ArtifactRoom.ArtifactRoomFloorList.Count - 5)) + 110;
        int nNowFloorY = 116 * (CsDungeonManager.Instance.ArtifactRoom.ArtifactRoomCurrentFloor - 1);

        if (nNowFloorY >= nMaxY)
        {
            nNowFloorY = nMaxY;
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, nNowFloorY);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateStaminaCount(string strTime = "00:00")
    {
        //현재 스테미너
        m_textStaminaCount.text = string.Format(CsConfiguration.Instance.GetString("A17_TXT_00010"), CsGameData.Instance.MyHeroInfo.Stamina, CsGameConfig.Instance.MaxStamina, strTime);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePurchaseStaminaCount()
    {
        //스테미너 구매 횟수
        m_textPurchaseStaminaCount.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_04005"), 0, CsGameData.Instance.MyHeroInfo.VipLevel.StaminaBuyMaxCount);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        Transform trPopupList = trCanvas2.Find("PopupList");

        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        CsItem csItem = CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList[0].ItemReward.Item;

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, 1, false, -1, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }

}
