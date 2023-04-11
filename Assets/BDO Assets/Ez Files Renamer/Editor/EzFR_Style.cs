using UnityEditor;
using UnityEngine;

public class EzFR_Style : Editor 
{
    public static Rect rect;

    public static bool Header(string title, bool useToggle, bool toggle)
    {
        rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);
        EditorGUILayout.Space();
        EditorGUI.indentLevel = 0;

        if (useToggle)
            toggle = EditorGUILayout.ToggleLeft(title, toggle, EditorStyles.boldLabel);
        else
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

        return toggle;
    }

    public static void Footer()
    {
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    public static void DisplayInformation(string forumLink)
    {
        BeginVerticalRectBox();
        Header("Information", false, false);

        GUILayout.Space(10);

        if (GUILayout.Button("BDO Assets"))
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:11524");
        if (GUILayout.Button("Support forum"))
            Application.OpenURL(forumLink);
        if (GUILayout.Button("Developer Website"))
            Application.OpenURL("http://betodeoliveira.com");

        Footer();
        EndVerticalRectBox();
    }

    #region ========== Rect ============================================================================================
    public static void BeginHorizontalRectBox()
    {
        rect = EditorGUILayout.BeginHorizontal();
        GUI.Box(rect, GUIContent.none);
    }

    public static void BeginVerticalRectBox()
    {
        rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);
    }

    public static void EndHorizontalRectBox() { EditorGUILayout.EndHorizontal(); }
    public static void EndVerticalRectBox() { EditorGUILayout.EndVertical(); }
    #endregion ========== Rect =========================================================================================
}