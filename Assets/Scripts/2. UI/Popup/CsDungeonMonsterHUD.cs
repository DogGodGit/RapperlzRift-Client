using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsDungeonMonsterHUD : MonoBehaviour
{
    Transform m_trSliderMonsterHp;

    float m_flLifeTime;
    IEnumerator m_iEnumerator;
	bool m_bViewSliderMonsterHp = false;

	//---------------------------------------------------------------------------------------------------
	public void Init(int nHp)
	{
		m_bViewSliderMonsterHp = true;
		ViewSliderMonsterHp(nHp);
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
    {
        CsRplzSession.Instance.EventEvtMonsterHit += OnEventEvtMonsterHit;
        m_trSliderMonsterHp = transform.Find("SliderMonsterHp");
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsRplzSession.Instance.EventEvtMonsterHit -= OnEventEvtMonsterHit;
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterHit(ClientCommon.SEBMonsterHitEventBody eventBody)
	{
		if (transform.name == eventBody.monsterInstanceId.ToString())
		{
			if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon || m_bViewSliderMonsterHp)
			{
				ViewSliderMonsterHp(eventBody.hitResult.hp);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ViewSliderMonsterHp(int nHp)
	{
		if (m_iEnumerator != null)
		{
			StopCoroutine(m_iEnumerator);
			m_iEnumerator = null;
		}

		m_iEnumerator = UpdateMonsterHUD(nHp);
		StartCoroutine(m_iEnumerator);
	}

    //---------------------------------------------------------------------------------------------------
    IEnumerator UpdateMonsterHUD(int nHp)
    {
        m_trSliderMonsterHp.gameObject.SetActive(true);
		if (nHp == 0)
		{
			yield return new WaitForSeconds(1.0f);
		}
		else
		{
			yield return new WaitForSeconds(3.0f);
		}
        m_trSliderMonsterHp.gameObject.SetActive(false);
    }
}