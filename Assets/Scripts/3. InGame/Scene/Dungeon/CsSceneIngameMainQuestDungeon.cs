using ClientCommon;
using System;
using System.Collections;
using UnityEngine;

public class CsSceneIngameMainQuestDungeon : CsSceneIngame
{
	AudioClip m_audioClipDefault = null;

	//----------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		Debug.Log("CsSceneIngameDungeon.Start");
		base.Start();
		CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter += OnEventContinentExitForMainQuestDungeonEnter;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter += OnEventMainQuestDungeonEnter;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon += OnEventMainQuestDungeonAbandon;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonSaftyRevive += OnEventMainQuestDungeonSaftyRevive;
	
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepStart += OnEventMainQuestDungeonStepStart;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted += OnEventMainQuestDungeonStepCompleted;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;

		m_audioClipDefault = m_audioSource.clip;
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		Debug.Log("CsSceneIngameDungeon.OnDestroy()");
		CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter -= OnEventContinentExitForMainQuestDungeonEnter;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter -= OnEventMainQuestDungeonEnter;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon -= OnEventMainQuestDungeonAbandon;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonSaftyRevive -= OnEventMainQuestDungeonSaftyRevive;

		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepStart -= OnEventMainQuestDungeonStepStart;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted -= OnEventMainQuestDungeonStepCompleted;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;
		base.OnDestroy();
	}

	#region MainQuestDungeon

	//----------------------------------------------------------------------------------------------------
	void OnEventContinentExitForMainQuestDungeonEnter(bool bChaegeScene) // 메인퀘스트던전입장을위한대륙퇴장    >>    모든 오브젝트 초기화.
	{
		Debug.Log("OnEventContinentExitForMainQuestDungeonEnter()   bChaegeScene= " + bChaegeScene);
		m_bChaegeScene = bChaegeScene;

		if (bChaegeScene == false)
		{
			DungeonEnter();
			StartCoroutine(SceneObjectClear(0.6f, true));
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventMainQuestDungeonEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId) // 메인퀘스트던전입장
	{
		m_audioSource.Stop();
		m_audioSource.clip = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/BGM/BGM_Marduka_Eimus");

		if (CsIngameData.Instance.BGM)
		{
			m_audioSource.Play();
		}

		SetMyHeroLocation(CsMainQuestDungeonManager.Instance.MainQuestDungeon.LocationId);		
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < CsMainQuestDungeonManager.Instance.MainQuestDungeon.MainQuestDungeonObstacleList.Count; i++) // 장애물 생성.
		{
			CsMainQuestDungeonObstacle Obstacle = CsMainQuestDungeonManager.Instance.MainQuestDungeon.MainQuestDungeonObstacleList[i];
			CreateObstacle(Obstacle.ObstacleId, new Vector3(Obstacle.XPosition, Obstacle.YPosition, Obstacle.ZPosition), Obstacle.YRotation, Obstacle.Scale);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonSaftyRevive() // 메인퀘스트던전안전부활
	{
		m_csPlayer.NetEventRevive();
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnEventMainQuestDungeonStepStart(PDMainQuestDungeonMonsterInstance[] apDMainQuestDungeonMonsterInstance) // 메인퀘스트던전단계시작
	{
		if (CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.Type == 1)
		{
			RemoveObstacle(CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.RemoveObstacleId);
		}
		for (int i = 0; i < apDMainQuestDungeonMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apDMainQuestDungeonMonsterInstance[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonStepCompleted(bool bLevelUp, long lGold, long lAcquiredExp)
	{
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonAbandon(int nContinentId, bool bChaegeScene)
	{
		MainQuestDungeonClear(bChaegeScene);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonBanished(int nContinentId, bool bChaegeScene)
	{
		MainQuestDungeonClear(bChaegeScene);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonExit(int nContinentId, bool bChaegeScene)
	{
		MainQuestDungeonClear(bChaegeScene);
	}

	#endregion MainQuestDungeon

	//----------------------------------------------------------------------------------------------------
	void MainQuestDungeonClear(bool bChaegeScene)
	{
		m_bChaegeScene = bChaegeScene;
		Debug.Log("MainQuestDungeonClear    m_bChaegeScene = "+ m_bChaegeScene);

		if (!bChaegeScene)
		{
			if (CsMainQuestDungeonManager.Instance.MainQuestDungeon.DungeonId == 2 && CsMainQuestDungeonManager.Instance.MainQuestDungeonClear) // 메인 퀘스트던전 클리어 연출.
			{
				StartCoroutine(DungeonClearDirect());
			}

			StartCoroutine(SceneObjectClear(0.6f));
			StartCoroutine(DelaySendPrevContinentEnterCommand(0.6f));
		}

		for (int i = 0; i < CsMainQuestDungeonManager.Instance.MainQuestDungeon.MainQuestDungeonObstacleList.Count; i++) // 장애물 제거.
		{
			RemoveObstacle(CsMainQuestDungeonManager.Instance.MainQuestDungeon.MainQuestDungeonObstacleList[i].ObstacleId);
		}

		DungeonExit();
		CsDungeonManager.Instance.ResetDungeon();

		m_audioSource.Stop();
		m_audioSource.clip = m_audioClipDefault;
		m_audioSource.Play();

		if (CsIngameData.Instance.BGM)
		{
			m_audioSource.Play();
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator SceneObjectClear(float flDelayTime, bool bEnter = false) // Scene 모든 객체 초기화.
	{
		yield return new WaitForSeconds(flDelayTime);					// UI FadeOut 속도에 맞춰 지연 처리.
		yield return new WaitUntil(() => CsIngameData.Instance.Directing == false); // 연출이 있는 경우.
		ClearMyHero();
		ClearHero();
		ClearMonster();
		ClearNpc();
		ClearInteractionObject();
		ClearPortal();
		ClearCartObject();

		yield return new WaitForSeconds(0.1f);

		if (bEnter) // 던전 진입에 의한 오브젝트 초기화.
		{
			if (CsMainQuestDungeonManager.Instance.MainQuestDungeon.DungeonId == 2) // 진입시 연출 있음.
			{
				CinemachineChief();
				CsGameEventToUI.Instance.OnEventFade(false);
				yield return new WaitUntil(() => CsIngameData.Instance.Directing == false); // 연출이 있는 경우 연출이 끝나고 진입 요청.
			}

			CsMainQuestDungeonManager.Instance.SendMainQuestDungeonEnter();
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator DungeonClearDirect()
	{
		Debug.Log(" DungeonClearDirect()   Directing = " + CsIngameData.Instance.Directing);
		CsIngameData.Instance.Directing = true;
		CsGameEventToUI.Instance.OnEventJoystickReset();
		m_csPlayer.SkillStatus.Reset();
		m_csPlayer.MyHeroNavMeshAgent.enabled = false;
		m_csPlayer.transform.position = new Vector3(81, 14, 230);
		m_csPlayer.ChangeEulerAngles(180f);
		m_csPlayer.MyHeroNavMeshAgent.enabled = true;
		CsIngameData.Instance.InGameCamera.SetFlightCamera(EnCameraMode.Camera3D);
		CsGameEventToUI.Instance.OnEventFade(false);
		CsGameEventToUI.Instance.OnEventHideMainUI(false);

		yield return new WaitForSeconds(0.5f);
		m_csPlayer.ChangeState(CsHero.EnState.Interaction);
		CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, m_csPlayer.transform, m_csPlayer.transform.position, "Flight_Direction", 3f);

		yield return new WaitForSeconds(3f);
		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.Flight);

		yield return new WaitUntil(() => m_csPlayer.IsTransformationStateFlight());
		yield return new WaitForSeconds(0.2f);
		m_csPlayer.MoveToPos(new Vector3(219, 12.8f, 163), 0.5f, false);

		yield return new WaitForSeconds(3f);
		m_csPlayer.FlightAccelerate = true;

		yield return new WaitForSeconds(2f);
		yield return new WaitUntil(() => m_csPlayer.MyHeroNavMeshAgent.baseOffset < 9f);
		m_csPlayer.FlightAccelerate = false;

		yield return new WaitUntil(() => m_csPlayer.IsTransformationStateNone());
		yield return new WaitForSeconds(2f);
		CsIngameData.Instance.IngameManagement.DirectingEnd(true);
		yield return new WaitForSeconds(1f);
		CsIngameData.Instance.InGameCamera.ResetCamera();
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator DelaySendPrevContinentEnterCommand(float flDelayTime) // 이전대륙입장 지연처리.
	{
		m_csPlayer.SetAutoPlay(null, true);
		yield return new WaitForSeconds(flDelayTime);
		yield return new WaitUntil(() => CsIngameData.Instance.Directing == false); // 연출이 있는 경우 연출이 끝나고 진입 요청.		
		SendPrevContinentEnterCommand();
	}

	//----------------------------------------------------------------------------------------------------
	void CinemachineChief() // 메인퀘스트 던전 2번 입장 연출
	{
		Debug.Log("CinemachineChief()  Directing = " + CsIngameData.Instance.Directing);

		CsIngameData.Instance.Directing = true;

		if (m_csPlayer != null)
		{
			m_csPlayer.MyHeroView(false);
		}

		CsGameEventToUI.Instance.OnEventFade(false);
		CsGameEventToUI.Instance.OnEventHideMainUI(true);
		GameObject goAppearChief = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Cinema/CineScene_Intro_Chief"), transform) as GameObject;
		goAppearChief.GetComponent<CsCinemachineSceneIntroChief>().Init();
	}
}

