using WebCommon;
using System;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicTrap
{
	int m_nTrapId;
	Vector3 m_vtPosition;
	float m_flWidth;
	float m_flHeight;
	int m_nStartDelayTime;
	int m_nRegenInterval;
	int m_nDuration;

	//---------------------------------------------------------------------------------------------------
	public int TrapId
	{
		get { return m_nTrapId; }
	}

	public Vector3 Position
	{
		get { return m_vtPosition; }
	}

	public float Width
	{
		get { return m_flWidth; }
	}

	public float Height
	{
		get { return m_flHeight; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int RegenInterval
	{
		get { return m_nRegenInterval; }
	}

	public int Duration
	{
		get { return m_nDuration; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicTrap(WPDAncientRelicTrap ancientRelicTrap)
	{
		m_nTrapId = ancientRelicTrap.trapId;
		m_vtPosition = new Vector3(ancientRelicTrap.xPosition, ancientRelicTrap.yPosition, ancientRelicTrap.zPosition);
		m_flWidth = ancientRelicTrap.width;
		m_flHeight = ancientRelicTrap.height;
		m_nStartDelayTime = ancientRelicTrap.startDelayTime;
		m_nRegenInterval = ancientRelicTrap.regenInterval;
		m_nDuration = ancientRelicTrap.duration;
	}
}
