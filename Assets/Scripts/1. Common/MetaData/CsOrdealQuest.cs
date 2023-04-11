using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-21)
//---------------------------------------------------------------------------------------------------

public class CsOrdealQuest
{
	int m_nQuestNo;
	string m_strName;
	string m_strDescription;
	int m_nRequiredHeroLevel;
	CsItem m_csItemAvailableReward;

	List<CsOrdealQuestMission> m_listCsOrdealQuestMission;

	//---------------------------------------------------------------------------------------------------
	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsItem AvailableRewardItem
	{
		get { return m_csItemAvailableReward; }
	}

	public List<CsOrdealQuestMission> OrdealQuestMissionList
	{
		get { return m_listCsOrdealQuestMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOrdealQuest(WPDOrdealQuest ordealQuest)
	{
		m_nQuestNo = ordealQuest.questNo;
		m_strName = CsConfiguration.Instance.GetString(ordealQuest.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(ordealQuest.descriptionKey);
		m_nRequiredHeroLevel = ordealQuest.requiredHeroLevel;
		m_csItemAvailableReward = CsGameData.Instance.GetItem(ordealQuest.availableRewardItemId);

		m_listCsOrdealQuestMission = new List<CsOrdealQuestMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsOrdealQuestMission GetOrdealQuestMission(int nSlotIndex, int nMissionNo)
	{
		for (int i = 0; i < m_listCsOrdealQuestMission.Count; i++)
		{
			if (m_listCsOrdealQuestMission[i].SlotIndex == nSlotIndex && m_listCsOrdealQuestMission[i].MissionNo == nMissionNo)
				return m_listCsOrdealQuestMission[i];
		}

		return null;
	}
}
