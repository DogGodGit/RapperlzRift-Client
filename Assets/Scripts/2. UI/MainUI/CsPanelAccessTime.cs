using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsPanelAccessTime : CsUpdateableMonoBehaviour {

	int m_nLastDisplayedId = 0;
	const float m_nDisplayDuration = 3.0f;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize() 
	{
		foreach (CsAccessRewardEntry csAccessRewardEntry in CsGameData.Instance.AccessRewardEntryList)
		{
			if (csAccessRewardEntry.AccessTime <= CsGameData.Instance.MyHeroInfo.DailyAccessTime  &&
				m_nLastDisplayedId < csAccessRewardEntry.EntryId)
			{
				m_nLastDisplayedId = csAccessRewardEntry.EntryId;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnUpdate(float flTime)
	{
		CheckAccessTime();
	}

	//---------------------------------------------------------------------------------------------------
	void CheckAccessTime()
	{
		foreach (CsAccessRewardEntry csAccessRewardEntry in CsGameData.Instance.AccessRewardEntryList)
		{
			if (csAccessRewardEntry.AccessTime <= CsGameData.Instance.MyHeroInfo.DailyAccessTime &&
				m_nLastDisplayedId < csAccessRewardEntry.EntryId)
			{
				m_nLastDisplayedId = csAccessRewardEntry.EntryId;

				if (CsUIData.Instance.MenuOpen(CsGameData.Instance.GetMenu((int)EnMenuId.Support)) &&
					CsUIData.Instance.MenuContentOpen(CsGameData.Instance.GetMenuContent((int)EnMenuContentId.AccessReward)))
				{
					StartCoroutine(DisplayAccessTime(csAccessRewardEntry));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DisplayAccessTime(CsAccessRewardEntry csAccessRewardEntry)
	{
		Transform trFrameAccessTime = transform.Find("FrameAccessTime");

		CsRookieGift csRookieGift = CsGameData.Instance.GetRookieGift(CsGameData.Instance.MyHeroInfo.RookieGiftNo);

		if (csRookieGift != null)
		{
			CsUIData.Instance.SetText(trFrameAccessTime.Find("TextTitle"), string.Format(CsConfiguration.Instance.GetString("A165_TXT_00002"), csAccessRewardEntry.AccessTime / 60), false);
			CsUIData.Instance.SetText(trFrameAccessTime.Find("TextDescription"), string.Format(CsConfiguration.Instance.GetString("A165_TXT_00003"), csAccessRewardEntry.AccessTime / 60), false);

			trFrameAccessTime.gameObject.SetActive(true);
		}

		yield return new WaitForSeconds(m_nDisplayDuration);

		trFrameAccessTime.gameObject.SetActive(false);
	}
}
