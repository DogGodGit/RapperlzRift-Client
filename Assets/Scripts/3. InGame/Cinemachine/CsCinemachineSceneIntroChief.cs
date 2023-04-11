using UnityEngine;
using UnityEngine.Playables;

public class CsCinemachineSceneIntroChief : MonoBehaviour
{
	PlayableDirector m_playableDirector = null;
	bool m_bTest = false;

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		m_playableDirector = this.transform.GetComponent<PlayableDirector>();
		m_playableDirector.Play();        
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_playableDirector == null) return;
		if (m_playableDirector.state == PlayState.Paused)
		{
			CsIngameData.Instance.InGameCamera.ResetFarClipPlane();
			GameObject.Destroy(gameObject);
		}
		else
		{
			if (CsIngameData.Instance.Directing && m_bTest == false && m_playableDirector.time > m_playableDirector.duration - 0.6f) // Fade 시간이 0.6초
			{
				m_bTest = true;
				CsIngameData.Instance.IngameManagement.DirectingEnd(true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		m_playableDirector = null;
	}
}
