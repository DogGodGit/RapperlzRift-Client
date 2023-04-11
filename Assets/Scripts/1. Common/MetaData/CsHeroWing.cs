using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-09)
//---------------------------------------------------------------------------------------------------

public class CsHeroWing
{
	CsWing m_csWing;
	int m_nMemoryPieceStep;
	List<CsHeroWingMemoryPieceSlot> m_listCsHeroWingMemoryPieceSlot;

	//---------------------------------------------------------------------------------------------------
	public CsWing Wing
	{
		get { return m_csWing; }
	}

	public int MemoryPieceStep
	{
		get { return m_nMemoryPieceStep; }
		set { m_nMemoryPieceStep = value; }
	}

	public List<CsHeroWingMemoryPieceSlot> HeroWingMemoryPieceSlotList
	{
		get { return m_listCsHeroWingMemoryPieceSlot; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWing(PDHeroWing heroWing)
	{
		m_csWing = CsGameData.Instance.GetWing(heroWing.wingId);
		m_nMemoryPieceStep = heroWing.memoryPieceStep;

		m_listCsHeroWingMemoryPieceSlot = new List<CsHeroWingMemoryPieceSlot>();

		for (int i = 0; i < heroWing.memoryPieceSlots.Length; i++)
		{
			m_listCsHeroWingMemoryPieceSlot.Add(new CsHeroWingMemoryPieceSlot(heroWing.memoryPieceSlots[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWingMemoryPieceSlot GetHeroWingMemoryPieceSlot(int nIndex)
	{
		for (int i = 0; i < m_listCsHeroWingMemoryPieceSlot.Count; i++)
		{
			if (m_listCsHeroWingMemoryPieceSlot[i].Index == nIndex)
				return m_listCsHeroWingMemoryPieceSlot[i];
		}

		return null;
	}
}
