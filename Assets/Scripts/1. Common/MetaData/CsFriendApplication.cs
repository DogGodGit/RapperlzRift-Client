using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsFriendApplication
{
	long m_lNo;
	Guid m_guidApplicationId;
	string m_strApplicationName;
	CsNation m_csNationApplication;
	Guid m_guidTargetId;
	string m_strTargetName;
	CsNation m_csNationTarget;

	//---------------------------------------------------------------------------------------------------
	public long No
	{
		get { return m_lNo; }
	}

	public Guid ApplicationId
	{
		get { return m_guidApplicationId; }
	}

	public string ApplicationName
	{
		get { return m_strApplicationName; }
	}

	public CsNation ApplicationNation
	{
		get { return m_csNationApplication; }
	}

	public Guid TargetId
	{
		get { return m_guidTargetId; }
	}

	public string TargetName
	{
		get { return m_strTargetName; }
	}
	
	public CsNation TargetNation
	{
		get { return m_csNationTarget; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFriendApplication(PDFriendApplication friendApplication)
	{
		m_lNo = friendApplication.no;
		m_guidApplicationId = friendApplication.applicantId;
		m_strApplicationName = friendApplication.applicantName;
		m_csNationApplication = CsGameData.Instance.GetNation(friendApplication.applicantNationId);
		m_guidTargetId = friendApplication.targetId;
		m_strTargetName = friendApplication.targetName;
		m_csNationTarget = CsGameData.Instance.GetNation(friendApplication.targetNationId);
	}
}
