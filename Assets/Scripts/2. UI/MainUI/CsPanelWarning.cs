using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPanelWarning : MonoBehaviour 
{
    Transform m_trImageWarning;
	Transform m_trPanelDialogFrame;

    float m_flTime = 0.0f;
    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trImageWarning = transform.Find("Image");
		m_trPanelDialogFrame = transform.parent.Find("PanelDialog/Frame");
    }

    //---------------------------------------------------------------------------------------------------
	void Update () 
    {
        if (m_flTime + 1f < Time.time)
        {
            UpdateWarning();
            m_flTime = Time.time;
        }
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateWarning()
    {
        if (CsIngameData.Instance.MyHeroDead ||
			m_trPanelDialogFrame.gameObject.activeSelf)
        {
            m_trImageWarning.gameObject.SetActive(false);
        }
        else
        {
            float flDeadWarningDisplayHpRate = CsGameConfig.Instance.DeadWarningDisplayHpRate / 10000.0f;
            float flHpRate = (CsGameData.Instance.MyHeroInfo.Hp * 1.0f) / CsGameData.Instance.MyHeroInfo.MaxHp;

            if (flHpRate <= flDeadWarningDisplayHpRate)
            {
                m_trImageWarning.gameObject.SetActive(true);
            }
            else
            {
                m_trImageWarning.gameObject.SetActive(false);
            }
        }
    }
}
