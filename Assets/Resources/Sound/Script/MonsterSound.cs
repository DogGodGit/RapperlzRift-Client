using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MonsterSound : MonoBehaviour 
{
    AudioSource m_Audio;

    public AudioClip m_acIdleSound;
    public AudioClip m_acAttackSound;
	public AudioClip m_acSkillSound;

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
    }

    //---------------------------------------------------------------------------------------------------
    public void IdleSound()
    {
		m_Audio.PlayOneShot(m_acIdleSound);
    }

    //---------------------------------------------------------------------------------------------------
    public void AttackSound()
    {
        m_Audio.PlayOneShot(m_acAttackSound);
    }

    //---------------------------------------------------------------------------------------------------
    public void SkillSound()
    {
		m_Audio.PlayOneShot(m_acSkillSound);
    }
}

