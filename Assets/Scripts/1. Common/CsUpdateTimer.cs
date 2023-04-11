using UnityEngine;
using System.Collections;

public class CsUpdateTimer
{
	public enum EN_CHK_RST {PLAY, STOP, FAIL}

	public float m_flTimeSpan;
	float m_flElapsedTime = 0;

	public CsUpdateTimer(float flTimeSpan, bool bStart = true)
	{
		m_flTimeSpan = flTimeSpan;
		if (!bStart)
		{
			m_flElapsedTime = -1;
		}
	}

	public bool IsActive()
	{
		return m_flElapsedTime >= 0;
	}

	public float GetRemainderTime()
	{
		if (m_flElapsedTime < 0) return 0;
		return m_flTimeSpan - m_flElapsedTime;
	}

	public float GetElapsedTime()
	{
		if (m_flElapsedTime < 0) return m_flTimeSpan;
		return m_flElapsedTime;
	}

	public float GetRemainderTimeRate()
	{
		return GetRemainderTime() / m_flTimeSpan;
	}

	public float GetElapsedTimeRate()
	{
		return GetElapsedTime() / m_flTimeSpan;
	}

	public void Init(float flTimeSpan, bool bStart = true)
	{
		m_flTimeSpan = flTimeSpan;
		if (bStart)
		{
			m_flElapsedTime = 0;
		}
		else
		{
			m_flElapsedTime = -1;
		}
	}

	public void Stop()
	{
		m_flElapsedTime = -1;
	}

	public void SetElapsedTime(float flElapsedTime)
	{
		m_flElapsedTime = flElapsedTime;
	}

	public void Reset()
	{
		m_flElapsedTime = 0;
	}

	public bool Check(float flDeltaTime)
	{
		if (m_flElapsedTime < 0) return false;

		m_flElapsedTime += flDeltaTime;
		if (m_flElapsedTime >= m_flTimeSpan)
		{
			m_flElapsedTime = -1;
			return true;
		}

		return false;
	}

	public EN_CHK_RST CheckEX(float flDeltaTime)
	{
		if (m_flElapsedTime < 0) return EN_CHK_RST.FAIL;

		m_flElapsedTime += flDeltaTime;
		if (m_flElapsedTime >= m_flTimeSpan)
		{
			m_flElapsedTime = -1;
			return EN_CHK_RST.STOP;
		}

		return EN_CHK_RST.PLAY;
	}

}
