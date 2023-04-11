﻿using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace VLB
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [SelectionBase]
    [HelpURL(Consts.HelpUrlBeam)]
    public class VolumetricLightBeam : MonoBehaviour
    {
        /// <summary>
        /// Get the color value from the light (when attached to a Spotlight) or not
        /// </summary>
        public bool colorFromLight = true;

        /// <summary>
        /// RGBA color of the beam (takes account of the alpha value).
        /// </summary>
        [ColorUsageAttribute(true, true, 0f, 10f, 0.125f, 3f)]
        [FormerlySerializedAs("colorValue")]
        public Color color = Color.white;

        /// <summary>
        /// Modulate the opacity of the inside geometry of the beam. Is multiplied to Color's alpha.
        /// </summary>
        [Range(0f, 1f)] public float alphaInside = Consts.Alpha;

        /// <summary>
        /// Modulate the opacity of the outside geometry of the beam. Is multiplied to Color's alpha.
        /// </summary>
        [Range(0f, 1f)] public float alphaOutside = Consts.Alpha;

        /// <summary>
        /// Get the spotAngle value from the light (when attached to a Spotlight) or not
        /// </summary>
        [FormerlySerializedAs("angleFromLight")]
        public bool spotAngleFromLight = true;

        /// <summary>
        /// Spot Angle (in degrees). This doesn't take account of the radiusStart, and is not necessarily the same than the cone angle.
        /// </summary>
        [Range(0.1f, 179.9f)] public float spotAngle = Consts.SpotAngle;

        /// <summary>
        /// Cone Angle (in degrees). This takes account of the radiusStart, and is not necessarily the same than the spot angle.
        /// </summary>
        public float coneAngle { get { return Mathf.Atan2(coneRadiusEnd - coneRadiusStart, fadeEnd) * Mathf.Rad2Deg * 2f; } }

        /// <summary>
        /// Start radius of the cone geometry.
        /// 0 will generate a perfect cone geometry. Higher values will generate truncated cones.
        /// </summary>
        [FormerlySerializedAs("radiusStart")]
        public float coneRadiusStart = Consts.ConeRadiusStart;

        /// <summary>
        /// End radius of the cone geometry
        /// </summary>
        public float coneRadiusEnd { get { return fadeEnd * Mathf.Tan(spotAngle * Mathf.Deg2Rad * 0.5f); } }

        /// <summary>
        /// Volume (in unit^3) of the cone (from the base to fadeEnd)
        /// </summary>
        public float coneVolume { get { float r1 = coneRadiusStart, r2 = coneRadiusEnd; return (Mathf.PI / 3) * (r1*r1 + r1*r2 + r2*r2) * fadeEnd; } }

        /// <summary>
        /// Apex distance of the truncated radius
        /// If coneRadiusStart = 0, the apex is the at the truncated radius, so coneApexOffsetZ = 0
        /// Otherwise, coneApexOffsetZ > 0 and represents the local position Z offset
        /// </summary>
        public float coneApexOffsetZ {
            get { // simple intercept
                float ratioRadius = coneRadiusStart / coneRadiusEnd;
                return ratioRadius == 1f ? 0f : ((fadeEnd * ratioRadius) / (1 - ratioRadius));
            }
        }

        /// <summary>
        /// Number of Sides of the cone.
        /// Higher values give better looking results, but require more memory and graphic performance.
        /// </summary>
        public int geomSides = Consts.GeomSides;

        /// <summary>
        /// Generate and show the cone cap (only visible from inside)
        /// </summary>
        public bool geomCap = Consts.GeomCap;

        /// <summary>
        /// Get the fadeEnd value from the light (when attached to a Spotlight) or not
        /// </summary>
        public bool fadeEndFromLight = true;

        public enum AttenuationEquation
        {
            Linear = 0,     // Simple linear attenuation.
            Quadratic = 1,  // Quadratic attenuation, which usually gives more realistic results.
            Blend = 2       // Custom blending mix between linear and quadratic attenuation formulas. Use attenuationEquation property to tweak the mix.
        }

        /// <summary>
        /// Light attenuation formula used to compute fading between 'fadeStart' and 'fadeEnd'
        /// </summary>
        public AttenuationEquation attenuationEquation = Consts.AttenuationEquation;

        /// <summary>
        /// Custom blending mix between linear and quadratic attenuation formulas.
        /// Only used if attenuationEquation is set to AttenuationEquation.Blend.
        /// 0.0 = 100% Linear
        /// 0.5 = Mix between 50% Linear and 50% Quadratic
        /// 1.0 = 100% Quadratic
        /// </summary>
        [Range(0f, 1f)] public float attenuationCustomBlending = Consts.AttenuationCustomBlending;

        /// <summary>
        /// Proper lerp value between linear and quadratic attenuation, used by the shader.
        /// </summary>
        public float attenuationLerpLinearQuad {
            get {
                if(attenuationEquation == AttenuationEquation.Linear) return 0f;
                else if (attenuationEquation == AttenuationEquation.Quadratic) return 1f;
                return attenuationCustomBlending;
            }
        }

        /// <summary>
        /// Distance from the light source (in units) the beam will start to fade out.
        /// </summary>
        public float fadeStart = Consts.FadeStart;

        /// <summary>
        /// Distance from the light source (in units) the beam is entirely faded out (alpha = 0, no more cone mesh).
        /// </summary>
        public float fadeEnd = Consts.FadeEnd;

        /// <summary>
        /// Distance from the world geometry the beam will fade.
        /// 0 = hard intersection
        /// Higher values produce soft intersection when the beam intersects other opaque geometry.
        /// </summary>
        public float depthBlendDistance = Consts.DepthBlendDistance;

        /// <summary>
        /// Distance from the camera the beam will fade.
        /// 0 = hard intersection
        /// Higher values produce soft intersection when the camera is near the cone triangles.
        /// </summary>
        public float cameraClippingDistance = Consts.CameraClippingDistance;

        /// <summary>
        /// Boost intensity factor when looking at the beam from the inside directly at the source.
        /// </summary>
        [Range(0f, 1f)]
        public float glareFrontal = Consts.GlareFrontal;

        /// <summary>
        /// Boost intensity factor when looking at the beam from behind.
        /// </summary>
        [Range(0f, 1f)]
        public float glareBehind = Consts.GlareBehind;

        [System.Obsolete("Use 'glareFrontal' instead")]
        public float boostDistanceInside = 0.5f;

        [System.Obsolete("This property has been merged with 'fresnelPow'")]
        public float fresnelPowInside = 6f;

        /// <summary>
        /// Modulate the thickness of the beam when looking at it from the side.
        /// Higher values produce thinner beam with softer transition at beam edges.
        /// </summary>
        [FormerlySerializedAs("fresnelPowOutside")]
        public float fresnelPow = Consts.FresnelPow;

        /// <summary>
        /// Enable 3D Noise effect
        /// </summary>
        public bool noiseEnabled = false;

        /// <summary>
        /// Contribution factor of the 3D Noise (when enabled).
        /// Higher intensity means the noise contribution is stronger and more visible.
        /// </summary>
        [Range(0f, 1f)] public float noiseIntensity = Consts.NoiseIntensityDefault;

        /// <summary>
        /// Get the noiseScale value from the Global 3D Noise configuration
        /// </summary>
        public bool noiseScaleUseGlobal = true;

        /// <summary>
        /// 3D Noise texture scaling: higher scale make the noise more visible, but potentially less realistic.
        /// </summary>
        [Range(Consts.NoiseScaleMin, Consts.NoiseScaleMax)] public float noiseScaleLocal = Consts.NoiseScaleDefault;

        /// <summary>
        /// Get the noiseVelocity value from the Global 3D Noise configuration
        /// </summary>
        public bool noiseVelocityUseGlobal = true;

        /// <summary>
        /// World Space direction and speed of the 3D Noise scrolling, simulating the fog/smoke movement.
        /// </summary>
        public Vector3 noiseVelocityLocal = Consts.NoiseVelocityDefault;

        /// <summary>
        /// If true, the light beam will keep track of the changes of its own properties and the spotlight attached to it (if any) during playtime.
        /// This would allow you to modify the light beam in realtime from Script, Animator and/or Timeline.
        /// Enabling this feature is at very minor performance cost. So keep it disabled if you don't plan to modify this light beam during playtime.
        /// </summary>
        public bool trackChangesDuringPlaytime = false;

        /// <summary> Has the geometry already been generated? </summary>
        public bool hasGeometry { get { return m_BeamGeom != null; } }

        /// <summary> Bounds of the geometry's mesh (if the geometry exists) </summary>
        public Bounds bounds { get { return m_BeamGeom != null ? m_BeamGeom.meshRenderer.bounds : new Bounds(Vector3.zero, Vector3.zero); } }

        /// <summary> Set the clipping plane equation. This function is used internally by the DynamicOcclusion component. </summary>
        public void SetClippingPlane(Plane planeWS) { if (m_BeamGeom) m_BeamGeom.SetClippingPlane(planeWS); }

        /// <summary> Disable the clipping plane. This function is used internally by the DynamicOcclusion component. </summary>
        public void SetClippingPlaneOff() { if (m_BeamGeom) m_BeamGeom.SetClippingPlaneOff(); }

        // INTERNAL
#pragma warning disable 0414
        [SerializeField] int pluginVersion = -1;
#pragma warning restore 0414

        BeamGeometry m_BeamGeom = null;

#if UNITY_EDITOR
        static VolumetricLightBeam[] _EditorFindAllInstances()
        {
            return Resources.FindObjectsOfTypeAll<VolumetricLightBeam>();
        }

        public static void _EditorSetAllMeshesDirty()
        {
            foreach (var instance in _EditorFindAllInstances())
                instance._EditorSetMeshDirty();
        }

        public void _EditorSetMeshDirty() { m_EditorDirtyFlags |= EditorDirtyFlags.Mesh; }

        [System.Flags]
        enum EditorDirtyFlags
        {
            Clean = 0,
            Props = 1 << 1,
            Mesh  = 1 << 2,
        }
        EditorDirtyFlags m_EditorDirtyFlags;
        CachedLightProperties m_PrevCachedLightProperties;
#endif

        public string meshStats
        {
            get
            {
                Mesh mesh = m_BeamGeom ? m_BeamGeom.coneMesh : null;
                if (mesh) return string.Format("Cone angle: {0:0.0} degrees\nMesh: {1} vertices, {2} triangles", coneAngle, mesh.vertexCount, mesh.triangles.Length / 3);
                else return "no mesh available";
            }
        }

        Light lightSpotAttached
        {
            get
            {
                var light = GetComponent<Light>();
                if (light && light.type == LightType.Spot) return light;
                return null;
            }
        }

        /// <summary>
        /// Returns a value indicating if the world position passed in argument is inside the light beam or not.
        /// This functions treats the beam like infinite (like the beam had an infinite length and never fell off)
        /// </summary>
        /// <param name="posWS">World position</param>
        /// <returns>
        /// < 0 position is out
        /// = 0 position is exactly on the beam geometry 
        /// > 0 position is inside the cone
        /// </returns>
        public float GetInsideBeamFactor(Vector3 posWS)
        {
            var posOS = transform.InverseTransformPoint(posWS);
            if (posOS.z < 0f) return -1f;

            // Compute a factor to know how far inside the beam cone the camera is
            var triangle2D = new Vector2(posOS.xy().magnitude, posOS.z + coneApexOffsetZ).normalized;
            const float maxRadiansDiff = 0.1f;
            float slopeRad = (coneAngle * Mathf.Deg2Rad) / 2;

            return Mathf.Clamp((Mathf.Abs(Mathf.Sin(slopeRad)) - Mathf.Abs(triangle2D.x)) / maxRadiansDiff, -1, 1);
        }

        [System.Obsolete("Use 'GenerateGeometry()' instead")]
        public void Generate() { GenerateGeometry(); }

        /// <summary>
        /// Regenerate the beam mesh (and also the material).
        /// This can be slow (it recreates a mesh from scratch), so don't call this function during playtime.
        /// You would need to call this function only if you want to change the properties 'geomSides' and 'geomCap' during playtime.
        /// Otherwise, for the other properties, just enable 'trackChangesDuringPlaytime', or manually call 'UpdateAfterManualPropertyChange()'
        /// </summary>
        public void GenerateGeometry()
        {
#if UNITY_EDITOR
            HandleBackwardCompatibility(pluginVersion, Version.Current);
#endif
            pluginVersion = Version.Current;

            AssignPropertiesFromSpotLight(lightSpotAttached);
            ValidateProperties();

            if (m_BeamGeom == null)
            {
                var shader = Config.Instance.beamShader;
                if (!shader)
                {
                    Debug.LogError("Invalid BeamShader set in VLB Config");
                    return;
                }
                m_BeamGeom = Utils.NewWithComponent<BeamGeometry>("Beam Geometry");
                m_BeamGeom.Initialize(this, shader);
            }

            m_BeamGeom.RegenerateMesh();
            m_BeamGeom.visible = enabled;
        }

        /// <summary>
        /// Update the beam material and its bounds.
        /// Calling manually this function is useless if your beam has its property 'trackChangesDuringPlaytime' enabled
        /// (because then this function is automatically called each frame).
        /// However, if 'trackChangesDuringPlaytime' is disabled, and you change a property via Script for example,
        /// you need to call this function to take the property change into account.
        /// All properties changes are took into account, expect 'geomSides' and 'geomCap' which require to regenerate the geometry via 'GenerateGeometry()'
        /// </summary>
        public void UpdateAfterManualPropertyChange()
        {
            if (m_BeamGeom) m_BeamGeom.UpdateMaterialAndBounds();
        }

#if !UNITY_EDITOR
        void Awake()
        {
            // In standalone builds, simply generate the geometry once in Awake
            GenerateGeometry();
        }
#else
        void Awake()
        {
            if (Application.isPlaying)
            {
                GenerateGeometry();
                m_EditorDirtyFlags = EditorDirtyFlags.Clean;
            }
            else
            {
                // In Editor, creating geometry from Awake and/or OnValidate generates warning in Unity 2017.
                // So we do it from Update
                m_EditorDirtyFlags = EditorDirtyFlags.Props | EditorDirtyFlags.Mesh;
            }
        }

        void OnValidate()
        {
            m_EditorDirtyFlags |= EditorDirtyFlags.Props; // Props have been modified from Editor
        }

        void Update() // EDITOR ONLY
        {
            // Handle edition of light properties in Editor
            if (!Application.isPlaying)
            {
                var newProps = new CachedLightProperties(lightSpotAttached);
                if(!newProps.Equals(m_PrevCachedLightProperties))
                    m_EditorDirtyFlags |= EditorDirtyFlags.Props;
                m_PrevCachedLightProperties = newProps;
            }

            if (m_EditorDirtyFlags == EditorDirtyFlags.Clean)
            {
                if (Application.isPlaying)
                {
                    if (!trackChangesDuringPlaytime) // during Playtime, realtime changes are handled by CoUpdateDuringPlaytime
                        return;
                }
            }
            else
            {
                if (m_EditorDirtyFlags.HasFlag(EditorDirtyFlags.Mesh))
                {
                    GenerateGeometry(); // regenerate everything
                }
                else if (m_EditorDirtyFlags.HasFlag(EditorDirtyFlags.Props))
                {
                    AssignPropertiesFromSpotLight(lightSpotAttached);
                    ValidateProperties();
                }
            }

            // If we modify the attached Spotlight properties, or if we animate the beam via Unity 2017's timeline,
            // we are not notified of properties changes. So we update the material anyway.
            UpdateAfterManualPropertyChange();

            m_EditorDirtyFlags = EditorDirtyFlags.Clean;
        }

        public void Reset()
        {
            color = Color.white;
            colorFromLight = true;

            alphaInside = Consts.Alpha;
            alphaOutside = Consts.Alpha;

            spotAngleFromLight = true;
            spotAngle = Consts.SpotAngle;

            coneRadiusStart = Consts.ConeRadiusStart;
            geomSides = Consts.GeomSides;
            geomCap = Consts.GeomCap;

            fadeEndFromLight = true;
            fadeStart = Consts.FadeStart;
            fadeEnd = Consts.FadeEnd;

            depthBlendDistance = Consts.DepthBlendDistance;
            cameraClippingDistance = Consts.CameraClippingDistance;

            glareFrontal = Consts.GlareFrontal;
            glareBehind = Consts.GlareBehind;

            fresnelPow = Consts.FresnelPow;

            noiseEnabled = false;
            noiseIntensity = Consts.NoiseIntensityDefault;
            noiseScaleUseGlobal = true;
            noiseScaleLocal = Consts.NoiseScaleDefault;
            noiseVelocityUseGlobal = true;
            noiseVelocityLocal = Consts.NoiseVelocityDefault;

            trackChangesDuringPlaytime = false;

            m_EditorDirtyFlags = EditorDirtyFlags.Props | EditorDirtyFlags.Mesh;
        }
#endif

        void OnEnable()
        {
            if (m_BeamGeom) m_BeamGeom.visible = true;
            if (Application.isPlaying && trackChangesDuringPlaytime)
            {
                StartCoroutine(CoUpdateDuringPlaytime());
            }
        }

        void OnDisable()
        {
            if (m_BeamGeom) m_BeamGeom.visible = false;
        }

        IEnumerator CoUpdateDuringPlaytime()
        {
            var cachedSpotLight = lightSpotAttached; // prevent from calling GetComponent<Light>() each frame
            while (trackChangesDuringPlaytime && enabled)
            {
                AssignPropertiesFromSpotLight(cachedSpotLight);
                ValidateProperties();

                if (m_BeamGeom) m_BeamGeom.UpdateMaterialAndBounds();
                yield return null;
            }
        }

        void OnDestroy()
        {
            if (m_BeamGeom) DestroyImmediate(m_BeamGeom.gameObject); // Make sure to delete the GAO
            m_BeamGeom = null;
        }

        void AssignPropertiesFromSpotLight(Light lightSpot)
        {
            if (lightSpot && lightSpot.type == LightType.Spot)
            {
                if (fadeEndFromLight) fadeEnd = lightSpot.range;
                if (spotAngleFromLight) spotAngle = lightSpot.spotAngle;
                if (colorFromLight) color = lightSpot.color;
            }
        }

        void ValidateProperties()
        {
            fadeEnd = Mathf.Max(Consts.FadeMinThreshold, fadeEnd);
            fadeStart = Mathf.Clamp(fadeStart, 0f, fadeEnd - Consts.FadeMinThreshold);

            const float kSpotAngleOffset = 0.1f;
            spotAngle = Mathf.Clamp(spotAngle, 0 + kSpotAngleOffset, 180 - kSpotAngleOffset);

            coneRadiusStart = Mathf.Max(coneRadiusStart, 0f);

            depthBlendDistance = Mathf.Max(depthBlendDistance, 0f);
            cameraClippingDistance = Mathf.Max(cameraClippingDistance, 0f);

            geomSides = Mathf.Clamp(geomSides, 3, 256);

            fresnelPow = Mathf.Max(0f, fresnelPow);
        }

#if UNITY_EDITOR
        void HandleBackwardCompatibility(int serializedVersion, int newVersion)
        {
            if (serializedVersion == -1) return;            // freshly new spawned entity: nothing to do
            if (serializedVersion == newVersion) return;    // same version: nothing to do

            if (serializedVersion < 1301) attenuationEquation = AttenuationEquation.Linear; // quadratic attenuation is a new feature of 1.3
        }
#endif
    }
}
