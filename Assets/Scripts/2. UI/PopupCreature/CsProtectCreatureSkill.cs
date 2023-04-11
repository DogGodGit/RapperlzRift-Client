using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-17)
//---------------------------------------------------------------------------------------------------

public class CsProtectCreatureSkill : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

	//---------------------------------------------------------------------------------------------------
	void IPointerDownHandler.OnPointerDown(PointerEventData pointerEventData)
	{
		int nSlotId = 0;

		if (int.TryParse(pointerEventData.selectedObject.name, out nSlotId))
		{
			CsGameEventUIToUI.Instance.OnEventPointerDownCreatureSkill(nSlotId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void IPointerUpHandler.OnPointerUp(PointerEventData pointerEventData)
	{
		CsGameEventUIToUI.Instance.OnEventPointerUpCreatureSkill();
	}

	//---------------------------------------------------------------------------------------------------
	void IPointerExitHandler.OnPointerExit(PointerEventData pointerEventData)
	{
		CsGameEventUIToUI.Instance.OnEventPointerExitCreatureSkill();
	}
}
