using System.Collections;
using UnityEngine;

public interface IPlayThemeQuestMain
{

}

public partial class CsPlayThemeQuestMain : CsPlayThemeQuest, IPlayThemeQuestMain
{
	bool m_bPlayDelayedBySkill;
	bool m_bArrival;
	CsState m_csState = new CsState();

	void SyncState()
	{
		if (CsMainQuestManager.Instance.MainQuest == null) return;

		switch (CsMainQuestManager.Instance.MainQuestState)
		{
		case EnMainQuestState.None: 
			SetState(new CsStateAccept());
			break;
		case EnMainQuestState.Accepted:
			EnMainQuestType enType = CsMainQuestManager.Instance.MainQuest.MainQuestType;
			if (enType == EnMainQuestType.Collect || enType == EnMainQuestType.Kill)
			{
				SetState(new CsStateExecuteBattle());
			}
			else if (enType == EnMainQuestType.Interaction)
			{
				SetState(new CsStateExecuteInteraction());
			}
			else if (enType == EnMainQuestType.Move)
			{
				SetState(new CsStateExecuteMove());
			}
			else if (enType == EnMainQuestType.Cart)
			{
				SetState(new CsStateExecuteCart());
			}
			else if (enType == EnMainQuestType.Dungeon)
			{
				SetState(new CsStateExecuteDungeon());
			}
			break;
		case EnMainQuestState.Executed:
			SetState(new CsStateComplete());
			break;
		default:
			SetState(new CsState());
			break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetState(CsState csStateNew)
	{
		if (m_csState != null)
		{
			m_csState.Uninit();
		}

		if (csStateNew != null)
		{
			csStateNew.Init(this, Player);
		}
		m_csState = csStateNew;
	}

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국에 있을때.
		{
			CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			if (csNpcInfo != null && Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange)) // 국가이동 Npc앞 도착.
			{
				NpcDialog(csNpcInfo);
				CsMainQuestManager.Instance.NationTransmissionReadyOK();
				return;
			}
		}

		m_bArrival = true;
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (IsThisAutoPlaying())
		{
			if (m_csState != null)
			{
				Player.StartCoroutine(CorutineStateChangedToIdle());
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		CsState cs = m_csState;

		EnMainQuestState en = CsMainQuestManager.Instance.MainQuestState;
		yield return new WaitForSeconds(0.2f);

		if (Player.IsStateIdle)
		{
			if (IsThisAutoPlaying())
			{
				if (m_bArrival)
				{
					m_bArrival = false;
				}
				else
				{
					m_csState.StateChangedToIdle();
				}
			}
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		Debug.Log("CsPlayThemeQuestMain.Init  csPlayer = " + csPlayer);
		base.Init(csPlayer);
			
		CsContinentObjectManager.Instance.MyPlayerGuid = m_csPlayer.HeroId;

		CsMainQuestManager csMainQuestManager = CsMainQuestManager.Instance;		
		csMainQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		csMainQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;

		csMainQuestManager.EventAccepted += OnEventAccepted;
		csMainQuestManager.EventCompleted += OnEventCompleted;
		csMainQuestManager.EventNextAutoPlay += OnEventNextAutoPlay;
		csMainQuestManager.EventExecuteDataUpdated += OnEventExecuteDataUpdated;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventAttackEnd += OnEventAttackEnd;
		Player.EventChangeCartRiding += OnEventChangeCartRiding;

		SyncState();

		ReStartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		CsMainQuestManager csMainQuestManager = CsMainQuestManager.Instance;
		csMainQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		csMainQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		csMainQuestManager.EventAccepted -= OnEventAccepted;
		csMainQuestManager.EventCompleted -= OnEventCompleted;
		csMainQuestManager.EventNextAutoPlay -= OnEventNextAutoPlay;
		csMainQuestManager.EventExecuteDataUpdated -= OnEventExecuteDataUpdated;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventAttackEnd -= OnEventAttackEnd;
		Player.EventChangeCartRiding -= OnEventChangeCartRiding;

		SetState(null);
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		m_bPlayDelayedBySkill = false;
		m_timer.Init(0.2f);
		if (m_corReStartCoroutine != null)
		{
			Player.StopCoroutine(m_corReStartCoroutine);
		}
		
		SyncState();
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsMainQuestManager.Instance.MainQuest == null)
		{
			Player.SetAutoPlay(null, true);
			return;
		}

		m_csState.Update();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestMain.StopAutoPlay() bNotify = " + bNotify);
			if (bNotify)
			{
				CsMainQuestManager.Instance.StopAutoPlay(this);
			}
		}
	}

	#endregion Override


	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (!CsIngameData.Instance.IngameManagement.IsContinent()) return;
		m_csState.ArrivalMoveByTouchByManual(bMoveByTouchTarget);
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventAttackEnd()
	{
		if (m_bPlayDelayedBySkill)
		{
			Debug.Log("OnEventAttackEnd()");
			m_bPlayDelayedBySkill = false;
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventChangeCartRiding()
	{
		if (IsThisAutoPlaying())
		{
			m_csState.OnEventChangeCartRiding();
		}
		else
		{
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.Player

	#region Event.QuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		MainQuestStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("CsPlayThemeQuestMain.OnEventAccepted  MainQuestType = "+ CsMainQuestManager.Instance.MainQuest.MainQuestType);
		SyncState();

		if (IsThisAutoPlaying())
		{
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventExecuteDataUpdated(int n)                   // 메인 퀘스트 갱신
	{
		SyncState();

		if (CsMainQuestManager.Instance.MainQuest.MainQuestType == EnMainQuestType.Move ||
			CsMainQuestManager.Instance.MainQuest.MainQuestType == EnMainQuestType.Cart ||
			CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed)
		{
			if (IsThisAutoPlaying())
			{
				PlayContinue();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest cs, bool b, long lAcquiredExp)           // 메인 퀘스트 완료 후	
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestMain.OnEventCompleted");
			PlayContinue();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventNextAutoPlay()
	{
		SyncState();

		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestMain.OnEventNextAutoPlay");
			PlayContinue();
		}
	}
	
	#endregion Event.QuestManager

	Coroutine m_corReStartCoroutine;
	//---------------------------------------------------------------------------------------------------
	void ReStartAutoPlay()
	{
		if (CsMainQuestManager.Instance.Auto)
		{
			m_corReStartCoroutine = Player.StartCoroutine(DelayStart());
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		Debug.Log("CsPlayThemeQuestMain.RestartAutoPlay()       CsMainQuestManager.Instance.Auto = " + CsMainQuestManager.Instance.Auto);
		MainQuestStart();
		m_corReStartCoroutine = null;
	}

	//---------------------------------------------------------------------------------------------------
	void MainQuestStart()
	{
		if (Player.Dead || CsMainQuestManager.Instance.MainQuest == null || !CsIngameData.Instance.IngameManagement.IsContinent())
		{
			CsMainQuestManager.Instance.StopAutoPlay(this);
			return;
		}

		SyncState();
		Player.SetAutoPlay(this, true);
		PlayContinue();
	}

	//---------------------------------------------------------------------------------------------------
	void PlayContinue()
	{
		m_csState.SetDisplayPath();

		if (Player.SkillStatus.IsStatusPlayAnim())
		{
			m_bPlayDelayedBySkill = true;
			Debug.Log("PlayContinue()1");
			return;
		}

		m_csState.Play();
	}
}
