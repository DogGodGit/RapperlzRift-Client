using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsSecretLetterQuestReward
{
	CsSecretLetterGrade m_csSecretLetterGrade;
	int m_nLevel;
	CsExpReward m_csExpReward;

	//---------------------------------------------------------------------------------------------------
	public CsSecretLetterGrade SecretLetterGrade
	{
		get { return m_csSecretLetterGrade; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSecretLetterQuestReward(WPDSecretLetterQuestReward secretLetterQuestReward)
	{
		m_csSecretLetterGrade = CsGameData.Instance.GetSecretLetterGrade(secretLetterQuestReward.grade);
		m_nLevel = secretLetterQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(secretLetterQuestReward.expRewardId);
	}
}
