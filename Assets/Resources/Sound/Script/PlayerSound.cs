using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    AudioSource m_Audio;

    public AudioClip m_acRunSound;

    public AudioClip m_acAttack01Sound;
	public AudioClip m_acAttack02Sound;
	public AudioClip m_acAttack03Sound;
	public AudioClip m_acISkill01Sound;
	public AudioClip m_acISkill02Sound;
	public AudioClip m_acISkill03Sound;
	public AudioClip m_acISkill04Sound;
	public AudioClip m_acISkill05Sound;

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
    }

    //---------------------------------------------------------------------------------------------------
    public void RunSound()
    {
        m_Audio.PlayOneShot(m_acRunSound);
    }

    //---------------------------------------------------------------------------------------------------
    public void Attack01Sound()
    {
		m_Audio.PlayOneShot(m_acAttack01Sound);
    }

	//---------------------------------------------------------------------------------------------------
	public void Attack02Sound()
	{
		m_Audio.PlayOneShot(m_acAttack02Sound);
	}

	//---------------------------------------------------------------------------------------------------
	public void Attack03Sound()
	{
		m_Audio.PlayOneShot(m_acAttack03Sound);
	}

	//---------------------------------------------------------------------------------------------------
	public void Skill01Sound()
	{
		m_Audio.PlayOneShot(m_acISkill01Sound);
	}

	//---------------------------------------------------------------------------------------------------
	public void Skill02Sound()
	{
		m_Audio.PlayOneShot(m_acISkill02Sound);
	}

	//---------------------------------------------------------------------------------------------------
	public void Skill03Sound()
	{
		m_Audio.PlayOneShot(m_acISkill03Sound);
	}

	//---------------------------------------------------------------------------------------------------
	public void Skill04Sound()
	{
		m_Audio.PlayOneShot(m_acISkill04Sound);
	}

	//---------------------------------------------------------------------------------------------------
	public void Skill05Sound()
	{
		m_Audio.PlayOneShot(m_acISkill05Sound);
	}
}
