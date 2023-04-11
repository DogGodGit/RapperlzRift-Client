using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarRevivalPointActivationCondition
{
	CsNationWarRevivalPoint m_csRevivalPoint;
	int m_nArrangeId;

	//---------------------------------------------------------------------------------------------------
	public CsNationWarRevivalPoint RevivalPoint
	{
		get { return m_csRevivalPoint; }
	}

	public int ArrangeId
	{
		get { return m_nArrangeId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarRevivalPointActivationCondition(WPDNationWarRevivalPointActivationCondition nationWarRevivalPointActivationCondition)
	{
		m_csRevivalPoint = CsGameData.Instance.GetNationWarRevivalPoint(nationWarRevivalPointActivationCondition.revivalPointId);
		m_nArrangeId = nationWarRevivalPointActivationCondition.arrangeId;
	}
}
