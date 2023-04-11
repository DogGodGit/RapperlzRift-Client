using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-03)
//---------------------------------------------------------------------------------------------------

public class CsPartyManagement : CsPopupSub
{
    Transform m_trPartyList;
    Transform m_trButtonList;
    Transform m_trSelect;

    Button m_buttonFriend;
    Button m_buttonChangeMaster;
    Button m_buttonBanish;

    bool m_bFirst = true;

    Guid m_guidSelectMember = Guid.Empty;




    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventPartyMembersUpdated += OnEventPartyMembersUpdated;
        CsGameEventUIToUI.Instance.EventPartyMasterChanged += OnEventPartyMasterChanged;
        CsGameEventUIToUI.Instance.EventPartyMemberExit += OnEventPartyMemberExit;
        CsGameEventUIToUI.Instance.EventPartyMemberEnter += OnEventPartyMemberEnter;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventPartyMembersUpdated -= OnEventPartyMembersUpdated;
        CsGameEventUIToUI.Instance.EventPartyMasterChanged -= OnEventPartyMasterChanged;
        CsGameEventUIToUI.Instance.EventPartyMemberExit -= OnEventPartyMemberExit;
        CsGameEventUIToUI.Instance.EventPartyMemberEnter -= OnEventPartyMemberEnter;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
        }
        else
        {
            DisplayParty();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPartyList = transform.Find("PartyList");
        m_trSelect = transform.Find("ImageSelect");
        m_trButtonList = m_trSelect.Find("ButtonList");

        Button buttonSelect = m_trSelect.GetComponent<Button>();
        buttonSelect.onClick.RemoveAllListeners();
        buttonSelect.onClick.AddListener(OnClickPartyMemberCancle);
        buttonSelect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //친구 추가 버튼
        m_buttonFriend = m_trButtonList.Find("ButtonFriend").GetComponent<Button>();
        m_buttonFriend.onClick.RemoveAllListeners();
        m_buttonFriend.onClick.AddListener(OnClickAddFriend);
        m_buttonFriend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textFriend = m_buttonFriend.transform.Find("Text").GetComponent<Text>();
        textFriend.text = CsConfiguration.Instance.GetString("A36_BTN_00006");
        CsUIData.Instance.SetFont(textFriend);

        //방장 변경 버튼
        m_buttonChangeMaster = m_trButtonList.Find("ButtonChangeMaster").GetComponent<Button>();
        m_buttonChangeMaster.onClick.RemoveAllListeners();
        m_buttonChangeMaster.onClick.AddListener(OnClickChangeMasterCheck);
        m_buttonChangeMaster.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textChangeMaster = m_buttonChangeMaster.transform.Find("Text").GetComponent<Text>();
        textChangeMaster.text = CsConfiguration.Instance.GetString("A36_BTN_00007");
        CsUIData.Instance.SetFont(textChangeMaster);

        //파티원 강퇴 버튼
        m_buttonBanish = m_trButtonList.Find("ButtonBanish").GetComponent<Button>();
        m_buttonBanish.onClick.RemoveAllListeners();
        m_buttonBanish.onClick.AddListener(OnClickPartyMemberBanishCheck);
        m_buttonBanish.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textBanish = m_buttonBanish.transform.Find("Text").GetComponent<Text>();
        textBanish.text = CsConfiguration.Instance.GetString("A36_BTN_00008");
        CsUIData.Instance.SetFont(textBanish);

        //파티원 조회 버튼
        Button buttonLookup = m_trButtonList.Find("ButtonLookup").GetComponent<Button>();
        buttonLookup.onClick.RemoveAllListeners();
        buttonLookup.onClick.AddListener(OnClickPartyMemberLookup);
        buttonLookup.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textLookup = buttonLookup.transform.Find("Text").GetComponent<Text>();
        textLookup.text = CsConfiguration.Instance.GetString("A36_BTN_00009");
        CsUIData.Instance.SetFont(textLookup);

        DisplayParty();
    }
    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyMember(Guid guidPartyId, int nIndex)
    {
        //자기 자신을 클릭한 경우
        if (CsGameData.Instance.MyHeroInfo.HeroId == guidPartyId)
        {
            return;
        }

        m_guidSelectMember = guidPartyId;

        //방장인 경우
        if (CsGameData.Instance.MyHeroInfo.HeroId == CsGameData.Instance.MyHeroInfo.Party.Master.Id)
        {
            m_buttonBanish.gameObject.SetActive(true);
            m_buttonChangeMaster.gameObject.SetActive(true);
        }
        else
        {
            m_buttonBanish.gameObject.SetActive(false);
            m_buttonChangeMaster.gameObject.SetActive(false);
        }

        //친구일 경우 친구 추가버튼 꺼줘야됨(추후)

        Transform trPartyMember = m_trPartyList.Find("PartyHero" + nIndex);

        m_trButtonList.transform.SetParent(trPartyMember.transform);
        m_trButtonList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -90f);
        m_trButtonList.transform.SetParent(m_trSelect.transform);

        m_trSelect.gameObject.SetActive(true);

    }

    //---------------------------------------------------------------------------------------------------
    //친구 추가
    void OnClickAddFriend()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, "준비중입니다");
    }

    //---------------------------------------------------------------------------------------------------
    //멤버 파티장 변경 팝업
    void OnClickChangeMasterCheck()
    {
        if (m_guidSelectMember != Guid.Empty)
        {
            List<CsPartyMember> listPartyMember = CsGameData.Instance.MyHeroInfo.Party.PartyMemberList;
            for (int i = 0; i < listPartyMember.Count; ++i)
            {
                if (listPartyMember[i].Id == m_guidSelectMember && !listPartyMember[i].IsLoggedIn)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04010"));
                    OnClickPartyMemberCancle();
                    return;
                }
            }

            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A36_TXT_03002"),
               CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickChangeMaster,
               CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickChangeMasterCancle, true);
            OnClickPartyMemberCancle();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //멤버 강퇴 팝업
    void OnClickPartyMemberBanishCheck()
    {
        if (m_guidSelectMember != Guid.Empty)
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A36_TXT_03003"),
               CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickPartyMemberBanish,
               CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnClickPartyMemberBanishCancle, true);
            OnClickPartyMemberCancle();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //멤버 조회
    void OnClickPartyMemberLookup()
    {
        CsCommandEventManager.Instance.SendHeroInfo(m_guidSelectMember);
    }

    //---------------------------------------------------------------------------------------------------
    //선택 취소
    void OnClickPartyMemberCancle()
    {
        m_trSelect.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    //멤버 강퇴 확인
    void OnClickPartyMemberBanish()
    {
        if (m_guidSelectMember != Guid.Empty)
        {
            CsCommandEventManager.Instance.SendPartyMemberBanish(m_guidSelectMember);
            m_guidSelectMember = Guid.Empty;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //멤버 강퇴 취소
    void OnClickPartyMemberBanishCancle()
    {
        m_guidSelectMember = Guid.Empty;
    }

    //---------------------------------------------------------------------------------------------------
    //파티장 변경 확인
    void OnClickChangeMaster()
    {
        if (m_guidSelectMember != Guid.Empty)
        {
            CsCommandEventManager.Instance.SendPartyMasterChange(m_guidSelectMember);
            m_guidSelectMember = Guid.Empty;
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티장 변경 취소
    void OnClickChangeMasterCancle()
    {
        m_guidSelectMember = Guid.Empty;
    }


    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMembersUpdated()
    {
        DisplayParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMasterChanged()
    {
        DisplayParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberExit(CsPartyMember csPartyMember, bool bBanished)
    {
        DisplayParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberEnter(CsPartyMember csPartyMember)
    {
        DisplayParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        DisplayParty();
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void DisplayParty()
    {
        DisplayPartyDefault();

        List<CsPartyMember> listPartyMember = CsGameData.Instance.MyHeroInfo.Party.PartyMemberList;
        CsPartyMember csPartyMemberMaster = CsGameData.Instance.MyHeroInfo.Party.Master;

        int nMemberCount = 0;

        for (int i = 0; i < listPartyMember.Count; ++i)
        {
            if (listPartyMember[i] == csPartyMemberMaster)
            {
                PartyMember(listPartyMember[i], 0);
            }
            else
            {
                nMemberCount++;
                PartyMember(listPartyMember[i], nMemberCount);
            }
        }

    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPartyDefault()
    {
        for (int i = 0; i < CsGameConfig.Instance.PartyMemberMaxCount; ++i)
        {
            Transform trMember = m_trPartyList.Find("PartyHero" + i);

            if (trMember != null)
            {
                trMember.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void PartyMember(CsPartyMember csPartyMember, int nMemberCount)
    {
        Transform trMember = m_trPartyList.Find("PartyHero" + nMemberCount);
        if (trMember != null)
        {
            int nIndex = nMemberCount;
            Text textLevel = trMember.Find("ImageBackGround/TextLevel").GetComponent<Text>();
            textLevel.text = string.Format(CsConfiguration.Instance.GetString("A36_TXT_01005"), csPartyMember.Level);
            CsUIData.Instance.SetFont(textLevel);

            Text textName = trMember.Find("ImageBackGround/ImageBottom/TextName").GetComponent<Text>();
            textName.text = csPartyMember.Name;
            CsUIData.Instance.SetFont(textName);

            Text textJob = trMember.Find("ImageBackGround/ImageBottom/TextJob").GetComponent<Text>();
            textJob.text = csPartyMember.Job.Name;
            CsUIData.Instance.SetFont(textJob);

            Text textBattlePoint = trMember.Find("ImageBackGround/ImageBottom/TextBattlePoint").GetComponent<Text>();
            textBattlePoint.text = csPartyMember.BattlePower.ToString("#,##0");
            CsUIData.Instance.SetFont(textBattlePoint);

            Image imageEmblem = trMember.Find("ImageBackGround/ImageEmblem").GetComponent<Image>();
            imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csPartyMember.Job.JobId);

            Button buttonPartyHero = trMember.GetComponent<Button>();
            buttonPartyHero.onClick.RemoveAllListeners();
            buttonPartyHero.onClick.AddListener(() => OnClickPartyMember(csPartyMember.Id, nIndex));
            buttonPartyHero.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            trMember.gameObject.SetActive(true);
        }
    }

}
