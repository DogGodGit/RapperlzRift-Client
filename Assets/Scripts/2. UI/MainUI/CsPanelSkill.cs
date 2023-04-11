using System.Collections;
using System.Collections.Generic;
using ClientCommon;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-21)
//---------------------------------------------------------------------------------------------------

public class CsPanelSkill : MonoBehaviour
{
    bool m_bStopUseNextChainSkill = false;
    bool m_bIsCheckChainSkillValidTimeCoroutine = false;
    bool m_bIsUseChainSkillProgressCoroutine = false;
    bool m_bStopUseChainSkillProgress = false;
    bool m_bStopUseOtherkill = false;
	bool m_bIsTransform = false; 

    float m_flRemainingAccelCoolTime = 0f;
    int m_nInteractionNpcId = 0;

    bool m_bAccelererateCheck = true;
    bool m_bCartHighSpeed = false;

	bool m_bIsClickedRankActiveSkill = false;

    Transform m_trCartGetOn;
    Transform m_trCartGetOff;
    Transform m_trCartAccelerate;
    Transform m_trPopup;
    Transform m_trSkillEffect;

    Image m_imageCartCoolTime;
    Text m_textCartCoolTime;

    Button m_buttonSkiil0;
    Button m_buttonInteraction;

    IMonsterObjectInfo m_iMonsterObjectInfo = null;
    CsMonsterInfo m_csMonsterInfoMainQuest = null;
    bool m_bMonsterTransformation = false;

    Dictionary<string, bool> m_dicEnterInteractionArea = new Dictionary<string, bool>();

    bool m_bInteraction = false;
    EnInteraction m_enInteraction = EnInteraction.None;

    bool m_bPressed = false;
    CsHeroSkill m_csHeroSkill = null;

	bool m_bSkillPanelVisible = true;
	List<bool> m_listSkillPanelVisible = new List<bool>();

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();
        CheckSkillOpen();
		
        CsGameEventUIToUI.Instance.EventHeroLogin += OnEventHeroLogin;

        CsGameEventToUI.Instance.EventConfirmUseSkill += OnEventConfirmUseSkill;
        CsGameEventToUI.Instance.EventUseAutoSkill += OnEventUseAutoSkill;
        CsGameEventToUI.Instance.EventConfirmUseCommonSkill += OnEventConfirmUseCommonSkill;
		CsGameEventToUI.Instance.EventConfirmUseRankActiveSkill += OnEventConfirmUseRankActiveSkill;

        CsGameEventUIToUI.Instance.EventLakAcquisition += OnEventLakAcquisition;

        CsCartManager.Instance.EventMyHeroCartGetOn += OnEventMyHeroCartGetOn;
        CsCartManager.Instance.EventMyHeroCartGetOff += OnEventMyHeroCartGetOff;
        CsCartManager.Instance.EventCartAccelerate += OnEventCartAccelerate;
        CsCartManager.Instance.EventMyCartHighSpeedEnd += OnEventMyCartHighSpeedEnd;

        CsMainQuestManager.Instance.EventAccepted += OnEventMainQuestAccepted;
        CsMainQuestManager.Instance.EventCompleted += OnEventMainQuestCompleted;
        CsGameEventToUI.Instance.EventHeroDead += OnEventHeroDead;                      // 영웅사망

        CsGuildManager.Instance.EventGuildSupplySupportQuestAccept += OnEventGuildSupplySupportQuestAccept;
        CsGuildManager.Instance.EventGuildSupplySupportQuestComplete += OnEventGuildSupplySupportQuestComplete;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail += OnEventGuildSupplySupportQuestFail;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept += OnEventSupplySupportQuestAccept;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail += OnEventSupplySupportQuestFail;

        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;

        CsRplzSession.Instance.EventEvtSkillCastResult += OnEventEvtSkillCastResult;

        // 몬스터 테이밍 버튼
        //CsDungeonManager.Instance.EventStoryDungeonTameable += OnEventStoryDungeonTameable;
        CsGameEventToUI.Instance.EventTameButton += OnEventTameButton;
        CsDungeonManager.Instance.EventStoryDungeonExit += OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;
        CsDungeonManager.Instance.EventStoryDungeonAbandon += OnEventStoryDungeonAbandon;

        // 정신력 감소 스킬 활성화
        CsGameEventToUI.Instance.EventSelectMonsterInfo += OnEventSelectMonsterInfo;
        CsGameEventToUI.Instance.EventSelectMonsterInfoStop += OnEventSelectMonsterInfoStop;

		// 계급액티브스킬 선택
		CsGameEventUIToUI.Instance.EventRankActiveSkillSelect += OnEventRankActiveSkillSelected;

		// 회수
		CsGameEventUIToUI.Instance.EventRetrieveGold += OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll += OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia += OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll += OnEventRetrieveDiaAll;

		// 유적 탈환
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationStart += OnEventRuinsReclaimMonsterTransformationStart;
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationFinished += OnEventRuinsReclaimMonsterTransformationFinished;
		CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		CsDungeonManager.Instance.EventRuinsReclaimWaveCompleted += OnEventRuinsReclaimWaveCompleted;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;
		CsDungeonManager.Instance.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;
		CsDungeonManager.Instance.EventRuinsReclaimExit += OnEventRuinsReclaimExit;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryTransformationObjectInteractionFinished += OnEventWarMemoryTransformationObjectInteractionFinished;
        CsDungeonManager.Instance.EventWarMemoryMonsterTransformationCancel += OnEventWarMemoryMonsterTransformationCancel;
        CsDungeonManager.Instance.EventWarMemoryMonsterTransformationFinished += OnEventWarMemoryMonsterTransformationFinished;
        CsDungeonManager.Instance.EventWarMemoryAbandon += OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished += OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryExit += OnEventWarMemoryExit;

        //상호작용 오브젝트
        CsContinentObjectManager.Instance.EventChangeInteractionState += OnEventChangeInteractionState;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionStarted += OnEventContinentObjectInteractionStarted;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionCancel += OnEventMyHeroContinentObjectInteractionCancel;
        CsContinentObjectManager.Instance.EventSendContinentObjectInteractionCancel += OnEventSendContinentObjectInteractionCancel;

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

        // 길드농장퀘스트
        CsGuildManager.Instance.EventFarmInteractionArea += OnEventFarmInteractionArea;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionStart += OnEventGuildFarmQuestInteractionStart;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCanceled += OnEventGuildFarmQuestInteractionCanceled;
        CsGuildManager.Instance.EventMyHeroGuildFarmQuestInteractionCancel += OnEventMyHeroGuildFarmQuestInteractionCancel;

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

        // 진정한 영웅
        CsTrueHeroQuestManager.Instance.EventChangeInteractionState += OnEventTrueHeroQuestChangeInteractionState;

        //NPC 대화
        CsGameEventToUI.Instance.EventNpcInteractionArea += OnEventNpcInteractionArea;

		// 던전
		CsDungeonManager.Instance.EventChangeDungeonInteractionState += OnEventChangeDungeonInteractionState;

        //
        CsGameEventUIToUI.Instance.EventHeroSkillReset += OnEventHeroSkillReset;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventHeroLogin -= OnEventHeroLogin;

        CsGameEventToUI.Instance.EventConfirmUseSkill -= OnEventConfirmUseSkill;
        CsGameEventToUI.Instance.EventUseAutoSkill -= OnEventUseAutoSkill;
        CsGameEventToUI.Instance.EventConfirmUseCommonSkill -= OnEventConfirmUseCommonSkill;
        CsGameEventToUI.Instance.EventConfirmUseRankActiveSkill -= OnEventConfirmUseRankActiveSkill;

        CsGameEventUIToUI.Instance.EventLakAcquisition -= OnEventLakAcquisition;

        CsCartManager.Instance.EventMyHeroCartGetOn -= OnEventMyHeroCartGetOn;
        CsCartManager.Instance.EventMyHeroCartGetOff -= OnEventMyHeroCartGetOff;
        CsCartManager.Instance.EventCartAccelerate -= OnEventCartAccelerate;
        CsCartManager.Instance.EventMyCartHighSpeedEnd -= OnEventMyCartHighSpeedEnd;

        CsMainQuestManager.Instance.EventAccepted -= OnEventMainQuestAccepted;
        CsMainQuestManager.Instance.EventCompleted -= OnEventMainQuestCompleted;
        CsGameEventToUI.Instance.EventHeroDead -= OnEventHeroDead;                      // 영웅사망

        CsGuildManager.Instance.EventGuildSupplySupportQuestAccept -= OnEventGuildSupplySupportQuestAccept;
        CsGuildManager.Instance.EventGuildSupplySupportQuestComplete -= OnEventGuildSupplySupportQuestComplete;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail -= OnEventGuildSupplySupportQuestFail;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept -= OnEventSupplySupportQuestAccept;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail -= OnEventSupplySupportQuestFail;

        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;

        CsRplzSession.Instance.EventEvtSkillCastResult -= OnEventEvtSkillCastResult;

        // 몬스터 테이밍 버튼
        //CsDungeonManager.Instance.EventStoryDungeonTameable -= OnEventStoryDungeonTameable;
        CsGameEventToUI.Instance.EventTameButton -= OnEventTameButton;
        CsDungeonManager.Instance.EventStoryDungeonExit -= OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;
        CsDungeonManager.Instance.EventStoryDungeonAbandon -= OnEventStoryDungeonAbandon;

        // 정신력 감소 스킬 활성화
        CsGameEventToUI.Instance.EventSelectMonsterInfo -= OnEventSelectMonsterInfo;
        CsGameEventToUI.Instance.EventSelectMonsterInfoStop -= OnEventSelectMonsterInfoStop;

        // 계급액티브스킬 선택
        CsGameEventUIToUI.Instance.EventRankActiveSkillSelect -= OnEventRankActiveSkillSelected;

        // 회수
        CsGameEventUIToUI.Instance.EventRetrieveGold -= OnEventRetrieveGold;
        CsGameEventUIToUI.Instance.EventRetrieveGoldAll -= OnEventRetrieveGoldAll;
        CsGameEventUIToUI.Instance.EventRetrieveDia -= OnEventRetrieveDia;
        CsGameEventUIToUI.Instance.EventRetrieveDiaAll -= OnEventRetrieveDiaAll;

        // 유적 탈환
        CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationStart -= OnEventRuinsReclaimMonsterTransformationStart;
        CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationFinished -= OnEventRuinsReclaimMonsterTransformationFinished;
        CsDungeonManager.Instance.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished -= OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
        CsDungeonManager.Instance.EventRuinsReclaimWaveCompleted -= OnEventRuinsReclaimWaveCompleted;
        CsDungeonManager.Instance.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;
        CsDungeonManager.Instance.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;
        CsDungeonManager.Instance.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryTransformationObjectInteractionFinished -= OnEventWarMemoryTransformationObjectInteractionFinished;
        CsDungeonManager.Instance.EventWarMemoryMonsterTransformationCancel -= OnEventWarMemoryMonsterTransformationCancel;
        CsDungeonManager.Instance.EventWarMemoryMonsterTransformationFinished -= OnEventWarMemoryMonsterTransformationFinished;
        CsDungeonManager.Instance.EventWarMemoryAbandon -= OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished -= OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryExit -= OnEventWarMemoryExit;

        //상호작용 오브젝트
        CsContinentObjectManager.Instance.EventChangeInteractionState -= OnEventChangeInteractionState;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionStarted -= OnEventContinentObjectInteractionStarted;
        CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionCancel -= OnEventMyHeroContinentObjectInteractionCancel;
        CsContinentObjectManager.Instance.EventSendContinentObjectInteractionCancel -= OnEventSendContinentObjectInteractionCancel;

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

        // 길드농장퀘스트
        CsGuildManager.Instance.EventFarmInteractionArea -= OnEventFarmInteractionArea;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionStart -= OnEventGuildFarmQuestInteractionStart;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCanceled -= OnEventGuildFarmQuestInteractionCanceled;
        CsGuildManager.Instance.EventMyHeroGuildFarmQuestInteractionCancel -= OnEventMyHeroGuildFarmQuestInteractionCancel;

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

        // 진정한 영웅
        CsTrueHeroQuestManager.Instance.EventChangeInteractionState -= OnEventTrueHeroQuestChangeInteractionState;

        //NPC 대화
        CsGameEventToUI.Instance.EventNpcInteractionArea -= OnEventNpcInteractionArea;

		// 던전
		CsDungeonManager.Instance.EventChangeDungeonInteractionState -= OnEventChangeDungeonInteractionState;

        //
        CsGameEventUIToUI.Instance.EventHeroSkillReset -= OnEventHeroSkillReset;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flRemainingAccelCoolTime - Time.realtimeSinceStartup > 0)
        {
            UpdateInteractionCart();
        }
        else if (!m_bAccelererateCheck)
        {
            ResetCartCoolTime();
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
	void OnEventSelectMonsterInfo(long lMonsterId, int nHpLineCount)
    {
        m_iMonsterObjectInfo = CsGameData.Instance.ListMonsterObjectInfo.Find(a => a.GetInstanceId() == lMonsterId);

        if (m_iMonsterObjectInfo == null)
        {
            return;
        }
        else
        {
            CsMonsterInfo csMonsterInfo = m_iMonsterObjectInfo.GetMonsterInfo();
            // 스토리 던전일 때
            if (csMonsterInfo.TamingEnabled)
            {
                DisplayCommonSkill(true);
            }
            else
            {
                DisplayCommonSkill(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectMonsterInfoStop()
    {
        m_iMonsterObjectInfo = null;
        DisplayCommonSkill(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonAbandon(int nPreviousContinentId)
    {
        UpdateSkillButtonMonsterMount(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nPreviousContinentId)
    {
        UpdateSkillButtonMonsterMount(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonExit(int nPreviousContinentId)
    {
        UpdateSkillButtonMonsterMount(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTameButton(bool bHide)
    {
        UpdateSkillButtonMonsterMount(bHide);
    }

    #region Interaction

    //---------------------------------------------------------------------------------------------------
    void DisplayInteractionButton(bool bIson, EnInteraction enInteraction)
    {
        if (m_dicEnterInteractionArea.ContainsKey(enInteraction.ToString()))
        {
            m_dicEnterInteractionArea[enInteraction.ToString()] = bIson;
        }
        else
        {
            m_dicEnterInteractionArea.Add(enInteraction.ToString(), bIson);
        }

        if (bIson)
        {
            m_buttonInteraction.onClick.RemoveAllListeners();
            m_buttonInteraction.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            m_buttonInteraction.onClick.AddListener(() => OnClickInteraction(enInteraction));
            m_buttonInteraction.gameObject.SetActive(true);
            if (m_trSkillEffect != null) m_trSkillEffect.gameObject.SetActive(true);

            Image imageIcon = m_buttonInteraction.transform.Find("ImageIcon").GetComponent<Image>();

            if (enInteraction == EnInteraction.NpcDialog || enInteraction == EnInteraction.SecretLetter)
            {
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_speak");
                imageIcon.rectTransform.sizeDelta = new Vector2(76, 60);
            }
			else if (enInteraction == EnInteraction.Dungeon)
			{
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_detail");
                imageIcon.rectTransform.sizeDelta = new Vector2(88, 88);
			}
			else if (enInteraction == EnInteraction.TrueHeroQuest)
			{
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_provoke");
				imageIcon.rectTransform.sizeDelta = new Vector2(88, 88);
			}
			else if (enInteraction == EnInteraction.DimensionRaid)
			{
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_fire");
				imageIcon.rectTransform.sizeDelta = new Vector2(76, 76);
			}
			else
			{
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_interaction_detail_2");
				imageIcon.rectTransform.sizeDelta = new Vector2(88, 88);
			}

            if (CsCartManager.Instance.IsMyHeroRidingCart)
            {
                m_trCartAccelerate.gameObject.SetActive(false);
                m_trCartGetOff.gameObject.SetActive(false);
            }
            else
            {
                m_bPressed = false;

                if (m_IEnumeratorPressed != null)
                {
                    StopCoroutine(m_IEnumeratorPressed);
                    m_IEnumeratorPressed = null;
                }
                
                m_buttonSkiil0.gameObject.SetActive(false);
            }
        }
        else
        {
            if (CheckEnterInteraction())
            {
                return;
            }

            m_buttonInteraction.gameObject.SetActive(false);
            if (m_trSkillEffect != null)
                m_trSkillEffect.gameObject.SetActive(false);

            if (CsCartManager.Instance.IsMyHeroRidingCart)
            {
                m_trCartAccelerate.gameObject.SetActive(true);
                m_trCartGetOff.gameObject.SetActive(true);
            }
            else
            {
                if (!m_bMonsterTransformation)
                {
                    m_buttonSkiil0.gameObject.SetActive(true);
                }
                else
                {
                    if (m_csMonsterInfoMainQuest.AttackEnabled)
                    {
                        m_buttonSkiil0.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_buttonSkiil0.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckEnterInteraction()
    {
        foreach (EnInteraction item in System.Enum.GetValues(typeof(EnInteraction)))
        {
            if (m_dicEnterInteractionArea.ContainsKey(item.ToString()) && m_dicEnterInteractionArea[item.ToString()])
            {
                DisplayInteractionButton(true, item);
                return true;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickInteraction(EnInteraction enInteraction)
    {
        if (enInteraction == EnInteraction.NpcDialog)
        {
            CsGameEventToIngame.Instance.OnEventRequestNpcDialog();
        }
        else
        {
            if (CsCartManager.Instance.IsMyHeroRidingCart)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_CART_INTERROR"));
            }
            else
            {
                switch (enInteraction)
                {
                    case EnInteraction.None:
                        break;

                    case EnInteraction.MainQuest:
                    case EnInteraction.GuildMission:
                    case EnInteraction.SubQuest:
                        CsContinentObjectManager.Instance.ContinentObjectInteractionStart();
                        break;

                    case EnInteraction.SecretLetter:
                        CsSecretLetterQuestManager.Instance.StartSecretLetterPickStart();
                        break;

                    case EnInteraction.MysteryBox:
                        CsMysteryBoxQuestManager.Instance.StartMysteryBoxPickStart();
                        break;

                    case EnInteraction.DimensionRaid:
                        CsDimensionRaidQuestManager.Instance.StartDimensionRaidInteractionStart();
                        break;

                    case EnInteraction.GuildAltarSpellInjection:
                        CsGuildManager.Instance.SendGuildAltarSpellInjectionMissionStart();
                        break;

                    case EnInteraction.Dungeon:
                        CsDungeonManager.Instance.DugeonObjectInteractionStart();
                        break;

                    case EnInteraction.TrueHeroQuest:
                        CsTrueHeroQuestManager.Instance.MyHeroTrueHeroQuestInteractionStart();
                        break;

                    case EnInteraction.GuildFarm:
                        CsGuildManager.Instance.FarmInteraction();
                        break;
                }
            }
        }
    }

    #endregion Interaction

    //---------------------------------------------------------------------------------------------------
    void UpdateSkillButtonMonsterMount(bool bHide)
    {
		m_bIsTransform = bHide;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
            Transform trSkill = transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);

            if (bHide)
            {
                if (i == 0)
                {
                    trSkill.Find("ImageIcon").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/ico_monster_attack");
                    Button buttonSkill = trSkill.GetComponent<Button>();
                    buttonSkill.onClick.RemoveAllListeners();
                    buttonSkill.onClick.AddListener(OnClickTameMonsterSkill);
                }
                else
                {
                    trSkill.gameObject.SetActive(false);
                }
            }
            else
            {
                trSkill.Find("ImageIcon").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.JobId + "_" + (i + 1));
                Button buttonSkill = trSkill.GetComponent<Button>();
                buttonSkill.onClick.RemoveAllListeners();
                buttonSkill.onClick.AddListener(() => OnClickSkill(csHeroSkill));

                if (!trSkill.gameObject.activeSelf)
                {
                    trSkill.gameObject.SetActive(true);
                }
                else
                {
                    continue;
                }
            }
        }

        for (int i = 0; i < CsGameData.Instance.JobCommonSkillList.Count; i++)
        {
            Transform trJobCommonSkill = transform.Find("ButtonCommonSkill" + CsGameData.Instance.JobCommonSkillList[i].SkillId);
            trJobCommonSkill.gameObject.SetActive(!bHide);
        }

		SwitchRankActiveSkill(!bHide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTameMonsterSkill()
    {
        CsGameEventToIngame.Instance.OnEventTameMonsterUseSkill();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtSkillCastResult(SEBSkillCastResultEventBody csEvt) // UI에서만 사용할 수 있음. 당사자.  // 라크스킬용.
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId == csEvt.heroId && csEvt.isSucceeded)
        {
            //Debug.Log("OnEventEvtSkillCastResult     csEvt.isSucceeded = " + csEvt.isSucceeded);
            CsGameData.Instance.MyHeroInfo.Lak = csEvt.lak;
            UpdateLak();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
    {
        DisplayMainQuestCart(true);

        if (nTransformationMonsterId == 0)
        {
            return;
        }
        else
        {
            m_csMonsterInfoMainQuest = CsGameData.Instance.GetMonsterInfo(nTransformationMonsterId);

            if (m_csMonsterInfoMainQuest == null)
            {
                return;
            }
            else
            {
                m_bMonsterTransformation = true;
                DisplaySkillPanelTransformationMonster(!m_bMonsterTransformation, m_csMonsterInfoMainQuest);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        CheckSkillOpen();
        CartButtonUpdate(false, false);

        if (m_csMonsterInfoMainQuest == null)
        {
            return;
        }
        else
        {
            m_bMonsterTransformation = false;
            DisplaySkillPanelTransformationMonster(!m_bMonsterTransformation);
            m_csMonsterInfoMainQuest = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroDead(string strName)
    {
        if (m_csMonsterInfoMainQuest == null)
        {
            return;
        }
        else
        {
            DisplaySkillPanelTransformationMonster(true);
            m_csMonsterInfoMainQuest = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestAccept(PDSupplySupportQuestCartInstance pDSupplySupportQuestCartInstance)
    {
        DisplaySupplySupportQuestCart(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp, long lGold, int nAcquiredExploitPoint)
    {
        CartButtonUpdate(false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestFail()
    {
        CartButtonUpdate(false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestFail()
    {
        CartButtonUpdate(false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestAccept(PDGuildSupplySupportQuestCartInstance pDGuildSupplySupportQuestCartInstance)
    {
        DisplayGuildSupplySupportQuestCart(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        CartButtonUpdate(false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bSceneLoad)
    {
        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
        {
            DisplayMainQuestCart();
            DisplaySupplySupportQuestCart();
            DisplayGuildSupplySupportQuestCart();
        }
        else
        {
            CartButtonUpdate(false, false);
        }

        ResetAllHeroSkill();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroLogin()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];

            Transform trSkill = transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);

            Button buttonSkill = trSkill.GetComponent<Button>();
            buttonSkill.onClick.RemoveAllListeners();
            buttonSkill.onClick.AddListener(() => OnClickSkill(csHeroSkill));

            if (csHeroSkill.JobSkill.SlotIndex == 0)
            {
                m_csHeroSkill = csHeroSkill;

                EventTrigger eventTrigger = buttonSkill.GetComponent<EventTrigger>();

                EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
                entryPointerDown.eventID = EventTriggerType.PointerDown;
                entryPointerDown.callback.AddListener((eventData) => OnPointerDown());
                eventTrigger.triggers.Add(entryPointerDown);

                EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
                entryPointerUp.eventID = EventTriggerType.PointerUp;
                entryPointerUp.callback.AddListener((eventData) => OnPointerUp());
                eventTrigger.triggers.Add(entryPointerUp);
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < CsGameData.Instance.JobCommonSkillList.Count; i++)
        {
            CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.JobCommonSkillList[i];

            Transform trSkill = transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);

            Button buttonSkill = trSkill.GetComponent<Button>();
            buttonSkill.onClick.RemoveAllListeners();
            buttonSkill.onClick.AddListener(() => OnClickSkill(csJobCommonSkill));
        }

        m_bMonsterTransformation = false;
        DisplaySkillPanelTransformationMonster(!m_bMonsterTransformation);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventConfirmUseSkill(bool bReturn, int nSkillId)
    {
        CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(nSkillId);

        if (bReturn)
        {
            if (csHeroSkill.FormType == EnFormType.Chain)   // 연계스킬일 경우.
            {
                if (m_bIsCheckChainSkillValidTimeCoroutine)
                {
                    m_bStopUseNextChainSkill = true;
                }

                // 마지막 연속기일 경우 처음으로 보낸다.
                if (csHeroSkill.JobSkill.JobChainSkillList.Count - 1 == csHeroSkill.ChainSkillSelectedIndex)
                {
                    csHeroSkill.Reset();
                }
                else
                {
                    csHeroSkill.ChainSkillSelectedIndex++;
                    csHeroSkill.StartUseSkill(true);
                    m_bIsUseChainSkillProgressCoroutine = true;
                    StartCoroutine(UseChainSkillProgressCoroutine(csHeroSkill));
                }
            }
            else
            {
                if (m_bIsUseChainSkillProgressCoroutine)
                    m_bStopUseChainSkillProgress = true;

                if (m_bIsCheckChainSkillValidTimeCoroutine)
                    m_bStopUseOtherkill = true;

                csHeroSkill.StartUseSkill();
                StartCoroutine(UseSkillProgressCoroutine(csHeroSkill));
            }
        }
        else
        {
            csHeroSkill.Reset();
            Transform trSkillslot = this.transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);
            trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = 0;
            Text textCooltime = trSkillslot.Find("TextCoolTime").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCooltime);
            textCooltime.text = "";
        }

        if (nSkillId == CsGameConfig.Instance.SpecialSkillId)
        {
            UpdateLak();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUseAutoSkill(int nSkillId)
    {
        CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(nSkillId);
        csHeroSkill.IsClicked = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventConfirmUseCommonSkill(bool bReturn, int nSkillId)
    {
        CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.GetJobCommonSkill(nSkillId);

        if (bReturn)
        {
            csJobCommonSkill.StartUseSkill();
            StartCoroutine(UseSkillProgressCoroutine(csJobCommonSkill));
        }
        else
        {
            csJobCommonSkill.Reset();
            Transform trSkillslot = this.transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);
            trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = 0;
            Text textCooltime = trSkillslot.Find("TextCoolTime").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCooltime);
            textCooltime.text = "";
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventConfirmUseRankActiveSkill(bool bReturn, int nSkillId)
	{
		CsRankActiveSkill csRankActiveSkill = CsGameData.Instance.GetRankActiveSkill(nSkillId);

		if (bReturn)
		{
			if (csRankActiveSkill != null)
			{
				CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime = csRankActiveSkill.CoolTime;
				StartCoroutine(UseSkillProgressCoroutine(csRankActiveSkill));
			}
		}
		else
		{
			ResetRankActiveSkill();
			Transform trSkillslot = this.transform.Find("ButtonRankActiveSkill");
			trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = 0;
			Text textCooltime = trSkillslot.Find("TextCoolTime").GetComponent<Text>();
			CsUIData.Instance.SetFont(textCooltime);
			textCooltime.text = "";
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventLakAcquisition()
    {
        // UpdateLak
        UpdateLak();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroCartGetOn(ClientCommon.PDCartInstance pDCartInstance)
    {
        DisplayMainQuestCart();
        DisplaySupplySupportQuestCart();
        DisplayGuildSupplySupportQuestCart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroCartGetOff()
    {
        DisplayMainQuestCart();
        DisplaySupplySupportQuestCart();
        DisplayGuildSupplySupportQuestCart();
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventCartAccelerate(bool bSuccess, float flRemainingAccelCoolTime)
    {
        m_bCartHighSpeed = bSuccess;
        m_bAccelererateCheck = false;
        m_flRemainingAccelCoolTime = flRemainingAccelCoolTime + Time.realtimeSinceStartup;
        m_trCartAccelerate.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
        m_trCartAccelerate.Find("ImageCart").GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);

        if (bSuccess)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A12_TXT_02003"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A12_TXT_02004"));
        }
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventMyCartHighSpeedEnd()
    {
        if (m_trCartAccelerate.gameObject.activeSelf)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A12_TXT_02002"));
        }

        m_bCartHighSpeed = false;
    }

	//----------------------------------------------------------------------------------------------------
	void OnEventRankActiveSkillSelected()
	{
		SetRankActiveSkill();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRetrieveGold(bool bLevelUp, long lExpAcq)
	{
		if (bLevelUp)
		{
			CheckSkillOpen();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGoldAll(bool bLevelUp, long lExpAcq)
	{
		if (bLevelUp)
		{
			CheckSkillOpen();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDia(bool bLevelUp, long lExpAcq)
	{
		if (bLevelUp)
		{
			CheckSkillOpen();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDiaAll(bool bLevelUp, long lExpAcq)
	{
		if (bLevelUp)
		{
			CheckSkillOpen();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationStart(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		m_bIsTransform = true;
		DisplaySkillPanelRuinsReclaim(false);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationFinished()
	{
		m_bIsTransform = false;
		DisplaySkillPanelRuinsReclaim(true);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(long lInstanceId)
	{
		m_bIsTransform = false;
		DisplaySkillPanelRuinsReclaim(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimWaveCompleted()
	{
		m_bIsTransform = false;
		DisplaySkillPanelRuinsReclaim(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nContinentId)
	{
		m_bIsTransform = false;
		DisplaySkillPanelRuinsReclaim(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nContinentId)
	{
		m_bIsTransform = false;
		DisplaySkillPanelRuinsReclaim(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nContinentId)
	{
		m_bIsTransform = false;
		DisplaySkillPanelRuinsReclaim(true);
	}

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryTransformationObjectInteractionFinished(int nObjectId, long lInstanceId, long[] arrRemovedAbnormalStateEffects)
    {
        m_bIsTransform = true;

        if (CsDungeonManager.Instance.WarMemory != null)
        {
            CsWarMemoryTransformationObject csWarMemoryTransformationObject = CsDungeonManager.Instance.WarMemoryWave.GetTransformationObject(nObjectId);

            if (csWarMemoryTransformationObject != null)
            {
                DisplaySkillPanelTransformationMonster(false, csWarMemoryTransformationObject.TransformationMonster);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMonsterTransformationCancel(long[] arrRemovedAbnormalStateEffects)
    {
        m_bIsTransform = false;
        DisplaySkillPanelTransformationMonster(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMonsterTransformationFinished(long[] arrRemovedAbnormalStateEffects)
    {
        m_bIsTransform = false;
        DisplaySkillPanelTransformationMonster(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryAbandon(int nContinentId)
    {
        m_bIsTransform = false;
        DisplaySkillPanelTransformationMonster(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryBanished(int nContinentId)
    {
        m_bIsTransform = false;
        DisplaySkillPanelTransformationMonster(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryExit(int nContinentId)
    {
        m_bIsTransform = false;
        DisplaySkillPanelTransformationMonster(true);
    }

    #endregion WarMemory

    #region Interaction

    //---------------------------------------------------------------------------------------------------
    void OnEventChangeInteractionState(EnInteractionState enInteractionState)
    {
        bool bEnter = (enInteractionState == EnInteractionState.ViewButton) ? true : false;

        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.MainQuest;

        DisplayInteractionButton(bEnter, EnInteraction.MainQuest);
    }
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentObjectInteractionStarted(long lInstanceId, EnInteractionQuestType enInteractionQuestTypeValue, CsContinentObject csContinentObject)
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.MainQuest;

        DisplayInteractionButton(false, EnInteraction.MainQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroContinentObjectInteractionCancel()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.MainQuest;

        DisplayInteractionButton(true, EnInteraction.MainQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSendContinentObjectInteractionCancel()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.MainQuest;

        DisplayInteractionButton(true, EnInteraction.MainQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterInteractionArea(bool bEnter)
    {
        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.SecretLetter;

        DisplayInteractionButton(bEnter, EnInteraction.SecretLetter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickStart()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.SecretLetter;

        DisplayInteractionButton(false, EnInteraction.SecretLetter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickCompleted()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed)
        {
            m_bInteraction = false;
            m_enInteraction = EnInteraction.SecretLetter;

            DisplayInteractionButton(false, EnInteraction.SecretLetter);
        }
        else
        {
            m_bInteraction = true;
            m_enInteraction = EnInteraction.SecretLetter;

            DisplayInteractionButton(true, EnInteraction.SecretLetter);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroSecretLetterPickCanceled()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.Completed && CsSecretLetterQuestManager.Instance.InteractionButton)
        {
            m_bInteraction = true;
            m_enInteraction = EnInteraction.SecretLetter;

            DisplayInteractionButton(true, EnInteraction.SecretLetter);
        }
        else
        {
            m_bInteraction = false;
            m_enInteraction = EnInteraction.SecretLetter;

            DisplayInteractionButton(false, EnInteraction.SecretLetter);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickCanceled()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.Completed && CsSecretLetterQuestManager.Instance.InteractionButton)
        {
            m_bInteraction = true;
            m_enInteraction = EnInteraction.SecretLetter;

            DisplayInteractionButton(true, EnInteraction.SecretLetter);
        }
        else
        {
            m_bInteraction = false;
            m_enInteraction = EnInteraction.SecretLetter;

            DisplayInteractionButton(false, EnInteraction.SecretLetter);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxInteractionArea(bool bEnter)
    {
        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.MysteryBox;

        DisplayInteractionButton(bEnter, EnInteraction.MysteryBox);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickStart()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.MysteryBox;

        DisplayInteractionButton(false, EnInteraction.MysteryBox);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickCompleted()
    {
        if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Accepted ||
            CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Executed &&
            CsGameData.Instance.MyHeroInfo.Nation.NationId != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
        {
            m_bInteraction = true;
            m_enInteraction = EnInteraction.MysteryBox;

            DisplayInteractionButton(true, EnInteraction.MysteryBox);
        }
        else
        {
            m_bInteraction = false;
            m_enInteraction = EnInteraction.MysteryBox;

            DisplayInteractionButton(false, EnInteraction.MysteryBox);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickCanceled()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.MysteryBox;

        DisplayInteractionButton(true, EnInteraction.MysteryBox);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroMysteryBoxPickCancel()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.MysteryBox;

        DisplayInteractionButton(true, EnInteraction.MysteryBox);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionArea(bool bEnter)
    {
        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.DimensionRaid;

        DisplayInteractionButton(bEnter, EnInteraction.DimensionRaid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionStart()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.DimensionRaid;

        DisplayInteractionButton(false, EnInteraction.DimensionRaid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroDimensionRaidInteractionCancel()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.DimensionRaid;

        DisplayInteractionButton(true, EnInteraction.DimensionRaid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionCanceled()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.DimensionRaid;

        DisplayInteractionButton(true, EnInteraction.DimensionRaid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFarmInteractionArea(bool bEnter)
    {
        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.GuildFarm;

        DisplayInteractionButton(bEnter, EnInteraction.GuildFarm);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionStart()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.GuildFarm;

        DisplayInteractionButton(false, EnInteraction.GuildFarm);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionCanceled()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.GuildFarm;

        DisplayInteractionButton(true, EnInteraction.GuildFarm);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroGuildFarmQuestInteractionCancel()
    {
        m_bInteraction = true;
        m_enInteraction = EnInteraction.GuildFarm;

        DisplayInteractionButton(true, EnInteraction.GuildFarm);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionStart()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.GuildAltarSpellInjection;
        //버튼끄기
        DisplayInteractionButton(false, EnInteraction.GuildAltarSpellInjection);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCompleted()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.GuildAltarSpellInjection;

        DisplayInteractionButton(false, EnInteraction.GuildAltarSpellInjection);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCanceled()
    {
        //상태체크해서 내가 아직 안쪽에 있으면 버튼켜기
        if (CsGuildManager.Instance.AltarEnter)
        {
            m_bInteraction = true;
            m_enInteraction = EnInteraction.GuildAltarSpellInjection;

            DisplayInteractionButton(true, EnInteraction.GuildAltarSpellInjection);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroGuildAltarSpellInjectionMissionCancel()
    {
        //상태체크해서 내가 아직 안쪽에 있으면 버튼켜기
        if (CsGuildManager.Instance.AltarEnter)
        {
            m_bInteraction = true;
            m_enInteraction = EnInteraction.GuildAltarSpellInjection;

            DisplayInteractionButton(true, EnInteraction.GuildAltarSpellInjection);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarGoOut(bool bAltar)
    {
        if (!bAltar)
        {
            m_bInteraction = false;
            m_enInteraction = EnInteraction.GuildAltarSpellInjection;

            DisplayInteractionButton(false, EnInteraction.GuildAltarSpellInjection);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDefenseMissionStart()
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.GuildAltarSpellInjection;

        DisplayInteractionButton(false, EnInteraction.GuildAltarSpellInjection);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDonate()
    {
        if (CsGuildManager.Instance.GuildMoralPoint >= CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint)
        {
            m_bInteraction = false;
            m_enInteraction = EnInteraction.GuildAltarSpellInjection;

            DisplayInteractionButton(false, EnInteraction.GuildAltarSpellInjection);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSpiritArea(bool bEnter)
    {
        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.GuildMission;

        DisplayInteractionButton(bEnter, EnInteraction.GuildMission);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionComplete(bool bLevelUp, long lAcquredExp)
    {
        m_bInteraction = false;
        m_enInteraction = EnInteraction.GuildMission;

        DisplayInteractionButton(false, EnInteraction.GuildMission);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTrueHeroQuestChangeInteractionState(EnInteractionState enInteractionState)
    {
        bool bEnter = enInteractionState == EnInteractionState.ViewButton;

        m_bInteraction = bEnter;
        m_enInteraction = EnInteraction.TrueHeroQuest;

        DisplayInteractionButton(bEnter, EnInteraction.TrueHeroQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNpcInteractionArea(bool bIson, int nNpcId)
    {
        m_bInteraction = bIson;
        m_enInteraction = EnInteraction.NpcDialog;

        if (bIson)
        {
            m_nInteractionNpcId = nNpcId;

            DisplayInteractionButton(bIson, EnInteraction.NpcDialog);
        }
        else
        {
            if (m_nInteractionNpcId == nNpcId)
            {
                DisplayInteractionButton(bIson, EnInteraction.NpcDialog);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventChangeDungeonInteractionState(EnInteractionState enInteractionState)
	{
		m_bInteraction = enInteractionState == EnInteractionState.ViewButton;
		m_enInteraction = EnInteraction.Dungeon;

		DisplayInteractionButton(enInteractionState == EnInteractionState.ViewButton, EnInteraction.Dungeon);
	}

    #endregion Interaction

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroSkillReset()
    {
        ResetAllHeroSkill();
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickSkill(CsHeroSkill csHeroSkill)
    {
        if (csHeroSkill != null)
        {
            if (csHeroSkill.RemainCoolTime == 0)
            {
                if (csHeroSkill.IsClicked)
                {
                    if (csHeroSkill.FormType == EnFormType.Chain)
                    {
                        return;
                    }
                    else
                    {
                        csHeroSkill.IsClicked = false;
                    }
                }
                else
                {
                    if (CsGameConfig.Instance.SpecialSkillId == csHeroSkill.JobSkill.SkillId)
                    {
                        if (CsGameEventToIngame.Instance.OnEventUseSkill(csHeroSkill) && CsGameData.Instance.MyHeroInfo.Lak >= CsGameConfig.Instance.SpecialSkillMaxLak)
                        {
                            csHeroSkill.IsClicked = true;

                            Button buttonSkillSpecial = transform.Find("ButtonSkill1").GetComponent<Button>();
                            buttonSkillSpecial.interactable = false;
                        }
                    }
                    else
                    {
                        if (CsGameEventToIngame.Instance.OnEventUseSkill(csHeroSkill))
                        {
                            csHeroSkill.IsClicked = true;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSkill(CsJobCommonSkill csJobCommonSkill)
    {
        if (csJobCommonSkill != null)
        {
            if (csJobCommonSkill.RemainCoolTime == 0 && !csJobCommonSkill.IsClicked)
            {
                Transform trMonsterTameHUD = m_trPopup.Find("MonsterTame");

                if (csJobCommonSkill.SkillId == 2 && trMonsterTameHUD != null)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A95_TXT_02001"));
                }
                else if(csJobCommonSkill.SkillId == 1 && CsUIData.Instance.DungeonInNow == EnDungeon.FieldOfHonor)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A31_TXT_02005"));
                }
                else
                {
                    if (CsGameEventToIngame.Instance.OnEventUseCommonSkill(csJobCommonSkill))
                    {
                        csJobCommonSkill.IsClicked = true;
                    }
                }
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickRankActiveSkill(CsRankActiveSkill csRankActiveSkill)
	{
		if (csRankActiveSkill != null)
		{
			if (CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime <= 0 && !m_bIsClickedRankActiveSkill)
			{
				if (CsGameEventToIngame.Instance.OnEventUseRankActiveSkill(csRankActiveSkill))
				{
					m_bIsClickedRankActiveSkill = true;
				}
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickCartAccelerate()
    {
        if (m_bAccelererateCheck && CsGameData.Instance.MyHeroInfo.CartInstance != null)
        {
            CsCartManager.Instance.SendCartAccelerate();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCartGetOn()
    {
        ICartObjectInfo cartObjectInfo = CsGameData.Instance.ListCartObjectInfo.Find(a => a.GetCartObject().OwnerId == CsGameData.Instance.MyHeroInfo.HeroId);

        if (cartObjectInfo != null)
        {
            CsCartManager.Instance.SendCartGetOn(cartObjectInfo.GetInstanceId());
        }
        else
        {
            CsCartManager.Instance.SendCartGetOn(CsMainQuestManager.Instance.CartInstanceId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCartGetOff()
    {
        if (CsGameData.Instance.MyHeroInfo.CartInstance != null)
        {
            CsCartManager.Instance.SendCartGetOff();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNpcDialog()
    {
        CsGameEventToIngame.Instance.OnEventRequestNpcDialog();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];

            Transform trSkill = transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);

            Button buttonSkill = trSkill.GetComponent<Button>();
            buttonSkill.onClick.RemoveAllListeners();
            buttonSkill.onClick.AddListener(() => OnClickSkill(csHeroSkill));

            Image imageIcon = trSkill.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.JobId + "_" + csHeroSkill.JobSkill.SkillId);

            Transform trLock = trSkill.Find("ImageLock");
            trLock.gameObject.SetActive(false);

            Image ImageCoolTime = trSkill.Find("ImageCoolTime").GetComponent<Image>();
            ImageCoolTime.gameObject.SetActive(true);
            ImageCoolTime.fillAmount = 0;

            Text textCoolTime = trSkill.Find("TextCoolTime").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCoolTime);
            textCoolTime.text = "";

            if (csHeroSkill.JobSkill.SlotIndex == 0)
            {
                m_csHeroSkill = csHeroSkill;

                EventTrigger eventTrigger = buttonSkill.GetComponent<EventTrigger>();

                EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
                entryPointerDown.eventID = EventTriggerType.PointerDown;
                entryPointerDown.callback.AddListener((eventData) => OnPointerDown());
                eventTrigger.triggers.Add(entryPointerDown);

                EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
                entryPointerUp.eventID = EventTriggerType.PointerUp;
                entryPointerUp.callback.AddListener((eventData) => OnPointerUp());
                eventTrigger.triggers.Add(entryPointerUp);
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < CsGameData.Instance.JobCommonSkillList.Count; i++)
        {
            CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.JobCommonSkillList[i];

            Transform trSkill = transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);

            Button buttonSkill = trSkill.GetComponent<Button>();
            buttonSkill.onClick.RemoveAllListeners();
            buttonSkill.onClick.AddListener(() => OnClickSkill(csJobCommonSkill));

            Transform trLock = trSkill.Find("ImageLock");
            trLock.gameObject.SetActive(false);

            Image ImageCoolTime = trSkill.Find("ImageCoolTime").GetComponent<Image>();
            ImageCoolTime.gameObject.SetActive(true);
            ImageCoolTime.fillAmount = 0;

            Text textCoolTime = trSkill.Find("TextCoolTime").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCoolTime);
            textCoolTime.text = "";
        }

		// 계급 액티브 스킬
		SetRankActiveSkill();
		
        //카트
        m_trCartAccelerate = transform.Find("ButtonCartAccelerate");
        Button buttonCartAccelerate = m_trCartAccelerate.GetComponent<Button>();
        buttonCartAccelerate.onClick.RemoveAllListeners();
        buttonCartAccelerate.onClick.AddListener(OnClickCartAccelerate);
        //buttonCartAccelerate.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_imageCartCoolTime = m_trCartAccelerate.Find("ImageCoolTime").GetComponent<Image>();
        m_textCartCoolTime = m_trCartAccelerate.Find("TextCoolTime").GetComponent<Text>();

        m_trCartGetOn = transform.Find("ButtonCartGetOn");
        Button buttonCartGetIn = m_trCartGetOn.GetComponent<Button>();
        buttonCartGetIn.onClick.RemoveAllListeners();
        buttonCartGetIn.onClick.AddListener(OnClickCartGetOn);
        //buttonCartGetIn.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trCartGetOff = transform.Find("ButtonCartGetOff");
        Button buttonCartGetOff = m_trCartGetOff.GetComponent<Button>();
        buttonCartGetOff.onClick.RemoveAllListeners();
        buttonCartGetOff.onClick.AddListener(OnClickCartGetOff);
        //buttonCartGetOff.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //NPC 대화
        m_buttonSkiil0 = transform.Find("ButtonSkill0").GetComponent<Button>();
        m_buttonInteraction = transform.Find("ButtonNpcInteraction").GetComponent<Button>();

        UpdateLak();

        m_trPopup = GameObject.Find("Canvas").transform.Find("Popup");
        DisplayCommonSkill(false);

        m_trSkillEffect = transform.Find("NPCTalkEffect");
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator UseChainSkillProgressCoroutine(CsHeroSkill csHeroSkill)
    {
        while (csHeroSkill.RemainCoolTime > 0)
        {
            yield return null;

            csHeroSkill.RemainCoolTime -= Time.deltaTime;

            // 연속기가 아닌 다른 스킬을 사용하였을 경우.
            if (m_bStopUseChainSkillProgress)
                break;
        }

        if (m_bStopUseChainSkillProgress)
        {
            csHeroSkill.Reset();
            DisplaySkillCoolTimeProgress(csHeroSkill);
            m_bIsUseChainSkillProgressCoroutine = false;
        }
        else
        {
            csHeroSkill.RemainCoolTime = 0;
            csHeroSkill.IsClicked = false;
            m_bIsUseChainSkillProgressCoroutine = false;
            m_bIsCheckChainSkillValidTimeCoroutine = true;
            //StartCoroutine(CheckChainSkillValidTimeCoroutine(csHeroSkill));
        }

        m_bStopUseChainSkillProgress = false;
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySkillCoolTimeProgress(CsHeroSkill csHeroSkill)
    {
        Transform trSkillslot = this.transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);

        if (csHeroSkill.FormType == EnFormType.Chain)
        {
            trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = csHeroSkill.ChainSkillCoolTimeProgress;
        }
        else
        {
            trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = csHeroSkill.CoolTimeProgress;
        }

        Text textCoolTime = trSkillslot.Find("TextCoolTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCoolTime);
        if (csHeroSkill.RemainCoolTime == 0)
        {
            textCoolTime.text = "";
        }
        else
        {
            textCoolTime.text = Mathf.CeilToInt(csHeroSkill.RemainCoolTime).ToString("00");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySkillCoolTimeProgress(CsJobCommonSkill csJobCommonSkill)
    {
        Transform trSkillslot = this.transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);

        trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = csJobCommonSkill.CoolTimeProgress;

        Text textCoolTime = trSkillslot.Find("TextCoolTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCoolTime);

        if (csJobCommonSkill.RemainCoolTime == 0)
        {
            textCoolTime.text = "";
        }
        else
        {
            textCoolTime.text = Mathf.CeilToInt(csJobCommonSkill.RemainCoolTime).ToString("00");
        }
    }

	//---------------------------------------------------------------------------------------------------
	void DisplaySkillCoolTimeProgress(CsRankActiveSkill csRankActiveSkill)
	{
		float flRemainCoolTime = CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime;

		Transform trSkillslot = this.transform.Find("ButtonRankActiveSkill");

		trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = flRemainCoolTime / csRankActiveSkill.CoolTime;

		Text textCoolTime = trSkillslot.Find("TextCoolTime").GetComponent<Text>();
		CsUIData.Instance.SetFont(textCoolTime);

		if (flRemainCoolTime <= 0)
		{
			textCoolTime.text = "";
		}
		else
		{
			textCoolTime.text = Mathf.CeilToInt(flRemainCoolTime).ToString("00");
		}
	}

    //---------------------------------------------------------------------------------------------------
    IEnumerator CheckChainSkillValidTimeCoroutine(CsHeroSkill csHeroSkill)
    {
        Transform trSkillslot = this.transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);
        float fValideTime = csHeroSkill.CurrentJobChainSkill.CastConditionEndTime - csHeroSkill.CurrentJobChainSkill.CastConditionStartTime;
        float fValideTotalTime = fValideTime;

        trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = 1;
        while (fValideTime > 0)
        {
            yield return null;

            fValideTime -= Time.deltaTime;
            trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = fValideTime / fValideTotalTime;
            // 다음 연속기를 사용했거나 다른 스킬을 사용하였을 경우.
            if (m_bStopUseNextChainSkill || m_bStopUseOtherkill)
                break;
        }
        // 타임아웃은 되었으나 타임아웃 이전에 연속기 클릭에 대한 응답이 오지 않으면 기다린다.
        yield return new WaitUntil(() => csHeroSkill.IsClicked == false);

        if (!m_bStopUseNextChainSkill || m_bStopUseOtherkill)
        {
            // 타임아웃 또는 다른 스킬을 사용했을 경우.
            csHeroSkill.Reset();
        }

        trSkillslot.Find("ImageCoolTime").GetComponent<Image>().fillAmount = 0;

        m_bStopUseNextChainSkill = false;
        m_bStopUseOtherkill = false;
        m_bIsCheckChainSkillValidTimeCoroutine = false;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator UseSkillProgressCoroutine(CsHeroSkill csHeroSkill)
    {
        DisplaySkillCoolTimeProgress(csHeroSkill);

        while (csHeroSkill.RemainCoolTime > 0)
        {
            yield return null;
            csHeroSkill.RemainCoolTime -= Time.deltaTime;
            DisplaySkillCoolTimeProgress(csHeroSkill);
        }

        csHeroSkill.Reset();
        DisplaySkillCoolTimeProgress(csHeroSkill);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator UseSkillProgressCoroutine(CsJobCommonSkill csJobCommonSkill)
    {
        DisplaySkillCoolTimeProgress(csJobCommonSkill);

        while (csJobCommonSkill.RemainCoolTime > 0)
        {
            yield return null;
            csJobCommonSkill.RemainCoolTime -= Time.deltaTime;
            DisplaySkillCoolTimeProgress(csJobCommonSkill);
        }

        csJobCommonSkill.Reset();
        DisplaySkillCoolTimeProgress(csJobCommonSkill);
    }

	//---------------------------------------------------------------------------------------------------
	IEnumerator UseSkillProgressCoroutine(CsRankActiveSkill csRankActiveSkill)
	{
		DisplaySkillCoolTimeProgress(csRankActiveSkill);

		while (CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime > 0)
		{
			yield return null;
			DisplaySkillCoolTimeProgress(csRankActiveSkill);
		}

		ResetRankActiveSkill();
		DisplaySkillCoolTimeProgress(csRankActiveSkill);
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateLak()
    {
        Button buttonSkillSpecial = transform.Find("ButtonSkill1").GetComponent<Button>();

        Image imageLak = buttonSkillSpecial.transform.Find("ImageLak").GetComponent<Image>();
        imageLak.fillAmount = (float)CsGameData.Instance.MyHeroInfo.Lak / (float)CsGameConfig.Instance.SpecialSkillMaxLak;

        Image imageCoolTime = buttonSkillSpecial.transform.Find("ImageCoolTime").GetComponent<Image>();

        if (CsGameData.Instance.MyHeroInfo.Lak < CsGameConfig.Instance.SpecialSkillMaxLak)
        {
            imageCoolTime.fillAmount = 1;
            buttonSkillSpecial.interactable = false;
        }
        else
        {
            imageCoolTime.fillAmount = 0;
            buttonSkillSpecial.interactable = true;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckSkillOpen()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];

            Transform trSkill = transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);

            Button buttonSkill = trSkill.GetComponent<Button>();
            Transform trLock = trSkill.Find("ImageLock");
            Transform trCoolTime = trSkill.Find("ImageCoolTime");

            if (CsMainQuestManager.Instance.MainQuest == null)
            {
                //해제
                trLock.gameObject.SetActive(false);
                buttonSkill.interactable = true;
                trCoolTime.gameObject.SetActive(true);
            }
            else
            {
                //Debug.Log("OpenRequiredMainQuestNo : " + csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo);
                //Debug.Log("MainQuestNo : " + CsMainQuestManager.Instance.MainQuest.MainQuestNo);

                if (CsMainQuestManager.Instance.MainQuest != null && csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo >= CsMainQuestManager.Instance.MainQuest.MainQuestNo)
                {
                    //잠금
                    trLock.gameObject.SetActive(true);
                    buttonSkill.interactable = false;
                    trCoolTime.gameObject.SetActive(false);
                    
                    if (i == 0)
                    {
                        buttonSkill.transform.GetComponent<EventTrigger>().enabled = false;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    //해제
                    trLock.gameObject.SetActive(false);
                    buttonSkill.interactable = true;
                    trCoolTime.gameObject.SetActive(true);

                    if (i == 0)
                    {
                        buttonSkill.transform.GetComponent<EventTrigger>().enabled = true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        for (int i = 0; i < CsGameData.Instance.JobCommonSkillList.Count; i++)
        {
            CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.JobCommonSkillList[i];

            Transform trSkill = transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);

            Button buttonSkill = trSkill.GetComponent<Button>();
            Transform trLock = trSkill.Find("ImageLock");
            Transform trCoolTime = trSkill.Find("ImageCoolTime");

            if (CsMainQuestManager.Instance.MainQuest == null)
            {
                //해제
                trLock.gameObject.SetActive(false);
                buttonSkill.interactable = true;
                trCoolTime.gameObject.SetActive(true);
            }
            else
            {
                if ((CsMainQuestManager.Instance.MainQuest != null) && csJobCommonSkill.OpenRequiredMainQuestNo >= CsMainQuestManager.Instance.MainQuest.MainQuestNo)
                {
                    //잠금
                    trLock.gameObject.SetActive(true);
                    buttonSkill.interactable = false;
                    trCoolTime.gameObject.SetActive(false);
                }
                else
                {
                    //해제
                    trLock.gameObject.SetActive(false);
                    buttonSkill.interactable = true;
                    trCoolTime.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMainQuestCart(bool bQuestAccepte = false)
    {
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;
        bool bCartCheck = false;
        bool bRiding = false;

		if (csMainQuest != null &&
			CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.Completed)
        {
            //메인퀘스트 카트
            if (csMainQuest.MainQuestType == EnMainQuestType.Cart && CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.None)
            {
                bCartCheck = true;

                if (bQuestAccepte)
                {
                    bRiding = true;
                }
                else
                {
                    bRiding = CsCartManager.Instance.IsMyHeroRidingCart;
                }

                CartButtonUpdate(bCartCheck, bRiding);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySupplySupportQuestCart(bool bQuestAccepte = false)
    {
        EnSupplySupportState enSupplySupportState = CsSupplySupportQuestManager.Instance.QuestState;
        bool bCartCheck = false;
        bool bRiding = false;

        if (enSupplySupportState != EnSupplySupportState.None)
        {
            bCartCheck = true;

            if (bQuestAccepte)
            {
                bRiding = true;
            }
            else
            {
                bRiding = CsCartManager.Instance.IsMyHeroRidingCart;

            }

            CartButtonUpdate(bCartCheck, bRiding);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayGuildSupplySupportQuestCart(bool bQuestAccepte = false)
    {
        EnGuildSupplySupportState enGuildSupplySupportState = CsGuildManager.Instance.GuildSupplySupportState;
        bool bCartCheck = false;
        bool bRiding = false;

        if (enGuildSupplySupportState != EnGuildSupplySupportState.None)
        {
            bCartCheck = true;

            if (bQuestAccepte)
            {
                bRiding = true;
            }
            else
            {
                bRiding = CsCartManager.Instance.IsMyHeroRidingCart;

            }

            CartButtonUpdate(bCartCheck, bRiding);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CartButtonUpdate(bool bCartCheck, bool bRiding)
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
            Transform trSkill = transform.Find("ButtonSkill" + csHeroSkill.JobSkill.SlotIndex);
            trSkill.gameObject.SetActive(!bRiding);
        }

        for (int i = 0; i < CsGameData.Instance.JobCommonSkillList.Count; i++)
        {
            CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.JobCommonSkillList[i];
            Transform trSkill = transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);
            trSkill.gameObject.SetActive(!bRiding);
        }

		SwitchRankActiveSkill(!bRiding);

        if (bCartCheck)
        {
            m_trCartGetOn.gameObject.SetActive(!bRiding);
            m_trCartGetOff.gameObject.SetActive(bRiding);
            m_trCartAccelerate.gameObject.SetActive(bRiding);
        }
        else
        {
            m_trCartGetOff.gameObject.SetActive(false);
            m_trCartGetOn.gameObject.SetActive(false);
            m_trCartAccelerate.gameObject.SetActive(false);
        }

        if (m_buttonInteraction.gameObject.activeSelf)
        {
            m_buttonSkiil0.gameObject.SetActive(false);
            m_trCartAccelerate.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractionCart()
    {
        //Transform trCart = transform.Find("ButtonSkill0");
        float flRemainingTime = m_flRemainingAccelCoolTime - Time.realtimeSinceStartup;
        m_imageCartCoolTime.fillAmount = flRemainingTime / CsGameConfig.Instance.CartAccelCoolTime;
        m_textCartCoolTime.text = flRemainingTime.ToString("0");
    }

    //---------------------------------------------------------------------------------------------------
    void ResetCartCoolTime()
    {
        if (m_trCartAccelerate.gameObject.activeSelf && !m_bCartHighSpeed)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A12_TXT_02002"));
        }

        m_bAccelererateCheck = true;
        m_flRemainingAccelCoolTime = 0;
        m_trCartAccelerate.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        m_trCartAccelerate.Find("ImageCart").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        m_imageCartCoolTime.fillAmount = 0;
        m_textCartCoolTime.text = "";
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayCommonSkill(bool bInteractable)
    {
        CsJobCommonSkill csJobCommonSkill = CsGameData.Instance.GetJobCommonSkill(2);
        Transform trSkill = transform.Find("ButtonCommonSkill" + csJobCommonSkill.SkillId);

        Button buttonSkill = trSkill.GetComponent<Button>();
        Transform trLock = trSkill.Find("ImageLock");
        Transform trCoolTime = trSkill.Find("ImageCoolTime");

        if (CsMainQuestManager.Instance.MainQuest == null)
        {
            //해제
            Button buttonCommonSkill = transform.Find("ButtonCommonSkill2").GetComponent<Button>();
            buttonCommonSkill.interactable = bInteractable;
        }
        else
        {
            if (CsMainQuestManager.Instance.MainQuest != null && csJobCommonSkill.OpenRequiredMainQuestNo >= CsMainQuestManager.Instance.MainQuest.MainQuestNo)
            {
                //잠금
                return;
            }
            else
            {
                //해제
                Button buttonCommonSkill = transform.Find("ButtonCommonSkill2").GetComponent<Button>();
                buttonCommonSkill.interactable = bInteractable;
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void SetRankActiveSkill()
	{
		CsRankActiveSkill csRankActiveSkill = CsGameData.Instance.GetRankActiveSkill(CsGameData.Instance.MyHeroInfo.SelectedRankActiveSkillId);
		Transform trRankActiveSkill = transform.Find("ButtonRankActiveSkill");

		if (csRankActiveSkill != null)
		{
			Button buttonRankActiveSkill = trRankActiveSkill.GetComponent<Button>();
			buttonRankActiveSkill.onClick.RemoveAllListeners();
			buttonRankActiveSkill.onClick.AddListener(() => OnClickRankActiveSkill(csRankActiveSkill));

			Image imageRankActiveSkillIcon = trRankActiveSkill.Find("ImageIcon").GetComponent<Image>();
			imageRankActiveSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/" + csRankActiveSkill.ImageName);

			Transform trRankActiveSkillLock = trRankActiveSkill.Find("ImageLock");
			trRankActiveSkillLock.gameObject.SetActive(false);

			Image ImageRankActiveSkillCoolTime = trRankActiveSkill.Find("ImageCoolTime").GetComponent<Image>();
			ImageRankActiveSkillCoolTime.gameObject.SetActive(true);
			
			Text textRankActiveSkillCoolTime = trRankActiveSkill.Find("TextCoolTime").GetComponent<Text>();
			CsUIData.Instance.SetFont(textRankActiveSkillCoolTime);

			if (CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime > 0)
			{
				StartCoroutine(UseSkillProgressCoroutine(csRankActiveSkill));
			}
			else
			{
				ImageRankActiveSkillCoolTime.fillAmount = 0;
				textRankActiveSkillCoolTime.text = "";
			}
			
		}

		SwitchRankActiveSkill(csRankActiveSkill != null);
	}

	//---------------------------------------------------------------------------------------------------
	void SwitchRankActiveSkill(bool bOn)
	{
		if (bOn)
		{
			CsRankActiveSkill csRankActiveSkill = CsGameData.Instance.GetRankActiveSkill(CsGameData.Instance.MyHeroInfo.SelectedRankActiveSkillId);
			transform.Find("ButtonRankActiveSkill").gameObject.SetActive(csRankActiveSkill != null && !m_bIsTransform);
		}
		else
		{
			transform.Find("ButtonRankActiveSkill").gameObject.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ResetRankActiveSkill()
	{
		m_bIsClickedRankActiveSkill = false;
	}

	//---------------------------------------------------------------------------------------------------
	void DisplaySkillPanelRuinsReclaim(bool bVisible)
	{
		if (m_bSkillPanelVisible != bVisible)
		{
			m_bSkillPanelVisible = bVisible;

			if (bVisible)
			{
				for (int i = 0; i < transform.childCount; i++)
				{
					transform.GetChild(i).gameObject.SetActive(m_listSkillPanelVisible[i]);
				}

				m_listSkillPanelVisible.Clear();
			}
			else
			{
				m_listSkillPanelVisible.Clear();

				for (int i = 0; i < transform.childCount; i++)
				{
					m_listSkillPanelVisible.Add(transform.GetChild(i).gameObject.activeSelf);
					transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void DisplaySkillPanelTransformationMonster(bool bVisible, CsMonsterInfo csMonsterInfo = null)
    {
        Button buttonSkill = null;
        Image imageButtonIcon = null;

        if (bVisible)
        {
            ResetAllHeroSkill();

            if (0 < m_listSkillPanelVisible.Count)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(m_listSkillPanelVisible[i]);

                    switch (transform.GetChild(i).name)
                    {
                        case "ButtonSkill0":
                            {
                                CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList.Find(a => a.JobSkill.SlotIndex == 0);

                                buttonSkill = transform.GetChild(i).GetComponent<Button>();
                                buttonSkill.onClick.RemoveAllListeners();
                                buttonSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                                buttonSkill.onClick.AddListener(() => OnClickSkill(csHeroSkill));

                                imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();
                                imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();

                                imageButtonIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.JobId + "_" + 1);

                                buttonSkill.gameObject.SetActive(true);
                            }
                            break;

                        case "ButtonSkill3":
                            {
                                CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList.Find(a => a.JobSkill.SlotIndex == 3);

                                buttonSkill = transform.GetChild(i).GetComponent<Button>();
                                buttonSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                                buttonSkill.onClick.AddListener(() => OnClickSkill(csHeroSkill));

                                imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();
                                imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();

                                imageButtonIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.JobId + "_" + 2);

                                buttonSkill.gameObject.SetActive(true);
                            }
                            break;
                    }
                }

                m_listSkillPanelVisible.Clear();
            }
            else
            {
                return;
            }
        }
        else
        {
            m_listSkillPanelVisible.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                m_listSkillPanelVisible.Add(transform.GetChild(i).gameObject.activeSelf);
                transform.GetChild(i).gameObject.SetActive(false);

                switch (transform.GetChild(i).name)
                {
                    case "ButtonSkill0":
                        {
                            buttonSkill = transform.GetChild(i).GetComponent<Button>();
                            imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();

                            if (csMonsterInfo == null)
                            {
                                return;
                            }
                            else
                            {
                                if (csMonsterInfo.MonsterOwnSkillList.Count > 0)
                                {
                                    buttonSkill.onClick.RemoveAllListeners();
                                    buttonSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                                    buttonSkill.onClick.AddListener(() => CsGameEventToIngame.Instance.OnEventUseTransformationMonsterSkillCast(CsGameData.Instance.GetMonsterSkill(csMonsterInfo.MonsterOwnSkillList[0].SkillId)));

                                    imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();
                                    imageButtonIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/ico_monster_attack");

                                    buttonSkill.gameObject.SetActive(true);
                                }
                                else
                                {
                                    buttonSkill.gameObject.SetActive(false);
                                }
                            }
                        }
                        break;

                    case "ButtonSkill3":
                        {
                            buttonSkill = transform.GetChild(i).GetComponent<Button>();
                            imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();

                            if (csMonsterInfo == null)
                            {
                                return;
                            }
                            else
                            {
                                if (csMonsterInfo.MonsterOwnSkillList.Count > 1)
                                {
                                    buttonSkill.onClick.RemoveAllListeners();
                                    buttonSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                                    buttonSkill.onClick.AddListener(() => CsGameEventToIngame.Instance.OnEventUseTransformationMonsterSkillCast(CsGameData.Instance.GetMonsterSkill(csMonsterInfo.MonsterOwnSkillList[1].SkillId)));

                                    imageButtonIcon = buttonSkill.transform.Find("ImageIcon").GetComponent<Image>();
                                    imageButtonIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/ico_skill_monster01");

                                    buttonSkill.gameObject.SetActive(true);
                                }
                                else
                                {
                                    buttonSkill.gameObject.SetActive(false);
                                }
                            }
                        }
                        break;
                }
            }
        }

        DisplayInteractionButton(m_bInteraction, m_enInteraction);
    }

    //---------------------------------------------------------------------------------------------------
    void OnPointerDown()
    {
        //Debug.Log("OnPointerDown");

        m_bPressed = true;
        if (m_trSkillEffect != null)
            m_trSkillEffect.gameObject.SetActive(true);

        if (m_IEnumeratorPressed != null)
        {
            StopCoroutine(m_IEnumeratorPressed);
            m_IEnumeratorPressed = null;
        }

        m_IEnumeratorPressed = SkillButtonPressed();
        StartCoroutine(m_IEnumeratorPressed);
    }

    //---------------------------------------------------------------------------------------------------
    void OnPointerUp()
    {
        //Debug.Log("OnPointerUp");

        m_bPressed = false;
        if (m_trSkillEffect != null)
            m_trSkillEffect.gameObject.SetActive(false);

        if (m_IEnumeratorPressed != null)
        {
            StopCoroutine(m_IEnumeratorPressed);
            m_IEnumeratorPressed = null;
        }
    }

    IEnumerator m_IEnumeratorPressed = null;
    
    //---------------------------------------------------------------------------------------------------
    IEnumerator SkillButtonPressed()
    {
        yield return new WaitForSeconds(0.5f);

        while (m_bPressed)
        {
            if (!m_bMonsterTransformation)
            {
                OnClickSkill(m_csHeroSkill);
            }
            else
            {
                CsGameEventToIngame.Instance.OnEventUseTransformationMonsterSkillCast(CsGameData.Instance.GetMonsterSkill(m_csMonsterInfoMainQuest.MonsterOwnSkillList[0].SkillId));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResetAllHeroSkill()
    {
        CsHeroSkill csHeroSkill = null;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
            csHeroSkill.IsClicked = false;
            csHeroSkill.ChainSkillSelectedIndex = 0;
        }
    }
}