using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-03)
//---------------------------------------------------------------------------------------------------

public class CsPartySurroundingParty : CsPopupSub
{

    [SerializeField] GameObject m_goSurroundingParty;

    Transform m_trContent;
    Text m_textNoParty;

    bool m_bFirst = true;

    CsSimpleParty[] m_simpleParties;


    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventPartySurroundingPartyList += OnEventPartySurroundingPartyList;
        CsGameEventUIToUI.Instance.EventPartySurroundingPartyListRequest += OnEventPartySurroundingPartyListRequest;
        CsGameEventUIToUI.Instance.EventPartyApply += OnEventPartyApply;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventPartySurroundingPartyList -= OnEventPartySurroundingPartyList;
        CsGameEventUIToUI.Instance.EventPartySurroundingPartyListRequest -= OnEventPartySurroundingPartyListRequest;
        CsGameEventUIToUI.Instance.EventPartyApply -= OnEventPartyApply;
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
            if (m_simpleParties != null)
            {
                DisplayOffSurroundingParty();
            }

            CsCommandEventManager.Instance.SendPartySurroundingPartyList();
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyApply(Guid guid)
    {

        for (int i = 0; i < m_simpleParties.Length; ++i)
        {
            if (m_simpleParties[i].Id == guid)
            {
                if (m_simpleParties[i].MemberCount >= CsGameConfig.Instance.PartyMemberMaxCount)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04006"));
                }
            }
        }

        List<CsPartyApplication> listPartyApplication = CsGameData.Instance.MyHeroInfo.PartyApplicationList;

        for (int i = 0; i < listPartyApplication.Count; ++i)
        {
            if (listPartyApplication[i].PartyId == guid)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04007"));
                return;
            }
        }

        if (guid != Guid.Empty)
        {
            CsCommandEventManager.Instance.SendPartyApply(guid);
        }
    }

    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventPartySurroundingPartyList(CsSimpleParty[] simpleParties)
    {
        m_simpleParties = simpleParties;
        DisplaySurroundingParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApply(CsPartyApplication csPartyApplication)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04019"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartySurroundingPartyListRequest()
    {
        CsCommandEventManager.Instance.SendPartySurroundingPartyList();
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");
        m_textNoParty = transform.Find("TextNoParty").GetComponent<Text>();
        m_textNoParty.text = CsConfiguration.Instance.GetString("A36_TXT_00008");
        CsUIData.Instance.SetFont(m_textNoParty);

        CsCommandEventManager.Instance.SendPartySurroundingPartyList();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySurroundingParty()
    {
        if (m_simpleParties != null)
        {
            if (m_simpleParties.Length > 0)
            {
                for (int i = 0; i < m_simpleParties.Length; ++i)
                {
                    CreateParty(m_simpleParties[i]);
                }

                m_textNoParty.gameObject.SetActive(false);
            }
            else
            {
                m_textNoParty.gameObject.SetActive(true);
            }
        }
        else
        {
            m_textNoParty.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayOffSurroundingParty()
    {
        if (m_simpleParties != null)
        {
            for (int i = 0; i < m_simpleParties.Length; ++i)
            {
                DestoryParty(m_simpleParties[i]);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateParty(CsSimpleParty csSimpleParty)
    {
        if (csSimpleParty == null)
        {
            return;
        }

        Transform trParty;

        if (csSimpleParty.Id != Guid.Empty)
        {
            trParty = m_trContent.Find(csSimpleParty.Id.ToString());
            if (trParty == null)
            {
                trParty = Instantiate(m_goSurroundingParty, m_trContent).transform;
                trParty.name = csSimpleParty.Id.ToString();
            }
            trParty.gameObject.SetActive(true);
        }
        else
        {
            return;
        }

        Text textLeaderInfo = trParty.Find("TextLeaderInfo").GetComponent<Text>();
        textLeaderInfo.text = CsConfiguration.Instance.GetString("A36_TXT_01002");
        CsUIData.Instance.SetFont(textLeaderInfo);

        Text textName = trParty.Find("TextName").GetComponent<Text>();
        textName.text = string.Format(CsConfiguration.Instance.GetString("A36_TXT_01001"), csSimpleParty.Master.Level, csSimpleParty.Master.Name, csSimpleParty.Master.BattlePower);
        CsUIData.Instance.SetFont(textName);

        Text textPartyPersonnel = trParty.Find("TextPartyPersonnel").GetComponent<Text>();
        textPartyPersonnel.text = CsConfiguration.Instance.GetString("A36_TXT_01003");
        CsUIData.Instance.SetFont(textPartyPersonnel);

        Text textPersonnel = trParty.Find("TextPersonnel").GetComponent<Text>();
        textPersonnel.text = string.Format(CsConfiguration.Instance.GetString("A36_TXT_01004"), csSimpleParty.MemberCount, CsGameConfig.Instance.PartyMemberMaxCount);
        CsUIData.Instance.SetFont(textPersonnel);

        Button buttonPartyApply = trParty.Find("ButtonPartyApply").GetComponent<Button>();
        buttonPartyApply.onClick.RemoveAllListeners();
        buttonPartyApply.onClick.AddListener(() => OnClickPartyApply(csSimpleParty.Id));
        Text textApply = buttonPartyApply.transform.Find("Text").GetComponent<Text>();
        textApply.text = CsConfiguration.Instance.GetString("A36_BTN_00005");
        CsUIData.Instance.SetFont(textApply);

    }

    //---------------------------------------------------------------------------------------------------
    void DestoryParty(CsSimpleParty csSimpleParty)
    {
        if (csSimpleParty == null)
        {
            return;
        }

        Transform trParty;

        if (csSimpleParty.Id != Guid.Empty)
        {
            trParty = m_trContent.Find(csSimpleParty.Id.ToString());
            if (trParty == null)
            {
                return;
            }
            else
            {
                trParty.gameObject.SetActive(false);
            }
        }
    }
}
