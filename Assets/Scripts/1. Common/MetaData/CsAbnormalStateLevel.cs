using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsAbnormalStateLevel
{
	int m_nAbnormalStateId;
	int m_nJobId;
	int m_nLevel;
	int m_nDuration;
	int m_nValue1;
	int m_nValue2;
	int m_nValue3;
	int m_nValue4;
	int m_nValue5;
	int m_nValue6;

	//---------------------------------------------------------------------------------------------------
	public int AbnormalStateId
	{
		get { return m_nAbnormalStateId; }
	}

	public int JobId
	{
		get { return m_nJobId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int Duration
	{
		get { return m_nDuration; }
	}

	public int Value1
	{
		get { return m_nValue1; }
	}

	public int Value2
	{
		get { return m_nValue2; }
	}

	public int Value3
	{
		get { return m_nValue3; }
	}

	public int Value4
	{
		get { return m_nValue4; }
	}

	public int Value5
	{
		get { return m_nValue5; }
	}

	public int Value6
	{
		get { return m_nValue6; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAbnormalStateLevel(WPDAbnormalStateLevel abnormalStateLevel)
	{
		m_nAbnormalStateId = abnormalStateLevel.abnormalStateId;
		m_nJobId = abnormalStateLevel.jobId;
		m_nLevel = abnormalStateLevel.level;
		m_nDuration = abnormalStateLevel.duration;
		m_nValue1 = abnormalStateLevel.value1;
		m_nValue2 = abnormalStateLevel.value2;
		m_nValue3 = abnormalStateLevel.value3;
		m_nValue4 = abnormalStateLevel.value4;
		m_nValue5 = abnormalStateLevel.value5;
		m_nValue6 = abnormalStateLevel.value6;
	}
}
