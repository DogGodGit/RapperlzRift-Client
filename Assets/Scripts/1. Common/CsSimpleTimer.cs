using System;
using UnityEngine;

public class CsSimpleTimer
{
	float m_flTimeStart;
	float m_flTimeSpan;
	int m_nCount = 0;

	//---------------------------------------------------------------------------------------------------
	public float TimeStart
	{
		get { return m_flTimeStart; }
	}

	//---------------------------------------------------------------------------------------------------
	public float TimeSpan
	{
		get { return m_flTimeSpan; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleTimer()
	{

	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleTimer(float flTimeSpan)
	{
		Init(flTimeSpan);
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(float flTimeSpan)
	{
		m_flTimeSpan = flTimeSpan;
		m_flTimeStart = Time.time;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckTimer()
	{
		return (Time.time >= m_flTimeStart + m_flTimeSpan);
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetTimer()
	{
		m_flTimeStart = Time.time;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckSetTimer()
	{
		if (m_flTimeSpan == 0) return true;

		if (m_nCount == 0)
		{
			ResetTimer();
			m_nCount = 1;
		}
		else if (CheckTimer())
		{
			ResetTimer();
			return true;
		}
		return false;
	}
}
