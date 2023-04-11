using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-03)
//---------------------------------------------------------------------------------------------------

public class CsPartyBottom : CsPopupSub
{

    Button m_buttonPartyCreate;
    Button m_buttonPartyDisband;
    Button m_buttonPartyExit;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept += OnEventPartyInvitationAccept;
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted += OnEventPartyApplicationAccepted;
        CsGameEventUIToUI.Instance.EventPartyExit += OnEventPartyExit;
        CsGameEventUIToUI.Instance.EventPartyBanished += OnEventPartyBanished;
        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyDisband += OnEventPartyDisband;
        CsGameEventUIToUI.Instance.EventPartyDisbanded += OnEventPartyDisbanded;
        CsGameEventUIToUI.Instance.EventPartyMasterChanged += OnEventPartyMasterChanged;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept -= OnEventPartyInvitationAccept;
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted -= OnEventPartyApplicationAccepted;
        CsGameEventUIToUI.Instance.EventPartyExit -= OnEventPartyExit;
        CsGameEventUIToUI.Instance.EventPartyBanished -= OnEventPartyBanished;
        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyDisband -= OnEventPartyDisband;
        CsGameEventUIToUI.Instance.EventPartyDisbanded -= OnEventPartyDisbanded;
        CsGameEventUIToUI.Instance.EventPartyMasterChanged -= OnEventPartyMasterChanged;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationAccept()
    {
        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyBanished()
    {
        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyExit()
    {
        PartyCheck();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04017"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisband()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04018"));
        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisbanded()
    {
        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationAccepted()
    {
        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMasterChanged()
    {
        PartyCheck();
    }

    #endregion EventHandler



    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedInvitationAutoAccept(bool bIson, Text text)
    {
        int nAutoAccept;

        if (bIson)
        {
            text.color = CsUIData.Instance.ColorWhite;
            nAutoAccept = 1;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
            nAutoAccept = 0;
        }

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept))
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, nAutoAccept);
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, nAutoAccept);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCallAutoAccept(bool bIson, Text text)
    {
        int nAutoCall;

        if (bIson)
        {
            text.color = CsUIData.Instance.ColorWhite;
            nAutoCall = 1;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
            nAutoCall = 0;
        }

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept))
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept, nAutoCall);
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept, nAutoCall);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티 생성
    void OnClickCreateParty()
    {
        if (CsGameData.Instance.MyHeroInfo.Party == null)
        {
            CsCommandEventManager.Instance.SendPartyCreate();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티 탈퇴
    void OnClickPartyExitCheck()
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A36_TXT_03006"),
               CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickPartyExit,
               CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티 해산
    void OnClickPartyDisbandCheck()
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A36_TXT_03005"),
               CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), OnClickPartyDisband,
               CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyExit()
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            CsCommandEventManager.Instance.SendPartyExit();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyDisband()
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            CsCommandEventManager.Instance.SendPartyDisband();
        }
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        //파티 생성
        m_buttonPartyCreate = transform.Find("ButtonPartyCreate").GetComponent<Button>();
        m_buttonPartyCreate.onClick.RemoveAllListeners();
        m_buttonPartyCreate.onClick.AddListener(OnClickCreateParty);
        m_buttonPartyCreate.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textPartyCreate = m_buttonPartyCreate.transform.Find("Text").GetComponent<Text>();
        textPartyCreate.text = CsConfiguration.Instance.GetString("A36_BTN_00001");
        CsUIData.Instance.SetFont(textPartyCreate);

        //파티 해산
        m_buttonPartyDisband = transform.Find("ButtonPartyDisband").GetComponent<Button>();
        m_buttonPartyDisband.onClick.RemoveAllListeners();
        m_buttonPartyDisband.onClick.AddListener(OnClickPartyDisbandCheck);
        m_buttonPartyDisband.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textPartyDisband = m_buttonPartyDisband.transform.Find("Text").GetComponent<Text>();
        textPartyDisband.text = CsConfiguration.Instance.GetString("A36_BTN_00002");
        CsUIData.Instance.SetFont(textPartyDisband);

        //파티 탈퇴
        m_buttonPartyExit = transform.Find("ButtonPartyExit").GetComponent<Button>();
        m_buttonPartyExit.onClick.RemoveAllListeners();
        m_buttonPartyExit.onClick.AddListener(OnClickPartyExitCheck);
        m_buttonPartyExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textPartyExit = m_buttonPartyExit.transform.Find("Text").GetComponent<Text>();
        textPartyExit.text = CsConfiguration.Instance.GetString("A36_BTN_00003");
        CsUIData.Instance.SetFont(textPartyExit);

        Toggle toggleInvitationAccept = transform.Find("ToggleList/ToggleInvitationAccept").GetComponent<Toggle>();
        Text textInvitationAccept = toggleInvitationAccept.transform.Find("TextName").GetComponent<Text>();
        toggleInvitationAccept.onValueChanged.RemoveAllListeners();

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept))
        {
            int nAutoAccept = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept);
            if (nAutoAccept == 1)
            {
                toggleInvitationAccept.isOn = true;
                textInvitationAccept.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                toggleInvitationAccept.isOn = false;
                textInvitationAccept.color = CsUIData.Instance.ColorGray;
            }
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept, 0);
        }

        toggleInvitationAccept.onValueChanged.AddListener((ison) => OnValueChangedInvitationAutoAccept(ison, textInvitationAccept));
        toggleInvitationAccept.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        textInvitationAccept.text = CsConfiguration.Instance.GetString("A36_TXT_00001");
        CsUIData.Instance.SetFont(textInvitationAccept);

        Toggle toggleCallAccept = transform.Find("ToggleList/ToggleCallAccept").GetComponent<Toggle>();
        Text textCallAccept = toggleCallAccept.transform.Find("TextName").GetComponent<Text>();
        toggleCallAccept.onValueChanged.RemoveAllListeners();

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept))
        {
            int nAutoCall = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept);
            if (nAutoCall == 1)
            {
                toggleCallAccept.isOn = true;
                textCallAccept.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                toggleCallAccept.isOn = false;
                textCallAccept.color = CsUIData.Instance.ColorGray;
            }
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept, 0);
        }

        toggleCallAccept.onValueChanged.AddListener((ison) => OnValueChangedCallAutoAccept(ison, textCallAccept));
        toggleCallAccept.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        textCallAccept.text = CsConfiguration.Instance.GetString("A36_TXT_00002");
        CsUIData.Instance.SetFont(textCallAccept);

        PartyCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void PartyCheck()
    {
        m_buttonPartyCreate.gameObject.SetActive(false);
        m_buttonPartyDisband.gameObject.SetActive(false);
        m_buttonPartyExit.gameObject.SetActive(false);

        if (CsUIData.Instance.DungeonInNow == EnDungeon.AncientRelic
            || CsUIData.Instance.DungeonInNow == EnDungeon.SoulCoveter)
        {
            transform.Find("ToggleList/ToggleInvitationAccept").gameObject.SetActive(false);
            return;
        }

        if (CsGameData.Instance.MyHeroInfo.Party == null)
        {
            m_buttonPartyCreate.gameObject.SetActive(true);
        }
        else
        {
            m_buttonPartyExit.gameObject.SetActive(true);

            if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                m_buttonPartyDisband.gameObject.SetActive(true);
            }

        }
    }
}
