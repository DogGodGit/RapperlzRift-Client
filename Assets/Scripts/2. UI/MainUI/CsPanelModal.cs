using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CsPanelModal : MonoBehaviour
{
    Transform m_trCommonModal;

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    public void InitializeUI()
    {
        m_trCommonModal = transform.Find("CommonModal");

        Text textMessage = m_trCommonModal.Find("TextMessage").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMessage);

        Text textButton1 = m_trCommonModal.transform.Find("Buttons/Button1/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton1);

        Text textButton2 = m_trCommonModal.transform.Find("Buttons/Button2/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton2);

        m_trCommonModal.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    public void Choice(string strMessage, string strButton1)
    {
        Text textMessage = m_trCommonModal.Find("TextMessage").GetComponent<Text>();
        Transform trButtons = m_trCommonModal.Find("Buttons");
        Button button1 = trButtons.Find("Button1").GetComponent<Button>();
        Button button2 = trButtons.Find("Button2").GetComponent<Button>();

        textMessage.text = strMessage;
        button2.transform.gameObject.SetActive(false);
        button1.transform.gameObject.SetActive(true);

        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(CloseCommonModal);
        button1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButton1 = button1.transform.Find("Text").GetComponent<Text>();
        textButton1.text = strButton1;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        m_trCommonModal.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    public void Choice(string strMessage, UnityAction unityAction1, string strButton1)
    {
        Text textMessage = m_trCommonModal.Find("TextMessage").GetComponent<Text>();
        Transform trButtons = m_trCommonModal.Find("Buttons");
        Button button1 = trButtons.Find("Button1").GetComponent<Button>();
        Button button2 = trButtons.Find("Button2").GetComponent<Button>();
        Text textButton1 = button1.transform.Find("Text").GetComponent<Text>();
        //Text textButton2 = button2.transform.Find("Text").GetComponent<Text>();

        textMessage.text = strMessage;
        button1.transform.gameObject.SetActive(true);
        button2.transform.gameObject.SetActive(false);
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        if (unityAction1 != null)
        {
            button1.onClick.AddListener(unityAction1);
        }

        button1.onClick.AddListener(CloseCommonModal);
        textButton1.text = strButton1;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        m_trCommonModal.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    public void Choice(string strMessage, UnityAction unityAction1, string strButton1, UnityAction unityAction2, string strButton2, bool bAutoClose = false)
    {
        Debug.Log("#@#@ Choice");
        Text textMessage = m_trCommonModal.Find("TextMessage").GetComponent<Text>();
        Transform trButtons = m_trCommonModal.Find("Buttons");
        Button button1 = trButtons.Find("Button1").GetComponent<Button>();
        Button button2 = trButtons.Find("Button2").GetComponent<Button>();
        Text textButton1 = button1.transform.Find("Text").GetComponent<Text>();
        Text textButton2 = button2.transform.Find("Text").GetComponent<Text>();

        textMessage.text = strMessage;

        button1.transform.gameObject.SetActive(true);
        button2.transform.gameObject.SetActive(true);

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        button2.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        textButton1.text = strButton1;
        textButton2.text = strButton2;

        if (bAutoClose)
        {
            button1.onClick.AddListener(CloseCommonModal);
            button2.onClick.AddListener(CloseCommonModal);
        }

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

        m_trCommonModal.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    public void CloseCommonModal()
    {
        m_trCommonModal.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
