#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

//// Live
//ReleaseName = "";
//Version = "";
//AssetBundleVersion = "";

//// Stage
//ReleaseName = "";
//Version = "";
//AssetBundleVersion = "";

//// Dev
//ReleaseName = "";
//Version = "";
//AssetBundleVersion = "";

// dev
//private string	m_sAuthServerApiUrl	= "http://gate-sdev.rappelzrift.mobblo.com/Api.ashx";
// Stage
//private string	m_sAuthServerApiUrl	= "http://gate-devstg.rappelzrift.mobblo.com/Api.ashx";

// ServiceCode ( KR = 1m PH = 2 )
// ServerType ( Dev = 1, Stage = 2, Live = 3)
// PlatformID { Android = 1, IOS = 2 }
// ConnectMode { UNITY_ONLY = 1, WITH_SDK = 2 }
// Store Type (playstore : onestore : appstore)

public enum StoreType
{
	playstore,
	onestore,
	appstore
}

public enum CountryType
{
	Korea,
	Philippines
}

public class BuildAuthSwitcherEditor : EditorWindow
{
    [SerializeField] private string _serverType = "";
    [SerializeField] private string _serviceCode = "";    
    [SerializeField] private string _bulidNumber = "";
    [SerializeField] private string _version = "";
    [SerializeField] private string _versionName = "";
    [SerializeField] private string _bundleIdentifier = "";

    [MenuItem("Tools/Build/Build Auth Settings")]
    static public void ShowWindow()
    {
        BuildAuthSwitcherEditor window = EditorWindow.GetWindow<BuildAuthSwitcherEditor>();
        window.Initialize();
    }
    
    public void Initialize()
    {
        _serverType = AuthSettings.ServerType;
        _serviceCode = AuthSettings.ServiceCode;
        _bulidNumber = AuthSettings.BulidNumber;
        _version = AuthSettings.Version;
        _versionName = AuthSettings.VersionName;
        _bundleIdentifier = Application.identifier;
    }

    [MenuItem("Assets/Create/AuthSettings Preset")]
    public static void CreateAuthPreset()
    {
        AuthSettingsPreset asset = ScriptableObject.CreateInstance<AuthSettingsPreset>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(AuthSettingsPreset).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    
    void OnGUI()
    {
        // Auth 설정 프로퍼티 설정 영역 
        EditorGUILayout.LabelField("Build Auth Settings (Android)", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        string ServerType = EditorGUILayout.TextField("ServerType", _serverType);
        if (GUI.changed)
        {
            Undo.RecordObject(this, "ServerType Changed");
            _serverType = ServerType;
        }

        string ServiceCode = EditorGUILayout.TextField("ServiceCode", _serviceCode);
        if (GUI.changed)
        {
            Undo.RecordObject(this, "ServiceCode Changed");
            _serviceCode = ServiceCode;
        }

        string BulidNumber = EditorGUILayout.TextField("Asset Bundle Version", _bulidNumber);
        if (GUI.changed)
        {
            Undo.RecordObject(this, "BulidNumber Changed");
            _bulidNumber = BulidNumber;
        }

        string version = EditorGUILayout.TextField("Vesion", _version);
        if (GUI.changed)
        {
            Undo.RecordObject(this, "version Changed");
            _version = version;
        }

        string versionName = EditorGUILayout.TextField("VersionName", _versionName);
        if (GUI.changed)
        {
            Undo.RecordObject(this, "versionName Changed");
            _versionName = versionName;
        }

        string bundleIdentifier = EditorGUILayout.TextField("Bundle Identifier", _bundleIdentifier);
        if (GUI.changed)
        {
            Undo.RecordObject(this, "bundleIdentifier Changed");
            _bundleIdentifier = bundleIdentifier;
        }

		EditorGUI.indentLevel--;
        // Auth 설정 프로퍼티 설정 영역 끝.

        // Preset 불러오기 영역
        EditorGUILayout.LabelField("Presets", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Dev"))
        {
            Undo.RecordObject(this, "Changed By Dev Preset");
            SetAuthSettingsEditor("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/Dev Settings.asset");
        } else if (GUILayout.Button("Stage"))
		{
			Undo.RecordObject(this, "Changed By Dev Preset");
			SetAuthSettingsEditor("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/Stage Settings.asset");
		}
		else if (GUILayout.Button("RealStage"))
		{
			Undo.RecordObject(this, "Changed By Dev Preset");
			SetAuthSettingsEditor("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/RealStage Settings.asset");
		}
		else if (GUILayout.Button("BalancesStage"))
		{
			Undo.RecordObject(this, "Changed By Dev Preset");
			SetAuthSettingsEditor("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/BalancesStage Settings.asset");
		}

		EditorGUILayout.EndHorizontal();
        // Preset 불러오기 영역 끝.
        // Preset 수정하기 영역
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit Dev"))
        {
            OpenAuthSettingsPresetAsset("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/Dev Settings.asset");
        }
		else if (GUILayout.Button("Edit Stage"))
		{
			OpenAuthSettingsPresetAsset("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/Stage Settings.asset");
		}
		else if (GUILayout.Button("Edit RealStage"))
		{
			OpenAuthSettingsPresetAsset("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/RealStage Settings.asset");
		}
		else if (GUILayout.Button("Edit BalancesStage"))
		{
			OpenAuthSettingsPresetAsset("Assets/Scripts/2. UI/Auth/System/AuthSettingsPreset/BalancesStage Settings.asset");
		}

		EditorGUILayout.EndHorizontal();

        // 스토어 타입 적용하기
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Setting StoreType", EditorStyles.boldLabel);
        //if (GUILayout.Button("PlayStore", GUILayout.Height(30)))
        //{
        //    _storeType = "playstore";

        //    if (_bundleIdentifier == "com.entermate.rapplz.one.stage")
        //    {
        //        _bundleIdentifier = "com.entermate.rapplz.stage";
        //    }
        //    else if (_bundleIdentifier == "com.entermate.rapplz.one.deploy")
        //    {
        //        _bundleIdentifier = "com.entermate.rapplz.deploy";
        //    }
        //}
        //if (GUILayout.Button("OneStore", GUILayout.Height(30)))
        //{
        //    _storeType = "onestore";

        //    if (_bundleIdentifier == "com.entermate.rapplz.stage")
        //    {
        //        _bundleIdentifier = "com.entermate.rapplz.one.stage";
        //    }
        //    else if (_bundleIdentifier == "com.entermate.rapplz.deploy")
        //    {
        //        _bundleIdentifier = "com.entermate.rapplz.one.deploy";
        //    }
        //}
        //if (GUILayout.Button("AppStore", GUILayout.Height(30)))
        //{
        //    _storeType = "appstore";
        //}
        //EditorGUILayout.EndHorizontal();

		// 해외 세팅
		//EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.LabelField("Setting Country", EditorStyles.boldLabel);
		//EditorGUILayout.EndHorizontal();
		//EditorGUILayout.BeginHorizontal();
		//if (GUILayout.Button(CountryType.Korea.ToString(), GUILayout.Height(30), GUILayout.Width(200)))
		//{
		//    CountrySetting(CountryType.Korea);
		//}
		//if (GUILayout.Button(CountryType.Philippines.ToString(), GUILayout.Height(30), GUILayout.Width(200)))
		//{
		//    CountrySetting(CountryType.Philippines);
		//}
		//EditorGUILayout.EndHorizontal();

        // Preset 수정하기 영역 끝.
        // 세팅 적용 영역

        EditorGUILayout.LabelField("Apply Settings", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Apply Settings", GUILayout.Height(50)))
        {
            SwitchAuthSettings(_serverType, _serviceCode, _bulidNumber, _version, _versionName, _bundleIdentifier);
        }

        // 세팅 적용 영역 끝.

    }

    private void SetAuthSettingsEditor(string authSettingsPresetPath)
    {
        AuthSettingsPreset preset = AssetDatabase.LoadAssetAtPath<AuthSettingsPreset>(authSettingsPresetPath);

        _serverType = preset.ServerType;
        _serviceCode = preset.ServiceCode;
        _bulidNumber = preset.BulidNumber;
        _version = preset.Version;
        _versionName = preset.VersionName;
        _bundleIdentifier = preset.BundleIdentifier;
		Repaint();
    }

    private void OpenAuthSettingsPresetAsset(string authSettingsPresetPath)
    {
        AuthSettingsPreset preset = AssetDatabase.LoadAssetAtPath<AuthSettingsPreset>(authSettingsPresetPath);
        AssetDatabase.OpenAsset(preset);
    }

    private void SwitchAuthSettings(string serverType,
									string serviceCode,
									string bulidNumber,
									string version, 
									string versionName,
									string bundleIdentifier)
    {
        //PlayerSettings.productName = productName;
        PlayerSettings.applicationIdentifier = bundleIdentifier;
        PlayerSettings.bundleVersion = version;
        //PlayerSettings.Android.bundleVersionCode = int.Parse(releaseName.Remove(0, 1));

        //string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        //if (enableDebug == true && defines.Contains("ENABLE_LOG") == false)
        //    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines + " ENABLE_LOG");
        //else if (enableDebug == false && defines.Contains("ENABLE_LOG") == true)
        //    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines.Replace("ENABLE_LOG", ""));

		string data = File.ReadAllText("Assets/Scripts/2. UI/Auth/System/AuthSettings.cs");
        data = Regex.Replace(data, "public const string ServerType = \"(.*)\";", "public const string ServerType = \"" + serverType + "\";");
        data = Regex.Replace(data, "public const string ServiceCode = \"(.*)\";", "public const string ServiceCode = \"" + serviceCode + "\";");
        data = Regex.Replace(data, "public const string BulidNumber = \"(.*)\";", "public const string BulidNumber = \"" + bulidNumber + "\";");
        data = Regex.Replace(data, "public const string Version = \"(.*)\";", "public const string Version = \"" + version + "\";");
        data = Regex.Replace(data, "public const string VersionName = \"(.*)\";", "public const string VersionName = \"" + versionName + "\";");
        data = Regex.Replace(data, "public const string BundleIdentifier = \"(.*)\";", "public const string BundleIdentifier = \"" + bundleIdentifier + "\";");
		System.IO.File.WriteAllText("Assets/Scripts/2. UI/Auth/System/AuthSettings.cs", data);

		AssetDatabase.Refresh();
		Debug.Log("Done!");
    }

	private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
	{
		// Get the subdirectories for the specified directory.
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);

		if (!dir.Exists)
		{
			throw new DirectoryNotFoundException(
				"Source directory does not exist or could not be found: "
				+ sourceDirName);
		}

		DirectoryInfo[] dirs = dir.GetDirectories();
		// If the destination directory doesn't exist, create it.
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}

		// Get the files in the directory and copy them to the new location.
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			string temppath = Path.Combine(destDirName, file.Name);
			file.CopyTo(temppath, true);
		}

		// If copying subdirectories, copy them and their contents to new location.
		if (copySubDirs)
		{
			foreach (DirectoryInfo subdir in dirs)
			{
				string temppath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath, copySubDirs);
			}
		}
	}

	private void CountrySetting(CountryType type)
	{
		switch (type)
		{
			case CountryType.Korea:
				{
					ReplaceInFile(Application.dataPath + "/Auth/NativeApiCommands/Entermate/AchievementUnlockNACommand.cs",
						"#define PH", "#define KR");

					AssetDatabase.Refresh();
					DirectoryCopy(Application.dataPath + "/Countries/Korea", Application.dataPath, true);
				}
				break;
			case CountryType.Philippines:
				{
					ReplaceInFile(Application.dataPath + "/Auth/NativeApiCommands/Entermate/AchievementUnlockNACommand.cs",
						"#define KR", "#define PH");

					AssetDatabase.Refresh();
					DirectoryCopy(Application.dataPath + "/Countries/Philippines", Application.dataPath, true);
				}
				break;
		}
	}

	private void ReplaceInFile(string filePath, string searchText, string replaceText)
	{
		StreamReader reader = new StreamReader( filePath );
		string content = reader.ReadToEnd();
		reader.Close();

		content = Regex.Replace( content, searchText, replaceText );

		StreamWriter writer = new StreamWriter( filePath );
		writer.Write( content );
		writer.Close();
	}
}

#endif