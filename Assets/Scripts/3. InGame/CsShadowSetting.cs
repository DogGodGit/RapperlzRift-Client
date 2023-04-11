using UnityEngine;
using UnityEngine.SceneManagement;

public class CsShadowSetting : MonoBehaviour 
{
    float m_flPrevShadowDistance;
    //Light m_light;

    //---------------------------------------------------------------------------------------------------
	void Start () 
    {
        //m_light = GetComponent<Light>();
        m_flPrevShadowDistance = QualitySettings.shadowDistance;
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;
        
		if (sceneName == "IntroLobby")
		{
			IntroShadow ();
		}
		else if (sceneName != "IntroLobby")
		{
			DefaultShadow ();
		}

    }

    //---------------------------------------------------------------------------------------------------
	void Update () 
    {
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;
		if (sceneName == "IntroLobby")
		{
			IntroShadow ();
		}
		else if (sceneName != "IntroLobby")
		{
			DefaultShadow ();
		}
	}

    //---------------------------------------------------------------------------------------------------
    void IntroShadow ()
	{
		QualitySettings.shadowDistance = 240f;
	}

    //---------------------------------------------------------------------------------------------------
    void DefaultShadow ()
	{
		QualitySettings.shadowDistance = m_flPrevShadowDistance;
	}
}
