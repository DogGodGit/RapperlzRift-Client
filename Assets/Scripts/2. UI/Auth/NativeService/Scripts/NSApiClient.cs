//#undef UNITY_EDITOR
//#undef UNITY_ANDROID
//#undef UNITY_IOS

//#define UNITY_EDITOR
//#define UNITY_ANDROID
//#define UNITY_IOS

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif

using LitJson;
using PubKit;

namespace NativeService
{
	public class NSApiClient : MonoBehaviour
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 대리자

		public delegate void ResponseHandler(string sContent);

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member variables


#if UNITY_ANDROID && !UNITY_EDITOR
		private AndroidJavaObject	m_apiClient = null;
#endif

#if !UNITY_EDITOR && UNITY_IOS
		[DllImport ("__Internal")]
		private static extern long apiClient(string sContent, string sObjectName, string sMethodName);
#endif

		private Dictionary<long, ResponseHandler>	m_responseHandlers = new Dictionary<long, ResponseHandler>();

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Member functions

		private void Awake()
		{
			DontDestroyOnLoad(this);

#if !UNITY_EDITOR

			if (!PKPubKit.instance.isNativeBuild)
				return;

	#if UNITY_ANDROID
			string apiClientClassName = string.Empty;

			apiClientClassName = "com.mobblo.api.ApiClient";

			AndroidJavaClass apiClientClass = new AndroidJavaClass(apiClientClassName);  // HACK : Bundle Identifier가 바뀌면 바뀌어야 함
            
			m_apiClient = apiClientClass.CallStatic<AndroidJavaObject>("getInstance");
	#endif
#endif
		}

		private ResponseHandler PopResponseHandler(long lnAsyncToken)
		{
			ResponseHandler handler;

			if (m_responseHandlers.TryGetValue(lnAsyncToken, out handler))
			{
				m_responseHandlers.Remove(lnAsyncToken);
			}

			return handler;
		}

		public void Request(string sContent, ResponseHandler handler)
		{
#if UNITY_EDITOR
			PKUtil.LogWarning("이 기능은 네이티브 빌드에서 사용할 수 있습니다.");
#else
			if (!PKPubKit.instance.isNativeBuild)
			{
				PKUtil.LogWarning("이 기능은 네이티브 빌드에서 사용할 수 있습니다.");
				return;
			}

			long lnAsyncToken = 0;

	#if UNITY_ANDROID
			lnAsyncToken = m_apiClient.Call<long>("request", sContent, name, "OnResponse");
	#endif

	#if UNITY_IOS
			lnAsyncToken = apiClient(sContent, name, "OnResponse");
	#endif

			//UIMessageBox.Show("unity asyncToken:" + lnAsyncToken, null);

			m_responseHandlers.Add(lnAsyncToken, handler);
#endif
		}

		public void OnResponse(string sResponse)
		{
			JsonData joResponse = JsonMapper.ToObject(sResponse);
			long lnAsyncToken = NSLitJsonUtil.GetLongProperty(joResponse, "asyncToken");
			string sContent = NSLitJsonUtil.GetStringProperty(joResponse, "content");

			ResponseHandler handler = PopResponseHandler(lnAsyncToken);

			if (handler != null) 
			{
				handler (sContent);
			}
			else
			{
				//UIMessageBox.Show("handler is null. asyncToken:" + lnAsyncToken, null);
			}
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static member variables

		private static NSApiClient	s_instance = null;

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Static properties

		public static NSApiClient instance
		{
			get
			{
				if (s_instance == null)
					s_instance = NSUtil.CreateComponent<NSApiClient>();

				return s_instance;
			}
		}
	}
}
