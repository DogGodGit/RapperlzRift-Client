using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-09)
//---------------------------------------------------------------------------------------------------

public class CsWingMemoryPieceStep
{
	int m_nWingId;
	int m_nStep;
	int m_nRequiredMemoryPieceCount;

	//---------------------------------------------------------------------------------------------------
	public int WingId
	{
		get { return m_nWingId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int RequiredMemoryPieceCount
	{
		get { return m_nRequiredMemoryPieceCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceStep(WPDWingMemoryPieceStep wingMemoryPieceStep)
	{
		m_nWingId = wingMemoryPieceStep.wingId;
		m_nStep = wingMemoryPieceStep.step;
		m_nRequiredMemoryPieceCount = wingMemoryPieceStep.requiredMemoryPieceCount;
	}
}
