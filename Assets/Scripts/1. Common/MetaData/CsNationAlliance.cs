using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-17)
//---------------------------------------------------------------------------------------------------

public class CsNationAlliance
{
	Guid m_guidId;
	int[] m_anNations;
	float m_flAllianceRenounceAvailableRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public int[] Nations
	{
		get { return m_anNations; }
	}

	public float AllianceRenounceAvailableRemainingTime
	{
		get { return m_flAllianceRenounceAvailableRemainingTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationAlliance(PDNationAlliance nationAlliance)
	{
		m_guidId = nationAlliance.id;
		m_anNations = nationAlliance.nationIds;
		m_flAllianceRenounceAvailableRemainingTime = nationAlliance.allianceRenounceAvailableRemainingTime + Time.realtimeSinceStartup;
	}
}
