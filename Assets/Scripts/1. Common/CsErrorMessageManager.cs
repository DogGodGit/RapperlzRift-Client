using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsErrorMessageManager
{
	public static CsErrorMessageManager Instance
	{
		get { return CsSingleton<CsErrorMessageManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<string> EventAlert;

	//---------------------------------------------------------------------------------------------------
	public void OnEventAlert(string strMessage)
	{
		if (EventAlert != null)
		{
			EventAlert(strMessage);
		}
	}
}


