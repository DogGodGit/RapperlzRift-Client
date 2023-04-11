using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsPopupSub : CsUpdateableMonoBehaviour
{
    protected IPopupMain m_iPopupMain;
    //---------------------------------------------------------------------------------------------------
    public IPopupMain PopupMain
    {
        set { m_iPopupMain = value; }
    }
}
