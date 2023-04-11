using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsHeroMainQuest
{
	CsMainQuest m_csMainQuest;
	int m_nProgressCount = 0;
	bool m_bCompleted = false;
	bool m_bAccepted = false;
	int m_nCartContinentId;
	long m_lCartInstanceId;
	Vector3 m_vtCartPosition;
	float m_flCartRotationY;

	//---------------------------------------------------------------------------------------------------
	public CsMainQuest MainQuest
	{
		get { return m_csMainQuest; }
		set { m_csMainQuest = value; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	public bool Completed
	{
		get { return m_bCompleted; }
		set { m_bCompleted = value; }
	}

	public bool Accepted
	{
		get { return m_bAccepted; }
		set { m_bAccepted = value; }
	}

	public int CartContinentId
	{
		get { return m_nCartContinentId; }
		set { m_nCartContinentId = value; }
	}

	public long CartInstanceId
	{
		get { return m_lCartInstanceId; }
		set { m_lCartInstanceId = value; }
	}

	public Vector3 CartPosition
	{
		get { return m_vtCartPosition; }
		set { m_vtCartPosition = value; }
	}

	public float CartRotationY
	{
		get { return m_flCartRotationY; }
		set { m_flCartRotationY = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainQuest(CsMainQuest csMainQuest)
	{
		m_csMainQuest = csMainQuest;
		m_nProgressCount = 0;
		m_bCompleted = false;
		m_bAccepted = false;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainQuest(PDHeroMainQuest heroMainQuest)
	{
		m_csMainQuest = CsGameData.Instance.GetMainQuest(heroMainQuest.no);
		m_nProgressCount = heroMainQuest.progressCount;
		m_bCompleted = heroMainQuest.completed;
		m_nCartContinentId = heroMainQuest.cartContinentId;
		m_lCartInstanceId = heroMainQuest.cartInstanceId;
		m_vtCartPosition = CsRplzSession.Translate(heroMainQuest.cartPosition);
		m_flCartRotationY = heroMainQuest.cartRotationY;
	}
}
