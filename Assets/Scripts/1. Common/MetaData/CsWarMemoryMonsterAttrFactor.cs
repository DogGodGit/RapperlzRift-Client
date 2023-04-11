using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryMonsterAttrFactor
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
	public CsWarMemoryMonsterAttrFactor(WPDWarMemoryMonsterAttrFactor warMemoryMonsterAttrFactor)
	{
		m_nLevel = warMemoryMonsterAttrFactor.level;
		m_flMaxHpFactor = warMemoryMonsterAttrFactor.maxHpFactor;
		m_flOffenseFactor = warMemoryMonsterAttrFactor.offenseFactor;
	}
}
