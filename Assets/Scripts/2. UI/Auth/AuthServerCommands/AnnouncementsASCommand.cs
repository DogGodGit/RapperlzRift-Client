using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class AnnouncementsASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public AnnouncementsASCommand() 
		: base("Announcements")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions
	protected override AuthServerResponse CreateResponse()
	{
		return new AnnouncementsASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		return joReq;
	}
}

public class AnnouncementsASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	List<CsAnnouncement> m_listCsAnnouncement;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public List<CsAnnouncement> AnnouncementList
	{
		get { return m_listCsAnnouncement; }
	}

	public AnnouncementsASResponse()
	{
		m_listCsAnnouncement = new List<CsAnnouncement>();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions
	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			JsonData jsonDataAnnouncements = LitJsonUtil.GetArrayProperty(m_joContent, "announcements");

			for (int i = 0; i < jsonDataAnnouncements.Count; i++)
			{
				m_listCsAnnouncement.Add(new CsAnnouncement(new Guid(LitJsonUtil.GetStringProperty(jsonDataAnnouncements[i], "announcementId")),
															LitJsonUtil.GetStringProperty(jsonDataAnnouncements[i], "title"),
															LitJsonUtil.GetStringProperty(jsonDataAnnouncements[i], "contentUrl"),
															DateTimeOffset.Parse(LitJsonUtil.GetStringProperty(jsonDataAnnouncements[i], "startTime")),
															DateTimeOffset.Parse(LitJsonUtil.GetStringProperty(jsonDataAnnouncements[i], "endTime")),
															LitJsonUtil.GetIntProperty(jsonDataAnnouncements[i], "sortNo")));
			}
		}
	}
}
