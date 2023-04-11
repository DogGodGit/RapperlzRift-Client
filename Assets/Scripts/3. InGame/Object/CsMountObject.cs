using UnityEngine;

public enum EnMountState { Idle = 0, Walk, Run }

public class CsMountObject : MonoBehaviour
{
	enum EnMountAnimStatus { Idle = 0, Walk, Run }
	static int s_nAnimatorHash_status = Animator.StringToHash("status");
	static AudioClip s_aclWalk_Left;
	static AudioClip s_aclWalk_Right;
	static AudioClip s_aclRun;
	static AudioClip s_aclWaterRun;

	AudioSource m_audioSource;
	Animator m_animator = null;
	bool bOnWater = false;

	//---------------------------------------------------------------------------------------------------
	public void Init(CsMount csMount, Transform trParent, EnJob enJob)
	{
		Debug.Log("CsMountObject.Init() : "+ enJob);

		if (enJob == EnJob.Gaia)
		{
			transform.localScale = new Vector3(1.1f, 1.1f, 1.1f); 
		}
		else if (enJob == EnJob.Asura)
		{
			transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
		else if (enJob == EnJob.Deva)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (enJob == EnJob.Witch)
		{
			transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		
		gameObject.tag = transform.tag;

		Transform[] atrArtifact = gameObject.GetComponentsInChildren<Transform>();
		int nLayer = transform.gameObject.layer;

		for (int i = 0; i < atrArtifact.Length; ++i)
		{
			atrArtifact[i].gameObject.layer = nLayer;
		}

		transform.position = trParent.position;
		transform.eulerAngles = trParent.eulerAngles;

		transform.SetParent(transform);
	}

	//---------------------------------------------------------------------------------------------------
	void Awake () 
	{
		m_animator = transform.GetComponent<Animator>();
		m_audioSource = transform.GetComponent<AudioSource>();
		m_audioSource.volume = 0.4f;
	}

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		s_aclWalk_Right = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_foot_slow_01");
		s_aclWalk_Left = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_foot_slow_02");
		s_aclRun = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_horse_foot_04");
		s_aclWaterRun = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_water_foot_twice_02");
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnMountState enNewMountState)
	{
		//Debug.Log("CsMountObject.ChangeState     EnMountState = " + enNewMountState);
		if (enNewMountState == EnMountState.Idle)
		{
			SetAnimStatus(EnMountAnimStatus.Idle);
		}
		else if (enNewMountState == EnMountState.Walk)
		{
			SetAnimStatus(EnMountAnimStatus.Walk);
		}
		else if (enNewMountState == EnMountState.Run)
		{
			SetAnimStatus(EnMountAnimStatus.Run);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnMountAnimStatus enAnimStatus)
	{
		if (enAnimStatus == (EnMountAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status)) return;
		m_animator.SetInteger(s_nAnimatorHash_status, (int)enAnimStatus);
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
			if (bOnWater)
			{
				m_audioSource.PlayOneShot(s_aclWaterRun);
			}
			else
			{
				m_audioSource.PlayOneShot(s_aclRun);
			}
		}
	}
}
