using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-17)
//---------------------------------------------------------------------------------------------------

public class CsNationAllianceApplication
{
	Guid m_guidId;
	int m_nNationId;
	int m_nTargetNationId;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public int NationId
	{
		get { return m_nNationId; }
	}

	public int TargetNationId
	{
		get { return m_nTargetNationId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationAllianceApplication(PDNationAllianceApplication nationAllianceApplication)
	{
		m_guidId = nationAllianceApplication.id;
		m_nNationId = nationAllianceApplication.nationId;
		m_nTargetNationId = nationAllianceApplication.targetNationId;
	}
}
