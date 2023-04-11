using UnityEngine;

public class CsOtherPlayerDeva : CsOtherPlayer
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		Debug.Log("CsOtherPlayerDeva.Start()");
		CsEffectHeroDeva.Instance.LoadSound(this);
		transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
	}

	#region Effect

	//---------------------------------------------------------------------------------------------------
	void OnAnimAttackSkillEffect00_Aura(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill00_Aura(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_Arrow(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill01_Arrow(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_01(int nInnerOrderIndex) { }
	void OnAnimAttackSkillEffect01_02(int nInnerOrderIndex) { }
	void OnAnimAttackSkillEffect01_03(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill01_03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect02(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect03(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill04(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04_Cast(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill04_Cast(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect05(int nInnerOrderIndex) { CsEffectHeroDeva.Instance.PlayEffectAttackSkill05(this, nInnerOrderIndex, m_trPivotHUD); }

	#endregion Effect


	#region Sound

	//---------------------------------------------------------------------------------------------------
	public override void NetEventHeroLevelUp(int nLevel, int nMaxHp, int nHp)
	{
		base.NetEventHeroLevelUp(nLevel, nMaxHp, nHp);
		CsEffectHeroDeva.Instance.PlaySoundLevelUp(m_audioSource);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound() { CsEffectHeroDeva.Instance.PlaySoundDead(m_audioSource); }
	void OnAnimInteractionSound() { CsEffectHeroDeva.Instance.PlaySoundInteraction(m_audioSource); }
	void OnAnimInteractionSoundEnd() { m_audioSource.Stop(); }
	void OnAnimDamageSound() { }

	void OnAnimWalk() { CsEffectHeroDeva.Instance.PlaySoundWalk(m_audioSource); }
	void OnAnimLeftRunSound() { CsEffectHeroDeva.Instance.PlaySoundRun_Left(m_audioSource); }
	void OnAnimRightRunSound() { CsEffectHeroDeva.Instance.PlaySoundRun_Right(m_audioSource); }

	void OnAnimSkill01_01Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill01_01(m_audioSource); }
	void OnAnimSkill01_02Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill01_02(m_audioSource); }
	void OnAnimSkill01_03Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill01_03(m_audioSource); }
	void OnAnimSkill01_04Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill01_04(m_audioSource); }
	void OnAnimSkill02Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill02(m_audioSource); }
	void OnAnimSkill02_HitSound() { CsEffectHeroDeva.Instance.PlaySoundSkill02_hit(m_audioSource); }
	void OnAnimSkill02_VoiceSound() { CsEffectHeroDeva.Instance.PlaySoundSkill02_Voice(m_audioSource); }
	void OnAnimSkill03_01Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill03_1(m_audioSource); }
	void OnAnimSkill03_02Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill03_2(m_audioSource); }
	void OnAnimSkill03_VoiceSound() { CsEffectHeroDeva.Instance.PlaySoundSkill03_Voice(m_audioSource); }
	void OnAnimSkill04Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill04(m_audioSource); }
	void OnAnimSkill04_VoiceSound() { CsEffectHeroDeva.Instance.PlaySoundSkill04_Voice(m_audioSource); }
	void OnAnimSkill05Sound() { CsEffectHeroDeva.Instance.PlaySoundSkill05(m_audioSource); }
	void OnAnimSkill05_CastSound() { CsEffectHeroDeva.Instance.PlaySoundSkill05_Cast(m_audioSource); }
	void OnAnimSkill05_VoiceSound() { CsEffectHeroDeva.Instance.PlaySoundSkill05_Voice(m_audioSource); }

	#endregion Sound
}
