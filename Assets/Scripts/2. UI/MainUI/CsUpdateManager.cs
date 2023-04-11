using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-21)
//---------------------------------------------------------------------------------------------------

public class CsUpdateManager : MonoBehaviour
{
    private static CsUpdateManager s_instance = null;

    public static CsUpdateManager Instance
    {
        get { return s_instance; }
    }

    List<IUpdateable> m_listIUpdateable = new List<IUpdateable>();

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        float flTime = Time.deltaTime;

        for (int i = 0; i < m_listIUpdateable.Count; i++)
        {
            m_listIUpdateable[i].OnUpdate(flTime);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void RegisterUpdateableObject(IUpdateable obj)
    {
        if (!m_listIUpdateable.Contains(obj))
            m_listIUpdateable.Add(obj);
    }

    //---------------------------------------------------------------------------------------------------
    public void DeregisterUpdateableObject(IUpdateable obj)
    {
        if (m_listIUpdateable.Contains(obj))
            m_listIUpdateable.Remove(obj);
    }
}

