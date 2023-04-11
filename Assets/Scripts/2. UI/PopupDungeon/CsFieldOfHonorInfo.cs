using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-02-28)
//---------------------------------------------------------------------------------------------------

public class CsFieldOfHonorInfo : MonoBehaviour
{
    Transform m_trBack;
    Transform m_trPopupRanking;
    Transform m_trShop;
    Transform m_trPopupList;

    GameObject m_goRanker;
    GameObject m_goPopupCalculator;
    GameObject m_goShopProduct;

    Text m_textMyInfoRank;
    Text m_textMyInfoCp;
    Text m_textMyInfoCount;
    Text m_textResetGuide;

    int m_nLoadCount = 0;
    int m_nstandardPosition = 0;
    int m_nHonorShopProductIndex = -1;

    bool m_bFirst = true;

    CsPopupCalculator m_csPopupCalculator;
    ClientCommon.PDFieldOfHonorRanking[] m_pdFieldOfHonorRanking;
    ClientCommon.PDFieldOfHonorHero[] m_pdFieldOfHonorHero;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trBack = transform.Find("ImageBackground");

        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

        CsDungeonManager.Instance.EventFieldOfHonorInfo += OnEventFieldOfHonorInfo;
        CsDungeonManager.Instance.EventFieldOfHonorTopRankingList += OnEventFieldOfHonorTopRankingList;
        CsDungeonManager.Instance.EventFieldOfHonorRankerInfo += OnEventFieldOfHonorRankerInfo;
        CsDungeonManager.Instance.EventFieldOfHonorRankingRewardReceive += OnEventFieldOfHonorRankingRewardReceive;		
        CsGameEventUIToUI.Instance.EventHonorShopProductBuy += OnEventHonorShopProductBuy;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            InitializeUI();
            m_bFirst = false;
        }

        CsDungeonManager.Instance.SendFieldOfHonorInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsDungeonManager.Instance.EventFieldOfHonorInfo -= OnEventFieldOfHonorInfo;
        CsDungeonManager.Instance.EventFieldOfHonorTopRankingList -= OnEventFieldOfHonorTopRankingList;
        CsDungeonManager.Instance.EventFieldOfHonorRankerInfo -= OnEventFieldOfHonorRankerInfo;
        CsDungeonManager.Instance.EventFieldOfHonorRankingRewardReceive -= OnEventFieldOfHonorRankingRewardReceive;
        CsGameEventUIToUI.Instance.EventHonorShopProductBuy -= OnEventHonorShopProductBuy;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_csPopupCalculator != null)
        {
            OnEventCloseCalculator();
        }
        if (m_trItemInfo != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Center);
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickGoBackDungeonList()
    {
        transform.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventGoBackDungeonCartegoryList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorInfo(ClientCommon.PDFieldOfHonorHistory[] pdFieldOfHonorHistory, ClientCommon.PDFieldOfHonorHero[] pdFieldOfHonorHero)
    {
        m_pdFieldOfHonorHero = pdFieldOfHonorHero;
        UpdateFieldOfHonor(pdFieldOfHonorHistory);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGetReward()
    {
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A30_TXT_02001"));
        CsDungeonManager.Instance.SendFieldOfHonorRankingRewardReceive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenRewardItemInfo(int nRankerIndex, int nItemIndex)
    {
        List<CsFieldOfHonorRankingReward> listCsFieldOfHonorRankingReward = new List<CsFieldOfHonorRankingReward>();
        listCsFieldOfHonorRankingReward.Clear();

        if (nRankerIndex > -1)
        {
            listCsFieldOfHonorRankingReward = CsGameData.Instance.FieldOfHonor.FieldOfHonorRankingRewardList.FindAll(a => a.LowRanking >= m_pdFieldOfHonorRanking[nRankerIndex].ranking &&
                                                                                                                      a.HighRanking <= m_pdFieldOfHonorRanking[nRankerIndex].ranking);
        }
        else
        {
            listCsFieldOfHonorRankingReward = CsGameData.Instance.FieldOfHonor.FieldOfHonorRankingRewardList.FindAll(a => a.LowRanking >= CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking &&
                                                                                                                      a.HighRanking <= CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking);
        }

        Debug.Log("nRankerIndex            : " + nRankerIndex + " nItemIndex  : " + nItemIndex + " ? " + listCsFieldOfHonorRankingReward.Count);

        if (listCsFieldOfHonorRankingReward.Count > 0)
        {
            CsFieldOfHonorRankingReward csFieldOfHonorRankingReward = listCsFieldOfHonorRankingReward[nItemIndex];

            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(() => OpenPopupItemInfo(csFieldOfHonorRankingReward)));
            }
            else
            {
                OpenPopupItemInfo(csFieldOfHonorRankingReward);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupRanking()
    {
        CsDungeonManager.Instance.SendOnEventResFieldOfHonorTopRankingList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorTopRankingList(ClientCommon.PDFieldOfHonorRanking[] pdFieldOfHonorRanking)
    {
        m_pdFieldOfHonorRanking = pdFieldOfHonorRanking;
        OpenRankingPopup();
        UpdateRankingPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupRanking()
    {
        CloseRankingPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupHonorShop()
    {
        m_trShop.gameObject.SetActive(true);
        UpdateHonorShop();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseShop()
    {
        m_trShop.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBuyHonorShopProduct(int nIndex)
    {
        m_nHonorShopProductIndex = nIndex;
        CsHonorShopProduct csHonorShopProduct = CsGameData.Instance.HonorShopProductList[nIndex];

        if (m_goPopupCalculator == null)
        {
            StartCoroutine(LoadPopupCalculatorCoroutine(() => OpenPopupBuyItem(csHonorShopProduct)));
        }
        else
        {
            OpenPopupBuyItem(csHonorShopProduct);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickChallenge(int nTargetIndex)
    {
        CsDungeonManager.Instance.SendContinentExitForFieldOfHonorChallenge(m_pdFieldOfHonorHero[nTargetIndex].ranking);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTargetInfo(int nTargetIndex)
    {
        Guid guid = Guid.Empty;
        CsHeroInfo csHeroInfo = new CsHeroInfo(guid, m_pdFieldOfHonorHero[nTargetIndex].name, m_pdFieldOfHonorHero[nTargetIndex].nationId, m_pdFieldOfHonorHero[nTargetIndex].jobId,
                                           m_pdFieldOfHonorHero[nTargetIndex].level, false, m_pdFieldOfHonorHero[nTargetIndex].battlePower, m_pdFieldOfHonorHero[nTargetIndex].equippedWingId,
                                           m_pdFieldOfHonorHero[nTargetIndex].mainGearEnchantLevelSetNo, m_pdFieldOfHonorHero[nTargetIndex].subGearSoulstoneLevelSetNo,
                                           m_pdFieldOfHonorHero[nTargetIndex].equippedWeapon, m_pdFieldOfHonorHero[nTargetIndex].equippedArmor,
                                           m_pdFieldOfHonorHero[nTargetIndex].equippedSubGears, m_pdFieldOfHonorHero[nTargetIndex].wingParts,
                                           m_pdFieldOfHonorHero[nTargetIndex].realAttrs, m_pdFieldOfHonorHero[nTargetIndex].wingStep,
                                           m_pdFieldOfHonorHero[nTargetIndex].wingLevel, m_pdFieldOfHonorHero[nTargetIndex].wings, m_pdFieldOfHonorHero[nTargetIndex].guildId, m_pdFieldOfHonorHero[nTargetIndex].guildName, m_pdFieldOfHonorHero[nTargetIndex].guildMemberGrade,
                                           m_pdFieldOfHonorHero[nTargetIndex].customPresetHair, m_pdFieldOfHonorHero[nTargetIndex].customFaceJawHeight, m_pdFieldOfHonorHero[nTargetIndex].customFaceJawWidth,
                                           m_pdFieldOfHonorHero[nTargetIndex].customFaceJawEndHeight, m_pdFieldOfHonorHero[nTargetIndex].customFaceWidth, m_pdFieldOfHonorHero[nTargetIndex].customFaceEyebrowHeight,
                                           m_pdFieldOfHonorHero[nTargetIndex].customFaceEyebrowRotation, m_pdFieldOfHonorHero[nTargetIndex].customFaceEyesWidth, m_pdFieldOfHonorHero[nTargetIndex].customFaceNoseHeight,
                                           m_pdFieldOfHonorHero[nTargetIndex].customFaceNoseWidth, m_pdFieldOfHonorHero[nTargetIndex].customFaceMouthHeight, m_pdFieldOfHonorHero[nTargetIndex].customFaceMouthWidth,
                                           m_pdFieldOfHonorHero[nTargetIndex].customBodyHeadSize, m_pdFieldOfHonorHero[nTargetIndex].customBodyArmsLength, m_pdFieldOfHonorHero[nTargetIndex].customBodyArmsWidth,
                                           m_pdFieldOfHonorHero[nTargetIndex].customBodyChestSize, m_pdFieldOfHonorHero[nTargetIndex].customBodyWaistWidth, m_pdFieldOfHonorHero[nTargetIndex].customBodyHipsSize,
                                           m_pdFieldOfHonorHero[nTargetIndex].customBodyPelvisWidth, m_pdFieldOfHonorHero[nTargetIndex].customBodyLegsLength, m_pdFieldOfHonorHero[nTargetIndex].customBodyLegsWidth,
                                           m_pdFieldOfHonorHero[nTargetIndex].customColorSkin, m_pdFieldOfHonorHero[nTargetIndex].customColorEyes, m_pdFieldOfHonorHero[nTargetIndex].customColorBeardAndEyebrow,
                                           m_pdFieldOfHonorHero[nTargetIndex].customColorHair, m_pdFieldOfHonorHero[nTargetIndex].equippedCostumeId, m_pdFieldOfHonorHero[nTargetIndex].appliedCostumeEffectId, m_pdFieldOfHonorHero[nTargetIndex].displayTitleId);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.ViewOtherUsers, EnSubMenu.OtherUsers, csHeroInfo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRankerInfo(int nIndex)
    {
        //서버수정필요
        CsDungeonManager.Instance.SendEventResFieldOfHonorRankerInfo(m_pdFieldOfHonorRanking[nIndex].heroId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorRankingRewardReceive(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        Transform trRewardFrame = m_trBack.Find("ImageRewardFrame");
        Button buttonGetReward = trRewardFrame.Find("ButtonGetReawrd").GetComponent<Button>();

        if (CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking == 0)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonGetReward, false);
        }
        else
        {
            if (CsGameData.Instance.FieldOfHonor.DailyFieldOfHonorRankingNo == CsGameData.Instance.FieldOfHonor.RewardedDailyFieldOfHonorRankingNo)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonGetReward, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonGetReward, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHonorShopProductBuy()
    {
        UpdateHonorShop();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsDungeonManager.Instance.SendFieldOfHonorInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorRankerInfo(ClientCommon.PDFieldOfHonorHero pdFieldOfHonorHero)
    {
        Guid guid = Guid.Empty;
        CsHeroInfo csHeroInfo = new CsHeroInfo(guid, pdFieldOfHonorHero.name, pdFieldOfHonorHero.nationId, pdFieldOfHonorHero.jobId,
                                           pdFieldOfHonorHero.level, false, pdFieldOfHonorHero.battlePower, pdFieldOfHonorHero.equippedWingId,
                                           pdFieldOfHonorHero.mainGearEnchantLevelSetNo, pdFieldOfHonorHero.subGearSoulstoneLevelSetNo,
                                           pdFieldOfHonorHero.equippedWeapon, pdFieldOfHonorHero.equippedArmor,
                                           pdFieldOfHonorHero.equippedSubGears, pdFieldOfHonorHero.wingParts,
                                           pdFieldOfHonorHero.realAttrs, pdFieldOfHonorHero.wingStep,
                                           pdFieldOfHonorHero.wingLevel, pdFieldOfHonorHero.wings, pdFieldOfHonorHero.guildId, pdFieldOfHonorHero.guildName, pdFieldOfHonorHero.guildMemberGrade,
                                           pdFieldOfHonorHero.customPresetHair, pdFieldOfHonorHero.customFaceJawHeight, pdFieldOfHonorHero.customFaceJawWidth,
                                           pdFieldOfHonorHero.customFaceJawEndHeight, pdFieldOfHonorHero.customFaceWidth, pdFieldOfHonorHero.customFaceEyebrowHeight,
                                           pdFieldOfHonorHero.customFaceEyebrowRotation, pdFieldOfHonorHero.customFaceEyesWidth, pdFieldOfHonorHero.customFaceNoseHeight,
                                           pdFieldOfHonorHero.customFaceNoseWidth, pdFieldOfHonorHero.customFaceMouthHeight, pdFieldOfHonorHero.customFaceMouthWidth,
                                           pdFieldOfHonorHero.customBodyHeadSize, pdFieldOfHonorHero.customBodyArmsLength, pdFieldOfHonorHero.customBodyArmsWidth,
                                           pdFieldOfHonorHero.customBodyChestSize, pdFieldOfHonorHero.customBodyWaistWidth, pdFieldOfHonorHero.customBodyHipsSize,
                                           pdFieldOfHonorHero.customBodyPelvisWidth, pdFieldOfHonorHero.customBodyLegsLength, pdFieldOfHonorHero.customBodyLegsWidth,
                                           pdFieldOfHonorHero.customColorSkin, pdFieldOfHonorHero.customColorEyes, pdFieldOfHonorHero.customColorBeardAndEyebrow,
                                           pdFieldOfHonorHero.customColorHair, pdFieldOfHonorHero.equippedCostumeId, pdFieldOfHonorHero.appliedCostumeEffectId, pdFieldOfHonorHero.displayTitleId);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.ViewOtherUsers, EnSubMenu.OtherUsers, csHeroInfo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedInventoryScrollbar(Scrollbar scrollbar)
    {
        if (m_nLoadCount < m_pdFieldOfHonorRanking.Length)
        {
            int nUpdateLine = Mathf.FloorToInt(Mathf.Lerp(0, m_pdFieldOfHonorRanking.Length, (1 - scrollbar.value))); // 최소 최대값 확인 필요.

            if (nUpdateLine > m_nstandardPosition)
            {
                int nStartCount = m_nLoadCount;
                int nEndCount = m_nLoadCount + 10;

                if (nEndCount >= m_pdFieldOfHonorRanking.Length)
                {
                    nEndCount = m_pdFieldOfHonorRanking.Length;
                }

                for (int i = nStartCount; i < nEndCount; i++)
                {
                    int nSlotNum = i;
                    CreateRanker(nSlotNum);
                }

                m_nstandardPosition = nUpdateLine;
            }
        }
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Button buttonBackDungeonList = m_trBack.Find("ButtonBackDungeonList").GetComponent<Button>();
        buttonBackDungeonList.onClick.RemoveAllListeners();
        buttonBackDungeonList.onClick.AddListener(OnClickGoBackDungeonList);
        buttonBackDungeonList.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textBackDungeonList = buttonBackDungeonList.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBackDungeonList);
        textBackDungeonList.text = CsConfiguration.Instance.GetString("A17_BTN_00003");

        Transform trMyInfo = m_trBack.Find("ImageFrameMyInfo");

        Text textRank = trMyInfo.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = CsConfiguration.Instance.GetString("A31_TXT_00001");

        m_textMyInfoRank = trMyInfo.Find("TextRankValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMyInfoRank);
        m_textMyInfoRank.text = "";

        Text textCp = trMyInfo.Find("TextCp").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCp);
        textCp.text = CsConfiguration.Instance.GetString("A31_TXT_00003");

        m_textMyInfoCp = trMyInfo.Find("TextCpValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMyInfoCp);
        m_textMyInfoCp.text = "";

        Text textCount = trMyInfo.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);
        textCount.text = CsConfiguration.Instance.GetString("A31_TXT_00004");

        m_textMyInfoCount = trMyInfo.Find("TextCountValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textMyInfoCount);
        m_textMyInfoCount.text = "";

        Transform trRewardFrame = m_trBack.Find("ImageRewardFrame");

        m_textResetGuide = trRewardFrame.Find("TextResetGuide").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textResetGuide);

        Button buttonReward0 = trRewardFrame.Find("ButtonReward0").GetComponent<Button>();
        buttonReward0.onClick.RemoveAllListeners();
        buttonReward0.onClick.AddListener(() => OnClickOpenRewardItemInfo(-1, 0));
        buttonReward0.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonReward1 = trRewardFrame.Find("ButtonReward1").GetComponent<Button>();
        buttonReward1.onClick.RemoveAllListeners();
        buttonReward1.onClick.AddListener(() => OnClickOpenRewardItemInfo(-1, 1));
        buttonReward1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonGetReward = trRewardFrame.Find("ButtonGetReawrd").GetComponent<Button>();
        buttonGetReward.onClick.RemoveAllListeners();
        buttonGetReward.onClick.AddListener(OnClickGetReward);
        buttonGetReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGetReward = buttonGetReward.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGetReward);
        textGetReward.text = CsConfiguration.Instance.GetString("A31_BTN_00001");

        Transform trHistory = m_trBack.Find("ImageHistory");

        for (int i = 0; i < trHistory.childCount; i++)
        {
            Text textHistory = trHistory.Find("TextHistory" + i).GetComponent<Text>();
            CsUIData.Instance.SetFont(textHistory);
            textHistory.text = "";
        }

        Button buttonOpenRanking = m_trBack.Find("ButtonOpenRanking").GetComponent<Button>();
        buttonOpenRanking.onClick.RemoveAllListeners();
        buttonOpenRanking.onClick.AddListener(OnClickOpenPopupRanking);
        buttonOpenRanking.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textOpenRanking = buttonOpenRanking.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOpenRanking);
        textOpenRanking.text = CsConfiguration.Instance.GetString("A31_BTN_00002");

        Button buttonHonorShop = m_trBack.Find("ButtonHonorShop").GetComponent<Button>();
        buttonHonorShop.onClick.RemoveAllListeners();
        buttonHonorShop.onClick.AddListener(OnClickOpenPopupHonorShop);
        buttonHonorShop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textOpenHonorShop = buttonHonorShop.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOpenHonorShop);
        textOpenHonorShop.text = CsConfiguration.Instance.GetString("A31_BTN_00003");

        Transform trBattleList = m_trBack.Find("BattleList");

        for (int i = 0; i < 5; i++)
        {
            int nIndex = i;
            Transform trBattleTarget = trBattleList.Find("BattleTarget" + i);

            Text textTargetRank = trBattleTarget.Find("TextRank").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTargetRank);

            Text textTargetName = trBattleTarget.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTargetName);

            Text textTargetCp = trBattleTarget.Find("TextCp").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTargetCp);

            Button buttonChallenge = trBattleTarget.Find("ButtonChallenge").GetComponent<Button>();
            buttonChallenge.onClick.RemoveAllListeners();
            buttonChallenge.onClick.AddListener(() => OnClickChallenge(nIndex));
            buttonChallenge.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textChallenge = buttonChallenge.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textChallenge);
            textChallenge.text = CsConfiguration.Instance.GetString("A31_BTN_00005");

            Button buttonTargetInfo = trBattleTarget.Find("ButtonHefoInfo").GetComponent<Button>();
            buttonTargetInfo.onClick.RemoveAllListeners();
            buttonTargetInfo.onClick.AddListener(() => OnClickTargetInfo(nIndex));
            buttonTargetInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        //랭킹팝업

        m_trPopupRanking = m_trBack.Find("ImageRanking");
        m_trPopupRanking.gameObject.SetActive(false);

        Text textPopupName = m_trPopupRanking.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A31_TITLE_00002");

        Button buttonPopupRankingClose = m_trPopupRanking.Find("ButtonClose").GetComponent<Button>();
        buttonPopupRankingClose.onClick.RemoveAllListeners();
        buttonPopupRankingClose.onClick.AddListener(OnClickClosePopupRanking);

        Transform trTab = m_trPopupRanking.Find("Tab");

        Text textRanking = trTab.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRanking);
        textRanking.text = CsConfiguration.Instance.GetString("A31_TXT_03001");

        Text textRankerNation = trTab.Find("TextNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankerNation);
        textRankerNation.text = CsConfiguration.Instance.GetString("A31_TXT_03002");

        Text textRankerName = trTab.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankerName);
        textRankerName.text = CsConfiguration.Instance.GetString("A31_TXT_03003");

        Text textRankerCp = trTab.Find("TextCp").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankerCp);
        textRankerCp.text = CsConfiguration.Instance.GetString("A31_TXT_03004");

        Text textRankerReward = trTab.Find("TextReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRankerReward);
        textRankerReward.text = CsConfiguration.Instance.GetString("A31_TXT_03005");

        Text textMyRanking = m_trPopupRanking.Find("TextMyHeroRaking").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyRanking);
        textMyRanking.text = CsConfiguration.Instance.GetString("A31_TXT_03006");

        Text textMyRankingValue = m_trPopupRanking.Find("TextMyHeroRakingValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyRankingValue);
        textMyRankingValue.text = "";

        // 명예상점
        m_trShop = m_trBack.Find("HonorShop");
        Transform trShopBack = m_trShop.Find("ImageBackground");

        Text textShopName = trShopBack.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textShopName);
        textShopName.text = CsConfiguration.Instance.GetString("A14_NAME_00001");

        Button buttonCloseShop = trShopBack.Find("ButtonClose").GetComponent<Button>();
        buttonCloseShop.onClick.RemoveAllListeners();
        buttonCloseShop.onClick.AddListener(OnClickCloseShop);
        buttonCloseShop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textResource = trShopBack.Find("ImageResourceFrame/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textResource);
        textResource.text = CsGameData.Instance.MyHeroInfo.HonorPoint.ToString("#,##0");
    }



    //---------------------------------------------------------------------------------------------------
    void UpdateFieldOfHonor(ClientCommon.PDFieldOfHonorHistory[] pdFieldOfHonorHistory)
    {
        m_trBack.gameObject.SetActive(true);

        //내정보세팅
        m_textMyInfoRank.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00002"), CsGameData.Instance.FieldOfHonor.MyRanking);
        m_textMyInfoCp.text = CsGameData.Instance.MyHeroInfo.BattlePower.ToString("#,##0");
        m_textMyInfoCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.FieldOfHonorEnterCount - CsGameData.Instance.FieldOfHonor.FieldOfHonorDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.FieldOfHonorEnterCount);

        //기록세팅
        Transform trHistory = m_trBack.Find("ImageHistory");

        for (int i = 0; i < pdFieldOfHonorHistory.Length; i++)
        {
            Text textHistory = trHistory.Find("TextHistory" + i).GetComponent<Text>();
            CsUIData.Instance.SetFont(textHistory);

            string strTime = "";

            TimeSpan timespanRegtime = CsGameData.Instance.MyHeroInfo.CurrentDateTime - pdFieldOfHonorHistory[i].regTime;

            if ((int)timespanRegtime.TotalDays > 0)
            {
                int diffMonth = 0;
                DateTime datetimeAdded = pdFieldOfHonorHistory[i].regTime.DateTime;

                while (true)
                {
                    datetimeAdded = datetimeAdded.AddMonths(1);

                    if (DateTime.Compare(datetimeAdded, CsGameData.Instance.MyHeroInfo.CurrentDateTime) == 1)
                    {
                        break;
                    }
                    else
                    {
                        diffMonth++;
                    }
                }

                if (diffMonth > 12)
                {
                    int diffYear = 0;
                    datetimeAdded = pdFieldOfHonorHistory[i].regTime.DateTime;

                    while (true)
                    {
                        datetimeAdded = datetimeAdded.AddYears(1);

                        if (DateTime.Compare(datetimeAdded, CsGameData.Instance.MyHeroInfo.CurrentDateTime) == 1)
                        {
                            break;
                        }
                        else
                        {
                            diffYear++;
                        }
                    }

                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_5"), diffYear);
                }
                else if (diffMonth < 1)
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_3"), (int)timespanRegtime.TotalDays);
                }
                else
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_4"), diffMonth);
                }
            }
            else
            {
                if ((int)timespanRegtime.TotalHours > 0)
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_2"), (int)timespanRegtime.TotalHours);
                }
                else if (timespanRegtime.TotalMinutes > 0)
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_1"), (int)timespanRegtime.TotalMinutes);
                }
            }
            
            /*
            if (pdFieldOfHonorHistory[i].regTime.Month > 0)
            {
                strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_5"), pdFieldOfHonorHistory[i].regTime.Month);
            }
            else if (pdFieldOfHonorHistory[i].regTime.Day > 0)
            {
                strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_4"), pdFieldOfHonorHistory[i].regTime.Day);
            }
            else if (pdFieldOfHonorHistory[i].regTime.Hour > 0)
            {
                strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_3"), pdFieldOfHonorHistory[i].regTime.Hour);
            }
            else if (pdFieldOfHonorHistory[i].regTime.Minute > 0)
            {
                strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_2"), pdFieldOfHonorHistory[i].regTime.Minute);
            }
            else if (pdFieldOfHonorHistory[i].regTime.Second > 0)
            {
                strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_1"), pdFieldOfHonorHistory[i].regTime.Second);
            }
            */

            if (pdFieldOfHonorHistory[i].isChallenged)
            {
                //도전이면
                if (pdFieldOfHonorHistory[i].isWin)
                {
                    //이겻으면
                    textHistory.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00008"), strTime, pdFieldOfHonorHistory[i].targetHeroName, pdFieldOfHonorHistory[i].ranking);
                }
                else
                {
                    //졌으면
                    textHistory.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00009"), strTime, pdFieldOfHonorHistory[i].targetHeroName);
                }
            }
            else
            {
                //도전받은거
                if (pdFieldOfHonorHistory[i].isWin)
                {
                    //이겻으면
                    textHistory.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00010"), strTime, pdFieldOfHonorHistory[i].targetHeroName);
                }
                else
                {
                    //졌으면
                    textHistory.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00011"), strTime, pdFieldOfHonorHistory[i].targetHeroName, pdFieldOfHonorHistory[i].ranking);
                }
            }
        }

        for (int i = 0; i < trHistory.childCount - pdFieldOfHonorHistory.Length; i++)
        {
            Text textHistory = trHistory.Find("TextHistory" + (i + pdFieldOfHonorHistory.Length)).GetComponent<Text>();
            textHistory.text = "";
        }

        //보상세팅
        Transform trRewardFrame = m_trBack.Find("ImageRewardFrame");

        List<CsFieldOfHonorRankingReward> listCsFieldOfHonorRankingReward = new List<CsFieldOfHonorRankingReward>();
        listCsFieldOfHonorRankingReward.Clear();

        m_textResetGuide.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00012"), CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking);

        listCsFieldOfHonorRankingReward = CsGameData.Instance.FieldOfHonor.FieldOfHonorRankingRewardList.FindAll(a => a.LowRanking >= CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking &&
                                                                                                                      a.HighRanking <= CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking);
        for (int i = 0; i < listCsFieldOfHonorRankingReward.Count; i++)
        {
            Transform trReward = trRewardFrame.Find("ButtonReward" + i);
            trReward.gameObject.SetActive(true);

            Image imageGrade = trReward.Find("ImageRank").GetComponent<Image>();
            imageGrade.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + listCsFieldOfHonorRankingReward[i].ItemReward.Item.Grade.ToString("00"));

            Image imageIcon = trReward.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + listCsFieldOfHonorRankingReward[i].ItemReward.Item.Image);
        }

        for (int i = 0; i < 2 - listCsFieldOfHonorRankingReward.Count; i++)
        {
            Transform trReward = trRewardFrame.Find("ButtonReward" + (i + listCsFieldOfHonorRankingReward.Count));
            trReward.gameObject.SetActive(false);
        }

        Button buttonGetReward = trRewardFrame.Find("ButtonGetReawrd").GetComponent<Button>();

        if (CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking == 0)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonGetReward, false);
        }
        else
        {
            if (CsGameData.Instance.FieldOfHonor.DailyFieldOfHonorRankingNo == CsGameData.Instance.FieldOfHonor.RewardedDailyFieldOfHonorRankingNo)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonGetReward, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonGetReward, true);
            }
        }

        //대결상대 세팅
        Transform trBattleList = m_trBack.Find("BattleList");

        for (int i = 0; i < m_pdFieldOfHonorHero.Length; i++)
        {
            Transform trTarget = trBattleList.Find("BattleTarget" + i);

            if (m_pdFieldOfHonorHero[i] == null)
            {
                trTarget.gameObject.SetActive(false);
            }
            else
            {
                trTarget.gameObject.SetActive(true);

                Image imageJob = trTarget.Find("ImageJob").GetComponent<Image>();
                imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + m_pdFieldOfHonorHero[i].jobId);

                Text textTargetRank = trTarget.Find("TextRank").GetComponent<Text>();
                textTargetRank.text = string.Format(CsConfiguration.Instance.GetString("A31_TXT_00002"), m_pdFieldOfHonorHero[i].ranking);

                Text textTargetName = trTarget.Find("TextName").GetComponent<Text>();
                textTargetName.text = m_pdFieldOfHonorHero[i].name;

                Text textTargetCp = trTarget.Find("TextCp").GetComponent<Text>();
                textTargetCp.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), m_pdFieldOfHonorHero[i].battlePower.ToString("#,###"));
            }
        }

        Scrollbar scrollbar = m_trPopupRanking.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((ison) => OnValueChangedInventoryScrollbar(scrollbar));
    }


    //---------------------------------------------------------------------------------------------------
    void OpenRankingPopup()
    {
        m_trPopupRanking.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void CloseRankingPopup()
    {
        m_trPopupRanking.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRankingPopup()
    {
        m_nLoadCount = 0;
        m_nstandardPosition = 0;

        Transform trContents = m_trPopupRanking.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < trContents.childCount; i++)
        {
            trContents.GetChild(i).gameObject.SetActive(false);
        }

        RectTransform rectTransform = trContents.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);

        int nMax = 10;

        if (m_pdFieldOfHonorRanking.Length < 10)
        {
            nMax = m_pdFieldOfHonorRanking.Length;
        }

        //스크롤뷰
        for (int i = 0; i < nMax; i++)
        {
            CreateRanker(i);
        }

        //
        Text textMyRankingValue = m_trPopupRanking.Find("TextMyHeroRakingValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyRankingValue);
        textMyRankingValue.text = CsGameData.Instance.FieldOfHonor.MyRanking.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void CreateRanker(int nIndex)
    {
        if (m_pdFieldOfHonorRanking[nIndex] == null)
        {
            return;
        }

        Transform trContents = m_trPopupRanking.Find("Scroll View/Viewport/Content");
        Transform trRanker = trContents.Find("Ranker" + m_pdFieldOfHonorRanking[nIndex].ranking);

        if (m_goRanker == null)
        {
            m_goRanker = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/Ranker");
        }

        if (trRanker == null)
        {
            GameObject goRanker = Instantiate(m_goRanker, trContents);
            goRanker.name = "Ranker" + m_pdFieldOfHonorRanking[nIndex].ranking;
            trRanker = goRanker.transform;
        }
        else
        {
            trRanker.gameObject.SetActive(true);
        }

        Button buttonHeroInfo = trRanker.Find("ButtonHeroInfo").GetComponent<Button>();
        buttonHeroInfo.onClick.RemoveAllListeners();
        buttonHeroInfo.onClick.AddListener(() => OnClickRankerInfo(nIndex));
        buttonHeroInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonRewardItem0 = trRanker.Find("ButtonReward0").GetComponent<Button>();
        buttonRewardItem0.onClick.RemoveAllListeners();
        buttonRewardItem0.onClick.AddListener(() => OnClickOpenRewardItemInfo(nIndex, 0));
        buttonRewardItem0.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonRewardItem1 = trRanker.Find("ButtonReward1").GetComponent<Button>();
        buttonRewardItem1.onClick.RemoveAllListeners();
        buttonRewardItem1.onClick.AddListener(() => OnClickOpenRewardItemInfo(nIndex, 1));
        buttonRewardItem1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textName = trRanker.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = m_pdFieldOfHonorRanking[nIndex].name;

        Text textRank = trRanker.Find("TextRank").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRank);
        textRank.text = m_pdFieldOfHonorRanking[nIndex].ranking.ToString("#,##0");

        Text textNation = trRanker.Find("TextNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNation);

        CsNation csNation = CsGameData.Instance.GetNation(m_pdFieldOfHonorRanking[nIndex].nationId);
        textNation.text = csNation.Name;

        Image imageNation = trRanker.Find("ImageNation").GetComponent<Image>();

        if (imageNation.sprite == null || imageNation.sprite.name != "ico_nation" + m_pdFieldOfHonorRanking[nIndex].nationId)
        {
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + m_pdFieldOfHonorRanking[nIndex].nationId);
        }

        Text textCp = trRanker.Find("TextCp").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCp);
        textCp.text = m_pdFieldOfHonorRanking[nIndex].battlePower.ToString("#,##0");

        //보상세팅

        List<CsFieldOfHonorRankingReward> listCsFieldOfHonorRankingReward = new List<CsFieldOfHonorRankingReward>();
        listCsFieldOfHonorRankingReward.Clear();

        listCsFieldOfHonorRankingReward = CsGameData.Instance.FieldOfHonor.FieldOfHonorRankingRewardList.FindAll(a => a.LowRanking >= m_pdFieldOfHonorRanking[nIndex].ranking &&
                                                                                                                      a.HighRanking <= m_pdFieldOfHonorRanking[nIndex].ranking);

        for (int i = 0; i < listCsFieldOfHonorRankingReward.Count; i++)
        {
            Transform trReward = trRanker.Find("ButtonReward" + i);
            trReward.gameObject.SetActive(true);

            Image imageGrade = trReward.Find("ImageRank").GetComponent<Image>();
            imageGrade.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + listCsFieldOfHonorRankingReward[i].ItemReward.Item.Grade.ToString("00"));

            Image imageIcon = trReward.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + listCsFieldOfHonorRankingReward[i].ItemReward.Item.Image);
        }

        for (int i = 0; i < 2 - listCsFieldOfHonorRankingReward.Count; i++)
        {
            Transform trReward = trRanker.Find("ButtonReward" + (i + listCsFieldOfHonorRankingReward.Count));
            trReward.gameObject.SetActive(false);
        }

        m_nLoadCount++;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHonorShop()
    {
        Transform trShopBack = m_trShop.Find("ImageBackground");

        Text textResource = trShopBack.Find("ImageResourceFrame/Text").GetComponent<Text>();
        textResource.text = CsGameData.Instance.MyHeroInfo.HonorPoint.ToString("#,##0");

        Transform trContent = trShopBack.Find("Scroll View/Viewport/Content");

        //상품생성

        if (m_goShopProduct == null)
        {
            m_goShopProduct = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/HonorShopItem");
        }

        for (int i = 0; i < CsGameData.Instance.HonorShopProductList.Count; i++)
        {
            int nIndex = i;
            Transform trProduct = trContent.Find("HonorShopItem" + i);

            if (trProduct == null)
            {
                GameObject goProduct = Instantiate(m_goShopProduct, trContent);
                goProduct.name = "HonorShopItem" + i;
                trProduct = goProduct.transform;
            }
            else
            {
                trProduct.gameObject.SetActive(true);
            }

            Text textName = trProduct.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = CsGameData.Instance.HonorShopProductList[i].Item.Name;

            Image imageIcon = trProduct.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsGameData.Instance.HonorShopProductList[i].Item.Image);

            Text textPrice = trProduct.Find("ImageResourceFrame/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPrice);
            textPrice.text = CsGameData.Instance.HonorShopProductList[i].RequiredHonorPoint.ToString("#,##0");

            Button buttonBuyProduct = trProduct.Find("ButtonBuySkillBook").GetComponent<Button>();
            buttonBuyProduct.onClick.RemoveAllListeners();
            buttonBuyProduct.onClick.AddListener(() => OnClickBuyHonorShopProduct(nIndex));
            buttonBuyProduct.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            //Text textBuyButton = buttonBuyProduct.transform.Find("Text").GetComponent<Text>();
            //CsUIData.Instance.SetFont(textBuyButton);
            //textBuyButton.text = CsConfiguration.Instance.GetString("A14_BTN_00005");
        }

        for (int i = 0; i < trContent.childCount - CsGameData.Instance.HonorShopProductList.Count; i++)
        {
            Transform trProduct = trContent.Find("HonorShopItem" + (i + CsGameData.Instance.HonorShopProductList.Count));

            if (trProduct != null)
            {
                trProduct.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCalculatorCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupBuyItem(CsHonorShopProduct csHonorShopProduct)
    {
        Transform trPopup = m_trPopupList.Find("PopupCalculator");

        if (trPopup == null)
        {
            GameObject goPopupBuyCount = Instantiate(m_goPopupCalculator, m_trPopupList);
            goPopupBuyCount.name = "PopupCalculator";
            trPopup = goPopupBuyCount.transform;
        }
        else
        {
            trPopup.gameObject.SetActive(false);
        }

        m_csPopupCalculator = trPopup.GetComponent<CsPopupCalculator>();
        m_csPopupCalculator.EventBuyItem += OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;
        m_csPopupCalculator.DisplayItem(csHonorShopProduct.Item, csHonorShopProduct.ItemOwned, csHonorShopProduct.RequiredHonorPoint, EnResourceType.Honor);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBuyItem(int nCount)
    {
        CsCommandEventManager.Instance.SendHonorShopProductBuy(CsGameData.Instance.HonorShopProductList[m_nHonorShopProductIndex].ProductId, nCount);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseCalculator()
    {
        m_csPopupCalculator.EventBuyItem -= OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator -= OnEventCloseCalculator;
        m_csPopupCalculator = null;

        Transform trPopup = m_trPopupList.Find("PopupCalculator");

        if (trPopup != null)
        {
            Destroy(trPopup.gameObject);
        }
    }

    GameObject m_goPopupItemInfo;
    Transform m_trItemInfo;
    CsPopupItemInfo m_csPopupItemInfo;

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsFieldOfHonorRankingReward csFieldOfHonorRankingReward)
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        Transform trPopupList = trCanvas2.Find("PopupList");

        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csFieldOfHonorRankingReward.ItemReward.Item, csFieldOfHonorRankingReward.ItemReward.ItemCount, csFieldOfHonorRankingReward.ItemReward.ItemOwned, -1, false, false);
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
