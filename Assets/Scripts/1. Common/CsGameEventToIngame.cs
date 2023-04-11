using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ClientCommon;

public class CsGameEventToIngame
{
	public static CsGameEventToIngame Instance
	{
		get { return CsSingleton<CsGameEventToIngame>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<EnMainGearCategory, CsHeroMainGear> EventMainGearChanged;				// 메인장비변경시

	public event DelegateR<bool, CsHeroSkill>EventUseSkill;										// 영웅스킬사용요청
    public event DelegateR<bool, CsJobCommonSkill> EventUseCommonSkill;							// 공용스킬사용요청
	public event DelegateR<bool, CsRankActiveSkill> EventUseRankActiveSkill;					// 계급스킬사용요청
	public event DelegateR<bool, CsMonsterSkill> EventUseTransformationMonsterSkillCast;		// 변신몬스터스킬사용요청

	public event DelegateR<bool> EventIsStateIdle;												// 대기상태확인

	public event DelegateR<bool> EventReturnScrollUseStart;										// ReturnScroll 사용 시작
	public event Delegate EventReturnScrollUseCancel;											// ReturnScroll 사용 취소

	public event Delegate EventImmediateRevived;												// 영웅 즉시 부활

	public event Delegate EventHpPotionUse;														// 영웅 물약 사용

	public event Delegate<int, int, Vector3> EventPartyCalled;									// 파티소집
    public event DelegateR<bool, int, int, Vector3, int> EventMapMove;							// 맵 자동이동
    public event Delegate<int, CsNpcInfo> EventNpcAutoMove;										// NPC 자동이동

    public event Delegate<EnBattleMode> EventAutoBattleStart;									// 자동전투 시작
	public event Delegate<EnAutoStateType> EventAutoStop;										// 자동중단

	public event Delegate EventTab;																// Tab 사용 (타켓변경)

	public event Delegate EventMyHeroLevelUp;													// 레벨업

	public event Delegate<CsHeroMount> EventHeroMountEquipped;									// 탈것장착
	public event Delegate EventMountGetOn;														// 탈것탑승
	public event Delegate EventMountGetOff;														// 탈것내리기

	public event Delegate EventWingEquip;														// 날개장착
    public event Delegate<EnPlayerPrefsKey, int> EventPlayerPrefsKeySet;						// 인게임 옵션 조정

    public event Delegate EventLoadingUIComplete;												// 로딩 화면 종료
	public event Delegate EventStartTutorial;													// 튜토리얼 시작.

    public event Delegate EventDungeonEnter;													// 던전 진입	
    public event Delegate EventBossAppearSkip;													// 보스 등장 연출 스킵

	public event DelegateR<bool> EventTameMonsterUseSkill;										// 테이밍 몬스터 스킬사용요청
    public event Delegate EventQuestCompltedError;												// 퀘스트 완료에 대한 에러

	public event Delegate<EnCameraMode> EventChangeCameraState;									// 카메라 상태 변경.
    public event Delegate<bool> EventSleepMode;													// 절전 모드

	public event Delegate EventGroggyMonsterItemStealStart;										// 그로기몬스터아이템훔치기시작.

	public event Delegate EventRequestNpcDialog;                                                // Npc 대화 요청.
	public event DelegateR<bool> EventCheckQuestAreaInHero;                                     // 영웅 퀘스트범위 확인.
	public event Delegate<bool> EventOtherHeroView;											// 타영웅 보기 상태 변경.
	
	//----------------------------------------------------------------------------------------------------
	public void OnEventMainGearChanged(EnMainGearCategory enMainGeartype, CsHeroMainGear csHeroMainGear)
	{
		if (EventMainGearChanged != null)
		{
			EventMainGearChanged(enMainGeartype, csHeroMainGear);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public bool OnEventUseSkill(CsHeroSkill csHeroSkill)
	{
		if (EventUseSkill != null)
		{
			return EventUseSkill(csHeroSkill);
		}
		return false;
	}

    //----------------------------------------------------------------------------------------------------
    public bool OnEventUseCommonSkill(CsJobCommonSkill csJobCommonSkill)
    {
        if(EventUseCommonSkill != null)
        {
            return EventUseCommonSkill(csJobCommonSkill);
        }
        return false;
    }

	//----------------------------------------------------------------------------------------------------
	public bool OnEventUseRankActiveSkill(CsRankActiveSkill csRankActiveSkill)
	{
		if (EventUseRankActiveSkill != null)
		{
			return EventUseRankActiveSkill(csRankActiveSkill);
		}
		return false;
	}

	//----------------------------------------------------------------------------------------------------
	public bool OnEventUseTransformationMonsterSkillCast(CsMonsterSkill csMonsterSkill)
	{
        if (EventUseTransformationMonsterSkillCast != null)
		{
			return EventUseTransformationMonsterSkillCast(csMonsterSkill);
		}

		return false;
	}

	//----------------------------------------------------------------------------------------------------
	public bool OnEventIsStateIdle()
	{
		if (EventIsStateIdle != null)
		{
			return EventIsStateIdle();
		}
		return false;
	}

	//----------------------------------------------------------------------------------------------------
	public bool OnEventReturnScrollUseStart()
	{
		if (EventReturnScrollUseStart != null)
		{
			return EventReturnScrollUseStart();
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventReturnScrollUseCancel()
	{
		if (EventReturnScrollUseCancel != null)
		{
			EventReturnScrollUseCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventImmediateRevived()
	{
		if (EventImmediateRevived != null)
		{
			EventImmediateRevived();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventHpPotionUse()
	{
		if (EventHpPotionUse != null)
		{
			EventHpPotionUse();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventPartyCalled(int nContinentId, int nNationId, Vector3 vtPosition)
	{
		if (EventPartyCalled != null)
		{
			EventPartyCalled(nContinentId, nNationId, vtPosition);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool OnEventMapMove(int nContinentId, int nNationId, Vector3 vtPosition, int nPortalId = 0)
	{
		if (EventMapMove != null)
		{
			return EventMapMove(nContinentId, nNationId, vtPosition, nPortalId);
		}
		return false;
	}

    //---------------------------------------------------------------------------------------------------
	public void OnEventNpcAutoMove(int nNationId, CsNpcInfo csNpcInfo)
    {
        if (EventNpcAutoMove != null)
        {
			EventNpcAutoMove(nNationId, csNpcInfo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventAutoBattleStart(EnBattleMode enBattleMode)
	{
		if (EventAutoBattleStart != null)
		{
			EventAutoBattleStart(enBattleMode);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventAutoStop(EnAutoStateType enAutoStateType)
	{
		if (EventAutoStop != null)
		{
			EventAutoStop(enAutoStateType);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventTab()
	{
		if (EventTab != null)
		{
			EventTab();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMyHeroLevelUp()
	{
		if (EventMyHeroLevelUp != null)
		{
			EventMyHeroLevelUp();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventHeroMountEquipped(CsHeroMount csHeroMount)
	{
		if (EventHeroMountEquipped != null)
		{
			EventHeroMountEquipped(csHeroMount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMountGetOn()
	{
		if (EventMountGetOn != null)
		{
			EventMountGetOn();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMountGetOff()
	{
		if (EventMountGetOff != null)
		{
			EventMountGetOff();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventWingEquip()
	{
		if (EventWingEquip != null)
		{
			EventWingEquip();
		}
	}

    //---------------------------------------------------------------------------------------------------
    public void OnEventPlayerPrefsKeySet(EnPlayerPrefsKey enPlayerPrefsKey, int nValue)
    {
        if (EventPlayerPrefsKeySet != null)
        {
			EventPlayerPrefsKeySet(enPlayerPrefsKey, nValue);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventLoadingUIComplete()
	{
        if (EventLoadingUIComplete != null)
		{
            EventLoadingUIComplete();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventStartTutorial()
	{
		if (EventStartTutorial != null)
		{
			EventStartTutorial();
		}
	}

    //---------------------------------------------------------------------------------------------------
    public void OnEventDungeonEnter()
    {
        if (EventDungeonEnter != null)
        {
            EventDungeonEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventBossAppearSkip()
	{
        if (EventBossAppearSkip != null)
		{
            EventBossAppearSkip();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool OnEventTameMonsterUseSkill()
	{
		if (EventTameMonsterUseSkill != null)
		{
			return  EventTameMonsterUseSkill();
		}
		return false;
	}

    //---------------------------------------------------------------------------------------------------
    public void OnEventQuestCompltedError()
    {
        if (EventQuestCompltedError != null)
        {
            EventQuestCompltedError();
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventChangeCameraState(EnCameraMode enCameraMode)
    {
        if(EventChangeCameraState != null)
        {
            EventChangeCameraState(enCameraMode);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventSleepMode(bool bIson)
    {
        if(EventSleepMode != null)
        {
            EventSleepMode(bIson);
        }
    }

	//---------------------------------------------------------------------------------------------------
	public void OnEventGroggyMonsterItemStealStart()
	{
		if (EventGroggyMonsterItemStealStart != null)
		{
			EventGroggyMonsterItemStealStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	//public void OnEventContinentObjectInteractionStart()
	//{
	//	if (EventContinentObjectInteractionStart != null)
	//	{
	//		EventContinentObjectInteractionStart();
	//	}
	//}

	//---------------------------------------------------------------------------------------------------
	public void OnEventRequestNpcDialog()
	{
		if (EventRequestNpcDialog != null)
		{
			EventRequestNpcDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool OnEventCheckQuestAreaInHero()
	{
		if (EventCheckQuestAreaInHero != null)
		{
			return EventCheckQuestAreaInHero();
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventOtherHeroView(bool bView)
	{
		if (EventOtherHeroView != null)
		{
			EventOtherHeroView(bView);
		}
	}
}
