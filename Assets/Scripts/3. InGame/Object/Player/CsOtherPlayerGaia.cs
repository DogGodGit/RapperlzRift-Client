using UnityEngine;

public class CsOtherPlayerGaia : CsOtherPlayer
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		Debug.Log("CsOtherPlayerGaia.Start()");
		CsEffectHeroGaia.Instance.LoadSound(this);
		transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
	}

	#region Effect

	//---------------------------------------------------------------------------------------------------
	void OnAnimAttackSkillEffect01_01(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill01_01(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_02(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill01_02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_03(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill01_03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect02(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect03(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill04(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04_Cast(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill04_Cast(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect05(int nInnerOrderIndex) { CsEffectHeroGaia.Instance.PlayEffectAttackSkill05(this, nInnerOrderIndex); }

	#endregion Effect

	#region Sound

	//---------------------------------------------------------------------------------------------------
	public override void NetEventHeroLevelUp(int nLevel, int nMaxHp, int nHp)
	{
		base.NetEventHeroLevelUp(nLevel, nMaxHp, nHp);
		CsEffectHeroGaia.Instance.PlaySoundLevelUp(m_audioSource);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound() { CsEffectHeroGaia.Instance.PlaySoundDead(m_audioSource); }
	void OnAnimInteractionSound() { CsEffectHeroGaia.Instance.PlaySoundInteraction(m_audioSource); }
	void OnAnimInteractionSoundEnd() { m_audioSource.Stop(); }
	void OnAnimDamageSound() { }

	void OnAnimWalk() { CsEffectHeroGaia.Instance.PlaySoundWalk(m_audioSource); }
	void OnAnimLeftRunSound() { CsEffectHeroGaia.Instance.PlaySoundRun_Left(m_audioSource); }
	void OnAnimRightRunSound() { CsEffectHeroGaia.Instance.PlaySoundRun_Right(m_audioSource); }

	void OnAnimSkill01_01Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill01_01(m_audioSource); }
	void OnAnimSkill01_02Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill01_02(m_audioSource); }
	void OnAnimSkill01_03Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill01_03(m_audioSource); }
	void OnAnimSkill01_04Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill01_04(m_audioSource); }
	void OnAnimSkill02Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill02(m_audioSource); }
	void OnAnimSkill02_CastSound() { CsEffectHeroGaia.Instance.PlaySoundSkill02_Cast(m_audioSource); }
	void OnAnimSkill02_MoveSound() { CsEffectHeroGaia.Instance.PlaySoundSkill02_Move(m_audioSource); }
	void OnAnimSkill02_VoiceSound() { CsEffectHeroGaia.Instance.PlaySoundSkill02_Voice(m_audioSource); }
	void OnAnimSkill03Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill03(m_audioSource); }
	void OnAnimSkill04Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill04(m_audioSource); }
	void OnAnimSkill04_VoiceSound() { CsEffectHeroGaia.Instance.PlaySoundSkill04_Voice(m_audioSource); }
	void OnAnimSkill05Sound() { CsEffectHeroGaia.Instance.PlaySoundSkill05(m_audioSource); }
	void OnAnimSkill05_CastSound() { CsEffectHeroGaia.Instance.PlaySoundSkill05_Cast(m_audioSource); }
	void OnAnimSkill05_VoiceSound() { CsEffectHeroGaia.Instance.PlaySoundSkill05_Voice(m_audioSource); }

	#endregion Sound
}