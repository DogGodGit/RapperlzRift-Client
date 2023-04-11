using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildFoodWarehouse
{
	string m_strName;
	int m_nLimitCount;
	CsGuildTerritoryNpc m_csGuildTerritoryNpc;
	int m_nLevelUpRequiredItemType;
	CsItemReward m_csItemRewardFullLevel;

	List<CsGuildFoodWarehouseLevel> m_listCsGuildFoodWarehouseLevel;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public CsGuildTerritoryNpc GuildTerritoryNpc
	{
		get { return m_csGuildTerritoryNpc; }
	}

	public int LevelUpRequiredItemType
	{
		get { return m_nLevelUpRequiredItemType; }
	}

	public CsItemReward ItemRewardFullLevel
	{
		get { return m_csItemRewardFullLevel; }
	}

	public List<CsGuildFoodWarehouseLevel> GuildFoodWarehouseLevelList
	{
		get { return m_listCsGuildFoodWarehouseLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildFoodWarehouse(WPDGuildFoodWarehouse guildFoodWarehouse)
	{
		m_strName = CsConfiguration.Instance.GetString(guildFoodWarehouse.nameKey);
		m_nLimitCount = guildFoodWarehouse.limitCount;
		m_csGuildTerritoryNpc = CsGameData.Instance.GuildTerritory.GetGuildTerritoryNpc(guildFoodWarehouse.guildTerritoryNpcId);
		m_nLevelUpRequiredItemType = guildFoodWarehouse.levelUpRequiredItemType;
		m_csItemRewardFullLevel = CsGameData.Instance.GetItemReward(guildFoodWarehouse.fullLevelItemRewardId);

		m_listCsGuildFoodWarehouseLevel = new List<CsGuildFoodWarehouseLevel>();
	}
}
