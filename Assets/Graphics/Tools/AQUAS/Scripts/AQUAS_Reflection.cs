using UnityEngine;
using System.Collections;

[AddComponentMenu("AQUAS/Reflection")]
[ExecuteInEditMode] // Make mirror live-update even when not in play mode
public class AQUAS_Reflection : MonoBehaviour
{
	static bool s_InsideRendering = false;

	Hashtable m_hashtableReflectionCameras = new Hashtable(); // Camera -> Camera table
	RenderTexture m_renderTexture = null;
	Camera m_camera, m_reflectionCamera;

	int m_nOldReflectionTextureSize = 0;
	int m_nTextureSize = 128;

	public float m_flClipPlaneOffset = 0.07f;
	public LayerMask m_ReflectLayers = -1;

    public void OnWillRenderObject()
	{
		if (!enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial || !GetComponent<Renderer>().enabled) 	return;

		if (m_camera == null)
		{
			m_camera = Camera.current;
			CreateMirrorObjects(m_camera, out m_reflectionCamera);
			m_reflectionCamera.useOcclusionCulling = true; // ignoreOcclusionCulling = false;
			//QualitySettings.pixelLightCount = 0;  // m_DisablePixelLights = true;
			return;
		}

		if (!s_InsideRendering)
		{
			s_InsideRendering = true;

			UpdateCameraModes(m_camera, m_reflectionCamera);

			float flD = -Vector3.Dot(transform.up, transform.position) - m_flClipPlaneOffset;   // Render reflection
			Vector4 reflectionPlane = new Vector4(transform.up.x, transform.up.y, transform.up.z, flD);  	// Reflect camera around reflection plane

			Matrix4x4 reflection = Matrix4x4.zero;
			CalculateReflectionMatrix(ref reflection, reflectionPlane);

			Vector3 oldpos = m_camera.transform.position;
			Vector3 newpos = reflection.MultiplyPoint(oldpos);

			m_reflectionCamera.worldToCameraMatrix = m_camera.worldToCameraMatrix * reflection;

			Vector4 clipPlane = CameraSpacePlane(m_reflectionCamera, transform.position, transform.up, 1.0f);
			Matrix4x4 projection = m_camera.projectionMatrix;
			CalculateObliqueMatrix(ref projection, clipPlane);

			m_reflectionCamera.projectionMatrix = projection;
			m_reflectionCamera.cullingMask = ~(1 << 4) & m_ReflectLayers.value;
			m_reflectionCamera.targetTexture = m_renderTexture;

			GL.SetRevertBackfacing(true);    //obsolete
			m_reflectionCamera.transform.position = newpos;
			Vector3 euler = m_camera.transform.eulerAngles;
			m_reflectionCamera.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
			m_reflectionCamera.Render();
			m_reflectionCamera.transform.position = oldpos;
			GL.SetRevertBackfacing(false);   //obsolete

			Material[] materials = GetComponent<Renderer>().sharedMaterials;

			Matrix4x4 scaleOffset = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
			Matrix4x4 mtx = transform.localToWorldMatrix * Matrix4x4.Scale(new Vector3(1.0f / transform.lossyScale.x, 1.0f / transform.lossyScale.y, 1.0f / transform.lossyScale.z));
			mtx = scaleOffset * m_camera.projectionMatrix * m_camera.worldToCameraMatrix * mtx;

			for (int i = 0; i < materials.Length; i++)
			{
				if (materials[i].HasProperty("_ReflectionTex"))
				{
					materials[i].SetTexture("_ReflectionTex", m_renderTexture);
					materials[i].SetMatrix("_ProjMatrix", mtx);
				}
			}
			s_InsideRendering = false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnDisable()
	{
		m_camera = m_reflectionCamera = null;

		if (m_renderTexture)
		{
			DestroyImmediate(m_renderTexture);
			m_renderTexture = null;
		}
		
		for (int i = 0; i < m_hashtableReflectionCameras.Count; i++)
		{
			DestroyImmediate((GameObject)m_hashtableReflectionCameras[i]);
		}

		m_hashtableReflectionCameras.Clear();
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCameraModes(Camera srcCamera, Camera destCamera)
	{
		if (destCamera == null) return;

		destCamera.clearFlags = srcCamera.clearFlags;
		destCamera.backgroundColor = srcCamera.backgroundColor;

		if (srcCamera.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox sky = srcCamera.GetComponent(typeof(Skybox)) as Skybox;
			Skybox mysky = destCamera.GetComponent(typeof(Skybox)) as Skybox;
			if (!sky || !sky.material)
			{
				mysky.enabled = false;
			}

			else
			{
				mysky.enabled = true;
				mysky.material = sky.material;
			}
		}

		destCamera.farClipPlane = srcCamera.farClipPlane;
		destCamera.nearClipPlane = srcCamera.nearClipPlane;
		destCamera.orthographic = srcCamera.orthographic;
		destCamera.fieldOfView = srcCamera.fieldOfView;
		destCamera.aspect = srcCamera.aspect;
		destCamera.orthographicSize = srcCamera.orthographicSize;
	}

	//---------------------------------------------------------------------------------------------------
	void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera) 	// Creates any objects needed on demand
	{
		reflectionCamera = null;

		if (!m_renderTexture || m_nOldReflectionTextureSize != m_nTextureSize)	// Reflection render texture
		{
			if (m_renderTexture)
			{
				DestroyImmediate(m_renderTexture);
			}
			m_renderTexture = new RenderTexture(m_nTextureSize, m_nTextureSize, 16);
			m_renderTexture.name = "__MirrorReflection" + GetInstanceID();
			m_renderTexture.isPowerOfTwo = true;
			m_renderTexture.hideFlags = HideFlags.DontSave;
			m_nOldReflectionTextureSize = m_nTextureSize;
		}

		reflectionCamera = m_hashtableReflectionCameras[currentCamera] as Camera; // Camera for reflection
		if (!reflectionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
		{
			GameObject go = new GameObject("Mirror Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
			reflectionCamera = go.GetComponent<Camera>();
			reflectionCamera.enabled = false;
			reflectionCamera.transform.position = transform.position;
			reflectionCamera.transform.rotation = transform.rotation;
			reflectionCamera.gameObject.AddComponent<FlareLayer>();
			go.hideFlags = HideFlags.HideAndDontSave;
			m_hashtableReflectionCameras[currentCamera] = reflectionCamera;
		}        
	}
	
	//---------------------------------------------------------------------------------------------------
	static float sgn(float a)
	{
		if (a > 0.0f) return 1.0f;
		if (a < 0.0f) return -1.0f;
		return 0.0f;
	}
	
	//---------------------------------------------------------------------------------------------------
	Vector4 CameraSpacePlane (Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 offsetPos = pos + normal * m_flClipPlaneOffset;
		Matrix4x4 m = cam.worldToCameraMatrix;
		Vector3 cpos = m.MultiplyPoint( offsetPos );
		Vector3 cnormal = m.MultiplyVector( normal ).normalized * sideSign;
		return new Vector4( cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos,cnormal) );
	}
	
	//---------------------------------------------------------------------------------------------------
	static void CalculateObliqueMatrix (ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 vtQ = projection.inverse * new Vector4(sgn(clipPlane.x), sgn(clipPlane.y), 1.0f, 1.0f);
		Vector4 vtC = clipPlane * (2.0F / (Vector4.Dot (clipPlane, vtQ)));

		projection[2] = vtC.x - projection[3];
		projection[6] = vtC.y - projection[7];
		projection[10] = vtC.z - projection[11];
		projection[14] = vtC.w - projection[15];
	}

	//---------------------------------------------------------------------------------------------------
	static void CalculateReflectionMatrix (ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = (1F - 2F*plane[0]*plane[0]);
		reflectionMat.m01 = (   - 2F*plane[0]*plane[1]);
		reflectionMat.m02 = (   - 2F*plane[0]*plane[2]);
		reflectionMat.m03 = (   - 2F*plane[3]*plane[0]);
		
		reflectionMat.m10 = (   - 2F*plane[1]*plane[0]);
		reflectionMat.m11 = (1F - 2F*plane[1]*plane[1]);
		reflectionMat.m12 = (   - 2F*plane[1]*plane[2]);
		reflectionMat.m13 = (   - 2F*plane[3]*plane[1]);
		
		reflectionMat.m20 = (   - 2F*plane[2]*plane[0]);
		reflectionMat.m21 = (   - 2F*plane[2]*plane[1]);
		reflectionMat.m22 = (1F - 2F*plane[2]*plane[2]);
		reflectionMat.m23 = (   - 2F*plane[3]*plane[2]);
		
		reflectionMat.m30 = 0F;
		reflectionMat.m31 = 0F;
		reflectionMat.m32 = 0F;
		reflectionMat.m33 = 1F;
	}
}
