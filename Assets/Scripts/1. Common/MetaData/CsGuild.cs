using System;
using System.Collections.Generic;
using ClientCommon; 

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuild
{
	Guid m_guidId;
	string m_strName;
	string m_strNotice;
	int m_nBuildingPoint;
	long m_lFund;
	Guid m_guidFoodWarehouseCollectionId;
	List<CsGuildBuildingInstance> m_listCsGuildBuildingInstance;
	DateTime m_dtMoralPointDate;
	int m_nMoralPoint;

	DateTime m_dtDailyGuildSupplySupportQuestStartDate;
	int m_nDailyGuildSupplySupportQuestStartCount;

	DateTime m_dtDailyHuntingDonationDate;
	int m_nDailyHuntingDonationCount;

	DateTime m_dtDailyObjectiveDate;
	int m_nDailyObjectiveContentId;
	int m_nDailyObjectiveCompletionMemberCount;

	DateTime m_dtWeeklyObjectiveDate;
	int m_nWeeklyObjectiveId;
	int m_nWeeklyObjectiveCompletionMemberCount;

	int m_nApplicationCount;

	DateTime m_dtLastBlessingBuffStartDate;
	bool m_bIsBlessingBuffRunning;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public Guid FoodWarehouseCollectionId
	{
		get { return m_guidFoodWarehouseCollectionId; }
		set { m_guidFoodWarehouseCollectionId = value; }
	}

	public List<CsGuildBuildingInstance> GuildBuildingInstanceList
	{
		get { return m_listCsGuildBuildingInstance; }
	}

	public string Notice
	{
		get { return m_strNotice; }
		set { m_strNotice = value; }
	}

	public int BuildingPoint
	{
		get { return m_nBuildingPoint; }
		set { m_nBuildingPoint = value; }
	}

	public long Fund
	{
		get { return m_lFund; }
		set { m_lFund = value; }
	}

	public DateTime MoralPointDate
	{
		get { return m_dtMoralPointDate; }
		set { m_dtMoralPointDate = value; }
	}

	public int MoralPoint
	{
		get { return m_nMoralPoint; }
		set { m_nMoralPoint = value; }
	}

	public DateTime DailyGuildSupplySupportQuestStartDate
	{
		get { return m_dtDailyGuildSupplySupportQuestStartDate; }
		set { m_dtDailyGuildSupplySupportQuestStartDate = value; }
	}

	public int DailyGuildSupplySupportQuestStartCount
	{
		get { return m_nDailyGuildSupplySupportQuestStartCount; }
		set { m_nDailyGuildSupplySupportQuestStartCount = value; }
	}

	public DateTime DailyHuntingDonationDate
	{
		get { return m_dtDailyHuntingDonationDate; }
		set { m_dtDailyHuntingDonationDate = value; }
	}

	public int DailyHuntingDonationCount
	{
		get { return m_nDailyHuntingDonationCount; }
		set { m_nDailyHuntingDonationCount = value; }
	}

	public DateTime DailyObjectiveDate
	{
		get { return m_dtDailyObjectiveDate; }
		set { m_dtDailyObjectiveDate = value; }
	}

	public int DailyObjectiveContentId
	{
		get { return m_nDailyObjectiveContentId; }
		set { m_nDailyObjectiveContentId = value; }
	}

	public int DailyObjectiveCompletionMemberCount
	{
		get { return m_nDailyObjectiveCompletionMemberCount; }
		set { m_nDailyObjectiveCompletionMemberCount = value; }
	}

	public DateTime WeeklyObjectiveDate
	{
		get { return m_dtWeeklyObjectiveDate; }
		set { m_dtWeeklyObjectiveDate = value; }
	}

	public int WeeklyObjectiveId
	{
		get { return m_nWeeklyObjectiveId; }
		set { m_nWeeklyObjectiveId = value; }
	}

	public int WeeklyObjectiveCompletionMemberCount
	{
		get { return m_nWeeklyObjectiveCompletionMemberCount; }
		set { m_nWeeklyObjectiveCompletionMemberCount = value; }
	}

	public int ApplicationCount
	{
		get { return m_nApplicationCount; }
		set { m_nApplicationCount = value; }
	}

	public DateTime LastBlessingBuffStartDate
	{
		get { return m_dtLastBlessingBuffStartDate; }
		set { m_dtLastBlessingBuffStartDate = value; }
	}

	public bool IsBlessingBuffRunning
	{
		get { return m_bIsBlessingBuffRunning; }
		set { m_bIsBlessingBuffRunning = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuild(PDGuild guild)
	{
		m_guidId = guild.id;
		m_strName = guild.name;
		m_strNotice = guild.notice;
		m_nBuildingPoint = guild.buildingPoint;
		m_lFund = guild.fund;
		m_guidFoodWarehouseCollectionId = guild.foodWarehouseCollectionId;
		m_dtMoralPointDate = guild.moralPointDate;
		m_nMoralPoint = guild.moralPoint;
		m_dtDailyGuildSupplySupportQuestStartDate = guild.dailyGuildSupplySupportQuestStartDate;
		m_nDailyGuildSupplySupportQuestStartCount = guild.dailyGuildSupplySupportQuestStartCount;

		m_dtDailyHuntingDonationDate = guild.dailyHuntingDonationDate;
		m_nDailyHuntingDonationCount = guild.dailyHuntingDonationCount;

		m_dtDailyObjectiveDate = guild.dailyObjectiveDate;
		m_nDailyObjectiveContentId = guild.dailyObjectiveContentId;
		m_nDailyObjectiveCompletionMemberCount = guild.dailyObjectiveCompletionMemberCount;

		m_dtWeeklyObjectiveDate = guild.weeklyObjectiveDate;
		m_nWeeklyObjectiveId = guild.weeklyObjectiveId;
		m_nWeeklyObjectiveCompletionMemberCount = guild.weeklyObjectiveCompletionMemberCount;

		m_nApplicationCount = guild.applicationCount;

		m_dtLastBlessingBuffStartDate = guild.lastBlessingBuffStartDate;
		m_bIsBlessingBuffRunning = guild.isBlessingBuffRunning;

		m_listCsGuildBuildingInstance = new List<CsGuildBuildingInstance>();

		for (int i = 0; i < guild.buildingInsts.Length; i++)
		{
			m_listCsGuildBuildingInstance.Add(new CsGuildBuildingInstance(guild.buildingInsts[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuildingInstance GetGuildBuildingInstance(int nBuildingId)
	{
		for (int i = 0; i < m_listCsGuildBuildingInstance.Count; i++)
		{
			if (m_listCsGuildBuildingInstance[i].BuildingId == nBuildingId)
				return m_listCsGuildBuildingInstance[i];
		}

		return null;
	}
}
