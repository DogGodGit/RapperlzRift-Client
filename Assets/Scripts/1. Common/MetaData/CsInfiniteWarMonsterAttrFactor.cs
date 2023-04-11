using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsInfiniteWarMonsterAttrFactor
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
	public CsInfiniteWarMonsterAttrFactor(WPDInfiniteWarMonsterAttrFactor infiniteWarMonsterAttrFactor)
	{
		m_nLevel = infiniteWarMonsterAttrFactor.level;
		m_flMaxHpFactor = infiniteWarMonsterAttrFactor.maxHpFactor;
		m_flOffenseFactor = infiniteWarMonsterAttrFactor.offenseFactor;
	}
}
