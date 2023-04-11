using System.Collections;
using UnityEngine;


public class CsEffectHero
{
	protected enum EnLoadSound{None, Other, My}; 	
	protected static AudioClip s_aclLevelUp = null;
	protected static AudioClip s_aclInteraction = null;

	protected static bool s_bCommonSoundLoadAsyncStarted = false;

	//---------------------------------------------------------------------------------------------------
	protected void PlaySoundOneShot(AudioSource auddioSource, AudioClip ac)
	{ 
		if (auddioSource != null && ac != null && CsIngameData.Instance.EffectSound)
		{
			auddioSource.PlayOneShot(ac);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public static void LoadCommonSound()
	{
		if (!s_bCommonSoundLoadAsyncStarted)
		{
			s_bCommonSoundLoadAsyncStarted = true;
			CsEffectPoolManager.Instance.StartCoroutine(LoadAsyncCommonSound());
		}
	}

	//---------------------------------------------------------------------------------------------------
	static IEnumerator LoadAsyncCommonSound()
	{
		ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<AudioClip>("Sound/ETC/SFX_Cha_LevelUp");
		yield return req;
		s_aclLevelUp = (AudioClip)req.asset;

		req = CsIngameData.Instance.LoadAssetAsync<AudioClip>("Sound/ETC/SFX_Cha_Interaction");
		yield return req;
		s_aclInteraction = (AudioClip)req.asset;
	}

	//---------------------------------------------------------------------------------------------------
	public void PlaySoundLevelUp		(AudioSource auddioSource) { PlaySoundOneShot(auddioSource, s_aclLevelUp); }
	public void PlaySoundInteraction 	(AudioSource auddioSource) { PlaySoundOneShot(auddioSource, s_aclInteraction); }

	//---------------------------------------------------------------------------------------------------
	static protected bool IsEffectable(bool bMyHero)
	{
		return ((CsIngameData.Instance.EffectEnum == EnOptionEffect.My && bMyHero) || CsIngameData.Instance.EffectEnum == EnOptionEffect.All);
	}
}
