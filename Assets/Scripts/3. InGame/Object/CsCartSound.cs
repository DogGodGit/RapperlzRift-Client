using UnityEngine;

public class CsCartSound : MonoBehaviour 
{
	static AudioClip s_aclWalk_Left;
	static AudioClip s_aclWalk_Right;
	static AudioClip s_aclRun;

	AudioSource m_audioSource;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_audioSource = transform.GetComponent<AudioSource>();
		if (m_audioSource != null)
		{
			m_audioSource.volume = 0.2f;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Start () 
	{
		s_aclWalk_Right = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_foot_slow_01");
		s_aclWalk_Left = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_foot_slow_02");
		s_aclRun = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_foot_04");
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRightWalk()
	{
		if (CsIngameData.Instance.EffectSound)
		{
			m_audioSource.PlayOneShot(s_aclWalk_Right);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimLeftWalk()
	{
		if (CsIngameData.Instance.EffectSound)
		{
			m_audioSource.PlayOneShot(s_aclWalk_Left);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRun()
	{
		if (CsIngameData.Instance.EffectSound)
		{
			m_audioSource.PlayOneShot(s_aclRun);
		}
	}
}
