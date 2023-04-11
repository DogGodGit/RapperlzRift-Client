using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroBiographyQuest
{
	int m_nQuestNo;
	int m_nProgressCount;
	bool m_bCompleted;
	CsBiographyQuest m_csBiographyQuest;

	//---------------------------------------------------------------------------------------------------
	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	public bool Completed
	{
		get { return m_bCompleted; }
		set { m_bCompleted = value; }
	}

	public bool Excuted
	{
		get
		{
			if (m_bCompleted)
				return false;

			if (m_csBiographyQuest == null)
				return false;

			return ProgressCount >= m_csBiographyQuest.TargetCount;
		}
	}

	public CsBiographyQuest BiographyQuest
	{
		get { return m_csBiographyQuest; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBiographyQuest(CsBiography csBiography, PDHeroBiograhyQuest heroBiograhyQuest)
	{
		m_nQuestNo = heroBiograhyQuest.questNo;
		m_nProgressCount = heroBiograhyQuest.progressCount;
		m_bCompleted = heroBiograhyQuest.completed;

		m_csBiographyQuest = csBiography.GetBiographyQuest(heroBiograhyQuest.questNo);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBiographyQuest(CsBiography csBiography, int nQuestNo)
	{
		m_nQuestNo = nQuestNo;
		m_nProgressCount = 0;
		m_bCompleted = false;

		m_csBiographyQuest = csBiography.GetBiographyQuest(m_nQuestNo);
	}
}
