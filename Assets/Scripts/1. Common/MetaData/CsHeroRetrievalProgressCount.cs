using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-19)
//---------------------------------------------------------------------------------------------------

public class CsHeroRetrievalProgressCount
{
	DateTime m_dtDate;
	int m_nRetrievalId;
	int m_nProgressCount;

	//---------------------------------------------------------------------------------------------------
	public DateTime Date
	{
		get { return m_dtDate; }
	}

	public int RetrievalId
	{
		get { return m_nRetrievalId; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRetrievalProgressCount(PDHeroRetrievalProgressCount heroRetrievalProgressCount)
	{
		m_dtDate = heroRetrievalProgressCount.date;
		m_nRetrievalId = heroRetrievalProgressCount.retrievalId;
		m_nProgressCount = heroRetrievalProgressCount.prorgressCount;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRetrievalProgressCount(DateTime dtDate, int nRetreivalId)
	{
		m_dtDate = dtDate;
		m_nRetrievalId = nRetreivalId;
		m_nProgressCount = 0;
	}
}
