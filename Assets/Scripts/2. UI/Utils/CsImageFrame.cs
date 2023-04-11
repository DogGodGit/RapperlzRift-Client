using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsImageFrame : MonoBehaviour 
{
	Image m_image;
	float m_flAlpha = 1;
	bool m_bFor = true;

	void Awake()
	{
		m_image = transform.GetComponent<Image>();
		m_flAlpha = 1;
		m_bFor = true;
	}

	void OnEnable()
	{
		m_image = transform.GetComponent<Image>();
		m_flAlpha = 1;
		m_bFor = true;
	}

	void Update () 
	{
		if (m_bFor)
		{
			m_flAlpha -= Time.deltaTime;

			if (m_flAlpha < 0)
			{
				m_flAlpha = 0;
				m_bFor = false;
			}
		}
		else
		{
			m_flAlpha += Time.deltaTime;

			if (m_flAlpha > 1)
			{
				m_flAlpha = 1;
				m_bFor = true;
			}
		}

		m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, m_flAlpha);
	}
}
