using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-03)
//---------------------------------------------------------------------------------------------------

public class CsPartySurroundingHero : CsPopupSub
{

    [SerializeField] GameObject m_goSurroundingHero;

    Transform m_trContent;
    Text m_textNoHero;

    bool m_bFirst = true;

    CsSimpleHero[] m_simpleHeroes;

    Guid m_guidCreateInvite = Guid.Empty;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventPartySurroundingHeroList += OnEventPartySurroundingHeroList;
        CsGameEventUIToUI.Instance.EventPartySurroundingHeroListRequest += OnEventPartySurroundingHeroListRequest;
        CsGameEventUIToUI.Instance.EventPartyDisbanded += OnEventPartyDisbanded;
        CsGameEventUIToUI.Instance.EventPartyDisband += OnEventPartyDisband;
        CsGameEventUIToUI.Instance.EventPartyExit += OnEventPartyExit;
        CsGameEventUIToUI.Instance.EventPartyBanished += OnEventPartyBanished;
        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyInvite += OnEventPartyInvite;

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
        CsGameEventUIToUI.Instance.EventPartySurroundingHeroList -= OnEventPartySurroundingHeroList;
        CsGameEventUIToUI.Instance.EventPartySurroundingHeroListRequest -= OnEventPartySurroundingHeroListRequest;
        CsGameEventUIToUI.Instance.EventPartyDisbanded -= OnEventPartyDisbanded;
        CsGameEventUIToUI.Instance.EventPartyDisband -= OnEventPartyDisband;
        CsGameEventUIToUI.Instance.EventPartyExit -= OnEventPartyExit;
        CsGameEventUIToUI.Instance.EventPartyBanished -= OnEventPartyBanished;
        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyInvite -= OnEventPartyInvite;

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
            if (m_simpleHeroes != null)
            {
                DisplayOffSurroundingHero();
            }

            CsCommandEventManager.Instance.SendPartySurroundingHeroList();
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyInvite(Guid guid)
    {
        m_guidCreateInvite = guid;

        //파티 초대를 했는데 파티가 없는 상태라면 파티를 만든다.
        if (CsGameData.Instance.MyHeroInfo.Party == null)
        {
            CsCommandEventManager.Instance.SendPartyCreate();
        }
        //파티가 가득찬 상황
        else if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count >= CsGameConfig.Instance.PartyMemberMaxCount)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04006"));
            m_guidCreateInvite = Guid.Empty;
        }
        else
        {
            //중복 초대
            List<CsPartyInvitation> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyInvitationList;
            for (int i = 0; i < listPartyApplication.Count; ++i)
            {
                if (listPartyApplication[i].TargetId == m_guidCreateInvite)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04007"));
                    return;
                }
            }
            if (m_guidCreateInvite != Guid.Empty)
            {
                CsCommandEventManager.Instance.SendPartyInvite(m_guidCreateInvite);
                m_guidCreateInvite = Guid.Empty;
            }
        }
    }

    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsCommandEventManager.Instance.SendPartySurroundingHeroList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartySurroundingHeroList(CsSimpleHero[] simpleHeroes)
    {
        m_simpleHeroes = simpleHeroes;
        DisplaySurroundingHero();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        if (m_guidCreateInvite != Guid.Empty)
        {
            CsCommandEventManager.Instance.SendPartyInvite(m_guidCreateInvite);
            m_guidCreateInvite = Guid.Empty;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvite(CsPartyInvitation csPartyInvitation)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04019"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartySurroundingHeroListRequest()
    {
        CsCommandEventManager.Instance.SendPartySurroundingHeroList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisbanded()
    {
        CsCommandEventManager.Instance.SendPartySurroundingHeroList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisband()
    {
        CsCommandEventManager.Instance.SendPartySurroundingHeroList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyExit()
    {
        CsCommandEventManager.Instance.SendPartySurroundingHeroList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyBanished()
    {
        CsCommandEventManager.Instance.SendPartySurroundingHeroList();
    }
    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");
        m_textNoHero = transform.Find("TextNoHero").GetComponent<Text>();
        m_textNoHero.text = CsConfiguration.Instance.GetString("A36_TXT_00007");
        CsUIData.Instance.SetFont(m_textNoHero);

        CsCommandEventManager.Instance.SendPartySurroundingHeroList();

    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySurroundingHero()
    {
        if (m_simpleHeroes != null)
        {
            if (m_simpleHeroes.Length > 0)
            {
                for (int i = 0; i < m_simpleHeroes.Length; ++i)
                {
                    CreateHero(m_simpleHeroes[i]);
                }

                m_textNoHero.gameObject.SetActive(false);
            }
            else
            {
                m_textNoHero.gameObject.SetActive(true);
            }

        }
        else
        {
            m_textNoHero.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayOffSurroundingHero()
    {
        if (m_simpleHeroes != null)
        {
            for (int i = 0; i < m_simpleHeroes.Length; ++i)
            {
                DestoryHero(m_simpleHeroes[i]);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateHero(CsSimpleHero csSimpleHero)
    {
        if (csSimpleHero == null)
        {
            return;
        }

        Transform trHero;

        if (csSimpleHero.HeroId != Guid.Empty)
        {
            trHero = m_trContent.Find(csSimpleHero.HeroId.ToString());
            if (trHero == null)
            {
                trHero = Instantiate(m_goSurroundingHero, m_trContent).transform;
                trHero.name = csSimpleHero.HeroId.ToString();
            }
            trHero.gameObject.SetActive(true);
        }
        else
        {
            return;
        }

        Image imageEmblem = trHero.Find("ImageEmblem").GetComponent<Image>();
        imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csSimpleHero.Job.JobId);

        Text textName = trHero.Find("TextName").GetComponent<Text>();
        textName.text = string.Format(CsConfiguration.Instance.GetString("A36_TXT_01001"), csSimpleHero.Level, csSimpleHero.Name, csSimpleHero.BattlePower);
        CsUIData.Instance.SetFont(textName);

        Button buttonInvite = trHero.Find("ButtonInvite").GetComponent<Button>();
        buttonInvite.onClick.RemoveAllListeners();
        buttonInvite.onClick.AddListener(() => OnClickPartyInvite(csSimpleHero.HeroId));
        Text textInvite = buttonInvite.transform.Find("Text").GetComponent<Text>();
        textInvite.text = CsConfiguration.Instance.GetString("A36_BTN_00004");
        CsUIData.Instance.SetFont(textInvite);

    }

    //---------------------------------------------------------------------------------------------------
    void DestoryHero(CsSimpleHero csSimpleHero)
    {
        if (csSimpleHero == null)
        {
            return;
        }

        Transform trHero;

        if (csSimpleHero.HeroId != Guid.Empty)
        {
            trHero = m_trContent.Find(csSimpleHero.HeroId.ToString());
            if (trHero == null)
            {
                return;
            }
            else
            {
                trHero.gameObject.SetActive(false);
            }
        }
    }
}
