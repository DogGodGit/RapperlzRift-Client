using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WebService : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	public const int	kErrorNo_Error		= 1;
	public const int	kErrorNo_Timeout	= 2;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Delegators

	public delegate void ResultHandler(string sContent);
	public delegate void ErrorHandler(int nErrorNo, string sErrorMessage);

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	public void Send(string sUrl, float fTimeoutInterval, ResultHandler resultHandler, ErrorHandler errorHandler)
	{
		StartCoroutine(Process(new WWW(sUrl), fTimeoutInterval, resultHandler, errorHandler));
	}

	public void Send(string sUrl, WWWForm form, float fTimeoutInterval, ResultHandler resultHandler, ErrorHandler errorHandler)
	{
		StartCoroutine(Process(new WWW(sUrl, form), fTimeoutInterval, resultHandler, errorHandler));
	}

	public void Send(string sUrl, byte[] postData, float fTimeoutInterval, ResultHandler resultHandler, ErrorHandler errorHandler)
	{
		StartCoroutine(Process(new WWW(sUrl, postData), fTimeoutInterval, resultHandler, errorHandler));
	}

	public void Send(string sUrl, byte[] postData, Dictionary<string, string> headers, float fTimeoutInterval, ResultHandler resultHandler, ErrorHandler errorHandler)
	{
		StartCoroutine(Process(new WWW(sUrl, postData, headers), fTimeoutInterval, resultHandler, errorHandler));
	}

	private IEnumerator Process(WWW www, float fTimeoutInterval, ResultHandler resultHandler, ErrorHandler errorHandler)
	{
		float fStartTime = Time.realtimeSinceStartup;

		//
		// 이 시점에 이미 완료된 상태일 수 있으므로(에러상황)
		// 비동기로 일관성있게 처리하기 위해 첫 프레임을 건너뛴다.
		//

		yield return null;

		while (!www.isDone)
		{
			float fElapsedTime = Time.realtimeSinceStartup - fStartTime;

			if (fTimeoutInterval > 0f && fElapsedTime >= fTimeoutInterval)
				break;

			yield return null;
		}

		//
		// www.isDone이 false인 경우 타임아웃.
		//

		if (!www.isDone)
		{
			www.Dispose();

			if (errorHandler != null)
				errorHandler(kErrorNo_Timeout, "Timeout.");

			yield break;
		}

		//
		// www.isDone이 true인 경우
		//

		if (!string.IsNullOrEmpty(www.error))
		{
			if (errorHandler != null)
				errorHandler(kErrorNo_Error, www.error);

			yield break;
		}

		if (resultHandler != null)
			resultHandler(www.text);
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Static member variables

	private static WebService	s_instance = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Static properties

	public static WebService instance
	{
		get
		{
			if (s_instance == null)
				s_instance = Util.CreateComponent<WebService>();

			return s_instance;
		}
	}
}
