using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-11)
//---------------------------------------------------------------------------------------------------

public class CsSharingEventManager
{

	//---------------------------------------------------------------------------------------------------
	public static CsSharingEventManager Instance
	{
		get { return CsSingleton<CsSharingEventManager>.GetInstance(); }
	}

	#region Protocol.Native.Command
	//---------------------------------------------------------------------------------------------------
	public void RequestFirebaseDynamicLink()
	{
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.WITH_SDK)
		{
			CsSharingEvent csSharingEvent = CsGameData.Instance.GetCurrentSharingEvent();

			if (csSharingEvent != null)
			{
				FirebaseDynamicLinkNACommand cmd = new FirebaseDynamicLinkNACommand();
				cmd.EventId = csSharingEvent.EventId;
				cmd.VirtualGameServerId = CsConfiguration.Instance.GameServerSelected.VirtualGameServerId;
				cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();

				if (csSharingEvent.ContentType == 1)
				{
					cmd.Content = csSharingEvent.Content;
				}
				else if (csSharingEvent.ContentType == 2)
				{
					CsConfiguration.Instance.GetString(csSharingEvent.Content);
				}

				cmd.AppStoreId = CsConfiguration.Instance.SystemSetting.AppStoreId;
				cmd.AuthUrl = CsConfiguration.Instance.SystemSetting.AuthUrl;

				cmd.Finished += ResponseFirebaseDynamicLink;
				cmd.Run();
			}
			else
			{
				CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A166_TXT_00006"));
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert("유니티 에디터에서는 사용할 수 없습니다.");
		}
	}

	void ResponseFirebaseDynamicLink(object sender, EventArgs e)
	{
		FirebaseDynamicLinkNACommand cmd = (FirebaseDynamicLinkNACommand)sender;

		if (!cmd.isOK)
		{
			CsGameEventUIToUI.Instance.OnEventAlert("FirebaseDynamicLinkNACommand Error : " + cmd.error.Message);
			return;
		}

		FirebaseDynamicLinkNAResponse res = (FirebaseDynamicLinkNAResponse)cmd.response;

		if (!res.isOK)
		{
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A166_TXT_00007"));
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RequestFirebaseDynamicLinkReceive()
	{
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.WITH_SDK)
		{
			CsSharingEvent csSharingEvent = CsGameData.Instance.GetCurrentSharingEvent();

			if (csSharingEvent != null)
			{
				FirebaseDynamicLinkReceiveNACommand cmd = new FirebaseDynamicLinkReceiveNACommand();
				cmd.EventId = csSharingEvent.EventId;
				cmd.VirtualGameServerId = CsConfiguration.Instance.GameServerSelected.VirtualGameServerId;
				cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
				cmd.AuthUrl = CsConfiguration.Instance.SystemSetting.AuthUrl;
				cmd.Finished += ResponseFirebaseDynamicLinkReceive;
				cmd.Run();
			}
		}
	}

	void ResponseFirebaseDynamicLinkReceive(object sender, EventArgs e)
	{
		FirebaseDynamicLinkReceiveNACommand cmd = (FirebaseDynamicLinkReceiveNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log("FirebaseDynamicLinkReceiveNACommand Error : " + cmd.error.Message);
			return;
		}

		FirebaseDynamicLinkReceiveNAResponse res = (FirebaseDynamicLinkReceiveNAResponse)cmd.response;

		if (!res.isOK)
		{
			Debug.Log("FirebaseDynamicLinkReceiveNAResponse Error : " + res.errorMessage);
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RequestFirebaseDynamicLinkReward()
	{
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.WITH_SDK)
		{
			CsSharingEvent csSharingEvent = CsGameData.Instance.GetCurrentSharingEvent();

			if (csSharingEvent != null && csSharingEvent.TargetLevel <= CsGameData.Instance.MyHeroInfo.Level)
			{
				FirebaseDynamicLinkRewardNACommand cmd = new FirebaseDynamicLinkRewardNACommand();
				cmd.EventId = csSharingEvent.EventId;
				cmd.VirtualGameServerId = CsConfiguration.Instance.GameServerSelected.VirtualGameServerId;
				cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
				cmd.AuthUrl = CsConfiguration.Instance.SystemSetting.AuthUrl;
				cmd.Finished += ResponseFirebaseDynamicLinkReward;
				cmd.Run();
			}
		}
	}

	void ResponseFirebaseDynamicLinkReward(object sender, EventArgs e)
	{
		FirebaseDynamicLinkRewardNACommand cmd = (FirebaseDynamicLinkRewardNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log("FirebaseDynamicLinkRewardNACommand Error : " + cmd.error.Message);
			return;
		}

		FirebaseDynamicLinkRewardNAResponse res = (FirebaseDynamicLinkRewardNAResponse)cmd.response;

		if (!res.isOK)
		{
			Debug.Log("FirebaseDynamicLinkRewardNAResponse Error : " + res.errorMessage);
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------

	#endregion Protocol.Native.Command
}
