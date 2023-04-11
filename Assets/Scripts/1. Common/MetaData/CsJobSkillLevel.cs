using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsJobSkillLevel
{
	int m_nJobId;											// 직업ID
	int m_nSkillId;											// 스킬ID
	int m_nLevel;											// 스킬레벨
	long m_lBattlePower;									// 전투력
	string m_strSummary;									// 효과설명
	CsAttrValueInfo m_csAttrValueInfoPhysicalOffenseAmp;	// 물리공격력증폭속성값
	CsAttrValueInfo m_csAttrValueInfoMagicalOffenseAmp;     // 마법공격력증폭속성값
	CsAttrValueInfo m_csAttrValueInfoOffensePoint;          // 공격포인트속성값

	//---------------------------------------------------------------------------------------------------
	public int JobId
	{
		get { return m_nJobId; }
	}

	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public long BattlePower
	{
		get { return m_lBattlePower; }
	}

	public string Summary
	{
		get { return m_strSummary; }
	}

	public int PhysicalOffenseAmp
	{
		get { return m_csAttrValueInfoPhysicalOffenseAmp.Value; }
	}

	public int MagicalOffenseAmp
	{
		get { return m_csAttrValueInfoMagicalOffenseAmp.Value; }
	}

	public int OffensePoint
	{
		get { return m_csAttrValueInfoOffensePoint.Value; }
	}


	//---------------------------------------------------------------------------------------------------
	public CsJobSkillLevel(WPDJobSkillLevel jobSkillLevel)
	{
		m_nJobId = jobSkillLevel.jobId;
		m_nSkillId = jobSkillLevel.skillId;
		m_nLevel = jobSkillLevel.level;
		m_lBattlePower = jobSkillLevel.battlePower;
		m_strSummary = CsConfiguration.Instance.GetString(jobSkillLevel.summaryKey);

		m_csAttrValueInfoPhysicalOffenseAmp = CsGameData.Instance.GetAttrValueInfo(jobSkillLevel.physicalOffenseAmpAttrValueId);
		m_csAttrValueInfoMagicalOffenseAmp = CsGameData.Instance.GetAttrValueInfo(jobSkillLevel.magicalOffenseAmpAttrValueId);
		m_csAttrValueInfoOffensePoint = CsGameData.Instance.GetAttrValueInfo(jobSkillLevel.offensePointAttrValueId);
	}
}
