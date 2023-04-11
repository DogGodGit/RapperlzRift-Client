using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-21)
//---------------------------------------------------------------------------------------------------

public class CsGameEventUIToUI
{
	public static CsGameEventUIToUI Instance
	{
		get { return CsSingleton<CsGameEventUIToUI>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventHeroLogin;
	public event Delegate<string> EventAlert;                                                                         // 메시지창오픈
	public event Delegate<string, string, UnityAction, string, UnityAction, bool> EventConfirm;                       // 메시지창오픈
	public event Delegate EventCommonModalClose;                                                                      // 메시지창닫기
	public event Delegate<EnTimerModalType, string, UnityAction, UnityAction, float> EventTimerConfirm;               // 타이머컨펌오픈
	public event Delegate<EnTimerModalType> EventTimerModalClose;                                                     // 타이머컨펌닫기

	public event Delegate<CsItem> EventOpenPopupItemInfo;								// 아이템 정보창
	public event Delegate<EnMenuId> EventSinglePopupOpen;								// 보상팝업오픈(오픈선물,신규선물)
	public event Delegate EventOpenPopupFieldBoss;										// 필드보스 팝업 오픈
	public event Delegate<EnMainMenu> EventOpenPopupHelp;								// 팝업메인 도움말 팝업
	public event Delegate<bool> EventOpenPopupCharging;									// 충전 팝업
	public event Delegate EventOpenPopupSmallAmountCharging;							// 소액 충전 팝업
	public event Delegate<EnMainMenu, EnSubMenu, CsHeroInfo> EventPopupOpen;            // 팝업오픈
	public event Delegate EventPopupClose;                                              // 팝업종료
	public event Delegate<EnToastType, string> EventToastMessage;                       // 토스트
	public event Delegate<EnToastType, string, string, bool> EventToastChangeArea;      // 지역 이동 토스트
	public event Delegate<long> EventToastChangeBattlePower;                            // 전투력 변경 토스트
	public event Delegate EventMyHeroLevelUp;                                           // 레벨업 시 컨텐츠 오픈 및 도달보상체크
	public event Delegate EventMyHeroExpUp;                                             // 경험치 얻었을 경우.
	public event Delegate<string> EventToastSystem;                                     // 시스템 토스트
	public event Delegate EventDateChanged;                                             // 날짜바뀜
	public event Delegate<bool> EventVisibleSection;                                    // 섹션 on/off
	public event Delegate EventDeleteAllHUD;                                            // 모든HUD삭제
	public event Delegate EventCloseAllPopup;                                           // 모든 팝업 종료
	public event Delegate<bool> EventVisibleMainUI;                                     // MainUI 보이기/감추기
	public event Delegate<EnAutoStateType> EventAutoCancelButtonOpen;                   // 자동이동 취소버튼
	public event Delegate EventLoadingSliderComplete;
	public event Delegate<EnPlayerPrefsKey, int> EventPlayerPrefsKeySet;                // UI 옵션 조정
	public event Delegate<string> EventQuestCompltedError;                              // 서버이벤트 오류에 대한 처리
	public event Delegate<bool> EventDisplayJoystickEffect;								// 조이스틱 이펙트

	// 메인장비
	public event Delegate<Guid> EventMainGearEquip;                 // 메인장비장착
	public event Delegate<Guid> EventMainGearUnequip;               // 메인장비장착해체
	public event Delegate<bool, Guid> EventMainGearEnchant;         // 메인장비강화
	public event Delegate<Guid, Guid> EventMainGearTransit;         // 메인장비전이
	public event Delegate<Guid> EventMainGearRefine;                // 메인장비세련
	public event Delegate<Guid> EventMainGearRefinementApply;       // 메인장비세련적용
	public event Delegate<Guid> EventMainGearSelected;              // [UI]메인장비리스트에서 선택시
	public event Delegate EventMainGearDisassemble;                 // 메인장비분해
	public event Delegate EventMainGearEnchantLevelSetActivate;     // 메인장비강화레벨세트활성	

	// 보조장비
	public event Delegate<int> EventSubGearEquip;                   // 보조장비장착
	public event Delegate<int> EventSubGearUnequip;                 // 보조장비장착해체
	public event Delegate<int> EventSoulstoneSocketMount;           // 소울스톤소켓장착
	public event Delegate<int> EventSoulstoneSocketUnmount;         // 소울스톤소켓장착해제 		
	public event Delegate<int> EventRuneSocketMount;                // 룬소켓장착
	public event Delegate<int> EventRuneSocketUnmount;              // 룬소켓장착해제	
	public event Delegate<int> EventSubGearLevelUp;                 // 보조장비레벨업
	public event Delegate<int> EventSubGearLevelUpTotally;          // 보조장비전체레벨업
	public event Delegate<int> EventSubGearGradeUp;                 // 보조장비등급업
	public event Delegate<int> EventSubGearQualityUp;               // 보조장비품질업
	public event Delegate<int, int> EventMountedSoulstoneCompose;   // 장착된소울스톤합성
	public event Delegate<int> EventSubGearSelected;                // [UI]보조장비리스트에서 선택시
	public event Delegate EventSubGearSoulstoneLevelSetActivate;    // 보조장비소울스톤레벨세트활성	

	// 메일
	public event Delegate<Guid> EventMailReceive;                   // 메일받기
	public event Delegate<Guid[]> EventMailReceiveAll;              // 메일전체받기
	public event Delegate<Guid> EventMailDelete;                    // 메일삭제
	public event Delegate EventMailDeleteAll;                       // 메일전체삭제				
	public event Delegate<Guid> EventNewMail;                       // 신규메일
	public event Delegate<Guid> EventMailSelected;                  // [UI]메일리스트에서 선택시

	// 라크
	public event Delegate EventLakAcquisition;                      // 라크획득
	// 경험치
	public event Delegate<long, bool> EventExpAcquisition;          // 경험치획득
	public event Delegate<long> EventExpAcuisitionText;             // 경험치 획득 텍스트

	// 스킬
	public event Delegate<int> EventSkillLevelUp;                   // 스킬레벨업		
	public event Delegate<int> EventSkillLevelUpTotally;            // 스킬전체레벨업		
	public event Delegate<int> EventSkillSelected;                  // [UI]스킬리스트에서 선택시

	// 간이상점
	public event Delegate EventSimpleShopBuy;                       // 간이상점구입
	public event Delegate EventSimpleShopSell;                      // 간이상점판매

	// 인벤토리
	public event Delegate EventInventorySlotExtend;                 // 인벤토리슬롯확장
	public event Delegate<CsHeroObject> EventGearShare;             // 장비공유

	// 아이템
	public event Delegate EventItemCompose;                         // 아이템합성
	public event Delegate EventItemComposeTotally;                  // 아이템전체합성	
	public event Delegate<int> EventHpPotionUse;                    // 물약사용
	public event Delegate<int> EventReturnScrollUseFinished;        // 귀환주문서사용완료 
	public event Delegate EventReturnScrollUseCancel;               // 귀환주문서사용취소
	public event Delegate<bool> EventPickBoxUse;                    // 뽑기상자사용
	public event Delegate<bool> EventMainGearBoxUse;                // 메인장비상자사용
	public event Delegate<bool, long> EventExpPotionUse;            // 경험치물약사용
	public event Delegate EventExpScrollUse;                        // 경험치주문서사용
	public event Delegate EventBountyHunterQuestScrollUse;          // 현상금사냥꾼퀘스트주문서사용
	public event Delegate EventFishingBaitUse;                      // 낚시미끼사용
	public event Delegate EventDistortionScrollUse;                 // 왜곡주문서사용
	public event Delegate EventDistortionCanceled;                  // 왜곡취소
	public event Delegate EventGoldItemUse;                         // 골드아이템사용
	public event Delegate EventOwnDiaItemUse;                       // 귀속다이아아이템사용
	public event Delegate EventHonorPointItemUse;                   // 명예포인트아이템사용
	public event Delegate EventExploitPointItemUse;                 // 공적포인트아이템사용
	public event Delegate EventWingItemUse;                         // 날개아이템사용
	public event Delegate<int> EventSpiritStoneItemUse;				// 영혼석아이템사용
	
	// 휴식
	public event Delegate<bool, long> EventRestRewardReceiveFree;   // 무료휴식보상받기
	public event Delegate<bool, long> EventRestRewardReceiveGold;   // 골드휴식보상받기
	public event Delegate<bool, long> EventRestRewardReceiveDia;    // 다이아휴식보상받기

	// 부활
	public event Delegate EventImmediateRevive;                     // 즉시부활
	public event Delegate<int> EventContinentSaftyRevive;           // 안전부활

	// 귀환
	public event Delegate EventReturnScrollUseStart;                // 귀환서

	// 아이템루팅
	public event Delegate<List<CsDropObject>, List<CsDropObject>> EventDropObjectLooted;    // 아이템루팅

	// 파티
	public event Delegate<CsSimpleHero[]> EventPartySurroundingHeroList;    // 주변영웅목록
	public event Delegate<CsSimpleParty[]> EventPartySurroundingPartyList;  // 주변파티목록
	public event Delegate EventPartyCreate;                                 // 파티생성
	public event Delegate EventPartyExit;                                   // 파티탈퇴
	public event Delegate<Guid> EventPartyMemberBanish;                     // 파티멤버강퇴
	public event Delegate EventPartyCall;                                   // 파티소집
	public event Delegate EventPartyDisband;                                // 파티해산
	public event Delegate<CsPartyApplication> EventPartyApply;              // 파티신청
	public event Delegate<CsPartyMember> EventPartyApplicationAccept;       // 파티신청수락
	public event Delegate EventPartyApplicationRefuse;                      // 파티신청거절
	public event Delegate<CsPartyInvitation> EventPartyInvite;              // 파티초대		
	public event Delegate EventPartyInvitationAccept;                       // 파티초대수락
	public event Delegate EventPartyInvitationRefuse;                       // 파티초대거절
	public event Delegate EventPartyMasterChange;                           // 파티마스터변경
	public event Delegate EventHeroPosition;                                // 파티집합

	public event Delegate<CsPartyApplication> EventPartyApplicationArrived; // 파티신청도착
	public event Delegate EventPartyApplicationCanceled;                    // 파티신청취소	
	public event Delegate EventPartyApplicationAccepted;                    // 파티신청수락
	public event Delegate EventPartyApplicationRefused;                     // 파티신청거절
	public event Delegate EventPartyApplicationLifetimeEnded;               // 파티신청수명종료

	public event Delegate<CsPartyInvitation> EventPartyInvitationArrived;   // 파티초대도착
	public event Delegate EventPartyInvitationCanceled;                     // 파티초대취소
	public event Delegate EventPartyInvitationAccepted;                     // 파티초대수락
	public event Delegate EventPartyInvitationRefused;                      // 파티초대거절
	public event Delegate EventPartyInvitationLifetimeEnded;                // 파티초대수명종료

	public event Delegate<CsPartyMember> EventPartyMemberEnter;             // 파티멤버입장
	public event Delegate<CsPartyMember, bool> EventPartyMemberExit;        // 파티멤버퇴장
	public event Delegate EventPartyBanished;                               // 파티강퇴
	public event Delegate EventPartyMasterChanged;                          // 파티장변경
	public event Delegate<int, int, Vector3> EventPartyCalled;              // 파티소집		
	public event Delegate EventPartyDisbanded;                              // 파티해산
	public event Delegate EventPartyMembersUpdated;                         // 파티멤버갱신
	public event Delegate EventPartySurroundingHeroListRequest;             // 주변영웅목록요청	
	public event Delegate EventPartySurroundingPartyListRequest;            // 주변파티목록요청

	// 채팅
	public event Delegate EventChattingMessageSend;                         // 채팅메시지발송
	public event Delegate<CsChattingMessage> EventChattingMessageReceived;  // 채팅메시지수신

	// 메인장비, 보조장비, 아이템 빠른 사용
	public event Delegate<List<CsHeroObject>> EventHeroObjectGot;           // 영웅장비아이템획득

	// 지원
	public event Delegate EventLevelUpRewardReceive;                                // 레벨업보상받기
	public event Delegate EventDailyAccessTimeRewardReceive;                        // 일일접속시간보상받기
	public event Delegate EventAttendRewardReceive;                                 // 출석보상받기
	public event Delegate<CsHeroSeriesMission> EventSeriesMissionRewardReceive;     // 연속미션보상받기
	public event Delegate<CsHeroTodayMission> EventTodayMissionRewardReceive;       // 오늘의미션보상받기
	public event Delegate<CsHeroSeriesMission> EventSeriesMissionUpdated;           // 연속미션갱신
	public event Delegate EventTodayMissionListChanged;                             // 오늘의미션목록변경
	public event Delegate<CsHeroTodayMission> EventTodayMissionUpdated;             // 오늘의미션갱신
	public event Delegate EventRookieGiftReceive;									// 신병선물받기
	public event Delegate<int> EventOpenGiftReceive;								// 오픈선물받기

	// 탈것
	public event Delegate EventMountEquip;                                  // 탈것장착
	public event Delegate<bool> EventMountLevelUp;                          // 탈것레벨업
	public event Delegate<bool> EventDisplayMountToggleIsOn;                // 탈것토글 켜기/끄기
	public event Delegate EventMountAwakeningLevelUp;
	public event Delegate EventMountAttrPotionUse;
    public event Delegate EventMountItemUse; 
    public event Delegate<int> EventMountSelected;                          // 탈것선택
    public event Delegate<int> EventOpenPopupMountAttrPotion;

	// 탈것장비
	public event Delegate<Guid> EventMountGearEquip;                        // 탈것장비장착
	public event Delegate<Guid> EventMountGearUnequip;                      // 탈것장비장착해제
	public event Delegate<Guid> EventMountGearRefine;                       // 탈것장비재강화
	public event Delegate EventMountGearPickBoxMake;                        // 탈것장비뽑기상자제작
	public event Delegate EventMountGearPickBoxMakeTotally;                 // 탈것장비뽑기상자모두제작

	//퀘스트 리스트
	public event Delegate<EnQuestType, int, bool> EventSelectQuest;            // 리스트에서 퀘스트 선택
	public event Delegate<EnQuestType, bool, int> EventDisplayQuestPanel;      // 패널에 있는 퀘스트 보이기/감추기
	public event Delegate<EnQuestCategoryType, int> EventStartQuestAutoPlay;   // 현재 선택된 퀘스트 자동 시작하기
	public event Delegate EventAcceptableSubQuestEmpty;						// 수령 가능한 서브 퀘스트가 없는 경우

	//유저 조회창
	public event Delegate<Guid> EventOpenOneToOneChat;                          // 조회중인 유저와의 1:1 대화창 오픈

	// 날개
	public event Delegate EventWingEquip;                                       // 날개장착
	public event Delegate EventWingEnchant;                                     // 날개강화
	public event Delegate EventWingEnchantTotally;                              // 날개전체강화
	public event Delegate EventWingAcquisition;                                 // 날개획득	
	public event Delegate EventWingMemoryPieceInstall;                          // 날개기억조각장착

	// 스토리던전
	public event Delegate<string> EventContinentExitForStoryDungeonEnter;       // 스토리던전입장을위한대륙퇴장
	public event Delegate<int> EventStoryDungeonAbandon;                        // 스토리던전포기
	public event Delegate<int> EventStoryDungeonExit;                           // 스토리던전퇴장 

	// 스테미나
	public event Delegate EventStaminaAutoRecovery;                             // 스태미나자동회복
	public event Delegate EventStaminaBuy;                                      // 체력구매
	public event Delegate EventStaminaScheduleRecovery;                         // 스태미나스케쥴회복

	// 영웅
	public event Delegate<CsHeroInfo> EventHeroInfo;                            // 영웅정보 조회

	// 던전 팝업
	public event Delegate<int> EventSelectDungeonCartegory;                     // 던전 선택.
	public event Delegate EventGoBackDungeonCartegoryList;                      // 리스트 창으로 돌아가기

	//맵
	public event Delegate<int> EventMiniMapSelected;                            //[UI]맵 선택시

	// 대륙전송
	public event Delegate<string> EventContinentTransmission;                   // 대륙전송	

	// 국가
	public event Delegate<string> EventNationTransmission;                      // 국가전송
	public event Delegate<List<CsSearchHero>> EventHeroSearch;                  // 영웅검색
	public event Delegate<int> EventNationDonate;                               // 국가기부
	public event Delegate EventNationNoblesseAppoint;                           // 국가관직임명
	public event Delegate EventNationNoblesseDismiss;                           // 국가관직해임
	public event Delegate<int, string> EventNationNoblesseAppointed;
	public event Delegate EventNationNoblesseDismissed;
	public event Delegate EventNationFundChanged;
	public event Delegate EventNationCall;
	public event Delegate<int> EventNationCallTransmission;
	public event Delegate<CsNationCall> EventNationCalled;

	// 낚시자동이동 팝업
	public event Delegate EventAutoMoveFishingZone;                             // 낚시터 이동

	// 오늘의 할일
	public event Delegate EventAchievementRewardReceive;                        // 달성보상받기
	public event Delegate EventTodayTaskUpdated;                                // 오늘의할일갱신

	// 매칭던전
	public event Delegate EventOpenPopupMatching;                               // 매칭팝업 오픈

	public event Delegate EventVipLevelRewardReceive;                           // Vip레벨보상받기

	// 계급
	public event Delegate EventRankAcquire;                                     // 계급획득
	public event Delegate EventRankRewardReceive;                               // 계급보상받기
	public event Delegate EventRankActiveSkillLevelUp;                          // 계급액티브스킬레벨업
	public event Delegate EventRankActiveSkillSelect;                           // 계급액티브스킬선택
	public event Delegate EventRankPassiveSkillLevelUp;                         // 계급패시브스킬레벨업

	// 명예상점
	public event Delegate EventHonorShopProductBuy;                             // 명예상점상품구매

	// 랭킹
	public event Delegate<CsRanking, List<CsRanking>> EventServerBattlePowerRanking;
	public event Delegate<CsRanking, List<CsRanking>> EventServerJobBattlePowerRanking;
	public event Delegate<CsRanking, List<CsRanking>> EventServerLevelRanking;
	public event Delegate EventServerLevelRankingRewardReceive;
	public event Delegate<CsRanking, List<CsRanking>> EventNationBattlePowerRanking;
	public event Delegate<CsRanking, List<CsRanking>> EventNationExploitPointRanking;
	public event Delegate EventDailyServerLevelRankingUpdated;
	public event Delegate<CsCreatureCardRanking, List<CsCreatureCardRanking>> EventServerCreatureCardRanking;
	public event Delegate<CsIllustratedBookRanking, List<CsIllustratedBookRanking>> EventServerIllustratedBookRanking;
	public event Delegate EventDailyServerNationPowerRankingUpdated;

	// 도달
	public event Delegate EventAttainmentRewardReceive;                         // 도달보상받기

	//퀘스트 자동이동
	public event Delegate<EnAutoStateType, int> EventAutoQuestStart;                 //퀘스트 자동이동

	// 유저 조회
	public event Delegate<CsHeroBase> EventOpenUserReference;                   // 타유져 조회 팝업 오픈
	public event Delegate<CsFriend> EventOpenFriendRefernce;
	public event Delegate<CsGuildMember> EventOpenGuildMemberReference;         // 길드 유저 조회 팝업 오픈
	public event Delegate<CsNationNoblesseInstance> EventOpenNationNoblesseReference;   // 국가 관직 유저 조회 팝업 오픈

	// 대륙
	public event Delegate<int> EventContinentBanished;                          // 대륙강퇴

	// 튜토리얼 종료
	public event Delegate EventTutorialEnd;                                     // 튜토리얼 종료
	public event Delegate<EnTutorialType> EventReferenceTutorial;               // 참고 튜토리얼0

	// 메인버튼업데이트.
	public event Delegate<int, int, bool> EventMainButtonUpdate;
	public event Delegate<bool> EventMenuOpenButtonNoticeUpdate;
	public event Delegate<bool> EventLeftTopMenuOpenButtonNoticeUpdate;

	public event Delegate EventOpenPopupNationWarResult;

	// 세팅
	public event Delegate EventBattleSettingSet;

	// 공지
	public event Delegate<string> EventNotice;

	// 오늘의미션튜토리얼시작
	public event Delegate EventTodayMissionTutorialStart;

	// 서버최고레벨갱신
	public event Delegate EventServerMaxLevelUpdated;

	// 크리처카드획득
	public event Delegate<List<CsHeroCreatureCard>> EventGetHeroCreatureCard;

	// 크리쳐 획득
	public event Delegate<List<CsHeroCreature>> EventGetHeroCreature;

	// 네트웍, 배터리
	public event Delegate<string, string> EventNetworkStatus;
	public event Delegate<string, string> EventBatteryStatus;

	// 그로기 몬스터
	public event Delegate EventGroggyMonsterItemStealStart;
	public event Delegate EventGroggyMonsterItemStealCancel;
	public event Delegate<CsItem, bool, int> EventGroggyMonsterItemStealFinished;

	//  NPC상점상품구입
	public event Delegate EventNpcShopProductBuy;

	// 오픈7일이벤트
	public event Delegate EventOpen7DayEventMissionRewardReceive;
	public event Delegate EventOpen7DayEventProductBuy;
	public event Delegate EventOpen7DayEventRewardReceive;
	public event Delegate EventOpen7DayEventProgressCountUpdated;

	// 회수
	public event Delegate<bool, long> EventRetrieveGold;
	public event Delegate<bool, long> EventRetrieveGoldAll;
	public event Delegate<bool, long> EventRetrieveDia;
	public event Delegate<bool, long> EventRetrieveDiaAll;
	public event Delegate EventRetrievalProgressCountUpdated;

	// 할일 위탁
	public event Delegate EventTaskConsignmentStart;
	public event Delegate<bool, long> EventTaskConsignmentComplete;
	public event Delegate<bool, long> EventTaskConsignmentImmediatelyComplete;

	// 한정선물
	public event Delegate EventLimitationGiftRewardReceive;

	// 주말보상
	public event Delegate EventWeekendRewardSelect;
	public event Delegate EventWeekendRewardReceive;

	// 창고
	public event Delegate EventWarehouseDeposit;
	public event Delegate EventWarehouseWithdraw;
	public event Delegate EventWarehouseSlotExtend;

	// 다이아상점상품구매
	public event Delegate EventDiaShopProductBuy;

	// 공포의 제단
	public event Delegate<bool> EventOpenPopupHalidomCollection;
	public event Delegate EventFearAltarHalidomElementalRewardReceive;
	public event Delegate EventFearAltarHalidomCollectionRewardReceive;
	public event Delegate<int> EventFearAltarHalidomAcquisition;

	// 시련 퀘스트
	public event Delegate EventOpenPopupOrdealQuest;									// 시련 퀘스트
	public event Delegate<bool, long> EventOrdealQuestSlotComplete;
	public event Delegate<bool, long> EventOrdealQuestComplete;
	public event Delegate EventOrdealQuestAccepted;
	public event Delegate EventOrdealQuestSlotProgressCountsUpdated;
	public event Delegate<CsOrdealQuestMission> EventStartOrdealQuestMission;

	// 전기 퀘스트
	public event Delegate EventClosePanelBiography;
	public event Delegate<int> EventOpenPanelBiographyComplete;
	public event Delegate<CsBiographyQuest> EventBiographyNpcDialog;
	public event Delegate<int> EventSelectToggleBiography;								// 전기 토글 선택

	// 포탈
	public event Delegate EventPortalEnter;

	// 계정중복로그인
	public event Delegate EventAccountLoginDuplicated;

	// 친구
	public event Delegate<bool> EventFriendAdd;
	public event Delegate EventSelectFriendDelete;

	// 절전
	public event Delegate EventSleepModeReset;

	public event Delegate EventContinueNextQuest;
	public event Delegate EventOnClickPanelDialogAccept;
	public event Delegate EventOnClickPanelDialogCancel;
	
	// 선물 오픈
	public event Delegate<Guid> EventOpenPopupPresent;
    public event Delegate EventCheckPresentButtonDisplay;

	// 크리처
	public event Delegate<Guid> OpenPopupGetCreature;				// 크리처 획득

	public event Delegate<Guid> EventSelectCreatureToggle;			// 크리처목록 토글전환
	public event Delegate EventCreatureSubMenuChanged;				// 크리처 서브메뉴 체인지
	public event DelegateR<Guid> EventGetSelectedCreature;			// 선택한 크리처
	public event Delegate<int> EventPointerDownCreatureFood;		// 크리처 음식 포인터 이벤트
	public event Delegate EventPointerUpCreatureFood;				// 크리처 음식 포인터 이벤트
	public event Delegate EventPointerExitCreatureFood;				// 크리처 음식 포인터 이벤트
	public event Delegate<int> EventPointerDownCreatureSkill;		// 크리처 스킬 포인터 이벤트
	public event Delegate EventPointerUpCreatureSkill;				// 크리처 스킬 포인터 이벤트
	public event Delegate EventPointerExitCreatureSkill;			// 크리처 스킬 포인터 이벤트

    // 길드 축복
    public event Delegate EventOpenPopupGuildBlessing;              // 길드 축복

	// 물약속성
	public event Delegate EventHeroAttrPotionUse;
	public event Delegate EventHeroAttrPotionUseAll;

	public event Delegate EventMainQuestNpcDialog;

	// 아티펙트
	public event Delegate<int> EventInventoryLongClick;

	// System Message
	public event Delegate<CsSystemMessage> EventSystemMessage;

    public event Delegate EventOpenPopupHeroPotionAttr;
    public event Delegate EventOpenPopupCostumeEffect;
    public event Delegate EventOpenPopupCostumeEnchant;

    public event Delegate<string> EventScheduleNotice;
    public event Delegate EventHeroSkillReset;

	// 웹뷰
	public event Delegate<string> EventOpenWebView;

    public event Delegate<EnBattleMode> EventChangeAutoBattleMode;

	//----------------------------------------------------------------------------------------------------
	// 영웅로그인
	public void OnEventHeroLogin()
	{
		if (EventHeroLogin != null)
		{
			EventHeroLogin();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 메시지창오픈
	public void OnEventAlert(string strMessage)
	{
		if (EventAlert != null)
		{
			EventAlert(strMessage);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 컨펌
	public void OnEventConfirm(string strMessage, string strButton1, UnityAction unityAction1, string strButton2, UnityAction unityAction2, bool bClose)
	{
		if (EventConfirm != null)
		{
			EventConfirm(strMessage, strButton1, unityAction1, strButton2, unityAction2, bClose);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 타이머컨펌오픈
	public void OnEventTimerConfirm(EnTimerModalType enTimerModalType, string strMessage, UnityAction unityAction1, UnityAction unityAction2, float flTime = 0)
	{
		if (EventTimerConfirm != null)
		{
			EventTimerConfirm(enTimerModalType, strMessage, unityAction1, unityAction2, flTime);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 타이머컨펌닫기
	public void OnEventTimerModalClose(EnTimerModalType enTimerModalType)
	{
		if (EventTimerModalClose != null)
		{
			EventTimerModalClose(enTimerModalType);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 메시지창닫기
	public void OnEventCommonModalClose()
	{
		if (EventCommonModalClose != null)
		{
			EventCommonModalClose();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 토스트
	public void OnEventToastMessage(EnToastType enToastType, string strMessage)
	{
		if (EventToastMessage != null)
		{
			EventToastMessage(enToastType, strMessage);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 지역이동 토스트
	public void OnEventToastChangeArea(EnToastType enToastType, string strMessage1, string strMessage2, bool bMyNation = true)
	{
		if (EventToastChangeArea != null)
		{
			EventToastChangeArea(enToastType, strMessage1, strMessage2, bMyNation);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 전투력 변경 토스트
	public void OnEventToastChangeBattlePower(long lOldBattlePower)
	{
		if (EventToastChangeBattlePower != null)
		{
			EventToastChangeBattlePower(lOldBattlePower);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 레벨업  - 컨텐츠 오픈 토스트 , 도달보상
	public void OnEventMyHeroLevelUp()
	{
		if (EventMyHeroLevelUp != null)
		{
			EventMyHeroLevelUp();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 경험치 상승
	public void OnEventMyHeroExpUp()
	{
		if (EventMyHeroExpUp != null)
		{
			EventMyHeroExpUp();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventContinueNextQuest()
	{
		if (EventContinueNextQuest != null)
		{
			EventContinueNextQuest();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOnClickPanelDialogAccept()
	{
		if (EventOnClickPanelDialogAccept != null)
		{
			EventOnClickPanelDialogAccept();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOnClickPanelDialogCancel()
	{
		if (EventOnClickPanelDialogCancel != null)
		{
			EventOnClickPanelDialogCancel();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 시스템 토스트
	public void OnEventToastSystem(string strMessage)
	{
		if (EventToastSystem != null)
		{
			EventToastSystem(strMessage);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날짜바뀜
	public void OnEventDateChanged()
	{
		if (EventDateChanged != null)
		{
			EventDateChanged();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventVisibleSection(bool bVisible)
	{
		if (EventVisibleSection != null)
		{
			EventVisibleSection(bVisible);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 모든HUD삭제
	public void OnEventDeleteAllHUD()
	{
		if (EventDeleteAllHUD != null)
		{
			EventDeleteAllHUD();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 모든 팝업 종료
	public void OnEventCloseAllPopup()
	{
		if (EventCloseAllPopup != null)
		{
			EventCloseAllPopup();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 모든 팝업 종료
	public void OnEventVisibleMainUI(bool bIsOn)
	{
		if (EventVisibleMainUI != null)
		{
			EventVisibleMainUI(bIsOn);
		}
	}

	//----------------------------------------------------------------------------------------------------
	//자동이동버튼취소 활성화
	public void OnEventAutoCancelButtonOpen(EnAutoStateType enAutoStateType)
	{
		if (EventAutoCancelButtonOpen != null)
		{
			EventAutoCancelButtonOpen(enAutoStateType);
		}
	}

	//----------------------------------------------------------------------------------------------------
	//로딩완료
	public void OnEventLoadingSliderComplete()
	{
		if (EventLoadingSliderComplete != null)
		{
			EventLoadingSliderComplete();
		}
	}

	//---------------------------------------------------------------------------------------------------
	//설정 변경
	public void OnEventPlayerPrefsKeySet(EnPlayerPrefsKey enPlayerPrefsKey, int nValue)
	{
		if (EventPlayerPrefsKeySet != null)
		{
			EventPlayerPrefsKeySet(enPlayerPrefsKey, nValue);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서버이벤트 오류에 대한 처리
	public void OnEventQuestCompltedError(string strErrorMessage)
	{
		if (EventQuestCompltedError != null)
		{
			EventQuestCompltedError(strErrorMessage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 자동 이동 가능 상태일 때 조이스틱 이펙트 표시
	public void OnEventDisplayJoystickEffect(bool bEnable)
	{
		if (EventDisplayJoystickEffect != null)
		{
			EventDisplayJoystickEffect(bEnable);
		}
	}

	#region Popup

	//----------------------------------------------------------------------------------------------------
	// 아이템 정보창
	public void OnEventOpenPopupItemInfo(CsItem csItem)
	{
		if (EventOpenPopupItemInfo != null)
		{
			EventOpenPopupItemInfo(csItem);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 보상팝업오픈(오픈선물,신규선물)
	public void OnEventSinglePopupOpen(EnMenuId enMenuId)
	{
		if (EventSinglePopupOpen != null)
		{
			EventSinglePopupOpen(enMenuId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 필드보스 팝업 오픈
	public void OnEventOpenPopupFieldBoss()
	{
		if (EventOpenPopupFieldBoss != null)
		{
			EventOpenPopupFieldBoss();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 팝업메인 도움말 팝업
	public void OnEventOpenPopupHelp(EnMainMenu enMainMenu)
	{
		if (EventOpenPopupHelp != null)
		{
			EventOpenPopupHelp(enMainMenu);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 충전 팝업
	public void OnEventOpenPopupCharging(bool bGetProductsFinished = false)
	{
		if (EventOpenPopupCharging != null)
		{
			EventOpenPopupCharging(bGetProductsFinished);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 소액 충전 팝업
	public void OnEventOpenPopupSmallAmountCharging()
	{
		if (EventOpenPopupSmallAmountCharging != null)
		{
			EventOpenPopupSmallAmountCharging();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 시련퀘스트 팝업 오픈
	public void OnEventOpenPopupOrdealQuest()
	{
		if (EventOpenPopupOrdealQuest != null)
		{
			EventOpenPopupOrdealQuest();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventSelectToggleBiography(int nBiographyId)
	{
		if (EventSelectToggleBiography != null)
		{
			EventSelectToggleBiography(nBiographyId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 팝업오픈 디폴트값으로
	public void OnEventPopupOpenDefault(EnMainMenu enMainMenu)
	{
		OnEventPopupOpen(enMainMenu, EnSubMenu.Default);
	}

	//----------------------------------------------------------------------------------------------------
	// 팝업오픈
	public void OnEventPopupOpen(EnMainMenu enMainMenu, EnSubMenu enSubMenu, CsHeroInfo csHeroInfo = null)
	{
		if (EventPopupOpen != null)
		{
			EventPopupOpen(enMainMenu, enSubMenu, csHeroInfo);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 팝업종료
	public void OnEventPopupClose()
	{
		if (EventPopupClose != null)
		{
			EventPopupClose();
		}
	}

	#endregion Popup

	#region MainGear
	//---------------------------------------------------------------------------------------------------
	// 메인장비장착
	public void OnEventMainGearEquip(Guid guidHeroGearId)
	{
		if (EventMainGearEquip != null)
		{
			EventMainGearEquip(guidHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비장착해체
	public void OnEventMainGearUnequip(Guid guidHeroGearId)
	{
		if (EventMainGearUnequip != null)
		{
			EventMainGearUnequip(guidHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비강화
	public void OnEventMainGearEnchant(bool bSuccess, Guid guidHeroGearId)
	{
		if (EventMainGearEnchant != null)
		{
			EventMainGearEnchant(bSuccess, guidHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비전이
	public void OnEventMainGearTransit(Guid guidTargetHeroGearId, Guid guidMaterialHeroGearId)
	{
		if (EventMainGearTransit != null)
		{
			EventMainGearTransit(guidTargetHeroGearId, guidMaterialHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비세련
	public void OnEventMainGearRefine(Guid guidHeroGearId)
	{
		if (EventMainGearRefine != null)
		{
			EventMainGearRefine(guidHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비세련적용
	public void OnEventMainGearRefinementApply(Guid guidHeroGearId)
	{
		if (EventMainGearRefinementApply != null)
		{
			EventMainGearRefinementApply(guidHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비리스트에서 선택시
	public void OnEventMainGearSelected(Guid guidHeroGearId)
	{
		if (EventMainGearSelected != null)
		{
			EventMainGearSelected(guidHeroGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비분해
	public void OnEventMainGearDisassemble()
	{
		if (EventMainGearDisassemble != null)
		{
			EventMainGearDisassemble();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인장비강화레벨세트활성
	public void OnEventMainGearEnchantLevelSetActivate()
	{
		if (EventMainGearEnchantLevelSetActivate != null)
		{
			EventMainGearEnchantLevelSetActivate();
		}
	}

	#endregion MainGear

	#region SubGear
	//---------------------------------------------------------------------------------------------------
	// 보조장비장착
	public void OnEventSubGearEquip(int nSubGearId)
	{
		if (EventSubGearEquip != null)
		{
			EventSubGearEquip(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비장착해체
	public void OnEventSubGearUnequip(int nSubGearId)
	{
		if (EventSubGearUnequip != null)
		{
			EventSubGearUnequip(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 소울스톤소켓장착
	public void OnEventSoulstoneSocketMount(int nSubGearId)
	{
		if (EventSoulstoneSocketMount != null)
		{
			EventSoulstoneSocketMount(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 소울스톤소켓장착해제 	
	public void OnEventSoulstoneSocketUnmount(int nSubGearId)
	{
		if (EventSoulstoneSocketUnmount != null)
		{
			EventSoulstoneSocketUnmount(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 룬소켓장착
	public void OnEventRuneSocketMount(int nSubGearId)
	{
		if (EventRuneSocketMount != null)
		{
			EventRuneSocketMount(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 룬소켓장착해제	
	public void OnEventRuneSocketUnmount(int nSubGearId)
	{
		if (EventRuneSocketUnmount != null)
		{
			EventRuneSocketUnmount(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비레벨업
	public void OnEventSubGearLevelUp(int nSubGearId)
	{
		if (EventSubGearLevelUp != null)
		{
			EventSubGearLevelUp(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비전체레벨업
	public void OnEventSubGearLevelUpTotally(int nSubGearId)
	{
		if (EventSubGearLevelUpTotally != null)
		{
			EventSubGearLevelUpTotally(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비등급업
	public void OnEventSubGearGradeUp(int nSubGearId)
	{
		if (EventSubGearGradeUp != null)
		{
			EventSubGearGradeUp(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비품질업
	public void OnEventSubGearQualityUp(int nSubGearId)
	{
		if (EventSubGearQualityUp != null)
		{
			EventSubGearQualityUp(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 장착된소울스톤합성
	public void OnEventMountedSoulstoneCompose(int nSubGearId, int nSocketIndex)
	{
		if (EventMountedSoulstoneCompose != null)
		{
			EventMountedSoulstoneCompose(nSubGearId, nSocketIndex);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비리스트에서 선택시
	public void OnEventSubGearSelected(int nSubGearId)
	{
		if (EventSubGearSelected != null)
		{
			EventSubGearSelected(nSubGearId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보조장비소울스톤레벨세트활성
	public void OnEventSubGearSoulstoneLevelSetActivate()
	{
		if (EventSubGearSoulstoneLevelSetActivate != null)
		{
			EventSubGearSoulstoneLevelSetActivate();
		}
	}

	#endregion SubGear

	#region Mail
	//---------------------------------------------------------------------------------------------------
	public void OnEventMailReceive(Guid guidMail)
	{
		if (EventMailReceive != null)
		{
			EventMailReceive(guidMail);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMailReceiveAll(Guid[] guidMails)
	{
		if (EventMailReceiveAll != null)
		{
			EventMailReceiveAll(guidMails);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMailDelete(Guid guidMail)
	{
		if (EventMailDelete != null)
		{
			EventMailDelete(guidMail);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMailDeleteAll()
	{
		if (EventMailDeleteAll != null)
		{
			EventMailDeleteAll();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventNewMail(Guid guidMail)
	{
		if (EventNewMail != null)
		{
			EventNewMail(guidMail);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMailSelected(Guid guidMail)
	{
		if (EventMailSelected != null)
		{
			EventMailSelected(guidMail);
		}
	}

	#endregion Mail

	#region Lak, Exp
	//----------------------------------------------------------------------------------------------------
	// 라크
	public void OnEventLakAcquisition()
	{
		if (EventLakAcquisition != null)
		{
			EventLakAcquisition();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 경험치획득
	public void OnEventExpAcquisition(long lExpAcq, bool bLevelUp)
	{
		if (EventExpAcquisition != null)
		{
			EventExpAcquisition(lExpAcq, bLevelUp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 경험치 획득 텍스트
	public void OnEventExpAcuisitionText(long lExpAcq)
	{
		if (EventExpAcuisitionText != null)
		{
			EventExpAcuisitionText(lExpAcq);
		}
	}

	#endregion Lak, Exp

	#region Skill

	//----------------------------------------------------------------------------------------------------
	// 스킬레벨업	
	public void OnEventSkillLevelUp(int nSkillId)
	{
		if (EventSkillLevelUp != null)
		{
			EventSkillLevelUp(nSkillId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 스킬전체레벨업		
	public void OnEventSkillLevelUpTotally(int nSkillId)
	{
		if (EventSkillLevelUpTotally != null)
		{
			EventSkillLevelUpTotally(nSkillId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 스킬리스트에서 선택시
	public void OnEventSkillSelected(int nSkillId)
	{
		if (EventSkillSelected != null)
		{
			EventSkillSelected(nSkillId);
		}
	}

	#endregion Skill

	#region SimpleShop

	//----------------------------------------------------------------------------------------------------
	// 간이상점구입
	public void OnEventSimpleShopBuy()
	{
		if (EventSimpleShopBuy != null)
		{
			EventSimpleShopBuy();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 간이상점판매
	public void OnEventSimpleShopSell()
	{
		if (EventSimpleShopSell != null)
		{
			EventSimpleShopSell();
		}
	}

	#endregion SimpleShop

	#region Inventory

	//----------------------------------------------------------------------------------------------------
	// 슬롯확장
	public void OnEventInventorySlotExtend()
	{
		if (EventInventorySlotExtend != null)
		{
			EventInventorySlotExtend();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 아이템공유
	public void OnEventGearShare(CsHeroObject csHeroObject)
	{
		if (EventGearShare != null)
		{
			EventGearShare(csHeroObject);
		}
	}

	#endregion Inventory

	#region Item
	//----------------------------------------------------------------------------------------------------
	// 아이템합성
	public void OnEventItemCompose()
	{
		if (EventItemCompose != null)
		{
			EventItemCompose();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 아이템전체합성
	public void OnEventItemComposeTotally()
	{
		if (EventItemComposeTotally != null)
		{
			EventItemComposeTotally();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 물약사용
	public void OnEventHpPotionUse(int nRecoveryHp)
	{
		if (EventHpPotionUse != null)
		{
			EventHpPotionUse(nRecoveryHp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 귀환주문서사용완료
	public void OnEventReturnScrollUseFinished(int nContinentId)
	{
		if (EventReturnScrollUseFinished != null)
		{
			EventReturnScrollUseFinished(nContinentId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 귀환주문서사용취소
	public void OnEventReturnScrollUseCancel()
	{
		if (EventReturnScrollUseCancel != null)
		{
			EventReturnScrollUseCancel();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 뽑기상자사용
	public void OnEventPickBoxUse(bool bFull)
	{
		if (EventPickBoxUse != null)
		{
			EventPickBoxUse(bFull);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 메인장비상자사용
	public void OnEventMainGearBoxUse(bool bFull)
	{
		if (EventMainGearBoxUse != null)
		{
			EventMainGearBoxUse(bFull);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 경험치물약사용
	public void OnEventExpPotionUse(bool bLevelUp, long lAcquiredExp)
	{
		if (EventExpPotionUse != null)
		{
			EventExpPotionUse(bLevelUp, lAcquiredExp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 경험치주문서사용
	public void OnEventExpScrollUse()
	{
		if (EventExpScrollUse != null)
		{
			EventExpScrollUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 현상금사냥꾼퀘스트주문서사용
	public void OnEventBountyHunterQuestScrollUse()
	{
		if (EventBountyHunterQuestScrollUse != null)
		{
			EventBountyHunterQuestScrollUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 낚시미끼사용
	public void OnEventFishingBaitUse()
	{
		if (EventFishingBaitUse != null)
		{
			EventFishingBaitUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 왜곡주문서사용
	public void OnEventDistortionScrollUse()
	{
		if (EventDistortionScrollUse != null)
		{
			EventDistortionScrollUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 왜곡취소
	public void OnEventDistortionCanceled()
	{
		if (EventDistortionCanceled != null)
		{
			EventDistortionCanceled();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 골드아이템사용
	public void OnEventGoldItemUse()
	{
		if (EventGoldItemUse != null)
		{
			EventGoldItemUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 귀속다이아아이템사용
	public void OnEventOwnDiaItemUse()
	{
		if (EventOwnDiaItemUse != null)
		{
			EventOwnDiaItemUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 명예포인트아이템사용
	public void OnEventHonorPointItemUse()
	{
		if (EventHonorPointItemUse != null)
		{
			EventHonorPointItemUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 공적포인트아이템사용
	public void OnEventExploitPointItemUse()
	{
		if (EventExploitPointItemUse != null)
		{
			EventExploitPointItemUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개아이템사용
	public void OnEventWingItemUse()
	{
		if (EventWingItemUse != null)
		{
			EventWingItemUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 영혼석아이템사용
	public void OnEventSpiritStoneItemUse(int nDifference)
	{
		if (EventSpiritStoneItemUse != null)
		{
			EventSpiritStoneItemUse(nDifference);
		}
	}

	#endregion Item

	#region Rest

	//----------------------------------------------------------------------------------------------------
	// 무료휴식보상받기
	public void OnEventRestRewardReceiveFree(bool bLevelUp, long lAcquiredExp)
	{
		if (EventRestRewardReceiveFree != null)
		{
			EventRestRewardReceiveFree(bLevelUp, lAcquiredExp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 골드휴식보상받기
	public void OnEventRestRewardReceiveGold(bool bLevelUp, long lAcquiredExp)
	{
		if (EventRestRewardReceiveGold != null)
		{
			EventRestRewardReceiveGold(bLevelUp, lAcquiredExp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 다이아휴식보상받기
	public void OnEventRestRewardReceiveDia(bool bLevelUp, long lAcquiredExp)
	{
		if (EventRestRewardReceiveDia != null)
		{
			EventRestRewardReceiveDia(bLevelUp, lAcquiredExp);
		}
	}

	#endregion Rest

	#region Revive

	//----------------------------------------------------------------------------------------------------
	// 즉시부활
	public void OnEventImmediateRevive()
	{
		if (EventImmediateRevive != null)
		{
			EventImmediateRevive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 대륙안전부활
	public void OnEventContinentSaftyRevive(int nContitnentId)
	{
		if (EventContinentSaftyRevive != null)
		{
			EventContinentSaftyRevive(nContitnentId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 귀환주문서 시작
	public void OnEventReturnScrollUseStart()
	{
		if (EventReturnScrollUseStart != null)
		{
			EventReturnScrollUseStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 아이템 루팅
	public void OnEventDropObjectLooted(List<CsDropObject> listLooted, List<CsDropObject> listNotLooted)
	{
		if (EventDropObjectLooted != null)
		{
			EventDropObjectLooted(listLooted, listNotLooted);
		}
	}

	#endregion Revive

	#region Party

	//----------------------------------------------------------------------------------------------------
	// 주변영웅목록
	public void OnEventPartySurroundingHeroList(CsSimpleHero[] simpleHeroes)
	{
		if (EventPartySurroundingHeroList != null)
		{
			EventPartySurroundingHeroList(simpleHeroes);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 주변파티목록
	public void OnEventPartySurroundingPartyList(CsSimpleParty[] simpleParties)
	{
		if (EventPartySurroundingPartyList != null)
		{
			EventPartySurroundingPartyList(simpleParties);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티생성
	public void OnEventPartyCreate()
	{
		if (EventPartyCreate != null)
		{
			EventPartyCreate();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티탈퇴
	public void OnEventPartyExit()
	{
		if (EventPartyExit != null)
		{
			EventPartyExit();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티멤버강퇴
	public void OnEventPartyMemberBanish(Guid guidMemberId)
	{
		if (EventPartyMemberBanish != null)
		{
			EventPartyMemberBanish(guidMemberId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티소집
	public void OnEventPartyCall()
	{
		if (EventPartyCall != null)
		{
			EventPartyCall();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티해산
	public void OnEventPartyDisband()
	{
		if (EventPartyDisband != null)
		{
			EventPartyDisband();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청
	public void OnEventPartyApply(CsPartyApplication csPartyApplication)
	{
		if (EventPartyApply != null)
		{
			EventPartyApply(csPartyApplication);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청수락
	public void OnEventPartyApplicationAccept(CsPartyMember csPartyMember)
	{
		if (EventPartyApplicationAccept != null)
		{
			EventPartyApplicationAccept(csPartyMember);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청거절
	public void OnEventPartyApplicationRefuse()
	{
		if (EventPartyApplicationRefuse != null)
		{
			EventPartyApplicationRefuse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티마스터변경
	public void OnEventPartyMasterChange()
	{
		if (EventPartyMasterChange != null)
		{
			EventPartyMasterChange();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대
	public void OnEventPartyInvite(CsPartyInvitation partyInvitation)
	{
		if (EventPartyInvite != null)
		{
			EventPartyInvite(partyInvitation);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대수락
	public void OnEventPartyInvitationAccept()
	{
		if (EventPartyInvitationAccept != null)
		{
			EventPartyInvitationAccept();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대거절
	public void OnEventPartyInvitationRefuse()
	{
		if (EventPartyInvitationRefuse != null)
		{
			EventPartyInvitationRefuse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청도착
	public void OnEventPartyApplicationArrived(CsPartyApplication csPartyApplication)
	{
		if (EventPartyApplicationArrived != null)
		{
			EventPartyApplicationArrived(csPartyApplication);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청취소	
	public void OnEventPartyApplicationCanceled()
	{
		if (EventPartyApplicationCanceled != null)
		{
			EventPartyApplicationCanceled();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청수락
	public void OnEventPartyApplicationAccepted()
	{
		if (EventPartyApplicationAccepted != null)
		{
			EventPartyApplicationAccepted();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청거절
	public void OnEventPartyApplicationRefused()
	{
		if (EventPartyApplicationRefused != null)
		{
			EventPartyApplicationRefused();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티신청수명종료
	public void OnEventPartyApplicationLifetimeEnded()
	{
		if (EventPartyApplicationLifetimeEnded != null)
		{
			EventPartyApplicationLifetimeEnded();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대도착
	public void OnEventPartyInvitationArrived(CsPartyInvitation csPartyInvitation)
	{
		if (EventPartyInvitationArrived != null)
		{
			EventPartyInvitationArrived(csPartyInvitation);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대취소
	public void OnEventPartyInvitationCanceled()
	{
		if (EventPartyInvitationCanceled != null)
		{
			EventPartyInvitationCanceled();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대수락
	public void OnEventPartyInvitationAccepted()
	{
		if (EventPartyInvitationAccepted != null)
		{
			EventPartyInvitationAccepted();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대거절
	public void OnEventPartyInvitationRefused()
	{
		if (EventPartyInvitationRefused != null)
		{
			EventPartyInvitationRefused();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티초대수명종료
	public void OnEventPartyInvitationLifetimeEnded()
	{
		if (EventPartyInvitationLifetimeEnded != null)
		{
			EventPartyInvitationLifetimeEnded();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티멤버입장
	public void OnEventPartyMemberEnter(CsPartyMember csPartyMember)
	{
		if (EventPartyMemberEnter != null)
		{
			EventPartyMemberEnter(csPartyMember);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티멤버퇴장
	public void OnEventPartyMemberExit(CsPartyMember csPartyMember, bool bBanished)
	{
		if (EventPartyMemberExit != null)
		{
			EventPartyMemberExit(csPartyMember, bBanished);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티강퇴
	public void OnEventPartyBanished()
	{
		if (EventPartyBanished != null)
		{
			EventPartyBanished();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티장변경
	public void OnEventPartyMasterChanged()
	{
		if (EventPartyMasterChanged != null)
		{
			EventPartyMasterChanged();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티소집	
	public void OnEventPartyCalled(int nContinentId, int nNationId, Vector3 v3Position)
	{
		if (EventPartyCalled != null)
		{
			EventPartyCalled(nContinentId, nNationId, v3Position);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티해산
	public void OnEventPartyDisbanded()
	{
		if (EventPartyDisbanded != null)
		{
			EventPartyDisbanded();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티멤버갱신
	public void OnEventPartyMembersUpdated()
	{
		if (EventPartyMembersUpdated != null)
		{
			EventPartyMembersUpdated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 주변영웅목록요청	
	public void OnEventPartySurroundingHeroListRequest()
	{
		if (EventPartySurroundingHeroListRequest != null)
		{
			EventPartySurroundingHeroListRequest();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 주변파티목록요청
	public void OnEventPartySurroundingPartyListRequest()
	{
		if (EventPartySurroundingPartyListRequest != null)
		{
			EventPartySurroundingPartyListRequest();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 파티집합
	public void OnEventHeroPosition()
	{
		if (EventHeroPosition != null)
		{
			EventHeroPosition();
		}
	}

	#endregion Party

	#region Chatting
	//----------------------------------------------------------------------------------------------------
	// 채팅메시지발송
	public void OnEventChattingMessageSend()
	{
		if (EventChattingMessageSend != null)
		{
			EventChattingMessageSend();
		}
	}


	//----------------------------------------------------------------------------------------------------
	// 채팅메시지수신
	public void OnEventChattingMessageReceived(CsChattingMessage csChattingMessage)
	{
		if (EventChattingMessageReceived != null)
		{
			EventChattingMessageReceived(csChattingMessage);
		}
	}

	#endregion Chatting

	//----------------------------------------------------------------------------------------------------
	// 영웅장비아이템획득
	public void OnEventHeroObjectGot(List<CsHeroObject> list)
	{
		if (EventHeroObjectGot != null)
		{
			EventHeroObjectGot(list);
		}
	}

	#region Support

	//----------------------------------------------------------------------------------------------------
	// 레벨업보상받기
	public void OnEventLevelUpRewardReceive()
	{
		if (EventLevelUpRewardReceive != null)
		{
			EventLevelUpRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 일일접속시간보상받기
	public void OnEventDailyAccessTimeRewardReceive()
	{
		if (EventDailyAccessTimeRewardReceive != null)
		{
			EventDailyAccessTimeRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 출석보상받기
	public void OnEventAttendRewardReceive()
	{
		if (EventAttendRewardReceive != null)
		{
			EventAttendRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 연속미션보상받기
	public void OnEventSeriesMissionRewardReceive(CsHeroSeriesMission csHeroSeriesMission)
	{
		if (EventSeriesMissionRewardReceive != null)
		{
			EventSeriesMissionRewardReceive(csHeroSeriesMission);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오늘의미션보상받기
	public void OnEventTodayMissionRewardReceive(CsHeroTodayMission csHeroTodayMission)
	{
		if (EventTodayMissionRewardReceive != null)
		{
			EventTodayMissionRewardReceive(csHeroTodayMission);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 연속미션갱신
	public void OnEventSeriesMissionUpdated(CsHeroSeriesMission csHeroSeriesMission)
	{
		if (EventSeriesMissionUpdated != null)
		{
			EventSeriesMissionUpdated(csHeroSeriesMission);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오늘의미션목록변경
	public void OnEventTodayMissionListChanged()
	{
		if (EventTodayMissionListChanged != null)
		{
			EventTodayMissionListChanged();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오늘의미션갱신
	public void OnEventTodayMissionUpdated(CsHeroTodayMission csHeroTodayMission)
	{
		if (EventTodayMissionUpdated != null)
		{
			EventTodayMissionUpdated(csHeroTodayMission);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 신병선물받기
	public void OnEventRookieGiftReceive()
	{
		if (EventRookieGiftReceive != null)
		{
			EventRookieGiftReceive();
		}

	}

	//----------------------------------------------------------------------------------------------------
	// 오픈선물받기
	public void OnEventOpenGiftReceive(int nDay)
	{
		if (EventOpenGiftReceive != null)
		{
			EventOpenGiftReceive(nDay);
		}
	}

	#endregion Support

	#region Mount

	//----------------------------------------------------------------------------------------------------
	// 탈것장착
	public void OnEventMountEquip()
	{
		if (EventMountEquip != null)
		{
			EventMountEquip();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 탈것레벨업
	public void OnEventMountLevelUp(bool bLevelUp)
	{
		if (EventMountLevelUp != null)
		{
			EventMountLevelUp(bLevelUp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 탈것 토글 켜기/끄기
	public void OnEventDisplayMountToggleIsOn(bool bIsOn)
	{
		if (EventDisplayMountToggleIsOn != null)
		{
			EventDisplayMountToggleIsOn(bIsOn);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 탈것각성레벨업
	public void OnEventMountAwakeningLevelUp()
	{
		if (EventMountAwakeningLevelUp != null)
		{
			EventMountAwakeningLevelUp();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 탈것속성물약사용
	public void OnEventMountAttrPotionUse()
	{
		if (EventMountAttrPotionUse != null)
		{
			EventMountAttrPotionUse();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 탈것아이템사용
	public void OnEventMountItemUse()
	{
		if (EventMountItemUse != null)
		{
			EventMountItemUse();
		}
	}

    //---------------------------------------------------------------------------------------------------
    public void OnEventMountSelected(int nMountId)
    {
        if (EventMountSelected != null)
        {
            EventMountSelected(nMountId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventOpenPopupMountAttrPotion(int nMountId)
    {
        if (EventOpenPopupMountAttrPotion != null)
        {
            EventOpenPopupMountAttrPotion(nMountId);
        }
    }

	#endregion Mount

	#region MountGear

	//----------------------------------------------------------------------------------------------------
	// 탈것장비장착
	public void OnEventMountGearEquip(Guid guidHeroMountGearId)
	{
		if (EventMountGearEquip != null)
		{
			EventMountGearEquip(guidHeroMountGearId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 탈것장비장착해제
	public void OnEventMountGearUnequip(Guid guidHeroMountGearId)
	{
		if (EventMountGearUnequip != null)
		{
			EventMountGearUnequip(guidHeroMountGearId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 탈것장비재강화
	public void OnEventMountGearRefine(Guid guidHeroMountGearId)
	{
		if (EventMountGearRefine != null)
		{
			EventMountGearRefine(guidHeroMountGearId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 탈것장비뽑기상자제작
	public void OnEventMountGearPickBoxMake()
	{
		if (EventMountGearPickBoxMake != null)
		{
			EventMountGearPickBoxMake();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 탈것장비뽑기상자모두제작
	public void OnEventMountGearPickBoxMakeTotally()
	{
		if (EventMountGearPickBoxMakeTotally != null)
		{
			EventMountGearPickBoxMakeTotally();
		}
	}

	#endregion MountGear

	#region Quest List

	//----------------------------------------------------------------------------------------------------
	// 퀘스트 리스트에서 퀘스트 선택시 우측 정보 갱신
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	public void OnEventSelectQuest(EnQuestType enQuestType, int nParam = 0, bool bReceivableSubQuest = false)
	{
		if (EventSelectQuest != null)
		{
			EventSelectQuest(enQuestType, nParam, bReceivableSubQuest);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 패널에 있는 퀘스트 리스트중 해당 패널 감추기
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	public void OnEventDisplayQuestPanel(EnQuestType enQuestType, bool bIsOn, int nParam = 0)
	{
		if (EventDisplayQuestPanel != null)
		{
			EventDisplayQuestPanel(enQuestType, bIsOn, nParam);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 선택 퀘스트 자동 시작.
	public void OnEventStartQuestAutoPlay(EnQuestCategoryType enQuestCartegoryType, int nQuestIndex)
	{
		if (EventStartQuestAutoPlay != null)
		{
			EventStartQuestAutoPlay(enQuestCartegoryType, nQuestIndex);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 수령 가능한 서브 퀘스트 창에서 수령 가능한 서브 퀘스트가 없는 경우 호출
	public void OnEventAcceptableSubQuestEmpty()
	{
		if (EventAcceptableSubQuestEmpty != null)
		{
			EventAcceptableSubQuestEmpty();
		}
	}

	#endregion Quest Lsit

	#region UserReference

	//----------------------------------------------------------------------------------------------------
	// 1:1대화창 호출
	public void OnEventOpenOneToOneChat(Guid guid)
	{
		if (EventOpenOneToOneChat != null)
		{
			EventOpenOneToOneChat(guid);
		}
	}

	#endregion UserReference

	#region Wing

	//----------------------------------------------------------------------------------------------------
	// 날개장착
	public void OnEventWingEquip()
	{
		if (EventWingEquip != null)
		{
			EventWingEquip();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개강화
	public void OnEventWingEnchant()
	{
		if (EventWingEnchant != null)
		{
			EventWingEnchant();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개전체강화
	public void OnEventWingEnchantTotally()
	{
		if (EventWingEnchantTotally != null)
		{
			EventWingEnchantTotally();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개획득
	public void OnEventWingAcquisition()
	{
		if (EventWingAcquisition != null)
		{
			EventWingAcquisition();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개기억조각장착
	public void OnEventWingMemoryPieceInstall()
	{
		if (EventWingMemoryPieceInstall != null)
		{
			EventWingMemoryPieceInstall();
		}
	}

	#endregion Wing

	#region StoryDungeon

	//----------------------------------------------------------------------------------------------------
	// 스토리던전입장을위한대륙퇴장
	public void OnEventContinentExitForStoryDungeonEnter(string strSceneName)
	{
		if (EventContinentExitForStoryDungeonEnter != null)
		{
			EventContinentExitForStoryDungeonEnter(strSceneName);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개전체강화
	public void OnEventStoryDungeonAbandon(int nContinentId)
	{
		if (EventStoryDungeonAbandon != null)
		{
			EventStoryDungeonAbandon(nContinentId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 날개전체강화
	public void OnEventStoryDungeonExit(int nContinentId)
	{
		if (EventStoryDungeonExit != null)
		{
			EventStoryDungeonExit(nContinentId);
		}
	}

	#endregion StoryDungeon

	#region Stamina

	//----------------------------------------------------------------------------------------------------
	public void OnEventStaminaAutoRecovery()
	{
		if (EventStaminaAutoRecovery != null)
		{
			EventStaminaAutoRecovery();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventStaminaBuy()
	{
		if (EventStaminaBuy != null)
		{
			EventStaminaBuy();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventStaminaScheduleRecovery()
	{
		if (EventStaminaScheduleRecovery != null)
		{
			EventStaminaScheduleRecovery();
		}
	}

	#endregion Stamina

	#region HeroInfo

	//----------------------------------------------------------------------------------------------------
	//영웅정보 조회
	public void OnEventHeroInfo(CsHeroInfo csHeroInfo)
	{
		if (EventHeroInfo != null)
		{
			EventHeroInfo(csHeroInfo);
		}
	}

	#endregion HeroInfo

	#region Dungeon

	//----------------------------------------------------------------------------------------------------
	//던전 카테고리 리스트에서 던전 선택.
	public void OnEventSelectDungeonCartegory(int nDungeonIndex)
	{
		if (EventSelectDungeonCartegory != null)
		{
			EventSelectDungeonCartegory(nDungeonIndex);
		}
	}

	//----------------------------------------------------------------------------------------------------
	//던전 카테고리 리스트로 돌아가기
	public void OnEventGoBackDungeonCartegoryList()
	{
		if (EventGoBackDungeonCartegoryList != null)
		{
			EventGoBackDungeonCartegoryList();
		}
	}

	#endregion Dungeon

	#region Minimap

	//[UI]맵선택시
	public void OnEventMiniMapSelected(int nLocationId)
	{
		if (EventMiniMapSelected != null)
		{
			EventMiniMapSelected(nLocationId);
		}
	}

	#endregion Minimap

	#region ContinentTrasmission

	// 대륙전송
	public void OnEventContinentTransmission(string strSceneName)
	{
		if (EventContinentTransmission != null)
		{
			EventContinentTransmission(strSceneName);
		}
	}

	#endregion ContinentTrasmission

	#region Nation

	// 국가전송
	public void OnEventNationTransmission(string strSceneName)
	{
		if (EventNationTransmission != null)
		{
			EventNationTransmission(strSceneName);
		}
	}

	//----------------------------------------------------------------------------------------------------
	//영웅검색
	public void OnEventHeroSearch(List<CsSearchHero> list)
	{
		if (EventHeroSearch != null)
		{
			EventHeroSearch(list);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가기부
	public void OnEventNationDonate(int nAcquiredExploitPoint)
	{
		if (EventNationDonate != null)
		{
			EventNationDonate(nAcquiredExploitPoint);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가관직임명
	public void OnEventNationNoblesseAppoint()
	{
		if (EventNationNoblesseAppoint != null)
		{
			EventNationNoblesseAppoint();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가관직해임
	public void OnEventNationNoblesseDismiss()
	{
		if (EventNationNoblesseDismiss != null)
		{
			EventNationNoblesseDismiss();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가관직임명 이벤트
	public void OnEventNationNoblesseAppointed(int nNoblesseId, string strHeroName)
	{
		if (EventNationNoblesseAppointed != null)
		{
			EventNationNoblesseAppointed(nNoblesseId, strHeroName);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가관직해임 이벤트
	public void OnEventNationNoblesseDismissed()
	{
		if (EventNationNoblesseDismissed != null)
		{
			EventNationNoblesseDismissed();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가자금변경 이벤트
	public void OnEventNationFundChanged()
	{
		if (EventNationFundChanged != null)
		{
			EventNationFundChanged();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가소집
	public void OnEventNationCall()
	{
		if (EventNationCall != null)
		{
			EventNationCall();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가소집전송
	public void OnEventNationCallTransmission(int nContinentId)
	{
		if (EventNationCallTransmission != null)
		{
			EventNationCallTransmission(nContinentId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가소집 이벤트
	public void OnEventNationCalled(CsNationCall csNationCall)
	{
		if (EventNationCalled != null)
		{
			EventNationCalled(csNationCall);
		}
	}

	#endregion Nation

	#region Fishing

	//낚시터이동
	public void OnEventAutoMoveFishingZone()
	{
		if (EventAutoMoveFishingZone != null)
		{
			EventAutoMoveFishingZone();
		}
	}

	#endregion Fishing

	#region TodayTasks

	//----------------------------------------------------------------------------------------------------
	// 달성보상받기
	public void OnEventAchievementRewardReceive()
	{
		if (EventAchievementRewardReceive != null)
		{
			EventAchievementRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오늘의할일갱신
	public void OnEventTodayTaskUpdated()
	{
		if (EventTodayTaskUpdated != null)
		{
			EventTodayTaskUpdated();
		}
	}

	#endregion TodayTasks

	#region Matching

	//----------------------------------------------------------------------------------------------------
	// 매칭 팝업 오픈
	public void OnEventOpenPopupMatching()
	{
		if (EventOpenPopupMatching != null)
		{
			EventOpenPopupMatching();
		}
	}

	#endregion

	#region VIP

	//----------------------------------------------------------------------------------------------------
	// Vip레벨보상받기
	public void OnEventVipLevelRewardReceive()
	{
		if (EventVipLevelRewardReceive != null)
		{
			EventVipLevelRewardReceive();
		}
	}

	#endregion VIP

	#region Rank

	//----------------------------------------------------------------------------------------------------
	// 계급획득
	public void OnEventRankAcquire()
	{
		if (EventRankAcquire != null)
		{
			EventRankAcquire();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 계급보상받기
	public void OnEventRankRewardReceive()
	{
		if (EventRankRewardReceive != null)
		{
			EventRankRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 계급액티브스킬레벨업
	public void OnEventRankActiveSkillLevelUp()
	{
		if (EventRankActiveSkillLevelUp != null)
		{
			EventRankActiveSkillLevelUp();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 계급액티브스킬선택
	public void OnEventRankActiveSkillSelect()
	{
		if (EventRankActiveSkillSelect != null)
		{
			EventRankActiveSkillSelect();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 계급패시브스킬레벨업
	public void OnEventRankPassiveSkillLevelUp()
	{
		if (EventRankPassiveSkillLevelUp != null)
		{
			EventRankPassiveSkillLevelUp();
		}
	}

	#endregion Rank

	#region HonorShop

	//----------------------------------------------------------------------------------------------------
	// 명예상점상품구매
	public void OnEventHonorShopProductBuy()
	{
		if (EventHonorShopProductBuy != null)
		{
			EventHonorShopProductBuy();
		}
	}

	#endregion HonorShop

	#region Ranking

	//----------------------------------------------------------------------------------------------------
	// 서버전투력랭킹
	public void OnEventServerBattlePowerRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
	{
		if (EventServerBattlePowerRanking != null)
		{
			EventServerBattlePowerRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 서버직업전투력랭킹
	public void OnEventServerJobBattlePowerRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
	{
		if (EventServerJobBattlePowerRanking != null)
		{
			EventServerJobBattlePowerRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 서버레벨랭킹
	public void OnEventServerLevelRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
	{
		if (EventServerLevelRanking != null)
		{
			EventServerLevelRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 서버레벨랭킹보상받기
	public void OnEventServerLevelRankingRewardReceive()
	{
		if (EventServerLevelRankingRewardReceive != null)
		{
			EventServerLevelRankingRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 본국전투력랭킹
	public void OnEventNationBattlePowerRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
	{
		if (EventNationBattlePowerRanking != null)
		{
			EventNationBattlePowerRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 본국공적포인트랭킹
	public void OnEventNationExploitPointRanking(CsRanking csRanking, List<CsRanking> listCsRanking)
	{
		if (EventNationExploitPointRanking != null)
		{
			EventNationExploitPointRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 일일서버레벨랭킹갱신
	public void OnEventDailyServerLevelRankingUpdated()
	{
		if (EventDailyServerLevelRankingUpdated != null)
		{
			EventDailyServerLevelRankingUpdated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 서버크리쳐카드랭킹
	public void OnEventServerCreatureCardRanking(CsCreatureCardRanking csRanking, List<CsCreatureCardRanking> listCsRanking)
	{
		if (EventServerCreatureCardRanking != null)
		{
			EventServerCreatureCardRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 서버도감랭킹
	public void OnEventServerIllustratedBookRanking(CsIllustratedBookRanking csRanking, List<CsIllustratedBookRanking> listCsRanking)
	{
		if (EventServerIllustratedBookRanking != null)
		{
			EventServerIllustratedBookRanking(csRanking, listCsRanking);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 일일서버국력랭킹갱신
	public void OnEventDailyServerNationPowerRankingUpdated()
	{
		if (EventDailyServerNationPowerRankingUpdated != null)
		{
			EventDailyServerNationPowerRankingUpdated();
		}
	}

	#endregion Ranking

	#region Attainment

	//----------------------------------------------------------------------------------------------------
	// 도달보상받기
	public void OnEventAttainmentRewardReceive()
	{
		if (EventAttainmentRewardReceive != null)
		{
			EventAttainmentRewardReceive();
		}
	}

	#endregion Attainment

	#region AutoQuest

	//----------------------------------------------------------------------------------------------------
	// 오늘의할일 퀘스트 자동이동
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	public void OnEventAutoQuestStart(EnAutoStateType enAutoStateType, int nParam = 0)
	{
		if (EventAutoQuestStart != null)
		{
			EventAutoQuestStart(enAutoStateType, nParam);
		}
	}

	#endregion AutoQuest

	#region User Reference

	//----------------------------------------------------------------------------------------------------
	// 유져 정보 조회 팝업 오픈
	public void OnEventOpenUserReference(CsHeroBase csHeroBase)
	{
		if (EventOpenUserReference != null)
		{
			EventOpenUserReference(csHeroBase);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOpenFriendRefernce(CsFriend csFriend)
	{
		if (EventOpenFriendRefernce != null)
		{
			EventOpenFriendRefernce(csFriend);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 길드 정보 조회 팝업 오픈
	public void OnEventOpenGuildMemberReference(CsGuildMember csGuildMember)
	{
		if (EventOpenGuildMemberReference != null)
		{
			EventOpenGuildMemberReference(csGuildMember);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 국가 관직 유저 조회 팝업 오픈
	public void OnEventOpenNationNoblesseReference(CsNationNoblesseInstance csNationNoblesseInstance)
	{
		if (EventOpenNationNoblesseReference != null)
		{
			EventOpenNationNoblesseReference(csNationNoblesseInstance);
		}
	}

	#endregion

	#region Tutorial

	//----------------------------------------------------------------------------------------------------
	// 튜토리얼 종료
	public void OnEventTutorialEnd()
	{
		if (EventTutorialEnd != null)
		{
			EventTutorialEnd();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 참고 튜토리얼
	public void OnEventReferenceTutorial(EnTutorialType enTutorialType)
	{
		if (EventReferenceTutorial != null)
		{
			CsConfiguration.Instance.TutorialComplete(enTutorialType);
			EventReferenceTutorial(enTutorialType);
		}
	}

	#endregion Tutorial

	#region Continent

	//----------------------------------------------------------------------------------------------------
	public void OnEventContinentBanished(int nContinentId)
	{
		if (EventContinentBanished != null)
		{
			EventContinentBanished(nContinentId);
		}
	}

	#endregion Continent

	#region Settings

	//----------------------------------------------------------------------------------------------------
	public void OnEventBattleSettingSet()
	{
		if (EventBattleSettingSet != null)
		{
			EventBattleSettingSet();
		}
	}

	#endregion Settings

	#region PotionAttr

	//----------------------------------------------------------------------------------------------------
	public void OnEventHeroAttrPotionUse()
	{
		if (EventHeroAttrPotionUse != null)
		{
			EventHeroAttrPotionUse();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventHeroAttrPotionUseAll()
	{
		if (EventHeroAttrPotionUseAll != null)
		{
			EventHeroAttrPotionUseAll();
		}
	}

	#endregion PotionAttr

	//----------------------------------------------------------------------------------------------------
	public void OnEventMainQuestNpcDialog()
	{
		if (EventMainQuestNpcDialog != null)
		{
			EventMainQuestNpcDialog();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventMainButtonUpdate(int nGroupNo, int nMenuId, bool bVisible)
	{
		if (EventMainButtonUpdate != null)
		{
			EventMainButtonUpdate(nGroupNo, nMenuId, bVisible);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventMenuOpenButtonNoticeUpdate(bool bVisible)
	{
		if (EventMenuOpenButtonNoticeUpdate != null)
		{
			EventMenuOpenButtonNoticeUpdate(bVisible);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventLeftTopMenuOpenButtonNoticeUpdate(bool bVisible)
	{
		if (EventLeftTopMenuOpenButtonNoticeUpdate != null)
		{
			EventLeftTopMenuOpenButtonNoticeUpdate(bVisible);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOpenPopupNationWarResult()
	{
		if (EventOpenPopupNationWarResult != null)
		{
			EventOpenPopupNationWarResult();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventNotice(string strContent)
	{
		if (EventNotice != null)
		{
			EventNotice(strContent);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventTodayMissionTutorialStart()
	{
		if (EventTodayMissionTutorialStart != null)
		{
			EventTodayMissionTutorialStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventServerMaxLevelUpdated()
	{
		if (EventServerMaxLevelUpdated != null)
		{
			EventServerMaxLevelUpdated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 크리처 획득
	public void OnEventGetHeroCreatureCard(List<CsHeroCreatureCard> list)
	{
		if (EventGetHeroCreatureCard != null)
		{
			EventGetHeroCreatureCard(list);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventNetworkStatus(string strNetworkType, string strSignalStrength)
	{
		if (EventNetworkStatus != null)
		{
			EventNetworkStatus(strNetworkType, strSignalStrength);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventBatteryStatus(string strBatteryStatus, string strChargeType)
	{
		if (EventBatteryStatus != null)
		{
			EventBatteryStatus(strBatteryStatus, strChargeType);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventGroggyMonsterItemStealStart()
	{
		if (EventGroggyMonsterItemStealStart != null)
		{
			EventGroggyMonsterItemStealStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventGroggyMonsterItemStealCancel()
	{
		if (EventGroggyMonsterItemStealCancel != null)
		{
			EventGroggyMonsterItemStealCancel();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventGroggyMonsterItemStealFinished(CsItem csItem, bool bOwned, int nCount)
	{
		if (EventGroggyMonsterItemStealFinished != null)
		{
			EventGroggyMonsterItemStealFinished(csItem, bOwned, nCount);
		}
	}

	//----------------------------------------------------------------------------------------------------
	//  NPC상점상품구입
	public void OnEventNpcShopProductBuy()
	{
		if (EventNpcShopProductBuy != null)
		{
			EventNpcShopProductBuy();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오픈7일이벤트미션보상받기
	public void OnEventOpen7DayEventMissionRewardReceive()
	{
		if (EventOpen7DayEventMissionRewardReceive != null)
		{
			EventOpen7DayEventMissionRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오픈7일이벤트상품구매
	public void OnEventOpen7DayEventProductBuy()
	{
		if (EventOpen7DayEventProductBuy != null)
		{
			EventOpen7DayEventProductBuy();
		}
	}

	public void OnEventOpen7DayEventRewardReceive()
	{
		if (EventOpen7DayEventRewardReceive != null)
		{
			EventOpen7DayEventRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 오픈7일이벤트진행카운트갱신
	public void OnEventOpen7DayEventProgressCountUpdated()
	{
		if (EventOpen7DayEventProgressCountUpdated != null)
		{
			EventOpen7DayEventProgressCountUpdated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 골드회수
	public void OnEventRetrieveGold(bool bLevelUp, long lExpAcq)
	{
		if (EventRetrieveGold != null)
		{
			EventRetrieveGold(bLevelUp, lExpAcq);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 골드모두회수
	public void OnEventRetrieveGoldAll(bool bLevelUp, long lExpAcq)
	{
		if (EventRetrieveGoldAll != null)
		{
			EventRetrieveGoldAll(bLevelUp, lExpAcq);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 다이아회수
	public void OnEventRetrieveDia(bool bLevelUp, long lExpAcq)
	{
		if (EventRetrieveDia != null)
		{
			EventRetrieveDia(bLevelUp, lExpAcq);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 다이아모두회수
	public void OnEventRetrieveDiaAll(bool bLevelUp, long lExpAcq)
	{
		if (EventRetrieveDiaAll != null)
		{
			EventRetrieveDiaAll(bLevelUp, lExpAcq);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 회수진행카운트갱신
	public void OnEventRetrievalProgressCountUpdated()
	{
		if (EventRetrievalProgressCountUpdated != null)
		{
			EventRetrievalProgressCountUpdated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 할일위탁시작
	public void OnEventTaskConsignmentStart()
	{
		if (EventTaskConsignmentStart != null)
		{
			EventTaskConsignmentStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 할일위탁완료
	public void OnEventTaskConsignmentComplete(bool bLevelUp, long lExpAcq)
	{
		if (EventTaskConsignmentComplete != null)
		{
			EventTaskConsignmentComplete(bLevelUp, lExpAcq);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 할일위탁즉시완료
	public void OnEventTaskConsignmentImmediatelyComplete(bool bLevelUp, long lExpAcq)
	{
		if (EventTaskConsignmentImmediatelyComplete != null)
		{
			EventTaskConsignmentImmediatelyComplete(bLevelUp, lExpAcq);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 한정선물보상받기
	public void OnEventLimitationGiftRewardReceive()
	{
		if (EventLimitationGiftRewardReceive != null)
		{
			EventLimitationGiftRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 주말보상선택
	public void OnEventWeekendRewardSelect()
	{
		if (EventWeekendRewardSelect != null)
		{
			EventWeekendRewardSelect();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 주말보상받기
	public void OnEventWeekendRewardReceive()
	{
		if (EventWeekendRewardReceive != null)
		{
			EventWeekendRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 창고입고
	public void OnEventWarehouseDeposit()
	{
		if (EventWarehouseDeposit != null)
		{
			EventWarehouseDeposit();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 창고출고
	public void OnEventWarehouseWithdraw()
	{
		if (EventWarehouseWithdraw != null)
		{
			EventWarehouseWithdraw();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 창고슬롯확장
	public void OnEventWarehouseSlotExtend()
	{
		if (EventWarehouseSlotExtend != null)
		{
			EventWarehouseSlotExtend();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 다이아상점상품구매
	public void OnEventDiaShopProductBuy()
	{
		if (EventDiaShopProductBuy != null)
		{
			EventDiaShopProductBuy();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 공포의 제단 성물 도감
	public void OnEventOpenPopupHalidomCollection(bool bRewardReceivable = true)
	{
		if (EventOpenPopupHalidomCollection != null)
		{
			EventOpenPopupHalidomCollection(bRewardReceivable);
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 공포의 제단 원소 보상
	public void OnEventFearAltarHalidomElementalRewardReceive()
	{
		if (EventFearAltarHalidomElementalRewardReceive != null)
		{
			EventFearAltarHalidomElementalRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 공포의 제단 수집 보상
	public void OnEventFearAltarHalidomCollectionRewardReceive()
	{
		if (EventFearAltarHalidomCollectionRewardReceive != null)
		{
			EventFearAltarHalidomCollectionRewardReceive();
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 공포의 제단 성물 획득
	public void OnEventFearAltarHalidomAcquisition(int nHalidomId)
	{
		if (EventFearAltarHalidomAcquisition != null)
		{
			EventFearAltarHalidomAcquisition(nHalidomId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOrdealQuestSlotComplete(bool bLevelUp, long lAcquiredExp)
	{
		if (EventOrdealQuestSlotComplete != null)
		{
			EventOrdealQuestSlotComplete(bLevelUp, lAcquiredExp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOrdealQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		if (EventOrdealQuestComplete != null)
		{
			EventOrdealQuestComplete(bLevelUp, lAcquiredExp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOrdealQuestAccepted()
	{
		if (EventOrdealQuestAccepted != null)
		{
			EventOrdealQuestAccepted();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOrdealQuestSlotProgressCountsUpdated()
	{
		if (EventOrdealQuestSlotProgressCountsUpdated != null)
		{
			EventOrdealQuestSlotProgressCountsUpdated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventStartOrdealQuestMission(CsOrdealQuestMission csOrdealQuestMission)
	{
		if (EventStartOrdealQuestMission != null)
		{
			EventStartOrdealQuestMission(csOrdealQuestMission);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventClosePanelBiography()
	{
		if (EventClosePanelBiography != null)
		{
			EventClosePanelBiography();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventOpenPanelBiographyComplete(int nBiographyId)
	{
		if (EventOpenPanelBiographyComplete != null)
		{
			EventOpenPanelBiographyComplete(nBiographyId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventBiographyNpcDialog(CsBiographyQuest csBiographyQuest)
	{
		if (EventBiographyNpcDialog != null)
		{
			EventBiographyNpcDialog(csBiographyQuest);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPortalEnter()
	{
		if (EventPortalEnter != null)
		{
			EventPortalEnter();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventAccountLoginDuplicated()
	{
		if (EventAccountLoginDuplicated != null)
		{
			EventAccountLoginDuplicated();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventFriendAdd(bool bAdd)
	{
		if (EventFriendAdd != null)
		{
			EventFriendAdd(bAdd);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventSelectFriendDelete()
	{
		if (EventSelectFriendDelete != null)
		{
			EventSelectFriendDelete();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventGetHeroCreature(List<CsHeroCreature> list)
	{
		if (EventGetHeroCreature != null)
		{
			EventGetHeroCreature(list);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventSleepModeReset()
	{
		if (EventSleepModeReset != null)
		{
			EventSleepModeReset();
		}
	}

    #region 선물

    //----------------------------------------------------------------------------------------------------
	public void OnEventOpenPopupPresent(Guid guidHeroId)
	{
		if (EventOpenPopupPresent != null)
		{
			EventOpenPopupPresent(guidHeroId);
		}
	}

    //----------------------------------------------------------------------------------------------------
    public void OnEventCheckPresentButtonDisplay()
    {
        if (EventCheckPresentButtonDisplay != null)
        {
            EventCheckPresentButtonDisplay();
        }
    }

    #endregion 선물

    #region 크리처
    //----------------------------------------------------------------------------------------------------
	public void OnEventSelectCreatureToggle(Guid guidCreatureInstanceId)
	{
		if (EventSelectCreatureToggle != null)
		{
			EventSelectCreatureToggle(guidCreatureInstanceId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventCreatureSubMenuChanged()
	{
		if (EventCreatureSubMenuChanged != null)
		{
			EventCreatureSubMenuChanged();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public Guid OnEventGetSelectedCreature()
	{
		if (EventGetSelectedCreature != null)
		{
			return EventGetSelectedCreature();
		}

		return Guid.Empty;
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPointerDownCreatureFood(int nItemId)
	{
		if (EventPointerDownCreatureFood != null)
		{
			EventPointerDownCreatureFood(nItemId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPointerUpCreatureFood()
	{
		if (EventPointerUpCreatureFood != null)
		{
			EventPointerUpCreatureFood();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPointerExitCreatureFood()
	{
		if (EventPointerExitCreatureFood != null)
		{
			EventPointerExitCreatureFood();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPointerDownCreatureSkill(int nSlotId)
	{
		if (EventPointerDownCreatureSkill != null)
		{
			EventPointerDownCreatureSkill(nSlotId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPointerUpCreatureSkill()
	{
		if (EventPointerUpCreatureSkill != null)
		{
			EventPointerUpCreatureSkill();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void OnEventPointerExitCreatureSkill()
	{
		if (EventPointerExitCreatureSkill != null)
		{
			EventPointerExitCreatureSkill();
		}
	}

	#endregion 크리처

    // 길드 축복
    //----------------------------------------------------------------------------------------------------
    public void OnEventOpenPopupGuildBlessing()
    {
        if (EventOpenPopupGuildBlessing != null)
        {
            EventOpenPopupGuildBlessing();
        }
    }

	//----------------------------------------------------------------------------------------------------
	// 아티펙트 인벤토리슬롯 롱클릭
	public void OnEventInventoryLongClick(int nIndex)
	{
		if (EventInventoryLongClick != null)
		{
			EventInventoryLongClick(nIndex);
		}
	}

    //----------------------------------------------------------------------------------------------------
	public void OnEventSystemMessage(CsSystemMessage csSystemMessage)
	{
		if (EventSystemMessage != null)
		{
			EventSystemMessage(csSystemMessage);
		}
	}

    //----------------------------------------------------------------------------------------------------
    public void OnEventOpenPopupHeroPotionAttr()
    {
        if (EventOpenPopupHeroPotionAttr != null)
        {
            EventOpenPopupHeroPotionAttr();
        }
    }

    //----------------------------------------------------------------------------------------------------
    public void OnEventOpenPopupCostumeEffect()
    {
        if (EventOpenPopupCostumeEffect != null)
        {
            EventOpenPopupCostumeEffect();
        }
    }

    //----------------------------------------------------------------------------------------------------
    public void OnEventOpenPopupCostumeEnchant()
    {
        if (EventOpenPopupCostumeEnchant != null)
        {
            EventOpenPopupCostumeEnchant();
        }
    }

    //----------------------------------------------------------------------------------------------------
    public void OnEventScheduleNotice(string strMessage)
    {
        if (EventScheduleNotice != null)
        {
            EventScheduleNotice(strMessage);
        }
    }
 	//----------------------------------------------------------------------------------------------------
    public void OnEventHeroSkillReset()
    {
        if (EventHeroSkillReset != null)
        {
            EventHeroSkillReset();
        }
    }
	
	//----------------------------------------------------------------------------------------------------
	public void OnEventOpenWebView(string strUrl)
	{
		if (EventOpenWebView != null)
		{
			EventOpenWebView(strUrl);
		}
	}

    //----------------------------------------------------------------------------------------------------
    public void OnEventChangeAutoBattleMode(EnBattleMode enBattleMode)
    {
        if (EventChangeAutoBattleMode != null)
        {
            EventChangeAutoBattleMode(enBattleMode);
        }
    }

    //----------------------------------------------------------------------------------------------------
}