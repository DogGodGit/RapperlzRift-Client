using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsGuildContent : IComparable
{
	/*
	1 : 길드미션
	2 : 길드헌팅
	3 : 길드용광로
	4 : 길드신수
	5 : 길드과일나무
	6 : 길드헌팅기부
	7 : 길드물자지원퀘스트
	*/
	int m_nGuildContentId;
	string m_strName;
	string m_strDescription;
	string m_strRewardText;
	string m_strEventTimeText;
	string m_strLockText;
	int m_nAchievementPoint;
	bool m_bIsDailyObjective;
	int m_nSortNo;
	int m_nTaskId;

	List<CsGuildContentAvailableReward> m_listCsGuildContentAvailableReward;

	//---------------------------------------------------------------------------------------------------
	public int GuildContentId
	{
		get { return m_nGuildContentId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string RewardText
	{
		get { return m_strRewardText; }
	}

	public string EventTimeText
	{
		get { return m_strEventTimeText; }
	}

	public string LockText
	{
		get { return m_strLockText; }
	}

	public int AchievementPoint
	{
		get { return m_nAchievementPoint; }
	}

	public bool IsDailyObjective
	{
		get { return m_bIsDailyObjective; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public int TaskId
	{
		get { return m_nTaskId; }
	}

	public List<CsGuildContentAvailableReward> GuildContentAvailableRewardList
	{
		get { return m_listCsGuildContentAvailableReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildContent(WPDGuildContent guildContent)
	{
		m_nGuildContentId = guildContent.guildContentId;
		m_strName = CsConfiguration.Instance.GetString(guildContent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(guildContent.descriptionKey);
		m_strRewardText = CsConfiguration.Instance.GetString(guildContent.rewardTextKey);
		m_strEventTimeText = CsConfiguration.Instance.GetString(guildContent.eventTimeTextKey);
		m_strLockText = CsConfiguration.Instance.GetString(guildContent.lockTextKey);
		m_nAchievementPoint = guildContent.achievementPoint;
		m_bIsDailyObjective = guildContent.isDailyObjective;
		m_nSortNo = guildContent.sortNo;
		m_nTaskId = guildContent.taskId;

		m_listCsGuildContentAvailableReward = new List<CsGuildContentAvailableReward>();
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsGuildContent)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
