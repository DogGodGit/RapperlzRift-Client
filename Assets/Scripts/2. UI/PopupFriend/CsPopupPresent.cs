using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupPresent : MonoBehaviour 
{
    GameObject m_goTogglePresentItemSlot;

    Transform m_trItemSlot;
    Transform m_trImageItemList;

    Text m_textDia;
    Text m_textItemName;
    Text m_textItemDesctiption;
    Text m_textPresentPopularity;
    Text m_textRequiredDia;

    Button m_buttonPresent;

    Guid m_guidTargetHeroId = Guid.Empty;
    CsPresent m_CsPresent = null;
 
    //---------------------------------------------------------------------------------------------------
	void Awake () 
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventOpenPopupPresent += OnEventOpenPopupPresent;
        CsPresentManager.Instance.EventPresentSend += OnEventPresentSend;
	}

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventOpenPopupPresent -= OnEventOpenPopupPresent;
        CsPresentManager.Instance.EventPresentSend -= OnEventPresentSend;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupPresent(Guid guidHeroId)
    {
        if (guidHeroId == Guid.Empty)
        {
            return;
        }

        m_guidTargetHeroId = guidHeroId;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPresentSend(int nPresentId)
    {
        UpdateUnOwnDiaCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedPresent(bool bIson, CsPresent csPresent)
    {
        if (bIson)
        {
            if (csPresent == null)
            {
                return;
            }

            UpdatePopupPresent(csPresent);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPresent()
    {
        if (m_CsPresent == null)
        {
            return;
        }

        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < m_CsPresent.RequiredVipLevel)
        {
            // Vip 레벨 부족
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A108_TXT_03001"), m_CsPresent.RequiredVipLevel));
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Dia < m_CsPresent.RequiredDia)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
            }
            else
            {
                string strMessage = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01037"), m_CsPresent.RequiredDia, m_CsPresent.DisplayCount, m_CsPresent.Name);
                CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsPresentManager.Instance.SendPresentSend(m_guidTargetHeroId, m_CsPresent.PresentId),
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        transform.gameObject.SetActive(false);

        Transform trImageBackground = transform.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A108_NAME_00005");

        m_textDia = trImageBackground.Find("ImageGoods/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDia);
        m_textDia.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClose);

        m_trItemSlot = trImageBackground.Find("ItemSlot");

        m_textItemName = trImageBackground.Find("TextItemName").GetComponent<Text>();
        m_textItemDesctiption = trImageBackground.Find("TextItemDescription").GetComponent<Text>();
        m_textPresentPopularity = trImageBackground.Find("TextPresentPopularity").GetComponent<Text>();

        CsUIData.Instance.SetFont(m_textItemName);
        CsUIData.Instance.SetFont(m_textItemDesctiption);
        CsUIData.Instance.SetFont(m_textPresentPopularity);

        m_buttonPresent = trImageBackground.Find("ButtonPresent").GetComponent<Button>();
        m_buttonPresent.onClick.RemoveAllListeners();
        m_buttonPresent.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonPresent.onClick.AddListener(OnClickPresent);

        Text textPresent = m_buttonPresent.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPresent);
        textPresent.text = CsConfiguration.Instance.GetString("A108_BTN_01001");

        m_textRequiredDia = m_buttonPresent.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRequiredDia);

        transform.gameObject.SetActive(true);

        m_trImageItemList = trImageBackground.Find("ImageItemList");
        StartCoroutine(LoadTogglePresentItemSlot());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadTogglePresentItemSlot()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/TogglePresentItemSlot");
        yield return resourceRequest;

        m_goTogglePresentItemSlot = (GameObject)resourceRequest.asset;
        InitializeImageItemList();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeImageItemList()
    {
        Transform trTogglePresentItemSlot = null;

        for (int i = 0; i < CsGameData.Instance.PresentList.Count; i++)
        {
            CsPresent csPresent = CsGameData.Instance.PresentList[i];

            if (csPresent == null)
            {
                continue;
            }
            else
            {
                trTogglePresentItemSlot = m_trImageItemList.Find("TogglePresentItemSlot" + i);

                if (trTogglePresentItemSlot == null)
                {
                    trTogglePresentItemSlot = Instantiate(m_goTogglePresentItemSlot, m_trImageItemList).transform;
                    trTogglePresentItemSlot.name = "TogglePresentItemSlot" + i;
                }
                else
                {
                    trTogglePresentItemSlot.gameObject.SetActive(true);
                }

                Toggle togglePresentItemSlot = trTogglePresentItemSlot.GetComponent<Toggle>();

                if (i == 0)
                {
                    UpdatePopupPresent(csPresent);
                    togglePresentItemSlot.isOn = true;
                }
                else
                {
                    togglePresentItemSlot.isOn = false;
                }

                togglePresentItemSlot.onValueChanged.RemoveAllListeners();
                togglePresentItemSlot.onValueChanged.AddListener((ison) => OnValueChangedPresent(ison, csPresent));
                togglePresentItemSlot.group = m_trImageItemList.GetComponent<ToggleGroup>();

                Image imageIcon = trTogglePresentItemSlot.Find("ImageIcon").GetComponent<Image>();
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupFriend/ico_present_item_" + csPresent.PresentId);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateUnOwnDiaCount()
    {
        m_textDia.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupPresent(CsPresent csPresent)
    {
        m_CsPresent = csPresent;

        Image imageIcon = m_trItemSlot.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupFriend/ico_present_item_" + csPresent.PresentId);
        
        m_textItemName.text = csPresent.Name;
        m_textItemDesctiption.text = csPresent.Description;
        m_textPresentPopularity.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01012"), csPresent.PopularityPoint);
        m_textRequiredDia.text = csPresent.RequiredDia.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopup()
    {
        Destroy(gameObject);
    }
}
