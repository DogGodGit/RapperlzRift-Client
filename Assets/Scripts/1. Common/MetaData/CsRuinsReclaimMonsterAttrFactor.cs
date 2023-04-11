using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimMonsterAttrFactor
{
	int m_nLevel;
	float m_flMaxHpFactor;
	float m_flOffenseFactor;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public float MaxHpFactor
	{
		get { return m_flMaxHpFactor; }
	}

	public float OffenseFactor
	{
		get { return m_flOffenseFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimMonsterAttrFactor(WPDRuinsReclaimMonsterAttrFactor ruinsReclaimMonsterAttrFactor)
	{
		m_nLevel = ruinsReclaimMonsterAttrFactor.level;
		m_flMaxHpFactor = ruinsReclaimMonsterAttrFactor.maxHpFactor;
		m_flOffenseFactor = ruinsReclaimMonsterAttrFactor.offenseFactor;
	}
}
