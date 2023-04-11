using UnityEngine;
using System;
using System.Collections;
using System.Text;

using LitJson;
using NativeService;

public abstract class NativeApiCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	//
	// 상태 코드.
	//

	public const int kStatus_None		= 0;
	public const int kStatus_Running	= 1;
	public const int kStatus_Finished	= 2;

	//
	// 결과 코드.
	//

	public const int kResult_OK			= 0;
	public const int kResult_Error		= 1;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Events

	public event EventHandler	Finished = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	protected string	m_sCommand = null;

	protected int		m_nStatus = kStatus_None;

	protected int					m_nResult = kResult_OK;
	protected NativeApiResponse		m_response = null;
	protected Exception				m_error = null;

	//
	// 사용자 정의 데이터.
	//

	protected object	m_stateData = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public NativeApiCommand(string sCommand)
	{
		m_sCommand = sCommand;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public virtual string command
	{
		get { return m_sCommand; }
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

	public virtual NativeApiResponse response
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

	protected virtual void Finish(int nResult, NativeApiResponse response, Exception error)
	{
		if (m_nStatus != kStatus_Running)
			return;

		m_nResult = nResult;
		m_response = response;
		m_error = error;

		m_nStatus = kStatus_Finished;

		if (Finished != null)
			Finished(this, EventArgs.Empty);
	}

	protected abstract NativeApiResponse CreateResponse();

	protected virtual JsonData MakeRequestContent()
	{
		JsonData joReq = LitJsonUtil.CreateObject();
		joReq["cmd"] = m_sCommand;

		return joReq;
	}

	public virtual void Run()
	{
		if (m_nStatus != kStatus_None)
			throw new InvalidOperationException("이미 실행되었습니다.");

		JsonData joContent = MakeRequestContent();
		string sContent = joContent == null ? null : joContent.ToJson();

		NSApiClient.instance.Request(sContent, OnResponse);

		m_nStatus = kStatus_Running;
	}

	protected virtual void OnResponse(string sContent)
	{
		//
		// 응답 생성 및 처리.
		//

		int nResult = kResult_OK;
		NativeApiResponse response = null;
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

public abstract class NativeApiResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	public const int kResult_OK		= 0;
	public const int kResult_Error	= 1;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	protected JsonData	m_joContent = null;

	protected int		m_nResult = kResult_OK;
	protected string	m_sErrorMessage = null;

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
			m_sErrorMessage = LitJsonUtil.GetStringProperty(m_joContent, "errMsg");

		//
		// Body.
		//

		if (bodyHandlingRequired)
			HandleBody();
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
