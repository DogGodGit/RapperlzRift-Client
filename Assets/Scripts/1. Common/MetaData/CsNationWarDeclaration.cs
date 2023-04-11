using System;
using ClientCommon;

public enum EnNationWarDeclaration
{
	Before = 0,
	Current,
	End
}

public class CsNationWarDeclaration
{
	Guid m_guidDeclarationId;

	int m_nNationId;
	int m_nTargetNationId;

	DateTimeOffset m_dtOffsetTime;
	int m_nStatus;

	//---------------------------------------------------------------------------------------------------
	public Guid DeclarationId
    {
		get { return m_guidDeclarationId; }
    }

	public int NationId
    {
		get { return m_nNationId; }
    }

	public int TargetNationId
    {
		get { return m_nTargetNationId; }
    }

	public DateTimeOffset Time
    {
		get { return m_dtOffsetTime; }
    }

//	public int Status
//    {
//		get { return m_nStatus; }
//    }

	public EnNationWarDeclaration Status
	{
		get { return (EnNationWarDeclaration)m_nStatus; }
        set { m_nStatus = (int)value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarDeclaration(PDNationWarDeclaration pDNationWarDeclaration)
	{
		m_guidDeclarationId = pDNationWarDeclaration.declarationId;
		m_nNationId = pDNationWarDeclaration.nationId;
		m_nTargetNationId = pDNationWarDeclaration.targetNationId;
		m_dtOffsetTime = pDNationWarDeclaration.time;
		m_nStatus = pDNationWarDeclaration.status;
	}
}
