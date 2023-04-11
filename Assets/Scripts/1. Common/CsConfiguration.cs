using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EnPlayerPrefsKey
{
    CombatRange = 1,
    AutoPortion = 2,
    AutoSkill2 = 3,
    AutoSkill3 = 4,
    AutoSkill4 = 5,
    AutoSkill5 = 6,
    BGM = 7,
    EffectSound = 8,
	UserViewLimit = 9,
	UserViewFilter = 10,
	//HideOtherUser = 9,
	//HideEnemyUser = 10,
	//HideCountryOtherUser = 11,
	//HideCountryEnemyUser = 12,
	BlockFriendInvitations = 13,
    BlockPartyInvitations = 14,
    PushMessage = 15,
    SmoothMode = 16,
    Frame = 17,
    Graphic = 18,
    Effect = 19,
}

public class CsConfiguration
{
	enum EnCharacter { Gaia = 1, Asura, Unknown1, Unknown2 }

	public enum EnServiceMode
	{
		None = 0,
		Gala = 1
	}

	public enum EnServiceCode
	{
		KR = 1,
		PH = 2,
	}

	public enum EnServerType
	{
		Dev = 1,
		Stage = 2,
		RealStage = 3,
		BalancesStage = 4,
		Live = 5
	}

	public enum EnPlatformID 
	{ 
		Android = 1, 
		iOS = 2, 
	}

	public enum EnConnectMode 
	{ 
		UNITY_ONLY = 1, 
		WITH_SDK = 2 
	}

    public static CsConfiguration Instance
	{
		get { return CsSingleton<CsConfiguration>.GetInstance(); }
	}

	const int c_nMoveDirectionCount = 16;
	const float c_flDirectionAngle = 2 * Mathf.PI / c_nMoveDirectionCount;
	const float c_flDirectionAngleHalf = c_flDirectionAngle / 2;

	//---------------------------------------------------------------------------------------------------
	Vector3[] m_avtMoveDirection = new Vector3[c_nMoveDirectionCount];
	Dictionary<string, string> m_dic;	// 클라이언트 텍스트 저장
	string m_strClientTextSavePath = Application.persistentDataPath + "/ClientTexts.json";		// 클라이언트 텍스트 저장 경로 및 파일명.
	string m_strGameMetaDataSavePath = Application.persistentDataPath + "/GameData.json";		// 게임 데이터 저장 경로 및 파일명.
	string m_strPlayerPrefsKeyUserCredentialId = "UserCredentialId";
	string m_strPlayerPrefsKeyUserCredentialSecret = "UserCredentialSecret";
	string m_strPlayerPrefsKeyGuestUserCredentialId = "GuestUserCredentialId";
	string m_strPlayerPrefsKeyGuestUserCredentialSecret = "GuestUserCredentialSecret";
	string m_strPlayerPrefsKeyClientTextVersion = "ClientTextVersion";
	string m_strPlayerPrefsKeyGameMetaDataVersion = "GameMetaDataVersion";
	string m_strPlayerPrefsLanguage = "Language";
	string m_strPlayerPrefsMaintenanceInfoUrl = "MaintenanceInfoUrl";

	string m_strPlayerPrefsKeyInvitationAutoAccept = "InvitationAutoAccept";
	string m_strPlayerPrefsKeyCallAutoAccept = "CallAutoAccept";
    string m_strPlayerPrefsKeyChattingBubble = "ChattingBubble";
    string m_strPlayerPrefsKeyFishingParty = "FishingParty";
    string m_strPlayerPrefsKeyFishingPartyDate = "FishingPartyDate";
    string m_strPlayerPrefsKeyFriendApplicationAutoAccept = "FriendApplicationAutoAccept";
    string m_strPlayerPrefsKeyPartyDungeonTeamInvitation = "PartyDungeonTeamInvitation";

	string m_strPlayerPrefsKeyFirstConnect = "FirstConnect";

    string m_strGateServerApiUrl = "";
	string m_strServerGroupName = "";
	int m_nBuildNo;
	CsSystemSetting m_csSystemSetting;
	int m_nFarmId;
	string m_strClientVersion;
	string m_strAuthServerApiUrl = "";
	
	CsGameServer m_csGameServerSelected = null;
	User m_user;

	EnServiceCode m_enServiceCode = EnServiceCode.KR;
	EnServiceMode m_enServiceMode = EnServiceMode.None;
	EnPlatformID m_enPlatformId = EnPlatformID.Android;
	EnServerType m_enServerType = EnServerType.Dev;

	//---------------------------------------------------------------------------------------------------
	public Dictionary<string, string> Dic
	{
		get { return m_dic; }
		set { m_dic = value; }
	}

	public string ClientTextSavePath
	{
		get { return m_strClientTextSavePath; }
	}

	public string GameMetaDataSavePath
	{
		get { return m_strGameMetaDataSavePath; }
	}

	public string PlayerPrefsKeyUserCredentialId
	{
		get { return m_strPlayerPrefsKeyUserCredentialId; }
	}

	public string PlayerPrefsKeyUserCredentialSecret
	{
		get { return m_strPlayerPrefsKeyUserCredentialSecret; }
	}

	public string PlayerPrefsKeyGuestUserCredentialId
	{
		get { return m_strPlayerPrefsKeyGuestUserCredentialId; }
	}

	public string PlayerPrefsKeyGuestUserCredentialSecret
	{
		get { return m_strPlayerPrefsKeyGuestUserCredentialSecret; }
	}

	public string PlayerPrefsKeyClientTextVersion
	{
		get { return m_strPlayerPrefsKeyClientTextVersion; }
	}

	public string PlayerPrefsKeyGameMetaDataVersion
	{
		get { return m_strPlayerPrefsKeyGameMetaDataVersion; }
	}

	public string PlayerPrefsKeyLanguage
	{
		get { return m_strPlayerPrefsLanguage; }
	}

	public string PlayerPrefsKeyMaintenanceInfoUrl
	{
		get { return m_strPlayerPrefsMaintenanceInfoUrl; }
	}

	public string PlayerPrefsKeyInvitationAutoAccept
	{
		get { return m_strPlayerPrefsKeyInvitationAutoAccept; }
	}

	public string PlayerPrefsKeyCallAutoAccept
	{
		get { return m_strPlayerPrefsKeyCallAutoAccept; }
	}

    public string PlayerPrefsKeyChattingBubble
    {
        get { return m_strPlayerPrefsKeyChattingBubble; }
    }

    public string PlayerPrefsKeyFishingParty
    {
        get { return m_strPlayerPrefsKeyFishingParty; }
    }

    public string PlayerPrefsKeyFishingPartyDate
    {
        get { return m_strPlayerPrefsKeyFishingPartyDate; }
    }

    public string PlayerPrefsKeyFriendApplicationAutoAccept
    {
        get { return m_strPlayerPrefsKeyFriendApplicationAutoAccept; }
    }

    public string PlayerPrefsKeyPartyDungeonTeamInvitation
    {
        get { return m_strPlayerPrefsKeyPartyDungeonTeamInvitation; }
    }

	public string PlayerPrefsKeyFirstConnect
	{
		get { return m_strPlayerPrefsKeyFirstConnect; }
	}

	public EnPlatformID PlatformId
	{
		get { return m_enPlatformId; }
		set { m_enPlatformId = value; }
	}
	public string ClientVersion
	{
		get { return m_strClientVersion; }
		set { m_strClientVersion = value; }
	}
	public int BuildNo
	{
		get { return m_nBuildNo; }
		set { m_nBuildNo = value; }
	}
	public CsSystemSetting SystemSetting
	{
		get { return m_csSystemSetting; }
		set { m_csSystemSetting = value;}
	}
	public int FarmId
	{
		get { return m_nFarmId; }
		set { m_nFarmId = value; }
	}
	public string ServerGroupName
	{
		get { return m_strServerGroupName; }
		set { m_strServerGroupName = value; }
	}

	public CsGameServer GameServerSelected
	{
		get { return m_csGameServerSelected; }
		set { m_csGameServerSelected = value;}
	}

	public User User
	{
		get { return m_user; }
		set { m_user = value; }
	}

	public string AuthServerApiUrl
	{
		get { return m_strAuthServerApiUrl; }
		set { m_strAuthServerApiUrl = value; }
	}

	public string GateServerApiUrl
	{
		get
		{
			if (ConnectMode == EnConnectMode.UNITY_ONLY)
			{
				switch (ServerType)
				{
				case EnServerType.Stage:
					Debug.Log("GateServerApiUrl          EnServerType.Stage       m_enServiceCode = " + m_enServiceCode);
					return "http://gate-devstg.rappelzrift.mobblo.com/Api.ashx";

					//switch (m_enServiceCode)
					//{
					//case EnServiceCode.KR:
					//    return "http://gate-devstg.rappelzrift.mobblo.com/Api.ashx";
					//case EnServiceCode.PH:
					//    return "";
					//default:
					//    return "";
					//}
				case EnServerType.RealStage:
					Debug.Log("GateServerApiUrl          EnServerType.RealStage       m_enServiceCode = " + m_enServiceCode);
					return "http://gate-realstg.rappelzrift.mobblo.com/Api.ashx";
				case EnServerType.BalancesStage:
					Debug.Log("GateServerApiUrl          EnServerType.BalancesStage       m_enServiceCode = " + m_enServiceCode);
					return "http://gate-balancestg.rappelzrift.mobblo.com/Api.ashx";
					//case EnServerType.Live:
					//	switch (m_enServiceCode)
					//	{
					//	case EnServiceCode.KR:
					//		return "";
					//	case EnServiceCode.PH:
					//		return "";
					//	default:
					//		return "";
					//	}
				default: // Dev.
					Debug.Log("GateServerApiUrl       EnServerType.Dev    m_enServiceCode = " + m_enServiceCode);
					return "http://127.0.0.1:8080/Api.ashx"; 
				}
			}
			else
			{
				return m_strGateServerApiUrl;
			}
		}

		set
		{
			m_strGateServerApiUrl = value;
		}
	}

	public EnServiceCode ServiceCode
	{
		get
		{
			return m_enServiceCode;
		}
		set
		{
			m_enServiceCode = value;
		}
	}

	public EnServiceMode ServiceMode
	{
		get
		{
			if (ConnectMode == EnConnectMode.UNITY_ONLY)
			{
				return EnServiceMode.None;
			}
			else
			{
				return m_enServiceMode;
			}
		}
		set
		{
			m_enServiceMode = value;
		}
	}

	public EnServerType ServerType
	{
		get { return m_enServerType; }
	}

	public EnConnectMode ConnectMode
	{
		get
		{
			//return EnConnectMode.UNITY_ONLY;
#if UNITY_EDITOR
			return EnConnectMode.UNITY_ONLY;
#else
            return EnConnectMode.WITH_SDK;
#endif
		}
	}

	public string AssetBundleVersion
	{
		get
		{
			if (m_enServerType == EnServerType.Dev)
			{
				return "dev/";
			}
			//else if (m_enServerType == EnServerType.Stage)
			//{
			//    return
			//}
			else
			{
				switch (m_enServiceCode)
				{
					case EnServiceCode.KR:
						return "";
					case EnServiceCode.PH:
						return "";
					default:
						return "dev/";
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsConfiguration()
	{
		for (int i = 0; i < c_nMoveDirectionCount; i++)
		{
			Vector3 vt = new Vector3(Mathf.Cos(i * c_flDirectionAngle), 0, Mathf.Sin(i * c_flDirectionAngle));
			vt.Normalize();
			m_avtMoveDirection[i] = vt;
		}

		m_dic = new Dictionary<string, string>();

        foreach (EnPlayerPrefsKey enumItem in Enum.GetValues(typeof(EnPlayerPrefsKey)))
        {
            AddSettingKey(enumItem);
        }

        m_enServerType = (EnServerType)Int32.Parse(AuthSettings.ServerType); // 0 Dev.
		m_enServiceCode = (EnServiceCode)Int32.Parse(AuthSettings.ServiceCode); // 1 KR.
		m_nBuildNo = Int32.Parse(AuthSettings.BulidNumber);
		m_strClientVersion = AuthSettings.Version;
	}

	//---------------------------------------------------------------------------------------------------
	public Vector3 GetMoveDirection(int nIndex)
	{
		return m_avtMoveDirection[nIndex];
	}

	//---------------------------------------------------------------------------------------------------
	public Vector3 GetMoveDirection(float flRad)
	{
		return m_avtMoveDirection[GetMoveDirectionIndex(flRad)];
	}

	//---------------------------------------------------------------------------------------------------
	public static int GetMoveDirectionIndex(float flRad)
	{
		if (flRad < 0)
		{
			flRad = (flRad % (2 * Mathf.PI)) + (2 * Mathf.PI);
		}
		
		float flRad2 = (flRad + c_flDirectionAngleHalf) % (2 * Mathf.PI);
		return (int)(flRad2 / c_flDirectionAngle);
	}

	//---------------------------------------------------------------------------------------------------
	public string GetCharacterName(int nCharNo)
	{
		return ((EnCharacter)nCharNo).ToString();
	}

	//---------------------------------------------------------------------------------------------------
	public string GetString(string strKey)
	{
		if (m_dic.ContainsKey(strKey))
		{
			return m_dic[strKey];
		}
		else
		{
			return strKey;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFirebaseLogEvent(string strEventName, string strParameter = "")
	{
		//Debug.Log("***************************************" + strEventName);
		CsCommandEventManager.Instance.SendLogEvent(strEventName, strParameter);
	}

	#region Setting

	//---------------------------------------------------------------------------------------------------
	void AddSettingKey(EnPlayerPrefsKey enSetting)
    {
        if (!PlayerPrefs.HasKey(enSetting.ToString()))
        {
            int nDefault;

            switch (enSetting)
            {
                case EnPlayerPrefsKey.AutoSkill5:
                case EnPlayerPrefsKey.BlockFriendInvitations:
                case EnPlayerPrefsKey.BlockPartyInvitations:
                case EnPlayerPrefsKey.PushMessage:
                    nDefault = 0;
                    break;
				case EnPlayerPrefsKey.UserViewLimit:
                    nDefault = 2;
                    break;
				case EnPlayerPrefsKey.UserViewFilter:
                    nDefault = 2;
                    break;
				case EnPlayerPrefsKey.CombatRange:
                case EnPlayerPrefsKey.Effect:
                    nDefault = 2;
                    break;
                default:
                    nDefault = 1;
                    break;
            }

            PlayerPrefs.SetInt(enSetting.ToString(), nDefault);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public int GetSettingKey(EnPlayerPrefsKey enSetting)
    {
        if (PlayerPrefs.HasKey(enSetting.ToString()))
        {
            return PlayerPrefs.GetInt(enSetting.ToString());
        }
        else
        {
            int nDefault;

            switch (enSetting)
            {
                case EnPlayerPrefsKey.AutoSkill5:
                case EnPlayerPrefsKey.BlockFriendInvitations:
                case EnPlayerPrefsKey.BlockPartyInvitations:
                case EnPlayerPrefsKey.PushMessage:
                    nDefault = 0;
                    break;
				case EnPlayerPrefsKey.UserViewLimit:
                    nDefault = 2;
                    break;
				case EnPlayerPrefsKey.UserViewFilter:
                    nDefault = 2;
                    break;
				case EnPlayerPrefsKey.Effect:
                case EnPlayerPrefsKey.CombatRange:
                    nDefault = 2;
                    break;
                default:
                    nDefault = 1;
                    break;
            }

            PlayerPrefs.SetInt(enSetting.ToString(), nDefault);
            return nDefault;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SetPlayerPrefsKey(EnPlayerPrefsKey enSetting, int nValue)
    {
        PlayerPrefs.SetInt(enSetting.ToString(), nValue);
        CsGameEventToIngame.Instance.OnEventPlayerPrefsKeySet(enSetting, nValue);
    }

    #endregion Setting

    #region Tutorial

    //---------------------------------------------------------------------------------------------------
    public  bool GetTutorialKey(EnTutorialType enTutorialType)
    {
        string strKey = CsGameData.Instance.MyHeroInfo.HeroId + enTutorialType.ToString();

        //1 일때 튜토리얼 가능
        if (PlayerPrefs.HasKey(strKey))
        {
            return PlayerPrefs.GetInt(strKey) == 1 ? true : false;
        }
        else
        {
            PlayerPrefs.SetInt(strKey, 1);
            return true;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void TutorialComplete(EnTutorialType enTutorialType)
    {
        string strKey = CsGameData.Instance.MyHeroInfo.HeroId + enTutorialType.ToString();
        PlayerPrefs.SetInt(strKey, 0);
    }

	#endregion Tutorial


}

