using SimpleDebugLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsEffectHeroGaia : CsEffectHero
{
	public static CsEffectHeroGaia Instance
	{
		get { return CsSingleton<CsEffectHeroGaia>.GetInstance(); }
	}

	#region Effect
	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_01(CsHero cs, int nInnerOrderIndex)
	{	
		if (!IsEffectable(cs.IsMyHero)) return;
			
		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Gaia_Attack_01", 0.8f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_02(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Gaia_Attack_02", 0.8f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_03(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		if (nInnerOrderIndex == 0)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Gaia_Attack_03_01", 1f);
		}
		else if (nInnerOrderIndex == 1)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Gaia_Attack_03_02", 1f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill02(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		if (nInnerOrderIndex == 0)
		{
			CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Gaia_Skill_01_01", 1.5f);
		}
		else if (nInnerOrderIndex == 1)
		{
			CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Gaia_Skill_01_02", 1.5f);
		}
		else if (nInnerOrderIndex == 2)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Gaia_Skill_01_03", 1.5f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill03(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Gaia_Skill_02", 2.7f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill04(CsHero cs, int nInnerOrderIndex) // 버프
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Gaia_Skill_03", 5f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill04_Cast(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Gaia_Skill_03_Casting", 2.7f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill05(CsHero cs, int nInnerOrderIndex) // 라크 필살기.
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Gaia_Skill_04", 3f);
	}

	#endregion Effect


	#region Sound

	//---------------------------------------------------------------------------------------------------
	enum EnSound : int 
	{
		Skill01_01, Skill01_02, Skill01_03, Skill01_04, 
		Skill02, Skill02_Cast, Skill02_Move, Skill02_Voice,
		Skill03,
		Skill04, Skill04_Voice,
		Skill05, Skill05_Cast, Skill05_Voice,
		Run_Left, Run_Right,
		HIT, Die01, Walk
	}

	Dictionary<EnSound, AudioClip> m_dicSound = new Dictionary<EnSound, AudioClip>();
	EnLoadSound m_enLoadSound = EnLoadSound.None;

	//---------------------------------------------------------------------------------------------------
	public void LoadSound(CsHero cs)
	{
		LoadCommonSound();

		EnLoadSound en = cs.IsMyHero ? EnLoadSound.My : EnLoadSound.Other;

		if (en <= m_enLoadSound) return;
		m_enLoadSound = en;

		CsEffectPoolManager.Instance.StartCoroutine(LoadAsyncSound());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadAsyncSound()
	{
		dd.d("LoadAsyncSound");
		int nFirst = (m_enLoadSound == EnLoadSound.My) ? 0 : (int)EnSound.Die01;
		int nCount = Enum.GetValues(typeof(EnSound)).Length;
		for (int i = nFirst; i < nCount; i++)
		{
			EnSound en = (EnSound)i;
			if (!m_dicSound.ContainsKey(en))
			{
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<AudioClip>("Sound/Hero/Sound_PC_01_" + en.ToString());
				yield return req;

				if (req == null)
				{
					dd.d("LoadAsyncSound 1");
				}
				else
				{
					if (req.asset == null)
					{
						dd.d("LoadAsyncSound 2");
					}
				}

				m_dicSound.Add(en, (AudioClip)req.asset);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlaySoundOneShot(AudioSource auddioSource, EnSound en)
	{
		if (CsIngameData.Instance.EffectSound)
		{
			if (m_dicSound.ContainsKey(en))
			{
				PlaySoundOneShot(auddioSource, m_dicSound[en]);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlaySoundSkill01_01 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_01); }
	public void PlaySoundSkill01_02		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_02); }
	public void PlaySoundSkill01_03 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_03); }
	public void PlaySoundSkill01_04 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_04); }
	public void PlaySoundSkill02 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02); }
	public void PlaySoundSkill02_Cast 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_Cast); }
	public void PlaySoundSkill02_Move 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_Move); }
	public void PlaySoundSkill02_Voice 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_Voice); }
	public void PlaySoundSkill03  		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill03); }
	public void PlaySoundSkill04 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill04); }
	public void PlaySoundSkill04_Voice 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill04_Voice); }
	public void PlaySoundSkill05 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05); }
	public void PlaySoundSkill05_Cast 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05_Cast); }
	public void PlaySoundSkill05_Voice 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05_Voice); }
	public void PlaySoundRun_Left 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Run_Left); }
	public void PlaySoundRun_Right 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Run_Right); }
	public void PlaySoundDamage 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.HIT); }
	public void PlaySoundDead 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Die01); }
	public void PlaySoundWalk			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Walk); }
	
	#endregion Sound
}