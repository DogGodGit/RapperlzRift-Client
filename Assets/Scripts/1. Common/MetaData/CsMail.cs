using System;
using System.Collections.Generic;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsMail
{
	Guid m_guid;					// 메일ID
	int m_nTitleType;               // 제목타입 (1 : 일반텍스트, 2 : 스트링Key)
	string m_strTitle;              // 제목	
	int m_nContentType;             // 내용타입 (1 : 일반텍스트, 2 : 스트링Key)
	string m_strContent;            // 내용	
	float m_flRemaingTime;          // 남은시간
	bool m_bReceived;
	
	List<CsMailAttachment> m_listCsMailAttachment;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guid; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string Content
	{
		get { return m_strContent; }
	}

	public float RemaingTime
	{
		get
		{
			float flflRemaingTime = m_flRemaingTime - Time.realtimeSinceStartup;

			if (flflRemaingTime > 0)
				return flflRemaingTime;
			else
				return 0;
		}
	}

	public bool Received
	{
		get { return m_bReceived; }
		set { m_bReceived = value; }
	}

	public List<CsMailAttachment> MailAttachmentList
	{
		get { return m_listCsMailAttachment; }
	}

	enum EnStringType
	{
		Text = 1,
		KeyString = 2,
	}

	//---------------------------------------------------------------------------------------------------
	public CsMail(PDMail mail)
	{
		m_guid = mail.id;
		m_nTitleType = mail.titleType;

		if ((EnStringType)m_nTitleType == EnStringType.Text)
			m_strTitle = mail.title;
		else
			m_strTitle = CsConfiguration.Instance.GetString(mail.title);


		m_nContentType = mail.contentType;

		if ((EnStringType)m_nContentType == EnStringType.Text)
			m_strContent = mail.content;
		else
			m_strContent = CsConfiguration.Instance.GetString(mail.content);

		m_flRemaingTime = mail.remainingTime + Time.realtimeSinceStartup;

		m_bReceived = mail.received;

		m_listCsMailAttachment = new List<CsMailAttachment>();

		for (int i = 0; i < mail.attachments.Length; i++)
		{
			m_listCsMailAttachment.Add(new CsMailAttachment(mail.attachments[i]));
		}
	}
}
