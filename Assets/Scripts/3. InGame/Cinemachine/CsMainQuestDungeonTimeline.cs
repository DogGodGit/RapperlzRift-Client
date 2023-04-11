using UnityEngine;
using UnityEngine.Playables;

public class CsMainQuestDungeonTimeline : MonoBehaviour 
{
    CsSceneDun01_Quest1 m_csSceneDun01_Quest1;
	int m_nShakeCount = 0;

    //---------------------------------------------------------------------------------------------------
	void Awake () 
    {
        m_csSceneDun01_Quest1 = this.transform.parent.GetComponent<CsSceneDun01_Quest1>();
	}

    //---------------------------------------------------------------------------------------------------
	void Update ()
	{
        if (this.transform.GetComponent<PlayableDirector>().state == PlayState.Paused)
        {
            if (m_csSceneDun01_Quest1.IsPlayTimeline == true)
            {
                m_csSceneDun01_Quest1.EffectTaxian.transform.Find("Mesh_posui03").GetComponent<Animation>().clip = null;
                m_csSceneDun01_Quest1.EffectTaxian.transform.Find("Mesh_posui_neiquan01").GetComponent<Animation>().clip = null;
                m_csSceneDun01_Quest1.EffectTaxian.transform.Find("MEsh_posui_003").GetComponent<Animation>().clip = null;
                m_csSceneDun01_Quest1.IsPlayTimeline = false;
				GameObject.Destroy(gameObject);
            }
        }

		if (m_csSceneDun01_Quest1.Director == null) return;

		if (m_csSceneDun01_Quest1.Director.time > 2 && m_nShakeCount == 0)
		{
			CsIngameData.Instance.InGameCamera.DoShake(11, false);
			m_nShakeCount = 1;
		}
		else if (m_csSceneDun01_Quest1.Director.time > 6 && m_nShakeCount ==1)
		{
			CsIngameData.Instance.InGameCamera.DoShake(10, false);
			m_nShakeCount = 2;
		}
		else if (m_csSceneDun01_Quest1.Director.time + 2f > m_csSceneDun01_Quest1.Director.duration && m_nShakeCount == 2)
		{
			CsIngameData.Instance.InGameCamera.ChangeCamera(false, false, true, 0, 0, 1.0f, 1f);
			m_nShakeCount = 3;
		}
	}
}
