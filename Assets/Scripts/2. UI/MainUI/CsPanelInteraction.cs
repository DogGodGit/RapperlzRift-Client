using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-28)
//---------------------------------------------------------------------------------------------------

public enum EnInteraction
{
    None = 0,
    MainQuest = 1,
    SecretLetter = 2,
    MysteryBox = 3,
    DimensionRaid = 4,
    GuildAltarSpellInjection = 5,
    GuildMission = 6,
    Dungeon = 7,
    TrueHeroQuest = 8,
    SubQuest = 9,
    GuildFarm = 10, 
    NpcDialog = 11, 
}

public class CsPanelInteraction : MonoBehaviour
{
    [SerializeField] GameObject m_goPopupSecretLetter;

    Transform m_trInteraction;
    Image m_ImageSlider;
    Text m_TextPercent;
    Text m_TextState;
    Button m_buttonInteraction;

    int m_nInteractionNpcId;
    long m_lInstanceId;

    float m_flReturnScrollCastTime;
    float m_flFishingCastingIntervalTime;
    float m_flMysteryBoxIntervalTime;
    float m_flDimensionRaidQuestIntervalTime;
    float m_flGuildFarmRemainingTime;
    float m_flGuildAlterSpellInjectionRemainingTime;
    float m_flGroggyMonsterItemStealRemainingTime;

	float m_flInteractionRemainingTime = 0;
	float m_flInteractionDuration = 0;

	float m_flDungeonInteractionRemainingTime = 0f;
	float m_flDungeonInteractionDuration;
	
    bool m_bCompletCheck = false;

    Dictionary<string, bool> m_dicEnterInteractionArea = new Dictionary<string, bool>();

    IEnumerator m_IEnumeratorImageChange;
    EnInteractionQuestType m_enInteractionQuestType = EnInteractionQuestType.None;
	EnInteraction m_enInteration = EnInteraction.None;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        // 귀환 주문서
        CsGameEventUIToUI.Instance.EventReturnScrollUseStart += OnEventReturnScrollUseStart;
        CsGameEventUIToUI.Instance.EventReturnScrollUseCancel += OnEventReturnScrollUseCancel;
        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished += OnEventReturnScrollUseFinished;
        CsGameEventToUI.Instance.EventReturnScrollUseCancel += OnEventReturnScrollUseCancelByIngame;

        //상호작용 오브젝트
		CsContinentObjectManager.Instance.EventChangeInteractionState += OnEventChangeInteractionState;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionStarted += OnEventContinentObjectInteractionStarted;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionCancel += OnEventMyHeroContinentObjectInteractionCancel;
		CsContinentObjectManager.Instance.EventSendContinentObjectInteractionCancel += OnEventSendContinentObjectInteractionCancel;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionFinished += EventMyHeroContinentObjectInteractionFinished;

		// 던전
		CsDungeonManager.Instance.EventChangeDungeonInteractionState += OnEventChangeDungeonInteractionState;
		
		// 낚시
        CsFishingQuestManager.Instance.EventFishingStart += OnEventFishingStart;
        CsFishingQuestManager.Instance.EventFishingCastingCompleted += OnEventFishingCastingCompleted;
        CsFishingQuestManager.Instance.EventFishingCanceled += OnEventFishingCanceled;
        CsFishingQuestManager.Instance.EventMyHeroFishingCanceled += OnEventMyHeroFishingCanceled;

        // 밀서
        CsSecretLetterQuestManager.Instance.EventInteractionArea += OnEventSecretLetterInteractionArea;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickStart += OnEventSecretLetterPickStart;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCompleted += OnEventSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCanceled += OnEventSecretLetterPickCanceled;
        CsSecretLetterQuestManager.Instance.EventMyHeroSecretLetterPickCanceled += OnEventMyHeroSecretLetterPickCanceled;

        // 의문의박스
        CsMysteryBoxQuestManager.Instance.EventInteractionArea += OnEventMysteryBoxInteractionArea;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickStart += OnEventMysteryBoxPickStart;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCompleted += OnEventMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCanceled += OnEventMysteryBoxPickCanceled;
        CsMysteryBoxQuestManager.Instance.EventMyHeroMysteryBoxPickCancel += OnEventMyHeroMysteryBoxPickCancel;

        // 차원의습격
        CsDimensionRaidQuestManager.Instance.EventInteractionArea += OnEventDimensionRaidInteractionArea;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionStart += OnEventDimensionRaidInteractionStart;
        CsDimensionRaidQuestManager.Instance.EventMyHeroDimensionRaidInteractionCancel += OnEventMyHeroDimensionRaidInteractionCancel;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCanceled += OnEventDimensionRaidInteractionCanceled;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCompleted += OnEventDimensionRaidInteractionCompleted;

        // 길드농장퀘스트
        CsGuildManager.Instance.EventFarmInteractionArea += OnEventFarmInteractionArea;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionStart += OnEventGuildFarmQuestInteractionStart;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCanceled += OnEventGuildFarmQuestInteractionCanceled;
        CsGuildManager.Instance.EventMyHeroGuildFarmQuestInteractionCancel += OnEventMyHeroGuildFarmQuestInteractionCancel;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCompleted += OnEventGuildFarmQuestInteractionCompleted;
        
        // 길드 마력주입 퀘스트
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionStart += OnEventGuildAltarSpellInjectionMissionStart;
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionCompleted += OnEventGuildAltarSpellInjectionMissionCompleted;
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionCanceled += OnEventGuildAltarSpellInjectionMissionCanceled;
        CsGuildManager.Instance.EventMyHeroGuildAltarSpellInjectionMissionCancel += OnEventMyHeroGuildAltarSpellInjectionMissionCancel;
        CsGuildManager.Instance.EventGuildAltarGoOut += OnEventGuildAltarGoOut;

        // 길드 기부, 제단수비 퀘스트
        CsGuildManager.Instance.EventGuildAltarDefenseMissionStart += OnEventGuildAltarDefenseMissionStart;
        CsGuildManager.Instance.EventGuildAltarDonate += OnEventGuildAltarDonate;

        // 길드 미션
        CsGuildManager.Instance.EventGuildSpiritArea += OnEventGuildSpiritArea;
        CsGuildManager.Instance.EventGuildMissionComplete += OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildMissionAbandon += OnEventGuildMissionAbandon;

        // 길드 현상금
        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;

        // 그로기 몬스터 아이템 훔치기
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealStart += OnEventGroggyMonsterItemStealStart;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealCancel += OnEventGroggyMonsterItemStealCancel;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealFinished += OnEventGroggyMonsterItemStealFinished;
        CsGameEventToUI.Instance.EventGroggyMonsterItemStealCancel += OnEventGroggyMonsterItemStealCancelByIngame;

		// 진정한 영웅
		CsTrueHeroQuestManager.Instance.EventChangeInteractionState += OnEventTrueHeroQuestChangeInteractionState;

        //NPC 대화
        CsGameEventToUI.Instance.EventNpcInteractionArea += OnEventNpcInteractionArea;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        // 귀환 스크롤
        if (CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup > 0)
        {
            UpdateInteractionUseReturnScroll();
        }

        // 그로기 몬스터
        if (m_flGroggyMonsterItemStealRemainingTime - Time.realtimeSinceStartup > 0)
        {
            UpdateInteractionGroggyMonsterItemSteal();
        }

        // 낚시
        if (m_flFishingCastingIntervalTime - Time.realtimeSinceStartup > 0)
        {
            UpdateInteractionFishing();
        }

        // 의문의 상자
        if (m_flMysteryBoxIntervalTime - Time.realtimeSinceStartup > 0)
        {
            UpdateInteractionMysteryBox();
        }

        // 차원의 습격
        if (m_flDimensionRaidQuestIntervalTime - Time.realtimeSinceStartup > 0)
        {
            UpdateInteractionDimensionRaidQuest();
        }

        // 길드 농장
        if (m_flGuildFarmRemainingTime - Time.realtimeSinceStartup > 0)
        {
            UpdateGuildFarmQuest();
        }

        // 길드 제단 주입
        if (m_flGuildAlterSpellInjectionRemainingTime - Time.realtimeSinceStartup > 0)
        {
            UpdateGuildAltarSpellInjection();
        }

        if (m_flDungeonInteractionRemainingTime - Time.realtimeSinceStartup > 0)
		{
			if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.WisdomTemple)
			{
				UpdateWisdomTempleInteraction();
			}
			else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.RuinsReclaim)
			{
				UpdateRuinsReclaimObjectInteraction();
			}
            else if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.WarMemory)
            {
                UpdateWarMemoryObjectInteraction();
            }
		}

		if (m_flInteractionRemainingTime - Time.realtimeSinceStartup > 0)
		{
			UpdateQuestObjectInteraction();
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        // 귀환 주문서
        CsGameEventUIToUI.Instance.EventReturnScrollUseStart -= OnEventReturnScrollUseStart;
        CsGameEventUIToUI.Instance.EventReturnScrollUseCancel -= OnEventReturnScrollUseCancel;
        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished -= OnEventReturnScrollUseFinished;
        CsGameEventToUI.Instance.EventReturnScrollUseCancel -= OnEventReturnScrollUseCancelByIngame;

        //상호작용 오브젝트
        CsContinentObjectManager.Instance.EventChangeInteractionState -= OnEventChangeInteractionState;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionStarted -= OnEventContinentObjectInteractionStarted;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionCancel -= OnEventMyHeroContinentObjectInteractionCancel;
        CsContinentObjectManager.Instance.EventSendContinentObjectInteractionCancel -= OnEventSendContinentObjectInteractionCancel;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionFinished -= EventMyHeroContinentObjectInteractionFinished;

        // 던전
        CsDungeonManager.Instance.EventChangeDungeonInteractionState -= OnEventChangeDungeonInteractionState;

        // 낚시
        CsFishingQuestManager.Instance.EventFishingStart -= OnEventFishingStart;
        CsFishingQuestManager.Instance.EventFishingCastingCompleted -= OnEventFishingCastingCompleted;
        CsFishingQuestManager.Instance.EventFishingCanceled -= OnEventFishingCanceled;
        CsFishingQuestManager.Instance.EventMyHeroFishingCanceled -= OnEventMyHeroFishingCanceled;

        // 밀서
        CsSecretLetterQuestManager.Instance.EventInteractionArea -= OnEventSecretLetterInteractionArea;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickStart -= OnEventSecretLetterPickStart;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCompleted -= OnEventSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCanceled -= OnEventSecretLetterPickCanceled;
        CsSecretLetterQuestManager.Instance.EventMyHeroSecretLetterPickCanceled -= OnEventMyHeroSecretLetterPickCanceled;

        // 의문의박스
        CsMysteryBoxQuestManager.Instance.EventInteractionArea -= OnEventMysteryBoxInteractionArea;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickStart -= OnEventMysteryBoxPickStart;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCompleted -= OnEventMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCanceled -= OnEventMysteryBoxPickCanceled;
        CsMysteryBoxQuestManager.Instance.EventMyHeroMysteryBoxPickCancel -= OnEventMyHeroMysteryBoxPickCancel;

        // 차원의습격
        CsDimensionRaidQuestManager.Instance.EventInteractionArea -= OnEventDimensionRaidInteractionArea;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionStart -= OnEventDimensionRaidInteractionStart;
        CsDimensionRaidQuestManager.Instance.EventMyHeroDimensionRaidInteractionCancel -= OnEventMyHeroDimensionRaidInteractionCancel;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCanceled -= OnEventDimensionRaidInteractionCanceled;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCompleted -= OnEventDimensionRaidInteractionCompleted;

        // 길드농장퀘스트
        CsGuildManager.Instance.EventFarmInteractionArea -= OnEventFarmInteractionArea;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionStart -= OnEventGuildFarmQuestInteractionStart;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCanceled -= OnEventGuildFarmQuestInteractionCanceled;
        CsGuildManager.Instance.EventMyHeroGuildFarmQuestInteractionCancel -= OnEventMyHeroGuildFarmQuestInteractionCancel;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCompleted -= OnEventGuildFarmQuestInteractionCompleted;

        // 길드 마력주입 퀘스트
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionStart -= OnEventGuildAltarSpellInjectionMissionStart;
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionCompleted -= OnEventGuildAltarSpellInjectionMissionCompleted;
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionCanceled -= OnEventGuildAltarSpellInjectionMissionCanceled;
        CsGuildManager.Instance.EventMyHeroGuildAltarSpellInjectionMissionCancel -= OnEventMyHeroGuildAltarSpellInjectionMissionCancel;
        CsGuildManager.Instance.EventGuildAltarGoOut -= OnEventGuildAltarGoOut;

        // 길드 기부, 제단수비 퀘스트
        CsGuildManager.Instance.EventGuildAltarDefenseMissionStart -= OnEventGuildAltarDefenseMissionStart;
        CsGuildManager.Instance.EventGuildAltarDonate -= OnEventGuildAltarDonate;

        // 길드 미션
        CsGuildManager.Instance.EventGuildSpiritArea -= OnEventGuildSpiritArea;
        CsGuildManager.Instance.EventGuildMissionComplete -= OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildMissionAbandon -= OnEventGuildMissionAbandon;

        // 길드 현상금
        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;

        // 그로기 몬스터 아이템 훔치기
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealStart -= OnEventGroggyMonsterItemStealStart;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealCancel -= OnEventGroggyMonsterItemStealCancel;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealFinished -= OnEventGroggyMonsterItemStealFinished;
        CsGameEventToUI.Instance.EventGroggyMonsterItemStealCancel -= OnEventGroggyMonsterItemStealCancelByIngame;

        // 진정한 영웅
        CsTrueHeroQuestManager.Instance.EventChangeInteractionState -= OnEventTrueHeroQuestChangeInteractionState;

        //NPC 대화
        CsGameEventToUI.Instance.EventNpcInteractionArea -= OnEventNpcInteractionArea;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickMainQuest()
    {
        //CsGameEventToIngame.Instance.OnEventContinentObjectInteractionStart();
        if (CsCartManager.Instance.IsMyHeroRidingCart)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_CART_INTERROR"));
        }
        else
        {
            CsContinentObjectManager.Instance.ContinentObjectInteractionStart();
        }
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickSecretLetter()
    {
        if (CsCartManager.Instance.IsMyHeroRidingCart)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_CART_INTERROR"));
        }
        else
        {
            CsSecretLetterQuestManager.Instance.StartSecretLetterPickStart();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMysteryBox()
    {
        if (CsCartManager.Instance.IsMyHeroRidingCart)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_CART_INTERROR"));
        }
        else
        {
            CsMysteryBoxQuestManager.Instance.StartMysteryBoxPickStart();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClicDimensionRaid()
    {
        if (CsCartManager.Instance.IsMyHeroRidingCart)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_CART_INTERROR"));
        }
        else
        {
            CsDimensionRaidQuestManager.Instance.StartDimensionRaidInteractionStart();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReStartSpellInjection()
    {
        CsGuildManager.Instance.SendGuildAltarSpellInjectionMissionStart();
    }


	//---------------------------------------------------------------------------------------------------
	void OnClickDungeonInteraction()
	{
		CsDungeonManager.Instance.DugeonObjectInteractionStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickTrueHeroInteraction()
	{
		CsTrueHeroQuestManager.Instance.MyHeroTrueHeroQuestInteractionStart();
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildFarmInteraction()
    {
        if (CsCartManager.Instance.IsMyHeroRidingCart)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_CART_INTERROR"));
        }
        else
        {
            CsGuildManager.Instance.FarmInteraction();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNpcInteraction()
    {
        CsGameEventToIngame.Instance.OnEventRequestNpcDialog();
    }
    
    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bChaegeScene)
    {
        ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    //길드미션

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSpiritArea(bool bEnter)
    {
        ChangeButtonInteractionState(EnInteraction.GuildMission, bEnter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionComplete(bool bLevelUp, long lAcquredExp)
    {
        ChangeButtonInteractionState(EnInteraction.GuildMission, false);
        ResetInteractionSlider();
        m_flInteractionRemainingTime = 0.0f;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionAbandon()
    {
        ResetInteractionSlider();
        m_flInteractionRemainingTime = 0.0f;
    }

    //길드 제단
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDefenseMissionStart()
    {
        ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDonate()
    {
        if (CsGuildManager.Instance.GuildMoralPoint >= CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint)
        {
            ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, false);
        }
        else
        {
            return;
        }
    }

    // 길드 마력주입
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarGoOut(bool bAltar)
    {
        if (!bAltar)
        {
            ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, false);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionStart()
    {
        m_flGuildAlterSpellInjectionRemainingTime = CsGameData.Instance.GuildAltar.SpellInjectionDuration + Time.realtimeSinceStartup;
        m_trInteraction.gameObject.SetActive(true);

        //버튼끄기
        ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCompleted()
    {
        ResetInteractionSlider();
        m_flGuildAlterSpellInjectionRemainingTime = 0;
        ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, false);

        //포인트 체크해서 최대가 아니면 다시 스타트
        if (CsGuildManager.Instance.GuildMoralPoint < CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint)
        {
            CsGuildManager.Instance.SendGuildAltarSpellInjectionMissionStart();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCanceled()
    {
        ResetInteractionSlider();
        m_flGuildAlterSpellInjectionRemainingTime = 0;

        //상태체크해서 내가 아직 안쪽에 있으면 버튼켜기
        if (CsGuildManager.Instance.AltarEnter)
        {
            ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, true);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroGuildAltarSpellInjectionMissionCancel()
    {
        ResetInteractionSlider();
        m_flGuildAlterSpellInjectionRemainingTime = 0;

        //상태체크해서 내가 아직 안쪽에 있으면 버튼켜기
        if (CsGuildManager.Instance.AltarEnter)
        {
            ChangeButtonInteractionState(EnInteraction.GuildAltarSpellInjection, true);
        }
        else
        {
            return;
        }
    }

    // 길드 농장

    //---------------------------------------------------------------------------------------------------
    void OnEventFarmInteractionArea(bool bEnter)
    {
        ChangeButtonInteractionState(EnInteraction.GuildFarm, bEnter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionStart()
    {
        m_flGuildFarmRemainingTime = CsGameData.Instance.GuildFarmQuest.InteractionDuration + Time.realtimeSinceStartup;
        m_trInteraction.gameObject.SetActive(true);
        ChangeButtonInteractionState(EnInteraction.GuildFarm, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionCanceled()
    {
        ResetInteractionSlider();
        m_flGuildFarmRemainingTime = 0;
        ChangeButtonInteractionState(EnInteraction.GuildFarm, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroGuildFarmQuestInteractionCancel()
    {
        ResetInteractionSlider();
        m_flGuildFarmRemainingTime = 0;
        ChangeButtonInteractionState(EnInteraction.GuildFarm, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionCompleted()
    {
        ResetInteractionSlider();
        m_flGuildFarmRemainingTime = 0;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentObjectInteractionStarted(long lInstanceId, EnInteractionQuestType enInteractionQuestTypeValue, CsContinentObject csContinentObject)
    {
        m_enInteractionQuestType = enInteractionQuestTypeValue;

		m_flInteractionDuration = csContinentObject.InteractionDuration;
		m_flInteractionRemainingTime = csContinentObject.InteractionDuration + Time.realtimeSinceStartup;
		m_TextState.text = csContinentObject.InteractionText;

		m_trInteraction.gameObject.SetActive(true);
        ChangeButtonInteractionState(EnInteraction.MainQuest, false);
    }
    
    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroContinentObjectInteractionCancel()
    {
        ResetInteractionSlider();
        m_flInteractionRemainingTime = 0;
        ChangeButtonInteractionState(EnInteraction.MainQuest, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSendContinentObjectInteractionCancel()
    {
        ResetInteractionSlider();

        m_flInteractionRemainingTime = 0;
        ChangeButtonInteractionState(EnInteraction.MainQuest, true);
    }

    //---------------------------------------------------------------------------------------------------
    void EventMyHeroContinentObjectInteractionFinished(long lInstanceId)
    {
        ResetInteractionSlider();
        m_flInteractionRemainingTime = 0;
    }

	// 던전
	//---------------------------------------------------------------------------------------------------
	void OnEventChangeDungeonInteractionState(EnInteractionState enInteractionState)
	{
		// 버튼 표시여부
		bool bEnter = enInteractionState == EnInteractionState.ViewButton;
		ChangeButtonInteractionState(EnInteraction.Dungeon, bEnter);

		// 상호작용 상태 변경 처리
		if (enInteractionState == EnInteractionState.interacting)
		{
			switch (CsDungeonManager.Instance.DungeonPlay)
			{
				case EnDungeonPlay.WisdomTemple:

					if (CsDungeonManager.Instance.WisdomTempleType == EnWisdomTempleType.ColorMatching)
					{
						m_TextState.text = CsConfiguration.Instance.GetString("A105_TXT_00003");
					}
					else if (CsDungeonManager.Instance.WisdomTempleType == EnWisdomTempleType.PuzzleReward)
					{
						m_TextState.text = CsConfiguration.Instance.GetString("A105_TXT_00004");
					}

					break;

				case EnDungeonPlay.RuinsReclaim:

					if (CsDungeonManager.Instance.RuinsReclaimStep.Type == 2)
					{
						CsRuinsReclaimObjectArrange csRuinsReclaimObjectArrange = CsDungeonManager.Instance.RuinsReclaimStep.GetRuinsReclaimObjectArrange(CsDungeonManager.Instance.ArrangeNo);

						if (csRuinsReclaimObjectArrange != null)
						{
							m_TextState.text = csRuinsReclaimObjectArrange.ObjectInteractionText;
						}
					}
					else
					{
						CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill = CsDungeonManager.Instance.RuinsReclaimStep.GetRuinsReclaimStepWaveSkill(CsDungeonManager.Instance.RuinsReclaimStepWave.WaveNo);

						if (csRuinsReclaimStepWaveSkill != null)
						{
							m_TextState.text = csRuinsReclaimStepWaveSkill.ObjectInteractionText;
						}
					}

					break;

                case EnDungeonPlay.WarMemory:

                    if (CsDungeonManager.Instance.WarMemoryWave != null)
                    {
                        Debug.Log("CsDungeonManager.Instance.InteractionObjectId : " + CsDungeonManager.Instance.InteractionObjectId);

                        if (CsDungeonManager.Instance.WarMemoryWave.GetTransformationObject(CsDungeonManager.Instance.InteractionObjectId) == null)
                        {
                            m_TextState.text = "";
                        }
                        else
                        {
                            m_TextState.text = CsDungeonManager.Instance.WarMemoryWave.GetTransformationObject(CsDungeonManager.Instance.InteractionObjectId).ObjectInteractionText;
                        }
                    }
                    else
                    {
                        m_TextState.text = "";
                    }

                    break;
			}

			m_flDungeonInteractionDuration = CsDungeonManager.Instance.InteractionDuration;
			m_flDungeonInteractionRemainingTime = m_flDungeonInteractionDuration + Time.realtimeSinceStartup;
			m_trInteraction.gameObject.SetActive(true);
		}
		else
		{
			ResetInteractionSlider();
			m_flDungeonInteractionRemainingTime = 0;
			m_flDungeonInteractionDuration = 0;
		}
	}

	#region GroggyMonster

    // 그로기 몬스터
    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealStart()
    {
        m_trInteraction.gameObject.SetActive(true);
        m_flGroggyMonsterItemStealRemainingTime = CsGameConfig.Instance.MonsterStealDuration + Time.realtimeSinceStartup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealFinished(CsItem csItem, bool bOwned, int nCount)
    {
        m_flGroggyMonsterItemStealRemainingTime = 0.0f;
        ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealCancel()
    {
        m_flGroggyMonsterItemStealRemainingTime = 0.0f;
        ResetInteractionSlider();
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealCancelByIngame()
    {
        CsCommandEventManager.Instance.SendGroggyMonsterItemStealCancel();
        m_flGroggyMonsterItemStealRemainingTime = 0.0f;
        ResetInteractionSlider();
    }

    // 그로기 몬스터
    #endregion GroggyMonster

	//---------------------------------------------------------------------------------------------------
	void OnEventChangeInteractionState(EnInteractionState enInteractionState)
    {
        int nObjectId = CsContinentObjectManager.Instance.InteractionObjectId;

		bool bEnter = (enInteractionState == EnInteractionState.ViewButton) ? true : false;
		ChangeButtonInteractionState(EnInteraction.MainQuest, bEnter);

        //튜토리얼 체크
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;
        
        if (enInteractionState == EnInteractionState.ViewButton && csMainQuest != null && csMainQuest.MainQuestNo < 2 && csMainQuest.TargetContinentObject.ObjectId == nObjectId &&
            CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Accepted && CsConfiguration.Instance.GetTutorialKey(EnTutorialType.Interaction))
        {
            CsGameEventUIToUI.Instance.OnEventReferenceTutorial(EnTutorialType.Interaction);
        }
        else
        {
            return;
        }
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventReturnScrollUseStart()
    {
        m_trInteraction.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseCancelByIngame()
    {
        CsCommandEventManager.Instance.SendReturnScrollUseCancel();
        ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseCancel()
    {
        ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseFinished(int nContinentId)
    {
        ResetInteractionSlider();
    }

    #region Fishing

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingStart()
    {
        m_flFishingCastingIntervalTime = CsGameData.Instance.FishingQuest.CastingInterval + Time.realtimeSinceStartup;
        m_trInteraction.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingCastingCompleted(bool bLevelUp, long lAcquiredExp, bool bBaitEnable)
    {
        if (bBaitEnable)
            m_flFishingCastingIntervalTime = CsGameData.Instance.FishingQuest.CastingInterval + Time.realtimeSinceStartup;
        else
            ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingCanceled()
    {
        ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroFishingCanceled()
    {
        ResetInteractionSlider();
    }

    #endregion Fishing

    #region SecretLetterQuest

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterInteractionArea(bool bEnter)
    {
        ChangeButtonInteractionState(EnInteraction.SecretLetter, bEnter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickStart()
    {
        m_bCompletCheck = false;
        ChangeButtonInteractionState(EnInteraction.SecretLetter, false);
        if (m_IEnumeratorImageChange != null)
        {
            StopCoroutine(m_IEnumeratorImageChange);
            m_IEnumeratorImageChange = null;
        }

        m_IEnumeratorImageChange = SecretLetterImageChange(CsSecretLetterQuestManager.Instance.PickCount + 1);
        OpenPopupSecretLetter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickCompleted()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed)
            ChangeButtonInteractionState(EnInteraction.SecretLetter, false);
        else
            ChangeButtonInteractionState(EnInteraction.SecretLetter, true);

        if (m_IEnumeratorImageChange != null)
        {
            m_bCompletCheck = true;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroSecretLetterPickCanceled()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.Completed && CsSecretLetterQuestManager.Instance.InteractionButton)
            ChangeButtonInteractionState(EnInteraction.SecretLetter, true);
        else
            ChangeButtonInteractionState(EnInteraction.SecretLetter, false);

        ClosePopupSecretLetter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickCanceled()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.Completed && CsSecretLetterQuestManager.Instance.InteractionButton)
            ChangeButtonInteractionState(EnInteraction.SecretLetter, true);
        else
            ChangeButtonInteractionState(EnInteraction.SecretLetter, false);

        ClosePopupSecretLetter();
    }

    #endregion SecretLetterQuest

    #region MysteryBox

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxInteractionArea(bool bEnter)
    {
        ChangeButtonInteractionState(EnInteraction.MysteryBox, bEnter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickStart()
    {
        m_flMysteryBoxIntervalTime = CsGameData.Instance.MysteryBoxQuest.InteractionDuration + Time.realtimeSinceStartup;
        m_trInteraction.gameObject.SetActive(true);
        ChangeButtonInteractionState(EnInteraction.MysteryBox, false);

    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickCompleted()
    {
        ResetInteractionSlider();

        if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Accepted ||
            CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Executed &&
            CsGameData.Instance.MyHeroInfo.Nation.NationId != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
        {
            ChangeButtonInteractionState(EnInteraction.MysteryBox, true);
        }
        else
        {
            ChangeButtonInteractionState(EnInteraction.MysteryBox, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickCanceled()
    {
        ResetInteractionSlider();
        ChangeButtonInteractionState(EnInteraction.MysteryBox, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroMysteryBoxPickCancel()
    {
        ResetInteractionSlider();
        ChangeButtonInteractionState(EnInteraction.MysteryBox, true);
    }

    #endregion MysteryBox

    #region DimensionRaidQuest

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionArea(bool bEnter)
    {
        ChangeButtonInteractionState(EnInteraction.DimensionRaid, bEnter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionStart()
    {
        m_flDimensionRaidQuestIntervalTime = CsGameData.Instance.DimensionRaidQuest.TargetInteractionDuration + Time.realtimeSinceStartup;
        m_trInteraction.gameObject.SetActive(true);
        ChangeButtonInteractionState(EnInteraction.DimensionRaid, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroDimensionRaidInteractionCancel()
    {
        ResetInteractionSlider();
        ChangeButtonInteractionState(EnInteraction.DimensionRaid, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionCanceled()
    {
        ResetInteractionSlider();
        ChangeButtonInteractionState(EnInteraction.DimensionRaid, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionCompleted()
    {
        ResetInteractionSlider();
    }

    #endregion DimensionRaidQuest

	#region TrueHeroQuest
	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestChangeInteractionState(EnInteractionState enInteractionState)
	{
		bool bEnter = enInteractionState == EnInteractionState.ViewButton;
		ChangeButtonInteractionState(EnInteraction.TrueHeroQuest, bEnter);

		if (enInteractionState == EnInteractionState.interacting)
		{
			m_TextState.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetObjectInteractionText;

			m_flInteractionRemainingTime = CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetObjectInteractionDuration + Time.realtimeSinceStartup;
			m_flInteractionDuration = CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetObjectInteractionDuration;

			m_trInteraction.gameObject.SetActive(true);
		}
		else
		{
			ResetInteractionSlider();
			m_flInteractionRemainingTime = 0;
			m_flInteractionDuration = 0;
		}
	}

	#endregion TrueHeroQuest

    //---------------------------------------------------------------------------------------------------
    void OnEventNpcInteractionArea(bool bIson, int nNpcId)
    {
        Debug.Log("#@#@ OnEventNpcInteractionArea #@#@ : " + bIson);
        if (bIson)
        {
            // 튜토리얼 체크
            CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;
            m_nInteractionNpcId = nNpcId;

            if (csMainQuest != null && csMainQuest.MainQuestNo < 2 && ((csMainQuest.StartNpc != null && csMainQuest.StartNpc.NpcId == nNpcId) || (csMainQuest.CompletionNpc != null && csMainQuest.CompletionNpc.NpcId == nNpcId)) && 
                (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None || CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed) && CsConfiguration.Instance.GetTutorialKey(EnTutorialType.Dialog))
            {
                CsGameEventUIToUI.Instance.OnEventReferenceTutorial(EnTutorialType.Dialog);
            }

            ChangeButtonInteractionState(EnInteraction.NpcDialog, bIson);
        }
        else
        {
            if (m_nInteractionNpcId == nNpcId)
            {
                ChangeButtonInteractionState(EnInteraction.NpcDialog, bIson);
            }
        }
    }

	#endregion EventHandler

	//---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trInteraction = transform.Find("InteractionSlider");

        m_buttonInteraction = transform.Find("ButtonInteraction").GetComponent<Button>();
        m_buttonInteraction.onClick.RemoveAllListeners();
		m_buttonInteraction.onClick.AddListener(OnClickButtonInteraction);
        m_buttonInteraction.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_ImageSlider = m_trInteraction.Find("ImageGuage").GetComponent<Image>();

        m_TextPercent = m_trInteraction.Find("TextPercent").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextPercent);

        m_TextState = m_trInteraction.Find("TextState").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextState);

        CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.ReturnScrollId);
        m_flReturnScrollCastTime = csItem.Value2;

        ResetInteractionSlider();
    }

    //---------------------------------------------------------------------------------------------------
    void ResetInteractionSlider()
    {
        m_trInteraction.gameObject.SetActive(false);
        m_ImageSlider.fillAmount = 0;
        m_TextPercent.text = "";
        m_TextState.text = "";
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractionGroggyMonsterItemSteal()
    {
        float flRemainingTime = m_flGroggyMonsterItemStealRemainingTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / CsGameConfig.Instance.MonsterStealDuration);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / CsGameConfig.Instance.MonsterStealDuration * 100)).ToString("#0"));
        m_TextState.text = CsConfiguration.Instance.GetString("A17_TXT_04003");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractionUseReturnScroll()
    {
        float flRemainingTime = CsUIData.Instance.ReturnScrollRemainingCastTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / m_flReturnScrollCastTime);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / m_flReturnScrollCastTime * 100)).ToString("#0"));
        m_TextState.text = CsConfiguration.Instance.GetString("A02_TXT_00024");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractionFishing()
    {
        float flRemainingTime = m_flFishingCastingIntervalTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / CsGameData.Instance.FishingQuest.CastingInterval);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / CsGameData.Instance.FishingQuest.CastingInterval * 100)).ToString("#0"));
        m_TextState.text = string.Format(CsConfiguration.Instance.GetString("A46_TXT_01001"), (CsGameData.Instance.FishingQuest.CastingCount - CsFishingQuestManager.Instance.CastingCount));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractionMysteryBox()
    {
        float flRemainingTime = m_flMysteryBoxIntervalTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / CsGameData.Instance.MysteryBoxQuest.InteractionDuration);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / CsGameData.Instance.MysteryBoxQuest.InteractionDuration * 100)).ToString("#0"));
        m_TextState.text = string.Format(CsConfiguration.Instance.GetString("A51_TXT_01001"), CsMysteryBoxQuestManager.Instance.PickCount + 1);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractionDimensionRaidQuest()
    {
        float flRemainingTime = m_flDimensionRaidQuestIntervalTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / CsGameData.Instance.DimensionRaidQuest.TargetInteractionDuration);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / CsGameData.Instance.DimensionRaidQuest.TargetInteractionDuration * 100)).ToString("#0"));
        m_TextState.text = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestStep(CsDimensionRaidQuestManager.Instance.Step).TargetInteractionText;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildFarmQuest()
    {
        float flRemainingTime = m_flGuildFarmRemainingTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / CsGameData.Instance.GuildFarmQuest.InteractionDuration);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / CsGameData.Instance.GuildFarmQuest.InteractionDuration * 100)).ToString("#0"));
        m_TextState.text = CsGameData.Instance.GuildFarmQuest.InteractionText;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildAltarSpellInjection()
    {
        float flRemainingTime = m_flGuildAlterSpellInjectionRemainingTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / CsGameData.Instance.GuildAltar.SpellInjectionDuration);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / CsGameData.Instance.GuildAltar.SpellInjectionDuration * 100)).ToString("#0"));
        m_TextState.text = CsConfiguration.Instance.GetString("A68_TXT_00006");
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateWisdomTempleInteraction()
	{
		float flRemainingTime = m_flDungeonInteractionRemainingTime - Time.realtimeSinceStartup;
		m_ImageSlider.fillAmount = 1 - (flRemainingTime / m_flDungeonInteractionDuration);
		m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / m_flDungeonInteractionDuration * 100)).ToString("#0"));
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateRuinsReclaimObjectInteraction()
	{
		float flRemainingTime = m_flDungeonInteractionRemainingTime - Time.realtimeSinceStartup;
		m_ImageSlider.fillAmount = 1 - (flRemainingTime / m_flDungeonInteractionDuration);
		m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / m_flDungeonInteractionDuration * 100)).ToString("#0"));
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateWarMemoryObjectInteraction()
    {
        float flRemainingTime = m_flDungeonInteractionRemainingTime - Time.realtimeSinceStartup;
        m_ImageSlider.fillAmount = 1 - (flRemainingTime / m_flDungeonInteractionDuration);
        m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / m_flDungeonInteractionDuration * 100)).ToString("#0"));
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateQuestObjectInteraction()
	{
		float flRemainingTime = m_flInteractionRemainingTime - Time.realtimeSinceStartup;
		m_ImageSlider.fillAmount = 1 - (flRemainingTime / m_flInteractionDuration);
		m_TextPercent.text = string.Format(CsConfiguration.Instance.GetString("A02_TXT_01006"), (100 - (flRemainingTime / m_flInteractionDuration * 100)).ToString("#0"));
	}

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSecretLetter()
    {
        Transform trSecretLetter = transform.Find("PopupSecretLetter");

        if (trSecretLetter == null)
        {
            trSecretLetter = Instantiate(m_goPopupSecretLetter, transform).transform;
            trSecretLetter.name = m_goPopupSecretLetter.name;
        }

        StartCoroutine(m_IEnumeratorImageChange);
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupSecretLetter()
    {
        if (m_IEnumeratorImageChange != null)
        {
            StopCoroutine(m_IEnumeratorImageChange);
            m_IEnumeratorImageChange = null;
        }

        Transform trSecretLetter = transform.Find("PopupSecretLetter");

        if (trSecretLetter != null)
        {
            Destroy(trSecretLetter.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator SecretLetterImageChange(int Count)
    {
        int nImageNumber = 0;

        Image ImageChangeSecretLetter = transform.Find("PopupSecretLetter/ImageBackground/ImageScroll").GetComponent<Image>();
        Text textCount = transform.Find("PopupSecretLetter/ImageBackground/Text").GetComponent<Text>();

        if (ImageChangeSecretLetter == null || textCount == null)
            yield break;

        textCount.text = string.Format(CsConfiguration.Instance.GetString("A49_TXT_01001"), Count);

        while (true)
        {
            if (m_bCompletCheck)
            {
                nImageNumber = CsSecretLetterQuestManager.Instance.PickedLetterGrade;
                ImageChangeSecretLetter.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_scroll0" + nImageNumber);
                yield return new WaitForSeconds(1f);
                ClosePopupSecretLetter();
                yield break;
            }
            else
            {
                if (nImageNumber > 4)
                    nImageNumber = 1;
                else
                    nImageNumber++;

                ImageChangeSecretLetter.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_quest_scroll0" + nImageNumber);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ChangeButtonInteractionState(EnInteraction enInteraction, bool bEnter)
    {
		m_enInteration = enInteraction;

        if (m_dicEnterInteractionArea.ContainsKey(enInteraction.ToString()))
        {
            m_dicEnterInteractionArea[enInteraction.ToString()] = bEnter;
        }
        else
        {
            m_dicEnterInteractionArea.Add(enInteraction.ToString(), bEnter);
        }

		if (bEnter)
        {
            Image imageIcon = m_buttonInteraction.transform.Find("ImageIcon").GetComponent<Image>();

            switch (enInteraction)
            {
                case EnInteraction.MainQuest:
				case EnInteraction.SubQuest:
                case EnInteraction.GuildFarm:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_detail");
                    imageIcon.rectTransform.sizeDelta = new Vector2(40f, 44f);
                    break;

                case EnInteraction.SecretLetter:
					if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Accepted ||
						CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Executed &&
						CsSecretLetterQuestManager.Instance.TargetNationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
					{
						bEnter = true;
					}
					else
					{
						bEnter = false;
					}

                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_speech");
                    imageIcon.rectTransform.sizeDelta = new Vector2(44f, 36f);
                    break;

                case EnInteraction.MysteryBox:
                    if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Accepted ||
                        CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Executed &&
                        CsGameData.Instance.MyHeroInfo.Nation.NationId != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                    {
						bEnter = true;
                    }
					else
					{
						bEnter = false;
					}

					imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_detail");
                    imageIcon.rectTransform.sizeDelta = new Vector2(40f, 44f);
                    break;

                case EnInteraction.DimensionRaid:
                    if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Accepted &&
                        CsGameData.Instance.MyHeroInfo.Nation.NationId != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                    {
						bEnter = true;
                    }
					else
					{
						bEnter = false;
					}

                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_fire");
                    imageIcon.rectTransform.sizeDelta = new Vector2(40f, 52f);
                    break;

                case EnInteraction.GuildAltarSpellInjection:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_guild_interaction_1");
                    imageIcon.rectTransform.sizeDelta = new Vector2(40f, 52f);
					break;

                case EnInteraction.GuildMission:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_guild_interaction_2");
                    imageIcon.rectTransform.sizeDelta = new Vector2(48f, 56f);
                    break;

				case EnInteraction.Dungeon:
					imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_detail");
                    imageIcon.rectTransform.sizeDelta = new Vector2(40f, 44f);
					break;

				case EnInteraction.TrueHeroQuest:
					imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_provoke");
					imageIcon.rectTransform.sizeDelta = new Vector2(52f, 56f);
					break;

                case EnInteraction.NpcDialog:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_speech");
                    imageIcon.rectTransform.sizeDelta = new Vector2(44f, 36f);
                    break;
            }
        }
        else
        {
            if (CheckEnterInteraction())
            {
                return;
            }
        }

		m_buttonInteraction.gameObject.SetActive(bEnter);
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonInteraction()
	{
		switch (m_enInteration)
		{
            case EnInteraction.MainQuest:
            case EnInteraction.GuildMission:
			case EnInteraction.SubQuest:
				OnClickMainQuest();
				break;

			case EnInteraction.SecretLetter:
				OnClickSecretLetter();
				break;

			case EnInteraction.MysteryBox:
				OnClickMysteryBox();
				break;

			case EnInteraction.DimensionRaid:
				OnClicDimensionRaid();
				break;

			case EnInteraction.GuildAltarSpellInjection:
				OnClickReStartSpellInjection();
				break;

			case EnInteraction.Dungeon:
				OnClickDungeonInteraction();
				break;

			case EnInteraction.TrueHeroQuest:
				OnClickTrueHeroInteraction();
				break;

            case EnInteraction.GuildFarm:
                OnClickGuildFarmInteraction();
                break;

            case EnInteraction.NpcDialog:
                OnClickNpcInteraction();
                break;

			default:
				break;
		}
	}

    //---------------------------------------------------------------------------------------------------
    bool CheckEnterInteraction()
    {
        foreach (EnInteraction item in Enum.GetValues(typeof(EnInteraction)))
        {
            if (m_dicEnterInteractionArea.ContainsKey(item.ToString()) && m_dicEnterInteractionArea[item.ToString()])
            {
                ChangeButtonInteractionState(item, true);
                return true;
            }
        }

        return false;
    }
}
