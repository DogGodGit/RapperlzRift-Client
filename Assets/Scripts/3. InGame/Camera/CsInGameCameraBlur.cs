using UnityEngine;

public class CsInGameCameraBlur : MonoBehaviour
{
	public float m_flBlurStrength = 1.8f;
	public float m_flBlurWidth = 1.0f;
	int m_nCount = 0;
	int m_nStopCount = 8;

	Shader m_shader;
	Material m_material = null;

	public float BlurStrength { get { return m_flBlurStrength; } set { m_flBlurStrength = value; } }
	public float BlurWidth { get { return m_flBlurWidth; } set { m_flBlurWidth = value; } }
	public int StopCount { get { return m_nStopCount; } set { m_nStopCount = value; } }

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		m_shader = Shader.Find("FX/RadialBlur");
	}

	//---------------------------------------------------------------------------------------------------
	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		if (source == null || dest == null)
		{
			this.enabled = false;
			return;
		}

        if (m_nCount < m_nStopCount)
		{
			if (m_shader == null)
			{
				m_shader = Shader.Find("FX/RadialBlur");
				return;
			}

			if (m_material == null)
			{
				m_material = new Material(m_shader);
				m_material.hideFlags = HideFlags.HideAndDontSave;
				return;
			}
			
			m_material.SetFloat("_BlurStrength", m_flBlurStrength);
			m_material.SetFloat("_BlurWidth", m_flBlurWidth);
			m_material.SetFloat("_ImgHeight", 1); // float ImageWidth = 1;
			m_material.SetFloat("_ImgWidth", 1); // float ImageHeight = 1;

			Graphics.Blit(source, dest, m_material);
			m_nCount++;
		}
		else
		{
			ResetValue();
			this.enabled = false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SetCameraBlur(float flBlurStrength, float flBlurWidth, int nStopCount)
	{
		m_flBlurStrength = flBlurStrength;
		m_flBlurWidth = flBlurWidth;
		m_nStopCount = nStopCount;
	}

	//---------------------------------------------------------------------------------------------------
	void ResetValue()
	{
		m_material = null;
		m_flBlurStrength = 1.8f;
		m_flBlurWidth = 1.0f;
		m_nStopCount = 8;
		m_nCount = 0;
	}
}



