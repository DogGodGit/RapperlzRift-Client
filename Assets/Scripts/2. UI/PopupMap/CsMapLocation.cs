using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsMapLocation : MonoBehaviour, IPointerClickHandler
{
    Camera m_UICamera;
    RectTransform m_rectTransform;

    CsMapArea m_csMapArea;

    int m_nLocationId;
    int m_nNationId;
    float m_flMinimapMagnification;
    float m_flX;
    float m_flZ;

    public CsMapArea MapArea { set { m_csMapArea = value; } }

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        m_rectTransform = transform.GetComponent<RectTransform>();
    }

    //---------------------------------------------------------------------------------------------------
    public void SettingValue(int nLocationId, int nNationId, float flMinimapMagnfication, float flX, float flZ)
    {
        m_nLocationId = nLocationId;
        m_nNationId = nNationId;
        m_flMinimapMagnification = flMinimapMagnfication;
        m_flX = flX;
        m_flZ = flZ;
    }

    //---------------------------------------------------------------------------------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 vtScreenPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, eventData.position, m_UICamera, out vtScreenPoint))
        {
            if(CsGameData.Instance.MyHeroInfo.LocationId == 201 && m_nLocationId != 201)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_03001"));
                return;
            }

            Vector3 vtPos = new Vector3(((vtScreenPoint.x + m_rectTransform.rect.width / 2f) / m_flMinimapMagnification) + m_flX, 100, ((vtScreenPoint.y + m_rectTransform.rect.height / 2f) / m_flMinimapMagnification) + m_flZ);

            if (CsGameEventToIngame.Instance.OnEventMapMove(m_nLocationId, m_nNationId, vtPos))
            {
                m_csMapArea.AllPathClear();
                CsUIData.Instance.AutoStateType = EnAutoStateType.Move;
                CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Move);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A29_TXT_02001"));
            }
        }
    }
}

