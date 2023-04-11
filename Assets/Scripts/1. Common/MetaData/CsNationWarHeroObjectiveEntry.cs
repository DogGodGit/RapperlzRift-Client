using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarHeroObjectiveEntry
{
	int m_nEntryNo;
	string m_strName;
	string m_strDescription;
	int m_nType;
	int m_nObjectiveCount;
	int m_nRewardType;
	CsOwnDiaReward m_csOwnDiaReward;
	CsExploitPointReward m_csExploitPointReward;

	//---------------------------------------------------------------------------------------------------
	public int EntryNo
	{
		get { return m_nEntryNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int ObjectiveCount
	{
		get { return m_nObjectiveCount; }
	}

	public int RewardType
	{
		get { return m_nRewardType; }
	}

	public CsOwnDiaReward OwnDiaReward
	{
		get { return m_csOwnDiaReward; }
	}

	public CsExploitPointReward ExploitPointReward
	{
		get { return m_csExploitPointReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarHeroObjectiveEntry(WPDNationWarHeroObjectiveEntry nationWarHeroObjectiveEntry)
	{
		m_nEntryNo = nationWarHeroObjectiveEntry.entryNo;
		m_strName = CsConfiguration.Instance.GetString(nationWarHeroObjectiveEntry.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(nationWarHeroObjectiveEntry.descriptionKey);
		m_nType = nationWarHeroObjectiveEntry.type;
		m_nObjectiveCount = nationWarHeroObjectiveEntry.objectiveCount;
		m_nRewardType = nationWarHeroObjectiveEntry.rewardType;
		m_csOwnDiaReward = CsGameData.Instance.GetOwnDiaReward(nationWarHeroObjectiveEntry.ownDiaRewardId);
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(nationWarHeroObjectiveEntry.exploitPointRewardId);
	}
}
