using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicStepRoute
{
	int m_nStep;
	int m_nRouteId;
	float m_flTargetXPosition;
	float m_flTargetYPosition;
	float m_flTargetZPosition;
	float m_flTargetRadius;
	int m_nRemoveObstacleId;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public int RouteId
	{
		get { return m_nRouteId; }
	}

	public float TargetXPosition
	{
		get { return m_flTargetXPosition; }
	}

	public float TargetYPosition
	{
		get { return m_flTargetYPosition; }
	}

	public float TargetZPosition
	{
		get { return m_flTargetZPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public int RemoveObstacleId
	{
		get { return m_nRemoveObstacleId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicStepRoute(WPDAncientRelicStepRoute ancientRelicStepRoute)
	{
		m_nStep = ancientRelicStepRoute.step;
		m_nRouteId = ancientRelicStepRoute.routeId;
		m_flTargetXPosition = ancientRelicStepRoute.targetXPosition;
		m_flTargetYPosition = ancientRelicStepRoute.targetYPosition;
		m_flTargetZPosition = ancientRelicStepRoute.targetZPosition;
		m_flTargetRadius = ancientRelicStepRoute.targetRadius;
		m_nRemoveObstacleId = ancientRelicStepRoute.removeObstacleId;
	}
}
