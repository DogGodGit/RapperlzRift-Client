using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsJobLevel
{
	int m_nJobId;
	CsJobLevelMaster m_csLevelMaster;

	CsAttrValueInfo m_csAttrValueInfoMaxHp;
	CsAttrValueInfo m_csAttrValueInfoPhysicalOffense;
	CsAttrValueInfo m_csAttrValueInfoMagicalOffense;
	CsAttrValueInfo m_csAttrValueInfoPhysicalDefense;
	CsAttrValueInfo m_csAttrValueInfoMagicalDefense;

	//---------------------------------------------------------------------------------------------------
	public int JobId
	{
		get { return m_nJobId; }
	}

	public int Level
	{
		get { return m_csLevelMaster.Level; }
	}

	public long NextLevelUpExp
	{
		get { return m_csLevelMaster.NextLevelUpExp; }
	}

	public int InventorySlotAccCount
	{
		get { return m_csLevelMaster.InventorySlotAccCount; }
	}

	public int MaxHp
	{
		get { return m_csAttrValueInfoMaxHp.Value; }
	}

	public int PhysicalOffense
	{
		get { return m_csAttrValueInfoPhysicalOffense.Value; }
	}

	public int MagicalOffense
	{
		get { return m_csAttrValueInfoMagicalOffense.Value; }
	}

	public int PhysicalDefense
	{
		get { return m_csAttrValueInfoPhysicalDefense.Value; }
	}

	public int MagicalDefense
	{
		get { return m_csAttrValueInfoMagicalDefense.Value; }
	}

	public CsJobLevelMaster LevelMaster
	{
		get { return m_csLevelMaster; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobLevel(WPDJobLevel jobLevel)
	{
		m_nJobId = jobLevel.jobId;
		m_csLevelMaster = CsGameData.Instance.GetJobLevelMaster(jobLevel.level);
		m_csAttrValueInfoMaxHp = CsGameData.Instance.GetAttrValueInfo(jobLevel.maxHpAttrValueId);
		m_csAttrValueInfoPhysicalOffense = CsGameData.Instance.GetAttrValueInfo(jobLevel.physicalOffenseAttrValueId);
		m_csAttrValueInfoMagicalOffense = CsGameData.Instance.GetAttrValueInfo(jobLevel.magicalOffenseAttrValueId);
		m_csAttrValueInfoPhysicalDefense = CsGameData.Instance.GetAttrValueInfo(jobLevel.physicalDefenseAttrValueId);
		m_csAttrValueInfoMagicalDefense = CsGameData.Instance.GetAttrValueInfo(jobLevel.magicalDefenseAttrValueId);
	}
}
