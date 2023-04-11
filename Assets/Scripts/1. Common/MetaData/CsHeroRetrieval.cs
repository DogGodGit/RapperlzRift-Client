using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-19)
//---------------------------------------------------------------------------------------------------

public class CsHeroRetrieval
{
	int m_nRetrievalId;
	int m_nCount;

	//---------------------------------------------------------------------------------------------------
	public int RetrievalId
	{
		get { return m_nRetrievalId; }
	}

	public int Count
	{
		get { return m_nCount; }
		set { m_nCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRetrieval(PDHeroRetrieval heroRetrieval)
	{
		m_nRetrievalId = heroRetrieval.retrievalId;
		m_nCount = heroRetrieval.count;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroRetrieval(int nRetrievalId, int nCount)
	{
		m_nRetrievalId = nRetrievalId;
		m_nCount = nCount;
	}
}
