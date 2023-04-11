using ClientCommon;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CsSceneDun01_Quest1 : CsSceneIngameMainQuestDungeon // 메인퀘스트 3번 던전.
{
    GameObject m_goDun01BossRoom = null;
    GameObject m_goEffectTaxian = null;
    bool m_bIsPlayTimeline = false;
	PlayableDirector m_playableDirector = null;

    //---------------------------------------------------------------------------------------------------
    public GameObject EffectTaxian { get { return m_goEffectTaxian; } }
    public bool IsPlayTimeline { get { return m_bIsPlayTimeline; } set { m_bIsPlayTimeline = value; } }
	public PlayableDirector Director { get { return m_playableDirector; } }

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter += OnEventContinentExitForMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonFail += OnEventMainQuestDungeonFail;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonMonsterSummon += OnEventMainQuestDungeonMonsterSummon;
		CsMainQuestDungeonManager.Instance.SendMainQuestDungeonEnter();

        m_goDun01BossRoom = GameObject.Find("Dun01_BossRoom");
        m_goDun01BossRoom.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonEnter();
        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter -= OnEventContinentExitForMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonFail -= OnEventMainQuestDungeonFail;
		CsMainQuestDungeonManager.Instance.EventMainQuestDungeonMonsterSummon -= OnEventMainQuestDungeonMonsterSummon;
		base.OnDestroy();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventContinentExitForMainQuestDungeonEnter(bool bChangeScene)
    {
        DungeonEnter();
    }

    //----------------------------------------------------------------------------------------------------
    protected override void OnEventMainQuestDungeonStepStart(PDMainQuestDungeonMonsterInstance[] apMainQuestDungeonMonsterInstance)
    {
		Debug.Log("OnEventMainQuestDungeonStepStart                       Current StepType: " + CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.Type);
		if (CsIngameData.Instance.MyHeroDead) return;

		if (CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.Type == 3)
		{
			StartCoroutine(StratTimeline());
		}
		else
		{
			if (m_playableDirector  != null && m_playableDirector.state == PlayState.Playing)
			{
				m_playableDirector.Stop();
			}
		}


		base.OnEventMainQuestDungeonStepStart(apMainQuestDungeonMonsterInstance);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonMonsterSummon(long lInstanceId, PDMainQuestDungeonSummonMonsterInstance[] apDMainQuestDungeonSummonMonsterInstance) // 메인퀘스트던전몬스터소환
	{
		if (m_dicMonsters.ContainsKey(lInstanceId)) // 소환 스킬을 사용하는 몬스가 존재 하는경우.
		{
			m_dicMonsters[lInstanceId].NetEventMonsterSummonSkillCast(); // 해당 보스 몬스터 소환스킬 시전.

			for (int i = 0; i < apDMainQuestDungeonSummonMonsterInstance.Length; i++)
			{
				StartCoroutine(AsyncCreateMonster(apDMainQuestDungeonSummonMonsterInstance[i]));
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator StratTimeline()
	{
		m_csPlayer.SetAutoPlay(null, false);
		m_csPlayer.ResetBattleMode();
		yield return new WaitForSeconds(1.5f);

		CsGameEventToUI.Instance.OnEventHideMainUI(true);
		yield return new WaitForSeconds(1f);

		m_csPlayer.SetAutoPlay(null, false);
		m_goDun01BossRoom.SetActive(true);
		CsGameEventToUI.Instance.OnEventFade(true);
		yield return new WaitForSeconds(0.5f);

		GameObject.Find("Dun01_Objects").SetActive(false);
		CsGameEventToUI.Instance.OnEventJoystickReset();
		m_csPlayer.MyHeroNavMeshAgent.enabled = false;
		yield return null;

        m_csPlayer.transform.position = CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.TargetPosition;
        m_csPlayer.ChangeEulerAngles(CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.DirectingStartYRotation);
        yield return new WaitForSeconds(0.2f);

		CsIngameData.Instance.InGameCamera.CameraSetValue(0.15f, 3.14f, 0.7f, 4f);
		yield return null;

		m_csPlayer.MyHeroNavMeshAgent.enabled = true;
		m_playableDirector = transform.Find("Timeline").GetComponent<PlayableDirector>();
        m_goEffectTaxian = GameObject.Find("Dun01_BossRoom/Effect_taxian");
		m_goEffectTaxian.transform.Find("Mesh_posui03").GetComponent<Animation>().clip = CsIngameData.Instance.LoadAsset<AnimationClip>("Prefab/Cinema/Dungeon/Take 001");
		m_goEffectTaxian.transform.Find("Mesh_posui_neiquan01").GetComponent<Animation>().clip = CsIngameData.Instance.LoadAsset<AnimationClip>("Prefab/Cinema/Dungeon/back");
		m_goEffectTaxian.transform.Find("MEsh_posui_003").GetComponent<Animation>().clip = CsIngameData.Instance.LoadAsset<AnimationClip>("Prefab/Cinema/Dungeon/Take 002");
		m_playableDirector.gameObject.SetActive(true);
		m_playableDirector.Play();
		CsGameEventToUI.Instance.OnEventFade(false);
		m_bIsPlayTimeline = true;
		yield return new WaitUntil(() => m_bIsPlayTimeline == false);

		CsIngameData.Instance.InGameCamera.ResetCamera();
		yield return new WaitForSeconds(1f);

		CsIngameData.Instance.InGameCamera.ChangeNewState(CsIngameData.Instance.CameraMode);
		CsGameEventToUI.Instance.OnEventHideMainUI(false);
	}

	//----------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonFail()
    {
        ClearMonster();
    }

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonMainQuest());
	}	
}
