using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-09)
//---------------------------------------------------------------------------------------------------

public class CsHeroWingMemoryPieceSlot
{
	int m_nIndex;
	int m_nAccAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public int AccAttrValue
	{
		get { return m_nAccAttrValue; }
		set { m_nAccAttrValue = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWingMemoryPieceSlot(PDHeroWingMemoryPieceSlot heroWingMemoryPieceSlot)
	{
		m_nIndex = heroWingMemoryPieceSlot.index;
		m_nAccAttrValue = heroWingMemoryPieceSlot.accAttrValue;
	}
}
