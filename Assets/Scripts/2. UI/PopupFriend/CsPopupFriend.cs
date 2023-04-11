using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-08-31)
//---------------------------------------------------------------------------------------------------

public class CsPopupFriend : CsPopupSub
{
    GameObject m_goToggleFriendItem;

    Transform m_trContent;
    Transform m_trImageBackground;
    Transform m_trPopupSearchFriend;
    Transform m_trNoFriend;

    float m_flTime = 0f;

    bool m_bPopupSearchOpen = true;
    bool m_bDelete = false;

    CsFriend m_csFriend = null;

    Guid m_guidSelectSearchFriendId = Guid.Empty;
    List<Guid> m_listSelectDeleteFriendId = new List<Guid>();

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsFriendManager.Instance.EventFriendList += OnEventFriendList;
        CsFriendManager.Instance.EventFriendDelete += OnEventFriendDelete;
        CsFriendManager.Instance.EventFriendApplicationAccept += OnEventFriendApplicationAccept;
        CsFriendManager.Instance.EventFriendApplicationAccepted += OnEventFriendApplicationAccepted;

        CsGameEventUIToUI.Instance.EventFriendAdd += OnEventFriendAdd;
        CsGameEventUIToUI.Instance.EventSelectFriendDelete += OnEventSelectFriendDelete;

        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyInvite += OnEventPartyInvite;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsFriendManager.Instance.EventFriendList -= OnEventFriendList;
        CsFriendManager.Instance.EventFriendDelete -= OnEventFriendDelete;
        CsFriendManager.Instance.EventFriendApplicationAccept -= OnEventFriendApplicationAccept;
        CsFriendManager.Instance.EventFriendApplicationAccepted -= OnEventFriendApplicationAccepted;

        CsGameEventUIToUI.Instance.EventFriendAdd -= OnEventFriendAdd;
        CsGameEventUIToUI.Instance.EventSelectFriendDelete -= OnEventSelectFriendDelete;

        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;
        CsGameEventUIToUI.Instance.EventPartyInvite -= OnEventPartyInvite;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + 1.0f < Time.time)
        {

            m_flTime = Time.time;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendList()
    {
        Debug.Log("#@#@ OnEventFriendList #@#@");
        DisplayFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendDelete()
    {
        CsFriendManager.Instance.SendFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationAccept()
    {
        CsFriendManager.Instance.SendFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationAccepted()
    {
        CsFriendManager.Instance.SendFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendAdd(bool bAdd)
    {
        ToggleFriendItemInteractableChange(bAdd);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectFriendDelete()
    {
        if (m_listSelectDeleteFriendId.Count == 0)
        {
            return;
        }
        else
        {
            CsFriendManager.Instance.SendFriendDelete(m_listSelectDeleteFriendId.ToArray());
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        if (m_csFriend == null)
        {
            return;
        }
        else
        {
            List<CsPartyInvitation> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyInvitationList;

            if (listPartyApplication.Find(a => a.TargetId == m_csFriend.Id) == null)
            {
                CsCommandEventManager.Instance.SendPartyInvite(m_csFriend.Id);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04007"));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvite(CsPartyInvitation partyInvitation)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04019"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsFriend csFriend = CsFriendManager.Instance.FriendList.Find(a => a.Id == guidHeroId);

        if (csFriend == null)
        {
            return;
        }
        else
        {
            UpdateToggleFriendItem(csFriend);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedFriendItem(bool bIson, CsFriend csFriend)
    {
        if (bIson)
        {
            m_listSelectDeleteFriendId.Add(csFriend.Id);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            m_listSelectDeleteFriendId.Remove(csFriend.Id);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWhisper(CsFriend csFriend)
    {
        CsGameEventUIToUI.Instance.OnEventOpenOneToOneChat(csFriend.Id);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickParty(CsFriend csFriend)
    {
        if (CsGameData.Instance.MyHeroInfo.Party == null)
        {
            CsCommandEventManager.Instance.SendPartyCreate();
            m_csFriend = csFriend;
        }
        else
        {
            List<CsPartyInvitation> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyInvitationList;

            if (listPartyApplication.Find(a => a.TargetId == csFriend.Id) == null)
            {
                CsCommandEventManager.Instance.SendPartyInvite(csFriend.Id);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04007"));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTeam()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");
        m_trNoFriend = transform.Find("NoFriend");

        Text textNoFriend = m_trNoFriend.Find("TextNoFriend").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoFriend);
        textNoFriend.text = CsConfiguration.Instance.GetString("A108_TXT_00025");

        CsFriendManager.Instance.SendFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayFriendList()
    {
        if (m_goToggleFriendItem == null)
        {
            StartCoroutine(LoadToggleFriendItem(UpdateFriendList));
        }
        else
        {
            UpdateFriendList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadToggleFriendItem(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/ToggleFriendItem");
        yield return resourceRequest;

        m_goToggleFriendItem = (GameObject)resourceRequest.asset;
        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateFriendList()
    {
        for (int i = 0; i < m_trContent.childCount; i++)
        {
            m_trContent.GetChild(i).gameObject.SetActive(false);
        }

        if (CsFriendManager.Instance.FriendList.Count == 0)
        {
            m_trNoFriend.gameObject.SetActive(true);
        }
        else
        {
            m_trNoFriend.gameObject.SetActive(false);

            Transform trToggleFriendItem = null;

            for (int i = 0; i < CsFriendManager.Instance.FriendList.Count; i++)
            {
                CsFriend csFriend = CsFriendManager.Instance.FriendList[i];

                trToggleFriendItem = m_trContent.Find("ToggleFriendItem" + csFriend.Id);

                if (trToggleFriendItem == null)
                {
                    trToggleFriendItem = Instantiate(m_goToggleFriendItem, m_trContent).transform;
                    trToggleFriendItem.name = "ToggleFriendItem" + csFriend.Id;
                }
                else
                {
                    trToggleFriendItem.gameObject.SetActive(true);
                }

                UpdateToggleFriendItem(csFriend);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleFriendItem(CsFriend csFriend)
    {
        Transform trToggleFriendItem = m_trContent.Find("ToggleFriendItem" + csFriend.Id);

        if (trToggleFriendItem == null)
        {
            return;
        }
        else
        {
            Image imageJobIcon = trToggleFriendItem.Find("ImageJobIcon").GetComponent<Image>();
            imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csFriend.Job.JobId);

            Text textLevelName = trToggleFriendItem.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
            textLevelName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01004"), csFriend.Level, csFriend.Name);

            Text textBattlePower = trToggleFriendItem.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);
            textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01027"), csFriend.BattlePower.ToString("#,##0"));

            CsNation csNation = CsGameData.Instance.GetNation(csFriend.Nation.NationId);

            Image imageNation = trToggleFriendItem.Find("ImageNation").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csNation.NationId);

            Text textNation = imageNation.transform.Find("TextNation").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNation);

            if (csNation == null)
            {
                imageNation.gameObject.SetActive(false);
                textNation.text = "";
            }
            else
            {
                imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csFriend.Nation.NationId);
                textNation.text = CsGameData.Instance.GetNation(csFriend.Nation.NationId).Name;
            }

            Image imageLogin = trToggleFriendItem.Find("ImageLogin").GetComponent<Image>();

            Text textLogin = imageLogin.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLogin);

            if (csFriend.IsLoggedIn)
            {
                imageLogin.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupFriend/ico_online");
                textLogin.text = CsConfiguration.Instance.GetString("A108_TXT_01006");
            }
            else
            {
                imageLogin.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupFriend/ico_offline");
                textLogin.text = CsConfiguration.Instance.GetString("A108_TXT_01028");
            }

            Button buttonWhisper = trToggleFriendItem.Find("ButtonWhisper").GetComponent<Button>();
            buttonWhisper.onClick.RemoveAllListeners();
            buttonWhisper.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonWhisper.onClick.AddListener(() => OnClickWhisper(csFriend));

            Text textButtonWhisper = buttonWhisper.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonWhisper);
            textButtonWhisper.text = CsConfiguration.Instance.GetString("A108_BTN_00013");

            Button buttonParty = trToggleFriendItem.Find("ButtonParty").GetComponent<Button>();

            if (csFriend.Nation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonParty, true);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonParty, false);
            }

            buttonParty.onClick.RemoveAllListeners();
            buttonParty.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonParty.onClick.AddListener(() => OnClickParty(csFriend));

            Text textButtonParty = buttonParty.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonParty);
            textButtonParty.text = CsConfiguration.Instance.GetString("A108_BTN_00014");

            Button buttonTeam = trToggleFriendItem.Find("ButtonTeam").GetComponent<Button>();
            buttonTeam.onClick.RemoveAllListeners();
            buttonTeam.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonTeam.onClick.AddListener(OnClickTeam);

            Text textButtonTeam = buttonTeam.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonTeam);
            textButtonTeam.text = CsConfiguration.Instance.GetString("A108_BTN_00015");

            Toggle toggleFriendItem = trToggleFriendItem.GetComponent<Toggle>();

            if (toggleFriendItem == null)
            {
                return;
            }
            else
            {
                toggleFriendItem.onValueChanged.RemoveAllListeners();
                toggleFriendItem.isOn = false;
                toggleFriendItem.onValueChanged.AddListener((ison) => OnValueChangedFriendItem(ison, csFriend));
                toggleFriendItem.interactable = m_bDelete;
            }

            Button buttonFriendItem = trToggleFriendItem.Find("Background").GetComponent<Button>();
            buttonFriendItem.onClick.RemoveAllListeners();
            buttonFriendItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonFriendItem.onClick.AddListener(() => CsGameEventUIToUI.Instance.OnEventOpenFriendRefernce(csFriend));
            buttonFriendItem.interactable = !m_bDelete;

            Image imageButtonFriendItem = buttonFriendItem.transform.GetComponent<Image>();
            imageButtonFriendItem.raycastTarget = !m_bDelete;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ToggleFriendItemInteractableChange(bool bAdd)
    {
        for (int i = 0; i < m_trContent.childCount; i++)
        {
            Transform trToggleFriendItem = m_trContent.GetChild(i);
            Toggle toggleFriendItem = trToggleFriendItem.GetComponent<Toggle>();

            Button buttonFriendItem = trToggleFriendItem.Find("Background").GetComponent<Button>();
            Image imageButtonFriendItem = buttonFriendItem.transform.GetComponent<Image>();

            if (toggleFriendItem == null || buttonFriendItem == null)
            {
                continue;
            }
            else
            {
                if (toggleFriendItem.isOn)
                {
                    toggleFriendItem.isOn = false;
                }

                toggleFriendItem.interactable = !bAdd;
                buttonFriendItem.interactable = bAdd;
                imageButtonFriendItem.raycastTarget = bAdd;
            }
        }
    }
}