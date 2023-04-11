using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsPanelFishing : MonoBehaviour
{
    //낚시 아이템 ID 기본값
    const int m_nItemIdDefault = 1401;

    Transform m_trBaitItemList;
    Transform m_trRecycle;
    Transform m_trAutoCancel;
    Transform m_trPopupFishingParty;

    int m_nSpotId = 0;
    bool m_bFishingZone = false;
    bool m_bAutoItemUse = true;
    bool m_bSurroundingFishingParty = false;

    CsSimpleParty[] m_simpleParties;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsFishingQuestManager.Instance.EventFishingZone += OnEventFishingZone;
        CsFishingQuestManager.Instance.EventFishingCastingCompleted += OnEventFishingCastingCompleted;
        CsFishingQuestManager.Instance.EventFishingCanceled += OnEventFishingCanceled;
        CsFishingQuestManager.Instance.EventMyHeroFishingCanceled += OnEventMyHeroFishingCanceled;
        CsFishingQuestManager.Instance.EventFishingStart += OnEventFishingStart;

        CsGameEventUIToUI.Instance.EventFishingBaitUse += OnEventFishingBaitUse;
        CsGameEventUIToUI.Instance.EventAutoMoveFishingZone += OnEventAutoMoveFishingZone;
        CsGameEventUIToUI.Instance.EventMailReceiveAll += OnEventMailReceiveAll;
        CsGameEventUIToUI.Instance.EventMailReceive += OnEventMailReceive;


        //CsGameEventUIToUI.Instance.EventPartySurroundingPartyList += OnEventPartySurroundingPartyList;
        //CsGameEventUIToUI.Instance.EventPartyApplicationAccepted += OnEventPartyApplicationAccepted;
        //CsGameEventUIToUI.Instance.EventPartyApplicationRefused += OnEventPartyApplicationRefuse;
        //CsGameEventUIToUI.Instance.EventPartyApplicationLifetimeEnded += OnEventPartyApplicationLifetimeEnded;
        //CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsFishingQuestManager.Instance.EventFishingZone -= OnEventFishingZone;
        CsFishingQuestManager.Instance.EventFishingCastingCompleted -= OnEventFishingCastingCompleted;
        CsFishingQuestManager.Instance.EventFishingCanceled -= OnEventFishingCanceled;
        CsFishingQuestManager.Instance.EventMyHeroFishingCanceled -= OnEventMyHeroFishingCanceled;
        CsFishingQuestManager.Instance.EventFishingStart -= OnEventFishingStart;

        CsGameEventUIToUI.Instance.EventFishingBaitUse -= OnEventFishingBaitUse;
        CsGameEventUIToUI.Instance.EventAutoMoveFishingZone -= OnEventAutoMoveFishingZone;
        CsGameEventUIToUI.Instance.EventMailReceiveAll -= OnEventMailReceiveAll;
        CsGameEventUIToUI.Instance.EventMailReceive -= OnEventMailReceive;

        //CsGameEventUIToUI.Instance.EventPartySurroundingPartyList -= OnEventPartySurroundingPartyList;
        //CsGameEventUIToUI.Instance.EventPartyApplicationAccepted -= OnEventPartyApplicationAccepted;
        //CsGameEventUIToUI.Instance.EventPartyApplicationRefused -= OnEventPartyApplicationRefuse;
        //CsGameEventUIToUI.Instance.EventPartyApplicationLifetimeEnded -= OnEventPartyApplicationLifetimeEnded;
        //CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trBaitItemList = transform.Find("BaitItemList");
        m_trRecycle = transform.Find("ButtonRecycle");
        m_trAutoCancel = transform.Find("ImageAutoCancel");
        m_trPopupFishingParty = transform.Find("PopupFishingParty/ImageBackground");

        Button buttonRecycle = m_trRecycle.GetComponent<Button>();
        buttonRecycle.onClick.RemoveAllListeners();
        buttonRecycle.onClick.AddListener(OnClickRecycle);
        buttonRecycle.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAuto = m_trAutoCancel.Find("TextAuto").GetComponent<Text>();
        textAuto.text = CsConfiguration.Instance.GetString("A46_TXT_02004");
        CsUIData.Instance.SetFont(textAuto);

        Button buttonAutoUseItem = m_trAutoCancel.Find("ButtonAutoUseItem").GetComponent<Button>();
        buttonAutoUseItem.onClick.RemoveAllListeners();
        buttonAutoUseItem.onClick.AddListener(OnClickAutoItemUseCancel);
        buttonAutoUseItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonAuto = buttonAutoUseItem.transform.Find("Text").GetComponent<Text>();
        textButtonAuto.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
        CsUIData.Instance.SetFont(textButtonAuto);

        Text textMessage = m_trPopupFishingParty.Find("TextMessage").GetComponent<Text>();
        textMessage.text = CsConfiguration.Instance.GetString("A46_TXT_03001");
        CsUIData.Instance.SetFont(textMessage);

        Toggle toggleFishingParty = m_trPopupFishingParty.Find("ToggleFishingParty").GetComponent<Toggle>();
        toggleFishingParty.onValueChanged.RemoveAllListeners();
        toggleFishingParty.onValueChanged.AddListener(OnValueChangedParty);
        toggleFishingParty.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textToggle = toggleFishingParty.transform.Find("Text").GetComponent<Text>();
        textToggle.text = CsConfiguration.Instance.GetString("A46_TXT_03002");
        CsUIData.Instance.SetFont(textToggle);

        Button button1 = m_trPopupFishingParty.Find("Buttons/Button1").GetComponent<Button>();
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(OnClickPartyYes);
        button1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButton1 = button1.transform.Find("Text").GetComponent<Text>();
        textButton1.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
        CsUIData.Instance.SetFont(textButton1);

        Button button2 = m_trPopupFishingParty.Find("Buttons/Button2").GetComponent<Button>();
        button2.onClick.RemoveAllListeners();
        button2.onClick.AddListener(OnClickPartyNo);
        button2.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButton2 = button2.transform.Find("Text").GetComponent<Text>();
        textButton2.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
        CsUIData.Instance.SetFont(textButton2);

        for (int i = 0; i < 5; ++i)
        {
            Transform trBaitItem = m_trBaitItemList.Find("BaitItem" + i);
            Button buttonItem = trBaitItem.Find("ButtonItem").GetComponent<Button>();
            buttonItem.onClick.RemoveAllListeners();
            int nItemId = m_nItemIdDefault + i;
            buttonItem.onClick.AddListener(() => { OnClickBaitItemUse(nItemId); });
            buttonItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image imageItem = buttonItem.GetComponent<Image>();
            imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + nItemId);

            Text textItemCount = buttonItem.transform.Find("Text").GetComponent<Text>();
            int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId);
            textItemCount.text = nItemCount.ToString("#,##0");
            CsUIData.Instance.SetFont(textItemCount);

            Transform trDim = trBaitItem.Find("ImageDim");
            trDim.gameObject.SetActive(nItemCount > 0 ? false : true);
        }

        if (!PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFishingParty))
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFishingParty, 0);
        }

    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedParty(bool bIson)
    {
        int nFishingParty = bIson ? 1 : 0;

        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFishingParty, nFishingParty);
        PlayerPrefs.SetString(CsConfiguration.Instance.PlayerPrefsKeyFishingPartyDate, CsGameData.Instance.MyHeroInfo.CurrentDateTime.ToString());
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyYes()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Party, EnSubMenu.SurroundingParty);

        //if (CsGameData.Instance.MyHeroInfo.Party == null)
        //{
        //    m_bSurroundingFishingParty = true;
        //    CsCommandEventManager.Instance.SendPartySurroundingPartyList();
        //}
        //else
        //{
        //    if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept))
        //    {
        //        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, 1);
        //    }
        //}

        m_trPopupFishingParty.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyNo()
    {
        m_trPopupFishingParty.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBaitItemUse(int nItemId)
    {
        FishingStart(nItemId);
    }

    //---------------------------------------------------------------------------------------------------
    //미끼 재사용
    void OnClickRecycle()
    {
        FishingStart();
    }

    //---------------------------------------------------------------------------------------------------
    //자동 사용 중지
    void OnClickAutoItemUseCancel()
    {
        m_bAutoItemUse = false;
        m_trAutoCancel.gameObject.SetActive(false);
    }

    #endregion Event Handler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventPartySurroundingPartyList(CsSimpleParty[] simpleParties)
    {
        if (m_bSurroundingFishingParty)
        {
            m_simpleParties = simpleParties;

            if (m_simpleParties.Length == 0)
            {
                if (CsGameData.Instance.MyHeroInfo.Party == null)
                    CsCommandEventManager.Instance.SendPartyCreate();
            }
            else
            {
                for (int i = 0; i < m_simpleParties.Length; ++i)
                {
                    //파티 자리가 있으면
                    if (m_simpleParties[i].MemberCount < CsGameConfig.Instance.PartyMemberMaxCount)
                    {
                        CsCommandEventManager.Instance.SendPartyApply(m_simpleParties[i].Id);
                    }
                }
            }

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingZone(bool bEnter, int nSpotId)
    {
        m_nSpotId = nSpotId;
        m_bFishingZone = bEnter;

        if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.GetItem(m_nItemIdDefault).RequiredMinHeroLevel && 
            ((CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId) == null && CsGameData.Instance.MyHeroInfo.LocationId == 201) || 
            CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam))
        {
            if (m_bFishingZone)
            {
                DisplayBaitItem();
            }
            else
            {
                DisplayAllOff();
            }
        }
        else
        {
            DisplayAllOff();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingCastingCompleted(bool bLevelUp, long lAcquiredExp, bool bBaitEnable)
    {
        //미끼가 1개 이하로 남았을때
        if (CsFishingQuestManager.Instance.CastingCount >= CsGameData.Instance.FishingQuest.CastingCount - 1)
        {
            int nItemId = UseItemCheck();

            //사용가능한 아이템이 없거나 사용횟수를 다 사용했을때는 표시하지 않음
            if (nItemId == 0 || CsFishingQuestManager.Instance.FishingQuestDailyStartCount >= CsGameData.Instance.FishingQuest.LimitCount)
            {
                if (!bBaitEnable)
                {
                    m_bAutoItemUse = false;
                    m_trAutoCancel.gameObject.SetActive(false);
                    DisplayBaitItem();
                }
            }
            else if (!m_bAutoItemUse)
            {
                DisplayBaitItem();
            }
            else
            {
                if (!bBaitEnable)
                {
                    CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(nItemId);
                    CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
                    m_trAutoCancel.gameObject.SetActive(false);
                }
                else
                {
                    m_trAutoCancel.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingCanceled()
    {
        if (m_bFishingZone)
        {
            DisplayBaitItem();
        }
        else
        {
            DisplayAllOff();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroFishingCanceled()
    {
        if (m_bFishingZone)
        {
            DisplayBaitItem();
        }
        else
        {
            DisplayAllOff();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingStart()
    {
        m_bAutoItemUse = true;
        DisplayAllOff();

        if (CsFishingQuestManager.Instance.CastingCount >= CsGameData.Instance.FishingQuest.CastingCount - 1)
        {
            int nItemId = UseItemCheck();

            //사용가능한 아이템이 없거나 사용횟수를 다 사용했을때는 표시하지 않음
            if (nItemId == 0 || CsFishingQuestManager.Instance.FishingQuestDailyStartCount > CsGameData.Instance.FishingQuest.LimitCount)
            {
                m_bAutoItemUse = false;
                m_trAutoCancel.gameObject.SetActive(false);
            }
            else
            {
                m_trAutoCancel.gameObject.SetActive(true);
            }
        }

        bool bCheck = false;

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFishingParty))
        {
            bCheck = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyFishingParty) == 1 ? true : false;
        }

        if (bCheck)
        {
            ////파티가 있을경우 파티 자동 초대 수락을 On
            //if (CsGameData.Instance.MyHeroInfo.Party == null)
            //{
            //    m_bSurroundingFishingParty = true;
            //    CsCommandEventManager.Instance.SendPartySurroundingPartyList();
            //}
            //else
            //{
            //    if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept))
            //    {
            //        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, 1);
            //    }
            //}
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Party == null)
            {
                m_trPopupFishingParty.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingBaitUse()
    {
        if (CsGuildManager.Instance.Guild == null)
        {
            OnClickCancelContinentExitForGuildTerritoryEnter();
        }
        else if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            if (m_bFishingZone)
            {
                CsFishingQuestManager.Instance.HeroFishingStart();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A46_TXT_03003"),
                                       CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Fishing), 
                                       CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickAutoMoveFishingZoneCancel, true);
                
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"), 
                                           CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                           CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickCancelContinentExitForGuildTerritoryEnter, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCancelContinentExitForGuildTerritoryEnter()
    {
        if (m_bFishingZone)
        {
            CsFishingQuestManager.Instance.HeroFishingStart();
        }
        else
        {
            OnEventAutoMoveFishingZone();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티 수락 되었을때
    void OnEventPartyApplicationAccepted()
    {
        if (m_bSurroundingFishingParty)
            m_bSurroundingFishingParty = false;
    }

    //---------------------------------------------------------------------------------------------------
    //파티 신청이 0이 될때까지 파티가 안구해지면 파티 생성
    void OnEventPartyApplicationRefuse()
    {
        if (m_bSurroundingFishingParty)
        {
            if (CsGameData.Instance.MyHeroInfo.PartyApplicationList.Count == 0 && CsGameData.Instance.MyHeroInfo.Party == null)
            {
                CsCommandEventManager.Instance.SendPartyCreate();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationLifetimeEnded()
    {
        if (m_bSurroundingFishingParty)
        {
            if (CsGameData.Instance.MyHeroInfo.PartyApplicationList.Count == 0 && CsGameData.Instance.MyHeroInfo.Party == null)
            {
                CsCommandEventManager.Instance.SendPartyCreate();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        if (m_bSurroundingFishingParty)
        {
            m_bSurroundingFishingParty = false;

            if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept))
            {
                PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, 1);
            }
            else
            {
                PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, 1);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceiveAll(Guid[] guidMails)
    {
        if (m_bFishingZone && !CsFishingQuestManager.Instance.Fishing)
        {
            DisplayBaitItem();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceive(Guid guidMail)
    {
        if (m_bFishingZone && !CsFishingQuestManager.Instance.Fishing)
        {
            DisplayBaitItem();
        }
        else
        {
            return;
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void FishingStart(int nItemId = 0)
    {
        CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(nItemId);

        if (CsFishingQuestManager.Instance.BaitItemId != 0 && CsFishingQuestManager.Instance.CastingCount < 60)
        {
			CsFishingQuestManager.Instance.HeroFishingStart();
        }
        else if (CsFishingQuestManager.Instance.FishingQuestDailyStartCount > CsGameData.Instance.FishingQuest.LimitCount)
        {
            //일일 사용 횟수 초과
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A46_TXT_02002"));
        }
        else if (CsFishingQuestManager.Instance.BaitItemId != 0)
        {
            //사용 중인 미끼가 있을때
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A46_TXT_02001"));
        }
        else
        {
            CsCommandEventManager.Instance.SendItemUse(csInventorySlot.Index);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayBaitItem()
    {
        for (int i = 0; i < 5; ++i)
        {
            Transform trBaitItem = m_trBaitItemList.Find("BaitItem" + i);
            int nItemId = m_nItemIdDefault + i;
            Text textItemCount = trBaitItem.transform.Find("ButtonItem/Text").GetComponent<Text>();
            int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId);
            textItemCount.text = nItemCount.ToString("#,##0");

            Transform trDim = trBaitItem.Find("ImageDim");
            trDim.gameObject.SetActive(nItemCount > 0 ? false : true);
        }

        if (CsFishingQuestManager.Instance.BaitItemId == 0)
        {
            m_trBaitItemList.gameObject.SetActive(true);
            m_trRecycle.gameObject.SetActive(false);
        }
        else
        {
            m_trBaitItemList.gameObject.SetActive(false);
            m_trRecycle.gameObject.SetActive(true);
        }

        m_trAutoCancel.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayAllOff()
    {
        m_trBaitItemList.gameObject.SetActive(false);
        m_trRecycle.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    //사용가능한 아이템있는지 체크 
    int UseItemCheck()
    {
        for (int i = 4; i >= 0; --i)
        {
            if (CsGameData.Instance.MyHeroInfo.GetItemCount((m_nItemIdDefault + i)) != 0)
            {
                return (m_nItemIdDefault + i);
            }
            else
            {
                continue;
            }
        }

        return 0;
    }

    #region AutoPopup

    //---------------------------------------------------------------------------------------------------
    void OnEventAutoMoveFishingZone()
    {
        if (m_bFishingZone)
        {
			CsFishingQuestManager.Instance.HeroFishingStart();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A46_TXT_03003"),
                                       CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickAutoMoveFishingZone,
                                       CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickAutoMoveFishingZoneCancel, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAutoMoveFishingZone()
    {
        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Fishing);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAutoMoveFishingZoneCancel()
    {

    }

    #endregion AutoPopup
}
