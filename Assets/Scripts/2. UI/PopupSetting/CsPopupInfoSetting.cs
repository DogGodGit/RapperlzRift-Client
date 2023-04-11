using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CsPopupInfoSetting : CsPopupSub
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

    #region EventHandler

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonHomepage()
	{
		CsGameEventUIToUI.Instance.OnEventOpenWebView(CsConfiguration.Instance.SystemSetting.HomepageUrl);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCS()
	{
		CsHelpshiftManager.Instance.DisplayHelpshift(EnHelpshiftType.CS);
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickCharacterChange()
    {
        CsUIData.Instance.IntroShortCutType = EnIntroShortCutType.CharacterSelect;
        SceneManager.LoadScene(0);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAccountChange()
    {
        CsUIData.Instance.IntroShortCutType = EnIntroShortCutType.LogOut;
        SceneManager.LoadScene(0);
    }

    void OnClickCouponCheck()
    {

    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
		Transform trImageFrameBottom = transform.Find("ImageFrameBottom");

		CsUIData.Instance.SetButton(trImageFrameBottom.Find("ButtonHomepage"), OnClickButtonHomepage);
		CsUIData.Instance.SetText(trImageFrameBottom.Find("ButtonHomepage/Text"), "A18_BTN_00027", true);

		CsUIData.Instance.SetButton(trImageFrameBottom.Find("ButtonCS"), OnClickButtonCS);
		CsUIData.Instance.SetText(trImageFrameBottom.Find("ButtonCS/Text"), "A18_BTN_00026", true);

		CsUIData.Instance.SetButton(trImageFrameBottom.Find("ButtonAccountChange"), OnClickAccountChange);
		CsUIData.Instance.SetText(trImageFrameBottom.Find("ButtonAccountChange/Text"), "A18_BTN_00017", true);

		CsUIData.Instance.SetButton(trImageFrameBottom.Find("ButtonCharacterChange"), OnClickCharacterChange);
		CsUIData.Instance.SetText(trImageFrameBottom.Find("ButtonCharacterChange/Text"), "A18_BTN_00018", true);
        
        Button buttonCouponCheck = transform.Find("ButtonCouponCheck").GetComponent<Button>();
        buttonCouponCheck.onClick.RemoveAllListeners();
        buttonCouponCheck.onClick.AddListener(OnClickCouponCheck);

        Text textCouponCheck = buttonCouponCheck.transform.Find("Text").GetComponent<Text>();
        textCouponCheck.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
        CsUIData.Instance.SetFont(textCouponCheck);

        Text textCoupon = transform.Find("TextCoupon").GetComponent<Text>();
        textCoupon.text = CsConfiguration.Instance.GetString("A18_TXT_00026");
        CsUIData.Instance.SetFont(textCoupon);

        Text textVersionInfo = transform.Find("TextVersionInfo").GetComponent<Text>();
        textVersionInfo.text = CsConfiguration.Instance.GetString("A18_TXT_00025");
        CsUIData.Instance.SetFont(textVersionInfo);

        Text textVersion = transform.Find("ImageVersion/Text").GetComponent<Text>();
        textVersion.text = CsConfiguration.Instance.ClientVersion;
        CsUIData.Instance.SetFont(textVersion);

        Text textUserInfo = transform.Find("TextUserInfo").GetComponent<Text>();
        textUserInfo.text = CsConfiguration.Instance.GetString("A18_TXT_00027");
        CsUIData.Instance.SetFont(textUserInfo);

        Text textUser = transform.Find("ImageUserInfo/Text").GetComponent<Text>();
        textUser.text = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
        CsUIData.Instance.SetFont(textUser);
    }
}