using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CsIntroCharacterTimeline : MonoBehaviour 
{
    Transform m_trCharacter;
	Transform m_trCharacterCamera;
	Animator m_animatorCamera;
	PlayableDirector m_playableDirector;
    TimelineAsset m_timelineAsset;
	GameObject m_goEffect;

	AudioClip m_audioClip = null;
	AudioSource m_audioSource;

	int m_nJobId = 0;
	float m_flEulerAnglesY;
	bool m_bFinish = false;

	public bool IsPlaying() { return m_playableDirector.state == PlayState.Playing; }
    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
		m_trCharacter = transform.Find("Character");
		m_flEulerAnglesY = m_trCharacter.eulerAngles.y;
		m_trCharacterCamera = transform.Find("IntroCamera/CharacterCamera");
		m_animatorCamera = m_trCharacterCamera.GetComponent<Animator>();
		m_playableDirector = transform.GetComponent<PlayableDirector>();
		m_audioSource = transform.GetComponent<AudioSource>();
		var lookComponent = m_trCharacter.GetComponent<CsHeadLookPlayer>();
		if (lookComponent != null) lookComponent.enabled = false;
		m_animatorCamera.enabled = false;
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_playableDirector == null || m_timelineAsset == null) return;
		if (m_playableDirector.time >= m_timelineAsset.duration || m_playableDirector.state == PlayState.Paused)
		{
			if (m_bFinish == false)
			{
				m_bFinish = true;
				TimeLineFinish();
			}
		}
		else  // 플레이 중일때.
		{
			if (m_bSelboolIntro == false)
			{
				m_bSelboolIntro = m_trCharacter.GetComponent<Animator>().GetBool("Intro");
				m_trCharacter.GetComponent<Animator>().SetBool("Intro", true);
			}

			m_trCharacter.eulerAngles = new Vector3(0f, m_flEulerAnglesY, 0f);
		}
	}

    //---------------------------------------------------------------------------------------------------
	public void TimeLineCreateStart(int nJobId)
	{
		Debug.Log("CsIntroCharacterTimeline     TimeLineCreateStart ???   : " + nJobId);
		TimeLineClear();
       // TimeLineStart(nJobId);
       // TimeLineFinish();

        string strTimeline = "";
		string strAudioClip = "";

		if (nJobId == 1)
		{
			strTimeline = "Prefab/Select/TimeLine/GaiaCreateTimeline";
			strAudioClip = "Sound/CharacterSelect/Gaia_select";
          //  TimeLineStart(nJobId);

        }
		else if (nJobId == 2)
		{
			strTimeline = "Prefab/Select/TimeLine/AsuraCreateTimeline";
			strAudioClip = "Sound/CharacterSelect/Asura_select";
		}
		else if (nJobId == 3)
		{
			strTimeline = "Prefab/Select/TimeLine/DevaCreateTimeline";
			strAudioClip = "Sound/CharacterSelect/Deva_select";
		}
		else if (nJobId == 4)
		{
			strTimeline = "Prefab/Select/TimeLine/WitchCreateTimeline";
			strAudioClip = "Sound/CharacterSelect/Witch_select";
		}

		m_timelineAsset = CsIngameData.Instance.LoadAsset<TimelineAsset>(strTimeline);
		m_audioClip = CsIngameData.Instance.LoadAsset<AudioClip>(strAudioClip);

		m_playableDirector.extrapolationMode = DirectorWrapMode.Hold;

		TimeLineStart(nJobId);
	}

    //---------------------------------------------------------------------------------------------------
    public void TimeLineSelectStart(int nJobId)
	{
		Debug.Log("CsIntroCharacterTimeline     TimeLineSelectStart    : "+ nJobId);
		TimeLineClear();
		string strTimeline = "";
		string strAudioClip = "";

		if (nJobId == 1)
        {
			strTimeline = "Prefab/Select/TimeLine/GaiaSelectTimeline";
			strAudioClip = "Sound/CharacterSelect/Gaia_select";
      
        }
        else if (nJobId == 2)
		{
			strTimeline = "Prefab/Select/TimeLine/AsuraSelectTimeline";
			strAudioClip = "Sound/CharacterSelect/Asura_select";
        }
        else if (nJobId == 3)
		{
			strTimeline = "Prefab/Select/TimeLine/DevaSelectTimeline";
			strAudioClip = "Sound/CharacterSelect/Deva_select";
        }
        else if (nJobId == 4)
		{
			strTimeline = "Prefab/Select/TimeLine/WitchSelectTimeline";
			strAudioClip = "Sound/CharacterSelect/Witch_select";
        }

		m_timelineAsset = CsIngameData.Instance.LoadAsset<TimelineAsset>(strTimeline) as TimelineAsset;
		m_audioClip = CsIngameData.Instance.LoadAsset<AudioClip>(strAudioClip);

		m_playableDirector.extrapolationMode = DirectorWrapMode.None;

		TimeLineStart(nJobId);
    }

	bool m_bSelboolIntro = false;
    //---------------------------------------------------------------------------------------------------
    void TimeLineStart(int nJobId)
	{
		//Debug.Log("######################               TimeLineStart      스타트~~~~~~~~~~~~~~~~~~~~~~~~~~~ = " + m_trCharacter.GetComponent<Animator>());
		m_nJobId = nJobId;
		m_trCharacter.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        m_playableDirector.SetGenericBinding(m_timelineAsset.GetOutputTrack(0), m_trCharacter.gameObject);
        m_playableDirector.SetGenericBinding(m_timelineAsset.GetOutputTrack(1), m_trCharacter.gameObject);
        m_playableDirector.SetGenericBinding(m_timelineAsset.GetOutputTrack(2), transform.Find("IntroCamera/CharacterCamera").gameObject);

		if (m_audioClip != null)
		{
			StartCoroutine(PlayCreateCharacterSound(nJobId));
		}
        transform.GetComponent<PlayableDirector>().Play(m_timelineAsset);
		m_animatorCamera.enabled = true;
		m_animatorCamera.SetBool("Create", false);
		m_bFinish = false;

		m_trCharacter.GetComponent<Animator>().SetBool("Intro", true);
		m_bSelboolIntro = m_trCharacter.GetComponent<Animator>().GetBool("Intro");
    }

	//---------------------------------------------------------------------------------------------------
	void TimeLineFinish()
	{
		//Debug.Log("TimeLineFinish()   m_playableDirector.extrapolationMode =" + m_playableDirector.extrapolationMode);
		if (m_playableDirector.extrapolationMode == DirectorWrapMode.None)
		{
			if (m_trCharacter != null)
			{
				var collider = m_trCharacter.GetComponent<CapsuleCollider>();
				if (collider != null)
					collider.enabled = true;
				m_trCharacter.GetComponent<Animator>().SetBool("Select", true);
			}
			m_animatorCamera.enabled = false;
		}
		else
		{
			m_animatorCamera.SetBool("Create", true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void TimeLineClear()
	{
        m_trCharacter.GetComponent<Animator>().SetBool("Select", false);
		if (m_goEffect != null)
		{
			Destroy(m_goEffect);
		}
		var colider = m_trCharacter.GetComponent<CapsuleCollider>();
		if (colider != null) colider.enabled = false;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator PlayCreateCharacterSound(int nJobId)
	{
		if (nJobId == 1)
		{
			yield return new WaitForSeconds(0.63f);
		}

		if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.EffectSound) == 1)
		{
			m_audioSource.PlayOneShot(m_audioClip);
		}

		yield return new WaitUntil(() => (m_playableDirector.time > m_timelineAsset.GetOutputTrack(2).end));
		m_goEffect = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Select/Effect/CharSelect_BackFX"), m_trCharacter) as GameObject;
	}
}
