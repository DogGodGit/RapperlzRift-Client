using SimpleDebugLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsEffectHeroDeva : CsEffectHero
{
	public static CsEffectHeroDeva Instance
	{
		get { return CsSingleton<CsEffectHeroDeva>.GetInstance(); }
	}

	#region Effect
	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill00_Aura(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Deva_Attack_00", 3f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_Arrow(CsHero cs, int nInnerOrderIndex)
	{	
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayMoveHitEffect(cs.transform, cs.TargetPos, "Deva_Attack_01", null, 0, true);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill01_03(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		if (nInnerOrderIndex == 0)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.TargetPos, "Deva_Attack_03_01", 2f);
		}
		else if (nInnerOrderIndex == 1)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.TargetPos, "Deva_Attack_03_02", 2f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill02(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.TargetPos, "Deva_Skill_01", 3f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill03(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.TargetPos, "Deva_Skill_02", 10f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill04(CsHero cs, int nInnerOrderIndex)
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Deva_Skill_03", 10f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill04_Cast(CsHero cs, int nInnerOrderIndex) // 버프 캐스팅
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		CsEffectPoolManager.Instance.PlayEffect(true, cs.transform, cs.transform.position, "Deva_Skill_03_Casting", 2.7f);
	}

	//---------------------------------------------------------------------------------------------------
	public void PlayEffectAttackSkill05(CsHero cs, int nInnerOrderIndex, Transform trPivoitHUD) 
	{
		if (!IsEffectable(cs.IsMyHero)) return;

		if (nInnerOrderIndex == 0)
		{
			string str = trPivoitHUD.name;
			string strName = str + " Pelvis/" + str + " Spine/" + str + " Spine1/" + str + " Neck/" + str + " L Clavicle/" + str + " L UpperArm/" + str + " L Forearm/" + str + " L Hand/";
			Transform trParent = trPivoitHUD.Find(strName);
			CsEffectPoolManager.Instance.PlayEffect(true, trParent, cs.transform.position, "Deva_Skill_04_Hand", 10f);
		}
		else if (nInnerOrderIndex == 1)
		{
			CsEffectPoolManager.Instance.PlayEffect(false, cs.transform, cs.transform.position, "Deva_Skill_04_01", 10f);
		}
	}

	#endregion Effect


	#region Sound

	//---------------------------------------------------------------------------------------------------
	enum EnSound : int 
	{
		Skill01_01, Skill01_02, Skill01_03, Skill01_04, 
		Skill02, Skill02_hit, Skill02_Voice,
		Skill03_1, Skill03_2, Skill03_Voice,
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
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<AudioClip>("Sound/Hero/Sound_PC_03_" + en.ToString());
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
	public void PlaySoundSkill01_01 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_01); }
	public void PlaySoundSkill01_02		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_02); }
	public void PlaySoundSkill01_03 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_03); }
	public void PlaySoundSkill01_04 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill01_04); }
	public void PlaySoundSkill02 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02); }
	public void PlaySoundSkill02_hit 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_hit); }
	public void PlaySoundSkill02_Voice 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill02_Voice); }
	public void PlaySoundSkill03_1  	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill03_1); }
	public void PlaySoundSkill03_2  	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill03_2); }
	public void PlaySoundSkill03_Voice  (AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill03_Voice); }
	public void PlaySoundSkill04 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill04); }
	public void PlaySoundSkill04_Voice 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill04_Voice); }
	public void PlaySoundSkill05 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05); }
	public void PlaySoundSkill05_Cast 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05_Cast); }
	public void PlaySoundSkill05_Voice 	(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Skill05_Voice); }
	public void PlaySoundRun_Left 		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Run_Left); }
	public void PlaySoundRun_Right		(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Run_Right); }
	public void PlaySoundDamage			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.HIT); }
	public void PlaySoundDead 			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Die01); }
	public void PlaySoundWalk			(AudioSource audioSource) { PlaySoundOneShot(audioSource, EnSound.Walk); }
	#endregion Sound
}