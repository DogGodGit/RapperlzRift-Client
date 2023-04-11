using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-21)
//---------------------------------------------------------------------------------------------------

public class CsUpdateableMonoBehaviour : MonoBehaviour, IUpdateable
{
    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        CsUpdateManager.Instance.RegisterUpdateableObject(this);
        Initialize();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsUpdateManager.Instance.DeregisterUpdateableObject(this);
        OnFinalize();
    }

    //---------------------------------------------------------------------------------------------------
    public virtual void OnUpdate(float flTime) { }

    //---------------------------------------------------------------------------------------------------
    protected virtual void Initialize() { }

    //---------------------------------------------------------------------------------------------------
    protected virtual void OnFinalize() { }
}

public interface IUpdateable
{
    void OnUpdate(float flTime);
}
