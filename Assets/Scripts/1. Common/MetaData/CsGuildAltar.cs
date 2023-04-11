using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-26)
//---------------------------------------------------------------------------------------------------

public class CsGuildAltar
{
	CsGuildTerritoryNpc m_csGuildTerritoryNpc;
	int m_nDailyHeroMaxMoralPoint;
	int m_nDailyGuildMaxMoralPoint;
	int m_nDonationGold;
	int m_nDonationRewardMoralPoint;
	int m_nSpellInjectionDuration;
	int m_nSpellInjectionRewardMoralPoint;
	long m_lDefenseMonsterArrangeId;
	int m_nDefenseRewardMoralPoint;
	int m_nDefenseCooltime;
	int m_nDefenseLimitTime;

	List<CsGuildAltarDefenseMonsterAttrFactor> m_listCsGuildAltarDefenseMonsterAttrFactor;

	//---------------------------------------------------------------------------------------------------
	public CsGuildTerritoryNpc GuildTerritoryNpc
	{
		get { return m_csGuildTerritoryNpc; }
	}

	public int DailyHeroMaxMoralPoint
	{
		get { return m_nDailyHeroMaxMoralPoint; }
	}

	public int DailyGuildMaxMoralPoint
	{
		get { return m_nDailyGuildMaxMoralPoint; }
	}

	public int DonationGold
	{
		get { return m_nDonationGold; }
	}

	public int DonationRewardMoralPoint
	{
		get { return m_nDonationRewardMoralPoint; }
	}

	public int SpellInjectionDuration
	{
		get { return m_nSpellInjectionDuration; }
	}

	public int SpellInjectionRewardMoralPoint
	{
		get { return m_nSpellInjectionRewardMoralPoint; }
	}

	public long DefenseMonsterArrangeId
	{
		get { return m_lDefenseMonsterArrangeId; }
	}

	public int DefenseRewardMoralPoint
	{
		get { return m_nDefenseRewardMoralPoint; }
	}

	public int DefenseCooltime
	{
		get { return m_nDefenseCooltime; }
	}

	public int DefenseLimitTime
	{
		get { return m_nDefenseLimitTime; }
	}

	public List<CsGuildAltarDefenseMonsterAttrFactor> GuildAltarDefenseMonsterAttrFactorList
	{
		get { return m_listCsGuildAltarDefenseMonsterAttrFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildAltar(WPDGuildAltar guildAltar)
	{
		m_csGuildTerritoryNpc = CsGameData.Instance.GuildTerritory.GetGuildTerritoryNpc(guildAltar.guildTerritoryNpcId);
		m_nDailyHeroMaxMoralPoint = guildAltar.dailyHeroMaxMoralPoint;
		m_nDailyGuildMaxMoralPoint = guildAltar.dailyGuildMaxMoralPoint;
		m_nDonationGold = guildAltar.donationGold;
		m_nDonationRewardMoralPoint = guildAltar.donationRewardMoralPoint;
		m_nSpellInjectionDuration = guildAltar.spellInjectionDuration;
		m_nSpellInjectionRewardMoralPoint = guildAltar.spellInjectionRewardMoralPoint;
		m_lDefenseMonsterArrangeId = guildAltar.defenseMonsterArrangeId;
		m_nDefenseRewardMoralPoint = guildAltar.defenseRewardMoralPoint;
		m_nDefenseCooltime = guildAltar.defenseCooltime;
		m_nDefenseLimitTime = guildAltar.defenseLimitTime;

		m_listCsGuildAltarDefenseMonsterAttrFactor = new List<CsGuildAltarDefenseMonsterAttrFactor>();
	}
}
