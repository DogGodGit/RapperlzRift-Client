using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupDungeonStartCount : MonoBehaviour 
{
    Transform m_trImageBackground;

    Image m_imageCount;

    float m_flTime = 0.0f;
    float m_flLimitTime = 0.0f;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trImageBackground = transform.Find("ImageBackground");
        m_imageCount = m_trImageBackground.Find("ImageCount").GetComponent<Image>();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            m_imageCount.gameObject.SetActive(false);

            if (1.0f <= m_flLimitTime - Time.realtimeSinceStartup)
            {
                m_imageCount.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/ico_count_" + (int)(m_flLimitTime - Time.realtimeSinceStartup));
                m_imageCount.SetNativeSize();
            }
            else if (0.0f <= m_flLimitTime - Time.realtimeSinceStartup)
            {
                m_imageCount.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/ico_count_go");
                m_imageCount.SetNativeSize();
            }
            else
            {
                Destroy(gameObject);
            }

            m_imageCount.gameObject.SetActive(true);

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayStartCount(float flLimitTime)
    {
        m_flLimitTime = flLimitTime + Time.realtimeSinceStartup;
        m_imageCount.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/ico_count_" + (int)m_flLimitTime);
        m_imageCount.SetNativeSize();
        m_trImageBackground.gameObject.SetActive(true);
    }
}