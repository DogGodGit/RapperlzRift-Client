using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonWisdomTemple : CsPlayThemeDungeon
{
	enum WisdomTempleType { None, ColorMatching, FindTreasureBox, Quiz }

	CsWisdomTempleObject m_csWisdomTempleObject;
	WisdomTempleType m_enWisdomTempleType = WisdomTempleType.None;

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDungeonStartAutoPlay += OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay += OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventDugeonObjectInteractionStart += OnEventDugeonObjectInteractionStart;
		m_csDungeonManager.EventDungeonInteractionStartCancel += OnEventDungeonInteractionStartCancel;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionFinished += OnEventWisdomTemplePuzzleRewardObjectInteractionFinished;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionCancel += OnEventWisdomTemplePuzzleRewardObjectInteractionCancel;
		m_csDungeonManager.EventWisdomTempleColorMatchingObjectInteractionFinished += OnEventWisdomTempleColorMatchingObjectInteractionFinished;
		m_csDungeonManager.EventWisdomTempleColorMatchingObjectInteractionCancel += OnEventWisdomTempleColorMatchingObjectInteractionCancel;
		m_csDungeonManager.EventWisdomTempleColorMatchingMonsterKill += OnEventWisdomTempleColorMatchingMonsterKill;

		m_csDungeonManager.EventWisdomTempleStepStart += OnEventWisdomTempleStepStart;
		m_csDungeonManager.EventWisdomTempleBanished += OnEventWisdomTempleBanished;
		m_csDungeonManager.EventWisdomTempleAbandon += OnEventWisdomTempleAbandon;
		m_csDungeonManager.EventWisdomTempleExit += OnEventWisdomTempleExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventDugeonObjectInteractionStart -= OnEventDugeonObjectInteractionStart;
		m_csDungeonManager.EventDungeonInteractionStartCancel -= OnEventDungeonInteractionStartCancel;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionFinished -= OnEventWisdomTemplePuzzleRewardObjectInteractionFinished;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionCancel -= OnEventWisdomTemplePuzzleRewardObjectInteractionCancel;
		m_csDungeonManager.EventWisdomTempleColorMatchingObjectInteractionFinished -= OnEventWisdomTempleColorMatchingObjectInteractionFinished;
		m_csDungeonManager.EventWisdomTempleColorMatchingObjectInteractionCancel -= OnEventWisdomTempleColorMatchingObjectInteractionCancel;
		m_csDungeonManager.EventWisdomTempleColorMatchingMonsterKill -= OnEventWisdomTempleColorMatchingMonsterKill;

		m_csDungeonManager.EventWisdomTempleStepStart -= OnEventWisdomTempleStepStart;
		m_csDungeonManager.EventWisdomTempleBanished -= OnEventWisdomTempleBanished;
		m_csDungeonManager.EventWisdomTempleAbandon -= OnEventWisdomTempleAbandon;
		m_csDungeonManager.EventWisdomTempleExit -= OnEventWisdomTempleExit;
		m_csDungeonManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_bDungeonStart)
			{
				if (m_enWisdomTempleType == WisdomTempleType.FindTreasureBox || m_enWisdomTempleType == WisdomTempleType.Quiz)
				{
					DugeonBattle(Player.transform.position, 100f);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeDungeonWisdomTemple.StopAutoPlay() bNotify = " + bNotify);
			if (bNotify)
			{
				m_csDungeonManager.StopAutoPlay(this);
			}
		}
	}

	#endregion Override

	#region Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStartAutoPlay(EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.WisdomTemple) return;
		Debug.Log("CsPlayThemeDungeonWisdomTemple.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.WisdomTemple) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Debug.Log("CsPlayThemeDungeonWisdomTemple.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDugeonObjectInteractionStart()
	{
		if (m_csDungeonManager.WisdomTempleStep == null) return;

		Debug.Log("OnEventInteractionCheck    Player.IsStateIdle = " + Player.IsStateIdle);
		if (Player.IsStateIdle)
		{
			if (FindInteractionObject())
			{
				Player.SelectTarget(m_csWisdomTempleObject.transform, true);

				Player.LookAtPosition(m_csWisdomTempleObject.transform.position);
				Player.ChangeState(CsHero.EnState.Idle);
				Player.ChangeState(CsHero.EnState.Interaction);
				Player.ForceSyncMoveDataWithServer();

				if (m_csDungeonManager.WisdomTempleType == EnWisdomTempleType.ColorMatching)        // 색맞추기
				{
					m_csDungeonManager.ColorMatchingObjectInteraction(m_csWisdomTempleObject.InstanceId, m_csWisdomTempleObject.ObjectId, m_csWisdomTempleObject.InteractionDuration);
				}
				else if (m_csDungeonManager.WisdomTempleType == EnWisdomTempleType.PuzzleReward)    // 퍼즐완료보상
				{
					m_csDungeonManager.PuzzleRewardObjectInteraction(m_csWisdomTempleObject.InstanceId, m_csWisdomTempleObject.InteractionDuration);
				}
				else
				{
					Player.ChangeState(CsHero.EnState.Idle);
					Debug.Log(" OnEventInteractionCheck       >>>>>>        상호장용 해당 상태 없음 확인 필요.");
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonInteractionStartCancel()
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleRewardObjectInteractionFinished(PDItemBooty booty, long lInstanceId)
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleRewardObjectInteractionCancel()
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingObjectInteractionFinished(PDWisdomTempleColorMatchingObjectInstance pDWisdomTempleColorMatchingObjectInstance)
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingMonsterKill(PDWisdomTempleColorMatchingObjectInstance[] colorMatchingObjectInsts, int nColorMatchingPoint)
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingObjectInteractionCancel()
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepStart(PDWisdomTempleMonsterInstance[] mon, PDWisdomTempleColorMatchingObjectInstance[] color, int nQuizNo)
	{
		m_bDungeonStart = true;
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	bool FindInteractionObject()
	{
		Transform trTarget = CsMyPlayer.FindTarget(Player.transform.position, Player.transform.position, "Object", "InteractionObject", 5);
		if (trTarget != null)
		{
			m_csWisdomTempleObject = trTarget.GetComponent<CsWisdomTempleObject>();
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void ResetInteractionData()
	{
		if (m_csWisdomTempleObject != null)
		{
			if (m_csWisdomTempleObject.transform == Player.Target)
			{
				Player.ResetTarget();
			}
			m_csWisdomTempleObject = null;
		}
	}
}
