using System.Collections;
using UnityEngine;

public static class CsDissolveHelper
{
	static int m_nDissolveAmountID = Shader.PropertyToID("_DissolveAmount");
	static int m_nGlowColorID = Shader.PropertyToID("_GlowColor");
	static int m_nEnvironmentIntensityID = Shader.PropertyToID("_envmapintensity");

	//---------------------------------------------------------------------------------------------------
	public static void ResetBrilliantly(Material mat)
	{
		if (mat != null)
		{
			if (mat.shader.name.Equals("QC_TFT Studio/Rappelz/Custom Hero Simple(Renewal)")) // 디졸브 쉐이더 일때만 발동.
			{
				if (mat.HasProperty(m_nEnvironmentIntensityID))
				{
					mat.SetFloat(m_nEnvironmentIntensityID, 0);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public static IEnumerator Brilliantly(Material mat, float value, float time) // 피격시 하얗게 변하는 이팩트 효과.
	{
		if (mat != null)
		{
			if (mat.shader.name.Equals("QC_TFT Studio/Rappelz/Custom Hero Simple(Renewal)")) // 디졸브 쉐이더 일때만 발동.
			{
				if (mat.HasProperty(m_nEnvironmentIntensityID))
				{
					if (mat.GetFloat(m_nEnvironmentIntensityID) == 0)
					{
						mat.SetColor(CsDissolveHelper.m_nGlowColorID, Color.red);
						mat.SetFloat(m_nEnvironmentIntensityID, value);
						yield return new WaitForSeconds(time);
						mat.SetFloat(m_nEnvironmentIntensityID, 0);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public static IEnumerator LinearDissolve(Transform trMonster, float from, float to, float time)
	{
		if (trMonster != null)
		{
			float elapsedTime = 0f;
			SkinnedMeshRenderer[] skinnedMeshRenderer = trMonster.GetComponentsInChildren<SkinnedMeshRenderer>();

			if (skinnedMeshRenderer.Length == 0) yield break;
			if (!skinnedMeshRenderer[0].material.shader.name.Equals("QC_TFT Studio/Rappelz/Custom Hero Simple(Renewal)")) yield break;

			skinnedMeshRenderer[0].material.SetColor(CsDissolveHelper.m_nGlowColorID, Color.red);

			if (from == 1)
			{
				skinnedMeshRenderer[0].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}

			while (elapsedTime < time)
			{
				if (skinnedMeshRenderer[0].material.HasProperty(m_nDissolveAmountID))
				{
					skinnedMeshRenderer[0].material.SetFloat(m_nDissolveAmountID, Mathf.Lerp(from, to, elapsedTime / time));

					if (from == 0)
					{
						if (skinnedMeshRenderer[0].material.GetFloat(m_nDissolveAmountID) > 0.5f && skinnedMeshRenderer[0].shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.On)
						{
							skinnedMeshRenderer[0].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
						}
					}
					else if (from == 1)
					{
						if (skinnedMeshRenderer[0].material.GetFloat(m_nDissolveAmountID) < 0.5f && skinnedMeshRenderer[0].shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.Off)
						{
							skinnedMeshRenderer[0].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
						}
					}
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			yield return new WaitForSeconds(0.5f);
			if (skinnedMeshRenderer[0].material.HasProperty(m_nDissolveAmountID))
			{
				skinnedMeshRenderer[0].material.SetFloat(m_nDissolveAmountID, to);
			}
		}
	}
}