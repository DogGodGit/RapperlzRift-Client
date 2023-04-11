using UnityEngine;

public class CsHeroCloneWitch : CsHeroClone
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		Debug.Log("CsOtherPlayerWitch.Start()");
		CsEffectHeroWitch.Instance.LoadSound(this);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnAnimSkillMove(int nMoveCount) // 스킬 동작중 이동.
	{
		if (m_csSkillStatus.JobSkill.CastingMoveType == 1 && m_csSkillStatus.JobSkill.CastingMoveValue1 == 0 && m_csSkillStatus.JobSkill.CastingMoveValue2 == 0) // 순간이동 처리.
		{
			Debug.Log("OnAnimSkillMove     마녀 순간이동 처리..");
			transform.position = m_vtSkillMovePos;
		}
		else
		{
			transform.LookAt(m_vtTargetPos);
			m_navMeshAgent.speed = m_flSkillMoveSpeed;
			m_navMeshAgent.updateRotation = false;
			m_navMeshAgent.SetDestination(m_vtSkillMovePos);
		}
	}

	#region Effect

	void OnAnimAttackSkillEffect01_01(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill01_01(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_02(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill01_02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_03(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill01_03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect02(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect03(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill04(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04_Cast(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill04_Cast(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect05(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill05(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect05_Ball(int nInnerOrderIndex) { CsEffectHeroWitch.Instance.PlayEffectAttackSkill05_Ball(this, nInnerOrderIndex); }

	#endregion Effect


	#region Sound

	//---------------------------------------------------------------------------------------------------
	protected override void DamgaeSound(int nSkill)
	{
		CsEffectHeroWitch.Instance.PlaySoundDamage(m_audioSource);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound() { CsEffectHeroWitch.Instance.PlaySoundDead(m_audioSource); }
	void OnAnimInteractionSound() { CsEffectHeroWitch.Instance.PlaySoundInteraction(m_audioSource); }
	void OnAnimInteractionSoundEnd() { m_audioSource.Stop(); }
	void OnAnimDamageSound() { }

	void OnAnimWalk() { CsEffectHeroWitch.Instance.PlaySoundWalk(m_audioSource); }
	void OnAnimLeftRunSound() { CsEffectHeroWitch.Instance.PlaySoundRun_Left(m_audioSource); }
	void OnAnimRightRunSound() { CsEffectHeroWitch.Instance.PlaySoundRun_Right(m_audioSource); }

	void OnAnimSkill01_01Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill01_01(m_audioSource); }
	void OnAnimSkill01_02Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill01_02(m_audioSource); }
	void OnAnimSkill01_03Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill01_03(m_audioSource); }
	void OnAnimSkill01_04Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill01_04(m_audioSource); }
	void OnAnimSkill02Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill02(m_audioSource); CsEffectHeroWitch.Instance.PlaySoundSkill02_Voice(m_audioSourceParent); }
	void OnAnimSkill03_CastSound() { CsEffectHeroWitch.Instance.PlaySoundSkill03_Cast(m_audioSource); CsEffectHeroWitch.Instance.PlaySoundSkill03_CastVoice(m_audioSourceParent); }
	void OnAnimSkill03Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill03(m_audioSource); CsEffectHeroWitch.Instance.PlaySoundSkill03_Voice(m_audioSourceParent); }
	void OnAnimSkill04Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill04(m_audioSource); CsEffectHeroWitch.Instance.PlaySoundSkill04_Voice(m_audioSourceParent); }
	void OnAnimSkill05_CastSound() { CsEffectHeroWitch.Instance.PlaySoundSkill05_Cast(m_audioSource); CsEffectHeroWitch.Instance.PlaySoundSkill05_CastVoice(m_audioSourceParent); }
	void OnAnimSkill05Sound() { CsEffectHeroWitch.Instance.PlaySoundSkill05(m_audioSource); CsEffectHeroWitch.Instance.PlaySoundSkill05_Voice(m_audioSourceParent); }

	#endregion Sound
}