using UnityEngine;
using System.Collections;

public class AuthSettingsPreset : ScriptableObject
{
    [SerializeField]
    private string _serverType = "";
    public string ServerType { get { return _serverType; } }
    [SerializeField]
    private string _serviceCode = "";
    public string ServiceCode { get { return _serviceCode; } }
    [SerializeField]
    private string _bulidNumber = "";
    public string BulidNumber { get { return _bulidNumber; } }
    [SerializeField]
    private string _version = "";
    public string Version { get { return _version; } }
    [SerializeField]
    private string _versionName = "";
    public string VersionName { get { return _versionName; } }
    [SerializeField]
    private string _bundleIdentifier = "";
    public string BundleIdentifier { get { return _bundleIdentifier; } }
}
