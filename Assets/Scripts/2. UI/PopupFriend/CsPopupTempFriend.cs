using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CsPopupTempFriend : CsPopupSub
{
    Transform m_trTempFriendContent;
    Transform m_trNoTempFriend;

    GameObject m_goToggleTempFriendItem;
    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsFriendManager.Instance.EventTempFriendAdded += OnEventTempFriendAdded;
        CsFriendManager.Instance.EventFriendApplicationAccepted += OnEventFriendApplicationAccepted;
        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsFriendManager.Instance.EventTempFriendAdded -= OnEventTempFriendAdded;
        CsFriendManager.Instance.EventFriendApplicationAccepted -= OnEventFriendApplicationAccepted;
        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventTempFriendAdded()
    {
        DisplayTempFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationAccepted()
    {
        DisplayTempFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsTempFriend csTempFriend = CsFriendManager.Instance.TempFriendList.Find(a => a.Id == guidHeroId);

        if (csTempFriend == null)
        {
            return;
        }
        else
        {
            UpdateTempFriendItem(csTempFriend);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTempFriendApply(CsTempFriend csTempFriend)
    {
        if (CsFriendManager.Instance.FriendApplicationList.Find(a => a.TargetId == csTempFriend.Id) != null)
        {
            // 친구 신청을 이미 함
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02006"));
        }
        else if (CsFriendManager.Instance.FriendList.Find(a => a.Id == csTempFriend.Id) != null)
        {
            // 이미 친구
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02007"));
        }
        else if (CsGameConfig.Instance.FriendMaxCount <= CsFriendManager.Instance.FriendList.Count)
        {
            // 친구 Max
        }
        else
        {
            // 친구 신청
            if (CsFriendManager.Instance.BlacklistEntryList.Find(a => a.HeroId == csTempFriend.Id) != null)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02011"));
            }
            else
            {
                CsFriendManager.Instance.SendFriendApply(csTempFriend.Id);
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trTempFriendContent = transform.Find("Scroll View/Viewport/Content");
        m_trNoTempFriend = transform.Find("NoTempFriend");

        Text textNoTempFriend = m_trNoTempFriend.Find("TextNoTempFriend").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoTempFriend);
        textNoTempFriend.text = CsConfiguration.Instance.GetString("A108_TXT_00026");

        DisplayTempFriendList();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayTempFriendList()
    {
        if (m_goToggleTempFriendItem == null)
        {
            StartCoroutine(LoadToggleFriendItem(UpdateTempFriendList));
        }
        else
        {
            UpdateTempFriendList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadToggleFriendItem( UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/ToggleFriendItem");
        yield return resourceRequest;

        m_goToggleTempFriendItem = (GameObject)resourceRequest.asset;
        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTempFriendList()
    {
        for (int i = 0; i < m_trTempFriendContent.childCount; i++)
        {
            m_trTempFriendContent.GetChild(i).gameObject.SetActive(false);
        }

        if (CsFriendManager.Instance.TempFriendList.Count == 0)
        {
            m_trNoTempFriend.gameObject.SetActive(true);
        }
        else
        {
            m_trNoTempFriend.gameObject.SetActive(false);

            Transform trToggleTempFriendItem = null;

            List<CsTempFriend> listCsTempFriend = new List<CsTempFriend>(CsFriendManager.Instance.TempFriendList);
            listCsTempFriend.Reverse();

            for (int i = 0; i < listCsTempFriend.Count; i++)
            {
                CsTempFriend csTempFriend = listCsTempFriend[i];

                if (CsFriendManager.Instance.FriendList.Find(a => a.Id == csTempFriend.Id) != null)
                {
                    continue;
                }
                else
                {
                    trToggleTempFriendItem = m_trTempFriendContent.Find("ToggleTempFriendItem" + csTempFriend.Id);

                    if (trToggleTempFriendItem == null)
                    {
                        trToggleTempFriendItem = Instantiate(m_goToggleTempFriendItem, m_trTempFriendContent).transform;
                        trToggleTempFriendItem.name = "ToggleTempFriendItem" + csTempFriend.Id;
                    }
                    else
                    {
                        trToggleTempFriendItem.gameObject.SetActive(true);
                    }

                    UpdateTempFriendItem(csTempFriend);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTempFriendItem(CsTempFriend csTempFriend)
    {
        Transform trToggleTempFriendItem = m_trTempFriendContent.Find("ToggleTempFriendItem" + csTempFriend.Id);

        if (trToggleTempFriendItem == null)
        {
            return;
        }
        else
        {
            Image imageJobIcon = trToggleTempFriendItem.Find("ImageJobIcon").GetComponent<Image>();
            imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csTempFriend.Job.JobId);

            Text textLevelName = trToggleTempFriendItem.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
            textLevelName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01004"), csTempFriend.Level, csTempFriend.Name);

            Text textBattlePower = trToggleTempFriendItem.Find("TextBattlePower").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBattlePower);
            textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01027"), csTempFriend.BattlePower.ToString("#,##0"));

            CsNation csNation = CsGameData.Instance.GetNation(csTempFriend.Nation.NationId);

            Image imageNation = trToggleTempFriendItem.Find("ImageNation").GetComponent<Image>();
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
                imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csTempFriend.Nation.NationId);
                textNation.text = CsGameData.Instance.GetNation(csTempFriend.Nation.NationId).Name;
            }

            Image imageLogin = trToggleTempFriendItem.Find("ImageLogin").GetComponent<Image>();
            imageLogin.gameObject.SetActive(false);

            Button buttonWhisper = trToggleTempFriendItem.Find("ButtonWhisper").GetComponent<Button>();
            buttonWhisper.gameObject.SetActive(false);

            Button buttonParty = trToggleTempFriendItem.Find("ButtonParty").GetComponent<Button>();
            buttonParty.gameObject.SetActive(false);

            Button buttonTeam = trToggleTempFriendItem.Find("ButtonTeam").GetComponent<Button>();
            buttonTeam.gameObject.SetActive(false);

            Button buttonFriendApply = trToggleTempFriendItem.Find("ButtonFriendApply").GetComponent<Button>();
            buttonFriendApply.onClick.RemoveAllListeners();
            buttonFriendApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonFriendApply.onClick.AddListener(() => OnClickTempFriendApply(csTempFriend));

            Text textButtonFriendApply = buttonFriendApply.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonFriendApply);
            textButtonFriendApply.text = CsConfiguration.Instance.GetString("A108_BTN_00030");

            buttonFriendApply.gameObject.SetActive(true);

            Toggle toggleFriendItem = trToggleTempFriendItem.GetComponent<Toggle>();

            if (toggleFriendItem == null)
            {
                return;
            }
            else
            {
                toggleFriendItem.onValueChanged.RemoveAllListeners();
                toggleFriendItem.isOn = false;
                toggleFriendItem.interactable = false;
            }
        }
    }
}
