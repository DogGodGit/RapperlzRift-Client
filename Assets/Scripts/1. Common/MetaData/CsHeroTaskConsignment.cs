using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroTaskConsignment
{
	Guid m_guidInstanceId;
	int m_nConsignmentId;
	int m_nUsedExpItemId;
	float m_flRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public Guid InstanceId
	{
		get { return m_guidInstanceId; }
	}

	public int ConsignmentId
	{
		get { return m_nConsignmentId; }
	}

	public int UsedExpItemId
	{
		get { return m_nUsedExpItemId; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTaskConsignment(PDHeroTaskConsignment heroTaskConsignment)
	{
		m_guidInstanceId = heroTaskConsignment.instanceId;
		m_nConsignmentId = heroTaskConsignment.consignmentId;
		m_nUsedExpItemId = heroTaskConsignment.usedExpItemId;
		m_flRemainingTime = heroTaskConsignment.remainingTime + Time.realtimeSinceStartup;
	}
}
