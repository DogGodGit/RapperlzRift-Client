using WebCommon;
using UnityEngine;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsSceneryQuest
{
	int m_nQuestId;
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	CsContinent m_csContinent;
	Vector3 m_vtPosition;
	float m_flRadius;
	int m_nWaitingTime;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int QuestId
	{
		get { return m_nQuestId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
	}

	public Vector3 Position
	{
		get { return m_vtPosition; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int WaitingTime
	{
		get { return m_nWaitingTime; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSceneryQuest(WPDSceneryQuest sceneryQuest)
	{
		m_nQuestId = sceneryQuest.questId;
		m_strName = CsConfiguration.Instance.GetString(sceneryQuest.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(sceneryQuest.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(sceneryQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(sceneryQuest.targetContentKey);
		m_csContinent = CsGameData.Instance.GetContinent(sceneryQuest.continentId);
		m_vtPosition = new Vector3(sceneryQuest.xPosition, sceneryQuest.yPosition, sceneryQuest.zPosition);
		m_flRadius = sceneryQuest.radius;
		m_nWaitingTime = sceneryQuest.waitingTime;
		m_csItemReward = CsGameData.Instance.GetItemReward(sceneryQuest.itemRewardId);
	}
}
