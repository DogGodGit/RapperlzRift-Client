using UnityEngine;
using UnityEngine.EventSystems;

public class CsPanelTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    CsTouchInfo m_csTouchInfo;

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        transform.SetAsFirstSibling();
        m_csTouchInfo = CsTouchInfo.Instance;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_csTouchInfo == null)
        {
            m_csTouchInfo = CsTouchInfo.Instance;
        }
        else
        {
            m_csTouchInfo.Update();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_csTouchInfo.OnPointerDown(eventData);
    }

    //-----------------------------------------------------------------------------------------------------------------
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_csTouchInfo.OnPointerUp(eventData);
    }
}