using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-09)
//---------------------------------------------------------------------------------------------------

public class CsWingMemoryPieceSlotStep
{
	int m_nWingId;
	int m_nSlotIndex;
	int m_nStep;
	int m_nAttrMaxValue;
	int m_nAttrIncBaseValue;
	int m_nAttrDecValue;

	//---------------------------------------------------------------------------------------------------
	public int WingId
	{
		get { return m_nWingId; }
	}

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int AttrMaxValue
	{
		get { return m_nAttrMaxValue; }
	}

	public int AttrIncBaseValue
	{
		get { return m_nAttrIncBaseValue; }
	}

	public int AttrDecValue
	{
		get { return m_nAttrDecValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceSlotStep(WPDWingMemoryPieceSlotStep wingMemoryPieceSlotStep)
	{
		m_nWingId = wingMemoryPieceSlotStep.wingId;
		m_nSlotIndex = wingMemoryPieceSlotStep.slotIndex;
		m_nStep = wingMemoryPieceSlotStep.step;
		m_nAttrMaxValue = wingMemoryPieceSlotStep.attrMaxValue;
		m_nAttrIncBaseValue = wingMemoryPieceSlotStep.attrIncBaseValue;
		m_nAttrDecValue = wingMemoryPieceSlotStep.attrDecValue;
	}
}
