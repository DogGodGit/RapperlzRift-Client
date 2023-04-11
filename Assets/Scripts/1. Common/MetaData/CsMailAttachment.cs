using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-30)
//---------------------------------------------------------------------------------------------------

public class CsMailAttachment
{
	int m_nNo;
	CsItem m_csItem;
	int m_nCount;
	bool m_bOwned;

	//---------------------------------------------------------------------------------------------------
	public int No
	{
		get { return m_nNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public int Count
	{
		get { return m_nCount; }
	}

	public bool Owned
	{
		get { return m_bOwned; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMailAttachment(PDMailAttachment mailAttachment)
	{
		m_nNo = mailAttachment.no;
		m_csItem = CsGameData.Instance.GetItem(mailAttachment.itemId);
		m_nCount = mailAttachment.itemCount;
		m_bOwned = mailAttachment.itemOwned;
	}
}
