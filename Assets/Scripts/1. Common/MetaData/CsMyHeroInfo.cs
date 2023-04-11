using System;
using System.Collections.Generic;
using UnityEngine;
using WebCommon;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-07)
//---------------------------------------------------------------------------------------------------

public class CsMyHeroInfo : CsHeroBase
{
	// 서버시간 (GetTime)
	DateTimeOffset m_dtOfsTime;
	DateTime m_dateTimeBase;
	float m_flStartRealtimeSinceStartup;

	long m_lExp;                                            // 경험치
	long m_lBattlePower;                                    // 전투력

	int m_nLak;                                             // 라크

	DateTime m_dtLoginDate;

	// 메인장비
	List<CsHeroMainGear> m_listCsHeroMainGear;              // 영웅메인장비
	List<CsHeroMainGear> m_listCsHeroMainGearEquipped;      // 장착장비

	// 보조장비
	List<CsHeroSubGear> m_listCsHeroSubGear;                // 보조장비

	// 인벤토리
	int m_nPaidInventorySlotCount;
	int m_nInventorySlotCount;
	List<CsInventorySlot> m_listCsInventorySlot;

	// 장고
	int m_nPaidWarehouseSlotCount;
	List<CsWarehouseSlot> m_listCsWarehouseSlot;

	// 메일
	List<CsMail> m_listCsMail;                  // 메일리스트

	// 초기입장위치
	int m_nInitEntranceLocationId;              // 초기입장위치ID
	int m_nInitEntranceLocationParam;           // 대륙 : 국가ID (국가영토인 경우 국가ID, 공용대륙인 경우 0)
	int m_nPreviousContinentId;
	int m_nPreviousNationId;

	int m_nLocationId;                          // 위치 ID

	// 스킬리스트.
	List<CsHeroSkill> m_listCsHeroSkill;

	// 메인장비인챈트
	DateTime m_dtDateMainGearEnchant;
	int m_nMainGearEnchantDailyCount;

	// 메인장비세련
	DateTime m_dtDateMainGearRefine;
	int m_nMainGearRefineDailyCount;

	// 재화
	long m_lGold;
	int m_nOwnDia;
	int m_nUnOwnDia;
	int m_nVipPoint;
	CsVipLevel m_csVipLevel;
	List<int> m_listReceivedVipLevelReward;


	// 금일무료즉시부활
	DateTime m_dtDateFreeImmediateRevival;
	int m_nFreeImmediateRevivalDailyCount;

	// 금일유료즉시부활
	DateTime m_dtDatePaidImmediateRevival;
	int m_nPaidImmediateRevivalDailyCount;

	// 금일경험치물약사용횟수
	DateTime m_dtDateExpPotionUseCount;
	int m_nExpPotionDailyUseCount;

	// 받은 레벨업보상 목록
	List<int> m_listReceivedLevelUpRewardList;

	// 받은 일일접속시간보상 목록
	DateTime m_dtDateDailyAccessReward;
	List<int> m_listReceivedDailyAccessRewardList;

	// 일일접속시간
	float m_flDailyAccessTime;

	// 받은 출석보상날짜, 없을 경우 DateTime.MinValue.Date
	DateTime m_dtDateReceivedAttendReward;
	int m_nReceivedAttendRewardDay;

	// 휴식보상(분)
	int m_nRestTime;

	// 파티
	CsParty m_csParty;
	List<CsPartyApplication> m_listCsPartyApplication;  // 신청리스트
	List<CsPartyInvitation> m_listCsPartyInvitation;    // 초대받은리스트

	// 탈것
	List<CsHeroMount> m_listCsHeroMount;                // 보유한 탈것 목록
	int m_nEquippedMountId;                             // 장착한 탈것ID
	bool m_bIsRiding;                                   // 탈것탑승여부

	// 탈것장비
	List<CsHeroMountGear> m_listCsHeroMountGear;        // 보유한 탈것장비 목록
	List<Guid> m_listEquippedMountGear;                 // 장착한 탈것장비ID 목록

	DateTime m_dtDateMountGearRefinement;               // 탈것장비 일일재강화 날짜
	int m_nMountGearRefinementDailyCount;               // 탈것장비 일일재강화횟수

	// 스테미나
	int m_nStamina;                                     // 스태미나
	float m_flStaminaAutoRecoveryRemainingTime;         // 스태미나자동회복남은시간(단위:초)
	int m_nDailyStaminaBuyCount;                        // 금일 체력구매횟수
	DateTime m_dtStaminaBuyCountDate;

	List<CsHeroWing> m_listCsHeroWing;                  // 보유 영웅 날개 목록
	int m_nEquippedWingId;                              // 장착된 영웅날개ID. 없을 경우 0

	int m_nWingStep;                                    // 날개단계
	int m_nWingLevel;                                   // 날개레벨
	int m_nWingExp;                                     // 날개경험치
	List<CsHeroWingPart> m_listCsHeroWingPart;          // 날개부위목록

	DateTime m_dtDateFreeSweepCount;                    // 무료소탕날짜
	int m_nFreeSweepDailyCount;                         // 금일무료소탕횟수

	int m_nMainGearEnchantLevelSetNo;                   // 활성화된 메인장비강화레벨세트번호
	int m_nSubGearSoulstoneLevelSetNo;                  // 활성화된 보조장비소울스톤레벨세트번호

	DateTime m_dtDateExpScrollUseCount;
	int m_nExpScrollDailyUseCount;                      // 금일경험치주문서사용횟수
	int m_nExpScrollItemId;                             // 사용중인 경험치주문서아이템ID
	float m_flExpScrollRemainingTime;                   // 사용중인 경험치주문서남은시간

	bool m_bIsCreateEnter = false;                      // 캐릭터 생성에 의한 입장 여부.

	// 공적포인트
	int m_nExploitPoint;
	DateTime m_dtDateExploitPoint;
	int m_nDailyExploitPoint;

	EnMyHeroEnterType m_enContinentEnterType = EnMyHeroEnterType.InitEnter;   // Continent 입장 타입.

	List<CsHeroSeriesMission> m_listCsHeroSeriesMission;    // 진행중인 연속미션 목록
	List<CsHeroTodayMission> m_listCsHeroTodayMission;      // 진행중인 오늘의미션 목록

	List<CsHeroTodayTask> m_listCsHeroTodayTask;        //오늘의할일 목록

	int m_nAchievementDailyPoint;                       //금일 달성포인트
	DateTime m_dtDateAchievementPoint;
	int m_nReceivedAchievementRewardNo;                 //금일 받은 달성보상번호
	DateTime m_dtDateReceivedAchievementReward;

	// 보유 명예포인트
	int m_nHonorPoint;

	DateTime m_dtRankRewardReceivedDate;    // 계급보상받은날짜
	int m_nRankRewardReceivedRankNo;        // 계급보상받은계급번호

	// 서버레벨랭킹
	int m_nDailyServerLevelRakingNo;            // 현재 일일서버레벨랭킹번호	
	int m_nDailyServerLevelRanking;             // 나의 현재 일일서버레벨랭킹. 순위권밖이면 0
	int m_nRewardedDailyServerLevelRankingNo;   // 보상받은 일일서버레벨랭킹번호. 없으면 0

	// 보상받은 도달항목번호. 없을 경우 0
	int m_nRewardedAttainmentEntryNo;

	// 왜곡주문서사용
	int m_nDistortionScrollDailyUseCount;       // 금일 왜곡주문서사용횟수
	DateTime m_dtDateDistortionScrollUseCount;
	float m_flRemainingDistortionTime;          // 왜곡남은시간(단위:초)

	PDCartInstance m_cartInstance;

	Dictionary<int, long> m_dicAttrValue = new Dictionary<int, long>();

	long m_lNationFund;                                             // 국가자금
	List<CsNationNoblesseInstance> m_listCsNationNoblesseInstance;  // 국가관직인스턴스 목록
	int m_nDailyNationDonationCount;                                // 금일 일일국가기부횟수
	DateTime m_dtNationDonationCountDate;

	List<CsNationInstance> m_listCsNationInstance;
	CsNationInstance m_csNationInstance;

	DateTime m_dtServerOpenDate;

	int m_nExplorationPoint;            // 탐험점수

	int m_nSoulPowder;

	int m_nLootingItemMinGrade;

	bool m_bTodayMissionTutorialStarted;

	int m_nServerMaxLevel;      // 서버최고레벨

	// Customizing
	int m_nCustomPresetHair;

	int m_nCustomFaceJawHeight;
	int m_nCustomFaceJawWidth;
	int m_nCustomFaceJawEndHeight;
	int m_nCustomFaceWidth;
	int m_nCustomFaceEyebrowHeight;
	int m_nCustomFaceEyebrowRotation;
	int m_nCustomFaceEyesWidth;
	int m_nCustomFaceNoseHeight;
	int m_nCustomFaceNoseWidth;
	int m_nCustomFaceMouthHeight;
	int m_nCustomFaceMouthWidth;

	int m_nCustomBodyHeadSize;
	int m_nCustomBodyArmsLength;
	int m_nCustomBodyArmsWidth;
	int m_nCustomBodyChestSize;
	int m_nCustomBodyWaistWidth;
	int m_nCustomBodyHipsSize;
	int m_nCustomBodyPelvisWidth;
	int m_nCustomBodyLegsLength;
	int m_nCustomBodyLegsWidth;

	int m_nCustomColorSkin;
	int m_nCustomColorEyes;
	int m_nCustomColorBeardAndEyebrow;
	int m_nCustomColorHair;

	// NPC상점구매목록
	List<CsHeroNpcShopProduct> m_listCsHeroNpcShopProduct;

	int m_nSpiritStone;

	// 계급스킬
	List<CsHeroRankActiveSkill> m_listCsHeroRankActiveSkill;
	List<CsHeroRankPassiveSkill> m_listCsHeroRankPassiveSkill;
	int m_nSelectedRankActiveSkillId;
	float m_flRankActiveSkillRemainingCoolTime;

	int m_nRookieGiftNo;
	float m_flRookieGiftRemainingTime;
	List<int> m_listReceivedOpenGiftReward;
	DateTime m_dtRegDate;

	List<int> m_listRewardedOpen7DayEventMission;
	List<int> m_listPurchasedOpen7DayEventProduct;
	List<CsHeroOpen7DayEventProgressCount> m_listCsHeroOpen7DayEventProgressCount;

	// 회수
	List<CsHeroRetrievalProgressCount> m_listCsHeroRetrievalProgressCount;
	List<CsHeroRetrieval> m_listCsHeroRetrieval;

	// 할일 위탁
	List<CsHeroTaskConsignment> m_listCsHeroTaskConsignment;
	List<CsHeroTaskConsignmentStartCount> m_listCsHeroTaskConsignmentStartCount;

	// 보상받은 한정선물스케쥴ID 목록
	List<int> m_listRewardedLimitationGiftScheduleId;
	// 주말보상
	CsHeroWeekendReward m_csHeroWeekendReward;

	// 금일 다이아상점상품구매횟수 목록
	List<CsHeroDiaShopProductBuyCount> m_listCsHeroDiaShopProductBuyCount;
	List<CsHeroDiaShopProductBuyCount> m_listCsHeroDiaShopProductBuyCountTotal;

	// 공포의제단 성물
	int m_nWeeklyFearAltarHalidomCollectionRewardNo;
	List<int> m_listWeeklyFearAltarHalidom;
	List<int> m_listWeeklyRewardReceivedFearAltarHalidomElemental;

	// 오픈7일이벤트보상여부
	bool m_bOpen7DayEventRewarded;

	// 국가랭킹목록
	List<CsNationPowerRanking> m_listCsNationPowerRanking;

	// 물약속성 목록
	List<CsHeroPotionAttr> m_listCsHeroPotionAttr;

	//---------------------------------------------------------------------------------------------------
	public DateTime LoginDate
	{
		get { return m_dtLoginDate; }
	}

	public DateTimeOffset TimeOffset
	{
		get { return m_dtOfsTime; }
		set
		{
			m_dtOfsTime = value;
			m_dateTimeBase = m_dtOfsTime.DateTime;
			m_flStartRealtimeSinceStartup = Time.realtimeSinceStartup;
		}
	}

	public DateTime DateTimeBase
	{
		get { return m_dateTimeBase; }
	}

	public float StartRealtimeSinceStartup
	{
		get { return m_flStartRealtimeSinceStartup; }
	}

	public DateTime CurrentDateTime
	{
		get { return m_dateTimeBase.AddSeconds(Time.realtimeSinceStartup - m_flStartRealtimeSinceStartup); }
	}

	public DateTime ServerOpenDate
	{
		get { return m_dtServerOpenDate; }
	}

	public long Exp
	{
		get { return m_lExp; }
		set { m_lExp = value; }
	}

	public long RequiredExp
	{
		get { return m_csJobLevel.NextLevelUpExp; }
	}

	public float ExpPercent
	{
		get { return ((m_lExp * 100.0f) / m_csJobLevel.NextLevelUpExp); }
	}

	public List<CsHeroMainGear> HeroMainGearList
	{
		get { return m_listCsHeroMainGear; }
	}

	public List<CsHeroMainGear> HeroMainGearEquippedList
	{
		get { return m_listCsHeroMainGearEquipped; }
	}

	public int InitEntranceLocationId
	{
		get { return m_nInitEntranceLocationId; }
	}

	public int InitEntranceLocationParam
	{
		get { return m_nInitEntranceLocationParam; }
		set { m_nInitEntranceLocationParam = value; }
	}

	public int PreviousContinentId
	{
		get { return m_nPreviousContinentId; }
	}

	public int PreviousNationId
	{
		get { return m_nPreviousNationId; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
		set { m_nLocationId = value; }
	}

	public long BattlePower
	{
		get { return m_lBattlePower; }
	}

	public int Lak
	{
		get { return m_nLak; }
		set
		{
			m_nLak = value;
			CsGameEventUIToUI.Instance.OnEventLakAcquisition();
		}
	}

	public int InventorySlotCount
	{
		get { return m_nPaidInventorySlotCount + m_csJobLevel.InventorySlotAccCount; }
	}

	public int InventorySlotMaxCount
	{
		get
		{
			return CsGameData.Instance.JobLevelMasterList[CsGameData.Instance.JobLevelMasterList.Count - 1].InventorySlotAccCount
				  + CsGameData.Instance.InventorySlotExtendRecipeList.Count;
		}
	}

	public int PaidInventorySlotCount
	{
		get { return m_nPaidInventorySlotCount; }
		set { m_nPaidInventorySlotCount = value; }
	}

	public List<CsInventorySlot> InventorySlotList
	{
		get { return m_listCsInventorySlot; }
	}

	public int PaidWarehouseSlotCount
	{
		get { return m_nPaidWarehouseSlotCount; }
		set { m_nPaidWarehouseSlotCount = value; }
	}

	public List<CsWarehouseSlot> WarehouseSlotList
	{
		get { return m_listCsWarehouseSlot; }
	}

	public List<CsHeroSubGear> HeroSubGearList
	{
		get { return m_listCsHeroSubGear; }
	}

	public List<CsHeroSkill> HeroSkillList
	{
		get { return m_listCsHeroSkill; }
	}

	public DateTime MainGearEnchantDate
	{
		get { return m_dtDateMainGearEnchant; }
		set { m_dtDateMainGearEnchant = value; }
	}

	public int MainGearEnchantDailyCount
	{
		get { return m_nMainGearEnchantDailyCount; }
		set { m_nMainGearEnchantDailyCount = value; }
	}

	public DateTime MainGearRefineDate
	{
		get { return m_dtDateMainGearRefine; }
		set { m_dtDateMainGearRefine = value; }
	}

	public int MainGearRefineDailyCount
	{
		get { return m_nMainGearRefineDailyCount; }
		set { m_nMainGearRefineDailyCount = value; }
	}

	public long Gold
	{
		get { return m_lGold; }
		set { m_lGold = value; }
	}

	public int OwnDia
	{
		get { return m_nOwnDia; }
		set { m_nOwnDia = value; }
	}

	public int UnOwnDia
	{
		get { return m_nUnOwnDia; }
		set { m_nUnOwnDia = value; }
	}

	public int Dia
	{
		get { return m_nUnOwnDia + m_nOwnDia; }
	}

	public int VipPoint
	{
		get { return m_nVipPoint; }
		set
		{
			m_nVipPoint = value;
			m_csVipLevel = CsGameData.Instance.GetVipLevelByAccVipPoint(m_nVipPoint);
		}
	}

	public CsVipLevel VipLevel
	{
		get { return m_csVipLevel; }
	}

	public List<CsMail> MailList
	{
		get { return m_listCsMail; }
	}

	// 금일무료즉시부활
	public DateTime FreeImmediateRevivalDate
	{
		get { return m_dtDateFreeImmediateRevival; }

		set { m_dtDateFreeImmediateRevival = value; }
	}

	public int FreeImmediateRevivalDailyCount
	{
		get { return m_nFreeImmediateRevivalDailyCount; }
		set { m_nFreeImmediateRevivalDailyCount = value; }
	}

	// 금일유료즉시부활
	public DateTime PaidImmediateRevivalDate
	{
		get { return m_dtDatePaidImmediateRevival; }
		set { m_dtDatePaidImmediateRevival = value; }
	}

	public int PaidImmediateRevivalDailyCount
	{
		get { return m_nPaidImmediateRevivalDailyCount; }
		set { m_nPaidImmediateRevivalDailyCount = value; }
	}

	// 금일경험치물약사용날짜
	public DateTime ExpPotionUseCountDate
	{
		get { return m_dtDateExpPotionUseCount; }
		set { m_dtDateExpPotionUseCount = value; }
	}

	// 금일경험치물약사용횟수
	public int ExpPotionDailyUseCount
	{
		get { return m_nExpPotionDailyUseCount; }
		set { m_nExpPotionDailyUseCount = value; }
	}

	// 받은 일일접속시간보상 시간 
	public DateTime ReceivedDailyAccessRewardDate
	{
		get { return m_dtDateDailyAccessReward; }
		set { m_dtDateDailyAccessReward = value; }
	}

	// 받은 일일접속시간보상 목록
	public List<int> ReceivedDailyAccessRewardList
	{
		get { return m_listReceivedDailyAccessRewardList; }
	}

	// 받은 레벨업보상 목록
	public List<int> ReceivedLevelUpRewardList
	{
		get { return m_listReceivedLevelUpRewardList; }
	}

	// 일일접속시간
	public float DailyAccessTime
	{
		get { return m_flDailyAccessTime + Time.realtimeSinceStartup - m_flStartRealtimeSinceStartup; }
		set { m_flDailyAccessTime = value + m_flStartRealtimeSinceStartup; }
	}

	// 받은 출석보상날짜, 없을 경우 DateTime.MinValue.Date
	public DateTime ReceivedAttendRewardDate
	{
		get { return m_dtDateReceivedAttendReward; }
		set { m_dtDateReceivedAttendReward = value; }
	}

	// 받은 출석보상일차, 없을 경우 0
	public int ReceivedAttendRewardDay
	{
		get { return m_nReceivedAttendRewardDay; }
		set { m_nReceivedAttendRewardDay = value; }
	}

	// 휴식보상
	public int RestTime
	{
		get { return m_nRestTime; }
		set { m_nRestTime = value; }
	}

	public CsParty Party
	{
		get { return m_csParty; }
		set { m_csParty = value; }
	}

	public List<CsPartyApplication> PartyApplicationList
	{
		get { return m_listCsPartyApplication; }
	}

	public List<CsPartyInvitation> PartyInvitationList
	{
		get { return m_listCsPartyInvitation; }
	}

	// 탈것
	public List<CsHeroMount> HeroMountList
	{
		get { return m_listCsHeroMount; }
	}

	public int EquippedMountId
	{
		get { return m_nEquippedMountId; }
		set { m_nEquippedMountId = value; }
	}

	public bool IsRiding
	{
		get { return m_bIsRiding; }
		set { m_bIsRiding = value; }
	}

	// 탈것장비
	public List<CsHeroMountGear> HeroMountGearList
	{
		get { return m_listCsHeroMountGear; }
	}

	public List<Guid> EquippedMountGearList
	{
		get { return m_listEquippedMountGear; }
	}

	// 탈것장비 일일재강화횟수
	public DateTime MountGearRefinementDate
	{
		get { return m_dtDateMountGearRefinement; }
		set { m_dtDateMountGearRefinement = value; }
	}

	public int MountGearRefinementDailyCount
	{
		get { return m_nMountGearRefinementDailyCount; }
		set { m_nMountGearRefinementDailyCount = value; }
	}

	// 스테미나
	public int Stamina
	{
		get { return m_nStamina; }
	}

	public float StaminaAutoRecoveryRemainingTime
	{
		get { return m_flStaminaAutoRecoveryRemainingTime - Time.realtimeSinceStartup; }
	}

	public List<CsHeroWing> HeroWingList
	{
		get { return m_listCsHeroWing; }
	}

	public int EquippedWingId
	{
		get { return m_nEquippedWingId; }
		set { m_nEquippedWingId = value; }
	}

	public int WingStep
	{
		get { return m_nWingStep; }
		set { m_nWingStep = value; }
	}

	public int WingLevel
	{
		get { return m_nWingLevel; }
		set { m_nWingLevel = value; }
	}

	public int WingExp
	{
		get { return m_nWingExp; }
		set { m_nWingExp = value; }
	}

	public List<CsHeroWingPart> HeroWingPartList
	{
		get { return m_listCsHeroWingPart; }
	}

	public DateTime FreeSweepCountDate
	{
		get { return m_dtDateFreeSweepCount; }
		set { m_dtDateFreeSweepCount = value; }
	}

	public int FreeSweepDailyCount
	{
		get { return m_nFreeSweepDailyCount; }
		set { m_nFreeSweepDailyCount = value; }
	}

	public bool IsCreateEnter
	{
		get { return m_bIsCreateEnter; }
		set { m_bIsCreateEnter = value; }
	}

	public int MainGearEnchantLevelSetNo
	{
		get { return m_nMainGearEnchantLevelSetNo; }
		set { m_nMainGearEnchantLevelSetNo = value; }
	}

	public int SubGearSoulstoneLevelSetNo
	{
		get { return m_nSubGearSoulstoneLevelSetNo; }
		set { m_nSubGearSoulstoneLevelSetNo = value; }
	}

	public DateTime ExpScrollUseCountDate
	{
		get { return m_dtDateExpScrollUseCount; }
		set { m_dtDateExpScrollUseCount = value; }
	}

	public int ExpScrollDailyUseCount
	{
		get { return m_nExpScrollDailyUseCount; }
		set { m_nExpScrollDailyUseCount = value; }
	}

	public int ExpScrollItemId
	{
		get { return m_nExpScrollItemId; }
		set { m_nExpScrollItemId = value; }
	}

	public float ExpScrollRemainingTime
	{
		get { return m_flExpScrollRemainingTime; }
		set { m_flExpScrollRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public EnMyHeroEnterType MyHeroEnterType
	{
		get { return m_enContinentEnterType; }
		set { m_enContinentEnterType = value; }
	}

	// 공적포인트
	public int ExploitPoint
	{
		get { return m_nExploitPoint; }
		set { m_nExploitPoint = value; }
	}

	public DateTime ExploitPointDate
	{
		get { return m_dtDateExploitPoint; }
		set { m_dtDateExploitPoint = value; }
	}

	public int DailyExploitPoint
	{
		get { return m_nDailyExploitPoint; }
		set { m_nDailyExploitPoint = value; }
	}

	// 진행중인 연속미션 목록
	public List<CsHeroSeriesMission> HeroSeriesMissionList
	{
		get { return m_listCsHeroSeriesMission; }
	}

	// 진행중인 오늘의미션 목록
	public List<CsHeroTodayMission> HeroTodayMissionList
	{
		get { return m_listCsHeroTodayMission; }
	}

	//오늘의할일 목록
	public List<CsHeroTodayTask> HeroTodayTaskList
	{
		get { return m_listCsHeroTodayTask; }
	}

	//금일 달성포인트
	public int AchievementDailyPoint
	{
		get { return m_nAchievementDailyPoint; }
		set { m_nAchievementDailyPoint = value; }
	}

	public DateTime AchievementPointDate
	{
		get { return m_dtDateAchievementPoint; }
		set { m_dtDateAchievementPoint = value; }
	}

	//금일 받은 달성보상번호
	public int ReceivedAchievementRewardNo
	{
		get { return m_nReceivedAchievementRewardNo; }
		set { m_nReceivedAchievementRewardNo = value; }
	}

	public DateTime ReceivedAchievementRewardDate
	{
		get { return m_dtDateReceivedAchievementReward; }
		set { m_dtDateReceivedAchievementReward = value; }
	}

	// 보유 명예포인트
	public int HonorPoint
	{
		get { return m_nHonorPoint; }
		set { m_nHonorPoint = value; }
	}


	public DateTime RankRewardReceivedDate    // 계급보상받은날짜
	{
		get { return m_dtRankRewardReceivedDate; }
		set { m_dtRankRewardReceivedDate = value; }
	}

	public int RankRewardReceivedRankNo        // 계급보상받은계급번호
	{
		get { return m_nRankRewardReceivedRankNo; }
		set { m_nRankRewardReceivedRankNo = value; }
	}

	public List<int> ReceivedVipLevelRewardList
	{
		get { return m_listReceivedVipLevelReward; }
	}

	public int DailyServerLevelRakingNo
	{
		get { return m_nDailyServerLevelRakingNo; }
		set { m_nDailyServerLevelRakingNo = value; }
	}

	public int DailyServerLevelRanking
	{
		get { return m_nDailyServerLevelRanking; }
		set { m_nDailyServerLevelRanking = value; }
	}

	public int RewardedDailyServerLevelRankingNo
	{
		get { return m_nRewardedDailyServerLevelRankingNo; }
		set { m_nRewardedDailyServerLevelRankingNo = value; }
	}

	// 보상받은 도달항목번호. 없을 경우 0
	public int RewardedAttainmentEntryNo
	{
		get { return m_nRewardedAttainmentEntryNo; }
		set { m_nRewardedAttainmentEntryNo = value; }
	}

	// 금일 왜곡주문서사용횟수
	public int DistortionScrollDailyUseCount
	{
		get { return m_nDistortionScrollDailyUseCount; }
		set { m_nDistortionScrollDailyUseCount = value; }
	}

	public DateTime DistortionScrollUseCountDate
	{
		get { return m_dtDateDistortionScrollUseCount; }
		set { m_dtDateDistortionScrollUseCount = value; }
	}

	// 왜곡남은시간(단위:초)
	public float RemainingDistortionTime
	{
		get { return m_flRemainingDistortionTime; }
		set { m_flRemainingDistortionTime = value + Time.realtimeSinceStartup; }
	}

	public PDCartInstance CartInstance
	{
		get { return m_cartInstance; }
		set { m_cartInstance = value; }
	}

	public Dictionary<int, long> DicAttrValue
	{
		get { return m_dicAttrValue; }
	}

	public long NationFund
	{
		get { return m_csNationInstance.Fund; }
		set { m_csNationInstance.Fund = value; }
	}

	public List<CsNationNoblesseInstance> NationNoblesseInstanceList
	{
		get { return m_csNationInstance.NationNoblesseInstanceList; }
	}

	public int DailyNationDonationCount
	{
		get { return m_nDailyNationDonationCount; }
		set { m_nDailyNationDonationCount = value; }
	}

	public DateTime NationDonationCountDate
	{
		get { return m_dtNationDonationCountDate; }
		set { m_dtNationDonationCountDate = value; }
	}

	public int ExplorationPoint
	{
		get { return m_nExplorationPoint; }
		set { m_nExplorationPoint = value; }
	}

	public int SoulPowder
	{
		get { return m_nSoulPowder; }
		set { m_nSoulPowder = value; }
	}

	public int DailyStaminaBuyCount
	{
		get { return m_nDailyStaminaBuyCount; }
		set { m_nDailyStaminaBuyCount = value; }
	}

	public DateTime StaminaBuyCountDate
	{
		get { return m_dtStaminaBuyCountDate; }
		set { m_dtStaminaBuyCountDate = value; }
	}

	public int LootingItemMinGrade
	{
		get { return m_nLootingItemMinGrade; }
		set { m_nLootingItemMinGrade = value; }
	}

	public bool TodayMissionTutorialStarted
	{
		get { return m_bTodayMissionTutorialStarted; }
		set { m_bTodayMissionTutorialStarted = value; }
	}

	public int ServerMaxLevel
	{
		get { return m_nServerMaxLevel; }
		set { m_nServerMaxLevel = value; }
	}

	public int CustomPresetHair
	{
		get { return m_nCustomPresetHair; }
		set { m_nCustomPresetHair = value; }
	}

	public int CustomFaceJawHeight
	{
		get { return m_nCustomFaceJawHeight; }
		set { m_nCustomFaceJawHeight = value; }
	}

	public int CustomFaceJawWidth
	{
		get { return m_nCustomFaceJawWidth; }
		set { m_nCustomFaceJawWidth = value; }
	}

	public int CustomFaceJawEndHeight
	{
		get { return m_nCustomFaceJawEndHeight; }
		set { m_nCustomFaceJawEndHeight = value; }
	}

	public int CustomFaceWidth
	{
		get { return m_nCustomFaceWidth; }
		set { m_nCustomFaceWidth = value; }
	}

	public int CustomFaceEyebrowHeight
	{
		get { return m_nCustomFaceEyebrowHeight; }
		set { m_nCustomFaceEyebrowHeight = value; }
	}

	public int CustomFaceEyebrowRotation
	{
		get { return m_nCustomFaceEyebrowRotation; }
		set { m_nCustomFaceEyebrowRotation = value; }
	}

	public int CustomFaceEyesWidth
	{
		get { return m_nCustomFaceEyesWidth; }
		set { m_nCustomFaceEyesWidth = value; }
	}

	public int CustomFaceNoseHeight
	{
		get { return m_nCustomFaceNoseHeight; }
		set { m_nCustomFaceNoseHeight = value; }
	}

	public int CustomFaceNoseWidth
	{
		get { return m_nCustomFaceNoseWidth; }
		set { m_nCustomFaceNoseWidth = value; }
	}

	public int CustomFaceMouthHeight
	{
		get { return m_nCustomFaceMouthHeight; }
		set { m_nCustomFaceMouthHeight = value; }
	}

	public int CustomFaceMouthWidth
	{
		get { return m_nCustomFaceMouthWidth; }
		set { m_nCustomFaceMouthWidth = value; }
	}

	public int CustomBodyHeadSize
	{
		get { return m_nCustomBodyHeadSize; }
		set { m_nCustomBodyHeadSize = value; }
	}

	public int CustomBodyArmsLength
	{
		get { return m_nCustomBodyArmsLength; }
		set { m_nCustomBodyArmsLength = value; }
	}

	public int CustomBodyArmsWidth
	{
		get { return m_nCustomBodyArmsWidth; }
		set { m_nCustomBodyArmsWidth = value; }
	}

	public int CustomBodyChestSize
	{
		get { return m_nCustomBodyChestSize; }
		set { m_nCustomBodyChestSize = value; }
	}

	public int CustomBodyWaistWidth
	{
		get { return m_nCustomBodyWaistWidth; }
		set { m_nCustomBodyWaistWidth = value; }
	}

	public int CustomBodyHipsSize
	{
		get { return m_nCustomBodyHipsSize; }
		set { m_nCustomBodyHipsSize = value; }
	}

	public int CustomBodyPelvisWidth
	{
		get { return m_nCustomBodyPelvisWidth; }
		set { m_nCustomBodyPelvisWidth = value; }
	}

	public int CustomBodyLegsLength
	{
		get { return m_nCustomBodyLegsLength; }
		set { m_nCustomBodyLegsLength = value; }
	}

	public int CustomBodyLegsWidth
	{
		get { return m_nCustomBodyLegsWidth; }
		set { m_nCustomBodyLegsWidth = value; }
	}

	public int CustomColorSkin
	{
		get { return m_nCustomColorSkin; }
		set { m_nCustomColorSkin = value; }
	}

	public int CustomColorEyes
	{
		get { return m_nCustomColorEyes; }
		set { m_nCustomColorEyes = value; }
	}

	public int CustomColorBeardAndEyebrow
	{
		get { return m_nCustomColorBeardAndEyebrow; }
		set { m_nCustomColorBeardAndEyebrow = value; }
	}

	public int CustomColorHair
	{
		get { return m_nCustomColorHair; }
		set { m_nCustomColorHair = value; }
	}

	public int SpiritStone
	{
		get { return m_nSpiritStone; }
		set { m_nSpiritStone = value; }
	}

	public int SelectedRankActiveSkillId
	{
		get { return m_nSelectedRankActiveSkillId; }
		set { m_nSelectedRankActiveSkillId = value; }
	}

	public float RankActiveSkillRemainingCoolTime
	{
		get { return m_flRankActiveSkillRemainingCoolTime - Time.realtimeSinceStartup; }
		set { m_flRankActiveSkillRemainingCoolTime = value + Time.realtimeSinceStartup; }
	}

	public List<CsHeroRankActiveSkill> HeroRankActiveSkillList
	{
		get { return m_listCsHeroRankActiveSkill; }
	}

	public List<CsHeroRankPassiveSkill> HeroRankPassiveSkillList
	{
		get { return m_listCsHeroRankPassiveSkill; }
	}

	public int RookieGiftNo
	{
		get { return m_nRookieGiftNo; }
		set { m_nRookieGiftNo = value; }
	}

	public float RookieGiftRemainingTime
	{
		get { return m_flRookieGiftRemainingTime - Time.realtimeSinceStartup; }
		set { m_flRookieGiftRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public List<int> ReceivedOpenGiftRewardList
	{
		get { return m_listReceivedOpenGiftReward; }
	}

	public DateTime RegDate
	{
		get { return m_dtRegDate; }
	}

	public List<int> RewardedOpen7DayEventMissionList
	{
		get { return m_listRewardedOpen7DayEventMission; }
	}

	public List<int> PurchasedOpen7DayEventProductList
	{
		get { return m_listPurchasedOpen7DayEventProduct; }
	}

	public List<CsHeroOpen7DayEventProgressCount> HeroOpen7DayEventProgressCountList
	{
		get { return m_listCsHeroOpen7DayEventProgressCount; }
	}

	public List<CsHeroRetrievalProgressCount> HeroRetrievalProgressCountList
	{
		get { return m_listCsHeroRetrievalProgressCount; }
	}

	public List<CsHeroRetrieval> HeroRetrievalList
	{
		get { return m_listCsHeroRetrieval; }
	}

	public List<CsHeroTaskConsignment> HeroTaskConsignmentList
	{
		get { return m_listCsHeroTaskConsignment; }
	}

	public List<CsHeroTaskConsignmentStartCount> HeroTaskConsignmentStartCountList
	{
		get { return m_listCsHeroTaskConsignmentStartCount; }
	}

	public List<int> RewardedLimitationGiftScheduleIdList
	{
		get { return m_listRewardedLimitationGiftScheduleId; }
	}

	public CsHeroWeekendReward HeroWeekendReward
	{
		get { return m_csHeroWeekendReward; }
	}

	public List<CsHeroDiaShopProductBuyCount> HeroDiaShopProductBuyCountList
	{
		get { return m_listCsHeroDiaShopProductBuyCount; }
	}

	public List<CsHeroDiaShopProductBuyCount> TotalHeroDiaShopProductBuyCountList
	{
		get { return m_listCsHeroDiaShopProductBuyCountTotal; }
	}

	// 공포의 제단 성물
	public int WeeklyFearAltarHalidomCollectionRewardNo
	{
		get { return m_nWeeklyFearAltarHalidomCollectionRewardNo; }
		set { m_nWeeklyFearAltarHalidomCollectionRewardNo = value; }
	}

	public List<int> WeeklyFearAltarHalidomList
	{
		get { return m_listWeeklyFearAltarHalidom; }
	}

	public List<int> WeeklyRewardReceivedFearAltarHalidomElementalList
	{
		get { return m_listWeeklyRewardReceivedFearAltarHalidomElemental; }
	}

	public bool Open7DayEventRewarded
	{
		get { return m_bOpen7DayEventRewarded; }
		set { m_bOpen7DayEventRewarded = value; }
	}

	public List<CsNationPowerRanking> NationPowerRankingList
	{
		get { return m_listCsNationPowerRanking; }
	}

	public List<CsHeroPotionAttr> HeroPotionAttrList
	{
		get { return m_listCsHeroPotionAttr; }
	}

	public List<CsNationInstance> NationInstanceList
	{
		get { return m_listCsNationInstance; }
	}

	public CsNationInstance NationInstance
	{
		get { return m_csNationInstance; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMyHeroInfo(Guid guidHeroId, string strName, int nNationId, int nJobId, int nLevel,
						long lExp, int nMaxHP, int nHp, DateTime dtDate, int nSpiritStone,
						int nPaidInventorySlotCount, int nInitEntranceLocationId, int nInitEntranceLocationParam, int nLak, int nMainGearEnchantDailyCount,
						int nMainGearRefineDailyCount, int nOwnDia, int nUnOwnDia, long lGold, int nFreeImmediateRevivalDailyCount,
						int nPaidImmediateRevivalDailyCount, int nRestTime, PDParty party, int nVipPoint, int nExpPotionDailyUseCount,
						int[] receivedLevelUpRewards, int[] receivedDailyAcessRewards, float flDailyAccessTime, DateTime dtReceivedAttendRewardDate, int nReceivedAttendRewardDay,
						int nEquippedMountId, int nMountGearRefinementDailyCount, PDHeroWing[] heroWings, int nEquippedWingId, int nWingStep,
						int nWingLevel, int nWingExp, PDHeroWingPart[] wingParts, int nFreeSweepDailyCount, PDStoryDungeonPlay[] storyDungeonPlays,
						PDStoryDungeonClear[] storyDungeonClears, int nStamina, float flStaminaAutoRecoveryRemainingTime, bool bIsRiding, int nExpDungeonDailyPlayCount,
						int[] expDungeonClearedDifficulties, int nMainGearEnchantLevelSetNo, int nSubGearSoulstoneLevelSetNo, int nExpScrollDailyUseCount, int nExpScrollItemId,
						float flExpScrollRemainingTime, int nGoldDungeonDailyPlayCount, int[] goldDungeonClearedDifficulties, float flUndergroundMazeDailyPlayTime, int nArtifactRoomBestFloor,
						int nArtifactRoomCurrentFloor, int nArtifactRoomDailyInitCount, int nArtifactRoomSweepProgressFloor, float flArtifactRoomSweepRemainingTime, DateTimeOffset dtOfsTime,
						int nExploitPoint, int nDailyExploitPoint, PDHeroSeriesMission[] seriesMissions, PDHeroTodayMission[] todayMissions, PDHeroTodayTask[] todayTasks,
						int nAchievementDailyPoint, int nReceivedAchievementRewardNo, int nHonorPoint, int[] receivedVipLevelRewards, int nRankNo,
						DateTime dtRankRewardReceivedDate, int nRankRewardReceivedRankNo, int nRewardedAttainmentEntryNo, int nDistortionScrollDailyUseCount, float flRemainingDistortionTime,
						PDCartInstance cartInstance, int nDailyServerLevelRakingNo, int nDailyServerLevelRanking, int nRewardedDailyServerLevelRankingNo, int nPreviousContinentId,
						int nPreviousNationId, PDNationInstance[] nationInsts, int nDailyNationDonationCount, DateTime dtServerOpenDate,
						int nExplorationPoint, int nSoulPowder, int nDailyStaminaBuyCount, int nLootingItemMinGrade, int nCustomPresetHair,
						int nCustomFaceJawHeight, int nCustomFaceJawWidth, int nCustomFaceJawEndHeight, int nCustomFaceWidth, int nCustomFaceEyebrowHeight,
						int nCustomFaceEyebrowRotation, int nCustomFaceEyesWidth, int nCustomFaceNoseHeight, int nCustomFaceNoseWidth, int nCustomFaceMouthHeight,
						int nCustomFaceMouthWidth, int nCustomBodyHeadSize, int nCustomBodyArmsLength, int nCustomBodyArmsWidth, int nCustomBodyChestSize,
						int nCustomBodyWaistWidth, int nCustomBodyHipsSize, int nCustomBodyPelvisWidth, int nCustomBodyLegsLength, int nCustomBodyLegsWidth,
						int nCustomColorSkin, int nCustomColorEyes, int nCustomColorBeardAndEyebrow, int nCustomColorHair, bool bTodayMissionTutorialStarted,
						int nServerMaxLevel, PDHeroNpcShopProduct[] heroNpcShopProducts, PDHeroRankActiveSkill[] rankActiveSkills, PDHeroRankPassiveSkill[] rankPassiveSkills, int nSelectedRankActiveSkillId,
						float flRankActiveSkillRemainingCoolTime, int nRookieGiftNo, float flRookieGiftRemainingTime, int[] receivedOpenGiftRewards, DateTime dtRegDate,
						int[] rewardedOpen7DayEventMissions, int[] purchasedOpen7DayEventProducts, PDHeroOpen7DayEventProgressCount[] open7DayEventProgressCounts, PDHeroRetrievalProgressCount[] retrievalProgressCounts, PDHeroRetrieval[] retrievals,
						PDHeroTaskConsignment[] taskConsignments, PDHeroTaskConsignmentStartCount[] taskConsignmentStartCounts, int[] rewardedLimitationGiftScheduleIds, PDHeroWeekendReward weekendReward, int nPaidWarehouseSlotCount,
						PDHeroDiaShopProductBuyCount[] dailyDiaShopProductBuyCounts, PDHeroDiaShopProductBuyCount[] totalDiaShopProductBuyCounts, int nWeeklyFearAltarHalidomCollectionRewardNo, int[] weeklyFearAltarHalidoms, int[] weeklyRewardReceivedFearAltarHalidomElementals,
						bool bOpen7DayEventRewarded, PDNationPowerRanking[] nationPowerRankings, PDHeroPotionAttr[] potionAttrs)
		: base(guidHeroId, strName, nNationId, nJobId, nLevel, nHp, nMaxHP, nRankNo)
	{

		m_dtOfsTime = dtOfsTime;
		m_dateTimeBase = m_dtOfsTime.DateTime;
		m_flStartRealtimeSinceStartup = Time.realtimeSinceStartup;

		m_lExp = lExp;

		m_nLak = nLak;

		m_nSpiritStone = nSpiritStone;

		m_dtLoginDate = dtDate;
		m_dtDateMainGearEnchant = dtDate;
		m_dtDateMainGearRefine = dtDate;

		m_nPaidInventorySlotCount = nPaidInventorySlotCount;
		m_nInitEntranceLocationId = m_nLocationId = nInitEntranceLocationId;
		m_nInitEntranceLocationParam = nInitEntranceLocationParam;

		m_nMainGearEnchantDailyCount = nMainGearEnchantDailyCount;
		m_nMainGearRefineDailyCount = nMainGearRefineDailyCount;

		m_nOwnDia = nOwnDia;
		m_nUnOwnDia = nUnOwnDia;
		m_lGold = lGold;
		VipPoint = nVipPoint;

		// 받은 vip레벨보상 목록(값 = vipLevel)
		m_listReceivedVipLevelReward = new List<int>(receivedVipLevelRewards);

		// 금일무료즉시부활
		m_dtDateFreeImmediateRevival = dtDate;
		m_nFreeImmediateRevivalDailyCount = nFreeImmediateRevivalDailyCount;

		// 금일유료즉시부활
		m_dtDatePaidImmediateRevival = dtDate;
		m_nPaidImmediateRevivalDailyCount = nPaidImmediateRevivalDailyCount;

		// 금일경험치물약사용횟수
		m_dtDateExpPotionUseCount = dtDate;
		m_nExpPotionDailyUseCount = nExpPotionDailyUseCount;

		// 받은 레벨업보상 목록
		m_listReceivedLevelUpRewardList = new List<int>(receivedLevelUpRewards);

		// 받은 일일접속시간보상 목록
		m_dtDateDailyAccessReward = dtDate;
		m_listReceivedDailyAccessRewardList = new List<int>(receivedDailyAcessRewards);

		// 일일접속시간
		m_flDailyAccessTime = flDailyAccessTime + m_flStartRealtimeSinceStartup;

		// 받은 출석보상날짜, 없을 경우 DateTime.MinValue.Date
		m_dtDateReceivedAttendReward = dtReceivedAttendRewardDate;
		m_nReceivedAttendRewardDay = nReceivedAttendRewardDay;

		// 휴식시간(분)
		if (nRestTime > CsGameData.Instance.RestRewardTimeList[CsGameData.Instance.RestRewardTimeList.Count - 1].RestTime)
			m_nRestTime = CsGameData.Instance.RestRewardTimeList[CsGameData.Instance.RestRewardTimeList.Count - 1].RestTime;
		else
			m_nRestTime = nRestTime;

		if (party != null)
			m_csParty = new CsParty(party);

		// 탈것
		m_listCsHeroMount = new List<CsHeroMount>();
		m_nEquippedMountId = nEquippedMountId;
		m_bIsRiding = bIsRiding;

		// 탈것장비
		m_listCsHeroMountGear = new List<CsHeroMountGear>();
		m_listEquippedMountGear = new List<Guid>();

		//탈것장비 일일재강화횟수
		m_dtDateMountGearRefinement = dtDate;
		m_nMountGearRefinementDailyCount = nMountGearRefinementDailyCount;

		// 스테미나
		m_nStamina = nStamina;
		m_flStaminaAutoRecoveryRemainingTime = flStaminaAutoRecoveryRemainingTime + Time.realtimeSinceStartup;

		m_listCsHeroWing = new List<CsHeroWing>();

		for (int i = 0; i < heroWings.Length; i++)
		{
			m_listCsHeroWing.Add(new CsHeroWing(heroWings[i]));
		}

		m_nEquippedWingId = nEquippedWingId;

		m_nWingStep = nWingStep;
		m_nWingLevel = nWingLevel;
		m_nWingExp = nWingExp;

		m_listCsHeroWingPart = new List<CsHeroWingPart>();

		for (int i = 0; i < wingParts.Length; i++)
		{
			m_listCsHeroWingPart.Add(new CsHeroWingPart(wingParts[i]));
		}

		m_dtDateFreeSweepCount = dtDate;
		m_nFreeSweepDailyCount = nFreeSweepDailyCount;

		// 스토리던전
		for (int i = 0; i < CsGameData.Instance.StoryDungeonList.Count; i++)
		{
			CsGameData.Instance.StoryDungeonList[i].Reset(dtDate);
		}

		for (int i = 0; i < storyDungeonPlays.Length; i++)
		{
			CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(storyDungeonPlays[i].dungeonNo);
			csStoryDungeon.PlayDate = dtDate;
			csStoryDungeon.PlayCount = storyDungeonPlays[i].count;
		}

		for (int i = 0; i < storyDungeonClears.Length; i++)
		{
			CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(storyDungeonClears[i].dungeonNo);
			csStoryDungeon.ClearMaxDifficulty = storyDungeonClears[i].clearMaxDifficulty;
		}

		// 경험치던전
		CsGameData.Instance.ExpDungeon.ExpDungeonPlayCountDate = dtDate;
		CsGameData.Instance.ExpDungeon.ExpDungeonDailyPlayCount = nExpDungeonDailyPlayCount;
		CsGameData.Instance.ExpDungeon.ExpDungeonClearedDifficultyList = new List<int>(expDungeonClearedDifficulties);

		m_nMainGearEnchantLevelSetNo = nMainGearEnchantLevelSetNo;
		m_nSubGearSoulstoneLevelSetNo = nSubGearSoulstoneLevelSetNo;

		m_dtDateExpScrollUseCount = dtDate;
		m_nExpScrollDailyUseCount = nExpScrollDailyUseCount;
		m_nExpScrollItemId = nExpScrollItemId;
		m_flExpScrollRemainingTime = flExpScrollRemainingTime + Time.realtimeSinceStartup;

		// 골드던전
		CsGameData.Instance.GoldDungeon.GoldDungeonPlayCountDate = dtDate;
		CsGameData.Instance.GoldDungeon.GoldDungeonDailyPlayCount = nGoldDungeonDailyPlayCount;
		CsGameData.Instance.GoldDungeon.GoldDungeonClearedDifficultyList = new List<int>(goldDungeonClearedDifficulties);

		// 지하미로
		CsGameData.Instance.UndergroundMaze.UndergroundMazePlayTimeDate = dtDate;
		CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime = flUndergroundMazeDailyPlayTime;

		// 고대유물의방
		CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor = nArtifactRoomBestFloor;
		CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor = nArtifactRoomCurrentFloor;
		CsGameData.Instance.ArtifactRoom.ArtifactRoomInitCountDate = dtDate;
		CsGameData.Instance.ArtifactRoom.ArtifactRoomDailyInitCount = nArtifactRoomDailyInitCount;
		CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepProgressFloor = nArtifactRoomSweepProgressFloor;
		CsGameData.Instance.ArtifactRoom.ArtifactRoomSweepRemainingTime = flArtifactRoomSweepRemainingTime;

		// 공적포인트
		m_nExploitPoint = nExploitPoint;
		m_dtDateExploitPoint = dtDate;
		m_nDailyExploitPoint = nDailyExploitPoint;

		// 진행중인 연속미션 목록
		m_listCsHeroSeriesMission = new List<CsHeroSeriesMission>();

		if (seriesMissions != null)
		{
			for (int i = 0; i < seriesMissions.Length; i++)
			{
				m_listCsHeroSeriesMission.Add(new CsHeroSeriesMission(seriesMissions[i]));
			}
		}

		// 진행중인 오늘의미션 목록
		m_listCsHeroTodayMission = new List<CsHeroTodayMission>();

		AddHeroTodayMissions(todayMissions, dtDate);

		//오늘의할일 목록
		m_listCsHeroTodayTask = new List<CsHeroTodayTask>();

		for (int i = 0; i < todayTasks.Length; i++)
		{
			m_listCsHeroTodayTask.Add(new CsHeroTodayTask(todayTasks[i], dtDate));
		}

		//금일 달성포인트
		m_nAchievementDailyPoint = nAchievementDailyPoint;
		m_dtDateAchievementPoint = dtDate;

		//금일 받은 달성보상번호
		m_nReceivedAchievementRewardNo = nReceivedAchievementRewardNo;
		m_dtDateReceivedAchievementReward = dtDate;

		// 보유 명예포인트
		m_nHonorPoint = nHonorPoint;

		m_dtRankRewardReceivedDate = dtRankRewardReceivedDate;          // 계급보상받은날짜
		m_nRankRewardReceivedRankNo = nRankRewardReceivedRankNo;        // 계급보상받은계급번호

		m_nDailyServerLevelRakingNo = nDailyServerLevelRakingNo;
		m_nDailyServerLevelRanking = nDailyServerLevelRanking;
		m_nRewardedDailyServerLevelRankingNo = nRewardedDailyServerLevelRankingNo;

		// 보상받은 도달항목번호. 없을 경우 0
		m_nRewardedAttainmentEntryNo = nRewardedAttainmentEntryNo;

		// 금일 왜곡주문서사용횟수
		m_nDistortionScrollDailyUseCount = nDistortionScrollDailyUseCount;
		m_dtDateDistortionScrollUseCount = dtDate;
		// 왜곡남은시간(단위:초)
		m_flRemainingDistortionTime = flRemainingDistortionTime + Time.realtimeSinceStartup;

		m_cartInstance = cartInstance;

		m_nPreviousContinentId = nPreviousContinentId;
		m_nPreviousNationId = nPreviousNationId;

		m_listCsNationInstance = new List<CsNationInstance>();

		for (int i = 0; i < nationInsts.Length; i++)
		{
			CsNationInstance csNationInstance = new CsNationInstance(nationInsts[i]);
			m_listCsNationInstance.Add(csNationInstance);

			if (nationInsts[i].nationId == Nation.NationId)
			{
				m_csNationInstance = csNationInstance;

				m_lNationFund = m_csNationInstance.Fund;
				m_listCsNationNoblesseInstance = m_csNationInstance.NationNoblesseInstanceList;
			}
		}

		m_nDailyNationDonationCount = nDailyNationDonationCount;
		m_dtNationDonationCountDate = dtDate;

		m_dtServerOpenDate = dtServerOpenDate;

		m_nExplorationPoint = nExplorationPoint;

		m_nSoulPowder = nSoulPowder;

		m_nDailyStaminaBuyCount = nDailyStaminaBuyCount;
		m_dtStaminaBuyCountDate = dtDate;

		m_nLootingItemMinGrade = nLootingItemMinGrade;

		m_bTodayMissionTutorialStarted = bTodayMissionTutorialStarted;

		m_nServerMaxLevel = nServerMaxLevel;

		// Customizing
		m_nCustomPresetHair = nCustomPresetHair;

		m_nCustomFaceJawHeight = nCustomFaceJawHeight;
		m_nCustomFaceJawWidth = nCustomFaceJawWidth;
		m_nCustomFaceJawEndHeight = nCustomFaceJawEndHeight;
		m_nCustomFaceWidth = nCustomFaceWidth;
		m_nCustomFaceEyebrowHeight = nCustomFaceEyebrowHeight;
		m_nCustomFaceEyebrowRotation = nCustomFaceEyebrowRotation;
		m_nCustomFaceEyesWidth = nCustomFaceEyesWidth;
		m_nCustomFaceNoseHeight = nCustomFaceNoseHeight;
		m_nCustomFaceNoseWidth = nCustomFaceNoseWidth;
		m_nCustomFaceMouthHeight = nCustomFaceMouthHeight;
		m_nCustomFaceMouthWidth = nCustomFaceMouthWidth;

		m_nCustomBodyHeadSize = nCustomBodyHeadSize;
		m_nCustomBodyArmsLength = nCustomBodyArmsLength;
		m_nCustomBodyArmsWidth = nCustomBodyArmsWidth;
		m_nCustomBodyChestSize = nCustomBodyChestSize;
		m_nCustomBodyWaistWidth = nCustomBodyWaistWidth;
		m_nCustomBodyHipsSize = nCustomBodyHipsSize;
		m_nCustomBodyPelvisWidth = nCustomBodyPelvisWidth;
		m_nCustomBodyLegsLength = nCustomBodyLegsLength;
		m_nCustomBodyLegsWidth = nCustomBodyLegsWidth;

		m_nCustomColorSkin = nCustomColorSkin;
		m_nCustomColorEyes = nCustomColorEyes;
		m_nCustomColorBeardAndEyebrow = nCustomColorBeardAndEyebrow;
		m_nCustomColorHair = nCustomColorHair;

		m_listCsHeroNpcShopProduct = new List<CsHeroNpcShopProduct>();

		for (int i = 0; i < heroNpcShopProducts.Length; i++)
		{
			m_listCsHeroNpcShopProduct.Add(new CsHeroNpcShopProduct(heroNpcShopProducts[i]));
		}

		m_listCsHeroRankActiveSkill = new List<CsHeroRankActiveSkill>();

		for (int i = 0; i < rankActiveSkills.Length; i++)
		{
			m_listCsHeroRankActiveSkill.Add(new CsHeroRankActiveSkill(rankActiveSkills[i]));
		}

		m_listCsHeroRankPassiveSkill = new List<CsHeroRankPassiveSkill>();

		for (int i = 0; i < rankPassiveSkills.Length; i++)
		{
			m_listCsHeroRankPassiveSkill.Add(new CsHeroRankPassiveSkill(rankPassiveSkills[i]));
		}

		m_nSelectedRankActiveSkillId = nSelectedRankActiveSkillId;
		m_flRankActiveSkillRemainingCoolTime = flRankActiveSkillRemainingCoolTime + Time.realtimeSinceStartup;

		m_nRookieGiftNo = nRookieGiftNo;
		m_flRookieGiftRemainingTime = flRookieGiftRemainingTime + Time.realtimeSinceStartup;
		m_listReceivedOpenGiftReward = new List<int>(receivedOpenGiftRewards);
		m_dtRegDate = dtRegDate;

		m_listRewardedOpen7DayEventMission = new List<int>(rewardedOpen7DayEventMissions);
		m_listPurchasedOpen7DayEventProduct = new List<int>(purchasedOpen7DayEventProducts);

		m_listCsHeroOpen7DayEventProgressCount = new List<CsHeroOpen7DayEventProgressCount>();

		for (int i = 0; i < open7DayEventProgressCounts.Length; i++)
		{
			m_listCsHeroOpen7DayEventProgressCount.Add(new CsHeroOpen7DayEventProgressCount(open7DayEventProgressCounts[i]));
		}

		m_listCsHeroRetrievalProgressCount = new List<CsHeroRetrievalProgressCount>();

		for (int i = 0; i < retrievalProgressCounts.Length; i++)
		{
			m_listCsHeroRetrievalProgressCount.Add(new CsHeroRetrievalProgressCount(retrievalProgressCounts[i]));
		}

		m_listCsHeroRetrieval = new List<CsHeroRetrieval>();

		for (int i = 0; i < retrievals.Length; i++)
		{
			m_listCsHeroRetrieval.Add(new CsHeroRetrieval(retrievals[i]));
		}

		m_listCsHeroTaskConsignment = new List<CsHeroTaskConsignment>();

		for (int i = 0; i < taskConsignments.Length; i++)
		{
			m_listCsHeroTaskConsignment.Add(new CsHeroTaskConsignment(taskConsignments[i]));
		}

		m_listCsHeroTaskConsignmentStartCount = new List<CsHeroTaskConsignmentStartCount>();

		for (int i = 0; i < taskConsignmentStartCounts.Length; i++)
		{
			m_listCsHeroTaskConsignmentStartCount.Add(new CsHeroTaskConsignmentStartCount(taskConsignmentStartCounts[i]));
		}

		m_listRewardedLimitationGiftScheduleId = new List<int>(rewardedLimitationGiftScheduleIds);

		if (weekendReward != null)
		{
			m_csHeroWeekendReward = new CsHeroWeekendReward(weekendReward);
		}

		m_nPaidWarehouseSlotCount = nPaidWarehouseSlotCount;
		m_listCsWarehouseSlot = new List<CsWarehouseSlot>();

		m_listCsHeroDiaShopProductBuyCount = new List<CsHeroDiaShopProductBuyCount>();

		for (int i = 0; i < dailyDiaShopProductBuyCounts.Length; i++)
		{
			m_listCsHeroDiaShopProductBuyCount.Add(new CsHeroDiaShopProductBuyCount(dailyDiaShopProductBuyCounts[i]));
		}

		m_listCsHeroDiaShopProductBuyCountTotal = new List<CsHeroDiaShopProductBuyCount>();

		for (int i = 0; i < totalDiaShopProductBuyCounts.Length; i++)
		{
			m_listCsHeroDiaShopProductBuyCountTotal.Add(new CsHeroDiaShopProductBuyCount(totalDiaShopProductBuyCounts[i]));
		}

		m_nWeeklyFearAltarHalidomCollectionRewardNo = nWeeklyFearAltarHalidomCollectionRewardNo;

		m_listWeeklyFearAltarHalidom = new List<int>();

		for (int i = 0; i < weeklyFearAltarHalidoms.Length; i++)
		{
			m_listWeeklyFearAltarHalidom.Add(weeklyFearAltarHalidoms[i]);
		}

		m_listWeeklyRewardReceivedFearAltarHalidomElemental = new List<int>();

		for (int i = 0; i < weeklyRewardReceivedFearAltarHalidomElementals.Length; i++)
		{
			m_listWeeklyRewardReceivedFearAltarHalidomElemental.Add(weeklyRewardReceivedFearAltarHalidomElementals[i]);
		}

		m_bOpen7DayEventRewarded = bOpen7DayEventRewarded;

		m_listCsNationPowerRanking = new List<CsNationPowerRanking>();

		for (int i = 0; i < nationPowerRankings.Length; i++)
		{
			m_listCsNationPowerRanking.Add(new CsNationPowerRanking(nationPowerRankings[i]));
		}

		m_listCsHeroPotionAttr = new List<CsHeroPotionAttr>();

		for (int i = 0; i < potionAttrs.Length; i++)
		{
			m_listCsHeroPotionAttr.Add(new CsHeroPotionAttr(potionAttrs[i]));
		}

		m_listCsHeroMainGearEquipped = new List<CsHeroMainGear>() { null, null };
		m_listCsMail = new List<CsMail>();
		m_listCsHeroMainGear = new List<CsHeroMainGear>();
		m_listCsHeroSubGear = new List<CsHeroSubGear>();
		m_listCsInventorySlot = new List<CsInventorySlot>();
		m_listCsHeroSkill = new List<CsHeroSkill>();

		m_listCsPartyApplication = new List<CsPartyApplication>();
		m_listCsPartyInvitation = new List<CsPartyInvitation>();

		m_lBattlePower = 0;
	}

	//---------------------------------------------------------------------------------------------------
	void AddAttrValue(int nAttrId, long lValue)
	{
		if (m_dicAttrValue.ContainsKey(nAttrId))
		{
			m_dicAttrValue[nAttrId] += lValue;
		}
		else
		{
			m_dicAttrValue.Add(nAttrId, lValue);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateBattlePower(bool bIsIntro = false)
	{
		m_dicAttrValue.Clear();

		// 스킬속성.
		long lSkill = 0;

		for (int i = 0; i < m_listCsHeroSkill.Count; i++)
		{
			if (CsMainQuestManager.Instance.MainQuest == null || m_listCsHeroSkill[i].JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
			{
				lSkill += m_listCsHeroSkill[i].JobSkillLevel.BattlePower;
			}
		}

		// 강화세트
		//int nEnchantLevelSet = 0;

		CsMainGearEnchantLevelSet csMainGearEnchantLevelSet = CsGameData.Instance.GetMainGearEnchantLevelSet(m_nMainGearEnchantLevelSetNo);

		if (csMainGearEnchantLevelSet != null)
		{
			for (int i = 0; i < csMainGearEnchantLevelSet.MainGearEnchantLevelSetAttrList.Count; i++)
			{
				//nEnchantLevelSet += csMainGearEnchantLevelSet.MainGearEnchantLevelSetAttrList[i].BattlePower;
				AddAttrValue(csMainGearEnchantLevelSet.MainGearEnchantLevelSetAttrList[i].Attr.AttrId, csMainGearEnchantLevelSet.MainGearEnchantLevelSetAttrList[i].AttrValueInfo.Value);
			}
		}

		// 소울스톤보석세트
		//int nSoulstoneLevelSet = 0;

		CsSubGearSoulstoneLevelSet csSubGearSoulstoneLevelSet = CsGameData.Instance.GetSubGearSoulstoneLevelSet(m_nSubGearSoulstoneLevelSetNo);

		if (csSubGearSoulstoneLevelSet != null)
		{
			for (int i = 0; i < csSubGearSoulstoneLevelSet.SubGearSoulstoneLevelSetAttrList.Count; i++)
			{
				//nSoulstoneLevelSet += csSubGearSoulstoneLevelSet.SubGearSoulstoneLevelSetAttrList[i].BattlePower;
				AddAttrValue(csSubGearSoulstoneLevelSet.SubGearSoulstoneLevelSetAttrList[i].Attr.AttrId, csSubGearSoulstoneLevelSet.SubGearSoulstoneLevelSetAttrList[i].AttrValueInfo.Value);
			}
		}


		// 메인장비 (기본 + 추가)
		//int nMainGear = 0;

		for (int i = 0; i < m_listCsHeroMainGearEquipped.Count; i++)
		{
			if (m_listCsHeroMainGearEquipped[i] != null)
			{
				//nMainGear += m_listCsHeroMainGearEquipped[i].BattlePower;

				for (int j = 0; j < m_listCsHeroMainGearEquipped[i].BaseAttrValueList.Count; j++)
				{
					AddAttrValue(m_listCsHeroMainGearEquipped[i].BaseAttrValueList[j].Attr.AttrId, m_listCsHeroMainGearEquipped[i].BaseAttrValueList[j].Value);
				}

				for (int j = 0; j < m_listCsHeroMainGearEquipped[i].OptionAttrList.Count; j++)
				{
					AddAttrValue(m_listCsHeroMainGearEquipped[i].OptionAttrList[j].Attr.AttrId, m_listCsHeroMainGearEquipped[i].OptionAttrList[j].Value);
				}
			}
		}

		// 메인장비세트.
		//int nMainGearSet = 0;

		if (m_listCsHeroMainGearEquipped[0] != null && m_listCsHeroMainGearEquipped[1] != null)
		{
			if (m_listCsHeroMainGearEquipped[0].MainGear.MainGearTier.Tier == m_listCsHeroMainGearEquipped[1].MainGear.MainGearTier.Tier
				&& m_listCsHeroMainGearEquipped[0].MainGear.MainGearGrade.Grade == m_listCsHeroMainGearEquipped[1].MainGear.MainGearGrade.Grade
				&& m_listCsHeroMainGearEquipped[0].MainGear.MainGearQuality.Quality == m_listCsHeroMainGearEquipped[1].MainGear.MainGearQuality.Quality)
			{
				CsMainGearSet csMainGearSet = CsGameData.Instance.GetMainGearSet(m_listCsHeroMainGearEquipped[0].MainGear.MainGearTier.Tier, m_listCsHeroMainGearEquipped[0].MainGear.MainGearGrade.Grade, m_listCsHeroMainGearEquipped[0].MainGear.MainGearQuality.Quality);

				if (csMainGearSet != null)
				{
					for (int i = 0; i < csMainGearSet.MainGearSetAttrList.Count; i++)
					{
						//nMainGearSet += csMainGearSet.MainGearSetAttrList[i].BattlePower;
						AddAttrValue(csMainGearSet.MainGearSetAttrList[i].Attr.AttrId, csMainGearSet.MainGearSetAttrList[i].AttrValueInfo.Value);
					}
				}
			}
		}

		// 보조장비 (기본 + 소울스톤 + 룬속성)
		//int nSubGear = 0;

		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			if (m_listCsHeroSubGear[i].Equipped)
			{
				//nSubGear += m_listCsHeroSubGear[i].BattlePower;

				for (int j = 0; j < m_listCsHeroSubGear[i].AttrValueList.Count; j++)
				{
					AddAttrValue(m_listCsHeroSubGear[i].AttrValueList[j].Attr.AttrId, m_listCsHeroSubGear[i].AttrValueList[j].Value);
				}

				for (int j = 0; j < m_listCsHeroSubGear[i].SoulstoneSocketList.Count; j++)
				{
					CsAttr csAttr = CsGameData.Instance.GetAttr(m_listCsHeroSubGear[i].SoulstoneSocketList[j].Item.Value1);
					CsAttrValueInfo csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(m_listCsHeroSubGear[i].SoulstoneSocketList[j].Item.LongValue1);
					AddAttrValue(csAttr.AttrId, csAttrValueInfo.Value);
				}

				for (int j = 0; j < m_listCsHeroSubGear[i].RuneSocketList.Count; j++)
				{
					CsAttr csAttr = CsGameData.Instance.GetAttr(m_listCsHeroSubGear[i].RuneSocketList[j].Item.Value1);
					CsAttrValueInfo csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(m_listCsHeroSubGear[i].RuneSocketList[j].Item.LongValue1);
					AddAttrValue(csAttr.AttrId, csAttrValueInfo.Value);
				}
			}
		}

		// 탈것장비(기본 + 추가속성)
		//int nMountGear = 0;

		for (int i = 0; i < m_listEquippedMountGear.Count; i++)
		{
			CsHeroMountGear csHeroMountGear = GetHeroMountGear(m_listEquippedMountGear[i]);

			//nMountGear += csHeroMountGear.BattlePower + csHeroMountGear.MountGear.BattlePowerValue;

			AddAttrValue((int)EnAttr.MaxHp, csHeroMountGear.MountGear.MaxHp);
			AddAttrValue((int)EnAttr.PhysicalOffense, csHeroMountGear.MountGear.PhysicalOffense);
			AddAttrValue((int)EnAttr.MagicalOffense, csHeroMountGear.MountGear.MagicalOffenseAttr);
			AddAttrValue((int)EnAttr.PhysicalDefense, csHeroMountGear.MountGear.PhysicalDefense);
			AddAttrValue((int)EnAttr.MagicalDefense, csHeroMountGear.MountGear.MagicalDefense);

			for (int j = 0; j < csHeroMountGear.HeroMountGearOptionAttrList.Count; j++)
			{
				AddAttrValue(csHeroMountGear.HeroMountGearOptionAttrList[j].Attr.AttrId, csHeroMountGear.HeroMountGearOptionAttrList[j].AttrValueInfo.Value);
			}
		}

		// 탈것
		//int nMount = 0;

		for (int i = 0; i < m_listCsHeroMount.Count; i++)
		{
			//nMount += m_listCsHeroMount[i].BattlePower;

			/*
			 * 장착여부에 따라 적용계수가 다름. 장착하지 않을 경우 각성레벨에 따라 다름.
			 */

			float flFactor = 1.0f;

			if (CsGameData.Instance.MyHeroInfo.EquippedMountId != m_listCsHeroMount[i].Mount.MountId)
				flFactor = m_listCsHeroMount[i].MountAwakeningLevelMaster.UnequippedAttrFactor;

			AddAttrValue((int)EnAttr.MaxHp, (long)(m_listCsHeroMount[i].MountLevel.MaxHp * flFactor));
			AddAttrValue((int)EnAttr.PhysicalOffense, (long)(m_listCsHeroMount[i].MountLevel.PhysicalOffense * flFactor));
			AddAttrValue((int)EnAttr.MagicalOffense, (long)(m_listCsHeroMount[i].MountLevel.MagicalOffense * flFactor));
			AddAttrValue((int)EnAttr.PhysicalDefense, (long)(m_listCsHeroMount[i].MountLevel.PhysicalDefense * flFactor));
			AddAttrValue((int)EnAttr.MagicalDefense, (long)(m_listCsHeroMount[i].MountLevel.MagicalDefense * flFactor));

			List<CsMountPotionAttrCount> list = CsGameData.Instance.GetMountPotionAttrCountList(m_listCsHeroMount[i].PotionAttrCount);

			for (int j = 0; j < list.Count; j++)
			{
				AddAttrValue(list[j].Attr.AttrId, (long)(list[j].AttrValue.Value * flFactor));
			}
		}

		// 보유날개전체(날개속성)
		for (int i = 0; i < m_listCsHeroWing.Count; i++)
		{
			CsWing csWing = m_listCsHeroWing[i].Wing;

			AddAttrValue(csWing.Attr.AttrId, csWing.AttrValueInfo.Value);

			for (int j = 0; j < csWing.WingMemoryPieceSlotList.Count; j++)
			{
				if (csWing.WingMemoryPieceSlotList[j].OpenStep <= m_listCsHeroWing[i].MemoryPieceStep)
				{
					CsHeroWingMemoryPieceSlot csHeroWingMemoryPieceSlot = m_listCsHeroWing[i].GetHeroWingMemoryPieceSlot(csWing.WingMemoryPieceSlotList[j].SlotIndex);
					AddAttrValue(csWing.WingMemoryPieceSlotList[j].Attr.AttrId, csHeroWingMemoryPieceSlot.AccAttrValue);
				}
			}
		}

		// 날개파트(강화속성)
		//int nWingPart = 0;

		for (int i = 0; i < m_listCsHeroWingPart.Count; i++)
		{
			//nWingPart += m_listCsHeroWingPart[i].GetBattlePower();

			for (int j = 0; j < m_listCsHeroWingPart[i].HeroWingEnchantList.Count; j++)
			{
				CsWingStep csWingStep = CsGameData.Instance.GetWingStep(m_listCsHeroWingPart[i].HeroWingEnchantList[j].Step);
				CsWingStepPart csWingStepPart = csWingStep.GetWingStepPart(m_listCsHeroWingPart[i].PartId);
				CsWingPart csWingPart = CsGameData.Instance.GetWingPart(m_listCsHeroWingPart[i].PartId);

				AddAttrValue(csWingPart.Attr.AttrId, m_listCsHeroWingPart[i].HeroWingEnchantList[j].EnchantCount * csWingStepPart.IncreaseAttrValueInfo.Value);
			}
		}

		// 계급
		//int nRank = 0;

		if (RankNo > 0)
		{
			CsRank csRank = CsGameData.Instance.GetRank(RankNo);
			//nRank = csRank.BattlePower;

			for (int i = 0; i < csRank.RankAttrList.Count; i++)
			{
				AddAttrValue(csRank.RankAttrList[i].Attr.AttrId, csRank.RankAttrList[i].AttrValueInfo.Value);
			}

			// 계급패시브스킬
			for (int i = 0; i < m_listCsHeroRankPassiveSkill.Count; i++)
			{
				CsRankPassiveSkillLevel csRankPassiveSkillLevel = m_listCsHeroRankPassiveSkill[i].RankPassiveSkill.GetRankPassiveSkillLevel(m_listCsHeroRankPassiveSkill[i].Level);

				for (int j = 0; j < csRankPassiveSkillLevel.RankPassiveSkillAttrLevelList.Count; j++)
				{
					AddAttrValue(csRankPassiveSkillLevel.RankPassiveSkillAttrLevelList[j].Attr.AttrId, csRankPassiveSkillLevel.RankPassiveSkillAttrLevelList[j].AttrValue.Value);
				}
			}
		}

		// 길드스킬
		//long lGuildSkill = 0;

		if (CsGuildManager.Instance.Guild != null)
		{
			CsGuildBuildingInstance csGuildBuildingInstance = CsGuildManager.Instance.Guild.GetGuildBuildingInstance(2);
			int nMaxLevel = csGuildBuildingInstance.Level * 10;

			for (int i = 0; i < CsGuildManager.Instance.HeroGuildSkillList.Count; i++)
			{
				int nGuildSkillLevel = 0;

				if (CsGuildManager.Instance.HeroGuildSkillList[i].Level > nMaxLevel)
					nGuildSkillLevel = nMaxLevel;

				else
					nGuildSkillLevel = CsGuildManager.Instance.HeroGuildSkillList[i].Level;

				//lGuildSkill += CsGuildManager.Instance.HeroGuildSkillList[i].GetBattlePower(nGuildSkillLevel);

				List<CsGuildSkillLevelAttrValue> list = CsGuildManager.Instance.HeroGuildSkillList[i].GetGuildSkillLevelAttrValue(nGuildSkillLevel);

				for (int j = 0; j < list.Count; j++)
				{
					AddAttrValue(list[j].Attr.AttrId, list[j].AttrValue.Value);
				}
			}
		}

		// 국가관직
		//long lNationNoblesse = 0;

		CsNationNoblesseInstance csNationNoblesseInstance = GetNationNoblesseInstanceByHeroId(HeroId);

		if (csNationNoblesseInstance != null)
		{
			for (int i = 0; i < csNationNoblesseInstance.NationNoblesse.NationNoblesseAttrList.Count; i++)
			{
				//lNationNoblesse += csNationNoblesseInstance.NationNoblesse.NationNoblesseAttrList[i].BattlePower;
				AddAttrValue(csNationNoblesseInstance.NationNoblesse.NationNoblesseAttrList[i].Attr.AttrId, csNationNoblesseInstance.NationNoblesse.NationNoblesseAttrList[i].AttrValueInfo.Value);
			}
		}

		// 칭호
		//long lTitle = 0;

		// 칭호 - 패시브
		for (int i = 0; i < CsTitleManager.Instance.HeroTitleList.Count; i++)
		{
			CsHeroTitle csHeroTitle = CsTitleManager.Instance.HeroTitleList[i];

			if (CsTitleManager.Instance.ActivationTitleId == csHeroTitle.Title.TitleId)
				continue;

			for (int j = 0; j < csHeroTitle.Title.TitlePassiveAttrList.Count; j++)
			{
				//lTitle += csHeroTitle.Title.TitlePassiveAttrList[j].BattlePower;
				AddAttrValue(csHeroTitle.Title.TitlePassiveAttrList[j].Attr.AttrId, csHeroTitle.Title.TitlePassiveAttrList[j].AttrValue.Value);
			}
		}

		// 칭호 - 액티브
		if (CsTitleManager.Instance.ActivationTitleId > 0)
		{
			CsHeroTitle csHeroTitle = CsTitleManager.Instance.GetHeroTitle(CsTitleManager.Instance.ActivationTitleId);

			for (int i = 0; i < csHeroTitle.Title.TitleActiveAttrList.Count; i++)
			{
				//lTitle += csHeroTitle.Title.TitleActiveAttrList[i].BattlePower;
				AddAttrValue(csHeroTitle.Title.TitleActiveAttrList[i].Attr.AttrId, csHeroTitle.Title.TitleActiveAttrList[i].AttrValue.Value);
			}
		}

		// 도감
		//long lIllustratedBook = 0;

		for (int i = 0; i < CsGameData.Instance.IllustratedBookExplorationStepList.Count; i++)
		{
			if (CsGameData.Instance.IllustratedBookExplorationStepList[i].StepNo <= CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo)
			{
				CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.IllustratedBookExplorationStepList[i];

				for (int j = 0; j < csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList.Count; j++)
				{
					//lIllustratedBook += csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].BattlePower;
					AddAttrValue(csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].Attr.AttrId, csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].AttrValue.Value);
				}
			}
		}

		// 도감 - 활성북
		for (int i = 0; i < CsIllustratedBookManager.Instance.ActivationIllustratedBookIdList.Count; i++)
		{
			CsIllustratedBook csIllustratedBook = CsGameData.Instance.GetIllustratedBook(CsIllustratedBookManager.Instance.ActivationIllustratedBookIdList[i]);

			if (csIllustratedBook != null)
			{
				for (int j = 0; j < csIllustratedBook.IllustratedBookAttrList.Count; j++)
				{
					//lIllustratedBook += csIllustratedBook.IllustratedBookAttrList[j].BattlePower;
					AddAttrValue(csIllustratedBook.IllustratedBookAttrList[j].Attr.AttrId, csIllustratedBook.IllustratedBookAttrList[j].AttrValue.Value);
				}
			}
		}

		// 정예도감
		//long lElite = 0;

		for (int i = 0; i < CsEliteManager.Instance.HeroEliteMonsterKillList.Count; i++)
		{
			CsEliteMonsterKillAttrValue csEliteMonsterKillAttrValue = CsEliteManager.Instance.HeroEliteMonsterKillList[i].GetEliteMonsterKillAttrValue();

			if (csEliteMonsterKillAttrValue != null)
			{
				//lElite += CsEliteManager.Instance.HeroEliteMonsterKillList[i].EliteMonster.Attr.BattlePowerFactor * csEliteMonsterKillAttrValue.AttrValue.Value;
				AddAttrValue(CsEliteManager.Instance.HeroEliteMonsterKillList[i].EliteMonster.Attr.AttrId, csEliteMonsterKillAttrValue.AttrValue.Value);
			}
		}

		// 크리처카드컬렉션.
		//long lCollection = 0;

		for (int i = 0; i < CsCreatureCardManager.Instance.ActivatedCreatureCardCollectionList.Count; i++)
		{
			CsCreatureCardCollection csCreatureCardCollection = CsGameData.Instance.GetCreatureCardCollection(CsCreatureCardManager.Instance.ActivatedCreatureCardCollectionList[i]);

			if (csCreatureCardCollection != null)
			{
				for (int j = 0; j < csCreatureCardCollection.CreatureCardCollectionAttrList.Count; j++)
				{
					//lCollection += csCreatureCardCollection.CreatureCardCollectionAttrList[j].BattlePower;
					AddAttrValue(csCreatureCardCollection.CreatureCardCollectionAttrList[j].Attr.AttrId, csCreatureCardCollection.CreatureCardCollectionAttrList[j].AttrValue.Value);
				}
			}
		}

		// 기본 레벨 속성.
		//long lLevel = MaxHp * CsGameData.Instance.GetAttr(EnAttr.MaxHp).BattlePowerFactor;

		//AddAttrValue((int)EnAttr.MaxHp, MaxHp);

		//long lValue = 0;
		//if (m_dicAttrValue.ContainsKey((int)EnAttr.MaxHp))
		//{
		//	//lValue = m_dicAttrValue[(int)EnAttr.MaxHp];
		//	m_dicAttrValue[(int)EnAttr.MaxHp] = MaxHp;
		//}
		//else
		//{
		//	m_dicAttrValue.Add((int)EnAttr.MaxHp, MaxHp);
		//}

		//long lDup = lValue * CsGameData.Instance.GetAttr(EnAttr.MaxHp).BattlePowerFactor;

		if (m_dicAttrValue.ContainsKey((int)EnAttr.OffenseAddPer))
		{
			int nPysicalOffense = (int)(m_csJobLevel.PhysicalOffense * (1 + m_dicAttrValue[(int)EnAttr.OffenseAddPer] / 10000f));
			//lLevel += nPysicalOffense * CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.PhysicalOffense, nPysicalOffense);

			int nMagicalOffense = (int)(m_csJobLevel.MagicalOffense * (1 + m_dicAttrValue[(int)EnAttr.OffenseAddPer] / 10000f));
			//lLevel += nMagicalOffense * CsGameData.Instance.GetAttr(EnAttr.MagicalOffense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.MagicalOffense, nMagicalOffense);
		}
		else
		{
			//lLevel += m_csJobLevel.PhysicalOffense * CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.PhysicalOffense, m_csJobLevel.PhysicalOffense);

			//lLevel += m_csJobLevel.MagicalOffense * CsGameData.Instance.GetAttr(EnAttr.MagicalOffense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.MagicalOffense, m_csJobLevel.MagicalOffense);
		}

		if (m_dicAttrValue.ContainsKey((int)EnAttr.PhysicalDefenseAddPer))
		{
			int nPhysicalDefense = (int)(m_csJobLevel.PhysicalDefense * (1 + m_dicAttrValue[(int)EnAttr.PhysicalDefenseAddPer] / 10000f));
			//lLevel += nPhysicalDefense * CsGameData.Instance.GetAttr(EnAttr.PhysicalDefense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.PhysicalDefense, nPhysicalDefense);
		}
		else
		{
			//lLevel += m_csJobLevel.PhysicalDefense * CsGameData.Instance.GetAttr(EnAttr.PhysicalDefense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.PhysicalDefense, m_csJobLevel.PhysicalDefense);
		}

		if (m_dicAttrValue.ContainsKey((int)EnAttr.MagicalDefenseAddPer))
		{
			int nMagicalDefense = (int)(m_csJobLevel.MagicalDefense * (1 + m_dicAttrValue[(int)EnAttr.MagicalDefenseAddPer] / 10000f));
			//lLevel += nMagicalDefense * CsGameData.Instance.GetAttr(EnAttr.MagicalDefense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.MagicalDefense, nMagicalDefense);
		}
		else
		{
			//lLevel += m_csJobLevel.MagicalDefense * CsGameData.Instance.GetAttr(EnAttr.MagicalDefense).BattlePowerFactor;
			AddAttrValue((int)EnAttr.MagicalDefense, m_csJobLevel.MagicalDefense);
		}

		// 내원소.
		long lMyElemental = 0;
		switch ((EnElemental)Job.Elemental.ElementalId)
		{
			case EnElemental.Fire:
				if (m_dicAttrValue.ContainsKey((int)EnAttr.EnchantFire))
				{
					lMyElemental = (long)(m_dicAttrValue[(int)EnAttr.EnchantFire] * 0.8f * CsGameData.Instance.GetAttr(EnAttr.EnchantFire).BattlePowerFactor);
				}
				break;
			case EnElemental.Electric:
				if (m_dicAttrValue.ContainsKey((int)EnAttr.EnchantElectric))
				{
					lMyElemental = (long)(m_dicAttrValue[(int)EnAttr.EnchantElectric] * 0.8f * CsGameData.Instance.GetAttr(EnAttr.EnchantElectric).BattlePowerFactor);
				}
				break;
			case EnElemental.Light:
				if (m_dicAttrValue.ContainsKey((int)EnAttr.EnchantLight))
				{
					lMyElemental = (long)(m_dicAttrValue[(int)EnAttr.EnchantLight] * 0.8f * CsGameData.Instance.GetAttr(EnAttr.EnchantLight).BattlePowerFactor);
				}
				break;
			case EnElemental.Dark:
				if (m_dicAttrValue.ContainsKey((int)EnAttr.EnchantDark))
				{
					lMyElemental = (long)(m_dicAttrValue[(int)EnAttr.EnchantDark] * 0.8f * CsGameData.Instance.GetAttr(EnAttr.EnchantDark).BattlePowerFactor);
				}
				break;
		}

		//long lAttack = 0;
		if (m_dicAttrValue.ContainsKey((int)EnAttr.Attack))
		{
			if (Job.OffenseType == EnOffenseType.Physical)
			{
				//lAttack = m_dicAttrValue[(int)EnAttr.Attack] * CsGameData.Instance.GetAttr((int)EnAttr.PhysicalOffense).BattlePowerFactor;
				AddAttrValue((int)EnAttr.PhysicalOffense, m_dicAttrValue[(int)EnAttr.Attack]);
			}
			else
			{
				//lAttack = m_dicAttrValue[(int)EnAttr.Attack] * CsGameData.Instance.GetAttr((int)EnAttr.MagicalOffense).BattlePowerFactor;
				AddAttrValue((int)EnAttr.MagicalOffense, m_dicAttrValue[(int)EnAttr.Attack]);
			}
		}

		// 크리처
		for (int i = 0; i < CsCreatureManager.Instance.HeroCreatureList.Count; i++)
		{
			// 출전크리처 또는 응원중인 크리쳐들만 전투력에 포함된다.
			if (CsCreatureManager.Instance.HeroCreatureList[i].Cheered || CsCreatureManager.Instance.HeroCreatureList[i].InstanceId == CsCreatureManager.Instance.ParticipatedCreatureId)
			{
				float flCreatureAttrFactor = CsCreatureManager.Instance.HeroCreatureList[i].Cheered ? CsGameConfig.Instance.CreatureCheerAttrFactor : 1;

				CsHeroCreature csHeroCreature = CsCreatureManager.Instance.HeroCreatureList[i];

				// 기본속성
				for (int j = 0; j < csHeroCreature.HeroCreatureBaseAttrList.Count; j++)
				{
					CsCreatureBaseAttrValue csCreatureBaseAttrValue = csHeroCreature.Creature.GetCreatureBaseAttrValue(csHeroCreature.HeroCreatureBaseAttrList[j].CreatureBaseAttr.Attr.AttrId);
					AddAttrValue(csHeroCreature.HeroCreatureBaseAttrList[j].CreatureBaseAttr.Attr.AttrId, (long)((csHeroCreature.HeroCreatureBaseAttrList[j].BaseValue + (int)((csHeroCreature.Level * csCreatureBaseAttrValue.IncAttrValue) * (csHeroCreature.Quality / 1000.0))) * flCreatureAttrFactor));
				}

				// 추가속성
				for (int j = 0; j < csHeroCreature.AdditionalAttrIdList.Count; j++)
				{
					CsCreatureAdditionalAttr csCreatureAdditionalAttr = CsGameData.Instance.GetCreatureAdditionalAttr(csHeroCreature.AdditionalAttrIdList[j]);
					CsCreatureAdditionalAttrValue csCreatureAdditionalAttrValue = csCreatureAdditionalAttr.GetCreatureAdditionalAttrValue(csHeroCreature.InjectionLevel);
					AddAttrValue(csCreatureAdditionalAttrValue.Attr.AttrId, (long)(csCreatureAdditionalAttrValue.AttrValue.Value * flCreatureAttrFactor));
				}

				// 스킬
				for (int j = 0; j < csHeroCreature.HeroCreatureSkillList.Count; j++)
				{
					CsCreatureSkillAttr csCreatureSkillAttr = csHeroCreature.HeroCreatureSkillList[j].CreatureSkill.GetCreatureSkillAttr(csHeroCreature.HeroCreatureSkillList[j].CreatureSkillGrade.SkillGrade);
					AddAttrValue(csHeroCreature.HeroCreatureSkillList[j].CreatureSkill.Attr.AttrId, (long)(csCreatureSkillAttr.AttrValue.Value * flCreatureAttrFactor));
				}
			}
		}

		// 영웅속성포션
		for (int i = 0; i < m_listCsHeroPotionAttr.Count; i++)
		{
			AddAttrValue(m_listCsHeroPotionAttr[i].PotionAttr.Attr.AttrId, m_listCsHeroPotionAttr[i].Count * m_listCsHeroPotionAttr[i].PotionAttr.AttrValueInc.Value);
		}

		// 별자리
		for (int i = 0; i < CsConstellationManager.Instance.HeroConstellationList.Count; i++)
		{
			for (int j = 0; j < CsConstellationManager.Instance.HeroConstellationList[i].HeroConstellationStepList.Count; j++)
			{
				CsHeroConstellationStep csHeroConstellationStep = CsConstellationManager.Instance.HeroConstellationList[i].HeroConstellationStepList[j];

				if (csHeroConstellationStep.Activated)  // 최종일경우.
				{
					// 현재 사이클
					for (int k = 0; k < csHeroConstellationStep.ConstellationCycle.ConstellationCycleBuffList.Count; k++)
					{
						AddAttrValue(csHeroConstellationStep.ConstellationCycle.ConstellationCycleBuffList[k].Attr.AttrId, csHeroConstellationStep.ConstellationCycle.ConstellationCycleBuffList[k].AttrValue.Value);
					}

					// 현재 엔트리
					for (int k = 0; k < csHeroConstellationStep.ConstellationEntry.ConstellationEntryBuffList.Count; k++)
					{
						AddAttrValue(csHeroConstellationStep.ConstellationEntry.ConstellationEntryBuffList[k].Attr.AttrId, csHeroConstellationStep.ConstellationEntry.ConstellationEntryBuffList[k].AttrValue.Value);
					}
				}
				else
				{
					if (csHeroConstellationStep.Cycle == 1)
					{
						// 엔트리 버프만 계산
						if (csHeroConstellationStep.EntryNo > 1)
						{
							CsConstellationEntry csConstellationEntry = csHeroConstellationStep.ConstellationCycle.GetConstellationEntry(csHeroConstellationStep.EntryNo - 1);

							for (int k = 0; k < csConstellationEntry.ConstellationEntryBuffList.Count; k++)
							{
								AddAttrValue(csConstellationEntry.ConstellationEntryBuffList[k].Attr.AttrId, csConstellationEntry.ConstellationEntryBuffList[k].AttrValue.Value);
							}
						}
					}
					else
					{
						// 이전 사이클
						CsConstellationCycle csConstellationCycle = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(csHeroConstellationStep.Cycle - 1);

						for (int k = 0; k < csConstellationCycle.ConstellationCycleBuffList.Count; k++)
						{
							AddAttrValue(csConstellationCycle.ConstellationCycleBuffList[k].Attr.AttrId, csConstellationCycle.ConstellationCycleBuffList[k].AttrValue.Value);
						}

						CsConstellationEntry csConstellationEntry = null;

						if (csHeroConstellationStep.EntryNo == 1)
						{
							// 이전 사이클의 마지막 엔트리
							csConstellationEntry = csConstellationCycle.ConstellationEntryList[csConstellationCycle.ConstellationEntryList.Count - 1];
						}
						else
						{
							// 현재 사이클의 이전 엔트리
							csConstellationEntry = csHeroConstellationStep.ConstellationCycle.GetConstellationEntry(csHeroConstellationStep.EntryNo - 1);
						}

						for (int k = 0; k < csConstellationEntry.ConstellationEntryBuffList.Count; k++)
						{
							AddAttrValue(csConstellationEntry.ConstellationEntryBuffList[k].Attr.AttrId, csConstellationEntry.ConstellationEntryBuffList[k].AttrValue.Value);
						}
					}
				}
			}
		}

		// 아티팩트
		if (CsArtifactManager.Instance.ArtifactNo > 0)
		{
			for (int i = 0; i < CsArtifactManager.Instance.ArtifactNo; i++)
			{
				CsArtifact csArtifact = CsGameData.Instance.ArtifactList[i];

				CsArtifactLevel csArtifactLevel = null;

				if (i == CsArtifactManager.Instance.ArtifactNo - 1)
				{
					// 현재 레벨의 속성 리스트
					csArtifactLevel = csArtifact.GetArtifactLevel(CsArtifactManager.Instance.ArtifactLevel);
				}
				else
				{
					// 최대 레벨의 속성 리스트
					csArtifactLevel = csArtifact.ArtifactLevelList[csArtifact.ArtifactLevelList.Count - 1];
				}

				for (int j = 0; j < csArtifactLevel.ArtifactLevelAttrList.Count; j++)
				{
					AddAttrValue(csArtifactLevel.ArtifactLevelAttrList[j].Attr.AttrId, csArtifactLevel.ArtifactLevelAttrList[j].AttrValue.Value);
				}
			}
		}

		// 코스튬
		for (int i = 0; i < CsCostumeManager.Instance.HeroCostumeList.Count; i++)
		{
			List<CsCostumeEnchantLevelAttr> list = CsGameData.Instance.GetCostumeEnchantLevelAttrList(CsCostumeManager.Instance.HeroCostumeList[i].Costume.CostumeId, CsCostumeManager.Instance.HeroCostumeList[i].EnchantLevel);

			for (int j = 0; j < list.Count; j++)
			{
				AddAttrValue(list[j].Attr.AttrId, list[j].AttrValue.Value);
			}
		}

		// 코스튬 콜렉션.
		if (CsCostumeManager.Instance.CostumeCollectionActivated)
		{
			CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(CsCostumeManager.Instance.CostumeCollectionId);

			for (int i = 0; i < csCostumeCollection.CostumeCollectionAttrList.Count; i++)
			{
				AddAttrValue(csCostumeCollection.CostumeCollectionAttrList[i].Attr.AttrId, csCostumeCollection.CostumeCollectionAttrList[i].AttrValue.Value);
			}
		}

		// 업적포인트
		if (CsAccomplishmentManager.Instance.AccomplishmentLevel > 0)
		{
			CsAccomplishmentLevel csAccomplishmentLevel = CsGameData.Instance.GetAccomplishmentLevel(CsAccomplishmentManager.Instance.AccomplishmentLevel);

			if (csAccomplishmentLevel != null)
			{
				for (int i = 0; i < csAccomplishmentLevel.AccomplishmentLevelAttrList.Count; i++)
				{
					AddAttrValue(csAccomplishmentLevel.AccomplishmentLevelAttrList[i].Attr.AttrId, csAccomplishmentLevel.AccomplishmentLevelAttrList[i].AttrValue.Value);
				}
			}
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////

		if (m_dicAttrValue.ContainsKey((int)EnAttr.MaxHp))
		{
			m_dicAttrValue[(int)EnAttr.MaxHp] = MaxHp;
		}
		else
		{
			m_dicAttrValue.Add((int)EnAttr.MaxHp, MaxHp);
		}

		long lOldBattlePower = m_lBattlePower;

		m_lBattlePower = lSkill + lMyElemental;

		m_lBattlePower += GetBattlePower();

		if (m_lBattlePower != lOldBattlePower)
		{
			if (!bIsIntro)
			{
				CsGameEventUIToUI.Instance.OnEventToastChangeBattlePower(lOldBattlePower);
			}

			if (CsAccomplishmentManager.Instance.MaxBattlePower < m_lBattlePower)
				CsAccomplishmentManager.Instance.MaxBattlePower = m_lBattlePower;
		}
	}

	//---------------------------------------------------------------------------------------------------
	long GetBattlePower()
	{
		long lBattlePower = 0;

		for (int i = 0; i < CsGameData.Instance.AttrList.Count; i++)
		{
			if (m_dicAttrValue.ContainsKey(CsGameData.Instance.AttrList[i].AttrId))
			{
				lBattlePower += m_dicAttrValue[CsGameData.Instance.AttrList[i].AttrId] * CsGameData.Instance.GetAttr(CsGameData.Instance.AttrList[i].AttrId).BattlePowerFactor;
			}
		}

		return lBattlePower;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetStamina(bool bAutoRecovery, int nStamina)
	{
		// 소모한 경우
		if (nStamina < m_nStamina)
		{
			// 최대 상태에서 스태미나를 소모한 경우
			if (m_nStamina >= CsGameConfig.Instance.MaxStamina)
			{
				m_flStaminaAutoRecoveryRemainingTime = CsGameConfig.Instance.StaminaRecoveryTime + Time.realtimeSinceStartup;
			}
		}
		// 회복한 경우
		else if (m_nStamina < nStamina)
		{
			// 자동 회복으로 회복한 경우
			if (nStamina < CsGameConfig.Instance.MaxStamina)
			{
				if (bAutoRecovery)
				{
					m_flStaminaAutoRecoveryRemainingTime = CsGameConfig.Instance.StaminaRecoveryTime + Time.realtimeSinceStartup;
				}
			}
			// 스태미나가 모두 회복된 경우
			else
			{
				m_flStaminaAutoRecoveryRemainingTime = 0;
			}
		}

		m_nStamina = nStamina;
	}

	//---------------------------------------------------------------------------------------------------

	#region Gear

	//---------------------------------------------------------------------------------------------------
	public void AddHeroMainGears(PDFullHeroMainGear[] heroMainGears, bool bClearAll = false, bool bEvent = true)
	{
		if (heroMainGears != null)
		{
			if (bClearAll)
			{
				m_listCsHeroMainGear.Clear();
			}

			List<CsHeroObject> listCsHeroObject = new List<CsHeroObject>();

			for (int i = 0; i < heroMainGears.Length; i++)
			{
				CsHeroMainGear csHeroMainGear = new CsHeroMainGear(heroMainGears[i]);
				m_listCsHeroMainGear.Add(csHeroMainGear);
				listCsHeroObject.Add(csHeroMainGear);
			}

			if (bEvent)
				CsGameEventUIToUI.Instance.OnEventHeroObjectGot(listCsHeroObject);

			SortHeroMainGearList();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SortHeroMainGearList()
	{
		m_listCsHeroMainGear.Sort((CsHeroMainGear gear1, CsHeroMainGear gear2) =>
		{
			// 티어
			int nComp = gear1.MainGear.MainGearTier.Tier.CompareTo(gear2.MainGear.MainGearTier.Tier);

			if (nComp == 0)
			{
				// 등급
				nComp = gear1.MainGear.MainGearGrade.Grade.CompareTo(gear2.MainGear.MainGearGrade.Grade);

				if (nComp == 0)
				{
					// 품질
					nComp = gear1.MainGear.MainGearQuality.Quality.CompareTo(gear2.MainGear.MainGearQuality.Quality);

					if (nComp == 0)
					{
						// 직업
						nComp = gear1.MainGear.JobId.CompareTo(gear2.MainGear.JobId);

						if (nComp == 0)
						{
							// 타입
							nComp = gear1.MainGear.MainGearType.MainGearType.CompareTo(gear2.MainGear.MainGearType.MainGearType);

							if (nComp == 0)
							{
								// 인스턴스.
								return gear1.Id.CompareTo(gear2.Id);
							}
							else
							{
								return nComp;
							}
						}
						else
						{
							return nComp;
						}
					}
					else
					{
						return nComp * (int)EnSortOrder.Descending;
					}
				}
				else
				{
					return nComp * (int)EnSortOrder.Descending;
				}
			}
			else
			{
				return nComp * (int)EnSortOrder.Descending;
			}

		});
	}

	//---------------------------------------------------------------------------------------------------
	public void AddHeroSubGears(PDFullHeroSubGear[] heroSubGear, bool bClearAll = false, bool bEvent = true)
	{
		if (heroSubGear != null)
		{
			if (bClearAll)
			{
				m_listCsHeroSubGear.Clear();
			}

			List<CsHeroObject> listCsHeroObject = new List<CsHeroObject>();

			for (int i = 0; i < heroSubGear.Length; i++)
			{
				CsHeroSubGear csHeroSubGear = new CsHeroSubGear(heroSubGear[i]);
				m_listCsHeroSubGear.Add(csHeroSubGear);
				listCsHeroObject.Add(csHeroSubGear);
			}

			if (bEvent)
				CsGameEventUIToUI.Instance.OnEventHeroObjectGot(listCsHeroObject);

			SortSubGearList();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SortSubGearList()
	{
		m_listCsHeroSubGear.Sort((CsHeroSubGear gear1, CsHeroSubGear gear2) =>
		{
			return gear1.SubGear.SubGearId.CompareTo(gear2.SubGear.SubGearId);

		});
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear GetHeroMainGear(Guid guid)
	{
		for (int i = 0; i < m_listCsHeroMainGear.Count; i++)
		{
			if (m_listCsHeroMainGear[i].Id == guid)
				return m_listCsHeroMainGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear GetHeroMainGearBySlotIndex(int nSlotIndex)
	{
		for (int i = 0; i < m_listCsHeroMainGearEquipped.Count; i++)
		{
			if (m_listCsHeroMainGearEquipped[i] == null)
				continue;

			if (m_listCsHeroMainGearEquipped[i].MainGear.MainGearType.SlotIndex == nSlotIndex)
				return m_listCsHeroMainGearEquipped[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGear GetHeroSubGear(int nSubGearId)
	{
		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			if (m_listCsHeroSubGear[i].SubGear.SubGearId == nSubGearId)
				return m_listCsHeroSubGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGear GetHeroSubGearBySlotIndex(int nSlotIndex)
	{
		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			if (m_listCsHeroSubGear[i].SubGear.SlotIndex == nSlotIndex)
				return m_listCsHeroSubGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateHeroSubGearEquipped(int nSubGearId, bool bEquipped)
	{
		CsHeroSubGear csHeroSubGear = GetHeroSubGear(nSubGearId);
		csHeroSubGear.Equipped = bEquipped;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear GetHeroMainGearEquipped(Guid guid)
	{
		for (int i = 0; i < m_listCsHeroMainGearEquipped.Count; i++)
		{
			if (m_listCsHeroMainGearEquipped[i] == null)
				continue;

			if (m_listCsHeroMainGearEquipped[i].Id == guid)
				return m_listCsHeroMainGearEquipped[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetHeroMainGearEquipped(Guid guid)
	{
		CsHeroMainGear csHeroMainGear = GetHeroMainGear(guid);

		if (csHeroMainGear != null)
		{
			m_listCsHeroMainGearEquipped[csHeroMainGear.MainGear.MainGearType.EquippedIndex] = csHeroMainGear;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckNoticeMainGearEnchant()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MainGearEnchant))
			return false;

		if (m_csVipLevel.MainGearEnchantMaxCount <= m_nMainGearEnchantDailyCount)
			return false;

		for (int i = 0; i < m_listCsHeroMainGearEquipped.Count; i++)
		{
			if (m_listCsHeroMainGearEquipped[i] != null && CsGameData.Instance.GetMainGearEnchantLevel(m_listCsHeroMainGearEquipped[i].EnchantLevel + 1) != null)
			{
				int nItemId = m_listCsHeroMainGearEquipped[i].MainGearEnchantLevel.MainGearEnchantStep.NextEnchantMaterialItem.ItemId;
				int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId);

				if (nCount > 0)
					return true;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckNoticeSubGearLevelUp()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearLevelUp))
			return false;

		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			if (m_listCsHeroSubGear[i].Equipped)
			{
				switch (m_listCsHeroSubGear[i].NextStep)
				{
					case EnNextStep.Quality:
						CsSubGearLevelQuality csSubGearLevelQuality = m_listCsHeroSubGear[i].SubGearLevel.GetSubGearLevelQuality(m_listCsHeroSubGear[i].Quality);

						if (csSubGearLevelQuality.NextQualityUpItem1 != null && csSubGearLevelQuality.NextQualityUpItem2 != null && CsGameData.Instance.MyHeroInfo.Level > m_listCsHeroSubGear[i].SubGearLevel.Level)
						{
							int nCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevelQuality.NextQualityUpItem1.ItemId);

							if (nCount1 >= csSubGearLevelQuality.NextQualityUpItem1Count)
							{
								int nCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevelQuality.NextQualityUpItem2.ItemId);

								if (nCount2 >= csSubGearLevelQuality.NextQualityUpItem2Count)
								{
									return true;
								}
							}
						}
						else if (csSubGearLevelQuality.NextQualityUpItem1 != null && csSubGearLevelQuality.NextQualityUpItem2 == null)
						{
							int nCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevelQuality.NextQualityUpItem1.ItemId);

							if (nCount1 >= csSubGearLevelQuality.NextQualityUpItem1Count)
							{
								return true;
							}
						}
						else if (csSubGearLevelQuality.NextQualityUpItem1 == null && csSubGearLevelQuality.NextQualityUpItem2 != null)
						{
							int nCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevelQuality.NextQualityUpItem2.ItemId);

							if (nCount2 >= csSubGearLevelQuality.NextQualityUpItem2Count)
							{
								return true;
							}
						}
						break;

					case EnNextStep.Level:
						if (m_listCsHeroSubGear[i].SubGearLevel.Level < CsGameData.Instance.MyHeroInfo.Level)
						{
							if (m_listCsHeroSubGear[i].SubGearLevel.NextLevelUpRequiredGold <= CsGameData.Instance.MyHeroInfo.Gold)
							{
								return true;
							}
						}
						break;

					case EnNextStep.Grade:
						if (m_listCsHeroSubGear[i].SubGearLevel.Level < CsGameData.Instance.MyHeroInfo.Level)
						{
							if (m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1 != null && m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2 != null)
							{
								int nCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1.ItemId);

								if (nCount1 >= m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1Count)
								{
									int nCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2.ItemId);

									if (nCount2 >= m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2Count)
									{
										return true;
									}
								}
							}
							else if (m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1 != null && m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2 == null)
							{
								int nCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1.ItemId);

								if (nCount1 >= m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1Count)
								{
									return true;
								}
							}
							else if (m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem1 == null && m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2 != null)
							{
								int nCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2.ItemId);

								if (nCount2 >= m_listCsHeroSubGear[i].SubGearLevel.NextGradeUpItem2Count)
								{
									return true;
								}
							}
						}
						break;
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckNoticeSubGearSoulstone()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearSoulstone))
			return false;

		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			// 장착보조장비
			if (m_listCsHeroSubGear[i].Equipped)
			{
				// 소켓리스트 장착가능한 빈슬롯이 있는지 체크
				for (int j = 0; j < m_listCsHeroSubGear[i].SubGear.SubGearSoulstoneSocketList.Count; j++)
				{
					CsSubGearSoulstoneSocket csSubGearSoulstoneSocket = m_listCsHeroSubGear[i].SubGear.SubGearSoulstoneSocketList[j];

					// 요구등급체크
					if (csSubGearSoulstoneSocket.RequiredSubGearGrade.Grade <= m_listCsHeroSubGear[i].SubGearLevel.SubGearGrade.Grade)
					{
						CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = m_listCsHeroSubGear[i].GetHeroSubGearSoulstoneSocket(csSubGearSoulstoneSocket.SocketIndex);

						// 빈슬롯
						if (csHeroSubGearSoulstoneSocket == null)
						{
							int nCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType(csSubGearSoulstoneSocket.ItemType);

							if (nCount > 0)
							{
								return true;
							}
						}
						else
						{
							// 장착 중인 소울스톤보다 높은 레벨의 소울스톤이 있는 경우
							List<CsInventorySlot> listSoulStone = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindAll(inventorySlot => inventorySlot.EnType == EnInventoryObjectType.Item &&
																																			inventorySlot.InventoryObjectItem.Item.ItemType.ItemType == csHeroSubGearSoulstoneSocket.Item.ItemType.ItemType &&
																																			inventorySlot.InventoryObjectItem.Item.Level > csHeroSubGearSoulstoneSocket.Item.Level);

							if (listSoulStone.Count > 0)
							{
								return true;
							}
						}
					}
				}

				// 아이템 합성 가능한 장착 소켓이 있는지 체크
				for (int j = 0; j < m_listCsHeroSubGear[i].SoulstoneSocketList.Count; j++)
				{
					CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(m_listCsHeroSubGear[i].SoulstoneSocketList[j].Item.ItemId);

					if (csItemCompositionRecipe == null)
						continue;

					int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemCompositionRecipe.MaterialItemId);

					if (nCount >= csItemCompositionRecipe.MaterialItemCount - 1 && CsGameData.Instance.MyHeroInfo.Gold >= csItemCompositionRecipe.Gold)
					{
						return true;
					}
				}
			}
		}

		return false;
	}


	//---------------------------------------------------------------------------------------------------
	public bool CheckNoticeSubGearRune()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearRune))
			return false;

		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			// 장착보조장비
			if (m_listCsHeroSubGear[i].Equipped)
			{
				// 소켓리스트 장착가능한 빈슬롯이 있는지 체크
				for (int j = 0; j < m_listCsHeroSubGear[i].SubGear.SubGearRuneSocketList.Count; j++)
				{
					CsSubGearRuneSocket csSubGearRuneSocket = m_listCsHeroSubGear[i].SubGear.SubGearRuneSocketList[j];

					// 요구레벨
					if (m_listCsHeroSubGear[i].Level >= csSubGearRuneSocket.RequiredSubGearLevel)
					{
						CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = m_listCsHeroSubGear[i].GetHeroSubGearRuneSocket(csSubGearRuneSocket.SocketIndex);

						if (csHeroSubGearRuneSocket == null)
						{
							for (int k = 0; k < csSubGearRuneSocket.SubGearRuneSocketAvailableItemTypeList.Count; k++)
							{
								int nCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType(csSubGearRuneSocket.SubGearRuneSocketAvailableItemTypeList[k].ItemType);

								if (nCount > 0)
								{
									return true;
								}
							}
						}
					}
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckMainGearEnchantLevelSet()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MainGearEnchant))
			return false;

		int nTotalEnchantLevel = 0;

		for (int i = 0; i < m_listCsHeroMainGearEquipped.Count; i++)
		{
			if (m_listCsHeroMainGearEquipped[i] != null)
			{
				nTotalEnchantLevel += m_listCsHeroMainGearEquipped[i].EnchantLevel;
			}
		}

		CsMainGearEnchantLevelSet csMainGearEnchantLevelSet = CsGameData.Instance.GetMainGearEnchantLevelSetByEnchantLevel(nTotalEnchantLevel);

		if (csMainGearEnchantLevelSet == null)
			return false;

		if (csMainGearEnchantLevelSet.SetNo > m_nMainGearEnchantLevelSetNo)
			return true;
		else
			return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckSubGearSoulstoneLevelSet()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearSoulstone))
			return false;

		int nTotalLevel = 0;

		for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
		{
			if (m_listCsHeroSubGear[i].Equipped)
			{
				for (int j = 0; j < m_listCsHeroSubGear[i].SoulstoneSocketList.Count; j++)
				{
					nTotalLevel += m_listCsHeroSubGear[i].SoulstoneSocketList[j].Item.Level;
				}
			}
		}

		CsSubGearSoulstoneLevelSet csSubGearSoulstoneLevelSet = CsGameData.Instance.GetSubGearSoulstoneLevelSetByLevel(nTotalLevel);

		if (csSubGearSoulstoneLevelSet == null)
			return false;

		if (csSubGearSoulstoneLevelSet.SetNo > m_nSubGearSoulstoneLevelSetNo)
			return true;
		else
			return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckOrdealQuest()
	{
		if (CsOrdealQuestManager.Instance.RewardReceivable)
			return true;

		if (CsOrdealQuestManager.Instance.CsHeroOrdealQuest != null &&
			CsOrdealQuestManager.Instance.CsHeroOrdealQuest.HeroOrdealQuestSlotList != null)
		{
			foreach (CsHeroOrdealQuestSlot csHeroOrdealQuestSlot in CsOrdealQuestManager.Instance.CsHeroOrdealQuest.HeroOrdealQuestSlotList)
			{
				if (csHeroOrdealQuestSlot.Receivable)
					return true;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckPotionAttr()
	{
		int nHeroPotionAttrUseCount = 0;
		CsHeroPotionAttr csHeroPotionAttr = null;

		for (int i = 0; i < CsGameData.Instance.PotionAttrList.Count; i++)
		{
			csHeroPotionAttr = m_listCsHeroPotionAttr.Find(a => a.PotionAttr.PotionAttrId == CsGameData.Instance.PotionAttrList[i].PotionAttrId);

			if (csHeroPotionAttr == null)
			{
				nHeroPotionAttrUseCount = 0;
			}
			else
			{
				nHeroPotionAttrUseCount = csHeroPotionAttr.Count;
			}

			if (0 < GetItemCount(CsGameData.Instance.PotionAttrList[i].ItemRequired.ItemId) &&
				nHeroPotionAttrUseCount < JobLevel.LevelMaster.PotionAttrMaxCount)
			{
				return true;
			}
			else
			{
				continue;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------

	#endregion Gear

	#region Inventory

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot GetInventorySlot(int nIndex)
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].Index == nIndex)
				return m_listCsInventorySlot[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot GetInventorySlotByHeroMainGearId(Guid guidHeroGearId)
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.MainGear)
			{
				if (m_listCsInventorySlot[i].InventoryObjectMainGear.HeroMainGear.Id == guidHeroGearId)
					return m_listCsInventorySlot[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot GetInventorySlotBySubGearId(int nSubGearId)
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.SubGear)
			{
				if (m_listCsInventorySlot[i].InventoryObjectSubGear.HeroSubGear.SubGear.SubGearId == nSubGearId)
					return m_listCsInventorySlot[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void AddInventorySlots(PDInventorySlot[] inventorySlot, bool bClearAll = false, bool bEvent = true, bool bRemove = true)
	{
		if (inventorySlot != null)
		{
			if (bClearAll)
			{
				m_listCsInventorySlot.Clear();
			}

			List<CsHeroObject> listCsHeroObject = new List<CsHeroObject>();

			for (int i = 0; i < inventorySlot.Length; i++)
			{
				if (inventorySlot[i] == null) continue;

				CsInventorySlot csInventorySlot = GetInventorySlot(inventorySlot[i].index);
				// 새로운 슬롯 추가
				if (csInventorySlot == null)
				{
					AddInventorySlot(inventorySlot[i]);

					if (inventorySlot[i].inventoryObject.type == (int)EnInventoryObjectType.Item)
					{
						CsInventorySlot csInventorySlotNew = m_listCsInventorySlot[m_listCsInventorySlot.Count - 1];
						listCsHeroObject.Add(new CsHeroItem(csInventorySlotNew.InventoryObjectItem.Item, csInventorySlotNew.InventoryObjectItem.Owned, csInventorySlotNew.InventoryObjectItem.Count, csInventorySlotNew.Index));
					}
				}
				else
				{
					// 기존 슬롯 삭제
					RemoveInventorySlot(csInventorySlot, bRemove);
					// 삭제가 아니면
					if (inventorySlot[i].inventoryObject != null)
					{
						// 슬롯 추가
						AddInventorySlot(inventorySlot[i]);

						// 아이템일 경우.
						if (inventorySlot[i].inventoryObject.type == (int)EnInventoryObjectType.Item)
						{
							CsInventorySlot csInventorySlotNew = m_listCsInventorySlot[m_listCsInventorySlot.Count - 1];

							// 삭제된 슬롯이 아이템일 경우
							if (csInventorySlot.EnType == EnInventoryObjectType.Item)
							{
								// 아이템 아이디가 같을 경우
								if (csInventorySlot.InventoryObjectItem.Item.ItemId == csInventorySlotNew.InventoryObjectItem.Item.ItemId)
								{
									// 아이템 카운트가 늘어난 경우 (추가로 아이템이 더해짐)
									if (csInventorySlot.InventoryObjectItem.Count < csInventorySlotNew.InventoryObjectItem.Count)
									{
										listCsHeroObject.Add(new CsHeroItem(csInventorySlotNew.InventoryObjectItem.Item, csInventorySlotNew.InventoryObjectItem.Owned, csInventorySlotNew.InventoryObjectItem.Count, csInventorySlotNew.Index));
									}
								}
								else
								{
									listCsHeroObject.Add(new CsHeroItem(csInventorySlotNew.InventoryObjectItem.Item, csInventorySlotNew.InventoryObjectItem.Owned, csInventorySlotNew.InventoryObjectItem.Count, csInventorySlotNew.Index));
								}
							}
							else
							{
								listCsHeroObject.Add(new CsHeroItem(csInventorySlotNew.InventoryObjectItem.Item, csInventorySlotNew.InventoryObjectItem.Owned, csInventorySlotNew.InventoryObjectItem.Count, csInventorySlotNew.Index));
							}
						}
					}
				}
			}

			if (bEvent)
			{
				CsGameEventUIToUI.Instance.OnEventHeroObjectGot(listCsHeroObject);
			}

			SortInventorySlotList();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddInventorySlot(PDInventorySlot inventorySlot)
	{
		if (inventorySlot != null)
		{
			m_listCsInventorySlot.Add(new CsInventorySlot(inventorySlot));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddInventorySlot(CsInventorySlot csInventorySlot)
	{
		if (csInventorySlot != null)
		{
			m_listCsInventorySlot.Add(csInventorySlot);
			SortInventorySlotList();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveInventorySlot(int nIndex, bool bRemoveHeroGear = true)
	{
		CsInventorySlot csInventorySlot = m_listCsInventorySlot.Find(slot => slot.Index == nIndex);

		if (csInventorySlot != null)
		{
			RemoveInventorySlot(csInventorySlot, bRemoveHeroGear);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveInventorySlot(CsInventorySlot csInventorySlot, bool bRemoveHeroGear = true)
	{
		if (csInventorySlot.EnType == EnInventoryObjectType.MainGear && bRemoveHeroGear)
		{
			m_listCsHeroMainGear.Remove(csInventorySlot.InventoryObjectMainGear.HeroMainGear);
		}
		else if (csInventorySlot.EnType == EnInventoryObjectType.MountGear && bRemoveHeroGear)
		{
			m_listCsHeroMountGear.Remove(csInventorySlot.InventoryObjectMountGear.HeroMountGear);
		}
		// 인벤토리에서 삭제.
		m_listCsInventorySlot.Remove(csInventorySlot);
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveInventorySlot(Guid guidMainGear)
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.MainGear)
			{
				if (m_listCsInventorySlot[i].InventoryObjectMainGear.HeroMainGear.Id == guidMainGear)
				{
					m_listCsHeroMainGear.Remove(m_listCsInventorySlot[i].InventoryObjectMainGear.HeroMainGear);
					m_listCsInventorySlot.RemoveAt(i);
					return;
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SortInventorySlotList()
	{
		m_listCsInventorySlot.Sort((CsInventorySlot inventorySlot1, CsInventorySlot inventorySlot2) =>
		{
			int nComp = inventorySlot1.EnType.CompareTo(inventorySlot2.EnType);

			if (nComp == 0)
			{
				switch (inventorySlot1.EnType)
				{
					case EnInventoryObjectType.MainGear:
						// 타입(오름차순)
						nComp = inventorySlot1.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearType.EnMainGearType.CompareTo(inventorySlot2.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearType.EnMainGearType);

						if (nComp == 0)
						{
							// 티어(내림차순)
							nComp = inventorySlot1.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearTier.Tier.CompareTo(inventorySlot2.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearTier.Tier);

							if (nComp == 0)
							{
								// 등급(내림차순)
								return inventorySlot1.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearGrade.EnGrade.CompareTo(inventorySlot2.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearGrade.EnGrade) * (int)EnSortOrder.Descending;
							}
							else
							{
								return nComp * (int)EnSortOrder.Descending;
							}
						}
						else
						{
							return nComp;
						}

					case EnInventoryObjectType.SubGear:
						// 보조장비아이디(오름차순)
						return inventorySlot1.InventoryObjectSubGear.HeroSubGear.SubGear.SubGearId.CompareTo(inventorySlot2.InventoryObjectSubGear.HeroSubGear.SubGear.SubGearId);

					case EnInventoryObjectType.Item:
						// 타입(오름차순)
						nComp = inventorySlot1.InventoryObjectItem.Item.ItemType.EnItemType.CompareTo(inventorySlot2.InventoryObjectItem.Item.ItemType.EnItemType);

						if (nComp == 0)
						{
							// 사용가능최소레벨(내림차순)
							nComp = inventorySlot1.InventoryObjectItem.Item.RequiredMinHeroLevel.CompareTo(inventorySlot2.InventoryObjectItem.Item.RequiredMinHeroLevel);

							if (nComp == 0)
							{
								// 등급(내림차순)
								return inventorySlot1.InventoryObjectItem.Item.Grade.CompareTo(inventorySlot2.InventoryObjectItem.Item.Grade) * (int)EnSortOrder.Descending;
							}
							else
							{
								return nComp * (int)EnSortOrder.Descending;
							}
						}
						else
						{
							return nComp;
						}

					default:
						return 0;
				}
			}
			else
			{
				return nComp;
			}

		});
	}

	//---------------------------------------------------------------------------------------------------
	public int GetItemCount(int nItemId)
	{
		int nCount = 0;

		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.Item)
			{
				if (m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemId == nItemId)
					nCount += m_listCsInventorySlot[i].InventoryObjectItem.Count;
			}
		}

		return nCount;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetItemCount(int nItemId, bool bOwned)
	{
		int nCount = 0;

		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.Item)
			{
				if (m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemId == nItemId && m_listCsInventorySlot[i].InventoryObjectItem.Owned == bOwned)
					nCount += m_listCsInventorySlot[i].InventoryObjectItem.Count;
			}
		}

		return nCount;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetRemainingItemCount(int nItemId, bool bOwned)
	{
		int nCount = 0;

		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			// 아이템
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.Item)
			{
				// 아이템아이디와 아이템귀속여부가 일치
				if (m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemId == nItemId && m_listCsInventorySlot[i].InventoryObjectItem.Owned == bOwned)
				{
					// 아이템이 추가될 수 있을 경우.
					if (m_listCsInventorySlot[i].InventoryObjectItem.Count < m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemType.MaxCountPerInventorySlot)
					{
						nCount += m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemType.MaxCountPerInventorySlot - m_listCsInventorySlot[i].InventoryObjectItem.Count;
					}
				}
			}
		}

		return nCount;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetItemCountByItemType(int nItemType)
	{
		int nCount = 0;

		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.Item)
			{
				if (m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemType.ItemType == nItemType)
					nCount += m_listCsInventorySlot[i].InventoryObjectItem.Count;
			}
		}

		return nCount;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot GetInventorySlotByItemId(int nItemId)
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.Item)
			{
				if (m_listCsInventorySlot[i].InventoryObjectItem.Item.ItemId == nItemId)
					return m_listCsInventorySlot[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	// 인벤토리 아이템 추가 가능여부 검사
	public bool CheckAddItemAvailable(IEnumerable<CsItemReward> enumerableCsItemReward)
	{
		int nCurrentInventorySlotCount = m_listCsInventorySlot.Count;
		int nMaxInventorySlotCount = m_nPaidInventorySlotCount + m_csJobLevel.InventorySlotAccCount;

		int nCount = 0;

		foreach (var csItemReward in enumerableCsItemReward)
		{
			if (csItemReward.ItemCount <= GetRemainingItemCount(csItemReward.Item.ItemId, csItemReward.ItemOwned))
			{
				continue;
			}

			if (nCurrentInventorySlotCount + nCount < nMaxInventorySlotCount)
			{
				nCount++;
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	//---------------------------------------------------------------------------------------------------

	#endregion Inventory

	#region Warehouse

	//---------------------------------------------------------------------------------------------------
	public void AddWarehouseSlots(PDWarehouseSlot[] warehouseSlot, bool bClearAll = false)
	{
		if (warehouseSlot != null)
		{
			if (bClearAll)
			{
				m_listCsWarehouseSlot.Clear();
			}

			for (int i = 0; i < warehouseSlot.Length; i++)
			{
				CsWarehouseSlot csWarehouseSlot = GetWarehouseSlot(warehouseSlot[i].index);

				// 새로운 슬롯 추가
				if (csWarehouseSlot == null)
				{
					AddWarehouseSlot(warehouseSlot[i]);
				}
				else
				{
					// 기존 슬롯 삭제
					RemoveWarehouseSlot(csWarehouseSlot);

					// 삭제가 아니면
					if (warehouseSlot[i].warehouseObject != null)
					{
						// 슬롯 추가
						AddWarehouseSlot(warehouseSlot[i]);
					}
				}
			}

			SortWarehouseSlotList();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddWarehouseSlot(PDWarehouseSlot warehouseSlot)
	{
		if (warehouseSlot != null)
		{
			m_listCsWarehouseSlot.Add(new CsWarehouseSlot(warehouseSlot));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddWarehouseSlot(CsWarehouseSlot csWarehouseSlot)
	{
		if (csWarehouseSlot != null)
		{
			m_listCsWarehouseSlot.Add(csWarehouseSlot);
			SortWarehouseSlotList();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveWarehouseSlot(CsWarehouseSlot csWarehouseSlot)
	{
		// 인벤토리에서 삭제.
		m_listCsWarehouseSlot.Remove(csWarehouseSlot);
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseSlot GetWarehouseSlot(int nIndex)
	{
		for (int i = 0; i < m_listCsWarehouseSlot.Count; i++)
		{
			if (m_listCsWarehouseSlot[i].Index == nIndex)
				return m_listCsWarehouseSlot[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void SortWarehouseSlotList()
	{
		m_listCsWarehouseSlot.Sort((CsWarehouseSlot warehouseSlot1, CsWarehouseSlot warehouseSlot2) =>
		{
			int nComp = warehouseSlot1.EnType.CompareTo(warehouseSlot2.EnType);

			if (nComp == 0)
			{
				switch (warehouseSlot1.EnType)
				{
					case EnWarehouseObjectType.MainGear:
						// 타입(오름차순)
						nComp = warehouseSlot1.WarehouseObjectMainGear.HeroMainGear.MainGear.MainGearType.EnMainGearType.CompareTo(warehouseSlot2.WarehouseObjectMainGear.HeroMainGear.MainGear.MainGearType.EnMainGearType);

						if (nComp == 0)
						{
							// 티어(내림차순)
							nComp = warehouseSlot1.WarehouseObjectMainGear.HeroMainGear.MainGear.MainGearTier.Tier.CompareTo(warehouseSlot2.WarehouseObjectMainGear.HeroMainGear.MainGear.MainGearTier.Tier);

							if (nComp == 0)
							{
								// 등급(내림차순)
								return warehouseSlot1.WarehouseObjectMainGear.HeroMainGear.MainGear.MainGearGrade.EnGrade.CompareTo(warehouseSlot2.WarehouseObjectMainGear.HeroMainGear.MainGear.MainGearGrade.EnGrade) * (int)EnSortOrder.Descending;
							}
							else
							{
								return nComp * (int)EnSortOrder.Descending;
							}
						}
						else
						{
							return nComp;
						}

					case EnWarehouseObjectType.SubGear:
						// 보조장비아이디(오름차순)
						return warehouseSlot1.WarehouseObjectSubGear.HeroSubGear.SubGear.SubGearId.CompareTo(warehouseSlot2.WarehouseObjectSubGear.HeroSubGear.SubGear.SubGearId);

					case EnWarehouseObjectType.Item:
						// 타입(오름차순)
						nComp = warehouseSlot1.WarehouseObjectItem.Item.ItemType.EnItemType.CompareTo(warehouseSlot2.WarehouseObjectItem.Item.ItemType.EnItemType);

						if (nComp == 0)
						{
							// 사용가능최소레벨(내림차순)
							nComp = warehouseSlot1.WarehouseObjectItem.Item.RequiredMinHeroLevel.CompareTo(warehouseSlot2.WarehouseObjectItem.Item.RequiredMinHeroLevel);

							if (nComp == 0)
							{
								// 등급(내림차순)
								return warehouseSlot1.WarehouseObjectItem.Item.Grade.CompareTo(warehouseSlot2.WarehouseObjectItem.Item.Grade) * (int)EnSortOrder.Descending;
							}
							else
							{
								return nComp * (int)EnSortOrder.Descending;
							}
						}
						else
						{
							return nComp;
						}

					default:
						return 0;
				}
			}
			else
			{
				return nComp;
			}

		});
	}

	#endregion Warehouse

	#region Skill

	//---------------------------------------------------------------------------------------------------
	public void AddHeroSkills(PDHeroSkill[] heroSkills)
	{
		for (int i = 0; i < heroSkills.Length; i++)
		{
			m_listCsHeroSkill.Add(new CsHeroSkill(Job.JobId, heroSkills[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSkill GetHeroSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsHeroSkill.Count; i++)
		{
			if (m_listCsHeroSkill[i].JobSkill.SkillId == nSkillId)
				return m_listCsHeroSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckNoticeSkill()
	{
		for (int i = 0; i < m_listCsHeroSkill.Count; i++)
		{
			if (m_listCsHeroSkill[i].IsLevelUp)
				return true;
		}

		return false;
	}

	#endregion Skill

	#region Mail

	//---------------------------------------------------------------------------------------------------
	public void AddMails(PDMail[] mails)
	{
		for (int i = 0; i < mails.Length; i++)
		{
			AddMail(mails[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddMail(PDMail mail)
	{
		m_listCsMail.Insert(0, new CsMail(mail));
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveMails(Guid[] guidMails)
	{
		for (int i = 0; i < guidMails.Length; i++)
		{
			RemoveMail(guidMails[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsMail GetMail(Guid guidMail)
	{
		for (int i = 0; i < m_listCsMail.Count; i++)
		{
			if (m_listCsMail[i].Id == guidMail)
				return m_listCsMail[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveMail(Guid guidMail)
	{
		for (int i = 0; i < m_listCsMail.Count; i++)
		{
			if (m_listCsMail[i].Id == guidMail)
			{
				m_listCsMail.RemoveAt(i);
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckNoticeMail()
	{
		/*
		if (m_listCsMail.Count == 0)
			return false;
		else
			return true;
        */
		for (int i = 0; i < m_listCsMail.Count; i++)
		{
			if (0 < m_listCsMail[i].MailAttachmentList.Count && !m_listCsMail[i].Received)
			{
				return true;
			}
		}

		return false;
	}

	#endregion Mail

	#region Party

	//---------------------------------------------------------------------------------------------------
	public void AddPartyApplication(CsPartyApplication csPartyApplication)
	{
		m_listCsPartyApplication.Add(csPartyApplication);
	}

	//---------------------------------------------------------------------------------------------------
	public CsPartyApplication GetPartyApplication(long lNo)
	{
		for (int i = 0; i < m_listCsPartyApplication.Count; i++)
		{
			if (m_listCsPartyApplication[i].No == lNo)
				return m_listCsPartyApplication[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemovePartyApplication(long lNo)
	{
		for (int i = 0; i < m_listCsPartyApplication.Count; i++)
		{
			if (m_listCsPartyApplication[i].No == lNo)
			{
				m_listCsPartyApplication.RemoveAt(i);
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddPartyInvitation(CsPartyInvitation csPartyInvitation)
	{
		m_listCsPartyInvitation.Add(csPartyInvitation);
	}

	//---------------------------------------------------------------------------------------------------
	public CsPartyInvitation GetPartyInvitation(long lNo)
	{
		for (int i = 0; i < m_listCsPartyInvitation.Count; i++)
		{
			if (m_listCsPartyInvitation[i].No == lNo)
				return m_listCsPartyInvitation[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemovePartyInvitation(long lNo)
	{
		for (int i = 0; i < m_listCsPartyInvitation.Count; i++)
		{
			if (m_listCsPartyInvitation[i].No == lNo)
			{
				m_listCsPartyInvitation.RemoveAt(i);
				return;
			}
		}
	}

	#endregion Party

	#region Mount

	//---------------------------------------------------------------------------------------------------
	public void AddHeroMounts(PDHeroMount[] mounts, bool bClearAll = false)
	{
		if (mounts != null)
		{
			if (bClearAll)
			{
				m_listCsHeroMount.Clear();
			}

			for (int i = 0; i < mounts.Length; i++)
			{
				m_listCsHeroMount.Add(new CsHeroMount(mounts[i]));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMount GetHeroMount(int nMoundId)
	{
		for (int i = 0; i < m_listCsHeroMount.Count; i++)
		{
			if (m_listCsHeroMount[i].Mount.MountId == nMoundId)
				return m_listCsHeroMount[i];
		}

		return null;
	}

    //---------------------------------------------------------------------------------------------------
	public bool CheckMountLevelUp()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MountLevelUp))
			return false;

		if (m_nEquippedMountId > 0)
		{
			int nCount = GetItemCount(CsGameConfig.Instance.MountLevelUpItemId);

			if (nCount > 0)
			{
				for (int i = 0; i < m_listCsHeroMount.Count; i++)
				{
					if (m_listCsHeroMount[i].Level < CsGameData.Instance.MountLevelMasterList[CsGameData.Instance.MountLevelMasterList.Count - 1].Level)
						return true;
				}
			}

            return false;
		}
		else
		{
			return false;
		}
	}

    //---------------------------------------------------------------------------------------------------
    public bool CheckMountAwakeningLevelUp()
    {
        // 탈것 각성
        int nCount = GetItemCount(CsGameConfig.Instance.MountAwakeningItemId);

        if (nCount > 0 && CsGameConfig.Instance.MountAwakeningRequiredHeroLevel <= Level)
        {
            for (int i = 0; i < m_listCsHeroMount.Count; i++)
            {
                if (m_listCsHeroMount[i].AwakeningLevel < CsGameData.Instance.MountAwakeningLevelMasterList[CsGameData.Instance.MountAwakeningLevelMasterList.Count - 1].AwakeningLevel)
                    return true;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------------------------------------
    public bool CheckMountPotionAttr()
    {
        // 탈것 속성 물약
        int nCount = GetItemCount(CsGameConfig.Instance.MountPotionAttrItemId);

        if (nCount > 0)
        {
            CsMountQuality csMountQuality = null;

            for (int i = 0; i < m_listCsHeroMount.Count; i++)
            {
                csMountQuality = m_listCsHeroMount[i].Mount.GetMountQuality(m_listCsHeroMount[i].MountLevel.MountLevelMaster.MountQualityMaster.Quality);

                if (csMountQuality != null && m_listCsHeroMount[i].PotionAttrCount < csMountQuality.PotionAttrMaxCount)
                {
                    return true;
                }
            }
        }

        return false;
    }

	#endregion Mount

	#region MountGear

	//---------------------------------------------------------------------------------------------------
	public void AddHeroMountGears(PDHeroMountGear[] mountGears, bool bClearAll = false)
	{
		if (mountGears != null)
		{
			if (bClearAll)
			{
				m_listCsHeroMountGear.Clear();
			}

			for (int i = 0; i < mountGears.Length; i++)
			{
				m_listCsHeroMountGear.Add(new CsHeroMountGear(mountGears[i]));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddEquippedMountGears(Guid[] equippedMountGears, bool bClearAll = false)
	{
		if (equippedMountGears != null)
		{
			if (bClearAll)
			{
				m_listEquippedMountGear.Clear();
			}

			for (int i = 0; i < equippedMountGears.Length; i++)
			{
				AddEquippedMountGear(equippedMountGears[i]);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddEquippedMountGear(Guid guidHeroMountGearId)
	{
		m_listEquippedMountGear.Add(guidHeroMountGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMountGear GetHeroMountGear(Guid guid)
	{
		for (int i = 0; i < m_listCsHeroMountGear.Count; i++)
		{
			if (m_listCsHeroMountGear[i].Id == guid)
				return m_listCsHeroMountGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot GetInventorySlotByHeroMountGearId(Guid guidHeroMountId)
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].EnType == EnInventoryObjectType.MountGear)
			{
				if (m_listCsInventorySlot[i].InventoryObjectMountGear.HeroMountGear.Id == guidHeroMountId)
					return m_listCsInventorySlot[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMountGear GetEquippedMountGearBySlotIndex(int nSlotIndex)
	{
		for (int i = 0; i < m_listEquippedMountGear.Count; i++)
		{
			CsHeroMountGear csHeroMountGear = GetHeroMountGear(m_listEquippedMountGear[i]);

			if (csHeroMountGear.MountGear.MountGearType.SlotIndex == nSlotIndex)
				return csHeroMountGear;
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveEquippedMountGear(Guid guidHeroMountGearId)
	{
		for (int i = 0; i < m_listEquippedMountGear.Count; i++)
		{
			if (m_listEquippedMountGear[i] == guidHeroMountGearId)
			{
				m_listEquippedMountGear.RemoveAt(i);
				return;
			}
		}
	}


	#endregion MountGear

	#region Wing

	//---------------------------------------------------------------------------------------------------
	public CsHeroWingPart GetHeroWingPart(int nPartId)
	{
		for (int i = 0; i < m_listCsHeroWingPart.Count; i++)
		{
			if (m_listCsHeroWingPart[i].PartId == nPartId)
				return m_listCsHeroWingPart[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckWingEnchant()
	{
		if (m_listCsHeroWing.Count > 0)
		{
			CsWingStep csWingStep = CsGameData.Instance.GetWingStep(m_nWingStep);

			if (csWingStep != null)
			{
				int nCount = GetItemCount(CsGameConfig.Instance.WingEnchantItemId);

				if (nCount >= csWingStep.EnchantMaterialItemCount)
				{
					if (m_nWingLevel >= csWingStep.WingStepLevelList[csWingStep.WingStepLevelList.Count - 1].Level)
					{
						return false;
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckWingInstall()
	{
		for (int i = 0; i < m_listCsHeroWing.Count; i++)
		{
			CsHeroWing csHeroWing = m_listCsHeroWing[i];

			// 영석 장착 가능 여부
			if (csHeroWing.Wing.MemoryPieceInstallationEnabled)
			{
				CsWing csWing = CsGameData.Instance.GetWing(csHeroWing.Wing.WingId);

				if (csWing == null)
				{
					continue;
				}
				else
				{
					bool bAllSlotInstall = true;

					for (int j = 0; j < csHeroWing.HeroWingMemoryPieceSlotList.Count; j++)
					{
						if (csHeroWing.HeroWingMemoryPieceSlotList[j].AccAttrValue < csWing.GetWingMemoryPieceSlot(csHeroWing.HeroWingMemoryPieceSlotList[j].Index).GetWingMemoryPieceSlotStep(csHeroWing.MemoryPieceStep).AttrMaxValue)
						{
							bAllSlotInstall = false;
							break;
						}
						else
						{
							continue;
						}
					}

					if (bAllSlotInstall)
					{
						// 해당 날개에 영석 모두 장착
						continue;
					}
					else
					{
						CsWingMemoryPieceStep csWingMemoryPieceStep = csWing.WingMemoryPieceStepList.Find(a => a.Step == csHeroWing.MemoryPieceStep);

						if (csWingMemoryPieceStep == null)
						{
							continue;
						}
						else
						{
							for (int j = 0; j < CsGameData.Instance.WingMemoryPieceTypeList.Count; j++)
							{
								int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.WingMemoryPieceTypeList[j].RequiredItem.ItemId);

								if (csWingMemoryPieceStep.RequiredMemoryPieceCount <= nItemCount)
								{
									return true;
								}
								else
								{
									continue;
								}
							}
						}
					}
				}
			}
			else
			{
				continue;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void AddHeroWing(PDHeroWing heroWing)
	{
		m_listCsHeroWing.Add(new CsHeroWing(heroWing));
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateHeroWing(int nWingId, int nMemoryPieceStep, PDHeroWingMemoryPieceSlot[] changedWingMemoryPieceSlots)
	{
		CsHeroWing csHeroWing = GetHeroWing(nWingId);

		if (csHeroWing != null)
		{
			csHeroWing.MemoryPieceStep = nMemoryPieceStep;

			for (int i = 0; i < changedWingMemoryPieceSlots.Length; i++)
			{
				CsHeroWingMemoryPieceSlot csHeroWingMemoryPieceSlot = csHeroWing.GetHeroWingMemoryPieceSlot(changedWingMemoryPieceSlots[i].index);

				if (csHeroWingMemoryPieceSlot != null)
				{
					csHeroWingMemoryPieceSlot.AccAttrValue = changedWingMemoryPieceSlots[i].accAttrValue;
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroWing GetHeroWing(int nWingId)
	{
		for (int i = 0; i < m_listCsHeroWing.Count; i++)
		{
			if (m_listCsHeroWing[i].Wing.WingId == nWingId)
				return m_listCsHeroWing[i];
		}

		return null;
	}

	#endregion Wing

	#region SeriesMission

	//---------------------------------------------------------------------------------------------------
	public CsHeroSeriesMission GetHeroSeriesMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsHeroSeriesMission.Count; i++)
		{
			if (m_listCsHeroSeriesMission[i].SeriesMission.MissionId == nMissionId)
				return m_listCsHeroSeriesMission[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveHeroSeriesMission(CsHeroSeriesMission csHeroSeriesMission)
	{
		m_listCsHeroSeriesMission.Remove(csHeroSeriesMission);
	}

	#endregion SeriesMission

	#region TodayMission

	public CsHeroTodayMission GetHeroTodayMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsHeroTodayMission.Count; i++)
		{
			if (m_listCsHeroTodayMission[i].TodayMission.MissionId == nMissionId)
				return m_listCsHeroTodayMission[i];
		}

		return null;
	}

	public void AddHeroTodayMissions(PDHeroTodayMission[] missions, DateTime dtDate)
	{
		m_listCsHeroTodayMission.Clear();

		if (missions != null)
		{
			for (int i = 0; i < missions.Length; i++)
			{
				m_listCsHeroTodayMission.Add(new CsHeroTodayMission(missions[i], dtDate));
			}
		}
	}

	#endregion TodayMission

	#region TodayTask

	public CsHeroTodayTask GetHeroTodayTask(int nTaskId)
	{
		for (int i = 0; i < m_listCsHeroTodayTask.Count; i++)
		{
			if (m_listCsHeroTodayTask[i].TodayTask.TaskId == nTaskId)
				return m_listCsHeroTodayTask[i];
		}

		return null;
	}

	public void AddHeroTodayTask(int nTaskId, int nProgressCount, DateTime dtDate)
	{
		m_listCsHeroTodayTask.Add(new CsHeroTodayTask(nTaskId, nProgressCount, dtDate));
	}

	public bool CheckTodayTask()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.TodayTask))
			return false;

		CsAchievementReward csAchievementReward = CsGameData.Instance.GetAchievementReward(m_nReceivedAchievementRewardNo + 1);

		if (csAchievementReward != null)
		{
			if (m_nAchievementDailyPoint >= csAchievementReward.RequiredAchievementPoint)
			{
				return true;
			}
		}

		return false;
	}

	#endregion TodayTask

	#region NationNoblesse

	public void AddNationNoblesse(int nNationId, int nNoblesseId, Guid guidHeroId, string strHeroName, int nJobId, DateTime dtAppointmentDate)
	{
		CsNationInstance csNationInstance = GetNationInstance(nNationId);

		CsNationNoblesseInstance csNationNoblesseInstance = csNationInstance.GetNationNoblesseInstance(nNoblesseId);
		csNationNoblesseInstance.HeroId = guidHeroId;
		csNationNoblesseInstance.HeroName = strHeroName;
		csNationNoblesseInstance.JobId = nJobId;
		csNationNoblesseInstance.AppointmentDate = dtAppointmentDate;
	}

	public void RemoveNationNoblesse(int nNationId, int nNoblesseId)
	{
		CsNationInstance csNationInstance = GetNationInstance(nNationId);

		CsNationNoblesseInstance csNationNoblesseInstance = csNationInstance.GetNationNoblesseInstance(nNoblesseId);
		csNationNoblesseInstance.HeroId = Guid.Empty;
		csNationNoblesseInstance.HeroName = string.Empty;
	}

	public CsNationNoblesseInstance GetNationNoblesseInstance(int nNoblesseId)
	{
		for (int i = 0; i < m_listCsNationNoblesseInstance.Count; i++)
		{
			if (m_listCsNationNoblesseInstance[i].NoblesseId == nNoblesseId)
				return m_listCsNationNoblesseInstance[i];
		}

		return null;
	}

	public CsNationNoblesseInstance GetMyNationNoblesseInstance(Guid guidHeroId)
	{
		for (int i = 0; i < m_listCsNationNoblesseInstance.Count; i++)
		{
			if (m_listCsNationNoblesseInstance[i].HeroId == guidHeroId)
				return m_listCsNationNoblesseInstance[i];
		}

		return null;
	}

	public CsNationNoblesseInstance GetNationNoblesseInstanceByHeroId(Guid guidHeroId)
	{
		for (int i = 0; i < m_listCsNationNoblesseInstance.Count; i++)
		{
			if (m_listCsNationNoblesseInstance[i].HeroId == guidHeroId)
				return m_listCsNationNoblesseInstance[i];
		}

		return null;
	}

	#endregion NationNoblesse

	#region Attainment

	public bool CheckAttainment()
	{
		if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Attaniment))
			return false;

		CsAttainmentEntry csAttainmentEntry = CsGameData.Instance.GetAttainmentEntry(m_nRewardedAttainmentEntryNo + 1);

		if (csAttainmentEntry != null)
		{
			// 레벨 조건
			if (csAttainmentEntry.Type == 1)
			{
				if (Level >= csAttainmentEntry.RequiredHeroLevel)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			//퀘스트 조건
			else if (csAttainmentEntry.Type == 2)
			{
				if (CsMainQuestManager.Instance.MainQuest != null && CsMainQuestManager.Instance.MainQuest.MainQuestNo >= csAttainmentEntry.RequiredMainQuestNo)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	#endregion Attainment

	#region Rank

	public bool CheckRankReward()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.Rank))
			return false;

		if (RankNo > 0)
		{
			// 보상조건
			if (m_dtRankRewardReceivedDate.Date.CompareTo(CurrentDateTime.Date) != 0 && m_nRankRewardReceivedRankNo != RankNo)
			{
				return true;
			}

			// 승급조건
			if (RankNo < CsGameData.Instance.RankList[CsGameData.Instance.RankList.Count - 1].RankNo)
			{
				CsRank csRank = CsGameData.Instance.GetRank(RankNo + 1);

				if (csRank != null)
				{
					if (m_nExploitPoint >= csRank.RequiredExploitPoint)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	public void UpdateRankSkill(int nRankNo)
	{
		for (int i = 0; i < CsGameData.Instance.RankActiveSkillList.Count; i++)
		{
			if (CsGameData.Instance.RankActiveSkillList[i].RequiredRankNo == nRankNo)
			{
				m_listCsHeroRankActiveSkill.Add(new CsHeroRankActiveSkill(CsGameData.Instance.RankActiveSkillList[i].SkillId, 1));

				// 처음으로 스킬을 획득한 경우
				if (m_listCsHeroRankActiveSkill.Count == 1)
				{
					m_nSelectedRankActiveSkillId = CsGameData.Instance.RankActiveSkillList[i].SkillId;
					CsGameEventUIToUI.Instance.OnEventRankActiveSkillSelect();
				}
			}
		}

		for (int i = 0; i < CsGameData.Instance.RankPassiveSkillList.Count; i++)
		{
			if (CsGameData.Instance.RankPassiveSkillList[i].RequiredRankNo == nRankNo)
			{
				m_listCsHeroRankPassiveSkill.Add(new CsHeroRankPassiveSkill(CsGameData.Instance.RankPassiveSkillList[i].SkillId, 1));
			}
		}
	}

	public CsHeroRankActiveSkill GetHeroRankActiveSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsHeroRankActiveSkill.Count; i++)
		{
			if (m_listCsHeroRankActiveSkill[i].SkillId == nSkillId)
				return m_listCsHeroRankActiveSkill[i];
		}

		return null;
	}

	public CsHeroRankPassiveSkill GetHeroRankPassiveSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsHeroRankPassiveSkill.Count; i++)
		{
			if (m_listCsHeroRankPassiveSkill[i].SkillId == nSkillId)
				return m_listCsHeroRankPassiveSkill[i];
		}

		return null;
	}

	public bool CheckRankSkillLevelUp()
	{
		for (int i = 0; i < m_listCsHeroRankActiveSkill.Count; i++)
		{
			// 최대레벨이 아니고 보유재화(골드, 아이템)을 만족할 경우
			CsRankActiveSkill csRankActiveSkill = CsGameData.Instance.GetRankActiveSkill(m_listCsHeroRankActiveSkill[i].SkillId);

			if (csRankActiveSkill.RankActiveSkillLevelList[csRankActiveSkill.RankActiveSkillLevelList.Count - 1].Level > m_listCsHeroRankActiveSkill[i].Level)
			{
				CsRankActiveSkillLevel csRankActiveSkillLevel = csRankActiveSkill.GetRankActiveSkillLevel(m_listCsHeroRankActiveSkill[i].Level);

				if (csRankActiveSkillLevel.NextLevelUpRequiredGold <= CsGameData.Instance.MyHeroInfo.Gold)
				{
					int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csRankActiveSkillLevel.NextLevelUpRequiredItem.ItemId);

					if (nItemCount >= csRankActiveSkillLevel.NextLevelUpRequiredItemCount)
					{
						return true;
					}
				}
			}
		}

		for (int i = 0; i < m_listCsHeroRankPassiveSkill.Count; i++)
		{
			// 최대레벨이 아니고 보유재화(골드, 스피릿스톤)을 만족할 경우
			if (m_listCsHeroRankPassiveSkill[i].RankPassiveSkill.RankPassiveSkillLevelList[m_listCsHeroRankPassiveSkill[i].RankPassiveSkill.RankPassiveSkillLevelList.Count - 1].Level > m_listCsHeroRankPassiveSkill[i].Level)
			{
				CsRankPassiveSkillLevel csRankPassiveSkillLevel = m_listCsHeroRankPassiveSkill[i].RankPassiveSkill.GetRankPassiveSkillLevel(m_listCsHeroRankPassiveSkill[i].Level);

				if (csRankPassiveSkillLevel.NextLevelUpRequiredGold <= CsGameData.Instance.MyHeroInfo.Gold && csRankPassiveSkillLevel.NextLevelUpRequiredSpiritStone <= CsGameData.Instance.MyHeroInfo.SpiritStone)
				{
					return true;
				}
			}
		}

		return false;
	}

	#endregion Rank

	#region Ranking

	public bool CheckRanking()
	{
		if (m_nDailyServerLevelRanking != 0 && m_nDailyServerLevelRakingNo != m_nRewardedDailyServerLevelRankingNo)
		{
			if (CsGameData.Instance.LevelRankingRewardList[CsGameData.Instance.LevelRankingRewardList.Count - 1].LowRanking >= m_nDailyServerLevelRanking)
			{
				return true;
			}
		}

		return false;
	}

	#endregion Ranking

	#region Support

	public bool CheckSupport()
	{
		if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Support))
			return false;

		if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.LevelUpReward))
		{
			// 레벨업 보상 중 1개 이상의 보상을 획득할 수 있거나
			if (Level >= CsGameData.Instance.LevelUpRewardEntryList[0].Level)
			{
				List<CsLevelUpRewardEntry> list = CsGameData.Instance.LevelUpRewardEntryList.FindAll(a => a.Level <= Level);

				for (int i = 0; i < m_listReceivedLevelUpRewardList.Count; i++)
				{
					list.RemoveAll(a => a.EntryId == m_listReceivedLevelUpRewardList[i]);
				}

				if (list.Count > 0)
				{
					return true;
				}
			}
		}

		if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AttendReward))
		{
			// 출석 보상 중 1개 이상의 보상을 획득할 수 있거나
			if (m_dtDateReceivedAttendReward.Date.CompareTo(CurrentDateTime.Date) != 0)
			{
				return true;
			}
		}

		if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.TodayMission))
		{
			// 오늘의 미션 보상 중 1개 이상의 보상을 획득할 수 있거나
			for (int i = 0; i < m_listCsHeroTodayMission.Count; i++)
			{
				if (!m_listCsHeroTodayMission[i].RewardReceived && m_listCsHeroTodayMission[i].TodayMission.TargetCount <= m_listCsHeroTodayMission[i].ProgressCount)
				{
					return true;
				}
			}
		}

		if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SeriesMisson))
		{
			// 연속 미션 보상 중 1개 이상의 보상을 획득할 수 있거나
			for (int i = 0; i < m_listCsHeroSeriesMission.Count; i++)
			{
				if (m_listCsHeroSeriesMission[i].SeriesMissionStep.TargetCount <= m_listCsHeroSeriesMission[i].ProgressCount)
				{
					return true;
				}
			}
		}

		if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AccessReward))
		{
			// 접속보상
			List<CsAccessRewardEntry> listAccessRewardEntry = CsGameData.Instance.AccessRewardEntryList.FindAll(a => a.AccessTime <= m_flDailyAccessTime + Time.realtimeSinceStartup);

			for (int i = 0; i < m_listReceivedDailyAccessRewardList.Count; i++)
			{
				listAccessRewardEntry.RemoveAll(a => a.EntryId == m_listReceivedDailyAccessRewardList[i]);
			}

			if (listAccessRewardEntry.Count > 0)
			{
				return true;
			}
		}

		if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.LimitationGift))
		{
			int nMyHeroDayOfWeek = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek;
			int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

			if (0 <= CsGameData.Instance.LimitationGift.LimitationGiftRewardDayOfWeekList.FindIndex(a => a.DayOfWeek == nMyHeroDayOfWeek))
			{
				for (int i = 0; i < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Count; i++)
				{
					CsLimitationGiftRewardSchedule csLimitationGiftRewardSchedule = CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i];

					if (CsGameData.Instance.MyHeroInfo.RewardedLimitationGiftScheduleIdList.FindIndex(a => a == csLimitationGiftRewardSchedule.ScheduleId) < 0 &&
						csLimitationGiftRewardSchedule.StartTime <= nSecond && nSecond < csLimitationGiftRewardSchedule.EndTime)
					{
						return true;
					}
				}
			}
		}

		if (CsGameData.Instance.WeekendReward.RequiredHeroLevel <= Level)
		{
			if (CurrentDateTime.DayOfWeek == DayOfWeek.Monday)
			{
				if ((HeroWeekendReward.Selection1 != -1 || HeroWeekendReward.Selection2 != -1 || HeroWeekendReward.Selection3 != -1) && !HeroWeekendReward.Rewarded)
				{
					return true;
				}
			}
			else if (CurrentDateTime.DayOfWeek == DayOfWeek.Friday && HeroWeekendReward.Selection1 == -1)
			{
				return true;
			}
			else if (CurrentDateTime.DayOfWeek == DayOfWeek.Saturday && HeroWeekendReward.Selection2 == -1)
			{
				return true;
			}
			else if (CurrentDateTime.DayOfWeek == DayOfWeek.Sunday && HeroWeekendReward.Selection3 == -1)
			{
				return true;
			}
		}

		return false;
	}

	public bool IsReceivedOpenGiftReward(int nDay)
	{
		for (int i = 0; i < m_listReceivedOpenGiftReward.Count; i++)
		{
			if (m_listReceivedOpenGiftReward[i] == nDay)
				return true;
		}

		return false;
	}


	#endregion Support

	#region Nation

	//---------------------------------------------------------------------------------------------------
	public bool CheckNation()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.NationDonation))
			return false;

		if (m_csVipLevel.NationDonationMaxCount > m_nDailyNationDonationCount)
		{
			for (int i = 0; i < CsGameData.Instance.NationDonationEntryList.Count; i++)
			{
				switch (CsGameData.Instance.NationDonationEntryList[i].MoneyType)
				{
					case 1:     // Gold
						if (CsGameData.Instance.MyHeroInfo.Gold >= CsGameData.Instance.NationDonationEntryList[i].MoneyAmount)
							return true;
						break;

					case 2:     // Dia
						if (CsGameData.Instance.MyHeroInfo.Dia >= CsGameData.Instance.NationDonationEntryList[i].MoneyAmount)
							return true;
						break;
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationInstance GetNationInstance(int nNationId)
	{
		for (int i = 0; i < m_listCsNationInstance.Count; i++)
		{
			if (m_listCsNationInstance[i].NationId == nNationId)
				return m_listCsNationInstance[i];
		}

		return null;
	}

	#endregion Nation

	#region Npc상점

	public void AddNpcShopProductBuyCount(int nProductId)
	{
		CsHeroNpcShopProduct csHeroNpcShopProduct = GetHeroNpcShopProduct(nProductId);

		if (csHeroNpcShopProduct != null)
		{
			csHeroNpcShopProduct.BuyCount++;
		}
		else
		{
			m_listCsHeroNpcShopProduct.Add(new CsHeroNpcShopProduct(nProductId, 1));
		}
	}

	public CsHeroNpcShopProduct GetHeroNpcShopProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsHeroNpcShopProduct.Count; i++)
		{
			if (m_listCsHeroNpcShopProduct[i].ProductId == nProductId)
			{
				return m_listCsHeroNpcShopProduct[i];
			}
		}

		return null;
	}


	#endregion Npc상점

	#region Gift

	public bool CheckOpenGift()
	{
		if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.OpenGiftRequiredHeroLevel)
			return false;

		if (!CsUIData.Instance.MenuOpen((int)EnMenuId.OpenGift))
			return false;

		int nDiff = (int)CurrentDateTime.Date.Subtract(m_dtRegDate).TotalDays + 1;
		int nLastDay = CsGameData.Instance.OpenGiftRewardList[CsGameData.Instance.OpenGiftRewardList.Count - 1].Day;

		if (ReceivedOpenGiftRewardList.Count < nLastDay)
		{
			for (int i = 0; i < Math.Min(nDiff, nLastDay); i++)
			{
				if (!IsReceivedOpenGiftReward(i + 1))
				{ 
					return true;
				}
			}
		}

		return false;
	}

	public bool CheckRookieGift()
	{
		if (m_nRookieGiftNo > 0 && m_nRookieGiftNo <= CsGameData.Instance.RookieGiftList[CsGameData.Instance.RookieGiftList.Count - 1].GiftNo)
		{
			if (m_flRookieGiftRemainingTime - Time.realtimeSinceStartup <= 0)
			{
				return true;
			}
		}

		return false;
	}

	#endregion Gift

	#region Open7DayEvent

	public CsHeroOpen7DayEventProgressCount GetHeroOpen7DayEventProgressCount(int nType)
	{
		for (int i = 0; i < m_listCsHeroOpen7DayEventProgressCount.Count; i++)
		{
			if (m_listCsHeroOpen7DayEventProgressCount[i].Type == nType)
				return m_listCsHeroOpen7DayEventProgressCount[i];
		}

		return null;
	}

    public bool CheckOpen7DayEvent()
    {
        if (CsUIData.Instance.MenuOpen((int)EnMenuId.Open7Day) && CsMainQuestManager.Instance.MainQuest != null && CsGameConfig.Instance.Open7DayEventRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
        {
            TimeSpan tsRegTime = CurrentDateTime.Date - RegDate.Date;

            for (int i = 0; i < CsGameData.Instance.Open7DayEventDayList.Count; i++)
            {
                if (CsGameData.Instance.Open7DayEventDayList[i].Day <= (tsRegTime.TotalDays + 1))
                {
                    for (int j = 0; j < CsGameData.Instance.Open7DayEventDayList[i].Open7DayEventMissionList.Count; j++)
                    {
                        CsOpen7DayEventMission csOpen7DayEventMission = CsGameData.Instance.Open7DayEventDayList[i].Open7DayEventMissionList[j];

                        if (m_listRewardedOpen7DayEventMission.FindIndex(a => a == csOpen7DayEventMission.MissionId) == -1 && csOpen7DayEventMission.TargetValue <= GetOpen7DayMissionProgressCount(csOpen7DayEventMission))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            return false;
        }
        else
        {
            return false;
        }
    }

    long GetOpen7DayMissionProgressCount(CsOpen7DayEventMission csOpen7DayEventMission)
    {
        long lProgressCount = 0;
        EnOpen7DayMissionType enOpen7DayMissionType = (EnOpen7DayMissionType)csOpen7DayEventMission.Type;
        CsHeroOpen7DayEventProgressCount csHeroOpen7DayEventProgressCount = GetHeroOpen7DayEventProgressCount(csOpen7DayEventMission.Type);

        if (enOpen7DayMissionType == EnOpen7DayMissionType.Level)
        {
            lProgressCount = Level;
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.SubGearLevel)
        {
            for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
            {
                if (m_listCsHeroSubGear[i] == null)
                {
                    continue;
                }
                else
                {
                    if (lProgressCount < m_listCsHeroSubGear[i].Level)
                    {
                        lProgressCount = m_listCsHeroSubGear[i].Level;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.SoulStoneLevel)
        {
            for (int i = 0; i < m_listCsHeroSubGear.Count; i++)
            {
                if (m_listCsHeroSubGear[i] == null)
                {
                    continue;
                }
                else
                {
                    for (int j = 0; j < m_listCsHeroSubGear[i].SoulstoneSocketList.Count; j++)
                    {
                        if (m_listCsHeroSubGear[i].SoulstoneSocketList[j].Item == null && m_listCsHeroSubGear[i].Equipped)
                        {
                            continue;
                        }
                        else
                        {
                            lProgressCount += m_listCsHeroSubGear[i].SoulstoneSocketList[j].Item.Level;
                        }
                    }
                }
            }
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.BattlePower)
        {
            lProgressCount = BattlePower;
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.MainGearEnchantLevel)
        {
            for (int i = 0; i < m_listCsHeroMainGearEquipped.Count; i++)
            {
                if (m_listCsHeroMainGearEquipped[i] == null)
                {
                    continue;
                }
                else
                {
                    if (lProgressCount < m_listCsHeroMainGearEquipped[i].EnchantLevel)
                    {
                        lProgressCount = m_listCsHeroMainGearEquipped[i].EnchantLevel;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.Rank)
        {
            lProgressCount = RankNo;
        }
        else
        {
            if (csHeroOpen7DayEventProgressCount == null)
            {
                lProgressCount = 0;
            }
            else
            {
                lProgressCount = csHeroOpen7DayEventProgressCount.AccProgressCount;
            }
        }

        return lProgressCount;
    }

	#endregion Open7DayEvent

	#region ChargingEvent

	public bool CheckChargingEvent()
	{
		// 첫충전
		if (CsCashManager.Instance.CheckFirstChargingEvent())
		{
			return true;
		}
		
		// 재충전
		if (CsCashManager.Instance.CheckReChargingEvent())
		{
			return true;
		}

		// 누적충전
		if (CsCashManager.Instance.CheckChargingEvent())
		{
			return true;
		}

		// 일일충전
		if (CsCashManager.Instance.CheckDailyChargingEvent())
		{
			return true;
		}

		// 누적소비
		if (CsCashManager.Instance.CheckConsumptionEvent())
		{
			return true;
		}

		// 일일소비
		if (CsCashManager.Instance.CheckDailyConsumptionEvent())
		{
			return true;
		}

		return false;
	}

	#endregion ChargingEvent

	#region Retrieval

	public void UpdateHeroRetrievalProgressCount(PDHeroRetrievalProgressCount heroRetrievalProgressCount)
	{
		CsHeroRetrievalProgressCount csHeroRetrievalProgressCount = GetHeroRetrievalProgressCount(heroRetrievalProgressCount.date, heroRetrievalProgressCount.retrievalId);

		if (csHeroRetrievalProgressCount == null)
		{
			m_listCsHeroRetrievalProgressCount.Add(new CsHeroRetrievalProgressCount(heroRetrievalProgressCount));
		}
		else
		{
			csHeroRetrievalProgressCount.ProgressCount = heroRetrievalProgressCount.prorgressCount;
		}
	}

	public CsHeroRetrievalProgressCount GetHeroRetrievalProgressCount(DateTime dtDate, int nRetrievalId)
	{
		for (int i = 0; i < m_listCsHeroRetrievalProgressCount.Count; i++)
		{
			if (m_listCsHeroRetrievalProgressCount[i].Date.CompareTo(dtDate) == 0 && m_listCsHeroRetrievalProgressCount[i].RetrievalId == nRetrievalId)
				return m_listCsHeroRetrievalProgressCount[i];
		}

		return null;
	}

	public void UpdateHeroRetrieval(int nRetrievalId, int nCount)
	{
		CsHeroRetrieval csHeroRetrieval = GetHeroRetrieval(nRetrievalId);

		if (csHeroRetrieval == null)
		{
			m_listCsHeroRetrieval.Add(new CsHeroRetrieval(nRetrievalId, nCount));
		}
		else
		{
			csHeroRetrieval.Count = nCount;
		}
	}

	public CsHeroRetrieval GetHeroRetrieval(int nRetrievalId)
	{
		for (int i = 0; i < m_listCsHeroRetrieval.Count; i++)
		{
			if (m_listCsHeroRetrieval[i].RetrievalId == nRetrievalId)
				return m_listCsHeroRetrieval[i];
		}

		return null;
	}

	public bool CheckRetrieval()
	{
		for (int i = 0; i < CsGameData.Instance.RetrievalList.Count; i++)
		{
			CsRetrieval csRetrieval = CsGameData.Instance.RetrievalList[i];

			if (csRetrieval.GetRemainingCount() > 0 &&
				(csRetrieval.GoldRetrievalRequiredGold <= CsGameData.Instance.MyHeroInfo.Gold ||
					csRetrieval.DiaRetrievalRequiredDia <= CsGameData.Instance.MyHeroInfo.Dia))
			{
				return true;
			}
		}

		return false;
	}

	#endregion Retrieval

	#region TaskConsignment

	//---------------------------------------------------------------------------------------------------
	public void UpdateHeroTaskConsignment(PDHeroTaskConsignment heroTaskConsignment)
	{
		m_listCsHeroTaskConsignment.Add(new CsHeroTaskConsignment(heroTaskConsignment));
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTaskConsignment GetHeroTaskConsignment(int nConsignmentId)
	{
		for (int i = 0; i < m_listCsHeroTaskConsignment.Count; i++)
		{
			if (m_listCsHeroTaskConsignment[i].ConsignmentId == nConsignmentId)
				return m_listCsHeroTaskConsignment[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveHeroTaskConsignment(Guid guidInstanceId)
	{
		for (int i = 0; i < m_listCsHeroTaskConsignment.Count; i++)
		{
			if (m_listCsHeroTaskConsignment[i].InstanceId == guidInstanceId)
			{
				m_listCsHeroTaskConsignment.RemoveAt(i);
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateHeroTaskConsignmentStartCount(PDHeroTaskConsignmentStartCount heroTaskConsignmentStartCount)
	{
		CsHeroTaskConsignmentStartCount csHeroTaskConsignmentStartCount = GetHeroTaskConsignmentStartCount(heroTaskConsignmentStartCount.consignmentId);

		if (csHeroTaskConsignmentStartCount == null)
		{
			m_listCsHeroTaskConsignmentStartCount.Add(new CsHeroTaskConsignmentStartCount(heroTaskConsignmentStartCount));
		}
		else
		{
			csHeroTaskConsignmentStartCount.StartCount = heroTaskConsignmentStartCount.startCount;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTaskConsignmentStartCount GetHeroTaskConsignmentStartCount(int nConsignmentId)
	{
		for (int i = 0; i < m_listCsHeroTaskConsignmentStartCount.Count; i++)
		{
			if (m_listCsHeroTaskConsignmentStartCount[i].ConsignmentId == nConsignmentId)
				return m_listCsHeroTaskConsignmentStartCount[i];
		}

		return null;
	}

	#endregion TaskConsignment

	#region DiaShop

	//---------------------------------------------------------------------------------------------------
	public void UpdateHeroDiaShopProductBuyCount(int nProductId, int nDailyBuyCount)
	{
		CsHeroDiaShopProductBuyCount csHeroDiaShopProductBuyCount = GetHeroDiaShopProductBuyCount(nProductId);

		if (csHeroDiaShopProductBuyCount == null)
		{
			m_listCsHeroDiaShopProductBuyCount.Add(new CsHeroDiaShopProductBuyCount(nProductId, nDailyBuyCount));
		}
		else
		{
			csHeroDiaShopProductBuyCount.BuyCount = nDailyBuyCount;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroDiaShopProductBuyCount GetHeroDiaShopProductBuyCount(int nProductId)
	{
		for (int i = 0; i < m_listCsHeroDiaShopProductBuyCount.Count; i++)
		{
			if (m_listCsHeroDiaShopProductBuyCount[i].ProductId == nProductId)
				return m_listCsHeroDiaShopProductBuyCount[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateTotalHeroDiaShopProductBuyCount(int nProductId, int nTotalBuyCount)
	{
		CsHeroDiaShopProductBuyCount csHeroDiaShopProductBuyCountTotal = GetTotalHeroDiaShopProductBuyCount(nProductId);

		if (csHeroDiaShopProductBuyCountTotal == null)
		{
			m_listCsHeroDiaShopProductBuyCountTotal.Add(new CsHeroDiaShopProductBuyCount(nProductId, nTotalBuyCount));
		}
		else
		{
			csHeroDiaShopProductBuyCountTotal.BuyCount = nTotalBuyCount;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroDiaShopProductBuyCount GetTotalHeroDiaShopProductBuyCount(int nProductId)
	{
		for (int i = 0; i < m_listCsHeroDiaShopProductBuyCountTotal.Count; i++)
		{
			if (m_listCsHeroDiaShopProductBuyCountTotal[i].ProductId == nProductId)
				return m_listCsHeroDiaShopProductBuyCountTotal[i];
		}

		return null;
	}

	#endregion DiaShop

	#region FearAltar Halidom Collection

	public void ResetWeeklyHalidomCollection()
	{
		m_nWeeklyFearAltarHalidomCollectionRewardNo = 0;
		m_listWeeklyFearAltarHalidom.Clear();
		m_listWeeklyRewardReceivedFearAltarHalidomElemental.Clear();
	}

	public bool HaveFearAltarHalidom(int nFearAltarHalidomId)
	{
		return m_listWeeklyFearAltarHalidom.Contains(nFearAltarHalidomId);
	}

	public bool RewardReceivedFearAltarHalidomElemental(int nFearAltarHalidomElementalId)
	{
		return m_listWeeklyRewardReceivedFearAltarHalidomElemental.Contains(nFearAltarHalidomElementalId);
	}

	#endregion FearAltar Halidom Collection

	#region Vip

	//---------------------------------------------------------------------------------------------------
	public bool CheckVipRewardReceivable(int nVipLevel)
	{
		if (nVipLevel == 0)
			return false;

		return nVipLevel <= m_csVipLevel.VipLevel && !m_listReceivedVipLevelReward.Contains(nVipLevel);
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckVipRewardsReceivable()
	{
		for (int i = 0; i < CsGameData.Instance.VipLevelList.Count; i++)
		{
			CsVipLevel csVipLevel = CsGameData.Instance.VipLevelList[i];

			if (CheckVipRewardReceivable(csVipLevel.VipLevel))
			{
				return true;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	#endregion Vip

	#region PotionAttr

	CsHeroPotionAttr GetHeroPotionAttr(int nPotionAttrId)
	{
		for (int i = 0; i < m_listCsHeroPotionAttr.Count; i++)
		{
			if (m_listCsHeroPotionAttr[i].PotionAttr.PotionAttrId == nPotionAttrId)
				return m_listCsHeroPotionAttr[i];
		}

		return null;
	}

	public void AddHeroPotionAttr(int nPotionAttrId, int nCount)
	{
		CsHeroPotionAttr csHeroPotionAttr = GetHeroPotionAttr(nPotionAttrId);

		if (csHeroPotionAttr == null)
		{
			m_listCsHeroPotionAttr.Add(new CsHeroPotionAttr(nPotionAttrId, nCount));
		}
		else
		{
			csHeroPotionAttr.Count = nCount;
		}
	}

	#endregion PotionAttr

}
