using UnityEngine;

public class CsPlayerAsura : CsMyPlayer
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		Debug.Log("CsPlayerAsura.Start()");

		CsEffectHeroAsura.Instance.LoadSound(this);
	}

	//---------------------------------------------------------------------------------------------------
	protected override Transform FindHudPivot()
	{
		return transform.Find("Bip01");
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnAnimSkillMove(int nMoveCount) // 스킬 동작중 이동.
	{
		if (nMoveCount == 1) // Asura skill 5번스킬 이중 이동시 사용.
		{
			SetSkillCastingFixedMoveTypeData(null, true);
		}
		base.OnAnimSkillMove(nMoveCount);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		base.OnEventAnimStartAttack(asi, nKey);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimCameraActionStart()
	{
		//CsIngameData.Instance.InGameCamera.DoAiming(true, 0.5f, 2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventAnimCameraActionEnd()
	{
		//CsIngameData.Instance.InGameCamera.DoAiming(false,0,0);
	}

	#region Effect

	//---------------------------------------------------------------------------------------------------
	void OnAnimAttackSkillEffect01_01(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill01_01(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_02(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill01_02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect01_03(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill01_03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect02(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill02(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect03(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill03(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04_Cast(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill04_Cast(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect04(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill04(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect05_Cast(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill05_Cast(this, nInnerOrderIndex); }
	void OnAnimAttackSkillEffect05(int nInnerOrderIndex) { CsEffectHeroAsura.Instance.PlayEffectAttackSkill05(this, nInnerOrderIndex); }

	#endregion Effect


	#region Sound

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventMyHeroLevelUp()
	{
		base.OnEventMyHeroLevelUp();
		CsEffectHeroAsura.Instance.PlaySoundLevelUp(m_audioSourceParent);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void DamgaeSound(int nSkill)
	{
		CsEffectHeroAsura.Instance.PlaySoundDamage(m_audioSourceParent); 
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDeadSound()			{ CsEffectHeroAsura.Instance.PlaySoundDead(m_audioSource); }
	void OnAnimInteractionSound() 	{ CsEffectHeroAsura.Instance.PlaySoundInteraction(m_audioSource); }
	void OnAnimInteractionSoundEnd(){ m_audioSource.Stop(); }

	void OnAnimWalk()				{ CsEffectHeroAsura.Instance.PlaySoundWalk(m_audioSource); }
	void OnAnimLeftRunSound() 		{ CsEffectHeroAsura.Instance.PlaySoundRun_Left(m_audioSource); }
	void OnAnimRightRunSound() 		{ CsEffectHeroAsura.Instance.PlaySoundRun_Right(m_audioSource); }

	void OnAnimDamageSound()		{  }
	void OnAnimSkill01_01Sound() 	{ CsEffectHeroAsura.Instance.PlaySoundSkill01_01(m_audioSource); }
	void OnAnimSkill01_02Sound()	{ CsEffectHeroAsura.Instance.PlaySoundSkill01_02(m_audioSource); }
	void OnAnimSkill01_03Sound()	{ CsEffectHeroAsura.Instance.PlaySoundSkill01_03(m_audioSource); }
	void OnAnimSkill01_04Sound() 	{ CsEffectHeroAsura.Instance.PlaySoundSkill01_04(m_audioSource); }
	void OnAnimSkill02_CastSound()	{ CsEffectHeroAsura.Instance.PlaySoundSkill02_Cast(m_audioSource); CsEffectHeroAsura.Instance.PlaySoundSkill02_Voice(m_audioSourceParent); }
	void OnAnimSkill02Sound() 		{ CsEffectHeroAsura.Instance.PlaySoundSkill02(m_audioSource); }
	void OnAnimSkill03Sound() 		{ CsEffectHeroAsura.Instance.PlaySoundSkill03(m_audioSource); }
	void OnAnimSkill04Sound() 		{ CsEffectHeroAsura.Instance.PlaySoundSkill04(m_audioSource); CsEffectHeroAsura.Instance.PlaySoundSkill04_Voice(m_audioSourceParent); }
	void OnAnimSkill05_CastSound()	{ CsEffectHeroAsura.Instance.PlaySoundSkill05_Cast(m_audioSource); CsEffectHeroAsura.Instance.PlaySoundSkill05_CastVoice(m_audioSourceParent); }
	void OnAnimSkill05Sound()		{ CsEffectHeroAsura.Instance.PlaySoundSkill05(m_audioSource); }

	#endregion Sound
}
