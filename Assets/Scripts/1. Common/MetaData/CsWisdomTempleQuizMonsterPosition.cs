using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleQuizMonsterPosition
{
	int m_nStepNo;
	int m_nRow;
	int m_nCol;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int Row
	{
		get { return m_nRow; }
	}

	public int Col
	{
		get { return m_nRow; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleQuizMonsterPosition(WPDWisdomTempleQuizMonsterPosition wisdomTempleQuizMonsterPosition)
	{
		m_nStepNo = wisdomTempleQuizMonsterPosition.stepNo;
		m_nRow = wisdomTempleQuizMonsterPosition.row;
		m_nCol = wisdomTempleQuizMonsterPosition.col;
	}
}
