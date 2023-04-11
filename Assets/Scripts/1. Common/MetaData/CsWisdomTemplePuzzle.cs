using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTemplePuzzle
{
	int m_nPuzzleId;
	string m_strTargetTitle;
	string m_strTargetContent;

	//---------------------------------------------------------------------------------------------------
	public int PuzzleId
	{
		get { return m_nPuzzleId; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTemplePuzzle(WPDWisdomTemplePuzzle wisdomTemplePuzzle)
	{
		m_nPuzzleId = wisdomTemplePuzzle.puzzleId;
		m_strTargetTitle = CsConfiguration.Instance.GetString(wisdomTemplePuzzle.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(wisdomTemplePuzzle.targetContentKey);
	}
}
