using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsHeroGuildHuntingQuest
{
	Guid m_guidId;
	int m_nObjectiveId;
	int m_nProgressCount;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public int ObjectiveId
	{
		get { return m_nObjectiveId; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroGuildHuntingQuest(PDHeroGuildHuntingQuest heroGuildHuntingQuest)
	{
		m_guidId = heroGuildHuntingQuest.id;
		m_nObjectiveId = heroGuildHuntingQuest.objectiveId;
		m_nProgressCount = heroGuildHuntingQuest.progressCount;
	}
}
