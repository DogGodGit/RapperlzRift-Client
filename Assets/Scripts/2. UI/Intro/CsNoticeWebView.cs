using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsNoticeWebView : MonoBehaviour
{
	bool m_bInit = false;
	WebViewObject m_webView;

	//---------------------------------------------------------------------------------------------------
	//void Awake()
	//{
	//    WebMediator.Install();
	//}

	//---------------------------------------------------------------------------------------------------
	public void Show(string strUrl, int nWidth, int nHeight)
	{
		//Transform trCanvas = GameObject.Find("Canvas").transform;
		//RectTransform rtrCanvas = trCanvas.GetComponent<RectTransform>();

		//float flRatioWidth = nWidth / rtrCanvas.rect.width;
		//float flRatioHeight = nHeight / rtrCanvas.rect.height;

		//RectTransform rtrParent = transform.parent.GetComponent<RectTransform>();
		//RectTransform rtrContent = transform.GetComponent<RectTransform>();

		//int nLeft = (int)(rtrContent.anchoredPosition.x);
		//int nTop = (int)rtrContent.anchoredPosition.y * -1;
		//int nRight = (int)(rtrParent.rect.width - rtrContent.anchoredPosition.x - rtrContent.sizeDelta.x);
		//int nBottom = (int)(rtrParent.rect.height + rtrContent.anchoredPosition.y - rtrContent.sizeDelta.y);

		//WebMediator.LoadUrl(sUrl);
		//WebMediator.SetMargin((int)(nLeft * flRatioWidth), (int)(nTop * flRatioHeight), (int)(nRight * flRatioWidth), (int)(nBottom * flRatioHeight));
		//WebMediator.Show();

		if (!m_bInit)
		{
			Transform trCanvas = GameObject.Find("Canvas").transform;
			RectTransform rtrCanvas = trCanvas.GetComponent<RectTransform>();

			float flRatioWidth = nWidth / rtrCanvas.rect.width;
			float flRatioHeight = nHeight / rtrCanvas.rect.height;

			RectTransform rtrParent = transform.parent.GetComponent<RectTransform>();
			RectTransform rtrContent = transform.GetComponent<RectTransform>();

			int nLeft = (int)(rtrContent.anchoredPosition.x);
			int nTop = (int)rtrContent.anchoredPosition.y * -1;
			int nRight = (int)(rtrParent.rect.width - rtrContent.anchoredPosition.x - rtrContent.sizeDelta.x);
			int nBottom = (int)(rtrParent.rect.height + rtrContent.anchoredPosition.y - rtrContent.sizeDelta.y);

			m_webView = transform.GetComponent<WebViewObject>();
			m_webView.Init();
			m_webView.SetMargins((int)(nLeft * flRatioWidth), (int)(nTop * flRatioHeight), (int)(nRight * flRatioWidth), (int)(nBottom * flRatioHeight));
#if !UNITY_EDITOR && UNITY_ANDROID
			m_webView.SetKeyboardVisible("false");
#endif
			m_bInit = true;
		}

		if (m_webView != null)
		{
			m_webView.SetVisibility(true);
			m_webView.LoadURL(strUrl.Replace(" ", "%20"));
		}
	}

	public void Close()
	{
		//WebMediator.Hide();
		//WebMediator.LoadUrl("about:blank");

		if (m_webView != null)
		{
			m_webView.SetVisibility(false);
			m_webView.LoadURL("about:blank");
			m_bInit = false;
		}
	}
}
