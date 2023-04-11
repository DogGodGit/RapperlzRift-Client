using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsAbnormalStateRankSkillLevel
{
	int m_nAbnormalStateId;
	int m_nSkillLevel;
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

	public int SkillLevel
	{
		get { return m_nSkillLevel; }
	}

	public int Duration
	{
		get { return m_nAbnormalStateId; }
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
	public CsAbnormalStateRankSkillLevel(WPDAbnormalStateRankSkillLevel abnormalStateRankSkillLevel)
	{
		m_nAbnormalStateId = abnormalStateRankSkillLevel.abnormalStateId;
		m_nSkillLevel = abnormalStateRankSkillLevel.skillLevel;
		m_nDuration = abnormalStateRankSkillLevel.duration;
		m_nValue1 = abnormalStateRankSkillLevel.value1;
		m_nValue2 = abnormalStateRankSkillLevel.value2;
		m_nValue3 = abnormalStateRankSkillLevel.value3;
		m_nValue4 = abnormalStateRankSkillLevel.value4;
		m_nValue5 = abnormalStateRankSkillLevel.value5;
		m_nValue6 = abnormalStateRankSkillLevel.value6;
	}
}
