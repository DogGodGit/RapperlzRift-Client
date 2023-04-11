using System;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-31)
//---------------------------------------------------------------------------------------------------

public class CsAnnouncement : IComparable
{
	Guid m_guidId;
	string m_strTitle;
	string m_strContentUrl;
	DateTimeOffset m_dtoStartTime;
	DateTimeOffset m_dtoEndTime;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string ContentUrl
	{
		get { return m_strContentUrl; }
	}

	public DateTimeOffset StartTime
	{
		get { return m_dtoStartTime; }
	}

	public DateTimeOffset EndTime
	{
		get { return m_dtoEndTime; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAnnouncement(Guid guidId, string strTitle, string strContentUrl, DateTimeOffset dtoStartTime, DateTimeOffset dtoEndTime, int nSortNo)
	{
		m_guidId = guidId;
		m_strTitle = strTitle;
		m_strContentUrl = strContentUrl;
		m_dtoStartTime = dtoStartTime;
		m_dtoEndTime = dtoEndTime;
		m_nSortNo = nSortNo;

		Debug.Log(guidId.ToString() + ":" + m_strTitle + ":" + m_strContentUrl + ":" + m_dtoStartTime.ToString() + ":" + m_dtoEndTime.ToString() + ":" + m_nSortNo);
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsAnnouncement)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
