using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSharingEvent
{
	int m_nEventId;
	int m_nContentType;
	string m_strContent;
	int m_nRewardRange;
	int m_nSenderRewardLimitCount;
	int m_nTargetLevel;
	DateTimeOffset m_dtoStartTime;
	DateTimeOffset m_dtoEndTime;
	string m_strImageName;
	string m_strDescription1;
	string m_strDescription2;

	List<CsSharingEventSenderReward> m_listCsSharingEventSenderReward;
	List<CsSharingEventReceiverReward> m_listCsSharingEventReceiverReward;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int ContentType
	{
		get { return m_nContentType; }
	}

	public string Content
	{
		get { return m_strContent; }
	}

	public int RewardRange
	{
		get { return m_nRewardRange; }
	}

	public int SenderRewardLimitCount
	{
		get { return m_nSenderRewardLimitCount; }
	}

	public int TargetLevel
	{
		get { return m_nTargetLevel; }
	}

	public DateTimeOffset StartTime
	{
		get { return m_dtoStartTime; }
	}

	public DateTimeOffset EndTime
	{
		get { return m_dtoEndTime; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public string Description1
	{
		get { return m_strDescription1; }
	}

	public string Description2
	{
		get { return m_strDescription2; }
	}

	public List<CsSharingEventSenderReward> SharingEventSenderRewardList
	{
		get { return m_listCsSharingEventSenderReward; }
	}

	public List<CsSharingEventReceiverReward> SharingEventReceiverRewardList
	{
		get { return m_listCsSharingEventReceiverReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSharingEvent(WPDSharingEvent sharingEvent)
	{
		m_nEventId = sharingEvent.eventId;
		m_nContentType = sharingEvent.contentType;
		m_strContent = sharingEvent.content;
		m_nRewardRange = sharingEvent.rewardRange;
		m_nSenderRewardLimitCount = sharingEvent.senderRewardLimitCount;
		m_nTargetLevel = sharingEvent.targetLevel;
		m_dtoStartTime = sharingEvent.startTime;
		m_dtoEndTime = sharingEvent.endTime;
		m_strImageName = sharingEvent.imageName;
		m_strDescription1 = CsConfiguration.Instance.GetString(sharingEvent.descriptionKey1);
		m_strDescription2 = CsConfiguration.Instance.GetString(sharingEvent.descriptionKey2);

		m_listCsSharingEventSenderReward = new List<CsSharingEventSenderReward>();
		m_listCsSharingEventReceiverReward = new List<CsSharingEventReceiverReward>();
	}
}
