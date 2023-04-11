using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-05)
//---------------------------------------------------------------------------------------------------

public class CsGuildSupplySupportQuestPlay
{
	long m_lCartInstanceId;
	int m_nCartContinentId;
	Vector3 m_v3CartPosition;
	float m_flCartRotationY;
	float m_flRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public long CartInstanceId
	{
		get { return m_lCartInstanceId; }
	}

	public int CartContinentId
	{
		get { return m_nCartContinentId; }
	}

	public Vector3 CartPosition
	{
		get { return m_v3CartPosition; }
	}

	public float CartRotationY
	{
		get { return m_flCartRotationY; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSupplySupportQuestPlay(PDGuildSupplySupportQuestPlay guildSupplySupportQuestPlay)
	{
		m_lCartInstanceId = guildSupplySupportQuestPlay.cartInstanceId;
		m_nCartContinentId = guildSupplySupportQuestPlay.cartContinentId;
		m_v3CartPosition = CsRplzSession.Translate(guildSupplySupportQuestPlay.cartPosition);
		m_flCartRotationY = guildSupplySupportQuestPlay.cartRotationY;
		m_flRemainingTime = guildSupplySupportQuestPlay.remainingTime;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSupplySupportQuestPlay(long lCartInstanceId, int nCartContinentId, Vector3 v3CartPosition, float flCartRotationY, float flRemainingTime)
	{
		m_lCartInstanceId = lCartInstanceId;
		m_nCartContinentId = nCartContinentId;
		m_v3CartPosition = v3CartPosition;
		m_flCartRotationY = flCartRotationY;
		m_flRemainingTime = flRemainingTime;
	}
}
