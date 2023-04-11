using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-16)
//---------------------------------------------------------------------------------------------------

public class CsPopupFrameWebView : MonoBehaviour 
{
	//---------------------------------------------------------------------------------------------------
	public void LoadUrl(string strUrl)
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;

#if UNITY_IOS
		transform.Find("ImageBackground/ImageTitle").gameObject.SetActive(false);
		transform.Find("ButtonClose").gameObject.SetActive(false);
#endif
		UniWebView webView = gameObject.AddComponent<UniWebView>();


#if UNITY_ANDROID
		Transform trCanvas = GameObject.Find("Canvas").transform;

		float flRealSizeRatio = CsUIData.Instance.DeviceResolution.y / trCanvas.GetComponent<CanvasScaler>().referenceResolution.y;

		RectTransform rtrImageTitle = transform.Find("ImageBackground/ImageTitle").GetComponent<RectTransform>();

		webView.insets = new UniWebViewEdgeInsets((int)(rtrImageTitle.sizeDelta.y * flRealSizeRatio), 0, 0, 0);
#endif



		Button button = transform.Find("ButtonClose").GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(PopupClose);
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
	{
		if (success)
		{
			webView.Show();
		}
		else
		{
			Debug.Log("You Fail Load Webview");
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
	{

	}

	//---------------------------------------------------------------------------------------------------
	bool OnWebViewShouldClose(UniWebView webView)
	{
		PopupClose();
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnReceivedKeyCode(UniWebView webView, int keyCode)
	{
	}
}
