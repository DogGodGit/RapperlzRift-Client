﻿using UnityEngine;
using System.Collections;

using LitJson;

public class HelpshiftNACommand : NativeApiCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public HelpshiftNACommand()
		: base("HelpShift")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	string m_strVirtualGameServerId = null;
	string m_strHeroId = null;
	int m_nProcessType = 0;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string VirtualGameServerId
	{
		get { return m_strVirtualGameServerId; }
		set { m_strVirtualGameServerId = value; }
	}

	public string HeroId
	{
		get { return m_strHeroId; }
		set { m_strHeroId = value; }
	}

	public int ProcessType
	{
		get { return m_nProcessType; }
		set { m_nProcessType = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override NativeApiResponse CreateResponse()
	{
		return new HelpshiftNAResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["virtualGameServerId"] = m_strVirtualGameServerId;
		joReq["heroId"] = m_strHeroId;
		joReq["processType"] = m_nProcessType;

		return joReq;
	}
}

public class HelpshiftNAResponse : NativeApiResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();
	}
}