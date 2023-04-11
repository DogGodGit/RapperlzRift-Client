using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-13)
//---------------------------------------------------------------------------------------------------

public class CsSupplySupportQuestWayPoint
{
	int m_nWayPoint;
	CsNpcInfo m_csNpcCartChange;

	//---------------------------------------------------------------------------------------------------
	public int WayPoint
	{
		get { return m_nWayPoint; }
	}

	public CsNpcInfo CartChangeNpc
	{
		get { return m_csNpcCartChange; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuestWayPoint(WPDSupplySupportQuestWayPoint supplySupportQuestWayPoint)
	{
		m_nWayPoint = supplySupportQuestWayPoint.wayPoint;
		m_csNpcCartChange = CsGameData.Instance.GetNpcInfo(supplySupportQuestWayPoint.cartChangeNpcId);
	}
}
