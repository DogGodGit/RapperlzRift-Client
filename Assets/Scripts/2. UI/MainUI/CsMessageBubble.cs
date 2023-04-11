using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-11)
//---------------------------------------------------------------------------------------------------

public class CsMessageBubble : MonoBehaviour
{
    float m_flRemainSec;
    float m_flTime = 0;

    void OnEnable()
    {
        m_flRemainSec = CsGameConfig.Instance.ChattingBubbleDisplayDuration;
    }

    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_flRemainSec > 0)
            {
                m_flRemainSec--;

                if (m_flRemainSec == 0)
                {
                    transform.gameObject.SetActive(false);
                }
            }

            m_flTime = Time.time;
        }
    }
}
