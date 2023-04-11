using System.Collections;
using UnityEngine;
using UnityEditor;


public class RotationTool : EditorWindow{

	enum RotationAxis { x , y , z };

	static RotationTool myWindow ;
	GameObject Current ;
	Mesh mesh;
	bool IsCurrentVailed = false ;
	float RadialOfRotation ;
	float DegreeOfRotaion ;
	float dor {
		set {
			RadialOfRotation = value / 180 * Mathf.PI;
			DegreeOfRotaion = value;
		}
		get {
			return DegreeOfRotaion;
		}
	}

	[MenuItem("Window/Rotation Tool")]
	static void OpenWindow (){
		myWindow = GetWindow<RotationTool> ();
	}

	void OnAwake (){
		myWindow.position = new Rect ( Event.current.mousePosition , new Vector2(70,70));
	}

	void OnDestroy (){
		myWindow.position = new Rect ( Event.current.mousePosition , new Vector2(70,70));
	}

	void OnGUI () {

		Current = Selection.activeGameObject;

		if (Current != null && Current.GetComponent<MeshFilter> () != null)
			IsCurrentVailed = true;
		else {
			IsCurrentVailed = false;
			mesh = null ;
		}
		
		dor = EditorGUILayout.FloatField ("degree of rotation", dor);

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("X+")) {
			if (IsCurrentVailed) {
				mesh = Current.GetComponent<MeshFilter> ().sharedMesh;
				mesh = RotateMesh (mesh, RotationAxis.x, RadialOfRotation);
			}
		}
		if (GUILayout.Button ("Y+")) {
			if (IsCurrentVailed) {
				mesh = Current.GetComponent<MeshFilter> ().sharedMesh;
				mesh = RotateMesh (mesh, RotationAxis.y, RadialOfRotation);
			}
		}
		if (GUILayout.Button ("Z+")) {
			if (IsCurrentVailed) {
				mesh = Current.GetComponent<MeshFilter> ().sharedMesh;
				mesh = RotateMesh (mesh, RotationAxis.z, RadialOfRotation);
			}
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("X-")) {
			if (IsCurrentVailed) {
				mesh = Current.GetComponent<MeshFilter> ().sharedMesh;
				mesh = RotateMesh (mesh, RotationAxis.x, -RadialOfRotation);
			}
		}
		if (GUILayout.Button ("Y-")) {
			if (IsCurrentVailed) {
				mesh = Current.GetComponent<MeshFilter> ().sharedMesh;
				mesh = RotateMesh (mesh, RotationAxis.y, -RadialOfRotation);
			}
		}
		if (GUILayout.Button ("Z-")) {
			if (IsCurrentVailed) {
				mesh = Current.GetComponent<MeshFilter> ().sharedMesh;
				mesh = RotateMesh (mesh, RotationAxis.z, -RadialOfRotation);
			}
		}
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("Save")) {
			SaveMesh (mesh);
		}

		if ( Current == null ){
			EditorGUILayout.HelpBox ("Select object to rotate with MeshFilter attach to it", MessageType.Info);
		}else{
			if (Current.GetComponent<MeshFilter> () == null)
				EditorGUILayout.HelpBox ("Select object to rotate with a MeshFilter attached to it.", MessageType.Info);
		}
			
	}

	void SaveMesh ( Mesh m ){
		Mesh newMesh = Instantiate (m);
		MeshUtility.Optimize (newMesh);
		string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
		path = FileUtil.GetProjectRelativePath(path);
		AssetDatabase.CreateAsset(newMesh, path);
		AssetDatabase.SaveAssets();
		Object CurrentPref = PrefabUtility.GetPrefabParent (Current);
		Current.GetComponent<MeshFilter> ().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh> (path);
		PrefabUtility.ReplacePrefab (Current , CurrentPref);
		UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty ();
	}

	Mesh RotateMesh ( Mesh M , RotationAxis Axis , float DOR ){

		Vector3[] vertices = M.vertices;
		Vector3[] normals = M.normals;

		switch (Axis) {
		case RotationAxis.x:
			{
				for (int i = 0; i < vertices.Length; i++) {
					float VerY = (Mathf.Cos (DOR) * vertices [i].y) - (Mathf.Sin (DOR) * vertices [i].z);
					float VerZ = (Mathf.Sin (DOR) * vertices [i].y) + (Mathf.Cos (DOR) * vertices [i].z);
					vertices [i] = new Vector3 (vertices [i].x, VerY, VerZ);
					float NorY = (Mathf.Cos (DOR) * normals [i].y) - (Mathf.Sin (DOR) * normals [i].z);
					float NorZ = (Mathf.Sin (DOR) * normals [i].y) + (Mathf.Cos (DOR) * normals [i].z);
					normals [i] = new Vector3 (normals [i].x, NorY, NorZ);
				}
				break;
			}
		case RotationAxis.y:
			{
				for (int i = 0; i < vertices.Length; i++) {
					float VerX = (Mathf.Cos (DOR) * vertices [i].x) - (Mathf.Sin (DOR) * vertices [i].z);
					float VerZ = (Mathf.Sin (DOR) * vertices [i].x) + (Mathf.Cos (DOR) * vertices [i].z);
					vertices [i] = new Vector3 (VerX, vertices [i].y, VerZ);
					float NorX = (Mathf.Cos (DOR) * normals [i].x) - (Mathf.Sin (DOR) * normals [i].z);
					float NorZ = (Mathf.Sin (DOR) * normals [i].x) + (Mathf.Cos (DOR) * normals [i].z);
					normals [i] = new Vector3 (NorX, normals [i].y, NorZ);
				}
				break;
			}
		case RotationAxis.z:
			{
				for (int i = 0; i < vertices.Length; i++) {
					float VerX = (Mathf.Cos (DOR) * vertices [i].x) - (Mathf.Sin (DOR) * vertices [i].y);
					float VerY = (Mathf.Sin (DOR) * vertices [i].x) + (Mathf.Cos (DOR) * vertices [i].y);
					vertices [i] = new Vector3 (VerX, VerY, vertices [i].z);
					float NorX = (Mathf.Cos (DOR) * normals [i].x) - (Mathf.Sin (DOR) * normals [i].y);
					float NorY = (Mathf.Sin (DOR) * normals [i].x) + (Mathf.Cos (DOR) * normals [i].y);
					normals [i] = new Vector3 (NorX, NorY, normals [i].z);
				}
				break;
			}
		}

		M.vertices = vertices;
		M.normals = normals;
		M.RecalculateBounds ();

		return M;

	}

}
