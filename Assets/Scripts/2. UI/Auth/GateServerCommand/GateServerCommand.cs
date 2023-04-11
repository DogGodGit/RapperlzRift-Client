using UnityEngine;
using System;
using System.Collections;
using System.Text;

using LitJson;

public abstract class GateServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	//
	// 상태 코드.
	//

	public const int kStatus_None = 0;
	public const int kStatus_Running = 1;
	public const int kStatus_Finished = 2;

	//
	// 결과 코드.
	//

	public const int kResult_OK = 0;
	public const int kResult_Error = 1;
	public const int kResult_WebError = 2;
	public const int kResult_Timeout = 3;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Events

	public event EventHandler Finished = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	protected string m_sCommand = null;
	protected float m_fTimeoutInterval = 0;

	protected int m_nStatus = kStatus_None;

	protected int m_nResult = kResult_OK;
	protected GateServerResponse m_response = null;
	protected Exception m_error = null;

	//
	// 사용자 정의 데이터.
	//

	protected object m_stateData = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public GateServerCommand(string sCommand)
	{
		m_sCommand = sCommand;

		//if (Config.instance.SystemSettingList.ContainsKey("clientTimeoutLimit"))
		//{
		//    m_fTimeoutInterval = float.Parse(Config.instance.SystemSettingList["clientTimeoutLimit"]);
		//}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public virtual string command
	{
		get { return m_sCommand; }
	}

	public virtual float timeoutInterval
	{
		get { return m_fTimeoutInterval; }
		set { m_fTimeoutInterval = value; }
	}

	public virtual int status
	{
		get { return m_nStatus; }
	}

	public virtual bool running
	{
		get { return m_nStatus == kStatus_Running; }
	}

	public virtual bool finished
	{
		get { return m_nStatus == kStatus_Finished; }
	}

	public virtual int result
	{
		get { return m_nResult; }
	}

	public virtual GateServerResponse response
	{
		get { return m_response; }
	}

	public virtual Exception error
	{
		get { return m_error; }
	}

	public virtual object stateData
	{
		get { return m_stateData; }
		set { m_stateData = value; }
	}

	public virtual bool isOK
	{
		get { return m_nResult == kResult_OK; }
	}

	public virtual bool isAllOK
	{
		get
		{
			if (!finished) 
				return true;

			return isOK && m_response.isOK;
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected virtual void Finish(int nResult, GateServerResponse response, Exception error)
	{
		if (m_nStatus != kStatus_Running) return;

		m_nResult = nResult;
		m_response = response;
		m_error = error;

		m_nStatus = kStatus_Finished;

		if (Finished != null)
		{
			Finished(this, EventArgs.Empty);
		}
	}

	protected abstract GateServerResponse CreateResponse();

	protected virtual JsonData MakeRequestContent()
	{
		JsonData joReq = LitJsonUtil.CreateObject();
		joReq["cmd"] = m_sCommand;

		return joReq;
	}

	public virtual void Run()
	{
		if (m_nStatus != kStatus_None)
		{
			throw new InvalidOperationException("이미 실행되었습니다.");
		}

		string sCommandUrl = CsConfiguration.Instance.GateServerApiUrl;

		JsonData joReq = MakeRequestContent();

		if (joReq != null)
		{
			WebService.instance.Send(sCommandUrl, Encoding.UTF8.GetBytes(joReq.ToJson()), m_fTimeoutInterval, OnResult, OnError);
		}
		else
		{
			WebService.instance.Send(sCommandUrl, m_fTimeoutInterval, OnResult, OnError);
		}

		m_nStatus = kStatus_Running;
	}

	protected virtual void OnResult(string sContent)
	{
		//
		// 응답 생성 및 처리.
		//

		int nResult = kResult_OK;
		GateServerResponse response = null;
		Exception error = null;

		try
		{
			response = CreateResponse();
			response.Handle(sContent);
		}
		catch (Exception ex)
		{
			nResult = kResult_Error;
			error = ex;
		}

		Finish(nResult, response, error);
	}

	protected virtual void OnError(int nErrorNo, string sErrorMessage)
	{
		int nResult = kResult_WebError;

		if (nErrorNo == WebService.kErrorNo_Timeout)
		{
			nResult = kResult_Timeout;
		}

		Finish(nResult, null, new Exception(sErrorMessage));
	}

	public virtual string Trace()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("[" + GetType().Name + "]");

		sb.Append(Environment.NewLine);
		sb.Append("# Request Content: " + TraceRequestContent());

		sb.Append(Environment.NewLine);
		sb.Append("# Command.result: " + m_nResult);

		sb.Append(Environment.NewLine);
		sb.Append("# Command.error: " + (m_error == null ? "null" : TraceError()));

		sb.Append(Environment.NewLine);
		sb.Append("# Response: " + (m_response == null ? "null" : m_response.Trace()));

		return sb.ToString();
	}

	public virtual string TraceError()
	{
		return m_error == null ? null : Util.ToString(m_error);
	}

	public virtual string TraceRequestContent()
	{
		JsonData joContent = MakeRequestContent();

		return joContent == null ? null : joContent.ToJson();
	}
}

public abstract class GateServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	public const int kResult_OK = 0;
	public const int kResult_Error = 1;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	protected JsonData m_joContent = null;

	protected int m_nResult = kResult_OK;
	protected string m_sErrorMessage = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public virtual JsonData content
	{
		get { return m_joContent; }
	}

	public virtual int result
	{
		get { return m_nResult; }
	}

	public virtual string errorMessage
	{
		get { return m_sErrorMessage; }
	}

	public virtual bool isOK
	{
		get { return m_nResult == kResult_OK; }
	}

	protected virtual bool bodyHandlingRequired
	{
		get { return isOK; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	public virtual void Handle(string sResponse)
	{
		m_joContent = JsonMapper.ToObject(sResponse);
		//
		// Result.
		//

		m_nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		//
		// Error Message.
		//

		if (LitJsonUtil.Contains(m_joContent, "errMsg"))
		{
			m_sErrorMessage = LitJsonUtil.GetStringProperty(m_joContent, "errMsg");
		}

		//
		// Body.
		//
		if (bodyHandlingRequired || LitJsonUtil.Contains(m_joContent, "downloadUrl"))
		{
			HandleBody();
		}
	}

	protected virtual void HandleBody()
	{
	}

	public virtual string Trace()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("[" + GetType().Name + "]");

		sb.Append(Environment.NewLine);
		sb.Append("# Response.result: " + m_nResult);

		sb.Append(Environment.NewLine);
		sb.Append("# Response.errorMessage: " + (m_sErrorMessage == null ? "null" : m_sErrorMessage));

		sb.Append(Environment.NewLine);
		sb.Append("# Response.content : " + (m_joContent == null ? "null" : m_joContent.ToJson()));

		return sb.ToString();
	}
}
