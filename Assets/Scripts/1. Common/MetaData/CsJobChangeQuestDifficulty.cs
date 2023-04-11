using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsJobChangeQuestDifficulty
{
	int m_nQuestNo;
	int m_nDifficulty;
	string m_strName;
	bool m_bIsTargetPlaceGuildTerritory;

	//---------------------------------------------------------------------------------------------------
	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public bool IsTargetPlaceGuildTerritory
	{
		get { return m_bIsTargetPlaceGuildTerritory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobChangeQuestDifficulty(WPDJobChangeQuestDifficulty jobChangeQuestDifficulty)
	{
		m_nQuestNo = jobChangeQuestDifficulty.questNo;
		m_nDifficulty = jobChangeQuestDifficulty.difficulty;
		m_strName = CsConfiguration.Instance.GetString(jobChangeQuestDifficulty.nameKey);
		m_bIsTargetPlaceGuildTerritory = jobChangeQuestDifficulty.isTargetPlaceGuildTerritory;
	}
}
