using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public enum EnCustomizingMainMenu
{
    Preset = 0,
    Face = 1,
    Body = 2,
    Color = 3,
}

public enum EnFaceSubMenu
{
    FaceType = 0,
    Eyebrow = 1,
    Eye = 2,
    Nose = 3,
    Mouth = 4,
}

public enum EnBodySubMenu
{
    Upper = 0,
    Lower = 1,
}

public enum EnColorSubMenu
{
    Skin = 0,
    Eye = 1,
    Beard = 2,
    EyeMakeUp = 3,
    Hair = 4,
}

public class CsCustomizing : MonoBehaviour
{
    Transform m_trMainMenuList;
    Transform m_trSubMenuList;
    Transform m_trDisplayPreset;
    Transform m_trDisplayControl;
    Transform m_trDisplayColor;
    RectTransform m_rtrArrow;

    GameObject m_goPresetButton;

    int m_nJobId = 0;
    int m_nMaxValue = 200;

    bool m_bIsIntro;
    bool m_bIsFirst = true;

    EnCustomizingMainMenu m_enCustomizingMainMenu = EnCustomizingMainMenu.Preset;
    int m_nSubIndex = 0;

    public event Delegate EventSaveCustomByIntro;
    public event Delegate<EnCustomState> EventFaceCamera;

    #region EventHandler
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCustomizingMainMenu(Toggle toggle, EnCustomizingMainMenu enCustomizingMainMenu)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_enCustomizingMainMenu = enCustomizingMainMenu;

            switch (enCustomizingMainMenu)
            {
                case EnCustomizingMainMenu.Preset:
                    m_trSubMenuList.gameObject.SetActive(false);
                    m_trDisplayPreset.gameObject.SetActive(true);
                    m_trDisplayControl.gameObject.SetActive(false);
                    m_trDisplayColor.gameObject.SetActive(false);
                    m_rtrArrow.gameObject.SetActive(false);
                    break;

                case EnCustomizingMainMenu.Face:
                    for (int i = 0; i < 5; i++)
                    {
                        int nIndex = i;
                        Transform trSubToggle = m_trSubMenuList.Find("ToggleSub" + i);
                        trSubToggle.gameObject.SetActive(true);

                        Toggle toggleSub = trSubToggle.GetComponent<Toggle>();

                        if (i == 0)
                        {
                            if (toggleSub.isOn)
                            {
                                OnValueChangedCustomizingSubMenu(toggleSub, 0);
                            }
                            else
                            {
                                toggleSub.isOn = true;
                            }
                        }
                        else
                        {
                            toggleSub.isOn = false;
                        }

                        Image imageBack = trSubToggle.Find("Background").GetComponent<Image>();
                        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_" + (int)m_enCustomizingMainMenu + "_" + nIndex);
                    }

                    m_trSubMenuList.gameObject.SetActive(true);
                    m_rtrArrow.gameObject.SetActive(true);
                    m_rtrArrow.anchoredPosition = new Vector2(-529.5f, 80);
                    break;

                case EnCustomizingMainMenu.Body:

                    for (int i = 0; i < 2; i++)
                    {
                        int nIndex = i;
                        Transform trSubToggle = m_trSubMenuList.Find("ToggleSub" + i);
                        trSubToggle.gameObject.SetActive(true);

                        Toggle toggleSub = trSubToggle.GetComponent<Toggle>();

                        if (i == 0)
                        {
                            if (toggleSub.isOn)
                            {
                                OnValueChangedCustomizingSubMenu(toggleSub, 0);
                            }
                            else
                            {
                                toggleSub.isOn = true;
                            }
                        }
                        else
                        {
                            toggleSub.isOn = false;
                        }

                        Image imageBack = trSubToggle.Find("Background").GetComponent<Image>();
                        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_" + (int)m_enCustomizingMainMenu + "_" + nIndex);
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        Transform trSubToggle = m_trSubMenuList.Find("ToggleSub" + (i + 2));
                        trSubToggle.gameObject.SetActive(false);
                    }

                    m_trSubMenuList.gameObject.SetActive(true);
                    m_rtrArrow.gameObject.SetActive(true);
                    m_rtrArrow.anchoredPosition = new Vector2(-529.5f, 0);
                    break;

                case EnCustomizingMainMenu.Color:

                    for (int i = 0; i < 5; i++)
                    {
                        int nIndex = i;
                        Transform trSubToggle = m_trSubMenuList.Find("ToggleSub" + i);
                        trSubToggle.gameObject.SetActive(true);

                        if (i == 3)
                        {
                            // EyeMakeUp 막기
                            trSubToggle.gameObject.SetActive(false);
                            continue;
                        }

                        Toggle toggleSub = trSubToggle.GetComponent<Toggle>();

                        if (i == 0)
                        {
                            if (toggleSub.isOn)
                            {
                                OnValueChangedCustomizingSubMenu(toggleSub, 0);
                            }
                            else
                            {
                                toggleSub.isOn = true;
                            }
                        }
                        else
                        {
                            toggleSub.isOn = false;
                        }

                        Image imageBack = trSubToggle.Find("Background").GetComponent<Image>();
                        imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_" + (int)m_enCustomizingMainMenu + "_" + nIndex);
                    }


                    m_trSubMenuList.gameObject.SetActive(true);
                    m_rtrArrow.gameObject.SetActive(true);
                    m_rtrArrow.anchoredPosition = new Vector2(-529.5f, -80);
                    break;
            }

            if (EventFaceCamera != null)
            {
               // bool bFace = EnCustomizingMainMenu.Face == enCustomizingMainMenu ? true : false;
				EnCustomState enCustomState = EnCustomState.Normal;

				if (enCustomizingMainMenu == EnCustomizingMainMenu.Face)
				{
					enCustomState = EnCustomState.Zoom;
				}
				else if (enCustomizingMainMenu == EnCustomizingMainMenu.Body)
				{
					enCustomState = EnCustomState.Far;
				}

				EventFaceCamera(enCustomState);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedCustomizingSubMenu(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_nSubIndex = nIndex;

            switch (m_enCustomizingMainMenu)
            {
                case EnCustomizingMainMenu.Face:

                    switch ((EnFaceSubMenu)nIndex)
                    {
                        case EnFaceSubMenu.FaceType:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00008");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight];
                                        break;

                                    case 1:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00005");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth];
                                        break;

                                    case 2:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00006");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight];
                                        break;

                                    case 3:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00009");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth];
                                        break;

                                    case 4:
                                        trControl.gameObject.SetActive(false);
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;

                        case EnFaceSubMenu.Eyebrow:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00010");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight];
                                        break;

                                    case 1:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00012");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation];
                                        break;

                                    case 2:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 3:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 4:
                                        trControl.gameObject.SetActive(false);
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;

                        case EnFaceSubMenu.Eye:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00014");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth];
                                        break;

                                    case 1:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 2:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 3:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 4:
                                        trControl.gameObject.SetActive(false);
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;

                        case EnFaceSubMenu.Nose:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00020");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight];
                                        break;

                                    case 1:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00023");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth];
                                        break;

                                    case 2:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 3:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 4:
                                        trControl.gameObject.SetActive(false);
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;

                        case EnFaceSubMenu.Mouth:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00024");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight];
                                        break;

                                    case 1:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00025");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth];
                                        break;

                                    case 2:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 3:
                                        trControl.gameObject.SetActive(false);
                                        break;

                                    case 4:
                                        trControl.gameObject.SetActive(false);
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;
                    }

                    break;

                case EnCustomizingMainMenu.Body:

                    switch ((EnBodySubMenu)nIndex)
                    {
                        case EnBodySubMenu.Upper:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00029");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize];
                                        break;

                                    case 1:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00033");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength];
                                        break;

                                    case 2:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00032");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth];
                                        break;

                                    case 3:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00030");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize];
                                        break;

                                    case 4:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00031");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth];
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;

                        case EnBodySubMenu.Lower:

                            for (int i = 0; i < 5; i++)
                            {
                                Transform trControl = m_trDisplayControl.Find("Control" + i);
                                trControl.gameObject.SetActive(true);

                                Text textName = trControl.Find("TextControlName").GetComponent<Text>();
                                Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
                                Slider slider = trControl.Find("Slider").GetComponent<Slider>();
                                slider.maxValue = m_nMaxValue;

                                switch (i)
                                {
                                    case 0:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00034");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize];
                                        break;

                                    case 1:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00035");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth];
                                        break;

                                    case 2:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00037");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength];
                                        break;

                                    case 3:
                                        textName.text = CsConfiguration.Instance.GetString("A81_TXT_00036");
                                        textValue.text = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth].ToString();
                                        slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth];
                                        break;

                                    case 4:
                                        trControl.gameObject.SetActive(false);
                                        break;
                                }
                            }

                            m_trDisplayPreset.gameObject.SetActive(false);
                            m_trDisplayControl.gameObject.SetActive(true);
                            m_trDisplayColor.gameObject.SetActive(false);
                            break;
                    }

                    break;

                case EnCustomizingMainMenu.Color:
                    Text textColorIndex = m_trDisplayColor.Find("TextColorIndex").GetComponent<Text>();

                    int nSelectIndex = 0;

                    switch ((EnColorSubMenu)nIndex)
                    {
                        case EnColorSubMenu.Skin:
                            nSelectIndex = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.Skin];
                            textColorIndex.text = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.Skin].ToString();
                            break;

                        case EnColorSubMenu.Eye:
                            nSelectIndex = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.Eyes];
                            textColorIndex.text = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.Eyes].ToString();
                            break;

                        case EnColorSubMenu.Beard:
                            nSelectIndex = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.EyeBrowAndLips];
                            textColorIndex.text = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.EyeBrowAndLips].ToString();
                            break;

                        //case EnColorSubMenu.EyeMakeUp:
                        //nSelectIndex = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.EyeMakeUp];
                        //textColorIndex.text = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.EyeMakeUp].ToString();
                        //break;

                        case EnColorSubMenu.Hair:
                            nSelectIndex = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.Hair];
                            textColorIndex.text = CsCustomizingManager.Instance.TempCustom.CustomColor[EnCustomColor.Hair].ToString();
                            break;
                    }

                    Transform trColorGrid = m_trDisplayColor.Find("ColorGrid");

                    if ((EnColorSubMenu)nIndex == EnColorSubMenu.Skin)
                    {
                        SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listSkinColor, nSelectIndex);
                    }
                    else
                    {
                        SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listFaceColor, nSelectIndex);
                    }
                    /* 수정
                    for (int i = 0; i < CsCustomizingManager.Instance.listColor.Count; i++)
                    {
                        int nColorIndex = i;

                        Toggle toggleColor = m_trDisplayColor.Find("ColorGrid/ToggleColor" + i).GetComponent<Toggle>();
                        toggleColor.onValueChanged.RemoveAllListeners();
                        toggleColor.isOn = (i == nSelectIndex);
                        toggleColor.onValueChanged.AddListener((ison) => OnValueChangedColor(toggleColor, nColorIndex));
                    }
                    */

                    m_trDisplayPreset.gameObject.SetActive(false);
                    m_trDisplayControl.gameObject.SetActive(false);
                    m_trDisplayColor.gameObject.SetActive(true);

                    if (EventFaceCamera != null)
                    {
                        bool bFace = EnColorSubMenu.Skin == (EnColorSubMenu)nIndex ? false : true;
						if (bFace)
						{
							EventFaceCamera(EnCustomState.Far);
						}
						else
						{
							EventFaceCamera(EnCustomState.Zoom);
						}
                    }
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedControlSlider(Slider slider, int nIndex)
    {
        ChangeSliderText(nIndex, (int)slider.value);

        switch (m_enCustomizingMainMenu)
        {
            case EnCustomizingMainMenu.Face:

                switch ((EnFaceSubMenu)m_nSubIndex)
                {
                    case EnFaceSubMenu.FaceType:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawHeight, (int)slider.value);
                                break;

                            case 1:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawWidth, (int)slider.value);
                                break;

                            case 2:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawEndHeight, (int)slider.value);
                                break;

                            case 3:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.FaceWidth, (int)slider.value);
                                break;

                            case 4:
                                //m_nFaceType4 = (int)slider.value;
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Eyebrow:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyebrowHeight, (int)slider.value);
                                break;

                            case 1:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyebrowRotation, (int)slider.value);
                                break;

                            case 2:
                                //m_nEyebrow2 = (int)slider.value;
                                break;

                            case 3:
                                //m_nEyebrow3 = (int)slider.value;
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Eye:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyesWidth, (int)slider.value);
                                break;

                            case 1:
                                //m_nEye1 = (int)slider.value;
                                break;

                            case 2:
                                //m_nEye2 = (int)slider.value;
                                break;

                            case 3:
                                //m_nEye3 = (int)slider.value;
                                break;

                            case 4:
                                //m_nEye4 = (int)slider.value;
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Nose:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.NoseHeight, (int)slider.value);
                                break;

                            case 1:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.NoseWidth, (int)slider.value);
                                break;

                            case 2:
                                //m_nNose2 = (int)slider.value;
                                break;

                            case 3:
                                //m_nNose3 = (int)slider.value;
                                break;

                            case 4:
                                //m_nNose4 = (int)slider.value;
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Mouth:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.MouthHeight, (int)slider.value);
                                break;

                            case 1:
                                CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.MouthWidth, (int)slider.value);
                                break;

                            case 2:
                                //m_nMouth2 = (int)slider.value;
                                break;

                            case 3:
                                //m_nMouth3 = (int)slider.value;
                                break;

                            case 4:
                                //m_nMouth4 = (int)slider.value;
                                break;
                        }
                        break;
                }

                break;

            case EnCustomizingMainMenu.Body:

                switch ((EnBodySubMenu)m_nSubIndex)
                {
                    case EnBodySubMenu.Upper:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.HeadSize, (int)slider.value);
                                break;

                            case 1:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ArmsLength, (int)slider.value);
                                break;

                            case 2:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ArmsWidth, (int)slider.value);
                                break;

                            case 3:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ChestSize, (int)slider.value);
                                break;

                            case 4:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.WaistWidth, (int)slider.value);
                                break;
                        }
                        break;

                    case EnBodySubMenu.Lower:

                        switch (nIndex)
                        {
                            case 0:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.HipsSize, (int)slider.value);
                                break;

                            case 1:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.PelvisWidth, (int)slider.value);
                                break;

                            case 2:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.LegsLength, (int)slider.value);
                                break;

                            case 3:
                                CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.LegsWidth, (int)slider.value);
                                break;
                        }
                        break;
                }
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickControlRight(int nIndex)
    {
        Slider slider = m_trDisplayControl.Find("Control" + nIndex + "/Slider").GetComponent<Slider>();

        switch (m_enCustomizingMainMenu)
        {
            case EnCustomizingMainMenu.Face:

                switch ((EnFaceSubMenu)m_nSubIndex)
                {
                    case EnFaceSubMenu.FaceType:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth];
                                }
                                break;

                            case 2:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawEndHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight];
                                }
                                break;

                            case 3:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.FaceWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth];
                                }
                                break;

                            case 4:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Eyebrow:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyebrowHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyebrowRotation, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation];
                                }
                                break;

                            case 2:
                                break;

                            case 3:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Eye:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyesWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth];
                                }
                                break;

                            case 1:
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Nose:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.NoseHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.NoseWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth];
                                }
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Mouth:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.MouthHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.MouthWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth];
                                }
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                        break;
                }

                break;

            case EnCustomizingMainMenu.Body:

                switch ((EnBodySubMenu)nIndex)
                {
                    case EnBodySubMenu.Upper:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.HeadSize, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ArmsLength, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength];
                                }
                                break;

                            case 2:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ArmsWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth];
                                }
                                break;

                            case 3:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ChestSize, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize];
                                }
                                break;

                            case 4:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.WaistWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth];
                                }
                                break;
                        }
                        break;

                    case EnBodySubMenu.Lower:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.HipsSize, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.PelvisWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth];
                                }
                                break;

                            case 2:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.LegsLength, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength];
                                }
                                break;

                            case 3:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth] < m_nMaxValue)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.LegsWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth] + 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth];
                                }
                                break;
                        }
                        break;
                }
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickControlLeft(int nIndex)
    {
        Slider slider = m_trDisplayControl.Find("Control" + nIndex + "/Slider").GetComponent<Slider>();

        switch (m_enCustomizingMainMenu)
        {
            case EnCustomizingMainMenu.Face:

                switch ((EnFaceSubMenu)m_nSubIndex)
                {
                    case EnFaceSubMenu.FaceType:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawWidth];
                                }
                                break;

                            case 2:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.JawEndHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.JawEndHeight];
                                }
                                break;

                            case 3:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.FaceWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.FaceWidth];
                                }
                                break;

                            case 4:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Eyebrow:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyebrowHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyebrowRotation, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyebrowRotation];
                                }
                                break;

                            case 2:
                                break;

                            case 3:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Eye:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.EyesWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.EyesWidth];
                                }
                                break;

                            case 1:
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Nose:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.NoseHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.NoseWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.NoseWidth];
                                }
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                        break;

                    case EnFaceSubMenu.Mouth:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.MouthHeight, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthHeight];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomFace(EnCustomFace.MouthWidth, CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomFace[EnCustomFace.MouthWidth];
                                }
                                break;

                            case 2:

                            case 3:
                                break;

                            case 4:
                                break;
                        }
                        break;
                }

                break;

            case EnCustomizingMainMenu.Body:

                switch ((EnBodySubMenu)nIndex)
                {
                    case EnBodySubMenu.Upper:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.HeadSize, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HeadSize];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ArmsLength, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsLength];
                                }
                                break;

                            case 2:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ArmsWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ArmsWidth];
                                }
                                break;

                            case 3:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.ChestSize, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.ChestSize];
                                }
                                break;

                            case 4:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.WaistWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.WaistWidth];
                                }
                                break;
                        }
                        break;

                    case EnBodySubMenu.Lower:

                        switch (nIndex)
                        {
                            case 0:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.HipsSize, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.HipsSize];
                                }
                                break;

                            case 1:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.PelvisWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.PelvisWidth];
                                }
                                break;

                            case 2:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.LegsLength, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsLength];
                                }
                                break;

                            case 3:
                                if (CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth] > 0)
                                {
                                    CsCustomizingManager.Instance.ChangeCustomBody(EnCustomBody.LegsWidth, CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth] - 1);
                                    slider.value = CsCustomizingManager.Instance.TempCustom.CustomBody[EnCustomBody.LegsWidth];
                                }
                                break;
                        }
                        break;
                }
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHairPreset(int nIndex)
	{
		if (EventFaceCamera != null)
		{
			EventFaceCamera(EnCustomState.Normal);
		}
        CsCustomizingManager.Instance.ChangeCustomFreeSet(EnCustomFreeSet.Hair, m_nJobId, nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickFacePreset(int nIndex)
    {
		if (EventFaceCamera != null)
		{
			EventFaceCamera(EnCustomState.Zoom);
		}
        CsCustomizingManager.Instance.ChangeCustomFreeSet(EnCustomFreeSet.Face, m_nJobId, nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBodyPreset(int nIndex)
	{
		if (EventFaceCamera != null)
		{
			EventFaceCamera(EnCustomState.Far);
		}
        CsCustomizingManager.Instance.ChangeCustomFreeSet(EnCustomFreeSet.Body, m_nJobId, nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedColor(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            Text textColorIndex = m_trDisplayColor.Find("TextColorIndex").GetComponent<Text>();
            textColorIndex.text = nIndex.ToString("#,##0");

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            switch ((EnColorSubMenu)m_nSubIndex)
            {
                case EnColorSubMenu.Skin:
                    CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.Skin, nIndex);
                    break;

                case EnColorSubMenu.Eye:
                    CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.Eyes, nIndex);
                    break;

                case EnColorSubMenu.Beard:
                    CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.EyeBrowAndLips, nIndex);
                    break;

                //case EnColorSubMenu.EyeMakeUp:
                //CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.EyeMakeUp, nIndex);
                //break;

                case EnColorSubMenu.Hair:
                    CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.Hair, nIndex);
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickResetColor()
	{
		Text textColorIndex = m_trDisplayColor.Find("TextColorIndex").GetComponent<Text>();
		Transform trColorGrid = m_trDisplayColor.Find("ColorGrid");

        switch ((EnColorSubMenu)m_nSubIndex)
        {
            case EnColorSubMenu.Skin:
				CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.Skin, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Skin]);
				textColorIndex.text = CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Skin].ToString();
				SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listSkinColor, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Skin]);
                break;

            case EnColorSubMenu.Eye:
				CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.Eyes, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Eyes]);
				textColorIndex.text = CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Eyes].ToString();
				SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listFaceColor, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Eyes]);
                break;

            case EnColorSubMenu.Beard:
				CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.EyeBrowAndLips, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.EyeBrowAndLips]);
				textColorIndex.text = CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.EyeBrowAndLips].ToString();
				SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listFaceColor, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.EyeBrowAndLips]);
                break;

            //case EnColorSubMenu.EyeMakeUp:
            //CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.EyeMakeUp, 0);
            //break;

            case EnColorSubMenu.Hair:
				CsCustomizingManager.Instance.ChangeCustomColor(EnCustomColor.Hair, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Hair]);
				textColorIndex.text = CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Hair].ToString();
				SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listFaceColor, CsCustomizingManager.Instance.DefaultCustom.CustomColor[EnCustomColor.Hair]);
                break;
        }

        /* 수정
        for (int i = 0; i < CsCustomizingManager.Instance.listColor.Count; i++)
        {
            int nColorIndex = i;

            Toggle toggleColor = m_trDisplayColor.Find("ColorGrid/ToggleColor" + i).GetComponent<Toggle>();
            toggleColor.onValueChanged.RemoveAllListeners();
            toggleColor.isOn = (i == 0);
            toggleColor.onValueChanged.AddListener((ison) => OnValueChangedColor(toggleColor, nColorIndex));
        }
        */
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickResetCustom()
    {
        if (m_bIsIntro)
        {
			CsCustomizingManager.Instance.Reset();
            Toggle toggleMain = m_trMainMenuList.Find("ToggleMain0").GetComponent<Toggle>();
            toggleMain.isOn = true;
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSaveCustom()
    {
        if (m_bIsIntro)
        {
            EventSaveCustomByIntro();
        }
        else
        {

        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    public void SetDisplay(int nJobId, bool bIntro)
    {
        m_nJobId = nJobId;
        m_bIsIntro = bIntro;

        if (m_bIsFirst)
        {
            InitializeUI();
            m_bIsFirst = false;
        }
        else
        {
            Toggle toggleMain = m_trMainMenuList.Find("ToggleMain0").GetComponent<Toggle>();
            toggleMain.isOn = true;

            UpdatePreset();
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SetDefault()
    {
        //CsCustomizingManager.Instance.Reset();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_rtrArrow = transform.Find("ImageArrow").GetComponent<RectTransform>();
        m_rtrArrow.gameObject.SetActive(false);

        m_trMainMenuList = transform.Find("CustomizingMainMenuList");

        for (int i = 0; i < m_trMainMenuList.childCount; i++)
        {
            int nIndex = i;
            Transform trMainToggle = m_trMainMenuList.Find("ToggleMain" + i);

            Toggle toggleMain = trMainToggle.GetComponent<Toggle>();
            toggleMain.onValueChanged.RemoveAllListeners();

            if (i == 0)
            {
                toggleMain.isOn = true;
            }
            else
            {
                toggleMain.isOn = false;
            }

            toggleMain.onValueChanged.AddListener((ison) => OnValueChangedCustomizingMainMenu(toggleMain, (EnCustomizingMainMenu)nIndex));
        }


        m_trSubMenuList = transform.Find("CustomizingSubMenuList");

        for (int i = 0; i < m_trSubMenuList.childCount; i++)
        {
            int nIndex = i;
            Transform trSubToggle = m_trSubMenuList.Find("ToggleSub" + i);

            Toggle toggleSub = trSubToggle.GetComponent<Toggle>();
            toggleSub.onValueChanged.RemoveAllListeners();

            if (i == 0)
            {
                toggleSub.isOn = true;
            }
            else
            {
                toggleSub.isOn = false;
            }

            toggleSub.onValueChanged.AddListener((ison) => OnValueChangedCustomizingSubMenu(toggleSub, nIndex));
        }

        m_trSubMenuList.gameObject.SetActive(false);

        // 우측 메뉴 -> 프리셋 세팅 / 슬라이더 텍스트 세팅 및 감추기 / 컬러 텍스트 세팅 및 감추기

        Transform trDisplay = transform.Find("CustomDisplay");

        //프리셋 리스트
        m_trDisplayPreset = trDisplay.Find("PresetList");

        if (m_goPresetButton == null)
        {
            m_goPresetButton = CsUIData.Instance.LoadAsset<GameObject>("GUI/Customizing/ButtonPreset");
        }

        Text texthair = m_trDisplayPreset.Find("TextHair").GetComponent<Text>();
        CsUIData.Instance.SetFont(texthair);
        texthair.text = CsConfiguration.Instance.GetString("A81_TXT_00002");

        Transform trHairContent = texthair.transform.Find("Scroll View/Viewport/Content");

        //헤어프리셋 생성
        for (int i = 0; i < 3; i++)
        {
            int nIndex = i;
            GameObject goPresetButton = Instantiate(m_goPresetButton, trHairContent);
            goPresetButton.name = "Preset" + i;

            Button buttonPreset = goPresetButton.GetComponent<Button>();
            buttonPreset.onClick.RemoveAllListeners();
            buttonPreset.onClick.AddListener(() => OnClickHairPreset(nIndex));
            buttonPreset.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            goPresetButton.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_hair_" + m_nJobId + "_" + (nIndex + 1));
        }

        Text textFace = m_trDisplayPreset.Find("TextFace").GetComponent<Text>();
        CsUIData.Instance.SetFont(textFace);
        textFace.text = CsConfiguration.Instance.GetString("A81_TXT_00003");

        Transform trFaceContent = textFace.transform.Find("Scroll View/Viewport/Content");

        //얼굴프리셋 생성
        for (int i = 0; i < 3; i++)
        {
            int nIndex = i;
            GameObject goPresetButton = Instantiate(m_goPresetButton, trFaceContent);
            goPresetButton.name = "Preset" + i;

            Button buttonPreset = goPresetButton.GetComponent<Button>();
            buttonPreset.onClick.RemoveAllListeners();
            buttonPreset.onClick.AddListener(() => OnClickFacePreset(nIndex));
            buttonPreset.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            goPresetButton.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_face_" + m_nJobId + "_" + (nIndex + 1));
        }

        Text textBody = m_trDisplayPreset.Find("TextBody").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBody);
        textBody.text = CsConfiguration.Instance.GetString("A81_TXT_00004");

        Transform trBodyContent = textBody.transform.Find("Scroll View/Viewport/Content");

        //바디프리셋 생성
        for (int i = 0; i < 3; i++)
        {
            int nIndex = i;
            GameObject goPresetButton = Instantiate(m_goPresetButton, trBodyContent);
            goPresetButton.name = "Preset" + i;

            Button buttonPreset = goPresetButton.GetComponent<Button>();
            buttonPreset.onClick.RemoveAllListeners();
            buttonPreset.onClick.AddListener(() => OnClickBodyPreset(nIndex));
            buttonPreset.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            goPresetButton.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_body_" + m_nJobId + "_" + (nIndex + 1));
        }

        m_trDisplayPreset.gameObject.SetActive(true);

        // 컨트롤러 리스트
        m_trDisplayControl = trDisplay.Find("SliderList");

        for (int i = 0; i < 5; i++)
        {
            int nIndex = i;

            Transform trControl = m_trDisplayControl.Find("Control" + i);

            Text textName = trControl.Find("TextControlName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);

            Text textValue = trControl.Find("TextControlValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textValue);

            Slider slider = trControl.Find("Slider").GetComponent<Slider>();
            slider.onValueChanged.AddListener((ison) => OnValueChangedControlSlider(slider, nIndex));

            Button buttonRight = trControl.Find("ButtonRight").GetComponent<Button>();
            buttonRight.onClick.RemoveAllListeners();
            buttonRight.onClick.AddListener(() => OnClickControlRight(nIndex));
            buttonRight.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Button buttonLeft = trControl.Find("ButtonLeft").GetComponent<Button>();
            buttonLeft.onClick.RemoveAllListeners();
            buttonLeft.onClick.AddListener(() => OnClickControlLeft(nIndex));
            buttonLeft.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        m_trDisplayControl.gameObject.SetActive(false);

        // 컬러판
        m_trDisplayColor = trDisplay.Find("ColorList");

        Text textColor = m_trDisplayColor.Find("TextColor").GetComponent<Text>();
        CsUIData.Instance.SetFont(textColor);
        textColor.text = CsConfiguration.Instance.GetString("A81_TXT_00038");

        Text textColorIndex = m_trDisplayColor.Find("TextColorIndex").GetComponent<Text>();
        CsUIData.Instance.SetFont(textColorIndex);
        textColorIndex.text = CsConfiguration.Instance.GetString("0");

        Transform trColorGrid = m_trDisplayColor.Find("ColorGrid");
        SetColorGridList(trColorGrid, CsCustomizingManager.Instance.listFaceColor);

        Button buttonResetColor = trColorGrid.Find("ButtonReset").GetComponent<Button>();
        buttonResetColor.onClick.RemoveAllListeners();
        buttonResetColor.onClick.AddListener(OnClickResetColor);
        buttonResetColor.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trDisplayColor.gameObject.SetActive(false);

        // 버튼 세팅 -> 리셋버튼 / 확인버튼 / 토글2개(현재미구현)

        Button buttonReset = transform.Find("ButtonReset").GetComponent<Button>();
        buttonReset.onClick.RemoveAllListeners();
        buttonReset.onClick.AddListener(OnClickResetCustom);
        buttonReset.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textReset = buttonReset.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReset);
        textReset.text = CsConfiguration.Instance.GetString("A81_TXT_00001");

        Button buttonSaveCustom = transform.Find("ButtonNext").GetComponent<Button>();
        buttonSaveCustom.onClick.RemoveAllListeners();
        buttonSaveCustom.onClick.AddListener(OnClickSaveCustom);
        buttonSaveCustom.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textNext = buttonSaveCustom.transform.Find("Text").GetComponent<Text>();
        textNext.gameObject.SetActive(m_bIsIntro);

        Transform trIngame = buttonSaveCustom.transform.Find("InGame");
        trIngame.gameObject.SetActive(!m_bIsIntro);

        Text textDia = trIngame.Find("TextDia").GetComponent<Text>();
        Text textSaveCustom = trIngame.Find("TextSave").GetComponent<Text>();

        if (m_bIsIntro)
        {
            CsUIData.Instance.SetFont(textNext);
            textNext.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NEXT");
        }
        else
        {
            CsUIData.Instance.SetFont(textDia);
            textDia.text = "";

            CsUIData.Instance.SetFont(textSaveCustom);
            textSaveCustom.text = CsConfiguration.Instance.GetString("A81_BTN_00002");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePreset()
    {
        Transform trHairContent = m_trDisplayPreset.Find("TextHair/Scroll View/Viewport/Content");

        //헤어프리셋
        for (int i = 0; i < 3; i++)
        {
            Transform trPreset = trHairContent.Find("Preset" + i);

            if (trPreset != null)
            {
                trPreset.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_hair_" + m_nJobId + "_" + (i + 1));
            }
        }

        Transform trFaceContent = m_trDisplayPreset.Find("TextFace/Scroll View/Viewport/Content");

        //얼굴프리셋 생성
        for (int i = 0; i < 3; i++)
        {
            Transform trPreset = trFaceContent.Find("Preset" + i);

            if (trPreset != null)
            {
                trPreset.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_face_" + m_nJobId + "_" + (i + 1));
            }
        }

        Transform trBodyContent = m_trDisplayPreset.Find("TextBody/Scroll View/Viewport/Content");

        //바디프리셋 생성
        for (int i = 0; i < 3; i++)
        {
            Transform trPreset = trBodyContent.Find("Preset" + i);

            if (trPreset != null)
            {
                trPreset.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Customizing/ico_customizing_body_" + m_nJobId + "_" + (i + 1));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ChangeSliderText(int nIndex, int nValue)
    {
        Text textValue = m_trDisplayControl.Find("Control" + nIndex + "/TextControlValue").GetComponent<Text>();
        textValue.text = nValue.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void SetColorGridList(Transform trColorGrid, List<Color32> listColor, int nOnIndex = 0)
    {
        trColorGrid = m_trDisplayColor.Find("ColorGrid");

        for (int i = 0; i < listColor.Count; i++)
        {
            int nIndex = i;

            Toggle toggleColor = trColorGrid.Find("ToggleColor" + i).GetComponent<Toggle>();
            toggleColor.onValueChanged.RemoveAllListeners();


            if (i == nOnIndex)
            {
                toggleColor.isOn = true;
            }
            else
            {
                toggleColor.isOn = false;
            }

            toggleColor.onValueChanged.AddListener((ison) => OnValueChangedColor(toggleColor, nIndex));

            Image imageBack = toggleColor.transform.Find("Background").GetComponent<Image>();
            imageBack.color = listColor[i];
        }
    }
}
