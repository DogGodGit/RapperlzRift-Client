using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroCreatureBaseAttr
{
	CsCreatureBaseAttr m_csCreatureBaseAttr;
	int m_nBaseValue;

	//---------------------------------------------------------------------------------------------------
	public CsCreatureBaseAttr CreatureBaseAttr
	{
		get { return m_csCreatureBaseAttr; }
	}

	public int BaseValue
	{
		get { return m_nBaseValue; }
		set { m_nBaseValue = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreatureBaseAttr(PDHeroCreatureBaseAttr heroCreatureBaseAttr)
	{
		m_csCreatureBaseAttr = CsGameData.Instance.GetCreatureBaseAttr(heroCreatureBaseAttr.attrId);
		m_nBaseValue = heroCreatureBaseAttr.baseValue;
	}
}
