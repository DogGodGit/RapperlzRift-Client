using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsFieldBossEvent
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredHeroLevel;
	float m_flRewardRadius;

	List<CsFieldBossEventSchedule> m_listCsFieldBossEventSchedule;
	List<CsFieldBossEventAvailableReward> m_listCsFieldBossEventAvailableReward;
	List<CsFieldBoss> m_listCsFieldBoss;

	//---------------------------------------------------------------------------------------------------
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

	public float RewardRadius
	{
		get { return m_flRewardRadius; }
	}

	public List<CsFieldBossEventSchedule> FieldBossEventScheduleList
	{
		get { return m_listCsFieldBossEventSchedule; }
	}

	public List<CsFieldBossEventAvailableReward> FieldBossEventAvailableRewardList
	{
		get { return m_listCsFieldBossEventAvailableReward; }
	}

	public List<CsFieldBoss> FieldBossList
	{
		get { return m_listCsFieldBoss; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFieldBossEvent(WPDFieldBossEvent fieldBossEvent)
	{
		m_strName = CsConfiguration.Instance.GetString(fieldBossEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(fieldBossEvent.descriptionKey);
		m_nRequiredHeroLevel = fieldBossEvent.requiredHeroLevel;
		m_flRewardRadius = fieldBossEvent.rewardRadius;

		m_listCsFieldBossEventSchedule = new List<CsFieldBossEventSchedule>();
		m_listCsFieldBossEventAvailableReward = new List<CsFieldBossEventAvailableReward>();
		m_listCsFieldBoss = new List<CsFieldBoss>();
	}
}
