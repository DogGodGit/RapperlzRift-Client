using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleQuizPoolEntry
{
	int m_nStepNo;
	int m_nQuizNo;
	string m_strQuestion;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int QuizNo
	{
		get { return m_nQuizNo; }
	}

	public string Question
	{
		get { return m_strQuestion; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleQuizPoolEntry(WPDWisdomTempleQuizPoolEntry wisdomTempleQuizPoolEntry)
	{
		m_nStepNo = wisdomTempleQuizPoolEntry.stepNo;
		m_nQuizNo = wisdomTempleQuizPoolEntry.quizNo;
		m_strQuestion = CsConfiguration.Instance.GetString(wisdomTempleQuizPoolEntry.questionKey);
	}
}
