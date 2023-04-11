using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsBountyHunterQuestReward
{
	int m_nQuestItemGrade;
	int m_nLevel;
	CsExpReward m_csExpReward;

	//---------------------------------------------------------------------------------------------------
	public int QuestItemGrade
	{
		get { return m_nQuestItemGrade; }
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
	public CsBountyHunterQuestReward(WPDBountyHunterQuestReward bountyHunterQuestReward)
	{
		m_nQuestItemGrade = bountyHunterQuestReward.questItemGrade;
		m_nLevel = bountyHunterQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(bountyHunterQuestReward.expRewardId);
	}

}
