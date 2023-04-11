using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsFishingQuest
{
	CsNpcInfo m_csNpcInfo;
	int m_nRequiredHeroLevel;
	int m_nLimitCount;
	int m_nCastingCount;
	int m_nCastingInterval;
	float m_flPartyRadius;
	float m_flPartyExpRewardFactor;
	int m_nPartyRecommendPopUpDisplayDuration;
	int m_nPartyRecommendPopUpHideDuration;
	float m_flGuildExpRewardFactor;

	List<CsFishingQuestSpot> m_listCsFishingQuestSpot;
	List<CsFishingQuestGuildTerritorySpot> m_listCsFishingQuestGuildTerritorySpot;

	//---------------------------------------------------------------------------------------------------
	public CsNpcInfo NpcInfo
	{
		get { return m_csNpcInfo; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int CastingCount
	{
		get { return m_nCastingCount; }
	}

	public int CastingInterval
	{
		get { return m_nCastingInterval; }
	}

	public float PartyRadius
	{
		get { return m_flPartyRadius; }
	}

	public float PartyExpRewardFactor
	{
		get { return m_flPartyExpRewardFactor; }
	}

	public int PartyRecommendPopUpDisplayDuration
	{
		get { return m_nPartyRecommendPopUpDisplayDuration; }
	}

	public int PartyRecommendPopUpHideDuration
	{
		get { return m_nPartyRecommendPopUpHideDuration; }
	}

	public List<CsFishingQuestSpot> FishingQuestSpotList
	{
		get { return m_listCsFishingQuestSpot; }
	}

	public List<CsFishingQuestGuildTerritorySpot> FishingQuestGuildTerritorySpotList
	{
		get { return m_listCsFishingQuestGuildTerritorySpot; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFishingQuest(WPDFishingQuest fishingQuest)
	{
		m_csNpcInfo = CsGameData.Instance.GetNpcInfo(fishingQuest.npcId);
		m_nRequiredHeroLevel = fishingQuest.requiredHeroLevel;
		m_nLimitCount = fishingQuest.limitCount;
		m_nCastingCount = fishingQuest.castingCount;
		m_nCastingInterval = fishingQuest.castingInterval;
		m_flPartyRadius = fishingQuest.partyRadius;
		m_flPartyExpRewardFactor = fishingQuest.partyExpRewardFactor;
		m_nPartyRecommendPopUpDisplayDuration = fishingQuest.partyRecommendPopUpDisplayDuration;
		m_nPartyRecommendPopUpHideDuration = fishingQuest.partyRecommendPopUpHideDuration;
		m_flGuildExpRewardFactor = fishingQuest.guildExpRewardFactor;

		m_listCsFishingQuestSpot = new List<CsFishingQuestSpot>();
		m_listCsFishingQuestGuildTerritorySpot = new List<CsFishingQuestGuildTerritorySpot>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsFishingQuestSpot GetFishingQuestSpot(int nSpotId)
	{
		for (int i = 0; i < m_listCsFishingQuestSpot.Count; i++)
		{
			if (m_listCsFishingQuestSpot[i].SpotId == nSpotId)
				return m_listCsFishingQuestSpot[i];
		}

		return null;
	}


	//---------------------------------------------------------------------------------------------------
	public CsFishingQuestGuildTerritorySpot GetFishingQuestGuildTerritorySpot(int nSpotId)
	{
		for (int i = 0; i < m_listCsFishingQuestGuildTerritorySpot.Count; i++)
		{
			if (m_listCsFishingQuestGuildTerritorySpot[i].SpotId == nSpotId)
				return m_listCsFishingQuestGuildTerritorySpot[i];
		}

		return null;
	}
}
