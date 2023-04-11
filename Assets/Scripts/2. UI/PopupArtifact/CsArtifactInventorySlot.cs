using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//-------------------------------------------------------------------------------------------------------
//작성: 최민수 (2018-10-02)
//-------------------------------------------------------------------------------------------------------

public class CsArtifactInventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	bool m_bIsPointerDown;
	bool m_bIsSelected;

	float m_flPointerDownTimer;
	float m_flDurationThreshold = 1.0f;

	GameObject m_goSelectedObject;

	public bool IsSelected
	{
		get { return m_bIsSelected; }
		set { m_bIsSelected = value; }
	}

	//---------------------------------------------------------------------------------------------------
	void Start () 
	{
		m_goSelectedObject = null;
		m_bIsSelected = false;
	}

	//---------------------------------------------------------------------------------------------------
	void Update () 
	{
		if (m_bIsPointerDown)
		{
			m_flPointerDownTimer += Time.deltaTime;
			if (m_flPointerDownTimer >= m_flDurationThreshold)
			{
				ResetTimer();
				int nInventoryIndex = 0;

				if (int.TryParse(m_goSelectedObject.name, out nInventoryIndex))
				{
					CsGameEventUIToUI.Instance.OnEventInventoryLongClick(nInventoryIndex);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnPointerDown(PointerEventData eventData)
	{
		m_goSelectedObject = eventData.selectedObject;
		m_bIsPointerDown = true;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnPointerUp(PointerEventData eventData)
	{
		ResetTimer();
	}

	//---------------------------------------------------------------------------------------------------
	void ResetTimer()
	{
		m_bIsPointerDown = false;
		m_flPointerDownTimer = 0.0f;
	}
}
