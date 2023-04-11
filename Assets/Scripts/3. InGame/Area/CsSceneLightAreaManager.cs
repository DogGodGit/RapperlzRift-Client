using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneLightAreaManager : MonoBehaviour 
{
	SceneSettingValue m_DefaultSceneSetting;
	AmplifyColorEffect m_AmplifyColorEffect;
	Texture m_tx;
	List<CsSceneLightArea> m_listSceneLightArea;

	SceneSettingValue m_CurrentSceneSetting;
	Coroutine m_coroutine;

	public SceneSettingValue DefaultSceneSetting { get { return m_DefaultSceneSetting; } }
	public AmplifyColorEffect AmplifyColorEffect { get { return m_AmplifyColorEffect; } }
	public List<CsSceneLightArea> EnterSceneLightAreaList { get { return m_listSceneLightArea; } }
	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_AmplifyColorEffect = Camera.main.GetComponent<AmplifyColorEffect>();
		float flBlendAmount = m_AmplifyColorEffect.BlendAmount; // 공통
		m_tx = m_AmplifyColorEffect.LutTexture;
		m_DefaultSceneSetting = new SceneSettingValue(RenderSettings.sun, RenderSettings.fogColor, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance, m_tx, flBlendAmount);
		m_listSceneLightArea = new List<CsSceneLightArea>();
		m_coroutine = null;
		m_CurrentSceneSetting = m_DefaultSceneSetting;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetPlay(CsSceneLightArea csSceneLightArea, bool bisEnter)
	{
		if (csSceneLightArea == null) return;

		if (bisEnter)
		{
			if (m_listSceneLightArea.Contains(csSceneLightArea) == false)
			{
				m_listSceneLightArea.Add(csSceneLightArea);

				if (m_CurrentSceneSetting == m_DefaultSceneSetting)
				{
					if (m_coroutine != null)
					{
						StopCoroutine(m_coroutine);
					}
					m_CurrentSceneSetting = csSceneLightArea.TargetSceneSetting;
					m_coroutine = StartCoroutine(ChangeValue(csSceneLightArea.TargetSceneSetting));
				}
				else
				{
					if (m_CurrentSceneSetting.AreaRank > csSceneLightArea.TargetSceneSetting.AreaRank)  // 낮은수 우선(1,2,3,4,5...)
					{
						if (m_coroutine != null)
						{
							StopCoroutine(m_coroutine);
						}
						m_CurrentSceneSetting = csSceneLightArea.TargetSceneSetting;
						m_coroutine = StartCoroutine(ChangeValue(csSceneLightArea.TargetSceneSetting));
					}
				}
			}
		}
		else
		{
			m_listSceneLightArea.Remove(csSceneLightArea);

			if (m_CurrentSceneSetting == csSceneLightArea.TargetSceneSetting)
			{
				SceneSettingValue SceneSettingValue = null;

				for (int i = 0; i < m_listSceneLightArea.Count; i++)
				{
					if (SceneSettingValue == null || SceneSettingValue.AreaRank > m_listSceneLightArea[i].TargetSceneSetting.AreaRank)
					{
						SceneSettingValue = m_listSceneLightArea[i].TargetSceneSetting;
					}
				}

				if (m_coroutine != null)
				{
					StopCoroutine(m_coroutine);
				}

				if (SceneSettingValue != null)
				{
					m_CurrentSceneSetting = SceneSettingValue;
					m_coroutine = StartCoroutine(ChangeValue(SceneSettingValue));
				}
				else
				{
					m_CurrentSceneSetting = m_DefaultSceneSetting;
					m_coroutine = StartCoroutine(ChangeValue(m_DefaultSceneSetting));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ChangeValue(SceneSettingValue sceneSettingValue)
	{
		float flLightIntensity = RenderSettings.sun.intensity;

		float flLightRotX = RenderSettings.sun.transform.rotation.eulerAngles.x;
		float flLightRotY = RenderSettings.sun.transform.rotation.eulerAngles.y;
		float flEulerX = sceneSettingValue.LightRotationX;
		float flEulerY = sceneSettingValue.LightRotationY;

		float flShadowStrength = RenderSettings.sun.shadowStrength;

		float flFogStart = RenderSettings.fogStartDistance;
		float flflFogEnd = RenderSettings.fogEndDistance;
		float flflBlendAmount = m_AmplifyColorEffect.BlendAmount;
		float flTime = 0;

		RenderSettings.sun.color = sceneSettingValue.LightColor;
		RenderSettings.fogColor = sceneSettingValue.FogColor;
		RenderSettings.sun.shadowBias = sceneSettingValue.ShadowBias;
		RenderSettings.sun.shadowNormalBias = sceneSettingValue.ShadowNormalBias;		
		m_AmplifyColorEffect.BlendTo(sceneSettingValue.LutTexture, 0.5f, null);

		if (flEulerX < 0)
		{
			flEulerX = flEulerX + 360;
		}
		else if (flEulerX > 360)
		{
			flEulerX = flEulerX - 360;
		}

		if (flEulerY < 0)
		{
			flEulerY += 360;
		}
		else if (flEulerY > 360)
		{
			flEulerY -= 360;
		}

		Quaternion qtCurrentRotation = RenderSettings.sun.transform.rotation;
		Quaternion qtTargetRotation = Quaternion.Euler(flEulerX, flEulerY, RenderSettings.sun.transform.eulerAngles.z);

		while (flTime < 1f)
		{
			flTime += Time.deltaTime;
			if (flTime > 1)
			{
				flTime = 1.0f;
			}

			RenderSettings.sun.transform.rotation = Quaternion.Lerp(qtCurrentRotation, qtTargetRotation, flTime);
			RenderSettings.sun.intensity = Mathf.Lerp(flLightIntensity, sceneSettingValue.LightIntensity, flTime);
			RenderSettings.sun.shadowStrength = Mathf.Lerp(flShadowStrength, sceneSettingValue.ShadowsStrength, flTime);
			RenderSettings.fogStartDistance = Mathf.Lerp(flFogStart, sceneSettingValue.FogStart, flTime);
			RenderSettings.fogEndDistance = Mathf.Lerp(flflFogEnd, sceneSettingValue.FogEnd, flTime);
			m_AmplifyColorEffect.BlendAmount = Mathf.Lerp(flflBlendAmount, sceneSettingValue.BlendAmount, flTime);

			yield return null;
		}

		RenderSettings.sun.intensity = sceneSettingValue.LightIntensity;
		RenderSettings.sun.transform.rotation = qtTargetRotation;
		RenderSettings.sun.shadowStrength = sceneSettingValue.ShadowsStrength;
		RenderSettings.fogStartDistance = sceneSettingValue.FogStart;
		RenderSettings.fogEndDistance = sceneSettingValue.FogEnd;
		m_AmplifyColorEffect.BlendAmount = sceneSettingValue.BlendAmount;

		m_coroutine = null;
	}
}
