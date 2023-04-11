using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-09)
//---------------------------------------------------------------------------------------------------

public class CsWingMemoryPieceSlot
{
	int m_nWingId;
	int m_nSlotIndex;
	CsAttr m_csAttr;
	int m_nOpenStep;

	List<CsWingMemoryPieceSlotStep> m_listCsWingMemoryPieceSlotStep;

	//---------------------------------------------------------------------------------------------------
	public int WingId
	{
		get { return m_nWingId; }
	}

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public int OpenStep
	{
		get { return m_nOpenStep; }
	}

	public List<CsWingMemoryPieceSlotStep> WingMemoryPieceSlotStepList
	{
		get { return m_listCsWingMemoryPieceSlotStep; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceSlot(WPDWingMemoryPieceSlot wingMemoryPieceSlot)
	{
		m_nWingId = wingMemoryPieceSlot.wingId;
		m_nSlotIndex = wingMemoryPieceSlot.slotIndex;
		m_csAttr = CsGameData.Instance.GetAttr(wingMemoryPieceSlot.attrId);
		m_nOpenStep = wingMemoryPieceSlot.openStep;

		m_listCsWingMemoryPieceSlotStep = new List<CsWingMemoryPieceSlotStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceSlotStep GetWingMemoryPieceSlotStep(int nStep)
	{
		for (int i = 0; i < m_listCsWingMemoryPieceSlotStep.Count; i++)
		{
			if (m_listCsWingMemoryPieceSlotStep[i].Step == nStep)
				return m_listCsWingMemoryPieceSlotStep[i];
		}

		return null;
	}
}
