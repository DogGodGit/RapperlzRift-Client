using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-17)
//---------------------------------------------------------------------------------------------------

public class CsRearItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

	//---------------------------------------------------------------------------------------------------
	void IPointerDownHandler.OnPointerDown(PointerEventData pointerEventData)
	{
		int nItemId = 0;

		if (int.TryParse(pointerEventData.selectedObject.name, out nItemId))
		{
			CsGameEventUIToUI.Instance.OnEventPointerDownCreatureFood(nItemId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void IPointerUpHandler.OnPointerUp(PointerEventData pointerEventData)
	{
		CsGameEventUIToUI.Instance.OnEventPointerUpCreatureFood();
	}

	//---------------------------------------------------------------------------------------------------
	void IPointerExitHandler.OnPointerExit(PointerEventData pointerEventData)
	{
		CsGameEventUIToUI.Instance.OnEventPointerExitCreatureFood();
	}
}
