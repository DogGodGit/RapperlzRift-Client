using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnOptionEffect {No = 0, My, All};
public enum EnPlayerAutoMode { Manual = 0, Auto }
public enum EnUserViewFilter { Other = 0, Enemy, All }

public class CsIngameData
{
	public static CsIngameData Instance
	{
		get { return CsSingleton<CsIngameData>.GetInstance(); }
	}

	CsScene m_csScene;
	IIngameManagement m_itfIngameManagement;
	Transform m_trTarget;
	float m_flHeroCenter;

	// 설정값 세팅.
	int m_nGraphic;
	bool m_bBGM;				
	bool m_bEffactSound;
	int m_nEffect;

	int m_nCombatRange;
	bool m_bAutoSkill2;
	bool m_bAutoSkill3;
	bool m_bAutoSkill4;
	bool m_bAutoSkill5;

	bool m_bOtherHeroView = true;
	bool m_bMyHeroDead = false;
	bool m_bReturnScroll = false;
	bool m_bActiveScene = false;
	bool m_bDirecting = false;

	int m_nAutoPlayKey;		 // Hero Auto 상태값.
	int m_nAutoPlaySub;
	int m_nAutoPlayNpcId;
	int m_nAutoPortalId;

	int m_nDungeonAreaNo;
	int m_nDungeonDifficulty;

	CsTameMonster m_CsTameMonster;
	CsInGameCamera m_csInGameCamera;
	CsInGameDungeonCamera m_csInGameDungeonCamera;

	EnCameraMode m_enCameraMode = EnCameraMode.CameraAuto;
	EnBattleMode m_enBattleMode = EnBattleMode.None;
	EnPlayerAutoMode m_enPlayerAutoMode = EnPlayerAutoMode.Manual;
	EnUserViewFilter m_enUserViewFilter = EnUserViewFilter.All;

	Dictionary<string, AudioClip> m_dicObjectSound = new Dictionary<string, AudioClip>();
	Dictionary<string, GameObject> m_dicOtherPlayer = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> m_dicEquipWeapon = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> m_dicEquipSkin = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> m_dicEquipFace = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> m_dicEquipHair = new Dictionary<string, GameObject>();

	//---------------------------------------------------------------------------------------------
	public CsScene Scene { get { return m_csScene; } set { m_csScene = value; } }
	public IIngameManagement IngameManagement { get { return m_itfIngameManagement; } set { m_itfIngameManagement = value; } }

	public Transform TargetTransform { get { return m_trTarget; } set { m_trTarget = value; } }
	public CsInGameCamera InGameCamera { get { return m_csInGameCamera; } set { m_csInGameCamera = value; } }
	public CsInGameDungeonCamera InGameDungeonCamera { get { return m_csInGameDungeonCamera; } set { m_csInGameDungeonCamera = value; } }
	public EnCameraMode CameraMode { get { return m_enCameraMode; } set { m_enCameraMode = value; } }
	public EnBattleMode AutoBattleMode { get { return m_enBattleMode; } set { m_enBattleMode = value; } }
	public EnPlayerAutoMode PlayerAutoMode { get { return m_enPlayerAutoMode; } set { m_enPlayerAutoMode = value; } }

	public CsTameMonster TameMonster { get { return m_CsTameMonster; } set { m_CsTameMonster = value; } }
	public float HeroCenter { get { return m_flHeroCenter; } set { m_flHeroCenter = m_csInGameCamera.HeightPlayerCenter = value; } }

	public int Graphic { get { return m_nGraphic; } set { m_nGraphic = value; } }
	public bool BGM { get { return m_bBGM; } set { m_bBGM = value; } }
	public bool EffectSound { get { return m_bEffactSound; } set { m_bEffactSound = value; } }
	public int Effect { get { return m_nEffect; } set { m_nEffect = value; } }
	public EnOptionEffect EffectEnum { get { return (EnOptionEffect)m_nEffect; } set { m_nEffect = (int)value; } }
	public EnUserViewFilter UserViewFilter { get { return m_enUserViewFilter; } set { m_enUserViewFilter = value; } }

	public int CombatRange { get { return m_nCombatRange; } set { m_nCombatRange = value; } }
	public bool AutoSkill2 { get { return m_bAutoSkill2; } set { m_bAutoSkill2 = value; } }
	public bool AutoSkill3 { get { return m_bAutoSkill3; } set { m_bAutoSkill3 = value; } }
	public bool AutoSkill4 { get { return m_bAutoSkill4; } set { m_bAutoSkill4 = value; } }
	public bool AutoSkill5 { get { return m_bAutoSkill5; } set { m_bAutoSkill5 = value; } }

	public bool OtherHeroView { get { return m_bOtherHeroView; } set { m_bOtherHeroView = value; } }
	public bool ReturnScroll { get { return m_bReturnScroll; } set { m_bReturnScroll = value; } }
	public bool MyHeroDead { get { return m_bMyHeroDead; } set { m_bMyHeroDead = value; } }
	public bool ActiveScene { get { return m_bActiveScene; } set { m_bActiveScene = value; } }
	public bool Directing { get { return m_bDirecting; } set { m_bDirecting = value; } }

	public int AutoPlayKey { get { return m_nAutoPlayKey; } set { m_nAutoPlayKey = value; } }
	public int AutoPlaySub { get { return m_nAutoPlaySub; } set { m_nAutoPlaySub = value; } }
	public int AutoPlayNpcId { get { return m_nAutoPlayNpcId; } set { m_nAutoPlayNpcId = value; } }
	public int AutoPortalId { get { return m_nAutoPortalId; } set { m_nAutoPortalId = value; } }

	public int DungeonAreaNo { get { return m_nDungeonAreaNo; } set { m_nDungeonAreaNo = value; } }
	public int DungeonDifficulty { get { return m_nDungeonDifficulty; } set { m_nDungeonDifficulty = value; } }

	public Dictionary<string, AudioClip> ObjectSound { get { return m_dicObjectSound; } set { m_dicObjectSound = value; } }
	public Dictionary<string, GameObject> OtherPlayer { get { return m_dicOtherPlayer; } set { m_dicOtherPlayer = value; } }
	public Dictionary<string, GameObject> Weapon { get { return m_dicEquipWeapon; } set { m_dicEquipWeapon = value; } }
	public Dictionary<string, GameObject> Armor { get { return m_dicEquipSkin; } set { m_dicEquipSkin = value; } }
	public Dictionary<string, GameObject> Face { get { return m_dicEquipFace; } set { m_dicEquipFace = value; } }
	public Dictionary<string, GameObject> Hair { get { return m_dicEquipHair; } set { m_dicEquipHair = value; } }


	public T LoadAsset<T>(string strPath) where T : Object
	{
		if (string.IsNullOrEmpty(strPath) == false)
		{
			return Resources.Load<T>(strPath);
		}
		return null;
	}

	public ResourceRequest LoadAssetAsync<T>(string strPath) where T : Object
	{
		if (string.IsNullOrEmpty(strPath) == false)
		{
			return Resources.LoadAsync<T>(strPath);
		}
		return null;
	}

	public T LoadAssetNation<T>(string strImageName) where T : Object
	{
		if (string.IsNullOrEmpty(strImageName) == false)
		{
			return Resources.Load<T>("GUINation/" + CsConfiguration.Instance.ServiceCode.ToString() + "/" + strImageName);
		}
		return null;
	}

	//public Dictionary<string, string> SystemSettingList { get; private set; }
	//public List<string> VersionList { get; set; }
	//public Dictionary<string, AssetBundle> AssetBundlesDic = new Dictionary<string, AssetBundle>();

	//public AssetBundle GetAssetBundle(string sNm)
	//{
	//	for (int i = 0; i < VersionList.Count; i++)
	//	{
	//		if (VersionList[i].IndexOf(sNm) > -1)
	//		{
	//			string[] sArr = VersionList[i].Split('`');
	//			string sUrl = SystemSettingList["assetBundleUrl"] + sArr[0] + CsConfiguration.Instance.AssetBundleVersion + sArr[1];
	//			int nVer = int.Parse(sArr[2]);

	//			string keyName = sUrl + nVer;
	//			AssetBundleRef abRef;
	//			if (AssetBundleRefList.TryGetValue(keyName, out abRef))
	//			{
	//				return abRef.assetBundle;
	//			}
	//			else
	//			{
	//				return null;
	//			}
	//		}
	//	}

	//	return null;
	//}

	//public T LoadBundleAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object
	//{
	//	if (string.IsNullOrEmpty(bundleName) || string.IsNullOrEmpty(assetName))
	//		return null;

	//	if (AssetBundlesDic.ContainsKey(bundleName) == false)
	//		AssetBundlesDic.Add(bundleName, GetAssetBundle(bundleName));

	//	if (AssetBundlesDic.ContainsKey(bundleName) == false)
	//	{
	//		Debug.Log("Cannot Find Bundle :: Bundle Name :" + bundleName);
	//		return null;
	//	}

	//	if (AssetBundlesDic[bundleName].Contains(assetName) == false)
	//	{
	//		Debug.Log("Cannot Find Assset :: Bundle Name : " + bundleName + " :: Asset Name : " + assetName);
	//		return null;
	//	}

	//	return AssetBundlesDic[bundleName].LoadAsset<T>(assetName);
	//}

	//public AssetBundleRequest LoadBundleAssetAsync<T>(string bundleName, string assetName) where T : UnityEngine.Object
	//{
	//	if (string.IsNullOrEmpty(bundleName) || string.IsNullOrEmpty(assetName))
	//		return null;

	//	if (AssetBundlesDic.ContainsKey(bundleName) == false)
	//	{
	//		AssetBundlesDic.Add(bundleName, GetAssetBundle(bundleName));
	//	}

	//	if (AssetBundlesDic.ContainsKey(bundleName) == false)
	//	{
	//		Debug.Log("Cannot Find Bundle :: Bundle Name :" + bundleName);
	//		return null;
	//	}

	//	if (AssetBundlesDic[bundleName].Contains(assetName) == false)
	//	{
	//		Debug.Log("Cannot Find Assset :: Bundle Name : " + bundleName + " :: Asset Name : " + assetName);
	//		return null;
	//	}

	//	AssetBundleRequest aeq = AssetBundlesDic[bundleName].LoadAssetAsync<T>(assetName);
	//	return aeq;
	//}
}
