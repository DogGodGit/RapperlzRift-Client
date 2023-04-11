	using UnityEditor;
	using UnityEngine;
	using System.Collections.Generic;

	public class SelectNoGLayer : EditorWindow
	{
		[MenuItem("Custom/Select Non-Layered Static")]
		static void Init()
		{

			// Find all the lightmap static objects

			Object[] All_GOs = FindObjectsOfType(typeof(GameObject));

			GameObject[] GOArray;
			GOArray = new GameObject[All_GOs.Length];

			int ArrayPointer = 0;

		int oneGLayer = 15;
		int twoGLayer = 16;
		int threeGLayer = 17;
		int TerrainLayer = 21;
		int FreeGLayer = 18;


			foreach(GameObject EachGO in All_GOs)
			{
				StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags( EachGO );

			if (EachGO.layer != oneGLayer && EachGO.layer != twoGLayer && EachGO.layer != threeGLayer && EachGO.layer != TerrainLayer&& EachGO.layer != FreeGLayer)
				{
					if( EachGO.GetComponent<MeshFilter>() != null )
					{
						GOArray[ArrayPointer] = EachGO;
						ArrayPointer += 1;
					}
				}

			}

			Selection.objects = GOArray;

		}

	}


