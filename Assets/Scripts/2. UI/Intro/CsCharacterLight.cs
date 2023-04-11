using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsCharacterLight : MonoBehaviour
{
    Transform m_trfGaiaLights;
    Transform m_trfAsuraLights;
    Transform m_trfDevaLights;
    Transform m_trfWitchLights;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        gameObject.SetActive(false);
        Transform m_trfLightList = GameObject.Find("LightList").transform;

        Transform m_trfLight = m_trfLightList.Find("Light").transform;
        m_trfLight.gameObject.SetActive(true);

        m_trfGaiaLights = m_trfLight.Find("Gaia_LightSetting");
        m_trfAsuraLights = m_trfLight.Find("Asura_LightSetting");
        m_trfDevaLights = m_trfLight.Find("Deva_LightSetting");
        m_trfWitchLights = m_trfLight.Find("Witch_LightSetting");
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        //Debug.Log("CsCharacterLight.OnEnable()");
        m_trfGaiaLights.gameObject.SetActive(false);
        m_trfDevaLights.gameObject.SetActive(false);
        m_trfAsuraLights.gameObject.SetActive(false);
        m_trfWitchLights.gameObject.SetActive(false);

        if (transform.parent.name == "Character1")
        {
            m_trfGaiaLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "Character2")
        {
            m_trfAsuraLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "Character3")
        {
            m_trfDevaLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "Character4")
        {
            m_trfWitchLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "CustomCharacter1")
        {
            m_trfGaiaLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "CustomCharacter2")
        {
            m_trfAsuraLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "CustomCharacter3")
        {
            m_trfDevaLights.gameObject.SetActive(true);
        }
        else if (transform.parent.name == "CustomCharacter4")
        {
            m_trfWitchLights.gameObject.SetActive(true);
        }
    }
}