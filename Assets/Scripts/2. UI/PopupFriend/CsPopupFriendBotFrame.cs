using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsPopupFriendBotFrame : CsPopupSub
{
    GameObject m_goPopupSearchFriend;
    GameObject m_goToggleSearchFriendInfo;

    Transform m_trPopupList;
    Transform m_trImageBackground;
    Transform m_trPopupSearchFriend;

    float m_flTime = 0.0f;

    bool m_bFirst = true;
    bool m_bAdd = true;

    Guid m_guidSelectSearchFriendId = Guid.Empty;

    string m_strSearchValue = string.Empty;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsFriendManager.Instance.EventHeroSearchForFriendApplication += OnEventHeroSearchForFriendApplication;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsFriendManager.Instance.EventHeroSearchForFriendApplication -= OnEventHeroSearchForFriendApplication;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + flTime < Time.time)
        {

            m_flTime = Time.time;
        }
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
            m_bAdd = true;
            UpdateBotFrame();

            CsGameEventUIToUI.Instance.OnEventFriendAdd(m_bAdd);
        }
    }
    
    #region Event
    
    //---------------------------------------------------------------------------------------------------
    void OnEventHeroSearchForFriendApplication(ClientCommon.PDSearchHero[] arrPDSearchHero)
    {
        DisplayPopupSearchFriend(arrPDSearchHero);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDelete()
    {
        m_bAdd = false;
        UpdateBotFrame();

        CsGameEventUIToUI.Instance.OnEventFriendAdd(m_bAdd);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAdd()
    {
        OpenPopupSearchFriend();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSelectDelete()
    {
        CsGameEventUIToUI.Instance.OnEventSelectFriendDelete();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCancel()
    {
        m_bAdd = true;
        UpdateBotFrame();

        CsGameEventUIToUI.Instance.OnEventFriendAdd(m_bAdd);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupSearchFriend()
    {
        if (m_trPopupSearchFriend == null)
        {
            return;
        }
        else
        {
            Destroy(m_trPopupSearchFriend.gameObject);
            m_trPopupSearchFriend = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSearchFriend(string strSearchValue)
    {
        if (strSearchValue == "")
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02012"));
        }
        else
        {
            String str = Regex.Replace(strSearchValue, @"[^0-9a-zA-Z가-힣]", "");

            if (string.IsNullOrEmpty(strSearchValue))
            {
                //길이 에러
                return;
            }
            else if (strSearchValue.Length > 8)
            {
                // 글자 수 제한
                return;
            }
            else if (strSearchValue.IndexOf("　") > -1 || strSearchValue.IndexOf(" ") > -1)  // ㄱ + 한자 중 첫번째 글자 또는 공백.
            {
                // 공백 문자
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02024"));
                return;
            }
            else if (str != strSearchValue)
            {
                //특수문자 찾기
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02023"));
                return;
            }
            else
            {
                m_strSearchValue = strSearchValue;
                CsFriendManager.Instance.SendHeroSearchForFriendApplication(strSearchValue);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleSearchFriendInfo(bool bIson, Guid guidSelectHeroId)
    {
        Button buttonAdd = m_trPopupSearchFriend.Find("ImageBackground/ButtonAdd").GetComponent<Button>();

        if (bIson)
        {
            m_guidSelectSearchFriendId = guidSelectHeroId;
            CsUIData.Instance.DisplayButtonInteractable(buttonAdd, true);
        }
        else
        {
            Transform trContent = m_trPopupSearchFriend.Find("ImageBackground/Scroll View/Viewport/Content");

            bool bSelect = false;

            for (int i = 0; i < trContent.childCount; i++)
            {
                Toggle toggleSearchFriend = trContent.GetChild(i).GetComponent<Toggle>();

                if (toggleSearchFriend == null)
                {
                    continue;
                }
                else
                {
                    if (toggleSearchFriend.isOn)
                    {
                        bSelect = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(buttonAdd, bSelect);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSearchFriendApply()
    {
        if (CsFriendManager.Instance.FriendList.Count < CsGameConfig.Instance.FriendMaxCount)
        {
            if (CsFriendManager.Instance.FriendApplicationList.Find(a => a.TargetId == m_guidSelectSearchFriendId) != null)
            {
                // 친구 신청을 이미 함
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02006"));
            }
            else if (CsFriendManager.Instance.FriendList.Find(a => a.Id == m_guidSelectSearchFriendId) != null)
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
                if (CsFriendManager.Instance.BlacklistEntryList.Find(a => a.HeroId == m_guidSelectSearchFriendId) != null)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02011"));
                }
                else
                {
                    CsFriendManager.Instance.SendFriendApply(m_guidSelectSearchFriendId);
                }
            }
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedApplicationAutoAccept(bool bIson, Text text)
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

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept))
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept, nAutoAccept);
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept, nAutoAccept);
        }
    }

    #endregion Event
    
    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

        m_trImageBackground = transform.Find("ImageBackground");

        Toggle toggleAuto = m_trImageBackground.Find("ToggleAuto").GetComponent<Toggle>();
        toggleAuto.onValueChanged.RemoveAllListeners();

        Text textAuto = toggleAuto.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAuto);
        textAuto.text = CsConfiguration.Instance.GetString("A108_TXT_00001");

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept))
        {
            int nAutoAccept = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept);

            if (nAutoAccept == 1)
            {
                toggleAuto.isOn = true;
                textAuto.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                toggleAuto.isOn = false;
                textAuto.color = CsUIData.Instance.ColorGray;
            }
        }
        else
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept, 0);

            toggleAuto.isOn = false;
            textAuto.color = CsUIData.Instance.ColorGray;
        }

        toggleAuto.onValueChanged.AddListener((ison) => OnValueChangedApplicationAutoAccept(ison, textAuto));
        toggleAuto.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Transform trAddFrame = m_trImageBackground.Find("AddFrame");

        Button buttonDelete = trAddFrame.Find("ButtonDelete").GetComponent<Button>();
        buttonDelete.onClick.RemoveAllListeners();
        buttonDelete.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonDelete.onClick.AddListener(OnClickDelete);

        Text textButtonDelete = buttonDelete.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonDelete);
        textButtonDelete.text = CsConfiguration.Instance.GetString("A108_BTN_00016");

        Button buttonAdd = trAddFrame.Find("ButtonAdd").GetComponent<Button>();
        buttonAdd.onClick.RemoveAllListeners();
        buttonAdd.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonAdd.onClick.AddListener(OnClickAdd);

        Text textButtonAdd = buttonAdd.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAdd);
        textButtonAdd.text = CsConfiguration.Instance.GetString("A108_BTN_00017");

        trAddFrame.gameObject.SetActive(true);

        Transform trDeleteFrame = m_trImageBackground.Find("DeleteFrame");

        Button buttonSelectDelete = trDeleteFrame.Find("ButtonSelectDelete").GetComponent<Button>();
        buttonSelectDelete.onClick.RemoveAllListeners();
        buttonSelectDelete.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonSelectDelete.onClick.AddListener(OnClickSelectDelete);

        Text textSelectDelete = buttonSelectDelete.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSelectDelete);
        textSelectDelete.text = CsConfiguration.Instance.GetString("A108_BTN_00018");

        Button buttonCancel = trDeleteFrame.Find("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonCancel.onClick.AddListener(OnClickCancel);

        Text textButtonCancel = buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonCancel);
        textButtonCancel.text = CsConfiguration.Instance.GetString("A108_BTN_00019");

        trDeleteFrame.gameObject.SetActive(false);
    }

    #region BotFrame

    //---------------------------------------------------------------------------------------------------
    void UpdateBotFrame()
    {
        Transform trAddFrame = m_trImageBackground.Find("AddFrame");
		Transform trDeleteFrame = m_trImageBackground.Find("DeleteFrame");

        Button buttonDelete = trAddFrame.Find("ButtonDelete").GetComponent<Button>();

        switch (m_iPopupMain.GetCurrentSubMenu().EnSubMenu)
        {
            case EnSubMenu.Friend:
			case EnSubMenu.FriendBlessing:

                if (m_bAdd)
                {
                    trAddFrame.gameObject.SetActive(true);
                    buttonDelete.gameObject.SetActive(true);
                    trDeleteFrame.gameObject.SetActive(false);
                }
                else
                {
                    trAddFrame.gameObject.SetActive(false);
                    trDeleteFrame.gameObject.SetActive(true);
                }

				break;

            case EnSubMenu.TempFriend:
                
				trAddFrame.gameObject.SetActive(true);
                buttonDelete.gameObject.SetActive(false);
                trDeleteFrame.gameObject.SetActive(false);

                break;

            case EnSubMenu.BlackList:
                
				trAddFrame.gameObject.SetActive(true);
                buttonDelete.gameObject.SetActive(false);
                trDeleteFrame.gameObject.SetActive(false);

                break;
        }
    }

    #endregion BotFrame

    #region SearchFriend

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSearchFriend()
    {
        if (m_goToggleSearchFriendInfo == null)
        {
            StartCoroutine(LoadPopupSearchFriend());
        }
        else
        {
            InitializePopupSearchFriend();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSearchFriend()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/PopupSearchFriend");
        yield return resourceRequest;

        m_goPopupSearchFriend = (GameObject)resourceRequest.asset;
        InitializePopupSearchFriend();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupSearchFriend()
    {
        m_trPopupSearchFriend = m_trPopupList.Find("PopupSearchFriend");

        if (m_trPopupSearchFriend == null)
        {
            m_trPopupSearchFriend = Instantiate(m_goPopupSearchFriend, m_trPopupList).transform;
            m_trPopupSearchFriend.name = "PopupSearchFriend";
        }

        Transform trImageBackground = m_trPopupSearchFriend.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A108_NAME_00004");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClosePopupSearchFriend);

        InputField inputField = trImageBackground.Find("InputField").GetComponent<InputField>();
        CsUIData.Instance.SetFont(inputField.textComponent);

        Text textPlaceHolder = inputField.transform.Find("Placeholder").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPlaceHolder);
        textPlaceHolder.text = CsConfiguration.Instance.GetString("A108_TXT_00002");

        Text textInputField = inputField.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInputField);

        Button buttonSearch = trImageBackground.Find("ButtonSearch").GetComponent<Button>();
        buttonSearch.onClick.RemoveAllListeners();
        buttonSearch.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonSearch.onClick.AddListener(() => OnClickSearchFriend(textInputField.text));

        Text textSearch = trImageBackground.Find("TextSearch").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSearch);
        textSearch.text = CsConfiguration.Instance.GetString("A108_TXT_00003");
        textSearch.gameObject.SetActive(true);

        Button buttonApply = trImageBackground.Find("ButtonAdd").GetComponent<Button>();
        buttonApply.onClick.RemoveAllListeners();
        buttonApply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonApply.onClick.AddListener(OnClickSearchFriendApply);
        CsUIData.Instance.DisplayButtonInteractable(buttonApply, false);

        Text textButtonAdd = buttonApply.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAdd);
        textButtonAdd.text = CsConfiguration.Instance.GetString("A108_BTN_00020");

        m_trPopupSearchFriend.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPopupSearchFriend(ClientCommon.PDSearchHero[] arrPDSearchHero)
    {
        if (m_goToggleSearchFriendInfo == null)
        {
            StartCoroutine(LoadToggleSearchHeroInfo(arrPDSearchHero));
        }
        else
        {
            UpdatePopupSearchFriend(arrPDSearchHero);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadToggleSearchHeroInfo(ClientCommon.PDSearchHero[] arrPDSearchHero)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/ToggleSearchFriendInfo");
        yield return resourceRequest;

        m_goToggleSearchFriendInfo = (GameObject)resourceRequest.asset;
        UpdatePopupSearchFriend(arrPDSearchHero);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupSearchFriend(ClientCommon.PDSearchHero[] arrPDSearchHero)
    {
        Transform trContent = m_trPopupSearchFriend.Find("ImageBackground/Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        Text textSearch = m_trPopupSearchFriend.Find("ImageBackground/TextSearch").GetComponent<Text>();
        textSearch.gameObject.SetActive(false);

        if (arrPDSearchHero.Length == 0)
        {
            textSearch.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01009"), m_strSearchValue);
            textSearch.gameObject.SetActive(true);
        }
        else
        {
            Transform trToggleSearchFriendInfo = null;

            for (int i = 0; i < arrPDSearchHero.Length; i++)
            {
                trToggleSearchFriendInfo = trContent.Find("ToggleSearchFriendInfo" + i);

                if (trToggleSearchFriendInfo == null)
                {
                    trToggleSearchFriendInfo = Instantiate(m_goToggleSearchFriendInfo, trContent).transform;
                    trToggleSearchFriendInfo.name = "ToggleSearchFriendInfo" + i;
                }
                else
                {
                    trToggleSearchFriendInfo.gameObject.SetActive(true);
                }

                Image imageJobIcon = trToggleSearchFriendInfo.Find("ImageJobIcon").GetComponent<Image>();
                imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + arrPDSearchHero[i].jobId);

                Text textLevelName = trToggleSearchFriendInfo.Find("TextLevelName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textLevelName);
                textLevelName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01024"), arrPDSearchHero[i].level, arrPDSearchHero[i].name);

                Text textHeroId = trToggleSearchFriendInfo.Find("TextHeroId").GetComponent<Text>();
                CsUIData.Instance.SetFont(textHeroId);
                textHeroId.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01036"), arrPDSearchHero[i].heroId.ToString());

                Guid guidHeroId = arrPDSearchHero[i].heroId;
                Toggle toggleSearchFriendInfo = trToggleSearchFriendInfo.GetComponent<Toggle>();
                toggleSearchFriendInfo.onValueChanged.RemoveAllListeners();
                toggleSearchFriendInfo.isOn = false;
                toggleSearchFriendInfo.onValueChanged.AddListener((ison) => OnValueChangedToggleSearchFriendInfo(ison, guidHeroId));
                toggleSearchFriendInfo.group = trContent.GetComponent<ToggleGroup>();
            }
        }
    }

    #endregion SearchFriend
}
