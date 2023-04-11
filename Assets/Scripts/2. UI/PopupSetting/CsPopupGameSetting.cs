using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupGameSetting : CsPopupSub
{

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {

    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
    }

    #region Event 

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSetting(bool bIson, EnPlayerPrefsKey enPlayerPrefsKey, int nValue, Text text)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsConfiguration.Instance.SetPlayerPrefsKey(enPlayerPrefsKey, nValue);
            text.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trContent = transform.Find("Scroll View/Viewport/Content");

        //사운드 설정
        Text textSoundSettingName = trContent.Find("SoundSetting/TextName").GetComponent<Text>();
        textSoundSettingName.text = CsConfiguration.Instance.GetString("A18_TXT_00005");
        CsUIData.Instance.SetFont(textSoundSettingName);

        //배경 음악
        Transform trBGM = trContent.Find("BGM");
        ToggleListSetting(EnPlayerPrefsKey.BGM, trBGM, "A18_TXT_00006");

        //효과음
        Transform trEffectSound = trContent.Find("EffectSound");
        ToggleListSetting(EnPlayerPrefsKey.EffectSound, trEffectSound, "A18_TXT_00007");

        //숨김 설정
        Text textUserViewLimitSettingName = trContent.Find("UserViewLimitSetting/TextName").GetComponent<Text>();
        textUserViewLimitSettingName.text = CsConfiguration.Instance.GetString("A18_TXT_00030");
        CsUIData.Instance.SetFont(textUserViewLimitSettingName);

        Transform trUserViewLimit = trContent.Find("UserViewLimit");
        UserViewLimitToggleListSetting(EnPlayerPrefsKey.UserViewLimit, trUserViewLimit, "A18_TXT_00028");

        Transform trUserViewFilter = trContent.Find("UserViewFilter");
        UserViewFilterToggleListSetting(EnPlayerPrefsKey.UserViewFilter, trUserViewFilter, "A18_TXT_00029");
        /*
        //다른 유저 캐릭터 숨김
        Transform trHideOtherUser = trContent.Find("HideOtherUser");
        ToggleListSetting(EnPlayerPrefsKey.HideOtherUser, trHideOtherUser, "A18_TXT_00009");

        //적국 유저 캐릭터 숨김
        Transform trHideEnemyUser = trContent.Find("HideEnemyUser");
        ToggleListSetting(EnPlayerPrefsKey.HideEnemyUser, trHideEnemyUser, "A18_TXT_00010");

        //국가전 설정
        Text textCountrySettingName = trContent.Find("CountrySetting/TextName").GetComponent<Text>();
        textCountrySettingName.text = CsConfiguration.Instance.GetString("A18_TXT_00011");
        CsUIData.Instance.SetFont(textCountrySettingName);

        //국가전 지도에서 본국 유저 캐릭터 숨김
        Transform trHideCountryOtherUser = trContent.Find("HideCountryOtherUser");
        ToggleListSetting(EnPlayerPrefsKey.HideCountryOtherUser, trHideCountryOtherUser, "A18_TXT_00012");

        //국가전 지도에서 적국 유저 캐릭터 숨김
        Transform trHideCountryEnemyUser = trContent.Find("HideCountryEnemyUser");
        ToggleListSetting(EnPlayerPrefsKey.HideCountryEnemyUser, trHideCountryEnemyUser, "A18_TXT_00013");
        */

        //간섭 설정
        Text textInterferenceSettingName = trContent.Find("InterferenceSetting/TextName").GetComponent<Text>();
        textInterferenceSettingName.text = CsConfiguration.Instance.GetString("A18_TXT_00014");
        CsUIData.Instance.SetFont(textInterferenceSettingName);

        //친구 초대 차단
        Transform trBlockFriendInvitations = trContent.Find("BlockFriendInvitations");
        ToggleListSetting(EnPlayerPrefsKey.BlockFriendInvitations, trBlockFriendInvitations, "A18_TXT_00015");

        //파티 초대 차단
        Transform trBlockPartyInvitations = trContent.Find("BlockPartyInvitations");
        ToggleListSetting(EnPlayerPrefsKey.BlockPartyInvitations, trBlockPartyInvitations, "A18_TXT_00016");

        //알림 설정
        Text textNoticeSettingName = trContent.Find("NoticeSetting/TextName").GetComponent<Text>();
        textNoticeSettingName.text = CsConfiguration.Instance.GetString("A18_TXT_00017");
        CsUIData.Instance.SetFont(textNoticeSettingName);

        //푸시 메시지 차단
        Transform trPushMessage = trContent.Find("PushMessage");
        ToggleListSetting(EnPlayerPrefsKey.PushMessage, trPushMessage, "A18_TXT_00018");

        //최적화 설정
        Text textOptimizationSettingName = trContent.Find("OptimizationSetting/TextName").GetComponent<Text>();
        textOptimizationSettingName.text = CsConfiguration.Instance.GetString("A18_TXT_00019");
        CsUIData.Instance.SetFont(textOptimizationSettingName);

        //원활 모드 사용
        Transform trSmoothMode = trContent.Find("SmoothMode");
        ToggleListSetting(EnPlayerPrefsKey.SmoothMode, trSmoothMode, "A18_TXT_00020");

    }

    //---------------------------------------------------------------------------------------------------
    void ToggleListSetting(EnPlayerPrefsKey enPlayerPrefsKey, Transform trParent, string strName)
    {
        Transform trList = trParent.Find("ToggleList");
        Text textName = trParent.Find("TextName").GetComponent<Text>();
        textName.text = CsConfiguration.Instance.GetString(strName);
        CsUIData.Instance.SetFont(textName);

        Toggle toggleOn = trList.Find("Toggle0").GetComponent<Toggle>();
        Text textToggleOn = toggleOn.transform.Find("TextToggle").GetComponent<Text>();
        toggleOn.onValueChanged.RemoveAllListeners();

        Toggle toggleOff = trList.Find("Toggle1").GetComponent<Toggle>();
        Text textToggleOff = toggleOff.transform.Find("TextToggle").GetComponent<Text>();
        toggleOff.onValueChanged.RemoveAllListeners();

        if (CsConfiguration.Instance.GetSettingKey(enPlayerPrefsKey) == 1)
        {
            toggleOn.isOn = true;
            textToggleOn.color = CsUIData.Instance.ColorWhite;
            textToggleOff.color = CsUIData.Instance.ColorGray;
        }
        else
        {
            toggleOff.isOn = true;
            textToggleOn.color = CsUIData.Instance.ColorGray;
            textToggleOff.color = CsUIData.Instance.ColorWhite;
        }


        toggleOn.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 1, textToggleOn));
        toggleOff.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 0, textToggleOff));

        textToggleOn.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ON");
        CsUIData.Instance.SetFont(textToggleOn);

        textToggleOff.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_OFF");
        CsUIData.Instance.SetFont(textToggleOff);
    }

    //---------------------------------------------------------------------------------------------------
    void UserViewLimitToggleListSetting(EnPlayerPrefsKey enPlayerPrefsKey, Transform trParent, string strName)
    {
        Transform trList = trParent.Find("ToggleList");

        Text textName = trParent.Find("TextName").GetComponent<Text>();
        textName.text = CsConfiguration.Instance.GetString(strName);
        CsUIData.Instance.SetFont(textName);

        Toggle toggleLowest = trList.Find("Toggle0").GetComponent<Toggle>();
        toggleLowest.onValueChanged.RemoveAllListeners();

        Text textToggleLowest = toggleLowest.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleLowest);
        textToggleLowest.text = CsConfiguration.Instance.GetString("A18_BTN_00019");

        Toggle toggleLow = trList.Find("Toggle1").GetComponent<Toggle>();
        toggleLow.onValueChanged.RemoveAllListeners();

        Text textToggleLow = toggleLow.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleLow);
        textToggleLow.text = CsConfiguration.Instance.GetString("A18_BTN_00020");

        Toggle toggleMedium = trList.Find("Toggle2").GetComponent<Toggle>();
        toggleMedium.onValueChanged.RemoveAllListeners();

        Text textToggleMedium = toggleMedium.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleMedium);
        textToggleMedium.text = CsConfiguration.Instance.GetString("A18_BTN_00021");
        
        Toggle toggleHigh = trList.Find("Toggle3").GetComponent<Toggle>();
        toggleHigh.onValueChanged.RemoveAllListeners();

        Text textToggleHigh = toggleHigh.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleHigh);
        textToggleHigh.text = CsConfiguration.Instance.GetString("A18_BTN_00022");

        Debug.Log("CsConfiguration.Instance.GetSettingKey(enPlayerPrefsKey) : " + CsConfiguration.Instance.GetSettingKey(enPlayerPrefsKey));
        switch (CsConfiguration.Instance.GetSettingKey(enPlayerPrefsKey))
        {
            case 0:
                toggleLowest.isOn = true;
                textToggleHigh.color = CsUIData.Instance.ColorGray;
                textToggleMedium.color = CsUIData.Instance.ColorGray;
                textToggleLow.color = CsUIData.Instance.ColorGray;
                textToggleLowest.color = CsUIData.Instance.ColorWhite;
                break;

            case 1:
                toggleLow.isOn = true;
                textToggleHigh.color = CsUIData.Instance.ColorGray;
                textToggleMedium.color = CsUIData.Instance.ColorGray;
                textToggleLow.color = CsUIData.Instance.ColorWhite;
                textToggleLowest.color = CsUIData.Instance.ColorGray;
                break;

            case 2:
                toggleMedium.isOn = true;
                textToggleHigh.color = CsUIData.Instance.ColorGray;
                textToggleMedium.color = CsUIData.Instance.ColorWhite;
                textToggleLow.color = CsUIData.Instance.ColorGray;
                textToggleLowest.color = CsUIData.Instance.ColorGray;
                break;

            case 3:
                toggleHigh.isOn = true;
                textToggleHigh.color = CsUIData.Instance.ColorWhite;
                textToggleMedium.color = CsUIData.Instance.ColorGray;
                textToggleLow.color = CsUIData.Instance.ColorGray;
                textToggleLowest.color = CsUIData.Instance.ColorGray;
                break;
        }

        toggleHigh.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 3, textToggleHigh));
        toggleMedium.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 2, textToggleMedium));
        toggleLow.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 1, textToggleLow));
        toggleLowest.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 0, textToggleLowest));
    }

    //---------------------------------------------------------------------------------------------------
    void UserViewFilterToggleListSetting(EnPlayerPrefsKey enPlayerPrefsKey, Transform trParent, string strName)
    {
        Transform trList = trParent.Find("ToggleList");

        Text textName = trParent.Find("TextName").GetComponent<Text>();
        textName.text = CsConfiguration.Instance.GetString(strName);
        CsUIData.Instance.SetFont(textName);

        Toggle toggleShowOtherUser = trList.Find("Toggle0").GetComponent<Toggle>();
        toggleShowOtherUser.onValueChanged.RemoveAllListeners();

        Text textToggleShowOtherUser = toggleShowOtherUser.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleShowOtherUser);
        textToggleShowOtherUser.text = CsConfiguration.Instance.GetString("A18_BTN_00023");

        Toggle toggleShowEnemyUser = trList.Find("Toggle1").GetComponent<Toggle>();
        toggleShowEnemyUser.onValueChanged.RemoveAllListeners();

        Text textToggleShowEnemyUser = toggleShowEnemyUser.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleShowEnemyUser);
        textToggleShowEnemyUser.text = CsConfiguration.Instance.GetString("A18_BTN_00024");

        Toggle toggleShowAllUser = trList.Find("Toggle2").GetComponent<Toggle>();
        toggleShowAllUser.onValueChanged.RemoveAllListeners();

        Text textToggleShowAllUser = toggleShowAllUser.transform.Find("TextToggle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleShowAllUser);
        textToggleShowAllUser.text = CsConfiguration.Instance.GetString("A18_BTN_00025");

        switch (CsConfiguration.Instance.GetSettingKey(enPlayerPrefsKey))
        {
            case 0:
                toggleShowOtherUser.isOn = true;

                textToggleShowAllUser.color = CsUIData.Instance.ColorGray;
                textToggleShowEnemyUser.color = CsUIData.Instance.ColorGray;
                textToggleShowOtherUser.color = CsUIData.Instance.ColorWhite;
                break;

            case 1:
                toggleShowEnemyUser.isOn = true;

                textToggleShowAllUser.color = CsUIData.Instance.ColorGray;
                textToggleShowEnemyUser.color = CsUIData.Instance.ColorWhite;
                textToggleShowOtherUser.color = CsUIData.Instance.ColorGray;
                break;

            case 2:
                toggleShowAllUser.isOn = true;

                textToggleShowAllUser.color = CsUIData.Instance.ColorWhite;
                textToggleShowEnemyUser.color = CsUIData.Instance.ColorGray;
                textToggleShowOtherUser.color = CsUIData.Instance.ColorGray;
                break;
        }

        toggleShowAllUser.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 2, textToggleShowAllUser));
        toggleShowEnemyUser.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 1, textToggleShowEnemyUser));
        toggleShowOtherUser.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, enPlayerPrefsKey, 0, textToggleShowOtherUser));
    }
}
