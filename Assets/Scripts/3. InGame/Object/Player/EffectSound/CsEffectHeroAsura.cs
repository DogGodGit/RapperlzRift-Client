using SimpleDebugLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsEffectHeroAsura : CsEffectHero
{
	//---------------------------------------------------------------------------------------------------
	public static CsEffectHeroAsura Instance
	{
		get { return CsSingleton<CsEffectHeroAsura>.GetInstance(); }
	}

	#region Effect

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_01(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Asura_Attack_01", 0.6f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_02(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Asura_Attack_02", 0.6f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_03(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Asura_Attack_03", 0.7f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill02(CsHero cs, int nInnerOrderIndex) // 차지샷.
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		if (nInnerOrderIndex == 0)
		{
			CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_01_01", 2.7f);
		}
		else if (nInnerOrderIndex == 1)
		{
			CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_01_02", 2f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill03(CsHero cs, int nInnerOrderIndex) // 멀티샷.
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_02", 3f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill04(CsHero cs, int nInnerOrderIndex) // 버프
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_03", 3f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill04_Cast(CsHero cs, int nInnerOrderIndex) // 버프 캐스팅
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_03_Casting", 2.7f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill05(CsHero cs, int nInnerOrderIndex) // 라크 필살기.
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_04", 5f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill05_Cast(CsHero cs, int nInnerOrderIndex) // 라크 필살기.
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Asura_Skill_04_Casting", 3f);
	}

	#endregion Effect


	#region Sound

	//---------------------------------------------------------------------------------------------------
	enum EnSound : int 
	{
		Skill01_01, Skill01_02, Skill01_03, Skill01_04, 
		Skill02, Skill02_Cast, Skill02_Voice,
		Skill03,
		Skill04, Skill04_Voice,
		Skill05, Skill05_Cast, Skill05_CastVoice,
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
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<AudioClip>("Sound/Hero/Sound_PC_02_" + en.ToString());
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
		if (m_dicSound.ContainsKey(en))
		{
			PlaySoundOneShot(auddioSource, m_dicSound[en]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlaySoundSkill01_01 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_01); }
	public void PlaySoundSkill01_02			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_02); }
	public void PlaySoundSkill01_03 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_03); }
	public void PlaySoundSkill01_04 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_04); }
	public void PlaySoundSkill02 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02); }
	public void PlaySoundSkill02_Cast 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_Cast); }
	public void PlaySoundSkill02_Voice		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_Voice); }
	public void PlaySoundSkill03  			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill03); }
	public void PlaySoundSkill04 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill04); }
	public void PlaySoundSkill04_Voice		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill04_Voice); }
	public void PlaySoundSkill05 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05); }
	public void PlaySoundSkill05_Cast 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05_Cast); }
	public void PlaySoundSkill05_CastVoice	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05_CastVoice); }
	public void PlaySoundRun_Left 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Run_Left); }
	public void PlaySoundRun_Right 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Run_Right); }
	public void PlaySoundDamage 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.HIT); }
	public void PlaySoundDead 				(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Die01); }
	public void PlaySoundWalk				(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Walk); }

	#endregion Sound
}