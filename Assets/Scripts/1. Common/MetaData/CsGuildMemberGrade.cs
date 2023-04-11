using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

/*
1 : 길드장
2 : 부길드장
3 : 로드
4 : 일반
*/
public class CsGuildMemberGrade
{
	int m_nMemberGrade;
	string m_strName;
	bool m_bInvitationEnabled;
	bool m_bApplicationAcceptanceEnabled;
	bool m_bGuildFoodWarehouseRewardCollectionEnabled;
	bool m_bGuildSupplySupportQuestEnabled;
	bool m_bGuildBuildingLevelUpEnabled;
	bool m_bGuildCallEnabled;
	bool m_bWeeklyObjectiveSettingEnabled;
	bool m_bGuildBlessingBuffEnabled;


	//---------------------------------------------------------------------------------------------------
	public int MemberGrade
	{
		get { return m_nMemberGrade; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public bool InvitationEnabled
	{
		get { return m_bInvitationEnabled; }
	}

	public bool ApplicationAcceptanceEnabled
	{
		get { return m_bApplicationAcceptanceEnabled; }
	}

	public bool GuildFoodWarehouseRewardCollectionEnabled
	{
		get { return m_bGuildFoodWarehouseRewardCollectionEnabled; }
	}

	public bool GuildSupplySupportQuestEnabled
	{
		get { return m_bGuildSupplySupportQuestEnabled; }
	}

	public bool GuildBuildingLevelUpEnabled
	{
		get { return m_bGuildBuildingLevelUpEnabled; }
	}

	public bool GuildCallEnabled
	{
		get { return m_bGuildCallEnabled; }
	}

	public bool WeeklyObjectiveSettingEnabled
	{
		get { return m_bWeeklyObjectiveSettingEnabled; }
	}

	public bool GuildBlessingBuffEnabled
	{
		get { return m_bGuildBlessingBuffEnabled; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMemberGrade(WPDGuildMemberGrade guildMemberGrade)
	{
		m_nMemberGrade = guildMemberGrade.memberGrade;
		m_strName = CsConfiguration.Instance.GetString(guildMemberGrade.nameKey);
		m_bInvitationEnabled = guildMemberGrade.invitationEnabled;
		m_bApplicationAcceptanceEnabled = guildMemberGrade.applicationAcceptanceEnabled;
		m_bGuildFoodWarehouseRewardCollectionEnabled = guildMemberGrade.guildFoodWarehouseRewardCollectionEnabled;
		m_bGuildSupplySupportQuestEnabled = guildMemberGrade.guildSupplySupportQuestEnabled;
		m_bGuildBuildingLevelUpEnabled = guildMemberGrade.guildBuildingLevelUpEnabled;
		m_bGuildCallEnabled = guildMemberGrade.guildCallEnabled;
		m_bWeeklyObjectiveSettingEnabled = guildMemberGrade.weeklyObjectiveSettingEnabled;
		m_bGuildBlessingBuffEnabled = guildMemberGrade.guildBlessingBuffEnabled;
	}
}
