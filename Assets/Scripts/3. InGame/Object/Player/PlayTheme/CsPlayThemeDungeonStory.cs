using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonStory : CsPlayThemeDungeon
{
	#region IAutoPlay

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDungeonStartAutoPlay += OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay += OnEventStopAutoPlay;

		m_csDungeonManager.EventStoryDungeonHeroStartTame += OnEventStoryDungeonConfirmTame;
		CsGameEventToIngame.Instance.EventTameMonsterUseSkill += OnEventTameMonsterUseSkill;                                // 스킬사용요청.

		m_csDungeonManager.EventStoryDungeonStepStart += OnEventStoryDungeonStepStart;
        m_csDungeonManager.EventStoryDungeonAbandon += OnEventStoryDungeonAbandon;
		m_csDungeonManager.EventStoryDungeonBanished += OnEventStoryDungeonBanished;
		m_csDungeonManager.EventStoryDungeonExit += OnEventStoryDungeonExit;

	} 

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventStoryDungeonHeroStartTame -= OnEventStoryDungeonConfirmTame;
		CsGameEventToIngame.Instance.EventTameMonsterUseSkill -= OnEventTameMonsterUseSkill;                                // 스킬사용요청.

		m_csDungeonManager.EventStoryDungeonStepStart -= OnEventStoryDungeonStepStart;
		m_csDungeonManager.EventStoryDungeonAbandon -= OnEventStoryDungeonAbandon;
		m_csDungeonManager.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;
		m_csDungeonManager.EventStoryDungeonExit -= OnEventStoryDungeonExit;

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
		if (m_bDungeonStart)
		{
			PlayAutoStoryDungeon();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeQuestMain.StopAutoPlay() bNotify = " + bNotify);
			if (bNotify)
			{
				m_csDungeonManager.StopAutoPlay(this);
			}
		}
	}

	#endregion Override

	#region Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay(EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.Story) return;
		Debug.Log("CsPlayThemeDugeonStory.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			m_csDungeonManager.StopAutoPlay(this);
			return;
		}

		SetDisplayPath();
		PlayAutoStoryDungeon();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.Story) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Debug.Log("CsPlayThemeDugeonStory.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonStepStart(PDStoryDungeonMonsterInstance[] apDStoryDungeonMonsterInstance)
	{
		m_bDungeonStart = true;
		if (IsThisAutoPlaying())
		{
			SetDisplayPath();
			PlayAutoStoryDungeon();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonBanished(int nContinent)
	{
		ExitDungeon();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonAbandon(int nContinent)
    {
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonExit(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonConfirmTame()
	{
		if (Player.SkillStatus.IsStatusPlayAnim())
		{
			Debug.Log("OnEventStoryDungeonConfirmTame()   Player.SkillStatus.IsStatusPlayAnim()");
			return;
		}
		m_csDungeonManager.SendStoryDungeonMonsterTame(CsIngameData.Instance.TameMonster.InstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventTameMonsterUseSkill()
	{
		Debug.Log("1. OnEventTameMonsterUseSkill()");
		if (CsIngameData.Instance.TameMonster.IsAttack) return false; // 공격중.

		if (!Player.ChangeTargetAutoToEnemy())
		{
			Player.SelectTargetEnemy(Player.FindBestTarget(Player.transform.position, Player.transform.position, 50), true);
		}
		
		if (Player.TargetEnemy == null)	// 허공에 공격.
		{
			CsIngameData.Instance.TameMonster.PlayBattle(null);
			return true;
		}
		else
		{
			if (MoveTameMonster(Player.TargetEnemy.position, CsIngameData.Instance.TameMonster.GetCastRange()))
			{
				Debug.Log("4. OnEventTameMonsterUseSkill()");
				Player.LookAtPosition(Player.TargetEnemy.position);
				DugeonBattle(Player.transform.position, 50f);
				return true; // 공격.
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	bool MoveTameMonster(Vector3 vtPosition, float flRange)
	{
		if (Player.IsTargetInDistance(vtPosition, flRange))
		{
			return true;
		}

		if (Player.IsStateIdle)
		{
			Player.MoveToPos(vtPosition, flRange, false);
		}
		return false;
	}

	#endregion Event.DungeonManager

	enum EnStroyDugeonType { Move = 1, Battle, Boss, Tame }

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null) return;
		EnWayPointType enWayPointType = EnWayPointType.None;
		switch ((EnStroyDugeonType)m_csDungeonManager.StoryDungeonStep.Type)
		{
			case EnStroyDugeonType.Move:
				enWayPointType = EnWayPointType.Move;
				Player.SetWayPoint(m_csDungeonManager.StoryDungeonStep.TargetPosition, enWayPointType, m_csDungeonManager.StoryDungeonStep.TargetRadius);
				Player.DisplayPath.SetPath(m_csDungeonManager.StoryDungeonStep.TargetPosition);
				break;
			case EnStroyDugeonType.Battle:
				enWayPointType = EnWayPointType.Battle;
				Player.SetWayPoint(m_csDungeonManager.StoryDungeonStep.TargetPosition, enWayPointType, m_csDungeonManager.StoryDungeonStep.TargetRadius);
				Player.DisplayPath.SetPath(m_csDungeonManager.StoryDungeonStep.TargetPosition);
				break;
			case EnStroyDugeonType.Boss:
				enWayPointType = EnWayPointType.Battle;
				Player.SetWayPoint(m_csDungeonManager.StoryDungeonStep.TargetPosition, enWayPointType, m_csDungeonManager.StoryDungeonStep.TargetRadius);
				Player.DisplayPath.SetPath(m_csDungeonManager.StoryDungeonStep.TargetPosition);
				break;
			case EnStroyDugeonType.Tame:
				enWayPointType = EnWayPointType.Interaction;
				if (Player.IsTransformationStateTame() == false)
				{
					Player.SetWayPoint(CsIngameData.Instance.TameMonster.transform.position, enWayPointType, CsIngameData.Instance.TameMonster.TameRadius);
					Player.DisplayPath.SetPath(CsIngameData.Instance.TameMonster.transform.position);
				}
				else
				{
					Player.SetWayPoint(m_csDungeonManager.StoryDungeonStep.TargetPosition, enWayPointType, m_csDungeonManager.StoryDungeonStep.TargetRadius);
					Player.DisplayPath.SetPath(m_csDungeonManager.StoryDungeonStep.TargetPosition);
				}
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlayAutoStoryDungeon()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csDungeonManager.StoryDungeon == null) return;
			if (m_csDungeonManager.StoryDungeonStep == null) return;
			if (CsIngameData.Instance.InGameCamera.CameraPlay == EnCameraPlay.Domination) return;
			if (CsIngameData.Instance.TameMonster != null && CsIngameData.Instance.TameMonster.IsAttack) return; // 테이밍 공격중.

			switch ((EnStroyDugeonType)m_csDungeonManager.StoryDungeonStep.Type)
			{
			case EnStroyDugeonType.Move:
				if (Player.IsStateIdle)
				{
					if (MovePlayer(m_csDungeonManager.StoryDungeonStep.TargetPosition, m_csDungeonManager.StoryDungeonStep.TargetRadius))
					{
						if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
						{
							m_csPlayer.ChangeState(CsHero.EnState.Idle);
						}
					}
				}
				break;
			case EnStroyDugeonType.Battle:
				if (MovePlayer(m_csDungeonManager.StoryDungeonStep.TargetPosition, m_csDungeonManager.StoryDungeonStep.TargetRadius))
				{
					if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
					{
						DugeonBattle(m_csDungeonManager.StoryDungeonStep.TargetPosition, 50f);
					}
				}
				break;
			case EnStroyDugeonType.Boss:
				if (MovePlayer(m_csDungeonManager.StoryDungeonStep.TargetPosition, m_csDungeonManager.StoryDungeonStep.TargetRadius))
				{
					if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
					{
						DugeonBattle(m_csDungeonManager.StoryDungeonStep.TargetPosition, 50f);
					}
				}
				break;
			case EnStroyDugeonType.Tame:
				if (Player.IsTransformationStateTame() == false)
				{
					if (m_csPlayer.IsStateIdle)
					{
						if (MovePlayer(CsIngameData.Instance.TameMonster.transform.position, CsIngameData.Instance.TameMonster.TameRadius)) // 테임 가능 범위 안으로 이동만.
						{
							if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
							{
								m_csPlayer.ChangeState(CsHero.EnState.Idle);
							}
						}
					}
				}
				break;
			}
		}
	}
}
