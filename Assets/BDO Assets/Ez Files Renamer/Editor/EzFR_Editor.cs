using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System.IO;

public class EzFR_Editor : EditorWindow 
{
    #region ========== Draw ====================================================
    private void OnGUI()
    {
        RenameGUI();
        SortGUI();
        EzFR_Style.DisplayInformation("https://forum.unity3d.com/threads/ez-files-renamer.300182/");
    }
    #endregion ======= Draw ====================================================

    #region ========== Menu Items ==============================================
    [MenuItem("Window/BDO Assets/Ez Files Renamer")]
	public static void ShowWindow()
	{
		EditorWindow editorWindow = EditorWindow.GetWindow(typeof(EzFR_Editor));
		GUIContent _titleContent = new GUIContent("Ez Files Renamer");

		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.titleContent = _titleContent;
		editorWindow.Show();
	}

    // Sort Selection
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/Name A_Z", false, 49)]
    private static void MenuSortSelectNameA_Z() { SortSelection(SortOptions.nameA_Z, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/Name Z_A", false, 50)]
    private static void MenuSortSelectNameZ_A() { SortSelection(SortOptions.nameZ_A, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/X Axis 0_9", false, 51)]
    private static void MenuSortSelectAxisXAscending() { SortSelection(SortOptions.xAxis0_9, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/X Axis 9_0", false, 52)]
    private static void MenuSortSelectAxisXDescending() { SortSelection(SortOptions.xAxis9_0, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/Y Axis 0_9", false, 53)]
    private static void MenuSortSelectAxisYAscending() { SortSelection(SortOptions.yAxis0_9, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/Y Axis 9_0", false, 54)]
    private static void MenuSortSelectAxisYDescending() { SortSelection(SortOptions.yAxis9_0, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/Z Axis 0_9", false, 55)]
    private static void MenuSortSelectAxisZAscending() { SortSelection(SortOptions.zAxis0_9, false); }
    [MenuItem("Tools/Ez Files Renamer/Sort Selection/Z Axis 9_0", false, 56)]
    private static void MenuSortSelectAxisZDescending() { SortSelection(SortOptions.zAxis9_0, false); }

    // Sort Children
    [MenuItem("Tools/Ez Files Renamer/Sort Children/Name A_Z", false, 49)]
    private static void MenuSortChildNameA_Z() { SortSelection(SortOptions.nameA_Z, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/Name Z_A", false, 50)]
    private static void MenuSortChildNameZ_A() { SortSelection(SortOptions.nameZ_A, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/X Axis 0_9", false, 51)]
    private static void MenuSortChildAxisXAscending() { SortSelection(SortOptions.xAxis0_9, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/X Axis 9_0", false, 52)]
    private static void MenuSortChildAxisXDescending() { SortSelection(SortOptions.xAxis9_0, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/Y Axis 0_9", false, 53)]
    private static void MenuSortChildAxisYAscending() { SortSelection(SortOptions.yAxis0_9, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/Y Axis 9_0", false, 54)]
    private static void MenuSortChildAxisYDescending() { SortSelection(SortOptions.yAxis9_0, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/Z Axis 0_9", false, 55)]
    private static void MenuSortChildAxisZAscending() { SortSelection(SortOptions.zAxis0_9, true); }
    [MenuItem("Tools/Ez Files Renamer/Sort Children/Z Axis 9_0", false, 56)]
    private static void MenuSortChildAxisZDescending() { SortSelection(SortOptions.zAxis9_0, true); }
    #endregion ======= Menu Items ==============================================

	#region ========== Rename ==================================================
    private enum SeparatedBy
    {
        nothing,
        hyphen,
        point,
        space_,
        underline,
        custom
    }
    private enum SequenceOptions
    {
        onEnd,
        onBegin,
        betweenPrefixAndName,
        betweenSuffixAndName
    }

    private SeparatedBy prefixSeparatedBy = SeparatedBy.nothing;
    private SeparatedBy suffixSeparatedBy = SeparatedBy.nothing;
    private SeparatedBy sequentialSeparatedBy = SeparatedBy.nothing;
    SequenceOptions sequenceOptions = SequenceOptions.onEnd;

    // Name
    private bool usePrefix;
    private bool useSuffix;
    private string filePrefix = "";
    private string fileSuffix;
    private string fileNewName;
    private string fileSeparator = "";
    private string prefixCustomSeparator = "";
    private string suffixCustomSeparator = "";
    // Make Sequencial
    private bool makeSequential;
    private int sequentialInitalNumber = 0;
    private int sequentialNumber = 0;
    private string sequentialCustomSeparator;

	private void RenameGUI()
	{
		EzFR_Style.Header("Rename", false, false);
        EditorGUI.indentLevel = 1;
		// New Name
		fileNewName = EditorGUILayout.TextField("New Name", fileNewName);
        EditorGUILayout.Space();

		// Prefix
        usePrefix = EditorGUILayout.Toggle("Use Prefix", usePrefix);
		if(usePrefix)
		{
			filePrefix = EditorGUILayout.TextField("Prefix", filePrefix);
			prefixSeparatedBy = (SeparatedBy)EditorGUILayout.EnumPopup("Separated By", prefixSeparatedBy);
			if(prefixSeparatedBy == SeparatedBy.custom)
				prefixCustomSeparator = EditorGUILayout.TextField("Custom Separator", prefixCustomSeparator);
			else
				prefixCustomSeparator = "";
            EditorGUILayout.Space();
		}

        // Suffix
        useSuffix = EditorGUILayout.Toggle("Use Suffix", useSuffix);
        if(useSuffix)
        {
            fileSuffix = EditorGUILayout.TextField("Suffix", fileSuffix);
            suffixSeparatedBy = (SeparatedBy)EditorGUILayout.EnumPopup("Separated By", suffixSeparatedBy);
            if (suffixSeparatedBy == SeparatedBy.custom)
                suffixCustomSeparator = EditorGUILayout.TextField("Custom Separator", suffixCustomSeparator);
            else
                suffixCustomSeparator = "";
            EditorGUILayout.Space();
        }
			
		// Make it Sequential
        makeSequential = EditorGUILayout.Toggle("Make Sequential", makeSequential);
		if(makeSequential)
		{
			sequenceOptions = (SequenceOptions)EditorGUILayout.EnumPopup("Number Goes", sequenceOptions);
			sequentialInitalNumber = EditorGUILayout.IntField("Initial Number", sequentialInitalNumber);
			sequentialSeparatedBy = (SeparatedBy)EditorGUILayout.EnumPopup("Separated By", sequentialSeparatedBy);

			if(sequentialSeparatedBy == SeparatedBy.custom)
				sequentialCustomSeparator = EditorGUILayout.TextField("Custom Separator", sequentialCustomSeparator);
			else
				sequentialCustomSeparator = "";
            EditorGUILayout.Space();
		}

        if(GUILayout.Button("Rename On Hierarchy", GUILayout.Height(25)))
			DoRenameHierarchy();
        if(GUILayout.Button("Rename On Project Folder", GUILayout.Height(25)))
			DoRenameFilesFolder();

		EzFR_Style.Footer();
	}

	private void DoRenameHierarchy()
	{
		GameObject[] _gameObjectsSelected = Selection.gameObjects;

		if(!RenameCheckErrorsToContinue(_gameObjectsSelected, null))
			return;	
			
		// Sort the gameobjects inside the array based on the siblin index
		System.Array.Sort(_gameObjectsSelected, delegate(GameObject tempSelection0, GameObject tempSelection1) {
			return EditorUtility.NaturalCompare(tempSelection0.transform.GetSiblingIndex().ToString(), tempSelection1.transform.GetSiblingIndex().ToString());
		});
			
		string _filePrefix = GetPrefix();
        string _fileSuffix = GetSuffix();
		sequentialNumber = sequentialInitalNumber;
		// Calculate the amount that each file will increase in the progress bar
		float _result = (float)_gameObjectsSelected.Length / 100f;

		for (int i = 0; i < _gameObjectsSelected.Length; i++) 
		{
            EditorUtility.DisplayProgressBar(EzFR_Messages.TITLE_00, EzFR_Messages.MESSAGE_00, _result * i);

			if(makeSequential)
			{
				switch(sequenceOptions)
				{
				case SequenceOptions.onEnd:
                        _gameObjectsSelected[i].name = _filePrefix + fileNewName + _fileSuffix + GetSeparationType(sequentialSeparatedBy) + sequentialNumber;
					break;

				case SequenceOptions.onBegin:
                        _gameObjectsSelected[i].name = sequentialNumber.ToString() + GetSeparationType(sequentialSeparatedBy) + _filePrefix + fileNewName + _fileSuffix;
					break;

				case SequenceOptions.betweenPrefixAndName:
                        _gameObjectsSelected[i].name = _filePrefix + sequentialNumber + GetSeparationType(sequentialSeparatedBy) + fileNewName + _fileSuffix;
					break;

                case SequenceOptions.betweenSuffixAndName:
                        _gameObjectsSelected[i].name = _filePrefix + fileNewName + GetSeparationType(sequentialSeparatedBy) + sequentialNumber + _fileSuffix;
                    break;
				}

				sequentialNumber++;
			}
			else
                _gameObjectsSelected[i].name = _filePrefix + fileNewName + _fileSuffix;
		}

		EditorUtility.ClearProgressBar();
	}

	private void DoRenameFilesFolder()
	{
		Object[] _objectsSelected = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

		if(!RenameCheckErrorsToContinue(null, _objectsSelected))
			return;

		// Sort the gameobjects inside the array based on name
		System.Array.Sort(_objectsSelected, delegate(Object objectSelected0, Object objectSelected1) {
			return EditorUtility.NaturalCompare(objectSelected0.name, objectSelected1.name);
		});

		sequentialNumber = sequentialInitalNumber;
		string _path; // Keep the path of the current file

		// Reset the obj name to prevent conflict, because in the project folder it is not permited 2 files with the same name
		foreach(Object obj in _objectsSelected)
		{
			_path = AssetDatabase.GetAssetPath(obj);
			AssetDatabase.RenameAsset(_path, sequentialNumber.ToString());
			sequentialNumber++;
		}
			
		string _filePrefix = GetPrefix();
        string _fileSuffix = GetSuffix();

		// Calculate the amount that each file will increase in the progress bar
		float _result = (float)_objectsSelected.Length / 100f;

		if(!makeSequential)
		{
			makeSequential = true;
			EditorUtility.DisplayDialog("Attention!", EzFR_Messages.WARNING_01, "Continue");
			sequentialInitalNumber = 0;
		}
		sequentialNumber = sequentialInitalNumber;

		// Rename the files
		for (int i = 0; i < _objectsSelected.Length; i++) 
		{
			EditorUtility.DisplayProgressBar("Renaming", "Wait until the files are renamed...", _result * i);
			_path = AssetDatabase.GetAssetPath(_objectsSelected[i]);

			switch(sequenceOptions)
			{
			case SequenceOptions.onEnd:
                    AssetDatabase.RenameAsset(_path, _filePrefix + fileNewName + _fileSuffix + GetSeparationType(sequentialSeparatedBy) + sequentialNumber);
				break;

			case SequenceOptions.onBegin:
                    AssetDatabase.RenameAsset(_path, sequentialNumber.ToString() + GetSeparationType(sequentialSeparatedBy) + _filePrefix + fileNewName + _fileSuffix);
				break;

			case SequenceOptions.betweenPrefixAndName:
                    AssetDatabase.RenameAsset(_path, _filePrefix + sequentialNumber + GetSeparationType(sequentialSeparatedBy) + fileNewName + _fileSuffix);
				break;

            case SequenceOptions.betweenSuffixAndName:
                    AssetDatabase.RenameAsset(_path, _filePrefix + fileNewName + GetSeparationType(sequentialSeparatedBy) + sequentialNumber + _fileSuffix);
                break;
			}

			sequentialNumber++;
		}

		EditorUtility.ClearProgressBar();
	}

	private bool RenameCheckErrorsToContinue(GameObject[] gameObjectSelection, Object[] objectsSelected)
	{
		if(gameObjectSelection != null)
		{
			// Verify if there's at least one gameobject selected
			if(gameObjectSelection.Length <= 0)
			{
                EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_00, EzFR_Messages.BUTTON_00);
				return false;
			}

			// Verify if all selected gameobjects has the same parent
			List<Transform> _parents = new List<Transform>();
			for (int i = 0; i < gameObjectSelection.Length; i++) 
			{
				if(!_parents.Contains(gameObjectSelection[i].transform.parent))
					_parents.Add(gameObjectSelection[i].transform.parent);

				if(_parents.Count > 1)
				{
                    if(EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.WARNING_00, EzFR_Messages.BUTTON_02, EzFR_Messages.BUTTON_01))
						return true;
					else
						return false;
				}
			}
		}
		else if(objectsSelected != null)
		{
			if(objectsSelected.Length <= 0)
			{
                EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_01, EzFR_Messages.BUTTON_00);
				return false;
			}

			// Verify if all the files are the same type
			List<string> _types = new List<string>();
			for (int i = 0; i < objectsSelected.Length; i++) 
			{
				if(!_types.Contains(Path.GetExtension(AssetDatabase.GetAssetPath(objectsSelected[i])).ToString()))
					_types.Add(Path.GetExtension(AssetDatabase.GetAssetPath(objectsSelected[i])).ToString());

				if(_types.Count > 1)
				{
                    EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_02, EzFR_Messages.BUTTON_00);
					return false;
				}
			}

			// Verify the file extension
			if(string.Equals(_types[0], ".cs") || string.Equals(_types[0], ".js") || string.Equals(_types[0], ".shader"))
			{
                EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_03, EzFR_Messages.BUTTON_00);
				return false;
			}
		}
		else
		{
            EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_06, EzFR_Messages.BUTTON_00);
			return false;
		}
			
		return true;
	}

	private string GetPrefix()
	{
        return (usePrefix) ? filePrefix + GetSeparationType(prefixSeparatedBy, "prefix") : "";
	}

    private string GetSuffix()
    {
        return (useSuffix) ? GetSeparationType(suffixSeparatedBy, "suffix") + fileSuffix : "";
    }

	private string GetSeparationType(SeparatedBy separatedBy, string prefixOrSuffix = "")
	{
		switch(separatedBy)
		{
		case SeparatedBy.nothing:
			fileSeparator = "";
			break;

		case SeparatedBy.hyphen:
			fileSeparator =  "-";
			break;

		case SeparatedBy.point:
			fileSeparator = ".";
			break;

		case SeparatedBy.space_:
			fileSeparator = " ";
			break;

		case SeparatedBy.underline:
			fileSeparator = "_";
			break;

		case SeparatedBy.custom:
            if (string.Equals(prefixOrSuffix, "prefix"))
                fileSeparator = prefixCustomSeparator;
            else if (string.Equals(prefixOrSuffix, "suffix"))
                fileSeparator = suffixCustomSeparator;
			else
				fileSeparator = sequentialCustomSeparator;
			break;
		}

		return fileSeparator;
	}
	#endregion ========== Rename ===============================================

	#region ========== Sort ====================================================
    private enum SortOptions
    {
        nameA_Z,
        nameZ_A,
        xAxis0_9,
        xAxis9_0,
        yAxis0_9,
        yAxis9_0,
        zAxis0_9,
        zAxis9_0
    }

    private SortOptions sortOption = SortOptions.nameA_Z;

    private bool sortChildren; // This option will be true when only one gameobject is select and if it has children

	private void SortGUI()
	{
		EzFR_Style.Header("Sort", false, false);
        EditorGUI.indentLevel = 1;
		sortOption = (SortOptions)EditorGUILayout.EnumPopup("Sort Option", sortOption);

        if(GUILayout.Button("Sort Selection", GUILayout.Height(25)))
			SortSelection(this.sortOption);
        if (GUILayout.Button("Sort Children", GUILayout.Height(25)))
            SortSelection(this.sortOption, true);

		EzFR_Style.Footer();
	}

	private static void SortSelection(SortOptions sortOption, bool sortChildren = false)
	{
		// Keep the selected gameobjects
		List<GameObject> _tempGameobjectsSelected = new List<GameObject>();
        // Keep the sibling index of each gameobject
        List<int> _gameObjectsSiblingIndex = new List<int>();

		if(!sortChildren)
		{
			foreach(GameObject gameObj in Selection.gameObjects)
				_tempGameobjectsSelected.Add(gameObj);
		}
		else
		{
			if(Selection.gameObjects.Length > 1)
			{
                EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_05, EzFR_Messages.BUTTON_00);
				return;
			}
				
			for (int i = 0; i < Selection.gameObjects[0].transform.childCount; i++)
				_tempGameobjectsSelected.Add(Selection.gameObjects[0].transform.GetChild(i).gameObject);
		}

		GameObject[] _gameobjectsSelected = _tempGameobjectsSelected.ToArray();

		if(!SortCheckErrorsToContinue(_gameobjectsSelected))
            return;

        // Get the sibling index of each game object. 
        // I'm using for because when using foreach the siblins are storage out of order.
        for (int i = 0; i < _gameobjectsSelected.Length; i++)
            _gameObjectsSiblingIndex.Add(_gameobjectsSelected[i].transform.GetSiblingIndex());
        // Sort the list to organize the values from the lowest to biggest
        _gameObjectsSiblingIndex.Sort();
        //for (int i = 0; i < _gameObjectsSiblingIndex.Count(); i++)
            //Debug.Log(_gameObjectsSiblingIndex[i]);


        // Sort the array based on the sort option
        if(sortOption == SortOptions.nameA_Z || sortOption == SortOptions.nameZ_A)
		{
			System.Array.Sort(_gameobjectsSelected, delegate(GameObject tempObjTrans0, GameObject tempObjTrans1) {
				return EditorUtility.NaturalCompare(tempObjTrans0.name, tempObjTrans1.name);
			});
		}
		else if(sortOption == SortOptions.xAxis0_9 || sortOption == SortOptions.xAxis9_0)
		{
			System.Array.Sort(_gameobjectsSelected, delegate(GameObject object0, GameObject object1) {
				return EditorUtility.NaturalCompare(object0.transform.position.x.ToString(), object1.transform.position.x.ToString());
			});
		}
		else if(sortOption == SortOptions.yAxis0_9 || sortOption == SortOptions.yAxis9_0)
		{
			System.Array.Sort(_gameobjectsSelected, delegate(GameObject object0, GameObject object1) {
				return EditorUtility.NaturalCompare(object0.transform.position.y.ToString(), object1.transform.position.y.ToString());
			});
		}
		else
		{
			System.Array.Sort(_gameobjectsSelected, delegate(GameObject object0, GameObject object1) {
				return EditorUtility.NaturalCompare(object0.transform.position.z.ToString(), object1.transform.position.z.ToString());
			});
		}

		// Calculate the amount that each file will increase in the progress bar
		float _result = (float)_gameobjectsSelected.Length / 100f;
			
		// Set the new sibling index
        if(sortOption == SortOptions.nameA_Z || 
			sortOption == SortOptions.xAxis0_9 ||
			sortOption == SortOptions.yAxis0_9 ||
			sortOption == SortOptions.zAxis0_9) 
		{
			for (int i = 0; i < _gameobjectsSelected.Length; i++) 
			{
                EditorUtility.DisplayProgressBar(EzFR_Messages.TITLE_02, EzFR_Messages.MESSAGE_01, _result * i);
				//_gameobjectsSelected[i].transform.SetSiblingIndex(i);
                _gameobjectsSelected[i].transform.SetSiblingIndex(_gameObjectsSiblingIndex[i]);
                //Debug.Log(_gameObjectsSiblingIndex[i]);
			}
				
		}
		else
		{
			for (int i = 0; i < _gameobjectsSelected.Length; i++) 
			{
                EditorUtility.DisplayProgressBar(EzFR_Messages.TITLE_02, EzFR_Messages.MESSAGE_01, _result * i);
                //int _index = (_gameobjectsSelected.Length - i) - 1;
                int _index = _gameObjectsSiblingIndex[(_gameObjectsSiblingIndex.Count() - 1) - i];
                //Debug.Log(_index);
				_gameobjectsSelected[i].transform.SetSiblingIndex(_index);
			}
		}

		EditorUtility.ClearProgressBar();
	}

	private static bool SortCheckErrorsToContinue(GameObject[] gameobjectsSelected)
	{
		if(gameobjectsSelected.Length <= 0)
		{
            EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_00, EzFR_Messages.BUTTON_00);
			return false;
		}

		// Verify if all selected gameobjects has the same parent
		List<Transform> _parents = new List<Transform>();
		for (int i = 0; i < gameobjectsSelected.Length; i++) 
		{
			if(!_parents.Contains(gameobjectsSelected[i].transform.parent))
				_parents.Add(gameobjectsSelected[i].transform.parent);

			if(_parents.Count > 1)
			{
                EditorUtility.DisplayDialog(EzFR_Messages.TITLE_01, EzFR_Messages.ERROR_04, EzFR_Messages.BUTTON_00);
				return false;
			}
		}

		return true;
	}
	#endregion ========== Sort =================================================
}