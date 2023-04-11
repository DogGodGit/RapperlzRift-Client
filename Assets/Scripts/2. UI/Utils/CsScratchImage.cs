using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-07-10)
//---------------------------------------------------------------------------------------------------
// Sprite의 Read/Write Enabled 속성 필요 & RGBA32 포맷 사용
public class CsScratchImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	int m_nDefaultDragThreshold = 0;

	public event Delegate EventScratchFinish;

	Texture2D m_texture2dNew;

	Vector2 m_vtResolutionRatio;

	RectTransform rectTransform;

	bool m_bScratchable = true;

	public bool Scratchable
	{
		get { return m_bScratchable; }
		set { m_bScratchable = value; }
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();

		m_nDefaultDragThreshold = EventSystem.current.pixelDragThreshold;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		Reset();
	}

	//---------------------------------------------------------------------------------------------------
	float GetPercent()
	{
		if (m_texture2dNew != null)
		{
			const float ratio = 0.2f; // 부하를 줄이기 위해 모든 픽셀을 체크하지 않고 10개 중에 2개만 체크

			int nWidth = (int)(m_texture2dNew.width * ratio);
			int nHeight = (int)(m_texture2dNew.height * ratio);

			int nPixelCount = nWidth * nHeight;
			int nRemovePixelCount = 0;

			for (int i = 0; i < nWidth; i++)
			{
				for (int j = 0; j < nHeight; j++)
				{
					if (m_texture2dNew.GetPixel((int)(i / ratio), (int)(j / ratio)).a < 1)
					{
						nRemovePixelCount++;
					}
				}
			}

			return (float)nRemovePixelCount / nPixelCount * 100.0f;
		}

		return 0f;
	}

	//---------------------------------------------------------------------------------------------------
	void Reset()
	{
		Sprite spriteOriginal;
		
		spriteOriginal = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupRookieGift/frm_scratch_" + UnityEngine.Random.Range(1, 9).ToString());
		
		m_texture2dNew = new Texture2D(spriteOriginal.texture.width, spriteOriginal.texture.height, spriteOriginal.texture.format, false);
		m_texture2dNew.SetPixels(spriteOriginal.texture.GetPixels());
		m_texture2dNew.Apply();

		transform.GetComponent<Image>().sprite = Sprite.Create(m_texture2dNew, new Rect(0, 0, m_texture2dNew.width, m_texture2dNew.height), new Vector2(0.5f, 0.5f));;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!m_bScratchable)
			return;

		if (EventSystem.current != null)
		{
			EventSystem.current.pixelDragThreshold = 0;
		}

		RemoveImage(eventData);
	}

	//---------------------------------------------------------------------------------------------------
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!m_bScratchable)
			return;

		if (EventSystem.current != null)
		{
			EventSystem.current.pixelDragThreshold = m_nDefaultDragThreshold;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnDrag(PointerEventData eventData)
	{
		if (!m_bScratchable)
			return;

		RemoveImage(eventData);
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveImage(PointerEventData eventData)
	{
		if (m_texture2dNew != null)
		{
			Vector2 vt2LocalPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out vt2LocalPosition);

			vt2LocalPosition = new Vector2(rectTransform.sizeDelta.x * 0.5f + vt2LocalPosition.x, vt2LocalPosition.y);

			const int nRadius = 80;

			for (int i = -nRadius; i <= nRadius; i++)
			{
				for (int j = -nRadius; j <= nRadius; j++)
				{
					Vector2 vt2Point = vt2LocalPosition + new Vector2(i, j);

					if (Vector2.Distance(vt2LocalPosition, vt2Point) <= nRadius)
					{
						RemovePixel(vt2Point);
					}
				}
			}
			
			m_texture2dNew.Apply();

			if (GetPercent() > 99.7)
			{
				if (EventScratchFinish != null)
					EventScratchFinish();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemovePixel(Vector2 vt2Position)
	{
		if (vt2Position.x < 1 || vt2Position.x > m_texture2dNew.width ||
				vt2Position.y < 1 || vt2Position.y > m_texture2dNew.height)
		{
			return;
		}

		Color color = m_texture2dNew.GetPixel((int)vt2Position.x, (int)vt2Position.y);

		if (color.a <= 0)
			return;

		color.a = 0;
		m_texture2dNew.SetPixel((int)vt2Position.x, (int)vt2Position.y, color);
	}

	//---------------------------------------------------------------------------------------------------
	public IEnumerator AutoScratch(int nAutoScratchSeconds)
	{
		yield return new WaitForSeconds(nAutoScratchSeconds);

		if (EventScratchFinish != null)
			EventScratchFinish();
	}
}
