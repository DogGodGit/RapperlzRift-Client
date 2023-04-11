using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public enum EnSubQuestStatus
{
	Acception = 0,
	Completion = 1,
	Abandon = 2,
	Excuted = 3, // 완료 대기(클라 전용)
}

public class CsHeroSubQuest
{
	CsSubQuest m_csSubQuest;
	int m_nProgressCount;
	int m_nStatus;

	//---------------------------------------------------------------------------------------------------
	public CsSubQuest SubQuest
	{
		get { return m_csSubQuest; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	public int Status
	{
		get { return m_nStatus; }
		set { m_nStatus = value; }
	}

	public EnSubQuestStatus EnStatus
	{
		get 
		{
			if ((EnSubQuestStatus)m_nStatus == EnSubQuestStatus.Acception &&
				m_nProgressCount >= m_csSubQuest.TargetCount)
			{
				return EnSubQuestStatus.Excuted;
			}

			return (EnSubQuestStatus)m_nStatus;
		}		
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubQuest(PDHeroSubQuest heroSubQuest)
	{
		m_csSubQuest = CsGameData.Instance.GetSubQuest(heroSubQuest.questId);
		m_nProgressCount = heroSubQuest.progressCount;
		m_nStatus = heroSubQuest.status;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubQuest(int nQuestId)
	{
		m_csSubQuest = CsGameData.Instance.GetSubQuest(nQuestId);
		m_nProgressCount = 0;
		m_nStatus = 0;
	}
}
