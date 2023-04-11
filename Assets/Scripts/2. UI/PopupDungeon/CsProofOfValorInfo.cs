using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-05-04)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorInfo : MonoBehaviour
{
    Transform m_trLeftInfo;

    Transform m_trBack;
    Transform m_trDungeonInfo;
    Transform m_trCardInfo;

    Transform m_trGradeList;
    Transform m_trRewardListContent;

    Text m_textFree;
    Text m_textPaidDia;
    Text m_textPaidCount;
    Text m_textName;
    Text m_textDesc;
    Text m_textEnterCount;

    Text m_textStaminaCount;
    Text m_textPurchaseStaminaCount;

    GameObject m_goToggleDifficulty;
    GameObject m_goPopupItemInfo;

    Button m_buttonSweep;
    Button m_buttonEnterDungeon;
    Button m_buttonStamina;
    Button m_buttonTip;
    Button m_buttonAcceleration;

    TimeSpan m_timeSpan;

    CsPopupItemInfo m_csPopupCardInfo;

    float m_flTime = 0;

    bool m_bIsFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsDungeonManager.Instance.EventProofOfValorRefresh += OnEventProofOfValorRefresh;
        CsDungeonManager.Instance.EventProofOfValorRefreshed += OnEventProofOfValorRefreshed;
        CsDungeonManager.Instance.EventProofOfValorSweep += OnEventProofOfValorSweep;

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

    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsDungeonManager.Instance.EventProofOfValorRefresh -= OnEventProofOfValorRefresh;
        CsDungeonManager.Instance.EventProofOfValorRefreshed -= OnEventProofOfValorRefreshed;
        CsDungeonManager.Instance.EventProofOfValorSweep -= OnEventProofOfValorSweep;

        CsGameEventUIToUI.Instance.EventStaminaBuy -= OnEventStaminaBuy;
        CsGameEventUIToUI.Instance.EventStaminaScheduleRecovery -= OnEventStaminaScheduleRecovery;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorRefresh()
    {
        UpdateMonster();
        UpdateRefreshCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorRefreshed()
    {
        UpdateMonster();
        UpdateRefreshCount();
        UpdateEnterCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorSweep(bool bLevelUp, long lAcquiredExp)
    {
        UpdateMonster();
        UpdateRefreshCount();
        UpdateEnterCount();
        UpdateButton();
    }

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

    #endregion Event    

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenItemInfo()
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupCardInfo(OpenPopupCardInfo));
        }
        else
        {
            OpenPopupCardInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSweep()
    {
        string strAlert;

        if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
        {
            //무료소탕
            strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03004"), CsDungeonManager.Instance.ProofOfValor.RequiredStamina, CsGameConfig.Instance.DungeonFreeSweepDailyCount - CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount);
            CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsDungeonManager.Instance.SendProofOfValorSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
        else
        {
            //소탕령 검사.
            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.DungeonSweepItemId) > 0)
            {
                //소탕가능
                strAlert = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03006"), CsDungeonManager.Instance.ProofOfValor.RequiredStamina);
                CsGameEventUIToUI.Instance.OnEventConfirm(strAlert, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsDungeonManager.Instance.SendProofOfValorSweep, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("DUN_GOTO_SHOP"),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop),
                    CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnterDungeon()
    {
        CsDungeonManager.Instance.SendContinentExitForProofOfValorEnter();
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

    //---------------------------------------------------------------------------------------------------
    void OnClickRefresh()
    {
        int nPayCount = CsGameData.Instance.ProofOfValor.PaidRefreshCount + 1;
        if (nPayCount > 4) nPayCount = 4;

        int nFreeCount = CsDungeonManager.Instance.ProofOfValor.MyDailyFreeRefreshCount;
        int nFreeMaxCount = CsDungeonManager.Instance.ProofOfValor.DailyFreeRefreshCount;

        //무료 횟수 남음
        if (nFreeCount < nFreeMaxCount)
        {
            CsDungeonManager.Instance.SendProofOfValorRefresh();
        }
        else if (CsGameData.Instance.MyHeroInfo.Dia >= CsDungeonManager.Instance.ProofOfValor.GetProofOfValorPaidRefresh(nPayCount).RequiredDia)
        {
            CsDungeonManager.Instance.SendProofOfValorRefresh();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
        }
    }

    #endregion Event Handler

    void UpdateMonster()
    {
		CsProofOfValorBossMonsterArrange csProofOfValorBossMonsterArrange = CsGameData.Instance.ProofOfValor.GetProofOfValorBossMonsterArrange(CsGameData.Instance.ProofOfValor.BossMonsterArrangeId);
        CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(csProofOfValorBossMonsterArrange.MonsterArrange.MonsterId);
		CsCreatureCard csCreatureCard = CsGameData.Instance.GetCreatureCard(CsGameData.Instance.ProofOfValor.CreatureCardId);

        m_textName.text = csMonsterInfo.Name;

        for (int i = 0; i < m_trGradeList.childCount; ++i)
        {
            if (i < csProofOfValorBossMonsterArrange.StarGrade)
            {
                m_trGradeList.GetChild(i).gameObject.SetActive(true);
                m_trGradeList.GetChild(i).Find("Image").gameObject.SetActive(csProofOfValorBossMonsterArrange.IsSpecial);
            }
            else
            {
                m_trGradeList.GetChild(i).gameObject.SetActive(false);
            }
        }

        Button buttonCard = m_trRewardListContent.Find("Card").GetComponent<Button>();
        buttonCard.onClick.RemoveAllListeners();
        buttonCard.onClick.AddListener(OnClickOpenItemInfo);
        buttonCard.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trRewardListContent.Find("Card/Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_card_" + csCreatureCard.CreatureCardGrade.Grade);
        m_trRewardListContent.Find("Card/TextName").GetComponent<Text>().text = csCreatureCard.Name;
        m_trRewardListContent.Find("Card/TextCount").GetComponent<Text>().text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), 1);
        m_trRewardListContent.Find("SoulPowder/TextCount").GetComponent<Text>().text = (csProofOfValorBossMonsterArrange.RewardSoulPowder + csProofOfValorBossMonsterArrange.SpecialRewardSoulPowder).ToString("#,##0");
        m_trRewardListContent.Find("Exp/TextCount").GetComponent<Text>().text = CsDungeonManager.Instance.ProofOfValor.GetProofOfValorReward(CsGameData.Instance.MyHeroInfo.Level).SuccessExpReward.Value.ToString("#,##0");
        LoadMonsterModel(csMonsterInfo.MonsterCharacter.PrefabName);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRefreshCount()
    {
        if (m_bIsFirst)
        {
            m_textFree = m_trLeftInfo.Find("TextFree").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textFree);

            m_textPaidDia = m_trLeftInfo.Find("TextPay").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textPaidDia);

            m_textPaidCount = m_trLeftInfo.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textPaidCount);
        }

		int nFreeCount = CsDungeonManager.Instance.ProofOfValor.MyDailyFreeRefreshCount;
        int nFreeMaxCount = CsDungeonManager.Instance.ProofOfValor.DailyFreeRefreshCount;

        //무료 횟수 남음
        if (nFreeCount < nFreeMaxCount)
        {
            m_textFree.text = string.Format(CsConfiguration.Instance.GetString("A89_TXT_01003"), nFreeMaxCount - nFreeCount, nFreeMaxCount);
            m_textFree.gameObject.SetActive(true);
            m_textPaidDia.gameObject.SetActive(false);
        }
        else
        {
			int nPayCount = CsGameData.Instance.ProofOfValor.PaidRefreshCount + 1;
            if (nPayCount > 4) nPayCount = 4;
            m_textPaidDia.text = CsDungeonManager.Instance.ProofOfValor.GetProofOfValorPaidRefresh(nPayCount).RequiredDia.ToString("#,##0");
            m_textFree.gameObject.SetActive(false);
            m_textPaidDia.gameObject.SetActive(true);
        }

		int nPlayCount = CsGameData.Instance.ProofOfValor.MyDailyPaidRefreshCount;
        int nPlayMaxCount = CsGameData.Instance.ProofOfValor.DailyPaidRefreshCount;

        m_textPaidCount.text = string.Format(CsConfiguration.Instance.GetString("A89_TXT_01004"), nPlayMaxCount - nPlayCount, nPlayMaxCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEnterCount()
    {
		int nCount = CsGameData.Instance.ProofOfValor.DailyPlayCount;
        int nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.ProofOfValorEnterCount;
        m_textEnterCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nMaxCount - nCount, nMaxCount);
    }

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

        m_trLeftInfo = m_trBack.Find("LeftInfo");

        Button buttonRefresh = m_trLeftInfo.Find("ButtonRefresh").GetComponent<Button>();
        buttonRefresh.onClick.RemoveAllListeners();
        buttonRefresh.onClick.AddListener(OnClickRefresh);
        buttonRefresh.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trDungeonInfo = m_trBack.Find("DungeonInfo");
        m_trGradeList = m_trDungeonInfo.Find("GradeList");

        m_textName = m_trDungeonInfo.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textName);

        m_textDesc = m_trDungeonInfo.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDesc);

        Text textDungeonInfo = m_trDungeonInfo.Find("TextDungeonInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDungeonInfo);
        textDungeonInfo.text = CsConfiguration.Instance.GetString("A17_TXT_00001");

        Transform trInfoList = m_trDungeonInfo.Find("InfoList");

        Text textStamina = trInfoList.Find("InfoStamina/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textStamina);
        textStamina.text = CsConfiguration.Instance.GetString("PUBLIC_DUN_STA");

        Text textStaminaValue = trInfoList.Find("InfoStamina/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textStaminaValue);
        textStaminaValue.text = CsDungeonManager.Instance.ProofOfValor.RequiredStamina.ToString("#,##0");

        Text textEnterCount = trInfoList.Find("InfoEnterCount/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnterCount);
        textEnterCount.text = CsConfiguration.Instance.GetString("PUBLIC_DUN_COUNT");

        m_textEnterCount = trInfoList.Find("InfoEnterCount/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEnterCount);

        Transform trRewardList = m_trDungeonInfo.Find("RewardList");
        m_trRewardListContent = trRewardList.Find("Viewport/Content");

        Text textCardName = m_trRewardListContent.Find("Card/TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCardName);

        Text textCardCount = m_trRewardListContent.Find("Card/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCardCount);

        Text textExpName = m_trRewardListContent.Find("Exp/TextName").GetComponent<Text>();
        textExpName.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");
        CsUIData.Instance.SetFont(textExpName);

        Text textExpCount = m_trRewardListContent.Find("Exp/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExpCount);

        Text textSoulPowder = m_trRewardListContent.Find("SoulPowder/TextName").GetComponent<Text>();
        textSoulPowder.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_SOULP");
        CsUIData.Instance.SetFont(textSoulPowder);

        Text textSoulPowderCount = m_trRewardListContent.Find("SoulPowder/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSoulPowderCount);

        Text textClearReward = trRewardList.Find("TextClearReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClearReward);
        textClearReward.text = CsConfiguration.Instance.GetString("A17_TXT_00006");

        m_buttonSweep = m_trDungeonInfo.Find("ButtonSweep").GetComponent<Button>();
        m_buttonSweep.onClick.RemoveAllListeners();
        m_buttonSweep.onClick.AddListener(OnClickSweep);
        m_buttonSweep.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textSweep = m_buttonSweep.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSweep);

        m_buttonEnterDungeon = m_trDungeonInfo.Find("ButtonChallenge").GetComponent<Button>();
        m_buttonEnterDungeon.onClick.RemoveAllListeners();
        m_buttonEnterDungeon.onClick.AddListener(OnClickEnterDungeon);
        m_buttonEnterDungeon.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textEnterDungeon = m_buttonEnterDungeon.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnterDungeon);
        textEnterDungeon.text = CsConfiguration.Instance.GetString("A13_BTN_00003");

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

        UpdateButton();
        UpdateEnterCount();
        UpdateMonster();
        UpdateRefreshCount();
        UpdateStaminaCount();
        UpdatePurchaseStaminaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButton()
    {
        if (CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount < CsGameConfig.Instance.DungeonFreeSweepDailyCount)
        {
            m_buttonSweep.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A13_BTN_00001");
        }
        else
        {
            m_buttonSweep.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A13_BTN_00002");
        }

        int nPlayCount = CsGameData.Instance.ProofOfValor.DailyPlayCount;
        int nPlayMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.ProofOfValorEnterCount;
        if (nPlayCount >= nPlayMaxCount)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, false);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEnterDungeon, true);

            if (CsGameData.Instance.ProofOfValor.ProofOfValorCleared)
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, true);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonSweep, false);
            }
        }
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

    #region ItemInfo

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCardInfo(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupCardInfo()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        Transform trPopupList = trCanvas2.Find("PopupList");

        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, trPopupList);
        m_trCardInfo = goPopupItemInfo.transform;
        m_csPopupCardInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        CsCreatureCard csCreatureCard = CsGameData.Instance.GetCreatureCard(CsDungeonManager.Instance.ProofOfValor.CreatureCardId);

        m_csPopupCardInfo.EventClosePopupItemInfo += OnEventClosePopupCardInfo;
        m_csPopupCardInfo.DisplayType(EnPopupItemInfoPositionType.Center, csCreatureCard);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCardInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupCardInfo.EventClosePopupItemInfo -= OnEventClosePopupCardInfo;
        Destroy(m_trCardInfo.gameObject);
        m_csPopupCardInfo = null;
        m_trCardInfo = null;
    }

    #endregion ItemInfo

    #region 3DLoad

    Transform m_tr3dModel;
    //---------------------------------------------------------------------------------------------------
    //모델 동적로드 함수
    void LoadMonsterModel(string strPrefabName)
    {
        //int nMonsterId = CsDungeonManager.Instance.HeroProofOfValorInstance.bossMonsterArrangeId;

        if (m_tr3dModel != null)
        {
            Destroy(m_tr3dModel.gameObject);
            m_tr3dModel = null;
        }

        StartCoroutine(LoadMonsterModelCoroutine(strPrefabName));
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadMonsterModelCoroutine(string strPrefabName)
    {
        Debug.Log("strPrefabName : " + strPrefabName);
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/MonsterObject/" + strPrefabName);
        yield return resourceRequest;
        m_tr3dModel = Instantiate<GameObject>((GameObject)resourceRequest.asset, m_trLeftInfo.Find("3DMonster")).transform;

        int nLayer = LayerMask.NameToLayer("UIChar");

        Transform[] atrMon = m_tr3dModel.GetComponentsInChildren<Transform>();

        for (int i = 0; i < atrMon.Length; ++i)
        {
            atrMon[i].gameObject.layer = nLayer;
        }

        m_tr3dModel.localPosition = new Vector3(20, -195, 580);
        m_tr3dModel.eulerAngles = new Vector3(0, 160, 0);
        m_tr3dModel.localScale = new Vector3(150, 150, 150);
        m_tr3dModel.name = strPrefabName;
        m_tr3dModel.gameObject.SetActive(true);
    }
    #endregion 3DLoad
}
