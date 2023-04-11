using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using System;
using SimpleDebugLog;

public enum EnGuildMissionState { None, Accepted, Competed }
public enum EnGuildFarmQuestState { None, Accepted, Executed, Competed }
public enum EnGuildPlayState { None = 0, FarmQuest, FoodWareHouse, Altar, Defense, Mission, SupplySupport, Hunting , Fishing}
public enum EnGuildSupplySupportState { None, Accepted }
public enum EnGuildHuntingState { None, Accepted, Executed, Competed }

public class CsGuildMissionMonster
{
	long m_lInstanceId;
	float m_time;
	int m_nContinentId;
	Vector3 m_vtPos;

	public CsGuildMissionMonster(long lInstanceId, int nContinentId, Vector3 vtPos, float flRemainingLifeTime)
	{
		m_lInstanceId = lInstanceId;
		m_nContinentId = nContinentId;
		m_vtPos = vtPos;
		m_time = Time.realtimeSinceStartup + flRemainingLifeTime;
	}

	public float RemainingLifeTime
	{
		get { return Mathf.Max(m_time - Time.realtimeSinceStartup, 0); }
	}

	public long InstanceId
	{
		get { return m_lInstanceId; }
	}

	public int ContinentId
	{
		get { return m_nContinentId; }
	}

	public Vector3 Position
	{
		get { return m_vtPos; }
	}
}

public class CsGuildManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	EnGuildPlayState m_enGuildPlayAutoState = EnGuildPlayState.None;

	bool m_bInteraction = false;
	bool m_bSpellInjection = false;
	bool m_bFarmQuestMissionExcuted = false;
    bool m_bGoToGuildFoodWareHouse = false;

	bool m_bGuildDefense = false;
    bool m_bGoAutoAltar = false;
	bool m_bAltarEnter = false;

	Guid m_guidAppId;
	Guid m_guidBanishMemberId;
	Guid m_guidInvitationId;
	//Guid m_guidTargetMemberId;
	int m_nTargetMemberGrade;
	int m_nBuildingId;
	int m_nSkillId;

	CsGuild m_csGuild;
	List<CsHeroGuildSkill> m_listCsHeroGuildSkill;
    CsGuildMemberGrade m_csGuildMemberGrade;
	int m_nTotalGuildContributionPoint;
	int m_nGuildContributionPoint;
	int m_nGuldPoint;

	float m_flGuildRejoinRemainingTime;

	int m_nDailyGuildApplicationCount;
	DateTime m_dtGuildApplicationCountDate;

	int m_nDailyGuildDonationCount;
	DateTime m_dtGuildDonationCountDate;

	List<CsHeroGuildApplication> m_listCsHeroGuildApplication;

	List<CsGuildMember> m_listCsGuildMember = new List<CsGuildMember>();

	int m_nDailyBanishmentCount;
	DateTime m_dtBanishmentCountDate;

	List<CsGuildApplication> m_listCsGuildApplication = new List<CsGuildApplication>();
	List<CsHeroGuildInvitation> m_listCsHeroGuildInvitation = new List<CsHeroGuildInvitation>();

	int m_nDailyGuildFarmQuestStartCount;
	DateTime m_dtGuildFarmQuestStartCountDate;

	int m_nDailyGuildFoodWarehouseStockCount;
	DateTime m_dtGuildFoodWarehouseStockCountDate;
	Guid m_guidReceivedGuildFoodWarehouseCollectionId;

	CsHeroGuildFarmQuest m_csHeroGuildFarmQuest;
	EnGuildFarmQuestState m_enGuildFarmQuestState = EnGuildFarmQuestState.None;

	CsGuildMission m_csGuildMission;
	CsGuildMissionMonster m_csGuildMissionMonster;
	int m_nMissionProgressCount;
	int m_nMissionCompletedCount;
	bool m_bMissionQuest = true;
	bool m_bMissionCompleted = false;
	//DateTime m_dtGuildMissionQuestStartDate;
	EnGuildMissionState m_enGuildMissionState = EnGuildMissionState.None;

	int m_nGuildMoralPoint;
	long m_lDefenseMonsterInstanceId;						// 길드제단수비몬스터인스턴스ID
	Vector3 m_vtDefenseMonsterPos;							// 길드제단수비몬스터위치
	float m_flGuildAltarDefenseMissionRemainingCoolTime;    // 길드제단수비미션남은쿨타임
	DateTime m_dtGuildAltarRewardReceivedDate;              // 길드제단보상받은날짜
	DateTime m_dtGuildDailyRewardReceivedDate;              // 길드일일보상받은날짜
	DateTime m_dtGuildMoralPointDate;

	CsGuildSupplySupportQuestPlay m_csGuildSupplySupportQuestPlay;	// 길드물자지원퀘스트플레이. 없으면 null
	float m_flGuildSupplySupportQuestRemainingTime;
    int m_nDailyGuildSupplySupportQuestStartCount;
    DateTime m_dtDailyGuildSupplySupportQuestStartCountDate;
	EnGuildSupplySupportState m_enGuildSupplySupportState = EnGuildSupplySupportState.None;
	
	CsHeroGuildHuntingQuest m_csHeroGuildHuntingQuest;
	CsGuildHuntingQuestObjective m_csGuildHuntingQuestObjective;
	int m_nDailyGuildHuntingQuestStartCount;
	DateTime m_dtGuildHuntingQuestStartCountDate;
	DateTime m_dtGuildHuntingDonationDate;
	DateTime m_dtGuildHuntingDonationRewardReceivedDate;
	EnGuildHuntingState m_enGuildHuntingState = EnGuildHuntingState.None;

	int m_nGuildDailyObjectiveRewardReceivedNo;
	DateTime m_dtGuildDailyObjectiveRewardReceivedDate;
	DateTime m_dtGuildWeeklyObjectiveRewardReceivedDate;

	float m_flGuildDailyObjectiveNoticeRemainingCoolTime;

	//---------------------------------------------------------------------------------------------------
	public static CsGuildManager Instance
	{
		get { return CsSingleton<CsGuildManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate <EnGuildPlayState> EventStartAutoPlay;		// ig, Ui
	public event Delegate<object, EnGuildPlayState> EventStopAutoPlay;  // ig, ui


	public event Delegate EventUpdateFarmQuestState;
	public event Delegate<int> EventFoodWarehouseNpcDialog;
	public event Delegate<int> EventFarmNpcDialog;
	public event Delegate<int> EventFishingDialog;
	public event Delegate EventGuildFoodWareHouseDialog;
	public event Delegate EventGuildFarmQuestNPCDialog;
    public event Delegate EventGuildAltarNPCDialog;
    
	public event Delegate<bool> EventGuildAltarGoOut;
	public event Delegate<bool> EventGuildSpiritArea;
	public event Delegate<bool> EventFarmInteractionArea;
	public event Delegate EventUpdateMissionState;
	public event Delegate EventMissionAcceptDialog;
	public event Delegate EventMissionMissionDialog;
	public event Delegate EventUpdateSupplySupportState;
	public event Delegate EventStartSupplySupportNpctDialog;
	public event Delegate EventEndSupplySupportNpctDialog;

	public event Delegate EventUpdateGuildHuntingQuestState;
	public event Delegate EventStartGuildHuntingDialog;
	public event Delegate EventEndGuildHuntingDialog;

	public event Delegate EventMyHeroGuildFarmQuestInteractionCancel;
    public event Delegate EventMyHeroGuildAltarSpellInjectionMissionCancel;

    // Command
	public event Delegate<List<CsSimpleGuild>> EventGuildList;
	public event Delegate EventGuildCreate;
	public event Delegate EventGuildApply;
	public event Delegate EventGuildMemberTabInfo;
	public event Delegate EventGuildApplicationList;
	public event Delegate EventGuildApplicationAccept;
	public event Delegate EventGuildApplicationRefuse;
	public event Delegate<int> EventGuildExit;
	public event Delegate EventGuildMemberBanish;
	public event Delegate EventGuildInvite;
	public event Delegate EventGuildInvitationAccept;
	public event Delegate EventGuildInvitationRefuse;
	public event Delegate EventGuildNoticeSet;
	public event Delegate EventGuildAppoint;
	public event Delegate EventGuildMasterTransfer;
	public event Delegate EventGuildDonate;
	public event Delegate EventGuildBuildingLevelUp;
	public event Delegate EventGuildSkillLevelUp;
	public event Delegate<string> EventContinentExitForGuildTerritoryEnter;
	public event Delegate<Guid, PDHero[], PDMonsterInstance[], PDVector3, float> EventGuildTerritoryEnter;
	public event Delegate<int> EventGuildTerritoryExit;

	public event Delegate<PDHeroGuildFarmQuest> EventGuildFarmQuestAccept;
	public event Delegate EventGuildFarmQuestInteractionStart;
	public event Delegate<bool, long> EventGuildFarmQuestComplete;
	public event Delegate EventGuildFarmQuestAbandon;

	public event Delegate<int, int> EventGuildFoodWarehouseInfo;
	public event Delegate<bool, long, int, int, int> EventGuildFoodWarehouseStock;
	public event Delegate EventGuildFoodWarehouseCollect;
	public event Delegate EventGuildFoodWarehouseRewardReceive;

	public event Delegate EventGuildAltarDonate;
	public event Delegate EventGuildAltarRewardReceive;
    public event Delegate EventGuildAltarSpellInjectionMissionStart;
    public event Delegate EventGuildAltarDefenseMissionStart;
    public event Delegate<bool, long> EventGuildAltarCompleted;

	public event Delegate EventGuildMissionQuestAccept; 
	public event Delegate EventGuildMissionAccept;
	public event Delegate EventGuildMissionAbandon;
	public event Delegate<bool, long> EventGuildMissionComplete;
	public event Delegate EventGuildMissionTargetNpcInteract;

	public event Delegate EventGuildDailyRewardReceive;
	public event Delegate EventGuildCall;
	public event Delegate<int> EventGuildCallTransmission;
    public event Delegate<PDContinentEntranceInfo> EventContinentEnterForGuildCallTransmission;

    public event Delegate<PDGuildSupplySupportQuestCartInstance> EventGuildSupplySupportQuestAccept;
    public event Delegate<bool, long> EventGuildSupplySupportQuestComplete;

	public event Delegate EventGuildHuntingQuestAccept;
	public event Delegate EventGuildHuntingQuestAbandon;
	public event Delegate EventGuildHuntingQuestComplete;
	public event Delegate EventGuildHuntingDonate;
	public event Delegate EventGuildHuntingDonationRewardReceive;
	public event Delegate EventGuildDailyObjectiveNotice;
	public event Delegate<List<CsGuildDailyObjectiveCompletionMember>> EventGuildDailyObjectiveCompletionMemberList;
	public event Delegate EventGuildDailyObjectiveRewardReceive;
	public event Delegate EventGuildWeeklyObjectiveSet;
	public event Delegate EventGuildWeeklyObjectiveRewardReceive;

	public event Delegate EventGuildBlessingBuffStart;

	// Event
	public event Delegate EventGuildApplicationAccepted;
	public event Delegate<string> EventGuildApplicationRefused;
	public event Delegate EventGuildApplicationCountUpdated;
	public event Delegate<Guid, string> EventGuildMemberEnter;
	public event Delegate<Guid, string, bool> EventGuildMemberExit;
	public event Delegate<int> EventGuildBanished;
	public event Delegate<Guid, Guid, string, int> EventHeroGuildInfoUpdated;
	public event Delegate<CsHeroGuildInvitation> EventGuildInvitationArrived;
	public event Delegate<Guid, string> EventGuildInvitationRefused;
	public event Delegate<Guid, Guid, string> EventGuildInvitationLifetimeEnded;
	public event Delegate<Guid, string, int, Guid, string, int> EventGuildAppointed;
	public event Delegate<Guid, string, Guid, string> EventGuildMasterTransferred;
	public event Delegate EventGuildBuildingLevelUpEvent;
	public event Delegate EventGuildFoodWarehouseCollected;
	public event Delegate EventGuildFarmQuestInteractionCompleted;
	public event Delegate EventGuildFarmQuestInteractionCanceled;
	public event Delegate<Guid> EventHeroGuildFarmQuestInteractionStarted;
	public event Delegate<Guid> EventHeroGuildFarmQuestInteractionCompleted;
	public event Delegate<Guid> EventHeroGuildFarmQuestInteractionCanceled;
	public event Delegate EventGuildNoticeChanged;
	public event Delegate EventGuildFundChanged;
	public event Delegate EventGuildMoralPointChanged;
    public event Delegate EventGuildAltarSpellInjectionMissionCompleted;
    public event Delegate EventGuildAltarSpellInjectionMissionCanceled;
    public event Delegate<Guid> EventHeroGuildAltarSpellInjectionMissionStarted;
    public event Delegate<Guid> EventHeroGuildAltarSpellInjectionMissionCompleted;
    public event Delegate<Guid> EventHeroGuildAltarSpellInjectionMissionCanceled;
    public event Delegate EventGuildAltarDefenseMissionCompleted;
    public event Delegate EventGuildAltarDefenseMissionFailed;
	public event Delegate EventGuildMissionFailed;	
	public event Delegate<CsChattingMessage> EventGuildSpiritAnnounced;
	public event Delegate<CsGuildRanking, List<CsGuildRanking>> EventServerGuildRanking;
	public event Delegate<CsGuildRanking, List<CsGuildRanking>> EventNationGuildRanking;
	public event Delegate<CsGuildCall> EventGuildCalled;
    public event Delegate EventGuildSupplySupportQuestStarted;
    public event Delegate <bool, long>EventGuildSupplySupportQuestCompleted;
    public event Delegate EventGuildSupplySupportQuestFail;

	public event Delegate EventGuildHuntingQuestUpdated;
	public event Delegate EventGuildHuntingDonationCountUpdated;
	public event Delegate<CsChattingMessage> EventGuildDailyObjectiveNoticeEvent;
	public event Delegate EventGuildDailyObjectiveSet;
	public event Delegate EventGuildDailyObjectiveCompletionMemberCountUpdated;
	public event Delegate EventGuildWeeklyObjectiveSetEvent;
	public event Delegate EventGuildWeeklyObjectiveCompletionMemberCountUpdated;

	public event Delegate EventGuildTerritoryRevive;
	public event Delegate<Guid, PDHero[], PDMonsterInstance[], PDVector3, float> EventGuildTerritoryEnterForGuildTerritoryRevival;

	public event Delegate<CsChattingMessage> EventGuildBlessingBuffStarted;
    public event Delegate<CsChattingMessage> EventGuildBlessingBuffEnded;

	//---------------------------------------------------------------------------------------------------
	public CsGuild Guild
	{
		get { return m_csGuild; }
	}

	public Guid GuildId
	{
		get
		{
			if (m_csGuild == null)
			{
				return Guid.Empty;
			}
			return m_csGuild.Id;
		}
	}

	public string GuildName
	{
		get
		{
			if (m_csGuild == null)
			{
				return "";
			}
			return m_csGuild.Name;
		}
	}

    public CsGuildMemberGrade MyGuildMemberGrade
    {
        get { return m_csGuildMemberGrade; }
    }

	public int TotalGuildContributionPoint
	{
		get { return m_nTotalGuildContributionPoint; }
		set { m_nTotalGuildContributionPoint = value; }
	}

	public int GuildContributionPoint
	{
		get { return m_nGuildContributionPoint; }
		set { m_nGuildContributionPoint = value; }
	}

	public int GuldPoint
	{
		get { return m_nGuldPoint; }
		set { m_nGuldPoint = value; }
	}

	public float GuildRejoinRemainingTime
	{
		get { return m_flGuildRejoinRemainingTime; }
		set { m_flGuildRejoinRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public int DailyGuildApplicationCount
	{
		get { return m_nDailyGuildApplicationCount; }
		set { m_nDailyGuildApplicationCount = value; }
	}

	public DateTime GuildApplicationCountDate
	{
		get { return m_dtGuildApplicationCountDate; }
		set { m_dtGuildApplicationCountDate = value; }
	}

	public int DailyGuildDonationCount
	{
		get { return m_nDailyGuildDonationCount; }
		set { m_nDailyGuildDonationCount = value; }
	}

	public DateTime GuildDonationCountDate
	{
		get { return m_dtGuildDonationCountDate; }
		set { m_dtGuildDonationCountDate = value; }
	}

	public List<CsHeroGuildApplication> HeroGuildApplicationList
	{
		get { return m_listCsHeroGuildApplication; }
	}

	public int Level
	{
		get
		{
			if (m_csGuild == null)
			{
				return -1;
			}
			else
			{
				return m_csGuild.GetGuildBuildingInstance(1).Level;
			}
		}
	}

	public string Notice
	{
		get { return m_csGuild.Notice; }
	}

	public int BuildingPoint
	{
		get { return m_csGuild.BuildingPoint; }
	}

	public long Fund
	{
		get { return m_csGuild.Fund; }
	}

	public List<CsGuildMember> GuildMemberList
	{
		get { return m_listCsGuildMember; }
	}

	public int DailyBanishmentCount
	{
		get { return m_nDailyBanishmentCount; }
		set { m_nDailyBanishmentCount = value; }
	}

	public DateTime BanishmentCountDate
	{
		get { return m_dtBanishmentCountDate; }
		set { m_dtBanishmentCountDate = value; }
	}

	public List<CsGuildApplication> GuildApplicationList
	{
		get { return m_listCsGuildApplication; }
	}

	public List<CsHeroGuildInvitation> HeroGuildInvitationList
	{
		get { return m_listCsHeroGuildInvitation; }
	}

	public int DailyGuildFarmQuestStartCount
	{
		get { return m_nDailyGuildFarmQuestStartCount; }
		set { m_nDailyGuildFarmQuestStartCount = value; }
	}

	public DateTime GuildFarmQuestStartCountDate
	{
		get { return m_dtGuildFarmQuestStartCountDate; }
		set { m_dtGuildFarmQuestStartCountDate = value; }
	}

	public CsHeroGuildFarmQuest HeroGuildFarmQuest
	{
		get { return m_csHeroGuildFarmQuest; }
		set { m_csHeroGuildFarmQuest = value; }
	}

	public int DailyGuildFoodWarehouseStockCount
	{
		get { return m_nDailyGuildFoodWarehouseStockCount; }
		set { m_nDailyGuildFoodWarehouseStockCount = value; }
	}

	public DateTime GuildFoodWarehouseStockCountDate
	{
		get { return m_dtGuildFoodWarehouseStockCountDate; }
		set { m_dtGuildFoodWarehouseStockCountDate = value; }
	}

	public Guid ReceivedGuildFoodWarehouseCollectionId
	{
		get { return m_guidReceivedGuildFoodWarehouseCollectionId; }
		set { m_guidReceivedGuildFoodWarehouseCollectionId = value; }
	}

	public EnGuildFarmQuestState GuildFarmQuestState
	{
		get { return m_enGuildFarmQuestState; }
		set { m_enGuildFarmQuestState = value; }
	}

	public CsGuildTerritory GuildTerritory
	{
		get { return CsGameData.Instance.GuildTerritory; }
	}

	public CsGuildMissionQuest GuildMissionQuest
	{
		get { return CsGameData.Instance.GuildMissionQuest; }
	}

	public CsGuildMission GuildMission
	{
		get { return m_csGuildMission; }
		set { m_csGuildMission = value; }
	}

	public EnGuildMissionState GuildMissionState
	{
		get { return m_enGuildMissionState; }
		set { m_enGuildMissionState = value; }
	}

	public CsGuildMissionMonster GuildMissionMonster
	{
		get { return m_csGuildMissionMonster; }
	}

	public bool MissionQuest
	{
		get { return m_bMissionQuest; }
	}

    public bool MissionCompleted
    {
        get { return m_bMissionCompleted; }
    }

	public int MissionProgressCount
	{
		get { return m_nMissionProgressCount; }
	}

    public int MissionCompletedCount
    {
        get { return m_nMissionCompletedCount; }
    }

    public bool Auto
	{
		get { return m_bAuto; }
		set { m_bAuto = value; }
	}

	public bool Interaction
	{
		get { return m_bInteraction || m_bSpellInjection; }
	}

	public int GuildMoralPoint
	{
		get { return m_nGuildMoralPoint; }
		set { m_nGuildMoralPoint = value; }
	}

	public DateTime GuildMoralPointDate
	{
		get { return m_dtGuildMoralPointDate; }
		set { m_dtGuildMoralPointDate = value; }
	}

	public long DefenseMonsterInstanceId
	{
		get { return m_lDefenseMonsterInstanceId; }
	}

	public Vector3 DefenseMonsterPos
	{
		get { return m_vtDefenseMonsterPos; }
	}		

	public float GuildAltarDefenseMissionRemainingCoolTime
	{
		get { return m_flGuildAltarDefenseMissionRemainingCoolTime; }
		set { m_flGuildAltarDefenseMissionRemainingCoolTime = value + Time.realtimeSinceStartup; }
	}

	public DateTime GuildAltarRewardReceivedDate
	{
		get { return m_dtGuildAltarRewardReceivedDate; }
		set { m_dtGuildAltarRewardReceivedDate = value; }
	}

	public bool AltarEnter
    {
		get { return m_bAltarEnter; }
		set { m_bAltarEnter = value; }
    }

    public bool GoToGuildFoodWareHouse
    {
        get { return m_bGoToGuildFoodWareHouse; }
        set { m_bGoToGuildFoodWareHouse = value; }
    }

    public EnGuildPlayState GuildPlayAutoState
    {
        get { return m_enGuildPlayAutoState; }
        set { m_enGuildPlayAutoState = value; }
    }

	public bool IsGuildDefense
	{
		get { return m_bGuildDefense; }
	}

    public bool IsGoAutoAltar
    {
        get { return m_bGoAutoAltar; }
    }

	public DateTime GuildDailyRewardReceivedDate
	{
		get { return m_dtGuildDailyRewardReceivedDate; }
		set { m_dtGuildDailyRewardReceivedDate = value; }
	}

	public List<CsHeroGuildSkill> HeroGuildSkillList
	{
		get { return m_listCsHeroGuildSkill; }
	}

	public CsGuildSupplySupportQuest GuildSupplySupportQuest
	{
		get { return CsGameData.Instance.GuildSupplySupportQuest; }
	}

	public CsGuildSupplySupportQuestPlay GuildSupplySupportQuestPlay
	{
		get { return m_csGuildSupplySupportQuestPlay; }
		set { m_csGuildSupplySupportQuestPlay = value; }
	}

	public float GuildSupplySupportQuestRemainingTime
	{
		get { return m_flGuildSupplySupportQuestRemainingTime; }
		set { m_flGuildSupplySupportQuestRemainingTime = value; }
	}

	public EnGuildSupplySupportState GuildSupplySupportState
	{
		get { return m_enGuildSupplySupportState; }
	}

    public int DailyGuildSupplySupportQuestStartCount
    {
        get { return m_nDailyGuildSupplySupportQuestStartCount; }
        set { m_nDailyGuildSupplySupportQuestStartCount = value; }
    }

    public DateTime DailyGuildSupplySupportQuestStartCountDate
    {
        get { return m_dtDailyGuildSupplySupportQuestStartCountDate; }
        set { m_dtDailyGuildSupplySupportQuestStartCountDate = value; }
    }
	public CsGuildHuntingQuest GuildHuntingQuest
	{
		get { return CsGameData.Instance.GuildHuntingQuest; }
	}

	public CsHeroGuildHuntingQuest HeroGuildHuntingQuest
	{
		get { return m_csHeroGuildHuntingQuest; }
		set { m_csHeroGuildHuntingQuest = value; }
	}

	public CsGuildHuntingQuestObjective GuildHuntingQuestObjective
	{
		get { return m_csGuildHuntingQuestObjective; }
		set { m_csGuildHuntingQuestObjective = value; }
	}

	public int DailyGuildHuntingQuestStartCount
	{
		get { return m_nDailyGuildHuntingQuestStartCount; }
		set { m_nDailyGuildHuntingQuestStartCount = value; }
	}

	public DateTime GuildHuntingQuestStartCountDate
	{
		get { return m_dtGuildHuntingQuestStartCountDate; }
		set { m_dtGuildHuntingQuestStartCountDate = value; }
	}

	public DateTime GuildHuntingDonationDate
	{
		get { return m_dtGuildHuntingDonationDate; }
		set { m_dtGuildHuntingDonationDate = value; }
	}

	public DateTime GuildHuntingDonationRewardReceivedDate
	{
		get { return m_dtGuildHuntingDonationRewardReceivedDate; }
		set { m_dtGuildHuntingDonationRewardReceivedDate = value; }
	}

	public int GuildDailyObjectiveRewardReceivedNo
	{
		get { return m_nGuildDailyObjectiveRewardReceivedNo; }
		set { m_nGuildDailyObjectiveRewardReceivedNo = value; }
	}

	public DateTime GuildDailyObjectiveRewardReceivedDate
	{
		get { return m_dtGuildDailyObjectiveRewardReceivedDate; }
		set { m_dtGuildDailyObjectiveRewardReceivedDate = value; }
	}

	public DateTime GuildWeeklyObjectiveRewardReceivedDate
	{
		get { return m_dtGuildWeeklyObjectiveRewardReceivedDate; }
		set { m_dtGuildWeeklyObjectiveRewardReceivedDate = value; }
	}

	public float GuildDailyObjectiveNoticeRemainingCoolTime
	{
		get { return m_flGuildDailyObjectiveNoticeRemainingCoolTime; }
		set { m_flGuildDailyObjectiveNoticeRemainingCoolTime = Time.realtimeSinceStartup + value; }
	}

	public EnGuildHuntingState GuildHuntingState
	{
		get { return m_enGuildHuntingState; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDGuild guild, PDHeroGuildSkill[] heroGuildSkills, int nGuildMemberGrade, int nTotalGuildContributionPoint, int nGuildContributionPoint,
					int nGuildPoint, float flGuildRejoinRemainingTime, PDHeroGuildApplication[] guildApplications, int nDailyGuildApplicationCount, DateTime dtDate,
					int nDailyGuildDonationCount, int nDailyGuildFarmQuestStartCount, PDHeroGuildFarmQuest heroGuildFarmQuest, int nDailyGuildFoodWarehouseStockCount, 
					Guid guidReceivedGuildFoodWarehouseCollectionId, int nGuildMoralPoint, float flGuildAltarDefenseMissionRemainingCoolTime, DateTime dtGuildAltarRewardReceivedDate, 
					PDHeroGuildMissionQuest heroGuildMissionQuest, DateTime dtGuildDailyRewardReceivedDate, PDGuildSupplySupportQuestPlay guildSupplySupportQuestPlay, 
					PDHeroGuildHuntingQuest guildHuntingQuest, int nDailyGuildHuntingQuestStartCount, DateTime guildHuntingDonationDate, DateTime guildHuntingDonationRewardReceivedDate, 
					int nGuildDailyObjectiveRewardReceivedNo, DateTime guildWeeklyObjectiveRewardReceivedDate, float flGuildDailyObjectiveNoticeRemainingCoolTime)
	{
		UnInit();

		if (guild != null)
		{
			m_csGuild = new CsGuild(guild);
		}
		else
		{
			m_csGuild = null;
		}

		m_listCsHeroGuildSkill = new List<CsHeroGuildSkill>();

		for (int i = 0; i < heroGuildSkills.Length; i++)
		{
			m_listCsHeroGuildSkill.Add(new CsHeroGuildSkill(heroGuildSkills[i]));
		}

        m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(nGuildMemberGrade);
		m_nTotalGuildContributionPoint = nTotalGuildContributionPoint;
		m_nGuildContributionPoint = nGuildContributionPoint;
		m_nGuldPoint = nGuildPoint;

		m_flGuildRejoinRemainingTime = flGuildRejoinRemainingTime + Time.realtimeSinceStartup;

		m_nDailyGuildApplicationCount = nDailyGuildApplicationCount;
		m_dtGuildApplicationCountDate = dtDate;

		m_nDailyGuildDonationCount = nDailyGuildDonationCount;
		m_dtGuildDonationCountDate = dtDate;
		
		m_listCsHeroGuildApplication = new List<CsHeroGuildApplication>();

		for (int i = 0; i < guildApplications.Length; i++)
		{
			m_listCsHeroGuildApplication.Add(new CsHeroGuildApplication(guildApplications[i]));
		}


		m_nDailyGuildFoodWarehouseStockCount = nDailyGuildFoodWarehouseStockCount;
		m_dtGuildFoodWarehouseStockCountDate = dtDate;
		m_guidReceivedGuildFoodWarehouseCollectionId = guidReceivedGuildFoodWarehouseCollectionId;

		m_nGuildMoralPoint = nGuildMoralPoint;
		m_dtGuildMoralPointDate = dtDate;

		m_flGuildAltarDefenseMissionRemainingCoolTime = flGuildAltarDefenseMissionRemainingCoolTime + Time.realtimeSinceStartup;
		if (flGuildAltarDefenseMissionRemainingCoolTime != 0)
		{
			m_bGuildDefense = true;
		}

		m_dtGuildAltarRewardReceivedDate = dtGuildAltarRewardReceivedDate;
		m_dtGuildDailyRewardReceivedDate = dtGuildDailyRewardReceivedDate;

		SetGuildFarmQuestInfo(heroGuildFarmQuest, nDailyGuildFarmQuestStartCount);	// 길드농장.
		SetGuildMission(heroGuildMissionQuest);										// 길드미션.
		SetGuildSupplySupport(guildSupplySupportQuestPlay);							// 길드물자.
		SetGuildHunting(guildHuntingQuest);											// 길드헌팅

		if (guildHuntingQuest != null)
		{
			m_csHeroGuildHuntingQuest = new CsHeroGuildHuntingQuest(guildHuntingQuest);
		}

		m_nDailyGuildHuntingQuestStartCount = nDailyGuildHuntingQuestStartCount;
		m_dtGuildHuntingQuestStartCountDate = dtDate;

		m_dtGuildHuntingDonationDate = guildHuntingDonationDate;
		m_dtGuildHuntingDonationRewardReceivedDate = guildHuntingDonationRewardReceivedDate;

		m_nGuildDailyObjectiveRewardReceivedNo = nGuildDailyObjectiveRewardReceivedNo;
		m_dtGuildDailyObjectiveRewardReceivedDate = dtDate;
		m_dtGuildWeeklyObjectiveRewardReceivedDate = guildWeeklyObjectiveRewardReceivedDate;

		m_flGuildDailyObjectiveNoticeRemainingCoolTime = Time.realtimeSinceStartup + flGuildDailyObjectiveNoticeRemainingCoolTime;

		if (m_csGuild != null)
		{
			m_nDailyGuildSupplySupportQuestStartCount = guild.dailyGuildSupplySupportQuestStartCount;
			m_dtDailyGuildSupplySupportQuestStartCountDate = guild.dailyGuildSupplySupportQuestStartDate;
		}

		// Command
		CsRplzSession.Instance.EventResGuildList += OnEventResGuildList;
		CsRplzSession.Instance.EventResGuildCreate += OnEventResGuildCreate;
		CsRplzSession.Instance.EventResGuildApply += OnEventResGuildApply;
		CsRplzSession.Instance.EventResGuildMemberTabInfo += OnEventResGuildMemberTabInfo;
		CsRplzSession.Instance.EventResGuildApplicationList += OnEventResGuildApplicationList;
		CsRplzSession.Instance.EventResGuildApplicationAccept += OnEventResGuildApplicationAccept;
		CsRplzSession.Instance.EventResGuildApplicationRefuse += OnEventResGuildApplicationRefuse;
		CsRplzSession.Instance.EventResGuildExit += OnEventResGuildExit;
		CsRplzSession.Instance.EventResGuildMemberBanish += OnEventResGuildMemberBanish;
		CsRplzSession.Instance.EventResGuildInvite += OnEventResGuildInvite;
		CsRplzSession.Instance.EventResGuildInvitationAccept += OnEventResGuildInvitationAccept;
		CsRplzSession.Instance.EventResGuildInvitationRefuse += OnEventResGuildInvitationRefuse;
		CsRplzSession.Instance.EventResGuildNoticeSet += OnEventResGuildNoticeSet;
		CsRplzSession.Instance.EventResGuildAppoint += OnEventResGuildAppoint;
		CsRplzSession.Instance.EventResGuildMasterTransfer += OnEventResGuildMasterTransfer;
		CsRplzSession.Instance.EventResGuildDonate += OnEventResGuildDonate;
		CsRplzSession.Instance.EventResGuildBuildingLevelUp += OnEventResGuildBuildingLevelUp;
		CsRplzSession.Instance.EventResGuildSkillLevelUp += OnEventResGuildSkillLevelUp;
		CsRplzSession.Instance.EventResContinentExitForGuildTerritoryEnter += OnEventResContinentExitForGuildTerritoryEnter;
		CsRplzSession.Instance.EventResGuildTerritoryEnter += OnEventResGuildTerritoryEnter;
		CsRplzSession.Instance.EventResGuildTerritoryExit += OnEventResGuildTerritoryExit;
		CsRplzSession.Instance.EventResGuildFarmQuestAccept += OnEventResGuildFarmQuestAccept;
		CsRplzSession.Instance.EventResGuildFarmQuestInteractionStart += OnEventResGuildFarmQuestInteractionStart;
		CsRplzSession.Instance.EventResGuildFarmQuestComplete += OnEventResGuildFarmQuestComplete;
		CsRplzSession.Instance.EventResGuildFarmQuestAbandon += OnEventResGuildFarmQuestAbandon;
		CsRplzSession.Instance.EventResGuildFoodWarehouseInfo += OnEventResGuildFoodWarehouseInfo;
		CsRplzSession.Instance.EventResGuildFoodWarehouseStock += OnEventResGuildFoodWarehouseStock;
		CsRplzSession.Instance.EventResGuildFoodWarehouseCollect += OnEventResGuildFoodWarehouseCollect;
		CsRplzSession.Instance.EventResGuildFoodWarehouseRewardReceive += OnEventResGuildFoodWarehouseRewardReceive;
		CsRplzSession.Instance.EventResGuildAltarDonate += OnEventResGuildAltarDonate;
		CsRplzSession.Instance.EventResGuildAltarRewardReceive += OnEventResGuildAltarRewardReceive;
        CsRplzSession.Instance.EventResGuildAltarSpellInjectionMissionStart += OnEventResGuildAltarSpellInjectionMissionStart;
        CsRplzSession.Instance.EventResGuildAltarDefenseMissionStart += OnEventResGuildAltarDefenseMissionStart;
		CsRplzSession.Instance.EventResGuildMissionQuestAccept += OnEventResGuildMissionQuestAccept;
		CsRplzSession.Instance.EventResGuildMissionAccept += OnEventResGuildMissionAccept;
		CsRplzSession.Instance.EventResGuildMissionAbandon += OnEventResGuildMissionAbandon;
		CsRplzSession.Instance.EventResGuildMissionTargetNpcInteract += OnEventResGuildMissionTargetNpcInteract;
		CsRplzSession.Instance.EventResServerGuildRanking += OnEventResServerGuildRanking;
		CsRplzSession.Instance.EventResNationGuildRanking += OnEventResNationGuildRanking;
		CsRplzSession.Instance.EventResGuildDailyRewardReceive += OnEventResGuildDailyRewardReceive;
		CsRplzSession.Instance.EventResGuildCall += OnEventResGuildCall; 
		CsRplzSession.Instance.EventResGuildCallTransmission += OnEventResGuildCallTransmission;
        CsRplzSession.Instance.EventResContinentEnterForGuildCallTransmission += OnEventResContinentEnterForGuildCallTransmission;
        CsRplzSession.Instance.EventResGuildSupplySupportQuestAccept += OnEventResGuildSupplySupportQuestAccept;
        CsRplzSession.Instance.EventResGuildSupplySupportQuestComplete += OnEventResGuildSupplySupportQuestComplete;
		CsRplzSession.Instance.EventResGuildHuntingQuestAccept += OnEventResGuildHuntingQuestAccept;
		CsRplzSession.Instance.EventResGuildHuntingQuestAbandon += OnEventResGuildHuntingQuestAbandon;
		CsRplzSession.Instance.EventResGuildHuntingQuestComplete += OnEventResGuildHuntingQuestComplete;
		CsRplzSession.Instance.EventResGuildHuntingDonate += OnEventResGuildHuntingDonate;
		CsRplzSession.Instance.EventResGuildHuntingDonationRewardReceive += OnEventResGuildHuntingDonationRewardReceive;
		CsRplzSession.Instance.EventResGuildDailyObjectiveNotice += OnEventResGuildDailyObjectiveNotice;
		CsRplzSession.Instance.EventResGuildDailyObjectiveCompletionMemberList += OnEventResGuildDailyObjectiveCompletionMemberList;
		CsRplzSession.Instance.EventResGuildDailyObjectiveRewardReceive += OnEventResGuildDailyObjectiveRewardReceive;
		CsRplzSession.Instance.EventResGuildWeeklyObjectiveSet += OnEventResGuildWeeklyObjectiveSet;
		CsRplzSession.Instance.EventResGuildWeeklyObjectiveRewardReceive += OnEventResGuildWeeklyObjectiveRewardReceive;
		CsRplzSession.Instance.EventResGuildTerritoryRevive += OnEventResGuildTerritoryRevive;
		CsRplzSession.Instance.EventResGuildTerritoryEnterForGuildTerritoryRevival += OnEventResGuildTerritoryEnterForGuildTerritoryRevival;
		CsRplzSession.Instance.EventResGuildBlessingBuffStart += OnEventResGuildBlessingBuffStart;

		// Event
		CsRplzSession.Instance.EventEvtGuildApplicationAccepted += OnEventEvtGuildApplicationAccepted;
		CsRplzSession.Instance.EventEvtGuildApplicationRefused += OnEventEvtGuildApplicationRefused;
		CsRplzSession.Instance.EventEvtGuildApplicationCountUpdated += OnEventEvtGuildApplicationCountUpdated;
		CsRplzSession.Instance.EventEvtGuildMemberEnter += OnEventEvtGuildMemberEnter;
		CsRplzSession.Instance.EventEvtGuildMemberExit += OnEventEvtGuildMemberExit;
		CsRplzSession.Instance.EventEvtGuildBanished += OnEventEvtGuildBanished;
		CsRplzSession.Instance.EventEvtHeroGuildInfoUpdated += OnEventEvtHeroGuildInfoUpdated;
		CsRplzSession.Instance.EventEvtGuildInvitationArrived += OnEventEvtGuildInvitationArrived;
		CsRplzSession.Instance.EventEvtGuildInvitationRefused += OnEventEvtGuildInvitationRefused;
		CsRplzSession.Instance.EventEvtGuildInvitationLifetimeEnded += OnEventEvtGuildInvitationLifetimeEnded;
		CsRplzSession.Instance.EventEvtGuildAppointed += OnEventEvtGuildAppointed;
		CsRplzSession.Instance.EventEvtGuildMasterTransferred += OnEventEvtGuildMasterTransferred;
		CsRplzSession.Instance.EventEvtGuildBuildingLevelUp += OnEventEvtGuildBuildingLevelUp;
		CsRplzSession.Instance.EventEvtGuildFoodWarehouseCollected += OnEventEvtGuildFoodWarehouseCollected;
		CsRplzSession.Instance.EventEvtGuildFarmQuestInteractionCompleted += OnEventEvtGuildFarmQuestInteractionCompleted;
		CsRplzSession.Instance.EventEvtGuildFarmQuestInteractionCanceled += OnEventEvtGuildFarmQuestInteractionCanceled;
		CsRplzSession.Instance.EventEvtHeroGuildFarmQuestInteractionStarted += OnEventEvtHeroGuildFarmQuestInteractionStarted;
		CsRplzSession.Instance.EventEvtHeroGuildFarmQuestInteractionCompleted += OnEventEvtHeroGuildFarmQuestInteractionCompleted;
		CsRplzSession.Instance.EventEvtHeroGuildFarmQuestInteractionCanceled += OnEventEvtHeroGuildFarmQuestInteractionCanceled;
		CsRplzSession.Instance.EventEvtGuildNoticeChanged += OnEventEvtGuildNoticeChanged;
		CsRplzSession.Instance.EventEvtGuildFundChanged += OnEventEvtGuildFundChanged;
		CsRplzSession.Instance.EventEvtGuildMoralPointChanged += OnEventEvtGuildMoralPointChanged;
        CsRplzSession.Instance.EventEvtGuildAltarSpellInjectionMissionCompleted += OnEventEvtGuildAltarSpellInjectionMissionCompleted;
        CsRplzSession.Instance.EventEvtGuildAltarSpellInjectionMissionCanceled += OnEventEvtGuildAltarSpellInjectionMissionCanceled;
        CsRplzSession.Instance.EventEvtHeroGuildAltarSpellInjectionMissionStarted += OnEventEvtHeroGuildAltarSpellInjectionMissionStarted;
        CsRplzSession.Instance.EventEvtHeroGuildAltarSpellInjectionMissionCompleted += OnEventEvtHeroGuildAltarSpellInjectionMissionCompleted;
        CsRplzSession.Instance.EventEvtHeroGuildAltarSpellInjectionMissionCanceled += OnEventEvtHeroGuildAltarSpellInjectionMissionCanceled;
        CsRplzSession.Instance.EventEvtGuildAltarDefenseMissionCompleted += OnEventEvtGuildAltarDefenseMissionCompleted;
        CsRplzSession.Instance.EventEvtGuildAltarDefenseMissionFailed += OnEventEvtGuildAltarDefenseMissionFailed;
		CsRplzSession.Instance.EventEvtGuildMissionFailed += OnEventEvtGuildMissionFailed;
		CsRplzSession.Instance.EventEvtGuildMissionComplete += OnEventEvtGuildMissionComplete;
		CsRplzSession.Instance.EventEvtGuildMissionUpdated += OnEventEvtGuildMissionUpdated;
		CsRplzSession.Instance.EventEvtGuildSpiritAnnounced += OnEventEvtGuildSpiritAnnounced;
		CsRplzSession.Instance.EventEvtGuildCall += OnEventEvtGuildCall;
        CsRplzSession.Instance.EventEvtGuildSupplySupportQuestStarted += OnEventEvtGuildSupplySupportQuestStarted;
        CsRplzSession.Instance.EventEvtGuildSupplySupportQuestCompleted += OnEventEvtGuildSupplySupportQuestCompleted;
        CsRplzSession.Instance.EventEvtGuildSupplySupportQuestFail += OnEventEvtGuildSupplySupportQuestFail;
		CsRplzSession.Instance.EventEvtGuildHuntingQuestUpdated += OnEventEvtGuildHuntingQuestUpdated;
		CsRplzSession.Instance.EventEvtGuildHuntingDonationCountUpdated += OnEventEvtGuildHuntingDonationCountUpdated;
		CsRplzSession.Instance.EventEvtGuildDailyObjectiveNotice += OnEventEvtGuildDailyObjectiveNotice;
		CsRplzSession.Instance.EventEvtGuildDailyObjectiveSet += OnEventEvtGuildDailyObjectiveSet;
		CsRplzSession.Instance.EventEvtGuildDailyObjectiveCompletionMemberCountUpdated += OnEventEvtGuildDailyObjectiveCompletionMemberCountUpdated;
		CsRplzSession.Instance.EventEvtGuildWeeklyObjectiveSet += OnEventEvtGuildWeeklyObjectiveSet;
        CsRplzSession.Instance.EventEvtGuildWeeklyObjectiveCompletionMemberCountUpdated += OnEventEvtGuildWeeklyObjectiveCompletionMemberCountUpdated;
        CsRplzSession.Instance.EventEvtGuildAltarCompleted += OnEventEvtGuildAltarCompleted;
		CsRplzSession.Instance.EventEvtGuildBlessingBuffStarted += OnEventEvtGuildBlessingBuffStarted;
		CsRplzSession.Instance.EventEvtGuildBlessingBuffEnded += OnEventEvtGuildBlessingBuffEnded;

	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResGuildList -= OnEventResGuildList;
		CsRplzSession.Instance.EventResGuildCreate -= OnEventResGuildCreate;
		CsRplzSession.Instance.EventResGuildApply -= OnEventResGuildApply;
		CsRplzSession.Instance.EventResGuildMemberTabInfo -= OnEventResGuildMemberTabInfo;
		CsRplzSession.Instance.EventResGuildApplicationList -= OnEventResGuildApplicationList;
		CsRplzSession.Instance.EventResGuildApplicationAccept -= OnEventResGuildApplicationAccept;
		CsRplzSession.Instance.EventResGuildApplicationRefuse -= OnEventResGuildApplicationRefuse;
		CsRplzSession.Instance.EventResGuildExit -= OnEventResGuildExit;
		CsRplzSession.Instance.EventResGuildMemberBanish -= OnEventResGuildMemberBanish;
		CsRplzSession.Instance.EventResGuildInvite -= OnEventResGuildInvite;
		CsRplzSession.Instance.EventResGuildInvitationAccept -= OnEventResGuildInvitationAccept;
		CsRplzSession.Instance.EventResGuildInvitationRefuse -= OnEventResGuildInvitationRefuse;
		CsRplzSession.Instance.EventResGuildNoticeSet -= OnEventResGuildNoticeSet;
		CsRplzSession.Instance.EventResGuildAppoint -= OnEventResGuildAppoint;
		CsRplzSession.Instance.EventResGuildMasterTransfer -= OnEventResGuildMasterTransfer;
		CsRplzSession.Instance.EventResGuildDonate -= OnEventResGuildDonate;
		CsRplzSession.Instance.EventResGuildBuildingLevelUp -= OnEventResGuildBuildingLevelUp;
		CsRplzSession.Instance.EventResGuildSkillLevelUp -= OnEventResGuildSkillLevelUp;
		CsRplzSession.Instance.EventResContinentExitForGuildTerritoryEnter -= OnEventResContinentExitForGuildTerritoryEnter;
		CsRplzSession.Instance.EventResGuildTerritoryEnter -= OnEventResGuildTerritoryEnter;
		CsRplzSession.Instance.EventResGuildTerritoryExit -= OnEventResGuildTerritoryExit;
		CsRplzSession.Instance.EventResGuildFarmQuestAccept -= OnEventResGuildFarmQuestAccept;
		CsRplzSession.Instance.EventResGuildFarmQuestInteractionStart -= OnEventResGuildFarmQuestInteractionStart;
		CsRplzSession.Instance.EventResGuildFarmQuestComplete -= OnEventResGuildFarmQuestComplete;
		CsRplzSession.Instance.EventResGuildFarmQuestAbandon -= OnEventResGuildFarmQuestAbandon;
		CsRplzSession.Instance.EventResGuildFoodWarehouseInfo -= OnEventResGuildFoodWarehouseInfo;
		CsRplzSession.Instance.EventResGuildFoodWarehouseStock -= OnEventResGuildFoodWarehouseStock;
		CsRplzSession.Instance.EventResGuildFoodWarehouseCollect -= OnEventResGuildFoodWarehouseCollect;
		CsRplzSession.Instance.EventResGuildFoodWarehouseRewardReceive -= OnEventResGuildFoodWarehouseRewardReceive;
		CsRplzSession.Instance.EventResGuildAltarDonate -= OnEventResGuildAltarDonate;
		CsRplzSession.Instance.EventResGuildAltarRewardReceive -= OnEventResGuildAltarRewardReceive;
        CsRplzSession.Instance.EventResGuildAltarSpellInjectionMissionStart -= OnEventResGuildAltarSpellInjectionMissionStart;
        CsRplzSession.Instance.EventResGuildAltarDefenseMissionStart -= OnEventResGuildAltarDefenseMissionStart;
		CsRplzSession.Instance.EventResGuildMissionQuestAccept -= OnEventResGuildMissionQuestAccept;
		CsRplzSession.Instance.EventResGuildMissionAccept -= OnEventResGuildMissionAccept;
		CsRplzSession.Instance.EventResGuildMissionAbandon -= OnEventResGuildMissionAbandon;
		CsRplzSession.Instance.EventResGuildMissionTargetNpcInteract -= OnEventResGuildMissionTargetNpcInteract;
		CsRplzSession.Instance.EventResServerGuildRanking -= OnEventResServerGuildRanking;
		CsRplzSession.Instance.EventResNationGuildRanking -= OnEventResNationGuildRanking;
		CsRplzSession.Instance.EventResGuildDailyRewardReceive -= OnEventResGuildDailyRewardReceive;
		CsRplzSession.Instance.EventResGuildCall -= OnEventResGuildCall; 
		CsRplzSession.Instance.EventResGuildCallTransmission -= OnEventResGuildCallTransmission;
        CsRplzSession.Instance.EventResContinentEnterForGuildCallTransmission -= OnEventResContinentEnterForGuildCallTransmission;
        CsRplzSession.Instance.EventResGuildSupplySupportQuestAccept -= OnEventResGuildSupplySupportQuestAccept;
        CsRplzSession.Instance.EventResGuildSupplySupportQuestComplete -= OnEventResGuildSupplySupportQuestComplete;
		CsRplzSession.Instance.EventResGuildHuntingQuestAccept -= OnEventResGuildHuntingQuestAccept;
		CsRplzSession.Instance.EventResGuildHuntingQuestAbandon -= OnEventResGuildHuntingQuestAbandon;
		CsRplzSession.Instance.EventResGuildHuntingQuestComplete -= OnEventResGuildHuntingQuestComplete;
		CsRplzSession.Instance.EventResGuildHuntingDonate -= OnEventResGuildHuntingDonate;
		CsRplzSession.Instance.EventResGuildHuntingDonationRewardReceive -= OnEventResGuildHuntingDonationRewardReceive;
		CsRplzSession.Instance.EventResGuildDailyObjectiveNotice -= OnEventResGuildDailyObjectiveNotice;
		CsRplzSession.Instance.EventResGuildDailyObjectiveCompletionMemberList -= OnEventResGuildDailyObjectiveCompletionMemberList;
		CsRplzSession.Instance.EventResGuildDailyObjectiveRewardReceive -= OnEventResGuildDailyObjectiveRewardReceive;
		CsRplzSession.Instance.EventResGuildWeeklyObjectiveSet -= OnEventResGuildWeeklyObjectiveSet;
		CsRplzSession.Instance.EventResGuildWeeklyObjectiveRewardReceive -= OnEventResGuildWeeklyObjectiveRewardReceive;
		CsRplzSession.Instance.EventResGuildTerritoryRevive -= OnEventResGuildTerritoryRevive;
		CsRplzSession.Instance.EventResGuildTerritoryEnterForGuildTerritoryRevival -= OnEventResGuildTerritoryEnterForGuildTerritoryRevival;
		CsRplzSession.Instance.EventResGuildBlessingBuffStart -= OnEventResGuildBlessingBuffStart;

		// Event
		CsRplzSession.Instance.EventEvtGuildApplicationAccepted -= OnEventEvtGuildApplicationAccepted;
		CsRplzSession.Instance.EventEvtGuildApplicationRefused -= OnEventEvtGuildApplicationRefused;
		CsRplzSession.Instance.EventEvtGuildApplicationCountUpdated -= OnEventEvtGuildApplicationCountUpdated;
		CsRplzSession.Instance.EventEvtGuildMemberEnter -= OnEventEvtGuildMemberEnter;
		CsRplzSession.Instance.EventEvtGuildMemberExit -= OnEventEvtGuildMemberExit;
		CsRplzSession.Instance.EventEvtGuildBanished -= OnEventEvtGuildBanished;
		CsRplzSession.Instance.EventEvtHeroGuildInfoUpdated -= OnEventEvtHeroGuildInfoUpdated;
		CsRplzSession.Instance.EventEvtGuildInvitationArrived -= OnEventEvtGuildInvitationArrived;
		CsRplzSession.Instance.EventEvtGuildInvitationRefused -= OnEventEvtGuildInvitationRefused;
		CsRplzSession.Instance.EventEvtGuildInvitationLifetimeEnded -= OnEventEvtGuildInvitationLifetimeEnded;
		CsRplzSession.Instance.EventEvtGuildAppointed -= OnEventEvtGuildAppointed;
		CsRplzSession.Instance.EventEvtGuildMasterTransferred -= OnEventEvtGuildMasterTransferred;
		CsRplzSession.Instance.EventEvtGuildBuildingLevelUp -= OnEventEvtGuildBuildingLevelUp;
		CsRplzSession.Instance.EventEvtGuildFoodWarehouseCollected -= OnEventEvtGuildFoodWarehouseCollected;
		CsRplzSession.Instance.EventEvtGuildNoticeChanged -= OnEventEvtGuildNoticeChanged;
		CsRplzSession.Instance.EventEvtGuildFundChanged -= OnEventEvtGuildFundChanged;
		CsRplzSession.Instance.EventEvtGuildMoralPointChanged -= OnEventEvtGuildMoralPointChanged;
        CsRplzSession.Instance.EventEvtGuildAltarSpellInjectionMissionCompleted -= OnEventEvtGuildAltarSpellInjectionMissionCompleted;
        CsRplzSession.Instance.EventEvtGuildAltarSpellInjectionMissionCanceled -= OnEventEvtGuildAltarSpellInjectionMissionCanceled;
        CsRplzSession.Instance.EventEvtHeroGuildAltarSpellInjectionMissionStarted -= OnEventEvtHeroGuildAltarSpellInjectionMissionStarted;
        CsRplzSession.Instance.EventEvtHeroGuildAltarSpellInjectionMissionCompleted -= OnEventEvtHeroGuildAltarSpellInjectionMissionCompleted;
        CsRplzSession.Instance.EventEvtHeroGuildAltarSpellInjectionMissionCanceled -= OnEventEvtHeroGuildAltarSpellInjectionMissionCanceled;
        CsRplzSession.Instance.EventEvtGuildAltarDefenseMissionCompleted -= OnEventEvtGuildAltarDefenseMissionCompleted;
        CsRplzSession.Instance.EventEvtGuildAltarDefenseMissionFailed -= OnEventEvtGuildAltarDefenseMissionFailed;
		CsRplzSession.Instance.EventEvtGuildMissionFailed -= OnEventEvtGuildMissionFailed;
		CsRplzSession.Instance.EventEvtGuildMissionComplete -= OnEventEvtGuildMissionComplete;
		CsRplzSession.Instance.EventEvtGuildMissionUpdated -= OnEventEvtGuildMissionUpdated;
		CsRplzSession.Instance.EventEvtGuildSpiritAnnounced -= OnEventEvtGuildSpiritAnnounced;
		CsRplzSession.Instance.EventEvtGuildCall -= OnEventEvtGuildCall;
        CsRplzSession.Instance.EventEvtGuildSupplySupportQuestStarted -= OnEventEvtGuildSupplySupportQuestStarted;
        CsRplzSession.Instance.EventEvtGuildSupplySupportQuestCompleted -= OnEventEvtGuildSupplySupportQuestCompleted;
        CsRplzSession.Instance.EventEvtGuildSupplySupportQuestFail -= OnEventEvtGuildSupplySupportQuestFail;
		CsRplzSession.Instance.EventEvtGuildHuntingQuestUpdated -= OnEventEvtGuildHuntingQuestUpdated;
		CsRplzSession.Instance.EventEvtGuildHuntingDonationCountUpdated -= OnEventEvtGuildHuntingDonationCountUpdated;
		CsRplzSession.Instance.EventEvtGuildDailyObjectiveNotice -= OnEventEvtGuildDailyObjectiveNotice;
		CsRplzSession.Instance.EventEvtGuildDailyObjectiveSet -= OnEventEvtGuildDailyObjectiveSet;
		CsRplzSession.Instance.EventEvtGuildDailyObjectiveCompletionMemberCountUpdated -= OnEventEvtGuildDailyObjectiveCompletionMemberCountUpdated;
		CsRplzSession.Instance.EventEvtGuildWeeklyObjectiveSet -= OnEventEvtGuildWeeklyObjectiveSet;
		CsRplzSession.Instance.EventEvtGuildWeeklyObjectiveCompletionMemberCountUpdated -= OnEventEvtGuildWeeklyObjectiveCompletionMemberCountUpdated;
        CsRplzSession.Instance.EventEvtGuildAltarCompleted -= OnEventEvtGuildAltarCompleted;
		CsRplzSession.Instance.EventEvtGuildBlessingBuffStarted -= OnEventEvtGuildBlessingBuffStarted;
		CsRplzSession.Instance.EventEvtGuildBlessingBuffEnded -= OnEventEvtGuildBlessingBuffEnded;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_enGuildPlayAutoState = EnGuildPlayState.None;
		m_bInteraction = false;
		m_bSpellInjection = false;
		m_bFarmQuestMissionExcuted = false;
		m_bGoToGuildFoodWareHouse = false;
		m_bGuildDefense = false;
		m_bGoAutoAltar = false;
		m_bAltarEnter = false;

		m_guidAppId = Guid.Empty;
		m_guidBanishMemberId = Guid.Empty;
		m_guidInvitationId = Guid.Empty;
		//m_guidTargetMemberId = Guid.Empty;
		m_csGuild = null;
		m_listCsHeroGuildSkill = null;
		m_csGuildMemberGrade = null;
		m_listCsHeroGuildApplication = null;

		m_listCsGuildMember.Clear();

		m_listCsGuildApplication.Clear();
		m_listCsHeroGuildInvitation.Clear();
		m_guidReceivedGuildFoodWarehouseCollectionId = Guid.Empty;

		m_csHeroGuildFarmQuest = null;
		m_enGuildFarmQuestState = EnGuildFarmQuestState.None;

		m_csGuildMission = null;
		m_csGuildMissionMonster = null;
		m_enGuildMissionState = EnGuildMissionState.None;

		m_csGuildSupplySupportQuestPlay = null;	// 길드물자지원퀘스트플레이. 없으면 null
		m_enGuildSupplySupportState = EnGuildSupplySupportState.None;

		m_csHeroGuildHuntingQuest = null;
		m_csGuildHuntingQuestObjective = null;
		m_enGuildHuntingState = EnGuildHuntingState.None;
	}

	//---------------------------------------------------------------------------------------------------
	// 오토 시작
	public void StartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
	{
		if (m_bWaitResponse) return;

		if (m_enGuildPlayAutoState != enGuildPlayAutoState)
		{
			if (m_bAuto)
			{
				if (EventStopAutoPlay != null)
				{
					EventStopAutoPlay(null, m_enGuildPlayAutoState);
				}
			}

			m_bAuto = true;
			m_enGuildPlayAutoState = enGuildPlayAutoState;
			Debug.Log("StartAutoPlay  enGuildPlayAutoState = " + enGuildPlayAutoState);

			if (EventStartAutoPlay != null)
			{
				EventStartAutoPlay(enGuildPlayAutoState);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 오토 정지
	public void StopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
	{
		if (m_bAuto)
		{
			m_bAuto = false;
			m_enGuildPlayAutoState = EnGuildPlayState.None;
			Debug.Log("StopAutoPlay()  enGuildPlayAutoState = " + enGuildPlayAutoState);
			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller, enGuildPlayAutoState);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset()
	{
		m_csGuild = null;
        m_csGuildMemberGrade = null;

		m_nTotalGuildContributionPoint = 0;
		m_nGuildContributionPoint = 0;
		m_nGuldPoint = 0;
		m_nDailyGuildDonationCount = 0;
		m_nGuildMoralPoint = 0;

		m_listCsGuildMember.Clear();
		m_listCsGuildApplication.Clear();

		m_csHeroGuildFarmQuest = null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsGuildMissionQuestObject(int nObjectId)
	{
		return (m_enGuildMissionState == EnGuildMissionState.Accepted && GuildMission != null && GuildMission.ContinentObjectTarget != null && GuildMission.ContinentObjectTarget.ObjectId == nObjectId);
	}
		
	//---------------------------------------------------------------------------------------------------
	public bool IsGuildHuntingQuestObject(int nObjectId)
	{
		return (m_enGuildHuntingState == EnGuildHuntingState.Accepted && m_csGuildHuntingQuestObjective != null && m_csGuildHuntingQuestObjective.TargetContinentObjectId == nObjectId);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsGuildFarmQuestInteractionNpc(int nNpcId)
	{
		return (m_enGuildFarmQuestState == EnGuildFarmQuestState.Accepted && CsGameData.Instance.GuildFarmQuest.TargetGuildTerritoryNpc.NpcId == nNpcId);
	}

	//---------------------------------------------------------------------------------------------------
	public void FishingDialog(int nNpcId)
	{
		Debug.Log("FishingDialog()");
		if (EventFishingDialog != null)
		{
			EventFishingDialog(nNpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void FoodWarehouseNpcDialog(int nNpcId)
	{
		Debug.Log("FoodWarehouseNpcDialog()");
		if (EventFoodWarehouseNpcDialog != null)
		{
			EventFoodWarehouseNpcDialog(nNpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void FarmNpcDialog(int nNpcId)
	{
		Debug.Log("FarmNpcDialog()");
		if (EventFarmNpcDialog != null)
		{
			EventFarmNpcDialog(nNpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void FarmInteraction()
	{
		SendGuildFarmQuestInteractionStart();
	}

	//---------------------------------------------------------------------------------------------------
	public void FarmInteractionArea(bool bEnter)
	{
		dd.d("GuildFarmQuestInteractionArea  bEnter = ", bEnter);
		if (EventFarmInteractionArea != null)
		{
			EventFarmInteractionArea(bEnter);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool MissionAcceptReadyOK() 
	{
		Debug.Log("MissionAcceptReadyOK()");
		if (m_bWaitResponse) return false;
		if (m_enGuildMissionState == EnGuildMissionState.None && m_nMissionCompletedCount >= GuildMissionQuest.LimitCount) return false;

		if (EventMissionAcceptDialog != null)
		{
			EventMissionAcceptDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void MissionNpcDialog()
	{
		Debug.Log("MissionNpcDialog()");
		if (EventMissionMissionDialog != null)
		{
			EventMissionMissionDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드 농장퀘스트 다이얼로그
	public bool GuildFarmQuestNPCDialog()
	{
		Debug.Log("1. GuildFarmQuestNPCDialog()");
		if (m_bWaitResponse) return false;
		if (m_enGuildFarmQuestState == EnGuildFarmQuestState.Competed) return false;
		if (m_enGuildFarmQuestState == EnGuildFarmQuestState.None && m_nDailyGuildFarmQuestStartCount >= CsGameData.Instance.GuildFarmQuest.LimitCount) return false;

		Debug.Log("2. GuildFarmQuestNPCDialog()");
		if (EventGuildFarmQuestNPCDialog != null)
		{
			EventGuildFarmQuestNPCDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	// 군량 NPC 다이얼 로그
	public void GuildFoodWareHouseDialog()
	{
		if (m_bWaitResponse) return;
		Debug.Log("GuildFoodWareHouseDialog()");
		if (EventGuildFoodWareHouseDialog != null)
		{
			EventGuildFoodWareHouseDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 제단 팝업창 표시 이벤트
	public void GuildAltarNPCDialog()
	{
		if (m_bWaitResponse) return;

		Debug.Log("GuildAltarNPCDialog()");
		if (EventGuildAltarNPCDialog != null)
		{
			EventGuildAltarNPCDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 제단과 위치 여부
	public void GuildAltarGoOut(bool bAltarEnter)
	{
		m_bAltarEnter = bAltarEnter;

		if (EventGuildAltarGoOut != null)
		{
			EventGuildAltarGoOut(m_bAltarEnter);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드미션 길드알리기 범위 입장 여부
	public void GuildSpiritArea(bool bEnter)
	{
		if (m_csGuildMission != null && (EnMissionType)m_csGuildMission.Type == EnMissionType.GuildSpirit)
		{
			if (EventGuildSpiritArea != null)
			{
				EventGuildSpiritArea(bEnter);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool StartSupplySupportNpctDialog()
	{
		if (m_bWaitResponse) return false;
		if (m_enGuildSupplySupportState == EnGuildSupplySupportState.None && m_nDailyGuildSupplySupportQuestStartCount >= 1) return false;

		if (EventStartSupplySupportNpctDialog != null)
		{
			EventStartSupplySupportNpctDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void EndSupplySupportNpcDialog()
	{
		if (EventEndSupplySupportNpctDialog != null)
		{
			EventEndSupplySupportNpctDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool GuildHuntingNpcDialog()
	{
		if (m_bWaitResponse) return false;
		if (m_enGuildHuntingState == EnGuildHuntingState.Competed) return false;
		//if (m_nDailyGuildHuntingQuestStartCount >= GuildHuntingQuest.LimitCount) return false;

		Debug.Log("GuildHuntingNpcDialog()");
		if (EventStartGuildHuntingDialog != null)
		{
			EventStartGuildHuntingDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void EndGuildHuntingDialog()
	{
		Debug.Log("EndGuildHuntingDialog()");
		if (EventEndGuildHuntingDialog != null)
		{
			EventEndGuildHuntingDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckGuild()
	{
		if (m_csGuild != null)
		{
			if (m_csGuild.ApplicationCount > 0 && CsGameData.Instance.GetGuildMemberGrade(m_csGuildMemberGrade.MemberGrade).ApplicationAcceptanceEnabled)
			{
				return true;
			}

			// 길드 기부 1회 이상 할 수 있거나
			if (m_nDailyGuildDonationCount < CsGameData.Instance.MyHeroInfo.VipLevel.GuildDonationMaxCount)
			{
				for (int i = 0; i < CsGameData.Instance.GuildDonationEntryList.Count; i++)
				{
					CsGuildDonationEntry csGuildDonationEntry = CsGameData.Instance.GetGuildDonationEntry(i + 1);

					if (csGuildDonationEntry.MoneyType == 1)
					{
						if (CsGameData.Instance.MyHeroInfo.Gold >= csGuildDonationEntry.MoneyAmount)
							return true;
					}
					else
					{
						if (CsGameData.Instance.MyHeroInfo.Dia >= csGuildDonationEntry.MoneyAmount)
							return true;
					}
				}
			}

			// 길드 보상을 수령할 수 있거나
			if (m_dtGuildDailyRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
			{
				return true;
			}

			// 길드 제단 보상을 수령할 수 있거나
			if (m_csGuild.MoralPoint >= CsGameData.Instance.GuildAltar.DailyGuildMaxMoralPoint && m_dtGuildAltarRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
			{
				return true;
			}
			// 길드 현상금 보상을 수령할 수 있거나

			if (m_csGuild.DailyHuntingDonationCount >= CsGameConfig.Instance.GuildHuntingDonationMaxCount)
			{
				if (m_dtGuildHuntingDonationRewardReceivedDate.Date.CompareTo(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) != 0)
				{
					return true;
				}
			}

			// 길드 군량 보상을 수령할 수 있으면,
			if (m_csGuild.FoodWarehouseCollectionId != Guid.Empty && m_csGuild.FoodWarehouseCollectionId != m_guidReceivedGuildFoodWarehouseCollectionId)
			{
				return true;
			}


		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroGuildSkill GetHeroGulildSkill(int nGuildSkillId)
	{
		for (int i = 0; i < m_listCsHeroGuildSkill.Count; i++)
		{
			if (m_listCsHeroGuildSkill[i].Id == nGuildSkillId)
				return m_listCsHeroGuildSkill[i];
		}

		return null;
	}

	#region Protocol.Command

	#region public.Event

	//---------------------------------------------------------------------------------------------------
	public void GuildInteractionCancel()
	{
		dd.d("GuildInteractionCancel ", m_bInteraction, m_bSpellInjection);

		if (m_bInteraction)
		{
			SendGuildFarmQuestInteractionCancel();
		}
		else if (m_bSpellInjection)
		{
			SendGuildAltarSpellInteractionMissionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드 농장 퀘스트 상호작용 취소 (클라이언트 이벤트)
	void SendGuildFarmQuestInteractionCancel()
	{
		Debug.Log("SendGuildFarmQuestInteractionCancel()");
		m_bInteraction = false;
		CEBGuildFarmQuestInteractionCancelEventBody csEvt = new CEBGuildFarmQuestInteractionCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.GuildFarmQuestInteractionCancel, csEvt);
		if (EventMyHeroGuildFarmQuestInteractionCancel != null)
		{
			EventMyHeroGuildFarmQuestInteractionCancel();
		}
	}

    //---------------------------------------------------------------------------------------------------
    // 길드 제단 마력주입 취소
    void SendGuildAltarSpellInteractionMissionCancel()
    {
		Debug.Log("SendGuildAltarSpellInteractionMissionCancel()");
		m_bSpellInjection = false;
        CEBGuildAltarSpellInjectionMissionCancelEventBody csEvt = new CEBGuildAltarSpellInjectionMissionCancelEventBody();
        CsRplzSession.Instance.Send(ClientEventName.GuildAltarSpellInjectionMissionCancel, csEvt);
        if (EventMyHeroGuildAltarSpellInjectionMissionCancel !=null)
        {
            EventMyHeroGuildAltarSpellInjectionMissionCancel();
        }
    }

	#endregion public.Event

	//---------------------------------------------------------------------------------------------------
	public void SendGuildList() // 길드목록
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildListCommandBody cmdBody = new GuildListCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildList, cmdBody);
		}
	}

	void OnEventResGuildList(int nReturnCode, GuildListResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			List<CsSimpleGuild> list = new List<CsSimpleGuild>();

			for (int i = 0; i < resBody.guilds.Length; i++)
			{
				list.Add(new CsSimpleGuild(resBody.guilds[i]));
			}

			if (EventGuildList != null)
			{
				EventGuildList(list);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildCreate(string strGuildName) // 길드생성
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildCreateCommandBody cmdBody = new GuildCreateCommandBody();
			cmdBody.guildName = strGuildName;
			CsRplzSession.Instance.Send(ClientCommandName.GuildCreate, cmdBody);
		}
	}

	void OnEventResGuildCreate(int nReturnCode, GuildCreateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsHeroGuildApplication.Clear();
			m_listCsHeroGuildInvitation.Clear();

			m_csGuild = new CsGuild(resBody.guild);
            m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(resBody.guildMemberGrade);

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOnwDia;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;

			// 전투력 갱신
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventGuildCreate != null)
			{
				EventGuildCreate();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 길드에 가입되어 있습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// VIP레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 다이아가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00204"));
		}
		else if (nReturnCode == 105)
		{
			// 이름이 이미 존재합니다.
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00205"));
		}
		else if (nReturnCode == 106)
		{
			// 해당 이름은 금지어입니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00206"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildApply(Guid guidId) // 길드신청
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildApplyCommandBody cmdBody = new GuildApplyCommandBody();
			cmdBody.guildId = guidId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildApply, cmdBody);
		}
	}

	void OnEventResGuildApply(int nReturnCode, GuildApplyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsHeroGuildApplication.Add(new CsHeroGuildApplication(resBody.application));
			m_nDailyGuildApplicationCount = resBody.dailyApplicationCount;
			m_dtGuildApplicationCountDate = resBody.date;

			if (EventGuildApply != null)
			{
				EventGuildApply();
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상길드가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 길드의 멤버수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 길드에 가입되어 있습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 가입대기시간이 경과하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00304"));
		}
		else if (nReturnCode == 105)
		{
			// 영웅레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00305"));
		}
		else if (nReturnCode == 106)
		{
			// 길드의 신청접수건수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00306"));
		}
		else if (nReturnCode == 107)
		{
			// 이미 신청한 길드입니다.
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_00307"));
		}
		else if (nReturnCode == 108)
		{
			// 일일신청횟수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00308"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildMemberTabInfo() // 길드멤버탭정보
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildMemberTabInfoCommandBody cmdBody = new GuildMemberTabInfoCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildMemberTabInfo, cmdBody);
		}
	}

	void OnEventResGuildMemberTabInfo(int nReturnCode, GuildMemberTabInfoResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsGuildMember.Clear();

			for (int i = 0; i < resBody.members.Length; i++)
			{
				m_listCsGuildMember.Add(new CsGuildMember(resBody.members[i]));
			}

			m_nDailyBanishmentCount = resBody.dailyBanishmentCount;
			m_dtBanishmentCountDate = resBody.date;

			if (EventGuildMemberTabInfo != null)
			{
				EventGuildMemberTabInfo();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00401"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildApplicationList() // 길드신청목록
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildApplicationListCommandBody cmdBody = new GuildApplicationListCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildApplicationList, cmdBody);
		}
	}

	void OnEventResGuildApplicationList(int nReturnCode, GuildApplicationListResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsGuildApplication.Clear();

			for (int i = 0; i < resBody.applications.Length; i++)
			{
				m_listCsGuildApplication.Add(new CsGuildApplication(resBody.applications[i]));
			}

			if (EventGuildApplicationList != null)
			{
				EventGuildApplicationList();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00502"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildApplicationAccept(Guid guidAppId) // 길드신청수락
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildApplicationAcceptCommandBody cmdBody = new GuildApplicationAcceptCommandBody();
			cmdBody.applicationId = m_guidAppId = guidAppId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildApplicationAccept, cmdBody);
		}
	}

	void OnEventResGuildApplicationAccept(int nReturnCode, GuildApplicationAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			// 길드가입요청 수락(권한있는 당사자)
			m_listCsGuildApplication.RemoveAll(a => a.Id == m_guidAppId);

			if (EventGuildApplicationAccept != null)
			{
				EventGuildApplicationAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00602"));
		}
		else if (nReturnCode == 103)
		{
			// 신청이 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00603"));
		}
		else if (nReturnCode == 104)
		{
			// 길드의 멤버수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00604"));
		}
		else if (nReturnCode == 105)
		{
			// 신청자가 이미 길드에 가입되어 있습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00605"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildApplicationRefuse(Guid guidAppId) // 길드신청거절
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildApplicationRefuseCommandBody cmdBody = new GuildApplicationRefuseCommandBody();
			cmdBody.applicationId = m_guidAppId = guidAppId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildApplicationRefuse, cmdBody);
		}
	}

	void OnEventResGuildApplicationRefuse(int nReturnCode, GuildApplicationRefuseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsGuildApplication.RemoveAll(a => a.Id == m_guidAppId);

			if (EventGuildApplicationRefuse != null)
			{
				EventGuildApplicationRefuse();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00702"));
		}
		else if (nReturnCode == 103)
		{
			// 신청이 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00703"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGuildExit() // 길드탈퇴
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildExitCommandBody cmdBody = new GuildExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildExit, cmdBody);
		}
	}

	void OnEventResGuildExit(int nReturnCode, GuildExitResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_flGuildRejoinRemainingTime = CsGameConfig.Instance.GuildRejoinIntervalTime + Time.realtimeSinceStartup;

			Reset();

			if (CsGameData.Instance.MyHeroInfo.LocationId == GuildTerritory.LocationId)
			{
				CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			}

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			// 전투력 업데이트.
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = resBody.previousNationId;

			if (EventGuildExit != null)
			{
				EventGuildExit(resBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 길드장은 탈퇴할 수 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00802"));
		}
		else if (nReturnCode == 103)
		{
			// 길드물자지원퀘스트를 수행중입니다.
			// CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드멤버추방
	public void SendGuildMemberBanish(Guid guidMemberId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildMemberBanishCommandBody cmdBody = new GuildMemberBanishCommandBody();
			cmdBody.targetMemberId = m_guidBanishMemberId = guidMemberId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildMemberBanish, cmdBody);
		}
	}

	void OnEventResGuildMemberBanish(int nReturnCode, GuildMemberBanishResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nDailyGuildApplicationCount++;

			m_listCsGuildMember.RemoveAll(a => a.Id == m_guidBanishMemberId);

			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			if (EventGuildMemberBanish != null)
			{
				EventGuildMemberBanish();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00901"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00902"));
		}
		else if (nReturnCode == 103)
		{
			// 대상멤버가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00903"));
		}
		else if (nReturnCode == 104)
		{
			// 일일추방횟수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_00904"));
		}
		else if (nReturnCode == 105)
		{
			// 길드물자지원퀘스트를 수행중입니다.
			// CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드초대
	public void SendGuildInvite(Guid guidHeroId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildInviteCommandBody cmdBody = new GuildInviteCommandBody();
			cmdBody.heroId = guidHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildInvite, cmdBody);
		}
	}

	void OnEventResGuildInvite(int nReturnCode, GuildInviteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventGuildInvite != null)
			{
				EventGuildInvite();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01001"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01002"));
		}
		else if (nReturnCode == 103)
		{
			// 길드의 멤버수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01003"));
		}
		else if (nReturnCode == 104)
		{
            // 대상영웅이 존재하지 않거나 다른 국가영웅입니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_01004"));
		}
		else if (nReturnCode == 105)
		{
			// 대상영웅의 레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01005"));
		}
		else if (nReturnCode == 106)
		{
            // 대상영웅의 길드가입대기시간이 경과하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_01006"));
		}
		else if (nReturnCode == 107)
		{
            // 대상영웅이 이미 길드에 가입되어 있습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_01007"));
		}
		else if (nReturnCode == 108)
		{
            // 대상영웅을 이미 초대했습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_01008"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드초대수락
	public void SendGuildInvitationAccept(Guid guidInvitationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildInvitationAcceptCommandBody cmdBody = new GuildInvitationAcceptCommandBody();
			cmdBody.invitationId = m_guidInvitationId = guidInvitationId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildInvitationAccept, cmdBody);
		}
	}

	void OnEventResGuildInvitationAccept(int nReturnCode, GuildInvitationAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			//CsHeroGuildInvitation csHeroGuildInvitation = m_listCsHeroGuildInvitation.Find(a => a.Id == m_guidInvitationId);

			m_csGuild = new CsGuild(resBody.guild);
            m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(resBody.memberGrade);

			m_listCsHeroGuildInvitation.Clear();
			m_listCsHeroGuildApplication.Clear();

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventGuildInvitationAccept != null)
			{
				EventGuildInvitationAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 초대가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01101"));
		}
		else if (nReturnCode == 102)
		{
			// 길드의 멤버수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 길드에 가입되어 있습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드초대거절
	public void SendGuildInvitationRefuse(Guid guidInvitationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildInvitationRefuseCommandBody cmdBody = new GuildInvitationRefuseCommandBody();
			cmdBody.invitationId = m_guidInvitationId = guidInvitationId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildInvitationRefuse, cmdBody);
		}
	}

	void OnEventResGuildInvitationRefuse(int nReturnCode, GuildInvitationRefuseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsHeroGuildInvitation.RemoveAll(a => a.Id == m_guidInvitationId);

			if (EventGuildInvitationRefuse != null)
			{
				EventGuildInvitationRefuse();
			}
		}
		else if (nReturnCode == 101)
		{
			// 초대가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_01201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	string m_strNoticeTemp;

	//---------------------------------------------------------------------------------------------------
	// 길드공지설정
	public void SendGuildNoticeSet(string strNotice)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildNoticeSetCommandBody cmdBody = new GuildNoticeSetCommandBody();
			cmdBody.notice = m_strNoticeTemp = strNotice;
			CsRplzSession.Instance.Send(ClientCommandName.GuildNoticeSet, cmdBody);
		}
	}

	void OnEventResGuildNoticeSet(int nReturnCode, GuildNoticeSetResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csGuild.Notice = m_strNoticeTemp;

			if (EventGuildNoticeSet != null)
			{
				EventGuildNoticeSet();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001301"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001302"));
		}
		else if (nReturnCode == 103)
		{
			// 공지 길이가 유효하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001303"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


	//---------------------------------------------------------------------------------------------------
	// 길드임명
	public void SendGuildAppoint(Guid guidTargetMemberId, int nTargetMemberGrade)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildAppointCommandBody cmdBody = new GuildAppointCommandBody();
			//cmdBody.targetMemberId = m_guidTargetMemberId = guidTargetMemberId;
			//cmdBody.targetMemberGrade = m_nTargetMemberGrade = nTargetMemberGrade;
			cmdBody.targetMemberId = guidTargetMemberId;
			cmdBody.targetMemberGrade = nTargetMemberGrade;
			CsRplzSession.Instance.Send(ClientCommandName.GuildAppoint, cmdBody);
		}
	}

	void OnEventResGuildAppoint(int nReturnCode, GuildAppointResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventGuildAppoint != null)
			{
				EventGuildAppoint();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001401"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001402"));
		}
		else if (nReturnCode == 103)
		{
			// 부길드장수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001403"));
		}
		else if (nReturnCode == 104)
		{
			// 로드수가 초대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001404"));
		}
		else if (nReturnCode == 105)
		{
			// 대상멤버가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001405"));
		}
		else if (nReturnCode == 106)
		{
			// 대상멤버는 이미 대상등급입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001406"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드장위임
	public void SendGuildMasterTransfer(Guid guidTargetMemberId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildMasterTransferCommandBody cmdBody = new GuildMasterTransferCommandBody();
			//cmdBody.targetMemberId = m_guidTargetMemberId = guidTargetMemberId;
			cmdBody.targetMemberId = guidTargetMemberId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildMasterTransfer, cmdBody);
		}
	}

	void OnEventResGuildMasterTransfer(int nReturnCode, GuildMasterTransferResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(resBody.memberGrade);

			if (EventGuildMasterTransfer != null)
			{
				EventGuildMasterTransfer();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001501"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001502"));
		}
		else if (nReturnCode == 103)
		{
			// 대상멤버가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001503"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드기부
	public void SendGuildDonate(int nEntryId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildDonateCommandBody cmdBody = new GuildDonateCommandBody();
			cmdBody.entryId = nEntryId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildDonate, cmdBody);
		}
	}

	void OnEventResGuildDonate(int nReturnCode, GuildDonateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nDailyGuildDonationCount = resBody.dailyDonationCount;
			m_dtGuildDonationCountDate = resBody.date;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			m_nTotalGuildContributionPoint = resBody.totalContributionPoint;

            int nOldGuildContributionPoint = m_nGuildContributionPoint;
			m_nGuildContributionPoint = resBody.contributionPoint;

            if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
            }

            long lOldGuildFund = m_csGuild.Fund;
            m_csGuild.Fund = resBody.fund;

            if (0 < m_csGuild.Fund - lOldGuildFund)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDM"), m_csGuild.Fund - lOldGuildFund));
            }
            
			if (EventGuildDonate != null)
			{
				EventGuildDonate();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001601"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001602"));
		}
		else if (nReturnCode == 103)
		{
			// 대상멤버가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001603"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드건물레벨업
	public void SendGuildBuildingLevelUp(int nBuildingId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildBuildingLevelUpCommandBody cmdBody = new GuildBuildingLevelUpCommandBody();
			cmdBody.buildingId = m_nBuildingId = nBuildingId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildBuildingLevelUp, cmdBody);
		}
	}

	void OnEventResGuildBuildingLevelUp(int nReturnCode, GuildBuildingLevelUpResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGuildBuildingInstance csGuildBuildingInstance = m_csGuild.GetGuildBuildingInstance(m_nBuildingId);
			csGuildBuildingInstance.Level = resBody.level;

            long lOldGuildFund = m_csGuild.Fund;
            m_csGuild.Fund = resBody.fund;

            if (0 < m_csGuild.Fund - lOldGuildFund)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDM"), m_csGuild.Fund - lOldGuildFund));
            }

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventGuildBuildingLevelUp != null)
			{
				EventGuildBuildingLevelUp();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001901"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001902"));
		}
		else if (nReturnCode == 103)
		{
			// 최대레벨입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001903"));
		}
		else if (nReturnCode == 104)
		{
			// 건물레벨은 길드레벨(로비레벨)을 초과할 수 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001904"));
		}
		else if (nReturnCode == 105)
		{
			// 길드건설도가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001905"));
		}
		else if (nReturnCode == 106)
		{
			// 길드자금이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_001906"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드스킬레벨업
	public void SendGuildSkillLevelUp(int nSkillId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildSkillLevelUpCommandBody cmdBody = new GuildSkillLevelUpCommandBody();
			cmdBody.skillId = m_nSkillId = nSkillId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildSkillLevelUp, cmdBody);
		}
	}

	void OnEventResGuildSkillLevelUp(int nReturnCode, GuildSkillLevelUpResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroGuildSkill csHeroGuildSkill = GetHeroGulildSkill(m_nSkillId);

			if (csHeroGuildSkill == null)
			{
				csHeroGuildSkill = new CsHeroGuildSkill(m_nSkillId, resBody.level);
				m_listCsHeroGuildSkill.Add(csHeroGuildSkill);
			}
			else
			{
				csHeroGuildSkill.Level = resBody.level;
			}

            int nOldGuildContributionPoint = m_nGuildContributionPoint;
			m_nGuildContributionPoint = resBody.contributionPoint;

            if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
            }

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventGuildSkillLevelUp != null)
			{
				EventGuildSkillLevelUp();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002001"));
		}
		else if (nReturnCode == 102)
		{
			// 개방된 스킬레벨이 아닙니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002002"));
		}
		else if (nReturnCode == 103)
		{
			// 공헌도가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002003"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드영지입장을위한대륙퇴장
	public void SendContinentExitForGuildTerritoryEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentExitForGuildTerritoryEnterCommandBody cmdBody = new ContinentExitForGuildTerritoryEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForGuildTerritoryEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForGuildTerritoryEnter(int nReturnCode, ContinentExitForGuildTerritoryEnterResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventContinentExitForGuildTerritoryEnter != null)
			{
				EventContinentExitForGuildTerritoryEnter(CsGameData.Instance.GuildTerritory.SceneName);
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은 상태입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 전투상태입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	//길드 영지 입장
	public void SendGuildTerritoryEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildTerritoryEnterCommandBody cmdBody = new GuildTerritoryEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildTerritoryEnter, cmdBody);
		}
	}

	void OnEventResGuildTerritoryEnter(int nReturnCode, GuildTerritoryEnterResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.LocationId = GuildTerritory.LocationId;
			if (EventGuildTerritoryEnter != null)
			{
				EventGuildTerritoryEnter(resBody.placeInstanceId, resBody.heroes, resBody.monsters, resBody.position, resBody.rotationY);
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

	}

	//---------------------------------------------------------------------------------------------------
	// 길드 영지 퇴장
	public void SendGuildTerritoryExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildTerritoryExitCommandBody cmdBody = new GuildTerritoryExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildTerritoryExit, cmdBody);
		}
	}

	void OnEventResGuildTerritoryExit(int nReturnCode, GuildTerritoryExitResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = resBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			if (EventGuildTerritoryExit != null)
			{
				EventGuildTerritoryExit(resBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			//영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드농장퀘스트 수락
	public void SendGuildFarmQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendGuildFarmQuestAccept");
			m_bWaitResponse = true;
			GuildFarmQuestAcceptCommandBody cmdBody = new GuildFarmQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFarmQuestAccept, cmdBody);
		}
	}

	void OnEventResGuildFarmQuestAccept(int nReturnCode, GuildFarmQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildFarmQuestStartCountDate = resBody.date;
			m_nDailyGuildFarmQuestStartCount = resBody.dailyStartCount;
			m_bFarmQuestMissionExcuted = resBody.quest.isObjectiveCompleted;

			UpdateGuildFarmQuestState();

			if (EventGuildFarmQuestAccept != null)
			{
				EventGuildFarmQuestAccept(resBody.quest);
			}
		}
		else if (nReturnCode == 101)
		{
            //일일시작횟가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002401"));
		}
		else if (nReturnCode == 102)
		{
			//퀘스트시간이 아닙니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(string.Format());
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드농장퀘스트 상호 작용 시작
	void SendGuildFarmQuestInteractionStart()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendGuildFarmQuestInteractionStart");
			m_bWaitResponse = true;
			GuildFarmQuestInteractionStartCommandBody cmdBody = new GuildFarmQuestInteractionStartCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFarmQuestInteractionStart, cmdBody);
		}
	}

	void OnEventResGuildFarmQuestInteractionStart(int nReturnCode, GuildFarmQuestInteractionStartResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bInteraction = true;
			if (EventGuildFarmQuestInteractionStart != null)
			{
				EventGuildFarmQuestInteractionStart();
			}
		}
		else if (nReturnCode == 101)
		{
            //영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002501"));
		}
		else if (nReturnCode == 102)
		{
            //영웅이 탈 것을 타고 있는 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002502"));
		}
		else if (nReturnCode == 103)
		{
			//영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002503"));
		}
		else if (nReturnCode == 104)
		{
            //영웅이 다른 행동중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002504"));
		}
		else if (nReturnCode == 105)
		{
            //수행중인 퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002505"));
		}
		else if (nReturnCode == 106)
		{
            //이미 목표가 완료되었습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002506"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	//길드농장퀘스트 완료
	public void SendGuildFarmQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendGuildFarmQuestComplete");
			m_bWaitResponse = true;
			GuildFarmQuestCompleteCommandBody cmdBody = new GuildFarmQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFarmQuestComplete, cmdBody);
		}
	}

	void OnEventResGuildFarmQuestComplete(int nReturnCode, GuildFarmQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			m_nTotalGuildContributionPoint = resBody.totalGuildContributionPoint;

            int nOldGuildContributionPoint = m_nGuildContributionPoint;
			m_nGuildContributionPoint = resBody.guildContributionPoint;

            if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
            }

            int nOldBuildingPoint = m_csGuild.BuildingPoint;
			m_csGuild.BuildingPoint = resBody.giBuildingPoint;

            if (0 < m_csGuild.BuildingPoint - nOldBuildingPoint)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDB"), m_csGuild.BuildingPoint - nOldBuildingPoint));
            }

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			UpdateGuildFarmQuestState(true);

			if (EventGuildFarmQuestComplete != null)
			{
				EventGuildFarmQuestComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
            //수행중인 퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A58_ERROR_002601"));
		}
		else if (nReturnCode == 102)
		{
            //목표가 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A58_ERROR_002602"));
		}
		else
		{
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드농장퀘스트 포기
	public void SendGuildFarmQuestAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildFarmQuestAbandonCommandBody cmdBody = new GuildFarmQuestAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFarmQuestAbandon, cmdBody);
		}
	}

	void OnEventResGuildFarmQuestAbandon(int nReturnCode, GuildFarmQuestAbandonResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			UpdateGuildFarmQuestState(true);
			if (EventGuildFarmQuestAbandon != null)
			{
				EventGuildFarmQuestAbandon();
			}
		}
		else if (nReturnCode == 101)
		{
            //수행중인 퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_003001"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드군량창고정보
	public void SendGuildFoodWarehouseInfo()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildFoodWarehouseInfoCommandBody cmdBody = new GuildFoodWarehouseInfoCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFoodWarehouseInfo, cmdBody);
		}
	}

	void OnEventResGuildFoodWarehouseInfo(int nReturnCode, GuildFoodWarehouseInfoResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventGuildFoodWarehouseInfo != null)
			{
				EventGuildFoodWarehouseInfo(resBody.level, resBody.exp);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드군량창고납부
	public void SendGuildFoodWarehouseStock(int nItemId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildFoodWarehouseStockCommandBody cmdBody = new GuildFoodWarehouseStockCommandBody();
			cmdBody.itemId = nItemId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildFoodWarehouseStock, cmdBody);
		}
	}

	void OnEventResGuildFoodWarehouseStock(int nReturnCode, GuildFoodWarehouseStockResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			PDInventorySlot[] inventorySlots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(inventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			m_nDailyGuildFoodWarehouseStockCount = resBody.dailyStockCount;
			m_dtGuildFoodWarehouseStockCountDate = resBody.date;

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventGuildFoodWarehouseStock != null)
			{
				EventGuildFoodWarehouseStock(bLevelUp, resBody.acquiredExp, resBody.addedFoodWarehouseExp, resBody.foodWarehouseLevel, resBody.foodWarehouseExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 금일 채우기횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드군량창고징수
	public void SendGuildFoodWarehouseCollect()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildFoodWarehouseCollectCommandBody cmdBody = new GuildFoodWarehouseCollectCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFoodWarehouseCollect, cmdBody);
		}
	}

	void OnEventResGuildFoodWarehouseCollect(int nReturnCode, GuildFoodWarehouseCollectResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csGuild.FoodWarehouseCollectionId = resBody.collectionId;

			if (EventGuildFoodWarehouseCollect != null)
			{
				EventGuildFoodWarehouseCollect();
			}
		}
		else if (nReturnCode == 101)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 군량창고가 최대레벨이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드군량창고보상받기
	public void SendGuildFoodWarehouseRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildFoodWarehouseRewardReceiveCommandBody cmdBody = new GuildFoodWarehouseRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildFoodWarehouseRewardReceive, cmdBody);
		}
	}

	void OnEventResGuildFoodWarehouseRewardReceive(int nReturnCode, GuildFoodWarehouseRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_guidReceivedGuildFoodWarehouseCollectionId = resBody.receivedCollectionId;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildFoodWarehouseRewardReceive != null)
			{
				EventGuildFoodWarehouseRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 군량창고를 징수하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A65_ERROR_00403"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드제단기부
	public void SendGuildAltarDonate()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildAltarDonateCommandBody cmdBody = new GuildAltarDonateCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildAltarDonate, cmdBody);
		}
	}

	void OnEventResGuildAltarDonate(int nReturnCode, GuildAltarDonateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildMoralPointDate = resBody.date;
			m_nGuildMoralPoint = resBody.guildMoralPoint;
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;

			m_csGuild.MoralPoint = resBody.giMoralPoint;
			m_csGuild.MoralPointDate = resBody.date;

			if (EventGuildAltarDonate != null)
			{
				EventGuildAltarDonate();
			}
		}
		else if (nReturnCode == 101)
		{
			// 금일 채울수 있는 모럴포인트가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 마력주임미션을 수행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 수비미션을 수행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드제단보상받기
	public void SendGuildAltarRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildAltarRewardReceiveCommandBody cmdBody = new GuildAltarRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildAltarRewardReceive, cmdBody);
		}
	}

	void OnEventResGuildAltarRewardReceive(int nReturnCode, GuildAltarRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildAltarRewardReceivedDate = resBody.date;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildAltarRewardReceive != null)
			{
				EventGuildAltarRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 길드의 모럴포인트가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00403"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

    //---------------------------------------------------------------------------------------------------
    //길드제단 마력주입 미션시작
    public void SendGuildAltarSpellInjectionMissionStart()
    {
        if (!m_bWaitResponse)
        {
			Debug.Log("SendGuildAltarSpellInjectionMissionStart");
            m_bWaitResponse = true;
            GuildAltarSpellInjectionMissionStartCommandBody cmdBody = new GuildAltarSpellInjectionMissionStartCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.GuildAltarSpellInjectionMissionStart, cmdBody);
        }
    }

    void OnEventResGuildAltarSpellInjectionMissionStart(int nReturnCode, GuildAltarSpellInjectionMissionStartResponseBody resBody)
    {
		Debug.Log("OnEventResGuildAltarSpellInjectionMissionStart  nReturnCode = " + nReturnCode);
        m_bWaitResponse = false;

        if (nReturnCode == 0)
        {
			m_bSpellInjection = true;
			
            if (EventGuildAltarSpellInjectionMissionStart != null)
            {
                EventGuildAltarSpellInjectionMissionStart();
            }
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00201"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅이 탈 것을 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00202"));
        }
        else if (nReturnCode == 103)
        {
            // 영웅이 다른 행동 중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00203"));
        }
        else if (nReturnCode == 104)
        {
            // 금일 채울 수 있는 모럴포인트가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00204"));
        }
        else if (nReturnCode == 105)
        {
            // 마력주입미션을 수행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00205"));
        }
        else if (nReturnCode == 106)
        {
            // 수비미션을 수행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A68_ERROR_00206"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 길드제단 수비 미션시작
    public void SendGuildAltarDefenseMissionStart()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            GuildAltarDefenseMissionStartCommandBody cmdBody = new GuildAltarDefenseMissionStartCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.GuildAltarDefenseMissionStart, cmdBody);
        }
    }

    void OnEventResGuildAltarDefenseMissionStart(int nReturnCode, GuildAltarDefenseMissionStartResponseBody resBody)
    {
        m_bWaitResponse = false;

        if (nReturnCode == 0)
        {
			m_bGuildDefense = true;
            m_flGuildAltarDefenseMissionRemainingCoolTime = resBody.remainingCoolTime + Time.realtimeSinceStartup;
			m_lDefenseMonsterInstanceId = resBody.monsterInstanceId;
			m_vtDefenseMonsterPos = CsRplzSession.Translate(resBody.monsterPosition);

            if (EventGuildAltarDefenseMissionStart != null)
            {
                EventGuildAltarDefenseMissionStart();
            }
        }
        else if (nReturnCode == 101)
        {
			// 금일 채울수 있는 모럴포인트가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A69_ERROR_00301"));
		}
		else if (nReturnCode == 102)
        {
			// 마력주입미션을 수행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A69_ERROR_00302"));
		}
		else if (nReturnCode == 103)
        {
			// 수비미션을 수행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A69_ERROR_00303"));
		}
		else if (nReturnCode == 104)
        {
			// 쿨타임이 경과되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A69_ERROR_00304"));
		}
		else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	#region GuildMissionQuest
	//---------------------------------------------------------------------------------------------------
	// 길드미션퀘스트수락
	public void SendGuildMissionQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendGuildMissionQuestAccept()");
			m_bWaitResponse = true;
			GuildMissionQuestAcceptCommandBody cmdBody = new GuildMissionQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildMissionQuestAccept, cmdBody);
		}
	}
	 
	void OnEventResGuildMissionQuestAccept(int nReturnCode, GuildMissionQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			SetGuildMission(resBody.quest);
			//m_dtGuildMissionQuestStartDate = resBody.date;
			UpdateGuildMission();

			if (EventGuildMissionQuestAccept != null)
			{
				EventGuildMissionQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 시작 NPC와 상호작용할 수 있는 위치가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00102"));
		}
        else if (nReturnCode == 103)
        {
            // 이미 위탁을 한 할일입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00103"));
        }
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드미션수락
	public void SendGuildMissionAccept()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendGuildMissionAccept()");
			m_bWaitResponse = true;
			GuildMissionAcceptCommandBody cmdBody = new GuildMissionAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildMissionAccept, cmdBody);
		}
	}

	void OnEventResGuildMissionAccept(int nReturnCode, GuildMissionAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nMissionProgressCount = 0;
			m_csGuildMission = GuildMissionQuest.GetGuildMission(resBody.mission.missionId);
			m_csGuildMissionMonster = new CsGuildMissionMonster(resBody.mission.monsterInstanceId,
																resBody.mission.monsterSpawnedContinentId,
																CsRplzSession.Translate(resBody.mission.monsterPosition),
																resBody.mission.remainingMonsterLifetime);
			UpdateGuildMission();

			if (EventGuildMissionAccept != null)
			{
				EventGuildMissionAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 시작 NPC와 상호작용할 수 있는 위치가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			//  현재 날짜가 퀘스트시작날짜와 다릅니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드미션포기
	public void SendGuildMissionAbandon()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendGuildMissionAbandon()");
			m_bWaitResponse = true;
			GuildMissionAbandonCommandBody cmdBody = new GuildMissionAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildMissionAbandon, cmdBody);
		}
	}

	void OnEventResGuildMissionAbandon(int nReturnCode, GuildMissionAbandonResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csGuildMission = null;
			m_csGuildMissionMonster = null;
			m_nMissionProgressCount = 0;
			UpdateGuildMission();

			if (EventGuildMissionAbandon != null)
			{
				EventGuildMissionAbandon();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드미션대상 NPC상호작용
	public void SendGuildMissionTargetNpcInteract()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildMissionTargetNpcInteractCommandBody cmdBody = new GuildMissionTargetNpcInteractCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildMissionTargetNpcInteract, cmdBody);
		}
	}

	void OnEventResGuildMissionTargetNpcInteract(int nReturnCode, GuildMissionTargetNpcInteractResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			//m_nMissionProgressCount = resBody.progressCount;
			UpdateGuildMission();
			if (EventGuildMissionTargetNpcInteract != null)
			{
				EventGuildMissionTargetNpcInteract();
			}
		}
		else if (nReturnCode == 101)
		{
			//  NPC와 상호작용할 수 있는 위치가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A71_ERROR_00501"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion GuildMissionQuest

	//---------------------------------------------------------------------------------------------------
	// 서버길드랭킹
	public void SendServerGuildRanking()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ServerGuildRankingCommandBody cmdBody = new ServerGuildRankingCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ServerGuildRanking, cmdBody);
		}
	}

	void OnEventResServerGuildRanking(int nReturnCode, ServerGuildRankingResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGuildRanking csGuildRanking = null;

			if (resBody.myGuildRanking != null)
				csGuildRanking = new CsGuildRanking(resBody.myGuildRanking);

			List<CsGuildRanking> listCsGuildRanking = new List<CsGuildRanking>();

			for (int i = 0; i < resBody.guildRankings.Length; i++)
			{
				listCsGuildRanking.Add(new CsGuildRanking(resBody.guildRankings[i]));
			}

			if (EventServerGuildRanking != null)
			{
				EventServerGuildRanking(csGuildRanking, listCsGuildRanking);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가길드랭킹
	public void SendNationGuildRanking()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationGuildRankingCommandBody cmdBody = new NationGuildRankingCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.NationGuildRanking, cmdBody);
		}
	}

	void OnEventResNationGuildRanking(int nReturnCode, NationGuildRankingResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGuildRanking csGuildRanking = null;

			if (resBody.myGuildRanking != null)
				csGuildRanking = new CsGuildRanking(resBody.myGuildRanking);

			List<CsGuildRanking> listCsGuildRanking = new List<CsGuildRanking>();

			for (int i = 0; i < resBody.guildRankings.Length; i++)
			{
				listCsGuildRanking.Add(new CsGuildRanking(resBody.guildRankings[i]));
			}

			if (EventNationGuildRanking != null)
			{
				EventNationGuildRanking(csGuildRanking, listCsGuildRanking);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일보상받기
	public void SendGuildDailyRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildDailyRewardReceiveCommandBody cmdBody = new GuildDailyRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildDailyRewardReceive, cmdBody);
		}
	}

	void OnEventResGuildDailyRewardReceive(int nReturnCode, GuildDailyRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildDailyRewardReceivedDate = resBody.date;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildDailyRewardReceive != null)
			{
				EventGuildDailyRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002701"));
		}
		else if (nReturnCode == 102)
		{
			// 금일 이미 보상으로 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002702"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드소집
	public void SendGuildCall(int nSlotIndex)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildCallCommandBody cmdBody = new GuildCallCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.GuildCall, cmdBody);
		}
	}

	void OnEventResGuildCall(int nReturnCode, GuildCallResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			PDInventorySlot[] inventorySlots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(inventorySlots);

			if (EventGuildCall != null)
			{
				EventGuildCall();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002801"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002802"));
		}
		else if (nReturnCode == 103)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002803"));
		}
		else if (nReturnCode == 104)
		{
			// 국가전이 진행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002804"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드소집전송
	public void SendGuildCallTransmission(long lCallId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildCallTransmissionCommandBody cmdBody = new GuildCallTransmissionCommandBody();
			cmdBody.callId = lCallId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildCallTransmission, cmdBody);
		}
	}

	void OnEventResGuildCallTransmission(int nReturnCode, GuildCallTransmissionResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.GuildCallTransmission;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = resBody.targetNationId;
			
			if (EventGuildCallTransmission != null)
			{
				EventGuildCallTransmission(resBody.targetContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002901"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002902"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002903"));
		}
		else if (nReturnCode == 104)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002904"));
		}
		else if (nReturnCode == 105)
		{
            // 길드소집이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_002905"));
		}
		else if (nReturnCode == 106)
		{
			// 국가전이 진행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002906"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

    //---------------------------------------------------------------------------------------------------
    // 길드소집 전송에 대한 대륙입장
    public void SendContinentEnterForGuildCallTransmission()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            ContinentEnterForGuildCallTransmissionCommandBody cmdBody = new ContinentEnterForGuildCallTransmissionCommandBody();

            CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForGuildCallTransmission, cmdBody);
        }
    }

    void OnEventResContinentEnterForGuildCallTransmission(int nReturnCode, ContinentEnterForGuildCallTransmissionResponseBody resBody)
    {
        m_bWaitResponse = false;

        if (nReturnCode == 0)
        {
            if (EventContinentEnterForGuildCallTransmission != null)
            {
                EventContinentEnterForGuildCallTransmission(resBody.entranceInfo);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 길드 물자지원 퀘스트 수락
    public void SendGuildSupplySupportQuestAccept()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            GuildSupplySupportQuestAcceptCommandBody cmdBody = new GuildSupplySupportQuestAcceptCommandBody();

            CsRplzSession.Instance.Send(ClientCommandName.GuildSupplySupportQuestAccept, cmdBody);
        }
    }

    void OnEventResGuildSupplySupportQuestAccept(int nReturnCode, GuildSupplySupportQuestAcceptResponseBody resBody)
    {
        m_bWaitResponse = false;

        if (nReturnCode == 0)
        {
			m_flGuildSupplySupportQuestRemainingTime = resBody.remainingTime  + Time.realtimeSinceStartup;
			m_dtDailyGuildSupplySupportQuestStartCountDate = resBody.date;
			m_nDailyGuildSupplySupportQuestStartCount = resBody.dailyGuildSupplySupportQuestStartCount;

			UpdateGuildSupplySupportState();
            if (EventGuildSupplySupportQuestAccept != null)
            {
                EventGuildSupplySupportQuestAccept(resBody.cartInst);
            }
        }
        else if (nReturnCode == 101)
        {
            // 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A72_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다..
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A72_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 퀘스트를 진행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A72_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 금일 이미 퀘스트를 수락했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A72_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 시작 NPC와 상호작용할 수 있는 위치가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A72_ERROR_00105"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 길드 물자지원 퀘스트 완료
    public void SendGuildSupplySupportQuestComplete()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            GuildSupplySupportQuestCompleteCommandBody cmdBody = new GuildSupplySupportQuestCompleteCommandBody();

            CsRplzSession.Instance.Send(ClientCommandName.GuildSupplySupportQuestComplete, cmdBody);
        }
    }

    void OnEventResGuildSupplySupportQuestComplete(int nReturnCode, GuildSupplySupportQuestCompleteResponseBody resBody)
    {
        m_bWaitResponse = false;

        if (nReturnCode == 0)
        {
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;

            int nOldGuildContributionPoint = m_nGuildContributionPoint;
			m_nGuildContributionPoint = resBody.guildContributionPoint;
			m_nTotalGuildContributionPoint = resBody.totalGuildContributionPoint;

			if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
            }

            long lOldGuildFund = m_csGuild.Fund;
			m_csGuild.Fund = resBody.giFund;

            if (0 < m_csGuild.Fund - lOldGuildFund)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDM"), m_csGuild.Fund - lOldGuildFund));
            }

            int nOldBuildingPoint = m_csGuild.BuildingPoint;
			m_csGuild.BuildingPoint = resBody.giBuildingPoint;

            if (0 < m_csGuild.BuildingPoint - nOldBuildingPoint)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDB"), m_csGuild.BuildingPoint - nOldBuildingPoint));
            }

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			UpdateGuildSupplySupportState(true);
            if (EventGuildSupplySupportQuestComplete != null)
            {
                EventGuildSupplySupportQuestComplete(bLevelUp, resBody.acquiredExp);
            }
        }
        else if (nReturnCode == 101)
        {
            // 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A72_ERROR_00201"));
        }
        else if (nReturnCode == 102)
        {
            // 퀘스트를 진행중이지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A72_ERROR_00202"));
        }
        else if (nReturnCode == 103)
        {
            // 영웅이 퀘스트완료NPC 상호작용 범위에 있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A72_ERROR_00203"));
        }
        else if (nReturnCode == 104)
        {
            // 퀘스트의 목표가 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A72_ERROR_00204"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅퀘스트수락
	public void SendGuildHuntingQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildHuntingQuestAcceptCommandBody cmdBody = new GuildHuntingQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildHuntingQuestAccept, cmdBody);
		}
	}

	void OnEventResGuildHuntingQuestAccept(int nReturnCode, GuildHuntingQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildHuntingQuestStartCountDate = resBody.date;
			m_nDailyGuildHuntingQuestStartCount = resBody.dailyGuildHuntingQuestStartCount;
			
			m_csHeroGuildHuntingQuest = new CsHeroGuildHuntingQuest(resBody.guildHuntingQuest);
			m_csGuildHuntingQuestObjective = CsGameData.Instance.GuildHuntingQuest.GuildHuntingQuestObjectiveList.Find(a => a.ObjectiveId == m_csHeroGuildHuntingQuest.ObjectiveId);
			m_csHeroGuildHuntingQuest.ProgressCount = 0;
			
			UpdateGuildHuntingQuestState();
			
			if (EventGuildHuntingQuestAccept != null)
			{
				EventGuildHuntingQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A75_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 시작 NPC와 상호작용할 수 있는 위치가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A75_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 금일퀘스트시작횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A75_ERROR_00103"));
		}
        else if (nReturnCode == 104)
        {
            // 이미 위탁을 한 할일입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A75_ERROR_00104"));
        }
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅퀘스트포기
	public void SendGuildHuntingQuestAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildHuntingQuestAbandonCommandBody cmdBody = new GuildHuntingQuestAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildHuntingQuestAbandon, cmdBody);
		}
	}

	void OnEventResGuildHuntingQuestAbandon(int nReturnCode, GuildHuntingQuestAbandonResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            UpdateGuildHuntingQuestState(true);

			if (EventGuildHuntingQuestAbandon != null)
			{
				EventGuildHuntingQuestAbandon();
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A75_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅퀘스트완료
	public void SendGuildHuntingQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildHuntingQuestCompleteCommandBody cmdBody = new GuildHuntingQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildHuntingQuestComplete, cmdBody);
		}
	}

	void OnEventResGuildHuntingQuestComplete(int nReturnCode, GuildHuntingQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);
			UpdateGuildHuntingQuestState(true);

			if (EventGuildHuntingQuestComplete != null)
			{
				EventGuildHuntingQuestComplete();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A75_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 퀘스트완료NPC 상호작용 범위에 있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A75_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅기부
	public void SendGuildHuntingDonate()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildHuntingDonateCommandBody cmdBody = new GuildHuntingDonateCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildHuntingDonate, cmdBody);
		}
	}

	void OnEventResGuildHuntingDonate(int nReturnCode, GuildHuntingDonateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csGuild.DailyHuntingDonationDate = m_dtGuildHuntingDonationDate = resBody.date;
			m_csGuild.DailyHuntingDonationCount = resBody.guildHuntingDonationCount;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildHuntingDonate != null)
			{
				EventGuildHuntingDonate();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 길드의 기부횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			//  이미 기부했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅기부보상받기
	public void SendGuildHuntingDonationRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildHuntingDonationRewardReceiveCommandBody cmdBody = new GuildHuntingDonationRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildHuntingDonationRewardReceive, cmdBody);
		}
	}

	void OnEventResGuildHuntingDonationRewardReceive(int nReturnCode, GuildHuntingDonationRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildHuntingDonationRewardReceivedDate = resBody.date;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildHuntingDonationRewardReceive != null)
			{
				EventGuildHuntingDonationRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 길드의 기부횟수가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A76_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일목표알림
	public void SendGuildDailyObjectiveNotice()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildDailyObjectiveNoticeCommandBody cmdBody = new GuildDailyObjectiveNoticeCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildDailyObjectiveNotice, cmdBody);
		}
	}

	void OnEventResGuildDailyObjectiveNotice(int nReturnCode, GuildDailyObjectiveNoticeResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_flGuildDailyObjectiveNoticeRemainingCoolTime = Time.realtimeSinceStartup + resBody.remainingCoolTime;

			if (EventGuildDailyObjectiveNotice != null)
			{
				EventGuildDailyObjectiveNotice();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A77_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 길드의 기부횟수가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A77_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_ERROR_002101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일목표완료멤버목록
	public void SendGuildDailyObjectiveCompletionMemberList()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildDailyObjectiveCompletionMemberListCommandBody cmdBody = new GuildDailyObjectiveCompletionMemberListCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildDailyObjectiveCompletionMemberList, cmdBody);
		}
	}

	void OnEventResGuildDailyObjectiveCompletionMemberList(int nReturnCode, GuildDailyObjectiveCompletionMemberListResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			List<CsGuildDailyObjectiveCompletionMember> list = new List<CsGuildDailyObjectiveCompletionMember>();

			for (int i = 0; i < resBody.completionMembers.Length; i++)
			{
				list.Add(new CsGuildDailyObjectiveCompletionMember(resBody.completionMembers[i]));
			}

			if (EventGuildDailyObjectiveCompletionMemberList != null)
			{
				EventGuildDailyObjectiveCompletionMemberList(list);
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A77_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일목표보상받기
	public void SendGuildDailyObjectiveRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildDailyObjectiveRewardReceiveCommandBody cmdBody = new GuildDailyObjectiveRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildDailyObjectiveRewardReceive, cmdBody);
		}
	}

	void OnEventResGuildDailyObjectiveRewardReceive(int nReturnCode, GuildDailyObjectiveRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildDailyObjectiveRewardReceivedDate = resBody.date;
			m_nGuildDailyObjectiveRewardReceivedNo = resBody.rewardNo;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildDailyObjectiveRewardReceive != null)
			{
				EventGuildDailyObjectiveRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A77_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 완료멤버수가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A77_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드주간목표설정
	public void SendGuildWeeklyObjectiveSet(int nObjectiveId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildWeeklyObjectiveSetCommandBody cmdBody = new GuildWeeklyObjectiveSetCommandBody();
			cmdBody.objectiveId = nObjectiveId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildWeeklyObjectiveSet, cmdBody);
		}
	}

	void OnEventResGuildWeeklyObjectiveSet(int nReturnCode, GuildWeeklyObjectiveSetResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventGuildWeeklyObjectiveSet != null)
			{
				EventGuildWeeklyObjectiveSet();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 목표가 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드주간목표보상받기
	public void SendGuildWeeklyObjectiveRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildWeeklyObjectiveRewardReceiveCommandBody cmdBody = new GuildWeeklyObjectiveRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildWeeklyObjectiveRewardReceive, cmdBody);
		}
	}

	void OnEventResGuildWeeklyObjectiveRewardReceive(int nReturnCode, GuildWeeklyObjectiveRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtGuildWeeklyObjectiveRewardReceivedDate = resBody.rewardReceivedDate;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventGuildWeeklyObjectiveRewardReceive != null)
			{
				EventGuildWeeklyObjectiveRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 주간목표가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 완료멤버수가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A78_ERROR_00204"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드영지부활
	public void SendGuildTerritoryRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildTerritoryReviveCommandBody cmdBody = new GuildTerritoryReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildTerritoryRevive, cmdBody);
		}
	}

	void OnEventResGuildTerritoryRevive(int nReturnCode, GuildTerritoryReviveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventGuildTerritoryRevive != null)
			{
				EventGuildTerritoryRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은상태가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A67_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드영지부활에대한길드영지입장
	public void SendGuildTerritoryEnterForGuildTerritoryRevival()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildTerritoryEnterForGuildTerritoryRevivalCommandBody cmdBody = new GuildTerritoryEnterForGuildTerritoryRevivalCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GuildTerritoryEnterForGuildTerritoryRevival, cmdBody);
		}
	}

	void OnEventResGuildTerritoryEnterForGuildTerritoryRevival(int nReturnCode, GuildTerritoryEnterForGuildTerritoryRevivalResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{			
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = resBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = resBody.paidImmediateRevivalDailyCount;

			if (EventGuildTerritoryEnterForGuildTerritoryRevival != null)
			{
				EventGuildTerritoryEnterForGuildTerritoryRevival(resBody.placeInstanceId, resBody.heroes, resBody.monsters, resBody.position, resBody.rotationY);
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드에 가입하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A67_ERROR_00301"));
			// 씬 로드 필요.
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent; // 이전대륙 입장 처리.
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드축복시작
	public void SendGuildBlessingBuffStart(int nGuildBlessingBuffId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GuildBlessingBuffStartCommandBody cmdBody = new GuildBlessingBuffStartCommandBody();
            cmdBody.buffId = nGuildBlessingBuffId;
			CsRplzSession.Instance.Send(ClientCommandName.GuildBlessingBuffStart, cmdBody);
		}
	}

	void OnEventResGuildBlessingBuffStart(int nReturnCode, GuildBlessingBuffStartResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			if (EventGuildBlessingBuffStart != null)
			{
				EventGuildBlessingBuffStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 길드멤버가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A152_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A152_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 오늘은 더 이상 축복버프를 시작할 수 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A152_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 축복버프가 진행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A152_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 목표NPC와 상호작용할 수 없는 위치입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A152_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A152_ERROR_00106"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 길드신청수락
	void OnEventEvtGuildApplicationAccepted(SEBGuildApplicationAcceptedEventBody eventBody) 
	{
		//	CsHeroGuildApplication csHeroGuildApplication = m_listCsHeroGuildApplication.Find(a => a.Id == eventBody.applicationId);
		//	eventBody.applicationId;  <<  처리 필요해보임.

		m_csGuild = new CsGuild(eventBody.guild);
        m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(eventBody.memberGrade);

		m_listCsHeroGuildApplication.Clear();
		m_listCsHeroGuildInvitation.Clear();

		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		if (EventGuildApplicationAccepted != null)
		{
			EventGuildApplicationAccepted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGuildApplicationRefused(SEBGuildApplicationRefusedEventBody eventBody) // 길드신청거절
	{
		CsHeroGuildApplication csHeroGuildApplication = m_listCsHeroGuildApplication.Find(a => a.Id == eventBody.applicationId);
        string strGuildName = csHeroGuildApplication.GuildName;

        m_listCsHeroGuildApplication.Remove(csHeroGuildApplication);

        if (EventGuildApplicationRefused != null)
		{
			EventGuildApplicationRefused(strGuildName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGuildApplicationCountUpdated(SEBGuildApplicationCountUpdatedEventBody eventBody) // 길드신청수갱신
	{
        m_csGuild.ApplicationCount = eventBody.count;

		if (EventGuildApplicationCountUpdated != null)
		{
			EventGuildApplicationCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGuildMemberEnter(SEBGuildMemberEnterEventBody eventBody) // 길드멤버가입
	{
		if (EventGuildMemberEnter != null)
		{
			EventGuildMemberEnter(eventBody.heroId, eventBody.name);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGuildMemberExit(SEBGuildMemberExitEventBody eventBody) // 길드멤버탈퇴
	{
		if (EventGuildMemberExit != null)
		{
			EventGuildMemberExit(eventBody.heroId, eventBody.name, eventBody.isBanished);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGuildBanished(SEBGuildBanishedEventBody eventBody) // 길드강퇴
	{
		m_flGuildRejoinRemainingTime = CsGameConfig.Instance.GuildRejoinIntervalTime + Time.realtimeSinceStartup;

		Reset();

        if (CsGameData.Instance.MyHeroInfo.LocationId == GuildTerritory.LocationId)
        {
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
        }

		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		if (EventGuildBanished != null)
		{
			EventGuildBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅길드정보갱신
	void OnEventEvtHeroGuildInfoUpdated(SEBHeroGuildInfoUpdatedEventBody eventBody) 
	{
		if (EventHeroGuildInfoUpdated != null)
		{
			EventHeroGuildInfoUpdated(eventBody.heroId, eventBody.guildId, eventBody.guildName, eventBody.guildMemberGrade);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드초대도착
	void OnEventEvtGuildInvitationArrived(SEBGuildInvitationArrivedEventBody eventBody) 
	{
		if (EventGuildInvitationArrived != null)
		{
			CsHeroGuildInvitation csHeroGuildInvitation = new CsHeroGuildInvitation(eventBody.invitation);
			m_listCsHeroGuildInvitation.Add(csHeroGuildInvitation);

			EventGuildInvitationArrived(csHeroGuildInvitation);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드초대거절
	void OnEventEvtGuildInvitationRefused(SEBGuildInvitationRefusedEventBody eventBody) 
	{
		if (EventGuildInvitationRefused != null)
		{
			EventGuildInvitationRefused(eventBody.targetId, eventBody.targetName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드초대수명종료
	void OnEventEvtGuildInvitationLifetimeEnded(SEBGuildInvitationLifetimeEndedEventBody eventBody) 
	{
		if (eventBody.targetId == CsGameData.Instance.MyHeroInfo.HeroId)
		{
			m_listCsHeroGuildInvitation.RemoveAll(a => a.Id == eventBody.invitationId);
		}
		
		if (EventGuildInvitationLifetimeEnded != null)
		{
			EventGuildInvitationLifetimeEnded(eventBody.invitationId, eventBody.targetId, eventBody.targetName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드임명
	void OnEventEvtGuildAppointed(SEBGuildAppointedEventBody eventBody) 
	{
		if (eventBody.appointeeId == CsGameData.Instance.MyHeroInfo.HeroId)
		{
            m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(eventBody.appointeeGrade);
		}

		if (EventGuildAppointed != null)
		{ 
			EventGuildAppointed(eventBody.appointerId, eventBody.appointerName, eventBody.appointerGrade, eventBody.appointeeId, eventBody.appointeeName, eventBody.appointeeGrade);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드장위임
	void OnEventEvtGuildMasterTransferred(SEBGuildMasterTransferredEventBody eventBody) 
	{
		if (eventBody.transfereeId == CsGameData.Instance.MyHeroInfo.HeroId)
		{
            m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(1);
		}

		if (EventGuildMasterTransferred != null)
		{
			EventGuildMasterTransferred(eventBody.transfererId, eventBody.transfererName, eventBody.transfereeId, eventBody.transfereeName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드건물레벨업
	void OnEventEvtGuildBuildingLevelUp(SEBGuildBuildingLevelUpEventBody eventBody)
	{
		CsGuildBuildingInstance csGuildBuildingInstance = m_csGuild.GetGuildBuildingInstance(eventBody.buildingId);
		csGuildBuildingInstance.Level = eventBody.buildingLevel;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		if (EventGuildBuildingLevelUpEvent != null)
		{
			EventGuildBuildingLevelUpEvent();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드군량창고징수
	void OnEventEvtGuildFoodWarehouseCollected(SEBGuildFoodWarehouseCollectedEventBody eventBody)
	{
		m_csGuild.FoodWarehouseCollectionId = eventBody.collectionId;

		if (EventGuildFoodWarehouseCollected != null)
		{
			EventGuildFoodWarehouseCollected();
		}
	}

    //---------------------------------------------------------------------------------------------------
    //길드농장퀘스트 상호작용완료
    void OnEventEvtGuildFarmQuestInteractionCompleted(SEBGuildFarmQuestInteractionCompletedEventBody eventBody)
    {
		m_bInteraction = false;
        if (EventGuildFarmQuestInteractionCompleted != null)
        {
            EventGuildFarmQuestInteractionCompleted();
        }

		m_bFarmQuestMissionExcuted = true;
		UpdateGuildFarmQuestState();
    }

    //---------------------------------------------------------------------------------------------------
    //길드농장퀘스트 상호작용취소
    void OnEventEvtGuildFarmQuestInteractionCanceled(SEBGuildFarmQuestInteractionCanceledEventBody eventBody)
    {
		m_bInteraction = false;
        if (EventGuildFarmQuestInteractionCanceled != null)
        {
            EventGuildFarmQuestInteractionCanceled();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //영웅길드농장퀘스트 상호작용시작
    void OnEventEvtHeroGuildFarmQuestInteractionStarted(SEBHeroGuildFarmQuestInteractionStartedEventBody eventBody)
    {
        if (EventHeroGuildFarmQuestInteractionStarted != null)
        {
            EventHeroGuildFarmQuestInteractionStarted(eventBody.heroId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //영웅길드농장퀘스트 상호작용완료
    void OnEventEvtHeroGuildFarmQuestInteractionCompleted(SEBHeroGuildFarmQuestInteractionCompletedEventBody eventBody)
    {
        if (EventHeroGuildFarmQuestInteractionCompleted != null)
        {
            EventHeroGuildFarmQuestInteractionCompleted(eventBody.heroId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //영웅길드농장퀘스트 상호작용취소
    void OnEventEvtHeroGuildFarmQuestInteractionCanceled(SEBHeroGuildFarmQuestInteractionCanceledEventBody eventBody)
    {
        if (EventHeroGuildFarmQuestInteractionCanceled != null)
        {
            EventHeroGuildFarmQuestInteractionCanceled(eventBody.heroId);
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 길드공지변경
	void OnEventEvtGuildNoticeChanged(SEBGuildNoticeChangedEventBody eventbody)
	{
		m_csGuild.Notice = eventbody.notice;

		if (EventGuildNoticeChanged != null)
		{
			EventGuildNoticeChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드자금변경
	void OnEventEvtGuildFundChanged(SEBGuildFundChangedEventBody eventBody)
	{
		m_csGuild.Fund = eventBody.fund;

		if (EventGuildFundChanged != null)
		{
			EventGuildFundChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드모럴포인트변경
	void OnEventEvtGuildMoralPointChanged(SEBGuildMoralPointChangedEventBody eventBody)
	{
		m_csGuild.MoralPoint = eventBody.moralPoint;
		m_csGuild.MoralPointDate = eventBody.date;

		if (EventGuildMoralPointChanged != null)
		{
			EventGuildMoralPointChanged();
		}
	}

    //---------------------------------------------------------------------------------------------------
    //길드제단 마력주입미션 완료
    void OnEventEvtGuildAltarSpellInjectionMissionCompleted(SEBGuildAltarSpellInjectionMissionCompletedEventBody eventBody)
    {
		m_bSpellInjection = false;
        m_dtGuildMoralPointDate = eventBody.date;
        m_nGuildMoralPoint = eventBody.guildMoralPoint;
        m_csGuild.MoralPoint = eventBody.giMoralPoint;

        if (EventGuildAltarSpellInjectionMissionCompleted != null)
        {
            EventGuildAltarSpellInjectionMissionCompleted();
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 길드제단 마력주입미션 취소
    void OnEventEvtGuildAltarSpellInjectionMissionCanceled(SEBGuildAltarSpellInjectionMissionCanceledEventBody eventBody)
    {
		m_bSpellInjection = false;
        if (EventGuildAltarSpellInjectionMissionCanceled != null)
        {
            EventGuildAltarSpellInjectionMissionCanceled();
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 영웅길드제단 마력주입미션 시작
    void OnEventEvtHeroGuildAltarSpellInjectionMissionStarted(SEBHeroGuildAltarSpellInjectionMissionStartedEventBody eventBody)
    {
        if (EventHeroGuildAltarSpellInjectionMissionStarted != null)
        {
            EventHeroGuildAltarSpellInjectionMissionStarted(eventBody.heroId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 영웅길드제단 마력주입미션 완료
    void OnEventEvtHeroGuildAltarSpellInjectionMissionCompleted(SEBHeroGuildAltarSpellInjectionMissionCompletedEventBody eventBody)
    {
        if (EventHeroGuildAltarSpellInjectionMissionCompleted != null)
        {
            EventHeroGuildAltarSpellInjectionMissionCompleted(eventBody.heroId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 영웅길드제단 마력주입 미션 취소
    void OnEventEvtHeroGuildAltarSpellInjectionMissionCanceled(SEBHeroGuildAltarSpellInjectionMissionCanceledEventBody eventBody)
    {
        if (EventHeroGuildAltarSpellInjectionMissionCanceled != null)
        {
            EventHeroGuildAltarSpellInjectionMissionCanceled(eventBody.heroId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 길드제단 수비미션 완료
    void OnEventEvtGuildAltarDefenseMissionCompleted(SEBGuildAltarDefenseMissionCompletedEventBody eventBody)
    {
        m_dtGuildMoralPointDate = eventBody.date;
        m_nGuildMoralPoint = eventBody.guildMoralPoint;
        m_csGuild.MoralPoint = eventBody.giMoralPoint;
		m_bGuildDefense = false;

        if (EventGuildAltarDefenseMissionCompleted != null)
        {
            EventGuildAltarDefenseMissionCompleted();
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 길드제단 수비미션 실패
    void OnEventEvtGuildAltarDefenseMissionFailed(SEBGuildAltarDefenseMissionFailedEventBody eventBody)
    {
		m_bGuildDefense = false;

        if (EventGuildAltarDefenseMissionFailed != null)
        {
            EventGuildAltarDefenseMissionFailed();
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 길드미션실패
	void OnEventEvtGuildMissionFailed(SEBGuildMissionFailedEventBody eventBody) // 당사자
	{
        m_csGuildMission = null;
        m_csGuildMissionMonster = null;
        m_nMissionProgressCount = 0;
        UpdateGuildMission();
        if (EventGuildMissionFailed != null)
		{
			EventGuildMissionFailed();
		}
	}
	//---------------------------------------------------------------------------------------------------
	// 길드미션완료
	void OnEventEvtGuildMissionComplete(SEBGuildMissionCompletedEventBody eventBody)
	{
		Debug.Log("#####     OnEventResGuildMissionComplete()     #####");
		CsMyHeroInfo csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
		int nOldLevel = csMyHeroInfo.Level;

		//DateTime GuildMissionCompleteDate = resBody.date;
		m_nMissionCompletedCount = eventBody.completedMissionCount;

        int nOldGuildContributionPoint = m_nGuildContributionPoint;
		m_nGuildContributionPoint = eventBody.guildContributionPoint;
		m_nTotalGuildContributionPoint = eventBody.totalGuildContributionPoint;

		if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
        }

        int nOldBuildingPoint = m_csGuild.BuildingPoint;
		m_csGuild.BuildingPoint = eventBody.giBuildingPoint;
        
        if (0 < m_csGuild.BuildingPoint - nOldBuildingPoint)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDB"), m_csGuild.BuildingPoint - nOldBuildingPoint));
        }

        long lOldGuildFund = m_csGuild.Fund;
		m_csGuild.Fund = eventBody.giFund;

        if (0 < m_csGuild.Fund - lOldGuildFund)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDM"), m_csGuild.Fund - lOldGuildFund));
        }

		csMyHeroInfo.Level = eventBody.level;
		csMyHeroInfo.Exp = eventBody.exp;
		csMyHeroInfo.MaxHp = eventBody.maxHP;
		csMyHeroInfo.Hp = eventBody.hp;
		csMyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		if (eventBody.nextMission != null)
		{
			m_csGuildMission = GuildMissionQuest.GetGuildMission(eventBody.nextMission.missionId);
			m_nMissionProgressCount = eventBody.nextMission.progressCount;
			m_csGuildMissionMonster = new CsGuildMissionMonster(eventBody.nextMission.monsterInstanceId,
																eventBody.nextMission.monsterSpawnedContinentId,
																CsRplzSession.Translate(eventBody.nextMission.monsterPosition),
																eventBody.nextMission.remainingMonsterLifetime);
		}
		else // 모든 미션 완료.
		{
			m_bMissionCompleted = true;
		}

		UpdateGuildMission();

		if (EventGuildMissionComplete != null)
		{
			bool bLevelUp = (nOldLevel == csMyHeroInfo.Level) ? false : true;
			EventGuildMissionComplete(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드미션갱신
	void OnEventEvtGuildMissionUpdated(SEBGuildMissionUpdatedEventBody eventBody) // 당사자
	{
		Debug.Log("#####     OnEventEvtGuildMissionUpdated     #####");
		m_nMissionProgressCount = eventBody.progressCount;
		UpdateGuildMission();
	}

	//---------------------------------------------------------------------------------------------------
	// 길드정신알림
	void OnEventEvtGuildSpiritAnnounced(SEBGuildSpiritAnnouncedEventBody eventBody) // 모든 국민
	{
        CsChattingMessage csChattingMessage = new CsChattingMessage((int)EnChattingType.Nation, (int)EnNoticeType.GuildApply, eventBody.guildId, eventBody.guildName, eventBody.heroId, eventBody.heroName, eventBody.continentId);
        CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);

		if (EventGuildSpiritAnnounced != null)
		{
			EventGuildSpiritAnnounced(csChattingMessage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드소집
	void OnEventEvtGuildCall(SEBGuildCallEventBody eventBody)
	{
		CsGuildCall csGuildCall = new CsGuildCall(eventBody.call);

		if (EventGuildCalled != null)
		{
			EventGuildCalled(csGuildCall);
		}
	}

    //---------------------------------------------------------------------------------------------------
    // 길드 물자지원 퀘스트 시작 (당사자외)
    void OnEventEvtGuildSupplySupportQuestStarted(SEBGuildSupplySupportQuestStartedEventBody eventBody)
    {
		Debug.Log("$$$$$$$$$$OnEventEvtGuildSupplySupportQuestStarted$$$$$$$$$$$$");
        m_dtDailyGuildSupplySupportQuestStartCountDate = eventBody.date;
        m_nDailyGuildSupplySupportQuestStartCount = eventBody.dailyGuildSupplySupportQuestStartCount;

        if (EventGuildSupplySupportQuestStarted != null)
        {
            EventGuildSupplySupportQuestStarted();
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 길드 물자지원 퀘스트 완료 (당사자외)
    void OnEventEvtGuildSupplySupportQuestCompleted(SEBGuildSupplySupportQuestCompletedEventBody eventBody)
    {
		Debug.Log("$$$$$$$$$$$$$OnEventEvtGuildSupplySupportQuestCompleted$$$$$$$$$$");

        int nOldGuildContributionPoint = m_nGuildContributionPoint;
		m_nGuildContributionPoint = eventBody.guildContributionPoint;
		m_nTotalGuildContributionPoint = eventBody.totalGuildContributionPoint;

		if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
        }

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
        CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;
        if (EventGuildSupplySupportQuestCompleted != null)
        {
            EventGuildSupplySupportQuestCompleted(bLevelUp, eventBody.acquiredExp);
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 길드 물자지원 퀘스트 실패
    void OnEventEvtGuildSupplySupportQuestFail(SEBGuildSupplySupportQuestFailEventBody eventBody)
    {
		Debug.Log("$$$$$$$$$$$$$OnEventEvtGuildSupplySupportQuestFail$$$$$$$$$$");
		UpdateGuildSupplySupportState(true);
        if (EventGuildSupplySupportQuestFail != null)
        {
            EventGuildSupplySupportQuestFail();
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 길드 제단 퀘스트 완료
    void OnEventEvtGuildAltarCompleted(SEBGuildAltarCompletedEventBody eventBody)
    {
        CsMyHeroInfo csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
        int nOldLevel = csMyHeroInfo.Level;

        csMyHeroInfo.Level = eventBody.level;
        csMyHeroInfo.Exp = eventBody.exp;
        csMyHeroInfo.MaxHp = eventBody.maxHp;
        csMyHeroInfo.Hp = eventBody.hp;

        int nOldGuildContributionPoint = m_nGuildContributionPoint;
        m_nGuildContributionPoint = eventBody.contributionPoint;

        if (0 < m_nGuildContributionPoint - nOldGuildContributionPoint)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDC"), m_nGuildContributionPoint - nOldGuildContributionPoint));
        }

        m_nTotalGuildContributionPoint = eventBody.totalContributionPoint;

        long lOldGuildFund = m_csGuild.Fund;
        m_csGuild.Fund = eventBody.giFund;

        if (0 < m_csGuild.Fund - lOldGuildFund)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDM"), m_csGuild.Fund - lOldGuildFund));
        }

        int nOldBuildingPoint = m_csGuild.BuildingPoint;
        m_csGuild.BuildingPoint = eventBody.giBuildingPoint;

        if (0 < m_csGuild.BuildingPoint - nOldBuildingPoint)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GUILDB"), m_csGuild.BuildingPoint - nOldBuildingPoint));
        }

        if (EventGuildAltarCompleted != null)
        {
            bool bLevelUp = (nOldLevel == csMyHeroInfo.Level) ? false : true;
            EventGuildAltarCompleted(bLevelUp, eventBody.acquiredExp);
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅퀘스트갱신
	void OnEventEvtGuildHuntingQuestUpdated(SEBGuildHuntingQuestUpdatedEventBody eventBody)
	{
		m_csHeroGuildHuntingQuest.ProgressCount = eventBody.progressCount;
		UpdateGuildHuntingQuestState();

		if (EventGuildHuntingQuestUpdated != null)
		{
			EventGuildHuntingQuestUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드헌팅기부횟수갱신
	void OnEventEvtGuildHuntingDonationCountUpdated(SEBGuildHuntingDonationCountUpdatedEventBody eventBody)
	{
		m_csGuild.DailyHuntingDonationCount = eventBody.donationCount;

		if (EventGuildHuntingDonationCountUpdated != null)
		{
			EventGuildHuntingDonationCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일목표알림
	void OnEventEvtGuildDailyObjectiveNotice(SEBGuildDailyObjectiveNoticeEventBody eventBody)
	{
        CsChattingMessage csChattingMessage = new CsChattingMessage((int)EnChattingType.Guild, (int)EnNoticeType.GuildEvent, eventBody.hero, eventBody.contentId);

        CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);

        if (EventGuildDailyObjectiveNoticeEvent != null)
		{
			EventGuildDailyObjectiveNoticeEvent(csChattingMessage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일목표설정
	void OnEventEvtGuildDailyObjectiveSet(SEBGuildDailyObjectiveSetEventBody eventBody)
	{
		m_csGuild.DailyObjectiveDate = eventBody.date;
		m_csGuild.DailyObjectiveContentId = eventBody.contentId;

		if (EventGuildDailyObjectiveSet != null)
		{
			EventGuildDailyObjectiveSet();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드일일목표완료멤버수갱신
	void OnEventEvtGuildDailyObjectiveCompletionMemberCountUpdated(SEBGuildDailyObjectiveCompletionMemberCountUpdatedEventBody eventBody)
	{
		m_csGuild.DailyObjectiveCompletionMemberCount = eventBody.count;

		if (EventGuildDailyObjectiveCompletionMemberCountUpdated != null)
		{
			EventGuildDailyObjectiveCompletionMemberCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드주간목표설정
	void OnEventEvtGuildWeeklyObjectiveSet(SEBGuildWeeklyObjectiveSetEventBody eventBody)
	{
		m_csGuild.WeeklyObjectiveDate = eventBody.date;
		m_csGuild.WeeklyObjectiveId = eventBody.objectiveId;

		if (EventGuildWeeklyObjectiveSetEvent != null)
		{
			EventGuildWeeklyObjectiveSetEvent();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드주간목표완료멤버수갱신
	void OnEventEvtGuildWeeklyObjectiveCompletionMemberCountUpdated(SEBGuildWeeklyObjectiveCompletionMemberCountUpdatedEventBody eventBody)
	{
		m_csGuild.WeeklyObjectiveCompletionMemberCount = eventBody.count;

		if (EventGuildWeeklyObjectiveCompletionMemberCountUpdated != null)
		{
			EventGuildWeeklyObjectiveCompletionMemberCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드축복시작
	void OnEventEvtGuildBlessingBuffStarted(SEBGuildBlessingBuffStartedEventBody eventBody)
	{
		m_csGuild.LastBlessingBuffStartDate = eventBody.blessingBuffStartDate;
		m_csGuild.IsBlessingBuffRunning = true;

        CsChattingMessage csChattingMessage = new CsChattingMessage(true);
        CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);
		
		if (EventGuildBlessingBuffStarted != null)
		{
            EventGuildBlessingBuffStarted(csChattingMessage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드축복종료
	void OnEventEvtGuildBlessingBuffEnded(SEBGuildBlessingBuffEndedEventBody eventBody)
	{
		m_csGuild.IsBlessingBuffRunning = false;

        CsChattingMessage csChattingMessage = new CsChattingMessage(false);
        CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);

		if (EventGuildBlessingBuffEnded != null)
		{
            EventGuildBlessingBuffEnded(csChattingMessage);
		}
	}

	#endregion Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 길드농장
	public void SetGuildFarmQuestInfo(PDHeroGuildFarmQuest heroGuildFarmQuest, int nDailyGuildFarmQuestStartCount)
	{
		m_nDailyGuildFarmQuestStartCount = nDailyGuildFarmQuestStartCount;
		if (heroGuildFarmQuest == null)
		{
			UpdateGuildFarmQuestState(true);
		}
		else
		{
			m_csHeroGuildFarmQuest = new CsHeroGuildFarmQuest(heroGuildFarmQuest);
			m_bFarmQuestMissionExcuted = heroGuildFarmQuest.isObjectiveCompleted;
			UpdateGuildFarmQuestState();
		}
	}


	//---------------------------------------------------------------------------------------------------
	void UpdateGuildFarmQuestState(bool bReset = false)
    {
        if (bReset)
		{
			if (m_nDailyGuildFarmQuestStartCount >= CsGameData.Instance.GuildFarmQuest.LimitCount)
			{
				m_enGuildFarmQuestState = EnGuildFarmQuestState.Competed;
			}
			else
			{
				m_csHeroGuildFarmQuest = null;
				m_bFarmQuestMissionExcuted = false;
				m_enGuildFarmQuestState = EnGuildFarmQuestState.None;
			}
		}
        else
		{
			if (m_bFarmQuestMissionExcuted)
			{
				m_enGuildFarmQuestState = EnGuildFarmQuestState.Executed;
			}
			else
			{
				m_enGuildFarmQuestState = EnGuildFarmQuestState.Accepted;
			}
        }

		Debug.Log("UpdateGuildFarmQuestState      m_enGuildFarmQuestState = " + m_enGuildFarmQuestState);
        if (EventUpdateFarmQuestState != null)
        {
            EventUpdateFarmQuestState();
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 길드미션
	void SetGuildMission(PDHeroGuildMissionQuest heroGuildMissionQuest)
	{
		if (heroGuildMissionQuest != null) // 길드미션 퀘스트.
		{
			m_bMissionQuest = true;
			m_bMissionCompleted = heroGuildMissionQuest.completed;
			m_nMissionCompletedCount = heroGuildMissionQuest.completedMissionCount;

			if (heroGuildMissionQuest.currentMission != null)
			{
				m_csGuildMission = GuildMissionQuest.GetGuildMission(heroGuildMissionQuest.currentMission.missionId);
				m_nMissionProgressCount = heroGuildMissionQuest.currentMission.progressCount;

				if (heroGuildMissionQuest.currentMission.monsterInstanceId != 0) // 소환몬스터 처치 미션인 경우.
				{
					m_csGuildMissionMonster = new CsGuildMissionMonster(heroGuildMissionQuest.currentMission.monsterInstanceId,
																		heroGuildMissionQuest.currentMission.monsterSpawnedContinentId,
																		CsRplzSession.Translate(heroGuildMissionQuest.currentMission.monsterPosition),
																		heroGuildMissionQuest.currentMission.remainingMonsterLifetime);
				}

				UpdateGuildMission();
				return;
			}

			UpdateGuildMission(true);
			return;
		}

		m_bMissionQuest = false;
		UpdateGuildMission(true);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateGuildMission(bool bReset = false)
	{
		if (bReset)
		{
			m_csGuildMission = null;
			m_csGuildMissionMonster = null;
			m_bMissionCompleted = false;
			m_nMissionProgressCount = 0;
			m_enGuildMissionState = EnGuildMissionState.None;
		}
		else
		{
			if (GuildMissionQuest.LimitCount == m_nMissionCompletedCount)
			{
				m_bMissionCompleted = true;
			}

			if (m_bMissionCompleted)  // 모든 퀘스트 완료시
			{
				m_csGuildMission = null;
				m_csGuildMissionMonster = null;
				m_nMissionProgressCount = 0;
				m_enGuildMissionState = EnGuildMissionState.Competed;				
			}
			else
			{
				m_enGuildMissionState = EnGuildMissionState.Accepted; // 미션 목표 미완료

                if (m_csGuildMission != null && m_csGuildMission.TargetCount <= m_nMissionProgressCount)	// 체크필요.
                {
					dd.d("UpdateGuildMission       확인중 20180803 광열.");
                    return;
                }
            }
		}

		dd.d("UpdateGuildMission       m_enGuildMissionState = ", m_enGuildMissionState);
		if (EventUpdateMissionState != null)
		{
			EventUpdateMissionState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드 물자지원
	void SetGuildSupplySupport(PDGuildSupplySupportQuestPlay guildSupplySupportQuestPlay)
	{
		if (guildSupplySupportQuestPlay != null)
		{
			m_csGuildSupplySupportQuestPlay = new CsGuildSupplySupportQuestPlay(guildSupplySupportQuestPlay);
			m_flGuildSupplySupportQuestRemainingTime = m_csGuildSupplySupportQuestPlay.RemainingTime + Time.realtimeSinceStartup;
			UpdateGuildSupplySupportState();
		}
		else
		{	
			UpdateGuildSupplySupportState(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateGuildSupplySupportState(bool bReset = false)
	{
		if (bReset)
		{
			m_csGuildSupplySupportQuestPlay = null;
			m_flGuildSupplySupportQuestRemainingTime = 0f;
			m_enGuildSupplySupportState = EnGuildSupplySupportState.None;
		}
		else
		{
			if (m_flGuildSupplySupportQuestRemainingTime == 0) // 수락전
			{
				m_enGuildSupplySupportState = EnGuildSupplySupportState.None;
			}
			else
			{
				m_enGuildSupplySupportState = EnGuildSupplySupportState.Accepted;
			}
		}

		dd.d("UpdateGuildSupplySupportState        GuildSupplySupportState = ", m_enGuildSupplySupportState);
		if (EventUpdateSupplySupportState != null)
		{
			EventUpdateSupplySupportState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 길드 헌팅
	void SetGuildHunting(PDHeroGuildHuntingQuest heroGuildHuntingQuest)
	{
		if (heroGuildHuntingQuest != null)
		{
			m_csHeroGuildHuntingQuest = new CsHeroGuildHuntingQuest(heroGuildHuntingQuest);
			m_csGuildHuntingQuestObjective = CsGameData.Instance.GuildHuntingQuest.GuildHuntingQuestObjectiveList.Find(a => a.ObjectiveId == m_csHeroGuildHuntingQuest.ObjectiveId);
			UpdateGuildHuntingQuestState();
		}
		else
		{
			UpdateGuildHuntingQuestState(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateGuildHuntingQuestState(bool bReset = false)
	{
		if (bReset)
		{
			m_csHeroGuildHuntingQuest = null;
			if (m_nDailyGuildHuntingQuestStartCount == CsGuildManager.Instance.GuildHuntingQuest.LimitCount)
			{
				m_enGuildHuntingState = EnGuildHuntingState.Competed;
			}
			else
			{
				m_enGuildHuntingState = EnGuildHuntingState.None;
			}
		}
		else
		{
			if (m_csHeroGuildHuntingQuest.ProgressCount < m_csGuildHuntingQuestObjective.TargetCount)
			{
				m_enGuildHuntingState = EnGuildHuntingState.Accepted;
			}
			else
			{
				m_enGuildHuntingState = EnGuildHuntingState.Executed;
			}
		}
		dd.d("UpdateGuildHuntingQuestState        GuildHuntingState = ", m_enGuildHuntingState);
		if (EventUpdateGuildHuntingQuestState != null)
		{
			EventUpdateGuildHuntingQuestState();
		}
	}

    //---------------------------------------------------------------------------------------------------
    public bool CheckWeeklyObjectiveSettingEnabled()
    {
        if (CsGuildManager.Instance.Guild == null)
        {
            return false;
        }
        else
        {
            if (CsGuildManager.Instance.MyGuildMemberGrade.WeeklyObjectiveSettingEnabled && CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == DayOfWeek.Monday && CsGuildManager.Instance.Guild.WeeklyObjectiveId == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}