using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsRankAttr
{
	int m_nRankNo;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int RankNo
	{
		get { return m_nRankNo; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInfo
	{
		get { return m_csAttrValueInfo; }
	}

	public int BattlePowerValue
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankAttr(WPDRankAttr rankAttr)
	{
		m_nRankNo = rankAttr.rankNo;
		m_csAttr = CsGameData.Instance.GetAttr(rankAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(rankAttr.attrValueId);
	}
}
