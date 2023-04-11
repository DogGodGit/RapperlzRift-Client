using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupGraphicSetting : CsPopupSub
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

        //프레임
        Transform trFrame = trContent.Find("Frame");
        Transform trFrameList = trFrame.Find("ToggleList");

        Text textFrameName = trFrame.Find("TextName").GetComponent<Text>();
        textFrameName.text = CsConfiguration.Instance.GetString("A18_TXT_00021");
        CsUIData.Instance.SetFont(textFrameName);

        for (int i = 0; i < 3; ++i)
        {
            int nValue = i;
            Toggle toggleFrame = trFrameList.Find("Toggle" + i).GetComponent<Toggle>();
            Text textToggle = toggleFrame.transform.Find("TextToggle").GetComponent<Text>();

            toggleFrame.onValueChanged.RemoveAllListeners();
            if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Frame) == i)
            {
                toggleFrame.isOn = true;
                textToggle.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textToggle.color = CsUIData.Instance.ColorGray;
            }

            toggleFrame.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, EnPlayerPrefsKey.Frame, nValue, textToggle));
            textToggle.text = CsConfiguration.Instance.GetString("A18_BTN_0000" + (i + 4));
            CsUIData.Instance.SetFont(textToggle);
        }

        //그래픽
        Transform trGraphic = trContent.Find("Graphic");
        Transform trGraphicList = trGraphic.Find("ToggleList");

        Text textGraphicName = trGraphic.Find("TextName").GetComponent<Text>();
        textGraphicName.text = CsConfiguration.Instance.GetString("A18_TXT_00022");
        CsUIData.Instance.SetFont(textGraphicName);

        for (int i = 0; i < 3; ++i)
        {
            int nValue = i;
            Toggle toggleGraphic = trGraphicList.Find("Toggle" + i).GetComponent<Toggle>();
            Text textToggle = toggleGraphic.transform.Find("TextToggle").GetComponent<Text>();
            toggleGraphic.onValueChanged.RemoveAllListeners();

            if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Graphic) == i)
            {
                toggleGraphic.isOn = true;
                textToggle.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textToggle.color = CsUIData.Instance.ColorGray;
            }

            toggleGraphic.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, EnPlayerPrefsKey.Graphic, nValue, textToggle));

            textToggle.text = CsConfiguration.Instance.GetString("A18_BTN_000" + (i + 7).ToString("0#"));
            CsUIData.Instance.SetFont(textToggle);
        }

        //이펙트
        Transform trEffect = trContent.Find("Effect");
        Transform trEffectList = trEffect.Find("ToggleList");

        Text textEffectName = trEffect.Find("TextName").GetComponent<Text>();
        textEffectName.text = CsConfiguration.Instance.GetString("A18_TXT_00024");
        CsUIData.Instance.SetFont(textEffectName);

        for (int i = 0; i < 3; ++i)
        {
            int nValue = i;
            Toggle toggleEffect = trEffectList.Find("Toggle" + i).GetComponent<Toggle>();
            Text textToggle = toggleEffect.transform.Find("TextToggle").GetComponent<Text>();
            toggleEffect.onValueChanged.RemoveAllListeners();
            if(CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Effect) == i)
            {
                toggleEffect.isOn = true;
                textToggle.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textToggle.color = CsUIData.Instance.ColorGray;
            }
            toggleEffect.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, EnPlayerPrefsKey.Effect, nValue, textToggle));

            textToggle.text = CsConfiguration.Instance.GetString("A18_BTN_000" + (i + 14));
            CsUIData.Instance.SetFont(textToggle);
        }

    }
}
