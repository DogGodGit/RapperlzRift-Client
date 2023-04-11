using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artngame.INfiniDy
{
	[ExecuteInEditMode]
	public class ControlCombineChildrenINfiniDyGrass : MonoBehaviour
	{
		public struct Meshy
		{
			public string name;
			public Vector3[] vertices;
			public Vector3[] normals;
			public Color[] colors;
			public Vector2[] uv;
			public Vector2[] uv1;
			public Vector4[] tangents;
			public int[] triangles;
			public bool thread_ended;
		}

		//bool noThreading = false;
		int threads_started = 0;
		int threads_ended = 0;
		bool all_threads_started = false;

		List<Meshy> MeshList = new List<Meshy>();
		List<int> MeshIDList = new List<int>();

		List<GameObject> Destroy_list;
		List<MeshFilter> Destroy_list_MF;
		List<GameObject> Destroy_list64;
		List<MeshFilter> Destroy_list_MF64;

		List<int> Splits = new List<int>();
		List<Material> Material_list = new List<Material>();

		public bool m_bShadowCasting = false;
		public bool m_bReceiveShadows = false;

		//---------------------------------------------------------------------------------------------------
		void Start()
		{
			if (Application.isPlaying == false) return;
			Full_reset();

			if (Destroy_list == null)
			{
				Destroy_list = new List<GameObject>();
			}

			if (Destroy_list_MF == null)
			{
				Destroy_list_MF = new List<MeshFilter>();
			}

			if (Destroy_list64 == null)
			{
				Destroy_list64 = new List<GameObject>();
			}

			if (Destroy_list_MF64 == null)
			{
				Destroy_list_MF64 = new List<MeshFilter>();
			}

			Component[] filters = GetComponentsInChildren(typeof(MeshFilter));

			for (int i = 0; i < filters.Length; i++)
			{
				Renderer curRenderer = filters[i].GetComponent<Renderer>();

				if (curRenderer != null)
				{
					curRenderer.receiveShadows = m_bReceiveShadows ? true : false;
					curRenderer.shadowCastingMode = m_bShadowCasting ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;

					if (curRenderer.enabled == false)
					{
						curRenderer.enabled = true;
					}
				}
			}

			StartCoroutine(SetGrass());
		}

		//---------------------------------------------------------------------------------------------------
		IEnumerator SetGrass()
		{
			yield return null;
			SetInfinityGrass();
			yield return null;
			yield return null;
			SetInfinityGrass();
		}

		//---------------------------------------------------------------------------------------------------
		public void Full_reset()
		{
			if (Application.isPlaying)
			{
				threads_started = 0;
				threads_ended = 0;
				all_threads_started = false;

				Component[] filters = GetComponentsInChildren<MeshFilter>(true);//  GetComponentsInChildren(typeof(MeshFilter));
				for (int i = 0; i < filters.Length; i++)
				{
					if (filters[i].gameObject.name != "Combined mesh64" & filters[i].gameObject.name != "Combined mesh")
					{

						Renderer curRenderer = filters[i].gameObject.GetComponentsInChildren<Renderer>(true)[0];

						if (curRenderer != null && curRenderer.sharedMaterial != null && curRenderer.sharedMaterial.name.Contains("LOD") && curRenderer.enabled)
						{
							curRenderer.enabled = false;
						}
						else
						{
							curRenderer.enabled = true;
						}

					}
					else
					{
						DestroyImmediate(filters[i].gameObject);
					}
				}

				if (Destroy_list != null)
				{
					for (int i = 0; i < Destroy_list.Count; i++)
					{
						if (Destroy_list[i] != null)
						{
							DestroyImmediate(Destroy_list[i]);
						}
					}
					Destroy_list.Clear();
				}

				if (Destroy_list_MF != null)
				{
					for (int i = 0; i < Destroy_list_MF.Count; i++)
					{
						if (Destroy_list_MF[i] != null)
						{
							DestroyImmediate(Destroy_list_MF[i].gameObject);
						}
					}
					Destroy_list_MF.Clear();
				}

				if (Destroy_list_MF64 != null)
				{
					for (int i = 0; i < Destroy_list_MF64.Count; i++)
					{
						if (Destroy_list_MF64[i] != null)
						{
							DestroyImmediate(Destroy_list_MF64[i].gameObject);
						}
					}
					Destroy_list_MF64.Clear();
				}

				if (Destroy_list64 != null)
				{
					for (int i = 0; i < Destroy_list64.Count; i++)
					{
						if (Destroy_list64[i] != null)
						{
							DestroyImmediate(Destroy_list64[i]);
						}
					}
					Destroy_list64.Clear();
				}
				MeshList.Clear();
				MeshIDList.Clear();
				Material_list.Clear();
				Splits.Clear();
			}
		}

		//---------------------------------------------------------------------------------------------------
		void SetInfinityGrass()
		{
			Component[] filters = GetComponentsInChildren(typeof(MeshFilter));

			if (filters != null || 1 == 0)
			{
				if (filters.Length > 0 || 1 == 0)
				{
					Matrix4x4 myTransform = transform.worldToLocalMatrix;
					Hashtable materialToMesh = new Hashtable();

					int Group_start = 0;
					int Group_end = filters.Length;

					for (int i = Group_start; i < Group_end; i++)
					{
						MeshFilter filter = (MeshFilter)filters[i];

						if (filter.sharedMesh != null && filter.sharedMesh.vertexCount <= 32000 && filter.gameObject.name != "Combined mesh64")
						{
							Renderer curRenderer = filters[i].GetComponent<Renderer>();
							MeshCombineUtilityINfiniDyGrass.MeshInstance instance = new MeshCombineUtilityINfiniDyGrass.MeshInstance();
							instance.mesh = filter.sharedMesh;
							if (curRenderer != null && curRenderer.enabled && instance.mesh != null)
							{
								instance.transform = myTransform * filter.transform.localToWorldMatrix;

								Material[] materials = curRenderer.sharedMaterials;
								for (int m = 0; m < materials.Length; m++)
								{
									instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);

									ArrayList objects = (ArrayList)materialToMesh[materials[m]];
									if (objects != null)
									{
										objects.Add(instance);
									}
									else
									{
										objects = new ArrayList();
										objects.Add(instance);
										materialToMesh.Add(materials[m], objects);
									}
								}

								curRenderer.receiveShadows = m_bReceiveShadows ? true : false;
								curRenderer.shadowCastingMode = m_bShadowCasting ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;

							}
						}
					}

					int List_increase = 0;
					int counted_entries = 1;

					foreach (DictionaryEntry de in materialToMesh)
					{
						Material_list.Add((Material)de.Key);

						ArrayList elements = (ArrayList)de.Value;
						MeshCombineUtilityINfiniDyGrass.MeshInstance[] instances = (MeshCombineUtilityINfiniDyGrass.MeshInstance[])elements.ToArray(typeof(MeshCombineUtilityINfiniDyGrass.MeshInstance));

						List<int> Split_index = new List<int>();
						Split_index.Add(0);

						int vertexes_count = 0;
						for (int i = 0; i < instances.Length; i++)
						{

							vertexes_count = vertexes_count + instances[i].mesh.vertexCount;// filter.sharedMesh.vertexCount;

							if (vertexes_count > 64000)
							{
								vertexes_count = 0;
								Split_index.Add(i);
								Debug.Log("Split at =" + i);
							}
						}

						Splits.Add(Split_index.Count);
						for (int j = 0; j < Split_index.Count; j++)
						{

							if (j < Split_index.Count - 1)
							{
								Group_start = Split_index[j];
								Group_end = Split_index[j + 1] - 1;
							}
							else
							{
								Group_start = Split_index[j];
								Group_end = instances.Length;
							}

							MeshCombineUtilityINfiniDyGrass.MeshInstance[] instances_Split = new MeshCombineUtilityINfiniDyGrass.MeshInstance[Group_end - Group_start];
							for (int k = 0; k < (Group_end - Group_start); k++)
							{
								instances_Split[k] = instances[Group_start + k - 0];
							}

							if (!all_threads_started)
							{
								int vertexCount = 0;
								int triangleCount = 0;
								List<int> Combine_Mesh_vertexCount = new List<int>();
								List<Vector3[]> Combine_Mesh_vertices = new List<Vector3[]>();
								List<Vector3[]> Combine_Mesh_normals = new List<Vector3[]>();
								List<Vector4[]> Combine_Mesh_tangets = new List<Vector4[]>();

								List<Vector2[]> Combine_Mesh_uv = new List<Vector2[]>();
								List<Vector2[]> Combine_Mesh_uv1 = new List<Vector2[]>();
								List<Color[]> Combine_Mesh_colors = new List<Color[]>();
								List<int[]> Combine_Mesh_triangles = new List<int[]>();
								List<int> Has_mesh = new List<int>();

								int count = 0;
								foreach (MeshCombineUtilityINfiniDyGrass.MeshInstance combine in instances_Split)
								{
									if (combine.mesh)
									{
										vertexCount += combine.mesh.vertexCount;
									}

									Combine_Mesh_vertexCount.Add(combine.mesh.vertexCount);
									Combine_Mesh_vertices.Add(combine.mesh.vertices);
									Combine_Mesh_normals.Add(combine.mesh.normals);
									Combine_Mesh_tangets.Add(combine.mesh.tangents);

									Combine_Mesh_uv.Add(combine.mesh.uv);
									Combine_Mesh_uv1.Add(combine.mesh.uv2);
									Combine_Mesh_colors.Add(combine.mesh.colors);
									Combine_Mesh_triangles.Add(combine.mesh.GetTriangles(combine.subMeshIndex));

									count++;
								}

								foreach (MeshCombineUtilityINfiniDyGrass.MeshInstance combine in instances_Split)
								{
									if (combine.mesh)
									{
										triangleCount += combine.mesh.GetTriangles(combine.subMeshIndex).Length;
										Has_mesh.Add(1);
									}
									else
									{
										Has_mesh.Add(0);
									}
								}

								ControlCombineChildrenINfiniDyGrass.Meshy mesh = new ControlCombineChildrenINfiniDyGrass.Meshy();
								mesh.thread_ended = false;
								MeshList.Add(mesh);
								MeshIDList.Add(0); //signal this thread is not done yet

								int counter_mesh = List_increase;
								LoomINfiniDyGRASS.RunAsync(() =>
								{
									MeshList[counter_mesh] = MeshCombineUtilityINfiniDyGrass.CombineM(counter_mesh, Has_mesh, all_threads_started, instances_Split,
																							 true, vertexCount, triangleCount,
																							 Combine_Mesh_vertexCount, Combine_Mesh_vertices, Combine_Mesh_normals,
																							 Combine_Mesh_tangets, Combine_Mesh_uv, Combine_Mesh_uv1, Combine_Mesh_colors, Combine_Mesh_triangles);
								});

								List_increase++;
								threads_started++;

								if (counted_entries == materialToMesh.Count & j == (Split_index.Count - 1))
								{
									all_threads_started = true;
								}
							}
						}
						counted_entries++;
					}
				}
			}

			int threads_ended_real = 0;

			for (int i = 0; i < MeshIDList.Count; i++)
			{
				if (MeshList[i].thread_ended)
				{
					threads_ended_real++;
				}
			}

			if (threads_ended_real >= threads_started && all_threads_started && 1 == 1)
			{
				MeshFilter filter1 = this.gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
				if (filter1 != null)
				{
					Mesh meshD = filter1.sharedMesh;
					DestroyImmediate(meshD, true);
					DestroyImmediate(filter1, true);

				}
				else
				{
					if (Destroy_list.Count > 0)
					{
						for (int i = 0; i < Destroy_list.Count; i++)
						{
							MeshFilter filter11 = Destroy_list[i].GetComponent(typeof(MeshFilter)) as MeshFilter;
							if (filter11 != null)
							{
								Mesh meshD = filter11.sharedMesh;

								DestroyImmediate(meshD, true);
								DestroyImmediate(filter11, true);
							}
						}
						for (int i = Destroy_list.Count - 1; i >= 0; i--)
						{
							Destroy_list_MF.RemoveAt(i);
							DestroyImmediate(Destroy_list[i]);
							Destroy_list.RemoveAt(i);
						}
					}
				}

				Component[] filters2 = GetComponentsInChildren(typeof(MeshFilter));
				int Group_start = 0;
				int Group_end = filters2.Length;

				for (int i = Group_start; i < Group_end; i++)
				{
					MeshFilter filter = (MeshFilter)filters2[i];
					Renderer curRenderer = filters2[i].GetComponent<Renderer>();

					if (curRenderer != null && curRenderer.enabled)
					{
						if (filter.gameObject.name != "Combined mesh64")
						{
							curRenderer.enabled = false;
						}
					}
				}

				int mesh_counter = 0;

				for (int i = 0; i < Material_list.Count; i++)
				{
					for (int j = 0; j < Splits[i]; j++)
					{						
						if (MeshList.Count > mesh_counter && (MeshList[mesh_counter].vertices != null & MeshIDList[mesh_counter] != 1))
						{
							MeshIDList[mesh_counter] = 1;

							string name = "Combined meshKHS";
							if (MeshList[mesh_counter].vertices.Length > 32000)
							{
								name = "Combined mesh64";
							}

							GameObject go = new GameObject(name);
							go.layer = gameObject.layer;
							go.transform.parent = transform;
							go.transform.localScale = Vector3.one;
							go.transform.localRotation = Quaternion.identity;
							go.transform.localPosition = Vector3.zero;
							go.AddComponent(typeof(MeshFilter));

							MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
							meshRenderer.shadowCastingMode = m_bShadowCasting ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
							meshRenderer.receiveShadows = m_bReceiveShadows ? true : false;
							meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

							go.GetComponent<Renderer>().material = Material_list[i];//(Material)de.Key;
							MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));

							if (MeshList[mesh_counter].vertices.Length > 0)
							{
								filter.mesh.name = MeshList[mesh_counter].name;
								filter.mesh.vertices = MeshList[mesh_counter].vertices;
								filter.mesh.normals = MeshList[mesh_counter].normals;
								filter.mesh.colors = MeshList[mesh_counter].colors;
								filter.mesh.uv = MeshList[mesh_counter].uv;
								filter.mesh.uv2 = MeshList[mesh_counter].uv1;
								filter.mesh.tangents = MeshList[mesh_counter].tangents;
								filter.mesh.triangles = MeshList[mesh_counter].triangles;
								threads_ended = threads_ended + 1;
							}

							if (MeshList[mesh_counter].vertices.Length <= 32000)
							{
								Destroy_list.Add(go);
								Destroy_list_MF.Add(filter);
							}
							else
							{								
								Destroy_list64.Add(go);
								Destroy_list_MF64.Add(filter);
							}
						}
						mesh_counter++;
					}
				}

				Splits.Clear();
				Splits = new List<int>();
				Material_list.Clear();
				Material_list = new List<Material>();

				threads_ended = 0;
				threads_started = 0;
				MeshList.Clear();
				MeshList = new List<Meshy>();

				MeshIDList.Clear();
				MeshIDList = new List<int>();

				all_threads_started = false;
				mesh_counter = 0;
			}


			if (Destroy_list_MF64.Count > 0)
			{
				for (int i = 0; i < Destroy_list_MF64.Count; i++)
				{
					if (Destroy_list_MF64[i] != null)
					{
						if (Destroy_list_MF64[i].gameObject.GetComponent<Renderer>().sharedMaterial.name.Contains("LOD"))
						{
							Destroy_list_MF64[i].gameObject.SetActive(false);
						}
					}
				}
			}

			if (Destroy_list_MF.Count > 0)
			{
				for (int i = 0; i < Destroy_list_MF.Count; i++)
				{
					if (Destroy_list_MF[i] != null)
					{
						if (Destroy_list_MF[i].gameObject.GetComponent<Renderer>().sharedMaterial.name.Contains("LOD"))
						{
							Destroy_list_MF[i].gameObject.SetActive(false);
						}
					}
				}
			}
		}

		//---------------------------------------------------------------------------------------------------
		public void Restore()
		{
			Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@                         Restore()                           @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
			Start();

			MeshFilter filter1 = this.gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
			if (filter1 != null)
			{
				Mesh meshD = filter1.sharedMesh;

				DestroyImmediate(meshD, true);
				DestroyImmediate(filter1, true);
			}
			else
			{
				if (Destroy_list.Count > 0)
				{
					for (int i = 0; i < Destroy_list.Count; i++)
					{
						MeshFilter filter11 = Destroy_list[i].GetComponent(typeof(MeshFilter)) as MeshFilter;
						if (filter11 != null)
						{
							Mesh meshD = filter11.sharedMesh;

							DestroyImmediate(meshD, true);
							DestroyImmediate(filter11, true);
						}
					}
					for (int i = Destroy_list.Count - 1; i >= 0; i--)
					{
						Destroy_list_MF.RemoveAt(i);
						DestroyImmediate(Destroy_list[i]);
						Destroy_list.RemoveAt(i);
					}
				}

				if (Destroy_list64.Count > 0)
				{
					for (int i = 0; i < Destroy_list64.Count; i++)
					{
						if (Destroy_list_MF64[i] != null)
						{
							Mesh meshD = Destroy_list_MF64[i].sharedMesh;

							DestroyImmediate(meshD, true);
							DestroyImmediate(Destroy_list_MF64[i], true);
						}
					}

					for (int i = Destroy_list64.Count - 1; i >= 0; i--)
					{
						Destroy_list_MF64.RemoveAt(i);
						DestroyImmediate(Destroy_list64[i]);
						Destroy_list64.RemoveAt(i);
					}
				}
			}
		}
	}
}