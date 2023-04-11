using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsHeroGuildFarmQuest
{
	bool m_bIsObjectiveCompleted;

	//---------------------------------------------------------------------------------------------------
	public bool IsObjectiveCompleted
	{
		get { return m_bIsObjectiveCompleted; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroGuildFarmQuest(PDHeroGuildFarmQuest heroGuildFarmQuest)
	{
		m_bIsObjectiveCompleted = heroGuildFarmQuest.isObjectiveCompleted;
	}
}
