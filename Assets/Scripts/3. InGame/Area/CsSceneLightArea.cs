using UnityEngine;
using ClientCommon;
using System.Collections.Generic;
using System.Collections;

//[RequireComponent(typeof(SphereCollider))]
[System.Serializable]

public class SceneSettingValue
{
	[SerializeField]
	Color m_LightColor;
	[SerializeField]
	float m_flLightIntensity;
	Vector3 m_vtLightRotationVector;
	Quaternion m_LightRotation;

	[Range(0.0f, 360.0f)]
	public float m_flLightRotationX;
	[Range(0.0f, 360.0f)]
	public float m_flLightRotationY;

	[SerializeField]
	float m_flShadowsStrength;
	[SerializeField]
	float m_flShadowBias;
	[SerializeField]
	float m_flShadowNormalBias;

	[SerializeField]
	Color m_FogColor;
	[SerializeField]
	float m_flFogStart;
	[SerializeField]
	float m_flFogEnd;
	
	[SerializeField]
	Texture m_textureLut;
	[SerializeField]
	float m_flBlendAmount;

	[SerializeField]
	int m_nAreaRank;

	public Color LightColor { get { return m_LightColor; } }
	public float LightIntensity { get { return m_flLightIntensity; } }
	public Vector3 LightRotationVector { get { return m_vtLightRotationVector; } }
	public float LightRotationX { get { return m_flLightRotationX; } }
	public float LightRotationY { get { return m_flLightRotationY; } }
	public float ShadowsStrength { get { return m_flShadowsStrength; } }
	public float ShadowBias { get { return m_flShadowBias; } }
	public float ShadowNormalBias { get { return m_flShadowNormalBias; } }

	public Color FogColor { get { return m_FogColor; } }
	public float FogStart { get { return m_flFogStart; } }
	public float FogEnd { get { return m_flFogEnd; } }
	public Texture LutTexture { get { return m_textureLut; } }
	public float BlendAmount { get { return m_flBlendAmount; } }
	public int AreaRank { get { return m_nAreaRank; } }
	
	//---------------------------------------------------------------------------------------------------
	public SceneSettingValue(Light light, Color colorFog, float flFogStartDistance, float flFogEndDistance, Texture textureLut, float flBlendAmount)
	{
		m_LightColor = light.color;
		m_flLightIntensity = light.intensity;

		if (light != null)
		{
			m_LightRotation = light.transform.rotation;
			m_flLightRotationX = m_LightRotation.eulerAngles.x;
			m_flLightRotationY = m_LightRotation.eulerAngles.y;
			m_vtLightRotationVector = new Vector3(m_flLightRotationX, m_flLightRotationY, m_LightRotation.eulerAngles.z);
			m_nAreaRank = 0;
		}

		m_flShadowsStrength = light.shadowStrength;
		m_flShadowBias = light.shadowBias;
		m_flShadowNormalBias = light.shadowNormalBias;

		m_FogColor = colorFog;
		m_flFogStart = flFogStartDistance;
		m_flFogEnd = flFogEndDistance;
		m_textureLut = textureLut; // 공통
		m_flBlendAmount = flBlendAmount; // 공통
	}
}

public class CsSceneLightArea : MonoBehaviour
{
	CsSceneLightAreaManager csSceneLightAreaManager;

	[SerializeField]
	SceneSettingValue m_TargetSceneSetting;

	public SceneSettingValue TargetSceneSetting { get { return m_TargetSceneSetting; } }

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		csSceneLightAreaManager = transform.parent.transform.GetComponent<CsSceneLightAreaManager>();
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			csSceneLightAreaManager.SetPlay(this, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))              
		{
			csSceneLightAreaManager.SetPlay(this, false);
		}
	}
}
