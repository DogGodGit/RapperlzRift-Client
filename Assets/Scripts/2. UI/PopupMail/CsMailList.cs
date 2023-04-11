using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-08)
//---------------------------------------------------------------------------------------------------

public class CsMailList : CsPopupSub
{
    [SerializeField] GameObject m_goToggleMail;

    Transform m_trContentMailList;
    Transform m_trNoMailText;

    ToggleGroup m_toggleGroup;

    float m_flTime = 0;
    int m_nSelectMailIndex = -1;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventNewMail += OnEventNewMail;
        CsGameEventUIToUI.Instance.EventMailReceive += OnEventMailReceive;
        CsGameEventUIToUI.Instance.EventMailReceiveAll += OnEventMailReceiveAll;
        CsGameEventUIToUI.Instance.EventMailDelete += OnEventMailDelete;
        CsGameEventUIToUI.Instance.EventMailDeleteAll += OnEventMailDeleteAll;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + 1.0f < Time.time)
        {
            UpdateRemaingTime();
            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventNewMail -= OnEventNewMail;
        CsGameEventUIToUI.Instance.EventMailReceive -= OnEventMailReceive;
        CsGameEventUIToUI.Instance.EventMailReceiveAll -= OnEventMailReceiveAll;
        CsGameEventUIToUI.Instance.EventMailDelete -= OnEventMailDelete;
        CsGameEventUIToUI.Instance.EventMailDeleteAll -= OnEventMailDeleteAll;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnValueChangeMail(int nIndex)
    {
        Toggle toggleMail = m_trContentMailList.Find("ToggleMail" + nIndex).GetComponent<Toggle>();

        if (toggleMail.isOn)
        {
            CsMail csMail = CsGameData.Instance.MyHeroInfo.MailList[nIndex];
            CsGameEventUIToUI.Instance.OnEventMailSelected(csMail.Id);
            m_nSelectMailIndex = nIndex;
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMailReceiveAll()
    {
        int nCount = 0;
        bool bReceive = true;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.MailList.Count; i++)
        {
            CsMail csMail = CsGameData.Instance.MyHeroInfo.MailList[i];
            
            for (int j = 0; j < csMail.MailAttachmentList.Count; j++)
            {
                if (csMail.MailAttachmentList[j].Count <= CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(csMail.MailAttachmentList[j].Item.ItemId, csMail.MailAttachmentList[j].Owned))
                {
                    continue;
                }
                else
                {
                    if ((CsGameData.Instance.MyHeroInfo.InventorySlotList.Count + nCount) < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        nCount++;
                    }
                    else
                    {
                        bReceive = false;
                        break;
                    }
                }
            }

            if (bReceive)
            {
                continue;
            }
            else
            {
                break;
            }
        }

        if (bReceive)
        {
            CsCommandEventManager.Instance.SendMailReceiveAll();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A03_TXT_02002"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMailDeleteAll()
    {
        CsCommandEventManager.Instance.SendMailDeleteAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNewMail(Guid guidMail)
    {
        GameObject goScrollView = transform.Find("Scroll View").gameObject;

        if (!goScrollView.activeSelf)
        {
            goScrollView.SetActive(true);
        }

        Button buttonMailReceiveAll = transform.Find("ButtonReceiveAll").GetComponent<Button>();

        if (!buttonMailReceiveAll.interactable)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonMailReceiveAll, true);
        }

        UpdateMailList();

        m_trContentMailList.Find("ToggleMail" + (++m_nSelectMailIndex)).GetComponent<Toggle>().isOn = true;

        if (CsGameData.Instance.MyHeroInfo.MailList.Count == 1)
        {
            CsGameEventUIToUI.Instance.OnEventMailSelected(guidMail);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceive(Guid guidMail)
    {
        UpdateMailList();

        if (CsGameData.Instance.MyHeroInfo.MailList.Count > 0)
        {
            //SetToggleZero();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventMailSelected(Guid.Empty);
            m_nSelectMailIndex = -1;
        }

        UpdateButtonMailReceiveAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceiveAll(Guid[] aguidMails)
    {
        UpdateMailList();
        UpdateButtonMailReceiveAll();
        //SetToggleZero();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailDelete(Guid guidMail)
    {
        UpdateMailList();

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A03_TXT_02005"));

        if (CsGameData.Instance.MyHeroInfo.MailList.Count > 0)
        {
            SetToggleZero();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventMailSelected(Guid.Empty);
            m_nSelectMailIndex = -1;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailDeleteAll()
    {
        UpdateMailList();

        CsGameEventUIToUI.Instance.OnEventMailSelected(Guid.Empty);
        m_nSelectMailIndex = -1;

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A03_TXT_02004"));
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContentMailList = transform.Find("Scroll View/Viewport/Content");
        m_toggleGroup = m_trContentMailList.GetComponent<ToggleGroup>();
        m_trNoMailText = transform.Find("TextNoMail");

        Button buttonMailReceiveAll = transform.Find("ButtonReceiveAll").GetComponent<Button>();
        buttonMailReceiveAll.onClick.RemoveAllListeners();
        buttonMailReceiveAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonMailReceiveAll.onClick.AddListener(OnClickMailReceiveAll);

        Button buttonMailDeleteAll = transform.Find("ButtonDeleteAll").GetComponent<Button>();
        buttonMailDeleteAll.onClick.RemoveAllListeners();
        buttonMailDeleteAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonMailDeleteAll.onClick.AddListener(OnClickMailDeleteAll);

        Text textButtonMailReceiveAll = buttonMailReceiveAll.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonMailReceiveAll);
        textButtonMailReceiveAll.text = CsConfiguration.Instance.GetString("A03_BTN_00001");

        Text textButtonMailDeleteAll = buttonMailDeleteAll.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonMailDeleteAll);
        textButtonMailDeleteAll.text = CsConfiguration.Instance.GetString("A03_BTN_00003");
        
        Text textNoMail = m_trNoMailText.GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoMail);
        textNoMail.text = CsConfiguration.Instance.GetString("A03_TXT_00003");

        if (CsGameData.Instance.MyHeroInfo.MailList.Count == 0)
        {
            m_trNoMailText.gameObject.SetActive(true);
            CsUIData.Instance.DisplayButtonInteractable(buttonMailReceiveAll, false);
            return;
        }

        m_trNoMailText.gameObject.SetActive(false);
        UpdateMailList();
        UpdateButtonMailReceiveAll();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMailList()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.MailList.Count; i++)
        {
            CrearteMail(i);
        }

        for (int i = CsGameData.Instance.MyHeroInfo.MailList.Count; i < m_trContentMailList.childCount; i++)
        {
            m_trContentMailList.Find("ToggleMail" + i).gameObject.SetActive(false);
            Toggle toggleMail = m_trContentMailList.Find("ToggleMail" + i).GetComponent<Toggle>();

            if (toggleMail.isOn)
            {
                int nIndex = i;
                toggleMail.onValueChanged.RemoveAllListeners();
                toggleMail.isOn = false;
                toggleMail.onValueChanged.AddListener((ison) => OnValueChangeMail(nIndex));
            }
        }

        if (CsGameData.Instance.MyHeroInfo.MailList.Count == 0)
        {
            // Off
            m_trNoMailText = transform.Find("TextNoMail");
            m_trNoMailText.gameObject.SetActive(true);

            transform.Find("Scroll View").gameObject.SetActive(false);

            Button buttonMailReceiveAll = transform.Find("ButtonReceiveAll").GetComponent<Button>();
            CsUIData.Instance.DisplayButtonInteractable(buttonMailReceiveAll, false);
        }
        else
        {
            m_trNoMailText.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CrearteMail(int nIndex)
    {
        Transform trMail = m_trContentMailList.Find("ToggleMail" + nIndex);

        if (trMail == null)
        {
            // 생성
            trMail = Instantiate(m_goToggleMail, m_trContentMailList).transform;
            trMail.name = "ToggleMail" + nIndex;

            Toggle toggleMail = trMail.GetComponent<Toggle>();
            toggleMail.group = m_toggleGroup;
            toggleMail.onValueChanged.RemoveAllListeners();

            if (nIndex == 0)
            {
                toggleMail.isOn = true;
            }

            toggleMail.onValueChanged.AddListener((ison) => OnValueChangeMail(nIndex));
        }
        else
        {
            // 업데이트
            if (!trMail.gameObject.activeInHierarchy)
            {
                trMail.gameObject.SetActive(true);
            }
        }

        CsMail csMail = CsGameData.Instance.MyHeroInfo.MailList[nIndex];
        UpdateMailSlot(trMail, csMail);
        UpdateToggleRemaingTime(nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMailSlot(Transform trToggleMail, CsMail csMail)
    {
        // Title
        Text textName = trToggleMail.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csMail.Title;

        if (180 < textName.preferredWidth)
        {
            for (int i = 1; i <= csMail.Title.Length; i++)
            {
                string strMailTitle = csMail.Title.Substring(0, csMail.Title.Length - i);
                textName.text = strMailTitle;

                if (textName.preferredWidth <= 162)
                {
                    textName.text = textName.text + "...";
                    break;
                }
            }
        }

        // Desc
        Text textContent = trToggleMail.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContent);
        textContent.text = csMail.Content;

        if (180 < textContent.preferredWidth)
        {
            for (int i = 1; i <= csMail.Content.Length; i++)
            {
                string strMailContent = csMail.Content.Substring(0, csMail.Content.Length - i);
                textContent.text = strMailContent;

                if (textContent.preferredWidth <= 165)
                {
                    textContent.text = textContent.text + "...";
                    break;
                }
            }
        }


        // RemainingTime
        Text textRemaingTime = trToggleMail.Find("TextTimeValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRemaingTime);

        // ImageAttach
        Transform trImageAttach = trToggleMail.Find("ImageAttach");

        if (csMail.Received)
        {
            trImageAttach.gameObject.SetActive(false);
        }
        else
        {
            if (csMail.MailAttachmentList.Count > 0)
            {
                trImageAttach.gameObject.SetActive(true);
            }
            else
            {
                trImageAttach.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRemaingTime()
    {
        int nDelCount = 0;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.MailList.Count; i++)
        {
            UpdateToggleRemaingTime(i);

            if (CsGameData.Instance.MyHeroInfo.MailList[i].RemaingTime <= 0)
            {
                nDelCount++;
            }
        }

        if (nDelCount > 0)
        {
            CsGameData.Instance.MyHeroInfo.MailList.RemoveAll(a => a.RemaingTime <= 0);
            UpdateMailList();

            if (CsGameData.Instance.MyHeroInfo.MailList.Count > 0)
            {
                SetToggleZero();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventMailSelected(Guid.Empty);
                m_nSelectMailIndex = -1;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleRemaingTime(int nIndex)
    {
        Transform trMail = m_trContentMailList.Find("ToggleMail" + nIndex);
        CsMail csMail = CsGameData.Instance.MyHeroInfo.MailList[nIndex];

        Transform trTextTimeValue = trMail.Find("TextTimeValue");
        Text textRemaingTime = trTextTimeValue.GetComponent<Text>();

        string strRemaingTime = null;
        TimeSpan timeSpan = TimeSpan.FromSeconds(csMail.RemaingTime);

        if (csMail.RemaingTime >= 86400)
        {
            strRemaingTime = string.Format(CsConfiguration.Instance.GetString("A03_TXT_00004"), timeSpan.Days, timeSpan.Hours);
            textRemaingTime.color = CsUIData.Instance.ColorGray;
        }
        else if (csMail.RemaingTime < 86400 && csMail.RemaingTime >= 3600)
        {
            strRemaingTime = string.Format(CsConfiguration.Instance.GetString("A03_TXT_00005"), timeSpan.Hours, timeSpan.Minutes);
            textRemaingTime.color = CsUIData.Instance.ColorGray;
        }
        else if (csMail.RemaingTime < 3600 && csMail.RemaingTime >= 60)
        {
            strRemaingTime = string.Format(CsConfiguration.Instance.GetString("A03_TXT_00006"), timeSpan.Minutes, timeSpan.Seconds);
        }
        else
        {
            strRemaingTime = string.Format(CsConfiguration.Instance.GetString("A03_TXT_00007"), timeSpan.Seconds);
        }

        textRemaingTime.text = strRemaingTime;
    }

    //---------------------------------------------------------------------------------------------------
    void SetToggleZero()
    {
        Toggle toggle = m_trContentMailList.Find("ToggleMail" + 0).GetComponent<Toggle>();

        if (toggle.isOn)
        {
            CsGameEventUIToUI.Instance.OnEventMailSelected(CsGameData.Instance.MyHeroInfo.MailList[0].Id);
        }
        else
        {
            toggle.isOn = true;
        }

        m_trContentMailList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonMailReceiveAll()
    {
        bool bActive = false;
        Button buttonMailReceiveAll = transform.Find("ButtonReceiveAll").GetComponent<Button>();
        
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.MailList.Count; i++)
        {
            if (0 < CsGameData.Instance.MyHeroInfo.MailList[i].MailAttachmentList.Count && !CsGameData.Instance.MyHeroInfo.MailList[i].Received)
            {
                bActive = true;
                break;
            }
        }
        Debug.Log("UpdateButtonMailReceiveAll : bActive : " + bActive);
        CsUIData.Instance.DisplayButtonInteractable(buttonMailReceiveAll, bActive);
    }
}