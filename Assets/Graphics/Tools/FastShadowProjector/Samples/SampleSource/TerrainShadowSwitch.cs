using UnityEngine;
using System.Collections;

namespace FSP_Samples {

	public class TerrainShadowSwitch : MonoBehaviour {

		// This is just for sample scene, when creating your own scene & terrain this will be done automatically
		public Terrain duplicateTerrain;
		public Material terrainMat;
		
		// This is just for sample scene, when creating your own scene & terrain this will be done automatically
		void Awake() {
			
	#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_2017
			duplicateTerrain.materialType = Terrain.MaterialType.Custom;
	#endif
			duplicateTerrain.materialTemplate = terrainMat;
			duplicateTerrain.materialTemplate.SetTexture("_ShadowTex", GlobalProjectorManager.Get().GetShadowTexture());
		}

		
		void OnGUI() {
			if (GUI.Button(new Rect(Screen.width - 140, 50, 130, 40), "Toggle Shadows")) {
				if (GetComponent<ShadowReceiver>() != null) {
					bool enabled = GetComponent<ShadowReceiver>().enabled;
					GetComponent<ShadowReceiver>().enabled =!enabled;
					GetComponent<Terrain>().enabled =!enabled;
				}
			}
		}
	}

}
