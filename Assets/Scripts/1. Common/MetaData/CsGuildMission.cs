using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-27)
//---------------------------------------------------------------------------------------------------

public class CsGuildMission
{
	int m_nMissionId;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strTargetDescription;

	/*
	1 : NPC찾기
	- 목표NPCID

	2 : 몬스터처치
	- 목표대륙ID
	- 목표x좌표
	- 목표y좌표
	- 목표z좌표
	- 목표반지름
	- 목표몬스터ID
	- 목표횟수

	3 : 소환몬스터처치
	- 목표소환몬스터배치ID
	- 목표소환몬스터반경
	- 목표소환몬스터킬제한시간

	4 : 길드정신알리기
	- 목표대륙ID
	- 목표x좌표
	- 목표y좌표
	- 목표z좌표
	- 목표반지름
	- 목표대륙오브젝트ID
	*/
	int m_nType;
	CsContinent m_csContinentTarget;
	Vector3 m_flTargetPosition;
	float m_flTargetRadius;
	CsNpcInfo m_csNpcTarget;
	CsContinentObject m_csContinentObjectTarget;
	CsMonsterInfo m_csMonsterTarget;
	long m_lTargetSummonMonsterArrangeId;
	float m_flTargetSummonMonsterRadius;
	int m_nTargetSummonMonsterKillLimitTime;
	int m_nTargetCount;
	CsGuildContributionPointReward m_csGuildContributionPointReward;
	CsGuildFundReward m_csGuildFundReward;
	CsGuildBuildingPointReward m_csGuildBuildingPointReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string TargetDescription
	{
		get { return m_strTargetDescription; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetPosition
	{
		get { return m_flTargetPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public CsNpcInfo TargetNpc
	{
		get { return m_csNpcTarget; }
	}

	public CsContinentObject ContinentObjectTarget
	{
		get { return m_csContinentObjectTarget; }
	}

	public CsMonsterInfo TargetMonster
	{
		get { return m_csMonsterTarget; }
	}

	public long TargetSummonMonsterArrangeId
	{
		get { return m_lTargetSummonMonsterArrangeId; }
	}

	public float TargetSummonMonsterRadius
	{
		get { return m_flTargetSummonMonsterRadius; }
	}

	public int TargetSummonMonsterKillLimitTime
	{
		get { return m_nTargetSummonMonsterKillLimitTime; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public CsGuildContributionPointReward GuildContributionPointReward
	{
		get { return m_csGuildContributionPointReward; }
	}

	public CsGuildFundReward GuildFundReward
	{
		get { return m_csGuildFundReward; }
	}

	public CsGuildBuildingPointReward GuildBuildingPointReward
	{
		get { return m_csGuildBuildingPointReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMission(WPDGuildMission guildMission)
	{
		m_nMissionId = guildMission.missionId;
		m_strTargetTitle = CsConfiguration.Instance.GetString(guildMission.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(guildMission.targetContentKey);
		m_strTargetDescription = CsConfiguration.Instance.GetString(guildMission.targetDescriptionKey);
		m_nType = guildMission.type;
		m_csContinentTarget = CsGameData.Instance.GetContinent(guildMission.targetContinentId);
		m_flTargetPosition = new Vector3(guildMission.targetXPosition, guildMission.targetYPosition, guildMission.targetZPosition);
		m_flTargetRadius = guildMission.targetRadius;
		m_csNpcTarget = CsGameData.Instance.GetNpcInfo(guildMission.targetNpcId);
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(guildMission.targetContinentObjectId);
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(guildMission.targetMonsterId);
		m_lTargetSummonMonsterArrangeId = guildMission.targetSummonMonsterArrangeId;
		m_flTargetSummonMonsterRadius = guildMission.targetSummonMonsterRadius;
		m_nTargetSummonMonsterKillLimitTime = guildMission.targetSummonMonsterKillLimitTime;
		m_nTargetCount = guildMission.targetCount;
		m_csGuildContributionPointReward = CsGameData.Instance.GetGuildContributionPointReward(guildMission.guildContributionPointRewardId);
		m_csGuildFundReward = CsGameData.Instance.GetGuildFundReward(guildMission.guildFundRewardId);
		m_csGuildBuildingPointReward = CsGameData.Instance.GetGuildBuildingPointReward(guildMission.guildBuildingPointRewardId);
	}
}
