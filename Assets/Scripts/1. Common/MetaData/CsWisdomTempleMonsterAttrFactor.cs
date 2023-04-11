using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleMonsterAttrFactor
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
	public CsWisdomTempleMonsterAttrFactor(WPDWisdomTempleMonsterAttrFactor wisdomTempleMonsterAttrFactor)
	{
		m_nLevel = wisdomTempleMonsterAttrFactor.level;
		m_flMaxHpFactor = wisdomTempleMonsterAttrFactor.maxHpFactor;
		m_flOffenseFactor = wisdomTempleMonsterAttrFactor.offenseFactor;
	}
}
