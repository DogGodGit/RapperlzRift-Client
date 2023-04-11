using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CsGameEventToUI
{
	public static CsGameEventToUI Instance
	{
		get { return CsSingleton<CsGameEventToUI>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<bool> EventSceneLoadComplete;														// 인게임로딩종료
	public event Delegate<bool, bool> EventFade;															// 연출 시작 / 종료.

	public event Delegate<bool, int> EventConfirmUseSkill;													// 스킬사용요청에 대한 결과
	public event Delegate<bool, int> EventConfirmUseCommonSkill;                                            // 공용 스킬사용요청에 대한 결과
	public event Delegate<bool, int> EventConfirmUseRankActiveSkill;                                        // 계급 스킬사용요청에 대한 결과
	public event Delegate<bool, int> EventConfirmUseTransformationSkill;									// 계급 스킬사용요청에 대한 결과
	
	public event Delegate<int> EventUseAutoSkill;															// 자동사냥중 스킬사용

    public event DelegateR<RectTransform, CsHeroBase, string, int, int, int, bool, bool, EnNationWarPlayerState, int, int> EventCreateHeroHUD;		// 영웅HUD생성
	public event Delegate<Guid> EventDeleteHeroHUD;															// 영웅HUD삭제

	public event DelegateR<RectTransform, int> EventCreateNpcHUD;											// NPC HUD생성
	public event Delegate<int> EventDeleteNpcHUD;															// NPC HUD삭제

    public event DelegateR<RectTransform, int> EventCreateGuildNpcHUD;                                      // 길드 NPC HUD생성
    public event Delegate<int> EventDeleteGuildNpcHUD;                                                      // 길드 NPC HUD삭제

	public event DelegateR<RectTransform, int> EventCreateNationWarNpcHUD;									// 국가전 NPC HUD생성  
	public event Delegate<int> EventDeleteNationWarNpcHUD;													// 국가전 NPC HUD삭제

    public event DelegateR<RectTransform, long, CsMonsterInfo, string, bool, int> EventCreateMonsterHUD;	// Monster HUD생성
	public event Delegate<long> EventDeleteMonsterHUD;                                                      // Monster HUD삭제
	public event Delegate<long> EventMonsterAttackToMyHero;														// Monster MyHero 공격.

	public event DelegateR<RectTransform> EventCreateTameMonster;											// Tame 몬스터 테이밍 UI 생성.
	public event Delegate EventDeleteTameMonster;															// Tame 몬스터 테이밍 UI 삭제.

	public event DelegateR<RectTransform, long> EventCreateCartHUD;											// Cart HUD 생성
	public event Delegate<long> EventDeleteCartHUD;															// Cart HUD 삭제

	public event Delegate EventReturnScrollUseCancel;														// ReturnScroll 사용 취소
	public event Delegate EventGroggyMonsterItemStealCancel;												// 그로기몬스터아이템훔치기취소
	
	public event Delegate<string> EventHeroDead;															// 영웅사망

	public event Delegate EventMyHeroInfoUpdate;                                                            // 영웅정보 업데이트

	public event Delegate<int> EventPortalEnter;															// 포탈이동 시작

	public event Delegate<EnAutoStateType> EventAutoStop;													// 자동 중단.	

	public event Delegate EventPrevContinentEnter;															// 던전에서 대륙으로 입장.

	public event Delegate EventMountGetOff;																	// 탈것 내리기 요청.

	public event Delegate<bool> EventHideMainUI;															// MainUI 끄고 켜기.
				
	public event Delegate<int> EventArrivalNpcByTouch;														// 터치 이동으로 Npc위치 도착.
	public event Delegate<int, int> EventArrivalNpcByAuto;													// 자동 이동으로 NPC위치 도착.

	public event Delegate<EnDamageTextType, int, Vector2, bool> EventCreatDamageText;

	public event Delegate EventSelectInfoUpdate;															// 선택 대상 정보 갱신.
	public event Delegate<Guid> EventSelectHeroInfo;														// 선택 영웅 정보 갱신.
	public event Delegate EventSelectHeroInfoStop;															// 영웅 선택 취소.
	public event Delegate<long, int> EventSelectMonsterInfo;												// 선택 몬스터 정보 갱신.
	public event Delegate EventSelectMonsterInfoStop;														// 몬스터 선택 취소.
	public event Delegate<long> EventSelectCartInfo;														// 선택 카트 정보 갱신.
	public event Delegate EventSelectCartInfoStop;															// 카트 선택 취소.

	public event Delegate EventStartTutorial;																// 튜토리얼 시작.

	public event Delegate EventContinentSaftySceneLoad;														// 안전대륙씬 로드 요청.

	public event Delegate EventHitCombo;																	// 몬스터 타격 콤보
	public event Delegate<string, bool> EventBossAppear;													// 보스등장.
	public event Delegate<EnDungeonPlay> EventClearDirectionFinish;											// 클리어 연출 종료
	public event Delegate<long, CsMonsterInfo, int> EventCreateBossMonster;

	public event Delegate EventJoystickReset;
	public event Delegate EventJoystickStartAutoMove;														// 조이스틱다운에 의한 자동이동 시작.
	public event Delegate<bool> EventTameButton;															// 테이밍UI로 교체 or 복귀.

	public event Delegate EventMyHeoGroggyMonsterItemStealCancel;											// 아이테훔치기 취소.
	public event Delegate<EnBattleMode> EventChangeAutoBattleMode;											// 자동전투 모드 변경

	public event Delegate<bool, int> EventNpcInteractionArea;                                               // Npc 상호작용 범위 활성, 비활성.
	public event Delegate EventChangeResolution;                                                            // Resolution 변경

	//---------------------------------------------------------------------------------------------------
	public void OnEventSceneLoadComplete(bool bChaegeScene)
	{
		if (EventSceneLoadComplete != null)
		{
			EventSceneLoadComplete(bChaegeScene);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventFade(bool bFadeStart, bool bFast = false)
	{
		Debug.Log("#####     OnEventFade     ######     bFadeStart = " + bFadeStart + " // bFast = " + bFast);
		if (EventFade != null)
		{
			EventFade(bFadeStart, bFast);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventConfirmUseSkill(bool bReturn, int nSkillId)
	{
		if (EventConfirmUseSkill != null)
		{
			EventConfirmUseSkill(bReturn, nSkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventConfirmUseCommonSkill(bool bReturn, int nSkillId)
	{
		if (EventConfirmUseCommonSkill != null)
		{
			EventConfirmUseCommonSkill(bReturn, nSkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventConfirmUseRankActiveSkill(bool bReturn, int nSkillId)
	{
		if (EventConfirmUseRankActiveSkill != null)
		{
			EventConfirmUseRankActiveSkill(bReturn, nSkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventConfirmUseTransformationSkill(bool bReturn, int nSkillId)
	{
		if (EventConfirmUseTransformationSkill != null)
		{
			EventConfirmUseTransformationSkill(bReturn, nSkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventUseAutoSkill(int nSkillId)
	{
		if (EventUseAutoSkill != null)
		{
			EventUseAutoSkill(nSkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public RectTransform OnEventCreateHeroHUD(CsHeroBase csHeroBase, string strGuildName, int nGuildMemberGrade, int nPickedSecretLetterGrade, int nPickedMysteryBoxGrade,
											  bool bDistorting, bool bSafeMode, EnNationWarPlayerState enNationWarPlayerState, int nNoblesseId, int nTileId)
	{
		if (EventCreateHeroHUD != null)
		{
			return EventCreateHeroHUD(csHeroBase, 
									  strGuildName, 
									  nGuildMemberGrade, 
									  nPickedSecretLetterGrade, 
									  nPickedMysteryBoxGrade, 
									  bDistorting,
									  bSafeMode,
									  enNationWarPlayerState, 
									  nNoblesseId, 
									  nTileId);
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteHeroHUD(Guid guidHeroId)
	{
		if (EventDeleteHeroHUD != null)
		{
			EventDeleteHeroHUD(guidHeroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public RectTransform OnEventCreateNpcHUD(int nNpcId)
	{
		if (EventCreateNpcHUD != null)
		{
			return EventCreateNpcHUD(nNpcId);
		}

		return null;
	}
	
	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteNpcHUD(int nNpcId)
	{
		if (EventDeleteNpcHUD != null)
		{
			EventDeleteNpcHUD(nNpcId);
		}
	}

    //---------------------------------------------------------------------------------------------------
    public RectTransform OnEventCreateGuildNpcHUD(int nNpcId)
    {
        if (EventCreateGuildNpcHUD != null)
        {
            return EventCreateGuildNpcHUD(nNpcId);
        }

        return null;
    }

	
    //---------------------------------------------------------------------------------------------------
    public void OnEventDeleteGuildNpcHUD(int nNpcId)
    {
        if (EventDeleteGuildNpcHUD != null)
        {
            EventDeleteGuildNpcHUD(nNpcId);
        }
    }

	//---------------------------------------------------------------------------------------------------
	public RectTransform OnEventCreateNationWarNpcHUD(int nNpcId)
	{
		if (EventCreateNationWarNpcHUD != null)
		{
			return EventCreateNationWarNpcHUD(nNpcId);
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteNationWarNpcHUD(int nNpcId)
	{
		if (EventDeleteNationWarNpcHUD != null)
		{
			EventDeleteNationWarNpcHUD(nNpcId);
		}
	}
	
    //---------------------------------------------------------------------------------------------------
    public RectTransform OnEventCreateMonsterHUD(long lInstanceId, CsMonsterInfo csMonsterInfo, string strHeroName, bool bExclusive, int nHp)
	{
		if (EventCreateMonsterHUD != null)
		{
			return EventCreateMonsterHUD(lInstanceId, csMonsterInfo, strHeroName, bExclusive, nHp);
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteMonsterHUD(long lInstanceId)
	{
		if (EventDeleteMonsterHUD != null)
		{
			EventDeleteMonsterHUD(lInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMonsterAttackToMyHero(long lInstanceId)
	{
		if (EventMonsterAttackToMyHero != null)
		{
			EventMonsterAttackToMyHero(lInstanceId);
		}
	}
	
	//---------------------------------------------------------------------------------------------------
	public RectTransform OnEventCreateTameMonster()
	{
		if (EventCreateTameMonster != null)
		{
			return EventCreateTameMonster();
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteTameMonster()
	{
		if (EventDeleteTameMonster != null)
		{
			EventDeleteTameMonster();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public RectTransform OnEventCreateCartHUD(long lInstanceId)
	{
		if (EventCreateCartHUD != null)
		{
			return EventCreateCartHUD(lInstanceId);
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteCartHUD(long lInstanceId)
	{
		if (EventDeleteCartHUD != null)
		{
			EventDeleteCartHUD(lInstanceId);
		}
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
	public void OnEventGroggyMonsterItemStealCancel()
	{
		if (EventGroggyMonsterItemStealCancel != null)
		{
			EventGroggyMonsterItemStealCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventHeroDead(string strName)
	{
		if (EventHeroDead != null)
		{
			EventHeroDead(strName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventMyHeroInfoUpdate()
	{
		if (EventMyHeroInfoUpdate != null)
		{
			EventMyHeroInfoUpdate();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventPortalEnter(int nPortalId)
	{
		if (EventPortalEnter != null)
		{
			EventPortalEnter(nPortalId);
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
	public void OnEventPrevContinentEnter()
	{
		if (EventPrevContinentEnter != null)
		{
			EventPrevContinentEnter();
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
	public void OnEventHideMainUI(bool bHide)
	{
		if (EventHideMainUI != null)
		{			
			EventHideMainUI(bHide);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventTameButton(bool bHide)
	{
		if (EventTameButton != null)
		{
			EventTameButton(bHide);
		}
	}
	
	//---------------------------------------------------------------------------------------------------
	public void OnEventArrivalNpcByTouch(int nNpcId)
	{
		if (EventArrivalNpcByTouch != null)
		{
			EventArrivalNpcByTouch(nNpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventArrivalNpcByAuto(int nNpcId, int nNationId = 0)
	{
		if (EventArrivalNpcByAuto != null)
		{
			EventArrivalNpcByAuto(nNpcId, nNationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventCreatDamageText(EnDamageTextType enDamageTextType, int nDamgae, Vector2 vtPos, bool bHero)
	{
		if (EventCreatDamageText != null)
		{
			EventCreatDamageText(enDamageTextType, nDamgae, vtPos, bHero);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectInfoUpdate()
	{
		if (EventSelectInfoUpdate != null)
		{
			EventSelectInfoUpdate();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectHeroInfo(Guid guidHeroId)
	{
		if (EventSelectHeroInfo != null)
		{
			EventSelectHeroInfo(guidHeroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectHeroInfoStop()
	{
		if (EventSelectHeroInfoStop != null)
		{
			EventSelectHeroInfoStop();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectMonsterInfo(long lInstanceId, int nHpLineCount = 1)
	{
		if (EventSelectMonsterInfo != null)
		{
			EventSelectMonsterInfo(lInstanceId, nHpLineCount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectMonsterInfoStop()
	{
		if (EventSelectMonsterInfoStop != null)
		{
			EventSelectMonsterInfoStop();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectCartInfo(long lInstanceId)
	{
		if (EventSelectCartInfo != null)
		{
			EventSelectCartInfo(lInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventSelectCartInfoStop()
	{
		if (EventSelectCartInfoStop != null)
		{
			EventSelectCartInfoStop();
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
	public void OnEventContinentSaftySceneLoad()
	{
		if (EventContinentSaftySceneLoad != null)
		{
			EventContinentSaftySceneLoad();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventHitCombo()
	{
		if (EventHitCombo != null)
		{
			EventHitCombo();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventBossAppear(string strName, bool bActive)
	{
		if (EventBossAppear != null)
		{
			EventBossAppear(strName, bActive);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventClearDirectionFinish(EnDungeonPlay enDungeonPlay)
	{
		if (EventClearDirectionFinish != null)
		{
			EventClearDirectionFinish(enDungeonPlay);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventCreateBossMonster(long lInstanceId, CsMonsterInfo csMonsterInfo, int nHpLineCount = 1)
	{
		if (EventCreateBossMonster != null)
		{
			EventCreateBossMonster(lInstanceId, csMonsterInfo, nHpLineCount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventJoystickReset()
	{
		if (EventJoystickReset != null)
		{
			EventJoystickReset();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventJoystickStartAutoMove()
	{
		if (EventJoystickStartAutoMove != null)
		{
			EventJoystickStartAutoMove();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnMyHeoGroggyMonsterItemStealCancel()
	{
		if (EventMyHeoGroggyMonsterItemStealCancel != null)
		{
			EventMyHeoGroggyMonsterItemStealCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventChangeAutoBattleMode(EnBattleMode enBattleMode)
	{
		if (EventChangeAutoBattleMode != null)
		{
			EventChangeAutoBattleMode(enBattleMode);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventNpcInteractionArea(bool bEnter, int nNpcId)
	{
		if (EventNpcInteractionArea != null)
		{
			EventNpcInteractionArea(bEnter, nNpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventChangeResolution()
	{
		if (EventChangeResolution != null)
		{
			EventChangeResolution();
		}
	}
}
