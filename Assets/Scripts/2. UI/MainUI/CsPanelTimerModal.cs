using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum EnTimerModalType
{
    GuildModal = 0,
    PartyModal = 1,
    NationModal = 2, 
    FriendModal = 3, 
}

public class CsPanelTimerModal : MonoBehaviour
{
    [SerializeField] GameObject m_goPopupModal;

    Text m_textGuildMessage;
    Text m_textPartyMessage;
    Text m_textNationMessage;
    Text m_textFriendMessage;

    float m_flGuildRemainingTime;
    float m_flPartyRemainingTime;
    float m_flNationRemainingTime;

    float m_flTime = 0;

    string m_strGuildTime;
    string m_strPartyTime;
    string m_strNationTime;

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1f < Time.time)
        {
            if (m_flGuildRemainingTime - Time.realtimeSinceStartup > 0)
            {
                UpdateGuildMessage();
            }

            if (m_flPartyRemainingTime - Time.realtimeSinceStartup > 0)
            {
                UpdatePartyMessage();
            }

            if ((m_flNationRemainingTime - Time.realtimeSinceStartup) > 0)
            {
                UpdateNationMessage();
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void Choice(EnTimerModalType enTimerModalType, string strMessage, UnityAction unityAction1, UnityAction unityAction2, float flTime = 0)
    {
        Transform trModal = transform.Find(enTimerModalType.ToString());

        if (trModal == null)
        {
            trModal = Instantiate(m_goPopupModal, transform).transform;
            trModal.name = enTimerModalType.ToString();
        }

        switch (enTimerModalType)
        {
            case EnTimerModalType.GuildModal:
                m_textGuildMessage = trModal.Find("TextMessage").GetComponent<Text>();
                CsUIData.Instance.SetFont(m_textGuildMessage);
                m_flGuildRemainingTime = flTime;

                if (m_flGuildRemainingTime != 0)
                {
                    m_strGuildTime = strMessage + CsConfiguration.Instance.GetString("A58_TXT_01013");
                    m_textGuildMessage.text = string.Format(m_strGuildTime, (int)(m_flGuildRemainingTime - Time.realtimeSinceStartup));
                }
                else
                {
                    m_textGuildMessage.text = strMessage;
                }
                break;

            case EnTimerModalType.PartyModal:
                m_textPartyMessage = trModal.Find("TextMessage").GetComponent<Text>();
                CsUIData.Instance.SetFont(m_textPartyMessage);
                m_flPartyRemainingTime = flTime;

                if (m_flPartyRemainingTime != 0)
                {
                    m_strPartyTime = strMessage + CsConfiguration.Instance.GetString("A58_TXT_01013");
                    m_textPartyMessage.text = string.Format(m_strPartyTime, (int)(m_flPartyRemainingTime - Time.realtimeSinceStartup));
                }
                else
                {
                    m_textPartyMessage.text = strMessage;
                }
                break;

            case EnTimerModalType.NationModal:
                m_textNationMessage = trModal.Find("TextMessage").GetComponent<Text>();
                CsUIData.Instance.SetFont(m_textNationMessage);
                m_flNationRemainingTime = flTime;

                if (m_flNationRemainingTime != 0)
                {
                    m_strNationTime = strMessage;
                    m_textNationMessage.text = string.Format(m_strNationTime, (int)(m_flNationRemainingTime - Time.realtimeSinceStartup));
                }
                else
                {
                    m_textNationMessage.text = strMessage;
                }
                break;

            case EnTimerModalType.FriendModal:

                m_textFriendMessage = trModal.Find("TextMessage").GetComponent<Text>();
                CsUIData.Instance.SetFont(m_textFriendMessage);
                m_textFriendMessage.text = strMessage;

                break;
        }

        Transform trButtons = trModal.Find("Buttons");

        Button button1 = trButtons.Find("Button1").GetComponent<Button>();
        Button button2 = trButtons.Find("Button2").GetComponent<Button>();

        Text textButton1 = button1.transform.Find("Text").GetComponent<Text>();
        textButton1.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
        CsUIData.Instance.SetFont(textButton1);

        Text textButton2 = button2.transform.Find("Text").GetComponent<Text>();
        textButton2.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");
        CsUIData.Instance.SetFont(textButton2);

        button1.transform.gameObject.SetActive(true);
        button2.transform.gameObject.SetActive(true);

        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        button2.onClick.RemoveAllListeners();
        button2.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        if (unityAction1 != null)
        {
            button1.onClick.AddListener(unityAction1);
        }

        if (unityAction2 != null)
        {
            button2.onClick.AddListener(unityAction2);
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        trModal.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildMessage()
    {
        if (m_textGuildMessage != null)
            m_textGuildMessage.text = string.Format(m_strGuildTime, (int)(m_flGuildRemainingTime - Time.realtimeSinceStartup));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePartyMessage()
    {
        if (m_textPartyMessage != null)
            m_textPartyMessage.text = string.Format(m_strPartyTime, (int)(m_flPartyRemainingTime - Time.realtimeSinceStartup));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationMessage()
    {
        if (m_textNationMessage != null)
            m_textNationMessage.text = string.Format(m_strNationTime, (int)(m_flNationRemainingTime - Time.realtimeSinceStartup));
    }

    //---------------------------------------------------------------------------------------------------
    public void CloseTimerModal(EnTimerModalType enTimerModalType)
    {
        Transform trModal = transform.Find(enTimerModalType.ToString());

        if (trModal != null)
        {
            DestroyImmediate(trModal.gameObject);
        }

        gameObject.SetActive(transform.childCount != 0);
    }
}