using UnityEngine;
using UnityEngine.Playables;

public class CsCinemachineSceneFirstEnter : MonoBehaviour
{
	PlayableDirector m_playableDirector = null;
	float m_flDelayCheckTime;

    //---------------------------------------------------------------------------------------------------
    public void Init()
    {
		Debug.Log("CsCinemachineSceneFirstEnter.Init()");
        m_playableDirector = transform.GetComponent<PlayableDirector>();
		m_playableDirector.Play();
    }

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_playableDirector == null) return;

		if (m_flDelayCheckTime > 3)
		{
			if (m_playableDirector.state == PlayState.Paused)
			{
				CsIngameData.Instance.IngameManagement.DirectingEnd(false);
				CsIngameData.Instance.InGameCamera.ResetFarClipPlane();
				Destroy(gameObject);
			}
		}
		m_flDelayCheckTime += Time.deltaTime;
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		m_playableDirector = null;
	}
}
