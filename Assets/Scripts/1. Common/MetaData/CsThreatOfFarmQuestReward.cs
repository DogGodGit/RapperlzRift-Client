using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-01)
//---------------------------------------------------------------------------------------------------

public class CsThreatOfFarmQuestReward
{
	int m_nLevel;
	CsExpReward m_csExpRewardMissionCompletion;
	CsItemReward m_csItemRewardMissionCompletion;
	CsItemReward m_csItemRewardQuestCompletion;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward MissionCompletionExpReward
	{
		get { return m_csExpRewardMissionCompletion; }
	}

	public CsItemReward MissionCompletionItemReward
	{
		get { return m_csItemRewardMissionCompletion; }
	}

	public CsItemReward QuestCompletionItemReward
	{
		get { return m_csItemRewardQuestCompletion; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuestReward(WPDTreatOfFarmQuestReward threatOfFarmQuestReward)
	{
		m_nLevel = threatOfFarmQuestReward.level;
		m_csExpRewardMissionCompletion = CsGameData.Instance.GetExpReward(threatOfFarmQuestReward.missionCompletionExpRewardId);
		m_csItemRewardMissionCompletion = CsGameData.Instance.GetItemReward(threatOfFarmQuestReward.missionCompletionItemRewardId);
		m_csItemRewardQuestCompletion = CsGameData.Instance.GetItemReward(threatOfFarmQuestReward.questCompletionItemRewardId);
	}
}
