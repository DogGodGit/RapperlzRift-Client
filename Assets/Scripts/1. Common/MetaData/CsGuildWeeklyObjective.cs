using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsGuildWeeklyObjective
{
	int m_nObjectiveId;
	string m_strName;
	string m_strDescription;
	int m_nCompletionMemberCount;
	CsItemReward m_csItemReward1;
	CsItemReward m_csItemReward2;
	CsItemReward m_csItemReward3;

	//---------------------------------------------------------------------------------------------------
	public int ObjectiveId
	{
		get { return m_nObjectiveId; }
	}
	
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int CompletionMemberCount
	{
		get { return m_nCompletionMemberCount; }
	}

	public CsItemReward ItemReward1
	{
		get { return m_csItemReward1; }
	}

	public CsItemReward ItemReward2
	{
		get { return m_csItemReward2; }
	}

	public CsItemReward ItemReward3
	{
		get { return m_csItemReward3; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildWeeklyObjective(WPDGuildWeeklyObjective guildWeeklyObjective)
	{
		m_nObjectiveId = guildWeeklyObjective.objectiveId;
		m_strName = CsConfiguration.Instance.GetString(guildWeeklyObjective.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(guildWeeklyObjective.descriptionKey);
		m_nCompletionMemberCount = guildWeeklyObjective.completionMemberCount;
		m_csItemReward1 = CsGameData.Instance.GetItemReward(guildWeeklyObjective.itemReward1Id);
		m_csItemReward2 = CsGameData.Instance.GetItemReward(guildWeeklyObjective.itemReward2Id);
		m_csItemReward3 = CsGameData.Instance.GetItemReward(guildWeeklyObjective.itemReward3Id);
	}
}
