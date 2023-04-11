using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-06-09)
//---------------------------------------------------------------------------------------------------

public class CsGameConfig 
{
	public static CsGameConfig Instance
	{
		get { return CsSingleton<CsGameConfig>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	float m_flScaleFactor;

	int m_nMaxHeroCount;								// 최대영웅수

	int m_nStartContinentId;							// 시작대륙ID		
	float m_flStartXPosition;							// 시작x좌표
	float m_flStartYPosition;							// 시작y좌표
	float m_flStartZPosition;							// 시작z좌표
	float m_flStartRadius;								// 시작반지름
	int m_nStartYRotationType;							// 시작방향타입
	float m_flStartYRotation;							// 시작방향

	int m_nMailRetentionDay;							// 메일유지기간 (단위:일)
	int m_nMainGearOptionAttrMinCount;					// 메인장비옵션속성최소개수
	int m_nMainGearOptionAttrMaxCount;					// 메인장비옵션속성최대개수

	int m_nMainGearRefinementItemId;					// 메인장비제련(마법부여)아이템ID

	int m_nSpecialSkillId;								// 필살기스킬ID
	int m_nSpecialSkillMaxLak;							// 필살기스킬최대라크


	int m_nFreeImmediateRevivalDailyCount;              // 무료즉시부활일일횟수
	int m_nAutoSaftyRevivalWatingTime;                  // 자동안전부활대기시간	
	
	float m_flStartContinentSaftyRevivalXPosition;      // 시작대륙안전부활x좌표
	float m_flStartContinentSaftyRevivalYPosition;      // 시작대륙안전부활y좌표
	float m_flStartContinentSaftyRevivalZPosition;      // 시작대륙안전부활z좌표
	float m_flStartContinentSaftyRevivalRadius;         // 시작대륙안전부활반지름
	int m_nStartContinentSaftyRevivalYRotationType;     // 시작대륙안전부활방향타입
	float m_flStartContinentSaftyRevivalYRotation;      // 시작대륙안전부활방향
	int m_nSaftyRevivalContinentId;                     // 안전부활대륙ID
	float m_flSaftyRevivalXPosition;                    // 안전부활x좌표
	float m_flSaftyRevivalYPosition;                    // 안전부활y좌표
	float m_flSaftyRevivalZPosition;                    // 안전부활z좌표
	float m_flSaftyRevivalRadius;                       // 안전부활반지름
	int m_nSaftyRevivalYRotationType;                   // 안전부활방향타입
	float m_flSaftyRevivalYRotation;                    // 안전부활방향

	int m_nSimpleShopSellSlotCount;                     // 간편상점 판매 슬롯 수

	int m_nMainGearDisassembleSlotCount;                // 메인장비분해슬롯수

	int m_nRestRewardRequiredHeroLevel;                 // 휴식보상필요영웅레벨	
	int m_nRestRewardGoldReceiveExpRate;                // 휴식보상골드수령경험치율(만분율)
	int m_nRestRewardDiaReceiveExpRate;                 // 휴식보상다이아수령경험치율(만분율)

	int m_nPartyMemberMaxCount;                         // 파티최대멤버수
	int m_nPartyMemberLogOutDuration;                   // 파티멤버로그아웃기간
	int m_nPartyInvitationLifetime;                     // 파티초대유지기간
	int m_nPartyApplicationLifetime;                    // 파티신청유지기간
	int m_nPartyCallCoolTime;                           // 파티소집쿨타임

	int m_nChattingMaxLength;                           // 채팅최대길이
	int m_nChattingMinInterval;                         // 채팅최소간격
	int m_nWorldChattingDisplayDuration;                // 세계채팅표시기간
	int m_nWorldChattingItemId;                         // 세계채팅아이템ID
	int m_nChattingSendHistoryMaxCount;                 // 채팅발송기록최대개수
	int m_nChattingBubbleDisplayDuration;               // 채팅말풍선표시기간
	int m_nChattingDisplayMaxCount;                     // 채팅표시최대수

	long m_lWeekendAttendItemRewardId;                  // 주말출석아이템보상ID

	int m_nMountLevelUpItemId;                          // 탈것레벨업아이템ID
	int m_nMountQualityUpRequiredLevelUpCount;          // 탈것품질업필요레벨업수
	float m_flEquippedMountAttrFactor;                  // 착용탈것속성계수
	int m_nMountGearOptionAttrCount;                    // 탈것장비옵션속성개수
	int m_nMountGearRefinementItemId;                   // 탈것장비재강화아이템ID	
	int m_nDungeonFreeSweepDailyCount;                  // 던전무료소탕일일횟수
	int m_nDungeonSweepItemId;                          // 던전소탕아이템ID

	int m_nWingEnchantItemId;							// 날개강화아이템ID
	int m_nWingEnchantExp;								// 날개강화경험치
	int m_nMaxStamina;									// 최대체력
	int m_nStaminaRecoveryTime;							// 체력회복시간
	int m_nDefaultToastDisplayDuration;					// 일반토스트표시기간
	int m_nDefaultToastDisplayCount;					// 일반토스트표시개수
	int m_nItemToastDisplayDuration;					// 아이템토스트표시기간
	int m_nBattlePowerToastDisplayDuration;				// 전투력토스트표시기간
	int m_nContentOpenToastDisplayDuration;				// 컨텐츠개방토스트표시기간
	int m_nLocationAreaToastDisplayDuration;            // 위치지역토스트표시기간
	int m_nGuideToastDisplayDuration;                   // 가이드토스트표시기간

	int m_nHpPotionAutoUseHpRate;                       // HP물약자동사용HP율
	float m_flStandingBattleRange;                      // 제자리전투범위	
	float m_flShortDistanceBattleRange;                 // 근거리전투범위
	int m_nOptimizationModeWaitingTime;                 // 최적화모드대기시간
	int m_nDeadWarningDisplayHpRate;                    // 사망위험표시HP율

	int m_nPvpMinHeroLevel;                             // PVP가능 최소영웅레벨
	int m_nCartNormalSpeed;                             // 수레 기본속도
	int m_nCartHighSpeed;								// 수레 가속속도
	int m_nCartHighSpeedDuration;                       // 수레가속유지시간
	int m_nCartHighSpeedDurationExtension;              // 수레가속유지연장시간
	int m_nCartAccelCoolTime;                           // 수레가속쿨타임

	int m_nWorldLevelExpBuffMinHeroLevel;               // 세계레벨경험치버프적용최소영웅레벨

	int m_nNationTransmissionRequiredHeroLevel;         // 국가전송필요영웅레벨
	float m_flNationTransmissionExitXPosition;          // 국가전송출구x좌표	
	float m_flNationTransmissionExitYPosition;          // 국가전송출구y좌표	 
	float m_flNationTransmissionExitZPosition;          // 국가전송출구z좌표 
	float m_flNationTransmissionExitRadius;             // 국가전송출구반지름
	float m_flNationTransmissionExitYRotationType;      // 국가전송출구방향타입	 
	float m_flNationTransmissionExitYRotation;          // 국가전송출구방향 
	int m_nBountyHunterQuestMaxCount;                   // 현상금사냥꾼퀘스트최대횟수
	int m_nBountyHunterQuestRequiredHeroLevel;          // 현상금사냥꾼퀘스트필요영웅레벨

	int m_nTodayMissionCount;                           // 오늘의미션수						
	int m_nFieldOfHonorDisplayMaxRanking;               // 결투장최대표시랭킹
	int m_nFieldOfHonorDisplayHistoryCount;             // 결투장기록표시수
	int m_nRankingDisplayMaxCount;                      // 랭킹표시최대수

	int m_nGuildRequiredHeroLevel;                      // 길드필요영웅레벨
	int m_nGuildCreationRequiredVipLevel;               // 길드생성필요VIP레벨
	int m_nGuildCreationRequiredDia;                    // 길드생성필요다이아
	int m_nGuildRejoinIntervalTime;                     // 길드재가입제한시간
	int m_nGuildApplicationReceptionMaxCount;           // 길드가입신청접수최대수
	int m_nGuildDailyApplicationMaxCount;               // 길드일일가입신청최대횟수
	int m_nGuildDailyBanishmentMaxCount;                // 길드일일추방최대횟수
	int m_nGuildInvitationLifetime;                     // 길드초대요청유효시간
	int m_nGuildNoticeMaxLength;                        // 길드공지최대길이
	int m_nGuildViceMasterCount;                        // 길드부길드장인원수
	int m_nGuildLordCount;                              // 길드로드인원수

	int m_nGuildCallLifetime;                           // 길드소환유지기간
	float m_flGuildCallRadius;                          // 길드소환반경
	int m_nNationCallLifetime;                          // 국가소집유지기간
	float m_flNationCallRadius;                         // 국가소집반경

	string m_strGuildDailyObjectiveNoticeText;			// 길드일일목표알림텍스트키
	int m_nGuildDailyObjectiveNoticeCoolTime;           // 	길드일일목표알림쿨타임
	int m_nDefaultGuildWeeklyObjectiveId;
	int m_nGuildHuntingDonationMaxCount;
	int m_nGuildHuntingDonationItemId;
	long m_lGuildHuntingDonationItemRewardId;
	long m_lGuildHuntingDonationCompletionItemRewardId;

	int m_nSignBoardDisplayDuration;                    // 전광판표시기간
	int m_nNoticeBoardDisplayDuration;                  // 알림판표시기간

	int m_nCreatureCardShopRandomProductCount;          // 크리처카드상점랜덤상품수
	int m_nCreatureCardShopPaidRefreshDia;              // 크리처카드상점유료갱신다이아

	int m_nGuideActivationRequiredHeroLevel;            // 도우미활성화필요영웅레벨

	int m_nAccelerationRequiredMoveDuration;			// 가속필요이동지속시간
	int m_nAccelerationMoveSpeed;						// 가속이동속도

	int m_nSceneryQuestRequiredMainQuestNo;             // 풍광퀘스트필요메인퀘스트번호
	int m_nMenuContentOpenPreviewRequiredHeroLevel;     // 메뉴컨텐츠개방미리보기필요영웅레벨

	int m_nMonsterGroggyDuration;
	int m_nMonsterStealDuration;

	int m_nRookieGiftScratchOpenDuration;				// 신병선물스크래치개방기간
	int m_nOpenGiftRequiredHeroLevel;                   // 오픈선물필요영웅레벨

	int m_nOpen7DayEventRequiredMainQuestNo;            // 오픈7일이벤트필요메인퀘스트번호

	int m_nTaskConsignmentRequiredVipLevel;             // 할일위탁필요VIP레벨

	int m_nWarehouseRequiredVipLevel;                   // 창고필요VIP레벨
	int m_nFreeWarehouseSlotCount;                      // 무료창고슬롯수

	int m_nWingMemoryPieceInstallationRequiredHeroLevel;	
	int m_nOrdealQuestSlotCount;                        // 시련퀘스트슬롯수

	int m_nFriendMaxCount;                              // 친구최대수
	int m_nTempFriendMaxCount;                          // 임시친구최대수
	int m_nDeadRecordMaxCount;                          // 사망기록최대수
	int m_nBlacklistEntryMaxCount;                      // 블랙리스트항목최대수

	int m_nBlessingQuestListMaxCount;					// 축복퀘스트목록최대수
	int m_nBlessingQuestRequiredHeroLevel;				// 축복퀘스트필요영웅레벨
	int m_nBlessingListMaxCount;						// 축복목록최대수
	int m_nOwnerProspectQuestListMaxCount;				// 소유자유망자퀘스트목록최대수
	int m_nTargetProspectQuestListMaxCount;				// 대상유망자퀘스트목록최대수

	int m_nCreatureMaxCount;                            // 크리처최대수
	int m_nCreatureCheerMaxCount;                       // 크리처응원수	
	float m_flCreatureCheerAttrFactor;                  // 크리처응원속성계수
	float m_flCreatureEvaluationFactor;                 // 크리처평점계수
	int m_nCreatureAdditionalAttrCount;                 // 크리처추가속성수
	int m_nCreatureSkillSlotMaxCount;                   // 크리처스킬슬롯최대수
	int m_nCreatureSkillSlotBaseOpenCount;              // 크리처스킬기본개방개수
	int m_nCreatureCompositionExpRetrievalRate;         // 크리처합성양육경험치회수비율	
	int m_nCreatureCompositionExpRetrievalResultItemId; // 크리처합성양육경험치회수결과아이템ID
	int m_nCreatureCompositionSkillProtectionItemId;    // 크리처합성스킬보호아이템ID
	int m_nCreatureInjectionExpRetrivalRate;            // 크리처주입경험치회수비율
	int m_nCreatureVariationRequiredItemId;             // 크리처변이필요아이템ID
	int m_nCreatureAdditionalAttrSwitchRequiredItemId;  // 크리처추가속성전환필요아이템ID
	int m_nCreatureReleaseExpRetrievalRate;             // 크리처방생양육경험치회수비율
	int m_nParticipationCreatureDisplayRequiredVipLevel;    // 출전크리처표시필요VIP레벨

	int m_nPresentPopularityPointRankingDisplayMaxCount;    // 선물인기점수랭킹표시최대수
	int m_nPresentContributionPointRankingDisplayMaxCount;  // 선물공헌점수랭킹표시최대수

	int m_nGuildBlessingGuildTerritoryNpcId;                // 길드축복길드영지NPCID]

	long m_lOpen7DayEventCostumeItemRewardId;               // 오픈7일이벤트코스튬보상아이템ID
	int m_nOpen7DayEventCostumeRewardRequiredItemId;        // 오픈7일이벤트코스튬보상필요아이템ID
	int m_nOpen7DayEventCostumeRewardRequiredItemCount;     // 오픈7일이벤트코스튬보상필요아이템수량

	int m_nNationAllianceUnavailableStartTime;
	int m_nNationAllianceUnavailableEndTime;
	long m_lNationAllianceRequiredFund;
	int m_nNationAllianceRenounceUnavailableDuration;
	int m_nNationBasePower;

	int m_nJobChangeRequiredHeroLevel;
	int m_nJobChangeRequiredItemId;
	int m_nJobChangeQuestCompletionClientTutorialId;

	int m_nChargeEventRequiredHeroLevel;                    // 충전이벤트필요영웅레벨
	int m_nConsumeEventRequiredHeroLevel;                   // 소비이벤트필요영웅레벨

	int m_nArtifactRequiredConditionType;
	int m_nArtifactRequiredConditionValue;
	int m_nArtifactMaxLevel;
	int m_nMountAwakeningRequiredHeroLevel;
	int m_nMountAwakeningItemId;
	int m_nMountPotionAttrItemId;

	int m_nCostumeEnchantItemId;
	int m_nCostumeCollectionActivationItemId;
	int m_nCostumeCollectionShuffleItemId;
	int m_nCostumeCollectionShuffleItemCount;

	int m_nSafeRevivalHpRecoveryRate;
	int m_nImmediateRevivalHpRecoveryRate;

	//---------------------------------------------------------------------------------------------------

	public CsItemReward ItemRewardWeekendAttend
	{
		get { return CsGameData.Instance.GetItemReward(m_lWeekendAttendItemRewardId); }
	}

	public float ScaleFactor
	{
		get { return m_flScaleFactor; }
		set { m_flScaleFactor = value; }
	}

	public int MaxHeroCount
	{
		get { return m_nMaxHeroCount; }
	}

	public int StartContinentId
	{
		get { return m_nStartContinentId; }
	}

	public float StartXPosition
	{
		get { return m_flStartXPosition; }
	}

	public float StartYPosition
	{
		get { return m_flStartYPosition; }
	}

	public float StartZPosition
	{
		get { return m_flStartZPosition; }
	}

	public float StartRadius
	{
		get { return m_flStartRadius; }
	}

	public int StartYRotationType
	{
		get { return m_nStartYRotationType; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public int MailRetentionDay
	{
		get { return m_nMailRetentionDay; }
	}

	public int MainGearOptionAttrMinCount
	{
		get { return m_nMainGearOptionAttrMinCount; }
	}

	public int MainGearOptionAttrMaxCount
	{
		get { return m_nMainGearOptionAttrMaxCount; }
	}

	public int MainGearRefinementItemId
	{
		get { return m_nMainGearRefinementItemId; }
	}

	public int SpecialSkillId
	{
		get { return m_nSpecialSkillId; }
	}

	public int SpecialSkillMaxLak
	{
		get { return m_nSpecialSkillMaxLak; }
	}

	public int FreeImmediateRevivalDailyCount
	{
		get { return m_nFreeImmediateRevivalDailyCount; }
	}

	public int AutoSaftyRevivalWatingTime
	{
		get { return m_nAutoSaftyRevivalWatingTime; }
	}

	public float StartContinentSaftyRevivalXPosition
	{
		get { return m_flStartContinentSaftyRevivalXPosition; }
	}

	public float StartContinentSaftyRevivalYPosition
	{
		get { return m_flStartContinentSaftyRevivalYPosition; }
	}

	public float StartContinentSaftyRevivalZPosition
	{
		get { return m_flStartContinentSaftyRevivalZPosition; }
	}

	public float StartContinentSaftyRevivalRadius
	{
		get { return m_flStartContinentSaftyRevivalRadius; }
	}

	public int StartContinentSaftyRevivalYRotationType
	{
		get { return m_nStartContinentSaftyRevivalYRotationType; }
	}

	public float StartContinentSaftyRevivalYRotation
	{
		get { return m_flStartContinentSaftyRevivalYRotation; }
	}

	public int SaftyRevivalContinentId
	{
		get { return m_nSaftyRevivalContinentId; }
	}

	public float SaftyRevivalXPosition
	{
		get { return m_flSaftyRevivalXPosition; }
	}

	public float SaftyRevivalYPosition
	{
		get { return m_flSaftyRevivalYPosition; }
	}

	public float SaftyRevivalZPosition
	{
		get { return m_flSaftyRevivalZPosition; }
	}

	public float SaftyRevivalRadius
	{
		get { return m_flSaftyRevivalRadius; }
	}

	public int SaftyRevivalYRotationType
	{
		get { return m_nSaftyRevivalYRotationType; }
	}

	public float SaftyRevivalYRotation
	{
		get { return m_flSaftyRevivalYRotation; }
	}

	public int SimpleShopSellSlotCount
	{
		get { return m_nSimpleShopSellSlotCount; }
	}

	public int MainGearDisassembleSlotCount
	{
		get { return m_nMainGearDisassembleSlotCount; }
	}

	public int RestRewardRequiredHeroLevel
	{
		get { return m_nRestRewardRequiredHeroLevel; }
	}

	public int RestRewardGoldReceiveExpPercentage
	{
		get { return m_nRestRewardGoldReceiveExpRate / 100; }
	}

	public int RestRewardDiaReceiveExpPercentage
	{
		get { return m_nRestRewardDiaReceiveExpRate / 100; }
	}

	public int PartyMemberMaxCount
	{
		get { return m_nPartyMemberMaxCount; }
	}

	public int PartyMemberLogOutDuration
	{
		get { return m_nPartyMemberLogOutDuration; }
	}

	public int PartyInvitationLifetime
	{
		get { return m_nPartyInvitationLifetime; }
	}

	public int PartyApplicationLifetime
	{
		get { return m_nPartyApplicationLifetime; }
	}

	public int PartyCallCoolTime
	{
		get { return m_nPartyCallCoolTime; }
	}

	public int ChattingMaxLength
	{
		get { return m_nChattingMaxLength; }
	}

	public int ChattingMinInterval
	{
		get { return m_nChattingMinInterval; }
	}

	public int WorldChattingDisplayDuration
	{
		get { return m_nWorldChattingDisplayDuration; }
	}

	public int WorldChattingItemId
	{
		get { return m_nWorldChattingItemId; }
	}

	public int ChattingSendHistoryMaxCount
	{
		get { return m_nChattingSendHistoryMaxCount; }
	}

	public int ChattingBubbleDisplayDuration
	{
		get { return m_nChattingBubbleDisplayDuration; }
	}

	public int ChattingDisplayMaxCount
	{
		get { return m_nChattingDisplayMaxCount; }
	}

	public int MountLevelUpItemId
	{
		get { return m_nMountLevelUpItemId; }
	}

	public int MountQualityUpRequiredLevelUpCount
	{
		get { return m_nMountQualityUpRequiredLevelUpCount; }
	}

	public float EquippedMountAttrFactor
	{
		get { return m_flEquippedMountAttrFactor; }
	}

	public int MountGearOptionAttrCount
	{
		get { return m_nMountGearOptionAttrCount; }
	}

	public int MountGearRefinementItemId
	{
		get { return m_nMountGearRefinementItemId; }
	}

	public int DungeonFreeSweepDailyCount
	{
		get { return m_nDungeonFreeSweepDailyCount; }
	}

	public int DungeonSweepItemId
	{
		get { return m_nDungeonSweepItemId; }
	}

	public int WingEnchantItemId
	{
		get { return m_nWingEnchantItemId; }
	}

	public int WingEnchantExp
	{
		get { return m_nWingEnchantExp; }
	}

	public int MaxStamina
	{
		get { return m_nMaxStamina; }
	}

	public int StaminaRecoveryTime
	{
		get { return m_nStaminaRecoveryTime; }
	}

	public int DefaultToastDisplayDuration
	{
		get { return m_nDefaultToastDisplayDuration; }
	}

	public int DefaultToastDisplayCount
	{
		get { return m_nDefaultToastDisplayCount; }
	}

	public int ItemToastDisplayDuration
	{
		get { return m_nItemToastDisplayDuration; }
	}

	public int BattlePowerToastDisplayDuration
	{
		get { return m_nBattlePowerToastDisplayDuration; }
	}

	public int ContentOpenToastDisplayDuration
	{
		get { return m_nContentOpenToastDisplayDuration; }
	}

	public int LocationAreaToastDisplayDuration
	{
		get { return m_nLocationAreaToastDisplayDuration; }
	}

	public int GuideToastDisplayDuration
	{
		get { return m_nGuideToastDisplayDuration; }
	}

	public int HpPotionAutoUseHpRate
	{
		get { return m_nHpPotionAutoUseHpRate; }
	}

	public float StandingBattleRange
	{
		get { return m_flStandingBattleRange; }
	}

	public float ShortDistanceBattleRange
	{
		get { return m_flShortDistanceBattleRange; }
	}

	public int OptimizationModeWaitingTime
	{
		get { return m_nOptimizationModeWaitingTime; }
	}

	public int DeadWarningDisplayHpRate
	{
		get { return m_nDeadWarningDisplayHpRate; }
	}

	public int PvpMinHeroLevel
	{
		get { return m_nPvpMinHeroLevel; }
	}

	public int CartNormalSpeed
	{
		get { return m_nCartNormalSpeed; }
	}

	public int CartHighSpeed
	{
		get { return m_nCartHighSpeed; }
	}

	public int CartHighSpeedDuration
	{
		get { return m_nCartHighSpeedDuration; }
	}

	public int CartHighSpeedDurationExtension
	{
		get { return m_nCartHighSpeedDurationExtension; }
	}

	public int CartAccelCoolTime
	{
		get { return m_nCartAccelCoolTime; }
	}

	public int WorldLevelExpBuffMinHeroLevel
	{
		get { return m_nWorldLevelExpBuffMinHeroLevel; }
	}

	public int NationTransmissionRequiredHeroLevel
	{
		get { return m_nNationTransmissionRequiredHeroLevel; }
	}

	public float NationTransmissionExitXPosition
	{
		get { return m_flNationTransmissionExitXPosition; }
	}

	public float NationTransmissionExitYPosition
	{
		get { return m_flNationTransmissionExitYPosition; }
	}

	public float NationTransmissionExitZPosition
	{
		get { return m_flNationTransmissionExitZPosition; }
	}

	public float NationTransmissionExitRadius
	{
		get { return m_flNationTransmissionExitRadius; }
	}

	public float NationTransmissionExitYRotationType
	{
		get { return m_flNationTransmissionExitYRotationType; }
	}

	public float NationTransmissionExitYRotation
	{
		get { return m_flNationTransmissionExitYRotation; }
	}

	public int BountyHunterQuestMaxCount
	{
		get { return m_nBountyHunterQuestMaxCount; }
	}

	public int BountyHunterQuestRequiredHeroLevel
	{
		get { return m_nBountyHunterQuestRequiredHeroLevel; }
	}

	public int TodayMissionCount
	{
		get { return m_nTodayMissionCount; }
	}

	public int FieldOfHonorDisplayMaxRanking
	{
		get { return m_nFieldOfHonorDisplayMaxRanking; }
	}

	public int FieldOfHonorDisplayHistoryCount
	{
		get { return m_nFieldOfHonorDisplayHistoryCount; }
	}

	public int RankingDisplayMaxCount
	{
		get { return m_nRankingDisplayMaxCount; }
	}

	public int GuildRequiredHeroLevel
	{
		get { return m_nGuildRequiredHeroLevel; }
	}

	public int GuildCreationRequiredVipLevel
	{
		get { return m_nGuildCreationRequiredVipLevel; }
	}

	public int GuildCreationRequiredDia
	{
		get { return m_nGuildCreationRequiredDia; }
	}

	public int GuildRejoinIntervalTime
	{
		get { return m_nGuildRejoinIntervalTime; }
	}

	public int GuildApplicationReceptionMaxCount
	{
		get { return m_nGuildApplicationReceptionMaxCount; }
	}

	public int GuildDailyApplicationMaxCount
	{
		get { return m_nGuildDailyApplicationMaxCount; }
	}

	public int GuildDailyBanishmentMaxCount
	{
		get { return m_nGuildDailyBanishmentMaxCount; }
	}

	public int GuildInvitationLifetime
	{
		get { return m_nGuildInvitationLifetime; }
	}

	public int GuildNoticeMaxLength
	{
		get { return m_nGuildNoticeMaxLength; }
	}

	public int GuildCallLifetime
	{
		get { return m_nGuildCallLifetime; }
	}

	public float GuildCallRadius
	{
		get { return m_flGuildCallRadius; }
	}

	public int NationCallLifetime
	{
		get { return m_nNationCallLifetime; }
	}

	public float NationCallRadius
	{
		get { return m_flNationCallRadius; }
	}

	public string GuildDailyObjectiveNoticeText
	{
		get { return m_strGuildDailyObjectiveNoticeText; }
	}

	public int GuildDailyObjectiveNoticeCoolTime
	{
		get { return m_nGuildDailyObjectiveNoticeCoolTime; }
	}

	public int DefaultGuildWeeklyObjectiveId
	{
		get { return m_nDefaultGuildWeeklyObjectiveId; }
	}

	public int GuildHuntingDonationMaxCount
	{
		get { return m_nGuildHuntingDonationMaxCount; }
	}

	public int GuildHuntingDonationItemId
	{
		get { return m_nGuildHuntingDonationItemId; }
	}

	public long GuildHuntingDonationItemRewardId
	{
		get { return m_lGuildHuntingDonationItemRewardId; }
	}

	public long GuildHuntingDonationCompletionItemRewardId
	{
		get { return m_lGuildHuntingDonationCompletionItemRewardId; }
	}

	public int SignBoardDisplayDuration
	{
		get { return m_nSignBoardDisplayDuration; }
	}

	public int NoticeBoardDisplayDuration
	{
		get { return m_nNoticeBoardDisplayDuration; }
	}

	public int CreatureCardShopRandomProductCount
	{
		get { return m_nCreatureCardShopRandomProductCount; }
	}

	public int CreatureCardShopPaidRefreshDia
	{
		get { return m_nCreatureCardShopPaidRefreshDia; }
	}

	public int GuideActivationRequiredHeroLevel
	{
		get { return m_nGuideActivationRequiredHeroLevel; }
	}

	public int AccelerationRequiredMoveDuration
	{
		get { return m_nAccelerationRequiredMoveDuration; }
	}

	public int AccelerationMoveSpeed
	{
		get { return m_nAccelerationMoveSpeed; }
	}

	public int SceneryQuestRequiredMainQuestNo
	{
		get { return m_nSceneryQuestRequiredMainQuestNo; }
	}

	public int MenuContentOpenPreviewRequiredHeroLevel
	{
		get { return m_nMenuContentOpenPreviewRequiredHeroLevel; }
	}

	public int MonsterGroggyDuration
	{
		get { return m_nMonsterGroggyDuration; }
	}

	public int MonsterStealDuration
	{
		get { return m_nMonsterStealDuration; }
	}

	public int RookieGiftScratchOpenDuration
	{
		get { return m_nRookieGiftScratchOpenDuration; }
	}

	public int OpenGiftRequiredHeroLevel
	{
		get { return m_nOpenGiftRequiredHeroLevel; }
	}

	public int Open7DayEventRequiredMainQuestNo
	{
		get { return m_nOpen7DayEventRequiredMainQuestNo; }
	}

    public int TaskConsignmentRequiredVipLevel
    {
        get { return m_nTaskConsignmentRequiredVipLevel; }
    }

	public int WarehouseRequiredVipLevel
	{
		get { return m_nWarehouseRequiredVipLevel; }
	}

	public int FreeWarehouseSlotCount
	{
		get { return m_nFreeWarehouseSlotCount; }
	}

	public int WingMemoryPieceInstallationRequiredHeroLevel
	{
		get { return m_nWingMemoryPieceInstallationRequiredHeroLevel; }
	}

	public int OrdealQuestSlotCount
	{
		get { return m_nOrdealQuestSlotCount; }
	}

	public int FriendMaxCount
	{
		get { return m_nFriendMaxCount; }
	}

	public int TempFriendMaxCount
	{
		get { return m_nTempFriendMaxCount; }
	}

	public int DeadRecordMaxCount
	{
		get { return m_nDeadRecordMaxCount; }
	}

	public int BlacklistEntryMaxCount
	{
		get { return m_nBlacklistEntryMaxCount; }
	}

	public int BlessingQuestListMaxCount
	{
		get { return m_nBlessingQuestListMaxCount; }
	}

	public int BlessingQuestRequiredHeroLevel
	{
		get { return m_nBlessingQuestRequiredHeroLevel; }
	}

	public int BlessingListMaxCount
	{
		get { return m_nBlessingListMaxCount; }
	}

	public int OwnerProspectQuestListMaxCount
	{
		get { return m_nOwnerProspectQuestListMaxCount; }
	}

	public int TargetProspectQuestListMaxCount
	{
		get { return m_nTargetProspectQuestListMaxCount; }
	}
	
	public int CreatureMaxCount
	{
		get { return m_nCreatureMaxCount; }
	}

	public int CreatureCheerMaxCount
	{
		get { return m_nCreatureCheerMaxCount; }
	}

	public float CreatureCheerAttrFactor
	{
		get { return m_flCreatureCheerAttrFactor; }
	}

	public float CreatureEvaluationFactor
	{
		get { return m_flCreatureEvaluationFactor; }
	}

	public int CreatureAdditionalAttrCount
	{
		get { return m_nCreatureAdditionalAttrCount; }
	}

	public int CreatureSkillSlotMaxCount
	{
		get { return m_nCreatureSkillSlotMaxCount; }
	}

	public int CreatureSkillSlotBaseOpenCount
	{
		get { return m_nCreatureSkillSlotBaseOpenCount; }
	}

	public int CreatureCompositionExpRetrievalRate
	{
		get { return m_nCreatureCompositionExpRetrievalRate; }
	}

	public int CreatureCompositionExpRetrievalResultItemId
	{
		get { return m_nCreatureCompositionExpRetrievalResultItemId; }
	}

	public int CreatureCompositionSkillProtectionItemId
	{
		get { return m_nCreatureCompositionSkillProtectionItemId; }
	}

	public int CreatureInjectionExpRetrivalRate
	{
		get { return m_nCreatureInjectionExpRetrivalRate; }
	}

	public int CreatureVariationRequiredItemId
	{
		get { return m_nCreatureVariationRequiredItemId; }
	}

	public int CreatureAdditionalAttrSwitchRequiredItemId
	{
		get { return m_nCreatureAdditionalAttrSwitchRequiredItemId; }
	}

	public int CreatureReleaseExpRetrievalRate
	{
		get { return m_nCreatureReleaseExpRetrievalRate; }
	}

	public int ParticipationCreatureDisplayRequiredVipLevel
	{
		get { return m_nParticipationCreatureDisplayRequiredVipLevel; }
	}

	public int PresentPopularityPointRankingDisplayMaxCount
	{
		get { return m_nPresentPopularityPointRankingDisplayMaxCount; }
	}

	public int PresentContributionPointRankingDisplayMaxCount
	{
		get { return m_nPresentContributionPointRankingDisplayMaxCount; }
	}

	public int GuildBlessingGuildTerritoryNpcId
	{
		get { return m_nGuildBlessingGuildTerritoryNpcId; }
	}

	public long Open7DayEventCostumeItemRewardId
	{
		get { return m_lOpen7DayEventCostumeItemRewardId; }
	}

	public int Open7DayEventCostumeRewardRequiredItemId
	{
		get { return m_nOpen7DayEventCostumeRewardRequiredItemId; }
	}

	public int Open7DayEventCostumeRewardRequiredItemCount
	{
		get { return m_nOpen7DayEventCostumeRewardRequiredItemCount; }
	}

	public int NationAllianceUnavailableStartTime
	{
		get { return m_nNationAllianceUnavailableStartTime; }
	}

	public int NationAllianceUnavailableEndTime
	{
		get { return m_nNationAllianceUnavailableEndTime; }
	}

	public long NationAllianceRequiredFund
	{
		get { return m_lNationAllianceRequiredFund; }
	}

	public int NationAllianceRenounceUnavailableDuration
	{
		get { return m_nNationAllianceRenounceUnavailableDuration; }
	}

	public int NationBasePower
	{
		get { return m_nNationBasePower; }
	}

	public int JobChangeRequiredHeroLevel
	{
		get { return m_nJobChangeRequiredHeroLevel; }
	}

	public int JobChangeRequiredItemId
	{
		get { return m_nJobChangeRequiredItemId; }
	}

	public int JobChangeQuestCompletionClientTutorialId
	{
		get { return m_nJobChangeQuestCompletionClientTutorialId; }
	}

	public int ChargeEventRequiredHeroLevel
	{
		get { return m_nChargeEventRequiredHeroLevel; }
	}

	public int ConsumeEventRequiredHeroLevel
	{
		get { return m_nConsumeEventRequiredHeroLevel; }
	}

	public int ArtifactRequiredConditionType
	{
		get { return m_nArtifactRequiredConditionType; }
	}

	public int ArtifactRequiredConditionValue
	{
		get { return m_nArtifactRequiredConditionValue; }
	}

	public int ArtifactMaxLevel
	{
		get { return m_nArtifactMaxLevel; }
	}

	public int MountAwakeningRequiredHeroLevel
	{
		get { return m_nMountAwakeningRequiredHeroLevel; }
	}

	public int MountAwakeningItemId
	{
		get { return m_nMountAwakeningItemId; }
	}

	public int MountPotionAttrItemId
	{
		get { return m_nMountPotionAttrItemId; }
	}

	public int CostumeEnchantItemId
	{
		get { return m_nCostumeEnchantItemId; }
	}

	public int CostumeCollectionActivationItemId
	{
		get { return m_nCostumeCollectionActivationItemId; }
	}

	public int CostumeCollectionShuffleItemId
	{
		get { return m_nCostumeCollectionShuffleItemId; }
	}

	public int CostumeCollectionShuffleItemCount
	{
		get { return m_nCostumeCollectionShuffleItemCount; }
	}

	public int SafeRevivalHpRecoveryRate
	{
		get { return m_nSafeRevivalHpRecoveryRate; }
	}

	public int ImmediateRevivalHpRecoveryRate
	{
		get { return m_nImmediateRevivalHpRecoveryRate; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Initialize(WPDGameConfig gameConfig)
	{
		m_nMaxHeroCount = gameConfig.maxHeroCount;

		m_nStartContinentId = gameConfig.startContinentId;
		m_flStartXPosition = gameConfig.startXPosition;
		m_flStartYPosition = gameConfig.startYPosition;
		m_flStartZPosition = gameConfig.startZPosition;
		m_flStartRadius = gameConfig.startRadius;
		m_nStartYRotationType = gameConfig.startYRotationType;
		m_flStartYRotation = gameConfig.startYRotation;

		m_nMailRetentionDay = gameConfig.mailRetentionDay;
		m_nMainGearOptionAttrMinCount = gameConfig.mainGearOptionAttrMinCount;
		m_nMainGearOptionAttrMaxCount = gameConfig.mainGearOptionAttrMaxCount;

		m_nMainGearRefinementItemId = gameConfig.mainGearRefinementItemId;

		m_nSpecialSkillId = gameConfig.specialSkillId;
		m_nSpecialSkillMaxLak = gameConfig.specialSkillMaxLak;

		m_nFreeImmediateRevivalDailyCount = gameConfig.freeImmediateRevivalDailyCount;

		m_nAutoSaftyRevivalWatingTime = gameConfig.autoSaftyRevivalWatingTime;
		m_flStartContinentSaftyRevivalXPosition = gameConfig.startContinentSaftyRevivalXPosition;
		m_flStartContinentSaftyRevivalYPosition = gameConfig.startContinentSaftyRevivalYPosition;
		m_flStartContinentSaftyRevivalZPosition = gameConfig.startContinentSaftyRevivalZPosition;
		m_flStartContinentSaftyRevivalRadius = gameConfig.startContinentSaftyRevivalRadius;
		m_nStartContinentSaftyRevivalYRotationType = gameConfig.startContinentSaftyRevivalYRotationType;
		m_flStartContinentSaftyRevivalYRotation = gameConfig.startContinentSaftyRevivalYRotation;
		m_nSaftyRevivalContinentId = gameConfig.saftyRevivalContinentId;
		m_flSaftyRevivalXPosition = gameConfig.saftyRevivalYPosition;
		m_flSaftyRevivalYPosition = gameConfig.saftyRevivalYPosition;
		m_flSaftyRevivalZPosition = gameConfig.saftyRevivalZPosition;
		m_flSaftyRevivalRadius = gameConfig.saftyRevivalRadius;
		m_nSaftyRevivalYRotationType = gameConfig.saftyRevivalYRotationType;
		m_flSaftyRevivalYRotation = gameConfig.saftyRevivalYRotation;

		m_nSimpleShopSellSlotCount = gameConfig.simpleShopSellSlotCount;

		m_nMainGearDisassembleSlotCount = gameConfig.mainGearDisassembleSlotCount;

		m_nRestRewardRequiredHeroLevel = gameConfig.restRewardRequiredHeroLevel;
		m_nRestRewardGoldReceiveExpRate = gameConfig.restRewardGoldReceiveExpRate;
		m_nRestRewardDiaReceiveExpRate = gameConfig.restRewardDiaReceiveExpRate;

		m_nPartyMemberMaxCount = gameConfig.partyMemberMaxCount;
		m_nPartyMemberLogOutDuration = gameConfig.partyMemberLogOutDuration;
		m_nPartyInvitationLifetime = gameConfig.partyInvitationLifetime;
		m_nPartyApplicationLifetime = gameConfig.partyApplicationLifetime;
		m_nPartyCallCoolTime = gameConfig.partyCallCoolTime;

		m_nChattingMaxLength = gameConfig.chattingMaxLength;
		m_nChattingMinInterval = gameConfig.chattingMinInterval;
		m_nWorldChattingDisplayDuration = gameConfig.worldChattingDisplayDuration;
		m_nWorldChattingItemId = gameConfig.worldChattingItemId;
		m_nChattingSendHistoryMaxCount = gameConfig.chattingSendHistoryMaxCount;
		m_nChattingBubbleDisplayDuration = gameConfig.chattingBubbleDisplayDuration;
		m_nChattingDisplayMaxCount = gameConfig.chattingDisplayMaxCount;

		m_lWeekendAttendItemRewardId = gameConfig.weekendAttendItemRewardId;

		m_nMountLevelUpItemId = gameConfig.mountLevelUpItemId;       
		m_nMountQualityUpRequiredLevelUpCount = gameConfig.mountQualityUpRequiredLevelUpCount;
		m_flEquippedMountAttrFactor = gameConfig.equippedMountAttrFactor;
		m_nMountGearOptionAttrCount = gameConfig.mountGearOptionAttrCount;
		m_nMountGearRefinementItemId = gameConfig.mountGearRefinementItemId;
		m_nDungeonFreeSweepDailyCount = gameConfig.dungeonFreeSweepDailyCount;
		m_nDungeonSweepItemId = gameConfig.dungeonSweepItemId;

		m_nWingEnchantItemId = gameConfig.wingEnchantItemId;
		m_nWingEnchantExp = gameConfig.wingEnchantExp;
		m_nMaxStamina = gameConfig.maxStamina;
		m_nStaminaRecoveryTime = gameConfig.staminaRecoveryTime;
		m_nDefaultToastDisplayDuration = gameConfig.defaultToastDisplayDuration;
		m_nDefaultToastDisplayCount = gameConfig.defaultToastDisplayCount;
		m_nItemToastDisplayDuration = gameConfig.itemToastDisplayDuration;
		m_nBattlePowerToastDisplayDuration = gameConfig.battlePowerToastDisplayDuration;
		m_nContentOpenToastDisplayDuration = gameConfig.contentOpenToastDisplayDuration;
		m_nLocationAreaToastDisplayDuration = gameConfig.locationAreaToastDisplayDuration;
		m_nGuideToastDisplayDuration = gameConfig.guideToastDisplayDuration;

		m_nHpPotionAutoUseHpRate = gameConfig.hpPotionAutoUseHpRate;
		m_flStandingBattleRange = gameConfig.standingBattleRange;
		m_flShortDistanceBattleRange = gameConfig.shortDistanceBattleRange;
		m_nOptimizationModeWaitingTime = gameConfig.optimizationModeWaitingTime;
		m_nDeadWarningDisplayHpRate = gameConfig.deadWarningDisplayHpRate;

		m_nPvpMinHeroLevel = gameConfig.pvpMinHeroLevel;
		m_nCartNormalSpeed = gameConfig.cartNormalSpeed;
		m_nCartHighSpeed = gameConfig.cartHighSpeed;
		m_nCartHighSpeedDuration = gameConfig.cartHighSpeedDuration;
		m_nCartHighSpeedDurationExtension = gameConfig.cartHighSpeedDurationExtension;
		m_nCartAccelCoolTime = gameConfig.cartAccelCoolTime;

		m_nWorldLevelExpBuffMinHeroLevel = gameConfig.worldLevelExpBuffMinHeroLevel;

		m_nNationTransmissionRequiredHeroLevel = gameConfig.nationTransmissionRequiredHeroLevel;
		m_flNationTransmissionExitXPosition = gameConfig.nationTransmissionExitXPosition;
		m_flNationTransmissionExitYPosition = gameConfig.nationTransmissionExitYPosition;
		m_flNationTransmissionExitZPosition = gameConfig.nationTransmissionExitZPosition;
		m_flNationTransmissionExitRadius = gameConfig.nationTransmissionExitRadius;
		m_flNationTransmissionExitYRotationType = gameConfig.nationTransmissionExitYRotationType;
		m_flNationTransmissionExitYRotation = gameConfig.nationTransmissionExitYRotation;
		m_nBountyHunterQuestMaxCount = gameConfig.bountyHunterQuestMaxCount;
		m_nBountyHunterQuestRequiredHeroLevel = gameConfig.bountyHunterQuestRequiredHeroLevel;

		m_nTodayMissionCount = gameConfig.todayMissionCount;
		m_nFieldOfHonorDisplayMaxRanking = gameConfig.fieldOfHonorDisplayMaxRanking;
		m_nFieldOfHonorDisplayHistoryCount = gameConfig.fieldOfHonorDisplayHistoryCount;
		m_nRankingDisplayMaxCount = gameConfig.rankingDisplayMaxCount;

		m_nGuildRequiredHeroLevel = gameConfig.guildRequiredHeroLevel;               
		m_nGuildCreationRequiredVipLevel = gameConfig.guildCreationRequiredVipLevel;        
		m_nGuildCreationRequiredDia = gameConfig.guildCreationRequiredDia;             
		m_nGuildRejoinIntervalTime = gameConfig.guildRejoinIntervalTime;              
		m_nGuildApplicationReceptionMaxCount = gameConfig.guildApplicationReceptionMaxCount;    
		m_nGuildDailyApplicationMaxCount = gameConfig.guildDailyApplicationMaxCount;        
		m_nGuildDailyBanishmentMaxCount = gameConfig.guildDailyBanishmentMaxCount;         
		m_nGuildInvitationLifetime = gameConfig.guildInvitationLifetime;              
		m_nGuildNoticeMaxLength = gameConfig.guildNoticeMaxLength;

		m_nGuildCallLifetime = gameConfig.guildCallLifetime;
		m_flGuildCallRadius = gameConfig.guildCallRadius;
		m_nNationCallLifetime = gameConfig.nationCallLifetime;
		m_flNationCallRadius = gameConfig.nationCallRadius;

		m_strGuildDailyObjectiveNoticeText = CsConfiguration.Instance.GetString(gameConfig.guildDailyObjectiveNoticeTextKey); 
		m_nGuildDailyObjectiveNoticeCoolTime = gameConfig.guildDailyObjectiveNoticeCoolTime;
		m_nDefaultGuildWeeklyObjectiveId = gameConfig.defaultGuildWeeklyObjectiveId;
		m_nGuildHuntingDonationMaxCount = gameConfig.guildHuntingDonationMaxCount;
		m_nGuildHuntingDonationItemId = gameConfig.guildHuntingDonationItemId;
		m_lGuildHuntingDonationItemRewardId = gameConfig.guildHuntingDonationItemRewardId;
		m_lGuildHuntingDonationCompletionItemRewardId = gameConfig.guildHuntingDonationCompletionItemRewardId;

		m_nSignBoardDisplayDuration = gameConfig.signBoardDisplayDuration;
		m_nNoticeBoardDisplayDuration = gameConfig.noticeBoardDisplayDuration;

		m_nCreatureCardShopRandomProductCount = gameConfig.creatureCardShopRandomProductCount;
		m_nCreatureCardShopPaidRefreshDia = gameConfig.creatureCardShopPaidRefreshDia;

		m_nGuideActivationRequiredHeroLevel = gameConfig.guideActivationRequiredHeroLevel;

		m_nAccelerationRequiredMoveDuration = gameConfig.accelerationRequiredMoveDuration;
		m_nAccelerationMoveSpeed = gameConfig.accelerationMoveSpeed;

		m_nSceneryQuestRequiredMainQuestNo = gameConfig.sceneryQuestRequiredMainQuestNo;
		m_nMenuContentOpenPreviewRequiredHeroLevel = gameConfig.menuContentOpenPreviewRequiredHeroLevel;

		m_nMonsterGroggyDuration = gameConfig.monsterGroggyDuration;
		m_nMonsterStealDuration = gameConfig.monsterStealDuration;

		m_nRookieGiftScratchOpenDuration = gameConfig.rookieGiftScratchOpenDuration;
		m_nOpenGiftRequiredHeroLevel = gameConfig.openGiftRequiredHeroLevel;

		m_nOpen7DayEventRequiredMainQuestNo = gameConfig.open7DayEventRequiredMainQuestNo;

        m_nTaskConsignmentRequiredVipLevel = gameConfig.taskConsignmentRequiredVipLevel;

		m_nWarehouseRequiredVipLevel = gameConfig.warehouseRequiredVipLevel;
		m_nFreeWarehouseSlotCount = gameConfig.freeWarehouseSlotCount;

		m_nWingMemoryPieceInstallationRequiredHeroLevel = gameConfig.wingMemoryPieceInstallationRequiredHeroLevel;
		m_nOrdealQuestSlotCount = gameConfig.ordealQuestSlotCount;

		m_nFriendMaxCount = gameConfig.friendMaxCount;
		m_nTempFriendMaxCount = gameConfig.tempFriendMaxCount;
		m_nDeadRecordMaxCount = gameConfig.deadRecordMaxCount;
		m_nBlacklistEntryMaxCount = gameConfig.blacklistEntryMaxCount;

		m_nBlessingQuestListMaxCount = gameConfig.blessingQuestListMaxCount;
		m_nBlessingQuestRequiredHeroLevel = gameConfig.blessingQuestRequiredHeroLevel;
		m_nBlessingListMaxCount = gameConfig.blessingListMaxCount;
		m_nOwnerProspectQuestListMaxCount = gameConfig.ownerProspectQuestListMaxCount;
		m_nTargetProspectQuestListMaxCount = gameConfig.targetProspectQuestListMaxCount;

		m_nCreatureMaxCount = gameConfig.creatureMaxCount;
		m_nCreatureCheerMaxCount = gameConfig.creatureCheerMaxCount;
		m_flCreatureCheerAttrFactor = gameConfig.creatureCheerAttrFactor;
		m_flCreatureEvaluationFactor = gameConfig.creatureEvaluationFactor;
		m_nCreatureAdditionalAttrCount = gameConfig.creatureAdditionalAttrCount;
		m_nCreatureSkillSlotMaxCount = gameConfig.creatureSkillSlotMaxCount;
		m_nCreatureSkillSlotBaseOpenCount = gameConfig.creatureSkillSlotBaseOpenCount;
		m_nCreatureCompositionExpRetrievalRate = gameConfig.creatureCompositionExpRetrievalRate;
		m_nCreatureCompositionExpRetrievalResultItemId = gameConfig.creatureCompositionExpRetrievalResultItemId;
		m_nCreatureCompositionSkillProtectionItemId = gameConfig.creatureCompositionSkillProtectionItemId;
		m_nCreatureInjectionExpRetrivalRate = gameConfig.creatureInjectionExpRetrievalRate;
		m_nCreatureVariationRequiredItemId = gameConfig.creatureVariationRequiredItemId;
		m_nCreatureAdditionalAttrSwitchRequiredItemId = gameConfig.creatureAdditionalAttrSwitchRequiredItemId;
		m_nCreatureReleaseExpRetrievalRate = gameConfig.creatureReleaseExpRetrievalRate;
		m_nParticipationCreatureDisplayRequiredVipLevel = gameConfig.participationCreatureDisplayRequiredVipLevel;

		m_nPresentPopularityPointRankingDisplayMaxCount = gameConfig.presentPopularityPointRankingDisplayMaxCount;
		m_nPresentContributionPointRankingDisplayMaxCount = gameConfig.presentContributionPointRankingDisplayMaxCount;

		m_nGuildBlessingGuildTerritoryNpcId = gameConfig.guildBlessingGuildTerritoryNpcId;

		m_lOpen7DayEventCostumeItemRewardId = gameConfig.open7DayEventCostumeItemRewardId;
		m_nOpen7DayEventCostumeRewardRequiredItemId = gameConfig.open7DayEventCostumeRewardRequiredItemId;
		m_nOpen7DayEventCostumeRewardRequiredItemCount = gameConfig.open7DayEventCostumeRewardRequiredItemCount;

		m_nNationAllianceUnavailableStartTime = gameConfig.nationAllianceUnavailableStartTime;
		m_nNationAllianceUnavailableEndTime = gameConfig.nationAllianceUnavailableEndTime;
		m_lNationAllianceRequiredFund = gameConfig.nationAllianceRequiredFund;
		m_nNationAllianceRenounceUnavailableDuration = gameConfig.nationAllianceRenounceUnavailableDuration;
		m_nNationBasePower = gameConfig.nationBasePower;

		m_nJobChangeRequiredHeroLevel = gameConfig.jobChangeRequiredHeroLevel;
		m_nJobChangeRequiredItemId = gameConfig.jobChangeRequiredItemId;
		m_nJobChangeQuestCompletionClientTutorialId = gameConfig.jobChangeQuestCompletionClientTutorialId;

		m_nChargeEventRequiredHeroLevel = gameConfig.chargeEventRequiredHeroLevel;
		m_nConsumeEventRequiredHeroLevel = gameConfig.consumeEventRequiredHeroLevel;

		m_nArtifactRequiredConditionType = gameConfig.artifactRequiredConditionType;
		m_nArtifactRequiredConditionValue = gameConfig.artifactRequiredConditionValue;
		m_nArtifactMaxLevel = gameConfig.artifactMaxLevel;
		m_nMountAwakeningRequiredHeroLevel = gameConfig.mountAwakeningRequiredHeroLevel;
		m_nMountAwakeningItemId = gameConfig.mountAwakeningItemId;
		m_nMountPotionAttrItemId = gameConfig.mountPotionAttrItemId;

		m_nCostumeEnchantItemId = gameConfig.costumeEnchantItemId;
		m_nCostumeCollectionActivationItemId = gameConfig.costumeCollectionActivationItemId;
		m_nCostumeCollectionShuffleItemId = gameConfig.costumeCollectionShuffleItemId;
		m_nCostumeCollectionShuffleItemCount = gameConfig.costumeCollectionShuffleItemCount;


	}

}
