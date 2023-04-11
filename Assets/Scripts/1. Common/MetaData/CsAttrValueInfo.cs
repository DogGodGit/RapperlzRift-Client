using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsAttrValueInfo
{
	long m_lAttrValueId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long AttrValueId
	{
		get { return m_lAttrValueId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}


	//---------------------------------------------------------------------------------------------------
	public CsAttrValueInfo(WPDAttrValue attrValue)
	{
		m_lAttrValueId = attrValue.attrValueId;
		m_nValue = attrValue.value;
	}
	
}
