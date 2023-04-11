using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnHelpshiftType
{
	FirstAccessNotice = 1,
	CS = 2,
}

public class CsHelpshiftManager 
{
	bool m_bDisplayNoticeFirstTime = true;

	public bool DisplayNoticeFirstTime
	{
		get { return m_bDisplayNoticeFirstTime; }
		set { m_bDisplayNoticeFirstTime = value; }
	}

	public static CsHelpshiftManager Instance
	{
		get { return CsSingleton<CsHelpshiftManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public void DisplayHelpshift(EnHelpshiftType enHelpshiftType)
	{
		if (!CsConfiguration.Instance.SystemSetting.HelpshiftSdkEnabled ||
			(enHelpshiftType == EnHelpshiftType.FirstAccessNotice && !m_bDisplayNoticeFirstTime))
		{
			return;
		}

		m_bDisplayNoticeFirstTime = false;

		if (CsConfiguration.Instance.SystemSetting.HelpshiftWebViewEnabled)
		{
			RequestHelpshiftByWebView(enHelpshiftType);
		}
		else
		{
			RequestHelpshift((int)enHelpshiftType);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RequestHelpshift(int nType)
	{
		HelpshiftNACommand cmd = new HelpshiftNACommand();
		cmd.VirtualGameServerId = CsConfiguration.Instance.GameServerSelected.VirtualGameServerId.ToString();
		cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
		cmd.ProcessType = nType;

		cmd.Run();
	}

	//---------------------------------------------------------------------------------------------------
	void RequestHelpshiftByWebView(EnHelpshiftType nType)
	{
		switch (nType)
		{
			case EnHelpshiftType.FirstAccessNotice:
				CsGameEventUIToUI.Instance.OnEventOpenWebView(CsConfiguration.Instance.SystemSetting.HelpshiftUrl);
				break;

			case EnHelpshiftType.CS:
				CsGameEventUIToUI.Instance.OnEventOpenWebView(CsConfiguration.Instance.SystemSetting.CsUrl);
				break;

			default:
				break;
		}
	}
}
